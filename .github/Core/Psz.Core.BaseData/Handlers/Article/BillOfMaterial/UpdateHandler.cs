using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UpdateHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.BOMStatusDetailsModel _data { get; set; }
		public UpdateHandler(UserModel user, Models.Article.BillOfMaterial.BOMStatusDetailsModel bomstatus)
		{
			_user = user;
			_data = bomstatus;
		}
		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				#region // -- transaction-based logic -- //
				//TODO: - insert process here
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// 
				botransaction.beginTransaction();

				var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArticleId, botransaction.connection, botransaction.transaction);
				var entity = this._data.ToEntity();
				entity.LastUpdateTime = DateTime.Now;
				entity.LastUpdateUserId = this._user.Id;

				if(articleExtEntity == null)
				{
					entity.BomVersion = 0;
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditCPRequirement(this._data.ArticleId, true, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.InsertWithTransaction(entity, botransaction.connection, botransaction.transaction);
				}


				entity.Id = articleExtEntity.Id;
				// -- if Bom was VALIDATED, change it to InPrep & Increment Version
				if(articleExtEntity.BomStatusId == (int)Enums.ArticleEnums.BomStatus.Approved)
				{
					entity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation;
					entity.BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription();
					entity.BomVersion = articleExtEntity.BomVersion + 1;

					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
						new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
                                    // - Status change log
                                    ObjectLogHelper.getLog(this._user, this._data.ArticleId, "Article BOM Status from", $"{Enums.ArticleEnums.BomStatus.Approved.GetDescription()}",
									$"{Enums.ArticleEnums.BomStatus.InPreparation.GetDescription()}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit),
                                    // - Version change log
                                    ObjectLogHelper.getLog(this._user, this._data.ArticleId, "Article BOM Version from", $"{articleExtEntity.BomVersion}",
									$"{entity.BomVersion}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit)
						}, botransaction.connection, botransaction.transaction);

					// -- BOM level logging
					Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(
						new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> {
                                    // - Status change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									Enums.ArticleEnums.BomStatus.Approved.GetDescription(), Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(), Enums.ObjectLogEnums.BOMLogType.StatusChange),
                                    // - Version change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									articleExtEntity.BomVersion?.ToString(), entity.BomVersion?.ToString(), Enums.ObjectLogEnums.BOMLogType.Version)
						}, botransaction.connection, botransaction.transaction);

					// -- Alert PM, that a new version of BOM is InPrep
					#region Email Notifications
					var addresses = new List<string>();
					//addresses.Add("sani.chaibousalaou@psz-electronic.com");

					// -- Article PM
					addresses.Add(
						Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(
						Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByArtikelNr(this._data.ArticleId, botransaction.connection, botransaction.transaction)
						?.UserId ?? -1, botransaction.connection, botransaction.transaction)
						?.Email);

					// - get Eng team by Production Site
					addresses.AddRange(ApplyChangesHandler.getEmailUserByArticleProductionSite(articleExtEntity.ArticleId, articleExtEntity.ArticleNumber, botransaction));

					var title = $"[{articleExtEntity.ArticleNumber}] New BOM In Preparation";
					var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
					+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just started preparing BOM v{articleExtEntity.BomVersion} for article <strong>{articleExtEntity.ArticleNumber?.ToUpper()}</strong>."
					+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleExtEntity.ArticleId}/bom'>here</a>"
					+ "<br/><br/>Regards, <br/>IT Department </div>";

					try
					{
						sendEmailNotification(title, content, addresses);
					} catch(Exception exm)
					{
						Infrastructure.Services.Logging.Logger.Log(exm);
					}
					#endregion Email Notifications
				}

				if(this._data.BomValidFrom.HasValue && (articleExtEntity.BomValidFrom != this._data.BomValidFrom || articleExtEntity.BomValidFrom.Value.Date != this._data.BomValidFrom.Value.Date)) //
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
						ObjectLogHelper.getLog(this._user, this._data.ArticleId, "Article BOM Valid from", $"{articleExtEntity.BomValidFrom?.ToString("dd/MM/yyyy")}",
						$"{this._data.BomValidFrom?.ToString("dd/MM/yyyy")}",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit), botransaction.connection, botransaction.transaction);

					// -- BOM level logging
					Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(
						new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> {
                                    // - Status change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									$"{articleExtEntity.BomValidFrom?.ToString("dd/MM/yyyy")}", $"{this._data.BomValidFrom?.ToString("dd/MM/yyyy")}", Enums.ObjectLogEnums.BOMLogType.ValidFrom)
						}, botransaction.connection, botransaction.transaction);
				}

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data.ArticleId, botransaction.connection, botransaction.transaction);
				if(this._data.CP_required.HasValue && this._data.CP_required != articleEntity.CP_required)
				{
					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
						new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
                                    // - Status change log
                                    ObjectLogHelper.getLog(this._user, this._data.ArticleId, "Article BOM CP required from", $"{articleEntity.CP_required}",
									$"{this._data.CP_required.Value}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit)
						}, botransaction.connection, botransaction.transaction);

					// -- BOM level logging
					Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(
						new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> {
                                    // - Status change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									$"{articleEntity.CP_required}",
									$"{this._data.CP_required.Value}", Enums.ObjectLogEnums.BOMLogType.CPRequired)
						}, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditCPRequirement(this._data.ArticleId, this._data.CP_required.Value, botransaction.connection, botransaction.transaction);
				}


				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.ArticleId);
					// -
				var updated = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.Update(entity);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(updated);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Article not found");

			if(this._data.CP_required == false
				&& (!articleEntity.CP_required.HasValue || articleEntity.CP_required.HasValue && articleEntity.CP_required.Value == true)
				&& Infrastructure.Data.Access.Tables.BSD.CP_snapshot_positionsAccess.ExistsByArticle(this._data.ArticleId) != null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "[Cutting Plan] exists for current article, cannot disable [CP Requirement].");

			if(this._user.IsGlobalDirector != true && this._user.SuperAdministrator != true && articleEntity.IsEDrawing == true && this._user.Access.MasterData.EDrawingEdit != true)
			{
				return ResponseModel<int>.FailureResponse($"Edit aborted: User cannot edit this article because E-Drawing is activated.");
			}

			return ResponseModel<int>.SuccessResponse();
		}

		void sendEmailNotification(string title, string contentHtml, List<string> toEmailAddresses)
		{
			try
			{
				Module.EmailingService.SendEmailAsync(title, contentHtml, toEmailAddresses, null);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(",", Module.EmailingService.EmailParamtersModel.BOMEmailDestinations)}]"));
				Infrastructure.Services.Logging.Logger.Log(ex);
			}
		}
	}
}
