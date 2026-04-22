using Newtonsoft.Json;
using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		private static string ManualDocumentName = "manual-creation";
		public static string ManaulDocumentPrefix = "mc-";

		public static Core.Models.ResponseModel<int> Create(Models.Order.CreateModel data,
		   Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return CreateInternal(data, user, out var id);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static Core.Models.ResponseModel<int> CreateInternal(Models.Order.CreateModel data,
		   Core.Identity.Models.UserModel user,
		   out int orderToBeId)
		{
			orderToBeId = -1;

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				var createElementsResponse = new Core.Models.ResponseModel<List<int>>();
				var customerId = 0;
				#region // -- transaction-based logic -- //
				//-

				lock(Locks.OrdersLock)
				{
					try
					{
						var isECOSIO = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.IsECOSIOByDuns(data?.SenderDuns ?? "", botransaction.connection, botransaction.transaction); // Only HORSCH
						var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByDunsNumber(data?.BuyerDuns, botransaction.connection, botransaction.transaction);
						if(adressDb == null)
						{
							adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(int.TryParse(data.BuyerPartyIdentification, out var knummer) ? knummer : 0, botransaction.connection, botransaction.transaction);
							isECOSIO = false;
						}

						if(adressDb == null)
						{
							return new Core.Models.ResponseModel<int>(customerId)
							{
								Errors = new List<string>()
								{
									"Customer number not found in Address Table"
								},
								Body = -1
							};
						}

						if(adressDb.Adresstyp != 1)
						{
							return new Core.Models.ResponseModel<int>(customerId)
							{
								Errors = new List<string>()
								{
									"Address Type must be equal to '1'"
								}
							};
						}

						// New address correspondance data points
						var adressECOSIODb = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.GetByDuns(long.TryParse(data.BuyerDuns, out var duns) ? duns : -1, botransaction.connection, botransaction.transaction);
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

						var adressConsigneeECOSIODb = Infrastructure.Data.Access.Tables.PRS.AdresseECOSIOAccess.GetByUnloadingPointAndStorageLocation(data.ConsigneeUnloadingPoint, data.ConsigneeStorageLocation, botransaction.connection, botransaction.transaction);
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

						var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adressDb.Nr, botransaction.connection, botransaction.transaction);
						if(customerDb == null)
						{
							return new Core.Models.ResponseModel<int>(customerId)
							{
								Errors = new List<string>() { "Customer not found" }
							};
						}
						// - 2025-11-28 - PS
						if(Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.IsArchived(customerDb.Nr))
						{
							return Core.Models.ResponseModel<int>.FailureResponse($"Customer is archived");
						}

						if(!data.IsManualCreation)
						{
							// > Check if DocumentNumber Exists if new Order
							var orderDbByUniqueNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByBezugAndKundenNr(data.DocumentNumber,
								adressDb.Nr,
								Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION, botransaction.connection, botransaction.transaction);

							// >>>>>> Logging
							Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace,
								$" OrderImport >>>>>> DocumentNumber:{data.DocumentNumber} || adressDb.Nr:{adressDb.Nr} || orderDbByUniqueNumber:{JsonConvert.SerializeObject(orderDbByUniqueNumber)}");

							if(orderDbByUniqueNumber != null)
							{
								orderToBeId = orderDbByUniqueNumber.Nr;
								return new Core.Models.ResponseModel<int>(customerDb.Nr)
								{
									Success = false,
									Errors = new List<string>() { "Document Exists" },
								};
							}

							// - 2022-10-04 - JH reject doc w/ multiple DIFFERENT Consignees in Position
							if(data.Elements != null && data.Elements.Count > 1)
							{
								var firstPosConsignee = data.Elements[0].Consignee;
								for(int i = 1; i < data.Elements.Count; i++)
								{
									if(firstPosConsignee?.ConsigneeName?.Trim()?.ToLower() != data.Elements[i].Consignee?.ConsigneeName?.Trim()?.ToLower())
									{
										return new Core.Models.ResponseModel<int>(customerDb.Nr)
										{
											Success = false,
											Errors = new List<string>() { $"Position {data.Elements[i].PositionNumber}: Different consignees in Positions" },
										};
									}
								}
							}
						}

						customerId = customerDb.Nr;

						#region > Check Items
						var itemsSuppliersNumbers = data.Elements
							.Where(e => !string.IsNullOrWhiteSpace(e.ItemNumber))
							.Select(e => e.ItemNumber?.Trim())
							.ToList();
						var itemsCustomersNumbers = data.Elements
							.Where(e => !string.IsNullOrWhiteSpace(e.CustomerItemNumber))
							.Select(e => e.CustomerItemNumber?.Trim()?.TrimStart('0'))
							.ToList();

						var itemsDbBySuppliersNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(itemsSuppliersNumbers, botransaction.connection, botransaction.transaction);
						var itemsDbByCustomerNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByBezeichnung1(itemsCustomersNumbers, botransaction.connection, botransaction.transaction);
						var stdEdiArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(adressDb.Kundennummer ?? -1, botransaction.connection, botransaction.transaction)
							?? new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
						var customerConcernItem = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByCustomerNumber(adressDb?.Kundennummer ?? -1, botransaction.connection, botransaction.transaction);
						var customerConcern = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetWithTransaction(customerConcernItem?.ConcernId ?? -1, botransaction.connection, botransaction.transaction);

						var stdEdiConcernArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(
							Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetCustomerNumbersInSameConcern(adressDb?.Kundennummer ?? -1), botransaction.connection, botransaction.transaction)
							?? new List<Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity>();
						var customersConcernItems = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetListByConcernId(customerConcern?.Id ?? -1, botransaction.connection, botransaction.transaction);
						var customerIndependentArticles = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetUniversalArticles();
						var itemsErrors = new List<string>();
						var errortracks = new List<Tuple<int, int>>();
						foreach(var lineItem in data.Elements)
						{
							// - 2025-06-11
							#region checks on XML data
							if(lineItem.UnitPriceBasis <= 0)
							{
								if(!errortracks.Exists(x => x.Item1 == lineItem.PositionNumber && x.Item2 == 1))
								{
									errortracks.Add(new Tuple<int, int>(lineItem.PositionNumber, 1));
									itemsErrors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: UnitPriceBasis " + lineItem.UnitPriceBasis + " is invalid");
								}
							}

							if(lineItem.OrderedQuantity <= 0)
							{
								if(!errortracks.Exists(x => x.Item1 == lineItem.PositionNumber && x.Item2 == 2))
								{
									errortracks.Add(new Tuple<int, int>(lineItem.PositionNumber, 2));
									itemsErrors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: Ordered Quantity " + lineItem.OrderedQuantity + " is invalid");
								}
							}

							if(lineItem.CurrentItemPriceCalculationNet < 0)
							{
								if(!errortracks.Exists(x => x.Item1 == lineItem.PositionNumber && x.Item2 == 3))
								{
									errortracks.Add(new Tuple<int, int>(lineItem.PositionNumber, 3));
									itemsErrors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: Current Item Price Calculation Net " + lineItem.CurrentItemPriceCalculationNet + " is invalid");
								}
							}
							#endregion

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
							var stdEdi = stdEdiArticles.FirstOrDefault(x => (x.CustomerItemNumber.TrimStart('0') == lineItem.CustomerItemNumber?.TrimStart('0')) ||
							(x.Bezeichnung1.TrimStart('0') == lineItem.CustomerItemNumber?.TrimStart('0')));
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
							stdEdi = stdEdiConcernArticles?.FirstOrDefault(x => FoundArticle(x, lineItem, customerConcern.IncludeDescription ?? false, customerConcern.TrimLeadingZeros ?? false));
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

								var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(customerNumbers, lineItem.CustomerItemNumber, botransaction.connection, botransaction.transaction, includeDesignation1, trimTrailingZeros);
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
								if(!errortracks.Exists(x => x.Item1 == lineItem.PositionNumber && x.Item2 == 0))
								{
									errortracks.Add(new Tuple<int, int>(lineItem.PositionNumber, 0));
									itemsErrors.Add("Position " + lineItem.PositionNumber + $" [{lineItem.ItemNumber} | {lineItem.CustomerItemNumber}]: Article not found.");
								}

								continue;
							}

						}

						if(itemsErrors.Count > 0)
						{
							return new Core.Models.ResponseModel<int>(customerId)
							{
								Errors = itemsErrors,
								Body = customerDb.Nr
							};
						}
						#endregion

						var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
							? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value, botransaction.connection, botransaction.transaction)
							: null;

						var orderDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity()
						{
							Bezug = data.IsManualCreation ? getUniqueDocumentName(adressDb.Nr, Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION, botransaction) : data.DocumentNumber,
							EDI_Dateiname_CSV = data.IsManualCreation ? ManualDocumentName : data.DocumentName,

							ABSENDER = data.BuyerName,
							Kunden_Nr = adressDb.Nr,
							Typ = "Auftragsbestätigung",
							Mandant = "PSZ electronic",
							EDI_Order_Neu = true,

							Vorname_NameFirma = isECOSIO ? adressECOSIODb.Firma : adressDb.Name1, //data.BuyerName,
							Name2 = isECOSIO ? "" : adressDb.Name2, /*adressECOSIODb.Bezeichnung*/ // >>>>> Ignore !?!
							Name3 = isECOSIO ? "" : adressDb.Name3,

							Ansprechpartner = isECOSIO ? "" : data.BuyerContactName, // >>>>> ECOSIO table correspondance ??? -- FIXME: IGNORE Contact & Department on HORSCH
							Abteilung = isECOSIO ? "" : data.BuyerPurchasingDepartment,
							Straße_Postfach = isECOSIO
								? adressECOSIODb.RStrasse
								: (string.IsNullOrWhiteSpace($"{data.BuyerStreet}{data.BuyerName2}")
									? $"{adressDb.StraBe}"
									: $"{data.BuyerStreet} / {data.BuyerName2}"?.Trim()?.TrimStart('/')?.TrimEnd('/')),
							Land_PLZ_Ort = isECOSIO
								? $"{adressECOSIODb.RPLZ?.Trim()} {adressECOSIODb.ROrt?.Trim()}".Trim(',')
								: (string.IsNullOrWhiteSpace($"{data.BuyerCountryName}{data.BuyerPostalCode}{data.BuyerCity}")
									? $"{adressDb.PLZ_StraBe} {adressDb.Ort}"
									: $"{data.BuyerCountryName} - {data.BuyerPostalCode} {data.BuyerCity}"?.Trim()?.Trim('-')),

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

							Freitext = String.IsNullOrWhiteSpace(data.FreeText) ? $"USt-ID-Nr.: {customerDb.EG___Identifikationsnummer}" : data.FreeText,
							Freie_Text = String.IsNullOrWhiteSpace(data.FreeText) ? $"" : data.FreeText,

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
							LVorname_NameFirma = isECOSIO ? adressConsigneeECOSIODb.Firma : data.ConsigneeName,
							LName2 = isECOSIO ? "" : data.ConsigneeName2, //adressConsigneeECOSIODb.Bezeichnung, //"", // >>>>> Ignore !
							LName3 = isECOSIO ? "" : data.ConsigneeName3,
							LAnsprechpartner = isECOSIO ? "" : data.ConsigneeContactName, // >>>>> Ignore !
							LAbteilung = data.ConsigneePurchasingDepartment,
							LStraße_Postfach = isECOSIO ? $"{adressConsigneeECOSIODb.LStrasse?.Trim()}" : data.ConsigneeStreet,
							LLand_PLZ_Ort = isECOSIO ? $"{adressConsigneeECOSIODb.LPLZ?.Trim()} {adressConsigneeECOSIODb.LOrt?.Trim()}".Trim() : $"{data.ConsigneePostalCode} {data.ConsigneeCity} {data.ConsigneeCountryName}".Trim(),
							LBriefanrede = "Sehr geehrte Damen und Herren",
							StorageLocation = isECOSIO ? adressConsigneeECOSIODb.AnlieferLagerort?.ToString() : data.ConsigneeStorageLocation,
							UnloadingPoint = isECOSIO ? adressConsigneeECOSIODb.Werksnummer?.ToString() : data.ConsigneeUnloadingPoint,

							Neu_Order = true,

							Angebot_Nr = 0, // < new
							Erledigt = false,

							// - Ridha 2021-07-01
							Nr_ang = 0,
							Anrede = "Firma",
							LsAddressNr = customerDb.LSADR, // - REM: 2025-03-25 handle Ecosio address
						};


						orderDb.Nr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.InsertWithTransaction(orderDb, botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateAbIdWithTransaction(orderDb.Nr, orderDb.Nr, botransaction.connection, botransaction.transaction);

						orderToBeId = orderDb.Nr;

						data.Elements.ForEach(e => e.UnloadingPoint = data.ConsigneeUnloadingPoint);

						var createElementsRequestData = new Models.Order.Element.NotCalculatedOrderElementsModel()
						{
							OrderId = orderDb.Nr,
							Elements = data.Elements
						};

						// > Main Extention
						var orderExtensionDb = new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity()
						{
							Id = -1,
							Version = 0,
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = (user?.Id ?? -1),
							LastUpdateUsername = user?.Username ?? "-",
							OrderId = orderDb.Nr,
							EdiValidationTime = DateTime.Now,
							EdiValidationUserId = (user?.Id ?? -1),
							SenderDuns = data.SenderDuns?.ToString(),

							SenderId = data.SenderId,
							RecipientId = data.RecipientId,


						};
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.InsertWithTRansaction(orderExtensionDb, botransaction.connection, botransaction.transaction);

						// > Insert Buyer Extension
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
							OrderType = (int)Enums.OrderEnums.OrderTypes.Order
						};
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.Insert(buyerDb, botransaction.connection, botransaction.transaction);

						// - 2022-07-14 - JH - set Standard Delivery Address from Customer Module if Consignee is missing
						///<param name="MissingConsignee"> --- </param>
						var standardDeliveryAddress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.LSADR ?? -1, botransaction.connection, botransaction.transaction);
						if(string.IsNullOrWhiteSpace(data.ConsigneeDUNS)
							|| string.IsNullOrWhiteSpace(data.ConsigneeName))
						{
							if(standardDeliveryAddress != null && !string.IsNullOrWhiteSpace(standardDeliveryAddress.Duns))
							{
								data.ConsigneeDUNS = standardDeliveryAddress?.Duns;
								data.ConsigneeCity = standardDeliveryAddress?.Ort;
								data.ConsigneeContactFax = standardDeliveryAddress?.Fax;
								data.ConsigneeContactName = standardDeliveryAddress?.Vorname;
								data.ConsigneeContactTelephone = standardDeliveryAddress?.Telefon;
								data.ConsigneeCountryName = standardDeliveryAddress?.Land;
								data.ConsigneeName = standardDeliveryAddress?.Name1;
								data.ConsigneeName2 = standardDeliveryAddress?.Name2;
								data.ConsigneeName3 = standardDeliveryAddress?.Name3;
								data.ConsigneeIdentificationCodeListQualifier = ""; //standardDeliveryAddress?.ConsigneeIdentificationCodeListQualifier;
								data.ConsigneePostalCode = standardDeliveryAddress?.PLZ_Postfach;
								data.ConsigneePurchasingDepartment = standardDeliveryAddress?.Abteilung;
								data.ConsigneeStorageLocation = ""; // standardDeliveryAddress?.ConsigneeStorageLocation;
								data.ConsigneeStreet = standardDeliveryAddress?.PLZ_StraBe;
								data.ConsigneeUnloadingPoint = "-1"; //REM:link to missing Consignee! //standardDeliveryAddress?.ConsigneeUnloadingPoint;
							}
						}

						// > Insert Consignee Extension
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
							OrderType = (int)Enums.OrderEnums.OrderTypes.Order,
							OrderElementId = null
						};
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.Insert(consigneeDb, botransaction.connection, botransaction.transaction);

						// > Insert Items
						createElementsResponse = Element.CreateOrderElementsInternal(createElementsRequestData, adressDb.Kundennummer ?? -1, user, botransaction);
						//logging
						var Log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var v) ? v : 0, orderDb.Typ, LogHelper.LogType.CREATIONOBJECT, "EDI", user)
							.LogCTS(null, null, null, 0);
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(Log, botransaction.connection, botransaction.transaction);

					} catch(Exception exception)
					{
						Infrastructure.Services.Logging.Logger.LogTrace($"Data >>> data:{Newtonsoft.Json.JsonConvert.SerializeObject(data)} >> orderToBeId:{orderToBeId}");
						Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
						return new Core.Models.ResponseModel<int>(customerId)
						{
							Errors = new List<string>() { "Exception: " + exception.Message }
						};
					}
				}

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return new Core.Models.ResponseModel<int>(customerId)
					{
						Success = createElementsResponse.Success,
						Errors = createElementsResponse.Errors,
					};
				}
				else
				{
					return Core.Models.ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private static bool FoundArticle(Infrastructure.Data.Entities.Tables.PRS.MinimalArtikelEntity articleEntity, Models.Order.Element.NotCalculatedElementModel lineItem,
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
		private static string getUniqueDocumentName(int kundenkNr, string documentType, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetUniqueByKundenkNr(kundenkNr, documentType, botransaction.connection, botransaction.transaction);
			if(orderDb == null)
			{
				return ManaulDocumentPrefix + "1";
			}

			// Extract and increment last Id
			var lastIdDb = orderDb.Bezug.TrimStart(ManaulDocumentPrefix.ToCharArray());
			if(int.TryParse(lastIdDb, out int lastId))
			{
				return ManaulDocumentPrefix + (lastId + 1);
			}

			return ManaulDocumentPrefix + lastIdDb + "1";
		}
	}
}
