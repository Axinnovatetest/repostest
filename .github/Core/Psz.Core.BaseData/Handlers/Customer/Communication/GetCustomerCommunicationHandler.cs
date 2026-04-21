using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer.Communication
{
	public class GetCustomerCommunicationHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Customer.CustomerCommunicationModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetCustomerCommunicationHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;

		}

		public ResponseModel<Models.Customer.CustomerCommunicationModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
				var adressEntity = int.TryParse(kundenEntity.Nummer.ToString(), out int Nummer)
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(Nummer)
					: null;
				var response = new Models.Customer.CustomerCommunicationModel(adressEntity);
				response.Id = kundenEntity.Nr;
				response.LanguageId = kundenEntity.Sprache.HasValue ? kundenEntity.Sprache.Value : -1;
				response.Isarchived = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(response.Id).IsArchived ?? false;
				response.OrderAddress = kundenEntity.OrderAddress ?? "";

				return ResponseModel<Models.Customer.CustomerCommunicationModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Customer.CustomerCommunicationModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Customer.CustomerCommunicationModel>.AccessDeniedResponse();
			}

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
			if(kundenEntity == null)
			{
				return new ResponseModel<Models.Customer.CustomerCommunicationModel>()
				{
					Errors = new List<ResponseModel<Models.Customer.CustomerCommunicationModel>.ResponseError>() {
						new ResponseModel<Models.Customer.CustomerCommunicationModel>.ResponseError {Key = "1", Value = "Customer not found"}
					}
				};
			}
			return ResponseModel<Models.Customer.CustomerCommunicationModel>.SuccessResponse();
		}
	}
}
