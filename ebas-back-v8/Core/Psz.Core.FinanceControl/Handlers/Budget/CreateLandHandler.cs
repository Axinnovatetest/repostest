using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateLandHandler
	{
		private Models.Budget.GetLandsModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateLandHandler(Models.Budget.GetLandsModel data, Identity.Models.UserModel user)
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
				var landsEntity = _data.ToBudgetLands();
				//var InsertedLand = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Insert(landsEntity);
				return ResponseModel<int>.SuccessResponse(/*InsertedLand*/0);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.ConfigCreateLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			//var errors = new List<ResponseModel<int>.ResponseError>();

			////var landEntity = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.GetByName(this._data.Land_name);
			////if(landEntity!= null)
			////    return ResponseModel<int>.FailureResponse(key: "1", value: $"Land [{this._data.Land_name}] already exists");

			//var purchaseAccessProfile = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.GetByMainAccessProfilesId(this._data.PurchaseId ?? -1);
			//if(purchaseAccessProfile == null)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Access Profile [{this._data.PurchaseName}] not found");
			//if (purchaseAccessProfile.ModuleActivated == false)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Access Profile [{this._data.PurchaseName}] not active");

			//var purchaseUsers = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByAccessProfilesIds(new List<int> { purchaseAccessProfile.MainAccessProfileId ?? -1 });
			//// ToDo: filter by land // CREATE LAND & Uaser LINK TO LAND should come before this step
			//if (purchaseUsers == null || purchaseUsers.Count <= 0)
			//    return ResponseModel<int>.FailureResponse(key: "1", value: $"No user with Access Profile [{this._data.PurchaseName}]");


			//if (errors.Count > 0)
			//{
			//    return new ResponseModel<int>() { Errors = errors };
			//}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
