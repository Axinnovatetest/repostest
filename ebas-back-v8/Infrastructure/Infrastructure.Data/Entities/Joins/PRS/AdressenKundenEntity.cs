namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class AdressenKundenEntity
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int SupplierId { get; set; }
		public int AddressType { get; set; }
		public string DUNS { get; set; }
		public string PreName { get; set; }
		public int? SupplierNumber { get; set; }
		public int? CustomerNumber { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }

		public string Country { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string StreetZipCode { get; set; }
		public string Mailbox { get; set; }
		public string MailboxZipCode { get; set; }
		public bool? MailboxIsPreferred { get; set; }

		public string PhoneNumber { get; set; }
		public string FaxNumber { get; set; }
		public string EmailAdress { get; set; }
		public string Website { get; set; }

		public string Note { get; set; }
		public string Notes { get; set; }

		public string Salutation { get; set; }
		public string Department { get; set; }
		public bool? Adresslock { get; set; }

		public AdressenKundenEntity(DataRow dr)

		{
			Id = (dr["Id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Id"]);

			CustomerId = (dr["CustomerId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["CustomerId"]);
			SupplierId = (dr["SupplierId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["SupplierId"]);
			AddressType = (dr["AddressType"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["AddressType"]);
			DUNS = (dr["DUNS"] == System.DBNull.Value) ? "" : Convert.ToString(dr["DUNS"]);
			PreName = (dr["PreName"] == System.DBNull.Value) ? "" : Convert.ToString(dr["PreName"]);
			SupplierNumber = (dr["SupplierNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["SupplierNumber"]);
			CustomerNumber = (dr["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["CustomerNumber"]);
			Name1 = (dr["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Name1"]);
			Name2 = (dr["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Name2"]);
			Name3 = (dr["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Name3"]);
			Country = (dr["Country"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Country"]);
			City = (dr["City"] == System.DBNull.Value) ? "" : Convert.ToString(dr["City"]);
			Street = (dr["Street"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Street"]);
			StreetZipCode = (dr["StreetZipCode"] == System.DBNull.Value) ? "" : Convert.ToString(dr["StreetZipCode"]);
			Mailbox = (dr["Mailbox"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Mailbox"]);
			MailboxZipCode = (dr["MailboxZipCode"] == System.DBNull.Value) ? "" : Convert.ToString(dr["MailboxZipCode"]);
			MailboxIsPreferred = (dr["MailboxIsPreferred"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["MailboxIsPreferred"]);
			PhoneNumber = (dr["PhoneNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dr["PhoneNumber"]);
			FaxNumber = (dr["FaxNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dr["FaxNumber"]);
			EmailAdress = (dr["EmailAdress"] == System.DBNull.Value) ? "" : Convert.ToString(dr["EmailAdress"]);
			Website = (dr["Website"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Website"]);
			Note = (dr["Note"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Note"]);
			Notes = (dr["Notes"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Notes"]);
			Salutation = (dr["Salutation"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Salutation"]);
			Department = (dr["Department"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Department"]);
			Adresslock = (dr["Adresslock"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["Adresslock"]);
		}
	}
}
