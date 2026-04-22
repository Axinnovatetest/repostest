using Infrastructure.Data.Entities.Tables.Support.Feedback;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.Support.Feedback
{
	public class ERP_LogsAccess
	{
		#region Default Methods
		public static ERP_LogsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ERP_Logs] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new ERP_LogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<ERP_LogsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ERP_Logs]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_LogsEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_LogsEntity>();
			}
		}
		public static List<ERP_LogsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<ERP_LogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<ERP_LogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<ERP_LogsEntity>();
		}
		private static List<ERP_LogsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [ERP_Logs] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_LogsEntity(x)).ToList();
				}
				else
				{
					return new List<ERP_LogsEntity>();
				}
			}
			return new List<ERP_LogsEntity>();
		}

		public static int Insert(ERP_LogsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [ERP_Logs] ([EndpointMethod],[EndpointName],[LogCaptureDate],[LogLevel],[LogMessage],[Module],[Treated],[UserId],[UserName]) OUTPUT INSERTED.[Id] VALUES (@EndpointMethod,@EndpointName,@LogCaptureDate,@LogLevel,@LogMessage,@Module,@Treated,@UserId,@UserName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("EndpointMethod", item.EndpointMethod == null ? (object)DBNull.Value : item.EndpointMethod);
					sqlCommand.Parameters.AddWithValue("EndpointName", item.EndpointName == null ? (object)DBNull.Value : item.EndpointName);
					sqlCommand.Parameters.AddWithValue("LogCaptureDate", item.LogCaptureDate == null ? (object)DBNull.Value : item.LogCaptureDate);
					sqlCommand.Parameters.AddWithValue("LogLevel", item.LogLevel == null ? (object)DBNull.Value : item.LogLevel);
					sqlCommand.Parameters.AddWithValue("LogMessage", item.LogMessage == null ? (object)DBNull.Value : item.LogMessage);
					sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
					sqlCommand.Parameters.AddWithValue("Treated", item.Treated == null ? (object)DBNull.Value : item.Treated);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<ERP_LogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insert(List<ERP_LogsEntity> items)
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
						query += " INSERT INTO [ERP_Logs] ([EndpointMethod],[EndpointName],[LogCaptureDate],[LogLevel],[LogMessage],[Module],[Treated],[UserId],[UserName]) VALUES ( "

							+ "@EndpointMethod" + i + ","
							+ "@EndpointName" + i + ","
							+ "@LogCaptureDate" + i + ","
							+ "@LogLevel" + i + ","
							+ "@LogMessage" + i + ","
							+ "@Module" + i + ","
							+ "@Treated" + i + ","
							+ "@UserId" + i + ","
							+ "@UserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("EndpointMethod" + i, item.EndpointMethod == null ? (object)DBNull.Value : item.EndpointMethod);
						sqlCommand.Parameters.AddWithValue("EndpointName" + i, item.EndpointName == null ? (object)DBNull.Value : item.EndpointName);
						sqlCommand.Parameters.AddWithValue("LogCaptureDate" + i, item.LogCaptureDate == null ? (object)DBNull.Value : item.LogCaptureDate);
						sqlCommand.Parameters.AddWithValue("LogLevel" + i, item.LogLevel == null ? (object)DBNull.Value : item.LogLevel);
						sqlCommand.Parameters.AddWithValue("LogMessage" + i, item.LogMessage == null ? (object)DBNull.Value : item.LogMessage);
						sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
						sqlCommand.Parameters.AddWithValue("Treated" + i, item.Treated == null ? (object)DBNull.Value : item.Treated);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(ERP_LogsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [ERP_Logs] SET [EndpointMethod]=@EndpointMethod, [EndpointName]=@EndpointName, [LogCaptureDate]=@LogCaptureDate, [LogLevel]=@LogLevel, [LogMessage]=@LogMessage, [Module]=@Module, [Treated]=@Treated, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("EndpointMethod", item.EndpointMethod == null ? (object)DBNull.Value : item.EndpointMethod);
				sqlCommand.Parameters.AddWithValue("EndpointName", item.EndpointName == null ? (object)DBNull.Value : item.EndpointName);
				sqlCommand.Parameters.AddWithValue("LogCaptureDate", item.LogCaptureDate == null ? (object)DBNull.Value : item.LogCaptureDate);
				sqlCommand.Parameters.AddWithValue("LogLevel", item.LogLevel == null ? (object)DBNull.Value : item.LogLevel);
				sqlCommand.Parameters.AddWithValue("LogMessage", item.LogMessage == null ? (object)DBNull.Value : item.LogMessage);
				sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
				sqlCommand.Parameters.AddWithValue("Treated", item.Treated == null ? (object)DBNull.Value : item.Treated);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<ERP_LogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int update(List<ERP_LogsEntity> items)
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
						query += " UPDATE [ERP_Logs] SET "

							+ "[EndpointMethod]=@EndpointMethod" + i + ","
							+ "[EndpointName]=@EndpointName" + i + ","
							+ "[LogCaptureDate]=@LogCaptureDate" + i + ","
							+ "[LogLevel]=@LogLevel" + i + ","
							+ "[LogMessage]=@LogMessage" + i + ","
							+ "[Module]=@Module" + i + ","
							+ "[Treated]=@Treated" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("EndpointMethod" + i, item.EndpointMethod == null ? (object)DBNull.Value : item.EndpointMethod);
						sqlCommand.Parameters.AddWithValue("EndpointName" + i, item.EndpointName == null ? (object)DBNull.Value : item.EndpointName);
						sqlCommand.Parameters.AddWithValue("LogCaptureDate" + i, item.LogCaptureDate == null ? (object)DBNull.Value : item.LogCaptureDate);
						sqlCommand.Parameters.AddWithValue("LogLevel" + i, item.LogLevel == null ? (object)DBNull.Value : item.LogLevel);
						sqlCommand.Parameters.AddWithValue("LogMessage" + i, item.LogMessage == null ? (object)DBNull.Value : item.LogMessage);
						sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
						sqlCommand.Parameters.AddWithValue("Treated" + i, item.Treated == null ? (object)DBNull.Value : item.Treated);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
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
				string query = "DELETE FROM [ERP_Logs] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [ERP_Logs] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static ERP_LogsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ERP_Logs] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new ERP_LogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<ERP_LogsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ERP_Logs]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_LogsEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_LogsEntity>();
			}
		}
		public static List<ERP_LogsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<ERP_LogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<ERP_LogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<ERP_LogsEntity>();
		}
		private static List<ERP_LogsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [ERP_Logs] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_LogsEntity(x)).ToList();
				}
				else
				{
					return new List<ERP_LogsEntity>();
				}
			}
			return new List<ERP_LogsEntity>();
		}

		public static int InsertWithTransaction(ERP_LogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [ERP_Logs] ([EndpointMethod],[EndpointName],[LogCaptureDate],[LogLevel],[LogMessage],[Module],[Treated],[UserId],[UserName]) OUTPUT INSERTED.[Id] VALUES (@EndpointMethod,@EndpointName,@LogCaptureDate,@LogLevel,@LogMessage,@Module,@Treated,@UserId,@UserName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("EndpointMethod", item.EndpointMethod == null ? (object)DBNull.Value : item.EndpointMethod);
			sqlCommand.Parameters.AddWithValue("EndpointName", item.EndpointName == null ? (object)DBNull.Value : item.EndpointName);
			sqlCommand.Parameters.AddWithValue("LogCaptureDate", item.LogCaptureDate == null ? (object)DBNull.Value : item.LogCaptureDate);
			sqlCommand.Parameters.AddWithValue("LogLevel", item.LogLevel == null ? (object)DBNull.Value : item.LogLevel);
			sqlCommand.Parameters.AddWithValue("LogMessage", item.LogMessage == null ? (object)DBNull.Value : item.LogMessage);
			sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
			sqlCommand.Parameters.AddWithValue("Treated", item.Treated == null ? (object)DBNull.Value : item.Treated);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<ERP_LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insertWithTransaction(List<ERP_LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [ERP_Logs] ([EndpointMethod],[EndpointName],[LogCaptureDate],[LogLevel],[LogMessage],[Module],[Treated],[UserId],[UserName]) VALUES ( "

						+ "@EndpointMethod" + i + ","
						+ "@EndpointName" + i + ","
						+ "@LogCaptureDate" + i + ","
						+ "@LogLevel" + i + ","
						+ "@LogMessage" + i + ","
						+ "@Module" + i + ","
						+ "@Treated" + i + ","
						+ "@UserId" + i + ","
						+ "@UserName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("EndpointMethod" + i, item.EndpointMethod == null ? (object)DBNull.Value : item.EndpointMethod);
					sqlCommand.Parameters.AddWithValue("EndpointName" + i, item.EndpointName == null ? (object)DBNull.Value : item.EndpointName);
					sqlCommand.Parameters.AddWithValue("LogCaptureDate" + i, item.LogCaptureDate == null ? (object)DBNull.Value : item.LogCaptureDate);
					sqlCommand.Parameters.AddWithValue("LogLevel" + i, item.LogLevel == null ? (object)DBNull.Value : item.LogLevel);
					sqlCommand.Parameters.AddWithValue("LogMessage" + i, item.LogMessage == null ? (object)DBNull.Value : item.LogMessage);
					sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
					sqlCommand.Parameters.AddWithValue("Treated" + i, item.Treated == null ? (object)DBNull.Value : item.Treated);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(ERP_LogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [ERP_Logs] SET [EndpointMethod]=@EndpointMethod, [EndpointName]=@EndpointName, [LogCaptureDate]=@LogCaptureDate, [LogLevel]=@LogLevel, [LogMessage]=@LogMessage, [Module]=@Module, [Treated]=@Treated, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("EndpointMethod", item.EndpointMethod == null ? (object)DBNull.Value : item.EndpointMethod);
			sqlCommand.Parameters.AddWithValue("EndpointName", item.EndpointName == null ? (object)DBNull.Value : item.EndpointName);
			sqlCommand.Parameters.AddWithValue("LogCaptureDate", item.LogCaptureDate == null ? (object)DBNull.Value : item.LogCaptureDate);
			sqlCommand.Parameters.AddWithValue("LogLevel", item.LogLevel == null ? (object)DBNull.Value : item.LogLevel);
			sqlCommand.Parameters.AddWithValue("LogMessage", item.LogMessage == null ? (object)DBNull.Value : item.LogMessage);
			sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
			sqlCommand.Parameters.AddWithValue("Treated", item.Treated == null ? (object)DBNull.Value : item.Treated);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<ERP_LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int updateWithTransaction(List<ERP_LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [ERP_Logs] SET "

					+ "[EndpointMethod]=@EndpointMethod" + i + ","
					+ "[EndpointName]=@EndpointName" + i + ","
					+ "[LogCaptureDate]=@LogCaptureDate" + i + ","
					+ "[LogLevel]=@LogLevel" + i + ","
					+ "[LogMessage]=@LogMessage" + i + ","
					+ "[Module]=@Module" + i + ","
					+ "[Treated]=@Treated" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("EndpointMethod" + i, item.EndpointMethod == null ? (object)DBNull.Value : item.EndpointMethod);
					sqlCommand.Parameters.AddWithValue("EndpointName" + i, item.EndpointName == null ? (object)DBNull.Value : item.EndpointName);
					sqlCommand.Parameters.AddWithValue("LogCaptureDate" + i, item.LogCaptureDate == null ? (object)DBNull.Value : item.LogCaptureDate);
					sqlCommand.Parameters.AddWithValue("LogLevel" + i, item.LogLevel == null ? (object)DBNull.Value : item.LogLevel);
					sqlCommand.Parameters.AddWithValue("LogMessage" + i, item.LogMessage == null ? (object)DBNull.Value : item.LogMessage);
					sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
					sqlCommand.Parameters.AddWithValue("Treated" + i, item.Treated == null ? (object)DBNull.Value : item.Treated);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [ERP_Logs] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			int results = 0;

			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
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
			return results;
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

				string query = "DELETE FROM [ERP_Logs] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Entities.Tables.Support.Feedback.ERP_LogsEntity> GetFeedbacksLogs(string level, string searchValue, DateTime? searchDate, string sortFieldName, bool sortDesc, bool treated, int currentPage = 0, int? pageSize = 0)
		{
			if(string.IsNullOrWhiteSpace(sortFieldName))
				sortFieldName = "LogCaptureDate";

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [ERP_Logs] ";
				var clauses = new List<string>();

				if(searchDate.HasValue)
				{
					clauses.Add($" Convert(date,[LogCaptureDate])=Convert(date,'{searchDate.Value.ToString("yyyyMMdd")}')");
				}

				if(!string.IsNullOrWhiteSpace(searchValue))
				{
					clauses.Add($" [LogMessage] LIKE '%{searchValue.SqlEscape()}%' OR [EndpointName] LIKE '{searchValue.SqlEscape()}%' OR [UserName] LIKE '%{searchValue.SqlEscape()}%' OR [Module] LIKE '%{searchValue.SqlEscape()}%'");
				}
				if(!string.IsNullOrWhiteSpace(level))
				{
					clauses.Add($" [LogLevel]='{level}' ");
				}
				if(treated)
				{
					clauses.Add($" ISNULL([Treated],0)={(treated == true ? 1 : 0)}");
				}

				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}

				query += $" ORDER BY {sortFieldName} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.Support.Feedback.ERP_LogsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.Support.Feedback.ERP_LogsEntity>();
			}
		}
		public static int GetFeedbacksLogs_Count(string level, string searchValue, DateTime? searchDate, bool isTreated)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(*) As Nb FROM [ERP_Logs]";

				var clauses = new List<string>();

				if(searchDate.HasValue)
				{
					clauses.Add($" Convert(date,[LogCaptureDate])=Convert(date,'{searchDate.Value.ToString("yyyyMMdd")}')");
				}
				if(isTreated)
				{
					clauses.Add($" ISNULL([Treated],0)={(isTreated == true ? 1 : 0)}");
				}
				if(!string.IsNullOrWhiteSpace(searchValue))
				{
					clauses.Add($" [LogMessage] LIKE '%{searchValue.SqlEscape()}%' OR [EndpointName] LIKE '{searchValue.SqlEscape()}%' OR [UserName] LIKE '%{searchValue.SqlEscape()}%' OR [Module] LIKE '%{searchValue.SqlEscape()}%'");
				}
				if(!string.IsNullOrWhiteSpace(level))
				{
					clauses.Add($" [LogLevel]='{level}' ");
				}

				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}


		}

		public static int InsertWithBulkTransaction(
	List<Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_LogsEntity> items,
	SqlConnection connection,
	SqlTransaction transaction)
		{
			if(items == null || items.Count == 0)
				return -1;

			var dataTable = new DataTable("ERP_Logs");

			dataTable.Columns.Add("EndpointMethod", typeof(string));
			dataTable.Columns.Add("EndpointName", typeof(string));
			dataTable.Columns.Add("LogCaptureDate", typeof(DateTime));
			dataTable.Columns.Add("LogLevel", typeof(string));
			dataTable.Columns.Add("LogMessage", typeof(string));
			dataTable.Columns.Add("Module", typeof(string));
			dataTable.Columns.Add("UserId", typeof(int));
			dataTable.Columns.Add("UserName", typeof(string));

			foreach(var item in items)
			{
				dataTable.Rows.Add(
					item.EndpointMethod ?? (object)DBNull.Value,
					item.EndpointName ?? (object)DBNull.Value,
					item.LogCaptureDate ?? (object)DBNull.Value,
					item.LogLevel ?? (object)DBNull.Value,
					item.LogMessage ?? (object)DBNull.Value,
					item.Module ?? (object)DBNull.Value,
					item.UserId ?? (object)DBNull.Value,
					item.UserName ?? (object)DBNull.Value
				);
			}

			using(var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
			{
				bulkCopy.DestinationTableName = "ERP_Logs";

				bulkCopy.ColumnMappings.Add("EndpointMethod", "EndpointMethod");
				bulkCopy.ColumnMappings.Add("EndpointName", "EndpointName");
				bulkCopy.ColumnMappings.Add("LogCaptureDate", "LogCaptureDate");
				bulkCopy.ColumnMappings.Add("LogLevel", "LogLevel");
				bulkCopy.ColumnMappings.Add("LogMessage", "LogMessage");
				bulkCopy.ColumnMappings.Add("Module", "Module");
				bulkCopy.ColumnMappings.Add("UserId", "UserId");
				bulkCopy.ColumnMappings.Add("UserName", "UserName");

				bulkCopy.WriteToServer(dataTable);
			}

			return items.Count;
		}
		public static int InsertBulk(
		   List<Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_LogsEntity> items,
		   SqlConnection connection,
		   SqlTransaction transaction)
		{
			if(items == null || items.Count == 0)
				return -1;

			int BatchSize = 50000; // Process in batches for efficiency
			int SqlBulkTimeout = 180; // Set timeout in seconds
			int totalInserted = 0;

			// Process large data in batches
			foreach(var batch in GetBatches(items, BatchSize))
			{
				var dataTable = ConvertToDataTable(batch);

				using(var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
				{
					bulkCopy.DestinationTableName = "ERP_Logs";
					bulkCopy.BatchSize = BatchSize;
					bulkCopy.BulkCopyTimeout = SqlBulkTimeout;
					bulkCopy.EnableStreaming = true;

					bulkCopy.ColumnMappings.Add("EndpointMethod", "EndpointMethod");
					bulkCopy.ColumnMappings.Add("EndpointName", "EndpointName");
					bulkCopy.ColumnMappings.Add("LogCaptureDate", "LogCaptureDate");
					bulkCopy.ColumnMappings.Add("LogLevel", "LogLevel");
					bulkCopy.ColumnMappings.Add("LogMessage", "LogMessage");
					bulkCopy.ColumnMappings.Add("Module", "Module");
					bulkCopy.ColumnMappings.Add("UserId", "UserId");
					bulkCopy.ColumnMappings.Add("UserName", "UserName");

					bulkCopy.WriteToServer(dataTable);
				}

				totalInserted += batch.Count;
			}

			return totalInserted;
		}

		private static DataTable ConvertToDataTable(List<Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_LogsEntity> items)
		{
			var dataTable = new DataTable("ERP_Logs")
			{
				// Preallocate space for improved performance
				CaseSensitive = false
			};

			dataTable.Columns.Add("EndpointMethod", typeof(string));
			dataTable.Columns.Add("EndpointName", typeof(string));
			dataTable.Columns.Add("LogCaptureDate", typeof(DateTime));
			dataTable.Columns.Add("LogLevel", typeof(string));
			dataTable.Columns.Add("LogMessage", typeof(string));
			dataTable.Columns.Add("Module", typeof(string));
			dataTable.Columns.Add("UserId", typeof(int));
			dataTable.Columns.Add("UserName", typeof(string));

			foreach(var item in items)
			{
				var row = dataTable.NewRow();
				row.SetField("EndpointMethod", item.EndpointMethod ?? (object)DBNull.Value);
				row.SetField("EndpointName", item.EndpointName ?? (object)DBNull.Value);
				row.SetField("LogCaptureDate", item.LogCaptureDate ?? (object)DBNull.Value);
				row.SetField("LogLevel", item.LogLevel ?? (object)DBNull.Value);
				row.SetField("LogMessage", item.LogMessage ?? (object)DBNull.Value);
				row.SetField("Module", item.Module ?? (object)DBNull.Value);
				row.SetField("UserId", item.UserId);
				row.SetField("UserName", item.UserName ?? (object)DBNull.Value);
				dataTable.Rows.Add(row);
			}

			return dataTable;
		}

		private static IEnumerable<List<T>> GetBatches<T>(List<T> source, int batchSize)
		{
			for(int i = 0; i < source.Count; i += batchSize)
			{
				yield return source.Skip(i).Take(batchSize).ToList();
			}
		}

		public static List<ERP_LogsEntity> GetAllByLogCaptureDate(List<DateTime> dates)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ERP_Logs] ";

				if(dates.Count > 0)
				{
					string datesString = string.Join(",", dates.Select(date => $"'{date.ToString("yyyyMMdd")}'"));
					query += $" WHERE Convert(date,[LogCaptureDate]) IN ({datesString})";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_LogsEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_LogsEntity>();
			}
		}
		public static int DeleteAllByLogCaptureDate(List<DateTime> dates, SqlConnection connection, SqlTransaction transaction)
		{
			if(dates?.Count<=0)
			{
				return 0;
			}

			string datesString = string.Join(",", dates.Select(date => $"'{date.ToString("yyyyMMdd")}'"));
			using(var sqlCommand = new SqlCommand($"DELETE FROM [ERP_Logs] WHERE Convert(date,[LogCaptureDate]) IN ({datesString})", connection, transaction))
			{
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var x) ? x : 0;
			}
		}

		public static List<ERP_LogsEntity> GetLastLogDate(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT top 1* FROM [ERP_Logs] order by LogCaptureDate desc";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_LogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Support.Feedback.ERP_LogsEntity>();
			}
		}


		public static int UpdateLogTreatedById(int logId, int treated)
		{
			int result = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [ERP_Logs] SET [Treated]=@Treated WHERE [Id]=@LogId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("LogId", logId);
				sqlCommand.Parameters.AddWithValue("Treated", treated);
				result = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return result;
		}

		//public static int DeleteWithBulkTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		//{
		//	if(ids == null || ids.Count == 0)
		//		return -1;

		//	var dataTable = new DataTable();
		//	dataTable.Columns.Add("Id", typeof(int));

		//	foreach(var id in ids)
		//	{
		//		dataTable.Rows.Add(id);
		//	}

		//	string tempTableQuery = "CREATE TABLE #TempIds (Id INT)";
		//	using(var createTableCmd = new SqlCommand(tempTableQuery, connection, transaction))
		//	{
		//		DbExecution.ExecuteNonQuery(createTableCmd);
		//	}

		//	using(var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
		//	{
		//		bulkCopy.DestinationTableName = "#TempIds";
		//		bulkCopy.ColumnMappings.Add("Id", "Id");
		//		bulkCopy.WriteToServer(dataTable);
		//	}

		//	string deleteQuery = "DELETE FROM [ERP_Logs] WHERE Id IN (SELECT Id FROM #TempIds)";
		//	using(var deleteCmd = new SqlCommand(deleteQuery, connection, transaction))
		//	{
		//		int results = DbExecution.ExecuteNonQuery(deleteCmd);

		//		string dropTableQuery = "DROP TABLE #TempIds";
		//		using(var dropCmd = new SqlCommand(dropTableQuery, connection, transaction))
		//		{
		//			DbExecution.ExecuteNonQuery(dropCmd);
		//		}

		//		return results;
		//	}
		//}
		public static int DeleteWithBulkTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids == null || ids.Count == 0)
				return -1;

			int BatchSize = 10000;
			int totalDeleted = 0;

			// Process large data in chunks to avoid memory and performance issues
			foreach(var batch in GetBatches(ids, BatchSize))
			{
				var dataTable = ListToDataTable(batch);

				using(var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
				{
					bulkCopy.DestinationTableName = "dbo.TempIdsTable"; // Assume a pre-created permanent staging table
					bulkCopy.ColumnMappings.Add("Id", "Id");
					bulkCopy.WriteToServer(dataTable);
				}

				// Use a DELETE JOIN to remove records efficiently
				using(var deleteCmd = new SqlCommand("DELETE ERP_Logs FROM ERP_Logs INNER JOIN dbo.TempIdsTable T ON ERP_Logs.Id = T.Id", connection, transaction))
				{
					totalDeleted += DbExecution.ExecuteNonQuery(deleteCmd);
				}

				// Truncate instead of DROP to retain schema and improve performance
				using(var truncateCmd = new SqlCommand("TRUNCATE TABLE dbo.TempIdsTable", connection, transaction))
				{
					DbExecution.ExecuteNonQuery(truncateCmd);
				}
			}

			return totalDeleted;
		}

		private static DataTable ListToDataTable(IEnumerable<int> ids)
		{
			var dataTable = new DataTable();
			dataTable.Columns.Add("Id", typeof(int));

			foreach(var id in ids)
				dataTable.Rows.Add(id);

			return dataTable;
		}

		private static IEnumerable<List<int>> GetBatches(List<int> source, int batchSize)
		{
			for(int i = 0; i < source.Count; i += batchSize)
			{
				yield return source.Skip(i).Take(batchSize).ToList();
			}
		}

		#endregion Custom Methods

	}
}

