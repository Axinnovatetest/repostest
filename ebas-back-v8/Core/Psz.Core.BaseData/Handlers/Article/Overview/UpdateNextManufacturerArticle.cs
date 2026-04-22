using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	public partial class ArticleService: IArticleService
	{
		public ResponseModel<int> UpdateNextManufacturerArticle(Identity.Models.UserModel user, Models.Article.Overview.UpdateManufacturerArticleRequestModel data)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.UpdateNextManufacturerArticle_Validate(user, data);

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(data.ArticleId, botransaction.connection, botransaction.transaction);
				var articleNextEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(data.ArticleId, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
				 new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>() {
					 ObjectLogHelper.getLog(user, data.ArticleId, "Next Manufacturer Article", articleEntity.ManufacturerPreviousArticle, data.ManufacturerArticle, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit),
					 ObjectLogHelper.getLog(user, data.ManufacturerArticleId, "Previous Manufacturer Article", articleNextEntity.ManufacturerPreviousArticle, articleEntity.ArtikelNummer, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit)
				 },
					botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.updatePreviousManufacturerAticle(data.ManufacturerArticleId, articleEntity.ArtikelNr, articleEntity.ArtikelNummer, botransaction.connection, botransaction.transaction);
				var response = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.updateNextManufacturerAticle(data.ArticleId, data.ManufacturerArticleId, data.ManufacturerArticle, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				// handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-03-30
					//CreateHandler.generateFileDAT(data.ArticleId);
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> UpdateNextManufacturerArticle_Validate(Identity.Models.UserModel user, Models.Article.Overview.UpdateManufacturerArticleRequestModel data)
		{
			if(user == null/*user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.ArticleId);
			if(articleEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found.");
			}
			var nextToBe = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.ManufacturerArticleId);
			if(nextToBe == null)
			{
				return ResponseModel<int>.FailureResponse("Manufacturer Article not found.");
			}
			if(nextToBe.ManufacturerPreviousArticleId != null && nextToBe.ManufacturerPreviousArticleId != data.ArticleId)
			{
				return ResponseModel<int>.FailureResponse($"Manufacturer Article is different than [{articleEntity.ArtikelNummer}].");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
