using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class AccessProfileEntity2
	{
		public bool? Access { get; set; }
		public string AccessProfileName { get; set; }
		public bool? AccessUpdate { get; set; }
		public bool? AllCustomers { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public bool? Customer { get; set; }
		public bool? CustomerUpdate { get; set; }
		public bool? EDI { get; set; }
		public int Id { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? Order { get; set; }
		public bool? OrderError { get; set; }
		public bool? OrderErrorHistory { get; set; }
		public bool? OrderErrorValidate { get; set; }
		public bool? OrderHistory { get; set; }
		public bool? OrderUpdate { get; set; }
		public bool? OrderValidate { get; set; }
		public bool? SuperAdministrator { get; set; }

		public AccessProfileEntity2() { }

		public AccessProfileEntity2(DataRow dataRow)
		{
			Access = (dataRow["Access"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Access"]);
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
			AccessUpdate = (dataRow["AccessUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AccessUpdate"]);
			AllCustomers = (dataRow["AllCustomers"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AllCustomers"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Customer = (dataRow["Customer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Customer"]);
			CustomerUpdate = (dataRow["CustomerUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerUpdate"]);
			EDI = (dataRow["EDI"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ModuleActivated = (dataRow["ModuleActivated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ModuleActivated"]);
			Order = (dataRow["Order"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Order"]);
			OrderError = (dataRow["OrderError"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OrderError"]);
			OrderErrorHistory = (dataRow["OrderErrorHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OrderErrorHistory"]);
			OrderErrorValidate = (dataRow["OrderErrorValidate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OrderErrorValidate"]);
			OrderHistory = (dataRow["OrderHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OrderHistory"]);
			OrderUpdate = (dataRow["OrderUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OrderUpdate"]);
			OrderValidate = (dataRow["OrderValidate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OrderValidate"]);
			SuperAdministrator = (dataRow["SuperAdministrator"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SuperAdministrator"]);
		}
	}
}

