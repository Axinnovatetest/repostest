using System;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class LagerBestandFGModel
	{
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Freigabestatus { get; set; }
		public string CS_Kontakt { get; set; }
		public string Lagerort { get; set; }
		public Decimal? Bestand { get; set; }
		public Decimal? VK_Gesamt { get; set; }
		public Decimal? Kosten_gesamt { get; set; }
		public Decimal? Kosten_gesamt_ohne_cu { get; set; }
		public Decimal? VKE { get; set; }

		public LagerBestandFGModel(Infrastructure.Data.Entities.Joins.CTS.LagerBestandFGEntity entity)
		{
			Artikelnummer = entity.Artikelnummer;
			Kunde = entity.Kunde;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Bezeichnung_2 = entity.Bezeichnung_2;
			Freigabestatus = entity.Freigabestatus;
			CS_Kontakt = entity.CS_Kontakt;
			Lagerort = entity.Lagerort;
			Bestand = entity.Bestand;
			VK_Gesamt = entity.VK_Gesamt;
			Kosten_gesamt = entity.Kosten_gesamt;
			Kosten_gesamt_ohne_cu = entity.Kosten_gesamt_ohne_cu;
			VKE = entity.VKE;
		}
	}
}
