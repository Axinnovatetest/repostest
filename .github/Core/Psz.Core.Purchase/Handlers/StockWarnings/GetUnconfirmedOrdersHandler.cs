using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<List<StockWarningsPoViewModel>> GetUnconfirmedOrders(UserModel user, ArtikelUnitRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<StockWarningsPoViewModel>>.AccessDeniedResponse();
			try
			{
				var warehouses = Enums.StockWarningEnums.GetWarehousesFromUnit((Enums.StockWarningEnums.StockWarningsUnits)data.Unit);
				var unconfirmedOrders = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetUnconfirmedOrders(warehouses, data.ArtikelNr);

				var response = unconfirmedOrders.Select(x => new StockWarningsPoViewModel(x)).ToList();
				return ResponseModel<List<StockWarningsPoViewModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}