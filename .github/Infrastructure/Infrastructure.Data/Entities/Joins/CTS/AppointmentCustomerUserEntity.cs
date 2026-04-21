namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class AppointmentCustomerUserEntity
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

		public AppointmentCustomerUserEntity() { }
		public AppointmentCustomerUserEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Id"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["CustomerNumber"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			CustomerName2 = (dataRow["CustomerName2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName2"]);
			CustomerName3 = (dataRow["CustomerName3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName3"]);
			CustomerAddress = (dataRow["CustomerAddress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerAddress"]);
			CustomerType = (dataRow["CustomerType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerType"]);
			CustomerContact = (dataRow["CustomerContact"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerContact"]);
			EmployeeName = (dataRow["EmployeeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EmployeeName"]);
			EmployeeId = (dataRow["EmployeeId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["EmployeeId"]);
			AccessProfileId = (dataRow["AccessProfileId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["AccessProfileId"]);
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
		}
	}
}
