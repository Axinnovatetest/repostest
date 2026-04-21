using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan
{
	public class DeleteHandler: IHandle<List<DeleteItemModel>, ResponseModel<object>>
	{
		private List<DeleteItemModel> data { get; set; }
		private Identity.Models.UserModel user { get; set; }

		public DeleteHandler(List<DeleteItemModel> data,
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


					if(this.data == null || this.data.Count <= 0)
						return ResponseModel<object>.FailureResponse("Invalid data");

					var createUserId = CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data[0].CountryId,
						this.data[0].HallId, this.data[0].Year)?.CreationUserId;
					var lastValidationEntity = CapacityPlanValidationLogAccess.GetLastByYearCountryHall(data[0].Year, data[0].CountryId, data[0].HallId);
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

					if(!Helpers.Config.CanEdit(this.data[0].Year, this.data[0].WeekFrom, Enums.Main.CapacityType.CapacityPlan))
						return ResponseModel<object>.FailureResponse($"Cannot delete plan earlier than KW {Helpers.Config.GetCapacityLastEditableWeek()}");

					// -
					var updated = false;
					foreach(var item in this.data)
					{
						var capacityPlanEntities = CapacityPlanAccess.Get(item.CountryId,
							item.Year,
							item.OperationId,
							item.HallId,
							item.DepartementId,
							item.WorkAreaId,
							item.WorkStationId,
							item.WeekFrom,
							item.WeekUntil);

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
							item.Year,
							item.CountryId,
							item.WeekFrom,
							item.WeekUntil,
							item.OperationId,
							item.HallId,
							item.DepartementId,
							item.WorkAreaId,
							item.WorkStationId
							);

						// -
						updated = updated || (capacityPlanEntities != null && capacityPlanEntities.Count > 0);
					}

					// > Update Capacity data
					if(updated)
					{
						return Capacity.UpdateCapacityHandler.Perform(user, new Models.Capacity.UpdateCapacityModel()
						{
							CountryId = this.data[0].CountryId,
							HallId = this.data[0].HallId,
							Year = this.data[0].Year,
							FirstWeekNumber = this.data[0].WeekFrom,
							LastWeekNumber = this.data[0].WeekUntil,
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
