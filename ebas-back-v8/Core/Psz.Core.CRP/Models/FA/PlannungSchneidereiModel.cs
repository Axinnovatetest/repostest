using System;

namespace Psz.Core.CRP.Models.FA
{
	public class PlannungSchneidereiResponseModel
	{
		public DateTime? FA_begonnen { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Freigabestatus { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public string Halle { get; set; }
		public string Kunde { get; set; }
		public Single? Order_Time { get; set; }
		public string PSZ_Artikelnummer { get; set; }
		public int? Quantity { get; set; }
		public DateTime? Termin_Planung { get; set; }
		public DateTime? Termin_Schneiderei { get; set; }

		// -
		public bool? FA_Gestartet { get; set; }

		public PlannungSchneidereiResponseModel(Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity entity)
		{
			if(entity == null)
				return;

			FA_begonnen = entity.FA_begonnen;
			Fertigungsnummer = entity.Fertigungsnummer;
			Freigabestatus = entity.Freigabestatus;
			Gewerk_1 = entity.Gewerk_1;
			Gewerk_2 = entity.Gewerk_2;
			Gewerk_3 = entity.Gewerk_3;
			Halle = entity.Halle;
			Kunde = entity.Kunde;
			Order_Time = entity.Order_Time;
			PSZ_Artikelnummer = entity.PSZ_Artikelnummer;
			Quantity = entity.Quantity;
			Termin_Planung = entity.Termin_Bestätigt1;
			Termin_Schneiderei = entity.Termin_Schneiderei;
		}
		public PlannungSchneidereiResponseModel(Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity entity)
		{
			if(entity == null)
				return;

			FA_begonnen = entity.FA_begonnen;
			Fertigungsnummer = entity.Fertigungsnummer;
			Freigabestatus = entity.Freigabestatus;
			Gewerk_1 = entity.Gewerk_1;
			Gewerk_2 = entity.Gewerk_2;
			Gewerk_3 = entity.Gewerk_3;
			Halle = entity.Halle;
			Kunde = entity.Kunde;
			Order_Time = entity.Order_Time;
			PSZ_Artikelnummer = entity.PSZ_Artikelnummer;
			Quantity = entity.Quantity;
			Termin_Planung = entity.Termin_Bestätigt1;
			Termin_Schneiderei = entity.Termin_Schneiderei;
			// -
			FA_Gestartet = entity.FA_Gestartet;
		}
	}
}
