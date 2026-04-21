using Microsoft.AspNetCore.Mvc.Formatters;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Psz.Core.MaterialManagement.Enums.StatisticsEnums;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers
{
	public class GetOverviewHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Statistics.SupplierOverviewResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int? _data { get; set; }
		public GetOverviewHandler(Identity.Models.UserModel user, int? data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<Models.Statistics.SupplierOverviewResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Statistics.SupplierOverviewResponseModel();
				var supplierEntity = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierOverview(this._data)
					?? new Infrastructure.Data.Entities.Tables.BSD.PrsSuppliersEntity();
				if(supplierEntity is not null)
				{
					var supplierClosedOrdersN1 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierClosedOrdersByYear(this._data, DateTime.Today.Year - 1);
					var supplierClosedOrdersN2 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierClosedOrdersByYear(this._data, DateTime.Today.Year - 2);
					var supplierClosedOrdersN3 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierClosedOrdersByYear(this._data, DateTime.Today.Year - 3);
					var supplierClosedOrdersN4 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierClosedOrdersByYear(this._data, DateTime.Today.Year - 4);
					var supplierTotalOrders = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierTotalOrders(this._data, supplierEntity.SyncId ?? 0);

					var supplierClosedOrders = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierClosedOrders(this._data, supplierEntity.SyncId ?? 0);
					var supplierDelayedDelivery = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierDelayedDelivery(this._data, supplierEntity.SyncId ?? 0);
					var supplierOpenOrders = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierOpenOrders(this._data, supplierEntity.SyncId ?? 0);
					var supplierUnplacedOrders = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierUnplacedOrders(this._data, supplierEntity.SyncId ?? 0);
					var supplierUnconfirmedDelivery = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierUnconfirmedDelivery(this._data, supplierEntity.SyncId ?? 0);
					var supplierDeliveryOverdue = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierDeliveryOverdue(this._data, supplierEntity.SyncId ?? 0);
					var supplierNext4KwDelivery = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierNext4KwDelivery(this._data, supplierEntity.SyncId ?? 0);

					#region // - 
					responseBody.SupplierId = !this._data.HasValue ? -1 : supplierEntity.SupplierId ?? -1;
					responseBody.SupplierAddressNr = !this._data.HasValue ? -1 : supplierEntity.SupplierAddressNr ?? -1;
					responseBody.SupplierName = !this._data.HasValue ? "" : supplierEntity.SupplierName;
					responseBody.SupplierBlockedForFurtherBe = !this._data.HasValue ? false : supplierEntity.SupplierBlockedForFurtherBe ?? false;
					responseBody.SupplierAddressBlocked = !this._data.HasValue ? false : supplierEntity.SupplierAddressBlocked ?? false;
					responseBody.Stufe = !this._data.HasValue ? "" : supplierEntity.Stufe;
					responseBody.StandardActiveArticlesCount = !this._data.HasValue || this._data.Value <= 0
							? Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierArticles_Count(true, true)
							: supplierEntity.StandardActiveArticlesCount ?? 0;
					responseBody.StandardArticlesCount = !this._data.HasValue || this._data.Value <= 0
							? Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierArticles_Count(true, null)
							: supplierEntity.StandardArticlesCount ?? 0;
					responseBody.AllActiveArticlesCount = !this._data.HasValue || this._data.Value <= 0
							? Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierArticles_Count(null, true)
							: supplierEntity.AllActiveArticlesCount ?? 0;
					responseBody.AllArticlesCount = !this._data.HasValue || this._data.Value <= 0
							? Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierArticles_Count(null, null)
							: supplierEntity.AllArticlesCount ?? 0;
					responseBody.SyncId = supplierEntity.SyncId ?? 0;
					responseBody.SyncDate = supplierEntity.SyncDate ?? DateTime.MinValue;
					responseBody.Lagers = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierLagers();
					#endregion

					#region // -
					responseBody.CurrentYear = new Models.Statistics.SupplierYearItem
					{
						BeAmount = supplierClosedOrders?.Sum(x => x.BeArticleAmount ?? 0) ?? 0,
						BeArticleCount = supplierClosedOrders?.Sum(x => x.BeArticleCount ?? 0) ?? 0,
						BeCount = supplierClosedOrders?.Sum(x => x.BeCount ?? 0) ?? 0,
						Year = DateTime.Today.Year
					};
					responseBody.LastN1Year = new Models.Statistics.SupplierYearItem
					{
						BeAmount = supplierClosedOrdersN1?.Sum(x => x.BeArticleAmount ?? 0) ?? 0,
						BeArticleCount = supplierClosedOrdersN1?.Sum(x => x.BeArticleCount ?? 0) ?? 0,
						BeCount = supplierClosedOrdersN1?.Sum(x => x.BeCount ?? 0) ?? 0,
						Year = DateTime.Today.Year - 1
					};
					responseBody.LastN2Year = new Models.Statistics.SupplierYearItem
					{
						BeAmount = supplierClosedOrdersN2?.Sum(x => x.BeArticleAmount ?? 0) ?? 0,
						BeArticleCount = supplierClosedOrdersN2?.Sum(x => x.BeArticleCount ?? 0) ?? 0,
						BeCount = supplierClosedOrdersN2?.Sum(x => x.BeCount ?? 0) ?? 0,
						Year = DateTime.Today.Year - 2
					};
					responseBody.LastN3Year = new Models.Statistics.SupplierYearItem
					{
						BeAmount = supplierClosedOrdersN3?.Sum(x => x.BeArticleAmount ?? 0) ?? 0,
						BeArticleCount = supplierClosedOrdersN3?.Sum(x => x.BeArticleCount ?? 0) ?? 0,
						BeCount = supplierClosedOrdersN3?.Sum(x => x.BeCount ?? 0) ?? 0,
						Year = DateTime.Today.Year - 3
					};
					responseBody.LastN4Year = new Models.Statistics.SupplierYearItem
					{
						BeAmount = supplierClosedOrdersN4?.Sum(x => x.BeArticleAmount ?? 0) ?? 0,
						BeArticleCount = supplierClosedOrdersN4?.Sum(x => x.BeArticleCount ?? 0) ?? 0,
						BeCount = supplierClosedOrdersN4?.Sum(x => x.BeCount ?? 0) ?? 0,
						Year = DateTime.Today.Year - 4
					};
					#endregion

					#region - // - History Current year, N1, N2, N3 - // -
					responseBody.PurchaseTotalBeCountsCurrentYear = new List<Models.Statistics.SupplierPuchaseItem>();
					responseBody.PurchaseTotalBeAmountsCurrentYear = new List<Models.Statistics.SupplierPuchaseItem>();
					responseBody.PurchaseTotalBeCountsLastN1Year = new List<Models.Statistics.SupplierPuchaseItem>();
					responseBody.PurchaseTotalBeAmountsLastN1Year = new List<Models.Statistics.SupplierPuchaseItem>();
					responseBody.PurchaseTotalBeCountsLastN2Year = new List<Models.Statistics.SupplierPuchaseItem>();
					responseBody.PurchaseTotalBeAmountsLastN2Year = new List<Models.Statistics.SupplierPuchaseItem>();

					// - Paddings - add missing KW as 0
					for(int i = 0; i <= 53; i++)
					{
						responseBody.PurchaseTotalBeCountsCurrentYear.Add(new Models.Statistics.SupplierPuchaseItem
						{
							Index = i,
							Kw = $"{DateTime.Today.Year}/{i}",
							KwValue = supplierClosedOrders?.Where(x => x.BeYear == DateTime.Today.Year && x.BeKw == i)?.Sum(x => x.BeArticleCount ?? 0) ?? 0
						});
						responseBody.PurchaseTotalBeAmountsCurrentYear.Add(new Models.Statistics.SupplierPuchaseItem
						{
							Index = i,
							Kw = $"{DateTime.Today.Year}/{i}",
							KwValue = Math.Round(supplierClosedOrders?.Where(x => x.BeYear == DateTime.Today.Year && x.BeKw == i)?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, 3)
						});

						// - last N1
						responseBody.PurchaseTotalBeCountsLastN1Year.Add(new Models.Statistics.SupplierPuchaseItem
						{
							Index = i,
							Kw = $"{DateTime.Today.Year}/{i}",
							KwValue = supplierClosedOrdersN1?.Where(x => x.BeYear == DateTime.Today.Year - 1 && x.BeKw == i)?.Sum(x => x.BeArticleCount ?? 0) ?? 0
						});
						responseBody.PurchaseTotalBeAmountsLastN1Year.Add(new Models.Statistics.SupplierPuchaseItem
						{
							Index = i,
							Kw = $"{DateTime.Today.Year}/{i}",
							KwValue = Math.Round(supplierClosedOrdersN1?.Where(x => x.BeYear == DateTime.Today.Year - 1 && x.BeKw == i)?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, 3)
						});

						// - last N2
						responseBody.PurchaseTotalBeCountsLastN2Year.Add(new Models.Statistics.SupplierPuchaseItem
						{
							Index = i,
							Kw = $"{DateTime.Today.Year}/{i}",
							KwValue = supplierClosedOrdersN2?.Where(x => x.BeYear == DateTime.Today.Year - 2 && x.BeKw == i)?.Sum(x => x.BeArticleCount ?? 0) ?? 0
						});
						responseBody.PurchaseTotalBeAmountsLastN2Year.Add(new Models.Statistics.SupplierPuchaseItem
						{
							Index = i,
							Kw = $"{DateTime.Today.Year}/{i}",
							KwValue = Math.Round(supplierClosedOrdersN2?.Where(x => x.BeYear == DateTime.Today.Year - 2 && x.BeKw == i)?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, 3)
						});
					}
					#endregion

					#region - // - Current Year Be summary - // -  
					responseBody.BeTotal = new List<Models.Statistics.SupplierLagerItem>();
					responseBody.BeClosed = new List<Models.Statistics.SupplierLagerItem>();
					responseBody.BeDelays = new List<Models.Statistics.SupplierLagerItem>();
					responseBody.BeOpen = new List<Models.Statistics.SupplierLagerItem>();
					responseBody.BeUnplaced = new List<Models.Statistics.SupplierLagerItem>();
					responseBody.BeUnconfirmed = new List<Models.Statistics.SupplierLagerItem>();
					responseBody.BeDeliveryOverdue = new List<Models.Statistics.SupplierLagerItem>();
					responseBody.BeNext4KwDelivery = new List<Models.Statistics.SupplierLagerItem>();
					foreach(var lager in responseBody.Lagers ?? new List<KeyValuePair<int, string>>())
					{
						var _supplierTotalOrders = supplierTotalOrders?.Where(x => x.LagerId == lager.Key);
						var _supplierClosedOrders = supplierClosedOrders?.Where(x => x.LagerId == lager.Key);
						var _supplierDelayedDelivery = supplierDelayedDelivery?.Where(x => x.LagerId == lager.Key);
						var _supplierOpenOrders = supplierOpenOrders?.Where(x => x.LagerId == lager.Key);
						var _supplierUnplacedOrders = supplierUnplacedOrders?.Where(x => x.LagerId == lager.Key);
						var _supplierUnconfirmedDelivery = supplierUnconfirmedDelivery?.Where(x => x.LagerId == lager.Key);
						var _supplierDeliveryOverdue = supplierDeliveryOverdue?.Where(x => x.LagerId == lager.Key);
						var _supplierNext4KwDelivery = supplierNext4KwDelivery?.Where(x => x.LagerId == lager.Key);

						responseBody.BeTotal.Add(new Models.Statistics.SupplierLagerItem { BeCount = (_supplierTotalOrders?.Sum(x => x.BeCount ?? 0) ?? 0), BeAmount = (_supplierTotalOrders?.Sum(x => x.BeArticleAmount ?? 0) ?? 0), Lager = lager.Key, BeArticleCount = (_supplierTotalOrders?.Sum(x => x.BeArticleCount ?? 0) ?? 0) });
						responseBody.BeClosed.Add(new Models.Statistics.SupplierLagerItem { BeCount = _supplierClosedOrders?.Sum(x => x.BeCount ?? 0) ?? 0, BeAmount = _supplierClosedOrders?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, Lager = lager.Key, BeArticleCount = _supplierClosedOrders?.Sum(x => x.BeArticleCount ?? 0) ?? 0 });
						responseBody.BeDelays.Add(new Models.Statistics.SupplierLagerItem { BeCount = _supplierDelayedDelivery?.Sum(x => x.BeCount ?? 0) ?? 0, BeAmount = _supplierDelayedDelivery?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, Lager = lager.Key, BeArticleCount = _supplierDelayedDelivery?.Sum(x => x.BeArticleCount ?? 0) ?? 0 });
						responseBody.BeOpen.Add(new Models.Statistics.SupplierLagerItem { BeCount = _supplierOpenOrders?.Sum(x => x.BeCount ?? 0) ?? 0, BeAmount = _supplierOpenOrders?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, Lager = lager.Key, BeArticleCount = _supplierOpenOrders?.Sum(x => x.BeArticleCount ?? 0) ?? 0 });
						responseBody.BeUnplaced.Add(new Models.Statistics.SupplierLagerItem { BeCount = _supplierUnplacedOrders?.Sum(x => x.BeCount ?? 0) ?? 0, BeAmount = _supplierUnplacedOrders?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, Lager = lager.Key, BeArticleCount = _supplierUnplacedOrders?.Sum(x => x.BeArticleCount ?? 0) ?? 0 });
						responseBody.BeUnconfirmed.Add(new Models.Statistics.SupplierLagerItem { BeCount = _supplierUnconfirmedDelivery?.Sum(x => x.BeCount ?? 0) ?? 0, BeAmount = _supplierUnconfirmedDelivery?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, Lager = lager.Key, BeArticleCount = _supplierUnconfirmedDelivery?.Sum(x => x.BeArticleCount ?? 0) ?? 0 });
						responseBody.BeDeliveryOverdue.Add(new Models.Statistics.SupplierLagerItem { BeCount = _supplierDeliveryOverdue?.Sum(x => x.BeCount ?? 0) ?? 0, BeAmount = _supplierDeliveryOverdue?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, Lager = lager.Key, BeArticleCount = _supplierDeliveryOverdue?.Sum(x => x.BeArticleCount ?? 0) ?? 0 });
						responseBody.BeNext4KwDelivery.Add(new Models.Statistics.SupplierLagerItem { BeCount = _supplierNext4KwDelivery?.Sum(x => x.BeCount ?? 0) ?? 0, BeAmount = _supplierNext4KwDelivery?.Sum(x => x.BeArticleAmount ?? 0) ?? 0, Lager = lager.Key, BeArticleCount = _supplierNext4KwDelivery?.Sum(x => x.BeArticleCount ?? 0) ?? 0 });
					}
					#endregion
				}
				// -
				return ResponseModel<Models.Statistics.SupplierOverviewResponseModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Statistics.SupplierOverviewResponseModel> Validate()
		{
			if(this._user is null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Statistics.SupplierOverviewResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Statistics.SupplierOverviewResponseModel>.SuccessResponse();
		}
	}
}
