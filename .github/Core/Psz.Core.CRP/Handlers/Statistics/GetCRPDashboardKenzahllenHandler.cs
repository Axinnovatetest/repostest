using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Dashboard;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService
	{
		public ResponseModel<CRPDashboardKenzahllenModel> GetDashboardKenzahllen(Identity.Models.UserModel user, int year)
		{
			if(user == null)
				return ResponseModel<CRPDashboardKenzahllenModel>.AccessDeniedResponse();
			try
			{

				return ResponseModel<CRPDashboardKenzahllenModel>.SuccessResponse(new CRPDashboardKenzahllenModel
				{
					ActiveArticlesByYear = Infrastructure.Data.Access.Joins.CRP.CRPDashboardAccess.GetActiveArticlesByYear(year),
					OpenFasByYear = Infrastructure.Data.Access.Joins.CRP.CRPDashboardAccess.GetOpenFasByYear(year),
					OpenFasHours = Infrastructure.Data.Access.Joins.CRP.CRPDashboardAccess.GetOpenFasHoursByYear(year),

				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<decimal> GetDashboardStockFG(Identity.Models.UserModel user)
		{
			if(user == null)
				return ResponseModel<decimal>.AccessDeniedResponse();
			try
			{

				return ResponseModel<decimal>.SuccessResponse(
					Infrastructure.Data.Access.Joins.CRP.CRPDashboardAccess.GetTotalStockFGByYear());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}