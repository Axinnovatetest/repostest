using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class PurchaseAccessModel
	{
		public bool ModuleActivated { get; set; } = false;

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
		public bool SuperAdministrator { get; set; } = false;
		public PurchaseAccessModel()
		{

		}
		public PurchaseAccessModel(List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;
			ModuleActivated = false;
			AllCustomers = false;
			EDI = false;
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
			SuperAdministrator = false;
			foreach(var accessItem in accessProfileEntities)
			{
				ModuleActivated = ModuleActivated || (accessItem?.ModuleActivated ?? false);
				AllCustomers = AllCustomers || (accessItem?.AllCustomers ?? false);
				EDI = EDI || (accessItem?.EDI ?? false);
				Access = Access || (accessItem?.Access ?? false);
				AccessUpdate = AccessUpdate || (accessItem?.AccessUpdate ?? false);
				Customer = Customer || (accessItem?.Customer ?? false);
				CustomerUpdate = CustomerUpdate || (accessItem?.CustomerUpdate ?? false);
				Order = Order || (accessItem?.Order ?? false);
				OrderError = OrderError || (accessItem?.OrderError ?? false);
				OrderErrorHistory = OrderErrorHistory || (accessItem?.OrderErrorHistory ?? false);
				OrderErrorValidate = OrderErrorValidate || (accessItem?.OrderErrorValidate ?? false);
				OrderHistory = OrderHistory || (accessItem?.OrderHistory ?? false);
				OrderUpdate = OrderUpdate || (accessItem?.OrderUpdate ?? false);
				OrderValidate = OrderValidate || (accessItem?.OrderValidate ?? false);
				SuperAdministrator = SuperAdministrator || (accessItem?.SuperAdministrator ?? false);
			}
		}
		public PurchaseAccessModel(List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;

			foreach(var accessItem in accessProfileEntities)
			{
				ModuleActivated = ModuleActivated || accessItem.ModuleActivated;
				AllCustomers = AllCustomers || accessItem.AllCustomers;
				EDI = EDI || accessItem.EDI;

				Access = Access || accessItem.Access;
				AccessUpdate = AccessUpdate || accessItem.AccessUpdate;
				Customer = Customer || accessItem.Customer;
				CustomerUpdate = CustomerUpdate || accessItem.CustomerUpdate;
				Order = Order || accessItem.Order;
				OrderError = OrderError || accessItem.OrderError;
				OrderErrorHistory = OrderErrorHistory || accessItem.OrderErrorHistory;
				OrderErrorValidate = OrderErrorValidate || accessItem.OrderErrorValidate;
				OrderHistory = OrderHistory || accessItem.OrderHistory;
				OrderUpdate = OrderUpdate || accessItem.OrderUpdate;
				OrderValidate = OrderValidate || accessItem.OrderValidate;
				SuperAdministrator = SuperAdministrator || accessItem.SuperAdministrator;
			}
		}

		internal Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
		{
			if(!this.ModuleActivated)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity()
				{
					Id = id,
					MainAccessProfileId = mainAccessProfileId,
					ModuleActivated = false,

					Access = true,  //this.Access,
					AccessUpdate = true,  //this.AccessUpdate,
					Customer = true,  //this.Customer,
					CustomerUpdate = true,  //this.CustomerUpdate,
					Order = true,  //this.Order,
					OrderError = true,  //this.OrderError,
					OrderErrorHistory = true,  //this.OrderErrorHistory,
					OrderErrorValidate = true,  //this.OrderErrorValidate,
					OrderHistory = true,  //this.OrderHistory,
					OrderUpdate = true,  //this.OrderUpdate,
					OrderValidate = true,  //this.OrderValidate,
					SuperAdministrator = false
				};
			}

			return new Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity()
			{
				Id = id,
				MainAccessProfileId = mainAccessProfileId,
				ModuleActivated = true,
				AllCustomers = this.AllCustomers,
				EDI = this.EDI,

				Access = true,  //this.Access,
				AccessUpdate = true,  //this.AccessUpdate,
				Customer = true,  //this.Customer,
				CustomerUpdate = true,  //this.CustomerUpdate,
				Order = true,  //this.Order,
				OrderError = true,  //this.OrderError,
				OrderErrorHistory = true,  //this.OrderErrorHistory,
				OrderErrorValidate = true,  //this.OrderErrorValidate,
				OrderHistory = true,  //this.OrderHistory,
				OrderUpdate = true,  //this.OrderUpdate,
				OrderValidate = true,  //this.OrderValidate,
				SuperAdministrator = false
			};
		}
	}
	public class PurchaseAccessMinimalModel
	{
		public bool ModuleActivated { get; set; }

		public bool EDI { get; set; }

		public bool Access { get; set; }

		public bool Order { get; set; }

		public bool OrderHistory { get; set; }

		public bool OrderError { get; set; }
		public bool OrderErrorHistory { get; set; }
		public bool SuperAdministrator { get; set; } = false;

		public PurchaseAccessMinimalModel()
		{

		}
		public PurchaseAccessMinimalModel(PurchaseAccessModel model)
		{
			ModuleActivated = model.ModuleActivated;

			EDI = model.EDI;

			Access = model.Access;

			Order = model.Order;

			OrderHistory = model.OrderHistory;

			OrderError = model.OrderError;
			OrderErrorHistory = model.OrderErrorHistory;
		}

		public Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity ToDbEntity(int mainId, int id)
		{
			return new Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity
			{
				Id = id,
				MainAccessProfileId = mainId,
				ModuleActivated = this.ModuleActivated,
				EDI = this.EDI,
				Access = this.Access,
				Order = this.Order,
				OrderHistory = this.OrderHistory,
				OrderError = this.OrderError,
				OrderErrorHistory = this.OrderErrorHistory,
			};
		}


	}
}
