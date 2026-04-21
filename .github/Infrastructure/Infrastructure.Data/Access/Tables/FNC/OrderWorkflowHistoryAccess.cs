using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class OrderWorkflowHistoryAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderWorkflowHistory] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderWorkflowHistory]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_OrderWorkflowHistory] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_OrderWorkflowHistory] ([OrderId],[OrderIssuerUserEmail],[OrderIssuerUserId],[OrderIssuerUserName],[OrderNumber],[WorkflowActionComments],[WorkflowActionId],[WorkflowActionName],[WorkflowActionTime],[WorkflowActionUserEmail],[WorkflowActionUserId],[WorkflowActionUserName])  VALUES (@OrderId,@OrderIssuerUserEmail,@OrderIssuerUserId,@OrderIssuerUserName,@OrderNumber,@WorkflowActionComments,@WorkflowActionId,@WorkflowActionName,@WorkflowActionTime,@WorkflowActionUserEmail,@WorkflowActionUserId,@WorkflowActionUserName); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserEmail", item.OrderIssuerUserEmail == null ? (object)DBNull.Value : item.OrderIssuerUserEmail);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserId", item.OrderIssuerUserId == null ? (object)DBNull.Value : item.OrderIssuerUserId);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserName", item.OrderIssuerUserName == null ? (object)DBNull.Value : item.OrderIssuerUserName);
					sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("WorkflowActionComments", item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
					sqlCommand.Parameters.AddWithValue("WorkflowActionId", item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
					sqlCommand.Parameters.AddWithValue("WorkflowActionName", item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
					sqlCommand.Parameters.AddWithValue("WorkflowActionTime", item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail", item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserId", item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserName", item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 13; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__FNC_OrderWorkflowHistory] ([OrderId],[OrderIssuerUserEmail],[OrderIssuerUserId],[OrderIssuerUserName],[OrderNumber],[WorkflowActionComments],[WorkflowActionId],[WorkflowActionName],[WorkflowActionTime],[WorkflowActionUserEmail],[WorkflowActionUserId],[WorkflowActionUserName]) VALUES ( "

							+ "@OrderId" + i + ","
							+ "@OrderIssuerUserEmail" + i + ","
							+ "@OrderIssuerUserId" + i + ","
							+ "@OrderIssuerUserName" + i + ","
							+ "@OrderNumber" + i + ","
							+ "@WorkflowActionComments" + i + ","
							+ "@WorkflowActionId" + i + ","
							+ "@WorkflowActionName" + i + ","
							+ "@WorkflowActionTime" + i + ","
							+ "@WorkflowActionUserEmail" + i + ","
							+ "@WorkflowActionUserId" + i + ","
							+ "@WorkflowActionUserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderIssuerUserEmail" + i, item.OrderIssuerUserEmail == null ? (object)DBNull.Value : item.OrderIssuerUserEmail);
						sqlCommand.Parameters.AddWithValue("OrderIssuerUserId" + i, item.OrderIssuerUserId == null ? (object)DBNull.Value : item.OrderIssuerUserId);
						sqlCommand.Parameters.AddWithValue("OrderIssuerUserName" + i, item.OrderIssuerUserName == null ? (object)DBNull.Value : item.OrderIssuerUserName);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("WorkflowActionComments" + i, item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
						sqlCommand.Parameters.AddWithValue("WorkflowActionId" + i, item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
						sqlCommand.Parameters.AddWithValue("WorkflowActionName" + i, item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
						sqlCommand.Parameters.AddWithValue("WorkflowActionTime" + i, item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail" + i, item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserId" + i, item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserName" + i, item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_OrderWorkflowHistory] SET [OrderId]=@OrderId, [OrderIssuerUserEmail]=@OrderIssuerUserEmail, [OrderIssuerUserId]=@OrderIssuerUserId, [OrderIssuerUserName]=@OrderIssuerUserName, [OrderNumber]=@OrderNumber, [WorkflowActionComments]=@WorkflowActionComments, [WorkflowActionId]=@WorkflowActionId, [WorkflowActionName]=@WorkflowActionName, [WorkflowActionTime]=@WorkflowActionTime, [WorkflowActionUserEmail]=@WorkflowActionUserEmail, [WorkflowActionUserId]=@WorkflowActionUserId, [WorkflowActionUserName]=@WorkflowActionUserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderIssuerUserEmail", item.OrderIssuerUserEmail == null ? (object)DBNull.Value : item.OrderIssuerUserEmail);
				sqlCommand.Parameters.AddWithValue("OrderIssuerUserId", item.OrderIssuerUserId == null ? (object)DBNull.Value : item.OrderIssuerUserId);
				sqlCommand.Parameters.AddWithValue("OrderIssuerUserName", item.OrderIssuerUserName == null ? (object)DBNull.Value : item.OrderIssuerUserName);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("WorkflowActionComments", item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
				sqlCommand.Parameters.AddWithValue("WorkflowActionId", item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
				sqlCommand.Parameters.AddWithValue("WorkflowActionName", item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
				sqlCommand.Parameters.AddWithValue("WorkflowActionTime", item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
				sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail", item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
				sqlCommand.Parameters.AddWithValue("WorkflowActionUserId", item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
				sqlCommand.Parameters.AddWithValue("WorkflowActionUserName", item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 13; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__FNC_OrderWorkflowHistory] SET "

							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderIssuerUserEmail]=@OrderIssuerUserEmail" + i + ","
							+ "[OrderIssuerUserId]=@OrderIssuerUserId" + i + ","
							+ "[OrderIssuerUserName]=@OrderIssuerUserName" + i + ","
							+ "[OrderNumber]=@OrderNumber" + i + ","
							+ "[WorkflowActionComments]=@WorkflowActionComments" + i + ","
							+ "[WorkflowActionId]=@WorkflowActionId" + i + ","
							+ "[WorkflowActionName]=@WorkflowActionName" + i + ","
							+ "[WorkflowActionTime]=@WorkflowActionTime" + i + ","
							+ "[WorkflowActionUserEmail]=@WorkflowActionUserEmail" + i + ","
							+ "[WorkflowActionUserId]=@WorkflowActionUserId" + i + ","
							+ "[WorkflowActionUserName]=@WorkflowActionUserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderIssuerUserEmail" + i, item.OrderIssuerUserEmail == null ? (object)DBNull.Value : item.OrderIssuerUserEmail);
						sqlCommand.Parameters.AddWithValue("OrderIssuerUserId" + i, item.OrderIssuerUserId == null ? (object)DBNull.Value : item.OrderIssuerUserId);
						sqlCommand.Parameters.AddWithValue("OrderIssuerUserName" + i, item.OrderIssuerUserName == null ? (object)DBNull.Value : item.OrderIssuerUserName);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("WorkflowActionComments" + i, item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
						sqlCommand.Parameters.AddWithValue("WorkflowActionId" + i, item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
						sqlCommand.Parameters.AddWithValue("WorkflowActionName" + i, item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
						sqlCommand.Parameters.AddWithValue("WorkflowActionTime" + i, item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail" + i, item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserId" + i, item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserName" + i, item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_OrderWorkflowHistory] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [__FNC_OrderWorkflowHistory] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_OrderWorkflowHistory] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_OrderWorkflowHistory]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__FNC_OrderWorkflowHistory] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__FNC_OrderWorkflowHistory] ([OrderId],[OrderIssuerUserEmail],[OrderIssuerUserId],[OrderIssuerUserName],[OrderNumber],[WorkflowActionComments],[WorkflowActionId],[WorkflowActionName],[WorkflowActionTime],[WorkflowActionUserEmail],[WorkflowActionUserId],[WorkflowActionUserName]) OUTPUT INSERTED.[Id] VALUES (@OrderId,@OrderIssuerUserEmail,@OrderIssuerUserId,@OrderIssuerUserName,@OrderNumber,@WorkflowActionComments,@WorkflowActionId,@WorkflowActionName,@WorkflowActionTime,@WorkflowActionUserEmail,@WorkflowActionUserId,@WorkflowActionUserName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderIssuerUserEmail", item.OrderIssuerUserEmail == null ? (object)DBNull.Value : item.OrderIssuerUserEmail);
			sqlCommand.Parameters.AddWithValue("OrderIssuerUserId", item.OrderIssuerUserId == null ? (object)DBNull.Value : item.OrderIssuerUserId);
			sqlCommand.Parameters.AddWithValue("OrderIssuerUserName", item.OrderIssuerUserName == null ? (object)DBNull.Value : item.OrderIssuerUserName);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("WorkflowActionComments", item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
			sqlCommand.Parameters.AddWithValue("WorkflowActionId", item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
			sqlCommand.Parameters.AddWithValue("WorkflowActionName", item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
			sqlCommand.Parameters.AddWithValue("WorkflowActionTime", item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
			sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail", item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
			sqlCommand.Parameters.AddWithValue("WorkflowActionUserId", item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
			sqlCommand.Parameters.AddWithValue("WorkflowActionUserName", item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 13; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__FNC_OrderWorkflowHistory] ([OrderId],[OrderIssuerUserEmail],[OrderIssuerUserId],[OrderIssuerUserName],[OrderNumber],[WorkflowActionComments],[WorkflowActionId],[WorkflowActionName],[WorkflowActionTime],[WorkflowActionUserEmail],[WorkflowActionUserId],[WorkflowActionUserName]) VALUES ( "

						+ "@OrderId" + i + ","
						+ "@OrderIssuerUserEmail" + i + ","
						+ "@OrderIssuerUserId" + i + ","
						+ "@OrderIssuerUserName" + i + ","
						+ "@OrderNumber" + i + ","
						+ "@WorkflowActionComments" + i + ","
						+ "@WorkflowActionId" + i + ","
						+ "@WorkflowActionName" + i + ","
						+ "@WorkflowActionTime" + i + ","
						+ "@WorkflowActionUserEmail" + i + ","
						+ "@WorkflowActionUserId" + i + ","
						+ "@WorkflowActionUserName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserEmail" + i, item.OrderIssuerUserEmail == null ? (object)DBNull.Value : item.OrderIssuerUserEmail);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserId" + i, item.OrderIssuerUserId == null ? (object)DBNull.Value : item.OrderIssuerUserId);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserName" + i, item.OrderIssuerUserName == null ? (object)DBNull.Value : item.OrderIssuerUserName);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("WorkflowActionComments" + i, item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
					sqlCommand.Parameters.AddWithValue("WorkflowActionId" + i, item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
					sqlCommand.Parameters.AddWithValue("WorkflowActionName" + i, item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
					sqlCommand.Parameters.AddWithValue("WorkflowActionTime" + i, item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail" + i, item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserId" + i, item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserName" + i, item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__FNC_OrderWorkflowHistory] SET [OrderId]=@OrderId, [OrderIssuerUserEmail]=@OrderIssuerUserEmail, [OrderIssuerUserId]=@OrderIssuerUserId, [OrderIssuerUserName]=@OrderIssuerUserName, [OrderNumber]=@OrderNumber, [WorkflowActionComments]=@WorkflowActionComments, [WorkflowActionId]=@WorkflowActionId, [WorkflowActionName]=@WorkflowActionName, [WorkflowActionTime]=@WorkflowActionTime, [WorkflowActionUserEmail]=@WorkflowActionUserEmail, [WorkflowActionUserId]=@WorkflowActionUserId, [WorkflowActionUserName]=@WorkflowActionUserName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderIssuerUserEmail", item.OrderIssuerUserEmail == null ? (object)DBNull.Value : item.OrderIssuerUserEmail);
			sqlCommand.Parameters.AddWithValue("OrderIssuerUserId", item.OrderIssuerUserId == null ? (object)DBNull.Value : item.OrderIssuerUserId);
			sqlCommand.Parameters.AddWithValue("OrderIssuerUserName", item.OrderIssuerUserName == null ? (object)DBNull.Value : item.OrderIssuerUserName);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("WorkflowActionComments", item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
			sqlCommand.Parameters.AddWithValue("WorkflowActionId", item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
			sqlCommand.Parameters.AddWithValue("WorkflowActionName", item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
			sqlCommand.Parameters.AddWithValue("WorkflowActionTime", item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
			sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail", item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
			sqlCommand.Parameters.AddWithValue("WorkflowActionUserId", item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
			sqlCommand.Parameters.AddWithValue("WorkflowActionUserName", item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 13; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__FNC_OrderWorkflowHistory] SET "

					+ "[OrderId]=@OrderId" + i + ","
					+ "[OrderIssuerUserEmail]=@OrderIssuerUserEmail" + i + ","
					+ "[OrderIssuerUserId]=@OrderIssuerUserId" + i + ","
					+ "[OrderIssuerUserName]=@OrderIssuerUserName" + i + ","
					+ "[OrderNumber]=@OrderNumber" + i + ","
					+ "[WorkflowActionComments]=@WorkflowActionComments" + i + ","
					+ "[WorkflowActionId]=@WorkflowActionId" + i + ","
					+ "[WorkflowActionName]=@WorkflowActionName" + i + ","
					+ "[WorkflowActionTime]=@WorkflowActionTime" + i + ","
					+ "[WorkflowActionUserEmail]=@WorkflowActionUserEmail" + i + ","
					+ "[WorkflowActionUserId]=@WorkflowActionUserId" + i + ","
					+ "[WorkflowActionUserName]=@WorkflowActionUserName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserEmail" + i, item.OrderIssuerUserEmail == null ? (object)DBNull.Value : item.OrderIssuerUserEmail);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserId" + i, item.OrderIssuerUserId == null ? (object)DBNull.Value : item.OrderIssuerUserId);
					sqlCommand.Parameters.AddWithValue("OrderIssuerUserName" + i, item.OrderIssuerUserName == null ? (object)DBNull.Value : item.OrderIssuerUserName);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("WorkflowActionComments" + i, item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
					sqlCommand.Parameters.AddWithValue("WorkflowActionId" + i, item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
					sqlCommand.Parameters.AddWithValue("WorkflowActionName" + i, item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
					sqlCommand.Parameters.AddWithValue("WorkflowActionTime" + i, item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail" + i, item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserId" + i, item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserName" + i, item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__FNC_OrderWorkflowHistory] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__FNC_OrderWorkflowHistory] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity> GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderWorkflowHistory] WHERE OrderId=@orderId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity>();
			}
		}
		#endregion
	}
}
