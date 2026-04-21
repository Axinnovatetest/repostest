namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class AnalyseWorkStationResponseModel
	{
		public List<Item> Capacities { get; set; } = new List<Item>();
		public List<Item> RequestedCapacities { get; set; } = new List<Item>();

		public class Item
		{
			public int WorkStationId { get; set; }
			public string WorkStationName { get; set; }
			public decimal Attendance { get; set; }
			public decimal PlanCapacity { get; set; }
			public decimal RequiredEmployees { get; set; }
		}
	}
}
