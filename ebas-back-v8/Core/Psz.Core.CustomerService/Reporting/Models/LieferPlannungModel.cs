using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class LieferPlannungReportModel
	{
		public List<DataItem> Data { get; set; }
	}

	public class DataItem
	{
		public int Year { get; set; }
		public List<Item> Items { get; set; }
	}

	public class Item
	{
		public int Week { get; set; }
		public List<LieferPlannungDetailsReportModel> Details { get; set; }
	}
	public class LieferPlannungDetailsReportModel
	{
		public string Vorname_NameFirma { get; set; }
		public string LName2 { get; set; }
		public string LLand_PLZ_Ort { get; set; }
		public string Wunschtermin { get; set; }
		public string Liefertermin { get; set; }
		public int Angebot_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int Menge_Offen { get; set; }
		public int Bestand { get; set; }
		public int Jahr { get; set; }
		public int KW { get; set; }
		public Decimal Gesamtpreis { get; set; }
		public string CSInterneBemerkung { get; set; }

		public LieferPlannungDetailsReportModel(Infrastructure.Data.Entities.Joins.CTS.LierferPlannungEntity entity)
		{
			if(entity == null)
				return;
			Vorname_NameFirma = $"{entity.Dokumentnummer} - {entity.Vorname_NameFirma}".Trim(new char[] { ' ', '-' });
			LName2 = entity?.LName2;
			LLand_PLZ_Ort = entity?.LLand_PLZ_Ort;
			Wunschtermin = entity.Wunschtermin.HasValue ? entity.Wunschtermin.Value.ToString("dd.MM.yyyy") : "";
			Liefertermin = entity.Liefertermin.HasValue ? entity.Liefertermin.Value.ToString("dd.MM.yyyy") : "";
			Angebot_Nr = entity.Angebot_Nr ?? -1;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Menge_Offen = entity.Menge_Offen ?? 0;
			Bestand = entity.Bestand ?? 0;
			Jahr = entity.Jahr ?? 0;
			KW = entity.KW ?? 0;
			Gesamtpreis = entity.Gesamtpreis ?? 0;
			CSInterneBemerkung = entity.CSInterneBemerkung;
		}
	}
}
