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
			};
		}
	}
}

