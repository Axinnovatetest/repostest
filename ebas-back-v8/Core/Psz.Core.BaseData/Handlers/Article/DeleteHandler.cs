using System;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public DeleteHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				var deletedId = 0;
				#region // -- transaction-based logic -- //
				lock((Locks.ArticleEditLock.GetOrAdd(this._data, new object())))
				{
					var artikel = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
					#region // - BOM - //
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.DeleteByArticleWithTransaction(this._data, botransaction.connection, botransaction.transaction);
					#endregion BOM

					#region // - Prod - //
					//if (this._data.GoodsGroupName?.Trim()?.ToLower() == "ef")
					{
						Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.DeleteByArticleWithTransaction(this._data, botransaction.connection, botransaction.transaction);
					}
					#endregion Prod

					#region // - Quality - // 
					Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.DeleteByArticleWithTransaction(this._data, botransaction.connection, botransaction.transaction);
					#endregion Quality

					#region // - Logistics - // 
					Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.DeleteByArticleWithTransaction(this._data, botransaction.connection, botransaction.transaction);
					#endregion Logistics

					// -
					deletedId = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.DeleteWithTransaction(this._data, botransaction.connection, botransaction.transaction);

					// - logs
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
						ObjectLogHelper.getLog(this._user, this._data, "Article", $"{artikel.ArtikelNummer}", "", Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Delete),
						botransaction.connection, botransaction.transaction);
				}

				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(deletedId);
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
			if(this._user == null || (this._user.SuperAdministrator != true && this._user.Access?.MasterData?.DeleteArticle != true))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found");
			}

			// - check for HBG
			var parentIds = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetParentIds(this._data);
			if(parentIds != null || parentIds.Count > 0)
			{
				var parents = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(parentIds);
				ResponseModel<int>.FailureResponse($"Cannot delete article with HBG [{string.Join(",", parents?.Take(5)?.Select(x => x.ArtikelNummer))}]");
			}

			// - check for open FA
			var notClosedFas = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetOpenByArticle(this._data, null);
			if(notClosedFas != null || notClosedFas.Count > 0)
			{
				ResponseModel<int>.FailureResponse($"Cannot delete article with FAs [{string.Join(",", notClosedFas?.Take(5)?.Select(x => x.Fertigungsnummer))}]");
			}
			var notHBGClosedFas = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetOpenHbgFaByArticleInLager(this._data, null);
			if(notHBGClosedFas != null || notHBGClosedFas.Count > 0)
			{
				ResponseModel<int>.FailureResponse($"Cannot delete article with HBG FAs [{string.Join(",", notClosedFas?.Take(5)?.Select(x => x.Fertigungsnummer))}]");
			}

			// - check for open ABs
			var openAbs = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetOpenAbByArticleInLager(this._data, null, true, false);
			if(openAbs != null || openAbs.Count > 0)
			{
				var abs = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(openAbs.Select(x => x.AngebotNr ?? -1).ToList());
				ResponseModel<int>.FailureResponse($"Cannot delete article with ABs [{string.Join(",", abs?.Take(5)?.Select(x => x.Angebot_Nr))}]");
			}
			// - check for open LSs
			var openLSs = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetOpenLsByArticleInLager(this._data, null, true, false);
			if(openAbs != null || openAbs.Count > 0)
			{
				var abs = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(openAbs.Select(x => x.AngebotNr ?? -1).ToList());
				ResponseModel<int>.FailureResponse($"Cannot delete article with LSs [{string.Join(",", abs?.Take(5)?.Select(x => x.Angebot_Nr))}]");
			}
			// - check for open RAs
			var openRAs = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetOpenRaByArticleInLager(this._data, null, true, false);
			if(openAbs != null || openAbs.Count > 0)
			{
				var abs = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(openAbs.Select(x => x.AngebotNr ?? -1).ToList());
				ResponseModel<int>.FailureResponse($"Cannot delete article with RAs [{string.Join(",", abs?.Take(5)?.Select(x => x.Angebot_Nr))}]");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}

}
