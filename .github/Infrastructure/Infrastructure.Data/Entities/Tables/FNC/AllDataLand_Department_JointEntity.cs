using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AllDataLand_Department_JointEntity
	{
		public int ID { get; set; }
		public int? ID_Department { get; set; }
		public string Departement_name { get; set; }
		public int? ID_Land { get; set; }
		public string Land_name { get; set; }
		public int? ID_user { get; set; }
		public string Name { get; set; }
		public string EmailUser { get; set; }

		public AllDataLand_Department_JointEntity() { }

		public AllDataLand_Department_JointEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Department = (dataRow["ID_Department"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Department"]);
			Departement_name = (dataRow["Departement_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Departement_name"]);
			ID_Land = (dataRow["ID_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Land"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			ID_user = (dataRow["ID_user"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_user"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			EmailUser = (dataRow["EmailUser"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmailUser"]);

		}
	}
}

