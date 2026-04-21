using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.STG
{
	public class CompanyEntity
	{
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public DateTime CreateTime { get; set; }
		public int CreateUserId { get; set; }
		public string Description { get; set; }
		public string DirectorEmail { get; set; }
		public int? DirectorId { get; set; }
		public string DirectorName { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public int Id { get; set; }
		public bool? IsActive { get; set; }
		public string LagalName { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public byte[] Logo { get; set; }
		public string LogoExtension { get; set; }
		public string Name { get; set; }
		public string PostalCode { get; set; }
		public string Telephone { get; set; }
		public string Telephone2 { get; set; }
		public string Type { get; set; }
		public int? CountryId { get; set; }
		public bool? Closed { get; set; }
		public string VatID { get; set; }

		public CompanyEntity() { }

		public CompanyEntity(DataRow dataRow)
		{
			Address = (dataRow["Address"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Address"]);
			Address2 = (dataRow["Address2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Address2"]);
			City = (dataRow["City"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["City"]);
			Country = (dataRow["Country"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Country"]);
			CountryId = (dataRow["CountryId"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["CountryId"]);
			CreateTime = Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = Convert.ToInt32(dataRow["CreateUserId"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			DirectorEmail = (dataRow["DirectorEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DirectorEmail"]);
			DirectorId = (dataRow["DirectorId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DirectorId"]);
			DirectorName = (dataRow["DirectorName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DirectorName"]);
			Email = (dataRow["Email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Email"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsActive = (dataRow["IsActive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsActive"]);
			LagalName = (dataRow["LagalName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LagalName"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			Logo = (dataRow["Logo"] == System.DBNull.Value) ? new byte[0] : (byte[])dataRow["Logo"];
			LogoExtension = (dataRow["LogoExtension"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogoExtension"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			PostalCode = (dataRow["PostalCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PostalCode"]);
			Telephone = (dataRow["Telephone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telephone"]);
			Telephone2 = (dataRow["Telephone2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telephone2"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			Closed = (dataRow["Closed"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Closed"]);
			VatID = (dataRow["VatID"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["VatID"]);
		}
	}
}

