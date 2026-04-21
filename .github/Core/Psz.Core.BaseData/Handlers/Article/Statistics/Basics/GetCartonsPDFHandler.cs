using System;
using System.Collections.Generic;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;
using System.Threading.Tasks;
namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	public class GetCartonsPDFHandler: IHandleAsync<UserModel, ResponseModel<string>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.CartonsCirculationRequestModel _data { get; set; }
		public GetCartonsPDFHandler(UserModel user, Models.Article.Statistics.Basics.CartonsCirculationRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<string>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();

				switch(this._data.Site)
				{
					case Enums.ArticleEnums.StatsCartonsCirculationSites.WolfHalleKHTN:
						statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetCartons_WS(this._data.DateFrom, this._data.DateTill)
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf>();
						break;
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

				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<string>.SuccessResponse(
						Convert.ToBase64String(
						await GetFilePDF(
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
		public async Task<ResponseModel<string>> ValidateAsync()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return await ResponseModel<string>.AccessDeniedResponseAsync();
			}

			return await ResponseModel<string>.SuccessResponseAsync();
		}
		public async Task<byte[]> GetFilePDF(List<Models.Article.Statistics.Basics.CirculationResponseModel> dataEntities)
		{
			try
			{
				var data = new Reporting.Models.StatisticsCartonsModel();

				// -
				data.Headers = new Reporting.Models.StatisticsCartonsModel.Header
				{
					From = this._data.DateFrom.ToString("dd/MM/yyyy"),
					To = this._data.DateTill.ToString("dd/MM/yyyy"),
					Lagerort = this._data.Site.GetDescription(),
				};

				// - Items
				data.Items = dataEntities
					.Select(x => new Reporting.Models.StatisticsCartonsModel.Item
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
				var response = await Reporting.IText.GetItextPDF(new Reporting.Models.ITextHeaderFooterProps
				{
					BodyData = data,
					BodyTemplate = "bsd_article_kartons_body",
					DocumentTitle = "",
					FooterCenterText = "",
					FooterData = null,
					FooterLeftText = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
					FooterTemplate = "BSD_Footer",
					FooterWithCounter = true,
					HasFooter = true,
					HasHeader = true,
					HeaderFirstPageOnly = true,
					HeaderLogoWithCounter = false,
					HeaderLogoWithText = true,
					HeaderText = "PSZ Electronic GmbH",
					Logo = $"data:image/png;base64,{System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo)}",
					Rotate = false
				});
				return response;
				//Module.ReportingService.Generate_BSD_ArticlesStatisticsCartons(Infrastructure.Services.Reporting.Helpers.ReportType.BSD_ART_STATS_CARTONS, data);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}