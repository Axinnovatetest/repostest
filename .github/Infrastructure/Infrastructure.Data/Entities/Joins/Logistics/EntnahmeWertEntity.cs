using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class EntnahmeWertEntity
	{
		public DateTime? datum { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public decimal anzahl { get; set; }
		public long zuFA { get; set; }
		public int grund { get; set; }
		public decimal kosten { get; set; }
		public string bemerkung { get; set; }
		public EntnahmeWertEntity(DataRow dr)
		{

			datum = (dr["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["Datum"]);
			artikelNr = (dr["ArtikelNr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["ArtikelNr"]);
			artikelnummer = (dr["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Artikelnummer"]);
			bezeichnung1 = (dr["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bezeichnung"]);
			zuFA = (dr["Fertigungsnummer"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dr["Fertigungsnummer"]);
			anzahl = (dr["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Anzahl"]);
			grund = (dr["Grund"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["Grund"]);
			kosten = (dr["Kosten"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dr["Kosten"]);
			bemerkung = (dr["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bemerkung"]);



		}
	}
}
