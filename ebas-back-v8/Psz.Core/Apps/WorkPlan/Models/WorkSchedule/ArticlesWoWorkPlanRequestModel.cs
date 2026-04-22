using System;

namespace Psz.Core.Apps.WorkPlan.Models.WorkSchedule
{
	public class ArticlesWoWorkPlanRequestModel
	{
		public bool? warengruppeEF { get; set; }
		public bool? wStuckliste { get; set; }
		public bool? wFa { get; set; }
		public bool? wOpenFa { get; set; }
		public int? lager { get; set; }
		public DateTime? faDateFrom { get; set; }
		public DateTime? faDateTill { get; set; }
	}
}
