using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.Currency
{
	public class GetCurrenciesForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Currency.CurrencyModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCurrenciesForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Currency.CurrencyModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var wahrungenEntities = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.GetCurrencies();

				var response = new List<Models.Currency.CurrencyModel>();

				foreach(var wahrungenEntity in wahrungenEntities)
				{
					response.Add(new Models.Currency.CurrencyModel(wahrungenEntity));
				}

				return ResponseModel<List<Models.Currency.CurrencyModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Currency.CurrencyModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Currency.CurrencyModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Currency.CurrencyModel>>.SuccessResponse();
		}
	}
}
