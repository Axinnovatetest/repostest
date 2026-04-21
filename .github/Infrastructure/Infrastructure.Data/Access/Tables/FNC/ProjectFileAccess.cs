using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class ProjectFileAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectFile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectFile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_ProjectFile] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_ProjectFile] ([CreationTime],[CreationUserId],[CreationUserName],[FileExtension],[FileId],[FileName],[ProjectId])  VALUES (@CreationTime,@CreationUserId,@CreationUserName,@FileExtension,@FileId,@FileName,@ProjectId); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("FileExtension", item.FileExtension);
					sqlCommand.Parameters.AddWithValue("FileId", item.FileId);
					sqlCommand.Parameters.AddWithValue("FileName", item.FileName);
					sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> items)
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
						query += " INSERT INTO [__FNC_ProjectFile] ([CreationTime],[CreationUserId],[CreationUserName],[FileExtension],[FileId],[FileName],[ProjectId]) VALUES ( "

							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CreationUserName" + i + ","
							+ "@FileExtension" + i + ","
							+ "@FileId" + i + ","
							+ "@FileName" + i + ","
							+ "@ProjectId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("FileExtension" + i, item.FileExtension);
						sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_ProjectFile] SET [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [FileExtension]=@FileExtension, [FileId]=@FileId, [FileName]=@FileName, [ProjectId]=@ProjectId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName);
				sqlCommand.Parameters.AddWithValue("FileExtension", item.FileExtension);
				sqlCommand.Parameters.AddWithValue("FileId", item.FileId);
				sqlCommand.Parameters.AddWithValue("FileName", item.FileName);
				sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> items)
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
						query += " UPDATE [__FNC_ProjectFile] SET "

							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CreationUserName]=@CreationUserName" + i + ","
							+ "[FileExtension]=@FileExtension" + i + ","
							+ "[FileId]=@FileId" + i + ","
							+ "[FileName]=@FileName" + i + ","
							+ "[ProjectId]=@ProjectId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("FileExtension" + i, item.FileExtension);
						sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_ProjectFile] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_ProjectFile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> GetByProjectId(int projectId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectFile] WHERE [ProjectId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", projectId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity> GetByProjectIds(List<int> projectIds)
		{
			if(projectIds == null || projectIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_ProjectFile] WHERE [ProjectId] IN ({string.Join(",", projectIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectFileEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static int DeleteByProjectIdwExceptIds(int projectId, List<int> projectIds)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"DELETE FROM [__FNC_ProjectFile] WHERE [ProjectId]=@id {(projectIds != null && projectIds.Count > 0 ? $" AND [FileId] NOT IN ({string.Join(", ", projectIds)})" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", projectId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		#endregion
	}
}
