using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	public partial class ArticleService: IArticleService
	{
		public ResponseModel<int> UpdateArticlePmData(Identity.Models.UserModel user, Models.Article.ArticleOverviewModel data)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.UpdateArticlePmData_Validate(user, data);

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(data.ArtikelNr, botransaction.connection, botransaction.transaction);

				// -
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				if(data.Projektname != articleEntity.Projektname)
				{
					logs.Add(ObjectLogHelper.getLog(user, data.ArtikelNr, "Projektname", articleEntity.Projektname, data.Projektname, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));
				}
				if(data.CustomerEnd != articleEntity.CustomerEnd)
				{
					logs.Add(ObjectLogHelper.getLog(user, data.ArtikelNr, "CustomerEnd", articleEntity.CustomerEnd, data.CustomerEnd, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));
				}
				if(data.CustomerTechnic != articleEntity.CustomerTechnic || data.CustomerTechnicId != articleEntity.CustomerTechnicId)
				{
					logs.Add(ObjectLogHelper.getLog(user, data.ArtikelNr, "CustomerTechnic", articleEntity.CustomerTechnic, data.CustomerTechnic, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));
				}
				if(logs.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
				}

				// -
				var response = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.updatePmDatArticle(data.ToEntity(), botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				// handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-03-30
					//CreateHandler.generateFileDAT(data.ArticleId);
					return ResponseModel<int>.SuccessResponse(0);
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
		public ResponseModel<int> UpdateArticlePmData_Validate(Identity.Models.UserModel user, Models.Article.ArticleOverviewModel data)
		{
			if(user == null/*user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.ArtikelNr) == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
