using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Forecasts;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService
	{
		public ResponseModel<ForecastPositionsResponseModel> GetForecastPositions(UserModel user, ForecastPositonsRequestModel data)
		{
			if(user == null)
				return ResponseModel<ForecastPositionsResponseModel>.AccessDeniedResponse();
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
						case "artikelnummer":
							sortFieldName = "[Artikelnummer]";
							break;
						case "material":
							sortFieldName = "[Material]";
							break;
						case "menge":
							sortFieldName = "[Menge]";
							break;
						case "datum":
							sortFieldName = "[Datum]";
							break;
						case "jahr":
							sortFieldName = "[Jahr]";
							break;
						case "kw":
							sortFieldName = "[KW]";
							break;
						case "vke":
							sortFieldName = "[VKE]";
							break;
						case "gesampreis":
							sortFieldName = "[GesamtPreis]";
							break;
						case "isordered":
							sortFieldName = "[IsOrdered]";
							break;
					}
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion
				var positions = Infrastructure.Data.Access.Tables.CRP.ForecastsPositionAccess.GetByForecastIdPaginated(data.Id, data.SearchText,
					dataSorting, dataPaging);
				var positionsCount = Infrastructure.Data.Access.Tables.CRP.ForecastsPositionAccess.GetByForecastIdPaginatedCount(data.Id, data.SearchText);

				return ResponseModel<ForecastPositionsResponseModel>.SuccessResponse(
					new ForecastPositionsResponseModel()
					{
						Items = positions?.Select(x => new ForecastPositionModel(x)).ToList(),
						PageRequested = data.RequestedPage,
						PageSize = data.PageSize,
						TotalCount = positionsCount > 0 ? positionsCount : 0,
						TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(positionsCount > 0 ? positionsCount : 0) / data.PageSize)) : 0,
					});
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
