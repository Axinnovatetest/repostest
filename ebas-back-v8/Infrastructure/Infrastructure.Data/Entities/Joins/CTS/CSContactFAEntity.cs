using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class CSContactFAEntity
	{
		public string CS_Kontakt { get; set; }
		public string Kennzeichen { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Datum { get; set; }
		public int? Fertigungsnummer { get; set; }
		DateTime? Termin_Bestätigt1 { get; set; }
		DateTime? Termin_Bestätigt2 { get; set; }
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }

		public CSContactFAEntity(DataRow dataRow)
		{
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Termin_Bestätigt2 = (dataRow["Termin_Bestätigt2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt2"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
		}

	}

	public class CSContactFAExcelEntity
	{
		public DateTime? Datum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int? Menge_Gesamt { get; set; }
		public int? Menge_Erledigt { get; set; }
		public int? OffeneMenge { get; set; }
		public string FA_Status { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Ursrünglicher_FA_Termin { get; set; }
		public DateTime? Neuer_Termin { get; set; }
		public string FA_Bemerkung_Intern { get; set; }
		public string Kunde { get; set; }
		public string CS_Kontakt { get; set; }

		public CSContactFAExcelEntity(DataRow dataRow)
		{
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Menge_Gesamt = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			Menge_Erledigt = (dataRow["Anzahl_erledigt"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_erledigt"]);
			OffeneMenge = (dataRow["Anzahl_aktuell"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_aktuell"]);
			FA_Status = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Ursrünglicher_FA_Termin = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Neuer_Termin = (dataRow["Termin_Bestätigt2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt2"]);
			FA_Bemerkung_Intern = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
		}
	}
}
