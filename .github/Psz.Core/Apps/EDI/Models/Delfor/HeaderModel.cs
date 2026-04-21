using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class HeaderModel
	{
		public string BuyerContactName { get; set; }
		public string BuyerContactTelephone { get; set; }
		public string BuyerDUNS { get; set; }
		public string BuyerPartyName { get; set; }
		public string BuyerPurchasingDepartment { get; set; }
		public string ConsigneeCity { get; set; }
		public string ConsigneeContactFax { get; set; }
		public string ConsigneeContactTelephone { get; set; }
		public string ConsigneeCountryName { get; set; }
		public string ConsigneeDUNS { get; set; }
		public string ConsigneePartyIdentification { get; set; }
		public string ConsigneePartyName { get; set; }
		public string ConsigneePostCode { get; set; }
		public string ConsigneeStreet { get; set; }
		public DateTime? CreationTime { get; set; }
		public string DocumentNumber { get; set; }
		public long Id { get; set; }
		public string MessageReferenceNumber { get; set; }
		public string MessageType { get; set; }
		public int? PreviousReferenceVersionNumber { get; set; }
		public DateTime? ReceivingDate { get; set; }
		public string RecipientId { get; set; }
		public string ReferenceNumber { get; set; }
		public int? ReferenceVersionNumber { get; set; }
		public string SenderId { get; set; }
		public string SupplierCity { get; set; }
		public string SupplierContactFax { get; set; }
		public string SupplierContactTelephone { get; set; }
		public string SupplierCountryName { get; set; }
		public string SupplierDUNS { get; set; }
		public string SupplierPartyIdentification { get; set; }
		public string SupplierPartyName { get; set; }
		public string SupplierPostCode { get; set; }
		public string SupplierStreet { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidTill { get; set; }

		public HeaderModel()
		{

		}
		public HeaderModel(Infrastructure.Data.Entities.Tables.CTS.HeaderEntity headerEntity)
		{
			if(headerEntity == null)
				return;

			BuyerContactName = headerEntity.BuyerContactName?.Trim();
			BuyerContactTelephone = headerEntity.BuyerContactTelephone?.Trim();
			BuyerDUNS = headerEntity.BuyerDUNS?.Trim();
			BuyerPartyName = headerEntity.BuyerPartyName?.Trim();
			BuyerPurchasingDepartment = headerEntity.BuyerPurchasingDepartment?.Trim();
			ConsigneeCity = headerEntity.ConsigneeCity?.Trim();
			ConsigneeContactFax = headerEntity.ConsigneeContactFax?.Trim();
			ConsigneeContactTelephone = headerEntity.ConsigneeContactTelephone?.Trim();
			ConsigneeCountryName = headerEntity.ConsigneeCountryName?.Trim();
			ConsigneeDUNS = headerEntity.ConsigneeDUNS?.Trim();
			ConsigneePartyIdentification = headerEntity.ConsigneePartyIdentification?.Trim();
			ConsigneePartyName = headerEntity.ConsigneePartyName?.Trim();
			ConsigneePostCode = headerEntity.ConsigneePostCode?.Trim();
			ConsigneeStreet = headerEntity.ConsigneeStreet?.Trim();
			CreationTime = headerEntity.CreationTime;
			DocumentNumber = headerEntity.DocumentNumber?.Trim();
			Id = headerEntity.Id;
			MessageReferenceNumber = headerEntity.MessageReferenceNumber?.Trim();
			MessageType = headerEntity.MessageType?.Trim();
			PreviousReferenceVersionNumber = headerEntity.PreviousReferenceVersionNumber;
			ReceivingDate = headerEntity.ReceivingDate;
			RecipientId = headerEntity.RecipientId?.Trim();
			ReferenceNumber = headerEntity.ReferenceNumber?.Trim();
			ReferenceVersionNumber = headerEntity.ReferenceVersionNumber;
			SenderId = headerEntity.SenderId?.Trim();
			SupplierCity = headerEntity.SupplierCity?.Trim();
			SupplierContactFax = headerEntity.SupplierContactFax?.Trim();
			SupplierContactTelephone = headerEntity.SupplierContactTelephone?.Trim();
			SupplierCountryName = headerEntity.SupplierCountryName?.Trim();
			SupplierDUNS = headerEntity.SupplierDUNS?.Trim();
			SupplierPartyIdentification = headerEntity.SupplierPartyIdentification?.Trim();
			SupplierPartyName = headerEntity.SupplierPartyName?.Trim();
			SupplierPostCode = headerEntity.SupplierPostCode?.Trim();
			SupplierStreet = headerEntity.SupplierStreet?.Trim();
			ValidFrom = headerEntity.ValidFrom;
			ValidTill = headerEntity.ValidTill;
		}
		public HeaderModel(Models.Delfor.ErpelBusinessDocumentHeaderInterchangeHeader documentHeaderData, Models.Delfor.DocumentHeader headerData)
		{
			if(documentHeaderData == null || headerData == null || headerData.BusinessEntities == null)
				return;

			Id = -1; // - headerData.Id;

			// -
			BuyerContactName = headerData.BusinessEntities.Buyer?.Contact?.Name;
			BuyerContactTelephone = headerData.BusinessEntities.Buyer?.Contact?.Telephone;
			BuyerDUNS = headerData.BusinessEntities.Buyer?.DUNS;
			BuyerPartyName = headerData.BusinessEntities.Buyer?.PartyName;
			BuyerPurchasingDepartment = headerData.BusinessEntities.Buyer?.PurchasingDepartment;
			ConsigneeCity = headerData.BusinessEntities.Consignee?.City;
			ConsigneeContactFax = headerData.BusinessEntities.Consignee?.Contact?.Fax;
			ConsigneeContactTelephone = headerData.BusinessEntities.Consignee?.Contact?.Telephone;
			ConsigneeCountryName = headerData.BusinessEntities.Consignee?.Country?.CountryName;
			ConsigneeDUNS = headerData.BusinessEntities.Consignee?.DUNS;
			ConsigneePartyIdentification = headerData.BusinessEntities.Consignee?.PartyIdentification;
			ConsigneePartyName = headerData.BusinessEntities.Consignee?.PartyName;
			ConsigneePostCode = headerData.BusinessEntities.Consignee?.PostCode;
			ConsigneeStreet = headerData.BusinessEntities.Consignee?.Street;

			DocumentNumber = headerData.BeginningOfMessage?.DocumentNumber;
			MessageReferenceNumber = headerData.MessageHeader?.MessageReferenceNumber;
			MessageType = headerData.MessageHeader?.MessageType;

			ValidFrom = headerData.Dates?.HorizonStartDate?.DateTime; // -!!
			ValidTill = headerData.Dates?.HorizionEndDate.DateTime; // -!!

			// -
			PreviousReferenceVersionNumber = headerData.ReferencedDocuments?.PreviouslyReceivedDocument?.ReferenceVersionNumber;
			ReceivingDate = headerData.Dates?.DocumentDate?.DateTime;
			ReferenceNumber = headerData.ReferencedDocuments?.SchedulingAgreement?.ReferenceNumber;
			ReferenceVersionNumber = headerData.ReferencedDocuments?.SchedulingAgreement?.ReferenceVersionNumber;

			// -
			RecipientId = documentHeaderData?.Recipient?.id;
			SenderId = documentHeaderData?.Sender?.id;
			CreationTime = documentHeaderData.DateTime.date;

			// - 
			SupplierCity = headerData.BusinessEntities.Supplier?.City;
			SupplierContactFax = headerData.BusinessEntities.Supplier?.Contact?.Fax;
			SupplierContactTelephone = headerData.BusinessEntities.Supplier?.Contact?.Telephone;
			SupplierCountryName = headerData.BusinessEntities.Supplier?.Country?.CountryName;
			SupplierDUNS = headerData.BusinessEntities.Supplier?.DUNS;
			SupplierPartyIdentification = headerData.BusinessEntities.Supplier?.PartyIdentification;
			SupplierPartyName = headerData.BusinessEntities.Supplier?.PartyName;
			SupplierPostCode = headerData.BusinessEntities.Supplier?.PostCode;
			SupplierStreet = headerData.BusinessEntities.Supplier?.Street;

		}
		public Infrastructure.Data.Entities.Tables.CTS.HeaderEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity
			{
				BuyerContactName = BuyerContactName,
				BuyerContactTelephone = BuyerContactTelephone,
				BuyerDUNS = BuyerDUNS,
				BuyerPartyName = BuyerPartyName,
				BuyerPurchasingDepartment = BuyerPurchasingDepartment,
				ConsigneeCity = ConsigneeCity,
				ConsigneeContactFax = ConsigneeContactFax,
				ConsigneeContactTelephone = ConsigneeContactTelephone,
				ConsigneeCountryName = ConsigneeCountryName,
				ConsigneeDUNS = ConsigneeDUNS,
				ConsigneePartyIdentification = ConsigneePartyIdentification,
				ConsigneePartyName = ConsigneePartyName,
				ConsigneePostCode = ConsigneePostCode,
				ConsigneeStreet = ConsigneeStreet,
				CreationTime = CreationTime,
				DocumentNumber = DocumentNumber,
				Id = Id,
				MessageReferenceNumber = MessageReferenceNumber,
				MessageType = MessageType,
				PreviousReferenceVersionNumber = PreviousReferenceVersionNumber,
				ReceivingDate = ReceivingDate,
				RecipientId = RecipientId,
				ReferenceNumber = ReferenceNumber,
				ReferenceVersionNumber = ReferenceVersionNumber,
				SenderId = SenderId,
				SupplierCity = SupplierCity,
				SupplierContactFax = SupplierContactFax,
				SupplierContactTelephone = SupplierContactTelephone,
				SupplierCountryName = SupplierCountryName,
				SupplierDUNS = SupplierDUNS,
				SupplierPartyIdentification = SupplierPartyIdentification,
				SupplierPartyName = SupplierPartyName,
				SupplierPostCode = SupplierPostCode,
				SupplierStreet = SupplierStreet,
				ValidFrom = ValidFrom,
				ValidTill = ValidTill
			};
		}
	}
}
