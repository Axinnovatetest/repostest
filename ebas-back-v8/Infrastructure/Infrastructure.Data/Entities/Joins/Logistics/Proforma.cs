using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class Proforma
	{

		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal Bestand { get; set; }
		public string Einheit { get; set; }
		public decimal EK { get; set; }
		public decimal EK_Summe { get; set; }
		public string Gewicht { get; set; }
		public decimal Gesamtgewicht { get; set; }
		public string Zolltarif_nr { get; set; }
		public string Ursprungsland { get; set; }
		public string Name1 { get; set; }
		public string Praeferenz_Aktuelles_jahr { get; set; }
		public string Standardlieferant { get; set; }
		public int totalRows { get; set; }
		public Proforma(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestand"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			EK = (dataRow["EK"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["EK"]);
			EK_Summe = (dataRow["EK_Summe"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["EK_Summe"]);
			Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Gewicht"]);
			Gesamtgewicht = (dataRow["Gesamtgewicht"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gesamtgewicht"]);
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Zolltarif_nr"]);
			Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Ursprungsland"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Praeferenz_Aktuelles_jahr = (dataRow["Praeferenz_Aktuelles_jahr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Praeferenz_Aktuelles_jahr"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardlieferant"]);
			totalRows = (dataRow["totalRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalRows"]);
		}
	}
}
