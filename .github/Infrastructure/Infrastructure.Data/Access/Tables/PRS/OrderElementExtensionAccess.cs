using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class OrderItemExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderElementExtension] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_OrderElementExtension]", sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [__EDI_OrderElementExtension] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_OrderElementExtension] "
					+ " ([CreationDate],[CreationUserId],[OrderElementId],[OrderId],[OriginalGesamtpreis], "
					+ " [OriginalQuantity],[OriginalVKGesamtpreis],[Status],[DeliveryDate], "
					+ " [Version],[LastUpdateTime],[LastUpdateUserId],[LastUpdateUsername],[PrimaryPositionNumber],[SecondaryPositionsCount])"
					+ " VALUES "
					+ " (@CreationDate,@CreationUserId,@OrderElementId,@OrderId,@OriginalGesamtpreis,"
					+ " @OriginalQuantity,@OriginalVKGesamtpreis,@Status,@DeliveryDate, "
					+ " @Version,@LastUpdateTime,@LastUpdateUserId,@LastUpdateUsername,@PrimaryPositionNumber,@SecondaryPositionsCount);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("OrderElementId", item.OrderItemId);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OriginalGesamtpreis", item.OriginalGesamtpreis);
					sqlCommand.Parameters.AddWithValue("OriginalQuantity", item.OriginalQuantity);
					sqlCommand.Parameters.AddWithValue("OriginalVKGesamtpreis", item.OriginalVKGesamtpreis);
					sqlCommand.Parameters.AddWithValue("Status", item.Status);
					sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DesiredDate == null ? (object)DBNull.Value : item.DesiredDate);

					sqlCommand.Parameters.AddWithValue("Version", item.Version);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername);

					sqlCommand.Parameters.AddWithValue("PrimaryPositionNumber", item.PrimaryPositionNumber == null ? (object)DBNull.Value : item.PrimaryPositionNumber);
					sqlCommand.Parameters.AddWithValue("SecondaryPositionsCount", item.SecondaryPositionsCount == null ? (object)DBNull.Value : item.SecondaryPositionsCount);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity item)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [__EDI_OrderElementExtension] SET "
					+ " [CreationDate]=@CreationDate, [CreationUserId]=@CreationUserId, [OrderElementId]=@OrderElementId, "
					+ " [OrderId]=@OrderId, [OriginalGesamtpreis]=@OriginalGesamtpreis, [OriginalQuantity]=@OriginalQuantity, "
					+ " [OriginalVKGesamtpreis]=@OriginalVKGesamtpreis, [Status]=@Status, [DeliveryDate]=@DeliveryDate, "
					+ " Version=@Version, [LastUpdateTime]=@LastUpdateTime, LastUpdateUserId=@LastUpdateUserId, "
					+ " [LastUpdateUsername]=@LastUpdateUsername, [PrimaryPositionNumber]=@PrimaryPositionNumber, [SecondaryPositionsCount]=@SecondaryPositionsCount  "
					+ " WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("OrderElementId", item.OrderItemId);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OriginalGesamtpreis", item.OriginalGesamtpreis);
				sqlCommand.Parameters.AddWithValue("OriginalQuantity", item.OriginalQuantity);
				sqlCommand.Parameters.AddWithValue("OriginalVKGesamtpreis", item.OriginalVKGesamtpreis);
				sqlCommand.Parameters.AddWithValue("Status", item.Status);
				sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DesiredDate == null ? (object)DBNull.Value : item.DesiredDate);

				sqlCommand.Parameters.AddWithValue("Version", item.Version);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername);

				sqlCommand.Parameters.AddWithValue("PrimaryPositionNumber", item.PrimaryPositionNumber == null ? (object)DBNull.Value : item.PrimaryPositionNumber);
				sqlCommand.Parameters.AddWithValue("SecondaryPositionsCount", item.SecondaryPositionsCount == null ? (object)DBNull.Value : item.SecondaryPositionsCount);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__EDI_OrderElementExtension] WHERE [Id]=@Id";
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
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [__EDI_OrderElementExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int UpdateStatus(Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_OrderElementExtension] SET [Status]=@Status WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Status", item.Status);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int UpdateStatus(Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int results = -1;
			string query = "UPDATE [__EDI_OrderElementExtension] SET [Status]=@Status WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Status", item.Status);

			results = sqlCommand.ExecuteNonQuery();

			return results;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity GetByOderIdAndOrderItemId(int orderId, int orderElementId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_OrderElementExtension] WHERE [OrderId]=@orderId ANd [OrderElementId]=@orderElementId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);
				sqlCommand.Parameters.AddWithValue("orderElementId", orderElementId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.OrderItemExtensionEntity> GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderElementExtension] WHERE [OrderId]=@orderId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.OrderItemExtensionEntity> { };
			}
		}
		public static List<Entities.Tables.PRS.OrderItemExtensionEntity> GetByOrderId(int orderId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_OrderElementExtension] WHERE [OrderId]=@orderId";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("orderId", orderId);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.OrderItemExtensionEntity> { };
			}
		}

		public static Entities.Tables.PRS.OrderItemExtensionEntity GetByOrderElementId(int orderElementId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderElementExtension] WHERE [OrderElementId]=@orderElementId ORDER BY CreationDate DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderElementId", orderElementId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.OrderItemExtensionEntity GetByOrderElementId(int orderElementId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_OrderElementExtension] WHERE [OrderElementId]=@orderElementId ORDER BY CreationDate DESC";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("orderElementId", orderElementId);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.OrderItemExtensionEntity> GetSecondaryByOrderId(int orderId, int positionNumber)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderElementExtension] WHERE PrimaryPositionNumber=@positionNumber AND [OrderId]=@OrderId ORDER BY CreationDate DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("OrderId", orderId);
				sqlCommand.Parameters.AddWithValue("positionNumber", positionNumber);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.OrderItemExtensionEntity> GetSecondaryByOrderId(int orderId, int positionNumber, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_OrderElementExtension] WHERE PrimaryPositionNumber=@positionNumber AND [OrderId]=@OrderId ORDER BY CreationDate DESC";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("OrderId", orderId);
			sqlCommand.Parameters.AddWithValue("positionNumber", positionNumber);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity> GetByOrderItemsIds(List<int> orderItemsIds)
		{
			if(orderItemsIds != null && orderItemsIds.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var results = new List<Entities.Tables.PRS.OrderItemExtensionEntity>();
				if(orderItemsIds.Count <= maxQueryNumber)
				{
					results = getByOrderItemsIds(orderItemsIds);
				}
				else
				{
					int batchNumber = orderItemsIds.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByOrderItemsIds(orderItemsIds.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByOrderItemsIds(orderItemsIds.GetRange(batchNumber * maxQueryNumber, orderItemsIds.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity> getByOrderItemsIds(List<int> orderItemsIds)
		{
			if(orderItemsIds != null && orderItemsIds.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < orderItemsIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, orderItemsIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [__EDI_OrderElementExtension] WHERE [OrderElementId] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>();
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity(dataRow)); }
			return list;
		}
		#endregion

		#region Querys with transactions
		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity item, SqlConnection conncetion, SqlTransaction transaction)
		{
			int response = -1;

			//using (var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			//{
			//    sqlConnection.Open();
			//    var sqlTransaction = sqlConnection.BeginTransaction();

			string query = "INSERT INTO [__EDI_OrderElementExtension] "
				+ " ([CreationDate],[CreationUserId],[OrderElementId],[OrderId],[OriginalGesamtpreis], "
				+ " [OriginalQuantity],[OriginalVKGesamtpreis],[Status],[DeliveryDate], "
				+ " [Version],[LastUpdateTime],[LastUpdateUserId],[LastUpdateUsername],[PrimaryPositionNumber],[SecondaryPositionsCount])"
				+ " VALUES "
				+ " (@CreationDate,@CreationUserId,@OrderElementId,@OrderId,@OriginalGesamtpreis,"
				+ " @OriginalQuantity,@OriginalVKGesamtpreis,@Status,@DeliveryDate, "
				+ " @Version,@LastUpdateTime,@LastUpdateUserId,@LastUpdateUsername,@PrimaryPositionNumber,@SecondaryPositionsCount);";
			query += "SELECT SCOPE_IDENTITY();";

			//using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			//{
			var sqlCommand = new SqlCommand(query, conncetion, transaction);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("OrderElementId", item.OrderItemId);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("OriginalGesamtpreis", item.OriginalGesamtpreis);
			sqlCommand.Parameters.AddWithValue("OriginalQuantity", item.OriginalQuantity);
			sqlCommand.Parameters.AddWithValue("OriginalVKGesamtpreis", item.OriginalVKGesamtpreis);
			sqlCommand.Parameters.AddWithValue("Status", item.Status);
			sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DesiredDate == null ? (object)DBNull.Value : item.DesiredDate);

			sqlCommand.Parameters.AddWithValue("Version", item.Version);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername);

			sqlCommand.Parameters.AddWithValue("PrimaryPositionNumber", item.PrimaryPositionNumber == null ? (object)DBNull.Value : item.PrimaryPositionNumber);
			sqlCommand.Parameters.AddWithValue("SecondaryPositionsCount", item.SecondaryPositionsCount == null ? (object)DBNull.Value : item.SecondaryPositionsCount);

			var result = sqlCommand.ExecuteScalar();
			response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			//}

			//sqlTransaction.Commit();

			return response;
			//}
		}
		#endregion
	}
}
