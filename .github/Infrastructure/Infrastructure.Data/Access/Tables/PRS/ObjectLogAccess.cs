using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class ObjectLogAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ObjectLog] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ObjectLog] ORDER BY [LastUpdateTime] DESC";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__PRS_ObjectLog] WHERE [Id] IN (" + queryIds + ")  ORDER BY LastUpdateTime DESC";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__PRS_ObjectLog] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId]) OUTPUT INSERTED.[Id] VALUES (@LastUpdateTime,@LastUpdateUserFullName,@LastUpdateUserId,@LastUpdateUsername,@LogDescription,@LogObject,@LogObjectId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
					sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> items)
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
						query += " INSERT INTO [__PRS_ObjectLog] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId]) VALUES ( "

							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserFullName" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@LastUpdateUsername" + i + ","
							+ "@LogDescription" + i + ","
							+ "@LogObject" + i + ","
							+ "@LogObjectId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName" + i, item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
						sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
						sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
						sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_ObjectLog] SET [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserFullName]=@LastUpdateUserFullName, [LastUpdateUserId]=@LastUpdateUserId, [LastUpdateUsername]=@LastUpdateUsername, [LogDescription]=@LogDescription, [LogObject]=@LogObject, [LogObjectId]=@LogObjectId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
				sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
				sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
				sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> items)
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
						query += " UPDATE [__PRS_ObjectLog] SET "

							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUserFullName]=@LastUpdateUserFullName" + i + ","
							+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
							+ "[LastUpdateUsername]=@LastUpdateUsername" + i + ","
							+ "[LogDescription]=@LogDescription" + i + ","
							+ "[LogObject]=@LogObject" + i + ","
							+ "[LogObjectId]=@LogObjectId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName" + i, item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
						sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
						sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
						sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId);
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
				string query = "DELETE FROM [__PRS_ObjectLog] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__PRS_ObjectLog] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__PRS_ObjectLog] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__PRS_ObjectLog]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__PRS_ObjectLog] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO [__PRS_ObjectLog] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId]) OUTPUT INSERTED.[Id] VALUES (@LastUpdateTime,@LastUpdateUserFullName,@LastUpdateUserId,@LastUpdateUsername,@LogDescription,@LogObject,@LogObjectId); ";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
			sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
			sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
			sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__PRS_ObjectLog] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId]) VALUES ( "

						+ "@LastUpdateTime" + i + ","
						+ "@LastUpdateUserFullName" + i + ","
						+ "@LastUpdateUserId" + i + ","
						+ "@LastUpdateUsername" + i + ","
						+ "@LogDescription" + i + ","
						+ "@LogObject" + i + ","
						+ "@LogObjectId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName" + i, item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
					sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__PRS_ObjectLog] SET [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserFullName]=@LastUpdateUserFullName, [LastUpdateUserId]=@LastUpdateUserId, [LastUpdateUsername]=@LastUpdateUsername, [LogDescription]=@LogDescription, [LogObject]=@LogObject, [LogObjectId]=@LogObjectId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
			sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
			sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
			sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__PRS_ObjectLog] SET "

					+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
					+ "[LastUpdateUserFullName]=@LastUpdateUserFullName" + i + ","
					+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
					+ "[LastUpdateUsername]=@LastUpdateUsername" + i + ","
					+ "[LogDescription]=@LogDescription" + i + ","
					+ "[LogObject]=@LogObject" + i + ","
					+ "[LogObjectId]=@LogObjectId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName" + i, item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
					sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__PRS_ObjectLog] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__PRS_ObjectLog] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion


		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> GetByObjetAndId(string objectName, int id, int max = -1)
		{
			max = (max > 1000 || max <= 0) ? 1000 : max;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP {max} * FROM [__PRS_ObjectLog] WHERE [LogObject]=@name AND [LogObjectId]=@id ORDER BY Id DESC ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", objectName);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> GetByObjetsAndId(List<string> objectNames, int max = -1)
		{
			max = (max > 1000 || max <= 0) ? 1000 : max;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP {max} * FROM [__PRS_ObjectLog] WHERE [LogObject] IN ('{string.Join("','", objectNames)}') ORDER BY LastUpdateTime DESC ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> GetByObjetsAndId_2(List<string> objectNames, int Id, int max = -1)
		{
			max = (max > 1000 || max <= 0) ? 1000 : max;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP {max} * FROM [__PRS_ObjectLog] WHERE [LogObject] IN ('{string.Join("','", objectNames)}') and LogObjectId=@LogObjectId ORDER BY LastUpdateTime DESC ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("LogObjectId", Id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> GetByObjectsNames(List<string> objectNames, int max = -1)
		{
			max = (max > 1000 || max <= 0) ? 1000 : max;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP {max} * FROM [__PRS_ObjectLog] WHERE [LogObject] IN ('{string.Join("','", objectNames)}') ORDER BY LastUpdateTime DESC ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			}
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
