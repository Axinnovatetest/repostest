using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class UserWorkStationAuthorizationAccess
	{
		#region Default Methods
		public static Entities.Tables.MTM.UserWorkStationAuthorizationEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_UserWorkStationAuthorization] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.UserWorkStationAuthorizationEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_UserWorkStationAuthorization]";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity>();
			}
		}
		public static List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity> results = new List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity>();
		}
		private static List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
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

					sqlCommand.CommandText = "SELECT * FROM [__MTM_CRP_UserWorkStationAuthorization] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity>();
				}
			}
			return new List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity>();
		}

		public static int Insert(Entities.Tables.MTM.UserWorkStationAuthorizationEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_UserWorkStationAuthorization] ([CreationTime],[CreationUserId],[UserId],[UserName],[WorkStationId],[WorkStationName])  VALUES (@CreationTime,@CreationUserId,@UserId,@UserName,@WorkStationId,@WorkStationName)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName);
					sqlCommand.Parameters.AddWithValue("WorkStationId", item.WorkStationId);
					sqlCommand.Parameters.AddWithValue("WorkStationName", item.WorkStationName);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id] FROM [__MTM_CRP_UserWorkStationAuthorization] WHERE [Id] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Entities.Tables.MTM.UserWorkStationAuthorizationEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_UserWorkStationAuthorization] SET [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [UserId]=@UserId, [UserName]=@UserName, [WorkStationId]=@WorkStationId, [WorkStationName]=@WorkStationName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName);
				sqlCommand.Parameters.AddWithValue("WorkStationId", item.WorkStationId);
				sqlCommand.Parameters.AddWithValue("WorkStationName", item.WorkStationName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_UserWorkStationAuthorization] WHERE [Id]=@Id";
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
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
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

					string query = "DELETE FROM [__MTM_CRP_UserWorkStationAuthorization] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity> GetByUserId(int userId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_UserWorkStationAuthorization] WHERE [UserId]=@userId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity>();
			}
		}
		#endregion

		#region Helpers
		private static List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity> toList(DataTable dataTable)
		{
			var list = new List<Entities.Tables.MTM.UserWorkStationAuthorizationEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Entities.Tables.MTM.UserWorkStationAuthorizationEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
