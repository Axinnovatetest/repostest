using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class AccessProfileEntity
	{
		public string AccessProfileName { get; set; }
		public bool Administration { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public bool CRP_AllRessourcesAuthorized { get; set; }
		public bool CRP_Capacity { get; set; }
		public bool CRP_CapacityEdit { get; set; }
		public bool CRP_CapacityPlan { get; set; }
		public bool CRP_CapacityPlanEdit { get; set; }
		public bool CRP_Configuration { get; set; }
		public bool CRP_ConfigurationEdit { get; set; }
		public bool CRP_Holiday { get; set; }
		public bool CRP_HolidayEdit { get; set; }
		public bool CRP_RessourceAuthorizationEdit { get; set; }
		public bool CRP_Validation { get; set; }
		public bool CRP_ValidationEdit { get; set; }
		public bool? DISPO_Dashboard { get; set; }
		public int Id { get; set; }
		public bool IsDefault { get; set; }
		public bool ModuleActivated { get; set; }
		public bool? ORD_Dashboard { get; set; }
		public bool? ORD_Order { get; set; }
		public bool? ORD_OrderAdd { get; set; }
		public bool? ORD_OrderDelete { get; set; }
		public bool? ORD_OrderEdit { get; set; }
		public bool? ORD_OrderQuickPO { get; set; }
		public bool? ORD_OrderUnValidate { get; set; }
		public bool? ORD_OrderValidate { get; set; }
		public bool? Rahmen { get; set; }
		public bool? RahmenAdd { get; set; }
		public bool? RahmenAddPositions { get; set; }
		public bool? RahmenCancelation { get; set; }
		public bool? RahmenClosure { get; set; }
		public bool? RahmenDelete { get; set; }
		public bool? RahmenDeletePositions { get; set; }
		public bool? RahmenDocumentFlow { get; set; }
		public bool? RahmenEditHeader { get; set; }
		public bool? RahmenEditPositions { get; set; }
		public bool? RahmenHistory { get; set; }
		public bool? RahmenValdation { get; set; }
		public bool? STAT_Dashboard { get; set; }
		public bool? WE { get; set; }
		public bool? WE_Create { get; set; }
		// - 2024-04-25
		public bool? ORD_ProjectPurchaseSetOrder { get; set; }
		public bool? ORD_ProjectPurchaseDeleteOrder { get; set; }

		//rami 09/09/2025 WorkPlan Access
		public bool? WP_Country { get; set; }
		public bool? WP_CountryCreate { get; set; }
		public bool? WP_CountryDelete { get; set; }
		public bool? WP_CountryUpdate { get; set; }

		public bool? WP_Departement { get; set; }
		public bool? WP_DepartementCreate { get; set; }
		public bool? WP_DepartementDelete { get; set; }
		public bool? WP_DepartementUpdate { get; set; }

		public bool? WP_Hall { get; set; }
		public bool? WP_HallCreate { get; set; }
		public bool? WP_HallDelete { get; set; }
		public bool? WP_HallUpdate { get; set; }

		public bool? WP_StandardOperation { get; set; }
		public bool? WP_StandardOperationCreate { get; set; }
		public bool? WP_StandardOperationDelete { get; set; }
		public bool? WP_StandardOperationUpdate { get; set; }

		public bool? WP_WorkArea { get; set; }
		public bool? WP_WorkAreaCreate { get; set; }
		public bool? WP_WorkAreaDelete { get; set; }
		public bool? WP_WorkAreaUpdate { get; set; }

		public bool? WP_WorkPlan { get; set; }
		public bool? WP_WorkPlanCreate { get; set; }
		public bool? WP_WorkPlanDelete { get; set; }
		public bool? WP_WorkPlanUpdate { get; set; }

		public bool? WP_WorkPlanReporting { get; set; }
		public bool? WP_WorkPlanReportingCreate { get; set; }
		public bool? WP_WorkPlanReportingDelete { get; set; }
		public bool? WP_WorkPlanReportingUpdate { get; set; }

		public bool? WP_WorkStation { get; set; }
		public bool? WP_WorkStationCreate { get; set; }
		public bool? WP_WorkStationDelete { get; set; }
		public bool? WP_WorkStationUpdate { get; set; }

		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			AccessProfileName = Convert.ToString(dataRow["AccessProfileName"]);
			Administration = Convert.ToBoolean(dataRow["Administration"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			CRP_AllRessourcesAuthorized = Convert.ToBoolean(dataRow["CRP_AllRessourcesAuthorized"]);
			CRP_Capacity = Convert.ToBoolean(dataRow["CRP_Capacity"]);
			CRP_CapacityEdit = Convert.ToBoolean(dataRow["CRP_CapacityEdit"]);
			CRP_CapacityPlan = Convert.ToBoolean(dataRow["CRP_CapacityPlan"]);
			CRP_CapacityPlanEdit = Convert.ToBoolean(dataRow["CRP_CapacityPlanEdit"]);
			CRP_Configuration = Convert.ToBoolean(dataRow["CRP_Configuration"]);
			CRP_ConfigurationEdit = Convert.ToBoolean(dataRow["CRP_ConfigurationEdit"]);
			CRP_Holiday = Convert.ToBoolean(dataRow["CRP_Holiday"]);
			CRP_HolidayEdit = Convert.ToBoolean(dataRow["CRP_HolidayEdit"]);
			CRP_RessourceAuthorizationEdit = Convert.ToBoolean(dataRow["CRP_RessourceAuthorizationEdit"]);
			CRP_Validation = Convert.ToBoolean(dataRow["CRP_Validation"]);
			CRP_ValidationEdit = Convert.ToBoolean(dataRow["CRP_ValidationEdit"]);
			DISPO_Dashboard = (dataRow["DISPO_Dashboard"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DISPO_Dashboard"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsDefault = Convert.ToBoolean(dataRow["IsDefault"]);
			ModuleActivated = Convert.ToBoolean(dataRow["ModuleActivated"]);
			ORD_Dashboard = (dataRow["ORD_Dashboard"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_Dashboard"]);
			ORD_Order = (dataRow["ORD_Order"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_Order"]);
			ORD_OrderAdd = (dataRow["ORD_OrderAdd"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_OrderAdd"]);
			ORD_OrderDelete = (dataRow["ORD_OrderDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_OrderDelete"]);
			ORD_OrderEdit = (dataRow["ORD_OrderEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_OrderEdit"]);
			ORD_OrderQuickPO = (dataRow["ORD_OrderQuickPO"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_OrderQuickPO"]);
			ORD_OrderUnValidate = (dataRow["ORD_OrderUnValidate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_OrderUnValidate"]);
			ORD_OrderValidate = (dataRow["ORD_OrderValidate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_OrderValidate"]);
			ORD_ProjectPurchaseDeleteOrder = (dataRow["ORD_ProjectPurchaseDeleteOrder"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_ProjectPurchaseDeleteOrder"]);
			ORD_ProjectPurchaseSetOrder = (dataRow["ORD_ProjectPurchaseSetOrder"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ORD_ProjectPurchaseSetOrder"]);
			Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen"]);
			RahmenAdd = (dataRow["RahmenAdd"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenAdd"]);
			RahmenAddPositions = (dataRow["RahmenAddPositions"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenAddPositions"]);
			RahmenCancelation = (dataRow["RahmenCancelation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenCancelation"]);
			RahmenClosure = (dataRow["RahmenClosure"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenClosure"]);
			RahmenDelete = (dataRow["RahmenDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenDelete"]);
			RahmenDeletePositions = (dataRow["RahmenDeletePositions"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenDeletePositions"]);
			RahmenDocumentFlow = (dataRow["RahmenDocumentFlow"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenDocumentFlow"]);
			RahmenEditHeader = (dataRow["RahmenEditHeader"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenEditHeader"]);
			RahmenEditPositions = (dataRow["RahmenEditPositions"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenEditPositions"]);
			RahmenHistory = (dataRow["RahmenHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenHistory"]);
			RahmenValdation = (dataRow["RahmenValdation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenValdation"]);
			STAT_Dashboard = (dataRow["STAT_Dashboard"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["STAT_Dashboard"]);
			WE = (dataRow["WE"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["WE"]);
			WE_Create = (dataRow["WE_Create"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["WE_Create"]);
			////Rami 09/09/2025
			WP_Country = dataRow["WP_Country"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_Country"]);
			WP_CountryCreate = dataRow["WP_CountryCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_CountryCreate"]);
			WP_CountryDelete = dataRow["WP_CountryDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_CountryDelete"]);
			WP_CountryUpdate = dataRow["WP_CountryUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_CountryUpdate"]);

			WP_Departement = dataRow["WP_Departement"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_Departement"]);
			WP_DepartementCreate = dataRow["WP_DepartementCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_DepartementCreate"]);
			WP_DepartementDelete = dataRow["WP_DepartementDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_DepartementDelete"]);
			WP_DepartementUpdate = dataRow["WP_DepartementUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_DepartementUpdate"]);

			WP_Hall = dataRow["WP_Hall"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_Hall"]);
			WP_HallCreate = dataRow["WP_HallCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_HallCreate"]);
			WP_HallDelete = dataRow["WP_HallDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_HallDelete"]);
			WP_HallUpdate = dataRow["WP_HallUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_HallUpdate"]);

			WP_StandardOperation = dataRow["WP_StandardOperation"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_StandardOperation"]);
			WP_StandardOperationCreate = dataRow["WP_StandardOperationCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_StandardOperationCreate"]);
			WP_StandardOperationDelete = dataRow["WP_StandardOperationDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_StandardOperationDelete"]);
			WP_StandardOperationUpdate = dataRow["WP_StandardOperationUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_StandardOperationUpdate"]);

			WP_WorkArea = dataRow["WP_WorkArea"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkArea"]);
			WP_WorkAreaCreate = dataRow["WP_WorkAreaCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkAreaCreate"]);
			WP_WorkAreaDelete = dataRow["WP_WorkAreaDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkAreaDelete"]);
			WP_WorkAreaUpdate = dataRow["WP_WorkAreaUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkAreaUpdate"]);

			WP_WorkPlan = dataRow["WP_WorkPlan"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkPlan"]);
			WP_WorkPlanCreate = dataRow["WP_WorkPlanCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkPlanCreate"]);
			WP_WorkPlanDelete = dataRow["WP_WorkPlanDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkPlanDelete"]);
			WP_WorkPlanUpdate = dataRow["WP_WorkPlanUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkPlanUpdate"]);

			WP_WorkPlanReporting = dataRow["WP_WorkPlanReporting"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkPlanReporting"]);
			WP_WorkPlanReportingCreate = dataRow["WP_WorkPlanReportingCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkPlanReportingCreate"]);
			WP_WorkPlanReportingDelete = dataRow["WP_WorkPlanReportingDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkPlanReportingDelete"]);
			WP_WorkPlanReportingUpdate = dataRow["WP_WorkPlanReportingUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkPlanReportingUpdate"]);

			WP_WorkStation = dataRow["WP_WorkStation"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkStation"]);
			WP_WorkStationCreate = dataRow["WP_WorkStationCreate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkStationCreate"]);
			WP_WorkStationDelete = dataRow["WP_WorkStationDelete"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkStationDelete"]);
			WP_WorkStationUpdate = dataRow["WP_WorkStationUpdate"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dataRow["WP_WorkStationUpdate"]);

		}

		public AccessProfileEntity ShallowClone()
		{
			return new AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				Administration = Administration,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CRP_AllRessourcesAuthorized = CRP_AllRessourcesAuthorized,
				CRP_Capacity = CRP_Capacity,
				CRP_CapacityEdit = CRP_CapacityEdit,
				CRP_CapacityPlan = CRP_CapacityPlan,
				CRP_CapacityPlanEdit = CRP_CapacityPlanEdit,
				CRP_Configuration = CRP_Configuration,
				CRP_ConfigurationEdit = CRP_ConfigurationEdit,
				CRP_Holiday = CRP_Holiday,
				CRP_HolidayEdit = CRP_HolidayEdit,
				CRP_RessourceAuthorizationEdit = CRP_RessourceAuthorizationEdit,
				CRP_Validation = CRP_Validation,
				CRP_ValidationEdit = CRP_ValidationEdit,
				DISPO_Dashboard = DISPO_Dashboard,
				Id = Id,
				IsDefault = IsDefault,
				ModuleActivated = ModuleActivated,
				ORD_Dashboard = ORD_Dashboard,
				ORD_Order = ORD_Order,
				ORD_OrderAdd = ORD_OrderAdd,
				ORD_OrderDelete = ORD_OrderDelete,
				ORD_OrderEdit = ORD_OrderEdit,
				ORD_OrderQuickPO = ORD_OrderQuickPO,
				ORD_OrderUnValidate = ORD_OrderUnValidate,
				ORD_OrderValidate = ORD_OrderValidate,
				ORD_ProjectPurchaseDeleteOrder = ORD_ProjectPurchaseDeleteOrder,
				ORD_ProjectPurchaseSetOrder = ORD_ProjectPurchaseSetOrder,
				Rahmen = Rahmen,
				RahmenAdd = RahmenAdd,
				RahmenAddPositions = RahmenAddPositions,
				RahmenCancelation = RahmenCancelation,
				RahmenClosure = RahmenClosure,
				RahmenDelete = RahmenDelete,
				RahmenDeletePositions = RahmenDeletePositions,
				RahmenDocumentFlow = RahmenDocumentFlow,
				RahmenEditHeader = RahmenEditHeader,
				RahmenEditPositions = RahmenEditPositions,
				RahmenHistory = RahmenHistory,
				RahmenValdation = RahmenValdation,
				STAT_Dashboard = STAT_Dashboard,
				WE = WE,
				WE_Create = WE_Create,
				//Rami 09/09/2025
				WP_Country = WP_Country,
				WP_CountryCreate = WP_CountryCreate,
				WP_CountryDelete = WP_CountryDelete,
				WP_CountryUpdate = WP_CountryUpdate,

				WP_Departement = WP_Departement,
				WP_DepartementCreate = WP_DepartementCreate,
				WP_DepartementDelete = WP_DepartementDelete,
				WP_DepartementUpdate = WP_DepartementUpdate,

				WP_Hall = WP_Hall,
				WP_HallCreate = WP_HallCreate,
				WP_HallDelete = WP_HallDelete,
				WP_HallUpdate = WP_HallUpdate,

				WP_StandardOperation = WP_StandardOperation,
				WP_StandardOperationCreate = WP_StandardOperationCreate,
				WP_StandardOperationDelete = WP_StandardOperationDelete,
				WP_StandardOperationUpdate = WP_StandardOperationUpdate,

				WP_WorkArea = WP_WorkArea,
				WP_WorkAreaCreate = WP_WorkAreaCreate,
				WP_WorkAreaDelete = WP_WorkAreaDelete,
				WP_WorkAreaUpdate = WP_WorkAreaUpdate,

				WP_WorkPlan = WP_WorkPlan,
				WP_WorkPlanCreate = WP_WorkPlanCreate,
				WP_WorkPlanDelete = WP_WorkPlanDelete,
				WP_WorkPlanUpdate = WP_WorkPlanUpdate,

				WP_WorkPlanReporting = WP_WorkPlanReporting,
				WP_WorkPlanReportingCreate = WP_WorkPlanReportingCreate,
				WP_WorkPlanReportingDelete = WP_WorkPlanReportingDelete,
				WP_WorkPlanReportingUpdate = WP_WorkPlanReportingUpdate,

				WP_WorkStation = WP_WorkStation,
				WP_WorkStationCreate = WP_WorkStationCreate,
				WP_WorkStationDelete = WP_WorkStationDelete,
				WP_WorkStationUpdate = WP_WorkStationUpdate

			};
		}
	}
}

