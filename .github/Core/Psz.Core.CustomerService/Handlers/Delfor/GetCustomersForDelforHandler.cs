using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetCustomersForDelforHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CustomerModel>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetCustomersForDelforHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				var customersList = new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
				var adresses = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
				if(_user.Access.Purchase.AllCustomers)
					customersList = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get();
				else
				{
					var userCustomers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(true, _user.Id);
					customersList = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(userCustomers?.Select(x => x.CustomerNumber).ToList());
				}
				if(customersList != null && customersList.Count > 0)
				{
					adresses = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersList.Select(x => x.Nummer ?? -1).ToList())?
						.Where(a => a.EDI_Aktiv.HasValue && a.EDI_Aktiv.Value).ToList();
					var adressesWithDuns = adresses?.Where(a => !string.IsNullOrEmpty(a.Duns) && !string.IsNullOrWhiteSpace(a.Duns)).ToList();
					response.AddRange(adressesWithDuns.Select(a => new CustomerModel
					{
						Id = customersList.FirstOrDefault(c => c.Nummer == a.Nr).Nr,
						Type = a.Anrede,
						CustomerNumber = customersList.FirstOrDefault(c => c.Nummer == a.Nr).Nummer ?? -1,
						AdressCustomerNumber = a.Kundennummer,

						Name = a.Name1,
						Name2 = a.Name2,
						Name3 = a.Name3,

						Contact = a.Briefanrede,
						Department = customersList.FirstOrDefault(c => c.Nummer == a.Nr).RG_Abteilung,
						CountryPostcode = customersList.FirstOrDefault(c => c.Nummer == a.Nr).RG_Land_PLZ_ORT,
						StreetPOBox = customersList.FirstOrDefault(c => c.Nummer == a.Nr).RG_Strasse_Postfach,
						Duns = a.Duns,

						Country = a.Land,
						Email = a.EMail,
						Fax = a.Fax,
						Phone = a.Telefon,
						POBox = a.Postfach,
						Street = a.StraBe
					}));
					var adresesWithoutDuns = adresses?.Where(a => string.IsNullOrEmpty(a.Duns) || string.IsNullOrWhiteSpace(a.Duns)).ToList();
					var adressesExtensions = Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByAddressNr(adresesWithoutDuns?.Select(a => a.Nr).ToList());
					if(adressesExtensions != null && adressesExtensions.Count > 0)
						response.AddRange(adressesExtensions.Select(a => new CustomerModel
						{
							Id = customersList.FirstOrDefault(c => c.Nummer == a.AdressenNr).Nr,
							Type = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Anrede,
							CustomerNumber = customersList.FirstOrDefault(c => c.Nummer == a.AdressenNr).Nummer ?? -1,
							AdressCustomerNumber = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Kundennummer,

							Name = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Name1,
							Name2 = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Name2,
							Name3 = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Name3,

							Contact = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Briefanrede,
							Department = customersList.FirstOrDefault(c => c.Nummer == a.AdressenNr).RG_Abteilung,
							CountryPostcode = customersList.FirstOrDefault(c => c.Nummer == a.AdressenNr).RG_Land_PLZ_ORT,
							StreetPOBox = customersList.FirstOrDefault(c => c.Nummer == a.AdressenNr).RG_Strasse_Postfach,
							Duns = a.Duns,

							Country = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Land,
							Email = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).EMail,
							Fax = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Fax,
							Phone = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Telefon,
							POBox = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).Postfach,
							Street = adresesWithoutDuns.FirstOrDefault(x => x.Nr == a.AdressenNr).StraBe
						}));
				}

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
