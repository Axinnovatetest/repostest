using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class CTSBlanketFilesAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTSBlanketFiles] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTSBlanketFiles]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTSBlanketFiles] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTSBlanketFiles] ([AngeboteNr],[CreationTime],[CreationUserId],[FileExtension],[FileName],[FileId])  VALUES (@AngeboteNr,@CreationTime,@CreationUserId,@FileExtension,@FileName,@FileId); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AngeboteNr", item.AngeboteNr == null ? (object)DBNull.Value : item.AngeboteNr);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("FileExtension", item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
					sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("FileId", item.FileId == null ? (object)DBNull.Value : item.FileId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> items)
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
						query += " INSERT INTO [__CTSBlanketFiles] ([AngeboteNr],[CreationTime],[CreationUserId],[FileExtension],[FileName],[FileId]) VALUES ( "

							+ "@AngeboteNr" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@FileExtension" + i + ","
							+ "@FileName" + i + ","
							+ "@FileId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AngeboteNr" + i, item.AngeboteNr == null ? (object)DBNull.Value : item.AngeboteNr);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("FileExtension" + i, item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId == null ? (object)DBNull.Value : item.FileId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTSBlanketFiles] SET [AngeboteNr]=@AngeboteNr, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [FileExtension]=@FileExtension, [FileName]=@FileName,[FileId]=@FileId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AngeboteNr", item.AngeboteNr == null ? (object)DBNull.Value : item.AngeboteNr);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("FileExtension", item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
				sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
				sqlCommand.Parameters.AddWithValue("FileId", item.FileId == null ? (object)DBNull.Value : item.FileId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> items)
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
						query += " UPDATE [__CTSBlanketFiles] SET "

							+ "[AngeboteNr]=@AngeboteNr" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[FileExtension]=@FileExtension" + i + ","
							+ "[FileId]=@FileId" + i + ","
							+ "[FileName]=@FileName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AngeboteNr" + i, item.AngeboteNr == null ? (object)DBNull.Value : item.AngeboteNr);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("FileExtension" + i, item.FileExtension == null ? (object)DBNull.Value : item.FileExtension);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId == null ? (object)DBNull.Value : item.FileId);
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
				string query = "DELETE FROM [__CTSBlanketFiles] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__CTSBlanketFiles] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static int DeleteByAngeboteNrwExceptIds(int AngeboteNr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"DELETE FROM [__CTSBlanketFiles] WHERE [AngeboteNr]=@AngeboteNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("AngeboteNr", AngeboteNr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity> GetByAngebotNr(int angebotNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTSBlanketFiles] WHERE [AngeboteNr]=@angebotNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity GetByName(string fileName)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTSBlanketFiles] WHERE [FileName]=@fileName";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fileName", fileName);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int DeleteFile(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__CTSBlanketFiles] WHERE [FileId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity GetFile(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTSBlanketFiles] WHERE [FileId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		//public static int DeleteFile(int id)
		//{
		//    int results = -1;
		//    using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//    {
		//        sqlConnection.Open();
		//        string query = "DELETE FROM [__CTSBlanketFiles] WHERE [FileId]=@Id";
		//        var sqlCommand = new SqlCommand(query, sqlConnection);
		//        sqlCommand.Parameters.AddWithValue("Id", id);

		//        results = DbExecution.ExecuteNonQuery(sqlCommand);
		//    }

		//    return results;
		//}
		//public static Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity GetFile(int id)
		//{
		//    var dataTable = new DataTable();
		//    using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//    {
		//        sqlConnection.Open();
		//        string query = "SELECT * FROM [__CTSBlanketFiles] WHERE [FileId]=@Id";
		//        var sqlCommand = new SqlCommand(query, sqlConnection);
		//        sqlCommand.Parameters.AddWithValue("Id", id);

		//        DbExecution.Fill(sqlCommand, dataTable);

		//    }

		//    if (dataTable.Rows.Count > 0)
		//    {
		//        return new Infrastructure.Data.Entities.Tables.CTS.CTSBlanketFilesEntity(dataTable.Rows[0]);
		//    }
		//    else
		//    {
		//        return null;
		//    }
		//}

		#endregion
	}
}
