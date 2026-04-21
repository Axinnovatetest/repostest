using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class LineItemAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.LineItemEntity Get(long id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_LineItem] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_LineItem]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> Get(List<long> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> get(List<long> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_LineItem] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
		}

		public static long Insert(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity item)
		{
			long response = long.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_DLF_LineItem] ([ArticleId],[BuyersInternalProductGroupCode],[CallOffDateTime],[CallOffNumber],[CumulativeReceivedQuantity],[CumulativeScheduledQuantity],[CustomersItemMaterialNumber],[DocumentNumber],[DrawingRevisionNumber],[HeaderId],[HeaderPreviousVersion],[HeaderVersion],[LastASNDate],[LastASNDeliveryDate],[LastASNNumber],[LastReceivedQuantity],[MaterialAuthorizationDate],[MaterialAuthorizationQuantity],[PlanningHorizionEnd],[PlanningHorizionStart],[PositionNumber],[PreviousCallOffDate],[PreviousCallOffNumber],[ProductionAuthorizationDateTime],[ProductionAuthorizationQuantity],[StorageLocation],[SuppliersItemMaterialNumber],[UnloadingPoint]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@BuyersInternalProductGroupCode,@CallOffDateTime,@CallOffNumber,@CumulativeReceivedQuantity,@CumulativeScheduledQuantity,@CustomersItemMaterialNumber,@DocumentNumber,@DrawingRevisionNumber,@HeaderId,@HeaderPreviousVersion,@HeaderVersion,@LastASNDate,@LastASNDeliveryDate,@LastASNNumber,@LastReceivedQuantity,@MaterialAuthorizationDate,@MaterialAuthorizationQuantity,@PlanningHorizionEnd,@PlanningHorizionStart,@PositionNumber,@PreviousCallOffDate,@PreviousCallOffNumber,@ProductionAuthorizationDateTime,@ProductionAuthorizationQuantity,@StorageLocation,@SuppliersItemMaterialNumber,@UnloadingPoint); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("BuyersInternalProductGroupCode", item.BuyersInternalProductGroupCode == null ? (object)DBNull.Value : item.BuyersInternalProductGroupCode);
					sqlCommand.Parameters.AddWithValue("CallOffDateTime", item.CallOffDateTime == null ? (object)DBNull.Value : item.CallOffDateTime);
					sqlCommand.Parameters.AddWithValue("CallOffNumber", item.CallOffNumber == null ? (object)DBNull.Value : item.CallOffNumber);
					sqlCommand.Parameters.AddWithValue("CumulativeReceivedQuantity", item.CumulativeReceivedQuantity == null ? (object)DBNull.Value : item.CumulativeReceivedQuantity);
					sqlCommand.Parameters.AddWithValue("CumulativeScheduledQuantity", item.CumulativeScheduledQuantity == null ? (object)DBNull.Value : item.CumulativeScheduledQuantity);
					sqlCommand.Parameters.AddWithValue("CustomersItemMaterialNumber", item.CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.CustomersItemMaterialNumber);
					sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
					sqlCommand.Parameters.AddWithValue("DrawingRevisionNumber", item.DrawingRevisionNumber == null ? (object)DBNull.Value : item.DrawingRevisionNumber);
					sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId);
					sqlCommand.Parameters.AddWithValue("HeaderPreviousVersion", item.HeaderPreviousVersion == null ? (object)DBNull.Value : item.HeaderPreviousVersion);
					sqlCommand.Parameters.AddWithValue("HeaderVersion", item.HeaderVersion == null ? (object)DBNull.Value : item.HeaderVersion);
					sqlCommand.Parameters.AddWithValue("LastASNDate", item.LastASNDate == null ? (object)DBNull.Value : item.LastASNDate);
					sqlCommand.Parameters.AddWithValue("LastASNDeliveryDate", item.LastASNDeliveryDate == null ? (object)DBNull.Value : item.LastASNDeliveryDate);
					sqlCommand.Parameters.AddWithValue("LastASNNumber", item.LastASNNumber == null ? (object)DBNull.Value : item.LastASNNumber);
					sqlCommand.Parameters.AddWithValue("LastReceivedQuantity", item.LastReceivedQuantity == null ? (object)DBNull.Value : item.LastReceivedQuantity);
					sqlCommand.Parameters.AddWithValue("MaterialAuthorizationDate", item.MaterialAuthorizationDate == null ? (object)DBNull.Value : item.MaterialAuthorizationDate);
					sqlCommand.Parameters.AddWithValue("MaterialAuthorizationQuantity", item.MaterialAuthorizationQuantity == null ? (object)DBNull.Value : item.MaterialAuthorizationQuantity);
					sqlCommand.Parameters.AddWithValue("PlanningHorizionEnd", item.PlanningHorizionEnd == null ? (object)DBNull.Value : item.PlanningHorizionEnd);
					sqlCommand.Parameters.AddWithValue("PlanningHorizionStart", item.PlanningHorizionStart == null ? (object)DBNull.Value : item.PlanningHorizionStart);
					sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
					sqlCommand.Parameters.AddWithValue("PreviousCallOffDate", item.PreviousCallOffDate == null ? (object)DBNull.Value : item.PreviousCallOffDate);
					sqlCommand.Parameters.AddWithValue("PreviousCallOffNumber", item.PreviousCallOffNumber == null ? (object)DBNull.Value : item.PreviousCallOffNumber);
					sqlCommand.Parameters.AddWithValue("ProductionAuthorizationDateTime", item.ProductionAuthorizationDateTime == null ? (object)DBNull.Value : item.ProductionAuthorizationDateTime);
					sqlCommand.Parameters.AddWithValue("ProductionAuthorizationQuantity", item.ProductionAuthorizationQuantity == null ? (object)DBNull.Value : item.ProductionAuthorizationQuantity);
					sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
					sqlCommand.Parameters.AddWithValue("SuppliersItemMaterialNumber", item.SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.SuppliersItemMaterialNumber);
					sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; /* Nb params per query */
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__EDI_DLF_LineItem] ([ArticleId],[BuyersInternalProductGroupCode],[CallOffDateTime],[CallOffNumber],[CumulativeReceivedQuantity],[CumulativeScheduledQuantity],[CustomersItemMaterialNumber],[DocumentNumber],[DrawingRevisionNumber],[HeaderId],[HeaderPreviousVersion],[HeaderVersion],[LastASNDate],[LastASNDeliveryDate],[LastASNNumber],[LastReceivedQuantity],[MaterialAuthorizationDate],[MaterialAuthorizationQuantity],[PlanningHorizionEnd],[PlanningHorizionStart],[PositionNumber],[PreviousCallOffDate],[PreviousCallOffNumber],[ProductionAuthorizationDateTime],[ProductionAuthorizationQuantity],[StorageLocation],[SuppliersItemMaterialNumber],[UnloadingPoint]) VALUES ("

							+ "@ArticleId" + i + ","
							+ "@BuyersInternalProductGroupCode" + i + ","
							+ "@CallOffDateTime" + i + ","
							+ "@CallOffNumber" + i + ","
							+ "@CumulativeReceivedQuantity" + i + ","
							+ "@CumulativeScheduledQuantity" + i + ","
							+ "@CustomersItemMaterialNumber" + i + ","
							+ "@DocumentNumber" + i + ","
							+ "@DrawingRevisionNumber" + i + ","
							+ "@HeaderId" + i + ","
							+ "@HeaderPreviousVersion" + i + ","
							+ "@HeaderVersion" + i + ","
							+ "@LastASNDate" + i + ","
							+ "@LastASNDeliveryDate" + i + ","
							+ "@LastASNNumber" + i + ","
							+ "@LastReceivedQuantity" + i + ","
							+ "@MaterialAuthorizationDate" + i + ","
							+ "@MaterialAuthorizationQuantity" + i + ","
							+ "@PlanningHorizionEnd" + i + ","
							+ "@PlanningHorizionStart" + i + ","
							+ "@PositionNumber" + i + ","
							+ "@PreviousCallOffDate" + i + ","
							+ "@PreviousCallOffNumber" + i + ","
							+ "@ProductionAuthorizationDateTime" + i + ","
							+ "@ProductionAuthorizationQuantity" + i + ","
							+ "@StorageLocation" + i + ","
							+ "@SuppliersItemMaterialNumber" + i + ","
							+ "@UnloadingPoint" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("BuyersInternalProductGroupCode" + i, item.BuyersInternalProductGroupCode == null ? (object)DBNull.Value : item.BuyersInternalProductGroupCode);
						sqlCommand.Parameters.AddWithValue("CallOffDateTime" + i, item.CallOffDateTime == null ? (object)DBNull.Value : item.CallOffDateTime);
						sqlCommand.Parameters.AddWithValue("CallOffNumber" + i, item.CallOffNumber == null ? (object)DBNull.Value : item.CallOffNumber);
						sqlCommand.Parameters.AddWithValue("CumulativeReceivedQuantity" + i, item.CumulativeReceivedQuantity == null ? (object)DBNull.Value : item.CumulativeReceivedQuantity);
						sqlCommand.Parameters.AddWithValue("CumulativeScheduledQuantity" + i, item.CumulativeScheduledQuantity == null ? (object)DBNull.Value : item.CumulativeScheduledQuantity);
						sqlCommand.Parameters.AddWithValue("CustomersItemMaterialNumber" + i, item.CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.CustomersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("DrawingRevisionNumber" + i, item.DrawingRevisionNumber == null ? (object)DBNull.Value : item.DrawingRevisionNumber);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId);
						sqlCommand.Parameters.AddWithValue("HeaderPreviousVersion" + i, item.HeaderPreviousVersion == null ? (object)DBNull.Value : item.HeaderPreviousVersion);
						sqlCommand.Parameters.AddWithValue("HeaderVersion" + i, item.HeaderVersion == null ? (object)DBNull.Value : item.HeaderVersion);
						sqlCommand.Parameters.AddWithValue("LastASNDate" + i, item.LastASNDate == null ? (object)DBNull.Value : item.LastASNDate);
						sqlCommand.Parameters.AddWithValue("LastASNDeliveryDate" + i, item.LastASNDeliveryDate == null ? (object)DBNull.Value : item.LastASNDeliveryDate);
						sqlCommand.Parameters.AddWithValue("LastASNNumber" + i, item.LastASNNumber == null ? (object)DBNull.Value : item.LastASNNumber);
						sqlCommand.Parameters.AddWithValue("LastReceivedQuantity" + i, item.LastReceivedQuantity == null ? (object)DBNull.Value : item.LastReceivedQuantity);
						sqlCommand.Parameters.AddWithValue("MaterialAuthorizationDate" + i, item.MaterialAuthorizationDate == null ? (object)DBNull.Value : item.MaterialAuthorizationDate);
						sqlCommand.Parameters.AddWithValue("MaterialAuthorizationQuantity" + i, item.MaterialAuthorizationQuantity == null ? (object)DBNull.Value : item.MaterialAuthorizationQuantity);
						sqlCommand.Parameters.AddWithValue("PlanningHorizionEnd" + i, item.PlanningHorizionEnd == null ? (object)DBNull.Value : item.PlanningHorizionEnd);
						sqlCommand.Parameters.AddWithValue("PlanningHorizionStart" + i, item.PlanningHorizionStart == null ? (object)DBNull.Value : item.PlanningHorizionStart);
						sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
						sqlCommand.Parameters.AddWithValue("PreviousCallOffDate" + i, item.PreviousCallOffDate == null ? (object)DBNull.Value : item.PreviousCallOffDate);
						sqlCommand.Parameters.AddWithValue("PreviousCallOffNumber" + i, item.PreviousCallOffNumber == null ? (object)DBNull.Value : item.PreviousCallOffNumber);
						sqlCommand.Parameters.AddWithValue("ProductionAuthorizationDateTime" + i, item.ProductionAuthorizationDateTime == null ? (object)DBNull.Value : item.ProductionAuthorizationDateTime);
						sqlCommand.Parameters.AddWithValue("ProductionAuthorizationQuantity" + i, item.ProductionAuthorizationQuantity == null ? (object)DBNull.Value : item.ProductionAuthorizationQuantity);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("SuppliersItemMaterialNumber" + i, item.SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.SuppliersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
					}

					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("", sqlConnection))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_LineItem] SET [ArticleId]=@ArticleId, [BuyersInternalProductGroupCode]=@BuyersInternalProductGroupCode, [CallOffDateTime]=@CallOffDateTime, [CallOffNumber]=@CallOffNumber, [CumulativeReceivedQuantity]=@CumulativeReceivedQuantity, [CumulativeScheduledQuantity]=@CumulativeScheduledQuantity, [CustomersItemMaterialNumber]=@CustomersItemMaterialNumber, [DocumentNumber]=@DocumentNumber, [DrawingRevisionNumber]=@DrawingRevisionNumber, [HeaderId]=@HeaderId, [HeaderPreviousVersion]=@HeaderPreviousVersion, [HeaderVersion]=@HeaderVersion, [LastASNDate]=@LastASNDate, [LastASNDeliveryDate]=@LastASNDeliveryDate, [LastASNNumber]=@LastASNNumber, [LastReceivedQuantity]=@LastReceivedQuantity, [MaterialAuthorizationDate]=@MaterialAuthorizationDate, [MaterialAuthorizationQuantity]=@MaterialAuthorizationQuantity, [PlanningHorizionEnd]=@PlanningHorizionEnd, [PlanningHorizionStart]=@PlanningHorizionStart, [PositionNumber]=@PositionNumber, [PreviousCallOffDate]=@PreviousCallOffDate, [PreviousCallOffNumber]=@PreviousCallOffNumber, [ProductionAuthorizationDateTime]=@ProductionAuthorizationDateTime, [ProductionAuthorizationQuantity]=@ProductionAuthorizationQuantity, [StorageLocation]=@StorageLocation, [SuppliersItemMaterialNumber]=@SuppliersItemMaterialNumber, [UnloadingPoint]=@UnloadingPoint WHERE [Id]=@Id";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("BuyersInternalProductGroupCode", item.BuyersInternalProductGroupCode == null ? (object)DBNull.Value : item.BuyersInternalProductGroupCode);
				sqlCommand.Parameters.AddWithValue("CallOffDateTime", item.CallOffDateTime == null ? (object)DBNull.Value : item.CallOffDateTime);
				sqlCommand.Parameters.AddWithValue("CallOffNumber", item.CallOffNumber == null ? (object)DBNull.Value : item.CallOffNumber);
				sqlCommand.Parameters.AddWithValue("CumulativeReceivedQuantity", item.CumulativeReceivedQuantity == null ? (object)DBNull.Value : item.CumulativeReceivedQuantity);
				sqlCommand.Parameters.AddWithValue("CumulativeScheduledQuantity", item.CumulativeScheduledQuantity == null ? (object)DBNull.Value : item.CumulativeScheduledQuantity);
				sqlCommand.Parameters.AddWithValue("CustomersItemMaterialNumber", item.CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.CustomersItemMaterialNumber);
				sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
				sqlCommand.Parameters.AddWithValue("DrawingRevisionNumber", item.DrawingRevisionNumber == null ? (object)DBNull.Value : item.DrawingRevisionNumber);
				sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId);
				sqlCommand.Parameters.AddWithValue("HeaderPreviousVersion", item.HeaderPreviousVersion == null ? (object)DBNull.Value : item.HeaderPreviousVersion);
				sqlCommand.Parameters.AddWithValue("HeaderVersion", item.HeaderVersion == null ? (object)DBNull.Value : item.HeaderVersion);
				sqlCommand.Parameters.AddWithValue("LastASNDate", item.LastASNDate == null ? (object)DBNull.Value : item.LastASNDate);
				sqlCommand.Parameters.AddWithValue("LastASNDeliveryDate", item.LastASNDeliveryDate == null ? (object)DBNull.Value : item.LastASNDeliveryDate);
				sqlCommand.Parameters.AddWithValue("LastASNNumber", item.LastASNNumber == null ? (object)DBNull.Value : item.LastASNNumber);
				sqlCommand.Parameters.AddWithValue("LastReceivedQuantity", item.LastReceivedQuantity == null ? (object)DBNull.Value : item.LastReceivedQuantity);
				sqlCommand.Parameters.AddWithValue("MaterialAuthorizationDate", item.MaterialAuthorizationDate == null ? (object)DBNull.Value : item.MaterialAuthorizationDate);
				sqlCommand.Parameters.AddWithValue("MaterialAuthorizationQuantity", item.MaterialAuthorizationQuantity == null ? (object)DBNull.Value : item.MaterialAuthorizationQuantity);
				sqlCommand.Parameters.AddWithValue("PlanningHorizionEnd", item.PlanningHorizionEnd == null ? (object)DBNull.Value : item.PlanningHorizionEnd);
				sqlCommand.Parameters.AddWithValue("PlanningHorizionStart", item.PlanningHorizionStart == null ? (object)DBNull.Value : item.PlanningHorizionStart);
				sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
				sqlCommand.Parameters.AddWithValue("PreviousCallOffDate", item.PreviousCallOffDate == null ? (object)DBNull.Value : item.PreviousCallOffDate);
				sqlCommand.Parameters.AddWithValue("PreviousCallOffNumber", item.PreviousCallOffNumber == null ? (object)DBNull.Value : item.PreviousCallOffNumber);
				sqlCommand.Parameters.AddWithValue("ProductionAuthorizationDateTime", item.ProductionAuthorizationDateTime == null ? (object)DBNull.Value : item.ProductionAuthorizationDateTime);
				sqlCommand.Parameters.AddWithValue("ProductionAuthorizationQuantity", item.ProductionAuthorizationQuantity == null ? (object)DBNull.Value : item.ProductionAuthorizationQuantity);
				sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
				sqlCommand.Parameters.AddWithValue("SuppliersItemMaterialNumber", item.SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.SuppliersItemMaterialNumber);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; /* Nb params per query */
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [__EDI_DLF_LineItem] SET "

							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[BuyersInternalProductGroupCode]=@BuyersInternalProductGroupCode" + i + ","
							+ "[CallOffDateTime]=@CallOffDateTime" + i + ","
							+ "[CallOffNumber]=@CallOffNumber" + i + ","
							+ "[CumulativeReceivedQuantity]=@CumulativeReceivedQuantity" + i + ","
							+ "[CumulativeScheduledQuantity]=@CumulativeScheduledQuantity" + i + ","
							+ "[CustomersItemMaterialNumber]=@CustomersItemMaterialNumber" + i + ","
							+ "[DocumentNumber]=@DocumentNumber" + i + ","
							+ "[DrawingRevisionNumber]=@DrawingRevisionNumber" + i + ","
							+ "[HeaderId]=@HeaderId" + i + ","
							+ "[HeaderPreviousVersion]=@HeaderPreviousVersion" + i + ","
							+ "[HeaderVersion]=@HeaderVersion" + i + ","
							+ "[LastASNDate]=@LastASNDate" + i + ","
							+ "[LastASNDeliveryDate]=@LastASNDeliveryDate" + i + ","
							+ "[LastASNNumber]=@LastASNNumber" + i + ","
							+ "[LastReceivedQuantity]=@LastReceivedQuantity" + i + ","
							+ "[MaterialAuthorizationDate]=@MaterialAuthorizationDate" + i + ","
							+ "[MaterialAuthorizationQuantity]=@MaterialAuthorizationQuantity" + i + ","
							+ "[PlanningHorizionEnd]=@PlanningHorizionEnd" + i + ","
							+ "[PlanningHorizionStart]=@PlanningHorizionStart" + i + ","
							+ "[PositionNumber]=@PositionNumber" + i + ","
							+ "[PreviousCallOffDate]=@PreviousCallOffDate" + i + ","
							+ "[PreviousCallOffNumber]=@PreviousCallOffNumber" + i + ","
							+ "[ProductionAuthorizationDateTime]=@ProductionAuthorizationDateTime" + i + ","
							+ "[ProductionAuthorizationQuantity]=@ProductionAuthorizationQuantity" + i + ","
							+ "[StorageLocation]=@StorageLocation" + i + ","
							+ "[SuppliersItemMaterialNumber]=@SuppliersItemMaterialNumber" + i + ","
							+ "[UnloadingPoint]=@UnloadingPoint" + i + $" WHERE [Id]=@Id{i}"
							+ "; ";

						sqlCommand.Parameters.AddWithValue($"Id{i}", item.Id);

						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("BuyersInternalProductGroupCode" + i, item.BuyersInternalProductGroupCode == null ? (object)DBNull.Value : item.BuyersInternalProductGroupCode);
						sqlCommand.Parameters.AddWithValue("CallOffDateTime" + i, item.CallOffDateTime == null ? (object)DBNull.Value : item.CallOffDateTime);
						sqlCommand.Parameters.AddWithValue("CallOffNumber" + i, item.CallOffNumber == null ? (object)DBNull.Value : item.CallOffNumber);
						sqlCommand.Parameters.AddWithValue("CumulativeReceivedQuantity" + i, item.CumulativeReceivedQuantity == null ? (object)DBNull.Value : item.CumulativeReceivedQuantity);
						sqlCommand.Parameters.AddWithValue("CumulativeScheduledQuantity" + i, item.CumulativeScheduledQuantity == null ? (object)DBNull.Value : item.CumulativeScheduledQuantity);
						sqlCommand.Parameters.AddWithValue("CustomersItemMaterialNumber" + i, item.CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.CustomersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("DrawingRevisionNumber" + i, item.DrawingRevisionNumber == null ? (object)DBNull.Value : item.DrawingRevisionNumber);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId);
						sqlCommand.Parameters.AddWithValue("HeaderPreviousVersion" + i, item.HeaderPreviousVersion == null ? (object)DBNull.Value : item.HeaderPreviousVersion);
						sqlCommand.Parameters.AddWithValue("HeaderVersion" + i, item.HeaderVersion == null ? (object)DBNull.Value : item.HeaderVersion);
						sqlCommand.Parameters.AddWithValue("LastASNDate" + i, item.LastASNDate == null ? (object)DBNull.Value : item.LastASNDate);
						sqlCommand.Parameters.AddWithValue("LastASNDeliveryDate" + i, item.LastASNDeliveryDate == null ? (object)DBNull.Value : item.LastASNDeliveryDate);
						sqlCommand.Parameters.AddWithValue("LastASNNumber" + i, item.LastASNNumber == null ? (object)DBNull.Value : item.LastASNNumber);
						sqlCommand.Parameters.AddWithValue("LastReceivedQuantity" + i, item.LastReceivedQuantity == null ? (object)DBNull.Value : item.LastReceivedQuantity);
						sqlCommand.Parameters.AddWithValue("MaterialAuthorizationDate" + i, item.MaterialAuthorizationDate == null ? (object)DBNull.Value : item.MaterialAuthorizationDate);
						sqlCommand.Parameters.AddWithValue("MaterialAuthorizationQuantity" + i, item.MaterialAuthorizationQuantity == null ? (object)DBNull.Value : item.MaterialAuthorizationQuantity);
						sqlCommand.Parameters.AddWithValue("PlanningHorizionEnd" + i, item.PlanningHorizionEnd == null ? (object)DBNull.Value : item.PlanningHorizionEnd);
						sqlCommand.Parameters.AddWithValue("PlanningHorizionStart" + i, item.PlanningHorizionStart == null ? (object)DBNull.Value : item.PlanningHorizionStart);
						sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
						sqlCommand.Parameters.AddWithValue("PreviousCallOffDate" + i, item.PreviousCallOffDate == null ? (object)DBNull.Value : item.PreviousCallOffDate);
						sqlCommand.Parameters.AddWithValue("PreviousCallOffNumber" + i, item.PreviousCallOffNumber == null ? (object)DBNull.Value : item.PreviousCallOffNumber);
						sqlCommand.Parameters.AddWithValue("ProductionAuthorizationDateTime" + i, item.ProductionAuthorizationDateTime == null ? (object)DBNull.Value : item.ProductionAuthorizationDateTime);
						sqlCommand.Parameters.AddWithValue("ProductionAuthorizationQuantity" + i, item.ProductionAuthorizationQuantity == null ? (object)DBNull.Value : item.ProductionAuthorizationQuantity);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("SuppliersItemMaterialNumber" + i, item.SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.SuppliersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
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
				string query = "DELETE FROM [__EDI_DLF_LineItem] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__EDI_DLF_LineItem] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.LineItemEntity GetWithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_LineItem] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_LineItem]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> GetWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> getWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_LineItem] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
		}

		public static long InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO __EDI_DLF_LineItem ([ArticleId],[BuyersInternalProductGroupCode],[CallOffDateTime],[CallOffNumber],[CumulativeReceivedQuantity],[CumulativeScheduledQuantity],[CustomersItemMaterialNumber],[DocumentNumber],[DrawingRevisionNumber],[HeaderId],[HeaderPreviousVersion],[HeaderVersion],[LastASNDate],[LastASNDeliveryDate],[LastASNNumber],[LastReceivedQuantity],[MaterialAuthorizationDate],[MaterialAuthorizationQuantity],[PlanningHorizionEnd],[PlanningHorizionStart],[PositionNumber],[PreviousCallOffDate],[PreviousCallOffNumber],[ProductionAuthorizationDateTime],[ProductionAuthorizationQuantity],[StorageLocation],[SuppliersItemMaterialNumber],[UnloadingPoint]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@BuyersInternalProductGroupCode,@CallOffDateTime,@CallOffNumber,@CumulativeReceivedQuantity,@CumulativeScheduledQuantity,@CustomersItemMaterialNumber,@DocumentNumber,@DrawingRevisionNumber,@HeaderId,@HeaderPreviousVersion,@HeaderVersion,@LastASNDate,@LastASNDeliveryDate,@LastASNNumber,@LastReceivedQuantity,@MaterialAuthorizationDate,@MaterialAuthorizationQuantity,@PlanningHorizionEnd,@PlanningHorizionStart,@PositionNumber,@PreviousCallOffDate,@PreviousCallOffNumber,@ProductionAuthorizationDateTime,@ProductionAuthorizationQuantity,@StorageLocation,@SuppliersItemMaterialNumber,@UnloadingPoint); ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("BuyersInternalProductGroupCode", item.BuyersInternalProductGroupCode == null ? (object)DBNull.Value : item.BuyersInternalProductGroupCode);
				sqlCommand.Parameters.AddWithValue("CallOffDateTime", item.CallOffDateTime == null ? (object)DBNull.Value : item.CallOffDateTime);
				sqlCommand.Parameters.AddWithValue("CallOffNumber", item.CallOffNumber == null ? (object)DBNull.Value : item.CallOffNumber);
				sqlCommand.Parameters.AddWithValue("CumulativeReceivedQuantity", item.CumulativeReceivedQuantity == null ? (object)DBNull.Value : item.CumulativeReceivedQuantity);
				sqlCommand.Parameters.AddWithValue("CumulativeScheduledQuantity", item.CumulativeScheduledQuantity == null ? (object)DBNull.Value : item.CumulativeScheduledQuantity);
				sqlCommand.Parameters.AddWithValue("CustomersItemMaterialNumber", item.CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.CustomersItemMaterialNumber);
				sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
				sqlCommand.Parameters.AddWithValue("DrawingRevisionNumber", item.DrawingRevisionNumber == null ? (object)DBNull.Value : item.DrawingRevisionNumber);
				sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId);
				sqlCommand.Parameters.AddWithValue("HeaderPreviousVersion", item.HeaderPreviousVersion == null ? (object)DBNull.Value : item.HeaderPreviousVersion);
				sqlCommand.Parameters.AddWithValue("HeaderVersion", item.HeaderVersion == null ? (object)DBNull.Value : item.HeaderVersion);
				sqlCommand.Parameters.AddWithValue("LastASNDate", item.LastASNDate == null ? (object)DBNull.Value : item.LastASNDate);
				sqlCommand.Parameters.AddWithValue("LastASNDeliveryDate", item.LastASNDeliveryDate == null ? (object)DBNull.Value : item.LastASNDeliveryDate);
				sqlCommand.Parameters.AddWithValue("LastASNNumber", item.LastASNNumber == null ? (object)DBNull.Value : item.LastASNNumber);
				sqlCommand.Parameters.AddWithValue("LastReceivedQuantity", item.LastReceivedQuantity == null ? (object)DBNull.Value : item.LastReceivedQuantity);
				sqlCommand.Parameters.AddWithValue("MaterialAuthorizationDate", item.MaterialAuthorizationDate == null ? (object)DBNull.Value : item.MaterialAuthorizationDate);
				sqlCommand.Parameters.AddWithValue("MaterialAuthorizationQuantity", item.MaterialAuthorizationQuantity == null ? (object)DBNull.Value : item.MaterialAuthorizationQuantity);
				sqlCommand.Parameters.AddWithValue("PlanningHorizionEnd", item.PlanningHorizionEnd == null ? (object)DBNull.Value : item.PlanningHorizionEnd);
				sqlCommand.Parameters.AddWithValue("PlanningHorizionStart", item.PlanningHorizionStart == null ? (object)DBNull.Value : item.PlanningHorizionStart);
				sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
				sqlCommand.Parameters.AddWithValue("PreviousCallOffDate", item.PreviousCallOffDate == null ? (object)DBNull.Value : item.PreviousCallOffDate);
				sqlCommand.Parameters.AddWithValue("PreviousCallOffNumber", item.PreviousCallOffNumber == null ? (object)DBNull.Value : item.PreviousCallOffNumber);
				sqlCommand.Parameters.AddWithValue("ProductionAuthorizationDateTime", item.ProductionAuthorizationDateTime == null ? (object)DBNull.Value : item.ProductionAuthorizationDateTime);
				sqlCommand.Parameters.AddWithValue("ProductionAuthorizationQuantity", item.ProductionAuthorizationQuantity == null ? (object)DBNull.Value : item.ProductionAuthorizationQuantity);
				sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
				sqlCommand.Parameters.AddWithValue("SuppliersItemMaterialNumber", item.SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.SuppliersItemMaterialNumber);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
				var result = sqlCommand.ExecuteScalar();
				return result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
			}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; /* Nb params per query */
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "INSERT INTO [__EDI_DLF_LineItem] ([ArticleId],[BuyersInternalProductGroupCode],[CallOffDateTime],[CallOffNumber],[CumulativeReceivedQuantity],[CumulativeScheduledQuantity],[CustomersItemMaterialNumber],[DocumentNumber],[DrawingRevisionNumber],[HeaderId],[HeaderPreviousVersion],[HeaderVersion],[LastASNDate],[LastASNDeliveryDate],[LastASNNumber],[LastReceivedQuantity],[MaterialAuthorizationDate],[MaterialAuthorizationQuantity],[PlanningHorizionEnd],[PlanningHorizionStart],[PositionNumber],[PreviousCallOffDate],[PreviousCallOffNumber],[ProductionAuthorizationDateTime],[ProductionAuthorizationQuantity],[StorageLocation],[SuppliersItemMaterialNumber],[UnloadingPoint]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@BuyersInternalProductGroupCode" + i + ","
						+ "@CallOffDateTime" + i + ","
						+ "@CallOffNumber" + i + ","
						+ "@CumulativeReceivedQuantity" + i + ","
						+ "@CumulativeScheduledQuantity" + i + ","
						+ "@CustomersItemMaterialNumber" + i + ","
						+ "@DocumentNumber" + i + ","
						+ "@DrawingRevisionNumber" + i + ","
						+ "@HeaderId" + i + ","
						+ "@HeaderPreviousVersion" + i + ","
						+ "@HeaderVersion" + i + ","
						+ "@LastASNDate" + i + ","
						+ "@LastASNDeliveryDate" + i + ","
						+ "@LastASNNumber" + i + ","
						+ "@LastReceivedQuantity" + i + ","
						+ "@MaterialAuthorizationDate" + i + ","
						+ "@MaterialAuthorizationQuantity" + i + ","
						+ "@PlanningHorizionEnd" + i + ","
						+ "@PlanningHorizionStart" + i + ","
						+ "@PositionNumber" + i + ","
						+ "@PreviousCallOffDate" + i + ","
						+ "@PreviousCallOffNumber" + i + ","
						+ "@ProductionAuthorizationDateTime" + i + ","
						+ "@ProductionAuthorizationQuantity" + i + ","
						+ "@StorageLocation" + i + ","
						+ "@SuppliersItemMaterialNumber" + i + ","
						+ "@UnloadingPoint" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("BuyersInternalProductGroupCode" + i, item.BuyersInternalProductGroupCode == null ? (object)DBNull.Value : item.BuyersInternalProductGroupCode);
						sqlCommand.Parameters.AddWithValue("CallOffDateTime" + i, item.CallOffDateTime == null ? (object)DBNull.Value : item.CallOffDateTime);
						sqlCommand.Parameters.AddWithValue("CallOffNumber" + i, item.CallOffNumber == null ? (object)DBNull.Value : item.CallOffNumber);
						sqlCommand.Parameters.AddWithValue("CumulativeReceivedQuantity" + i, item.CumulativeReceivedQuantity == null ? (object)DBNull.Value : item.CumulativeReceivedQuantity);
						sqlCommand.Parameters.AddWithValue("CumulativeScheduledQuantity" + i, item.CumulativeScheduledQuantity == null ? (object)DBNull.Value : item.CumulativeScheduledQuantity);
						sqlCommand.Parameters.AddWithValue("CustomersItemMaterialNumber" + i, item.CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.CustomersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("DrawingRevisionNumber" + i, item.DrawingRevisionNumber == null ? (object)DBNull.Value : item.DrawingRevisionNumber);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId);
						sqlCommand.Parameters.AddWithValue("HeaderPreviousVersion" + i, item.HeaderPreviousVersion == null ? (object)DBNull.Value : item.HeaderPreviousVersion);
						sqlCommand.Parameters.AddWithValue("HeaderVersion" + i, item.HeaderVersion == null ? (object)DBNull.Value : item.HeaderVersion);
						sqlCommand.Parameters.AddWithValue("LastASNDate" + i, item.LastASNDate == null ? (object)DBNull.Value : item.LastASNDate);
						sqlCommand.Parameters.AddWithValue("LastASNDeliveryDate" + i, item.LastASNDeliveryDate == null ? (object)DBNull.Value : item.LastASNDeliveryDate);
						sqlCommand.Parameters.AddWithValue("LastASNNumber" + i, item.LastASNNumber == null ? (object)DBNull.Value : item.LastASNNumber);
						sqlCommand.Parameters.AddWithValue("LastReceivedQuantity" + i, item.LastReceivedQuantity == null ? (object)DBNull.Value : item.LastReceivedQuantity);
						sqlCommand.Parameters.AddWithValue("MaterialAuthorizationDate" + i, item.MaterialAuthorizationDate == null ? (object)DBNull.Value : item.MaterialAuthorizationDate);
						sqlCommand.Parameters.AddWithValue("MaterialAuthorizationQuantity" + i, item.MaterialAuthorizationQuantity == null ? (object)DBNull.Value : item.MaterialAuthorizationQuantity);
						sqlCommand.Parameters.AddWithValue("PlanningHorizionEnd" + i, item.PlanningHorizionEnd == null ? (object)DBNull.Value : item.PlanningHorizionEnd);
						sqlCommand.Parameters.AddWithValue("PlanningHorizionStart" + i, item.PlanningHorizionStart == null ? (object)DBNull.Value : item.PlanningHorizionStart);
						sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
						sqlCommand.Parameters.AddWithValue("PreviousCallOffDate" + i, item.PreviousCallOffDate == null ? (object)DBNull.Value : item.PreviousCallOffDate);
						sqlCommand.Parameters.AddWithValue("PreviousCallOffNumber" + i, item.PreviousCallOffNumber == null ? (object)DBNull.Value : item.PreviousCallOffNumber);
						sqlCommand.Parameters.AddWithValue("ProductionAuthorizationDateTime" + i, item.ProductionAuthorizationDateTime == null ? (object)DBNull.Value : item.ProductionAuthorizationDateTime);
						sqlCommand.Parameters.AddWithValue("ProductionAuthorizationQuantity" + i, item.ProductionAuthorizationQuantity == null ? (object)DBNull.Value : item.ProductionAuthorizationQuantity);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("SuppliersItemMaterialNumber" + i, item.SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.SuppliersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
					}

					sqlCommand.CommandText = query;

					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [__EDI_DLF_LineItem] SET [ArticleId]=@ArticleId, [BuyersInternalProductGroupCode]=@BuyersInternalProductGroupCode, [CallOffDateTime]=@CallOffDateTime, [CallOffNumber]=@CallOffNumber, [CumulativeReceivedQuantity]=@CumulativeReceivedQuantity, [CumulativeScheduledQuantity]=@CumulativeScheduledQuantity, [CustomersItemMaterialNumber]=@CustomersItemMaterialNumber, [DocumentNumber]=@DocumentNumber, [DrawingRevisionNumber]=@DrawingRevisionNumber, [HeaderId]=@HeaderId, [HeaderPreviousVersion]=@HeaderPreviousVersion, [HeaderVersion]=@HeaderVersion, [LastASNDate]=@LastASNDate, [LastASNDeliveryDate]=@LastASNDeliveryDate, [LastASNNumber]=@LastASNNumber, [LastReceivedQuantity]=@LastReceivedQuantity, [MaterialAuthorizationDate]=@MaterialAuthorizationDate, [MaterialAuthorizationQuantity]=@MaterialAuthorizationQuantity, [PlanningHorizionEnd]=@PlanningHorizionEnd, [PlanningHorizionStart]=@PlanningHorizionStart, [PositionNumber]=@PositionNumber, [PreviousCallOffDate]=@PreviousCallOffDate, [PreviousCallOffNumber]=@PreviousCallOffNumber, [ProductionAuthorizationDateTime]=@ProductionAuthorizationDateTime, [ProductionAuthorizationQuantity]=@ProductionAuthorizationQuantity, [StorageLocation]=@StorageLocation, [SuppliersItemMaterialNumber]=@SuppliersItemMaterialNumber, [UnloadingPoint]=@UnloadingPoint WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("BuyersInternalProductGroupCode", item.BuyersInternalProductGroupCode == null ? (object)DBNull.Value : item.BuyersInternalProductGroupCode);
				sqlCommand.Parameters.AddWithValue("CallOffDateTime", item.CallOffDateTime == null ? (object)DBNull.Value : item.CallOffDateTime);
				sqlCommand.Parameters.AddWithValue("CallOffNumber", item.CallOffNumber == null ? (object)DBNull.Value : item.CallOffNumber);
				sqlCommand.Parameters.AddWithValue("CumulativeReceivedQuantity", item.CumulativeReceivedQuantity == null ? (object)DBNull.Value : item.CumulativeReceivedQuantity);
				sqlCommand.Parameters.AddWithValue("CumulativeScheduledQuantity", item.CumulativeScheduledQuantity == null ? (object)DBNull.Value : item.CumulativeScheduledQuantity);
				sqlCommand.Parameters.AddWithValue("CustomersItemMaterialNumber", item.CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.CustomersItemMaterialNumber);
				sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
				sqlCommand.Parameters.AddWithValue("DrawingRevisionNumber", item.DrawingRevisionNumber == null ? (object)DBNull.Value : item.DrawingRevisionNumber);
				sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId);
				sqlCommand.Parameters.AddWithValue("HeaderPreviousVersion", item.HeaderPreviousVersion == null ? (object)DBNull.Value : item.HeaderPreviousVersion);
				sqlCommand.Parameters.AddWithValue("HeaderVersion", item.HeaderVersion == null ? (object)DBNull.Value : item.HeaderVersion);
				sqlCommand.Parameters.AddWithValue("LastASNDate", item.LastASNDate == null ? (object)DBNull.Value : item.LastASNDate);
				sqlCommand.Parameters.AddWithValue("LastASNDeliveryDate", item.LastASNDeliveryDate == null ? (object)DBNull.Value : item.LastASNDeliveryDate);
				sqlCommand.Parameters.AddWithValue("LastASNNumber", item.LastASNNumber == null ? (object)DBNull.Value : item.LastASNNumber);
				sqlCommand.Parameters.AddWithValue("LastReceivedQuantity", item.LastReceivedQuantity == null ? (object)DBNull.Value : item.LastReceivedQuantity);
				sqlCommand.Parameters.AddWithValue("MaterialAuthorizationDate", item.MaterialAuthorizationDate == null ? (object)DBNull.Value : item.MaterialAuthorizationDate);
				sqlCommand.Parameters.AddWithValue("MaterialAuthorizationQuantity", item.MaterialAuthorizationQuantity == null ? (object)DBNull.Value : item.MaterialAuthorizationQuantity);
				sqlCommand.Parameters.AddWithValue("PlanningHorizionEnd", item.PlanningHorizionEnd == null ? (object)DBNull.Value : item.PlanningHorizionEnd);
				sqlCommand.Parameters.AddWithValue("PlanningHorizionStart", item.PlanningHorizionStart == null ? (object)DBNull.Value : item.PlanningHorizionStart);
				sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
				sqlCommand.Parameters.AddWithValue("PreviousCallOffDate", item.PreviousCallOffDate == null ? (object)DBNull.Value : item.PreviousCallOffDate);
				sqlCommand.Parameters.AddWithValue("PreviousCallOffNumber", item.PreviousCallOffNumber == null ? (object)DBNull.Value : item.PreviousCallOffNumber);
				sqlCommand.Parameters.AddWithValue("ProductionAuthorizationDateTime", item.ProductionAuthorizationDateTime == null ? (object)DBNull.Value : item.ProductionAuthorizationDateTime);
				sqlCommand.Parameters.AddWithValue("ProductionAuthorizationQuantity", item.ProductionAuthorizationQuantity == null ? (object)DBNull.Value : item.ProductionAuthorizationQuantity);
				sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
				sqlCommand.Parameters.AddWithValue("SuppliersItemMaterialNumber", item.SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.SuppliersItemMaterialNumber);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; /* Nb params per query */
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [__EDI_DLF_LineItem] SET "

						+ "[ArticleId]=@ArticleId" + i + ","
						+ "[BuyersInternalProductGroupCode]=@BuyersInternalProductGroupCode" + i + ","
						+ "[CallOffDateTime]=@CallOffDateTime" + i + ","
						+ "[CallOffNumber]=@CallOffNumber" + i + ","
						+ "[CumulativeReceivedQuantity]=@CumulativeReceivedQuantity" + i + ","
						+ "[CumulativeScheduledQuantity]=@CumulativeScheduledQuantity" + i + ","
						+ "[CustomersItemMaterialNumber]=@CustomersItemMaterialNumber" + i + ","
						+ "[DocumentNumber]=@DocumentNumber" + i + ","
						+ "[DrawingRevisionNumber]=@DrawingRevisionNumber" + i + ","
						+ "[HeaderId]=@HeaderId" + i + ","
						+ "[HeaderPreviousVersion]=@HeaderPreviousVersion" + i + ","
						+ "[HeaderVersion]=@HeaderVersion" + i + ","
						+ "[LastASNDate]=@LastASNDate" + i + ","
						+ "[LastASNDeliveryDate]=@LastASNDeliveryDate" + i + ","
						+ "[LastASNNumber]=@LastASNNumber" + i + ","
						+ "[LastReceivedQuantity]=@LastReceivedQuantity" + i + ","
						+ "[MaterialAuthorizationDate]=@MaterialAuthorizationDate" + i + ","
						+ "[MaterialAuthorizationQuantity]=@MaterialAuthorizationQuantity" + i + ","
						+ "[PlanningHorizionEnd]=@PlanningHorizionEnd" + i + ","
						+ "[PlanningHorizionStart]=@PlanningHorizionStart" + i + ","
						+ "[PositionNumber]=@PositionNumber" + i + ","
						+ "[PreviousCallOffDate]=@PreviousCallOffDate" + i + ","
						+ "[PreviousCallOffNumber]=@PreviousCallOffNumber" + i + ","
						+ "[ProductionAuthorizationDateTime]=@ProductionAuthorizationDateTime" + i + ","
						+ "[ProductionAuthorizationQuantity]=@ProductionAuthorizationQuantity" + i + ","
						+ "[StorageLocation]=@StorageLocation" + i + ","
						+ "[SuppliersItemMaterialNumber]=@SuppliersItemMaterialNumber" + i + ","
						+ "[UnloadingPoint]=@UnloadingPoint" + i + " WHERE [Id]=@Id" + i
							+ ";";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);

						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("BuyersInternalProductGroupCode" + i, item.BuyersInternalProductGroupCode == null ? (object)DBNull.Value : item.BuyersInternalProductGroupCode);
						sqlCommand.Parameters.AddWithValue("CallOffDateTime" + i, item.CallOffDateTime == null ? (object)DBNull.Value : item.CallOffDateTime);
						sqlCommand.Parameters.AddWithValue("CallOffNumber" + i, item.CallOffNumber == null ? (object)DBNull.Value : item.CallOffNumber);
						sqlCommand.Parameters.AddWithValue("CumulativeReceivedQuantity" + i, item.CumulativeReceivedQuantity == null ? (object)DBNull.Value : item.CumulativeReceivedQuantity);
						sqlCommand.Parameters.AddWithValue("CumulativeScheduledQuantity" + i, item.CumulativeScheduledQuantity == null ? (object)DBNull.Value : item.CumulativeScheduledQuantity);
						sqlCommand.Parameters.AddWithValue("CustomersItemMaterialNumber" + i, item.CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.CustomersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("DrawingRevisionNumber" + i, item.DrawingRevisionNumber == null ? (object)DBNull.Value : item.DrawingRevisionNumber);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId);
						sqlCommand.Parameters.AddWithValue("HeaderPreviousVersion" + i, item.HeaderPreviousVersion == null ? (object)DBNull.Value : item.HeaderPreviousVersion);
						sqlCommand.Parameters.AddWithValue("HeaderVersion" + i, item.HeaderVersion == null ? (object)DBNull.Value : item.HeaderVersion);
						sqlCommand.Parameters.AddWithValue("LastASNDate" + i, item.LastASNDate == null ? (object)DBNull.Value : item.LastASNDate);
						sqlCommand.Parameters.AddWithValue("LastASNDeliveryDate" + i, item.LastASNDeliveryDate == null ? (object)DBNull.Value : item.LastASNDeliveryDate);
						sqlCommand.Parameters.AddWithValue("LastASNNumber" + i, item.LastASNNumber == null ? (object)DBNull.Value : item.LastASNNumber);
						sqlCommand.Parameters.AddWithValue("LastReceivedQuantity" + i, item.LastReceivedQuantity == null ? (object)DBNull.Value : item.LastReceivedQuantity);
						sqlCommand.Parameters.AddWithValue("MaterialAuthorizationDate" + i, item.MaterialAuthorizationDate == null ? (object)DBNull.Value : item.MaterialAuthorizationDate);
						sqlCommand.Parameters.AddWithValue("MaterialAuthorizationQuantity" + i, item.MaterialAuthorizationQuantity == null ? (object)DBNull.Value : item.MaterialAuthorizationQuantity);
						sqlCommand.Parameters.AddWithValue("PlanningHorizionEnd" + i, item.PlanningHorizionEnd == null ? (object)DBNull.Value : item.PlanningHorizionEnd);
						sqlCommand.Parameters.AddWithValue("PlanningHorizionStart" + i, item.PlanningHorizionStart == null ? (object)DBNull.Value : item.PlanningHorizionStart);
						sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
						sqlCommand.Parameters.AddWithValue("PreviousCallOffDate" + i, item.PreviousCallOffDate == null ? (object)DBNull.Value : item.PreviousCallOffDate);
						sqlCommand.Parameters.AddWithValue("PreviousCallOffNumber" + i, item.PreviousCallOffNumber == null ? (object)DBNull.Value : item.PreviousCallOffNumber);
						sqlCommand.Parameters.AddWithValue("ProductionAuthorizationDateTime" + i, item.ProductionAuthorizationDateTime == null ? (object)DBNull.Value : item.ProductionAuthorizationDateTime);
						sqlCommand.Parameters.AddWithValue("ProductionAuthorizationQuantity" + i, item.ProductionAuthorizationQuantity == null ? (object)DBNull.Value : item.ProductionAuthorizationQuantity);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("SuppliersItemMaterialNumber" + i, item.SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.SuppliersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
					}

					sqlCommand.CommandText = query;
					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}

		public static int DeleteWithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__EDI_DLF_LineItem] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__EDI_DLF_LineItem] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int GetDocumentLastVersionNumber(string documentNumber)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return 0;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT MAX([CallOffNumber]) FROM [__EDI_DLF_LineItem] WHERE [DocumentNumber]=@documentNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var _r) ? _r : 0;
			}
		}

		public static Infrastructure.Data.Entities.Tables.CTS.LineItemEntity GetByDocumentLastVersionNumber(string documentNumber, long? headerId)
		{
			if(string.IsNullOrWhiteSpace(documentNumber) || headerId <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_LineItem] WHERE {(headerId.HasValue ? "[HeaderId]=@headerId AND " : "")}[DocumentNumber]=@documentNumber AND [HeaderVersion]=(SELECT MAX([HeaderVersion]) FROM [__EDI_DLF_LineItem] WHERE [DocumentNumber]=@documentNumber)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber);
				if(headerId.HasValue)
				{
					sqlCommand.Parameters.AddWithValue("headerId", headerId);
				}

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Infrastructure.Data.Entities.Tables.CTS.LineItemEntity GetPrevVersion(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT H1.* FROM[__EDI_DLF_LineItem] H1
                    JOIN [__EDI_DLF_LineItem] H2 ON H2.DocumentNumber=H1.DocumentNumber AND H2.PositionNumber=H1.PositionNumber
                    JOIN (SELECT MAX(T1.HeaderVersion) HeaderVersion FROM [__EDI_DLF_LineItem] T1
                    JOIN [__EDI_DLF_LineItem] T2 ON T1.DocumentNumber=T2.DocumentNumber AND T1.PositionNumber=T2.PositionNumber AND T1.CallOffNumber <T2.CallOffNumber
                    WHERE T2.Id=@id  GROUP BY T1.DocumentNumber, T1.PositionNumber) AS H3 ON H3.HeaderVersion=H1.HeaderVersion WHERE H2.Id=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.LineItemEntity GetNextVersion(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT H1.* FROM[__EDI_DLF_LineItem] H1 "
					+ "JOIN [__EDI_DLF_LineItem] H2 ON H2.DocumentNumber=H1.DocumentNumber AND H2.PositionNumber=H1.PositionNumber "
					+ "JOIN (SELECT MIN(T1.HeaderVersion) HeaderVersion FROM [__EDI_DLF_LineItem] T1 "
					+ "JOIN [__EDI_DLF_LineItem] T2 ON T1.DocumentNumber=T2.DocumentNumber AND T1.PositionNumber=T2.PositionNumber AND T1.HeaderVersion>T2.HeaderVersion "
					+ "WHERE T2.Id=@id  GROUP BY T1.DocumentNumber, T1.PositionNumber) AS H3 ON H3.HeaderVersion=H1.HeaderVersion WHERE H2.Id=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<KeyValuePair<string, int>> GetPositionsCount(List<string> documentNumbers)
		{
			if(documentNumbers == null || documentNumbers.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DocumentNumber, COUNT(*) as nb FROM [__EDI_DLF_LineItem] GROUP BY DocumentNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<string, int>(x[0].ToString(), int.TryParse(x[1].ToString(), out var _r) ? _r : 0)).ToList();
			}
			else
			{
				return new List<KeyValuePair<string, int>>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> GetByHeaderId(int documentId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_LineItem] WHERE [HeaderId]=@documentId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("documentId", documentId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> GetByHeaderId(int documentId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__EDI_DLF_LineItem] WHERE [HeaderId]=@documentId";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("documentId", documentId);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> GetByHeaderId(List<long> documentIds)
		{
			if(documentIds == null || documentIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_LineItem] WHERE [HeaderId] IN ({string.Join(",", documentIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> GetByHeaderId(List<long> documentIds, SqlConnection connection, SqlTransaction transaction)
		{
			if(documentIds == null || documentIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [__EDI_DLF_LineItem] WHERE [HeaderId] IN ({string.Join(",", documentIds)})";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity> GetNextOrPreviousVersion(string documentId, int positionId, int version, int nextOrPrevious)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [__EDI_DLF_LineItem] WHERE [DocumentNumber]=@documentId 
                              AND [PositionNumber]=@positionId AND [HeaderVersion]{(nextOrPrevious == 1 ? ">" : "<")}@version
                              ORDER BY [HeaderVersion] DESC";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("documentId", documentId);
				sqlCommand.Parameters.AddWithValue("positionId", positionId);
				sqlCommand.Parameters.AddWithValue("version", version);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemEntity>();
			}
		}
		public static int ToggleStatus(List<long> headerIds, string documentNumber, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $"UPDATE [__EDI_DLF_LineItem] SET [DocumentNumber]=@documentNumber WHERE [HeaderId] IN ({(headerIds?.Count > 0 ? string.Join(",", headerIds) : "")})";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber);

			return sqlCommand.ExecuteNonQuery();
		}
		#endregion Custom Methods

	}
}
