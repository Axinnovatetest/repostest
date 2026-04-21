using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Models.Article.Cts
{
	public class ArticleCtsModel
	{
		public int Id { get; set; }
		public int ArticleId { get; set; }
		public bool? AlternativeProductionPlace { get; set; }
		public int? ProductionPlace1_Id { get; set; }
		public int? ProductionPlace2_Id { get; set; }
		public int? ProductionPlace3_Id { get; set; }
		public string ProductionPlace1_Name { get; set; }
		public string ProductionPlace2_Name { get; set; }
		public string ProductionPlace3_Name { get; set; }
		public string ExternalStatus { get; set; }
		public string Prufstatus { get; set; }
		public string InternalStatus { get; set; }
		// -
		public decimal? CuGewicht { get; set; }//
		public decimal? Gewicht { get; set; } // MGK
		public decimal? Grosse { get; set; } // - Ber. Gewicht
		public int? Kupferbasis { get; set; }
		public decimal? CopperCostBasis { get; set; }//
		public decimal? CopperCostBasis150 { get; set; }// Kupferbasis { get; set; }//

		public string Langtext { get; set; }
		public string Verpackung { get; set; }
		public bool aktiv { get; set; }
		public string BemerkungCRP { get; set; }
		public string BemerkungCRPPlanung { get; set; }

		public ArticleCtsModel()
		{

		}
		public ArticleCtsModel(
			Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity productionExtensionEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity,
			List<KeyValuePair<int, string>> productionPlaces,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity artikelExtensionEntity)
		{
			if(productionExtensionEntity != null)
			{
				Id = productionExtensionEntity.Id;
				ProductionPlace1_Id = productionExtensionEntity.ProductionPlace1_Id;
				ProductionPlace2_Id = productionExtensionEntity.ProductionPlace2_Id;
				ProductionPlace3_Id = productionExtensionEntity.ProductionPlace3_Id;

				ProductionPlace1_Name = productionPlaces.FirstOrDefault(x => x.Key == (productionExtensionEntity.ProductionPlace1_Id ?? -1)).Value; // productionExtensionEntity.ProductionPlace1_Name;
				ProductionPlace2_Name = productionPlaces.FirstOrDefault(x => x.Key == (productionExtensionEntity.ProductionPlace2_Id ?? -1)).Value; // productionExtensionEntity.ProductionPlace2_Name;
				ProductionPlace3_Name = productionPlaces.FirstOrDefault(x => x.Key == (productionExtensionEntity.ProductionPlace3_Id ?? -1)).Value; // productionExtensionEntity.ProductionPlace3_Name;

				AlternativeProductionPlace = productionExtensionEntity.AlternativeProductionPlace;
			}
			if(articleEntity != null)
			{
				ArticleId = articleEntity.ArtikelNr;
				ExternalStatus = articleEntity.Freigabestatus;
				Prufstatus = articleEntity.PrufstatusTNWare;
				InternalStatus = articleEntity.FreigabestatusTNIntern;
				// -
				CuGewicht = articleEntity.CuGewicht;
				Gewicht = articleEntity.Gewicht; // MGK
				Grosse = articleEntity.Größe; // - Ber. Gewicht
				Kupferbasis = articleEntity.Kupferbasis;//

				Langtext = articleEntity.Langtext;
				Verpackung = articleEntity.Verpackung;
				aktiv = articleEntity.aktiv ?? false;
				BemerkungCRP = (articleEntity.BemerkungCRP == null) ? "" : articleEntity.BemerkungCRP;
				BemerkungCRPPlanung = (articleEntity.BemerkungCRPPlanung == null) ? "" : articleEntity.BemerkungCRPPlanung;
			}

			if(artikelExtensionEntity != null)
			{
				CopperCostBasis = artikelExtensionEntity.CopperCostBasis;//
				CopperCostBasis150 = artikelExtensionEntity.CopperCostBasis150;//}
			}
		}
	}

	public class ArticleOpenFA
	{
		public int? Angebot_Artikel_Nr { get; set; }
		public int? Angebot_nr { get; set; }
		public int? Anzahl { get; set; }
		public int? Anzahl_aktuell { get; set; }
		public int? Anzahl_erledigt { get; set; }
		public int AnzahlnachgedrucktPPS { get; set; }
		public int? Artikel_Nr { get; set; }
		public bool? Ausgangskontrolle { get; set; }
		public string Bemerkung { get; set; }
		public string Bemerkung_II_Planung { get; set; }
		public string Bemerkung_ohne_statte { get; set; }
		public string Bemerkung_Kommissionierung_AL { get; set; }
		public string Bemerkung_Planung { get; set; }
		public string Bemerkung_Technik { get; set; }
		public string Bemerkung_zu_Prio { get; set; }
		public int? BomVersion { get; set; }
		public bool? CAO { get; set; }
		public bool? Check_FAbegonnen { get; set; }
		public bool? Check_Gewerk1 { get; set; }
		public bool? Check_Gewerk1_Teilweise { get; set; }
		public bool? Check_Gewerk2 { get; set; }
		public bool? Check_Gewerk2_Teilweise { get; set; }
		public bool? Check_Gewerk3 { get; set; }
		public bool? Check_Gewerk3_Teilweise { get; set; }
		public bool? Check_Kabelgeschnitten { get; set; }
		public int? CPVersion { get; set; }
		public DateTime? Datum { get; set; }
		public bool? Endkontrolle { get; set; }
		public DateTime? Erledigte_FA_Datum { get; set; }
		public bool? Erstmuster { get; set; }
		public DateTime? FA_begonnen { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public bool? FA_Gestartet { get; set; }
		public bool? Fa_NachdruckPPS { get; set; }
		public int? Fertigungsnummer { get; set; }
		public bool? Gebucht { get; set; }
		public bool? Gedruckt { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public string Gewerk_Teilweise_Bemerkung { get; set; }
		public string GrundNachdruckPPS { get; set; }
		public int ID { get; set; }
		public int? ID_Hauptartikel { get; set; }
		public int? ID_Rahmenfertigung { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public DateTime? Kabel_geschnitten_Datum { get; set; }
		public bool? Kabel_Schneidebeginn { get; set; }
		public DateTime? Kabel_Schneidebeginn_Datum { get; set; }
		public string Kennzeichen { get; set; }
		public bool? Kommisioniert_komplett { get; set; }
		public bool? Kommisioniert_teilweise { get; set; }
		public DateTime? Kunden_Index_Datum { get; set; }
		public string KundenIndex { get; set; }
		public int? Lagerort_id { get; set; }
		public int? Lagerort_id_zubuchen { get; set; }
		public int? Letzte_Gebuchte_Menge { get; set; }
		public bool? Loschen { get; set; }
		public string Mandant { get; set; }
		public int? Menge1 { get; set; }
		public int? Menge2 { get; set; }
		public int? Originalanzahl { get; set; }
		public string Planungsstatus { get; set; }
		public decimal? Preis { get; set; }
		public bool? Prio { get; set; }
		public bool? Quick_Area { get; set; }
		public bool? ROH_umgebucht { get; set; }
		public bool? SpritzgieBerei_abgeschlossen { get; set; }
		public int? Tage_Abweichung { get; set; }
		public bool? Technik { get; set; }
		public string Techniker { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public DateTime? Termin_Bestatigt2 { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public DateTime? Termin_Material { get; set; }
		public DateTime? Termin_Ursprunglich { get; set; }
		public DateTime? Termin_voranderung { get; set; }
		public bool? UBG { get; set; }
		public bool? UBGTransfer { get; set; }
		public string Urs_Artikelnummer { get; set; }
		public string Urs_Fa { get; set; }
		public decimal? Zeit { get; set; }

		public ArticleOpenFA(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity fertigungEntity)
		{
			if(fertigungEntity == null)
				return;

			// -
			Datum = fertigungEntity.Datum; // Date
			Erledigte_FA_Datum = fertigungEntity.Erledigte_FA_Datum; // Date completed
			FA_begonnen = fertigungEntity.FA_begonnen; // Started date
			FA_Druckdatum = fertigungEntity.FA_Druckdatum; // Print date
			Kabel_geschnitten_Datum = fertigungEntity.Kabel_geschnitten_Datum; // Cable cut date
			Kabel_Schneidebeginn_Datum = fertigungEntity.Kabel_Schneidebeginn_Datum; // Cable cut start date
			Termin_Bestatigt1 = fertigungEntity.Termin_Bestatigt1; // Appointment confirmed 1 date
			Termin_Bestatigt2 = fertigungEntity.Termin_Bestatigt2; // Appointment confirmed 2 date
			Termin_Fertigstellung = fertigungEntity.Termin_Fertigstellung; // Deadline completion date
			Termin_Material = fertigungEntity.Termin_Material; // Appointment material date
			Termin_Ursprunglich = fertigungEntity.Termin_Ursprunglich; // Date Original Date,
			Termin_voranderung = fertigungEntity.Termin_voranderung; // Appointment advance date
			Angebot_Artikel_Nr = fertigungEntity.Angebot_Artikel_Nr;
			Angebot_nr = fertigungEntity.Angebot_nr;
			Anzahl = fertigungEntity.Anzahl;
			Anzahl_aktuell = fertigungEntity.Anzahl_aktuell;
			Anzahl_erledigt = fertigungEntity.Anzahl_erledigt;
			AnzahlnachgedrucktPPS = fertigungEntity.AnzahlnachgedrucktPPS;
			Artikel_Nr = fertigungEntity.Artikel_Nr;
			Ausgangskontrolle = fertigungEntity.Ausgangskontrolle;
			Bemerkung = fertigungEntity.Bemerkung;
			Bemerkung_II_Planung = fertigungEntity.Bemerkung_II_Planung;
			Bemerkung_ohne_statte = fertigungEntity.Bemerkung_ohne_statte;
			Bemerkung_Planung = fertigungEntity.Bemerkung_Planung;
			Bemerkung_Technik = fertigungEntity.Bemerkung_Technik;
			Bemerkung_zu_Prio = fertigungEntity.Bemerkung_zu_Prio;
			Bemerkung_Kommissionierung_AL = fertigungEntity.Bemerkung_Kommissionierung_AL;
			Check_FAbegonnen = fertigungEntity.Check_FAbegonnen;
			Check_Gewerk1 = fertigungEntity.Check_Gewerk1;
			Check_Gewerk1_Teilweise = fertigungEntity.Check_Gewerk1_Teilweise;
			Check_Gewerk2 = fertigungEntity.Check_Gewerk2;
			Check_Gewerk2_Teilweise = fertigungEntity.Check_Gewerk2_Teilweise;
			Check_Gewerk3 = fertigungEntity.Check_Gewerk3;
			Check_Gewerk3_Teilweise = fertigungEntity.Check_Gewerk3_Teilweise;
			Check_Kabelgeschnitten = fertigungEntity.Check_Kabelgeschnitten;
			Endkontrolle = fertigungEntity.Endkontrolle;
			Erstmuster = fertigungEntity.Erstmuster;
			FA_Gestartet = fertigungEntity.FA_Gestartet;
			Fa_NachdruckPPS = fertigungEntity.Fa_NachdruckPPS;
			Fertigungsnummer = fertigungEntity.Fertigungsnummer;
			Gebucht = fertigungEntity.Gebucht;
			Gedruckt = fertigungEntity.Gedruckt;
			Gewerk_1 = fertigungEntity.Gewerk_1;
			Gewerk_2 = fertigungEntity.Gewerk_2;
			Gewerk_3 = fertigungEntity.Gewerk_3;
			Gewerk_Teilweise_Bemerkung = fertigungEntity.Gewerk_Teilweise_Bemerkung;
			GrundNachdruckPPS = fertigungEntity.GrundNachdruckPPS;
			ID = fertigungEntity.ID;
			ID_Hauptartikel = fertigungEntity.ID_Hauptartikel;
			ID_Rahmenfertigung = fertigungEntity.ID_Rahmenfertigung;
			Kabel_geschnitten = fertigungEntity.Kabel_geschnitten;
			Kabel_Schneidebeginn = fertigungEntity.Kabel_Schneidebeginn;
			Kennzeichen = fertigungEntity.Kennzeichen;
			Kommisioniert_komplett = fertigungEntity.Kommisioniert_komplett;
			Kommisioniert_teilweise = fertigungEntity.Kommisioniert_teilweise;
			KundenIndex = fertigungEntity.KundenIndex;
			Kunden_Index_Datum = fertigungEntity.Kunden_Index_Datum;
			Lagerort_id = fertigungEntity.Lagerort_id;
			Lagerort_id_zubuchen = fertigungEntity.Lagerort_id_zubuchen;
			Letzte_Gebuchte_Menge = fertigungEntity.Letzte_Gebuchte_Menge;
			Loschen = fertigungEntity.Löschen;
			Mandant = fertigungEntity.Mandant;
			Menge1 = fertigungEntity.Menge1;
			Menge2 = fertigungEntity.Menge2;
			Originalanzahl = fertigungEntity.Originalanzahl;
			Planungsstatus = fertigungEntity.Planungsstatus;
			Preis = fertigungEntity.Preis;
			Prio = fertigungEntity.Prio;
			Quick_Area = fertigungEntity.Quick_Area;
			ROH_umgebucht = fertigungEntity.ROH_umgebucht;
			SpritzgieBerei_abgeschlossen = fertigungEntity.SpritzgieBerei_abgeschlossen;
			Tage_Abweichung = fertigungEntity.Tage_Abweichung;
			Technik = fertigungEntity.Technik;
			Techniker = fertigungEntity.Techniker;
			UBG = fertigungEntity.UBG;
			UBGTransfer = fertigungEntity.UBGTransfer;
			Urs_Artikelnummer = fertigungEntity.Urs_Artikelnummer;
			Urs_Fa = fertigungEntity.Urs_Fa;
			Zeit = fertigungEntity.Zeit;
			CPVersion = fertigungEntity.CPVersion;
			BomVersion = fertigungEntity.BomVersion;
		}
	}
	public class ArticleCtsUpdateRequestModel
	{
		public int ArticleId { get; set; }
		public string Langtext { get; set; }
		public string Verpackung { get; set; }
		public string BemerkungCRP { get; set; }
		public string BemerkungCRPPlanung { get; set; }

		// - 
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				ArtikelNr = ArticleId,
				Langtext = Langtext,
				Verpackung = Verpackung,
				BemerkungCRP = (BemerkungCRP == null) ? "" : BemerkungCRP,
				BemerkungCRPPlanung = (BemerkungCRPPlanung == null) ? "" : BemerkungCRPPlanung,
			};
		}
	}
}
