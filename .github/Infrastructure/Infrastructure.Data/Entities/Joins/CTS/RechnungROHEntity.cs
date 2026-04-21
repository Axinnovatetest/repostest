using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class RechnungROHEntity
	{
		public string Typ { get; set; }
		public DateTime? Datum { get; set; }
		public string Artikelnummer { get; set; }
		public string Zolltarif_nr { get; set; }
		public int? Lagerplatz { get; set; }
		public Decimal? Eingangsmenge { get; set; }
		public Decimal? Gewicht { get; set; }
		public Decimal? Größe { get; set; }
		public bool? Standardlieferant { get; set; }
		public Decimal? Einkaufspreis { get; set; }
		public Decimal? Wert { get; set; }
		public RechnungROHEntity(DataRow dataRow)
		{
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			Lagerplatz = (dataRow["Lagerplatz"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerplatz"]);
			Eingangsmenge = (dataRow["Eingangsmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Eingangsmenge"]);
			Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gewicht"]);
			Größe = (dataRow["Größe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Größe"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Wert = (dataRow["Wert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Wert"]);
		}
	}
	public class RechnungROHTNEntity
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public Decimal? Menge { get; set; }

		public RechnungROHTNEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
		}
	}

}
