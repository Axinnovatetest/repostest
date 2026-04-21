using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class HourlyRateAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_HourlyRate] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_HourlyRate]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_HourlyRate] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_HourlyRate] ([CreationTime],[CreationUserId],[CreationUserName],[HourlyRate],[LastEditTime],[LastEditUserId],[LastEditUserName],[ProductionSiteId],[ProductionSiteName]) OUTPUT INSERTED.[Id] VALUES (@CreationTime,@CreationUserId,@CreationUserName,@HourlyRate,@LastEditTime,@LastEditUserId,@LastEditUserName,@ProductionSiteId,@ProductionSiteName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("HourlyRate", item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteId", item.ProductionSiteId == null ? (object)DBNull.Value : item.ProductionSiteId);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> items)
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
						query += " INSERT INTO [__BSD_HourlyRate] ([CreationTime],[CreationUserId],[CreationUserName],[HourlyRate],[LastEditTime],[LastEditUserId],[LastEditUserName],[ProductionSiteId],[ProductionSiteName]) VALUES ( "

							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CreationUserName" + i + ","
							+ "@HourlyRate" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@LastEditUserName" + i + ","
							+ "@ProductionSiteId" + i + ","
							+ "@ProductionSiteName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("HourlyRate" + i, item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
						sqlCommand.Parameters.AddWithValue("ProductionSiteId" + i, item.ProductionSiteId == null ? (object)DBNull.Value : item.ProductionSiteId);
						sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_HourlyRate] SET [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [HourlyRate]=@HourlyRate, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [LastEditUserName]=@LastEditUserName, [ProductionSiteId]=@ProductionSiteId, [ProductionSiteName]=@ProductionSiteName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
				sqlCommand.Parameters.AddWithValue("HourlyRate", item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
				sqlCommand.Parameters.AddWithValue("ProductionSiteId", item.ProductionSiteId == null ? (object)DBNull.Value : item.ProductionSiteId);
				sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> items)
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
						query += " UPDATE [__BSD_HourlyRate] SET "

							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CreationUserName]=@CreationUserName" + i + ","
							+ "[HourlyRate]=@HourlyRate" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[LastEditUserName]=@LastEditUserName" + i + ","
							+ "[ProductionSiteId]=@ProductionSiteId" + i + ","
							+ "[ProductionSiteName]=@ProductionSiteName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("HourlyRate" + i, item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
						sqlCommand.Parameters.AddWithValue("ProductionSiteId" + i, item.ProductionSiteId == null ? (object)DBNull.Value : item.ProductionSiteId);
						sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
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
				string query = "DELETE FROM [__BSD_HourlyRate] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_HourlyRate] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_HourlyRate] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_HourlyRate]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_HourlyRate] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__BSD_HourlyRate] ([CreationTime],[CreationUserId],[CreationUserName],[HourlyRate],[LastEditTime],[LastEditUserId],[LastEditUserName],[ProductionSiteId],[ProductionSiteName]) OUTPUT INSERTED.[Id] VALUES (@CreationTime,@CreationUserId,@CreationUserName,@HourlyRate,@LastEditTime,@LastEditUserId,@LastEditUserName,@ProductionSiteId,@ProductionSiteName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
			sqlCommand.Parameters.AddWithValue("HourlyRate", item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
			sqlCommand.Parameters.AddWithValue("ProductionSiteId", item.ProductionSiteId == null ? (object)DBNull.Value : item.ProductionSiteId);
			sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_HourlyRate] ([CreationTime],[CreationUserId],[CreationUserName],[HourlyRate],[LastEditTime],[LastEditUserId],[LastEditUserName],[ProductionSiteId],[ProductionSiteName]) VALUES ( "

						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CreationUserName" + i + ","
						+ "@HourlyRate" + i + ","
						+ "@LastEditTime" + i + ","
						+ "@LastEditUserId" + i + ","
						+ "@LastEditUserName" + i + ","
						+ "@ProductionSiteId" + i + ","
						+ "@ProductionSiteName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("HourlyRate" + i, item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteId" + i, item.ProductionSiteId == null ? (object)DBNull.Value : item.ProductionSiteId);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_HourlyRate] SET [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [HourlyRate]=@HourlyRate, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [LastEditUserName]=@LastEditUserName, [ProductionSiteId]=@ProductionSiteId, [ProductionSiteName]=@ProductionSiteName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
			sqlCommand.Parameters.AddWithValue("HourlyRate", item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
			sqlCommand.Parameters.AddWithValue("ProductionSiteId", item.ProductionSiteId == null ? (object)DBNull.Value : item.ProductionSiteId);
			sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_HourlyRate] SET "

					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CreationUserName]=@CreationUserName" + i + ","
					+ "[HourlyRate]=@HourlyRate" + i + ","
					+ "[LastEditTime]=@LastEditTime" + i + ","
					+ "[LastEditUserId]=@LastEditUserId" + i + ","
					+ "[LastEditUserName]=@LastEditUserName" + i + ","
					+ "[ProductionSiteId]=@ProductionSiteId" + i + ","
					+ "[ProductionSiteName]=@ProductionSiteName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("HourlyRate" + i, item.HourlyRate == null ? (object)DBNull.Value : item.HourlyRate);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteId" + i, item.ProductionSiteId == null ? (object)DBNull.Value : item.ProductionSiteId);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [__BSD_HourlyRate] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			return DbExecution.ExecuteNonQuery(sqlCommand);
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

				string query = "DELETE FROM [__BSD_HourlyRate] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> GetByProdutionSiteId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_HourlyRate] WHERE [ProductionSiteId]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity> GetByProdutionSiteId(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__BSD_HourlyRate] WHERE [ProductionSiteId]=@id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("id", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity>();
			}
		}
		#endregion Custom Methods

	}
}
