using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
namespace Psz.Core.BaseData.Handlers.Article
{
	public partial class ArticleService: IArticleService
	{
		public ResponseModel<int> ResetPreviousManufacturerArticle(Identity.Models.UserModel user, int data)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.ResetPreviousManufacturerArticle_Validate(user, data);

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(data, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>()
					{
						ObjectLogHelper.getLog(user, data, "Reset Previous Manufacturer Article", articleEntity.ManufacturerPreviousArticle, "", Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit)
					},
					botransaction.connection, botransaction.transaction);
				var response = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.ResetPreviousManufacturerAticle(data,  botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				// handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-03-30
					//CreateHandler.generateFileDAT(data);
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
		public ResponseModel<int> ResetPreviousManufacturerArticle_Validate(Identity.Models.UserModel user, int data)
		{
			if(user == null/*user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data);
			if(articleEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
