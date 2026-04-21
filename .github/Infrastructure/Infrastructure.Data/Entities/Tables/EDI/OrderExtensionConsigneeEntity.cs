using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class OrderExtensionConsigneeEntity
	{
		public string City { get; set; }
		public string ContactFax { get; set; }
		public string ContactName { get; set; }
		public string ContatTelephone { get; set; }
		public string CountryName { get; set; }
		public string DUNS { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public int OrderId { get; set; }
		public string PartyIdentification { get; set; }
		public string PartyIdentificationCodeListQualifier { get; set; }
		public string PostalCode { get; set; }
		public string PurchasingDepartment { get; set; }
		public string StorageLocation { get; set; }
		public string Street { get; set; }
		public string UnloadingPoint { get; set; }
		public int OrderType { get; set; }
		public int? OrderElementId { get; set; }

		public OrderExtensionConsigneeEntity() { }

		public OrderExtensionConsigneeEntity(DataRow dataRow)
		{
			City = (dataRow["City"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["City"]);
			ContactFax = (dataRow["ContactFax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ContactFax"]);
			ContactName = (dataRow["ContactName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ContactName"]);
			ContatTelephone = (dataRow["ContatTelephone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ContatTelephone"]);
			CountryName = (dataRow["CountryName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CountryName"]);
			DUNS = (dataRow["DUNS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DUNS"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderType = Convert.ToInt32(dataRow["OrderType"]);
			OrderElementId = (dataRow["OrderElementId"] == System.DBNull.Value) ? null : (int?)Convert.ToInt32(dataRow["OrderElementId"]);
			PartyIdentificationCodeListQualifier = (dataRow["PartyIdentificationCodeListQualifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PartyIdentificationCodeListQualifier"]);
			PostalCode = (dataRow["PostalCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PostalCode"]);
			PurchasingDepartment = (dataRow["PurchasingDepartment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PurchasingDepartment"]);
			StorageLocation = (dataRow["StorageLocation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StorageLocation"]);
			Street = (dataRow["Street"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Street"]);
			UnloadingPoint = (dataRow["UnloadingPoint"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UnloadingPoint"]);
		}
	}
}

