using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.MGO
{
	public class AccessProfileEntity
	{
		public string AccessProfileName { get; set; }
		public bool? Administration { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public bool? CtsPlanning { get; set; }
		public bool? CtsSales { get; set; }
		public bool? CustomerService { get; set; }
		public bool? DashboardProduction { get; set; }
		public bool? DashboardProductionEdit { get; set; }
		public int Id { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? PmExtraRoh { get; set; }
		public bool? PmFaultyArticles { get; set; }
		public bool? PmMissingRoh { get; set; }
		public bool? Production { get; set; }
		public bool? ProductionDashboard { get; set; }
		public bool? ProductionHourPlanningSerieFirstOrder { get; set; }
		public bool? ProductionPlanningByCustomer { get; set; }
		public bool? ProductionWorkLoad { get; set; }
		public bool? ProductSale { get; set; }
		public bool? ProjectManagment { get; set; }
		public bool? SalesHbgUbg { get; set; }
		public bool? SalesInvoice { get; set; }
		public bool? SalesOrders { get; set; }
		public bool? SalesProduction { get; set; }
		public bool? SalesRoh { get; set; }
		public bool? Statistics { get; set; }
		public bool? StatisticsDel { get; set; }
		public bool? StatisticsFgAL { get; set; }
		public bool? StatisticsFgBETN { get; set; }
		public bool? StatisticsFgCZ { get; set; }
		public bool? StatisticsFgGZ { get; set; }
		public bool? StatisticsFgTN { get; set; }
		public bool? StatisticsFgWS { get; set; }
		public bool? StatisticsLogs { get; set; }
		public bool? StatisticsReasonUpdate { get; set; }
		public bool? StatisticsUmsatz { get; set; }
		public bool? AllProductionWarehouses { get; set; }

		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
			Administration = (dataRow["Administration"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Administration"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CtsPlanning = (dataRow["CtsPlanning"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CtsPlanning"]);
			CtsSales = (dataRow["CtsSales"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CtsSales"]);
			CustomerService = (dataRow["CustomerService"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerService"]);
			DashboardProduction = (dataRow["DashboardProduction"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DashboardProduction"]);
			DashboardProductionEdit = (dataRow["DashboardProductionEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DashboardProductionEdit"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ModuleActivated = (dataRow["ModuleActivated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ModuleActivated"]);
			PmExtraRoh = (dataRow["PmExtraRoh"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PmExtraRoh"]);
			PmFaultyArticles = (dataRow["PmFaultyArticles"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PmFaultyArticles"]);
			PmMissingRoh = (dataRow["PmMissingRoh"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PmMissingRoh"]);
			Production = (dataRow["Production"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Production"]);
			ProductionDashboard = (dataRow["ProductionDashboard"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProductionDashboard"]);
			ProductionHourPlanningSerieFirstOrder = (dataRow["ProductionHourPlanningSerieFirstOrder"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProductionHourPlanningSerieFirstOrder"]);
			ProductionPlanningByCustomer = (dataRow["ProductionPlanningByCustomer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProductionPlanningByCustomer"]);
			ProductionWorkLoad = (dataRow["ProductionWorkLoad"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProductionWorkLoad"]);
			ProductSale = (dataRow["ProductSale"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProductSale"]);
			ProjectManagment = (dataRow["ProjectManagment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectManagment"]);
			SalesHbgUbg = (dataRow["SalesHbgUbg"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SalesHbgUbg"]);
			SalesInvoice = (dataRow["SalesInvoice"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SalesInvoice"]);
			SalesOrders = (dataRow["SalesOrders"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SalesOrders"]);
			SalesProduction = (dataRow["SalesProduction"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SalesProduction"]);
			SalesRoh = (dataRow["SalesRoh"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SalesRoh"]);
			Statistics = (dataRow["Statistics"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Statistics"]);
			StatisticsDel = (dataRow["StatisticsDel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsDel"]);
			StatisticsFgAL = (dataRow["StatisticsFgAL"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsFgAL"]);
			StatisticsFgBETN = (dataRow["StatisticsFgBETN"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsFgBETN"]);
			StatisticsFgCZ = (dataRow["StatisticsFgCZ"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsFgCZ"]);
			StatisticsFgGZ = (dataRow["StatisticsFgGZ"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsFgGZ"]);
			StatisticsFgTN = (dataRow["StatisticsFgTN"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsFgTN"]);
			StatisticsFgWS = (dataRow["StatisticsFgWS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsFgWS"]);
			StatisticsLogs = (dataRow["StatisticsLogs"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsLogs"]);
			StatisticsReasonUpdate = (dataRow["StatisticsReasonUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsReasonUpdate"]);
			StatisticsUmsatz = (dataRow["StatisticsUmsatz"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsUmsatz"]);
			AllProductionWarehouses = (dataRow["AllProductionWarehouses"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AllProductionWarehouses"]);
		}

		public AccessProfileEntity ShallowClone()
		{
			return new AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				Administration = Administration,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CtsPlanning = CtsPlanning,
				CtsSales = CtsSales,
				CustomerService = CustomerService,
				DashboardProduction = DashboardProduction,
				DashboardProductionEdit = DashboardProductionEdit,
				Id = Id,
				ModuleActivated = ModuleActivated,
				PmExtraRoh = PmExtraRoh,
				PmFaultyArticles = PmFaultyArticles,
				PmMissingRoh = PmMissingRoh,
				Production = Production,
				ProductionDashboard = ProductionDashboard,
				ProductionHourPlanningSerieFirstOrder = ProductionHourPlanningSerieFirstOrder,
				ProductionPlanningByCustomer = ProductionPlanningByCustomer,
				ProductionWorkLoad = ProductionWorkLoad,
				ProductSale = ProductSale,
				ProjectManagment = ProjectManagment,
				SalesHbgUbg = SalesHbgUbg,
				SalesInvoice = SalesInvoice,
				SalesOrders = SalesOrders,
				SalesProduction = SalesProduction,
				SalesRoh = SalesRoh,
				Statistics = Statistics,
				StatisticsDel = StatisticsDel,
				StatisticsFgAL = StatisticsFgAL,
				StatisticsFgBETN = StatisticsFgBETN,
				StatisticsFgCZ = StatisticsFgCZ,
				StatisticsFgGZ = StatisticsFgGZ,
				StatisticsFgTN = StatisticsFgTN,
				StatisticsFgWS = StatisticsFgWS,
				StatisticsLogs = StatisticsLogs,
				StatisticsReasonUpdate = StatisticsReasonUpdate,
				StatisticsUmsatz = StatisticsUmsatz,
				AllProductionWarehouses = AllProductionWarehouses
			};
		}
	}
}

