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
		#region >>>>>>>>>> WPL <<<<<<<<<<<<
		public WPL WPLAccess { get; set; }
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

			//rami 09/09/2025
			WPLAccess = new WPL();
			WPLAccess.Country = accessProfileEntity.WP_Country ?? false;
			WPLAccess.CountryCreate = accessProfileEntity.WP_CountryCreate ?? false;
			WPLAccess.CountryDelete = accessProfileEntity.WP_CountryDelete ?? false;
			WPLAccess.CountryUpdate = accessProfileEntity.WP_CountryUpdate ?? false;

			WPLAccess.Departement = accessProfileEntity.WP_Departement ?? false;
			WPLAccess.DepartementCreate = accessProfileEntity.WP_DepartementCreate ?? false;
			WPLAccess.DepartementDelete = accessProfileEntity.WP_DepartementDelete ?? false;
			WPLAccess.DepartementUpdate = accessProfileEntity.WP_DepartementUpdate ?? false;

			WPLAccess.Hall = accessProfileEntity.WP_Hall ?? false;
			WPLAccess.HallCreate = accessProfileEntity.WP_HallCreate ?? false;
			WPLAccess.HallDelete = accessProfileEntity.WP_HallDelete ?? false;
			WPLAccess.HallUpdate = accessProfileEntity.WP_HallUpdate ?? false;

			WPLAccess.StandardOperation = accessProfileEntity.WP_StandardOperation ?? false;
			WPLAccess.StandardOperationCreate = accessProfileEntity.WP_StandardOperationCreate ?? false;
			WPLAccess.StandardOperationDelete = accessProfileEntity.WP_StandardOperationDelete ?? false;
			WPLAccess.StandardOperationUpdate = accessProfileEntity.WP_StandardOperationUpdate ?? false;

			WPLAccess.WorkArea = accessProfileEntity.WP_WorkArea ?? false;
			WPLAccess.WorkAreaCreate = accessProfileEntity.WP_WorkAreaCreate ?? false;
			WPLAccess.WorkAreaDelete = accessProfileEntity.WP_WorkAreaDelete ?? false;
			WPLAccess.WorkAreaUpdate = accessProfileEntity.WP_WorkAreaUpdate ?? false;

			WPLAccess.WorkPlan = accessProfileEntity.WP_WorkPlan ?? false;
			WPLAccess.WorkPlanCreate = accessProfileEntity.WP_WorkPlanCreate ?? false;
			WPLAccess.WorkPlanDelete = accessProfileEntity.WP_WorkPlanDelete ?? false;
			WPLAccess.WorkPlanUpdate = accessProfileEntity.WP_WorkPlanUpdate ?? false;

			WPLAccess.WorkPlanReporting = accessProfileEntity.WP_WorkPlanReporting ?? false;
			WPLAccess.WorkPlanReportingCreate = accessProfileEntity.WP_WorkPlanReportingCreate ?? false;
			WPLAccess.WorkPlanReportingDelete = accessProfileEntity.WP_WorkPlanReportingDelete ?? false;
			WPLAccess.WorkPlanReportingUpdate = accessProfileEntity.WP_WorkPlanReportingUpdate ?? false;

			WPLAccess.WorkStation = accessProfileEntity.WP_WorkStation ?? false;
			WPLAccess.WorkStationCreate = accessProfileEntity.WP_WorkStationCreate ?? false;
			WPLAccess.WorkStationDelete = accessProfileEntity.WP_WorkStationDelete ?? false;
			WPLAccess.WorkStationUpdate = accessProfileEntity.WP_WorkStationUpdate ?? false;

			WPLAccess.ModuleActivated =
				WPLAccess.Country || WPLAccess.CountryCreate || WPLAccess.CountryDelete || WPLAccess.CountryUpdate
				|| WPLAccess.Departement || WPLAccess.DepartementCreate || WPLAccess.DepartementDelete || WPLAccess.DepartementUpdate
				|| WPLAccess.Hall || WPLAccess.HallCreate || WPLAccess.HallDelete || WPLAccess.HallUpdate
				|| WPLAccess.StandardOperation || WPLAccess.StandardOperationCreate || WPLAccess.StandardOperationDelete || WPLAccess.StandardOperationUpdate
				|| WPLAccess.WorkArea || WPLAccess.WorkAreaCreate || WPLAccess.WorkAreaDelete || WPLAccess.WorkAreaUpdate
				|| WPLAccess.WorkPlan || WPLAccess.WorkPlanCreate || WPLAccess.WorkPlanDelete || WPLAccess.WorkPlanUpdate
				|| WPLAccess.WorkPlanReporting || WPLAccess.WorkPlanReportingCreate || WPLAccess.WorkPlanReportingDelete || WPLAccess.WorkPlanReportingUpdate
				|| WPLAccess.WorkStation || WPLAccess.WorkStationCreate || WPLAccess.WorkStationDelete || WPLAccess.WorkStationUpdate;

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
				//rami 10/09/2025
				WP_Country = WPLAccess?.Country ?? false,
				WP_CountryCreate = WPLAccess?.CountryCreate ?? false,
				WP_CountryDelete = WPLAccess?.CountryDelete ?? false,
				WP_CountryUpdate = WPLAccess?.CountryUpdate ?? false,

				WP_Departement = WPLAccess?.Departement ?? false,
				WP_DepartementCreate = WPLAccess?.DepartementCreate ?? false,
				WP_DepartementDelete = WPLAccess?.DepartementDelete ?? false,
				WP_DepartementUpdate = WPLAccess?.DepartementUpdate ?? false,

				WP_Hall = WPLAccess?.Hall ?? false,
				WP_HallCreate = WPLAccess?.HallCreate ?? false,
				WP_HallDelete = WPLAccess?.HallDelete ?? false,
				WP_HallUpdate = WPLAccess?.HallUpdate ?? false,

				WP_StandardOperation = WPLAccess?.StandardOperation ?? false,
				WP_StandardOperationCreate = WPLAccess?.StandardOperationCreate ?? false,
				WP_StandardOperationDelete = WPLAccess?.StandardOperationDelete ?? false,
				WP_StandardOperationUpdate = WPLAccess?.StandardOperationUpdate ?? false,

				WP_WorkArea = WPLAccess?.WorkArea ?? false,
				WP_WorkAreaCreate = WPLAccess?.WorkAreaCreate ?? false,
				WP_WorkAreaDelete = WPLAccess?.WorkAreaDelete ?? false,
				WP_WorkAreaUpdate = WPLAccess?.WorkAreaUpdate ?? false,

				WP_WorkPlan = WPLAccess?.WorkPlan ?? false,
				WP_WorkPlanCreate = WPLAccess?.WorkPlanCreate ?? false,
				WP_WorkPlanDelete = WPLAccess?.WorkPlanDelete ?? false,
				WP_WorkPlanUpdate = WPLAccess?.WorkPlanUpdate ?? false,

				WP_WorkPlanReporting = WPLAccess?.WorkPlanReporting ?? false,
				WP_WorkPlanReportingCreate = WPLAccess?.WorkPlanReportingCreate ?? false,
				WP_WorkPlanReportingDelete = WPLAccess?.WorkPlanReportingDelete ?? false,
				WP_WorkPlanReportingUpdate = WPLAccess?.WorkPlanReportingUpdate ?? false,

				WP_WorkStation = WPLAccess?.WorkStation ?? false,
				WP_WorkStationCreate = WPLAccess?.WorkStationCreate ?? false,
				WP_WorkStationDelete = WPLAccess?.WorkStationDelete ?? false,
				WP_WorkStationUpdate = WPLAccess?.WorkStationUpdate ?? false,

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

		public class WPL
		{
			public bool ModuleActivated { get; set; } = false;

			public bool Country { get; set; }
			public bool CountryCreate { get; set; }
			public bool CountryDelete { get; set; }
			public bool CountryUpdate { get; set; }

			public bool Departement { get; set; }
			public bool DepartementCreate { get; set; }
			public bool DepartementDelete { get; set; }
			public bool DepartementUpdate { get; set; }

			public bool Hall { get; set; }
			public bool HallCreate { get; set; }
			public bool HallDelete { get; set; }
			public bool HallUpdate { get; set; }

			public bool StandardOperation { get; set; }
			public bool StandardOperationCreate { get; set; }
			public bool StandardOperationDelete { get; set; }
			public bool StandardOperationUpdate { get; set; }

			public bool WorkArea { get; set; }
			public bool WorkAreaCreate { get; set; }
			public bool WorkAreaDelete { get; set; }
			public bool WorkAreaUpdate { get; set; }

			public bool WorkPlan { get; set; }
			public bool WorkPlanCreate { get; set; }
			public bool WorkPlanDelete { get; set; }
			public bool WorkPlanUpdate { get; set; }

			public bool WorkPlanReporting { get; set; }
			public bool WorkPlanReportingCreate { get; set; }
			public bool WorkPlanReportingDelete { get; set; }
			public bool WorkPlanReportingUpdate { get; set; }

			public bool WorkStation { get; set; }
			public bool WorkStationCreate { get; set; }
			public bool WorkStationDelete { get; set; }
			public bool WorkStationUpdate { get; set; }
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
