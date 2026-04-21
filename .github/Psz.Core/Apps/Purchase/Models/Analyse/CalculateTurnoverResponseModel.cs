using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Analyse
{
	public class CalculateTurnoverResponseModel
	{
		public decimal TotalTurnover { get; set; }
		public List<CustomerTurnoverModel> Customers { get; set; }
		public string RequestUnitType { get; set; }
		public int RequestYear { get; set; }
		public string RequestUnitData { get; set; }
		public DateTime CalculatedStartDate { get; set; }
		public DateTime CalculatedEndDate { get; set; }

		public class CustomerTurnoverModel
		{
			public int CustomerId { get; set; }
			public int CustomerNumber { get; set; }
			public string CustomerName { get; set; }
			public decimal TotalTurnover { get; set; }
		}
	}
}
