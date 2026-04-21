namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlan
{
	public class GetPlanItemRequestModel: WorkLocationBaseModel
	{
		public int Year { get; set; }
		public int OperationId { get; set; }
		public int HallId { get; set; }
		public int DepartementId { get; set; }
		public int? WorkAreaId { get; set; }
		public int? WorkStationId { get; set; }

		public int WeekFrom { get; set; }
		public int WeekTo { get; set; }
	}
}
