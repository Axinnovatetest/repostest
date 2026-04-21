
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Psz.Core.Apps.EDI.Models.DespatchAdvice
{
	// Define a static class to store namespaces
	public static class Namespaces
	{
		public const string Document = "http://erpel.at/schemas/1p0/documents";
		public const string Ext = "http://erpel.at/schemas/1p0/documents/ext";
		public const string Bosch = "http://erpel.at/schemas/1p0/documents/extensions/bosch";
		public const string Edifact = "http://erpel.at/schemas/1p0/documents/extensions/edifact";
		public const string Klosterquell = "http://erpel.at/schemas/1p0/documents/extensions/klosterquell";
		public const string Header = "http://erpel.at/schemas/1p0/messaging/header";
		public const string Erpel = "http://erpel.at/schemas/1p0/messaging/message";
		public const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
		public const string SchemaLocation = "http://erpel.at/schemas/1p0/messaging/message file://.psf/Home/schemas/Schemas/ERPEL1p0/Message.xsd";
	}

	[XmlRoot(ElementName = "Sender")]
	public class Sender
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "CodeQualifier")]
		public string CodeQualifier { get; set; }
	}

	[XmlRoot(ElementName = "Recipient")]
	public class Recipient
	{
		[XmlElement(ElementName = "id")]
		public string Id { get; set; }
		[XmlElement(ElementName = "CodeQualifier")]
		public string CodeQualifier { get; set; }
	}

	[XmlRoot(ElementName = "DateTime")]
	public class DateTime
	{
		[XmlElement(ElementName = "date")]
		public string Date { get; set; }
		[XmlElement(ElementName = "time")]
		public string Time { get; set; }
	}

	[XmlRoot(ElementName = "InterchangeHeader")]
	public class InterchangeHeader
	{
		[XmlElement(ElementName = "Sender")]
		public Sender Sender { get; set; }
		[XmlElement(ElementName = "Recipient")]
		public Recipient Recipient { get; set; }
		[XmlElement(ElementName = "DateTime")]
		public DateTime DateTime { get; set; }
		[XmlElement(ElementName = "ControlRef")]
		public string ControlRef { get; set; }
		[XmlElement(ElementName = "ApplicationRef")]
		public string ApplicationRef { get; set; }
		[XmlElement(ElementName = "TestIndicator")]
		public string TestIndicator { get; set; }
	}

	[XmlRoot(ElementName = "ErpelBusinessDocumentHeader")]
	public class ErpelBusinessDocumentHeader
	{
		[XmlElement(ElementName = "InterchangeHeader")]
		public InterchangeHeader InterchangeHeader { get; set; }
	}


	[XmlRoot(ElementName = "DeliveryRecipient")]
	public class DeliveryRecipient
	{

		[XmlElement(ElementName = "FurtherIdentification")]
		public List<FurtherIdentification> FurtherIdentification { get; set; }

		[XmlElement(ElementName = "Address")]
		public Address Address { get; set; }
	}
	public class FurtherIdentification
	{
		[XmlAttribute(AttributeName = "IdentificationType", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string IdentificationType { get; set; }
		[XmlText]
		public string Text { get; set; }
	}
	// -
	public class ContainedPackagingItems
	{
		[XmlAttribute(AttributeName = "SupplierUnit", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string SupplierUnit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}
	public class Quantity
	{
		[XmlAttribute(AttributeName = "Unit", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string Unit { get; set; }
		[XmlText]
		public string Text { get; set; }
	}
	public class AdditionalDate
	{
		[XmlAttribute(AttributeName = "DateFunctionCodeQualifier", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string DateFunctionCodeQualifier { get; set; }
		[XmlText]
		public string Text { get; set; }
	}
	public class ArticleNumber
	{
		[XmlAttribute(AttributeName = "ArticleNumberType", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string ArticleNumberType { get; set; }
		[XmlText]
		public string Text { get; set; }
	}
	//
	[XmlRoot(ElementName = "DeliveryDetails")]
	public class DeliveryDetails
	{
		[XmlElement(ElementName = "Date")]
		public string Date { get; set; }
	}
	public class Address
	{
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "PartyIdentification")]
		public string PartyIdentification { get; set; }
		[XmlElement(ElementName = "Street")]
		public string Street { get; set; }
		[XmlElement(ElementName = "Town")]
		public string Town { get; set; }
		[XmlElement(ElementName = "ZIP")]
		public string ZIP { get; set; }
		[XmlElement(ElementName = "Country")]
		public string Country { get; set; }
	}
	public class DocumentReference
	{
		[XmlElement(ElementName = "DocumentID")]
		public string DocumentID { get; set; }
		[XmlElement(ElementName = "DocumentType")]
		public string DocumentType { get; set; }
		[XmlElement(ElementName = "ReferenceDate")]
		public string ReferenceDate { get; set; }
	}
	public class ItemList
	{
		[XmlElement(ElementName = "DeliveryListLineItem")]
		public List<DeliveryListLineItem> DeliveryListLineItem { get; set; }
	}

	public class DeliveryListLineItem
	{
		[XmlElement(ElementName = "ConsignmentPackagingSequence")]
		public ConsignmentPackagingSequence ConsignmentPackagingSequence { get; set; }

	}
	public class ConsignmentPackagingSequence
	{
		[XmlElement(ElementName = "HierarchicalId")]
		public string HierarchicalId { get; set; }
		[XmlElement(ElementName = "HierarchicalParentId")]
		public string HierarchicalParentId { get; set; }
		[XmlElement(ElementName = "ConsignmentItemInformation")]
		public ConsignmentItemInformation ConsignmentItemInformation { get; set; }
		[XmlElement(ElementName = "ListLineItem")]
		public ListLineItem ListLineItem { get; set; }
	}
	public class ConsignmentItemInformation
	{
		[XmlElement(ElementName = "NVE")]
		public string NVE { get; set; }
		[XmlElement(ElementName = "ContainedPackagingItems")]
		public List<ContainedPackagingItems> ContainedPackagingItems { get; set; }
	}
	public class ListLineItem
	{
		[XmlElement(ElementName = "PositionNumber")]
		public string PositionNumber { get; set; }
		[XmlElement(ElementName = "ShortDescription")]
		public string ShortDescription { get; set; }

		[XmlElement(ElementName = "Description")]
		public string Description { get; set; }

		[XmlElement(ElementName = "ArticleNumber")]
		public List<ArticleNumber> ArticleNumber { get; set; }
		[XmlElement(ElementName = "Quantity")]
		public List<Quantity> Quantity { get; set; }
	}
	public class DocumentDate
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
		public DateTime Date { get; set; }
	}

	[XmlRoot(ElementName = "PurchaseOrderReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class PurchaseOrderReferenceNumber
	{
		[XmlElement(ElementName = "ReferenceQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceQualifier { get; set; }
		[XmlElement(ElementName = "ReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceNumber { get; set; }
		[XmlElement(ElementName = "Date", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public DateTime Date { get; set; }
	}

	[XmlRoot(ElementName = "ReferencedDocument", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public class ReferencedDocument
	{
		[XmlElement(ElementName = "ReferenceQualifier", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceQualifier { get; set; }
		[XmlElement(ElementName = "ReferenceNumber", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public string ReferenceNumber { get; set; }
		[XmlElement(ElementName = "Date", Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public DateTime Date { get; set; }
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

	[XmlRoot(ElementName = "Supplier")]
	public class Supplier
	{
		[XmlElement(ElementName = "VATIdentificationNumber")]
		public string VATIdentificationNumber { get; set; }

		[XmlElement(ElementName = "FurtherIdentification")]
		public List<FurtherIdentification> FurtherIdentification { get; set; }
		[XmlElement(ElementName = "DocumentReference")]
		public DocumentReference DocumentReference { get; set; }
		[XmlElement(ElementName = "Address")]
		public Address Address { get; set; }
	}
	[XmlRoot(ElementName = "Customer")]
	public class Customer
	{
		[XmlElement(ElementName = "FurtherIdentification")]
		public List<FurtherIdentification> FurtherIdentification { get; set; }
		[XmlElement(ElementName = "DocumentReference")]
		public DocumentReference DocumentReference { get; set; }
		[XmlElement(ElementName = "Address")]
		public Address Address { get; set; }
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

	[XmlRoot(ElementName = "Delivery")]
	public class Delivery
	{
		[XmlElement(ElementName = "DeliveryRecipient")]
		public DeliveryRecipient DeliveryRecipient { get; set; }

		[XmlElement(ElementName = "DeliveryDetails")]
		public DeliveryDetails DeliveryDetails { get; set; }
	}
	public class DocumentExtensionEdifact
	{
		[XmlElement(ElementName = "AdditionalDate")]
		public List<AdditionalDate> AdditionalDate { get; set; }
	}
	public class DocumentExtensionExt
	{
		[XmlElement("DocumentExtension", Namespace = Namespaces.Edifact)]
		public DocumentExtensionEdifact DocumentExtension { get; set; }
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
		public DateTime Date { get; set; }
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

	[XmlRoot(ElementName = "Details")]
	public class Details
	{
		[XmlElement(ElementName = "ItemList")]
		public ItemList ItemList { get; set; }
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

	public class Document
	{
		[XmlAttribute("GeneratingSystem", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string GeneratingSystem { get; set; } = "";
		[XmlAttribute("DocumentType", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string DocumentType { get; set; } = "DispatchAdvice";
		[XmlAttribute("DocumentCurrency", Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
		public string DocumentCurrency { get; set; } = "EUR";


		[XmlElement(ElementName = "DocumentNumber")]
		public string DocumentNumber { get; set; }
		[XmlElement(ElementName = "DocumentDate")]
		public string DocumentDate { get; set; }
		[XmlElement(ElementName = "Delivery")]
		public Delivery Delivery { get; set; }
		[XmlElement(ElementName = "Supplier")]
		public Supplier Supplier { get; set; }
		[XmlElement(ElementName = "Customer")]
		public Customer Customer { get; set; }
		[XmlElement(ElementName = "Details")]
		public Details Details { get; set; }

		[XmlElement(ElementName = "DocumentExtension", Namespace = Namespaces.Ext)]
		public DocumentExtensionExt DocumentExtension { get; set; }
	}

	[XmlRoot(ElementName = "ErpelMessage", Namespace = Namespaces.Erpel)]
	public class ErpelMessage
	{
		[XmlElement(ElementName = "ErpelBusinessDocumentHeader", Namespace = Namespaces.Header)]
		public ErpelBusinessDocumentHeader ErpelBusinessDocumentHeader { get; set; }
		[XmlElement(ElementName = "Document", Namespace = Namespaces.Document)]
		public Document Document { get; set; }
	}
}
