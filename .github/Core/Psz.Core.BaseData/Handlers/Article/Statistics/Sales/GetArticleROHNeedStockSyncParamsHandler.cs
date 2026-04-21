using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Sales
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetArticleROHNeedStockSyncParamsHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncParamsResponseModel>>
	{
		private UserModel _user { get; set; }
		public GetArticleROHNeedStockSyncParamsHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncParamsResponseModel> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncParamsResponseModel>.SuccessResponse(
					new Models.Article.Statistics.Sales.ArticleROHNeedStockSyncParamsResponseModel(
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_SyncParams()));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncParamsResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncParamsResponseModel>.AccessDeniedResponse();
			}


			return ResponseModel<Models.Article.Statistics.Sales.ArticleROHNeedStockSyncParamsResponseModel>.SuccessResponse();
		}

	}
}
