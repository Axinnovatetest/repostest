using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class PSZ_FA_erledigen_HilfstabelleEntity
	{
		public int? Anzahl { get; set; }
		public int? Anzahl_aktuell { get; set; }
		public int? Anzahl_erledigt { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int? Faktor_Material { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int ID { get; set; }
		public string Kennzeichen { get; set; }
		public string Lagerort { get; set; }
		public string Lagerort_Entnahme { get; set; }
		public int? Lagerort_id { get; set; }
		public int? Lagerort_id_Entnahme { get; set; }
		public string Mitarbeiter { get; set; }
		public int? Originalanzahl { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public int? zeit { get; set; }

		public PSZ_FA_erledigen_HilfstabelleEntity() { }

		public PSZ_FA_erledigen_HilfstabelleEntity(DataRow dataRow)
		{
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Anzahl_aktuell = (dataRow["Anzahl_aktuell"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_aktuell"]);
			Anzahl_erledigt = (dataRow["Anzahl_erledigt"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_erledigt"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Faktor_Material = (dataRow["Faktor Material"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Faktor Material"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Lagerort_Entnahme = (dataRow["Lagerort Entnahme"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort Entnahme"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Lagerort_id_Entnahme = (dataRow["Lagerort_id Entnahme"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id Entnahme"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter"]);
			Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			Termin_Fertigstellung = (dataRow["Termin_Fertigstellung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Fertigstellung"]);
			zeit = (dataRow["zeit"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["zeit"]);
		}
	}
}

