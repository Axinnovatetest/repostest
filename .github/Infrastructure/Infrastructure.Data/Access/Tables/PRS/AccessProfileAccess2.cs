using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class AccessProfileAccess2
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2 Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AccessProfile2] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_AccessProfile2]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [EDI_AccessProfile2] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2 item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [EDI_AccessProfile2] ([Access],[AccessProfileName],[AccessUpdate],[AllCustomers],[CreationTime],[CreationUserId],[Customer],[CustomerUpdate],[EDI],[ModuleActivated],[Order],[OrderError],[OrderErrorHistory],[OrderErrorValidate],[OrderHistory],[OrderUpdate],[OrderValidate],[SuperAdministrator])  VALUES (@Access,@AccessProfileName,@AccessUpdate,@AllCustomers,@CreationTime,@CreationUserId,@Customer,@CustomerUpdate,@EDI,@ModuleActivated,@Order,@OrderError,@OrderErrorHistory,@OrderErrorValidate,@OrderHistory,@OrderUpdate,@OrderValidate,@SuperAdministrator); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Access", item.Access == null ? (object)DBNull.Value : item.Access);
					sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
					sqlCommand.Parameters.AddWithValue("AccessUpdate", item.AccessUpdate == null ? (object)DBNull.Value : item.AccessUpdate);
					sqlCommand.Parameters.AddWithValue("AllCustomers", item.AllCustomers == null ? (object)DBNull.Value : item.AllCustomers);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Customer", item.Customer == null ? (object)DBNull.Value : item.Customer);
					sqlCommand.Parameters.AddWithValue("CustomerUpdate", item.CustomerUpdate == null ? (object)DBNull.Value : item.CustomerUpdate);
					sqlCommand.Parameters.AddWithValue("EDI", item.EDI == null ? (object)DBNull.Value : item.EDI);
					sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("Order", item.Order == null ? (object)DBNull.Value : item.Order);
					sqlCommand.Parameters.AddWithValue("OrderError", item.OrderError == null ? (object)DBNull.Value : item.OrderError);
					sqlCommand.Parameters.AddWithValue("OrderErrorHistory", item.OrderErrorHistory == null ? (object)DBNull.Value : item.OrderErrorHistory);
					sqlCommand.Parameters.AddWithValue("OrderErrorValidate", item.OrderErrorValidate == null ? (object)DBNull.Value : item.OrderErrorValidate);
					sqlCommand.Parameters.AddWithValue("OrderHistory", item.OrderHistory == null ? (object)DBNull.Value : item.OrderHistory);
					sqlCommand.Parameters.AddWithValue("OrderUpdate", item.OrderUpdate == null ? (object)DBNull.Value : item.OrderUpdate);
					sqlCommand.Parameters.AddWithValue("OrderValidate", item.OrderValidate == null ? (object)DBNull.Value : item.OrderValidate);
					sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator == null ? (object)DBNull.Value : item.SuperAdministrator);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [EDI_AccessProfile2] ([Access],[AccessProfileName],[AccessUpdate],[AllCustomers],[CreationTime],[CreationUserId],[Customer],[CustomerUpdate],[EDI],[ModuleActivated],[Order],[OrderError],[OrderErrorHistory],[OrderErrorValidate],[OrderHistory],[OrderUpdate],[OrderValidate],[SuperAdministrator]) VALUES ( "

							+ "@Access" + i + ","
							+ "@AccessProfileName" + i + ","
							+ "@AccessUpdate" + i + ","
							+ "@AllCustomers" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@Customer" + i + ","
							+ "@CustomerUpdate" + i + ","
							+ "@EDI" + i + ","
							+ "@ModuleActivated" + i + ","
							+ "@Order" + i + ","
							+ "@OrderError" + i + ","
							+ "@OrderErrorHistory" + i + ","
							+ "@OrderErrorValidate" + i + ","
							+ "@OrderHistory" + i + ","
							+ "@OrderUpdate" + i + ","
							+ "@OrderValidate" + i + ","
							+ "@SuperAdministrator" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Access" + i, item.Access == null ? (object)DBNull.Value : item.Access);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("AccessUpdate" + i, item.AccessUpdate == null ? (object)DBNull.Value : item.AccessUpdate);
						sqlCommand.Parameters.AddWithValue("AllCustomers" + i, item.AllCustomers == null ? (object)DBNull.Value : item.AllCustomers);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Customer" + i, item.Customer == null ? (object)DBNull.Value : item.Customer);
						sqlCommand.Parameters.AddWithValue("CustomerUpdate" + i, item.CustomerUpdate == null ? (object)DBNull.Value : item.CustomerUpdate);
						sqlCommand.Parameters.AddWithValue("EDI" + i, item.EDI == null ? (object)DBNull.Value : item.EDI);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("Order" + i, item.Order == null ? (object)DBNull.Value : item.Order);
						sqlCommand.Parameters.AddWithValue("OrderError" + i, item.OrderError == null ? (object)DBNull.Value : item.OrderError);
						sqlCommand.Parameters.AddWithValue("OrderErrorHistory" + i, item.OrderErrorHistory == null ? (object)DBNull.Value : item.OrderErrorHistory);
						sqlCommand.Parameters.AddWithValue("OrderErrorValidate" + i, item.OrderErrorValidate == null ? (object)DBNull.Value : item.OrderErrorValidate);
						sqlCommand.Parameters.AddWithValue("OrderHistory" + i, item.OrderHistory == null ? (object)DBNull.Value : item.OrderHistory);
						sqlCommand.Parameters.AddWithValue("OrderUpdate" + i, item.OrderUpdate == null ? (object)DBNull.Value : item.OrderUpdate);
						sqlCommand.Parameters.AddWithValue("OrderValidate" + i, item.OrderValidate == null ? (object)DBNull.Value : item.OrderValidate);
						sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator == null ? (object)DBNull.Value : item.SuperAdministrator);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2 item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [EDI_AccessProfile2] SET [Access]=@Access, [AccessProfileName]=@AccessProfileName, [AccessUpdate]=@AccessUpdate, [AllCustomers]=@AllCustomers, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [Customer]=@Customer, [CustomerUpdate]=@CustomerUpdate, [EDI]=@EDI, [ModuleActivated]=@ModuleActivated, [Order]=@Order, [OrderError]=@OrderError, [OrderErrorHistory]=@OrderErrorHistory, [OrderErrorValidate]=@OrderErrorValidate, [OrderHistory]=@OrderHistory, [OrderUpdate]=@OrderUpdate, [OrderValidate]=@OrderValidate, [SuperAdministrator]=@SuperAdministrator WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Access", item.Access == null ? (object)DBNull.Value : item.Access);
				sqlCommand.Parameters.AddWithValue("AccessProfileName", item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
				sqlCommand.Parameters.AddWithValue("AccessUpdate", item.AccessUpdate == null ? (object)DBNull.Value : item.AccessUpdate);
				sqlCommand.Parameters.AddWithValue("AllCustomers", item.AllCustomers == null ? (object)DBNull.Value : item.AllCustomers);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Customer", item.Customer == null ? (object)DBNull.Value : item.Customer);
				sqlCommand.Parameters.AddWithValue("CustomerUpdate", item.CustomerUpdate == null ? (object)DBNull.Value : item.CustomerUpdate);
				sqlCommand.Parameters.AddWithValue("EDI", item.EDI == null ? (object)DBNull.Value : item.EDI);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("Order", item.Order == null ? (object)DBNull.Value : item.Order);
				sqlCommand.Parameters.AddWithValue("OrderError", item.OrderError == null ? (object)DBNull.Value : item.OrderError);
				sqlCommand.Parameters.AddWithValue("OrderErrorHistory", item.OrderErrorHistory == null ? (object)DBNull.Value : item.OrderErrorHistory);
				sqlCommand.Parameters.AddWithValue("OrderErrorValidate", item.OrderErrorValidate == null ? (object)DBNull.Value : item.OrderErrorValidate);
				sqlCommand.Parameters.AddWithValue("OrderHistory", item.OrderHistory == null ? (object)DBNull.Value : item.OrderHistory);
				sqlCommand.Parameters.AddWithValue("OrderUpdate", item.OrderUpdate == null ? (object)DBNull.Value : item.OrderUpdate);
				sqlCommand.Parameters.AddWithValue("OrderValidate", item.OrderValidate == null ? (object)DBNull.Value : item.OrderValidate);
				sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator == null ? (object)DBNull.Value : item.SuperAdministrator);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.AccessProfileEntity2> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [EDI_AccessProfile2] SET "

							+ "[Access]=@Access" + i + ","
							+ "[AccessProfileName]=@AccessProfileName" + i + ","
							+ "[AccessUpdate]=@AccessUpdate" + i + ","
							+ "[AllCustomers]=@AllCustomers" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[Customer]=@Customer" + i + ","
							+ "[CustomerUpdate]=@CustomerUpdate" + i + ","
							+ "[EDI]=@EDI" + i + ","
							+ "[ModuleActivated]=@ModuleActivated" + i + ","
							+ "[Order]=@Order" + i + ","
							+ "[OrderError]=@OrderError" + i + ","
							+ "[OrderErrorHistory]=@OrderErrorHistory" + i + ","
							+ "[OrderErrorValidate]=@OrderErrorValidate" + i + ","
							+ "[OrderHistory]=@OrderHistory" + i + ","
							+ "[OrderUpdate]=@OrderUpdate" + i + ","
							+ "[OrderValidate]=@OrderValidate" + i + ","
							+ "[SuperAdministrator]=@SuperAdministrator" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Access" + i, item.Access == null ? (object)DBNull.Value : item.Access);
						sqlCommand.Parameters.AddWithValue("AccessProfileName" + i, item.AccessProfileName == null ? (object)DBNull.Value : item.AccessProfileName);
						sqlCommand.Parameters.AddWithValue("AccessUpdate" + i, item.AccessUpdate == null ? (object)DBNull.Value : item.AccessUpdate);
						sqlCommand.Parameters.AddWithValue("AllCustomers" + i, item.AllCustomers == null ? (object)DBNull.Value : item.AllCustomers);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Customer" + i, item.Customer == null ? (object)DBNull.Value : item.Customer);
						sqlCommand.Parameters.AddWithValue("CustomerUpdate" + i, item.CustomerUpdate == null ? (object)DBNull.Value : item.CustomerUpdate);
						sqlCommand.Parameters.AddWithValue("EDI" + i, item.EDI == null ? (object)DBNull.Value : item.EDI);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated == null ? (object)DBNull.Value : item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("Order" + i, item.Order == null ? (object)DBNull.Value : item.Order);
						sqlCommand.Parameters.AddWithValue("OrderError" + i, item.OrderError == null ? (object)DBNull.Value : item.OrderError);
						sqlCommand.Parameters.AddWithValue("OrderErrorHistory" + i, item.OrderErrorHistory == null ? (object)DBNull.Value : item.OrderErrorHistory);
						sqlCommand.Parameters.AddWithValue("OrderErrorValidate" + i, item.OrderErrorValidate == null ? (object)DBNull.Value : item.OrderErrorValidate);
						sqlCommand.Parameters.AddWithValue("OrderHistory" + i, item.OrderHistory == null ? (object)DBNull.Value : item.OrderHistory);
						sqlCommand.Parameters.AddWithValue("OrderUpdate" + i, item.OrderUpdate == null ? (object)DBNull.Value : item.OrderUpdate);
						sqlCommand.Parameters.AddWithValue("OrderValidate" + i, item.OrderValidate == null ? (object)DBNull.Value : item.OrderValidate);
						sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator == null ? (object)DBNull.Value : item.SuperAdministrator);
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
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [EDI_AccessProfile2] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [EDI_AccessProfile2] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		#endregion
	}
}
