namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class SearchRequestModel
	{
		public int CountryId { get; set; }
		public int Year { get; set; }
		public int WeekNumberFrom { get; set; }
		public int WeekNumberUntil { get; set; }

		public int? OperationId { get; set; }
		public int? HallId { get; set; }
		public int? DepartementId { get; set; }
		public int? WorkAreaId { get; set; }
		public int? WorkStationId { get; set; }
		public bool Focus { get; set; } = true;
	}
}
