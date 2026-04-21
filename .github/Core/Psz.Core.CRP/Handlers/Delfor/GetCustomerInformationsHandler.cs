using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.Delfor;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.Delfor
{
	public partial class DelforService
	{
		public ResponseModel<DeliveryForcastHeaderModel> GetCustomerInformations(UserModel user, int data)
		{
			try
			{
				var validationResponse = this.ValidateGetCustomerInformations(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//var kunden = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data);
				var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(data);
				var customerContact = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(data);
				var duns = !string.IsNullOrEmpty(adress.Duns) && !string.IsNullOrWhiteSpace(adress.Duns)
					? adress.Duns
					: Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByAddressNr(adress.Nr)?.Duns;
				var resposne = new DeliveryForcastHeaderModel
				{
					CustomerId = data,
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
		public ResponseModel<DeliveryForcastHeaderModel> ValidateGetCustomerInformations(UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<DeliveryForcastHeaderModel>.AccessDeniedResponse();
			}

			return ResponseModel<DeliveryForcastHeaderModel>.SuccessResponse();
		}
	}
}