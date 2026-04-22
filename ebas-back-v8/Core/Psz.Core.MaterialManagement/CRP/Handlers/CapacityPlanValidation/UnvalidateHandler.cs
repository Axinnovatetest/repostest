using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation
{
	public class UnvalidateHandler: IHandle<Models.CapacityPlanValidation.ValidateModel, ResponseModel<int>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }
		private Models.CapacityPlanValidation.ValidateModel data { get; set; }

		public UnvalidateHandler(Identity.Models.UserModel user, Models.CapacityPlanValidation.ValidateModel data)
		{
			this.user = user;
			this.data = data;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CapacityPlanLock)
			{
				try
				{
					Validate();

					//-
					var planEntities = Infrastructure.Data.Access.Tables.MTM.CapacityPlanAccess.GetBy_CountryId_HallId_Year(data.CountryId, data.HallId, data.Year);
					var countryEntity = CountryAccess.Get(data.CountryId);
					var HallEntity = HallAccess.Get(data.HallId);

					CapacityPlanValidationLogAccess.Insert(new CapacityPlanValidationLogEntity
					{
						CountryId = countryEntity.Id,
						CountryName = countryEntity.Name,
						HallId = HallEntity.Id,
						HallName = HallEntity.Name,
						ValidationLevel = data.Level,
						ValidationStatus = (int)Enums.CapacityPlan.ValidationStatuses.Unvalidated,
						ValidationStatusName = Enums.CapacityPlan.ValidationStatuses.Unvalidated.GetDescription(),
						ValidationTime = DateTime.Now,
						ValidationUserId = user.Id,
						Year = data.Year,
						ValidationReason = data.Reason
					});

					// - Delete from Editable Capacities -
					int firstWeek = 1;
					if(this.data.Year == DateTime.Today.Year)
					{
						firstWeek = Helpers.Config.GetCapacityLastEditableWeek();
					}
					CapacityAccess.Update_IsArchived_ArchiveTime_ArchiveUserId(data.Year, data.CountryId, data.HallId, true, DateTime.Now, user.Id, firstWeek);

					#region >>> Email <<<
					var planOwner = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
									CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data.CountryId, this.data.HallId, this.data.Year)?.CreationUserId ?? -1);
					var content = $"<br/><span style='font-size:1.15em;'><strong>{user.Name?.ToUpper()}</strong> has just unvalidated the Capacity Plan <strong>[{countryEntity.Name} - {HallEntity.Name} - {data.Year}]</strong>.<br/>Plan edition is now enabled once more."
						+ $"<br/><strong>Reject Reason:</strong> <br/>{data.Reason}"
					+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}#/crp/planning'>here</a>";
					Helpers.Email.Send(user, "Plan UnValidation", content, new List<string> { planOwner.Email });
					#endregion

					CapacityPlanAccess.UpdateVersion(data.Year, data.CountryId, data.HallId, this.user.Id);
					return ResponseModel<int>.SuccessResponse(CapacityPlanValidationAccess.Delete(data.Year, data.CountryId, data.HallId));
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
			{
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
			}

			if(CountryAccess.Get(data.CountryId) == null)
				return ResponseModel<int>.FailureResponse("Country not found.");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id) == null)
				return ResponseModel<int>.FailureResponse("User not found.");

			var planEntities = Infrastructure.Data.Access.Tables.MTM.CapacityPlanAccess.GetBy_CountryId_HallId_Year(data.CountryId, data.HallId, data.Year);
			if(planEntities == null || planEntities.Count <= 0)
				return ResponseModel<int>.FailureResponse("Plan not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
