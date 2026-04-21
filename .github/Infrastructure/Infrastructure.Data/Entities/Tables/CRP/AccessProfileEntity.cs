using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
    public class AccessProfileEntity
    {
		public string AccessProfileName { get; set; }
		public bool? Administration { get; set; }
		public bool? AgentFGXlsHistory { get; set; }
		public bool? AllFaChanges { get; set; }
		public bool? AllProductionWarehouses { get; set; }
		public bool? Configuration { get; set; }
		public bool? ConfigurationAppoitments { get; set; }
		public bool? ConfigurationChangeEmployees { get; set; }
		public bool? ConfigurationReplacements { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public bool? CRPDashboardActiveArticles { get; set; }
		public bool? CRPDashboardCancelledOrders { get; set; }
		public bool? CRPDashboardCreatedOrders { get; set; }
		public bool? CRPDashboardOpenOrders { get; set; }
		public bool? CRPDashboardOpenOrdersHours { get; set; }
		public bool? CRPDashboardTotalStockFG { get; set; }
		public bool? CRPFAPlannung { get; set; }
		public bool? CRPPlanning { get; set; }
		public bool? CRPRequirement { get; set; }
		public bool? CRPUBGPlanning { get; set; }
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
		public bool? ExportFGXlsHistory { get; set; }
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
		public bool? FaDateChangeHistory { get; set; }
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
		public bool? FaHoursMovement { get; set; }
		public bool? FALaufkarteSchneiderei { get; set; }
		public bool? FaPlanningEdit { get; set; }
		public bool? FaPlanningView { get; set; }
		public bool? FaPlanningViolation { get; set; }
		public bool? FAPlannung { get; set; }
		public bool? FAPlannungHistory { get; set; }
		public bool? FAPlannungHistoryForceAgent { get; set; }
		public bool? FAPlannungHistoryXLSExport { get; set; }
		public bool? FAPlannungHistoryXLSImport { get; set; }
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
		public bool? FAUpdatePrio { get; set; }
		public bool? FAUpdateTerminHorizon1 { get; set; }
		public bool? FAUpdateTerminHorizon2 { get; set; }
		public bool? FAUpdateTerminHorizon3 { get; set; }
		public bool? FAWerkWunshAdmin { get; set; }
		public bool? Fertigung { get; set; }
		public bool? FertigungLog { get; set; }
		public bool? Forecast { get; set; }
		public bool? ForecastCreate { get; set; }
		public bool? ForecastDelete { get; set; }
		public bool? ForecastStatistics { get; set; }
		public int Id { get; set; }
		public bool? ImportFGXlsHistory { get; set; }
		public bool? isDefault { get; set; }
		public bool? LogsFGXlsHistory { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? ProductionPlanningByCustomer { get; set; }
		public bool? Statistics { get; set; }
		public bool? StatsAnteileingelasteteFA { get; set; }
		public bool? StatsBacklogFG { get; set; }
		public bool? StatsCapaHorizons { get; set; }
		public bool? StatsCapaPlanning { get; set; }
		public bool? StatsCreatedInvoices { get; set; }
		public bool? StatsDeliveries { get; set; }
		public bool? StatsFAAnderungshistoire { get; set; }
		public bool? StatsLagerbestandFGCRP { get; set; }
		public bool? StatsRahmenSale { get; set; }
		public bool? StatsStockCS { get; set; }
		public bool? StatsStockExternalWarehouse { get; set; }
		public bool? StatsStockFG { get; set; }
		public bool? SystemLogs { get; set; }
		public bool? UBGStatusChange { get; set; }
		public bool? ViewFGBestandHistory { get; set; }

		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
			Administration = (dataRow["Administration"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Administration"]);
			AgentFGXlsHistory = (dataRow["AgentFGXlsHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AgentFGXlsHistory"]);
			AllFaChanges = (dataRow["AllFaChanges"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AllFaChanges"]);
			AllProductionWarehouses = (dataRow["AllProductionWarehouses"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AllProductionWarehouses"]);
			Configuration = (dataRow["Configuration"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Configuration"]);
			ConfigurationAppoitments = (dataRow["ConfigurationAppoitments"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigurationAppoitments"]);
			ConfigurationChangeEmployees = (dataRow["ConfigurationChangeEmployees"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigurationChangeEmployees"]);
			ConfigurationReplacements = (dataRow["ConfigurationReplacements"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigurationReplacements"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CRPDashboardActiveArticles = (dataRow["CRPDashboardActiveArticles"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPDashboardActiveArticles"]);
			CRPDashboardCancelledOrders = (dataRow["CRPDashboardCancelledOrders"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPDashboardCancelledOrders"]);
			CRPDashboardCreatedOrders = (dataRow["CRPDashboardCreatedOrders"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPDashboardCreatedOrders"]);
			CRPDashboardOpenOrders = (dataRow["CRPDashboardOpenOrders"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPDashboardOpenOrders"]);
			CRPDashboardOpenOrdersHours = (dataRow["CRPDashboardOpenOrdersHours"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPDashboardOpenOrdersHours"]);
			CRPDashboardTotalStockFG = (dataRow["CRPDashboardTotalStockFG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPDashboardTotalStockFG"]);
			CRPFAPlannung = (dataRow["CRPFAPlannung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPFAPlannung"]);
			CRPPlanning = (dataRow["CRPPlanning"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPPlanning"]);
			CRPRequirement = (dataRow["CRPRequirement"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPRequirement"]);
			CRPUBGPlanning = (dataRow["CRPUBGPlanning"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CRPUBGPlanning"]);
			DelforCreate = (dataRow["DelforCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforCreate"]);
			DelforDelete = (dataRow["DelforDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforDelete"]);
			DelforDeletePosition = (dataRow["DelforDeletePosition"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforDeletePosition"]);
			DelforOrderConfirmation = (dataRow["DelforOrderConfirmation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforOrderConfirmation"]);
			DelforReport = (dataRow["DelforReport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforReport"]);
			DelforStatistics = (dataRow["DelforStatistics"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforStatistics"]);
			DelforView = (dataRow["DelforView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforView"]);
			DLFPosHorizon1 = (dataRow["DLFPosHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DLFPosHorizon1"]);
			DLFPosHorizon2 = (dataRow["DLFPosHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DLFPosHorizon2"]);
			DLFPosHorizon3 = (dataRow["DLFPosHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DLFPosHorizon3"]);
			ExportFGXlsHistory = (dataRow["ExportFGXlsHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ExportFGXlsHistory"]);
			FaABEdit = (dataRow["FaABEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaABEdit"]);
			FaABView = (dataRow["FaABView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaABView"]);
			FaActionBook = (dataRow["FaActionBook"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaActionBook"]);
			FaActionComplete = (dataRow["FaActionComplete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaActionComplete"]);
			FaActionDelete = (dataRow["FaActionDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaActionDelete"]);
			FaActionPrint = (dataRow["FaActionPrint"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaActionPrint"]);
			FaAdmin = (dataRow["FaAdmin"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaAdmin"]);
			FAAKtualTerminUpdate = (dataRow["FAAKtualTerminUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAAKtualTerminUpdate"]);
			FaAnalysis = (dataRow["FaAnalysis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaAnalysis"]);
			FAAuswertungEndkontrolle = (dataRow["FAAuswertungEndkontrolle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAAuswertungEndkontrolle"]);
			FABemerkungPlannug = (dataRow["FABemerkungPlannug"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FABemerkungPlannug"]);
			FABemerkungZuGewerk = (dataRow["FABemerkungZuGewerk"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FABemerkungZuGewerk"]);
			FABemerkungZuPrio = (dataRow["FABemerkungZuPrio"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FABemerkungZuPrio"]);
			FACancelHorizon1 = (dataRow["FACancelHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FACancelHorizon1"]);
			FACancelHorizon2 = (dataRow["FACancelHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FACancelHorizon2"]);
			FACancelHorizon3 = (dataRow["FACancelHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FACancelHorizon3"]);
			FACommissionert = (dataRow["FACommissionert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FACommissionert"]);
			FaCreate = (dataRow["FaCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaCreate"]);
			FACreateHorizon1 = (dataRow["FACreateHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FACreateHorizon1"]);
			FACreateHorizon2 = (dataRow["FACreateHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FACreateHorizon2"]);
			FACreateHorizon3 = (dataRow["FACreateHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FACreateHorizon3"]);
			FaDateChangeHistory = (dataRow["FaDateChangeHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaDateChangeHistory"]);
			FaDatenEdit = (dataRow["FaDatenEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaDatenEdit"]);
			FaDatenView = (dataRow["FaDatenView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaDatenView"]);
			FaDelete = (dataRow["FaDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaDelete"]);
			FADrucken = (dataRow["FADrucken"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FADrucken"]);
			FaEdit = (dataRow["FaEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaEdit"]);
			FAErlidegen = (dataRow["FAErlidegen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAErlidegen"]);
			FAExcelUpdateWerk = (dataRow["FAExcelUpdateWerk"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAExcelUpdateWerk"]);
			FAExcelUpdateWunsh = (dataRow["FAExcelUpdateWunsh"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAExcelUpdateWunsh"]);
			FAFehlrMaterial = (dataRow["FAFehlrMaterial"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAFehlrMaterial"]);
			FaHomeAnalysis = (dataRow["FaHomeAnalysis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaHomeAnalysis"]);
			FaHomeUpdate = (dataRow["FaHomeUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaHomeUpdate"]);
			FaHoursMovement = (dataRow["FaHoursMovement"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaHoursMovement"]);
			FALaufkarteSchneiderei = (dataRow["FALaufkarteSchneiderei"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FALaufkarteSchneiderei"]);
			FaPlanningEdit = (dataRow["FaPlanningEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaPlanningEdit"]);
			FaPlanningView = (dataRow["FaPlanningView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaPlanningView"]);
			FaPlanningViolation = (dataRow["FaPlanningViolation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaPlanningViolation"]);
			FAPlannung = (dataRow["FAPlannung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPlannung"]);
			FAPlannungHistory = (dataRow["FAPlannungHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPlannungHistory"]);
			FAPlannungHistoryForceAgent = (dataRow["FAPlannungHistoryForceAgent"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPlannungHistoryForceAgent"]);
			FAPlannungHistoryXLSExport = (dataRow["FAPlannungHistoryXLSExport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPlannungHistoryXLSExport"]);
			FAPlannungHistoryXLSImport = (dataRow["FAPlannungHistoryXLSImport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPlannungHistoryXLSImport"]);
			FAPlannungTechnick = (dataRow["FAPlannungTechnick"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPlannungTechnick"]);
			FAPriesZeitUpdate = (dataRow["FAPriesZeitUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPriesZeitUpdate"]);
			FAProductionPlannung = (dataRow["FAProductionPlannung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAProductionPlannung"]);
			FAStappleDruck = (dataRow["FAStappleDruck"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAStappleDruck"]);
			FAStatusAlbania = (dataRow["FAStatusAlbania"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAStatusAlbania"]);
			FAStatusCzech = (dataRow["FAStatusCzech"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAStatusCzech"]);
			FAStatusTunisia = (dataRow["FAStatusTunisia"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAStatusTunisia"]);
			FAStorno = (dataRow["FAStorno"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAStorno"]);
			FAStucklist = (dataRow["FAStucklist"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAStucklist"]);
			FaTechnicEdit = (dataRow["FaTechnicEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaTechnicEdit"]);
			FaTechnicView = (dataRow["FaTechnicView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaTechnicView"]);
			FATerminWerk = (dataRow["FATerminWerk"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FATerminWerk"]);
			FAUpdateBemerkungExtern = (dataRow["FAUpdateBemerkungExtern"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateBemerkungExtern"]);
			FAUpdateByArticle = (dataRow["FAUpdateByArticle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateByArticle"]);
			FAUpdateByFA = (dataRow["FAUpdateByFA"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateByFA"]);
			FAUpdatePrio = (dataRow["FAUpdatePrio"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdatePrio"]);
			FAUpdateTerminHorizon1 = (dataRow["FAUpdateTerminHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateTerminHorizon1"]);
			FAUpdateTerminHorizon2 = (dataRow["FAUpdateTerminHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateTerminHorizon2"]);
			FAUpdateTerminHorizon3 = (dataRow["FAUpdateTerminHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateTerminHorizon3"]);
			FAWerkWunshAdmin = (dataRow["FAWerkWunshAdmin"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAWerkWunshAdmin"]);
			Fertigung = (dataRow["Fertigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Fertigung"]);
			FertigungLog = (dataRow["FertigungLog"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FertigungLog"]);
			Forecast = (dataRow["Forecast"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Forecast"]);
			ForecastCreate = (dataRow["ForecastCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForecastCreate"]);
			ForecastDelete = (dataRow["ForecastDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForecastDelete"]);
			ForecastStatistics = (dataRow["ForecastStatistics"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForecastStatistics"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ImportFGXlsHistory = (dataRow["ImportFGXlsHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ImportFGXlsHistory"]);
			isDefault = (dataRow["isDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["isDefault"]);
			LogsFGXlsHistory = (dataRow["LogsFGXlsHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LogsFGXlsHistory"]);
			ModuleActivated = (dataRow["ModuleActivated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ModuleActivated"]);
			ProductionPlanningByCustomer = (dataRow["ProductionPlanningByCustomer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProductionPlanningByCustomer"]);
			Statistics = (dataRow["Statistics"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Statistics"]);
			StatsAnteileingelasteteFA = (dataRow["StatsAnteileingelasteteFA"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsAnteileingelasteteFA"]);
			StatsBacklogFG = (dataRow["StatsBacklogFG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsBacklogFG"]);
			StatsCapaHorizons = (dataRow["StatsCapaHorizons"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsCapaHorizons"]);
			StatsCapaPlanning = (dataRow["StatsCapaPlanning"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsCapaPlanning"]);
			StatsCreatedInvoices = (dataRow["StatsCreatedInvoices"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsCreatedInvoices"]);
			StatsDeliveries = (dataRow["StatsDeliveries"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsDeliveries"]);
			StatsFAAnderungshistoire = (dataRow["StatsFAAnderungshistoire"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsFAAnderungshistoire"]);
			StatsLagerbestandFGCRP = (dataRow["StatsLagerbestandFGCRP"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsLagerbestandFGCRP"]);
			StatsRahmenSale = (dataRow["StatsRahmenSale"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsRahmenSale"]);
			StatsStockCS = (dataRow["StatsStockCS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsStockCS"]);
			StatsStockExternalWarehouse = (dataRow["StatsStockExternalWarehouse"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsStockExternalWarehouse"]);
			StatsStockFG = (dataRow["StatsStockFG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsStockFG"]);
			SystemLogs = (dataRow["SystemLogs"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SystemLogs"]);
			UBGStatusChange = (dataRow["UBGStatusChange"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBGStatusChange"]);
			ViewFGBestandHistory = (dataRow["ViewFGBestandHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewFGBestandHistory"]);
		}

		public AccessProfileEntity ShallowClone()
		{
			return new AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				Administration = Administration,
				AgentFGXlsHistory = AgentFGXlsHistory,
				AllFaChanges = AllFaChanges,
				AllProductionWarehouses = AllProductionWarehouses,
				Configuration = Configuration,
				ConfigurationAppoitments = ConfigurationAppoitments,
				ConfigurationChangeEmployees = ConfigurationChangeEmployees,
				ConfigurationReplacements = ConfigurationReplacements,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CRPDashboardActiveArticles = CRPDashboardActiveArticles,
				CRPDashboardCancelledOrders = CRPDashboardCancelledOrders,
				CRPDashboardCreatedOrders = CRPDashboardCreatedOrders,
				CRPDashboardOpenOrders = CRPDashboardOpenOrders,
				CRPDashboardOpenOrdersHours = CRPDashboardOpenOrdersHours,
				CRPDashboardTotalStockFG = CRPDashboardTotalStockFG,
				CRPFAPlannung = CRPFAPlannung,
				CRPPlanning = CRPPlanning,
				CRPRequirement = CRPRequirement,
				CRPUBGPlanning = CRPUBGPlanning,
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
				ExportFGXlsHistory = ExportFGXlsHistory,
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
				FaDateChangeHistory = FaDateChangeHistory,
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
				FaHoursMovement = FaHoursMovement,
				FALaufkarteSchneiderei = FALaufkarteSchneiderei,
				FaPlanningEdit = FaPlanningEdit,
				FaPlanningView = FaPlanningView,
				FaPlanningViolation = FaPlanningViolation,
				FAPlannung = FAPlannung,
				FAPlannungHistory = FAPlannungHistory,
				FAPlannungHistoryForceAgent = FAPlannungHistoryForceAgent,
				FAPlannungHistoryXLSExport = FAPlannungHistoryXLSExport,
				FAPlannungHistoryXLSImport = FAPlannungHistoryXLSImport,
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
				FAUpdatePrio = FAUpdatePrio,
				FAUpdateTerminHorizon1 = FAUpdateTerminHorizon1,
				FAUpdateTerminHorizon2 = FAUpdateTerminHorizon2,
				FAUpdateTerminHorizon3 = FAUpdateTerminHorizon3,
				FAWerkWunshAdmin = FAWerkWunshAdmin,
				Fertigung = Fertigung,
				FertigungLog = FertigungLog,
				Forecast = Forecast,
				ForecastCreate = ForecastCreate,
				ForecastDelete = ForecastDelete,
				ForecastStatistics = ForecastStatistics,
				Id = Id,
				ImportFGXlsHistory = ImportFGXlsHistory,
				isDefault = isDefault,
				LogsFGXlsHistory = LogsFGXlsHistory,
				ModuleActivated = ModuleActivated,
				ProductionPlanningByCustomer = ProductionPlanningByCustomer,
				Statistics = Statistics,
				StatsAnteileingelasteteFA = StatsAnteileingelasteteFA,
				StatsBacklogFG = StatsBacklogFG,
				StatsCapaHorizons = StatsCapaHorizons,
				StatsCapaPlanning = StatsCapaPlanning,
				StatsCreatedInvoices = StatsCreatedInvoices,
				StatsDeliveries = StatsDeliveries,
				StatsFAAnderungshistoire = StatsFAAnderungshistoire,
				StatsLagerbestandFGCRP = StatsLagerbestandFGCRP,
				StatsRahmenSale = StatsRahmenSale,
				StatsStockCS = StatsStockCS,
				StatsStockExternalWarehouse = StatsStockExternalWarehouse,
				StatsStockFG = StatsStockFG,
				SystemLogs = SystemLogs,
				UBGStatusChange = UBGStatusChange,
				ViewFGBestandHistory = ViewFGBestandHistory
			};
		}
	}
}

