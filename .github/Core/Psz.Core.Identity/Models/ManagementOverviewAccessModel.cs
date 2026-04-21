using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class ManagementOverviewAccessModel
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
		public bool ProductionWorkLoad { get; set; } = false;
		public bool ProductionHourPlanningSerieFirstOrder { get; set; } = false;
		public bool ProductionDashboard { get; set; } = false;
		public bool ProductionPlanningByCustomer { get; set; } = false;
		public bool CustomerService { get; set; } = false;
		public bool ProjectManagment { get; set; } = false;
		public bool ProductSale { get; set; } = false;
		public bool Statistics { get; set; } = false;
		public bool Production { get; set; } = false;
		public bool AllProductionWarehouses { get; set; } = false;
		public ManagementOverviewAccessModel()
		{
			// - 
			ModuleActivated = false;
		}
		public ManagementOverviewAccessModel(ManagementOverviewAccessModel model)
		{
			if(model == null)
				return;
			// - 
			Id = model.Id;
			AccessProfileName = model.AccessProfileName;
			CreationTime = model.CreationTime;
			CreationUserId = model.CreationUserId;
			ModuleActivated = model.ModuleActivated;
			Administration = model.Administration;
			CtsPlanning = model.CtsPlanning;
			CtsSales = model.CtsSales;
			PmExtraRoh = model.PmExtraRoh;
			PmFaultyArticles = model.PmFaultyArticles;
			PmMissingRoh = model.PmMissingRoh;
			SalesHbgUbg = model.SalesHbgUbg;
			SalesInvoice = model.SalesInvoice;
			SalesOrders = model.SalesOrders;
			SalesProduction = model.SalesProduction;
			SalesRoh = model.SalesRoh;
			StatisticsDel = model.StatisticsDel;
			StatisticsFgAL = model.StatisticsFgAL;
			StatisticsFgBETN = model.StatisticsFgBETN;
			StatisticsFgCZ = model.StatisticsFgCZ;
			StatisticsFgGZ = model.StatisticsFgGZ;
			StatisticsFgTN = model.StatisticsFgTN;
			StatisticsFgWS = model.StatisticsFgWS;
			StatisticsLogs = model.StatisticsLogs;
			StatisticsReasonUpdate = model.StatisticsReasonUpdate;
			StatisticsUmsatz = model.StatisticsUmsatz;
			DashboardProduction = model.DashboardProduction;
			DashboardProductionEdit = model.DashboardProductionEdit;
			//-
			ProductionWorkLoad = model.ProductionWorkLoad;
			ProductionHourPlanningSerieFirstOrder = model.ProductionHourPlanningSerieFirstOrder;
			ProductionDashboard = model.ProductionDashboard;
			ProductionPlanningByCustomer = model.ProductionPlanningByCustomer;
			CustomerService = model.CustomerService;
			ProjectManagment = model.ProjectManagment;
			ProductSale = model.ProductSale;
			Statistics = model.Statistics;
			Production = model.Production;
			AllProductionWarehouses = model.AllProductionWarehouses;
		}
		public ManagementOverviewAccessModel(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> profileEntities)
		{
			ModuleActivated = false;
			Administration = false;
			CtsPlanning = false;
			CtsSales = false;
			PmExtraRoh = false;
			PmFaultyArticles = false;
			PmMissingRoh = false;
			SalesHbgUbg = false;
			SalesInvoice = false;
			SalesOrders = false;
			SalesProduction = false;
			SalesRoh = false;
			StatisticsDel = false;
			StatisticsFgAL = false;
			StatisticsFgBETN = false;
			StatisticsFgCZ = false;
			StatisticsFgGZ = false;
			StatisticsFgTN = false;
			StatisticsFgWS = false;
			StatisticsLogs = false;
			StatisticsReasonUpdate = false;
			StatisticsUmsatz = false;
			DashboardProduction = false;
			DashboardProductionEdit = false;
			//-
			ProductionWorkLoad = false;
			ProductionHourPlanningSerieFirstOrder = false;
			ProductionDashboard = false;
			ProductionPlanningByCustomer = false;
			CustomerService = false;
			ProjectManagment = false;
			ProductSale = false;
			Statistics = false;
			Production = false;
			AllProductionWarehouses = false;

			// - 
			if(profileEntities == null || profileEntities.Count <= 0)
				return;

			foreach(var entity in profileEntities)
			{
				ModuleActivated = ModuleActivated || (entity.ModuleActivated ?? false);
				Administration = Administration || (entity.Administration ?? false);
				CtsPlanning = CtsPlanning || (entity.CtsPlanning ?? false);
				CtsSales = CtsSales || (entity.CtsSales ?? false);
				PmExtraRoh = PmExtraRoh || (entity.PmExtraRoh ?? false);
				PmFaultyArticles = PmFaultyArticles || (entity.PmFaultyArticles ?? false);
				PmMissingRoh = PmMissingRoh || (entity.PmMissingRoh ?? false);
				SalesHbgUbg = SalesHbgUbg || (entity.SalesHbgUbg ?? false);
				SalesInvoice = SalesInvoice || (entity.SalesInvoice ?? false);
				SalesOrders = SalesOrders || (entity.SalesOrders ?? false);
				SalesProduction = SalesProduction || (entity.SalesProduction ?? false);
				SalesRoh = SalesRoh || (entity.SalesRoh ?? false);
				StatisticsDel = StatisticsDel || (entity.StatisticsDel ?? false);
				StatisticsFgAL = StatisticsFgAL || (entity.StatisticsFgAL ?? false);
				StatisticsFgBETN = StatisticsFgBETN || (entity.StatisticsFgBETN ?? false);
				StatisticsFgCZ = StatisticsFgCZ || (entity.StatisticsFgCZ ?? false);
				StatisticsFgGZ = StatisticsFgGZ || (entity.StatisticsFgGZ ?? false);
				StatisticsFgTN = StatisticsFgTN || (entity.StatisticsFgTN ?? false);
				StatisticsFgWS = StatisticsFgWS || (entity.StatisticsFgWS ?? false);
				StatisticsLogs = StatisticsLogs || (entity.StatisticsLogs ?? false);
				StatisticsReasonUpdate = StatisticsReasonUpdate || (entity.StatisticsReasonUpdate ?? false);
				StatisticsUmsatz = StatisticsUmsatz || (entity.StatisticsUmsatz ?? false);
				DashboardProduction = DashboardProduction || (entity.DashboardProduction ?? false);
				DashboardProductionEdit = DashboardProductionEdit || (entity.DashboardProductionEdit ?? false);
				//-
				ProductionWorkLoad = ProductionWorkLoad || (entity.ProductionWorkLoad ?? false);
				ProductionHourPlanningSerieFirstOrder = ProductionHourPlanningSerieFirstOrder || (entity.ProductionHourPlanningSerieFirstOrder ?? false);
				ProductionDashboard = ProductionDashboard || (entity.ProductionDashboard ?? false);
				ProductionPlanningByCustomer = ProductionPlanningByCustomer || (entity.ProductionPlanningByCustomer ?? false);
				CustomerService = CustomerService || (entity.CustomerService ?? false);
				ProjectManagment = ProjectManagment || (entity.ProjectManagment ?? false);
				ProductSale = ProductSale || (entity.ProductSale ?? false);
				Statistics = Statistics || (entity.Statistics ?? false);
				Production = Production || (entity.Production ?? false);
				AllProductionWarehouses = AllProductionWarehouses || (entity.AllProductionWarehouses ?? false);
			}
			// - 
			ModuleActivated = Administration ||
				CtsPlanning ||
				CtsSales ||
				PmExtraRoh ||
				PmFaultyArticles ||
				PmMissingRoh ||
				SalesHbgUbg ||
				SalesInvoice ||
				SalesOrders ||
				SalesProduction ||
				SalesRoh ||
				StatisticsDel ||
				StatisticsFgAL ||
				StatisticsFgBETN ||
				StatisticsFgCZ ||
				StatisticsFgGZ ||
				StatisticsFgTN ||
				StatisticsFgWS ||
				StatisticsLogs ||
				StatisticsReasonUpdate ||
				StatisticsUmsatz ||
				DashboardProduction ||
				DashboardProductionEdit ||
				ProductionWorkLoad ||
				ProductionHourPlanningSerieFirstOrder ||
				ProductionDashboard ||
				ProductionPlanningByCustomer ||
				CustomerService ||
				ProjectManagment ||
				ProductSale ||
				Statistics ||
				Production ||
				AllProductionWarehouses;
		}
	}
}