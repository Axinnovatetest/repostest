using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.WorkScheduleDetails
{
	public class WorkscheduleOrder
	{
		public int Id { get; set; }
		public int OrderDisplayId { get; set; }
	}
	public class WorkScheduleDetailOrder
	{
		public List<WorkscheduleOrder> Orders { get; set; }
		public WorkScheduleDetailOrder()
		{
			Orders = new List<WorkscheduleOrder>();
		}
	}
}
