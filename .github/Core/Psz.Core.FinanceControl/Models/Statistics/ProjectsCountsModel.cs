using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class ProjectsStatsModel
	{
		public List<ProjectsCountsModel> Counts { get; set; }
		public int Internal { get; set; }
		public int External { get; set; }
		public int Finance { get; set; }
	}
	public class ProjectsCountsModel
	{
		public string Status { get; set; }
		public int Count { get; set; }
	}
}
