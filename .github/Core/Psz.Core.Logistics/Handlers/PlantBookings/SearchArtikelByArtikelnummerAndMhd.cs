namespace Psz.Core.Logistics.Handlers.PlantBookings;
	public class SearchArtikelByArtikelnummerAndMhdHandler: IHandle<Models.Lagebewegung.ArticleSearchMhdModel, ResponseModel<Models.Lagebewegung.ArticleSearchMHDResponseModel>>
	{
		private Models.Lagebewegung.ArticleSearchMhdModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchArtikelByArtikelnummerAndMhdHandler(Identity.Models.UserModel user, Models.Lagebewegung.ArticleSearchMhdModel data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<Models.Lagebewegung.ArticleSearchMHDResponseModel> Handle()
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


				var articles = new List<Models.PlantBookings.ArtikelWithMhModel>();
				int allCount = 0;

				List<string> articleNummers = null;
				//if(!string.IsNullOrWhiteSpace(this._data.CustomerNr) && !string.IsNullOrEmpty(this._data.CustomerNr))
				//{
				//	articleNummers = Infrastructure.Data.Access.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Access.GetByKunden(this._data.CustomerNr, this._data.CustomerPrefix)?.Select(x => x.Artikelnummer).Distinct().ToList();
				//}

				var articleEntities = Infrastructure.Data.Access.Tables.Logistics.LagerArtikelAccess.SearchArtikelByArtikelnummerAndMhd(
					this._data.artikelnummer,
					dataSorting,
					dataPaging);

				if(articleEntities != null && articleEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.Logistics.LagerArtikelAccess.SearchArtikelByArtikelnummer_CountAll(
						this._data.artikelnummer
						);
					articles = articleEntities.Select(x => new Models.PlantBookings.ArtikelWithMhModel(x)).ToList();


				}

				return ResponseModel<Models.Lagebewegung.ArticleSearchMHDResponseModel>.SuccessResponse(
					new Models.Lagebewegung.ArticleSearchMHDResponseModel()
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

		public ResponseModel<Models.Lagebewegung.ArticleSearchMHDResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Lagebewegung.ArticleSearchMHDResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Lagebewegung.ArticleSearchMHDResponseModel>.SuccessResponse();
		}
	}




