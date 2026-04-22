using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class OrderChangeEntity
	{
		public int Id { get; set; }

		public DateTime? ActionTime { get; set; }
		public int? ActionUserId { get; set; }
		public string ActionUsername { get; set; }

		public string ConsigneeContactName { get; set; }
		public string ConsigneeName { get; set; }
		public string ConsigneePurchasingDepartment { get; set; }
		public string ConsigneeStreetPostalCode { get; set; }
		public string ConsigneeUnloadingPoint { get; set; }

		public string CustomerContactName { get; set; }
		public string CustomerName { get; set; }
		public string CustomerPurchasingDepartment { get; set; }
		public string CustomerStreetCityPostalCode { get; set; }
		public string CustomerStreetPostalCode { get; set; }

		public string DocumentName { get; set; }
		public string DocumentNumber { get; set; }
		public string Duns { get; set; }
		public int GlobalStatus { get; set; }
		public string Notes { get; set; }
		public int OrderId { get; set; }
		public string Reference { get; set; }
		public string MessageReferenceNumber { get; set; }

		public DateTime CreationTime { get; set; }

		// - update 2021-12-15 - expansion to covert all fields
		public string ConsigneePartyIdentification { get; set; }
		public string ConsigneePartyIdentificationCodeListQualifier { get; set; }
		public string ConsigneeName2 { get; set; }
		public string ConsigneeName3 { get; set; }
		public string ConsigneeStreet { get; set; }
		public string ConsigneeCity { get; set; }
		public string ConsigneePostalCode { get; set; }
		public string ConsigneeCountryName { get; set; }
		public string ConsigneeStorageLocation { get; set; }
		public string ConsigneeContactTelephone { get; set; }
		public string ConsigneeContactFax { get; set; }

		public string CustomerPartyIdentification { get; set; }
		public string CustomerPartyIdentificationCodeListQualifier { get; set; }
		public string CustomerName2 { get; set; }
		public string CustomerName3 { get; set; }
		public string CustomerStreet { get; set; }
		public string CustomerCity { get; set; }
		public string CustomerPostalCode { get; set; }
		public string CustomerCountryName { get; set; }
		public string CustomerContactTelephone { get; set; }
		public string CustomerContactFax { get; set; }



		public OrderChangeEntity() { }
		public OrderChangeEntity(DataRow dataRow)
		{
			ActionTime = (dataRow["ActionTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ActionTime"]);
			ActionUserId = (dataRow["ActionUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ActionUserId"]);
			ActionUsername = (dataRow["ActionUserName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ActionUserName"]);
			ConsigneeCity = (dataRow["ConsigneeCity"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeCity"]);
			ConsigneeContactFax = (dataRow["ConsigneeContactFax"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeContactFax"]);
			ConsigneeContactName = (dataRow["ConsigneeContactName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeContactName"]);
			ConsigneeContactTelephone = (dataRow["ConsigneeContactTelephone"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeContactTelephone"]);
			ConsigneeCountryName = (dataRow["ConsigneeCountryName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeCountryName"]);
			ConsigneeName = (dataRow["ConsigneeName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeName"]);
			ConsigneeName2 = (dataRow["ConsigneeName2"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeName2"]);
			ConsigneeName3 = (dataRow["ConsigneeName3"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeName3"]);
			ConsigneePartyIdentification = (dataRow["ConsigneePartyIdentification"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneePartyIdentification"]);
			ConsigneePartyIdentificationCodeListQualifier = (dataRow["ConsigneePartyIdentificationCodeListQualifier"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneePartyIdentificationCodeListQualifier"]);
			ConsigneePostalCode = (dataRow["ConsigneePostalCode"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneePostalCode"]);
			ConsigneePurchasingDepartment = (dataRow["ConsigneePurchasingDepartment"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneePurchasingDepartment"]);
			ConsigneeStorageLocation = (dataRow["ConsigneeStorageLocation"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeStorageLocation"]);
			ConsigneeStreet = (dataRow["ConsigneeStreet"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeStreet"]);
			ConsigneeStreetPostalCode = (dataRow["ConsigneeStreetPostalCode"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeStreetPostalCode"]);
			ConsigneeUnloadingPoint = (dataRow["ConsigneeUnloadingPoint"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ConsigneeUnloadingPoint"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CustomerCity = (dataRow["CustomerCity"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerCity"]);
			CustomerContactFax = (dataRow["CustomerContactFax"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerContactFax"]);
			CustomerContactName = (dataRow["CustomerContactName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerContactName"]);
			CustomerContactTelephone = (dataRow["CustomerContactTelephone"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerContactTelephone"]);
			CustomerCountryName = (dataRow["CustomerCountryName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerCountryName"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerName"]);
			CustomerName2 = (dataRow["CustomerName2"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerName2"]);
			CustomerName3 = (dataRow["CustomerName3"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerName3"]);
			CustomerPartyIdentification = (dataRow["CustomerPartyIdentification"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerPartyIdentification"]);
			CustomerPartyIdentificationCodeListQualifier = (dataRow["CustomerPartyIdentificationCodeListQualifier"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerPartyIdentificationCodeListQualifier"]);
			CustomerPostalCode = (dataRow["CustomerPostalCode"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerPostalCode"]);
			CustomerPurchasingDepartment = (dataRow["CustomerPurchasingDepartment"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerPurchasingDepartment"]);
			CustomerStreet = (dataRow["CustomerStreet"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerStreet"]);
			CustomerStreetCityPostalCode = (dataRow["CustomerStreetCityPostalCode"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerStreetCityPostalCode"]);
			CustomerStreetPostalCode = (dataRow["CustomerStreetPostalCode"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CustomerStreetPostalCode"]);
			DocumentName = Convert.ToString(dataRow["DocumentName"]);
			DocumentNumber = Convert.ToString(dataRow["DocumentNumber"]);
			Duns = Convert.ToString(dataRow["Duns"]);
			GlobalStatus = Convert.ToInt32(dataRow["GlobalStatus"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			MessageReferenceNumber = (dataRow["MessageReferenceNumber"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["MessageReferenceNumber"]);
			Notes = (dataRow["Notes"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Notes"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			Reference = Convert.ToString(dataRow["Reference"]);
		}
	}
}

