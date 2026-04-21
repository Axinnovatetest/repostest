using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics.INS;
using Psz.Core.Identity.Models;
using System;
using System.Globalization;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<INSOverviewRückständige_BestellungenDetailsResponseModel> Get_INSOverviewRückständige_BestellungenDetails(UserModel user, INSOverviewRückständige_BestellungenDetailsRequestModel data)
		{
			if(data == null)
				return ResponseModel<INSOverviewRückständige_BestellungenDetailsResponseModel>.AccessDeniedResponse();

			try
			{
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.ToLower())
					{
						default:
						case "pos":
							sortFieldName = "aa.[Pos]";
							break;
						case "angebotnr":
							sortFieldName = "a.[Angebot-Nr]";
							break;
						case "artikelnummer":
							sortFieldName = "ar.[Artikelnummer]";
							break;
						case "anzahl":
							sortFieldName = "aa.[anzahl]";
							break;
						case "gesamtpreis":
							sortFieldName = "aa.[Gesamtpreis]";
							break;
						case "liefertermin":
							sortFieldName = "aa.[Liefertermin]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion
				string date = null;
				int week = 0, year = 0;
				if(data.Date.Contains("-"))
				{
					var correctFormat = $"{data.Date.Split("-")[2]}/{data.Date.Split("-")[1]}/{data.Date.Split("-")[0]}";
					if(DateTime.TryParseExact(correctFormat, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _date))
					{
						date = _date.ToString("dd/MM/yyyy");
					}
				}
				else
				{
					date = null;
					week = Convert.ToInt32(data.Date.Split('/')[0]);
					year = Convert.ToInt32(data.Date.Split('/')[1]);
				}
				int? userId = user.IsAdministrator || user.IsCorporateDirector || user.IsGlobalDirector || user.Access.Purchase.AllCustomers
						? null
						: user.Id;
				var entities = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Rückständige_BestellungenDetails(date, week, year, data.OlderDate, data.CustomerNumber, data.userId, data.SearchText, dataPaging, dataSorting, userId);
				var count = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_Rückständige_BestellungenDetails_Count(date, data.OlderDate, week, year, userId, data.SearchText);

				var response = new INSOverviewRückständige_BestellungenDetailsResponseModel
				{
					Items = entities?.Select(e => new INSOverviewRückständige_BestellungenDetailsModel(e)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<INSOverviewRückständige_BestellungenDetailsResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}