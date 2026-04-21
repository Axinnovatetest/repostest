using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.PlantBookings
{

	public class PlantBookingDetailsResponseModel
	{
		public int? Aktiv { get; set; }
		public decimal? Akzeptierte_Menge { get; set; }
		public int? Anzahl_Verpackungen { get; set; }
		public string Artikelnummer { get; set; }
		public int? Bestellung_Nr { get; set; }
		public int? Clock_Number { get; set; }
		public DateTime? Datum { get; set; }
		public int? Eingangslieferscheinnr { get; set; }
		public decimal? Geprufte_Prufmenge { get; set; }
		public decimal? Gesamtmenge { get; set; }
		public string Inspektor { get; set; }
		public string Kunde { get; set; }
		public int? LagerortID { get; set; }
		public int? Laufende_Nummer { get; set; }
		public decimal? Menge { get; set; }
		public DateTime? MHDDatum { get; set; }
		public int Nummer_Verpackung { get; set; }
		public bool? Prufentscheid { get; set; }
		public decimal? Prufmenge { get; set; }
		public decimal? Pruftiefe { get; set; }
		public decimal? Reklamierte_Menge { get; set; }
		public decimal? Restmenge_Rolle_PPS { get; set; }
		public string Resultat { get; set; }
		public int? Status_Rolle { get; set; }
		public int? Verpackungsnr { get; set; }
		public decimal? WE_Anzahl_VOH { get; set; }
		public DateTime? WE_Datum_VOH { get; set; }
		public string WE_LS_VOH { get; set; }
		public int? Previous { get; set; } 
		public int? Next { get; set; }
		public PlantBookingDetailsResponseModel()
		{
			
		}

		public PlantBookingDetailsResponseModel(Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity data)
		{
			Aktiv = data.Aktiv == null ? 0 : data.Aktiv;
			Akzeptierte_Menge = data.Akzeptierte_Menge;
			Anzahl_Verpackungen = data.Anzahl_Verpackungen;
			Artikelnummer = data.Artikelnummer;
			Bestellung_Nr = data.Bestellung_Nr;
			Clock_Number = data.Clock_Number;
			Datum = data.Datum;
			Eingangslieferscheinnr = data.Eingangslieferscheinnr;
			Geprufte_Prufmenge = data.Geprufte_Prufmenge;
			Gesamtmenge = data.Gesamtmenge;
			Inspektor = data.Inspektor;
			Kunde = data.Kunde;
			LagerortID = data.LagerortID;
			Laufende_Nummer = data.Laufende_Nummer;
			Menge = data.Menge;
			MHDDatum = data.MHDDatum;
			Nummer_Verpackung = data.Nummer_Verpackung;
			Prufentscheid = data.Prufentscheid;
			Prufmenge = data.Prufmenge;
			Pruftiefe = data.Pruftiefe;
			Reklamierte_Menge = data.Reklamierte_Menge;
			Restmenge_Rolle_PPS = data.Restmenge_Rolle_PPS;
			Resultat = data.Resultat;
			Status_Rolle = data.Status_Rolle;
			Verpackungsnr = data.Verpackungsnr;
			WE_Anzahl_VOH = data.WE_Anzahl_VOH;
			WE_Datum_VOH = data.WE_Datum_VOH;
			WE_LS_VOH = data.WE_LS_VOH;
		}
	}
}
