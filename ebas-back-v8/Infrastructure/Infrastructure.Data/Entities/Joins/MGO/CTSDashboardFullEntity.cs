using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class CTSDashboardFullEntity
	{
		public string KundenKlasse { get; set; }
		public int? ArtikelGesamtzahl { get; set; }
		public int? KundenGesamtzahl { get; set; }
		public decimal? UsamtzTotal { get; set; }
		public decimal? SofortUmsatz { get; set; }
		public decimal? FAUmsatz { get; set; }
		public decimal? Ergebnis { get; set; }
		public DateTime? SyncDatum { get; set; }

		public CTSDashboardFullEntity() { }

		public CTSDashboardFullEntity(DataRow dataRow)
		{
			KundenKlasse = Convert.ToString(dataRow["KundenKlasse"]);
			ArtikelGesamtzahl = (dataRow["ArtikelGesamtzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArtikelGesamtzahl"]);
			KundenGesamtzahl = (dataRow["KundenGesamtzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KundenGesamtzahl"]);
			UsamtzTotal = (dataRow["UsamtzTotal"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["UsamtzTotal"]);
			SofortUmsatz = (dataRow["SofortUmsatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SofortUmsatz"]);
			FAUmsatz = (dataRow["FAUmsatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FAUmsatz"]);
			Ergebnis = (dataRow["Ergebnis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ergebnis"]);
			SyncDatum = (dataRow["SyncDatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SyncDatum"]);
		}
	}
}
