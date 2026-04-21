namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class AnalyseWorkAreaResponseModel
	{
		public List<Item> Capacities { get; set; } = new List<Item>();
		public List<Item> RequestedCapacities { get; set; } = new List<Item>();

		public class Item
		{
			public int WorkAreaId { get; set; }
			public string WorkAreaName { get; set; }
			public decimal Attendance { get; set; }
			public decimal PlanCapacity { get; set; }
			public decimal RequiredEmployees { get; set; }
		}
	}
}
