using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class OrdersHighestOrLowestAmountModel
	{
		public IEnumerable<OrdersAmountsModel> HighestAmounts { get; set; }
		public IEnumerable<OrdersAmountsModel> LowestAmounts { get; set; }
	}

	public class OrdersAmountsModel
	{
		public string OrderNumber { get; set; }
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public OrdersAmountsModel(Tuple<int, string, decimal> entity)
		{

			Id = entity.Item1;
			OrderNumber = entity.Item2;
			Amount = entity.Item3;
		}
	}
}