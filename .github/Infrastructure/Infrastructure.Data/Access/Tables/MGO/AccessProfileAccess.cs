using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.MGO
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MGO_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MGO_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__MGO_AccessProfile] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MGO_AccessProfile] ([AccessProfileName],[Administration],[AllProductionWarehouses],[CreationTime],[CreationUserId],[CtsPlanning],[CtsSales],[CustomerService],[DashboardProduction],[DashboardProductionEdit],[ModuleActivated],[PmExtraRoh],[PmFaultyArticles],[PmMissingRoh],[Production],[ProductionDashboard],[ProductionHourPlanningSerieFirstOrder],[ProductionPlanningByCustomer],[ProductionWorkLoad],[ProductSale],[ProjectManagment],[SalesHbgUbg],[SalesInvoice],[SalesOrders],[SalesProduction],[SalesRoh],[Statistics],[StatisticsDel],[StatisticsFgAL],[StatisticsFgBETN],[StatisticsFgCZ],[StatisticsFgGZ],[StatisticsFgTN],[StatisticsFgWS],[StatisticsLogs],[StatisticsReasonUpdate],[StatisticsUmsatz]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@Administration,@AllProductionWarehouses,@CreationTime,@CreationUserId,@CtsPlanning,@CtsSales,@CustomerService,@DashboardProduction,@DashboardProductionEdit,@ModuleActivated,@PmExtraRoh,@PmFaultyArticles,@PmMissingRoh,@Production,@ProductionDashboard,@ProductionHourPlanningSerieFirstOrder,@ProductionPlanningByCustomer,@ProductionWorkLoad,@ProductSale,@ProjectManagment,@SalesHbgUbg,@SalesInvoice,@SalesOrders,@SalesProduction,@SalesRoh,@Statistics,@StatisticsDel,@StatisticsFgAL,@StatisticsFgBETN,@StatisticsFgCZ,@StatisticsFgGZ,@StatisticsFgTN,@StatisticsFgWS,@StatisticsLogs,@StatisticsReasonUpdate,@StatisticsUmsatz); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AllProductionWarehouses", item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CtsPlanning", item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
					sqlCommand.Parameters.AddWithValue("CtsSales", item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
					sqlCommand.Parameters.AddWithValue("CustomerService", item.CustomerService == null ? (object)DBNull.Value : item.CustomerService);
					sqlCommand.Parameters.AddWithValue("DashboardProduction", item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
					sqlCommand.Parameters.AddWithValue("DashboardProductionEdit", item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);
					sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("PmExtraRoh", item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
					sqlCommand.Parameters.AddWithValue("PmFaultyArticles", item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
					sqlCommand.Parameters.AddWithValue("PmMissingRoh", item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
					sqlCommand.Parameters.AddWithValue("Production", item.Production == null ? (object)DBNull.Value : item.Production);
					sqlCommand.Parameters.AddWithValue("ProductionDashboard", item.ProductionDashboard == null ? (object)DBNull.Value : item.ProductionDashboard);
					sqlCommand.Parameters.AddWithValue("ProductionHourPlanningSerieFirstOrder", item.ProductionHourPlanningSerieFirstOrder == null ? (object)DBNull.Value : item.ProductionHourPlanningSerieFirstOrder);
					sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer", item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
					sqlCommand.Parameters.AddWithValue("ProductionWorkLoad", item.ProductionWorkLoad == null ? (object)DBNull.Value : item.ProductionWorkLoad);
					sqlCommand.Parameters.AddWithValue("ProductSale", item.ProductSale == null ? (object)DBNull.Value : item.ProductSale);
					sqlCommand.Parameters.AddWithValue("ProjectManagment", item.ProjectManagment == null ? (object)DBNull.Value : item.ProjectManagment);
					sqlCommand.Parameters.AddWithValue("SalesHbgUbg", item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
					sqlCommand.Parameters.AddWithValue("SalesInvoice", item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
					sqlCommand.Parameters.AddWithValue("SalesOrders", item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
					sqlCommand.Parameters.AddWithValue("SalesProduction", item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
					sqlCommand.Parameters.AddWithValue("SalesRoh", item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
					sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatisticsDel", item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
					sqlCommand.Parameters.AddWithValue("StatisticsFgAL", item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
					sqlCommand.Parameters.AddWithValue("StatisticsFgBETN", item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
					sqlCommand.Parameters.AddWithValue("StatisticsFgCZ", item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
					sqlCommand.Parameters.AddWithValue("StatisticsFgGZ", item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
					sqlCommand.Parameters.AddWithValue("StatisticsFgTN", item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
					sqlCommand.Parameters.AddWithValue("StatisticsFgWS", item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
					sqlCommand.Parameters.AddWithValue("StatisticsLogs", item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
					sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate", item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
					sqlCommand.Parameters.AddWithValue("StatisticsUmsatz", item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 38; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> items)
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
						query += " INSERT INTO [__MGO_AccessProfile] ([AccessProfileName],[Administration],[AllProductionWarehouses],[CreationTime],[CreationUserId],[CtsPlanning],[CtsSales],[CustomerService],[DashboardProduction],[DashboardProductionEdit],[ModuleActivated],[PmExtraRoh],[PmFaultyArticles],[PmMissingRoh],[Production],[ProductionDashboard],[ProductionHourPlanningSerieFirstOrder],[ProductionPlanningByCustomer],[ProductionWorkLoad],[ProductSale],[ProjectManagment],[SalesHbgUbg],[SalesInvoice],[SalesOrders],[SalesProduction],[SalesRoh],[Statistics],[StatisticsDel],[StatisticsFgAL],[StatisticsFgBETN],[StatisticsFgCZ],[StatisticsFgGZ],[StatisticsFgTN],[StatisticsFgWS],[StatisticsLogs],[StatisticsReasonUpdate],[StatisticsUmsatz]) VALUES ( "

							+ "@AccessProfileName" + i + ","
							+ "@Administration" + i + ","
							+ "@AllProductionWarehouses" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CtsPlanning" + i + ","
							+ "@CtsSales" + i + ","
							+ "@CustomerService" + i + ","
							+ "@DashboardProduction" + i + ","
							+ "@DashboardProductionEdit" + i + ","
							+ "@ModuleActivated" + i + ","
							+ "@PmExtraRoh" + i + ","
							+ "@PmFaultyArticles" + i + ","
							+ "@PmMissingRoh" + i + ","
							+ "@Production" + i + ","
							+ "@ProductionDashboard" + i + ","
							+ "@ProductionHourPlanningSerieFirstOrder" + i + ","
							+ "@ProductionPlanningByCustomer" + i + ","
							+ "@ProductionWorkLoad" + i + ","
							+ "@ProductSale" + i + ","
							+ "@ProjectManagment" + i + ","
							+ "@SalesHbgUbg" + i + ","
							+ "@SalesInvoice" + i + ","
							+ "@SalesOrders" + i + ","
							+ "@SalesProduction" + i + ","
							+ "@SalesRoh" + i + ","
							+ "@Statistics" + i + ","
							+ "@StatisticsDel" + i + ","
							+ "@StatisticsFgAL" + i + ","
							+ "@StatisticsFgBETN" + i + ","
							+ "@StatisticsFgCZ" + i + ","
							+ "@StatisticsFgGZ" + i + ","
							+ "@StatisticsFgTN" + i + ","
							+ "@StatisticsFgWS" + i + ","
							+ "@StatisticsLogs" + i + ","
							+ "@StatisticsReasonUpdate" + i + ","
							+ "@StatisticsUmsatz" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
						sqlCommand.Parameters.AddWithValue("AllProductionWarehouses" + i, item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CtsPlanning" + i, item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
						sqlCommand.Parameters.AddWithValue("CtsSales" + i, item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
						sqlCommand.Parameters.AddWithValue("CustomerService" + i, item.CustomerService == null ? (object)DBNull.Value : item.CustomerService);
						sqlCommand.Parameters.AddWithValue("DashboardProduction" + i, item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
						sqlCommand.Parameters.AddWithValue("DashboardProductionEdit" + i, item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("PmExtraRoh" + i, item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
						sqlCommand.Parameters.AddWithValue("PmFaultyArticles" + i, item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
						sqlCommand.Parameters.AddWithValue("PmMissingRoh" + i, item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
						sqlCommand.Parameters.AddWithValue("Production" + i, item.Production == null ? (object)DBNull.Value : item.Production);
						sqlCommand.Parameters.AddWithValue("ProductionDashboard" + i, item.ProductionDashboard == null ? (object)DBNull.Value : item.ProductionDashboard);
						sqlCommand.Parameters.AddWithValue("ProductionHourPlanningSerieFirstOrder" + i, item.ProductionHourPlanningSerieFirstOrder == null ? (object)DBNull.Value : item.ProductionHourPlanningSerieFirstOrder);
						sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer" + i, item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
						sqlCommand.Parameters.AddWithValue("ProductionWorkLoad" + i, item.ProductionWorkLoad == null ? (object)DBNull.Value : item.ProductionWorkLoad);
						sqlCommand.Parameters.AddWithValue("ProductSale" + i, item.ProductSale == null ? (object)DBNull.Value : item.ProductSale);
						sqlCommand.Parameters.AddWithValue("ProjectManagment" + i, item.ProjectManagment == null ? (object)DBNull.Value : item.ProjectManagment);
						sqlCommand.Parameters.AddWithValue("SalesHbgUbg" + i, item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
						sqlCommand.Parameters.AddWithValue("SalesInvoice" + i, item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
						sqlCommand.Parameters.AddWithValue("SalesOrders" + i, item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
						sqlCommand.Parameters.AddWithValue("SalesProduction" + i, item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
						sqlCommand.Parameters.AddWithValue("SalesRoh" + i, item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
						sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
						sqlCommand.Parameters.AddWithValue("StatisticsDel" + i, item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
						sqlCommand.Parameters.AddWithValue("StatisticsFgAL" + i, item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
						sqlCommand.Parameters.AddWithValue("StatisticsFgBETN" + i, item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
						sqlCommand.Parameters.AddWithValue("StatisticsFgCZ" + i, item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
						sqlCommand.Parameters.AddWithValue("StatisticsFgGZ" + i, item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
						sqlCommand.Parameters.AddWithValue("StatisticsFgTN" + i, item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
						sqlCommand.Parameters.AddWithValue("StatisticsFgWS" + i, item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
						sqlCommand.Parameters.AddWithValue("StatisticsLogs" + i, item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
						sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate" + i, item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
						sqlCommand.Parameters.AddWithValue("StatisticsUmsatz" + i, item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MGO_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [AllProductionWarehouses]=@AllProductionWarehouses, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CtsPlanning]=@CtsPlanning, [CtsSales]=@CtsSales, [CustomerService]=@CustomerService, [DashboardProduction]=@DashboardProduction, [DashboardProductionEdit]=@DashboardProductionEdit, [ModuleActivated]=@ModuleActivated, [PmExtraRoh]=@PmExtraRoh, [PmFaultyArticles]=@PmFaultyArticles, [PmMissingRoh]=@PmMissingRoh, [Production]=@Production, [ProductionDashboard]=@ProductionDashboard, [ProductionHourPlanningSerieFirstOrder]=@ProductionHourPlanningSerieFirstOrder, [ProductionPlanningByCustomer]=@ProductionPlanningByCustomer, [ProductionWorkLoad]=@ProductionWorkLoad, [ProductSale]=@ProductSale, [ProjectManagment]=@ProjectManagment, [SalesHbgUbg]=@SalesHbgUbg, [SalesInvoice]=@SalesInvoice, [SalesOrders]=@SalesOrders, [SalesProduction]=@SalesProduction, [SalesRoh]=@SalesRoh, [Statistics]=@Statistics, [StatisticsDel]=@StatisticsDel, [StatisticsFgAL]=@StatisticsFgAL, [StatisticsFgBETN]=@StatisticsFgBETN, [StatisticsFgCZ]=@StatisticsFgCZ, [StatisticsFgGZ]=@StatisticsFgGZ, [StatisticsFgTN]=@StatisticsFgTN, [StatisticsFgWS]=@StatisticsFgWS, [StatisticsLogs]=@StatisticsLogs, [StatisticsReasonUpdate]=@StatisticsReasonUpdate, [StatisticsUmsatz]=@StatisticsUmsatz WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
				sqlCommand.Parameters.AddWithValue("AllProductionWarehouses", item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CtsPlanning", item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
				sqlCommand.Parameters.AddWithValue("CtsSales", item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
				sqlCommand.Parameters.AddWithValue("CustomerService", item.CustomerService == null ? (object)DBNull.Value : item.CustomerService);
				sqlCommand.Parameters.AddWithValue("DashboardProduction", item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
				sqlCommand.Parameters.AddWithValue("DashboardProductionEdit", item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("PmExtraRoh", item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
				sqlCommand.Parameters.AddWithValue("PmFaultyArticles", item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
				sqlCommand.Parameters.AddWithValue("PmMissingRoh", item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
				sqlCommand.Parameters.AddWithValue("Production", item.Production == null ? (object)DBNull.Value : item.Production);
				sqlCommand.Parameters.AddWithValue("ProductionDashboard", item.ProductionDashboard == null ? (object)DBNull.Value : item.ProductionDashboard);
				sqlCommand.Parameters.AddWithValue("ProductionHourPlanningSerieFirstOrder", item.ProductionHourPlanningSerieFirstOrder == null ? (object)DBNull.Value : item.ProductionHourPlanningSerieFirstOrder);
				sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer", item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
				sqlCommand.Parameters.AddWithValue("ProductionWorkLoad", item.ProductionWorkLoad == null ? (object)DBNull.Value : item.ProductionWorkLoad);
				sqlCommand.Parameters.AddWithValue("ProductSale", item.ProductSale == null ? (object)DBNull.Value : item.ProductSale);
				sqlCommand.Parameters.AddWithValue("ProjectManagment", item.ProjectManagment == null ? (object)DBNull.Value : item.ProjectManagment);
				sqlCommand.Parameters.AddWithValue("SalesHbgUbg", item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
				sqlCommand.Parameters.AddWithValue("SalesInvoice", item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
				sqlCommand.Parameters.AddWithValue("SalesOrders", item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
				sqlCommand.Parameters.AddWithValue("SalesProduction", item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
				sqlCommand.Parameters.AddWithValue("SalesRoh", item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
				sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
				sqlCommand.Parameters.AddWithValue("StatisticsDel", item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
				sqlCommand.Parameters.AddWithValue("StatisticsFgAL", item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
				sqlCommand.Parameters.AddWithValue("StatisticsFgBETN", item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
				sqlCommand.Parameters.AddWithValue("StatisticsFgCZ", item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
				sqlCommand.Parameters.AddWithValue("StatisticsFgGZ", item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
				sqlCommand.Parameters.AddWithValue("StatisticsFgTN", item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
				sqlCommand.Parameters.AddWithValue("StatisticsFgWS", item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
				sqlCommand.Parameters.AddWithValue("StatisticsLogs", item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
				sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate", item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
				sqlCommand.Parameters.AddWithValue("StatisticsUmsatz", item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 38; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> items)
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
						query += " UPDATE [__MGO_AccessProfile] SET "

							+ "[AccessProfileName]=@AccessProfileName" + i + ","
							+ "[Administration]=@Administration" + i + ","
							+ "[AllProductionWarehouses]=@AllProductionWarehouses" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CtsPlanning]=@CtsPlanning" + i + ","
							+ "[CtsSales]=@CtsSales" + i + ","
							+ "[CustomerService]=@CustomerService" + i + ","
							+ "[DashboardProduction]=@DashboardProduction" + i + ","
							+ "[DashboardProductionEdit]=@DashboardProductionEdit" + i + ","
							+ "[ModuleActivated]=@ModuleActivated" + i + ","
							+ "[PmExtraRoh]=@PmExtraRoh" + i + ","
							+ "[PmFaultyArticles]=@PmFaultyArticles" + i + ","
							+ "[PmMissingRoh]=@PmMissingRoh" + i + ","
							+ "[Production]=@Production" + i + ","
							+ "[ProductionDashboard]=@ProductionDashboard" + i + ","
							+ "[ProductionHourPlanningSerieFirstOrder]=@ProductionHourPlanningSerieFirstOrder" + i + ","
							+ "[ProductionPlanningByCustomer]=@ProductionPlanningByCustomer" + i + ","
							+ "[ProductionWorkLoad]=@ProductionWorkLoad" + i + ","
							+ "[ProductSale]=@ProductSale" + i + ","
							+ "[ProjectManagment]=@ProjectManagment" + i + ","
							+ "[SalesHbgUbg]=@SalesHbgUbg" + i + ","
							+ "[SalesInvoice]=@SalesInvoice" + i + ","
							+ "[SalesOrders]=@SalesOrders" + i + ","
							+ "[SalesProduction]=@SalesProduction" + i + ","
							+ "[SalesRoh]=@SalesRoh" + i + ","
							+ "[Statistics]=@Statistics" + i + ","
							+ "[StatisticsDel]=@StatisticsDel" + i + ","
							+ "[StatisticsFgAL]=@StatisticsFgAL" + i + ","
							+ "[StatisticsFgBETN]=@StatisticsFgBETN" + i + ","
							+ "[StatisticsFgCZ]=@StatisticsFgCZ" + i + ","
							+ "[StatisticsFgGZ]=@StatisticsFgGZ" + i + ","
							+ "[StatisticsFgTN]=@StatisticsFgTN" + i + ","
							+ "[StatisticsFgWS]=@StatisticsFgWS" + i + ","
							+ "[StatisticsLogs]=@StatisticsLogs" + i + ","
							+ "[StatisticsReasonUpdate]=@StatisticsReasonUpdate" + i + ","
							+ "[StatisticsUmsatz]=@StatisticsUmsatz" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
						sqlCommand.Parameters.AddWithValue("AllProductionWarehouses" + i, item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CtsPlanning" + i, item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
						sqlCommand.Parameters.AddWithValue("CtsSales" + i, item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
						sqlCommand.Parameters.AddWithValue("CustomerService" + i, item.CustomerService == null ? (object)DBNull.Value : item.CustomerService);
						sqlCommand.Parameters.AddWithValue("DashboardProduction" + i, item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
						sqlCommand.Parameters.AddWithValue("DashboardProductionEdit" + i, item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("PmExtraRoh" + i, item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
						sqlCommand.Parameters.AddWithValue("PmFaultyArticles" + i, item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
						sqlCommand.Parameters.AddWithValue("PmMissingRoh" + i, item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
						sqlCommand.Parameters.AddWithValue("Production" + i, item.Production == null ? (object)DBNull.Value : item.Production);
						sqlCommand.Parameters.AddWithValue("ProductionDashboard" + i, item.ProductionDashboard == null ? (object)DBNull.Value : item.ProductionDashboard);
						sqlCommand.Parameters.AddWithValue("ProductionHourPlanningSerieFirstOrder" + i, item.ProductionHourPlanningSerieFirstOrder == null ? (object)DBNull.Value : item.ProductionHourPlanningSerieFirstOrder);
						sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer" + i, item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
						sqlCommand.Parameters.AddWithValue("ProductionWorkLoad" + i, item.ProductionWorkLoad == null ? (object)DBNull.Value : item.ProductionWorkLoad);
						sqlCommand.Parameters.AddWithValue("ProductSale" + i, item.ProductSale == null ? (object)DBNull.Value : item.ProductSale);
						sqlCommand.Parameters.AddWithValue("ProjectManagment" + i, item.ProjectManagment == null ? (object)DBNull.Value : item.ProjectManagment);
						sqlCommand.Parameters.AddWithValue("SalesHbgUbg" + i, item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
						sqlCommand.Parameters.AddWithValue("SalesInvoice" + i, item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
						sqlCommand.Parameters.AddWithValue("SalesOrders" + i, item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
						sqlCommand.Parameters.AddWithValue("SalesProduction" + i, item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
						sqlCommand.Parameters.AddWithValue("SalesRoh" + i, item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
						sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
						sqlCommand.Parameters.AddWithValue("StatisticsDel" + i, item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
						sqlCommand.Parameters.AddWithValue("StatisticsFgAL" + i, item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
						sqlCommand.Parameters.AddWithValue("StatisticsFgBETN" + i, item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
						sqlCommand.Parameters.AddWithValue("StatisticsFgCZ" + i, item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
						sqlCommand.Parameters.AddWithValue("StatisticsFgGZ" + i, item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
						sqlCommand.Parameters.AddWithValue("StatisticsFgTN" + i, item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
						sqlCommand.Parameters.AddWithValue("StatisticsFgWS" + i, item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
						sqlCommand.Parameters.AddWithValue("StatisticsLogs" + i, item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
						sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate" + i, item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
						sqlCommand.Parameters.AddWithValue("StatisticsUmsatz" + i, item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);
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
				string query = "DELETE FROM [__MGO_AccessProfile] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__MGO_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__MGO_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__MGO_AccessProfile]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__MGO_AccessProfile] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__MGO_AccessProfile] ([AccessProfileName],[Administration],[AllProductionWarehouses],[CreationTime],[CreationUserId],[CtsPlanning],[CtsSales],[CustomerService],[DashboardProduction],[DashboardProductionEdit],[ModuleActivated],[PmExtraRoh],[PmFaultyArticles],[PmMissingRoh],[Production],[ProductionDashboard],[ProductionHourPlanningSerieFirstOrder],[ProductionPlanningByCustomer],[ProductionWorkLoad],[ProductSale],[ProjectManagment],[SalesHbgUbg],[SalesInvoice],[SalesOrders],[SalesProduction],[SalesRoh],[Statistics],[StatisticsDel],[StatisticsFgAL],[StatisticsFgBETN],[StatisticsFgCZ],[StatisticsFgGZ],[StatisticsFgTN],[StatisticsFgWS],[StatisticsLogs],[StatisticsReasonUpdate],[StatisticsUmsatz]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@Administration,@AllProductionWarehouses,@CreationTime,@CreationUserId,@CtsPlanning,@CtsSales,@CustomerService,@DashboardProduction,@DashboardProductionEdit,@ModuleActivated,@PmExtraRoh,@PmFaultyArticles,@PmMissingRoh,@Production,@ProductionDashboard,@ProductionHourPlanningSerieFirstOrder,@ProductionPlanningByCustomer,@ProductionWorkLoad,@ProductSale,@ProjectManagment,@SalesHbgUbg,@SalesInvoice,@SalesOrders,@SalesProduction,@SalesRoh,@Statistics,@StatisticsDel,@StatisticsFgAL,@StatisticsFgBETN,@StatisticsFgCZ,@StatisticsFgGZ,@StatisticsFgTN,@StatisticsFgWS,@StatisticsLogs,@StatisticsReasonUpdate,@StatisticsUmsatz); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
			sqlCommand.Parameters.AddWithValue("AllProductionWarehouses", item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CtsPlanning", item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
			sqlCommand.Parameters.AddWithValue("CtsSales", item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
			sqlCommand.Parameters.AddWithValue("CustomerService", item.CustomerService == null ? (object)DBNull.Value : item.CustomerService);
			sqlCommand.Parameters.AddWithValue("DashboardProduction", item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
			sqlCommand.Parameters.AddWithValue("DashboardProductionEdit", item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("PmExtraRoh", item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
			sqlCommand.Parameters.AddWithValue("PmFaultyArticles", item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
			sqlCommand.Parameters.AddWithValue("PmMissingRoh", item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
			sqlCommand.Parameters.AddWithValue("Production", item.Production == null ? (object)DBNull.Value : item.Production);
			sqlCommand.Parameters.AddWithValue("ProductionDashboard", item.ProductionDashboard == null ? (object)DBNull.Value : item.ProductionDashboard);
			sqlCommand.Parameters.AddWithValue("ProductionHourPlanningSerieFirstOrder", item.ProductionHourPlanningSerieFirstOrder == null ? (object)DBNull.Value : item.ProductionHourPlanningSerieFirstOrder);
			sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer", item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
			sqlCommand.Parameters.AddWithValue("ProductionWorkLoad", item.ProductionWorkLoad == null ? (object)DBNull.Value : item.ProductionWorkLoad);
			sqlCommand.Parameters.AddWithValue("ProductSale", item.ProductSale == null ? (object)DBNull.Value : item.ProductSale);
			sqlCommand.Parameters.AddWithValue("ProjectManagment", item.ProjectManagment == null ? (object)DBNull.Value : item.ProjectManagment);
			sqlCommand.Parameters.AddWithValue("SalesHbgUbg", item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
			sqlCommand.Parameters.AddWithValue("SalesInvoice", item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
			sqlCommand.Parameters.AddWithValue("SalesOrders", item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
			sqlCommand.Parameters.AddWithValue("SalesProduction", item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
			sqlCommand.Parameters.AddWithValue("SalesRoh", item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
			sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
			sqlCommand.Parameters.AddWithValue("StatisticsDel", item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
			sqlCommand.Parameters.AddWithValue("StatisticsFgAL", item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
			sqlCommand.Parameters.AddWithValue("StatisticsFgBETN", item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
			sqlCommand.Parameters.AddWithValue("StatisticsFgCZ", item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
			sqlCommand.Parameters.AddWithValue("StatisticsFgGZ", item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
			sqlCommand.Parameters.AddWithValue("StatisticsFgTN", item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
			sqlCommand.Parameters.AddWithValue("StatisticsFgWS", item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
			sqlCommand.Parameters.AddWithValue("StatisticsLogs", item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
			sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate", item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
			sqlCommand.Parameters.AddWithValue("StatisticsUmsatz", item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 38; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__MGO_AccessProfile] ([AccessProfileName],[Administration],[AllProductionWarehouses],[CreationTime],[CreationUserId],[CtsPlanning],[CtsSales],[CustomerService],[DashboardProduction],[DashboardProductionEdit],[ModuleActivated],[PmExtraRoh],[PmFaultyArticles],[PmMissingRoh],[Production],[ProductionDashboard],[ProductionHourPlanningSerieFirstOrder],[ProductionPlanningByCustomer],[ProductionWorkLoad],[ProductSale],[ProjectManagment],[SalesHbgUbg],[SalesInvoice],[SalesOrders],[SalesProduction],[SalesRoh],[Statistics],[StatisticsDel],[StatisticsFgAL],[StatisticsFgBETN],[StatisticsFgCZ],[StatisticsFgGZ],[StatisticsFgTN],[StatisticsFgWS],[StatisticsLogs],[StatisticsReasonUpdate],[StatisticsUmsatz]) VALUES ( "

						+ "@AccessProfileName" + i + ","
						+ "@Administration" + i + ","
						+ "@AllProductionWarehouses" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CtsPlanning" + i + ","
						+ "@CtsSales" + i + ","
						+ "@CustomerService" + i + ","
						+ "@DashboardProduction" + i + ","
						+ "@DashboardProductionEdit" + i + ","
						+ "@ModuleActivated" + i + ","
						+ "@PmExtraRoh" + i + ","
						+ "@PmFaultyArticles" + i + ","
						+ "@PmMissingRoh" + i + ","
						+ "@Production" + i + ","
						+ "@ProductionDashboard" + i + ","
						+ "@ProductionHourPlanningSerieFirstOrder" + i + ","
						+ "@ProductionPlanningByCustomer" + i + ","
						+ "@ProductionWorkLoad" + i + ","
						+ "@ProductSale" + i + ","
						+ "@ProjectManagment" + i + ","
						+ "@SalesHbgUbg" + i + ","
						+ "@SalesInvoice" + i + ","
						+ "@SalesOrders" + i + ","
						+ "@SalesProduction" + i + ","
						+ "@SalesRoh" + i + ","
						+ "@Statistics" + i + ","
						+ "@StatisticsDel" + i + ","
						+ "@StatisticsFgAL" + i + ","
						+ "@StatisticsFgBETN" + i + ","
						+ "@StatisticsFgCZ" + i + ","
						+ "@StatisticsFgGZ" + i + ","
						+ "@StatisticsFgTN" + i + ","
						+ "@StatisticsFgWS" + i + ","
						+ "@StatisticsLogs" + i + ","
						+ "@StatisticsReasonUpdate" + i + ","
						+ "@StatisticsUmsatz" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AllProductionWarehouses" + i, item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CtsPlanning" + i, item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
					sqlCommand.Parameters.AddWithValue("CtsSales" + i, item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
					sqlCommand.Parameters.AddWithValue("CustomerService" + i, item.CustomerService == null ? (object)DBNull.Value : item.CustomerService);
					sqlCommand.Parameters.AddWithValue("DashboardProduction" + i, item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
					sqlCommand.Parameters.AddWithValue("DashboardProductionEdit" + i, item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("PmExtraRoh" + i, item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
					sqlCommand.Parameters.AddWithValue("PmFaultyArticles" + i, item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
					sqlCommand.Parameters.AddWithValue("PmMissingRoh" + i, item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
					sqlCommand.Parameters.AddWithValue("Production" + i, item.Production == null ? (object)DBNull.Value : item.Production);
					sqlCommand.Parameters.AddWithValue("ProductionDashboard" + i, item.ProductionDashboard == null ? (object)DBNull.Value : item.ProductionDashboard);
					sqlCommand.Parameters.AddWithValue("ProductionHourPlanningSerieFirstOrder" + i, item.ProductionHourPlanningSerieFirstOrder == null ? (object)DBNull.Value : item.ProductionHourPlanningSerieFirstOrder);
					sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer" + i, item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
					sqlCommand.Parameters.AddWithValue("ProductionWorkLoad" + i, item.ProductionWorkLoad == null ? (object)DBNull.Value : item.ProductionWorkLoad);
					sqlCommand.Parameters.AddWithValue("ProductSale" + i, item.ProductSale == null ? (object)DBNull.Value : item.ProductSale);
					sqlCommand.Parameters.AddWithValue("ProjectManagment" + i, item.ProjectManagment == null ? (object)DBNull.Value : item.ProjectManagment);
					sqlCommand.Parameters.AddWithValue("SalesHbgUbg" + i, item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
					sqlCommand.Parameters.AddWithValue("SalesInvoice" + i, item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
					sqlCommand.Parameters.AddWithValue("SalesOrders" + i, item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
					sqlCommand.Parameters.AddWithValue("SalesProduction" + i, item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
					sqlCommand.Parameters.AddWithValue("SalesRoh" + i, item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
					sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatisticsDel" + i, item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
					sqlCommand.Parameters.AddWithValue("StatisticsFgAL" + i, item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
					sqlCommand.Parameters.AddWithValue("StatisticsFgBETN" + i, item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
					sqlCommand.Parameters.AddWithValue("StatisticsFgCZ" + i, item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
					sqlCommand.Parameters.AddWithValue("StatisticsFgGZ" + i, item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
					sqlCommand.Parameters.AddWithValue("StatisticsFgTN" + i, item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
					sqlCommand.Parameters.AddWithValue("StatisticsFgWS" + i, item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
					sqlCommand.Parameters.AddWithValue("StatisticsLogs" + i, item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
					sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate" + i, item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
					sqlCommand.Parameters.AddWithValue("StatisticsUmsatz" + i, item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__MGO_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [AllProductionWarehouses]=@AllProductionWarehouses, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CtsPlanning]=@CtsPlanning, [CtsSales]=@CtsSales, [CustomerService]=@CustomerService, [DashboardProduction]=@DashboardProduction, [DashboardProductionEdit]=@DashboardProductionEdit, [ModuleActivated]=@ModuleActivated, [PmExtraRoh]=@PmExtraRoh, [PmFaultyArticles]=@PmFaultyArticles, [PmMissingRoh]=@PmMissingRoh, [Production]=@Production, [ProductionDashboard]=@ProductionDashboard, [ProductionHourPlanningSerieFirstOrder]=@ProductionHourPlanningSerieFirstOrder, [ProductionPlanningByCustomer]=@ProductionPlanningByCustomer, [ProductionWorkLoad]=@ProductionWorkLoad, [ProductSale]=@ProductSale, [ProjectManagment]=@ProjectManagment, [SalesHbgUbg]=@SalesHbgUbg, [SalesInvoice]=@SalesInvoice, [SalesOrders]=@SalesOrders, [SalesProduction]=@SalesProduction, [SalesRoh]=@SalesRoh, [Statistics]=@Statistics, [StatisticsDel]=@StatisticsDel, [StatisticsFgAL]=@StatisticsFgAL, [StatisticsFgBETN]=@StatisticsFgBETN, [StatisticsFgCZ]=@StatisticsFgCZ, [StatisticsFgGZ]=@StatisticsFgGZ, [StatisticsFgTN]=@StatisticsFgTN, [StatisticsFgWS]=@StatisticsFgWS, [StatisticsLogs]=@StatisticsLogs, [StatisticsReasonUpdate]=@StatisticsReasonUpdate, [StatisticsUmsatz]=@StatisticsUmsatz WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
			sqlCommand.Parameters.AddWithValue("AllProductionWarehouses", item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CtsPlanning", item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
			sqlCommand.Parameters.AddWithValue("CtsSales", item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
			sqlCommand.Parameters.AddWithValue("CustomerService", item.CustomerService == null ? (object)DBNull.Value : item.CustomerService);
			sqlCommand.Parameters.AddWithValue("DashboardProduction", item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
			sqlCommand.Parameters.AddWithValue("DashboardProductionEdit", item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("PmExtraRoh", item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
			sqlCommand.Parameters.AddWithValue("PmFaultyArticles", item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
			sqlCommand.Parameters.AddWithValue("PmMissingRoh", item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
			sqlCommand.Parameters.AddWithValue("Production", item.Production == null ? (object)DBNull.Value : item.Production);
			sqlCommand.Parameters.AddWithValue("ProductionDashboard", item.ProductionDashboard == null ? (object)DBNull.Value : item.ProductionDashboard);
			sqlCommand.Parameters.AddWithValue("ProductionHourPlanningSerieFirstOrder", item.ProductionHourPlanningSerieFirstOrder == null ? (object)DBNull.Value : item.ProductionHourPlanningSerieFirstOrder);
			sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer", item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
			sqlCommand.Parameters.AddWithValue("ProductionWorkLoad", item.ProductionWorkLoad == null ? (object)DBNull.Value : item.ProductionWorkLoad);
			sqlCommand.Parameters.AddWithValue("ProductSale", item.ProductSale == null ? (object)DBNull.Value : item.ProductSale);
			sqlCommand.Parameters.AddWithValue("ProjectManagment", item.ProjectManagment == null ? (object)DBNull.Value : item.ProjectManagment);
			sqlCommand.Parameters.AddWithValue("SalesHbgUbg", item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
			sqlCommand.Parameters.AddWithValue("SalesInvoice", item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
			sqlCommand.Parameters.AddWithValue("SalesOrders", item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
			sqlCommand.Parameters.AddWithValue("SalesProduction", item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
			sqlCommand.Parameters.AddWithValue("SalesRoh", item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
			sqlCommand.Parameters.AddWithValue("Statistics", item.Statistics == null ? (object)DBNull.Value : item.Statistics);
			sqlCommand.Parameters.AddWithValue("StatisticsDel", item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
			sqlCommand.Parameters.AddWithValue("StatisticsFgAL", item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
			sqlCommand.Parameters.AddWithValue("StatisticsFgBETN", item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
			sqlCommand.Parameters.AddWithValue("StatisticsFgCZ", item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
			sqlCommand.Parameters.AddWithValue("StatisticsFgGZ", item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
			sqlCommand.Parameters.AddWithValue("StatisticsFgTN", item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
			sqlCommand.Parameters.AddWithValue("StatisticsFgWS", item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
			sqlCommand.Parameters.AddWithValue("StatisticsLogs", item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
			sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate", item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
			sqlCommand.Parameters.AddWithValue("StatisticsUmsatz", item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 38; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__MGO_AccessProfile] SET "

					+ "[AccessProfileName]=@AccessProfileName" + i + ","
					+ "[Administration]=@Administration" + i + ","
					+ "[AllProductionWarehouses]=@AllProductionWarehouses" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CtsPlanning]=@CtsPlanning" + i + ","
					+ "[CtsSales]=@CtsSales" + i + ","
					+ "[CustomerService]=@CustomerService" + i + ","
					+ "[DashboardProduction]=@DashboardProduction" + i + ","
					+ "[DashboardProductionEdit]=@DashboardProductionEdit" + i + ","
					+ "[ModuleActivated]=@ModuleActivated" + i + ","
					+ "[PmExtraRoh]=@PmExtraRoh" + i + ","
					+ "[PmFaultyArticles]=@PmFaultyArticles" + i + ","
					+ "[PmMissingRoh]=@PmMissingRoh" + i + ","
					+ "[Production]=@Production" + i + ","
					+ "[ProductionDashboard]=@ProductionDashboard" + i + ","
					+ "[ProductionHourPlanningSerieFirstOrder]=@ProductionHourPlanningSerieFirstOrder" + i + ","
					+ "[ProductionPlanningByCustomer]=@ProductionPlanningByCustomer" + i + ","
					+ "[ProductionWorkLoad]=@ProductionWorkLoad" + i + ","
					+ "[ProductSale]=@ProductSale" + i + ","
					+ "[ProjectManagment]=@ProjectManagment" + i + ","
					+ "[SalesHbgUbg]=@SalesHbgUbg" + i + ","
					+ "[SalesInvoice]=@SalesInvoice" + i + ","
					+ "[SalesOrders]=@SalesOrders" + i + ","
					+ "[SalesProduction]=@SalesProduction" + i + ","
					+ "[SalesRoh]=@SalesRoh" + i + ","
					+ "[Statistics]=@Statistics" + i + ","
					+ "[StatisticsDel]=@StatisticsDel" + i + ","
					+ "[StatisticsFgAL]=@StatisticsFgAL" + i + ","
					+ "[StatisticsFgBETN]=@StatisticsFgBETN" + i + ","
					+ "[StatisticsFgCZ]=@StatisticsFgCZ" + i + ","
					+ "[StatisticsFgGZ]=@StatisticsFgGZ" + i + ","
					+ "[StatisticsFgTN]=@StatisticsFgTN" + i + ","
					+ "[StatisticsFgWS]=@StatisticsFgWS" + i + ","
					+ "[StatisticsLogs]=@StatisticsLogs" + i + ","
					+ "[StatisticsReasonUpdate]=@StatisticsReasonUpdate" + i + ","
					+ "[StatisticsUmsatz]=@StatisticsUmsatz" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration == null ? (object)DBNull.Value : item.Administration);
					sqlCommand.Parameters.AddWithValue("AllProductionWarehouses" + i, item.AllProductionWarehouses == null ? (object)DBNull.Value : item.AllProductionWarehouses);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CtsPlanning" + i, item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
					sqlCommand.Parameters.AddWithValue("CtsSales" + i, item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
					sqlCommand.Parameters.AddWithValue("CustomerService" + i, item.CustomerService == null ? (object)DBNull.Value : item.CustomerService);
					sqlCommand.Parameters.AddWithValue("DashboardProduction" + i, item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
					sqlCommand.Parameters.AddWithValue("DashboardProductionEdit" + i, item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("PmExtraRoh" + i, item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
					sqlCommand.Parameters.AddWithValue("PmFaultyArticles" + i, item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
					sqlCommand.Parameters.AddWithValue("PmMissingRoh" + i, item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
					sqlCommand.Parameters.AddWithValue("Production" + i, item.Production == null ? (object)DBNull.Value : item.Production);
					sqlCommand.Parameters.AddWithValue("ProductionDashboard" + i, item.ProductionDashboard == null ? (object)DBNull.Value : item.ProductionDashboard);
					sqlCommand.Parameters.AddWithValue("ProductionHourPlanningSerieFirstOrder" + i, item.ProductionHourPlanningSerieFirstOrder == null ? (object)DBNull.Value : item.ProductionHourPlanningSerieFirstOrder);
					sqlCommand.Parameters.AddWithValue("ProductionPlanningByCustomer" + i, item.ProductionPlanningByCustomer == null ? (object)DBNull.Value : item.ProductionPlanningByCustomer);
					sqlCommand.Parameters.AddWithValue("ProductionWorkLoad" + i, item.ProductionWorkLoad == null ? (object)DBNull.Value : item.ProductionWorkLoad);
					sqlCommand.Parameters.AddWithValue("ProductSale" + i, item.ProductSale == null ? (object)DBNull.Value : item.ProductSale);
					sqlCommand.Parameters.AddWithValue("ProjectManagment" + i, item.ProjectManagment == null ? (object)DBNull.Value : item.ProjectManagment);
					sqlCommand.Parameters.AddWithValue("SalesHbgUbg" + i, item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
					sqlCommand.Parameters.AddWithValue("SalesInvoice" + i, item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
					sqlCommand.Parameters.AddWithValue("SalesOrders" + i, item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
					sqlCommand.Parameters.AddWithValue("SalesProduction" + i, item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
					sqlCommand.Parameters.AddWithValue("SalesRoh" + i, item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
					sqlCommand.Parameters.AddWithValue("Statistics" + i, item.Statistics == null ? (object)DBNull.Value : item.Statistics);
					sqlCommand.Parameters.AddWithValue("StatisticsDel" + i, item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
					sqlCommand.Parameters.AddWithValue("StatisticsFgAL" + i, item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
					sqlCommand.Parameters.AddWithValue("StatisticsFgBETN" + i, item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
					sqlCommand.Parameters.AddWithValue("StatisticsFgCZ" + i, item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
					sqlCommand.Parameters.AddWithValue("StatisticsFgGZ" + i, item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
					sqlCommand.Parameters.AddWithValue("StatisticsFgTN" + i, item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
					sqlCommand.Parameters.AddWithValue("StatisticsFgWS" + i, item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
					sqlCommand.Parameters.AddWithValue("StatisticsLogs" + i, item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
					sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate" + i, item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
					sqlCommand.Parameters.AddWithValue("StatisticsUmsatz" + i, item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__MGO_AccessProfile] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__MGO_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity GetByName(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MGO_AccessProfile] WHERE [AccessProfileName]=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateName(Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MGO_AccessProfile] SET [AccessProfileName]=@AccessProfileName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateWoCreationData(Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MGO_AccessProfile] SET  [Administration]=@Administration, [CtsPlanning]=@CtsPlanning, [CtsSales]=@CtsSales, [PmExtraRoh]=@PmExtraRoh, [PmFaultyArticles]=@PmFaultyArticles, [PmMissingRoh]=@PmMissingRoh, [SalesHbgUbg]=@SalesHbgUbg, [SalesInvoice]=@SalesInvoice, [SalesOrders]=@SalesOrders, [SalesProduction]=@SalesProduction, [SalesRoh]=@SalesRoh, [StatisticsDel]=@StatisticsDel, [StatisticsFgAL]=@StatisticsFgAL, [StatisticsFgBETN]=@StatisticsFgBETN, [StatisticsFgCZ]=@StatisticsFgCZ, [StatisticsFgGZ]=@StatisticsFgGZ, [StatisticsFgTN]=@StatisticsFgTN, [StatisticsFgWS]=@StatisticsFgWS, [StatisticsLogs]=@StatisticsLogs, [StatisticsReasonUpdate]=@StatisticsReasonUpdate, [StatisticsUmsatz]=@StatisticsUmsatz,[DashboardProduction]=@DashboardProduction,[DashboardProductionEdit]=@DashboardProductionEdit  WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration == null ? (object)DBNull.Value : item.Administration);
				sqlCommand.Parameters.AddWithValue("CtsPlanning", item.CtsPlanning == null ? (object)DBNull.Value : item.CtsPlanning);
				sqlCommand.Parameters.AddWithValue("CtsSales", item.CtsSales == null ? (object)DBNull.Value : item.CtsSales);
				sqlCommand.Parameters.AddWithValue("PmExtraRoh", item.PmExtraRoh == null ? (object)DBNull.Value : item.PmExtraRoh);
				sqlCommand.Parameters.AddWithValue("PmFaultyArticles", item.PmFaultyArticles == null ? (object)DBNull.Value : item.PmFaultyArticles);
				sqlCommand.Parameters.AddWithValue("PmMissingRoh", item.PmMissingRoh == null ? (object)DBNull.Value : item.PmMissingRoh);
				sqlCommand.Parameters.AddWithValue("SalesHbgUbg", item.SalesHbgUbg == null ? (object)DBNull.Value : item.SalesHbgUbg);
				sqlCommand.Parameters.AddWithValue("SalesInvoice", item.SalesInvoice == null ? (object)DBNull.Value : item.SalesInvoice);
				sqlCommand.Parameters.AddWithValue("SalesOrders", item.SalesOrders == null ? (object)DBNull.Value : item.SalesOrders);
				sqlCommand.Parameters.AddWithValue("SalesProduction", item.SalesProduction == null ? (object)DBNull.Value : item.SalesProduction);
				sqlCommand.Parameters.AddWithValue("SalesRoh", item.SalesRoh == null ? (object)DBNull.Value : item.SalesRoh);
				sqlCommand.Parameters.AddWithValue("StatisticsDel", item.StatisticsDel == null ? (object)DBNull.Value : item.StatisticsDel);
				sqlCommand.Parameters.AddWithValue("StatisticsFgAL", item.StatisticsFgAL == null ? (object)DBNull.Value : item.StatisticsFgAL);
				sqlCommand.Parameters.AddWithValue("StatisticsFgBETN", item.StatisticsFgBETN == null ? (object)DBNull.Value : item.StatisticsFgBETN);
				sqlCommand.Parameters.AddWithValue("StatisticsFgCZ", item.StatisticsFgCZ == null ? (object)DBNull.Value : item.StatisticsFgCZ);
				sqlCommand.Parameters.AddWithValue("StatisticsFgGZ", item.StatisticsFgGZ == null ? (object)DBNull.Value : item.StatisticsFgGZ);
				sqlCommand.Parameters.AddWithValue("StatisticsFgTN", item.StatisticsFgTN == null ? (object)DBNull.Value : item.StatisticsFgTN);
				sqlCommand.Parameters.AddWithValue("StatisticsFgWS", item.StatisticsFgWS == null ? (object)DBNull.Value : item.StatisticsFgWS);
				sqlCommand.Parameters.AddWithValue("StatisticsLogs", item.StatisticsLogs == null ? (object)DBNull.Value : item.StatisticsLogs);
				sqlCommand.Parameters.AddWithValue("StatisticsReasonUpdate", item.StatisticsReasonUpdate == null ? (object)DBNull.Value : item.StatisticsReasonUpdate);
				sqlCommand.Parameters.AddWithValue("StatisticsUmsatz", item.StatisticsUmsatz == null ? (object)DBNull.Value : item.StatisticsUmsatz);
				sqlCommand.Parameters.AddWithValue("DashboardProduction", item.DashboardProduction == null ? (object)DBNull.Value : item.DashboardProduction);
				sqlCommand.Parameters.AddWithValue("DashboardProductionEdit", item.DashboardProductionEdit == null ? (object)DBNull.Value : item.DashboardProductionEdit);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion Custom Methods
	}
}