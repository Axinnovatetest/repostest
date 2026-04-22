using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHUBGStockStatusItemHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.CustomerService.StockStatusItemRequestModel _data { get; set; }
		public GetHUBGStockStatusItemHandler(UserModel user, Models.Article.Statistics.CustomerService.StockStatusItemRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Article.Statistics.CustomerService.StockStatusItemResponseModel();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
				responseBody.ArticleId = this._data.ArticleId;
				responseBody.ArticleNumber = articleEntity?.ArtikelNummer;

				var stockLagers = Module.AppSettings.ProductionLagerIds ?? new List<int>(); // - new List<int> { 6, 7, 15, 26, 42, 60 };
				stockLagers.AddRange(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetTransfertLagers()?.Select(x => x.Lagerort_id) ?? new List<int>());
				stockLagers.AddRange(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetPLLagers()?.Select(x => x.Lagerort_id) ?? new List<int>());
				stockLagers.AddRange(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetHauptLagers()?.Select(x => x.Lagerort_id) ?? new List<int>());
				// - Stock - 
				var stockEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetStandardByArticleAndId(this._data.ArticleId, this._data.LagerId.HasValue == true ? new List<int> { this._data.LagerId.Value } : stockLagers);
				responseBody.StockItems.AddRange(stockEntities.Select(x => new Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.StockItem(x))?.ToList()
					?? new List<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.StockItem>());

				// - FA - where Article is main - Stock
				var openPositiveFaEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetOpenByArticleInLager(this._data.ArticleId, this._data.LagerId);
				if(openPositiveFaEntities != null && openPositiveFaEntities.Count > 0)
				{
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(openPositiveFaEntities.Select(x => x.Artikel_Nr ?? -1)?.ToList());
					responseBody.FAItems = new List<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.FAItem>();
					foreach(var faItem in openPositiveFaEntities)
					{
						var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == (faItem?.Artikel_Nr ?? -1));
						// - 
						responseBody.FAItemsPositive.Add(new Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.FAItem
						{
							FAId = faItem?.ID ?? 0,
							FAArticleId = faItem?.Artikel_Nr ?? 0,
							FAArticleNumber = article?.ArtikelNummer,
							FAArticleCustomerIndex = faItem?.KundenIndex,
							FAOpenQuantity = faItem.Anzahl ?? 0,
							FAOriginalQuantity = faItem.Originalanzahl ?? 0,
							FADate = faItem?.Termin_Bestatigt1, // - 2022-12-12 - Schremmer faItem?.Termin_Bestatigt2,
							FAWDate = faItem?.Termin_Bestatigt2,
							FASite = faItem?.Lagerort_id,
							FANumber = faItem?.Fertigungsnummer ?? 0
						});
					}
				}


				// - FA - where Article is UBG - NEED
				var openFAPositionEntities = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetByArticleInLager(this._data.ArticleId, this._data.LagerId);
				if(openFAPositionEntities != null && openFAPositionEntities.Count > 0)
				{
					var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(openFAPositionEntities.Select(x => x.ID_Fertigung ?? 1)?.ToList());
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(faEntities?.Select(x => x.Artikel_Nr ?? -1)?.ToList());
					responseBody.FAItems = new List<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.FAItem>();
					foreach(var posItem in openFAPositionEntities)
					{
						var fa = faEntities.FirstOrDefault(x => x.ID == posItem.ID_Fertigung);
						var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == fa.Artikel_Nr);
						// - 
						responseBody.FAItems.Add(new Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.FAItem(fa, posItem, article));
					}
				}

				// - AB - where Article is Pos - NEED
				var lager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetByStandard(this._data.LagerId ?? -1);
				var openABPositiionEntities = Infrastructure.Data.Access.Joins.BSD.DashboardAccess.GetOpenAbByArticleInLager(this._data.ArticleId, lager?.Lagerort_id, true, false);
				if(openABPositiionEntities != null && openABPositiionEntities.Count > 0)
				{
					var abEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(openABPositiionEntities.Select(x => x.AngebotNr ?? 1)?.ToList());
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(openABPositiionEntities.Select(x => x.ArtikelNr ?? -1)?.ToList());
					responseBody.ABItems = new List<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.ABItem>();
					foreach(var posItem in openABPositiionEntities)
					{
						var ab = abEntities.FirstOrDefault(x => x.Nr == posItem.AngebotNr);
						var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == posItem.ArtikelNr);
						// - 
						responseBody.ABItems.Add(new Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.ABItem(ab, posItem, article));
					}
				}

				// - compute Stock movements
				var uniqueSites = responseBody.StockItems?.Select(x => x.Site).Distinct()?.ToList() ?? new List<int>();
				uniqueSites.AddRange(responseBody.FAItems.Select(x => x.FASite ?? -1)?.Distinct()?.ToList() ?? new List<int>());
				uniqueSites.AddRange(responseBody.FAItemsPositive.Select(x => x.FASite ?? -1)?.Distinct()?.ToList() ?? new List<int>());
				uniqueSites = uniqueSites.Distinct().ToList();
				var siteEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetStandardByArticleAndId(this._data.ArticleId, uniqueSites);
				foreach(var siteItem in uniqueSites)
				{
					var siteIdx = responseBody.StockItems.FindIndex(x => x.Site == siteItem);
					if(siteIdx < 0)
					{
						var siteEntity = siteEntities.FirstOrDefault(x => x.Lagerort_id == siteItem);
						responseBody.StockItems.Add(new Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.StockItem
						{
							Id = siteEntity?.ID ?? -1,
							CurrentQuantity = siteEntity?.Bestand ?? 0,
							MinStockQuantity = siteEntity?.Mindestbestand ?? 0,
							ReservedStockQuantity = siteEntity?.Bestand_reserviert ?? 0,
							Site = siteEntity?.Lagerort_id ?? -1
						});
						siteIdx = responseBody.StockItems.Count - 1;
					}

					// - 
					responseBody.StockItems[siteIdx].ForecastPositive = responseBody.FAItemsPositive?.Where(x => x.FASite == siteItem)?.Sum(x => x.FAOpenQuantity) ?? 0;
					responseBody.StockItems[siteIdx].ForecastNegative = responseBody.FAItems?.Where(x => x.FASite == siteItem)?.Sum(x => x.FAOpenQuantity) ?? 0;
				}

				// - add virtual for AB, bc AB do not have site
				var idx = responseBody.StockItems.FindIndex(x => x.Site == -1);
				if(idx < 0)
				{
					responseBody.StockItems.Add(new Models.Article.Statistics.CustomerService.StockStatusItemResponseModel.StockItem
					{
						Id = -1,
						CurrentQuantity = 0,
						MinStockQuantity = 0,
						ReservedStockQuantity = 0,
						Site = -1
					});
					idx = responseBody.StockItems.Count - 1;
				}
				responseBody.StockItems[idx].ForecastNegative += responseBody.ABItems?.Sum(x => x.ABPostionOpenQuantity) ?? 0;

				// -
				responseBody.StockItems = responseBody.StockItems.Where(x => x.CurrentQuantity != 0 || x.MinStockQuantity > 0 || x.ReservedStockQuantity > 0 || x.ForecastNegative > 0 || x.ForecastPositive > 0).ToList();

				// -
				return ResponseModel<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
			{
				return ResponseModel<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel>.FailureResponse("Article not found");
			}
			if(articleEntity.UBG != true)
			{
				return ResponseModel<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel>.FailureResponse("Article not UBG");
			}

			return ResponseModel<Models.Article.Statistics.CustomerService.StockStatusItemResponseModel>.SuccessResponse();
		}
	}
}
