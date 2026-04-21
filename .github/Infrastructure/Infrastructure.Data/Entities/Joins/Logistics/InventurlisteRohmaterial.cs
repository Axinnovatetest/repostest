using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class InventurlisteRohmaterial
	{
		public decimal? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal Bestand { get; set; }
		public string Lagerort { get; set; }
		public decimal EK { get; set; }
		public decimal EK_Summe { get; set; }
		public string Gewicht { get; set; }
		public decimal Gesamtgewicht { get; set; }
		public string? Zolltarif_nr { get; set; }
		public string Ursprungsland { get; set; }
		public decimal LieferantenNr { get; set; }
		public string Name1 { get; set; }
		public string BestellNr { get; set; }
		public string BezeichnungAL { get; set; }
		public string Praferenz { get; set; }
		public int totalRows { get; set; }
		public InventurlisteRohmaterial(DataRow dataRow)
		{
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? null : Convert.ToDecimal(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestand"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			EK = (dataRow["EK"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["EK"]);
			EK_Summe = (dataRow["EK_Summe"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["EK_Summe"]);
			Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? "0" : (dataRow["Gewicht"].ToString().Contains("E")) ? dataRow["Gewicht"].ToString() : dataRow["Gewicht"].ToString();
			Gesamtgewicht = (dataRow["Gesamtgewicht"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gesamtgewicht"]);
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Zolltarif_nr"]);
			Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Ursprungsland"]);
			LieferantenNr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Lieferanten-Nr"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			BestellNr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			BezeichnungAL = (dataRow["BezeichnungAL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BezeichnungAL"]);
			Praferenz = (dataRow["Präferenz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Präferenz"]);
			totalRows = (dataRow["totalRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalRows"]);
		}
	}

}
