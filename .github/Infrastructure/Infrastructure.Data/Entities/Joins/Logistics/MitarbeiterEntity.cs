using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class MitarbeiterEntity
	{
		public string username { get; set; }
		public string name { get; set; }
		public string land { get; set; }
		public string abteilung { get; set; }

		public MitarbeiterEntity(DataRow dataRow)
		{
			username = (dataRow["username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["username"]);
			name = (dataRow["nameMitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["nameMitarbeiter"]);
			land = (dataRow["nameLand"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["nameLand"]);
			abteilung = (dataRow["nameAbteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["nameAbteilung"]);
		}
	}
}
