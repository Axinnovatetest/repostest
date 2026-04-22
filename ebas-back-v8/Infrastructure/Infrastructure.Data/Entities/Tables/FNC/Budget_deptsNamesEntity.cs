using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_deptsNamesEntity
	{
		public int ID { get; set; }
		public string Departement_name { get; set; }
		public string Land { get; set; }

		public Budget_deptsNamesEntity() { }

		public Budget_deptsNamesEntity(DataRow dataRow)
		{
			Departement_name = (dataRow["Departement_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Departement_name"]);
			Land = (dataRow["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}
	}
}
