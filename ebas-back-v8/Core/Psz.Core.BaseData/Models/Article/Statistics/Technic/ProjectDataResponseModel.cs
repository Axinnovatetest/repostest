using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Technic
{
	public class ProjectDataResponseModel
	{
		public DateTime? AB_Datum { get; set; }
		public int? Arbeitszeit_Serien_Pro_Kabesatz { get; set; }
		public string Artikelnummer { get; set; }
		public string Bemerkungen { get; set; }
		public int? EAU { get; set; }
		public string EMPB { get; set; }
		public DateTime? Erstanlage { get; set; }
		public DateTime? FA_Datum { get; set; }
		public int ID { get; set; }
		public string Kontakt_AV_PSZ { get; set; }
		public string Kontakt_CS_PSZ { get; set; }
		public string Kontakt_Technik_Kunde { get; set; }
		public string Kontakt_Technik_PSZ { get; set; }
		public int? Kosten { get; set; }
		public string Krimp_WKZ { get; set; }
		public string Material_Eskalation_AV { get; set; }
		public string Material_Eskalation_Termin { get; set; }
		public string Material_Komplett { get; set; }
		public int? Menge { get; set; }
		public int? MOQ { get; set; }
		public string Projekt_betreung { get; set; }
		public string Projekt_Start { get; set; }
		public bool? Projektmeldung { get; set; }
		public string Projekt_Nr { get; set; }
		public string Rapid_Prototyp { get; set; }
		public string Serie_PSZ { get; set; }
		public string SG_WKZ { get; set; }
		public string Standort_Muster { get; set; }
		public string Standort_Serie { get; set; }
		public string Summe_Arbeitszeit { get; set; }
		public string Termin_mit_Technik_abgesprochen { get; set; }
		public string TSP_Kunden { get; set; }
		public string Typ { get; set; }
		public string UL_Verpackung { get; set; }
		public DateTime? Wunschtermin_Kunde { get; set; }
		public string Zuschlag { get; set; }

		public ProjectDataResponseModel()
		{

		}
		public ProjectDataResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_PSZ_Projektdaten_DetailsEntity entity)
		{
			if(entity == null)
				return;

			AB_Datum = entity.AB_Datum;
			Arbeitszeit_Serien_Pro_Kabesatz = entity.Arbeitszeit_Serien_Pro_Kabesatz;
			Artikelnummer = entity.Artikelnummer;
			Bemerkungen = entity.Bemerkungen;
			EAU = entity.EAU;
			EMPB = entity.EMPB;
			Erstanlage = entity.Erstanlage;
			FA_Datum = entity.FA_Datum;
			ID = entity.ID;
			Kontakt_AV_PSZ = entity.Kontakt_AV_PSZ;
			Kontakt_CS_PSZ = entity.Kontakt_CS_PSZ;
			Kontakt_Technik_Kunde = entity.Kontakt_Technik_Kunde;
			Kontakt_Technik_PSZ = entity.Kontakt_Technik_PSZ;
			Kosten = entity.Kosten;
			Krimp_WKZ = entity.Krimp_WKZ;
			Material_Eskalation_AV = entity.Material_Eskalation_AV;
			Material_Eskalation_Termin = entity.Material_Eskalation_Termin;
			Material_Komplett = entity.Material_Komplett;
			Menge = entity.Menge;
			MOQ = entity.MOQ;
			Projekt_betreung = entity.Projekt_betreung;
			Projekt_Start = entity.Projekt_Start;
			Projektmeldung = entity.Projektmeldung;
			Projekt_Nr = entity.Projekt_Nr;
			Rapid_Prototyp = entity.Rapid_Prototyp;
			Serie_PSZ = entity.Serie_PSZ;
			SG_WKZ = entity.SG_WKZ;
			Standort_Muster = entity.Standort_Muster;
			Standort_Serie = entity.Standort_Serie;
			Summe_Arbeitszeit = entity.Summe_Arbeitszeit;
			Termin_mit_Technik_abgesprochen = entity.Termin_mit_Technik_abgesprochen;
			TSP_Kunden = entity.TSP_Kunden;
			Typ = entity.Typ;
			UL_Verpackung = entity.UL_Verpackung;
			Wunschtermin_Kunde = entity.Wunschtermin_Kunde;
			Zuschlag = entity.Zuschlag;
		}
	}
}
