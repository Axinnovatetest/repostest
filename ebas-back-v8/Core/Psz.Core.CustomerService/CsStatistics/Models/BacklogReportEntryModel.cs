using Psz.Core.Common.Models;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class BacklogReportEntryModel: IDateRangeModel
	{
		public string Mandant { get; set; }
		public int? Lager { get; set; }
		//public int Code { get; set; }
	}
}
