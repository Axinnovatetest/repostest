using System;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class Liste_50_ROH_Artikel_Selectione_Entity
	{
		public Liste_50_ROH_Artikel_Selectione_Entity(System.Data.DataRow dataRow)
		{
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerort_id"]);
			CCID = (dataRow["CCID"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["CCID"]);
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestand"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Stuckliste = (dataRow["Stückliste"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Stückliste"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Standardlieferant"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Wert = (dataRow["Wert"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Wert"]);
			CCID_Datum = (dataRow["CCID_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CCID_Datum"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Gewichte = (dataRow["Gewichte"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gewichte"]);

		}
		public int Lagerort_id { get; set; }
		public int CCID { get; set; }
		public int ArtikelNr { get; set; }
		public decimal Bestand { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int Stuckliste { get; set; }
		public int Standardlieferant { get; set; }
		public decimal Einkaufspreis { get; set; }
		public decimal Wert { get; set; }
		public DateTime? CCID_Datum { get; set; }
		public string Lagerort { get; set; }
		public decimal Gewichte { get; set; }
	}
}
