using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class ProjectMessageResponseModel
	{
		public List<ProjectMessageItemModel> data { get; set; }

		// - pagination data
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}

	public class ProjectMessageMultiRequestModel
	{
		public List<ProjectMessageItemModel> Items { get; set; }
	}
	public class ProjectMessageItemModel
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

		public ProjectMessageItemModel()
		{

		}
		public ProjectMessageItemModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessage projectMessage)
		{
			if(projectMessage == null)
				return;

			AB_Datum = projectMessage.AB_Datum;
			Arbeitszeit_Serien_Pro_Kabesatz = projectMessage.Arbeitszeit_Serien_Pro_Kabesatz;
			Artikelnummer = projectMessage.Artikelnummer;
			Bemerkungen = projectMessage.Bemerkungen;
			EAU = projectMessage.EAU;
			EMPB = projectMessage.EMPB;
			Erstanlage = projectMessage.Erstanlage;
			FA_Datum = projectMessage.FA_Datum;
			ID = projectMessage.ID;
			Kontakt_AV_PSZ = projectMessage.Kontakt_AV_PSZ;
			Kontakt_CS_PSZ = projectMessage.Kontakt_CS_PSZ;
			Kontakt_Technik_Kunde = projectMessage.Kontakt_Technik_Kunde;
			Kontakt_Technik_PSZ = projectMessage.Kontakt_Technik_PSZ;
			Kosten = projectMessage.Kosten;
			Krimp_WKZ = projectMessage.Krimp_WKZ;
			Material_Eskalation_AV = projectMessage.Material_Eskalation_AV;
			Material_Eskalation_Termin = projectMessage.Material_Eskalation_Termin;
			Material_Komplett = projectMessage.Material_Komplett;
			Menge = projectMessage.Menge;
			MOQ = projectMessage.MOQ;
			Projekt_betreung = projectMessage.Projekt_betreung;
			Projekt_Start = projectMessage.Projekt_Start;
			Projektmeldung = projectMessage.Projektmeldung;
			Projekt_Nr = projectMessage.Projekt_Nr;
			Rapid_Prototyp = projectMessage.Rapid_Prototyp;
			Serie_PSZ = projectMessage.Serie_PSZ;
			SG_WKZ = projectMessage.SG_WKZ;
			Standort_Muster = projectMessage.Standort_Muster;
			Standort_Serie = projectMessage.Standort_Serie;
			Summe_Arbeitszeit = $"{(decimal.TryParse(projectMessage.Summe_Arbeitszeit, out var v) ? v.ToString("0.####") + "Std." : "")}"; // - projectMessage.Summe_Arbeitszeit;
			Termin_mit_Technik_abgesprochen = projectMessage.Termin_mit_Technik_abgesprochen;
			TSP_Kunden = projectMessage.TSP_Kunden;
			Typ = projectMessage.Typ;
			UL_Verpackung = projectMessage.UL_Verpackung;
			Wunschtermin_Kunde = projectMessage.Wunschtermin_Kunde;
			Zuschlag = projectMessage.Zuschlag;
		}
		public Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity
			{
				AB_Datum = AB_Datum,
				Arbeitszeit_Serien_Pro_Kabesatz = Arbeitszeit_Serien_Pro_Kabesatz,
				Artikelnummer = Artikelnummer,
				Bemerkungen = Bemerkungen,
				EAU = EAU,
				EMPB = EMPB,
				Erstanlage = Erstanlage,
				FA_Datum = FA_Datum,
				ID = ID,
				Kontakt_AV_PSZ = Kontakt_AV_PSZ,
				Kontakt_CS_PSZ = Kontakt_CS_PSZ,
				Kontakt_Technik_Kunde = Kontakt_Technik_Kunde,
				Kontakt_Technik_PSZ = Kontakt_Technik_PSZ,
				Kosten = Kosten,
				Krimp_WKZ = Krimp_WKZ,
				Material_Eskalation_AV = Material_Eskalation_AV,
				Material_Eskalation_Termin = Material_Eskalation_Termin,
				Material_Komplett = Material_Komplett,
				Menge = Menge,
				MOQ = MOQ,
				Projekt_betreung = Projekt_betreung,
				Projekt_Start = Projekt_Start,
				Projektmeldung = Projektmeldung,
				Projekt_Nr = Projekt_Nr,
				Rapid_Prototyp = Rapid_Prototyp,
				Serie_PSZ = Serie_PSZ,
				SG_WKZ = SG_WKZ,
				Standort_Muster = Standort_Muster,
				Standort_Serie = Standort_Serie,
				// Summe_Arbeitszeit = $"{(decimal.TryParse(Summe_Arbeitszeit, out var v) ? v.ToString("0.####") + "Std." : "")}", // Summe_Arbeitszeit,
				Termin_mit_Technik_abgesprochen = Termin_mit_Technik_abgesprochen,
				TSP_Kunden = TSP_Kunden,
				Typ = Typ,
				UL_Verpackung = UL_Verpackung,
				Wunschtermin_Kunde = Wunschtermin_Kunde,
				Zuschlag = Zuschlag
			};
		}
	}
	public class ProjectMessageItemRequestModel
	{
		public string AB_Datum { get; set; }
		public int? Arbeitszeit_Serien_Pro_Kabesatz { get; set; }
		public string Artikelnummer { get; set; }
		public string Bemerkungen { get; set; }
		public int? EAU { get; set; }
		public string EMPB { get; set; }
		public string Erstanlage { get; set; }
		public string FA_Datum { get; set; }
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
		public bool isPrint { get; set; } = false;

		public ProjectMessageItemRequestModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessage projectMessage)
		{
			if(projectMessage == null)
				return;

			AB_Datum = projectMessage.AB_Datum?.ToString("dd/MM/yyyy");
			Arbeitszeit_Serien_Pro_Kabesatz = projectMessage.Arbeitszeit_Serien_Pro_Kabesatz;
			Artikelnummer = projectMessage.Artikelnummer;
			Bemerkungen = projectMessage.Bemerkungen;
			EAU = projectMessage.EAU;
			EMPB = projectMessage.EMPB;
			Erstanlage = projectMessage.Erstanlage?.ToString("dd/MM/yyyy");
			FA_Datum = projectMessage.FA_Datum?.ToString("dd/MM/yyyy");
			ID = projectMessage.ID;
			Kontakt_AV_PSZ = projectMessage.Kontakt_AV_PSZ;
			Kontakt_CS_PSZ = projectMessage.Kontakt_CS_PSZ;
			Kontakt_Technik_Kunde = projectMessage.Kontakt_Technik_Kunde;
			Kontakt_Technik_PSZ = projectMessage.Kontakt_Technik_PSZ;
			Kosten = projectMessage.Kosten;
			Krimp_WKZ = projectMessage.Krimp_WKZ;
			Material_Eskalation_AV = projectMessage.Material_Eskalation_AV;
			Material_Eskalation_Termin = projectMessage.Material_Eskalation_Termin;
			Material_Komplett = projectMessage.Material_Komplett;
			Menge = projectMessage.Menge;
			MOQ = projectMessage.MOQ;
			Projekt_betreung = projectMessage.Projekt_betreung;
			Projekt_Start = projectMessage.Projekt_Start;
			Projektmeldung = projectMessage.Projektmeldung;
			Projekt_Nr = projectMessage.Projekt_Nr;
			Rapid_Prototyp = projectMessage.Rapid_Prototyp;
			Serie_PSZ = projectMessage.Serie_PSZ;
			SG_WKZ = projectMessage.SG_WKZ;
			Standort_Muster = projectMessage.Standort_Muster;
			Standort_Serie = projectMessage.Standort_Serie;
			Summe_Arbeitszeit = $"{(decimal.TryParse(projectMessage.Summe_Arbeitszeit, out var v) ? v.ToString("0.####") + "Std." : "")}"; // - projectMessage.Summe_Arbeitszeit;
			Termin_mit_Technik_abgesprochen = projectMessage.Termin_mit_Technik_abgesprochen;
			TSP_Kunden = projectMessage.TSP_Kunden;
			Typ = projectMessage.Typ;
			UL_Verpackung = projectMessage.UL_Verpackung;
			Wunschtermin_Kunde = projectMessage.Wunschtermin_Kunde;
			Zuschlag = projectMessage.Zuschlag;
		}
		public Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity
			{
				AB_Datum = string.IsNullOrWhiteSpace(AB_Datum) ? null : DateTime.Parse(AB_Datum.Replace(".", "/")),
				Arbeitszeit_Serien_Pro_Kabesatz = Arbeitszeit_Serien_Pro_Kabesatz,
				Artikelnummer = Artikelnummer,
				Bemerkungen = Bemerkungen,
				EAU = EAU,
				EMPB = EMPB,
				Erstanlage = string.IsNullOrWhiteSpace(Erstanlage) ? null : DateTime.Parse(Erstanlage.Replace(".", "/")),
				FA_Datum = string.IsNullOrWhiteSpace(FA_Datum) ? null : DateTime.Parse(FA_Datum.Replace(".", "/")),
				ID = ID,
				Kontakt_AV_PSZ = Kontakt_AV_PSZ,
				Kontakt_CS_PSZ = Kontakt_CS_PSZ,
				Kontakt_Technik_Kunde = Kontakt_Technik_Kunde,
				Kontakt_Technik_PSZ = Kontakt_Technik_PSZ,
				Kosten = Kosten,
				Krimp_WKZ = Krimp_WKZ,
				Material_Eskalation_AV = Material_Eskalation_AV,
				Material_Eskalation_Termin = Material_Eskalation_Termin,
				Material_Komplett = Material_Komplett,
				Menge = Menge,
				MOQ = MOQ,
				Projekt_betreung = Projekt_betreung,
				Projekt_Start = Projekt_Start,
				Projektmeldung = Projektmeldung,
				Projekt_Nr = Projekt_Nr,
				Rapid_Prototyp = Rapid_Prototyp,
				Serie_PSZ = Serie_PSZ,
				SG_WKZ = SG_WKZ,
				Standort_Muster = Standort_Muster,
				Standort_Serie = Standort_Serie,
				// Summe_Arbeitszeit = $"{(decimal.TryParse(Summe_Arbeitszeit, out var v) ? v.ToString("0.####") + "Std." : "")}", // Summe_Arbeitszeit,
				Termin_mit_Technik_abgesprochen = Termin_mit_Technik_abgesprochen,
				TSP_Kunden = TSP_Kunden,
				Typ = Typ,
				UL_Verpackung = UL_Verpackung,
				Wunschtermin_Kunde = Wunschtermin_Kunde,
				Zuschlag = Zuschlag
			};
		}
		public ProjectMessageItemModel ToModel()
		{
			return new ProjectMessageItemModel
			{
				AB_Datum = string.IsNullOrWhiteSpace(AB_Datum) ? null : DateTime.Parse(AB_Datum.Replace(".", "/")),
				Arbeitszeit_Serien_Pro_Kabesatz = Arbeitszeit_Serien_Pro_Kabesatz,
				Artikelnummer = Artikelnummer,
				Bemerkungen = Bemerkungen,
				EAU = EAU,
				EMPB = EMPB,
				Erstanlage = string.IsNullOrWhiteSpace(Erstanlage) ? null : DateTime.Parse(Erstanlage.Replace(".", "/")),
				FA_Datum = string.IsNullOrWhiteSpace(FA_Datum) ? null : DateTime.Parse(FA_Datum.Replace(".", "/")),
				ID = ID,
				Kontakt_AV_PSZ = Kontakt_AV_PSZ,
				Kontakt_CS_PSZ = Kontakt_CS_PSZ,
				Kontakt_Technik_Kunde = Kontakt_Technik_Kunde,
				Kontakt_Technik_PSZ = Kontakt_Technik_PSZ,
				Kosten = Kosten,
				Krimp_WKZ = Krimp_WKZ,
				Material_Eskalation_AV = Material_Eskalation_AV,
				Material_Eskalation_Termin = Material_Eskalation_Termin,
				Material_Komplett = Material_Komplett,
				Menge = Menge,
				MOQ = MOQ,
				Projekt_betreung = Projekt_betreung,
				Projekt_Start = Projekt_Start,
				Projektmeldung = Projektmeldung,
				Projekt_Nr = Projekt_Nr,
				Rapid_Prototyp = Rapid_Prototyp,
				Serie_PSZ = Serie_PSZ,
				SG_WKZ = SG_WKZ,
				Standort_Muster = Standort_Muster,
				Standort_Serie = Standort_Serie,
				// Summe_Arbeitszeit = $"{(decimal.TryParse(Summe_Arbeitszeit, out var v) ? v.ToString("0.####") + "Std." : "")}", // Summe_Arbeitszeit,
				Termin_mit_Technik_abgesprochen = Termin_mit_Technik_abgesprochen,
				TSP_Kunden = TSP_Kunden,
				Typ = Typ,
				UL_Verpackung = UL_Verpackung,
				Wunschtermin_Kunde = Wunschtermin_Kunde,
				Zuschlag = Zuschlag
			};
		}
	}
}
