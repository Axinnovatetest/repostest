using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class SearchHandler: IHandle<Models.Article.ArticleSearchModel, ResponseModel<Models.Article.ArticleSearchResponseModel>>
	{
		private Models.Article.ArticleSearchModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchHandler(Identity.Models.UserModel user, Models.Article.ArticleSearchModel data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<Models.Article.ArticleSearchResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(this._data.SortFieldKey.ToLower())
					{
						default:
						case "artikelnr":
							sortFieldName = "[Artikel-Nr]";
							break;
						case "artikelnummer":
							sortFieldName = "[ArtikelNummer]";
							break;
						case "bezeichnung1":
							sortFieldName = "[Bezeichnung 1]";
							break;
						case "bezeichnung2":
							sortFieldName = "[Bezeichnung 2]";
							break;
						case "freigabestatus":
							sortFieldName = "[Freigabestatus]";
							break;
						case "index_kunde":
							sortFieldName = "[index_kunde]";
							break;
						case "index_kunde_datum":
							sortFieldName = "[index_kunde_datum]";
							break;
						case "artikelfamilie_kunde":
							sortFieldName = "[Artikelfamilie_Kunde]";
							break;
						case "customeritemnumber":
							sortFieldName = "[CustomerItemNumber]";
							break;
						case "edidefault":
							sortFieldName = "[EdiDefault]";
							break;
						case "aktiv":
							sortFieldName = "[Aktiv]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				#endregion

				var articles = new List<Models.Article.ArticleMinimalModel>();
				int allCount = 0;

				List<string> articleNummers = null;
				if(!string.IsNullOrWhiteSpace(this._data.CustomerNr) && !string.IsNullOrEmpty(this._data.CustomerNr))
				{
					articleNummers = Infrastructure.Data.Access.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Access.GetByKunden(this._data.CustomerNr, this._data.CustomerPrefix)?.Select(x => x.Artikelnummer).Distinct().ToList();
				}

				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.SearchByNrNumberDesignation(
					articleNummers,
					this._data.ArticleNummer,
					this._data.ArticleDesignation,
					this._data.GoodsGroup,
					this._data.active,
					this._data.ArticleFamily,
					this._data.CustomerItemNumber,
					this._data.Details, this._data.EdiDefault, this._data.EDrawing,
					this._data.ArticleReference,
					dataSorting,
					dataPaging);

				if(articleEntities != null && articleEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.SearchByNrNumberDesignation_CountAll(
						articleNummers,
						this._data.ArticleNummer,
						this._data.ArticleDesignation,
						this._data.GoodsGroup,
						this._data.active,
					this._data.ArticleFamily,
					this._data.CustomerItemNumber,
						this._data.Details, this._data.EdiDefault, this._data.EDrawing);
					var articleExtEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNrs(articleEntities.Select(x => x.ArtikelNr).ToList());

					for(int i = 0; i < articleEntities.Count; i++)
					{
						var articleExtEntity = articleExtEntities.FirstOrDefault(x => x.ArtikelNr == articleEntities[i].ArtikelNr);
						articles.Add(new Models.Article.ArticleMinimalModel(articleEntities[i], articleExtEntity?.OrderNumber));
					}
				}

				return ResponseModel<Models.Article.ArticleSearchResponseModel>.SuccessResponse(
					new Models.Article.ArticleSearchResponseModel()
					{
						Articles = articles,
						RequestedPage = this._data.RequestedPage,
						ItemsPerPage = this._data.ItemsPerPage,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
					});

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Article.ArticleSearchResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ArticleSearchResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.ArticleSearchResponseModel>.SuccessResponse();
		}
	}
}
