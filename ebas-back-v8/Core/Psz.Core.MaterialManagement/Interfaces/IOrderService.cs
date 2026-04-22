using Psz.Core.MaterialManagement.Orders.Models.Orders;

namespace Psz.Core.MaterialManagement.Interfaces
{
	public interface IOrderService
	{
		public ResponseModel<int> TogglePurchaseProject(UserModel user, int data);
		public ResponseModel<OrderPrioViewResponseModel> GetOrdersPrioView(UserModel user, GetRequestModel data);
		public ResponseModel<OrderPrioViewResponseModel> GetOrdersAnomalies(UserModel user, OrdersAnomaliesRequestModel data);
		public ResponseModel<int> UpdateConfirmedDateAndComment(UserModel user, ConfirmedDateAndCommentModel data);
		public ResponseModel<List<KeyValuePair<int, string>>> GetArticlesForRahmenFilter(UserModel user, string searchText);
		public ResponseModel<ROHArticleRahmenNeedsResponseModel> GetROHRahmenNeeds(UserModel user, int artikelNr);
		public ResponseModel<List<NeedsInRahmenSaleModel>> GetNeedsInRahmenSaleDetails(UserModel user, int artikelNr);
		public ResponseModel<int> AcitvateROH_RA_NeedsAgent(UserModel user);
		public ResponseModel<DateTime?> GetROH_RA_NeedsAgent_LastExecution(UserModel user);
		public ResponseModel<List<KeyValuePair<DateTime, string>>> GetROH_RA_NeedsAgentLogs(UserModel user);
	}
}