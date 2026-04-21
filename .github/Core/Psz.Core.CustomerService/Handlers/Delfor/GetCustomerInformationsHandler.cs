using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetCustomerInformationsHandler: IHandle<Identity.Models.UserModel, ResponseModel<DeliveryForcastHeaderModel>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetCustomerInformationsHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<DeliveryForcastHeaderModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//var kunden = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data);
				var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(_data);
				var customerContact = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(_data);
				var duns = !string.IsNullOrEmpty(adress.Duns) && !string.IsNullOrWhiteSpace(adress.Duns)
					? adress.Duns
					: Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByAddressNr(adress.Nr)?.Duns;
				var resposne = new DeliveryForcastHeaderModel
				{
					CustomerId = _data,
					CustomerCity = adress.Ort,
					CustomerPostCode = adress.Postfach,
					CustomerStreet = adress.StraBe,
					CustomerContactName = customerContact != null && customerContact.Count > 0
					? customerContact[0].Ansprechpartner
					: null,
					CustomerContactFax = adress.Fax,
					CustomerContactPhone = adress.Telefon,
					CustomerName = adress.Name1,
					DUNS = duns,
					Date = null
				};

				return ResponseModel<DeliveryForcastHeaderModel>.SuccessResponse(resposne);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<DeliveryForcastHeaderModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<DeliveryForcastHeaderModel>.AccessDeniedResponse();
			}

			return ResponseModel<DeliveryForcastHeaderModel>.SuccessResponse();
		}

	}
}
