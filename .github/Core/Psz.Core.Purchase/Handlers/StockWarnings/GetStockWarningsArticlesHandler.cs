using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Interfaces;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService: IPRSService
	{
		public ResponseModel<ArticlesResponseModel> GetArticles(UserModel user, ArticlesRequestModel data)
		{
			if(user == null)
				return ResponseModel<ArticlesResponseModel>.AccessDeniedResponse();
			try
			{
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				var dataSorting = new Infrastructure.Data.Access.Settings.SortingModel();
				dataSorting.SortFieldName = "[Artikelnummer]";
				dataSorting.SortDesc = false;
				#endregion
				var unit = data.UnitId is not null
					? ((Enums.StockWarningEnums.StockWarningsUnits)data.UnitId).GetDescription()
					: null;
				var articles = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetArticles(unit, data.Prio, data.Artikelnummer, dataSorting, dataPaging);
				var count = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetArticlesCount(unit, data.Prio, data.Artikelnummer);

				var response = new ArticlesResponseModel
				{
					Items = articles,
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<ArticlesResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}


