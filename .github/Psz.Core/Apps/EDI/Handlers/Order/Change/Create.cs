using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Change
		{
			internal static Core.Models.ResponseModel<int> Create(Models.Order.Change.CreateModel data, out int orderToBeId)
			{
				lock(Locks.OrdersLock)
				{
					orderToBeId = -1;
					try
					{
						var customerId = -1;

						var transactionManager = new Infrastructure.Services.Utils.TransactionsManager();
						transactionManager.beginTransaction();
						try
						{
							if(data == null)
							{
								return Core.Models.ResponseModel<int>.SuccessResponse();
							}

							// -
							var isECOSIO = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.IsECOSIOByDuns(data?.SenderDuns ?? "");
							var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByDunsNumber(data?.BuyerDuns);
							if(adressDb == null)
							{
								adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(int.TryParse(data.BuyerPartyIdentification, out var knummer) ? knummer : 0);
								isECOSIO = false;
							}

							if(adressDb == null)
							{
								return new Core.Models.ResponseModel<int>(customerId)
								{
									Errors = new List<string>() { "Customer number not found in Address Table" }
								};
							}

							if(adressDb.Adresstyp != 1)
							{
								return new Core.Models.ResponseModel<int>(customerId)
								{
									Errors = new List<string>() { "Address Type must be equal to '1'" }
								};
							}

							// New address correspondance data points
							var adressECOSIODb = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.GetByDuns(long.TryParse(data.BuyerDuns, out var duns) ? duns : -1);
							if(isECOSIO && adressECOSIODb == null)
							{
								return new Core.Models.ResponseModel<int>(customerId)
								{
									Errors = new List<string>()
								{
									"Customer DUNS number not found in ECOSIO Address"
								},
									Body = -1
								};
							}
							if(isECOSIO && adressECOSIODb.Werksnummer.HasValue && adressECOSIODb.Werksnummer.Value.ToString() != data.BuyerPartyIdentification.Trim())
							{
								return new Core.Models.ResponseModel<int>(customerId)
								{
									Errors = new List<string>()
								{
									"Customer Party Identification different from ECOSIO Address Werksnummer"
								},
									Body = -1
								};
							}

							if(isECOSIO && (string.IsNullOrEmpty(data.ConsigneeUnloadingPoint) || string.IsNullOrWhiteSpace(data.ConsigneeUnloadingPoint)))
							{
								return new Core.Models.ResponseModel<int>(customerId)
								{
									Errors = new List<string>()
								{
									$"Consignee Unloading Point number invalid value: '{data.ConsigneeUnloadingPoint}'"
								},
									Body = -1
								};
							}
							if(isECOSIO && (string.IsNullOrEmpty(data.ConsigneeStorageLocation) || string.IsNullOrWhiteSpace(data.ConsigneeStorageLocation)))
							{
								return new Core.Models.ResponseModel<int>(customerId)
								{
									Errors = new List<string>()
								{
									$"Consignee Storage Location Point number invalid value: '{data.ConsigneeStorageLocation}'"
								},
									Body = -1
								};
							}

							//var adressConsigneeECOSIODb = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.GetByUnloadingPoint(data.ConsigneeUnloadingPoint/*, data.ConsigneeUnloadingPoint*/);
							var adressConsigneeECOSIODb = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.GetByUnloadingPointAndStorageLocation(data.ConsigneeUnloadingPoint, data.ConsigneeStorageLocation);
							if(isECOSIO && adressConsigneeECOSIODb == null)
							{
								return new Core.Models.ResponseModel<int>(customerId)
								{
									Errors = new List<string>()
								{
									"Consignee not found in ECOSIO Address"
								},
									Body = -1
								};
							}

							var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adressDb.Nr);
							if(customerDb == null)
							{
								return new Core.Models.ResponseModel<int>(customerId)
								{
									Errors = new List<string>() { "Customer not found" }
								};
							}

							var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByBezugAndKundenNr(data.DocumentNumber,
								adressDb.Nr,
								Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION);
							if(orderDb == null)
							{
								return new Core.Models.ResponseModel<int>(customerDb.Nr)
								{
									Errors = new List<string>() { "Order not found" }
								};
							}


							// needed to save XML data
							orderToBeId = orderDb.Nr;

							if(orderDb.Typ.ToLower() != "auftragsbestätigung")
							{
								return new Core.Models.ResponseModel<int>(customerDb.Nr)
								{
									Errors = new List<string>() { "Order type is not Auftragsbestätigung" }
								};
							}

							#region > Check Items
							var orderItemsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb.Nr, false);
							var concernItemEntity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByCustomerNumber(adressDb?.Kundennummer ?? -1);
							var concernEntity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get(concernItemEntity?.ConcernId ?? -1);
							var includeBz1 = concernEntity?.IncludeDescription ?? false;

							var itemsSuppliersNumbers = data.Items
														.Where(e => !string.IsNullOrWhiteSpace(e.ItemNumber))
														.Select(e => e.ItemNumber?.Trim())
														.ToList();
							var itemsCustomersNumbers = data.Items
											.Where(e => !string.IsNullOrWhiteSpace(e.CustomerItemNumber))
											.Select(e => e.CustomerItemNumber?.Trim()?.TrimStart('0'))
											.ToList();

							var itemsDbBySuppliersNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(itemsSuppliersNumbers, transactionManager.connection, transactionManager.transaction);
							var itemsDbByCustomerNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByBezeichnung1(itemsCustomersNumbers, transactionManager.connection, transactionManager.transaction);
							var stdEdiArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(adressDb.Kundennummer ?? -1, transactionManager.connection, transactionManager.transaction)
											?? new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
							var customerConcernItem = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByCustomerNumber(adressDb?.Kundennummer ?? -1, transactionManager.connection, transactionManager.transaction);
							var customerConcern = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetWithTransaction(customerConcernItem?.ConcernId ?? -1, transactionManager.connection, transactionManager.transaction);
							var stdEdiConcernArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(
											Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetCustomerNumbersInSameConcern(adressDb?.Kundennummer ?? -1), transactionManager.connection, transactionManager.transaction)
											?? new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
							var customersConcernItems = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetListByConcernId(customerConcern.Id, transactionManager.connection, transactionManager.transaction);
							var customerIndependentArticles = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetUniversalArticles();
							var itemsErrors = new List<string>();
							var errortracks = new List<Tuple<int, int>>();

							#endregion
							foreach(var lineItem in data.Items)
							{
								var type = (Enums.OrderEnums.OrderChangeItemTypes)lineItem.Type;

								if(type == Enums.OrderEnums.OrderChangeItemTypes.NotChanged)
								{
									continue;
								}


								// - 2022-08-02
								#region >>> check XML Positions <<<
								// - 1 - Pos Number unicity
								var itemCount = data.Items.Where(x => x.PositionNumber == lineItem.PositionNumber).Count();
								if(itemCount > 1)
								{
									itemsErrors.Add($"Position {lineItem.PositionNumber} [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: changed more than once.");
								}
								// - 2 - All Pos Number should exist in Original Order - except for New Pos
								if((Enums.OrderEnums.OrderChangeItemTypes)lineItem.Type != Enums.OrderEnums.OrderChangeItemTypes.New
									&& !orderItemsDb.Exists(x => x.Position == lineItem.PositionNumber))
								{
									itemsErrors.Add($"Position {lineItem.PositionNumber} [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: not found in original order.");
									continue;
								}
								if(lineItem.UnitPriceBasis <= 0)
								{
									itemsErrors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: UnitPriceBasis " + lineItem.UnitPriceBasis + " is invalid");
								}

								if(lineItem.OrderedQuantity <= 0)
								{
									itemsErrors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: Ordered Quantity " + lineItem.OrderedQuantity + " is invalid");
								}

								if(lineItem.CurrentItemPriceCalculationNet < 0)
								{
									itemsErrors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: Current Item Price Calculation Net " + lineItem.CurrentItemPriceCalculationNet + " is invalid");
								}
								// - 2022-11-29 - check Positions only by CustomerItemNumber
								if(string.IsNullOrWhiteSpace(lineItem.CustomerItemNumber))
								{
									itemsErrors.Add($"Position {lineItem.PositionNumber} [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: invalid CustomerItemNumber value [{lineItem.PositionNumber}].");
									// -
									continue;
								}
								#endregion check XML Positions


								// - 2025-06-03 - customer-independent articles - anyone can order them.
								#region Universal article 
								if(!string.IsNullOrEmpty(lineItem.CustomerItemNumber))
								{
									if(customerIndependentArticles?.Exists(x =>
										(x.CustomerItemNumber.TrimStart('0') == lineItem.CustomerItemNumber.TrimStart('0')) ||
										(x.Bezeichnung1.TrimStart('0') == lineItem.CustomerItemNumber.TrimStart('0'))) == true)
									{
										// - The ordered article is Universal
										continue;
									}
								}
								else
								{
									if(!string.IsNullOrEmpty(lineItem.ItemNumber))
									{
										if(customerIndependentArticles?.Exists(x => x.ArtikelNummer?.ToLower() == lineItem.ItemNumber.ToLower()) == true)
										{
											// - The ordered article is Universal
											continue;
										}
									}
								}
								#endregion


								// 2023-01-20 - check StdEdi
								var stdEdi = stdEdiArticles.FirstOrDefault(x => (x.CustomerItemNumber.TrimStart('0') == lineItem.CustomerItemNumber.TrimStart('0')) ||
								(x.Bezeichnung1.TrimStart('0') == lineItem.CustomerItemNumber.TrimStart('0')));
								if(stdEdi != null)
								{
									// - we're good to go
									// - 2025-05-02 - but first set matching PSZ number if not the case - Khelil/Groetsch (ticket #)
									var lineItemNumber = lineItem.ItemNumber?.Trim()?.ToLower();
									if(!string.IsNullOrEmpty(lineItemNumber))
									{
										if(stdEdi.ArtikelNummer?.Trim()?.ToLower() != lineItemNumber)
										{
											lineItem.ItemNumber = stdEdi.ArtikelNummer;
										}
									}
									continue;
								}
								// 2023-08-09 - check StdEdi in Concern
								stdEdi = stdEdiConcernArticles.FirstOrDefault(x => FoundArticle(x, lineItem, customerConcern.IncludeDescription ?? false, customerConcern.TrimLeadingZeros ?? false));
								if(stdEdi != null)
								{
									// - we're good to go, here too
									// - 2025-05-02 - but first set matching PSZ number if not the case - Khelil/Groetsch (ticket #)
									var lineItemNumber = lineItem.ItemNumber?.Trim()?.ToLower();
									if(!string.IsNullOrEmpty(lineItemNumber))
									{
										if(stdEdi.ArtikelNummer?.Trim()?.ToLower() != lineItemNumber)
										{
											lineItem.ItemNumber = stdEdi.ArtikelNummer;
										}
									}
									continue;
								}
								if(!string.IsNullOrEmpty(lineItem.CustomerItemNumber))
								{
									var includeDesignation1 = true;
									var trimTrailingZeros = true;
									var customerNumbers = new List<int> { adressDb.Kundennummer ?? -1 };
									if(customersConcernItems?.Count > 0)
									{
										customerNumbers = customersConcernItems.Select(c => c.CustomerNumber ?? -1).ToList();
										if(customerConcern.IncludeDescription != true)
											includeDesignation1 = false;
										if(customerConcern.TrimLeadingZeros != true)
											trimTrailingZeros = false;
									}

									var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(customerNumbers, lineItem.CustomerItemNumber, transactionManager.connection, transactionManager.transaction, includeDesignation1, trimTrailingZeros);
									if(articles != null && articles.Count > 0)
									{
										itemsErrors.Add("Position " + lineItem.PositionNumber + $" CustomerItemNumber [{lineItem.CustomerItemNumber}] is not EDI standard.");
										continue;
									}
								}
								// - Search Article by PSZ number, if any
								List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> itemDbs = null;
								if(!string.IsNullOrWhiteSpace(lineItem.ItemNumber))
								{
									itemDbs = !string.IsNullOrWhiteSpace(lineItem.ItemNumber)
											? itemsDbBySuppliersNumbers.FindAll(e => e.ArtikelNummer == lineItem.ItemNumber).ToList()
											: null;
									if(!string.IsNullOrEmpty(lineItem.CustomerItemNumber) && itemDbs != null && itemDbs.Count > 0)
									{
										itemsErrors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.ItemNumber}] : CustomerItemNumber [{lineItem.CustomerItemNumber}] does not match PSZ number [{lineItem.ItemNumber}]");
									}
								}

								if(itemDbs == null || itemDbs.Count <= 0)
								{
									itemsErrors.Add("Position " + lineItem.PositionNumber + ": Article not found.");
									continue;
								}
								// Take any article after 'Obsolete' filtering
								var itemDb = itemDbs[0];
							}

							if(itemsErrors.Count > 0)
							{
								return new Core.Models.ResponseModel<int>(customerDb.Nr)
								{
									Errors = itemsErrors
								};
							}


							#region > Unvalidate Order
							// - 2022-11-24 - do not reset Order to Neu if imported from P3000 - changed my mind but validation is blocked for those later
							if(!string.IsNullOrEmpty(orderDb.EDI_Dateiname_CSV) && orderDb.EDI_Dateiname_CSV.Trim().ToLower().Substring(orderDb.EDI_Dateiname_CSV.Trim().Length - 4, 4) == ".csv")
							{
								//return new Core.Models.ResponseModel<int>(customerDb.Nr)
								//{
								//	Errors = new List<string>() { "Order file is CSV, it might be imported from P3000" }
								//};
								Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateNeuOrder(orderDb.Nr, true, transactionManager.connection, transactionManager.transaction);
							}
							else
							{
								Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateNeuOrder(orderDb.Nr, true, transactionManager.connection, transactionManager.transaction);
							}

							#region unbook Order
							if(orderDb.Gebucht == true)
							{
								Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateGebucht(orderDb.Nr, false, transactionManager.connection, transactionManager.transaction);
							}
							#endregion Order

							#endregion

							#region > Overwrite Global change
							var globalChangeStatusPendingInt = (int)Enums.OrderEnums.GlobalOrderChangeStatus.Pending;
							var globalChangeStatusOverwrittenInt = (int)Enums.OrderEnums.GlobalOrderChangeStatus.Overwritten;

							var oldOrderChangesDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.GetByOrderId(orderDb.Nr)
								.FindAll(e => e.GlobalStatus == globalChangeStatusPendingInt);
							foreach(var oldOrderChangeDb in oldOrderChangesDb)
							{
								oldOrderChangeDb.GlobalStatus = globalChangeStatusOverwrittenInt;
								oldOrderChangeDb.ActionTime = DateTime.Now;
								oldOrderChangeDb.ActionUserId = null;
								oldOrderChangeDb.ActionUsername = null;

								Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Update(oldOrderChangeDb, transactionManager.connection, transactionManager.transaction);
							}
							#endregion

							#region > Insert Order Change
							var orderChangeDb = new Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity()
							{
								Id = -1,

								GlobalStatus = (int)Enums.OrderEnums.OrderChangeItemStatus.Pending,
								OrderId = orderDb.Nr,
								Reference = orderDb.Bezug,
								DocumentName = data.DocumentName,
								DocumentNumber = data.DocumentNumber,
								Duns = data.BuyerDuns ?? data.SenderDuns,

								MessageReferenceNumber = data.MessageReferenceNumber,
								ConsigneeContactName = data.ConsigneeContactName,
								ConsigneeName = data.ConsigneeName,
								ConsigneePurchasingDepartment = data.ConsigneePurchasingDepartment,
								ConsigneeStreetPostalCode = data.ConsigneeStreet == null && data.ConsigneePostalCode == null ? null : data.ConsigneeStreet + " " + data.ConsigneePostalCode,
								ConsigneeUnloadingPoint = data.ConsigneeUnloadingPoint,

								CustomerContactName = data.BuyerContactName,
								CustomerName = data.BuyerName,
								CustomerPurchasingDepartment = data.BuyerPurchasingDepartment,
								CustomerStreetCityPostalCode = data.BuyerStreet == null && data.BuyerCity == null && data.BuyerPostalCode == null ? null : data.BuyerStreet + " " + data.BuyerCity + " " + data.BuyerPostalCode,
								CustomerStreetPostalCode = data.BuyerStreet == null && data.BuyerPostalCode == null ? null : data.BuyerStreet + " " + data.BuyerPostalCode,

								Notes = data.FreeText,

								CreationTime = DateTime.Now,

								ActionTime = DateTime.Now,
								ActionUserId = null,
								ActionUsername = null,

								// - update 2021-12-15 - expansion to covert all fields
								ConsigneePartyIdentificationCodeListQualifier = data.ConsigneeIdentificationCodeListQualifier,
								ConsigneePartyIdentification = data.ConsigneeIdentification,
								ConsigneeName2 = data.ConsigneeName2,
								ConsigneeName3 = data.ConsigneeName3,
								ConsigneeStreet = data.ConsigneeStreet,
								ConsigneeCity = data.ConsigneeCity,
								ConsigneePostalCode = data.ConsigneePostalCode,
								ConsigneeCountryName = data.ConsigneeCountryName,
								ConsigneeStorageLocation = data.ConsigneeStorageLocation,
								ConsigneeContactTelephone = data.ConsigneeContactTelephone,
								ConsigneeContactFax = data.ConsigneeContactFax,

								CustomerPartyIdentification = data.BuyerPartyIdentification,
								CustomerPartyIdentificationCodeListQualifier = data.BuyerPartyIdentificationCodeListQualifier,
								CustomerName2 = data.BuyerName2,
								CustomerName3 = data.BuyerName3,
								CustomerStreet = data.BuyerStreet,
								CustomerCity = data.BuyerCity,
								CustomerPostalCode = data.BuyerPostalCode,
								CustomerCountryName = data.BuyerCountryName,
								CustomerContactTelephone = data.BuyerContactTelephone,
								CustomerContactFax = data.BuyerContactFax,
							};
							orderChangeDb.Id = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Insert(orderChangeDb, transactionManager.connection, transactionManager.transaction);
							#endregion

							#region > Overwrite Order Items Change
							var itemChangeTypeChangedInt = (int)Enums.OrderEnums.OrderChangeItemTypes.Changed;
							var itemChangeStatusPendingInt = (int)Enums.OrderEnums.OrderChangeItemStatus.Pending;
							var itemChangeStatusOverwrittenInt = (int)Enums.OrderEnums.OrderChangeItemStatus.Overwritten;

							var changedPositionsNumbers = data.Items
								.Where(e => e.Type == itemChangeTypeChangedInt)
								.Select(e => e.PositionNumber)
								.ToList();

							var pendingItemsChangesDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.GetByOrderId(orderDb.Nr, transactionManager.connection, transactionManager.transaction)
								.FindAll(e => e.Type == itemChangeTypeChangedInt
									&& e.Status == itemChangeStatusPendingInt
									&& changedPositionsNumbers.Contains(e.PositionNumber));
							foreach(var pendingItemChangeDb in pendingItemsChangesDb)
							{
								pendingItemChangeDb.Status = itemChangeStatusOverwrittenInt;
								pendingItemChangeDb.ActionTime = DateTime.Now;
								pendingItemChangeDb.ActionUserId = null;
								pendingItemChangeDb.ActionUsername = null;

								Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.Update(pendingItemChangeDb, transactionManager.connection, transactionManager.transaction);
							}
							#endregion

							#region > Insert Order Items Changes
							foreach(var itemData in data.Items)
							{
								// - 2024-04-27 - EDI Concern
								var itemDb = stdEdiArticles?.FirstOrDefault(x => x.CustomerItemNumber == itemData.CustomerItemNumber
								|| (includeBz1 == true && x.Bezeichnung1 == itemData.CustomerItemNumber));

								// - if not found in Customer's articles, look for in Concerne's articles
								if(itemDb == null)
								{
									itemDb = stdEdiConcernArticles?.FirstOrDefault(x => x.CustomerItemNumber == itemData.CustomerItemNumber
									|| (includeBz1 == true && x.Bezeichnung1 == itemData.CustomerItemNumber));
								}

								// - 2022-11-29 - check Positions only by CustomerItemNumber NOT PSZ#
								//var itemDb = itemsDbByCustomerItemNumbers.FirstOrDefault(e =>
								//			e.Bezeichnung1?.ToLower().Trim().Contains(itemData.CustomerItemNumber?.ToLower().Trim()?.TrimStart('0')) == true
								//			|| e.CustomerItemNumber?.ToLower().Trim().Contains(itemData.CustomerItemNumber?.ToLower().Trim()?.TrimStart('0')) == true);
								//var itemDb = !string.IsNullOrWhiteSpace(itemData.ItemNumber)
								//	? itemsDbBySuppliersNumbers.Find(e => e.ArtikelNummer == itemData.ItemNumber)
								//	: null;
								//if (itemDb == null)
								//{
								//	itemDb = !string.IsNullOrEmpty(itemData.CustomerItemNumber)
								//		? itemsDbByCustomerNumbers.Find(e => e.Bezeichnung1?.ToLower().Trim().Contains(itemData.CustomerItemNumber?.ToLower().Trim()?.TrimStart('0')) == true)
								//		: null;
								//}

								// - 2024-04-28 add change even if artitem not found
								//if(itemDb == null)
								//{
								//	Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "itemDb == null");
								//	continue;
								//}

								var orderChangeItemDb = new Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity()
								{
									Id = -1,

									OrderReference = orderDb.Bezug,
									Type = itemData.Type,
									Status = (int)Enums.OrderEnums.OrderChangeItemStatus.Pending,

									OrderChangeId = orderChangeDb.Id,
									OrderId = orderDb.Nr,

									ActionTime = null,
									ActionUserId = null,
									ActionUsername = null,

									CurrentItemPriceCalculationNet = itemData.CurrentItemPriceCalculationNet,
									CustomerItemNumber = itemData.CustomerItemNumber,
									DesiredDate = itemData.DesiredDate,
									ItemDescription = itemData.ItemDescription,
									ItemNumber = itemData.ItemNumber, // - 2022-12-14 - Reil - ignore PSZ# change and keep order's orignal Artikel
									OrderedQuantity = itemData.OrderedQuantity,
									MeasureUnitQualifier = itemData.MeasureUnitQualifier,
									PositionNumber = itemData.PositionNumber,
									UnitPriceBasis = itemData.UnitPriceBasis,

									Notes = itemData.FreeText,

									CreationTime = DateTime.Now,
									LineItemAmount = itemData.LineItemAmount
								};

								Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.Insert(orderChangeItemDb, transactionManager.connection, transactionManager.transaction);
							}
							#endregion

							#region > Buyer Extension
							var buyerDb = new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity
							{
								Id = -1,
								OrderId = orderDb.Nr,
								DUNS = data.BuyerDuns?.ToString(),
								Name = isECOSIO ? adressECOSIODb.Firma : data.BuyerName,
								Name2 = isECOSIO ? "" : data.BuyerName2, //adressECOSIODb.Bezeichnung, // >>>>> Ignore !
								Name3 = isECOSIO ? "" : data.BuyerName3,
								City = isECOSIO ? adressECOSIODb.ROrt : data.BuyerCity,
								ContactFax = data.BuyerContactFax,
								ContactName = isECOSIO ? "" : data.BuyerContactName, // >>>>> Ignore !
								ContactTelephone = data.BuyerContactTelephone,
								CountryName = data.BuyerCountryName,
								PartyIdentification = data.BuyerPartyIdentification,
								PartyIdentificationCodeListQualifier = data.BuyerPartyIdentificationCodeListQualifier,
								PostalCode = isECOSIO ? adressECOSIODb.RPLZ : data.BuyerPostalCode,
								PurchasingDepartment = data.BuyerPurchasingDepartment,
								Street = isECOSIO ? adressECOSIODb.RStrasse : data.BuyerStreet,
								OrderType = (int)Enums.OrderEnums.OrderTypes.OrderChange
							};

							Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.Insert(buyerDb, transactionManager.connection, transactionManager.transaction);
							#endregion

							#region > Consignee Extension
							var consigneeDb = new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity
							{
								Id = -1,
								OrderId = orderDb.Nr,
								DUNS = isECOSIO ? adressECOSIODb.DUNSNummer?.ToString() : data.ConsigneeDUNS, // Set Buyer DUNS to Consignee
								City = isECOSIO ? adressConsigneeECOSIODb.LOrt : data.ConsigneeCity,
								ContactFax = data.ConsigneeContactFax,
								ContactName = data.ConsigneeContactName,
								ContatTelephone = data.ConsigneeContactTelephone,
								CountryName = data.ConsigneeCountryName,
								Name = isECOSIO ? adressConsigneeECOSIODb.Firma : data.ConsigneeName,
								Name2 = isECOSIO ? adressConsigneeECOSIODb.Bezeichnung : data.ConsigneeName2,
								Name3 = isECOSIO ? "" : data.ConsigneeName3,
								PartyIdentificationCodeListQualifier = data.ConsigneeIdentificationCodeListQualifier,
								PostalCode = isECOSIO ? adressConsigneeECOSIODb.LPLZ : data.ConsigneePostalCode,
								PurchasingDepartment = data.ConsigneePurchasingDepartment,
								StorageLocation = data.ConsigneeStorageLocation,
								Street = isECOSIO ? adressConsigneeECOSIODb.LStrasse : data.ConsigneeStreet,
								UnloadingPoint = data.ConsigneeUnloadingPoint,
								OrderType = (int)Enums.OrderEnums.OrderTypes.OrderChange
							};

							Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.Insert(consigneeDb, transactionManager.connection, transactionManager.transaction);
							#endregion

							if(transactionManager.commit())
							{

								return Core.Models.ResponseModel<int>.SuccessResponse(customerDb.Nr);
							}
							else
							{
								return Core.Models.ResponseModel<int>.FailureResponse("Transaction error");
							}
						} catch(Exception exception)
						{
							transactionManager.rollback();
							Infrastructure.Services.Logging.Logger.LogTrace($"Data >>> data:{Newtonsoft.Json.JsonConvert.SerializeObject(data)} >> orderToBeId:{orderToBeId}");
							Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
							return new Core.Models.ResponseModel<int>(customerId)
							{
								Errors = new List<string>() { "Exception: " + exception.Message }
							};
						}
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.LogTrace($"Data >>> data:{Newtonsoft.Json.JsonConvert.SerializeObject(data)} >> orderToBeId:{orderToBeId}");
						Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
						throw;
					}
				}
			}
			private static bool FoundArticle(Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity articleEntity, Models.Order.Change.CreateItemModel lineItem,
			bool includeDescription1, bool trimLeadingZeros)
			{
				//true - true
				if(includeDescription1 && trimLeadingZeros)
				{
					return articleEntity.CustomerItemNumber.TrimStart('0') == lineItem.CustomerItemNumber.TrimStart('0')
						|| articleEntity.Bezeichnung1.TrimStart('0') == lineItem.CustomerItemNumber.TrimStart('0');
				}
				// true - false
				if(includeDescription1 && !trimLeadingZeros)
				{
					return articleEntity.CustomerItemNumber == lineItem.CustomerItemNumber
						|| articleEntity.Bezeichnung1 == lineItem.CustomerItemNumber;
				}
				// false - true
				if(!includeDescription1 && trimLeadingZeros)
				{
					return articleEntity.CustomerItemNumber.TrimStart('0') == lineItem.CustomerItemNumber.TrimStart('0');
				}
				// false - false
				if(!includeDescription1 && !trimLeadingZeros)
				{
					return articleEntity.CustomerItemNumber == lineItem.CustomerItemNumber;
				}

				return false;
			}
		}
	}
}