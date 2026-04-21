namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderLeasingRequestModel
	{
		public int Year { get; set; }
		public int? CompanyId { get; set; }
		public int? DepartmentId { get; set; }
		public int? EmployeeId { get; set; }
	}
}
