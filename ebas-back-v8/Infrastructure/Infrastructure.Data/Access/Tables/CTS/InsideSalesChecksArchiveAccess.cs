using Infrastructure.Data.Entities.Tables.CTS;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class InsideSalesChecksArchiveAccess
	{

		#region Default Methods
		public static InsideSalesChecksArchiveEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesChecksArchive] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new InsideSalesChecksArchiveEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<InsideSalesChecksArchiveEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesChecksArchive]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksArchiveEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesChecksArchiveEntity>();
			}
		}
		public static List<InsideSalesChecksArchiveEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<InsideSalesChecksArchiveEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<InsideSalesChecksArchiveEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<InsideSalesChecksArchiveEntity>();
		}
		private static List<InsideSalesChecksArchiveEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_InsideSalesChecksArchive] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksArchiveEntity(x)).ToList();
				}
				else
				{
					return new List<InsideSalesChecksArchiveEntity>();
				}
			}
			return new List<InsideSalesChecksArchiveEntity>();
		}

		public static List<InsideSalesChecksArchiveEntity> GetByPage(Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesChecksArchive]";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [Id] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksArchiveEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesChecksArchiveEntity>();
			}
		}

		public static int Insert(InsideSalesChecksArchiveEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTS_InsideSalesChecksArchive] ([ArticleId],[ArticleNumber],[CheckCRP],[CheckCRPComments],[CheckCRPDate],[CheckCRPDateAdjusted],[CheckCRPUserId],[CheckCRPUserName],[CheckCRPWeek],[CheckCRPWTCompliedOk],[CheckFa],[CheckFaAvaialable],[CheckFaComments],[CheckFaDate],[CheckFaDateOk],[CheckFaUserId],[CheckFaUserName],[CheckFaWeek],[CheckFST],[CheckFSTComments],[CheckFSTDate],[CheckFSTKapaOk],[CheckFSTKapaReason],[CheckFSTKapaWeek],[CheckFSTUserId],[CheckFSTUserName],[CheckINS],[CheckINSAbConfirmed],[CheckINSComments],[CheckINSDate],[CheckINSUserId],[CheckINSUserName],[CheckPRS],[CheckPRSComments],[CheckPRSDate],[CheckPRSMaterialLastDeliveryDate],[CheckPRSMaterialMissing],[CheckPRSMaterialOk],[CheckPRSUserId],[CheckPRSUserName],[CheckStock],[CheckStockComments],[CheckStockDate],[CheckStockUserId],[CheckStockUserName],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[IsCheckedStock],[OrderDeliveryDate],[OrderId],[OrderNumber],[OrderOpenQuantity],[OrderPositionId],[RevertArchiveDate],[RevertArchiveUserId],[RevertArchiveUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@CheckCRP,@CheckCRPComments,@CheckCRPDate,@CheckCRPDateAdjusted,@CheckCRPUserId,@CheckCRPUserName,@CheckCRPWeek,@CheckCRPWTCompliedOk,@CheckFa,@CheckFaAvaialable,@CheckFaComments,@CheckFaDate,@CheckFaDateOk,@CheckFaUserId,@CheckFaUserName,@CheckFaWeek,@CheckFST,@CheckFSTComments,@CheckFSTDate,@CheckFSTKapaOk,@CheckFSTKapaReason,@CheckFSTKapaWeek,@CheckFSTUserId,@CheckFSTUserName,@CheckINS,@CheckINSAbConfirmed,@CheckINSComments,@CheckINSDate,@CheckINSUserId,@CheckINSUserName,@CheckPRS,@CheckPRSComments,@CheckPRSDate,@CheckPRSMaterialLastDeliveryDate,@CheckPRSMaterialMissing,@CheckPRSMaterialOk,@CheckPRSUserId,@CheckPRSUserName,@CheckStock,@CheckStockComments,@CheckStockDate,@CheckStockUserId,@CheckStockUserName,@CustomerName,@CustomerNumber,@CustomerOrderNumber,@IsCheckedStock,@OrderDeliveryDate,@OrderId,@OrderNumber,@OrderOpenQuantity,@OrderPositionId,@RevertArchiveDate,@RevertArchiveUserId,@RevertArchiveUserName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CheckCRP", item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
					sqlCommand.Parameters.AddWithValue("CheckCRPComments", item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
					sqlCommand.Parameters.AddWithValue("CheckCRPDate", item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
					sqlCommand.Parameters.AddWithValue("CheckCRPDateAdjusted", item.CheckCRPDateAdjusted == null ? (object)DBNull.Value : item.CheckCRPDateAdjusted);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserId", item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserName", item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
					sqlCommand.Parameters.AddWithValue("CheckCRPWeek", item.CheckCRPWeek == null ? (object)DBNull.Value : item.CheckCRPWeek);
					sqlCommand.Parameters.AddWithValue("CheckCRPWTCompliedOk", item.CheckCRPWTCompliedOk == null ? (object)DBNull.Value : item.CheckCRPWTCompliedOk);
					sqlCommand.Parameters.AddWithValue("CheckFa", item.CheckFa == null ? (object)DBNull.Value : item.CheckFa);
					sqlCommand.Parameters.AddWithValue("CheckFaAvaialable", item.CheckFaAvaialable == null ? (object)DBNull.Value : item.CheckFaAvaialable);
					sqlCommand.Parameters.AddWithValue("CheckFaComments", item.CheckFaComments == null ? (object)DBNull.Value : item.CheckFaComments);
					sqlCommand.Parameters.AddWithValue("CheckFaDate", item.CheckFaDate == null ? (object)DBNull.Value : item.CheckFaDate);
					sqlCommand.Parameters.AddWithValue("CheckFaDateOk", item.CheckFaDateOk == null ? (object)DBNull.Value : item.CheckFaDateOk);
					sqlCommand.Parameters.AddWithValue("CheckFaUserId", item.CheckFaUserId == null ? (object)DBNull.Value : item.CheckFaUserId);
					sqlCommand.Parameters.AddWithValue("CheckFaUserName", item.CheckFaUserName == null ? (object)DBNull.Value : item.CheckFaUserName);
					sqlCommand.Parameters.AddWithValue("CheckFaWeek", item.CheckFaWeek == null ? (object)DBNull.Value : item.CheckFaWeek);
					sqlCommand.Parameters.AddWithValue("CheckFST", item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
					sqlCommand.Parameters.AddWithValue("CheckFSTComments", item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
					sqlCommand.Parameters.AddWithValue("CheckFSTDate", item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaOk", item.CheckFSTKapaOk == null ? (object)DBNull.Value : item.CheckFSTKapaOk);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaReason", item.CheckFSTKapaReason == null ? (object)DBNull.Value : item.CheckFSTKapaReason);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaWeek", item.CheckFSTKapaWeek == null ? (object)DBNull.Value : item.CheckFSTKapaWeek);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserId", item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserName", item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
					sqlCommand.Parameters.AddWithValue("CheckINS", item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
					sqlCommand.Parameters.AddWithValue("CheckINSAbConfirmed", item.CheckINSAbConfirmed == null ? (object)DBNull.Value : item.CheckINSAbConfirmed);
					sqlCommand.Parameters.AddWithValue("CheckINSComments", item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
					sqlCommand.Parameters.AddWithValue("CheckINSDate", item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
					sqlCommand.Parameters.AddWithValue("CheckINSUserId", item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
					sqlCommand.Parameters.AddWithValue("CheckINSUserName", item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
					sqlCommand.Parameters.AddWithValue("CheckPRS", item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
					sqlCommand.Parameters.AddWithValue("CheckPRSComments", item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
					sqlCommand.Parameters.AddWithValue("CheckPRSDate", item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialLastDeliveryDate", item.CheckPRSMaterialLastDeliveryDate == null ? (object)DBNull.Value : item.CheckPRSMaterialLastDeliveryDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialMissing", item.CheckPRSMaterialMissing == null ? (object)DBNull.Value : item.CheckPRSMaterialMissing);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialOk", item.CheckPRSMaterialOk == null ? (object)DBNull.Value : item.CheckPRSMaterialOk);
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
		public static int Insert(List<InsideSalesChecksArchiveEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 58; // Nb params per query
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
		private static int insert(List<InsideSalesChecksArchiveEntity> items)
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
						query += " INSERT INTO [__CTS_InsideSalesChecksArchive] ([ArticleId],[ArticleNumber],[CheckCRP],[CheckCRPComments],[CheckCRPDate],[CheckCRPDateAdjusted],[CheckCRPUserId],[CheckCRPUserName],[CheckCRPWeek],[CheckCRPWTCompliedOk],[CheckFa],[CheckFaAvaialable],[CheckFaComments],[CheckFaDate],[CheckFaDateOk],[CheckFaUserId],[CheckFaUserName],[CheckFaWeek],[CheckFST],[CheckFSTComments],[CheckFSTDate],[CheckFSTKapaOk],[CheckFSTKapaReason],[CheckFSTKapaWeek],[CheckFSTUserId],[CheckFSTUserName],[CheckINS],[CheckINSAbConfirmed],[CheckINSComments],[CheckINSDate],[CheckINSUserId],[CheckINSUserName],[CheckPRS],[CheckPRSComments],[CheckPRSDate],[CheckPRSMaterialLastDeliveryDate],[CheckPRSMaterialMissing],[CheckPRSMaterialOk],[CheckPRSUserId],[CheckPRSUserName],[CheckStock],[CheckStockComments],[CheckStockDate],[CheckStockUserId],[CheckStockUserName],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[IsCheckedStock],[OrderDeliveryDate],[OrderId],[OrderNumber],[OrderOpenQuantity],[OrderPositionId],[RevertArchiveDate],[RevertArchiveUserId],[RevertArchiveUserName]) VALUES ( "

							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@CheckCRP" + i + ","
							+ "@CheckCRPComments" + i + ","
							+ "@CheckCRPDate" + i + ","
							+ "@CheckCRPDateAdjusted" + i + ","
							+ "@CheckCRPUserId" + i + ","
							+ "@CheckCRPUserName" + i + ","
							+ "@CheckCRPWeek" + i + ","
							+ "@CheckCRPWTCompliedOk" + i + ","
							+ "@CheckFa" + i + ","
							+ "@CheckFaAvaialable" + i + ","
							+ "@CheckFaComments" + i + ","
							+ "@CheckFaDate" + i + ","
							+ "@CheckFaDateOk" + i + ","
							+ "@CheckFaUserId" + i + ","
							+ "@CheckFaUserName" + i + ","
							+ "@CheckFaWeek" + i + ","
							+ "@CheckFST" + i + ","
							+ "@CheckFSTComments" + i + ","
							+ "@CheckFSTDate" + i + ","
							+ "@CheckFSTKapaOk" + i + ","
							+ "@CheckFSTKapaReason" + i + ","
							+ "@CheckFSTKapaWeek" + i + ","
							+ "@CheckFSTUserId" + i + ","
							+ "@CheckFSTUserName" + i + ","
							+ "@CheckINS" + i + ","
							+ "@CheckINSAbConfirmed" + i + ","
							+ "@CheckINSComments" + i + ","
							+ "@CheckINSDate" + i + ","
							+ "@CheckINSUserId" + i + ","
							+ "@CheckINSUserName" + i + ","
							+ "@CheckPRS" + i + ","
							+ "@CheckPRSComments" + i + ","
							+ "@CheckPRSDate" + i + ","
							+ "@CheckPRSMaterialLastDeliveryDate" + i + ","
							+ "@CheckPRSMaterialMissing" + i + ","
							+ "@CheckPRSMaterialOk" + i + ","
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
						sqlCommand.Parameters.AddWithValue("CheckCRPDateAdjusted" + i, item.CheckCRPDateAdjusted == null ? (object)DBNull.Value : item.CheckCRPDateAdjusted);
						sqlCommand.Parameters.AddWithValue("CheckCRPUserId" + i, item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
						sqlCommand.Parameters.AddWithValue("CheckCRPUserName" + i, item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
						sqlCommand.Parameters.AddWithValue("CheckCRPWeek" + i, item.CheckCRPWeek == null ? (object)DBNull.Value : item.CheckCRPWeek);
						sqlCommand.Parameters.AddWithValue("CheckCRPWTCompliedOk" + i, item.CheckCRPWTCompliedOk == null ? (object)DBNull.Value : item.CheckCRPWTCompliedOk);
						sqlCommand.Parameters.AddWithValue("CheckFa" + i, item.CheckFa == null ? (object)DBNull.Value : item.CheckFa);
						sqlCommand.Parameters.AddWithValue("CheckFaAvaialable" + i, item.CheckFaAvaialable == null ? (object)DBNull.Value : item.CheckFaAvaialable);
						sqlCommand.Parameters.AddWithValue("CheckFaComments" + i, item.CheckFaComments == null ? (object)DBNull.Value : item.CheckFaComments);
						sqlCommand.Parameters.AddWithValue("CheckFaDate" + i, item.CheckFaDate == null ? (object)DBNull.Value : item.CheckFaDate);
						sqlCommand.Parameters.AddWithValue("CheckFaDateOk" + i, item.CheckFaDateOk == null ? (object)DBNull.Value : item.CheckFaDateOk);
						sqlCommand.Parameters.AddWithValue("CheckFaUserId" + i, item.CheckFaUserId == null ? (object)DBNull.Value : item.CheckFaUserId);
						sqlCommand.Parameters.AddWithValue("CheckFaUserName" + i, item.CheckFaUserName == null ? (object)DBNull.Value : item.CheckFaUserName);
						sqlCommand.Parameters.AddWithValue("CheckFaWeek" + i, item.CheckFaWeek == null ? (object)DBNull.Value : item.CheckFaWeek);
						sqlCommand.Parameters.AddWithValue("CheckFST" + i, item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
						sqlCommand.Parameters.AddWithValue("CheckFSTComments" + i, item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
						sqlCommand.Parameters.AddWithValue("CheckFSTDate" + i, item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
						sqlCommand.Parameters.AddWithValue("CheckFSTKapaOk" + i, item.CheckFSTKapaOk == null ? (object)DBNull.Value : item.CheckFSTKapaOk);
						sqlCommand.Parameters.AddWithValue("CheckFSTKapaReason" + i, item.CheckFSTKapaReason == null ? (object)DBNull.Value : item.CheckFSTKapaReason);
						sqlCommand.Parameters.AddWithValue("CheckFSTKapaWeek" + i, item.CheckFSTKapaWeek == null ? (object)DBNull.Value : item.CheckFSTKapaWeek);
						sqlCommand.Parameters.AddWithValue("CheckFSTUserId" + i, item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
						sqlCommand.Parameters.AddWithValue("CheckFSTUserName" + i, item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
						sqlCommand.Parameters.AddWithValue("CheckINS" + i, item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
						sqlCommand.Parameters.AddWithValue("CheckINSAbConfirmed" + i, item.CheckINSAbConfirmed == null ? (object)DBNull.Value : item.CheckINSAbConfirmed);
						sqlCommand.Parameters.AddWithValue("CheckINSComments" + i, item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
						sqlCommand.Parameters.AddWithValue("CheckINSDate" + i, item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
						sqlCommand.Parameters.AddWithValue("CheckINSUserId" + i, item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
						sqlCommand.Parameters.AddWithValue("CheckINSUserName" + i, item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
						sqlCommand.Parameters.AddWithValue("CheckPRS" + i, item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
						sqlCommand.Parameters.AddWithValue("CheckPRSComments" + i, item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
						sqlCommand.Parameters.AddWithValue("CheckPRSDate" + i, item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
						sqlCommand.Parameters.AddWithValue("CheckPRSMaterialLastDeliveryDate" + i, item.CheckPRSMaterialLastDeliveryDate == null ? (object)DBNull.Value : item.CheckPRSMaterialLastDeliveryDate);
						sqlCommand.Parameters.AddWithValue("CheckPRSMaterialMissing" + i, item.CheckPRSMaterialMissing == null ? (object)DBNull.Value : item.CheckPRSMaterialMissing);
						sqlCommand.Parameters.AddWithValue("CheckPRSMaterialOk" + i, item.CheckPRSMaterialOk == null ? (object)DBNull.Value : item.CheckPRSMaterialOk);
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

		public static int Update(InsideSalesChecksArchiveEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesChecksArchive] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [CheckCRP]=@CheckCRP, [CheckCRPComments]=@CheckCRPComments, [CheckCRPDate]=@CheckCRPDate, [CheckCRPDateAdjusted]=@CheckCRPDateAdjusted, [CheckCRPUserId]=@CheckCRPUserId, [CheckCRPUserName]=@CheckCRPUserName, [CheckCRPWeek]=@CheckCRPWeek, [CheckCRPWTCompliedOk]=@CheckCRPWTCompliedOk, [CheckFa]=@CheckFa, [CheckFaAvaialable]=@CheckFaAvaialable, [CheckFaComments]=@CheckFaComments, [CheckFaDate]=@CheckFaDate, [CheckFaDateOk]=@CheckFaDateOk, [CheckFaUserId]=@CheckFaUserId, [CheckFaUserName]=@CheckFaUserName, [CheckFaWeek]=@CheckFaWeek, [CheckFST]=@CheckFST, [CheckFSTComments]=@CheckFSTComments, [CheckFSTDate]=@CheckFSTDate, [CheckFSTKapaOk]=@CheckFSTKapaOk, [CheckFSTKapaReason]=@CheckFSTKapaReason, [CheckFSTKapaWeek]=@CheckFSTKapaWeek, [CheckFSTUserId]=@CheckFSTUserId, [CheckFSTUserName]=@CheckFSTUserName, [CheckINS]=@CheckINS, [CheckINSAbConfirmed]=@CheckINSAbConfirmed, [CheckINSComments]=@CheckINSComments, [CheckINSDate]=@CheckINSDate, [CheckINSUserId]=@CheckINSUserId, [CheckINSUserName]=@CheckINSUserName, [CheckPRS]=@CheckPRS, [CheckPRSComments]=@CheckPRSComments, [CheckPRSDate]=@CheckPRSDate, [CheckPRSMaterialLastDeliveryDate]=@CheckPRSMaterialLastDeliveryDate, [CheckPRSMaterialMissing]=@CheckPRSMaterialMissing, [CheckPRSMaterialOk]=@CheckPRSMaterialOk, [CheckPRSUserId]=@CheckPRSUserId, [CheckPRSUserName]=@CheckPRSUserName, [CheckStock]=@CheckStock, [CheckStockComments]=@CheckStockComments, [CheckStockDate]=@CheckStockDate, [CheckStockUserId]=@CheckStockUserId, [CheckStockUserName]=@CheckStockUserName, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [CustomerOrderNumber]=@CustomerOrderNumber, [IsCheckedStock]=@IsCheckedStock, [OrderDeliveryDate]=@OrderDeliveryDate, [OrderId]=@OrderId, [OrderNumber]=@OrderNumber, [OrderOpenQuantity]=@OrderOpenQuantity, [OrderPositionId]=@OrderPositionId, [RevertArchiveDate]=@RevertArchiveDate, [RevertArchiveUserId]=@RevertArchiveUserId, [RevertArchiveUserName]=@RevertArchiveUserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("CheckCRP", item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
				sqlCommand.Parameters.AddWithValue("CheckCRPComments", item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
				sqlCommand.Parameters.AddWithValue("CheckCRPDate", item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
				sqlCommand.Parameters.AddWithValue("CheckCRPDateAdjusted", item.CheckCRPDateAdjusted == null ? (object)DBNull.Value : item.CheckCRPDateAdjusted);
				sqlCommand.Parameters.AddWithValue("CheckCRPUserId", item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
				sqlCommand.Parameters.AddWithValue("CheckCRPUserName", item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
				sqlCommand.Parameters.AddWithValue("CheckCRPWeek", item.CheckCRPWeek == null ? (object)DBNull.Value : item.CheckCRPWeek);
				sqlCommand.Parameters.AddWithValue("CheckCRPWTCompliedOk", item.CheckCRPWTCompliedOk == null ? (object)DBNull.Value : item.CheckCRPWTCompliedOk);
				sqlCommand.Parameters.AddWithValue("CheckFa", item.CheckFa == null ? (object)DBNull.Value : item.CheckFa);
				sqlCommand.Parameters.AddWithValue("CheckFaAvaialable", item.CheckFaAvaialable == null ? (object)DBNull.Value : item.CheckFaAvaialable);
				sqlCommand.Parameters.AddWithValue("CheckFaComments", item.CheckFaComments == null ? (object)DBNull.Value : item.CheckFaComments);
				sqlCommand.Parameters.AddWithValue("CheckFaDate", item.CheckFaDate == null ? (object)DBNull.Value : item.CheckFaDate);
				sqlCommand.Parameters.AddWithValue("CheckFaDateOk", item.CheckFaDateOk == null ? (object)DBNull.Value : item.CheckFaDateOk);
				sqlCommand.Parameters.AddWithValue("CheckFaUserId", item.CheckFaUserId == null ? (object)DBNull.Value : item.CheckFaUserId);
				sqlCommand.Parameters.AddWithValue("CheckFaUserName", item.CheckFaUserName == null ? (object)DBNull.Value : item.CheckFaUserName);
				sqlCommand.Parameters.AddWithValue("CheckFaWeek", item.CheckFaWeek == null ? (object)DBNull.Value : item.CheckFaWeek);
				sqlCommand.Parameters.AddWithValue("CheckFST", item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
				sqlCommand.Parameters.AddWithValue("CheckFSTComments", item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
				sqlCommand.Parameters.AddWithValue("CheckFSTDate", item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
				sqlCommand.Parameters.AddWithValue("CheckFSTKapaOk", item.CheckFSTKapaOk == null ? (object)DBNull.Value : item.CheckFSTKapaOk);
				sqlCommand.Parameters.AddWithValue("CheckFSTKapaReason", item.CheckFSTKapaReason == null ? (object)DBNull.Value : item.CheckFSTKapaReason);
				sqlCommand.Parameters.AddWithValue("CheckFSTKapaWeek", item.CheckFSTKapaWeek == null ? (object)DBNull.Value : item.CheckFSTKapaWeek);
				sqlCommand.Parameters.AddWithValue("CheckFSTUserId", item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
				sqlCommand.Parameters.AddWithValue("CheckFSTUserName", item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
				sqlCommand.Parameters.AddWithValue("CheckINS", item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
				sqlCommand.Parameters.AddWithValue("CheckINSAbConfirmed", item.CheckINSAbConfirmed == null ? (object)DBNull.Value : item.CheckINSAbConfirmed);
				sqlCommand.Parameters.AddWithValue("CheckINSComments", item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
				sqlCommand.Parameters.AddWithValue("CheckINSDate", item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
				sqlCommand.Parameters.AddWithValue("CheckINSUserId", item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
				sqlCommand.Parameters.AddWithValue("CheckINSUserName", item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
				sqlCommand.Parameters.AddWithValue("CheckPRS", item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
				sqlCommand.Parameters.AddWithValue("CheckPRSComments", item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
				sqlCommand.Parameters.AddWithValue("CheckPRSDate", item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
				sqlCommand.Parameters.AddWithValue("CheckPRSMaterialLastDeliveryDate", item.CheckPRSMaterialLastDeliveryDate == null ? (object)DBNull.Value : item.CheckPRSMaterialLastDeliveryDate);
				sqlCommand.Parameters.AddWithValue("CheckPRSMaterialMissing", item.CheckPRSMaterialMissing == null ? (object)DBNull.Value : item.CheckPRSMaterialMissing);
				sqlCommand.Parameters.AddWithValue("CheckPRSMaterialOk", item.CheckPRSMaterialOk == null ? (object)DBNull.Value : item.CheckPRSMaterialOk);
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
		public static int Update(List<InsideSalesChecksArchiveEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 58; // Nb params per query
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
		private static int update(List<InsideSalesChecksArchiveEntity> items)
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
						query += " UPDATE [__CTS_InsideSalesChecksArchive] SET "

							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[CheckCRP]=@CheckCRP" + i + ","
							+ "[CheckCRPComments]=@CheckCRPComments" + i + ","
							+ "[CheckCRPDate]=@CheckCRPDate" + i + ","
							+ "[CheckCRPDateAdjusted]=@CheckCRPDateAdjusted" + i + ","
							+ "[CheckCRPUserId]=@CheckCRPUserId" + i + ","
							+ "[CheckCRPUserName]=@CheckCRPUserName" + i + ","
							+ "[CheckCRPWeek]=@CheckCRPWeek" + i + ","
							+ "[CheckCRPWTCompliedOk]=@CheckCRPWTCompliedOk" + i + ","
							+ "[CheckFa]=@CheckFa" + i + ","
							+ "[CheckFaAvaialable]=@CheckFaAvaialable" + i + ","
							+ "[CheckFaComments]=@CheckFaComments" + i + ","
							+ "[CheckFaDate]=@CheckFaDate" + i + ","
							+ "[CheckFaDateOk]=@CheckFaDateOk" + i + ","
							+ "[CheckFaUserId]=@CheckFaUserId" + i + ","
							+ "[CheckFaUserName]=@CheckFaUserName" + i + ","
							+ "[CheckFaWeek]=@CheckFaWeek" + i + ","
							+ "[CheckFST]=@CheckFST" + i + ","
							+ "[CheckFSTComments]=@CheckFSTComments" + i + ","
							+ "[CheckFSTDate]=@CheckFSTDate" + i + ","
							+ "[CheckFSTKapaOk]=@CheckFSTKapaOk" + i + ","
							+ "[CheckFSTKapaReason]=@CheckFSTKapaReason" + i + ","
							+ "[CheckFSTKapaWeek]=@CheckFSTKapaWeek" + i + ","
							+ "[CheckFSTUserId]=@CheckFSTUserId" + i + ","
							+ "[CheckFSTUserName]=@CheckFSTUserName" + i + ","
							+ "[CheckINS]=@CheckINS" + i + ","
							+ "[CheckINSAbConfirmed]=@CheckINSAbConfirmed" + i + ","
							+ "[CheckINSComments]=@CheckINSComments" + i + ","
							+ "[CheckINSDate]=@CheckINSDate" + i + ","
							+ "[CheckINSUserId]=@CheckINSUserId" + i + ","
							+ "[CheckINSUserName]=@CheckINSUserName" + i + ","
							+ "[CheckPRS]=@CheckPRS" + i + ","
							+ "[CheckPRSComments]=@CheckPRSComments" + i + ","
							+ "[CheckPRSDate]=@CheckPRSDate" + i + ","
							+ "[CheckPRSMaterialLastDeliveryDate]=@CheckPRSMaterialLastDeliveryDate" + i + ","
							+ "[CheckPRSMaterialMissing]=@CheckPRSMaterialMissing" + i + ","
							+ "[CheckPRSMaterialOk]=@CheckPRSMaterialOk" + i + ","
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
						sqlCommand.Parameters.AddWithValue("CheckCRPDateAdjusted" + i, item.CheckCRPDateAdjusted == null ? (object)DBNull.Value : item.CheckCRPDateAdjusted);
						sqlCommand.Parameters.AddWithValue("CheckCRPUserId" + i, item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
						sqlCommand.Parameters.AddWithValue("CheckCRPUserName" + i, item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
						sqlCommand.Parameters.AddWithValue("CheckCRPWeek" + i, item.CheckCRPWeek == null ? (object)DBNull.Value : item.CheckCRPWeek);
						sqlCommand.Parameters.AddWithValue("CheckCRPWTCompliedOk" + i, item.CheckCRPWTCompliedOk == null ? (object)DBNull.Value : item.CheckCRPWTCompliedOk);
						sqlCommand.Parameters.AddWithValue("CheckFa" + i, item.CheckFa == null ? (object)DBNull.Value : item.CheckFa);
						sqlCommand.Parameters.AddWithValue("CheckFaAvaialable" + i, item.CheckFaAvaialable == null ? (object)DBNull.Value : item.CheckFaAvaialable);
						sqlCommand.Parameters.AddWithValue("CheckFaComments" + i, item.CheckFaComments == null ? (object)DBNull.Value : item.CheckFaComments);
						sqlCommand.Parameters.AddWithValue("CheckFaDate" + i, item.CheckFaDate == null ? (object)DBNull.Value : item.CheckFaDate);
						sqlCommand.Parameters.AddWithValue("CheckFaDateOk" + i, item.CheckFaDateOk == null ? (object)DBNull.Value : item.CheckFaDateOk);
						sqlCommand.Parameters.AddWithValue("CheckFaUserId" + i, item.CheckFaUserId == null ? (object)DBNull.Value : item.CheckFaUserId);
						sqlCommand.Parameters.AddWithValue("CheckFaUserName" + i, item.CheckFaUserName == null ? (object)DBNull.Value : item.CheckFaUserName);
						sqlCommand.Parameters.AddWithValue("CheckFaWeek" + i, item.CheckFaWeek == null ? (object)DBNull.Value : item.CheckFaWeek);
						sqlCommand.Parameters.AddWithValue("CheckFST" + i, item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
						sqlCommand.Parameters.AddWithValue("CheckFSTComments" + i, item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
						sqlCommand.Parameters.AddWithValue("CheckFSTDate" + i, item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
						sqlCommand.Parameters.AddWithValue("CheckFSTKapaOk" + i, item.CheckFSTKapaOk == null ? (object)DBNull.Value : item.CheckFSTKapaOk);
						sqlCommand.Parameters.AddWithValue("CheckFSTKapaReason" + i, item.CheckFSTKapaReason == null ? (object)DBNull.Value : item.CheckFSTKapaReason);
						sqlCommand.Parameters.AddWithValue("CheckFSTKapaWeek" + i, item.CheckFSTKapaWeek == null ? (object)DBNull.Value : item.CheckFSTKapaWeek);
						sqlCommand.Parameters.AddWithValue("CheckFSTUserId" + i, item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
						sqlCommand.Parameters.AddWithValue("CheckFSTUserName" + i, item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
						sqlCommand.Parameters.AddWithValue("CheckINS" + i, item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
						sqlCommand.Parameters.AddWithValue("CheckINSAbConfirmed" + i, item.CheckINSAbConfirmed == null ? (object)DBNull.Value : item.CheckINSAbConfirmed);
						sqlCommand.Parameters.AddWithValue("CheckINSComments" + i, item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
						sqlCommand.Parameters.AddWithValue("CheckINSDate" + i, item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
						sqlCommand.Parameters.AddWithValue("CheckINSUserId" + i, item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
						sqlCommand.Parameters.AddWithValue("CheckINSUserName" + i, item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
						sqlCommand.Parameters.AddWithValue("CheckPRS" + i, item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
						sqlCommand.Parameters.AddWithValue("CheckPRSComments" + i, item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
						sqlCommand.Parameters.AddWithValue("CheckPRSDate" + i, item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
						sqlCommand.Parameters.AddWithValue("CheckPRSMaterialLastDeliveryDate" + i, item.CheckPRSMaterialLastDeliveryDate == null ? (object)DBNull.Value : item.CheckPRSMaterialLastDeliveryDate);
						sqlCommand.Parameters.AddWithValue("CheckPRSMaterialMissing" + i, item.CheckPRSMaterialMissing == null ? (object)DBNull.Value : item.CheckPRSMaterialMissing);
						sqlCommand.Parameters.AddWithValue("CheckPRSMaterialOk" + i, item.CheckPRSMaterialOk == null ? (object)DBNull.Value : item.CheckPRSMaterialOk);
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

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__CTS_InsideSalesChecksArchive] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__CTS_InsideSalesChecksArchive] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static InsideSalesChecksArchiveEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_InsideSalesChecksArchive] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new InsideSalesChecksArchiveEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<InsideSalesChecksArchiveEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_InsideSalesChecksArchive]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksArchiveEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesChecksArchiveEntity>();
			}
		}
		public static List<InsideSalesChecksArchiveEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<InsideSalesChecksArchiveEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<InsideSalesChecksArchiveEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<InsideSalesChecksArchiveEntity>();
		}
		private static List<InsideSalesChecksArchiveEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__CTS_InsideSalesChecksArchive] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksArchiveEntity(x)).ToList();
				}
				else
				{
					return new List<InsideSalesChecksArchiveEntity>();
				}
			}
			return new List<InsideSalesChecksArchiveEntity>();
		}

		public static int InsertWithTransaction(InsideSalesChecksArchiveEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__CTS_InsideSalesChecksArchive] ([ArticleId],[ArticleNumber],[CheckCRP],[CheckCRPComments],[CheckCRPDate],[CheckCRPDateAdjusted],[CheckCRPUserId],[CheckCRPUserName],[CheckCRPWeek],[CheckCRPWTCompliedOk],[CheckFa],[CheckFaAvaialable],[CheckFaComments],[CheckFaDate],[CheckFaDateOk],[CheckFaUserId],[CheckFaUserName],[CheckFaWeek],[CheckFST],[CheckFSTComments],[CheckFSTDate],[CheckFSTKapaOk],[CheckFSTKapaReason],[CheckFSTKapaWeek],[CheckFSTUserId],[CheckFSTUserName],[CheckINS],[CheckINSAbConfirmed],[CheckINSComments],[CheckINSDate],[CheckINSUserId],[CheckINSUserName],[CheckPRS],[CheckPRSComments],[CheckPRSDate],[CheckPRSMaterialLastDeliveryDate],[CheckPRSMaterialMissing],[CheckPRSMaterialOk],[CheckPRSUserId],[CheckPRSUserName],[CheckStock],[CheckStockComments],[CheckStockDate],[CheckStockUserId],[CheckStockUserName],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[IsCheckedStock],[OrderDeliveryDate],[OrderId],[OrderNumber],[OrderOpenQuantity],[OrderPositionId],[RevertArchiveDate],[RevertArchiveUserId],[RevertArchiveUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@CheckCRP,@CheckCRPComments,@CheckCRPDate,@CheckCRPDateAdjusted,@CheckCRPUserId,@CheckCRPUserName,@CheckCRPWeek,@CheckCRPWTCompliedOk,@CheckFa,@CheckFaAvaialable,@CheckFaComments,@CheckFaDate,@CheckFaDateOk,@CheckFaUserId,@CheckFaUserName,@CheckFaWeek,@CheckFST,@CheckFSTComments,@CheckFSTDate,@CheckFSTKapaOk,@CheckFSTKapaReason,@CheckFSTKapaWeek,@CheckFSTUserId,@CheckFSTUserName,@CheckINS,@CheckINSAbConfirmed,@CheckINSComments,@CheckINSDate,@CheckINSUserId,@CheckINSUserName,@CheckPRS,@CheckPRSComments,@CheckPRSDate,@CheckPRSMaterialLastDeliveryDate,@CheckPRSMaterialMissing,@CheckPRSMaterialOk,@CheckPRSUserId,@CheckPRSUserName,@CheckStock,@CheckStockComments,@CheckStockDate,@CheckStockUserId,@CheckStockUserName,@CustomerName,@CustomerNumber,@CustomerOrderNumber,@IsCheckedStock,@OrderDeliveryDate,@OrderId,@OrderNumber,@OrderOpenQuantity,@OrderPositionId,@RevertArchiveDate,@RevertArchiveUserId,@RevertArchiveUserName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("CheckCRP", item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
			sqlCommand.Parameters.AddWithValue("CheckCRPComments", item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
			sqlCommand.Parameters.AddWithValue("CheckCRPDate", item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
			sqlCommand.Parameters.AddWithValue("CheckCRPDateAdjusted", item.CheckCRPDateAdjusted == null ? (object)DBNull.Value : item.CheckCRPDateAdjusted);
			sqlCommand.Parameters.AddWithValue("CheckCRPUserId", item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
			sqlCommand.Parameters.AddWithValue("CheckCRPUserName", item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
			sqlCommand.Parameters.AddWithValue("CheckCRPWeek", item.CheckCRPWeek == null ? (object)DBNull.Value : item.CheckCRPWeek);
			sqlCommand.Parameters.AddWithValue("CheckCRPWTCompliedOk", item.CheckCRPWTCompliedOk == null ? (object)DBNull.Value : item.CheckCRPWTCompliedOk);
			sqlCommand.Parameters.AddWithValue("CheckFa", item.CheckFa == null ? (object)DBNull.Value : item.CheckFa);
			sqlCommand.Parameters.AddWithValue("CheckFaAvaialable", item.CheckFaAvaialable == null ? (object)DBNull.Value : item.CheckFaAvaialable);
			sqlCommand.Parameters.AddWithValue("CheckFaComments", item.CheckFaComments == null ? (object)DBNull.Value : item.CheckFaComments);
			sqlCommand.Parameters.AddWithValue("CheckFaDate", item.CheckFaDate == null ? (object)DBNull.Value : item.CheckFaDate);
			sqlCommand.Parameters.AddWithValue("CheckFaDateOk", item.CheckFaDateOk == null ? (object)DBNull.Value : item.CheckFaDateOk);
			sqlCommand.Parameters.AddWithValue("CheckFaUserId", item.CheckFaUserId == null ? (object)DBNull.Value : item.CheckFaUserId);
			sqlCommand.Parameters.AddWithValue("CheckFaUserName", item.CheckFaUserName == null ? (object)DBNull.Value : item.CheckFaUserName);
			sqlCommand.Parameters.AddWithValue("CheckFaWeek", item.CheckFaWeek == null ? (object)DBNull.Value : item.CheckFaWeek);
			sqlCommand.Parameters.AddWithValue("CheckFST", item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
			sqlCommand.Parameters.AddWithValue("CheckFSTComments", item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
			sqlCommand.Parameters.AddWithValue("CheckFSTDate", item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
			sqlCommand.Parameters.AddWithValue("CheckFSTKapaOk", item.CheckFSTKapaOk == null ? (object)DBNull.Value : item.CheckFSTKapaOk);
			sqlCommand.Parameters.AddWithValue("CheckFSTKapaReason", item.CheckFSTKapaReason == null ? (object)DBNull.Value : item.CheckFSTKapaReason);
			sqlCommand.Parameters.AddWithValue("CheckFSTKapaWeek", item.CheckFSTKapaWeek == null ? (object)DBNull.Value : item.CheckFSTKapaWeek);
			sqlCommand.Parameters.AddWithValue("CheckFSTUserId", item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
			sqlCommand.Parameters.AddWithValue("CheckFSTUserName", item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
			sqlCommand.Parameters.AddWithValue("CheckINS", item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
			sqlCommand.Parameters.AddWithValue("CheckINSAbConfirmed", item.CheckINSAbConfirmed == null ? (object)DBNull.Value : item.CheckINSAbConfirmed);
			sqlCommand.Parameters.AddWithValue("CheckINSComments", item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
			sqlCommand.Parameters.AddWithValue("CheckINSDate", item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
			sqlCommand.Parameters.AddWithValue("CheckINSUserId", item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
			sqlCommand.Parameters.AddWithValue("CheckINSUserName", item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
			sqlCommand.Parameters.AddWithValue("CheckPRS", item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
			sqlCommand.Parameters.AddWithValue("CheckPRSComments", item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
			sqlCommand.Parameters.AddWithValue("CheckPRSDate", item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
			sqlCommand.Parameters.AddWithValue("CheckPRSMaterialLastDeliveryDate", item.CheckPRSMaterialLastDeliveryDate == null ? (object)DBNull.Value : item.CheckPRSMaterialLastDeliveryDate);
			sqlCommand.Parameters.AddWithValue("CheckPRSMaterialMissing", item.CheckPRSMaterialMissing == null ? (object)DBNull.Value : item.CheckPRSMaterialMissing);
			sqlCommand.Parameters.AddWithValue("CheckPRSMaterialOk", item.CheckPRSMaterialOk == null ? (object)DBNull.Value : item.CheckPRSMaterialOk);
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
		public static int InsertWithTransaction(List<InsideSalesChecksArchiveEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 58; // Nb params per query
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
		private static int insertWithTransaction(List<InsideSalesChecksArchiveEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__CTS_InsideSalesChecksArchive] ([ArticleId],[ArticleNumber],[CheckCRP],[CheckCRPComments],[CheckCRPDate],[CheckCRPDateAdjusted],[CheckCRPUserId],[CheckCRPUserName],[CheckCRPWeek],[CheckCRPWTCompliedOk],[CheckFa],[CheckFaAvaialable],[CheckFaComments],[CheckFaDate],[CheckFaDateOk],[CheckFaUserId],[CheckFaUserName],[CheckFaWeek],[CheckFST],[CheckFSTComments],[CheckFSTDate],[CheckFSTKapaOk],[CheckFSTKapaReason],[CheckFSTKapaWeek],[CheckFSTUserId],[CheckFSTUserName],[CheckINS],[CheckINSAbConfirmed],[CheckINSComments],[CheckINSDate],[CheckINSUserId],[CheckINSUserName],[CheckPRS],[CheckPRSComments],[CheckPRSDate],[CheckPRSMaterialLastDeliveryDate],[CheckPRSMaterialMissing],[CheckPRSMaterialOk],[CheckPRSUserId],[CheckPRSUserName],[CheckStock],[CheckStockComments],[CheckStockDate],[CheckStockUserId],[CheckStockUserName],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[IsCheckedStock],[OrderDeliveryDate],[OrderId],[OrderNumber],[OrderOpenQuantity],[OrderPositionId],[RevertArchiveDate],[RevertArchiveUserId],[RevertArchiveUserName]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@CheckCRP" + i + ","
						+ "@CheckCRPComments" + i + ","
						+ "@CheckCRPDate" + i + ","
						+ "@CheckCRPDateAdjusted" + i + ","
						+ "@CheckCRPUserId" + i + ","
						+ "@CheckCRPUserName" + i + ","
						+ "@CheckCRPWeek" + i + ","
						+ "@CheckCRPWTCompliedOk" + i + ","
						+ "@CheckFa" + i + ","
						+ "@CheckFaAvaialable" + i + ","
						+ "@CheckFaComments" + i + ","
						+ "@CheckFaDate" + i + ","
						+ "@CheckFaDateOk" + i + ","
						+ "@CheckFaUserId" + i + ","
						+ "@CheckFaUserName" + i + ","
						+ "@CheckFaWeek" + i + ","
						+ "@CheckFST" + i + ","
						+ "@CheckFSTComments" + i + ","
						+ "@CheckFSTDate" + i + ","
						+ "@CheckFSTKapaOk" + i + ","
						+ "@CheckFSTKapaReason" + i + ","
						+ "@CheckFSTKapaWeek" + i + ","
						+ "@CheckFSTUserId" + i + ","
						+ "@CheckFSTUserName" + i + ","
						+ "@CheckINS" + i + ","
						+ "@CheckINSAbConfirmed" + i + ","
						+ "@CheckINSComments" + i + ","
						+ "@CheckINSDate" + i + ","
						+ "@CheckINSUserId" + i + ","
						+ "@CheckINSUserName" + i + ","
						+ "@CheckPRS" + i + ","
						+ "@CheckPRSComments" + i + ","
						+ "@CheckPRSDate" + i + ","
						+ "@CheckPRSMaterialLastDeliveryDate" + i + ","
						+ "@CheckPRSMaterialMissing" + i + ","
						+ "@CheckPRSMaterialOk" + i + ","
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
					sqlCommand.Parameters.AddWithValue("CheckCRPDateAdjusted" + i, item.CheckCRPDateAdjusted == null ? (object)DBNull.Value : item.CheckCRPDateAdjusted);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserId" + i, item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserName" + i, item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
					sqlCommand.Parameters.AddWithValue("CheckCRPWeek" + i, item.CheckCRPWeek == null ? (object)DBNull.Value : item.CheckCRPWeek);
					sqlCommand.Parameters.AddWithValue("CheckCRPWTCompliedOk" + i, item.CheckCRPWTCompliedOk == null ? (object)DBNull.Value : item.CheckCRPWTCompliedOk);
					sqlCommand.Parameters.AddWithValue("CheckFa" + i, item.CheckFa == null ? (object)DBNull.Value : item.CheckFa);
					sqlCommand.Parameters.AddWithValue("CheckFaAvaialable" + i, item.CheckFaAvaialable == null ? (object)DBNull.Value : item.CheckFaAvaialable);
					sqlCommand.Parameters.AddWithValue("CheckFaComments" + i, item.CheckFaComments == null ? (object)DBNull.Value : item.CheckFaComments);
					sqlCommand.Parameters.AddWithValue("CheckFaDate" + i, item.CheckFaDate == null ? (object)DBNull.Value : item.CheckFaDate);
					sqlCommand.Parameters.AddWithValue("CheckFaDateOk" + i, item.CheckFaDateOk == null ? (object)DBNull.Value : item.CheckFaDateOk);
					sqlCommand.Parameters.AddWithValue("CheckFaUserId" + i, item.CheckFaUserId == null ? (object)DBNull.Value : item.CheckFaUserId);
					sqlCommand.Parameters.AddWithValue("CheckFaUserName" + i, item.CheckFaUserName == null ? (object)DBNull.Value : item.CheckFaUserName);
					sqlCommand.Parameters.AddWithValue("CheckFaWeek" + i, item.CheckFaWeek == null ? (object)DBNull.Value : item.CheckFaWeek);
					sqlCommand.Parameters.AddWithValue("CheckFST" + i, item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
					sqlCommand.Parameters.AddWithValue("CheckFSTComments" + i, item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
					sqlCommand.Parameters.AddWithValue("CheckFSTDate" + i, item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaOk" + i, item.CheckFSTKapaOk == null ? (object)DBNull.Value : item.CheckFSTKapaOk);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaReason" + i, item.CheckFSTKapaReason == null ? (object)DBNull.Value : item.CheckFSTKapaReason);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaWeek" + i, item.CheckFSTKapaWeek == null ? (object)DBNull.Value : item.CheckFSTKapaWeek);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserId" + i, item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserName" + i, item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
					sqlCommand.Parameters.AddWithValue("CheckINS" + i, item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
					sqlCommand.Parameters.AddWithValue("CheckINSAbConfirmed" + i, item.CheckINSAbConfirmed == null ? (object)DBNull.Value : item.CheckINSAbConfirmed);
					sqlCommand.Parameters.AddWithValue("CheckINSComments" + i, item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
					sqlCommand.Parameters.AddWithValue("CheckINSDate" + i, item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
					sqlCommand.Parameters.AddWithValue("CheckINSUserId" + i, item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
					sqlCommand.Parameters.AddWithValue("CheckINSUserName" + i, item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
					sqlCommand.Parameters.AddWithValue("CheckPRS" + i, item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
					sqlCommand.Parameters.AddWithValue("CheckPRSComments" + i, item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
					sqlCommand.Parameters.AddWithValue("CheckPRSDate" + i, item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialLastDeliveryDate" + i, item.CheckPRSMaterialLastDeliveryDate == null ? (object)DBNull.Value : item.CheckPRSMaterialLastDeliveryDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialMissing" + i, item.CheckPRSMaterialMissing == null ? (object)DBNull.Value : item.CheckPRSMaterialMissing);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialOk" + i, item.CheckPRSMaterialOk == null ? (object)DBNull.Value : item.CheckPRSMaterialOk);
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

		public static int UpdateWithTransaction(InsideSalesChecksArchiveEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__CTS_InsideSalesChecksArchive] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [CheckCRP]=@CheckCRP, [CheckCRPComments]=@CheckCRPComments, [CheckCRPDate]=@CheckCRPDate, [CheckCRPDateAdjusted]=@CheckCRPDateAdjusted, [CheckCRPUserId]=@CheckCRPUserId, [CheckCRPUserName]=@CheckCRPUserName, [CheckCRPWeek]=@CheckCRPWeek, [CheckCRPWTCompliedOk]=@CheckCRPWTCompliedOk, [CheckFa]=@CheckFa, [CheckFaAvaialable]=@CheckFaAvaialable, [CheckFaComments]=@CheckFaComments, [CheckFaDate]=@CheckFaDate, [CheckFaDateOk]=@CheckFaDateOk, [CheckFaUserId]=@CheckFaUserId, [CheckFaUserName]=@CheckFaUserName, [CheckFaWeek]=@CheckFaWeek, [CheckFST]=@CheckFST, [CheckFSTComments]=@CheckFSTComments, [CheckFSTDate]=@CheckFSTDate, [CheckFSTKapaOk]=@CheckFSTKapaOk, [CheckFSTKapaReason]=@CheckFSTKapaReason, [CheckFSTKapaWeek]=@CheckFSTKapaWeek, [CheckFSTUserId]=@CheckFSTUserId, [CheckFSTUserName]=@CheckFSTUserName, [CheckINS]=@CheckINS, [CheckINSAbConfirmed]=@CheckINSAbConfirmed, [CheckINSComments]=@CheckINSComments, [CheckINSDate]=@CheckINSDate, [CheckINSUserId]=@CheckINSUserId, [CheckINSUserName]=@CheckINSUserName, [CheckPRS]=@CheckPRS, [CheckPRSComments]=@CheckPRSComments, [CheckPRSDate]=@CheckPRSDate, [CheckPRSMaterialLastDeliveryDate]=@CheckPRSMaterialLastDeliveryDate, [CheckPRSMaterialMissing]=@CheckPRSMaterialMissing, [CheckPRSMaterialOk]=@CheckPRSMaterialOk, [CheckPRSUserId]=@CheckPRSUserId, [CheckPRSUserName]=@CheckPRSUserName, [CheckStock]=@CheckStock, [CheckStockComments]=@CheckStockComments, [CheckStockDate]=@CheckStockDate, [CheckStockUserId]=@CheckStockUserId, [CheckStockUserName]=@CheckStockUserName, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [CustomerOrderNumber]=@CustomerOrderNumber, [IsCheckedStock]=@IsCheckedStock, [OrderDeliveryDate]=@OrderDeliveryDate, [OrderId]=@OrderId, [OrderNumber]=@OrderNumber, [OrderOpenQuantity]=@OrderOpenQuantity, [OrderPositionId]=@OrderPositionId, [RevertArchiveDate]=@RevertArchiveDate, [RevertArchiveUserId]=@RevertArchiveUserId, [RevertArchiveUserName]=@RevertArchiveUserName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("CheckCRP", item.CheckCRP == null ? (object)DBNull.Value : item.CheckCRP);
			sqlCommand.Parameters.AddWithValue("CheckCRPComments", item.CheckCRPComments == null ? (object)DBNull.Value : item.CheckCRPComments);
			sqlCommand.Parameters.AddWithValue("CheckCRPDate", item.CheckCRPDate == null ? (object)DBNull.Value : item.CheckCRPDate);
			sqlCommand.Parameters.AddWithValue("CheckCRPDateAdjusted", item.CheckCRPDateAdjusted == null ? (object)DBNull.Value : item.CheckCRPDateAdjusted);
			sqlCommand.Parameters.AddWithValue("CheckCRPUserId", item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
			sqlCommand.Parameters.AddWithValue("CheckCRPUserName", item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
			sqlCommand.Parameters.AddWithValue("CheckCRPWeek", item.CheckCRPWeek == null ? (object)DBNull.Value : item.CheckCRPWeek);
			sqlCommand.Parameters.AddWithValue("CheckCRPWTCompliedOk", item.CheckCRPWTCompliedOk == null ? (object)DBNull.Value : item.CheckCRPWTCompliedOk);
			sqlCommand.Parameters.AddWithValue("CheckFa", item.CheckFa == null ? (object)DBNull.Value : item.CheckFa);
			sqlCommand.Parameters.AddWithValue("CheckFaAvaialable", item.CheckFaAvaialable == null ? (object)DBNull.Value : item.CheckFaAvaialable);
			sqlCommand.Parameters.AddWithValue("CheckFaComments", item.CheckFaComments == null ? (object)DBNull.Value : item.CheckFaComments);
			sqlCommand.Parameters.AddWithValue("CheckFaDate", item.CheckFaDate == null ? (object)DBNull.Value : item.CheckFaDate);
			sqlCommand.Parameters.AddWithValue("CheckFaDateOk", item.CheckFaDateOk == null ? (object)DBNull.Value : item.CheckFaDateOk);
			sqlCommand.Parameters.AddWithValue("CheckFaUserId", item.CheckFaUserId == null ? (object)DBNull.Value : item.CheckFaUserId);
			sqlCommand.Parameters.AddWithValue("CheckFaUserName", item.CheckFaUserName == null ? (object)DBNull.Value : item.CheckFaUserName);
			sqlCommand.Parameters.AddWithValue("CheckFaWeek", item.CheckFaWeek == null ? (object)DBNull.Value : item.CheckFaWeek);
			sqlCommand.Parameters.AddWithValue("CheckFST", item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
			sqlCommand.Parameters.AddWithValue("CheckFSTComments", item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
			sqlCommand.Parameters.AddWithValue("CheckFSTDate", item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
			sqlCommand.Parameters.AddWithValue("CheckFSTKapaOk", item.CheckFSTKapaOk == null ? (object)DBNull.Value : item.CheckFSTKapaOk);
			sqlCommand.Parameters.AddWithValue("CheckFSTKapaReason", item.CheckFSTKapaReason == null ? (object)DBNull.Value : item.CheckFSTKapaReason);
			sqlCommand.Parameters.AddWithValue("CheckFSTKapaWeek", item.CheckFSTKapaWeek == null ? (object)DBNull.Value : item.CheckFSTKapaWeek);
			sqlCommand.Parameters.AddWithValue("CheckFSTUserId", item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
			sqlCommand.Parameters.AddWithValue("CheckFSTUserName", item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
			sqlCommand.Parameters.AddWithValue("CheckINS", item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
			sqlCommand.Parameters.AddWithValue("CheckINSAbConfirmed", item.CheckINSAbConfirmed == null ? (object)DBNull.Value : item.CheckINSAbConfirmed);
			sqlCommand.Parameters.AddWithValue("CheckINSComments", item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
			sqlCommand.Parameters.AddWithValue("CheckINSDate", item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
			sqlCommand.Parameters.AddWithValue("CheckINSUserId", item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
			sqlCommand.Parameters.AddWithValue("CheckINSUserName", item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
			sqlCommand.Parameters.AddWithValue("CheckPRS", item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
			sqlCommand.Parameters.AddWithValue("CheckPRSComments", item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
			sqlCommand.Parameters.AddWithValue("CheckPRSDate", item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
			sqlCommand.Parameters.AddWithValue("CheckPRSMaterialLastDeliveryDate", item.CheckPRSMaterialLastDeliveryDate == null ? (object)DBNull.Value : item.CheckPRSMaterialLastDeliveryDate);
			sqlCommand.Parameters.AddWithValue("CheckPRSMaterialMissing", item.CheckPRSMaterialMissing == null ? (object)DBNull.Value : item.CheckPRSMaterialMissing);
			sqlCommand.Parameters.AddWithValue("CheckPRSMaterialOk", item.CheckPRSMaterialOk == null ? (object)DBNull.Value : item.CheckPRSMaterialOk);
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
		public static int UpdateWithTransaction(List<InsideSalesChecksArchiveEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 58; // Nb params per query
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
		private static int updateWithTransaction(List<InsideSalesChecksArchiveEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__CTS_InsideSalesChecksArchive] SET "

					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[CheckCRP]=@CheckCRP" + i + ","
					+ "[CheckCRPComments]=@CheckCRPComments" + i + ","
					+ "[CheckCRPDate]=@CheckCRPDate" + i + ","
					+ "[CheckCRPDateAdjusted]=@CheckCRPDateAdjusted" + i + ","
					+ "[CheckCRPUserId]=@CheckCRPUserId" + i + ","
					+ "[CheckCRPUserName]=@CheckCRPUserName" + i + ","
					+ "[CheckCRPWeek]=@CheckCRPWeek" + i + ","
					+ "[CheckCRPWTCompliedOk]=@CheckCRPWTCompliedOk" + i + ","
					+ "[CheckFa]=@CheckFa" + i + ","
					+ "[CheckFaAvaialable]=@CheckFaAvaialable" + i + ","
					+ "[CheckFaComments]=@CheckFaComments" + i + ","
					+ "[CheckFaDate]=@CheckFaDate" + i + ","
					+ "[CheckFaDateOk]=@CheckFaDateOk" + i + ","
					+ "[CheckFaUserId]=@CheckFaUserId" + i + ","
					+ "[CheckFaUserName]=@CheckFaUserName" + i + ","
					+ "[CheckFaWeek]=@CheckFaWeek" + i + ","
					+ "[CheckFST]=@CheckFST" + i + ","
					+ "[CheckFSTComments]=@CheckFSTComments" + i + ","
					+ "[CheckFSTDate]=@CheckFSTDate" + i + ","
					+ "[CheckFSTKapaOk]=@CheckFSTKapaOk" + i + ","
					+ "[CheckFSTKapaReason]=@CheckFSTKapaReason" + i + ","
					+ "[CheckFSTKapaWeek]=@CheckFSTKapaWeek" + i + ","
					+ "[CheckFSTUserId]=@CheckFSTUserId" + i + ","
					+ "[CheckFSTUserName]=@CheckFSTUserName" + i + ","
					+ "[CheckINS]=@CheckINS" + i + ","
					+ "[CheckINSAbConfirmed]=@CheckINSAbConfirmed" + i + ","
					+ "[CheckINSComments]=@CheckINSComments" + i + ","
					+ "[CheckINSDate]=@CheckINSDate" + i + ","
					+ "[CheckINSUserId]=@CheckINSUserId" + i + ","
					+ "[CheckINSUserName]=@CheckINSUserName" + i + ","
					+ "[CheckPRS]=@CheckPRS" + i + ","
					+ "[CheckPRSComments]=@CheckPRSComments" + i + ","
					+ "[CheckPRSDate]=@CheckPRSDate" + i + ","
					+ "[CheckPRSMaterialLastDeliveryDate]=@CheckPRSMaterialLastDeliveryDate" + i + ","
					+ "[CheckPRSMaterialMissing]=@CheckPRSMaterialMissing" + i + ","
					+ "[CheckPRSMaterialOk]=@CheckPRSMaterialOk" + i + ","
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
					sqlCommand.Parameters.AddWithValue("CheckCRPDateAdjusted" + i, item.CheckCRPDateAdjusted == null ? (object)DBNull.Value : item.CheckCRPDateAdjusted);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserId" + i, item.CheckCRPUserId == null ? (object)DBNull.Value : item.CheckCRPUserId);
					sqlCommand.Parameters.AddWithValue("CheckCRPUserName" + i, item.CheckCRPUserName == null ? (object)DBNull.Value : item.CheckCRPUserName);
					sqlCommand.Parameters.AddWithValue("CheckCRPWeek" + i, item.CheckCRPWeek == null ? (object)DBNull.Value : item.CheckCRPWeek);
					sqlCommand.Parameters.AddWithValue("CheckCRPWTCompliedOk" + i, item.CheckCRPWTCompliedOk == null ? (object)DBNull.Value : item.CheckCRPWTCompliedOk);
					sqlCommand.Parameters.AddWithValue("CheckFa" + i, item.CheckFa == null ? (object)DBNull.Value : item.CheckFa);
					sqlCommand.Parameters.AddWithValue("CheckFaAvaialable" + i, item.CheckFaAvaialable == null ? (object)DBNull.Value : item.CheckFaAvaialable);
					sqlCommand.Parameters.AddWithValue("CheckFaComments" + i, item.CheckFaComments == null ? (object)DBNull.Value : item.CheckFaComments);
					sqlCommand.Parameters.AddWithValue("CheckFaDate" + i, item.CheckFaDate == null ? (object)DBNull.Value : item.CheckFaDate);
					sqlCommand.Parameters.AddWithValue("CheckFaDateOk" + i, item.CheckFaDateOk == null ? (object)DBNull.Value : item.CheckFaDateOk);
					sqlCommand.Parameters.AddWithValue("CheckFaUserId" + i, item.CheckFaUserId == null ? (object)DBNull.Value : item.CheckFaUserId);
					sqlCommand.Parameters.AddWithValue("CheckFaUserName" + i, item.CheckFaUserName == null ? (object)DBNull.Value : item.CheckFaUserName);
					sqlCommand.Parameters.AddWithValue("CheckFaWeek" + i, item.CheckFaWeek == null ? (object)DBNull.Value : item.CheckFaWeek);
					sqlCommand.Parameters.AddWithValue("CheckFST" + i, item.CheckFST == null ? (object)DBNull.Value : item.CheckFST);
					sqlCommand.Parameters.AddWithValue("CheckFSTComments" + i, item.CheckFSTComments == null ? (object)DBNull.Value : item.CheckFSTComments);
					sqlCommand.Parameters.AddWithValue("CheckFSTDate" + i, item.CheckFSTDate == null ? (object)DBNull.Value : item.CheckFSTDate);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaOk" + i, item.CheckFSTKapaOk == null ? (object)DBNull.Value : item.CheckFSTKapaOk);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaReason" + i, item.CheckFSTKapaReason == null ? (object)DBNull.Value : item.CheckFSTKapaReason);
					sqlCommand.Parameters.AddWithValue("CheckFSTKapaWeek" + i, item.CheckFSTKapaWeek == null ? (object)DBNull.Value : item.CheckFSTKapaWeek);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserId" + i, item.CheckFSTUserId == null ? (object)DBNull.Value : item.CheckFSTUserId);
					sqlCommand.Parameters.AddWithValue("CheckFSTUserName" + i, item.CheckFSTUserName == null ? (object)DBNull.Value : item.CheckFSTUserName);
					sqlCommand.Parameters.AddWithValue("CheckINS" + i, item.CheckINS == null ? (object)DBNull.Value : item.CheckINS);
					sqlCommand.Parameters.AddWithValue("CheckINSAbConfirmed" + i, item.CheckINSAbConfirmed == null ? (object)DBNull.Value : item.CheckINSAbConfirmed);
					sqlCommand.Parameters.AddWithValue("CheckINSComments" + i, item.CheckINSComments == null ? (object)DBNull.Value : item.CheckINSComments);
					sqlCommand.Parameters.AddWithValue("CheckINSDate" + i, item.CheckINSDate == null ? (object)DBNull.Value : item.CheckINSDate);
					sqlCommand.Parameters.AddWithValue("CheckINSUserId" + i, item.CheckINSUserId == null ? (object)DBNull.Value : item.CheckINSUserId);
					sqlCommand.Parameters.AddWithValue("CheckINSUserName" + i, item.CheckINSUserName == null ? (object)DBNull.Value : item.CheckINSUserName);
					sqlCommand.Parameters.AddWithValue("CheckPRS" + i, item.CheckPRS == null ? (object)DBNull.Value : item.CheckPRS);
					sqlCommand.Parameters.AddWithValue("CheckPRSComments" + i, item.CheckPRSComments == null ? (object)DBNull.Value : item.CheckPRSComments);
					sqlCommand.Parameters.AddWithValue("CheckPRSDate" + i, item.CheckPRSDate == null ? (object)DBNull.Value : item.CheckPRSDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialLastDeliveryDate" + i, item.CheckPRSMaterialLastDeliveryDate == null ? (object)DBNull.Value : item.CheckPRSMaterialLastDeliveryDate);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialMissing" + i, item.CheckPRSMaterialMissing == null ? (object)DBNull.Value : item.CheckPRSMaterialMissing);
					sqlCommand.Parameters.AddWithValue("CheckPRSMaterialOk" + i, item.CheckPRSMaterialOk == null ? (object)DBNull.Value : item.CheckPRSMaterialOk);
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

			string query = "DELETE FROM [__CTS_InsideSalesChecksArchive] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__CTS_InsideSalesChecksArchive] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods
		#region Custom Methods
		public static int updateRevertIns(int instructionId, int userId, string username, DateTime date, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__CTS_InsideSalesChecksArchive] SET  " +
				" [RevertArchiveUserId]=@userId, [RevertArchiveUserName]=@userName, [RevertArchiveDate]=@insDate WHERE [Id]=@Id";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", instructionId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("userName", username);
				sqlCommand.Parameters.AddWithValue("insDate", date);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<InsideSalesChecksArchiveEntity> GetSalesHistoryBySearchValue(bool filterCustomers, List<int> customerIds, string searchValue, Settings.SortingModel sorting, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__CTS_InsideSalesChecksArchive]";

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
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesChecksArchiveEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesChecksArchiveEntity>();
			}
		}

		public static int Count_By_SearchValue(string searchValue)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS CountNr FROM [__CTS_InsideSalesChecksArchive] ";

				using(var sqlCommand = new SqlCommand())
				{

					if(!string.IsNullOrWhiteSpace(searchValue))
					{
						query += $@" WHERE CustomerName LIKE '%{searchValue}%' OR CustomerOrderNumber LIKE '{searchValue}%' OR OrderNumber LIKE '{searchValue}%' 
							OR ArticleNumber LIKE '{searchValue}%'";
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
		#endregion Custom Methods

	}
}
