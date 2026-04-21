namespace Infrastructure.Data.Access.Tables.CRP.HistoryFG
{
	public class HistoryDetailsFGBestandAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity Get(long id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Details] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Details]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> Get(List<long> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> get(List<long> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [stats].[__CRP_HistoryFG_Details] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
		}

		public static long Insert(Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity item)
		{
			long response = long.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [stats].[__CRP_HistoryFG_Details] ([ArticleDesignation1],[ArticleDesignation2],[ArticleNumber],[ArticleReleaseStatus],[CsContact],[CustomerName],[CustomerNumber],[EdiStandard],[HeaderId],[StockQuantity],[TotalCostsWithCu],[TotalCostsWithoutCu],[TotalSalesPrice],[UBG],[UnitSalesPrice],[VKE],[WarehouseId],[WarehouseName]) OUTPUT INSERTED.[Id] VALUES (@ArticleDesignation1,@ArticleDesignation2,@ArticleNumber,@ArticleReleaseStatus,@CsContact,@CustomerName,@CustomerNumber,@EdiStandard,@HeaderId,@StockQuantity,@TotalCostsWithCu,@TotalCostsWithoutCu,@TotalSalesPrice,@UBG,@UnitSalesPrice,@VKE,@WarehouseId,@WarehouseName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleDesignation1", item.ArticleDesignation1 == null ? (object)DBNull.Value : item.ArticleDesignation1);
					sqlCommand.Parameters.AddWithValue("ArticleDesignation2", item.ArticleDesignation2 == null ? (object)DBNull.Value : item.ArticleDesignation2);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("ArticleReleaseStatus", item.ArticleReleaseStatus == null ? (object)DBNull.Value : item.ArticleReleaseStatus);
					sqlCommand.Parameters.AddWithValue("CsContact", item.CsContact == null ? (object)DBNull.Value : item.CsContact);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("EdiStandard", item.EdiStandard == null ? (object)DBNull.Value : item.EdiStandard);
					sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("StockQuantity", item.StockQuantity == null ? (object)DBNull.Value : item.StockQuantity);
					sqlCommand.Parameters.AddWithValue("TotalCostsWithCu", item.TotalCostsWithCu == null ? (object)DBNull.Value : item.TotalCostsWithCu);
					sqlCommand.Parameters.AddWithValue("TotalCostsWithoutCu", item.TotalCostsWithoutCu == null ? (object)DBNull.Value : item.TotalCostsWithoutCu);
					sqlCommand.Parameters.AddWithValue("TotalSalesPrice", item.TotalSalesPrice == null ? (object)DBNull.Value : item.TotalSalesPrice);
					sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
					sqlCommand.Parameters.AddWithValue("UnitSalesPrice", item.UnitSalesPrice == null ? (object)DBNull.Value : item.UnitSalesPrice);
					sqlCommand.Parameters.AddWithValue("VKE", item.VKE == null ? (object)DBNull.Value : item.VKE);
					sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseName", item.WarehouseName == null ? (object)DBNull.Value : item.WarehouseName);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> items)
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
						query += " INSERT INTO [stats].[__CRP_HistoryFG_Details] ([ArticleDesignation1],[ArticleDesignation2],[ArticleNumber],[ArticleReleaseStatus],[CsContact],[CustomerName],[CustomerNumber],[EdiStandard],[HeaderId],[StockQuantity],[TotalCostsWithCu],[TotalCostsWithoutCu],[TotalSalesPrice],[UBG],[UnitSalesPrice],[VKE],[WarehouseId],[WarehouseName]) VALUES ( "

							+ "@ArticleDesignation1" + i + ","
							+ "@ArticleDesignation2" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@ArticleReleaseStatus" + i + ","
							+ "@CsContact" + i + ","
							+ "@CustomerName" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@EdiStandard" + i + ","
							+ "@HeaderId" + i + ","
							+ "@StockQuantity" + i + ","
							+ "@TotalCostsWithCu" + i + ","
							+ "@TotalCostsWithoutCu" + i + ","
							+ "@TotalSalesPrice" + i + ","
							+ "@UBG" + i + ","
							+ "@UnitSalesPrice" + i + ","
							+ "@VKE" + i + ","
							+ "@WarehouseId" + i + ","
							+ "@WarehouseName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleDesignation1" + i, item.ArticleDesignation1 == null ? (object)DBNull.Value : item.ArticleDesignation1);
						sqlCommand.Parameters.AddWithValue("ArticleDesignation2" + i, item.ArticleDesignation2 == null ? (object)DBNull.Value : item.ArticleDesignation2);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("ArticleReleaseStatus" + i, item.ArticleReleaseStatus == null ? (object)DBNull.Value : item.ArticleReleaseStatus);
						sqlCommand.Parameters.AddWithValue("CsContact" + i, item.CsContact == null ? (object)DBNull.Value : item.CsContact);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("EdiStandard" + i, item.EdiStandard == null ? (object)DBNull.Value : item.EdiStandard);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
						sqlCommand.Parameters.AddWithValue("StockQuantity" + i, item.StockQuantity == null ? (object)DBNull.Value : item.StockQuantity);
						sqlCommand.Parameters.AddWithValue("TotalCostsWithCu" + i, item.TotalCostsWithCu == null ? (object)DBNull.Value : item.TotalCostsWithCu);
						sqlCommand.Parameters.AddWithValue("TotalCostsWithoutCu" + i, item.TotalCostsWithoutCu == null ? (object)DBNull.Value : item.TotalCostsWithoutCu);
						sqlCommand.Parameters.AddWithValue("TotalSalesPrice" + i, item.TotalSalesPrice == null ? (object)DBNull.Value : item.TotalSalesPrice);
						sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
						sqlCommand.Parameters.AddWithValue("UnitSalesPrice" + i, item.UnitSalesPrice == null ? (object)DBNull.Value : item.UnitSalesPrice);
						sqlCommand.Parameters.AddWithValue("VKE" + i, item.VKE == null ? (object)DBNull.Value : item.VKE);
						sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
						sqlCommand.Parameters.AddWithValue("WarehouseName" + i, item.WarehouseName == null ? (object)DBNull.Value : item.WarehouseName);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [stats].[__CRP_HistoryFG_Details] SET [ArticleDesignation1]=@ArticleDesignation1, [ArticleDesignation2]=@ArticleDesignation2, [ArticleNumber]=@ArticleNumber, [ArticleReleaseStatus]=@ArticleReleaseStatus, [CsContact]=@CsContact, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [EdiStandard]=@EdiStandard, [HeaderId]=@HeaderId, [StockQuantity]=@StockQuantity, [TotalCostsWithCu]=@TotalCostsWithCu, [TotalCostsWithoutCu]=@TotalCostsWithoutCu, [TotalSalesPrice]=@TotalSalesPrice, [UBG]=@UBG, [UnitSalesPrice]=@UnitSalesPrice, [VKE]=@VKE, [WarehouseId]=@WarehouseId, [WarehouseName]=@WarehouseName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleDesignation1", item.ArticleDesignation1 == null ? (object)DBNull.Value : item.ArticleDesignation1);
				sqlCommand.Parameters.AddWithValue("ArticleDesignation2", item.ArticleDesignation2 == null ? (object)DBNull.Value : item.ArticleDesignation2);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("ArticleReleaseStatus", item.ArticleReleaseStatus == null ? (object)DBNull.Value : item.ArticleReleaseStatus);
				sqlCommand.Parameters.AddWithValue("CsContact", item.CsContact == null ? (object)DBNull.Value : item.CsContact);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("EdiStandard", item.EdiStandard == null ? (object)DBNull.Value : item.EdiStandard);
				sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
				sqlCommand.Parameters.AddWithValue("StockQuantity", item.StockQuantity == null ? (object)DBNull.Value : item.StockQuantity);
				sqlCommand.Parameters.AddWithValue("TotalCostsWithCu", item.TotalCostsWithCu == null ? (object)DBNull.Value : item.TotalCostsWithCu);
				sqlCommand.Parameters.AddWithValue("TotalCostsWithoutCu", item.TotalCostsWithoutCu == null ? (object)DBNull.Value : item.TotalCostsWithoutCu);
				sqlCommand.Parameters.AddWithValue("TotalSalesPrice", item.TotalSalesPrice == null ? (object)DBNull.Value : item.TotalSalesPrice);
				sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
				sqlCommand.Parameters.AddWithValue("UnitSalesPrice", item.UnitSalesPrice == null ? (object)DBNull.Value : item.UnitSalesPrice);
				sqlCommand.Parameters.AddWithValue("VKE", item.VKE == null ? (object)DBNull.Value : item.VKE);
				sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
				sqlCommand.Parameters.AddWithValue("WarehouseName", item.WarehouseName == null ? (object)DBNull.Value : item.WarehouseName);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> items)
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
						query += " UPDATE [stats].[__CRP_HistoryFG_Details] SET "

							+ "[ArticleDesignation1]=@ArticleDesignation1" + i + ","
							+ "[ArticleDesignation2]=@ArticleDesignation2" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[ArticleReleaseStatus]=@ArticleReleaseStatus" + i + ","
							+ "[CsContact]=@CsContact" + i + ","
							+ "[CustomerName]=@CustomerName" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[EdiStandard]=@EdiStandard" + i + ","
							+ "[HeaderId]=@HeaderId" + i + ","
							+ "[StockQuantity]=@StockQuantity" + i + ","
							+ "[TotalCostsWithCu]=@TotalCostsWithCu" + i + ","
							+ "[TotalCostsWithoutCu]=@TotalCostsWithoutCu" + i + ","
							+ "[TotalSalesPrice]=@TotalSalesPrice" + i + ","
							+ "[UBG]=@UBG" + i + ","
							+ "[UnitSalesPrice]=@UnitSalesPrice" + i + ","
							+ "[VKE]=@VKE" + i + ","
							+ "[WarehouseId]=@WarehouseId" + i + ","
							+ "[WarehouseName]=@WarehouseName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleDesignation1" + i, item.ArticleDesignation1 == null ? (object)DBNull.Value : item.ArticleDesignation1);
						sqlCommand.Parameters.AddWithValue("ArticleDesignation2" + i, item.ArticleDesignation2 == null ? (object)DBNull.Value : item.ArticleDesignation2);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("ArticleReleaseStatus" + i, item.ArticleReleaseStatus == null ? (object)DBNull.Value : item.ArticleReleaseStatus);
						sqlCommand.Parameters.AddWithValue("CsContact" + i, item.CsContact == null ? (object)DBNull.Value : item.CsContact);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("EdiStandard" + i, item.EdiStandard == null ? (object)DBNull.Value : item.EdiStandard);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
						sqlCommand.Parameters.AddWithValue("StockQuantity" + i, item.StockQuantity == null ? (object)DBNull.Value : item.StockQuantity);
						sqlCommand.Parameters.AddWithValue("TotalCostsWithCu" + i, item.TotalCostsWithCu == null ? (object)DBNull.Value : item.TotalCostsWithCu);
						sqlCommand.Parameters.AddWithValue("TotalCostsWithoutCu" + i, item.TotalCostsWithoutCu == null ? (object)DBNull.Value : item.TotalCostsWithoutCu);
						sqlCommand.Parameters.AddWithValue("TotalSalesPrice" + i, item.TotalSalesPrice == null ? (object)DBNull.Value : item.TotalSalesPrice);
						sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
						sqlCommand.Parameters.AddWithValue("UnitSalesPrice" + i, item.UnitSalesPrice == null ? (object)DBNull.Value : item.UnitSalesPrice);
						sqlCommand.Parameters.AddWithValue("VKE" + i, item.VKE == null ? (object)DBNull.Value : item.VKE);
						sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
						sqlCommand.Parameters.AddWithValue("WarehouseName" + i, item.WarehouseName == null ? (object)DBNull.Value : item.WarehouseName);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
				string query = "DELETE FROM [stats].[__CRP_HistoryFG_Details] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [stats].[__CRP_HistoryFG_Details] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity GetWithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Details] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Details]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> GetWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> getWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [stats].[__CRP_HistoryFG_Details] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
		}

		public static long InsertWithTransaction(Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			long response = long.MinValue;


			string query = "INSERT INTO [stats].[__CRP_HistoryFG_Details] ([ArticleDesignation1],[ArticleDesignation2],[ArticleNumber],[ArticleReleaseStatus],[CsContact],[CustomerName],[CustomerNumber],[EdiStandard],[HeaderId],[StockQuantity],[TotalCostsWithCu],[TotalCostsWithoutCu],[TotalSalesPrice],[UBG],[UnitSalesPrice],[VKE],[WarehouseId],[WarehouseName]) OUTPUT INSERTED.[Id] VALUES (@ArticleDesignation1,@ArticleDesignation2,@ArticleNumber,@ArticleReleaseStatus,@CsContact,@CustomerName,@CustomerNumber,@EdiStandard,@HeaderId,@StockQuantity,@TotalCostsWithCu,@TotalCostsWithoutCu,@TotalSalesPrice,@UBG,@UnitSalesPrice,@VKE,@WarehouseId,@WarehouseName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleDesignation1", item.ArticleDesignation1 == null ? (object)DBNull.Value : item.ArticleDesignation1);
			sqlCommand.Parameters.AddWithValue("ArticleDesignation2", item.ArticleDesignation2 == null ? (object)DBNull.Value : item.ArticleDesignation2);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("ArticleReleaseStatus", item.ArticleReleaseStatus == null ? (object)DBNull.Value : item.ArticleReleaseStatus);
			sqlCommand.Parameters.AddWithValue("CsContact", item.CsContact == null ? (object)DBNull.Value : item.CsContact);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("EdiStandard", item.EdiStandard == null ? (object)DBNull.Value : item.EdiStandard);
			sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
			sqlCommand.Parameters.AddWithValue("StockQuantity", item.StockQuantity == null ? (object)DBNull.Value : item.StockQuantity);
			sqlCommand.Parameters.AddWithValue("TotalCostsWithCu", item.TotalCostsWithCu == null ? (object)DBNull.Value : item.TotalCostsWithCu);
			sqlCommand.Parameters.AddWithValue("TotalCostsWithoutCu", item.TotalCostsWithoutCu == null ? (object)DBNull.Value : item.TotalCostsWithoutCu);
			sqlCommand.Parameters.AddWithValue("TotalSalesPrice", item.TotalSalesPrice == null ? (object)DBNull.Value : item.TotalSalesPrice);
			sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
			sqlCommand.Parameters.AddWithValue("UnitSalesPrice", item.UnitSalesPrice == null ? (object)DBNull.Value : item.UnitSalesPrice);
			sqlCommand.Parameters.AddWithValue("VKE", item.VKE == null ? (object)DBNull.Value : item.VKE);
			sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
			sqlCommand.Parameters.AddWithValue("WarehouseName", item.WarehouseName == null ? (object)DBNull.Value : item.WarehouseName);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [stats].[__CRP_HistoryFG_Details] ([ArticleDesignation1],[ArticleDesignation2],[ArticleNumber],[ArticleReleaseStatus],[CsContact],[CustomerName],[CustomerNumber],[EdiStandard],[HeaderId],[StockQuantity],[TotalCostsWithCu],[TotalCostsWithoutCu],[TotalSalesPrice],[UBG],[UnitSalesPrice],[VKE],[WarehouseId],[WarehouseName]) VALUES ( "

						+ "@ArticleDesignation1" + i + ","
						+ "@ArticleDesignation2" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@ArticleReleaseStatus" + i + ","
						+ "@CsContact" + i + ","
						+ "@CustomerName" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@EdiStandard" + i + ","
						+ "@HeaderId" + i + ","
						+ "@StockQuantity" + i + ","
						+ "@TotalCostsWithCu" + i + ","
						+ "@TotalCostsWithoutCu" + i + ","
						+ "@TotalSalesPrice" + i + ","
						+ "@UBG" + i + ","
						+ "@UnitSalesPrice" + i + ","
						+ "@VKE" + i + ","
						+ "@WarehouseId" + i + ","
						+ "@WarehouseName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleDesignation1" + i, item.ArticleDesignation1 == null ? (object)DBNull.Value : item.ArticleDesignation1);
					sqlCommand.Parameters.AddWithValue("ArticleDesignation2" + i, item.ArticleDesignation2 == null ? (object)DBNull.Value : item.ArticleDesignation2);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("ArticleReleaseStatus" + i, item.ArticleReleaseStatus == null ? (object)DBNull.Value : item.ArticleReleaseStatus);
					sqlCommand.Parameters.AddWithValue("CsContact" + i, item.CsContact == null ? (object)DBNull.Value : item.CsContact);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("EdiStandard" + i, item.EdiStandard == null ? (object)DBNull.Value : item.EdiStandard);
					sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("StockQuantity" + i, item.StockQuantity == null ? (object)DBNull.Value : item.StockQuantity);
					sqlCommand.Parameters.AddWithValue("TotalCostsWithCu" + i, item.TotalCostsWithCu == null ? (object)DBNull.Value : item.TotalCostsWithCu);
					sqlCommand.Parameters.AddWithValue("TotalCostsWithoutCu" + i, item.TotalCostsWithoutCu == null ? (object)DBNull.Value : item.TotalCostsWithoutCu);
					sqlCommand.Parameters.AddWithValue("TotalSalesPrice" + i, item.TotalSalesPrice == null ? (object)DBNull.Value : item.TotalSalesPrice);
					sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
					sqlCommand.Parameters.AddWithValue("UnitSalesPrice" + i, item.UnitSalesPrice == null ? (object)DBNull.Value : item.UnitSalesPrice);
					sqlCommand.Parameters.AddWithValue("VKE" + i, item.VKE == null ? (object)DBNull.Value : item.VKE);
					sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseName" + i, item.WarehouseName == null ? (object)DBNull.Value : item.WarehouseName);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [stats].[__CRP_HistoryFG_Details] SET [ArticleDesignation1]=@ArticleDesignation1, [ArticleDesignation2]=@ArticleDesignation2, [ArticleNumber]=@ArticleNumber, [ArticleReleaseStatus]=@ArticleReleaseStatus, [CsContact]=@CsContact, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [EdiStandard]=@EdiStandard, [HeaderId]=@HeaderId, [StockQuantity]=@StockQuantity, [TotalCostsWithCu]=@TotalCostsWithCu, [TotalCostsWithoutCu]=@TotalCostsWithoutCu, [TotalSalesPrice]=@TotalSalesPrice, [UBG]=@UBG, [UnitSalesPrice]=@UnitSalesPrice, [VKE]=@VKE, [WarehouseId]=@WarehouseId, [WarehouseName]=@WarehouseName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleDesignation1", item.ArticleDesignation1 == null ? (object)DBNull.Value : item.ArticleDesignation1);
			sqlCommand.Parameters.AddWithValue("ArticleDesignation2", item.ArticleDesignation2 == null ? (object)DBNull.Value : item.ArticleDesignation2);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("ArticleReleaseStatus", item.ArticleReleaseStatus == null ? (object)DBNull.Value : item.ArticleReleaseStatus);
			sqlCommand.Parameters.AddWithValue("CsContact", item.CsContact == null ? (object)DBNull.Value : item.CsContact);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("EdiStandard", item.EdiStandard == null ? (object)DBNull.Value : item.EdiStandard);
			sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
			sqlCommand.Parameters.AddWithValue("StockQuantity", item.StockQuantity == null ? (object)DBNull.Value : item.StockQuantity);
			sqlCommand.Parameters.AddWithValue("TotalCostsWithCu", item.TotalCostsWithCu == null ? (object)DBNull.Value : item.TotalCostsWithCu);
			sqlCommand.Parameters.AddWithValue("TotalCostsWithoutCu", item.TotalCostsWithoutCu == null ? (object)DBNull.Value : item.TotalCostsWithoutCu);
			sqlCommand.Parameters.AddWithValue("TotalSalesPrice", item.TotalSalesPrice == null ? (object)DBNull.Value : item.TotalSalesPrice);
			sqlCommand.Parameters.AddWithValue("UBG", item.UBG == null ? (object)DBNull.Value : item.UBG);
			sqlCommand.Parameters.AddWithValue("UnitSalesPrice", item.UnitSalesPrice == null ? (object)DBNull.Value : item.UnitSalesPrice);
			sqlCommand.Parameters.AddWithValue("VKE", item.VKE == null ? (object)DBNull.Value : item.VKE);
			sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
			sqlCommand.Parameters.AddWithValue("WarehouseName", item.WarehouseName == null ? (object)DBNull.Value : item.WarehouseName);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [stats].[__CRP_HistoryFG_Details] SET "

					+ "[ArticleDesignation1]=@ArticleDesignation1" + i + ","
					+ "[ArticleDesignation2]=@ArticleDesignation2" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[ArticleReleaseStatus]=@ArticleReleaseStatus" + i + ","
					+ "[CsContact]=@CsContact" + i + ","
					+ "[CustomerName]=@CustomerName" + i + ","
					+ "[CustomerNumber]=@CustomerNumber" + i + ","
					+ "[EdiStandard]=@EdiStandard" + i + ","
					+ "[HeaderId]=@HeaderId" + i + ","
					+ "[StockQuantity]=@StockQuantity" + i + ","
					+ "[TotalCostsWithCu]=@TotalCostsWithCu" + i + ","
					+ "[TotalCostsWithoutCu]=@TotalCostsWithoutCu" + i + ","
					+ "[TotalSalesPrice]=@TotalSalesPrice" + i + ","
					+ "[UBG]=@UBG" + i + ","
					+ "[UnitSalesPrice]=@UnitSalesPrice" + i + ","
					+ "[VKE]=@VKE" + i + ","
					+ "[WarehouseId]=@WarehouseId" + i + ","
					+ "[WarehouseName]=@WarehouseName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleDesignation1" + i, item.ArticleDesignation1 == null ? (object)DBNull.Value : item.ArticleDesignation1);
					sqlCommand.Parameters.AddWithValue("ArticleDesignation2" + i, item.ArticleDesignation2 == null ? (object)DBNull.Value : item.ArticleDesignation2);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("ArticleReleaseStatus" + i, item.ArticleReleaseStatus == null ? (object)DBNull.Value : item.ArticleReleaseStatus);
					sqlCommand.Parameters.AddWithValue("CsContact" + i, item.CsContact == null ? (object)DBNull.Value : item.CsContact);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("EdiStandard" + i, item.EdiStandard == null ? (object)DBNull.Value : item.EdiStandard);
					sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("StockQuantity" + i, item.StockQuantity == null ? (object)DBNull.Value : item.StockQuantity);
					sqlCommand.Parameters.AddWithValue("TotalCostsWithCu" + i, item.TotalCostsWithCu == null ? (object)DBNull.Value : item.TotalCostsWithCu);
					sqlCommand.Parameters.AddWithValue("TotalCostsWithoutCu" + i, item.TotalCostsWithoutCu == null ? (object)DBNull.Value : item.TotalCostsWithoutCu);
					sqlCommand.Parameters.AddWithValue("TotalSalesPrice" + i, item.TotalSalesPrice == null ? (object)DBNull.Value : item.TotalSalesPrice);
					sqlCommand.Parameters.AddWithValue("UBG" + i, item.UBG == null ? (object)DBNull.Value : item.UBG);
					sqlCommand.Parameters.AddWithValue("UnitSalesPrice" + i, item.UnitSalesPrice == null ? (object)DBNull.Value : item.UnitSalesPrice);
					sqlCommand.Parameters.AddWithValue("VKE" + i, item.VKE == null ? (object)DBNull.Value : item.VKE);
					sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseName" + i, item.WarehouseName == null ? (object)DBNull.Value : item.WarehouseName);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [stats].[__CRP_HistoryFG_Details] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


			return results;
		}
		public static int DeleteWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
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
		private static int deleteWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
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

				string query = "DELETE FROM [stats].[__CRP_HistoryFG_Details] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> GetHistoryFGDetailsData(string filterSearch, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			string paging = "";
			string sorting = "";

			if(dataPaging != null && (0 >= dataPaging.RequestRows || dataPaging.RequestRows > 100))
			{
				dataPaging.RequestRows = 100;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Details]";

				if(filterSearch != null && filterSearch.Length > 0)
				{
					query += $" WHERE [CustomerNumber] LIKE '%{filterSearch}%' OR [CustomerName] LIKE '%{filterSearch}%' ";
				}
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [ArticleNumber]";
				}
				if(paging is not null)
				{
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();

			}
		}
		public static int CountHistoryFGDetailsData(Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = @$"SELECT Count(*) FROM[stats].[__CRP_HistoryFG_Details]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity> GetWithSearchPosition(int id, string filterSearch, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			string paging = "";
			string sorting = "";

			if(dataPaging != null && (0 >= dataPaging.RequestRows || dataPaging.RequestRows > 100))
			{
				dataPaging.RequestRows = 100;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Details]  WHERE [HeaderId]=@Id";

				if(filterSearch != null && filterSearch.Length > 0)
				{
					query += $" And ( [CustomerName] LIKE '{filterSearch.Trim().ToLower()}%'  OR [CsContact] LIKE '{filterSearch.Trim()}%'  OR [WarehouseName] LIKE '{filterSearch.Trim()}%' OR [ArticleNumber] LIKE '{filterSearch.Trim()}%' OR [ArticleDesignation1] LIKE '{filterSearch.Trim()}%' OR [ArticleDesignation2] LIKE '{filterSearch.Trim()}%')";
				}
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [ArticleNumber]";
				}
				if(paging is not null)
				{
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();

			}
		}
		public static int CountWithSearchPosition(int id, string? filterSearch)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = @$"SELECT Count(*) FROM [stats].[__CRP_HistoryFG_Details]  WHERE [HeaderId]=@Id";

				if(filterSearch != null && filterSearch.Length > 0)
				{
					query += $" And ( [CustomerName] LIKE '%{filterSearch}%'  OR [CsContact] LIKE '%{filterSearch}%'  OR [WarehouseName] LIKE '%{filterSearch}%' OR [ArticleNumber] LIKE '%{filterSearch}%' OR [ArticleDesignation1] LIKE '%{filterSearch}%' OR [ArticleDesignation2] LIKE '%{filterSearch}%')";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				sqlCommand.CommandTimeout = 300;

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}
		#endregion Custom Methods
	}
}