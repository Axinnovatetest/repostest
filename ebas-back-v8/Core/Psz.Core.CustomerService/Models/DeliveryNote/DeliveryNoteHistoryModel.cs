using System;

namespace Psz.Core.CustomerService.Models.DeliveryNote
{
	public class DeliveryNoteHistoryModel
	{
		public string Artikelnummer { get; set; }
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
		public DeliveryNoteHistoryModel(Infrastructure.Data.Entities.Tables.CTS.DeliveryNoteHistoryEntity historyEntity)
		{
			Artikelnummer = (historyEntity.Artikel_Nr != -1) ? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(historyEntity.Artikel_Nr).ArtikelNummer : "";
			Nr = historyEntity.Nr;
			Project_Nr = historyEntity.Project_Nr;
			Vorname_NameFirma = historyEntity.Vorname_NameFirma;
			Typ = historyEntity.Typ;
			erledigt = historyEntity.erledigt;
			Vorfall_Nr = historyEntity.Vorfall_Nr;
			Position = historyEntity.Position;
			Liefertermin = historyEntity.Liefertermin;
			Bezeichnung1 = historyEntity.Bezeichnung1;
			Anzahl = historyEntity.Anzahl;
			OriginalAnzahl = historyEntity.OriginalAnzahl;
			Geliefert_Abgerufen = historyEntity.Geliefert_Abgerufen;
			erledigt_pos = historyEntity.erledigt_pos;
			Fertigungsnummer = historyEntity.Fertigungsnummer;
			Benutzer = historyEntity.Benutzer;
		}
	}
}
