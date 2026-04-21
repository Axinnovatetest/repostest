using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteLandHandler: IHandle<Models.Budget.GetLandsModel, ResponseModel<int>>
	{
		private int _LandID { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteLandHandler(int landid, Identity.Models.UserModel user)
		{
			this._LandID = landid;
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

				//var landentity = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get(_LandID);
				//if (landentity == null)
				//{
				//    return ResponseModel<int>.SuccessResponse();
				//}

				return ResponseModel<int>.SuccessResponse(/*Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Delete(landentity.ID)*/0);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.ConfigDeleteLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
