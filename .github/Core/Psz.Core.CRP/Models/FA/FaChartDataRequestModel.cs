

using Psz.Core.MaterialManagement.Settings;

namespace Psz.Core.CRP.Models.FA
{
	public class FaChartDataRequestModel
	{
		public List<int> LagerIds { get; set; }
		public DateTime? AffectedWeekStartDate { get; set; }
	}
}
