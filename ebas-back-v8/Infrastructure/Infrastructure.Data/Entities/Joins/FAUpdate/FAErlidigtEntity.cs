using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAUpdate
{
	public class FAErlidigtEntity
	{
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public Decimal? Anzahl { get; set; }
		public Decimal? Originalanzahl { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public Decimal? Anzahl_erledigt { get; set; }
		public string Mitarbeiter { get; set; }
		public string Lagerort { get; set; }
		public Decimal? Anzahl_aktuell { get; set; }
		public int? Lagerort_id { get; set; }
		public string Kennzeichen { get; set; }
		public Decimal? Faktor_Material { get; set; }
		public string Lagerort_Entnahme { get; set; }
		public int? Lagerort_id_Entnahme { get; set; }
		public Decimal? Zeit { get; set; }

		public FAErlidigtEntity(DataRow dataRow)
		{
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Originalanzahl"]);
			Termin_Fertigstellung = (dataRow["Termin_Fertigstellung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Fertigstellung"]);
			Anzahl_erledigt = (dataRow["Anzahl_erledigt"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Anzahl_erledigt"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Anzahl_aktuell = (dataRow["Anzahl_aktuell"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Anzahl_aktuell"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Faktor_Material = (dataRow["Faktor Material"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Faktor Material"]);
			Lagerort_Entnahme = (dataRow["Lagerort01"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort01"]);
			Lagerort_id_Entnahme = (dataRow["Lage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lage"]);
			Zeit = (dataRow["Zeit"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Zeit"]);
		}
	}
}
