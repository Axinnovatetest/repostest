using Infrastructure.Data.Entities.Tables.BSD;
using System;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleModel
	{
		public int ArticleImageId { get; set; }
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public string Abladestelle { get; set; }
		public bool aktiv { get; set; }
		public Nullable<DateTime> aktualisiert { get; set; }
		public Nullable<decimal> Anfangsbestand { get; set; }
		public Nullable<bool> ArtikelAusEigenerProduktion { get; set; }
		public Nullable<bool> ArtikelFürWeitereBestellungenSperren { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public string Artikelfamilie_Kunde_Detail1 { get; set; }
		public string Artikelfamilie_Kunde_Detail2 { get; set; }
		public string Artikelkurztext { get; set; }
		public Nullable<bool> Barverkauf { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Bezeichnung2 { get; set; }
		public string Bezeichnung3 { get; set; }
		public string BezeichnungAL { get; set; }
		public Nullable<bool> COF_Pflichtig { get; set; }
		public string Crossreferenz { get; set; }
		public Nullable<decimal> CuGewicht { get; set; }
		public Nullable<DateTime> DatumAnfangsbestand { get; set; }
		public Nullable<int> DEL { get; set; }
		public Nullable<bool> DELFixiert { get; set; }
		public string Dokumente { get; set; }
		public string EAN { get; set; }
		public string Einheit { get; set; }
		public Nullable<bool> EMPB { get; set; }
		public Nullable<bool> EMPB_Freigegeben { get; set; }
		public Nullable<int> Ersatzartikel { get; set; }
		public Nullable<bool> ESD_Schutz { get; set; }
		public Nullable<decimal> Exportgewicht { get; set; }
		public Nullable<bool> fakturierenStückliste { get; set; }
		public string Farbe { get; set; }
		public Nullable<int> fibu_rahmen { get; set; }
		public string Freigabestatus { get; set; }
		public string FreigabestatusTNIntern { get; set; }
		public string Gebinde { get; set; }
		public Nullable<decimal> Gewicht { get; set; }
		public Nullable<decimal> Größe { get; set; }
		public string GrundFürSperre { get; set; }
		public Nullable<DateTime> gültigBis { get; set; }
		public string Halle { get; set; }
		public Nullable<bool> Hubmastleitungen { get; set; }
		public Nullable<int> ID_Klassifizierung { get; set; }
		public string Index_Kunde { get; set; }
		public Nullable<DateTime> Index_Kunde_Datum { get; set; }
		public string Info_WE { get; set; }
		public Nullable<bool> Kanban { get; set; }
		public string Kategorie { get; set; }
		public string Klassifizierung { get; set; }
		public string Kriterium1 { get; set; }
		public string Kriterium2 { get; set; }
		public string Kriterium3 { get; set; }
		public string Kriterium4 { get; set; }
		public Nullable<int> Kupferbasis { get; set; }
		public Nullable<decimal> Kupferzahl { get; set; }
		public Nullable<bool> Lagerartikel { get; set; }
		public Nullable<decimal> Lagerhaltungskosten { get; set; }
		public string Langtext { get; set; }
		public Nullable<bool> Langtext_drucken_AB { get; set; }
		public Nullable<bool> Langtext_drucken_BW { get; set; }
		public string Lieferzeit { get; set; }
		public Nullable<int> Losgroesse { get; set; }
		public Nullable<decimal> Materialkosten_Alt { get; set; }
		public Nullable<bool> MHD { get; set; }
		public Nullable<bool> MineralsConfirmity { get; set; }
		public string Praeferenz_Aktuelles_jahr { get; set; }
		public string Praeferenz_Folgejahr { get; set; }
		public Nullable<decimal> Preiseinheit { get; set; }
		public string proZeiteinheit { get; set; }
		public Nullable<decimal> Produktionszeit { get; set; }
		public Nullable<bool> Provisionsartikel { get; set; }
		public string PrufstatusTNWare { get; set; }
		public Nullable<bool> Rabattierfähig { get; set; }
		public Nullable<bool> Rahmen { get; set; }
		public Nullable<bool> Rahmen2 { get; set; }
		public Nullable<DateTime> Rahmenauslauf { get; set; }
		public Nullable<DateTime> Rahmenauslauf2 { get; set; }
		public Nullable<decimal> Rahmenmenge { get; set; }
		public Nullable<decimal> Rahmenmenge2 { get; set; }
		public string RahmenNr { get; set; }
		public string RahmenNr2 { get; set; }
		public Nullable<bool> REACHSVHCConfirmity { get; set; }
		public Nullable<bool> ROHSEEEConfirmity { get; set; }
		public string Seriennummer { get; set; }
		public Nullable<bool> Seriennummernverwaltung { get; set; }
		public Nullable<decimal> Sonderrabatt { get; set; }
		public Nullable<int> Standard_Lagerort_id { get; set; }
		public Nullable<bool> Stückliste { get; set; }
		// https://stackoverflow.com/questions/582797/should-you-choose-the-money-or-decimalx-y-datatypes-in-sql-server
		public Nullable<decimal> Stundensatz { get; set; }
		public string Sysmonummer { get; set; }
		public Nullable<bool> ULEtikett { get; set; }
		public Nullable<bool> ULzertifiziert { get; set; }
		public Nullable<decimal> Umsatzsteuer { get; set; }
		public string Ursprungsland { get; set; }
		public string Verpackung { get; set; }
		public string Verpackungsart { get; set; }
		public Nullable<int> Verpackungsmenge { get; set; }
		public Nullable<bool> VKFestpreis { get; set; }
		public string Volumen { get; set; }
		public string Warengruppe { get; set; }
		public Nullable<int> Warentyp { get; set; }
		public Nullable<bool> Webshop { get; set; }
		public string Werkzeug { get; set; }
		public Nullable<decimal> Wert_Anfangsbestand { get; set; }
		public string Zeichnungsnummer { get; set; }
		public string Zeitraum_MHD { get; set; }
		public string Zolltarif_nr { get; set; }


		// >>>>> Data Extension
		public int? ProjectTypeId { get; set; }
		public string OrderNumber { get; set; }
		public string CustomerInquiryNumber { get; set; }
		public decimal? QuotationsBased12Months { get; set; }
		public DateTime? SOPAppointmentCustomer { get; set; }
		public string Consumption12Months { get; set; }
		public string Sales12MonthsPerItem { get; set; }
		public string CreatorOrder { get; set; }
		public DateTime? OrderValidity { get; set; }
		public int? CustomerContactPersonId { get; set; }
		public decimal? CopperCostBasis { get; set; }
		public decimal? CopperCostBasis150 { get; set; }

		// >>>>> Quality extension
		public int? CoC_Pflichtig_AttachmentId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public int? Dienstleistung_AttachmentId { get; set; }
		public int? EMPB_AttachmentId { get; set; }
		public int? EMPB_Freigegeben_AttachmentId { get; set; }
		public int? ESD_AttachmentId { get; set; }
		public int? HM_AttachmentId { get; set; }
		public int Id { get; set; }
		public int? LLE_AttachmentId { get; set; }
		public int? MHD_AttachmentId { get; set; }
		public int? MineralsConfirmity_AttachmentId { get; set; }
		public bool? PackagingRegulation_Available { get; set; }
		public int? PackagingRegulation_Available_AttachmentId { get; set; }
		public bool? PurchasingArticleInspection__SpecialArticlesCustomerSpecific { get; set; }
		public int? PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId { get; set; }
		public bool? QSV { get; set; }
		public int? QSV_AttachmentId { get; set; }
		public int? REACH_SVHC_Confirmity_AttachmentId { get; set; }
		public int? ROHS_EEE_Confirmity_AttachmentId { get; set; }
		public bool? SpecialCustomerReleases__DeviationReleases { get; set; }
		public int? SpecialCustomerReleases__DeviationReleases_AttachmentId { get; set; }
		public bool? TSP_Available { get; set; }
		public int? TSP_Available_AttachmentId { get; set; }
		public int? UL_Etikett_AttachmentId { get; set; }
		public int? UL_zertifiziert_AttachmentId { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }

		// >>>>>>> Logistics Extension
		#region Logistics Extension
		public DateTime? Anlieferadresse_Abladeort { get; set; }
		public int ArticleId { get; set; }
		public bool? VDALabel { get; set; } // VDA_E / VDA_1
		public bool? ULLabel { get; set; }
		public string ZolltarifNummer { get; set; }
		public Decimal? Exportgewicht______ { get; set; } // <<<<<<< Exists in decimal!
		public int? Ursprungsland_Id { get; set; }
		public string Ursprungsland_name { get; set; }
		public bool? VDA_P { get; set; } // VDA_2
		public decimal? Grosse { get; set; }
		#endregion Logistics Extension

		// >>>>>>>> Production Extension
		public int? ProductionPlace1_Id { get; set; }
		public int? ProductionPlace2_Id { get; set; }
		public int? ProductionPlace3_Id { get; set; }
		public string ProductionPlace1_Name { get; set; }
		public string ProductionPlace2_Name { get; set; }
		public string ProductionPlace3_Name { get; set; }
		public bool? AlternativeProductionPlace { get; set; }

		// - 2022-08-13 - New Nomm.
		public int CustomerNumber { get; set; }
		public string CustomerPrefix { get; set; }
		public string CustomerItemNumber { get; set; }
		public int? CustomerItemNumberSequence { get; set; }
		public string CustomerItemIndex { get; set; }
		public int? CustomerItemIndexSequence { get; set; }
		public string ProductionCountryCode { get; set; }
		public string ProductionCountryName { get; set; }
		public string ProductionSiteCode { get; set; }
		public string ProductionSiteName { get; set; }
		public string ArticleNumber { get; set; }
		// - 2022-09-05
		public bool UBG { get; set; }
		// - 2023-01-20 
		public bool EdiDefault { get; set; }
		public bool IsArticleNumberSpecial { get; set; }
		// - 2024-02-28 - Capital // E-Drawing
		public bool IsEDrawing { get; set; }

		public string NextNr { get; set; }
		public string PreviousNr { get; set; }
		public int NextArtikelNr { get; set; }
		public int PreviousArtikelNr { get; set; }
		//>>>>>00024 PM - FG1(back)
		public string Artikelbezeichnung { get; set; }

		public ArticleModel() { }
		public ArticleModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			if(artikelEntity == null)
				return;

			Id = artikelEntity.ArtikelNr;
			ArtikelNr = artikelEntity.ArtikelNr;
			ArtikelNummer = artikelEntity.ArtikelNummer;

			Abladestelle = artikelEntity.Abladestelle;
			aktiv = artikelEntity.aktiv ?? false;
			aktualisiert = artikelEntity.aktualisiert;
			Anfangsbestand = artikelEntity.Anfangsbestand;
			ArtikelAusEigenerProduktion = artikelEntity.ArtikelAusEigenerProduktion;
			ArtikelFürWeitereBestellungenSperren = artikelEntity.ArtikelFürWeitereBestellungenSperren;
			Artikelfamilie_Kunde = artikelEntity.Artikelfamilie_Kunde;
			Artikelfamilie_Kunde_Detail1 = artikelEntity.Artikelfamilie_Kunde_Detail1;
			Artikelfamilie_Kunde_Detail2 = artikelEntity.Artikelfamilie_Kunde_Detail2;
			Artikelkurztext = artikelEntity.Artikelkurztext;
			Barverkauf = artikelEntity.Barverkauf;
			Bezeichnung1 = artikelEntity.Bezeichnung1;
			Bezeichnung2 = artikelEntity.Bezeichnung2;
			Bezeichnung3 = artikelEntity.Bezeichnung3;
			BezeichnungAL = artikelEntity.BezeichnungAL;
			COF_Pflichtig = artikelEntity.COF_Pflichtig;
			Crossreferenz = artikelEntity.Crossreferenz;
			CuGewicht = artikelEntity.CuGewicht;
			DatumAnfangsbestand = artikelEntity.DatumAnfangsbestand;
			DEL = artikelEntity.DEL;
			DELFixiert = artikelEntity.DELFixiert;
			Dokumente = artikelEntity.Dokumente;
			EAN = artikelEntity.EAN;
			Einheit = artikelEntity.Einheit;
			EMPB = artikelEntity.EMPB;
			EMPB_Freigegeben = artikelEntity.EMPB_Freigegeben;
			Ersatzartikel = artikelEntity.Ersatzartikel;
			ESD_Schutz = artikelEntity.ESD_Schutz;
			Exportgewicht = artikelEntity.Exportgewicht;
			fakturierenStückliste = artikelEntity.fakturierenStückliste;
			Farbe = artikelEntity.Farbe;
			fibu_rahmen = artikelEntity.fibu_rahmen;
			Freigabestatus = artikelEntity.Freigabestatus;
			FreigabestatusTNIntern = artikelEntity.FreigabestatusTNIntern;
			Gebinde = artikelEntity.Gebinde;
			Gewicht = artikelEntity.Gewicht;
			Größe = artikelEntity.Größe;
			GrundFürSperre = artikelEntity.GrundFürSperre;
			gültigBis = artikelEntity.gültigBis;
			Halle = artikelEntity.Halle;
			Hubmastleitungen = artikelEntity.Hubmastleitungen;
			ID_Klassifizierung = artikelEntity.ID_Klassifizierung;
			Index_Kunde = artikelEntity.Index_Kunde;
			Index_Kunde_Datum = artikelEntity.Index_Kunde_Datum;
			Info_WE = artikelEntity.Info_WE;
			Kanban = artikelEntity.Kanban;
			Kategorie = artikelEntity.Kategorie;
			Klassifizierung = artikelEntity.Klassifizierung;
			Kriterium1 = artikelEntity.Kriterium1;
			Kriterium2 = artikelEntity.Kriterium2;
			Kriterium3 = artikelEntity.Kriterium3;
			Kriterium4 = artikelEntity.Kriterium4;
			Kupferbasis = artikelEntity.Kupferbasis;
			Kupferzahl = artikelEntity.Kupferzahl;
			Lagerartikel = artikelEntity.Lagerartikel;
			Lagerhaltungskosten = artikelEntity.Lagerhaltungskosten;
			Langtext = artikelEntity.Langtext;
			Langtext_drucken_AB = artikelEntity.Langtext_drucken_AB;
			Langtext_drucken_BW = artikelEntity.Langtext_drucken_BW;
			Lieferzeit = artikelEntity.Lieferzeit;
			Losgroesse = artikelEntity.Losgroesse;
			Materialkosten_Alt = artikelEntity.Materialkosten_Alt;
			MHD = artikelEntity.MHD;
			MineralsConfirmity = artikelEntity.MineralsConfirmity;
			Praeferenz_Aktuelles_jahr = artikelEntity.Praeferenz_Aktuelles_jahr;
			Praeferenz_Folgejahr = artikelEntity.Praeferenz_Folgejahr;
			Preiseinheit = artikelEntity.Preiseinheit;
			proZeiteinheit = artikelEntity.proZeiteinheit;
			Produktionszeit = artikelEntity.Produktionszeit;
			Provisionsartikel = artikelEntity.Provisionsartikel;
			PrufstatusTNWare = artikelEntity.PrufstatusTNWare;
			Rabattierfähig = artikelEntity.Rabattierfähig;
			Rahmen = artikelEntity.Rahmen;
			Rahmen2 = artikelEntity.Rahmen2;
			Rahmenauslauf = artikelEntity.Rahmenauslauf;
			Rahmenauslauf2 = artikelEntity.Rahmenauslauf2;
			Rahmenmenge = artikelEntity.Rahmenmenge;
			Rahmenmenge2 = artikelEntity.Rahmenmenge2;
			RahmenNr = artikelEntity.RahmenNr;
			RahmenNr2 = artikelEntity.RahmenNr2;
			REACHSVHCConfirmity = artikelEntity.REACHSVHCConfirmity;
			ROHSEEEConfirmity = artikelEntity.ROHSEEEConfirmity;
			Seriennummer = artikelEntity.Seriennummer;
			Seriennummernverwaltung = artikelEntity.Seriennummernverwaltung;
			Sonderrabatt = artikelEntity.Sonderrabatt;
			Standard_Lagerort_id = artikelEntity.Standard_Lagerort_id;
			Stückliste = artikelEntity.Stuckliste;
			Stundensatz = artikelEntity.Stundensatz;
			Sysmonummer = artikelEntity.Sysmonummer;
			ULEtikett = artikelEntity.ULEtikett;
			ULzertifiziert = artikelEntity.ULzertifiziert;
			Umsatzsteuer = artikelEntity.Umsatzsteuer;
			Ursprungsland = artikelEntity.Ursprungsland;
			Verpackung = artikelEntity.Verpackung;
			Verpackungsart = artikelEntity.Verpackungsart;
			Verpackungsmenge = artikelEntity.Verpackungsmenge;
			VKFestpreis = artikelEntity.VKFestpreis;
			Volumen = artikelEntity.Volumen;
			Warengruppe = artikelEntity.Warengruppe;
			Warentyp = artikelEntity.Warentyp;
			Webshop = artikelEntity.Webshop;
			Werkzeug = artikelEntity.Werkzeug;
			Wert_Anfangsbestand = artikelEntity.Wert_Anfangsbestand;
			Zeichnungsnummer = artikelEntity.Zeichnungsnummer;
			Zeitraum_MHD = artikelEntity.Zeitraum_MHD;
			Zolltarif_nr = artikelEntity.Zolltarif_nr;
			aktiv = artikelEntity.aktiv ?? false;


			// - 2022-08-13 - New Nomm.
			CustomerNumber = artikelEntity.CustomerNumber ?? -1;
			CustomerPrefix = artikelEntity.CustomerPrefix;
			CustomerItemNumber = artikelEntity.CustomerItemNumber;
			CustomerItemNumberSequence = artikelEntity.CustomerItemNumberSequence;
			CustomerItemIndex = artikelEntity.CustomerIndex;
			CustomerItemIndexSequence = artikelEntity.CustomerIndexSequence;
			ProductionCountryCode = artikelEntity.ProductionCountryCode;
			ProductionCountryName = artikelEntity.ProductionCountryName;
			ProductionSiteCode = artikelEntity.ProductionSiteCode;
			ProductionSiteName = artikelEntity.ProductionSiteName;
			ArticleNumber = artikelEntity.ArticleNumber;

			// - 2022-09-005
			UBG = artikelEntity.UBG;

			// - 2023-01-20
			EdiDefault = artikelEntity.EdiDefault ?? false;
			IsArticleNumberSpecial = artikelEntity.IsArticleNumberSpecial ?? false;
			IsEDrawing = artikelEntity.IsEDrawing ?? false;
		}
		public ArticleModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity artikelExtensionEntity) : this(artikelEntity)
		{
			if(artikelEntity == null)
				return;

			if(artikelExtensionEntity != null)
			{
				ArticleImageId = artikelExtensionEntity.ImageId;
				ProjectTypeId = artikelExtensionEntity.ProjectTypeId;
				OrderNumber = artikelExtensionEntity.OrderNumber;
				CustomerInquiryNumber = artikelExtensionEntity.CustomerInquiryNumber;
				QuotationsBased12Months = artikelExtensionEntity.QuotationsBased12Months;
				SOPAppointmentCustomer = artikelExtensionEntity.SOPAppointmentCustomer;
				Consumption12Months = artikelExtensionEntity.Consumption12Months;
				Sales12MonthsPerItem = artikelExtensionEntity.Sales12MonthsPerItem;
				CreatorOrder = artikelExtensionEntity.CreatorOrder;
				CustomerContactPersonId = artikelExtensionEntity.CustomerContactPersonId;
				CopperCostBasis = artikelExtensionEntity.CopperCostBasis;
				CopperCostBasis150 = artikelExtensionEntity.CopperCostBasis150;
			}

		}
		public ArticleModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity artikelExtensionEntity,
			Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity qualityExtensionEntity)
			: this(artikelEntity, artikelExtensionEntity)
		{
			if(artikelEntity == null)
				return;

			if(qualityExtensionEntity != null)
			{
				CoC_Pflichtig_AttachmentId = qualityExtensionEntity.CoC_Pflichtig_AttachmentId;
				CreateTime = qualityExtensionEntity.CreateTime;
				CreateUserId = qualityExtensionEntity.CreateUserId;
				Dienstleistung_AttachmentId = qualityExtensionEntity.Dienstleistung_AttachmentId;
				EMPB_AttachmentId = qualityExtensionEntity.EMPB_AttachmentId;
				EMPB_Freigegeben_AttachmentId = qualityExtensionEntity.EMPB_Freigegeben_AttachmentId;
				ESD_AttachmentId = qualityExtensionEntity.ESD_AttachmentId;
				HM_AttachmentId = qualityExtensionEntity.HM_AttachmentId;
				LLE_AttachmentId = qualityExtensionEntity.LLE_AttachmentId;
				MHD_AttachmentId = qualityExtensionEntity.MHD_AttachmentId;
				MineralsConfirmity_AttachmentId = qualityExtensionEntity.MineralsConfirmity_AttachmentId;
				PackagingRegulation_Available = qualityExtensionEntity.PackagingRegulation_Available;
				PackagingRegulation_Available_AttachmentId = qualityExtensionEntity.PackagingRegulation_Available_AttachmentId;
				PurchasingArticleInspection__SpecialArticlesCustomerSpecific = qualityExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific;
				PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId = qualityExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId;
				QSV = qualityExtensionEntity.QSV;
				QSV_AttachmentId = qualityExtensionEntity.QSV_AttachmentId;
				REACH_SVHC_Confirmity_AttachmentId = qualityExtensionEntity.REACH_SVHC_Confirmity_AttachmentId;
				ROHS_EEE_Confirmity_AttachmentId = qualityExtensionEntity.ROHS_EEE_Confirmity_AttachmentId;
				SpecialCustomerReleases__DeviationReleases = qualityExtensionEntity.SpecialCustomerReleases__DeviationReleases;
				SpecialCustomerReleases__DeviationReleases_AttachmentId = qualityExtensionEntity.SpecialCustomerReleases__DeviationReleases_AttachmentId;
				TSP_Available = qualityExtensionEntity.TSP_Available;
				TSP_Available_AttachmentId = qualityExtensionEntity.TSP_Available_AttachmentId;
				UL_Etikett_AttachmentId = qualityExtensionEntity.UL_Etikett_AttachmentId;
				UL_zertifiziert_AttachmentId = qualityExtensionEntity.UL_zertifiziert_AttachmentId;
				UpdateTime = qualityExtensionEntity.UpdateTime;
				UpdateUserId = qualityExtensionEntity.UpdateUserId;
			}


		}
		public ArticleModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity artikelExtensionEntity,
			Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity qualityExtensionEntity,
			Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity artikelProductionExtensionEntity)
			: this(artikelEntity, artikelExtensionEntity, qualityExtensionEntity)
		{
			if(artikelEntity == null)
				return;

			if(artikelProductionExtensionEntity != null)
			{
				ProductionPlace1_Id = artikelProductionExtensionEntity.ProductionPlace1_Id;
				ProductionPlace2_Id = artikelProductionExtensionEntity.ProductionPlace2_Id;
				ProductionPlace3_Id = artikelProductionExtensionEntity.ProductionPlace3_Id;
				ProductionPlace1_Name = artikelProductionExtensionEntity.ProductionPlace1_Name;
				ProductionPlace2_Name = artikelProductionExtensionEntity.ProductionPlace2_Name;
				ProductionPlace3_Name = artikelProductionExtensionEntity.ProductionPlace3_Name;
				AlternativeProductionPlace = artikelProductionExtensionEntity.AlternativeProductionPlace;
			}
		}
		public ArticleModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity artikelExtensionEntity,
			Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity qualityExtensionEntity,
			Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity artikelProductionExtensionEntity,
			Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity logisticsExtensionEntity)
			: this(artikelEntity, artikelExtensionEntity, qualityExtensionEntity, artikelProductionExtensionEntity)
		{
			if(artikelEntity == null)
				return;
			ZolltarifNummer = artikelEntity.Zolltarif_nr;
			Exportgewicht______ = artikelEntity.Exportgewicht;
			Artikelbezeichnung = artikelEntity.Artikelbezeichnung;

			if(logisticsExtensionEntity != null)
			{
				Anlieferadresse_Abladeort = logisticsExtensionEntity.Anlieferadresse_Abladeort;
				ArticleId = logisticsExtensionEntity.ArticleId;
				VDALabel = logisticsExtensionEntity.VDALabel;
			}
		}

		public ArticleModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity objectLogEntity)
		{
			if(artikelEntity == null)
				return;
			if(objectLogEntity != null)
			{

			}
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				ArtikelNr = this.ArtikelNr,
				ArtikelNummer = this.ArtikelNummer,

				Abladestelle = this.Abladestelle,
				aktiv = this.aktiv,
				aktualisiert = this.aktualisiert,
				Anfangsbestand = this.Anfangsbestand,
				ArtikelAusEigenerProduktion = this.ArtikelAusEigenerProduktion,
				ArtikelFürWeitereBestellungenSperren = this.ArtikelFürWeitereBestellungenSperren,
				Artikelfamilie_Kunde = this.Artikelfamilie_Kunde,
				Artikelfamilie_Kunde_Detail1 = this.Artikelfamilie_Kunde_Detail1,
				Artikelfamilie_Kunde_Detail2 = this.Artikelfamilie_Kunde_Detail2,
				Artikelkurztext = this.Artikelkurztext,
				Barverkauf = this.Barverkauf,
				Bezeichnung1 = this.Bezeichnung1,
				Bezeichnung2 = this.Bezeichnung2,
				Bezeichnung3 = this.Bezeichnung3,
				BezeichnungAL = this.BezeichnungAL,
				COF_Pflichtig = this.COF_Pflichtig,
				Crossreferenz = this.Crossreferenz,
				CuGewicht = this.CuGewicht,
				DatumAnfangsbestand = this.DatumAnfangsbestand,
				DEL = this.DEL,
				DELFixiert = this.DELFixiert,
				Dokumente = this.Dokumente,
				EAN = this.EAN,
				Einheit = this.Einheit,
				EMPB = this.EMPB,
				EMPB_Freigegeben = this.EMPB_Freigegeben,
				Ersatzartikel = this.Ersatzartikel,
				ESD_Schutz = this.ESD_Schutz,
				Exportgewicht = this.Exportgewicht,
				fakturierenStückliste = this.fakturierenStückliste,
				Farbe = this.Farbe,
				fibu_rahmen = this.fibu_rahmen,
				Freigabestatus = this.Freigabestatus,
				FreigabestatusTNIntern = this.FreigabestatusTNIntern,
				Gebinde = this.Gebinde,
				Gewicht = this.Gewicht,
				Größe = this.Größe,
				GrundFürSperre = this.GrundFürSperre,
				gültigBis = this.gültigBis,
				Halle = this.Halle,
				Hubmastleitungen = this.Hubmastleitungen,
				ID_Klassifizierung = this.ID_Klassifizierung,
				Index_Kunde = this.Index_Kunde,
				Index_Kunde_Datum = this.Index_Kunde_Datum,
				Info_WE = this.Info_WE,
				Kanban = this.Kanban,
				Kategorie = this.Kategorie,
				Klassifizierung = this.Klassifizierung,
				Kriterium1 = this.Kriterium1,
				Kriterium2 = this.Kriterium2,
				Kriterium3 = this.Kriterium3,
				Kriterium4 = this.Kriterium4,
				Kupferbasis = this.Kupferbasis,
				Kupferzahl = this.Kupferzahl,
				Lagerartikel = this.Lagerartikel,
				Lagerhaltungskosten = this.Lagerhaltungskosten,
				Langtext = this.Langtext,
				Langtext_drucken_AB = this.Langtext_drucken_AB,
				Langtext_drucken_BW = this.Langtext_drucken_BW,
				Lieferzeit = this.Lieferzeit,
				Losgroesse = this.Losgroesse,
				Materialkosten_Alt = this.Materialkosten_Alt,
				MHD = this.MHD,
				MineralsConfirmity = this.MineralsConfirmity,
				Praeferenz_Aktuelles_jahr = this.Praeferenz_Aktuelles_jahr,
				Praeferenz_Folgejahr = this.Praeferenz_Folgejahr,
				Preiseinheit = this.Preiseinheit,
				proZeiteinheit = this.proZeiteinheit,
				Produktionszeit = this.Produktionszeit,
				Provisionsartikel = this.Provisionsartikel,
				PrufstatusTNWare = this.PrufstatusTNWare,
				Rabattierfähig = this.Rabattierfähig,
				Rahmen = this.Rahmen,
				Rahmen2 = this.Rahmen2,
				Rahmenauslauf = this.Rahmenauslauf,
				Rahmenauslauf2 = this.Rahmenauslauf2,
				Rahmenmenge = this.Rahmenmenge,
				Rahmenmenge2 = this.Rahmenmenge2,
				RahmenNr = this.RahmenNr,
				RahmenNr2 = this.RahmenNr2,
				REACHSVHCConfirmity = this.REACHSVHCConfirmity,
				ROHSEEEConfirmity = this.ROHSEEEConfirmity,
				Seriennummer = this.Seriennummer,
				Seriennummernverwaltung = this.Seriennummernverwaltung,
				Sonderrabatt = this.Sonderrabatt,
				Standard_Lagerort_id = this.Standard_Lagerort_id,
				Stuckliste = this.Stückliste,
				Stundensatz = this.Stundensatz,
				Sysmonummer = this.Sysmonummer,
				ULEtikett = this.ULEtikett,
				ULzertifiziert = this.ULzertifiziert,
				Umsatzsteuer = this.Umsatzsteuer,
				Ursprungsland = this.Ursprungsland,
				Verpackung = this.Verpackung,
				Verpackungsart = this.Verpackungsart,
				Verpackungsmenge = this.Verpackungsmenge,
				VKFestpreis = this.VKFestpreis,
				Volumen = this.Volumen,
				Warengruppe = this.Warengruppe,
				Warentyp = this.Warentyp,
				Webshop = this.Webshop,
				Werkzeug = this.Werkzeug,
				Wert_Anfangsbestand = this.Wert_Anfangsbestand,
				Zeichnungsnummer = this.Zeichnungsnummer,
				Zeitraum_MHD = this.Zeitraum_MHD,
				Zolltarif_nr = this.Zolltarif_nr,
				UBG = this.UBG,
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity ToExtensionEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity
			{
				Id = -1,
				ImageId = ArticleImageId,
				ProjectTypeId = ProjectTypeId,
				OrderNumber = OrderNumber,
				CustomerInquiryNumber = CustomerInquiryNumber,
				QuotationsBased12Months = QuotationsBased12Months,
				SOPAppointmentCustomer = SOPAppointmentCustomer,
				Consumption12Months = Consumption12Months,
				Sales12MonthsPerItem = Sales12MonthsPerItem,
				CreatorOrder = CreatorOrder,
				CustomerContactPersonId = CustomerContactPersonId,
				CopperCostBasis = CopperCostBasis,
				CopperCostBasis150 = CopperCostBasis150
			};
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity ToQualityExtension()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity
			{
				ArticleId = Id,
				CoC_Pflichtig_AttachmentId = CoC_Pflichtig_AttachmentId,
				CreateTime = CreateTime,
				CreateUserId = CreateUserId,
				Dienstleistung_AttachmentId = Dienstleistung_AttachmentId,
				EMPB_AttachmentId = EMPB_AttachmentId,
				EMPB_Freigegeben_AttachmentId = EMPB_Freigegeben_AttachmentId,
				ESD_AttachmentId = ESD_AttachmentId,
				HM_AttachmentId = HM_AttachmentId,
				LLE_AttachmentId = LLE_AttachmentId,
				MHD_AttachmentId = MHD_AttachmentId,
				MineralsConfirmity_AttachmentId = MineralsConfirmity_AttachmentId,
				PackagingRegulation_Available = PackagingRegulation_Available,
				PackagingRegulation_Available_AttachmentId = PackagingRegulation_Available_AttachmentId,
				PurchasingArticleInspection__SpecialArticlesCustomerSpecific = PurchasingArticleInspection__SpecialArticlesCustomerSpecific,
				PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId = PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId,
				QSV = QSV,
				QSV_AttachmentId = QSV_AttachmentId,
				REACH_SVHC_Confirmity_AttachmentId = REACH_SVHC_Confirmity_AttachmentId,
				ROHS_EEE_Confirmity_AttachmentId = ROHS_EEE_Confirmity_AttachmentId,
				SpecialCustomerReleases__DeviationReleases = SpecialCustomerReleases__DeviationReleases,
				SpecialCustomerReleases__DeviationReleases_AttachmentId = SpecialCustomerReleases__DeviationReleases_AttachmentId,
				TSP_Available = TSP_Available,
				TSP_Available_AttachmentId = TSP_Available_AttachmentId,
				UL_Etikett_AttachmentId = UL_Etikett_AttachmentId,
				UL_zertifiziert_AttachmentId = UL_zertifiziert_AttachmentId,
				UpdateTime = UpdateTime,
				UpdateUserId = UpdateUserId,
			};
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity ToProductionExtension()
		{
			return new ArtikelProductionExtensionEntity
			{
				Id = -1,
				ArticleId = Id,
				ProductionPlace1_Id = ProductionPlace1_Id,
				ProductionPlace2_Id = ProductionPlace2_Id,
				ProductionPlace3_Id = ProductionPlace3_Id,
				ProductionPlace1_Name = ProductionPlace1_Name,
				ProductionPlace2_Name = ProductionPlace2_Name,
				ProductionPlace3_Name = ProductionPlace3_Name,
				AlternativeProductionPlace = AlternativeProductionPlace
			};
		}
		//
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToLogisticsEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				ArtikelNr = this.ArtikelNr,
				//ArtikelNummer = this.ArtikelNummer,

				//Abladestelle = this.Abladestelle,
				//aktiv = this.aktiv,
				//aktualisiert = this.aktualisiert,
				//Anfangsbestand = this.Anfangsbestand,
				//ArtikelAusEigenerProduktion = this.ArtikelAusEigenerProduktion,
				//ArtikelFürWeitereBestellungenSperren = this.ArtikelFürWeitereBestellungenSperren,
				//Artikelfamilie_Kunde = this.Artikelfamilie_Kunde,
				//Artikelfamilie_Kunde_Detail1 = this.Artikelfamilie_Kunde_Detail1,
				//Artikelfamilie_Kunde_Detail2 = this.Artikelfamilie_Kunde_Detail2,
				//Artikelkurztext = this.Artikelkurztext,
				//Barverkauf = this.Barverkauf,
				//Bezeichnung1 = this.Bezeichnung1,
				//Bezeichnung2 = this.Bezeichnung2,
				//Bezeichnung3 = this.Bezeichnung3,
				//BezeichnungAL = this.BezeichnungAL,
				//COF_Pflichtig = this.COF_Pflichtig,
				//Crossreferenz = this.Crossreferenz,
				//CuGewicht = this.CuGewicht,
				//DatumAnfangsbestand = this.DatumAnfangsbestand,
				//DEL = this.DEL,
				//DELFixiert = this.DELFixiert,
				//Dokumente = this.Dokumente,
				//EAN = this.EAN,
				//Einheit = this.Einheit,
				//EMPB = this.EMPB,
				//EMPB_Freigegeben = this.EMPB_Freigegeben,
				//Ersatzartikel = this.Ersatzartikel,
				//ESD_Schutz = this.ESD_Schutz,
				Exportgewicht = this.Exportgewicht,
				//fakturierenStückliste = this.fakturierenStückliste,
				//Farbe = this.Farbe,
				//fibu_rahmen = this.fibu_rahmen,
				//Freigabestatus = this.Freigabestatus,
				//FreigabestatusTNIntern = this.FreigabestatusTNIntern,
				//Gebinde = this.Gebinde,
				//Gewicht = this.Gewicht,
				//Größe = this.Größe,
				//GrundFürSperre = this.GrundFürSperre,
				//gültigBis = this.gültigBis,
				//Halle = this.Halle,
				//Hubmastleitungen = this.Hubmastleitungen,
				//ID_Klassifizierung = this.ID_Klassifizierung,
				//Index_Kunde = this.Index_Kunde,
				//Index_Kunde_Datum = this.Index_Kunde_Datum,
				//Info_WE = this.Info_WE,
				//Kanban = this.Kanban,
				//Kategorie = this.Kategorie,
				//Klassifizierung = this.Klassifizierung,
				//Kriterium1 = this.Kriterium1,
				//Kriterium2 = this.Kriterium2,
				//Kriterium3 = this.Kriterium3,
				//Kriterium4 = this.Kriterium4,
				//Kupferbasis = this.Kupferbasis,
				//Kupferzahl = this.Kupferzahl,
				//Lagerartikel = this.Lagerartikel,
				//Lagerhaltungskosten = this.Lagerhaltungskosten,
				//Langtext = this.Langtext,
				//Langtext_drucken_AB = this.Langtext_drucken_AB,
				//Langtext_drucken_BW = this.Langtext_drucken_BW,
				//Lieferzeit = this.Lieferzeit,
				//Losgroesse = this.Losgroesse,
				//Materialkosten_Alt = this.Materialkosten_Alt,
				//MHD = this.MHD,
				//MineralsConfirmity = this.MineralsConfirmity,
				//Praeferenz_Aktuelles_jahr = this.Praeferenz_Aktuelles_jahr,
				//Praeferenz_Folgejahr = this.Praeferenz_Folgejahr,
				//Preiseinheit = this.Preiseinheit,
				//proZeiteinheit = this.proZeiteinheit,
				//Produktionszeit = this.Produktionszeit,
				//Provisionsartikel = this.Provisionsartikel,
				//PrufstatusTNWare = this.PrufstatusTNWare,
				//Rabattierfähig = this.Rabattierfähig,
				//Rahmen = this.Rahmen,
				//Rahmen2 = this.Rahmen2,
				//Rahmenauslauf = this.Rahmenauslauf,
				//Rahmenauslauf2 = this.Rahmenauslauf2,
				//Rahmenmenge = this.Rahmenmenge,
				//Rahmenmenge2 = this.Rahmenmenge2,
				//RahmenNr = this.RahmenNr,
				//RahmenNr2 = this.RahmenNr2,
				//REACHSVHCConfirmity = this.REACHSVHCConfirmity,
				//ROHSEEEConfirmity = this.ROHSEEEConfirmity,
				//Seriennummer = this.Seriennummer,
				//Seriennummernverwaltung = this.Seriennummernverwaltung,
				//Sonderrabatt = this.Sonderrabatt,
				//Standard_Lagerort_id = this.Standard_Lagerort_id,
				//Stückliste = this.Stückliste,
				//Stundensatz = this.Stundensatz,
				//Sysmonummer = this.Sysmonummer,
				//ULEtikett = this.ULEtikett,
				//ULzertifiziert = this.ULzertifiziert,
				//Umsatzsteuer = this.Umsatzsteuer,
				Ursprungsland = this.Ursprungsland,
				//Verpackung = this.Verpackung,
				//Verpackungsart = this.Verpackungsart,
				//Verpackungsmenge = this.Verpackungsmenge,
				//VKFestpreis = this.VKFestpreis,
				//Volumen = this.Volumen,
				//Warengruppe = this.Warengruppe,
				//Warentyp = this.Warentyp,
				//Webshop = this.Webshop,
				//Werkzeug = this.Werkzeug,
				//Wert_Anfangsbestand = this.Wert_Anfangsbestand,
				//Zeichnungsnummer = this.Zeichnungsnummer,
				//Zeitraum_MHD = this.Zeitraum_MHD,
				Zolltarif_nr = this.Zolltarif_nr,
				VDA_1 = this.VDALabel,
				VDA_2 = this.VDA_P,
				Größe = this.Grosse
			};
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity ToLogisticsExtension()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity
			{
				ArticleId = Id,
				CreateTime = CreateTime,
				CreateUserId = CreateUserId,
				Anlieferadresse_Abladeort = Anlieferadresse_Abladeort,
				VDALabel = VDALabel,
				UpdateTime = UpdateTime,
				UpdateUserId = UpdateUserId,
			};
		}
	}
}
