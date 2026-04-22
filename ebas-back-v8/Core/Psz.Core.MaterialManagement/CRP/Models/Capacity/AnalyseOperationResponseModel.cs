namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class AnalyseOperationResponseModel
	{
		public List<Item> Capacities { get; set; } = new List<Item>();
		public List<Item> RequestedCapacities { get; set; } = new List<Item>();

		public class Item
		{
			public int OperationId { get; set; }
			public string OperationName { get; set; }
			public decimal Attendance { get; set; }
			public decimal PlanCapacity { get; set; }
			public decimal RequiredEmployees { get; set; }
		}
	}
}
