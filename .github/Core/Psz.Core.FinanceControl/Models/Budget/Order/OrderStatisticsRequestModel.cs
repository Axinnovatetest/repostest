using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderStatisticsRequestModel: IPaginatedRequestModel
	{
		public int Year { get; set; }
		public string Type { get; set; }
		public List<int>? CompanyIds { get; set; }
		public string Filter { get; set; }
	}
}
