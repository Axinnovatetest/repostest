using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class SearchArtikelHandler: IHandle<Models.Lagebewegung.ArticleSearchModel, ResponseModel<Models.Lagebewegung.ArticleSearchResponseModel>>
	{
		private Models.Lagebewegung.ArticleSearchModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchArtikelHandler(Identity.Models.UserModel user, Models.Lagebewegung.ArticleSearchModel data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<Models.Lagebewegung.ArticleSearchResponseModel> Handle()
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
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;


				var articles = new List<Models.Lagebewegung.ArtikelMinimalLagerbewegungModel>();
				int allCount = 0;

				List<string> articleNummers = null;
				//if(!string.IsNullOrWhiteSpace(this._data.CustomerNr) && !string.IsNullOrEmpty(this._data.CustomerNr))
				//{
				//	articleNummers = Infrastructure.Data.Access.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Access.GetByKunden(this._data.CustomerNr, this._data.CustomerPrefix)?.Select(x => x.Artikelnummer).Distinct().ToList();
				//}

				var articleEntities = Infrastructure.Data.Access.Tables.Logistics.LagerArtikelAccess.SearchArtikelByArtikelnummer(
					this._data.artikelnummer,
					dataSorting,
					dataPaging);

				if(articleEntities != null && articleEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.Logistics.LagerArtikelAccess.SearchArtikelByArtikelnummer_CountAll(
						this._data.artikelnummer
						);
					articles = articleEntities.Select(x => new Models.Lagebewegung.ArtikelMinimalLagerbewegungModel(x)).ToList();


				}

				return ResponseModel<Models.Lagebewegung.ArticleSearchResponseModel>.SuccessResponse(
					new Models.Lagebewegung.ArticleSearchResponseModel()
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

		public ResponseModel<Models.Lagebewegung.ArticleSearchResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Lagebewegung.ArticleSearchResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Lagebewegung.ArticleSearchResponseModel>.SuccessResponse();
		}
	}
}
