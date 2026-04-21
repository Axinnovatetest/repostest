using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetCurrencyHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetCurrencyModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetCurrencyHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.GetCurrencyModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetCurrencyModel>();
				var currencys_tableEntities = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get();
				foreach(var currency_tableEntity in currencys_tableEntities)
				{
					responseBody.Add(new Models.Budget.GetCurrencyModel(currency_tableEntity));
				}

				return ResponseModel<List<Models.Budget.GetCurrencyModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.GetCurrencyModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.GetCurrencyModel>>.SuccessResponse();
		}
	}
}
