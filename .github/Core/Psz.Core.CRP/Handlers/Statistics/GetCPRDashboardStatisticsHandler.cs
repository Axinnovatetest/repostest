using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Dashboard;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService
	{
		public ResponseModel<CRPDashboardStatisticsModel> GetDashboardStatistics(Identity.Models.UserModel user, CRPDashboardRequestModel data)
		{
			if(user == null)
				return ResponseModel<CRPDashboardStatisticsModel>.AccessDeniedResponse();
			try
			{

				return ResponseModel<CRPDashboardStatisticsModel>.SuccessResponse(new CRPDashboardStatisticsModel
				{
					CreatedFas = Infrastructure.Data.Access.Joins.CRP.CRPDashboardAccess.GetCreatedFas(data.Month, data.Year),
					CancelledFas = Infrastructure.Data.Access.Joins.CRP.CRPDashboardAccess.GetCancelledFas(data.Month, data.Year),
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}