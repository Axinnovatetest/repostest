using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpgradeHUBGHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.Data.UpgradableHUBGResponseModel _data { get; set; }

		public UpgradeHUBGHandler(Identity.Models.UserModel user, Models.Article.Data.UpgradableHUBGResponseModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{

				#region // -- transaction-based logic -- //

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				// - HBG - update BOM w new article - where current is USED // - upgrade BOM parent of current
				#region // - HBG - // - upgrade BOM parent of current
				var hbgToUpgrade = this._data.HBGItems?.Where(x => x.Upgrade == true);
				if(hbgToUpgrade != null && hbgToUpgrade.Count() > 0)
				{
					var ids = hbgToUpgrade.Select(x => x.CurrentArticleId).ToList();
					// - get Parent BOMs - where current is UBG
					var bomExtEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticles(ids);
					var bomPosEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticleUBG(this._data.CurrentArticleId);
					if(bomExtEntities != null && bomExtEntities.Count > 0)
					{
						var newArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.NewArticleId);
						var extUpgrades = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
						var changed = false;
						foreach(var hbgItem in hbgToUpgrade)
						{
							changed = false;
							for(int i = 0; i < bomPosEntities.Count; i++)
							{
								// - replace child Article in BOM Pos
								if(bomPosEntities[i].Artikel_Nr == hbgItem.CurrentArticleId && bomPosEntities[i].Artikel_Nr_des_Bauteils == this._data.CurrentArticleId)
								{
									bomPosEntities[i].Artikel_Nr_des_Bauteils = this._data.NewArticleId;
									bomPosEntities[i].Bezeichnung_des_Bauteils = newArticle?.Bezeichnung1;
									bomPosEntities[i].Artikelnummer = newArticle?.ArtikelNummer;

								}
								changed = true;
							}
							if(changed)
							{
								var extEntity = bomExtEntities.FirstOrDefault(x => x.ArticleId == hbgItem.CurrentArticleId);
								if(extEntity != null)
								{
									if((extEntity.BomStatusId ?? -1) == (int)Enums.ArticleEnums.BomStatus.Approved)
									{
										extEntity.BomVersion = (extEntity.BomVersion ?? 0) + 1;
									}
									extEntity.BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription();
									extEntity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation;
									extEntity.LastUpdateTime = DateTime.Now;
									extEntity.LastUpdateUserId = this._user.Id;
									extUpgrades.Add(extEntity);
								}
							}
						}

						// - aply upgrade
						Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.UpdateWithTransaction(extUpgrades, botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.UpdateWithTransaction(bomPosEntities, botransaction.connection, botransaction.transaction);
					}
				}
				#endregion


				// - UBG - update BOM w new article - where current is Parent // - upgrade BOM current
				#region // - UGB - // - upgrade BOM current
				var ubgToUpgrade = this._data.UBGItems?.Where(x => x.Upgrade == true);
				if(ubgToUpgrade != null && ubgToUpgrade.Count() > 0)
				{
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(ubgToUpgrade.Select(x => x.NewArticleId)?.ToList());
					var bomEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data.CurrentArticleId);
					for(int i = 0; i < bomEntities?.Count; i++)
					{
						var newA = ubgToUpgrade.FirstOrDefault(x => x.CurrentArticleId == bomEntities[i].Artikel_Nr_des_Bauteils);
						if(newA != null)
						{
							var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == newA.NewArticleId);
							bomEntities[i].Artikel_Nr_des_Bauteils = newA.NewArticleId;
							bomEntities[i].Artikelnummer = article.ArtikelNummer;
							bomEntities[i].Bezeichnung_des_Bauteils = article.Bezeichnung1;
						}
					}
					// - apply upgrade
					var extEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.CurrentArticleId);
					if((extEntity.BomStatusId ?? -1) == (int)Enums.ArticleEnums.BomStatus.Approved)
					{
						extEntity.BomVersion = (extEntity.BomVersion ?? 0) + 1;
					}
					extEntity.BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription();
					extEntity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation;
					extEntity.LastUpdateTime = DateTime.Now;
					extEntity.LastUpdateUserId = this._user.Id;

					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.UpdateWithTransaction(extEntity, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.UpdateWithTransaction(bomEntities, botransaction.connection, botransaction.transaction);
				}
				#endregion  // - UGB - //

				#endregion transaction-based logic

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - Email PM HeadOf, Projektbetreuer, ENg. ProductionPlace
					#region // - Eamil - //
					var emailAddresses = new List<string>();

					// - HeadOf

					// - Owner
					var pOwner = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
						Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByArtikelNr(this._data.CurrentArticleId)?.UserId ?? -1);
					if(pOwner != null && !string.IsNullOrWhiteSpace(pOwner.Email))
					{
						emailAddresses.Add(pOwner.Email);
					}

					// - Eng. team
					var bomNotifiactionUsers = Infrastructure.Data.Access.Tables.BSD.BomMailUsersAccess.GetBySite(
						Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(this._data.CurrentArticleId)?.ProductionPlace1_Name);
					if(bomNotifiactionUsers != null && bomNotifiactionUsers.Count > 0)
					{
						emailAddresses.AddRange(
							bomNotifiactionUsers.Select(x => x.UserEmail));
					}

					// - 
					emailAddresses = emailAddresses.Distinct()?.ToList();
					if(emailAddresses.Count > 0)
					{
						var articleExtEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.CurrentArticleId);
						var title = $"Customer Index Upgrade";
						var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

						content += $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just added a new customer index for article <strong>{articleExtEntity?.ArticleNumber?.ToUpper()}</strong>." +
						$"This may have implied upgrade in BOM of HBG articles." +
						$"<br/>Please consider checking the Master Data Dashboard for more details.";


						content += $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleExtEntity.ArtikelNr}/overview'>here</a>"
							+ "<br/><br/>Regards, <br/>IT Department </div>";

						Module.EmailingService.SendEmailAsync(title, content, emailAddresses);
					}
					#endregion
					// -
					return ResponseModel<int>.SuccessResponse(1);
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

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
