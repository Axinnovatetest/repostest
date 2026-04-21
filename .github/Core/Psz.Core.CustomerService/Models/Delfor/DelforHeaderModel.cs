using System;

namespace Psz.Core.CustomerService.Models.Delfor
{
	public class DelforHeaderModel
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
		public DateTime? HorizonStartDate { get; set; }
		public DateTime? HorizonEndDate { get; set; }
		public string ConsigneeStorageLocation { get; set; }
		public string ConsigneeUnloadingPoint { get; set; }

		public DelforHeaderModel()
		{

		}
		public DelforHeaderModel(Infrastructure.Data.Entities.Tables.CTS.HeaderEntity headerEntity)
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
			HorizonStartDate = headerEntity.ValidFrom;
			HorizonEndDate = headerEntity.ValidTill;
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
				ValidFrom = HorizonStartDate,
				ValidTill = HorizonEndDate,
			};
		}
	}
}
