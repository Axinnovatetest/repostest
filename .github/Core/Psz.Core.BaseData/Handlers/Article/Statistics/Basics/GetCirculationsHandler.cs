using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetCirculationsHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Basics.CirculationResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.CartonsCirculationRequestModel _data { get; set; }
		public GetCirculationsHandler(UserModel user, Models.Article.Statistics.Basics.CartonsCirculationRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.Basics.CirculationResponseModel>> Handle()
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
					return ResponseModel<List<Models.Article.Statistics.Basics.CirculationResponseModel>>.SuccessResponse(
						statisticsEntities.Select(x => new Models.Article.Statistics.Basics.CirculationResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Basics.CirculationResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Basics.CirculationResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Basics.CirculationResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Basics.CirculationResponseModel>>.SuccessResponse();
		}

	}
}
