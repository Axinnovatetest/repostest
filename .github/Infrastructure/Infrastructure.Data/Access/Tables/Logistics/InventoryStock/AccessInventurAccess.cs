namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
    public class AccessInventurAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity Get(int lagerort_id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Inventory].[AccessInventur] WHERE [Lagerort_id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", lagerort_id); 

                DbExecution.Fill(sqlCommand, dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [Inventory].[AccessInventur]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                DbExecution.Fill(sqlCommand, dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> get(List<int> ids)
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

                    sqlCommand.CommandText = $"SELECT * FROM [Inventory].[AccessInventur] WHERE [Lagerort_id] IN ({queryIds})";                    
                DbExecution.Fill(sqlCommand, dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Inventory].[AccessInventur] ([Lagerort_id],[Blocking_FA_Start],[Blocking_ScannerInventur],[Blocking_Schneidenerei_Gewerk_1_3],[Blocking_Typ1HL_PL],[Blocking_Typ1PL_HL],[Blocking_Typ2HL_PL],[Blocking_Typ2PL_HL],[Blocking_WarehouseMovements]) OUTPUT INSERTED.[Lagerort_id] VALUES (@Lagerort_id,@Blocking_FA_Start,@Blocking_ScannerInventur,@Blocking_Schneidenerei_Gewerk_1_3,@Blocking_Typ1HL_PL,@Blocking_Typ1PL_HL,@Blocking_Typ2HL_PL,@Blocking_Typ2PL_HL,@Blocking_WarehouseMovements); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Blocking_FA_Start", item.Blocking_FA_Start == null ? (object)DBNull.Value : item.Blocking_FA_Start);
					sqlCommand.Parameters.AddWithValue("Blocking_ScannerInventur", item.Blocking_ScannerInventur == null ? (object)DBNull.Value : item.Blocking_ScannerInventur);
					sqlCommand.Parameters.AddWithValue("Blocking_Schneidenerei_Gewerk_1_3", item.Blocking_Schneidenerei_Gewerk_1_3 == null ? (object)DBNull.Value : item.Blocking_Schneidenerei_Gewerk_1_3);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ1HL_PL", item.Blocking_Typ1HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ1HL_PL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ1PL_HL", item.Blocking_Typ1PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ1PL_HL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ2HL_PL", item.Blocking_Typ2HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ2HL_PL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ2PL_HL", item.Blocking_Typ2PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ2PL_HL);
					sqlCommand.Parameters.AddWithValue("Blocking_WarehouseMovements", item.Blocking_WarehouseMovements == null ? (object)DBNull.Value : item.Blocking_WarehouseMovements);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> items)
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
						query += " INSERT INTO [Inventory].[AccessInventur] ([Lagerort_id],[Blocking_FA_Start],[Blocking_ScannerInventur],[Blocking_Schneidenerei_Gewerk_1_3],[Blocking_Typ1HL_PL],[Blocking_Typ1PL_HL],[Blocking_Typ2HL_PL],[Blocking_Typ2PL_HL],[Blocking_WarehouseMovements]) VALUES ( "

							+ "@Lagerort_id" + i + ","
							+ "@Blocking_FA_Start" + i + ","
							+ "@Blocking_ScannerInventur" + i + ","
							+ "@Blocking_Schneidenerei_Gewerk_1_3" + i + ","
							+ "@Blocking_Typ1HL_PL" + i + ","
							+ "@Blocking_Typ1PL_HL" + i + ","
							+ "@Blocking_Typ2HL_PL" + i + ","
							+ "@Blocking_Typ2PL_HL" + i + ","
							+ "@Blocking_WarehouseMovements" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Blocking_FA_Start" + i, item.Blocking_FA_Start == null ? (object)DBNull.Value : item.Blocking_FA_Start);
						sqlCommand.Parameters.AddWithValue("Blocking_ScannerInventur" + i, item.Blocking_ScannerInventur == null ? (object)DBNull.Value : item.Blocking_ScannerInventur);
						sqlCommand.Parameters.AddWithValue("Blocking_Schneidenerei_Gewerk_1_3" + i, item.Blocking_Schneidenerei_Gewerk_1_3 == null ? (object)DBNull.Value : item.Blocking_Schneidenerei_Gewerk_1_3);
						sqlCommand.Parameters.AddWithValue("Blocking_Typ1HL_PL" + i, item.Blocking_Typ1HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ1HL_PL);
						sqlCommand.Parameters.AddWithValue("Blocking_Typ1PL_HL" + i, item.Blocking_Typ1PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ1PL_HL);
						sqlCommand.Parameters.AddWithValue("Blocking_Typ2HL_PL" + i, item.Blocking_Typ2HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ2HL_PL);
						sqlCommand.Parameters.AddWithValue("Blocking_Typ2PL_HL" + i, item.Blocking_Typ2PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ2PL_HL);
						sqlCommand.Parameters.AddWithValue("Blocking_WarehouseMovements" + i, item.Blocking_WarehouseMovements == null ? (object)DBNull.Value : item.Blocking_WarehouseMovements);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Inventory].[AccessInventur] SET [Blocking_FA_Start]=@Blocking_FA_Start, [Blocking_ScannerInventur]=@Blocking_ScannerInventur, [Blocking_Schneidenerei_Gewerk_1_3]=@Blocking_Schneidenerei_Gewerk_1_3, [Blocking_Typ1HL_PL]=@Blocking_Typ1HL_PL, [Blocking_Typ1PL_HL]=@Blocking_Typ1PL_HL, [Blocking_Typ2HL_PL]=@Blocking_Typ2HL_PL, [Blocking_Typ2PL_HL]=@Blocking_Typ2PL_HL, [Blocking_WarehouseMovements]=@Blocking_WarehouseMovements WHERE [Lagerort_id]=@Lagerort_id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Blocking_FA_Start", item.Blocking_FA_Start == null ? (object)DBNull.Value : item.Blocking_FA_Start);
				sqlCommand.Parameters.AddWithValue("Blocking_ScannerInventur", item.Blocking_ScannerInventur == null ? (object)DBNull.Value : item.Blocking_ScannerInventur);
				sqlCommand.Parameters.AddWithValue("Blocking_Schneidenerei_Gewerk_1_3", item.Blocking_Schneidenerei_Gewerk_1_3 == null ? (object)DBNull.Value : item.Blocking_Schneidenerei_Gewerk_1_3);
				sqlCommand.Parameters.AddWithValue("Blocking_Typ1HL_PL", item.Blocking_Typ1HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ1HL_PL);
				sqlCommand.Parameters.AddWithValue("Blocking_Typ1PL_HL", item.Blocking_Typ1PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ1PL_HL);
				sqlCommand.Parameters.AddWithValue("Blocking_Typ2HL_PL", item.Blocking_Typ2HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ2HL_PL);
				sqlCommand.Parameters.AddWithValue("Blocking_Typ2PL_HL", item.Blocking_Typ2PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ2PL_HL);
				sqlCommand.Parameters.AddWithValue("Blocking_WarehouseMovements", item.Blocking_WarehouseMovements == null ? (object)DBNull.Value : item.Blocking_WarehouseMovements);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> items)
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
						query += " UPDATE [Inventory].[AccessInventur] SET "

							+ "[Blocking_FA_Start]=@Blocking_FA_Start" + i + ","
							+ "[Blocking_ScannerInventur]=@Blocking_ScannerInventur" + i + ","
							+ "[Blocking_Schneidenerei_Gewerk_1_3]=@Blocking_Schneidenerei_Gewerk_1_3" + i + ","
							+ "[Blocking_Typ1HL_PL]=@Blocking_Typ1HL_PL" + i + ","
							+ "[Blocking_Typ1PL_HL]=@Blocking_Typ1PL_HL" + i + ","
							+ "[Blocking_Typ2HL_PL]=@Blocking_Typ2HL_PL" + i + ","
							+ "[Blocking_Typ2PL_HL]=@Blocking_Typ2PL_HL" + i + ","
							+ "[Blocking_WarehouseMovements]=@Blocking_WarehouseMovements" + i + " WHERE [Lagerort_id]=@Lagerort_id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Blocking_FA_Start" + i, item.Blocking_FA_Start == null ? (object)DBNull.Value : item.Blocking_FA_Start);
						sqlCommand.Parameters.AddWithValue("Blocking_ScannerInventur" + i, item.Blocking_ScannerInventur == null ? (object)DBNull.Value : item.Blocking_ScannerInventur);
						sqlCommand.Parameters.AddWithValue("Blocking_Schneidenerei_Gewerk_1_3" + i, item.Blocking_Schneidenerei_Gewerk_1_3 == null ? (object)DBNull.Value : item.Blocking_Schneidenerei_Gewerk_1_3);
						sqlCommand.Parameters.AddWithValue("Blocking_Typ1HL_PL" + i, item.Blocking_Typ1HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ1HL_PL);
						sqlCommand.Parameters.AddWithValue("Blocking_Typ1PL_HL" + i, item.Blocking_Typ1PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ1PL_HL);
						sqlCommand.Parameters.AddWithValue("Blocking_Typ2HL_PL" + i, item.Blocking_Typ2HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ2HL_PL);
						sqlCommand.Parameters.AddWithValue("Blocking_Typ2PL_HL" + i, item.Blocking_Typ2PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ2PL_HL);
						sqlCommand.Parameters.AddWithValue("Blocking_WarehouseMovements" + i, item.Blocking_WarehouseMovements == null ? (object)DBNull.Value : item.Blocking_WarehouseMovements);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int lagerort_id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Inventory].[AccessInventur] WHERE [Lagerort_id]=@Lagerort_id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", lagerort_id);

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

					string query = "DELETE FROM [Inventory].[AccessInventur] WHERE [Lagerort_id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity GetWithTransaction(int lagerort_id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[AccessInventur] WHERE [Lagerort_id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", lagerort_id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[AccessInventur]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Inventory].[AccessInventur] WHERE [Lagerort_id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Inventory].[AccessInventur] ([Lagerort_id],[Blocking_FA_Start],[Blocking_ScannerInventur],[Blocking_Schneidenerei_Gewerk_1_3],[Blocking_Typ1HL_PL],[Blocking_Typ1PL_HL],[Blocking_Typ2HL_PL],[Blocking_Typ2PL_HL],[Blocking_WarehouseMovements]) OUTPUT INSERTED.[Lagerort_id] VALUES (@Lagerort_id,@Blocking_FA_Start,@Blocking_ScannerInventur,@Blocking_Schneidenerei_Gewerk_1_3,@Blocking_Typ1HL_PL,@Blocking_Typ1PL_HL,@Blocking_Typ2HL_PL,@Blocking_Typ2PL_HL,@Blocking_WarehouseMovements); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Blocking_FA_Start", item.Blocking_FA_Start == null ? (object)DBNull.Value : item.Blocking_FA_Start);
			sqlCommand.Parameters.AddWithValue("Blocking_ScannerInventur", item.Blocking_ScannerInventur == null ? (object)DBNull.Value : item.Blocking_ScannerInventur);
			sqlCommand.Parameters.AddWithValue("Blocking_Schneidenerei_Gewerk_1_3", item.Blocking_Schneidenerei_Gewerk_1_3 == null ? (object)DBNull.Value : item.Blocking_Schneidenerei_Gewerk_1_3);
			sqlCommand.Parameters.AddWithValue("Blocking_Typ1HL_PL", item.Blocking_Typ1HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ1HL_PL);
			sqlCommand.Parameters.AddWithValue("Blocking_Typ1PL_HL", item.Blocking_Typ1PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ1PL_HL);
			sqlCommand.Parameters.AddWithValue("Blocking_Typ2HL_PL", item.Blocking_Typ2HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ2HL_PL);
			sqlCommand.Parameters.AddWithValue("Blocking_Typ2PL_HL", item.Blocking_Typ2PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ2PL_HL);
			sqlCommand.Parameters.AddWithValue("Blocking_WarehouseMovements", item.Blocking_WarehouseMovements == null ? (object)DBNull.Value : item.Blocking_WarehouseMovements);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Inventory].[AccessInventur] ([Lagerort_id],[Blocking_FA_Start],[Blocking_ScannerInventur],[Blocking_Schneidenerei_Gewerk_1_3],[Blocking_Typ1HL_PL],[Blocking_Typ1PL_HL],[Blocking_Typ2HL_PL],[Blocking_Typ2PL_HL],[Blocking_WarehouseMovements]) VALUES ( "

						+ "@Lagerort_id" + i + ","
						+ "@Blocking_FA_Start" + i + ","
						+ "@Blocking_ScannerInventur" + i + ","
						+ "@Blocking_Schneidenerei_Gewerk_1_3" + i + ","
						+ "@Blocking_Typ1HL_PL" + i + ","
						+ "@Blocking_Typ1PL_HL" + i + ","
						+ "@Blocking_Typ2HL_PL" + i + ","
						+ "@Blocking_Typ2PL_HL" + i + ","
						+ "@Blocking_WarehouseMovements" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Blocking_FA_Start" + i, item.Blocking_FA_Start == null ? (object)DBNull.Value : item.Blocking_FA_Start);
					sqlCommand.Parameters.AddWithValue("Blocking_ScannerInventur" + i, item.Blocking_ScannerInventur == null ? (object)DBNull.Value : item.Blocking_ScannerInventur);
					sqlCommand.Parameters.AddWithValue("Blocking_Schneidenerei_Gewerk_1_3" + i, item.Blocking_Schneidenerei_Gewerk_1_3 == null ? (object)DBNull.Value : item.Blocking_Schneidenerei_Gewerk_1_3);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ1HL_PL" + i, item.Blocking_Typ1HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ1HL_PL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ1PL_HL" + i, item.Blocking_Typ1PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ1PL_HL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ2HL_PL" + i, item.Blocking_Typ2HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ2HL_PL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ2PL_HL" + i, item.Blocking_Typ2PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ2PL_HL);
					sqlCommand.Parameters.AddWithValue("Blocking_WarehouseMovements" + i, item.Blocking_WarehouseMovements == null ? (object)DBNull.Value : item.Blocking_WarehouseMovements);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Inventory].[AccessInventur] SET [Blocking_FA_Start]=@Blocking_FA_Start, [Blocking_ScannerInventur]=@Blocking_ScannerInventur, [Blocking_Schneidenerei_Gewerk_1_3]=@Blocking_Schneidenerei_Gewerk_1_3, [Blocking_Typ1HL_PL]=@Blocking_Typ1HL_PL, [Blocking_Typ1PL_HL]=@Blocking_Typ1PL_HL, [Blocking_Typ2HL_PL]=@Blocking_Typ2HL_PL, [Blocking_Typ2PL_HL]=@Blocking_Typ2PL_HL, [Blocking_WarehouseMovements]=@Blocking_WarehouseMovements WHERE [Lagerort_id]=@Lagerort_id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Blocking_FA_Start", item.Blocking_FA_Start == null ? (object)DBNull.Value : item.Blocking_FA_Start);
			sqlCommand.Parameters.AddWithValue("Blocking_ScannerInventur", item.Blocking_ScannerInventur == null ? (object)DBNull.Value : item.Blocking_ScannerInventur);
			sqlCommand.Parameters.AddWithValue("Blocking_Schneidenerei_Gewerk_1_3", item.Blocking_Schneidenerei_Gewerk_1_3 == null ? (object)DBNull.Value : item.Blocking_Schneidenerei_Gewerk_1_3);
			sqlCommand.Parameters.AddWithValue("Blocking_Typ1HL_PL", item.Blocking_Typ1HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ1HL_PL);
			sqlCommand.Parameters.AddWithValue("Blocking_Typ1PL_HL", item.Blocking_Typ1PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ1PL_HL);
			sqlCommand.Parameters.AddWithValue("Blocking_Typ2HL_PL", item.Blocking_Typ2HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ2HL_PL);
			sqlCommand.Parameters.AddWithValue("Blocking_Typ2PL_HL", item.Blocking_Typ2PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ2PL_HL);
			sqlCommand.Parameters.AddWithValue("Blocking_WarehouseMovements", item.Blocking_WarehouseMovements == null ? (object)DBNull.Value : item.Blocking_WarehouseMovements);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Inventory].[AccessInventur] SET "

					+ "[Blocking_FA_Start]=@Blocking_FA_Start" + i + ","
					+ "[Blocking_ScannerInventur]=@Blocking_ScannerInventur" + i + ","
					+ "[Blocking_Schneidenerei_Gewerk_1_3]=@Blocking_Schneidenerei_Gewerk_1_3" + i + ","
					+ "[Blocking_Typ1HL_PL]=@Blocking_Typ1HL_PL" + i + ","
					+ "[Blocking_Typ1PL_HL]=@Blocking_Typ1PL_HL" + i + ","
					+ "[Blocking_Typ2HL_PL]=@Blocking_Typ2HL_PL" + i + ","
					+ "[Blocking_Typ2PL_HL]=@Blocking_Typ2PL_HL" + i + ","
					+ "[Blocking_WarehouseMovements]=@Blocking_WarehouseMovements" + i + " WHERE [Lagerort_id]=@Lagerort_id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Blocking_FA_Start" + i, item.Blocking_FA_Start == null ? (object)DBNull.Value : item.Blocking_FA_Start);
					sqlCommand.Parameters.AddWithValue("Blocking_ScannerInventur" + i, item.Blocking_ScannerInventur == null ? (object)DBNull.Value : item.Blocking_ScannerInventur);
					sqlCommand.Parameters.AddWithValue("Blocking_Schneidenerei_Gewerk_1_3" + i, item.Blocking_Schneidenerei_Gewerk_1_3 == null ? (object)DBNull.Value : item.Blocking_Schneidenerei_Gewerk_1_3);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ1HL_PL" + i, item.Blocking_Typ1HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ1HL_PL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ1PL_HL" + i, item.Blocking_Typ1PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ1PL_HL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ2HL_PL" + i, item.Blocking_Typ2HL_PL == null ? (object)DBNull.Value : item.Blocking_Typ2HL_PL);
					sqlCommand.Parameters.AddWithValue("Blocking_Typ2PL_HL" + i, item.Blocking_Typ2PL_HL == null ? (object)DBNull.Value : item.Blocking_Typ2PL_HL);
					sqlCommand.Parameters.AddWithValue("Blocking_WarehouseMovements" + i, item.Blocking_WarehouseMovements == null ? (object)DBNull.Value : item.Blocking_WarehouseMovements);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int lagerort_id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Inventory].[AccessInventur] WHERE [Lagerort_id]=@Lagerort_id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", lagerort_id);

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

				string query = "DELETE FROM [Inventory].[AccessInventur] WHERE [Lagerort_id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int BlockFaStart(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Inventory].[AccessInventur] SET [Blocking_FA_Start]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int BlockT1HauptToProd(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Inventory].[AccessInventur] SET [Blocking_Typ1HL_PL]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int BlockT1ProdToHaupt(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Inventory].[AccessInventur] SET [Blocking_Typ1PL_HL]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int BlockT2HauptToProd(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Inventory].[AccessInventur] SET [Blocking_Typ2HL_PL]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int BlockT2ProdToHaupt(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Inventory].[AccessInventur] SET [Blocking_Typ2PL_HL]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int BlockScanner(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Inventory].[AccessInventur] SET [Blocking_ScannerInventur]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int BlockWarehouses(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Inventory].[AccessInventur] SET [Blocking_WarehouseMovements]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int BlockSchneiderei(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Inventory].[AccessInventur] SET [Blocking_Schneidenerei_Gewerk_1_3]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> GetBlockedWarehouses(IEnumerable<int> lagerIds = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Inventory].[AccessInventur] WHERE [Blocking_WarehouseMovements]=1{(lagerIds?.Count() > 0 ? $" AND [Lagerort_id] IN ({string.Join(",", lagerIds)})" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
			}
		}
		public static IEnumerable<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity> GetInventoryActive(IEnumerable<int> lagerIds)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [Inventory].[AccessInventur] WHERE (ISNULL([Blocking_FA_Start],0)=1 OR ISNULL([Blocking_ScannerInventur],0)=1 OR ISNULL([Blocking_Schneidenerei_Gewerk_1_3],0)=1 OR ISNULL([Blocking_Typ1HL_PL],0)=1 OR ISNULL([Blocking_Typ1PL_HL],0)=1 
									OR ISNULL([Blocking_Typ2HL_PL],0)=1 OR ISNULL([Blocking_Typ2PL_HL],0)=1 OR ISNULL([Blocking_WarehouseMovements],0)=1)
									{(lagerIds?.Count() > 0 ? $" AND [Lagerort_id] IN ({string.Join(",", lagerIds)})" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.AccessInventurEntity>();
			}
		}
		#endregion Custom Methods
	}
}
