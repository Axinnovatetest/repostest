using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{

	public class ProcessedFileAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_ProcessedFile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_ProcessedFile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_ProcessedFile] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_DLF_ProcessedFile] ([FileName],[ProcessStatus],[ProcessStatusName],[ProcessStatusTrace],[ProcessTime]) OUTPUT INSERTED.[Id] VALUES (@FileName,@ProcessStatus,@ProcessStatusName,@ProcessStatusTrace,@ProcessTime); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("FileName", item.FileName);
					sqlCommand.Parameters.AddWithValue("ProcessStatus", item.ProcessStatus);
					sqlCommand.Parameters.AddWithValue("ProcessStatusName", item.ProcessStatusName);
					sqlCommand.Parameters.AddWithValue("ProcessStatusTrace", item.ProcessStatusTrace == null ? (object)DBNull.Value : item.ProcessStatusTrace);
					sqlCommand.Parameters.AddWithValue("ProcessTime", item.ProcessTime);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> items)
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
						query += " INSERT INTO [__EDI_DLF_ProcessedFile] ([FileName],[ProcessStatus],[ProcessStatusName],[ProcessStatusTrace],[ProcessTime]) VALUES ( "

							+ "@FileName" + i + ","
							+ "@ProcessStatus" + i + ","
							+ "@ProcessStatusName" + i + ","
							+ "@ProcessStatusTrace" + i + ","
							+ "@ProcessTime" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName);
						sqlCommand.Parameters.AddWithValue("ProcessStatus" + i, item.ProcessStatus);
						sqlCommand.Parameters.AddWithValue("ProcessStatusName" + i, item.ProcessStatusName);
						sqlCommand.Parameters.AddWithValue("ProcessStatusTrace" + i, item.ProcessStatusTrace == null ? (object)DBNull.Value : item.ProcessStatusTrace);
						sqlCommand.Parameters.AddWithValue("ProcessTime" + i, item.ProcessTime);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_ProcessedFile] SET [FileName]=@FileName, [ProcessStatus]=@ProcessStatus, [ProcessStatusName]=@ProcessStatusName, [ProcessStatusTrace]=@ProcessStatusTrace, [ProcessTime]=@ProcessTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("FileName", item.FileName);
				sqlCommand.Parameters.AddWithValue("ProcessStatus", item.ProcessStatus);
				sqlCommand.Parameters.AddWithValue("ProcessStatusName", item.ProcessStatusName);
				sqlCommand.Parameters.AddWithValue("ProcessStatusTrace", item.ProcessStatusTrace == null ? (object)DBNull.Value : item.ProcessStatusTrace);
				sqlCommand.Parameters.AddWithValue("ProcessTime", item.ProcessTime);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> items)
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
						query += " UPDATE [__EDI_DLF_ProcessedFile] SET "

							+ "[FileName]=@FileName" + i + ","
							+ "[ProcessStatus]=@ProcessStatus" + i + ","
							+ "[ProcessStatusName]=@ProcessStatusName" + i + ","
							+ "[ProcessStatusTrace]=@ProcessStatusTrace" + i + ","
							+ "[ProcessTime]=@ProcessTime" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName);
						sqlCommand.Parameters.AddWithValue("ProcessStatus" + i, item.ProcessStatus);
						sqlCommand.Parameters.AddWithValue("ProcessStatusName" + i, item.ProcessStatusName);
						sqlCommand.Parameters.AddWithValue("ProcessStatusTrace" + i, item.ProcessStatusTrace == null ? (object)DBNull.Value : item.ProcessStatusTrace);
						sqlCommand.Parameters.AddWithValue("ProcessTime" + i, item.ProcessTime);
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
				string query = "DELETE FROM [__EDI_DLF_ProcessedFile] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__EDI_DLF_ProcessedFile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_ProcessedFile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_ProcessedFile]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_ProcessedFile] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__EDI_DLF_ProcessedFile] ([FileName],[ProcessStatus],[ProcessStatusName],[ProcessStatusTrace],[ProcessTime]) OUTPUT INSERTED.[Id] VALUES (@FileName,@ProcessStatus,@ProcessStatusName,@ProcessStatusTrace,@ProcessTime); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("FileName", item.FileName);
			sqlCommand.Parameters.AddWithValue("ProcessStatus", item.ProcessStatus);
			sqlCommand.Parameters.AddWithValue("ProcessStatusName", item.ProcessStatusName);
			sqlCommand.Parameters.AddWithValue("ProcessStatusTrace", item.ProcessStatusTrace == null ? (object)DBNull.Value : item.ProcessStatusTrace);
			sqlCommand.Parameters.AddWithValue("ProcessTime", item.ProcessTime);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__EDI_DLF_ProcessedFile] ([FileName],[ProcessStatus],[ProcessStatusName],[ProcessStatusTrace],[ProcessTime]) VALUES ( "

						+ "@FileName" + i + ","
						+ "@ProcessStatus" + i + ","
						+ "@ProcessStatusName" + i + ","
						+ "@ProcessStatusTrace" + i + ","
						+ "@ProcessTime" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName);
					sqlCommand.Parameters.AddWithValue("ProcessStatus" + i, item.ProcessStatus);
					sqlCommand.Parameters.AddWithValue("ProcessStatusName" + i, item.ProcessStatusName);
					sqlCommand.Parameters.AddWithValue("ProcessStatusTrace" + i, item.ProcessStatusTrace == null ? (object)DBNull.Value : item.ProcessStatusTrace);
					sqlCommand.Parameters.AddWithValue("ProcessTime" + i, item.ProcessTime);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__EDI_DLF_ProcessedFile] SET [FileName]=@FileName, [ProcessStatus]=@ProcessStatus, [ProcessStatusName]=@ProcessStatusName, [ProcessStatusTrace]=@ProcessStatusTrace, [ProcessTime]=@ProcessTime WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("FileName", item.FileName);
			sqlCommand.Parameters.AddWithValue("ProcessStatus", item.ProcessStatus);
			sqlCommand.Parameters.AddWithValue("ProcessStatusName", item.ProcessStatusName);
			sqlCommand.Parameters.AddWithValue("ProcessStatusTrace", item.ProcessStatusTrace == null ? (object)DBNull.Value : item.ProcessStatusTrace);
			sqlCommand.Parameters.AddWithValue("ProcessTime", item.ProcessTime);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.ProcessedFileEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__EDI_DLF_ProcessedFile] SET "

					+ "[FileName]=@FileName" + i + ","
					+ "[ProcessStatus]=@ProcessStatus" + i + ","
					+ "[ProcessStatusName]=@ProcessStatusName" + i + ","
					+ "[ProcessStatusTrace]=@ProcessStatusTrace" + i + ","
					+ "[ProcessTime]=@ProcessTime" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName);
					sqlCommand.Parameters.AddWithValue("ProcessStatus" + i, item.ProcessStatus);
					sqlCommand.Parameters.AddWithValue("ProcessStatusName" + i, item.ProcessStatusName);
					sqlCommand.Parameters.AddWithValue("ProcessStatusTrace" + i, item.ProcessStatusTrace == null ? (object)DBNull.Value : item.ProcessStatusTrace);
					sqlCommand.Parameters.AddWithValue("ProcessTime" + i, item.ProcessTime);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__EDI_DLF_ProcessedFile] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__EDI_DLF_ProcessedFile] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods



		#endregion
	}
}
