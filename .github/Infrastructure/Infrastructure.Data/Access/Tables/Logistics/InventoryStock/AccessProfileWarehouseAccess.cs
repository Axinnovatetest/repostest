using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
    public class AccessProfileWarehouseAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity Get(int id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Inventory].[AccessProfileWarehouse] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id); 

                DbExecution.Fill(sqlCommand, dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Inventory].[AccessProfileWarehouse]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                DbExecution.Fill(sqlCommand, dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> get(List<int> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [Inventory].[AccessProfileWarehouse] WHERE [Id] IN ({queryIds})";                    
                DbExecution.Fill(sqlCommand, dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();}

        public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [Inventory].[AccessProfileWarehouse] ([AccessProfileId],[LastEditTime],[LastEditUser],[WarehouseId]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileId,@LastEditTime,@LastEditUser,@WarehouseId); ";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccessProfileId",item.AccessProfileId);
					sqlCommand.Parameters.AddWithValue("LastEditTime",item.LastEditTime == null ? (object)DBNull.Value  : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUser",item.LastEditUser == null ? (object)DBNull.Value  : item.LastEditUser);
					sqlCommand.Parameters.AddWithValue("WarehouseId",item.WarehouseId);

                    var result = DbExecution.ExecuteScalar(sqlCommand);
                    response = result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
                int results=0;
                if(items.Count <= maxParamsNumber)
                {
                    results = insert(items);
                }else
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
        private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " INSERT INTO [Inventory].[AccessProfileWarehouse] ([AccessProfileId],[LastEditTime],[LastEditUser],[WarehouseId]) VALUES ( "

							+ "@AccessProfileId"+ i +","
							+ "@LastEditTime"+ i +","
							+ "@LastEditUser"+ i +","
							+ "@WarehouseId"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("AccessProfileId" + i, item.AccessProfileId);
							sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value  : item.LastEditTime);
							sqlCommand.Parameters.AddWithValue("LastEditUser" + i, item.LastEditUser == null ? (object)DBNull.Value  : item.LastEditUser);
							sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId);
                    }

                    sqlCommand.CommandText = query;

                    results = DbExecution.ExecuteNonQuery(sqlCommand);
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "UPDATE [Inventory].[AccessProfileWarehouse] SET [AccessProfileId]=@AccessProfileId, [LastEditTime]=@LastEditTime, [LastEditUser]=@LastEditUser, [WarehouseId]=@WarehouseId WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileId",item.AccessProfileId);
				sqlCommand.Parameters.AddWithValue("LastEditTime",item.LastEditTime == null ? (object)DBNull.Value  : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUser",item.LastEditUser == null ? (object)DBNull.Value  : item.LastEditUser);
				sqlCommand.Parameters.AddWithValue("WarehouseId",item.WarehouseId);
                        
                results = DbExecution.ExecuteNonQuery(sqlCommand);
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
                int results = 0;
                if(items.Count <= maxParamsNumber)
                {
                    results = update(items);
                }else
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
        private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " UPDATE [Inventory].[AccessProfileWarehouse] SET "

							+ "[AccessProfileId]=@AccessProfileId"+ i +","
							+ "[LastEditTime]=@LastEditTime"+ i +","
							+ "[LastEditUser]=@LastEditUser"+ i +","
							+ "[WarehouseId]=@WarehouseId"+ i +" WHERE [Id]=@Id" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
							sqlCommand.Parameters.AddWithValue("AccessProfileId" + i, item.AccessProfileId);
							sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value  : item.LastEditTime);
							sqlCommand.Parameters.AddWithValue("LastEditUser" + i, item.LastEditUser == null ? (object)DBNull.Value  : item.LastEditUser);
							sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId);
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
                string query = "DELETE FROM [Inventory].[AccessProfileWarehouse] WHERE [Id]=@Id";
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

                    string query = "DELETE FROM [Inventory].[AccessProfileWarehouse] WHERE [Id] IN ("+ queryIds +")";                    
                    sqlCommand.CommandText = query;
                        
                    results = DbExecution.ExecuteNonQuery(sqlCommand);
                }

                return results;
            }
            return -1;
        }

        #region Methods with transaction
        public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
        {
            var dataTable = new DataTable();

            string query = "SELECT * FROM [Inventory].[AccessProfileWarehouse] WHERE [Id]=@Id";
            var sqlCommand = new SqlCommand(query, connection, transaction);
            sqlCommand.Parameters.AddWithValue("Id", id); 
            DbExecution.Fill(sqlCommand, dataTable);

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
        {  
            var dataTable = new DataTable();     

            string query = "SELECT * FROM [Inventory].[AccessProfileWarehouse]";
            var sqlCommand = new SqlCommand(query, connection, transaction); 

            DbExecution.Fill(sqlCommand, dataTable);

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = getWithTransaction(ids, connection, transaction);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
                    }
                    results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber), connection, transaction));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
        {
            if(ids != null && ids.Count > 0)
            {
                var dataTable = new DataTable();

                var sqlCommand = new SqlCommand("", connection, transaction);
                string queryIds = string.Empty;
                for(int i=0; i<ids.Count; i++)
                {
                    queryIds += "@Id" + i + ",";
                    sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                }
                queryIds = queryIds.TrimEnd(',');

                sqlCommand.CommandText = $"SELECT * FROM [Inventory].[AccessProfileWarehouse] WHERE [Id] IN ({queryIds})";                    
                DbExecution.Fill(sqlCommand, dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
        }

        public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity item, SqlConnection connection, SqlTransaction transaction)
        {
            int response = int.MinValue;
           

                string query = "INSERT INTO [Inventory].[AccessProfileWarehouse] ([AccessProfileId],[LastEditTime],[LastEditUser],[WarehouseId]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileId,@LastEditTime,@LastEditUser,@WarehouseId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AccessProfileId",item.AccessProfileId);
			sqlCommand.Parameters.AddWithValue("LastEditTime",item.LastEditTime == null ? (object)DBNull.Value  : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUser",item.LastEditUser == null ? (object)DBNull.Value  : item.LastEditUser);
			sqlCommand.Parameters.AddWithValue("WarehouseId",item.WarehouseId);

            var result = DbExecution.ExecuteScalar(sqlCommand);
            return result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
        }
        public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> items, SqlConnection connection, SqlTransaction transaction)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
                int results=0;
                if(items.Count <= maxParamsNumber)
                {
                    results = insertWithTransaction(items, connection, transaction);
                }else
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
        private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> items, SqlConnection connection, SqlTransaction transaction)
        {
            if (items != null && items.Count > 0)
            {
                string query = "";
                var sqlCommand = new SqlCommand(query, connection, transaction);

                int i = 0;
                foreach (var item in items)
                {
                    i++;
                    query += " INSERT INTO [Inventory].[AccessProfileWarehouse] ([AccessProfileId],[LastEditTime],[LastEditUser],[WarehouseId]) VALUES ( "

						+ "@AccessProfileId"+ i +","
						+ "@LastEditTime"+ i +","
						+ "@LastEditUser"+ i +","
						+ "@WarehouseId"+ i 
                            + "); ";

                            
						sqlCommand.Parameters.AddWithValue("AccessProfileId" + i, item.AccessProfileId);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value  : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUser" + i, item.LastEditUser == null ? (object)DBNull.Value  : item.LastEditUser);
						sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId);
                    }

                sqlCommand.CommandText = query;

                return DbExecution.ExecuteNonQuery(sqlCommand);
            }

            return -1;
        }

        public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity item, SqlConnection connection, SqlTransaction transaction)
        {   
            int results = -1;

            string query = "UPDATE [Inventory].[AccessProfileWarehouse] SET [AccessProfileId]=@AccessProfileId, [LastEditTime]=@LastEditTime, [LastEditUser]=@LastEditUser, [WarehouseId]=@WarehouseId WHERE [Id]=@Id";
            var sqlCommand = new SqlCommand(query, connection, transaction);
                    
                sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AccessProfileId",item.AccessProfileId);
			sqlCommand.Parameters.AddWithValue("LastEditTime",item.LastEditTime == null ? (object)DBNull.Value  : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUser",item.LastEditUser == null ? (object)DBNull.Value  : item.LastEditUser);
			sqlCommand.Parameters.AddWithValue("WarehouseId",item.WarehouseId);
                        
            results = DbExecution.ExecuteNonQuery(sqlCommand);
            return results;
        }
        public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> items, SqlConnection connection, SqlTransaction transaction)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
                int results = 0;
                if(items.Count <= maxParamsNumber)
                {
                    results = updateWithTransaction(items, connection, transaction);
                }else
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
        private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> items, SqlConnection connection, SqlTransaction transaction)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                string query = "";
                var sqlCommand = new SqlCommand(query, connection, transaction);

                int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " UPDATE [Inventory].[AccessProfileWarehouse] SET "

						+ "[AccessProfileId]=@AccessProfileId"+ i +","
						+ "[LastEditTime]=@LastEditTime"+ i +","
						+ "[LastEditUser]=@LastEditUser"+ i +","
						+ "[WarehouseId]=@WarehouseId"+ i +" WHERE [Id]=@Id" + i 
                            + "; ";

                        sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AccessProfileId" + i, item.AccessProfileId);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value  : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUser" + i, item.LastEditUser == null ? (object)DBNull.Value  : item.LastEditUser);
						sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId);
                    }

                sqlCommand.CommandText = query;
                return DbExecution.ExecuteNonQuery(sqlCommand);
            }

            return -1;
        }

        public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
        {
            int results = -1;

            string query = "DELETE FROM [Inventory].[AccessProfileWarehouse] WHERE [Id]=@Id";
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
                int results=0;
                if(ids.Count <= maxParamsNumber)
                {
                    results = deleteWithTransaction(ids, connection, transaction);
                } else
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
                for(int i=0; i<ids.Count; i++)
                {
                    queryIds += "@Id" + i + ",";
                    sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                }
                queryIds = queryIds.TrimEnd(',');

                string query = "DELETE FROM [Inventory].[AccessProfileWarehouse] WHERE [Id] IN ("+ queryIds +")";                    
                sqlCommand.CommandText = query;
                        
                results = DbExecution.ExecuteNonQuery(sqlCommand);
 

                return results;
            }
            return -1;
        }
		#endregion Methods with transaction
		#endregion Default Methods
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity> GetByAccessProfiles(IEnumerable<int> ids)
		{
			if(ids?.Count()<=0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Inventory].[AccessProfileWarehouse] WHERE [AccessProfileId] IN ({string.Join(",", ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity>();
			}
		}
		public static int DeleteByAccessProfile(int accessProfileId, IEnumerable <int> warehouseIds, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = $"DELETE FROM [Inventory].[AccessProfileWarehouse] WHERE [AccessProfileId]=@accessProfileId AND [WarehouseId] IN ({string.Join(",", warehouseIds)})";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("accessProfileId", accessProfileId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		#region Custom Methods

		#endregion Custom Methods

	}
}
