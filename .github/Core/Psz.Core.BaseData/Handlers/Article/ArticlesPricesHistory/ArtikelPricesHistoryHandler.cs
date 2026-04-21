using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.BaseData.Models.Article.ArticlesPricesHistory;

namespace Psz.Core.BaseData.Handlers.Article.ArticlesPricesHistory
{
	public class ArtikelPricesHistoryHandler: IHandle<UserModel, ResponseModel<IPaginatedResponseModel<ArtikelsPricesChangesHistoryModel>>>
	{
		private UserModel _user { get; set; }
		private ArtikelsPricesHistoryRequestModel _data { get; set; }
		public ArtikelPricesHistoryHandler(UserModel user, ArtikelsPricesHistoryRequestModel after)
		{
			this._user = user;
			this._data = after;

		}
		public ResponseModel<IPaginatedResponseModel<ArtikelsPricesChangesHistoryModel>> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};

				var fetchedData = Infrastructure.Data.Access.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryAccess.GetArticleHistoryByDateLast(this._data.Artikelnummer, dataPaging);

				if(fetchedData is null || fetchedData.Count == 0)
					return ResponseModel<IPaginatedResponseModel<ArtikelsPricesChangesHistoryModel>>.SuccessResponse();
				var result = fetchedData.Select(x => new ArtikelsPricesChangesHistoryModel(x)).ToList();
				return ResponseModel<IPaginatedResponseModel<ArtikelsPricesChangesHistoryModel>>.SuccessResponse(new IPaginatedResponseModel<ArtikelsPricesChangesHistoryModel>
				{
					Items = result,
					PageRequested = this._data.RequestedPage,
					PageSize = this._data.PageSize,
					TotalCount = result.FirstOrDefault().TotalCount ?? 0,
					TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(result.FirstOrDefault().TotalCount > 0 ? result.FirstOrDefault().TotalCount : 0) / this._data.PageSize)) : 0
				}
	);

			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<IPaginatedResponseModel<ArtikelsPricesChangesHistoryModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<IPaginatedResponseModel<ArtikelsPricesChangesHistoryModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<ArtikelsPricesChangesHistoryModel>>.SuccessResponse();
		}
	}
}
