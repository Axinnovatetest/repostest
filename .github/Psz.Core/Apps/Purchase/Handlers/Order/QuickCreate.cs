using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<int> QuickCreate(Models.Order.QuickCreateModel data,
		   Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					if(user == null || (!user.Access.CustomerService.ModuleActivated && !user.Access.Purchase.ModuleActivated))
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(data.CustomerId);
					if(customerDb == null)
					{
						return new Core.Models.ResponseModel<int>(data.CustomerId)
						{
							Errors = new List<string>() { "Customer not found" }
						};
					}

					var adressDb = customerDb.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
						: null;
					if(adressDb == null)
					{
						return new Core.Models.ResponseModel<int>(data.CustomerId)
						{
							Errors = new List<string>() { "Address not found" }
						};
					}
					return CreateInner(new Models.Order.CreateModel()
					{
						FreeText = $"USt - ID - Nr.: {customerDb.EG___Identifikationsnummer}",
						DocumentName = string.Empty,
						DocumentNumber = string.Empty,
						ManualCreationCustomerId = customerDb.Nr,
						ManualCreationCustomerNummer = customerDb.Nummer ?? -1,

						BuyerDuns = adressDb.Duns,
						BuyerName = adressDb.Name1,
						BuyerName2 = adressDb.Name2,
						BuyerName3 = adressDb.Name3,
						BuyerContactName = adressDb.Abteilung,
						BuyerPurchasingDepartment = adressDb.Abteilung,
						BuyerPostalCode = adressDb.PLZ_StraBe,//adressDb.Postfach,
						BuyerStreet = $"{adressDb.StraBe}",//$"{adressDb.PLZ_StraBe} {adressDb.StraBe}",
						BuyerCity = adressDb.Ort,
						BuyerCountryName = adressDb.Land,
						BuyerContactFax = null,
						BuyerContactTelephone = null,
						BuyerPartyIdentification = null,
						BuyerPartyIdentificationCodeListQualifier = null,

						ConsigneeName = adressDb.Name1,
						ConsigneeName2 = adressDb.Name2,
						ConsigneeName3 = adressDb.Name3,
						ConsigneeContactName = adressDb.Abteilung,
						ConsigneePurchasingDepartment = adressDb.Abteilung,
						ConsigneeStreet = $"{adressDb.PLZ_StraBe} {adressDb.StraBe}",
						ConsigneePostalCode = adressDb.Postfach,
						ConsigneeCity = adressDb.Ort,
						ConsigneeSalutation = adressDb.Briefanrede,
						ConsigneeCountryName = adressDb.Land,
						ConsigneeContactFax = null,
						ConsigneeContactTelephone = null,
						ConsigneeDUNS = null,
						ConsigneeIdentification = null,
						ConsigneeIdentificationCodeListQualifier = null,
						ConsigneeStorageLocation = null,
						ConsigneeUnloadingPoint = null,
						DocumentNr = data.DocumentCustomer,
						Elements = new List<Models.Order.Element.CreateItemModel>()
					}, user);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
