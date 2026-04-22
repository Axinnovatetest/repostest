using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class EmailNotificationUsersAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity Get(long id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_EmailNotificationUsers] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_EmailNotificationUsers]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> Get(List<long> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> get(List<long> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_EmailNotificationUsers] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
		}

		public static long Insert(Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity item)
		{
			long response = long.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_EmailNotificationUsers] ([ArticleBomCpControl_Engineering],[ArticleBomCpControl_Quality],[ArticlePurchase],[ArticleSales],[UserEmail],[UserId],[UserName])  VALUES (@ArticleBomCpControl_Engineering,@ArticleBomCpControl_Quality,@ArticlePurchase,@ArticleSales,@UserEmail,@UserId,@UserName); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleBomCpControl_Engineering", item.ArticleBomCpControl_Engineering == null ? (object)DBNull.Value : item.ArticleBomCpControl_Engineering);
					sqlCommand.Parameters.AddWithValue("ArticleBomCpControl_Quality", item.ArticleBomCpControl_Quality == null ? (object)DBNull.Value : item.ArticleBomCpControl_Quality);
					sqlCommand.Parameters.AddWithValue("ArticlePurchase", item.ArticlePurchase == null ? (object)DBNull.Value : item.ArticlePurchase);
					sqlCommand.Parameters.AddWithValue("ArticleSales", item.ArticleSales == null ? (object)DBNull.Value : item.ArticleSales);
					sqlCommand.Parameters.AddWithValue("UserEmail", item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> items)
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
						query += " INSERT INTO [__BSD_EmailNotificationUsers] ([ArticleBomCpControl_Engineering],[ArticleBomCpControl_Quality],[ArticlePurchase],[ArticleSales],[UserEmail],[UserId],[UserName]) VALUES ( "

							+ "@ArticleBomCpControl_Engineering" + i + ","
							+ "@ArticleBomCpControl_Quality" + i + ","
							+ "@ArticlePurchase" + i + ","
							+ "@ArticleSales" + i + ","
							+ "@UserEmail" + i + ","
							+ "@UserId" + i + ","
							+ "@UserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleBomCpControl_Engineering" + i, item.ArticleBomCpControl_Engineering == null ? (object)DBNull.Value : item.ArticleBomCpControl_Engineering);
						sqlCommand.Parameters.AddWithValue("ArticleBomCpControl_Quality" + i, item.ArticleBomCpControl_Quality == null ? (object)DBNull.Value : item.ArticleBomCpControl_Quality);
						sqlCommand.Parameters.AddWithValue("ArticlePurchase" + i, item.ArticlePurchase == null ? (object)DBNull.Value : item.ArticlePurchase);
						sqlCommand.Parameters.AddWithValue("ArticleSales" + i, item.ArticleSales == null ? (object)DBNull.Value : item.ArticleSales);
						sqlCommand.Parameters.AddWithValue("UserEmail" + i, item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
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

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_EmailNotificationUsers] SET [ArticleBomCpControl_Engineering]=@ArticleBomCpControl_Engineering, [ArticleBomCpControl_Quality]=@ArticleBomCpControl_Quality, [ArticlePurchase]=@ArticlePurchase, [ArticleSales]=@ArticleSales, [UserEmail]=@UserEmail, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleBomCpControl_Engineering", item.ArticleBomCpControl_Engineering == null ? (object)DBNull.Value : item.ArticleBomCpControl_Engineering);
				sqlCommand.Parameters.AddWithValue("ArticleBomCpControl_Quality", item.ArticleBomCpControl_Quality == null ? (object)DBNull.Value : item.ArticleBomCpControl_Quality);
				sqlCommand.Parameters.AddWithValue("ArticlePurchase", item.ArticlePurchase == null ? (object)DBNull.Value : item.ArticlePurchase);
				sqlCommand.Parameters.AddWithValue("ArticleSales", item.ArticleSales == null ? (object)DBNull.Value : item.ArticleSales);
				sqlCommand.Parameters.AddWithValue("UserEmail", item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> items)
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
						query += " UPDATE [__BSD_EmailNotificationUsers] SET "

							+ "[ArticleBomCpControl_Engineering]=@ArticleBomCpControl_Engineering" + i + ","
							+ "[ArticleBomCpControl_Quality]=@ArticleBomCpControl_Quality" + i + ","
							+ "[ArticlePurchase]=@ArticlePurchase" + i + ","
							+ "[ArticleSales]=@ArticleSales" + i + ","
							+ "[UserEmail]=@UserEmail" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleBomCpControl_Engineering" + i, item.ArticleBomCpControl_Engineering == null ? (object)DBNull.Value : item.ArticleBomCpControl_Engineering);
						sqlCommand.Parameters.AddWithValue("ArticleBomCpControl_Quality" + i, item.ArticleBomCpControl_Quality == null ? (object)DBNull.Value : item.ArticleBomCpControl_Quality);
						sqlCommand.Parameters.AddWithValue("ArticlePurchase" + i, item.ArticlePurchase == null ? (object)DBNull.Value : item.ArticlePurchase);
						sqlCommand.Parameters.AddWithValue("ArticleSales" + i, item.ArticleSales == null ? (object)DBNull.Value : item.ArticleSales);
						sqlCommand.Parameters.AddWithValue("UserEmail" + i, item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
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

		public static int Delete(long id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__BSD_EmailNotificationUsers] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<long> ids)
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
		private static int delete(List<long> ids)
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

					string query = "DELETE FROM [__BSD_EmailNotificationUsers] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> GetByUsers(List<long> ids)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_EmailNotificationUsers] WHERE [UserId] IN ({string.Join(",", ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> GetSales()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_EmailNotificationUsers] WHERE [ArticleSales] = 1 ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> GetPurchase()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_EmailNotificationUsers] WHERE [ArticlePurchase] = 1 ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> GetPurchase(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [__BSD_EmailNotificationUsers] WHERE [ArticlePurchase] = 1 ";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> GetBomCpControl_Engineering()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_EmailNotificationUsers] WHERE [ArticleBomCpControl_Engineering] = 1 ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity> GetBomCpControl_Quality()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_EmailNotificationUsers] WHERE [ArticleBomCpControl_Quality] = 1 ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
			}
		}
		public static int DeleteByUser(long id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__BSD_EmailNotificationUsers] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		#endregion
	}
}
