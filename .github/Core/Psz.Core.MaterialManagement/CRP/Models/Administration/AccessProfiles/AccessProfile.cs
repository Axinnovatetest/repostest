namespace Psz.Core.MaterialManagement.Models.Administration.AccessProfiles
{
	public class AccessProfileAddRequestModel
	{
		public int Id { get; set; }
		public string AccessProfileName { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? CreationTime { get; set; }
		public bool ModuleActivated { get; set; }
		public bool IsDefault { get; set; }
		public Administration AdministrationAccess { get; set; }


		// - CRP
		#region >>>>>>>>>> CRP <<<<<<<<<<<<
		public CRP CRPAccess { get; set; }
		#endregion

		// - Purchasing
		#region >>>>>>>>>> Purchasing <<<<<<<<<<<<
		public PurchasingModule Purchasing { get; set; }
		#endregion

		public AccessProfileAddRequestModel(Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity accessProfileEntity)
		{
			if(accessProfileEntity == null)
				return;

			// -
			Id = accessProfileEntity.Id;
			AccessProfileName = accessProfileEntity.AccessProfileName;
			AdministrationAccess = new Administration();
			AdministrationAccess.ModuleActivated = accessProfileEntity.Administration;
			IsDefault = accessProfileEntity.IsDefault;


			// - CRP
			CRPAccess = new CRP();
			CRPAccess.AllRessourcesAuthorized = accessProfileEntity.CRP_AllRessourcesAuthorized;
			CRPAccess.Capacity = accessProfileEntity.CRP_Capacity;
			CRPAccess.CapacityEdit = accessProfileEntity.CRP_Capacity && accessProfileEntity.CRP_CapacityEdit;
			CRPAccess.CapacityPlan = accessProfileEntity.CRP_CapacityPlan;
			CRPAccess.CapacityPlanEdit = accessProfileEntity.CRP_CapacityPlan && accessProfileEntity.CRP_CapacityPlanEdit;
			CRPAccess.Holiday = accessProfileEntity.CRP_Holiday;
			CRPAccess.HolidayEdit = accessProfileEntity.CRP_Holiday && accessProfileEntity.CRP_HolidayEdit;
			CRPAccess.Validation = accessProfileEntity.CRP_Validation;
			CRPAccess.ValidationEdit = accessProfileEntity.CRP_Validation && accessProfileEntity.CRP_ValidationEdit;
			CRPAccess.Configuration = accessProfileEntity.CRP_Configuration;
			CRPAccess.ConfigurationEdit = accessProfileEntity.CRP_Configuration && accessProfileEntity.CRP_ConfigurationEdit;
			CRPAccess.RessourceAuthorizationEdit = accessProfileEntity.CRP_RessourceAuthorizationEdit;

			CRPAccess.ModuleActivated = CRPAccess.AllRessourcesAuthorized || CRPAccess.Capacity || CRPAccess.CapacityPlan
			|| CRPAccess.Holiday || CRPAccess.Validation || CRPAccess.Configuration || CRPAccess.RessourceAuthorizationEdit;

			// - Purchasing
			Purchasing = new PurchasingModule();
			Purchasing.Dashboard = accessProfileEntity.ORD_Dashboard ?? false;
			Purchasing.Order = accessProfileEntity.ORD_Order ?? false;
			Purchasing.OrderAdd = accessProfileEntity.ORD_OrderAdd ?? false;
			Purchasing.OrderEdit = accessProfileEntity.ORD_OrderEdit ?? false;
			Purchasing.OrderDelete = accessProfileEntity.ORD_OrderDelete ?? false;
			Purchasing.OrderUnValidate = accessProfileEntity.ORD_OrderUnValidate ?? false;
			Purchasing.OrderValidate = accessProfileEntity.ORD_OrderValidate ?? false;
			Purchasing.OrderQuickPO = accessProfileEntity.ORD_OrderQuickPO ?? false;
			Purchasing.STAT_Dashboard = accessProfileEntity.STAT_Dashboard ?? false;
			Purchasing.DISPO_Dashboard = accessProfileEntity.DISPO_Dashboard ?? false;
			Purchasing.WE = accessProfileEntity.WE ?? false;
			Purchasing.WE_Create = accessProfileEntity.WE_Create ?? false;
			//souilmi 26-07-2023
			Purchasing.Rahmen = accessProfileEntity.Rahmen ?? false;
			Purchasing.RahmenDelete = accessProfileEntity.RahmenDelete ?? false;
			Purchasing.RahmenHistory = accessProfileEntity.RahmenHistory ?? false;
			Purchasing.RahmenDocumentFlow = accessProfileEntity.RahmenDocumentFlow ?? false;
			Purchasing.RahmenEditHeader = accessProfileEntity.RahmenEditHeader ?? false;
			Purchasing.RahmenEditPositions = accessProfileEntity.RahmenEditPositions ?? false;
			Purchasing.RahmenAddPositions = accessProfileEntity.RahmenAddPositions ?? false;
			Purchasing.RahmenDeletePositions = accessProfileEntity.RahmenDeletePositions ?? false;
			Purchasing.RahmenValdation = accessProfileEntity.RahmenValdation ?? false;
			Purchasing.RahmenCancelation = accessProfileEntity.RahmenCancelation ?? false;
			Purchasing.RahmenClosure = accessProfileEntity.RahmenClosure ?? false;
			Purchasing.RahmenAdd = accessProfileEntity.RahmenAdd ?? false;
			Purchasing.ProjectPurchaseDeleteOrder = accessProfileEntity.ORD_ProjectPurchaseDeleteOrder ?? false;
			Purchasing.ProjectPurchaseSetOrder = accessProfileEntity.ORD_ProjectPurchaseSetOrder ?? false;

			Purchasing.ModuleActivated = Purchasing.Dashboard || Purchasing.Order || Purchasing.OrderAdd
				|| Purchasing.OrderEdit || Purchasing.OrderDelete || Purchasing.OrderValidate || Purchasing.OrderUnValidate
				|| Purchasing.RahmenAdd || Purchasing.RahmenDelete || Purchasing.RahmenHistory || Purchasing.RahmenDocumentFlow || Purchasing.RahmenEditHeader
				|| Purchasing.RahmenEditPositions || Purchasing.RahmenAddPositions || Purchasing.RahmenDeletePositions || Purchasing.RahmenValdation || Purchasing.RahmenCancelation
				|| Purchasing.RahmenClosure || Purchasing.Rahmen || Purchasing.WE || Purchasing.WE_Create || Purchasing.ProjectPurchaseSetOrder || Purchasing.ProjectPurchaseDeleteOrder;

			ModuleActivated = Purchasing.ModuleActivated;// || CRPAccess.ModuleActivated;
		}
		public Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity
			{
				Id = Id,
				AccessProfileName = AccessProfileName,
				IsDefault = IsDefault,
				Administration = AdministrationAccess?.ModuleActivated ?? false,
				CRP_AllRessourcesAuthorized = CRPAccess?.AllRessourcesAuthorized ?? false,
				CRP_Capacity = CRPAccess?.Capacity ?? false,
				CRP_CapacityEdit = CRPAccess?.CapacityEdit ?? false,
				CRP_CapacityPlan = CRPAccess?.CapacityPlan ?? false,
				CRP_CapacityPlanEdit = CRPAccess?.CapacityPlanEdit ?? false,
				CRP_Holiday = CRPAccess?.Holiday ?? false,
				CRP_HolidayEdit = CRPAccess?.HolidayEdit ?? false,
				CRP_RessourceAuthorizationEdit = CRPAccess?.RessourceAuthorizationEdit ?? false,
				CRP_Configuration = CRPAccess?.Configuration ?? false,
				CRP_ConfigurationEdit = CRPAccess?.ConfigurationEdit ?? false,
				CRP_Validation = CRPAccess?.Validation ?? false,
				CRP_ValidationEdit = CRPAccess?.ValidationEdit ?? false,
				ORD_Dashboard = Purchasing?.Dashboard ?? false,
				ORD_Order = Purchasing?.Order ?? false,
				ORD_OrderAdd = Purchasing?.OrderAdd ?? false,
				ORD_OrderEdit = Purchasing?.OrderEdit ?? false,
				ORD_OrderDelete = Purchasing?.OrderDelete ?? false,
				ORD_OrderUnValidate = Purchasing?.OrderUnValidate ?? false,
				ORD_OrderValidate = Purchasing?.OrderValidate ?? false,
				STAT_Dashboard = Purchasing?.STAT_Dashboard ?? false,
				DISPO_Dashboard = Purchasing?.DISPO_Dashboard ?? false,
				ORD_OrderQuickPO = Purchasing?.OrderQuickPO ?? false,
				WE = Purchasing?.WE ?? false,
				WE_Create = Purchasing?.WE_Create ?? false,
				//souilmi 26-07-2023
				RahmenAdd = Purchasing?.RahmenAdd ?? false,
				RahmenDelete = Purchasing?.RahmenDelete ?? false,
				RahmenHistory = Purchasing?.RahmenHistory ?? false,
				RahmenDocumentFlow = Purchasing?.RahmenDocumentFlow ?? false,
				RahmenEditHeader = Purchasing?.RahmenEditHeader ?? false,
				RahmenEditPositions = Purchasing?.RahmenEditPositions ?? false,
				RahmenAddPositions = Purchasing?.RahmenAddPositions ?? false,
				RahmenDeletePositions = Purchasing?.RahmenDeletePositions ?? false,
				RahmenValdation = Purchasing?.RahmenValdation ?? false,
				RahmenCancelation = Purchasing?.RahmenCancelation ?? false,
				RahmenClosure = Purchasing?.RahmenClosure ?? false,
				Rahmen = Purchasing?.Rahmen ?? false,
				ORD_ProjectPurchaseDeleteOrder = Purchasing?.ProjectPurchaseDeleteOrder ?? false,
				ORD_ProjectPurchaseSetOrder = Purchasing?.ProjectPurchaseSetOrder ?? false,
			};
		}

		public class CRP
		{
			public bool ModuleActivated { get; set; } = false;
			public bool AllRessourcesAuthorized { get; set; }
			public bool Capacity { get; set; }
			public bool CapacityEdit { get; set; }
			public bool CapacityPlan { get; set; }
			public bool CapacityPlanEdit { get; set; }
			public bool Holiday { get; set; }
			public bool HolidayEdit { get; set; }
			public bool RessourceAuthorizationEdit { get; set; }
			public bool Validation { get; set; }
			public bool ValidationEdit { get; set; }
			public bool Configuration { get; set; }
			public bool ConfigurationEdit { get; set; }
		}
		public class PurchasingModule
		{
			public bool Dashboard { get; set; }
			public bool ModuleActivated { get; set; }
			public bool Order { get; set; }
			public bool OrderEdit { get; set; }
			public bool OrderDelete { get; set; }
			public bool OrderAdd { get; set; }
			public bool OrderValidate { get; set; }
			public bool OrderUnValidate { get; set; }
			public bool STAT_Dashboard { get; set; }
			public bool DISPO_Dashboard { get; set; }
			public bool OrderQuickPO { get; set; }
			public bool WE { get; set; }
			public bool WE_Create { get; set; }
			//souilmi 26-07-2023
			public bool Rahmen { get; set; }
			public bool RahmenClosure { get; set; }
			public bool RahmenCancelation { get; set; }
			public bool RahmenValdation { get; set; }
			public bool RahmenDeletePositions { get; set; }
			public bool RahmenAddPositions { get; set; }
			public bool RahmenEditPositions { get; set; }
			public bool RahmenEditHeader { get; set; }
			public bool RahmenDocumentFlow { get; set; }
			public bool RahmenHistory { get; set; }
			public bool RahmenDelete { get; set; }
			public bool RahmenAdd { get; set; }
			// - 2024-04-25
			public bool ProjectPurchaseSetOrder { get; set; }
			public bool ProjectPurchaseDeleteOrder { get; set; }
		}
		public class Administration
		{
			public bool ModuleActivated { get; set; }
			public bool Order { get; set; }
			public bool OrderEdit { get; set; }
			public bool OrderDelete { get; set; }
			public bool OrderAdd { get; set; }
			public bool OrderValidate { get; set; }
			public bool OrderUnValidate { get; set; }
			public bool OrderQuickPO { get; set; }
			public bool DISPO_Dashboard { get; set; }
			public bool STAT_Dashboard { get; set; }

		}
	}
}
