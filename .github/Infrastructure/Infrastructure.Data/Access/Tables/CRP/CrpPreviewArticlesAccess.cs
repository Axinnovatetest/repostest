using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CRP
{
    public class CrpPreviewArticlesAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity Get(int id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [stats].[CrpPreviewArticles] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id); 

                DbExecution.Fill(sqlCommand, dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [stats].[CrpPreviewArticles]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                DbExecution.Fill(sqlCommand, dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> get(List<int> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [stats].[CrpPreviewArticles] WHERE [Id] IN ({queryIds})";                    
                DbExecution.Fill(sqlCommand, dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();}

		public static int Insert(Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [stats].[CrpPreviewArticles] ([ArticleId],[ArticleNumber],[Designation],[ExternalStatus],[MinimumStock],[Stock],[SumNeeds],[SumNeedsAB],[SumNeedsFC],[SumNeedsLP],[SumProds],[SyncDate],[SyncId]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@Designation,@ExternalStatus,@MinimumStock,@Stock,@SumNeeds,@SumNeedsAB,@SumNeedsFC,@SumNeedsLP,@SumProds,@SyncDate,@SyncId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Designation", item.Designation == null ? (object)DBNull.Value : item.Designation);
					sqlCommand.Parameters.AddWithValue("ExternalStatus", item.ExternalStatus == null ? (object)DBNull.Value : item.ExternalStatus);
					sqlCommand.Parameters.AddWithValue("MinimumStock", item.MinimumStock == null ? (object)DBNull.Value : item.MinimumStock);
					sqlCommand.Parameters.AddWithValue("Stock", item.Stock == null ? (object)DBNull.Value : item.Stock);
					sqlCommand.Parameters.AddWithValue("SumNeeds", item.SumNeeds == null ? (object)DBNull.Value : item.SumNeeds);
					sqlCommand.Parameters.AddWithValue("SumNeedsAB", item.SumNeedsAB == null ? (object)DBNull.Value : item.SumNeedsAB);
					sqlCommand.Parameters.AddWithValue("SumNeedsFC", item.SumNeedsFC == null ? (object)DBNull.Value : item.SumNeedsFC);
					sqlCommand.Parameters.AddWithValue("SumNeedsLP", item.SumNeedsLP == null ? (object)DBNull.Value : item.SumNeedsLP);
					sqlCommand.Parameters.AddWithValue("SumProds", item.SumProds == null ? (object)DBNull.Value : item.SumProds);
					sqlCommand.Parameters.AddWithValue("SyncDate", item.SyncDate == null ? (object)DBNull.Value : item.SyncDate);
					sqlCommand.Parameters.AddWithValue("SyncId", item.SyncId == null ? (object)DBNull.Value : item.SyncId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> items)
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
						query += " INSERT INTO [stats].[CrpPreviewArticles] ([ArticleId],[ArticleNumber],[Designation],[ExternalStatus],[MinimumStock],[Stock],[SumNeeds],[SumNeedsAB],[SumNeedsFC],[SumNeedsLP],[SumProds],[SyncDate],[SyncId]) VALUES ( "

							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@Designation" + i + ","
							+ "@ExternalStatus" + i + ","
							+ "@MinimumStock" + i + ","
							+ "@Stock" + i + ","
							+ "@SumNeeds" + i + ","
							+ "@SumNeedsAB" + i + ","
							+ "@SumNeedsFC" + i + ","
							+ "@SumNeedsLP" + i + ","
							+ "@SumProds" + i + ","
							+ "@SyncDate" + i + ","
							+ "@SyncId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation == null ? (object)DBNull.Value : item.Designation);
						sqlCommand.Parameters.AddWithValue("ExternalStatus" + i, item.ExternalStatus == null ? (object)DBNull.Value : item.ExternalStatus);
						sqlCommand.Parameters.AddWithValue("MinimumStock" + i, item.MinimumStock == null ? (object)DBNull.Value : item.MinimumStock);
						sqlCommand.Parameters.AddWithValue("Stock" + i, item.Stock == null ? (object)DBNull.Value : item.Stock);
						sqlCommand.Parameters.AddWithValue("SumNeeds" + i, item.SumNeeds == null ? (object)DBNull.Value : item.SumNeeds);
						sqlCommand.Parameters.AddWithValue("SumNeedsAB" + i, item.SumNeedsAB == null ? (object)DBNull.Value : item.SumNeedsAB);
						sqlCommand.Parameters.AddWithValue("SumNeedsFC" + i, item.SumNeedsFC == null ? (object)DBNull.Value : item.SumNeedsFC);
						sqlCommand.Parameters.AddWithValue("SumNeedsLP" + i, item.SumNeedsLP == null ? (object)DBNull.Value : item.SumNeedsLP);
						sqlCommand.Parameters.AddWithValue("SumProds" + i, item.SumProds == null ? (object)DBNull.Value : item.SumProds);
						sqlCommand.Parameters.AddWithValue("SyncDate" + i, item.SyncDate == null ? (object)DBNull.Value : item.SyncDate);
						sqlCommand.Parameters.AddWithValue("SyncId" + i, item.SyncId == null ? (object)DBNull.Value : item.SyncId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [stats].[CrpPreviewArticles] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [Designation]=@Designation, [ExternalStatus]=@ExternalStatus, [MinimumStock]=@MinimumStock, [Stock]=@Stock, [SumNeeds]=@SumNeeds, [SumNeedsAB]=@SumNeedsAB, [SumNeedsFC]=@SumNeedsFC, [SumNeedsLP]=@SumNeedsLP, [SumProds]=@SumProds, [SyncDate]=@SyncDate, [SyncId]=@SyncId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("Designation", item.Designation == null ? (object)DBNull.Value : item.Designation);
				sqlCommand.Parameters.AddWithValue("ExternalStatus", item.ExternalStatus == null ? (object)DBNull.Value : item.ExternalStatus);
				sqlCommand.Parameters.AddWithValue("MinimumStock", item.MinimumStock == null ? (object)DBNull.Value : item.MinimumStock);
				sqlCommand.Parameters.AddWithValue("Stock", item.Stock == null ? (object)DBNull.Value : item.Stock);
				sqlCommand.Parameters.AddWithValue("SumNeeds", item.SumNeeds == null ? (object)DBNull.Value : item.SumNeeds);
				sqlCommand.Parameters.AddWithValue("SumNeedsAB", item.SumNeedsAB == null ? (object)DBNull.Value : item.SumNeedsAB);
				sqlCommand.Parameters.AddWithValue("SumNeedsFC", item.SumNeedsFC == null ? (object)DBNull.Value : item.SumNeedsFC);
				sqlCommand.Parameters.AddWithValue("SumNeedsLP", item.SumNeedsLP == null ? (object)DBNull.Value : item.SumNeedsLP);
				sqlCommand.Parameters.AddWithValue("SumProds", item.SumProds == null ? (object)DBNull.Value : item.SumProds);
				sqlCommand.Parameters.AddWithValue("SyncDate", item.SyncDate == null ? (object)DBNull.Value : item.SyncDate);
				sqlCommand.Parameters.AddWithValue("SyncId", item.SyncId == null ? (object)DBNull.Value : item.SyncId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> items)
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
						query += " UPDATE [stats].[CrpPreviewArticles] SET "

							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[Designation]=@Designation" + i + ","
							+ "[ExternalStatus]=@ExternalStatus" + i + ","
							+ "[MinimumStock]=@MinimumStock" + i + ","
							+ "[Stock]=@Stock" + i + ","
							+ "[SumNeeds]=@SumNeeds" + i + ","
							+ "[SumNeedsAB]=@SumNeedsAB" + i + ","
							+ "[SumNeedsFC]=@SumNeedsFC" + i + ","
							+ "[SumNeedsLP]=@SumNeedsLP" + i + ","
							+ "[SumProds]=@SumProds" + i + ","
							+ "[SyncDate]=@SyncDate" + i + ","
							+ "[SyncId]=@SyncId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation == null ? (object)DBNull.Value : item.Designation);
						sqlCommand.Parameters.AddWithValue("ExternalStatus" + i, item.ExternalStatus == null ? (object)DBNull.Value : item.ExternalStatus);
						sqlCommand.Parameters.AddWithValue("MinimumStock" + i, item.MinimumStock == null ? (object)DBNull.Value : item.MinimumStock);
						sqlCommand.Parameters.AddWithValue("Stock" + i, item.Stock == null ? (object)DBNull.Value : item.Stock);
						sqlCommand.Parameters.AddWithValue("SumNeeds" + i, item.SumNeeds == null ? (object)DBNull.Value : item.SumNeeds);
						sqlCommand.Parameters.AddWithValue("SumNeedsAB" + i, item.SumNeedsAB == null ? (object)DBNull.Value : item.SumNeedsAB);
						sqlCommand.Parameters.AddWithValue("SumNeedsFC" + i, item.SumNeedsFC == null ? (object)DBNull.Value : item.SumNeedsFC);
						sqlCommand.Parameters.AddWithValue("SumNeedsLP" + i, item.SumNeedsLP == null ? (object)DBNull.Value : item.SumNeedsLP);
						sqlCommand.Parameters.AddWithValue("SumProds" + i, item.SumProds == null ? (object)DBNull.Value : item.SumProds);
						sqlCommand.Parameters.AddWithValue("SyncDate" + i, item.SyncDate == null ? (object)DBNull.Value : item.SyncDate);
						sqlCommand.Parameters.AddWithValue("SyncId" + i, item.SyncId == null ? (object)DBNull.Value : item.SyncId);
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
                string query = "DELETE FROM [stats].[CrpPreviewArticles] WHERE [Id]=@Id";
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

                    string query = "DELETE FROM [stats].[CrpPreviewArticles] WHERE [Id] IN ("+ queryIds +")";                    
                    sqlCommand.CommandText = query;
                        
                    results = DbExecution.ExecuteNonQuery(sqlCommand);
                }

                return results;
            }
            return -1;
        }

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[CrpPreviewArticles] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[CrpPreviewArticles]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [stats].[CrpPreviewArticles] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [stats].[CrpPreviewArticles] ([ArticleId],[ArticleNumber],[Designation],[ExternalStatus],[MinimumStock],[Stock],[SumNeeds],[SumNeedsAB],[SumNeedsFC],[SumNeedsLP],[SumProds],[SyncDate],[SyncId]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@Designation,@ExternalStatus,@MinimumStock,@Stock,@SumNeeds,@SumNeedsAB,@SumNeedsFC,@SumNeedsLP,@SumProds,@SyncDate,@SyncId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Designation", item.Designation == null ? (object)DBNull.Value : item.Designation);
			sqlCommand.Parameters.AddWithValue("ExternalStatus", item.ExternalStatus == null ? (object)DBNull.Value : item.ExternalStatus);
			sqlCommand.Parameters.AddWithValue("MinimumStock", item.MinimumStock == null ? (object)DBNull.Value : item.MinimumStock);
			sqlCommand.Parameters.AddWithValue("Stock", item.Stock == null ? (object)DBNull.Value : item.Stock);
			sqlCommand.Parameters.AddWithValue("SumNeeds", item.SumNeeds == null ? (object)DBNull.Value : item.SumNeeds);
			sqlCommand.Parameters.AddWithValue("SumNeedsAB", item.SumNeedsAB == null ? (object)DBNull.Value : item.SumNeedsAB);
			sqlCommand.Parameters.AddWithValue("SumNeedsFC", item.SumNeedsFC == null ? (object)DBNull.Value : item.SumNeedsFC);
			sqlCommand.Parameters.AddWithValue("SumNeedsLP", item.SumNeedsLP == null ? (object)DBNull.Value : item.SumNeedsLP);
			sqlCommand.Parameters.AddWithValue("SumProds", item.SumProds == null ? (object)DBNull.Value : item.SumProds);
			sqlCommand.Parameters.AddWithValue("SyncDate", item.SyncDate == null ? (object)DBNull.Value : item.SyncDate);
			sqlCommand.Parameters.AddWithValue("SyncId", item.SyncId == null ? (object)DBNull.Value : item.SyncId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [stats].[CrpPreviewArticles] ([ArticleId],[ArticleNumber],[Designation],[ExternalStatus],[MinimumStock],[Stock],[SumNeeds],[SumNeedsAB],[SumNeedsFC],[SumNeedsLP],[SumProds],[SyncDate],[SyncId]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@Designation" + i + ","
						+ "@ExternalStatus" + i + ","
						+ "@MinimumStock" + i + ","
						+ "@Stock" + i + ","
						+ "@SumNeeds" + i + ","
						+ "@SumNeedsAB" + i + ","
						+ "@SumNeedsFC" + i + ","
						+ "@SumNeedsLP" + i + ","
						+ "@SumProds" + i + ","
						+ "@SyncDate" + i + ","
						+ "@SyncId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation == null ? (object)DBNull.Value : item.Designation);
					sqlCommand.Parameters.AddWithValue("ExternalStatus" + i, item.ExternalStatus == null ? (object)DBNull.Value : item.ExternalStatus);
					sqlCommand.Parameters.AddWithValue("MinimumStock" + i, item.MinimumStock == null ? (object)DBNull.Value : item.MinimumStock);
					sqlCommand.Parameters.AddWithValue("Stock" + i, item.Stock == null ? (object)DBNull.Value : item.Stock);
					sqlCommand.Parameters.AddWithValue("SumNeeds" + i, item.SumNeeds == null ? (object)DBNull.Value : item.SumNeeds);
					sqlCommand.Parameters.AddWithValue("SumNeedsAB" + i, item.SumNeedsAB == null ? (object)DBNull.Value : item.SumNeedsAB);
					sqlCommand.Parameters.AddWithValue("SumNeedsFC" + i, item.SumNeedsFC == null ? (object)DBNull.Value : item.SumNeedsFC);
					sqlCommand.Parameters.AddWithValue("SumNeedsLP" + i, item.SumNeedsLP == null ? (object)DBNull.Value : item.SumNeedsLP);
					sqlCommand.Parameters.AddWithValue("SumProds" + i, item.SumProds == null ? (object)DBNull.Value : item.SumProds);
					sqlCommand.Parameters.AddWithValue("SyncDate" + i, item.SyncDate == null ? (object)DBNull.Value : item.SyncDate);
					sqlCommand.Parameters.AddWithValue("SyncId" + i, item.SyncId == null ? (object)DBNull.Value : item.SyncId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [stats].[CrpPreviewArticles] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [Designation]=@Designation, [ExternalStatus]=@ExternalStatus, [MinimumStock]=@MinimumStock, [Stock]=@Stock, [SumNeeds]=@SumNeeds, [SumNeedsAB]=@SumNeedsAB, [SumNeedsFC]=@SumNeedsFC, [SumNeedsLP]=@SumNeedsLP, [SumProds]=@SumProds, [SyncDate]=@SyncDate, [SyncId]=@SyncId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Designation", item.Designation == null ? (object)DBNull.Value : item.Designation);
			sqlCommand.Parameters.AddWithValue("ExternalStatus", item.ExternalStatus == null ? (object)DBNull.Value : item.ExternalStatus);
			sqlCommand.Parameters.AddWithValue("MinimumStock", item.MinimumStock == null ? (object)DBNull.Value : item.MinimumStock);
			sqlCommand.Parameters.AddWithValue("Stock", item.Stock == null ? (object)DBNull.Value : item.Stock);
			sqlCommand.Parameters.AddWithValue("SumNeeds", item.SumNeeds == null ? (object)DBNull.Value : item.SumNeeds);
			sqlCommand.Parameters.AddWithValue("SumNeedsAB", item.SumNeedsAB == null ? (object)DBNull.Value : item.SumNeedsAB);
			sqlCommand.Parameters.AddWithValue("SumNeedsFC", item.SumNeedsFC == null ? (object)DBNull.Value : item.SumNeedsFC);
			sqlCommand.Parameters.AddWithValue("SumNeedsLP", item.SumNeedsLP == null ? (object)DBNull.Value : item.SumNeedsLP);
			sqlCommand.Parameters.AddWithValue("SumProds", item.SumProds == null ? (object)DBNull.Value : item.SumProds);
			sqlCommand.Parameters.AddWithValue("SyncDate", item.SyncDate == null ? (object)DBNull.Value : item.SyncDate);
			sqlCommand.Parameters.AddWithValue("SyncId", item.SyncId == null ? (object)DBNull.Value : item.SyncId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [stats].[CrpPreviewArticles] SET "

					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[Designation]=@Designation" + i + ","
					+ "[ExternalStatus]=@ExternalStatus" + i + ","
					+ "[MinimumStock]=@MinimumStock" + i + ","
					+ "[Stock]=@Stock" + i + ","
					+ "[SumNeeds]=@SumNeeds" + i + ","
					+ "[SumNeedsAB]=@SumNeedsAB" + i + ","
					+ "[SumNeedsFC]=@SumNeedsFC" + i + ","
					+ "[SumNeedsLP]=@SumNeedsLP" + i + ","
					+ "[SumProds]=@SumProds" + i + ","
					+ "[SyncDate]=@SyncDate" + i + ","
					+ "[SyncId]=@SyncId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation == null ? (object)DBNull.Value : item.Designation);
					sqlCommand.Parameters.AddWithValue("ExternalStatus" + i, item.ExternalStatus == null ? (object)DBNull.Value : item.ExternalStatus);
					sqlCommand.Parameters.AddWithValue("MinimumStock" + i, item.MinimumStock == null ? (object)DBNull.Value : item.MinimumStock);
					sqlCommand.Parameters.AddWithValue("Stock" + i, item.Stock == null ? (object)DBNull.Value : item.Stock);
					sqlCommand.Parameters.AddWithValue("SumNeeds" + i, item.SumNeeds == null ? (object)DBNull.Value : item.SumNeeds);
					sqlCommand.Parameters.AddWithValue("SumNeedsAB" + i, item.SumNeedsAB == null ? (object)DBNull.Value : item.SumNeedsAB);
					sqlCommand.Parameters.AddWithValue("SumNeedsFC" + i, item.SumNeedsFC == null ? (object)DBNull.Value : item.SumNeedsFC);
					sqlCommand.Parameters.AddWithValue("SumNeedsLP" + i, item.SumNeedsLP == null ? (object)DBNull.Value : item.SumNeedsLP);
					sqlCommand.Parameters.AddWithValue("SumProds" + i, item.SumProds == null ? (object)DBNull.Value : item.SumProds);
					sqlCommand.Parameters.AddWithValue("SyncDate" + i, item.SyncDate == null ? (object)DBNull.Value : item.SyncDate);
					sqlCommand.Parameters.AddWithValue("SyncId" + i, item.SyncId == null ? (object)DBNull.Value : item.SyncId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [stats].[CrpPreviewArticles] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [stats].[CrpPreviewArticles] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity GetByArticle(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM  [stats].[CrpPreviewArticles] WHERE [ArticleId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.CrpPreviewArticlesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods

	}
}
