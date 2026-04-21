using System;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticlePartialModel
	{
		public int PartialData { get; set; } // Enums.ArticleEnums.ArticlePartialData
		#region Default properties
		public int ArticleImageId { get; set; }
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }

		public string Abladestelle { get; set; }
		public Nullable<bool> aktiv { get; set; }
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
		#endregion Default properties


		// >>>>> Data Extension
		#region Data Extension
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
		#endregion Data Extension

		// >>>>> Quality extension
		#region Quality extension
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
		public bool? PurchasingArticleInspection__SpecialArticlesCustomerSpecific { get; set; }
		public bool? QSV { get; set; }
		public int? REACH_SVHC_Confirmity_AttachmentId { get; set; }
		public int? ROHS_EEE_Confirmity_AttachmentId { get; set; }
		public bool? SpecialCustomerReleases__DeviationReleases { get; set; }
		public bool? TSP_Available { get; set; }
		public int? UL_Etikett_AttachmentId { get; set; }
		public int? UL_zertifiziert_AttachmentId { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }
		#endregion Quality extension

		// Logistics Extension
		#region Logistics Extension
		public DateTime? Anlieferadresse_Abladeort { get; set; }
		public bool? VDALabel { get; set; }
		public bool? ULLabel { get; set; }
		public string ZolltarifNummer { get; set; }
		public int? Exportgewicht______ { get; set; } // <<<<<<< Exists in decimal!
		public int? Ursprungsland_Id { get; set; }
		public string Ursprungsland_name { get; set; }
		#endregion Logistics Extension


		// >>>>>>>> Production Extension
		#region Production Extension
		public int? ProductionPlace1_Id { get; set; }
		public int? ProductionPlace2_Id { get; set; }
		public int? ProductionPlace3_Id { get; set; }
		public string ProductionPlace1_Name { get; set; }
		public string ProductionPlace2_Name { get; set; }
		public string ProductionPlace3_Name { get; set; }
		public bool? AlternativeProductionPlace { get; set; }
		#endregion Production Extension


		public ArticlePartialModel() { }

		/// <summary>
		/// Set ArtikelEntity and ArtikelQualityExtensionEntity properties from Model data
		/// </summary>
		/// <param name="artikelEntity"></param>
		/// <param name="qualityExtensionEntity"></param>
		public void SetQualityEntity(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity qualityExtensionEntity)
		{
			if(artikelEntity == null)
				return;

			artikelEntity.ArtikelNr = ArtikelNr;

			artikelEntity.ULzertifiziert = ULzertifiziert;
			artikelEntity.ULEtikett = ULEtikett;
			artikelEntity.Webshop = Webshop;
			artikelEntity.Hubmastleitungen = Hubmastleitungen;
			artikelEntity.ROHSEEEConfirmity = ROHSEEEConfirmity;
			artikelEntity.MineralsConfirmity = MineralsConfirmity;
			artikelEntity.REACHSVHCConfirmity = REACHSVHCConfirmity;
			// diens NOT FOUND in Artikel
			artikelEntity.MHD = MHD;
			artikelEntity.Zeitraum_MHD = Zeitraum_MHD;
			artikelEntity.COF_Pflichtig = COF_Pflichtig;
			artikelEntity.EMPB = EMPB;
			artikelEntity.EMPB_Freigegeben = EMPB_Freigegeben;
			artikelEntity.Freigabestatus = Freigabestatus;
			artikelEntity.PrufstatusTNWare = PrufstatusTNWare;
			artikelEntity.FreigabestatusTNIntern = FreigabestatusTNIntern;
			artikelEntity.ESD_Schutz = ESD_Schutz;


			if(qualityExtensionEntity != null)
			{
				qualityExtensionEntity.ArticleId = ArtikelNr;
				qualityExtensionEntity.CoC_Pflichtig_AttachmentId = CoC_Pflichtig_AttachmentId;
				qualityExtensionEntity.CreateTime = CreateTime;
				qualityExtensionEntity.CreateUserId = CreateUserId;
				qualityExtensionEntity.Dienstleistung_AttachmentId = Dienstleistung_AttachmentId;
				qualityExtensionEntity.EMPB_AttachmentId = EMPB_AttachmentId;
				qualityExtensionEntity.EMPB_Freigegeben_AttachmentId = EMPB_Freigegeben_AttachmentId;
				qualityExtensionEntity.ESD_AttachmentId = ESD_AttachmentId;
				qualityExtensionEntity.HM_AttachmentId = HM_AttachmentId;
				qualityExtensionEntity.LLE_AttachmentId = LLE_AttachmentId;
				qualityExtensionEntity.MHD_AttachmentId = MHD_AttachmentId;
				qualityExtensionEntity.MineralsConfirmity_AttachmentId = MineralsConfirmity_AttachmentId;
				qualityExtensionEntity.PackagingRegulation_Available = PackagingRegulation_Available;
				qualityExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific = PurchasingArticleInspection__SpecialArticlesCustomerSpecific;
				qualityExtensionEntity.QSV = QSV;
				qualityExtensionEntity.REACH_SVHC_Confirmity_AttachmentId = REACH_SVHC_Confirmity_AttachmentId;
				qualityExtensionEntity.ROHS_EEE_Confirmity_AttachmentId = ROHS_EEE_Confirmity_AttachmentId;
				qualityExtensionEntity.SpecialCustomerReleases__DeviationReleases = SpecialCustomerReleases__DeviationReleases;
				qualityExtensionEntity.TSP_Available = TSP_Available;
				qualityExtensionEntity.UL_Etikett_AttachmentId = UL_Etikett_AttachmentId;
				qualityExtensionEntity.UL_zertifiziert_AttachmentId = UL_zertifiziert_AttachmentId;
				qualityExtensionEntity.UpdateTime = UpdateTime;
				qualityExtensionEntity.UpdateUserId = UpdateUserId;
			}
		}
		/// <summary>
		/// Set ArtikelEntity and ArtikelLogisticsExtensionEntity properties from Model data
		/// </summary>
		/// <param name="artikelEntity"></param>
		/// <param name="logisticsExtensionEntity"></param>
		public void SetLogisticsEntity(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity logisticsExtensionEntity)
		{
			if(artikelEntity == null)
				return;

			artikelEntity.ArtikelNr = ArtikelNr;

			artikelEntity.Zolltarif_nr = Zolltarif_nr;
			artikelEntity.Exportgewicht = Exportgewicht;
			artikelEntity.Ursprungsland = Ursprungsland;


			if(logisticsExtensionEntity != null)
			{
				logisticsExtensionEntity.ArticleId = ArtikelNr;
				logisticsExtensionEntity.VDALabel = VDALabel;
				logisticsExtensionEntity.Anlieferadresse_Abladeort = Anlieferadresse_Abladeort;
				logisticsExtensionEntity.CreateTime = CreateTime;
				logisticsExtensionEntity.CreateUserId = CreateUserId;
				logisticsExtensionEntity.UpdateTime = UpdateTime;
				logisticsExtensionEntity.UpdateUserId = UpdateUserId;
			}
		}
		/// <summary>
		/// Set ArtikelEntity and ArtikelProductionExtensionEntity properties from Model data
		/// </summary>
		/// <param name="artikelEntity"></param>
		/// <param name="productionExtensionEntity"></param>
		public void SetProductionEntity(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity productionExtensionEntity)
		{
			if(artikelEntity == null)
				return;

			artikelEntity.ArtikelNr = ArtikelNr;

			artikelEntity.Zolltarif_nr = Zolltarif_nr;
			artikelEntity.Exportgewicht = Exportgewicht;
			artikelEntity.Ursprungsland = Ursprungsland;


			if(productionExtensionEntity != null)
			{
				productionExtensionEntity.ArticleId = ArtikelNr;
				productionExtensionEntity.ProductionPlace1_Id = ProductionPlace1_Id;
				productionExtensionEntity.ProductionPlace2_Id = ProductionPlace2_Id;
				productionExtensionEntity.ProductionPlace3_Id = ProductionPlace3_Id;
				productionExtensionEntity.ProductionPlace1_Name = ProductionPlace1_Name;
				productionExtensionEntity.ProductionPlace2_Name = ProductionPlace2_Name;
				productionExtensionEntity.ProductionPlace3_Name = ProductionPlace3_Name;
				productionExtensionEntity.AlternativeProductionPlace = AlternativeProductionPlace;

				productionExtensionEntity.CreateTime = CreateTime;
				productionExtensionEntity.CreateUserId = CreateUserId;
				productionExtensionEntity.UpdateTime = UpdateTime;
				productionExtensionEntity.UpdateUserId = UpdateUserId;
			}
		}
	}
}
