using System;

namespace Psz.Core.CRP.Models.FA
{
	public class FAUpdateTerminModel
	{
		public DateTime? Anderungsdatum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string? Bezeichnung { get; set; }
		public Decimal? FA_Menge { get; set; }
		public string? Artikelnummer { get; set; }
		public string? CA_Mitarbeiter { get; set; }
		public string? Angebot_Nr { get; set; }
		public DateTime? Termin_Angebot { get; set; }
		public DateTime? Termin_Wunsh { get; set; }
		public DateTime? Ursprunglicher_termin { get; set; }
		public DateTime? Termin_voranderung1 { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public bool? Materialproblem { get; set; }
		public string? Materialproblematik { get; set; }
		public bool? Kapazitatsproblem { get; set; }
		public string? Kapazitatsproblematik { get; set; }
		public bool? Werkzeugproblem { get; set; }
		public string? Werkzeugproblematik { get; set; }
		public bool? Wunsch_CS { get; set; }
		public string? Grund_CS { get; set; }
		public bool? Sonstiges { get; set; }
		public string? Sonstige_Problematik { get; set; }
		public FAUpdateTerminModel()
		{

		}
		public FAUpdateTerminModel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity faEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angebotEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angebotArticleEntity,
			string csKontakt
			)
		{
			FA_Menge = faEntity.Anzahl;
			Anderungsdatum = DateTime.Now;
			Fertigungsnummer = faEntity.Fertigungsnummer;
			Bezeichnung = articleEntity.Bezeichnung1;
			Artikelnummer = articleEntity.ArtikelNummer;
			Termin_Wunsh = faEntity.Termin_Fertigstellung;
			Ursprunglicher_termin = faEntity.Termin_Ursprunglich;
			Termin_voranderung1 = faEntity.Termin_Bestatigt1;
			Angebot_Nr = angebotEntity?.Projekt_Nr;
			Termin_Angebot = angebotArticleEntity?.Liefertermin;
			CA_Mitarbeiter = csKontakt;
		}
	}
}