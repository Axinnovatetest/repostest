using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<CustomerModel>> getCustomers(UserModel user, string searchText)
		{
			if(user == null)
				return ResponseModel<List<CustomerModel>>.AccessDeniedResponse();
			try
			{

				var response = Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get());
				if(searchText.IsNullOrEmptyOrWitheSpaces() || searchText == "_")
					response = response.Take(10).ToList();
				else
					response = response.Where(c => c.Name.ToLower().Contains(searchText.ToLower())
					|| c.AdressCustomerNumber.ToString().Contains(searchText.ToLower())).ToList();

				return ResponseModel<List<CustomerModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static List<CustomerModel> Get(List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> customersDb, bool? isEDIActive = null)
		{
			try
			{
				var customers = new List<CustomerModel>();

				var customersNumbers = customersDb
					.Where(e => e.Nummer.HasValue)
					.Select(e => e.Nummer.Value).ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNumbers);
				if(isEDIActive.HasValue)
				{
					adressesDb = adressesDb.FindAll(e => e.Adresstyp == 1
						&& (e.EDI_Aktiv.HasValue ? e.EDI_Aktiv == isEDIActive.Value : false));
				}
				else
				{
					adressesDb = adressesDb.FindAll(e => e.Adresstyp == 1);
				}


				foreach(var customerDb in customersDb)
				{
					var adressDb = adressesDb.Find(e => e.Nr == customerDb.Nummer);
					if(adressDb == null)
					{
						continue;
					}

					customers.Add(new CustomerModel()
					{
						Id = customerDb.Nr,
						Type = adressDb?.Anrede,
						CustomerNumber = customerDb.Nummer ?? -1,
						AdressCustomerNumber = adressDb?.Kundennummer,

						Name = adressDb?.Name1,
						Name2 = adressDb?.Name2,
						Name3 = adressDb?.Name3,

						Contact = adressDb.Briefanrede,
						Department = customerDb.RG_Abteilung,
						CountryPostcode = customerDb.RG_Land_PLZ_ORT,
						StreetPOBox = customerDb.RG_Strasse_Postfach,
						Duns = adressDb?.Duns,

						Country = adressDb?.Land,
						Email = adressDb?.EMail,
						Fax = adressDb?.Fax,
						Phone = adressDb?.Telefon,
						POBox = adressDb?.Postfach,
						Street = adressDb?.StraBe
					});
				}

				return customers;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}