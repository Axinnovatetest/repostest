using Psz.Core.BaseData.Models.Article.BillOfMaterial;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class ValidateBomWithPartialDocAndBCRHandler: IHandle<UserModel, ResponseModel<List<ValidateBomResponseWithBcrModel>>>
	{
		private UserModel _user { get; set; }
		private ValidateBomWBcrPartialDocRequestModel _data { get; set; }

		public ValidateBomWithPartialDocAndBCRHandler(UserModel user, ValidateBomWBcrPartialDocRequestModel data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<List<ValidateBomResponseWithBcrModel>> Handle()
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

				botransaction.beginTransaction();
				// -
				if(this._data.CanPartialValidation)
				{
					this.decreaseBomVersion(botransaction.connection, botransaction.transaction);
				}

				var versioningLagerIds = Module.LagersWithVersionning ?? new List<int> { };
				// -
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data.ArticleId, botransaction.connection, botransaction.transaction);
				var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArticleId, botransaction.connection, botransaction.transaction);


				//  - Update Article BOM Status
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
					articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArticleId, botransaction.connection, botransaction.transaction);
				}
				articleExtEntity.LastUpdateTime = DateTime.Now;
				articleExtEntity.LastUpdateUserId = this._user.Id;
				articleExtEntity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.Approved;
				articleExtEntity.BomStatus = Enums.ArticleEnums.BomStatus.Approved.GetDescription();


				// -- -1- Save snapshots Bom + ALT Bom
				List<string> errors;
				saveBomSnapshot_PartialValidationWithBcrId(this._data.ArticleId, articleExtEntity.BomVersion, this._user.Id, out errors, this._data.BcrId, botransaction.connection, botransaction.transaction);
				if(errors != null && errors.Count > 0)
					return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse(errors);

				// -- 2022/09/16 - increment CP version if partial doc + CP valid
				int newCpVersion = 0;
				if(this._data.CanPartialValidation)
				{
					var cpVersionEntity = Infrastructure.Data.Access.Tables.BSD.CAO_DecoupageAccess.GetLastByArticle(this._data.ArticleId);
					if(cpVersionEntity != null)
					{
						newCpVersion = (cpVersionEntity.CP_version ?? 0);
						if(cpVersionEntity.Validee == true)
						{
							newCpVersion += 1;
						}
					}
				}
				// -- -2- Unvalidate CP - To Confirm abt conflict before validation
				if(Infrastructure.Data.Access.Tables.BSD.CAO_DecoupageAccess.ResetValidation(this._data.ArticleId, articleExtEntity.BomVersion ?? -1, newCpVersion, botransaction.connection, botransaction.transaction) > 0)
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
				var bomSnapshotCountByArticle = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetBOMVersionByArticle_Count(this._data.ArticleId, botransaction.connection, botransaction.transaction);
				if(bomSnapshotCountByArticle == 1)
				{
					var nonStartedFA_IDs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetNonStartedByArticle(this._data.ArticleId, botransaction.connection, botransaction.transaction)
						?.Where(x => versioningLagerIds.Contains(x.Lagerort_id ?? -1)) // - Update only BETN - for now - not any more
						?.Select(x => x.ID)?.ToList();
					if(nonStartedFA_IDs != null && nonStartedFA_IDs.Count > 0)
						UpgradeFABomHandler.upgradeFABOM(botransaction.connection, botransaction.transaction, this._user, this._data.ArticleId, nonStartedFA_IDs, articleExtEntity.BomVersion ?? -1, true);
				}
				// -- -3.2- if CP IS Required for Article, upgrade previous FA without CP to current
				if(articleEntity.CP_required.HasValue && articleEntity.CP_required.Value)
				{
					var nonStartedFAwoCP_IDs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetNonStartedByArticle(this._data.ArticleId, botransaction.connection, botransaction.transaction)
						?.Where(x => versioningLagerIds.Contains(x.Lagerort_id ?? -1)) // - Update only BETN - for now - not any more
						?.Where(x => !x.CPVersion.HasValue)
						?.Select(x => x.ID)?.ToList();
					if(nonStartedFAwoCP_IDs != null && nonStartedFAwoCP_IDs.Count > 0)
						UpgradeFABomHandler.upgradeFABOM(botransaction.connection, botransaction.transaction, this._user, this._data.ArticleId, nonStartedFAwoCP_IDs, articleExtEntity.BomVersion ?? -1);
				}
				else
				{
					// - if keeping same BOM version
					if(this._data.CanPartialValidation)
					{
						var nonStartedFA_IDs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetNonStartedByArticle(this._data.ArticleId, botransaction.connection, botransaction.transaction)
							?.Where(x => versioningLagerIds.Contains(x.Lagerort_id ?? -1)) // - Update only BETN - for now - not any more
							?.Select(x => x.ID)?.ToList();
						UpgradeFABomHandler.upgradeFABOM(botransaction.connection, botransaction.transaction, this._user, this._data.ArticleId, nonStartedFA_IDs, articleExtEntity.BomVersion ?? -1);
					}
				}
				// -- -3.3- Get FA with older Bom and w/ CP, open and non-started
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> potentialUpgradeFA = null;
				if(this._data.CanPartialValidation == false && !string.IsNullOrWhiteSpace(articleEntity.CustomerItemNumber))
				{
					var youngerSiblings = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetIndexSiblings(this._data.ArticleId, botransaction.connection, botransaction.transaction)
					?.Where(x => x.CustomerIndexSequence <= articleEntity?.CustomerIndexSequence)?.ToList();
					potentialUpgradeFA = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetBOMValidationUpgradable(
						new List<int> { this._data.ArticleId }, //// - 2024-03-11 - Sax & Schremmer - only suggest FA form same article for Upgrade 
																////   youngerSiblings?.Select(x => x.ArtikelNr)
																//////?.Where(x => x == this._data.ArticleId) // - 2023-01-30 - remove CustomerItemNumber siblings - as the can change article in AB which should update PRICING!!!
																////?.ToList(), 
						botransaction.connection, botransaction.transaction)
						?.Where(x => versioningLagerIds.Contains(x.Lagerort_id ?? -1))?.ToList(); // - Update only BETN - for now - not any more
				}



				// -- -5- Validate BOM
				Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.UpdateWithTransaction(articleExtEntity, botransaction.connection, botransaction.transaction);


				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, this._data.ArticleId,
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
				var LagerExtension = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndex(_data.ArticleId, articleEntity.Index_Kunde, botransaction.connection, botransaction.transaction);
				if(LagerExtension == null || LagerExtension.Count == 0)
				{
					var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrAndBestand(_data.ArticleId, botransaction.connection, botransaction.transaction)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> { };
					Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.InsertWithTransaction(lagerEntities?.Select(x => x.Lagerort_id ?? -1)
						.Union(Module.HauplagerCTS)?.Select(l => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity
						{
							ArtikelNr = _data.ArticleId,
							Bestand = 0m,
							Index_Kunde = articleEntity.Index_Kunde,
							Lagerort_id = l,
							LastEditTime = DateTime.Now,
							LastEditUserId = -2,
						})?.ToList(), botransaction.connection, botransaction.transaction);
				}

				// - handle transaction state (success or failure)
				if(botransaction.commit())
				{

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArticleId);

					// -- -4- Send Email notification to PM, Engineering and Quality Mgmt teams
					#region Email Notifications
					var addresses = new List<string>();
					// addresses.Add("sani.chaibousalaou@psz-electronic.com");

					// -- Article PM
					addresses.Add(
						Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
						Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByArtikelNr(this._data.ArticleId)
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
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(potentialUpgradeFA?.Select(x => x.Artikel_Nr ?? -1)?.ToList());
					return ResponseModel<List<ValidateBomResponseWithBcrModel>>.SuccessResponse(
						potentialUpgradeFA
						?.Select(x => new ValidateBomResponseWithBcrModel(x, articleEntities?.FirstOrDefault(y => y.ArtikelNr == x.Artikel_Nr)?.ArtikelNummer))
						?.ToList());
				}
				else
				{
					return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ValidateBomResponseWithBcrModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<ValidateBomResponseWithBcrModel>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
				return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse(key: "1", value: "Article not found");

			var bomPositions = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data.ArticleId);
			if(bomPositions == null || bomPositions.Count <= 0)
				return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse(key: "1", value: "Cannot validate article with BOM empty");

			var lastBOMSnapshot = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticle(articleEntity.ArtikelNr);
			var lastCP = Infrastructure.Data.Access.Tables.BSD.CAO_DecoupageAccess.GetLastByArticle(articleEntity.ArtikelNr);

			// - given, Article, BOM and CP check control validation if fully complete
			if(Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.CanValidateBom(articleEntity.ArtikelNr, lastBOMSnapshot?.BomVersion ?? 0, lastCP?.CP_version) == false)
				return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse(key: "1", value: "Cannot validate article while control not fully validated");

			// - string trucation - warnings
			var errors = new List<string>();
			if(this._data.CanPartialValidation && checkStringLength(errors))
				return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse(errors);

			// - can Partial Validate only vBOM > 0
			var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArticleId);
			if(this._data.CanPartialValidation && (articleExtEntity == null || articleExtEntity.BomVersion <= 0))
				return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse($"This is the first BOM, Partial Validation not allowed");

			if(this._data.CanPartialValidation && lastBOMSnapshot == null)
				return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse($"This is the first BOM, Partial Validation not allowed");

			if(this._data.CanPartialValidation && articleEntity.Index_Kunde != lastBOMSnapshot.KundenIndex)
				return ResponseModel<List<ValidateBomResponseWithBcrModel>>.FailureResponse($"Customer Index [{articleEntity.Index_Kunde}] has changed since previous BOM [{lastBOMSnapshot.KundenIndex}], Partial Validation not allowed");

			// - 
			return ResponseModel<List<ValidateBomResponseWithBcrModel>>.SuccessResponse();
		}
		void saveBomSnapshot_PartialValidationWithBcrId(int articleId, int? version, int userId, out List<string> errors, int bcrId, SqlConnection connection, SqlTransaction transaction)
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
						Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtension_SnapshotAccess.OverwriteSnapshotWBcrWithTransaction(articleId, version ?? -1, userId, bcrId, connection, transaction);
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
						SnapshotUserId = userId,
						BrcId = bcrId
					}, connection, transaction);

					// - Save Quality Checks 
					var entity = new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity
					{
						ArticleBOMVersion = version ?? 0,
						ArticleCPVersion = null,
						ArticleKundenIndex = articleEntity.Index_Kunde,
						ArticleKundenIndexDatum = articleEntity.Index_Kunde_Datum,
						ArticleNr = articleEntity.ArtikelNr,
						ArticleNumber = articleEntity.ArtikelNummer,
						BOMValidationDate = DateTime.Now,
						CPValidationDate = null,

						EngineeringControl = false,
						EngineeringDistribution = false,
						EngineeringPrint = false,
						EngineeringUpdate = false,
						EngineeringValidationFull = false,
						// -
						QualityControl = false,
						QualityDistribution = false,
						QualityPrint = false,
						QualityUpdate = false,
						QualityValidationFull = false,
						ValidationFull = false,
						// - 2022-07-13 - Partial Documentation
						IsPartialDocumentation = this._data.CanPartialValidation && this._data.Commission || this._data.Readiness || this._data.CrimpBeforeManual || this._data.CrimpBefore
														|| this._data.Ultrason || this._data.Twisting || this._data.InjectionPlastic || this._data.Welding || this._data.Assembling1
														|| this._data.Assembling2 || this._data.Assembling3 || this._data.CrimpAfterManual || this._data.CrimpAfter || this._data.InjectionOnCables
														|| this._data.Pressing || this._data.LabelingPlan || this._data.ControleElectrical || this._data.ControleVisual || this._data.Finition
														|| this._data.Packaging || this._data.Validation || this._data.Translation
					};

					// - save comments
					if(entity.IsPartialDocumentation == true)
					{
						entity.Commission = this._data.Commission;                     // - Commission
						entity.Readiness = this._data.Readiness;                       // - Bereit
						entity.CrimpBeforeManual = this._data.CrimpBeforeManual;       // - Sertissage avant manuel
						entity.CrimpBefore = this._data.CrimpBefore;                   // - Sertissage avant
						entity.Ultrason = this._data.Ultrason;                         // - ultrason
						entity.Twisting = this._data.Twisting;                         // - Torsadage
						entity.InjectionPlastic = this._data.InjectionPlastic;         // - Injection Plastique
						entity.Welding = this._data.Welding;                           // - Soudure
						entity.Assembling1 = this._data.Assembling1;                   // - Assemblage (1)
						entity.Assembling2 = this._data.Assembling2;                   // - Assemblage (2)
						entity.Assembling3 = this._data.Assembling3;                   // - Assemblage (3)
						entity.CrimpAfterManual = this._data.CrimpAfterManual;         // - Sertissage apres manuel
						entity.CrimpAfter = this._data.CrimpAfter;                     // - Sertissage apres
						entity.InjectionOnCables = this._data.InjectionOnCables;       // - Injection sur cables
						entity.Pressing = this._data.Pressing;                         // - Pressage
						entity.LabelingPlan = this._data.LabelingPlan;                 // - Plan d´etiquette
						entity.ControleElectrical = this._data.ControleElectrical;     // - Controle Electrique
						entity.ControleVisual = this._data.ControleVisual;             // - Contrôle visual
						entity.Finition = this._data.Finition;                         // - Finition
						entity.Packaging = this._data.Packaging;                       // - Emballage
						entity.Validation = this._data.Validation;                     // - Validation
						entity.Translation = this._data.Translation;                   // - Traduction

						entity.CommissionNotes = this._data.CommissionNotes;
						entity.ReadinessNotes = this._data.ReadinessNotes;
						entity.CrimpBeforeManualNotes = this._data.CrimpBeforeManualNotes;
						entity.CrimpBeforeNotes = this._data.CrimpBeforeNotes;
						entity.UltrasonNotes = this._data.UltrasonNotes;
						entity.TwistingNotes = this._data.TwistingNotes;
						entity.InjectionPlasticNotes = this._data.InjectionPlasticNotes;
						entity.WeldingNotes = this._data.WeldingNotes;
						entity.Assembling1Notes = this._data.Assembling1Notes;
						entity.Assembling2Notes = this._data.Assembling2Notes;
						entity.Assembling3Notes = this._data.Assembling3Notes;
						entity.CrimpAfterManualNotes = this._data.CrimpAfterManualNotes;
						entity.CrimpAfterNotes = this._data.CrimpAfterNotes;
						entity.InjectionOnCablesNotes = this._data.InjectionOnCablesNotes;
						entity.PressingNotes = this._data.PressingNotes;
						entity.LabelingPlanNotes = this._data.LabelingPlanNotes;
						entity.ControleElectricalNotes = this._data.ControleElectricalNotes;
						entity.ControleVisualNotes = this._data.ControleVisualNotes;
						entity.FinitionNotes = this._data.FinitionNotes;
						entity.PackagingNotes = this._data.PackagingNotes;
						entity.ValidationNotes = this._data.ValidationNotes;
						entity.TranslationNotes = this._data.TranslationNotes;
					}

					// - Check CP requirement 
					entity.PendingCpValidation = articleEntity.CP_required.HasValue == false || (articleEntity.CP_required.HasValue && articleEntity.CP_required.Value == true);

					if(this._data.CanPartialValidation)
					{
						var lastEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.GetByArticleBomWithTransaction(this._data.ArticleId, version ?? 0, articleEntity.Index_Kunde, connection, transaction);
						entity.Id = lastEntity?.Id ?? -1;
						Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.UpdateWithTransaction(entity, connection, transaction);
					}
					else
					{
						Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.Init(entity, connection, transaction);
					}

				}
			}
			else
			{
				errors.Add("Article not found");
			}
		}

		bool checkStringLength(List<string> errors)
		{
			int MAX_LENGTH = 250;
			if(this._data.Commission != true && this._data.Readiness != true && this._data.CrimpBeforeManual != true && this._data.CrimpBefore != true
				&& this._data.Ultrason != true && this._data.Twisting != true && this._data.InjectionPlastic != true && this._data.Welding != true && this._data.Assembling1 != true
				&& this._data.Assembling2 != true && this._data.Assembling3 != true && this._data.CrimpAfterManual != true && this._data.CrimpAfter != true && this._data.InjectionOnCables != true
				&& this._data.Pressing != true && this._data.LabelingPlan != true && this._data.ControleElectrical != true && this._data.ControleVisual != true && this._data.Finition != true
				&& this._data.Packaging != true && this._data.Validation != true && this._data.Translation != true)
			{
				errors.Add("At least one change is required.");
				return true;
			}

			if(!string.IsNullOrWhiteSpace(this._data.Assembling1Notes) && this._data.Assembling1Notes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Assembling1] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.CommissionNotes) && this._data.CommissionNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Commission] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.ReadinessNotes) && this._data.ReadinessNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Readiness] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.CrimpBeforeManualNotes) && this._data.CrimpBeforeManualNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [CrimpBeforeManual] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.CrimpBeforeNotes) && this._data.CrimpBeforeNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [CrimpBefore] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.UltrasonNotes) && this._data.UltrasonNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Ultrason] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.TwistingNotes) && this._data.TwistingNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Twisting] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.InjectionOnCablesNotes) && this._data.InjectionOnCablesNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [InjectionOnCables] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.InjectionPlasticNotes) && this._data.InjectionPlasticNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [InjectionPlastic] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.WeldingNotes) && this._data.WeldingNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Welding] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.Assembling2Notes) && this._data.Assembling2Notes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Assembling2] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.Assembling3Notes) && this._data.Assembling3Notes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Assembling3] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.CrimpAfterManualNotes) && this._data.CrimpAfterManualNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [CrimpAfterManual] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.CrimpAfterNotes) && this._data.CrimpAfterNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [CrimpAfter] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.PressingNotes) && this._data.PressingNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Pressing] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.LabelingPlanNotes) && this._data.LabelingPlanNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [LabelingPlan] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.ControleElectricalNotes) && this._data.ControleElectricalNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [ControleElectrical] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.ControleVisualNotes) && this._data.ControleVisualNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [ControleVisual] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.FinitionNotes) && this._data.FinitionNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Finition] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.PackagingNotes) && this._data.PackagingNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Packaging] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.ValidationNotes) && this._data.ValidationNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Validation] is too long. Max size is [{MAX_LENGTH}] characters");
			if(!string.IsNullOrWhiteSpace(this._data.TranslationNotes) && this._data.TranslationNotes.Trim().Length > MAX_LENGTH)
				errors.Add($"Text [Translation] is too long. Max size is [{MAX_LENGTH}] characters");


			// - 
			return errors != null && errors.Count > 0;
		}
		void decreaseBomVersion(SqlConnection connection, SqlTransaction transaction)
		{
			var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArticleId, connection, transaction);
			// - null check in Validate if CanPartialValidation
			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
				new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
                            // - Version change log
                            ObjectLogHelper.getLog(this._user, this._data.ArticleId, "(PartialValidation) Article BOM Version", $"{articleExtEntity.BomVersion}",
							$"{(articleExtEntity.BomVersion ?? 0) - 1}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)
				});

			// -- BOM level logging
			Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(
				new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> {
                                    // - Version change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									articleExtEntity.BomVersion?.ToString(), ((articleExtEntity.BomVersion ?? 0) - 1).ToString(), Enums.ObjectLogEnums.BOMLogType.Version)
				});

			articleExtEntity.BomVersion = (articleExtEntity.BomVersion ?? 0) - 1;
			Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.UpdateWithTransaction(articleExtEntity, connection, transaction);

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
