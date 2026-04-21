using System;
using System.Collections.Generic;


namespace Psz.Core.Identity.Models
{
	public class CRPAccessModel
	{
		public string AccessProfileName { get; set; }
		public bool Administration { get; set; }
		public bool Configuration { get; set; }
		public bool ConfigurationAppoitments { get; set; }
		public bool ConfigurationChangeEmployees { get; set; }
		public bool ConfigurationReplacements { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public bool DelforCreate { get; set; }
		public bool DelforDelete { get; set; }
		public bool DelforDeletePosition { get; set; }
		public bool DelforOrderConfirmation { get; set; }
		public bool DelforReport { get; set; }
		public bool DelforStatistics { get; set; }
		public bool DelforView { get; set; }
		public bool DLFPosHorizon1 { get; set; }
		public bool DLFPosHorizon2 { get; set; }
		public bool DLFPosHorizon3 { get; set; }
		public bool FaABEdit { get; set; }
		public bool FaABView { get; set; }
		public bool FaActionBook { get; set; }
		public bool FaActionComplete { get; set; }
		public bool FaActionDelete { get; set; }
		public bool FaActionPrint { get; set; }
		public bool FaAdmin { get; set; }
		public bool FAAKtualTerminUpdate { get; set; }
		public bool FaAnalysis { get; set; }
		public bool FAAuswertungEndkontrolle { get; set; }
		public bool FABemerkungPlannug { get; set; }
		public bool FABemerkungZuGewerk { get; set; }
		public bool FABemerkungZuPrio { get; set; }
		public bool FACancelHorizon1 { get; set; }
		public bool FACancelHorizon2 { get; set; }
		public bool FACancelHorizon3 { get; set; }
		public bool FACommissionert { get; set; }
		public bool FaCreate { get; set; }
		public bool FACreateHorizon1 { get; set; }
		public bool FACreateHorizon2 { get; set; }
		public bool FACreateHorizon3 { get; set; }
		public bool FaDatenEdit { get; set; }
		public bool FaDatenView { get; set; }
		public bool FaDelete { get; set; }
		public bool FADrucken { get; set; }
		public bool FaEdit { get; set; }
		public bool FAErlidegen { get; set; }
		public bool FAExcelUpdateWerk { get; set; }
		public bool FAExcelUpdateWunsh { get; set; }
		public bool FAFehlrMaterial { get; set; }
		public bool FaHomeAnalysis { get; set; }
		public bool FaHomeUpdate { get; set; }
		public bool FALaufkarteSchneiderei { get; set; }
		public bool FaPlanningEdit { get; set; }
		public bool FaPlanningView { get; set; }
		public bool FAPlannung { get; set; }
		public bool FAPlannungTechnick { get; set; }
		public bool FAPriesZeitUpdate { get; set; }
		public bool FAProductionPlannung { get; set; }
		public bool FAStappleDruck { get; set; }
		public bool FAStatusAlbania { get; set; }
		public bool FAStatusCzech { get; set; }
		public bool FAStatusTunisia { get; set; }
		public bool FAStorno { get; set; }
		public bool FAStucklist { get; set; }
		public bool FaTechnicEdit { get; set; }
		public bool FaTechnicView { get; set; }
		public bool FATerminWerk { get; set; }
		public bool FAUpdateBemerkungExtern { get; set; }
		public bool FAUpdateByArticle { get; set; }
		public bool FAUpdateByFA { get; set; }
		public bool FAUpdateTerminHorizon1 { get; set; }
		public bool FAUpdateTerminHorizon2 { get; set; }
		public bool FAUpdateTerminHorizon3 { get; set; }
		public bool FAWerkWunshAdmin { get; set; }
		public bool Fertigung { get; set; }
		public bool FertigungLog { get; set; }
		public int Id { get; set; }
		public bool isDefault { get; set; }
		public bool ModuleActivated { get; set; }
		public bool Statistics { get; set; }
		public bool StatsCapaHorizons { get; set; }
		public bool StatsCapaPlanning { get; set; }
		public bool StatsStockCS { get; set; }
		public bool StatsStockExternalWarehouse { get; set; }
		public bool StatsStockFG { get; set; }
		public bool UBGStatusChange { get; set; }
		public bool Forecast { get; set; }
		public bool ForecastCreate { get; set; }
		public bool ForecastDelete { get; set; }
		public bool ForecastStatistics { get; set; }
		public bool CRPFAPlannung { get; set; }
		public bool StatsCreatedInvoices { get; set; }
		public bool StatsBacklogFG { get; set; }
		public bool StatsDeliveries { get; set; }
		public bool StatsFAAnderungshistoire { get; set; }
		public bool StatsAnteileingelasteteFA { get; set; }
		public bool StatsLagerbestandFGCRP { get; set; }
		public bool SystemLogs { get; set; }
		public bool CRPPlanning { get; set; }
		public bool CRPUBGPlanning { get; set; }
		public bool CRPRequirement { get; set; }
		public bool FAPlannungHistory { get; set; }
		public bool FAPlannungHistoryXLSImport { get; set; }
		public bool FAPlannungHistoryXLSExport { get; set; }
		public bool FAPlannungHistoryForceAgent { get; set; }
		public bool StatsRahmenSale { get; set; }
		//-- crp dahboard
		public bool CRPDashboardCreatedOrders { get; set; }
		public bool CRPDashboardCancelledOrders { get; set; }
		public bool CRPDashboardActiveArticles { get; set; }
		public bool CRPDashboardOpenOrders { get; set; }
		public bool CRPDashboardOpenOrdersHours { get; set; }
		public bool CRPDashboardTotalStockFG { get; set; }
		//--FG Bestand History
		public bool ViewFGBestandHistory { get; set; }
		public bool ImportFGXlsHistory { get; set; }
		public bool ExportFGXlsHistory { get; set; }
		public bool LogsFGXlsHistory { get; set; }
		public bool AgentFGXlsHistory { get; set; }
		//-- souilmi 14-05-2025
		public bool FAUpdatePrio { get; set; }

		//-- CRP Statistics FA Changes
		public bool FaDateChangeHistory { get; set; }
		public bool FaHoursMovement { get; set; }
		public bool AllFaChanges { get; set; }
		public bool FaPlanningViolation { get; set; }

		// -- CRP Fa hours planning by customer ( from mgo production)
		public bool ProductionPlanningByCustomer { get; set; }
		public bool AllProductionWarehouses { get; set; }

		//--
		public bool FAUpdateCRP { get; set; }
		public CRPAccessModel()
		{

		}
		public CRPAccessModel(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;
			Administration = false;
			Configuration = false;
			ConfigurationAppoitments = false;
			ConfigurationChangeEmployees = false;
			ConfigurationReplacements = false;
			DelforCreate = false;
			DelforDelete = false;
			DelforDeletePosition = false;
			DelforOrderConfirmation = false;
			DelforReport = false;
			DelforStatistics = false;
			DelforView = false;
			DLFPosHorizon1 = false;
			DLFPosHorizon2 = false;
			DLFPosHorizon3 = false;
			FaABEdit = false;
			FaABView = false;
			FaActionBook = false;
			FaActionComplete = false;
			FaActionDelete = false;
			FaActionPrint = false;
			FaAdmin = false;
			FAAKtualTerminUpdate = false;
			FaAnalysis = false;
			FAAuswertungEndkontrolle = false;
			FABemerkungPlannug = false;
			FABemerkungZuGewerk = false;
			FABemerkungZuPrio = false;
			FACancelHorizon1 = false;
			FACancelHorizon2 = false;
			FACancelHorizon3 = false;
			FACommissionert = false;
			FaCreate = false;
			FACreateHorizon1 = false;
			FACreateHorizon2 = false;
			FACreateHorizon3 = false;
			FaDatenEdit = false;
			FaDatenView = false;
			FaDelete = false;
			FADrucken = false;
			FaEdit = false;
			FAErlidegen = false;
			FAExcelUpdateWerk = false;
			FAExcelUpdateWunsh = false;
			FAFehlrMaterial = false;
			FaHomeAnalysis = false;
			FaHomeUpdate = false;
			FALaufkarteSchneiderei = false;
			FaPlanningEdit = false;
			FaPlanningView = false;
			FAPlannung = false;
			FAPlannungTechnick = false;
			FAPriesZeitUpdate = false;
			FAProductionPlannung = false;
			FAStappleDruck = false;
			FAStatusAlbania = false;
			FAStatusCzech = false;
			FAStatusTunisia = false;
			FAStorno = false;
			FAStucklist = false;
			FaTechnicEdit = false;
			FaTechnicView = false;
			FATerminWerk = false;
			FAUpdateBemerkungExtern = false;
			FAUpdateByArticle = false;
			FAUpdateByFA = false;
			FAUpdateTerminHorizon1 = false;
			FAUpdateTerminHorizon2 = false;
			FAUpdateTerminHorizon3 = false;
			FAWerkWunshAdmin = false;
			Fertigung = false;
			FertigungLog = false;
			isDefault = false;
			ModuleActivated = false;
			Statistics = false;
			StatsCapaHorizons = false;
			StatsCapaPlanning = false;
			StatsStockCS = false;
			StatsStockExternalWarehouse = false;
			StatsStockFG = false;
			UBGStatusChange = false;
			Forecast = false;
			ForecastCreate = false;
			ForecastDelete = false;
			ForecastStatistics = false;
			CRPFAPlannung = false;
			//
			StatsCreatedInvoices = false;
			StatsBacklogFG = false;
			StatsDeliveries = false;
			StatsFAAnderungshistoire = false;
			StatsAnteileingelasteteFA = false;
			StatsLagerbestandFGCRP = false;
			SystemLogs = false;
			CRPPlanning = false;
			CRPUBGPlanning = false;
			CRPRequirement = false;
			//-
			FAPlannungHistory = false;
			FAPlannungHistoryXLSImport = false;
			FAPlannungHistoryXLSExport = false;
			FAPlannungHistoryForceAgent = false;

			//-
			StatsRahmenSale = false;
			//-- crp dashboard
			CRPDashboardCreatedOrders = false;
			CRPDashboardCancelledOrders = false;
			CRPDashboardActiveArticles = false;
			CRPDashboardOpenOrders = false;
			CRPDashboardOpenOrdersHours = false;
			CRPDashboardTotalStockFG = false;
			//--FG Bestand History
			ViewFGBestandHistory = false;
			ImportFGXlsHistory = false;
			ExportFGXlsHistory = false;
			LogsFGXlsHistory = false;
			AgentFGXlsHistory = false;
			//-- souilmi 14-05-2025
			FAUpdatePrio = false;

			//-- CRP Statistics FA Changes
			AllFaChanges = false;
			FaHoursMovement = false;
			FaPlanningViolation = false;
			FaDateChangeHistory = false;
			ProductionPlanningByCustomer = false;
			AllProductionWarehouses = false;
			foreach(var accessItem in accessProfileEntities)
			{
				Administration = Administration || (accessItem?.Administration ?? false);
				Configuration = Configuration || (accessItem?.Configuration ?? false);
				ConfigurationAppoitments = ConfigurationAppoitments || (accessItem?.ConfigurationAppoitments ?? false);
				ConfigurationChangeEmployees = ConfigurationChangeEmployees || (accessItem?.ConfigurationChangeEmployees ?? false);
				ConfigurationReplacements = ConfigurationReplacements || (accessItem?.ConfigurationReplacements ?? false);
				DelforCreate = DelforCreate || (accessItem?.DelforCreate ?? false);
				DelforDelete = DelforDelete || (accessItem?.DelforDelete ?? false);
				DelforDeletePosition = DelforDeletePosition || (accessItem?.DelforDeletePosition ?? false);
				DelforOrderConfirmation = DelforOrderConfirmation || (accessItem?.DelforOrderConfirmation ?? false);
				DelforReport = DelforReport || (accessItem?.DelforReport ?? false);
				DelforStatistics = DelforStatistics || (accessItem?.DelforStatistics ?? false);
				DelforView = DelforView || (accessItem?.DelforView ?? false);
				DLFPosHorizon1 = DLFPosHorizon1 || (accessItem?.DLFPosHorizon1 ?? false);
				DLFPosHorizon2 = DLFPosHorizon2 || (accessItem?.DLFPosHorizon2 ?? false);
				DLFPosHorizon3 = DLFPosHorizon3 || (accessItem?.DLFPosHorizon3 ?? false);
				FaABEdit = FaABEdit || (accessItem?.FaABEdit ?? false);
				FaABView = FaABView || (accessItem?.FaABView ?? false);
				FaActionBook = FaActionBook || (accessItem?.FaActionBook ?? false);
				FaActionComplete = FaActionComplete || (accessItem?.FaActionComplete ?? false);
				FaActionDelete = FaActionDelete || (accessItem?.FaActionDelete ?? false);
				FaActionPrint = FaActionPrint || (accessItem?.FaActionPrint ?? false);
				FaAdmin = FaAdmin || (accessItem?.FaAdmin ?? false);
				FAAKtualTerminUpdate = FAAKtualTerminUpdate || (accessItem?.FAAKtualTerminUpdate ?? false);
				FaAnalysis = FaAnalysis || (accessItem?.FaAnalysis ?? false);
				FAAuswertungEndkontrolle = FAAuswertungEndkontrolle || (accessItem?.FAAuswertungEndkontrolle ?? false);
				FABemerkungPlannug = FABemerkungPlannug || (accessItem?.FABemerkungPlannug ?? false);
				FABemerkungZuGewerk = FABemerkungZuGewerk || (accessItem?.FABemerkungZuGewerk ?? false);
				FABemerkungZuPrio = FABemerkungZuPrio || (accessItem?.FABemerkungZuPrio ?? false);
				FACancelHorizon1 = FACancelHorizon1 || (accessItem?.FACancelHorizon1 ?? false);
				FACancelHorizon2 = FACancelHorizon2 || (accessItem?.FACancelHorizon2 ?? false);
				FACancelHorizon3 = FACancelHorizon3 || (accessItem?.FACancelHorizon3 ?? false);
				FACommissionert = FACommissionert || (accessItem?.FACommissionert ?? false);
				FaCreate = FaCreate || (accessItem?.FaCreate ?? false);
				FACreateHorizon1 = FACreateHorizon1 || (accessItem?.FACreateHorizon1 ?? false);
				FACreateHorizon2 = FACreateHorizon2 || (accessItem?.FACreateHorizon2 ?? false);
				FACreateHorizon3 = FACreateHorizon3 || (accessItem?.FACreateHorizon3 ?? false);
				FaDatenEdit = FaDatenEdit || (accessItem?.FaDatenEdit ?? false);
				FaDatenView = FaDatenView || (accessItem?.FaDatenView ?? false);
				FaDelete = FaDelete || (accessItem?.FaDelete ?? false);
				FADrucken = FADrucken || (accessItem?.FADrucken ?? false);
				FaEdit = FaEdit || (accessItem?.FaEdit ?? false);
				FAErlidegen = FAErlidegen || (accessItem?.FAErlidegen ?? false);
				FAExcelUpdateWerk = FAExcelUpdateWerk || (accessItem?.FAExcelUpdateWerk ?? false);
				FAExcelUpdateWunsh = FAExcelUpdateWunsh || (accessItem?.FAExcelUpdateWunsh ?? false);
				FAFehlrMaterial = FAFehlrMaterial || (accessItem?.FAFehlrMaterial ?? false);
				FaHomeAnalysis = FaHomeAnalysis || (accessItem?.FaHomeAnalysis ?? false);
				FaHomeUpdate = FaHomeUpdate || (accessItem?.FaHomeUpdate ?? false);
				FALaufkarteSchneiderei = FALaufkarteSchneiderei || (accessItem?.FALaufkarteSchneiderei ?? false);
				FaPlanningEdit = FaPlanningEdit || (accessItem?.FaPlanningEdit ?? false);
				FaPlanningView = FaPlanningView || (accessItem?.FaPlanningView ?? false);
				FAPlannung = FAPlannung || (accessItem?.FAPlannung ?? false);
				FAPlannungTechnick = FAPlannungTechnick || (accessItem?.FAPlannungTechnick ?? false);
				FAPriesZeitUpdate = FAPriesZeitUpdate || (accessItem?.FAPriesZeitUpdate ?? false);
				FAProductionPlannung = FAProductionPlannung || (accessItem?.FAProductionPlannung ?? false);
				FAStappleDruck = FAStappleDruck || (accessItem?.FAStappleDruck ?? false);
				FAStatusAlbania = FAStatusAlbania || (accessItem?.FAStatusAlbania ?? false);
				FAStatusCzech = FAStatusCzech || (accessItem?.FAStatusCzech ?? false);
				FAStatusTunisia = FAStatusTunisia || (accessItem?.FAStatusTunisia ?? false);
				FAStorno = FAStorno || (accessItem?.FAStorno ?? false);
				FAStucklist = FAStucklist || (accessItem?.FAStucklist ?? false);
				FaTechnicEdit = FaTechnicEdit || (accessItem?.FaTechnicEdit ?? false);
				FaTechnicView = FaTechnicView || (accessItem?.FaTechnicView ?? false);
				FATerminWerk = FATerminWerk || (accessItem?.FATerminWerk ?? false);
				FAUpdateBemerkungExtern = FAUpdateBemerkungExtern || (accessItem?.FAUpdateBemerkungExtern ?? false);
				FAUpdateByArticle = FAUpdateByArticle || (accessItem?.FAUpdateByArticle ?? false);
				FAUpdateByFA = FAUpdateByFA || (accessItem?.FAUpdateByFA ?? false);
				FAUpdateTerminHorizon1 = FAUpdateTerminHorizon1 || (accessItem?.FAUpdateTerminHorizon1 ?? false);
				FAUpdateTerminHorizon2 = FAUpdateTerminHorizon2 || (accessItem?.FAUpdateTerminHorizon2 ?? false);
				FAUpdateTerminHorizon3 = FAUpdateTerminHorizon3 || (accessItem?.FAUpdateTerminHorizon3 ?? false);
				FAWerkWunshAdmin = FAWerkWunshAdmin || (accessItem?.FAWerkWunshAdmin ?? false);
				Fertigung = Fertigung || (accessItem?.Fertigung ?? false);
				FertigungLog = FertigungLog || (accessItem?.FertigungLog ?? false);
				isDefault = isDefault || (accessItem?.isDefault ?? false);
				ModuleActivated = ModuleActivated || (accessItem?.ModuleActivated ?? false);
				Statistics = Statistics || (accessItem?.Statistics ?? false);
				StatsCapaHorizons = StatsCapaHorizons || (accessItem?.StatsCapaHorizons ?? false);
				StatsCapaPlanning = StatsCapaPlanning || (accessItem?.StatsCapaPlanning ?? false);
				StatsStockCS = StatsStockCS || (accessItem?.StatsStockCS ?? false);
				StatsStockExternalWarehouse = StatsStockExternalWarehouse || (accessItem?.StatsStockExternalWarehouse ?? false);
				StatsStockFG = StatsStockFG || (accessItem?.StatsStockFG ?? false);
				UBGStatusChange = UBGStatusChange || (accessItem?.UBGStatusChange ?? false);
				Forecast = Forecast || (accessItem?.Forecast ?? false);
				ForecastCreate = ForecastCreate || (accessItem?.ForecastCreate ?? false);
				ForecastDelete = ForecastDelete || (accessItem?.ForecastDelete ?? false);
				ForecastStatistics = ForecastStatistics || (accessItem?.ForecastStatistics ?? false);
				CRPFAPlannung = CRPFAPlannung || (accessItem?.CRPFAPlannung ?? false);
				//
				StatsCreatedInvoices = StatsCreatedInvoices || (accessItem?.StatsCreatedInvoices ?? false);
				StatsBacklogFG = StatsBacklogFG || (accessItem?.StatsBacklogFG ?? false);
				StatsDeliveries = StatsDeliveries || (accessItem?.StatsDeliveries ?? false);
				StatsFAAnderungshistoire = StatsFAAnderungshistoire || (accessItem?.StatsFAAnderungshistoire ?? false);
				StatsAnteileingelasteteFA = StatsAnteileingelasteteFA || (accessItem?.StatsAnteileingelasteteFA ?? false);
				StatsLagerbestandFGCRP = StatsLagerbestandFGCRP || (accessItem?.StatsLagerbestandFGCRP ?? false);
				SystemLogs = SystemLogs || (accessItem?.SystemLogs ?? false);
				CRPPlanning = CRPPlanning || (accessItem?.CRPPlanning ?? false);
				CRPUBGPlanning = CRPUBGPlanning || (accessItem?.CRPUBGPlanning ?? false);
				CRPRequirement = CRPRequirement || (accessItem?.CRPRequirement ?? false);
				//-
				FAPlannungHistory = FAPlannungHistory || (accessItem?.FAPlannungHistory ?? false);
				FAPlannungHistoryXLSImport = FAPlannungHistoryXLSImport || (accessItem?.FAPlannungHistoryXLSImport ?? false);
				FAPlannungHistoryXLSExport = FAPlannungHistoryXLSExport || (accessItem?.FAPlannungHistoryXLSExport ?? false);
				FAPlannungHistoryForceAgent = FAPlannungHistoryForceAgent || (accessItem?.FAPlannungHistoryForceAgent ?? false);
				//-
				StatsRahmenSale = StatsRahmenSale || (accessItem?.StatsRahmenSale ?? false);
				//-- crp dashboard
				CRPDashboardCreatedOrders = CRPDashboardCreatedOrders || (accessItem?.CRPDashboardCreatedOrders ?? false);
				CRPDashboardCancelledOrders = CRPDashboardCancelledOrders || (accessItem?.CRPDashboardCancelledOrders ?? false);
				CRPDashboardActiveArticles = CRPDashboardActiveArticles || (accessItem?.CRPDashboardActiveArticles ?? false);
				CRPDashboardOpenOrders = CRPDashboardOpenOrders || (accessItem?.CRPDashboardOpenOrders ?? false);
				CRPDashboardOpenOrdersHours = CRPDashboardOpenOrdersHours || (accessItem?.CRPDashboardOpenOrdersHours ?? false);
				CRPDashboardTotalStockFG = CRPDashboardTotalStockFG || (accessItem?.CRPDashboardTotalStockFG ?? false);
				//--FG Bestand History
				ViewFGBestandHistory = ViewFGBestandHistory || (accessItem?.ViewFGBestandHistory ?? false);
				ImportFGXlsHistory = ImportFGXlsHistory || (accessItem?.ImportFGXlsHistory ?? false);
				ExportFGXlsHistory = ExportFGXlsHistory || (accessItem?.ExportFGXlsHistory ?? false);
				LogsFGXlsHistory = LogsFGXlsHistory || (accessItem?.LogsFGXlsHistory ?? false);
				AgentFGXlsHistory = AgentFGXlsHistory || (accessItem?.AgentFGXlsHistory ?? false);
				//-- souilmi 14-05-2025
				FAUpdatePrio = FAUpdatePrio || (accessItem?.FAUpdatePrio ?? false);

				//-- CRP Statistics FA Changes
				AllFaChanges = AllFaChanges || (accessItem?.AllFaChanges ?? false);
				FaHoursMovement = FaHoursMovement || (accessItem?.FaHoursMovement ?? false);
				FaPlanningViolation = FaPlanningViolation || (accessItem?.FaPlanningViolation ?? false);
				FaDateChangeHistory = FaDateChangeHistory || (accessItem?.FaDateChangeHistory ?? false);
				ProductionPlanningByCustomer = ProductionPlanningByCustomer || (accessItem?.ProductionPlanningByCustomer ?? false) ;
				AllProductionWarehouses = AllProductionWarehouses || (accessItem?.AllProductionWarehouses ?? false);
				//--
				FAUpdateCRP = FAUpdateCRP || (accessItem?.FAUpdateCRP ?? false);
			}
		}
		public CRPAccessModel(CRPAccessModel entity)
		{
			if(entity is null)
			{
				return;
			}
			Id = entity.Id;
			Administration = entity.Administration;
			Configuration = entity.Configuration;
			ConfigurationAppoitments = entity.ConfigurationAppoitments;
			ConfigurationChangeEmployees = entity.ConfigurationChangeEmployees;
			ConfigurationReplacements = entity.ConfigurationReplacements;
			DelforCreate = entity.DelforCreate;
			DelforDelete = entity.DelforDelete;
			DelforDeletePosition = entity.DelforDeletePosition;
			DelforOrderConfirmation = entity.DelforOrderConfirmation;
			DelforReport = entity.DelforReport;
			DelforStatistics = entity.DelforStatistics;
			DelforView = entity.DelforView;
			DLFPosHorizon1 = entity.DLFPosHorizon1;
			DLFPosHorizon2 = entity.DLFPosHorizon2;
			DLFPosHorizon3 = entity.DLFPosHorizon3;
			FaABEdit = entity.FaABEdit;
			FaABView = entity.FaABView;
			FaActionBook = entity.FaActionBook;
			FaActionComplete = entity.FaActionComplete;
			FaActionDelete = entity.FaActionDelete;
			FaActionPrint = entity.FaActionPrint;
			FaAdmin = entity.FaAdmin;
			FAAKtualTerminUpdate = entity.FAAKtualTerminUpdate;
			FaAnalysis = entity.FaAnalysis;
			FAAuswertungEndkontrolle = entity.FAAuswertungEndkontrolle;
			FABemerkungPlannug = entity.FABemerkungPlannug;
			FABemerkungZuGewerk = entity.FABemerkungZuGewerk;
			FABemerkungZuPrio = entity.FABemerkungZuPrio;
			FACancelHorizon1 = entity.FACancelHorizon1;
			FACancelHorizon2 = entity.FACancelHorizon2;
			FACancelHorizon3 = entity.FACancelHorizon3;
			FACommissionert = entity.FACommissionert;
			FaCreate = entity.FaCreate;
			FACreateHorizon1 = entity.FACreateHorizon1;
			FACreateHorizon2 = entity.FACreateHorizon2;
			FACreateHorizon3 = entity.FACreateHorizon3;
			FaDatenEdit = entity.FaDatenEdit;
			FaDatenView = entity.FaDatenView;
			FaDelete = entity.FaDelete;
			FADrucken = entity.FADrucken;
			FaEdit = entity.FaEdit;
			FAErlidegen = entity.FAErlidegen;
			FAExcelUpdateWerk = entity.FAExcelUpdateWerk;
			FAExcelUpdateWunsh = entity.FAExcelUpdateWunsh;
			FAFehlrMaterial = entity.FAFehlrMaterial;
			FaHomeAnalysis = entity.FaHomeAnalysis;
			FaHomeUpdate = entity.FaHomeUpdate;
			FALaufkarteSchneiderei = entity.FALaufkarteSchneiderei;
			FaPlanningEdit = entity.FaPlanningEdit;
			FaPlanningView = entity.FaPlanningView;
			FAPlannung = entity.FAPlannung;
			FAPlannungTechnick = entity.FAPlannungTechnick;
			FAPriesZeitUpdate = entity.FAPriesZeitUpdate;
			FAProductionPlannung = entity.FAProductionPlannung;
			FAStappleDruck = entity.FAStappleDruck;
			FAStatusAlbania = entity.FAStatusAlbania;
			FAStatusCzech = entity.FAStatusCzech;
			FAStatusTunisia = entity.FAStatusTunisia;
			FAStorno = entity.FAStorno;
			FAStucklist = entity.FAStucklist;
			FaTechnicEdit = entity.FaTechnicEdit;
			FaTechnicView = entity.FaTechnicView;
			FATerminWerk = entity.FATerminWerk;
			FAUpdateBemerkungExtern = entity.FAUpdateBemerkungExtern;
			FAUpdateByArticle = entity.FAUpdateByArticle;
			FAUpdateByFA = entity.FAUpdateByFA;
			FAUpdateTerminHorizon1 = entity.FAUpdateTerminHorizon1;
			FAUpdateTerminHorizon2 = entity.FAUpdateTerminHorizon2;
			FAUpdateTerminHorizon3 = entity.FAUpdateTerminHorizon3;
			FAWerkWunshAdmin = entity.FAWerkWunshAdmin;
			Fertigung = entity.Fertigung;
			FertigungLog = entity.FertigungLog;
			isDefault = entity.isDefault;
			ModuleActivated = entity.ModuleActivated;
			Statistics = entity.Statistics;
			StatsCapaHorizons = entity.StatsCapaHorizons;
			StatsCapaPlanning = entity.StatsCapaPlanning;
			StatsStockCS = entity.StatsStockCS;
			StatsStockExternalWarehouse = entity.StatsStockExternalWarehouse;
			StatsStockFG = entity.StatsStockFG;
			UBGStatusChange = entity.UBGStatusChange;
			Forecast = entity.Forecast;
			ForecastCreate = entity.ForecastCreate;
			ForecastDelete = entity.ForecastDelete;
			ForecastStatistics = entity.ForecastStatistics;
			CRPFAPlannung = entity.CRPFAPlannung;
			//
			StatsCreatedInvoices = entity.StatsCreatedInvoices;
			StatsBacklogFG = entity.StatsBacklogFG;
			StatsDeliveries = entity.StatsDeliveries;
			StatsFAAnderungshistoire = entity.StatsFAAnderungshistoire;
			StatsAnteileingelasteteFA = entity.StatsAnteileingelasteteFA;
			StatsLagerbestandFGCRP = entity.StatsLagerbestandFGCRP;
			SystemLogs = entity.SystemLogs;
			CRPPlanning = entity.CRPPlanning;
			CRPUBGPlanning = entity.CRPUBGPlanning;
			CRPRequirement = entity.CRPRequirement;
			//-
			FAPlannungHistory = FAPlannungHistory;
			FAPlannungHistoryXLSImport = FAPlannungHistoryXLSImport;
			FAPlannungHistoryXLSExport = FAPlannungHistoryXLSExport;
			FAPlannungHistoryForceAgent = FAPlannungHistoryForceAgent;
			//-
			StatsRahmenSale = entity.StatsRahmenSale;
			//-- crp dashboard
			CRPDashboardCreatedOrders = entity.CRPDashboardCreatedOrders;
			CRPDashboardCancelledOrders = entity.CRPDashboardCancelledOrders;
			CRPDashboardActiveArticles = entity.CRPDashboardActiveArticles;
			CRPDashboardOpenOrders = entity.CRPDashboardOpenOrders;
			CRPDashboardOpenOrdersHours = entity.CRPDashboardOpenOrdersHours;
			CRPDashboardTotalStockFG = entity.CRPDashboardTotalStockFG;
			//--FG Bestand History
			ViewFGBestandHistory = entity.ViewFGBestandHistory;
			ImportFGXlsHistory = entity.ImportFGXlsHistory;
			ExportFGXlsHistory = entity.ExportFGXlsHistory;
			LogsFGXlsHistory = entity.LogsFGXlsHistory;
			AgentFGXlsHistory = entity.AgentFGXlsHistory;
			//-- souilmi 14-05-2025
			FAUpdatePrio = entity.FAUpdatePrio;

			//-- CRP Statistics FA Changes
			AllFaChanges = entity.AllFaChanges;
			FaHoursMovement = entity.FaHoursMovement;
			FaPlanningViolation = entity.FaPlanningViolation;
			FaDateChangeHistory = entity.FaDateChangeHistory;
			ProductionPlanningByCustomer = entity.ProductionPlanningByCustomer;
			AllProductionWarehouses = entity.AllProductionWarehouses;
			//--
			FAUpdateCRP = entity.FAUpdateCRP;
		}
		public Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity ToDbEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity
			{
				Id = Id,
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
				//-- CRP Statistics FA Changes
				AllFaChanges = AllFaChanges,
				FaHoursMovement = FaHoursMovement,
				FaPlanningViolation = FaPlanningViolation,
				FaDateChangeHistory = FaDateChangeHistory,
				ProductionPlanningByCustomer = ProductionPlanningByCustomer,
				AllProductionWarehouses = AllProductionWarehouses,
				//--
				FAUpdateCRP = FAUpdateCRP
			};
		}
	}
}
