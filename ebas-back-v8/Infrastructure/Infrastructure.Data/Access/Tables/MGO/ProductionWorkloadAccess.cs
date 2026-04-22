using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.MGO
{
    public class ProductionWorkloadAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity Get(int id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [__MGO_ProductionWorkload] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id); 

                DbExecution.Fill(sqlCommand, dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [__MGO_ProductionWorkload]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                DbExecution.Fill(sqlCommand, dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> get(List<int> ids)
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
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    sqlCommand.CommandText = $"SELECT * FROM [__MGO_ProductionWorkload] WHERE [Id] IN ({queryIds})";                    
                DbExecution.Fill(sqlCommand, dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();}

		public static int Insert(Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MGO_ProductionWorkload] ([FaArticleCount],[FaCount],[FaTime],[FaTotalQuantity],[FaWeek],[FaYear],[RecordSyncId],[RecordSyncTime],[RecordSyncUserId],[WarehouseId],[WarehouseMaxCapacity]) OUTPUT INSERTED.[Id] VALUES (@FaArticleCount,@FaCount,@FaTime,@FaTotalQuantity,@FaWeek,@FaYear,@RecordSyncId,@RecordSyncTime,@RecordSyncUserId,@WarehouseId,@WarehouseMaxCapacity); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("FaArticleCount", item.FaArticleCount == null ? (object)DBNull.Value : item.FaArticleCount);
					sqlCommand.Parameters.AddWithValue("FaCount", item.FaCount == null ? (object)DBNull.Value : item.FaCount);
					sqlCommand.Parameters.AddWithValue("FaTime", item.FaTime == null ? (object)DBNull.Value : item.FaTime);
					sqlCommand.Parameters.AddWithValue("FaTotalQuantity", item.FaTotalQuantity == null ? (object)DBNull.Value : item.FaTotalQuantity);
					sqlCommand.Parameters.AddWithValue("FaWeek", item.FaWeek == null ? (object)DBNull.Value : item.FaWeek);
					sqlCommand.Parameters.AddWithValue("FaYear", item.FaYear == null ? (object)DBNull.Value : item.FaYear);
					sqlCommand.Parameters.AddWithValue("RecordSyncId", item.RecordSyncId == null ? (object)DBNull.Value : item.RecordSyncId);
					sqlCommand.Parameters.AddWithValue("RecordSyncTime", item.RecordSyncTime == null ? (object)DBNull.Value : item.RecordSyncTime);
					sqlCommand.Parameters.AddWithValue("RecordSyncUserId", item.RecordSyncUserId == null ? (object)DBNull.Value : item.RecordSyncUserId);
					sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseMaxCapacity", item.WarehouseMaxCapacity == null ? (object)DBNull.Value : item.WarehouseMaxCapacity);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> items)
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
						query += " INSERT INTO [__MGO_ProductionWorkload] ([FaArticleCount],[FaCount],[FaTime],[FaTotalQuantity],[FaWeek],[FaYear],[RecordSyncId],[RecordSyncTime],[RecordSyncUserId],[WarehouseId],[WarehouseMaxCapacity]) VALUES ( "

							+ "@FaArticleCount" + i + ","
							+ "@FaCount" + i + ","
							+ "@FaTime" + i + ","
							+ "@FaTotalQuantity" + i + ","
							+ "@FaWeek" + i + ","
							+ "@FaYear" + i + ","
							+ "@RecordSyncId" + i + ","
							+ "@RecordSyncTime" + i + ","
							+ "@RecordSyncUserId" + i + ","
							+ "@WarehouseId" + i + ","
							+ "@WarehouseMaxCapacity" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("FaArticleCount" + i, item.FaArticleCount == null ? (object)DBNull.Value : item.FaArticleCount);
						sqlCommand.Parameters.AddWithValue("FaCount" + i, item.FaCount == null ? (object)DBNull.Value : item.FaCount);
						sqlCommand.Parameters.AddWithValue("FaTime" + i, item.FaTime == null ? (object)DBNull.Value : item.FaTime);
						sqlCommand.Parameters.AddWithValue("FaTotalQuantity" + i, item.FaTotalQuantity == null ? (object)DBNull.Value : item.FaTotalQuantity);
						sqlCommand.Parameters.AddWithValue("FaWeek" + i, item.FaWeek == null ? (object)DBNull.Value : item.FaWeek);
						sqlCommand.Parameters.AddWithValue("FaYear" + i, item.FaYear == null ? (object)DBNull.Value : item.FaYear);
						sqlCommand.Parameters.AddWithValue("RecordSyncId" + i, item.RecordSyncId == null ? (object)DBNull.Value : item.RecordSyncId);
						sqlCommand.Parameters.AddWithValue("RecordSyncTime" + i, item.RecordSyncTime == null ? (object)DBNull.Value : item.RecordSyncTime);
						sqlCommand.Parameters.AddWithValue("RecordSyncUserId" + i, item.RecordSyncUserId == null ? (object)DBNull.Value : item.RecordSyncUserId);
						sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
						sqlCommand.Parameters.AddWithValue("WarehouseMaxCapacity" + i, item.WarehouseMaxCapacity == null ? (object)DBNull.Value : item.WarehouseMaxCapacity);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MGO_ProductionWorkload] SET [FaArticleCount]=@FaArticleCount, [FaCount]=@FaCount, [FaTime]=@FaTime, [FaTotalQuantity]=@FaTotalQuantity, [FaWeek]=@FaWeek, [FaYear]=@FaYear, [RecordSyncId]=@RecordSyncId, [RecordSyncTime]=@RecordSyncTime, [RecordSyncUserId]=@RecordSyncUserId, [WarehouseId]=@WarehouseId, [WarehouseMaxCapacity]=@WarehouseMaxCapacity WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("FaArticleCount", item.FaArticleCount == null ? (object)DBNull.Value : item.FaArticleCount);
				sqlCommand.Parameters.AddWithValue("FaCount", item.FaCount == null ? (object)DBNull.Value : item.FaCount);
				sqlCommand.Parameters.AddWithValue("FaTime", item.FaTime == null ? (object)DBNull.Value : item.FaTime);
				sqlCommand.Parameters.AddWithValue("FaTotalQuantity", item.FaTotalQuantity == null ? (object)DBNull.Value : item.FaTotalQuantity);
				sqlCommand.Parameters.AddWithValue("FaWeek", item.FaWeek == null ? (object)DBNull.Value : item.FaWeek);
				sqlCommand.Parameters.AddWithValue("FaYear", item.FaYear == null ? (object)DBNull.Value : item.FaYear);
				sqlCommand.Parameters.AddWithValue("RecordSyncId", item.RecordSyncId == null ? (object)DBNull.Value : item.RecordSyncId);
				sqlCommand.Parameters.AddWithValue("RecordSyncTime", item.RecordSyncTime == null ? (object)DBNull.Value : item.RecordSyncTime);
				sqlCommand.Parameters.AddWithValue("RecordSyncUserId", item.RecordSyncUserId == null ? (object)DBNull.Value : item.RecordSyncUserId);
				sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
				sqlCommand.Parameters.AddWithValue("WarehouseMaxCapacity", item.WarehouseMaxCapacity == null ? (object)DBNull.Value : item.WarehouseMaxCapacity);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> items)
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
						query += " UPDATE [__MGO_ProductionWorkload] SET "

							+ "[FaArticleCount]=@FaArticleCount" + i + ","
							+ "[FaCount]=@FaCount" + i + ","
							+ "[FaTime]=@FaTime" + i + ","
							+ "[FaTotalQuantity]=@FaTotalQuantity" + i + ","
							+ "[FaWeek]=@FaWeek" + i + ","
							+ "[FaYear]=@FaYear" + i + ","
							+ "[RecordSyncId]=@RecordSyncId" + i + ","
							+ "[RecordSyncTime]=@RecordSyncTime" + i + ","
							+ "[RecordSyncUserId]=@RecordSyncUserId" + i + ","
							+ "[WarehouseId]=@WarehouseId" + i + ","
							+ "[WarehouseMaxCapacity]=@WarehouseMaxCapacity" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("FaArticleCount" + i, item.FaArticleCount == null ? (object)DBNull.Value : item.FaArticleCount);
						sqlCommand.Parameters.AddWithValue("FaCount" + i, item.FaCount == null ? (object)DBNull.Value : item.FaCount);
						sqlCommand.Parameters.AddWithValue("FaTime" + i, item.FaTime == null ? (object)DBNull.Value : item.FaTime);
						sqlCommand.Parameters.AddWithValue("FaTotalQuantity" + i, item.FaTotalQuantity == null ? (object)DBNull.Value : item.FaTotalQuantity);
						sqlCommand.Parameters.AddWithValue("FaWeek" + i, item.FaWeek == null ? (object)DBNull.Value : item.FaWeek);
						sqlCommand.Parameters.AddWithValue("FaYear" + i, item.FaYear == null ? (object)DBNull.Value : item.FaYear);
						sqlCommand.Parameters.AddWithValue("RecordSyncId" + i, item.RecordSyncId == null ? (object)DBNull.Value : item.RecordSyncId);
						sqlCommand.Parameters.AddWithValue("RecordSyncTime" + i, item.RecordSyncTime == null ? (object)DBNull.Value : item.RecordSyncTime);
						sqlCommand.Parameters.AddWithValue("RecordSyncUserId" + i, item.RecordSyncUserId == null ? (object)DBNull.Value : item.RecordSyncUserId);
						sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
						sqlCommand.Parameters.AddWithValue("WarehouseMaxCapacity" + i, item.WarehouseMaxCapacity == null ? (object)DBNull.Value : item.WarehouseMaxCapacity);
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
                string query = "DELETE FROM [__MGO_ProductionWorkload] WHERE [Id]=@Id";
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
                int results=0;
                if(ids.Count <= maxParamsNumber)
                {
                    results = delete(ids);
                } else
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
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    string query = "DELETE FROM [__MGO_ProductionWorkload] WHERE [Id] IN ("+ queryIds +")";                    
                    sqlCommand.CommandText = query;
                        
                    results = DbExecution.ExecuteNonQuery(sqlCommand);
                }

                return results;
            }
            return -1;
        }

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__MGO_ProductionWorkload] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__MGO_ProductionWorkload]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__MGO_ProductionWorkload] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__MGO_ProductionWorkload] ([FaArticleCount],[FaCount],[FaTime],[FaTotalQuantity],[FaWeek],[FaYear],[RecordSyncId],[RecordSyncTime],[RecordSyncUserId],[WarehouseId],[WarehouseMaxCapacity]) OUTPUT INSERTED.[Id] VALUES (@FaArticleCount,@FaCount,@FaTime,@FaTotalQuantity,@FaWeek,@FaYear,@RecordSyncId,@RecordSyncTime,@RecordSyncUserId,@WarehouseId,@WarehouseMaxCapacity); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("FaArticleCount", item.FaArticleCount == null ? (object)DBNull.Value : item.FaArticleCount);
			sqlCommand.Parameters.AddWithValue("FaCount", item.FaCount == null ? (object)DBNull.Value : item.FaCount);
			sqlCommand.Parameters.AddWithValue("FaTime", item.FaTime == null ? (object)DBNull.Value : item.FaTime);
			sqlCommand.Parameters.AddWithValue("FaTotalQuantity", item.FaTotalQuantity == null ? (object)DBNull.Value : item.FaTotalQuantity);
			sqlCommand.Parameters.AddWithValue("FaWeek", item.FaWeek == null ? (object)DBNull.Value : item.FaWeek);
			sqlCommand.Parameters.AddWithValue("FaYear", item.FaYear == null ? (object)DBNull.Value : item.FaYear);
			sqlCommand.Parameters.AddWithValue("RecordSyncId", item.RecordSyncId == null ? (object)DBNull.Value : item.RecordSyncId);
			sqlCommand.Parameters.AddWithValue("RecordSyncTime", item.RecordSyncTime == null ? (object)DBNull.Value : item.RecordSyncTime);
			sqlCommand.Parameters.AddWithValue("RecordSyncUserId", item.RecordSyncUserId == null ? (object)DBNull.Value : item.RecordSyncUserId);
			sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
			sqlCommand.Parameters.AddWithValue("WarehouseMaxCapacity", item.WarehouseMaxCapacity == null ? (object)DBNull.Value : item.WarehouseMaxCapacity);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__MGO_ProductionWorkload] ([FaArticleCount],[FaCount],[FaTime],[FaTotalQuantity],[FaWeek],[FaYear],[RecordSyncId],[RecordSyncTime],[RecordSyncUserId],[WarehouseId],[WarehouseMaxCapacity]) VALUES ( "

						+ "@FaArticleCount" + i + ","
						+ "@FaCount" + i + ","
						+ "@FaTime" + i + ","
						+ "@FaTotalQuantity" + i + ","
						+ "@FaWeek" + i + ","
						+ "@FaYear" + i + ","
						+ "@RecordSyncId" + i + ","
						+ "@RecordSyncTime" + i + ","
						+ "@RecordSyncUserId" + i + ","
						+ "@WarehouseId" + i + ","
						+ "@WarehouseMaxCapacity" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("FaArticleCount" + i, item.FaArticleCount == null ? (object)DBNull.Value : item.FaArticleCount);
					sqlCommand.Parameters.AddWithValue("FaCount" + i, item.FaCount == null ? (object)DBNull.Value : item.FaCount);
					sqlCommand.Parameters.AddWithValue("FaTime" + i, item.FaTime == null ? (object)DBNull.Value : item.FaTime);
					sqlCommand.Parameters.AddWithValue("FaTotalQuantity" + i, item.FaTotalQuantity == null ? (object)DBNull.Value : item.FaTotalQuantity);
					sqlCommand.Parameters.AddWithValue("FaWeek" + i, item.FaWeek == null ? (object)DBNull.Value : item.FaWeek);
					sqlCommand.Parameters.AddWithValue("FaYear" + i, item.FaYear == null ? (object)DBNull.Value : item.FaYear);
					sqlCommand.Parameters.AddWithValue("RecordSyncId" + i, item.RecordSyncId == null ? (object)DBNull.Value : item.RecordSyncId);
					sqlCommand.Parameters.AddWithValue("RecordSyncTime" + i, item.RecordSyncTime == null ? (object)DBNull.Value : item.RecordSyncTime);
					sqlCommand.Parameters.AddWithValue("RecordSyncUserId" + i, item.RecordSyncUserId == null ? (object)DBNull.Value : item.RecordSyncUserId);
					sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseMaxCapacity" + i, item.WarehouseMaxCapacity == null ? (object)DBNull.Value : item.WarehouseMaxCapacity);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__MGO_ProductionWorkload] SET [FaArticleCount]=@FaArticleCount, [FaCount]=@FaCount, [FaTime]=@FaTime, [FaTotalQuantity]=@FaTotalQuantity, [FaWeek]=@FaWeek, [FaYear]=@FaYear, [RecordSyncId]=@RecordSyncId, [RecordSyncTime]=@RecordSyncTime, [RecordSyncUserId]=@RecordSyncUserId, [WarehouseId]=@WarehouseId, [WarehouseMaxCapacity]=@WarehouseMaxCapacity WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("FaArticleCount", item.FaArticleCount == null ? (object)DBNull.Value : item.FaArticleCount);
			sqlCommand.Parameters.AddWithValue("FaCount", item.FaCount == null ? (object)DBNull.Value : item.FaCount);
			sqlCommand.Parameters.AddWithValue("FaTime", item.FaTime == null ? (object)DBNull.Value : item.FaTime);
			sqlCommand.Parameters.AddWithValue("FaTotalQuantity", item.FaTotalQuantity == null ? (object)DBNull.Value : item.FaTotalQuantity);
			sqlCommand.Parameters.AddWithValue("FaWeek", item.FaWeek == null ? (object)DBNull.Value : item.FaWeek);
			sqlCommand.Parameters.AddWithValue("FaYear", item.FaYear == null ? (object)DBNull.Value : item.FaYear);
			sqlCommand.Parameters.AddWithValue("RecordSyncId", item.RecordSyncId == null ? (object)DBNull.Value : item.RecordSyncId);
			sqlCommand.Parameters.AddWithValue("RecordSyncTime", item.RecordSyncTime == null ? (object)DBNull.Value : item.RecordSyncTime);
			sqlCommand.Parameters.AddWithValue("RecordSyncUserId", item.RecordSyncUserId == null ? (object)DBNull.Value : item.RecordSyncUserId);
			sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
			sqlCommand.Parameters.AddWithValue("WarehouseMaxCapacity", item.WarehouseMaxCapacity == null ? (object)DBNull.Value : item.WarehouseMaxCapacity);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__MGO_ProductionWorkload] SET "

					+ "[FaArticleCount]=@FaArticleCount" + i + ","
					+ "[FaCount]=@FaCount" + i + ","
					+ "[FaTime]=@FaTime" + i + ","
					+ "[FaTotalQuantity]=@FaTotalQuantity" + i + ","
					+ "[FaWeek]=@FaWeek" + i + ","
					+ "[FaYear]=@FaYear" + i + ","
					+ "[RecordSyncId]=@RecordSyncId" + i + ","
					+ "[RecordSyncTime]=@RecordSyncTime" + i + ","
					+ "[RecordSyncUserId]=@RecordSyncUserId" + i + ","
					+ "[WarehouseId]=@WarehouseId" + i + ","
					+ "[WarehouseMaxCapacity]=@WarehouseMaxCapacity" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("FaArticleCount" + i, item.FaArticleCount == null ? (object)DBNull.Value : item.FaArticleCount);
					sqlCommand.Parameters.AddWithValue("FaCount" + i, item.FaCount == null ? (object)DBNull.Value : item.FaCount);
					sqlCommand.Parameters.AddWithValue("FaTime" + i, item.FaTime == null ? (object)DBNull.Value : item.FaTime);
					sqlCommand.Parameters.AddWithValue("FaTotalQuantity" + i, item.FaTotalQuantity == null ? (object)DBNull.Value : item.FaTotalQuantity);
					sqlCommand.Parameters.AddWithValue("FaWeek" + i, item.FaWeek == null ? (object)DBNull.Value : item.FaWeek);
					sqlCommand.Parameters.AddWithValue("FaYear" + i, item.FaYear == null ? (object)DBNull.Value : item.FaYear);
					sqlCommand.Parameters.AddWithValue("RecordSyncId" + i, item.RecordSyncId == null ? (object)DBNull.Value : item.RecordSyncId);
					sqlCommand.Parameters.AddWithValue("RecordSyncTime" + i, item.RecordSyncTime == null ? (object)DBNull.Value : item.RecordSyncTime);
					sqlCommand.Parameters.AddWithValue("RecordSyncUserId" + i, item.RecordSyncUserId == null ? (object)DBNull.Value : item.RecordSyncUserId);
					sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseMaxCapacity" + i, item.WarehouseMaxCapacity == null ? (object)DBNull.Value : item.WarehouseMaxCapacity);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__MGO_ProductionWorkload] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__MGO_ProductionWorkload] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int RefreshWorkload(int userdId, int warehouseId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "[usp_RefreshProductionWorkload]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.Parameters.AddWithValue("userId", userdId);
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> GetLastData(int warehouseId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT pw.*
								FROM [__MGO_ProductionWorkload] pw
								JOIN (
									SELECT MAX(RecordSyncId) AS MaxRecordSyncId
									FROM [__MGO_ProductionWorkload]
									WHERE [warehouseId] = {warehouseId}
								) maxRecord ON pw.RecordSyncId = maxRecord.MaxRecordSyncId WHERE pw.[warehouseId] = {warehouseId};";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity> GetLastHistoryData(int warehouseId, int week, int year, int maxItems = 15)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
								WITH CTE AS (
									SELECT  *,
										LAG(FaTotalQuantity) OVER (
											PARTITION BY WarehouseId, FaWeek, FaYear 
											ORDER BY RecordSyncId DESC
										) AS Prev_FaTotalQuantity
									FROM [__MGO_ProductionWorkload]
									WHERE WarehouseId = {warehouseId} 
										AND FaWeek = {week} 
										AND FaYear = {year}
								)
								SELECT TOP {maxItems} *
								FROM CTE
								WHERE FaTotalQuantity <> Prev_FaTotalQuantity OR Prev_FaTotalQuantity IS NULL
								ORDER BY RecordSyncId DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MGO.ProductionWorkloadEntity>();
			}
		}
		public static List<KeyValuePair<int, int>> GetBacklogData(int warehouseId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT pw.FaYear, pw.FaWeek
								FROM [__MGO_ProductionWorkload] pw
								JOIN (
									SELECT MAX(RecordSyncId) AS MaxRecordSyncId
									FROM [__MGO_ProductionWorkload]
									WHERE [warehouseId] = {warehouseId}
								) maxRecord ON pw.RecordSyncId = maxRecord.MaxRecordSyncId WHERE pw.[warehouseId] = {warehouseId} 
								AND CAST(DATEADD(DAY, (FaWeek - 1) * 7, DATEADD(WEEK, DATEDIFF(WEEK, 0, DATEFROMPARTS(FaYear, 1, 4)), 0)) AS DATE) < CAST(DATEADD(DAY,-(DATEPART(dw,GETDATE())-1),GETDATE()) AS DATE)
								ORDER BY pw.FaYear, pw.FaWeek;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, int>(int.Parse(x[0].ToString()), int.Parse(x[1].ToString()))).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, int>>();
			}
		}
		#endregion Custom Methods

	}
}
