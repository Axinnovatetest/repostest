using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class KonditionsZuordnungsTabelleEntity
	{
		public int Nr { get; set; }
		public string Bemerkung { get; set; }
		public int? Nettotage { get; set; }
		public double? Skonto { get; set; }
		public int? Skontotage { get; set; }
		public string Text { get; set; }

		public KonditionsZuordnungsTabelleEntity() { }
		public KonditionsZuordnungsTabelleEntity(DataRow dataRow)
		{
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Nettotage = (dataRow["Nettotage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nettotage"]);
			Skonto = (dataRow["Skonto"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Skonto"]);
			Skontotage = (dataRow["Skontotage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Skontotage"]);
			Text = (dataRow["Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text"]);
		}
	}
}

