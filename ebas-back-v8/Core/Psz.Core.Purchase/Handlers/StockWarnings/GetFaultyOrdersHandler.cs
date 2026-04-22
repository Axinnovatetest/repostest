using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<FaultyOrdersResponseModel> GetFaultyOrders(UserModel user, FaultyRequestModel data)
		{
			if(user == null)
				return ResponseModel<FaultyOrdersResponseModel>.AccessDeniedResponse();
			try
			{

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				var dataSorting = new Infrastructure.Data.Access.Settings.SortingModel();
				dataSorting.SortFieldName = "BA.Bestätigter_Termin";
				dataSorting.SortDesc = false;
				#endregion

				var warehouse = data.Unit is not null
									? Enums.StockWarningEnums.GetWarehousesFromUnit((Enums.StockWarningEnums.StockWarningsUnits)data.Unit)
									: new List<int> { };
				var faultyOrders = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetFaultyOrders(warehouse, data.ArtiekNr, dataSorting, dataPaging);
				var faultyOrdersCount = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetFaultyOrdersCount(warehouse, data.ArtiekNr);

				var response = new FaultyOrdersResponseModel
				{
					Items = faultyOrders.Select(x => new FaultyOrdersModel(x)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = faultyOrdersCount > 0 ? faultyOrdersCount : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(faultyOrdersCount > 0 ? faultyOrdersCount : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<FaultyOrdersResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}