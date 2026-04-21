using MailKit.Search;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<FaultyFasResponseModel> GetFaultyFas(UserModel user, FaultyRequestModel data)
		{
			if(user == null)
				return ResponseModel<FaultyFasResponseModel>.AccessDeniedResponse();
			try
			{

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				var dataSorting = new Infrastructure.Data.Access.Settings.SortingModel();
				dataSorting.SortFieldName = "Termin_Bestätigt1";
				dataSorting.SortDesc = false;
				#endregion
				string SearchTerms = data.SearchTerms;

				var warehouse = data.Unit is not null
					? Enums.StockWarningEnums.GetWarehousesFromUnit((Enums.StockWarningEnums.StockWarningsUnits)data.Unit)
					: new List<int> { };
				var faultyOrders = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetFaultyFas(warehouse, data.ArtiekNr, SearchTerms, dataSorting, dataPaging);
				var faultyOrdersCount = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetFaultyFasCount(warehouse, data.ArtiekNr, SearchTerms);

				var response = new FaultyFasResponseModel
				{
					Items = faultyOrders.Select(x => new FaultyFasModel(x)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = faultyOrdersCount > 0 ? faultyOrdersCount : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(faultyOrdersCount > 0 ? faultyOrdersCount : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<FaultyFasResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}