namespace Psz.Core.Apps.EDI.Models.Customers.Users
{
	public class AppointmentCustomerUserRequestModel
	{
		public string CustomerName { get; set; }
		public string EmployeeName { get; set; }
		public bool? IsAssignedEmployee { get; set; }
	}
}
