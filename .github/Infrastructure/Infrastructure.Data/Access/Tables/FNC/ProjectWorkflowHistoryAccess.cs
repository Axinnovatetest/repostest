using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class ProjectWorkflowHistoryAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectWorkflowHistory] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectWorkflowHistory]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_ProjectWorkflowHistory] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_ProjectWorkflowHistory] ([ProjectId],[ProjectName],[ProjectOwnerUserEmail],[ProjectOwnerUserId],[ProjectOwnerUserName],[WorkflowActionComments],[WorkflowActionId],[WorkflowActionName],[WorkflowActionTime],[WorkflowActionUserEmail],[WorkflowActionUserId],[WorkflowActionUserName])  VALUES (@ProjectId,@ProjectName,@ProjectOwnerUserEmail,@ProjectOwnerUserId,@ProjectOwnerUserName,@WorkflowActionComments,@WorkflowActionId,@WorkflowActionName,@WorkflowActionTime,@WorkflowActionUserEmail,@WorkflowActionUserId,@WorkflowActionUserName); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId);
					sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
					sqlCommand.Parameters.AddWithValue("ProjectOwnerUserEmail", item.ProjectOwnerUserEmail == null ? (object)DBNull.Value : item.ProjectOwnerUserEmail);
					sqlCommand.Parameters.AddWithValue("ProjectOwnerUserId", item.ProjectOwnerUserId == null ? (object)DBNull.Value : item.ProjectOwnerUserId);
					sqlCommand.Parameters.AddWithValue("ProjectOwnerUserName", item.ProjectOwnerUserName == null ? (object)DBNull.Value : item.ProjectOwnerUserName);
					sqlCommand.Parameters.AddWithValue("WorkflowActionComments", item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
					sqlCommand.Parameters.AddWithValue("WorkflowActionId", item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
					sqlCommand.Parameters.AddWithValue("WorkflowActionName", item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
					sqlCommand.Parameters.AddWithValue("WorkflowActionTime", item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail", item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserId", item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
					sqlCommand.Parameters.AddWithValue("WorkflowActionUserName", item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> items)
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
						query += " INSERT INTO [__FNC_ProjectWorkflowHistory] ([ProjectId],[ProjectName],[ProjectOwnerUserEmail],[ProjectOwnerUserId],[ProjectOwnerUserName],[WorkflowActionComments],[WorkflowActionId],[WorkflowActionName],[WorkflowActionTime],[WorkflowActionUserEmail],[WorkflowActionUserId],[WorkflowActionUserName]) VALUES ( "

							+ "@ProjectId" + i + ","
							+ "@ProjectName" + i + ","
							+ "@ProjectOwnerUserEmail" + i + ","
							+ "@ProjectOwnerUserId" + i + ","
							+ "@ProjectOwnerUserName" + i + ","
							+ "@WorkflowActionComments" + i + ","
							+ "@WorkflowActionId" + i + ","
							+ "@WorkflowActionName" + i + ","
							+ "@WorkflowActionTime" + i + ","
							+ "@WorkflowActionUserEmail" + i + ","
							+ "@WorkflowActionUserId" + i + ","
							+ "@WorkflowActionUserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId);
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("ProjectOwnerUserEmail" + i, item.ProjectOwnerUserEmail == null ? (object)DBNull.Value : item.ProjectOwnerUserEmail);
						sqlCommand.Parameters.AddWithValue("ProjectOwnerUserId" + i, item.ProjectOwnerUserId == null ? (object)DBNull.Value : item.ProjectOwnerUserId);
						sqlCommand.Parameters.AddWithValue("ProjectOwnerUserName" + i, item.ProjectOwnerUserName == null ? (object)DBNull.Value : item.ProjectOwnerUserName);
						sqlCommand.Parameters.AddWithValue("WorkflowActionComments" + i, item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
						sqlCommand.Parameters.AddWithValue("WorkflowActionId" + i, item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
						sqlCommand.Parameters.AddWithValue("WorkflowActionName" + i, item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
						sqlCommand.Parameters.AddWithValue("WorkflowActionTime" + i, item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail" + i, item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserId" + i, item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserName" + i, item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_ProjectWorkflowHistory] SET [ProjectId]=@ProjectId, [ProjectName]=@ProjectName, [ProjectOwnerUserEmail]=@ProjectOwnerUserEmail, [ProjectOwnerUserId]=@ProjectOwnerUserId, [ProjectOwnerUserName]=@ProjectOwnerUserName, [WorkflowActionComments]=@WorkflowActionComments, [WorkflowActionId]=@WorkflowActionId, [WorkflowActionName]=@WorkflowActionName, [WorkflowActionTime]=@WorkflowActionTime, [WorkflowActionUserEmail]=@WorkflowActionUserEmail, [WorkflowActionUserId]=@WorkflowActionUserId, [WorkflowActionUserName]=@WorkflowActionUserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId);
				sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
				sqlCommand.Parameters.AddWithValue("ProjectOwnerUserEmail", item.ProjectOwnerUserEmail == null ? (object)DBNull.Value : item.ProjectOwnerUserEmail);
				sqlCommand.Parameters.AddWithValue("ProjectOwnerUserId", item.ProjectOwnerUserId == null ? (object)DBNull.Value : item.ProjectOwnerUserId);
				sqlCommand.Parameters.AddWithValue("ProjectOwnerUserName", item.ProjectOwnerUserName == null ? (object)DBNull.Value : item.ProjectOwnerUserName);
				sqlCommand.Parameters.AddWithValue("WorkflowActionComments", item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
				sqlCommand.Parameters.AddWithValue("WorkflowActionId", item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
				sqlCommand.Parameters.AddWithValue("WorkflowActionName", item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
				sqlCommand.Parameters.AddWithValue("WorkflowActionTime", item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
				sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail", item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
				sqlCommand.Parameters.AddWithValue("WorkflowActionUserId", item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
				sqlCommand.Parameters.AddWithValue("WorkflowActionUserName", item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> items)
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
						query += " UPDATE [__FNC_ProjectWorkflowHistory] SET "

							+ "[ProjectId]=@ProjectId" + i + ","
							+ "[ProjectName]=@ProjectName" + i + ","
							+ "[ProjectOwnerUserEmail]=@ProjectOwnerUserEmail" + i + ","
							+ "[ProjectOwnerUserId]=@ProjectOwnerUserId" + i + ","
							+ "[ProjectOwnerUserName]=@ProjectOwnerUserName" + i + ","
							+ "[WorkflowActionComments]=@WorkflowActionComments" + i + ","
							+ "[WorkflowActionId]=@WorkflowActionId" + i + ","
							+ "[WorkflowActionName]=@WorkflowActionName" + i + ","
							+ "[WorkflowActionTime]=@WorkflowActionTime" + i + ","
							+ "[WorkflowActionUserEmail]=@WorkflowActionUserEmail" + i + ","
							+ "[WorkflowActionUserId]=@WorkflowActionUserId" + i + ","
							+ "[WorkflowActionUserName]=@WorkflowActionUserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId);
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("ProjectOwnerUserEmail" + i, item.ProjectOwnerUserEmail == null ? (object)DBNull.Value : item.ProjectOwnerUserEmail);
						sqlCommand.Parameters.AddWithValue("ProjectOwnerUserId" + i, item.ProjectOwnerUserId == null ? (object)DBNull.Value : item.ProjectOwnerUserId);
						sqlCommand.Parameters.AddWithValue("ProjectOwnerUserName" + i, item.ProjectOwnerUserName == null ? (object)DBNull.Value : item.ProjectOwnerUserName);
						sqlCommand.Parameters.AddWithValue("WorkflowActionComments" + i, item.WorkflowActionComments == null ? (object)DBNull.Value : item.WorkflowActionComments);
						sqlCommand.Parameters.AddWithValue("WorkflowActionId" + i, item.WorkflowActionId == null ? (object)DBNull.Value : item.WorkflowActionId);
						sqlCommand.Parameters.AddWithValue("WorkflowActionName" + i, item.WorkflowActionName == null ? (object)DBNull.Value : item.WorkflowActionName);
						sqlCommand.Parameters.AddWithValue("WorkflowActionTime" + i, item.WorkflowActionTime == null ? (object)DBNull.Value : item.WorkflowActionTime);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserEmail" + i, item.WorkflowActionUserEmail == null ? (object)DBNull.Value : item.WorkflowActionUserEmail);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserId" + i, item.WorkflowActionUserId == null ? (object)DBNull.Value : item.WorkflowActionUserId);
						sqlCommand.Parameters.AddWithValue("WorkflowActionUserName" + i, item.WorkflowActionUserName == null ? (object)DBNull.Value : item.WorkflowActionUserName);
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
				string query = "DELETE FROM [__FNC_ProjectWorkflowHistory] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_ProjectWorkflowHistory] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods


		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity> GetByProjectId(int projectId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectWorkflowHistory] WHERE [ProjectId]=@projectId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("projectId", projectId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity>();
			}
		}
		#endregion
	}
}
