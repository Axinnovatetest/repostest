using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<List<StockWarningsPoViewModel>> GetPoView(UserModel user, ArtikelUnitRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<StockWarningsPoViewModel>>.AccessDeniedResponse();
			try
			{
				var unit = ((Enums.StockWarningEnums.StockWarningsUnits)data.Unit).GetDescription();
				var warehouses = Enums.StockWarningEnums.GetWarehousesFromUnit((Enums.StockWarningEnums.StockWarningsUnits)data.Unit);
				var pos = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetPoView(warehouses, data.ArtikelNr, data.Backlog, data.Year ?? 0, data.Week ?? 0, unit);
				var response = pos?.Select(p => new StockWarningsPoViewModel(p)).ToList();

				return ResponseModel<List<StockWarningsPoViewModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}