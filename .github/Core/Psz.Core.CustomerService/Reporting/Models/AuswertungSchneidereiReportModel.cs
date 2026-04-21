using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class AuswertungSchneidereiReportModel
	{
		public List<AuswertungSchneidereiHeaderReport> Header { get; set; }
		public List<AuswertungSchneidereiDetailsReport> Details { get; set; }
	}
	public class AuswertungSchneidereiHeaderReport
	{
		public string Lager { get; set; }
		public int LagerId { get; set; }
	}
	public class AuswertungSchneidereiDetailsReport
	{
		public int KG { get; set; }
		public int KNG { get; set; }
		public string Woche { get; set; }
		public int KGesamt { get; set; }
		public string Datum_Bis { get; set; }
		public Decimal Percent { get; set; }

	}
}
