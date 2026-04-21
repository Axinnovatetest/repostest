using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class BomMailUsersAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_BomMailUsers] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_BomMailUsers]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_BomMailUsers] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_BomMailUsers] ([SiteCode],[SiteId],[SiteName],[UserEmail],[UserfullName],[UserId],[UserName])  VALUES (@SiteCode,@SiteId,@SiteName,@UserEmail,@UserfullName,@UserId,@UserName); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("SiteCode", item.SiteCode == null ? (object)DBNull.Value : item.SiteCode);
					sqlCommand.Parameters.AddWithValue("SiteId", item.SiteId);
					sqlCommand.Parameters.AddWithValue("SiteName", item.SiteName == null ? (object)DBNull.Value : item.SiteName);
					sqlCommand.Parameters.AddWithValue("UserEmail", item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
					sqlCommand.Parameters.AddWithValue("UserfullName", item.UserfullName == null ? (object)DBNull.Value : item.UserfullName);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> items)
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
						query += " INSERT INTO [__BSD_BomMailUsers] ([SiteCode],[SiteId],[SiteName],[UserEmail],[UserfullName],[UserId],[UserName]) VALUES ( "

							+ "@SiteCode" + i + ","
							+ "@SiteId" + i + ","
							+ "@SiteName" + i + ","
							+ "@UserEmail" + i + ","
							+ "@UserfullName" + i + ","
							+ "@UserId" + i + ","
							+ "@UserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("SiteCode" + i, item.SiteCode == null ? (object)DBNull.Value : item.SiteCode);
						sqlCommand.Parameters.AddWithValue("SiteId" + i, item.SiteId);
						sqlCommand.Parameters.AddWithValue("SiteName" + i, item.SiteName == null ? (object)DBNull.Value : item.SiteName);
						sqlCommand.Parameters.AddWithValue("UserEmail" + i, item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
						sqlCommand.Parameters.AddWithValue("UserfullName" + i, item.UserfullName == null ? (object)DBNull.Value : item.UserfullName);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_BomMailUsers] SET [SiteCode]=@SiteCode, [SiteId]=@SiteId, [SiteName]=@SiteName, [UserEmail]=@UserEmail, [UserfullName]=@UserfullName, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("SiteCode", item.SiteCode == null ? (object)DBNull.Value : item.SiteCode);
				sqlCommand.Parameters.AddWithValue("SiteId", item.SiteId);
				sqlCommand.Parameters.AddWithValue("SiteName", item.SiteName == null ? (object)DBNull.Value : item.SiteName);
				sqlCommand.Parameters.AddWithValue("UserEmail", item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
				sqlCommand.Parameters.AddWithValue("UserfullName", item.UserfullName == null ? (object)DBNull.Value : item.UserfullName);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> items)
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
						query += " UPDATE [__BSD_BomMailUsers] SET "

							+ "[SiteCode]=@SiteCode" + i + ","
							+ "[SiteId]=@SiteId" + i + ","
							+ "[SiteName]=@SiteName" + i + ","
							+ "[UserEmail]=@UserEmail" + i + ","
							+ "[UserfullName]=@UserfullName" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("SiteCode" + i, item.SiteCode == null ? (object)DBNull.Value : item.SiteCode);
						sqlCommand.Parameters.AddWithValue("SiteId" + i, item.SiteId);
						sqlCommand.Parameters.AddWithValue("SiteName" + i, item.SiteName == null ? (object)DBNull.Value : item.SiteName);
						sqlCommand.Parameters.AddWithValue("UserEmail" + i, item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
						sqlCommand.Parameters.AddWithValue("UserfullName" + i, item.UserfullName == null ? (object)DBNull.Value : item.UserfullName);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
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
				string query = "DELETE FROM [__BSD_BomMailUsers] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_BomMailUsers] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion


		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> GetBySite(string site)
		{
			if(string.IsNullOrWhiteSpace(site))
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_BomMailUsers] where [SiteCode]=@site";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("site", site);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity> GetBySite(string site, SqlConnection connection, SqlTransaction transaction)
		{
			if(string.IsNullOrWhiteSpace(site))
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();

			var dataTable = new DataTable();
				string query = "SELECT * FROM [__BSD_BomMailUsers] where [SiteCode]=@site";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("site", site);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomMailUsersEntity>();
			}
		}
		public static int DeleteUserFromSite(int userId, int siteId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__BSD_BomMailUsers] WHERE [UserId]=@userId AND [SiteId]=@siteId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("siteId", siteId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion
	}
}
