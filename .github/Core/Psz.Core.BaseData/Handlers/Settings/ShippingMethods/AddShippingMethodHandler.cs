using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ShippingMethods
{
	public class AddShippingMethodHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.ShippingMethod.ShippingMethodModel _data { get; set; }
		public AddShippingMethodHandler(Identity.Models.UserModel user, Models.ShippingMethod.ShippingMethodModel data)
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
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Versandarten_AuswahlAccess.Insert(_entity);

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
			var check = shippingMethodsEntities.Where(x => x.Versandarten == this._data.ShippingMethod);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A shipping method with the same name exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
