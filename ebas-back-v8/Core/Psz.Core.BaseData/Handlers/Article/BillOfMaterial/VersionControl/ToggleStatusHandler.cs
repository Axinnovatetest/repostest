using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl
{
	public class ToggleStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<bool>>
	{
		private UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.VersionControl.ToggleStatusRequestModel _data { get; set; }
		public ToggleStatusHandler(UserModel user, Models.Article.BillOfMaterial.VersionControl.ToggleStatusRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<bool> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var articleValidationEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.Get(this._data.Id);
			var _engineeringFull = articleValidationEntity.EngineeringValidationFull;
			switch(this._data.Status)
			{
				case Enums.ArticleEnums.BOMControlStatus.All:
					break;
				case Enums.ArticleEnums.BOMControlStatus.EngineeringControl:
					articleValidationEntity.EngineeringControl = !articleValidationEntity.EngineeringControl;
					articleValidationEntity.EngineeringControlEditDate = DateTime.Now;
					articleValidationEntity.EngineeringControlEditUserId = this._user.Id;
					articleValidationEntity.EngineeringControlEditUserName = this._user.Name;
					articleValidationEntity.EngineeringControlEditUserEmail = this._user.Email;
					// - update full validation(s)
					updateFullValidations(articleValidationEntity);
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
						$"Article BOM / CP Control - {Enums.ArticleEnums.BOMControlStatus.EngineeringControl.GetDescription()}",
						$"{!articleValidationEntity.EngineeringControl}",
						$"{articleValidationEntity.EngineeringControl}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
					break;
				case Enums.ArticleEnums.BOMControlStatus.EngineeringUpdate:
					articleValidationEntity.EngineeringUpdate = !articleValidationEntity.EngineeringUpdate;
					articleValidationEntity.EngineeringUpdateEditDate = DateTime.Now;
					articleValidationEntity.EngineeringUpdateEditUserId = this._user.Id;
					articleValidationEntity.EngineeringUpdateEditUserName = this._user.Name;
					articleValidationEntity.EngineeringUpdateEditUserEmail = this._user.Email;
					// - update full validation(s)
					updateFullValidations(articleValidationEntity);
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
						$"Article BOM / CP Control - {Enums.ArticleEnums.BOMControlStatus.EngineeringUpdate.GetDescription()}",
						$"{!articleValidationEntity.EngineeringUpdate}",
						$"{articleValidationEntity.EngineeringUpdate}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
					break;
				case Enums.ArticleEnums.BOMControlStatus.EngineeringPrint:
					articleValidationEntity.EngineeringPrint = !articleValidationEntity.EngineeringPrint;
					articleValidationEntity.EngineeringPrintEditDate = DateTime.Now;
					articleValidationEntity.EngineeringPrintEditUserId = this._user.Id;
					articleValidationEntity.EngineeringPrintEditUserName = this._user.Name;
					articleValidationEntity.EngineeringPrintEditUserEmail = this._user.Email;
					// - update full validation(s)
					updateFullValidations(articleValidationEntity);
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
						$"Article BOM / CP Control - {Enums.ArticleEnums.BOMControlStatus.EngineeringPrint.GetDescription()}",
						$"{!articleValidationEntity.EngineeringPrint}",
						$"{articleValidationEntity.EngineeringPrint}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
					break;
				case Enums.ArticleEnums.BOMControlStatus.EngineeringDistribution:
					articleValidationEntity.EngineeringDistribution = !articleValidationEntity.EngineeringDistribution;
					articleValidationEntity.EngineeringDistributionEditDate = DateTime.Now;
					articleValidationEntity.EngineeringDistributionEditUserId = this._user.Id;
					articleValidationEntity.EngineeringDistributionEditUserName = this._user.Name;
					articleValidationEntity.EngineeringDistributionEditUserEmail = this._user.Email;
					// - update full validation(s)
					updateFullValidations(articleValidationEntity);
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
						$"Article BOM / CP Control - {Enums.ArticleEnums.BOMControlStatus.EngineeringDistribution.GetDescription()}",
						$"{!articleValidationEntity.EngineeringDistribution}",
						$"{articleValidationEntity.EngineeringDistribution}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
					break;
				case Enums.ArticleEnums.BOMControlStatus.QualityControl:
					articleValidationEntity.QualityControl = !articleValidationEntity.QualityControl;
					articleValidationEntity.QualityControlEditDate = DateTime.Now;
					articleValidationEntity.QualityControlEditUserId = this._user.Id;
					articleValidationEntity.QualityControlEditUserName = this._user.Name;
					articleValidationEntity.QualityControlEditUserEmail = this._user.Email;
					// - update full validation(s)
					updateFullValidations(articleValidationEntity);
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
						$"Article BOM / CP Control - {Enums.ArticleEnums.BOMControlStatus.QualityControl.GetDescription()}",
						$"{!articleValidationEntity.QualityControl}",
						$"{articleValidationEntity.QualityControl}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
					break;
				case Enums.ArticleEnums.BOMControlStatus.QualityUpdate:
					articleValidationEntity.QualityUpdate = !articleValidationEntity.QualityUpdate;
					articleValidationEntity.QualityUpdateEditDate = DateTime.Now;
					articleValidationEntity.QualityUpdateEditUserId = this._user.Id;
					articleValidationEntity.QualityUpdateEditUserName = this._user.Name;
					articleValidationEntity.QualityUpdateEditUserEmail = this._user.Email;
					// - update full validation(s)
					updateFullValidations(articleValidationEntity);
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
						$"Article BOM / CP Control - {Enums.ArticleEnums.BOMControlStatus.QualityUpdate.GetDescription()}",
						$"{!articleValidationEntity.QualityUpdate}",
						$"{articleValidationEntity.QualityUpdate}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
					break;
				case Enums.ArticleEnums.BOMControlStatus.QualityPrint:
					articleValidationEntity.QualityPrint = !articleValidationEntity.QualityPrint;
					articleValidationEntity.QualityPrintEditDate = DateTime.Now;
					articleValidationEntity.QualityPrintEditUserId = this._user.Id;
					articleValidationEntity.QualityPrintEditUserName = this._user.Name;
					articleValidationEntity.QualityPrintEditUserEmail = this._user.Email;
					// - update full validation(s)
					updateFullValidations(articleValidationEntity);
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
						$"Article BOM / CP Control - {Enums.ArticleEnums.BOMControlStatus.QualityPrint.GetDescription()}",
						$"{!articleValidationEntity.QualityPrint}",
						$"{articleValidationEntity.QualityPrint}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
					break;
				case Enums.ArticleEnums.BOMControlStatus.QualityDistribution:
					articleValidationEntity.QualityDistribution = !articleValidationEntity.QualityDistribution;
					articleValidationEntity.QualityDistributionEditDate = DateTime.Now;
					articleValidationEntity.QualityDistributionEditUserId = this._user.Id;
					articleValidationEntity.QualityDistributionEditUserName = this._user.Name;
					articleValidationEntity.QualityDistributionEditUserEmail = this._user.Email;
					// - update full validation(s)
					updateFullValidations(articleValidationEntity);
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
						$"Article BOM / CP Control - {Enums.ArticleEnums.BOMControlStatus.QualityDistribution.GetDescription()}",
						$"{!articleValidationEntity.QualityDistribution}",
						$"{articleValidationEntity.QualityDistribution}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
					break;
				default:
					break;
			}

			// - Email Quality when articles passes to Full Engineering validation
			if(_engineeringFull == false && articleValidationEntity.EngineeringValidationFull == true)
			{
				var title = $"[{articleValidationEntity.ArticleNumber}] BOM Change Control";
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
					+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just validated BOM v{articleValidationEntity.ArticleBOMVersion} for article <strong>{articleValidationEntity.ArticleNumber?.ToUpper()}</strong>.<br/>Please consider preparing the corresponding Quality Checklist ASAP."
					+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles'>here</a>"
					+ "<br/><br/>Regards, <br/>IT Department </div>";

				var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetBomCpControl_Quality()
							?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
							.Select(x => x.UserEmail)?.ToList();

				Module.EmailingService.SendEmailAsync(title, content, addresses, null);
			}

			// -
			if(articleValidationEntity.ValidationFull)
			{
				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					ObjectLogHelper.getLog(this._user, this._data.ArticleNr,
					$"Article BOM / CP Control - Full",
					$"false",
					$"true",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Edit));
			}
			return ResponseModel<bool>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.Update(articleValidationEntity) > 0);
		}

		private static void updateFullValidations(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity articleValidationEntity)
		{
			articleValidationEntity.EngineeringValidationFull = articleValidationEntity.EngineeringControl
									&& articleValidationEntity.EngineeringUpdate
									&& articleValidationEntity.EngineeringPrint
									&& articleValidationEntity.EngineeringDistribution;

			articleValidationEntity.QualityValidationFull = articleValidationEntity.QualityControl
									&& articleValidationEntity.QualityUpdate
									&& articleValidationEntity.QualityPrint
									&& articleValidationEntity.QualityDistribution;
			// -
			articleValidationEntity.ValidationFull = articleValidationEntity.EngineeringValidationFull && articleValidationEntity.QualityValidationFull;
		}

		public ResponseModel<bool> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleNr) == null)
				return ResponseModel<bool>.FailureResponse("Article not found. Please refresh the page and try again.");

			var validationEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.Get(this._data.Id);
			if(validationEntity == null)
				return ResponseModel<bool>.FailureResponse("Article control not found. Please refresh the page and try again.");

			if( // - Quality control started
				(validationEntity.QualityControl == true
				|| validationEntity.QualityDistribution == true
				|| validationEntity.QualityPrint == true
				|| validationEntity.QualityUpdate == true)
				&& // - toggling Engineering control
				(this._data.Status == Enums.ArticleEnums.BOMControlStatus.EngineeringControl
				|| this._data.Status == Enums.ArticleEnums.BOMControlStatus.EngineeringUpdate
				|| this._data.Status == Enums.ArticleEnums.BOMControlStatus.EngineeringPrint
				|| this._data.Status == Enums.ArticleEnums.BOMControlStatus.EngineeringDistribution)
				)
			{
				return ResponseModel<bool>.FailureResponse("Article already validated from Quality. Please remove Quality controls and try again.");
			}

			if(
				(this._data.Status == Enums.ArticleEnums.BOMControlStatus.QualityControl
				|| this._data.Status == Enums.ArticleEnums.BOMControlStatus.QualityUpdate
				|| this._data.Status == Enums.ArticleEnums.BOMControlStatus.QualityPrint
				|| this._data.Status == Enums.ArticleEnums.BOMControlStatus.QualityDistribution)
				&& (validationEntity.EngineeringValidationFull != true))
			{
				return ResponseModel<bool>.FailureResponse("Article not validated from Engineering. Please check Engineering controls and try again.");
			}

			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
