using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class AccessProfileEntity
	{
		public bool Access { get; set; }
		public bool AccessUpdate { get; set; }
		public bool Customer { get; set; }
		public bool CustomerUpdate { get; set; }
		public int Id { get; set; }
		public int MainAccessProfileId { get; set; }
		public bool ModuleActivated { get; set; }
		public bool Order { get; set; }
		public bool OrderError { get; set; }
		public bool OrderErrorHistory { get; set; }
		public bool OrderErrorValidate { get; set; }
		public bool OrderHistory { get; set; }
		public bool OrderUpdate { get; set; }
		public bool OrderValidate { get; set; }

		public bool AllCustomers { get; set; }
		public bool EDI { get; set; }
		public bool SuperAdministrator { get; set; }

		public AccessProfileEntity() { }
		public AccessProfileEntity(DataRow dataRow)
		{
			Access = Convert.ToBoolean(dataRow["Access"]);
			AccessUpdate = Convert.ToBoolean(dataRow["AccessUpdate"]);
			Customer = Convert.ToBoolean(dataRow["Customer"]);
			CustomerUpdate = Convert.ToBoolean(dataRow["CustomerUpdate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			MainAccessProfileId = Convert.ToInt32(dataRow["MainAccessProfileId"]);
			ModuleActivated = Convert.ToBoolean(dataRow["ModuleActivated"]);
			Order = Convert.ToBoolean(dataRow["Order"]);
			OrderError = Convert.ToBoolean(dataRow["OrderError"]);
			OrderErrorHistory = Convert.ToBoolean(dataRow["OrderErrorHistory"]);
			OrderErrorValidate = Convert.ToBoolean(dataRow["OrderErrorValidate"]);
			OrderHistory = Convert.ToBoolean(dataRow["OrderHistory"]);
			OrderUpdate = Convert.ToBoolean(dataRow["OrderUpdate"]);
			OrderValidate = Convert.ToBoolean(dataRow["OrderValidate"]);

			AllCustomers = Convert.ToBoolean(dataRow["AllCustomers"]);
			EDI = Convert.ToBoolean(dataRow["EDI"]);
			SuperAdministrator = Convert.ToBoolean(dataRow["SuperAdministrator"]);
		}
	}
}

