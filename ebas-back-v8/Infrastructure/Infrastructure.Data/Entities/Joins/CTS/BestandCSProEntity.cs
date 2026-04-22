using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class BestandCSProEntity
	{
		public Decimal? Wert { get; set; }
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }
		public string CS_Kontakt { get; set; }
		public int? Lagerort_id { get; set; }
		public int? Bestand { get; set; }
		public string Lagerort { get; set; }
		public Decimal? VK { get; set; }
		public Decimal? Materialkosten { get; set; }
		public Decimal? Arbeitskosten { get; set; }

		public BestandCSProEntity(DataRow dataRow)
		{
			Wert = (dataRow["Wert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Wert"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestand"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			VK = (dataRow["VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK"]);
			Materialkosten = (dataRow["Materialkosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Materialkosten"]);
			Arbeitskosten = (dataRow["Arbeitskosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Arbeitskosten"]);
		}
	}
}
