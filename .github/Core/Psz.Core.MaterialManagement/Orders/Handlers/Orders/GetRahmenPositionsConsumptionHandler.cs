using Psz.Core.MaterialManagement.Orders.Models.Orders;
using System.Linq;


namespace Psz.Core.MaterialManagement.Orders.Handlers
{
	public partial class OrderService
	{
		//public ResponseModel<RahmenPositionsConsumptionResponseModel> GetRahmenPositionsConsumption(UserModel user, RahmenPositionsConsumptionRequestModel data)
		//{
		//	if(user == null)
		//	{
		//		return ResponseModel<RahmenPositionsConsumptionResponseModel>.AccessDeniedResponse();
		//	}

		//	try
		//	{
		//		#region > Data sorting & paging
		//		var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
		//		{
		//			FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
		//			RequestRows = data.PageSize
		//		};

		//		Infrastructure.Data.Access.Settings.SortingModel dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
		//		{
		//			SortFieldName = "bea.ExtensionDate",
		//			SortDesc = true,
		//		};
		//		if(!string.IsNullOrWhiteSpace(data.SortField))
		//		{
		//			var sortFieldName = "";
		//			dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
		//			{
		//				SortFieldName = sortFieldName,
		//				SortDesc = data.SortDesc,
		//			};
		//		}
		//		#endregion

		//		var entities = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetRahmenPositionsConsumption(data.SearchText, dataSorting, dataPaging);
		//		var allCount = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetRahmenPositionsConsumptionCount(data.SearchText);

		//		return ResponseModel<RahmenPositionsConsumptionResponseModel>.SuccessResponse(
		//			new RahmenPositionsConsumptionResponseModel()
		//			{
		//				Items = entities?.Select(e => new RahmenPositionsConsumptionModel(e)).ToList(),
		//				PageRequested = data.RequestedPage,
		//				PageSize = data.PageSize,
		//				TotalCount = allCount > 0 ? allCount : 0,
		//				TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / data.PageSize)) : 0,
		//			});
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		throw;
		//	}
		//}
	}
}
