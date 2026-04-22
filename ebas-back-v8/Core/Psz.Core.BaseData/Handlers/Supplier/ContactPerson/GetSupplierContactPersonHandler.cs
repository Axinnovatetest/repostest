using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.ContactPerson
{
	public class GetSupplierContactPersonHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Supplier.ContactPersonModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetSupplierContactPersonHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Supplier.ContactPersonModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
				var contactPersonEntity = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(lieferantenEntity.Nummer ?? -1);

				List<Models.Supplier.ContactPersonModel> response = new List<Models.Supplier.ContactPersonModel>();
				if(contactPersonEntity != null)
				{
					foreach(var item in contactPersonEntity)
					{
						response.Add(new Models.Supplier.ContactPersonModel(item));
					}
				}
				foreach(var item in response)
				{
					item.KundenId = lieferantenEntity.Nr;
				}
				return ResponseModel<List<Models.Supplier.ContactPersonModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Supplier.ContactPersonModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Supplier.ContactPersonModel>>.AccessDeniedResponse();
			}

			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
			if(lieferantenEntity == null)
			{
				return new ResponseModel<List<Models.Supplier.ContactPersonModel>>()
				{
					Errors = new List<ResponseModel<List<Models.Supplier.ContactPersonModel>>.ResponseError>() {
						new ResponseModel<List<Models.Supplier.ContactPersonModel>>.ResponseError {Key = "1", Value = "Supplier not found"}
					}
				};
			}

			return ResponseModel<List<Models.Supplier.ContactPersonModel>>.SuccessResponse();
		}
	}
}
