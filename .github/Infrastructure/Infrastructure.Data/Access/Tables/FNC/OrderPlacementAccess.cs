using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class OrderPlacementAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderPlacement] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderPlacement]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_OrderPlacement] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_OrderPlacement] ([OrderId],[OrderPlacedEmailMessage],[OrderPlacedEmailTitle],[OrderPlacedReportFileId],[OrderPlacedSendingEmail],[OrderPlacedSupplierEmail],[OrderPlacedTime],[OrderPlacedUserEmail],[OrderPlacedUserId],[OrderPlacedUserName],[OrderPlacementCCEmail],[SupplierEmail],[SupplierNummer])  VALUES (@OrderId,@OrderPlacedEmailMessage,@OrderPlacedEmailTitle,@OrderPlacedReportFileId,@OrderPlacedSendingEmail,@OrderPlacedSupplierEmail,@OrderPlacedTime,@OrderPlacedUserEmail,@OrderPlacedUserId,@OrderPlacedUserName,@OrderPlacementCCEmail,@SupplierEmail,@SupplierNummer); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage", item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle", item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
					sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId", item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail", item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail", item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedTime", item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail", item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserId", item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserName", item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
					sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail", item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
					sqlCommand.Parameters.AddWithValue("SupplierEmail", item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
					sqlCommand.Parameters.AddWithValue("SupplierNummer", item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> items)
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
						query += " INSERT INTO [__FNC_OrderPlacement] ([OrderId],[OrderPlacedEmailMessage],[OrderPlacedEmailTitle],[OrderPlacedReportFileId],[OrderPlacedSendingEmail],[OrderPlacedSupplierEmail],[OrderPlacedTime],[OrderPlacedUserEmail],[OrderPlacedUserId],[OrderPlacedUserName],[OrderPlacementCCEmail],[SupplierEmail],[SupplierNummer]) VALUES ( "

							+ "@OrderId" + i + ","
							+ "@OrderPlacedEmailMessage" + i + ","
							+ "@OrderPlacedEmailTitle" + i + ","
							+ "@OrderPlacedReportFileId" + i + ","
							+ "@OrderPlacedSendingEmail" + i + ","
							+ "@OrderPlacedSupplierEmail" + i + ","
							+ "@OrderPlacedTime" + i + ","
							+ "@OrderPlacedUserEmail" + i + ","
							+ "@OrderPlacedUserId" + i + ","
							+ "@OrderPlacedUserName" + i + ","
							+ "@OrderPlacementCCEmail" + i + ","
							+ "@SupplierEmail" + i + ","
							+ "@SupplierNummer" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage" + i, item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle" + i, item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
						sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId" + i, item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail" + i, item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail" + i, item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedTime" + i, item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail" + i, item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserId" + i, item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserName" + i, item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
						sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail" + i, item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
						sqlCommand.Parameters.AddWithValue("SupplierEmail" + i, item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
						sqlCommand.Parameters.AddWithValue("SupplierNummer" + i, item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_OrderPlacement] SET [OrderId]=@OrderId, [OrderPlacedEmailMessage]=@OrderPlacedEmailMessage, [OrderPlacedEmailTitle]=@OrderPlacedEmailTitle, [OrderPlacedReportFileId]=@OrderPlacedReportFileId, [OrderPlacedSendingEmail]=@OrderPlacedSendingEmail, [OrderPlacedSupplierEmail]=@OrderPlacedSupplierEmail, [OrderPlacedTime]=@OrderPlacedTime, [OrderPlacedUserEmail]=@OrderPlacedUserEmail, [OrderPlacedUserId]=@OrderPlacedUserId, [OrderPlacedUserName]=@OrderPlacedUserName, [OrderPlacementCCEmail]=@OrderPlacementCCEmail, [SupplierEmail]=@SupplierEmail, [SupplierNummer]=@SupplierNummer WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage", item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
				sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle", item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
				sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId", item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
				sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail", item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail", item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedTime", item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail", item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserId", item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserName", item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
				sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail", item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
				sqlCommand.Parameters.AddWithValue("SupplierEmail", item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
				sqlCommand.Parameters.AddWithValue("SupplierNummer", item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> items)
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
						query += " UPDATE [__FNC_OrderPlacement] SET "

							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderPlacedEmailMessage]=@OrderPlacedEmailMessage" + i + ","
							+ "[OrderPlacedEmailTitle]=@OrderPlacedEmailTitle" + i + ","
							+ "[OrderPlacedReportFileId]=@OrderPlacedReportFileId" + i + ","
							+ "[OrderPlacedSendingEmail]=@OrderPlacedSendingEmail" + i + ","
							+ "[OrderPlacedSupplierEmail]=@OrderPlacedSupplierEmail" + i + ","
							+ "[OrderPlacedTime]=@OrderPlacedTime" + i + ","
							+ "[OrderPlacedUserEmail]=@OrderPlacedUserEmail" + i + ","
							+ "[OrderPlacedUserId]=@OrderPlacedUserId" + i + ","
							+ "[OrderPlacedUserName]=@OrderPlacedUserName" + i + ","
							+ "[OrderPlacementCCEmail]=@OrderPlacementCCEmail" + i + ","
							+ "[SupplierEmail]=@SupplierEmail" + i + ","
							+ "[SupplierNummer]=@SupplierNummer" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage" + i, item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle" + i, item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
						sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId" + i, item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail" + i, item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail" + i, item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedTime" + i, item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail" + i, item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserId" + i, item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserName" + i, item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
						sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail" + i, item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
						sqlCommand.Parameters.AddWithValue("SupplierEmail" + i, item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
						sqlCommand.Parameters.AddWithValue("SupplierNummer" + i, item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
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
				string query = "DELETE FROM [__FNC_OrderPlacement] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_OrderPlacement] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity> GetByOrderId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderPlacement] WHERE [OrderId]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity GetLastByOrderId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderPlacement] WHERE [OrderId]=@Id AND OrderPlacedTime = (SELECT MAX(OrderPlacedTime) FROM [__FNC_OrderPlacement] WHERE [OrderId]=@Id)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
