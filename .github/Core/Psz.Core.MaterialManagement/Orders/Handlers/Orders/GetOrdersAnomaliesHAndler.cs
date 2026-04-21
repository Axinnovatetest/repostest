using Psz.Core.MaterialManagement.Orders.Models.Orders;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers
{
	public partial class OrderService
	{
		public ResponseModel<OrderPrioViewResponseModel> GetOrdersAnomalies(UserModel user, OrdersAnomaliesRequestModel data)
		{
			if(user == null)
			{
				return ResponseModel<OrderPrioViewResponseModel>.AccessDeniedResponse();
			}

			try
			{
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.Trim().ToLower())
					{
						case "ordernumber":
							sortFieldName = "b.[Bestellung-Nr]";
							break;
						case "projectnumber":
							sortFieldName = "b.[Projekt-Nr]";
							break;
						case "position":
							sortFieldName = "ba.Position";
							break;
						case "artikelnummer":
							sortFieldName = "a.Artikelnummer";
							break;
						case "liefertermin":
							sortFieldName = "ba.Liefertermin";
							break;
						case "bestatigter_termin":
							sortFieldName = "ba.Bestätigter_Termin";
							break;
						case "name_companyname":
							sortFieldName = "b.[Vorname/NameFirma]";
							break;
						default:
							sortFieldName = null;
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				var anomalies = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetOrdersAnomalies(data.SearchText, dataSorting, dataPaging);
				var allcount = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetOrdersAnomaliesCount(data.SearchText);
				var ordersList = anomalies.Select(x => new OrderPrioViewModel(x)).ToList();
				var counts = Infrastructure.Data.Access.Tables.PRS.OrderPlacementHistoryAccess.GetCountByOrderIds(ordersList?.Select(o => o.Nr));
				ordersList?.ForEach(o =>
				{
					var countPlacements = counts?.FirstOrDefault(x => x.Key == o.Nr);
					o.HavePlacements = countPlacements?.Value > 0;
				});
				return ResponseModel<OrderPrioViewResponseModel>.SuccessResponse(
					new OrderPrioViewResponseModel()
					{
						Items = ordersList,
						PageRequested = data.RequestedPage,
						PageSize = data.PageSize,
						TotalCount = allcount > 0 ? allcount : 0,
						TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allcount > 0 ? allcount : 0) / data.PageSize)) : 0,
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}