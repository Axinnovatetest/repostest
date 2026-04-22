using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan
{
	public class DeleteItemHandler: IHandle<DeleteItemModel, ResponseModel<object>>
	{
		private DeleteItemModel data { get; set; }
		private Identity.Models.UserModel user { get; set; }

		public DeleteItemHandler(DeleteItemModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<object> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					if(this.data == null)
						return ResponseModel<object>.FailureResponse("Invalid data");

					var createUserId = CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data.CountryId,
						this.data.HallId, this.data.Year)?.CreationUserId;
					var lastValidationEntity = CapacityPlanValidationLogAccess.GetLastByYearCountryHall(data.Year, data.CountryId, data.HallId);
					if(lastValidationEntity == null)
					{
						if(createUserId != null && this.user.Id != createUserId)
						{
							return ResponseModel<object>.FailureResponse("Only owner can delete plan.");
						}
					}
					else
					{
						if((int)lastValidationEntity.ValidationStatus != (int)Enums.CapacityPlan.ValidationStatuses.Rejected
							&& (int)lastValidationEntity.ValidationStatus != (int)Enums.CapacityPlan.ValidationStatuses.Unvalidated)
						{
							return ResponseModel<object>.FailureResponse("Cannot delete plan while validation in progress.");
						}
					}

					if(!Helpers.Config.CanEdit(this.data.Year, this.data.WeekFrom, Enums.Main.CapacityType.CapacityPlan))
						return ResponseModel<object>.FailureResponse($"Cannot delete plan earlier than KW {Helpers.Config.GetCapacityLastEditableWeek()}");

					// -
					var capacityPlanEntities = CapacityPlanAccess.Get(this.data.CountryId,
						this.data.Year,
						this.data.OperationId,
						this.data.HallId,
						this.data.DepartementId,
						this.data.WorkAreaId,
						this.data.WorkStationId,
						this.data.WeekFrom,
						this.data.WeekUntil);

					//CapacityPlanAccess.SoftDelete(capacityPlanEntities.Select(x => x.Id)?.ToList(),
					//    true,
					//    DateTime.Now,
					//    user.Id);
					// - Archive in CapacityPlans
					CapacityPlanAccess.Update_IsArchived_ArchiveTime_ArchiveUserId(capacityPlanEntities.Select(x => x.Id)?.ToList(),
						true,
						DateTime.Now,
						user.Id);
					// - Delete from Capacities
					CapacityAccess.Delete(
						this.data.Year,
						this.data.CountryId,
						this.data.WeekFrom,
						this.data.WeekUntil,
						this.data.OperationId,
						this.data.HallId,
						this.data.DepartementId,
						this.data.WorkAreaId,
						this.data.WorkStationId
						);

					// > Update Capacity data
					if(capacityPlanEntities != null && capacityPlanEntities.Count > 0)
					{
						return Capacity.UpdateCapacityHandler.Perform(user, new Models.Capacity.UpdateCapacityModel()
						{
							CountryId = this.data.CountryId,
							HallId = this.data.HallId,
							Year = this.data.Year,
							FirstWeekNumber = this.data.WeekFrom,
							LastWeekNumber = this.data.WeekUntil,
						});
					}

					return ResponseModel<object>.SuccessResponse(null);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<object> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
