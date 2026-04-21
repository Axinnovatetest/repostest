using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ShippingMethods
{
	public class UpdateShippingMethodHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.ShippingMethod.ShippingMethodModel _data { get; set; }
		public UpdateShippingMethodHandler(Identity.Models.UserModel user, Models.ShippingMethod.ShippingMethodModel data)
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
				var _oldEntity = Infrastructure.Data.Access.Tables.BSD.Versandarten_AuswahlAccess.Get(_entity.ID);
				if(_oldEntity.Versandarten != _entity.Versandarten)
					Infrastructure.Data.Access.Tables.PRS.KundenAccess.UpdateShippingMethodCascade(_oldEntity.Versandarten, _entity.Versandarten);
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Versandarten_AuswahlAccess.Update(_entity);

				return ResponseModel<int>.SuccessResponse(responseBody);
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
			var shippingMethodEntity = Infrastructure.Data.Access.Tables.BSD.Versandarten_AuswahlAccess.Get(this._data.ID);
			if(shippingMethodEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Shipping Method not found" }
						}
				};
			}
			if(string.IsNullOrEmpty(this._data.ShippingMethod))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Shipping Method name should not be empty" }
						}
				};
			}
			var shippingMethodsEntities = Infrastructure.Data.Access.Tables.BSD.Versandarten_AuswahlAccess.Get();
			var theRest = shippingMethodsEntities.Where(x => x.ID != this._data.ID);
			var check = theRest.Where(x => x.Versandarten == this._data.ShippingMethod);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A shipping method with the same name already exsists" }
						}
				};
			}
			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByShippingMethod(shippingMethodEntity.Versandarten);
			if(exsist1 != null && exsist1.Count > 0)
			{
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(exsist1.Select(x => x.Nummer ?? -1)?.Distinct().ToList());
				return ResponseModel<int>.FailureResponse(
					$"Cannot update Shipping Method. The following customer(s) use this Shipping Method [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Kundennummer} - {x.Name1}")?.ToList())}]");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
