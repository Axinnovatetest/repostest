using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class RechnungEndkontrolleEntity
	{
		public DateTime? Datum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Originalanzahl { get; set; }
		public int? Anzahl_erledigt { get; set; }
		public int? Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public decimal? Betrag { get; set; }
		public decimal? Ausdr3 { get; set; }
		public decimal? Preis { get; set; }
		public string Bemerkung { get; set; }
		public string Bezfeld { get; set; }
		public bool? Erstmuster { get; set; }
		public RechnungEndkontrolleEntity(DataRow dataRow)
		{
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			Anzahl_erledigt = (dataRow["Anzahl_erledigt"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_erledigt"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Ausdr3 = (dataRow["Ausdr3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ausdr3"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bezfeld = (dataRow["Bezfeld"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezfeld"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
		}

	}
}
