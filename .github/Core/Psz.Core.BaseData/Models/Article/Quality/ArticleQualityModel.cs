using System;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.Quality
{
	public class ArticleQualityModel
	{
		public int ArticleID { get; set; }
		public int Id { get; set; }
		//qualityExtension
		public bool? TSP_Available { get; set; }
		public int? TSP_Available_AttachmentId { get; set; }
		public bool? PackagingRegulation_Available { get; set; }
		public int? PackagingRegulation_Available_AttachmentId { get; set; }
		public bool? QSV { get; set; }
		public int? QSV_AttachmentId { get; set; }
		public bool? PurchasingArticleInspection__SpecialArticlesCustomerSpecific { get; set; }
		public int? PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId { get; set; }
		public bool? SpecialCustomerReleases__DeviationReleases { get; set; }
		public int? SpecialCustomerReleases__DeviationReleases_AttachmentId { get; set; }
		//Artikel
		public bool? UL_zertifiziert { get; set; }
		public int? UL_zertifiziert_AttachmentId { get; set; }
		public bool? UL_Etikett { get; set; }
		public int? UL_Etikett_AttachmentId { get; set; }
		public bool? Webshop { get; set; }
		public int? LLE_AttachmentId { get; set; }
		public bool? ESD_Schutz { get; set; }
		public string ESD_Schutz_Text { get; set; }
		public int? ESD_AttachmentId { get; set; }
		public bool? Hubmastleitungen { get; set; }
		public int? HM_AttachmentId { get; set; }
		public bool? ROHS_EEE_Confirmity { get; set; }
		public int? ROHS_EEE_Confirmity_AttachmentId { get; set; }
		public bool? Minerals_Confirmity { get; set; }
		public int? MineralsConfirmity_AttachmentId { get; set; }
		public bool? REACH_SVHC_Confirmity { get; set; }
		public int? REACH_SVHC_Confirmity_AttachmentId { get; set; }
		public bool? Dienstelistung { get; set; }
		public int? Dienstleistung_AttachmentId { get; set; }
		public bool? MHD { get; set; }
		public int? MHD_AttachmentId { get; set; }
		public string Zeitraum_MHD { get; set; }
		public bool? COF_Pflichtig { get; set; }
		public int? CoC_Pflichtig_AttachmentId { get; set; }
		public bool? EMPB { get; set; }
		public int? EMPB_AttachmentId { get; set; }
		public bool? EMPB_Freigegeben { get; set; }
		public int? EMPB_Freigegeben_AttachmentId { get; set; }
		public string Freigabestatus { get; set; }
		public string Freigabestatus_ChangeReason { get; set; }
		public string Prufstatus_TN_Ware { get; set; }
		public string Freigabestatus_TN_intern { get; set; }
		public string Freigabestatus_TN_intern_ChangeReason { get; set; }
		public bool? Kanban { get; set; }
		public bool aktiv { get; set; }
		public bool UBG { get; set; }
		// - 2023-08-24 - CoC version
		public string CocVersion { get; set; }
		// - 2024-08-06 -
		public string DeliveryNoteCustomerComments { get; set; }
		public ArticleQualityModel()
		{

		}
		public ArticleQualityModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity, Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity qualityExtensionEntity)
		{
			if(articleEntity != null)
			{
				ArticleID = articleEntity.ArtikelNr;
				UL_zertifiziert = articleEntity.ULzertifiziert;
				UL_Etikett = articleEntity.ULEtikett;
				Webshop = articleEntity.Webshop;
				ESD_Schutz = articleEntity.ESD_Schutz;
				ESD_Schutz_Text = ((articleEntity.ESD_Schutz_Text == null || articleEntity.ESD_Schutz_Text == "") && (articleEntity.ESD_Schutz == null || articleEntity.ESD_Schutz.HasValue && !articleEntity.ESD_Schutz.Value)) ? BaseData.Enums.ArticleEnums.ESDSchutz.Keine.GetDescription() : articleEntity.ESD_Schutz_Text;
				Hubmastleitungen = articleEntity.Hubmastleitungen;
				ROHS_EEE_Confirmity = articleEntity.ROHSEEEConfirmity;
				Minerals_Confirmity = articleEntity.MineralsConfirmity;
				REACH_SVHC_Confirmity = articleEntity.REACHSVHCConfirmity;
				Dienstelistung = articleEntity.Dienstelistung.HasValue ? (articleEntity.Dienstelistung.Value == 0 ? false : true) : false;
				MHD = articleEntity.MHD;
				Zeitraum_MHD = articleEntity.Zeitraum_MHD;
				COF_Pflichtig = articleEntity.COF_Pflichtig;
				EMPB = articleEntity.EMPB;
				EMPB_Freigegeben = articleEntity.EMPB_Freigegeben;
				Freigabestatus = articleEntity.Freigabestatus;
				Prufstatus_TN_Ware = articleEntity.PrufstatusTNWare;
				Freigabestatus_TN_intern = articleEntity.FreigabestatusTNIntern;
				Kanban = articleEntity.Kanban;
				aktiv = articleEntity.aktiv ?? false;
				UBG = articleEntity.UBG;
				CocVersion = articleEntity.CocVersion;
				DeliveryNoteCustomerComments = articleEntity.DeliveryNoteCustomerComments;
			}
			if(qualityExtensionEntity != null)
			{
				Id = qualityExtensionEntity.Id;
				TSP_Available = qualityExtensionEntity.TSP_Available;
				TSP_Available_AttachmentId = qualityExtensionEntity.TSP_Available_AttachmentId;
				PackagingRegulation_Available = qualityExtensionEntity.PackagingRegulation_Available;
				PackagingRegulation_Available_AttachmentId = qualityExtensionEntity.PackagingRegulation_Available_AttachmentId;
				QSV = qualityExtensionEntity.QSV;
				QSV_AttachmentId = qualityExtensionEntity.QSV_AttachmentId;
				PurchasingArticleInspection__SpecialArticlesCustomerSpecific = qualityExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific;
				PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId = qualityExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId;
				SpecialCustomerReleases__DeviationReleases = qualityExtensionEntity.SpecialCustomerReleases__DeviationReleases;
				SpecialCustomerReleases__DeviationReleases_AttachmentId = qualityExtensionEntity.SpecialCustomerReleases__DeviationReleases_AttachmentId;

				MHD_AttachmentId = qualityExtensionEntity.MHD_AttachmentId;
				UL_zertifiziert_AttachmentId = qualityExtensionEntity.UL_zertifiziert_AttachmentId;
				UL_Etikett_AttachmentId = qualityExtensionEntity.UL_Etikett_AttachmentId;
				LLE_AttachmentId = qualityExtensionEntity.LLE_AttachmentId;
				ESD_AttachmentId = qualityExtensionEntity.ESD_AttachmentId;
				HM_AttachmentId = qualityExtensionEntity.HM_AttachmentId;
				ROHS_EEE_Confirmity_AttachmentId = qualityExtensionEntity.ROHS_EEE_Confirmity_AttachmentId;
				MineralsConfirmity_AttachmentId = qualityExtensionEntity.MineralsConfirmity_AttachmentId;
				REACH_SVHC_Confirmity_AttachmentId = qualityExtensionEntity.REACH_SVHC_Confirmity_AttachmentId;
				Dienstleistung_AttachmentId = qualityExtensionEntity.Dienstleistung_AttachmentId;
				CoC_Pflichtig_AttachmentId = qualityExtensionEntity.CoC_Pflichtig_AttachmentId;
				EMPB_AttachmentId = qualityExtensionEntity.EMPB_AttachmentId;
				EMPB_Freigegeben_AttachmentId = qualityExtensionEntity.EMPB_Freigegeben_AttachmentId;
			}
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToArticleEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				ArtikelNr = ArticleID,
				ULzertifiziert = UL_zertifiziert,
				ULEtikett = UL_Etikett,
				Webshop = Webshop,
				ESD_Schutz = ESD_Schutz,
				ESD_Schutz_Text = ESD_Schutz_Text,
				Hubmastleitungen = Hubmastleitungen,
				ROHSEEEConfirmity = ROHS_EEE_Confirmity,
				MineralsConfirmity = Minerals_Confirmity,
				REACHSVHCConfirmity = REACH_SVHC_Confirmity,
				Dienstelistung = (Dienstelistung.HasValue && Dienstelistung.Value) ? 1 : 0,
				MHD = MHD,
				Zeitraum_MHD = Zeitraum_MHD,
				COF_Pflichtig = COF_Pflichtig,
				EMPB = EMPB,
				EMPB_Freigegeben = EMPB_Freigegeben,
				Freigabestatus = Freigabestatus,
				PrufstatusTNWare = Prufstatus_TN_Ware,
				FreigabestatusTNIntern = Freigabestatus_TN_intern,
				Kanban = Kanban,
				UBG = UBG,
				CocVersion = CocVersion,
				DeliveryNoteCustomerComments = DeliveryNoteCustomerComments,
			};
		}
		public Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity ToQualityExtensionEntity(int UserId, Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity artikelQualityExtensionEntity, bool? persistAttachments = true)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity
			{
				Id = Id,
				ArticleId = ArticleID,
				TSP_Available = TSP_Available,
				TSP_Available_AttachmentId = (persistAttachments == true && TSP_Available_AttachmentId.HasValue && artikelQualityExtensionEntity?.TSP_Available_AttachmentId != TSP_Available_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, TSP_Available_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : TSP_Available_AttachmentId,
				PackagingRegulation_Available = PackagingRegulation_Available,
				PackagingRegulation_Available_AttachmentId = (persistAttachments == true && PackagingRegulation_Available_AttachmentId.HasValue && artikelQualityExtensionEntity?.PackagingRegulation_Available_AttachmentId != PackagingRegulation_Available_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, PackagingRegulation_Available_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : PackagingRegulation_Available_AttachmentId,
				QSV = QSV,
				QSV_AttachmentId = (persistAttachments == true && QSV_AttachmentId.HasValue && artikelQualityExtensionEntity?.QSV_AttachmentId != QSV_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, QSV_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : QSV_AttachmentId,
				PurchasingArticleInspection__SpecialArticlesCustomerSpecific = PurchasingArticleInspection__SpecialArticlesCustomerSpecific,
				PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId = (persistAttachments == true && PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId.HasValue && artikelQualityExtensionEntity?.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId != PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId,
				SpecialCustomerReleases__DeviationReleases = SpecialCustomerReleases__DeviationReleases,
				SpecialCustomerReleases__DeviationReleases_AttachmentId = (persistAttachments == true && SpecialCustomerReleases__DeviationReleases_AttachmentId.HasValue && artikelQualityExtensionEntity?.SpecialCustomerReleases__DeviationReleases_AttachmentId != SpecialCustomerReleases__DeviationReleases_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, SpecialCustomerReleases__DeviationReleases_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : SpecialCustomerReleases__DeviationReleases_AttachmentId,

				MHD_AttachmentId = (persistAttachments == true && MHD_AttachmentId.HasValue && artikelQualityExtensionEntity?.MHD_AttachmentId != MHD_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, MHD_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : MHD_AttachmentId,
				UL_zertifiziert_AttachmentId = (persistAttachments == true && UL_zertifiziert_AttachmentId.HasValue && artikelQualityExtensionEntity?.UL_zertifiziert_AttachmentId != UL_zertifiziert_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, UL_zertifiziert_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : UL_zertifiziert_AttachmentId,
				UL_Etikett_AttachmentId = (persistAttachments == true && UL_Etikett_AttachmentId.HasValue && artikelQualityExtensionEntity?.UL_Etikett_AttachmentId != UL_Etikett_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, UL_Etikett_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : UL_Etikett_AttachmentId,
				LLE_AttachmentId = (persistAttachments == true && LLE_AttachmentId.HasValue && artikelQualityExtensionEntity?.LLE_AttachmentId != LLE_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, LLE_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : LLE_AttachmentId,
				ESD_AttachmentId = (persistAttachments == true && ESD_AttachmentId.HasValue && artikelQualityExtensionEntity?.ESD_AttachmentId != ESD_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, ESD_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : ESD_AttachmentId,
				HM_AttachmentId = (persistAttachments == true && HM_AttachmentId.HasValue && artikelQualityExtensionEntity?.HM_AttachmentId != HM_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, HM_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : HM_AttachmentId,
				ROHS_EEE_Confirmity_AttachmentId = (persistAttachments == true && ROHS_EEE_Confirmity_AttachmentId.HasValue && artikelQualityExtensionEntity?.ROHS_EEE_Confirmity_AttachmentId != ROHS_EEE_Confirmity_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, ROHS_EEE_Confirmity_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : ROHS_EEE_Confirmity_AttachmentId,
				MineralsConfirmity_AttachmentId = (persistAttachments == true && MineralsConfirmity_AttachmentId.HasValue && artikelQualityExtensionEntity?.MineralsConfirmity_AttachmentId != MineralsConfirmity_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, MineralsConfirmity_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : MineralsConfirmity_AttachmentId,
				REACH_SVHC_Confirmity_AttachmentId = (persistAttachments == true && REACH_SVHC_Confirmity_AttachmentId.HasValue && artikelQualityExtensionEntity?.REACH_SVHC_Confirmity_AttachmentId != REACH_SVHC_Confirmity_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, REACH_SVHC_Confirmity_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : REACH_SVHC_Confirmity_AttachmentId,
				Dienstleistung_AttachmentId = (persistAttachments == true && Dienstleistung_AttachmentId.HasValue && artikelQualityExtensionEntity?.Dienstleistung_AttachmentId != Dienstleistung_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, Dienstleistung_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : Dienstleistung_AttachmentId,
				CoC_Pflichtig_AttachmentId = (persistAttachments == true && CoC_Pflichtig_AttachmentId.HasValue && artikelQualityExtensionEntity?.CoC_Pflichtig_AttachmentId != CoC_Pflichtig_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, CoC_Pflichtig_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : CoC_Pflichtig_AttachmentId,
				EMPB_AttachmentId = (persistAttachments == true && EMPB_AttachmentId.HasValue && artikelQualityExtensionEntity?.EMPB_AttachmentId != EMPB_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, EMPB_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : EMPB_AttachmentId,
				EMPB_Freigegeben_AttachmentId = (persistAttachments == true && EMPB_Freigegeben_AttachmentId.HasValue && artikelQualityExtensionEntity?.EMPB_Freigegeben_AttachmentId != EMPB_Freigegeben_AttachmentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFileasync(UserId, EMPB_Freigegeben_AttachmentId.Value, (int)Enums.ArticleEnums.ArticleFileType.QualityAttachement).Result : EMPB_Freigegeben_AttachmentId,
			};
		}

		public bool ConvertIntToBool(int value)
		{
			bool result;
			switch(value)
			{
				case 0:
					result = false;
					break;
				case 1:
					result = true;
					break;
				default:
					Infrastructure.Services.Logging.Logger.Log("Integer value is not valid");
					throw new InvalidOperationException("Integer value is not valid");
			}
			return result;
		}
	}
}
