using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Sales
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticleROHNeedStockBySupplierClassHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Sales.ArticleROHNeedStockBySupplierClassRequestModel _data { get; set; }
		public GetArticleROHNeedStockBySupplierClassHandler(UserModel user, Models.Article.Statistics.Sales.ArticleROHNeedStockBySupplierClassRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//-
				return ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Functions.ArticleStatisticsAccess.GetArticleROHNeedStock("", 23, 2)
							.Select(x => new Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel(x)).ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>>.SuccessResponse();
		}
	}
}
