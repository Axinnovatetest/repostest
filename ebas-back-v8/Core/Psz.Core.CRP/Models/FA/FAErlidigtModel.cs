
namespace Psz.Core.CRP.Models.FA
{
	public class FAErlidigtModel
	{
		public int? Fertigungsnummer { get; set; }
		public string? Artikelnummer { get; set; }
		public string? Bezeichnung_1 { get; set; }
		public Decimal? Anzahl { get; set; }
		public Decimal? Originalanzahl { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public Decimal? Anzahl_erledigt { get; set; }
		public string? Mitarbeiter { get; set; }
		public string? Lagerort { get; set; }
		public Decimal? Anzahl_aktuell { get; set; }
		public int? Lagerort_id { get; set; }
		public string? Kennzeichen { get; set; }
		public Decimal? Faktor_Material { get; set; }
		public string? Lagerort_Entnahme { get; set; }
		public int? Lagerort_id_Entnahme { get; set; }
		public Decimal? Zeit { get; set; }
		public int ID { get; set; }
		public FAErlidigtModel()
		{

		}

		public FAErlidigtModel(Infrastructure.Data.Entities.Joins.FAUpdate.FAErlidigtEntity entity)
		{
			Fertigungsnummer = entity.Fertigungsnummer;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Anzahl = entity.Anzahl;
			Originalanzahl = entity.Originalanzahl;
			Termin_Fertigstellung = entity.Termin_Fertigstellung;
			Anzahl_erledigt = entity.Anzahl_erledigt;
			Mitarbeiter = entity.Mitarbeiter;
			Lagerort = entity.Lagerort;
			Anzahl_aktuell = entity.Anzahl_aktuell;
			Lagerort_id = entity.Lagerort_id;
			Kennzeichen = entity.Kennzeichen;
			Faktor_Material = entity.Faktor_Material;
			Lagerort_Entnahme = entity.Lagerort_Entnahme;
			Lagerort_id_Entnahme = entity.Lagerort_id_Entnahme;
			Zeit = entity.Zeit;
		}

		public Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity ToFAErledigenEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity
			{
				ID = ID,
				Fertigungsnummer = Fertigungsnummer,
				Artikelnummer = Artikelnummer,
				Bezeichnung_1 = Bezeichnung_1,
				Anzahl = Anzahl.HasValue ? Convert.ToInt32(Anzahl.Value) : 0,
				Originalanzahl = Originalanzahl.HasValue ? Convert.ToInt32(Originalanzahl.Value) : 0,
				Termin_Fertigstellung = Termin_Fertigstellung,
				Anzahl_erledigt = Anzahl_erledigt.HasValue ? Convert.ToInt32(Anzahl_erledigt.Value) : 0,
				Lagerort_id = Lagerort_id,
				Kennzeichen = Kennzeichen,
				Faktor_Material = Faktor_Material.HasValue ? Convert.ToInt32(Faktor_Material.Value) : 0,
				Lagerort_Entnahme = Lagerort_Entnahme,
				Lagerort_id_Entnahme = Lagerort_id_Entnahme,
				zeit = Zeit.HasValue ? Convert.ToInt32(Zeit.Value) : 0,
			};
		}
	}
}
