namespace Psz.Core.Apps.Settings.Models.AccessProfiles
{
	public class PurchaseAccessModel
	{
		public bool ModuleActivated { get; set; }

		public bool EDI { get; set; }
		public bool AllCustomers { get; set; }

		public bool Access { get; set; }
		public bool AccessUpdate { get; set; }

		public bool Customer { get; set; }
		public bool CustomerUpdate { get; set; }

		public bool Order { get; set; }
		public bool OrderUpdate { get; set; }
		public bool OrderValidate { get; set; }

		public bool OrderHistory { get; set; }

		public bool OrderError { get; set; }
		public bool OrderErrorHistory { get; set; }
		public bool OrderErrorValidate { get; set; }

		public bool SuperAdministrator { get; set; }

		internal Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
		{
			if(!this.ModuleActivated)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity()
				{
					Id = id,
					MainAccessProfileId = mainAccessProfileId,
					ModuleActivated = false,
				};
			}

			return new Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity()
			{
				Id = id,
				MainAccessProfileId = mainAccessProfileId,
				ModuleActivated = true,

				Access = this.Access,
				AccessUpdate = this.AccessUpdate,
				Customer = this.Customer,
				CustomerUpdate = this.CustomerUpdate,
				Order = this.Order,
				OrderError = this.OrderError,
				OrderErrorHistory = this.OrderErrorHistory,
				OrderErrorValidate = this.OrderErrorValidate,
				OrderHistory = this.OrderHistory,
				OrderUpdate = this.OrderUpdate,
				OrderValidate = this.OrderValidate,

				AllCustomers = this.AllCustomers,
				EDI = this.EDI
			};
		}

		internal void DenyAll()
		{
			ModuleActivated = false;

			Access = false;
			AccessUpdate = false;
			Customer = false;
			CustomerUpdate = false;
			Order = false;
			OrderError = false;
			OrderErrorHistory = false;
			OrderErrorValidate = false;
			OrderHistory = false;
			OrderUpdate = false;
			OrderValidate = false;

			AllCustomers = false;
			EDI = false;
		}
	}
}
