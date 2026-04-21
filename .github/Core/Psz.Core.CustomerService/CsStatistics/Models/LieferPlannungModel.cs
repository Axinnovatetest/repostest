using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class LieferPlannungModel
	{
		public string Vorname_NameFirma { get; set; }
		public string LName2 { get; set; }
		public string LLand_PLZ_Ort { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int? Menge_Offen { get; set; }
		public int? Bestand { get; set; }
		public int? Jahr { get; set; }
		public int? KW { get; set; }
		public Decimal? Gesamtpreis { get; set; }

		public LieferPlannungModel(Infrastructure.Data.Entities.Joins.CTS.LierferPlannungEntity entity)
		{
			Vorname_NameFirma = entity.Vorname_NameFirma;
			LName2 = entity.LName2;
			LLand_PLZ_Ort = entity.LLand_PLZ_Ort;
			Wunschtermin = entity.Wunschtermin;
			Liefertermin = entity.Liefertermin;
			Angebot_Nr = entity.Angebot_Nr;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Menge_Offen = entity.Menge_Offen;
			Bestand = entity.Bestand;
			Jahr = entity.Jahr;
			KW = entity.KW;
			Gesamtpreis = entity.Gesamtpreis;
		}
	}
	public class LieferPlannungDetailsModel
	{
		public string PSZ { get; set; }
		public string Bezeichung { get; set; }
		public List<LieferPlannungBestandModel> Bestand { get; set; }
		public List<LieferPlannungFertigungModel> Fertigung { get; set; }
	}
	public class LieferPlannungBestandModel
	{
		public int? LagerNr { get; set; }
		public string Lagerort { get; set; }
		public int? Bestand { get; set; }

		public LieferPlannungBestandModel(Infrastructure.Data.Entities.Joins.CTS.LieferPlannungBestandEntity entity)
		{
			LagerNr = entity.Lagerort_id;
			Lagerort = entity.Lagerort;
			Bestand = entity.Bestand;
		}
	}

	public class LieferPlannungFertigungModel
	{
		public int? FA { get; set; }
		public string Lagerort { get; set; }
		public bool? Gestart { get; set; }
		public int? Original_menge { get; set; }
		public int? FA_Menge { get; set; }
		public DateTime? TerminProduction { get; set; }

		public LieferPlannungFertigungModel(Infrastructure.Data.Entities.Joins.CTS.LieferPlannungFertigungEntity entity)
		{
			FA = entity.Fertigungsnummer;
			Lagerort = entity.Lagerort;
			Gestart = entity.FA_Gestartet;
			Original_menge = entity.Originalanzahl;
			FA_Menge = entity.Menge_Offen;
			TerminProduction = entity.Termin_Bestätigt1;
		}
	}
}
