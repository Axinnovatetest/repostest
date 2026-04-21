using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetCartonsHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Basics.CartonResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.CartonsCirculationRequestModel _data { get; set; }
		public GetCartonsHandler(UserModel user, Models.Article.Statistics.Basics.CartonsCirculationRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.Basics.CartonResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<Models.Article.Statistics.Basics.CartonResponseModel> responseBody = new List<Models.Article.Statistics.Basics.CartonResponseModel>();

				// -
				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> statisticsEntities = new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
				switch(this._data.Site)
				{
					//case Enums.ArticleEnums.StatsCartonsCirculationSites.Tunesien:
					//	statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_TN(this._data.DateFrom, this._data.DateTill)
					//		?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
					//	break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.WolfHalleKHTN:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_WS(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					//case Enums.ArticleEnums.StatsCartonsCirculationSites.BETN:
					//	statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_BETN(this._data.DateFrom, this._data.DateTill)
					//		?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
					//	break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.Albanien:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_AL(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.Tschechien:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_CZ(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.Vohenstrauss:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_DE(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.Gesamt:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_Gesamt(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.GZTN:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_GZTN(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					default:
						break;
				}

				if(statisticsEntities.Count > 0)
				{
					var distinctArticles = statisticsEntities
								  .GroupBy(x => new { x.Artikelnummer_Umlauf, x.SummevonBedarf, x.Bestand, x.Transfer_Bestand })
								  .Select(g => g.First())
								  .ToList();

					foreach(var articleItem in distinctArticles)
					{
						responseBody.Add(new Models.Article.Statistics.Basics.CartonResponseModel
						{
							Artikelnummer_Umlauf = articleItem.Artikelnummer_Umlauf,
							SummevonBedarf = articleItem.SummevonBedarf,
							Bestand = articleItem.Bestand,
							Transfer_Bestand = articleItem.Transfer_Bestand,
							OrderItems = statisticsEntities
								.FindAll(x =>
									x.Artikelnummer_Umlauf == articleItem.Artikelnummer_Umlauf &&
									x.SummevonBedarf == articleItem.SummevonBedarf &&
									x.Bestand == articleItem.Bestand &&
									x.Transfer_Bestand == articleItem.Transfer_Bestand)
								.Select(x => new Models.Article.Statistics.Basics.CartonResponseModel.OrderItem
								{
									Anzahl = x.Anzahl,
									Bestatigter_Termin = x.Bestatigter_Termin,
									Bestellung_Nr = x.Bestellung_Nr
								})?.ToList()
						});
					}
				}

				return ResponseModel<List<Models.Article.Statistics.Basics.CartonResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Basics.CartonResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Basics.CartonResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Basics.CartonResponseModel>>.SuccessResponse();
		}
	}
}
