using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.BaseData.Models.Article.ArticlesPricesHistory;


namespace Psz.Core.BaseData.Handlers.Article.ArticlesPricesHistory
{
	public class ArtikelPricesHistoryChartHandler: IHandle<UserModel, ResponseModel<List<ArtikelsPricesChangesHistoryModel>>>
	{
		private UserModel _user { get; set; }
		private ArtikelsPricesHistoryRequestModel _data { get; set; }
		public ArtikelPricesHistoryChartHandler(UserModel user, ArtikelsPricesHistoryRequestModel after)
		{
			this._user = user;
			this._data = after;
		}
		public ResponseModel<List<ArtikelsPricesChangesHistoryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var fetchedData = Infrastructure.Data.Access.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryAccess.GetArticleHistoryByDateForChart(this._data.Artikelnummer, this._data.changes);

				if(fetchedData is null || fetchedData.Count == 0)
					return ResponseModel<List<ArtikelsPricesChangesHistoryModel>>.SuccessResponse();
				var result = fetchedData.Select(x => new ArtikelsPricesChangesHistoryModel(x)).ToList();
				return ResponseModel<List<ArtikelsPricesChangesHistoryModel>>.SuccessResponse(result);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<List<ArtikelsPricesChangesHistoryModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<ArtikelsPricesChangesHistoryModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<ArtikelsPricesChangesHistoryModel>>.SuccessResponse();
		}
	}
}
