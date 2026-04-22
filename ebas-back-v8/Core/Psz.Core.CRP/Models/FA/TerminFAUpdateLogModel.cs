using System;

namespace Psz.Core.CRP.Models.FA
{
	public class TerminFAUpdateLogModel
	{
		public DateTime? Anderungsdatum { get; set; }
		public int? Angebot_Nr { get; set; }
		public string? Artikelnummer { get; set; }
		public string? Bemerkung { get; set; }
		public string? Bezeichnung { get; set; }
		public string? CS_Mitarbeiter { get; set; }
		public bool? Erstmuster { get; set; }
		public int? FA_Menge { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string? Grund_CS { get; set; }
		public int ID { get; set; }
		public bool? Kapazitatsproblem { get; set; }
		public string? Kapazitatsproblematik { get; set; }
		public int? Lagerort_id { get; set; }
		public bool? Materialproblem { get; set; }
		public string? Materialproblematik { get; set; }
		public string? Mitarbeiter { get; set; }
		public string? Sonstige_Problematik { get; set; }
		public bool? Sonstiges { get; set; }
		public DateTime? Termin_Angebot { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public DateTime? Termin_voranderung { get; set; }
		public DateTime? Termin_Wunsch { get; set; }
		public DateTime? Ursprunglicher_termin { get; set; }
		public bool? Werkzeugproblem { get; set; }
		public string? Werkzeugproblematik { get; set; }
		public bool? Wunsch_CS { get; set; }
		// - 03/03-2025
		public TerminFAUpdateLogModel()
		{

		}
		public TerminFAUpdateLogModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity entity)
		{
			Anderungsdatum = entity.Änderungsdatum;
			Angebot_Nr = entity.Angebot_Nr;
			Artikelnummer = entity.Artikelnummer;
			Bemerkung = entity.Bemerkung;
			Bezeichnung = entity.Bezeichnung;
			CS_Mitarbeiter = entity.CS_Mitarbeiter;
			Erstmuster = entity.Erstmuster;
			FA_Menge = entity.FA_Menge;
			Fertigungsnummer = entity.Fertigungsnummer;
			Grund_CS = entity.Grund_CS;
			ID = entity.ID;
			Kapazitatsproblem = entity.Kapazitätsproblem;
			Kapazitatsproblematik = entity.Kapazitätsproblematik;
			Lagerort_id = entity.Lagerort_id;
			Materialproblem = entity.Materialproblem;
			Materialproblematik = entity.Materialproblematik;
			Mitarbeiter = entity.Mitarbeiter;
			Sonstige_Problematik = entity.Sonstige_Problematik;
			Sonstiges = entity.Sonstiges;
			Termin_Angebot = entity.Termin_Angebot;
			Termin_Bestatigt1 = entity.Termin_Bestätigt1;
			Termin_voranderung = entity.Termin_voränderung;
			Termin_Wunsch = entity.Termin_Wunsch;
			Ursprunglicher_termin = entity.Ursprünglicher_termin;
			Werkzeugproblem = entity.Werkzeugproblem;
			Werkzeugproblematik = entity.Werkzeugproblematik;
			Wunsch_CS = entity.Wunsch_CS;

		}
	}
}