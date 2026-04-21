using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class HeaderEntity
	{
		public string BuyerContactName { get; set; }
		public string BuyerContactTelephone { get; set; }
		public string BuyerDUNS { get; set; }
		public string BuyerPartyIdentification { get; set; }
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
		public string ConsigneeStorageLocation { get; set; }
		public string ConsigneeStreet { get; set; }
		public string ConsigneeUnloadingPoint { get; set; }
		public DateTime? CreationTime { get; set; }
		public DateTime? DocumentImportTime { get; set; }
		public string DocumentNumber { get; set; }
		public bool? Done { get; set; }
		public long Id { get; set; }
		public bool? ManualCreation { get; set; }
		public string MessageReferenceNumber { get; set; }
		public string MessageType { get; set; }
		public int? PreviousReferenceVersionNumber { get; set; }
		public int? PSZCustomernumber { get; set; }
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

		public HeaderEntity() { }

		public HeaderEntity(DataRow dataRow)
		{
			BuyerContactName = Convert.ToString(dataRow["BuyerContactName"]);
			BuyerContactTelephone = Convert.ToString(dataRow["BuyerContactTelephone"]);
			BuyerDUNS = Convert.ToString(dataRow["BuyerDUNS"]);
			BuyerPartyIdentification = (dataRow["BuyerPartyIdentification"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BuyerPartyIdentification"]);
			BuyerPartyName = Convert.ToString(dataRow["BuyerPartyName"]);
			BuyerPurchasingDepartment = Convert.ToString(dataRow["BuyerPurchasingDepartment"]);
			ConsigneeCity = Convert.ToString(dataRow["ConsigneeCity"]);
			ConsigneeContactFax = Convert.ToString(dataRow["ConsigneeContactFax"]);
			ConsigneeContactTelephone = Convert.ToString(dataRow["ConsigneeContactTelephone"]);
			ConsigneeCountryName = Convert.ToString(dataRow["ConsigneeCountryName"]);
			ConsigneeDUNS = Convert.ToString(dataRow["ConsigneeDUNS"]);
			ConsigneePartyIdentification = Convert.ToString(dataRow["ConsigneePartyIdentification"]);
			ConsigneePartyName = Convert.ToString(dataRow["ConsigneePartyName"]);
			ConsigneePostCode = Convert.ToString(dataRow["ConsigneePostCode"]);
			ConsigneeStorageLocation = (dataRow["ConsigneeStorageLocation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ConsigneeStorageLocation"]);
			ConsigneeStreet = Convert.ToString(dataRow["ConsigneeStreet"]);
			ConsigneeUnloadingPoint = (dataRow["ConsigneeUnloadingPoint"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ConsigneeUnloadingPoint"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			DocumentImportTime = (dataRow["DocumentImportTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DocumentImportTime"]);
			DocumentNumber = Convert.ToString(dataRow["DocumentNumber"]);
			Done = (dataRow["Done"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Done"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			ManualCreation = (dataRow["ManualCreation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ManualCreation"]);
			MessageReferenceNumber = Convert.ToString(dataRow["MessageReferenceNumber"]);
			MessageType = Convert.ToString(dataRow["MessageType"]);
			PreviousReferenceVersionNumber = (dataRow["PreviousReferenceVersionNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PreviousReferenceVersionNumber"]);
			PSZCustomernumber = (dataRow["PSZCustomernumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PSZCustomernumber"]);
			ReceivingDate = (dataRow["ReceivingDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ReceivingDate"]);
			RecipientId = Convert.ToString(dataRow["RecipientId"]);
			ReferenceNumber = Convert.ToString(dataRow["ReferenceNumber"]);
			ReferenceVersionNumber = (dataRow["ReferenceVersionNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ReferenceVersionNumber"]);
			SenderId = Convert.ToString(dataRow["SenderId"]);
			SupplierCity = Convert.ToString(dataRow["SupplierCity"]);
			SupplierContactFax = Convert.ToString(dataRow["SupplierContactFax"]);
			SupplierContactTelephone = Convert.ToString(dataRow["SupplierContactTelephone"]);
			SupplierCountryName = Convert.ToString(dataRow["SupplierCountryName"]);
			SupplierDUNS = Convert.ToString(dataRow["SupplierDUNS"]);
			SupplierPartyIdentification = Convert.ToString(dataRow["SupplierPartyIdentification"]);
			SupplierPartyName = Convert.ToString(dataRow["SupplierPartyName"]);
			SupplierPostCode = Convert.ToString(dataRow["SupplierPostCode"]);
			SupplierStreet = Convert.ToString(dataRow["SupplierStreet"]);
			ValidFrom = (dataRow["ValidFrom"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidFrom"]);
			ValidTill = (dataRow["ValidTill"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidTill"]);
		}

		public HeaderEntity ShallowClone()
		{
			return new HeaderEntity
			{
				BuyerContactName = BuyerContactName,
				BuyerContactTelephone = BuyerContactTelephone,
				BuyerDUNS = BuyerDUNS,
				BuyerPartyIdentification = BuyerPartyIdentification,
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
				ConsigneeStorageLocation = ConsigneeStorageLocation,
				ConsigneeStreet = ConsigneeStreet,
				ConsigneeUnloadingPoint = ConsigneeUnloadingPoint,
				CreationTime = CreationTime,
				DocumentImportTime = DocumentImportTime,
				DocumentNumber = DocumentNumber,
				Done = Done,
				Id = Id,
				ManualCreation = ManualCreation,
				MessageReferenceNumber = MessageReferenceNumber,
				MessageType = MessageType,
				PreviousReferenceVersionNumber = PreviousReferenceVersionNumber,
				PSZCustomernumber = PSZCustomernumber,
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

