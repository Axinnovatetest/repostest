using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> Validate(int orderId, Core.Identity.Models.UserModel user)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				var response = new Core.Models.ResponseModel<object>();
				#region // -- transaction-based logic -- //
				//-
				lock(Locks.OrdersLock)
				{
					#region > Validation
					if(user == null || !user.Access.CustomerService.EDIOrderEdit)
					{
						return Core.Models.ResponseModel<object>.AccessDeniedResponse();
					}

					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(orderId, botransaction.connection, botransaction.transaction);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Success = false,
							Errors = new List<string>()
						{
							"Order not found"
						}
						};
					}

					if(!(orderDb.Neu_Order ?? false))
					{
						return new Core.Models.ResponseModel<object>()
						{
							Success = false,
							Errors = new List<string>()
						{
							"Order already validated"
						}
						};
					}

					// - 2022-11-24 - prevent validation of Orders imported from P3000
					if(!string.IsNullOrEmpty(orderDb.EDI_Dateiname_CSV) && orderDb.EDI_Dateiname_CSV.Trim().ToLower().Substring(orderDb.EDI_Dateiname_CSV.Trim().Length - 4, 4) == ".csv")
					{
						return Core.Models.ResponseModel<object>.FailureResponse("Cannot validate CSV file order, please try it from P3000");
					}

					// Positions validation
					var positionErrors = new List<string> { };
					var postionsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderId, botransaction.connection, botransaction.transaction, false);
					if(postionsDb != null || postionsDb.Count > 0)
					{
						for(int i = 0; i < postionsDb.Count; i++)
						{
							if(postionsDb[i].Anzahl == 0)
							{
								continue;
							}

							if(postionsDb[i].Anzahl < 0)
							{
								positionErrors.Add($"Position {postionsDb[i].Position}: invalid value '{postionsDb[i].Anzahl}' for quantity.");
							}
							if(postionsDb[i].Liefertermin == null || DateTime.TryParse(postionsDb[i].Liefertermin.ToString(), out DateTime d) != true)
							{
								positionErrors.Add($"Position {postionsDb[i].Position}: invalid value '{postionsDb[i].Liefertermin}' for delivery date.");
							}
							if(postionsDb[i].Lagerort_id == null || postionsDb[i].Lagerort_id <= 0)
							{
								positionErrors.Add($"Position {postionsDb[i].Position}: invalid value for storage location.");
							}
						}
					}
					if(positionErrors.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Success = false,
							Errors = positionErrors
						};
					}
					#endregion

					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateNeuOrder(orderDb.Nr, false, botransaction.connection, botransaction.transaction);

					// if order was already assigned a project value; DO NOT change it!
					var nextAngeboteNr = (string.IsNullOrEmpty(orderDb.Projekt_Nr) || string.IsNullOrWhiteSpace(orderDb.Projekt_Nr)) ?
						$"{Convert.ToInt32(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.MaxAngebotNrByTyp(Apps.Purchase.Enums.OrderEnums.TypeToData(Purchase.Enums.OrderEnums.Types.Confirmation), botransaction.connection, botransaction.transaction)) + 1}"
						: orderDb.Projekt_Nr;

					var nextProjektNr = nextAngeboteNr;
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(orderDb.Nr,
						nextAngeboteNr,
						nextProjektNr,
						$"Gebucht, {user.Username}, {DateTime.Now}",
						true, botransaction.connection, botransaction.transaction);

					var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(orderDb.Nr, botransaction.connection, botransaction.transaction);
					if(orderExtensionDb == null)
					{
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.InsertWithTRansaction(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity()
						{
							Id = -1,
							OrderId = orderDb.Nr,
							EdiValidationTime = DateTime.Now,
							EdiValidationUserId = user.Id,
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = user.Id,
							LastUpdateUsername = user.Username,
							RecipientId = null,
							SenderDuns = null,
						}, botransaction.connection, botransaction.transaction);
					}
					else
					{
						orderExtensionDb.EdiValidationUserId = user.Id;
						orderExtensionDb.EdiValidationTime = DateTime.Now; // Convert Berlin Time
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.UpdateWithTRansaction(orderExtensionDb, botransaction.connection, botransaction.transaction);
					}

					// > Notify
					Core.Program.Notifier.PushEdiImportedOrdersNotification(new Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel()
					{
						Type = "Success",
						Payload = Order.CountUnvalidated(null, botransaction).ToString()
					});

					// > Order Response
					response = GenerateOrderResponse(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(orderDb.Nr, botransaction.connection, botransaction.transaction), botransaction);
				}

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return response;
				}
				else
				{
					return Core.Models.ResponseModel<object>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}


		}

		private static List<Models.OrderResponse.OrdersScheduleLine> getOrderScheduleLines(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity positionDb,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> itemSplitPositionDb)
		{
			var response = new List<Models.OrderResponse.OrdersScheduleLine>();

			response.Add(new Models.OrderResponse.OrdersScheduleLine
			{
				ScheduledQuantity = positionDb.Anzahl != null
					? positionDb.Anzahl.ToString()
					: null,
				ScheduledQuantityDate = positionDb.Liefertermin != null
					? new Models.OrderResponse.ScheduledQuantityDate()
					{
						DateQualifier = "2",
						DateTime2 = positionDb.Liefertermin == null ? null : ((DateTime)positionDb.Liefertermin).ToString("yyyy-MM-ddTHH:mm:ss")
					}
					: null
			});

			if(itemSplitPositionDb != null)
			{
				foreach(var position in itemSplitPositionDb)
				{
					response.Add(new Models.OrderResponse.OrdersScheduleLine
					{
						ScheduledQuantity = position.Anzahl == null ? null : position.Anzahl.ToString(),
						ScheduledQuantityDate = position.Liefertermin != null
							? new Models.OrderResponse.ScheduledQuantityDate
							{
								DateQualifier = "2",
								DateTime2 = position.Liefertermin == null ? null : (DateTime.TryParse(position.Liefertermin.ToString(), out DateTime d) ? d.ToString("yyyy-MM-ddTHH:mm:ss") : null)
							}
							: null
					});
				}
			}

			return response;
		}
		private static Psz.Core.Apps.EDI.Models.OrderResponse.ErpelBusinessDocumentHeader GetOrderResponseHeader()
		{
			return new Psz.Core.Apps.EDI.Models.OrderResponse.ErpelBusinessDocumentHeader
			{
				MessageReceivedAt = "2019-12-16T14:35:30.139",
				InterchangeHeader = new Models.OrderResponse.InterchangeHeader
				{
					Sender = new Models.OrderResponse.Sender
					{
						Id = "332358188"
					},
					Recipient = new Models.OrderResponse.Recipient
					{
						Id = "316356880" // DUNS
					},
					DateTime = new Models.OrderResponse.DateTime
					{
						Date = "12019-12-16",
						Time = "14:35:30"
					}
				}
			};
		}
		private static Psz.Core.Apps.EDI.Models.OrderResponse.Document GetOrderResponseDocument()
		{
			return new Psz.Core.Apps.EDI.Models.OrderResponse.Document
			{
				Header = new Models.OrderResponse.Header
				{
					MessageHeader = new Models.OrderResponse.MessageHeader
					{
						MessageReferenceNumber = "1",
						MessageType = "ORDRSP"
					},
					BeginningOfMessage = new Models.OrderResponse.BeginningOfMessage
					{
						DocumentNameEncoded = "231",
						DocumentNumber = "4500645643-" + DateTime.Today.ToString("YYYYMMDD")
					},
					Dates = new Models.OrderResponse.Dates
					{
						DocumentDate = new Models.OrderResponse.DocumentDate
						{
							DateQualifier = "4",
							DateTime2 = DateTime.Today.ToString("YYYYMMDD")
						}
					},
					ReferenceCurrency = "EUR",
					ReferencedDocuments = new Models.OrderResponse.ReferencedDocuments
					{
						PurchaseOrderReferenceNumber = new Models.OrderResponse.PurchaseOrderReferenceNumber
						{
							//
						}
					}
				},
				Details = new Models.OrderResponse.Details
				{
					OrdersData = new Models.OrderResponse.OrdersData
					{
						OrdersLineItem = new List<Models.OrderResponse.OrdersLineItem> {
							new Models.OrderResponse.OrdersLineItem{
								PositionNumber = 1,
								CustomersItemMaterialNumber = "00346578",
								SuppliersItemMaterialNumber = "932-015-00TN",
								ItemDescription = "HyVerlKab RS(31)  8500"
							}
						}
					}
				},
				Footer = new Models.OrderResponse.Footer
				{
					InvoiceFooter = new Models.OrderResponse.InvoiceFooter
					{
						InvoiceTotals = new Models.OrderResponse.InvoiceTotals
						{
							TotalTaxableAmount = "2234.55",
							InvoiceAmount = "2234.55"
						}
					}
				}
			};
		}
		private static XmlSerializerNamespaces getXmlNamespaces()
		{
			var namespaces = new XmlSerializerNamespaces();
			namespaces.Add("", "http://schemas.ecosio.com/erpel-industry-1p0/message");
			namespaces.Add("header", "http://schemas.ecosio.com/erpel-industry-1p0/header");
			namespaces.Add("document", "http://schemas.ecosio.com/erpel-industry-1p0/document");
			namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			namespaces.Add("schemaLocation", "http://schemas.ecosio.com/erpel-industry-1p0/message file:///C:/Users/Martin/Documents/mappings/Schemas/ERPEL1p0-Industry/Message.xsd");

			return namespaces;
		}
		private static string getOrderResponseAction(Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity itemExtentionEntity)
		{
			if(itemExtentionEntity == null)
			{
				return "5";
			}

			switch((Enums.OrderElementEnums.OrderElementStatus)itemExtentionEntity.Status)
			{
				case Enums.OrderElementEnums.OrderElementStatus.Original:
					return "5";
				case Enums.OrderElementEnums.OrderElementStatus.Changed:
					return "6";
				case Enums.OrderElementEnums.OrderElementStatus.Deleted:
					return "7";
				default:
					return "";
			}
		}
		private static Models.OrderResponse.Consignee getConsignee(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity consigneeDb)
		{
			if(consigneeDb == null)
			{
				return null;
			}

			var consignee = new Models.OrderResponse.Consignee
			{
				PartyQualifier = "DP", // FIXED
				DUNS = isValidString(consigneeDb?.DUNS) ? consigneeDb?.DUNS : null,
				PartyName = new List<string> { },
				Street = isValidString(consigneeDb?.Street) ? consigneeDb?.Street : null,
				City = isValidString(consigneeDb?.City) ? consigneeDb?.City : null,
				PostCode = isValidString(consigneeDb?.PostalCode) ? consigneeDb?.PostalCode : null,
				Country = isValidString(consigneeDb?.CountryName)
									? new Models.OrderResponse.Country
									{
										CountryName = consigneeDb?.CountryName,
									}
									: null,
				Contact = isValidString(consigneeDb?.ContactName)
											|| isValidString(consigneeDb?.ContatTelephone)
											|| isValidString(consigneeDb?.ContactFax)
									? new Models.OrderResponse.Contact
									{
										Name = isValidString(consigneeDb?.ContactName) ? consigneeDb?.ContactName : null, //
										Telephone = isValidString(consigneeDb?.ContatTelephone) ? consigneeDb?.ContatTelephone : null,
										Fax = isValidString(consigneeDb?.ContactFax) ? consigneeDb?.ContactFax : null,
									}
									: null,

				UnloadingPoint = isValidString(consigneeDb?.UnloadingPoint) ? consigneeDb?.UnloadingPoint : null,
				StorageLocation = isValidString(consigneeDb?.StorageLocation) ? consigneeDb?.StorageLocation : null,
			};


			// - Consignee PartyName
			if(isValidString(consigneeDb?.Name)
				|| isValidString(consigneeDb?.Name2)
				|| isValidString(consigneeDb?.Name3))
			{
				consignee.PartyName = new List<string>();
				if(isValidString(consigneeDb?.Name))
				{
					consignee.PartyName.Add(consigneeDb?.Name);
				}
				if(isValidString(consigneeDb?.Name2))
				{
					consignee.PartyName.Add(consigneeDb?.Name2);
				}
				if(isValidString(consigneeDb?.Name3))
				{
					consignee.PartyName.AddRange(consigneeDb?.Name3.Split("||").ToList());
				}
			}

			if(consignee.DUNS == null
				&& (consignee.PartyName == null || consignee.PartyName.Count == 0)
				&& consignee.Street == null
				&& consignee.City == null
				&& consignee.PostCode == null
				&& consignee.Country == null
				&& consignee.Contact == null
				&& consignee.UnloadingPoint == null
				&& consignee.StorageLocation == null
				)
			{
				return null;
			}

			// if any data return consignee
			if(consignee.City != null || consignee.Contact != null || consignee.Country != null || consignee.DUNS != null
					|| consignee.PartyIdentification != null || consignee.PartyIdentificationCodeListQualifier != null
					|| (consignee.PartyName != null && consignee.PartyName.Count > 0) || consignee.PostCode != null || consignee.PurchasingDepartment != null
					|| consignee.StorageLocation != null || consignee.Street != null || consignee.UnloadingPoint != null)
			{
				return consignee;
			}

			// This allow to not render the node in XML
			return null;
		}
		private static bool isValidString(string value)
		{
			return !string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value);
		}
	}
}
