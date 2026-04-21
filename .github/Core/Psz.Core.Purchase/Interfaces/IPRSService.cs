using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Interfaces
{
	public interface IPRSService
	{
		ResponseModel<StockWarningsResponseModel> GetStockWarnings(UserModel user, StockWarningsRequesteModel data);
		ResponseModel<ArticlesResponseModel> GetArticles(UserModel user, ArticlesRequestModel data);
		ResponseModel<int> ForceComputeStockWarningsAgent(UserModel user);
		ResponseModel<DateTime?> GetLastComputeAgentExecutionTime(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetCountries(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetUnits(UserModel user, int country);
		ResponseModel<List<StcoWarningsNeedsInOtherPlantsModel>> GetNeedsInOtherPlants(UserModel user, ArtikelUnitRequestModel data);
		ResponseModel<List<StockWarningsFaViewModel>> GetFaView(UserModel user, ArtikelUnitRequestModel data);
		ResponseModel<List<StockWarningsPoViewModel>> GetPoView(UserModel user, ArtikelUnitRequestModel data);
		ResponseModel<List<StockWarningsLogsModel>> GetStockWarningsAgentLogs(UserModel user);
		ResponseModel<FaultyOrdersResponseModel> GetFaultyOrders(UserModel user, FaultyRequestModel data);
		ResponseModel<FaultyFasResponseModel> GetFaultyFas(UserModel user, FaultyRequestModel data);
		ResponseModel<List<StockWarningsPoViewModel>> GetUnconfirmedOrders(UserModel user, ArtikelUnitRequestModel data);
		ResponseModel<List<ExtraOrdersNeedsInOtherPlantsModel>> GetExtraOrdersNeedsInOtherPlants(UserModel user, ExtraOrdersNeedsInOtherPlantsRequestModel data);
		ResponseModel<byte[]> GetStockWarningAuswertungExcel(UserModel user, StockWarningAuswertungRequestModel data);
		ResponseModel<byte[]> GetExtraOrdersAuswertungExcel(UserModel user, ExtraOrdersNeedsInOtherPlantsRequestModel data);

	}
}