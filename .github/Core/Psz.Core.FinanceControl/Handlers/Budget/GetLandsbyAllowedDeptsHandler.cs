using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetLandsbyAllowedDeptsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetLandsModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetLandsbyAllowedDeptsHandler(Identity.Models.UserModel user, int id_user)
		{
			this._user = user;
			this._data = id_user;

		}
		public ResponseModel<List<Models.Budget.GetLandsModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetLandsModel>();
				//var Lands_tableEntities = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.GetAllowedLandsbyDept(this._data);

				//if (Lands_tableEntities != null)
				//{
				//    foreach (var dep_tableEntity in Lands_tableEntities)
				//    {
				//        responseBody.Add(new Models.Budget.GetLandsModel(dep_tableEntity));
				//    }
				//}

				return ResponseModel<List<Models.Budget.GetLandsModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.GetLandsModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.GetLandsModel>>.SuccessResponse();
		}
	}
}
