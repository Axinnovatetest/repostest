using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class OrderChangeItemAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderChangeItem] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderChangeItem]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var results = new List<Entities.Tables.PRS.OrderChangeItemEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [__EDI_OrderChangeItem] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_OrderChangeItem] ([LineItemAmount],[ActionTime],[ActionUserId],[ActionUsername],[CreationTime],[CurrentItemPriceCalculationNet],[CustomerItemNumber],[DesiredDate],[ItemDescription],[ItemNumber],[MeasureUnitQualifier],[Notes],[OrderChangeId],[OrderedQuantity],[OrderId],[OrderReference],[PositionNumber],[Status],[Type],[UnitPriceBasis])  VALUES (@LineItemAmount,@ActionTime,@ActionUserId,@ActionUsername,@CreationTime,@CurrentItemPriceCalculationNet,@CustomerItemNumber,@DesiredDate,@ItemDescription,@ItemNumber,@MeasureUnitQualifier,@Notes,@OrderChangeId,@OrderedQuantity,@OrderId,@OrderReference,@PositionNumber,@Status,@Type,@UnitPriceBasis);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("LineItemAmount", item.LineItemAmount);
					sqlCommand.Parameters.AddWithValue("ActionTime", item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
					sqlCommand.Parameters.AddWithValue("ActionUserId", item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
					sqlCommand.Parameters.AddWithValue("ActionUsername", item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CurrentItemPriceCalculationNet", item.CurrentItemPriceCalculationNet);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("DesiredDate", item.DesiredDate);
					sqlCommand.Parameters.AddWithValue("ItemDescription", item.ItemDescription == null ? (object)DBNull.Value : item.ItemDescription);
					sqlCommand.Parameters.AddWithValue("ItemNumber", item.ItemNumber == null ? (object)DBNull.Value : item.ItemNumber);
					sqlCommand.Parameters.AddWithValue("MeasureUnitQualifier", item.MeasureUnitQualifier);
					sqlCommand.Parameters.AddWithValue("Notes", item.Notes == null ? (object)DBNull.Value : item.Notes);
					sqlCommand.Parameters.AddWithValue("OrderChangeId", item.OrderChangeId);
					sqlCommand.Parameters.AddWithValue("OrderedQuantity", item.OrderedQuantity);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderReference", item.OrderReference);
					sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
					sqlCommand.Parameters.AddWithValue("Status", item.Status);
					sqlCommand.Parameters.AddWithValue("Type", item.Type);
					sqlCommand.Parameters.AddWithValue("UnitPriceBasis", item.UnitPriceBasis);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 20; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__EDI_OrderChangeItem] ([ActionTime],[ActionUserId],[ActionUsername],[CreationTime],[CurrentItemPriceCalculationNet],[CustomerItemNumber],[DesiredDate],[ItemDescription],[ItemNumber],[MeasureUnitQualifier],[Notes],[OrderChangeId],[OrderedQuantity],[OrderId],[OrderReference],[PositionNumber],[Status],[Type],[UnitPriceBasis]) VALUES ( "

							+ "@ActionTime" + i + ","
							+ "@ActionUserId" + i + ","
							+ "@ActionUsername" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CurrentItemPriceCalculationNet" + i + ","
							+ "@CustomerItemNumber" + i + ","
							+ "@DesiredDate" + i + ","
							+ "@ItemDescription" + i + ","
							+ "@ItemNumber" + i + ","
							+ "@MeasureUnitQualifier" + i + ","
							+ "@Notes" + i + ","
							+ "@OrderChangeId" + i + ","
							+ "@OrderedQuantity" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderReference" + i + ","
							+ "@PositionNumber" + i + ","
							+ "@Status" + i + ","
							+ "@Type" + i + ","
							+ "@UnitPriceBasis" + i
								+ "); ";


						sqlCommand.Parameters.AddWithValue("ActionTime" + i, item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
						sqlCommand.Parameters.AddWithValue("ActionUserId" + i, item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
						sqlCommand.Parameters.AddWithValue("ActionUsername" + i, item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CurrentItemPriceCalculationNet" + i, item.CurrentItemPriceCalculationNet);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("DesiredDate" + i, item.DesiredDate);
						sqlCommand.Parameters.AddWithValue("ItemDescription" + i, item.ItemDescription == null ? (object)DBNull.Value : item.ItemDescription);
						sqlCommand.Parameters.AddWithValue("ItemNumber" + i, item.ItemNumber == null ? (object)DBNull.Value : item.ItemNumber);
						sqlCommand.Parameters.AddWithValue("MeasureUnitQualifier" + i, item.MeasureUnitQualifier);
						sqlCommand.Parameters.AddWithValue("Notes" + i, item.Notes == null ? (object)DBNull.Value : item.Notes);
						sqlCommand.Parameters.AddWithValue("OrderChangeId" + i, item.OrderChangeId);
						sqlCommand.Parameters.AddWithValue("OrderedQuantity" + i, item.OrderedQuantity);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderReference" + i, item.OrderReference);
						sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type);
						sqlCommand.Parameters.AddWithValue("UnitPriceBasis" + i, item.UnitPriceBasis);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_OrderChangeItem] SET [LineItemAmount]=@LineItemAmount, [ActionTime]=@ActionTime, [ActionUserId]=@ActionUserId, [ActionUsername]=@ActionUsername, [CreationTime]=@CreationTime, [CurrentItemPriceCalculationNet]=@CurrentItemPriceCalculationNet, [CustomerItemNumber]=@CustomerItemNumber, [DesiredDate]=@DesiredDate, [ItemDescription]=@ItemDescription, [ItemNumber]=@ItemNumber, [MeasureUnitQualifier]=@MeasureUnitQualifier, [Notes]=@Notes, [OrderChangeId]=@OrderChangeId, [OrderedQuantity]=@OrderedQuantity, [OrderId]=@OrderId, [OrderReference]=@OrderReference, [PositionNumber]=@PositionNumber, [Status]=@Status, [Type]=@Type, [UnitPriceBasis]=@UnitPriceBasis WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ActionTime", item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
				sqlCommand.Parameters.AddWithValue("ActionUserId", item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
				sqlCommand.Parameters.AddWithValue("ActionUsername", item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CurrentItemPriceCalculationNet", item.CurrentItemPriceCalculationNet);
				sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
				sqlCommand.Parameters.AddWithValue("DesiredDate", item.DesiredDate);
				sqlCommand.Parameters.AddWithValue("ItemDescription", item.ItemDescription == null ? (object)DBNull.Value : item.ItemDescription);
				sqlCommand.Parameters.AddWithValue("ItemNumber", item.ItemNumber == null ? (object)DBNull.Value : item.ItemNumber);
				sqlCommand.Parameters.AddWithValue("MeasureUnitQualifier", item.MeasureUnitQualifier);
				sqlCommand.Parameters.AddWithValue("Notes", item.Notes == null ? (object)DBNull.Value : item.Notes);
				sqlCommand.Parameters.AddWithValue("OrderChangeId", item.OrderChangeId);
				sqlCommand.Parameters.AddWithValue("OrderedQuantity", item.OrderedQuantity);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderReference", item.OrderReference);
				sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
				sqlCommand.Parameters.AddWithValue("Status", item.Status);
				sqlCommand.Parameters.AddWithValue("Type", item.Type);
				sqlCommand.Parameters.AddWithValue("UnitPriceBasis", item.UnitPriceBasis);
				sqlCommand.Parameters.AddWithValue("LineItemAmount", item.LineItemAmount);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 20; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__EDI_OrderChangeItem] SET "

								+ "[ActionTime]=@ActionTime" + i + ","
								+ "[ActionUserId]=@ActionUserId" + i + ","
								+ "[ActionUsername]=@ActionUsername" + i + ","
								+ "[CreationTime]=@CreationTime" + i + ","
								+ "[CurrentItemPriceCalculationNet]=@CurrentItemPriceCalculationNet" + i + ","
								+ "[CustomerItemNumber]=@CustomerItemNumber" + i + ","
								+ "[DesiredDate]=@DesiredDate" + i + ","
								+ "[ItemDescription]=@ItemDescription" + i + ","
								+ "[ItemNumber]=@ItemNumber" + i + ","
								+ "[MeasureUnitQualifier]=@MeasureUnitQualifier" + i + ","
								+ "[Notes]=@Notes" + i + ","
								+ "[OrderChangeId]=@OrderChangeId" + i + ","
								+ "[OrderedQuantity]=@OrderedQuantity" + i + ","
								+ "[OrderId]=@OrderId" + i + ","
								+ "[OrderReference]=@OrderReference" + i + ","
								+ "[PositionNumber]=@PositionNumber" + i + ","
								+ "[Status]=@Status" + i + ","
								+ "[Type]=@Type" + i + ","
								+ "[LineItemAmount]=@LineItemAmount" + i + ","
								+ "[UnitPriceBasis]=@UnitPriceBasis" + i + " WHERE [Id]=@Id" + i
								+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ActionTime" + i, item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
						sqlCommand.Parameters.AddWithValue("ActionUserId" + i, item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
						sqlCommand.Parameters.AddWithValue("ActionUsername" + i, item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CurrentItemPriceCalculationNet" + i, item.CurrentItemPriceCalculationNet);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("DesiredDate" + i, item.DesiredDate);
						sqlCommand.Parameters.AddWithValue("ItemDescription" + i, item.ItemDescription == null ? (object)DBNull.Value : item.ItemDescription);
						sqlCommand.Parameters.AddWithValue("ItemNumber" + i, item.ItemNumber == null ? (object)DBNull.Value : item.ItemNumber);
						sqlCommand.Parameters.AddWithValue("MeasureUnitQualifier" + i, item.MeasureUnitQualifier);
						sqlCommand.Parameters.AddWithValue("Notes" + i, item.Notes == null ? (object)DBNull.Value : item.Notes);
						sqlCommand.Parameters.AddWithValue("OrderChangeId" + i, item.OrderChangeId);
						sqlCommand.Parameters.AddWithValue("OrderedQuantity" + i, item.OrderedQuantity);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderReference" + i, item.OrderReference);
						sqlCommand.Parameters.AddWithValue("PositionNumber" + i, item.PositionNumber);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type);
						sqlCommand.Parameters.AddWithValue("UnitPriceBasis" + i, item.UnitPriceBasis);
						sqlCommand.Parameters.AddWithValue("LineItemAmount" + i, item.LineItemAmount);
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
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__EDI_OrderChangeItem] WHERE [Id]=@Id";
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
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [__EDI_OrderChangeItem] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = -1;
			string query = "INSERT INTO [__EDI_OrderChangeItem] ([LineItemAmount],[ActionTime],[ActionUserId],[ActionUsername],[CreationTime],[CurrentItemPriceCalculationNet],[CustomerItemNumber],[DesiredDate],[ItemDescription],[ItemNumber],[MeasureUnitQualifier],[Notes],[OrderChangeId],[OrderedQuantity],[OrderId],[OrderReference],[PositionNumber],[Status],[Type],[UnitPriceBasis])  VALUES (@LineItemAmount,@ActionTime,@ActionUserId,@ActionUsername,@CreationTime,@CurrentItemPriceCalculationNet,@CustomerItemNumber,@DesiredDate,@ItemDescription,@ItemNumber,@MeasureUnitQualifier,@Notes,@OrderChangeId,@OrderedQuantity,@OrderId,@OrderReference,@PositionNumber,@Status,@Type,@UnitPriceBasis);";
			query += "SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{

				sqlCommand.Parameters.AddWithValue("LineItemAmount", item.LineItemAmount);
				sqlCommand.Parameters.AddWithValue("ActionTime", item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
				sqlCommand.Parameters.AddWithValue("ActionUserId", item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
				sqlCommand.Parameters.AddWithValue("ActionUsername", item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CurrentItemPriceCalculationNet", item.CurrentItemPriceCalculationNet);
				sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
				sqlCommand.Parameters.AddWithValue("DesiredDate", item.DesiredDate);
				sqlCommand.Parameters.AddWithValue("ItemDescription", item.ItemDescription == null ? (object)DBNull.Value : item.ItemDescription);
				sqlCommand.Parameters.AddWithValue("ItemNumber", item.ItemNumber == null ? (object)DBNull.Value : item.ItemNumber);
				sqlCommand.Parameters.AddWithValue("MeasureUnitQualifier", item.MeasureUnitQualifier);
				sqlCommand.Parameters.AddWithValue("Notes", item.Notes == null ? (object)DBNull.Value : item.Notes);
				sqlCommand.Parameters.AddWithValue("OrderChangeId", item.OrderChangeId);
				sqlCommand.Parameters.AddWithValue("OrderedQuantity", item.OrderedQuantity);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderReference", item.OrderReference);
				sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
				sqlCommand.Parameters.AddWithValue("Status", item.Status);
				sqlCommand.Parameters.AddWithValue("Type", item.Type);
				sqlCommand.Parameters.AddWithValue("UnitPriceBasis", item.UnitPriceBasis);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int results = -1;
			string query = "UPDATE [__EDI_OrderChangeItem] SET [LineItemAmount]=@LineItemAmount, [ActionTime]=@ActionTime, [ActionUserId]=@ActionUserId, [ActionUsername]=@ActionUsername, [CreationTime]=@CreationTime, [CurrentItemPriceCalculationNet]=@CurrentItemPriceCalculationNet, [CustomerItemNumber]=@CustomerItemNumber, [DesiredDate]=@DesiredDate, [ItemDescription]=@ItemDescription, [ItemNumber]=@ItemNumber, [MeasureUnitQualifier]=@MeasureUnitQualifier, [Notes]=@Notes, [OrderChangeId]=@OrderChangeId, [OrderedQuantity]=@OrderedQuantity, [OrderId]=@OrderId, [OrderReference]=@OrderReference, [PositionNumber]=@PositionNumber, [Status]=@Status, [Type]=@Type, [UnitPriceBasis]=@UnitPriceBasis WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ActionTime", item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
				sqlCommand.Parameters.AddWithValue("ActionUserId", item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
				sqlCommand.Parameters.AddWithValue("ActionUsername", item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CurrentItemPriceCalculationNet", item.CurrentItemPriceCalculationNet);
				sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
				sqlCommand.Parameters.AddWithValue("DesiredDate", item.DesiredDate);
				sqlCommand.Parameters.AddWithValue("ItemDescription", item.ItemDescription == null ? (object)DBNull.Value : item.ItemDescription);
				sqlCommand.Parameters.AddWithValue("ItemNumber", item.ItemNumber == null ? (object)DBNull.Value : item.ItemNumber);
				sqlCommand.Parameters.AddWithValue("MeasureUnitQualifier", item.MeasureUnitQualifier);
				sqlCommand.Parameters.AddWithValue("Notes", item.Notes == null ? (object)DBNull.Value : item.Notes);
				sqlCommand.Parameters.AddWithValue("OrderChangeId", item.OrderChangeId);
				sqlCommand.Parameters.AddWithValue("OrderedQuantity", item.OrderedQuantity);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderReference", item.OrderReference);
				sqlCommand.Parameters.AddWithValue("PositionNumber", item.PositionNumber);
				sqlCommand.Parameters.AddWithValue("Status", item.Status);
				sqlCommand.Parameters.AddWithValue("Type", item.Type);
				sqlCommand.Parameters.AddWithValue("UnitPriceBasis", item.UnitPriceBasis);
				sqlCommand.Parameters.AddWithValue("LineItemAmount", item.LineItemAmount);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> GetByOrderChangeIds(List<int> orderChangesIds)
		{
			if(orderChangesIds != null && orderChangesIds.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var results = new List<Entities.Tables.PRS.OrderChangeItemEntity>();
				if(orderChangesIds.Count <= maxQueryNumber)
				{
					results = getByOrderChangeIds(orderChangesIds);
				}
				else
				{
					int batchNumber = orderChangesIds.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByOrderChangeIds(orderChangesIds.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByOrderChangeIds(orderChangesIds.GetRange(batchNumber * maxQueryNumber, orderChangesIds.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> getByOrderChangeIds(List<int> orderChangesIds)
		{
			if(orderChangesIds != null && orderChangesIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < orderChangesIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, orderChangesIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [__EDI_OrderChangeItem] WHERE [OrderChangeId] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderChangeItem] WHERE [OrderId]=@orderId";

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
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> GetByOrderId(int orderId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_OrderChangeItem] WHERE [OrderId]=@orderId";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> GetByPosition(int orderId, int positionNumber)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderChangeItem] WHERE [OrderId]=@orderId and [PositionNumber]=@positionNumber";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("orderId", orderId);
				sqlCommand.Parameters.AddWithValue("positionNumber", positionNumber);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> GetByChangePosition(int orderId, int positionNumber, int changeType)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderChangeItem] WHERE [OrderId]=@orderId and [PositionNumber]=@positionNumber and [Type]=@changeType";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("orderId", orderId);
				sqlCommand.Parameters.AddWithValue("positionNumber", positionNumber);
				sqlCommand.Parameters.AddWithValue("changeType", changeType);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
			}
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
