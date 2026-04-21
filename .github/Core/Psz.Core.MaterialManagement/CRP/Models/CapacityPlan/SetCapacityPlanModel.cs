namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlan
{
	public class SetCapacityPlanModel
	{
		public int CountryId { get; set; }
		public int? HallId { get; set; }
		public int Year { get; set; }
		public int FirstWeekNumber { get; set; }
		public int LastWeekNumber { get; set; }

		public List<CalculateItemModel> Items { get; set; } = new List<CalculateItemModel>();
	}
}
