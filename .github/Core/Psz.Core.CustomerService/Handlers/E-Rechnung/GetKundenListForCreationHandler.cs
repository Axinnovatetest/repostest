using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class GetKundenListForCreationHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CustomerModel>>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetKundenListForCreationHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<CustomerModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<CustomerModel>();
				var adressenList = Infrastructure.Data.Access.Joins.CTS.Divers.GetKundenListForERechnung(_data);
				var kundenList = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(adressenList?.Select(x => x.Nr).ToList());
				response = adressenList?.Select(x => new CustomerModel
				{
					AdressCustomerNumber = x.Kundennummer,
					Contact = x.Briefanrede,
					Country = x.Land,
					CountryPostcode = x.PLZ_Postfach,
					CustomerNumber = x.Nr,
					Department = kundenList.FirstOrDefault(k => k.Nummer == x.Nr)?.RG_Abteilung,
					Duns = x.Duns,
					Email = x.EMail,
					Fax = x.Fax,
					Id = kundenList.FirstOrDefault(k => k.Nummer == x.Nr)?.Nr ?? -1,
					Name = x.Name1,
					Name2 = x.Name2,
					Name3 = x.Name3,
					Phone = x.Telefon,
					POBox = x.Postfach,
					Street = x.StraBe,
					StreetPOBox = kundenList.FirstOrDefault(k => k.Nummer == x.Nr)?.RG_Strasse_Postfach,
					Type = x.Anrede,
				}).ToList();

				return ResponseModel<List<CustomerModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<CustomerModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<CustomerModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<CustomerModel>>.SuccessResponse();
		}
	}
}
