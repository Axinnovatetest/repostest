using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.Currency
{
	public class AddCurrencyHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Currency.CurrencyModel _data { get; set; }

		public AddCurrencyHandler(Identity.Models.UserModel user, Models.Currency.CurrencyModel data)
		{
			this._user = user;
			this._data = data;
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
				var _entity = this._data.ToEntity();
				_entity.Stand = DateTime.Now;
				var response = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Insert(_entity);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			if(string.IsNullOrEmpty(this._data.Name))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Name should not be empty" }
						}
				};
			}
			if(string.IsNullOrEmpty(this._data.Symbol))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Symbol should not be empty" }
						}
				};
			}
			var currencyEntities = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get();
			var check = currencyEntities.Where(x => x.Wahrung == this._data.Name || x.Symbol == this._data.Symbol);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A Currency with the same name or symbol already exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
