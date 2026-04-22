using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		// - 
		public static int generateDay()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // - rebuild ORDRSP -
				//var orders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(new List<int> { 1008035, 1007974, 1006731, 1000195, 1000164, 1000163, 997618, 997617, 997616, 997615, 997613, 997606, 997605, 997604, 996907, 996901, 996630, 993182, 993181, 993180, 993179 });
				//var orders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(new List<int> { 1006934 , 1004893 , 1004874 });

				// - 2023-04-20 - Reil - 5222587415,5222592834,5222596602,5222596604,5222596606,5222596610,5222596614
				//var orders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(new List<int> { 1050017, 1053051, 1055375, 1055377, 1055379, 1055385, 1055389 });
				//foreach(var item in orders)
				//{
				//	GenerateOrderResponse(item, botransaction);
				//}

				//// - 2023-05-11 - Ecosio.Katzensteiner - 2643753 ,2643067 ,2643750 
				//var orders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(new List<int> { 942022, 934359, 940068 });
				//foreach(var item in orders)
				//{
				//	GenerateOrderResponse(item, botransaction);
				//}

				//// - 2023-07-11 - Townsend/Ecosio - 4501550556
				//var orders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(new List<int> { 1056299 });
				//foreach(var item in orders)
				//{
				//	GenerateOrderResponse(item, botransaction);
				//}
				#endregion


				#region // REM - 2023-04-18 - regenerate Horsch data to correct Bz1 was StartTrimmed 0 
				//var orders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetHorschOrdrsp();
				//foreach(var item in orders)
				//{
				//	GenerateOrderResponse(item, botransaction);
				//}
				#endregion



				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return 1;
				}
				else
				{
					return 0;
				}
				//return 0;
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<object> GenerateOrderResponse(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity orderDb, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			try
			{
				var addressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)orderDb.Kunden_Nr, botransaction.connection, botransaction.transaction);
				if(addressDb == null)
				{
					return new Core.Models.ResponseModel<object>(orderDb.Nr)
					{
						Errors = new List<string>()
						{
							"Customer number not found in Address Table"
						}
					};
				}

				var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(orderDb.Nr, botransaction.connection, botransaction.transaction);
				var orderItemsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetOriginalByAngeboteNr(orderDb.Nr, botransaction.connection, botransaction.transaction); // <<<<< This is already filtered to send back only Primary Positions
				var buyerDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrderType(orderDb.Nr, (int)Enums.OrderEnums.OrderTypes.Order, botransaction.connection, botransaction.transaction);
				var consigneesDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.GetByOrderType(orderDb.Nr, (int)Enums.OrderEnums.OrderTypes.Order, botransaction.connection, botransaction.transaction);
				var headerConsigneeDb = consigneesDb?.FindAll(c => c?.OrderElementId == null)?.FirstOrDefault();
				var splitPositionsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetSplitPositionByAngeboteNr(orderDb.Nr, botransaction.connection, botransaction.transaction);

				#region > Get and Update Items Extensions data
				foreach(var orderItemDb in orderItemsDb)
				{
					OrderElementExtension.SetStatus(orderItemDb.Nr, botransaction);
				}

				var orderItemsExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderId(orderDb.Nr, botransaction.connection, botransaction.transaction);
				#endregion

				var responseErrors = new List<string>();
				var orderResponseContent = new Psz.Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage();

				var currentUtcDateTime = DateTime.UtcNow;
				var berlinZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // >>> Berlin time zone
				var berlinDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcDateTime, berlinZone);  // >>>>>>>>>> Extension ValidationTime

				#region > Header
				orderResponseContent.ErpelBusinessDocumentHeader = new Psz.Core.Apps.EDI.Models.OrderResponse.ErpelBusinessDocumentHeader
				{
					MessageReceivedAt = berlinDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
					InterchangeHeader = new Models.OrderResponse.InterchangeHeader
					{
						Sender = new Models.OrderResponse.Sender
						{
							Id = orderExtensionDb?.RecipientId
						},
						Recipient = new Models.OrderResponse.Recipient
						{
							Id = orderExtensionDb?.SenderDuns
						},
						DateTime = new Models.OrderResponse.DateTime
						{
							Date = berlinDateTime.ToString("yyyy-MM-dd"),
							Time = berlinDateTime.ToString("HH:mm:ss")
						}
					}
				};
				#endregion

				#region > Document
				orderResponseContent.Document = GetOrderResponseDocument();

				// Footer variables
				decimal invoiceAmount = 0;
				decimal totalTaxableAmount = 0;

				// >>>>> Document >>> Header
				orderResponseContent.Document.Header = new Models.OrderResponse.Header
				{
					MessageHeader = new Models.OrderResponse.MessageHeader
					{
						MessageReferenceNumber = "1", // >>> Facultatif: numéro croissant de Message 1-2-3... / Par commande 
						MessageType = "ORDRSP" // FIXED
					},
					BeginningOfMessage = new Models.OrderResponse.BeginningOfMessage
					{
						DocumentNameEncoded = "231", // FIXED
						DocumentNumber = orderDb.Projekt_Nr, // Numéro de document de la confirmation de commande  "Projekt-Nr"  dans le Tableau Angebote // >>> $"{orderDb.Bezug}-{((DateTime)orderDb.Datum).ToString("yyyyMMdd")}" // FIXME: >>> ????? >>>
					},
					Dates = new Models.OrderResponse.Dates
					{
						DocumentDate = new Models.OrderResponse.DocumentDate
						{
							DateQualifier = "4", // FIXED
							DateTime2 = berlinDateTime.ToString("yyyy-MM-ddTHH:mm:ss")
						}
					},
					ReferenceCurrency = "EUR", // FIXED
					ReferencedDocuments = getReferencedDocuments(orderDb),
					BusinessEntities = new Models.OrderResponse.BusinessEntities
					{
						Buyer = new Models.OrderResponse.Buyer
						{
							PartyQualifier = "BY", // FIXED
							PartyIdentification = isValidString(orderDb.Unser_Zeichen)
								? orderDb.Unser_Zeichen //.PadLeft(10, '0')
								: null,
							PartyIdentificationCodeListQualifier = "ZZZ", // FIXED
							DUNS = buyerDb?.DUNS,
							PartyName = new List<string> { },
							Street = isValidString(buyerDb?.Street) ? buyerDb?.Street : null,
							City = isValidString(buyerDb?.City) ? buyerDb?.City : null,
							PostCode = isValidString(buyerDb?.PostalCode) ? buyerDb?.PostalCode : null,
							Country = isValidString(buyerDb?.CountryName)
								? new Models.OrderResponse.Country { CountryName = buyerDb?.CountryName, }
								: null,
							Contact = (isValidString(buyerDb?.ContactFax) || isValidString(buyerDb?.ContactName) || isValidString(buyerDb?.ContactTelephone))
								? new Models.OrderResponse.Contact
								{
									Name = isValidString(buyerDb?.ContactName) ? buyerDb?.ContactName : null,
									Telephone = isValidString(buyerDb?.ContactTelephone) ? buyerDb?.ContactTelephone : null,
									Fax = isValidString(buyerDb?.ContactFax) ? buyerDb?.ContactFax : null,
								}
								: null,
						},
						Supplier = new Models.OrderResponse.Supplier
						{
							PartyQualifier = "SU", // FIXED
							PartyIdentification = isValidString(orderDb.Ihr_Zeichen) ? orderDb.Ihr_Zeichen : null,
							PartyIdentificationCodeListQualifier = "ZZZ", // FIXED
							DUNS = "332358188" // FIXED: Numero DUNS PSZ 
						},
						Consignee = getConsignee(headerConsigneeDb)
					}
				};

				// - Buyer PartyName
				if(isValidString(buyerDb?.Name)
				|| isValidString(buyerDb?.Name2)
				|| isValidString(buyerDb?.Name3))
				{
					orderResponseContent.Document.Header.BusinessEntities.Buyer.PartyName = new List<string>();
					if(isValidString(buyerDb?.Name))
					{
						orderResponseContent.Document.Header.BusinessEntities.Buyer.PartyName.Add(buyerDb?.Name);
					}
					if(isValidString(buyerDb?.Name2))
					{
						orderResponseContent.Document.Header.BusinessEntities.Buyer.PartyName.Add(buyerDb?.Name2);
					}
					if(isValidString(buyerDb?.Name3))
					{
						orderResponseContent.Document.Header.BusinessEntities.Buyer.PartyName.AddRange(buyerDb?.Name3.Split("||").ToList());
					}
				}


				// >>>>> Document >>> Details
				if(orderItemsDb != null && orderItemsDb.Count > 0)
				{
					orderResponseContent.Document.Details.OrdersData.OrdersLineItem = new List<Models.OrderResponse.OrdersLineItem>();

					foreach(var orderItemEntity in orderItemsDb)
					{
						var itemDb = orderItemEntity.ArtikelNr.HasValue
							? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntity.ArtikelNr.Value)
							: null;
						var itemSplitPositionDb = splitPositionsDb?.FindAll(e => e?.PositionZUEDI == orderItemEntity.Nr)
							?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
						var consigneeItem = consigneesDb?.FindAll(e => e?.OrderElementId != null && (int)e.OrderElementId == orderItemEntity.Nr).FirstOrDefault();
						var itemExtentionEntity = orderItemsExtensionsDb?.FirstOrDefault(e => e.OrderItemId == orderItemEntity.Nr);

						orderResponseContent.Document.Details.OrdersData.OrdersLineItem.Add(getOrderLineItem(orderDb,
							itemExtentionEntity,
							orderItemEntity,
							itemDb,
							itemSplitPositionDb,
							consigneeItem));

						var allowanceChargeAmount = (orderItemEntity.Gesamtkupferzuschlag ?? 0m) + splitPositionsDb.Sum(e => (e.Gesamtkupferzuschlag ?? 0m));

						totalTaxableAmount += (orderItemEntity.Gesamtpreis ?? 0m)
							+ splitPositionsDb.Sum(e => (e.Gesamtpreis ?? 0m));
						invoiceAmount += ((orderItemEntity.Gesamtpreis ?? 0m) * (1 + orderItemEntity.USt ?? 0))
							+ splitPositionsDb.Sum(e => (e.Gesamtpreis ?? 0m) * (1 + e.USt ?? 0));
					}
				}

				// >>>>> Document >>> Footer
				orderResponseContent.Document.Footer.InvoiceFooter.InvoiceTotals.TotalTaxableAmount = totalTaxableAmount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture).ToString(System.Globalization.CultureInfo.InvariantCulture);
				orderResponseContent.Document.Footer.InvoiceFooter.InvoiceTotals.InvoiceAmount = invoiceAmount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture).ToString(System.Globalization.CultureInfo.InvariantCulture);
				#endregion >>>>> Document

				var filePath = System.IO.Path.Combine(Psz.Core.Program.EdiOrderResponseArchiveDiretory, DateTime.Today.ToString("dd.MM.yyyy"),
					$"ORDRSP-{orderDb.Bezug}-{addressDb.Duns}-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.xml");
				SaveXmlFile(orderResponseContent, filePath);

				return new Core.Models.ResponseModel<object>()
				{
					Success = true,
					Errors = responseErrors
				};
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				Infrastructure.Services.Logging.Logger.Log(exception.InnerException + "\n\n\n" + exception.StackTrace);
				return new Core.Models.ResponseModel<object>(orderDb.Nr)
				{
					Errors = new List<string>() { "Exception: " + exception.Message }
				};
			}
		}

		private static void SaveXmlFile(Psz.Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage content, string filePath)
		{
			var namespaces = getXmlNamespaces();

			var serializer = new XmlSerializer(content.GetType());
			var settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Encoding = Encoding.UTF8;
			settings.Indent = true;
			settings.IndentChars = "\t";
			settings.NewLineOnAttributes = true;

			createFolder(Path.GetDirectoryName(filePath));
			using(var xmlWriter = XmlWriter.Create(filePath, settings))
			{
				xmlWriter.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>\r\n");
				serializer.Serialize(xmlWriter, content, namespaces);
			}
		}

		private static Models.OrderResponse.OrdersLineItem getOrderLineItem(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity orderDb,
			Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity itemExtentionEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity orderItemEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity itemEntity,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> itemSplitPositionEntities,
			Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity consigneeItemEntity)
		{
			var orderScheduleLines = getOrderScheduleLines(orderItemEntity, itemSplitPositionEntities);
			var consignee = getConsignee(consigneeItemEntity);

			var quantity = 0m;
			if(orderScheduleLines != null && orderScheduleLines.Count > 0)
			{
				foreach(var item in orderScheduleLines)
				{
					if(decimal.TryParse(item.ScheduledQuantity, out var itemQty))
					{
						quantity += itemQty;
					}
				}
			}

			var lineItemAmount = Convert.ToDecimal((orderItemEntity.Gesamtpreis ?? 0m) + itemSplitPositionEntities.Sum(e => (e.Gesamtpreis ?? 0m)));
			var allowanceChargeAmount = Convert.ToDecimal((orderItemEntity.Gesamtkupferzuschlag ?? 0m) + itemSplitPositionEntities.Sum(e => (e.Gesamtkupferzuschlag ?? 0m)));
			var bezeichnung = (string.IsNullOrWhiteSpace(orderItemEntity.Bezeichnung1) ? "" : orderItemEntity.Bezeichnung1.Split(" ")?[0]);

			// - 2022-12-01 - set bezeichnung to CustomerItemNumber or Bz1
			if(!string.IsNullOrWhiteSpace(itemEntity.CustomerItemNumber))
			{
				// - restore leading ZEROS - Sirona
				if(bezeichnung[0] == '0' && itemEntity.CustomerItemNumber[0] != '0')
				{
					var leads = "";
					int _i = 0;
					while(bezeichnung[_i] == '0')
					{
						leads = $"{leads}0";
						_i++;
					}
					bezeichnung = $"{leads}{itemEntity.CustomerItemNumber.TrimStart(' ', '0')}";
				}
				else
				{
					// - 2023-04-05
					bezeichnung = $"{itemEntity.CustomerItemNumber}";
				}
			}

			// - 2022-11-30 - trim Alternativ
			var altIdx = bezeichnung.ToLower().IndexOf("'alternativ");
			if(altIdx >= 0)
			{
				bezeichnung = bezeichnung.Substring(0, altIdx);
			}

			// - 2024-09-05 // - Quantity decimal with point
			orderScheduleLines.ForEach(x => x.ScheduledQuantity = Convert.ToDecimal(x.ScheduledQuantity).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
			return new Models.OrderResponse.OrdersLineItem
			{
				PositionNumber = orderItemEntity.Position ?? -1,
				CustomersItemMaterialNumber = isValidString(bezeichnung) ? bezeichnung : null,
				SuppliersItemMaterialNumber = isValidString(itemEntity.ArtikelNummer) ? itemEntity.ArtikelNummer : null,
				ItemDescription = isValidString(orderItemEntity.Bezeichnung2) ? orderItemEntity.Bezeichnung2 : null,

				// > Important: Zeichnungsnummer
				ItemCategory = isValidString(orderItemEntity.Zeichnungsnummer) ? orderItemEntity.Zeichnungsnummer : null,
				OrderedQuantity = (orderScheduleLines == null || orderScheduleLines.Count <= 0)
					? string.Empty
					: Convert.ToDecimal(quantity, System.Globalization.CultureInfo.InvariantCulture).ToString(System.Globalization.CultureInfo.InvariantCulture),
				MeasureUnitQualifier = "PCE",
				CurrentItemPriceCalculationGross = orderItemEntity.VKEinzelpreis == null
					? string.Empty
					: Convert.ToDecimal(orderItemEntity.VKEinzelpreis, System.Globalization.CultureInfo.InvariantCulture).ToString(System.Globalization.CultureInfo.InvariantCulture),
				CurrentItemPriceCalculationNet = orderItemEntity.Einzelpreis == null
					? string.Empty
					: Convert.ToDecimal(orderItemEntity.Einzelpreis).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture).ToString(System.Globalization.CultureInfo.InvariantCulture),
				UnitPriceBasis = orderItemEntity.Preiseinheit == null
					? string.Empty
					: Convert.ToDecimal(orderItemEntity.Preiseinheit, System.Globalization.CultureInfo.InvariantCulture).ToString(System.Globalization.CultureInfo.InvariantCulture),
				LineItemAmount = Convert.ToDecimal(lineItemAmount).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),

				LineItemActionRequest = getOrderResponseAction(itemExtentionEntity),

				// > Free Text
				FreeText = isValidString(orderItemEntity.Freies_Format_EDI)
					? new Models.OrderResponse.FreeText
					{
						TextLanguage = "D",
						TextQualifier = "ZZZ",
						Text = orderItemEntity.Freies_Format_EDI
					}
					: null,

				// > References
				References = new Models.OrderResponse.References
				{
					OrderReference = new Models.OrderResponse.OrderReference
					{
						ReferenceQualifier = "ON",
						ReferenceNumber = orderDb.Bezug,
						LineNumber = $"{orderItemEntity.Position ?? -1}",
						Date = new Models.OrderResponse.Date
						{
							DateTime2 = orderDb.Datum == null ? null : (DateTime.TryParse(orderDb.Datum.ToString(), out DateTime d) ? d.ToString("yyyy-MM-ddTHH:mm:ss") : null)
						}
					}
				},

				// > Orders Schedule
				OrdersScheduleLine = orderScheduleLines,
				Consignee = consignee,

				// > AllowancesAndCharges
				AllowancesAndCharges = new Models.OrderResponse.AllowancesAndCharges()
				{
					AllowanceChargeQualifier = "Charge",
					AllowanceChargeAmount = allowanceChargeAmount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture).ToString(System.Globalization.CultureInfo.InvariantCulture),
					AllowanceChargeComment = "Kupferzuschlag"
				},
			};
		}

		private static Models.OrderResponse.ReferencedDocuments getReferencedDocuments(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity orderDb)
		{
			return new Models.OrderResponse.ReferencedDocuments
			{
				PurchaseOrderReferenceNumber = new Models.OrderResponse.PurchaseOrderReferenceNumber
				{
					ReferenceNumber = orderDb.Bezug,
					Date = new Models.OrderResponse.Date
					{
						DateTime2 = orderDb.Datum == null ? null : ((DateTime)orderDb.Datum).ToString("yyyy-MM-ddTHH:mm:ss")
					}
				},
				ReferencedDocument = (isValidString(orderDb.Projekt_Nr) && int.TryParse(orderDb.Projekt_Nr, out int projectNumber) && projectNumber > 0)
					? new Models.OrderResponse.ReferencedDocument
					{
						ReferenceQualifier = "VN",
						ReferenceNumber = projectNumber.ToString(),
						Date = new Models.OrderResponse.Date
						{
							DateTime2 = orderDb.Datum == null ? null : (DateTime.TryParse(orderDb.Datum.ToString(), out DateTime d) ? d.ToString("yyyy-MM-ddTHH:mm:ss") : null) // FIXME: >>> ????? >>>
						}
					}
					: null
			};
		}

		static void createFolder(string path)
		{
			if(!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}
	}
}
