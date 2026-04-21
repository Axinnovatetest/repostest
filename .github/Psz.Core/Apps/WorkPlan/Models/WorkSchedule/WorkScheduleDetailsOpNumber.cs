using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.WorkScheduleDetails
{
	public class WorkScheduleDetailsOpNumber
	{
		public int Id { get; set; }
		public int OperationNumber { get; set; }
		public int PredecessorOperation { get; set; }
	}
	public class WorkScheduleDetailsOpNumbers
	{
		public List<WorkScheduleDetailsOpNumber> OperationNumber { get; set; }
		public WorkScheduleDetailsOpNumbers()
		{
			OperationNumber = new List<WorkScheduleDetailsOpNumber>();
		}
	}
}
