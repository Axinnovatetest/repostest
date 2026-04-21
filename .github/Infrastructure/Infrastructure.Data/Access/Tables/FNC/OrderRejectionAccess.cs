using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class OrderRejectionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderRejection] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderRejection]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_OrderRejection] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_OrderRejection] ([OrderArticleCount],[OrderId],[OrderProjectId],[OrderTotalAmount],[OrderType],[OrderUserId],[RejectionLevel],[RejectionNotes],[RejectionTime],[UserId])  VALUES (@OrderArticleCount,@OrderId,@OrderProjectId,@OrderTotalAmount,@OrderType,@OrderUserId,@RejectionLevel,@RejectionNotes,@RejectionTime,@UserId); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("OrderArticleCount", item.OrderArticleCount);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderProjectId", item.OrderProjectId);
					sqlCommand.Parameters.AddWithValue("OrderTotalAmount", item.OrderTotalAmount);
					sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
					sqlCommand.Parameters.AddWithValue("OrderUserId", item.OrderUserId);
					sqlCommand.Parameters.AddWithValue("RejectionLevel", item.RejectionLevel);
					sqlCommand.Parameters.AddWithValue("RejectionNotes", item.RejectionNotes == null ? (object)DBNull.Value : item.RejectionNotes);
					sqlCommand.Parameters.AddWithValue("RejectionTime", item.RejectionTime);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity> items)
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
						query += " INSERT INTO [__FNC_OrderRejection] ([OrderArticleCount],[OrderId],[OrderProjectId],[OrderTotalAmount],[OrderType],[OrderUserId],[RejectionLevel],[RejectionNotes],[RejectionTime],[UserId]) VALUES ( "

							+ "@OrderArticleCount" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderProjectId" + i + ","
							+ "@OrderTotalAmount" + i + ","
							+ "@OrderType" + i + ","
							+ "@OrderUserId" + i + ","
							+ "@RejectionLevel" + i + ","
							+ "@RejectionNotes" + i + ","
							+ "@RejectionTime" + i + ","
							+ "@UserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("OrderArticleCount" + i, item.OrderArticleCount);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderProjectId" + i, item.OrderProjectId);
						sqlCommand.Parameters.AddWithValue("OrderTotalAmount" + i, item.OrderTotalAmount);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("OrderUserId" + i, item.OrderUserId);
						sqlCommand.Parameters.AddWithValue("RejectionLevel" + i, item.RejectionLevel);
						sqlCommand.Parameters.AddWithValue("RejectionNotes" + i, item.RejectionNotes == null ? (object)DBNull.Value : item.RejectionNotes);
						sqlCommand.Parameters.AddWithValue("RejectionTime" + i, item.RejectionTime);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_OrderRejection] SET [OrderArticleCount]=@OrderArticleCount, [OrderId]=@OrderId, [OrderProjectId]=@OrderProjectId, [OrderTotalAmount]=@OrderTotalAmount, [OrderType]=@OrderType, [OrderUserId]=@OrderUserId, [RejectionLevel]=@RejectionLevel, [RejectionNotes]=@RejectionNotes, [RejectionTime]=@RejectionTime, [UserId]=@UserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("OrderArticleCount", item.OrderArticleCount);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderProjectId", item.OrderProjectId);
				sqlCommand.Parameters.AddWithValue("OrderTotalAmount", item.OrderTotalAmount);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
				sqlCommand.Parameters.AddWithValue("OrderUserId", item.OrderUserId);
				sqlCommand.Parameters.AddWithValue("RejectionLevel", item.RejectionLevel);
				sqlCommand.Parameters.AddWithValue("RejectionNotes", item.RejectionNotes == null ? (object)DBNull.Value : item.RejectionNotes);
				sqlCommand.Parameters.AddWithValue("RejectionTime", item.RejectionTime);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity> items)
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
						query += " UPDATE [__FNC_OrderRejection] SET "

							+ "[OrderArticleCount]=@OrderArticleCount" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderProjectId]=@OrderProjectId" + i + ","
							+ "[OrderTotalAmount]=@OrderTotalAmount" + i + ","
							+ "[OrderType]=@OrderType" + i + ","
							+ "[OrderUserId]=@OrderUserId" + i + ","
							+ "[RejectionLevel]=@RejectionLevel" + i + ","
							+ "[RejectionNotes]=@RejectionNotes" + i + ","
							+ "[RejectionTime]=@RejectionTime" + i + ","
							+ "[UserId]=@UserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("OrderArticleCount" + i, item.OrderArticleCount);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderProjectId" + i, item.OrderProjectId);
						sqlCommand.Parameters.AddWithValue("OrderTotalAmount" + i, item.OrderTotalAmount);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("OrderUserId" + i, item.OrderUserId);
						sqlCommand.Parameters.AddWithValue("RejectionLevel" + i, item.RejectionLevel);
						sqlCommand.Parameters.AddWithValue("RejectionNotes" + i, item.RejectionNotes == null ? (object)DBNull.Value : item.RejectionNotes);
						sqlCommand.Parameters.AddWithValue("RejectionTime" + i, item.RejectionTime);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
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
				string query = "DELETE FROM [__FNC_OrderRejection] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_OrderRejection] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods



		#endregion
	}
}
