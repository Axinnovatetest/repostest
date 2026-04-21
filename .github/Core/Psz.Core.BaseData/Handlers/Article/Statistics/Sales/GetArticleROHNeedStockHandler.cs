using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Sales
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticleROHNeedStockHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Sales.ArticleROHNeedStockRequestModel _data { get; set; }
		public GetArticleROHNeedStockHandler(UserModel user, Models.Article.Statistics.Sales.ArticleROHNeedStockRequestModel data)
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
				var supplierNames = new List<string>();
				if(this._data.AdressenNr != null && this._data.AdressenNr > 0)
				{
					var supplier = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AdressenNr.Value);
					if(supplier != null)
					{
						supplierNames.Add(supplier.Name1);
					}
				}
				else
				{
					var suppliers = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByStufe(this._data.Stufe);
					if(suppliers != null)
					{
						supplierNames.AddRange(suppliers.Select(x => x.Name1));
					}
				}

				return ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock(supplierNames, this._data.ArticleNumber)
							?.Select(x => new Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel(x)).ToList());
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
