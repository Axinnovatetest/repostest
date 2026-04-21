using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_AccessProfile] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTS_AccessProfile] ([AB_LT],[AB_LT_EDI],[ABPosHorizon1],[ABPosHorizon2],[ABPosHorizon3],[AccessProfileName],[Administration],[BVBookedEdit],[BVDoneEdit],[BVFaCreate],[Configuration],[ConfigurationAppoitments],[ConfigurationChangeEmployees],[ConfigurationReplacements],[ConfigurationReporting],[ConfirmationBookedEdit],[ConfirmationCreate],[ConfirmationDelete],[ConfirmationDeliveryNote],[ConfirmationDoneEdit],[ConfirmationEdit],[ConfirmationPositionEdit],[ConfirmationPositionProduction],[ConfirmationReport],[ConfirmationValidate],[ConfirmationView],[CreationTime],[CreationUserId],[CSInfoEdit],[DelforCreate],[DelforDelete],[DelforDeletePosition],[DelforOrderConfirmation],[DelforReport],[DelforStatistics],[DelforView],[DeliveryNoteBookedEdit],[DeliveryNoteCreate],[DeliveryNoteDelete],[DeliveryNoteDoneEdit],[DeliveryNoteEdit],[DeliveryNoteLog],[DeliveryNotePositionEdit],[DeliveryNoteReport],[DeliveryNoteView],[DLFPosHorizon1],[DLFPosHorizon2],[DLFPosHorizon3],[EDI],[EDIDownloadFile],[EDIError],[EDIErrorEdit],[EDIErrorValidated],[EDILogOrderValidated],[EDIOrder],[EDIOrderEdit],[EDIOrderPositionEdit],[EDIOrderProduction],[EDIOrderProductionPosition],[EDIOrderReport],[EDIOrderValidated],[EDIOrderValidatedEdit],[FaABEdit],[FaABView],[FaActionBook],[FaActionComplete],[FaActionDelete],[FaActionPrint],[FaAdmin],[FAAKtualTerminUpdate],[FaAnalysis],[FAAuswertungEndkontrolle],[FABemerkungPlannug],[FABemerkungZuGewerk],[FABemerkungZuPrio],[FACancelHorizon1],[FACancelHorizon2],[FACancelHorizon3],[FACommissionert],[FaCreate],[FACreateHorizon1],[FACreateHorizon2],[FACreateHorizon3],[FaDatenEdit],[FaDatenView],[FaDelete],[FADrucken],[FaEdit],[FAErlidegen],[FAExcelUpdateWerk],[FAExcelUpdateWunsh],[FAFehlrMaterial],[FaHomeAnalysis],[FaHomeUpdate],[FALaufkarteSchneiderei],[FaPlanningEdit],[FaPlanningView],[FAPlannung],[FAPlannungTechnick],[FAPriesZeitUpdate],[FAProductionPlannung],[FAStappleDruck],[FAStatusAlbania],[FAStatusCzech],[FAStatusTunisia],[FAStorno],[FAStucklist],[FaTechnicEdit],[FaTechnicView],[FATerminWerk],[FAUpdateBemerkungExtern],[FAUpdateByArticle],[FAUpdateByFA],[FAUpdateTerminHorizon1],[FAUpdateTerminHorizon2],[FAUpdateTerminHorizon3],[FAWerkWunshAdmin],[Fertigung],[FertigungLog],[ForcastCreate],[ForcastDelete],[ForcastEdit],[ForcastLog],[ForcastPositionEdit],[ForcastReport],[ForcastView],[FRCPosHorizon1],[FRCPosHorizon2],[FRCPosHorizon3],[GSPosHorizon1],[GSPosHorizon2],[GSPosHorizon3],[GutschriftBookedEdit],[GutschriftCreate],[GutschriftDelete],[GutschriftDoneEdit],[GutschriftEdit],[GutschriftLog],[GutschriftPositionEdit],[GutschriftReport],[GutschriftView],[InsideSalesChecks],[InsideSalesChecksArchive],[InsideSalesCustomerSummary],[InsideSalesMinimumStockEvaluation],[InsideSalesMinimumStockEvaluationTable],[InsideSalesOverdueOrders],[InsideSalesOverdueOrdersTable],[InsideSalesTotalUnbookedOrders],[InsideSalesTotalUnbookedOrdersTable],[InsideSalesTurnoverCurrentWeek],[IsDefault],[LSPosHorizon1],[LSPosHorizon2],[LSPosHorizon3],[mId],[ModuleActivated],[OrderProcessing],[OrderProcessingLog],[Rahmen],[RahmenAdd],[RahmenAddAB],[RahmenAddPositions],[RahmenCancelation],[RahmenClosure],[RahmenDelete],[RahmenDeletePositions],[RahmenDocumentFlow],[RahmenEditHeader],[RahmenEditPositions],[RahmenHistory],[RahmenValdation],[RAPosHorizon1],[RAPosHorizon2],[RAPosHorizon3],[Rechnung],[RechnungAutoCreation],[RechnungBookedEdit],[RechnungConfig],[RechnungDelete],[RechnungDoneEdit],[RechnungManualCreation],[RechnungReport],[RechnungSend],[RechnungValidate],[RGPosHorizon1],[RGPosHorizon2],[RGPosHorizon3],[Statistics],[StatsBacklogFGAdmin],[StatsBacklogHWAdmin],[StatsCapaCutting],[StatsCapaHorizons],[StatsCapaLong],[StatsCapaPlanning],[StatsCapaShort],[StatsRechnungAL],[StatsRechnungBETN],[StatsRechnungCZ],[StatsRechnungDE],[StatsRechnungGZTN],[StatsRechnungTN],[StatsRechnungWS],[StatsStockCS],[StatsStockExternalWarehouse],[StatsStockFG],[UBGStatusChange]) OUTPUT INSERTED.[Id] VALUES (@AB_LT,@AB_LT_EDI,@ABPosHorizon1,@ABPosHorizon2,@ABPosHorizon3,@AccessProfileName,@Administration,@BVBookedEdit,@BVDoneEdit,@BVFaCreate,@Configuration,@ConfigurationAppoitments,@ConfigurationChangeEmployees,@ConfigurationReplacements,@ConfigurationReporting,@ConfirmationBookedEdit,@ConfirmationCreate,@ConfirmationDelete,@ConfirmationDeliveryNote,@ConfirmationDoneEdit,@ConfirmationEdit,@ConfirmationPositionEdit,@ConfirmationPositionProduction,@ConfirmationReport,@ConfirmationValidate,@ConfirmationView,@CreationTime,@CreationUserId,@CSInfoEdit,@DelforCreate,@DelforDelete,@DelforDeletePosition,@DelforOrderConfirmation,@DelforReport,@DelforStatistics,@DelforView,@DeliveryNoteBookedEdit,@DeliveryNoteCreate,@DeliveryNoteDelete,@DeliveryNoteDoneEdit,@DeliveryNoteEdit,@DeliveryNoteLog,@DeliveryNotePositionEdit,@DeliveryNoteReport,@DeliveryNoteView,@DLFPosHorizon1,@DLFPosHorizon2,@DLFPosHorizon3,@EDI,@EDIDownloadFile,@EDIError,@EDIErrorEdit,@EDIErrorValidated,@EDILogOrderValidated,@EDIOrder,@EDIOrderEdit,@EDIOrderPositionEdit,@EDIOrderProduction,@EDIOrderProductionPosition,@EDIOrderReport,@EDIOrderValidated,@EDIOrderValidatedEdit,@FaABEdit,@FaABView,@FaActionBook,@FaActionComplete,@FaActionDelete,@FaActionPrint,@FaAdmin,@FAAKtualTerminUpdate,@FaAnalysis,@FAAuswertungEndkontrolle,@FABemerkungPlannug,@FABemerkungZuGewerk,@FABemerkungZuPrio,@FACancelHorizon1,@FACancelHorizon2,@FACancelHorizon3,@FACommissionert,@FaCreate,@FACreateHorizon1,@FACreateHorizon2,@FACreateHorizon3,@FaDatenEdit,@FaDatenView,@FaDelete,@FADrucken,@FaEdit,@FAErlidegen,@FAExcelUpdateWerk,@FAExcelUpdateWunsh,@FAFehlrMaterial,@FaHomeAnalysis,@FaHomeUpdate,@FALaufkarteSchneiderei,@FaPlanningEdit,@FaPlanningView,@FAPlannung,@FAPlannungTechnick,@FAPriesZeitUpdate,@FAProductionPlannung,@FAStappleDruck,@FAStatusAlbania,@FAStatusCzech,@FAStatusTunisia,@FAStorno,@FAStucklist,@FaTechnicEdit,@FaTechnicView,@FATerminWerk,@FAUpdateBemerkungExtern,@FAUpdateByArticle,@FAUpdateByFA,@FAUpdateTerminHorizon1,@FAUpdateTerminHorizon2,@FAUpdateTerminHorizon3,@FAWerkWunshAdmin,@Fertigung,@FertigungLog,@ForcastCreate,@ForcastDelete,@ForcastEdit,@ForcastLog,@ForcastPositionEdit,@ForcastReport,@ForcastView,@FRCPosHorizon1,@FRCPosHorizon2,@FRCPosHorizon3,@GSPosHorizon1,@GSPosHorizon2,@GSPosHorizon3,@GutschriftBookedEdit,@GutschriftCreate,@GutschriftDelete,@GutschriftDoneEdit,@GutschriftEdit,@GutschriftLog,@GutschriftPositionEdit,@GutschriftReport,@GutschriftView,@InsideSalesChecks,@InsideSalesChecksArchive,@InsideSalesCustomerSummary,@InsideSalesMinimumStockEvaluation,@InsideSalesMinimumStockEvaluationTable,@InsideSalesOverdueOrders,@InsideSalesOverdueOrdersTable,@InsideSalesTotalUnbookedOrders,@InsideSalesTotalUnbookedOrdersTable,@InsideSalesTurnoverCurrentWeek,@IsDefault,@LSPosHorizon1,@LSPosHorizon2,@LSPosHorizon3,@mId,@ModuleActivated,@OrderProcessing,@OrderProcessingLog,@Rahmen,@RahmenAdd,@RahmenAddAB,@RahmenAddPositions,@RahmenCancelation,@RahmenClosure,@RahmenDelete,@RahmenDeletePositions,@RahmenDocumentFlow,@RahmenEditHeader,@RahmenEditPositions,@RahmenHistory,@RahmenValdation,@RAPosHorizon1,@RAPosHorizon2,@RAPosHorizon3,@Rechnung,@RechnungAutoCreation,@RechnungBookedEdit,@RechnungConfig,@RechnungDelete,@RechnungDoneEdit,@RechnungManualCreation,@RechnungReport,@RechnungSend,@RechnungValidate,@RGPosHorizon1,@RGPosHorizon2,@RGPosHorizon3,@Statistics,@StatsBacklogFGAdmin,@StatsBacklogHWAdmin,@StatsCapaCutting,@StatsCapaHorizons,@StatsCapaLong,@StatsCapaPlanning,@StatsCapaShort,@StatsRechnungAL,@StatsRechnungBETN,@StatsRechnungCZ,@StatsRechnungDE,@StatsRechnungGZTN,@StatsRechnungTN,@StatsRechnungWS,@StatsStockCS,@StatsStockExternalWarehouse,@StatsStockFG,@UBGStatusChange); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB_LT", item.AB_LT == null ? (object)DBNull.Value : item.AB_LT);
					sqlCommand.Parameters.AddWithValue("AB_LT_EDI", item.AB_LT_EDI == null ? (object)DBNull.Value : item.AB_LT_EDI);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon1", item.ABPosHorizon1 == null ? (object)DBNull.Value : item.ABPosHorizon1);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon2", item.ABPosHorizon2 == null ? (object)DBNull.Value : item.ABPosHorizon2);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon3", item.ABPosHorizon3 == null ? (object)DBNull.Value : item.ABPosHorizon3);
					sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("BVBookedEdit", item.BVBookedEdit == null ? (object)DBNull.Value : item.BVBookedEdit);
					sqlCommand.Parameters.AddWithValue("BVDoneEdit", item.BVDoneEdit == null ? (object)DBNull.Value : item.BVDoneEdit);
					sqlCommand.Parameters.AddWithValue("BVFaCreate", item.BVFaCreate == null ? (object)DBNull.Value : item.BVFaCreate);
					sqlCommand.Parameters.AddWithValue("Configuration", item.Configuration == null ? (object)DBNull.Value : item.Configuration);
					sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments", item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
					sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees", item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
					sqlCommand.Parameters.AddWithValue("ConfigurationReplacements", item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
					sqlCommand.Parameters.AddWithValue("ConfigurationReporting", item.ConfigurationReporting == null ? (object)DBNull.Value : item.ConfigurationReporting);
					sqlCommand.Parameters.AddWithValue("ConfirmationBookedEdit", item.ConfirmationBookedEdit == null ? (object)DBNull.Value : item.ConfirmationBookedEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationCreate", item.ConfirmationCreate == null ? (object)DBNull.Value : item.ConfirmationCreate);
					sqlCommand.Parameters.AddWithValue("ConfirmationDelete", item.ConfirmationDelete == null ? (object)DBNull.Value : item.ConfirmationDelete);
					sqlCommand.Parameters.AddWithValue("ConfirmationDeliveryNote", item.ConfirmationDeliveryNote == null ? (object)DBNull.Value : item.ConfirmationDeliveryNote);
					sqlCommand.Parameters.AddWithValue("ConfirmationDoneEdit", item.ConfirmationDoneEdit == null ? (object)DBNull.Value : item.ConfirmationDoneEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationEdit", item.ConfirmationEdit == null ? (object)DBNull.Value : item.ConfirmationEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationPositionEdit", item.ConfirmationPositionEdit == null ? (object)DBNull.Value : item.ConfirmationPositionEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationPositionProduction", item.ConfirmationPositionProduction == null ? (object)DBNull.Value : item.ConfirmationPositionProduction);
					sqlCommand.Parameters.AddWithValue("ConfirmationReport", item.ConfirmationReport == null ? (object)DBNull.Value : item.ConfirmationReport);
					sqlCommand.Parameters.AddWithValue("ConfirmationValidate", item.ConfirmationValidate == null ? (object)DBNull.Value : item.ConfirmationValidate);
					sqlCommand.Parameters.AddWithValue("ConfirmationView", item.ConfirmationView == null ? (object)DBNull.Value : item.ConfirmationView);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CSInfoEdit", item.CSInfoEdit == null ? (object)DBNull.Value : item.CSInfoEdit);
					sqlCommand.Parameters.AddWithValue("DelforCreate", item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
					sqlCommand.Parameters.AddWithValue("DelforDelete", item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
					sqlCommand.Parameters.AddWithValue("DelforDeletePosition", item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
					sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation", item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
					sqlCommand.Parameters.AddWithValue("DelforReport", item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
					sqlCommand.Parameters.AddWithValue("DelforStatistics", item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
					sqlCommand.Parameters.AddWithValue("DelforView", item.DelforView == null ? (object)DBNull.Value : item.DelforView);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteBookedEdit", item.DeliveryNoteBookedEdit == null ? (object)DBNull.Value : item.DeliveryNoteBookedEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteCreate", item.DeliveryNoteCreate == null ? (object)DBNull.Value : item.DeliveryNoteCreate);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteDelete", item.DeliveryNoteDelete == null ? (object)DBNull.Value : item.DeliveryNoteDelete);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteDoneEdit", item.DeliveryNoteDoneEdit == null ? (object)DBNull.Value : item.DeliveryNoteDoneEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteEdit", item.DeliveryNoteEdit == null ? (object)DBNull.Value : item.DeliveryNoteEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteLog", item.DeliveryNoteLog == null ? (object)DBNull.Value : item.DeliveryNoteLog);
					sqlCommand.Parameters.AddWithValue("DeliveryNotePositionEdit", item.DeliveryNotePositionEdit == null ? (object)DBNull.Value : item.DeliveryNotePositionEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteReport", item.DeliveryNoteReport == null ? (object)DBNull.Value : item.DeliveryNoteReport);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteView", item.DeliveryNoteView == null ? (object)DBNull.Value : item.DeliveryNoteView);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon1", item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon2", item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon3", item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
					sqlCommand.Parameters.AddWithValue("EDI", item.EDI == null ? (object)DBNull.Value : item.EDI);
					sqlCommand.Parameters.AddWithValue("EDIDownloadFile", item.EDIDownloadFile == null ? (object)DBNull.Value : item.EDIDownloadFile);
					sqlCommand.Parameters.AddWithValue("EDIError", item.EDIError == null ? (object)DBNull.Value : item.EDIError);
					sqlCommand.Parameters.AddWithValue("EDIErrorEdit", item.EDIErrorEdit == null ? (object)DBNull.Value : item.EDIErrorEdit);
					sqlCommand.Parameters.AddWithValue("EDIErrorValidated", item.EDIErrorValidated == null ? (object)DBNull.Value : item.EDIErrorValidated);
					sqlCommand.Parameters.AddWithValue("EDILogOrderValidated", item.EDILogOrderValidated == null ? (object)DBNull.Value : item.EDILogOrderValidated);
					sqlCommand.Parameters.AddWithValue("EDIOrder", item.EDIOrder == null ? (object)DBNull.Value : item.EDIOrder);
					sqlCommand.Parameters.AddWithValue("EDIOrderEdit", item.EDIOrderEdit == null ? (object)DBNull.Value : item.EDIOrderEdit);
					sqlCommand.Parameters.AddWithValue("EDIOrderPositionEdit", item.EDIOrderPositionEdit == null ? (object)DBNull.Value : item.EDIOrderPositionEdit);
					sqlCommand.Parameters.AddWithValue("EDIOrderProduction", item.EDIOrderProduction == null ? (object)DBNull.Value : item.EDIOrderProduction);
					sqlCommand.Parameters.AddWithValue("EDIOrderProductionPosition", item.EDIOrderProductionPosition == null ? (object)DBNull.Value : item.EDIOrderProductionPosition);
					sqlCommand.Parameters.AddWithValue("EDIOrderReport", item.EDIOrderReport == null ? (object)DBNull.Value : item.EDIOrderReport);
					sqlCommand.Parameters.AddWithValue("EDIOrderValidated", item.EDIOrderValidated == null ? (object)DBNull.Value : item.EDIOrderValidated);
					sqlCommand.Parameters.AddWithValue("EDIOrderValidatedEdit", item.EDIOrderValidatedEdit == null ? (object)DBNull.Value : item.EDIOrderValidatedEdit);
					sqlCommand.Parameters.AddWithValue("FaABEdit", item.FaABEdit == null ? (object)DBNull.Value : item.FaABEdit);
					sqlCommand.Parameters.AddWithValue("FaABView", item.FaABView == null ? (object)DBNull.Value : item.FaABView);
					sqlCommand.Parameters.AddWithValue("FaActionBook", item.FaActionBook == null ? (object)DBNull.Value : item.FaActionBook);
					sqlCommand.Parameters.AddWithValue("FaActionComplete", item.FaActionComplete == null ? (object)DBNull.Value : item.FaActionComplete);
					sqlCommand.Parameters.AddWithValue("FaActionDelete", item.FaActionDelete == null ? (object)DBNull.Value : item.FaActionDelete);
					sqlCommand.Parameters.AddWithValue("FaActionPrint", item.FaActionPrint == null ? (object)DBNull.Value : item.FaActionPrint);
					sqlCommand.Parameters.AddWithValue("FaAdmin", item.FaAdmin == null ? (object)DBNull.Value : item.FaAdmin);
					sqlCommand.Parameters.AddWithValue("FAAKtualTerminUpdate", item.FAAKtualTerminUpdate == null ? (object)DBNull.Value : item.FAAKtualTerminUpdate);
					sqlCommand.Parameters.AddWithValue("FaAnalysis", item.FaAnalysis == null ? (object)DBNull.Value : item.FaAnalysis);
					sqlCommand.Parameters.AddWithValue("FAAuswertungEndkontrolle", item.FAAuswertungEndkontrolle == null ? (object)DBNull.Value : item.FAAuswertungEndkontrolle);
					sqlCommand.Parameters.AddWithValue("FABemerkungPlannug", item.FABemerkungPlannug == null ? (object)DBNull.Value : item.FABemerkungPlannug);
					sqlCommand.Parameters.AddWithValue("FABemerkungZuGewerk", item.FABemerkungZuGewerk == null ? (object)DBNull.Value : item.FABemerkungZuGewerk);
					sqlCommand.Parameters.AddWithValue("FABemerkungZuPrio", item.FABemerkungZuPrio == null ? (object)DBNull.Value : item.FABemerkungZuPrio);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon1", item.FACancelHorizon1 == null ? (object)DBNull.Value : item.FACancelHorizon1);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon2", item.FACancelHorizon2 == null ? (object)DBNull.Value : item.FACancelHorizon2);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon3", item.FACancelHorizon3 == null ? (object)DBNull.Value : item.FACancelHorizon3);
					sqlCommand.Parameters.AddWithValue("FACommissionert", item.FACommissionert == null ? (object)DBNull.Value : item.FACommissionert);
					sqlCommand.Parameters.AddWithValue("FaCreate", item.FaCreate == null ? (object)DBNull.Value : item.FaCreate);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon1", item.FACreateHorizon1 == null ? (object)DBNull.Value : item.FACreateHorizon1);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon2", item.FACreateHorizon2 == null ? (object)DBNull.Value : item.FACreateHorizon2);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon3", item.FACreateHorizon3 == null ? (object)DBNull.Value : item.FACreateHorizon3);
					sqlCommand.Parameters.AddWithValue("FaDatenEdit", item.FaDatenEdit == null ? (object)DBNull.Value : item.FaDatenEdit);
					sqlCommand.Parameters.AddWithValue("FaDatenView", item.FaDatenView == null ? (object)DBNull.Value : item.FaDatenView);
					sqlCommand.Parameters.AddWithValue("FaDelete", item.FaDelete == null ? (object)DBNull.Value : item.FaDelete);
					sqlCommand.Parameters.AddWithValue("FADrucken", item.FADrucken == null ? (object)DBNull.Value : item.FADrucken);
					sqlCommand.Parameters.AddWithValue("FaEdit", item.FaEdit == null ? (object)DBNull.Value : item.FaEdit);
					sqlCommand.Parameters.AddWithValue("FAErlidegen", item.FAErlidegen == null ? (object)DBNull.Value : item.FAErlidegen);
					sqlCommand.Parameters.AddWithValue("FAExcelUpdateWerk", item.FAExcelUpdateWerk == null ? (object)DBNull.Value : item.FAExcelUpdateWerk);
					sqlCommand.Parameters.AddWithValue("FAExcelUpdateWunsh", item.FAExcelUpdateWunsh == null ? (object)DBNull.Value : item.FAExcelUpdateWunsh);
					sqlCommand.Parameters.AddWithValue("FAFehlrMaterial", item.FAFehlrMaterial == null ? (object)DBNull.Value : item.FAFehlrMaterial);
					sqlCommand.Parameters.AddWithValue("FaHomeAnalysis", item.FaHomeAnalysis == null ? (object)DBNull.Value : item.FaHomeAnalysis);
					sqlCommand.Parameters.AddWithValue("FaHomeUpdate", item.FaHomeUpdate == null ? (object)DBNull.Value : item.FaHomeUpdate);
					sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei", item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
					sqlCommand.Parameters.AddWithValue("FaPlanningEdit", item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
					sqlCommand.Parameters.AddWithValue("FaPlanningView", item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
					sqlCommand.Parameters.AddWithValue("FAPlannung", item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
					sqlCommand.Parameters.AddWithValue("FAPlannungTechnick", item.FAPlannungTechnick == null ? (object)DBNull.Value : item.FAPlannungTechnick);
					sqlCommand.Parameters.AddWithValue("FAPriesZeitUpdate", item.FAPriesZeitUpdate == null ? (object)DBNull.Value : item.FAPriesZeitUpdate);
					sqlCommand.Parameters.AddWithValue("FAProductionPlannung", item.FAProductionPlannung == null ? (object)DBNull.Value : item.FAProductionPlannung);
					sqlCommand.Parameters.AddWithValue("FAStappleDruck", item.FAStappleDruck == null ? (object)DBNull.Value : item.FAStappleDruck);
					sqlCommand.Parameters.AddWithValue("FAStatusAlbania", item.FAStatusAlbania == null ? (object)DBNull.Value : item.FAStatusAlbania);
					sqlCommand.Parameters.AddWithValue("FAStatusCzech", item.FAStatusCzech == null ? (object)DBNull.Value : item.FAStatusCzech);
					sqlCommand.Parameters.AddWithValue("FAStatusTunisia", item.FAStatusTunisia == null ? (object)DBNull.Value : item.FAStatusTunisia);
					sqlCommand.Parameters.AddWithValue("FAStorno", item.FAStorno == null ? (object)DBNull.Value : item.FAStorno);
					sqlCommand.Parameters.AddWithValue("FAStucklist", item.FAStucklist == null ? (object)DBNull.Value : item.FAStucklist);
					sqlCommand.Parameters.AddWithValue("FaTechnicEdit", item.FaTechnicEdit == null ? (object)DBNull.Value : item.FaTechnicEdit);
					sqlCommand.Parameters.AddWithValue("FaTechnicView", item.FaTechnicView == null ? (object)DBNull.Value : item.FaTechnicView);
					sqlCommand.Parameters.AddWithValue("FATerminWerk", item.FATerminWerk == null ? (object)DBNull.Value : item.FATerminWerk);
					sqlCommand.Parameters.AddWithValue("FAUpdateBemerkungExtern", item.FAUpdateBemerkungExtern == null ? (object)DBNull.Value : item.FAUpdateBemerkungExtern);
					sqlCommand.Parameters.AddWithValue("FAUpdateByArticle", item.FAUpdateByArticle == null ? (object)DBNull.Value : item.FAUpdateByArticle);
					sqlCommand.Parameters.AddWithValue("FAUpdateByFA", item.FAUpdateByFA == null ? (object)DBNull.Value : item.FAUpdateByFA);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1", item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2", item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3", item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
					sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin", item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
					sqlCommand.Parameters.AddWithValue("Fertigung", item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
					sqlCommand.Parameters.AddWithValue("FertigungLog", item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
					sqlCommand.Parameters.AddWithValue("ForcastCreate", item.ForcastCreate == null ? (object)DBNull.Value : item.ForcastCreate);
					sqlCommand.Parameters.AddWithValue("ForcastDelete", item.ForcastDelete == null ? (object)DBNull.Value : item.ForcastDelete);
					sqlCommand.Parameters.AddWithValue("ForcastEdit", item.ForcastEdit == null ? (object)DBNull.Value : item.ForcastEdit);
					sqlCommand.Parameters.AddWithValue("ForcastLog", item.ForcastLog == null ? (object)DBNull.Value : item.ForcastLog);
					sqlCommand.Parameters.AddWithValue("ForcastPositionEdit", item.ForcastPositionEdit == null ? (object)DBNull.Value : item.ForcastPositionEdit);
					sqlCommand.Parameters.AddWithValue("ForcastReport", item.ForcastReport == null ? (object)DBNull.Value : item.ForcastReport);
					sqlCommand.Parameters.AddWithValue("ForcastView", item.ForcastView == null ? (object)DBNull.Value : item.ForcastView);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon1", item.FRCPosHorizon1 == null ? (object)DBNull.Value : item.FRCPosHorizon1);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon2", item.FRCPosHorizon2 == null ? (object)DBNull.Value : item.FRCPosHorizon2);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon3", item.FRCPosHorizon3 == null ? (object)DBNull.Value : item.FRCPosHorizon3);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon1", item.GSPosHorizon1 == null ? (object)DBNull.Value : item.GSPosHorizon1);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon2", item.GSPosHorizon2 == null ? (object)DBNull.Value : item.GSPosHorizon2);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon3", item.GSPosHorizon3 == null ? (object)DBNull.Value : item.GSPosHorizon3);
					sqlCommand.Parameters.AddWithValue("GutschriftBookedEdit", item.GutschriftBookedEdit == null ? (object)DBNull.Value : item.GutschriftBookedEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftCreate", item.GutschriftCreate == null ? (object)DBNull.Value : item.GutschriftCreate);
					sqlCommand.Parameters.AddWithValue("GutschriftDelete", item.GutschriftDelete == null ? (object)DBNull.Value : item.GutschriftDelete);
					sqlCommand.Parameters.AddWithValue("GutschriftDoneEdit", item.GutschriftDoneEdit == null ? (object)DBNull.Value : item.GutschriftDoneEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftEdit", item.GutschriftEdit == null ? (object)DBNull.Value : item.GutschriftEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftLog", item.GutschriftLog == null ? (object)DBNull.Value : item.GutschriftLog);
					sqlCommand.Parameters.AddWithValue("GutschriftPositionEdit", item.GutschriftPositionEdit == null ? (object)DBNull.Value : item.GutschriftPositionEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftReport", item.GutschriftReport == null ? (object)DBNull.Value : item.GutschriftReport);
					sqlCommand.Parameters.AddWithValue("GutschriftView", item.GutschriftView == null ? (object)DBNull.Value : item.GutschriftView);
					sqlCommand.Parameters.AddWithValue("InsideSalesChecks", item.InsideSalesChecks == null ? (object)DBNull.Value : item.InsideSalesChecks);
					sqlCommand.Parameters.AddWithValue("InsideSalesChecksArchive", item.InsideSalesChecksArchive == null ? (object)DBNull.Value : item.InsideSalesChecksArchive);
					sqlCommand.Parameters.AddWithValue("InsideSalesCustomerSummary", item.InsideSalesCustomerSummary == null ? (object)DBNull.Value : item.InsideSalesCustomerSummary);
					sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluation", item.InsideSalesMinimumStockEvaluation == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluation);
					sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluationTable", item.InsideSalesMinimumStockEvaluationTable == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluationTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrders", item.InsideSalesOverdueOrders == null ? (object)DBNull.Value : item.InsideSalesOverdueOrders);
					sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrdersTable", item.InsideSalesOverdueOrdersTable == null ? (object)DBNull.Value : item.InsideSalesOverdueOrdersTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrders", item.InsideSalesTotalUnbookedOrders == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrders);
					sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrdersTable", item.InsideSalesTotalUnbookedOrdersTable == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrdersTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesTurnoverCurrentWeek", item.InsideSalesTurnoverCurrentWeek == null ? (object)DBNull.Value : item.InsideSalesTurnoverCurrentWeek);
					sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon1", item.LSPosHorizon1 == null ? (object)DBNull.Value : item.LSPosHorizon1);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon2", item.LSPosHorizon2 == null ? (object)DBNull.Value : item.LSPosHorizon2);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon3", item.LSPosHorizon3 == null ? (object)DBNull.Value : item.LSPosHorizon3);
					sqlCommand.Parameters.AddWithValue("mId", item.mId == null ? (object)DBNull.Value : item.mId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("OrderProcessing", item.OrderProcessing == null ? (object)DBNull.Value : item.OrderProcessing);
					sqlCommand.Parameters.AddWithValue("OrderProcessingLog", item.OrderProcessingLog == null ? (object)DBNull.Value : item.OrderProcessingLog);
					sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
					sqlCommand.Parameters.AddWithValue("RahmenAddAB", item.RahmenAddAB == null ? (object)DBNull.Value : item.RahmenAddAB);
					sqlCommand.Parameters.AddWithValue("RahmenAddPositions", item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
					sqlCommand.Parameters.AddWithValue("RahmenCancelation", item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
					sqlCommand.Parameters.AddWithValue("RahmenClosure", item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
					sqlCommand.Parameters.AddWithValue("RahmenDelete", item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
					sqlCommand.Parameters.AddWithValue("RahmenDeletePositions", item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
					sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow", item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
					sqlCommand.Parameters.AddWithValue("RahmenEditHeader", item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
					sqlCommand.Parameters.AddWithValue("RahmenEditPositions", item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
					sqlCommand.Parameters.AddWithValue("RahmenHistory", item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
					sqlCommand.Parameters.AddWithValue("RahmenValdation", item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon1", item.RAPosHorizon1 == null ? (object)DBNull.Value : item.RAPosHorizon1);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon2", item.RAPosHorizon2 == null ? (object)DBNull.Value : item.RAPosHorizon2);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon3", item.RAPosHorizon3 == null ? (object)DBNull.Value : item.RAPosHorizon3);
					sqlCommand.Parameters.AddWithValue("Rechnung", item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
					sqlCommand.Parameters.AddWithValue("RechnungAutoCreation", item.RechnungAutoCreation == null ? (object)DBNull.Value : item.RechnungAutoCreation);
					sqlCommand.Parameters.AddWithValue("RechnungBookedEdit", item.RechnungBookedEdit == null ? (object)DBNull.Value : item.RechnungBookedEdit);
					sqlCommand.Parameters.AddWithValue("RechnungConfig", item.RechnungConfig == null ? (object)DBNull.Value : item.RechnungConfig);
					sqlCommand.Parameters.AddWithValue("RechnungDelete", item.RechnungDelete == null ? (object)DBNull.Value : item.RechnungDelete);
					sqlCommand.Parameters.AddWithValue("RechnungDoneEdit", item.RechnungDoneEdit == null ? (object)DBNull.Value : item.RechnungDoneEdit);
					sqlCommand.Parameters.AddWithValue("RechnungManualCreation", item.RechnungManualCreation == null ? (object)DBNull.Value : item.RechnungManualCreation);
					sqlCommand.Parameters.AddWithValue("RechnungReport", item.RechnungReport == null ? (object)DBNull.Value : item.RechnungReport);
					sqlCommand.Parameters.AddWithValue("RechnungSend", item.RechnungSend == null ? (object)DBNull.Value : item.RechnungSend);
					sqlCommand.Parameters.AddWithValue("RechnungValidate", item.RechnungValidate == null ? (object)DBNull.Value : item.RechnungValidate);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon1", item.RGPosHorizon1 == null ? (object)DBNull.Value : item.RGPosHorizon1);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon2", item.RGPosHorizon2 == null ? (object)DBNull.Value : item.RGPosHorizon2);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon3", item.RGPosHorizon3 == null ? (object)DBNull.Value : item.RGPosHorizon3);
					sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatsBacklogFGAdmin", item.StatsBacklogFGAdmin == null ? (object)DBNull.Value : item.StatsBacklogFGAdmin);
					sqlCommand.Parameters.AddWithValue("StatsBacklogHWAdmin", item.StatsBacklogHWAdmin == null ? (object)DBNull.Value : item.StatsBacklogHWAdmin);
					sqlCommand.Parameters.AddWithValue("StatsCapaCutting", item.StatsCapaCutting == null ? (object)DBNull.Value : item.StatsCapaCutting);
					sqlCommand.Parameters.AddWithValue("StatsCapaHorizons", item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
					sqlCommand.Parameters.AddWithValue("StatsCapaLong", item.StatsCapaLong == null ? (object)DBNull.Value : item.StatsCapaLong);
					sqlCommand.Parameters.AddWithValue("StatsCapaPlanning", item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
					sqlCommand.Parameters.AddWithValue("StatsCapaShort", item.StatsCapaShort == null ? (object)DBNull.Value : item.StatsCapaShort);
					sqlCommand.Parameters.AddWithValue("StatsRechnungAL", item.StatsRechnungAL == null ? (object)DBNull.Value : item.StatsRechnungAL);
					sqlCommand.Parameters.AddWithValue("StatsRechnungBETN", item.StatsRechnungBETN == null ? (object)DBNull.Value : item.StatsRechnungBETN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungCZ", item.StatsRechnungCZ == null ? (object)DBNull.Value : item.StatsRechnungCZ);
					sqlCommand.Parameters.AddWithValue("StatsRechnungDE", item.StatsRechnungDE == null ? (object)DBNull.Value : item.StatsRechnungDE);
					sqlCommand.Parameters.AddWithValue("StatsRechnungGZTN", item.StatsRechnungGZTN == null ? (object)DBNull.Value : item.StatsRechnungGZTN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungTN", item.StatsRechnungTN == null ? (object)DBNull.Value : item.StatsRechnungTN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungWS", item.StatsRechnungWS == null ? (object)DBNull.Value : item.StatsRechnungWS);
					sqlCommand.Parameters.AddWithValue("StatsStockCS", item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
					sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse", item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
					sqlCommand.Parameters.AddWithValue("StatsStockFG", item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
					sqlCommand.Parameters.AddWithValue("UBGStatusChange", item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 208; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}

			return -1;
		}
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__CTS_AccessProfile] ([AB_LT],[AB_LT_EDI],[ABPosHorizon1],[ABPosHorizon2],[ABPosHorizon3],[AccessProfileName],[Administration],[BVBookedEdit],[BVDoneEdit],[BVFaCreate],[Configuration],[ConfigurationAppoitments],[ConfigurationChangeEmployees],[ConfigurationReplacements],[ConfigurationReporting],[ConfirmationBookedEdit],[ConfirmationCreate],[ConfirmationDelete],[ConfirmationDeliveryNote],[ConfirmationDoneEdit],[ConfirmationEdit],[ConfirmationPositionEdit],[ConfirmationPositionProduction],[ConfirmationReport],[ConfirmationValidate],[ConfirmationView],[CreationTime],[CreationUserId],[CSInfoEdit],[DelforCreate],[DelforDelete],[DelforDeletePosition],[DelforOrderConfirmation],[DelforReport],[DelforStatistics],[DelforView],[DeliveryNoteBookedEdit],[DeliveryNoteCreate],[DeliveryNoteDelete],[DeliveryNoteDoneEdit],[DeliveryNoteEdit],[DeliveryNoteLog],[DeliveryNotePositionEdit],[DeliveryNoteReport],[DeliveryNoteView],[DLFPosHorizon1],[DLFPosHorizon2],[DLFPosHorizon3],[EDI],[EDIDownloadFile],[EDIError],[EDIErrorEdit],[EDIErrorValidated],[EDILogOrderValidated],[EDIOrder],[EDIOrderEdit],[EDIOrderPositionEdit],[EDIOrderProduction],[EDIOrderProductionPosition],[EDIOrderReport],[EDIOrderValidated],[EDIOrderValidatedEdit],[FaABEdit],[FaABView],[FaActionBook],[FaActionComplete],[FaActionDelete],[FaActionPrint],[FaAdmin],[FAAKtualTerminUpdate],[FaAnalysis],[FAAuswertungEndkontrolle],[FABemerkungPlannug],[FABemerkungZuGewerk],[FABemerkungZuPrio],[FACancelHorizon1],[FACancelHorizon2],[FACancelHorizon3],[FACommissionert],[FaCreate],[FACreateHorizon1],[FACreateHorizon2],[FACreateHorizon3],[FaDatenEdit],[FaDatenView],[FaDelete],[FADrucken],[FaEdit],[FAErlidegen],[FAExcelUpdateWerk],[FAExcelUpdateWunsh],[FAFehlrMaterial],[FaHomeAnalysis],[FaHomeUpdate],[FALaufkarteSchneiderei],[FaPlanningEdit],[FaPlanningView],[FAPlannung],[FAPlannungTechnick],[FAPriesZeitUpdate],[FAProductionPlannung],[FAStappleDruck],[FAStatusAlbania],[FAStatusCzech],[FAStatusTunisia],[FAStorno],[FAStucklist],[FaTechnicEdit],[FaTechnicView],[FATerminWerk],[FAUpdateBemerkungExtern],[FAUpdateByArticle],[FAUpdateByFA],[FAUpdateTerminHorizon1],[FAUpdateTerminHorizon2],[FAUpdateTerminHorizon3],[FAWerkWunshAdmin],[Fertigung],[FertigungLog],[ForcastCreate],[ForcastDelete],[ForcastEdit],[ForcastLog],[ForcastPositionEdit],[ForcastReport],[ForcastView],[FRCPosHorizon1],[FRCPosHorizon2],[FRCPosHorizon3],[GSPosHorizon1],[GSPosHorizon2],[GSPosHorizon3],[GutschriftBookedEdit],[GutschriftCreate],[GutschriftDelete],[GutschriftDoneEdit],[GutschriftEdit],[GutschriftLog],[GutschriftPositionEdit],[GutschriftReport],[GutschriftView],[InsideSalesChecks],[InsideSalesChecksArchive],[InsideSalesCustomerSummary],[InsideSalesMinimumStockEvaluation],[InsideSalesMinimumStockEvaluationTable],[InsideSalesOverdueOrders],[InsideSalesOverdueOrdersTable],[InsideSalesTotalUnbookedOrders],[InsideSalesTotalUnbookedOrdersTable],[InsideSalesTurnoverCurrentWeek],[IsDefault],[LSPosHorizon1],[LSPosHorizon2],[LSPosHorizon3],[mId],[ModuleActivated],[OrderProcessing],[OrderProcessingLog],[Rahmen],[RahmenAdd],[RahmenAddAB],[RahmenAddPositions],[RahmenCancelation],[RahmenClosure],[RahmenDelete],[RahmenDeletePositions],[RahmenDocumentFlow],[RahmenEditHeader],[RahmenEditPositions],[RahmenHistory],[RahmenValdation],[RAPosHorizon1],[RAPosHorizon2],[RAPosHorizon3],[Rechnung],[RechnungAutoCreation],[RechnungBookedEdit],[RechnungConfig],[RechnungDelete],[RechnungDoneEdit],[RechnungManualCreation],[RechnungReport],[RechnungSend],[RechnungValidate],[RGPosHorizon1],[RGPosHorizon2],[RGPosHorizon3],[Statistics],[StatsBacklogFGAdmin],[StatsBacklogHWAdmin],[StatsCapaCutting],[StatsCapaHorizons],[StatsCapaLong],[StatsCapaPlanning],[StatsCapaShort],[StatsRechnungAL],[StatsRechnungBETN],[StatsRechnungCZ],[StatsRechnungDE],[StatsRechnungGZTN],[StatsRechnungTN],[StatsRechnungWS],[StatsStockCS],[StatsStockExternalWarehouse],[StatsStockFG],[UBGStatusChange]) VALUES ( "

							+ "@AB_LT" + i + ","
							+ "@AB_LT_EDI" + i + ","
							+ "@ABPosHorizon1" + i + ","
							+ "@ABPosHorizon2" + i + ","
							+ "@ABPosHorizon3" + i + ","
							+ "@AccessProfileName" + i + ","
							+ "@Administration" + i + ","
							+ "@BVBookedEdit" + i + ","
							+ "@BVDoneEdit" + i + ","
							+ "@BVFaCreate" + i + ","
							+ "@Configuration" + i + ","
							+ "@ConfigurationAppoitments" + i + ","
							+ "@ConfigurationChangeEmployees" + i + ","
							+ "@ConfigurationReplacements" + i + ","
							+ "@ConfigurationReporting" + i + ","
							+ "@ConfirmationBookedEdit" + i + ","
							+ "@ConfirmationCreate" + i + ","
							+ "@ConfirmationDelete" + i + ","
							+ "@ConfirmationDeliveryNote" + i + ","
							+ "@ConfirmationDoneEdit" + i + ","
							+ "@ConfirmationEdit" + i + ","
							+ "@ConfirmationPositionEdit" + i + ","
							+ "@ConfirmationPositionProduction" + i + ","
							+ "@ConfirmationReport" + i + ","
							+ "@ConfirmationValidate" + i + ","
							+ "@ConfirmationView" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CSInfoEdit" + i + ","
							+ "@DelforCreate" + i + ","
							+ "@DelforDelete" + i + ","
							+ "@DelforDeletePosition" + i + ","
							+ "@DelforOrderConfirmation" + i + ","
							+ "@DelforReport" + i + ","
							+ "@DelforStatistics" + i + ","
							+ "@DelforView" + i + ","
							+ "@DeliveryNoteBookedEdit" + i + ","
							+ "@DeliveryNoteCreate" + i + ","
							+ "@DeliveryNoteDelete" + i + ","
							+ "@DeliveryNoteDoneEdit" + i + ","
							+ "@DeliveryNoteEdit" + i + ","
							+ "@DeliveryNoteLog" + i + ","
							+ "@DeliveryNotePositionEdit" + i + ","
							+ "@DeliveryNoteReport" + i + ","
							+ "@DeliveryNoteView" + i + ","
							+ "@DLFPosHorizon1" + i + ","
							+ "@DLFPosHorizon2" + i + ","
							+ "@DLFPosHorizon3" + i + ","
							+ "@EDI" + i + ","
							+ "@EDIDownloadFile" + i + ","
							+ "@EDIError" + i + ","
							+ "@EDIErrorEdit" + i + ","
							+ "@EDIErrorValidated" + i + ","
							+ "@EDILogOrderValidated" + i + ","
							+ "@EDIOrder" + i + ","
							+ "@EDIOrderEdit" + i + ","
							+ "@EDIOrderPositionEdit" + i + ","
							+ "@EDIOrderProduction" + i + ","
							+ "@EDIOrderProductionPosition" + i + ","
							+ "@EDIOrderReport" + i + ","
							+ "@EDIOrderValidated" + i + ","
							+ "@EDIOrderValidatedEdit" + i + ","
							+ "@FaABEdit" + i + ","
							+ "@FaABView" + i + ","
							+ "@FaActionBook" + i + ","
							+ "@FaActionComplete" + i + ","
							+ "@FaActionDelete" + i + ","
							+ "@FaActionPrint" + i + ","
							+ "@FaAdmin" + i + ","
							+ "@FAAKtualTerminUpdate" + i + ","
							+ "@FaAnalysis" + i + ","
							+ "@FAAuswertungEndkontrolle" + i + ","
							+ "@FABemerkungPlannug" + i + ","
							+ "@FABemerkungZuGewerk" + i + ","
							+ "@FABemerkungZuPrio" + i + ","
							+ "@FACancelHorizon1" + i + ","
							+ "@FACancelHorizon2" + i + ","
							+ "@FACancelHorizon3" + i + ","
							+ "@FACommissionert" + i + ","
							+ "@FaCreate" + i + ","
							+ "@FACreateHorizon1" + i + ","
							+ "@FACreateHorizon2" + i + ","
							+ "@FACreateHorizon3" + i + ","
							+ "@FaDatenEdit" + i + ","
							+ "@FaDatenView" + i + ","
							+ "@FaDelete" + i + ","
							+ "@FADrucken" + i + ","
							+ "@FaEdit" + i + ","
							+ "@FAErlidegen" + i + ","
							+ "@FAExcelUpdateWerk" + i + ","
							+ "@FAExcelUpdateWunsh" + i + ","
							+ "@FAFehlrMaterial" + i + ","
							+ "@FaHomeAnalysis" + i + ","
							+ "@FaHomeUpdate" + i + ","
							+ "@FALaufkarteSchneiderei" + i + ","
							+ "@FaPlanningEdit" + i + ","
							+ "@FaPlanningView" + i + ","
							+ "@FAPlannung" + i + ","
							+ "@FAPlannungTechnick" + i + ","
							+ "@FAPriesZeitUpdate" + i + ","
							+ "@FAProductionPlannung" + i + ","
							+ "@FAStappleDruck" + i + ","
							+ "@FAStatusAlbania" + i + ","
							+ "@FAStatusCzech" + i + ","
							+ "@FAStatusTunisia" + i + ","
							+ "@FAStorno" + i + ","
							+ "@FAStucklist" + i + ","
							+ "@FaTechnicEdit" + i + ","
							+ "@FaTechnicView" + i + ","
							+ "@FATerminWerk" + i + ","
							+ "@FAUpdateBemerkungExtern" + i + ","
							+ "@FAUpdateByArticle" + i + ","
							+ "@FAUpdateByFA" + i + ","
							+ "@FAUpdateTerminHorizon1" + i + ","
							+ "@FAUpdateTerminHorizon2" + i + ","
							+ "@FAUpdateTerminHorizon3" + i + ","
							+ "@FAWerkWunshAdmin" + i + ","
							+ "@Fertigung" + i + ","
							+ "@FertigungLog" + i + ","
							+ "@ForcastCreate" + i + ","
							+ "@ForcastDelete" + i + ","
							+ "@ForcastEdit" + i + ","
							+ "@ForcastLog" + i + ","
							+ "@ForcastPositionEdit" + i + ","
							+ "@ForcastReport" + i + ","
							+ "@ForcastView" + i + ","
							+ "@FRCPosHorizon1" + i + ","
							+ "@FRCPosHorizon2" + i + ","
							+ "@FRCPosHorizon3" + i + ","
							+ "@GSPosHorizon1" + i + ","
							+ "@GSPosHorizon2" + i + ","
							+ "@GSPosHorizon3" + i + ","
							+ "@GutschriftBookedEdit" + i + ","
							+ "@GutschriftCreate" + i + ","
							+ "@GutschriftDelete" + i + ","
							+ "@GutschriftDoneEdit" + i + ","
							+ "@GutschriftEdit" + i + ","
							+ "@GutschriftLog" + i + ","
							+ "@GutschriftPositionEdit" + i + ","
							+ "@GutschriftReport" + i + ","
							+ "@GutschriftView" + i + ","
							+ "@InsideSalesChecks" + i + ","
							+ "@InsideSalesChecksArchive" + i + ","
							+ "@InsideSalesCustomerSummary" + i + ","
							+ "@InsideSalesMinimumStockEvaluation" + i + ","
							+ "@InsideSalesMinimumStockEvaluationTable" + i + ","
							+ "@InsideSalesOverdueOrders" + i + ","
							+ "@InsideSalesOverdueOrdersTable" + i + ","
							+ "@InsideSalesTotalUnbookedOrders" + i + ","
							+ "@InsideSalesTotalUnbookedOrdersTable" + i + ","
							+ "@InsideSalesTurnoverCurrentWeek" + i + ","
							+ "@IsDefault" + i + ","
							+ "@LSPosHorizon1" + i + ","
							+ "@LSPosHorizon2" + i + ","
							+ "@LSPosHorizon3" + i + ","
							+ "@mId" + i + ","
							+ "@ModuleActivated" + i + ","
							+ "@OrderProcessing" + i + ","
							+ "@OrderProcessingLog" + i + ","
							+ "@Rahmen" + i + ","
							+ "@RahmenAdd" + i + ","
							+ "@RahmenAddAB" + i + ","
							+ "@RahmenAddPositions" + i + ","
							+ "@RahmenCancelation" + i + ","
							+ "@RahmenClosure" + i + ","
							+ "@RahmenDelete" + i + ","
							+ "@RahmenDeletePositions" + i + ","
							+ "@RahmenDocumentFlow" + i + ","
							+ "@RahmenEditHeader" + i + ","
							+ "@RahmenEditPositions" + i + ","
							+ "@RahmenHistory" + i + ","
							+ "@RahmenValdation" + i + ","
							+ "@RAPosHorizon1" + i + ","
							+ "@RAPosHorizon2" + i + ","
							+ "@RAPosHorizon3" + i + ","
							+ "@Rechnung" + i + ","
							+ "@RechnungAutoCreation" + i + ","
							+ "@RechnungBookedEdit" + i + ","
							+ "@RechnungConfig" + i + ","
							+ "@RechnungDelete" + i + ","
							+ "@RechnungDoneEdit" + i + ","
							+ "@RechnungManualCreation" + i + ","
							+ "@RechnungReport" + i + ","
							+ "@RechnungSend" + i + ","
							+ "@RechnungValidate" + i + ","
							+ "@RGPosHorizon1" + i + ","
							+ "@RGPosHorizon2" + i + ","
							+ "@RGPosHorizon3" + i + ","
							+ "@Statistics" + i + ","
							+ "@StatsBacklogFGAdmin" + i + ","
							+ "@StatsBacklogHWAdmin" + i + ","
							+ "@StatsCapaCutting" + i + ","
							+ "@StatsCapaHorizons" + i + ","
							+ "@StatsCapaLong" + i + ","
							+ "@StatsCapaPlanning" + i + ","
							+ "@StatsCapaShort" + i + ","
							+ "@StatsRechnungAL" + i + ","
							+ "@StatsRechnungBETN" + i + ","
							+ "@StatsRechnungCZ" + i + ","
							+ "@StatsRechnungDE" + i + ","
							+ "@StatsRechnungGZTN" + i + ","
							+ "@StatsRechnungTN" + i + ","
							+ "@StatsRechnungWS" + i + ","
							+ "@StatsStockCS" + i + ","
							+ "@StatsStockExternalWarehouse" + i + ","
							+ "@StatsStockFG" + i + ","
							+ "@UBGStatusChange" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB_LT" + i, item.AB_LT == null ? (object)DBNull.Value : item.AB_LT);
						sqlCommand.Parameters.AddWithValue("AB_LT_EDI" + i, item.AB_LT_EDI == null ? (object)DBNull.Value : item.AB_LT_EDI);
						sqlCommand.Parameters.AddWithValue("ABPosHorizon1" + i, item.ABPosHorizon1 == null ? (object)DBNull.Value : item.ABPosHorizon1);
						sqlCommand.Parameters.AddWithValue("ABPosHorizon2" + i, item.ABPosHorizon2 == null ? (object)DBNull.Value : item.ABPosHorizon2);
						sqlCommand.Parameters.AddWithValue("ABPosHorizon3" + i, item.ABPosHorizon3 == null ? (object)DBNull.Value : item.ABPosHorizon3);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
						sqlCommand.Parameters.AddWithValue("BVBookedEdit" + i, item.BVBookedEdit == null ? (object)DBNull.Value : item.BVBookedEdit);
						sqlCommand.Parameters.AddWithValue("BVDoneEdit" + i, item.BVDoneEdit == null ? (object)DBNull.Value : item.BVDoneEdit);
						sqlCommand.Parameters.AddWithValue("BVFaCreate" + i, item.BVFaCreate == null ? (object)DBNull.Value : item.BVFaCreate);
						sqlCommand.Parameters.AddWithValue("Configuration" + i, item.Configuration == null ? (object)DBNull.Value : item.Configuration);
						sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments" + i, item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
						sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees" + i, item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
						sqlCommand.Parameters.AddWithValue("ConfigurationReplacements" + i, item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
						sqlCommand.Parameters.AddWithValue("ConfigurationReporting" + i, item.ConfigurationReporting == null ? (object)DBNull.Value : item.ConfigurationReporting);
						sqlCommand.Parameters.AddWithValue("ConfirmationBookedEdit" + i, item.ConfirmationBookedEdit == null ? (object)DBNull.Value : item.ConfirmationBookedEdit);
						sqlCommand.Parameters.AddWithValue("ConfirmationCreate" + i, item.ConfirmationCreate == null ? (object)DBNull.Value : item.ConfirmationCreate);
						sqlCommand.Parameters.AddWithValue("ConfirmationDelete" + i, item.ConfirmationDelete == null ? (object)DBNull.Value : item.ConfirmationDelete);
						sqlCommand.Parameters.AddWithValue("ConfirmationDeliveryNote" + i, item.ConfirmationDeliveryNote == null ? (object)DBNull.Value : item.ConfirmationDeliveryNote);
						sqlCommand.Parameters.AddWithValue("ConfirmationDoneEdit" + i, item.ConfirmationDoneEdit == null ? (object)DBNull.Value : item.ConfirmationDoneEdit);
						sqlCommand.Parameters.AddWithValue("ConfirmationEdit" + i, item.ConfirmationEdit == null ? (object)DBNull.Value : item.ConfirmationEdit);
						sqlCommand.Parameters.AddWithValue("ConfirmationPositionEdit" + i, item.ConfirmationPositionEdit == null ? (object)DBNull.Value : item.ConfirmationPositionEdit);
						sqlCommand.Parameters.AddWithValue("ConfirmationPositionProduction" + i, item.ConfirmationPositionProduction == null ? (object)DBNull.Value : item.ConfirmationPositionProduction);
						sqlCommand.Parameters.AddWithValue("ConfirmationReport" + i, item.ConfirmationReport == null ? (object)DBNull.Value : item.ConfirmationReport);
						sqlCommand.Parameters.AddWithValue("ConfirmationValidate" + i, item.ConfirmationValidate == null ? (object)DBNull.Value : item.ConfirmationValidate);
						sqlCommand.Parameters.AddWithValue("ConfirmationView" + i, item.ConfirmationView == null ? (object)DBNull.Value : item.ConfirmationView);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CSInfoEdit" + i, item.CSInfoEdit == null ? (object)DBNull.Value : item.CSInfoEdit);
						sqlCommand.Parameters.AddWithValue("DelforCreate" + i, item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
						sqlCommand.Parameters.AddWithValue("DelforDelete" + i, item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
						sqlCommand.Parameters.AddWithValue("DelforDeletePosition" + i, item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
						sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation" + i, item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
						sqlCommand.Parameters.AddWithValue("DelforReport" + i, item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
						sqlCommand.Parameters.AddWithValue("DelforStatistics" + i, item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
						sqlCommand.Parameters.AddWithValue("DelforView" + i, item.DelforView == null ? (object)DBNull.Value : item.DelforView);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteBookedEdit" + i, item.DeliveryNoteBookedEdit == null ? (object)DBNull.Value : item.DeliveryNoteBookedEdit);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteCreate" + i, item.DeliveryNoteCreate == null ? (object)DBNull.Value : item.DeliveryNoteCreate);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteDelete" + i, item.DeliveryNoteDelete == null ? (object)DBNull.Value : item.DeliveryNoteDelete);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteDoneEdit" + i, item.DeliveryNoteDoneEdit == null ? (object)DBNull.Value : item.DeliveryNoteDoneEdit);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteEdit" + i, item.DeliveryNoteEdit == null ? (object)DBNull.Value : item.DeliveryNoteEdit);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteLog" + i, item.DeliveryNoteLog == null ? (object)DBNull.Value : item.DeliveryNoteLog);
						sqlCommand.Parameters.AddWithValue("DeliveryNotePositionEdit" + i, item.DeliveryNotePositionEdit == null ? (object)DBNull.Value : item.DeliveryNotePositionEdit);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteReport" + i, item.DeliveryNoteReport == null ? (object)DBNull.Value : item.DeliveryNoteReport);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteView" + i, item.DeliveryNoteView == null ? (object)DBNull.Value : item.DeliveryNoteView);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon1" + i, item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon2" + i, item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon3" + i, item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
						sqlCommand.Parameters.AddWithValue("EDI" + i, item.EDI == null ? (object)DBNull.Value : item.EDI);
						sqlCommand.Parameters.AddWithValue("EDIDownloadFile" + i, item.EDIDownloadFile == null ? (object)DBNull.Value : item.EDIDownloadFile);
						sqlCommand.Parameters.AddWithValue("EDIError" + i, item.EDIError == null ? (object)DBNull.Value : item.EDIError);
						sqlCommand.Parameters.AddWithValue("EDIErrorEdit" + i, item.EDIErrorEdit == null ? (object)DBNull.Value : item.EDIErrorEdit);
						sqlCommand.Parameters.AddWithValue("EDIErrorValidated" + i, item.EDIErrorValidated == null ? (object)DBNull.Value : item.EDIErrorValidated);
						sqlCommand.Parameters.AddWithValue("EDILogOrderValidated" + i, item.EDILogOrderValidated == null ? (object)DBNull.Value : item.EDILogOrderValidated);
						sqlCommand.Parameters.AddWithValue("EDIOrder" + i, item.EDIOrder == null ? (object)DBNull.Value : item.EDIOrder);
						sqlCommand.Parameters.AddWithValue("EDIOrderEdit" + i, item.EDIOrderEdit == null ? (object)DBNull.Value : item.EDIOrderEdit);
						sqlCommand.Parameters.AddWithValue("EDIOrderPositionEdit" + i, item.EDIOrderPositionEdit == null ? (object)DBNull.Value : item.EDIOrderPositionEdit);
						sqlCommand.Parameters.AddWithValue("EDIOrderProduction" + i, item.EDIOrderProduction == null ? (object)DBNull.Value : item.EDIOrderProduction);
						sqlCommand.Parameters.AddWithValue("EDIOrderProductionPosition" + i, item.EDIOrderProductionPosition == null ? (object)DBNull.Value : item.EDIOrderProductionPosition);
						sqlCommand.Parameters.AddWithValue("EDIOrderReport" + i, item.EDIOrderReport == null ? (object)DBNull.Value : item.EDIOrderReport);
						sqlCommand.Parameters.AddWithValue("EDIOrderValidated" + i, item.EDIOrderValidated == null ? (object)DBNull.Value : item.EDIOrderValidated);
						sqlCommand.Parameters.AddWithValue("EDIOrderValidatedEdit" + i, item.EDIOrderValidatedEdit == null ? (object)DBNull.Value : item.EDIOrderValidatedEdit);
						sqlCommand.Parameters.AddWithValue("FaABEdit" + i, item.FaABEdit == null ? (object)DBNull.Value : item.FaABEdit);
						sqlCommand.Parameters.AddWithValue("FaABView" + i, item.FaABView == null ? (object)DBNull.Value : item.FaABView);
						sqlCommand.Parameters.AddWithValue("FaActionBook" + i, item.FaActionBook == null ? (object)DBNull.Value : item.FaActionBook);
						sqlCommand.Parameters.AddWithValue("FaActionComplete" + i, item.FaActionComplete == null ? (object)DBNull.Value : item.FaActionComplete);
						sqlCommand.Parameters.AddWithValue("FaActionDelete" + i, item.FaActionDelete == null ? (object)DBNull.Value : item.FaActionDelete);
						sqlCommand.Parameters.AddWithValue("FaActionPrint" + i, item.FaActionPrint == null ? (object)DBNull.Value : item.FaActionPrint);
						sqlCommand.Parameters.AddWithValue("FaAdmin" + i, item.FaAdmin == null ? (object)DBNull.Value : item.FaAdmin);
						sqlCommand.Parameters.AddWithValue("FAAKtualTerminUpdate" + i, item.FAAKtualTerminUpdate == null ? (object)DBNull.Value : item.FAAKtualTerminUpdate);
						sqlCommand.Parameters.AddWithValue("FaAnalysis" + i, item.FaAnalysis == null ? (object)DBNull.Value : item.FaAnalysis);
						sqlCommand.Parameters.AddWithValue("FAAuswertungEndkontrolle" + i, item.FAAuswertungEndkontrolle == null ? (object)DBNull.Value : item.FAAuswertungEndkontrolle);
						sqlCommand.Parameters.AddWithValue("FABemerkungPlannug" + i, item.FABemerkungPlannug == null ? (object)DBNull.Value : item.FABemerkungPlannug);
						sqlCommand.Parameters.AddWithValue("FABemerkungZuGewerk" + i, item.FABemerkungZuGewerk == null ? (object)DBNull.Value : item.FABemerkungZuGewerk);
						sqlCommand.Parameters.AddWithValue("FABemerkungZuPrio" + i, item.FABemerkungZuPrio == null ? (object)DBNull.Value : item.FABemerkungZuPrio);
						sqlCommand.Parameters.AddWithValue("FACancelHorizon1" + i, item.FACancelHorizon1 == null ? (object)DBNull.Value : item.FACancelHorizon1);
						sqlCommand.Parameters.AddWithValue("FACancelHorizon2" + i, item.FACancelHorizon2 == null ? (object)DBNull.Value : item.FACancelHorizon2);
						sqlCommand.Parameters.AddWithValue("FACancelHorizon3" + i, item.FACancelHorizon3 == null ? (object)DBNull.Value : item.FACancelHorizon3);
						sqlCommand.Parameters.AddWithValue("FACommissionert" + i, item.FACommissionert == null ? (object)DBNull.Value : item.FACommissionert);
						sqlCommand.Parameters.AddWithValue("FaCreate" + i, item.FaCreate == null ? (object)DBNull.Value : item.FaCreate);
						sqlCommand.Parameters.AddWithValue("FACreateHorizon1" + i, item.FACreateHorizon1 == null ? (object)DBNull.Value : item.FACreateHorizon1);
						sqlCommand.Parameters.AddWithValue("FACreateHorizon2" + i, item.FACreateHorizon2 == null ? (object)DBNull.Value : item.FACreateHorizon2);
						sqlCommand.Parameters.AddWithValue("FACreateHorizon3" + i, item.FACreateHorizon3 == null ? (object)DBNull.Value : item.FACreateHorizon3);
						sqlCommand.Parameters.AddWithValue("FaDatenEdit" + i, item.FaDatenEdit == null ? (object)DBNull.Value : item.FaDatenEdit);
						sqlCommand.Parameters.AddWithValue("FaDatenView" + i, item.FaDatenView == null ? (object)DBNull.Value : item.FaDatenView);
						sqlCommand.Parameters.AddWithValue("FaDelete" + i, item.FaDelete == null ? (object)DBNull.Value : item.FaDelete);
						sqlCommand.Parameters.AddWithValue("FADrucken" + i, item.FADrucken == null ? (object)DBNull.Value : item.FADrucken);
						sqlCommand.Parameters.AddWithValue("FaEdit" + i, item.FaEdit == null ? (object)DBNull.Value : item.FaEdit);
						sqlCommand.Parameters.AddWithValue("FAErlidegen" + i, item.FAErlidegen == null ? (object)DBNull.Value : item.FAErlidegen);
						sqlCommand.Parameters.AddWithValue("FAExcelUpdateWerk" + i, item.FAExcelUpdateWerk == null ? (object)DBNull.Value : item.FAExcelUpdateWerk);
						sqlCommand.Parameters.AddWithValue("FAExcelUpdateWunsh" + i, item.FAExcelUpdateWunsh == null ? (object)DBNull.Value : item.FAExcelUpdateWunsh);
						sqlCommand.Parameters.AddWithValue("FAFehlrMaterial" + i, item.FAFehlrMaterial == null ? (object)DBNull.Value : item.FAFehlrMaterial);
						sqlCommand.Parameters.AddWithValue("FaHomeAnalysis" + i, item.FaHomeAnalysis == null ? (object)DBNull.Value : item.FaHomeAnalysis);
						sqlCommand.Parameters.AddWithValue("FaHomeUpdate" + i, item.FaHomeUpdate == null ? (object)DBNull.Value : item.FaHomeUpdate);
						sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei" + i, item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
						sqlCommand.Parameters.AddWithValue("FaPlanningEdit" + i, item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
						sqlCommand.Parameters.AddWithValue("FaPlanningView" + i, item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
						sqlCommand.Parameters.AddWithValue("FAPlannung" + i, item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
						sqlCommand.Parameters.AddWithValue("FAPlannungTechnick" + i, item.FAPlannungTechnick == null ? (object)DBNull.Value : item.FAPlannungTechnick);
						sqlCommand.Parameters.AddWithValue("FAPriesZeitUpdate" + i, item.FAPriesZeitUpdate == null ? (object)DBNull.Value : item.FAPriesZeitUpdate);
						sqlCommand.Parameters.AddWithValue("FAProductionPlannung" + i, item.FAProductionPlannung == null ? (object)DBNull.Value : item.FAProductionPlannung);
						sqlCommand.Parameters.AddWithValue("FAStappleDruck" + i, item.FAStappleDruck == null ? (object)DBNull.Value : item.FAStappleDruck);
						sqlCommand.Parameters.AddWithValue("FAStatusAlbania" + i, item.FAStatusAlbania == null ? (object)DBNull.Value : item.FAStatusAlbania);
						sqlCommand.Parameters.AddWithValue("FAStatusCzech" + i, item.FAStatusCzech == null ? (object)DBNull.Value : item.FAStatusCzech);
						sqlCommand.Parameters.AddWithValue("FAStatusTunisia" + i, item.FAStatusTunisia == null ? (object)DBNull.Value : item.FAStatusTunisia);
						sqlCommand.Parameters.AddWithValue("FAStorno" + i, item.FAStorno == null ? (object)DBNull.Value : item.FAStorno);
						sqlCommand.Parameters.AddWithValue("FAStucklist" + i, item.FAStucklist == null ? (object)DBNull.Value : item.FAStucklist);
						sqlCommand.Parameters.AddWithValue("FaTechnicEdit" + i, item.FaTechnicEdit == null ? (object)DBNull.Value : item.FaTechnicEdit);
						sqlCommand.Parameters.AddWithValue("FaTechnicView" + i, item.FaTechnicView == null ? (object)DBNull.Value : item.FaTechnicView);
						sqlCommand.Parameters.AddWithValue("FATerminWerk" + i, item.FATerminWerk == null ? (object)DBNull.Value : item.FATerminWerk);
						sqlCommand.Parameters.AddWithValue("FAUpdateBemerkungExtern" + i, item.FAUpdateBemerkungExtern == null ? (object)DBNull.Value : item.FAUpdateBemerkungExtern);
						sqlCommand.Parameters.AddWithValue("FAUpdateByArticle" + i, item.FAUpdateByArticle == null ? (object)DBNull.Value : item.FAUpdateByArticle);
						sqlCommand.Parameters.AddWithValue("FAUpdateByFA" + i, item.FAUpdateByFA == null ? (object)DBNull.Value : item.FAUpdateByFA);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1" + i, item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2" + i, item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3" + i, item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
						sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin" + i, item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
						sqlCommand.Parameters.AddWithValue("Fertigung" + i, item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
						sqlCommand.Parameters.AddWithValue("FertigungLog" + i, item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
						sqlCommand.Parameters.AddWithValue("ForcastCreate" + i, item.ForcastCreate == null ? (object)DBNull.Value : item.ForcastCreate);
						sqlCommand.Parameters.AddWithValue("ForcastDelete" + i, item.ForcastDelete == null ? (object)DBNull.Value : item.ForcastDelete);
						sqlCommand.Parameters.AddWithValue("ForcastEdit" + i, item.ForcastEdit == null ? (object)DBNull.Value : item.ForcastEdit);
						sqlCommand.Parameters.AddWithValue("ForcastLog" + i, item.ForcastLog == null ? (object)DBNull.Value : item.ForcastLog);
						sqlCommand.Parameters.AddWithValue("ForcastPositionEdit" + i, item.ForcastPositionEdit == null ? (object)DBNull.Value : item.ForcastPositionEdit);
						sqlCommand.Parameters.AddWithValue("ForcastReport" + i, item.ForcastReport == null ? (object)DBNull.Value : item.ForcastReport);
						sqlCommand.Parameters.AddWithValue("ForcastView" + i, item.ForcastView == null ? (object)DBNull.Value : item.ForcastView);
						sqlCommand.Parameters.AddWithValue("FRCPosHorizon1" + i, item.FRCPosHorizon1 == null ? (object)DBNull.Value : item.FRCPosHorizon1);
						sqlCommand.Parameters.AddWithValue("FRCPosHorizon2" + i, item.FRCPosHorizon2 == null ? (object)DBNull.Value : item.FRCPosHorizon2);
						sqlCommand.Parameters.AddWithValue("FRCPosHorizon3" + i, item.FRCPosHorizon3 == null ? (object)DBNull.Value : item.FRCPosHorizon3);
						sqlCommand.Parameters.AddWithValue("GSPosHorizon1" + i, item.GSPosHorizon1 == null ? (object)DBNull.Value : item.GSPosHorizon1);
						sqlCommand.Parameters.AddWithValue("GSPosHorizon2" + i, item.GSPosHorizon2 == null ? (object)DBNull.Value : item.GSPosHorizon2);
						sqlCommand.Parameters.AddWithValue("GSPosHorizon3" + i, item.GSPosHorizon3 == null ? (object)DBNull.Value : item.GSPosHorizon3);
						sqlCommand.Parameters.AddWithValue("GutschriftBookedEdit" + i, item.GutschriftBookedEdit == null ? (object)DBNull.Value : item.GutschriftBookedEdit);
						sqlCommand.Parameters.AddWithValue("GutschriftCreate" + i, item.GutschriftCreate == null ? (object)DBNull.Value : item.GutschriftCreate);
						sqlCommand.Parameters.AddWithValue("GutschriftDelete" + i, item.GutschriftDelete == null ? (object)DBNull.Value : item.GutschriftDelete);
						sqlCommand.Parameters.AddWithValue("GutschriftDoneEdit" + i, item.GutschriftDoneEdit == null ? (object)DBNull.Value : item.GutschriftDoneEdit);
						sqlCommand.Parameters.AddWithValue("GutschriftEdit" + i, item.GutschriftEdit == null ? (object)DBNull.Value : item.GutschriftEdit);
						sqlCommand.Parameters.AddWithValue("GutschriftLog" + i, item.GutschriftLog == null ? (object)DBNull.Value : item.GutschriftLog);
						sqlCommand.Parameters.AddWithValue("GutschriftPositionEdit" + i, item.GutschriftPositionEdit == null ? (object)DBNull.Value : item.GutschriftPositionEdit);
						sqlCommand.Parameters.AddWithValue("GutschriftReport" + i, item.GutschriftReport == null ? (object)DBNull.Value : item.GutschriftReport);
						sqlCommand.Parameters.AddWithValue("GutschriftView" + i, item.GutschriftView == null ? (object)DBNull.Value : item.GutschriftView);
						sqlCommand.Parameters.AddWithValue("InsideSalesChecks" + i, item.InsideSalesChecks == null ? (object)DBNull.Value : item.InsideSalesChecks);
						sqlCommand.Parameters.AddWithValue("InsideSalesChecksArchive" + i, item.InsideSalesChecksArchive == null ? (object)DBNull.Value : item.InsideSalesChecksArchive);
						sqlCommand.Parameters.AddWithValue("InsideSalesCustomerSummary" + i, item.InsideSalesCustomerSummary == null ? (object)DBNull.Value : item.InsideSalesCustomerSummary);
						sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluation" + i, item.InsideSalesMinimumStockEvaluation == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluation);
						sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluationTable" + i, item.InsideSalesMinimumStockEvaluationTable == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluationTable);
						sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrders" + i, item.InsideSalesOverdueOrders == null ? (object)DBNull.Value : item.InsideSalesOverdueOrders);
						sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrdersTable" + i, item.InsideSalesOverdueOrdersTable == null ? (object)DBNull.Value : item.InsideSalesOverdueOrdersTable);
						sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrders" + i, item.InsideSalesTotalUnbookedOrders == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrders);
						sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrdersTable" + i, item.InsideSalesTotalUnbookedOrdersTable == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrdersTable);
						sqlCommand.Parameters.AddWithValue("InsideSalesTurnoverCurrentWeek" + i, item.InsideSalesTurnoverCurrentWeek == null ? (object)DBNull.Value : item.InsideSalesTurnoverCurrentWeek);
						sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
						sqlCommand.Parameters.AddWithValue("LSPosHorizon1" + i, item.LSPosHorizon1 == null ? (object)DBNull.Value : item.LSPosHorizon1);
						sqlCommand.Parameters.AddWithValue("LSPosHorizon2" + i, item.LSPosHorizon2 == null ? (object)DBNull.Value : item.LSPosHorizon2);
						sqlCommand.Parameters.AddWithValue("LSPosHorizon3" + i, item.LSPosHorizon3 == null ? (object)DBNull.Value : item.LSPosHorizon3);
						sqlCommand.Parameters.AddWithValue("mId" + i, item.mId == null ? (object)DBNull.Value : item.mId);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("OrderProcessing" + i, item.OrderProcessing == null ? (object)DBNull.Value : item.OrderProcessing);
						sqlCommand.Parameters.AddWithValue("OrderProcessingLog" + i, item.OrderProcessingLog == null ? (object)DBNull.Value : item.OrderProcessingLog);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
						sqlCommand.Parameters.AddWithValue("RahmenAdd" + i, item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
						sqlCommand.Parameters.AddWithValue("RahmenAddAB" + i, item.RahmenAddAB == null ? (object)DBNull.Value : item.RahmenAddAB);
						sqlCommand.Parameters.AddWithValue("RahmenAddPositions" + i, item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
						sqlCommand.Parameters.AddWithValue("RahmenCancelation" + i, item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
						sqlCommand.Parameters.AddWithValue("RahmenClosure" + i, item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
						sqlCommand.Parameters.AddWithValue("RahmenDelete" + i, item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
						sqlCommand.Parameters.AddWithValue("RahmenDeletePositions" + i, item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
						sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow" + i, item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
						sqlCommand.Parameters.AddWithValue("RahmenEditHeader" + i, item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
						sqlCommand.Parameters.AddWithValue("RahmenEditPositions" + i, item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
						sqlCommand.Parameters.AddWithValue("RahmenHistory" + i, item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
						sqlCommand.Parameters.AddWithValue("RahmenValdation" + i, item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
						sqlCommand.Parameters.AddWithValue("RAPosHorizon1" + i, item.RAPosHorizon1 == null ? (object)DBNull.Value : item.RAPosHorizon1);
						sqlCommand.Parameters.AddWithValue("RAPosHorizon2" + i, item.RAPosHorizon2 == null ? (object)DBNull.Value : item.RAPosHorizon2);
						sqlCommand.Parameters.AddWithValue("RAPosHorizon3" + i, item.RAPosHorizon3 == null ? (object)DBNull.Value : item.RAPosHorizon3);
						sqlCommand.Parameters.AddWithValue("Rechnung" + i, item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
						sqlCommand.Parameters.AddWithValue("RechnungAutoCreation" + i, item.RechnungAutoCreation == null ? (object)DBNull.Value : item.RechnungAutoCreation);
						sqlCommand.Parameters.AddWithValue("RechnungBookedEdit" + i, item.RechnungBookedEdit == null ? (object)DBNull.Value : item.RechnungBookedEdit);
						sqlCommand.Parameters.AddWithValue("RechnungConfig" + i, item.RechnungConfig == null ? (object)DBNull.Value : item.RechnungConfig);
						sqlCommand.Parameters.AddWithValue("RechnungDelete" + i, item.RechnungDelete == null ? (object)DBNull.Value : item.RechnungDelete);
						sqlCommand.Parameters.AddWithValue("RechnungDoneEdit" + i, item.RechnungDoneEdit == null ? (object)DBNull.Value : item.RechnungDoneEdit);
						sqlCommand.Parameters.AddWithValue("RechnungManualCreation" + i, item.RechnungManualCreation == null ? (object)DBNull.Value : item.RechnungManualCreation);
						sqlCommand.Parameters.AddWithValue("RechnungReport" + i, item.RechnungReport == null ? (object)DBNull.Value : item.RechnungReport);
						sqlCommand.Parameters.AddWithValue("RechnungSend" + i, item.RechnungSend == null ? (object)DBNull.Value : item.RechnungSend);
						sqlCommand.Parameters.AddWithValue("RechnungValidate" + i, item.RechnungValidate == null ? (object)DBNull.Value : item.RechnungValidate);
						sqlCommand.Parameters.AddWithValue("RGPosHorizon1" + i, item.RGPosHorizon1 == null ? (object)DBNull.Value : item.RGPosHorizon1);
						sqlCommand.Parameters.AddWithValue("RGPosHorizon2" + i, item.RGPosHorizon2 == null ? (object)DBNull.Value : item.RGPosHorizon2);
						sqlCommand.Parameters.AddWithValue("RGPosHorizon3" + i, item.RGPosHorizon3 == null ? (object)DBNull.Value : item.RGPosHorizon3);
						sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
						sqlCommand.Parameters.AddWithValue("StatsBacklogFGAdmin" + i, item.StatsBacklogFGAdmin == null ? (object)DBNull.Value : item.StatsBacklogFGAdmin);
						sqlCommand.Parameters.AddWithValue("StatsBacklogHWAdmin" + i, item.StatsBacklogHWAdmin == null ? (object)DBNull.Value : item.StatsBacklogHWAdmin);
						sqlCommand.Parameters.AddWithValue("StatsCapaCutting" + i, item.StatsCapaCutting == null ? (object)DBNull.Value : item.StatsCapaCutting);
						sqlCommand.Parameters.AddWithValue("StatsCapaHorizons" + i, item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
						sqlCommand.Parameters.AddWithValue("StatsCapaLong" + i, item.StatsCapaLong == null ? (object)DBNull.Value : item.StatsCapaLong);
						sqlCommand.Parameters.AddWithValue("StatsCapaPlanning" + i, item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
						sqlCommand.Parameters.AddWithValue("StatsCapaShort" + i, item.StatsCapaShort == null ? (object)DBNull.Value : item.StatsCapaShort);
						sqlCommand.Parameters.AddWithValue("StatsRechnungAL" + i, item.StatsRechnungAL == null ? (object)DBNull.Value : item.StatsRechnungAL);
						sqlCommand.Parameters.AddWithValue("StatsRechnungBETN" + i, item.StatsRechnungBETN == null ? (object)DBNull.Value : item.StatsRechnungBETN);
						sqlCommand.Parameters.AddWithValue("StatsRechnungCZ" + i, item.StatsRechnungCZ == null ? (object)DBNull.Value : item.StatsRechnungCZ);
						sqlCommand.Parameters.AddWithValue("StatsRechnungDE" + i, item.StatsRechnungDE == null ? (object)DBNull.Value : item.StatsRechnungDE);
						sqlCommand.Parameters.AddWithValue("StatsRechnungGZTN" + i, item.StatsRechnungGZTN == null ? (object)DBNull.Value : item.StatsRechnungGZTN);
						sqlCommand.Parameters.AddWithValue("StatsRechnungTN" + i, item.StatsRechnungTN == null ? (object)DBNull.Value : item.StatsRechnungTN);
						sqlCommand.Parameters.AddWithValue("StatsRechnungWS" + i, item.StatsRechnungWS == null ? (object)DBNull.Value : item.StatsRechnungWS);
						sqlCommand.Parameters.AddWithValue("StatsStockCS" + i, item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
						sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse" + i, item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
						sqlCommand.Parameters.AddWithValue("StatsStockFG" + i, item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
						sqlCommand.Parameters.AddWithValue("UBGStatusChange" + i, item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_AccessProfile] SET [AB_LT]=@AB_LT, [AB_LT_EDI]=@AB_LT_EDI, [ABPosHorizon1]=@ABPosHorizon1, [ABPosHorizon2]=@ABPosHorizon2, [ABPosHorizon3]=@ABPosHorizon3, [AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [BVBookedEdit]=@BVBookedEdit, [BVDoneEdit]=@BVDoneEdit, [BVFaCreate]=@BVFaCreate, [Configuration]=@Configuration, [ConfigurationAppoitments]=@ConfigurationAppoitments, [ConfigurationChangeEmployees]=@ConfigurationChangeEmployees, [ConfigurationReplacements]=@ConfigurationReplacements, [ConfigurationReporting]=@ConfigurationReporting, [ConfirmationBookedEdit]=@ConfirmationBookedEdit, [ConfirmationCreate]=@ConfirmationCreate, [ConfirmationDelete]=@ConfirmationDelete, [ConfirmationDeliveryNote]=@ConfirmationDeliveryNote, [ConfirmationDoneEdit]=@ConfirmationDoneEdit, [ConfirmationEdit]=@ConfirmationEdit, [ConfirmationPositionEdit]=@ConfirmationPositionEdit, [ConfirmationPositionProduction]=@ConfirmationPositionProduction, [ConfirmationReport]=@ConfirmationReport, [ConfirmationValidate]=@ConfirmationValidate, [ConfirmationView]=@ConfirmationView, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CSInfoEdit]=@CSInfoEdit, [DelforCreate]=@DelforCreate, [DelforDelete]=@DelforDelete, [DelforDeletePosition]=@DelforDeletePosition, [DelforOrderConfirmation]=@DelforOrderConfirmation, [DelforReport]=@DelforReport, [DelforStatistics]=@DelforStatistics, [DelforView]=@DelforView, [DeliveryNoteBookedEdit]=@DeliveryNoteBookedEdit, [DeliveryNoteCreate]=@DeliveryNoteCreate, [DeliveryNoteDelete]=@DeliveryNoteDelete, [DeliveryNoteDoneEdit]=@DeliveryNoteDoneEdit, [DeliveryNoteEdit]=@DeliveryNoteEdit, [DeliveryNoteLog]=@DeliveryNoteLog, [DeliveryNotePositionEdit]=@DeliveryNotePositionEdit, [DeliveryNoteReport]=@DeliveryNoteReport, [DeliveryNoteView]=@DeliveryNoteView, [DLFPosHorizon1]=@DLFPosHorizon1, [DLFPosHorizon2]=@DLFPosHorizon2, [DLFPosHorizon3]=@DLFPosHorizon3, [EDI]=@EDI, [EDIDownloadFile]=@EDIDownloadFile, [EDIError]=@EDIError, [EDIErrorEdit]=@EDIErrorEdit, [EDIErrorValidated]=@EDIErrorValidated, [EDILogOrderValidated]=@EDILogOrderValidated, [EDIOrder]=@EDIOrder, [EDIOrderEdit]=@EDIOrderEdit, [EDIOrderPositionEdit]=@EDIOrderPositionEdit, [EDIOrderProduction]=@EDIOrderProduction, [EDIOrderProductionPosition]=@EDIOrderProductionPosition, [EDIOrderReport]=@EDIOrderReport, [EDIOrderValidated]=@EDIOrderValidated, [EDIOrderValidatedEdit]=@EDIOrderValidatedEdit, [FaABEdit]=@FaABEdit, [FaABView]=@FaABView, [FaActionBook]=@FaActionBook, [FaActionComplete]=@FaActionComplete, [FaActionDelete]=@FaActionDelete, [FaActionPrint]=@FaActionPrint, [FaAdmin]=@FaAdmin, [FAAKtualTerminUpdate]=@FAAKtualTerminUpdate, [FaAnalysis]=@FaAnalysis, [FAAuswertungEndkontrolle]=@FAAuswertungEndkontrolle, [FABemerkungPlannug]=@FABemerkungPlannug, [FABemerkungZuGewerk]=@FABemerkungZuGewerk, [FABemerkungZuPrio]=@FABemerkungZuPrio, [FACancelHorizon1]=@FACancelHorizon1, [FACancelHorizon2]=@FACancelHorizon2, [FACancelHorizon3]=@FACancelHorizon3, [FACommissionert]=@FACommissionert, [FaCreate]=@FaCreate, [FACreateHorizon1]=@FACreateHorizon1, [FACreateHorizon2]=@FACreateHorizon2, [FACreateHorizon3]=@FACreateHorizon3, [FaDatenEdit]=@FaDatenEdit, [FaDatenView]=@FaDatenView, [FaDelete]=@FaDelete, [FADrucken]=@FADrucken, [FaEdit]=@FaEdit, [FAErlidegen]=@FAErlidegen, [FAExcelUpdateWerk]=@FAExcelUpdateWerk, [FAExcelUpdateWunsh]=@FAExcelUpdateWunsh, [FAFehlrMaterial]=@FAFehlrMaterial, [FaHomeAnalysis]=@FaHomeAnalysis, [FaHomeUpdate]=@FaHomeUpdate, [FALaufkarteSchneiderei]=@FALaufkarteSchneiderei, [FaPlanningEdit]=@FaPlanningEdit, [FaPlanningView]=@FaPlanningView, [FAPlannung]=@FAPlannung, [FAPlannungTechnick]=@FAPlannungTechnick, [FAPriesZeitUpdate]=@FAPriesZeitUpdate, [FAProductionPlannung]=@FAProductionPlannung, [FAStappleDruck]=@FAStappleDruck, [FAStatusAlbania]=@FAStatusAlbania, [FAStatusCzech]=@FAStatusCzech, [FAStatusTunisia]=@FAStatusTunisia, [FAStorno]=@FAStorno, [FAStucklist]=@FAStucklist, [FaTechnicEdit]=@FaTechnicEdit, [FaTechnicView]=@FaTechnicView, [FATerminWerk]=@FATerminWerk, [FAUpdateBemerkungExtern]=@FAUpdateBemerkungExtern, [FAUpdateByArticle]=@FAUpdateByArticle, [FAUpdateByFA]=@FAUpdateByFA, [FAUpdateTerminHorizon1]=@FAUpdateTerminHorizon1, [FAUpdateTerminHorizon2]=@FAUpdateTerminHorizon2, [FAUpdateTerminHorizon3]=@FAUpdateTerminHorizon3, [FAWerkWunshAdmin]=@FAWerkWunshAdmin, [Fertigung]=@Fertigung, [FertigungLog]=@FertigungLog, [ForcastCreate]=@ForcastCreate, [ForcastDelete]=@ForcastDelete, [ForcastEdit]=@ForcastEdit, [ForcastLog]=@ForcastLog, [ForcastPositionEdit]=@ForcastPositionEdit, [ForcastReport]=@ForcastReport, [ForcastView]=@ForcastView, [FRCPosHorizon1]=@FRCPosHorizon1, [FRCPosHorizon2]=@FRCPosHorizon2, [FRCPosHorizon3]=@FRCPosHorizon3, [GSPosHorizon1]=@GSPosHorizon1, [GSPosHorizon2]=@GSPosHorizon2, [GSPosHorizon3]=@GSPosHorizon3, [GutschriftBookedEdit]=@GutschriftBookedEdit, [GutschriftCreate]=@GutschriftCreate, [GutschriftDelete]=@GutschriftDelete, [GutschriftDoneEdit]=@GutschriftDoneEdit, [GutschriftEdit]=@GutschriftEdit, [GutschriftLog]=@GutschriftLog, [GutschriftPositionEdit]=@GutschriftPositionEdit, [GutschriftReport]=@GutschriftReport, [GutschriftView]=@GutschriftView, [InsideSalesChecks]=@InsideSalesChecks, [InsideSalesChecksArchive]=@InsideSalesChecksArchive, [InsideSalesCustomerSummary]=@InsideSalesCustomerSummary, [InsideSalesMinimumStockEvaluation]=@InsideSalesMinimumStockEvaluation, [InsideSalesMinimumStockEvaluationTable]=@InsideSalesMinimumStockEvaluationTable, [InsideSalesOverdueOrders]=@InsideSalesOverdueOrders, [InsideSalesOverdueOrdersTable]=@InsideSalesOverdueOrdersTable, [InsideSalesTotalUnbookedOrders]=@InsideSalesTotalUnbookedOrders, [InsideSalesTotalUnbookedOrdersTable]=@InsideSalesTotalUnbookedOrdersTable, [InsideSalesTurnoverCurrentWeek]=@InsideSalesTurnoverCurrentWeek, [IsDefault]=@IsDefault, [LSPosHorizon1]=@LSPosHorizon1, [LSPosHorizon2]=@LSPosHorizon2, [LSPosHorizon3]=@LSPosHorizon3, [mId]=@mId, [ModuleActivated]=@ModuleActivated, [OrderProcessing]=@OrderProcessing, [OrderProcessingLog]=@OrderProcessingLog, [Rahmen]=@Rahmen, [RahmenAdd]=@RahmenAdd, [RahmenAddAB]=@RahmenAddAB, [RahmenAddPositions]=@RahmenAddPositions, [RahmenCancelation]=@RahmenCancelation, [RahmenClosure]=@RahmenClosure, [RahmenDelete]=@RahmenDelete, [RahmenDeletePositions]=@RahmenDeletePositions, [RahmenDocumentFlow]=@RahmenDocumentFlow, [RahmenEditHeader]=@RahmenEditHeader, [RahmenEditPositions]=@RahmenEditPositions, [RahmenHistory]=@RahmenHistory, [RahmenValdation]=@RahmenValdation, [RAPosHorizon1]=@RAPosHorizon1, [RAPosHorizon2]=@RAPosHorizon2, [RAPosHorizon3]=@RAPosHorizon3, [Rechnung]=@Rechnung, [RechnungAutoCreation]=@RechnungAutoCreation, [RechnungBookedEdit]=@RechnungBookedEdit, [RechnungConfig]=@RechnungConfig, [RechnungDelete]=@RechnungDelete, [RechnungDoneEdit]=@RechnungDoneEdit, [RechnungManualCreation]=@RechnungManualCreation, [RechnungReport]=@RechnungReport, [RechnungSend]=@RechnungSend, [RechnungValidate]=@RechnungValidate, [RGPosHorizon1]=@RGPosHorizon1, [RGPosHorizon2]=@RGPosHorizon2, [RGPosHorizon3]=@RGPosHorizon3, [Statistics]=@Statistics, [StatsBacklogFGAdmin]=@StatsBacklogFGAdmin, [StatsBacklogHWAdmin]=@StatsBacklogHWAdmin, [StatsCapaCutting]=@StatsCapaCutting, [StatsCapaHorizons]=@StatsCapaHorizons, [StatsCapaLong]=@StatsCapaLong, [StatsCapaPlanning]=@StatsCapaPlanning, [StatsCapaShort]=@StatsCapaShort, [StatsRechnungAL]=@StatsRechnungAL, [StatsRechnungBETN]=@StatsRechnungBETN, [StatsRechnungCZ]=@StatsRechnungCZ, [StatsRechnungDE]=@StatsRechnungDE, [StatsRechnungGZTN]=@StatsRechnungGZTN, [StatsRechnungTN]=@StatsRechnungTN, [StatsRechnungWS]=@StatsRechnungWS, [StatsStockCS]=@StatsStockCS, [StatsStockExternalWarehouse]=@StatsStockExternalWarehouse, [StatsStockFG]=@StatsStockFG, [UBGStatusChange]=@UBGStatusChange WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AB_LT", item.AB_LT == null ? (object)DBNull.Value : item.AB_LT);
				sqlCommand.Parameters.AddWithValue("AB_LT_EDI", item.AB_LT_EDI == null ? (object)DBNull.Value : item.AB_LT_EDI);
				sqlCommand.Parameters.AddWithValue("ABPosHorizon1", item.ABPosHorizon1 == null ? (object)DBNull.Value : item.ABPosHorizon1);
				sqlCommand.Parameters.AddWithValue("ABPosHorizon2", item.ABPosHorizon2 == null ? (object)DBNull.Value : item.ABPosHorizon2);
				sqlCommand.Parameters.AddWithValue("ABPosHorizon3", item.ABPosHorizon3 == null ? (object)DBNull.Value : item.ABPosHorizon3);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
				sqlCommand.Parameters.AddWithValue("BVBookedEdit", item.BVBookedEdit == null ? (object)DBNull.Value : item.BVBookedEdit);
				sqlCommand.Parameters.AddWithValue("BVDoneEdit", item.BVDoneEdit == null ? (object)DBNull.Value : item.BVDoneEdit);
				sqlCommand.Parameters.AddWithValue("BVFaCreate", item.BVFaCreate == null ? (object)DBNull.Value : item.BVFaCreate);
				sqlCommand.Parameters.AddWithValue("Configuration", item.Configuration == null ? (object)DBNull.Value : item.Configuration);
				sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments", item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
				sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees", item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
				sqlCommand.Parameters.AddWithValue("ConfigurationReplacements", item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
				sqlCommand.Parameters.AddWithValue("ConfigurationReporting", item.ConfigurationReporting == null ? (object)DBNull.Value : item.ConfigurationReporting);
				sqlCommand.Parameters.AddWithValue("ConfirmationBookedEdit", item.ConfirmationBookedEdit == null ? (object)DBNull.Value : item.ConfirmationBookedEdit);
				sqlCommand.Parameters.AddWithValue("ConfirmationCreate", item.ConfirmationCreate == null ? (object)DBNull.Value : item.ConfirmationCreate);
				sqlCommand.Parameters.AddWithValue("ConfirmationDelete", item.ConfirmationDelete == null ? (object)DBNull.Value : item.ConfirmationDelete);
				sqlCommand.Parameters.AddWithValue("ConfirmationDeliveryNote", item.ConfirmationDeliveryNote == null ? (object)DBNull.Value : item.ConfirmationDeliveryNote);
				sqlCommand.Parameters.AddWithValue("ConfirmationDoneEdit", item.ConfirmationDoneEdit == null ? (object)DBNull.Value : item.ConfirmationDoneEdit);
				sqlCommand.Parameters.AddWithValue("ConfirmationEdit", item.ConfirmationEdit == null ? (object)DBNull.Value : item.ConfirmationEdit);
				sqlCommand.Parameters.AddWithValue("ConfirmationPositionEdit", item.ConfirmationPositionEdit == null ? (object)DBNull.Value : item.ConfirmationPositionEdit);
				sqlCommand.Parameters.AddWithValue("ConfirmationPositionProduction", item.ConfirmationPositionProduction == null ? (object)DBNull.Value : item.ConfirmationPositionProduction);
				sqlCommand.Parameters.AddWithValue("ConfirmationReport", item.ConfirmationReport == null ? (object)DBNull.Value : item.ConfirmationReport);
				sqlCommand.Parameters.AddWithValue("ConfirmationValidate", item.ConfirmationValidate == null ? (object)DBNull.Value : item.ConfirmationValidate);
				sqlCommand.Parameters.AddWithValue("ConfirmationView", item.ConfirmationView == null ? (object)DBNull.Value : item.ConfirmationView);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CSInfoEdit", item.CSInfoEdit == null ? (object)DBNull.Value : item.CSInfoEdit);
				sqlCommand.Parameters.AddWithValue("DelforCreate", item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
				sqlCommand.Parameters.AddWithValue("DelforDelete", item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
				sqlCommand.Parameters.AddWithValue("DelforDeletePosition", item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
				sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation", item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
				sqlCommand.Parameters.AddWithValue("DelforReport", item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
				sqlCommand.Parameters.AddWithValue("DelforStatistics", item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
				sqlCommand.Parameters.AddWithValue("DelforView", item.DelforView == null ? (object)DBNull.Value : item.DelforView);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteBookedEdit", item.DeliveryNoteBookedEdit == null ? (object)DBNull.Value : item.DeliveryNoteBookedEdit);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteCreate", item.DeliveryNoteCreate == null ? (object)DBNull.Value : item.DeliveryNoteCreate);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteDelete", item.DeliveryNoteDelete == null ? (object)DBNull.Value : item.DeliveryNoteDelete);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteDoneEdit", item.DeliveryNoteDoneEdit == null ? (object)DBNull.Value : item.DeliveryNoteDoneEdit);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteEdit", item.DeliveryNoteEdit == null ? (object)DBNull.Value : item.DeliveryNoteEdit);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteLog", item.DeliveryNoteLog == null ? (object)DBNull.Value : item.DeliveryNoteLog);
				sqlCommand.Parameters.AddWithValue("DeliveryNotePositionEdit", item.DeliveryNotePositionEdit == null ? (object)DBNull.Value : item.DeliveryNotePositionEdit);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteReport", item.DeliveryNoteReport == null ? (object)DBNull.Value : item.DeliveryNoteReport);
				sqlCommand.Parameters.AddWithValue("DeliveryNoteView", item.DeliveryNoteView == null ? (object)DBNull.Value : item.DeliveryNoteView);
				sqlCommand.Parameters.AddWithValue("DLFPosHorizon1", item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
				sqlCommand.Parameters.AddWithValue("DLFPosHorizon2", item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
				sqlCommand.Parameters.AddWithValue("DLFPosHorizon3", item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
				sqlCommand.Parameters.AddWithValue("EDI", item.EDI == null ? (object)DBNull.Value : item.EDI);
				sqlCommand.Parameters.AddWithValue("EDIDownloadFile", item.EDIDownloadFile == null ? (object)DBNull.Value : item.EDIDownloadFile);
				sqlCommand.Parameters.AddWithValue("EDIError", item.EDIError == null ? (object)DBNull.Value : item.EDIError);
				sqlCommand.Parameters.AddWithValue("EDIErrorEdit", item.EDIErrorEdit == null ? (object)DBNull.Value : item.EDIErrorEdit);
				sqlCommand.Parameters.AddWithValue("EDIErrorValidated", item.EDIErrorValidated == null ? (object)DBNull.Value : item.EDIErrorValidated);
				sqlCommand.Parameters.AddWithValue("EDILogOrderValidated", item.EDILogOrderValidated == null ? (object)DBNull.Value : item.EDILogOrderValidated);
				sqlCommand.Parameters.AddWithValue("EDIOrder", item.EDIOrder == null ? (object)DBNull.Value : item.EDIOrder);
				sqlCommand.Parameters.AddWithValue("EDIOrderEdit", item.EDIOrderEdit == null ? (object)DBNull.Value : item.EDIOrderEdit);
				sqlCommand.Parameters.AddWithValue("EDIOrderPositionEdit", item.EDIOrderPositionEdit == null ? (object)DBNull.Value : item.EDIOrderPositionEdit);
				sqlCommand.Parameters.AddWithValue("EDIOrderProduction", item.EDIOrderProduction == null ? (object)DBNull.Value : item.EDIOrderProduction);
				sqlCommand.Parameters.AddWithValue("EDIOrderProductionPosition", item.EDIOrderProductionPosition == null ? (object)DBNull.Value : item.EDIOrderProductionPosition);
				sqlCommand.Parameters.AddWithValue("EDIOrderReport", item.EDIOrderReport == null ? (object)DBNull.Value : item.EDIOrderReport);
				sqlCommand.Parameters.AddWithValue("EDIOrderValidated", item.EDIOrderValidated == null ? (object)DBNull.Value : item.EDIOrderValidated);
				sqlCommand.Parameters.AddWithValue("EDIOrderValidatedEdit", item.EDIOrderValidatedEdit == null ? (object)DBNull.Value : item.EDIOrderValidatedEdit);
				sqlCommand.Parameters.AddWithValue("FaABEdit", item.FaABEdit == null ? (object)DBNull.Value : item.FaABEdit);
				sqlCommand.Parameters.AddWithValue("FaABView", item.FaABView == null ? (object)DBNull.Value : item.FaABView);
				sqlCommand.Parameters.AddWithValue("FaActionBook", item.FaActionBook == null ? (object)DBNull.Value : item.FaActionBook);
				sqlCommand.Parameters.AddWithValue("FaActionComplete", item.FaActionComplete == null ? (object)DBNull.Value : item.FaActionComplete);
				sqlCommand.Parameters.AddWithValue("FaActionDelete", item.FaActionDelete == null ? (object)DBNull.Value : item.FaActionDelete);
				sqlCommand.Parameters.AddWithValue("FaActionPrint", item.FaActionPrint == null ? (object)DBNull.Value : item.FaActionPrint);
				sqlCommand.Parameters.AddWithValue("FaAdmin", item.FaAdmin == null ? (object)DBNull.Value : item.FaAdmin);
				sqlCommand.Parameters.AddWithValue("FAAKtualTerminUpdate", item.FAAKtualTerminUpdate == null ? (object)DBNull.Value : item.FAAKtualTerminUpdate);
				sqlCommand.Parameters.AddWithValue("FaAnalysis", item.FaAnalysis == null ? (object)DBNull.Value : item.FaAnalysis);
				sqlCommand.Parameters.AddWithValue("FAAuswertungEndkontrolle", item.FAAuswertungEndkontrolle == null ? (object)DBNull.Value : item.FAAuswertungEndkontrolle);
				sqlCommand.Parameters.AddWithValue("FABemerkungPlannug", item.FABemerkungPlannug == null ? (object)DBNull.Value : item.FABemerkungPlannug);
				sqlCommand.Parameters.AddWithValue("FABemerkungZuGewerk", item.FABemerkungZuGewerk == null ? (object)DBNull.Value : item.FABemerkungZuGewerk);
				sqlCommand.Parameters.AddWithValue("FABemerkungZuPrio", item.FABemerkungZuPrio == null ? (object)DBNull.Value : item.FABemerkungZuPrio);
				sqlCommand.Parameters.AddWithValue("FACancelHorizon1", item.FACancelHorizon1 == null ? (object)DBNull.Value : item.FACancelHorizon1);
				sqlCommand.Parameters.AddWithValue("FACancelHorizon2", item.FACancelHorizon2 == null ? (object)DBNull.Value : item.FACancelHorizon2);
				sqlCommand.Parameters.AddWithValue("FACancelHorizon3", item.FACancelHorizon3 == null ? (object)DBNull.Value : item.FACancelHorizon3);
				sqlCommand.Parameters.AddWithValue("FACommissionert", item.FACommissionert == null ? (object)DBNull.Value : item.FACommissionert);
				sqlCommand.Parameters.AddWithValue("FaCreate", item.FaCreate == null ? (object)DBNull.Value : item.FaCreate);
				sqlCommand.Parameters.AddWithValue("FACreateHorizon1", item.FACreateHorizon1 == null ? (object)DBNull.Value : item.FACreateHorizon1);
				sqlCommand.Parameters.AddWithValue("FACreateHorizon2", item.FACreateHorizon2 == null ? (object)DBNull.Value : item.FACreateHorizon2);
				sqlCommand.Parameters.AddWithValue("FACreateHorizon3", item.FACreateHorizon3 == null ? (object)DBNull.Value : item.FACreateHorizon3);
				sqlCommand.Parameters.AddWithValue("FaDatenEdit", item.FaDatenEdit == null ? (object)DBNull.Value : item.FaDatenEdit);
				sqlCommand.Parameters.AddWithValue("FaDatenView", item.FaDatenView == null ? (object)DBNull.Value : item.FaDatenView);
				sqlCommand.Parameters.AddWithValue("FaDelete", item.FaDelete == null ? (object)DBNull.Value : item.FaDelete);
				sqlCommand.Parameters.AddWithValue("FADrucken", item.FADrucken == null ? (object)DBNull.Value : item.FADrucken);
				sqlCommand.Parameters.AddWithValue("FaEdit", item.FaEdit == null ? (object)DBNull.Value : item.FaEdit);
				sqlCommand.Parameters.AddWithValue("FAErlidegen", item.FAErlidegen == null ? (object)DBNull.Value : item.FAErlidegen);
				sqlCommand.Parameters.AddWithValue("FAExcelUpdateWerk", item.FAExcelUpdateWerk == null ? (object)DBNull.Value : item.FAExcelUpdateWerk);
				sqlCommand.Parameters.AddWithValue("FAExcelUpdateWunsh", item.FAExcelUpdateWunsh == null ? (object)DBNull.Value : item.FAExcelUpdateWunsh);
				sqlCommand.Parameters.AddWithValue("FAFehlrMaterial", item.FAFehlrMaterial == null ? (object)DBNull.Value : item.FAFehlrMaterial);
				sqlCommand.Parameters.AddWithValue("FaHomeAnalysis", item.FaHomeAnalysis == null ? (object)DBNull.Value : item.FaHomeAnalysis);
				sqlCommand.Parameters.AddWithValue("FaHomeUpdate", item.FaHomeUpdate == null ? (object)DBNull.Value : item.FaHomeUpdate);
				sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei", item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
				sqlCommand.Parameters.AddWithValue("FaPlanningEdit", item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
				sqlCommand.Parameters.AddWithValue("FaPlanningView", item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
				sqlCommand.Parameters.AddWithValue("FAPlannung", item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
				sqlCommand.Parameters.AddWithValue("FAPlannungTechnick", item.FAPlannungTechnick == null ? (object)DBNull.Value : item.FAPlannungTechnick);
				sqlCommand.Parameters.AddWithValue("FAPriesZeitUpdate", item.FAPriesZeitUpdate == null ? (object)DBNull.Value : item.FAPriesZeitUpdate);
				sqlCommand.Parameters.AddWithValue("FAProductionPlannung", item.FAProductionPlannung == null ? (object)DBNull.Value : item.FAProductionPlannung);
				sqlCommand.Parameters.AddWithValue("FAStappleDruck", item.FAStappleDruck == null ? (object)DBNull.Value : item.FAStappleDruck);
				sqlCommand.Parameters.AddWithValue("FAStatusAlbania", item.FAStatusAlbania == null ? (object)DBNull.Value : item.FAStatusAlbania);
				sqlCommand.Parameters.AddWithValue("FAStatusCzech", item.FAStatusCzech == null ? (object)DBNull.Value : item.FAStatusCzech);
				sqlCommand.Parameters.AddWithValue("FAStatusTunisia", item.FAStatusTunisia == null ? (object)DBNull.Value : item.FAStatusTunisia);
				sqlCommand.Parameters.AddWithValue("FAStorno", item.FAStorno == null ? (object)DBNull.Value : item.FAStorno);
				sqlCommand.Parameters.AddWithValue("FAStucklist", item.FAStucklist == null ? (object)DBNull.Value : item.FAStucklist);
				sqlCommand.Parameters.AddWithValue("FaTechnicEdit", item.FaTechnicEdit == null ? (object)DBNull.Value : item.FaTechnicEdit);
				sqlCommand.Parameters.AddWithValue("FaTechnicView", item.FaTechnicView == null ? (object)DBNull.Value : item.FaTechnicView);
				sqlCommand.Parameters.AddWithValue("FATerminWerk", item.FATerminWerk == null ? (object)DBNull.Value : item.FATerminWerk);
				sqlCommand.Parameters.AddWithValue("FAUpdateBemerkungExtern", item.FAUpdateBemerkungExtern == null ? (object)DBNull.Value : item.FAUpdateBemerkungExtern);
				sqlCommand.Parameters.AddWithValue("FAUpdateByArticle", item.FAUpdateByArticle == null ? (object)DBNull.Value : item.FAUpdateByArticle);
				sqlCommand.Parameters.AddWithValue("FAUpdateByFA", item.FAUpdateByFA == null ? (object)DBNull.Value : item.FAUpdateByFA);
				sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1", item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
				sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2", item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
				sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3", item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
				sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin", item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
				sqlCommand.Parameters.AddWithValue("Fertigung", item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
				sqlCommand.Parameters.AddWithValue("FertigungLog", item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
				sqlCommand.Parameters.AddWithValue("ForcastCreate", item.ForcastCreate == null ? (object)DBNull.Value : item.ForcastCreate);
				sqlCommand.Parameters.AddWithValue("ForcastDelete", item.ForcastDelete == null ? (object)DBNull.Value : item.ForcastDelete);
				sqlCommand.Parameters.AddWithValue("ForcastEdit", item.ForcastEdit == null ? (object)DBNull.Value : item.ForcastEdit);
				sqlCommand.Parameters.AddWithValue("ForcastLog", item.ForcastLog == null ? (object)DBNull.Value : item.ForcastLog);
				sqlCommand.Parameters.AddWithValue("ForcastPositionEdit", item.ForcastPositionEdit == null ? (object)DBNull.Value : item.ForcastPositionEdit);
				sqlCommand.Parameters.AddWithValue("ForcastReport", item.ForcastReport == null ? (object)DBNull.Value : item.ForcastReport);
				sqlCommand.Parameters.AddWithValue("ForcastView", item.ForcastView == null ? (object)DBNull.Value : item.ForcastView);
				sqlCommand.Parameters.AddWithValue("FRCPosHorizon1", item.FRCPosHorizon1 == null ? (object)DBNull.Value : item.FRCPosHorizon1);
				sqlCommand.Parameters.AddWithValue("FRCPosHorizon2", item.FRCPosHorizon2 == null ? (object)DBNull.Value : item.FRCPosHorizon2);
				sqlCommand.Parameters.AddWithValue("FRCPosHorizon3", item.FRCPosHorizon3 == null ? (object)DBNull.Value : item.FRCPosHorizon3);
				sqlCommand.Parameters.AddWithValue("GSPosHorizon1", item.GSPosHorizon1 == null ? (object)DBNull.Value : item.GSPosHorizon1);
				sqlCommand.Parameters.AddWithValue("GSPosHorizon2", item.GSPosHorizon2 == null ? (object)DBNull.Value : item.GSPosHorizon2);
				sqlCommand.Parameters.AddWithValue("GSPosHorizon3", item.GSPosHorizon3 == null ? (object)DBNull.Value : item.GSPosHorizon3);
				sqlCommand.Parameters.AddWithValue("GutschriftBookedEdit", item.GutschriftBookedEdit == null ? (object)DBNull.Value : item.GutschriftBookedEdit);
				sqlCommand.Parameters.AddWithValue("GutschriftCreate", item.GutschriftCreate == null ? (object)DBNull.Value : item.GutschriftCreate);
				sqlCommand.Parameters.AddWithValue("GutschriftDelete", item.GutschriftDelete == null ? (object)DBNull.Value : item.GutschriftDelete);
				sqlCommand.Parameters.AddWithValue("GutschriftDoneEdit", item.GutschriftDoneEdit == null ? (object)DBNull.Value : item.GutschriftDoneEdit);
				sqlCommand.Parameters.AddWithValue("GutschriftEdit", item.GutschriftEdit == null ? (object)DBNull.Value : item.GutschriftEdit);
				sqlCommand.Parameters.AddWithValue("GutschriftLog", item.GutschriftLog == null ? (object)DBNull.Value : item.GutschriftLog);
				sqlCommand.Parameters.AddWithValue("GutschriftPositionEdit", item.GutschriftPositionEdit == null ? (object)DBNull.Value : item.GutschriftPositionEdit);
				sqlCommand.Parameters.AddWithValue("GutschriftReport", item.GutschriftReport == null ? (object)DBNull.Value : item.GutschriftReport);
				sqlCommand.Parameters.AddWithValue("GutschriftView", item.GutschriftView == null ? (object)DBNull.Value : item.GutschriftView);
				sqlCommand.Parameters.AddWithValue("InsideSalesChecks", item.InsideSalesChecks == null ? (object)DBNull.Value : item.InsideSalesChecks);
				sqlCommand.Parameters.AddWithValue("InsideSalesChecksArchive", item.InsideSalesChecksArchive == null ? (object)DBNull.Value : item.InsideSalesChecksArchive);
				sqlCommand.Parameters.AddWithValue("InsideSalesCustomerSummary", item.InsideSalesCustomerSummary == null ? (object)DBNull.Value : item.InsideSalesCustomerSummary);
				sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluation", item.InsideSalesMinimumStockEvaluation == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluation);
				sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluationTable", item.InsideSalesMinimumStockEvaluationTable == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluationTable);
				sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrders", item.InsideSalesOverdueOrders == null ? (object)DBNull.Value : item.InsideSalesOverdueOrders);
				sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrdersTable", item.InsideSalesOverdueOrdersTable == null ? (object)DBNull.Value : item.InsideSalesOverdueOrdersTable);
				sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrders", item.InsideSalesTotalUnbookedOrders == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrders);
				sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrdersTable", item.InsideSalesTotalUnbookedOrdersTable == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrdersTable);
				sqlCommand.Parameters.AddWithValue("InsideSalesTurnoverCurrentWeek", item.InsideSalesTurnoverCurrentWeek == null ? (object)DBNull.Value : item.InsideSalesTurnoverCurrentWeek);
				sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
				sqlCommand.Parameters.AddWithValue("LSPosHorizon1", item.LSPosHorizon1 == null ? (object)DBNull.Value : item.LSPosHorizon1);
				sqlCommand.Parameters.AddWithValue("LSPosHorizon2", item.LSPosHorizon2 == null ? (object)DBNull.Value : item.LSPosHorizon2);
				sqlCommand.Parameters.AddWithValue("LSPosHorizon3", item.LSPosHorizon3 == null ? (object)DBNull.Value : item.LSPosHorizon3);
				sqlCommand.Parameters.AddWithValue("mId", item.mId == null ? (object)DBNull.Value : item.mId);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("OrderProcessing", item.OrderProcessing == null ? (object)DBNull.Value : item.OrderProcessing);
				sqlCommand.Parameters.AddWithValue("OrderProcessingLog", item.OrderProcessingLog == null ? (object)DBNull.Value : item.OrderProcessingLog);
				sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
				sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
				sqlCommand.Parameters.AddWithValue("RahmenAddAB", item.RahmenAddAB == null ? (object)DBNull.Value : item.RahmenAddAB);
				sqlCommand.Parameters.AddWithValue("RahmenAddPositions", item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
				sqlCommand.Parameters.AddWithValue("RahmenCancelation", item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
				sqlCommand.Parameters.AddWithValue("RahmenClosure", item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
				sqlCommand.Parameters.AddWithValue("RahmenDelete", item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
				sqlCommand.Parameters.AddWithValue("RahmenDeletePositions", item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
				sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow", item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
				sqlCommand.Parameters.AddWithValue("RahmenEditHeader", item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
				sqlCommand.Parameters.AddWithValue("RahmenEditPositions", item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
				sqlCommand.Parameters.AddWithValue("RahmenHistory", item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
				sqlCommand.Parameters.AddWithValue("RahmenValdation", item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
				sqlCommand.Parameters.AddWithValue("RAPosHorizon1", item.RAPosHorizon1 == null ? (object)DBNull.Value : item.RAPosHorizon1);
				sqlCommand.Parameters.AddWithValue("RAPosHorizon2", item.RAPosHorizon2 == null ? (object)DBNull.Value : item.RAPosHorizon2);
				sqlCommand.Parameters.AddWithValue("RAPosHorizon3", item.RAPosHorizon3 == null ? (object)DBNull.Value : item.RAPosHorizon3);
				sqlCommand.Parameters.AddWithValue("Rechnung", item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
				sqlCommand.Parameters.AddWithValue("RechnungAutoCreation", item.RechnungAutoCreation == null ? (object)DBNull.Value : item.RechnungAutoCreation);
				sqlCommand.Parameters.AddWithValue("RechnungBookedEdit", item.RechnungBookedEdit == null ? (object)DBNull.Value : item.RechnungBookedEdit);
				sqlCommand.Parameters.AddWithValue("RechnungConfig", item.RechnungConfig == null ? (object)DBNull.Value : item.RechnungConfig);
				sqlCommand.Parameters.AddWithValue("RechnungDelete", item.RechnungDelete == null ? (object)DBNull.Value : item.RechnungDelete);
				sqlCommand.Parameters.AddWithValue("RechnungDoneEdit", item.RechnungDoneEdit == null ? (object)DBNull.Value : item.RechnungDoneEdit);
				sqlCommand.Parameters.AddWithValue("RechnungManualCreation", item.RechnungManualCreation == null ? (object)DBNull.Value : item.RechnungManualCreation);
				sqlCommand.Parameters.AddWithValue("RechnungReport", item.RechnungReport == null ? (object)DBNull.Value : item.RechnungReport);
				sqlCommand.Parameters.AddWithValue("RechnungSend", item.RechnungSend == null ? (object)DBNull.Value : item.RechnungSend);
				sqlCommand.Parameters.AddWithValue("RechnungValidate", item.RechnungValidate == null ? (object)DBNull.Value : item.RechnungValidate);
				sqlCommand.Parameters.AddWithValue("RGPosHorizon1", item.RGPosHorizon1 == null ? (object)DBNull.Value : item.RGPosHorizon1);
				sqlCommand.Parameters.AddWithValue("RGPosHorizon2", item.RGPosHorizon2 == null ? (object)DBNull.Value : item.RGPosHorizon2);
				sqlCommand.Parameters.AddWithValue("RGPosHorizon3", item.RGPosHorizon3 == null ? (object)DBNull.Value : item.RGPosHorizon3);
				sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
				sqlCommand.Parameters.AddWithValue("StatsBacklogFGAdmin", item.StatsBacklogFGAdmin == null ? (object)DBNull.Value : item.StatsBacklogFGAdmin);
				sqlCommand.Parameters.AddWithValue("StatsBacklogHWAdmin", item.StatsBacklogHWAdmin == null ? (object)DBNull.Value : item.StatsBacklogHWAdmin);
				sqlCommand.Parameters.AddWithValue("StatsCapaCutting", item.StatsCapaCutting == null ? (object)DBNull.Value : item.StatsCapaCutting);
				sqlCommand.Parameters.AddWithValue("StatsCapaHorizons", item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
				sqlCommand.Parameters.AddWithValue("StatsCapaLong", item.StatsCapaLong == null ? (object)DBNull.Value : item.StatsCapaLong);
				sqlCommand.Parameters.AddWithValue("StatsCapaPlanning", item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
				sqlCommand.Parameters.AddWithValue("StatsCapaShort", item.StatsCapaShort == null ? (object)DBNull.Value : item.StatsCapaShort);
				sqlCommand.Parameters.AddWithValue("StatsRechnungAL", item.StatsRechnungAL == null ? (object)DBNull.Value : item.StatsRechnungAL);
				sqlCommand.Parameters.AddWithValue("StatsRechnungBETN", item.StatsRechnungBETN == null ? (object)DBNull.Value : item.StatsRechnungBETN);
				sqlCommand.Parameters.AddWithValue("StatsRechnungCZ", item.StatsRechnungCZ == null ? (object)DBNull.Value : item.StatsRechnungCZ);
				sqlCommand.Parameters.AddWithValue("StatsRechnungDE", item.StatsRechnungDE == null ? (object)DBNull.Value : item.StatsRechnungDE);
				sqlCommand.Parameters.AddWithValue("StatsRechnungGZTN", item.StatsRechnungGZTN == null ? (object)DBNull.Value : item.StatsRechnungGZTN);
				sqlCommand.Parameters.AddWithValue("StatsRechnungTN", item.StatsRechnungTN == null ? (object)DBNull.Value : item.StatsRechnungTN);
				sqlCommand.Parameters.AddWithValue("StatsRechnungWS", item.StatsRechnungWS == null ? (object)DBNull.Value : item.StatsRechnungWS);
				sqlCommand.Parameters.AddWithValue("StatsStockCS", item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
				sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse", item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
				sqlCommand.Parameters.AddWithValue("StatsStockFG", item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
				sqlCommand.Parameters.AddWithValue("UBGStatusChange", item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 208; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = update(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__CTS_AccessProfile] SET "

							+ "[AB_LT]=@AB_LT" + i + ","
							+ "[AB_LT_EDI]=@AB_LT_EDI" + i + ","
							+ "[ABPosHorizon1]=@ABPosHorizon1" + i + ","
							+ "[ABPosHorizon2]=@ABPosHorizon2" + i + ","
							+ "[ABPosHorizon3]=@ABPosHorizon3" + i + ","
							+ "[AccessProfileName]=@AccessProfileName" + i + ","
							+ "[Administration]=@Administration" + i + ","
							+ "[BVBookedEdit]=@BVBookedEdit" + i + ","
							+ "[BVDoneEdit]=@BVDoneEdit" + i + ","
							+ "[BVFaCreate]=@BVFaCreate" + i + ","
							+ "[Configuration]=@Configuration" + i + ","
							+ "[ConfigurationAppoitments]=@ConfigurationAppoitments" + i + ","
							+ "[ConfigurationChangeEmployees]=@ConfigurationChangeEmployees" + i + ","
							+ "[ConfigurationReplacements]=@ConfigurationReplacements" + i + ","
							+ "[ConfigurationReporting]=@ConfigurationReporting" + i + ","
							+ "[ConfirmationBookedEdit]=@ConfirmationBookedEdit" + i + ","
							+ "[ConfirmationCreate]=@ConfirmationCreate" + i + ","
							+ "[ConfirmationDelete]=@ConfirmationDelete" + i + ","
							+ "[ConfirmationDeliveryNote]=@ConfirmationDeliveryNote" + i + ","
							+ "[ConfirmationDoneEdit]=@ConfirmationDoneEdit" + i + ","
							+ "[ConfirmationEdit]=@ConfirmationEdit" + i + ","
							+ "[ConfirmationPositionEdit]=@ConfirmationPositionEdit" + i + ","
							+ "[ConfirmationPositionProduction]=@ConfirmationPositionProduction" + i + ","
							+ "[ConfirmationReport]=@ConfirmationReport" + i + ","
							+ "[ConfirmationValidate]=@ConfirmationValidate" + i + ","
							+ "[ConfirmationView]=@ConfirmationView" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CSInfoEdit]=@CSInfoEdit" + i + ","
							+ "[DelforCreate]=@DelforCreate" + i + ","
							+ "[DelforDelete]=@DelforDelete" + i + ","
							+ "[DelforDeletePosition]=@DelforDeletePosition" + i + ","
							+ "[DelforOrderConfirmation]=@DelforOrderConfirmation" + i + ","
							+ "[DelforReport]=@DelforReport" + i + ","
							+ "[DelforStatistics]=@DelforStatistics" + i + ","
							+ "[DelforView]=@DelforView" + i + ","
							+ "[DeliveryNoteBookedEdit]=@DeliveryNoteBookedEdit" + i + ","
							+ "[DeliveryNoteCreate]=@DeliveryNoteCreate" + i + ","
							+ "[DeliveryNoteDelete]=@DeliveryNoteDelete" + i + ","
							+ "[DeliveryNoteDoneEdit]=@DeliveryNoteDoneEdit" + i + ","
							+ "[DeliveryNoteEdit]=@DeliveryNoteEdit" + i + ","
							+ "[DeliveryNoteLog]=@DeliveryNoteLog" + i + ","
							+ "[DeliveryNotePositionEdit]=@DeliveryNotePositionEdit" + i + ","
							+ "[DeliveryNoteReport]=@DeliveryNoteReport" + i + ","
							+ "[DeliveryNoteView]=@DeliveryNoteView" + i + ","
							+ "[DLFPosHorizon1]=@DLFPosHorizon1" + i + ","
							+ "[DLFPosHorizon2]=@DLFPosHorizon2" + i + ","
							+ "[DLFPosHorizon3]=@DLFPosHorizon3" + i + ","
							+ "[EDI]=@EDI" + i + ","
							+ "[EDIDownloadFile]=@EDIDownloadFile" + i + ","
							+ "[EDIError]=@EDIError" + i + ","
							+ "[EDIErrorEdit]=@EDIErrorEdit" + i + ","
							+ "[EDIErrorValidated]=@EDIErrorValidated" + i + ","
							+ "[EDILogOrderValidated]=@EDILogOrderValidated" + i + ","
							+ "[EDIOrder]=@EDIOrder" + i + ","
							+ "[EDIOrderEdit]=@EDIOrderEdit" + i + ","
							+ "[EDIOrderPositionEdit]=@EDIOrderPositionEdit" + i + ","
							+ "[EDIOrderProduction]=@EDIOrderProduction" + i + ","
							+ "[EDIOrderProductionPosition]=@EDIOrderProductionPosition" + i + ","
							+ "[EDIOrderReport]=@EDIOrderReport" + i + ","
							+ "[EDIOrderValidated]=@EDIOrderValidated" + i + ","
							+ "[EDIOrderValidatedEdit]=@EDIOrderValidatedEdit" + i + ","
							+ "[FaABEdit]=@FaABEdit" + i + ","
							+ "[FaABView]=@FaABView" + i + ","
							+ "[FaActionBook]=@FaActionBook" + i + ","
							+ "[FaActionComplete]=@FaActionComplete" + i + ","
							+ "[FaActionDelete]=@FaActionDelete" + i + ","
							+ "[FaActionPrint]=@FaActionPrint" + i + ","
							+ "[FaAdmin]=@FaAdmin" + i + ","
							+ "[FAAKtualTerminUpdate]=@FAAKtualTerminUpdate" + i + ","
							+ "[FaAnalysis]=@FaAnalysis" + i + ","
							+ "[FAAuswertungEndkontrolle]=@FAAuswertungEndkontrolle" + i + ","
							+ "[FABemerkungPlannug]=@FABemerkungPlannug" + i + ","
							+ "[FABemerkungZuGewerk]=@FABemerkungZuGewerk" + i + ","
							+ "[FABemerkungZuPrio]=@FABemerkungZuPrio" + i + ","
							+ "[FACancelHorizon1]=@FACancelHorizon1" + i + ","
							+ "[FACancelHorizon2]=@FACancelHorizon2" + i + ","
							+ "[FACancelHorizon3]=@FACancelHorizon3" + i + ","
							+ "[FACommissionert]=@FACommissionert" + i + ","
							+ "[FaCreate]=@FaCreate" + i + ","
							+ "[FACreateHorizon1]=@FACreateHorizon1" + i + ","
							+ "[FACreateHorizon2]=@FACreateHorizon2" + i + ","
							+ "[FACreateHorizon3]=@FACreateHorizon3" + i + ","
							+ "[FaDatenEdit]=@FaDatenEdit" + i + ","
							+ "[FaDatenView]=@FaDatenView" + i + ","
							+ "[FaDelete]=@FaDelete" + i + ","
							+ "[FADrucken]=@FADrucken" + i + ","
							+ "[FaEdit]=@FaEdit" + i + ","
							+ "[FAErlidegen]=@FAErlidegen" + i + ","
							+ "[FAExcelUpdateWerk]=@FAExcelUpdateWerk" + i + ","
							+ "[FAExcelUpdateWunsh]=@FAExcelUpdateWunsh" + i + ","
							+ "[FAFehlrMaterial]=@FAFehlrMaterial" + i + ","
							+ "[FaHomeAnalysis]=@FaHomeAnalysis" + i + ","
							+ "[FaHomeUpdate]=@FaHomeUpdate" + i + ","
							+ "[FALaufkarteSchneiderei]=@FALaufkarteSchneiderei" + i + ","
							+ "[FaPlanningEdit]=@FaPlanningEdit" + i + ","
							+ "[FaPlanningView]=@FaPlanningView" + i + ","
							+ "[FAPlannung]=@FAPlannung" + i + ","
							+ "[FAPlannungTechnick]=@FAPlannungTechnick" + i + ","
							+ "[FAPriesZeitUpdate]=@FAPriesZeitUpdate" + i + ","
							+ "[FAProductionPlannung]=@FAProductionPlannung" + i + ","
							+ "[FAStappleDruck]=@FAStappleDruck" + i + ","
							+ "[FAStatusAlbania]=@FAStatusAlbania" + i + ","
							+ "[FAStatusCzech]=@FAStatusCzech" + i + ","
							+ "[FAStatusTunisia]=@FAStatusTunisia" + i + ","
							+ "[FAStorno]=@FAStorno" + i + ","
							+ "[FAStucklist]=@FAStucklist" + i + ","
							+ "[FaTechnicEdit]=@FaTechnicEdit" + i + ","
							+ "[FaTechnicView]=@FaTechnicView" + i + ","
							+ "[FATerminWerk]=@FATerminWerk" + i + ","
							+ "[FAUpdateBemerkungExtern]=@FAUpdateBemerkungExtern" + i + ","
							+ "[FAUpdateByArticle]=@FAUpdateByArticle" + i + ","
							+ "[FAUpdateByFA]=@FAUpdateByFA" + i + ","
							+ "[FAUpdateTerminHorizon1]=@FAUpdateTerminHorizon1" + i + ","
							+ "[FAUpdateTerminHorizon2]=@FAUpdateTerminHorizon2" + i + ","
							+ "[FAUpdateTerminHorizon3]=@FAUpdateTerminHorizon3" + i + ","
							+ "[FAWerkWunshAdmin]=@FAWerkWunshAdmin" + i + ","
							+ "[Fertigung]=@Fertigung" + i + ","
							+ "[FertigungLog]=@FertigungLog" + i + ","
							+ "[ForcastCreate]=@ForcastCreate" + i + ","
							+ "[ForcastDelete]=@ForcastDelete" + i + ","
							+ "[ForcastEdit]=@ForcastEdit" + i + ","
							+ "[ForcastLog]=@ForcastLog" + i + ","
							+ "[ForcastPositionEdit]=@ForcastPositionEdit" + i + ","
							+ "[ForcastReport]=@ForcastReport" + i + ","
							+ "[ForcastView]=@ForcastView" + i + ","
							+ "[FRCPosHorizon1]=@FRCPosHorizon1" + i + ","
							+ "[FRCPosHorizon2]=@FRCPosHorizon2" + i + ","
							+ "[FRCPosHorizon3]=@FRCPosHorizon3" + i + ","
							+ "[GSPosHorizon1]=@GSPosHorizon1" + i + ","
							+ "[GSPosHorizon2]=@GSPosHorizon2" + i + ","
							+ "[GSPosHorizon3]=@GSPosHorizon3" + i + ","
							+ "[GutschriftBookedEdit]=@GutschriftBookedEdit" + i + ","
							+ "[GutschriftCreate]=@GutschriftCreate" + i + ","
							+ "[GutschriftDelete]=@GutschriftDelete" + i + ","
							+ "[GutschriftDoneEdit]=@GutschriftDoneEdit" + i + ","
							+ "[GutschriftEdit]=@GutschriftEdit" + i + ","
							+ "[GutschriftLog]=@GutschriftLog" + i + ","
							+ "[GutschriftPositionEdit]=@GutschriftPositionEdit" + i + ","
							+ "[GutschriftReport]=@GutschriftReport" + i + ","
							+ "[GutschriftView]=@GutschriftView" + i + ","
							+ "[InsideSalesChecks]=@InsideSalesChecks" + i + ","
							+ "[InsideSalesChecksArchive]=@InsideSalesChecksArchive" + i + ","
							+ "[InsideSalesCustomerSummary]=@InsideSalesCustomerSummary" + i + ","
							+ "[InsideSalesMinimumStockEvaluation]=@InsideSalesMinimumStockEvaluation" + i + ","
							+ "[InsideSalesMinimumStockEvaluationTable]=@InsideSalesMinimumStockEvaluationTable" + i + ","
							+ "[InsideSalesOverdueOrders]=@InsideSalesOverdueOrders" + i + ","
							+ "[InsideSalesOverdueOrdersTable]=@InsideSalesOverdueOrdersTable" + i + ","
							+ "[InsideSalesTotalUnbookedOrders]=@InsideSalesTotalUnbookedOrders" + i + ","
							+ "[InsideSalesTotalUnbookedOrdersTable]=@InsideSalesTotalUnbookedOrdersTable" + i + ","
							+ "[InsideSalesTurnoverCurrentWeek]=@InsideSalesTurnoverCurrentWeek" + i + ","
							+ "[IsDefault]=@IsDefault" + i + ","
							+ "[LSPosHorizon1]=@LSPosHorizon1" + i + ","
							+ "[LSPosHorizon2]=@LSPosHorizon2" + i + ","
							+ "[LSPosHorizon3]=@LSPosHorizon3" + i + ","
							+ "[mId]=@mId" + i + ","
							+ "[ModuleActivated]=@ModuleActivated" + i + ","
							+ "[OrderProcessing]=@OrderProcessing" + i + ","
							+ "[OrderProcessingLog]=@OrderProcessingLog" + i + ","
							+ "[Rahmen]=@Rahmen" + i + ","
							+ "[RahmenAdd]=@RahmenAdd" + i + ","
							+ "[RahmenAddAB]=@RahmenAddAB" + i + ","
							+ "[RahmenAddPositions]=@RahmenAddPositions" + i + ","
							+ "[RahmenCancelation]=@RahmenCancelation" + i + ","
							+ "[RahmenClosure]=@RahmenClosure" + i + ","
							+ "[RahmenDelete]=@RahmenDelete" + i + ","
							+ "[RahmenDeletePositions]=@RahmenDeletePositions" + i + ","
							+ "[RahmenDocumentFlow]=@RahmenDocumentFlow" + i + ","
							+ "[RahmenEditHeader]=@RahmenEditHeader" + i + ","
							+ "[RahmenEditPositions]=@RahmenEditPositions" + i + ","
							+ "[RahmenHistory]=@RahmenHistory" + i + ","
							+ "[RahmenValdation]=@RahmenValdation" + i + ","
							+ "[RAPosHorizon1]=@RAPosHorizon1" + i + ","
							+ "[RAPosHorizon2]=@RAPosHorizon2" + i + ","
							+ "[RAPosHorizon3]=@RAPosHorizon3" + i + ","
							+ "[Rechnung]=@Rechnung" + i + ","
							+ "[RechnungAutoCreation]=@RechnungAutoCreation" + i + ","
							+ "[RechnungBookedEdit]=@RechnungBookedEdit" + i + ","
							+ "[RechnungConfig]=@RechnungConfig" + i + ","
							+ "[RechnungDelete]=@RechnungDelete" + i + ","
							+ "[RechnungDoneEdit]=@RechnungDoneEdit" + i + ","
							+ "[RechnungManualCreation]=@RechnungManualCreation" + i + ","
							+ "[RechnungReport]=@RechnungReport" + i + ","
							+ "[RechnungSend]=@RechnungSend" + i + ","
							+ "[RechnungValidate]=@RechnungValidate" + i + ","
							+ "[RGPosHorizon1]=@RGPosHorizon1" + i + ","
							+ "[RGPosHorizon2]=@RGPosHorizon2" + i + ","
							+ "[RGPosHorizon3]=@RGPosHorizon3" + i + ","
							+ "[Statistics]=@Statistics" + i + ","
							+ "[StatsBacklogFGAdmin]=@StatsBacklogFGAdmin" + i + ","
							+ "[StatsBacklogHWAdmin]=@StatsBacklogHWAdmin" + i + ","
							+ "[StatsCapaCutting]=@StatsCapaCutting" + i + ","
							+ "[StatsCapaHorizons]=@StatsCapaHorizons" + i + ","
							+ "[StatsCapaLong]=@StatsCapaLong" + i + ","
							+ "[StatsCapaPlanning]=@StatsCapaPlanning" + i + ","
							+ "[StatsCapaShort]=@StatsCapaShort" + i + ","
							+ "[StatsRechnungAL]=@StatsRechnungAL" + i + ","
							+ "[StatsRechnungBETN]=@StatsRechnungBETN" + i + ","
							+ "[StatsRechnungCZ]=@StatsRechnungCZ" + i + ","
							+ "[StatsRechnungDE]=@StatsRechnungDE" + i + ","
							+ "[StatsRechnungGZTN]=@StatsRechnungGZTN" + i + ","
							+ "[StatsRechnungTN]=@StatsRechnungTN" + i + ","
							+ "[StatsRechnungWS]=@StatsRechnungWS" + i + ","
							+ "[StatsStockCS]=@StatsStockCS" + i + ","
							+ "[StatsStockExternalWarehouse]=@StatsStockExternalWarehouse" + i + ","
							+ "[StatsStockFG]=@StatsStockFG" + i + ","
							+ "[UBGStatusChange]=@UBGStatusChange" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AB_LT" + i, item.AB_LT == null ? (object)DBNull.Value : item.AB_LT);
						sqlCommand.Parameters.AddWithValue("AB_LT_EDI" + i, item.AB_LT_EDI == null ? (object)DBNull.Value : item.AB_LT_EDI);
						sqlCommand.Parameters.AddWithValue("ABPosHorizon1" + i, item.ABPosHorizon1 == null ? (object)DBNull.Value : item.ABPosHorizon1);
						sqlCommand.Parameters.AddWithValue("ABPosHorizon2" + i, item.ABPosHorizon2 == null ? (object)DBNull.Value : item.ABPosHorizon2);
						sqlCommand.Parameters.AddWithValue("ABPosHorizon3" + i, item.ABPosHorizon3 == null ? (object)DBNull.Value : item.ABPosHorizon3);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
						sqlCommand.Parameters.AddWithValue("BVBookedEdit" + i, item.BVBookedEdit == null ? (object)DBNull.Value : item.BVBookedEdit);
						sqlCommand.Parameters.AddWithValue("BVDoneEdit" + i, item.BVDoneEdit == null ? (object)DBNull.Value : item.BVDoneEdit);
						sqlCommand.Parameters.AddWithValue("BVFaCreate" + i, item.BVFaCreate == null ? (object)DBNull.Value : item.BVFaCreate);
						sqlCommand.Parameters.AddWithValue("Configuration" + i, item.Configuration == null ? (object)DBNull.Value : item.Configuration);
						sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments" + i, item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
						sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees" + i, item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
						sqlCommand.Parameters.AddWithValue("ConfigurationReplacements" + i, item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
						sqlCommand.Parameters.AddWithValue("ConfigurationReporting" + i, item.ConfigurationReporting == null ? (object)DBNull.Value : item.ConfigurationReporting);
						sqlCommand.Parameters.AddWithValue("ConfirmationBookedEdit" + i, item.ConfirmationBookedEdit == null ? (object)DBNull.Value : item.ConfirmationBookedEdit);
						sqlCommand.Parameters.AddWithValue("ConfirmationCreate" + i, item.ConfirmationCreate == null ? (object)DBNull.Value : item.ConfirmationCreate);
						sqlCommand.Parameters.AddWithValue("ConfirmationDelete" + i, item.ConfirmationDelete == null ? (object)DBNull.Value : item.ConfirmationDelete);
						sqlCommand.Parameters.AddWithValue("ConfirmationDeliveryNote" + i, item.ConfirmationDeliveryNote == null ? (object)DBNull.Value : item.ConfirmationDeliveryNote);
						sqlCommand.Parameters.AddWithValue("ConfirmationDoneEdit" + i, item.ConfirmationDoneEdit == null ? (object)DBNull.Value : item.ConfirmationDoneEdit);
						sqlCommand.Parameters.AddWithValue("ConfirmationEdit" + i, item.ConfirmationEdit == null ? (object)DBNull.Value : item.ConfirmationEdit);
						sqlCommand.Parameters.AddWithValue("ConfirmationPositionEdit" + i, item.ConfirmationPositionEdit == null ? (object)DBNull.Value : item.ConfirmationPositionEdit);
						sqlCommand.Parameters.AddWithValue("ConfirmationPositionProduction" + i, item.ConfirmationPositionProduction == null ? (object)DBNull.Value : item.ConfirmationPositionProduction);
						sqlCommand.Parameters.AddWithValue("ConfirmationReport" + i, item.ConfirmationReport == null ? (object)DBNull.Value : item.ConfirmationReport);
						sqlCommand.Parameters.AddWithValue("ConfirmationValidate" + i, item.ConfirmationValidate == null ? (object)DBNull.Value : item.ConfirmationValidate);
						sqlCommand.Parameters.AddWithValue("ConfirmationView" + i, item.ConfirmationView == null ? (object)DBNull.Value : item.ConfirmationView);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CSInfoEdit" + i, item.CSInfoEdit == null ? (object)DBNull.Value : item.CSInfoEdit);
						sqlCommand.Parameters.AddWithValue("DelforCreate" + i, item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
						sqlCommand.Parameters.AddWithValue("DelforDelete" + i, item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
						sqlCommand.Parameters.AddWithValue("DelforDeletePosition" + i, item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
						sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation" + i, item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
						sqlCommand.Parameters.AddWithValue("DelforReport" + i, item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
						sqlCommand.Parameters.AddWithValue("DelforStatistics" + i, item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
						sqlCommand.Parameters.AddWithValue("DelforView" + i, item.DelforView == null ? (object)DBNull.Value : item.DelforView);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteBookedEdit" + i, item.DeliveryNoteBookedEdit == null ? (object)DBNull.Value : item.DeliveryNoteBookedEdit);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteCreate" + i, item.DeliveryNoteCreate == null ? (object)DBNull.Value : item.DeliveryNoteCreate);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteDelete" + i, item.DeliveryNoteDelete == null ? (object)DBNull.Value : item.DeliveryNoteDelete);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteDoneEdit" + i, item.DeliveryNoteDoneEdit == null ? (object)DBNull.Value : item.DeliveryNoteDoneEdit);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteEdit" + i, item.DeliveryNoteEdit == null ? (object)DBNull.Value : item.DeliveryNoteEdit);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteLog" + i, item.DeliveryNoteLog == null ? (object)DBNull.Value : item.DeliveryNoteLog);
						sqlCommand.Parameters.AddWithValue("DeliveryNotePositionEdit" + i, item.DeliveryNotePositionEdit == null ? (object)DBNull.Value : item.DeliveryNotePositionEdit);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteReport" + i, item.DeliveryNoteReport == null ? (object)DBNull.Value : item.DeliveryNoteReport);
						sqlCommand.Parameters.AddWithValue("DeliveryNoteView" + i, item.DeliveryNoteView == null ? (object)DBNull.Value : item.DeliveryNoteView);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon1" + i, item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon2" + i, item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon3" + i, item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
						sqlCommand.Parameters.AddWithValue("EDI" + i, item.EDI == null ? (object)DBNull.Value : item.EDI);
						sqlCommand.Parameters.AddWithValue("EDIDownloadFile" + i, item.EDIDownloadFile == null ? (object)DBNull.Value : item.EDIDownloadFile);
						sqlCommand.Parameters.AddWithValue("EDIError" + i, item.EDIError == null ? (object)DBNull.Value : item.EDIError);
						sqlCommand.Parameters.AddWithValue("EDIErrorEdit" + i, item.EDIErrorEdit == null ? (object)DBNull.Value : item.EDIErrorEdit);
						sqlCommand.Parameters.AddWithValue("EDIErrorValidated" + i, item.EDIErrorValidated == null ? (object)DBNull.Value : item.EDIErrorValidated);
						sqlCommand.Parameters.AddWithValue("EDILogOrderValidated" + i, item.EDILogOrderValidated == null ? (object)DBNull.Value : item.EDILogOrderValidated);
						sqlCommand.Parameters.AddWithValue("EDIOrder" + i, item.EDIOrder == null ? (object)DBNull.Value : item.EDIOrder);
						sqlCommand.Parameters.AddWithValue("EDIOrderEdit" + i, item.EDIOrderEdit == null ? (object)DBNull.Value : item.EDIOrderEdit);
						sqlCommand.Parameters.AddWithValue("EDIOrderPositionEdit" + i, item.EDIOrderPositionEdit == null ? (object)DBNull.Value : item.EDIOrderPositionEdit);
						sqlCommand.Parameters.AddWithValue("EDIOrderProduction" + i, item.EDIOrderProduction == null ? (object)DBNull.Value : item.EDIOrderProduction);
						sqlCommand.Parameters.AddWithValue("EDIOrderProductionPosition" + i, item.EDIOrderProductionPosition == null ? (object)DBNull.Value : item.EDIOrderProductionPosition);
						sqlCommand.Parameters.AddWithValue("EDIOrderReport" + i, item.EDIOrderReport == null ? (object)DBNull.Value : item.EDIOrderReport);
						sqlCommand.Parameters.AddWithValue("EDIOrderValidated" + i, item.EDIOrderValidated == null ? (object)DBNull.Value : item.EDIOrderValidated);
						sqlCommand.Parameters.AddWithValue("EDIOrderValidatedEdit" + i, item.EDIOrderValidatedEdit == null ? (object)DBNull.Value : item.EDIOrderValidatedEdit);
						sqlCommand.Parameters.AddWithValue("FaABEdit" + i, item.FaABEdit == null ? (object)DBNull.Value : item.FaABEdit);
						sqlCommand.Parameters.AddWithValue("FaABView" + i, item.FaABView == null ? (object)DBNull.Value : item.FaABView);
						sqlCommand.Parameters.AddWithValue("FaActionBook" + i, item.FaActionBook == null ? (object)DBNull.Value : item.FaActionBook);
						sqlCommand.Parameters.AddWithValue("FaActionComplete" + i, item.FaActionComplete == null ? (object)DBNull.Value : item.FaActionComplete);
						sqlCommand.Parameters.AddWithValue("FaActionDelete" + i, item.FaActionDelete == null ? (object)DBNull.Value : item.FaActionDelete);
						sqlCommand.Parameters.AddWithValue("FaActionPrint" + i, item.FaActionPrint == null ? (object)DBNull.Value : item.FaActionPrint);
						sqlCommand.Parameters.AddWithValue("FaAdmin" + i, item.FaAdmin == null ? (object)DBNull.Value : item.FaAdmin);
						sqlCommand.Parameters.AddWithValue("FAAKtualTerminUpdate" + i, item.FAAKtualTerminUpdate == null ? (object)DBNull.Value : item.FAAKtualTerminUpdate);
						sqlCommand.Parameters.AddWithValue("FaAnalysis" + i, item.FaAnalysis == null ? (object)DBNull.Value : item.FaAnalysis);
						sqlCommand.Parameters.AddWithValue("FAAuswertungEndkontrolle" + i, item.FAAuswertungEndkontrolle == null ? (object)DBNull.Value : item.FAAuswertungEndkontrolle);
						sqlCommand.Parameters.AddWithValue("FABemerkungPlannug" + i, item.FABemerkungPlannug == null ? (object)DBNull.Value : item.FABemerkungPlannug);
						sqlCommand.Parameters.AddWithValue("FABemerkungZuGewerk" + i, item.FABemerkungZuGewerk == null ? (object)DBNull.Value : item.FABemerkungZuGewerk);
						sqlCommand.Parameters.AddWithValue("FABemerkungZuPrio" + i, item.FABemerkungZuPrio == null ? (object)DBNull.Value : item.FABemerkungZuPrio);
						sqlCommand.Parameters.AddWithValue("FACancelHorizon1" + i, item.FACancelHorizon1 == null ? (object)DBNull.Value : item.FACancelHorizon1);
						sqlCommand.Parameters.AddWithValue("FACancelHorizon2" + i, item.FACancelHorizon2 == null ? (object)DBNull.Value : item.FACancelHorizon2);
						sqlCommand.Parameters.AddWithValue("FACancelHorizon3" + i, item.FACancelHorizon3 == null ? (object)DBNull.Value : item.FACancelHorizon3);
						sqlCommand.Parameters.AddWithValue("FACommissionert" + i, item.FACommissionert == null ? (object)DBNull.Value : item.FACommissionert);
						sqlCommand.Parameters.AddWithValue("FaCreate" + i, item.FaCreate == null ? (object)DBNull.Value : item.FaCreate);
						sqlCommand.Parameters.AddWithValue("FACreateHorizon1" + i, item.FACreateHorizon1 == null ? (object)DBNull.Value : item.FACreateHorizon1);
						sqlCommand.Parameters.AddWithValue("FACreateHorizon2" + i, item.FACreateHorizon2 == null ? (object)DBNull.Value : item.FACreateHorizon2);
						sqlCommand.Parameters.AddWithValue("FACreateHorizon3" + i, item.FACreateHorizon3 == null ? (object)DBNull.Value : item.FACreateHorizon3);
						sqlCommand.Parameters.AddWithValue("FaDatenEdit" + i, item.FaDatenEdit == null ? (object)DBNull.Value : item.FaDatenEdit);
						sqlCommand.Parameters.AddWithValue("FaDatenView" + i, item.FaDatenView == null ? (object)DBNull.Value : item.FaDatenView);
						sqlCommand.Parameters.AddWithValue("FaDelete" + i, item.FaDelete == null ? (object)DBNull.Value : item.FaDelete);
						sqlCommand.Parameters.AddWithValue("FADrucken" + i, item.FADrucken == null ? (object)DBNull.Value : item.FADrucken);
						sqlCommand.Parameters.AddWithValue("FaEdit" + i, item.FaEdit == null ? (object)DBNull.Value : item.FaEdit);
						sqlCommand.Parameters.AddWithValue("FAErlidegen" + i, item.FAErlidegen == null ? (object)DBNull.Value : item.FAErlidegen);
						sqlCommand.Parameters.AddWithValue("FAExcelUpdateWerk" + i, item.FAExcelUpdateWerk == null ? (object)DBNull.Value : item.FAExcelUpdateWerk);
						sqlCommand.Parameters.AddWithValue("FAExcelUpdateWunsh" + i, item.FAExcelUpdateWunsh == null ? (object)DBNull.Value : item.FAExcelUpdateWunsh);
						sqlCommand.Parameters.AddWithValue("FAFehlrMaterial" + i, item.FAFehlrMaterial == null ? (object)DBNull.Value : item.FAFehlrMaterial);
						sqlCommand.Parameters.AddWithValue("FaHomeAnalysis" + i, item.FaHomeAnalysis == null ? (object)DBNull.Value : item.FaHomeAnalysis);
						sqlCommand.Parameters.AddWithValue("FaHomeUpdate" + i, item.FaHomeUpdate == null ? (object)DBNull.Value : item.FaHomeUpdate);
						sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei" + i, item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
						sqlCommand.Parameters.AddWithValue("FaPlanningEdit" + i, item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
						sqlCommand.Parameters.AddWithValue("FaPlanningView" + i, item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
						sqlCommand.Parameters.AddWithValue("FAPlannung" + i, item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
						sqlCommand.Parameters.AddWithValue("FAPlannungTechnick" + i, item.FAPlannungTechnick == null ? (object)DBNull.Value : item.FAPlannungTechnick);
						sqlCommand.Parameters.AddWithValue("FAPriesZeitUpdate" + i, item.FAPriesZeitUpdate == null ? (object)DBNull.Value : item.FAPriesZeitUpdate);
						sqlCommand.Parameters.AddWithValue("FAProductionPlannung" + i, item.FAProductionPlannung == null ? (object)DBNull.Value : item.FAProductionPlannung);
						sqlCommand.Parameters.AddWithValue("FAStappleDruck" + i, item.FAStappleDruck == null ? (object)DBNull.Value : item.FAStappleDruck);
						sqlCommand.Parameters.AddWithValue("FAStatusAlbania" + i, item.FAStatusAlbania == null ? (object)DBNull.Value : item.FAStatusAlbania);
						sqlCommand.Parameters.AddWithValue("FAStatusCzech" + i, item.FAStatusCzech == null ? (object)DBNull.Value : item.FAStatusCzech);
						sqlCommand.Parameters.AddWithValue("FAStatusTunisia" + i, item.FAStatusTunisia == null ? (object)DBNull.Value : item.FAStatusTunisia);
						sqlCommand.Parameters.AddWithValue("FAStorno" + i, item.FAStorno == null ? (object)DBNull.Value : item.FAStorno);
						sqlCommand.Parameters.AddWithValue("FAStucklist" + i, item.FAStucklist == null ? (object)DBNull.Value : item.FAStucklist);
						sqlCommand.Parameters.AddWithValue("FaTechnicEdit" + i, item.FaTechnicEdit == null ? (object)DBNull.Value : item.FaTechnicEdit);
						sqlCommand.Parameters.AddWithValue("FaTechnicView" + i, item.FaTechnicView == null ? (object)DBNull.Value : item.FaTechnicView);
						sqlCommand.Parameters.AddWithValue("FATerminWerk" + i, item.FATerminWerk == null ? (object)DBNull.Value : item.FATerminWerk);
						sqlCommand.Parameters.AddWithValue("FAUpdateBemerkungExtern" + i, item.FAUpdateBemerkungExtern == null ? (object)DBNull.Value : item.FAUpdateBemerkungExtern);
						sqlCommand.Parameters.AddWithValue("FAUpdateByArticle" + i, item.FAUpdateByArticle == null ? (object)DBNull.Value : item.FAUpdateByArticle);
						sqlCommand.Parameters.AddWithValue("FAUpdateByFA" + i, item.FAUpdateByFA == null ? (object)DBNull.Value : item.FAUpdateByFA);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1" + i, item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2" + i, item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3" + i, item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
						sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin" + i, item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
						sqlCommand.Parameters.AddWithValue("Fertigung" + i, item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
						sqlCommand.Parameters.AddWithValue("FertigungLog" + i, item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
						sqlCommand.Parameters.AddWithValue("ForcastCreate" + i, item.ForcastCreate == null ? (object)DBNull.Value : item.ForcastCreate);
						sqlCommand.Parameters.AddWithValue("ForcastDelete" + i, item.ForcastDelete == null ? (object)DBNull.Value : item.ForcastDelete);
						sqlCommand.Parameters.AddWithValue("ForcastEdit" + i, item.ForcastEdit == null ? (object)DBNull.Value : item.ForcastEdit);
						sqlCommand.Parameters.AddWithValue("ForcastLog" + i, item.ForcastLog == null ? (object)DBNull.Value : item.ForcastLog);
						sqlCommand.Parameters.AddWithValue("ForcastPositionEdit" + i, item.ForcastPositionEdit == null ? (object)DBNull.Value : item.ForcastPositionEdit);
						sqlCommand.Parameters.AddWithValue("ForcastReport" + i, item.ForcastReport == null ? (object)DBNull.Value : item.ForcastReport);
						sqlCommand.Parameters.AddWithValue("ForcastView" + i, item.ForcastView == null ? (object)DBNull.Value : item.ForcastView);
						sqlCommand.Parameters.AddWithValue("FRCPosHorizon1" + i, item.FRCPosHorizon1 == null ? (object)DBNull.Value : item.FRCPosHorizon1);
						sqlCommand.Parameters.AddWithValue("FRCPosHorizon2" + i, item.FRCPosHorizon2 == null ? (object)DBNull.Value : item.FRCPosHorizon2);
						sqlCommand.Parameters.AddWithValue("FRCPosHorizon3" + i, item.FRCPosHorizon3 == null ? (object)DBNull.Value : item.FRCPosHorizon3);
						sqlCommand.Parameters.AddWithValue("GSPosHorizon1" + i, item.GSPosHorizon1 == null ? (object)DBNull.Value : item.GSPosHorizon1);
						sqlCommand.Parameters.AddWithValue("GSPosHorizon2" + i, item.GSPosHorizon2 == null ? (object)DBNull.Value : item.GSPosHorizon2);
						sqlCommand.Parameters.AddWithValue("GSPosHorizon3" + i, item.GSPosHorizon3 == null ? (object)DBNull.Value : item.GSPosHorizon3);
						sqlCommand.Parameters.AddWithValue("GutschriftBookedEdit" + i, item.GutschriftBookedEdit == null ? (object)DBNull.Value : item.GutschriftBookedEdit);
						sqlCommand.Parameters.AddWithValue("GutschriftCreate" + i, item.GutschriftCreate == null ? (object)DBNull.Value : item.GutschriftCreate);
						sqlCommand.Parameters.AddWithValue("GutschriftDelete" + i, item.GutschriftDelete == null ? (object)DBNull.Value : item.GutschriftDelete);
						sqlCommand.Parameters.AddWithValue("GutschriftDoneEdit" + i, item.GutschriftDoneEdit == null ? (object)DBNull.Value : item.GutschriftDoneEdit);
						sqlCommand.Parameters.AddWithValue("GutschriftEdit" + i, item.GutschriftEdit == null ? (object)DBNull.Value : item.GutschriftEdit);
						sqlCommand.Parameters.AddWithValue("GutschriftLog" + i, item.GutschriftLog == null ? (object)DBNull.Value : item.GutschriftLog);
						sqlCommand.Parameters.AddWithValue("GutschriftPositionEdit" + i, item.GutschriftPositionEdit == null ? (object)DBNull.Value : item.GutschriftPositionEdit);
						sqlCommand.Parameters.AddWithValue("GutschriftReport" + i, item.GutschriftReport == null ? (object)DBNull.Value : item.GutschriftReport);
						sqlCommand.Parameters.AddWithValue("GutschriftView" + i, item.GutschriftView == null ? (object)DBNull.Value : item.GutschriftView);
						sqlCommand.Parameters.AddWithValue("InsideSalesChecks" + i, item.InsideSalesChecks == null ? (object)DBNull.Value : item.InsideSalesChecks);
						sqlCommand.Parameters.AddWithValue("InsideSalesChecksArchive" + i, item.InsideSalesChecksArchive == null ? (object)DBNull.Value : item.InsideSalesChecksArchive);
						sqlCommand.Parameters.AddWithValue("InsideSalesCustomerSummary" + i, item.InsideSalesCustomerSummary == null ? (object)DBNull.Value : item.InsideSalesCustomerSummary);
						sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluation" + i, item.InsideSalesMinimumStockEvaluation == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluation);
						sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluationTable" + i, item.InsideSalesMinimumStockEvaluationTable == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluationTable);
						sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrders" + i, item.InsideSalesOverdueOrders == null ? (object)DBNull.Value : item.InsideSalesOverdueOrders);
						sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrdersTable" + i, item.InsideSalesOverdueOrdersTable == null ? (object)DBNull.Value : item.InsideSalesOverdueOrdersTable);
						sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrders" + i, item.InsideSalesTotalUnbookedOrders == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrders);
						sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrdersTable" + i, item.InsideSalesTotalUnbookedOrdersTable == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrdersTable);
						sqlCommand.Parameters.AddWithValue("InsideSalesTurnoverCurrentWeek" + i, item.InsideSalesTurnoverCurrentWeek == null ? (object)DBNull.Value : item.InsideSalesTurnoverCurrentWeek);
						sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
						sqlCommand.Parameters.AddWithValue("LSPosHorizon1" + i, item.LSPosHorizon1 == null ? (object)DBNull.Value : item.LSPosHorizon1);
						sqlCommand.Parameters.AddWithValue("LSPosHorizon2" + i, item.LSPosHorizon2 == null ? (object)DBNull.Value : item.LSPosHorizon2);
						sqlCommand.Parameters.AddWithValue("LSPosHorizon3" + i, item.LSPosHorizon3 == null ? (object)DBNull.Value : item.LSPosHorizon3);
						sqlCommand.Parameters.AddWithValue("mId" + i, item.mId == null ? (object)DBNull.Value : item.mId);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("OrderProcessing" + i, item.OrderProcessing == null ? (object)DBNull.Value : item.OrderProcessing);
						sqlCommand.Parameters.AddWithValue("OrderProcessingLog" + i, item.OrderProcessingLog == null ? (object)DBNull.Value : item.OrderProcessingLog);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
						sqlCommand.Parameters.AddWithValue("RahmenAdd" + i, item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
						sqlCommand.Parameters.AddWithValue("RahmenAddAB" + i, item.RahmenAddAB == null ? (object)DBNull.Value : item.RahmenAddAB);
						sqlCommand.Parameters.AddWithValue("RahmenAddPositions" + i, item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
						sqlCommand.Parameters.AddWithValue("RahmenCancelation" + i, item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
						sqlCommand.Parameters.AddWithValue("RahmenClosure" + i, item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
						sqlCommand.Parameters.AddWithValue("RahmenDelete" + i, item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
						sqlCommand.Parameters.AddWithValue("RahmenDeletePositions" + i, item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
						sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow" + i, item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
						sqlCommand.Parameters.AddWithValue("RahmenEditHeader" + i, item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
						sqlCommand.Parameters.AddWithValue("RahmenEditPositions" + i, item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
						sqlCommand.Parameters.AddWithValue("RahmenHistory" + i, item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
						sqlCommand.Parameters.AddWithValue("RahmenValdation" + i, item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
						sqlCommand.Parameters.AddWithValue("RAPosHorizon1" + i, item.RAPosHorizon1 == null ? (object)DBNull.Value : item.RAPosHorizon1);
						sqlCommand.Parameters.AddWithValue("RAPosHorizon2" + i, item.RAPosHorizon2 == null ? (object)DBNull.Value : item.RAPosHorizon2);
						sqlCommand.Parameters.AddWithValue("RAPosHorizon3" + i, item.RAPosHorizon3 == null ? (object)DBNull.Value : item.RAPosHorizon3);
						sqlCommand.Parameters.AddWithValue("Rechnung" + i, item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
						sqlCommand.Parameters.AddWithValue("RechnungAutoCreation" + i, item.RechnungAutoCreation == null ? (object)DBNull.Value : item.RechnungAutoCreation);
						sqlCommand.Parameters.AddWithValue("RechnungBookedEdit" + i, item.RechnungBookedEdit == null ? (object)DBNull.Value : item.RechnungBookedEdit);
						sqlCommand.Parameters.AddWithValue("RechnungConfig" + i, item.RechnungConfig == null ? (object)DBNull.Value : item.RechnungConfig);
						sqlCommand.Parameters.AddWithValue("RechnungDelete" + i, item.RechnungDelete == null ? (object)DBNull.Value : item.RechnungDelete);
						sqlCommand.Parameters.AddWithValue("RechnungDoneEdit" + i, item.RechnungDoneEdit == null ? (object)DBNull.Value : item.RechnungDoneEdit);
						sqlCommand.Parameters.AddWithValue("RechnungManualCreation" + i, item.RechnungManualCreation == null ? (object)DBNull.Value : item.RechnungManualCreation);
						sqlCommand.Parameters.AddWithValue("RechnungReport" + i, item.RechnungReport == null ? (object)DBNull.Value : item.RechnungReport);
						sqlCommand.Parameters.AddWithValue("RechnungSend" + i, item.RechnungSend == null ? (object)DBNull.Value : item.RechnungSend);
						sqlCommand.Parameters.AddWithValue("RechnungValidate" + i, item.RechnungValidate == null ? (object)DBNull.Value : item.RechnungValidate);
						sqlCommand.Parameters.AddWithValue("RGPosHorizon1" + i, item.RGPosHorizon1 == null ? (object)DBNull.Value : item.RGPosHorizon1);
						sqlCommand.Parameters.AddWithValue("RGPosHorizon2" + i, item.RGPosHorizon2 == null ? (object)DBNull.Value : item.RGPosHorizon2);
						sqlCommand.Parameters.AddWithValue("RGPosHorizon3" + i, item.RGPosHorizon3 == null ? (object)DBNull.Value : item.RGPosHorizon3);
						sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
						sqlCommand.Parameters.AddWithValue("StatsBacklogFGAdmin" + i, item.StatsBacklogFGAdmin == null ? (object)DBNull.Value : item.StatsBacklogFGAdmin);
						sqlCommand.Parameters.AddWithValue("StatsBacklogHWAdmin" + i, item.StatsBacklogHWAdmin == null ? (object)DBNull.Value : item.StatsBacklogHWAdmin);
						sqlCommand.Parameters.AddWithValue("StatsCapaCutting" + i, item.StatsCapaCutting == null ? (object)DBNull.Value : item.StatsCapaCutting);
						sqlCommand.Parameters.AddWithValue("StatsCapaHorizons" + i, item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
						sqlCommand.Parameters.AddWithValue("StatsCapaLong" + i, item.StatsCapaLong == null ? (object)DBNull.Value : item.StatsCapaLong);
						sqlCommand.Parameters.AddWithValue("StatsCapaPlanning" + i, item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
						sqlCommand.Parameters.AddWithValue("StatsCapaShort" + i, item.StatsCapaShort == null ? (object)DBNull.Value : item.StatsCapaShort);
						sqlCommand.Parameters.AddWithValue("StatsRechnungAL" + i, item.StatsRechnungAL == null ? (object)DBNull.Value : item.StatsRechnungAL);
						sqlCommand.Parameters.AddWithValue("StatsRechnungBETN" + i, item.StatsRechnungBETN == null ? (object)DBNull.Value : item.StatsRechnungBETN);
						sqlCommand.Parameters.AddWithValue("StatsRechnungCZ" + i, item.StatsRechnungCZ == null ? (object)DBNull.Value : item.StatsRechnungCZ);
						sqlCommand.Parameters.AddWithValue("StatsRechnungDE" + i, item.StatsRechnungDE == null ? (object)DBNull.Value : item.StatsRechnungDE);
						sqlCommand.Parameters.AddWithValue("StatsRechnungGZTN" + i, item.StatsRechnungGZTN == null ? (object)DBNull.Value : item.StatsRechnungGZTN);
						sqlCommand.Parameters.AddWithValue("StatsRechnungTN" + i, item.StatsRechnungTN == null ? (object)DBNull.Value : item.StatsRechnungTN);
						sqlCommand.Parameters.AddWithValue("StatsRechnungWS" + i, item.StatsRechnungWS == null ? (object)DBNull.Value : item.StatsRechnungWS);
						sqlCommand.Parameters.AddWithValue("StatsStockCS" + i, item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
						sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse" + i, item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
						sqlCommand.Parameters.AddWithValue("StatsStockFG" + i, item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
						sqlCommand.Parameters.AddWithValue("UBGStatusChange" + i, item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__CTS_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [__CTS_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_AccessProfile]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [__CTS_AccessProfile] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__CTS_AccessProfile] ([AB_LT],[AB_LT_EDI],[ABPosHorizon1],[ABPosHorizon2],[ABPosHorizon3],[AccessProfileName],[Administration],[BVBookedEdit],[BVDoneEdit],[BVFaCreate],[Configuration],[ConfigurationAppoitments],[ConfigurationChangeEmployees],[ConfigurationReplacements],[ConfigurationReporting],[ConfirmationBookedEdit],[ConfirmationCreate],[ConfirmationDelete],[ConfirmationDeliveryNote],[ConfirmationDoneEdit],[ConfirmationEdit],[ConfirmationPositionEdit],[ConfirmationPositionProduction],[ConfirmationReport],[ConfirmationValidate],[ConfirmationView],[CreationTime],[CreationUserId],[CSInfoEdit],[DelforCreate],[DelforDelete],[DelforDeletePosition],[DelforOrderConfirmation],[DelforReport],[DelforStatistics],[DelforView],[DeliveryNoteBookedEdit],[DeliveryNoteCreate],[DeliveryNoteDelete],[DeliveryNoteDoneEdit],[DeliveryNoteEdit],[DeliveryNoteLog],[DeliveryNotePositionEdit],[DeliveryNoteReport],[DeliveryNoteView],[DLFPosHorizon1],[DLFPosHorizon2],[DLFPosHorizon3],[EDI],[EDIDownloadFile],[EDIError],[EDIErrorEdit],[EDIErrorValidated],[EDILogOrderValidated],[EDIOrder],[EDIOrderEdit],[EDIOrderPositionEdit],[EDIOrderProduction],[EDIOrderProductionPosition],[EDIOrderReport],[EDIOrderValidated],[EDIOrderValidatedEdit],[FaABEdit],[FaABView],[FaActionBook],[FaActionComplete],[FaActionDelete],[FaActionPrint],[FaAdmin],[FAAKtualTerminUpdate],[FaAnalysis],[FAAuswertungEndkontrolle],[FABemerkungPlannug],[FABemerkungZuGewerk],[FABemerkungZuPrio],[FACancelHorizon1],[FACancelHorizon2],[FACancelHorizon3],[FACommissionert],[FaCreate],[FACreateHorizon1],[FACreateHorizon2],[FACreateHorizon3],[FaDatenEdit],[FaDatenView],[FaDelete],[FADrucken],[FaEdit],[FAErlidegen],[FAExcelUpdateWerk],[FAExcelUpdateWunsh],[FAFehlrMaterial],[FaHomeAnalysis],[FaHomeUpdate],[FALaufkarteSchneiderei],[FaPlanningEdit],[FaPlanningView],[FAPlannung],[FAPlannungTechnick],[FAPriesZeitUpdate],[FAProductionPlannung],[FAStappleDruck],[FAStatusAlbania],[FAStatusCzech],[FAStatusTunisia],[FAStorno],[FAStucklist],[FaTechnicEdit],[FaTechnicView],[FATerminWerk],[FAUpdateBemerkungExtern],[FAUpdateByArticle],[FAUpdateByFA],[FAUpdateTerminHorizon1],[FAUpdateTerminHorizon2],[FAUpdateTerminHorizon3],[FAWerkWunshAdmin],[Fertigung],[FertigungLog],[ForcastCreate],[ForcastDelete],[ForcastEdit],[ForcastLog],[ForcastPositionEdit],[ForcastReport],[ForcastView],[FRCPosHorizon1],[FRCPosHorizon2],[FRCPosHorizon3],[GSPosHorizon1],[GSPosHorizon2],[GSPosHorizon3],[GutschriftBookedEdit],[GutschriftCreate],[GutschriftDelete],[GutschriftDoneEdit],[GutschriftEdit],[GutschriftLog],[GutschriftPositionEdit],[GutschriftReport],[GutschriftView],[InsideSalesChecks],[InsideSalesChecksArchive],[InsideSalesCustomerSummary],[InsideSalesMinimumStockEvaluation],[InsideSalesMinimumStockEvaluationTable],[InsideSalesOverdueOrders],[InsideSalesOverdueOrdersTable],[InsideSalesTotalUnbookedOrders],[InsideSalesTotalUnbookedOrdersTable],[InsideSalesTurnoverCurrentWeek],[IsDefault],[LSPosHorizon1],[LSPosHorizon2],[LSPosHorizon3],[mId],[ModuleActivated],[OrderProcessing],[OrderProcessingLog],[Rahmen],[RahmenAdd],[RahmenAddAB],[RahmenAddPositions],[RahmenCancelation],[RahmenClosure],[RahmenDelete],[RahmenDeletePositions],[RahmenDocumentFlow],[RahmenEditHeader],[RahmenEditPositions],[RahmenHistory],[RahmenValdation],[RAPosHorizon1],[RAPosHorizon2],[RAPosHorizon3],[Rechnung],[RechnungAutoCreation],[RechnungBookedEdit],[RechnungConfig],[RechnungDelete],[RechnungDoneEdit],[RechnungManualCreation],[RechnungReport],[RechnungSend],[RechnungValidate],[RGPosHorizon1],[RGPosHorizon2],[RGPosHorizon3],[Statistics],[StatsBacklogFGAdmin],[StatsBacklogHWAdmin],[StatsCapaCutting],[StatsCapaHorizons],[StatsCapaLong],[StatsCapaPlanning],[StatsCapaShort],[StatsRechnungAL],[StatsRechnungBETN],[StatsRechnungCZ],[StatsRechnungDE],[StatsRechnungGZTN],[StatsRechnungTN],[StatsRechnungWS],[StatsStockCS],[StatsStockExternalWarehouse],[StatsStockFG],[UBGStatusChange]) OUTPUT INSERTED.[Id] VALUES (@AB_LT,@AB_LT_EDI,@ABPosHorizon1,@ABPosHorizon2,@ABPosHorizon3,@AccessProfileName,@Administration,@BVBookedEdit,@BVDoneEdit,@BVFaCreate,@Configuration,@ConfigurationAppoitments,@ConfigurationChangeEmployees,@ConfigurationReplacements,@ConfigurationReporting,@ConfirmationBookedEdit,@ConfirmationCreate,@ConfirmationDelete,@ConfirmationDeliveryNote,@ConfirmationDoneEdit,@ConfirmationEdit,@ConfirmationPositionEdit,@ConfirmationPositionProduction,@ConfirmationReport,@ConfirmationValidate,@ConfirmationView,@CreationTime,@CreationUserId,@CSInfoEdit,@DelforCreate,@DelforDelete,@DelforDeletePosition,@DelforOrderConfirmation,@DelforReport,@DelforStatistics,@DelforView,@DeliveryNoteBookedEdit,@DeliveryNoteCreate,@DeliveryNoteDelete,@DeliveryNoteDoneEdit,@DeliveryNoteEdit,@DeliveryNoteLog,@DeliveryNotePositionEdit,@DeliveryNoteReport,@DeliveryNoteView,@DLFPosHorizon1,@DLFPosHorizon2,@DLFPosHorizon3,@EDI,@EDIDownloadFile,@EDIError,@EDIErrorEdit,@EDIErrorValidated,@EDILogOrderValidated,@EDIOrder,@EDIOrderEdit,@EDIOrderPositionEdit,@EDIOrderProduction,@EDIOrderProductionPosition,@EDIOrderReport,@EDIOrderValidated,@EDIOrderValidatedEdit,@FaABEdit,@FaABView,@FaActionBook,@FaActionComplete,@FaActionDelete,@FaActionPrint,@FaAdmin,@FAAKtualTerminUpdate,@FaAnalysis,@FAAuswertungEndkontrolle,@FABemerkungPlannug,@FABemerkungZuGewerk,@FABemerkungZuPrio,@FACancelHorizon1,@FACancelHorizon2,@FACancelHorizon3,@FACommissionert,@FaCreate,@FACreateHorizon1,@FACreateHorizon2,@FACreateHorizon3,@FaDatenEdit,@FaDatenView,@FaDelete,@FADrucken,@FaEdit,@FAErlidegen,@FAExcelUpdateWerk,@FAExcelUpdateWunsh,@FAFehlrMaterial,@FaHomeAnalysis,@FaHomeUpdate,@FALaufkarteSchneiderei,@FaPlanningEdit,@FaPlanningView,@FAPlannung,@FAPlannungTechnick,@FAPriesZeitUpdate,@FAProductionPlannung,@FAStappleDruck,@FAStatusAlbania,@FAStatusCzech,@FAStatusTunisia,@FAStorno,@FAStucklist,@FaTechnicEdit,@FaTechnicView,@FATerminWerk,@FAUpdateBemerkungExtern,@FAUpdateByArticle,@FAUpdateByFA,@FAUpdateTerminHorizon1,@FAUpdateTerminHorizon2,@FAUpdateTerminHorizon3,@FAWerkWunshAdmin,@Fertigung,@FertigungLog,@ForcastCreate,@ForcastDelete,@ForcastEdit,@ForcastLog,@ForcastPositionEdit,@ForcastReport,@ForcastView,@FRCPosHorizon1,@FRCPosHorizon2,@FRCPosHorizon3,@GSPosHorizon1,@GSPosHorizon2,@GSPosHorizon3,@GutschriftBookedEdit,@GutschriftCreate,@GutschriftDelete,@GutschriftDoneEdit,@GutschriftEdit,@GutschriftLog,@GutschriftPositionEdit,@GutschriftReport,@GutschriftView,@InsideSalesChecks,@InsideSalesChecksArchive,@InsideSalesCustomerSummary,@InsideSalesMinimumStockEvaluation,@InsideSalesMinimumStockEvaluationTable,@InsideSalesOverdueOrders,@InsideSalesOverdueOrdersTable,@InsideSalesTotalUnbookedOrders,@InsideSalesTotalUnbookedOrdersTable,@InsideSalesTurnoverCurrentWeek,@IsDefault,@LSPosHorizon1,@LSPosHorizon2,@LSPosHorizon3,@mId,@ModuleActivated,@OrderProcessing,@OrderProcessingLog,@Rahmen,@RahmenAdd,@RahmenAddAB,@RahmenAddPositions,@RahmenCancelation,@RahmenClosure,@RahmenDelete,@RahmenDeletePositions,@RahmenDocumentFlow,@RahmenEditHeader,@RahmenEditPositions,@RahmenHistory,@RahmenValdation,@RAPosHorizon1,@RAPosHorizon2,@RAPosHorizon3,@Rechnung,@RechnungAutoCreation,@RechnungBookedEdit,@RechnungConfig,@RechnungDelete,@RechnungDoneEdit,@RechnungManualCreation,@RechnungReport,@RechnungSend,@RechnungValidate,@RGPosHorizon1,@RGPosHorizon2,@RGPosHorizon3,@Statistics,@StatsBacklogFGAdmin,@StatsBacklogHWAdmin,@StatsCapaCutting,@StatsCapaHorizons,@StatsCapaLong,@StatsCapaPlanning,@StatsCapaShort,@StatsRechnungAL,@StatsRechnungBETN,@StatsRechnungCZ,@StatsRechnungDE,@StatsRechnungGZTN,@StatsRechnungTN,@StatsRechnungWS,@StatsStockCS,@StatsStockExternalWarehouse,@StatsStockFG,@UBGStatusChange); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AB_LT", item.AB_LT == null ? (object)DBNull.Value : item.AB_LT);
			sqlCommand.Parameters.AddWithValue("AB_LT_EDI", item.AB_LT_EDI == null ? (object)DBNull.Value : item.AB_LT_EDI);
			sqlCommand.Parameters.AddWithValue("ABPosHorizon1", item.ABPosHorizon1 == null ? (object)DBNull.Value : item.ABPosHorizon1);
			sqlCommand.Parameters.AddWithValue("ABPosHorizon2", item.ABPosHorizon2 == null ? (object)DBNull.Value : item.ABPosHorizon2);
			sqlCommand.Parameters.AddWithValue("ABPosHorizon3", item.ABPosHorizon3 == null ? (object)DBNull.Value : item.ABPosHorizon3);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
			sqlCommand.Parameters.AddWithValue("BVBookedEdit", item.BVBookedEdit == null ? (object)DBNull.Value : item.BVBookedEdit);
			sqlCommand.Parameters.AddWithValue("BVDoneEdit", item.BVDoneEdit == null ? (object)DBNull.Value : item.BVDoneEdit);
			sqlCommand.Parameters.AddWithValue("BVFaCreate", item.BVFaCreate == null ? (object)DBNull.Value : item.BVFaCreate);
			sqlCommand.Parameters.AddWithValue("Configuration", item.Configuration == null ? (object)DBNull.Value : item.Configuration);
			sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments", item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
			sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees", item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
			sqlCommand.Parameters.AddWithValue("ConfigurationReplacements", item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
			sqlCommand.Parameters.AddWithValue("ConfigurationReporting", item.ConfigurationReporting == null ? (object)DBNull.Value : item.ConfigurationReporting);
			sqlCommand.Parameters.AddWithValue("ConfirmationBookedEdit", item.ConfirmationBookedEdit == null ? (object)DBNull.Value : item.ConfirmationBookedEdit);
			sqlCommand.Parameters.AddWithValue("ConfirmationCreate", item.ConfirmationCreate == null ? (object)DBNull.Value : item.ConfirmationCreate);
			sqlCommand.Parameters.AddWithValue("ConfirmationDelete", item.ConfirmationDelete == null ? (object)DBNull.Value : item.ConfirmationDelete);
			sqlCommand.Parameters.AddWithValue("ConfirmationDeliveryNote", item.ConfirmationDeliveryNote == null ? (object)DBNull.Value : item.ConfirmationDeliveryNote);
			sqlCommand.Parameters.AddWithValue("ConfirmationDoneEdit", item.ConfirmationDoneEdit == null ? (object)DBNull.Value : item.ConfirmationDoneEdit);
			sqlCommand.Parameters.AddWithValue("ConfirmationEdit", item.ConfirmationEdit == null ? (object)DBNull.Value : item.ConfirmationEdit);
			sqlCommand.Parameters.AddWithValue("ConfirmationPositionEdit", item.ConfirmationPositionEdit == null ? (object)DBNull.Value : item.ConfirmationPositionEdit);
			sqlCommand.Parameters.AddWithValue("ConfirmationPositionProduction", item.ConfirmationPositionProduction == null ? (object)DBNull.Value : item.ConfirmationPositionProduction);
			sqlCommand.Parameters.AddWithValue("ConfirmationReport", item.ConfirmationReport == null ? (object)DBNull.Value : item.ConfirmationReport);
			sqlCommand.Parameters.AddWithValue("ConfirmationValidate", item.ConfirmationValidate == null ? (object)DBNull.Value : item.ConfirmationValidate);
			sqlCommand.Parameters.AddWithValue("ConfirmationView", item.ConfirmationView == null ? (object)DBNull.Value : item.ConfirmationView);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CSInfoEdit", item.CSInfoEdit == null ? (object)DBNull.Value : item.CSInfoEdit);
			sqlCommand.Parameters.AddWithValue("DelforCreate", item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
			sqlCommand.Parameters.AddWithValue("DelforDelete", item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
			sqlCommand.Parameters.AddWithValue("DelforDeletePosition", item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
			sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation", item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
			sqlCommand.Parameters.AddWithValue("DelforReport", item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
			sqlCommand.Parameters.AddWithValue("DelforStatistics", item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
			sqlCommand.Parameters.AddWithValue("DelforView", item.DelforView == null ? (object)DBNull.Value : item.DelforView);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteBookedEdit", item.DeliveryNoteBookedEdit == null ? (object)DBNull.Value : item.DeliveryNoteBookedEdit);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteCreate", item.DeliveryNoteCreate == null ? (object)DBNull.Value : item.DeliveryNoteCreate);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteDelete", item.DeliveryNoteDelete == null ? (object)DBNull.Value : item.DeliveryNoteDelete);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteDoneEdit", item.DeliveryNoteDoneEdit == null ? (object)DBNull.Value : item.DeliveryNoteDoneEdit);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteEdit", item.DeliveryNoteEdit == null ? (object)DBNull.Value : item.DeliveryNoteEdit);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteLog", item.DeliveryNoteLog == null ? (object)DBNull.Value : item.DeliveryNoteLog);
			sqlCommand.Parameters.AddWithValue("DeliveryNotePositionEdit", item.DeliveryNotePositionEdit == null ? (object)DBNull.Value : item.DeliveryNotePositionEdit);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteReport", item.DeliveryNoteReport == null ? (object)DBNull.Value : item.DeliveryNoteReport);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteView", item.DeliveryNoteView == null ? (object)DBNull.Value : item.DeliveryNoteView);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon1", item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon2", item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon3", item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
			sqlCommand.Parameters.AddWithValue("EDI", item.EDI == null ? (object)DBNull.Value : item.EDI);
			sqlCommand.Parameters.AddWithValue("EDIDownloadFile", item.EDIDownloadFile == null ? (object)DBNull.Value : item.EDIDownloadFile);
			sqlCommand.Parameters.AddWithValue("EDIError", item.EDIError == null ? (object)DBNull.Value : item.EDIError);
			sqlCommand.Parameters.AddWithValue("EDIErrorEdit", item.EDIErrorEdit == null ? (object)DBNull.Value : item.EDIErrorEdit);
			sqlCommand.Parameters.AddWithValue("EDIErrorValidated", item.EDIErrorValidated == null ? (object)DBNull.Value : item.EDIErrorValidated);
			sqlCommand.Parameters.AddWithValue("EDILogOrderValidated", item.EDILogOrderValidated == null ? (object)DBNull.Value : item.EDILogOrderValidated);
			sqlCommand.Parameters.AddWithValue("EDIOrder", item.EDIOrder == null ? (object)DBNull.Value : item.EDIOrder);
			sqlCommand.Parameters.AddWithValue("EDIOrderEdit", item.EDIOrderEdit == null ? (object)DBNull.Value : item.EDIOrderEdit);
			sqlCommand.Parameters.AddWithValue("EDIOrderPositionEdit", item.EDIOrderPositionEdit == null ? (object)DBNull.Value : item.EDIOrderPositionEdit);
			sqlCommand.Parameters.AddWithValue("EDIOrderProduction", item.EDIOrderProduction == null ? (object)DBNull.Value : item.EDIOrderProduction);
			sqlCommand.Parameters.AddWithValue("EDIOrderProductionPosition", item.EDIOrderProductionPosition == null ? (object)DBNull.Value : item.EDIOrderProductionPosition);
			sqlCommand.Parameters.AddWithValue("EDIOrderReport", item.EDIOrderReport == null ? (object)DBNull.Value : item.EDIOrderReport);
			sqlCommand.Parameters.AddWithValue("EDIOrderValidated", item.EDIOrderValidated == null ? (object)DBNull.Value : item.EDIOrderValidated);
			sqlCommand.Parameters.AddWithValue("EDIOrderValidatedEdit", item.EDIOrderValidatedEdit == null ? (object)DBNull.Value : item.EDIOrderValidatedEdit);
			sqlCommand.Parameters.AddWithValue("FaABEdit", item.FaABEdit == null ? (object)DBNull.Value : item.FaABEdit);
			sqlCommand.Parameters.AddWithValue("FaABView", item.FaABView == null ? (object)DBNull.Value : item.FaABView);
			sqlCommand.Parameters.AddWithValue("FaActionBook", item.FaActionBook == null ? (object)DBNull.Value : item.FaActionBook);
			sqlCommand.Parameters.AddWithValue("FaActionComplete", item.FaActionComplete == null ? (object)DBNull.Value : item.FaActionComplete);
			sqlCommand.Parameters.AddWithValue("FaActionDelete", item.FaActionDelete == null ? (object)DBNull.Value : item.FaActionDelete);
			sqlCommand.Parameters.AddWithValue("FaActionPrint", item.FaActionPrint == null ? (object)DBNull.Value : item.FaActionPrint);
			sqlCommand.Parameters.AddWithValue("FaAdmin", item.FaAdmin == null ? (object)DBNull.Value : item.FaAdmin);
			sqlCommand.Parameters.AddWithValue("FAAKtualTerminUpdate", item.FAAKtualTerminUpdate == null ? (object)DBNull.Value : item.FAAKtualTerminUpdate);
			sqlCommand.Parameters.AddWithValue("FaAnalysis", item.FaAnalysis == null ? (object)DBNull.Value : item.FaAnalysis);
			sqlCommand.Parameters.AddWithValue("FAAuswertungEndkontrolle", item.FAAuswertungEndkontrolle == null ? (object)DBNull.Value : item.FAAuswertungEndkontrolle);
			sqlCommand.Parameters.AddWithValue("FABemerkungPlannug", item.FABemerkungPlannug == null ? (object)DBNull.Value : item.FABemerkungPlannug);
			sqlCommand.Parameters.AddWithValue("FABemerkungZuGewerk", item.FABemerkungZuGewerk == null ? (object)DBNull.Value : item.FABemerkungZuGewerk);
			sqlCommand.Parameters.AddWithValue("FABemerkungZuPrio", item.FABemerkungZuPrio == null ? (object)DBNull.Value : item.FABemerkungZuPrio);
			sqlCommand.Parameters.AddWithValue("FACancelHorizon1", item.FACancelHorizon1 == null ? (object)DBNull.Value : item.FACancelHorizon1);
			sqlCommand.Parameters.AddWithValue("FACancelHorizon2", item.FACancelHorizon2 == null ? (object)DBNull.Value : item.FACancelHorizon2);
			sqlCommand.Parameters.AddWithValue("FACancelHorizon3", item.FACancelHorizon3 == null ? (object)DBNull.Value : item.FACancelHorizon3);
			sqlCommand.Parameters.AddWithValue("FACommissionert", item.FACommissionert == null ? (object)DBNull.Value : item.FACommissionert);
			sqlCommand.Parameters.AddWithValue("FaCreate", item.FaCreate == null ? (object)DBNull.Value : item.FaCreate);
			sqlCommand.Parameters.AddWithValue("FACreateHorizon1", item.FACreateHorizon1 == null ? (object)DBNull.Value : item.FACreateHorizon1);
			sqlCommand.Parameters.AddWithValue("FACreateHorizon2", item.FACreateHorizon2 == null ? (object)DBNull.Value : item.FACreateHorizon2);
			sqlCommand.Parameters.AddWithValue("FACreateHorizon3", item.FACreateHorizon3 == null ? (object)DBNull.Value : item.FACreateHorizon3);
			sqlCommand.Parameters.AddWithValue("FaDatenEdit", item.FaDatenEdit == null ? (object)DBNull.Value : item.FaDatenEdit);
			sqlCommand.Parameters.AddWithValue("FaDatenView", item.FaDatenView == null ? (object)DBNull.Value : item.FaDatenView);
			sqlCommand.Parameters.AddWithValue("FaDelete", item.FaDelete == null ? (object)DBNull.Value : item.FaDelete);
			sqlCommand.Parameters.AddWithValue("FADrucken", item.FADrucken == null ? (object)DBNull.Value : item.FADrucken);
			sqlCommand.Parameters.AddWithValue("FaEdit", item.FaEdit == null ? (object)DBNull.Value : item.FaEdit);
			sqlCommand.Parameters.AddWithValue("FAErlidegen", item.FAErlidegen == null ? (object)DBNull.Value : item.FAErlidegen);
			sqlCommand.Parameters.AddWithValue("FAExcelUpdateWerk", item.FAExcelUpdateWerk == null ? (object)DBNull.Value : item.FAExcelUpdateWerk);
			sqlCommand.Parameters.AddWithValue("FAExcelUpdateWunsh", item.FAExcelUpdateWunsh == null ? (object)DBNull.Value : item.FAExcelUpdateWunsh);
			sqlCommand.Parameters.AddWithValue("FAFehlrMaterial", item.FAFehlrMaterial == null ? (object)DBNull.Value : item.FAFehlrMaterial);
			sqlCommand.Parameters.AddWithValue("FaHomeAnalysis", item.FaHomeAnalysis == null ? (object)DBNull.Value : item.FaHomeAnalysis);
			sqlCommand.Parameters.AddWithValue("FaHomeUpdate", item.FaHomeUpdate == null ? (object)DBNull.Value : item.FaHomeUpdate);
			sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei", item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
			sqlCommand.Parameters.AddWithValue("FaPlanningEdit", item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
			sqlCommand.Parameters.AddWithValue("FaPlanningView", item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
			sqlCommand.Parameters.AddWithValue("FAPlannung", item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
			sqlCommand.Parameters.AddWithValue("FAPlannungTechnick", item.FAPlannungTechnick == null ? (object)DBNull.Value : item.FAPlannungTechnick);
			sqlCommand.Parameters.AddWithValue("FAPriesZeitUpdate", item.FAPriesZeitUpdate == null ? (object)DBNull.Value : item.FAPriesZeitUpdate);
			sqlCommand.Parameters.AddWithValue("FAProductionPlannung", item.FAProductionPlannung == null ? (object)DBNull.Value : item.FAProductionPlannung);
			sqlCommand.Parameters.AddWithValue("FAStappleDruck", item.FAStappleDruck == null ? (object)DBNull.Value : item.FAStappleDruck);
			sqlCommand.Parameters.AddWithValue("FAStatusAlbania", item.FAStatusAlbania == null ? (object)DBNull.Value : item.FAStatusAlbania);
			sqlCommand.Parameters.AddWithValue("FAStatusCzech", item.FAStatusCzech == null ? (object)DBNull.Value : item.FAStatusCzech);
			sqlCommand.Parameters.AddWithValue("FAStatusTunisia", item.FAStatusTunisia == null ? (object)DBNull.Value : item.FAStatusTunisia);
			sqlCommand.Parameters.AddWithValue("FAStorno", item.FAStorno == null ? (object)DBNull.Value : item.FAStorno);
			sqlCommand.Parameters.AddWithValue("FAStucklist", item.FAStucklist == null ? (object)DBNull.Value : item.FAStucklist);
			sqlCommand.Parameters.AddWithValue("FaTechnicEdit", item.FaTechnicEdit == null ? (object)DBNull.Value : item.FaTechnicEdit);
			sqlCommand.Parameters.AddWithValue("FaTechnicView", item.FaTechnicView == null ? (object)DBNull.Value : item.FaTechnicView);
			sqlCommand.Parameters.AddWithValue("FATerminWerk", item.FATerminWerk == null ? (object)DBNull.Value : item.FATerminWerk);
			sqlCommand.Parameters.AddWithValue("FAUpdateBemerkungExtern", item.FAUpdateBemerkungExtern == null ? (object)DBNull.Value : item.FAUpdateBemerkungExtern);
			sqlCommand.Parameters.AddWithValue("FAUpdateByArticle", item.FAUpdateByArticle == null ? (object)DBNull.Value : item.FAUpdateByArticle);
			sqlCommand.Parameters.AddWithValue("FAUpdateByFA", item.FAUpdateByFA == null ? (object)DBNull.Value : item.FAUpdateByFA);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1", item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2", item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3", item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
			sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin", item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
			sqlCommand.Parameters.AddWithValue("Fertigung", item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
			sqlCommand.Parameters.AddWithValue("FertigungLog", item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
			sqlCommand.Parameters.AddWithValue("ForcastCreate", item.ForcastCreate == null ? (object)DBNull.Value : item.ForcastCreate);
			sqlCommand.Parameters.AddWithValue("ForcastDelete", item.ForcastDelete == null ? (object)DBNull.Value : item.ForcastDelete);
			sqlCommand.Parameters.AddWithValue("ForcastEdit", item.ForcastEdit == null ? (object)DBNull.Value : item.ForcastEdit);
			sqlCommand.Parameters.AddWithValue("ForcastLog", item.ForcastLog == null ? (object)DBNull.Value : item.ForcastLog);
			sqlCommand.Parameters.AddWithValue("ForcastPositionEdit", item.ForcastPositionEdit == null ? (object)DBNull.Value : item.ForcastPositionEdit);
			sqlCommand.Parameters.AddWithValue("ForcastReport", item.ForcastReport == null ? (object)DBNull.Value : item.ForcastReport);
			sqlCommand.Parameters.AddWithValue("ForcastView", item.ForcastView == null ? (object)DBNull.Value : item.ForcastView);
			sqlCommand.Parameters.AddWithValue("FRCPosHorizon1", item.FRCPosHorizon1 == null ? (object)DBNull.Value : item.FRCPosHorizon1);
			sqlCommand.Parameters.AddWithValue("FRCPosHorizon2", item.FRCPosHorizon2 == null ? (object)DBNull.Value : item.FRCPosHorizon2);
			sqlCommand.Parameters.AddWithValue("FRCPosHorizon3", item.FRCPosHorizon3 == null ? (object)DBNull.Value : item.FRCPosHorizon3);
			sqlCommand.Parameters.AddWithValue("GSPosHorizon1", item.GSPosHorizon1 == null ? (object)DBNull.Value : item.GSPosHorizon1);
			sqlCommand.Parameters.AddWithValue("GSPosHorizon2", item.GSPosHorizon2 == null ? (object)DBNull.Value : item.GSPosHorizon2);
			sqlCommand.Parameters.AddWithValue("GSPosHorizon3", item.GSPosHorizon3 == null ? (object)DBNull.Value : item.GSPosHorizon3);
			sqlCommand.Parameters.AddWithValue("GutschriftBookedEdit", item.GutschriftBookedEdit == null ? (object)DBNull.Value : item.GutschriftBookedEdit);
			sqlCommand.Parameters.AddWithValue("GutschriftCreate", item.GutschriftCreate == null ? (object)DBNull.Value : item.GutschriftCreate);
			sqlCommand.Parameters.AddWithValue("GutschriftDelete", item.GutschriftDelete == null ? (object)DBNull.Value : item.GutschriftDelete);
			sqlCommand.Parameters.AddWithValue("GutschriftDoneEdit", item.GutschriftDoneEdit == null ? (object)DBNull.Value : item.GutschriftDoneEdit);
			sqlCommand.Parameters.AddWithValue("GutschriftEdit", item.GutschriftEdit == null ? (object)DBNull.Value : item.GutschriftEdit);
			sqlCommand.Parameters.AddWithValue("GutschriftLog", item.GutschriftLog == null ? (object)DBNull.Value : item.GutschriftLog);
			sqlCommand.Parameters.AddWithValue("GutschriftPositionEdit", item.GutschriftPositionEdit == null ? (object)DBNull.Value : item.GutschriftPositionEdit);
			sqlCommand.Parameters.AddWithValue("GutschriftReport", item.GutschriftReport == null ? (object)DBNull.Value : item.GutschriftReport);
			sqlCommand.Parameters.AddWithValue("GutschriftView", item.GutschriftView == null ? (object)DBNull.Value : item.GutschriftView);
			sqlCommand.Parameters.AddWithValue("InsideSalesChecks", item.InsideSalesChecks == null ? (object)DBNull.Value : item.InsideSalesChecks);
			sqlCommand.Parameters.AddWithValue("InsideSalesChecksArchive", item.InsideSalesChecksArchive == null ? (object)DBNull.Value : item.InsideSalesChecksArchive);
			sqlCommand.Parameters.AddWithValue("InsideSalesCustomerSummary", item.InsideSalesCustomerSummary == null ? (object)DBNull.Value : item.InsideSalesCustomerSummary);
			sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluation", item.InsideSalesMinimumStockEvaluation == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluation);
			sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluationTable", item.InsideSalesMinimumStockEvaluationTable == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluationTable);
			sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrders", item.InsideSalesOverdueOrders == null ? (object)DBNull.Value : item.InsideSalesOverdueOrders);
			sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrdersTable", item.InsideSalesOverdueOrdersTable == null ? (object)DBNull.Value : item.InsideSalesOverdueOrdersTable);
			sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrders", item.InsideSalesTotalUnbookedOrders == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrders);
			sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrdersTable", item.InsideSalesTotalUnbookedOrdersTable == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrdersTable);
			sqlCommand.Parameters.AddWithValue("InsideSalesTurnoverCurrentWeek", item.InsideSalesTurnoverCurrentWeek == null ? (object)DBNull.Value : item.InsideSalesTurnoverCurrentWeek);
			sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
			sqlCommand.Parameters.AddWithValue("LSPosHorizon1", item.LSPosHorizon1 == null ? (object)DBNull.Value : item.LSPosHorizon1);
			sqlCommand.Parameters.AddWithValue("LSPosHorizon2", item.LSPosHorizon2 == null ? (object)DBNull.Value : item.LSPosHorizon2);
			sqlCommand.Parameters.AddWithValue("LSPosHorizon3", item.LSPosHorizon3 == null ? (object)DBNull.Value : item.LSPosHorizon3);
			sqlCommand.Parameters.AddWithValue("mId", item.mId == null ? (object)DBNull.Value : item.mId);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("OrderProcessing", item.OrderProcessing == null ? (object)DBNull.Value : item.OrderProcessing);
			sqlCommand.Parameters.AddWithValue("OrderProcessingLog", item.OrderProcessingLog == null ? (object)DBNull.Value : item.OrderProcessingLog);
			sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
			sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
			sqlCommand.Parameters.AddWithValue("RahmenAddAB", item.RahmenAddAB == null ? (object)DBNull.Value : item.RahmenAddAB);
			sqlCommand.Parameters.AddWithValue("RahmenAddPositions", item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
			sqlCommand.Parameters.AddWithValue("RahmenCancelation", item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
			sqlCommand.Parameters.AddWithValue("RahmenClosure", item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
			sqlCommand.Parameters.AddWithValue("RahmenDelete", item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
			sqlCommand.Parameters.AddWithValue("RahmenDeletePositions", item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
			sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow", item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
			sqlCommand.Parameters.AddWithValue("RahmenEditHeader", item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
			sqlCommand.Parameters.AddWithValue("RahmenEditPositions", item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
			sqlCommand.Parameters.AddWithValue("RahmenHistory", item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
			sqlCommand.Parameters.AddWithValue("RahmenValdation", item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
			sqlCommand.Parameters.AddWithValue("RAPosHorizon1", item.RAPosHorizon1 == null ? (object)DBNull.Value : item.RAPosHorizon1);
			sqlCommand.Parameters.AddWithValue("RAPosHorizon2", item.RAPosHorizon2 == null ? (object)DBNull.Value : item.RAPosHorizon2);
			sqlCommand.Parameters.AddWithValue("RAPosHorizon3", item.RAPosHorizon3 == null ? (object)DBNull.Value : item.RAPosHorizon3);
			sqlCommand.Parameters.AddWithValue("Rechnung", item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
			sqlCommand.Parameters.AddWithValue("RechnungAutoCreation", item.RechnungAutoCreation == null ? (object)DBNull.Value : item.RechnungAutoCreation);
			sqlCommand.Parameters.AddWithValue("RechnungBookedEdit", item.RechnungBookedEdit == null ? (object)DBNull.Value : item.RechnungBookedEdit);
			sqlCommand.Parameters.AddWithValue("RechnungConfig", item.RechnungConfig == null ? (object)DBNull.Value : item.RechnungConfig);
			sqlCommand.Parameters.AddWithValue("RechnungDelete", item.RechnungDelete == null ? (object)DBNull.Value : item.RechnungDelete);
			sqlCommand.Parameters.AddWithValue("RechnungDoneEdit", item.RechnungDoneEdit == null ? (object)DBNull.Value : item.RechnungDoneEdit);
			sqlCommand.Parameters.AddWithValue("RechnungManualCreation", item.RechnungManualCreation == null ? (object)DBNull.Value : item.RechnungManualCreation);
			sqlCommand.Parameters.AddWithValue("RechnungReport", item.RechnungReport == null ? (object)DBNull.Value : item.RechnungReport);
			sqlCommand.Parameters.AddWithValue("RechnungSend", item.RechnungSend == null ? (object)DBNull.Value : item.RechnungSend);
			sqlCommand.Parameters.AddWithValue("RechnungValidate", item.RechnungValidate == null ? (object)DBNull.Value : item.RechnungValidate);
			sqlCommand.Parameters.AddWithValue("RGPosHorizon1", item.RGPosHorizon1 == null ? (object)DBNull.Value : item.RGPosHorizon1);
			sqlCommand.Parameters.AddWithValue("RGPosHorizon2", item.RGPosHorizon2 == null ? (object)DBNull.Value : item.RGPosHorizon2);
			sqlCommand.Parameters.AddWithValue("RGPosHorizon3", item.RGPosHorizon3 == null ? (object)DBNull.Value : item.RGPosHorizon3);
			sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
			sqlCommand.Parameters.AddWithValue("StatsBacklogFGAdmin", item.StatsBacklogFGAdmin == null ? (object)DBNull.Value : item.StatsBacklogFGAdmin);
			sqlCommand.Parameters.AddWithValue("StatsBacklogHWAdmin", item.StatsBacklogHWAdmin == null ? (object)DBNull.Value : item.StatsBacklogHWAdmin);
			sqlCommand.Parameters.AddWithValue("StatsCapaCutting", item.StatsCapaCutting == null ? (object)DBNull.Value : item.StatsCapaCutting);
			sqlCommand.Parameters.AddWithValue("StatsCapaHorizons", item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
			sqlCommand.Parameters.AddWithValue("StatsCapaLong", item.StatsCapaLong == null ? (object)DBNull.Value : item.StatsCapaLong);
			sqlCommand.Parameters.AddWithValue("StatsCapaPlanning", item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
			sqlCommand.Parameters.AddWithValue("StatsCapaShort", item.StatsCapaShort == null ? (object)DBNull.Value : item.StatsCapaShort);
			sqlCommand.Parameters.AddWithValue("StatsRechnungAL", item.StatsRechnungAL == null ? (object)DBNull.Value : item.StatsRechnungAL);
			sqlCommand.Parameters.AddWithValue("StatsRechnungBETN", item.StatsRechnungBETN == null ? (object)DBNull.Value : item.StatsRechnungBETN);
			sqlCommand.Parameters.AddWithValue("StatsRechnungCZ", item.StatsRechnungCZ == null ? (object)DBNull.Value : item.StatsRechnungCZ);
			sqlCommand.Parameters.AddWithValue("StatsRechnungDE", item.StatsRechnungDE == null ? (object)DBNull.Value : item.StatsRechnungDE);
			sqlCommand.Parameters.AddWithValue("StatsRechnungGZTN", item.StatsRechnungGZTN == null ? (object)DBNull.Value : item.StatsRechnungGZTN);
			sqlCommand.Parameters.AddWithValue("StatsRechnungTN", item.StatsRechnungTN == null ? (object)DBNull.Value : item.StatsRechnungTN);
			sqlCommand.Parameters.AddWithValue("StatsRechnungWS", item.StatsRechnungWS == null ? (object)DBNull.Value : item.StatsRechnungWS);
			sqlCommand.Parameters.AddWithValue("StatsStockCS", item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
			sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse", item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
			sqlCommand.Parameters.AddWithValue("StatsStockFG", item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
			sqlCommand.Parameters.AddWithValue("UBGStatusChange", item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 208; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__CTS_AccessProfile] ([AB_LT],[AB_LT_EDI],[ABPosHorizon1],[ABPosHorizon2],[ABPosHorizon3],[AccessProfileName],[Administration],[BVBookedEdit],[BVDoneEdit],[BVFaCreate],[Configuration],[ConfigurationAppoitments],[ConfigurationChangeEmployees],[ConfigurationReplacements],[ConfigurationReporting],[ConfirmationBookedEdit],[ConfirmationCreate],[ConfirmationDelete],[ConfirmationDeliveryNote],[ConfirmationDoneEdit],[ConfirmationEdit],[ConfirmationPositionEdit],[ConfirmationPositionProduction],[ConfirmationReport],[ConfirmationValidate],[ConfirmationView],[CreationTime],[CreationUserId],[CSInfoEdit],[DelforCreate],[DelforDelete],[DelforDeletePosition],[DelforOrderConfirmation],[DelforReport],[DelforStatistics],[DelforView],[DeliveryNoteBookedEdit],[DeliveryNoteCreate],[DeliveryNoteDelete],[DeliveryNoteDoneEdit],[DeliveryNoteEdit],[DeliveryNoteLog],[DeliveryNotePositionEdit],[DeliveryNoteReport],[DeliveryNoteView],[DLFPosHorizon1],[DLFPosHorizon2],[DLFPosHorizon3],[EDI],[EDIDownloadFile],[EDIError],[EDIErrorEdit],[EDIErrorValidated],[EDILogOrderValidated],[EDIOrder],[EDIOrderEdit],[EDIOrderPositionEdit],[EDIOrderProduction],[EDIOrderProductionPosition],[EDIOrderReport],[EDIOrderValidated],[EDIOrderValidatedEdit],[FaABEdit],[FaABView],[FaActionBook],[FaActionComplete],[FaActionDelete],[FaActionPrint],[FaAdmin],[FAAKtualTerminUpdate],[FaAnalysis],[FAAuswertungEndkontrolle],[FABemerkungPlannug],[FABemerkungZuGewerk],[FABemerkungZuPrio],[FACancelHorizon1],[FACancelHorizon2],[FACancelHorizon3],[FACommissionert],[FaCreate],[FACreateHorizon1],[FACreateHorizon2],[FACreateHorizon3],[FaDatenEdit],[FaDatenView],[FaDelete],[FADrucken],[FaEdit],[FAErlidegen],[FAExcelUpdateWerk],[FAExcelUpdateWunsh],[FAFehlrMaterial],[FaHomeAnalysis],[FaHomeUpdate],[FALaufkarteSchneiderei],[FaPlanningEdit],[FaPlanningView],[FAPlannung],[FAPlannungTechnick],[FAPriesZeitUpdate],[FAProductionPlannung],[FAStappleDruck],[FAStatusAlbania],[FAStatusCzech],[FAStatusTunisia],[FAStorno],[FAStucklist],[FaTechnicEdit],[FaTechnicView],[FATerminWerk],[FAUpdateBemerkungExtern],[FAUpdateByArticle],[FAUpdateByFA],[FAUpdateTerminHorizon1],[FAUpdateTerminHorizon2],[FAUpdateTerminHorizon3],[FAWerkWunshAdmin],[Fertigung],[FertigungLog],[ForcastCreate],[ForcastDelete],[ForcastEdit],[ForcastLog],[ForcastPositionEdit],[ForcastReport],[ForcastView],[FRCPosHorizon1],[FRCPosHorizon2],[FRCPosHorizon3],[GSPosHorizon1],[GSPosHorizon2],[GSPosHorizon3],[GutschriftBookedEdit],[GutschriftCreate],[GutschriftDelete],[GutschriftDoneEdit],[GutschriftEdit],[GutschriftLog],[GutschriftPositionEdit],[GutschriftReport],[GutschriftView],[InsideSalesChecks],[InsideSalesChecksArchive],[InsideSalesCustomerSummary],[InsideSalesMinimumStockEvaluation],[InsideSalesMinimumStockEvaluationTable],[InsideSalesOverdueOrders],[InsideSalesOverdueOrdersTable],[InsideSalesTotalUnbookedOrders],[InsideSalesTotalUnbookedOrdersTable],[InsideSalesTurnoverCurrentWeek],[IsDefault],[LSPosHorizon1],[LSPosHorizon2],[LSPosHorizon3],[mId],[ModuleActivated],[OrderProcessing],[OrderProcessingLog],[Rahmen],[RahmenAdd],[RahmenAddAB],[RahmenAddPositions],[RahmenCancelation],[RahmenClosure],[RahmenDelete],[RahmenDeletePositions],[RahmenDocumentFlow],[RahmenEditHeader],[RahmenEditPositions],[RahmenHistory],[RahmenValdation],[RAPosHorizon1],[RAPosHorizon2],[RAPosHorizon3],[Rechnung],[RechnungAutoCreation],[RechnungBookedEdit],[RechnungConfig],[RechnungDelete],[RechnungDoneEdit],[RechnungManualCreation],[RechnungReport],[RechnungSend],[RechnungValidate],[RGPosHorizon1],[RGPosHorizon2],[RGPosHorizon3],[Statistics],[StatsBacklogFGAdmin],[StatsBacklogHWAdmin],[StatsCapaCutting],[StatsCapaHorizons],[StatsCapaLong],[StatsCapaPlanning],[StatsCapaShort],[StatsRechnungAL],[StatsRechnungBETN],[StatsRechnungCZ],[StatsRechnungDE],[StatsRechnungGZTN],[StatsRechnungTN],[StatsRechnungWS],[StatsStockCS],[StatsStockExternalWarehouse],[StatsStockFG],[UBGStatusChange]) VALUES ( "

						+ "@AB_LT" + i + ","
						+ "@AB_LT_EDI" + i + ","
						+ "@ABPosHorizon1" + i + ","
						+ "@ABPosHorizon2" + i + ","
						+ "@ABPosHorizon3" + i + ","
						+ "@AccessProfileName" + i + ","
						+ "@Administration" + i + ","
						+ "@BVBookedEdit" + i + ","
						+ "@BVDoneEdit" + i + ","
						+ "@BVFaCreate" + i + ","
						+ "@Configuration" + i + ","
						+ "@ConfigurationAppoitments" + i + ","
						+ "@ConfigurationChangeEmployees" + i + ","
						+ "@ConfigurationReplacements" + i + ","
						+ "@ConfigurationReporting" + i + ","
						+ "@ConfirmationBookedEdit" + i + ","
						+ "@ConfirmationCreate" + i + ","
						+ "@ConfirmationDelete" + i + ","
						+ "@ConfirmationDeliveryNote" + i + ","
						+ "@ConfirmationDoneEdit" + i + ","
						+ "@ConfirmationEdit" + i + ","
						+ "@ConfirmationPositionEdit" + i + ","
						+ "@ConfirmationPositionProduction" + i + ","
						+ "@ConfirmationReport" + i + ","
						+ "@ConfirmationValidate" + i + ","
						+ "@ConfirmationView" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CSInfoEdit" + i + ","
						+ "@DelforCreate" + i + ","
						+ "@DelforDelete" + i + ","
						+ "@DelforDeletePosition" + i + ","
						+ "@DelforOrderConfirmation" + i + ","
						+ "@DelforReport" + i + ","
						+ "@DelforStatistics" + i + ","
						+ "@DelforView" + i + ","
						+ "@DeliveryNoteBookedEdit" + i + ","
						+ "@DeliveryNoteCreate" + i + ","
						+ "@DeliveryNoteDelete" + i + ","
						+ "@DeliveryNoteDoneEdit" + i + ","
						+ "@DeliveryNoteEdit" + i + ","
						+ "@DeliveryNoteLog" + i + ","
						+ "@DeliveryNotePositionEdit" + i + ","
						+ "@DeliveryNoteReport" + i + ","
						+ "@DeliveryNoteView" + i + ","
						+ "@DLFPosHorizon1" + i + ","
						+ "@DLFPosHorizon2" + i + ","
						+ "@DLFPosHorizon3" + i + ","
						+ "@EDI" + i + ","
						+ "@EDIDownloadFile" + i + ","
						+ "@EDIError" + i + ","
						+ "@EDIErrorEdit" + i + ","
						+ "@EDIErrorValidated" + i + ","
						+ "@EDILogOrderValidated" + i + ","
						+ "@EDIOrder" + i + ","
						+ "@EDIOrderEdit" + i + ","
						+ "@EDIOrderPositionEdit" + i + ","
						+ "@EDIOrderProduction" + i + ","
						+ "@EDIOrderProductionPosition" + i + ","
						+ "@EDIOrderReport" + i + ","
						+ "@EDIOrderValidated" + i + ","
						+ "@EDIOrderValidatedEdit" + i + ","
						+ "@FaABEdit" + i + ","
						+ "@FaABView" + i + ","
						+ "@FaActionBook" + i + ","
						+ "@FaActionComplete" + i + ","
						+ "@FaActionDelete" + i + ","
						+ "@FaActionPrint" + i + ","
						+ "@FaAdmin" + i + ","
						+ "@FAAKtualTerminUpdate" + i + ","
						+ "@FaAnalysis" + i + ","
						+ "@FAAuswertungEndkontrolle" + i + ","
						+ "@FABemerkungPlannug" + i + ","
						+ "@FABemerkungZuGewerk" + i + ","
						+ "@FABemerkungZuPrio" + i + ","
						+ "@FACancelHorizon1" + i + ","
						+ "@FACancelHorizon2" + i + ","
						+ "@FACancelHorizon3" + i + ","
						+ "@FACommissionert" + i + ","
						+ "@FaCreate" + i + ","
						+ "@FACreateHorizon1" + i + ","
						+ "@FACreateHorizon2" + i + ","
						+ "@FACreateHorizon3" + i + ","
						+ "@FaDatenEdit" + i + ","
						+ "@FaDatenView" + i + ","
						+ "@FaDelete" + i + ","
						+ "@FADrucken" + i + ","
						+ "@FaEdit" + i + ","
						+ "@FAErlidegen" + i + ","
						+ "@FAExcelUpdateWerk" + i + ","
						+ "@FAExcelUpdateWunsh" + i + ","
						+ "@FAFehlrMaterial" + i + ","
						+ "@FaHomeAnalysis" + i + ","
						+ "@FaHomeUpdate" + i + ","
						+ "@FALaufkarteSchneiderei" + i + ","
						+ "@FaPlanningEdit" + i + ","
						+ "@FaPlanningView" + i + ","
						+ "@FAPlannung" + i + ","
						+ "@FAPlannungTechnick" + i + ","
						+ "@FAPriesZeitUpdate" + i + ","
						+ "@FAProductionPlannung" + i + ","
						+ "@FAStappleDruck" + i + ","
						+ "@FAStatusAlbania" + i + ","
						+ "@FAStatusCzech" + i + ","
						+ "@FAStatusTunisia" + i + ","
						+ "@FAStorno" + i + ","
						+ "@FAStucklist" + i + ","
						+ "@FaTechnicEdit" + i + ","
						+ "@FaTechnicView" + i + ","
						+ "@FATerminWerk" + i + ","
						+ "@FAUpdateBemerkungExtern" + i + ","
						+ "@FAUpdateByArticle" + i + ","
						+ "@FAUpdateByFA" + i + ","
						+ "@FAUpdateTerminHorizon1" + i + ","
						+ "@FAUpdateTerminHorizon2" + i + ","
						+ "@FAUpdateTerminHorizon3" + i + ","
						+ "@FAWerkWunshAdmin" + i + ","
						+ "@Fertigung" + i + ","
						+ "@FertigungLog" + i + ","
						+ "@ForcastCreate" + i + ","
						+ "@ForcastDelete" + i + ","
						+ "@ForcastEdit" + i + ","
						+ "@ForcastLog" + i + ","
						+ "@ForcastPositionEdit" + i + ","
						+ "@ForcastReport" + i + ","
						+ "@ForcastView" + i + ","
						+ "@FRCPosHorizon1" + i + ","
						+ "@FRCPosHorizon2" + i + ","
						+ "@FRCPosHorizon3" + i + ","
						+ "@GSPosHorizon1" + i + ","
						+ "@GSPosHorizon2" + i + ","
						+ "@GSPosHorizon3" + i + ","
						+ "@GutschriftBookedEdit" + i + ","
						+ "@GutschriftCreate" + i + ","
						+ "@GutschriftDelete" + i + ","
						+ "@GutschriftDoneEdit" + i + ","
						+ "@GutschriftEdit" + i + ","
						+ "@GutschriftLog" + i + ","
						+ "@GutschriftPositionEdit" + i + ","
						+ "@GutschriftReport" + i + ","
						+ "@GutschriftView" + i + ","
						+ "@InsideSalesChecks" + i + ","
						+ "@InsideSalesChecksArchive" + i + ","
						+ "@InsideSalesCustomerSummary" + i + ","
						+ "@InsideSalesMinimumStockEvaluation" + i + ","
						+ "@InsideSalesMinimumStockEvaluationTable" + i + ","
						+ "@InsideSalesOverdueOrders" + i + ","
						+ "@InsideSalesOverdueOrdersTable" + i + ","
						+ "@InsideSalesTotalUnbookedOrders" + i + ","
						+ "@InsideSalesTotalUnbookedOrdersTable" + i + ","
						+ "@InsideSalesTurnoverCurrentWeek" + i + ","
						+ "@IsDefault" + i + ","
						+ "@LSPosHorizon1" + i + ","
						+ "@LSPosHorizon2" + i + ","
						+ "@LSPosHorizon3" + i + ","
						+ "@mId" + i + ","
						+ "@ModuleActivated" + i + ","
						+ "@OrderProcessing" + i + ","
						+ "@OrderProcessingLog" + i + ","
						+ "@Rahmen" + i + ","
						+ "@RahmenAdd" + i + ","
						+ "@RahmenAddAB" + i + ","
						+ "@RahmenAddPositions" + i + ","
						+ "@RahmenCancelation" + i + ","
						+ "@RahmenClosure" + i + ","
						+ "@RahmenDelete" + i + ","
						+ "@RahmenDeletePositions" + i + ","
						+ "@RahmenDocumentFlow" + i + ","
						+ "@RahmenEditHeader" + i + ","
						+ "@RahmenEditPositions" + i + ","
						+ "@RahmenHistory" + i + ","
						+ "@RahmenValdation" + i + ","
						+ "@RAPosHorizon1" + i + ","
						+ "@RAPosHorizon2" + i + ","
						+ "@RAPosHorizon3" + i + ","
						+ "@Rechnung" + i + ","
						+ "@RechnungAutoCreation" + i + ","
						+ "@RechnungBookedEdit" + i + ","
						+ "@RechnungConfig" + i + ","
						+ "@RechnungDelete" + i + ","
						+ "@RechnungDoneEdit" + i + ","
						+ "@RechnungManualCreation" + i + ","
						+ "@RechnungReport" + i + ","
						+ "@RechnungSend" + i + ","
						+ "@RechnungValidate" + i + ","
						+ "@RGPosHorizon1" + i + ","
						+ "@RGPosHorizon2" + i + ","
						+ "@RGPosHorizon3" + i + ","
						+ "@Statistics" + i + ","
						+ "@StatsBacklogFGAdmin" + i + ","
						+ "@StatsBacklogHWAdmin" + i + ","
						+ "@StatsCapaCutting" + i + ","
						+ "@StatsCapaHorizons" + i + ","
						+ "@StatsCapaLong" + i + ","
						+ "@StatsCapaPlanning" + i + ","
						+ "@StatsCapaShort" + i + ","
						+ "@StatsRechnungAL" + i + ","
						+ "@StatsRechnungBETN" + i + ","
						+ "@StatsRechnungCZ" + i + ","
						+ "@StatsRechnungDE" + i + ","
						+ "@StatsRechnungGZTN" + i + ","
						+ "@StatsRechnungTN" + i + ","
						+ "@StatsRechnungWS" + i + ","
						+ "@StatsStockCS" + i + ","
						+ "@StatsStockExternalWarehouse" + i + ","
						+ "@StatsStockFG" + i + ","
						+ "@UBGStatusChange" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AB_LT" + i, item.AB_LT == null ? (object)DBNull.Value : item.AB_LT);
					sqlCommand.Parameters.AddWithValue("AB_LT_EDI" + i, item.AB_LT_EDI == null ? (object)DBNull.Value : item.AB_LT_EDI);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon1" + i, item.ABPosHorizon1 == null ? (object)DBNull.Value : item.ABPosHorizon1);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon2" + i, item.ABPosHorizon2 == null ? (object)DBNull.Value : item.ABPosHorizon2);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon3" + i, item.ABPosHorizon3 == null ? (object)DBNull.Value : item.ABPosHorizon3);
					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("BVBookedEdit" + i, item.BVBookedEdit == null ? (object)DBNull.Value : item.BVBookedEdit);
					sqlCommand.Parameters.AddWithValue("BVDoneEdit" + i, item.BVDoneEdit == null ? (object)DBNull.Value : item.BVDoneEdit);
					sqlCommand.Parameters.AddWithValue("BVFaCreate" + i, item.BVFaCreate == null ? (object)DBNull.Value : item.BVFaCreate);
					sqlCommand.Parameters.AddWithValue("Configuration" + i, item.Configuration == null ? (object)DBNull.Value : item.Configuration);
					sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments" + i, item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
					sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees" + i, item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
					sqlCommand.Parameters.AddWithValue("ConfigurationReplacements" + i, item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
					sqlCommand.Parameters.AddWithValue("ConfigurationReporting" + i, item.ConfigurationReporting == null ? (object)DBNull.Value : item.ConfigurationReporting);
					sqlCommand.Parameters.AddWithValue("ConfirmationBookedEdit" + i, item.ConfirmationBookedEdit == null ? (object)DBNull.Value : item.ConfirmationBookedEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationCreate" + i, item.ConfirmationCreate == null ? (object)DBNull.Value : item.ConfirmationCreate);
					sqlCommand.Parameters.AddWithValue("ConfirmationDelete" + i, item.ConfirmationDelete == null ? (object)DBNull.Value : item.ConfirmationDelete);
					sqlCommand.Parameters.AddWithValue("ConfirmationDeliveryNote" + i, item.ConfirmationDeliveryNote == null ? (object)DBNull.Value : item.ConfirmationDeliveryNote);
					sqlCommand.Parameters.AddWithValue("ConfirmationDoneEdit" + i, item.ConfirmationDoneEdit == null ? (object)DBNull.Value : item.ConfirmationDoneEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationEdit" + i, item.ConfirmationEdit == null ? (object)DBNull.Value : item.ConfirmationEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationPositionEdit" + i, item.ConfirmationPositionEdit == null ? (object)DBNull.Value : item.ConfirmationPositionEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationPositionProduction" + i, item.ConfirmationPositionProduction == null ? (object)DBNull.Value : item.ConfirmationPositionProduction);
					sqlCommand.Parameters.AddWithValue("ConfirmationReport" + i, item.ConfirmationReport == null ? (object)DBNull.Value : item.ConfirmationReport);
					sqlCommand.Parameters.AddWithValue("ConfirmationValidate" + i, item.ConfirmationValidate == null ? (object)DBNull.Value : item.ConfirmationValidate);
					sqlCommand.Parameters.AddWithValue("ConfirmationView" + i, item.ConfirmationView == null ? (object)DBNull.Value : item.ConfirmationView);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CSInfoEdit" + i, item.CSInfoEdit == null ? (object)DBNull.Value : item.CSInfoEdit);
					sqlCommand.Parameters.AddWithValue("DelforCreate" + i, item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
					sqlCommand.Parameters.AddWithValue("DelforDelete" + i, item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
					sqlCommand.Parameters.AddWithValue("DelforDeletePosition" + i, item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
					sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation" + i, item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
					sqlCommand.Parameters.AddWithValue("DelforReport" + i, item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
					sqlCommand.Parameters.AddWithValue("DelforStatistics" + i, item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
					sqlCommand.Parameters.AddWithValue("DelforView" + i, item.DelforView == null ? (object)DBNull.Value : item.DelforView);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteBookedEdit" + i, item.DeliveryNoteBookedEdit == null ? (object)DBNull.Value : item.DeliveryNoteBookedEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteCreate" + i, item.DeliveryNoteCreate == null ? (object)DBNull.Value : item.DeliveryNoteCreate);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteDelete" + i, item.DeliveryNoteDelete == null ? (object)DBNull.Value : item.DeliveryNoteDelete);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteDoneEdit" + i, item.DeliveryNoteDoneEdit == null ? (object)DBNull.Value : item.DeliveryNoteDoneEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteEdit" + i, item.DeliveryNoteEdit == null ? (object)DBNull.Value : item.DeliveryNoteEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteLog" + i, item.DeliveryNoteLog == null ? (object)DBNull.Value : item.DeliveryNoteLog);
					sqlCommand.Parameters.AddWithValue("DeliveryNotePositionEdit" + i, item.DeliveryNotePositionEdit == null ? (object)DBNull.Value : item.DeliveryNotePositionEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteReport" + i, item.DeliveryNoteReport == null ? (object)DBNull.Value : item.DeliveryNoteReport);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteView" + i, item.DeliveryNoteView == null ? (object)DBNull.Value : item.DeliveryNoteView);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon1" + i, item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon2" + i, item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon3" + i, item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
					sqlCommand.Parameters.AddWithValue("EDI" + i, item.EDI == null ? (object)DBNull.Value : item.EDI);
					sqlCommand.Parameters.AddWithValue("EDIDownloadFile" + i, item.EDIDownloadFile == null ? (object)DBNull.Value : item.EDIDownloadFile);
					sqlCommand.Parameters.AddWithValue("EDIError" + i, item.EDIError == null ? (object)DBNull.Value : item.EDIError);
					sqlCommand.Parameters.AddWithValue("EDIErrorEdit" + i, item.EDIErrorEdit == null ? (object)DBNull.Value : item.EDIErrorEdit);
					sqlCommand.Parameters.AddWithValue("EDIErrorValidated" + i, item.EDIErrorValidated == null ? (object)DBNull.Value : item.EDIErrorValidated);
					sqlCommand.Parameters.AddWithValue("EDILogOrderValidated" + i, item.EDILogOrderValidated == null ? (object)DBNull.Value : item.EDILogOrderValidated);
					sqlCommand.Parameters.AddWithValue("EDIOrder" + i, item.EDIOrder == null ? (object)DBNull.Value : item.EDIOrder);
					sqlCommand.Parameters.AddWithValue("EDIOrderEdit" + i, item.EDIOrderEdit == null ? (object)DBNull.Value : item.EDIOrderEdit);
					sqlCommand.Parameters.AddWithValue("EDIOrderPositionEdit" + i, item.EDIOrderPositionEdit == null ? (object)DBNull.Value : item.EDIOrderPositionEdit);
					sqlCommand.Parameters.AddWithValue("EDIOrderProduction" + i, item.EDIOrderProduction == null ? (object)DBNull.Value : item.EDIOrderProduction);
					sqlCommand.Parameters.AddWithValue("EDIOrderProductionPosition" + i, item.EDIOrderProductionPosition == null ? (object)DBNull.Value : item.EDIOrderProductionPosition);
					sqlCommand.Parameters.AddWithValue("EDIOrderReport" + i, item.EDIOrderReport == null ? (object)DBNull.Value : item.EDIOrderReport);
					sqlCommand.Parameters.AddWithValue("EDIOrderValidated" + i, item.EDIOrderValidated == null ? (object)DBNull.Value : item.EDIOrderValidated);
					sqlCommand.Parameters.AddWithValue("EDIOrderValidatedEdit" + i, item.EDIOrderValidatedEdit == null ? (object)DBNull.Value : item.EDIOrderValidatedEdit);
					sqlCommand.Parameters.AddWithValue("FaABEdit" + i, item.FaABEdit == null ? (object)DBNull.Value : item.FaABEdit);
					sqlCommand.Parameters.AddWithValue("FaABView" + i, item.FaABView == null ? (object)DBNull.Value : item.FaABView);
					sqlCommand.Parameters.AddWithValue("FaActionBook" + i, item.FaActionBook == null ? (object)DBNull.Value : item.FaActionBook);
					sqlCommand.Parameters.AddWithValue("FaActionComplete" + i, item.FaActionComplete == null ? (object)DBNull.Value : item.FaActionComplete);
					sqlCommand.Parameters.AddWithValue("FaActionDelete" + i, item.FaActionDelete == null ? (object)DBNull.Value : item.FaActionDelete);
					sqlCommand.Parameters.AddWithValue("FaActionPrint" + i, item.FaActionPrint == null ? (object)DBNull.Value : item.FaActionPrint);
					sqlCommand.Parameters.AddWithValue("FaAdmin" + i, item.FaAdmin == null ? (object)DBNull.Value : item.FaAdmin);
					sqlCommand.Parameters.AddWithValue("FAAKtualTerminUpdate" + i, item.FAAKtualTerminUpdate == null ? (object)DBNull.Value : item.FAAKtualTerminUpdate);
					sqlCommand.Parameters.AddWithValue("FaAnalysis" + i, item.FaAnalysis == null ? (object)DBNull.Value : item.FaAnalysis);
					sqlCommand.Parameters.AddWithValue("FAAuswertungEndkontrolle" + i, item.FAAuswertungEndkontrolle == null ? (object)DBNull.Value : item.FAAuswertungEndkontrolle);
					sqlCommand.Parameters.AddWithValue("FABemerkungPlannug" + i, item.FABemerkungPlannug == null ? (object)DBNull.Value : item.FABemerkungPlannug);
					sqlCommand.Parameters.AddWithValue("FABemerkungZuGewerk" + i, item.FABemerkungZuGewerk == null ? (object)DBNull.Value : item.FABemerkungZuGewerk);
					sqlCommand.Parameters.AddWithValue("FABemerkungZuPrio" + i, item.FABemerkungZuPrio == null ? (object)DBNull.Value : item.FABemerkungZuPrio);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon1" + i, item.FACancelHorizon1 == null ? (object)DBNull.Value : item.FACancelHorizon1);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon2" + i, item.FACancelHorizon2 == null ? (object)DBNull.Value : item.FACancelHorizon2);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon3" + i, item.FACancelHorizon3 == null ? (object)DBNull.Value : item.FACancelHorizon3);
					sqlCommand.Parameters.AddWithValue("FACommissionert" + i, item.FACommissionert == null ? (object)DBNull.Value : item.FACommissionert);
					sqlCommand.Parameters.AddWithValue("FaCreate" + i, item.FaCreate == null ? (object)DBNull.Value : item.FaCreate);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon1" + i, item.FACreateHorizon1 == null ? (object)DBNull.Value : item.FACreateHorizon1);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon2" + i, item.FACreateHorizon2 == null ? (object)DBNull.Value : item.FACreateHorizon2);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon3" + i, item.FACreateHorizon3 == null ? (object)DBNull.Value : item.FACreateHorizon3);
					sqlCommand.Parameters.AddWithValue("FaDatenEdit" + i, item.FaDatenEdit == null ? (object)DBNull.Value : item.FaDatenEdit);
					sqlCommand.Parameters.AddWithValue("FaDatenView" + i, item.FaDatenView == null ? (object)DBNull.Value : item.FaDatenView);
					sqlCommand.Parameters.AddWithValue("FaDelete" + i, item.FaDelete == null ? (object)DBNull.Value : item.FaDelete);
					sqlCommand.Parameters.AddWithValue("FADrucken" + i, item.FADrucken == null ? (object)DBNull.Value : item.FADrucken);
					sqlCommand.Parameters.AddWithValue("FaEdit" + i, item.FaEdit == null ? (object)DBNull.Value : item.FaEdit);
					sqlCommand.Parameters.AddWithValue("FAErlidegen" + i, item.FAErlidegen == null ? (object)DBNull.Value : item.FAErlidegen);
					sqlCommand.Parameters.AddWithValue("FAExcelUpdateWerk" + i, item.FAExcelUpdateWerk == null ? (object)DBNull.Value : item.FAExcelUpdateWerk);
					sqlCommand.Parameters.AddWithValue("FAExcelUpdateWunsh" + i, item.FAExcelUpdateWunsh == null ? (object)DBNull.Value : item.FAExcelUpdateWunsh);
					sqlCommand.Parameters.AddWithValue("FAFehlrMaterial" + i, item.FAFehlrMaterial == null ? (object)DBNull.Value : item.FAFehlrMaterial);
					sqlCommand.Parameters.AddWithValue("FaHomeAnalysis" + i, item.FaHomeAnalysis == null ? (object)DBNull.Value : item.FaHomeAnalysis);
					sqlCommand.Parameters.AddWithValue("FaHomeUpdate" + i, item.FaHomeUpdate == null ? (object)DBNull.Value : item.FaHomeUpdate);
					sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei" + i, item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
					sqlCommand.Parameters.AddWithValue("FaPlanningEdit" + i, item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
					sqlCommand.Parameters.AddWithValue("FaPlanningView" + i, item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
					sqlCommand.Parameters.AddWithValue("FAPlannung" + i, item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
					sqlCommand.Parameters.AddWithValue("FAPlannungTechnick" + i, item.FAPlannungTechnick == null ? (object)DBNull.Value : item.FAPlannungTechnick);
					sqlCommand.Parameters.AddWithValue("FAPriesZeitUpdate" + i, item.FAPriesZeitUpdate == null ? (object)DBNull.Value : item.FAPriesZeitUpdate);
					sqlCommand.Parameters.AddWithValue("FAProductionPlannung" + i, item.FAProductionPlannung == null ? (object)DBNull.Value : item.FAProductionPlannung);
					sqlCommand.Parameters.AddWithValue("FAStappleDruck" + i, item.FAStappleDruck == null ? (object)DBNull.Value : item.FAStappleDruck);
					sqlCommand.Parameters.AddWithValue("FAStatusAlbania" + i, item.FAStatusAlbania == null ? (object)DBNull.Value : item.FAStatusAlbania);
					sqlCommand.Parameters.AddWithValue("FAStatusCzech" + i, item.FAStatusCzech == null ? (object)DBNull.Value : item.FAStatusCzech);
					sqlCommand.Parameters.AddWithValue("FAStatusTunisia" + i, item.FAStatusTunisia == null ? (object)DBNull.Value : item.FAStatusTunisia);
					sqlCommand.Parameters.AddWithValue("FAStorno" + i, item.FAStorno == null ? (object)DBNull.Value : item.FAStorno);
					sqlCommand.Parameters.AddWithValue("FAStucklist" + i, item.FAStucklist == null ? (object)DBNull.Value : item.FAStucklist);
					sqlCommand.Parameters.AddWithValue("FaTechnicEdit" + i, item.FaTechnicEdit == null ? (object)DBNull.Value : item.FaTechnicEdit);
					sqlCommand.Parameters.AddWithValue("FaTechnicView" + i, item.FaTechnicView == null ? (object)DBNull.Value : item.FaTechnicView);
					sqlCommand.Parameters.AddWithValue("FATerminWerk" + i, item.FATerminWerk == null ? (object)DBNull.Value : item.FATerminWerk);
					sqlCommand.Parameters.AddWithValue("FAUpdateBemerkungExtern" + i, item.FAUpdateBemerkungExtern == null ? (object)DBNull.Value : item.FAUpdateBemerkungExtern);
					sqlCommand.Parameters.AddWithValue("FAUpdateByArticle" + i, item.FAUpdateByArticle == null ? (object)DBNull.Value : item.FAUpdateByArticle);
					sqlCommand.Parameters.AddWithValue("FAUpdateByFA" + i, item.FAUpdateByFA == null ? (object)DBNull.Value : item.FAUpdateByFA);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1" + i, item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2" + i, item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3" + i, item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
					sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin" + i, item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
					sqlCommand.Parameters.AddWithValue("Fertigung" + i, item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
					sqlCommand.Parameters.AddWithValue("FertigungLog" + i, item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
					sqlCommand.Parameters.AddWithValue("ForcastCreate" + i, item.ForcastCreate == null ? (object)DBNull.Value : item.ForcastCreate);
					sqlCommand.Parameters.AddWithValue("ForcastDelete" + i, item.ForcastDelete == null ? (object)DBNull.Value : item.ForcastDelete);
					sqlCommand.Parameters.AddWithValue("ForcastEdit" + i, item.ForcastEdit == null ? (object)DBNull.Value : item.ForcastEdit);
					sqlCommand.Parameters.AddWithValue("ForcastLog" + i, item.ForcastLog == null ? (object)DBNull.Value : item.ForcastLog);
					sqlCommand.Parameters.AddWithValue("ForcastPositionEdit" + i, item.ForcastPositionEdit == null ? (object)DBNull.Value : item.ForcastPositionEdit);
					sqlCommand.Parameters.AddWithValue("ForcastReport" + i, item.ForcastReport == null ? (object)DBNull.Value : item.ForcastReport);
					sqlCommand.Parameters.AddWithValue("ForcastView" + i, item.ForcastView == null ? (object)DBNull.Value : item.ForcastView);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon1" + i, item.FRCPosHorizon1 == null ? (object)DBNull.Value : item.FRCPosHorizon1);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon2" + i, item.FRCPosHorizon2 == null ? (object)DBNull.Value : item.FRCPosHorizon2);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon3" + i, item.FRCPosHorizon3 == null ? (object)DBNull.Value : item.FRCPosHorizon3);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon1" + i, item.GSPosHorizon1 == null ? (object)DBNull.Value : item.GSPosHorizon1);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon2" + i, item.GSPosHorizon2 == null ? (object)DBNull.Value : item.GSPosHorizon2);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon3" + i, item.GSPosHorizon3 == null ? (object)DBNull.Value : item.GSPosHorizon3);
					sqlCommand.Parameters.AddWithValue("GutschriftBookedEdit" + i, item.GutschriftBookedEdit == null ? (object)DBNull.Value : item.GutschriftBookedEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftCreate" + i, item.GutschriftCreate == null ? (object)DBNull.Value : item.GutschriftCreate);
					sqlCommand.Parameters.AddWithValue("GutschriftDelete" + i, item.GutschriftDelete == null ? (object)DBNull.Value : item.GutschriftDelete);
					sqlCommand.Parameters.AddWithValue("GutschriftDoneEdit" + i, item.GutschriftDoneEdit == null ? (object)DBNull.Value : item.GutschriftDoneEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftEdit" + i, item.GutschriftEdit == null ? (object)DBNull.Value : item.GutschriftEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftLog" + i, item.GutschriftLog == null ? (object)DBNull.Value : item.GutschriftLog);
					sqlCommand.Parameters.AddWithValue("GutschriftPositionEdit" + i, item.GutschriftPositionEdit == null ? (object)DBNull.Value : item.GutschriftPositionEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftReport" + i, item.GutschriftReport == null ? (object)DBNull.Value : item.GutschriftReport);
					sqlCommand.Parameters.AddWithValue("GutschriftView" + i, item.GutschriftView == null ? (object)DBNull.Value : item.GutschriftView);
					sqlCommand.Parameters.AddWithValue("InsideSalesChecks" + i, item.InsideSalesChecks == null ? (object)DBNull.Value : item.InsideSalesChecks);
					sqlCommand.Parameters.AddWithValue("InsideSalesChecksArchive" + i, item.InsideSalesChecksArchive == null ? (object)DBNull.Value : item.InsideSalesChecksArchive);
					sqlCommand.Parameters.AddWithValue("InsideSalesCustomerSummary" + i, item.InsideSalesCustomerSummary == null ? (object)DBNull.Value : item.InsideSalesCustomerSummary);
					sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluation" + i, item.InsideSalesMinimumStockEvaluation == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluation);
					sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluationTable" + i, item.InsideSalesMinimumStockEvaluationTable == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluationTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrders" + i, item.InsideSalesOverdueOrders == null ? (object)DBNull.Value : item.InsideSalesOverdueOrders);
					sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrdersTable" + i, item.InsideSalesOverdueOrdersTable == null ? (object)DBNull.Value : item.InsideSalesOverdueOrdersTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrders" + i, item.InsideSalesTotalUnbookedOrders == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrders);
					sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrdersTable" + i, item.InsideSalesTotalUnbookedOrdersTable == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrdersTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesTurnoverCurrentWeek" + i, item.InsideSalesTurnoverCurrentWeek == null ? (object)DBNull.Value : item.InsideSalesTurnoverCurrentWeek);
					sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon1" + i, item.LSPosHorizon1 == null ? (object)DBNull.Value : item.LSPosHorizon1);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon2" + i, item.LSPosHorizon2 == null ? (object)DBNull.Value : item.LSPosHorizon2);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon3" + i, item.LSPosHorizon3 == null ? (object)DBNull.Value : item.LSPosHorizon3);
					sqlCommand.Parameters.AddWithValue("mId" + i, item.mId == null ? (object)DBNull.Value : item.mId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("OrderProcessing" + i, item.OrderProcessing == null ? (object)DBNull.Value : item.OrderProcessing);
					sqlCommand.Parameters.AddWithValue("OrderProcessingLog" + i, item.OrderProcessingLog == null ? (object)DBNull.Value : item.OrderProcessingLog);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("RahmenAdd" + i, item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
					sqlCommand.Parameters.AddWithValue("RahmenAddAB" + i, item.RahmenAddAB == null ? (object)DBNull.Value : item.RahmenAddAB);
					sqlCommand.Parameters.AddWithValue("RahmenAddPositions" + i, item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
					sqlCommand.Parameters.AddWithValue("RahmenCancelation" + i, item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
					sqlCommand.Parameters.AddWithValue("RahmenClosure" + i, item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
					sqlCommand.Parameters.AddWithValue("RahmenDelete" + i, item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
					sqlCommand.Parameters.AddWithValue("RahmenDeletePositions" + i, item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
					sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow" + i, item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
					sqlCommand.Parameters.AddWithValue("RahmenEditHeader" + i, item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
					sqlCommand.Parameters.AddWithValue("RahmenEditPositions" + i, item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
					sqlCommand.Parameters.AddWithValue("RahmenHistory" + i, item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
					sqlCommand.Parameters.AddWithValue("RahmenValdation" + i, item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon1" + i, item.RAPosHorizon1 == null ? (object)DBNull.Value : item.RAPosHorizon1);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon2" + i, item.RAPosHorizon2 == null ? (object)DBNull.Value : item.RAPosHorizon2);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon3" + i, item.RAPosHorizon3 == null ? (object)DBNull.Value : item.RAPosHorizon3);
					sqlCommand.Parameters.AddWithValue("Rechnung" + i, item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
					sqlCommand.Parameters.AddWithValue("RechnungAutoCreation" + i, item.RechnungAutoCreation == null ? (object)DBNull.Value : item.RechnungAutoCreation);
					sqlCommand.Parameters.AddWithValue("RechnungBookedEdit" + i, item.RechnungBookedEdit == null ? (object)DBNull.Value : item.RechnungBookedEdit);
					sqlCommand.Parameters.AddWithValue("RechnungConfig" + i, item.RechnungConfig == null ? (object)DBNull.Value : item.RechnungConfig);
					sqlCommand.Parameters.AddWithValue("RechnungDelete" + i, item.RechnungDelete == null ? (object)DBNull.Value : item.RechnungDelete);
					sqlCommand.Parameters.AddWithValue("RechnungDoneEdit" + i, item.RechnungDoneEdit == null ? (object)DBNull.Value : item.RechnungDoneEdit);
					sqlCommand.Parameters.AddWithValue("RechnungManualCreation" + i, item.RechnungManualCreation == null ? (object)DBNull.Value : item.RechnungManualCreation);
					sqlCommand.Parameters.AddWithValue("RechnungReport" + i, item.RechnungReport == null ? (object)DBNull.Value : item.RechnungReport);
					sqlCommand.Parameters.AddWithValue("RechnungSend" + i, item.RechnungSend == null ? (object)DBNull.Value : item.RechnungSend);
					sqlCommand.Parameters.AddWithValue("RechnungValidate" + i, item.RechnungValidate == null ? (object)DBNull.Value : item.RechnungValidate);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon1" + i, item.RGPosHorizon1 == null ? (object)DBNull.Value : item.RGPosHorizon1);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon2" + i, item.RGPosHorizon2 == null ? (object)DBNull.Value : item.RGPosHorizon2);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon3" + i, item.RGPosHorizon3 == null ? (object)DBNull.Value : item.RGPosHorizon3);
					sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatsBacklogFGAdmin" + i, item.StatsBacklogFGAdmin == null ? (object)DBNull.Value : item.StatsBacklogFGAdmin);
					sqlCommand.Parameters.AddWithValue("StatsBacklogHWAdmin" + i, item.StatsBacklogHWAdmin == null ? (object)DBNull.Value : item.StatsBacklogHWAdmin);
					sqlCommand.Parameters.AddWithValue("StatsCapaCutting" + i, item.StatsCapaCutting == null ? (object)DBNull.Value : item.StatsCapaCutting);
					sqlCommand.Parameters.AddWithValue("StatsCapaHorizons" + i, item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
					sqlCommand.Parameters.AddWithValue("StatsCapaLong" + i, item.StatsCapaLong == null ? (object)DBNull.Value : item.StatsCapaLong);
					sqlCommand.Parameters.AddWithValue("StatsCapaPlanning" + i, item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
					sqlCommand.Parameters.AddWithValue("StatsCapaShort" + i, item.StatsCapaShort == null ? (object)DBNull.Value : item.StatsCapaShort);
					sqlCommand.Parameters.AddWithValue("StatsRechnungAL" + i, item.StatsRechnungAL == null ? (object)DBNull.Value : item.StatsRechnungAL);
					sqlCommand.Parameters.AddWithValue("StatsRechnungBETN" + i, item.StatsRechnungBETN == null ? (object)DBNull.Value : item.StatsRechnungBETN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungCZ" + i, item.StatsRechnungCZ == null ? (object)DBNull.Value : item.StatsRechnungCZ);
					sqlCommand.Parameters.AddWithValue("StatsRechnungDE" + i, item.StatsRechnungDE == null ? (object)DBNull.Value : item.StatsRechnungDE);
					sqlCommand.Parameters.AddWithValue("StatsRechnungGZTN" + i, item.StatsRechnungGZTN == null ? (object)DBNull.Value : item.StatsRechnungGZTN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungTN" + i, item.StatsRechnungTN == null ? (object)DBNull.Value : item.StatsRechnungTN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungWS" + i, item.StatsRechnungWS == null ? (object)DBNull.Value : item.StatsRechnungWS);
					sqlCommand.Parameters.AddWithValue("StatsStockCS" + i, item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
					sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse" + i, item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
					sqlCommand.Parameters.AddWithValue("StatsStockFG" + i, item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
					sqlCommand.Parameters.AddWithValue("UBGStatusChange" + i, item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__CTS_AccessProfile] SET [AB_LT]=@AB_LT, [AB_LT_EDI]=@AB_LT_EDI, [ABPosHorizon1]=@ABPosHorizon1, [ABPosHorizon2]=@ABPosHorizon2, [ABPosHorizon3]=@ABPosHorizon3, [AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [BVBookedEdit]=@BVBookedEdit, [BVDoneEdit]=@BVDoneEdit, [BVFaCreate]=@BVFaCreate, [Configuration]=@Configuration, [ConfigurationAppoitments]=@ConfigurationAppoitments, [ConfigurationChangeEmployees]=@ConfigurationChangeEmployees, [ConfigurationReplacements]=@ConfigurationReplacements, [ConfigurationReporting]=@ConfigurationReporting, [ConfirmationBookedEdit]=@ConfirmationBookedEdit, [ConfirmationCreate]=@ConfirmationCreate, [ConfirmationDelete]=@ConfirmationDelete, [ConfirmationDeliveryNote]=@ConfirmationDeliveryNote, [ConfirmationDoneEdit]=@ConfirmationDoneEdit, [ConfirmationEdit]=@ConfirmationEdit, [ConfirmationPositionEdit]=@ConfirmationPositionEdit, [ConfirmationPositionProduction]=@ConfirmationPositionProduction, [ConfirmationReport]=@ConfirmationReport, [ConfirmationValidate]=@ConfirmationValidate, [ConfirmationView]=@ConfirmationView, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CSInfoEdit]=@CSInfoEdit, [DelforCreate]=@DelforCreate, [DelforDelete]=@DelforDelete, [DelforDeletePosition]=@DelforDeletePosition, [DelforOrderConfirmation]=@DelforOrderConfirmation, [DelforReport]=@DelforReport, [DelforStatistics]=@DelforStatistics, [DelforView]=@DelforView, [DeliveryNoteBookedEdit]=@DeliveryNoteBookedEdit, [DeliveryNoteCreate]=@DeliveryNoteCreate, [DeliveryNoteDelete]=@DeliveryNoteDelete, [DeliveryNoteDoneEdit]=@DeliveryNoteDoneEdit, [DeliveryNoteEdit]=@DeliveryNoteEdit, [DeliveryNoteLog]=@DeliveryNoteLog, [DeliveryNotePositionEdit]=@DeliveryNotePositionEdit, [DeliveryNoteReport]=@DeliveryNoteReport, [DeliveryNoteView]=@DeliveryNoteView, [DLFPosHorizon1]=@DLFPosHorizon1, [DLFPosHorizon2]=@DLFPosHorizon2, [DLFPosHorizon3]=@DLFPosHorizon3, [EDI]=@EDI, [EDIDownloadFile]=@EDIDownloadFile, [EDIError]=@EDIError, [EDIErrorEdit]=@EDIErrorEdit, [EDIErrorValidated]=@EDIErrorValidated, [EDILogOrderValidated]=@EDILogOrderValidated, [EDIOrder]=@EDIOrder, [EDIOrderEdit]=@EDIOrderEdit, [EDIOrderPositionEdit]=@EDIOrderPositionEdit, [EDIOrderProduction]=@EDIOrderProduction, [EDIOrderProductionPosition]=@EDIOrderProductionPosition, [EDIOrderReport]=@EDIOrderReport, [EDIOrderValidated]=@EDIOrderValidated, [EDIOrderValidatedEdit]=@EDIOrderValidatedEdit, [FaABEdit]=@FaABEdit, [FaABView]=@FaABView, [FaActionBook]=@FaActionBook, [FaActionComplete]=@FaActionComplete, [FaActionDelete]=@FaActionDelete, [FaActionPrint]=@FaActionPrint, [FaAdmin]=@FaAdmin, [FAAKtualTerminUpdate]=@FAAKtualTerminUpdate, [FaAnalysis]=@FaAnalysis, [FAAuswertungEndkontrolle]=@FAAuswertungEndkontrolle, [FABemerkungPlannug]=@FABemerkungPlannug, [FABemerkungZuGewerk]=@FABemerkungZuGewerk, [FABemerkungZuPrio]=@FABemerkungZuPrio, [FACancelHorizon1]=@FACancelHorizon1, [FACancelHorizon2]=@FACancelHorizon2, [FACancelHorizon3]=@FACancelHorizon3, [FACommissionert]=@FACommissionert, [FaCreate]=@FaCreate, [FACreateHorizon1]=@FACreateHorizon1, [FACreateHorizon2]=@FACreateHorizon2, [FACreateHorizon3]=@FACreateHorizon3, [FaDatenEdit]=@FaDatenEdit, [FaDatenView]=@FaDatenView, [FaDelete]=@FaDelete, [FADrucken]=@FADrucken, [FaEdit]=@FaEdit, [FAErlidegen]=@FAErlidegen, [FAExcelUpdateWerk]=@FAExcelUpdateWerk, [FAExcelUpdateWunsh]=@FAExcelUpdateWunsh, [FAFehlrMaterial]=@FAFehlrMaterial, [FaHomeAnalysis]=@FaHomeAnalysis, [FaHomeUpdate]=@FaHomeUpdate, [FALaufkarteSchneiderei]=@FALaufkarteSchneiderei, [FaPlanningEdit]=@FaPlanningEdit, [FaPlanningView]=@FaPlanningView, [FAPlannung]=@FAPlannung, [FAPlannungTechnick]=@FAPlannungTechnick, [FAPriesZeitUpdate]=@FAPriesZeitUpdate, [FAProductionPlannung]=@FAProductionPlannung, [FAStappleDruck]=@FAStappleDruck, [FAStatusAlbania]=@FAStatusAlbania, [FAStatusCzech]=@FAStatusCzech, [FAStatusTunisia]=@FAStatusTunisia, [FAStorno]=@FAStorno, [FAStucklist]=@FAStucklist, [FaTechnicEdit]=@FaTechnicEdit, [FaTechnicView]=@FaTechnicView, [FATerminWerk]=@FATerminWerk, [FAUpdateBemerkungExtern]=@FAUpdateBemerkungExtern, [FAUpdateByArticle]=@FAUpdateByArticle, [FAUpdateByFA]=@FAUpdateByFA, [FAUpdateTerminHorizon1]=@FAUpdateTerminHorizon1, [FAUpdateTerminHorizon2]=@FAUpdateTerminHorizon2, [FAUpdateTerminHorizon3]=@FAUpdateTerminHorizon3, [FAWerkWunshAdmin]=@FAWerkWunshAdmin, [Fertigung]=@Fertigung, [FertigungLog]=@FertigungLog, [ForcastCreate]=@ForcastCreate, [ForcastDelete]=@ForcastDelete, [ForcastEdit]=@ForcastEdit, [ForcastLog]=@ForcastLog, [ForcastPositionEdit]=@ForcastPositionEdit, [ForcastReport]=@ForcastReport, [ForcastView]=@ForcastView, [FRCPosHorizon1]=@FRCPosHorizon1, [FRCPosHorizon2]=@FRCPosHorizon2, [FRCPosHorizon3]=@FRCPosHorizon3, [GSPosHorizon1]=@GSPosHorizon1, [GSPosHorizon2]=@GSPosHorizon2, [GSPosHorizon3]=@GSPosHorizon3, [GutschriftBookedEdit]=@GutschriftBookedEdit, [GutschriftCreate]=@GutschriftCreate, [GutschriftDelete]=@GutschriftDelete, [GutschriftDoneEdit]=@GutschriftDoneEdit, [GutschriftEdit]=@GutschriftEdit, [GutschriftLog]=@GutschriftLog, [GutschriftPositionEdit]=@GutschriftPositionEdit, [GutschriftReport]=@GutschriftReport, [GutschriftView]=@GutschriftView, [InsideSalesChecks]=@InsideSalesChecks, [InsideSalesChecksArchive]=@InsideSalesChecksArchive, [InsideSalesCustomerSummary]=@InsideSalesCustomerSummary, [InsideSalesMinimumStockEvaluation]=@InsideSalesMinimumStockEvaluation, [InsideSalesMinimumStockEvaluationTable]=@InsideSalesMinimumStockEvaluationTable, [InsideSalesOverdueOrders]=@InsideSalesOverdueOrders, [InsideSalesOverdueOrdersTable]=@InsideSalesOverdueOrdersTable, [InsideSalesTotalUnbookedOrders]=@InsideSalesTotalUnbookedOrders, [InsideSalesTotalUnbookedOrdersTable]=@InsideSalesTotalUnbookedOrdersTable, [InsideSalesTurnoverCurrentWeek]=@InsideSalesTurnoverCurrentWeek, [IsDefault]=@IsDefault, [LSPosHorizon1]=@LSPosHorizon1, [LSPosHorizon2]=@LSPosHorizon2, [LSPosHorizon3]=@LSPosHorizon3, [mId]=@mId, [ModuleActivated]=@ModuleActivated, [OrderProcessing]=@OrderProcessing, [OrderProcessingLog]=@OrderProcessingLog, [Rahmen]=@Rahmen, [RahmenAdd]=@RahmenAdd, [RahmenAddAB]=@RahmenAddAB, [RahmenAddPositions]=@RahmenAddPositions, [RahmenCancelation]=@RahmenCancelation, [RahmenClosure]=@RahmenClosure, [RahmenDelete]=@RahmenDelete, [RahmenDeletePositions]=@RahmenDeletePositions, [RahmenDocumentFlow]=@RahmenDocumentFlow, [RahmenEditHeader]=@RahmenEditHeader, [RahmenEditPositions]=@RahmenEditPositions, [RahmenHistory]=@RahmenHistory, [RahmenValdation]=@RahmenValdation, [RAPosHorizon1]=@RAPosHorizon1, [RAPosHorizon2]=@RAPosHorizon2, [RAPosHorizon3]=@RAPosHorizon3, [Rechnung]=@Rechnung, [RechnungAutoCreation]=@RechnungAutoCreation, [RechnungBookedEdit]=@RechnungBookedEdit, [RechnungConfig]=@RechnungConfig, [RechnungDelete]=@RechnungDelete, [RechnungDoneEdit]=@RechnungDoneEdit, [RechnungManualCreation]=@RechnungManualCreation, [RechnungReport]=@RechnungReport, [RechnungSend]=@RechnungSend, [RechnungValidate]=@RechnungValidate, [RGPosHorizon1]=@RGPosHorizon1, [RGPosHorizon2]=@RGPosHorizon2, [RGPosHorizon3]=@RGPosHorizon3, [Statistics]=@Statistics, [StatsBacklogFGAdmin]=@StatsBacklogFGAdmin, [StatsBacklogHWAdmin]=@StatsBacklogHWAdmin, [StatsCapaCutting]=@StatsCapaCutting, [StatsCapaHorizons]=@StatsCapaHorizons, [StatsCapaLong]=@StatsCapaLong, [StatsCapaPlanning]=@StatsCapaPlanning, [StatsCapaShort]=@StatsCapaShort, [StatsRechnungAL]=@StatsRechnungAL, [StatsRechnungBETN]=@StatsRechnungBETN, [StatsRechnungCZ]=@StatsRechnungCZ, [StatsRechnungDE]=@StatsRechnungDE, [StatsRechnungGZTN]=@StatsRechnungGZTN, [StatsRechnungTN]=@StatsRechnungTN, [StatsRechnungWS]=@StatsRechnungWS, [StatsStockCS]=@StatsStockCS, [StatsStockExternalWarehouse]=@StatsStockExternalWarehouse, [StatsStockFG]=@StatsStockFG, [UBGStatusChange]=@UBGStatusChange WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AB_LT", item.AB_LT == null ? (object)DBNull.Value : item.AB_LT);
			sqlCommand.Parameters.AddWithValue("AB_LT_EDI", item.AB_LT_EDI == null ? (object)DBNull.Value : item.AB_LT_EDI);
			sqlCommand.Parameters.AddWithValue("ABPosHorizon1", item.ABPosHorizon1 == null ? (object)DBNull.Value : item.ABPosHorizon1);
			sqlCommand.Parameters.AddWithValue("ABPosHorizon2", item.ABPosHorizon2 == null ? (object)DBNull.Value : item.ABPosHorizon2);
			sqlCommand.Parameters.AddWithValue("ABPosHorizon3", item.ABPosHorizon3 == null ? (object)DBNull.Value : item.ABPosHorizon3);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
			sqlCommand.Parameters.AddWithValue("BVBookedEdit", item.BVBookedEdit == null ? (object)DBNull.Value : item.BVBookedEdit);
			sqlCommand.Parameters.AddWithValue("BVDoneEdit", item.BVDoneEdit == null ? (object)DBNull.Value : item.BVDoneEdit);
			sqlCommand.Parameters.AddWithValue("BVFaCreate", item.BVFaCreate == null ? (object)DBNull.Value : item.BVFaCreate);
			sqlCommand.Parameters.AddWithValue("Configuration", item.Configuration == null ? (object)DBNull.Value : item.Configuration);
			sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments", item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
			sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees", item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
			sqlCommand.Parameters.AddWithValue("ConfigurationReplacements", item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
			sqlCommand.Parameters.AddWithValue("ConfigurationReporting", item.ConfigurationReporting == null ? (object)DBNull.Value : item.ConfigurationReporting);
			sqlCommand.Parameters.AddWithValue("ConfirmationBookedEdit", item.ConfirmationBookedEdit == null ? (object)DBNull.Value : item.ConfirmationBookedEdit);
			sqlCommand.Parameters.AddWithValue("ConfirmationCreate", item.ConfirmationCreate == null ? (object)DBNull.Value : item.ConfirmationCreate);
			sqlCommand.Parameters.AddWithValue("ConfirmationDelete", item.ConfirmationDelete == null ? (object)DBNull.Value : item.ConfirmationDelete);
			sqlCommand.Parameters.AddWithValue("ConfirmationDeliveryNote", item.ConfirmationDeliveryNote == null ? (object)DBNull.Value : item.ConfirmationDeliveryNote);
			sqlCommand.Parameters.AddWithValue("ConfirmationDoneEdit", item.ConfirmationDoneEdit == null ? (object)DBNull.Value : item.ConfirmationDoneEdit);
			sqlCommand.Parameters.AddWithValue("ConfirmationEdit", item.ConfirmationEdit == null ? (object)DBNull.Value : item.ConfirmationEdit);
			sqlCommand.Parameters.AddWithValue("ConfirmationPositionEdit", item.ConfirmationPositionEdit == null ? (object)DBNull.Value : item.ConfirmationPositionEdit);
			sqlCommand.Parameters.AddWithValue("ConfirmationPositionProduction", item.ConfirmationPositionProduction == null ? (object)DBNull.Value : item.ConfirmationPositionProduction);
			sqlCommand.Parameters.AddWithValue("ConfirmationReport", item.ConfirmationReport == null ? (object)DBNull.Value : item.ConfirmationReport);
			sqlCommand.Parameters.AddWithValue("ConfirmationValidate", item.ConfirmationValidate == null ? (object)DBNull.Value : item.ConfirmationValidate);
			sqlCommand.Parameters.AddWithValue("ConfirmationView", item.ConfirmationView == null ? (object)DBNull.Value : item.ConfirmationView);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CSInfoEdit", item.CSInfoEdit == null ? (object)DBNull.Value : item.CSInfoEdit);
			sqlCommand.Parameters.AddWithValue("DelforCreate", item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
			sqlCommand.Parameters.AddWithValue("DelforDelete", item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
			sqlCommand.Parameters.AddWithValue("DelforDeletePosition", item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
			sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation", item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
			sqlCommand.Parameters.AddWithValue("DelforReport", item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
			sqlCommand.Parameters.AddWithValue("DelforStatistics", item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
			sqlCommand.Parameters.AddWithValue("DelforView", item.DelforView == null ? (object)DBNull.Value : item.DelforView);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteBookedEdit", item.DeliveryNoteBookedEdit == null ? (object)DBNull.Value : item.DeliveryNoteBookedEdit);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteCreate", item.DeliveryNoteCreate == null ? (object)DBNull.Value : item.DeliveryNoteCreate);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteDelete", item.DeliveryNoteDelete == null ? (object)DBNull.Value : item.DeliveryNoteDelete);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteDoneEdit", item.DeliveryNoteDoneEdit == null ? (object)DBNull.Value : item.DeliveryNoteDoneEdit);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteEdit", item.DeliveryNoteEdit == null ? (object)DBNull.Value : item.DeliveryNoteEdit);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteLog", item.DeliveryNoteLog == null ? (object)DBNull.Value : item.DeliveryNoteLog);
			sqlCommand.Parameters.AddWithValue("DeliveryNotePositionEdit", item.DeliveryNotePositionEdit == null ? (object)DBNull.Value : item.DeliveryNotePositionEdit);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteReport", item.DeliveryNoteReport == null ? (object)DBNull.Value : item.DeliveryNoteReport);
			sqlCommand.Parameters.AddWithValue("DeliveryNoteView", item.DeliveryNoteView == null ? (object)DBNull.Value : item.DeliveryNoteView);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon1", item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon2", item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon3", item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
			sqlCommand.Parameters.AddWithValue("EDI", item.EDI == null ? (object)DBNull.Value : item.EDI);
			sqlCommand.Parameters.AddWithValue("EDIDownloadFile", item.EDIDownloadFile == null ? (object)DBNull.Value : item.EDIDownloadFile);
			sqlCommand.Parameters.AddWithValue("EDIError", item.EDIError == null ? (object)DBNull.Value : item.EDIError);
			sqlCommand.Parameters.AddWithValue("EDIErrorEdit", item.EDIErrorEdit == null ? (object)DBNull.Value : item.EDIErrorEdit);
			sqlCommand.Parameters.AddWithValue("EDIErrorValidated", item.EDIErrorValidated == null ? (object)DBNull.Value : item.EDIErrorValidated);
			sqlCommand.Parameters.AddWithValue("EDILogOrderValidated", item.EDILogOrderValidated == null ? (object)DBNull.Value : item.EDILogOrderValidated);
			sqlCommand.Parameters.AddWithValue("EDIOrder", item.EDIOrder == null ? (object)DBNull.Value : item.EDIOrder);
			sqlCommand.Parameters.AddWithValue("EDIOrderEdit", item.EDIOrderEdit == null ? (object)DBNull.Value : item.EDIOrderEdit);
			sqlCommand.Parameters.AddWithValue("EDIOrderPositionEdit", item.EDIOrderPositionEdit == null ? (object)DBNull.Value : item.EDIOrderPositionEdit);
			sqlCommand.Parameters.AddWithValue("EDIOrderProduction", item.EDIOrderProduction == null ? (object)DBNull.Value : item.EDIOrderProduction);
			sqlCommand.Parameters.AddWithValue("EDIOrderProductionPosition", item.EDIOrderProductionPosition == null ? (object)DBNull.Value : item.EDIOrderProductionPosition);
			sqlCommand.Parameters.AddWithValue("EDIOrderReport", item.EDIOrderReport == null ? (object)DBNull.Value : item.EDIOrderReport);
			sqlCommand.Parameters.AddWithValue("EDIOrderValidated", item.EDIOrderValidated == null ? (object)DBNull.Value : item.EDIOrderValidated);
			sqlCommand.Parameters.AddWithValue("EDIOrderValidatedEdit", item.EDIOrderValidatedEdit == null ? (object)DBNull.Value : item.EDIOrderValidatedEdit);
			sqlCommand.Parameters.AddWithValue("FaABEdit", item.FaABEdit == null ? (object)DBNull.Value : item.FaABEdit);
			sqlCommand.Parameters.AddWithValue("FaABView", item.FaABView == null ? (object)DBNull.Value : item.FaABView);
			sqlCommand.Parameters.AddWithValue("FaActionBook", item.FaActionBook == null ? (object)DBNull.Value : item.FaActionBook);
			sqlCommand.Parameters.AddWithValue("FaActionComplete", item.FaActionComplete == null ? (object)DBNull.Value : item.FaActionComplete);
			sqlCommand.Parameters.AddWithValue("FaActionDelete", item.FaActionDelete == null ? (object)DBNull.Value : item.FaActionDelete);
			sqlCommand.Parameters.AddWithValue("FaActionPrint", item.FaActionPrint == null ? (object)DBNull.Value : item.FaActionPrint);
			sqlCommand.Parameters.AddWithValue("FaAdmin", item.FaAdmin == null ? (object)DBNull.Value : item.FaAdmin);
			sqlCommand.Parameters.AddWithValue("FAAKtualTerminUpdate", item.FAAKtualTerminUpdate == null ? (object)DBNull.Value : item.FAAKtualTerminUpdate);
			sqlCommand.Parameters.AddWithValue("FaAnalysis", item.FaAnalysis == null ? (object)DBNull.Value : item.FaAnalysis);
			sqlCommand.Parameters.AddWithValue("FAAuswertungEndkontrolle", item.FAAuswertungEndkontrolle == null ? (object)DBNull.Value : item.FAAuswertungEndkontrolle);
			sqlCommand.Parameters.AddWithValue("FABemerkungPlannug", item.FABemerkungPlannug == null ? (object)DBNull.Value : item.FABemerkungPlannug);
			sqlCommand.Parameters.AddWithValue("FABemerkungZuGewerk", item.FABemerkungZuGewerk == null ? (object)DBNull.Value : item.FABemerkungZuGewerk);
			sqlCommand.Parameters.AddWithValue("FABemerkungZuPrio", item.FABemerkungZuPrio == null ? (object)DBNull.Value : item.FABemerkungZuPrio);
			sqlCommand.Parameters.AddWithValue("FACancelHorizon1", item.FACancelHorizon1 == null ? (object)DBNull.Value : item.FACancelHorizon1);
			sqlCommand.Parameters.AddWithValue("FACancelHorizon2", item.FACancelHorizon2 == null ? (object)DBNull.Value : item.FACancelHorizon2);
			sqlCommand.Parameters.AddWithValue("FACancelHorizon3", item.FACancelHorizon3 == null ? (object)DBNull.Value : item.FACancelHorizon3);
			sqlCommand.Parameters.AddWithValue("FACommissionert", item.FACommissionert == null ? (object)DBNull.Value : item.FACommissionert);
			sqlCommand.Parameters.AddWithValue("FaCreate", item.FaCreate == null ? (object)DBNull.Value : item.FaCreate);
			sqlCommand.Parameters.AddWithValue("FACreateHorizon1", item.FACreateHorizon1 == null ? (object)DBNull.Value : item.FACreateHorizon1);
			sqlCommand.Parameters.AddWithValue("FACreateHorizon2", item.FACreateHorizon2 == null ? (object)DBNull.Value : item.FACreateHorizon2);
			sqlCommand.Parameters.AddWithValue("FACreateHorizon3", item.FACreateHorizon3 == null ? (object)DBNull.Value : item.FACreateHorizon3);
			sqlCommand.Parameters.AddWithValue("FaDatenEdit", item.FaDatenEdit == null ? (object)DBNull.Value : item.FaDatenEdit);
			sqlCommand.Parameters.AddWithValue("FaDatenView", item.FaDatenView == null ? (object)DBNull.Value : item.FaDatenView);
			sqlCommand.Parameters.AddWithValue("FaDelete", item.FaDelete == null ? (object)DBNull.Value : item.FaDelete);
			sqlCommand.Parameters.AddWithValue("FADrucken", item.FADrucken == null ? (object)DBNull.Value : item.FADrucken);
			sqlCommand.Parameters.AddWithValue("FaEdit", item.FaEdit == null ? (object)DBNull.Value : item.FaEdit);
			sqlCommand.Parameters.AddWithValue("FAErlidegen", item.FAErlidegen == null ? (object)DBNull.Value : item.FAErlidegen);
			sqlCommand.Parameters.AddWithValue("FAExcelUpdateWerk", item.FAExcelUpdateWerk == null ? (object)DBNull.Value : item.FAExcelUpdateWerk);
			sqlCommand.Parameters.AddWithValue("FAExcelUpdateWunsh", item.FAExcelUpdateWunsh == null ? (object)DBNull.Value : item.FAExcelUpdateWunsh);
			sqlCommand.Parameters.AddWithValue("FAFehlrMaterial", item.FAFehlrMaterial == null ? (object)DBNull.Value : item.FAFehlrMaterial);
			sqlCommand.Parameters.AddWithValue("FaHomeAnalysis", item.FaHomeAnalysis == null ? (object)DBNull.Value : item.FaHomeAnalysis);
			sqlCommand.Parameters.AddWithValue("FaHomeUpdate", item.FaHomeUpdate == null ? (object)DBNull.Value : item.FaHomeUpdate);
			sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei", item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
			sqlCommand.Parameters.AddWithValue("FaPlanningEdit", item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
			sqlCommand.Parameters.AddWithValue("FaPlanningView", item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
			sqlCommand.Parameters.AddWithValue("FAPlannung", item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
			sqlCommand.Parameters.AddWithValue("FAPlannungTechnick", item.FAPlannungTechnick == null ? (object)DBNull.Value : item.FAPlannungTechnick);
			sqlCommand.Parameters.AddWithValue("FAPriesZeitUpdate", item.FAPriesZeitUpdate == null ? (object)DBNull.Value : item.FAPriesZeitUpdate);
			sqlCommand.Parameters.AddWithValue("FAProductionPlannung", item.FAProductionPlannung == null ? (object)DBNull.Value : item.FAProductionPlannung);
			sqlCommand.Parameters.AddWithValue("FAStappleDruck", item.FAStappleDruck == null ? (object)DBNull.Value : item.FAStappleDruck);
			sqlCommand.Parameters.AddWithValue("FAStatusAlbania", item.FAStatusAlbania == null ? (object)DBNull.Value : item.FAStatusAlbania);
			sqlCommand.Parameters.AddWithValue("FAStatusCzech", item.FAStatusCzech == null ? (object)DBNull.Value : item.FAStatusCzech);
			sqlCommand.Parameters.AddWithValue("FAStatusTunisia", item.FAStatusTunisia == null ? (object)DBNull.Value : item.FAStatusTunisia);
			sqlCommand.Parameters.AddWithValue("FAStorno", item.FAStorno == null ? (object)DBNull.Value : item.FAStorno);
			sqlCommand.Parameters.AddWithValue("FAStucklist", item.FAStucklist == null ? (object)DBNull.Value : item.FAStucklist);
			sqlCommand.Parameters.AddWithValue("FaTechnicEdit", item.FaTechnicEdit == null ? (object)DBNull.Value : item.FaTechnicEdit);
			sqlCommand.Parameters.AddWithValue("FaTechnicView", item.FaTechnicView == null ? (object)DBNull.Value : item.FaTechnicView);
			sqlCommand.Parameters.AddWithValue("FATerminWerk", item.FATerminWerk == null ? (object)DBNull.Value : item.FATerminWerk);
			sqlCommand.Parameters.AddWithValue("FAUpdateBemerkungExtern", item.FAUpdateBemerkungExtern == null ? (object)DBNull.Value : item.FAUpdateBemerkungExtern);
			sqlCommand.Parameters.AddWithValue("FAUpdateByArticle", item.FAUpdateByArticle == null ? (object)DBNull.Value : item.FAUpdateByArticle);
			sqlCommand.Parameters.AddWithValue("FAUpdateByFA", item.FAUpdateByFA == null ? (object)DBNull.Value : item.FAUpdateByFA);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1", item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2", item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3", item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
			sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin", item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
			sqlCommand.Parameters.AddWithValue("Fertigung", item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
			sqlCommand.Parameters.AddWithValue("FertigungLog", item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
			sqlCommand.Parameters.AddWithValue("ForcastCreate", item.ForcastCreate == null ? (object)DBNull.Value : item.ForcastCreate);
			sqlCommand.Parameters.AddWithValue("ForcastDelete", item.ForcastDelete == null ? (object)DBNull.Value : item.ForcastDelete);
			sqlCommand.Parameters.AddWithValue("ForcastEdit", item.ForcastEdit == null ? (object)DBNull.Value : item.ForcastEdit);
			sqlCommand.Parameters.AddWithValue("ForcastLog", item.ForcastLog == null ? (object)DBNull.Value : item.ForcastLog);
			sqlCommand.Parameters.AddWithValue("ForcastPositionEdit", item.ForcastPositionEdit == null ? (object)DBNull.Value : item.ForcastPositionEdit);
			sqlCommand.Parameters.AddWithValue("ForcastReport", item.ForcastReport == null ? (object)DBNull.Value : item.ForcastReport);
			sqlCommand.Parameters.AddWithValue("ForcastView", item.ForcastView == null ? (object)DBNull.Value : item.ForcastView);
			sqlCommand.Parameters.AddWithValue("FRCPosHorizon1", item.FRCPosHorizon1 == null ? (object)DBNull.Value : item.FRCPosHorizon1);
			sqlCommand.Parameters.AddWithValue("FRCPosHorizon2", item.FRCPosHorizon2 == null ? (object)DBNull.Value : item.FRCPosHorizon2);
			sqlCommand.Parameters.AddWithValue("FRCPosHorizon3", item.FRCPosHorizon3 == null ? (object)DBNull.Value : item.FRCPosHorizon3);
			sqlCommand.Parameters.AddWithValue("GSPosHorizon1", item.GSPosHorizon1 == null ? (object)DBNull.Value : item.GSPosHorizon1);
			sqlCommand.Parameters.AddWithValue("GSPosHorizon2", item.GSPosHorizon2 == null ? (object)DBNull.Value : item.GSPosHorizon2);
			sqlCommand.Parameters.AddWithValue("GSPosHorizon3", item.GSPosHorizon3 == null ? (object)DBNull.Value : item.GSPosHorizon3);
			sqlCommand.Parameters.AddWithValue("GutschriftBookedEdit", item.GutschriftBookedEdit == null ? (object)DBNull.Value : item.GutschriftBookedEdit);
			sqlCommand.Parameters.AddWithValue("GutschriftCreate", item.GutschriftCreate == null ? (object)DBNull.Value : item.GutschriftCreate);
			sqlCommand.Parameters.AddWithValue("GutschriftDelete", item.GutschriftDelete == null ? (object)DBNull.Value : item.GutschriftDelete);
			sqlCommand.Parameters.AddWithValue("GutschriftDoneEdit", item.GutschriftDoneEdit == null ? (object)DBNull.Value : item.GutschriftDoneEdit);
			sqlCommand.Parameters.AddWithValue("GutschriftEdit", item.GutschriftEdit == null ? (object)DBNull.Value : item.GutschriftEdit);
			sqlCommand.Parameters.AddWithValue("GutschriftLog", item.GutschriftLog == null ? (object)DBNull.Value : item.GutschriftLog);
			sqlCommand.Parameters.AddWithValue("GutschriftPositionEdit", item.GutschriftPositionEdit == null ? (object)DBNull.Value : item.GutschriftPositionEdit);
			sqlCommand.Parameters.AddWithValue("GutschriftReport", item.GutschriftReport == null ? (object)DBNull.Value : item.GutschriftReport);
			sqlCommand.Parameters.AddWithValue("GutschriftView", item.GutschriftView == null ? (object)DBNull.Value : item.GutschriftView);
			sqlCommand.Parameters.AddWithValue("InsideSalesChecks", item.InsideSalesChecks == null ? (object)DBNull.Value : item.InsideSalesChecks);
			sqlCommand.Parameters.AddWithValue("InsideSalesChecksArchive", item.InsideSalesChecksArchive == null ? (object)DBNull.Value : item.InsideSalesChecksArchive);
			sqlCommand.Parameters.AddWithValue("InsideSalesCustomerSummary", item.InsideSalesCustomerSummary == null ? (object)DBNull.Value : item.InsideSalesCustomerSummary);
			sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluation", item.InsideSalesMinimumStockEvaluation == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluation);
			sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluationTable", item.InsideSalesMinimumStockEvaluationTable == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluationTable);
			sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrders", item.InsideSalesOverdueOrders == null ? (object)DBNull.Value : item.InsideSalesOverdueOrders);
			sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrdersTable", item.InsideSalesOverdueOrdersTable == null ? (object)DBNull.Value : item.InsideSalesOverdueOrdersTable);
			sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrders", item.InsideSalesTotalUnbookedOrders == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrders);
			sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrdersTable", item.InsideSalesTotalUnbookedOrdersTable == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrdersTable);
			sqlCommand.Parameters.AddWithValue("InsideSalesTurnoverCurrentWeek", item.InsideSalesTurnoverCurrentWeek == null ? (object)DBNull.Value : item.InsideSalesTurnoverCurrentWeek);
			sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
			sqlCommand.Parameters.AddWithValue("LSPosHorizon1", item.LSPosHorizon1 == null ? (object)DBNull.Value : item.LSPosHorizon1);
			sqlCommand.Parameters.AddWithValue("LSPosHorizon2", item.LSPosHorizon2 == null ? (object)DBNull.Value : item.LSPosHorizon2);
			sqlCommand.Parameters.AddWithValue("LSPosHorizon3", item.LSPosHorizon3 == null ? (object)DBNull.Value : item.LSPosHorizon3);
			sqlCommand.Parameters.AddWithValue("mId", item.mId == null ? (object)DBNull.Value : item.mId);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("OrderProcessing", item.OrderProcessing == null ? (object)DBNull.Value : item.OrderProcessing);
			sqlCommand.Parameters.AddWithValue("OrderProcessingLog", item.OrderProcessingLog == null ? (object)DBNull.Value : item.OrderProcessingLog);
			sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
			sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
			sqlCommand.Parameters.AddWithValue("RahmenAddAB", item.RahmenAddAB == null ? (object)DBNull.Value : item.RahmenAddAB);
			sqlCommand.Parameters.AddWithValue("RahmenAddPositions", item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
			sqlCommand.Parameters.AddWithValue("RahmenCancelation", item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
			sqlCommand.Parameters.AddWithValue("RahmenClosure", item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
			sqlCommand.Parameters.AddWithValue("RahmenDelete", item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
			sqlCommand.Parameters.AddWithValue("RahmenDeletePositions", item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
			sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow", item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
			sqlCommand.Parameters.AddWithValue("RahmenEditHeader", item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
			sqlCommand.Parameters.AddWithValue("RahmenEditPositions", item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
			sqlCommand.Parameters.AddWithValue("RahmenHistory", item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
			sqlCommand.Parameters.AddWithValue("RahmenValdation", item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
			sqlCommand.Parameters.AddWithValue("RAPosHorizon1", item.RAPosHorizon1 == null ? (object)DBNull.Value : item.RAPosHorizon1);
			sqlCommand.Parameters.AddWithValue("RAPosHorizon2", item.RAPosHorizon2 == null ? (object)DBNull.Value : item.RAPosHorizon2);
			sqlCommand.Parameters.AddWithValue("RAPosHorizon3", item.RAPosHorizon3 == null ? (object)DBNull.Value : item.RAPosHorizon3);
			sqlCommand.Parameters.AddWithValue("Rechnung", item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
			sqlCommand.Parameters.AddWithValue("RechnungAutoCreation", item.RechnungAutoCreation == null ? (object)DBNull.Value : item.RechnungAutoCreation);
			sqlCommand.Parameters.AddWithValue("RechnungBookedEdit", item.RechnungBookedEdit == null ? (object)DBNull.Value : item.RechnungBookedEdit);
			sqlCommand.Parameters.AddWithValue("RechnungConfig", item.RechnungConfig == null ? (object)DBNull.Value : item.RechnungConfig);
			sqlCommand.Parameters.AddWithValue("RechnungDelete", item.RechnungDelete == null ? (object)DBNull.Value : item.RechnungDelete);
			sqlCommand.Parameters.AddWithValue("RechnungDoneEdit", item.RechnungDoneEdit == null ? (object)DBNull.Value : item.RechnungDoneEdit);
			sqlCommand.Parameters.AddWithValue("RechnungManualCreation", item.RechnungManualCreation == null ? (object)DBNull.Value : item.RechnungManualCreation);
			sqlCommand.Parameters.AddWithValue("RechnungReport", item.RechnungReport == null ? (object)DBNull.Value : item.RechnungReport);
			sqlCommand.Parameters.AddWithValue("RechnungSend", item.RechnungSend == null ? (object)DBNull.Value : item.RechnungSend);
			sqlCommand.Parameters.AddWithValue("RechnungValidate", item.RechnungValidate == null ? (object)DBNull.Value : item.RechnungValidate);
			sqlCommand.Parameters.AddWithValue("RGPosHorizon1", item.RGPosHorizon1 == null ? (object)DBNull.Value : item.RGPosHorizon1);
			sqlCommand.Parameters.AddWithValue("RGPosHorizon2", item.RGPosHorizon2 == null ? (object)DBNull.Value : item.RGPosHorizon2);
			sqlCommand.Parameters.AddWithValue("RGPosHorizon3", item.RGPosHorizon3 == null ? (object)DBNull.Value : item.RGPosHorizon3);
			sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
			sqlCommand.Parameters.AddWithValue("StatsBacklogFGAdmin", item.StatsBacklogFGAdmin == null ? (object)DBNull.Value : item.StatsBacklogFGAdmin);
			sqlCommand.Parameters.AddWithValue("StatsBacklogHWAdmin", item.StatsBacklogHWAdmin == null ? (object)DBNull.Value : item.StatsBacklogHWAdmin);
			sqlCommand.Parameters.AddWithValue("StatsCapaCutting", item.StatsCapaCutting == null ? (object)DBNull.Value : item.StatsCapaCutting);
			sqlCommand.Parameters.AddWithValue("StatsCapaHorizons", item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
			sqlCommand.Parameters.AddWithValue("StatsCapaLong", item.StatsCapaLong == null ? (object)DBNull.Value : item.StatsCapaLong);
			sqlCommand.Parameters.AddWithValue("StatsCapaPlanning", item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
			sqlCommand.Parameters.AddWithValue("StatsCapaShort", item.StatsCapaShort == null ? (object)DBNull.Value : item.StatsCapaShort);
			sqlCommand.Parameters.AddWithValue("StatsRechnungAL", item.StatsRechnungAL == null ? (object)DBNull.Value : item.StatsRechnungAL);
			sqlCommand.Parameters.AddWithValue("StatsRechnungBETN", item.StatsRechnungBETN == null ? (object)DBNull.Value : item.StatsRechnungBETN);
			sqlCommand.Parameters.AddWithValue("StatsRechnungCZ", item.StatsRechnungCZ == null ? (object)DBNull.Value : item.StatsRechnungCZ);
			sqlCommand.Parameters.AddWithValue("StatsRechnungDE", item.StatsRechnungDE == null ? (object)DBNull.Value : item.StatsRechnungDE);
			sqlCommand.Parameters.AddWithValue("StatsRechnungGZTN", item.StatsRechnungGZTN == null ? (object)DBNull.Value : item.StatsRechnungGZTN);
			sqlCommand.Parameters.AddWithValue("StatsRechnungTN", item.StatsRechnungTN == null ? (object)DBNull.Value : item.StatsRechnungTN);
			sqlCommand.Parameters.AddWithValue("StatsRechnungWS", item.StatsRechnungWS == null ? (object)DBNull.Value : item.StatsRechnungWS);
			sqlCommand.Parameters.AddWithValue("StatsStockCS", item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
			sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse", item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
			sqlCommand.Parameters.AddWithValue("StatsStockFG", item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
			sqlCommand.Parameters.AddWithValue("UBGStatusChange", item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 208; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [__CTS_AccessProfile] SET "

					+ "[AB_LT]=@AB_LT" + i + ","
					+ "[AB_LT_EDI]=@AB_LT_EDI" + i + ","
					+ "[ABPosHorizon1]=@ABPosHorizon1" + i + ","
					+ "[ABPosHorizon2]=@ABPosHorizon2" + i + ","
					+ "[ABPosHorizon3]=@ABPosHorizon3" + i + ","
					+ "[AccessProfileName]=@AccessProfileName" + i + ","
					+ "[Administration]=@Administration" + i + ","
					+ "[BVBookedEdit]=@BVBookedEdit" + i + ","
					+ "[BVDoneEdit]=@BVDoneEdit" + i + ","
					+ "[BVFaCreate]=@BVFaCreate" + i + ","
					+ "[Configuration]=@Configuration" + i + ","
					+ "[ConfigurationAppoitments]=@ConfigurationAppoitments" + i + ","
					+ "[ConfigurationChangeEmployees]=@ConfigurationChangeEmployees" + i + ","
					+ "[ConfigurationReplacements]=@ConfigurationReplacements" + i + ","
					+ "[ConfigurationReporting]=@ConfigurationReporting" + i + ","
					+ "[ConfirmationBookedEdit]=@ConfirmationBookedEdit" + i + ","
					+ "[ConfirmationCreate]=@ConfirmationCreate" + i + ","
					+ "[ConfirmationDelete]=@ConfirmationDelete" + i + ","
					+ "[ConfirmationDeliveryNote]=@ConfirmationDeliveryNote" + i + ","
					+ "[ConfirmationDoneEdit]=@ConfirmationDoneEdit" + i + ","
					+ "[ConfirmationEdit]=@ConfirmationEdit" + i + ","
					+ "[ConfirmationPositionEdit]=@ConfirmationPositionEdit" + i + ","
					+ "[ConfirmationPositionProduction]=@ConfirmationPositionProduction" + i + ","
					+ "[ConfirmationReport]=@ConfirmationReport" + i + ","
					+ "[ConfirmationValidate]=@ConfirmationValidate" + i + ","
					+ "[ConfirmationView]=@ConfirmationView" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CSInfoEdit]=@CSInfoEdit" + i + ","
					+ "[DelforCreate]=@DelforCreate" + i + ","
					+ "[DelforDelete]=@DelforDelete" + i + ","
					+ "[DelforDeletePosition]=@DelforDeletePosition" + i + ","
					+ "[DelforOrderConfirmation]=@DelforOrderConfirmation" + i + ","
					+ "[DelforReport]=@DelforReport" + i + ","
					+ "[DelforStatistics]=@DelforStatistics" + i + ","
					+ "[DelforView]=@DelforView" + i + ","
					+ "[DeliveryNoteBookedEdit]=@DeliveryNoteBookedEdit" + i + ","
					+ "[DeliveryNoteCreate]=@DeliveryNoteCreate" + i + ","
					+ "[DeliveryNoteDelete]=@DeliveryNoteDelete" + i + ","
					+ "[DeliveryNoteDoneEdit]=@DeliveryNoteDoneEdit" + i + ","
					+ "[DeliveryNoteEdit]=@DeliveryNoteEdit" + i + ","
					+ "[DeliveryNoteLog]=@DeliveryNoteLog" + i + ","
					+ "[DeliveryNotePositionEdit]=@DeliveryNotePositionEdit" + i + ","
					+ "[DeliveryNoteReport]=@DeliveryNoteReport" + i + ","
					+ "[DeliveryNoteView]=@DeliveryNoteView" + i + ","
					+ "[DLFPosHorizon1]=@DLFPosHorizon1" + i + ","
					+ "[DLFPosHorizon2]=@DLFPosHorizon2" + i + ","
					+ "[DLFPosHorizon3]=@DLFPosHorizon3" + i + ","
					+ "[EDI]=@EDI" + i + ","
					+ "[EDIDownloadFile]=@EDIDownloadFile" + i + ","
					+ "[EDIError]=@EDIError" + i + ","
					+ "[EDIErrorEdit]=@EDIErrorEdit" + i + ","
					+ "[EDIErrorValidated]=@EDIErrorValidated" + i + ","
					+ "[EDILogOrderValidated]=@EDILogOrderValidated" + i + ","
					+ "[EDIOrder]=@EDIOrder" + i + ","
					+ "[EDIOrderEdit]=@EDIOrderEdit" + i + ","
					+ "[EDIOrderPositionEdit]=@EDIOrderPositionEdit" + i + ","
					+ "[EDIOrderProduction]=@EDIOrderProduction" + i + ","
					+ "[EDIOrderProductionPosition]=@EDIOrderProductionPosition" + i + ","
					+ "[EDIOrderReport]=@EDIOrderReport" + i + ","
					+ "[EDIOrderValidated]=@EDIOrderValidated" + i + ","
					+ "[EDIOrderValidatedEdit]=@EDIOrderValidatedEdit" + i + ","
					+ "[FaABEdit]=@FaABEdit" + i + ","
					+ "[FaABView]=@FaABView" + i + ","
					+ "[FaActionBook]=@FaActionBook" + i + ","
					+ "[FaActionComplete]=@FaActionComplete" + i + ","
					+ "[FaActionDelete]=@FaActionDelete" + i + ","
					+ "[FaActionPrint]=@FaActionPrint" + i + ","
					+ "[FaAdmin]=@FaAdmin" + i + ","
					+ "[FAAKtualTerminUpdate]=@FAAKtualTerminUpdate" + i + ","
					+ "[FaAnalysis]=@FaAnalysis" + i + ","
					+ "[FAAuswertungEndkontrolle]=@FAAuswertungEndkontrolle" + i + ","
					+ "[FABemerkungPlannug]=@FABemerkungPlannug" + i + ","
					+ "[FABemerkungZuGewerk]=@FABemerkungZuGewerk" + i + ","
					+ "[FABemerkungZuPrio]=@FABemerkungZuPrio" + i + ","
					+ "[FACancelHorizon1]=@FACancelHorizon1" + i + ","
					+ "[FACancelHorizon2]=@FACancelHorizon2" + i + ","
					+ "[FACancelHorizon3]=@FACancelHorizon3" + i + ","
					+ "[FACommissionert]=@FACommissionert" + i + ","
					+ "[FaCreate]=@FaCreate" + i + ","
					+ "[FACreateHorizon1]=@FACreateHorizon1" + i + ","
					+ "[FACreateHorizon2]=@FACreateHorizon2" + i + ","
					+ "[FACreateHorizon3]=@FACreateHorizon3" + i + ","
					+ "[FaDatenEdit]=@FaDatenEdit" + i + ","
					+ "[FaDatenView]=@FaDatenView" + i + ","
					+ "[FaDelete]=@FaDelete" + i + ","
					+ "[FADrucken]=@FADrucken" + i + ","
					+ "[FaEdit]=@FaEdit" + i + ","
					+ "[FAErlidegen]=@FAErlidegen" + i + ","
					+ "[FAExcelUpdateWerk]=@FAExcelUpdateWerk" + i + ","
					+ "[FAExcelUpdateWunsh]=@FAExcelUpdateWunsh" + i + ","
					+ "[FAFehlrMaterial]=@FAFehlrMaterial" + i + ","
					+ "[FaHomeAnalysis]=@FaHomeAnalysis" + i + ","
					+ "[FaHomeUpdate]=@FaHomeUpdate" + i + ","
					+ "[FALaufkarteSchneiderei]=@FALaufkarteSchneiderei" + i + ","
					+ "[FaPlanningEdit]=@FaPlanningEdit" + i + ","
					+ "[FaPlanningView]=@FaPlanningView" + i + ","
					+ "[FAPlannung]=@FAPlannung" + i + ","
					+ "[FAPlannungTechnick]=@FAPlannungTechnick" + i + ","
					+ "[FAPriesZeitUpdate]=@FAPriesZeitUpdate" + i + ","
					+ "[FAProductionPlannung]=@FAProductionPlannung" + i + ","
					+ "[FAStappleDruck]=@FAStappleDruck" + i + ","
					+ "[FAStatusAlbania]=@FAStatusAlbania" + i + ","
					+ "[FAStatusCzech]=@FAStatusCzech" + i + ","
					+ "[FAStatusTunisia]=@FAStatusTunisia" + i + ","
					+ "[FAStorno]=@FAStorno" + i + ","
					+ "[FAStucklist]=@FAStucklist" + i + ","
					+ "[FaTechnicEdit]=@FaTechnicEdit" + i + ","
					+ "[FaTechnicView]=@FaTechnicView" + i + ","
					+ "[FATerminWerk]=@FATerminWerk" + i + ","
					+ "[FAUpdateBemerkungExtern]=@FAUpdateBemerkungExtern" + i + ","
					+ "[FAUpdateByArticle]=@FAUpdateByArticle" + i + ","
					+ "[FAUpdateByFA]=@FAUpdateByFA" + i + ","
					+ "[FAUpdateTerminHorizon1]=@FAUpdateTerminHorizon1" + i + ","
					+ "[FAUpdateTerminHorizon2]=@FAUpdateTerminHorizon2" + i + ","
					+ "[FAUpdateTerminHorizon3]=@FAUpdateTerminHorizon3" + i + ","
					+ "[FAWerkWunshAdmin]=@FAWerkWunshAdmin" + i + ","
					+ "[Fertigung]=@Fertigung" + i + ","
					+ "[FertigungLog]=@FertigungLog" + i + ","
					+ "[ForcastCreate]=@ForcastCreate" + i + ","
					+ "[ForcastDelete]=@ForcastDelete" + i + ","
					+ "[ForcastEdit]=@ForcastEdit" + i + ","
					+ "[ForcastLog]=@ForcastLog" + i + ","
					+ "[ForcastPositionEdit]=@ForcastPositionEdit" + i + ","
					+ "[ForcastReport]=@ForcastReport" + i + ","
					+ "[ForcastView]=@ForcastView" + i + ","
					+ "[FRCPosHorizon1]=@FRCPosHorizon1" + i + ","
					+ "[FRCPosHorizon2]=@FRCPosHorizon2" + i + ","
					+ "[FRCPosHorizon3]=@FRCPosHorizon3" + i + ","
					+ "[GSPosHorizon1]=@GSPosHorizon1" + i + ","
					+ "[GSPosHorizon2]=@GSPosHorizon2" + i + ","
					+ "[GSPosHorizon3]=@GSPosHorizon3" + i + ","
					+ "[GutschriftBookedEdit]=@GutschriftBookedEdit" + i + ","
					+ "[GutschriftCreate]=@GutschriftCreate" + i + ","
					+ "[GutschriftDelete]=@GutschriftDelete" + i + ","
					+ "[GutschriftDoneEdit]=@GutschriftDoneEdit" + i + ","
					+ "[GutschriftEdit]=@GutschriftEdit" + i + ","
					+ "[GutschriftLog]=@GutschriftLog" + i + ","
					+ "[GutschriftPositionEdit]=@GutschriftPositionEdit" + i + ","
					+ "[GutschriftReport]=@GutschriftReport" + i + ","
					+ "[GutschriftView]=@GutschriftView" + i + ","
					+ "[InsideSalesChecks]=@InsideSalesChecks" + i + ","
					+ "[InsideSalesChecksArchive]=@InsideSalesChecksArchive" + i + ","
					+ "[InsideSalesCustomerSummary]=@InsideSalesCustomerSummary" + i + ","
					+ "[InsideSalesMinimumStockEvaluation]=@InsideSalesMinimumStockEvaluation" + i + ","
					+ "[InsideSalesMinimumStockEvaluationTable]=@InsideSalesMinimumStockEvaluationTable" + i + ","
					+ "[InsideSalesOverdueOrders]=@InsideSalesOverdueOrders" + i + ","
					+ "[InsideSalesOverdueOrdersTable]=@InsideSalesOverdueOrdersTable" + i + ","
					+ "[InsideSalesTotalUnbookedOrders]=@InsideSalesTotalUnbookedOrders" + i + ","
					+ "[InsideSalesTotalUnbookedOrdersTable]=@InsideSalesTotalUnbookedOrdersTable" + i + ","
					+ "[InsideSalesTurnoverCurrentWeek]=@InsideSalesTurnoverCurrentWeek" + i + ","
					+ "[IsDefault]=@IsDefault" + i + ","
					+ "[LSPosHorizon1]=@LSPosHorizon1" + i + ","
					+ "[LSPosHorizon2]=@LSPosHorizon2" + i + ","
					+ "[LSPosHorizon3]=@LSPosHorizon3" + i + ","
					+ "[mId]=@mId" + i + ","
					+ "[ModuleActivated]=@ModuleActivated" + i + ","
					+ "[OrderProcessing]=@OrderProcessing" + i + ","
					+ "[OrderProcessingLog]=@OrderProcessingLog" + i + ","
					+ "[Rahmen]=@Rahmen" + i + ","
					+ "[RahmenAdd]=@RahmenAdd" + i + ","
					+ "[RahmenAddAB]=@RahmenAddAB" + i + ","
					+ "[RahmenAddPositions]=@RahmenAddPositions" + i + ","
					+ "[RahmenCancelation]=@RahmenCancelation" + i + ","
					+ "[RahmenClosure]=@RahmenClosure" + i + ","
					+ "[RahmenDelete]=@RahmenDelete" + i + ","
					+ "[RahmenDeletePositions]=@RahmenDeletePositions" + i + ","
					+ "[RahmenDocumentFlow]=@RahmenDocumentFlow" + i + ","
					+ "[RahmenEditHeader]=@RahmenEditHeader" + i + ","
					+ "[RahmenEditPositions]=@RahmenEditPositions" + i + ","
					+ "[RahmenHistory]=@RahmenHistory" + i + ","
					+ "[RahmenValdation]=@RahmenValdation" + i + ","
					+ "[RAPosHorizon1]=@RAPosHorizon1" + i + ","
					+ "[RAPosHorizon2]=@RAPosHorizon2" + i + ","
					+ "[RAPosHorizon3]=@RAPosHorizon3" + i + ","
					+ "[Rechnung]=@Rechnung" + i + ","
					+ "[RechnungAutoCreation]=@RechnungAutoCreation" + i + ","
					+ "[RechnungBookedEdit]=@RechnungBookedEdit" + i + ","
					+ "[RechnungConfig]=@RechnungConfig" + i + ","
					+ "[RechnungDelete]=@RechnungDelete" + i + ","
					+ "[RechnungDoneEdit]=@RechnungDoneEdit" + i + ","
					+ "[RechnungManualCreation]=@RechnungManualCreation" + i + ","
					+ "[RechnungReport]=@RechnungReport" + i + ","
					+ "[RechnungSend]=@RechnungSend" + i + ","
					+ "[RechnungValidate]=@RechnungValidate" + i + ","
					+ "[RGPosHorizon1]=@RGPosHorizon1" + i + ","
					+ "[RGPosHorizon2]=@RGPosHorizon2" + i + ","
					+ "[RGPosHorizon3]=@RGPosHorizon3" + i + ","
					+ "[Statistics]=@Statistics" + i + ","
					+ "[StatsBacklogFGAdmin]=@StatsBacklogFGAdmin" + i + ","
					+ "[StatsBacklogHWAdmin]=@StatsBacklogHWAdmin" + i + ","
					+ "[StatsCapaCutting]=@StatsCapaCutting" + i + ","
					+ "[StatsCapaHorizons]=@StatsCapaHorizons" + i + ","
					+ "[StatsCapaLong]=@StatsCapaLong" + i + ","
					+ "[StatsCapaPlanning]=@StatsCapaPlanning" + i + ","
					+ "[StatsCapaShort]=@StatsCapaShort" + i + ","
					+ "[StatsRechnungAL]=@StatsRechnungAL" + i + ","
					+ "[StatsRechnungBETN]=@StatsRechnungBETN" + i + ","
					+ "[StatsRechnungCZ]=@StatsRechnungCZ" + i + ","
					+ "[StatsRechnungDE]=@StatsRechnungDE" + i + ","
					+ "[StatsRechnungGZTN]=@StatsRechnungGZTN" + i + ","
					+ "[StatsRechnungTN]=@StatsRechnungTN" + i + ","
					+ "[StatsRechnungWS]=@StatsRechnungWS" + i + ","
					+ "[StatsStockCS]=@StatsStockCS" + i + ","
					+ "[StatsStockExternalWarehouse]=@StatsStockExternalWarehouse" + i + ","
					+ "[StatsStockFG]=@StatsStockFG" + i + ","
					+ "[UBGStatusChange]=@UBGStatusChange" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AB_LT" + i, item.AB_LT == null ? (object)DBNull.Value : item.AB_LT);
					sqlCommand.Parameters.AddWithValue("AB_LT_EDI" + i, item.AB_LT_EDI == null ? (object)DBNull.Value : item.AB_LT_EDI);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon1" + i, item.ABPosHorizon1 == null ? (object)DBNull.Value : item.ABPosHorizon1);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon2" + i, item.ABPosHorizon2 == null ? (object)DBNull.Value : item.ABPosHorizon2);
					sqlCommand.Parameters.AddWithValue("ABPosHorizon3" + i, item.ABPosHorizon3 == null ? (object)DBNull.Value : item.ABPosHorizon3);
					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("BVBookedEdit" + i, item.BVBookedEdit == null ? (object)DBNull.Value : item.BVBookedEdit);
					sqlCommand.Parameters.AddWithValue("BVDoneEdit" + i, item.BVDoneEdit == null ? (object)DBNull.Value : item.BVDoneEdit);
					sqlCommand.Parameters.AddWithValue("BVFaCreate" + i, item.BVFaCreate == null ? (object)DBNull.Value : item.BVFaCreate);
					sqlCommand.Parameters.AddWithValue("Configuration" + i, item.Configuration == null ? (object)DBNull.Value : item.Configuration);
					sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments" + i, item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
					sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees" + i, item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
					sqlCommand.Parameters.AddWithValue("ConfigurationReplacements" + i, item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
					sqlCommand.Parameters.AddWithValue("ConfigurationReporting" + i, item.ConfigurationReporting == null ? (object)DBNull.Value : item.ConfigurationReporting);
					sqlCommand.Parameters.AddWithValue("ConfirmationBookedEdit" + i, item.ConfirmationBookedEdit == null ? (object)DBNull.Value : item.ConfirmationBookedEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationCreate" + i, item.ConfirmationCreate == null ? (object)DBNull.Value : item.ConfirmationCreate);
					sqlCommand.Parameters.AddWithValue("ConfirmationDelete" + i, item.ConfirmationDelete == null ? (object)DBNull.Value : item.ConfirmationDelete);
					sqlCommand.Parameters.AddWithValue("ConfirmationDeliveryNote" + i, item.ConfirmationDeliveryNote == null ? (object)DBNull.Value : item.ConfirmationDeliveryNote);
					sqlCommand.Parameters.AddWithValue("ConfirmationDoneEdit" + i, item.ConfirmationDoneEdit == null ? (object)DBNull.Value : item.ConfirmationDoneEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationEdit" + i, item.ConfirmationEdit == null ? (object)DBNull.Value : item.ConfirmationEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationPositionEdit" + i, item.ConfirmationPositionEdit == null ? (object)DBNull.Value : item.ConfirmationPositionEdit);
					sqlCommand.Parameters.AddWithValue("ConfirmationPositionProduction" + i, item.ConfirmationPositionProduction == null ? (object)DBNull.Value : item.ConfirmationPositionProduction);
					sqlCommand.Parameters.AddWithValue("ConfirmationReport" + i, item.ConfirmationReport == null ? (object)DBNull.Value : item.ConfirmationReport);
					sqlCommand.Parameters.AddWithValue("ConfirmationValidate" + i, item.ConfirmationValidate == null ? (object)DBNull.Value : item.ConfirmationValidate);
					sqlCommand.Parameters.AddWithValue("ConfirmationView" + i, item.ConfirmationView == null ? (object)DBNull.Value : item.ConfirmationView);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CSInfoEdit" + i, item.CSInfoEdit == null ? (object)DBNull.Value : item.CSInfoEdit);
					sqlCommand.Parameters.AddWithValue("DelforCreate" + i, item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
					sqlCommand.Parameters.AddWithValue("DelforDelete" + i, item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
					sqlCommand.Parameters.AddWithValue("DelforDeletePosition" + i, item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
					sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation" + i, item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
					sqlCommand.Parameters.AddWithValue("DelforReport" + i, item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
					sqlCommand.Parameters.AddWithValue("DelforStatistics" + i, item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
					sqlCommand.Parameters.AddWithValue("DelforView" + i, item.DelforView == null ? (object)DBNull.Value : item.DelforView);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteBookedEdit" + i, item.DeliveryNoteBookedEdit == null ? (object)DBNull.Value : item.DeliveryNoteBookedEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteCreate" + i, item.DeliveryNoteCreate == null ? (object)DBNull.Value : item.DeliveryNoteCreate);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteDelete" + i, item.DeliveryNoteDelete == null ? (object)DBNull.Value : item.DeliveryNoteDelete);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteDoneEdit" + i, item.DeliveryNoteDoneEdit == null ? (object)DBNull.Value : item.DeliveryNoteDoneEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteEdit" + i, item.DeliveryNoteEdit == null ? (object)DBNull.Value : item.DeliveryNoteEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteLog" + i, item.DeliveryNoteLog == null ? (object)DBNull.Value : item.DeliveryNoteLog);
					sqlCommand.Parameters.AddWithValue("DeliveryNotePositionEdit" + i, item.DeliveryNotePositionEdit == null ? (object)DBNull.Value : item.DeliveryNotePositionEdit);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteReport" + i, item.DeliveryNoteReport == null ? (object)DBNull.Value : item.DeliveryNoteReport);
					sqlCommand.Parameters.AddWithValue("DeliveryNoteView" + i, item.DeliveryNoteView == null ? (object)DBNull.Value : item.DeliveryNoteView);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon1" + i, item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon2" + i, item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon3" + i, item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
					sqlCommand.Parameters.AddWithValue("EDI" + i, item.EDI == null ? (object)DBNull.Value : item.EDI);
					sqlCommand.Parameters.AddWithValue("EDIDownloadFile" + i, item.EDIDownloadFile == null ? (object)DBNull.Value : item.EDIDownloadFile);
					sqlCommand.Parameters.AddWithValue("EDIError" + i, item.EDIError == null ? (object)DBNull.Value : item.EDIError);
					sqlCommand.Parameters.AddWithValue("EDIErrorEdit" + i, item.EDIErrorEdit == null ? (object)DBNull.Value : item.EDIErrorEdit);
					sqlCommand.Parameters.AddWithValue("EDIErrorValidated" + i, item.EDIErrorValidated == null ? (object)DBNull.Value : item.EDIErrorValidated);
					sqlCommand.Parameters.AddWithValue("EDILogOrderValidated" + i, item.EDILogOrderValidated == null ? (object)DBNull.Value : item.EDILogOrderValidated);
					sqlCommand.Parameters.AddWithValue("EDIOrder" + i, item.EDIOrder == null ? (object)DBNull.Value : item.EDIOrder);
					sqlCommand.Parameters.AddWithValue("EDIOrderEdit" + i, item.EDIOrderEdit == null ? (object)DBNull.Value : item.EDIOrderEdit);
					sqlCommand.Parameters.AddWithValue("EDIOrderPositionEdit" + i, item.EDIOrderPositionEdit == null ? (object)DBNull.Value : item.EDIOrderPositionEdit);
					sqlCommand.Parameters.AddWithValue("EDIOrderProduction" + i, item.EDIOrderProduction == null ? (object)DBNull.Value : item.EDIOrderProduction);
					sqlCommand.Parameters.AddWithValue("EDIOrderProductionPosition" + i, item.EDIOrderProductionPosition == null ? (object)DBNull.Value : item.EDIOrderProductionPosition);
					sqlCommand.Parameters.AddWithValue("EDIOrderReport" + i, item.EDIOrderReport == null ? (object)DBNull.Value : item.EDIOrderReport);
					sqlCommand.Parameters.AddWithValue("EDIOrderValidated" + i, item.EDIOrderValidated == null ? (object)DBNull.Value : item.EDIOrderValidated);
					sqlCommand.Parameters.AddWithValue("EDIOrderValidatedEdit" + i, item.EDIOrderValidatedEdit == null ? (object)DBNull.Value : item.EDIOrderValidatedEdit);
					sqlCommand.Parameters.AddWithValue("FaABEdit" + i, item.FaABEdit == null ? (object)DBNull.Value : item.FaABEdit);
					sqlCommand.Parameters.AddWithValue("FaABView" + i, item.FaABView == null ? (object)DBNull.Value : item.FaABView);
					sqlCommand.Parameters.AddWithValue("FaActionBook" + i, item.FaActionBook == null ? (object)DBNull.Value : item.FaActionBook);
					sqlCommand.Parameters.AddWithValue("FaActionComplete" + i, item.FaActionComplete == null ? (object)DBNull.Value : item.FaActionComplete);
					sqlCommand.Parameters.AddWithValue("FaActionDelete" + i, item.FaActionDelete == null ? (object)DBNull.Value : item.FaActionDelete);
					sqlCommand.Parameters.AddWithValue("FaActionPrint" + i, item.FaActionPrint == null ? (object)DBNull.Value : item.FaActionPrint);
					sqlCommand.Parameters.AddWithValue("FaAdmin" + i, item.FaAdmin == null ? (object)DBNull.Value : item.FaAdmin);
					sqlCommand.Parameters.AddWithValue("FAAKtualTerminUpdate" + i, item.FAAKtualTerminUpdate == null ? (object)DBNull.Value : item.FAAKtualTerminUpdate);
					sqlCommand.Parameters.AddWithValue("FaAnalysis" + i, item.FaAnalysis == null ? (object)DBNull.Value : item.FaAnalysis);
					sqlCommand.Parameters.AddWithValue("FAAuswertungEndkontrolle" + i, item.FAAuswertungEndkontrolle == null ? (object)DBNull.Value : item.FAAuswertungEndkontrolle);
					sqlCommand.Parameters.AddWithValue("FABemerkungPlannug" + i, item.FABemerkungPlannug == null ? (object)DBNull.Value : item.FABemerkungPlannug);
					sqlCommand.Parameters.AddWithValue("FABemerkungZuGewerk" + i, item.FABemerkungZuGewerk == null ? (object)DBNull.Value : item.FABemerkungZuGewerk);
					sqlCommand.Parameters.AddWithValue("FABemerkungZuPrio" + i, item.FABemerkungZuPrio == null ? (object)DBNull.Value : item.FABemerkungZuPrio);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon1" + i, item.FACancelHorizon1 == null ? (object)DBNull.Value : item.FACancelHorizon1);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon2" + i, item.FACancelHorizon2 == null ? (object)DBNull.Value : item.FACancelHorizon2);
					sqlCommand.Parameters.AddWithValue("FACancelHorizon3" + i, item.FACancelHorizon3 == null ? (object)DBNull.Value : item.FACancelHorizon3);
					sqlCommand.Parameters.AddWithValue("FACommissionert" + i, item.FACommissionert == null ? (object)DBNull.Value : item.FACommissionert);
					sqlCommand.Parameters.AddWithValue("FaCreate" + i, item.FaCreate == null ? (object)DBNull.Value : item.FaCreate);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon1" + i, item.FACreateHorizon1 == null ? (object)DBNull.Value : item.FACreateHorizon1);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon2" + i, item.FACreateHorizon2 == null ? (object)DBNull.Value : item.FACreateHorizon2);
					sqlCommand.Parameters.AddWithValue("FACreateHorizon3" + i, item.FACreateHorizon3 == null ? (object)DBNull.Value : item.FACreateHorizon3);
					sqlCommand.Parameters.AddWithValue("FaDatenEdit" + i, item.FaDatenEdit == null ? (object)DBNull.Value : item.FaDatenEdit);
					sqlCommand.Parameters.AddWithValue("FaDatenView" + i, item.FaDatenView == null ? (object)DBNull.Value : item.FaDatenView);
					sqlCommand.Parameters.AddWithValue("FaDelete" + i, item.FaDelete == null ? (object)DBNull.Value : item.FaDelete);
					sqlCommand.Parameters.AddWithValue("FADrucken" + i, item.FADrucken == null ? (object)DBNull.Value : item.FADrucken);
					sqlCommand.Parameters.AddWithValue("FaEdit" + i, item.FaEdit == null ? (object)DBNull.Value : item.FaEdit);
					sqlCommand.Parameters.AddWithValue("FAErlidegen" + i, item.FAErlidegen == null ? (object)DBNull.Value : item.FAErlidegen);
					sqlCommand.Parameters.AddWithValue("FAExcelUpdateWerk" + i, item.FAExcelUpdateWerk == null ? (object)DBNull.Value : item.FAExcelUpdateWerk);
					sqlCommand.Parameters.AddWithValue("FAExcelUpdateWunsh" + i, item.FAExcelUpdateWunsh == null ? (object)DBNull.Value : item.FAExcelUpdateWunsh);
					sqlCommand.Parameters.AddWithValue("FAFehlrMaterial" + i, item.FAFehlrMaterial == null ? (object)DBNull.Value : item.FAFehlrMaterial);
					sqlCommand.Parameters.AddWithValue("FaHomeAnalysis" + i, item.FaHomeAnalysis == null ? (object)DBNull.Value : item.FaHomeAnalysis);
					sqlCommand.Parameters.AddWithValue("FaHomeUpdate" + i, item.FaHomeUpdate == null ? (object)DBNull.Value : item.FaHomeUpdate);
					sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei" + i, item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
					sqlCommand.Parameters.AddWithValue("FaPlanningEdit" + i, item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
					sqlCommand.Parameters.AddWithValue("FaPlanningView" + i, item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
					sqlCommand.Parameters.AddWithValue("FAPlannung" + i, item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
					sqlCommand.Parameters.AddWithValue("FAPlannungTechnick" + i, item.FAPlannungTechnick == null ? (object)DBNull.Value : item.FAPlannungTechnick);
					sqlCommand.Parameters.AddWithValue("FAPriesZeitUpdate" + i, item.FAPriesZeitUpdate == null ? (object)DBNull.Value : item.FAPriesZeitUpdate);
					sqlCommand.Parameters.AddWithValue("FAProductionPlannung" + i, item.FAProductionPlannung == null ? (object)DBNull.Value : item.FAProductionPlannung);
					sqlCommand.Parameters.AddWithValue("FAStappleDruck" + i, item.FAStappleDruck == null ? (object)DBNull.Value : item.FAStappleDruck);
					sqlCommand.Parameters.AddWithValue("FAStatusAlbania" + i, item.FAStatusAlbania == null ? (object)DBNull.Value : item.FAStatusAlbania);
					sqlCommand.Parameters.AddWithValue("FAStatusCzech" + i, item.FAStatusCzech == null ? (object)DBNull.Value : item.FAStatusCzech);
					sqlCommand.Parameters.AddWithValue("FAStatusTunisia" + i, item.FAStatusTunisia == null ? (object)DBNull.Value : item.FAStatusTunisia);
					sqlCommand.Parameters.AddWithValue("FAStorno" + i, item.FAStorno == null ? (object)DBNull.Value : item.FAStorno);
					sqlCommand.Parameters.AddWithValue("FAStucklist" + i, item.FAStucklist == null ? (object)DBNull.Value : item.FAStucklist);
					sqlCommand.Parameters.AddWithValue("FaTechnicEdit" + i, item.FaTechnicEdit == null ? (object)DBNull.Value : item.FaTechnicEdit);
					sqlCommand.Parameters.AddWithValue("FaTechnicView" + i, item.FaTechnicView == null ? (object)DBNull.Value : item.FaTechnicView);
					sqlCommand.Parameters.AddWithValue("FATerminWerk" + i, item.FATerminWerk == null ? (object)DBNull.Value : item.FATerminWerk);
					sqlCommand.Parameters.AddWithValue("FAUpdateBemerkungExtern" + i, item.FAUpdateBemerkungExtern == null ? (object)DBNull.Value : item.FAUpdateBemerkungExtern);
					sqlCommand.Parameters.AddWithValue("FAUpdateByArticle" + i, item.FAUpdateByArticle == null ? (object)DBNull.Value : item.FAUpdateByArticle);
					sqlCommand.Parameters.AddWithValue("FAUpdateByFA" + i, item.FAUpdateByFA == null ? (object)DBNull.Value : item.FAUpdateByFA);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1" + i, item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2" + i, item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3" + i, item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
					sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin" + i, item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
					sqlCommand.Parameters.AddWithValue("Fertigung" + i, item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
					sqlCommand.Parameters.AddWithValue("FertigungLog" + i, item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
					sqlCommand.Parameters.AddWithValue("ForcastCreate" + i, item.ForcastCreate == null ? (object)DBNull.Value : item.ForcastCreate);
					sqlCommand.Parameters.AddWithValue("ForcastDelete" + i, item.ForcastDelete == null ? (object)DBNull.Value : item.ForcastDelete);
					sqlCommand.Parameters.AddWithValue("ForcastEdit" + i, item.ForcastEdit == null ? (object)DBNull.Value : item.ForcastEdit);
					sqlCommand.Parameters.AddWithValue("ForcastLog" + i, item.ForcastLog == null ? (object)DBNull.Value : item.ForcastLog);
					sqlCommand.Parameters.AddWithValue("ForcastPositionEdit" + i, item.ForcastPositionEdit == null ? (object)DBNull.Value : item.ForcastPositionEdit);
					sqlCommand.Parameters.AddWithValue("ForcastReport" + i, item.ForcastReport == null ? (object)DBNull.Value : item.ForcastReport);
					sqlCommand.Parameters.AddWithValue("ForcastView" + i, item.ForcastView == null ? (object)DBNull.Value : item.ForcastView);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon1" + i, item.FRCPosHorizon1 == null ? (object)DBNull.Value : item.FRCPosHorizon1);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon2" + i, item.FRCPosHorizon2 == null ? (object)DBNull.Value : item.FRCPosHorizon2);
					sqlCommand.Parameters.AddWithValue("FRCPosHorizon3" + i, item.FRCPosHorizon3 == null ? (object)DBNull.Value : item.FRCPosHorizon3);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon1" + i, item.GSPosHorizon1 == null ? (object)DBNull.Value : item.GSPosHorizon1);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon2" + i, item.GSPosHorizon2 == null ? (object)DBNull.Value : item.GSPosHorizon2);
					sqlCommand.Parameters.AddWithValue("GSPosHorizon3" + i, item.GSPosHorizon3 == null ? (object)DBNull.Value : item.GSPosHorizon3);
					sqlCommand.Parameters.AddWithValue("GutschriftBookedEdit" + i, item.GutschriftBookedEdit == null ? (object)DBNull.Value : item.GutschriftBookedEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftCreate" + i, item.GutschriftCreate == null ? (object)DBNull.Value : item.GutschriftCreate);
					sqlCommand.Parameters.AddWithValue("GutschriftDelete" + i, item.GutschriftDelete == null ? (object)DBNull.Value : item.GutschriftDelete);
					sqlCommand.Parameters.AddWithValue("GutschriftDoneEdit" + i, item.GutschriftDoneEdit == null ? (object)DBNull.Value : item.GutschriftDoneEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftEdit" + i, item.GutschriftEdit == null ? (object)DBNull.Value : item.GutschriftEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftLog" + i, item.GutschriftLog == null ? (object)DBNull.Value : item.GutschriftLog);
					sqlCommand.Parameters.AddWithValue("GutschriftPositionEdit" + i, item.GutschriftPositionEdit == null ? (object)DBNull.Value : item.GutschriftPositionEdit);
					sqlCommand.Parameters.AddWithValue("GutschriftReport" + i, item.GutschriftReport == null ? (object)DBNull.Value : item.GutschriftReport);
					sqlCommand.Parameters.AddWithValue("GutschriftView" + i, item.GutschriftView == null ? (object)DBNull.Value : item.GutschriftView);
					sqlCommand.Parameters.AddWithValue("InsideSalesChecks" + i, item.InsideSalesChecks == null ? (object)DBNull.Value : item.InsideSalesChecks);
					sqlCommand.Parameters.AddWithValue("InsideSalesChecksArchive" + i, item.InsideSalesChecksArchive == null ? (object)DBNull.Value : item.InsideSalesChecksArchive);
					sqlCommand.Parameters.AddWithValue("InsideSalesCustomerSummary" + i, item.InsideSalesCustomerSummary == null ? (object)DBNull.Value : item.InsideSalesCustomerSummary);
					sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluation" + i, item.InsideSalesMinimumStockEvaluation == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluation);
					sqlCommand.Parameters.AddWithValue("InsideSalesMinimumStockEvaluationTable" + i, item.InsideSalesMinimumStockEvaluationTable == null ? (object)DBNull.Value : item.InsideSalesMinimumStockEvaluationTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrders" + i, item.InsideSalesOverdueOrders == null ? (object)DBNull.Value : item.InsideSalesOverdueOrders);
					sqlCommand.Parameters.AddWithValue("InsideSalesOverdueOrdersTable" + i, item.InsideSalesOverdueOrdersTable == null ? (object)DBNull.Value : item.InsideSalesOverdueOrdersTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrders" + i, item.InsideSalesTotalUnbookedOrders == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrders);
					sqlCommand.Parameters.AddWithValue("InsideSalesTotalUnbookedOrdersTable" + i, item.InsideSalesTotalUnbookedOrdersTable == null ? (object)DBNull.Value : item.InsideSalesTotalUnbookedOrdersTable);
					sqlCommand.Parameters.AddWithValue("InsideSalesTurnoverCurrentWeek" + i, item.InsideSalesTurnoverCurrentWeek == null ? (object)DBNull.Value : item.InsideSalesTurnoverCurrentWeek);
					sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault == null ? (object)DBNull.Value : item.IsDefault);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon1" + i, item.LSPosHorizon1 == null ? (object)DBNull.Value : item.LSPosHorizon1);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon2" + i, item.LSPosHorizon2 == null ? (object)DBNull.Value : item.LSPosHorizon2);
					sqlCommand.Parameters.AddWithValue("LSPosHorizon3" + i, item.LSPosHorizon3 == null ? (object)DBNull.Value : item.LSPosHorizon3);
					sqlCommand.Parameters.AddWithValue("mId" + i, item.mId == null ? (object)DBNull.Value : item.mId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("OrderProcessing" + i, item.OrderProcessing == null ? (object)DBNull.Value : item.OrderProcessing);
					sqlCommand.Parameters.AddWithValue("OrderProcessingLog" + i, item.OrderProcessingLog == null ? (object)DBNull.Value : item.OrderProcessingLog);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("RahmenAdd" + i, item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
					sqlCommand.Parameters.AddWithValue("RahmenAddAB" + i, item.RahmenAddAB == null ? (object)DBNull.Value : item.RahmenAddAB);
					sqlCommand.Parameters.AddWithValue("RahmenAddPositions" + i, item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
					sqlCommand.Parameters.AddWithValue("RahmenCancelation" + i, item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
					sqlCommand.Parameters.AddWithValue("RahmenClosure" + i, item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
					sqlCommand.Parameters.AddWithValue("RahmenDelete" + i, item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
					sqlCommand.Parameters.AddWithValue("RahmenDeletePositions" + i, item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
					sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow" + i, item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
					sqlCommand.Parameters.AddWithValue("RahmenEditHeader" + i, item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
					sqlCommand.Parameters.AddWithValue("RahmenEditPositions" + i, item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
					sqlCommand.Parameters.AddWithValue("RahmenHistory" + i, item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
					sqlCommand.Parameters.AddWithValue("RahmenValdation" + i, item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon1" + i, item.RAPosHorizon1 == null ? (object)DBNull.Value : item.RAPosHorizon1);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon2" + i, item.RAPosHorizon2 == null ? (object)DBNull.Value : item.RAPosHorizon2);
					sqlCommand.Parameters.AddWithValue("RAPosHorizon3" + i, item.RAPosHorizon3 == null ? (object)DBNull.Value : item.RAPosHorizon3);
					sqlCommand.Parameters.AddWithValue("Rechnung" + i, item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
					sqlCommand.Parameters.AddWithValue("RechnungAutoCreation" + i, item.RechnungAutoCreation == null ? (object)DBNull.Value : item.RechnungAutoCreation);
					sqlCommand.Parameters.AddWithValue("RechnungBookedEdit" + i, item.RechnungBookedEdit == null ? (object)DBNull.Value : item.RechnungBookedEdit);
					sqlCommand.Parameters.AddWithValue("RechnungConfig" + i, item.RechnungConfig == null ? (object)DBNull.Value : item.RechnungConfig);
					sqlCommand.Parameters.AddWithValue("RechnungDelete" + i, item.RechnungDelete == null ? (object)DBNull.Value : item.RechnungDelete);
					sqlCommand.Parameters.AddWithValue("RechnungDoneEdit" + i, item.RechnungDoneEdit == null ? (object)DBNull.Value : item.RechnungDoneEdit);
					sqlCommand.Parameters.AddWithValue("RechnungManualCreation" + i, item.RechnungManualCreation == null ? (object)DBNull.Value : item.RechnungManualCreation);
					sqlCommand.Parameters.AddWithValue("RechnungReport" + i, item.RechnungReport == null ? (object)DBNull.Value : item.RechnungReport);
					sqlCommand.Parameters.AddWithValue("RechnungSend" + i, item.RechnungSend == null ? (object)DBNull.Value : item.RechnungSend);
					sqlCommand.Parameters.AddWithValue("RechnungValidate" + i, item.RechnungValidate == null ? (object)DBNull.Value : item.RechnungValidate);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon1" + i, item.RGPosHorizon1 == null ? (object)DBNull.Value : item.RGPosHorizon1);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon2" + i, item.RGPosHorizon2 == null ? (object)DBNull.Value : item.RGPosHorizon2);
					sqlCommand.Parameters.AddWithValue("RGPosHorizon3" + i, item.RGPosHorizon3 == null ? (object)DBNull.Value : item.RGPosHorizon3);
					sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatsBacklogFGAdmin" + i, item.StatsBacklogFGAdmin == null ? (object)DBNull.Value : item.StatsBacklogFGAdmin);
					sqlCommand.Parameters.AddWithValue("StatsBacklogHWAdmin" + i, item.StatsBacklogHWAdmin == null ? (object)DBNull.Value : item.StatsBacklogHWAdmin);
					sqlCommand.Parameters.AddWithValue("StatsCapaCutting" + i, item.StatsCapaCutting == null ? (object)DBNull.Value : item.StatsCapaCutting);
					sqlCommand.Parameters.AddWithValue("StatsCapaHorizons" + i, item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
					sqlCommand.Parameters.AddWithValue("StatsCapaLong" + i, item.StatsCapaLong == null ? (object)DBNull.Value : item.StatsCapaLong);
					sqlCommand.Parameters.AddWithValue("StatsCapaPlanning" + i, item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
					sqlCommand.Parameters.AddWithValue("StatsCapaShort" + i, item.StatsCapaShort == null ? (object)DBNull.Value : item.StatsCapaShort);
					sqlCommand.Parameters.AddWithValue("StatsRechnungAL" + i, item.StatsRechnungAL == null ? (object)DBNull.Value : item.StatsRechnungAL);
					sqlCommand.Parameters.AddWithValue("StatsRechnungBETN" + i, item.StatsRechnungBETN == null ? (object)DBNull.Value : item.StatsRechnungBETN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungCZ" + i, item.StatsRechnungCZ == null ? (object)DBNull.Value : item.StatsRechnungCZ);
					sqlCommand.Parameters.AddWithValue("StatsRechnungDE" + i, item.StatsRechnungDE == null ? (object)DBNull.Value : item.StatsRechnungDE);
					sqlCommand.Parameters.AddWithValue("StatsRechnungGZTN" + i, item.StatsRechnungGZTN == null ? (object)DBNull.Value : item.StatsRechnungGZTN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungTN" + i, item.StatsRechnungTN == null ? (object)DBNull.Value : item.StatsRechnungTN);
					sqlCommand.Parameters.AddWithValue("StatsRechnungWS" + i, item.StatsRechnungWS == null ? (object)DBNull.Value : item.StatsRechnungWS);
					sqlCommand.Parameters.AddWithValue("StatsStockCS" + i, item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
					sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse" + i, item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
					sqlCommand.Parameters.AddWithValue("StatsStockFG" + i, item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
					sqlCommand.Parameters.AddWithValue("UBGStatusChange" + i, item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__CTS_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [__CTS_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int UpdateName(Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_AccessProfile] SET [AccessProfileName]=@AccessProfileName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Entities.Tables.CTS.AccessProfileEntity> GetByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.CTS.AccessProfileEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = getByMainAccessProfilesIds(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Entities.Tables.CTS.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getByMainAccessProfilesIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(getByMainAccessProfilesIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.CTS.AccessProfileEntity>();
		}
		private static List<Entities.Tables.CTS.AccessProfileEntity> getByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [__CTS_AccessProfile] WHERE [Id] IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.CTS.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.CTS.AccessProfileEntity>();
		}
		public static Entities.Tables.CTS.AccessProfileEntity GetByMainAccessProfilesId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.CTS.AccessProfileEntity GetByName(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AccessProfile] WHERE [AccessProfileName]=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name.Trim());

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> GetAdminBlanket()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_AccessProfile] WHere RahmenValdation=1";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity> GetDefaultProfiles(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__CTS_AccessProfile] WHERE [isDefault]=1", connection,transaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.AccessProfileEntity>();
			}
		}
		#endregion Custom Methods
	}
}