using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CRP
{
    public class AccessProfileAccess
    {
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CRP_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CRP_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CRP_AccessProfile] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CRP_AccessProfile] ([AccessProfileName],[Administration],[AgentFGXlsHistory],[AllFaChanges],[AllProductionWarehouses],[Configuration],[ConfigurationAppoitments],[ConfigurationChangeEmployees],[ConfigurationReplacements],[CreationTime],[CreationUserId],[CRPDashboardActiveArticles],[CRPDashboardCancelledOrders],[CRPDashboardCreatedOrders],[CRPDashboardOpenOrders],[CRPDashboardOpenOrdersHours],[CRPDashboardTotalStockFG],[CRPFAPlannung],[CRPPlanning],[CRPRequirement],[CRPUBGPlanning],[DelforCreate],[DelforDelete],[DelforDeletePosition],[DelforOrderConfirmation],[DelforReport],[DelforStatistics],[DelforView],[DLFPosHorizon1],[DLFPosHorizon2],[DLFPosHorizon3],[ExportFGXlsHistory],[FaABEdit],[FaABView],[FaActionBook],[FaActionComplete],[FaActionDelete],[FaActionPrint],[FaAdmin],[FAAKtualTerminUpdate],[FaAnalysis],[FAAuswertungEndkontrolle],[FABemerkungPlannug],[FABemerkungZuGewerk],[FABemerkungZuPrio],[FACancelHorizon1],[FACancelHorizon2],[FACancelHorizon3],[FACommissionert],[FaCreate],[FACreateHorizon1],[FACreateHorizon2],[FACreateHorizon3],[FaDateChangeHistory],[FaDatenEdit],[FaDatenView],[FaDelete],[FADrucken],[FaEdit],[FAErlidegen],[FAExcelUpdateWerk],[FAExcelUpdateWunsh],[FAFehlrMaterial],[FaHomeAnalysis],[FaHomeUpdate],[FaHoursMovement],[FALaufkarteSchneiderei],[FaPlanningEdit],[FaPlanningView],[FaPlanningViolation],[FAPlannung],[FAPlannungHistory],[FAPlannungHistoryForceAgent],[FAPlannungHistoryXLSExport],[FAPlannungHistoryXLSImport],[FAPlannungTechnick],[FAPriesZeitUpdate],[FAProductionPlannung],[FAStappleDruck],[FAStatusAlbania],[FAStatusCzech],[FAStatusTunisia],[FAStorno],[FAStucklist],[FaTechnicEdit],[FaTechnicView],[FATerminWerk],[FAUpdateBemerkungExtern],[FAUpdateByArticle],[FAUpdateByFA],[FAUpdatePrio],[FAUpdateTerminHorizon1],[FAUpdateTerminHorizon2],[FAUpdateTerminHorizon3],[FAWerkWunshAdmin],[Fertigung],[FertigungLog],[Forecast],[ForecastCreate],[ForecastDelete],[ForecastStatistics],[ImportFGXlsHistory],[isDefault],[LogsFGXlsHistory],[ModuleActivated],[ProductionPlanningByCustomer],[Statistics],[StatsAnteileingelasteteFA],[StatsBacklogFG],[StatsCapaHorizons],[StatsCapaPlanning],[StatsCreatedInvoices],[StatsDeliveries],[StatsFAAnderungshistoire],[StatsLagerbestandFGCRP],[StatsRahmenSale],[StatsStockCS],[StatsStockExternalWarehouse],[StatsStockFG],[SystemLogs],[UBGStatusChange],[ViewFGBestandHistory]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@Administration,@AgentFGXlsHistory,@AllFaChanges,@AllProductionWarehouses,@Configuration,@ConfigurationAppoitments,@ConfigurationChangeEmployees,@ConfigurationReplacements,@CreationTime,@CreationUserId,@CRPDashboardActiveArticles,@CRPDashboardCancelledOrders,@CRPDashboardCreatedOrders,@CRPDashboardOpenOrders,@CRPDashboardOpenOrdersHours,@CRPDashboardTotalStockFG,@CRPFAPlannung,@CRPPlanning,@CRPRequirement,@CRPUBGPlanning,@DelforCreate,@DelforDelete,@DelforDeletePosition,@DelforOrderConfirmation,@DelforReport,@DelforStatistics,@DelforView,@DLFPosHorizon1,@DLFPosHorizon2,@DLFPosHorizon3,@ExportFGXlsHistory,@FaABEdit,@FaABView,@FaActionBook,@FaActionComplete,@FaActionDelete,@FaActionPrint,@FaAdmin,@FAAKtualTerminUpdate,@FaAnalysis,@FAAuswertungEndkontrolle,@FABemerkungPlannug,@FABemerkungZuGewerk,@FABemerkungZuPrio,@FACancelHorizon1,@FACancelHorizon2,@FACancelHorizon3,@FACommissionert,@FaCreate,@FACreateHorizon1,@FACreateHorizon2,@FACreateHorizon3,@FaDateChangeHistory,@FaDatenEdit,@FaDatenView,@FaDelete,@FADrucken,@FaEdit,@FAErlidegen,@FAExcelUpdateWerk,@FAExcelUpdateWunsh,@FAFehlrMaterial,@FaHomeAnalysis,@FaHomeUpdate,@FaHoursMovement,@FALaufkarteSchneiderei,@FaPlanningEdit,@FaPlanningView,@FaPlanningViolation,@FAPlannung,@FAPlannungHistory,@FAPlannungHistoryForceAgent,@FAPlannungHistoryXLSExport,@FAPlannungHistoryXLSImport,@FAPlannungTechnick,@FAPriesZeitUpdate,@FAProductionPlannung,@FAStappleDruck,@FAStatusAlbania,@FAStatusCzech,@FAStatusTunisia,@FAStorno,@FAStucklist,@FaTechnicEdit,@FaTechnicView,@FATerminWerk,@FAUpdateBemerkungExtern,@FAUpdateByArticle,@FAUpdateByFA,@FAUpdatePrio,@FAUpdateTerminHorizon1,@FAUpdateTerminHorizon2,@FAUpdateTerminHorizon3,@FAWerkWunshAdmin,@Fertigung,@FertigungLog,@Forecast,@ForecastCreate,@ForecastDelete,@ForecastStatistics,@ImportFGXlsHistory,@isDefault,@LogsFGXlsHistory,@ModuleActivated,@ProductionPlanningByCustomer,@Statistics,@StatsAnteileingelasteteFA,@StatsBacklogFG,@StatsCapaHorizons,@StatsCapaPlanning,@StatsCreatedInvoices,@StatsDeliveries,@StatsFAAnderungshistoire,@StatsLagerbestandFGCRP,@StatsRahmenSale,@StatsStockCS,@StatsStockExternalWarehouse,@StatsStockFG,@SystemLogs,@UBGStatusChange,@ViewFGBestandHistory); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AgentFGXlsHistory", item.AgentFGXlsHistory == null ? (object)DBNull.Value : item.AgentFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("AllFaChanges", item.AllFaChanges == null ? (object)DBNull.Value : item.AllFaChanges);
					sqlCommand.Parameters.AddWithValue("AllProductionWarehouses", item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
					sqlCommand.Parameters.AddWithValue("Configuration", item.Configuration == null ? (object)DBNull.Value : item.Configuration);
					sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments", item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
					sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees", item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
					sqlCommand.Parameters.AddWithValue("ConfigurationReplacements", item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CRPDashboardActiveArticles", item.CRPDashboardActiveArticles == null ? (object)DBNull.Value : item.CRPDashboardActiveArticles);
					sqlCommand.Parameters.AddWithValue("CRPDashboardCancelledOrders", item.CRPDashboardCancelledOrders == null ? (object)DBNull.Value : item.CRPDashboardCancelledOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardCreatedOrders", item.CRPDashboardCreatedOrders == null ? (object)DBNull.Value : item.CRPDashboardCreatedOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrders", item.CRPDashboardOpenOrders == null ? (object)DBNull.Value : item.CRPDashboardOpenOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrdersHours", item.CRPDashboardOpenOrdersHours == null ? (object)DBNull.Value : item.CRPDashboardOpenOrdersHours);
					sqlCommand.Parameters.AddWithValue("CRPDashboardTotalStockFG", item.CRPDashboardTotalStockFG == null ? (object)DBNull.Value : item.CRPDashboardTotalStockFG);
					sqlCommand.Parameters.AddWithValue("CRPFAPlannung", item.CRPFAPlannung == null ? (object)DBNull.Value : item.CRPFAPlannung);
					sqlCommand.Parameters.AddWithValue("CRPPlanning", item.CRPPlanning == null ? (object)DBNull.Value : item.CRPPlanning);
					sqlCommand.Parameters.AddWithValue("CRPRequirement", item.CRPRequirement == null ? (object)DBNull.Value : item.CRPRequirement);
					sqlCommand.Parameters.AddWithValue("CRPUBGPlanning", item.CRPUBGPlanning == null ? (object)DBNull.Value : item.CRPUBGPlanning);
					sqlCommand.Parameters.AddWithValue("DelforCreate", item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
					sqlCommand.Parameters.AddWithValue("DelforDelete", item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
					sqlCommand.Parameters.AddWithValue("DelforDeletePosition", item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
					sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation", item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
					sqlCommand.Parameters.AddWithValue("DelforReport", item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
					sqlCommand.Parameters.AddWithValue("DelforStatistics", item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
					sqlCommand.Parameters.AddWithValue("DelforView", item.DelforView == null ? (object)DBNull.Value : item.DelforView);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon1", item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon2", item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon3", item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
					sqlCommand.Parameters.AddWithValue("ExportFGXlsHistory", item.ExportFGXlsHistory == null ? (object)DBNull.Value : item.ExportFGXlsHistory);
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
					sqlCommand.Parameters.AddWithValue("FaDateChangeHistory", item.FaDateChangeHistory == null ? (object)DBNull.Value : item.FaDateChangeHistory);
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
					sqlCommand.Parameters.AddWithValue("FaHoursMovement", item.FaHoursMovement == null ? (object)DBNull.Value : item.FaHoursMovement);
					sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei", item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
					sqlCommand.Parameters.AddWithValue("FaPlanningEdit", item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
					sqlCommand.Parameters.AddWithValue("FaPlanningView", item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
					sqlCommand.Parameters.AddWithValue("FaPlanningViolation", item.FaPlanningViolation == null ? (object)DBNull.Value : item.FaPlanningViolation);
					sqlCommand.Parameters.AddWithValue("FAPlannung", item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistory", item.FAPlannungHistory == null ? (object)DBNull.Value : item.FAPlannungHistory);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryForceAgent", item.FAPlannungHistoryForceAgent == null ? (object)DBNull.Value : item.FAPlannungHistoryForceAgent);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSExport", item.FAPlannungHistoryXLSExport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSExport);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSImport", item.FAPlannungHistoryXLSImport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSImport);
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
					sqlCommand.Parameters.AddWithValue("FAUpdatePrio", item.FAUpdatePrio == null ? (object)DBNull.Value : item.FAUpdatePrio);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1", item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2", item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3", item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
					sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin", item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
					sqlCommand.Parameters.AddWithValue("Fertigung", item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
					sqlCommand.Parameters.AddWithValue("FertigungLog", item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
					sqlCommand.Parameters.AddWithValue("Forecast", item.Forecast == null ? (object)DBNull.Value : item.Forecast);
					sqlCommand.Parameters.AddWithValue("ForecastCreate", item.ForecastCreate == null ? (object)DBNull.Value : item.ForecastCreate);
					sqlCommand.Parameters.AddWithValue("ForecastDelete", item.ForecastDelete == null ? (object)DBNull.Value : item.ForecastDelete);
					sqlCommand.Parameters.AddWithValue("ForecastStatistics", item.ForecastStatistics == null ? (object)DBNull.Value : item.ForecastStatistics);
					sqlCommand.Parameters.AddWithValue("ImportFGXlsHistory", item.ImportFGXlsHistory == null ? (object)DBNull.Value : item.ImportFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("LogsFGXlsHistory", item.LogsFGXlsHistory == null ? (object)DBNull.Value : item.LogsFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer", item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
					sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatsAnteileingelasteteFA", item.StatsAnteileingelasteteFA == null ? (object)DBNull.Value : item.StatsAnteileingelasteteFA);
					sqlCommand.Parameters.AddWithValue("StatsBacklogFG", item.StatsBacklogFG == null ? (object)DBNull.Value : item.StatsBacklogFG);
					sqlCommand.Parameters.AddWithValue("StatsCapaHorizons", item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
					sqlCommand.Parameters.AddWithValue("StatsCapaPlanning", item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
					sqlCommand.Parameters.AddWithValue("StatsCreatedInvoices", item.StatsCreatedInvoices == null ? (object)DBNull.Value : item.StatsCreatedInvoices);
					sqlCommand.Parameters.AddWithValue("StatsDeliveries", item.StatsDeliveries == null ? (object)DBNull.Value : item.StatsDeliveries);
					sqlCommand.Parameters.AddWithValue("StatsFAAnderungshistoire", item.StatsFAAnderungshistoire == null ? (object)DBNull.Value : item.StatsFAAnderungshistoire);
					sqlCommand.Parameters.AddWithValue("StatsLagerbestandFGCRP", item.StatsLagerbestandFGCRP == null ? (object)DBNull.Value : item.StatsLagerbestandFGCRP);
					sqlCommand.Parameters.AddWithValue("StatsRahmenSale", item.StatsRahmenSale == null ? (object)DBNull.Value : item.StatsRahmenSale);
					sqlCommand.Parameters.AddWithValue("StatsStockCS", item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
					sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse", item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
					sqlCommand.Parameters.AddWithValue("StatsStockFG", item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
					sqlCommand.Parameters.AddWithValue("SystemLogs", item.SystemLogs == null ? (object)DBNull.Value : item.SystemLogs);
					sqlCommand.Parameters.AddWithValue("UBGStatusChange", item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
					sqlCommand.Parameters.AddWithValue("ViewFGBestandHistory", item.ViewFGBestandHistory == null ? (object)DBNull.Value : item.ViewFGBestandHistory);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 124; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> items)
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
						query += " INSERT INTO [__CRP_AccessProfile] ([AccessProfileName],[Administration],[AgentFGXlsHistory],[AllFaChanges],[AllProductionWarehouses],[Configuration],[ConfigurationAppoitments],[ConfigurationChangeEmployees],[ConfigurationReplacements],[CreationTime],[CreationUserId],[CRPDashboardActiveArticles],[CRPDashboardCancelledOrders],[CRPDashboardCreatedOrders],[CRPDashboardOpenOrders],[CRPDashboardOpenOrdersHours],[CRPDashboardTotalStockFG],[CRPFAPlannung],[CRPPlanning],[CRPRequirement],[CRPUBGPlanning],[DelforCreate],[DelforDelete],[DelforDeletePosition],[DelforOrderConfirmation],[DelforReport],[DelforStatistics],[DelforView],[DLFPosHorizon1],[DLFPosHorizon2],[DLFPosHorizon3],[ExportFGXlsHistory],[FaABEdit],[FaABView],[FaActionBook],[FaActionComplete],[FaActionDelete],[FaActionPrint],[FaAdmin],[FAAKtualTerminUpdate],[FaAnalysis],[FAAuswertungEndkontrolle],[FABemerkungPlannug],[FABemerkungZuGewerk],[FABemerkungZuPrio],[FACancelHorizon1],[FACancelHorizon2],[FACancelHorizon3],[FACommissionert],[FaCreate],[FACreateHorizon1],[FACreateHorizon2],[FACreateHorizon3],[FaDateChangeHistory],[FaDatenEdit],[FaDatenView],[FaDelete],[FADrucken],[FaEdit],[FAErlidegen],[FAExcelUpdateWerk],[FAExcelUpdateWunsh],[FAFehlrMaterial],[FaHomeAnalysis],[FaHomeUpdate],[FaHoursMovement],[FALaufkarteSchneiderei],[FaPlanningEdit],[FaPlanningView],[FaPlanningViolation],[FAPlannung],[FAPlannungHistory],[FAPlannungHistoryForceAgent],[FAPlannungHistoryXLSExport],[FAPlannungHistoryXLSImport],[FAPlannungTechnick],[FAPriesZeitUpdate],[FAProductionPlannung],[FAStappleDruck],[FAStatusAlbania],[FAStatusCzech],[FAStatusTunisia],[FAStorno],[FAStucklist],[FaTechnicEdit],[FaTechnicView],[FATerminWerk],[FAUpdateBemerkungExtern],[FAUpdateByArticle],[FAUpdateByFA],[FAUpdatePrio],[FAUpdateTerminHorizon1],[FAUpdateTerminHorizon2],[FAUpdateTerminHorizon3],[FAWerkWunshAdmin],[Fertigung],[FertigungLog],[Forecast],[ForecastCreate],[ForecastDelete],[ForecastStatistics],[ImportFGXlsHistory],[isDefault],[LogsFGXlsHistory],[ModuleActivated],[ProductionPlanningByCustomer],[Statistics],[StatsAnteileingelasteteFA],[StatsBacklogFG],[StatsCapaHorizons],[StatsCapaPlanning],[StatsCreatedInvoices],[StatsDeliveries],[StatsFAAnderungshistoire],[StatsLagerbestandFGCRP],[StatsRahmenSale],[StatsStockCS],[StatsStockExternalWarehouse],[StatsStockFG],[SystemLogs],[UBGStatusChange],[ViewFGBestandHistory]) VALUES ( "

							+ "@AccessProfileName" + i + ","
							+ "@Administration" + i + ","
							+ "@AgentFGXlsHistory" + i + ","
							+ "@AllFaChanges" + i + ","
							+ "@AllProductionWarehouses" + i + ","
							+ "@Configuration" + i + ","
							+ "@ConfigurationAppoitments" + i + ","
							+ "@ConfigurationChangeEmployees" + i + ","
							+ "@ConfigurationReplacements" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CRPDashboardActiveArticles" + i + ","
							+ "@CRPDashboardCancelledOrders" + i + ","
							+ "@CRPDashboardCreatedOrders" + i + ","
							+ "@CRPDashboardOpenOrders" + i + ","
							+ "@CRPDashboardOpenOrdersHours" + i + ","
							+ "@CRPDashboardTotalStockFG" + i + ","
							+ "@CRPFAPlannung" + i + ","
							+ "@CRPPlanning" + i + ","
							+ "@CRPRequirement" + i + ","
							+ "@CRPUBGPlanning" + i + ","
							+ "@DelforCreate" + i + ","
							+ "@DelforDelete" + i + ","
							+ "@DelforDeletePosition" + i + ","
							+ "@DelforOrderConfirmation" + i + ","
							+ "@DelforReport" + i + ","
							+ "@DelforStatistics" + i + ","
							+ "@DelforView" + i + ","
							+ "@DLFPosHorizon1" + i + ","
							+ "@DLFPosHorizon2" + i + ","
							+ "@DLFPosHorizon3" + i + ","
							+ "@ExportFGXlsHistory" + i + ","
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
							+ "@FaDateChangeHistory" + i + ","
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
							+ "@FaHoursMovement" + i + ","
							+ "@FALaufkarteSchneiderei" + i + ","
							+ "@FaPlanningEdit" + i + ","
							+ "@FaPlanningView" + i + ","
							+ "@FaPlanningViolation" + i + ","
							+ "@FAPlannung" + i + ","
							+ "@FAPlannungHistory" + i + ","
							+ "@FAPlannungHistoryForceAgent" + i + ","
							+ "@FAPlannungHistoryXLSExport" + i + ","
							+ "@FAPlannungHistoryXLSImport" + i + ","
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
							+ "@FAUpdatePrio" + i + ","
							+ "@FAUpdateTerminHorizon1" + i + ","
							+ "@FAUpdateTerminHorizon2" + i + ","
							+ "@FAUpdateTerminHorizon3" + i + ","
							+ "@FAWerkWunshAdmin" + i + ","
							+ "@Fertigung" + i + ","
							+ "@FertigungLog" + i + ","
							+ "@Forecast" + i + ","
							+ "@ForecastCreate" + i + ","
							+ "@ForecastDelete" + i + ","
							+ "@ForecastStatistics" + i + ","
							+ "@ImportFGXlsHistory" + i + ","
							+ "@isDefault" + i + ","
							+ "@LogsFGXlsHistory" + i + ","
							+ "@ModuleActivated" + i + ","
							+ "@ProductionPlanningByCustomer" + i + ","
							+ "@Statistics" + i + ","
							+ "@StatsAnteileingelasteteFA" + i + ","
							+ "@StatsBacklogFG" + i + ","
							+ "@StatsCapaHorizons" + i + ","
							+ "@StatsCapaPlanning" + i + ","
							+ "@StatsCreatedInvoices" + i + ","
							+ "@StatsDeliveries" + i + ","
							+ "@StatsFAAnderungshistoire" + i + ","
							+ "@StatsLagerbestandFGCRP" + i + ","
							+ "@StatsRahmenSale" + i + ","
							+ "@StatsStockCS" + i + ","
							+ "@StatsStockExternalWarehouse" + i + ","
							+ "@StatsStockFG" + i + ","
							+ "@SystemLogs" + i + ","
							+ "@UBGStatusChange" + i + ","
							+ "@ViewFGBestandHistory" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
						sqlCommand.Parameters.AddWithValue("AgentFGXlsHistory" + i, item.AgentFGXlsHistory == null ? (object)DBNull.Value : item.AgentFGXlsHistory);
						sqlCommand.Parameters.AddWithValue("AllFaChanges" + i, item.AllFaChanges == null ? (object)DBNull.Value : item.AllFaChanges);
						sqlCommand.Parameters.AddWithValue("AllProductionWarehouses" + i, item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
						sqlCommand.Parameters.AddWithValue("Configuration" + i, item.Configuration == null ? (object)DBNull.Value : item.Configuration);
						sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments" + i, item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
						sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees" + i, item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
						sqlCommand.Parameters.AddWithValue("ConfigurationReplacements" + i, item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CRPDashboardActiveArticles" + i, item.CRPDashboardActiveArticles == null ? (object)DBNull.Value : item.CRPDashboardActiveArticles);
						sqlCommand.Parameters.AddWithValue("CRPDashboardCancelledOrders" + i, item.CRPDashboardCancelledOrders == null ? (object)DBNull.Value : item.CRPDashboardCancelledOrders);
						sqlCommand.Parameters.AddWithValue("CRPDashboardCreatedOrders" + i, item.CRPDashboardCreatedOrders == null ? (object)DBNull.Value : item.CRPDashboardCreatedOrders);
						sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrders" + i, item.CRPDashboardOpenOrders == null ? (object)DBNull.Value : item.CRPDashboardOpenOrders);
						sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrdersHours" + i, item.CRPDashboardOpenOrdersHours == null ? (object)DBNull.Value : item.CRPDashboardOpenOrdersHours);
						sqlCommand.Parameters.AddWithValue("CRPDashboardTotalStockFG" + i, item.CRPDashboardTotalStockFG == null ? (object)DBNull.Value : item.CRPDashboardTotalStockFG);
						sqlCommand.Parameters.AddWithValue("CRPFAPlannung" + i, item.CRPFAPlannung == null ? (object)DBNull.Value : item.CRPFAPlannung);
						sqlCommand.Parameters.AddWithValue("CRPPlanning" + i, item.CRPPlanning == null ? (object)DBNull.Value : item.CRPPlanning);
						sqlCommand.Parameters.AddWithValue("CRPRequirement" + i, item.CRPRequirement == null ? (object)DBNull.Value : item.CRPRequirement);
						sqlCommand.Parameters.AddWithValue("CRPUBGPlanning" + i, item.CRPUBGPlanning == null ? (object)DBNull.Value : item.CRPUBGPlanning);
						sqlCommand.Parameters.AddWithValue("DelforCreate" + i, item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
						sqlCommand.Parameters.AddWithValue("DelforDelete" + i, item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
						sqlCommand.Parameters.AddWithValue("DelforDeletePosition" + i, item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
						sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation" + i, item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
						sqlCommand.Parameters.AddWithValue("DelforReport" + i, item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
						sqlCommand.Parameters.AddWithValue("DelforStatistics" + i, item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
						sqlCommand.Parameters.AddWithValue("DelforView" + i, item.DelforView == null ? (object)DBNull.Value : item.DelforView);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon1" + i, item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon2" + i, item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon3" + i, item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
						sqlCommand.Parameters.AddWithValue("ExportFGXlsHistory" + i, item.ExportFGXlsHistory == null ? (object)DBNull.Value : item.ExportFGXlsHistory);
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
						sqlCommand.Parameters.AddWithValue("FaDateChangeHistory" + i, item.FaDateChangeHistory == null ? (object)DBNull.Value : item.FaDateChangeHistory);
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
						sqlCommand.Parameters.AddWithValue("FaHoursMovement" + i, item.FaHoursMovement == null ? (object)DBNull.Value : item.FaHoursMovement);
						sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei" + i, item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
						sqlCommand.Parameters.AddWithValue("FaPlanningEdit" + i, item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
						sqlCommand.Parameters.AddWithValue("FaPlanningView" + i, item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
						sqlCommand.Parameters.AddWithValue("FaPlanningViolation" + i, item.FaPlanningViolation == null ? (object)DBNull.Value : item.FaPlanningViolation);
						sqlCommand.Parameters.AddWithValue("FAPlannung" + i, item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
						sqlCommand.Parameters.AddWithValue("FAPlannungHistory" + i, item.FAPlannungHistory == null ? (object)DBNull.Value : item.FAPlannungHistory);
						sqlCommand.Parameters.AddWithValue("FAPlannungHistoryForceAgent" + i, item.FAPlannungHistoryForceAgent == null ? (object)DBNull.Value : item.FAPlannungHistoryForceAgent);
						sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSExport" + i, item.FAPlannungHistoryXLSExport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSExport);
						sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSImport" + i, item.FAPlannungHistoryXLSImport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSImport);
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
						sqlCommand.Parameters.AddWithValue("FAUpdatePrio" + i, item.FAUpdatePrio == null ? (object)DBNull.Value : item.FAUpdatePrio);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1" + i, item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2" + i, item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3" + i, item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
						sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin" + i, item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
						sqlCommand.Parameters.AddWithValue("Fertigung" + i, item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
						sqlCommand.Parameters.AddWithValue("FertigungLog" + i, item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
						sqlCommand.Parameters.AddWithValue("Forecast" + i, item.Forecast == null ? (object)DBNull.Value : item.Forecast);
						sqlCommand.Parameters.AddWithValue("ForecastCreate" + i, item.ForecastCreate == null ? (object)DBNull.Value : item.ForecastCreate);
						sqlCommand.Parameters.AddWithValue("ForecastDelete" + i, item.ForecastDelete == null ? (object)DBNull.Value : item.ForecastDelete);
						sqlCommand.Parameters.AddWithValue("ForecastStatistics" + i, item.ForecastStatistics == null ? (object)DBNull.Value : item.ForecastStatistics);
						sqlCommand.Parameters.AddWithValue("ImportFGXlsHistory" + i, item.ImportFGXlsHistory == null ? (object)DBNull.Value : item.ImportFGXlsHistory);
						sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
						sqlCommand.Parameters.AddWithValue("LogsFGXlsHistory" + i, item.LogsFGXlsHistory == null ? (object)DBNull.Value : item.LogsFGXlsHistory);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer" + i, item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
						sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
						sqlCommand.Parameters.AddWithValue("StatsAnteileingelasteteFA" + i, item.StatsAnteileingelasteteFA == null ? (object)DBNull.Value : item.StatsAnteileingelasteteFA);
						sqlCommand.Parameters.AddWithValue("StatsBacklogFG" + i, item.StatsBacklogFG == null ? (object)DBNull.Value : item.StatsBacklogFG);
						sqlCommand.Parameters.AddWithValue("StatsCapaHorizons" + i, item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
						sqlCommand.Parameters.AddWithValue("StatsCapaPlanning" + i, item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
						sqlCommand.Parameters.AddWithValue("StatsCreatedInvoices" + i, item.StatsCreatedInvoices == null ? (object)DBNull.Value : item.StatsCreatedInvoices);
						sqlCommand.Parameters.AddWithValue("StatsDeliveries" + i, item.StatsDeliveries == null ? (object)DBNull.Value : item.StatsDeliveries);
						sqlCommand.Parameters.AddWithValue("StatsFAAnderungshistoire" + i, item.StatsFAAnderungshistoire == null ? (object)DBNull.Value : item.StatsFAAnderungshistoire);
						sqlCommand.Parameters.AddWithValue("StatsLagerbestandFGCRP" + i, item.StatsLagerbestandFGCRP == null ? (object)DBNull.Value : item.StatsLagerbestandFGCRP);
						sqlCommand.Parameters.AddWithValue("StatsRahmenSale" + i, item.StatsRahmenSale == null ? (object)DBNull.Value : item.StatsRahmenSale);
						sqlCommand.Parameters.AddWithValue("StatsStockCS" + i, item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
						sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse" + i, item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
						sqlCommand.Parameters.AddWithValue("StatsStockFG" + i, item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
						sqlCommand.Parameters.AddWithValue("SystemLogs" + i, item.SystemLogs == null ? (object)DBNull.Value : item.SystemLogs);
						sqlCommand.Parameters.AddWithValue("UBGStatusChange" + i, item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
						sqlCommand.Parameters.AddWithValue("ViewFGBestandHistory" + i, item.ViewFGBestandHistory == null ? (object)DBNull.Value : item.ViewFGBestandHistory);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CRP_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [AgentFGXlsHistory]=@AgentFGXlsHistory, [AllFaChanges]=@AllFaChanges, [AllProductionWarehouses]=@AllProductionWarehouses, [Configuration]=@Configuration, [ConfigurationAppoitments]=@ConfigurationAppoitments, [ConfigurationChangeEmployees]=@ConfigurationChangeEmployees, [ConfigurationReplacements]=@ConfigurationReplacements, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CRPDashboardActiveArticles]=@CRPDashboardActiveArticles, [CRPDashboardCancelledOrders]=@CRPDashboardCancelledOrders, [CRPDashboardCreatedOrders]=@CRPDashboardCreatedOrders, [CRPDashboardOpenOrders]=@CRPDashboardOpenOrders, [CRPDashboardOpenOrdersHours]=@CRPDashboardOpenOrdersHours, [CRPDashboardTotalStockFG]=@CRPDashboardTotalStockFG, [CRPFAPlannung]=@CRPFAPlannung, [CRPPlanning]=@CRPPlanning, [CRPRequirement]=@CRPRequirement, [CRPUBGPlanning]=@CRPUBGPlanning, [DelforCreate]=@DelforCreate, [DelforDelete]=@DelforDelete, [DelforDeletePosition]=@DelforDeletePosition, [DelforOrderConfirmation]=@DelforOrderConfirmation, [DelforReport]=@DelforReport, [DelforStatistics]=@DelforStatistics, [DelforView]=@DelforView, [DLFPosHorizon1]=@DLFPosHorizon1, [DLFPosHorizon2]=@DLFPosHorizon2, [DLFPosHorizon3]=@DLFPosHorizon3, [ExportFGXlsHistory]=@ExportFGXlsHistory, [FaABEdit]=@FaABEdit, [FaABView]=@FaABView, [FaActionBook]=@FaActionBook, [FaActionComplete]=@FaActionComplete, [FaActionDelete]=@FaActionDelete, [FaActionPrint]=@FaActionPrint, [FaAdmin]=@FaAdmin, [FAAKtualTerminUpdate]=@FAAKtualTerminUpdate, [FaAnalysis]=@FaAnalysis, [FAAuswertungEndkontrolle]=@FAAuswertungEndkontrolle, [FABemerkungPlannug]=@FABemerkungPlannug, [FABemerkungZuGewerk]=@FABemerkungZuGewerk, [FABemerkungZuPrio]=@FABemerkungZuPrio, [FACancelHorizon1]=@FACancelHorizon1, [FACancelHorizon2]=@FACancelHorizon2, [FACancelHorizon3]=@FACancelHorizon3, [FACommissionert]=@FACommissionert, [FaCreate]=@FaCreate, [FACreateHorizon1]=@FACreateHorizon1, [FACreateHorizon2]=@FACreateHorizon2, [FACreateHorizon3]=@FACreateHorizon3, [FaDateChangeHistory]=@FaDateChangeHistory, [FaDatenEdit]=@FaDatenEdit, [FaDatenView]=@FaDatenView, [FaDelete]=@FaDelete, [FADrucken]=@FADrucken, [FaEdit]=@FaEdit, [FAErlidegen]=@FAErlidegen, [FAExcelUpdateWerk]=@FAExcelUpdateWerk, [FAExcelUpdateWunsh]=@FAExcelUpdateWunsh, [FAFehlrMaterial]=@FAFehlrMaterial, [FaHomeAnalysis]=@FaHomeAnalysis, [FaHomeUpdate]=@FaHomeUpdate, [FaHoursMovement]=@FaHoursMovement, [FALaufkarteSchneiderei]=@FALaufkarteSchneiderei, [FaPlanningEdit]=@FaPlanningEdit, [FaPlanningView]=@FaPlanningView, [FaPlanningViolation]=@FaPlanningViolation, [FAPlannung]=@FAPlannung, [FAPlannungHistory]=@FAPlannungHistory, [FAPlannungHistoryForceAgent]=@FAPlannungHistoryForceAgent, [FAPlannungHistoryXLSExport]=@FAPlannungHistoryXLSExport, [FAPlannungHistoryXLSImport]=@FAPlannungHistoryXLSImport, [FAPlannungTechnick]=@FAPlannungTechnick, [FAPriesZeitUpdate]=@FAPriesZeitUpdate, [FAProductionPlannung]=@FAProductionPlannung, [FAStappleDruck]=@FAStappleDruck, [FAStatusAlbania]=@FAStatusAlbania, [FAStatusCzech]=@FAStatusCzech, [FAStatusTunisia]=@FAStatusTunisia, [FAStorno]=@FAStorno, [FAStucklist]=@FAStucklist, [FaTechnicEdit]=@FaTechnicEdit, [FaTechnicView]=@FaTechnicView, [FATerminWerk]=@FATerminWerk, [FAUpdateBemerkungExtern]=@FAUpdateBemerkungExtern, [FAUpdateByArticle]=@FAUpdateByArticle, [FAUpdateByFA]=@FAUpdateByFA, [FAUpdatePrio]=@FAUpdatePrio, [FAUpdateTerminHorizon1]=@FAUpdateTerminHorizon1, [FAUpdateTerminHorizon2]=@FAUpdateTerminHorizon2, [FAUpdateTerminHorizon3]=@FAUpdateTerminHorizon3, [FAWerkWunshAdmin]=@FAWerkWunshAdmin, [Fertigung]=@Fertigung, [FertigungLog]=@FertigungLog, [Forecast]=@Forecast, [ForecastCreate]=@ForecastCreate, [ForecastDelete]=@ForecastDelete, [ForecastStatistics]=@ForecastStatistics, [ImportFGXlsHistory]=@ImportFGXlsHistory, [isDefault]=@isDefault, [LogsFGXlsHistory]=@LogsFGXlsHistory, [ModuleActivated]=@ModuleActivated, [ProductionPlanningByCustomer]=@ProductionPlanningByCustomer, [Statistics]=@Statistics, [StatsAnteileingelasteteFA]=@StatsAnteileingelasteteFA, [StatsBacklogFG]=@StatsBacklogFG, [StatsCapaHorizons]=@StatsCapaHorizons, [StatsCapaPlanning]=@StatsCapaPlanning, [StatsCreatedInvoices]=@StatsCreatedInvoices, [StatsDeliveries]=@StatsDeliveries, [StatsFAAnderungshistoire]=@StatsFAAnderungshistoire, [StatsLagerbestandFGCRP]=@StatsLagerbestandFGCRP, [StatsRahmenSale]=@StatsRahmenSale, [StatsStockCS]=@StatsStockCS, [StatsStockExternalWarehouse]=@StatsStockExternalWarehouse, [StatsStockFG]=@StatsStockFG, [SystemLogs]=@SystemLogs, [UBGStatusChange]=@UBGStatusChange, [ViewFGBestandHistory]=@ViewFGBestandHistory WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
				sqlCommand.Parameters.AddWithValue("AgentFGXlsHistory", item.AgentFGXlsHistory == null ? (object)DBNull.Value : item.AgentFGXlsHistory);
				sqlCommand.Parameters.AddWithValue("AllFaChanges", item.AllFaChanges == null ? (object)DBNull.Value : item.AllFaChanges);
				sqlCommand.Parameters.AddWithValue("AllProductionWarehouses", item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
				sqlCommand.Parameters.AddWithValue("Configuration", item.Configuration == null ? (object)DBNull.Value : item.Configuration);
				sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments", item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
				sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees", item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
				sqlCommand.Parameters.AddWithValue("ConfigurationReplacements", item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CRPDashboardActiveArticles", item.CRPDashboardActiveArticles == null ? (object)DBNull.Value : item.CRPDashboardActiveArticles);
				sqlCommand.Parameters.AddWithValue("CRPDashboardCancelledOrders", item.CRPDashboardCancelledOrders == null ? (object)DBNull.Value : item.CRPDashboardCancelledOrders);
				sqlCommand.Parameters.AddWithValue("CRPDashboardCreatedOrders", item.CRPDashboardCreatedOrders == null ? (object)DBNull.Value : item.CRPDashboardCreatedOrders);
				sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrders", item.CRPDashboardOpenOrders == null ? (object)DBNull.Value : item.CRPDashboardOpenOrders);
				sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrdersHours", item.CRPDashboardOpenOrdersHours == null ? (object)DBNull.Value : item.CRPDashboardOpenOrdersHours);
				sqlCommand.Parameters.AddWithValue("CRPDashboardTotalStockFG", item.CRPDashboardTotalStockFG == null ? (object)DBNull.Value : item.CRPDashboardTotalStockFG);
				sqlCommand.Parameters.AddWithValue("CRPFAPlannung", item.CRPFAPlannung == null ? (object)DBNull.Value : item.CRPFAPlannung);
				sqlCommand.Parameters.AddWithValue("CRPPlanning", item.CRPPlanning == null ? (object)DBNull.Value : item.CRPPlanning);
				sqlCommand.Parameters.AddWithValue("CRPRequirement", item.CRPRequirement == null ? (object)DBNull.Value : item.CRPRequirement);
				sqlCommand.Parameters.AddWithValue("CRPUBGPlanning", item.CRPUBGPlanning == null ? (object)DBNull.Value : item.CRPUBGPlanning);
				sqlCommand.Parameters.AddWithValue("DelforCreate", item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
				sqlCommand.Parameters.AddWithValue("DelforDelete", item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
				sqlCommand.Parameters.AddWithValue("DelforDeletePosition", item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
				sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation", item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
				sqlCommand.Parameters.AddWithValue("DelforReport", item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
				sqlCommand.Parameters.AddWithValue("DelforStatistics", item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
				sqlCommand.Parameters.AddWithValue("DelforView", item.DelforView == null ? (object)DBNull.Value : item.DelforView);
				sqlCommand.Parameters.AddWithValue("DLFPosHorizon1", item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
				sqlCommand.Parameters.AddWithValue("DLFPosHorizon2", item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
				sqlCommand.Parameters.AddWithValue("DLFPosHorizon3", item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
				sqlCommand.Parameters.AddWithValue("ExportFGXlsHistory", item.ExportFGXlsHistory == null ? (object)DBNull.Value : item.ExportFGXlsHistory);
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
				sqlCommand.Parameters.AddWithValue("FaDateChangeHistory", item.FaDateChangeHistory == null ? (object)DBNull.Value : item.FaDateChangeHistory);
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
				sqlCommand.Parameters.AddWithValue("FaHoursMovement", item.FaHoursMovement == null ? (object)DBNull.Value : item.FaHoursMovement);
				sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei", item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
				sqlCommand.Parameters.AddWithValue("FaPlanningEdit", item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
				sqlCommand.Parameters.AddWithValue("FaPlanningView", item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
				sqlCommand.Parameters.AddWithValue("FaPlanningViolation", item.FaPlanningViolation == null ? (object)DBNull.Value : item.FaPlanningViolation);
				sqlCommand.Parameters.AddWithValue("FAPlannung", item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
				sqlCommand.Parameters.AddWithValue("FAPlannungHistory", item.FAPlannungHistory == null ? (object)DBNull.Value : item.FAPlannungHistory);
				sqlCommand.Parameters.AddWithValue("FAPlannungHistoryForceAgent", item.FAPlannungHistoryForceAgent == null ? (object)DBNull.Value : item.FAPlannungHistoryForceAgent);
				sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSExport", item.FAPlannungHistoryXLSExport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSExport);
				sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSImport", item.FAPlannungHistoryXLSImport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSImport);
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
				sqlCommand.Parameters.AddWithValue("FAUpdatePrio", item.FAUpdatePrio == null ? (object)DBNull.Value : item.FAUpdatePrio);
				sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1", item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
				sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2", item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
				sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3", item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
				sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin", item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
				sqlCommand.Parameters.AddWithValue("Fertigung", item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
				sqlCommand.Parameters.AddWithValue("FertigungLog", item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
				sqlCommand.Parameters.AddWithValue("Forecast", item.Forecast == null ? (object)DBNull.Value : item.Forecast);
				sqlCommand.Parameters.AddWithValue("ForecastCreate", item.ForecastCreate == null ? (object)DBNull.Value : item.ForecastCreate);
				sqlCommand.Parameters.AddWithValue("ForecastDelete", item.ForecastDelete == null ? (object)DBNull.Value : item.ForecastDelete);
				sqlCommand.Parameters.AddWithValue("ForecastStatistics", item.ForecastStatistics == null ? (object)DBNull.Value : item.ForecastStatistics);
				sqlCommand.Parameters.AddWithValue("ImportFGXlsHistory", item.ImportFGXlsHistory == null ? (object)DBNull.Value : item.ImportFGXlsHistory);
				sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
				sqlCommand.Parameters.AddWithValue("LogsFGXlsHistory", item.LogsFGXlsHistory == null ? (object)DBNull.Value : item.LogsFGXlsHistory);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer", item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
				sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
				sqlCommand.Parameters.AddWithValue("StatsAnteileingelasteteFA", item.StatsAnteileingelasteteFA == null ? (object)DBNull.Value : item.StatsAnteileingelasteteFA);
				sqlCommand.Parameters.AddWithValue("StatsBacklogFG", item.StatsBacklogFG == null ? (object)DBNull.Value : item.StatsBacklogFG);
				sqlCommand.Parameters.AddWithValue("StatsCapaHorizons", item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
				sqlCommand.Parameters.AddWithValue("StatsCapaPlanning", item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
				sqlCommand.Parameters.AddWithValue("StatsCreatedInvoices", item.StatsCreatedInvoices == null ? (object)DBNull.Value : item.StatsCreatedInvoices);
				sqlCommand.Parameters.AddWithValue("StatsDeliveries", item.StatsDeliveries == null ? (object)DBNull.Value : item.StatsDeliveries);
				sqlCommand.Parameters.AddWithValue("StatsFAAnderungshistoire", item.StatsFAAnderungshistoire == null ? (object)DBNull.Value : item.StatsFAAnderungshistoire);
				sqlCommand.Parameters.AddWithValue("StatsLagerbestandFGCRP", item.StatsLagerbestandFGCRP == null ? (object)DBNull.Value : item.StatsLagerbestandFGCRP);
				sqlCommand.Parameters.AddWithValue("StatsRahmenSale", item.StatsRahmenSale == null ? (object)DBNull.Value : item.StatsRahmenSale);
				sqlCommand.Parameters.AddWithValue("StatsStockCS", item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
				sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse", item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
				sqlCommand.Parameters.AddWithValue("StatsStockFG", item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
				sqlCommand.Parameters.AddWithValue("SystemLogs", item.SystemLogs == null ? (object)DBNull.Value : item.SystemLogs);
				sqlCommand.Parameters.AddWithValue("UBGStatusChange", item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
				sqlCommand.Parameters.AddWithValue("ViewFGBestandHistory", item.ViewFGBestandHistory == null ? (object)DBNull.Value : item.ViewFGBestandHistory);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 124; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> items)
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
						query += " UPDATE [__CRP_AccessProfile] SET "

							+ "[AccessProfileName]=@AccessProfileName" + i + ","
							+ "[Administration]=@Administration" + i + ","
							+ "[AgentFGXlsHistory]=@AgentFGXlsHistory" + i + ","
							+ "[AllFaChanges]=@AllFaChanges" + i + ","
							+ "[AllProductionWarehouses]=@AllProductionWarehouses" + i + ","
							+ "[Configuration]=@Configuration" + i + ","
							+ "[ConfigurationAppoitments]=@ConfigurationAppoitments" + i + ","
							+ "[ConfigurationChangeEmployees]=@ConfigurationChangeEmployees" + i + ","
							+ "[ConfigurationReplacements]=@ConfigurationReplacements" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CRPDashboardActiveArticles]=@CRPDashboardActiveArticles" + i + ","
							+ "[CRPDashboardCancelledOrders]=@CRPDashboardCancelledOrders" + i + ","
							+ "[CRPDashboardCreatedOrders]=@CRPDashboardCreatedOrders" + i + ","
							+ "[CRPDashboardOpenOrders]=@CRPDashboardOpenOrders" + i + ","
							+ "[CRPDashboardOpenOrdersHours]=@CRPDashboardOpenOrdersHours" + i + ","
							+ "[CRPDashboardTotalStockFG]=@CRPDashboardTotalStockFG" + i + ","
							+ "[CRPFAPlannung]=@CRPFAPlannung" + i + ","
							+ "[CRPPlanning]=@CRPPlanning" + i + ","
							+ "[CRPRequirement]=@CRPRequirement" + i + ","
							+ "[CRPUBGPlanning]=@CRPUBGPlanning" + i + ","
							+ "[DelforCreate]=@DelforCreate" + i + ","
							+ "[DelforDelete]=@DelforDelete" + i + ","
							+ "[DelforDeletePosition]=@DelforDeletePosition" + i + ","
							+ "[DelforOrderConfirmation]=@DelforOrderConfirmation" + i + ","
							+ "[DelforReport]=@DelforReport" + i + ","
							+ "[DelforStatistics]=@DelforStatistics" + i + ","
							+ "[DelforView]=@DelforView" + i + ","
							+ "[DLFPosHorizon1]=@DLFPosHorizon1" + i + ","
							+ "[DLFPosHorizon2]=@DLFPosHorizon2" + i + ","
							+ "[DLFPosHorizon3]=@DLFPosHorizon3" + i + ","
							+ "[ExportFGXlsHistory]=@ExportFGXlsHistory" + i + ","
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
							+ "[FaDateChangeHistory]=@FaDateChangeHistory" + i + ","
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
							+ "[FaHoursMovement]=@FaHoursMovement" + i + ","
							+ "[FALaufkarteSchneiderei]=@FALaufkarteSchneiderei" + i + ","
							+ "[FaPlanningEdit]=@FaPlanningEdit" + i + ","
							+ "[FaPlanningView]=@FaPlanningView" + i + ","
							+ "[FaPlanningViolation]=@FaPlanningViolation" + i + ","
							+ "[FAPlannung]=@FAPlannung" + i + ","
							+ "[FAPlannungHistory]=@FAPlannungHistory" + i + ","
							+ "[FAPlannungHistoryForceAgent]=@FAPlannungHistoryForceAgent" + i + ","
							+ "[FAPlannungHistoryXLSExport]=@FAPlannungHistoryXLSExport" + i + ","
							+ "[FAPlannungHistoryXLSImport]=@FAPlannungHistoryXLSImport" + i + ","
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
							+ "[FAUpdatePrio]=@FAUpdatePrio" + i + ","
							+ "[FAUpdateTerminHorizon1]=@FAUpdateTerminHorizon1" + i + ","
							+ "[FAUpdateTerminHorizon2]=@FAUpdateTerminHorizon2" + i + ","
							+ "[FAUpdateTerminHorizon3]=@FAUpdateTerminHorizon3" + i + ","
							+ "[FAWerkWunshAdmin]=@FAWerkWunshAdmin" + i + ","
							+ "[Fertigung]=@Fertigung" + i + ","
							+ "[FertigungLog]=@FertigungLog" + i + ","
							+ "[Forecast]=@Forecast" + i + ","
							+ "[ForecastCreate]=@ForecastCreate" + i + ","
							+ "[ForecastDelete]=@ForecastDelete" + i + ","
							+ "[ForecastStatistics]=@ForecastStatistics" + i + ","
							+ "[ImportFGXlsHistory]=@ImportFGXlsHistory" + i + ","
							+ "[isDefault]=@isDefault" + i + ","
							+ "[LogsFGXlsHistory]=@LogsFGXlsHistory" + i + ","
							+ "[ModuleActivated]=@ModuleActivated" + i + ","
							+ "[ProductionPlanningByCustomer]=@ProductionPlanningByCustomer" + i + ","
							+ "[Statistics]=@Statistics" + i + ","
							+ "[StatsAnteileingelasteteFA]=@StatsAnteileingelasteteFA" + i + ","
							+ "[StatsBacklogFG]=@StatsBacklogFG" + i + ","
							+ "[StatsCapaHorizons]=@StatsCapaHorizons" + i + ","
							+ "[StatsCapaPlanning]=@StatsCapaPlanning" + i + ","
							+ "[StatsCreatedInvoices]=@StatsCreatedInvoices" + i + ","
							+ "[StatsDeliveries]=@StatsDeliveries" + i + ","
							+ "[StatsFAAnderungshistoire]=@StatsFAAnderungshistoire" + i + ","
							+ "[StatsLagerbestandFGCRP]=@StatsLagerbestandFGCRP" + i + ","
							+ "[StatsRahmenSale]=@StatsRahmenSale" + i + ","
							+ "[StatsStockCS]=@StatsStockCS" + i + ","
							+ "[StatsStockExternalWarehouse]=@StatsStockExternalWarehouse" + i + ","
							+ "[StatsStockFG]=@StatsStockFG" + i + ","
							+ "[SystemLogs]=@SystemLogs" + i + ","
							+ "[UBGStatusChange]=@UBGStatusChange" + i + ","
							+ "[ViewFGBestandHistory]=@ViewFGBestandHistory" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
						sqlCommand.Parameters.AddWithValue("AgentFGXlsHistory" + i, item.AgentFGXlsHistory == null ? (object)DBNull.Value : item.AgentFGXlsHistory);
						sqlCommand.Parameters.AddWithValue("AllFaChanges" + i, item.AllFaChanges == null ? (object)DBNull.Value : item.AllFaChanges);
						sqlCommand.Parameters.AddWithValue("AllProductionWarehouses" + i, item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
						sqlCommand.Parameters.AddWithValue("Configuration" + i, item.Configuration == null ? (object)DBNull.Value : item.Configuration);
						sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments" + i, item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
						sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees" + i, item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
						sqlCommand.Parameters.AddWithValue("ConfigurationReplacements" + i, item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CRPDashboardActiveArticles" + i, item.CRPDashboardActiveArticles == null ? (object)DBNull.Value : item.CRPDashboardActiveArticles);
						sqlCommand.Parameters.AddWithValue("CRPDashboardCancelledOrders" + i, item.CRPDashboardCancelledOrders == null ? (object)DBNull.Value : item.CRPDashboardCancelledOrders);
						sqlCommand.Parameters.AddWithValue("CRPDashboardCreatedOrders" + i, item.CRPDashboardCreatedOrders == null ? (object)DBNull.Value : item.CRPDashboardCreatedOrders);
						sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrders" + i, item.CRPDashboardOpenOrders == null ? (object)DBNull.Value : item.CRPDashboardOpenOrders);
						sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrdersHours" + i, item.CRPDashboardOpenOrdersHours == null ? (object)DBNull.Value : item.CRPDashboardOpenOrdersHours);
						sqlCommand.Parameters.AddWithValue("CRPDashboardTotalStockFG" + i, item.CRPDashboardTotalStockFG == null ? (object)DBNull.Value : item.CRPDashboardTotalStockFG);
						sqlCommand.Parameters.AddWithValue("CRPFAPlannung" + i, item.CRPFAPlannung == null ? (object)DBNull.Value : item.CRPFAPlannung);
						sqlCommand.Parameters.AddWithValue("CRPPlanning" + i, item.CRPPlanning == null ? (object)DBNull.Value : item.CRPPlanning);
						sqlCommand.Parameters.AddWithValue("CRPRequirement" + i, item.CRPRequirement == null ? (object)DBNull.Value : item.CRPRequirement);
						sqlCommand.Parameters.AddWithValue("CRPUBGPlanning" + i, item.CRPUBGPlanning == null ? (object)DBNull.Value : item.CRPUBGPlanning);
						sqlCommand.Parameters.AddWithValue("DelforCreate" + i, item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
						sqlCommand.Parameters.AddWithValue("DelforDelete" + i, item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
						sqlCommand.Parameters.AddWithValue("DelforDeletePosition" + i, item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
						sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation" + i, item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
						sqlCommand.Parameters.AddWithValue("DelforReport" + i, item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
						sqlCommand.Parameters.AddWithValue("DelforStatistics" + i, item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
						sqlCommand.Parameters.AddWithValue("DelforView" + i, item.DelforView == null ? (object)DBNull.Value : item.DelforView);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon1" + i, item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon2" + i, item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
						sqlCommand.Parameters.AddWithValue("DLFPosHorizon3" + i, item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
						sqlCommand.Parameters.AddWithValue("ExportFGXlsHistory" + i, item.ExportFGXlsHistory == null ? (object)DBNull.Value : item.ExportFGXlsHistory);
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
						sqlCommand.Parameters.AddWithValue("FaDateChangeHistory" + i, item.FaDateChangeHistory == null ? (object)DBNull.Value : item.FaDateChangeHistory);
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
						sqlCommand.Parameters.AddWithValue("FaHoursMovement" + i, item.FaHoursMovement == null ? (object)DBNull.Value : item.FaHoursMovement);
						sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei" + i, item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
						sqlCommand.Parameters.AddWithValue("FaPlanningEdit" + i, item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
						sqlCommand.Parameters.AddWithValue("FaPlanningView" + i, item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
						sqlCommand.Parameters.AddWithValue("FaPlanningViolation" + i, item.FaPlanningViolation == null ? (object)DBNull.Value : item.FaPlanningViolation);
						sqlCommand.Parameters.AddWithValue("FAPlannung" + i, item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
						sqlCommand.Parameters.AddWithValue("FAPlannungHistory" + i, item.FAPlannungHistory == null ? (object)DBNull.Value : item.FAPlannungHistory);
						sqlCommand.Parameters.AddWithValue("FAPlannungHistoryForceAgent" + i, item.FAPlannungHistoryForceAgent == null ? (object)DBNull.Value : item.FAPlannungHistoryForceAgent);
						sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSExport" + i, item.FAPlannungHistoryXLSExport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSExport);
						sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSImport" + i, item.FAPlannungHistoryXLSImport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSImport);
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
						sqlCommand.Parameters.AddWithValue("FAUpdatePrio" + i, item.FAUpdatePrio == null ? (object)DBNull.Value : item.FAUpdatePrio);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1" + i, item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2" + i, item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
						sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3" + i, item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
						sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin" + i, item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
						sqlCommand.Parameters.AddWithValue("Fertigung" + i, item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
						sqlCommand.Parameters.AddWithValue("FertigungLog" + i, item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
						sqlCommand.Parameters.AddWithValue("Forecast" + i, item.Forecast == null ? (object)DBNull.Value : item.Forecast);
						sqlCommand.Parameters.AddWithValue("ForecastCreate" + i, item.ForecastCreate == null ? (object)DBNull.Value : item.ForecastCreate);
						sqlCommand.Parameters.AddWithValue("ForecastDelete" + i, item.ForecastDelete == null ? (object)DBNull.Value : item.ForecastDelete);
						sqlCommand.Parameters.AddWithValue("ForecastStatistics" + i, item.ForecastStatistics == null ? (object)DBNull.Value : item.ForecastStatistics);
						sqlCommand.Parameters.AddWithValue("ImportFGXlsHistory" + i, item.ImportFGXlsHistory == null ? (object)DBNull.Value : item.ImportFGXlsHistory);
						sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
						sqlCommand.Parameters.AddWithValue("LogsFGXlsHistory" + i, item.LogsFGXlsHistory == null ? (object)DBNull.Value : item.LogsFGXlsHistory);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer" + i, item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
						sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
						sqlCommand.Parameters.AddWithValue("StatsAnteileingelasteteFA" + i, item.StatsAnteileingelasteteFA == null ? (object)DBNull.Value : item.StatsAnteileingelasteteFA);
						sqlCommand.Parameters.AddWithValue("StatsBacklogFG" + i, item.StatsBacklogFG == null ? (object)DBNull.Value : item.StatsBacklogFG);
						sqlCommand.Parameters.AddWithValue("StatsCapaHorizons" + i, item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
						sqlCommand.Parameters.AddWithValue("StatsCapaPlanning" + i, item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
						sqlCommand.Parameters.AddWithValue("StatsCreatedInvoices" + i, item.StatsCreatedInvoices == null ? (object)DBNull.Value : item.StatsCreatedInvoices);
						sqlCommand.Parameters.AddWithValue("StatsDeliveries" + i, item.StatsDeliveries == null ? (object)DBNull.Value : item.StatsDeliveries);
						sqlCommand.Parameters.AddWithValue("StatsFAAnderungshistoire" + i, item.StatsFAAnderungshistoire == null ? (object)DBNull.Value : item.StatsFAAnderungshistoire);
						sqlCommand.Parameters.AddWithValue("StatsLagerbestandFGCRP" + i, item.StatsLagerbestandFGCRP == null ? (object)DBNull.Value : item.StatsLagerbestandFGCRP);
						sqlCommand.Parameters.AddWithValue("StatsRahmenSale" + i, item.StatsRahmenSale == null ? (object)DBNull.Value : item.StatsRahmenSale);
						sqlCommand.Parameters.AddWithValue("StatsStockCS" + i, item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
						sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse" + i, item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
						sqlCommand.Parameters.AddWithValue("StatsStockFG" + i, item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
						sqlCommand.Parameters.AddWithValue("SystemLogs" + i, item.SystemLogs == null ? (object)DBNull.Value : item.SystemLogs);
						sqlCommand.Parameters.AddWithValue("UBGStatusChange" + i, item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
						sqlCommand.Parameters.AddWithValue("ViewFGBestandHistory" + i, item.ViewFGBestandHistory == null ? (object)DBNull.Value : item.ViewFGBestandHistory);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
				string query = "DELETE FROM [__CRP_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [__CRP_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CRP_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CRP_AccessProfile]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__CRP_AccessProfile] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__CRP_AccessProfile] ([AccessProfileName],[Administration],[AgentFGXlsHistory],[AllFaChanges],[AllProductionWarehouses],[Configuration],[ConfigurationAppoitments],[ConfigurationChangeEmployees],[ConfigurationReplacements],[CreationTime],[CreationUserId],[CRPDashboardActiveArticles],[CRPDashboardCancelledOrders],[CRPDashboardCreatedOrders],[CRPDashboardOpenOrders],[CRPDashboardOpenOrdersHours],[CRPDashboardTotalStockFG],[CRPFAPlannung],[CRPPlanning],[CRPRequirement],[CRPUBGPlanning],[DelforCreate],[DelforDelete],[DelforDeletePosition],[DelforOrderConfirmation],[DelforReport],[DelforStatistics],[DelforView],[DLFPosHorizon1],[DLFPosHorizon2],[DLFPosHorizon3],[ExportFGXlsHistory],[FaABEdit],[FaABView],[FaActionBook],[FaActionComplete],[FaActionDelete],[FaActionPrint],[FaAdmin],[FAAKtualTerminUpdate],[FaAnalysis],[FAAuswertungEndkontrolle],[FABemerkungPlannug],[FABemerkungZuGewerk],[FABemerkungZuPrio],[FACancelHorizon1],[FACancelHorizon2],[FACancelHorizon3],[FACommissionert],[FaCreate],[FACreateHorizon1],[FACreateHorizon2],[FACreateHorizon3],[FaDateChangeHistory],[FaDatenEdit],[FaDatenView],[FaDelete],[FADrucken],[FaEdit],[FAErlidegen],[FAExcelUpdateWerk],[FAExcelUpdateWunsh],[FAFehlrMaterial],[FaHomeAnalysis],[FaHomeUpdate],[FaHoursMovement],[FALaufkarteSchneiderei],[FaPlanningEdit],[FaPlanningView],[FaPlanningViolation],[FAPlannung],[FAPlannungHistory],[FAPlannungHistoryForceAgent],[FAPlannungHistoryXLSExport],[FAPlannungHistoryXLSImport],[FAPlannungTechnick],[FAPriesZeitUpdate],[FAProductionPlannung],[FAStappleDruck],[FAStatusAlbania],[FAStatusCzech],[FAStatusTunisia],[FAStorno],[FAStucklist],[FaTechnicEdit],[FaTechnicView],[FATerminWerk],[FAUpdateBemerkungExtern],[FAUpdateByArticle],[FAUpdateByFA],[FAUpdatePrio],[FAUpdateTerminHorizon1],[FAUpdateTerminHorizon2],[FAUpdateTerminHorizon3],[FAWerkWunshAdmin],[Fertigung],[FertigungLog],[Forecast],[ForecastCreate],[ForecastDelete],[ForecastStatistics],[ImportFGXlsHistory],[isDefault],[LogsFGXlsHistory],[ModuleActivated],[ProductionPlanningByCustomer],[Statistics],[StatsAnteileingelasteteFA],[StatsBacklogFG],[StatsCapaHorizons],[StatsCapaPlanning],[StatsCreatedInvoices],[StatsDeliveries],[StatsFAAnderungshistoire],[StatsLagerbestandFGCRP],[StatsRahmenSale],[StatsStockCS],[StatsStockExternalWarehouse],[StatsStockFG],[SystemLogs],[UBGStatusChange],[ViewFGBestandHistory]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@Administration,@AgentFGXlsHistory,@AllFaChanges,@AllProductionWarehouses,@Configuration,@ConfigurationAppoitments,@ConfigurationChangeEmployees,@ConfigurationReplacements,@CreationTime,@CreationUserId,@CRPDashboardActiveArticles,@CRPDashboardCancelledOrders,@CRPDashboardCreatedOrders,@CRPDashboardOpenOrders,@CRPDashboardOpenOrdersHours,@CRPDashboardTotalStockFG,@CRPFAPlannung,@CRPPlanning,@CRPRequirement,@CRPUBGPlanning,@DelforCreate,@DelforDelete,@DelforDeletePosition,@DelforOrderConfirmation,@DelforReport,@DelforStatistics,@DelforView,@DLFPosHorizon1,@DLFPosHorizon2,@DLFPosHorizon3,@ExportFGXlsHistory,@FaABEdit,@FaABView,@FaActionBook,@FaActionComplete,@FaActionDelete,@FaActionPrint,@FaAdmin,@FAAKtualTerminUpdate,@FaAnalysis,@FAAuswertungEndkontrolle,@FABemerkungPlannug,@FABemerkungZuGewerk,@FABemerkungZuPrio,@FACancelHorizon1,@FACancelHorizon2,@FACancelHorizon3,@FACommissionert,@FaCreate,@FACreateHorizon1,@FACreateHorizon2,@FACreateHorizon3,@FaDateChangeHistory,@FaDatenEdit,@FaDatenView,@FaDelete,@FADrucken,@FaEdit,@FAErlidegen,@FAExcelUpdateWerk,@FAExcelUpdateWunsh,@FAFehlrMaterial,@FaHomeAnalysis,@FaHomeUpdate,@FaHoursMovement,@FALaufkarteSchneiderei,@FaPlanningEdit,@FaPlanningView,@FaPlanningViolation,@FAPlannung,@FAPlannungHistory,@FAPlannungHistoryForceAgent,@FAPlannungHistoryXLSExport,@FAPlannungHistoryXLSImport,@FAPlannungTechnick,@FAPriesZeitUpdate,@FAProductionPlannung,@FAStappleDruck,@FAStatusAlbania,@FAStatusCzech,@FAStatusTunisia,@FAStorno,@FAStucklist,@FaTechnicEdit,@FaTechnicView,@FATerminWerk,@FAUpdateBemerkungExtern,@FAUpdateByArticle,@FAUpdateByFA,@FAUpdatePrio,@FAUpdateTerminHorizon1,@FAUpdateTerminHorizon2,@FAUpdateTerminHorizon3,@FAWerkWunshAdmin,@Fertigung,@FertigungLog,@Forecast,@ForecastCreate,@ForecastDelete,@ForecastStatistics,@ImportFGXlsHistory,@isDefault,@LogsFGXlsHistory,@ModuleActivated,@ProductionPlanningByCustomer,@Statistics,@StatsAnteileingelasteteFA,@StatsBacklogFG,@StatsCapaHorizons,@StatsCapaPlanning,@StatsCreatedInvoices,@StatsDeliveries,@StatsFAAnderungshistoire,@StatsLagerbestandFGCRP,@StatsRahmenSale,@StatsStockCS,@StatsStockExternalWarehouse,@StatsStockFG,@SystemLogs,@UBGStatusChange,@ViewFGBestandHistory); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
			sqlCommand.Parameters.AddWithValue("AgentFGXlsHistory", item.AgentFGXlsHistory == null ? (object)DBNull.Value : item.AgentFGXlsHistory);
			sqlCommand.Parameters.AddWithValue("AllFaChanges", item.AllFaChanges == null ? (object)DBNull.Value : item.AllFaChanges);
			sqlCommand.Parameters.AddWithValue("AllProductionWarehouses", item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
			sqlCommand.Parameters.AddWithValue("Configuration", item.Configuration == null ? (object)DBNull.Value : item.Configuration);
			sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments", item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
			sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees", item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
			sqlCommand.Parameters.AddWithValue("ConfigurationReplacements", item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CRPDashboardActiveArticles", item.CRPDashboardActiveArticles == null ? (object)DBNull.Value : item.CRPDashboardActiveArticles);
			sqlCommand.Parameters.AddWithValue("CRPDashboardCancelledOrders", item.CRPDashboardCancelledOrders == null ? (object)DBNull.Value : item.CRPDashboardCancelledOrders);
			sqlCommand.Parameters.AddWithValue("CRPDashboardCreatedOrders", item.CRPDashboardCreatedOrders == null ? (object)DBNull.Value : item.CRPDashboardCreatedOrders);
			sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrders", item.CRPDashboardOpenOrders == null ? (object)DBNull.Value : item.CRPDashboardOpenOrders);
			sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrdersHours", item.CRPDashboardOpenOrdersHours == null ? (object)DBNull.Value : item.CRPDashboardOpenOrdersHours);
			sqlCommand.Parameters.AddWithValue("CRPDashboardTotalStockFG", item.CRPDashboardTotalStockFG == null ? (object)DBNull.Value : item.CRPDashboardTotalStockFG);
			sqlCommand.Parameters.AddWithValue("CRPFAPlannung", item.CRPFAPlannung == null ? (object)DBNull.Value : item.CRPFAPlannung);
			sqlCommand.Parameters.AddWithValue("CRPPlanning", item.CRPPlanning == null ? (object)DBNull.Value : item.CRPPlanning);
			sqlCommand.Parameters.AddWithValue("CRPRequirement", item.CRPRequirement == null ? (object)DBNull.Value : item.CRPRequirement);
			sqlCommand.Parameters.AddWithValue("CRPUBGPlanning", item.CRPUBGPlanning == null ? (object)DBNull.Value : item.CRPUBGPlanning);
			sqlCommand.Parameters.AddWithValue("DelforCreate", item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
			sqlCommand.Parameters.AddWithValue("DelforDelete", item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
			sqlCommand.Parameters.AddWithValue("DelforDeletePosition", item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
			sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation", item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
			sqlCommand.Parameters.AddWithValue("DelforReport", item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
			sqlCommand.Parameters.AddWithValue("DelforStatistics", item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
			sqlCommand.Parameters.AddWithValue("DelforView", item.DelforView == null ? (object)DBNull.Value : item.DelforView);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon1", item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon2", item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon3", item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
			sqlCommand.Parameters.AddWithValue("ExportFGXlsHistory", item.ExportFGXlsHistory == null ? (object)DBNull.Value : item.ExportFGXlsHistory);
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
			sqlCommand.Parameters.AddWithValue("FaDateChangeHistory", item.FaDateChangeHistory == null ? (object)DBNull.Value : item.FaDateChangeHistory);
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
			sqlCommand.Parameters.AddWithValue("FaHoursMovement", item.FaHoursMovement == null ? (object)DBNull.Value : item.FaHoursMovement);
			sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei", item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
			sqlCommand.Parameters.AddWithValue("FaPlanningEdit", item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
			sqlCommand.Parameters.AddWithValue("FaPlanningView", item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
			sqlCommand.Parameters.AddWithValue("FaPlanningViolation", item.FaPlanningViolation == null ? (object)DBNull.Value : item.FaPlanningViolation);
			sqlCommand.Parameters.AddWithValue("FAPlannung", item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
			sqlCommand.Parameters.AddWithValue("FAPlannungHistory", item.FAPlannungHistory == null ? (object)DBNull.Value : item.FAPlannungHistory);
			sqlCommand.Parameters.AddWithValue("FAPlannungHistoryForceAgent", item.FAPlannungHistoryForceAgent == null ? (object)DBNull.Value : item.FAPlannungHistoryForceAgent);
			sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSExport", item.FAPlannungHistoryXLSExport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSExport);
			sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSImport", item.FAPlannungHistoryXLSImport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSImport);
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
			sqlCommand.Parameters.AddWithValue("FAUpdatePrio", item.FAUpdatePrio == null ? (object)DBNull.Value : item.FAUpdatePrio);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1", item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2", item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3", item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
			sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin", item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
			sqlCommand.Parameters.AddWithValue("Fertigung", item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
			sqlCommand.Parameters.AddWithValue("FertigungLog", item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
			sqlCommand.Parameters.AddWithValue("Forecast", item.Forecast == null ? (object)DBNull.Value : item.Forecast);
			sqlCommand.Parameters.AddWithValue("ForecastCreate", item.ForecastCreate == null ? (object)DBNull.Value : item.ForecastCreate);
			sqlCommand.Parameters.AddWithValue("ForecastDelete", item.ForecastDelete == null ? (object)DBNull.Value : item.ForecastDelete);
			sqlCommand.Parameters.AddWithValue("ForecastStatistics", item.ForecastStatistics == null ? (object)DBNull.Value : item.ForecastStatistics);
			sqlCommand.Parameters.AddWithValue("ImportFGXlsHistory", item.ImportFGXlsHistory == null ? (object)DBNull.Value : item.ImportFGXlsHistory);
			sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
			sqlCommand.Parameters.AddWithValue("LogsFGXlsHistory", item.LogsFGXlsHistory == null ? (object)DBNull.Value : item.LogsFGXlsHistory);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer", item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
			sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
			sqlCommand.Parameters.AddWithValue("StatsAnteileingelasteteFA", item.StatsAnteileingelasteteFA == null ? (object)DBNull.Value : item.StatsAnteileingelasteteFA);
			sqlCommand.Parameters.AddWithValue("StatsBacklogFG", item.StatsBacklogFG == null ? (object)DBNull.Value : item.StatsBacklogFG);
			sqlCommand.Parameters.AddWithValue("StatsCapaHorizons", item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
			sqlCommand.Parameters.AddWithValue("StatsCapaPlanning", item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
			sqlCommand.Parameters.AddWithValue("StatsCreatedInvoices", item.StatsCreatedInvoices == null ? (object)DBNull.Value : item.StatsCreatedInvoices);
			sqlCommand.Parameters.AddWithValue("StatsDeliveries", item.StatsDeliveries == null ? (object)DBNull.Value : item.StatsDeliveries);
			sqlCommand.Parameters.AddWithValue("StatsFAAnderungshistoire", item.StatsFAAnderungshistoire == null ? (object)DBNull.Value : item.StatsFAAnderungshistoire);
			sqlCommand.Parameters.AddWithValue("StatsLagerbestandFGCRP", item.StatsLagerbestandFGCRP == null ? (object)DBNull.Value : item.StatsLagerbestandFGCRP);
			sqlCommand.Parameters.AddWithValue("StatsRahmenSale", item.StatsRahmenSale == null ? (object)DBNull.Value : item.StatsRahmenSale);
			sqlCommand.Parameters.AddWithValue("StatsStockCS", item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
			sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse", item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
			sqlCommand.Parameters.AddWithValue("StatsStockFG", item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
			sqlCommand.Parameters.AddWithValue("SystemLogs", item.SystemLogs == null ? (object)DBNull.Value : item.SystemLogs);
			sqlCommand.Parameters.AddWithValue("UBGStatusChange", item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
			sqlCommand.Parameters.AddWithValue("ViewFGBestandHistory", item.ViewFGBestandHistory == null ? (object)DBNull.Value : item.ViewFGBestandHistory);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 124; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__CRP_AccessProfile] ([AccessProfileName],[Administration],[AgentFGXlsHistory],[AllFaChanges],[AllProductionWarehouses],[Configuration],[ConfigurationAppoitments],[ConfigurationChangeEmployees],[ConfigurationReplacements],[CreationTime],[CreationUserId],[CRPDashboardActiveArticles],[CRPDashboardCancelledOrders],[CRPDashboardCreatedOrders],[CRPDashboardOpenOrders],[CRPDashboardOpenOrdersHours],[CRPDashboardTotalStockFG],[CRPFAPlannung],[CRPPlanning],[CRPRequirement],[CRPUBGPlanning],[DelforCreate],[DelforDelete],[DelforDeletePosition],[DelforOrderConfirmation],[DelforReport],[DelforStatistics],[DelforView],[DLFPosHorizon1],[DLFPosHorizon2],[DLFPosHorizon3],[ExportFGXlsHistory],[FaABEdit],[FaABView],[FaActionBook],[FaActionComplete],[FaActionDelete],[FaActionPrint],[FaAdmin],[FAAKtualTerminUpdate],[FaAnalysis],[FAAuswertungEndkontrolle],[FABemerkungPlannug],[FABemerkungZuGewerk],[FABemerkungZuPrio],[FACancelHorizon1],[FACancelHorizon2],[FACancelHorizon3],[FACommissionert],[FaCreate],[FACreateHorizon1],[FACreateHorizon2],[FACreateHorizon3],[FaDateChangeHistory],[FaDatenEdit],[FaDatenView],[FaDelete],[FADrucken],[FaEdit],[FAErlidegen],[FAExcelUpdateWerk],[FAExcelUpdateWunsh],[FAFehlrMaterial],[FaHomeAnalysis],[FaHomeUpdate],[FaHoursMovement],[FALaufkarteSchneiderei],[FaPlanningEdit],[FaPlanningView],[FaPlanningViolation],[FAPlannung],[FAPlannungHistory],[FAPlannungHistoryForceAgent],[FAPlannungHistoryXLSExport],[FAPlannungHistoryXLSImport],[FAPlannungTechnick],[FAPriesZeitUpdate],[FAProductionPlannung],[FAStappleDruck],[FAStatusAlbania],[FAStatusCzech],[FAStatusTunisia],[FAStorno],[FAStucklist],[FaTechnicEdit],[FaTechnicView],[FATerminWerk],[FAUpdateBemerkungExtern],[FAUpdateByArticle],[FAUpdateByFA],[FAUpdatePrio],[FAUpdateTerminHorizon1],[FAUpdateTerminHorizon2],[FAUpdateTerminHorizon3],[FAWerkWunshAdmin],[Fertigung],[FertigungLog],[Forecast],[ForecastCreate],[ForecastDelete],[ForecastStatistics],[ImportFGXlsHistory],[isDefault],[LogsFGXlsHistory],[ModuleActivated],[ProductionPlanningByCustomer],[Statistics],[StatsAnteileingelasteteFA],[StatsBacklogFG],[StatsCapaHorizons],[StatsCapaPlanning],[StatsCreatedInvoices],[StatsDeliveries],[StatsFAAnderungshistoire],[StatsLagerbestandFGCRP],[StatsRahmenSale],[StatsStockCS],[StatsStockExternalWarehouse],[StatsStockFG],[SystemLogs],[UBGStatusChange],[ViewFGBestandHistory]) VALUES ( "

						+ "@AccessProfileName" + i + ","
						+ "@Administration" + i + ","
						+ "@AgentFGXlsHistory" + i + ","
						+ "@AllFaChanges" + i + ","
						+ "@AllProductionWarehouses" + i + ","
						+ "@Configuration" + i + ","
						+ "@ConfigurationAppoitments" + i + ","
						+ "@ConfigurationChangeEmployees" + i + ","
						+ "@ConfigurationReplacements" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CRPDashboardActiveArticles" + i + ","
						+ "@CRPDashboardCancelledOrders" + i + ","
						+ "@CRPDashboardCreatedOrders" + i + ","
						+ "@CRPDashboardOpenOrders" + i + ","
						+ "@CRPDashboardOpenOrdersHours" + i + ","
						+ "@CRPDashboardTotalStockFG" + i + ","
						+ "@CRPFAPlannung" + i + ","
						+ "@CRPPlanning" + i + ","
						+ "@CRPRequirement" + i + ","
						+ "@CRPUBGPlanning" + i + ","
						+ "@DelforCreate" + i + ","
						+ "@DelforDelete" + i + ","
						+ "@DelforDeletePosition" + i + ","
						+ "@DelforOrderConfirmation" + i + ","
						+ "@DelforReport" + i + ","
						+ "@DelforStatistics" + i + ","
						+ "@DelforView" + i + ","
						+ "@DLFPosHorizon1" + i + ","
						+ "@DLFPosHorizon2" + i + ","
						+ "@DLFPosHorizon3" + i + ","
						+ "@ExportFGXlsHistory" + i + ","
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
						+ "@FaDateChangeHistory" + i + ","
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
						+ "@FaHoursMovement" + i + ","
						+ "@FALaufkarteSchneiderei" + i + ","
						+ "@FaPlanningEdit" + i + ","
						+ "@FaPlanningView" + i + ","
						+ "@FaPlanningViolation" + i + ","
						+ "@FAPlannung" + i + ","
						+ "@FAPlannungHistory" + i + ","
						+ "@FAPlannungHistoryForceAgent" + i + ","
						+ "@FAPlannungHistoryXLSExport" + i + ","
						+ "@FAPlannungHistoryXLSImport" + i + ","
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
						+ "@FAUpdatePrio" + i + ","
						+ "@FAUpdateTerminHorizon1" + i + ","
						+ "@FAUpdateTerminHorizon2" + i + ","
						+ "@FAUpdateTerminHorizon3" + i + ","
						+ "@FAWerkWunshAdmin" + i + ","
						+ "@Fertigung" + i + ","
						+ "@FertigungLog" + i + ","
						+ "@Forecast" + i + ","
						+ "@ForecastCreate" + i + ","
						+ "@ForecastDelete" + i + ","
						+ "@ForecastStatistics" + i + ","
						+ "@ImportFGXlsHistory" + i + ","
						+ "@isDefault" + i + ","
						+ "@LogsFGXlsHistory" + i + ","
						+ "@ModuleActivated" + i + ","
						+ "@ProductionPlanningByCustomer" + i + ","
						+ "@Statistics" + i + ","
						+ "@StatsAnteileingelasteteFA" + i + ","
						+ "@StatsBacklogFG" + i + ","
						+ "@StatsCapaHorizons" + i + ","
						+ "@StatsCapaPlanning" + i + ","
						+ "@StatsCreatedInvoices" + i + ","
						+ "@StatsDeliveries" + i + ","
						+ "@StatsFAAnderungshistoire" + i + ","
						+ "@StatsLagerbestandFGCRP" + i + ","
						+ "@StatsRahmenSale" + i + ","
						+ "@StatsStockCS" + i + ","
						+ "@StatsStockExternalWarehouse" + i + ","
						+ "@StatsStockFG" + i + ","
						+ "@SystemLogs" + i + ","
						+ "@UBGStatusChange" + i + ","
						+ "@ViewFGBestandHistory" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AgentFGXlsHistory" + i, item.AgentFGXlsHistory == null ? (object)DBNull.Value : item.AgentFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("AllFaChanges" + i, item.AllFaChanges == null ? (object)DBNull.Value : item.AllFaChanges);
					sqlCommand.Parameters.AddWithValue("AllProductionWarehouses" + i, item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
					sqlCommand.Parameters.AddWithValue("Configuration" + i, item.Configuration == null ? (object)DBNull.Value : item.Configuration);
					sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments" + i, item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
					sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees" + i, item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
					sqlCommand.Parameters.AddWithValue("ConfigurationReplacements" + i, item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CRPDashboardActiveArticles" + i, item.CRPDashboardActiveArticles == null ? (object)DBNull.Value : item.CRPDashboardActiveArticles);
					sqlCommand.Parameters.AddWithValue("CRPDashboardCancelledOrders" + i, item.CRPDashboardCancelledOrders == null ? (object)DBNull.Value : item.CRPDashboardCancelledOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardCreatedOrders" + i, item.CRPDashboardCreatedOrders == null ? (object)DBNull.Value : item.CRPDashboardCreatedOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrders" + i, item.CRPDashboardOpenOrders == null ? (object)DBNull.Value : item.CRPDashboardOpenOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrdersHours" + i, item.CRPDashboardOpenOrdersHours == null ? (object)DBNull.Value : item.CRPDashboardOpenOrdersHours);
					sqlCommand.Parameters.AddWithValue("CRPDashboardTotalStockFG" + i, item.CRPDashboardTotalStockFG == null ? (object)DBNull.Value : item.CRPDashboardTotalStockFG);
					sqlCommand.Parameters.AddWithValue("CRPFAPlannung" + i, item.CRPFAPlannung == null ? (object)DBNull.Value : item.CRPFAPlannung);
					sqlCommand.Parameters.AddWithValue("CRPPlanning" + i, item.CRPPlanning == null ? (object)DBNull.Value : item.CRPPlanning);
					sqlCommand.Parameters.AddWithValue("CRPRequirement" + i, item.CRPRequirement == null ? (object)DBNull.Value : item.CRPRequirement);
					sqlCommand.Parameters.AddWithValue("CRPUBGPlanning" + i, item.CRPUBGPlanning == null ? (object)DBNull.Value : item.CRPUBGPlanning);
					sqlCommand.Parameters.AddWithValue("DelforCreate" + i, item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
					sqlCommand.Parameters.AddWithValue("DelforDelete" + i, item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
					sqlCommand.Parameters.AddWithValue("DelforDeletePosition" + i, item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
					sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation" + i, item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
					sqlCommand.Parameters.AddWithValue("DelforReport" + i, item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
					sqlCommand.Parameters.AddWithValue("DelforStatistics" + i, item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
					sqlCommand.Parameters.AddWithValue("DelforView" + i, item.DelforView == null ? (object)DBNull.Value : item.DelforView);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon1" + i, item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon2" + i, item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon3" + i, item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
					sqlCommand.Parameters.AddWithValue("ExportFGXlsHistory" + i, item.ExportFGXlsHistory == null ? (object)DBNull.Value : item.ExportFGXlsHistory);
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
					sqlCommand.Parameters.AddWithValue("FaDateChangeHistory" + i, item.FaDateChangeHistory == null ? (object)DBNull.Value : item.FaDateChangeHistory);
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
					sqlCommand.Parameters.AddWithValue("FaHoursMovement" + i, item.FaHoursMovement == null ? (object)DBNull.Value : item.FaHoursMovement);
					sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei" + i, item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
					sqlCommand.Parameters.AddWithValue("FaPlanningEdit" + i, item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
					sqlCommand.Parameters.AddWithValue("FaPlanningView" + i, item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
					sqlCommand.Parameters.AddWithValue("FaPlanningViolation" + i, item.FaPlanningViolation == null ? (object)DBNull.Value : item.FaPlanningViolation);
					sqlCommand.Parameters.AddWithValue("FAPlannung" + i, item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistory" + i, item.FAPlannungHistory == null ? (object)DBNull.Value : item.FAPlannungHistory);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryForceAgent" + i, item.FAPlannungHistoryForceAgent == null ? (object)DBNull.Value : item.FAPlannungHistoryForceAgent);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSExport" + i, item.FAPlannungHistoryXLSExport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSExport);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSImport" + i, item.FAPlannungHistoryXLSImport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSImport);
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
					sqlCommand.Parameters.AddWithValue("FAUpdatePrio" + i, item.FAUpdatePrio == null ? (object)DBNull.Value : item.FAUpdatePrio);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1" + i, item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2" + i, item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3" + i, item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
					sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin" + i, item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
					sqlCommand.Parameters.AddWithValue("Fertigung" + i, item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
					sqlCommand.Parameters.AddWithValue("FertigungLog" + i, item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
					sqlCommand.Parameters.AddWithValue("Forecast" + i, item.Forecast == null ? (object)DBNull.Value : item.Forecast);
					sqlCommand.Parameters.AddWithValue("ForecastCreate" + i, item.ForecastCreate == null ? (object)DBNull.Value : item.ForecastCreate);
					sqlCommand.Parameters.AddWithValue("ForecastDelete" + i, item.ForecastDelete == null ? (object)DBNull.Value : item.ForecastDelete);
					sqlCommand.Parameters.AddWithValue("ForecastStatistics" + i, item.ForecastStatistics == null ? (object)DBNull.Value : item.ForecastStatistics);
					sqlCommand.Parameters.AddWithValue("ImportFGXlsHistory" + i, item.ImportFGXlsHistory == null ? (object)DBNull.Value : item.ImportFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("LogsFGXlsHistory" + i, item.LogsFGXlsHistory == null ? (object)DBNull.Value : item.LogsFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer" + i, item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
					sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatsAnteileingelasteteFA" + i, item.StatsAnteileingelasteteFA == null ? (object)DBNull.Value : item.StatsAnteileingelasteteFA);
					sqlCommand.Parameters.AddWithValue("StatsBacklogFG" + i, item.StatsBacklogFG == null ? (object)DBNull.Value : item.StatsBacklogFG);
					sqlCommand.Parameters.AddWithValue("StatsCapaHorizons" + i, item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
					sqlCommand.Parameters.AddWithValue("StatsCapaPlanning" + i, item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
					sqlCommand.Parameters.AddWithValue("StatsCreatedInvoices" + i, item.StatsCreatedInvoices == null ? (object)DBNull.Value : item.StatsCreatedInvoices);
					sqlCommand.Parameters.AddWithValue("StatsDeliveries" + i, item.StatsDeliveries == null ? (object)DBNull.Value : item.StatsDeliveries);
					sqlCommand.Parameters.AddWithValue("StatsFAAnderungshistoire" + i, item.StatsFAAnderungshistoire == null ? (object)DBNull.Value : item.StatsFAAnderungshistoire);
					sqlCommand.Parameters.AddWithValue("StatsLagerbestandFGCRP" + i, item.StatsLagerbestandFGCRP == null ? (object)DBNull.Value : item.StatsLagerbestandFGCRP);
					sqlCommand.Parameters.AddWithValue("StatsRahmenSale" + i, item.StatsRahmenSale == null ? (object)DBNull.Value : item.StatsRahmenSale);
					sqlCommand.Parameters.AddWithValue("StatsStockCS" + i, item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
					sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse" + i, item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
					sqlCommand.Parameters.AddWithValue("StatsStockFG" + i, item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
					sqlCommand.Parameters.AddWithValue("SystemLogs" + i, item.SystemLogs == null ? (object)DBNull.Value : item.SystemLogs);
					sqlCommand.Parameters.AddWithValue("UBGStatusChange" + i, item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
					sqlCommand.Parameters.AddWithValue("ViewFGBestandHistory" + i, item.ViewFGBestandHistory == null ? (object)DBNull.Value : item.ViewFGBestandHistory);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__CRP_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [AgentFGXlsHistory]=@AgentFGXlsHistory, [AllFaChanges]=@AllFaChanges, [AllProductionWarehouses]=@AllProductionWarehouses, [Configuration]=@Configuration, [ConfigurationAppoitments]=@ConfigurationAppoitments, [ConfigurationChangeEmployees]=@ConfigurationChangeEmployees, [ConfigurationReplacements]=@ConfigurationReplacements, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CRPDashboardActiveArticles]=@CRPDashboardActiveArticles, [CRPDashboardCancelledOrders]=@CRPDashboardCancelledOrders, [CRPDashboardCreatedOrders]=@CRPDashboardCreatedOrders, [CRPDashboardOpenOrders]=@CRPDashboardOpenOrders, [CRPDashboardOpenOrdersHours]=@CRPDashboardOpenOrdersHours, [CRPDashboardTotalStockFG]=@CRPDashboardTotalStockFG, [CRPFAPlannung]=@CRPFAPlannung, [CRPPlanning]=@CRPPlanning, [CRPRequirement]=@CRPRequirement, [CRPUBGPlanning]=@CRPUBGPlanning, [DelforCreate]=@DelforCreate, [DelforDelete]=@DelforDelete, [DelforDeletePosition]=@DelforDeletePosition, [DelforOrderConfirmation]=@DelforOrderConfirmation, [DelforReport]=@DelforReport, [DelforStatistics]=@DelforStatistics, [DelforView]=@DelforView, [DLFPosHorizon1]=@DLFPosHorizon1, [DLFPosHorizon2]=@DLFPosHorizon2, [DLFPosHorizon3]=@DLFPosHorizon3, [ExportFGXlsHistory]=@ExportFGXlsHistory, [FaABEdit]=@FaABEdit, [FaABView]=@FaABView, [FaActionBook]=@FaActionBook, [FaActionComplete]=@FaActionComplete, [FaActionDelete]=@FaActionDelete, [FaActionPrint]=@FaActionPrint, [FaAdmin]=@FaAdmin, [FAAKtualTerminUpdate]=@FAAKtualTerminUpdate, [FaAnalysis]=@FaAnalysis, [FAAuswertungEndkontrolle]=@FAAuswertungEndkontrolle, [FABemerkungPlannug]=@FABemerkungPlannug, [FABemerkungZuGewerk]=@FABemerkungZuGewerk, [FABemerkungZuPrio]=@FABemerkungZuPrio, [FACancelHorizon1]=@FACancelHorizon1, [FACancelHorizon2]=@FACancelHorizon2, [FACancelHorizon3]=@FACancelHorizon3, [FACommissionert]=@FACommissionert, [FaCreate]=@FaCreate, [FACreateHorizon1]=@FACreateHorizon1, [FACreateHorizon2]=@FACreateHorizon2, [FACreateHorizon3]=@FACreateHorizon3, [FaDateChangeHistory]=@FaDateChangeHistory, [FaDatenEdit]=@FaDatenEdit, [FaDatenView]=@FaDatenView, [FaDelete]=@FaDelete, [FADrucken]=@FADrucken, [FaEdit]=@FaEdit, [FAErlidegen]=@FAErlidegen, [FAExcelUpdateWerk]=@FAExcelUpdateWerk, [FAExcelUpdateWunsh]=@FAExcelUpdateWunsh, [FAFehlrMaterial]=@FAFehlrMaterial, [FaHomeAnalysis]=@FaHomeAnalysis, [FaHomeUpdate]=@FaHomeUpdate, [FaHoursMovement]=@FaHoursMovement, [FALaufkarteSchneiderei]=@FALaufkarteSchneiderei, [FaPlanningEdit]=@FaPlanningEdit, [FaPlanningView]=@FaPlanningView, [FaPlanningViolation]=@FaPlanningViolation, [FAPlannung]=@FAPlannung, [FAPlannungHistory]=@FAPlannungHistory, [FAPlannungHistoryForceAgent]=@FAPlannungHistoryForceAgent, [FAPlannungHistoryXLSExport]=@FAPlannungHistoryXLSExport, [FAPlannungHistoryXLSImport]=@FAPlannungHistoryXLSImport, [FAPlannungTechnick]=@FAPlannungTechnick, [FAPriesZeitUpdate]=@FAPriesZeitUpdate, [FAProductionPlannung]=@FAProductionPlannung, [FAStappleDruck]=@FAStappleDruck, [FAStatusAlbania]=@FAStatusAlbania, [FAStatusCzech]=@FAStatusCzech, [FAStatusTunisia]=@FAStatusTunisia, [FAStorno]=@FAStorno, [FAStucklist]=@FAStucklist, [FaTechnicEdit]=@FaTechnicEdit, [FaTechnicView]=@FaTechnicView, [FATerminWerk]=@FATerminWerk, [FAUpdateBemerkungExtern]=@FAUpdateBemerkungExtern, [FAUpdateByArticle]=@FAUpdateByArticle, [FAUpdateByFA]=@FAUpdateByFA, [FAUpdatePrio]=@FAUpdatePrio, [FAUpdateTerminHorizon1]=@FAUpdateTerminHorizon1, [FAUpdateTerminHorizon2]=@FAUpdateTerminHorizon2, [FAUpdateTerminHorizon3]=@FAUpdateTerminHorizon3, [FAWerkWunshAdmin]=@FAWerkWunshAdmin, [Fertigung]=@Fertigung, [FertigungLog]=@FertigungLog, [Forecast]=@Forecast, [ForecastCreate]=@ForecastCreate, [ForecastDelete]=@ForecastDelete, [ForecastStatistics]=@ForecastStatistics, [ImportFGXlsHistory]=@ImportFGXlsHistory, [isDefault]=@isDefault, [LogsFGXlsHistory]=@LogsFGXlsHistory, [ModuleActivated]=@ModuleActivated, [ProductionPlanningByCustomer]=@ProductionPlanningByCustomer, [Statistics]=@Statistics, [StatsAnteileingelasteteFA]=@StatsAnteileingelasteteFA, [StatsBacklogFG]=@StatsBacklogFG, [StatsCapaHorizons]=@StatsCapaHorizons, [StatsCapaPlanning]=@StatsCapaPlanning, [StatsCreatedInvoices]=@StatsCreatedInvoices, [StatsDeliveries]=@StatsDeliveries, [StatsFAAnderungshistoire]=@StatsFAAnderungshistoire, [StatsLagerbestandFGCRP]=@StatsLagerbestandFGCRP, [StatsRahmenSale]=@StatsRahmenSale, [StatsStockCS]=@StatsStockCS, [StatsStockExternalWarehouse]=@StatsStockExternalWarehouse, [StatsStockFG]=@StatsStockFG, [SystemLogs]=@SystemLogs, [UBGStatusChange]=@UBGStatusChange, [ViewFGBestandHistory]=@ViewFGBestandHistory WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
			sqlCommand.Parameters.AddWithValue("AgentFGXlsHistory", item.AgentFGXlsHistory == null ? (object)DBNull.Value : item.AgentFGXlsHistory);
			sqlCommand.Parameters.AddWithValue("AllFaChanges", item.AllFaChanges == null ? (object)DBNull.Value : item.AllFaChanges);
			sqlCommand.Parameters.AddWithValue("AllProductionWarehouses", item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
			sqlCommand.Parameters.AddWithValue("Configuration", item.Configuration == null ? (object)DBNull.Value : item.Configuration);
			sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments", item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
			sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees", item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
			sqlCommand.Parameters.AddWithValue("ConfigurationReplacements", item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CRPDashboardActiveArticles", item.CRPDashboardActiveArticles == null ? (object)DBNull.Value : item.CRPDashboardActiveArticles);
			sqlCommand.Parameters.AddWithValue("CRPDashboardCancelledOrders", item.CRPDashboardCancelledOrders == null ? (object)DBNull.Value : item.CRPDashboardCancelledOrders);
			sqlCommand.Parameters.AddWithValue("CRPDashboardCreatedOrders", item.CRPDashboardCreatedOrders == null ? (object)DBNull.Value : item.CRPDashboardCreatedOrders);
			sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrders", item.CRPDashboardOpenOrders == null ? (object)DBNull.Value : item.CRPDashboardOpenOrders);
			sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrdersHours", item.CRPDashboardOpenOrdersHours == null ? (object)DBNull.Value : item.CRPDashboardOpenOrdersHours);
			sqlCommand.Parameters.AddWithValue("CRPDashboardTotalStockFG", item.CRPDashboardTotalStockFG == null ? (object)DBNull.Value : item.CRPDashboardTotalStockFG);
			sqlCommand.Parameters.AddWithValue("CRPFAPlannung", item.CRPFAPlannung == null ? (object)DBNull.Value : item.CRPFAPlannung);
			sqlCommand.Parameters.AddWithValue("CRPPlanning", item.CRPPlanning == null ? (object)DBNull.Value : item.CRPPlanning);
			sqlCommand.Parameters.AddWithValue("CRPRequirement", item.CRPRequirement == null ? (object)DBNull.Value : item.CRPRequirement);
			sqlCommand.Parameters.AddWithValue("CRPUBGPlanning", item.CRPUBGPlanning == null ? (object)DBNull.Value : item.CRPUBGPlanning);
			sqlCommand.Parameters.AddWithValue("DelforCreate", item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
			sqlCommand.Parameters.AddWithValue("DelforDelete", item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
			sqlCommand.Parameters.AddWithValue("DelforDeletePosition", item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
			sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation", item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
			sqlCommand.Parameters.AddWithValue("DelforReport", item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
			sqlCommand.Parameters.AddWithValue("DelforStatistics", item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
			sqlCommand.Parameters.AddWithValue("DelforView", item.DelforView == null ? (object)DBNull.Value : item.DelforView);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon1", item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon2", item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
			sqlCommand.Parameters.AddWithValue("DLFPosHorizon3", item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
			sqlCommand.Parameters.AddWithValue("ExportFGXlsHistory", item.ExportFGXlsHistory == null ? (object)DBNull.Value : item.ExportFGXlsHistory);
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
			sqlCommand.Parameters.AddWithValue("FaDateChangeHistory", item.FaDateChangeHistory == null ? (object)DBNull.Value : item.FaDateChangeHistory);
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
			sqlCommand.Parameters.AddWithValue("FaHoursMovement", item.FaHoursMovement == null ? (object)DBNull.Value : item.FaHoursMovement);
			sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei", item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
			sqlCommand.Parameters.AddWithValue("FaPlanningEdit", item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
			sqlCommand.Parameters.AddWithValue("FaPlanningView", item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
			sqlCommand.Parameters.AddWithValue("FaPlanningViolation", item.FaPlanningViolation == null ? (object)DBNull.Value : item.FaPlanningViolation);
			sqlCommand.Parameters.AddWithValue("FAPlannung", item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
			sqlCommand.Parameters.AddWithValue("FAPlannungHistory", item.FAPlannungHistory == null ? (object)DBNull.Value : item.FAPlannungHistory);
			sqlCommand.Parameters.AddWithValue("FAPlannungHistoryForceAgent", item.FAPlannungHistoryForceAgent == null ? (object)DBNull.Value : item.FAPlannungHistoryForceAgent);
			sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSExport", item.FAPlannungHistoryXLSExport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSExport);
			sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSImport", item.FAPlannungHistoryXLSImport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSImport);
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
			sqlCommand.Parameters.AddWithValue("FAUpdatePrio", item.FAUpdatePrio == null ? (object)DBNull.Value : item.FAUpdatePrio);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1", item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2", item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
			sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3", item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
			sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin", item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
			sqlCommand.Parameters.AddWithValue("Fertigung", item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
			sqlCommand.Parameters.AddWithValue("FertigungLog", item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
			sqlCommand.Parameters.AddWithValue("Forecast", item.Forecast == null ? (object)DBNull.Value : item.Forecast);
			sqlCommand.Parameters.AddWithValue("ForecastCreate", item.ForecastCreate == null ? (object)DBNull.Value : item.ForecastCreate);
			sqlCommand.Parameters.AddWithValue("ForecastDelete", item.ForecastDelete == null ? (object)DBNull.Value : item.ForecastDelete);
			sqlCommand.Parameters.AddWithValue("ForecastStatistics", item.ForecastStatistics == null ? (object)DBNull.Value : item.ForecastStatistics);
			sqlCommand.Parameters.AddWithValue("ImportFGXlsHistory", item.ImportFGXlsHistory == null ? (object)DBNull.Value : item.ImportFGXlsHistory);
			sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
			sqlCommand.Parameters.AddWithValue("LogsFGXlsHistory", item.LogsFGXlsHistory == null ? (object)DBNull.Value : item.LogsFGXlsHistory);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer", item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
			sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
			sqlCommand.Parameters.AddWithValue("StatsAnteileingelasteteFA", item.StatsAnteileingelasteteFA == null ? (object)DBNull.Value : item.StatsAnteileingelasteteFA);
			sqlCommand.Parameters.AddWithValue("StatsBacklogFG", item.StatsBacklogFG == null ? (object)DBNull.Value : item.StatsBacklogFG);
			sqlCommand.Parameters.AddWithValue("StatsCapaHorizons", item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
			sqlCommand.Parameters.AddWithValue("StatsCapaPlanning", item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
			sqlCommand.Parameters.AddWithValue("StatsCreatedInvoices", item.StatsCreatedInvoices == null ? (object)DBNull.Value : item.StatsCreatedInvoices);
			sqlCommand.Parameters.AddWithValue("StatsDeliveries", item.StatsDeliveries == null ? (object)DBNull.Value : item.StatsDeliveries);
			sqlCommand.Parameters.AddWithValue("StatsFAAnderungshistoire", item.StatsFAAnderungshistoire == null ? (object)DBNull.Value : item.StatsFAAnderungshistoire);
			sqlCommand.Parameters.AddWithValue("StatsLagerbestandFGCRP", item.StatsLagerbestandFGCRP == null ? (object)DBNull.Value : item.StatsLagerbestandFGCRP);
			sqlCommand.Parameters.AddWithValue("StatsRahmenSale", item.StatsRahmenSale == null ? (object)DBNull.Value : item.StatsRahmenSale);
			sqlCommand.Parameters.AddWithValue("StatsStockCS", item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
			sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse", item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
			sqlCommand.Parameters.AddWithValue("StatsStockFG", item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
			sqlCommand.Parameters.AddWithValue("SystemLogs", item.SystemLogs == null ? (object)DBNull.Value : item.SystemLogs);
			sqlCommand.Parameters.AddWithValue("UBGStatusChange", item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
			sqlCommand.Parameters.AddWithValue("ViewFGBestandHistory", item.ViewFGBestandHistory == null ? (object)DBNull.Value : item.ViewFGBestandHistory);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 124; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__CRP_AccessProfile] SET "

					+ "[AccessProfileName]=@AccessProfileName" + i + ","
					+ "[Administration]=@Administration" + i + ","
					+ "[AgentFGXlsHistory]=@AgentFGXlsHistory" + i + ","
					+ "[AllFaChanges]=@AllFaChanges" + i + ","
					+ "[AllProductionWarehouses]=@AllProductionWarehouses" + i + ","
					+ "[Configuration]=@Configuration" + i + ","
					+ "[ConfigurationAppoitments]=@ConfigurationAppoitments" + i + ","
					+ "[ConfigurationChangeEmployees]=@ConfigurationChangeEmployees" + i + ","
					+ "[ConfigurationReplacements]=@ConfigurationReplacements" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CRPDashboardActiveArticles]=@CRPDashboardActiveArticles" + i + ","
					+ "[CRPDashboardCancelledOrders]=@CRPDashboardCancelledOrders" + i + ","
					+ "[CRPDashboardCreatedOrders]=@CRPDashboardCreatedOrders" + i + ","
					+ "[CRPDashboardOpenOrders]=@CRPDashboardOpenOrders" + i + ","
					+ "[CRPDashboardOpenOrdersHours]=@CRPDashboardOpenOrdersHours" + i + ","
					+ "[CRPDashboardTotalStockFG]=@CRPDashboardTotalStockFG" + i + ","
					+ "[CRPFAPlannung]=@CRPFAPlannung" + i + ","
					+ "[CRPPlanning]=@CRPPlanning" + i + ","
					+ "[CRPRequirement]=@CRPRequirement" + i + ","
					+ "[CRPUBGPlanning]=@CRPUBGPlanning" + i + ","
					+ "[DelforCreate]=@DelforCreate" + i + ","
					+ "[DelforDelete]=@DelforDelete" + i + ","
					+ "[DelforDeletePosition]=@DelforDeletePosition" + i + ","
					+ "[DelforOrderConfirmation]=@DelforOrderConfirmation" + i + ","
					+ "[DelforReport]=@DelforReport" + i + ","
					+ "[DelforStatistics]=@DelforStatistics" + i + ","
					+ "[DelforView]=@DelforView" + i + ","
					+ "[DLFPosHorizon1]=@DLFPosHorizon1" + i + ","
					+ "[DLFPosHorizon2]=@DLFPosHorizon2" + i + ","
					+ "[DLFPosHorizon3]=@DLFPosHorizon3" + i + ","
					+ "[ExportFGXlsHistory]=@ExportFGXlsHistory" + i + ","
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
					+ "[FaDateChangeHistory]=@FaDateChangeHistory" + i + ","
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
					+ "[FaHoursMovement]=@FaHoursMovement" + i + ","
					+ "[FALaufkarteSchneiderei]=@FALaufkarteSchneiderei" + i + ","
					+ "[FaPlanningEdit]=@FaPlanningEdit" + i + ","
					+ "[FaPlanningView]=@FaPlanningView" + i + ","
					+ "[FaPlanningViolation]=@FaPlanningViolation" + i + ","
					+ "[FAPlannung]=@FAPlannung" + i + ","
					+ "[FAPlannungHistory]=@FAPlannungHistory" + i + ","
					+ "[FAPlannungHistoryForceAgent]=@FAPlannungHistoryForceAgent" + i + ","
					+ "[FAPlannungHistoryXLSExport]=@FAPlannungHistoryXLSExport" + i + ","
					+ "[FAPlannungHistoryXLSImport]=@FAPlannungHistoryXLSImport" + i + ","
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
					+ "[FAUpdatePrio]=@FAUpdatePrio" + i + ","
					+ "[FAUpdateTerminHorizon1]=@FAUpdateTerminHorizon1" + i + ","
					+ "[FAUpdateTerminHorizon2]=@FAUpdateTerminHorizon2" + i + ","
					+ "[FAUpdateTerminHorizon3]=@FAUpdateTerminHorizon3" + i + ","
					+ "[FAWerkWunshAdmin]=@FAWerkWunshAdmin" + i + ","
					+ "[Fertigung]=@Fertigung" + i + ","
					+ "[FertigungLog]=@FertigungLog" + i + ","
					+ "[Forecast]=@Forecast" + i + ","
					+ "[ForecastCreate]=@ForecastCreate" + i + ","
					+ "[ForecastDelete]=@ForecastDelete" + i + ","
					+ "[ForecastStatistics]=@ForecastStatistics" + i + ","
					+ "[ImportFGXlsHistory]=@ImportFGXlsHistory" + i + ","
					+ "[isDefault]=@isDefault" + i + ","
					+ "[LogsFGXlsHistory]=@LogsFGXlsHistory" + i + ","
					+ "[ModuleActivated]=@ModuleActivated" + i + ","
					+ "[ProductionPlanningByCustomer]=@ProductionPlanningByCustomer" + i + ","
					+ "[Statistics]=@Statistics" + i + ","
					+ "[StatsAnteileingelasteteFA]=@StatsAnteileingelasteteFA" + i + ","
					+ "[StatsBacklogFG]=@StatsBacklogFG" + i + ","
					+ "[StatsCapaHorizons]=@StatsCapaHorizons" + i + ","
					+ "[StatsCapaPlanning]=@StatsCapaPlanning" + i + ","
					+ "[StatsCreatedInvoices]=@StatsCreatedInvoices" + i + ","
					+ "[StatsDeliveries]=@StatsDeliveries" + i + ","
					+ "[StatsFAAnderungshistoire]=@StatsFAAnderungshistoire" + i + ","
					+ "[StatsLagerbestandFGCRP]=@StatsLagerbestandFGCRP" + i + ","
					+ "[StatsRahmenSale]=@StatsRahmenSale" + i + ","
					+ "[StatsStockCS]=@StatsStockCS" + i + ","
					+ "[StatsStockExternalWarehouse]=@StatsStockExternalWarehouse" + i + ","
					+ "[StatsStockFG]=@StatsStockFG" + i + ","
					+ "[SystemLogs]=@SystemLogs" + i + ","
					+ "[UBGStatusChange]=@UBGStatusChange" + i + ","
					+ "[ViewFGBestandHistory]=@ViewFGBestandHistory" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AgentFGXlsHistory" + i, item.AgentFGXlsHistory == null ? (object)DBNull.Value : item.AgentFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("AllFaChanges" + i, item.AllFaChanges == null ? (object)DBNull.Value : item.AllFaChanges);
					sqlCommand.Parameters.AddWithValue("AllProductionWarehouses" + i, item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
					sqlCommand.Parameters.AddWithValue("Configuration" + i, item.Configuration == null ? (object)DBNull.Value : item.Configuration);
					sqlCommand.Parameters.AddWithValue("ConfigurationAppoitments" + i, item.ConfigurationAppoitments == null ? (object)DBNull.Value : item.ConfigurationAppoitments);
					sqlCommand.Parameters.AddWithValue("ConfigurationChangeEmployees" + i, item.ConfigurationChangeEmployees == null ? (object)DBNull.Value : item.ConfigurationChangeEmployees);
					sqlCommand.Parameters.AddWithValue("ConfigurationReplacements" + i, item.ConfigurationReplacements == null ? (object)DBNull.Value : item.ConfigurationReplacements);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CRPDashboardActiveArticles" + i, item.CRPDashboardActiveArticles == null ? (object)DBNull.Value : item.CRPDashboardActiveArticles);
					sqlCommand.Parameters.AddWithValue("CRPDashboardCancelledOrders" + i, item.CRPDashboardCancelledOrders == null ? (object)DBNull.Value : item.CRPDashboardCancelledOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardCreatedOrders" + i, item.CRPDashboardCreatedOrders == null ? (object)DBNull.Value : item.CRPDashboardCreatedOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrders" + i, item.CRPDashboardOpenOrders == null ? (object)DBNull.Value : item.CRPDashboardOpenOrders);
					sqlCommand.Parameters.AddWithValue("CRPDashboardOpenOrdersHours" + i, item.CRPDashboardOpenOrdersHours == null ? (object)DBNull.Value : item.CRPDashboardOpenOrdersHours);
					sqlCommand.Parameters.AddWithValue("CRPDashboardTotalStockFG" + i, item.CRPDashboardTotalStockFG == null ? (object)DBNull.Value : item.CRPDashboardTotalStockFG);
					sqlCommand.Parameters.AddWithValue("CRPFAPlannung" + i, item.CRPFAPlannung == null ? (object)DBNull.Value : item.CRPFAPlannung);
					sqlCommand.Parameters.AddWithValue("CRPPlanning" + i, item.CRPPlanning == null ? (object)DBNull.Value : item.CRPPlanning);
					sqlCommand.Parameters.AddWithValue("CRPRequirement" + i, item.CRPRequirement == null ? (object)DBNull.Value : item.CRPRequirement);
					sqlCommand.Parameters.AddWithValue("CRPUBGPlanning" + i, item.CRPUBGPlanning == null ? (object)DBNull.Value : item.CRPUBGPlanning);
					sqlCommand.Parameters.AddWithValue("DelforCreate" + i, item.DelforCreate == null ? (object)DBNull.Value : item.DelforCreate);
					sqlCommand.Parameters.AddWithValue("DelforDelete" + i, item.DelforDelete == null ? (object)DBNull.Value : item.DelforDelete);
					sqlCommand.Parameters.AddWithValue("DelforDeletePosition" + i, item.DelforDeletePosition == null ? (object)DBNull.Value : item.DelforDeletePosition);
					sqlCommand.Parameters.AddWithValue("DelforOrderConfirmation" + i, item.DelforOrderConfirmation == null ? (object)DBNull.Value : item.DelforOrderConfirmation);
					sqlCommand.Parameters.AddWithValue("DelforReport" + i, item.DelforReport == null ? (object)DBNull.Value : item.DelforReport);
					sqlCommand.Parameters.AddWithValue("DelforStatistics" + i, item.DelforStatistics == null ? (object)DBNull.Value : item.DelforStatistics);
					sqlCommand.Parameters.AddWithValue("DelforView" + i, item.DelforView == null ? (object)DBNull.Value : item.DelforView);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon1" + i, item.DLFPosHorizon1 == null ? (object)DBNull.Value : item.DLFPosHorizon1);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon2" + i, item.DLFPosHorizon2 == null ? (object)DBNull.Value : item.DLFPosHorizon2);
					sqlCommand.Parameters.AddWithValue("DLFPosHorizon3" + i, item.DLFPosHorizon3 == null ? (object)DBNull.Value : item.DLFPosHorizon3);
					sqlCommand.Parameters.AddWithValue("ExportFGXlsHistory" + i, item.ExportFGXlsHistory == null ? (object)DBNull.Value : item.ExportFGXlsHistory);
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
					sqlCommand.Parameters.AddWithValue("FaDateChangeHistory" + i, item.FaDateChangeHistory == null ? (object)DBNull.Value : item.FaDateChangeHistory);
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
					sqlCommand.Parameters.AddWithValue("FaHoursMovement" + i, item.FaHoursMovement == null ? (object)DBNull.Value : item.FaHoursMovement);
					sqlCommand.Parameters.AddWithValue("FALaufkarteSchneiderei" + i, item.FALaufkarteSchneiderei == null ? (object)DBNull.Value : item.FALaufkarteSchneiderei);
					sqlCommand.Parameters.AddWithValue("FaPlanningEdit" + i, item.FaPlanningEdit == null ? (object)DBNull.Value : item.FaPlanningEdit);
					sqlCommand.Parameters.AddWithValue("FaPlanningView" + i, item.FaPlanningView == null ? (object)DBNull.Value : item.FaPlanningView);
					sqlCommand.Parameters.AddWithValue("FaPlanningViolation" + i, item.FaPlanningViolation == null ? (object)DBNull.Value : item.FaPlanningViolation);
					sqlCommand.Parameters.AddWithValue("FAPlannung" + i, item.FAPlannung == null ? (object)DBNull.Value : item.FAPlannung);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistory" + i, item.FAPlannungHistory == null ? (object)DBNull.Value : item.FAPlannungHistory);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryForceAgent" + i, item.FAPlannungHistoryForceAgent == null ? (object)DBNull.Value : item.FAPlannungHistoryForceAgent);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSExport" + i, item.FAPlannungHistoryXLSExport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSExport);
					sqlCommand.Parameters.AddWithValue("FAPlannungHistoryXLSImport" + i, item.FAPlannungHistoryXLSImport == null ? (object)DBNull.Value : item.FAPlannungHistoryXLSImport);
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
					sqlCommand.Parameters.AddWithValue("FAUpdatePrio" + i, item.FAUpdatePrio == null ? (object)DBNull.Value : item.FAUpdatePrio);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon1" + i, item.FAUpdateTerminHorizon1 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon1);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon2" + i, item.FAUpdateTerminHorizon2 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon2);
					sqlCommand.Parameters.AddWithValue("FAUpdateTerminHorizon3" + i, item.FAUpdateTerminHorizon3 == null ? (object)DBNull.Value : item.FAUpdateTerminHorizon3);
					sqlCommand.Parameters.AddWithValue("FAWerkWunshAdmin" + i, item.FAWerkWunshAdmin == null ? (object)DBNull.Value : item.FAWerkWunshAdmin);
					sqlCommand.Parameters.AddWithValue("Fertigung" + i, item.Fertigung == null ? (object)DBNull.Value : item.Fertigung);
					sqlCommand.Parameters.AddWithValue("FertigungLog" + i, item.FertigungLog == null ? (object)DBNull.Value : item.FertigungLog);
					sqlCommand.Parameters.AddWithValue("Forecast" + i, item.Forecast == null ? (object)DBNull.Value : item.Forecast);
					sqlCommand.Parameters.AddWithValue("ForecastCreate" + i, item.ForecastCreate == null ? (object)DBNull.Value : item.ForecastCreate);
					sqlCommand.Parameters.AddWithValue("ForecastDelete" + i, item.ForecastDelete == null ? (object)DBNull.Value : item.ForecastDelete);
					sqlCommand.Parameters.AddWithValue("ForecastStatistics" + i, item.ForecastStatistics == null ? (object)DBNull.Value : item.ForecastStatistics);
					sqlCommand.Parameters.AddWithValue("ImportFGXlsHistory" + i, item.ImportFGXlsHistory == null ? (object)DBNull.Value : item.ImportFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("LogsFGXlsHistory" + i, item.LogsFGXlsHistory == null ? (object)DBNull.Value : item.LogsFGXlsHistory);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer" + i, item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
					sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatsAnteileingelasteteFA" + i, item.StatsAnteileingelasteteFA == null ? (object)DBNull.Value : item.StatsAnteileingelasteteFA);
					sqlCommand.Parameters.AddWithValue("StatsBacklogFG" + i, item.StatsBacklogFG == null ? (object)DBNull.Value : item.StatsBacklogFG);
					sqlCommand.Parameters.AddWithValue("StatsCapaHorizons" + i, item.StatsCapaHorizons == null ? (object)DBNull.Value : item.StatsCapaHorizons);
					sqlCommand.Parameters.AddWithValue("StatsCapaPlanning" + i, item.StatsCapaPlanning == null ? (object)DBNull.Value : item.StatsCapaPlanning);
					sqlCommand.Parameters.AddWithValue("StatsCreatedInvoices" + i, item.StatsCreatedInvoices == null ? (object)DBNull.Value : item.StatsCreatedInvoices);
					sqlCommand.Parameters.AddWithValue("StatsDeliveries" + i, item.StatsDeliveries == null ? (object)DBNull.Value : item.StatsDeliveries);
					sqlCommand.Parameters.AddWithValue("StatsFAAnderungshistoire" + i, item.StatsFAAnderungshistoire == null ? (object)DBNull.Value : item.StatsFAAnderungshistoire);
					sqlCommand.Parameters.AddWithValue("StatsLagerbestandFGCRP" + i, item.StatsLagerbestandFGCRP == null ? (object)DBNull.Value : item.StatsLagerbestandFGCRP);
					sqlCommand.Parameters.AddWithValue("StatsRahmenSale" + i, item.StatsRahmenSale == null ? (object)DBNull.Value : item.StatsRahmenSale);
					sqlCommand.Parameters.AddWithValue("StatsStockCS" + i, item.StatsStockCS == null ? (object)DBNull.Value : item.StatsStockCS);
					sqlCommand.Parameters.AddWithValue("StatsStockExternalWarehouse" + i, item.StatsStockExternalWarehouse == null ? (object)DBNull.Value : item.StatsStockExternalWarehouse);
					sqlCommand.Parameters.AddWithValue("StatsStockFG" + i, item.StatsStockFG == null ? (object)DBNull.Value : item.StatsStockFG);
					sqlCommand.Parameters.AddWithValue("SystemLogs" + i, item.SystemLogs == null ? (object)DBNull.Value : item.SystemLogs);
					sqlCommand.Parameters.AddWithValue("UBGStatusChange" + i, item.UBGStatusChange == null ? (object)DBNull.Value : item.UBGStatusChange);
					sqlCommand.Parameters.AddWithValue("ViewFGBestandHistory" + i, item.ViewFGBestandHistory == null ? (object)DBNull.Value : item.ViewFGBestandHistory);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__CRP_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


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

				string query = "DELETE FROM [__CRP_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods


		#region Custom Methods
		public static int UpdateName(Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity item)
        {
            int results = -1;
            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "UPDATE [__CRP_AccessProfile] SET [AccessProfileName]=@AccessProfileName WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);

                sqlCommand.Parameters.AddWithValue("Id", item.Id);
                sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public static List<Entities.Tables.CRP.AccessProfileEntity> GetByMainAccessProfilesIds(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE;
                var response = new List<Entities.Tables.CRP.AccessProfileEntity>();
                if (ids.Count <= maxQueryNumber)
                {
                    response = getByMainAccessProfilesIds(ids);
                }
                else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    response = new List<Entities.Tables.CRP.AccessProfileEntity>();
                    for (int i = 0; i < batchNumber; i++)
                    {
                        response.AddRange(getByMainAccessProfilesIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    response.AddRange(getByMainAccessProfilesIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
                }
                return response;
            }
            return new List<Entities.Tables.CRP.AccessProfileEntity>();
        }
        private static List<Entities.Tables.CRP.AccessProfileEntity> getByMainAccessProfilesIds(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                var dataTable = new DataTable();

                using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
                {
                    sqlConnection.Open();

                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;

                    string queryIds = string.Empty;
                    for (int i = 0; i < ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    sqlCommand.CommandText = "SELECT * FROM [__CRP_AccessProfile] WHERE [Id] IN (" + queryIds + ")";

                    new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(x)).ToList();
                }
                else
                {
                    return new List<Entities.Tables.CRP.AccessProfileEntity>();
                }
            }
            return new List<Entities.Tables.CRP.AccessProfileEntity>();
        }
        public static Entities.Tables.CRP.AccessProfileEntity GetByMainAccessProfilesId(int id)
        {
            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [__CRP_AccessProfile] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public static Entities.Tables.CRP.AccessProfileEntity GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [__CRP_AccessProfile] WHERE [AccessProfileName]=@name";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("name", name.Trim());

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.CRP.AccessProfileEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion Custom Methods
    }
}
