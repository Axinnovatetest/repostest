using Psz.Core.Common.Models;
using Psz.Core.CRP.Models;
using Psz.Core.Identity.Models;
using Psz.Core.CRP.Models.Dashboard;


namespace Psz.Core.CRP.Interfaces
{
	public interface ICrpStatisticsService
	{
		ResponseModel<OrderProcessingSearchLogsModel> GetOrderProcessingLogs(Identity.Models.UserModel user, OPSearchLogsModel data);
		ResponseModel<byte[]> GetLagerBestandFG_CRPExcel(Identity.Models.UserModel user);
		ResponseModel<byte[]> DownloadExcelOrderProcessingLogs(Identity.Models.UserModel user, OPSearchLogsModel data);
		ResponseModel<List<string>> AutoComplete(Identity.Models.UserModel user, int columnValue, string searchValue);
		ResponseModel<byte[]> GetCRPAuswertungRahmenFGArtikel(UserModel user);
		ResponseModel<CRPDashboardStatisticsModel> GetDashboardStatistics(Identity.Models.UserModel user, CRPDashboardRequestModel data);
		ResponseModel<CRPDashboardKenzahllenModel> GetDashboardKenzahllen(Identity.Models.UserModel user, int year);
		ResponseModel<decimal> GetDashboardStockFG(Identity.Models.UserModel user);
	}
}