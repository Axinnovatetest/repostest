using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer.Adress
{
	public class GetCustomerAdressHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Customer.CustomerAdressModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetCustomerAdressHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Customer.CustomerAdressModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
				var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)kundenEntity.Nummer);

				var response = new Models.Customer.CustomerAdressModel(adressenEntity);
				response.Id = kundenEntity.Nr;
				response.Number = adressenEntity.Kundennummer.HasValue ? adressenEntity.Kundennummer.Value : -1;
				response.Isarchived = (Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(response.Id) != null) ? Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(response.Id).IsArchived : false;
				response.IsBoth = (adressenEntity.Lieferantennummer.HasValue) ? true : false;
				return ResponseModel<Models.Customer.CustomerAdressModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Customer.CustomerAdressModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Customer.CustomerAdressModel>.AccessDeniedResponse();
			}

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
			if(kundenEntity == null)
			{
				return new ResponseModel<Models.Customer.CustomerAdressModel>()
				{
					Errors = new List<ResponseModel<Models.Customer.CustomerAdressModel>.ResponseError>() {
						new ResponseModel<Models.Customer.CustomerAdressModel>.ResponseError {Key = "1", Value = "Customer not found"}
					}
				};
			}

			return ResponseModel<Models.Customer.CustomerAdressModel>.SuccessResponse();
		}
	}
}
