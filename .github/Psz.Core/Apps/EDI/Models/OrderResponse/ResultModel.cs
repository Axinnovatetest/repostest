using System.Collections.Generic;
using System.Xml.Serialization;

namespace Psz.Core.Apps.EDI.Models.OrderResponse
{
	[XmlRoot(ElementName = "Sender", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public class Sender
	{
		[XmlElement(ElementName = "id", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public string Id { get; set; }

		//[XmlElement(ElementName = "DUNS", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		//      public int DUNS { get; set; }
	}

	[XmlRoot(ElementName = "Recipient", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public class Recipient
	{
		[XmlElement(ElementName = "id", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public string Id { get; set; }
	}

	[XmlRoot(ElementName = "DateTime", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public class DateTime
	{
		[XmlElement(ElementName = "date", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public string Date { get; set; }
		[XmlElement(ElementName = "time", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public string Time { get; set; }
	}

	[XmlRoot(ElementName = "InterchangeHeader", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public class InterchangeHeader
	{
		[XmlElement(ElementName = "Sender", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public Sender Sender { get; set; }
		[XmlElement(ElementName = "Recipient", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public Recipient Recipient { get; set; }
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public DateTime DateTime { get; set; }
		[XmlElement(ElementName = "ControlRef", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public string ControlRef { get; set; }
		[XmlElement(ElementName = "ApplicationRef", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public string ApplicationRef { get; set; }
		[XmlElement(ElementName = "TestIndicator", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public string TestIndicator { get; set; }
	}

	[XmlRoot(ElementName = "ErpelBusinessDocumentHeader", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public class ErpelBusinessDocumentHeader
	{
		[XmlElement(ElementName = "MessageReceivedAt", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public string MessageReceivedAt { get; set; }
		[XmlElement(ElementName = "InterchangeHeader", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public InterchangeHeader InterchangeHeader { get; set; }
	}

	[XmlRoot(ElementName = "MessageHeader", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class MessageHeader
	{
		[XmlElement(ElementName = "MessageReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string MessageReferenceNumber { get; set; }
		[XmlElement(ElementName = "MessageType", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string MessageType { get; set; }
	}

	[XmlRoot(ElementName = "BeginningOfMessage", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class BeginningOfMessage
	{
		[XmlElement(ElementName = "DocumentNameEncoded", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DocumentNameEncoded { get; set; }
		[XmlElement(ElementName = "DocumentNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DocumentNumber { get; set; }
		[XmlElement(ElementName = "MessageFunction", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string MessageFunction { get; set; }
	}

	[XmlRoot(ElementName = "DocumentDate", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class DocumentDate
	{
		[XmlElement(ElementName = "DateQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DateQualifier { get; set; }
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DateTime2 { get; set; }
	}

	[XmlRoot(ElementName = "Date", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Date
	{
		[XmlElement(ElementName = "DateQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DateQualifier { get; set; }
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DateTime2 { get; set; }
	}

	[XmlRoot(ElementName = "Dates", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Dates
	{
		[XmlElement(ElementName = "DocumentDate", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public DocumentDate DocumentDate { get; set; }
		[XmlElement(ElementName = "Date", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Date Date { get; set; }
	}

	[XmlRoot(ElementName = "PurchaseOrderReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class PurchaseOrderReferenceNumber
	{
		[XmlElement(ElementName = "ReferenceQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceQualifier { get; set; }
		[XmlElement(ElementName = "ReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceNumber { get; set; }
		[XmlElement(ElementName = "Date", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Date Date { get; set; }
	}

	[XmlRoot(ElementName = "ReferencedDocument", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class ReferencedDocument
	{
		[XmlElement(ElementName = "ReferenceQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceQualifier { get; set; }
		[XmlElement(ElementName = "ReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceNumber { get; set; }
		[XmlElement(ElementName = "Date", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Date Date { get; set; }
	}

	[XmlRoot(ElementName = "ReferencedDocuments", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class ReferencedDocuments
	{
		[XmlElement(ElementName = "PurchaseOrderReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public PurchaseOrderReferenceNumber PurchaseOrderReferenceNumber { get; set; }
		[XmlElement(ElementName = "ReferencedDocument", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public ReferencedDocument ReferencedDocument { get; set; }
	}

	[XmlRoot(ElementName = "Contact", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Contact
	{
		[XmlElement(ElementName = "Name", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Telephone", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string Telephone { get; set; }
		[XmlElement(ElementName = "Fax", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string Fax { get; set; }
	}

	[XmlRoot(ElementName = "Buyer", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Buyer
	{
		[XmlElement(ElementName = "PartyQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyQualifier { get; set; }
		[XmlElement(ElementName = "PartyIdentification", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyIdentification { get; set; }
		[XmlElement(ElementName = "PartyIdentificationCodeListQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyIdentificationCodeListQualifier { get; set; }
		[XmlElement(ElementName = "DUNS", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DUNS { get; set; }
		[XmlElement(ElementName = "PartyName", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public List<string> PartyName { get; set; }
		[XmlElement(ElementName = "Street", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string City { get; set; }
		[XmlElement(ElementName = "PostCode", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PostCode { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Country Country { get; set; }
		[XmlElement(ElementName = "Contact", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Contact Contact { get; set; }

		[XmlElement(ElementName = "PurchasingDepartment", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PurchasingDepartment { get; set; }
	}

	[XmlRoot(ElementName = "Country", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Country
	{
		[XmlElement(ElementName = "CountryName", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string CountryName { get; set; }
	}

	[XmlRoot(ElementName = "Supplier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Supplier
	{
		[XmlElement(ElementName = "PartyQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyQualifier { get; set; }
		[XmlElement(ElementName = "PartyIdentification", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyIdentification { get; set; }
		[XmlElement(ElementName = "PartyIdentificationCodeListQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyIdentificationCodeListQualifier { get; set; }
		[XmlElement(ElementName = "DUNS", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DUNS { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Country Country { get; set; }
		//[XmlElement(ElementName = "Contact", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")] // >>>>> Sirona is sending ithis as a class
		//public string Contact { get; set; }
	}

	[XmlRoot(ElementName = "Consignee", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Consignee
	{
		[XmlElement(ElementName = "PartyQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyQualifier { get; set; }
		[XmlElement(ElementName = "PartyIdentification", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyIdentification { get; set; }
		[XmlElement(ElementName = "PartyIdentificationCodeListQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PartyIdentificationCodeListQualifier { get; set; }
		[XmlElement(ElementName = "DUNS", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DUNS { get; set; }
		[XmlElement(ElementName = "PartyName", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public List<string> PartyName { get; set; }
		[XmlElement(ElementName = "Street", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string Street { get; set; }
		[XmlElement(ElementName = "City", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string City { get; set; }
		[XmlElement(ElementName = "PostCode", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PostCode { get; set; }
		[XmlElement(ElementName = "Country", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Country Country { get; set; }
		[XmlElement(ElementName = "Contact", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Contact Contact { get; set; }
		[XmlElement(ElementName = "UnloadingPoint", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string UnloadingPoint { get; set; }
		[XmlElement(ElementName = "StorageLocation", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string StorageLocation { get; set; }

		[XmlElement(ElementName = "PurchasingDepartment", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PurchasingDepartment { get; set; }
	}

	[XmlRoot(ElementName = "BusinessEntities", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class BusinessEntities
	{
		[XmlElement(ElementName = "Buyer", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Buyer Buyer { get; set; }
		[XmlElement(ElementName = "Supplier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Supplier Supplier { get; set; }
		[XmlElement(ElementName = "Consignee", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Consignee Consignee { get; set; }
	}

	[XmlRoot(ElementName = "Header", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Header
	{
		[XmlElement(ElementName = "MessageHeader", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public MessageHeader MessageHeader { get; set; }
		[XmlElement(ElementName = "BeginningOfMessage", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public BeginningOfMessage BeginningOfMessage { get; set; }
		[XmlElement(ElementName = "Dates", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Dates Dates { get; set; }
		[XmlElement(ElementName = "ReferenceCurrency", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceCurrency { get; set; }
		[XmlElement(ElementName = "ReferencedDocuments", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public ReferencedDocuments ReferencedDocuments { get; set; }
		[XmlElement(ElementName = "BusinessEntities", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public BusinessEntities BusinessEntities { get; set; }
		[XmlElement(ElementName = "FreeText", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public FreeText FreeText { get; set; }
	}

	[XmlRoot(ElementName = "FreeText", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class FreeText
	{
		[XmlElement(ElementName = "TextQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string TextQualifier { get; set; }
		[XmlElement(ElementName = "TextLanguage", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string TextLanguage { get; set; }
		[XmlElement(ElementName = "Text", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "OrderReference", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class OrderReference
	{
		[XmlElement(ElementName = "ReferenceQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceQualifier { get; set; }
		[XmlElement(ElementName = "ReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceNumber { get; set; }
		[XmlElement(ElementName = "LineNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string LineNumber { get; set; }
		[XmlElement(ElementName = "Date", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Date Date { get; set; }
	}

	[XmlRoot(ElementName = "References", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class References
	{
		[XmlElement(ElementName = "OrderReference", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public OrderReference OrderReference { get; set; }
	}

	[XmlRoot(ElementName = "ScheduledQuantityDate", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class ScheduledQuantityDate
	{
		[XmlElement(ElementName = "DateQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DateQualifier { get; set; }
		[XmlElement(ElementName = "DateTime", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string DateTime2 { get; set; }
	}

	[XmlRoot(ElementName = "OrdersScheduleLine", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class OrdersScheduleLine
	{
		[XmlElement(ElementName = "ScheduledQuantity", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ScheduledQuantity { get; set; }
		[XmlElement(ElementName = "ScheduledQuantityDate", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public ScheduledQuantityDate ScheduledQuantityDate { get; set; }
		[XmlElement(ElementName = "PreviousScheduledQuantity", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PreviousScheduledQuantity { get; set; }
	}

	[XmlRoot(ElementName = "AllowancesAndCharges", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class AllowancesAndCharges
	{
		[XmlElement(ElementName = "AllowanceChargeQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string AllowanceChargeQualifier { get; set; }

		[XmlElement(ElementName = "AllowanceChargeAmount", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string AllowanceChargeAmount { get; set; }

		[XmlElement(ElementName = "AllowanceChargeComment", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string AllowanceChargeComment { get; set; }
	}

	[XmlRoot(ElementName = "OrdersLineItem", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class OrdersLineItem
	{
		[XmlElement(ElementName = "PositionNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public int PositionNumber { get; set; }
		[XmlElement(ElementName = "CustomersItemMaterialNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string CustomersItemMaterialNumber { get; set; }
		[XmlElement(ElementName = "SuppliersItemMaterialNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string SuppliersItemMaterialNumber { get; set; }
		[XmlElement(ElementName = "ItemDescription", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ItemDescription { get; set; }
		[XmlElement(ElementName = "OrderedQuantity", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string OrderedQuantity { get; set; }
		[XmlElement(ElementName = "MeasureUnitQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string MeasureUnitQualifier { get; set; }
		[XmlElement(ElementName = "CurrentItemPriceCalculationNet", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string CurrentItemPriceCalculationNet { get; set; }

		[XmlElement(ElementName = "CurrentItemPriceCalculationGross", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string CurrentItemPriceCalculationGross { get; set; }
		[XmlElement(ElementName = "UnitPriceBasis", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string UnitPriceBasis { get; set; }
		[XmlElement(ElementName = "LineItemAmount", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string LineItemAmount { get; set; }
		[XmlElement(ElementName = "LineItemActionRequest", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string LineItemActionRequest { get; set; }
		[XmlElement(ElementName = "FreeText", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public FreeText FreeText { get; set; }
		[XmlElement(ElementName = "References", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public References References { get; set; }

		[XmlElement(ElementName = "ItemCategory", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ItemCategory { get; set; }
		[XmlElement(ElementName = "Consignee", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Consignee Consignee { get; set; }

		[XmlElement(ElementName = "OrdersScheduleLine", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public List<OrdersScheduleLine> OrdersScheduleLine { get; set; }

		[XmlElement(ElementName = "AllowancesAndCharges", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public AllowancesAndCharges AllowancesAndCharges { get; set; }
	}

	[XmlRoot(ElementName = "OrdersData", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class OrdersData
	{
		[XmlElement(ElementName = "OrdersLineItem", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public List<OrdersLineItem> OrdersLineItem { get; set; }
	}

	[XmlRoot(ElementName = "DeliveryLineItems", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class DeliveryLineItems
	{
		[XmlElement(ElementName = "PositionNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string PositionNumber { get; set; }
		[XmlElement(ElementName = "ShippingMark", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ShippingMark { get; set; }
		[XmlElement(ElementName = "NetWeight", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string NetWeight { get; set; }
		[XmlElement(ElementName = "Weight", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string Weight { get; set; }
	}

	[XmlRoot(ElementName = "ConsignmentPackagingSequence", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class ConsignmentPackagingSequence
	{
		[XmlElement(ElementName = "DeliveryLineItems", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public List<DeliveryLineItems> DeliveryLineItems { get; set; }
	}

	[XmlRoot(ElementName = "DispatchData", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class DispatchData
	{
		[XmlElement(ElementName = "ConsignmentPackagingSequence", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public ConsignmentPackagingSequence ConsignmentPackagingSequence { get; set; }
	}

	[XmlRoot(ElementName = "Details", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Details
	{
		[XmlElement(ElementName = "OrdersData", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public OrdersData OrdersData { get; set; }
		[XmlElement(ElementName = "DispatchData", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public DispatchData DispatchData { get; set; }
	}

	[XmlRoot(ElementName = "InvoiceTotals", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class InvoiceTotals
	{
		[XmlElement(ElementName = "TotalTaxableAmount", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string TotalTaxableAmount { get; set; }
		[XmlElement(ElementName = "InvoiceAmount", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string InvoiceAmount { get; set; }
		[XmlElement(ElementName = "TotalLineItemAmount", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string TotalLineItemAmount { get; set; }
		[XmlElement(ElementName = "CurrencyQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string CurrencyQualifier { get; set; }
	}

	[XmlRoot(ElementName = "InvoiceFooter", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class InvoiceFooter
	{
		[XmlElement(ElementName = "InvoiceTotals", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public InvoiceTotals InvoiceTotals { get; set; }
	}

	[XmlRoot(ElementName = "Footer", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Footer
	{
		[XmlElement(ElementName = "InvoiceFooter", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public InvoiceFooter InvoiceFooter { get; set; }
	}

	[XmlRoot(ElementName = "Document", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class Document
	{
		[XmlElement(ElementName = "Header", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Header Header { get; set; }
		[XmlElement(ElementName = "Details", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Details Details { get; set; }
		[XmlElement(ElementName = "Footer", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Footer Footer { get; set; }
	}

	[XmlRoot(ElementName = "ErpelIndustryMessage", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/message")]
	public class ErpelIndustryMessage
	{
		[XmlElement(ElementName = "ErpelBusinessDocumentHeader", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public ErpelBusinessDocumentHeader ErpelBusinessDocumentHeader { get; set; }
		[XmlElement(ElementName = "Document", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Document Document { get; set; }
		[XmlAttribute(AttributeName = "document", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string _Document { get; set; }
		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }
		[XmlAttribute(AttributeName = "header", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Header { get; set; }
	}
}

