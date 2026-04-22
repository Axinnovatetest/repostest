using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class CSContactFAReportModel
	{
		public CSContactFAReportHeaderModel Header { get; set; }
		public List<CSContactFAReportDetailsModel> Details { get; set; }
	}

	public class CSContactFAReportHeaderModel
	{
		public string DateFrom { get; set; }
		public string DateTo { get; set; }
		public Decimal Total { get; set; }
	}

	public class CSContactFAReportDetailsModel
	{
		public string Contact { get; set; }
		public int FACount { get; set; }
		public Decimal Percentage { get; set; }
	}
}
