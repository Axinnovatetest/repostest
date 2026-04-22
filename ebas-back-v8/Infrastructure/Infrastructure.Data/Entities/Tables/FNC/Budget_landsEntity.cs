using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_landsEntity
	{
		public int ID { get; set; }
		public string Land_name { get; set; }
		public string PurchaseEmail { get; set; }
		public string PurchaseGroupName { get; set; }
		public int? PurchaseId { get; set; }
		public string PurchaseName { get; set; }
		public string SiteDirectorEmail { get; set; }
		public int? SiteDirectorId { get; set; }
		public string SiteDirectorName { get; set; }

		public Budget_landsEntity() { }

		public Budget_landsEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			PurchaseEmail = (dataRow["PurchaseEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PurchaseEmail"]);
			PurchaseGroupName = (dataRow["PurchaseGroupName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PurchaseGroupName"]);
			PurchaseId = (dataRow["PurchaseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PurchaseId"]);
			PurchaseName = (dataRow["PurchaseName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PurchaseName"]);
			SiteDirectorEmail = (dataRow["SiteDirectorEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SiteDirectorEmail"]);
			SiteDirectorId = (dataRow["SiteDirectorId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SiteDirectorId"]);
			SiteDirectorName = (dataRow["SiteDirectorName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SiteDirectorName"]);
		}
	}
}

