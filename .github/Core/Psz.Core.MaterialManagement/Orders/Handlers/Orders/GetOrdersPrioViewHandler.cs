using Psz.Core.MaterialManagement.Interfaces;
using Psz.Core.MaterialManagement.Orders.Models.Orders;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers

{
	public partial class OrderService
	{
		public ResponseModel<OrderPrioViewResponseModel> GetOrdersPrioView(UserModel user, GetRequestModel data)
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
						case "typ":
							sortFieldName = "b.Typ";
							break;
						case "konditionen":
							sortFieldName = "b.Konditionen";
							break;
						case "datum":
							sortFieldName = "b.Datum";
							break;
						case "bearbeiter":
							sortFieldName = "u.[Name]";
							break;
						case "projekt_nr":
							sortFieldName = "b.[Projekt-Nr]";
							break;
						case "bestellung_nr":
							sortFieldName = "b.[Bestellung-Nr]";
							break;
						case "vorname_namefirma":
							sortFieldName = "b.[Vorname/NameFirma]";
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
						case "storagelocation":
							sortFieldName = "l.Lagerort";
							break;
						case "status":
							sortFieldName = "b.gebucht";
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
				var orders = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetOrdersPrioView(
					dataSorting,
					dataPaging,
					data.Lieferanten_Nr,
					data.OrderType,
					data.Bestellung_Nr,
					data.Benutzer,
					data.ArtikelNr,
					data.Projekt_Nr,
					data.CreationDateFrom,
					data.CreationDateTo,
					data.DraftsOnly,
					data.CanCreateWereingang,
					data.ProjectPurchase ?? false,
					data.IncludeDone ?? false);

				var ordersCount = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetOrdersPrioViewCount(
					data.Lieferanten_Nr,
					data.OrderType,
					data.Bestellung_Nr,
					data.Benutzer,
					data.ArtikelNr,
					data.Projekt_Nr,
					data.CreationDateFrom,
					data.CreationDateTo,
					data.DraftsOnly,
					data.CanCreateWereingang,
					data.ProjectPurchase ?? false,
					data.IncludeDone ?? false);

				var ordersList = orders.Select(x => new OrderPrioViewModel(x)).ToList();
				var counts = Infrastructure.Data.Access.Tables.PRS.OrderPlacementHistoryAccess.GetCountByOrderIds(ordersList?.Select(o=> o.Nr));
				ordersList?.ForEach(o =>
				{
					var countPlacements = counts?.FirstOrDefault(x=>x.Key == o.Nr);
					o.HavePlacements = countPlacements?.Value > 0;
				});

				return ResponseModel<OrderPrioViewResponseModel>.SuccessResponse(
					new OrderPrioViewResponseModel()
					{
						Items = ordersList,
						PageRequested = data.RequestedPage,
						PageSize = data.PageSize,
						TotalCount = ordersCount > 0 ? ordersCount : 0,
						TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(ordersCount > 0 ? ordersCount : 0) / data.PageSize)) : 0,
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}