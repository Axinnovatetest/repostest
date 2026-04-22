using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class PSZ_Fertigungsauftrag_ÄnderungshistorieEntity
	{
		public DateTime? Änderungsdatum { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bemerkung { get; set; }
		public string Bezeichnung { get; set; }
		public string CS_Mitarbeiter { get; set; }
		public bool? Erstmuster { get; set; }
		public int? FA_Menge { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Grund_CS { get; set; }
		public int ID { get; set; }
		public bool? Kapazitätsproblem { get; set; }
		public string Kapazitätsproblematik { get; set; }
		public int? Lagerort_id { get; set; }
		public bool? Materialproblem { get; set; }
		public string Materialproblematik { get; set; }
		public string Mitarbeiter { get; set; }
		public string Sonstige_Problematik { get; set; }
		public bool? Sonstiges { get; set; }
		public DateTime? Termin_Angebot { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }
		public DateTime? Termin_voränderung { get; set; }
		public DateTime? Termin_Wunsch { get; set; }
		public DateTime? Ursprünglicher_termin { get; set; }
		public bool? Werkzeugproblem { get; set; }
		public string Werkzeugproblematik { get; set; }
		public bool? Wunsch_CS { get; set; }

		public PSZ_Fertigungsauftrag_ÄnderungshistorieEntity() { }

		public PSZ_Fertigungsauftrag_ÄnderungshistorieEntity(DataRow dataRow)
		{
			Änderungsdatum = (dataRow["Änderungsdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Änderungsdatum"]);
			Angebot_Nr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			CS_Mitarbeiter = (dataRow["CS_Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS_Mitarbeiter"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			FA_Menge = (dataRow["FA_Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA_Menge"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Grund_CS = (dataRow["Grund_CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund_CS"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kapazitätsproblem = (dataRow["Kapazitätsproblem"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kapazitätsproblem"]);
			Kapazitätsproblematik = (dataRow["Kapazitätsproblematik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kapazitätsproblematik"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Materialproblem = (dataRow["Materialproblem"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Materialproblem"]);
			Materialproblematik = (dataRow["Materialproblematik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Materialproblematik"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter"]);
			Sonstige_Problematik = (dataRow["Sonstige_Problematik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sonstige_Problematik"]);
			Sonstiges = (dataRow["Sonstiges"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Sonstiges"]);
			Termin_Angebot = (dataRow["Termin_Angebot"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Angebot"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Termin_voränderung = (dataRow["Termin_voränderung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_voränderung"]);
			Termin_Wunsch = (dataRow["Termin_Wunsch"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Wunsch"]);
			Ursprünglicher_termin = (dataRow["Ursprünglicher_termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ursprünglicher_termin"]);
			Werkzeugproblem = (dataRow["Werkzeugproblem"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Werkzeugproblem"]);
			Werkzeugproblematik = (dataRow["Werkzeugproblematik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Werkzeugproblematik"]);
			Wunsch_CS = (dataRow["Wunsch_CS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Wunsch_CS"]);
		}
	}
}

