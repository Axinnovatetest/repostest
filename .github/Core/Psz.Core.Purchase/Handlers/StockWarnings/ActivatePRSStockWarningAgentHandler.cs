using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<int> ForceComputeStockWarningsAgent(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			try
			{
				var response = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.PrsStockWarningsData(user.Id);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.PRS_StockWarnings_Agents, false);
				throw;
			}
		}
	}
}