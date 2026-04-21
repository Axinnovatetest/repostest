using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetCirculationsPDFHandler: IHandle<UserModel, ResponseModel<string>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.CartonsCirculationRequestModel _data { get; set; }
		public GetCirculationsPDFHandler(UserModel user, Models.Article.Statistics.Basics.CartonsCirculationRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();

				switch(this._data.Site)
				{
					//case Enums.ArticleEnums.StatsCartonsCirculationSites.Tunesien:
					//	statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCirculations_TN(this._data.DateFrom, this._data.DateTill)
					//		?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
					//	break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.WolfHalleKHTN:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCirculations_WS(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					//case Enums.ArticleEnums.StatsCartonsCirculationSites.BETN:
					//	statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCirculations_BETN(this._data.DateFrom, this._data.DateTill)
					//		?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
					//	break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.Albanien:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCirculations_AL(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.Tschechien:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCirculations_CZ(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.Vohenstrauss:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCirculations_DE(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.Gesamt:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCirculations_Gesamt(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					case Enums.ArticleEnums.StatsCartonsCirculationSites.GZTN:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCirculations_GZTN(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
					default:
						break;
				}

				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<string>.SuccessResponse(
						Convert.ToBase64String(
						GetFilePDF(
							statisticsEntities.Select(x => new Models.Article.Statistics.Basics.CirculationResponseModel(x)).ToList()))
						?? "");
				}

				return ResponseModel<string>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			return ResponseModel<string>.SuccessResponse();
		}

		public byte[] GetFilePDF(List<Models.Article.Statistics.Basics.CirculationResponseModel> dataEntities)
		{
			try
			{
				var data = new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsCartonsModel();

				// -
				data.Headers = new List<Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsCartonsModel.Header> {
					new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsCartonsModel.Header {
						From= this._data.DateFrom.ToString("dd/MM/yyyy"),
						To= decimal.TryParse(this._data.DateTill.ToString("dd/MM/yyyy"), out decimal v) == true? "": "",
						Lagerort= this._data.Site.GetDescription(),
					}
				};

				// - Items
				data.Items = dataEntities
					.Select(x => new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsCartonsModel.Item
					{
						Anzahl = (x.Anzahl ?? 0).ToString(),
						Artikelnummer_Umlauf = x.Artikelnummer_Umlauf,
						Bestand = (x.Bestand ?? 0).ToString(),
						Bestatigter_Termin = x.Bestatigter_Termin?.ToString("dd/MM/yyyy"),
						Bestellung_Nr = (x.Bestellung_Nr ?? 0).ToString(),
						Liefertermin = x.Liefertermin?.ToString("dd/MM/yyyy"),
						SummevonBedarf = (x.SummevonBedarf ?? 0).ToString(),
						Transfer_Bestand = (x.Transfer_Bestand ?? 0).ToString()
					})?.OrderBy(x => x.Artikelnummer_Umlauf)?.ThenBy(x => x.Bestellung_Nr).ToList();

				// -
				return Module.ReportingService.Generate_BSD_ArticlesStatisticsCirculations(Infrastructure.Services.Reporting.Helpers.ReportType.BSD_ART_STATS_CIRCULATIONS, data);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
