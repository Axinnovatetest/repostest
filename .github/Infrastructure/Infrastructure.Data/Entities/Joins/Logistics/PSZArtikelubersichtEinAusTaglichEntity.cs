using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class PSZArtikelubersichtEinAusTaglichEntity
	{
		public PSZArtikelubersichtEinAusTaglichEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			BestellungNr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? null : Convert.ToDecimal(dataRow["Anzahl"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Datum"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Lagerplatz_von = (dataRow["Lagerplatz_von"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerplatz_von"]);
			Lagerplatz_nach = (dataRow["Lagerplatz_nach"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerplatz_nach"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? null : Convert.ToDecimal(dataRow["Mindestbestellmenge"]);
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? null : Convert.ToDecimal(dataRow["Verpackungseinheit"]);
			totalRows = (dataRow["totalRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalRows"]);
		}
		public string Artikelnummer { get; set; }
		public string Typ { get; set; }
		public int? BestellungNr { get; set; }
		public decimal? Anzahl { get; set; }
		public DateTime? Datum { get; set; }
		public string Name1 { get; set; }
		public int Lagerplatz_von { get; set; }
		public int Lagerplatz_nach { get; set; }
		public decimal? Mindestbestellmenge { get; set; }
		public decimal? Verpackungseinheit { get; set; }
		public int totalRows { get; set; }
	}
}
