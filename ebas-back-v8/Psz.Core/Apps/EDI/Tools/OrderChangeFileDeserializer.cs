using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.EDI.Tools
{
	public class OrderChangeFileDeserializer
	{
		private string _newFileFullPath;
		private string _errorOrdersDirectory;
		private string _processedOrdersDirectory;

		public OrderChangeFileDeserializer(string newOrdersDirectory,
			string errorOrdersDirectory,
			string processedOrdersDirectory)
		{
			_newFileFullPath = newOrdersDirectory;
			_errorOrdersDirectory = errorOrdersDirectory;
			_processedOrdersDirectory = processedOrdersDirectory;
		}

		public void Deserialize(Models.OrderResponse.ErpelIndustryMessage data,
		   string documentName)
		{
			lock(Locks.DocumentsLock)
			{
				try
				{
					if(data.Document.Header.MessageHeader.MessageType.ToUpper() != "ORDCHG")
					{
						this.HandleError(documentName, "invalid MessageType", "Other", -1);
						return;
					}

					Models.Order.CreateModel createRequestData = null;

					string senderDuns = null;
					string senderId = null;
					string recipientId = null;

					var orderToBeId = -1;
					string documentNumber = null;
					string freeText = null;
					string customerContactName = null;
					string customerName = null;
					string customerPurchasingDepartment = null;
					string customerStreetPostalCode = null;
					string customerStreetCityPostalCode = null;
					string consigneeContactName = null;
					string consigneeName = null;
					string consigneePurchasingDepartment = null;
					string consigneeStreetPostalCode = null;
					string consigneeUnloadingPoint = null;
					string consigneeContactTelephone = null;
					string consigneeContactFax = null;
					string consigneeCountryName = null;

					// > Buyer
					string BuyerDuns = null;
					string BuyerPartyIdentification = null;
					string BuyerPartyIdentificationCodeListQualifier = null;
					string BuyerName = null;
					string BuyerName2 = null;
					string BuyerName3 = null;
					string BuyerStreet = null;
					string BuyerPostalCode = null;
					string BuyerCity = null;
					string BuyerCountryName = null;
					string BuyerPurchasingDepartment = null;
					// > Buyer >> Contact
					string BuyerContactName = null;
					string BuyerContactTelephone = null;
					string BuyerContactFax = null;

					// > Consignee
					string ConsigneeIdentification = null;
					string ConsigneeIdentificationCodeListQualifier = null;
					string ConsigneeDUNS = null;
					string ConsigneeName = null;
					string ConsigneeName2 = null;
					string ConsigneeName3 = null;
					string ConsigneeStreet = null;
					string ConsigneeCity = null;
					string ConsigneePostalCode = null;
					string ConsigneeCountryName = null;
					string ConsigneePurchasingDepartment = null;
					string ConsigneeUnloadingPoint = null;
					string ConsigneeStorageLocation = null;

					// > Consignee >> Contact
					string ConsigneeContactName = null;
					string ConsigneeContactTelephone = null;
					string ConsigneeContactFax = null;

					try
					{
						documentNumber = data.Document?.Header?.BeginningOfMessage?.DocumentNumber?.Trim();
						freeText = data.Document?.Header?.FreeText?.Text;

						recipientId = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.Recipient?.Id;
						senderId = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.Sender?.Id?.Trim();
						senderDuns = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.Sender?.Id?.Trim();
						if(string.IsNullOrEmpty(senderDuns) || string.IsNullOrWhiteSpace(senderDuns))
						{
							this.HandleError(documentName, $"Invalid Sender DUNS [{senderDuns}]", "Other", -1);
							return;
						}

						// > Buyer
						BuyerDuns = data.Document?.Header?.BusinessEntities?.Buyer?.DUNS.ToString();
						if(string.IsNullOrEmpty(BuyerDuns) || string.IsNullOrWhiteSpace(BuyerDuns))
						{
							this.HandleError(documentName, $"Invalid Buyer DUNS [{BuyerDuns}]", "Other", -1);
							return;
						}
						BuyerPartyIdentification = data.Document?.Header?.BusinessEntities?.Buyer?.PartyIdentification?.Trim();
						BuyerPartyIdentificationCodeListQualifier = data.Document?.Header?.BusinessEntities?.Buyer?.PartyIdentificationCodeListQualifier?.Trim();
						if(data.Document?.Header?.BusinessEntities?.Buyer?.PartyName != null)
						{
							if(data.Document.Header?.BusinessEntities?.Buyer?.PartyName.Count > 0)
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
						BuyerStreet = data.Document?.Header?.BusinessEntities?.Buyer?.Street?.Trim();
						BuyerPostalCode = data.Document?.Header?.BusinessEntities?.Buyer?.PostCode?.Trim();
						BuyerCity = data.Document?.Header?.BusinessEntities?.Buyer?.City?.Trim();
						BuyerCountryName = data.Document?.Header?.BusinessEntities?.Buyer?.Country?.CountryName?.Trim();
						BuyerPurchasingDepartment = data.Document?.Header?.BusinessEntities?.Buyer?.PurchasingDepartment?.Trim();
						// > Buyer >> Contact
						BuyerContactName = data.Document?.Header?.BusinessEntities?.Buyer?.Contact?.Name?.Trim();
						BuyerContactTelephone = data.Document?.Header?.BusinessEntities?.Buyer?.Contact?.Telephone?.Trim();
						BuyerContactFax = data.Document?.Header?.BusinessEntities?.Buyer?.Contact?.Fax?.Trim();

						// > Consignee
						ConsigneeIdentification = data.Document?.Header?.BusinessEntities?.Consignee?.PartyIdentification?.Trim();
						ConsigneeIdentificationCodeListQualifier = data.Document?.Header?.BusinessEntities?.Consignee?.PartyIdentificationCodeListQualifier?.Trim();
						ConsigneeDUNS = data.Document?.Header?.BusinessEntities?.Consignee?.DUNS?.Trim();
						if(data.Document?.Header?.BusinessEntities?.Buyer?.PartyName != null)
						{
							if(data.Document.Header?.BusinessEntities?.Consignee?.PartyName.Count > 0)
							{
								ConsigneeName = data.Document.Header?.BusinessEntities?.Consignee?.PartyName[0]?.Trim();
							}
							if(data.Document.Header?.BusinessEntities?.Consignee?.PartyName.Count > 1)
							{
								ConsigneeName2 = data.Document.Header?.BusinessEntities?.Consignee?.PartyName[1]?.Trim();
							}
							if(data.Document.Header?.BusinessEntities?.Consignee?.PartyName.Count > 2)
							{
								ConsigneeName3 = string.Join("||", data.Document.Header?.BusinessEntities?.Consignee?.PartyName?.Skip(2));
							}
						}
						ConsigneeStreet = data.Document?.Header?.BusinessEntities?.Consignee?.Street?.Trim();
						ConsigneeCity = data.Document?.Header?.BusinessEntities?.Consignee?.City?.Trim();
						ConsigneePostalCode = data.Document?.Header?.BusinessEntities?.Consignee?.PostCode?.Trim();
						ConsigneeCountryName = data.Document?.Header?.BusinessEntities?.Consignee?.Country?.CountryName?.Trim();
						ConsigneePurchasingDepartment = data.Document?.Header?.BusinessEntities?.Consignee?.PurchasingDepartment?.Trim();
						ConsigneeUnloadingPoint = data.Document?.Header?.BusinessEntities?.Consignee?.UnloadingPoint?.Trim();
						ConsigneeStorageLocation = data.Document?.Header?.BusinessEntities?.Consignee?.StorageLocation?.Trim();

						// > Consignee >> Contact
						ConsigneeContactName = data.Document?.Header?.BusinessEntities?.Consignee?.Contact?.Name?.Trim();
						ConsigneeContactTelephone = data.Document?.Header?.BusinessEntities?.Consignee?.Contact?.Telephone?.Trim();
						ConsigneeContactFax = data.Document?.Header?.BusinessEntities?.Consignee?.Contact?.Fax?.Trim();
					} catch(NullReferenceException e)
					{
						this.HandleError(documentName, "Invalid file format", "Other", -1);
						return;
					}

					createRequestData = new Models.Order.CreateModel()
					{
						DocumentName = documentName,
						DocumentNumber = documentNumber,
						FreeText = freeText,

						SenderId = senderId,
						RecipientId = recipientId,

						SenderDuns = senderDuns,

						// > Buyer
						BuyerDuns = BuyerDuns,
						BuyerPartyIdentification = BuyerPartyIdentification,
						BuyerPartyIdentificationCodeListQualifier = BuyerPartyIdentificationCodeListQualifier,
						BuyerName = BuyerName,
						BuyerName2 = BuyerName2,
						BuyerName3 = BuyerName3,
						BuyerStreet = BuyerStreet,
						BuyerPostalCode = BuyerPostalCode,
						BuyerCity = BuyerCity,
						BuyerCountryName = BuyerCountryName,
						BuyerPurchasingDepartment = BuyerPurchasingDepartment,
						// > Buyer >> Contact
						BuyerContactName = BuyerContactName,
						BuyerContactTelephone = BuyerContactTelephone,
						BuyerContactFax = BuyerContactFax,

						// > Consignee
						ConsigneeIdentification = ConsigneeIdentification,
						ConsigneeIdentificationCodeListQualifier = ConsigneeIdentificationCodeListQualifier,
						ConsigneeDUNS = ConsigneeDUNS,
						ConsigneeName = ConsigneeName,
						ConsigneeName2 = ConsigneeName2,
						ConsigneeName3 = ConsigneeName3,
						ConsigneeStreet = ConsigneeStreet,
						ConsigneeCity = ConsigneeCity,
						ConsigneePostalCode = ConsigneePostalCode,
						ConsigneeCountryName = ConsigneeCountryName,
						ConsigneePurchasingDepartment = ConsigneePurchasingDepartment,
						ConsigneeUnloadingPoint = ConsigneeUnloadingPoint,
						ConsigneeStorageLocation = ConsigneeStorageLocation,

						// > Consignee >> Contact
						ConsigneeContactName = ConsigneeContactName,
						ConsigneeContactTelephone = ConsigneeContactTelephone,
						ConsigneeContactFax = ConsigneeContactFax,

						Elements = new List<Models.Order.Element.NotCalculatedElementModel>(),
					};

					//
					var _id = 0;
					foreach(var documentElement in data.Document?.Details?.OrdersData?.OrdersLineItem)
					{
						string ConsigneeNameItem = null;
						string ConsigneeNameItem2 = null;
						string ConsigneeNameItem3 = null;
						if(documentElement.Consignee?.PartyName != null)
						{
							if(documentElement.Consignee?.PartyName.Count > 0)
							{
								ConsigneeNameItem = documentElement.Consignee?.PartyName[0]?.Trim();
							}
							if(documentElement.Consignee?.PartyName.Count > 1)
							{
								ConsigneeNameItem2 = documentElement.Consignee?.PartyName[1]?.Trim();
							}
							if(documentElement.Consignee?.PartyName.Count > 2)
							{
								ConsigneeNameItem3 = string.Join("||", documentElement.Consignee?.PartyName?.Skip(2));
							}
						}

						createRequestData.Elements.Add(new Models.Order.Element.NotCalculatedElementModel()
						{
							Id = _id,
							ItemNumber = documentElement.SuppliersItemMaterialNumber?.Trim(),
							CustomerItemNumber = Program.EDI?.Edi?.TrimSatrtZeroSenderIds?.FindIndex(x => x.ToLower().Trim() == senderId?.ToLower()) >= 0 ? documentElement.CustomersItemMaterialNumber?.Trim()?.TrimStart('0') : documentElement.CustomersItemMaterialNumber?.Trim(),// -- 2022-04-10 Sirona trimStart and Horsch not!, // 2022-04-08 - Khelil remove TrimStart Leading for Error from Horsch
							FreeText = documentElement.FreeText?.Text?.Trim(),
							DesiredDate = Convert.ToDateTime(documentElement.OrdersScheduleLine?[0]?.ScheduledQuantityDate?.DateTime2),
							ItemDescription = documentElement.ItemDescription?.Trim(),
							MeasureUnitQualifier = documentElement.MeasureUnitQualifier?.Trim(),
							CurrentItemPriceCalculationNet = !string.IsNullOrEmpty(documentElement.CurrentItemPriceCalculationNet)
								? Convert.ToDecimal(documentElement.CurrentItemPriceCalculationNet, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
								: 0,
							OrderedQuantity = !string.IsNullOrEmpty(documentElement.OrderedQuantity)
								? Convert.ToDecimal(documentElement.OrderedQuantity, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
								: 0,
							PositionNumber = documentElement.PositionNumber,
							UnitPriceBasis = !string.IsNullOrEmpty(documentElement.UnitPriceBasis)
								? Convert.ToDecimal(documentElement.UnitPriceBasis, Psz.Core.Helpers.StringHelpers.NUMERIC_CULTURE_INFO)
								: 0, // >>>>>>>>>>>>>>> CONFIRM !!!!!
							LineItemAmount = Convert.ToDecimal(documentElement.LineItemAmount),
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
								ConsigneeName = ConsigneeNameItem,
								ConsigneeName2 = ConsigneeNameItem2,
								ConsigneeName3 = ConsigneeNameItem3,
								ConsigneePostalCode = documentElement.Consignee?.PostCode?.Trim(),
								ConsigneePurchasingDepartment = documentElement.Consignee?.PurchasingDepartment?.Trim(),
								ConsigneeStorageLocation = documentElement.Consignee?.StorageLocation?.Trim(),
								ConsigneeStreet = documentElement.Consignee?.Street?.Trim(),
								ConsigneeUnloadingPoint = documentElement.Consignee?.UnloadingPoint?.Trim()
							}
						});

						_id += 1;
					}

					var createResponse = Handlers.Order.CreateInternal(createRequestData, null, out orderToBeId);
					if(createResponse.Success)
					{

						// > Move File
						this.moveFile(_newFileFullPath, Path.Combine(_processedOrdersDirectory, DateTime.Today.ToString("dd.MM.yyyy"), Path.GetFileName(documentName)));

						// > Success Notification
						Program.Notifier.PushEdiImportedOrdersNotification(new Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel()
						{
							Type = "Success",
							Payload = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CountCustomerOrdersByIsNew(true).ToString()
						});
					}
					else
					{
						this.HandleError(documentName, string.Join(", ", createResponse.Errors), "Other", createResponse.Body);
					}

					// >>> Save raw data
					XMLFile.SaveFileToDB(orderToBeId, data);
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

				this.saveError(moveToPath,
					errorMessage,
					customerName,
					customerId);

				this.moveFile(_newFileFullPath,
					moveToPath);

				Program.Notifier.PushEdiImportedOrdersNotification(new Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel()
				{
					Type = "Error",
					Payload = (Handlers.OrderError.OrderError.CountNotValidated(null) + 1).ToString()
				});
			}
		}

		private void moveFile(string moveFrom,
			string moveTo)
		{
			lock(Locks.DocumentsLock)
			{
				try
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"moving file from {moveFrom} to {moveTo}");
					moveTo = checkAndFixFileName(moveTo);
					OportedOrdersHandler.createIfNotExists(moveTo);

					File.Move(moveFrom, moveTo);
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

		private static string checkAndFixFileName(string filePath)
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
	}
}
