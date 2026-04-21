using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics.INS;
using Psz.Core.Identity.Models;
using System.Linq;
using System;

namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<INSOverviewUmsatz_Aktuelle_WocheDetailsResponseModel> Get_INSOverviewUmsatz_Aktuelle_WocheDetails(UserModel user, INSOverviewUmsatz_Aktuelle_WocheDetailsRequestModel data)
		{
			if(user == null)
				return ResponseModel<INSOverviewUmsatz_Aktuelle_WocheDetailsResponseModel>.AccessDeniedResponse();

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

				var entities = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_INSOverviewUmsatz_Aktuelle_WocheDetails(data.Date, data.SearchText, dataPaging, dataSorting);
				var count = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.Get_INSOverviewUmsatz_Aktuelle_WocheDetails_Count(data.Date, data.SearchText);

				var response = new INSOverviewUmsatz_Aktuelle_WocheDetailsResponseModel
				{
					Items = entities?.Select(e => new INSOverviewUmsatz_Aktuelle_WocheDetailsModel(e)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<INSOverviewUmsatz_Aktuelle_WocheDetailsResponseModel>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}