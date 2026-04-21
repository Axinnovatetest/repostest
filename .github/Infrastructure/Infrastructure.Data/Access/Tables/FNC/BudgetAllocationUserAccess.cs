using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class BudgetAllocationUserAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetAllocationUser] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetAllocationUser]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_BudgetAllocationUser] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_BudgetAllocationUser] ([AmountFix],[AmountInvest],[AmountMonth],[AmountNotificationThreshold],[AmountOrder],[AmountSpent],[AmountYear],[CreationTime],[CreationUserId],[LastEditTime],[LastEditUserId],[LastFreezeTime],[LastFreezeUserId],[LastResetTime],[LastResetUserId],[LastUnFreezeTime],[LastUnFreezeUserId],[LastUnResetTime],[LastUnResetUserId],[UserId],[UserName],[Year]) OUTPUT INSERTED.[Id] VALUES (@AmountFix,@AmountInvest,@AmountMonth,@AmountNotificationThreshold,@AmountOrder,@AmountSpent,@AmountYear,@CreationTime,@CreationUserId,@LastEditTime,@LastEditUserId,@LastFreezeTime,@LastFreezeUserId,@LastResetTime,@LastResetUserId,@LastUnFreezeTime,@LastUnFreezeUserId,@LastUnResetTime,@LastUnResetUserId,@UserId,@UserName,@Year); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AmountFix", item.AmountFix);
					sqlCommand.Parameters.AddWithValue("AmountInvest", item.AmountInvest);
					sqlCommand.Parameters.AddWithValue("AmountMonth", item.AmountMonth);
					sqlCommand.Parameters.AddWithValue("AmountNotificationThreshold", item.AmountNotificationThreshold == null ? (object)DBNull.Value : item.AmountNotificationThreshold);
					sqlCommand.Parameters.AddWithValue("AmountOrder", item.AmountOrder);
					sqlCommand.Parameters.AddWithValue("AmountSpent", item.AmountSpent);
					sqlCommand.Parameters.AddWithValue("AmountYear", item.AmountYear);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastFreezeTime", item.LastFreezeTime == null ? (object)DBNull.Value : item.LastFreezeTime);
					sqlCommand.Parameters.AddWithValue("LastFreezeUserId", item.LastFreezeUserId == null ? (object)DBNull.Value : item.LastFreezeUserId);
					sqlCommand.Parameters.AddWithValue("LastResetTime", item.LastResetTime == null ? (object)DBNull.Value : item.LastResetTime);
					sqlCommand.Parameters.AddWithValue("LastResetUserId", item.LastResetUserId == null ? (object)DBNull.Value : item.LastResetUserId);
					sqlCommand.Parameters.AddWithValue("LastUnFreezeTime", item.LastUnFreezeTime == null ? (object)DBNull.Value : item.LastUnFreezeTime);
					sqlCommand.Parameters.AddWithValue("LastUnFreezeUserId", item.LastUnFreezeUserId == null ? (object)DBNull.Value : item.LastUnFreezeUserId);
					sqlCommand.Parameters.AddWithValue("LastUnResetTime", item.LastUnResetTime == null ? (object)DBNull.Value : item.LastUnResetTime);
					sqlCommand.Parameters.AddWithValue("LastUnResetUserId", item.LastUnResetUserId == null ? (object)DBNull.Value : item.LastUnResetUserId);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
					sqlCommand.Parameters.AddWithValue("Year", item.Year);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> items)
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
						query += " INSERT INTO [__FNC_BudgetAllocationUser] ([AmountFix],[AmountInvest],[AmountMonth],[AmountNotificationThreshold],[AmountOrder],[AmountSpent],[AmountYear],[CreationTime],[CreationUserId],[LastEditTime],[LastEditUserId],[LastFreezeTime],[LastFreezeUserId],[LastResetTime],[LastResetUserId],[LastUnFreezeTime],[LastUnFreezeUserId],[LastUnResetTime],[LastUnResetUserId],[UserId],[UserName],[Year]) VALUES ( "

							+ "@AmountFix" + i + ","
							+ "@AmountInvest" + i + ","
							+ "@AmountMonth" + i + ","
							+ "@AmountNotificationThreshold" + i + ","
							+ "@AmountOrder" + i + ","
							+ "@AmountSpent" + i + ","
							+ "@AmountYear" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@LastFreezeTime" + i + ","
							+ "@LastFreezeUserId" + i + ","
							+ "@LastResetTime" + i + ","
							+ "@LastResetUserId" + i + ","
							+ "@LastUnFreezeTime" + i + ","
							+ "@LastUnFreezeUserId" + i + ","
							+ "@LastUnResetTime" + i + ","
							+ "@LastUnResetUserId" + i + ","
							+ "@UserId" + i + ","
							+ "@UserName" + i + ","
							+ "@Year" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AmountFix" + i, item.AmountFix);
						sqlCommand.Parameters.AddWithValue("AmountInvest" + i, item.AmountInvest);
						sqlCommand.Parameters.AddWithValue("AmountMonth" + i, item.AmountMonth);
						sqlCommand.Parameters.AddWithValue("AmountNotificationThreshold" + i, item.AmountNotificationThreshold == null ? (object)DBNull.Value : item.AmountNotificationThreshold);
						sqlCommand.Parameters.AddWithValue("AmountOrder" + i, item.AmountOrder);
						sqlCommand.Parameters.AddWithValue("AmountSpent" + i, item.AmountSpent);
						sqlCommand.Parameters.AddWithValue("AmountYear" + i, item.AmountYear);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastFreezeTime" + i, item.LastFreezeTime == null ? (object)DBNull.Value : item.LastFreezeTime);
						sqlCommand.Parameters.AddWithValue("LastFreezeUserId" + i, item.LastFreezeUserId == null ? (object)DBNull.Value : item.LastFreezeUserId);
						sqlCommand.Parameters.AddWithValue("LastResetTime" + i, item.LastResetTime == null ? (object)DBNull.Value : item.LastResetTime);
						sqlCommand.Parameters.AddWithValue("LastResetUserId" + i, item.LastResetUserId == null ? (object)DBNull.Value : item.LastResetUserId);
						sqlCommand.Parameters.AddWithValue("LastUnFreezeTime" + i, item.LastUnFreezeTime == null ? (object)DBNull.Value : item.LastUnFreezeTime);
						sqlCommand.Parameters.AddWithValue("LastUnFreezeUserId" + i, item.LastUnFreezeUserId == null ? (object)DBNull.Value : item.LastUnFreezeUserId);
						sqlCommand.Parameters.AddWithValue("LastUnResetTime" + i, item.LastUnResetTime == null ? (object)DBNull.Value : item.LastUnResetTime);
						sqlCommand.Parameters.AddWithValue("LastUnResetUserId" + i, item.LastUnResetUserId == null ? (object)DBNull.Value : item.LastUnResetUserId);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_BudgetAllocationUser] SET [AmountFix]=@AmountFix, [AmountInvest]=@AmountInvest, [AmountMonth]=@AmountMonth, [AmountNotificationThreshold]=@AmountNotificationThreshold, [AmountOrder]=@AmountOrder, [AmountSpent]=@AmountSpent, [AmountYear]=@AmountYear, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [LastFreezeTime]=@LastFreezeTime, [LastFreezeUserId]=@LastFreezeUserId, [LastResetTime]=@LastResetTime, [LastResetUserId]=@LastResetUserId, [LastUnFreezeTime]=@LastUnFreezeTime, [LastUnFreezeUserId]=@LastUnFreezeUserId, [LastUnResetTime]=@LastUnResetTime, [LastUnResetUserId]=@LastUnResetUserId, [UserId]=@UserId, [UserName]=@UserName, [Year]=@Year WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AmountFix", item.AmountFix);
				sqlCommand.Parameters.AddWithValue("AmountInvest", item.AmountInvest);
				sqlCommand.Parameters.AddWithValue("AmountMonth", item.AmountMonth);
				sqlCommand.Parameters.AddWithValue("AmountNotificationThreshold", item.AmountNotificationThreshold == null ? (object)DBNull.Value : item.AmountNotificationThreshold);
				sqlCommand.Parameters.AddWithValue("AmountOrder", item.AmountOrder);
				sqlCommand.Parameters.AddWithValue("AmountSpent", item.AmountSpent);
				sqlCommand.Parameters.AddWithValue("AmountYear", item.AmountYear);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("LastFreezeTime", item.LastFreezeTime == null ? (object)DBNull.Value : item.LastFreezeTime);
				sqlCommand.Parameters.AddWithValue("LastFreezeUserId", item.LastFreezeUserId == null ? (object)DBNull.Value : item.LastFreezeUserId);
				sqlCommand.Parameters.AddWithValue("LastResetTime", item.LastResetTime == null ? (object)DBNull.Value : item.LastResetTime);
				sqlCommand.Parameters.AddWithValue("LastResetUserId", item.LastResetUserId == null ? (object)DBNull.Value : item.LastResetUserId);
				sqlCommand.Parameters.AddWithValue("LastUnFreezeTime", item.LastUnFreezeTime == null ? (object)DBNull.Value : item.LastUnFreezeTime);
				sqlCommand.Parameters.AddWithValue("LastUnFreezeUserId", item.LastUnFreezeUserId == null ? (object)DBNull.Value : item.LastUnFreezeUserId);
				sqlCommand.Parameters.AddWithValue("LastUnResetTime", item.LastUnResetTime == null ? (object)DBNull.Value : item.LastUnResetTime);
				sqlCommand.Parameters.AddWithValue("LastUnResetUserId", item.LastUnResetUserId == null ? (object)DBNull.Value : item.LastUnResetUserId);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
				sqlCommand.Parameters.AddWithValue("Year", item.Year);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> items)
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
						query += " UPDATE [__FNC_BudgetAllocationUser] SET "

							+ "[AmountFix]=@AmountFix" + i + ","
							+ "[AmountInvest]=@AmountInvest" + i + ","
							+ "[AmountMonth]=@AmountMonth" + i + ","
							+ "[AmountNotificationThreshold]=@AmountNotificationThreshold" + i + ","
							+ "[AmountOrder]=@AmountOrder" + i + ","
							+ "[AmountSpent]=@AmountSpent" + i + ","
							+ "[AmountYear]=@AmountYear" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[LastFreezeTime]=@LastFreezeTime" + i + ","
							+ "[LastFreezeUserId]=@LastFreezeUserId" + i + ","
							+ "[LastResetTime]=@LastResetTime" + i + ","
							+ "[LastResetUserId]=@LastResetUserId" + i + ","
							+ "[LastUnFreezeTime]=@LastUnFreezeTime" + i + ","
							+ "[LastUnFreezeUserId]=@LastUnFreezeUserId" + i + ","
							+ "[LastUnResetTime]=@LastUnResetTime" + i + ","
							+ "[LastUnResetUserId]=@LastUnResetUserId" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserName]=@UserName" + i + ","
							+ "[Year]=@Year" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AmountFix" + i, item.AmountFix);
						sqlCommand.Parameters.AddWithValue("AmountInvest" + i, item.AmountInvest);
						sqlCommand.Parameters.AddWithValue("AmountMonth" + i, item.AmountMonth);
						sqlCommand.Parameters.AddWithValue("AmountNotificationThreshold" + i, item.AmountNotificationThreshold == null ? (object)DBNull.Value : item.AmountNotificationThreshold);
						sqlCommand.Parameters.AddWithValue("AmountOrder" + i, item.AmountOrder);
						sqlCommand.Parameters.AddWithValue("AmountSpent" + i, item.AmountSpent);
						sqlCommand.Parameters.AddWithValue("AmountYear" + i, item.AmountYear);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastFreezeTime" + i, item.LastFreezeTime == null ? (object)DBNull.Value : item.LastFreezeTime);
						sqlCommand.Parameters.AddWithValue("LastFreezeUserId" + i, item.LastFreezeUserId == null ? (object)DBNull.Value : item.LastFreezeUserId);
						sqlCommand.Parameters.AddWithValue("LastResetTime" + i, item.LastResetTime == null ? (object)DBNull.Value : item.LastResetTime);
						sqlCommand.Parameters.AddWithValue("LastResetUserId" + i, item.LastResetUserId == null ? (object)DBNull.Value : item.LastResetUserId);
						sqlCommand.Parameters.AddWithValue("LastUnFreezeTime" + i, item.LastUnFreezeTime == null ? (object)DBNull.Value : item.LastUnFreezeTime);
						sqlCommand.Parameters.AddWithValue("LastUnFreezeUserId" + i, item.LastUnFreezeUserId == null ? (object)DBNull.Value : item.LastUnFreezeUserId);
						sqlCommand.Parameters.AddWithValue("LastUnResetTime" + i, item.LastUnResetTime == null ? (object)DBNull.Value : item.LastUnResetTime);
						sqlCommand.Parameters.AddWithValue("LastUnResetUserId" + i, item.LastUnResetUserId == null ? (object)DBNull.Value : item.LastUnResetUserId);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
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
				string query = "DELETE FROM [__FNC_BudgetAllocationUser] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_BudgetAllocationUser] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_BudgetAllocationUser] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_BudgetAllocationUser]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__FNC_BudgetAllocationUser] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__FNC_BudgetAllocationUser] ([AmountFix],[AmountInvest],[AmountMonth],[AmountNotificationThreshold],[AmountOrder],[AmountSpent],[AmountYear],[CreationTime],[CreationUserId],[LastEditTime],[LastEditUserId],[LastFreezeTime],[LastFreezeUserId],[LastResetTime],[LastResetUserId],[LastUnFreezeTime],[LastUnFreezeUserId],[LastUnResetTime],[LastUnResetUserId],[UserId],[UserName],[Year]) OUTPUT INSERTED.[Id] VALUES (@AmountFix,@AmountInvest,@AmountMonth,@AmountNotificationThreshold,@AmountOrder,@AmountSpent,@AmountYear,@CreationTime,@CreationUserId,@LastEditTime,@LastEditUserId,@LastFreezeTime,@LastFreezeUserId,@LastResetTime,@LastResetUserId,@LastUnFreezeTime,@LastUnFreezeUserId,@LastUnResetTime,@LastUnResetUserId,@UserId,@UserName,@Year); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AmountFix", item.AmountFix);
			sqlCommand.Parameters.AddWithValue("AmountInvest", item.AmountInvest);
			sqlCommand.Parameters.AddWithValue("AmountMonth", item.AmountMonth);
			sqlCommand.Parameters.AddWithValue("AmountNotificationThreshold", item.AmountNotificationThreshold == null ? (object)DBNull.Value : item.AmountNotificationThreshold);
			sqlCommand.Parameters.AddWithValue("AmountOrder", item.AmountOrder);
			sqlCommand.Parameters.AddWithValue("AmountSpent", item.AmountSpent);
			sqlCommand.Parameters.AddWithValue("AmountYear", item.AmountYear);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LastFreezeTime", item.LastFreezeTime == null ? (object)DBNull.Value : item.LastFreezeTime);
			sqlCommand.Parameters.AddWithValue("LastFreezeUserId", item.LastFreezeUserId == null ? (object)DBNull.Value : item.LastFreezeUserId);
			sqlCommand.Parameters.AddWithValue("LastResetTime", item.LastResetTime == null ? (object)DBNull.Value : item.LastResetTime);
			sqlCommand.Parameters.AddWithValue("LastResetUserId", item.LastResetUserId == null ? (object)DBNull.Value : item.LastResetUserId);
			sqlCommand.Parameters.AddWithValue("LastUnFreezeTime", item.LastUnFreezeTime == null ? (object)DBNull.Value : item.LastUnFreezeTime);
			sqlCommand.Parameters.AddWithValue("LastUnFreezeUserId", item.LastUnFreezeUserId == null ? (object)DBNull.Value : item.LastUnFreezeUserId);
			sqlCommand.Parameters.AddWithValue("LastUnResetTime", item.LastUnResetTime == null ? (object)DBNull.Value : item.LastUnResetTime);
			sqlCommand.Parameters.AddWithValue("LastUnResetUserId", item.LastUnResetUserId == null ? (object)DBNull.Value : item.LastUnResetUserId);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
			sqlCommand.Parameters.AddWithValue("Year", item.Year);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__FNC_BudgetAllocationUser] ([AmountFix],[AmountInvest],[AmountMonth],[AmountNotificationThreshold],[AmountOrder],[AmountSpent],[AmountYear],[CreationTime],[CreationUserId],[LastEditTime],[LastEditUserId],[LastFreezeTime],[LastFreezeUserId],[LastResetTime],[LastResetUserId],[LastUnFreezeTime],[LastUnFreezeUserId],[LastUnResetTime],[LastUnResetUserId],[UserId],[UserName],[Year]) VALUES ( "

						+ "@AmountFix" + i + ","
						+ "@AmountInvest" + i + ","
						+ "@AmountMonth" + i + ","
						+ "@AmountNotificationThreshold" + i + ","
						+ "@AmountOrder" + i + ","
						+ "@AmountSpent" + i + ","
						+ "@AmountYear" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@LastEditTime" + i + ","
						+ "@LastEditUserId" + i + ","
						+ "@LastFreezeTime" + i + ","
						+ "@LastFreezeUserId" + i + ","
						+ "@LastResetTime" + i + ","
						+ "@LastResetUserId" + i + ","
						+ "@LastUnFreezeTime" + i + ","
						+ "@LastUnFreezeUserId" + i + ","
						+ "@LastUnResetTime" + i + ","
						+ "@LastUnResetUserId" + i + ","
						+ "@UserId" + i + ","
						+ "@UserName" + i + ","
						+ "@Year" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AmountFix" + i, item.AmountFix);
					sqlCommand.Parameters.AddWithValue("AmountInvest" + i, item.AmountInvest);
					sqlCommand.Parameters.AddWithValue("AmountMonth" + i, item.AmountMonth);
					sqlCommand.Parameters.AddWithValue("AmountNotificationThreshold" + i, item.AmountNotificationThreshold == null ? (object)DBNull.Value : item.AmountNotificationThreshold);
					sqlCommand.Parameters.AddWithValue("AmountOrder" + i, item.AmountOrder);
					sqlCommand.Parameters.AddWithValue("AmountSpent" + i, item.AmountSpent);
					sqlCommand.Parameters.AddWithValue("AmountYear" + i, item.AmountYear);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastFreezeTime" + i, item.LastFreezeTime == null ? (object)DBNull.Value : item.LastFreezeTime);
					sqlCommand.Parameters.AddWithValue("LastFreezeUserId" + i, item.LastFreezeUserId == null ? (object)DBNull.Value : item.LastFreezeUserId);
					sqlCommand.Parameters.AddWithValue("LastResetTime" + i, item.LastResetTime == null ? (object)DBNull.Value : item.LastResetTime);
					sqlCommand.Parameters.AddWithValue("LastResetUserId" + i, item.LastResetUserId == null ? (object)DBNull.Value : item.LastResetUserId);
					sqlCommand.Parameters.AddWithValue("LastUnFreezeTime" + i, item.LastUnFreezeTime == null ? (object)DBNull.Value : item.LastUnFreezeTime);
					sqlCommand.Parameters.AddWithValue("LastUnFreezeUserId" + i, item.LastUnFreezeUserId == null ? (object)DBNull.Value : item.LastUnFreezeUserId);
					sqlCommand.Parameters.AddWithValue("LastUnResetTime" + i, item.LastUnResetTime == null ? (object)DBNull.Value : item.LastUnResetTime);
					sqlCommand.Parameters.AddWithValue("LastUnResetUserId" + i, item.LastUnResetUserId == null ? (object)DBNull.Value : item.LastUnResetUserId);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__FNC_BudgetAllocationUser] SET [AmountFix]=@AmountFix, [AmountInvest]=@AmountInvest, [AmountMonth]=@AmountMonth, [AmountNotificationThreshold]=@AmountNotificationThreshold, [AmountOrder]=@AmountOrder, [AmountSpent]=@AmountSpent, [AmountYear]=@AmountYear, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [LastFreezeTime]=@LastFreezeTime, [LastFreezeUserId]=@LastFreezeUserId, [LastResetTime]=@LastResetTime, [LastResetUserId]=@LastResetUserId, [LastUnFreezeTime]=@LastUnFreezeTime, [LastUnFreezeUserId]=@LastUnFreezeUserId, [LastUnResetTime]=@LastUnResetTime, [LastUnResetUserId]=@LastUnResetUserId, [UserId]=@UserId, [UserName]=@UserName, [Year]=@Year WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AmountFix", item.AmountFix);
			sqlCommand.Parameters.AddWithValue("AmountInvest", item.AmountInvest);
			sqlCommand.Parameters.AddWithValue("AmountMonth", item.AmountMonth);
			sqlCommand.Parameters.AddWithValue("AmountNotificationThreshold", item.AmountNotificationThreshold == null ? (object)DBNull.Value : item.AmountNotificationThreshold);
			sqlCommand.Parameters.AddWithValue("AmountOrder", item.AmountOrder);
			sqlCommand.Parameters.AddWithValue("AmountSpent", item.AmountSpent);
			sqlCommand.Parameters.AddWithValue("AmountYear", item.AmountYear);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LastFreezeTime", item.LastFreezeTime == null ? (object)DBNull.Value : item.LastFreezeTime);
			sqlCommand.Parameters.AddWithValue("LastFreezeUserId", item.LastFreezeUserId == null ? (object)DBNull.Value : item.LastFreezeUserId);
			sqlCommand.Parameters.AddWithValue("LastResetTime", item.LastResetTime == null ? (object)DBNull.Value : item.LastResetTime);
			sqlCommand.Parameters.AddWithValue("LastResetUserId", item.LastResetUserId == null ? (object)DBNull.Value : item.LastResetUserId);
			sqlCommand.Parameters.AddWithValue("LastUnFreezeTime", item.LastUnFreezeTime == null ? (object)DBNull.Value : item.LastUnFreezeTime);
			sqlCommand.Parameters.AddWithValue("LastUnFreezeUserId", item.LastUnFreezeUserId == null ? (object)DBNull.Value : item.LastUnFreezeUserId);
			sqlCommand.Parameters.AddWithValue("LastUnResetTime", item.LastUnResetTime == null ? (object)DBNull.Value : item.LastUnResetTime);
			sqlCommand.Parameters.AddWithValue("LastUnResetUserId", item.LastUnResetUserId == null ? (object)DBNull.Value : item.LastUnResetUserId);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
			sqlCommand.Parameters.AddWithValue("Year", item.Year);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__FNC_BudgetAllocationUser] SET "

					+ "[AmountFix]=@AmountFix" + i + ","
					+ "[AmountInvest]=@AmountInvest" + i + ","
					+ "[AmountMonth]=@AmountMonth" + i + ","
					+ "[AmountNotificationThreshold]=@AmountNotificationThreshold" + i + ","
					+ "[AmountOrder]=@AmountOrder" + i + ","
					+ "[AmountSpent]=@AmountSpent" + i + ","
					+ "[AmountYear]=@AmountYear" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[LastEditTime]=@LastEditTime" + i + ","
					+ "[LastEditUserId]=@LastEditUserId" + i + ","
					+ "[LastFreezeTime]=@LastFreezeTime" + i + ","
					+ "[LastFreezeUserId]=@LastFreezeUserId" + i + ","
					+ "[LastResetTime]=@LastResetTime" + i + ","
					+ "[LastResetUserId]=@LastResetUserId" + i + ","
					+ "[LastUnFreezeTime]=@LastUnFreezeTime" + i + ","
					+ "[LastUnFreezeUserId]=@LastUnFreezeUserId" + i + ","
					+ "[LastUnResetTime]=@LastUnResetTime" + i + ","
					+ "[LastUnResetUserId]=@LastUnResetUserId" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[UserName]=@UserName" + i + ","
					+ "[Year]=@Year" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AmountFix" + i, item.AmountFix);
					sqlCommand.Parameters.AddWithValue("AmountInvest" + i, item.AmountInvest);
					sqlCommand.Parameters.AddWithValue("AmountMonth" + i, item.AmountMonth);
					sqlCommand.Parameters.AddWithValue("AmountNotificationThreshold" + i, item.AmountNotificationThreshold == null ? (object)DBNull.Value : item.AmountNotificationThreshold);
					sqlCommand.Parameters.AddWithValue("AmountOrder" + i, item.AmountOrder);
					sqlCommand.Parameters.AddWithValue("AmountSpent" + i, item.AmountSpent);
					sqlCommand.Parameters.AddWithValue("AmountYear" + i, item.AmountYear);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastFreezeTime" + i, item.LastFreezeTime == null ? (object)DBNull.Value : item.LastFreezeTime);
					sqlCommand.Parameters.AddWithValue("LastFreezeUserId" + i, item.LastFreezeUserId == null ? (object)DBNull.Value : item.LastFreezeUserId);
					sqlCommand.Parameters.AddWithValue("LastResetTime" + i, item.LastResetTime == null ? (object)DBNull.Value : item.LastResetTime);
					sqlCommand.Parameters.AddWithValue("LastResetUserId" + i, item.LastResetUserId == null ? (object)DBNull.Value : item.LastResetUserId);
					sqlCommand.Parameters.AddWithValue("LastUnFreezeTime" + i, item.LastUnFreezeTime == null ? (object)DBNull.Value : item.LastUnFreezeTime);
					sqlCommand.Parameters.AddWithValue("LastUnFreezeUserId" + i, item.LastUnFreezeUserId == null ? (object)DBNull.Value : item.LastUnFreezeUserId);
					sqlCommand.Parameters.AddWithValue("LastUnResetTime" + i, item.LastUnResetTime == null ? (object)DBNull.Value : item.LastUnResetTime);
					sqlCommand.Parameters.AddWithValue("LastUnResetUserId" + i, item.LastUnResetUserId == null ? (object)DBNull.Value : item.LastUnResetUserId);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__FNC_BudgetAllocationUser] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__FNC_BudgetAllocationUser] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity> GetByUsersAndYear(List<int> userIds, int? year = null)
		{
			if(userIds == null || userIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BudgetAllocationUser]  WHERE UserId IN ({string.Join(",", userIds)}) {(year.HasValue ? $" AND Year={year.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity GetByUserAndYear(int id, int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetAllocationUser] WHERE [UserId]=@Id AND Year=@year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int DeleteByUsersAndYear(List<int> userIds, int? year = null)
		{
			if(userIds == null || userIds.Count <= 0)
				return 0;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"DELETE FROM [__FNC_BudgetAllocationUser]  WHERE UserId IN ({string.Join(",", userIds)}) {(year.HasValue ? $" AND Year={year.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion Custom Methods
	}
}
