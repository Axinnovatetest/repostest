namespace Psz.Core.Apps.WorkPlan.Models.WorkSchedule
{
	public class GetSimulationResponseModel
	{
		public int WorkScheduleId { get; set; }
		public decimal FASize { get; set; }
		public double TotalOperationValueAdding { get; set; }
		public decimal TotalOperationTime { get; set; }
		public decimal Ratio { get; set; }
	}
}
