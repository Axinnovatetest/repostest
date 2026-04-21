using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class DeliveryNoteHistoryEntity
	{
		public int Nr { get; set; }
		public int Project_Nr { get; set; }
		public string Vorname_NameFirma { get; set; }
		public string Typ { get; set; }
		public string erledigt { get; set; }
		public int Vorfall_Nr { get; set; }
		public int Position { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string Bezeichnung1 { get; set; }
		public int? Anzahl { get; set; }
		public int? OriginalAnzahl { get; set; }
		public int? Geliefert_Abgerufen { get; set; }
		public bool? erledigt_pos { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Benutzer { get; set; }
		public int Artikel_Nr { get; set; }
		public DeliveryNoteHistoryEntity()
		{

		}

		public DeliveryNoteHistoryEntity(DataRow dataRow)
		{
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Project_Nr = Convert.ToInt32(dataRow["Projekt-Nr"]);
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["erledigt"]);
			Vorfall_Nr = Convert.ToInt32(dataRow["Vorfall-Nr"]);
			Position = Convert.ToInt32(dataRow["Position"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			OriginalAnzahl = (dataRow["OriginalAnzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OriginalAnzahl"]);
			Geliefert_Abgerufen = (dataRow["Geliefert_Abgerufen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Geliefert_Abgerufen"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Benutzer"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Artikel-Nr"]);
		}
	}
}
