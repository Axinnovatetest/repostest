using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;

namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class SingleCapacityUpdateModel: CalculateItemModel
	{
		public int CapacityId { get; set; }
		public int WeekNumber { get; set; }
		public int Year { get; set; }
		public int CountryId { get; set; }
	}
}
