using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.Data
{
	public class GetSupplierDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Supplier.SupplierDataModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetSupplierDataHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Supplier.SupplierDataModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var lieferentenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
				var adressEntity = int.TryParse(lieferentenEntity.Nummer.ToString(), out int lieferentenNummer)
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(lieferentenNummer)
					: null;
				return ResponseModel<Models.Supplier.SupplierDataModel>.SuccessResponse(new Models.Supplier.SupplierDataModel(lieferentenEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Supplier.SupplierDataModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Supplier.SupplierDataModel>.AccessDeniedResponse();
			}

			var lieferentenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
			if(lieferentenEntity == null)
			{
				return new ResponseModel<Models.Supplier.SupplierDataModel>()
				{
					Errors = new List<ResponseModel<Models.Supplier.SupplierDataModel>.ResponseError>() {
						new ResponseModel<Models.Supplier.SupplierDataModel>.ResponseError {Key = "1", Value = "Supplier not found"}
					}
				};
			}
			return ResponseModel<Models.Supplier.SupplierDataModel>.SuccessResponse();
		}
	}
}
