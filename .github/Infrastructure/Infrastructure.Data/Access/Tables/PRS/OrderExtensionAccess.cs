using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class OrderExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_OrderExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_OrderExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [EDI_OrderExtension] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [EDI_OrderExtension] ([LastUpdateTime],[LastUpdateUserId],[LastUpdateUsername],[OrderId],[RecipientId],[SenderDuns],[SenderId],[ValidationTime],[ValidationUserId],[Version])  VALUES (@LastUpdateTime,@LastUpdateUserId,@LastUpdateUsername,@OrderId,@RecipientId,@SenderDuns,@SenderId,@ValidationTime,@ValidationUserId,@Version);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("RecipientId", item.RecipientId == null ? (object)DBNull.Value : item.RecipientId);
					sqlCommand.Parameters.AddWithValue("SenderDuns", item.SenderDuns == null ? (object)DBNull.Value : item.SenderDuns);
					sqlCommand.Parameters.AddWithValue("SenderId", item.SenderId == null ? (object)DBNull.Value : item.SenderId);
					sqlCommand.Parameters.AddWithValue("ValidationTime", item.EdiValidationTime);
					sqlCommand.Parameters.AddWithValue("ValidationUserId", item.EdiValidationUserId);
					sqlCommand.Parameters.AddWithValue("Version", item.Version);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity> items)
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
						query += " INSERT INTO [EDI_OrderExtension] ([LastUpdateTime],[LastUpdateUserId],[LastUpdateUsername],[OrderId],[RecipientId],[SenderDuns],[SenderId],[ValidationTime],[ValidationUserId],[Version]) VALUES ( "

							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@LastUpdateUsername" + i + ","
							+ "@OrderId" + i + ","
							+ "@RecipientId" + i + ","
							+ "@SenderDuns" + i + ","
							+ "@SenderId" + i + ","
							+ "@ValidationTime" + i + ","
							+ "@ValidationUserId" + i + ","
							+ "@Version" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("RecipientId" + i, item.RecipientId == null ? (object)DBNull.Value : item.RecipientId);
						sqlCommand.Parameters.AddWithValue("SenderDuns" + i, item.SenderDuns == null ? (object)DBNull.Value : item.SenderDuns);
						sqlCommand.Parameters.AddWithValue("SenderId" + i, item.SenderId == null ? (object)DBNull.Value : item.SenderId);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.EdiValidationTime);
						sqlCommand.Parameters.AddWithValue("ValidationUserId" + i, item.EdiValidationUserId);
						sqlCommand.Parameters.AddWithValue("Version" + i, item.Version);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [EDI_OrderExtension] SET [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [LastUpdateUsername]=@LastUpdateUsername, [OrderId]=@OrderId, [RecipientId]=@RecipientId, [SenderDuns]=@SenderDuns, [SenderId]=@SenderId, [ValidationTime]=@ValidationTime, [ValidationUserId]=@ValidationUserId, [Version]=@Version WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("RecipientId", item.RecipientId == null ? (object)DBNull.Value : item.RecipientId);
				sqlCommand.Parameters.AddWithValue("SenderDuns", item.SenderDuns == null ? (object)DBNull.Value : item.SenderDuns);
				sqlCommand.Parameters.AddWithValue("SenderId", item.SenderId == null ? (object)DBNull.Value : item.SenderId);
				sqlCommand.Parameters.AddWithValue("ValidationTime", item.EdiValidationTime);
				sqlCommand.Parameters.AddWithValue("ValidationUserId", item.EdiValidationUserId);
				sqlCommand.Parameters.AddWithValue("Version", item.Version);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity> items)
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
						query += " UPDATE [EDI_OrderExtension] SET "

							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
							+ "[LastUpdateUsername]=@LastUpdateUsername" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[RecipientId]=@RecipientId" + i + ","
							+ "[SenderDuns]=@SenderDuns" + i + ","
							+ "[SenderId]=@SenderId" + i + ","
							+ "[ValidationTime]=@ValidationTime" + i + ","
							+ "[ValidationUserId]=@ValidationUserId" + i + ","
							+ "[Version]=@Version" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("RecipientId" + i, item.RecipientId == null ? (object)DBNull.Value : item.RecipientId);
						sqlCommand.Parameters.AddWithValue("SenderDuns" + i, item.SenderDuns == null ? (object)DBNull.Value : item.SenderDuns);
						sqlCommand.Parameters.AddWithValue("SenderId" + i, item.SenderId == null ? (object)DBNull.Value : item.SenderId);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.EdiValidationTime);
						sqlCommand.Parameters.AddWithValue("ValidationUserId" + i, item.EdiValidationUserId);
						sqlCommand.Parameters.AddWithValue("Version" + i, item.Version);
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
				string query = "DELETE FROM [EDI_OrderExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [EDI_OrderExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static Entities.Tables.PRS.OrderExtensionEntity GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [EDI_OrderExtension] WHERE [OrderId]=@orderId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.OrderExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.OrderExtensionEntity GetByOrderId(int orderId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [EDI_OrderExtension] WHERE [OrderId]=@orderId";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("orderId", orderId);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.OrderExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.OrderExtensionEntity> GetByOrdersIds(List<int> ordersIds)
		{
			if(ordersIds != null && ordersIds.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.PRS.OrderExtensionEntity>();
				if(ordersIds.Count <= maxQueryNumber)
				{
					response = getOrdersIds(ordersIds);
				}
				else
				{
					int batchNumber = ordersIds.Count / maxQueryNumber;
					response = new List<Entities.Tables.PRS.OrderExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getOrdersIds(ordersIds.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(getOrdersIds(ordersIds.GetRange(batchNumber * maxQueryNumber, ordersIds.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.PRS.OrderExtensionEntity>();
		}
		private static List<Entities.Tables.PRS.OrderExtensionEntity> getOrdersIds(List<int> ordersIds)
		{
			if(ordersIds != null && ordersIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ordersIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ordersIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [EDI_OrderExtension] WHERE [OrderId] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.PRS.OrderExtensionEntity>();
				}
			}
			return new List<Entities.Tables.PRS.OrderExtensionEntity>();
		}

		#endregion

		#region Query with transactions
		public static int InsertWithTRansaction(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;

			string query = "INSERT INTO [EDI_OrderExtension] ([LastUpdateTime],[LastUpdateUserId],[LastUpdateUsername],[OrderId],[RecipientId],[SenderDuns],[SenderId],[ValidationTime],[ValidationUserId],[Version])  VALUES (@LastUpdateTime,@LastUpdateUserId,@LastUpdateUsername,@OrderId,@RecipientId,@SenderDuns,@SenderId,@ValidationTime,@ValidationUserId,@Version);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("RecipientId", item.RecipientId == null ? (object)DBNull.Value : item.RecipientId);
			sqlCommand.Parameters.AddWithValue("SenderDuns", item.SenderDuns == null ? (object)DBNull.Value : item.SenderDuns);
			sqlCommand.Parameters.AddWithValue("SenderId", item.SenderId == null ? (object)DBNull.Value : item.SenderId);
			sqlCommand.Parameters.AddWithValue("ValidationTime", item.EdiValidationTime);
			sqlCommand.Parameters.AddWithValue("ValidationUserId", item.EdiValidationUserId);
			sqlCommand.Parameters.AddWithValue("Version", item.Version);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			return response;
		}

		public static int UpdateWithTRansaction(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			string query = "UPDATE [EDI_OrderExtension] SET [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [LastUpdateUsername]=@LastUpdateUsername, [OrderId]=@OrderId, [RecipientId]=@RecipientId, [SenderDuns]=@SenderDuns, [SenderId]=@SenderId, [ValidationTime]=@ValidationTime, [ValidationUserId]=@ValidationUserId, [Version]=@Version WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("RecipientId", item.RecipientId == null ? (object)DBNull.Value : item.RecipientId);
			sqlCommand.Parameters.AddWithValue("SenderDuns", item.SenderDuns == null ? (object)DBNull.Value : item.SenderDuns);
			sqlCommand.Parameters.AddWithValue("SenderId", item.SenderId == null ? (object)DBNull.Value : item.SenderId);
			sqlCommand.Parameters.AddWithValue("ValidationTime", item.EdiValidationTime);
			sqlCommand.Parameters.AddWithValue("ValidationUserId", item.EdiValidationUserId);
			sqlCommand.Parameters.AddWithValue("Version", item.Version);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			// }

			return results;
		}

		public static Entities.Tables.PRS.OrderExtensionEntity GetByOrderIdWithTRansaction(int orderId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			//{
			//    sqlConnection.Open();

			string query = "SELECT * FROM [EDI_OrderExtension] WHERE [OrderId]=@orderId";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("orderId", orderId);

			DbExecution.Fill(sqlCommand, dataTable);
			//}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.OrderExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion
	}
}
