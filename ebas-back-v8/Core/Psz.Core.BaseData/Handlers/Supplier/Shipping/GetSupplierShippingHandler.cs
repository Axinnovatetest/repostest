using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.Shipping
{
	public class GetSupplierShippingHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Supplier.SupplierShippingModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetSupplierShippingHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Supplier.SupplierShippingModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
				var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(lieferantenEntity.Nummer ?? -1);
				var response = new Models.Supplier.SupplierShippingModel(lieferantenEntity, adressenEntity);
				return ResponseModel<Models.Supplier.SupplierShippingModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Supplier.SupplierShippingModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Supplier.SupplierShippingModel>.AccessDeniedResponse();
			}

			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
			if(lieferantenEntity == null)
			{
				return new ResponseModel<Models.Supplier.SupplierShippingModel>()
				{
					Errors = new List<ResponseModel<Models.Supplier.SupplierShippingModel>.ResponseError>() {
						new ResponseModel<Models.Supplier.SupplierShippingModel>.ResponseError {Key = "1", Value = "Supplier not found"}
					}
				};
			}
			return ResponseModel<Models.Supplier.SupplierShippingModel>.SuccessResponse();
		}
	}
}
