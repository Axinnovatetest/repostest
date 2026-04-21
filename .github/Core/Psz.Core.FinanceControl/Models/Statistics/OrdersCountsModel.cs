using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class OrdersCountsModel
	{
		public IEnumerable<OrdersCountByPoPaymentType> ByPoPaymentType { get; set; }
		public IEnumerable<OrdersCountByType> ByType { get; set; }
		public IEnumerable<OrdersCountByStatus> ByStatus { get; set; }
		public int Placed { get; set; }
		public int Booked { get; set; }
	}
	public class OrdersCountByPoPaymentType
	{
		public int Count { get; set; }
		public string PoPaymentTypeName { get; set; }
	}
	public class OrdersCountByType
	{
		public int Count { get; set; }
		public string OrderType { get; set; }
	}
	public class OrdersCountByStatus
	{
		public int Count { get; set; }
		public string Status { get; set; }
	}
}