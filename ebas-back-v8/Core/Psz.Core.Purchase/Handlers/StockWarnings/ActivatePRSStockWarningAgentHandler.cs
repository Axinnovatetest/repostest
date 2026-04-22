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
				Infrastructure.Services.Utils.TransactionsManager botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				botransaction.beginTransaction();
				var response = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.PrsStockWarningsData(user.Id);
				Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.PRS_StockWarnings_Agents, false);
				if(botransaction.commit())
				{
					Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.PRS_StockWarnings_Agents, false);
					return ResponseModel<int>.SuccessResponse(response);
				}
				Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.PRS_StockWarnings_Agents, false);
				return ResponseModel<int>.SuccessResponse(0);
			} catch(Exception e)
			{
				Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.PRS_StockWarnings_Agents, false);
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}