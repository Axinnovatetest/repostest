using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.Communication
{
	public class GetSupplierCommunicationHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Supplier.SupplierCommunicationModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetSupplierCommunicationHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Supplier.SupplierCommunicationModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
				var adressEntity = int.TryParse(lieferantenEntity.Nummer.ToString(), out int Nummer)
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(Nummer)
					: null;
				var response = new Models.Supplier.SupplierCommunicationModel(adressEntity);
				response.Id = lieferantenEntity.Nr;
				response.LanguageId = lieferantenEntity.Sprache.HasValue ? lieferantenEntity.Sprache.Value : -1;
				response.Isarchived = (Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(response.Id) != null) ? Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(response.Id).IsArchived : false;
				return ResponseModel<Models.Supplier.SupplierCommunicationModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Supplier.SupplierCommunicationModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Supplier.SupplierCommunicationModel>.AccessDeniedResponse();
			}

			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
			if(lieferantenEntity == null)
			{
				return new ResponseModel<Models.Supplier.SupplierCommunicationModel>()
				{
					Errors = new List<ResponseModel<Models.Supplier.SupplierCommunicationModel>.ResponseError>() {
						new ResponseModel<Models.Supplier.SupplierCommunicationModel>.ResponseError {Key = "1", Value = "Supplier not found"}
					}
				};
			}
			return ResponseModel<Models.Supplier.SupplierCommunicationModel>.SuccessResponse();
		}
	}
}
