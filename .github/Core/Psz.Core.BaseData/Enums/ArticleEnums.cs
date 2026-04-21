using System.ComponentModel;

namespace Psz.Core.BaseData.Enums
{
	public class ArticleEnums
	{
		public enum OrderTypes
		{
			[Description("Bestellung")]
			Order = 1,
			[Description("Kanbanabruf")]
			Kanaban = 2,
			[Description("Wareneingang")]
			Wareneingang = 3
		}
		public enum ArticlePartialData
		{
			Overview = 0,
			Data = 1,
			Sales = 2,
			Production = 3,
			Quality = 4,
			Logstics = 5
		}
		public enum CustomPriceType
		{
			[Description("S 1")]
			S1 = 1,
			[Description("S 2")]
			S2 = 2,
			[Description("S 3")]
			S3 = 3,
			[Description("S 4")]
			S4 = 4
		}
		public enum ArticleQualityAttachments
		{
			[Description("UL Certified")]
			UL_zertifiziert,
			[Description("UL label")]
			UL_Etikett,
			[Description("LLE")]
			LLE,
			[Description("ESD")]
			ESD,
			[Description("HM")]
			HM,
			[Description("ROHS EEE Confirmity")]
			ROHS_EEE_Confirmity,
			[Description("Minerals Confirmity")]
			MineralsConfirmity,
			[Description("REACH SVHC Confirmity")]
			REACH_SVHC_Confirmity,
			[Description("Service")]
			Dienstleistung,
			[Description("MHD")]
			MHD,
			[Description("CoC - Obligatory")]
			CoC_Pflichtig,
			[Description("EMPB")]
			EMPB,
			[Description("EMPB Approved")]
			EMPB_Freigegeben,
			[Description("Packaging Regulation")]
			PackagingRegulation_Available,
			[Description("Purchasing Inspection/Customer Specific")]
			PurchasingArticleInspection__SpecialArticlesCustomerSpecific,
			[Description("QSV")]
			QSV,
			[Description("Special Customer Releases/Deviation Releases")]
			SpecialCustomerReleases__DeviationReleases,
			[Description("TSP")]
			TSP_Available
		}

		public enum BomStatus
		{
			[Description("In Preparation")]
			InPreparation = 0,
			[Description("Blocked")]
			Blocked = 2,
			[Description("Approved")]
			Approved = 1
		}

		public enum ESDSchutz
		{
			[Description("Ja")]
			Ja = 0,
			[Description("Nein")]
			Nein = 1,
			[Description("Keine")]
			Keine = 2
		}

		public enum QualityExtensionColumns
		{
			[Description("CoC_Pflichtig_AttachmentId")]
			CoC_Pflichtig_AttachmentId,
			[Description("Dienstleistung_AttachmentId")]
			Dienstleistung_AttachmentId,
			[Description("EMPB_AttachmentId")]
			EMPB_AttachmentId,
			[Description("EMPB_Freigegeben_AttachmentId")]
			EMPB_Freigegeben_AttachmentId,
			[Description("ESD_AttachmentId")]
			ESD_AttachmentId,
			[Description("HM_AttachmentId")]
			HM_AttachmentId,
			[Description("LLE_AttachmentId")]
			LLE_AttachmentId,
			[Description("MHD_AttachmentId")]
			MHD_AttachmentId,
			[Description("MineralsConfirmity_AttachmentId")]
			MineralsConfirmity_AttachmentId,
			[Description("REACH_SVHC_Confirmity_AttachmentId")]
			REACH_SVHC_Confirmity_AttachmentId,
			[Description("ROHS_EEE_Confirmity_AttachmentId")]
			ROHS_EEE_Confirmity_AttachmentId,
			[Description("UL_Etikett_AttachmentId")]
			UL_Etikett_AttachmentId,
			[Description("UL_zertifiziert_AttachmentId")]
			UL_zertifiziert_AttachmentId,
			[Description("TSP_Available_AttachmentId")]
			TSP_Available_AttachmentId,
			[Description("PackagingRegulation_Available_AttachmentId")]
			PackagingRegulation_Available_AttachmentId,
			[Description("QSV_AttachmentId")]
			QSV_AttachmentId,
			[Description("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId")]
			PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId,
			[Description("SpecialCustomerReleases__DeviationReleases_AttachmentId")]
			SpecialCustomerReleases__DeviationReleases_AttachmentId
		}

		public enum ArticleFileType
		{
			[Description("OverviewImage")]
			OverviewImage = 0,
			[Description("QualityAttachement")]
			QualityAttachement = 1,
			[Description("BOMPosition")]
			BOMPosition = 2,
			[Description("BOMAltPosition")]
			BOMAltPosition = 3,
			[Description("RahmenAuftrag")]
			RahmenAuftrag = 4
		}

		public enum BOMControlStatus
		{
			All = 0,
			[Description("Engineering Control")]
			EngineeringControl = 1,
			[Description("Engineering Update")]
			EngineeringUpdate = 2,
			[Description("Engineering Print")]
			EngineeringPrint = 3,
			[Description("Engineering Distribution")]
			EngineeringDistribution = 4,
			[Description("Quality Control")]
			QualityControl = 5,
			[Description("Quality Update")]
			QualityUpdate = 6,
			[Description("Quality Print")]
			QualityPrint = 7,
			[Description("Quality Distribution")]
			QualityDistribution = 8
		}
		public enum NotificationOptions
		{
			Sales = 0,
			Purchase = 1,
			BomCpControlEngineering = 3,
			BomCpControlQuality = 4
		}
		public enum StatsCartonsCirculationSites
		{
			//[Description("Tunesien")]
			//Tunesien = 1,
			[Description("WS/TN")]
			WolfHalleKHTN = 2,
			//[Description("BE_TN")]
			//BETN = 3,
			[Description("Albanien")]
			Albanien = 4,
			[Description("Tschechien")]
			Tschechien = 5,
			[Description("Vohenstrauss")]
			Vohenstrauss = 6,
			[Description("Gesamt")]
			Gesamt = 7,
			[Description("GZTN")]
			GZTN = 8
		}

		public enum ProductionSites
		{
			[Description("TN")]
			Tunesien = 7,
			[Description("TN")]
			WS = 42,
			[Description("TN")]
			BETN = 60,
			[Description("AL")]
			AL = 26,
			[Description("CZ")]
			CZ = 6,
			[Description("DE")]
			DE = 15,
			[Description("GZ")]
			GZ = 102
		}
	}
}
