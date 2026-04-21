using System;

namespace Psz.Core.CustomerService.Models.Administration.AccessProfiles
{
	public class AccessProfileModel
	{
		public string AccessProfileName { get; set; }
		public int Id { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public bool? ModuleActivated { get; set; }

		public bool? Administration { get; set; }
		public bool? Configuration { get; set; }
		public bool? ConfigurationAppoitments { get; set; }
		public bool? ConfigurationChangeEmployees { get; set; }
		public bool? ConfigurationReplacements { get; set; }
		public bool? ConfigurationReporting { get; set; }
		public bool? ConfirmationCreate { get; set; }
		public bool? ConfirmationDelete { get; set; }
		public bool? ConfirmationDeliveryNote { get; set; }
		public bool? ConfirmationEdit { get; set; }
		public bool? ConfirmationPositionEdit { get; set; }
		public bool? ConfirmationPositionProduction { get; set; }
		public bool? ConfirmationReport { get; set; }
		public bool? ConfirmationValidate { get; set; }
		public bool? ConfirmationView { get; set; }
		public bool? DeliveryNoteCreate { get; set; }
		public bool? DeliveryNoteDelete { get; set; }
		public bool? DeliveryNoteEdit { get; set; }
		public bool? DeliveryNoteLog { get; set; }
		public bool? DeliveryNotePositionEdit { get; set; }
		public bool? DeliveryNoteReport { get; set; }
		public bool? DeliveryNoteView { get; set; }
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
		public bool? FaAnalysis { get; set; }
		public bool? FaCreate { get; set; }
		public bool? FaDatenEdit { get; set; }
		public bool? FaDatenView { get; set; }
		public bool? FaDelete { get; set; }
		public bool? FaEdit { get; set; }
		public bool? FaHomeAnalysis { get; set; }
		public bool? FaHomeUpdate { get; set; }
		public bool? FaPlanningEdit { get; set; }
		public bool? FaPlanningView { get; set; }
		public bool? FaTechnicEdit { get; set; }
		public bool? FaTechnicView { get; set; }
		public bool? Fertigung { get; set; }
		public bool? OrderProcessing { get; set; }
		public bool? Statistics { get; set; }
		public bool? FAWerkWunshAdmin { get; set; }
		public bool? CSInfoEdit { get; set; }
		public bool? OrderProcessingLog { get; set; }
		public bool? FertigungLog { get; set; }
		public bool? UBGStatusChange { get; set; }
		public bool? FAAKtualTerminUpdate { get; set; }
		public bool? FAAuswertungEndkontrolle { get; set; }
		public bool? FACommissionert { get; set; }
		public bool? FADrucken { get; set; }
		public bool? FAErlidegen { get; set; }
		public bool? FAExcelUpdateWerk { get; set; }
		public bool? FAExcelUpdateWunsh { get; set; }
		public bool? FAFehlrMaterial { get; set; }
		public bool? FALaufkarteSchneiderei { get; set; }
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
		public bool? FAUpdateByArticle { get; set; }
		public bool? FAUpdateByFA { get; set; }
		public bool? FAUpdateBemerkungExtern { get; set; }
		public bool? StatsBacklogFGAdmin { get; set; }
		public bool? StatsBacklogHWAdmin { get; set; }
		public bool? RahmenAdd { get; set; }
		public bool? RahmenAddPositions { get; set; }
		public bool? RahmenDelete { get; set; }
		public bool? RahmenDeletePositions { get; set; }
		public bool? RahmenDocumentFlow { get; set; }
		public bool? RahmenEditHeader { get; set; }
		public bool? RahmenEditPositions { get; set; }
		public bool? RahmenHistory { get; set; }
		public bool? Rahmen { get; set; }
		//souilmi 30/05/2022
		public bool? GutschriftView { get; set; }
		public bool? GutschriftCreate { get; set; }
		public bool? GutschriftEdit { get; set; }
		public bool? GutschriftDelete { get; set; }
		public bool? GutschriftReport { get; set; }
		public bool? GutschriftLog { get; set; }
		public bool? GutschriftPositionEdit { get; set; }
		//souilmi 08/06/2022
		public bool? RahmenValdation { get; set; }
		public bool? RahmenCancelation { get; set; }
		public bool? RahmenClosure { get; set; }
		// - 2022-11-06
		public bool StatsRechnungAL { get; set; }
		public bool StatsRechnungCZ { get; set; }
		public bool StatsRechnungDE { get; set; }
		public bool StatsRechnungTN { get; set; }
		public bool StatsRechnungBETN { get; set; }
		public bool StatsRechnungGZTN { get; set; }
		public bool StatsRechnungWS { get; set; }
		//souilmi 09/09/2022
		public bool? ForcastView { get; set; }
		public bool? ForcastCreate { get; set; }
		public bool? ForcastEdit { get; set; }
		public bool? ForcastDelete { get; set; }
		public bool? ForcastReport { get; set; }
		public bool? ForcastLog { get; set; }
		public bool? ForcastPositionEdit { get; set; }
		//souilmi 23/09/2022
		public bool? DelforCreate { get; set; }
		public bool? DelforReport { get; set; }
		public bool? DelforView { get; set; }
		public bool? DelforOrderConfirmation { get; set; }
		//souilmi 09/11/2022
		public bool? DelforDelete { get; set; }
		public bool? DelforDeletePosition { get; set; }
		// - 2022-1-09
		public bool StatsStockCS { get; set; }
		public bool StatsStockExternalWarehouse { get; set; }
		public bool StatsStockFG { get; set; }
		public bool? Rechnung { get; set; }
		public bool? RechnungAutoCreation { get; set; }
		public bool? RechnungManualCreation { get; set; }
		public bool? RechnungValidate { get; set; }
		public bool? RechnungDelete { get; set; }
		public bool? RechnungSend { get; set; }
		public bool? RechnungReport { get; set; }
		public bool? RechnungConfig { get; set; }
		public bool? isDefault { get; set; }
		//souilmi 01/08/2023
		public bool? FACreateHorizon1 { get; set; }
		public bool? FACreateHorizon2 { get; set; }
		public bool? FACreateHorizon3 { get; set; }
		public bool? FAUpdateTerminHorizon1 { get; set; }
		public bool? FAUpdateTerminHorizon2 { get; set; }
		public bool? FAUpdateTerminHorizon3 { get; set; }
		// - 2023-08-07
		public bool? StatsCapaHorizons { get; set; }
		public bool? StatsCapaPlanning { get; set; }
		public bool? StatsCapaLong { get; set; }
		public bool? StatsCapaShort { get; set; }
		public bool? StatsCapaCutting { get; set; }
		// souilmi 15/08/2023
		public bool? FACancelHorizon1 { get; set; }
		public bool? FACancelHorizon2 { get; set; }
		public bool? FACancelHorizon3 { get; set; }
		public bool? ABPosHorizon1 { get; set; }
		public bool? ABPosHorizon2 { get; set; }
		public bool? ABPosHorizon3 { get; set; }
		public bool? GSPosHorizon1 { get; set; }
		public bool? GSPosHorizon2 { get; set; }
		public bool? GSPosHorizon3 { get; set; }
		public bool? LSPosHorizon1 { get; set; }
		public bool? LSPosHorizon2 { get; set; }
		public bool? LSPosHorizon3 { get; set; }
		public bool? RAPosHorizon1 { get; set; }
		public bool? RAPosHorizon2 { get; set; }
		public bool? RAPosHorizon3 { get; set; }
		public bool? RGPosHorizon1 { get; set; }
		public bool? RGPosHorizon2 { get; set; }
		public bool? RGPosHorizon3 { get; set; }
		public bool? DLFPosHorizon1 { get; set; }
		public bool? DLFPosHorizon2 { get; set; }
		public bool? DLFPosHorizon3 { get; set; }
		public bool? FRCPosHorizon1 { get; set; }
		public bool? FRCPosHorizon2 { get; set; }
		public bool? FRCPosHorizon3 { get; set; }
		//souilmi 26/09/2023
		public bool? AB_LT { get; set; }
		public bool? AB_LT_EDI { get; set; }
		public bool? RahmenAddAB { get; set; }
		public bool? BVFaCreate { get; set; }
		public bool? DelforStatistics { get; set; }
		//souilmi 03/10/2023
		public bool? FATerminWerk { get; set; }
		public bool? FABemerkungPlannug { get; set; }
		public bool? FABemerkungZuPrio { get; set; }
		public bool? FABemerkungZuGewerk { get; set; }
		// -
		public bool? ConfirmationDoneEdit { get; set; }
		public bool? ConfirmationBookedEdit { get; set; }
		public bool? DeliveryNoteDoneEdit { get; set; }
		public bool? DeliveryNoteBookedEdit { get; set; }
		public bool? GutschriftDoneEdit { get; set; }
		public bool? GutschriftBookedEdit { get; set; }
		public bool? BVDoneEdit { get; set; }
		public bool? BVBookedEdit { get; set; }
		public bool? RechnungDoneEdit { get; set; }
		public bool? RechnungBookedEdit { get; set; }
		public bool? InsideSalesChecksArchive { get; set; }
		public bool? InsideSalesChecks { get; set; }
		public bool? InsideSalesCustomerSummary { get; set; }

		//-- inside sales overview
		public bool? InsideSalesOverdueOrders { get; set; }
		public bool? InsideSalesTurnoverCurrentWeek { get; set; }
		public bool? InsideSalesTotalUnbookedOrders { get; set; }
		public bool? InsideSalesMinimumStockEvaluation { get; set; }
		public bool? InsideSalesOverdueOrdersTable { get; set; }
		public bool? InsideSalesTotalUnbookedOrdersTable { get; set; }
		public bool? InsideSalesMinimumStockEvaluationTable { get; set; }
		public AccessProfileModel(Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity accessProfileEntity)
		{
			if(accessProfileEntity == null)
				return;
			// -
			Id = accessProfileEntity.Id;
			ModuleActivated = accessProfileEntity.ModuleActivated;
			AccessProfileName = accessProfileEntity.AccessProfileName;
			CreationUserId = accessProfileEntity.CreationUserId;
			CreationTime = accessProfileEntity.CreationTime;
			Administration = accessProfileEntity.Administration;
			Configuration = accessProfileEntity.Configuration;
			ConfigurationAppoitments = accessProfileEntity.ConfigurationAppoitments;
			ConfigurationChangeEmployees = accessProfileEntity.ConfigurationChangeEmployees;
			ConfigurationReplacements = accessProfileEntity.ConfigurationReplacements;
			ConfigurationReporting = accessProfileEntity.ConfigurationReporting;
			ConfirmationCreate = accessProfileEntity.ConfirmationCreate;
			ConfirmationDelete = accessProfileEntity.ConfirmationDelete;
			ConfirmationDeliveryNote = accessProfileEntity.ConfirmationDeliveryNote;
			ConfirmationEdit = accessProfileEntity.ConfirmationEdit;
			ConfirmationPositionEdit = accessProfileEntity.ConfirmationPositionEdit;
			ConfirmationPositionProduction = accessProfileEntity.ConfirmationPositionProduction;
			ConfirmationReport = accessProfileEntity.ConfirmationReport;
			ConfirmationValidate = accessProfileEntity.ConfirmationValidate;
			ConfirmationView = accessProfileEntity.ConfirmationView;
			DeliveryNoteCreate = accessProfileEntity.DeliveryNoteCreate;
			DeliveryNoteDelete = accessProfileEntity.DeliveryNoteDelete;
			DeliveryNoteEdit = accessProfileEntity.DeliveryNoteEdit;
			DeliveryNoteLog = accessProfileEntity.DeliveryNoteLog;
			DeliveryNotePositionEdit = accessProfileEntity.DeliveryNotePositionEdit;
			DeliveryNoteReport = accessProfileEntity.DeliveryNoteReport;
			DeliveryNoteView = accessProfileEntity.DeliveryNoteView;
			EDI = accessProfileEntity.EDI;
			EDIDownloadFile = accessProfileEntity.EDIDownloadFile;
			EDIError = accessProfileEntity.EDIError;
			EDIErrorEdit = accessProfileEntity.EDIErrorEdit;
			EDIErrorValidated = accessProfileEntity.EDIErrorValidated;
			EDILogOrderValidated = accessProfileEntity.EDILogOrderValidated;
			EDIOrder = accessProfileEntity.EDIOrder;
			EDIOrderEdit = accessProfileEntity.EDIOrderEdit;
			EDIOrderPositionEdit = accessProfileEntity.EDIOrderPositionEdit;
			EDIOrderProduction = accessProfileEntity.EDIOrderProduction;
			EDIOrderProductionPosition = accessProfileEntity.EDIOrderProductionPosition;
			EDIOrderReport = accessProfileEntity.EDIOrderReport;
			EDIOrderValidated = accessProfileEntity.EDIOrderValidated;
			EDIOrderValidatedEdit = accessProfileEntity.EDIOrderValidatedEdit;
			FaABEdit = accessProfileEntity.FaABEdit;
			FaABView = accessProfileEntity.FaABView;
			FaActionBook = accessProfileEntity.FaActionBook;
			FaActionComplete = accessProfileEntity.FaActionComplete;
			FaActionDelete = accessProfileEntity.FaActionDelete;
			FaActionPrint = accessProfileEntity.FaActionPrint;
			FaAdmin = accessProfileEntity.FaAdmin;
			FaAnalysis = accessProfileEntity.FaAnalysis;
			FaCreate = accessProfileEntity.FaCreate;
			FaDatenEdit = accessProfileEntity.FaDatenEdit;
			FaDatenView = accessProfileEntity.FaDatenView;
			FaDelete = accessProfileEntity.FaDelete;
			FaEdit = accessProfileEntity.FaEdit;
			FaHomeAnalysis = accessProfileEntity.FaHomeAnalysis;
			FaHomeUpdate = accessProfileEntity.FaHomeUpdate;
			FaPlanningEdit = accessProfileEntity.FaPlanningEdit;
			FaPlanningView = accessProfileEntity.FaPlanningView;
			FaTechnicEdit = accessProfileEntity.FaTechnicEdit;
			FaTechnicView = accessProfileEntity.FaTechnicView;
			Fertigung = accessProfileEntity.Fertigung;
			OrderProcessing = accessProfileEntity.OrderProcessing;
			Statistics = accessProfileEntity.Statistics;
			FAWerkWunshAdmin = accessProfileEntity.FAWerkWunshAdmin;
			CSInfoEdit = accessProfileEntity.CSInfoEdit;
			OrderProcessingLog = accessProfileEntity.OrderProcessingLog;
			FertigungLog = accessProfileEntity.FertigungLog;
			UBGStatusChange = accessProfileEntity.UBGStatusChange;
			FAUpdateByArticle = accessProfileEntity.FAUpdateByArticle;
			FAStappleDruck = accessProfileEntity.FAStappleDruck;
			FAExcelUpdateWerk = accessProfileEntity.FAExcelUpdateWerk;
			FAExcelUpdateWunsh = accessProfileEntity.FAExcelUpdateWunsh;
			FACommissionert = accessProfileEntity.FACommissionert;
			FAProductionPlannung = accessProfileEntity.FAProductionPlannung;
			FAAKtualTerminUpdate = accessProfileEntity.FAAKtualTerminUpdate;
			FAPriesZeitUpdate = accessProfileEntity.FAPriesZeitUpdate;
			FAUpdateByFA = accessProfileEntity.FAUpdateByFA;
			FAFehlrMaterial = accessProfileEntity.FAFehlrMaterial;
			FAStorno = accessProfileEntity.FAStorno;
			FAErlidegen = accessProfileEntity.FAErlidegen;
			FADrucken = accessProfileEntity.FADrucken;
			FAStucklist = accessProfileEntity.FAStucklist;
			FAAuswertungEndkontrolle = accessProfileEntity.FAAuswertungEndkontrolle;
			FALaufkarteSchneiderei = accessProfileEntity.FALaufkarteSchneiderei;
			FAStatusAlbania = accessProfileEntity.FAStatusAlbania;
			FAStatusCzech = accessProfileEntity.FAStatusCzech;
			FAStatusTunisia = accessProfileEntity.FAStatusTunisia;
			FAPlannung = accessProfileEntity.FAPlannung;
			FAPlannungTechnick = accessProfileEntity.FAPlannungTechnick;
			RahmenAdd = accessProfileEntity.RahmenAdd;
			RahmenAddPositions = accessProfileEntity.RahmenAddPositions;
			RahmenDelete = accessProfileEntity.RahmenDelete;
			RahmenDeletePositions = accessProfileEntity.RahmenDeletePositions;
			RahmenDocumentFlow = accessProfileEntity.RahmenDocumentFlow;
			RahmenEditHeader = accessProfileEntity.RahmenEditHeader;
			RahmenEditPositions = accessProfileEntity.RahmenEditPositions;
			RahmenHistory = accessProfileEntity.RahmenHistory;
			Rahmen = accessProfileEntity.Rahmen ?? false;
			FAUpdateBemerkungExtern = accessProfileEntity.FAUpdateBemerkungExtern;
			StatsBacklogFGAdmin = accessProfileEntity.StatsBacklogFGAdmin;
			StatsBacklogHWAdmin = accessProfileEntity.StatsBacklogHWAdmin;
			//souilmi 30/05/2022
			GutschriftView = accessProfileEntity.GutschriftView;
			GutschriftCreate = accessProfileEntity.GutschriftCreate;
			GutschriftEdit = accessProfileEntity.GutschriftEdit;
			GutschriftDelete = accessProfileEntity.GutschriftDelete;
			GutschriftReport = accessProfileEntity.GutschriftReport;
			GutschriftLog = accessProfileEntity.GutschriftLog;
			GutschriftPositionEdit = accessProfileEntity.GutschriftPositionEdit;
			//souilmi 08/06/2022
			RahmenValdation = accessProfileEntity.RahmenValdation;
			RahmenCancelation = accessProfileEntity.RahmenCancelation;
			RahmenClosure = accessProfileEntity.RahmenClosure;
			// - 2022-11-06
			StatsRechnungAL = accessProfileEntity.StatsRechnungAL ?? false;
			StatsRechnungCZ = accessProfileEntity.StatsRechnungCZ ?? false;
			StatsRechnungDE = accessProfileEntity.StatsRechnungDE ?? false;
			StatsRechnungTN = accessProfileEntity.StatsRechnungTN ?? false;
			StatsRechnungBETN = accessProfileEntity.StatsRechnungBETN ?? false;
			StatsRechnungGZTN = accessProfileEntity.StatsRechnungGZTN ?? false;
			StatsRechnungWS = accessProfileEntity.StatsRechnungWS ?? false;
			// - 2022-11-09
			StatsStockCS = accessProfileEntity.StatsStockCS ?? false;
			StatsStockExternalWarehouse = accessProfileEntity.StatsStockExternalWarehouse ?? false;
			StatsStockFG = accessProfileEntity.StatsStockFG ?? false;
			DelforCreate = accessProfileEntity.DelforCreate;
			DelforReport = accessProfileEntity.DelforReport;
			DelforView = accessProfileEntity.DelforView;
			DelforOrderConfirmation = accessProfileEntity.DelforOrderConfirmation;
			DelforDelete = accessProfileEntity.DelforDelete;
			DelforDeletePosition = accessProfileEntity.DelforDeletePosition;
			Rechnung = accessProfileEntity.Rechnung;
			RechnungAutoCreation = accessProfileEntity.RechnungAutoCreation;
			RechnungManualCreation = accessProfileEntity.RechnungManualCreation;
			RechnungValidate = accessProfileEntity.RechnungValidate;
			RechnungDelete = accessProfileEntity.RechnungDelete;
			RechnungSend = accessProfileEntity.RechnungSend;
			RechnungReport = accessProfileEntity.RechnungReport;
			RechnungConfig = accessProfileEntity.RechnungConfig;
			isDefault = accessProfileEntity.IsDefault;
			//
			ForcastView = accessProfileEntity.ForcastView;
			ForcastCreate = accessProfileEntity.ForcastCreate;
			ForcastEdit = accessProfileEntity.ForcastEdit;
			ForcastDelete = accessProfileEntity.ForcastDelete;
			ForcastReport = accessProfileEntity.ForcastReport;
			ForcastLog = accessProfileEntity.ForcastLog;
			ForcastPositionEdit = accessProfileEntity.ForcastPositionEdit;
			//souilmi 01/08/2023
			FACreateHorizon1 = accessProfileEntity.FACreateHorizon1;
			FACreateHorizon2 = accessProfileEntity.FACreateHorizon2;
			FACreateHorizon3 = accessProfileEntity.FACreateHorizon3;
			FAUpdateTerminHorizon1 = accessProfileEntity.FAUpdateTerminHorizon1;
			FAUpdateTerminHorizon2 = accessProfileEntity.FAUpdateTerminHorizon2;
			FAUpdateTerminHorizon3 = accessProfileEntity.FAUpdateTerminHorizon3;
			// - 2023-08-07
			StatsCapaHorizons = accessProfileEntity.StatsCapaHorizons;
			StatsCapaPlanning = accessProfileEntity.StatsCapaPlanning;
			StatsCapaLong = accessProfileEntity.StatsCapaLong;
			StatsCapaShort = accessProfileEntity.StatsCapaShort;
			StatsCapaCutting = accessProfileEntity.StatsCapaCutting;
			// souilmi 15/08/2023
			FACancelHorizon1 = accessProfileEntity.FACancelHorizon1;
			FACancelHorizon2 = accessProfileEntity.FACancelHorizon2;
			FACancelHorizon3 = accessProfileEntity.FACancelHorizon3;
			ABPosHorizon1 = accessProfileEntity.ABPosHorizon1;
			ABPosHorizon2 = accessProfileEntity.ABPosHorizon2;
			ABPosHorizon3 = accessProfileEntity.ABPosHorizon3;
			GSPosHorizon1 = accessProfileEntity.GSPosHorizon1;
			GSPosHorizon2 = accessProfileEntity.GSPosHorizon2;
			GSPosHorizon3 = accessProfileEntity.GSPosHorizon3;
			LSPosHorizon1 = accessProfileEntity.LSPosHorizon1;
			LSPosHorizon2 = accessProfileEntity.LSPosHorizon2;
			LSPosHorizon3 = accessProfileEntity.LSPosHorizon3;
			RAPosHorizon1 = accessProfileEntity.RAPosHorizon1;
			RAPosHorizon2 = accessProfileEntity.RAPosHorizon2;
			RAPosHorizon3 = accessProfileEntity.RAPosHorizon3;
			RGPosHorizon1 = accessProfileEntity.RGPosHorizon1;
			RGPosHorizon2 = accessProfileEntity.RGPosHorizon2;
			RGPosHorizon3 = accessProfileEntity.RGPosHorizon3;
			DLFPosHorizon1 = accessProfileEntity.DLFPosHorizon1;
			DLFPosHorizon2 = accessProfileEntity.DLFPosHorizon2;
			DLFPosHorizon3 = accessProfileEntity.DLFPosHorizon3;
			FRCPosHorizon1 = accessProfileEntity.FRCPosHorizon1;
			FRCPosHorizon2 = accessProfileEntity.FRCPosHorizon2;
			FRCPosHorizon3 = accessProfileEntity.FRCPosHorizon3;
			//
			AB_LT = accessProfileEntity.AB_LT;
			AB_LT_EDI = accessProfileEntity.AB_LT_EDI;
			RahmenAddAB = accessProfileEntity.RahmenAddAB;
			BVFaCreate = accessProfileEntity.BVFaCreate;
			DelforStatistics = accessProfileEntity.DelforStatistics;
			//souilmi 03/10/2023
			FATerminWerk = accessProfileEntity.FATerminWerk;
			FABemerkungPlannug = accessProfileEntity.FABemerkungPlannug;
			FABemerkungZuPrio = accessProfileEntity.FABemerkungZuPrio;
			FABemerkungZuGewerk = accessProfileEntity.FABemerkungZuGewerk;
			// -
			ConfirmationDoneEdit = accessProfileEntity.ConfirmationDoneEdit;
			ConfirmationBookedEdit = accessProfileEntity.ConfirmationBookedEdit;
			DeliveryNoteDoneEdit = accessProfileEntity.DeliveryNoteDoneEdit;
			DeliveryNoteBookedEdit = accessProfileEntity.DeliveryNoteBookedEdit;
			GutschriftDoneEdit = accessProfileEntity.GutschriftDoneEdit;
			GutschriftBookedEdit = accessProfileEntity.GutschriftBookedEdit;
			BVDoneEdit = accessProfileEntity.BVDoneEdit;
			BVBookedEdit = accessProfileEntity.BVBookedEdit;
			RechnungDoneEdit = accessProfileEntity.RechnungDoneEdit;
			RechnungBookedEdit = accessProfileEntity.RechnungBookedEdit;
			InsideSalesChecks = accessProfileEntity.InsideSalesChecks;
			InsideSalesChecksArchive = accessProfileEntity.InsideSalesChecksArchive;
			InsideSalesCustomerSummary = accessProfileEntity.InsideSalesCustomerSummary;
			//-- inside sales overview
			InsideSalesOverdueOrders = accessProfileEntity.InsideSalesOverdueOrders;
			InsideSalesTurnoverCurrentWeek = accessProfileEntity.InsideSalesTurnoverCurrentWeek;
			InsideSalesTotalUnbookedOrders = accessProfileEntity.InsideSalesTotalUnbookedOrders;
			InsideSalesMinimumStockEvaluation = accessProfileEntity.InsideSalesMinimumStockEvaluation;
			InsideSalesOverdueOrdersTable = accessProfileEntity.InsideSalesOverdueOrdersTable;
			InsideSalesTotalUnbookedOrdersTable = accessProfileEntity.InsideSalesTotalUnbookedOrdersTable;
			InsideSalesMinimumStockEvaluationTable = accessProfileEntity.InsideSalesMinimumStockEvaluationTable;
		}
		public Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity
			{
				Id = Id,
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
				FAUpdateByArticle = FAUpdateByArticle,
				FAStappleDruck = FAStappleDruck,
				FAExcelUpdateWerk = FAExcelUpdateWerk,
				FAExcelUpdateWunsh = FAExcelUpdateWunsh,
				FACommissionert = FACommissionert,
				FAProductionPlannung = FAProductionPlannung,
				FAAKtualTerminUpdate = FAAKtualTerminUpdate,
				FAPriesZeitUpdate = FAPriesZeitUpdate,
				FAUpdateByFA = FAUpdateByFA,
				FAFehlrMaterial = FAFehlrMaterial,
				FAStorno = FAStorno,
				FAErlidegen = FAErlidegen,
				FADrucken = FADrucken,
				FAStucklist = FAStucklist,
				FAAuswertungEndkontrolle = FAAuswertungEndkontrolle,
				FALaufkarteSchneiderei = FALaufkarteSchneiderei,
				FAStatusAlbania = FAStatusAlbania,
				FAStatusCzech = FAStatusCzech,
				FAStatusTunisia = FAStatusTunisia,
				FAPlannung = FAPlannung,
				FAPlannungTechnick = FAPlannungTechnick,
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
				//souilmi 30/05/2022
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
				ForcastView = ForcastView,
				ForcastCreate = ForcastCreate,
				ForcastEdit = ForcastEdit,
				ForcastDelete = ForcastDelete,
				ForcastReport = ForcastReport,
				ForcastLog = ForcastLog,
				ForcastPositionEdit = ForcastPositionEdit,
				//souilmi 23/09/2022
				DelforCreate = DelforCreate,
				DelforReport = DelforReport,
				DelforView = DelforView,
				DelforOrderConfirmation = DelforOrderConfirmation,
				//souilmi 09/11/2022
				DelforDelete = DelforDelete,
				DelforDeletePosition = DelforDeletePosition,
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
				//souilmi 20/01/2023
				Rechnung = Rechnung,
				RechnungAutoCreation = RechnungAutoCreation,
				RechnungManualCreation = RechnungManualCreation,
				RechnungValidate = RechnungValidate,
				RechnungDelete = RechnungDelete,
				RechnungSend = RechnungSend,
				RechnungReport = RechnungReport,
				RechnungConfig = RechnungConfig,
				IsDefault = isDefault,
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
				// -
				ConfirmationDoneEdit = ConfirmationDoneEdit,
				ConfirmationBookedEdit = ConfirmationBookedEdit,
				DeliveryNoteDoneEdit = DeliveryNoteDoneEdit,
				DeliveryNoteBookedEdit = DeliveryNoteBookedEdit,
				GutschriftDoneEdit = GutschriftDoneEdit,
				GutschriftBookedEdit = GutschriftBookedEdit,
				BVDoneEdit = BVDoneEdit,
				BVBookedEdit = BVBookedEdit,
				RechnungDoneEdit = RechnungDoneEdit,
				RechnungBookedEdit = RechnungBookedEdit,
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
