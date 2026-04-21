using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Tools
{
	public class XMLFile
	{
		public static void SaveFileToDB(int orderId,
			Psz.Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage data)
		{
			try
			{
				var artikelEntities = orderId >= 0
					? Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderId)
					: null;

				var idFile = Infrastructure.Data.Access.Tables.PRS.XML_MessageAccess.GetMaxFileId();

				var message = ToMessageEntity(orderId, idFile, data);
				Infrastructure.Data.Access.Tables.PRS.XML_MessageAccess.Insert(message);

				var orderLines = ToOrdersLineItemEntities(idFile, data);
				if(orderLines != null && orderLines.Count > 0)
				{
					var orderSchedules = ToOrdersScheduleLineEntities(artikelEntities, data);
					for(int i = 0; i < orderLines.Count; i++)
					{
						var id = Infrastructure.Data.Access.Tables.PRS.XML_OrdersLineItemAccess.Insert(orderLines[i]);
						if(id >= 0)
						{
							var scheduleLines = orderSchedules?.FindAll(x => x.IdOrdersLineItem == i);
							if(scheduleLines != null && scheduleLines.Count > 0)
							{
								for(int j = 0; j < scheduleLines.Count; j++)
								{
									scheduleLines[j].IdOrdersLineItem = id;
								}
								Infrastructure.Data.Access.Tables.PRS.XML_OrdersScheduleLineAccess.Insert(scheduleLines);
							}
						}
					}
				}

				var deliveryItems = ToDeliveryLineItemEntities(idFile, data);
				Infrastructure.Data.Access.Tables.PRS.XML_DeliveryLineItemAccess.Insert(deliveryItems);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public static Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity ToMessageEntity(int orderId, int idFile,
			Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage data)
		{
			return new Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity
			{
				Id = -1,
				IdOrder = orderId,
				IdFile = idFile,
				Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier = data?.Document?.Footer?.InvoiceFooter?.InvoiceTotals?.CurrencyQualifier?.Trim(),
				Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount = data?.Document?.Footer?.InvoiceFooter?.InvoiceTotals?.TotalLineItemAmount?.Trim(),
				Message_Document_Header_BeginningOfMessage_DocumentNameEncoded = data?.Document?.Header?.BeginningOfMessage?.DocumentNameEncoded?.Trim(),
				Message_Document_Header_BeginningOfMessage_DocumentNumber = data?.Document?.Header?.BeginningOfMessage?.DocumentNumber?.Trim(),
				Message_Document_Header_BeginningOfMessage_MessageFunction = data?.Document?.Header?.BeginningOfMessage?.MessageFunction?.Trim(),
				Message_Document_Header_Dates_Date_DateQualifier = data?.Document?.Header?.Dates?.Date?.DateQualifier?.Trim(),
				Message_Document_Header_Dates_Date_DateTime = data?.Document?.Header?.Dates?.Date?.DateTime2?.Trim(),
				Message_Document_Header_Dates_DocumentDate_DateQualifier = data?.Document?.Header?.Dates?.DocumentDate?.DateQualifier?.Trim(),
				Message_Document_Header_Dates_DocumentDate_DateTime = data?.Document?.Header?.Dates?.DocumentDate?.DateTime2?.Trim(),
				Message_Document_Header_MessageHeader_MessageReferenceNumber = data?.Document?.Header?.MessageHeader?.MessageReferenceNumber?.Trim(),
				Message_Document_Header_MessageHeader_MessageType = data?.Document?.Header?.MessageHeader?.MessageType?.Trim(),
				Message_Document_Header_ReferenceCurrency = data?.Document?.Header?.ReferenceCurrency?.Trim(),
				Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier = data?.Document?.Header?.ReferencedDocuments?.PurchaseOrderReferenceNumber?.Date?.DateQualifier?.Trim(),
				Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime = data?.Document?.Header?.ReferencedDocuments?.PurchaseOrderReferenceNumber?.Date?.DateTime2?.Trim(),
				Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber = data?.Document?.Header?.ReferencedDocuments?.PurchaseOrderReferenceNumber?.ReferenceNumber?.Trim(),
				Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier = data?.Document?.Header?.ReferencedDocuments?.PurchaseOrderReferenceNumber?.ReferenceQualifier?.Trim(),
				Message_Header_InterchangeHeader_ApplicationRef = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.ApplicationRef?.Trim(),
				Message_Header_InterchangeHeader_ControlRef = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.ControlRef?.Trim(),
				Message_Header_InterchangeHeader_DateTime_date = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.DateTime?.Date?.Trim(),
				Message_Header_InterchangeHeader_DateTime_time = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.DateTime?.Time?.Trim(),
				Message_Header_InterchangeHeader_Recipient_id = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.Recipient?.Id?.Trim(),
				Message_Header_InterchangeHeader_Sender_id = data?.ErpelBusinessDocumentHeader?.InterchangeHeader?.Sender?.Id?.Trim(),
			};
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> ToOrdersLineItemEntities(int idFile,
			Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage data)
		{
			if(data != null
				&& data.Document != null
				&& data.Document.Details != null
				&& data.Document.Details.OrdersData != null
				&& data.Document.Details.OrdersData.OrdersLineItem != null
				&& data.Document.Details.OrdersData.OrdersLineItem.Count > 0)
			{
				return data.Document.Details.OrdersData.OrdersLineItem.Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity
				{
					Id = -1,
					IdFile = idFile,
					Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet = x?.CurrentItemPriceCalculationNet?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber = x?.CustomersItemMaterialNumber?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage = x?.FreeText?.TextLanguage?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier = x?.FreeText?.TextQualifier?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text = x?.FreeText?.Text?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription = x?.ItemDescription?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest = x?.LineItemActionRequest?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount = x?.LineItemAmount?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier = x?.MeasureUnitQualifier?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity = x?.OrderedQuantity?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber = x?.PositionNumber.ToString()?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier = x?.References?.OrderReference?.Date?.DateQualifier?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime = x?.References?.OrderReference?.Date?.DateTime2?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber = x?.References?.OrderReference?.LineNumber?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber = x?.References?.OrderReference?.ReferenceNumber?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier = x?.References?.OrderReference?.ReferenceQualifier?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber = x?.SuppliersItemMaterialNumber?.Trim(),
					Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis = x?.UnitPriceBasis?.Trim(),
				}).ToList();
			}

			return null;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> ToOrdersScheduleLineEntities(
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> artikelEntities,
			Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage data)
		{
			if(data != null
				&& data.Document != null
				&& data.Document.Details != null
				&& data.Document.Details.OrdersData != null
				&& data.Document.Details.OrdersData.OrdersLineItem != null
				&& data.Document.Details.OrdersData.OrdersLineItem.Count > 0)
			{


				var items = new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity>();
				for(int i = 0; i < data.Document.Details.OrdersData.OrdersLineItem.Count; i++)
				{
					var item = data.Document.Details.OrdersData.OrdersLineItem[i];
					if(item.OrdersScheduleLine != null && item.OrdersScheduleLine.Count > 0)
					{
						foreach(var x in item.OrdersScheduleLine)
						{
							var idx = artikelEntities?.FindIndex(y => y.Position == item.PositionNumber
								&& y.OriginalAnzahl == Convert.ToDecimal(x.ScheduledQuantity)
								&& y.Wunschtermin == Convert.ToDateTime(x.ScheduledQuantityDate.DateTime2));

							items.Add(new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity
							{
								Id = -1,
								IdOrderElement = idx.HasValue && idx >= 0 ? artikelEntities[idx.Value].Nr : -1,
								IdOrdersLineItem = i, // >>> Link to bind with Order Line
								Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity = x?.PreviousScheduledQuantity?.Trim(),
								Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity = x?.ScheduledQuantity?.Trim(),
								Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier = x?.ScheduledQuantityDate?.DateQualifier?.Trim(),
								Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime = x?.ScheduledQuantityDate?.DateTime2?.Trim(),
								CreateTime = System.DateTime.Now
							});
						}
					}
				}

				return items;
			}
			return null;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> ToDeliveryLineItemEntities(int idFile,
			Core.Apps.EDI.Models.OrderResponse.ErpelIndustryMessage data)
		{
			if(data != null
				&& data.Document != null
				&& data.Document.Details != null
				&& data.Document.Details.DispatchData != null
				&& data.Document.Details.DispatchData.ConsignmentPackagingSequence != null
				&& data.Document.Details.DispatchData.ConsignmentPackagingSequence.DeliveryLineItems != null
				&& data.Document.Details.DispatchData.ConsignmentPackagingSequence.DeliveryLineItems.Count > 0)
			{

				return data.Document.Details.DispatchData.ConsignmentPackagingSequence.DeliveryLineItems.Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity
				{
					Id = -1,
					IdFile = idFile,
					Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight = x?.NetWeight?.Trim(),
					Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber = x?.PositionNumber?.Trim(),
					Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark = x?.ShippingMark?.Trim(),
					Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight = x?.Weight?.Trim(),
				}).ToList();
			}

			return null;
		}
	}
}
