using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Customer.CustomerModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Customer.CustomerModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
				var adressEntity = int.TryParse(kundenEntity.Nummer.ToString(), out int knudenNummer)
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(knudenNummer)
					: null;
				var contactPersonsEntities = adressEntity != null
					? Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(adressEntity.Nr)
					: new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();

				var kundenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(kundenEntity.Nr);

				return ResponseModel<Models.Customer.CustomerModel>.SuccessResponse(new Models.Customer.CustomerModel(kundenEntity, adressEntity, contactPersonsEntities, kundenExtensionEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Customer.CustomerModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Customer.CustomerModel>.AccessDeniedResponse();
			}

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
			if(kundenEntity == null)
			{
				return new ResponseModel<Models.Customer.CustomerModel>()
				{
					Errors = new List<ResponseModel<Models.Customer.CustomerModel>.ResponseError>() {
						new ResponseModel<Models.Customer.CustomerModel>.ResponseError {Key = "1", Value = "Customer not found"}
					}
				};
			}


			return ResponseModel<Models.Customer.CustomerModel>.SuccessResponse();
		}
	}

}
