using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<List<StockWarningsLogsModel>> GetStockWarningsAgentLogs(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<StockWarningsLogsModel>>.AccessDeniedResponse();
			try
			{

				var logs = Infrastructure.Data.Access.Joins.PRS.__PRS_StockWarnings_ComputeLogsAccess.GetWithUsername();
				var response = logs?.Select(x => new StockWarningsLogsModel(x)).ToList();

				return ResponseModel<List<StockWarningsLogsModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}