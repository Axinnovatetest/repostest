using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class StucklistenArticleExtension_SnapshotAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticleExtension_Snapshot]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_StucklistenArticleExtension_Snapshot] ([ArticleDesignation],[ArticleId],[ArticleNumber],[BomValidFrom],[BomVersion],[BrcId],[KundenIndex],[KundenIndexDate],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[SnapshotTime],[SnapshotUserId]) OUTPUT INSERTED.[Id] VALUES (@ArticleDesignation,@ArticleId,@ArticleNumber,@BomValidFrom,@BomVersion,@BrcId,@KundenIndex,@KundenIndexDate,@Overwritten,@OverwrittenTime,@OverwrittenUserId,@SnapshotTime,@SnapshotUserId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleDesignation", item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
					sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("BrcId", item.BrcId == null ? (object)DBNull.Value : item.BrcId);
					sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("KundenIndexDate", item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
					sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> items)
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
						query += " INSERT INTO [__BSD_StucklistenArticleExtension_Snapshot] ([ArticleDesignation],[ArticleId],[ArticleNumber],[BomValidFrom],[BomVersion],[BrcId],[KundenIndex],[KundenIndexDate],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[SnapshotTime],[SnapshotUserId]) VALUES ( "

							+ "@ArticleDesignation" + i + ","
							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@BomValidFrom" + i + ","
							+ "@BomVersion" + i + ","
							+ "@BrcId" + i + ","
							+ "@KundenIndex" + i + ","
							+ "@KundenIndexDate" + i + ","
							+ "@Overwritten" + i + ","
							+ "@OverwrittenTime" + i + ","
							+ "@OverwrittenUserId" + i + ","
							+ "@SnapshotTime" + i + ","
							+ "@SnapshotUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleDesignation" + i, item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("BomValidFrom" + i, item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
						sqlCommand.Parameters.AddWithValue("BrcId" + i, item.BrcId == null ? (object)DBNull.Value : item.BrcId);
						sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
						sqlCommand.Parameters.AddWithValue("KundenIndexDate" + i, item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
						sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
						sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
						sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
						sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
						sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_StucklistenArticleExtension_Snapshot] SET [ArticleDesignation]=@ArticleDesignation, [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [BomValidFrom]=@BomValidFrom, [BomVersion]=@BomVersion, [BrcId]=@BrcId, [KundenIndex]=@KundenIndex, [KundenIndexDate]=@KundenIndexDate, [Overwritten]=@Overwritten, [OverwrittenTime]=@OverwrittenTime, [OverwrittenUserId]=@OverwrittenUserId, [SnapshotTime]=@SnapshotTime, [SnapshotUserId]=@SnapshotUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleDesignation", item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
				sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
				sqlCommand.Parameters.AddWithValue("BrcId", item.BrcId == null ? (object)DBNull.Value : item.BrcId);
				sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
				sqlCommand.Parameters.AddWithValue("KundenIndexDate", item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
				sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
				sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
				sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
				sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
				sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> items)
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
						query += " UPDATE [__BSD_StucklistenArticleExtension_Snapshot] SET "

							+ "[ArticleDesignation]=@ArticleDesignation" + i + ","
							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[BomValidFrom]=@BomValidFrom" + i + ","
							+ "[BomVersion]=@BomVersion" + i + ","
							+ "[BrcId]=@BrcId" + i + ","
							+ "[KundenIndex]=@KundenIndex" + i + ","
							+ "[KundenIndexDate]=@KundenIndexDate" + i + ","
							+ "[Overwritten]=@Overwritten" + i + ","
							+ "[OverwrittenTime]=@OverwrittenTime" + i + ","
							+ "[OverwrittenUserId]=@OverwrittenUserId" + i + ","
							+ "[SnapshotTime]=@SnapshotTime" + i + ","
							+ "[SnapshotUserId]=@SnapshotUserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleDesignation" + i, item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("BomValidFrom" + i, item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
						sqlCommand.Parameters.AddWithValue("BrcId" + i, item.BrcId == null ? (object)DBNull.Value : item.BrcId);
						sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
						sqlCommand.Parameters.AddWithValue("KundenIndexDate" + i, item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
						sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
						sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
						sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
						sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
						sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
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
				string query = "DELETE FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenArticleExtension_Snapshot]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__BSD_StucklistenArticleExtension_Snapshot] ([ArticleDesignation],[ArticleId],[ArticleNumber],[BomValidFrom],[BomVersion],[BrcId],[KundenIndex],[KundenIndexDate],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[SnapshotTime],[SnapshotUserId]) OUTPUT INSERTED.[Id] VALUES (@ArticleDesignation,@ArticleId,@ArticleNumber,@BomValidFrom,@BomVersion,@BrcId,@KundenIndex,@KundenIndexDate,@Overwritten,@OverwrittenTime,@OverwrittenUserId,@SnapshotTime,@SnapshotUserId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleDesignation", item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
			sqlCommand.Parameters.AddWithValue("BrcId", item.BrcId == null ? (object)DBNull.Value : item.BrcId);
			sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
			sqlCommand.Parameters.AddWithValue("KundenIndexDate", item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
			sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
			sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
			sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
			sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_StucklistenArticleExtension_Snapshot] ([ArticleDesignation],[ArticleId],[ArticleNumber],[BomValidFrom],[BomVersion],[BrcId],[KundenIndex],[KundenIndexDate],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[SnapshotTime],[SnapshotUserId]) VALUES ( "

						+ "@ArticleDesignation" + i + ","
						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@BomValidFrom" + i + ","
						+ "@BomVersion" + i + ","
						+ "@BrcId" + i + ","
						+ "@KundenIndex" + i + ","
						+ "@KundenIndexDate" + i + ","
						+ "@Overwritten" + i + ","
						+ "@OverwrittenTime" + i + ","
						+ "@OverwrittenUserId" + i + ","
						+ "@SnapshotTime" + i + ","
						+ "@SnapshotUserId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleDesignation" + i, item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("BomValidFrom" + i, item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("BrcId" + i, item.BrcId == null ? (object)DBNull.Value : item.BrcId);
					sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("KundenIndexDate" + i, item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
					sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_StucklistenArticleExtension_Snapshot] SET [ArticleDesignation]=@ArticleDesignation, [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [BomValidFrom]=@BomValidFrom, [BomVersion]=@BomVersion, [BrcId]=@BrcId, [KundenIndex]=@KundenIndex, [KundenIndexDate]=@KundenIndexDate, [Overwritten]=@Overwritten, [OverwrittenTime]=@OverwrittenTime, [OverwrittenUserId]=@OverwrittenUserId, [SnapshotTime]=@SnapshotTime, [SnapshotUserId]=@SnapshotUserId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleDesignation", item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("BomValidFrom", item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
			sqlCommand.Parameters.AddWithValue("BrcId", item.BrcId == null ? (object)DBNull.Value : item.BrcId);
			sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
			sqlCommand.Parameters.AddWithValue("KundenIndexDate", item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
			sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
			sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
			sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
			sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 14; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_StucklistenArticleExtension_Snapshot] SET "

					+ "[ArticleDesignation]=@ArticleDesignation" + i + ","
					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[BomValidFrom]=@BomValidFrom" + i + ","
					+ "[BomVersion]=@BomVersion" + i + ","
					+ "[BrcId]=@BrcId" + i + ","
					+ "[KundenIndex]=@KundenIndex" + i + ","
					+ "[KundenIndexDate]=@KundenIndexDate" + i + ","
					+ "[Overwritten]=@Overwritten" + i + ","
					+ "[OverwrittenTime]=@OverwrittenTime" + i + ","
					+ "[OverwrittenUserId]=@OverwrittenUserId" + i + ","
					+ "[SnapshotTime]=@SnapshotTime" + i + ","
					+ "[SnapshotUserId]=@SnapshotUserId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleDesignation" + i, item.ArticleDesignation == null ? (object)DBNull.Value : item.ArticleDesignation);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("BomValidFrom" + i, item.BomValidFrom == null ? (object)DBNull.Value : item.BomValidFrom);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("BrcId" + i, item.BrcId == null ? (object)DBNull.Value : item.BrcId);
					sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("KundenIndexDate" + i, item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
					sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity> GetByArticleId(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticleExtension_Snapshot] WHERE [ArticleId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity>();
			}
		}
		public static int OverwriteSnapshotWithTransaction(int articleId, int bomVersion, int userId, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_StucklistenArticleExtension_Snapshot] SET [ArticleId]=-1, [Overwritten]=1, [OverwrittenTime]=GETDATE(), [OverwrittenUserId]=@OverwrittenUserId WHERE [ArticleId]=@ArtikelNr AND [BomVersion]=@BomVersion";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ArtikelNr", articleId);
			sqlCommand.Parameters.AddWithValue("BomVersion", bomVersion);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", userId);


			results = sqlCommand.ExecuteNonQuery();
			return results;
		}

		public static int OverwriteSnapshotWBcrWithTransaction(int articleId, int bomVersion, int userId, int? bcrId, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_StucklistenArticleExtension_Snapshot] SET [ArticleId]=-1, [Overwritten]=1, [OverwrittenTime]=GETDATE(), [OverwrittenUserId]=@OverwrittenUserId,[BrcId]=@BcrId WHERE [ArticleId]=@ArtikelNr AND [BomVersion]=@BomVersion";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ArtikelNr", articleId);
			sqlCommand.Parameters.AddWithValue("BomVersion", bomVersion);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", userId);
			sqlCommand.Parameters.AddWithValue("BcrId", bcrId);


			results = sqlCommand.ExecuteNonQuery();
			return results;
		}

		#endregion
	}
}
