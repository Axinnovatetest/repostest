using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<DateTime?> GetLastComputeAgentExecutionTime(UserModel user)
		{
			if(user == null)
				return ResponseModel<DateTime?>.AccessDeniedResponse();
			try
			{

				var response = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetLastAgentExecutionTime();
				return ResponseModel<DateTime?>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}