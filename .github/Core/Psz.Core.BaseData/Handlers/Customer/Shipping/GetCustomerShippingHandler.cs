using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer.Shipping
{
	public class GetCustomerShippingHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Customer.CustomerShippingModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetCustomerShippingHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Customer.CustomerShippingModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
				var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(kundenEntity.Nummer ?? -1);
				var response = new Models.Customer.CustomerShippingModel(kundenEntity, adressenEntity);
				return ResponseModel<Models.Customer.CustomerShippingModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Customer.CustomerShippingModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Customer.CustomerShippingModel>.AccessDeniedResponse();
			}

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
			if(kundenEntity == null)
			{
				return new ResponseModel<Models.Customer.CustomerShippingModel>()
				{
					Errors = new List<ResponseModel<Models.Customer.CustomerShippingModel>.ResponseError>() {
						new ResponseModel<Models.Customer.CustomerShippingModel>.ResponseError {Key = "1", Value = "Customer not found"}
					}
				};
			}
			return ResponseModel<Models.Customer.CustomerShippingModel>.SuccessResponse();
		}
	}
}
