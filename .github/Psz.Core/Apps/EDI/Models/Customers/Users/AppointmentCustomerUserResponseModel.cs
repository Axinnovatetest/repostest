using Infrastructure.Data.Entities.Joins.CTS;

namespace Psz.Core.Apps.EDI.Models.Customers.Users
{
	public class AppointmentCustomerUserResponseModel
	{
		public int Id { get; set; }
		public int CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public string CustomerName2 { get; set; }
		public string CustomerName3 { get; set; }
		public string CustomerAddress { get; set; }
		public string CustomerType { get; set; }
		public string CustomerContact { get; set; }
		public string EmployeeName { get; set; }
		public int EmployeeId { get; set; }
		public int AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }
		public AppointmentCustomerUserResponseModel() { }
		public AppointmentCustomerUserResponseModel(AppointmentCustomerUserEntity entity)
		{
			if(entity == null)
			{
				return;
			}
			Id = entity.Id;
			CustomerNumber = entity.CustomerNumber;
			CustomerName = entity.CustomerName;
			CustomerName2 = entity.CustomerName2;
			CustomerName3 = entity.CustomerName3;
			CustomerAddress = entity.CustomerAddress;
			CustomerType = entity.CustomerType;
			CustomerContact = entity.CustomerContact;
			EmployeeName = entity.EmployeeName;
			EmployeeId = entity.EmployeeId;
			AccessProfileId = entity.AccessProfileId;
			AccessProfileName = entity.AccessProfileName;
		}
	}
}
