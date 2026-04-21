using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class BestSuppliersStatsModel
	{
		public List<OrdersCountModel> BestSuppliersOrdersCount { get; set; }
		public List<OrdersAmountModel> BestSuppliersOrdersAmount { get; set; }
	}
	public class OrdersCountModel
	{
		public string Supplier { get; set; }
		public int OrdersCount { get; set; }
		public OrdersCountModel(KeyValuePair<string, int> entity)
		{
			Supplier = entity.Key;
			OrdersCount = entity.Value;
		}
	}
	public class OrdersAmountModel
	{
		public string Supplier { get; set; }
		public decimal OrdersAmount { get; set; }
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public OrdersAmountModel(KeyValuePair<string, decimal> entity)
		{
			Supplier = entity.Key;
			OrdersAmount = entity.Value;
		}
	}
}