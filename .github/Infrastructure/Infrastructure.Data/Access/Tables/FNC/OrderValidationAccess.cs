using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class OrderValidationAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderValidation] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderValidation]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_OrderValidation] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_OrderValidation] ([OrderArticleCount],[OrderId],[OrderProjectId],[OrderTotalAmount],[OrderType],[OrderUserId],[UserEmail],[UserId],[Username],[ValidationLevel],[ValidationNotes],[ValidationTime])  VALUES (@OrderArticleCount,@OrderId,@OrderProjectId,@OrderTotalAmount,@OrderType,@OrderUserId,@UserEmail,@UserId,@Username,@ValidationLevel,@ValidationNotes,@ValidationTime); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("OrderArticleCount", item.OrderArticleCount);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderProjectId", item.OrderProjectId);
					sqlCommand.Parameters.AddWithValue("OrderTotalAmount", item.OrderTotalAmount);
					sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
					sqlCommand.Parameters.AddWithValue("OrderUserId", item.OrderUserId);
					sqlCommand.Parameters.AddWithValue("UserEmail", item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);
					sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel);
					sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
					sqlCommand.Parameters.AddWithValue("ValidationTime", item.ValidationTime);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 13; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity> items)
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
						query += " INSERT INTO [__FNC_OrderValidation] ([OrderArticleCount],[OrderId],[OrderProjectId],[OrderTotalAmount],[OrderType],[OrderUserId],[UserEmail],[UserId],[Username],[ValidationLevel],[ValidationNotes],[ValidationTime]) VALUES ( "

							+ "@OrderArticleCount" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderProjectId" + i + ","
							+ "@OrderTotalAmount" + i + ","
							+ "@OrderType" + i + ","
							+ "@OrderUserId" + i + ","
							+ "@UserEmail" + i + ","
							+ "@UserId" + i + ","
							+ "@Username" + i + ","
							+ "@ValidationLevel" + i + ","
							+ "@ValidationNotes" + i + ","
							+ "@ValidationTime" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("OrderArticleCount" + i, item.OrderArticleCount);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderProjectId" + i, item.OrderProjectId);
						sqlCommand.Parameters.AddWithValue("OrderTotalAmount" + i, item.OrderTotalAmount);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("OrderUserId" + i, item.OrderUserId);
						sqlCommand.Parameters.AddWithValue("UserEmail" + i, item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel);
						sqlCommand.Parameters.AddWithValue("ValidationNotes" + i, item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.ValidationTime);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_OrderValidation] SET [OrderArticleCount]=@OrderArticleCount, [OrderId]=@OrderId, [OrderProjectId]=@OrderProjectId, [OrderTotalAmount]=@OrderTotalAmount, [OrderType]=@OrderType, [OrderUserId]=@OrderUserId, [UserEmail]=@UserEmail, [UserId]=@UserId, [Username]=@Username, [ValidationLevel]=@ValidationLevel, [ValidationNotes]=@ValidationNotes, [ValidationTime]=@ValidationTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("OrderArticleCount", item.OrderArticleCount);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderProjectId", item.OrderProjectId);
				sqlCommand.Parameters.AddWithValue("OrderTotalAmount", item.OrderTotalAmount);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
				sqlCommand.Parameters.AddWithValue("OrderUserId", item.OrderUserId);
				sqlCommand.Parameters.AddWithValue("UserEmail", item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);
				sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel);
				sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
				sqlCommand.Parameters.AddWithValue("ValidationTime", item.ValidationTime);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 13; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity> items)
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
						query += " UPDATE [__FNC_OrderValidation] SET "

							+ "[OrderArticleCount]=@OrderArticleCount" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderProjectId]=@OrderProjectId" + i + ","
							+ "[OrderTotalAmount]=@OrderTotalAmount" + i + ","
							+ "[OrderType]=@OrderType" + i + ","
							+ "[OrderUserId]=@OrderUserId" + i + ","
							+ "[UserEmail]=@UserEmail" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[Username]=@Username" + i + ","
							+ "[ValidationLevel]=@ValidationLevel" + i + ","
							+ "[ValidationNotes]=@ValidationNotes" + i + ","
							+ "[ValidationTime]=@ValidationTime" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("OrderArticleCount" + i, item.OrderArticleCount);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderProjectId" + i, item.OrderProjectId);
						sqlCommand.Parameters.AddWithValue("OrderTotalAmount" + i, item.OrderTotalAmount);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("OrderUserId" + i, item.OrderUserId);
						sqlCommand.Parameters.AddWithValue("UserEmail" + i, item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel);
						sqlCommand.Parameters.AddWithValue("ValidationNotes" + i, item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.ValidationTime);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_OrderValidation] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_OrderValidation] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity GetLastByOrderId(int orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderValidation] WHERE [OrderId]=@orderId AND ValidationTime = (SELECT MAX(ValidationTime) FROM [__FNC_OrderValidation] WHERE [OrderId]=@orderId)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
