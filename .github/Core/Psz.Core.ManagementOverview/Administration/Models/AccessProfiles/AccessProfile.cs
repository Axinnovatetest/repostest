using Infrastructure.Data.Entities.Joins.CTS;
using System;

namespace Psz.Core.ManagementOverview.Administration.Models.AccessProfiles
{
	public class AccessProfileAddRequestModel
	{
		public int Id { get; set; }
		public string AccessProfileName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public bool ModuleActivated { get; set; } = false;
		public bool Administration { get; set; } = false;
		public bool CtsPlanning { get; set; } = false;
		public bool CtsSales { get; set; } = false;
		public bool PmExtraRoh { get; set; } = false;
		public bool PmFaultyArticles { get; set; } = false;
		public bool PmMissingRoh { get; set; } = false;
		public bool SalesHbgUbg { get; set; } = false;
		public bool SalesInvoice { get; set; } = false;
		public bool SalesOrders { get; set; } = false;
		public bool SalesProduction { get; set; } = false;
		public bool SalesRoh { get; set; } = false;
		public bool StatisticsDel { get; set; } = false;
		public bool StatisticsFgAL { get; set; } = false;
		public bool StatisticsFgBETN { get; set; } = false;
		public bool StatisticsFgCZ { get; set; } = false;
		public bool StatisticsFgGZ { get; set; } = false;
		public bool StatisticsFgTN { get; set; } = false;
		public bool StatisticsFgWS { get; set; } = false;
		public bool StatisticsLogs { get; set; } = false;
		public bool StatisticsReasonUpdate { get; set; } = false;
		public bool StatisticsUmsatz { get; set; } = false;
		public bool DashboardProduction { get; set; } = false;
		public bool DashboardProductionEdit { get; set; } = false;
		//-
		public bool? ProductionWorkLoad { get; set; } = false;
		public bool? ProductionHourPlanningSerieFirstOrder { get; set; } = false;
		public bool? ProductionDashboard { get; set; } = false;
		public bool? ProductionPlanningByCustomer { get; set; } = false;
		public bool? CustomerService { get; set; } = false;
		public bool? ProjectManagment { get; set; } = false;
		public bool? ProductSale { get; set; } = false;
		public bool? Statistics { get; set; } = false;
		public bool? Production { get; set; } = false;
		public AccessProfileAddRequestModel()
		{

		}
		public AccessProfileAddRequestModel(Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity entity)
		{
			if(entity == null)
				return;

			// -
			Id = entity.Id;
			AccessProfileName = entity.AccessProfileName;
			CreationTime = entity.CreationTime;
			CreationUserId = entity.CreationUserId;
			Administration = entity.Administration ?? false;
			CtsPlanning = entity.CtsPlanning ?? false;
			CtsSales = entity.CtsSales ?? false;
			ModuleActivated = entity.ModuleActivated ?? false;
			PmExtraRoh = entity.PmExtraRoh ?? false;
			PmFaultyArticles = entity.PmFaultyArticles ?? false;
			PmMissingRoh = entity.PmMissingRoh ?? false;
			SalesHbgUbg = entity.SalesHbgUbg ?? false;
			SalesInvoice = entity.SalesInvoice ?? false;
			SalesOrders = entity.SalesOrders ?? false;
			SalesProduction = entity.SalesProduction ?? false;
			SalesRoh = entity.SalesRoh ?? false;
			StatisticsDel = entity.StatisticsDel ?? false;
			StatisticsFgAL = entity.StatisticsFgAL ?? false;
			StatisticsFgBETN = entity.StatisticsFgBETN ?? false;
			StatisticsFgCZ = entity.StatisticsFgCZ ?? false;
			StatisticsFgGZ = entity.StatisticsFgGZ ?? false;
			StatisticsFgTN = entity.StatisticsFgTN ?? false;
			StatisticsFgWS = entity.StatisticsFgWS ?? false;
			StatisticsLogs = entity.StatisticsLogs ?? false;
			StatisticsReasonUpdate = entity.StatisticsReasonUpdate ?? false;
			StatisticsUmsatz = entity.StatisticsUmsatz ?? false;
			DashboardProduction = entity.DashboardProduction ?? false;
			DashboardProductionEdit = entity.DashboardProductionEdit ?? false;
			CustomerService = entity.CustomerService ?? false;
			ProjectManagment = entity.ProjectManagment ?? false;
			ProductSale = entity.ProductSale ?? false;
			Statistics = entity.Statistics ?? false;
			Production = entity.Production ?? false;
			//-
			ProductionWorkLoad = entity.ProductionWorkLoad;
			ProductionHourPlanningSerieFirstOrder = entity.ProductionHourPlanningSerieFirstOrder;
			ProductionDashboard = entity.ProductionDashboard;
			ProductionPlanningByCustomer = entity.ProductionPlanningByCustomer;
		}
		public Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				Administration = Administration,
				CtsPlanning = CtsPlanning,
				CtsSales = CtsSales,
				Id = Id,
				PmExtraRoh = PmExtraRoh,
				PmFaultyArticles = PmFaultyArticles,
				PmMissingRoh = PmMissingRoh,
				SalesHbgUbg = SalesHbgUbg,
				SalesInvoice = SalesInvoice,
				SalesOrders = SalesOrders,
				SalesProduction = SalesProduction,
				SalesRoh = SalesRoh,
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
				DashboardProduction = DashboardProduction,
				DashboardProductionEdit = DashboardProductionEdit,
				//-
				ProductionWorkLoad = ProductionWorkLoad,
				ProductionHourPlanningSerieFirstOrder = ProductionHourPlanningSerieFirstOrder,
				ProductionDashboard = ProductionDashboard,
				ProductionPlanningByCustomer = ProductionPlanningByCustomer,
				CustomerService = CustomerService,
				ProjectManagment = ProjectManagment,
				ProductSale = ProductSale,
				Statistics = Statistics,
				Production = Production,
			};
		}
	}
}