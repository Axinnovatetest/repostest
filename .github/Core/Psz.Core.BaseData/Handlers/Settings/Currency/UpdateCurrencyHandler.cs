using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.Currency
{
	public class UpdateCurrencyHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Currency.CurrencyModel _data { get; set; }

		public UpdateCurrencyHandler(Identity.Models.UserModel user, Models.Currency.CurrencyModel data)
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
				var response = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Update(_entity);
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
			var currencyEntity = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get(this._data.Id);
			if(currencyEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Currency not found" }
						}
				};
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
			var theRest = currencyEntities.Where(x => x.Nr != this._data.Id);
			var check = theRest.Where(x => x.Wahrung == this._data.Name || x.Symbol == this._data.Symbol);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A Currency with the same name or symbol already exsists" }
						}
				};
			}

			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByCurrency(this._data.Id);
			var exsist2 = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByCurrency(this._data.Id);
			if(exsist1 != null && exsist1.Count > 0 || exsist2 != null && exsist2.Count > 0)
			{
				var nr = exsist1?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>();
				nr.AddRange(exsist2?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>());
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(nr);
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update Currency. The following customers and/or suppliers use this Currency. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Lieferantennummer+" "+ x.Kundennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
