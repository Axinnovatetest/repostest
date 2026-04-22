using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetLandHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetLandsModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetLandHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				//var lands_tableEntities = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get();


				//foreach (var land_tableEntity in lands_tableEntities)
				//{
				//    responseBody.Add(new Models.Budget.GetLandsModel(land_tableEntity));
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
