using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> Get(bool? isEDIActive = false)
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(), isEDIActive);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.UnexpectedErrorResponse();
			}
		}

		public static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> Get(List<int> ids)
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(ids));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.UnexpectedErrorResponse();
			}
		}

		internal static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> GetByNumbers(List<int> numbers)
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(numbers));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.UnexpectedErrorResponse();
			}
		}

		public static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> Get(List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> customersDb, bool? isEDIActive = null)
		{
			try
			{
				var customers = new List<Models.Customers.CustomerModel>();

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

					customers.Add(new Models.Customers.CustomerModel()
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

				return new Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>()
				{
					Success = true,
					Body = customers
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
