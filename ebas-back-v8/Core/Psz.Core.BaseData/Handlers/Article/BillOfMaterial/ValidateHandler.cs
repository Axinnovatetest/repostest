using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Data.SqlClient;
	using System.Linq;


	[Obsolete("See ValidateWithPartialDocHandler", true)]
	public class ValidateHandler: IHandle<UserModel, ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public ValidateHandler(UserModel user, int articleId)
		{
			_user = user;
			_data = articleId;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				//TODO: - insert process here
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var idsVersions = Module.LagersWithVersionning ?? new List<int> { };
				botransaction.beginTransaction();
				// 
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
				var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data, botransaction.connection, botransaction.transaction);
				if(articleExtEntity == null)
				{
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
						{
							ArticleId = articleEntity.ArtikelNr,
							ArticleDesignation = articleEntity.Bezeichnung1,
							ArticleNumber = articleEntity.ArtikelNummer,
							BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(),
							BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation,
							BomVersion = 0,
						}, botransaction.connection, botransaction.transaction);
					articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data, botransaction.connection, botransaction.transaction);
				}
				articleExtEntity.LastUpdateTime = DateTime.Now;
				articleExtEntity.LastUpdateUserId = this._user.Id;
				articleExtEntity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.Approved;
				articleExtEntity.BomStatus = Enums.ArticleEnums.BomStatus.Approved.GetDescription();


				// -- -1- Save snapshots Bom + ALT Bom
				List<string> errors;
				saveBomSnapshot_PartialValidation(this._data, articleExtEntity.BomVersion, this._user.Id, out errors, botransaction.connection, botransaction.transaction);
				if(errors != null && errors.Count > 0)
					return ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>.FailureResponse(errors);

				// -- -2- Unvalidate CP - To Confirm abt conflict before validation
				if(Infrastructure.Data.Access.Tables.BSD.CAO_DecoupageAccess.ResetValidation(this._data, articleExtEntity.BomVersion ?? -1, 0, botransaction.connection, botransaction.transaction) > 0)
				{
					Infrastructure.Data.Access.Tables.BSD.CAO_Validation_LOGAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity
						{
							artikelnummer = articleEntity.ArtikelNummer,
							date_time = DateTime.Now,
							kunden_index = articleEntity.Index_Kunde,
							username = this._user.Name,
							val_status = "unvalidation after BOM approval"
						}, botransaction.connection, botransaction.transaction);
				}


				// -- -3.1- If first BOM Validation, just upgrade everything
				var bomSnapshotCountByArticle = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetBOMVersionByArticle_Count(this._data, botransaction.connection, botransaction.transaction);
				if(bomSnapshotCountByArticle == 1)
				{
					var nonStartedFA_IDs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetNonStartedByArticle(this._data, botransaction.connection, botransaction.transaction)
						?.Where(x => idsVersions.Exists(y => y == x.Lagerort_id)) // - Update only BETN - for now
						?.Select(x => x.ID)?.ToList();
					if(nonStartedFA_IDs != null && nonStartedFA_IDs.Count > 0)
						UpgradeFABomHandler.upgradeFABOM(botransaction.connection, botransaction.transaction, this._user, this._data, nonStartedFA_IDs, articleExtEntity.BomVersion ?? -1, true);
				}
				// -- -3.2- if CP IS Required for Article, upgrade previous FA without CP to current
				if(articleEntity.CP_required.HasValue && articleEntity.CP_required.Value)
				{
					var nonStartedFAwoCP_IDs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetNonStartedByArticle(this._data, botransaction.connection, botransaction.transaction)
						?.Where(x => idsVersions.Exists(y => y == x.Lagerort_id)) // - Update only BETN - for now
						?.Where(x => !x.CPVersion.HasValue)
						?.Select(x => x.ID)?.ToList();
					if(nonStartedFAwoCP_IDs != null && nonStartedFAwoCP_IDs.Count > 0)
						UpgradeFABomHandler.upgradeFABOM(botransaction.connection, botransaction.transaction, this._user, this._data, nonStartedFAwoCP_IDs, articleExtEntity.BomVersion ?? -1);
				}
				// -- -3.3- Get FA with older Bom and w/ CP, open and non-started
				var potentialUpgradeFA = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetBOMValidationUpgradable(this._data, articleExtEntity.BomVersion, articleEntity.CP_required.HasValue && articleEntity.CP_required.Value, botransaction.connection, botransaction.transaction)
					?.Where(x => idsVersions.Exists(y => y == x.Lagerort_id)); // - Update only BETN - for now



				// -- -5- Validate BOM
				Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.UpdateWithTransaction(articleExtEntity, botransaction.connection, botransaction.transaction);


				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, this._data,
					"Article BOM Status from",
					$"{Enums.ArticleEnums.BomStatus.InPreparation.GetDescription()}",
					$"{Enums.ArticleEnums.BomStatus.Approved.GetDescription()}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Edit), botransaction.connection, botransaction.transaction);

				// -- BOM level logging
				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.InsertWithTransaction(
					Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0,
					articleExtEntity.ArticleNumber, null, null,
					Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(),
					Enums.ArticleEnums.BomStatus.Approved.GetDescription(),
					Enums.ObjectLogEnums.BOMLogType.StatusChange), botransaction.connection, botransaction.transaction);



				//add line in lagerExtension if index not exsist
				var LagerExtension = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndex(_data, articleEntity.Index_Kunde, botransaction.connection, botransaction.transaction);
				if(LagerExtension == null || LagerExtension.Count == 0)
				{
					var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrAndBestand(_data, botransaction.connection, botransaction.transaction)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> { };
					Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.InsertWithTransaction(lagerEntities?.Select(x => x.Lagerort_id ?? -1)
						.Union(Module.HauplagerCTS)?.Select(l => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
						{
							ArtikelNr = _data,
							Bestand = 0m,
							Index_Kunde = articleEntity.Index_Kunde,
							Lagerort_id = l,
							LastEditTime = DateTime.Now,
							LastEditUserId = -2,
						})?.ToList(), botransaction.connection, botransaction.transaction);
				}

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data);

					// -- -4- Send Email notification to PM, Engineering and Quality Mgmt teams
					#region Email Notifications
					var addresses = new List<string>();
					// addresses.Add("sani.chaibousalaou@psz-electronic.com");

					// -- Article PM
					addresses.Add(
						Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
						Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByArtikelNr(this._data)
						?.UserId ?? -1)
						?.Email);

					// -- Eng. guys
					addresses.AddRange(
						(Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetBomCpControl_Engineering()
						?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
						.Select(x => x.UserEmail));

					// - get Eng team by Production Site
					addresses.AddRange(ApplyChangesHandler.getEmailUserByArticleProductionSite(articleExtEntity.ArticleId, articleExtEntity.ArticleNumber));

					var title = $"[{articleExtEntity.ArticleNumber}] BOM Validation";
					var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
						+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

					// - Alert CP Validation guys, if NOT required - if required email will go out after CP validation
					if(articleEntity.CP_required.HasValue && articleEntity.CP_required.Value == true)
					{
						content += $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just validated BOM v{articleExtEntity.BomVersion} for article <strong>{articleExtEntity.ArticleNumber?.ToUpper()}</strong>.<br/>Please consider preparing the corresponding cutting plan ASAP.";
					}
					else
					{
						content += $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just validated BOM v{articleExtEntity.BomVersion} for article <strong>{articleExtEntity.ArticleNumber?.ToUpper()}</strong>.<br/>Please consider preparing the corresponding Quality Checklist ASAP.";
					}

					content += $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleExtEntity.ArticleId}/bom'>here</a>"
						+ "<br/><br/>Regards, <br/>IT Department </div>";

					Module.EmailingService.SendEmailAsync(title, content, addresses, null);
					#endregion Email


					// -
					return ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>.SuccessResponse(
						potentialUpgradeFA
						?.Select(x => new Models.Article.BillOfMaterial.ValidateBomResponseModel(x, articleExtEntity.ArticleNumber))
						?.ToList());
				}
				else
				{
					return ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>.FailureResponse(key: "1", value: "Article not found");

			var bomPositions = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data);
			if(bomPositions == null || bomPositions.Count <= 0)
				return ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>.FailureResponse(key: "1", value: "Cannot validate article with BOM empty");

			var lastBOMSnapshot = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticle(articleEntity.ArtikelNr);
			var lastCP = Infrastructure.Data.Access.Tables.BSD.CAO_DecoupageAccess.GetLastByArticle(articleEntity.ArtikelNr);

			// - given, Article, BOM and CP check control validation if fully complete
			if(Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.CanValidateBom(articleEntity.ArtikelNr, lastBOMSnapshot?.BomVersion ?? 0, lastCP?.CP_version) == false)
				return ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>.FailureResponse(key: "1", value: "Cannot validate article while control not fully validated");

			return ResponseModel<List<Models.Article.BillOfMaterial.ValidateBomResponseModel>>.SuccessResponse();
		}

		void saveBomSnapshot(int articleId, int? version, int userId, out List<string> errors, SqlConnection connection, SqlTransaction transaction)
		{
			errors = new List<string>();
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(articleId, connection, transaction);
			if(articleEntity != null)
			{
				var bomExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(articleId, connection, transaction);
				if(bomExtensionEntity != null)
				{
					var bom = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(articleId, connection, transaction);
					var bomAlt = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBoms(bom?.Select(x => x.Nr)?.ToList(), connection, transaction);
					var snapshotTime = DateTime.Now;

					// -- check if Article exists w same BomVersion
					if(Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.Get_count(articleId, version ?? -1, connection, transaction) > 0
						|| Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAlt_SnapshotAccess.Get_count(articleId, version ?? -1, connection, transaction) > 0)
					{
						errors.Add("Snapshot of Article with the same BomVersion already exists");
						return;
					}

					Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.InsertWithTransaction(bom.Select(x => getBomSnapshot(userId, version ?? -1, articleEntity?.Index_Kunde, articleEntity?.Index_Kunde_Datum, x, snapshotTime))?.ToList(),
						connection, transaction);
					Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAlt_SnapshotAccess.InsertWithTransaction(bomAlt.Select(x => getBomAltSnapshot(userId, version ?? -1, x, snapshotTime))?.ToList(),
						connection, transaction);
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtension_SnapshotAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity
					{
						ArticleId = bomExtensionEntity.ArticleId,
						ArticleDesignation = bomExtensionEntity.ArticleDesignation,
						ArticleNumber = bomExtensionEntity.ArticleNumber,
						BomValidFrom = bomExtensionEntity.BomValidFrom,
						BomVersion = bomExtensionEntity.BomVersion,
						Id = -1,
						KundenIndex = articleEntity.Index_Kunde,
						KundenIndexDate = articleEntity.Index_Kunde_Datum,
						SnapshotTime = snapshotTime,
						SnapshotUserId = userId
					}, connection, transaction);

					// - Save Quality Checks if CP not required
					if(articleEntity.CP_required.HasValue && articleEntity.CP_required.Value == false)
					{
						Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.Init(
							new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity
							{
								ArticleBOMVersion = version ?? 0,
								ArticleCPVersion = null,
								ArticleKundenIndex = articleEntity.Index_Kunde,
								ArticleKundenIndexDatum = articleEntity.Index_Kunde_Datum,
								ArticleNr = articleEntity.ArtikelNr,
								ArticleNumber = articleEntity.ArtikelNummer,
								BOMValidationDate = DateTime.Now,
								CPValidationDate = null
							}, connection, transaction
							);
					}
				}
			}
			else
			{
				errors.Add("Article not found");
			}
		}

		/// <summary>
		/// 2022-07-13
		/// User can chose to only update BOM positions without upgrading the Version Number
		/// In this case, the Snapshot should be overwritten.
		/// </summary>
		/// <param name="articleId"></param>
		/// <param name="version"></param>
		/// <param name="userId"></param>
		/// <param name="errors"></param>
		/// <param name="connection"></param>
		/// <param name="transaction"></param>
		void saveBomSnapshot_PartialValidation(int articleId, int? version, int userId, out List<string> errors, SqlConnection connection, SqlTransaction transaction)
		{
			errors = new List<string>();
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(articleId, connection, transaction);
			if(articleEntity != null)
			{
				var bomExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(articleId, connection, transaction);
				if(bomExtensionEntity != null)
				{
					var bom = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(articleId, connection, transaction);
					var bomAlt = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAltAccess.GetByOriginalBoms(bom?.Select(x => x.Nr)?.ToList(), connection, transaction);
					var snapshotTime = DateTime.Now;

					// -- check if Article exists w same BomVersion - then Archive it
					if(Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.Get_count(articleId, version ?? -1, connection, transaction) > 0
						|| Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAlt_SnapshotAccess.Get_count(articleId, version ?? -1, connection, transaction) > 0)
					{
						Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.OverwriteSnapshotWithTransaction(articleId, version ?? -1, userId, connection, transaction);
						Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAlt_SnapshotAccess.OverwriteSnapshotWithTransaction(articleId, version ?? -1, userId, connection, transaction);
					}

					Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.InsertWithTransaction(bom.Select(x => getBomSnapshot(userId, version ?? -1, articleEntity?.Index_Kunde, articleEntity?.Index_Kunde_Datum, x, snapshotTime))?.ToList(),
						connection, transaction);
					Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAlt_SnapshotAccess.InsertWithTransaction(bomAlt.Select(x => getBomAltSnapshot(userId, version ?? -1, x, snapshotTime))?.ToList(),
						connection, transaction);
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtension_SnapshotAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity
					{
						ArticleId = bomExtensionEntity.ArticleId,
						ArticleDesignation = bomExtensionEntity.ArticleDesignation,
						ArticleNumber = bomExtensionEntity.ArticleNumber,
						BomValidFrom = bomExtensionEntity.BomValidFrom,
						BomVersion = bomExtensionEntity.BomVersion,
						Id = -1,
						KundenIndex = articleEntity.Index_Kunde,
						KundenIndexDate = articleEntity.Index_Kunde_Datum,
						SnapshotTime = snapshotTime,
						SnapshotUserId = userId
					}, connection, transaction);

					// - Save Quality Checks if CP not required
					if(articleEntity.CP_required.HasValue && articleEntity.CP_required.Value == false)
					{
						Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.Init(
							new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity
							{
								ArticleBOMVersion = version ?? 0,
								ArticleCPVersion = null,
								ArticleKundenIndex = articleEntity.Index_Kunde,
								ArticleKundenIndexDatum = articleEntity.Index_Kunde_Datum,
								ArticleNr = articleEntity.ArtikelNr,
								ArticleNumber = articleEntity.ArtikelNummer,
								BOMValidationDate = DateTime.Now,
								CPValidationDate = null
							}, connection, transaction);
					}
				}
			}
			else
			{
				errors.Add("Article not found");
			}
		}

		Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity getBomSnapshot(int userId, int bomVersion, string kundenIndex, DateTime? kundenIndexDate, Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity stucklisten, DateTime snapshotTime)
		{
			if(stucklisten == null)
				return null;

			return new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity
			{
				Anzahl = stucklisten.Anzahl,
				Artikelnummer = stucklisten.Artikelnummer,
				Artikel_Nr = stucklisten.Artikel_Nr,
				Artikel_Nr_des_Bauteils = stucklisten.Artikel_Nr_des_Bauteils,
				Bezeichnung_des_Bauteils = stucklisten.Bezeichnung_des_Bauteils,
				BomVersion = bomVersion,
				DocumentId = stucklisten.DocumentId,
				Nr = -1,
				Position = stucklisten.Position,
				SnapshotTime = snapshotTime,
				SnapshotUserId = userId,
				Variante = stucklisten.Variante,
				Vorgang_Nr = stucklisten.Vorgang_Nr,
				KundenIndex = kundenIndex,
				KundenIndexDate = kundenIndexDate,
			};
		}
		Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity getBomAltSnapshot(int userId, int bomVersion, Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity stucklisten, DateTime snapshotTime)
		{
			if(stucklisten == null)
				return null;

			return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity
			{
				Anzahl = stucklisten.Anzahl,
				ArtikelBezeichnung = stucklisten.ArtikelBezeichnung,
				ArtikelNr = stucklisten.ArtikelNr,
				ArtikelNummer = stucklisten.ArtikelNummer,
				BomVersion = bomVersion,
				DocumentId = stucklisten.DocumentId,
				LastUpdateTime = stucklisten.LastUpdateTime,
				LastUpdateUserId = stucklisten.LastUpdateUserId,
				Nr = -1,
				OriginalStucklistenNr = stucklisten.OriginalStucklistenNr,
				ParentArtikelNr = stucklisten.ParentArtikelNr,
				Position = stucklisten.Position,
				SnapshotTime = snapshotTime,
				SnapshotUserId = userId
			};
		}
	}
}
