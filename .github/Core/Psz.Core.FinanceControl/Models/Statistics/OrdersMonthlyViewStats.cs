using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class OrdersMonthlyViewStats
	{
		public IEnumerable<OrdersMonthlyViewByType> OverviewInternal { get; set; }
		public IEnumerable<OrdersMonthlyViewByType> OverviewExternal { get; set; }
		public IEnumerable<OrdersMonthlyViewByPoPaymentType> OverviewPurchase { get; set; }
		public IEnumerable<OrdersMonthlyViewByPoPaymentType> OverviewLeasing { get; set; }
	}
	public class OrdersMonthlyViewByType
	{
		public string Month { get; set; }
		public string Type { get; set; }
		public int Count { get; set; }
		public decimal Amount { get; set; }
		public OrdersMonthlyViewByType()
		{

		}
		public OrdersMonthlyViewByType(Tuple<string, int, string, int> entity, decimal amount)
		{
			Month = entity.Item1;
			Type = entity.Item3;
			Count = entity.Item4;
			Amount = amount;
		}
	}
	public class OrdersMonthlyViewByPoPaymentType
	{
		public string Month { get; set; }
		public string PoPaymentType { get; set; }
		public int Count { get; set; }
		public decimal Amount { get; set; }

		public OrdersMonthlyViewByPoPaymentType()
		{

		}
		public OrdersMonthlyViewByPoPaymentType(Tuple<string, int, string, int> entity, decimal amount)
		{
			Month = entity.Item1;
			PoPaymentType = entity.Item3;
			Count = entity.Item4;
			Amount = amount;
		}
	}
}
