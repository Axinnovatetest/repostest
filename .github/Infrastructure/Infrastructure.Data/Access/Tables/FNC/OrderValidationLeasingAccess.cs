using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class OrderValidationLeasingAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderValidationLeasing] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderValidationLeasing]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_OrderValidationLeasing] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_OrderValidationLeasing] ([DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[OrderArticleCount],[OrderId],[OrderIssuerId],[OrderLeasingMonthAmount],[OrderLeasingMonthAmountDefaultCurrency],[OrderLeasingYear],[OrderLeasingYearTotalAmount],[OrderLeasingYearTotalAmountDefaultCurrency],[OrderLeasingYearTotalMonths],[OrderProjectId],[OrderTotalAmount],[OrderTotalAmountDefaultCurrency],[OrderType],[UserId],[ValidationLevel],[ValidationNotes],[ValidationTime])  VALUES (@DefaultCurrencyDecimals,@DefaultCurrencyId,@DefaultCurrencyName,@DefaultCurrencyRate,@OrderArticleCount,@OrderId,@OrderIssuerId,@OrderLeasingMonthAmount,@OrderLeasingMonthAmountDefaultCurrency,@OrderLeasingYear,@OrderLeasingYearTotalAmount,@OrderLeasingYearTotalAmountDefaultCurrency,@OrderLeasingYearTotalMonths,@OrderProjectId,@OrderTotalAmount,@OrderTotalAmountDefaultCurrency,@OrderType,@UserId,@ValidationLevel,@ValidationNotes,@ValidationTime); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("OrderArticleCount", item.OrderArticleCount);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderIssuerId", item.OrderIssuerId);
					sqlCommand.Parameters.AddWithValue("OrderLeasingMonthAmount", item.OrderLeasingMonthAmount);
					sqlCommand.Parameters.AddWithValue("OrderLeasingMonthAmountDefaultCurrency", item.OrderLeasingMonthAmountDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("OrderLeasingYear", item.OrderLeasingYear == null ? (object)DBNull.Value : item.OrderLeasingYear);
					sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalAmount", item.OrderLeasingYearTotalAmount == null ? (object)DBNull.Value : item.OrderLeasingYearTotalAmount);
					sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalAmountDefaultCurrency", item.OrderLeasingYearTotalAmountDefaultCurrency == null ? (object)DBNull.Value : item.OrderLeasingYearTotalAmountDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalMonths", item.OrderLeasingYearTotalMonths == null ? (object)DBNull.Value : item.OrderLeasingYearTotalMonths);
					sqlCommand.Parameters.AddWithValue("OrderProjectId", item.OrderProjectId);
					sqlCommand.Parameters.AddWithValue("OrderTotalAmount", item.OrderTotalAmount);
					sqlCommand.Parameters.AddWithValue("OrderTotalAmountDefaultCurrency", item.OrderTotalAmountDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel);
					sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
					sqlCommand.Parameters.AddWithValue("ValidationTime", item.ValidationTime);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__FNC_OrderValidationLeasing] ([DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[OrderArticleCount],[OrderId],[OrderIssuerId],[OrderLeasingMonthAmount],[OrderLeasingMonthAmountDefaultCurrency],[OrderLeasingYear],[OrderLeasingYearTotalAmount],[OrderLeasingYearTotalAmountDefaultCurrency],[OrderLeasingYearTotalMonths],[OrderProjectId],[OrderTotalAmount],[OrderTotalAmountDefaultCurrency],[OrderType],[UserId],[ValidationLevel],[ValidationNotes],[ValidationTime]) VALUES ( "

							+ "@DefaultCurrencyDecimals" + i + ","
							+ "@DefaultCurrencyId" + i + ","
							+ "@DefaultCurrencyName" + i + ","
							+ "@DefaultCurrencyRate" + i + ","
							+ "@OrderArticleCount" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderIssuerId" + i + ","
							+ "@OrderLeasingMonthAmount" + i + ","
							+ "@OrderLeasingMonthAmountDefaultCurrency" + i + ","
							+ "@OrderLeasingYear" + i + ","
							+ "@OrderLeasingYearTotalAmount" + i + ","
							+ "@OrderLeasingYearTotalAmountDefaultCurrency" + i + ","
							+ "@OrderLeasingYearTotalMonths" + i + ","
							+ "@OrderProjectId" + i + ","
							+ "@OrderTotalAmount" + i + ","
							+ "@OrderTotalAmountDefaultCurrency" + i + ","
							+ "@OrderType" + i + ","
							+ "@UserId" + i + ","
							+ "@ValidationLevel" + i + ","
							+ "@ValidationNotes" + i + ","
							+ "@ValidationTime" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("OrderArticleCount" + i, item.OrderArticleCount);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderIssuerId" + i, item.OrderIssuerId);
						sqlCommand.Parameters.AddWithValue("OrderLeasingMonthAmount" + i, item.OrderLeasingMonthAmount);
						sqlCommand.Parameters.AddWithValue("OrderLeasingMonthAmountDefaultCurrency" + i, item.OrderLeasingMonthAmountDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("OrderLeasingYear" + i, item.OrderLeasingYear == null ? (object)DBNull.Value : item.OrderLeasingYear);
						sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalAmount" + i, item.OrderLeasingYearTotalAmount == null ? (object)DBNull.Value : item.OrderLeasingYearTotalAmount);
						sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalAmountDefaultCurrency" + i, item.OrderLeasingYearTotalAmountDefaultCurrency == null ? (object)DBNull.Value : item.OrderLeasingYearTotalAmountDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalMonths" + i, item.OrderLeasingYearTotalMonths == null ? (object)DBNull.Value : item.OrderLeasingYearTotalMonths);
						sqlCommand.Parameters.AddWithValue("OrderProjectId" + i, item.OrderProjectId);
						sqlCommand.Parameters.AddWithValue("OrderTotalAmount" + i, item.OrderTotalAmount);
						sqlCommand.Parameters.AddWithValue("OrderTotalAmountDefaultCurrency" + i, item.OrderTotalAmountDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel);
						sqlCommand.Parameters.AddWithValue("ValidationNotes" + i, item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.ValidationTime);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_OrderValidationLeasing] SET [DefaultCurrencyDecimals]=@DefaultCurrencyDecimals, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DefaultCurrencyRate]=@DefaultCurrencyRate, [OrderArticleCount]=@OrderArticleCount, [OrderId]=@OrderId, [OrderIssuerId]=@OrderIssuerId, [OrderLeasingMonthAmount]=@OrderLeasingMonthAmount, [OrderLeasingMonthAmountDefaultCurrency]=@OrderLeasingMonthAmountDefaultCurrency, [OrderLeasingYear]=@OrderLeasingYear, [OrderLeasingYearTotalAmount]=@OrderLeasingYearTotalAmount, [OrderLeasingYearTotalAmountDefaultCurrency]=@OrderLeasingYearTotalAmountDefaultCurrency, [OrderLeasingYearTotalMonths]=@OrderLeasingYearTotalMonths, [OrderProjectId]=@OrderProjectId, [OrderTotalAmount]=@OrderTotalAmount, [OrderTotalAmountDefaultCurrency]=@OrderTotalAmountDefaultCurrency, [OrderType]=@OrderType, [UserId]=@UserId, [ValidationLevel]=@ValidationLevel, [ValidationNotes]=@ValidationNotes, [ValidationTime]=@ValidationTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
				sqlCommand.Parameters.AddWithValue("OrderArticleCount", item.OrderArticleCount);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderIssuerId", item.OrderIssuerId);
				sqlCommand.Parameters.AddWithValue("OrderLeasingMonthAmount", item.OrderLeasingMonthAmount);
				sqlCommand.Parameters.AddWithValue("OrderLeasingMonthAmountDefaultCurrency", item.OrderLeasingMonthAmountDefaultCurrency);
				sqlCommand.Parameters.AddWithValue("OrderLeasingYear", item.OrderLeasingYear == null ? (object)DBNull.Value : item.OrderLeasingYear);
				sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalAmount", item.OrderLeasingYearTotalAmount == null ? (object)DBNull.Value : item.OrderLeasingYearTotalAmount);
				sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalAmountDefaultCurrency", item.OrderLeasingYearTotalAmountDefaultCurrency == null ? (object)DBNull.Value : item.OrderLeasingYearTotalAmountDefaultCurrency);
				sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalMonths", item.OrderLeasingYearTotalMonths == null ? (object)DBNull.Value : item.OrderLeasingYearTotalMonths);
				sqlCommand.Parameters.AddWithValue("OrderProjectId", item.OrderProjectId);
				sqlCommand.Parameters.AddWithValue("OrderTotalAmount", item.OrderTotalAmount);
				sqlCommand.Parameters.AddWithValue("OrderTotalAmountDefaultCurrency", item.OrderTotalAmountDefaultCurrency);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel);
				sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
				sqlCommand.Parameters.AddWithValue("ValidationTime", item.ValidationTime);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__FNC_OrderValidationLeasing] SET "

							+ "[DefaultCurrencyDecimals]=@DefaultCurrencyDecimals" + i + ","
							+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
							+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
							+ "[DefaultCurrencyRate]=@DefaultCurrencyRate" + i + ","
							+ "[OrderArticleCount]=@OrderArticleCount" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderIssuerId]=@OrderIssuerId" + i + ","
							+ "[OrderLeasingMonthAmount]=@OrderLeasingMonthAmount" + i + ","
							+ "[OrderLeasingMonthAmountDefaultCurrency]=@OrderLeasingMonthAmountDefaultCurrency" + i + ","
							+ "[OrderLeasingYear]=@OrderLeasingYear" + i + ","
							+ "[OrderLeasingYearTotalAmount]=@OrderLeasingYearTotalAmount" + i + ","
							+ "[OrderLeasingYearTotalAmountDefaultCurrency]=@OrderLeasingYearTotalAmountDefaultCurrency" + i + ","
							+ "[OrderLeasingYearTotalMonths]=@OrderLeasingYearTotalMonths" + i + ","
							+ "[OrderProjectId]=@OrderProjectId" + i + ","
							+ "[OrderTotalAmount]=@OrderTotalAmount" + i + ","
							+ "[OrderTotalAmountDefaultCurrency]=@OrderTotalAmountDefaultCurrency" + i + ","
							+ "[OrderType]=@OrderType" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[ValidationLevel]=@ValidationLevel" + i + ","
							+ "[ValidationNotes]=@ValidationNotes" + i + ","
							+ "[ValidationTime]=@ValidationTime" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("OrderArticleCount" + i, item.OrderArticleCount);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderIssuerId" + i, item.OrderIssuerId);
						sqlCommand.Parameters.AddWithValue("OrderLeasingMonthAmount" + i, item.OrderLeasingMonthAmount);
						sqlCommand.Parameters.AddWithValue("OrderLeasingMonthAmountDefaultCurrency" + i, item.OrderLeasingMonthAmountDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("OrderLeasingYear" + i, item.OrderLeasingYear == null ? (object)DBNull.Value : item.OrderLeasingYear);
						sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalAmount" + i, item.OrderLeasingYearTotalAmount == null ? (object)DBNull.Value : item.OrderLeasingYearTotalAmount);
						sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalAmountDefaultCurrency" + i, item.OrderLeasingYearTotalAmountDefaultCurrency == null ? (object)DBNull.Value : item.OrderLeasingYearTotalAmountDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("OrderLeasingYearTotalMonths" + i, item.OrderLeasingYearTotalMonths == null ? (object)DBNull.Value : item.OrderLeasingYearTotalMonths);
						sqlCommand.Parameters.AddWithValue("OrderProjectId" + i, item.OrderProjectId);
						sqlCommand.Parameters.AddWithValue("OrderTotalAmount" + i, item.OrderTotalAmount);
						sqlCommand.Parameters.AddWithValue("OrderTotalAmountDefaultCurrency" + i, item.OrderTotalAmountDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel);
						sqlCommand.Parameters.AddWithValue("ValidationNotes" + i, item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.ValidationTime);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_OrderValidationLeasing] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [__FNC_OrderValidationLeasing] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderValidationLeasing] WHERE OrderId=@orderId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity> GetFirstByOrderId(List<int> orderIds)
		{
			if(orderIds == null || orderIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_OrderValidationLeasing] WHERE OrderId IN ({string.Join(",", orderIds)}) AND [ValidationLevel]=0";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity>();
			}
		}

		public static int DeleteByOrderId(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_OrderValidationLeasing] WHERE [OrderId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion
	}
}
