namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlan
{
	public class DeleteItemModel
	{
		public int Year { get; set; }
		public int CountryId { get; set; }

		public int WeekFrom { get; set; }
		public int WeekUntil { get; set; }

		public int OperationId { get; set; }
		public int HallId { get; set; }
		public int DepartementId { get; set; }
		public int WorkAreaId { get; set; }
		public int? WorkStationId { get; set; }
	}
}
