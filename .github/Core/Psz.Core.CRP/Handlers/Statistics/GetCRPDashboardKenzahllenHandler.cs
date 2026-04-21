using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Dashboard;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService
	{
		public static decimal CURRENT_TOTALSTOCK_FGBYYEAR = -1m;
		public static DateTime LASTUPDATE;
		public static int UPDATE_FREQ = 15;
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
				if(CURRENT_TOTALSTOCK_FGBYYEAR <= 0)
				{
					CURRENT_TOTALSTOCK_FGBYYEAR = Infrastructure.Data.Access.Joins.CRP.CRPDashboardAccess.GetTotalStockFGByYear();
					LASTUPDATE = DateTime.Now;
				}
				else if(Math.Abs((DateTime.Now - LASTUPDATE).TotalMinutes) < UPDATE_FREQ)
				{
					return ResponseModel<decimal>.SuccessResponse(
						CURRENT_TOTALSTOCK_FGBYYEAR);
				}
				LASTUPDATE = DateTime.Now;
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