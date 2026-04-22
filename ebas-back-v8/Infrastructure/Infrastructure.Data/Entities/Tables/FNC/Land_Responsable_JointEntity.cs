using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Land_Responsable_JointEntity
	{
		public int ID { get; set; }
		public int? ID_Land { get; set; }
		public int? ID_user { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }

		public Land_Responsable_JointEntity() { }

		public Land_Responsable_JointEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Land = (dataRow["ID_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Land"]);
			ID_user = (dataRow["ID_user"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_user"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Username = Convert.ToString(dataRow["Username"]);
		}
	}
}

