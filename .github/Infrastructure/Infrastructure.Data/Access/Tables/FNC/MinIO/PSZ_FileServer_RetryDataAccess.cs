using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

namespace Infrastructure.Data.Access.Tables.FNC.MinIO
{
	public class PSZ_FileServer_RetryDataAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_FileServer_RetryData] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_FileServer_RetryData]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_FileServer_RetryData] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_FileServer_RetryData] ([AddedOn],[ErrorLevel],[Exception],[FileExtension],[FileName],[UserId]) OUTPUT INSERTED.[Id] VALUES (@AddedOn,@ErrorLevel,@Exception,@FileExtension,@FileName,@UserId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AddedOn", item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
					sqlCommand.Parameters.AddWithValue("ErrorLevel", item.ErrorLevel == null ? (object)DBNull.Value : item.ErrorLevel);
					sqlCommand.Parameters.AddWithValue("Exception", item.Exception == null ? (object)DBNull.Value : item.Exception);
					sqlCommand.Parameters.AddWithValue("FileExtension", item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
					sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> items)
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
						query += " INSERT INTO [PSZ_FileServer_RetryData] ([AddedOn],[ErrorLevel],[Exception],[FileExtension],[FileName],[UserId]) VALUES ( "

							+ "@AddedOn" + i + ","
							+ "@ErrorLevel" + i + ","
							+ "@Exception" + i + ","
							+ "@FileExtension" + i + ","
							+ "@FileName" + i + ","
							+ "@UserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AddedOn" + i, item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
						sqlCommand.Parameters.AddWithValue("ErrorLevel" + i, item.ErrorLevel == null ? (object)DBNull.Value : item.ErrorLevel);
						sqlCommand.Parameters.AddWithValue("Exception" + i, item.Exception == null ? (object)DBNull.Value : item.Exception);
						sqlCommand.Parameters.AddWithValue("FileExtension" + i, item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_FileServer_RetryData] SET [AddedOn]=@AddedOn, [ErrorLevel]=@ErrorLevel, [Exception]=@Exception, [FileExtension]=@FileExtension, [FileName]=@FileName, [UserId]=@UserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AddedOn", item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
				sqlCommand.Parameters.AddWithValue("ErrorLevel", item.ErrorLevel == null ? (object)DBNull.Value : item.ErrorLevel);
				sqlCommand.Parameters.AddWithValue("Exception", item.Exception == null ? (object)DBNull.Value : item.Exception);
				sqlCommand.Parameters.AddWithValue("FileExtension", item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
				sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> items)
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
						query += " UPDATE [PSZ_FileServer_RetryData] SET "

							+ "[AddedOn]=@AddedOn" + i + ","
							+ "[ErrorLevel]=@ErrorLevel" + i + ","
							+ "[Exception]=@Exception" + i + ","
							+ "[FileExtension]=@FileExtension" + i + ","
							+ "[FileName]=@FileName" + i + ","
							+ "[UserId]=@UserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AddedOn" + i, item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
						sqlCommand.Parameters.AddWithValue("ErrorLevel" + i, item.ErrorLevel == null ? (object)DBNull.Value : item.ErrorLevel);
						sqlCommand.Parameters.AddWithValue("Exception" + i, item.Exception == null ? (object)DBNull.Value : item.Exception);
						sqlCommand.Parameters.AddWithValue("FileExtension" + i, item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
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
				string query = "DELETE FROM [PSZ_FileServer_RetryData] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [PSZ_FileServer_RetryData] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_FileServer_RetryData] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_FileServer_RetryData]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [PSZ_FileServer_RetryData] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [PSZ_FileServer_RetryData] ([AddedOn],[ErrorLevel],[Exception],[FileExtension],[FileName],[UserId]) OUTPUT INSERTED.[Id] VALUES (@AddedOn,@ErrorLevel,@Exception,@FileExtension,@FileName,@UserId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AddedOn", item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
			sqlCommand.Parameters.AddWithValue("ErrorLevel", item.ErrorLevel == null ? (object)DBNull.Value : item.ErrorLevel);
			sqlCommand.Parameters.AddWithValue("Exception", item.Exception == null ? (object)DBNull.Value : item.Exception);
			sqlCommand.Parameters.AddWithValue("FileExtension", item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
			sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [PSZ_FileServer_RetryData] ([AddedOn],[ErrorLevel],[Exception],[FileExtension],[FileName],[UserId]) VALUES ( "

						+ "@AddedOn" + i + ","
						+ "@ErrorLevel" + i + ","
						+ "@Exception" + i + ","
						+ "@FileExtension" + i + ","
						+ "@FileName" + i + ","
						+ "@UserId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AddedOn" + i, item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
					sqlCommand.Parameters.AddWithValue("ErrorLevel" + i, item.ErrorLevel == null ? (object)DBNull.Value : item.ErrorLevel);
					sqlCommand.Parameters.AddWithValue("Exception" + i, item.Exception == null ? (object)DBNull.Value : item.Exception);
					sqlCommand.Parameters.AddWithValue("FileExtension" + i, item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
					sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [PSZ_FileServer_RetryData] SET [AddedOn]=@AddedOn, [ErrorLevel]=@ErrorLevel, [Exception]=@Exception, [FileExtension]=@FileExtension, [FileName]=@FileName, [UserId]=@UserId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AddedOn", item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
			sqlCommand.Parameters.AddWithValue("ErrorLevel", item.ErrorLevel == null ? (object)DBNull.Value : item.ErrorLevel);
			sqlCommand.Parameters.AddWithValue("Exception", item.Exception == null ? (object)DBNull.Value : item.Exception);
			sqlCommand.Parameters.AddWithValue("FileExtension", item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
			sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PSZ_FileServer_RetryData] SET "

					+ "[AddedOn]=@AddedOn" + i + ","
					+ "[ErrorLevel]=@ErrorLevel" + i + ","
					+ "[Exception]=@Exception" + i + ","
					+ "[FileExtension]=@FileExtension" + i + ","
					+ "[FileName]=@FileName" + i + ","
					+ "[UserId]=@UserId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AddedOn" + i, item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
					sqlCommand.Parameters.AddWithValue("ErrorLevel" + i, item.ErrorLevel == null ? (object)DBNull.Value : item.ErrorLevel);
					sqlCommand.Parameters.AddWithValue("Exception" + i, item.Exception == null ? (object)DBNull.Value : item.Exception);
					sqlCommand.Parameters.AddWithValue("FileExtension" + i, item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
					sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [PSZ_FileServer_RetryData] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [PSZ_FileServer_RetryData] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int UpdateErrorLevel(int Id, int errorLevel)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"UPDATE [PSZ_FileServer_RetryData] SET [ErrorLevel]={errorLevel} WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", Id);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		//GetWithPagination



		public static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity3> GetAllFailedFile(int userId = 0)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						select Count(*) over() TotalCount,* from PSZ_FileServer_RetryData
								where ErrorLevel != 1  order by AddedOn desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity3(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity3>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity3> GetAllFailedFiles()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						select * from PSZ_FileServer_RetryData
								where ErrorLevel != 1  order by AddedOn ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity3(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity3>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity4> GetFilesErrorsCount()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						select top 1  Count(*) over() TotalCount from [PSZ_FileServer_RetryData] where ErrorLevel IN (-1,-2) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity4(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.MinIO.PSZ_FileServer_RetryDataEntity4>();
			}
		}
		#endregion Custom Methods

	}


}
