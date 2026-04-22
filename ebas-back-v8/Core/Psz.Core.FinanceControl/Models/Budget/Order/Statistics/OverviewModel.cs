using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Order.Statistics
{
	public class OverviewModel
	{
		public List<OverviewItemModel> Items { get; set; }

		public class OverviewItemModel
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string Value { get; set; }
			public int Count { get; set; }
		}
	}
}
