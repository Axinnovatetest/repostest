using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Quality
{
	public class UpdateArticleQualityHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.Quality.ArticleQualityModel _data { get; set; }
		public UpdateArticleQualityHandler(Identity.Models.UserModel user, Models.Article.Quality.ArticleQualityModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				lock(Locks.ArticleEditLock.GetOrAdd(this._data.ArticleID, new object()))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var logs = LogChanges();
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);

					var response = -1;
					var oldArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleID);
					var articleQualityEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data.ArticleID);
					EliminateEmptyAttachement();
					var newAricleEntity = this._data.ToArticleEntity();

					var addedQualtiyEntity = this._data.ToQualityExtensionEntity(_user.Id, articleQualityEntity);
					//
					if(newAricleEntity != null)
					{
						if(newAricleEntity.ESD_Schutz_Text == BaseData.Enums.ArticleEnums.ESDSchutz.Ja.GetDescription())
							newAricleEntity.ESD_Schutz = true;
						if(newAricleEntity.ESD_Schutz_Text == BaseData.Enums.ArticleEnums.ESDSchutz.Nein.GetDescription() ||
							newAricleEntity.ESD_Schutz_Text == BaseData.Enums.ArticleEnums.ESDSchutz.Keine.GetDescription())
							newAricleEntity.ESD_Schutz = false;

						// - 2023-08-24
						newAricleEntity.COF_Pflichtig = this._data.COF_Pflichtig;
						newAricleEntity.CocVersion = this._data.COF_Pflichtig == true ? this._data.CocVersion : null;
						Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditQuality(newAricleEntity);
					}
					if(articleQualityEntity == null)
					{
						addedQualtiyEntity.CreateTime = DateTime.Now;
						addedQualtiyEntity.CreateUserId = this._user.Id;
						response = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.Insert(addedQualtiyEntity);
					}
					else
					{
						addedQualtiyEntity.CreateTime = articleQualityEntity.CreateTime;
						addedQualtiyEntity.CreateUserId = articleQualityEntity.CreateUserId;

						addedQualtiyEntity.UpdateTime = DateTime.Now;
						addedQualtiyEntity.UpdateUserId = this._user.Id;

						response = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.Update(addedQualtiyEntity);
					}

					// - save External Status History
					if(oldArticleEntity.Freigabestatus?.Trim()?.ToLower() != newAricleEntity.Freigabestatus?.Trim()?.ToLower())
					{
						Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.Insert(
							new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity
							{
								Artikel_Nr = this._data.ArticleID,
								Datum_Anderung = DateTime.Now,
								ID = -1, // not needed coz AutoIncrement
								Anderungsbereich = $"Freigabestatus changed from [{oldArticleEntity.Freigabestatus}] to [{newAricleEntity.Freigabestatus}]",
								Anderungsbeschreibung = this._data.Freigabestatus_ChangeReason,
								Anderung_von = this._user.Username
							});
					}

					// - save Internal Status History
					if(oldArticleEntity.FreigabestatusTNIntern?.Trim()?.ToLower() != newAricleEntity.FreigabestatusTNIntern?.Trim()?.ToLower())
					{
						Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.Insert(
							new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity
							{
								Artikel_Nr = this._data.ArticleID,
								Datum_Anderung = DateTime.Now,
								ID = -1, // not needed coz AutoIncrement
								Anderungsbereich = $"Freigabestatus_TN_Intern changed from [{oldArticleEntity.FreigabestatusTNIntern}] to [{newAricleEntity.FreigabestatusTNIntern}]",
								Anderungsbeschreibung = this._data.Freigabestatus_TN_intern_ChangeReason,
								Anderung_von = this._user.Username
							});
					}

					// - save Internal Status History
					if(oldArticleEntity.UBG != newAricleEntity.UBG)
					{
						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
							ObjectLogHelper.getLog(this._user, this._data.ArticleID, "UBG", $"{oldArticleEntity.UBG}", $"{newAricleEntity.UBG}", Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));
					}
					// - save CoC History
					if(oldArticleEntity.COF_Pflichtig != newAricleEntity.COF_Pflichtig)
					{
						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
							ObjectLogHelper.getLog(this._user, this._data.ArticleID, "COC", $"{oldArticleEntity.COF_Pflichtig} - {oldArticleEntity.CocVersion}", $"{newAricleEntity.COF_Pflichtig} - {newAricleEntity.CocVersion}", Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));
					}
					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArticleID);

					// -
					return ResponseModel<int>.SuccessResponse(response);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleID);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}
			if(articleEntity.aktiv.HasValue && !articleEntity.aktiv.Value)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "2", Value = "Article is not Active"}
					}
				};
			}

			// - 2022-10-26 - MHD Tage is required when MHD is active
			if(this._data.MHD == true && string.IsNullOrWhiteSpace(this._data.Zeitraum_MHD))
			{
				return ResponseModel<int>.FailureResponse("MHD Tage is required");
			}

			// - Feedback v2 - Quality Attachments are no longer required
			//List<string> NullAttachement = ValidateAttachement();
			//if (NullAttachement != null && NullAttachement.Count > 0)
			//{
			//    return new ResponseModel<int>()
			//    {
			//        Success = false,
			//        Errors = new List<ResponseModel<int>.ResponseError>() {
			//            new ResponseModel<int>.ResponseError {Key = "3", Value = $"You must upload Conformity Files for these values [{String.Join(",", NullAttachement)}]"}
			//        }
			//    };
			//}

			// - 2023-08-24 - CoC
			if(_data.COF_Pflichtig == null)
			{
				return ResponseModel<int>.FailureResponse(new List<string> { $"CoC : invalid value [{_data.COF_Pflichtig}]" });
			}
			else
			{
				if(_data.COF_Pflichtig == true && string.IsNullOrEmpty(_data.CocVersion))
				{
					return ResponseModel<int>.FailureResponse(new List<string> { $"CoC Version: invalid value [{_data.CocVersion}]" });
				}
			}

			//List<string> fregibatusList = new List<string> { "P" /*,"F", "FT", "X", "O"*/ };
			//var bcrList = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.GetOpenWArtikel(articleEntity.ArtikelNr);

			//if(!fregibatusList.Contains(_data.Freigabestatus) && fregibatusList.Contains(articleEntity.Freigabestatus) && bcrList?.Count > 0)
			//{
			//	return ResponseModel<int>.FailureResponse($"BCR : Artikel '{articleEntity.ArtikelNummer}' has open Bom change request .");

			//}


			return ResponseModel<int>.SuccessResponse();
		}
		public List<string> ValidateAttachement()
		{
			//var articleQualityEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data.ArticleID);
			List<string> _nullAttachement = new List<string>();
			//if (articleQualityEntity!=null)
			//{
			if(!VerifyAttachementExsist(this._data.TSP_Available, this._data.TSP_Available_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.TSP_Available.GetDescription());

			if(!VerifyAttachementExsist(this._data.PackagingRegulation_Available, this._data.PackagingRegulation_Available_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.PackagingRegulation_Available.GetDescription());

			if(!VerifyAttachementExsist(this._data.QSV, this._data.QSV_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.QSV.GetDescription());

			if(!VerifyAttachementExsist(this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific, this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.PurchasingArticleInspection__SpecialArticlesCustomerSpecific.GetDescription());

			if(!VerifyAttachementExsist(this._data.SpecialCustomerReleases__DeviationReleases, this._data.SpecialCustomerReleases__DeviationReleases_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.QSV.GetDescription());

			if(!VerifyAttachementExsist(this._data.UL_zertifiziert, this._data.UL_zertifiziert_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.UL_zertifiziert.GetDescription());

			if(!VerifyAttachementExsist(this._data.UL_Etikett, this._data.UL_Etikett_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.UL_Etikett.GetDescription());

			if(!VerifyAttachementExsist(this._data.Webshop, this._data.LLE_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.LLE.GetDescription());

			//to be seen
			//if (!VerifyAttachementExsist(this._data.ESD_Schutz, articleQualityEntity.ESD_AttachmentId))
			//    _nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.ESD_Schutz.GetDescription());

			if(!VerifyAttachementExsist(this._data.Hubmastleitungen, this._data.HM_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.HM.GetDescription());

			if(!VerifyAttachementExsist(this._data.ROHS_EEE_Confirmity, this._data.ROHS_EEE_Confirmity_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.ROHS_EEE_Confirmity.GetDescription());

			if(!VerifyAttachementExsist(this._data.Minerals_Confirmity, this._data.MineralsConfirmity_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.MineralsConfirmity.GetDescription());

			if(!VerifyAttachementExsist(this._data.REACH_SVHC_Confirmity, this._data.REACH_SVHC_Confirmity_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.REACH_SVHC_Confirmity.GetDescription());

			if(!VerifyAttachementExsist(this._data.Dienstelistung, this._data.Dienstleistung_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.Dienstleistung.GetDescription());

			if(!VerifyAttachementExsist(this._data.MHD, this._data.MHD_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.MHD.GetDescription());

			if(!VerifyAttachementExsist(this._data.COF_Pflichtig, this._data.CoC_Pflichtig_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.CoC_Pflichtig.GetDescription());

			if(!VerifyAttachementExsist(this._data.EMPB, this._data.EMPB_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.EMPB.GetDescription());

			if(!VerifyAttachementExsist(this._data.EMPB_Freigegeben, this._data.EMPB_Freigegeben_AttachmentId))
				_nullAttachement.Add(Enums.ArticleEnums.ArticleQualityAttachments.EMPB_Freigegeben.GetDescription());

			//}
			return _nullAttachement;
		}
		public void EliminateEmptyAttachement()
		{
			var articleQualityEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data.ArticleID);
			if(this._data.TSP_Available.HasValue && !this._data.TSP_Available.Value && this._data.TSP_Available_AttachmentId != null)
			{
				this._data.TSP_Available_AttachmentId = null;
				if(articleQualityEntity?.TSP_Available_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.TSP_Available_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.TSP_Available_AttachmentId);
			}

			if(this._data.PackagingRegulation_Available.HasValue && !this._data.PackagingRegulation_Available.Value && this._data.PackagingRegulation_Available_AttachmentId != null)
			{
				this._data.PackagingRegulation_Available_AttachmentId = null;
				if(articleQualityEntity?.PackagingRegulation_Available_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.PackagingRegulation_Available_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.PackagingRegulation_Available_AttachmentId);
			}

			if(this._data.QSV.HasValue && !this._data.QSV.Value && this._data.QSV_AttachmentId != null)
			{
				this._data.QSV_AttachmentId = null;
				if(articleQualityEntity?.QSV_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.QSV_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.QSV_AttachmentId);
			}

			if(this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific.HasValue && !this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific.Value &&
				this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId != null)
			{
				this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId = null;
				if(articleQualityEntity?.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
			}

			if(this._data.SpecialCustomerReleases__DeviationReleases.HasValue && !this._data.SpecialCustomerReleases__DeviationReleases.Value
				&& this._data.SpecialCustomerReleases__DeviationReleases_AttachmentId != null)
			{
				this._data.SpecialCustomerReleases__DeviationReleases_AttachmentId = null;
				if(articleQualityEntity?.SpecialCustomerReleases__DeviationReleases_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.SpecialCustomerReleases__DeviationReleases_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.SpecialCustomerReleases__DeviationReleases_AttachmentId);
			}

			if(this._data.UL_zertifiziert.HasValue && !this._data.UL_zertifiziert.Value
				&& this._data.UL_zertifiziert_AttachmentId != null)
			{
				this._data.UL_zertifiziert_AttachmentId = null;
				if(articleQualityEntity?.UL_zertifiziert_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.UL_zertifiziert_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.UL_zertifiziert_AttachmentId);
			}

			if(this._data.UL_Etikett.HasValue && !this._data.UL_Etikett.Value && this._data.UL_Etikett_AttachmentId != null)
			{
				this._data.UL_Etikett_AttachmentId = null;
				if(articleQualityEntity?.UL_Etikett_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.UL_Etikett_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.UL_Etikett_AttachmentId);
			}

			if(this._data.Webshop.HasValue && !this._data.Webshop.Value && this._data.LLE_AttachmentId != null)
			{
				this._data.LLE_AttachmentId = null;
				if(articleQualityEntity?.LLE_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.LLE_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.LLE_AttachmentId);
			}

			if(this._data.Hubmastleitungen.HasValue && !this._data.Hubmastleitungen.Value && this._data.HM_AttachmentId != null)
			{
				this._data.HM_AttachmentId = null;
				if(articleQualityEntity?.HM_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.HM_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.HM_AttachmentId);
			}

			if(this._data.ROHS_EEE_Confirmity.HasValue && !this._data.ROHS_EEE_Confirmity.Value && this._data.ROHS_EEE_Confirmity_AttachmentId != null)
			{
				this._data.ROHS_EEE_Confirmity_AttachmentId = null;
				if(articleQualityEntity?.ROHS_EEE_Confirmity_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.ROHS_EEE_Confirmity_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.ROHS_EEE_Confirmity_AttachmentId);
			}

			if(this._data.Minerals_Confirmity.HasValue && !this._data.Minerals_Confirmity.Value && this._data.MineralsConfirmity_AttachmentId != null)
			{
				this._data.MineralsConfirmity_AttachmentId = null;
				if(articleQualityEntity?.MineralsConfirmity_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.MineralsConfirmity_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.MineralsConfirmity_AttachmentId);
			}

			if(this._data.REACH_SVHC_Confirmity.HasValue && !this._data.REACH_SVHC_Confirmity.Value && this._data.REACH_SVHC_Confirmity_AttachmentId != null)
			{
				this._data.REACH_SVHC_Confirmity_AttachmentId = null;
				if(articleQualityEntity?.REACH_SVHC_Confirmity_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.REACH_SVHC_Confirmity_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.REACH_SVHC_Confirmity_AttachmentId);
			}

			if(this._data.Dienstelistung.HasValue && !this._data.Dienstelistung.Value && this._data.Dienstleistung_AttachmentId != null)
			{
				this._data.Dienstleistung_AttachmentId = null;
				if(articleQualityEntity?.Dienstleistung_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.Dienstleistung_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.Dienstleistung_AttachmentId);
			}

			if(this._data.MHD.HasValue && !this._data.MHD.Value && this._data.MHD_AttachmentId != null)
			{
				this._data.MHD_AttachmentId = null;
				if(articleQualityEntity?.MHD_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.MHD_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.MHD_AttachmentId);
			}

			if(this._data.COF_Pflichtig.HasValue && !this._data.COF_Pflichtig.Value && this._data.CoC_Pflichtig_AttachmentId != null)
			{
				this._data.CoC_Pflichtig_AttachmentId = null;
				if(articleQualityEntity?.CoC_Pflichtig_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.CoC_Pflichtig_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.CoC_Pflichtig_AttachmentId);
			}

			if(this._data.EMPB.HasValue && !this._data.EMPB.Value && this._data.EMPB_AttachmentId != null)
			{
				this._data.EMPB_AttachmentId = null;
				if(articleQualityEntity?.EMPB_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.EMPB_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.EMPB_AttachmentId);
			}

			if(this._data.EMPB_Freigegeben.HasValue && !this._data.EMPB_Freigegeben.Value && this._data.EMPB_Freigegeben_AttachmentId != null)
			{
				this._data.EMPB_Freigegeben_AttachmentId = null;
				if(articleQualityEntity?.EMPB_Freigegeben_AttachmentId != null)
				{
					Psz.Core.Common.Helpers.ImageFileHelper.DeleteFile((int)articleQualityEntity?.EMPB_Freigegeben_AttachmentId);
				}
				//Infrastructure.Data.Access.Tables.FileAccess.Delete((int)this._data.EMPB_Freigegeben_AttachmentId);
			}
		}
		public bool VerifyAttachementExsist(bool? value, int? attachemntID)
		{
			if((value.HasValue && value.Value) && (!attachemntID.HasValue))
				return false;
			else
				return true;
		}
		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleID);
			var articleExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data.ArticleID);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;


			/*
             UL_zertifiziert = articleEntity.ULzertifiziert;
                UL_Etikett = articleEntity.ULEtikett;
                Webshop = articleEntity.Webshop;
                ESD_Schutz = articleEntity.ESD_Schutz;
                ESD_Schutz_Text = ((articleEntity.ESD_Schutz_Text == null || articleEntity.ESD_Schutz_Text == "") && (articleEntity.ESD_Schutz == null || articleEntity.ESD_Schutz.HasValue && !articleEntity.ESD_Schutz.Value)) ? BaseData.Enums.ArticleEnums.ESDSchutz.Keine.GetDescription() : articleEntity.ESD_Schutz_Text;
                Hubmastleitungen = articleEntity.Hubmastleitungen;
                ROHS_EEE_Confirmity = articleEntity.ROHSEEEConfirmity;
                Minerals_Confirmity = articleEntity.MineralsConfirmity;
                REACH_SVHC_Confirmity = articleEntity.REACHSVHCConfirmity;
                Dienstelistung = articleEntity.Dienstelistung.HasValue ? ConvertIntToBool(articleEntity.Dienstelistung.Value) : false;
                MHD = articleEntity.MHD;
                Zeitraum_MHD = articleEntity.Zeitraum_MHD;
                COF_Pflichtig = articleEntity.COF_Pflichtig;
                EMPB = articleEntity.EMPB;
                EMPB_Freigegeben = articleEntity.EMPB_Freigegeben;
                Freigabestatus = articleEntity.Freigabestatus;
                Prufstatus_TN_Ware = articleEntity.PrufstatusTNWare;
                Freigabestatus_TN_intern = articleEntity.FreigabestatusTNIntern;
             */
			if(articleEntity.Kanban != this._data.Kanban)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Kanban", articleEntity.Kanban?.ToString(), this._data.Kanban?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.ULEtikett != this._data.UL_Etikett)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "UL Etikett", articleEntity.ULEtikett?.ToString(), this._data.UL_Etikett?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.ULzertifiziert != this._data.UL_zertifiziert)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "UL Zertifiziert", articleEntity.ULzertifiziert?.ToString(), this._data.UL_zertifiziert?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Webshop != this._data.Webshop)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Webshop", articleEntity.Webshop?.ToString(), this._data.Webshop?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.ESD_Schutz_Text?.Trim().ToLower() != this._data.ESD_Schutz_Text?.Trim().ToLower())
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "ESD Schutz", articleEntity.ESD_Schutz_Text?.ToString(), this._data.ESD_Schutz_Text?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Hubmastleitungen != this._data.Hubmastleitungen)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Hubmastleitungen", articleEntity.Hubmastleitungen?.ToString(), this._data.Hubmastleitungen?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.ROHSEEEConfirmity != this._data.ROHS_EEE_Confirmity)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "ROHS EEE Confirmity", articleEntity.ROHSEEEConfirmity?.ToString(), this._data.ROHS_EEE_Confirmity?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.MineralsConfirmity != this._data.Minerals_Confirmity)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Minerals Confirmity", articleEntity.MineralsConfirmity?.ToString(), this._data.Minerals_Confirmity?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.REACHSVHCConfirmity != this._data.REACH_SVHC_Confirmity)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "REACH SVHC Confirmity", articleEntity.REACHSVHCConfirmity?.ToString(), this._data.REACH_SVHC_Confirmity?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Dienstelistung != (this._data.Dienstelistung.HasValue ? (this._data.Dienstelistung.Value == true ? 1 : 0) : (int?)null))
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Dienstelistung", articleEntity.Dienstelistung?.ToString(), this._data.Dienstelistung?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.MHD != this._data.MHD)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "MHD", articleEntity.MHD?.ToString(), this._data.MHD?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Zeitraum_MHD?.Trim().ToLower() != this._data.Zeitraum_MHD?.Trim().ToLower())
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Zeitraum MHD", articleEntity.Zeitraum_MHD, this._data.Zeitraum_MHD, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.COF_Pflichtig != this._data.COF_Pflichtig)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "COC Pflichtig", articleEntity.COF_Pflichtig?.ToString(), this._data.COF_Pflichtig?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.EMPB != this._data.EMPB)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "EMPB", articleEntity.EMPB?.ToString(), this._data.EMPB?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.EMPB_Freigegeben != this._data.EMPB_Freigegeben)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "EMPB Freigegeben", articleEntity.EMPB_Freigegeben?.ToString(), this._data.EMPB_Freigegeben?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			// - 
			if(articleEntity.Freigabestatus != this._data.Freigabestatus)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Freigabestatus", articleEntity.Freigabestatus, this._data.Freigabestatus, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleEntity.PrufstatusTNWare != this._data.Prufstatus_TN_Ware)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Prufstatus TN Ware", articleEntity.PrufstatusTNWare, this._data.Prufstatus_TN_Ware, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleEntity.FreigabestatusTNIntern != this._data.Freigabestatus_TN_intern)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Freigabestatus TN intern", articleEntity.FreigabestatusTNIntern, this._data.Freigabestatus_TN_intern, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			// - 2024-12-13
			if(articleEntity.DeliveryNoteCustomerComments?.ToLower()?.Trim() != this._data.DeliveryNoteCustomerComments?.ToLower()?.Trim())
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Delivery Note Customer Comments", articleEntity.DeliveryNoteCustomerComments, this._data.DeliveryNoteCustomerComments, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleExtension != null)
			{
				if(articleExtension.TSP_Available != this._data.TSP_Available)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "TSP Available", articleExtension.TSP_Available?.ToString(), this._data.TSP_Available?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
				if(articleExtension.PackagingRegulation_Available != this._data.PackagingRegulation_Available)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Packaging Regulation Available", articleExtension.PackagingRegulation_Available?.ToString(), this._data.PackagingRegulation_Available?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
				if(articleExtension.QSV != this._data.QSV)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "QSV", articleExtension.QSV?.ToString(), this._data.QSV?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
				if(articleExtension.PurchasingArticleInspection__SpecialArticlesCustomerSpecific != this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Purchasing Article Inspection / SpecialArticles Customer Specific", articleExtension.PurchasingArticleInspection__SpecialArticlesCustomerSpecific?.ToString(), this._data.PurchasingArticleInspection__SpecialArticlesCustomerSpecific?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
				if(articleExtension.SpecialCustomerReleases__DeviationReleases != this._data.SpecialCustomerReleases__DeviationReleases)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArticleID, "Special Customer Release / Deviation Releases", articleExtension.SpecialCustomerReleases__DeviationReleases?.ToString(), this._data.SpecialCustomerReleases__DeviationReleases?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
			}
			else
			{
				//if (this._data.CopperCostBasis.HasValue)
				//{
				//    logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis", "", this._data.CopperCostBasis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				//}
				//if (this._data.CopperCostBasis150.HasValue)
				//{
				//    logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis150", "", this._data.CopperCostBasis150.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				//}
			}
			//
			return logs;
		}
	}
}
