using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class BomMailUsersEntity
	{
		public int Id { get; set; }
		public string SiteCode { get; set; }
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public string UserEmail { get; set; }
		public string UserfullName { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }

		public BomMailUsersEntity() { }

		public BomMailUsersEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			SiteCode = (dataRow["SiteCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SiteCode"]);
			SiteId = Convert.ToInt32(dataRow["SiteId"]);
			SiteName = (dataRow["SiteName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SiteName"]);
			UserEmail = (dataRow["UserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserEmail"]);
			UserfullName = (dataRow["UserfullName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserfullName"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
		}
	}
}

