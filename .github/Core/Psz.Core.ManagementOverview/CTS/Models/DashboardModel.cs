using Infrastructure.Data.Entities.Tables.Statistics;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Statistics.Enums;
using System;
using System.Collections.Generic;

namespace Psz.Core.ManagementOverview.CTS.Models
{
	public class DashboardRequestModel
	{
		public string CustomerName { get; set; }
		public DateTime? DateTill { get; set; } = DateTime.Today.AddDays(7 * 5);
	}
	public class DashboardTopRequestModel
	{
		public DateTime? DateTill { get; set; } = DateTime.Today.AddDays(7 * 5);
	}
	public class DashboardKwRequestModel
	{
		public string CustomerName { get; set; }
		public DateTime? DateTill { get; set; } = DateTime.Today.AddDays(7 * 5);
		public int? ArticleId { get; set; } = null;
	}

	public record DashboardRefreshResponseModel
	{
		public DateTime? RefreshDate { get; set; }
	}
	public class DashboardResponseModel
	{
		public List<CTSDashboardItemModel> Items { get; set; }

		// 
		public decimal TotalAmount { get; set; } = 0;
		public decimal ImmediatAmount { get; set; } = 0;
		public decimal ProductionAMount { get; set; } = 0;
		public decimal Results { get; set; } = 0;
		// - 
		public DateTime DateTill { get; set; }

	}

	public record InvoiceAmountResponseModel
	{
		public DateTime InvoiceDate { get; set; }
		public decimal? Amount { get; set; } = 0;
	}

	public class DashboardSummaryResponseModel
	{
		public string CustomerName { get; set; }
		public decimal TotalAmount { get; set; } = 0;
		public decimal ImmediatAmount { get; set; } = 0;
		public decimal ProductionAmount { get; set; } = 0;
		public decimal Results { get; set; } = 0;
		// - 
		public DateTime DateTill { get; set; }
		public DashboardSummaryResponseModel(Infrastructure.Data.Entities.Joins.MGO.CTSDashboardSummaryEntity entity, DateTime date, string customerName)
		{
			if(entity == null)
			{
				return;
			}

			// -
			DateTill = date;
			CustomerName = customerName;
			ImmediatAmount = entity.ImmediatAmount ?? 0;
			TotalAmount = entity.TotalAmount ?? 0;
			ProductionAmount = entity.ProductionAmount ?? 0;
			// -
			Results = (entity.ImmediatAmount ?? 0) + (entity.ProductionAmount ?? 0);
		}
		public DashboardSummaryResponseModel()
		{

		}

	}
	public class DashboardSummaryTopResponseModel
	{
		public List<DashboardSummaryGroupTopResponseModel> CustomerGroupItems { get; set; }
		// - 
		public DateTime DateTill { get; set; }
		public DateTime? DateLastRefresh { get; set; }
	}
	public class DayOffResponseModel
	{
		public short KW { get; set; }
		public int NbDaysOff { get; set; } = 0;
	}

	public class DashboardSummaryGroupTopResponseModel
	{
		public List<CTSDashboardItemModel> SuspiciousArticles { get; set; }
		public List<DashboardSummaryResponseModel> CustomersData { get; set; }

		public List<string> CustomersName { get; set; }
		public decimal TotalAmount { get; set; } = 0;
		public decimal ImmediatAmount { get; set; } = 0;
		public decimal ProductionAmount { get; set; } = 0;
		public decimal Results { get; set; } = 0;
		public string CustomerGroupClass { get; set; } = "";
		public int CustomerGroupOrder { get; set; } = 0;
		// - 
		public DateTime DateTill { get; set; }
		public DateTime? DateLastRefresh { get; set; }
	}
	public class CTSDashboardItemModel
	{
		public decimal? ABBedarf { get; set; }
		public decimal? ABGesamt { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? ImmediatAmount { get; set; }
		public int OpenFa { get; set; }
		public decimal? ProductionAmount { get; set; }
		public bool SuspiciousPrice { get; set; }
		public int Artikel_Nr { get; set; }
		public CTSDashboardItemModel(Infrastructure.Data.Entities.Joins.MGO.CTSDashboardEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// -
			ABBedarf = entity.ABBedarf;
			ABGesamt = entity.ABGesamt;
			Artikelnummer = entity.Artikelnummer;
			Bestand = entity.Bestand;
			ImmediatAmount = entity.ImmediatAmount;
			OpenFa = entity.OpenFa;
			ProductionAmount = entity.ProductionAmount;
			SuspiciousPrice = entity.SuspiciousPrice;
			Artikel_Nr = entity.Artikel_Nr;
		}
	}
	public class CTSDashboardItemKwModel
	{
		public decimal? ABBedarf { get; set; }
		public decimal? ABGesamt { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? ImmediatAmount { get; set; }
		public int OpenFa { get; set; }
		public int DYear { get; set; }
		public int DKw { get; set; }
		public decimal? ProductionAmount { get; set; }
		public int? ArticleNr { get; set; }

		public CTSDashboardItemKwModel(Infrastructure.Data.Entities.Joins.MGO.CTSDashboardKwEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// -
			ABBedarf = entity.ABBedarf;
			ABGesamt = entity.ABGesamt;
			Artikelnummer = entity.Artikelnummer;
			Bestand = entity.Bestand;
			ImmediatAmount = entity.ImmediatAmount;
			OpenFa = entity.OpenFa;
			ProductionAmount = entity.ProductionAmount;
			DYear = entity.DYear ?? 0;
			DKw = entity.DKw ?? 0;
			ArticleNr = entity.ArticleNr;
		}
	}

	#region Article VK Margin
	public class ArticleVKMarginResponseModel
	{
		public string Artikelnummer { get; set; }
		public decimal? DB_I_Mit_CU { get; set; }
		public decimal? DB_I_Ohne_CU { get; set; }
		public decimal? EK_Mit_CU { get; set; }
		public decimal? EK_ohne_CU { get; set; }
		public string Freigabestatus { get; set; }
		public decimal? Kalkulatorische_kosten { get; set; }
		public decimal? Prozent_Mit_CU { get; set; }
		public decimal? Prozent_Ohne_CU { get; set; }
		public decimal? SUM_Material_Mit_CU { get; set; }
		public decimal? SUM_Material_ohne_CU { get; set; }
		public decimal? VK_PSZ { get; set; }
		public DateTime? LastSyncDate { get; set; }
		public ArticleVKMarginResponseModel(Infrastructure.Data.Entities.Joins.MGO.CTSArticleVKMargeEntity entity)
		{
			if(entity == null)
			{
				return;
			}
			// - 
			Artikelnummer = entity.Artikelnummer;
			LastSyncDate = entity.LastSyncDate;
			DB_I_Mit_CU = entity.DB_I_Mit_CU ?? 0;
			DB_I_Ohne_CU = entity.DB_I_Ohne_CU ?? 0;
			EK_Mit_CU = entity.EK_Mit_CU ?? 0;
			EK_ohne_CU = entity.EK_ohne_CU ?? 0;
			Freigabestatus = entity.Freigabestatus;
			Kalkulatorische_kosten = entity.Kalkulatorische_kosten ?? 0;
			Prozent_Mit_CU = entity.Prozent_Mit_CU ?? 0;
			Prozent_Ohne_CU = entity.Prozent_Ohne_CU ?? 0;
			SUM_Material_Mit_CU = entity.SUM_Material_Mit_CU ?? 0;
			SUM_Material_ohne_CU = entity.SUM_Material_ohne_CU ?? 0;
			VK_PSZ = entity.VK_PSZ ?? 0;
		}
	}
	#endregion Article VK Margin

	#region FA Frozen Zone
	public class ProductionFrozenZoneResponseModel
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
		public bool? gebucht { get; set; }
		public bool? gedruckt { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public string Gewerk_Teilweise_Bemerkung { get; set; }
		public string GrundNachdruckPPS { get; set; }
		public int? HBGFAPositionId { get; set; }
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
		public DateTime? LastUpdateDate { get; set; }
		public int? Letzte_Gebuchte_Menge { get; set; }
		public bool? Loschen { get; set; }
		public string Mandant { get; set; }
		public int? Menge1 { get; set; }
		public int? Menge2 { get; set; }
		public int? Originalanzahl { get; set; }
		public string Planungsstatus { get; set; }
		public Single? Preis { get; set; }
		public bool? Prio { get; set; }
		public bool? Quick_Area { get; set; }
		public bool? ROH_umgebucht { get; set; }
		public bool? Spritzgiesserei_abgeschlossen { get; set; }
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
		public Single? Zeit { get; set; }
		public int? LastUpdateKW { get; set; }
		public int TotalCount { get; set; }
		public DateTime? SyncDate { get; set; }


		public ProductionFrozenZoneResponseModel(Infrastructure.Data.Entities.Joins.MGO.CTSProductionFrozenZoneEntity entity)
		{
			if(entity == null)
			{
				return;
			}
			// -
			Angebot_Artikel_Nr = entity.Angebot_Artikel_Nr;
			Angebot_nr = entity.Angebot_nr;
			Anzahl = entity.Anzahl;
			Anzahl_aktuell = entity.Anzahl_aktuell;
			Anzahl_erledigt = entity.Anzahl_erledigt;
			AnzahlnachgedrucktPPS = entity.AnzahlnachgedrucktPPS;
			Artikel_Nr = entity.Artikel_Nr;
			Ausgangskontrolle = entity.Ausgangskontrolle;
			Bemerkung = entity.Bemerkung;
			Bemerkung_II_Planung = entity.Bemerkung_II_Planung;
			Bemerkung_ohne_statte = entity.Bemerkung_ohne_statte;
			Bemerkung_Kommissionierung_AL = entity.Bemerkung_Kommissionierung_AL;
			Bemerkung_Planung = entity.Bemerkung_Planung;
			Bemerkung_Technik = entity.Bemerkung_Technik;
			Bemerkung_zu_Prio = entity.Bemerkung_zu_Prio;
			BomVersion = entity.BomVersion;
			CAO = entity.CAO;
			Check_FAbegonnen = entity.Check_FAbegonnen;
			Check_Gewerk1 = entity.Check_Gewerk1;
			Check_Gewerk1_Teilweise = entity.Check_Gewerk1_Teilweise;
			Check_Gewerk2 = entity.Check_Gewerk2;
			Check_Gewerk2_Teilweise = entity.Check_Gewerk2_Teilweise;
			Check_Gewerk3 = entity.Check_Gewerk3;
			Check_Gewerk3_Teilweise = entity.Check_Gewerk3_Teilweise;
			Check_Kabelgeschnitten = entity.Check_Kabelgeschnitten;
			CPVersion = entity.CPVersion;
			Datum = entity.Datum;
			Endkontrolle = entity.Endkontrolle;
			Erledigte_FA_Datum = entity.Erledigte_FA_Datum;
			Erstmuster = entity.Erstmuster;
			FA_begonnen = entity.FA_begonnen;
			FA_Druckdatum = entity.FA_Druckdatum;
			FA_Gestartet = entity.FA_Gestartet;
			Fa_NachdruckPPS = entity.Fa_NachdruckPPS;
			Fertigungsnummer = entity.Fertigungsnummer;
			gebucht = entity.gebucht;
			gedruckt = entity.gedruckt;
			Gewerk_1 = entity.Gewerk_1;
			Gewerk_2 = entity.Gewerk_2;
			Gewerk_3 = entity.Gewerk_3;
			Gewerk_Teilweise_Bemerkung = entity.Gewerk_Teilweise_Bemerkung;
			GrundNachdruckPPS = entity.GrundNachdruckPPS;
			HBGFAPositionId = entity.HBGFAPositionId;
			ID = entity.ID;
			ID_Hauptartikel = entity.ID_Hauptartikel;
			ID_Rahmenfertigung = entity.ID_Rahmenfertigung;
			Kabel_geschnitten = entity.Kabel_geschnitten;
			Kabel_geschnitten_Datum = entity.Kabel_geschnitten_Datum;
			Kabel_Schneidebeginn = entity.Kabel_Schneidebeginn;
			Kabel_Schneidebeginn_Datum = entity.Kabel_Schneidebeginn_Datum;
			Kennzeichen = entity.Kennzeichen;
			Kommisioniert_komplett = entity.Kommisioniert_komplett;
			Kommisioniert_teilweise = entity.Kommisioniert_teilweise;
			Kunden_Index_Datum = entity.Kunden_Index_Datum;
			KundenIndex = entity.KundenIndex;
			Lagerort_id = entity.Lagerort_id;
			Lagerort_id_zubuchen = entity.Lagerort_id_zubuchen;
			LastUpdateDate = entity.LastUpdateDate;
			Letzte_Gebuchte_Menge = entity.Letzte_Gebuchte_Menge;
			Loschen = entity.Loschen;
			Mandant = entity.Mandant;
			Menge1 = entity.Menge1;
			Menge2 = entity.Menge2;
			Originalanzahl = entity.Originalanzahl;
			Planungsstatus = entity.Planungsstatus;
			Preis = entity.Preis;
			Prio = entity.Prio;
			Quick_Area = entity.Quick_Area;
			ROH_umgebucht = entity.ROH_umgebucht;
			Spritzgiesserei_abgeschlossen = entity.Spritzgiesserei_abgeschlossen;
			Tage_Abweichung = entity.Tage_Abweichung;
			Technik = entity.Technik;
			Techniker = entity.Techniker;
			Termin_Bestatigt1 = entity.Termin_Bestatigt1;
			Termin_Bestatigt2 = entity.Termin_Bestatigt2;
			Termin_Fertigstellung = entity.Termin_Fertigstellung;
			Termin_Material = entity.Termin_Material;
			Termin_Ursprunglich = entity.Termin_Ursprunglich;
			Termin_voranderung = entity.Termin_voranderung;
			UBG = entity.UBG;
			UBGTransfer = entity.UBGTransfer;
			Urs_Artikelnummer = entity.Urs_Artikelnummer;
			Urs_Fa = entity.Urs_Fa;
			Zeit = entity.Zeit;
			LastUpdateKW = entity.lastUpdateKW;
			TotalCount = entity.TotalCount;
			SyncDate = entity.SyncDate;
		}
	}

	public class ProductionReschedulingRequestModel
	{
		public IPaginatedRequestModel pagination { get; set; }
		public DateTime? from { get; set; }
		public DateTime? to { get; set; }
		public int? fzKwCount { get; set; }
		public Enums.CTSDashboard productType { get; set; }
	}

	public class ProductionReschedulingResponseModel
	{
		public IPaginatedResponseModel<ProductionFrozenZoneResponseModel> Items { get; set; }
		public int KwCount { get; set; } = 5;
	}
	#endregion FA Frozen Zone

	#region Analyse Bedarf
	public record RefreshDashboardRequestModel
	{
		public StatisticType statisticType { get; set; }
	}
	public record RefreshDashboardResponseModel
	{
	}

	public class BedarfBestandRequestModel: IPaginatedRequestModel
	{
		public DateTime? DateTill { get; set; }
		public ProductType? ProductType { get; set; }
		public bool isPaginated { get; set; }
	}
	public record DashboardClass
	{
		public string ClassName { get; set; }
		public string ClassValue { get; set; }

	}
	public record BedarfBestandItemClass: DashboardClass
	{
		public List<BedarfBestandItemModel> Items { get; set; }

	}
	public class BedarfBestandResponseModel
	{
		public List<BedarfBestandItemClass> ItemsExtraROH { get; set; }
		public List<BedarfBestandItemClass> ItemsMissingROH { get; set; }
		public DateTime? LastSyncDate { get; set; }
		public DateTime DateTill { get; set; }
	}
	public class BedarfBestandItemModel
	{
		public string Artikelnummer { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal? DiffPrice { get; set; }
		public decimal? DiffQuantity { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public string Name1 { get; set; }
		public decimal? ROH_Bestand { get; set; }
		public decimal? ROH_Quantity { get; set; }
		public decimal? Wert_LagerBestandBedarf { get; set; }
		public DateTime? LastSyncDate { get; set; }
		public int TotalCount { get; set; }
		public BedarfBestandItemModel(BedarfAnalyseEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			Artikelnummer = entity.Artikelnummer;
			Bestell_Nr = entity.Bestell_Nr;
			DiffPrice = entity.DiffPrice;
			DiffQuantity = entity.DiffQuantity;
			Einkaufspreis = entity.Einkaufspreis;
			Gesamtpreis = entity.Gesamtpreis;
			Name1 = entity.Name1;
			ROH_Bestand = entity.ROH_Bestand;
			ROH_Quantity = entity.ROH_Quantity;
			Wert_LagerBestandBedarf = entity.Wert_LagerBestandBedarf;
			LastSyncDate = entity.LastSyncDate;
			TotalCount = entity.TotalCount ?? 0;
		}
	}
	#endregion Analyse Bedarf
}
