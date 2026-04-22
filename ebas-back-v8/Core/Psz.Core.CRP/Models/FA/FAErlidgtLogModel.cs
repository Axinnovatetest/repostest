using System;

namespace Psz.Core.CRP.Models.FA
{
	public class FAErlidgtLogModel
	{
		public int? Anzahl { get; set; }
		public int? Anzahl_aktuell { get; set; }
		public int? Anzahl_erledigt { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int? Faktor_Material { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Kennzeichen { get; set; }
		public string Lagerort { get; set; }
		public string Mitarbeiter { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }

		public FAErlidgtLogModel()
		{

		}
		public FAErlidgtLogModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity entity)
		{
			Anzahl = entity.Anzahl;
			Anzahl_aktuell = entity.Anzahl_aktuell;
			Anzahl_erledigt = entity.Anzahl_erledigt;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Faktor_Material = entity.Faktor_Material;
			Fertigungsnummer = entity.Fertigungsnummer;
			Kennzeichen = entity.Kennzeichen;
			Lagerort = entity.Lagerort;
			Mitarbeiter = entity.Mitarbeiter;
			Termin_Fertigstellung = entity.Termin_Fertigstellung;
		}

	}
}