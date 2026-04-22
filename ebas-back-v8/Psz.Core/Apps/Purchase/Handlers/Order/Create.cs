using Newtonsoft.Json;
using Psz.Core.Apps.Purchase.Models.Order;
using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public const string MANUAL_DOCUMENT_PREFIX = "MC-";

		public static Core.Models.ResponseModel<int> Create(Models.Order.CreateModel data,
		   Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null || !user.Access.Purchase.ModuleActivated)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return CreateInner(data, user);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static Core.Models.ResponseModel<int> CreateInner(Models.Order.CreateModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					try
					{
						var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(data.ManualCreationCustomerId);
						if(customerDb == null)
						{
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = new List<string>() { "Customer not found" }
							};
						}

						var customerId = customerDb.Nr;
						var customerNummer = customerDb.Nummer;
						var adressDb = customerDb.Nummer.HasValue
							? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
							: null;
						if(adressDb == null)
						{
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = new List<string>() { "Address not found" }
							};
						}

						// > Check if DocumentNumber Exists if new Order
						//var orderDbByUniqueNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByBezugAndKundenNr(data.DocumentNumber,
						//    adressDb.Nr);
						//if (orderDbByUniqueNumber != null)
						//{
						//    return new Core.Models.ResponseModel<int>(-1)
						//    {
						//        Errors = new List<string>() { "Document Exists" }
						//    };
						//}

						#region > Check Items
						var itemsSuppliersNumbers = data.Elements
							.Where(e => !string.IsNullOrWhiteSpace(e.ItemNumber))
							.Select(e => e.ItemNumber?.Trim())
							.ToList();
						var itemsCustomersNumbers = data.Elements
							.Where(e => !string.IsNullOrWhiteSpace(e.CustomerItemNumber))
							.Select(e => e.CustomerItemNumber?.Trim()?.TrimStart('0'))
							.ToList();

						var itemsDbBySuppliersNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(itemsSuppliersNumbers, itemsCustomersNumbers);
						var itemsDbByCustomerNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByBezeichnung1(itemsCustomersNumbers);

						var itemsErrors = new List<string>();
						foreach(var lineItem in data.Elements)
						{
							// - Search Article by PSZ number, if any
							List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> itemDbs = null;
							if(!string.IsNullOrWhiteSpace(lineItem.ItemNumber))
							{
								itemDbs = !string.IsNullOrWhiteSpace(lineItem.ItemNumber)
										? itemsDbBySuppliersNumbers.FindAll(e => e.ArtikelNummer == lineItem.ItemNumber).ToList()
										: null;
							}
							else
							{
								itemDbs = !string.IsNullOrWhiteSpace(lineItem.CustomerItemNumber)
										   ? itemsDbBySuppliersNumbers.FindAll(e => e.Bezeichnung1?.ToLower().Trim().Contains(lineItem.CustomerItemNumber?.ToLower().Trim()?.TrimStart('0')) == true)?.ToList()
										   : null;
							}

							if(itemDbs == null || itemDbs.Count <= 0)
							{
								itemDbs = !string.IsNullOrWhiteSpace(lineItem.CustomerItemNumber)
									? itemsDbByCustomerNumbers.FindAll(e => e.Bezeichnung1?.ToLower().Trim().Contains(lineItem.CustomerItemNumber?.ToLower().Trim()?.TrimStart('0')) == true)?.ToList()
									: null;
							}

							if(itemDbs == null || itemDbs.Count <= 0)
							{
								itemsErrors.Add("Position number " + lineItem.PositionNumber + ": Article not found.");
								continue;
							}
							//if(itemDbs.Count > 0)
							//{
							//    if(itemDbs.All(e => e.Freigabestatus == "O"))
							//    {
							//        itemsErrors.Add("Position number " + lineItem.PositionNumber + ": Article is obsolete");
							//        continue;
							//    }
							//}

							if(lineItem.UnitPriceBasis <= 0)
							{
								itemsErrors.Add("Position number " + lineItem.PositionNumber + ": UnitPriceBasis " + lineItem.UnitPriceBasis + " is invalid");
							}

							if(lineItem.OrderedQuantity <= 0)
							{
								itemsErrors.Add("Position number " + lineItem.PositionNumber + ": Ordered Quantity " + lineItem.OrderedQuantity + " is invalid");
							}

							if(lineItem.CurrentItemPriceCalculationNet < 0)
							{
								itemsErrors.Add("Position number " + lineItem.PositionNumber + ": Current Item Price Calculation Net " + lineItem.CurrentItemPriceCalculationNet + " is invalid");
							}
						}

						if(itemsErrors.Count > 0)
						{
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = itemsErrors
							};
						}
						#endregion

						var itemsPricingGroupsNrs = itemsDbBySuppliersNumbers.Select(e => e.ArtikelNr).ToList();
						itemsPricingGroupsNrs.AddRange(itemsDbByCustomerNumbers.Select(e => e.ArtikelNr));

						var itemsPricingGroupsDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(itemsPricingGroupsNrs);

						var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
							? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
							: null;

						var mailBoxIsPreferred = adressDb.Postfach_bevorzugt == true;

						// Datum --- Date
						// Falligkeit --- DueDate
						// Liefertermin --- DeliveryDate
						// Versanddatum_Auswahl --- ShippingDate
						// Wunschtermin --- DesiredDate

						var orderDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity()
						{
							Bezug = string.IsNullOrEmpty(data.DocumentNr) || string.IsNullOrWhiteSpace(data.DocumentNr) ? getUniqueDocumentName(customerNummer ?? -1, Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION) : data.DocumentNr,
							EDI_Dateiname_CSV = data.DocumentName,

							ABSENDER = data.BuyerName,
							Kunden_Nr = adressDb.Nr,
							Typ = Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Confirmation), // "Auftragsbestätigung",
							Mandant = "PSZ electronic",
							EDI_Order_Neu = false,

							Vorname_NameFirma = data.BuyerName,
							Name2 = adressDb.Name2,
							Name3 = adressDb.Name3,

							Ansprechpartner = data.BuyerContactName,
							Abteilung = data.BuyerPurchasingDepartment,

							Straße_Postfach = mailBoxIsPreferred
								? $"Postfach {adressDb.Postfach}"
								: $"{data.BuyerStreet}",
							//mailBoxIsPreferred
							//    ? $"Postfach {adressDb.Postfach}"
							//    : $"{data.BuyerStreet}",
							Land_PLZ_Ort = mailBoxIsPreferred
								? $"{adressDb.PLZ_Postfach} {adressDb.Ort}"
								: $"{data.BuyerPostalCode} {data.BuyerCity} ",
							//mailBoxIsPreferred
							//    ? $"{adressDb.PLZ_Postfach} {adressDb.Ort}"
							//    : $"{data.BuyerStreet}, {data.BuyerCity} {data.BuyerPostalCode}",

							Versandart = customerDb.Versandart,
							Zahlungsweise = customerDb.Zahlungsweise,

							Konditionen = conditionAssignementTableDb?.Text,

							Unser_Zeichen = adressDb.Kundennummer.HasValue ? adressDb.Kundennummer.ToString() : "",
							Ihr_Zeichen = customerDb.Lieferantenummer__Kunden_,
							USt_Berechnen = customerDb.Umsatzsteuer_berechnen,
							Falligkeit = DateTime.Now.AddDays(+30),
							Datum = DateTime.Now,
							Briefanrede = adressDb.Briefanrede,
							Personal_Nr = 0,

							Freitext = data.FreeText,

							Lieferadresse = "0",
							Reparatur_nr = 0,
							Ab_id = -1, // update after insert
							Nr_BV = 0,
							Nr_RA = 0,
							Nr_Kanban = 0,
							Nr_auf = 0,
							Nr_lie = 0,
							Nr_rec = 0,
							Nr_pro = 0,
							Nr_gut = 0,
							Nr_sto = 0,
							Belegkreis = 0,
							Wunschtermin = new DateTime(2999, 12, 31),
							Neu = -1,

							LAnrede = adressDb.Anrede,
							LVorname_NameFirma = data.ConsigneeName,
							LName2 = data.ConsigneeName2,
							LName3 = data.ConsigneeName3,
							LAnsprechpartner = data.ConsigneeContactName,
							LAbteilung = data.ConsigneePurchasingDepartment,
							LStraße_Postfach = $"{data.ConsigneeStreet} {data.ConsigneePostalCode}",
							LLand_PLZ_Ort = $"{data.ConsigneeStreet} {data.ConsigneePostalCode}, {data.ConsigneeCity}",
							LBriefanrede = data.ConsigneeSalutation,

							Neu_Order = null, //(!data.IsManualCreation),
							Angebot_Nr = 0, // < new
							Erledigt = false
						};

						// >>>>>> Logging
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Debug, $" OrderImport[Purchase] >>>>>> insert orderDb:{JsonConvert.SerializeObject(orderDb)} ");

						orderDb.Nr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Insert(orderDb);
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateAbId(orderDb.Nr, orderDb.Nr);
						var OrderAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderDb.Nr);
						UpdateGlobalDataModel updatedGlobal = new UpdateGlobalDataModel
						{
							Id = OrderAfterInsert.Nr,
							Conditions = OrderAfterInsert.Konditionen,
							Contact = OrderAfterInsert.Ansprechpartner,
							CountryPostcode = OrderAfterInsert.Land_PLZ_Ort,
							Date = OrderAfterInsert.Datum,
							DeliveryDate = OrderAfterInsert.Falligkeit,
							Department = OrderAfterInsert.Abteilung,
							DesiredDate = OrderAfterInsert.Wunschtermin,
							DocumentNumber = OrderAfterInsert.Bezug,
							DueDate = OrderAfterInsert.Falligkeit,
							Freetext = OrderAfterInsert.Freitext,
							Name = OrderAfterInsert.Vorname_NameFirma,
							Name2 = OrderAfterInsert.Name2,
							Name3 = OrderAfterInsert.Name3,
							OrderTitle = OrderAfterInsert.Briefanrede,
							Payment = OrderAfterInsert.Zahlungsweise,
							PersonalNumber = OrderAfterInsert.Personal_Nr,
							RepairNumber = OrderAfterInsert.Reparatur_nr,
							Shipping = OrderAfterInsert.Versandart,
							ShippingAddress = OrderAfterInsert.Lieferadresse,
							StreetPOBox = OrderAfterInsert.Straße_Postfach,
							Vat = OrderAfterInsert.USt_Berechnen ?? false,
						};
						Psz.Core.Apps.Purchase.Handlers.Order.Confirm(updatedGlobal, user);
						data.Elements.ForEach(e => e.UnloadingPoint = data.ConsigneeUnloadingPoint);

						var createElementsRequestData = new Models.Order.Element.NotCalculatedOrderElementsModel()
						{
							OrderId = orderDb.Nr,
							Items = data.Elements
						};

						var createElementsResponse = Element.CreateItems(createElementsRequestData, user);

						// > Main Extention
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity()
						{
							Id = -1,
							Version = 0,
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = user.Id,
							LastUpdateUsername = user.Username,
							OrderId = orderDb.Nr,
							EdiValidationTime = DateTime.Now,
							EdiValidationUserId = -1,
							SenderDuns = null,
						});
						;

						// > Extension Buyer 
						var buyerDb = new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity
						{
							Id = -1,
							OrderId = orderDb.Nr,
							DUNS = data.BuyerDuns?.ToString(),
							Name = data.BuyerName,
							Name2 = data.BuyerName2,
							Name3 = data.BuyerName3,
							City = data.BuyerCity,
							ContactFax = data.BuyerContactFax,
							ContactName = data.BuyerContactName,
							ContactTelephone = data.BuyerContactTelephone,
							CountryName = data.BuyerCountryName,
							PartyIdentification = data.BuyerPartyIdentification,
							PartyIdentificationCodeListQualifier = data.BuyerPartyIdentificationCodeListQualifier,
							PostalCode = data.BuyerPostalCode,
							PurchasingDepartment = data.BuyerPurchasingDepartment,
							Street = data.BuyerStreet,
							OrderType = (int)Enums.OrderEnums.EdiDocumentTypes.Order
						};
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.Insert(buyerDb);

						// > Extension Consignee
						var consigneeDb = new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity
						{
							Id = -1,
							OrderId = orderDb.Nr,
							DUNS = data.ConsigneeDUNS,
							City = data.ConsigneeCity,
							ContactFax = data.ConsigneeContactFax,
							ContactName = data.ConsigneeContactName,
							ContatTelephone = data.ConsigneeContactTelephone,
							CountryName = data.ConsigneeCountryName,
							Name = data.ConsigneeName,
							Name2 = data.ConsigneeName2,
							Name3 = data.ConsigneeName3,
							PartyIdentificationCodeListQualifier = data.ConsigneeIdentificationCodeListQualifier,
							PostalCode = data.ConsigneePostalCode,
							PurchasingDepartment = data.ConsigneePurchasingDepartment,
							StorageLocation = data.ConsigneeStorageLocation,
							Street = data.ConsigneeStreet,
							UnloadingPoint = data.ConsigneeUnloadingPoint,
							OrderType = (int)Enums.OrderEnums.EdiDocumentTypes.Order,
							OrderElementId = null
						};
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.Insert(consigneeDb);

						//Logging
						var insertedObject = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderDb.Nr);
						var _log = new LogHelper(insertedObject.Nr, (int)insertedObject.Angebot_Nr,
							int.TryParse(insertedObject.Projekt_Nr, out var val) ? val : 0, insertedObject.Typ, LogHelper.LogType.CREATIONOBJECT, "CTS", user)
							.LogCTS(null, null, null, 0);
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
						return new Core.Models.ResponseModel<int>(orderDb.Nr)
						{
							Success = createElementsResponse.Success,
							Errors = createElementsResponse.Errors,
						};
					} catch(Exception exception)
					{
						Infrastructure.Services.Logging.Logger.Log(exception);
						return new Core.Models.ResponseModel<int>(-1)
						{
							Errors = new List<string>() { "Exception: " + exception.Message }
						};
					}
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		private static string getUniqueDocumentName(int customerId, string documentType)
		{
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetUniqueByKundenkNr(customerId, documentType);
			if(orderDb == null)
			{
				return MANUAL_DOCUMENT_PREFIX + "1";
			}

			// > Extract and increment last Id
			var lastIdDb = orderDb.Bezug.TrimStart(MANUAL_DOCUMENT_PREFIX.ToCharArray());
			if(int.TryParse(lastIdDb, out int lastId))
			{
				return MANUAL_DOCUMENT_PREFIX + (lastId + 1);
			}

			return MANUAL_DOCUMENT_PREFIX + lastIdDb + "1";
		}
	}
}
