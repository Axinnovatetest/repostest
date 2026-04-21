namespace Psz.Core.Apps.EDI.Models.Delfor
{

	// REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
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

		private uint controlRefField;

		private ulong applicationRefField;

		private byte testIndicatorField;

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
		public uint ControlRef
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
		public ulong ApplicationRef
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
		public byte TestIndicator
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

		private string idField;

		/// <remarks/>
		public string id
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

		private string idField;

		/// <remarks/>
		public string id
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

		private System.DateTime? dateField;

		private System.DateTime? timeField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
		public System.DateTime? date
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
		public System.DateTime? time
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

		private object footerField;

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
		public object Footer
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

		private DocumentHeaderMessageHeader messageHeaderField;

		private DocumentHeaderBeginningOfMessage beginningOfMessageField;

		private DocumentHeaderDates datesField;

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

		private byte documentNameEncodedField;

		private string documentNumberField;

		/// <remarks/>
		public byte DocumentNameEncoded
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
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderDates
	{

		private DocumentHeaderDatesDocumentDate documentDateField;

		private DocumentHeaderDatesHorizonStartDate horizonStartDateField;

		private DocumentHeaderDatesHorizionEndDate horizionEndDateField;

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
		public DocumentHeaderDatesHorizonStartDate HorizonStartDate
		{
			get
			{
				return this.horizonStartDateField;
			}
			set
			{
				this.horizonStartDateField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderDatesHorizionEndDate HorizionEndDate
		{
			get
			{
				return this.horizionEndDateField;
			}
			set
			{
				this.horizionEndDateField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderDatesDocumentDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentHeaderDatesHorizonStartDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentHeaderDatesHorizionEndDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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

		private DocumentHeaderReferencedDocumentsSchedulingAgreement schedulingAgreementField;

		private DocumentHeaderReferencedDocumentsPreviouslyReceivedDocument previouslyReceivedDocumentField;

		/// <remarks/>
		public DocumentHeaderReferencedDocumentsSchedulingAgreement SchedulingAgreement
		{
			get
			{
				return this.schedulingAgreementField;
			}
			set
			{
				this.schedulingAgreementField = value;
			}
		}

		/// <remarks/>
		public DocumentHeaderReferencedDocumentsPreviouslyReceivedDocument PreviouslyReceivedDocument
		{
			get
			{
				return this.previouslyReceivedDocumentField;
			}
			set
			{
				this.previouslyReceivedDocumentField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderReferencedDocumentsSchedulingAgreement
	{

		private string referenceNumberField;

		private int referenceVersionNumberField;

		/// <remarks/>
		public string ReferenceNumber
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
		public int ReferenceVersionNumber
		{
			get
			{
				return this.referenceVersionNumberField;
			}
			set
			{
				this.referenceVersionNumberField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderReferencedDocumentsPreviouslyReceivedDocument
	{

		private int referenceVersionNumberField;

		/// <remarks/>
		public int ReferenceVersionNumber
		{
			get
			{
				return this.referenceVersionNumberField;
			}
			set
			{
				this.referenceVersionNumberField = value;
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

		private string partyIdentification;
		private string partyQualifierField;

		private string partyIdentificationCodeListQualifierField;

		private string dUNSField;

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
		public string PartyIdentification
		{
			get
			{
				return this.partyIdentification;
			}
			set
			{
				this.partyIdentification = value;
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
		public string DUNS
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
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesBuyerContact
	{

		private string nameField;

		private string telephoneField;

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
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentHeaderBusinessEntitiesSupplier
	{

		private string partyQualifierField;

		private string partyIdentificationField;

		private string partyIdentificationCodeListQualifierField;

		private string dUNSField;

		private string partyNameField;

		private string streetField;

		private string cityField;

		private string postCodeField;

		private DocumentHeaderBusinessEntitiesSupplierCountry countryField;

		private DocumentHeaderBusinessEntitiesSupplierContact contactField;

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
		public string PartyIdentification
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
		public string DUNS
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
		public DocumentHeaderBusinessEntitiesSupplierContact Contact
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
	public partial class DocumentHeaderBusinessEntitiesSupplierContact
	{

		private string telephoneField;

		private string faxField;

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
	public partial class DocumentHeaderBusinessEntitiesConsignee
	{

		private string partyQualifierField;

		private string partyIdentificationField;

		private string partyIdentificationCodeListQualifierField;

		private string dUNSField;

		private string partyNameField;

		private string streetField;

		private string cityField;

		private string postCodeField;
		private string unloadingPointField;
		private string storageLocationField;

		private DocumentHeaderBusinessEntitiesConsigneeCountry countryField;

		private DocumentHeaderBusinessEntitiesConsigneeContact contactField;

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
		public string PartyIdentification
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
		public string DUNS
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
		public string StorageLocation
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

		private string telephoneField;

		private string faxField;

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

		private DocumentDetailsForecastData forecastDataField;

		/// <remarks/>
		public DocumentDetailsForecastData ForecastData
		{
			get
			{
				return this.forecastDataField;
			}
			set
			{
				this.forecastDataField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsForecastData
	{

		private DocumentDetailsForecastDataForecastLineItem[] forecastLineItemField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("ForecastLineItem")]
		public DocumentDetailsForecastDataForecastLineItem[] ForecastLineItem
		{
			get
			{
				return this.forecastLineItemField;
			}
			set
			{
				this.forecastLineItemField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsForecastDataForecastLineItem
	{

		private ushort positionNumberField;

		private DocumentHeaderBusinessEntities businessEntitiesField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValues referencedDocumentsDatesValuesField;

		private DocumentDetailsForecastDataForecastLineItemAdditionalInformation additionalInformationField;

		private DocumentDetailsForecastDataForecastLineItemPlanningQuantity[] planningQuantityField;

		private DocumentDetailsForecastDataForecastLineItemReferencedOrderDocument referencedOrderDocumentField;

		/// <remarks/>
		public ushort PositionNumber
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

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValues ReferencedDocumentsDatesValues
		{
			get
			{
				return this.referencedDocumentsDatesValuesField;
			}
			set
			{
				this.referencedDocumentsDatesValuesField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemAdditionalInformation AdditionalInformation
		{
			get
			{
				return this.additionalInformationField;
			}
			set
			{
				this.additionalInformationField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("PlanningQuantity")]
		public DocumentDetailsForecastDataForecastLineItemPlanningQuantity[] PlanningQuantity
		{
			get
			{
				return this.planningQuantityField;
			}
			set
			{
				this.planningQuantityField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedOrderDocument ReferencedOrderDocument
		{
			get
			{
				return this.referencedOrderDocumentField;
			}
			set
			{
				this.referencedOrderDocumentField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValues
	{

		private ushort callOffNumberField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesCallOffDate callOffDateField;

		private ushort previousCallOfNumberField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPreviousCallOffDate previousCallOffDateField;

		private decimal cumulativeReceivedQuantityField;

		private decimal cumulativeScheduledQuantityField;

		private decimal lastReceivedQuantityField;

		private string lastASNNumberField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesLastASNDate lastASNDateField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesLastASNDeliveryDate lastASNDeliveryDateField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPlanningHorizionStartDate planningHorizionStartDateField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPlanningHorizionEndDate planningHorizionEndDateField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesMaterialAuthorization materialAuthorizationField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesProductionAuthorization productionAuthorizationField;

		/// <remarks/>
		public ushort CallOffNumber
		{
			get
			{
				return this.callOffNumberField;
			}
			set
			{
				this.callOffNumberField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesCallOffDate CallOffDate
		{
			get
			{
				return this.callOffDateField;
			}
			set
			{
				this.callOffDateField = value;
			}
		}

		/// <remarks/>
		public ushort PreviousCallOfNumber
		{
			get
			{
				return this.previousCallOfNumberField;
			}
			set
			{
				this.previousCallOfNumberField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPreviousCallOffDate PreviousCallOffDate
		{
			get
			{
				return this.previousCallOffDateField;
			}
			set
			{
				this.previousCallOffDateField = value;
			}
		}

		/// <remarks/>
		public decimal CumulativeReceivedQuantity
		{
			get
			{
				return this.cumulativeReceivedQuantityField;
			}
			set
			{
				this.cumulativeReceivedQuantityField = value;
			}
		}

		/// <remarks/>
		public decimal CumulativeScheduledQuantity
		{
			get
			{
				return this.cumulativeScheduledQuantityField;
			}
			set
			{
				this.cumulativeScheduledQuantityField = value;
			}
		}

		/// <remarks/>
		public decimal LastReceivedQuantity
		{
			get
			{
				return this.lastReceivedQuantityField;
			}
			set
			{
				this.lastReceivedQuantityField = value;
			}
		}

		/// <remarks/>
		public string LastASNNumber
		{
			get
			{
				return this.lastASNNumberField;
			}
			set
			{
				this.lastASNNumberField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesLastASNDate LastASNDate
		{
			get
			{
				return this.lastASNDateField;
			}
			set
			{
				this.lastASNDateField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesLastASNDeliveryDate LastASNDeliveryDate
		{
			get
			{
				return this.lastASNDeliveryDateField;
			}
			set
			{
				this.lastASNDeliveryDateField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPlanningHorizionStartDate PlanningHorizionStartDate
		{
			get
			{
				return this.planningHorizionStartDateField;
			}
			set
			{
				this.planningHorizionStartDateField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPlanningHorizionEndDate PlanningHorizionEndDate
		{
			get
			{
				return this.planningHorizionEndDateField;
			}
			set
			{
				this.planningHorizionEndDateField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesMaterialAuthorization MaterialAuthorization
		{
			get
			{
				return this.materialAuthorizationField;
			}
			set
			{
				this.materialAuthorizationField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesProductionAuthorization ProductionAuthorization
		{
			get
			{
				return this.productionAuthorizationField;
			}
			set
			{
				this.productionAuthorizationField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesCallOffDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPreviousCallOffDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesLastASNDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesLastASNDeliveryDate
	{

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPlanningHorizionStartDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesPlanningHorizionEndDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesMaterialAuthorization
	{

		private ushort authorizationQuantityField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesMaterialAuthorizationAuthorizationEndDate authorizationEndDateField;

		/// <remarks/>
		public ushort AuthorizationQuantity
		{
			get
			{
				return this.authorizationQuantityField;
			}
			set
			{
				this.authorizationQuantityField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesMaterialAuthorizationAuthorizationEndDate AuthorizationEndDate
		{
			get
			{
				return this.authorizationEndDateField;
			}
			set
			{
				this.authorizationEndDateField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesMaterialAuthorizationAuthorizationEndDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesProductionAuthorization
	{

		private ushort authorizationQuantityField;

		private DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesProductionAuthorizationAuthorizationEndDate authorizationEndDateField;

		/// <remarks/>
		public ushort AuthorizationQuantity
		{
			get
			{
				return this.authorizationQuantityField;
			}
			set
			{
				this.authorizationQuantityField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesProductionAuthorizationAuthorizationEndDate AuthorizationEndDate
		{
			get
			{
				return this.authorizationEndDateField;
			}
			set
			{
				this.authorizationEndDateField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedDocumentsDatesValuesProductionAuthorizationAuthorizationEndDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemAdditionalInformation
	{

		private string customersItemMaterialNumberField;

		private string suppliersItemMaterialNumberField;
		private string drawingRevisionNumberField;

		private string buyersInternalProductGroupCodeField;

		private string uPCField;

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
		public string DrawingRevisionNumber
		{
			get
			{
				return this.drawingRevisionNumberField;
			}
			set
			{
				this.drawingRevisionNumberField = value;
			}
		}

		/// <remarks/>
		public string BuyersInternalProductGroupCode
		{
			get
			{
				return this.buyersInternalProductGroupCodeField;
			}
			set
			{
				this.buyersInternalProductGroupCodeField = value;
			}
		}

		/// <remarks/>
		public string UPC
		{
			get
			{
				return this.uPCField;
			}
			set
			{
				this.uPCField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsForecastDataForecastLineItemPlanningQuantity
	{

		private ushort planningQuantityField;

		private string quantityUnitOfMeasureField;

		private DocumentDetailsForecastDataForecastLineItemPlanningQuantityRequestedShipmentDate requestedShipmentDateField;

		private DocumentDetailsForecastDataForecastLineItemPlanningQuantityWeeklyPeriodEndDate weeklyPeriodEndDateField;

		private int deliveryPlanStatusIdentifierField;

		private string frequencyIdentifierField;

		private decimal cumulativeQuantityField;

		/// <remarks/>
		public ushort PlanningQuantity
		{
			get
			{
				return this.planningQuantityField;
			}
			set
			{
				this.planningQuantityField = value;
			}
		}

		/// <remarks/>
		public string QuantityUnitOfMeasure
		{
			get
			{
				return this.quantityUnitOfMeasureField;
			}
			set
			{
				this.quantityUnitOfMeasureField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemPlanningQuantityRequestedShipmentDate RequestedShipmentDate
		{
			get
			{
				return this.requestedShipmentDateField;
			}
			set
			{
				this.requestedShipmentDateField = value;
			}
		}

		/// <remarks/>
		public DocumentDetailsForecastDataForecastLineItemPlanningQuantityWeeklyPeriodEndDate WeeklyPeriodEndDate
		{
			get
			{
				return this.weeklyPeriodEndDateField;
			}
			set
			{
				this.weeklyPeriodEndDateField = value;
			}
		}

		/// <remarks/>
		public int DeliveryPlanStatusIdentifier
		{
			get
			{
				return this.deliveryPlanStatusIdentifierField;
			}
			set
			{
				this.deliveryPlanStatusIdentifierField = value;
			}
		}

		/// <remarks/>
		public string FrequencyIdentifier
		{
			get
			{
				return this.frequencyIdentifierField;
			}
			set
			{
				this.frequencyIdentifierField = value;
			}
		}

		/// <remarks/>
		public decimal CumulativeQuantity
		{
			get
			{
				return this.cumulativeQuantityField;
			}
			set
			{
				this.cumulativeQuantityField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.ecosio.com/erpel-industry-1p0/document")]
	public partial class DocumentDetailsForecastDataForecastLineItemPlanningQuantityRequestedShipmentDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemPlanningQuantityWeeklyPeriodEndDate
	{

		private byte dateQualifierField;

		private System.DateTime? dateTimeField;

		/// <remarks/>
		public byte DateQualifier
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
		public System.DateTime? DateTime
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
	public partial class DocumentDetailsForecastDataForecastLineItemReferencedOrderDocument
	{

		private string referenceQualifierField;

		private ulong referenceNumberField;

		private ushort lineNumberField;

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
		public ulong ReferenceNumber
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
		public ushort LineNumber
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
	}


}
