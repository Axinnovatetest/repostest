using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.CTS.SendAbEmailConfirmLogs
{
	public class SendAbEmailConfirmationLogsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [SendAbEmailConfirmationLogs] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [SendAbEmailConfirmationLogs]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [SendAbEmailConfirmationLogs] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [SendAbEmailConfirmationLogs] ([LastUserFullName],[LogDescription],[LogObject],[LogObjectId],[SendGridId],[UserId],[Username]) OUTPUT INSERTED.[Id] VALUES (@LastUserFullName,@LogDescription,@LogObject,@LogObjectId,@SendGridId,@UserId,@Username); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("LastUserFullName", item.LastUserFullName == null ? (object)DBNull.Value : item.LastUserFullName);
					sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
					sqlCommand.Parameters.AddWithValue("SendGridId", item.SendGridId == null ? (object)DBNull.Value : item.SendGridId);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [SendAbEmailConfirmationLogs] ([LastUserFullName],[LogDescription],[LogObject],[LogObjectId],[SendGridId],[UserId],[Username]) VALUES ( "

							+ "@LastUserFullName" + i + ","
							+ "@LogDescription" + i + ","
							+ "@LogObject" + i + ","
							+ "@LogObjectId" + i + ","
							+ "@SendGridId" + i + ","
							+ "@UserId" + i + ","
							+ "@Username" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("LastUserFullName" + i, item.LastUserFullName == null ? (object)DBNull.Value : item.LastUserFullName);
						sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
						sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
						sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
						sqlCommand.Parameters.AddWithValue("SendGridId" + i, item.SendGridId == null ? (object)DBNull.Value : item.SendGridId);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [SendAbEmailConfirmationLogs] SET [LastUserFullName]=@LastUserFullName, [LogDescription]=@LogDescription, [LogObject]=@LogObject, [LogObjectId]=@LogObjectId, [SendGridId]=@SendGridId, [UserId]=@UserId, [Username]=@Username WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("LastUserFullName", item.LastUserFullName == null ? (object)DBNull.Value : item.LastUserFullName);
				sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
				sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
				sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
				sqlCommand.Parameters.AddWithValue("SendGridId", item.SendGridId == null ? (object)DBNull.Value : item.SendGridId);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
				sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [SendAbEmailConfirmationLogs] SET "

							+ "[LastUserFullName]=@LastUserFullName" + i + ","
							+ "[LogDescription]=@LogDescription" + i + ","
							+ "[LogObject]=@LogObject" + i + ","
							+ "[LogObjectId]=@LogObjectId" + i + ","
							+ "[SendGridId]=@SendGridId" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[Username]=@Username" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("LastUserFullName" + i, item.LastUserFullName == null ? (object)DBNull.Value : item.LastUserFullName);
						sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
						sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
						sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
						sqlCommand.Parameters.AddWithValue("SendGridId" + i, item.SendGridId == null ? (object)DBNull.Value : item.SendGridId);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
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
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [SendAbEmailConfirmationLogs] WHERE [Id]=@Id";
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
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [SendAbEmailConfirmationLogs] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [SendAbEmailConfirmationLogs] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [SendAbEmailConfirmationLogs]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [SendAbEmailConfirmationLogs] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [SendAbEmailConfirmationLogs] ([LastUserFullName],[LogDescription],[LogObject],[LogObjectId],[SendGridId],[UserId],[Username]) OUTPUT INSERTED.[Id] VALUES (@LastUserFullName,@LogDescription,@LogObject,@LogObjectId,@SendGridId,@UserId,@Username); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("LastUserFullName", item.LastUserFullName == null ? (object)DBNull.Value : item.LastUserFullName);
			sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
			sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
			sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
			sqlCommand.Parameters.AddWithValue("SendGridId", item.SendGridId == null ? (object)DBNull.Value : item.SendGridId);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [SendAbEmailConfirmationLogs] ([LastUserFullName],[LogDescription],[LogObject],[LogObjectId],[SendGridId],[UserId],[Username]) VALUES ( "

						+ "@LastUserFullName" + i + ","
						+ "@LogDescription" + i + ","
						+ "@LogObject" + i + ","
						+ "@LogObjectId" + i + ","
						+ "@SendGridId" + i + ","
						+ "@UserId" + i + ","
						+ "@Username" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("LastUserFullName" + i, item.LastUserFullName == null ? (object)DBNull.Value : item.LastUserFullName);
					sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
					sqlCommand.Parameters.AddWithValue("SendGridId" + i, item.SendGridId == null ? (object)DBNull.Value : item.SendGridId);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [SendAbEmailConfirmationLogs] SET [LastUserFullName]=@LastUserFullName, [LogDescription]=@LogDescription, [LogObject]=@LogObject, [LogObjectId]=@LogObjectId, [SendGridId]=@SendGridId, [UserId]=@UserId, [Username]=@Username WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("LastUserFullName", item.LastUserFullName == null ? (object)DBNull.Value : item.LastUserFullName);
			sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
			sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
			sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
			sqlCommand.Parameters.AddWithValue("SendGridId", item.SendGridId == null ? (object)DBNull.Value : item.SendGridId);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.SendAbEmailConfirmationLogsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [SendAbEmailConfirmationLogs] SET "

					+ "[LastUserFullName]=@LastUserFullName" + i + ","
					+ "[LogDescription]=@LogDescription" + i + ","
					+ "[LogObject]=@LogObject" + i + ","
					+ "[LogObjectId]=@LogObjectId" + i + ","
					+ "[SendGridId]=@SendGridId" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[Username]=@Username" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("LastUserFullName" + i, item.LastUserFullName == null ? (object)DBNull.Value : item.LastUserFullName);
					sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
					sqlCommand.Parameters.AddWithValue("SendGridId" + i, item.SendGridId == null ? (object)DBNull.Value : item.SendGridId);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [SendAbEmailConfirmationLogs] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
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

				string query = "DELETE FROM [SendAbEmailConfirmationLogs] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		#endregion Custom Methods

	}
}
