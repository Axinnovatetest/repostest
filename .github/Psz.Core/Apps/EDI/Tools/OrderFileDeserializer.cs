using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.EDI.Tools
{
	public class OportedOrdersHandler
	{
		private string _newFileFullPath;
		private string _errorOrdersDirectory;
		private string _processedOrdersDirectory;

		public OportedOrdersHandler(string newOrdersDirectory,
			string errorOrdersDirectory,
			string processedOrdersDirectory)
		{
			_newFileFullPath = newOrdersDirectory;
			_errorOrdersDirectory = errorOrdersDirectory;
			_processedOrdersDirectory = processedOrdersDirectory;
			Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $">>>>>>>>>>>>>> INIT DESRIALIZER  {newOrdersDirectory} | {errorOrdersDirectory} | {processedOrdersDirectory}");

		}

		public void Handle(Psz.Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage data,
		   string documentName)
		{
			lock(Locks.DocumentsLock)
			{
				try
				{
					if(data.Document.Header.MessageHeader.MessageType.ToUpper() != "ORDERS"
						&& data.Document.Header.MessageHeader.MessageType.ToUpper() != "ORDCHG")
					{
						this.HandleError(documentName, "invalid MessageType", "Other", -1);
						return;
					}


					var isNewOrder = data.Document.Header.MessageHeader.MessageType.ToUpper() == "ORDERS";

					var orderToBeId = -1;
					string documentNumber = null;
					string messageReferenceNumber = null;
					string freeText = null;

					string customerContactName = null;
					string customerName = null;
					string customerPurchasingDepartment = null;
					string customerStreetPostalCode = null;
					string customerStreetCityPostalCode = null;

					string senderId = null;
					string recipientId = null;
					string senderDuns = null;

					string buyerDuns = null;
					string buyerPartyIdentification = null;
					string buyerPartyIdentificationCodeListQualifier = null;
					string BuyerName = null;
					string BuyerName2 = null;
					string BuyerName3 = null;
					string buyerStreet = null;
					string buyerPostalCode = null;
					string buyerCity = null;
					string buyerCountryName = null;
					string buyerPurchasingDepartment = null;
					string buyerContactName = null;
					string buyerContactTelephone = null;
					string buyerContactFax = null;

					string consigneeIdentification = null;
					string consigneeIdentificationCodeListQualifier = null;
					string consigneeDUNS = null;
					string consigneeName = null;
					string consigneeName2 = null;
					string consigneeName3 = null;
					string consigneeStreet = null;
					string consigneeCity = null;
					string consigneePostalCode = null;
					string consigneeCountryName = null;
					string consigneePurchasingDepartment = null;
					string consigneeUnloadingPoint = null;
					string consigneeStorageLocation = null;
					string consigneeContactName = null;
					string consigneeContactTelephone = null;
					string consigneeContactFax = null;
					string consigneeStreetPostalCode = null;

					try
					{
						documentNumber = data?.Document?.Header?.BeginningOfMessage?.DocumentNumber?.Trim();
						if(string.IsNullOrWhiteSpace(documentName))
						{
							this.HandleError(documentName, "Invalid Document Number", "Other", -1);
							return;
						}

						// REM: -- Temp Remove - Sirona test is using not number string as DUNS
						senderDuns = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.Sender?.Id;

						if(string.IsNullOrEmpty(senderDuns) || string.IsNullOrWhiteSpace(senderDuns))
						{
							this.HandleError(documentName, $"[Document: {documentNumber}] Invalid Sender DUNS [{senderDuns}]", "Other", -1);
							return;
						}

						buyerDuns = data.Document?.Header?.BusinessEntities?.Buyer?.DUNS?.ToString();
						//buyerDuns = buyerDuns ?? senderDuns; // >>>>>>>>>>>> CONFIRM!!!!!
						//if (string.IsNullOrEmpty(buyerDuns) || string.IsNullOrWhiteSpace(buyerDuns))
						//{
						//    this.HandleError(documentName, $"[Document: {documentNumber}] Invalid Buyer DUNS [{buyerDuns}]", "Other", -1);
						//    return;
						//}


						freeText = data?.Document?.Header?.FreeText?.Text;
						messageReferenceNumber = data?.Document?.Header?.MessageHeader?.MessageReferenceNumber?.Trim();

						recipientId = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.Recipient?.Id?.Trim();
						senderId = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.Sender?.Id?.Trim();

						buyerPartyIdentification = data.Document?.Header?.BusinessEntities?.Buyer?.PartyIdentification?.Trim();
						if((string.IsNullOrEmpty(buyerDuns) || string.IsNullOrWhiteSpace(buyerDuns))
							&& (string.IsNullOrEmpty(buyerPartyIdentification) || string.IsNullOrWhiteSpace(buyerPartyIdentification)))
						{
							this.HandleError(documentName, $"[Document: {documentNumber}] Invalid Buyer PartyIdentification [{buyerPartyIdentification}]", "Other", -1);
							return;
						}

						buyerPartyIdentificationCodeListQualifier = data.Document?.Header?.BusinessEntities?.Buyer?.PartyIdentificationCodeListQualifier?.Trim();
						if(data.Document?.Header?.BusinessEntities?.Buyer?.PartyName != null)
						{
							if(data.Document?.Header?.BusinessEntities?.Buyer?.PartyName.Count > 0)
							{
								BuyerName = data.Document.Header?.BusinessEntities?.Buyer?.PartyName[0]?.Trim();
							}
							if(data.Document.Header?.BusinessEntities?.Buyer?.PartyName.Count > 1)
							{
								BuyerName2 = data.Document.Header?.BusinessEntities?.Buyer?.PartyName[1]?.Trim();
							}
							if(data.Document.Header?.BusinessEntities?.Buyer?.PartyName.Count > 2)
							{
								BuyerName3 = string.Join("||", data.Document.Header?.BusinessEntities?.Buyer?.PartyName?.Skip(2));
							}
						}
						buyerStreet = data.Document?.Header?.BusinessEntities?.Buyer?.Street?.Trim();
						buyerPostalCode = data.Document?.Header?.BusinessEntities?.Buyer?.PostCode?.Trim();
						buyerCity = data.Document.Header?.BusinessEntities?.Buyer?.City?.Trim();
						buyerCountryName = data.Document?.Header?.BusinessEntities?.Buyer?.Country?.CountryName?.Trim();
						buyerPurchasingDepartment = data.Document?.Header?.BusinessEntities?.Buyer?.PurchasingDepartment?.Trim();
						buyerContactName = data.Document?.Header?.BusinessEntities?.Buyer?.Contact?.Name?.Trim();
						buyerContactTelephone = data.Document?.Header?.BusinessEntities?.Buyer?.Contact?.Telephone?.Trim();
						buyerContactFax = data.Document?.Header?.BusinessEntities?.Buyer?.Contact?.Fax?.Trim();

						consigneeIdentification = data.Document?.Header?.BusinessEntities?.Consignee?.PartyIdentification?.Trim();
						consigneeIdentificationCodeListQualifier = data.Document?.Header?.BusinessEntities?.Consignee?.PartyIdentificationCodeListQualifier?.Trim();
						consigneeDUNS = data.Document?.Header?.BusinessEntities?.Consignee?.DUNS;
						consigneeDUNS = consigneeDUNS ?? senderDuns; // >>>>>>>>>>>> CONFIRM!!!!!
						if(data.Document?.Header?.BusinessEntities?.Buyer?.PartyName != null)
						{
							if(data.Document.Header?.BusinessEntities?.Consignee?.PartyName.Count > 0)
							{
								consigneeName = data.Document?.Header?.BusinessEntities?.Consignee?.PartyName[0]?.Trim();
							}
							if(data.Document.Header?.BusinessEntities?.Consignee?.PartyName.Count > 1)
							{
								consigneeName2 = data.Document.Header?.BusinessEntities?.Consignee?.PartyName[1]?.Trim();
							}
							if(data.Document.Header?.BusinessEntities?.Consignee?.PartyName.Count > 2)
							{
								consigneeName3 = string.Join("||", data.Document.Header?.BusinessEntities?.Consignee?.PartyName?.Skip(2));
							}
						}
						consigneeStreet = data.Document?.Header?.BusinessEntities?.Consignee?.Street?.Trim();
						consigneeCity = data.Document?.Header?.BusinessEntities?.Consignee?.City?.Trim();
						consigneePostalCode = data.Document?.Header?.BusinessEntities?.Consignee?.PostCode?.Trim();
						consigneeCountryName = data.Document?.Header?.BusinessEntities?.Consignee?.Country?.CountryName?.Trim();
						consigneePurchasingDepartment = data.Document?.Header?.BusinessEntities?.Consignee?.PurchasingDepartment?.Trim();
						consigneeUnloadingPoint = data.Document?.Header?.BusinessEntities?.Consignee?.UnloadingPoint?.Trim();
						consigneeStorageLocation = data.Document?.Header?.BusinessEntities?.Consignee?.StorageLocation?.Trim();
						consigneeContactName = data.Document?.Header?.BusinessEntities?.Consignee?.Contact?.Name?.Trim();
						consigneeContactTelephone = data.Document?.Header?.BusinessEntities?.Consignee?.Contact?.Telephone?.Trim();
						consigneeContactFax = data.Document?.Header?.BusinessEntities?.Consignee?.Contact?.Fax?.Trim();
					} catch(NullReferenceException e)
					{
						this.HandleError(documentName, $"[Document: {documentNumber}] Invalid file format", "Other", -1);
						Infrastructure.Services.Logging.Logger.Log(e);
						return;
					}

					if(isNewOrder)
					{
						#region > New Order
						var createRequestData = new Models.Order.CreateModel()
						{
							SenderId = senderId,
							RecipientId = recipientId,

							DocumentName = documentName,
							DocumentNumber = documentNumber,
							FreeText = freeText,

							SenderDuns = senderDuns,

							BuyerDuns = buyerDuns,
							BuyerPartyIdentification = buyerPartyIdentification,
							BuyerPartyIdentificationCodeListQualifier = buyerPartyIdentificationCodeListQualifier,
							BuyerName = BuyerName,
							BuyerName2 = BuyerName2,
							BuyerName3 = BuyerName3,
							BuyerStreet = buyerStreet,
							BuyerPostalCode = buyerPostalCode,
							BuyerCity = buyerCity,
							BuyerCountryName = buyerCountryName,
							BuyerPurchasingDepartment = buyerPurchasingDepartment,
							BuyerContactName = buyerContactName,
							BuyerContactTelephone = buyerContactTelephone,
							BuyerContactFax = buyerContactFax,

							ConsigneeIdentification = consigneeIdentification,
							ConsigneeIdentificationCodeListQualifier = consigneeIdentificationCodeListQualifier,
							ConsigneeDUNS = consigneeDUNS,
							ConsigneeName = consigneeName,
							ConsigneeName2 = consigneeName2,
							ConsigneeName3 = consigneeName3,
							ConsigneeStreet = consigneeStreet,
							ConsigneeCity = consigneeCity,
							ConsigneePostalCode = consigneePostalCode,
							ConsigneeCountryName = consigneeCountryName,
							ConsigneePurchasingDepartment = consigneePurchasingDepartment,
							ConsigneeUnloadingPoint = consigneeUnloadingPoint,
							ConsigneeStorageLocation = consigneeStorageLocation,
							ConsigneeContactName = consigneeContactName,
							ConsigneeContactTelephone = consigneeContactTelephone,
							ConsigneeContactFax = consigneeContactFax,

							Elements = new List<Models.Order.Element.NotCalculatedElementModel>()
						};

						if(data.Document?.Details?.OrdersData?.OrdersLineItem != null && data.Document.Details.OrdersData.OrdersLineItem.Count > 0)
						{
							foreach(var documentElement in data.Document?.Details?.OrdersData?.OrdersLineItem)
							{
								string consigneeNameItem = null;
								string consigneeNameItem2 = null;
								string consigneeNameItem3 = null;
								if(documentElement.Consignee?.PartyName != null)
								{
									if(documentElement.Consignee?.PartyName.Count > 0)
									{
										consigneeNameItem = documentElement.Consignee?.PartyName[0];
									}
									if(documentElement.Consignee?.PartyName.Count > 1)
									{
										consigneeNameItem2 = documentElement.Consignee?.PartyName[1];
									}
									if(documentElement.Consignee?.PartyName.Count > 2)
									{
										consigneeNameItem3 = string.Join("||", documentElement.Consignee?.PartyName?.Skip(2));
									}
								}

								var _id = 0;
								foreach(var ordersScheduleLine in documentElement.OrdersScheduleLine)
								{
									createRequestData.Elements.Add(new Models.Order.Element.NotCalculatedElementModel()
									{
										Id = _id,
										ItemNumber = documentElement.SuppliersItemMaterialNumber?.Trim(),
										CustomerItemNumber = Program.EDI?.Edi?.TrimSatrtZeroSenderIds?.FindIndex(x => x.ToLower().Trim() == senderId?.ToLower()) >= 0 ? documentElement.CustomersItemMaterialNumber?.Trim()?.TrimStart('0') : documentElement.CustomersItemMaterialNumber?.Trim(),// -- 2022-04-10 Sirona trimStart and Horsch not!
										FreeText = documentElement.FreeText?.Text?.Trim(),
										DesiredDate = Convert.ToDateTime(ordersScheduleLine.ScheduledQuantityDate.DateTime2),
										ItemDescription = documentElement.ItemDescription?.Trim(),
										MeasureUnitQualifier = documentElement.MeasureUnitQualifier?.Trim(),
										CurrentItemPriceCalculationNet = !string.IsNullOrEmpty(documentElement.CurrentItemPriceCalculationNet)
											? Convert.ToDecimal(documentElement.CurrentItemPriceCalculationNet, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
											: 0,
										OrderedQuantity = !string.IsNullOrEmpty(ordersScheduleLine.ScheduledQuantity)
											? Convert.ToDecimal(ordersScheduleLine.ScheduledQuantity, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
											: 0,
										PositionNumber = documentElement.PositionNumber,
										UnitPriceBasis = !string.IsNullOrEmpty(documentElement.UnitPriceBasis)
											? Convert.ToDecimal(documentElement.UnitPriceBasis, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
											: 0, // >>>>>>>>>>>>>>> CONFIRM !!!!!
										LineItemAmount = Convert.ToDecimal(documentElement?.LineItemAmount ?? "0", Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO),
										Consignee = new Models.Order.ConsigneeModel
										{
											ConsigneeCity = documentElement.Consignee?.City?.Trim(),
											ConsigneeContactFax = documentElement.Consignee?.Contact?.Fax?.Trim(),
											ConsigneeContactName = documentElement.Consignee?.Contact?.Name?.Trim(),
											ConsigneeContactTelephone = documentElement.Consignee?.Contact?.Telephone?.Trim(),
											ConsigneeCountryName = documentElement.Consignee?.Country?.CountryName?.Trim(),
											ConsigneeDUNS = documentElement.Consignee?.DUNS?.Trim(),
											ConsigneeIdentification = documentElement.Consignee?.PartyIdentification?.Trim(),
											ConsigneeIdentificationCodeListQualifier = documentElement.Consignee?.PartyIdentificationCodeListQualifier?.Trim(),
											ConsigneeName = consigneeNameItem?.Trim(),
											ConsigneeName2 = consigneeNameItem2?.Trim(),
											ConsigneeName3 = consigneeNameItem3?.Trim(),
											ConsigneePostalCode = documentElement.Consignee?.PostCode?.Trim(),
											ConsigneePurchasingDepartment = documentElement.Consignee?.PurchasingDepartment?.Trim(),
											ConsigneeStorageLocation = documentElement.Consignee?.StorageLocation?.Trim(),
											ConsigneeStreet = documentElement.Consignee?.Street?.Trim(),
											ConsigneeUnloadingPoint = documentElement.Consignee?.UnloadingPoint?.Trim()
										}
									});
									_id += 1;
								}
							}
						}

						var createResponse = Handlers.Order.CreateInternal(createRequestData, null, out orderToBeId);
						if(createResponse.Success)
						{
							var mPath = Path.Combine(_processedOrdersDirectory, documentName);
							Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"Moving to: {mPath}");

							// > Move File
							moveFile(_newFileFullPath, Path.Combine(_processedOrdersDirectory, DateTime.Today.ToString("dd.MM.yyyy"), Path.GetFileName(documentName)));

							// > Success Notification
							Program.Notifier.PushEdiImportedOrdersNotification(new Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel()
							{
								Type = "Success",
								Payload = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CountCustomerOrdersByIsNew(true).ToString()
							});
						}
						else
						{
							this.HandleError(documentName, $"[Document: {documentNumber}] {string.Join(", ", createResponse.Errors)}", "Other", createResponse.Body);
						}

						// >>> Save raw data
						XMLFile.SaveFileToDB(orderToBeId, data);

						#endregion
					}
					else
					{
						#region > Order Change
						var createRequestData = new Models.Order.Change.CreateModel()
						{
							SenderId = senderId,
							RecipientId = recipientId,
							SenderDuns = senderDuns,

							MessageReferenceNumber = messageReferenceNumber,
							DocumentName = documentName,
							DocumentNumber = documentNumber,
							FreeText = freeText,


							// > Buyer
							BuyerDuns = buyerDuns,
							BuyerPartyIdentification = buyerPartyIdentification,
							BuyerPartyIdentificationCodeListQualifier = buyerPartyIdentificationCodeListQualifier,
							BuyerName = BuyerName,
							BuyerName2 = BuyerName2,
							BuyerName3 = BuyerName3,
							BuyerStreet = buyerStreet,
							BuyerPostalCode = buyerPostalCode,
							BuyerCity = buyerCity,
							BuyerCountryName = buyerCountryName,
							BuyerPurchasingDepartment = buyerPurchasingDepartment,
							BuyerContactName = buyerContactName,
							BuyerContactTelephone = buyerContactTelephone,
							BuyerContactFax = buyerContactFax,

							// > Consignee
							ConsigneeIdentification = consigneeIdentification,
							ConsigneeIdentificationCodeListQualifier = consigneeIdentificationCodeListQualifier,
							ConsigneeDUNS = consigneeDUNS,
							ConsigneeName = consigneeName,
							ConsigneeName2 = consigneeName2,
							ConsigneeName3 = consigneeName3,
							ConsigneeStreet = consigneeStreet,
							ConsigneeCity = consigneeCity,
							ConsigneePostalCode = consigneePostalCode,
							ConsigneeCountryName = consigneeCountryName,
							ConsigneePurchasingDepartment = consigneePurchasingDepartment,
							ConsigneeUnloadingPoint = consigneeUnloadingPoint,
							ConsigneeStorageLocation = consigneeStorageLocation,
							ConsigneeContactName = consigneeContactName,
							ConsigneeContactTelephone = consigneeContactTelephone,
							ConsigneeContactFax = consigneeContactFax,

							Items = new List<Models.Order.Change.CreateItemModel>()
						};

						foreach(var documentElement in data.Document?.Details?.OrdersData?.OrdersLineItem)
						{
							string consigneeNameItem = null;
							string consigneeNameItem2 = null;
							string consigneeNameItem3 = null;
							if(documentElement.Consignee?.PartyName != null)
							{
								if(documentElement.Consignee?.PartyName.Count > 0)
								{
									consigneeNameItem = documentElement.Consignee?.PartyName[0];
								}
								if(documentElement.Consignee?.PartyName.Count > 1)
								{
									consigneeNameItem2 = documentElement.Consignee?.PartyName[1];
								}
								if(documentElement.Consignee?.PartyName.Count > 2)
								{
									consigneeNameItem3 = string.Join("||", documentElement.Consignee?.PartyName?.Skip(2));
								}
							}


							var LineItemAmount = !string.IsNullOrEmpty(documentElement.LineItemAmount)
										? Convert.ToDecimal(documentElement.LineItemAmount ?? "0", Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
										: 0;

							var _idElt = 0;
							foreach(var ordersScheduleLine in documentElement.OrdersScheduleLine)
							{
								createRequestData.Items.Add(new Models.Order.Change.CreateItemModel()
								{
									LineItemAmount = _idElt == 0 ? LineItemAmount : 0,
									ItemNumber = documentElement.SuppliersItemMaterialNumber?.Trim(),
									CustomerItemNumber = Program.EDI?.Edi?.TrimSatrtZeroSenderIds?.FindIndex(x => x.ToLower().Trim() == senderId?.ToLower()) >= 0 ? documentElement.CustomersItemMaterialNumber?.Trim()?.TrimStart('0') : documentElement.CustomersItemMaterialNumber?.Trim(),// -- 2022-04-10 Sirona trimStart and Horsch not!
									FreeText = documentElement.FreeText?.Text,
									DesiredDate = Convert.ToDateTime(ordersScheduleLine.ScheduledQuantityDate.DateTime2),
									ItemDescription = documentElement.ItemDescription,
									MeasureUnitQualifier = documentElement.MeasureUnitQualifier,
									CurrentItemPriceCalculationNet = !string.IsNullOrEmpty(documentElement.CurrentItemPriceCalculationNet)
										? Convert.ToDecimal(documentElement.CurrentItemPriceCalculationNet, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
										: 0,
									OrderedQuantity = !string.IsNullOrEmpty(ordersScheduleLine.ScheduledQuantity)
										? Convert.ToDecimal(ordersScheduleLine.ScheduledQuantity, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
										: 0,
									PositionNumber = documentElement.PositionNumber,
									UnitPriceBasis = !string.IsNullOrEmpty(documentElement.UnitPriceBasis)
										? Convert.ToDecimal(documentElement.UnitPriceBasis, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
										: 0,
									Type = int.TryParse(documentElement.LineItemActionRequest, out int typ) ? typ : -1,
									Consignee = new Models.Order.ConsigneeModel
									{
										ConsigneeCity = documentElement.Consignee?.City,
										ConsigneeContactFax = documentElement.Consignee?.Contact?.Fax,
										ConsigneeContactName = documentElement.Consignee?.Contact?.Name,
										ConsigneeContactTelephone = documentElement.Consignee?.Contact?.Telephone,
										ConsigneeCountryName = documentElement.Consignee?.Country?.CountryName,
										ConsigneeDUNS = documentElement.Consignee?.DUNS,
										ConsigneeIdentification = documentElement.Consignee?.PartyIdentification,
										ConsigneeIdentificationCodeListQualifier = documentElement.Consignee?.PartyIdentificationCodeListQualifier,
										ConsigneeName = consigneeNameItem,
										ConsigneeName2 = consigneeNameItem2,
										ConsigneeName3 = consigneeNameItem3,
										ConsigneePostalCode = documentElement.Consignee?.PostCode,
										ConsigneePurchasingDepartment = documentElement.Consignee?.PurchasingDepartment,
										ConsigneeStorageLocation = documentElement.Consignee?.StorageLocation,
										ConsigneeStreet = documentElement.Consignee?.Street,
										ConsigneeUnloadingPoint = documentElement.Consignee?.UnloadingPoint
									}
								});
								_idElt += 1;
							}
						}

						var createResponse = Handlers.Order.Change.Create(createRequestData, out orderToBeId);
						if(createResponse.Success)
						{
							var mPath = Path.Combine(_processedOrdersDirectory, documentName);
							Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"Moving to: {mPath}");
							// > Move File
							moveFile(_newFileFullPath, Path.Combine(_processedOrdersDirectory, DateTime.Today.ToString("dd.MM.yyyy"), Path.GetFileName(documentName)));
							//this.moveFile(_newFileFullPath, mPath);

							// > Success Notification
							Program.Notifier.PushEdiImportedOrdersNotification(new Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel()
							{
								Type = "Success",
								Payload = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CountCustomerOrdersByIsNew(true).ToString()
							});
						}
						else
						{
							this.HandleError(documentName, $"[Document: {createRequestData?.DocumentNumber}] {string.Join(", ", createResponse.Errors)}", "Other", createResponse.Body);
						}

						// >>> Save raw data
						XMLFile.SaveFileToDB(orderToBeId, data);

						#endregion
					}
				} catch(Exception exception)
				{
					this.HandleError(documentName, "Exception: " + exception.Message, "Other", -1);
					Infrastructure.Services.Logging.Logger.Log(exception);
				}
			}
		}

		public void HandleError(string fileName,
			string errorMessage,
			string customerName,
			int customerId)
		{
			lock(Locks.DocumentsLock)
			{
				var moveToPath = Path.Combine(_errorOrdersDirectory, DateTime.Today.ToString("dd.MM.yyyy"), Path.GetFileName(fileName));
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"Moving to: ERROR {_errorOrdersDirectory} -- {moveToPath}");

				var destPath = moveFile(_newFileFullPath, moveToPath);

				this.saveError(destPath,
					errorMessage,
					customerName,
					customerId);


				Program.Notifier.PushEdiImportedOrdersNotification(new Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel()
				{
					Type = "Error",
					Payload = (Handlers.OrderError.OrderError.CountNotValidated(null) + 1).ToString()
				});
			}
		}

		public static string moveFile(string moveFrom,
			string moveTo)
		{
			Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"moving file from {moveFrom} to {moveTo}");
			lock(Locks.DocumentsLock)
			{
				try
				{
					moveTo = checkAndFixFileName(moveTo);
					createIfNotExists(moveTo);

					File.Move(moveFrom, moveTo);

					return moveTo;
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		private void saveError(string moveTo,
			string error,
			string customerName,
			int customerId)
		{
			// Get Customer
			var customerResponse = Handlers.Customers.Get(new List<int> { customerId });

			Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity
			{
				Error = error,
				FileName = moveTo,
				Validated = false,
				CustomerId = customerId,

				CustomerNumber = customerResponse.Success && customerResponse.Body.Count > 0 ? customerResponse.Body[0].CustomerNumber.ToString() : "-1",
				CustomerName = customerResponse.Success && customerResponse.Body.Count > 0 ? customerResponse.Body[0].Name : customerName,
				CreationTime = DateTime.Now
			});
		}

		public static string checkAndFixFileName(string filePath)
		{
			var extension = System.IO.Path.GetExtension(filePath);

			int i = 0;
			while(File.Exists(filePath))
			{
				i++;
				var toAppendText = Core.Helpers.StringHelpers.GenerateRandomKey(1, allowLetters: true, customChars: "_-+.%");
				filePath = filePath.Replace(extension, string.Concat(Enumerable.Repeat(toAppendText, i)) + extension);
			}

			return filePath;
		}

		public static void createIfNotExists(string filePath)
		{
			var dir = Path.GetDirectoryName(filePath);
			if(!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}
	}
}
