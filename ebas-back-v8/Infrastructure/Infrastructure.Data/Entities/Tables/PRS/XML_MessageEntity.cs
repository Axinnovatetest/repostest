using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class XML_MessageEntity
	{
		public DateTime? EditTime { get; set; }
		public int Id { get; set; }
		public int IdFile { get; set; }
		public int IdOrder { get; set; }
		public string Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier { get; set; }
		public string Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount { get; set; }
		public string Message_Document_Header_BeginningOfMessage_DocumentNameEncoded { get; set; }
		public string Message_Document_Header_BeginningOfMessage_DocumentNumber { get; set; }
		public string Message_Document_Header_BeginningOfMessage_MessageFunction { get; set; }
		public string Message_Document_Header_Dates_Date_DateQualifier { get; set; }
		public string Message_Document_Header_Dates_Date_DateTime { get; set; }
		public string Message_Document_Header_Dates_DocumentDate_DateQualifier { get; set; }
		public string Message_Document_Header_Dates_DocumentDate_DateTime { get; set; }
		public string Message_Document_Header_MessageHeader_MessageReferenceNumber { get; set; }
		public string Message_Document_Header_MessageHeader_MessageType { get; set; }
		public string Message_Document_Header_ReferenceCurrency { get; set; }
		public string Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier { get; set; }
		public string Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime { get; set; }
		public string Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber { get; set; }
		public string Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier { get; set; }
		public string Message_Header_InterchangeHeader_ApplicationRef { get; set; }
		public string Message_Header_InterchangeHeader_ControlRef { get; set; }
		public string Message_Header_InterchangeHeader_DateTime_date { get; set; }
		public string Message_Header_InterchangeHeader_DateTime_time { get; set; }
		public string Message_Header_InterchangeHeader_Recipient_id { get; set; }
		public string Message_Header_InterchangeHeader_Sender_id { get; set; }

		public XML_MessageEntity() { }

		public XML_MessageEntity(DataRow dataRow)
		{
			EditTime = (dataRow["EditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EditTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdFile = Convert.ToInt32(dataRow["IdFile"]);
			IdOrder = Convert.ToInt32(dataRow["IdOrder"]);
			Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier = (dataRow["Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier"]);
			Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount = (dataRow["Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount"]);
			Message_Document_Header_BeginningOfMessage_DocumentNameEncoded = (dataRow["Message_Document_Header_BeginningOfMessage_DocumentNameEncoded"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_BeginningOfMessage_DocumentNameEncoded"]);
			Message_Document_Header_BeginningOfMessage_DocumentNumber = (dataRow["Message_Document_Header_BeginningOfMessage_DocumentNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_BeginningOfMessage_DocumentNumber"]);
			Message_Document_Header_BeginningOfMessage_MessageFunction = (dataRow["Message_Document_Header_BeginningOfMessage_MessageFunction"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_BeginningOfMessage_MessageFunction"]);
			Message_Document_Header_Dates_Date_DateQualifier = (dataRow["Message_Document_Header_Dates_Date_DateQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_Dates_Date_DateQualifier"]);
			Message_Document_Header_Dates_Date_DateTime = (dataRow["Message_Document_Header_Dates_Date_DateTime"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_Dates_Date_DateTime"]);
			Message_Document_Header_Dates_DocumentDate_DateQualifier = (dataRow["Message_Document_Header_Dates_DocumentDate_DateQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_Dates_DocumentDate_DateQualifier"]);
			Message_Document_Header_Dates_DocumentDate_DateTime = (dataRow["Message_Document_Header_Dates_DocumentDate_DateTime"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_Dates_DocumentDate_DateTime"]);
			Message_Document_Header_MessageHeader_MessageReferenceNumber = (dataRow["Message_Document_Header_MessageHeader_MessageReferenceNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_MessageHeader_MessageReferenceNumber"]);
			Message_Document_Header_MessageHeader_MessageType = (dataRow["Message_Document_Header_MessageHeader_MessageType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_MessageHeader_MessageType"]);
			Message_Document_Header_ReferenceCurrency = (dataRow["Message_Document_Header_ReferenceCurrency"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_ReferenceCurrency"]);
			Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier = (dataRow["Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier"]);
			Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime = (dataRow["Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime"]);
			Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber = (dataRow["Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber"]);
			Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier = (dataRow["Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier"]);
			Message_Header_InterchangeHeader_ApplicationRef = (dataRow["Message_Header_InterchangeHeader_ApplicationRef"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Header_InterchangeHeader_ApplicationRef"]);
			Message_Header_InterchangeHeader_ControlRef = (dataRow["Message_Header_InterchangeHeader_ControlRef"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Header_InterchangeHeader_ControlRef"]);
			Message_Header_InterchangeHeader_DateTime_date = (dataRow["Message_Header_InterchangeHeader_DateTime_date"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Header_InterchangeHeader_DateTime_date"]);
			Message_Header_InterchangeHeader_DateTime_time = (dataRow["Message_Header_InterchangeHeader_DateTime_time"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Header_InterchangeHeader_DateTime_time"]);
			Message_Header_InterchangeHeader_Recipient_id = (dataRow["Message_Header_InterchangeHeader_Recipient_id"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Header_InterchangeHeader_Recipient_id"]);
			Message_Header_InterchangeHeader_Sender_id = (dataRow["Message_Header_InterchangeHeader_Sender_id"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message_Header_InterchangeHeader_Sender_id"]);
		}
	}
}

