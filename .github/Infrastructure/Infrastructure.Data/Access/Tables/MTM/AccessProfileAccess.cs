using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
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

					sqlCommand.CommandText = $"SELECT * FROM [__MTM_AccessProfile] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_AccessProfile] ([AccessProfileName],[Administration],[CreationTime],[CreationUserId],[CRP_AllRessourcesAuthorized],[CRP_Capacity],[CRP_CapacityEdit],[CRP_CapacityPlan],[CRP_CapacityPlanEdit],[CRP_Configuration],[CRP_ConfigurationEdit],[CRP_Holiday],[CRP_HolidayEdit],[CRP_RessourceAuthorizationEdit],[CRP_Validation],[CRP_ValidationEdit],[DISPO_Dashboard],[IsDefault],[ModuleActivated],[ORD_Dashboard],[ORD_Order],[ORD_OrderAdd],[ORD_OrderDelete],[ORD_OrderEdit],[ORD_OrderQuickPO],[ORD_OrderUnValidate],[ORD_OrderValidate],[ORD_ProjectPurchaseDeleteOrder],[ORD_ProjectPurchaseSetOrder],[Rahmen],[RahmenAdd],[RahmenAddPositions],[RahmenCancelation],[RahmenClosure],[RahmenDelete],[RahmenDeletePositions],[RahmenDocumentFlow],[RahmenEditHeader],[RahmenEditPositions],[RahmenHistory],[RahmenValdation],[STAT_Dashboard],[we],[WE_Create]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@Administration,@CreationTime,@CreationUserId,@CRP_AllRessourcesAuthorized,@CRP_Capacity,@CRP_CapacityEdit,@CRP_CapacityPlan,@CRP_CapacityPlanEdit,@CRP_Configuration,@CRP_ConfigurationEdit,@CRP_Holiday,@CRP_HolidayEdit,@CRP_RessourceAuthorizationEdit,@CRP_Validation,@CRP_ValidationEdit,@DISPO_Dashboard,@IsDefault,@ModuleActivated,@ORD_Dashboard,@ORD_Order,@ORD_OrderAdd,@ORD_OrderDelete,@ORD_OrderEdit,@ORD_OrderQuickPO,@ORD_OrderUnValidate,@ORD_OrderValidate,@ORD_ProjectPurchaseDeleteOrder,@ORD_ProjectPurchaseSetOrder,@Rahmen,@RahmenAdd,@RahmenAddPositions,@RahmenCancelation,@RahmenClosure,@RahmenDelete,@RahmenDeletePositions,@RahmenDocumentFlow,@RahmenEditHeader,@RahmenEditPositions,@RahmenHistory,@RahmenValdation,@STAT_Dashboard,@we,@WE_Create); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized", item.CRP_AllRessourcesAuthorized);
					sqlCommand.Parameters.AddWithValue("CRP_Capacity", item.CRP_Capacity);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit", item.CRP_CapacityEdit);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan", item.CRP_CapacityPlan);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit", item.CRP_CapacityPlanEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Configuration", item.CRP_Configuration);
					sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit", item.CRP_ConfigurationEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Holiday", item.CRP_Holiday);
					sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit", item.CRP_HolidayEdit);
					sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit", item.CRP_RessourceAuthorizationEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Validation", item.CRP_Validation);
					sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit", item.CRP_ValidationEdit);
					sqlCommand.Parameters.AddWithValue("DISPO_Dashboard", item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
					sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault);
					sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ORD_Dashboard", item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
					sqlCommand.Parameters.AddWithValue("ORD_Order", item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
					sqlCommand.Parameters.AddWithValue("ORD_OrderAdd", item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
					sqlCommand.Parameters.AddWithValue("ORD_OrderDelete", item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
					sqlCommand.Parameters.AddWithValue("ORD_OrderEdit", item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
					sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO", item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
					sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate", item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
					sqlCommand.Parameters.AddWithValue("ORD_OrderValidate", item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
					sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder", item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
					sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder", item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
					sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
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
					sqlCommand.Parameters.AddWithValue("STAT_Dashboard", item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
					sqlCommand.Parameters.AddWithValue("we", item.WE == null ? (object)DBNull.Value : item.WE);
					sqlCommand.Parameters.AddWithValue("WE_Create", item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 45; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__MTM_AccessProfile] ([AccessProfileName],[Administration],[CreationTime],[CreationUserId],[CRP_AllRessourcesAuthorized],[CRP_Capacity],[CRP_CapacityEdit],[CRP_CapacityPlan],[CRP_CapacityPlanEdit],[CRP_Configuration],[CRP_ConfigurationEdit],[CRP_Holiday],[CRP_HolidayEdit],[CRP_RessourceAuthorizationEdit],[CRP_Validation],[CRP_ValidationEdit],[DISPO_Dashboard],[IsDefault],[ModuleActivated],[ORD_Dashboard],[ORD_Order],[ORD_OrderAdd],[ORD_OrderDelete],[ORD_OrderEdit],[ORD_OrderQuickPO],[ORD_OrderUnValidate],[ORD_OrderValidate],[ORD_ProjectPurchaseDeleteOrder],[ORD_ProjectPurchaseSetOrder],[Rahmen],[RahmenAdd],[RahmenAddPositions],[RahmenCancelation],[RahmenClosure],[RahmenDelete],[RahmenDeletePositions],[RahmenDocumentFlow],[RahmenEditHeader],[RahmenEditPositions],[RahmenHistory],[RahmenValdation],[STAT_Dashboard],[we],[WE_Create]) VALUES ( "

							+ "@AccessProfileName" + i + ","
							+ "@Administration" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CRP_AllRessourcesAuthorized" + i + ","
							+ "@CRP_Capacity" + i + ","
							+ "@CRP_CapacityEdit" + i + ","
							+ "@CRP_CapacityPlan" + i + ","
							+ "@CRP_CapacityPlanEdit" + i + ","
							+ "@CRP_Configuration" + i + ","
							+ "@CRP_ConfigurationEdit" + i + ","
							+ "@CRP_Holiday" + i + ","
							+ "@CRP_HolidayEdit" + i + ","
							+ "@CRP_RessourceAuthorizationEdit" + i + ","
							+ "@CRP_Validation" + i + ","
							+ "@CRP_ValidationEdit" + i + ","
							+ "@DISPO_Dashboard" + i + ","
							+ "@IsDefault" + i + ","
							+ "@ModuleActivated" + i + ","
							+ "@ORD_Dashboard" + i + ","
							+ "@ORD_Order" + i + ","
							+ "@ORD_OrderAdd" + i + ","
							+ "@ORD_OrderDelete" + i + ","
							+ "@ORD_OrderEdit" + i + ","
							+ "@ORD_OrderQuickPO" + i + ","
							+ "@ORD_OrderUnValidate" + i + ","
							+ "@ORD_OrderValidate" + i + ","
							+ "@ORD_ProjectPurchaseDeleteOrder" + i + ","
							+ "@ORD_ProjectPurchaseSetOrder" + i + ","
							+ "@Rahmen" + i + ","
							+ "@RahmenAdd" + i + ","
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
							+ "@STAT_Dashboard" + i + ","
							+ "@we" + i + ","
							+ "@WE_Create" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized" + i, item.CRP_AllRessourcesAuthorized);
						sqlCommand.Parameters.AddWithValue("CRP_Capacity" + i, item.CRP_Capacity);
						sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit" + i, item.CRP_CapacityEdit);
						sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan" + i, item.CRP_CapacityPlan);
						sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit" + i, item.CRP_CapacityPlanEdit);
						sqlCommand.Parameters.AddWithValue("CRP_Configuration" + i, item.CRP_Configuration);
						sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit" + i, item.CRP_ConfigurationEdit);
						sqlCommand.Parameters.AddWithValue("CRP_Holiday" + i, item.CRP_Holiday);
						sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit" + i, item.CRP_HolidayEdit);
						sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit" + i, item.CRP_RessourceAuthorizationEdit);
						sqlCommand.Parameters.AddWithValue("CRP_Validation" + i, item.CRP_Validation);
						sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit" + i, item.CRP_ValidationEdit);
						sqlCommand.Parameters.AddWithValue("DISPO_Dashboard" + i, item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
						sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("ORD_Dashboard" + i, item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
						sqlCommand.Parameters.AddWithValue("ORD_Order" + i, item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
						sqlCommand.Parameters.AddWithValue("ORD_OrderAdd" + i, item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
						sqlCommand.Parameters.AddWithValue("ORD_OrderDelete" + i, item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
						sqlCommand.Parameters.AddWithValue("ORD_OrderEdit" + i, item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
						sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO" + i, item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
						sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate" + i, item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
						sqlCommand.Parameters.AddWithValue("ORD_OrderValidate" + i, item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
						sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder" + i, item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
						sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder" + i, item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
						sqlCommand.Parameters.AddWithValue("RahmenAdd" + i, item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
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
						sqlCommand.Parameters.AddWithValue("STAT_Dashboard" + i, item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
						sqlCommand.Parameters.AddWithValue("we" + i, item.WE == null ? (object)DBNull.Value : item.WE);
						sqlCommand.Parameters.AddWithValue("WE_Create" + i, item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CRP_AllRessourcesAuthorized]=@CRP_AllRessourcesAuthorized, [CRP_Capacity]=@CRP_Capacity, [CRP_CapacityEdit]=@CRP_CapacityEdit, [CRP_CapacityPlan]=@CRP_CapacityPlan, [CRP_CapacityPlanEdit]=@CRP_CapacityPlanEdit, [CRP_Configuration]=@CRP_Configuration, [CRP_ConfigurationEdit]=@CRP_ConfigurationEdit, [CRP_Holiday]=@CRP_Holiday, [CRP_HolidayEdit]=@CRP_HolidayEdit, [CRP_RessourceAuthorizationEdit]=@CRP_RessourceAuthorizationEdit, [CRP_Validation]=@CRP_Validation, [CRP_ValidationEdit]=@CRP_ValidationEdit, [DISPO_Dashboard]=@DISPO_Dashboard, [IsDefault]=@IsDefault, [ModuleActivated]=@ModuleActivated, [ORD_Dashboard]=@ORD_Dashboard, [ORD_Order]=@ORD_Order, [ORD_OrderAdd]=@ORD_OrderAdd, [ORD_OrderDelete]=@ORD_OrderDelete, [ORD_OrderEdit]=@ORD_OrderEdit, [ORD_OrderQuickPO]=@ORD_OrderQuickPO, [ORD_OrderUnValidate]=@ORD_OrderUnValidate, [ORD_OrderValidate]=@ORD_OrderValidate, [ORD_ProjectPurchaseDeleteOrder]=@ORD_ProjectPurchaseDeleteOrder, [ORD_ProjectPurchaseSetOrder]=@ORD_ProjectPurchaseSetOrder, [Rahmen]=@Rahmen, [RahmenAdd]=@RahmenAdd, [RahmenAddPositions]=@RahmenAddPositions, [RahmenCancelation]=@RahmenCancelation, [RahmenClosure]=@RahmenClosure, [RahmenDelete]=@RahmenDelete, [RahmenDeletePositions]=@RahmenDeletePositions, [RahmenDocumentFlow]=@RahmenDocumentFlow, [RahmenEditHeader]=@RahmenEditHeader, [RahmenEditPositions]=@RahmenEditPositions, [RahmenHistory]=@RahmenHistory, [RahmenValdation]=@RahmenValdation, [STAT_Dashboard]=@STAT_Dashboard, [we]=@we, [WE_Create]=@WE_Create WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized", item.CRP_AllRessourcesAuthorized);
				sqlCommand.Parameters.AddWithValue("CRP_Capacity", item.CRP_Capacity);
				sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit", item.CRP_CapacityEdit);
				sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan", item.CRP_CapacityPlan);
				sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit", item.CRP_CapacityPlanEdit);
				sqlCommand.Parameters.AddWithValue("CRP_Configuration", item.CRP_Configuration);
				sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit", item.CRP_ConfigurationEdit);
				sqlCommand.Parameters.AddWithValue("CRP_Holiday", item.CRP_Holiday);
				sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit", item.CRP_HolidayEdit);
				sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit", item.CRP_RessourceAuthorizationEdit);
				sqlCommand.Parameters.AddWithValue("CRP_Validation", item.CRP_Validation);
				sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit", item.CRP_ValidationEdit);
				sqlCommand.Parameters.AddWithValue("DISPO_Dashboard", item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
				sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("ORD_Dashboard", item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
				sqlCommand.Parameters.AddWithValue("ORD_Order", item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
				sqlCommand.Parameters.AddWithValue("ORD_OrderAdd", item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
				sqlCommand.Parameters.AddWithValue("ORD_OrderDelete", item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
				sqlCommand.Parameters.AddWithValue("ORD_OrderEdit", item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
				sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO", item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
				sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate", item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
				sqlCommand.Parameters.AddWithValue("ORD_OrderValidate", item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
				sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder", item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
				sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder", item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
				sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
				sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
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
				sqlCommand.Parameters.AddWithValue("STAT_Dashboard", item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
				sqlCommand.Parameters.AddWithValue("we", item.WE == null ? (object)DBNull.Value : item.WE);
				sqlCommand.Parameters.AddWithValue("WE_Create", item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 45; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__MTM_AccessProfile] SET "

							+ "[AccessProfileName]=@AccessProfileName" + i + ","
							+ "[Administration]=@Administration" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CRP_AllRessourcesAuthorized]=@CRP_AllRessourcesAuthorized" + i + ","
							+ "[CRP_Capacity]=@CRP_Capacity" + i + ","
							+ "[CRP_CapacityEdit]=@CRP_CapacityEdit" + i + ","
							+ "[CRP_CapacityPlan]=@CRP_CapacityPlan" + i + ","
							+ "[CRP_CapacityPlanEdit]=@CRP_CapacityPlanEdit" + i + ","
							+ "[CRP_Configuration]=@CRP_Configuration" + i + ","
							+ "[CRP_ConfigurationEdit]=@CRP_ConfigurationEdit" + i + ","
							+ "[CRP_Holiday]=@CRP_Holiday" + i + ","
							+ "[CRP_HolidayEdit]=@CRP_HolidayEdit" + i + ","
							+ "[CRP_RessourceAuthorizationEdit]=@CRP_RessourceAuthorizationEdit" + i + ","
							+ "[CRP_Validation]=@CRP_Validation" + i + ","
							+ "[CRP_ValidationEdit]=@CRP_ValidationEdit" + i + ","
							+ "[DISPO_Dashboard]=@DISPO_Dashboard" + i + ","
							+ "[IsDefault]=@IsDefault" + i + ","
							+ "[ModuleActivated]=@ModuleActivated" + i + ","
							+ "[ORD_Dashboard]=@ORD_Dashboard" + i + ","
							+ "[ORD_Order]=@ORD_Order" + i + ","
							+ "[ORD_OrderAdd]=@ORD_OrderAdd" + i + ","
							+ "[ORD_OrderDelete]=@ORD_OrderDelete" + i + ","
							+ "[ORD_OrderEdit]=@ORD_OrderEdit" + i + ","
							+ "[ORD_OrderQuickPO]=@ORD_OrderQuickPO" + i + ","
							+ "[ORD_OrderUnValidate]=@ORD_OrderUnValidate" + i + ","
							+ "[ORD_OrderValidate]=@ORD_OrderValidate" + i + ","
							+ "[ORD_ProjectPurchaseDeleteOrder]=@ORD_ProjectPurchaseDeleteOrder" + i + ","
							+ "[ORD_ProjectPurchaseSetOrder]=@ORD_ProjectPurchaseSetOrder" + i + ","
							+ "[Rahmen]=@Rahmen" + i + ","
							+ "[RahmenAdd]=@RahmenAdd" + i + ","
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
							+ "[STAT_Dashboard]=@STAT_Dashboard" + i + ","
							+ "[we]=@we" + i + ","
							+ "[WE_Create]=@WE_Create" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized" + i, item.CRP_AllRessourcesAuthorized);
						sqlCommand.Parameters.AddWithValue("CRP_Capacity" + i, item.CRP_Capacity);
						sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit" + i, item.CRP_CapacityEdit);
						sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan" + i, item.CRP_CapacityPlan);
						sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit" + i, item.CRP_CapacityPlanEdit);
						sqlCommand.Parameters.AddWithValue("CRP_Configuration" + i, item.CRP_Configuration);
						sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit" + i, item.CRP_ConfigurationEdit);
						sqlCommand.Parameters.AddWithValue("CRP_Holiday" + i, item.CRP_Holiday);
						sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit" + i, item.CRP_HolidayEdit);
						sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit" + i, item.CRP_RessourceAuthorizationEdit);
						sqlCommand.Parameters.AddWithValue("CRP_Validation" + i, item.CRP_Validation);
						sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit" + i, item.CRP_ValidationEdit);
						sqlCommand.Parameters.AddWithValue("DISPO_Dashboard" + i, item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
						sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("ORD_Dashboard" + i, item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
						sqlCommand.Parameters.AddWithValue("ORD_Order" + i, item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
						sqlCommand.Parameters.AddWithValue("ORD_OrderAdd" + i, item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
						sqlCommand.Parameters.AddWithValue("ORD_OrderDelete" + i, item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
						sqlCommand.Parameters.AddWithValue("ORD_OrderEdit" + i, item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
						sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO" + i, item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
						sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate" + i, item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
						sqlCommand.Parameters.AddWithValue("ORD_OrderValidate" + i, item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
						sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder" + i, item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
						sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder" + i, item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
						sqlCommand.Parameters.AddWithValue("RahmenAdd" + i, item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
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
						sqlCommand.Parameters.AddWithValue("STAT_Dashboard" + i, item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
						sqlCommand.Parameters.AddWithValue("we" + i, item.WE == null ? (object)DBNull.Value : item.WE);
						sqlCommand.Parameters.AddWithValue("WE_Create" + i, item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_AccessProfile] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
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

					string query = "DELETE FROM [__MTM_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__MTM_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__MTM_AccessProfile]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__MTM_AccessProfile] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__MTM_AccessProfile] ([AccessProfileName],[Administration],[CreationTime],[CreationUserId],[CRP_AllRessourcesAuthorized],[CRP_Capacity],[CRP_CapacityEdit],[CRP_CapacityPlan],[CRP_CapacityPlanEdit],[CRP_Configuration],[CRP_ConfigurationEdit],[CRP_Holiday],[CRP_HolidayEdit],[CRP_RessourceAuthorizationEdit],[CRP_Validation],[CRP_ValidationEdit],[DISPO_Dashboard],[IsDefault],[ModuleActivated],[ORD_Dashboard],[ORD_Order],[ORD_OrderAdd],[ORD_OrderDelete],[ORD_OrderEdit],[ORD_OrderQuickPO],[ORD_OrderUnValidate],[ORD_OrderValidate],[ORD_ProjectPurchaseDeleteOrder],[ORD_ProjectPurchaseSetOrder],[Rahmen],[RahmenAdd],[RahmenAddPositions],[RahmenCancelation],[RahmenClosure],[RahmenDelete],[RahmenDeletePositions],[RahmenDocumentFlow],[RahmenEditHeader],[RahmenEditPositions],[RahmenHistory],[RahmenValdation],[STAT_Dashboard],[we],[WE_Create]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileName,@Administration,@CreationTime,@CreationUserId,@CRP_AllRessourcesAuthorized,@CRP_Capacity,@CRP_CapacityEdit,@CRP_CapacityPlan,@CRP_CapacityPlanEdit,@CRP_Configuration,@CRP_ConfigurationEdit,@CRP_Holiday,@CRP_HolidayEdit,@CRP_RessourceAuthorizationEdit,@CRP_Validation,@CRP_ValidationEdit,@DISPO_Dashboard,@IsDefault,@ModuleActivated,@ORD_Dashboard,@ORD_Order,@ORD_OrderAdd,@ORD_OrderDelete,@ORD_OrderEdit,@ORD_OrderQuickPO,@ORD_OrderUnValidate,@ORD_OrderValidate,@ORD_ProjectPurchaseDeleteOrder,@ORD_ProjectPurchaseSetOrder,@Rahmen,@RahmenAdd,@RahmenAddPositions,@RahmenCancelation,@RahmenClosure,@RahmenDelete,@RahmenDeletePositions,@RahmenDocumentFlow,@RahmenEditHeader,@RahmenEditPositions,@RahmenHistory,@RahmenValdation,@STAT_Dashboard,@we,@WE_Create); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized", item.CRP_AllRessourcesAuthorized);
			sqlCommand.Parameters.AddWithValue("CRP_Capacity", item.CRP_Capacity);
			sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit", item.CRP_CapacityEdit);
			sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan", item.CRP_CapacityPlan);
			sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit", item.CRP_CapacityPlanEdit);
			sqlCommand.Parameters.AddWithValue("CRP_Configuration", item.CRP_Configuration);
			sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit", item.CRP_ConfigurationEdit);
			sqlCommand.Parameters.AddWithValue("CRP_Holiday", item.CRP_Holiday);
			sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit", item.CRP_HolidayEdit);
			sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit", item.CRP_RessourceAuthorizationEdit);
			sqlCommand.Parameters.AddWithValue("CRP_Validation", item.CRP_Validation);
			sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit", item.CRP_ValidationEdit);
			sqlCommand.Parameters.AddWithValue("DISPO_Dashboard", item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
			sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("ORD_Dashboard", item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
			sqlCommand.Parameters.AddWithValue("ORD_Order", item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
			sqlCommand.Parameters.AddWithValue("ORD_OrderAdd", item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
			sqlCommand.Parameters.AddWithValue("ORD_OrderDelete", item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
			sqlCommand.Parameters.AddWithValue("ORD_OrderEdit", item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
			sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO", item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
			sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate", item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
			sqlCommand.Parameters.AddWithValue("ORD_OrderValidate", item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
			sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder", item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
			sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder", item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
			sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
			sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
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
			sqlCommand.Parameters.AddWithValue("STAT_Dashboard", item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
			sqlCommand.Parameters.AddWithValue("we", item.WE == null ? (object)DBNull.Value : item.WE);
			sqlCommand.Parameters.AddWithValue("WE_Create", item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 45; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__MTM_AccessProfile] ([AccessProfileName],[Administration],[CreationTime],[CreationUserId],[CRP_AllRessourcesAuthorized],[CRP_Capacity],[CRP_CapacityEdit],[CRP_CapacityPlan],[CRP_CapacityPlanEdit],[CRP_Configuration],[CRP_ConfigurationEdit],[CRP_Holiday],[CRP_HolidayEdit],[CRP_RessourceAuthorizationEdit],[CRP_Validation],[CRP_ValidationEdit],[DISPO_Dashboard],[IsDefault],[ModuleActivated],[ORD_Dashboard],[ORD_Order],[ORD_OrderAdd],[ORD_OrderDelete],[ORD_OrderEdit],[ORD_OrderQuickPO],[ORD_OrderUnValidate],[ORD_OrderValidate],[ORD_ProjectPurchaseDeleteOrder],[ORD_ProjectPurchaseSetOrder],[Rahmen],[RahmenAdd],[RahmenAddPositions],[RahmenCancelation],[RahmenClosure],[RahmenDelete],[RahmenDeletePositions],[RahmenDocumentFlow],[RahmenEditHeader],[RahmenEditPositions],[RahmenHistory],[RahmenValdation],[STAT_Dashboard],[we],[WE_Create]) VALUES ( "

						+ "@AccessProfileName" + i + ","
						+ "@Administration" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CRP_AllRessourcesAuthorized" + i + ","
						+ "@CRP_Capacity" + i + ","
						+ "@CRP_CapacityEdit" + i + ","
						+ "@CRP_CapacityPlan" + i + ","
						+ "@CRP_CapacityPlanEdit" + i + ","
						+ "@CRP_Configuration" + i + ","
						+ "@CRP_ConfigurationEdit" + i + ","
						+ "@CRP_Holiday" + i + ","
						+ "@CRP_HolidayEdit" + i + ","
						+ "@CRP_RessourceAuthorizationEdit" + i + ","
						+ "@CRP_Validation" + i + ","
						+ "@CRP_ValidationEdit" + i + ","
						+ "@DISPO_Dashboard" + i + ","
						+ "@IsDefault" + i + ","
						+ "@ModuleActivated" + i + ","
						+ "@ORD_Dashboard" + i + ","
						+ "@ORD_Order" + i + ","
						+ "@ORD_OrderAdd" + i + ","
						+ "@ORD_OrderDelete" + i + ","
						+ "@ORD_OrderEdit" + i + ","
						+ "@ORD_OrderQuickPO" + i + ","
						+ "@ORD_OrderUnValidate" + i + ","
						+ "@ORD_OrderValidate" + i + ","
						+ "@ORD_ProjectPurchaseDeleteOrder" + i + ","
						+ "@ORD_ProjectPurchaseSetOrder" + i + ","
						+ "@Rahmen" + i + ","
						+ "@RahmenAdd" + i + ","
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
						+ "@STAT_Dashboard" + i + ","
						+ "@we" + i + ","
						+ "@WE_Create" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized" + i, item.CRP_AllRessourcesAuthorized);
					sqlCommand.Parameters.AddWithValue("CRP_Capacity" + i, item.CRP_Capacity);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit" + i, item.CRP_CapacityEdit);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan" + i, item.CRP_CapacityPlan);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit" + i, item.CRP_CapacityPlanEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Configuration" + i, item.CRP_Configuration);
					sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit" + i, item.CRP_ConfigurationEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Holiday" + i, item.CRP_Holiday);
					sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit" + i, item.CRP_HolidayEdit);
					sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit" + i, item.CRP_RessourceAuthorizationEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Validation" + i, item.CRP_Validation);
					sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit" + i, item.CRP_ValidationEdit);
					sqlCommand.Parameters.AddWithValue("DISPO_Dashboard" + i, item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
					sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ORD_Dashboard" + i, item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
					sqlCommand.Parameters.AddWithValue("ORD_Order" + i, item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
					sqlCommand.Parameters.AddWithValue("ORD_OrderAdd" + i, item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
					sqlCommand.Parameters.AddWithValue("ORD_OrderDelete" + i, item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
					sqlCommand.Parameters.AddWithValue("ORD_OrderEdit" + i, item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
					sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO" + i, item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
					sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate" + i, item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
					sqlCommand.Parameters.AddWithValue("ORD_OrderValidate" + i, item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
					sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder" + i, item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
					sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder" + i, item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("RahmenAdd" + i, item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
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
					sqlCommand.Parameters.AddWithValue("STAT_Dashboard" + i, item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
					sqlCommand.Parameters.AddWithValue("we" + i, item.WE == null ? (object)DBNull.Value : item.WE);
					sqlCommand.Parameters.AddWithValue("WE_Create" + i, item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__MTM_AccessProfile] SET [AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CRP_AllRessourcesAuthorized]=@CRP_AllRessourcesAuthorized, [CRP_Capacity]=@CRP_Capacity, [CRP_CapacityEdit]=@CRP_CapacityEdit, [CRP_CapacityPlan]=@CRP_CapacityPlan, [CRP_CapacityPlanEdit]=@CRP_CapacityPlanEdit, [CRP_Configuration]=@CRP_Configuration, [CRP_ConfigurationEdit]=@CRP_ConfigurationEdit, [CRP_Holiday]=@CRP_Holiday, [CRP_HolidayEdit]=@CRP_HolidayEdit, [CRP_RessourceAuthorizationEdit]=@CRP_RessourceAuthorizationEdit, [CRP_Validation]=@CRP_Validation, [CRP_ValidationEdit]=@CRP_ValidationEdit, [DISPO_Dashboard]=@DISPO_Dashboard, [IsDefault]=@IsDefault, [ModuleActivated]=@ModuleActivated, [ORD_Dashboard]=@ORD_Dashboard, [ORD_Order]=@ORD_Order, [ORD_OrderAdd]=@ORD_OrderAdd, [ORD_OrderDelete]=@ORD_OrderDelete, [ORD_OrderEdit]=@ORD_OrderEdit, [ORD_OrderQuickPO]=@ORD_OrderQuickPO, [ORD_OrderUnValidate]=@ORD_OrderUnValidate, [ORD_OrderValidate]=@ORD_OrderValidate, [ORD_ProjectPurchaseDeleteOrder]=@ORD_ProjectPurchaseDeleteOrder, [ORD_ProjectPurchaseSetOrder]=@ORD_ProjectPurchaseSetOrder, [Rahmen]=@Rahmen, [RahmenAdd]=@RahmenAdd, [RahmenAddPositions]=@RahmenAddPositions, [RahmenCancelation]=@RahmenCancelation, [RahmenClosure]=@RahmenClosure, [RahmenDelete]=@RahmenDelete, [RahmenDeletePositions]=@RahmenDeletePositions, [RahmenDocumentFlow]=@RahmenDocumentFlow, [RahmenEditHeader]=@RahmenEditHeader, [RahmenEditPositions]=@RahmenEditPositions, [RahmenHistory]=@RahmenHistory, [RahmenValdation]=@RahmenValdation, [STAT_Dashboard]=@STAT_Dashboard, [we]=@we, [WE_Create]=@WE_Create WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
			sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized", item.CRP_AllRessourcesAuthorized);
			sqlCommand.Parameters.AddWithValue("CRP_Capacity", item.CRP_Capacity);
			sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit", item.CRP_CapacityEdit);
			sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan", item.CRP_CapacityPlan);
			sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit", item.CRP_CapacityPlanEdit);
			sqlCommand.Parameters.AddWithValue("CRP_Configuration", item.CRP_Configuration);
			sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit", item.CRP_ConfigurationEdit);
			sqlCommand.Parameters.AddWithValue("CRP_Holiday", item.CRP_Holiday);
			sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit", item.CRP_HolidayEdit);
			sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit", item.CRP_RessourceAuthorizationEdit);
			sqlCommand.Parameters.AddWithValue("CRP_Validation", item.CRP_Validation);
			sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit", item.CRP_ValidationEdit);
			sqlCommand.Parameters.AddWithValue("DISPO_Dashboard", item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
			sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("ORD_Dashboard", item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
			sqlCommand.Parameters.AddWithValue("ORD_Order", item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
			sqlCommand.Parameters.AddWithValue("ORD_OrderAdd", item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
			sqlCommand.Parameters.AddWithValue("ORD_OrderDelete", item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
			sqlCommand.Parameters.AddWithValue("ORD_OrderEdit", item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
			sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO", item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
			sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate", item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
			sqlCommand.Parameters.AddWithValue("ORD_OrderValidate", item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
			sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder", item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
			sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder", item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
			sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
			sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
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
			sqlCommand.Parameters.AddWithValue("STAT_Dashboard", item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
			sqlCommand.Parameters.AddWithValue("we", item.WE == null ? (object)DBNull.Value : item.WE);
			sqlCommand.Parameters.AddWithValue("WE_Create", item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 45; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [__MTM_AccessProfile] SET "

					+ "[AccessProfileName]=@AccessProfileName" + i + ","
					+ "[Administration]=@Administration" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CRP_AllRessourcesAuthorized]=@CRP_AllRessourcesAuthorized" + i + ","
					+ "[CRP_Capacity]=@CRP_Capacity" + i + ","
					+ "[CRP_CapacityEdit]=@CRP_CapacityEdit" + i + ","
					+ "[CRP_CapacityPlan]=@CRP_CapacityPlan" + i + ","
					+ "[CRP_CapacityPlanEdit]=@CRP_CapacityPlanEdit" + i + ","
					+ "[CRP_Configuration]=@CRP_Configuration" + i + ","
					+ "[CRP_ConfigurationEdit]=@CRP_ConfigurationEdit" + i + ","
					+ "[CRP_Holiday]=@CRP_Holiday" + i + ","
					+ "[CRP_HolidayEdit]=@CRP_HolidayEdit" + i + ","
					+ "[CRP_RessourceAuthorizationEdit]=@CRP_RessourceAuthorizationEdit" + i + ","
					+ "[CRP_Validation]=@CRP_Validation" + i + ","
					+ "[CRP_ValidationEdit]=@CRP_ValidationEdit" + i + ","
					+ "[DISPO_Dashboard]=@DISPO_Dashboard" + i + ","
					+ "[IsDefault]=@IsDefault" + i + ","
					+ "[ModuleActivated]=@ModuleActivated" + i + ","
					+ "[ORD_Dashboard]=@ORD_Dashboard" + i + ","
					+ "[ORD_Order]=@ORD_Order" + i + ","
					+ "[ORD_OrderAdd]=@ORD_OrderAdd" + i + ","
					+ "[ORD_OrderDelete]=@ORD_OrderDelete" + i + ","
					+ "[ORD_OrderEdit]=@ORD_OrderEdit" + i + ","
					+ "[ORD_OrderQuickPO]=@ORD_OrderQuickPO" + i + ","
					+ "[ORD_OrderUnValidate]=@ORD_OrderUnValidate" + i + ","
					+ "[ORD_OrderValidate]=@ORD_OrderValidate" + i + ","
					+ "[ORD_ProjectPurchaseDeleteOrder]=@ORD_ProjectPurchaseDeleteOrder" + i + ","
					+ "[ORD_ProjectPurchaseSetOrder]=@ORD_ProjectPurchaseSetOrder" + i + ","
					+ "[Rahmen]=@Rahmen" + i + ","
					+ "[RahmenAdd]=@RahmenAdd" + i + ","
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
					+ "[STAT_Dashboard]=@STAT_Dashboard" + i + ","
					+ "[we]=@we" + i + ","
					+ "[WE_Create]=@WE_Create" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("Administration" + i, item.Administration);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized" + i, item.CRP_AllRessourcesAuthorized);
					sqlCommand.Parameters.AddWithValue("CRP_Capacity" + i, item.CRP_Capacity);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit" + i, item.CRP_CapacityEdit);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan" + i, item.CRP_CapacityPlan);
					sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit" + i, item.CRP_CapacityPlanEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Configuration" + i, item.CRP_Configuration);
					sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit" + i, item.CRP_ConfigurationEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Holiday" + i, item.CRP_Holiday);
					sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit" + i, item.CRP_HolidayEdit);
					sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit" + i, item.CRP_RessourceAuthorizationEdit);
					sqlCommand.Parameters.AddWithValue("CRP_Validation" + i, item.CRP_Validation);
					sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit" + i, item.CRP_ValidationEdit);
					sqlCommand.Parameters.AddWithValue("DISPO_Dashboard" + i, item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
					sqlCommand.Parameters.AddWithValue("IsDefault" + i, item.IsDefault);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("ORD_Dashboard" + i, item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
					sqlCommand.Parameters.AddWithValue("ORD_Order" + i, item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
					sqlCommand.Parameters.AddWithValue("ORD_OrderAdd" + i, item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
					sqlCommand.Parameters.AddWithValue("ORD_OrderDelete" + i, item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
					sqlCommand.Parameters.AddWithValue("ORD_OrderEdit" + i, item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
					sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO" + i, item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
					sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate" + i, item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
					sqlCommand.Parameters.AddWithValue("ORD_OrderValidate" + i, item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
					sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder" + i, item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
					sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder" + i, item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("RahmenAdd" + i, item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
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
					sqlCommand.Parameters.AddWithValue("STAT_Dashboard" + i, item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
					sqlCommand.Parameters.AddWithValue("we" + i, item.WE == null ? (object)DBNull.Value : item.WE);
					sqlCommand.Parameters.AddWithValue("WE_Create" + i, item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__MTM_AccessProfile] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__MTM_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity GetByName(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_AccessProfile] WHERE [AccessProfileName]=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateName(Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_AccessProfile] SET [AccessProfileName]=@AccessProfileName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int UpdateWoCreationData(Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = @"UPDATE [__MTM_AccessProfile] SET  [DISPO_Dashboard]=@DISPO_Dashboard ,[STAT_Dashboard]=@STAT_Dashboard,[ORD_OrderQuickPO]=@ORD_OrderQuickPO,
[AccessProfileName]=@AccessProfileName, [Administration]=@Administration, [CRP_AllRessourcesAuthorized]=@CRP_AllRessourcesAuthorized,
[CRP_Capacity]=@CRP_Capacity, [CRP_CapacityEdit]=@CRP_CapacityEdit, [CRP_CapacityPlan]=@CRP_CapacityPlan, [CRP_CapacityPlanEdit]=@CRP_CapacityPlanEdit,
[CRP_Configuration]=@CRP_Configuration, [CRP_ConfigurationEdit]=@CRP_ConfigurationEdit, [CRP_Holiday]=@CRP_Holiday, [CRP_HolidayEdit]=@CRP_HolidayEdit,
[CRP_RessourceAuthorizationEdit]=@CRP_RessourceAuthorizationEdit, [CRP_Validation]=@CRP_Validation, [CRP_ValidationEdit]=@CRP_ValidationEdit, 
[ModuleActivated]=@ORD_Order, [ORD_Dashboard]=@ORD_Dashboard, [ORD_Order]=@ORD_Order, [ORD_OrderAdd]=@ORD_OrderAdd, [ORD_OrderDelete]=@ORD_OrderDelete,
[ORD_OrderEdit]=@ORD_OrderEdit, [ORD_OrderUnValidate]=@ORD_OrderUnValidate, [ORD_OrderValidate]=@ORD_OrderValidate,
[RahmenAdd]=@RahmenAdd,
[RahmenDelete]=@RahmenDelete,
[RahmenHistory]=@RahmenHistory,
[RahmenDocumentFlow]=@RahmenDocumentFlow,
[RahmenEditHeader]=@RahmenEditHeader,
[RahmenEditPositions]=@RahmenEditPositions,
[RahmenAddPositions]=@RahmenAddPositions,
[RahmenDeletePositions]=@RahmenDeletePositions,
[RahmenValdation]=@RahmenValdation,
[RahmenCancelation]=@RahmenCancelation,
[RahmenClosure]=@RahmenClosure,
[Rahmen]=@Rahmen,
[WE]=@WE, 
[WE_Create]=@WE_Create,
[ORD_ProjectPurchaseDeleteOrder]=@ORD_ProjectPurchaseDeleteOrder,
[ORD_ProjectPurchaseSetOrder]=@ORD_ProjectPurchaseSetOrder,
[IsDefault]=@IsDefault
WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("IsDefault", item.IsDefault);
				sqlCommand.Parameters.AddWithValue("Administration", item.Administration);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CRP_AllRessourcesAuthorized", item.CRP_AllRessourcesAuthorized);
				sqlCommand.Parameters.AddWithValue("CRP_Capacity", item.CRP_Capacity);
				sqlCommand.Parameters.AddWithValue("CRP_CapacityEdit", item.CRP_CapacityEdit);
				sqlCommand.Parameters.AddWithValue("CRP_CapacityPlan", item.CRP_CapacityPlan);
				sqlCommand.Parameters.AddWithValue("CRP_CapacityPlanEdit", item.CRP_CapacityPlanEdit);
				sqlCommand.Parameters.AddWithValue("CRP_Configuration", item.CRP_Configuration);
				sqlCommand.Parameters.AddWithValue("CRP_ConfigurationEdit", item.CRP_ConfigurationEdit);
				sqlCommand.Parameters.AddWithValue("CRP_Holiday", item.CRP_Holiday);
				sqlCommand.Parameters.AddWithValue("CRP_HolidayEdit", item.CRP_HolidayEdit);
				sqlCommand.Parameters.AddWithValue("CRP_RessourceAuthorizationEdit", item.CRP_RessourceAuthorizationEdit);
				sqlCommand.Parameters.AddWithValue("CRP_Validation", item.CRP_Validation);
				sqlCommand.Parameters.AddWithValue("CRP_ValidationEdit", item.CRP_ValidationEdit);
				sqlCommand.Parameters.AddWithValue("DISPO_Dashboard", item.DISPO_Dashboard == null ? (object)DBNull.Value : item.DISPO_Dashboard);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("ORD_Dashboard", item.ORD_Dashboard == null ? (object)DBNull.Value : item.ORD_Dashboard);
				sqlCommand.Parameters.AddWithValue("ORD_Order", item.ORD_Order == null ? (object)DBNull.Value : item.ORD_Order);
				sqlCommand.Parameters.AddWithValue("ORD_OrderAdd", item.ORD_OrderAdd == null ? (object)DBNull.Value : item.ORD_OrderAdd);
				sqlCommand.Parameters.AddWithValue("ORD_OrderDelete", item.ORD_OrderDelete == null ? (object)DBNull.Value : item.ORD_OrderDelete);
				sqlCommand.Parameters.AddWithValue("ORD_OrderEdit", item.ORD_OrderEdit == null ? (object)DBNull.Value : item.ORD_OrderEdit);
				sqlCommand.Parameters.AddWithValue("ORD_OrderQuickPO", item.ORD_OrderQuickPO == null ? (object)DBNull.Value : item.ORD_OrderQuickPO);
				sqlCommand.Parameters.AddWithValue("ORD_OrderUnValidate", item.ORD_OrderUnValidate == null ? (object)DBNull.Value : item.ORD_OrderUnValidate);
				sqlCommand.Parameters.AddWithValue("ORD_OrderValidate", item.ORD_OrderValidate == null ? (object)DBNull.Value : item.ORD_OrderValidate);
				sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseDeleteOrder", item.ORD_ProjectPurchaseDeleteOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseDeleteOrder);
				sqlCommand.Parameters.AddWithValue("ORD_ProjectPurchaseSetOrder", item.ORD_ProjectPurchaseSetOrder == null ? (object)DBNull.Value : item.ORD_ProjectPurchaseSetOrder);
				sqlCommand.Parameters.AddWithValue("STAT_Dashboard", item.STAT_Dashboard == null ? (object)DBNull.Value : item.STAT_Dashboard);
				sqlCommand.Parameters.AddWithValue("WE", item.WE == null ? (object)DBNull.Value : item.WE);
				sqlCommand.Parameters.AddWithValue("WE_Create", item.WE_Create == null ? (object)DBNull.Value : item.WE_Create);

				sqlCommand.Parameters.AddWithValue("RahmenAdd", item.RahmenAdd == null ? (object)DBNull.Value : item.RahmenAdd);
				sqlCommand.Parameters.AddWithValue("RahmenDelete", item.RahmenDelete == null ? (object)DBNull.Value : item.RahmenDelete);
				sqlCommand.Parameters.AddWithValue("RahmenHistory", item.RahmenHistory == null ? (object)DBNull.Value : item.RahmenHistory);
				sqlCommand.Parameters.AddWithValue("RahmenDocumentFlow", item.RahmenDocumentFlow == null ? (object)DBNull.Value : item.RahmenDocumentFlow);
				sqlCommand.Parameters.AddWithValue("RahmenEditHeader", item.RahmenEditHeader == null ? (object)DBNull.Value : item.RahmenEditHeader);
				sqlCommand.Parameters.AddWithValue("RahmenEditPositions", item.RahmenEditPositions == null ? (object)DBNull.Value : item.RahmenEditPositions);
				sqlCommand.Parameters.AddWithValue("RahmenAddPositions", item.RahmenAddPositions == null ? (object)DBNull.Value : item.RahmenAddPositions);
				sqlCommand.Parameters.AddWithValue("RahmenDeletePositions", item.RahmenDeletePositions == null ? (object)DBNull.Value : item.RahmenDeletePositions);
				sqlCommand.Parameters.AddWithValue("RahmenValdation", item.RahmenValdation == null ? (object)DBNull.Value : item.RahmenValdation);
				sqlCommand.Parameters.AddWithValue("RahmenCancelation", item.RahmenCancelation == null ? (object)DBNull.Value : item.RahmenCancelation);
				sqlCommand.Parameters.AddWithValue("RahmenClosure", item.RahmenClosure == null ? (object)DBNull.Value : item.RahmenClosure);
				sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> GetDefaultProfiles(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__MTM_AccessProfile] WHERE [isDefault]=1", connection, transaction))
			{
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> GetProfilesWithValidation()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_AccessProfile] WHERE RahmenValdation = 1";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
			}
		}
		#endregion Custom Methods

	}
}
