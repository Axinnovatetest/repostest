using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		// -DESADV
		public static int testGenerate(int lsNr)
		{
			// -
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				//var resents = Infrastructure.Data.Access.Joins.CTS.Divers.GetDeliveryNotesForDesadv();
				//if(resents?.Count > 0)
				//{
				//	foreach(var item in resents)
				//	{
				//		GenerateDesadvResponse(item, botransaction);
				//	}
				//}
				var ls = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(lsNr); //1223477// 1223479
				GenerateDesadvResponse(ls, botransaction);

				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return 1;
				}
				else
				{
					return 0;
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<object> GenerateDesadvResponse(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity orderDb, Infrastructure.Services.Utils.TransactionsManager botransaction)
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
				var kundeEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByAddressNr(addressDb.Nr, botransaction.connection, botransaction.transaction);
				if(kundeEntity is null)
				{
					return new Core.Models.ResponseModel<object>(orderDb.Nr)
					{
						Errors = new List<string>()
						{
							"Customer number not found in Kunden Table"
						}
					};
				}
				if(kundeEntity.Edi_Aktiv_Desadv is null || kundeEntity.Edi_Aktiv_Desadv == false)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Success = true
					};
				}

				var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(orderDb.Nr, botransaction.connection, botransaction.transaction);
				var orderItemsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetOriginalByAngeboteNr(orderDb.Nr, botransaction.connection, botransaction.transaction); // <<<<< This is already filtered to send back only Primary Positions
				var buyerDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrderType(orderDb.Nr, (int)Enums.OrderEnums.OrderTypes.Order, botransaction.connection, botransaction.transaction);
				var consigneesDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.GetByOrderType(orderDb.Nr, (int)Enums.OrderEnums.OrderTypes.Order, botransaction.connection, botransaction.transaction);
				var headerConsigneeDb = consigneesDb?.FindAll(c => c?.OrderElementId == null)?.FirstOrDefault();
				var splitPositionsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetSplitPositionByAngeboteNr(orderDb.Nr, botransaction.connection, botransaction.transaction);
				var salesExtensionEntities = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndType(orderItemsDb.Select(x => x.ArtikelNr ?? -1), 3, botransaction.connection, botransaction.transaction); // 3- Type serie
				var artikelDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(orderItemsDb.Select(x => x.ArtikelNr ?? -1).ToList(), botransaction.connection, botransaction.transaction);
				var addressLDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)orderDb.Nr, botransaction.connection, botransaction.transaction);
				var ab = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(orderDb.Ab_id??0, botransaction.connection, botransaction.transaction);
				var lineitemplan = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetWithTransaction(ab?.nr_dlf ?? 0, botransaction.connection, botransaction.transaction);
				var lineitem = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetWithTransaction(lineitemplan?.LineItemId ?? 0, botransaction.connection, botransaction.transaction);
				var delforheader = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetWithTransaction(lineitem?.HeaderId ?? 0, botransaction.connection, botransaction.transaction);

				// 31-10-2025/ Desadv should not be generated for the articles starts with "UM"
				orderItemsDb = orderItemsDb
					.Where(item =>
					{
						var articleEntity = artikelDb.FirstOrDefault(a => a.ArtikelNr == item.ArtikelNr);
						return articleEntity == null
							|| string.IsNullOrEmpty(articleEntity.ArtikelNummer)
							|| (
								!articleEntity.ArtikelNummer.StartsWith("UM", StringComparison.OrdinalIgnoreCase)
								&& !articleEntity.ArtikelNummer.StartsWith("SONS", StringComparison.OrdinalIgnoreCase)
								&& !articleEntity.ArtikelNummer.StartsWith("Fracht", StringComparison.OrdinalIgnoreCase)
								);
							//|| !articleEntity.ArtikelNummer.StartsWith("UM", StringComparison.OrdinalIgnoreCase)
							//|| !articleEntity.ArtikelNummer.StartsWith("SONS", StringComparison.OrdinalIgnoreCase)
							//|| !articleEntity.ArtikelNummer.StartsWith("Fracht", StringComparison.OrdinalIgnoreCase);
					})
					.ToList();

				if(orderItemsDb.Count == 0)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Success = true
					};
				}

				var delforAddress = new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity();
				if(delforheader == null)
				{
					var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(addressDb.Nr, botransaction.connection, botransaction.transaction);
					delforAddress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderDb.LsAddressNr is not null ? orderDb.LsAddressNr.Value : (customerEntity.LSADR ?? 0), botransaction.connection, botransaction.transaction);
				}

				#region > Get and Update Items Extensions data
				foreach(var orderItemDb in orderItemsDb)
				{
					OrderElementExtension.SetStatus(orderItemDb.Nr, botransaction);
				}

				var orderItemsExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderId(orderDb.Nr, botransaction.connection, botransaction.transaction);
				#endregion

				#region // #Document Structure
				var responseErrors = new List<string>();
				var desadvContent = new Psz.Core.Apps.EDI.Models.DespatchAdvice.ErpelMessage();

				var currentUtcDateTime = DateTime.UtcNow;
				var berlinZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"); // >>> Berlin time zone
				var berlinDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcDateTime, berlinZone);  // >>>>>>>>>> Extension ValidationTime

				#region > Header
				desadvContent.ErpelBusinessDocumentHeader = new Psz.Core.Apps.EDI.Models.DespatchAdvice.ErpelBusinessDocumentHeader
				{
					InterchangeHeader = new Models.DespatchAdvice.InterchangeHeader
					{
						Sender = new Models.DespatchAdvice.Sender
						{
							Id = "PSZ_DE",//orderExtensionDb?.RecipientId
							CodeQualifier = "ZZZ",
						},
						Recipient = new Models.DespatchAdvice.Recipient
						{
							Id= delforheader is not null ? delforheader?.SenderId : getRecipientId(orderDb.Kunden_Nr ?? 0, botransaction),
							CodeQualifier = "ZZZ",
						},
						DateTime = new Models.DespatchAdvice.DateTime
						{
							Date = berlinDateTime.ToString("yyyy-MM-dd"),
							Time = berlinDateTime.ToString("HH:mm:ss")
						},
						ControlRef = $"{orderDb.Nr}"
					},
				};
				#endregion

				#region > Document
				// 20-06-2025
				desadvContent.Document = GetDespatchAdviceDocument(orderDb, orderItemsDb, salesExtensionEntities, artikelDb, addressDb, delforheader, delforAddress, botransaction);

				#endregion >>>>> Document
				#endregion

				var filePath = System.IO.Path.Combine(Psz.Core.Program.EDI.DesadvDirectoryName, DateTime.Today.ToString("dd.MM.yyyy"),
					orderDb.Unser_Zeichen,
				$"DESADV-{orderDb.Bezug}-{addressDb.Duns}-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.xml");
				SaveXmlFile(desadvContent, filePath);

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

		private static void SaveXmlFile(Psz.Core.Apps.EDI.Models.DespatchAdvice.ErpelMessage content, string filePath)
		{
			var namespaces = new XmlSerializerNamespaces();
			namespaces.Add("document", Models.DespatchAdvice.Namespaces.Document);
			namespaces.Add("ext", Models.DespatchAdvice.Namespaces.Ext);
			namespaces.Add("bosch", Models.DespatchAdvice.Namespaces.Bosch);
			namespaces.Add("edifact", Models.DespatchAdvice.Namespaces.Edifact);
			namespaces.Add("klosterquell", Models.DespatchAdvice.Namespaces.Klosterquell);
			namespaces.Add("header", Models.DespatchAdvice.Namespaces.Header);
			namespaces.Add("erpel", Models.DespatchAdvice.Namespaces.Erpel);
			namespaces.Add("xsi", Models.DespatchAdvice.Namespaces.Xsi);
			namespaces.Add("schemaLocation", Models.DespatchAdvice.Namespaces.SchemaLocation);


			var serializer = new XmlSerializer(content.GetType());
			var settings = new XmlWriterSettings();
			settings.Encoding = Encoding.UTF8;
			settings.Indent = true;
			settings.IndentChars = "\t";
			settings.NewLineOnAttributes = true;

			filePath = Infrastructure.Services.Files.Utils.SanitizePath(filePath);
			createFolder(Path.GetDirectoryName(filePath));
			using(var xmlWriter = XmlWriter.Create(filePath, settings))
			{
				serializer.Serialize(xmlWriter, content, namespaces);
			}
		}
		private static Psz.Core.Apps.EDI.Models.DespatchAdvice.Document GetDespatchAdviceDocument(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity orderDb,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> orderItemsDb,
			List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> salesExtensionEntities,
			List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelDb,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressDb,
			Infrastructure.Data.Entities.Tables.CTS.HeaderEntity delforheader,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity delforAddress,
			//20-06-2025
			Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			var ab = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(orderDb?.Ab_id ?? 0, botransaction.connection, botransaction.transaction);

			var locationParts = orderDb.LLand_PLZ_Ort?.Split(',');
			string zip =  string.Empty;
			string town = string.Empty;
			string country = string.Empty;
			if(locationParts?.Length>1)
			{
				zip = locationParts?.Length > 0 ? locationParts[0].Trim() : string.Empty;
				town = locationParts?.Length > 1 ? locationParts[1].Trim() : string.Empty;
				country = locationParts?.Length > 2 ? locationParts[2].Trim() : string.Empty;
			}
			else
			{
				locationParts = orderDb.LLand_PLZ_Ort?.Split(' ');
				zip = locationParts?.Length > 0 ? locationParts[0].Trim() : string.Empty;
				town = locationParts?.Length > 1 ? locationParts[1].Trim() : string.Empty;
				country = locationParts?.Length > 2 ? locationParts[2].Trim() : string.Empty;
			}

			var switchClaasNumber = false;
			if($"{(delforheader is not null ? delforheader.ConsigneePartyIdentification : addressDb.Kundennummer)}" == "11725"
				|| (orderDb?.LStraße_Postfach == "Muehlenwinkel 1" && town== "Harsewinkel" && zip=="33428"))
			{
				switchClaasNumber = true;
			}

			var document = new Psz.Core.Apps.EDI.Models.DespatchAdvice.Document
			{
				GeneratingSystem = "",
				DocumentType = "DispatchAdvice",
				DocumentCurrency = "EUR",
				DocumentNumber = $"{orderDb.Angebot_Nr}",
				DocumentDate = $"{orderDb.Datum?.ToString("yyyy-MM-ddT00:00:00")}", //Versanddatum_Auswahl?.ToString("yyyy-MM-ddT00:00:00")
				Delivery = new Models.DespatchAdvice.Delivery
				{
					DeliveryRecipient = new Models.DespatchAdvice.DeliveryRecipient
					{
						// - FurtherIdentification to be processed
						FurtherIdentification = new List<Models.DespatchAdvice.FurtherIdentification>
						{
							new Models.DespatchAdvice.FurtherIdentification
							{
								IdentificationType = "ZZZ",
								Text = $"{(delforheader is not null ? delforheader.ConsigneeUnloadingPoint : (!string.IsNullOrWhiteSpace(ab?.UnloadingPoint)? ab.UnloadingPoint: delforAddress.UnloadingPoint))}"
							},
							new Models.DespatchAdvice.FurtherIdentification
							{
								IdentificationType = "UnloadingPoint",
								Text =  $"{(delforheader is not null ? delforheader.ConsigneeUnloadingPoint : (! string.IsNullOrWhiteSpace(ab ?.UnloadingPoint) ? ab.UnloadingPoint : delforAddress.UnloadingPoint))}"
							},
							new Models.DespatchAdvice.FurtherIdentification
							{
								IdentificationType="StorageLocation",
								Text =  $"{(delforheader is not null ? delforheader.ConsigneeStorageLocation : (!string.IsNullOrWhiteSpace(ab?.StorageLocation)? ab.StorageLocation: delforAddress.StorageLocation))}"
							},
							new Models.DespatchAdvice.FurtherIdentification
							{
								IdentificationType="PlantCode",
								Text = $"{(delforheader is not null ? delforheader.ConsigneeUnloadingPoint : (!string.IsNullOrWhiteSpace(ab?.UnloadingPoint)? ab.UnloadingPoint: delforAddress.UnloadingPoint))}"
							}
						},
						Address = new Models.DespatchAdvice.Address
						{
							// - Consignee
							Name = $"{orderDb?.LVorname_NameFirma}{orderDb.LName2}", // - 2025-08-29 - CLAAS wants to get 0010 instead of their customerNumber 11725 (see email from Khelil 2025-08-27) - see Chhoud for proper fix
							PartyIdentification = switchClaasNumber ? "0010": $"{(delforheader is not null ? delforheader.ConsigneePartyIdentification : addressDb.Kundennummer)}",
							Street = $"{orderDb?.LStraße_Postfach}",							
							ZIP = zip,
							Town = town,
							Country = country,
						}
					},
					DeliveryDetails = new Models.DespatchAdvice.DeliveryDetails
					{
						Date = $"{orderDb.LsDeliveryDate?.ToString("yyyy-MM-ddT00:00:00")}",
					},
				},
				Supplier = new Models.DespatchAdvice.Supplier
				{
					VATIdentificationNumber = $"{orderDb.Freitext}".Trim().Replace("USt - ID - Nr.:", "").Trim(),
					FurtherIdentification = new List<Models.DespatchAdvice.FurtherIdentification>
					{
						new Models.DespatchAdvice.FurtherIdentification
						{
							IdentificationType = "ZZZ",
							Text =  $"{orderDb.Ihr_Zeichen}"
						},
					},
					DocumentReference = new Models.DespatchAdvice.DocumentReference
					{
						DocumentID = $"{orderDb.Angebot_Nr}",
						DocumentType = "DispatchAdvice",
						ReferenceDate = $"{orderDb.Datum?.ToString("yyyy-MM-ddT00:00:00")}"
					},
					Address = new Models.DespatchAdvice.Address
					{
						Name = "PSZ electronic GmbH",
						Street = "Im Gstaudach 6",
						Town = "Vohenstrauß",
						ZIP = "92648",
						Country = "DE",
					}
				},
				Customer = new Models.DespatchAdvice.Customer
				{
					FurtherIdentification = new List<Models.DespatchAdvice.FurtherIdentification>
					{
						new Models.DespatchAdvice.FurtherIdentification
						{
							IdentificationType = "ZZZ",
							Text = $"{orderDb.Unser_Zeichen}"
						},
					},
					DocumentReference = new Models.DespatchAdvice.DocumentReference
					{
						DocumentID = $"{orderDb.Bezug}",
						DocumentType = "ContractReference",
						ReferenceDate = $"{orderDb.Versanddatum_Auswahl?.ToString("yyyy-MM-ddT00:00:00")}",
					},
					Address = new Models.DespatchAdvice.Address
					{
						Name = $"{addressDb?.Name1}",
						Street = $"{addressDb?.StraBe}",
						Town = $"{addressDb?.Ort}",
						ZIP = $"{addressDb?.PLZ_StraBe}",
						Country = $"{addressDb?.Land}",
					}
				},
				Details = new Models.DespatchAdvice.Details
				{
					ItemList = new Models.DespatchAdvice.ItemList
					{
						DeliveryListLineItem = new List<Psz.Core.Apps.EDI.Models.DespatchAdvice.DeliveryListLineItem>()
					},
				},
				DocumentExtension = new Models.DespatchAdvice.DocumentExtensionExt
				{
					DocumentExtension = new Models.DespatchAdvice.DocumentExtensionEdifact
					{
						AdditionalDate = new List<Models.DespatchAdvice.AdditionalDate>
						{
							new Models.DespatchAdvice.AdditionalDate
							{
								DateFunctionCodeQualifier = "17",
								Text= $"{orderDb.Versanddatum_Auswahl?.ToString("yyyy-MM-ddT00:00:00")}",
								//Text = "2024-07-31"
							},
						},
					}
				}
			};

			// -ItemList
			foreach(var item in orderItemsDb)
			{
				var saleExtensionEntity = salesExtensionEntities.FirstOrDefault(x => x.ArticleNr == item.ArtikelNr);
				var articleEntity = artikelDb.FirstOrDefault(x => x.ArtikelNr == item.ArtikelNr);
				document.Details.ItemList.DeliveryListLineItem.Add(new Models.DespatchAdvice.DeliveryListLineItem
				{
					ConsignmentPackagingSequence = new Models.DespatchAdvice.ConsignmentPackagingSequence
					{
						HierarchicalId = "1",
						HierarchicalParentId = "1",
						ConsignmentItemInformation = new Models.DespatchAdvice.ConsignmentItemInformation
						{
							NVE = $"{item.Nr}",
							ContainedPackagingItems = new List<Models.DespatchAdvice.ContainedPackagingItems>
							{
								new Models.DespatchAdvice.ContainedPackagingItems
								{
									SupplierUnit = "PAL",
									Text = $"{(( (saleExtensionEntity?.Verpackungsmenge is null?saleExtensionEntity.Losgroesse: saleExtensionEntity?.Verpackungsmenge) ?? 0).ToString("0.00", CultureInfo.InvariantCulture))}"
								}
							},
						},
						ListLineItem = new Models.DespatchAdvice.ListLineItem
						{
							PositionNumber = $"{item.Position}",
							//ShortDescription = $"{item.Bezeichnung2}",
							ShortDescription = item.Bezeichnung2?.Length > 22
							? item.Bezeichnung2.Substring(0, 22)
							: item.Bezeichnung2 ?? string.Empty,

							//Description = $"{item.Bezeichnung1}",
							Description = item.Bezeichnung1?.Length > 150
							? item.Bezeichnung1.Substring(0, 150)
							: item.Bezeichnung1 ?? string.Empty,

							ArticleNumber = new List<Models.DespatchAdvice.ArticleNumber>
						{
							new Models.DespatchAdvice.ArticleNumber
							{
								ArticleNumberType = "SuppliersArticleNumber",
								Text = $"{articleEntity?.ArtikelNummer}"
							},
							new Models.DespatchAdvice.ArticleNumber
							{
								ArticleNumberType = "CustomersArticleNumber",
								Text = $"{articleEntity?.CustomerItemNumber}"
							},
						},
							Quantity = new List<Models.DespatchAdvice.Quantity>
						{
							new Models.DespatchAdvice.Quantity
							{
								Unit = "PCE",
								Text = $"{item.Anzahl}"
							},
						},
						},
					},
				});
			}

			return document;
		}
		private static string getRecipientId(int addressNr, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			var addressExtensionEntity = Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByAddressNr(addressNr, botransaction.connection, botransaction.transaction);
			if(addressExtensionEntity is not null)
			{
				return addressExtensionEntity.Duns;
			}

			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(addressNr, botransaction.connection, botransaction.transaction);
			return $"{adressenEntity?.Kundennummer}";
		}
	}
}
