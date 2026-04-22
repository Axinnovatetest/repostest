using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.Order.ImportedFileModel
{
	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/message")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/message", IsNullable = false)]
	public partial class ErpelIndustryMessage
	{

		private ErpelBusinessDocumentHeader erpelBusinessDocumentHeaderField;

		private Document documentField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
		public ErpelBusinessDocumentHeader ErpelBusinessDocumentHeader
		{
			get
			{
				return this.erpelBusinessDocumentHeaderField;
			}
			set
			{
				this.erpelBusinessDocumentHeaderField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
		public Document Document
		{
			get
			{
				return this.documentField;
			}
			set
			{
				this.documentField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header", IsNullable = false)]
	public partial class ErpelBusinessDocumentHeader
	{

		private ErpelBusinessDocumentHeaderInterchangeHeader interchangeHeaderField;

		/// <remarks/>
		public ErpelBusinessDocumentHeaderInterchangeHeader InterchangeHeader
		{
			get
			{
				return this.interchangeHeaderField;
			}
			set
			{
				this.interchangeHeaderField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public partial class ErpelBusinessDocumentHeaderInterchangeHeader
	{

		private ErpelBusinessDocumentHeaderInterchangeHeaderSender senderField;

		private ErpelBusinessDocumentHeaderInterchangeHeaderRecipient recipientField;

		private ErpelBusinessDocumentHeaderInterchangeHeaderDateTime dateTimeField;

		private int controlRefField;

		private long applicationRefField;

		private int testIndicatorField;

		/// <remarks/>
		public ErpelBusinessDocumentHeaderInterchangeHeaderSender Sender
		{
			get
			{
				return this.senderField;
			}
			set
			{
				this.senderField = value;
			}
		}

		/// <remarks/>
		public ErpelBusinessDocumentHeaderInterchangeHeaderRecipient Recipient
		{
			get
			{
				return this.recipientField;
			}
			set
			{
				this.recipientField = value;
			}
		}

		/// <remarks/>
		public ErpelBusinessDocumentHeaderInterchangeHeaderDateTime DateTime
		{
			get
			{
				return this.dateTimeField;
			}
			set
			{
				this.dateTimeField = value;
			}
		}

		/// <remarks/>
		public int ControlRef
		{
			get
			{
				return this.controlRefField;
			}
			set
			{
				this.controlRefField = value;
			}
		}

		/// <remarks/>
		public long ApplicationRef
		{
			get
			{
				return this.applicationRefField;
			}
			set
			{
				this.applicationRefField = value;
			}
		}

		/// <remarks/>
		public int TestIndicator
		{
			get
			{
				return this.testIndicatorField;
			}
			set
			{
				this.testIndicatorField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public partial class ErpelBusinessDocumentHeaderInterchangeHeaderSender
	{

		private int idField;

		/// <remarks/>
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public partial class ErpelBusinessDocumentHeaderInterchangeHeaderRecipient
	{

		private int idField;

		/// <remarks/>
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/header")]
	public partial class ErpelBusinessDocumentHeaderInterchangeHeaderDateTime
	{

		private System.DateTime dateField;

		private System.DateTime timeField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
		public System.DateTime date
		{
			get
			{
				return this.dateField;
			}
			set
			{
				this.dateField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
		public System.DateTime time
		{
			get
			{
				return this.timeField;
			}
			set
			{
				this.timeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document", IsNullable = false)]
	public partial class Document
	{

		private DocumentHeader headerField;

		private DocumentDetails detailsField;

		private DocumentFooter footerField;

		/// <remarks/>
		public DocumentHeader Header
		{
			get
			{
				return this.headerField;
			}
			set
			{
				this.headerField = value;
			}
		}

		/// <remarks/>
		public DocumentDetails Details
		{
			get
			{
				return this.detailsField;
			}
			set
			{
				this.detailsField = value;
			}
		}

		/// <remarks/>
		public DocumentFooter Footer
		{
			get
			{
				return this.footerField;
			}
			set
			{
				this.footerField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeader
	{
		public string GetFreeText()
		{
			if(freeTextField == null || freeTextField.Length == 0)
			{
				return null;
			}

			var texts = new List<string>();

			foreach(var freeText in freeTextField)
			{
				if(freeText.Text == null
					|| freeText.Text.Length == 0)
				{
					continue;
				}

				foreach(var text in freeText.Text)
				{
					if(!string.IsNullOrWhiteSpace(text))
					{
						texts.Add(text);
					}
				}
			}

			return texts.Count > 0 ? string.Join(", ", texts) : null;
		}

		private DocumentHeaderFreeText[] freeTextField;
		/// <remarks/>
		public DocumentHeaderFreeText[] FreeText
		{
			get
			{
				return this.freeTextField;
			}
			set
			{
				this.freeTextField = value;
			}
		}


		private DocumentHeaderMessageHeader messageHeaderField;

		private DocumentHeaderBeginningOfMessage beginningOfMessageField;

		private DocumentHeaderDates datesField;

		private string referenceCurrencyField;

		private DocumentHeaderReferencedDocuments referencedDocumentsField;

		private DocumentHeaderBusinessEntities businessEntitiesField;

		/// <remarks/>
		public DocumentHeaderMessageHeader MessageHeader
		{
			get
			{
				return this.messageHeaderField;
			}
			set
			{
				this.messageHeaderField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderBeginningOfMessage BeginningOfMessage
		{
			get
			{
				return this.beginningOfMessageField;
			}
			set
			{
				this.beginningOfMessageField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderDates Dates
		{
			get
			{
				return this.datesField;
			}
			set
			{
				this.datesField = value;
			}
		}

		/// <remarks/>
		public string ReferenceCurrency
		{
			get
			{
				return this.referenceCurrencyField;
			}
			set
			{
				this.referenceCurrencyField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderReferencedDocuments ReferencedDocuments
		{
			get
			{
				return this.referencedDocumentsField;
			}
			set
			{
				this.referencedDocumentsField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderBusinessEntities BusinessEntities
		{
			get
			{
				return this.businessEntitiesField;
			}
			set
			{
				this.businessEntitiesField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderFreeText
	{

		private string textQualifierField;

		private string textLanguageField;

		private string[] textField;

		/// <remarks/>
		public string TextQualifier
		{
			get
			{
				return this.textQualifierField;
			}
			set
			{
				this.textQualifierField = value;
			}
		}

		/// <remarks/>
		public string TextLanguage
		{
			get
			{
				return this.textLanguageField;
			}
			set
			{
				this.textLanguageField = value;
			}
		}

		/// <remarks/>
		public string[] Text
		{
			get
			{
				return this.textField;
			}
			set
			{
				this.textField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderMessageHeader
	{

		private string messageReferenceNumberField;

		private string messageTypeField;

		/// <remarks/>
		public string MessageReferenceNumber
		{
			get
			{
				return this.messageReferenceNumberField;
			}
			set
			{
				this.messageReferenceNumberField = value;
			}
		}

		/// <remarks/>
		public string MessageType
		{
			get
			{
				return this.messageTypeField;
			}
			set
			{
				this.messageTypeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBeginningOfMessage
	{

		private int documentNameEncodedField;

		private string documentNumberField;

		private int messageFunctionField;

		/// <remarks/>
		public int DocumentNameEncoded
		{
			get
			{
				return this.documentNameEncodedField;
			}
			set
			{
				this.documentNameEncodedField = value;
			}
		}

		/// <remarks/>
		public string DocumentNumber
		{
			get
			{
				return this.documentNumberField;
			}
			set
			{
				this.documentNumberField = value;
			}
		}

		/// <remarks/>
		public int MessageFunction
		{
			get
			{
				return this.messageFunctionField;
			}
			set
			{
				this.messageFunctionField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderDates
	{

		private DocumentHeaderDatesDocumentDate documentDateField;

		private DocumentHeaderDatesDate dateField;

		/// <remarks/>
		public DocumentHeaderDatesDocumentDate DocumentDate
		{
			get
			{
				return this.documentDateField;
			}
			set
			{
				this.documentDateField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderDatesDate Date
		{
			get
			{
				return this.dateField;
			}
			set
			{
				this.dateField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderDatesDocumentDate
	{

		private int dateQualifierField;

		private System.DateTime dateTimeField;

		/// <remarks/>
		public int DateQualifier
		{
			get
			{
				return this.dateQualifierField;
			}
			set
			{
				this.dateQualifierField = value;
			}
		}

		/// <remarks/>
		public System.DateTime DateTime
		{
			get
			{
				return this.dateTimeField;
			}
			set
			{
				this.dateTimeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderDatesDate
	{

		private int dateQualifierField;

		private System.DateTime dateTimeField;

		/// <remarks/>
		public int DateQualifier
		{
			get
			{
				return this.dateQualifierField;
			}
			set
			{
				this.dateQualifierField = value;
			}
		}

		/// <remarks/>
		public System.DateTime DateTime
		{
			get
			{
				return this.dateTimeField;
			}
			set
			{
				this.dateTimeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderReferencedDocuments
	{

		private DocumentHeaderReferencedDocumentsPurchaseOrderReferenceNumber purchaseOrderReferenceNumberField;

		/// <remarks/>
		public DocumentHeaderReferencedDocumentsPurchaseOrderReferenceNumber PurchaseOrderReferenceNumber
		{
			get
			{
				return this.purchaseOrderReferenceNumberField;
			}
			set
			{
				this.purchaseOrderReferenceNumberField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderReferencedDocumentsPurchaseOrderReferenceNumber
	{

		private string referenceQualifierField;

		private long referenceNumberField;

		private DocumentHeaderReferencedDocumentsPurchaseOrderReferenceNumberDate dateField;

		/// <remarks/>
		public string ReferenceQualifier
		{
			get
			{
				return this.referenceQualifierField;
			}
			set
			{
				this.referenceQualifierField = value;
			}
		}

		/// <remarks/>
		public long ReferenceNumber
		{
			get
			{
				return this.referenceNumberField;
			}
			set
			{
				this.referenceNumberField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderReferencedDocumentsPurchaseOrderReferenceNumberDate Date
		{
			get
			{
				return this.dateField;
			}
			set
			{
				this.dateField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderReferencedDocumentsPurchaseOrderReferenceNumberDate
	{

		private int dateQualifierField;

		private System.DateTime dateTimeField;

		/// <remarks/>
		public int DateQualifier
		{
			get
			{
				return this.dateQualifierField;
			}
			set
			{
				this.dateQualifierField = value;
			}
		}

		/// <remarks/>
		public System.DateTime DateTime
		{
			get
			{
				return this.dateTimeField;
			}
			set
			{
				this.dateTimeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntities
	{

		private DocumentHeaderBusinessEntitiesBuyer buyerField;

		private DocumentHeaderBusinessEntitiesSupplier supplierField;

		private DocumentHeaderBusinessEntitiesConsignee consigneeField;

		/// <remarks/>
		public DocumentHeaderBusinessEntitiesBuyer Buyer
		{
			get
			{
				return this.buyerField;
			}
			set
			{
				this.buyerField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderBusinessEntitiesSupplier Supplier
		{
			get
			{
				return this.supplierField;
			}
			set
			{
				this.supplierField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderBusinessEntitiesConsignee Consignee
		{
			get
			{
				return this.consigneeField;
			}
			set
			{
				this.consigneeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesBuyer
	{

		private string partyQualifierField;

		private int partyIdentificationField;

		private string partyIdentificationCodeListQualifierField;

		private int dUNSField;

		private string partyNameField;

		private DocumentHeaderBusinessEntitiesBuyerContact contactField;

		private string purchasingDepartmentField;

		/// <remarks/>
		public string PartyQualifier
		{
			get
			{
				return this.partyQualifierField;
			}
			set
			{
				this.partyQualifierField = value;
			}
		}

		/// <remarks/>
		public int PartyIdentification
		{
			get
			{
				return this.partyIdentificationField;
			}
			set
			{
				this.partyIdentificationField = value;
			}
		}

		/// <remarks/>
		public string PartyIdentificationCodeListQualifier
		{
			get
			{
				return this.partyIdentificationCodeListQualifierField;
			}
			set
			{
				this.partyIdentificationCodeListQualifierField = value;
			}
		}

		/// <remarks/>
		public int DUNS
		{
			get
			{
				return this.dUNSField;
			}
			set
			{
				this.dUNSField = value;
			}
		}

		/// <remarks/>
		public string PartyName
		{
			get
			{
				return this.partyNameField;
			}
			set
			{
				this.partyNameField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderBusinessEntitiesBuyerContact Contact
		{
			get
			{
				return this.contactField;
			}
			set
			{
				this.contactField = value;
			}
		}

		/// <remarks/>
		public string PurchasingDepartment
		{
			get
			{
				return this.purchasingDepartmentField;
			}
			set
			{
				this.purchasingDepartmentField = value;
			}
		}


		private string streetField;
		private string cityField;
		private string postCodeField;
		public string Street
		{
			get
			{
				return this.streetField;
			}
			set
			{
				this.streetField = value;
			}
		}
		public string City
		{
			get
			{
				return this.cityField;
			}
			set
			{
				this.cityField = value;
			}
		}
		public string PostCode
		{
			get
			{
				return this.postCodeField;
			}
			set
			{
				this.postCodeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesBuyerContact
	{

		private string nameField;

		private string telephoneField;

		private string faxField;

		/// <remarks/>
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <remarks/>
		public string Telephone
		{
			get
			{
				return this.telephoneField;
			}
			set
			{
				this.telephoneField = value;
			}
		}

		/// <remarks/>
		public string Fax
		{
			get
			{
				return this.faxField;
			}
			set
			{
				this.faxField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesSupplier
	{

		private string partyQualifierField;

		private int partyIdentificationField;

		private string partyIdentificationCodeListQualifierField;

		private int dUNSField;

		private DocumentHeaderBusinessEntitiesSupplierCountry countryField;

		private object contactField;

		/// <remarks/>
		public string PartyQualifier
		{
			get
			{
				return this.partyQualifierField;
			}
			set
			{
				this.partyQualifierField = value;
			}
		}

		/// <remarks/>
		public int PartyIdentification
		{
			get
			{
				return this.partyIdentificationField;
			}
			set
			{
				this.partyIdentificationField = value;
			}
		}

		/// <remarks/>
		public string PartyIdentificationCodeListQualifier
		{
			get
			{
				return this.partyIdentificationCodeListQualifierField;
			}
			set
			{
				this.partyIdentificationCodeListQualifierField = value;
			}
		}

		/// <remarks/>
		public int DUNS
		{
			get
			{
				return this.dUNSField;
			}
			set
			{
				this.dUNSField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderBusinessEntitiesSupplierCountry Country
		{
			get
			{
				return this.countryField;
			}
			set
			{
				this.countryField = value;
			}
		}

		/// <remarks/>
		public object Contact
		{
			get
			{
				return this.contactField;
			}
			set
			{
				this.contactField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesSupplierCountry
	{

		private object countryNameField;

		/// <remarks/>
		public object CountryName
		{
			get
			{
				return this.countryNameField;
			}
			set
			{
				this.countryNameField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesConsignee
	{

		private string partyQualifierField;

		private int partyIdentificationField;

		private string partyIdentificationCodeListQualifierField;

		private int dUNSField;

		private string partyNameField;

		private string streetField;

		private string cityField;

		private int postCodeField;

		private DocumentHeaderBusinessEntitiesConsigneeCountry countryField;

		private DocumentHeaderBusinessEntitiesConsigneeContact contactField;

		private string unloadingPointField;

		private int storageLocationField;

		private string purchasingDepartmentField;

		/// <remarks/>
		public string PartyQualifier
		{
			get
			{
				return this.partyQualifierField;
			}
			set
			{
				this.partyQualifierField = value;
			}
		}

		/// <remarks/>
		public int PartyIdentification
		{
			get
			{
				return this.partyIdentificationField;
			}
			set
			{
				this.partyIdentificationField = value;
			}
		}

		/// <remarks/>
		public string PartyIdentificationCodeListQualifier
		{
			get
			{
				return this.partyIdentificationCodeListQualifierField;
			}
			set
			{
				this.partyIdentificationCodeListQualifierField = value;
			}
		}

		/// <remarks/>
		public int DUNS
		{
			get
			{
				return this.dUNSField;
			}
			set
			{
				this.dUNSField = value;
			}
		}

		/// <remarks/>
		public string PartyName
		{
			get
			{
				return this.partyNameField;
			}
			set
			{
				this.partyNameField = value;
			}
		}

		/// <remarks/>
		public string Street
		{
			get
			{
				return this.streetField;
			}
			set
			{
				this.streetField = value;
			}
		}

		/// <remarks/>
		public string City
		{
			get
			{
				return this.cityField;
			}
			set
			{
				this.cityField = value;
			}
		}

		/// <remarks/>
		public int PostCode
		{
			get
			{
				return this.postCodeField;
			}
			set
			{
				this.postCodeField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderBusinessEntitiesConsigneeCountry Country
		{
			get
			{
				return this.countryField;
			}
			set
			{
				this.countryField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderBusinessEntitiesConsigneeContact Contact
		{
			get
			{
				return this.contactField;
			}
			set
			{
				this.contactField = value;
			}
		}

		/// <remarks/>
		public string UnloadingPoint
		{
			get
			{
				return this.unloadingPointField;
			}
			set
			{
				this.unloadingPointField = value;
			}
		}

		/// <remarks/>
		public int StorageLocation
		{
			get
			{
				return this.storageLocationField;
			}
			set
			{
				this.storageLocationField = value;
			}
		}

		/// <remarks/>
		public string PurchasingDepartment
		{
			get
			{
				return this.purchasingDepartmentField;
			}
			set
			{
				this.purchasingDepartmentField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesConsigneeCountry
	{

		private string countryNameField;

		/// <remarks/>
		public string CountryName
		{
			get
			{
				return this.countryNameField;
			}
			set
			{
				this.countryNameField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesConsigneeContact
	{

		private string nameField;

		private string telephoneField;

		private string faxField;

		/// <remarks/>
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <remarks/>
		public string Telephone
		{
			get
			{
				return this.telephoneField;
			}
			set
			{
				this.telephoneField = value;
			}
		}

		/// <remarks/>
		public string Fax
		{
			get
			{
				return this.faxField;
			}
			set
			{
				this.faxField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetails
	{

		private DocumentDetailsOrdersLineItem[] ordersDataField;

		private DocumentDetailsDispatchData dispatchDataField;

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("OrdersLineItem", IsNullable = false)]
		public DocumentDetailsOrdersLineItem[] OrdersData
		{
			get
			{
				return this.ordersDataField;
			}
			set
			{
				this.ordersDataField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsDispatchData DispatchData
		{
			get
			{
				return this.dispatchDataField;
			}
			set
			{
				this.dispatchDataField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsOrdersLineItem
	{
		public string GetFreeText()
		{
			if(freeTextField == null || freeTextField.Length == 0)
			{
				return null;
			}

			var texts = new List<string>();

			foreach(var freeText in freeTextField)
			{
				if(freeText.Text == null
					|| freeText.Text.Length == 0)
				{
					continue;
				}

				foreach(var text in freeText.Text)
				{
					if(!string.IsNullOrWhiteSpace(text))
					{
						texts.Add(text);
					}
				}
			}

			return texts.Count > 0 ? string.Join(", ", texts) : null;
		}

		private int positionNumberField;

		private string customersItemMaterialNumberField;

		private string suppliersItemMaterialNumberField;

		private string itemDescriptionField;

		private decimal orderedQuantityField;

		private string measureUnitQualifierField;

		private decimal currentItemPriceCalculationNetField;

		private int unitPriceBasisField;

		private decimal lineItemAmountField;

		private int lineItemActionRequestField;

		private DocumentDetailsOrdersLineItemFreeText[] freeTextField;

		private DocumentDetailsOrdersLineItemReferences referencesField;

		private DocumentDetailsOrdersLineItemOrdersScheduleLine ordersScheduleLineField;

		/// <remarks/>
		public int PositionNumber
		{
			get
			{
				return this.positionNumberField;
			}
			set
			{
				this.positionNumberField = value;
			}
		}

		/// <remarks/>
		public string CustomersItemMaterialNumber
		{
			get
			{
				return this.customersItemMaterialNumberField;
			}
			set
			{
				this.customersItemMaterialNumberField = value;
			}
		}

		/// <remarks/>
		public string SuppliersItemMaterialNumber
		{
			get
			{
				return this.suppliersItemMaterialNumberField;
			}
			set
			{
				this.suppliersItemMaterialNumberField = value;
			}
		}

		/// <remarks/>
		public string ItemDescription
		{
			get
			{
				return this.itemDescriptionField;
			}
			set
			{
				this.itemDescriptionField = value;
			}
		}

		/// <remarks/>
		public decimal OrderedQuantity
		{
			get
			{
				return this.orderedQuantityField;
			}
			set
			{
				this.orderedQuantityField = value;
			}
		}

		/// <remarks/>
		public string MeasureUnitQualifier
		{
			get
			{
				return this.measureUnitQualifierField;
			}
			set
			{
				this.measureUnitQualifierField = value;
			}
		}

		/// <remarks/>
		public decimal CurrentItemPriceCalculationNet
		{
			get
			{
				return this.currentItemPriceCalculationNetField;
			}
			set
			{
				this.currentItemPriceCalculationNetField = value;
			}
		}

		/// <remarks/>
		public int UnitPriceBasis
		{
			get
			{
				return this.unitPriceBasisField;
			}
			set
			{
				this.unitPriceBasisField = value;
			}
		}

		/// <remarks/>
		public decimal LineItemAmount
		{
			get
			{
				return this.lineItemAmountField;
			}
			set
			{
				this.lineItemAmountField = value;
			}
		}

		/// <remarks/>
		public int LineItemActionRequest
		{
			get
			{
				return this.lineItemActionRequestField;
			}
			set
			{
				this.lineItemActionRequestField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsOrdersLineItemFreeText[] FreeText
		{
			get
			{
				return this.freeTextField;
			}
			set
			{
				this.freeTextField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsOrdersLineItemReferences References
		{
			get
			{
				return this.referencesField;
			}
			set
			{
				this.referencesField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsOrdersLineItemOrdersScheduleLine OrdersScheduleLine
		{
			get
			{
				return this.ordersScheduleLineField;
			}
			set
			{
				this.ordersScheduleLineField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsOrdersLineItemFreeText
	{


		private string textQualifierField;

		private string textLanguageField;

		private string[] textField;

		/// <remarks/>
		public string TextQualifier
		{
			get
			{
				return this.textQualifierField;
			}
			set
			{
				this.textQualifierField = value;
			}
		}

		/// <remarks/>
		public string TextLanguage
		{
			get
			{
				return this.textLanguageField;
			}
			set
			{
				this.textLanguageField = value;
			}
		}

		/// <remarks/>
		public string[] Text
		{
			get
			{
				return this.textField;
			}
			set
			{
				this.textField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsOrdersLineItemReferences
	{

		private DocumentDetailsOrdersLineItemReferencesOrderReference orderReferenceField;

		/// <remarks/>
		public DocumentDetailsOrdersLineItemReferencesOrderReference OrderReference
		{
			get
			{
				return this.orderReferenceField;
			}
			set
			{
				this.orderReferenceField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsOrdersLineItemReferencesOrderReference
	{

		private string referenceQualifierField;

		private long referenceNumberField;

		private int lineNumberField;

		private DocumentDetailsOrdersLineItemReferencesOrderReferenceDate dateField;

		/// <remarks/>
		public string ReferenceQualifier
		{
			get
			{
				return this.referenceQualifierField;
			}
			set
			{
				this.referenceQualifierField = value;
			}
		}

		/// <remarks/>
		public long ReferenceNumber
		{
			get
			{
				return this.referenceNumberField;
			}
			set
			{
				this.referenceNumberField = value;
			}
		}

		/// <remarks/>
		public int LineNumber
		{
			get
			{
				return this.lineNumberField;
			}
			set
			{
				this.lineNumberField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsOrdersLineItemReferencesOrderReferenceDate Date
		{
			get
			{
				return this.dateField;
			}
			set
			{
				this.dateField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsOrdersLineItemReferencesOrderReferenceDate
	{

		private int dateQualifierField;

		private System.DateTime dateTimeField;

		/// <remarks/>
		public int DateQualifier
		{
			get
			{
				return this.dateQualifierField;
			}
			set
			{
				this.dateQualifierField = value;
			}
		}

		/// <remarks/>
		public System.DateTime DateTime
		{
			get
			{
				return this.dateTimeField;
			}
			set
			{
				this.dateTimeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsOrdersLineItemOrdersScheduleLine
	{

		private int scheduledQuantityField;

		private DocumentDetailsOrdersLineItemOrdersScheduleLineScheduledQuantityDate scheduledQuantityDateField;

		private int previousScheduledQuantityField;

		/// <remarks/>
		public int ScheduledQuantity
		{
			get
			{
				return this.scheduledQuantityField;
			}
			set
			{
				this.scheduledQuantityField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsOrdersLineItemOrdersScheduleLineScheduledQuantityDate ScheduledQuantityDate
		{
			get
			{
				return this.scheduledQuantityDateField;
			}
			set
			{
				this.scheduledQuantityDateField = value;
			}
		}

		/// <remarks/>
		public int PreviousScheduledQuantity
		{
			get
			{
				return this.previousScheduledQuantityField;
			}
			set
			{
				this.previousScheduledQuantityField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsOrdersLineItemOrdersScheduleLineScheduledQuantityDate
	{

		private int dateQualifierField;

		private System.DateTime dateTimeField;

		/// <remarks/>
		public int DateQualifier
		{
			get
			{
				return this.dateQualifierField;
			}
			set
			{
				this.dateQualifierField = value;
			}
		}

		/// <remarks/>
		public System.DateTime DateTime
		{
			get
			{
				return this.dateTimeField;
			}
			set
			{
				this.dateTimeField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsDispatchData
	{

		private DocumentDetailsDispatchDataConsignmentPackagingSequence consignmentPackagingSequenceField;

		/// <remarks/>
		public DocumentDetailsDispatchDataConsignmentPackagingSequence ConsignmentPackagingSequence
		{
			get
			{
				return this.consignmentPackagingSequenceField;
			}
			set
			{
				this.consignmentPackagingSequenceField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsDispatchDataConsignmentPackagingSequence
	{

		private DocumentDetailsDispatchDataConsignmentPackagingSequenceDeliveryLineItems deliveryLineItemsField;

		/// <remarks/>
		public DocumentDetailsDispatchDataConsignmentPackagingSequenceDeliveryLineItems DeliveryLineItems
		{
			get
			{
				return this.deliveryLineItemsField;
			}
			set
			{
				this.deliveryLineItemsField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsDispatchDataConsignmentPackagingSequenceDeliveryLineItems
	{

		private int positionNumberField;

		private string shippingMarkField;

		private decimal netWeightField;

		private decimal weightField;

		/// <remarks/>
		public int PositionNumber
		{
			get
			{
				return this.positionNumberField;
			}
			set
			{
				this.positionNumberField = value;
			}
		}

		/// <remarks/>
		public string ShippingMark
		{
			get
			{
				return this.shippingMarkField;
			}
			set
			{
				this.shippingMarkField = value;
			}
		}

		/// <remarks/>
		public decimal NetWeight
		{
			get
			{
				return this.netWeightField;
			}
			set
			{
				this.netWeightField = value;
			}
		}

		/// <remarks/>
		public decimal Weight
		{
			get
			{
				return this.weightField;
			}
			set
			{
				this.weightField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentFooter
	{

		private DocumentFooterInvoiceFooter invoiceFooterField;

		/// <remarks/>
		public DocumentFooterInvoiceFooter InvoiceFooter
		{
			get
			{
				return this.invoiceFooterField;
			}
			set
			{
				this.invoiceFooterField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentFooterInvoiceFooter
	{

		private DocumentFooterInvoiceFooterInvoiceTotals invoiceTotalsField;

		/// <remarks/>
		public DocumentFooterInvoiceFooterInvoiceTotals InvoiceTotals
		{
			get
			{
				return this.invoiceTotalsField;
			}
			set
			{
				this.invoiceTotalsField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentFooterInvoiceFooterInvoiceTotals
	{

		private decimal totalLineItemAmountField;

		private string currencyQualifierField;

		/// <remarks/>
		public decimal TotalLineItemAmount
		{
			get
			{
				return this.totalLineItemAmountField;
			}
			set
			{
				this.totalLineItemAmountField = value;
			}
		}

		/// <remarks/>
		public string CurrencyQualifier
		{
			get
			{
				return this.currencyQualifierField;
			}
			set
			{
				this.currencyQualifierField = value;
			}
		}
	}



}
