using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation
{
	public class GetHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.CapacityPlanValidation.HistoryResponseModel>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }
		private Models.CapacityPlanValidation.HistoryRequestModel data { get; set; }
		public GetHistoryHandler(Identity.Models.UserModel user, Models.CapacityPlanValidation.HistoryRequestModel data)
		{
			this.user = user;
			this.data = data;
		}

		public ResponseModel<Models.CapacityPlanValidation.HistoryResponseModel> Handle()
		{
			lock(Locks.CapacityPlanLock)
			{
				try
				{
					var validate = Validate();
					if(!validate.Success)
					{
						return validate;
					}

					// -
					var capacityPlanLogEntities = CapacityPlanValidationLogAccess.GetByYearCountryHall(this.data.Year, this.data.CountryId, this.data.HallId)
						?? new List<CapacityPlanValidationLogEntity>();
					var userEntities = UserAccess.Get(capacityPlanLogEntities.Select(x => x.ValidationUserId ?? -1).ToList())
						?? new List<Infrastructure.Data.Entities.Tables.WPL.UserEntity>();

					var capacityValidationItems = new Models.CapacityPlanValidation.HistoryResponseModel
					{
						Items = null,
						CountryId = this.data.CountryId,
						HallId = this.data.HallId,
						Year = this.data.Year
					};

					capacityValidationItems.Items = capacityPlanLogEntities.Select(x => new Models.CapacityPlanValidation.HistoryResponseModel.HistoryItem
					{
						Date = x.ValidationTime,
						Level = x.ValidationLevel,
						LevelDescription = ((Enums.CapacityPlan.ValidationLevels)x.ValidationLevel).GetDescription(),
						ValidationStatus = x.ValidationStatusName,
						UserId = x.ValidationUserId,
						UserName = userEntities.Find(y => y.Id == x.ValidationUserId)?.Username
					}).ToList();

					// -
					return ResponseModel<Models.CapacityPlanValidation.HistoryResponseModel>.SuccessResponse(capacityValidationItems);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<Models.CapacityPlanValidation.HistoryResponseModel> Validate()
		{
			if(user == null)
			{
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
			}

			if(CountryAccess.Get(this.data.CountryId) == null)
				return ResponseModel<Models.CapacityPlanValidation.HistoryResponseModel>.FailureResponse("Country not found");

			if(HallAccess.Get(this.data.HallId) == null)
				return ResponseModel<Models.CapacityPlanValidation.HistoryResponseModel>.FailureResponse("Hall not found");

			// - 
			return ResponseModel<Models.CapacityPlanValidation.HistoryResponseModel>.SuccessResponse();
		}
	}
}
