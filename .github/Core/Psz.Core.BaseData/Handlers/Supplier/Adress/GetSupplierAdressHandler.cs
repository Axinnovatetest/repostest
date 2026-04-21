using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.Adress
{
	public class GetSupplierAdressHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Supplier.SupplierAdressModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetSupplierAdressHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Supplier.SupplierAdressModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var lieferentenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
				var adressEntity = int.TryParse(lieferentenEntity.Nummer.ToString(), out int Nummer)
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(Nummer)
					: null;
				var response = new Models.Supplier.SupplierAdressModel(adressEntity);
				response.Id = lieferentenEntity.Nr;
				response.Number = adressEntity != null && adressEntity.Lieferantennummer.HasValue ? adressEntity.Lieferantennummer.Value : -1;
				response.Isarchived = (Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(response.Id) != null) ? Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(response.Id).IsArchived : false;
				response.IsBoth = (adressEntity.Kundennummer.HasValue) ? true : false;
				return ResponseModel<Models.Supplier.SupplierAdressModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Supplier.SupplierAdressModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Supplier.SupplierAdressModel>.AccessDeniedResponse();
			}

			var lieferentenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
			if(lieferentenEntity == null)
			{
				return new ResponseModel<Models.Supplier.SupplierAdressModel>()
				{
					Errors = new List<ResponseModel<Models.Supplier.SupplierAdressModel>.ResponseError>() {
						new ResponseModel<Models.Supplier.SupplierAdressModel>.ResponseError {Key = "1", Value = "Supplier not found"}
					}
				};
			}
			return ResponseModel<Models.Supplier.SupplierAdressModel>.SuccessResponse();
		}
	}
}
