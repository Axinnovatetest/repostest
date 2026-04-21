using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UpdateAttachmentHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.UpdateAttachmentModel _data { get; set; }
		public UpdateAttachmentHandler(Identity.Models.UserModel user, Models.UpdateAttachmentModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var newAttachmentId = -1;
				//newAttachmentId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(-1, this._data.AttachmentFile, this._data.AttachmentFileExtension,(int)Enums.ArticleEnums.ArticleFileType.QualityAttachement);

				newAttachmentId = Psz.Core.Common.Helpers.ImageFileHelper.NewTempFile(this._data.AttachmentFile, this._data.AttachmentFileExtension);



				//var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Id);

				//var artikelExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data.Id);
				//if (artikelExtensionEntity == null)
				//{
				//    newAttachmentId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(-1, this._data.AttachmentFile, this._data.AttachmentFileExtension);
				//    var artcleQuality = new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity
				//    {
				//        Id = -1,
				//        ArticleId = artikelEntity.ArtikelNr,
				//    };
				//    switch ((Enums.ArticleEnums.ArticleQualityAttachments)this._data.AttachmentType)
				//    {
				//        case Enums.ArticleEnums.ArticleQualityAttachments.UL_zertifiziert:
				//            artcleQuality.UL_zertifiziert_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.UL_Etikett:
				//            artcleQuality.UL_Etikett_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.LLE:
				//            artcleQuality.LLE_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.ESD:
				//            artcleQuality.ESD_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.HM:
				//            artcleQuality.HM_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.ROHS_EEE_Confirmity:
				//            artcleQuality.ROHS_EEE_Confirmity_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.MineralsConfirmity:
				//            artcleQuality.MineralsConfirmity_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.REACH_SVHC_Confirmity:
				//            artcleQuality.REACH_SVHC_Confirmity_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.Dienstleistung:
				//            artcleQuality.Dienstleistung_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.MHD:
				//            artcleQuality.MHD_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.CoC_Pflichtig:
				//            artcleQuality.CoC_Pflichtig_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.EMPB:
				//            artcleQuality.EMPB_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.EMPB_Freigegeben:
				//            artcleQuality.EMPB_Freigegeben_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.TSP_Available:
				//            artcleQuality.TSP_Available_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.PurchasingArticleInspection__SpecialArticlesCustomerSpecific:
				//            artcleQuality.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.PackagingRegulation_Available:
				//            artcleQuality.PackagingRegulation_Available_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.QSV:
				//            artcleQuality.QSV_AttachmentId = newAttachmentId;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.SpecialCustomerReleases__DeviationReleases:
				//            artcleQuality.SpecialCustomerReleases__DeviationReleases_AttachmentId = newAttachmentId;
				//            break;
				//        default:
				//            break;
				//    }

				//    if (Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.Insert(artcleQuality) > 0)
				//    {
				//        return ResponseModel<int>.SuccessResponse(newAttachmentId);
				//    }
				//}
				//else
				//{
				//    var attachmentId = -1;
				//    switch ((Enums.ArticleEnums.ArticleQualityAttachments)this._data.AttachmentType)
				//    {
				//        case Enums.ArticleEnums.ArticleQualityAttachments.UL_zertifiziert:
				//            attachmentId = artikelExtensionEntity.UL_zertifiziert_AttachmentId.HasValue ? artikelExtensionEntity.UL_zertifiziert_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.UL_Etikett:
				//            attachmentId = artikelExtensionEntity.UL_Etikett_AttachmentId.HasValue ? artikelExtensionEntity.UL_Etikett_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.LLE:
				//            attachmentId = artikelExtensionEntity.LLE_AttachmentId.HasValue ? artikelExtensionEntity.LLE_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.ESD:
				//            attachmentId = artikelExtensionEntity.ESD_AttachmentId.HasValue ? artikelExtensionEntity.ESD_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.HM:
				//            attachmentId = artikelExtensionEntity.HM_AttachmentId.HasValue ? artikelExtensionEntity.HM_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.ROHS_EEE_Confirmity:
				//            attachmentId = artikelExtensionEntity.ROHS_EEE_Confirmity_AttachmentId.HasValue ? artikelExtensionEntity.ROHS_EEE_Confirmity_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.MineralsConfirmity:
				//            attachmentId = artikelExtensionEntity.MineralsConfirmity_AttachmentId.HasValue ? artikelExtensionEntity.MineralsConfirmity_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.REACH_SVHC_Confirmity:
				//            attachmentId = artikelExtensionEntity.REACH_SVHC_Confirmity_AttachmentId.HasValue ? artikelExtensionEntity.REACH_SVHC_Confirmity_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.Dienstleistung:
				//            attachmentId = artikelExtensionEntity.Dienstleistung_AttachmentId.HasValue ? artikelExtensionEntity.Dienstleistung_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.MHD:
				//            attachmentId = artikelExtensionEntity.MHD_AttachmentId.HasValue ? artikelExtensionEntity.MHD_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.CoC_Pflichtig:
				//            attachmentId = artikelExtensionEntity.CoC_Pflichtig_AttachmentId.HasValue ? artikelExtensionEntity.CoC_Pflichtig_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.EMPB:
				//            attachmentId = artikelExtensionEntity.EMPB_AttachmentId.HasValue ? artikelExtensionEntity.EMPB_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.EMPB_Freigegeben:
				//            attachmentId = artikelExtensionEntity.EMPB_Freigegeben_AttachmentId.HasValue ? artikelExtensionEntity.EMPB_Freigegeben_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.PackagingRegulation_Available:
				//            attachmentId = artikelExtensionEntity.PackagingRegulation_Available_AttachmentId.HasValue ? artikelExtensionEntity.PackagingRegulation_Available_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.PurchasingArticleInspection__SpecialArticlesCustomerSpecific:
				//            attachmentId = artikelExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId.HasValue ? artikelExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.QSV:
				//            attachmentId = artikelExtensionEntity.QSV_AttachmentId.HasValue ? artikelExtensionEntity.QSV_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.SpecialCustomerReleases__DeviationReleases:
				//            attachmentId = artikelExtensionEntity.SpecialCustomerReleases__DeviationReleases_AttachmentId.HasValue ? artikelExtensionEntity.SpecialCustomerReleases__DeviationReleases_AttachmentId.Value : -1;
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.TSP_Available:
				//            attachmentId = artikelExtensionEntity.TSP_Available_AttachmentId.HasValue ? artikelExtensionEntity.TSP_Available_AttachmentId.Value : -1;
				//            break;
				//        default:
				//            break;
				//    }

				//    newAttachmentId = Common.Helpers.ImageFileHelper.updateImage(attachmentId, this._data.AttachmentFile, this._data.AttachmentFileExtension);
				//    var updateColumn = "";
				//    switch ((Enums.ArticleEnums.ArticleQualityAttachments)this._data.AttachmentType)
				//    {
				//        case Enums.ArticleEnums.ArticleQualityAttachments.UL_zertifiziert:
				//            artikelExtensionEntity.UL_zertifiziert_AttachmentId = newAttachmentId;
				//            //artikelEntity.ULzertifiziert = true;
				//            updateColumn = QualityExtensionColumns.UL_zertifiziert_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.UL_Etikett:
				//            artikelExtensionEntity.UL_Etikett_AttachmentId = newAttachmentId;
				//            //artikelEntity.ULEtikett = true;
				//            updateColumn = QualityExtensionColumns.UL_Etikett_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.LLE:
				//            artikelExtensionEntity.LLE_AttachmentId = newAttachmentId;
				//            //artikelEntity.Webshop = true;
				//            updateColumn = QualityExtensionColumns.LLE_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.ESD:
				//            artikelExtensionEntity.ESD_AttachmentId = newAttachmentId;
				//            //artikelEntity.ESD_Schutz = true;
				//            updateColumn = QualityExtensionColumns.ESD_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.HM:
				//            artikelExtensionEntity.HM_AttachmentId = newAttachmentId;
				//            //artikelEntity.Hubmastleitungen = true;
				//            updateColumn = QualityExtensionColumns.HM_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.ROHS_EEE_Confirmity:
				//            artikelExtensionEntity.ROHS_EEE_Confirmity_AttachmentId = newAttachmentId;
				//            //artikelEntity.ROHSEEEConfirmity = true;
				//            updateColumn = QualityExtensionColumns.ROHS_EEE_Confirmity_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.MineralsConfirmity:
				//            artikelExtensionEntity.MineralsConfirmity_AttachmentId = newAttachmentId;
				//            //artikelEntity.MineralsConfirmity = true;
				//            updateColumn = QualityExtensionColumns.MineralsConfirmity_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.REACH_SVHC_Confirmity:
				//            artikelExtensionEntity.REACH_SVHC_Confirmity_AttachmentId = newAttachmentId;
				//            //artikelEntity.REACHSVHCConfirmity = true;
				//            updateColumn = QualityExtensionColumns.REACH_SVHC_Confirmity_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.Dienstleistung:
				//            artikelExtensionEntity.Dienstleistung_AttachmentId = newAttachmentId;
				//            //artikelEntity.ULEtikett = true; >>> MISSING FIELD
				//            updateColumn = QualityExtensionColumns.Dienstleistung_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.MHD:
				//            artikelExtensionEntity.MHD_AttachmentId = newAttachmentId;
				//            //artikelEntity.MHD = true;
				//            updateColumn = QualityExtensionColumns.MHD_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.CoC_Pflichtig:
				//            artikelExtensionEntity.CoC_Pflichtig_AttachmentId = newAttachmentId;
				//            //artikelEntity.COF_Pflichtig = true;
				//            updateColumn = QualityExtensionColumns.CoC_Pflichtig_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.EMPB:
				//            artikelExtensionEntity.EMPB_AttachmentId = newAttachmentId;
				//            //artikelEntity.EMPB = true;
				//            updateColumn = QualityExtensionColumns.EMPB_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.EMPB_Freigegeben:
				//            artikelExtensionEntity.EMPB_Freigegeben_AttachmentId = newAttachmentId;
				//            //artikelEntity.EMPB_Freigegeben = true;
				//            updateColumn = QualityExtensionColumns.EMPB_Freigegeben_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.PackagingRegulation_Available:
				//            artikelExtensionEntity.PackagingRegulation_Available_AttachmentId = newAttachmentId;
				//            //artikelExtensionEntity.PackagingRegulation_Available = true;
				//            updateColumn = QualityExtensionColumns.PackagingRegulation_Available_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.PurchasingArticleInspection__SpecialArticlesCustomerSpecific:
				//            artikelExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId = newAttachmentId;
				//            //artikelExtensionEntity.PurchasingArticleInspection__SpecialArticlesCustomerSpecific = true;
				//            updateColumn = QualityExtensionColumns.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.QSV:
				//            artikelExtensionEntity.QSV_AttachmentId = newAttachmentId;
				//            //artikelExtensionEntity.QSV = true;
				//            updateColumn = QualityExtensionColumns.QSV_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.SpecialCustomerReleases__DeviationReleases:
				//            artikelExtensionEntity.SpecialCustomerReleases__DeviationReleases_AttachmentId = newAttachmentId;
				//            //artikelExtensionEntity.SpecialCustomerReleases__DeviationReleases = true;
				//            updateColumn = QualityExtensionColumns.SpecialCustomerReleases__DeviationReleases_AttachmentId.GetDescription();
				//            break;
				//        case Enums.ArticleEnums.ArticleQualityAttachments.TSP_Available:
				//            artikelExtensionEntity.TSP_Available_AttachmentId = newAttachmentId;
				//            //artikelExtensionEntity.TSP_Available = true;
				//            updateColumn = QualityExtensionColumns.TSP_Available_AttachmentId.GetDescription();
				//            break;
				//        default:
				//            break;
				//    }

				//    if (!string.IsNullOrEmpty(updateColumn) && !string.IsNullOrWhiteSpace(updateColumn))
				//    {
				//        Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.Update(artikelExtensionEntity);
				//        //Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Edit(artikelEntity);
				//        if (Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.UpdateAttachmentId(
				//            artikelEntity.ArtikelNr,
				//            newAttachmentId,
				//            updateColumn) > 0)
				//        {
				//            return ResponseModel<int>.SuccessResponse(newAttachmentId);
				//        }
				//    }
				//}

				return ResponseModel<int>.SuccessResponse(newAttachmentId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Id);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Article not found" }
						}
				};
			}
			if(articleEntity.aktiv.HasValue && !articleEntity.aktiv.Value)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article is not Active"}
					}
				};
			}

			if(this._data.AttachmentFile == null || this._data.AttachmentFile.Length < 0)
				return ResponseModel<int>.FailureResponse("File too small");

			return ResponseModel<int>.SuccessResponse();
		}

		//public enum QualityExtensionColumns
		//{
		//    [Description("CoC_Pflichtig_AttachmentId")]
		//    CoC_Pflichtig_AttachmentId,
		//    [Description("Dienstleistung_AttachmentId")]
		//    Dienstleistung_AttachmentId,
		//    [Description("EMPB_AttachmentId")]
		//    EMPB_AttachmentId,
		//    [Description("EMPB_Freigegeben_AttachmentId")]
		//    EMPB_Freigegeben_AttachmentId,
		//    [Description("ESD_AttachmentId")]
		//    ESD_AttachmentId,
		//    [Description("HM_AttachmentId")]
		//    HM_AttachmentId,
		//    [Description("LLE_AttachmentId")]
		//    LLE_AttachmentId,
		//    [Description("MHD_AttachmentId")]
		//    MHD_AttachmentId,
		//    [Description("MineralsConfirmity_AttachmentId")]
		//    MineralsConfirmity_AttachmentId,
		//    [Description("REACH_SVHC_Confirmity_AttachmentId")]
		//    REACH_SVHC_Confirmity_AttachmentId,
		//    [Description("ROHS_EEE_Confirmity_AttachmentId")]
		//    ROHS_EEE_Confirmity_AttachmentId,
		//    [Description("UL_Etikett_AttachmentId")]
		//    UL_Etikett_AttachmentId,
		//    [Description("UL_zertifiziert_AttachmentId")]
		//    UL_zertifiziert_AttachmentId,
		//    [Description("TSP_Available_AttachmentId")]
		//    TSP_Available_AttachmentId,
		//    [Description("PackagingRegulation_Available_AttachmentId")]
		//    PackagingRegulation_Available_AttachmentId,
		//    [Description("QSV_AttachmentId")]
		//    QSV_AttachmentId,
		//    [Description("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId")]
		//    PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId,
		//    [Description("SpecialCustomerReleases__DeviationReleases_AttachmentId")]
		//    SpecialCustomerReleases__DeviationReleases_AttachmentId
		//}
	}
}
