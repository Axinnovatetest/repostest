using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class AccessProfileEntity
	{
		public bool? AB_LT { get; set; }
		public bool? AB_LT_EDI { get; set; }
		public bool? ABPosHorizon1 { get; set; }
		public bool? ABPosHorizon2 { get; set; }
		public bool? ABPosHorizon3 { get; set; }
		public string AccessProfileName { get; set; }
		public bool? Administration { get; set; }
		public bool? BVBookedEdit { get; set; }
		public bool? BVDoneEdit { get; set; }
		public bool? BVFaCreate { get; set; }
		public bool? Configuration { get; set; }
		public bool? ConfigurationAppoitments { get; set; }
		public bool? ConfigurationChangeEmployees { get; set; }
		public bool? ConfigurationReplacements { get; set; }
		public bool? ConfigurationReporting { get; set; }
		public bool? ConfirmationBookedEdit { get; set; }
		public bool? ConfirmationCreate { get; set; }
		public bool? ConfirmationDelete { get; set; }
		public bool? ConfirmationDeliveryNote { get; set; }
		public bool? ConfirmationDoneEdit { get; set; }
		public bool? ConfirmationEdit { get; set; }
		public bool? ConfirmationPositionEdit { get; set; }
		public bool? ConfirmationPositionProduction { get; set; }
		public bool? ConfirmationReport { get; set; }
		public bool? ConfirmationValidate { get; set; }
		public bool? ConfirmationView { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public bool? CSInfoEdit { get; set; }
		public bool? DelforCreate { get; set; }
		public bool? DelforDelete { get; set; }
		public bool? DelforDeletePosition { get; set; }
		public bool? DelforOrderConfirmation { get; set; }
		public bool? DelforReport { get; set; }
		public bool? DelforStatistics { get; set; }
		public bool? DelforView { get; set; }
		public bool? DeliveryNoteBookedEdit { get; set; }
		public bool? DeliveryNoteCreate { get; set; }
		public bool? DeliveryNoteDelete { get; set; }
		public bool? DeliveryNoteDoneEdit { get; set; }
		public bool? DeliveryNoteEdit { get; set; }
		public bool? DeliveryNoteLog { get; set; }
		public bool? DeliveryNotePositionEdit { get; set; }
		public bool? DeliveryNoteReport { get; set; }
		public bool? DeliveryNoteView { get; set; }
		public bool? DLFPosHorizon1 { get; set; }
		public bool? DLFPosHorizon2 { get; set; }
		public bool? DLFPosHorizon3 { get; set; }
		public bool? EDI { get; set; }
		public bool? EDIDownloadFile { get; set; }
		public bool? EDIError { get; set; }
		public bool? EDIErrorEdit { get; set; }
		public bool? EDIErrorValidated { get; set; }
		public bool? EDILogOrderValidated { get; set; }
		public bool? EDIOrder { get; set; }
		public bool? EDIOrderEdit { get; set; }
		public bool? EDIOrderPositionEdit { get; set; }
		public bool? EDIOrderProduction { get; set; }
		public bool? EDIOrderProductionPosition { get; set; }
		public bool? EDIOrderReport { get; set; }
		public bool? EDIOrderValidated { get; set; }
		public bool? EDIOrderValidatedEdit { get; set; }
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
		public bool? ForcastCreate { get; set; }
		public bool? ForcastDelete { get; set; }
		public bool? ForcastEdit { get; set; }
		public bool? ForcastLog { get; set; }
		public bool? ForcastPositionEdit { get; set; }
		public bool? ForcastReport { get; set; }
		public bool? ForcastView { get; set; }
		public bool? FRCPosHorizon1 { get; set; }
		public bool? FRCPosHorizon2 { get; set; }
		public bool? FRCPosHorizon3 { get; set; }
		public bool? GSPosHorizon1 { get; set; }
		public bool? GSPosHorizon2 { get; set; }
		public bool? GSPosHorizon3 { get; set; }
		public bool? GutschriftBookedEdit { get; set; }
		public bool? GutschriftCreate { get; set; }
		public bool? GutschriftDelete { get; set; }
		public bool? GutschriftDoneEdit { get; set; }
		public bool? GutschriftEdit { get; set; }
		public bool? GutschriftLog { get; set; }
		public bool? GutschriftPositionEdit { get; set; }
		public bool? GutschriftReport { get; set; }
		public bool? GutschriftView { get; set; }
		public int Id { get; set; }
		public bool? InsideSalesChecks { get; set; }
		public bool? InsideSalesChecksArchive { get; set; }
		public bool? InsideSalesCustomerSummary { get; set; }
		public bool? InsideSalesMinimumStockEvaluation { get; set; }
		public bool? InsideSalesMinimumStockEvaluationTable { get; set; }
		public bool? InsideSalesOverdueOrders { get; set; }
		public bool? InsideSalesOverdueOrdersTable { get; set; }
		public bool? InsideSalesTotalUnbookedOrders { get; set; }
		public bool? InsideSalesTotalUnbookedOrdersTable { get; set; }
		public bool? InsideSalesTurnoverCurrentWeek { get; set; }
		public bool? IsDefault { get; set; }
		public bool? LSPosHorizon1 { get; set; }
		public bool? LSPosHorizon2 { get; set; }
		public bool? LSPosHorizon3 { get; set; }
		public int? mId { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? OrderProcessing { get; set; }
		public bool? OrderProcessingLog { get; set; }
		public bool? Rahmen { get; set; }
		public bool? RahmenAdd { get; set; }
		public bool? RahmenAddAB { get; set; }
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
		public bool? RAPosHorizon1 { get; set; }
		public bool? RAPosHorizon2 { get; set; }
		public bool? RAPosHorizon3 { get; set; }
		public bool? Rechnung { get; set; }
		public bool? RechnungAutoCreation { get; set; }
		public bool? RechnungBookedEdit { get; set; }
		public bool? RechnungConfig { get; set; }
		public bool? RechnungDelete { get; set; }
		public bool? RechnungDoneEdit { get; set; }
		public bool? RechnungManualCreation { get; set; }
		public bool? RechnungReport { get; set; }
		public bool? RechnungSend { get; set; }
		public bool? RechnungValidate { get; set; }
		public bool? RGPosHorizon1 { get; set; }
		public bool? RGPosHorizon2 { get; set; }
		public bool? RGPosHorizon3 { get; set; }
		public bool? Statistics { get; set; }
		public bool? StatsBacklogFGAdmin { get; set; }
		public bool? StatsBacklogHWAdmin { get; set; }
		public bool? StatsCapaCutting { get; set; }
		public bool? StatsCapaHorizons { get; set; }
		public bool? StatsCapaLong { get; set; }
		public bool? StatsCapaPlanning { get; set; }
		public bool? StatsCapaShort { get; set; }
		public bool? StatsRechnungAL { get; set; }
		public bool? StatsRechnungBETN { get; set; }
		public bool? StatsRechnungCZ { get; set; }
		public bool? StatsRechnungDE { get; set; }
		public bool? StatsRechnungGZTN { get; set; }
		public bool? StatsRechnungTN { get; set; }
		public bool? StatsRechnungWS { get; set; }
		public bool? StatsStockCS { get; set; }
		public bool? StatsStockExternalWarehouse { get; set; }
		public bool? StatsStockFG { get; set; }
		public bool? UBGStatusChange { get; set; }

		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			AB_LT = (dataRow["AB_LT"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AB_LT"]);
			AB_LT_EDI = (dataRow["AB_LT_EDI"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AB_LT_EDI"]);
			ABPosHorizon1 = (dataRow["ABPosHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ABPosHorizon1"]);
			ABPosHorizon2 = (dataRow["ABPosHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ABPosHorizon2"]);
			ABPosHorizon3 = (dataRow["ABPosHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ABPosHorizon3"]);
			AccessProfileName = Convert.ToString(dataRow["AccessProfileName"]);
			Administration = (dataRow["Administration"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Administration"]);
			BVBookedEdit = (dataRow["BVBookedEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BVBookedEdit"]);
			BVDoneEdit = (dataRow["BVDoneEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BVDoneEdit"]);
			BVFaCreate = (dataRow["BVFaCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BVFaCreate"]);
			Configuration = (dataRow["Configuration"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Configuration"]);
			ConfigurationAppoitments = (dataRow["ConfigurationAppoitments"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigurationAppoitments"]);
			ConfigurationChangeEmployees = (dataRow["ConfigurationChangeEmployees"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigurationChangeEmployees"]);
			ConfigurationReplacements = (dataRow["ConfigurationReplacements"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigurationReplacements"]);
			ConfigurationReporting = (dataRow["ConfigurationReporting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigurationReporting"]);
			ConfirmationBookedEdit = (dataRow["ConfirmationBookedEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationBookedEdit"]);
			ConfirmationCreate = (dataRow["ConfirmationCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationCreate"]);
			ConfirmationDelete = (dataRow["ConfirmationDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationDelete"]);
			ConfirmationDeliveryNote = (dataRow["ConfirmationDeliveryNote"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationDeliveryNote"]);
			ConfirmationDoneEdit = (dataRow["ConfirmationDoneEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationDoneEdit"]);
			ConfirmationEdit = (dataRow["ConfirmationEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationEdit"]);
			ConfirmationPositionEdit = (dataRow["ConfirmationPositionEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationPositionEdit"]);
			ConfirmationPositionProduction = (dataRow["ConfirmationPositionProduction"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationPositionProduction"]);
			ConfirmationReport = (dataRow["ConfirmationReport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationReport"]);
			ConfirmationValidate = (dataRow["ConfirmationValidate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationValidate"]);
			ConfirmationView = (dataRow["ConfirmationView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfirmationView"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			CSInfoEdit = (dataRow["CSInfoEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CSInfoEdit"]);
			DelforCreate = (dataRow["DelforCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforCreate"]);
			DelforDelete = (dataRow["DelforDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforDelete"]);
			DelforDeletePosition = (dataRow["DelforDeletePosition"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforDeletePosition"]);
			DelforOrderConfirmation = (dataRow["DelforOrderConfirmation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforOrderConfirmation"]);
			DelforReport = (dataRow["DelforReport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforReport"]);
			DelforStatistics = (dataRow["DelforStatistics"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforStatistics"]);
			DelforView = (dataRow["DelforView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DelforView"]);
			DeliveryNoteBookedEdit = (dataRow["DeliveryNoteBookedEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNoteBookedEdit"]);
			DeliveryNoteCreate = (dataRow["DeliveryNoteCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNoteCreate"]);
			DeliveryNoteDelete = (dataRow["DeliveryNoteDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNoteDelete"]);
			DeliveryNoteDoneEdit = (dataRow["DeliveryNoteDoneEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNoteDoneEdit"]);
			DeliveryNoteEdit = (dataRow["DeliveryNoteEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNoteEdit"]);
			DeliveryNoteLog = (dataRow["DeliveryNoteLog"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNoteLog"]);
			DeliveryNotePositionEdit = (dataRow["DeliveryNotePositionEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNotePositionEdit"]);
			DeliveryNoteReport = (dataRow["DeliveryNoteReport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNoteReport"]);
			DeliveryNoteView = (dataRow["DeliveryNoteView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeliveryNoteView"]);
			DLFPosHorizon1 = (dataRow["DLFPosHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DLFPosHorizon1"]);
			DLFPosHorizon2 = (dataRow["DLFPosHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DLFPosHorizon2"]);
			DLFPosHorizon3 = (dataRow["DLFPosHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DLFPosHorizon3"]);
			EDI = (dataRow["EDI"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI"]);
			EDIDownloadFile = (dataRow["EDIDownloadFile"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIDownloadFile"]);
			EDIError = (dataRow["EDIError"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIError"]);
			EDIErrorEdit = (dataRow["EDIErrorEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIErrorEdit"]);
			EDIErrorValidated = (dataRow["EDIErrorValidated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIErrorValidated"]);
			EDILogOrderValidated = (dataRow["EDILogOrderValidated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDILogOrderValidated"]);
			EDIOrder = (dataRow["EDIOrder"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIOrder"]);
			EDIOrderEdit = (dataRow["EDIOrderEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIOrderEdit"]);
			EDIOrderPositionEdit = (dataRow["EDIOrderPositionEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIOrderPositionEdit"]);
			EDIOrderProduction = (dataRow["EDIOrderProduction"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIOrderProduction"]);
			EDIOrderProductionPosition = (dataRow["EDIOrderProductionPosition"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIOrderProductionPosition"]);
			EDIOrderReport = (dataRow["EDIOrderReport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIOrderReport"]);
			EDIOrderValidated = (dataRow["EDIOrderValidated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIOrderValidated"]);
			EDIOrderValidatedEdit = (dataRow["EDIOrderValidatedEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDIOrderValidatedEdit"]);
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
			FALaufkarteSchneiderei = (dataRow["FALaufkarteSchneiderei"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FALaufkarteSchneiderei"]);
			FaPlanningEdit = (dataRow["FaPlanningEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaPlanningEdit"]);
			FaPlanningView = (dataRow["FaPlanningView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FaPlanningView"]);
			FAPlannung = (dataRow["FAPlannung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAPlannung"]);
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
			FAUpdateTerminHorizon1 = (dataRow["FAUpdateTerminHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateTerminHorizon1"]);
			FAUpdateTerminHorizon2 = (dataRow["FAUpdateTerminHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateTerminHorizon2"]);
			FAUpdateTerminHorizon3 = (dataRow["FAUpdateTerminHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAUpdateTerminHorizon3"]);
			FAWerkWunshAdmin = (dataRow["FAWerkWunshAdmin"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FAWerkWunshAdmin"]);
			Fertigung = (dataRow["Fertigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Fertigung"]);
			FertigungLog = (dataRow["FertigungLog"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FertigungLog"]);
			ForcastCreate = (dataRow["ForcastCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForcastCreate"]);
			ForcastDelete = (dataRow["ForcastDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForcastDelete"]);
			ForcastEdit = (dataRow["ForcastEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForcastEdit"]);
			ForcastLog = (dataRow["ForcastLog"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForcastLog"]);
			ForcastPositionEdit = (dataRow["ForcastPositionEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForcastPositionEdit"]);
			ForcastReport = (dataRow["ForcastReport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForcastReport"]);
			ForcastView = (dataRow["ForcastView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ForcastView"]);
			FRCPosHorizon1 = (dataRow["FRCPosHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FRCPosHorizon1"]);
			FRCPosHorizon2 = (dataRow["FRCPosHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FRCPosHorizon2"]);
			FRCPosHorizon3 = (dataRow["FRCPosHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FRCPosHorizon3"]);
			GSPosHorizon1 = (dataRow["GSPosHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GSPosHorizon1"]);
			GSPosHorizon2 = (dataRow["GSPosHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GSPosHorizon2"]);
			GSPosHorizon3 = (dataRow["GSPosHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GSPosHorizon3"]);
			GutschriftBookedEdit = (dataRow["GutschriftBookedEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftBookedEdit"]);
			GutschriftCreate = (dataRow["GutschriftCreate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftCreate"]);
			GutschriftDelete = (dataRow["GutschriftDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftDelete"]);
			GutschriftDoneEdit = (dataRow["GutschriftDoneEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftDoneEdit"]);
			GutschriftEdit = (dataRow["GutschriftEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftEdit"]);
			GutschriftLog = (dataRow["GutschriftLog"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftLog"]);
			GutschriftPositionEdit = (dataRow["GutschriftPositionEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftPositionEdit"]);
			GutschriftReport = (dataRow["GutschriftReport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftReport"]);
			GutschriftView = (dataRow["GutschriftView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GutschriftView"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			InsideSalesChecks = (dataRow["InsideSalesChecks"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesChecks"]);
			InsideSalesChecksArchive = (dataRow["InsideSalesChecksArchive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesChecksArchive"]);
			InsideSalesCustomerSummary = (dataRow["InsideSalesCustomerSummary"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesCustomerSummary"]);
			InsideSalesMinimumStockEvaluation = (dataRow["InsideSalesMinimumStockEvaluation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesMinimumStockEvaluation"]);
			InsideSalesMinimumStockEvaluationTable = (dataRow["InsideSalesMinimumStockEvaluationTable"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesMinimumStockEvaluationTable"]);
			InsideSalesOverdueOrders = (dataRow["InsideSalesOverdueOrders"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesOverdueOrders"]);
			InsideSalesOverdueOrdersTable = (dataRow["InsideSalesOverdueOrdersTable"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesOverdueOrdersTable"]);
			InsideSalesTotalUnbookedOrders = (dataRow["InsideSalesTotalUnbookedOrders"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesTotalUnbookedOrders"]);
			InsideSalesTotalUnbookedOrdersTable = (dataRow["InsideSalesTotalUnbookedOrdersTable"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesTotalUnbookedOrdersTable"]);
			InsideSalesTurnoverCurrentWeek = (dataRow["InsideSalesTurnoverCurrentWeek"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InsideSalesTurnoverCurrentWeek"]);
			IsDefault = (dataRow["IsDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsDefault"]);
			LSPosHorizon1 = (dataRow["LSPosHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSPosHorizon1"]);
			LSPosHorizon2 = (dataRow["LSPosHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSPosHorizon2"]);
			LSPosHorizon3 = (dataRow["LSPosHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSPosHorizon3"]);
			mId = (dataRow["mId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["mId"]);
			ModuleActivated = (dataRow["ModuleActivated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ModuleActivated"]);
			OrderProcessing = (dataRow["OrderProcessing"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OrderProcessing"]);
			OrderProcessingLog = (dataRow["OrderProcessingLog"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OrderProcessingLog"]);
			Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen"]);
			RahmenAdd = (dataRow["RahmenAdd"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenAdd"]);
			RahmenAddAB = (dataRow["RahmenAddAB"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RahmenAddAB"]);
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
			RAPosHorizon1 = (dataRow["RAPosHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RAPosHorizon1"]);
			RAPosHorizon2 = (dataRow["RAPosHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RAPosHorizon2"]);
			RAPosHorizon3 = (dataRow["RAPosHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RAPosHorizon3"]);
			Rechnung = (dataRow["Rechnung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rechnung"]);
			RechnungAutoCreation = (dataRow["RechnungAutoCreation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungAutoCreation"]);
			RechnungBookedEdit = (dataRow["RechnungBookedEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungBookedEdit"]);
			RechnungConfig = (dataRow["RechnungConfig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungConfig"]);
			RechnungDelete = (dataRow["RechnungDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungDelete"]);
			RechnungDoneEdit = (dataRow["RechnungDoneEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungDoneEdit"]);
			RechnungManualCreation = (dataRow["RechnungManualCreation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungManualCreation"]);
			RechnungReport = (dataRow["RechnungReport"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungReport"]);
			RechnungSend = (dataRow["RechnungSend"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungSend"]);
			RechnungValidate = (dataRow["RechnungValidate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RechnungValidate"]);
			RGPosHorizon1 = (dataRow["RGPosHorizon1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RGPosHorizon1"]);
			RGPosHorizon2 = (dataRow["RGPosHorizon2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RGPosHorizon2"]);
			RGPosHorizon3 = (dataRow["RGPosHorizon3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RGPosHorizon3"]);
			Statistics = (dataRow["Statistics"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Statistics"]);
			StatsBacklogFGAdmin = (dataRow["StatsBacklogFGAdmin"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsBacklogFGAdmin"]);
			StatsBacklogHWAdmin = (dataRow["StatsBacklogHWAdmin"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsBacklogHWAdmin"]);
			StatsCapaCutting = (dataRow["StatsCapaCutting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsCapaCutting"]);
			StatsCapaHorizons = (dataRow["StatsCapaHorizons"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsCapaHorizons"]);
			StatsCapaLong = (dataRow["StatsCapaLong"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsCapaLong"]);
			StatsCapaPlanning = (dataRow["StatsCapaPlanning"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsCapaPlanning"]);
			StatsCapaShort = (dataRow["StatsCapaShort"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsCapaShort"]);
			StatsRechnungAL = (dataRow["StatsRechnungAL"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsRechnungAL"]);
			StatsRechnungBETN = (dataRow["StatsRechnungBETN"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsRechnungBETN"]);
			StatsRechnungCZ = (dataRow["StatsRechnungCZ"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsRechnungCZ"]);
			StatsRechnungDE = (dataRow["StatsRechnungDE"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsRechnungDE"]);
			StatsRechnungGZTN = (dataRow["StatsRechnungGZTN"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsRechnungGZTN"]);
			StatsRechnungTN = (dataRow["StatsRechnungTN"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsRechnungTN"]);
			StatsRechnungWS = (dataRow["StatsRechnungWS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsRechnungWS"]);
			StatsStockCS = (dataRow["StatsStockCS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsStockCS"]);
			StatsStockExternalWarehouse = (dataRow["StatsStockExternalWarehouse"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsStockExternalWarehouse"]);
			StatsStockFG = (dataRow["StatsStockFG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatsStockFG"]);
			UBGStatusChange = (dataRow["UBGStatusChange"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBGStatusChange"]);
		}

		public AccessProfileEntity ShallowClone()
		{
			return new AccessProfileEntity
			{
				AB_LT = AB_LT,
				AB_LT_EDI = AB_LT_EDI,
				ABPosHorizon1 = ABPosHorizon1,
				ABPosHorizon2 = ABPosHorizon2,
				ABPosHorizon3 = ABPosHorizon3,
				AccessProfileName = AccessProfileName,
				Administration = Administration,
				BVBookedEdit = BVBookedEdit,
				BVDoneEdit = BVDoneEdit,
				BVFaCreate = BVFaCreate,
				Configuration = Configuration,
				ConfigurationAppoitments = ConfigurationAppoitments,
				ConfigurationChangeEmployees = ConfigurationChangeEmployees,
				ConfigurationReplacements = ConfigurationReplacements,
				ConfigurationReporting = ConfigurationReporting,
				ConfirmationBookedEdit = ConfirmationBookedEdit,
				ConfirmationCreate = ConfirmationCreate,
				ConfirmationDelete = ConfirmationDelete,
				ConfirmationDeliveryNote = ConfirmationDeliveryNote,
				ConfirmationDoneEdit = ConfirmationDoneEdit,
				ConfirmationEdit = ConfirmationEdit,
				ConfirmationPositionEdit = ConfirmationPositionEdit,
				ConfirmationPositionProduction = ConfirmationPositionProduction,
				ConfirmationReport = ConfirmationReport,
				ConfirmationValidate = ConfirmationValidate,
				ConfirmationView = ConfirmationView,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CSInfoEdit = CSInfoEdit,
				DelforCreate = DelforCreate,
				DelforDelete = DelforDelete,
				DelforDeletePosition = DelforDeletePosition,
				DelforOrderConfirmation = DelforOrderConfirmation,
				DelforReport = DelforReport,
				DelforStatistics = DelforStatistics,
				DelforView = DelforView,
				DeliveryNoteBookedEdit = DeliveryNoteBookedEdit,
				DeliveryNoteCreate = DeliveryNoteCreate,
				DeliveryNoteDelete = DeliveryNoteDelete,
				DeliveryNoteDoneEdit = DeliveryNoteDoneEdit,
				DeliveryNoteEdit = DeliveryNoteEdit,
				DeliveryNoteLog = DeliveryNoteLog,
				DeliveryNotePositionEdit = DeliveryNotePositionEdit,
				DeliveryNoteReport = DeliveryNoteReport,
				DeliveryNoteView = DeliveryNoteView,
				DLFPosHorizon1 = DLFPosHorizon1,
				DLFPosHorizon2 = DLFPosHorizon2,
				DLFPosHorizon3 = DLFPosHorizon3,
				EDI = EDI,
				EDIDownloadFile = EDIDownloadFile,
				EDIError = EDIError,
				EDIErrorEdit = EDIErrorEdit,
				EDIErrorValidated = EDIErrorValidated,
				EDILogOrderValidated = EDILogOrderValidated,
				EDIOrder = EDIOrder,
				EDIOrderEdit = EDIOrderEdit,
				EDIOrderPositionEdit = EDIOrderPositionEdit,
				EDIOrderProduction = EDIOrderProduction,
				EDIOrderProductionPosition = EDIOrderProductionPosition,
				EDIOrderReport = EDIOrderReport,
				EDIOrderValidated = EDIOrderValidated,
				EDIOrderValidatedEdit = EDIOrderValidatedEdit,
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
				ForcastCreate = ForcastCreate,
				ForcastDelete = ForcastDelete,
				ForcastEdit = ForcastEdit,
				ForcastLog = ForcastLog,
				ForcastPositionEdit = ForcastPositionEdit,
				ForcastReport = ForcastReport,
				ForcastView = ForcastView,
				FRCPosHorizon1 = FRCPosHorizon1,
				FRCPosHorizon2 = FRCPosHorizon2,
				FRCPosHorizon3 = FRCPosHorizon3,
				GSPosHorizon1 = GSPosHorizon1,
				GSPosHorizon2 = GSPosHorizon2,
				GSPosHorizon3 = GSPosHorizon3,
				GutschriftBookedEdit = GutschriftBookedEdit,
				GutschriftCreate = GutschriftCreate,
				GutschriftDelete = GutschriftDelete,
				GutschriftDoneEdit = GutschriftDoneEdit,
				GutschriftEdit = GutschriftEdit,
				GutschriftLog = GutschriftLog,
				GutschriftPositionEdit = GutschriftPositionEdit,
				GutschriftReport = GutschriftReport,
				GutschriftView = GutschriftView,
				Id = Id,
				InsideSalesChecks = InsideSalesChecks,
				InsideSalesChecksArchive = InsideSalesChecksArchive,
				InsideSalesCustomerSummary = InsideSalesCustomerSummary,
				InsideSalesMinimumStockEvaluation = InsideSalesMinimumStockEvaluation,
				InsideSalesMinimumStockEvaluationTable = InsideSalesMinimumStockEvaluationTable,
				InsideSalesOverdueOrders = InsideSalesOverdueOrders,
				InsideSalesOverdueOrdersTable = InsideSalesOverdueOrdersTable,
				InsideSalesTotalUnbookedOrders = InsideSalesTotalUnbookedOrders,
				InsideSalesTotalUnbookedOrdersTable = InsideSalesTotalUnbookedOrdersTable,
				InsideSalesTurnoverCurrentWeek = InsideSalesTurnoverCurrentWeek,
				IsDefault = IsDefault,
				LSPosHorizon1 = LSPosHorizon1,
				LSPosHorizon2 = LSPosHorizon2,
				LSPosHorizon3 = LSPosHorizon3,
				mId = mId,
				ModuleActivated = ModuleActivated,
				OrderProcessing = OrderProcessing,
				OrderProcessingLog = OrderProcessingLog,
				Rahmen = Rahmen,
				RahmenAdd = RahmenAdd,
				RahmenAddAB = RahmenAddAB,
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
				RAPosHorizon1 = RAPosHorizon1,
				RAPosHorizon2 = RAPosHorizon2,
				RAPosHorizon3 = RAPosHorizon3,
				Rechnung = Rechnung,
				RechnungAutoCreation = RechnungAutoCreation,
				RechnungBookedEdit = RechnungBookedEdit,
				RechnungConfig = RechnungConfig,
				RechnungDelete = RechnungDelete,
				RechnungDoneEdit = RechnungDoneEdit,
				RechnungManualCreation = RechnungManualCreation,
				RechnungReport = RechnungReport,
				RechnungSend = RechnungSend,
				RechnungValidate = RechnungValidate,
				RGPosHorizon1 = RGPosHorizon1,
				RGPosHorizon2 = RGPosHorizon2,
				RGPosHorizon3 = RGPosHorizon3,
				Statistics = Statistics,
				StatsBacklogFGAdmin = StatsBacklogFGAdmin,
				StatsBacklogHWAdmin = StatsBacklogHWAdmin,
				StatsCapaCutting = StatsCapaCutting,
				StatsCapaHorizons = StatsCapaHorizons,
				StatsCapaLong = StatsCapaLong,
				StatsCapaPlanning = StatsCapaPlanning,
				StatsCapaShort = StatsCapaShort,
				StatsRechnungAL = StatsRechnungAL,
				StatsRechnungBETN = StatsRechnungBETN,
				StatsRechnungCZ = StatsRechnungCZ,
				StatsRechnungDE = StatsRechnungDE,
				StatsRechnungGZTN = StatsRechnungGZTN,
				StatsRechnungTN = StatsRechnungTN,
				StatsRechnungWS = StatsRechnungWS,
				StatsStockCS = StatsStockCS,
				StatsStockExternalWarehouse = StatsStockExternalWarehouse,
				StatsStockFG = StatsStockFG,
				UBGStatusChange = UBGStatusChange
			};
		}
	}
}