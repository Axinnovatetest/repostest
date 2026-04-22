using iText.StyledXmlParser.Jsoup.Nodes;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	[XmlRoot(ElementName = "Invoice", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2")]
	public class XRechnungModel
	{
		[XmlElement(Namespace = CBC)] public string CustomizationID { get; set; }
		[XmlElement(Namespace = CBC)] public string ProfileID { get; set; }
		[XmlElement(Namespace = CBC)] public string ID { get; set; }
		[XmlElement(DataType = "date", Namespace = CBC)] public DateTime IssueDate { get; set; }
		[XmlElement(DataType = "date", Namespace = CBC)] public DateTime? DueDate { get; set; }
		[XmlElement(Namespace = CBC)] public string InvoiceTypeCode { get; set; }
		[XmlElement(Namespace = CBC)] public string Note { get; set; }
		[XmlElement(Namespace = CBC)] public string DocumentCurrencyCode { get; set; }
		[XmlElement(Namespace = CBC)] public string BuyerReference { get; set; }
		[XmlElement(Namespace = CAC)] public OrderReference OrderReference { get; set; }
		[XmlElement(Namespace = CAC)] public ProjectReference ProjectReference { get; set; }
		[XmlElement(Namespace = CAC)] public PartyWrapper AccountingSupplierParty { get; set; }
		[XmlElement(Namespace = CAC)] public PartyWrapper AccountingCustomerParty { get; set; }
		[XmlElement(Namespace = CAC)] public Delivery Delivery { get; set; }
		//[XmlElement(Namespace = CAC)] public PaymentMeans PaymentMeans { get; set; }
		[XmlElement("PaymentMeans", Namespace = CAC)]
		public List<PaymentMeans> PaymentMeans { get; set; }

		[XmlElement(Namespace = CAC)] public PaymentTerms PaymentTerms { get; set; }
		[XmlElement(Namespace = CAC)] public TaxTotal TaxTotal { get; set; }
		[XmlElement(Namespace = CAC)] public LegalMonetaryTotal LegalMonetaryTotal { get; set; }
		[XmlElement("InvoiceLine", Namespace = CAC)] public List<InvoiceLine> InvoiceLine { get; set; }

		public const string CAC = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
		public const string CBC = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
	}

	public class ProjectReference
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
	}
	public class OrderReference
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string SalesOrderID { get; set; }
	}

	public class PartyWrapper
	{
		[XmlElement(Namespace = XRechnungModel.CAC)] public Party Party { get; set; }
	}

	public class Party
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public EndpointID EndpointID { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public PartyIdentification PartyIdentification { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public PartyName PartyName { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public Address PostalAddress { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public PartyTaxScheme PartyTaxScheme { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public PartyLegalEntity PartyLegalEntity { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public Contact Contact { get; set; }
	}

	public class EndpointID
	{
		[XmlAttribute] public string schemeID { get; set; }
		[XmlText] public string Value { get; set; }
	}

	public class PartyIdentification
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
	}

	public class PartyName
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string Name { get; set; }
	}

	public class Address
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string StreetName { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string AdditionalStreetName { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string CityName { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string PostalZone { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public Country Country { get; set; }
	}

	public class Country
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string IdentificationCode { get; set; }
	}

	public class PartyTaxScheme
	{
		[XmlElement(Namespace = XRechnungModel.CBC)]
		public string CompanyID { get; set; }

		[XmlElement(Namespace = XRechnungModel.CAC)]
		public TaxScheme TaxScheme { get; set; }
	}

	public class TaxScheme
	{
		[XmlElement(Namespace = XRechnungModel.CBC)]
		public string ID { get; set; }
	}


	public class PartyLegalEntity
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string RegistrationName { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string CompanyID { get; set; }
	}

	public class Contact
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string Name { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string Telephone { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ElectronicMail { get; set; }
	}

	public class Delivery
	{
		[XmlElement(DataType = "date", Namespace = XRechnungModel.CBC)] public DateTime ActualDeliveryDate { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public DeliveryLocation DeliveryLocation { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public DeliveryParty DeliveryParty { get; set; }
	}

	public class DeliveryLocation
	{
		[XmlElement(Namespace = XRechnungModel.CAC)] public Address Address { get; set; }
	}

	public class DeliveryParty
	{
		[XmlElement(Namespace = XRechnungModel.CAC)] public PartyName PartyName { get; set; }
	}

	public class PaymentMeans
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public PaymentMeansCode PaymentMeansCode { get; set; }
		//
		[XmlElement(Namespace = XRechnungModel.CBC)] public PaymentID PaymentID { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public PayeeFinancialAccount PayeeFinancialAccount { get; set; }
	}

	public class PaymentMeansCode
	{
		[XmlText] public string Value { get; set; }
	}
	public class PaymentID
	{
		[XmlText] public string Value { get; set; }
	}

	public class PayeeFinancialAccount
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string Name { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public FinancialInstitutionBranch FinancialInstitutionBranch { get; set; }
	}

	public class FinancialInstitutionBranch
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
	}

	public class PaymentTerms
	{
		[XmlElement("Note", Namespace = XRechnungModel.CBC)]
		public string Note { get; set; }
	}

	public class TaxTotal
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount TaxAmount { get; set; }
		[XmlElement("TaxSubtotal", Namespace = XRechnungModel.CAC)]
		public List<TaxSubtotal> TaxSubtotal { get; set; }
	}

	public class TaxSubtotal
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount TaxableAmount { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount TaxAmount { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public TaxCategory TaxCategory { get; set; }
	}

	public class TaxCategory
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public decimal Percent { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public TaxScheme TaxScheme { get; set; }
	}

	public class LegalMonetaryTotal
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount LineExtensionAmount { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount TaxExclusiveAmount { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount TaxInclusiveAmount { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount PayableAmount { get; set; }
	}

	public class Amount
	{
		[XmlAttribute("currencyID")]
		public string CurrencyID { get; set; }

		[XmlText]
		public decimal Value { get; set; }
	}
	public class InvoiceLine
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public Quantity InvoicedQuantity { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount LineExtensionAmount { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public OrderLineReference OrderLineReference { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public DocumentReference DocumentReference { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public AllowanceCharge AllowanceCharge { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public Item Item { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public Price Price { get; set; }
	}

	public class Quantity
	{
		[XmlAttribute] public string unitCode { get; set; }
		[XmlText] public decimal Value { get; set; }
	}

	public class OrderLineReference
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string LineID { get; set; }
	}

	public class DocumentReference
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string DocumentTypeCode { get; set; }
	}

	public class AllowanceCharge
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public bool ChargeIndicator { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string AllowanceChargeReasonCode { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string AllowanceChargeReason { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount Amount { get; set; }
	}

	public class Item
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string Description { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public string Name { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public BuyersItemIdentification BuyersItemIdentification { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public SellersItemIdentification SellersItemIdentification { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public Country OriginCountry { get; set; }
		[XmlElement(Namespace = XRechnungModel.CAC)] public TaxCategory ClassifiedTaxCategory { get; set; }
	}

	public class BuyersItemIdentification
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
	}

	public class SellersItemIdentification
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public string ID { get; set; }
	}

	public class Price
	{
		[XmlElement(Namespace = XRechnungModel.CBC)] public Amount PriceAmount { get; set; }
		[XmlElement(Namespace = XRechnungModel.CBC)] public Quantity BaseQuantity { get; set; }
	}

}
