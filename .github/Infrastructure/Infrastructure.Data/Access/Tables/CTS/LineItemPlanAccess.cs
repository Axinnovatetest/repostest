using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class LineItemPlanAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity Get(long id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_LineItemPlan] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_LineItemPlan]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> Get(List<long> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> get(List<long> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_LineItemPlan] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
		}

		public static long Insert(Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity item)
		{
			long response = long.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_DLF_LineItemPlan] ([LineItemId],[OrderDate],[OrderId],[OrderItemId],[OrderUserId],[PlanningQuantityChange],[PlanningQuantityCumulativeQuantity],[PlanningQuantityDeliveryPlanStatusIdentifier],[PlanningQuantityFrequencyIdentifier],[PlanningQuantityQuantity],[PlanningQuantityRequestedShipmentDate],[PlanningQuantityUnitOfMeasure],[PlanningQuantityWeeklyPeriodEndDate],[PositionNumber],[ProductionConfirmationDate],[ProductionConfirmationId],[ProductionConfirmationItemId],[ProductionConfirmationUserId],[ProductionDate],[ProductionId],[ProductionOrderId],[ProductionOrderUserId],[ProductionUserId]) OUTPUT INSERTED.[Id] VALUES (@LineItemId,@OrderDate,@OrderId,@OrderItemId,@OrderUserId,@PlanningQuantityChange,@PlanningQuantityCumulativeQuantity,@PlanningQuantityDeliveryPlanStatusIdentifier,@PlanningQuantityFrequencyIdentifier,@PlanningQuantityQuantity,@PlanningQuantityRequestedShipmentDate,@PlanningQuantityUnitOfMeasure,@PlanningQuantityWeeklyPeriodEndDate,@PositionNumber,@ProductionConfirmationDate,@ProductionConfirmationId,@ProductionConfirmationItemId,@ProductionConfirmationUserId,@ProductionDate,@ProductionId,@ProductionOrderId,@ProductionOrderUserId,@ProductionUserId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("LineItemId", item.LineItemId);
					sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderItemId", item.OrderItemId == null ? (object)DBNull.Value : item.OrderItemId);
					sqlCommand.Parameters.AddWithValue("OrderUserId", item.OrderUserId == null ? (object)DBNull.Value : item.OrderUserId);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityChange", item.PlanningQuantityChange == null ? (object)DBNull.Value : item.PlanningQuantityChange);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityCumulativeQuantity", item.PlanningQuantityCumulativeQuantity == null ? (object)DBNull.Value : item.PlanningQuantityCumulativeQuantity);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityDeliveryPlanStatusIdentifier", item.PlanningQuantityDeliveryPlanStatusIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityDeliveryPlanStatusIdentifier);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityFrequencyIdentifier", item.PlanningQuantityFrequencyIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityFrequencyIdentifier);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityQuantity", item.PlanningQuantityQuantity == null ? (object)DBNull.Value : item.PlanningQuantityQuantity);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityRequestedShipmentDate", item.PlanningQuantityRequestedShipmentDate == null ? (object)DBNull.Value : item.PlanningQuantityRequestedShipmentDate);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityUnitOfMeasure", item.PlanningQuantityUnitOfMeasure == null ? (object)DBNull.Value : item.PlanningQuantityUnitOfMeasure);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityWeeklyPeriodEndDate", item.PlanningQuantityWeeklyPeriodEndDate == null ? (object)DBNull.Value : item.PlanningQuantityWeeklyPeriodEndDate);
					sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationDate", item.ProductionConfirmationDate == null ? (object)DBNull.Value : item.ProductionConfirmationDate);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationId", item.ProductionConfirmationId == null ? (object)DBNull.Value : item.ProductionConfirmationId);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationItemId", item.ProductionConfirmationItemId == null ? (object)DBNull.Value : item.ProductionConfirmationItemId);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationUserId", item.ProductionConfirmationUserId == null ? (object)DBNull.Value : item.ProductionConfirmationUserId);
					sqlCommand.Parameters.AddWithValue("ProductionDate", item.ProductionDate == null ? (object)DBNull.Value : item.ProductionDate);
					sqlCommand.Parameters.AddWithValue("ProductionId", item.ProductionId == null ? (object)DBNull.Value : item.ProductionId);
					sqlCommand.Parameters.AddWithValue("ProductionOrderId", item.ProductionOrderId == null ? (object)DBNull.Value : item.ProductionOrderId);
					sqlCommand.Parameters.AddWithValue("ProductionOrderUserId", item.ProductionOrderUserId == null ? (object)DBNull.Value : item.ProductionOrderUserId);
					sqlCommand.Parameters.AddWithValue("ProductionUserId", item.ProductionUserId == null ? (object)DBNull.Value : item.ProductionUserId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> items)
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
						query += " INSERT INTO [__EDI_DLF_LineItemPlan] ([LineItemId],[OrderDate],[OrderId],[OrderItemId],[OrderUserId],[PlanningQuantityChange],[PlanningQuantityCumulativeQuantity],[PlanningQuantityDeliveryPlanStatusIdentifier],[PlanningQuantityFrequencyIdentifier],[PlanningQuantityQuantity],[PlanningQuantityRequestedShipmentDate],[PlanningQuantityUnitOfMeasure],[PlanningQuantityWeeklyPeriodEndDate],[PositionNumber],[ProductionConfirmationDate],[ProductionConfirmationId],[ProductionConfirmationItemId],[ProductionConfirmationUserId],[ProductionDate],[ProductionId],[ProductionOrderId],[ProductionOrderUserId],[ProductionUserId]) VALUES ( "

							+ "@LineItemId" + i + ","
							+ "@OrderDate" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderItemId" + i + ","
							+ "@OrderUserId" + i + ","
							+ "@PlanningQuantityChange" + i + ","
							+ "@PlanningQuantityCumulativeQuantity" + i + ","
							+ "@PlanningQuantityDeliveryPlanStatusIdentifier" + i + ","
							+ "@PlanningQuantityFrequencyIdentifier" + i + ","
							+ "@PlanningQuantityQuantity" + i + ","
							+ "@PlanningQuantityRequestedShipmentDate" + i + ","
							+ "@PlanningQuantityUnitOfMeasure" + i + ","
							+ "@PlanningQuantityWeeklyPeriodEndDate" + i + ","
							+ "@PositionNumber" + i + ","
							+ "@ProductionConfirmationDate" + i + ","
							+ "@ProductionConfirmationId" + i + ","
							+ "@ProductionConfirmationItemId" + i + ","
							+ "@ProductionConfirmationUserId" + i + ","
							+ "@ProductionDate" + i + ","
							+ "@ProductionId" + i + ","
							+ "@ProductionOrderId" + i + ","
							+ "@ProductionOrderUserId" + i + ","
							+ "@ProductionUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("LineItemId" + i, item.LineItemId);
						sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderItemId" + i, item.OrderItemId == null ? (object)DBNull.Value : item.OrderItemId);
						sqlCommand.Parameters.AddWithValue("OrderUserId" + i, item.OrderUserId == null ? (object)DBNull.Value : item.OrderUserId);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityChange" + i, item.PlanningQuantityChange == null ? (object)DBNull.Value : item.PlanningQuantityChange);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityCumulativeQuantity" + i, item.PlanningQuantityCumulativeQuantity == null ? (object)DBNull.Value : item.PlanningQuantityCumulativeQuantity);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityDeliveryPlanStatusIdentifier" + i, item.PlanningQuantityDeliveryPlanStatusIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityDeliveryPlanStatusIdentifier);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityFrequencyIdentifier" + i, item.PlanningQuantityFrequencyIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityFrequencyIdentifier);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityQuantity" + i, item.PlanningQuantityQuantity == null ? (object)DBNull.Value : item.PlanningQuantityQuantity);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityRequestedShipmentDate" + i, item.PlanningQuantityRequestedShipmentDate == null ? (object)DBNull.Value : item.PlanningQuantityRequestedShipmentDate);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityUnitOfMeasure" + i, item.PlanningQuantityUnitOfMeasure == null ? (object)DBNull.Value : item.PlanningQuantityUnitOfMeasure);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityWeeklyPeriodEndDate" + i, item.PlanningQuantityWeeklyPeriodEndDate == null ? (object)DBNull.Value : item.PlanningQuantityWeeklyPeriodEndDate);
						sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
						sqlCommand.Parameters.AddWithValue("ProductionConfirmationDate" + i, item.ProductionConfirmationDate == null ? (object)DBNull.Value : item.ProductionConfirmationDate);
						sqlCommand.Parameters.AddWithValue("ProductionConfirmationId" + i, item.ProductionConfirmationId == null ? (object)DBNull.Value : item.ProductionConfirmationId);
						sqlCommand.Parameters.AddWithValue("ProductionConfirmationItemId" + i, item.ProductionConfirmationItemId == null ? (object)DBNull.Value : item.ProductionConfirmationItemId);
						sqlCommand.Parameters.AddWithValue("ProductionConfirmationUserId" + i, item.ProductionConfirmationUserId == null ? (object)DBNull.Value : item.ProductionConfirmationUserId);
						sqlCommand.Parameters.AddWithValue("ProductionDate" + i, item.ProductionDate == null ? (object)DBNull.Value : item.ProductionDate);
						sqlCommand.Parameters.AddWithValue("ProductionId" + i, item.ProductionId == null ? (object)DBNull.Value : item.ProductionId);
						sqlCommand.Parameters.AddWithValue("ProductionOrderId" + i, item.ProductionOrderId == null ? (object)DBNull.Value : item.ProductionOrderId);
						sqlCommand.Parameters.AddWithValue("ProductionOrderUserId" + i, item.ProductionOrderUserId == null ? (object)DBNull.Value : item.ProductionOrderUserId);
						sqlCommand.Parameters.AddWithValue("ProductionUserId" + i, item.ProductionUserId == null ? (object)DBNull.Value : item.ProductionUserId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_LineItemPlan] SET [LineItemId]=@LineItemId, [OrderDate]=@OrderDate, [OrderId]=@OrderId, [OrderItemId]=@OrderItemId, [OrderUserId]=@OrderUserId, [PlanningQuantityChange]=@PlanningQuantityChange, [PlanningQuantityCumulativeQuantity]=@PlanningQuantityCumulativeQuantity, [PlanningQuantityDeliveryPlanStatusIdentifier]=@PlanningQuantityDeliveryPlanStatusIdentifier, [PlanningQuantityFrequencyIdentifier]=@PlanningQuantityFrequencyIdentifier, [PlanningQuantityQuantity]=@PlanningQuantityQuantity, [PlanningQuantityRequestedShipmentDate]=@PlanningQuantityRequestedShipmentDate, [PlanningQuantityUnitOfMeasure]=@PlanningQuantityUnitOfMeasure, [PlanningQuantityWeeklyPeriodEndDate]=@PlanningQuantityWeeklyPeriodEndDate, [PositionNumber]=@PositionNumber, [ProductionConfirmationDate]=@ProductionConfirmationDate, [ProductionConfirmationId]=@ProductionConfirmationId, [ProductionConfirmationItemId]=@ProductionConfirmationItemId, [ProductionConfirmationUserId]=@ProductionConfirmationUserId, [ProductionDate]=@ProductionDate, [ProductionId]=@ProductionId, [ProductionOrderId]=@ProductionOrderId, [ProductionOrderUserId]=@ProductionOrderUserId, [ProductionUserId]=@ProductionUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("LineItemId", item.LineItemId);
				sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderItemId", item.OrderItemId == null ? (object)DBNull.Value : item.OrderItemId);
				sqlCommand.Parameters.AddWithValue("OrderUserId", item.OrderUserId == null ? (object)DBNull.Value : item.OrderUserId);
				sqlCommand.Parameters.AddWithValue("PlanningQuantityChange", item.PlanningQuantityChange == null ? (object)DBNull.Value : item.PlanningQuantityChange);
				sqlCommand.Parameters.AddWithValue("PlanningQuantityCumulativeQuantity", item.PlanningQuantityCumulativeQuantity == null ? (object)DBNull.Value : item.PlanningQuantityCumulativeQuantity);
				sqlCommand.Parameters.AddWithValue("PlanningQuantityDeliveryPlanStatusIdentifier", item.PlanningQuantityDeliveryPlanStatusIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityDeliveryPlanStatusIdentifier);
				sqlCommand.Parameters.AddWithValue("PlanningQuantityFrequencyIdentifier", item.PlanningQuantityFrequencyIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityFrequencyIdentifier);
				sqlCommand.Parameters.AddWithValue("PlanningQuantityQuantity", item.PlanningQuantityQuantity == null ? (object)DBNull.Value : item.PlanningQuantityQuantity);
				sqlCommand.Parameters.AddWithValue("PlanningQuantityRequestedShipmentDate", item.PlanningQuantityRequestedShipmentDate == null ? (object)DBNull.Value : item.PlanningQuantityRequestedShipmentDate);
				sqlCommand.Parameters.AddWithValue("PlanningQuantityUnitOfMeasure", item.PlanningQuantityUnitOfMeasure == null ? (object)DBNull.Value : item.PlanningQuantityUnitOfMeasure);
				sqlCommand.Parameters.AddWithValue("PlanningQuantityWeeklyPeriodEndDate", item.PlanningQuantityWeeklyPeriodEndDate == null ? (object)DBNull.Value : item.PlanningQuantityWeeklyPeriodEndDate);
				sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
				sqlCommand.Parameters.AddWithValue("ProductionConfirmationDate", item.ProductionConfirmationDate == null ? (object)DBNull.Value : item.ProductionConfirmationDate);
				sqlCommand.Parameters.AddWithValue("ProductionConfirmationId", item.ProductionConfirmationId == null ? (object)DBNull.Value : item.ProductionConfirmationId);
				sqlCommand.Parameters.AddWithValue("ProductionConfirmationItemId", item.ProductionConfirmationItemId == null ? (object)DBNull.Value : item.ProductionConfirmationItemId);
				sqlCommand.Parameters.AddWithValue("ProductionConfirmationUserId", item.ProductionConfirmationUserId == null ? (object)DBNull.Value : item.ProductionConfirmationUserId);
				sqlCommand.Parameters.AddWithValue("ProductionDate", item.ProductionDate == null ? (object)DBNull.Value : item.ProductionDate);
				sqlCommand.Parameters.AddWithValue("ProductionId", item.ProductionId == null ? (object)DBNull.Value : item.ProductionId);
				sqlCommand.Parameters.AddWithValue("ProductionOrderId", item.ProductionOrderId == null ? (object)DBNull.Value : item.ProductionOrderId);
				sqlCommand.Parameters.AddWithValue("ProductionOrderUserId", item.ProductionOrderUserId == null ? (object)DBNull.Value : item.ProductionOrderUserId);
				sqlCommand.Parameters.AddWithValue("ProductionUserId", item.ProductionUserId == null ? (object)DBNull.Value : item.ProductionUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> items)
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
						query += " UPDATE [__EDI_DLF_LineItemPlan] SET "

							+ "[LineItemId]=@LineItemId" + i + ","
							+ "[OrderDate]=@OrderDate" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderItemId]=@OrderItemId" + i + ","
							+ "[OrderUserId]=@OrderUserId" + i + ","
							+ "[PlanningQuantityChange]=@PlanningQuantityChange" + i + ","
							+ "[PlanningQuantityCumulativeQuantity]=@PlanningQuantityCumulativeQuantity" + i + ","
							+ "[PlanningQuantityDeliveryPlanStatusIdentifier]=@PlanningQuantityDeliveryPlanStatusIdentifier" + i + ","
							+ "[PlanningQuantityFrequencyIdentifier]=@PlanningQuantityFrequencyIdentifier" + i + ","
							+ "[PlanningQuantityQuantity]=@PlanningQuantityQuantity" + i + ","
							+ "[PlanningQuantityRequestedShipmentDate]=@PlanningQuantityRequestedShipmentDate" + i + ","
							+ "[PlanningQuantityUnitOfMeasure]=@PlanningQuantityUnitOfMeasure" + i + ","
							+ "[PlanningQuantityWeeklyPeriodEndDate]=@PlanningQuantityWeeklyPeriodEndDate" + i + ","
							+ "[PositionNumber]=@PositionNumber" + i + ","
							+ "[ProductionConfirmationDate]=@ProductionConfirmationDate" + i + ","
							+ "[ProductionConfirmationId]=@ProductionConfirmationId" + i + ","
							+ "[ProductionConfirmationItemId]=@ProductionConfirmationItemId" + i + ","
							+ "[ProductionConfirmationUserId]=@ProductionConfirmationUserId" + i + ","
							+ "[ProductionDate]=@ProductionDate" + i + ","
							+ "[ProductionId]=@ProductionId" + i + ","
							+ "[ProductionOrderId]=@ProductionOrderId" + i + ","
							+ "[ProductionOrderUserId]=@ProductionOrderUserId" + i + ","
							+ "[ProductionUserId]=@ProductionUserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("LineItemId" + i, item.LineItemId);
						sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderItemId" + i, item.OrderItemId == null ? (object)DBNull.Value : item.OrderItemId);
						sqlCommand.Parameters.AddWithValue("OrderUserId" + i, item.OrderUserId == null ? (object)DBNull.Value : item.OrderUserId);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityChange" + i, item.PlanningQuantityChange == null ? (object)DBNull.Value : item.PlanningQuantityChange);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityCumulativeQuantity" + i, item.PlanningQuantityCumulativeQuantity == null ? (object)DBNull.Value : item.PlanningQuantityCumulativeQuantity);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityDeliveryPlanStatusIdentifier" + i, item.PlanningQuantityDeliveryPlanStatusIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityDeliveryPlanStatusIdentifier);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityFrequencyIdentifier" + i, item.PlanningQuantityFrequencyIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityFrequencyIdentifier);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityQuantity" + i, item.PlanningQuantityQuantity == null ? (object)DBNull.Value : item.PlanningQuantityQuantity);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityRequestedShipmentDate" + i, item.PlanningQuantityRequestedShipmentDate == null ? (object)DBNull.Value : item.PlanningQuantityRequestedShipmentDate);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityUnitOfMeasure" + i, item.PlanningQuantityUnitOfMeasure == null ? (object)DBNull.Value : item.PlanningQuantityUnitOfMeasure);
						sqlCommand.Parameters.AddWithValue("PlanningQuantityWeeklyPeriodEndDate" + i, item.PlanningQuantityWeeklyPeriodEndDate == null ? (object)DBNull.Value : item.PlanningQuantityWeeklyPeriodEndDate);
						sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
						sqlCommand.Parameters.AddWithValue("ProductionConfirmationDate" + i, item.ProductionConfirmationDate == null ? (object)DBNull.Value : item.ProductionConfirmationDate);
						sqlCommand.Parameters.AddWithValue("ProductionConfirmationId" + i, item.ProductionConfirmationId == null ? (object)DBNull.Value : item.ProductionConfirmationId);
						sqlCommand.Parameters.AddWithValue("ProductionConfirmationItemId" + i, item.ProductionConfirmationItemId == null ? (object)DBNull.Value : item.ProductionConfirmationItemId);
						sqlCommand.Parameters.AddWithValue("ProductionConfirmationUserId" + i, item.ProductionConfirmationUserId == null ? (object)DBNull.Value : item.ProductionConfirmationUserId);
						sqlCommand.Parameters.AddWithValue("ProductionDate" + i, item.ProductionDate == null ? (object)DBNull.Value : item.ProductionDate);
						sqlCommand.Parameters.AddWithValue("ProductionId" + i, item.ProductionId == null ? (object)DBNull.Value : item.ProductionId);
						sqlCommand.Parameters.AddWithValue("ProductionOrderId" + i, item.ProductionOrderId == null ? (object)DBNull.Value : item.ProductionOrderId);
						sqlCommand.Parameters.AddWithValue("ProductionOrderUserId" + i, item.ProductionOrderUserId == null ? (object)DBNull.Value : item.ProductionOrderUserId);
						sqlCommand.Parameters.AddWithValue("ProductionUserId" + i, item.ProductionUserId == null ? (object)DBNull.Value : item.ProductionUserId);
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
				string query = "DELETE FROM [__EDI_DLF_LineItemPlan] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__EDI_DLF_LineItemPlan] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity GetWithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_LineItemPlan] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_LineItemPlan]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> GetWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> getWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_LineItemPlan] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
		}

		public static long InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//	long response = long.MinValue;


			string query = "INSERT INTO [__EDI_DLF_LineItemPlan] ([LineItemId],[OrderDate],[OrderId],[OrderItemId],[OrderUserId],[PlanningQuantityChange],[PlanningQuantityCumulativeQuantity],[PlanningQuantityDeliveryPlanStatusIdentifier],[PlanningQuantityFrequencyIdentifier],[PlanningQuantityQuantity],[PlanningQuantityRequestedShipmentDate],[PlanningQuantityUnitOfMeasure],[PlanningQuantityWeeklyPeriodEndDate],[PositionNumber],[ProductionConfirmationDate],[ProductionConfirmationId],[ProductionConfirmationItemId],[ProductionConfirmationUserId],[ProductionDate],[ProductionId],[ProductionOrderId],[ProductionOrderUserId],[ProductionUserId]) OUTPUT INSERTED.[Id] VALUES (@LineItemId,@OrderDate,@OrderId,@OrderItemId,@OrderUserId,@PlanningQuantityChange,@PlanningQuantityCumulativeQuantity,@PlanningQuantityDeliveryPlanStatusIdentifier,@PlanningQuantityFrequencyIdentifier,@PlanningQuantityQuantity,@PlanningQuantityRequestedShipmentDate,@PlanningQuantityUnitOfMeasure,@PlanningQuantityWeeklyPeriodEndDate,@PositionNumber,@ProductionConfirmationDate,@ProductionConfirmationId,@ProductionConfirmationItemId,@ProductionConfirmationUserId,@ProductionDate,@ProductionId,@ProductionOrderId,@ProductionOrderUserId,@ProductionUserId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("LineItemId", item.LineItemId);
			sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderItemId", item.OrderItemId == null ? (object)DBNull.Value : item.OrderItemId);
			sqlCommand.Parameters.AddWithValue("OrderUserId", item.OrderUserId == null ? (object)DBNull.Value : item.OrderUserId);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityChange", item.PlanningQuantityChange == null ? (object)DBNull.Value : item.PlanningQuantityChange);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityCumulativeQuantity", item.PlanningQuantityCumulativeQuantity == null ? (object)DBNull.Value : item.PlanningQuantityCumulativeQuantity);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityDeliveryPlanStatusIdentifier", item.PlanningQuantityDeliveryPlanStatusIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityDeliveryPlanStatusIdentifier);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityFrequencyIdentifier", item.PlanningQuantityFrequencyIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityFrequencyIdentifier);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityQuantity", item.PlanningQuantityQuantity == null ? (object)DBNull.Value : item.PlanningQuantityQuantity);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityRequestedShipmentDate", item.PlanningQuantityRequestedShipmentDate == null ? (object)DBNull.Value : item.PlanningQuantityRequestedShipmentDate);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityUnitOfMeasure", item.PlanningQuantityUnitOfMeasure == null ? (object)DBNull.Value : item.PlanningQuantityUnitOfMeasure);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityWeeklyPeriodEndDate", item.PlanningQuantityWeeklyPeriodEndDate == null ? (object)DBNull.Value : item.PlanningQuantityWeeklyPeriodEndDate);
			sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
			sqlCommand.Parameters.AddWithValue("ProductionConfirmationDate", item.ProductionConfirmationDate == null ? (object)DBNull.Value : item.ProductionConfirmationDate);
			sqlCommand.Parameters.AddWithValue("ProductionConfirmationId", item.ProductionConfirmationId == null ? (object)DBNull.Value : item.ProductionConfirmationId);
			sqlCommand.Parameters.AddWithValue("ProductionConfirmationItemId", item.ProductionConfirmationItemId == null ? (object)DBNull.Value : item.ProductionConfirmationItemId);
			sqlCommand.Parameters.AddWithValue("ProductionConfirmationUserId", item.ProductionConfirmationUserId == null ? (object)DBNull.Value : item.ProductionConfirmationUserId);
			sqlCommand.Parameters.AddWithValue("ProductionDate", item.ProductionDate == null ? (object)DBNull.Value : item.ProductionDate);
			sqlCommand.Parameters.AddWithValue("ProductionId", item.ProductionId == null ? (object)DBNull.Value : item.ProductionId);
			sqlCommand.Parameters.AddWithValue("ProductionOrderId", item.ProductionOrderId == null ? (object)DBNull.Value : item.ProductionOrderId);
			sqlCommand.Parameters.AddWithValue("ProductionOrderUserId", item.ProductionOrderUserId == null ? (object)DBNull.Value : item.ProductionOrderUserId);
			sqlCommand.Parameters.AddWithValue("ProductionUserId", item.ProductionUserId == null ? (object)DBNull.Value : item.ProductionUserId);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__EDI_DLF_LineItemPlan] ([LineItemId],[OrderDate],[OrderId],[OrderItemId],[OrderUserId],[PlanningQuantityChange],[PlanningQuantityCumulativeQuantity],[PlanningQuantityDeliveryPlanStatusIdentifier],[PlanningQuantityFrequencyIdentifier],[PlanningQuantityQuantity],[PlanningQuantityRequestedShipmentDate],[PlanningQuantityUnitOfMeasure],[PlanningQuantityWeeklyPeriodEndDate],[PositionNumber],[ProductionConfirmationDate],[ProductionConfirmationId],[ProductionConfirmationItemId],[ProductionConfirmationUserId],[ProductionDate],[ProductionId],[ProductionOrderId],[ProductionOrderUserId],[ProductionUserId]) VALUES ( "

						+ "@LineItemId" + i + ","
						+ "@OrderDate" + i + ","
						+ "@OrderId" + i + ","
						+ "@OrderItemId" + i + ","
						+ "@OrderUserId" + i + ","
						+ "@PlanningQuantityChange" + i + ","
						+ "@PlanningQuantityCumulativeQuantity" + i + ","
						+ "@PlanningQuantityDeliveryPlanStatusIdentifier" + i + ","
						+ "@PlanningQuantityFrequencyIdentifier" + i + ","
						+ "@PlanningQuantityQuantity" + i + ","
						+ "@PlanningQuantityRequestedShipmentDate" + i + ","
						+ "@PlanningQuantityUnitOfMeasure" + i + ","
						+ "@PlanningQuantityWeeklyPeriodEndDate" + i + ","
						+ "@PositionNumber" + i + ","
						+ "@ProductionConfirmationDate" + i + ","
						+ "@ProductionConfirmationId" + i + ","
						+ "@ProductionConfirmationItemId" + i + ","
						+ "@ProductionConfirmationUserId" + i + ","
						+ "@ProductionDate" + i + ","
						+ "@ProductionId" + i + ","
						+ "@ProductionOrderId" + i + ","
						+ "@ProductionOrderUserId" + i + ","
						+ "@ProductionUserId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("LineItemId" + i, item.LineItemId);
					sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderItemId" + i, item.OrderItemId == null ? (object)DBNull.Value : item.OrderItemId);
					sqlCommand.Parameters.AddWithValue("OrderUserId" + i, item.OrderUserId == null ? (object)DBNull.Value : item.OrderUserId);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityChange" + i, item.PlanningQuantityChange == null ? (object)DBNull.Value : item.PlanningQuantityChange);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityCumulativeQuantity" + i, item.PlanningQuantityCumulativeQuantity == null ? (object)DBNull.Value : item.PlanningQuantityCumulativeQuantity);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityDeliveryPlanStatusIdentifier" + i, item.PlanningQuantityDeliveryPlanStatusIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityDeliveryPlanStatusIdentifier);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityFrequencyIdentifier" + i, item.PlanningQuantityFrequencyIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityFrequencyIdentifier);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityQuantity" + i, item.PlanningQuantityQuantity == null ? (object)DBNull.Value : item.PlanningQuantityQuantity);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityRequestedShipmentDate" + i, item.PlanningQuantityRequestedShipmentDate == null ? (object)DBNull.Value : item.PlanningQuantityRequestedShipmentDate);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityUnitOfMeasure" + i, item.PlanningQuantityUnitOfMeasure == null ? (object)DBNull.Value : item.PlanningQuantityUnitOfMeasure);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityWeeklyPeriodEndDate" + i, item.PlanningQuantityWeeklyPeriodEndDate == null ? (object)DBNull.Value : item.PlanningQuantityWeeklyPeriodEndDate);
					sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationDate" + i, item.ProductionConfirmationDate == null ? (object)DBNull.Value : item.ProductionConfirmationDate);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationId" + i, item.ProductionConfirmationId == null ? (object)DBNull.Value : item.ProductionConfirmationId);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationItemId" + i, item.ProductionConfirmationItemId == null ? (object)DBNull.Value : item.ProductionConfirmationItemId);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationUserId" + i, item.ProductionConfirmationUserId == null ? (object)DBNull.Value : item.ProductionConfirmationUserId);
					sqlCommand.Parameters.AddWithValue("ProductionDate" + i, item.ProductionDate == null ? (object)DBNull.Value : item.ProductionDate);
					sqlCommand.Parameters.AddWithValue("ProductionId" + i, item.ProductionId == null ? (object)DBNull.Value : item.ProductionId);
					sqlCommand.Parameters.AddWithValue("ProductionOrderId" + i, item.ProductionOrderId == null ? (object)DBNull.Value : item.ProductionOrderId);
					sqlCommand.Parameters.AddWithValue("ProductionOrderUserId" + i, item.ProductionOrderUserId == null ? (object)DBNull.Value : item.ProductionOrderUserId);
					sqlCommand.Parameters.AddWithValue("ProductionUserId" + i, item.ProductionUserId == null ? (object)DBNull.Value : item.ProductionUserId);
				}

				sqlCommand.CommandText = query;
				try
				{
					return sqlCommand.ExecuteNonQuery();
				} catch(Exception e)
				{

					throw;
				}
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__EDI_DLF_LineItemPlan] SET [LineItemId]=@LineItemId, [OrderDate]=@OrderDate, [OrderId]=@OrderId, [OrderItemId]=@OrderItemId, [OrderUserId]=@OrderUserId, [PlanningQuantityChange]=@PlanningQuantityChange, [PlanningQuantityCumulativeQuantity]=@PlanningQuantityCumulativeQuantity, [PlanningQuantityDeliveryPlanStatusIdentifier]=@PlanningQuantityDeliveryPlanStatusIdentifier, [PlanningQuantityFrequencyIdentifier]=@PlanningQuantityFrequencyIdentifier, [PlanningQuantityQuantity]=@PlanningQuantityQuantity, [PlanningQuantityRequestedShipmentDate]=@PlanningQuantityRequestedShipmentDate, [PlanningQuantityUnitOfMeasure]=@PlanningQuantityUnitOfMeasure, [PlanningQuantityWeeklyPeriodEndDate]=@PlanningQuantityWeeklyPeriodEndDate, [PositionNumber]=@PositionNumber, [ProductionConfirmationDate]=@ProductionConfirmationDate, [ProductionConfirmationId]=@ProductionConfirmationId, [ProductionConfirmationItemId]=@ProductionConfirmationItemId, [ProductionConfirmationUserId]=@ProductionConfirmationUserId, [ProductionDate]=@ProductionDate, [ProductionId]=@ProductionId, [ProductionOrderId]=@ProductionOrderId, [ProductionOrderUserId]=@ProductionOrderUserId, [ProductionUserId]=@ProductionUserId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("LineItemId", item.LineItemId);
			sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId == null ? (object)DBNull.Value : item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderItemId", item.OrderItemId == null ? (object)DBNull.Value : item.OrderItemId);
			sqlCommand.Parameters.AddWithValue("OrderUserId", item.OrderUserId == null ? (object)DBNull.Value : item.OrderUserId);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityChange", item.PlanningQuantityChange == null ? (object)DBNull.Value : item.PlanningQuantityChange);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityCumulativeQuantity", item.PlanningQuantityCumulativeQuantity == null ? (object)DBNull.Value : item.PlanningQuantityCumulativeQuantity);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityDeliveryPlanStatusIdentifier", item.PlanningQuantityDeliveryPlanStatusIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityDeliveryPlanStatusIdentifier);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityFrequencyIdentifier", item.PlanningQuantityFrequencyIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityFrequencyIdentifier);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityQuantity", item.PlanningQuantityQuantity == null ? (object)DBNull.Value : item.PlanningQuantityQuantity);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityRequestedShipmentDate", item.PlanningQuantityRequestedShipmentDate == null ? (object)DBNull.Value : item.PlanningQuantityRequestedShipmentDate);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityUnitOfMeasure", item.PlanningQuantityUnitOfMeasure == null ? (object)DBNull.Value : item.PlanningQuantityUnitOfMeasure);
			sqlCommand.Parameters.AddWithValue("PlanningQuantityWeeklyPeriodEndDate", item.PlanningQuantityWeeklyPeriodEndDate == null ? (object)DBNull.Value : item.PlanningQuantityWeeklyPeriodEndDate);
			sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
			sqlCommand.Parameters.AddWithValue("ProductionConfirmationDate", item.ProductionConfirmationDate == null ? (object)DBNull.Value : item.ProductionConfirmationDate);
			sqlCommand.Parameters.AddWithValue("ProductionConfirmationId", item.ProductionConfirmationId == null ? (object)DBNull.Value : item.ProductionConfirmationId);
			sqlCommand.Parameters.AddWithValue("ProductionConfirmationItemId", item.ProductionConfirmationItemId == null ? (object)DBNull.Value : item.ProductionConfirmationItemId);
			sqlCommand.Parameters.AddWithValue("ProductionConfirmationUserId", item.ProductionConfirmationUserId == null ? (object)DBNull.Value : item.ProductionConfirmationUserId);
			sqlCommand.Parameters.AddWithValue("ProductionDate", item.ProductionDate == null ? (object)DBNull.Value : item.ProductionDate);
			sqlCommand.Parameters.AddWithValue("ProductionId", item.ProductionId == null ? (object)DBNull.Value : item.ProductionId);
			sqlCommand.Parameters.AddWithValue("ProductionOrderId", item.ProductionOrderId == null ? (object)DBNull.Value : item.ProductionOrderId);
			sqlCommand.Parameters.AddWithValue("ProductionOrderUserId", item.ProductionOrderUserId == null ? (object)DBNull.Value : item.ProductionOrderUserId);
			sqlCommand.Parameters.AddWithValue("ProductionUserId", item.ProductionUserId == null ? (object)DBNull.Value : item.ProductionUserId);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [__EDI_DLF_LineItemPlan] SET "

					+ "[LineItemId]=@LineItemId" + i + ","
					+ "[OrderDate]=@OrderDate" + i + ","
					+ "[OrderId]=@OrderId" + i + ","
					+ "[OrderItemId]=@OrderItemId" + i + ","
					+ "[OrderUserId]=@OrderUserId" + i + ","
					+ "[PlanningQuantityChange]=@PlanningQuantityChange" + i + ","
					+ "[PlanningQuantityCumulativeQuantity]=@PlanningQuantityCumulativeQuantity" + i + ","
					+ "[PlanningQuantityDeliveryPlanStatusIdentifier]=@PlanningQuantityDeliveryPlanStatusIdentifier" + i + ","
					+ "[PlanningQuantityFrequencyIdentifier]=@PlanningQuantityFrequencyIdentifier" + i + ","
					+ "[PlanningQuantityQuantity]=@PlanningQuantityQuantity" + i + ","
					+ "[PlanningQuantityRequestedShipmentDate]=@PlanningQuantityRequestedShipmentDate" + i + ","
					+ "[PlanningQuantityUnitOfMeasure]=@PlanningQuantityUnitOfMeasure" + i + ","
					+ "[PlanningQuantityWeeklyPeriodEndDate]=@PlanningQuantityWeeklyPeriodEndDate" + i + ","
					+ "[PositionNumber]=@PositionNumber" + i + ","
					+ "[ProductionConfirmationDate]=@ProductionConfirmationDate" + i + ","
					+ "[ProductionConfirmationId]=@ProductionConfirmationId" + i + ","
					+ "[ProductionConfirmationItemId]=@ProductionConfirmationItemId" + i + ","
					+ "[ProductionConfirmationUserId]=@ProductionConfirmationUserId" + i + ","
					+ "[ProductionDate]=@ProductionDate" + i + ","
					+ "[ProductionId]=@ProductionId" + i + ","
					+ "[ProductionOrderId]=@ProductionOrderId" + i + ","
					+ "[ProductionOrderUserId]=@ProductionOrderUserId" + i + ","
					+ "[ProductionUserId]=@ProductionUserId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("LineItemId" + i, item.LineItemId);
					sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId == null ? (object)DBNull.Value : item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderItemId" + i, item.OrderItemId == null ? (object)DBNull.Value : item.OrderItemId);
					sqlCommand.Parameters.AddWithValue("OrderUserId" + i, item.OrderUserId == null ? (object)DBNull.Value : item.OrderUserId);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityChange" + i, item.PlanningQuantityChange == null ? (object)DBNull.Value : item.PlanningQuantityChange);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityCumulativeQuantity" + i, item.PlanningQuantityCumulativeQuantity == null ? (object)DBNull.Value : item.PlanningQuantityCumulativeQuantity);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityDeliveryPlanStatusIdentifier" + i, item.PlanningQuantityDeliveryPlanStatusIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityDeliveryPlanStatusIdentifier);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityFrequencyIdentifier" + i, item.PlanningQuantityFrequencyIdentifier == null ? (object)DBNull.Value : item.PlanningQuantityFrequencyIdentifier);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityQuantity" + i, item.PlanningQuantityQuantity == null ? (object)DBNull.Value : item.PlanningQuantityQuantity);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityRequestedShipmentDate" + i, item.PlanningQuantityRequestedShipmentDate == null ? (object)DBNull.Value : item.PlanningQuantityRequestedShipmentDate);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityUnitOfMeasure" + i, item.PlanningQuantityUnitOfMeasure == null ? (object)DBNull.Value : item.PlanningQuantityUnitOfMeasure);
					sqlCommand.Parameters.AddWithValue("PlanningQuantityWeeklyPeriodEndDate" + i, item.PlanningQuantityWeeklyPeriodEndDate == null ? (object)DBNull.Value : item.PlanningQuantityWeeklyPeriodEndDate);
					sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationDate" + i, item.ProductionConfirmationDate == null ? (object)DBNull.Value : item.ProductionConfirmationDate);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationId" + i, item.ProductionConfirmationId == null ? (object)DBNull.Value : item.ProductionConfirmationId);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationItemId" + i, item.ProductionConfirmationItemId == null ? (object)DBNull.Value : item.ProductionConfirmationItemId);
					sqlCommand.Parameters.AddWithValue("ProductionConfirmationUserId" + i, item.ProductionConfirmationUserId == null ? (object)DBNull.Value : item.ProductionConfirmationUserId);
					sqlCommand.Parameters.AddWithValue("ProductionDate" + i, item.ProductionDate == null ? (object)DBNull.Value : item.ProductionDate);
					sqlCommand.Parameters.AddWithValue("ProductionId" + i, item.ProductionId == null ? (object)DBNull.Value : item.ProductionId);
					sqlCommand.Parameters.AddWithValue("ProductionOrderId" + i, item.ProductionOrderId == null ? (object)DBNull.Value : item.ProductionOrderId);
					sqlCommand.Parameters.AddWithValue("ProductionOrderUserId" + i, item.ProductionOrderUserId == null ? (object)DBNull.Value : item.ProductionOrderUserId);
					sqlCommand.Parameters.AddWithValue("ProductionUserId" + i, item.ProductionUserId == null ? (object)DBNull.Value : item.ProductionUserId);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__EDI_DLF_LineItemPlan] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__EDI_DLF_LineItemPlan] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> GetByLineItems(List<long> lineIds)
		{
			if(lineIds == null || lineIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_LineItemPlan] WHERE LineItemId IN ({string.Join(", ", lineIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> GetByLineItems(List<long> lineIds, SqlConnection connection, SqlTransaction transaction)
		{
			if(lineIds == null || lineIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [__EDI_DLF_LineItemPlan] WHERE LineItemId IN ({string.Join(", ", lineIds)})";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> GetFirstWithOpenQtyByLineItems(List<long> lineIds)
		{
			if(lineIds == null || lineIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
								SELECT * FROM [__EDI_DLF_LineItemPlan] A JOIN (
								SELECT LineItemId, MIN(PlanningQuantityRequestedShipmentDate) MinPlanningQuantityRequestedShipmentDate FROM [__EDI_DLF_LineItemPlan] WHERE PlanningQuantityQuantity>0 AND LineItemId IN ({string.Join(", ", lineIds)}) GROUP BY LineItemId) AS B
								ON B.LineItemId=A.LineItemId AND B.MinPlanningQuantityRequestedShipmentDate=A.PlanningQuantityRequestedShipmentDate;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
			}
		}

		public static int AddOrder(int id, int orderId, int itemId, int userId, DateTime orderDate)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_LineItemPlan] SET [OrderDate]=@OrderDate, [OrderItemId]=@OrderItemId, [OrderId]=@OrderId, [OrderUserId]=@OrderUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("OrderDate", orderDate);
				sqlCommand.Parameters.AddWithValue("OrderItemId", itemId);
				sqlCommand.Parameters.AddWithValue("OrderId", orderId);
				sqlCommand.Parameters.AddWithValue("OrderUserId", userId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int DeleteOrder(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_LineItemPlan] SET [OrderDate]=@OrderDate, [OrderItemId]=@OrderItemId, [OrderId]=@OrderId, [OrderUserId]=@OrderUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("OrderDate", (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("OrderItemId", (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("OrderId", (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("OrderUserId", (object)DBNull.Value);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int ConfirmProduction(int id, int productionOrderId, int productionUserId, DateTime productionDate)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_LineItemPlan] SET [ProductionDate]=@ProductionDate, [ProductionId]=@ProductionId, [ProductionUserId]=@ProductionUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("ProductionDate", productionDate);
				sqlCommand.Parameters.AddWithValue("ProductionId", productionOrderId);
				sqlCommand.Parameters.AddWithValue("ProductionUserId", productionUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int CancelProduction(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_LineItemPlan] SET [ProductionDate]=@ProductionDate, [ProductionId]=@ProductionId, [ProductionUserId]=@ProductionUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("ProductionDate", (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("ProductionId", (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("ProductionUserId", (object)DBNull.Value);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> GetByOrder(int orderId)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_LineItemPlan] WHERE [OrderId] =@orderId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity> GetByOrder(int orderId, SqlConnection connection, SqlTransaction transaction)
		{

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [__EDI_DLF_LineItemPlan] WHERE [OrderId] =@orderId";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("orderId", orderId);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity>();
			}
		}
		//public static string GetUnloadingPoint(int nr_dlf)
		//{
		//	var dataTable = new DataTable();
		//	using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//	{
		//		sqlConnection.Open();
		//		string query = @"select UnloadingPoint from __EDI_DLF_LineItem where id=(select LineItemId from __EDI_DLF_LineItemPlan where id=@nr_dlf)";

		//		var sqlCommand = new SqlCommand(query, sqlConnection);
		//		sqlCommand.Parameters.AddWithValue("nr_dlf", nr_dlf);

		//		new SqlDataAdapter(sqlCommand).Fill(dataTable);
		//	}

		//	if(dataTable.Rows.Count > 0)
		//	{
		//		return Convert.ToString(dataTable.Rows[0]["UnloadingPoint"]);
		//	}
		//	else
		//	{
		//		return null;
		//	}

		//}
		#endregion Custom Methods

	}
}
