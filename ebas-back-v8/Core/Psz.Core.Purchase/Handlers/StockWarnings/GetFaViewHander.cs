using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<List<StockWarningsFaViewModel>> GetFaView(UserModel user, ArtikelUnitRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<StockWarningsFaViewModel>>.AccessDeniedResponse();
			try
			{
				var unit = ((Enums.StockWarningEnums.StockWarningsUnits)data.Unit).GetDescription();
				var fas = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetFaView(data.Unit, data.ArtikelNr, data.Year ?? 0, data.Week ?? 0, data.Backlog, unit, data.Unit == (int)Enums.StockWarningEnums.StockWarningsUnits.WS_TN);
				var response = fas?.Select(f => new StockWarningsFaViewModel(f)).ToList();

				return ResponseModel<List<StockWarningsFaViewModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}