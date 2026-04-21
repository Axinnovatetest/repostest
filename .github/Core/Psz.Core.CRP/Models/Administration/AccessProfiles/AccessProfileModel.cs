using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.Administration.AccessProfiles
{
	public class AccessProfileModel
	{
		public string AccessProfileName { get; set; }
		public bool? Administration { get; set; }
		public bool? Configuration { get; set; }
		public bool? ConfigurationAppoitments { get; set; }
		public bool? ConfigurationChangeEmployees { get; set; }
		public bool? ConfigurationReplacements { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public bool? DelforCreate { get; set; }
		public bool? DelforDelete { get; set; }
		public bool? DelforDeletePosition { get; set; }
		public bool? DelforOrderConfirmation { get; set; }
		public bool? DelforReport { get; set; }
		public bool? DelforStatistics { get; set; }
		public bool? DelforView { get; set; }
		public bool? DLFPosHorizon1 { get; set; }
		public bool? DLFPosHorizon2 { get; set; }
		public bool? DLFPosHorizon3 { get; set; }
		public bool? FaABEdit { get; set; }
		public bool? FaABView { get; set; }
		public bool? FaActionBook { get; set; }
		public bool? FaActionComplete { get; set; }
		public bool? FaActionDelete { get; set; }
		public bool? FaActionPrint { get; set; }
		public bool? FaAdmin { get; set; }
		public bool? FAAKtualTerminUpdate { get; set; }
		public bool? FaAnalysis { get; set; }
		public bool? FAAuswertungEndkontrolle { get; set; }
		public bool? FABemerkungPlannug { get; set; }
		public bool? FABemerkungZuGewerk { get; set; }
		public bool? FABemerkungZuPrio { get; set; }
		public bool? FACancelHorizon1 { get; set; }
		public bool? FACancelHorizon2 { get; set; }
		public bool? FACancelHorizon3 { get; set; }
		public bool? FACommissionert { get; set; }
		public bool? FaCreate { get; set; }
		public bool? FACreateHorizon1 { get; set; }
		public bool? FACreateHorizon2 { get; set; }
		public bool? FACreateHorizon3 { get; set; }
		public bool? FaDatenEdit { get; set; }
		public bool? FaDatenView { get; set; }
		public bool? FaDelete { get; set; }
		public bool? FADrucken { get; set; }
		public bool? FaEdit { get; set; }
		public bool? FAErlidegen { get; set; }
		public bool? FAExcelUpdateWerk { get; set; }
		public bool? FAExcelUpdateWunsh { get; set; }
		public bool? FAFehlrMaterial { get; set; }
		public bool? FaHomeAnalysis { get; set; }
		public bool? FaHomeUpdate { get; set; }
		public bool? FALaufkarteSchneiderei { get; set; }
		public bool? FaPlanningEdit { get; set; }
		public bool? FaPlanningView { get; set; }
		public bool? FAPlannung { get; set; }
		public bool? FAPlannungTechnick { get; set; }
		public bool? FAPriesZeitUpdate { get; set; }
		public bool? FAProductionPlannung { get; set; }
		public bool? FAStappleDruck { get; set; }
		public bool? FAStatusAlbania { get; set; }
		public bool? FAStatusCzech { get; set; }
		public bool? FAStatusTunisia { get; set; }
		public bool? FAStorno { get; set; }
		public bool? FAStucklist { get; set; }
		public bool? FaTechnicEdit { get; set; }
		public bool? FaTechnicView { get; set; }
		public bool? FATerminWerk { get; set; }
		public bool? FAUpdateBemerkungExtern { get; set; }
		public bool? FAUpdateByArticle { get; set; }
		public bool? FAUpdateByFA { get; set; }
		public bool? FAUpdateTerminHorizon1 { get; set; }
		public bool? FAUpdateTerminHorizon2 { get; set; }
		public bool? FAUpdateTerminHorizon3 { get; set; }
		public bool? FAWerkWunshAdmin { get; set; }
		public bool? Fertigung { get; set; }
		public bool? FertigungLog { get; set; }
		public int Id { get; set; }
		public bool? isDefault { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? Statistics { get; set; }
		public bool? StatsCapaHorizons { get; set; }
		public bool? StatsCapaPlanning { get; set; }
		public bool? StatsStockCS { get; set; }
		public bool? StatsStockExternalWarehouse { get; set; }
		public bool? StatsStockFG { get; set; }
		public bool? UBGStatusChange { get; set; }
		public bool? Forecast { get; set; }
		public bool? ForecastCreate { get; set; }
		public bool? ForecastDelete { get; set; }
		public bool? ForecastStatistics { get; set; }
		public bool? CRPFAPlannung { get; set; }
		//
		public bool? StatsCreatedInvoices { get; set; }
		public bool? StatsBacklogFG { get; set; }
		public bool? StatsDeliveries { get; set; }
		public bool? StatsFAAnderungshistoire { get; set; }
		public bool? StatsAnteileingelasteteFA { get; set; }
		public bool? StatsLagerbestandFGCRP { get; set; }
		public bool? SystemLogs { get; set; }
		public bool? CRPPlanning { get; set; }
		public bool? CRPUBGPlanning { get; set; }
		public bool? CRPRequirement { get; set; }
		//- 
		public bool? StatsRahmenSale { get; set; }
		//-- crp dahboard
		public bool? CRPDashboardCreatedOrders { get; set; }
		public bool? CRPDashboardCancelledOrders { get; set; }
		public bool? CRPDashboardActiveArticles { get; set; }
		public bool? CRPDashboardOpenOrders { get; set; }
		public bool? CRPDashboardOpenOrdersHours { get; set; }
		public bool? CRPDashboardTotalStockFG { get; set; }
		//-- souilmi 14-05-2025
		public bool? FAUpdatePrio { get; set; }

		//--FG Bestand History
		public bool? ViewFGBestandHistory { get; set; }
		public bool? ImportFGXlsHistory { get; set; }
		public bool? ExportFGXlsHistory { get; set; }
		public bool? LogsFGXlsHistory { get; set; }
		public bool? AgentFGXlsHistory { get; set; }

		//-
		public bool? FAPlannungHistory { get; set; }
		public bool? FAPlannungHistoryXLSImport { get; set; }
		public bool? FAPlannungHistoryXLSExport { get; set; }
		public bool? FAPlannungHistoryForceAgent { get; set; }

		//-- CRP Statistics FA Changes
		public bool? FaDateChangeHistory { get; set; }
		public bool? FaHoursMovement { get; set; }
		public bool? AllFaChanges { get; set; }
		public bool? FaPlanningViolation { get; set; }
		// -- CRP Fa hours planning by customer ( from mgo production)

		public bool? ProductionPlanningByCustomer { get; set; }
		public bool? AllProductionWarehouses { get; set; }
		public AccessProfileModel()
		{

		}
		public AccessProfileModel(Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity accessProfileEntity)
		{
			Id = accessProfileEntity.Id;
			AccessProfileName = accessProfileEntity.AccessProfileName;
			Administration = accessProfileEntity.Administration;
			Configuration = accessProfileEntity.Configuration;
			ConfigurationAppoitments = accessProfileEntity.ConfigurationAppoitments;
			ConfigurationChangeEmployees = accessProfileEntity.ConfigurationChangeEmployees;
			ConfigurationReplacements = accessProfileEntity.ConfigurationReplacements;
			DelforCreate = accessProfileEntity.DelforCreate;
			DelforDelete = accessProfileEntity.DelforDelete;
			DelforDeletePosition = accessProfileEntity.DelforDeletePosition;
			DelforOrderConfirmation = accessProfileEntity.DelforOrderConfirmation;
			DelforReport = accessProfileEntity.DelforReport;
			DelforStatistics = accessProfileEntity.DelforStatistics;
			DelforView = accessProfileEntity.DelforView;
			DLFPosHorizon1 = accessProfileEntity.DLFPosHorizon1;
			DLFPosHorizon2 = accessProfileEntity.DLFPosHorizon2;
			DLFPosHorizon3 = accessProfileEntity.DLFPosHorizon3;
			FaABEdit = accessProfileEntity.FaABEdit;
			FaABView = accessProfileEntity.FaABView;
			FaActionBook = accessProfileEntity.FaActionBook;
			FaActionComplete = accessProfileEntity.FaActionComplete;
			FaActionDelete = accessProfileEntity.FaActionDelete;
			FaActionPrint = accessProfileEntity.FaActionPrint;
			FaAdmin = accessProfileEntity.FaAdmin;
			FAAKtualTerminUpdate = accessProfileEntity.FAAKtualTerminUpdate;
			FaAnalysis = accessProfileEntity.FaAnalysis;
			FAAuswertungEndkontrolle = accessProfileEntity.FAAuswertungEndkontrolle;
			FABemerkungPlannug = accessProfileEntity.FABemerkungPlannug;
			FABemerkungZuGewerk = accessProfileEntity.FABemerkungZuGewerk;
			FABemerkungZuPrio = accessProfileEntity.FABemerkungZuPrio;
			FACancelHorizon1 = accessProfileEntity.FACancelHorizon1;
			FACancelHorizon2 = accessProfileEntity.FACancelHorizon2;
			FACancelHorizon3 = accessProfileEntity.FACancelHorizon3;
			FACommissionert = accessProfileEntity.FACommissionert;
			FaCreate = accessProfileEntity.FaCreate;
			FACreateHorizon1 = accessProfileEntity.FACreateHorizon1;
			FACreateHorizon2 = accessProfileEntity.FACreateHorizon2;
			FACreateHorizon3 = accessProfileEntity.FACreateHorizon3;
			FaDatenEdit = accessProfileEntity.FaDatenEdit;
			FaDatenView = accessProfileEntity.FaDatenView;
			FaDelete = accessProfileEntity.FaDelete;
			FADrucken = accessProfileEntity.FADrucken;
			FaEdit = accessProfileEntity.FaEdit;
			FAErlidegen = accessProfileEntity.FAErlidegen;
			FAExcelUpdateWerk = accessProfileEntity.FAExcelUpdateWerk;
			FAExcelUpdateWunsh = accessProfileEntity.FAExcelUpdateWunsh;
			FAFehlrMaterial = accessProfileEntity.FAFehlrMaterial;
			FaHomeAnalysis = accessProfileEntity.FaHomeAnalysis;
			FaHomeUpdate = accessProfileEntity.FaHomeUpdate;
			FALaufkarteSchneiderei = accessProfileEntity.FALaufkarteSchneiderei;
			FaPlanningEdit = accessProfileEntity.FaPlanningEdit;
			FaPlanningView = accessProfileEntity.FaPlanningView;
			FAPlannung = accessProfileEntity.FAPlannung;
			FAPlannungTechnick = accessProfileEntity.FAPlannungTechnick;
			FAPriesZeitUpdate = accessProfileEntity.FAPriesZeitUpdate;
			FAProductionPlannung = accessProfileEntity.FAProductionPlannung;
			FAStappleDruck = accessProfileEntity.FAStappleDruck;
			FAStatusAlbania = accessProfileEntity.FAStatusAlbania;
			FAStatusCzech = accessProfileEntity.FAStatusCzech;
			FAStatusTunisia = accessProfileEntity.FAStatusTunisia;
			FAStorno = accessProfileEntity.FAStorno;
			FAStucklist = accessProfileEntity.FAStucklist;
			FaTechnicEdit = accessProfileEntity.FaTechnicEdit;
			FaTechnicView = accessProfileEntity.FaTechnicView;
			FATerminWerk = accessProfileEntity.FATerminWerk;
			FAUpdateBemerkungExtern = accessProfileEntity.FAUpdateBemerkungExtern;
			FAUpdateByArticle = accessProfileEntity.FAUpdateByArticle;
			FAUpdateByFA = accessProfileEntity.FAUpdateByFA;
			FAUpdateTerminHorizon1 = accessProfileEntity.FAUpdateTerminHorizon1;
			FAUpdateTerminHorizon2 = accessProfileEntity.FAUpdateTerminHorizon2;
			FAUpdateTerminHorizon3 = accessProfileEntity.FAUpdateTerminHorizon3;
			FAWerkWunshAdmin = accessProfileEntity.FAWerkWunshAdmin;
			Fertigung = accessProfileEntity.Fertigung;
			FertigungLog = accessProfileEntity.FertigungLog;
			isDefault = accessProfileEntity.isDefault;
			ModuleActivated = accessProfileEntity.ModuleActivated;
			Statistics = accessProfileEntity.Statistics;
			StatsCapaHorizons = accessProfileEntity.StatsCapaHorizons;
			StatsCapaPlanning = accessProfileEntity.StatsCapaPlanning;
			StatsStockCS = accessProfileEntity.StatsStockCS;
			StatsStockExternalWarehouse = accessProfileEntity.StatsStockExternalWarehouse;
			StatsStockFG = accessProfileEntity.StatsStockFG;
			UBGStatusChange = accessProfileEntity.UBGStatusChange;
			Forecast = accessProfileEntity.Forecast;
			ForecastCreate = accessProfileEntity.ForecastCreate;
			ForecastDelete = accessProfileEntity.ForecastDelete;
			ForecastStatistics = accessProfileEntity.ForecastStatistics;
			CRPFAPlannung = accessProfileEntity.CRPFAPlannung;
			//
			StatsCreatedInvoices = accessProfileEntity.StatsCreatedInvoices;
			StatsBacklogFG = accessProfileEntity.StatsBacklogFG;
			StatsDeliveries = accessProfileEntity.StatsDeliveries;
			StatsFAAnderungshistoire = accessProfileEntity.StatsFAAnderungshistoire;
			StatsAnteileingelasteteFA = accessProfileEntity.StatsAnteileingelasteteFA;
			StatsLagerbestandFGCRP = accessProfileEntity.StatsLagerbestandFGCRP;
			SystemLogs = accessProfileEntity.SystemLogs;
			CRPPlanning = accessProfileEntity.CRPPlanning;
			CRPUBGPlanning = accessProfileEntity.CRPUBGPlanning;
			CRPRequirement = accessProfileEntity.CRPRequirement;
			//-
			FAPlannungHistory = accessProfileEntity.FAPlannungHistory;
			FAPlannungHistoryXLSImport = accessProfileEntity.FAPlannungHistoryXLSImport;
			FAPlannungHistoryXLSExport = accessProfileEntity.FAPlannungHistoryXLSExport;
			FAPlannungHistoryForceAgent = accessProfileEntity.FAPlannungHistoryForceAgent;
			//-
			StatsRahmenSale = accessProfileEntity.StatsRahmenSale;
			//-- crp dashboard
			CRPDashboardCreatedOrders = accessProfileEntity.CRPDashboardCreatedOrders;
			CRPDashboardCancelledOrders = accessProfileEntity.CRPDashboardCancelledOrders;
			CRPDashboardActiveArticles = accessProfileEntity.CRPDashboardActiveArticles;
			CRPDashboardOpenOrders = accessProfileEntity.CRPDashboardOpenOrders;
			CRPDashboardOpenOrdersHours = accessProfileEntity.CRPDashboardOpenOrdersHours;
			CRPDashboardTotalStockFG = accessProfileEntity.CRPDashboardTotalStockFG;

			//--FG Bestand History
			ViewFGBestandHistory = accessProfileEntity.ViewFGBestandHistory;
			ImportFGXlsHistory = accessProfileEntity.ImportFGXlsHistory;
			ExportFGXlsHistory = accessProfileEntity.ExportFGXlsHistory;
			LogsFGXlsHistory = accessProfileEntity.LogsFGXlsHistory;
			AgentFGXlsHistory = accessProfileEntity.AgentFGXlsHistory;
			//-- souilmi 14-05-2025
			FAUpdatePrio = accessProfileEntity.FAUpdatePrio;

			FaDateChangeHistory = accessProfileEntity.FaDateChangeHistory;
			FaHoursMovement = accessProfileEntity.FaHoursMovement;
			AllFaChanges = accessProfileEntity.AllFaChanges;
			FaPlanningViolation = accessProfileEntity.FaPlanningViolation;
			ProductionPlanningByCustomer = accessProfileEntity.ProductionPlanningByCustomer;
			AllProductionWarehouses = accessProfileEntity.AllProductionWarehouses;

		}
		public Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity
			{
				Id = Id,
				AccessProfileName = AccessProfileName,
				Administration = Administration,
				Configuration = Configuration,
				ConfigurationAppoitments = ConfigurationAppoitments,
				ConfigurationChangeEmployees = ConfigurationChangeEmployees,
				ConfigurationReplacements = ConfigurationReplacements,
				DelforCreate = DelforCreate,
				DelforDelete = DelforDelete,
				DelforDeletePosition = DelforDeletePosition,
				DelforOrderConfirmation = DelforOrderConfirmation,
				DelforReport = DelforReport,
				DelforStatistics = DelforStatistics,
				DelforView = DelforView,
				DLFPosHorizon1 = DLFPosHorizon1,
				DLFPosHorizon2 = DLFPosHorizon2,
				DLFPosHorizon3 = DLFPosHorizon3,
				FaABEdit = FaABEdit,
				FaABView = FaABView,
				FaActionBook = FaActionBook,
				FaActionComplete = FaActionComplete,
				FaActionDelete = FaActionDelete,
				FaActionPrint = FaActionPrint,
				FaAdmin = FaAdmin,
				FAAKtualTerminUpdate = FAAKtualTerminUpdate,
				FaAnalysis = FaAnalysis,
				FAAuswertungEndkontrolle = FAAuswertungEndkontrolle,
				FABemerkungPlannug = FABemerkungPlannug,
				FABemerkungZuGewerk = FABemerkungZuGewerk,
				FABemerkungZuPrio = FABemerkungZuPrio,
				FACancelHorizon1 = FACancelHorizon1,
				FACancelHorizon2 = FACancelHorizon2,
				FACancelHorizon3 = FACancelHorizon3,
				FACommissionert = FACommissionert,
				FaCreate = FaCreate,
				FACreateHorizon1 = FACreateHorizon1,
				FACreateHorizon2 = FACreateHorizon2,
				FACreateHorizon3 = FACreateHorizon3,
				FaDatenEdit = FaDatenEdit,
				FaDatenView = FaDatenView,
				FaDelete = FaDelete,
				FADrucken = FADrucken,
				FaEdit = FaEdit,
				FAErlidegen = FAErlidegen,
				FAExcelUpdateWerk = FAExcelUpdateWerk,
				FAExcelUpdateWunsh = FAExcelUpdateWunsh,
				FAFehlrMaterial = FAFehlrMaterial,
				FaHomeAnalysis = FaHomeAnalysis,
				FaHomeUpdate = FaHomeUpdate,
				FALaufkarteSchneiderei = FALaufkarteSchneiderei,
				FaPlanningEdit = FaPlanningEdit,
				FaPlanningView = FaPlanningView,
				FAPlannung = FAPlannung,
				FAPlannungTechnick = FAPlannungTechnick,
				FAPriesZeitUpdate = FAPriesZeitUpdate,
				FAProductionPlannung = FAProductionPlannung,
				FAStappleDruck = FAStappleDruck,
				FAStatusAlbania = FAStatusAlbania,
				FAStatusCzech = FAStatusCzech,
				FAStatusTunisia = FAStatusTunisia,
				FAStorno = FAStorno,
				FAStucklist = FAStucklist,
				FaTechnicEdit = FaTechnicEdit,
				FaTechnicView = FaTechnicView,
				FATerminWerk = FATerminWerk,
				FAUpdateBemerkungExtern = FAUpdateBemerkungExtern,
				FAUpdateByArticle = FAUpdateByArticle,
				FAUpdateByFA = FAUpdateByFA,
				FAUpdateTerminHorizon1 = FAUpdateTerminHorizon1,
				FAUpdateTerminHorizon2 = FAUpdateTerminHorizon2,
				FAUpdateTerminHorizon3 = FAUpdateTerminHorizon3,
				FAWerkWunshAdmin = FAWerkWunshAdmin,
				Fertigung = Fertigung,
				FertigungLog = FertigungLog,
				isDefault = isDefault,
				ModuleActivated = ModuleActivated,
				Statistics = Statistics,
				StatsCapaHorizons = StatsCapaHorizons,
				StatsCapaPlanning = StatsCapaPlanning,
				StatsStockCS = StatsStockCS,
				StatsStockExternalWarehouse = StatsStockExternalWarehouse,
				StatsStockFG = StatsStockFG,
				UBGStatusChange = UBGStatusChange,
				Forecast = Forecast,
				ForecastCreate = ForecastCreate,
				ForecastDelete = ForecastDelete,
				ForecastStatistics = ForecastStatistics,
				CRPFAPlannung = CRPFAPlannung,
				//
				StatsCreatedInvoices = StatsCreatedInvoices,
				StatsBacklogFG = StatsBacklogFG,
				StatsDeliveries = StatsDeliveries,
				StatsFAAnderungshistoire = StatsFAAnderungshistoire,
				StatsAnteileingelasteteFA = StatsAnteileingelasteteFA,
				StatsLagerbestandFGCRP = StatsLagerbestandFGCRP,
				SystemLogs = SystemLogs,
				CRPPlanning = CRPPlanning,
				CRPUBGPlanning = CRPUBGPlanning,
				CRPRequirement = CRPRequirement,
				//-
				FAPlannungHistory = FAPlannungHistory,
				FAPlannungHistoryXLSImport = FAPlannungHistoryXLSImport,
				FAPlannungHistoryXLSExport = FAPlannungHistoryXLSExport,
				FAPlannungHistoryForceAgent = FAPlannungHistoryForceAgent,
				//-
				StatsRahmenSale = StatsRahmenSale,
				//-- crp dashboard
				CRPDashboardCreatedOrders = CRPDashboardCreatedOrders,
				CRPDashboardCancelledOrders = CRPDashboardCancelledOrders,
				CRPDashboardActiveArticles = CRPDashboardActiveArticles,
				CRPDashboardOpenOrders = CRPDashboardOpenOrders,
				CRPDashboardOpenOrdersHours = CRPDashboardOpenOrdersHours,
				CRPDashboardTotalStockFG = CRPDashboardTotalStockFG,

				//--FG Bestand History
				ViewFGBestandHistory = ViewFGBestandHistory,
				ImportFGXlsHistory = ImportFGXlsHistory,
				ExportFGXlsHistory = ExportFGXlsHistory,
				LogsFGXlsHistory = LogsFGXlsHistory,
				AgentFGXlsHistory = AgentFGXlsHistory,

				//-- souilmi 14-05-2025
				FAUpdatePrio = FAUpdatePrio,
				FaDateChangeHistory = FaDateChangeHistory,
				FaHoursMovement = FaHoursMovement,
				AllFaChanges = AllFaChanges,
				FaPlanningViolation = FaPlanningViolation,
				ProductionPlanningByCustomer = ProductionPlanningByCustomer,
				AllProductionWarehouses = AllProductionWarehouses
			};
		}
	}
}