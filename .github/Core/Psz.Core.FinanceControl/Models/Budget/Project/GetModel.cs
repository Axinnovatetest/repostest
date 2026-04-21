namespace Psz.Core.FinanceControl.Models.Budget.Project
{
	public class GetRequestModel
	{
		public int UserId { get; set; }
		public bool? Approved { get; set; }
		public bool? Closed { get; set; }
		public bool? Archived { get; set; }
		public int? Status { get; set; } // Active, Pending, ...
		public int? Type { get; set; } // Internal or External
		public int? CreationYear { get; set; }
		public int MinOrderCount { get; set; } = 0;
		public int? CompanyId { get; set; }
		public int? DepartmentId { get; set; }
	}
}
