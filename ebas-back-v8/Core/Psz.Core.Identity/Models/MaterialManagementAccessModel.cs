using Newtonsoft.Json;
using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class MaterialManagementAccessModel
	{

		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated;

		[JsonProperty("Purchasing")]
		public PurchasingAccessModel Purchasing;

		[JsonProperty("InventoryManagement")]
		public InventoryManagement InventoryManagement;

		[JsonProperty("MaterialPlanning")]
		public MaterialPlanning MaterialPlanning;

		[JsonProperty("InvoiceVerification")]
		public InvoiceVerification InvoiceVerification;

		[JsonProperty("CRPAccess")]
		public MaterialRequirementPlanning CRPAccess;

		[JsonProperty("WarehouseManagement")]
		public WarehouseManagement WarehouseManagement;

		[JsonProperty("VendorValuation")]
		public VendorValuation VendorValuation;

		[JsonProperty("Administration")]
		public Administration Administration;
		//rami 09/09/2025
		[JsonProperty("WPLAccess")]
		public MTM_WorkPlanAccessModel WPLAccess;
		public MaterialManagementAccessModel()
		{
			InventoryManagement = new InventoryManagement();
			MaterialPlanning = new MaterialPlanning();
			InvoiceVerification = new InvoiceVerification();
			CRPAccess = new MaterialRequirementPlanning();
			WPLAccess = new MTM_WorkPlanAccessModel();
			WarehouseManagement = new WarehouseManagement();
			VendorValuation = new VendorValuation();
			Administration = new Administration();
			Purchasing = new PurchasingAccessModel();
			// - 
			ModuleActivated = false;
		}
		public MaterialManagementAccessModel(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> profileEntities)
		{
			InventoryManagement = new InventoryManagement();
			MaterialPlanning = new MaterialPlanning();
			InvoiceVerification = new InvoiceVerification();
			CRPAccess = new MaterialRequirementPlanning();
			WPLAccess = new MTM_WorkPlanAccessModel();
			WarehouseManagement = new WarehouseManagement();
			VendorValuation = new VendorValuation();
			Administration = new Administration();
			Purchasing = new PurchasingAccessModel();

			ModuleActivated = false;
			// - 
			if(profileEntities == null || profileEntities.Count <= 0)
				return;

			// -
			InventoryManagement = new InventoryManagement();
			MaterialPlanning = new MaterialPlanning();
			InvoiceVerification = new InvoiceVerification();
			CRPAccess = new MaterialRequirementPlanning(profileEntities);
			WPLAccess = new MTM_WorkPlanAccessModel(profileEntities);
			WarehouseManagement = new WarehouseManagement();
			VendorValuation = new VendorValuation();
			Administration = new Administration(profileEntities);
			Purchasing = new PurchasingAccessModel(profileEntities);

			// - 
			//ModuleActivated = profileEntities.Exists(x=> x.ModuleActivated == true);	
			ModuleActivated = true;
		}
	}
	public class MaterialManagementAccessMinimalModel
	{

		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated;

		[JsonProperty("Purchasing")]
		public bool Purchasing;

		[JsonProperty("InventoryManagement")]
		public bool InventoryManagement;

		[JsonProperty("MaterialPlanning")]
		public bool MaterialPlanning;

		[JsonProperty("InvoiceVerification")]
		public bool InvoiceVerification;

		[JsonProperty("MaterialRequirementPlanning")]
		public bool MaterialRequirementPlanning;

		[JsonProperty("WarehouseManagement")]
		public bool WarehouseManagement;

		[JsonProperty("VendorValuation")]
		public bool VendorValuation;

		[JsonProperty("Administration")]
		public bool Administration;

		[JsonProperty("WPLAccess")]
		public bool MTM_WorkPlanAccessModel;
		public MaterialManagementAccessMinimalModel()
		{

		}
		public MaterialManagementAccessMinimalModel(MaterialManagementAccessModel model)
		{
			ModuleActivated = model.ModuleActivated;
		}
	}
	public class Purchasing
	{
		public bool ModuleActivated { get; set; }
		public bool Dashboard { get; set; }
		public Purchasing()
		{

		}
		public Purchasing(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> profileEntities)
		{
			ModuleActivated = false;
			Dashboard = false;
			foreach(var item in profileEntities)
			{
				Dashboard = Dashboard || (item.ORD_Dashboard ?? false);
				ModuleActivated = ModuleActivated || Dashboard;
			}
		}
	}
}