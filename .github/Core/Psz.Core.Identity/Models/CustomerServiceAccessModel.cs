using System;
using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class CustomerServiceAccessModel
	{
		public string AccessProfileName { get; set; }
		public int Id { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public bool ModuleActivated { get; set; }
		public bool Administration { get; set; }
		public bool Configuration { get; set; }
		public bool ConfigurationAppoitments { get; set; }
		public bool ConfigurationChangeEmployees { get; set; }
		public bool ConfigurationReplacements { get; set; }
		public bool ConfigurationReporting { get; set; }
		public bool ConfirmationCreate { get; set; }
		public bool ConfirmationDelete { get; set; }
		public bool ConfirmationDeliveryNote { get; set; }
		public bool ConfirmationEdit { get; set; }
		public bool ConfirmationPositionEdit { get; set; }
		public bool ConfirmationPositionProduction { get; set; }
		public bool ConfirmationReport { get; set; }
		public bool ConfirmationValidate { get; set; }
		public bool ConfirmationView { get; set; }
		public bool DeliveryNoteCreate { get; set; }
		public bool DeliveryNoteDelete { get; set; }
		public bool DeliveryNoteEdit { get; set; }
		public bool DeliveryNoteLog { get; set; }
		public bool DeliveryNotePositionEdit { get; set; }
		public bool DeliveryNoteReport { get; set; }
		public bool DeliveryNoteView { get; set; }
		public bool EDI { get; set; }
		public bool EDIDownloadFile { get; set; }
		public bool EDIError { get; set; }
		public bool EDIErrorEdit { get; set; }
		public bool EDIErrorValidated { get; set; }
		public bool EDILogOrderValidated { get; set; }
		public bool EDIOrder { get; set; }
		public bool EDIOrderEdit { get; set; }
		public bool EDIOrderPositionEdit { get; set; }
		public bool EDIOrderProduction { get; set; }
		public bool EDIOrderProductionPosition { get; set; }
		public bool EDIOrderReport { get; set; }
		public bool EDIOrderValidated { get; set; }
		public bool EDIOrderValidatedEdit { get; set; }
		public bool FaABEdit { get; set; }
		public bool FaABView { get; set; }
		public bool FaActionBook { get; set; }
		public bool FaActionComplete { get; set; }
		public bool FaActionDelete { get; set; }
		public bool FaActionPrint { get; set; }
		public bool FaAdmin { get; set; }
		public bool FaAnalysis { get; set; }
		public bool FaCreate { get; set; }
		public bool FaDatenEdit { get; set; }
		public bool FaDatenView { get; set; }
		public bool FaDelete { get; set; }
		public bool FaEdit { get; set; }
		public bool FaHomeAnalysis { get; set; }
		public bool FaHomeUpdate { get; set; }
		public bool FaPlanningEdit { get; set; }
		public bool FaPlanningView { get; set; }
		public bool FaTechnicEdit { get; set; }
		public bool FaTechnicView { get; set; }
		public bool Fertigung { get; set; }
		public bool OrderProcessing { get; set; }
		public bool Statistics { get; set; }
		public bool FAWerkWunshAdmin { get; set; }
		public bool CSInfoEdit { get; set; }
		public bool OrderProcessingLog { get; set; }
		public bool FertigungLog { get; set; }
		public bool UBGStatusChange { get; set; }
		public bool FAAKtualTerminUpdate { get; set; }
		public bool FAAuswertungEndkontrolle { get; set; }
		public bool FACommissionert { get; set; }
		public bool FADrucken { get; set; }
		public bool FAErlidegen { get; set; }
		public bool FAExcelUpdateWerk { get; set; }
		public bool FAExcelUpdateWunsh { get; set; }
		public bool FAFehlrMaterial { get; set; }
		public bool FALaufkarteSchneiderei { get; set; }
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
		public bool FAUpdateByArticle { get; set; }
		public bool FAUpdateByFA { get; set; }
		public bool FAUpdateBemerkungExtern { get; set; }
		public bool StatsBacklogFGAdmin { get; set; }
		public bool StatsBacklogHWAdmin { get; set; }
		public bool RahmenAdd { get; set; }
		public bool RahmenAddPositions { get; set; }
		public bool RahmenDelete { get; set; }
		public bool RahmenDeletePositions { get; set; }
		public bool RahmenDocumentFlow { get; set; }
		public bool RahmenEditHeader { get; set; }
		public bool RahmenEditPositions { get; set; }
		public bool RahmenHistory { get; set; }
		public bool Rahmen { get; set; }
		//souilmi 30/05/2022
		public bool GutschriftView { get; set; }
		public bool GutschriftCreate { get; set; }
		public bool GutschriftEdit { get; set; }
		public bool GutschriftDelete { get; set; }
		public bool GutschriftReport { get; set; }
		public bool GutschriftLog { get; set; }
		public bool GutschriftPositionEdit { get; set; }
		//souilmi 08/06/2022
		public bool RahmenValdation { get; set; }
		public bool RahmenCancelation { get; set; }
		public bool RahmenClosure { get; set; }
		// - 2022-11-06
		public bool StatsRechnungAL { get; set; }
		public bool StatsRechnungCZ { get; set; }
		public bool StatsRechnungDE { get; set; }
		public bool StatsRechnungTN { get; set; }
		public bool StatsRechnungBETN { get; set; }
		public bool StatsRechnungGZTN { get; set; }
		public bool StatsRechnungWS { get; set; }
		//souilmi 23/09/2022
		public bool DelforCreate { get; set; }
		public bool DelforReport { get; set; }
		public bool DelforView { get; set; }
		public bool DelforOrderConfirmation { get; set; }
		public bool DelforDelete { get; set; }//souilmi 09/11/2022
		public bool DelforDeletePosition { get; set; }
		// - 2022-1-09
		public bool StatsStockCS { get; set; }
		public bool StatsStockExternalWarehouse { get; set; }
		public bool StatsStockFG { get; set; }
		//souilmi 09/09/2022
		public bool ForcastView { get; set; }
		public bool ForcastCreate { get; set; }
		public bool ForcastEdit { get; set; }
		public bool ForcastDelete { get; set; }
		public bool ForcastReport { get; set; }
		public bool ForcastLog { get; set; }
		public bool ForcastPositionEdit { get; set; }
		//souilmi 20/01/2023
		public bool Rechnung { get; set; }
		public bool RechnungAutoCreation { get; set; }
		public bool RechnungManualCreation { get; set; }
		public bool RechnungValidate { get; set; }
		public bool RechnungDelete { get; set; }
		public bool RechnungSend { get; set; }
		public bool RechnungReport { get; set; }
		public bool RechnungConfig { get; set; }
		//souilmi 01/08/2023
		public bool FACreateHorizon1 { get; set; }
		public bool FACreateHorizon2 { get; set; }
		public bool FACreateHorizon3 { get; set; }
		public bool FAUpdateTerminHorizon1 { get; set; }
		public bool FAUpdateTerminHorizon2 { get; set; }
		public bool FAUpdateTerminHorizon3 { get; set; }
		// - 2023-08-07
		public bool StatsCapaHorizons { get; set; }
		public bool StatsCapaPlanning { get; set; }
		public bool StatsCapaLong { get; set; }
		public bool StatsCapaShort { get; set; }
		public bool StatsCapaCutting { get; set; }
		// souilmi 15/08/2023
		public bool FACancelHorizon1 { get; set; }
		public bool FACancelHorizon2 { get; set; }
		public bool FACancelHorizon3 { get; set; }
		public bool ABPosHorizon1 { get; set; }
		public bool ABPosHorizon2 { get; set; }
		public bool ABPosHorizon3 { get; set; }
		public bool GSPosHorizon1 { get; set; }
		public bool GSPosHorizon2 { get; set; }
		public bool GSPosHorizon3 { get; set; }
		public bool LSPosHorizon1 { get; set; }
		public bool LSPosHorizon2 { get; set; }
		public bool LSPosHorizon3 { get; set; }
		public bool RAPosHorizon1 { get; set; }
		public bool RAPosHorizon2 { get; set; }
		public bool RAPosHorizon3 { get; set; }
		public bool RGPosHorizon1 { get; set; }
		public bool RGPosHorizon2 { get; set; }
		public bool RGPosHorizon3 { get; set; }
		public bool DLFPosHorizon1 { get; set; }
		public bool DLFPosHorizon2 { get; set; }
		public bool DLFPosHorizon3 { get; set; }
		public bool FRCPosHorizon1 { get; set; }
		public bool FRCPosHorizon2 { get; set; }
		public bool FRCPosHorizon3 { get; set; }
		//souilmi 26/092023
		public bool AB_LT { get; set; }
		public bool AB_LT_EDI { get; set; }
		public bool RahmenAddAB { get; set; }
		public bool BVCreate { get; set; }
		public bool BVEdit { get; set; }
		public bool BVDelete { get; set; }
		public bool BVReport { get; set; }
		public bool BVLog { get; set; }
		public bool BVPositionEdit { get; set; }
		public bool BVFaCreate { get; set; }
		public bool DelforStatistics { get; set; }
		//souilmi 03/10/2023
		public bool FATerminWerk { get; set; }
		public bool FABemerkungPlannug { get; set; }
		public bool FABemerkungZuPrio { get; set; }
		public bool FABemerkungZuGewerk { get; set; }
		// -
		public bool ConfirmationDoneEdit { get; set; }
		public bool ConfirmationBookedEdit { get; set; }
		public bool DeliveryNoteDoneEdit { get; set; }
		public bool DeliveryNoteBookedEdit { get; set; }
		public bool GutschriftDoneEdit { get; set; }
		public bool GutschriftBookedEdit { get; set; }
		public bool BVDoneEdit { get; set; }
		public bool BVBookedEdit { get; set; }
		public bool RechnungDoneEdit { get; set; }
		public bool RechnungBookedEdit { get; set; }
		public bool InsideSalesChecks { get; set; }
		public bool InsideSalesChecksArchive { get; set; }
		public bool InsideSalesCustomerSummary { get; set; }

		//-- inside sales overview
		public bool InsideSalesOverdueOrders { get; set; }
		public bool InsideSalesTurnoverCurrentWeek { get; set; }
		public bool InsideSalesTotalUnbookedOrders { get; set; }
		public bool InsideSalesMinimumStockEvaluation { get; set; }
		public bool InsideSalesOverdueOrdersTable { get; set; }
		public bool InsideSalesTotalUnbookedOrdersTable { get; set; }
		public bool InsideSalesMinimumStockEvaluationTable { get; set; }

		public CustomerServiceAccessModel()
		{
		}
		public CustomerServiceAccessModel(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;
			ModuleActivated = false;
			Administration = false;
			Configuration = false;
			ConfigurationAppoitments = false;
			ConfigurationChangeEmployees = false;
			ConfigurationReplacements = false;
			ConfigurationReporting = false;
			ConfirmationCreate = false;
			ConfirmationDelete = false;
			ConfirmationDeliveryNote = false;
			ConfirmationEdit = false;
			ConfirmationPositionEdit = false;
			ConfirmationPositionProduction = false;
			ConfirmationReport = false;
			ConfirmationValidate = false;
			ConfirmationView = false;
			DeliveryNoteCreate = false;
			DeliveryNoteDelete = false;
			DeliveryNoteEdit = false;
			DeliveryNoteLog = false;
			DeliveryNotePositionEdit = false;
			DeliveryNoteReport = false;
			DeliveryNoteView = false;
			EDI = false;
			EDIDownloadFile = false;
			EDIError = false;
			EDIErrorEdit = false;
			EDIErrorValidated = false;
			EDILogOrderValidated = false;
			EDIOrder = false;
			EDIOrderEdit = false;
			EDIOrderPositionEdit = false;
			EDIOrderProduction = false;
			EDIOrderProductionPosition = false;
			EDIOrderReport = false;
			EDIOrderValidated = false;
			EDIOrderValidatedEdit = false;
			FaABEdit = false;
			FaABView = false;
			FaActionBook = false;
			FaActionComplete = false;
			FaActionDelete = false;
			FaActionPrint = false;
			FaAdmin = false;
			FaAnalysis = false;
			FaCreate = false;
			FaDatenEdit = false;
			FaDatenView = false;
			FaDelete = false;
			FaEdit = false;
			FaHomeAnalysis = false;
			FaHomeUpdate = false;
			FaPlanningEdit = false;
			FaPlanningView = false;
			FaTechnicEdit = false;
			FaTechnicView = false;
			Fertigung = false;
			OrderProcessing = false;
			Statistics = false;
			FAWerkWunshAdmin = false;
			CSInfoEdit = false;
			OrderProcessingLog = false;
			FertigungLog = false;
			UBGStatusChange = false;

			// -
			FAAKtualTerminUpdate = false;
			FAAuswertungEndkontrolle = false;
			FACommissionert = false;
			FADrucken = false;
			FAErlidegen = false;
			FAExcelUpdateWerk = false;
			FAExcelUpdateWunsh = false;
			FAFehlrMaterial = false;
			FALaufkarteSchneiderei = false;
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
			FAUpdateByArticle = false;
			FAUpdateByFA = false;
			FAPlannung = false;
			FAPlannungTechnick = false;
			//
			RahmenAdd = false;
			RahmenAddPositions = false;
			RahmenDelete = false;
			RahmenDeletePositions = false;
			RahmenDocumentFlow = false;
			RahmenEditHeader = false;
			RahmenEditPositions = false;
			RahmenHistory = false;
			Rahmen = false;

			FAUpdateBemerkungExtern = false;
			StatsBacklogFGAdmin = false;
			StatsBacklogHWAdmin = false;
			//souilmi 30-05-2022
			GutschriftView = false;
			GutschriftCreate = false;
			GutschriftEdit = false;
			GutschriftDelete = false;
			GutschriftReport = false;
			GutschriftLog = false;
			GutschriftPositionEdit = false;
			//souilmi 08/06/2022
			RahmenValdation = false;
			RahmenCancelation = false;
			RahmenClosure = false;
			// - 2022-11-06
			StatsRechnungAL = false;
			StatsRechnungCZ = false;
			StatsRechnungDE = false;
			StatsRechnungTN = false;
			StatsRechnungBETN = false;
			StatsRechnungGZTN = false;
			StatsRechnungWS = false;
			// - 2022-11-09
			StatsStockCS = false;
			StatsStockExternalWarehouse = false;
			StatsStockFG = false;
			//souilmi 09/09/2022 //souilmi 23/09/2022
			DelforCreate = false;
			DelforReport = false;
			DelforView = false;
			DelforOrderConfirmation = false;
			//souilmi 09/11/2022
			DelforDelete = false;
			DelforDeletePosition = false;
			ForcastView = false;
			ForcastCreate = false;
			ForcastEdit = false;
			ForcastDelete = false;
			ForcastReport = false;
			ForcastLog = false;
			ForcastPositionEdit = false;
			//souilmi 23/09/2022
			DelforCreate = false;
			DelforReport = false;
			DelforView = false;
			DelforOrderConfirmation = false;
			//souilmi 09/11/2022
			DelforDelete = false;
			DelforDeletePosition = false;
			//souilmi 20/01/2023
			Rechnung = false;
			RechnungAutoCreation = false;
			RechnungManualCreation = false;
			RechnungValidate = false;
			RechnungDelete = false;
			RechnungSend = false;
			RechnungReport = false;
			RechnungConfig = false;
			//souilmi 20/01/2023
			Rechnung = false;
			RechnungAutoCreation = false;
			RechnungManualCreation = false;
			RechnungValidate = false;
			RechnungDelete = false;
			RechnungSend = false;
			RechnungReport = false;
			RechnungConfig = false;
			//souilmi 01/08/2023
			FACreateHorizon1 = false;
			FACreateHorizon2 = false;
			FACreateHorizon3 = false;
			FAUpdateTerminHorizon1 = false;
			FAUpdateTerminHorizon2 = false;
			FAUpdateTerminHorizon3 = false;
			// - 2023-08-07
			StatsCapaHorizons = false;
			StatsCapaPlanning = false;
			StatsCapaLong = false;
			StatsCapaShort = false;
			StatsCapaCutting = false;
			// souilmi 15/08/2023
			FACancelHorizon1 = false;
			FACancelHorizon2 = false;
			FACancelHorizon3 = false;
			ABPosHorizon1 = false;
			ABPosHorizon2 = false;
			ABPosHorizon3 = false;
			GSPosHorizon1 = false;
			GSPosHorizon2 = false;
			GSPosHorizon3 = false;
			LSPosHorizon1 = false;
			LSPosHorizon2 = false;
			LSPosHorizon3 = false;
			RAPosHorizon1 = false;
			RAPosHorizon2 = false;
			RAPosHorizon3 = false;
			RGPosHorizon1 = false;
			RGPosHorizon2 = false;
			RGPosHorizon3 = false;
			DLFPosHorizon1 = false;
			DLFPosHorizon2 = false;
			DLFPosHorizon3 = false;
			FRCPosHorizon1 = false;
			FRCPosHorizon2 = false;
			FRCPosHorizon3 = false;
			//souilmi 26/09/2023
			AB_LT = false;
			AB_LT_EDI = false;
			RahmenAddAB = false;
			BVCreate = false;
			BVEdit = false;
			BVDelete = false;
			BVReport = false;
			BVLog = false;
			BVPositionEdit = false;
			BVFaCreate = false;
			DelforStatistics = false;
			//souilmi 03/10/2023
			FATerminWerk = false;
			FABemerkungPlannug = false;
			FABemerkungZuPrio = false;
			FABemerkungZuGewerk = false;
			// -
			ConfirmationDoneEdit = false;
			ConfirmationBookedEdit = false;
			DeliveryNoteDoneEdit = false;
			DeliveryNoteBookedEdit = false;
			GutschriftDoneEdit = false;
			GutschriftBookedEdit = false;
			BVDoneEdit = false;
			BVBookedEdit = false;
			RechnungDoneEdit = false;
			RechnungBookedEdit = false;
			InsideSalesChecks = false;
			InsideSalesChecksArchive = false;
			InsideSalesCustomerSummary = false;
			//-- inside sales overview
			InsideSalesOverdueOrders = false;
			InsideSalesTurnoverCurrentWeek = false;
			InsideSalesTotalUnbookedOrders = false;
			InsideSalesMinimumStockEvaluation = false;
			InsideSalesOverdueOrdersTable = false;
			InsideSalesTotalUnbookedOrdersTable = false;
			InsideSalesMinimumStockEvaluationTable = false;

			foreach(var accessItem in accessProfileEntities)
			{
				ModuleActivated = ModuleActivated || (accessItem?.ModuleActivated ?? false);

				Administration = Administration || (accessItem?.Administration ?? false);
				Configuration = Configuration || (accessItem?.Configuration ?? false);
				ConfigurationAppoitments = ConfigurationAppoitments || (accessItem?.ConfigurationAppoitments ?? false);
				ConfigurationChangeEmployees = ConfigurationChangeEmployees || (accessItem?.ConfigurationChangeEmployees ?? false);
				ConfigurationReplacements = ConfigurationReplacements || (accessItem?.ConfigurationReplacements ?? false);
				ConfigurationReporting = ConfigurationReporting || (accessItem?.ConfigurationReporting ?? false);
				ConfirmationCreate = ConfirmationCreate || (accessItem?.ConfirmationCreate ?? false);
				ConfirmationDelete = ConfirmationDelete || (accessItem?.ConfirmationDelete ?? false);
				ConfirmationDeliveryNote = ConfirmationDeliveryNote || (accessItem?.ConfirmationDeliveryNote ?? false);
				ConfirmationEdit = ConfirmationEdit || (accessItem?.ConfirmationEdit ?? false);
				ConfirmationPositionEdit = ConfirmationPositionEdit || (accessItem?.ConfirmationPositionEdit ?? false);
				ConfirmationPositionProduction = ConfirmationPositionProduction || (accessItem?.ConfirmationPositionProduction ?? false);
				ConfirmationReport = ConfirmationReport || (accessItem?.ConfirmationReport ?? false);
				ConfirmationValidate = ConfirmationValidate || (accessItem?.ConfirmationValidate ?? false);
				ConfirmationView = ConfirmationView || (accessItem?.ConfirmationView ?? false);
				DeliveryNoteCreate = DeliveryNoteCreate || (accessItem?.DeliveryNoteCreate ?? false);
				DeliveryNoteDelete = DeliveryNoteDelete || (accessItem?.DeliveryNoteDelete ?? false);
				DeliveryNoteEdit = DeliveryNoteEdit || (accessItem?.DeliveryNoteEdit ?? false);
				DeliveryNoteLog = DeliveryNoteLog || (accessItem?.DeliveryNoteLog ?? false);
				DeliveryNotePositionEdit = DeliveryNotePositionEdit || (accessItem?.DeliveryNotePositionEdit ?? false);
				DeliveryNoteReport = DeliveryNoteReport || (accessItem?.DeliveryNoteReport ?? false);
				DeliveryNoteView = DeliveryNoteView || (accessItem?.DeliveryNoteView ?? false);
				EDI = EDI || (accessItem?.EDI ?? false);
				EDIDownloadFile = EDIDownloadFile || (accessItem?.EDIDownloadFile ?? false);
				EDIError = EDIError || (accessItem?.EDIError ?? false);
				EDIErrorEdit = EDIErrorEdit || (accessItem?.EDIErrorEdit ?? false);
				EDIErrorValidated = EDIErrorValidated || (accessItem?.EDIErrorValidated ?? false);
				EDILogOrderValidated = EDILogOrderValidated || (accessItem?.EDILogOrderValidated ?? false);
				EDIOrder = EDIOrder || (accessItem?.EDIOrder ?? false);
				EDIOrderEdit = EDIOrderEdit || (accessItem?.EDIOrderEdit ?? false);
				EDIOrderPositionEdit = EDIOrderPositionEdit || (accessItem?.EDIOrderPositionEdit ?? false);
				EDIOrderProduction = EDIOrderProduction || (accessItem?.EDIOrderProduction ?? false);
				EDIOrderProductionPosition = EDIOrderProductionPosition || (accessItem?.EDIOrderProductionPosition ?? false);
				EDIOrderReport = EDIOrderReport || (accessItem?.EDIOrderReport ?? false);
				EDIOrderValidated = EDIOrderValidated || (accessItem?.EDIOrderValidated ?? false);
				EDIOrderValidatedEdit = EDIOrderValidatedEdit || (accessItem?.EDIOrderValidatedEdit ?? false);
				FaABEdit = FaABEdit || (accessItem?.FaABEdit ?? false);
				FaABView = FaABView || (accessItem?.FaABView ?? false);
				FaActionBook = FaActionBook || (accessItem?.FaActionBook ?? false);
				FaActionComplete = FaActionComplete || (accessItem?.FaActionComplete ?? false);
				FaActionDelete = FaActionDelete || (accessItem?.FaActionDelete ?? false);
				FaActionPrint = FaActionPrint || (accessItem?.FaActionPrint ?? false);
				FaAdmin = FaAdmin || (accessItem?.FaAdmin ?? false);
				FaAnalysis = FaAnalysis || (accessItem?.FaAnalysis ?? false);
				FaCreate = FaCreate || (accessItem?.FaCreate ?? false);
				FaDatenEdit = FaDatenEdit || (accessItem?.FaDatenEdit ?? false);
				FaDatenView = FaDatenView || (accessItem?.FaDatenView ?? false);
				FaDelete = FaDelete || (accessItem?.FaDelete ?? false);
				FaEdit = FaEdit || (accessItem?.FaEdit ?? false);
				FaHomeAnalysis = FaHomeAnalysis || (accessItem?.FaHomeAnalysis ?? false);
				FaHomeUpdate = FaHomeUpdate || (accessItem?.FaHomeUpdate ?? false);
				FaPlanningEdit = FaPlanningEdit || (accessItem?.FaPlanningEdit ?? false);
				FaPlanningView = FaPlanningView || (accessItem?.FaPlanningView ?? false);
				FaTechnicEdit = FaTechnicEdit || (accessItem?.FaTechnicEdit ?? false);
				FaTechnicView = FaTechnicView || (accessItem?.FaTechnicView ?? false);
				Fertigung = Fertigung || (accessItem?.Fertigung ?? false);
				OrderProcessing = OrderProcessing || (accessItem?.OrderProcessing ?? false);
				Statistics = Statistics || (accessItem?.Statistics ?? false);
				FAWerkWunshAdmin = FAWerkWunshAdmin || (accessItem?.FAWerkWunshAdmin ?? false);
				CSInfoEdit = CSInfoEdit || (accessItem?.CSInfoEdit ?? false);
				OrderProcessingLog = OrderProcessingLog || (accessItem?.OrderProcessingLog ?? false);
				FertigungLog = FertigungLog || (accessItem?.FertigungLog ?? false);
				UBGStatusChange = UBGStatusChange || (accessItem?.UBGStatusChange ?? false);
				FAAKtualTerminUpdate = FAAKtualTerminUpdate || (accessItem?.FAAKtualTerminUpdate ?? false);
				FAAuswertungEndkontrolle = FAAuswertungEndkontrolle || (accessItem?.FAAuswertungEndkontrolle ?? false);
				FACommissionert = FACommissionert || (accessItem?.FACommissionert ?? false);
				FADrucken = FADrucken || (accessItem?.FADrucken ?? false);
				FAErlidegen = FAErlidegen || (accessItem?.FAErlidegen ?? false);
				FAExcelUpdateWerk = FAExcelUpdateWerk || (accessItem?.FAExcelUpdateWerk ?? false);
				FAExcelUpdateWunsh = FAExcelUpdateWunsh || (accessItem?.FAExcelUpdateWunsh ?? false);
				FAFehlrMaterial = FAFehlrMaterial || (accessItem?.FAFehlrMaterial ?? false);
				FALaufkarteSchneiderei = FALaufkarteSchneiderei || (accessItem?.FALaufkarteSchneiderei ?? false);
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
				FAUpdateByArticle = FAUpdateByArticle || (accessItem?.FAUpdateByArticle ?? false);
				FAUpdateByFA = FAUpdateByFA || (accessItem?.FAUpdateByFA ?? false);
				FAUpdateBemerkungExtern = FAUpdateBemerkungExtern || (accessItem?.FAUpdateBemerkungExtern ?? false);
				StatsBacklogFGAdmin = StatsBacklogFGAdmin || (accessItem?.StatsBacklogFGAdmin ?? false);
				StatsBacklogHWAdmin = StatsBacklogHWAdmin || (accessItem?.StatsBacklogHWAdmin ?? false);
				FAPlannung = FAPlannung || (accessItem?.FAPlannung ?? false);
				FAPlannungTechnick = FAPlannungTechnick || (accessItem?.FAPlannungTechnick ?? false);
				RahmenAdd = RahmenAdd || (accessItem?.RahmenAdd ?? false);
				RahmenAddPositions = RahmenAddPositions || (accessItem?.RahmenAddPositions ?? false);
				RahmenDelete = RahmenDelete || (accessItem?.RahmenDelete ?? false);
				RahmenDeletePositions = RahmenDeletePositions || (accessItem?.RahmenDeletePositions ?? false);
				RahmenDocumentFlow = RahmenDocumentFlow || (accessItem?.RahmenDocumentFlow ?? false);
				RahmenEditHeader = RahmenEditHeader || (accessItem?.RahmenEditHeader ?? false);
				RahmenEditPositions = RahmenEditPositions || (accessItem?.RahmenEditPositions ?? false);
				RahmenHistory = RahmenHistory || (accessItem?.RahmenHistory ?? false);
				Rahmen = Rahmen || (accessItem?.Rahmen ?? false);
				//souilmi 30-05-2022
				GutschriftView = GutschriftView || (accessItem?.GutschriftView ?? false);
				GutschriftCreate = GutschriftCreate || (accessItem?.GutschriftCreate ?? false);
				GutschriftEdit = GutschriftEdit || (accessItem?.GutschriftEdit ?? false);
				GutschriftDelete = GutschriftDelete || (accessItem?.GutschriftDelete ?? false);
				GutschriftReport = GutschriftReport || (accessItem?.GutschriftReport ?? false);
				GutschriftLog = GutschriftLog || (accessItem?.GutschriftLog ?? false);
				GutschriftPositionEdit = GutschriftPositionEdit || (accessItem?.GutschriftPositionEdit ?? false);
				//souilmi 08/06/2022
				RahmenValdation = RahmenValdation || (accessItem?.RahmenValdation ?? false);
				RahmenCancelation = RahmenCancelation || (accessItem?.RahmenCancelation ?? false);
				RahmenClosure = RahmenClosure || (accessItem?.RahmenClosure ?? false);
				// - 2022-11-06
				StatsRechnungAL = StatsRechnungAL || (accessItem?.StatsRechnungAL ?? false);
				StatsRechnungCZ = StatsRechnungCZ || (accessItem?.StatsRechnungCZ ?? false);
				StatsRechnungDE = StatsRechnungDE || (accessItem?.StatsRechnungDE ?? false);
				StatsRechnungTN = StatsRechnungTN || (accessItem?.StatsRechnungTN ?? false);
				StatsRechnungBETN = StatsRechnungBETN || (accessItem?.StatsRechnungBETN ?? false);
				StatsRechnungGZTN = StatsRechnungGZTN || (accessItem?.StatsRechnungGZTN ?? false);
				StatsRechnungWS = StatsRechnungWS || (accessItem?.StatsRechnungWS ?? false);
				// - 2022-11-09
				StatsStockCS = StatsStockCS || (accessItem.StatsStockCS ?? false);
				StatsStockExternalWarehouse = StatsStockCS || (accessItem.StatsStockExternalWarehouse ?? false);
				StatsStockFG = StatsStockCS || (accessItem.StatsStockFG ?? false);
				//
				DelforCreate = DelforCreate || (accessItem?.DelforCreate ?? false);
				DelforReport = DelforReport || (accessItem?.DelforReport ?? false);
				DelforView = DelforView || (accessItem?.DelforView ?? false);
				DelforOrderConfirmation = DelforOrderConfirmation || (accessItem?.DelforOrderConfirmation ?? false);
				//souilmi 09/11/2022
				DelforDelete = DelforDelete || (accessItem?.DelforDelete ?? false);
				DelforDeletePosition = DelforDeletePosition || (accessItem?.DelforDeletePosition ?? false);
				//souilmi 09/09/2022
				ForcastView = ForcastView || (accessItem?.ForcastView ?? false);
				ForcastCreate = ForcastCreate || (accessItem?.ForcastCreate ?? false);
				ForcastEdit = ForcastEdit || (accessItem?.ForcastEdit ?? false);
				ForcastDelete = ForcastDelete || (accessItem?.ForcastDelete ?? false);
				ForcastReport = ForcastReport || (accessItem?.ForcastReport ?? false);
				ForcastLog = ForcastLog || (accessItem?.ForcastLog ?? false);
				ForcastPositionEdit = ForcastPositionEdit || (accessItem?.ForcastPositionEdit ?? false);
				//souilmi 20/01/2023
				Rechnung = Rechnung || (accessItem?.Rechnung ?? false);
				RechnungAutoCreation = RechnungAutoCreation || (accessItem?.RechnungAutoCreation ?? false);
				RechnungManualCreation = RechnungManualCreation || (accessItem?.RechnungManualCreation ?? false);
				RechnungValidate = RechnungValidate || (accessItem?.RechnungValidate ?? false);
				RechnungDelete = RechnungDelete || (accessItem?.RechnungDelete ?? false);
				RechnungSend = RechnungSend || (accessItem?.RechnungSend ?? false);
				RechnungReport = RechnungReport || (accessItem?.RechnungReport ?? false);
				RechnungConfig = RechnungConfig || (accessItem?.RechnungConfig ?? false);
				//souilmi 01/08/2023
				FACreateHorizon1 = FACreateHorizon1 || (accessItem?.FACreateHorizon1 ?? false);
				FACreateHorizon2 = FACreateHorizon2 || (accessItem?.FACreateHorizon2 ?? false);
				FACreateHorizon3 = FACreateHorizon3 || (accessItem?.FACreateHorizon3 ?? false);
				FAUpdateTerminHorizon1 = FAUpdateTerminHorizon1 || (accessItem?.FAUpdateTerminHorizon1 ?? false);
				FAUpdateTerminHorizon2 = FAUpdateTerminHorizon2 || (accessItem?.FAUpdateTerminHorizon2 ?? false);
				FAUpdateTerminHorizon3 = FAUpdateTerminHorizon3 || (accessItem?.FAUpdateTerminHorizon3 ?? false);
				// - 2023-08-07
				StatsCapaHorizons = StatsCapaHorizons || (accessItem?.StatsCapaHorizons ?? false);
				StatsCapaPlanning = StatsCapaPlanning || (accessItem?.StatsCapaPlanning ?? false);
				StatsCapaLong = StatsCapaLong || (accessItem?.StatsCapaLong ?? false);
				StatsCapaShort = StatsCapaShort || (accessItem?.StatsCapaShort ?? false);
				StatsCapaCutting = StatsCapaCutting || (accessItem?.StatsCapaCutting ?? false);
				// souilmi 15/08/2023
				FACancelHorizon1 = FACancelHorizon1 || (accessItem?.FACancelHorizon1 ?? false);
				FACancelHorizon2 = FACancelHorizon2 || (accessItem?.FACancelHorizon2 ?? false);
				FACancelHorizon3 = FACancelHorizon3 || (accessItem?.FACancelHorizon3 ?? false);
				ABPosHorizon1 = ABPosHorizon1 || (accessItem?.ABPosHorizon1 ?? false);
				ABPosHorizon2 = ABPosHorizon2 || (accessItem?.ABPosHorizon2 ?? false);
				ABPosHorizon3 = ABPosHorizon3 || (accessItem?.ABPosHorizon3 ?? false);
				GSPosHorizon1 = GSPosHorizon1 || (accessItem?.GSPosHorizon1 ?? false);
				GSPosHorizon2 = GSPosHorizon2 || (accessItem?.GSPosHorizon2 ?? false);
				GSPosHorizon3 = GSPosHorizon3 || (accessItem?.GSPosHorizon3 ?? false);
				LSPosHorizon1 = LSPosHorizon1 || (accessItem?.LSPosHorizon1 ?? false);
				LSPosHorizon2 = LSPosHorizon2 || (accessItem?.LSPosHorizon2 ?? false);
				LSPosHorizon3 = LSPosHorizon3 || (accessItem?.LSPosHorizon3 ?? false);
				RAPosHorizon1 = RAPosHorizon1 || (accessItem?.RAPosHorizon1 ?? false);
				RAPosHorizon2 = RAPosHorizon2 || (accessItem?.RAPosHorizon2 ?? false);
				RAPosHorizon3 = RAPosHorizon3 || (accessItem?.RAPosHorizon3 ?? false);
				RGPosHorizon1 = RGPosHorizon1 || (accessItem?.RGPosHorizon1 ?? false);
				RGPosHorizon2 = RGPosHorizon2 || (accessItem?.RGPosHorizon2 ?? false);
				RGPosHorizon3 = RGPosHorizon3 || (accessItem?.RGPosHorizon3 ?? false);
				DLFPosHorizon1 = DLFPosHorizon1 || (accessItem?.DLFPosHorizon1 ?? false);
				DLFPosHorizon2 = DLFPosHorizon2 || (accessItem?.DLFPosHorizon2 ?? false);
				DLFPosHorizon3 = DLFPosHorizon3 || (accessItem?.DLFPosHorizon3 ?? false);
				FRCPosHorizon1 = FRCPosHorizon1 || (accessItem?.FRCPosHorizon1 ?? false);
				FRCPosHorizon2 = FRCPosHorizon2 || (accessItem?.FRCPosHorizon2 ?? false);
				FRCPosHorizon3 = FRCPosHorizon3 || (accessItem?.FRCPosHorizon3 ?? false);
				//souilmi 26/09/2023
				AB_LT = AB_LT || (accessItem?.AB_LT ?? false);
				AB_LT_EDI = AB_LT_EDI || (accessItem?.AB_LT_EDI ?? false);
				RahmenAddAB = RahmenAddAB || (accessItem?.RahmenAddAB ?? false);
				BVFaCreate = BVFaCreate || (accessItem?.BVFaCreate ?? false);
				DelforStatistics = DelforStatistics || (accessItem?.DelforStatistics ?? false);
				//souilmi 03/10/2023
				FATerminWerk = FATerminWerk || (accessItem?.FATerminWerk ?? false);
				FABemerkungPlannug = FABemerkungPlannug || (accessItem?.FABemerkungPlannug ?? false);
				FABemerkungZuPrio = FABemerkungZuPrio || (accessItem?.FABemerkungZuPrio ?? false);
				FABemerkungZuGewerk = FABemerkungZuGewerk || (accessItem?.FABemerkungZuGewerk ?? false);
				// -
				ConfirmationDoneEdit = ConfirmationDoneEdit || (accessItem.ConfirmationDoneEdit ?? false);
				ConfirmationBookedEdit = ConfirmationBookedEdit || (accessItem.ConfirmationBookedEdit ?? false);
				DeliveryNoteDoneEdit = DeliveryNoteDoneEdit || (accessItem.DeliveryNoteDoneEdit ?? false);
				DeliveryNoteBookedEdit = DeliveryNoteBookedEdit || (accessItem.DeliveryNoteBookedEdit ?? false);
				GutschriftDoneEdit = GutschriftDoneEdit || (accessItem.GutschriftDoneEdit ?? false);
				GutschriftBookedEdit = GutschriftBookedEdit || (accessItem.GutschriftBookedEdit ?? false);
				BVDoneEdit = BVDoneEdit || (accessItem.BVDoneEdit ?? false);
				BVBookedEdit = BVBookedEdit || (accessItem.BVBookedEdit ?? false);
				RechnungDoneEdit = RechnungDoneEdit || (accessItem.RechnungDoneEdit ?? false);
				RechnungBookedEdit = RechnungBookedEdit || (accessItem.RechnungBookedEdit ?? false);
				InsideSalesChecks = InsideSalesChecks || (accessItem.InsideSalesChecks ?? false);
				InsideSalesChecksArchive = InsideSalesChecksArchive || (accessItem.InsideSalesChecksArchive ?? false);
				InsideSalesCustomerSummary = InsideSalesCustomerSummary || (accessItem.InsideSalesCustomerSummary ?? false);
				//--inside sales overview
				InsideSalesOverdueOrders = InsideSalesOverdueOrders || (accessItem.InsideSalesOverdueOrders ?? false);
				InsideSalesTurnoverCurrentWeek = InsideSalesTurnoverCurrentWeek || (accessItem.InsideSalesTurnoverCurrentWeek ?? false);
				InsideSalesTotalUnbookedOrders = InsideSalesTotalUnbookedOrders || (accessItem.InsideSalesTotalUnbookedOrders ?? false);
				InsideSalesMinimumStockEvaluation = InsideSalesMinimumStockEvaluation || (accessItem.InsideSalesMinimumStockEvaluation ?? false);
				InsideSalesOverdueOrdersTable = InsideSalesOverdueOrdersTable || (accessItem.InsideSalesOverdueOrdersTable ?? false);
				InsideSalesTotalUnbookedOrdersTable = InsideSalesTotalUnbookedOrdersTable || (accessItem.InsideSalesTotalUnbookedOrdersTable ?? false);
				InsideSalesMinimumStockEvaluationTable = InsideSalesMinimumStockEvaluationTable || (accessItem.InsideSalesMinimumStockEvaluationTable ?? false);
			}
		}
		public CustomerServiceAccessModel(CustomerServiceAccessModel entity)
		{

			Id = entity?.Id ?? -1;
			ModuleActivated = entity?.ModuleActivated ?? false;
			Administration = entity?.Administration ?? false;
			Configuration = entity?.Configuration ?? false;
			ConfigurationAppoitments = entity?.ConfigurationAppoitments ?? false;
			ConfigurationChangeEmployees = entity?.ConfigurationChangeEmployees ?? false;
			ConfigurationReplacements = entity?.ConfigurationReplacements ?? false;
			ConfigurationReporting = entity?.ConfigurationReporting ?? false;
			ConfirmationCreate = entity?.ConfirmationCreate ?? false;
			ConfirmationDelete = entity?.ConfirmationDelete ?? false;
			ConfirmationDeliveryNote = entity?.ConfirmationDeliveryNote ?? false;
			ConfirmationEdit = entity?.ConfirmationEdit ?? false;
			ConfirmationPositionEdit = entity?.ConfirmationPositionEdit ?? false;
			ConfirmationPositionProduction = entity?.ConfirmationPositionProduction ?? false;
			ConfirmationReport = entity?.ConfirmationReport ?? false;
			ConfirmationValidate = entity?.ConfirmationValidate ?? false;
			ConfirmationView = entity?.ConfirmationView ?? false;
			DeliveryNoteCreate = entity?.DeliveryNoteCreate ?? false;
			DeliveryNoteDelete = entity?.DeliveryNoteDelete ?? false;
			DeliveryNoteEdit = entity?.DeliveryNoteEdit ?? false;
			DeliveryNoteLog = entity?.DeliveryNoteLog ?? false;
			DeliveryNotePositionEdit = entity?.DeliveryNotePositionEdit ?? false;
			DeliveryNoteReport = entity?.DeliveryNoteReport ?? false;
			DeliveryNoteView = entity?.DeliveryNoteView ?? false;
			EDI = entity?.EDI ?? false;
			EDIDownloadFile = entity?.EDIDownloadFile ?? false;
			EDIError = entity?.EDIError ?? false;
			EDIErrorEdit = entity?.EDIErrorEdit ?? false;
			EDIErrorValidated = entity?.EDIErrorValidated ?? false;
			EDILogOrderValidated = entity?.EDILogOrderValidated ?? false;
			EDIOrder = entity?.EDIOrder ?? false;
			EDIOrderEdit = entity?.EDIOrderEdit ?? false;
			EDIOrderPositionEdit = entity?.EDIOrderPositionEdit ?? false;
			EDIOrderProduction = entity?.EDIOrderProduction ?? false;
			EDIOrderProductionPosition = entity?.EDIOrderProductionPosition ?? false;
			EDIOrderReport = entity?.EDIOrderReport ?? false;
			EDIOrderValidated = entity?.EDIOrderValidated ?? false;
			EDIOrderValidatedEdit = entity?.EDIOrderValidatedEdit ?? false;
			FaABEdit = entity?.FaABEdit ?? false;
			FaABView = entity?.FaABView ?? false;
			FaActionBook = entity?.FaActionBook ?? false;
			FaActionComplete = entity?.FaActionComplete ?? false;
			FaActionDelete = entity?.FaActionDelete ?? false;
			FaActionPrint = entity?.FaActionPrint ?? false;
			FaAdmin = entity?.FaAdmin ?? false;
			FaAnalysis = entity?.FaAnalysis ?? false;
			FaCreate = entity?.FaCreate ?? false;
			FaDatenEdit = entity?.FaDatenEdit ?? false;
			FaDatenView = entity?.FaDatenView ?? false;
			FaDelete = entity?.FaDelete ?? false;
			FaEdit = entity?.FaEdit ?? false;
			FaHomeAnalysis = entity?.FaHomeAnalysis ?? false;
			FaHomeUpdate = entity?.FaHomeUpdate ?? false;
			FaPlanningEdit = entity?.FaPlanningEdit ?? false;
			FaPlanningView = entity?.FaPlanningView ?? false;
			FaTechnicEdit = entity?.FaTechnicEdit ?? false;
			FaTechnicView = entity?.FaTechnicView ?? false;
			Fertigung = entity?.Fertigung ?? false;
			OrderProcessing = entity?.OrderProcessing ?? false;
			Statistics = entity?.Statistics ?? false;
			FAWerkWunshAdmin = entity?.FAWerkWunshAdmin ?? false;
			CSInfoEdit = entity?.CSInfoEdit ?? false;
			OrderProcessingLog = entity?.OrderProcessingLog ?? false;
			FertigungLog = entity?.FertigungLog ?? false;
			UBGStatusChange = entity.UBGStatusChange;
			FAAKtualTerminUpdate = entity.FAAKtualTerminUpdate;
			FAAuswertungEndkontrolle = entity.FAAuswertungEndkontrolle;
			FACommissionert = entity.FACommissionert;
			FADrucken = entity.FADrucken;
			FAErlidegen = entity.FAErlidegen;
			FAExcelUpdateWerk = entity.FAExcelUpdateWerk;
			FAExcelUpdateWunsh = entity.FAExcelUpdateWunsh;
			FAFehlrMaterial = entity.FAFehlrMaterial;
			FALaufkarteSchneiderei = entity.FALaufkarteSchneiderei;
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
			FAUpdateByArticle = entity.FAUpdateByArticle;
			FAUpdateByFA = entity.FAUpdateByFA;
			FAUpdateBemerkungExtern = entity.FAUpdateBemerkungExtern;
			StatsBacklogFGAdmin = entity.StatsBacklogFGAdmin;
			StatsBacklogHWAdmin = entity.StatsBacklogHWAdmin;
			FAPlannung = entity.FAPlannung;
			FAPlannungTechnick = entity.FAPlannungTechnick;
			RahmenAdd = entity.RahmenAdd;
			RahmenAddPositions = entity.RahmenAddPositions;
			RahmenDelete = entity.RahmenDelete;
			RahmenDeletePositions = entity.RahmenDeletePositions;
			RahmenDocumentFlow = entity.RahmenDocumentFlow;
			RahmenEditHeader = entity.RahmenEditHeader;
			RahmenEditPositions = entity.RahmenEditPositions;
			RahmenHistory = entity.RahmenHistory;
			Rahmen = entity.Rahmen;
			//souilmi 30-05-2022
			GutschriftView = entity.GutschriftView;
			GutschriftCreate = entity.GutschriftCreate;
			GutschriftEdit = entity.GutschriftEdit;
			GutschriftDelete = entity.GutschriftDelete;
			GutschriftReport = entity.GutschriftReport;
			GutschriftLog = entity.GutschriftLog;
			GutschriftPositionEdit = entity.GutschriftPositionEdit;
			//souilmi 08/06/2022
			RahmenValdation = entity.RahmenValdation;
			RahmenCancelation = entity.RahmenCancelation;
			RahmenClosure = entity.RahmenClosure;
			// - 2022-11-06
			StatsRechnungAL = entity.StatsRechnungAL;
			StatsRechnungCZ = entity.StatsRechnungCZ;
			StatsRechnungDE = entity.StatsRechnungDE;
			StatsRechnungTN = entity.StatsRechnungTN;
			StatsRechnungBETN = entity.StatsRechnungBETN;
			StatsRechnungGZTN = entity.StatsRechnungGZTN;
			StatsRechnungWS = entity.StatsRechnungWS;
			// - 2022-11-09
			StatsStockCS = entity.StatsStockCS;
			StatsStockExternalWarehouse = entity.StatsStockExternalWarehouse;
			StatsStockFG = entity.StatsStockFG;
			//souilmi 23/09/2022
			DelforCreate = entity.DelforCreate;
			DelforReport = entity.DelforReport;
			DelforView = entity.DelforView;
			DelforOrderConfirmation = entity.DelforOrderConfirmation;
			//souilmi 09/11/2022
			DelforDelete = entity.DelforDelete;
			DelforDeletePosition = entity.DelforDeletePosition;
			ForcastView = entity.ForcastView;
			ForcastCreate = entity.ForcastCreate;
			ForcastEdit = entity.ForcastEdit;
			ForcastDelete = entity.ForcastDelete;
			ForcastReport = entity.ForcastReport;
			ForcastLog = entity.ForcastLog;
			ForcastPositionEdit = entity.ForcastPositionEdit;
			//souilmi 20/01/2023
			Rechnung = entity.Rechnung;
			RechnungAutoCreation = entity.RechnungAutoCreation;
			RechnungManualCreation = entity.RechnungManualCreation;
			RechnungValidate = entity.RechnungValidate;
			RechnungDelete = entity.RechnungDelete;
			RechnungSend = entity.RechnungSend;
			RechnungReport = entity.RechnungReport;
			RechnungConfig = entity.RechnungConfig;
			//souilmi 01/08/2023
			FACreateHorizon1 = entity.FACreateHorizon1;
			FACreateHorizon2 = entity.FACreateHorizon2;
			FACreateHorizon3 = entity.FACreateHorizon3;
			FAUpdateTerminHorizon1 = entity.FAUpdateTerminHorizon1;
			FAUpdateTerminHorizon2 = entity.FAUpdateTerminHorizon2;
			FAUpdateTerminHorizon3 = entity.FAUpdateTerminHorizon3;
			// - 2023-08-07
			StatsCapaHorizons = entity.StatsCapaHorizons;
			StatsCapaPlanning = entity.StatsCapaPlanning;
			StatsCapaLong = entity.StatsCapaLong;
			StatsCapaShort = entity.StatsCapaShort;
			StatsCapaCutting = entity.StatsCapaCutting;
			// souilmi 15/08/2023
			FACancelHorizon1 = entity.FACancelHorizon1;
			FACancelHorizon2 = entity.FACancelHorizon2;
			FACancelHorizon3 = entity.FACancelHorizon3;
			ABPosHorizon1 = entity.ABPosHorizon1;
			ABPosHorizon2 = entity.ABPosHorizon2;
			ABPosHorizon3 = entity.ABPosHorizon3;
			GSPosHorizon1 = entity.GSPosHorizon1;
			GSPosHorizon2 = entity.GSPosHorizon2;
			GSPosHorizon3 = entity.GSPosHorizon3;
			LSPosHorizon1 = entity.LSPosHorizon1;
			LSPosHorizon2 = entity.LSPosHorizon2;
			LSPosHorizon3 = entity.LSPosHorizon3;
			RAPosHorizon1 = entity.RAPosHorizon1;
			RAPosHorizon2 = entity.RAPosHorizon2;
			RAPosHorizon3 = entity.RAPosHorizon3;
			RGPosHorizon1 = entity.RGPosHorizon1;
			RGPosHorizon2 = entity.RGPosHorizon2;
			RGPosHorizon3 = entity.RGPosHorizon3;
			DLFPosHorizon1 = entity.DLFPosHorizon1;
			DLFPosHorizon2 = entity.DLFPosHorizon2;
			DLFPosHorizon3 = entity.DLFPosHorizon3;
			FRCPosHorizon1 = entity.FRCPosHorizon1;
			FRCPosHorizon2 = entity.FRCPosHorizon2;
			FRCPosHorizon3 = entity.FRCPosHorizon3;
			//souilmi 26/09/2023
			AB_LT = entity.AB_LT;
			AB_LT_EDI = entity.AB_LT_EDI;
			RahmenAddAB = entity.RahmenAddAB;
			BVFaCreate = entity.BVFaCreate;
			DelforStatistics = entity.DelforStatistics;
			//souilmi 03/10/2023
			FATerminWerk = entity.FATerminWerk;
			FABemerkungPlannug = entity.FABemerkungPlannug;
			FABemerkungZuPrio = entity.FABemerkungZuPrio;
			FABemerkungZuGewerk = entity.FABemerkungZuGewerk;
			// -
			ConfirmationDoneEdit = entity.ConfirmationDoneEdit;
			ConfirmationBookedEdit = entity.ConfirmationBookedEdit;
			DeliveryNoteDoneEdit = entity.DeliveryNoteDoneEdit;
			DeliveryNoteBookedEdit = entity.DeliveryNoteBookedEdit;
			GutschriftDoneEdit = entity.GutschriftDoneEdit;
			GutschriftBookedEdit = entity.GutschriftBookedEdit;
			BVDoneEdit = entity.BVDoneEdit;
			BVBookedEdit = entity.BVBookedEdit;
			RechnungDoneEdit = entity.RechnungDoneEdit;
			RechnungBookedEdit = entity.RechnungBookedEdit;
			InsideSalesChecks = entity.InsideSalesChecks;
			InsideSalesChecksArchive = entity.InsideSalesChecksArchive;
			InsideSalesCustomerSummary = entity.InsideSalesCustomerSummary;
			//-- inside sales overview
			InsideSalesOverdueOrders = entity.InsideSalesOverdueOrders;
			InsideSalesTurnoverCurrentWeek = entity.InsideSalesTurnoverCurrentWeek;
			InsideSalesTotalUnbookedOrders = entity.InsideSalesTotalUnbookedOrders;
			InsideSalesMinimumStockEvaluation = entity.InsideSalesMinimumStockEvaluation;
			InsideSalesOverdueOrdersTable = entity.InsideSalesOverdueOrdersTable;
			InsideSalesTotalUnbookedOrdersTable = entity.InsideSalesTotalUnbookedOrdersTable;
			InsideSalesMinimumStockEvaluationTable = entity.InsideSalesMinimumStockEvaluationTable;
		}


		public Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity ToDbEntity(int id, int mainId)
		{
			return new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity
			{
				Id = id,
				ModuleActivated = ModuleActivated,
				AccessProfileName = AccessProfileName,
				CreationUserId = CreationUserId,
				CreationTime = CreationTime,
				Administration = Administration,
				Configuration = Configuration,
				ConfigurationAppoitments = ConfigurationAppoitments,
				ConfigurationChangeEmployees = ConfigurationChangeEmployees,
				ConfigurationReplacements = ConfigurationReplacements,
				ConfigurationReporting = ConfigurationReporting,
				ConfirmationCreate = ConfirmationCreate,
				ConfirmationDelete = ConfirmationDelete,
				ConfirmationDeliveryNote = ConfirmationDeliveryNote,
				ConfirmationEdit = ConfirmationEdit,
				ConfirmationPositionEdit = ConfirmationPositionEdit,
				ConfirmationPositionProduction = ConfirmationPositionProduction,
				ConfirmationReport = ConfirmationReport,
				ConfirmationValidate = ConfirmationValidate,
				ConfirmationView = ConfirmationView,
				DeliveryNoteCreate = DeliveryNoteCreate,
				DeliveryNoteDelete = DeliveryNoteDelete,
				DeliveryNoteEdit = DeliveryNoteEdit,
				DeliveryNoteLog = DeliveryNoteLog,
				DeliveryNotePositionEdit = DeliveryNotePositionEdit,
				DeliveryNoteReport = DeliveryNoteReport,
				DeliveryNoteView = DeliveryNoteView,
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
				FaAnalysis = FaAnalysis,
				FaCreate = FaCreate,
				FaDatenEdit = FaDatenEdit,
				FaDatenView = FaDatenView,
				FaDelete = FaDelete,
				FaEdit = FaEdit,
				FaHomeAnalysis = FaHomeAnalysis,
				FaHomeUpdate = FaHomeUpdate,
				FaPlanningEdit = FaPlanningEdit,
				FaPlanningView = FaPlanningView,
				FaTechnicEdit = FaTechnicEdit,
				FaTechnicView = FaTechnicView,
				Fertigung = Fertigung,
				OrderProcessing = OrderProcessing,
				Statistics = Statistics,
				FAWerkWunshAdmin = FAWerkWunshAdmin,
				CSInfoEdit = CSInfoEdit,
				OrderProcessingLog = OrderProcessingLog,
				FertigungLog = FertigungLog,
				UBGStatusChange = UBGStatusChange,
				FAAKtualTerminUpdate = FAAKtualTerminUpdate,
				FAAuswertungEndkontrolle = FAAuswertungEndkontrolle,
				FACommissionert = FACommissionert,
				FADrucken = FADrucken,
				FAErlidegen = FAErlidegen,
				FAExcelUpdateWerk = FAExcelUpdateWerk,
				FAExcelUpdateWunsh = FAExcelUpdateWunsh,
				FAFehlrMaterial = FAFehlrMaterial,
				FALaufkarteSchneiderei = FALaufkarteSchneiderei,
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
				FAUpdateByArticle = FAUpdateByArticle,
				FAUpdateByFA = FAUpdateByFA,
				FAUpdateBemerkungExtern = FAUpdateBemerkungExtern,
				StatsBacklogFGAdmin = StatsBacklogFGAdmin,
				StatsBacklogHWAdmin = StatsBacklogHWAdmin,
				RahmenAdd = RahmenAdd,
				RahmenAddPositions = RahmenAddPositions,
				RahmenDelete = RahmenDelete,
				RahmenDeletePositions = RahmenDeletePositions,
				RahmenDocumentFlow = RahmenDocumentFlow,
				RahmenEditHeader = RahmenEditHeader,
				RahmenEditPositions = RahmenEditPositions,
				RahmenHistory = RahmenHistory,
				Rahmen = Rahmen,
				//souilmi 30-05-2022
				GutschriftView = GutschriftView,
				GutschriftCreate = GutschriftCreate,
				GutschriftEdit = GutschriftEdit,
				GutschriftDelete = GutschriftDelete,
				GutschriftReport = GutschriftReport,
				GutschriftLog = GutschriftLog,
				GutschriftPositionEdit = GutschriftPositionEdit,
				//souilmi 08/06/2022
				RahmenValdation = RahmenValdation,
				RahmenCancelation = RahmenCancelation,
				RahmenClosure = RahmenClosure,
				//souilmi 09/09/2022
				// - 2022-11-06
				StatsRechnungAL = StatsRechnungAL,
				StatsRechnungCZ = StatsRechnungCZ,
				StatsRechnungDE = StatsRechnungDE,
				StatsRechnungTN = StatsRechnungTN,
				StatsRechnungBETN = StatsRechnungBETN,
				StatsRechnungGZTN = StatsRechnungGZTN,
				StatsRechnungWS = StatsRechnungWS,
				StatsStockCS = StatsStockCS,
				StatsStockExternalWarehouse = StatsStockExternalWarehouse,
				StatsStockFG = StatsStockFG,
				//souilmi 23/09/2022
				DelforCreate = DelforCreate,
				DelforReport = DelforReport,
				DelforView = DelforView,
				DelforOrderConfirmation = DelforOrderConfirmation,
				//souilmi 09/11/2022
				DelforDelete = DelforDelete,
				DelforDeletePosition = DelforDeletePosition,
				ForcastView = ForcastView,
				ForcastCreate = ForcastCreate,
				ForcastEdit = ForcastEdit,
				ForcastDelete = ForcastDelete,
				ForcastReport = ForcastReport,
				ForcastLog = ForcastLog,
				ForcastPositionEdit = ForcastPositionEdit,
				//souilmi 20/01/2023
				Rechnung = Rechnung,
				RechnungAutoCreation = RechnungAutoCreation,
				RechnungManualCreation = RechnungManualCreation,
				RechnungValidate = RechnungValidate,
				RechnungDelete = RechnungDelete,
				RechnungSend = RechnungSend,
				RechnungReport = RechnungReport,
				RechnungConfig = RechnungConfig,
				//souilmi 01/08/2023
				FACreateHorizon1 = FACreateHorizon1,
				FACreateHorizon2 = FACreateHorizon2,
				FACreateHorizon3 = FACreateHorizon3,
				FAUpdateTerminHorizon1 = FAUpdateTerminHorizon1,
				FAUpdateTerminHorizon2 = FAUpdateTerminHorizon2,
				FAUpdateTerminHorizon3 = FAUpdateTerminHorizon3,
				// - 2023-08-07
				StatsCapaHorizons = StatsCapaHorizons,
				StatsCapaPlanning = StatsCapaPlanning,
				StatsCapaLong = StatsCapaLong,
				StatsCapaShort = StatsCapaShort,
				StatsCapaCutting = StatsCapaCutting,
				// souilmi 15/08/2023
				FACancelHorizon1 = FACancelHorizon1,
				FACancelHorizon2 = FACancelHorizon2,
				FACancelHorizon3 = FACancelHorizon3,
				ABPosHorizon1 = ABPosHorizon1,
				ABPosHorizon2 = ABPosHorizon2,
				ABPosHorizon3 = ABPosHorizon3,
				GSPosHorizon1 = GSPosHorizon1,
				GSPosHorizon2 = GSPosHorizon2,
				GSPosHorizon3 = GSPosHorizon3,
				LSPosHorizon1 = LSPosHorizon1,
				LSPosHorizon2 = LSPosHorizon2,
				LSPosHorizon3 = LSPosHorizon3,
				RAPosHorizon1 = RAPosHorizon1,
				RAPosHorizon2 = RAPosHorizon2,
				RAPosHorizon3 = RAPosHorizon3,
				RGPosHorizon1 = RGPosHorizon1,
				RGPosHorizon2 = RGPosHorizon2,
				RGPosHorizon3 = RGPosHorizon3,
				DLFPosHorizon1 = DLFPosHorizon1,
				DLFPosHorizon2 = DLFPosHorizon2,
				DLFPosHorizon3 = DLFPosHorizon3,
				FRCPosHorizon1 = FRCPosHorizon1,
				FRCPosHorizon2 = FRCPosHorizon2,
				FRCPosHorizon3 = FRCPosHorizon3,
				//souilmi 26/09/2023
				AB_LT = AB_LT,
				AB_LT_EDI = AB_LT_EDI,
				RahmenAddAB = RahmenAddAB,
				BVFaCreate = BVFaCreate,
				DelforStatistics = DelforStatistics,
				//souilmi 03/10/2023
				FATerminWerk = FATerminWerk,
				FABemerkungPlannug = FABemerkungPlannug,
				FABemerkungZuPrio = FABemerkungZuPrio,
				FABemerkungZuGewerk = FABemerkungZuGewerk,
				InsideSalesChecks = InsideSalesChecks,
				InsideSalesChecksArchive = InsideSalesChecksArchive,
				InsideSalesCustomerSummary = InsideSalesCustomerSummary,
				//-- inside sales overview
				InsideSalesOverdueOrders = InsideSalesOverdueOrders,
				InsideSalesTurnoverCurrentWeek = InsideSalesTurnoverCurrentWeek,
				InsideSalesTotalUnbookedOrders = InsideSalesTotalUnbookedOrders,
				InsideSalesMinimumStockEvaluation = InsideSalesMinimumStockEvaluation,
				InsideSalesOverdueOrdersTable = InsideSalesOverdueOrdersTable,
				InsideSalesTotalUnbookedOrdersTable = InsideSalesTotalUnbookedOrdersTable,
				InsideSalesMinimumStockEvaluationTable = InsideSalesMinimumStockEvaluationTable,
			};
		}
	}
}
