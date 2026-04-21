using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class PosRahmenABEntity
	{
		public int? AngebotNr { get; set; }
		public int? Position { get; set; }

		public string Einheit { get; set; }
		public decimal? Menge { get; set; }
		public decimal? RefWert { get; set; }
		public PosRahmenABEntity(DataRow dataRow)
		{
			AngebotNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			Menge = (dataRow["OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OriginalAnzahl"]);
			RefWert = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
		}
	}
}
