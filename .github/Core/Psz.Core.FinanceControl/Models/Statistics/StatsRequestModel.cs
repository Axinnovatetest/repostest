using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class StatsRequestModel
	{
		public int? Year { get; set; }
		public int? CompanyId { get; set; }
		public int? DepartmentId { get; set; }
		public int? EmployeeId { get; set; }
	}
	public class StatsRequestModelMonth: StatsRequestModel
	{
		public int? Month
		{
			get; set;
		}
	}
	public class StatsRequestModelStatus: StatsRequestModel
	{
		public string Status
		{
			get; set;
		}
	}
	public class StatsRequestModelType: StatsRequestModel
	{
		public string Type
		{
			get; set;
		}
	}
	public class StatsRequestModelOrdersOverview: StatsRequestModel
	{
		public string Text
		{
			get; set;
		}
	}
	public class StatsRequestModelOrdersOverviewMonthly: StatsRequestModelOrdersOverview
	{
		public string Month { get; set; }
	}
	public class StatsRequestBestSuppliersOverviewOrdersCount: StatsRequestModel
	{
		public string Supplier { get; set; }
	}
}