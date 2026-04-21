using System.ComponentModel;

namespace Psz.Core.MaterialManagement.CRP.Enums
{
	public class Main
	{
		public enum CapacityType
		{
			Capacity = 0,
			CapacityPlan = 1
		}
	}
	public class CapacityPlan
	{
		public enum ValidationStatuses
		{
			[Description("Validated")]
			Validated = 1,
			[Description("Rejected")]
			Rejected = 2,
			[Description("Unvalidated")]
			Unvalidated = 3,
			[Description("In-validation")]
			Invalidation = 4
		}
		public enum ValidationLevels
		{
			[Description("Request")]
			Request = 0,
			[Description("First")]
			First = 1,
			[Description("Second")]
			Second = 2,
			[Description("Third")]
			Third = 3
		}
	}
}
