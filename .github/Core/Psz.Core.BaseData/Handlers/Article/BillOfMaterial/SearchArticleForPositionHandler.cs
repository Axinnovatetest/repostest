using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class SearchArticleForPositionHandler: IHandle<Models.Article.ArticleSearchModel, ResponseModel<Models.Article.ArticleSearchResponseModel>>
	{
		private Models.Article.ArticleSearchModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchArticleForPositionHandler(Identity.Models.UserModel user, Models.Article.ArticleSearchModel data)
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
						case "nr":
							sortFieldName = "[Artikel-Nr]";
							break;
						case "nummer":
							sortFieldName = "[ArtikelNummer]";
							break;
						case "designation1":
							sortFieldName = "[Bezeichnung1]";
							break;
						case "designation2":
							sortFieldName = "[Bezeichnung2]";
							break;
						case "loadingpoint":
							sortFieldName = "[Abladestelle]";
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
					articleNummers = Infrastructure.Data.Access.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Access.GetByKunden(this._data.CustomerNr)?.Select(x => x.Artikelnummer).Distinct().ToList();
				}

				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.SearchByNrNumberDesignation(
					articleNummers,
					this._data.ArticleNummer,
					this._data.ArticleDesignation,
					this._data.GoodsGroup,
					this._data.active,
					this._data.ArticleFamily,
					this._data.CustomerItemNumber,
					this._data.Details, this._data.EdiDefault, this._data.EDrawing, this._data.ArticleReference,
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
					this._data.CustomerItemNumber, this._data.Details, this._data.EdiDefault, this._data.EDrawing, this._data.ArticleReference);

					for(int i = 0; i < articleEntities.Count; i++)
					{
						articles.Add(new Models.Article.ArticleMinimalModel(articleEntities[i]));
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
