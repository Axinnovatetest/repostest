using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Land_Department_JointEntity
	{
		public string EmailUser { get; set; }
		public int ID { get; set; }
		public int? ID_Department { get; set; }
		public int? ID_Land { get; set; }
		public int? ID_user { get; set; }

		public Land_Department_JointEntity() { }

		public Land_Department_JointEntity(DataRow dataRow)
		{
			EmailUser = (dataRow["EmailUser"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailUser"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Department = (dataRow["ID_Department"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Department"]);
			ID_Land = (dataRow["ID_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Land"]);
			ID_user = (dataRow["ID_user"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_user"]);
		}
	}
}

