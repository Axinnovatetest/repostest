using Infrastructure.Data.Entities.Tables.CTS;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class InsideSalesChecksAccess
	{
		#region Default Methods
		public static InsideSalesChecksEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesChecks] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new InsideSalesChecksEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<InsideSalesChecksEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesChecks]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesChecksEntity>();
			}
		}
		public static List<InsideSalesChecksEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<InsideSalesChecksEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<InsideSalesChecksEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<InsideSalesChecksEntity>();
		}
		private static List<InsideSalesChecksEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_InsideSalesChecks] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksEntity(x)).ToList();
				}
				else
				{
					return new List<InsideSalesChecksEntity>();
				}
			}
			return new List<InsideSalesChecksEntity>();
		}

		public static int Insert(InsideSalesChecksEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTS_InsideSalesChecks] ([ArticleId],[ArticleNumber],[CheckCRP],[CheckCRPComments],[CheckCRPDate],[CheckCRPUserId],[CheckCRPUserName],[CheckFST],[CheckFSTComments],[CheckFSTDate],[CheckFSTUserId],[CheckFSTUserName],[CheckINS],[CheckINSComments],[CheckINSDate],[CheckINSUserId],[CheckINSUserName],[CheckPRS],[CheckPRSComments],[CheckPRSDate],[CheckPRSUserId],[CheckPRSUserName],[CheckStock],[CheckStockComments],[CheckStockDate],[CheckStockUserId],[CheckStockUserName],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[IsCheckedStock],[OrderDeliveryDate],[OrderId],[OrderNumber],[OrderOpenQuantity],[OrderPositionId],[RevertArchiveDate],[RevertArchiveUserId],[RevertArchiveUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@CheckCRP,@CheckCRPComments,@CheckCRPDate,@CheckCRPUserId,@CheckCRPUserName,@CheckFST,@CheckFSTComments,@CheckFSTDate,@CheckFSTUserId,@CheckFSTUserName,@CheckINS,@CheckINSComments,@CheckINSDate,@CheckINSUserId,@CheckINSUserName,@CheckPRS,@CheckPRSComments,@CheckPRSDate,@CheckPRSUserId,@CheckPRSUserName,@CheckStock,@CheckStockComments,@CheckStockDate,@CheckStockUserId,@CheckStockUserName,@CustomerName,@CustomerNumber,@CustomerOrderNumber,@IsCheckedStock,@OrderDeliveryDate,@OrderId,@OrderNumber,@OrderOpenQuantity,@OrderPositionId,@RevertArchiveDate,@RevertArchiveUserId,@RevertArchiveUserName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CheckCRP", item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
					sqlCommand.Parameters.AddWithValue("CheckCRPComments", item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
					sqlCommand.Parameters.AddWithValue("CheckCRPDate", item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserId", item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserName", item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
					sqlCommand.Parameters.AddWithValue("CheckFST", item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
					sqlCommand.Parameters.AddWithValue("CheckFSTComments", item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
					sqlCommand.Parameters.AddWithValue("CheckFSTDate", item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserId", item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserName", item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
					sqlCommand.Parameters.AddWithValue("CheckINS", item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
					sqlCommand.Parameters.AddWithValue("CheckINSComments", item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
					sqlCommand.Parameters.AddWithValue("CheckINSDate", item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
					sqlCommand.Parameters.AddWithValue("CheckINSUserId", item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
					sqlCommand.Parameters.AddWithValue("CheckINSUserName", item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
					sqlCommand.Parameters.AddWithValue("CheckPRS", item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
					sqlCommand.Parameters.AddWithValue("CheckPRSComments", item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
					sqlCommand.Parameters.AddWithValue("CheckPRSDate", item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSUserId", item.CheckPRSUserId == null ? (object)DBNull.Value : item.CheckPRSUserId);
					sqlCommand.Parameters.AddWithValue("CheckPRSUserName", item.CheckPRSUserName == null ? (object)DBNull.Value : item.CheckPRSUserName);
					sqlCommand.Parameters.AddWithValue("CheckStock", item.CheckStock == null ? (object)DBNull.Value : item.CheckStock);
					sqlCommand.Parameters.AddWithValue("CheckStockComments", item.CheckStockComments == null ? (object)DBNull.Value : item.CheckStockComments);
					sqlCommand.Parameters.AddWithValue("CheckStockDate", item.CheckStockDate == null ? (object)DBNull.Value : item.CheckStockDate);
					sqlCommand.Parameters.AddWithValue("CheckStockUserId", item.CheckStockUserId == null ? (object)DBNull.Value : item.CheckStockUserId);
					sqlCommand.Parameters.AddWithValue("CheckStockUserName", item.CheckStockUserName == null ? (object)DBNull.Value : item.CheckStockUserName);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOrderNumber", item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
					sqlCommand.Parameters.AddWithValue("IsCheckedStock", item.IsCheckedStock == null ? (object)DBNull.Value : item.IsCheckedStock);
					sqlCommand.Parameters.AddWithValue("OrderDeliveryDate", item.OrderDeliveryDate == null ? (object)DBNull.Value : item.OrderDeliveryDate);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderOpenQuantity", item.OrderOpenQuantity == null ? (object)DBNull.Value : item.OrderOpenQuantity);
					sqlCommand.Parameters.AddWithValue("OrderPositionId", item.OrderPositionId == null ? (object)DBNull.Value : item.OrderPositionId);
					sqlCommand.Parameters.AddWithValue("RevertArchiveDate", item.RevertArchiveDate == null ? (object)DBNull.Value : item.RevertArchiveDate);
					sqlCommand.Parameters.AddWithValue("RevertArchiveUserId", item.RevertArchiveUserId == null ? (object)DBNull.Value : item.RevertArchiveUserId);
					sqlCommand.Parameters.AddWithValue("RevertArchiveUserName", item.RevertArchiveUserName == null ? (object)DBNull.Value : item.RevertArchiveUserName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<InsideSalesChecksEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 40; // Nb params per query
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
		private static int insert(List<InsideSalesChecksEntity> items)
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
						query += " INSERT INTO [__CTS_InsideSalesChecks] ([ArticleId],[ArticleNumber],[CheckCRP],[CheckCRPComments],[CheckCRPDate],[CheckCRPUserId],[CheckCRPUserName],[CheckFST],[CheckFSTComments],[CheckFSTDate],[CheckFSTUserId],[CheckFSTUserName],[CheckINS],[CheckINSComments],[CheckINSDate],[CheckINSUserId],[CheckINSUserName],[CheckPRS],[CheckPRSComments],[CheckPRSDate],[CheckPRSUserId],[CheckPRSUserName],[CheckStock],[CheckStockComments],[CheckStockDate],[CheckStockUserId],[CheckStockUserName],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[IsCheckedStock],[OrderDeliveryDate],[OrderId],[OrderNumber],[OrderOpenQuantity],[OrderPositionId],[RevertArchiveDate],[RevertArchiveUserId],[RevertArchiveUserName]) VALUES ( "

							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@CheckCRP" + i + ","
							+ "@CheckCRPComments" + i + ","
							+ "@CheckCRPDate" + i + ","
							+ "@CheckCRPUserId" + i + ","
							+ "@CheckCRPUserName" + i + ","
							+ "@CheckFST" + i + ","
							+ "@CheckFSTComments" + i + ","
							+ "@CheckFSTDate" + i + ","
							+ "@CheckFSTUserId" + i + ","
							+ "@CheckFSTUserName" + i + ","
							+ "@CheckINS" + i + ","
							+ "@CheckINSComments" + i + ","
							+ "@CheckINSDate" + i + ","
							+ "@CheckINSUserId" + i + ","
							+ "@CheckINSUserName" + i + ","
							+ "@CheckPRS" + i + ","
							+ "@CheckPRSComments" + i + ","
							+ "@CheckPRSDate" + i + ","
							+ "@CheckPRSUserId" + i + ","
							+ "@CheckPRSUserName" + i + ","
							+ "@CheckStock" + i + ","
							+ "@CheckStockComments" + i + ","
							+ "@CheckStockDate" + i + ","
							+ "@CheckStockUserId" + i + ","
							+ "@CheckStockUserName" + i + ","
							+ "@CustomerName" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@CustomerOrderNumber" + i + ","
							+ "@IsCheckedStock" + i + ","
							+ "@OrderDeliveryDate" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderNumber" + i + ","
							+ "@OrderOpenQuantity" + i + ","
							+ "@OrderPositionId" + i + ","
							+ "@RevertArchiveDate" + i + ","
							+ "@RevertArchiveUserId" + i + ","
							+ "@RevertArchiveUserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("CheckCRP" + i, item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
						sqlCommand.Parameters.AddWithValue("CheckCRPComments" + i, item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
						sqlCommand.Parameters.AddWithValue("CheckCRPDate" + i, item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
						sqlCommand.Parameters.AddWithValue("CheckCRPUserId" + i, item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
						sqlCommand.Parameters.AddWithValue("CheckCRPUserName" + i, item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
						sqlCommand.Parameters.AddWithValue("CheckFST" + i, item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
						sqlCommand.Parameters.AddWithValue("CheckFSTComments" + i, item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
						sqlCommand.Parameters.AddWithValue("CheckFSTDate" + i, item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
						sqlCommand.Parameters.AddWithValue("CheckFSTUserId" + i, item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
						sqlCommand.Parameters.AddWithValue("CheckFSTUserName" + i, item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
						sqlCommand.Parameters.AddWithValue("CheckINS" + i, item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
						sqlCommand.Parameters.AddWithValue("CheckINSComments" + i, item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
						sqlCommand.Parameters.AddWithValue("CheckINSDate" + i, item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
						sqlCommand.Parameters.AddWithValue("CheckINSUserId" + i, item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
						sqlCommand.Parameters.AddWithValue("CheckINSUserName" + i, item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
						sqlCommand.Parameters.AddWithValue("CheckPRS" + i, item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
						sqlCommand.Parameters.AddWithValue("CheckPRSComments" + i, item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
						sqlCommand.Parameters.AddWithValue("CheckPRSDate" + i, item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
						sqlCommand.Parameters.AddWithValue("CheckPRSUserId" + i, item.CheckPRSUserId == null ? (object)DBNull.Value : item.CheckPRSUserId);
						sqlCommand.Parameters.AddWithValue("CheckPRSUserName" + i, item.CheckPRSUserName == null ? (object)DBNull.Value : item.CheckPRSUserName);
						sqlCommand.Parameters.AddWithValue("CheckStock" + i, item.CheckStock == null ? (object)DBNull.Value : item.CheckStock);
						sqlCommand.Parameters.AddWithValue("CheckStockComments" + i, item.CheckStockComments == null ? (object)DBNull.Value : item.CheckStockComments);
						sqlCommand.Parameters.AddWithValue("CheckStockDate" + i, item.CheckStockDate == null ? (object)DBNull.Value : item.CheckStockDate);
						sqlCommand.Parameters.AddWithValue("CheckStockUserId" + i, item.CheckStockUserId == null ? (object)DBNull.Value : item.CheckStockUserId);
						sqlCommand.Parameters.AddWithValue("CheckStockUserName" + i, item.CheckStockUserName == null ? (object)DBNull.Value : item.CheckStockUserName);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerOrderNumber" + i, item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
						sqlCommand.Parameters.AddWithValue("IsCheckedStock" + i, item.IsCheckedStock == null ? (object)DBNull.Value : item.IsCheckedStock);
						sqlCommand.Parameters.AddWithValue("OrderDeliveryDate" + i, item.OrderDeliveryDate == null ? (object)DBNull.Value : item.OrderDeliveryDate);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderOpenQuantity" + i, item.OrderOpenQuantity == null ? (object)DBNull.Value : item.OrderOpenQuantity);
						sqlCommand.Parameters.AddWithValue("OrderPositionId" + i, item.OrderPositionId == null ? (object)DBNull.Value : item.OrderPositionId);
						sqlCommand.Parameters.AddWithValue("RevertArchiveDate" + i, item.RevertArchiveDate == null ? (object)DBNull.Value : item.RevertArchiveDate);
						sqlCommand.Parameters.AddWithValue("RevertArchiveUserId" + i, item.RevertArchiveUserId == null ? (object)DBNull.Value : item.RevertArchiveUserId);
						sqlCommand.Parameters.AddWithValue("RevertArchiveUserName" + i, item.RevertArchiveUserName == null ? (object)DBNull.Value : item.RevertArchiveUserName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(InsideSalesChecksEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesChecks] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [CheckCRP]=@CheckCRP, [CheckCRPComments]=@CheckCRPComments, [CheckCRPDate]=@CheckCRPDate, [CheckCRPUserId]=@CheckCRPUserId, [CheckCRPUserName]=@CheckCRPUserName, [CheckFST]=@CheckFST, [CheckFSTComments]=@CheckFSTComments, [CheckFSTDate]=@CheckFSTDate, [CheckFSTUserId]=@CheckFSTUserId, [CheckFSTUserName]=@CheckFSTUserName, [CheckINS]=@CheckINS, [CheckINSComments]=@CheckINSComments, [CheckINSDate]=@CheckINSDate, [CheckINSUserId]=@CheckINSUserId, [CheckINSUserName]=@CheckINSUserName, [CheckPRS]=@CheckPRS, [CheckPRSComments]=@CheckPRSComments, [CheckPRSDate]=@CheckPRSDate, [CheckPRSUserId]=@CheckPRSUserId, [CheckPRSUserName]=@CheckPRSUserName, [CheckStock]=@CheckStock, [CheckStockComments]=@CheckStockComments, [CheckStockDate]=@CheckStockDate, [CheckStockUserId]=@CheckStockUserId, [CheckStockUserName]=@CheckStockUserName, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [CustomerOrderNumber]=@CustomerOrderNumber, [IsCheckedStock]=@IsCheckedStock, [OrderDeliveryDate]=@OrderDeliveryDate, [OrderId]=@OrderId, [OrderNumber]=@OrderNumber, [OrderOpenQuantity]=@OrderOpenQuantity, [OrderPositionId]=@OrderPositionId, [RevertArchiveDate]=@RevertArchiveDate, [RevertArchiveUserId]=@RevertArchiveUserId, [RevertArchiveUserName]=@RevertArchiveUserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("CheckCRP", item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
				sqlCommand.Parameters.AddWithValue("CheckCRPComments", item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
				sqlCommand.Parameters.AddWithValue("CheckCRPDate", item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
				sqlCommand.Parameters.AddWithValue("CheckCRPUserId", item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
				sqlCommand.Parameters.AddWithValue("CheckCRPUserName", item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
				sqlCommand.Parameters.AddWithValue("CheckFST", item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
				sqlCommand.Parameters.AddWithValue("CheckFSTComments", item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
				sqlCommand.Parameters.AddWithValue("CheckFSTDate", item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
				sqlCommand.Parameters.AddWithValue("CheckFSTUserId", item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
				sqlCommand.Parameters.AddWithValue("CheckFSTUserName", item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
				sqlCommand.Parameters.AddWithValue("CheckINS", item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
				sqlCommand.Parameters.AddWithValue("CheckINSComments", item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
				sqlCommand.Parameters.AddWithValue("CheckINSDate", item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
				sqlCommand.Parameters.AddWithValue("CheckINSUserId", item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
				sqlCommand.Parameters.AddWithValue("CheckINSUserName", item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
				sqlCommand.Parameters.AddWithValue("CheckPRS", item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
				sqlCommand.Parameters.AddWithValue("CheckPRSComments", item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
				sqlCommand.Parameters.AddWithValue("CheckPRSDate", item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
				sqlCommand.Parameters.AddWithValue("CheckPRSUserId", item.CheckPRSUserId == null ? (object)DBNull.Value : item.CheckPRSUserId);
				sqlCommand.Parameters.AddWithValue("CheckPRSUserName", item.CheckPRSUserName == null ? (object)DBNull.Value : item.CheckPRSUserName);
				sqlCommand.Parameters.AddWithValue("CheckStock", item.CheckStock == null ? (object)DBNull.Value : item.CheckStock);
				sqlCommand.Parameters.AddWithValue("CheckStockComments", item.CheckStockComments == null ? (object)DBNull.Value : item.CheckStockComments);
				sqlCommand.Parameters.AddWithValue("CheckStockDate", item.CheckStockDate == null ? (object)DBNull.Value : item.CheckStockDate);
				sqlCommand.Parameters.AddWithValue("CheckStockUserId", item.CheckStockUserId == null ? (object)DBNull.Value : item.CheckStockUserId);
				sqlCommand.Parameters.AddWithValue("CheckStockUserName", item.CheckStockUserName == null ? (object)DBNull.Value : item.CheckStockUserName);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerOrderNumber", item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
				sqlCommand.Parameters.AddWithValue("IsCheckedStock", item.IsCheckedStock == null ? (object)DBNull.Value : item.IsCheckedStock);
				sqlCommand.Parameters.AddWithValue("OrderDeliveryDate", item.OrderDeliveryDate == null ? (object)DBNull.Value : item.OrderDeliveryDate);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderOpenQuantity", item.OrderOpenQuantity == null ? (object)DBNull.Value : item.OrderOpenQuantity);
				sqlCommand.Parameters.AddWithValue("OrderPositionId", item.OrderPositionId == null ? (object)DBNull.Value : item.OrderPositionId);
				sqlCommand.Parameters.AddWithValue("RevertArchiveDate", item.RevertArchiveDate == null ? (object)DBNull.Value : item.RevertArchiveDate);
				sqlCommand.Parameters.AddWithValue("RevertArchiveUserId", item.RevertArchiveUserId == null ? (object)DBNull.Value : item.RevertArchiveUserId);
				sqlCommand.Parameters.AddWithValue("RevertArchiveUserName", item.RevertArchiveUserName == null ? (object)DBNull.Value : item.RevertArchiveUserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<InsideSalesChecksEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 40; // Nb params per query
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
		private static int update(List<InsideSalesChecksEntity> items)
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
						query += " UPDATE [__CTS_InsideSalesChecks] SET "

							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[CheckCRP]=@CheckCRP" + i + ","
							+ "[CheckCRPComments]=@CheckCRPComments" + i + ","
							+ "[CheckCRPDate]=@CheckCRPDate" + i + ","
							+ "[CheckCRPUserId]=@CheckCRPUserId" + i + ","
							+ "[CheckCRPUserName]=@CheckCRPUserName" + i + ","
							+ "[CheckFST]=@CheckFST" + i + ","
							+ "[CheckFSTComments]=@CheckFSTComments" + i + ","
							+ "[CheckFSTDate]=@CheckFSTDate" + i + ","
							+ "[CheckFSTUserId]=@CheckFSTUserId" + i + ","
							+ "[CheckFSTUserName]=@CheckFSTUserName" + i + ","
							+ "[CheckINS]=@CheckINS" + i + ","
							+ "[CheckINSComments]=@CheckINSComments" + i + ","
							+ "[CheckINSDate]=@CheckINSDate" + i + ","
							+ "[CheckINSUserId]=@CheckINSUserId" + i + ","
							+ "[CheckINSUserName]=@CheckINSUserName" + i + ","
							+ "[CheckPRS]=@CheckPRS" + i + ","
							+ "[CheckPRSComments]=@CheckPRSComments" + i + ","
							+ "[CheckPRSDate]=@CheckPRSDate" + i + ","
							+ "[CheckPRSUserId]=@CheckPRSUserId" + i + ","
							+ "[CheckPRSUserName]=@CheckPRSUserName" + i + ","
							+ "[CheckStock]=@CheckStock" + i + ","
							+ "[CheckStockComments]=@CheckStockComments" + i + ","
							+ "[CheckStockDate]=@CheckStockDate" + i + ","
							+ "[CheckStockUserId]=@CheckStockUserId" + i + ","
							+ "[CheckStockUserName]=@CheckStockUserName" + i + ","
							+ "[CustomerName]=@CustomerName" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[CustomerOrderNumber]=@CustomerOrderNumber" + i + ","
							+ "[IsCheckedStock]=@IsCheckedStock" + i + ","
							+ "[OrderDeliveryDate]=@OrderDeliveryDate" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderNumber]=@OrderNumber" + i + ","
							+ "[OrderOpenQuantity]=@OrderOpenQuantity" + i + ","
							+ "[OrderPositionId]=@OrderPositionId" + i + ","
							+ "[RevertArchiveDate]=@RevertArchiveDate" + i + ","
							+ "[RevertArchiveUserId]=@RevertArchiveUserId" + i + ","
							+ "[RevertArchiveUserName]=@RevertArchiveUserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("CheckCRP" + i, item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
						sqlCommand.Parameters.AddWithValue("CheckCRPComments" + i, item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
						sqlCommand.Parameters.AddWithValue("CheckCRPDate" + i, item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
						sqlCommand.Parameters.AddWithValue("CheckCRPUserId" + i, item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
						sqlCommand.Parameters.AddWithValue("CheckCRPUserName" + i, item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
						sqlCommand.Parameters.AddWithValue("CheckFST" + i, item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
						sqlCommand.Parameters.AddWithValue("CheckFSTComments" + i, item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
						sqlCommand.Parameters.AddWithValue("CheckFSTDate" + i, item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
						sqlCommand.Parameters.AddWithValue("CheckFSTUserId" + i, item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
						sqlCommand.Parameters.AddWithValue("CheckFSTUserName" + i, item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
						sqlCommand.Parameters.AddWithValue("CheckINS" + i, item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
						sqlCommand.Parameters.AddWithValue("CheckINSComments" + i, item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
						sqlCommand.Parameters.AddWithValue("CheckINSDate" + i, item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
						sqlCommand.Parameters.AddWithValue("CheckINSUserId" + i, item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
						sqlCommand.Parameters.AddWithValue("CheckINSUserName" + i, item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
						sqlCommand.Parameters.AddWithValue("CheckPRS" + i, item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
						sqlCommand.Parameters.AddWithValue("CheckPRSComments" + i, item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
						sqlCommand.Parameters.AddWithValue("CheckPRSDate" + i, item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
						sqlCommand.Parameters.AddWithValue("CheckPRSUserId" + i, item.CheckPRSUserId == null ? (object)DBNull.Value : item.CheckPRSUserId);
						sqlCommand.Parameters.AddWithValue("CheckPRSUserName" + i, item.CheckPRSUserName == null ? (object)DBNull.Value : item.CheckPRSUserName);
						sqlCommand.Parameters.AddWithValue("CheckStock" + i, item.CheckStock == null ? (object)DBNull.Value : item.CheckStock);
						sqlCommand.Parameters.AddWithValue("CheckStockComments" + i, item.CheckStockComments == null ? (object)DBNull.Value : item.CheckStockComments);
						sqlCommand.Parameters.AddWithValue("CheckStockDate" + i, item.CheckStockDate == null ? (object)DBNull.Value : item.CheckStockDate);
						sqlCommand.Parameters.AddWithValue("CheckStockUserId" + i, item.CheckStockUserId == null ? (object)DBNull.Value : item.CheckStockUserId);
						sqlCommand.Parameters.AddWithValue("CheckStockUserName" + i, item.CheckStockUserName == null ? (object)DBNull.Value : item.CheckStockUserName);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerOrderNumber" + i, item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
						sqlCommand.Parameters.AddWithValue("IsCheckedStock" + i, item.IsCheckedStock == null ? (object)DBNull.Value : item.IsCheckedStock);
						sqlCommand.Parameters.AddWithValue("OrderDeliveryDate" + i, item.OrderDeliveryDate == null ? (object)DBNull.Value : item.OrderDeliveryDate);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderOpenQuantity" + i, item.OrderOpenQuantity == null ? (object)DBNull.Value : item.OrderOpenQuantity);
						sqlCommand.Parameters.AddWithValue("OrderPositionId" + i, item.OrderPositionId == null ? (object)DBNull.Value : item.OrderPositionId);
						sqlCommand.Parameters.AddWithValue("RevertArchiveDate" + i, item.RevertArchiveDate == null ? (object)DBNull.Value : item.RevertArchiveDate);
						sqlCommand.Parameters.AddWithValue("RevertArchiveUserId" + i, item.RevertArchiveUserId == null ? (object)DBNull.Value : item.RevertArchiveUserId);
						sqlCommand.Parameters.AddWithValue("RevertArchiveUserName" + i, item.RevertArchiveUserName == null ? (object)DBNull.Value : item.RevertArchiveUserName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int updateStockComment(int saleId, string comment)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesChecks] SET  [CheckStockComments]=@stockComment WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("stockComment", comment == null ? (object)DBNull.Value : comment);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int updateFSTComment(int saleId, string comment)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesChecks] SET  [CheckFSTComments]=@fstComment WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("fstComment", comment == null ? (object)DBNull.Value : comment);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int updatePRSComment(int saleId, string comment)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesChecks] SET [CheckPRSComments]=@prsComment WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("prsComment", comment == null ? (object)DBNull.Value : comment);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int updateCRPComment(int saleId, string comment)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesChecks] SET [CheckCRPComments]=@crpComment WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("crpComment", comment == null ? (object)DBNull.Value : comment);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int updateINSComment(int saleId, string comment)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesChecks] SET [CheckINSComments]=@insComment WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("insComment", comment == null ? (object)DBNull.Value : comment);


				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__CTS_InsideSalesChecks] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__CTS_InsideSalesChecks] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static InsideSalesChecksEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_InsideSalesChecks] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new InsideSalesChecksEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<InsideSalesChecksEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_InsideSalesChecks]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesChecksEntity>();
			}
		}
		public static List<InsideSalesChecksEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<InsideSalesChecksEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<InsideSalesChecksEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<InsideSalesChecksEntity>();
		}
		private static List<InsideSalesChecksEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__CTS_InsideSalesChecks] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksEntity(x)).ToList();
				}
				else
				{
					return new List<InsideSalesChecksEntity>();
				}
			}
			return new List<InsideSalesChecksEntity>();
		}

		public static int InsertWithTransaction(InsideSalesChecksEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__CTS_InsideSalesChecks] ([ArticleId],[ArticleNumber],[CheckCRP],[CheckCRPComments],[CheckCRPDate],[CheckCRPUserId],[CheckCRPUserName],[CheckFST],[CheckFSTComments],[CheckFSTDate],[CheckFSTUserId],[CheckFSTUserName],[CheckINS],[CheckINSComments],[CheckINSDate],[CheckINSUserId],[CheckINSUserName],[CheckPRS],[CheckPRSComments],[CheckPRSDate],[CheckPRSUserId],[CheckPRSUserName],[CheckStock],[CheckStockComments],[CheckStockDate],[CheckStockUserId],[CheckStockUserName],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[IsCheckedStock],[OrderDeliveryDate],[OrderId],[OrderNumber],[OrderOpenQuantity],[OrderPositionId],[RevertArchiveDate],[RevertArchiveUserId],[RevertArchiveUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@CheckCRP,@CheckCRPComments,@CheckCRPDate,@CheckCRPUserId,@CheckCRPUserName,@CheckFST,@CheckFSTComments,@CheckFSTDate,@CheckFSTUserId,@CheckFSTUserName,@CheckINS,@CheckINSComments,@CheckINSDate,@CheckINSUserId,@CheckINSUserName,@CheckPRS,@CheckPRSComments,@CheckPRSDate,@CheckPRSUserId,@CheckPRSUserName,@CheckStock,@CheckStockComments,@CheckStockDate,@CheckStockUserId,@CheckStockUserName,@CustomerName,@CustomerNumber,@CustomerOrderNumber,@IsCheckedStock,@OrderDeliveryDate,@OrderId,@OrderNumber,@OrderOpenQuantity,@OrderPositionId,@RevertArchiveDate,@RevertArchiveUserId,@RevertArchiveUserName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("CheckCRP", item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
			sqlCommand.Parameters.AddWithValue("CheckCRPComments", item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
			sqlCommand.Parameters.AddWithValue("CheckCRPDate", item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
			sqlCommand.Parameters.AddWithValue("CheckCRPUserId", item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
			sqlCommand.Parameters.AddWithValue("CheckCRPUserName", item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
			sqlCommand.Parameters.AddWithValue("CheckFST", item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
			sqlCommand.Parameters.AddWithValue("CheckFSTComments", item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
			sqlCommand.Parameters.AddWithValue("CheckFSTDate", item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
			sqlCommand.Parameters.AddWithValue("CheckFSTUserId", item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
			sqlCommand.Parameters.AddWithValue("CheckFSTUserName", item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
			sqlCommand.Parameters.AddWithValue("CheckINS", item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
			sqlCommand.Parameters.AddWithValue("CheckINSComments", item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
			sqlCommand.Parameters.AddWithValue("CheckINSDate", item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
			sqlCommand.Parameters.AddWithValue("CheckINSUserId", item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
			sqlCommand.Parameters.AddWithValue("CheckINSUserName", item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
			sqlCommand.Parameters.AddWithValue("CheckPRS", item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
			sqlCommand.Parameters.AddWithValue("CheckPRSComments", item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
			sqlCommand.Parameters.AddWithValue("CheckPRSDate", item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
			sqlCommand.Parameters.AddWithValue("CheckPRSUserId", item.CheckPRSUserId == null ? (object)DBNull.Value : item.CheckPRSUserId);
			sqlCommand.Parameters.AddWithValue("CheckPRSUserName", item.CheckPRSUserName == null ? (object)DBNull.Value : item.CheckPRSUserName);
			sqlCommand.Parameters.AddWithValue("CheckStock", item.CheckStock == null ? (object)DBNull.Value : item.CheckStock);
			sqlCommand.Parameters.AddWithValue("CheckStockComments", item.CheckStockComments == null ? (object)DBNull.Value : item.CheckStockComments);
			sqlCommand.Parameters.AddWithValue("CheckStockDate", item.CheckStockDate == null ? (object)DBNull.Value : item.CheckStockDate);
			sqlCommand.Parameters.AddWithValue("CheckStockUserId", item.CheckStockUserId == null ? (object)DBNull.Value : item.CheckStockUserId);
			sqlCommand.Parameters.AddWithValue("CheckStockUserName", item.CheckStockUserName == null ? (object)DBNull.Value : item.CheckStockUserName);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerOrderNumber", item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
			sqlCommand.Parameters.AddWithValue("IsCheckedStock", item.IsCheckedStock == null ? (object)DBNull.Value : item.IsCheckedStock);
			sqlCommand.Parameters.AddWithValue("OrderDeliveryDate", item.OrderDeliveryDate == null ? (object)DBNull.Value : item.OrderDeliveryDate);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("OrderOpenQuantity", item.OrderOpenQuantity == null ? (object)DBNull.Value : item.OrderOpenQuantity);
			sqlCommand.Parameters.AddWithValue("OrderPositionId", item.OrderPositionId == null ? (object)DBNull.Value : item.OrderPositionId);
			sqlCommand.Parameters.AddWithValue("RevertArchiveDate", item.RevertArchiveDate == null ? (object)DBNull.Value : item.RevertArchiveDate);
			sqlCommand.Parameters.AddWithValue("RevertArchiveUserId", item.RevertArchiveUserId == null ? (object)DBNull.Value : item.RevertArchiveUserId);
			sqlCommand.Parameters.AddWithValue("RevertArchiveUserName", item.RevertArchiveUserName == null ? (object)DBNull.Value : item.RevertArchiveUserName);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<InsideSalesChecksEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 40; // Nb params per query
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
		private static int insertWithTransaction(List<InsideSalesChecksEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__CTS_InsideSalesChecks] ([ArticleId],[ArticleNumber],[CheckCRP],[CheckCRPComments],[CheckCRPDate],[CheckCRPUserId],[CheckCRPUserName],[CheckFST],[CheckFSTComments],[CheckFSTDate],[CheckFSTUserId],[CheckFSTUserName],[CheckINS],[CheckINSComments],[CheckINSDate],[CheckINSUserId],[CheckINSUserName],[CheckPRS],[CheckPRSComments],[CheckPRSDate],[CheckPRSUserId],[CheckPRSUserName],[CheckStock],[CheckStockComments],[CheckStockDate],[CheckStockUserId],[CheckStockUserName],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[IsCheckedStock],[OrderDeliveryDate],[OrderId],[OrderNumber],[OrderOpenQuantity],[OrderPositionId],[RevertArchiveDate],[RevertArchiveUserId],[RevertArchiveUserName]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@CheckCRP" + i + ","
						+ "@CheckCRPComments" + i + ","
						+ "@CheckCRPDate" + i + ","
						+ "@CheckCRPUserId" + i + ","
						+ "@CheckCRPUserName" + i + ","
						+ "@CheckFST" + i + ","
						+ "@CheckFSTComments" + i + ","
						+ "@CheckFSTDate" + i + ","
						+ "@CheckFSTUserId" + i + ","
						+ "@CheckFSTUserName" + i + ","
						+ "@CheckINS" + i + ","
						+ "@CheckINSComments" + i + ","
						+ "@CheckINSDate" + i + ","
						+ "@CheckINSUserId" + i + ","
						+ "@CheckINSUserName" + i + ","
						+ "@CheckPRS" + i + ","
						+ "@CheckPRSComments" + i + ","
						+ "@CheckPRSDate" + i + ","
						+ "@CheckPRSUserId" + i + ","
						+ "@CheckPRSUserName" + i + ","
						+ "@CheckStock" + i + ","
						+ "@CheckStockComments" + i + ","
						+ "@CheckStockDate" + i + ","
						+ "@CheckStockUserId" + i + ","
						+ "@CheckStockUserName" + i + ","
						+ "@CustomerName" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@CustomerOrderNumber" + i + ","
						+ "@IsCheckedStock" + i + ","
						+ "@OrderDeliveryDate" + i + ","
						+ "@OrderId" + i + ","
						+ "@OrderNumber" + i + ","
						+ "@OrderOpenQuantity" + i + ","
						+ "@OrderPositionId" + i + ","
						+ "@RevertArchiveDate" + i + ","
						+ "@RevertArchiveUserId" + i + ","
						+ "@RevertArchiveUserName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CheckCRP" + i, item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
					sqlCommand.Parameters.AddWithValue("CheckCRPComments" + i, item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
					sqlCommand.Parameters.AddWithValue("CheckCRPDate" + i, item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserId" + i, item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserName" + i, item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
					sqlCommand.Parameters.AddWithValue("CheckFST" + i, item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
					sqlCommand.Parameters.AddWithValue("CheckFSTComments" + i, item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
					sqlCommand.Parameters.AddWithValue("CheckFSTDate" + i, item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserId" + i, item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserName" + i, item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
					sqlCommand.Parameters.AddWithValue("CheckINS" + i, item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
					sqlCommand.Parameters.AddWithValue("CheckINSComments" + i, item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
					sqlCommand.Parameters.AddWithValue("CheckINSDate" + i, item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
					sqlCommand.Parameters.AddWithValue("CheckINSUserId" + i, item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
					sqlCommand.Parameters.AddWithValue("CheckINSUserName" + i, item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
					sqlCommand.Parameters.AddWithValue("CheckPRS" + i, item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
					sqlCommand.Parameters.AddWithValue("CheckPRSComments" + i, item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
					sqlCommand.Parameters.AddWithValue("CheckPRSDate" + i, item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSUserId" + i, item.CheckPRSUserId == null ? (object)DBNull.Value : item.CheckPRSUserId);
					sqlCommand.Parameters.AddWithValue("CheckPRSUserName" + i, item.CheckPRSUserName == null ? (object)DBNull.Value : item.CheckPRSUserName);
					sqlCommand.Parameters.AddWithValue("CheckStock" + i, item.CheckStock == null ? (object)DBNull.Value : item.CheckStock);
					sqlCommand.Parameters.AddWithValue("CheckStockComments" + i, item.CheckStockComments == null ? (object)DBNull.Value : item.CheckStockComments);
					sqlCommand.Parameters.AddWithValue("CheckStockDate" + i, item.CheckStockDate == null ? (object)DBNull.Value : item.CheckStockDate);
					sqlCommand.Parameters.AddWithValue("CheckStockUserId" + i, item.CheckStockUserId == null ? (object)DBNull.Value : item.CheckStockUserId);
					sqlCommand.Parameters.AddWithValue("CheckStockUserName" + i, item.CheckStockUserName == null ? (object)DBNull.Value : item.CheckStockUserName);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOrderNumber" + i, item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
					sqlCommand.Parameters.AddWithValue("IsCheckedStock" + i, item.IsCheckedStock == null ? (object)DBNull.Value : item.IsCheckedStock);
					sqlCommand.Parameters.AddWithValue("OrderDeliveryDate" + i, item.OrderDeliveryDate == null ? (object)DBNull.Value : item.OrderDeliveryDate);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderOpenQuantity" + i, item.OrderOpenQuantity == null ? (object)DBNull.Value : item.OrderOpenQuantity);
					sqlCommand.Parameters.AddWithValue("OrderPositionId" + i, item.OrderPositionId == null ? (object)DBNull.Value : item.OrderPositionId);
					sqlCommand.Parameters.AddWithValue("RevertArchiveDate" + i, item.RevertArchiveDate == null ? (object)DBNull.Value : item.RevertArchiveDate);
					sqlCommand.Parameters.AddWithValue("RevertArchiveUserId" + i, item.RevertArchiveUserId == null ? (object)DBNull.Value : item.RevertArchiveUserId);
					sqlCommand.Parameters.AddWithValue("RevertArchiveUserName" + i, item.RevertArchiveUserName == null ? (object)DBNull.Value : item.RevertArchiveUserName);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(InsideSalesChecksEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__CTS_InsideSalesChecks] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [CheckCRP]=@CheckCRP, [CheckCRPComments]=@CheckCRPComments, [CheckCRPDate]=@CheckCRPDate, [CheckCRPUserId]=@CheckCRPUserId, [CheckCRPUserName]=@CheckCRPUserName, [CheckFST]=@CheckFST, [CheckFSTComments]=@CheckFSTComments, [CheckFSTDate]=@CheckFSTDate, [CheckFSTUserId]=@CheckFSTUserId, [CheckFSTUserName]=@CheckFSTUserName, [CheckINS]=@CheckINS, [CheckINSComments]=@CheckINSComments, [CheckINSDate]=@CheckINSDate, [CheckINSUserId]=@CheckINSUserId, [CheckINSUserName]=@CheckINSUserName, [CheckPRS]=@CheckPRS, [CheckPRSComments]=@CheckPRSComments, [CheckPRSDate]=@CheckPRSDate, [CheckPRSUserId]=@CheckPRSUserId, [CheckPRSUserName]=@CheckPRSUserName, [CheckStock]=@CheckStock, [CheckStockComments]=@CheckStockComments, [CheckStockDate]=@CheckStockDate, [CheckStockUserId]=@CheckStockUserId, [CheckStockUserName]=@CheckStockUserName, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [CustomerOrderNumber]=@CustomerOrderNumber, [IsCheckedStock]=@IsCheckedStock, [OrderDeliveryDate]=@OrderDeliveryDate, [OrderId]=@OrderId, [OrderNumber]=@OrderNumber, [OrderOpenQuantity]=@OrderOpenQuantity, [OrderPositionId]=@OrderPositionId, [RevertArchiveDate]=@RevertArchiveDate, [RevertArchiveUserId]=@RevertArchiveUserId, [RevertArchiveUserName]=@RevertArchiveUserName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("CheckCRP", item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
			sqlCommand.Parameters.AddWithValue("CheckCRPComments", item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
			sqlCommand.Parameters.AddWithValue("CheckCRPDate", item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
			sqlCommand.Parameters.AddWithValue("CheckCRPUserId", item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
			sqlCommand.Parameters.AddWithValue("CheckCRPUserName", item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
			sqlCommand.Parameters.AddWithValue("CheckFST", item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
			sqlCommand.Parameters.AddWithValue("CheckFSTComments", item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
			sqlCommand.Parameters.AddWithValue("CheckFSTDate", item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
			sqlCommand.Parameters.AddWithValue("CheckFSTUserId", item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
			sqlCommand.Parameters.AddWithValue("CheckFSTUserName", item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
			sqlCommand.Parameters.AddWithValue("CheckINS", item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
			sqlCommand.Parameters.AddWithValue("CheckINSComments", item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
			sqlCommand.Parameters.AddWithValue("CheckINSDate", item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
			sqlCommand.Parameters.AddWithValue("CheckINSUserId", item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
			sqlCommand.Parameters.AddWithValue("CheckINSUserName", item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
			sqlCommand.Parameters.AddWithValue("CheckPRS", item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
			sqlCommand.Parameters.AddWithValue("CheckPRSComments", item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
			sqlCommand.Parameters.AddWithValue("CheckPRSDate", item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
			sqlCommand.Parameters.AddWithValue("CheckPRSUserId", item.CheckPRSUserId == null ? (object)DBNull.Value : item.CheckPRSUserId);
			sqlCommand.Parameters.AddWithValue("CheckPRSUserName", item.CheckPRSUserName == null ? (object)DBNull.Value : item.CheckPRSUserName);
			sqlCommand.Parameters.AddWithValue("CheckStock", item.CheckStock == null ? (object)DBNull.Value : item.CheckStock);
			sqlCommand.Parameters.AddWithValue("CheckStockComments", item.CheckStockComments == null ? (object)DBNull.Value : item.CheckStockComments);
			sqlCommand.Parameters.AddWithValue("CheckStockDate", item.CheckStockDate == null ? (object)DBNull.Value : item.CheckStockDate);
			sqlCommand.Parameters.AddWithValue("CheckStockUserId", item.CheckStockUserId == null ? (object)DBNull.Value : item.CheckStockUserId);
			sqlCommand.Parameters.AddWithValue("CheckStockUserName", item.CheckStockUserName == null ? (object)DBNull.Value : item.CheckStockUserName);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerOrderNumber", item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
			sqlCommand.Parameters.AddWithValue("IsCheckedStock", item.IsCheckedStock == null ? (object)DBNull.Value : item.IsCheckedStock);
			sqlCommand.Parameters.AddWithValue("OrderDeliveryDate", item.OrderDeliveryDate == null ? (object)DBNull.Value : item.OrderDeliveryDate);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("OrderOpenQuantity", item.OrderOpenQuantity == null ? (object)DBNull.Value : item.OrderOpenQuantity);
			sqlCommand.Parameters.AddWithValue("OrderPositionId", item.OrderPositionId == null ? (object)DBNull.Value : item.OrderPositionId);
			sqlCommand.Parameters.AddWithValue("RevertArchiveDate", item.RevertArchiveDate == null ? (object)DBNull.Value : item.RevertArchiveDate);
			sqlCommand.Parameters.AddWithValue("RevertArchiveUserId", item.RevertArchiveUserId == null ? (object)DBNull.Value : item.RevertArchiveUserId);
			sqlCommand.Parameters.AddWithValue("RevertArchiveUserName", item.RevertArchiveUserName == null ? (object)DBNull.Value : item.RevertArchiveUserName);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<InsideSalesChecksEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 40; // Nb params per query
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
		private static int updateWithTransaction(List<InsideSalesChecksEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__CTS_InsideSalesChecks] SET "

					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[CheckCRP]=@CheckCRP" + i + ","
					+ "[CheckCRPComments]=@CheckCRPComments" + i + ","
					+ "[CheckCRPDate]=@CheckCRPDate" + i + ","
					+ "[CheckCRPUserId]=@CheckCRPUserId" + i + ","
					+ "[CheckCRPUserName]=@CheckCRPUserName" + i + ","
					+ "[CheckFST]=@CheckFST" + i + ","
					+ "[CheckFSTComments]=@CheckFSTComments" + i + ","
					+ "[CheckFSTDate]=@CheckFSTDate" + i + ","
					+ "[CheckFSTUserId]=@CheckFSTUserId" + i + ","
					+ "[CheckFSTUserName]=@CheckFSTUserName" + i + ","
					+ "[CheckINS]=@CheckINS" + i + ","
					+ "[CheckINSComments]=@CheckINSComments" + i + ","
					+ "[CheckINSDate]=@CheckINSDate" + i + ","
					+ "[CheckINSUserId]=@CheckINSUserId" + i + ","
					+ "[CheckINSUserName]=@CheckINSUserName" + i + ","
					+ "[CheckPRS]=@CheckPRS" + i + ","
					+ "[CheckPRSComments]=@CheckPRSComments" + i + ","
					+ "[CheckPRSDate]=@CheckPRSDate" + i + ","
					+ "[CheckPRSUserId]=@CheckPRSUserId" + i + ","
					+ "[CheckPRSUserName]=@CheckPRSUserName" + i + ","
					+ "[CheckStock]=@CheckStock" + i + ","
					+ "[CheckStockComments]=@CheckStockComments" + i + ","
					+ "[CheckStockDate]=@CheckStockDate" + i + ","
					+ "[CheckStockUserId]=@CheckStockUserId" + i + ","
					+ "[CheckStockUserName]=@CheckStockUserName" + i + ","
					+ "[CustomerName]=@CustomerName" + i + ","
					+ "[CustomerNumber]=@CustomerNumber" + i + ","
					+ "[CustomerOrderNumber]=@CustomerOrderNumber" + i + ","
					+ "[IsCheckedStock]=@IsCheckedStock" + i + ","
					+ "[OrderDeliveryDate]=@OrderDeliveryDate" + i + ","
					+ "[OrderId]=@OrderId" + i + ","
					+ "[OrderNumber]=@OrderNumber" + i + ","
					+ "[OrderOpenQuantity]=@OrderOpenQuantity" + i + ","
					+ "[OrderPositionId]=@OrderPositionId" + i + ","
					+ "[RevertArchiveDate]=@RevertArchiveDate" + i + ","
					+ "[RevertArchiveUserId]=@RevertArchiveUserId" + i + ","
					+ "[RevertArchiveUserName]=@RevertArchiveUserName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CheckCRP" + i, item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
					sqlCommand.Parameters.AddWithValue("CheckCRPComments" + i, item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
					sqlCommand.Parameters.AddWithValue("CheckCRPDate" + i, item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserId" + i, item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserName" + i, item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
					sqlCommand.Parameters.AddWithValue("CheckFST" + i, item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
					sqlCommand.Parameters.AddWithValue("CheckFSTComments" + i, item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
					sqlCommand.Parameters.AddWithValue("CheckFSTDate" + i, item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserId" + i, item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserName" + i, item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
					sqlCommand.Parameters.AddWithValue("CheckINS" + i, item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
					sqlCommand.Parameters.AddWithValue("CheckINSComments" + i, item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
					sqlCommand.Parameters.AddWithValue("CheckINSDate" + i, item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
					sqlCommand.Parameters.AddWithValue("CheckINSUserId" + i, item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
					sqlCommand.Parameters.AddWithValue("CheckINSUserName" + i, item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
					sqlCommand.Parameters.AddWithValue("CheckPRS" + i, item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
					sqlCommand.Parameters.AddWithValue("CheckPRSComments" + i, item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
					sqlCommand.Parameters.AddWithValue("CheckPRSDate" + i, item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSUserId" + i, item.CheckPRSUserId == null ? (object)DBNull.Value : item.CheckPRSUserId);
					sqlCommand.Parameters.AddWithValue("CheckPRSUserName" + i, item.CheckPRSUserName == null ? (object)DBNull.Value : item.CheckPRSUserName);
					sqlCommand.Parameters.AddWithValue("CheckStock" + i, item.CheckStock == null ? (object)DBNull.Value : item.CheckStock);
					sqlCommand.Parameters.AddWithValue("CheckStockComments" + i, item.CheckStockComments == null ? (object)DBNull.Value : item.CheckStockComments);
					sqlCommand.Parameters.AddWithValue("CheckStockDate" + i, item.CheckStockDate == null ? (object)DBNull.Value : item.CheckStockDate);
					sqlCommand.Parameters.AddWithValue("CheckStockUserId" + i, item.CheckStockUserId == null ? (object)DBNull.Value : item.CheckStockUserId);
					sqlCommand.Parameters.AddWithValue("CheckStockUserName" + i, item.CheckStockUserName == null ? (object)DBNull.Value : item.CheckStockUserName);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOrderNumber" + i, item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
					sqlCommand.Parameters.AddWithValue("IsCheckedStock" + i, item.IsCheckedStock == null ? (object)DBNull.Value : item.IsCheckedStock);
					sqlCommand.Parameters.AddWithValue("OrderDeliveryDate" + i, item.OrderDeliveryDate == null ? (object)DBNull.Value : item.OrderDeliveryDate);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderOpenQuantity" + i, item.OrderOpenQuantity == null ? (object)DBNull.Value : item.OrderOpenQuantity);
					sqlCommand.Parameters.AddWithValue("OrderPositionId" + i, item.OrderPositionId == null ? (object)DBNull.Value : item.OrderPositionId);
					sqlCommand.Parameters.AddWithValue("RevertArchiveDate" + i, item.RevertArchiveDate == null ? (object)DBNull.Value : item.RevertArchiveDate);
					sqlCommand.Parameters.AddWithValue("RevertArchiveUserId" + i, item.RevertArchiveUserId == null ? (object)DBNull.Value : item.RevertArchiveUserId);
					sqlCommand.Parameters.AddWithValue("RevertArchiveUserName" + i, item.RevertArchiveUserName == null ? (object)DBNull.Value : item.RevertArchiveUserName);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__CTS_InsideSalesChecks] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__CTS_InsideSalesChecks] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<InsideSalesChecksEntity> Get(bool filterCustomers, List<int> customerIds, Settings.SortingModel sorting,
			Settings.PaginModel paging, string searchValue)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesChecks]";

				var clauses = new List<string>();
				using(var sqlCommand = new SqlCommand())
				{
					if(filterCustomers)
					{
						clauses.Add($"[CustomerNumber] IN ({string.Join(",", customerIds)})");
					}
					if(searchValue != null || searchValue != "")
					{
						clauses.Add($@"(CustomerName LIKE '%{searchValue}%' OR CustomerOrderNumber LIKE '{searchValue}%' OR OrderNumber LIKE '{searchValue}%' 
							OR ArticleNumber LIKE '{searchValue}%')");
					}
					if(clauses.Count > 0)
					{
						query += $" WHERE {string.Join(" AND ", clauses)}";
					}
					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY CustomerName DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesChecksEntity>();
			}
		}

		public static int Count_By_CustomerName(string customerName)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS CountNr FROM [__CTS_InsideSalesChecks] ";

				using(var sqlCommand = new SqlCommand())
				{
					bool isFirstCondition = true;

					if(!string.IsNullOrWhiteSpace(customerName))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [CustomerName] LIKE '{customerName.Trim()}%' ";
						isFirstCondition = false;
					}
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return 0;
			}

			return Convert.ToInt32(dataTable.Rows[0]["CountNr"]);
		}

		public static int updateFACheck(int saleId, int faAvailableCheckedValue, int faDateOkCheckedValue, string checkFaComments, int? checkFaWeek, int userId, string username, DateTime checkFaDate, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__CTS_InsideSalesChecks] SET  [CheckFaAvaialable]=@faAvailableCheckedValue,  " +
				"[CheckFa]=1,[CheckFST]=0, [CheckINS]=0,  [CheckPRS]=0, [CheckFaUserId]=@userId,[CheckFaUserName]=@username,[CheckFaDateOk]=@faDateOk,[CheckFaDate]=@checkFaDate, " +
				"[CheckFaComments]=@faBemerkung,[CheckFaWeek]=@faWeek WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("faAvailableCheckedValue", faAvailableCheckedValue == 0 ? 0 : faAvailableCheckedValue);
				sqlCommand.Parameters.AddWithValue("faDateOk", faDateOkCheckedValue);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("username", username);
				sqlCommand.Parameters.AddWithValue("faWeek", checkFaWeek);
				sqlCommand.Parameters.AddWithValue("faBemerkung", checkFaComments == null ? (object)DBNull.Value : checkFaComments);
				sqlCommand.Parameters.AddWithValue("checkFaDate", checkFaDate);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}


		public static int updateFa(int saleId, int? faCheckedValue, int userId, string username, DateTime date, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__CTS_InsideSalesChecks] SET  [CheckCRP]=0,  [CheckFST]=0, [CheckINS]=0,  [CheckPRS]=0, [CheckFa]=@checkFa, " +
				"[CheckFaUserId]=@userId, [CheckFaDate]=@date, [CheckFaUserName]=@username WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("username", username);
				sqlCommand.Parameters.AddWithValue("date", date);
				sqlCommand.Parameters.AddWithValue("checkFa", faCheckedValue);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int updateStock(bool IsCheckedStock, int saleId, int? checkedTypeValue, int userId, string username, DateTime date, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__CTS_InsideSalesChecks] SET  [CheckCRP]=0,  [CheckFST]=0, [CheckINS]=0,  [CheckPRS]=0, [CheckStock]=@checkStock, " +
				"[IsCheckedStock]=@IsCheckedStock, [CheckStockUserId]=@userId, [CheckStockDate]=@date, [CheckStockUserName]=@username WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("username", username);
				sqlCommand.Parameters.AddWithValue("date", date);
				sqlCommand.Parameters.AddWithValue("checkStock", checkedTypeValue);
				sqlCommand.Parameters.AddWithValue("IsCheckedStock", IsCheckedStock);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return results;
		}

		public static int updateFST(int saleId, int? checkedTypeValue, int fstKapaOkCheckedValue, string CheckFSTKapaReason, int? CheckFSTKapaWeek, int userId, string username, DateTime date, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__CTS_InsideSalesChecks] SET  [CheckCRP]=0,  [CheckFST]=@checkFst, [CheckINS]=0,  [CheckPRS]=0, [CheckFSTUserId]=@userId," +
				"[CheckFSTKapaOk]=@fstKapaOk , [CheckFSTKapaReason]=@kapaReason , [CheckFSTKapaWeek]=@fstKapaWeek," +
				" [CheckFSTUserName]=@userName, [CheckFSTDate]=@fstDate WHERE [Id]=@Id";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("userName", username);
				sqlCommand.Parameters.AddWithValue("fstDate", date);
				sqlCommand.Parameters.AddWithValue("checkFst", checkedTypeValue);
				sqlCommand.Parameters.AddWithValue("fstKapaOk", fstKapaOkCheckedValue);
				sqlCommand.Parameters.AddWithValue("kapaReason", CheckFSTKapaReason is null ? (object)DBNull.Value : CheckFSTKapaReason);
				sqlCommand.Parameters.AddWithValue("fstKapaWeek", CheckFSTKapaWeek == 0 ? (object)DBNull.Value : CheckFSTKapaWeek);
				//sqlCommand.Parameters.AddWithValue("fstComment", fstComment == null ? (object)DBNull.Value : fstComment);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return results;
		}
		public static int updatePRS(int saleId, int? checkedTypeValue, int? prsMaterialOkCheckedValue, string? checkPRSMaterialMissing, DateTime? checkPRSMaterialLastDeliveryDate, int userId, string username, DateTime date, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__CTS_InsideSalesChecks] SET  [CheckCRP]=0,[CheckINS]=0,[CheckPRS]=@checkPrs,[CheckPRSUserId]=@userId, " +
				"[CheckPRSMaterialOk]=@prsMaterialOk,[CheckPRSMaterialMissing]=@prsMaterialMissing,[CheckPRSMaterialLastDeliveryDate]=@materialLastDelivery," +
				"[CheckPRSUserName]=@userName, [CheckPRSDate]=@prsDate WHERE [Id]=@Id";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("userName", username);
				sqlCommand.Parameters.AddWithValue("prsDate", date);
				sqlCommand.Parameters.AddWithValue("checkPrs", checkedTypeValue);
				sqlCommand.Parameters.AddWithValue("prsMaterialOk", prsMaterialOkCheckedValue is null ? (object)DBNull.Value : prsMaterialOkCheckedValue);
				sqlCommand.Parameters.AddWithValue("prsMaterialMissing", checkPRSMaterialMissing is null ? (object)DBNull.Value : checkPRSMaterialMissing);
				sqlCommand.Parameters.AddWithValue("materialLastDelivery", checkPRSMaterialLastDeliveryDate is null ? (object)DBNull.Value : checkPRSMaterialLastDeliveryDate);
				//sqlCommand.Parameters.AddWithValue("prsComment", prsComment == null ? (object)DBNull.Value : prsComment);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return results;
		}

		public static int updateCRP(int saleId, int? checkedTypeValue, int userId, string username, DateTime date, SqlConnection connection, SqlTransaction transaction, int crpDateAdjusted, int crpCompliedWith, int crpWeek)
		{
			int results = -1;
			string query = "UPDATE [__CTS_InsideSalesChecks] SET [CheckCRP]=@crpCheck,[CheckINS]=0,[CheckCRPUserId]=@userId, [CheckCRPUserName]=@userName, " +
				"[CheckCRPDateAdjusted]=@crpDateAdjusted , [CheckCRPWTCompliedOk]=@crpCompliedWith , [CheckCRPWeek]=@checkCrpWeek," +
				"[CheckCRPDate]=@crpDate WHERE [Id]=@Id";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("userName", username);
				sqlCommand.Parameters.AddWithValue("crpDate", date);
				sqlCommand.Parameters.AddWithValue("crpCheck", checkedTypeValue);
				sqlCommand.Parameters.AddWithValue("crpDateAdjusted", crpDateAdjusted);
				sqlCommand.Parameters.AddWithValue("crpCompliedWith", crpCompliedWith);
				sqlCommand.Parameters.AddWithValue("checkCrpWeek", checkedTypeValue);
				//sqlCommand.Parameters.AddWithValue("crpComment", crpComment == null ? (object)DBNull.Value : crpComment);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int updateINS(int saleId, int? checkedTypeValue, int userId, string username, DateTime date, SqlConnection connection, SqlTransaction transaction, int? insConfirmedValue)
		{
			int results = -1;
			string query = "UPDATE [__CTS_InsideSalesChecks] SET  [CheckINSAbConfirmed]=@InsAbConfirmedValue,[CheckINS]=@checkIns, [CheckINSUserId]=@userId, [CheckINSUserName]=@userName, [CheckINSDate]=@insDate WHERE [Id]=@Id";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", saleId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("userName", username);
				sqlCommand.Parameters.AddWithValue("insDate", date);
				sqlCommand.Parameters.AddWithValue("checkIns", checkedTypeValue);
				sqlCommand.Parameters.AddWithValue("InsAbConfirmedValue", insConfirmedValue);
				//sqlCommand.Parameters.AddWithValue("insComment", insComment == null ? (object)DBNull.Value : insComment);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static InsideSalesChecksEntity GetSalesCheckByOrderId(int? orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesChecks] WHERE [OrderId]=@orderId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new InsideSalesChecksEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods
	}
}
