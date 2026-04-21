using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ArtikelQualityExtensionEntity
	{
		public int ArticleId { get; set; }
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

		public ArtikelQualityExtensionEntity() { }

		public ArtikelQualityExtensionEntity(DataRow dataRow)
		{
			ArticleId = Convert.ToInt32(dataRow["ArticleId"]);
			CoC_Pflichtig_AttachmentId = (dataRow["CoC_Pflichtig_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CoC_Pflichtig_AttachmentId"]);
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			Dienstleistung_AttachmentId = (dataRow["Dienstleistung_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Dienstleistung_AttachmentId"]);
			EMPB_AttachmentId = (dataRow["EMPB_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EMPB_AttachmentId"]);
			EMPB_Freigegeben_AttachmentId = (dataRow["EMPB_Freigegeben_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EMPB_Freigegeben_AttachmentId"]);
			ESD_AttachmentId = (dataRow["ESD_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ESD_AttachmentId"]);
			HM_AttachmentId = (dataRow["HM_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HM_AttachmentId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LLE_AttachmentId = (dataRow["LLE_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LLE_AttachmentId"]);
			MHD_AttachmentId = (dataRow["MHD_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MHD_AttachmentId"]);
			MineralsConfirmity_AttachmentId = (dataRow["MineralsConfirmity_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MineralsConfirmity_AttachmentId"]);
			PackagingRegulation_Available = (dataRow["PackagingRegulation_Available"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["PackagingRegulation_Available"]);
			PackagingRegulation_Available_AttachmentId = (dataRow["PackagingRegulation_Available_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PackagingRegulation_Available_AttachmentId"]);
			PurchasingArticleInspection__SpecialArticlesCustomerSpecific = (dataRow["PurchasingArticleInspection__SpecialArticlesCustomerSpecific"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["PurchasingArticleInspection__SpecialArticlesCustomerSpecific"]);
			PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId = (dataRow["PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId"]);
			QSV = (dataRow["QSV"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["QSV"]);
			QSV_AttachmentId = (dataRow["QSV_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["QSV_AttachmentId"]);
			REACH_SVHC_Confirmity_AttachmentId = (dataRow["REACH_SVHC_Confirmity_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["REACH_SVHC_Confirmity_AttachmentId"]);
			ROHS_EEE_Confirmity_AttachmentId = (dataRow["ROHS_EEE_Confirmity_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ROHS_EEE_Confirmity_AttachmentId"]);
			SpecialCustomerReleases__DeviationReleases = (dataRow["SpecialCustomerReleases__DeviationReleases"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["SpecialCustomerReleases__DeviationReleases"]);
			SpecialCustomerReleases__DeviationReleases_AttachmentId = (dataRow["SpecialCustomerReleases__DeviationReleases_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SpecialCustomerReleases__DeviationReleases_AttachmentId"]);
			TSP_Available = (dataRow["TSP_Available"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["TSP_Available"]);
			TSP_Available_AttachmentId = (dataRow["TSP_Available_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TSP_Available_AttachmentId"]);
			UL_Etikett_AttachmentId = (dataRow["UL_Etikett_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UL_Etikett_AttachmentId"]);
			UL_zertifiziert_AttachmentId = (dataRow["UL_zertifiziert_AttachmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UL_zertifiziert_AttachmentId"]);
			UpdateTime = (dataRow["UpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
		}
	}
}

