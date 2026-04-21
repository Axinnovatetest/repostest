using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class StucklistenArticleExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticleExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticleExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_StucklistenArticleExtension] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_StucklistenArticleExtension] ([ArticleDesignation],[ArticleId],[ArticleNumber],[BomStatus],[BomStatusId],[BomValidFrom],[BomVersion],[LastUpdateTime],[LastUpdateUserId]) OUTPUT INSERTED.[Id] VALUES (@ArticleDesignation,@ArticleId,@ArticleNumber,@BomStatus,@BomStatusId,@BomValidFrom,@BomVersion,@LastUpdateTime,@LastUpdateUserId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleDesignation", item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("BomStatus", item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
					sqlCommand.Parameters.AddWithValue("BomStatusId", item.BomStatusId == null ? (object)DBNull.Value : item.BomStatusId);
					sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
					sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> items)
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
						query += " INSERT INTO [__BSD_StucklistenArticleExtension] ([ArticleDesignation],[ArticleId],[ArticleNumber],[BomStatus],[BomStatusId],[BomValidFrom],[BomVersion],[LastUpdateTime],[LastUpdateUserId]) VALUES ( "

							+ "@ArticleDesignation" + i + ","
							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@BomStatus" + i + ","
							+ "@BomStatusId" + i + ","
							+ "@BomValidFrom" + i + ","
							+ "@BomVersion" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleDesignation" + i, item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("BomStatus" + i, item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
						sqlCommand.Parameters.AddWithValue("BomStatusId" + i, item.BomStatusId == null ? (object)DBNull.Value : item.BomStatusId);
						sqlCommand.Parameters.AddWithValue("BomValidFrom" + i, item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_StucklistenArticleExtension] SET [ArticleDesignation]=@ArticleDesignation, [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [BomStatus]=@BomStatus, [BomStatusId]=@BomStatusId, [BomValidFrom]=@BomValidFrom, [BomVersion]=@BomVersion, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleDesignation", item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("BomStatus", item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
				sqlCommand.Parameters.AddWithValue("BomStatusId", item.BomStatusId == null ? (object)DBNull.Value : item.BomStatusId);
				sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
				sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> items)
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
						query += " UPDATE [__BSD_StucklistenArticleExtension] SET "

							+ "[ArticleDesignation]=@ArticleDesignation" + i + ","
							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[BomStatus]=@BomStatus" + i + ","
							+ "[BomStatusId]=@BomStatusId" + i + ","
							+ "[BomValidFrom]=@BomValidFrom" + i + ","
							+ "[BomVersion]=@BomVersion" + i + ","
							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUserId]=@LastUpdateUserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleDesignation" + i, item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("BomStatus" + i, item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
						sqlCommand.Parameters.AddWithValue("BomStatusId" + i, item.BomStatusId == null ? (object)DBNull.Value : item.BomStatusId);
						sqlCommand.Parameters.AddWithValue("BomValidFrom" + i, item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
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
				string query = "DELETE FROM [__BSD_StucklistenArticleExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_StucklistenArticleExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenArticleExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenArticleExtension]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_StucklistenArticleExtension] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__BSD_StucklistenArticleExtension] ([ArticleDesignation],[ArticleId],[ArticleNumber],[BomStatus],[BomStatusId],[BomValidFrom],[BomVersion],[LastUpdateTime],[LastUpdateUserId]) OUTPUT INSERTED.[Id] VALUES (@ArticleDesignation,@ArticleId,@ArticleNumber,@BomStatus,@BomStatusId,@BomValidFrom,@BomVersion,@LastUpdateTime,@LastUpdateUserId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleDesignation", item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("BomStatus", item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
			sqlCommand.Parameters.AddWithValue("BomStatusId", item.BomStatusId == null ? (object)DBNull.Value : item.BomStatusId);
			sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_StucklistenArticleExtension] ([ArticleDesignation],[ArticleId],[ArticleNumber],[BomStatus],[BomStatusId],[BomValidFrom],[BomVersion],[LastUpdateTime],[LastUpdateUserId]) VALUES ( "

						+ "@ArticleDesignation" + i + ","
						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@BomStatus" + i + ","
						+ "@BomStatusId" + i + ","
						+ "@BomValidFrom" + i + ","
						+ "@BomVersion" + i + ","
						+ "@LastUpdateTime" + i + ","
						+ "@LastUpdateUserId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleDesignation" + i, item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("BomStatus" + i, item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
					sqlCommand.Parameters.AddWithValue("BomStatusId" + i, item.BomStatusId == null ? (object)DBNull.Value : item.BomStatusId);
					sqlCommand.Parameters.AddWithValue("BomValidFrom" + i, item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_StucklistenArticleExtension] SET [ArticleDesignation]=@ArticleDesignation, [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [BomStatus]=@BomStatus, [BomStatusId]=@BomStatusId, [BomValidFrom]=@BomValidFrom, [BomVersion]=@BomVersion, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleDesignation", item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("BomStatus", item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
			sqlCommand.Parameters.AddWithValue("BomStatusId", item.BomStatusId == null ? (object)DBNull.Value : item.BomStatusId);
			sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_StucklistenArticleExtension] SET "

					+ "[ArticleDesignation]=@ArticleDesignation" + i + ","
					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[BomStatus]=@BomStatus" + i + ","
					+ "[BomStatusId]=@BomStatusId" + i + ","
					+ "[BomValidFrom]=@BomValidFrom" + i + ","
					+ "[BomVersion]=@BomVersion" + i + ","
					+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
					+ "[LastUpdateUserId]=@LastUpdateUserId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleDesignation" + i, item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("BomStatus" + i, item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
					sqlCommand.Parameters.AddWithValue("BomStatusId" + i, item.BomStatusId == null ? (object)DBNull.Value : item.BomStatusId);
					sqlCommand.Parameters.AddWithValue("BomValidFrom" + i, item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_StucklistenArticleExtension] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__BSD_StucklistenArticleExtension] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int UpdateStatus(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_StucklistenArticleExtension] SET [BomStatus]=@BomStatus, [BomValidFrom]=@BomValidFrom, [BomVersion]=@BomVersion, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("BomStatus", item.BomStatus == null ? (object)DBNull.Value : item.BomStatus);
				sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
				sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity GetByArticle(int articleId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			{
				string query = "SELECT * FROM [__BSD_StucklistenArticleExtension] WHERE [ArticleId]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity GetByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticleExtension] WHERE [ArticleId]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity> GetByArticles(List<int> articleIds)
		{
			if(articleIds == null || articleIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_StucklistenArticleExtension] WHERE [ArticleId] IN ({string.Join(",", articleIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity GetByArticleAndBomVersion(int articleId, int bomVersion)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticleExtension] WHERE [ArticleId]=@articleId AND [BomVersion]=@bomVersion";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int DeleteByArticleWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_StucklistenArticleExtension] WHERE [ArticleId]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


			return results;
		}
		#endregion
	}
}
