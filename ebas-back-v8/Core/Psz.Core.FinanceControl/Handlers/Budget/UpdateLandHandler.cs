using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateLandHandler: IHandle<Models.Budget.GetLandsModel, ResponseModel<int>>
	{
		private Models.Budget.GetLandsModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateLandHandler(Models.Budget.GetLandsModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var Landentity = _data.ToBudgetLands();
				//var updatedLand = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Update(Landentity);
				return ResponseModel<int>.SuccessResponse(/*updatedLand*/0);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.ConfigEditLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			//var LandEntity = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get(this._data.ID);
			//if (LandEntity == null)
			//{
			//    return new ResponseModel<int>()
			//    {
			//        Errors = new List<ResponseModel<int>.ResponseError>() {
			//            new ResponseModel<int>.ResponseError {Key = "", Value = "Land not found"}
			//        }
			//    };
			//}

			//var errors = new List<ResponseModel<int>.ResponseError>();

			//var purchaseAccessProfile = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.GetByMainAccessProfilesId(this._data.PurchaseId ?? -1);
			//if (purchaseAccessProfile == null)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Access Profile [{this._data.PurchaseName}] not found");
			//if (purchaseAccessProfile.ModuleActivated == false)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Access Profile [{this._data.PurchaseName}] not active");

			//var purchaseUsers = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByAccessProfilesIds(new List<int> { purchaseAccessProfile.MainAccessProfileId ?? -1 });
			//if (purchaseUsers == null || purchaseUsers.Count <= 0)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: $"No user with Access Profile [{this._data.PurchaseName}]");

			//// Filter by Land
			////var landUsers = Infrastructure.Data.Access.Tables.FNC.Land_User_JointAccess.GetByUsersIds(purchaseUsers.Select(x => x.Id)?.ToList());
			////if (landUsers == null)
			////    return ResponseModel<int>.FailureResponse(key: "1", value: $"No user with Access Profile [{this._data.PurchaseName}] in Land [{LandEntity.Land_name}]");

			//if (errors.Count > 0)
			//{
			//    return new ResponseModel<int>() { Errors = errors };
			//}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
