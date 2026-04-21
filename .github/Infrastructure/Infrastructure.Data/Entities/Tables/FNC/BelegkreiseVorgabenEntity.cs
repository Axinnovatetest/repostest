using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class BelegkreiseVorgabenEntity
	{
		public int? Belegkreis { get; set; }
		public string Bezeichnung { get; set; }
		public int ID { get; set; }

		public BelegkreiseVorgabenEntity() { }
		public BelegkreiseVorgabenEntity(DataRow dataRow)
		{
			Belegkreis = (dataRow["Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}
	}
}
