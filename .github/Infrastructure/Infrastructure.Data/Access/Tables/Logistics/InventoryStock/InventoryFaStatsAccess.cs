
namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
	public class InventoryFaStatsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[InventoryStats] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM  [Inventory].[InventoryStats]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM  [Inventory].[InventoryStats] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Inventory].[InventoryStats] ([HqLastRejectionId],[HqLastRejectionName],[HqLastRejectionNotes],[HqLastRejectionTime],[HqNotesHL],[HqNotesPL],[HqValidatorId],[HqValidatorName],[HqValidatorValidateTime],[OpenFaCount],[RohSurplusCount],[RohWihtoutNeedCount],[StartedFaCount],[StartTime],[StartUsername],[StopTime],[StopUsername],[WarehouseId],[WarehouseNotesHL],[WarehouseNotesPL],[WarehouseValidatorId],[WarehouseValidatorName],[WarehouseValidatorValidateTime]) OUTPUT INSERTED.[Id] VALUES (@HqLastRejectionId,@HqLastRejectionName,@HqLastRejectionNotes,@HqLastRejectionTime,@HqNotesHL,@HqNotesPL,@HqValidatorId,@HqValidatorName,@HqValidatorValidateTime,@OpenFaCount,@RohSurplusCount,@RohWihtoutNeedCount,@StartedFaCount,@StartTime,@StartUsername,@StopTime,@StopUsername,@WarehouseId,@WarehouseNotesHL,@WarehouseNotesPL,@WarehouseValidatorId,@WarehouseValidatorName,@WarehouseValidatorValidateTime); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("HqLastRejectionId", item.HqLastRejectionId == null ? (object)DBNull.Value : item.HqLastRejectionId);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionName", item.HqLastRejectionName == null ? (object)DBNull.Value : item.HqLastRejectionName);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionNotes", item.HqLastRejectionNotes == null ? (object)DBNull.Value : item.HqLastRejectionNotes);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionTime", item.HqLastRejectionTime == null ? (object)DBNull.Value : item.HqLastRejectionTime);
					sqlCommand.Parameters.AddWithValue("HqNotesHL", item.HqNotesHL == null ? (object)DBNull.Value : item.HqNotesHL);
					sqlCommand.Parameters.AddWithValue("HqNotesPL", item.HqNotesPL == null ? (object)DBNull.Value : item.HqNotesPL);
					sqlCommand.Parameters.AddWithValue("HqValidatorId", item.HqValidatorId == null ? (object)DBNull.Value : item.HqValidatorId);
					sqlCommand.Parameters.AddWithValue("HqValidatorName", item.HqValidatorName == null ? (object)DBNull.Value : item.HqValidatorName);
					sqlCommand.Parameters.AddWithValue("HqValidatorValidateTime", item.HqValidatorValidateTime == null ? (object)DBNull.Value : item.HqValidatorValidateTime);
					sqlCommand.Parameters.AddWithValue("OpenFaCount", item.OpenFaCount == null ? (object)DBNull.Value : item.OpenFaCount);
					sqlCommand.Parameters.AddWithValue("RohSurplusCount", item.RohSurplusCount == null ? (object)DBNull.Value : item.RohSurplusCount);
					sqlCommand.Parameters.AddWithValue("RohWihtoutNeedCount", item.RohWihtoutNeedCount == null ? (object)DBNull.Value : item.RohWihtoutNeedCount);
					sqlCommand.Parameters.AddWithValue("StartedFaCount", item.StartedFaCount == null ? (object)DBNull.Value : item.StartedFaCount);
					sqlCommand.Parameters.AddWithValue("StartTime", item.StartTime == null ? (object)DBNull.Value : item.StartTime);
					sqlCommand.Parameters.AddWithValue("StartUsername", item.StartUsername == null ? (object)DBNull.Value : item.StartUsername);
					sqlCommand.Parameters.AddWithValue("StopTime", item.StopTime == null ? (object)DBNull.Value : item.StopTime);
					sqlCommand.Parameters.AddWithValue("StopUsername", item.StopUsername == null ? (object)DBNull.Value : item.StopUsername);
					sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseNotesHL", item.WarehouseNotesHL == null ? (object)DBNull.Value : item.WarehouseNotesHL);
					sqlCommand.Parameters.AddWithValue("WarehouseNotesPL", item.WarehouseNotesPL == null ? (object)DBNull.Value : item.WarehouseNotesPL);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorId", item.WarehouseValidatorId == null ? (object)DBNull.Value : item.WarehouseValidatorId);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorName", item.WarehouseValidatorName == null ? (object)DBNull.Value : item.WarehouseValidatorName);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorValidateTime", item.WarehouseValidatorValidateTime == null ? (object)DBNull.Value : item.WarehouseValidatorValidateTime);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Inventory].[InventoryStats] ([HqLastRejectionId],[HqLastRejectionName],[HqLastRejectionNotes],[HqLastRejectionTime],[HqNotesHL],[HqNotesPL],[HqValidatorId],[HqValidatorName],[HqValidatorValidateTime],[OpenFaCount],[RohSurplusCount],[RohWihtoutNeedCount],[StartedFaCount],[StartTime],[StartUsername],[StopTime],[StopUsername],[WarehouseId],[WarehouseNotesHL],[WarehouseNotesPL],[WarehouseValidatorId],[WarehouseValidatorName],[WarehouseValidatorValidateTime]) VALUES ( "

							+ "@HqLastRejectionId" + i + ","
							+ "@HqLastRejectionName" + i + ","
							+ "@HqLastRejectionNotes" + i + ","
							+ "@HqLastRejectionTime" + i + ","
							+ "@HqNotesHL" + i + ","
							+ "@HqNotesPL" + i + ","
							+ "@HqValidatorId" + i + ","
							+ "@HqValidatorName" + i + ","
							+ "@HqValidatorValidateTime" + i + ","
							+ "@OpenFaCount" + i + ","
							+ "@RohSurplusCount" + i + ","
							+ "@RohWihtoutNeedCount" + i + ","
							+ "@StartedFaCount" + i + ","
							+ "@StartTime" + i + ","
							+ "@StartUsername" + i + ","
							+ "@StopTime" + i + ","
							+ "@StopUsername" + i + ","
							+ "@WarehouseId" + i + ","
							+ "@WarehouseNotesHL" + i + ","
							+ "@WarehouseNotesPL" + i + ","
							+ "@WarehouseValidatorId" + i + ","
							+ "@WarehouseValidatorName" + i + ","
							+ "@WarehouseValidatorValidateTime" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("HqLastRejectionId" + i, item.HqLastRejectionId == null ? (object)DBNull.Value : item.HqLastRejectionId);
						sqlCommand.Parameters.AddWithValue("HqLastRejectionName" + i, item.HqLastRejectionName == null ? (object)DBNull.Value : item.HqLastRejectionName);
						sqlCommand.Parameters.AddWithValue("HqLastRejectionNotes" + i, item.HqLastRejectionNotes == null ? (object)DBNull.Value : item.HqLastRejectionNotes);
						sqlCommand.Parameters.AddWithValue("HqLastRejectionTime" + i, item.HqLastRejectionTime == null ? (object)DBNull.Value : item.HqLastRejectionTime);
						sqlCommand.Parameters.AddWithValue("HqNotesHL" + i, item.HqNotesHL == null ? (object)DBNull.Value : item.HqNotesHL);
						sqlCommand.Parameters.AddWithValue("HqNotesPL" + i, item.HqNotesPL == null ? (object)DBNull.Value : item.HqNotesPL);
						sqlCommand.Parameters.AddWithValue("HqValidatorId" + i, item.HqValidatorId == null ? (object)DBNull.Value : item.HqValidatorId);
						sqlCommand.Parameters.AddWithValue("HqValidatorName" + i, item.HqValidatorName == null ? (object)DBNull.Value : item.HqValidatorName);
						sqlCommand.Parameters.AddWithValue("HqValidatorValidateTime" + i, item.HqValidatorValidateTime == null ? (object)DBNull.Value : item.HqValidatorValidateTime);
						sqlCommand.Parameters.AddWithValue("OpenFaCount" + i, item.OpenFaCount == null ? (object)DBNull.Value : item.OpenFaCount);
						sqlCommand.Parameters.AddWithValue("RohSurplusCount" + i, item.RohSurplusCount == null ? (object)DBNull.Value : item.RohSurplusCount);
						sqlCommand.Parameters.AddWithValue("RohWihtoutNeedCount" + i, item.RohWihtoutNeedCount == null ? (object)DBNull.Value : item.RohWihtoutNeedCount);
						sqlCommand.Parameters.AddWithValue("StartedFaCount" + i, item.StartedFaCount == null ? (object)DBNull.Value : item.StartedFaCount);
						sqlCommand.Parameters.AddWithValue("StartTime" + i, item.StartTime == null ? (object)DBNull.Value : item.StartTime);
						sqlCommand.Parameters.AddWithValue("StartUsername" + i, item.StartUsername == null ? (object)DBNull.Value : item.StartUsername);
						sqlCommand.Parameters.AddWithValue("StopTime" + i, item.StopTime == null ? (object)DBNull.Value : item.StopTime);
						sqlCommand.Parameters.AddWithValue("StopUsername" + i, item.StopUsername == null ? (object)DBNull.Value : item.StopUsername);
						sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
						sqlCommand.Parameters.AddWithValue("WarehouseNotesHL" + i, item.WarehouseNotesHL == null ? (object)DBNull.Value : item.WarehouseNotesHL);
						sqlCommand.Parameters.AddWithValue("WarehouseNotesPL" + i, item.WarehouseNotesPL == null ? (object)DBNull.Value : item.WarehouseNotesPL);
						sqlCommand.Parameters.AddWithValue("WarehouseValidatorId" + i, item.WarehouseValidatorId == null ? (object)DBNull.Value : item.WarehouseValidatorId);
						sqlCommand.Parameters.AddWithValue("WarehouseValidatorName" + i, item.WarehouseValidatorName == null ? (object)DBNull.Value : item.WarehouseValidatorName);
						sqlCommand.Parameters.AddWithValue("WarehouseValidatorValidateTime" + i, item.WarehouseValidatorValidateTime == null ? (object)DBNull.Value : item.WarehouseValidatorValidateTime);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Inventory].[InventoryStats] SET [HqLastRejectionId]=@HqLastRejectionId, [HqLastRejectionName]=@HqLastRejectionName, [HqLastRejectionNotes]=@HqLastRejectionNotes, [HqLastRejectionTime]=@HqLastRejectionTime, [HqNotesHL]=@HqNotesHL, [HqNotesPL]=@HqNotesPL, [HqValidatorId]=@HqValidatorId, [HqValidatorName]=@HqValidatorName, [HqValidatorValidateTime]=@HqValidatorValidateTime, [OpenFaCount]=@OpenFaCount, [RohSurplusCount]=@RohSurplusCount, [RohWihtoutNeedCount]=@RohWihtoutNeedCount, [StartedFaCount]=@StartedFaCount, [StartTime]=@StartTime, [StartUsername]=@StartUsername, [StopTime]=@StopTime, [StopUsername]=@StopUsername, [WarehouseId]=@WarehouseId, [WarehouseNotesHL]=@WarehouseNotesHL, [WarehouseNotesPL]=@WarehouseNotesPL, [WarehouseValidatorId]=@WarehouseValidatorId, [WarehouseValidatorName]=@WarehouseValidatorName, [WarehouseValidatorValidateTime]=@WarehouseValidatorValidateTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("HqLastRejectionId", item.HqLastRejectionId == null ? (object)DBNull.Value : item.HqLastRejectionId);
				sqlCommand.Parameters.AddWithValue("HqLastRejectionName", item.HqLastRejectionName == null ? (object)DBNull.Value : item.HqLastRejectionName);
				sqlCommand.Parameters.AddWithValue("HqLastRejectionNotes", item.HqLastRejectionNotes == null ? (object)DBNull.Value : item.HqLastRejectionNotes);
				sqlCommand.Parameters.AddWithValue("HqLastRejectionTime", item.HqLastRejectionTime == null ? (object)DBNull.Value : item.HqLastRejectionTime);
				sqlCommand.Parameters.AddWithValue("HqNotesHL", item.HqNotesHL == null ? (object)DBNull.Value : item.HqNotesHL);
				sqlCommand.Parameters.AddWithValue("HqNotesPL", item.HqNotesPL == null ? (object)DBNull.Value : item.HqNotesPL);
				sqlCommand.Parameters.AddWithValue("HqValidatorId", item.HqValidatorId == null ? (object)DBNull.Value : item.HqValidatorId);
				sqlCommand.Parameters.AddWithValue("HqValidatorName", item.HqValidatorName == null ? (object)DBNull.Value : item.HqValidatorName);
				sqlCommand.Parameters.AddWithValue("HqValidatorValidateTime", item.HqValidatorValidateTime == null ? (object)DBNull.Value : item.HqValidatorValidateTime);
				sqlCommand.Parameters.AddWithValue("OpenFaCount", item.OpenFaCount == null ? (object)DBNull.Value : item.OpenFaCount);
				sqlCommand.Parameters.AddWithValue("RohSurplusCount", item.RohSurplusCount == null ? (object)DBNull.Value : item.RohSurplusCount);
				sqlCommand.Parameters.AddWithValue("RohWihtoutNeedCount", item.RohWihtoutNeedCount == null ? (object)DBNull.Value : item.RohWihtoutNeedCount);
				sqlCommand.Parameters.AddWithValue("StartedFaCount", item.StartedFaCount == null ? (object)DBNull.Value : item.StartedFaCount);
				sqlCommand.Parameters.AddWithValue("StartTime", item.StartTime == null ? (object)DBNull.Value : item.StartTime);
				sqlCommand.Parameters.AddWithValue("StartUsername", item.StartUsername == null ? (object)DBNull.Value : item.StartUsername);
				sqlCommand.Parameters.AddWithValue("StopTime", item.StopTime == null ? (object)DBNull.Value : item.StopTime);
				sqlCommand.Parameters.AddWithValue("StopUsername", item.StopUsername == null ? (object)DBNull.Value : item.StopUsername);
				sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
				sqlCommand.Parameters.AddWithValue("WarehouseNotesHL", item.WarehouseNotesHL == null ? (object)DBNull.Value : item.WarehouseNotesHL);
				sqlCommand.Parameters.AddWithValue("WarehouseNotesPL", item.WarehouseNotesPL == null ? (object)DBNull.Value : item.WarehouseNotesPL);
				sqlCommand.Parameters.AddWithValue("WarehouseValidatorId", item.WarehouseValidatorId == null ? (object)DBNull.Value : item.WarehouseValidatorId);
				sqlCommand.Parameters.AddWithValue("WarehouseValidatorName", item.WarehouseValidatorName == null ? (object)DBNull.Value : item.WarehouseValidatorName);
				sqlCommand.Parameters.AddWithValue("WarehouseValidatorValidateTime", item.WarehouseValidatorValidateTime == null ? (object)DBNull.Value : item.WarehouseValidatorValidateTime);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Inventory].[InventoryStats] SET "

							+ "[HqLastRejectionId]=@HqLastRejectionId" + i + ","
							+ "[HqLastRejectionName]=@HqLastRejectionName" + i + ","
							+ "[HqLastRejectionNotes]=@HqLastRejectionNotes" + i + ","
							+ "[HqLastRejectionTime]=@HqLastRejectionTime" + i + ","
							+ "[HqNotesHL]=@HqNotesHL" + i + ","
							+ "[HqNotesPL]=@HqNotesPL" + i + ","
							+ "[HqValidatorId]=@HqValidatorId" + i + ","
							+ "[HqValidatorName]=@HqValidatorName" + i + ","
							+ "[HqValidatorValidateTime]=@HqValidatorValidateTime" + i + ","
							+ "[OpenFaCount]=@OpenFaCount" + i + ","
							+ "[RohSurplusCount]=@RohSurplusCount" + i + ","
							+ "[RohWihtoutNeedCount]=@RohWihtoutNeedCount" + i + ","
							+ "[StartedFaCount]=@StartedFaCount" + i + ","
							+ "[StartTime]=@StartTime" + i + ","
							+ "[StartUsername]=@StartUsername" + i + ","
							+ "[StopTime]=@StopTime" + i + ","
							+ "[StopUsername]=@StopUsername" + i + ","
							+ "[WarehouseId]=@WarehouseId" + i + ","
							+ "[WarehouseNotesHL]=@WarehouseNotesHL" + i + ","
							+ "[WarehouseNotesPL]=@WarehouseNotesPL" + i + ","
							+ "[WarehouseValidatorId]=@WarehouseValidatorId" + i + ","
							+ "[WarehouseValidatorName]=@WarehouseValidatorName" + i + ","
							+ "[WarehouseValidatorValidateTime]=@WarehouseValidatorValidateTime" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("HqLastRejectionId" + i, item.HqLastRejectionId == null ? (object)DBNull.Value : item.HqLastRejectionId);
						sqlCommand.Parameters.AddWithValue("HqLastRejectionName" + i, item.HqLastRejectionName == null ? (object)DBNull.Value : item.HqLastRejectionName);
						sqlCommand.Parameters.AddWithValue("HqLastRejectionNotes" + i, item.HqLastRejectionNotes == null ? (object)DBNull.Value : item.HqLastRejectionNotes);
						sqlCommand.Parameters.AddWithValue("HqLastRejectionTime" + i, item.HqLastRejectionTime == null ? (object)DBNull.Value : item.HqLastRejectionTime);
						sqlCommand.Parameters.AddWithValue("HqNotesHL" + i, item.HqNotesHL == null ? (object)DBNull.Value : item.HqNotesHL);
						sqlCommand.Parameters.AddWithValue("HqNotesPL" + i, item.HqNotesPL == null ? (object)DBNull.Value : item.HqNotesPL);
						sqlCommand.Parameters.AddWithValue("HqValidatorId" + i, item.HqValidatorId == null ? (object)DBNull.Value : item.HqValidatorId);
						sqlCommand.Parameters.AddWithValue("HqValidatorName" + i, item.HqValidatorName == null ? (object)DBNull.Value : item.HqValidatorName);
						sqlCommand.Parameters.AddWithValue("HqValidatorValidateTime" + i, item.HqValidatorValidateTime == null ? (object)DBNull.Value : item.HqValidatorValidateTime);
						sqlCommand.Parameters.AddWithValue("OpenFaCount" + i, item.OpenFaCount == null ? (object)DBNull.Value : item.OpenFaCount);
						sqlCommand.Parameters.AddWithValue("RohSurplusCount" + i, item.RohSurplusCount == null ? (object)DBNull.Value : item.RohSurplusCount);
						sqlCommand.Parameters.AddWithValue("RohWihtoutNeedCount" + i, item.RohWihtoutNeedCount == null ? (object)DBNull.Value : item.RohWihtoutNeedCount);
						sqlCommand.Parameters.AddWithValue("StartedFaCount" + i, item.StartedFaCount == null ? (object)DBNull.Value : item.StartedFaCount);
						sqlCommand.Parameters.AddWithValue("StartTime" + i, item.StartTime == null ? (object)DBNull.Value : item.StartTime);
						sqlCommand.Parameters.AddWithValue("StartUsername" + i, item.StartUsername == null ? (object)DBNull.Value : item.StartUsername);
						sqlCommand.Parameters.AddWithValue("StopTime" + i, item.StopTime == null ? (object)DBNull.Value : item.StopTime);
						sqlCommand.Parameters.AddWithValue("StopUsername" + i, item.StopUsername == null ? (object)DBNull.Value : item.StopUsername);
						sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
						sqlCommand.Parameters.AddWithValue("WarehouseNotesHL" + i, item.WarehouseNotesHL == null ? (object)DBNull.Value : item.WarehouseNotesHL);
						sqlCommand.Parameters.AddWithValue("WarehouseNotesPL" + i, item.WarehouseNotesPL == null ? (object)DBNull.Value : item.WarehouseNotesPL);
						sqlCommand.Parameters.AddWithValue("WarehouseValidatorId" + i, item.WarehouseValidatorId == null ? (object)DBNull.Value : item.WarehouseValidatorId);
						sqlCommand.Parameters.AddWithValue("WarehouseValidatorName" + i, item.WarehouseValidatorName == null ? (object)DBNull.Value : item.WarehouseValidatorName);
						sqlCommand.Parameters.AddWithValue("WarehouseValidatorValidateTime" + i, item.WarehouseValidatorValidateTime == null ? (object)DBNull.Value : item.WarehouseValidatorValidateTime);
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
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Inventory].[InventoryStats] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Inventory].[InventoryStats] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[InventoryStats] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[InventoryStats]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Inventory].[InventoryStats] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Inventory].[InventoryStats] ([HqLastRejectionId],[HqLastRejectionName],[HqLastRejectionNotes],[HqLastRejectionTime],[HqNotesHL],[HqNotesPL],[HqValidatorId],[HqValidatorName],[HqValidatorValidateTime],[OpenFaCount],[RohSurplusCount],[RohWihtoutNeedCount],[StartedFaCount],[StartTime],[StartUsername],[StopTime],[StopUsername],[WarehouseId],[WarehouseNotesHL],[WarehouseNotesPL],[WarehouseValidatorId],[WarehouseValidatorName],[WarehouseValidatorValidateTime]) OUTPUT INSERTED.[Id] VALUES (@HqLastRejectionId,@HqLastRejectionName,@HqLastRejectionNotes,@HqLastRejectionTime,@HqNotesHL,@HqNotesPL,@HqValidatorId,@HqValidatorName,@HqValidatorValidateTime,@OpenFaCount,@RohSurplusCount,@RohWihtoutNeedCount,@StartedFaCount,@StartTime,@StartUsername,@StopTime,@StopUsername,@WarehouseId,@WarehouseNotesHL,@WarehouseNotesPL,@WarehouseValidatorId,@WarehouseValidatorName,@WarehouseValidatorValidateTime); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("HqLastRejectionId", item.HqLastRejectionId == null ? (object)DBNull.Value : item.HqLastRejectionId);
			sqlCommand.Parameters.AddWithValue("HqLastRejectionName", item.HqLastRejectionName == null ? (object)DBNull.Value : item.HqLastRejectionName);
			sqlCommand.Parameters.AddWithValue("HqLastRejectionNotes", item.HqLastRejectionNotes == null ? (object)DBNull.Value : item.HqLastRejectionNotes);
			sqlCommand.Parameters.AddWithValue("HqLastRejectionTime", item.HqLastRejectionTime == null ? (object)DBNull.Value : item.HqLastRejectionTime);
			sqlCommand.Parameters.AddWithValue("HqNotesHL", item.HqNotesHL == null ? (object)DBNull.Value : item.HqNotesHL);
			sqlCommand.Parameters.AddWithValue("HqNotesPL", item.HqNotesPL == null ? (object)DBNull.Value : item.HqNotesPL);
			sqlCommand.Parameters.AddWithValue("HqValidatorId", item.HqValidatorId == null ? (object)DBNull.Value : item.HqValidatorId);
			sqlCommand.Parameters.AddWithValue("HqValidatorName", item.HqValidatorName == null ? (object)DBNull.Value : item.HqValidatorName);
			sqlCommand.Parameters.AddWithValue("HqValidatorValidateTime", item.HqValidatorValidateTime == null ? (object)DBNull.Value : item.HqValidatorValidateTime);
			sqlCommand.Parameters.AddWithValue("OpenFaCount", item.OpenFaCount == null ? (object)DBNull.Value : item.OpenFaCount);
			sqlCommand.Parameters.AddWithValue("RohSurplusCount", item.RohSurplusCount == null ? (object)DBNull.Value : item.RohSurplusCount);
			sqlCommand.Parameters.AddWithValue("RohWihtoutNeedCount", item.RohWihtoutNeedCount == null ? (object)DBNull.Value : item.RohWihtoutNeedCount);
			sqlCommand.Parameters.AddWithValue("StartedFaCount", item.StartedFaCount == null ? (object)DBNull.Value : item.StartedFaCount);
			sqlCommand.Parameters.AddWithValue("StartTime", item.StartTime == null ? (object)DBNull.Value : item.StartTime);
			sqlCommand.Parameters.AddWithValue("StartUsername", item.StartUsername == null ? (object)DBNull.Value : item.StartUsername);
			sqlCommand.Parameters.AddWithValue("StopTime", item.StopTime == null ? (object)DBNull.Value : item.StopTime);
			sqlCommand.Parameters.AddWithValue("StopUsername", item.StopUsername == null ? (object)DBNull.Value : item.StopUsername);
			sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
			sqlCommand.Parameters.AddWithValue("WarehouseNotesHL", item.WarehouseNotesHL == null ? (object)DBNull.Value : item.WarehouseNotesHL);
			sqlCommand.Parameters.AddWithValue("WarehouseNotesPL", item.WarehouseNotesPL == null ? (object)DBNull.Value : item.WarehouseNotesPL);
			sqlCommand.Parameters.AddWithValue("WarehouseValidatorId", item.WarehouseValidatorId == null ? (object)DBNull.Value : item.WarehouseValidatorId);
			sqlCommand.Parameters.AddWithValue("WarehouseValidatorName", item.WarehouseValidatorName == null ? (object)DBNull.Value : item.WarehouseValidatorName);
			sqlCommand.Parameters.AddWithValue("WarehouseValidatorValidateTime", item.WarehouseValidatorValidateTime == null ? (object)DBNull.Value : item.WarehouseValidatorValidateTime);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Inventory].[InventoryStats] ([HqLastRejectionId],[HqLastRejectionName],[HqLastRejectionNotes],[HqLastRejectionTime],[HqNotesHL],[HqNotesPL],[HqValidatorId],[HqValidatorName],[HqValidatorValidateTime],[OpenFaCount],[RohSurplusCount],[RohWihtoutNeedCount],[StartedFaCount],[StartTime],[StartUsername],[StopTime],[StopUsername],[WarehouseId],[WarehouseNotesHL],[WarehouseNotesPL],[WarehouseValidatorId],[WarehouseValidatorName],[WarehouseValidatorValidateTime]) VALUES ( "

						+ "@HqLastRejectionId" + i + ","
						+ "@HqLastRejectionName" + i + ","
						+ "@HqLastRejectionNotes" + i + ","
						+ "@HqLastRejectionTime" + i + ","
						+ "@HqNotesHL" + i + ","
						+ "@HqNotesPL" + i + ","
						+ "@HqValidatorId" + i + ","
						+ "@HqValidatorName" + i + ","
						+ "@HqValidatorValidateTime" + i + ","
						+ "@OpenFaCount" + i + ","
						+ "@RohSurplusCount" + i + ","
						+ "@RohWihtoutNeedCount" + i + ","
						+ "@StartedFaCount" + i + ","
						+ "@StartTime" + i + ","
						+ "@StartUsername" + i + ","
						+ "@StopTime" + i + ","
						+ "@StopUsername" + i + ","
						+ "@WarehouseId" + i + ","
						+ "@WarehouseNotesHL" + i + ","
						+ "@WarehouseNotesPL" + i + ","
						+ "@WarehouseValidatorId" + i + ","
						+ "@WarehouseValidatorName" + i + ","
						+ "@WarehouseValidatorValidateTime" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("HqLastRejectionId" + i, item.HqLastRejectionId == null ? (object)DBNull.Value : item.HqLastRejectionId);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionName" + i, item.HqLastRejectionName == null ? (object)DBNull.Value : item.HqLastRejectionName);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionNotes" + i, item.HqLastRejectionNotes == null ? (object)DBNull.Value : item.HqLastRejectionNotes);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionTime" + i, item.HqLastRejectionTime == null ? (object)DBNull.Value : item.HqLastRejectionTime);
					sqlCommand.Parameters.AddWithValue("HqNotesHL" + i, item.HqNotesHL == null ? (object)DBNull.Value : item.HqNotesHL);
					sqlCommand.Parameters.AddWithValue("HqNotesPL" + i, item.HqNotesPL == null ? (object)DBNull.Value : item.HqNotesPL);
					sqlCommand.Parameters.AddWithValue("HqValidatorId" + i, item.HqValidatorId == null ? (object)DBNull.Value : item.HqValidatorId);
					sqlCommand.Parameters.AddWithValue("HqValidatorName" + i, item.HqValidatorName == null ? (object)DBNull.Value : item.HqValidatorName);
					sqlCommand.Parameters.AddWithValue("HqValidatorValidateTime" + i, item.HqValidatorValidateTime == null ? (object)DBNull.Value : item.HqValidatorValidateTime);
					sqlCommand.Parameters.AddWithValue("OpenFaCount" + i, item.OpenFaCount == null ? (object)DBNull.Value : item.OpenFaCount);
					sqlCommand.Parameters.AddWithValue("RohSurplusCount" + i, item.RohSurplusCount == null ? (object)DBNull.Value : item.RohSurplusCount);
					sqlCommand.Parameters.AddWithValue("RohWihtoutNeedCount" + i, item.RohWihtoutNeedCount == null ? (object)DBNull.Value : item.RohWihtoutNeedCount);
					sqlCommand.Parameters.AddWithValue("StartedFaCount" + i, item.StartedFaCount == null ? (object)DBNull.Value : item.StartedFaCount);
					sqlCommand.Parameters.AddWithValue("StartTime" + i, item.StartTime == null ? (object)DBNull.Value : item.StartTime);
					sqlCommand.Parameters.AddWithValue("StartUsername" + i, item.StartUsername == null ? (object)DBNull.Value : item.StartUsername);
					sqlCommand.Parameters.AddWithValue("StopTime" + i, item.StopTime == null ? (object)DBNull.Value : item.StopTime);
					sqlCommand.Parameters.AddWithValue("StopUsername" + i, item.StopUsername == null ? (object)DBNull.Value : item.StopUsername);
					sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseNotesHL" + i, item.WarehouseNotesHL == null ? (object)DBNull.Value : item.WarehouseNotesHL);
					sqlCommand.Parameters.AddWithValue("WarehouseNotesPL" + i, item.WarehouseNotesPL == null ? (object)DBNull.Value : item.WarehouseNotesPL);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorId" + i, item.WarehouseValidatorId == null ? (object)DBNull.Value : item.WarehouseValidatorId);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorName" + i, item.WarehouseValidatorName == null ? (object)DBNull.Value : item.WarehouseValidatorName);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorValidateTime" + i, item.WarehouseValidatorValidateTime == null ? (object)DBNull.Value : item.WarehouseValidatorValidateTime);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Inventory].[InventoryStats] SET [HqLastRejectionId]=@HqLastRejectionId, [HqLastRejectionName]=@HqLastRejectionName, [HqLastRejectionNotes]=@HqLastRejectionNotes, [HqLastRejectionTime]=@HqLastRejectionTime, [HqNotesHL]=@HqNotesHL, [HqNotesPL]=@HqNotesPL, [HqValidatorId]=@HqValidatorId, [HqValidatorName]=@HqValidatorName, [HqValidatorValidateTime]=@HqValidatorValidateTime, [OpenFaCount]=@OpenFaCount, [RohSurplusCount]=@RohSurplusCount, [RohWihtoutNeedCount]=@RohWihtoutNeedCount, [StartedFaCount]=@StartedFaCount, [StartTime]=@StartTime, [StartUsername]=@StartUsername, [StopTime]=@StopTime, [StopUsername]=@StopUsername, [WarehouseId]=@WarehouseId, [WarehouseNotesHL]=@WarehouseNotesHL, [WarehouseNotesPL]=@WarehouseNotesPL, [WarehouseValidatorId]=@WarehouseValidatorId, [WarehouseValidatorName]=@WarehouseValidatorName, [WarehouseValidatorValidateTime]=@WarehouseValidatorValidateTime WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("HqLastRejectionId", item.HqLastRejectionId == null ? (object)DBNull.Value : item.HqLastRejectionId);
			sqlCommand.Parameters.AddWithValue("HqLastRejectionName", item.HqLastRejectionName == null ? (object)DBNull.Value : item.HqLastRejectionName);
			sqlCommand.Parameters.AddWithValue("HqLastRejectionNotes", item.HqLastRejectionNotes == null ? (object)DBNull.Value : item.HqLastRejectionNotes);
			sqlCommand.Parameters.AddWithValue("HqLastRejectionTime", item.HqLastRejectionTime == null ? (object)DBNull.Value : item.HqLastRejectionTime);
			sqlCommand.Parameters.AddWithValue("HqNotesHL", item.HqNotesHL == null ? (object)DBNull.Value : item.HqNotesHL);
			sqlCommand.Parameters.AddWithValue("HqNotesPL", item.HqNotesPL == null ? (object)DBNull.Value : item.HqNotesPL);
			sqlCommand.Parameters.AddWithValue("HqValidatorId", item.HqValidatorId == null ? (object)DBNull.Value : item.HqValidatorId);
			sqlCommand.Parameters.AddWithValue("HqValidatorName", item.HqValidatorName == null ? (object)DBNull.Value : item.HqValidatorName);
			sqlCommand.Parameters.AddWithValue("HqValidatorValidateTime", item.HqValidatorValidateTime == null ? (object)DBNull.Value : item.HqValidatorValidateTime);
			sqlCommand.Parameters.AddWithValue("OpenFaCount", item.OpenFaCount == null ? (object)DBNull.Value : item.OpenFaCount);
			sqlCommand.Parameters.AddWithValue("RohSurplusCount", item.RohSurplusCount == null ? (object)DBNull.Value : item.RohSurplusCount);
			sqlCommand.Parameters.AddWithValue("RohWihtoutNeedCount", item.RohWihtoutNeedCount == null ? (object)DBNull.Value : item.RohWihtoutNeedCount);
			sqlCommand.Parameters.AddWithValue("StartedFaCount", item.StartedFaCount == null ? (object)DBNull.Value : item.StartedFaCount);
			sqlCommand.Parameters.AddWithValue("StartTime", item.StartTime == null ? (object)DBNull.Value : item.StartTime);
			sqlCommand.Parameters.AddWithValue("StartUsername", item.StartUsername == null ? (object)DBNull.Value : item.StartUsername);
			sqlCommand.Parameters.AddWithValue("StopTime", item.StopTime == null ? (object)DBNull.Value : item.StopTime);
			sqlCommand.Parameters.AddWithValue("StopUsername", item.StopUsername == null ? (object)DBNull.Value : item.StopUsername);
			sqlCommand.Parameters.AddWithValue("WarehouseId", item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
			sqlCommand.Parameters.AddWithValue("WarehouseNotesHL", item.WarehouseNotesHL == null ? (object)DBNull.Value : item.WarehouseNotesHL);
			sqlCommand.Parameters.AddWithValue("WarehouseNotesPL", item.WarehouseNotesPL == null ? (object)DBNull.Value : item.WarehouseNotesPL);
			sqlCommand.Parameters.AddWithValue("WarehouseValidatorId", item.WarehouseValidatorId == null ? (object)DBNull.Value : item.WarehouseValidatorId);
			sqlCommand.Parameters.AddWithValue("WarehouseValidatorName", item.WarehouseValidatorName == null ? (object)DBNull.Value : item.WarehouseValidatorName);
			sqlCommand.Parameters.AddWithValue("WarehouseValidatorValidateTime", item.WarehouseValidatorValidateTime == null ? (object)DBNull.Value : item.WarehouseValidatorValidateTime);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Inventory].[InventoryStats] SET "

					+ "[HqLastRejectionId]=@HqLastRejectionId" + i + ","
					+ "[HqLastRejectionName]=@HqLastRejectionName" + i + ","
					+ "[HqLastRejectionNotes]=@HqLastRejectionNotes" + i + ","
					+ "[HqLastRejectionTime]=@HqLastRejectionTime" + i + ","
					+ "[HqNotesHL]=@HqNotesHL" + i + ","
					+ "[HqNotesPL]=@HqNotesPL" + i + ","
					+ "[HqValidatorId]=@HqValidatorId" + i + ","
					+ "[HqValidatorName]=@HqValidatorName" + i + ","
					+ "[HqValidatorValidateTime]=@HqValidatorValidateTime" + i + ","
					+ "[OpenFaCount]=@OpenFaCount" + i + ","
					+ "[RohSurplusCount]=@RohSurplusCount" + i + ","
					+ "[RohWihtoutNeedCount]=@RohWihtoutNeedCount" + i + ","
					+ "[StartedFaCount]=@StartedFaCount" + i + ","
					+ "[StartTime]=@StartTime" + i + ","
					+ "[StartUsername]=@StartUsername" + i + ","
					+ "[StopTime]=@StopTime" + i + ","
					+ "[StopUsername]=@StopUsername" + i + ","
					+ "[WarehouseId]=@WarehouseId" + i + ","
					+ "[WarehouseNotesHL]=@WarehouseNotesHL" + i + ","
					+ "[WarehouseNotesPL]=@WarehouseNotesPL" + i + ","
					+ "[WarehouseValidatorId]=@WarehouseValidatorId" + i + ","
					+ "[WarehouseValidatorName]=@WarehouseValidatorName" + i + ","
					+ "[WarehouseValidatorValidateTime]=@WarehouseValidatorValidateTime" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionId" + i, item.HqLastRejectionId == null ? (object)DBNull.Value : item.HqLastRejectionId);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionName" + i, item.HqLastRejectionName == null ? (object)DBNull.Value : item.HqLastRejectionName);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionNotes" + i, item.HqLastRejectionNotes == null ? (object)DBNull.Value : item.HqLastRejectionNotes);
					sqlCommand.Parameters.AddWithValue("HqLastRejectionTime" + i, item.HqLastRejectionTime == null ? (object)DBNull.Value : item.HqLastRejectionTime);
					sqlCommand.Parameters.AddWithValue("HqNotesHL" + i, item.HqNotesHL == null ? (object)DBNull.Value : item.HqNotesHL);
					sqlCommand.Parameters.AddWithValue("HqNotesPL" + i, item.HqNotesPL == null ? (object)DBNull.Value : item.HqNotesPL);
					sqlCommand.Parameters.AddWithValue("HqValidatorId" + i, item.HqValidatorId == null ? (object)DBNull.Value : item.HqValidatorId);
					sqlCommand.Parameters.AddWithValue("HqValidatorName" + i, item.HqValidatorName == null ? (object)DBNull.Value : item.HqValidatorName);
					sqlCommand.Parameters.AddWithValue("HqValidatorValidateTime" + i, item.HqValidatorValidateTime == null ? (object)DBNull.Value : item.HqValidatorValidateTime);
					sqlCommand.Parameters.AddWithValue("OpenFaCount" + i, item.OpenFaCount == null ? (object)DBNull.Value : item.OpenFaCount);
					sqlCommand.Parameters.AddWithValue("RohSurplusCount" + i, item.RohSurplusCount == null ? (object)DBNull.Value : item.RohSurplusCount);
					sqlCommand.Parameters.AddWithValue("RohWihtoutNeedCount" + i, item.RohWihtoutNeedCount == null ? (object)DBNull.Value : item.RohWihtoutNeedCount);
					sqlCommand.Parameters.AddWithValue("StartedFaCount" + i, item.StartedFaCount == null ? (object)DBNull.Value : item.StartedFaCount);
					sqlCommand.Parameters.AddWithValue("StartTime" + i, item.StartTime == null ? (object)DBNull.Value : item.StartTime);
					sqlCommand.Parameters.AddWithValue("StartUsername" + i, item.StartUsername == null ? (object)DBNull.Value : item.StartUsername);
					sqlCommand.Parameters.AddWithValue("StopTime" + i, item.StopTime == null ? (object)DBNull.Value : item.StopTime);
					sqlCommand.Parameters.AddWithValue("StopUsername" + i, item.StopUsername == null ? (object)DBNull.Value : item.StopUsername);
					sqlCommand.Parameters.AddWithValue("WarehouseId" + i, item.WarehouseId == null ? (object)DBNull.Value : item.WarehouseId);
					sqlCommand.Parameters.AddWithValue("WarehouseNotesHL" + i, item.WarehouseNotesHL == null ? (object)DBNull.Value : item.WarehouseNotesHL);
					sqlCommand.Parameters.AddWithValue("WarehouseNotesPL" + i, item.WarehouseNotesPL == null ? (object)DBNull.Value : item.WarehouseNotesPL);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorId" + i, item.WarehouseValidatorId == null ? (object)DBNull.Value : item.WarehouseValidatorId);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorName" + i, item.WarehouseValidatorName == null ? (object)DBNull.Value : item.WarehouseValidatorName);
					sqlCommand.Parameters.AddWithValue("WarehouseValidatorValidateTime" + i, item.WarehouseValidatorValidateTime == null ? (object)DBNull.Value : item.WarehouseValidatorValidateTime);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Inventory].[InventoryStats] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
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

				string query = "DELETE FROM [Inventory].[InventoryStats] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity GetByWarehouse(int warehouseId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM  [Inventory].[InventoryStats] where [WarehouseId]=@warehouseId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity Get(int warehouseId, int year, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[InventoryStats] WHERE [WarehouseId]=@warehouseId AND YEAR([StartTime])=@year";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);
			sqlCommand.Parameters.AddWithValue("year", year);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int DeleteDataFa()
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "TRUNCATE TABLE  [Inventory].[InventoryStats]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int CloseInventory(int warehouseId, string username, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Inventory].[InventoryStats] SET [StopTime]=GETDATE(), [StopUsername]=@username WHERE [WarehouseId]=@warehouseId";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);
				sqlCommand.Parameters.AddWithValue("username", username);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int HQApproveInventoryRelease(int warehouseId, int examinerId, string examinerName, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Inventory].[InventoryStats] SET [HqValidatorId]=@examinerId,[HqValidatorName]=@examinerName, [HqValidatorValidateTime]=GETDATE() WHERE [WarehouseId]=@warehouseId AND YEAR([StartTime])=@year";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);
				sqlCommand.Parameters.AddWithValue("examinerId", examinerId);
				sqlCommand.Parameters.AddWithValue("examinerName", examinerName?.SqlEscape());
				sqlCommand.Parameters.AddWithValue("year", DateTime.UtcNow.Year);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int WarehouseValidateInventory(int warehouseId, int responsibleId, string responsibleName, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Inventory].[InventoryStats] SET [WarehouseValidatorId]=@responsibleId, [WarehouseValidatorName]=@responsibleName, [WarehouseValidatorValidateTime]=GETDATE() WHERE [WarehouseId]=@warehouseId AND YEAR([StartTime])=@year";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);
				sqlCommand.Parameters.AddWithValue("responsibleId", responsibleId);
				sqlCommand.Parameters.AddWithValue("responsibleName", responsibleName?.SqlEscape());
				sqlCommand.Parameters.AddWithValue("year", DateTime.UtcNow.Year);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int UpdateHQNotesInventory(int warehouseId, string remarksHL, string remarksPL, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Inventory].[InventoryStats] SET [HqNotesHL]=@remarksHL, [HqNotesPL]=@remarksPL WHERE [WarehouseId]=@warehouseId AND YEAR([StartTime])=@year";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);
				sqlCommand.Parameters.AddWithValue("remarksHL", remarksHL?.SqlEscape());
				sqlCommand.Parameters.AddWithValue("remarksPL", remarksPL?.SqlEscape());
				sqlCommand.Parameters.AddWithValue("year", DateTime.UtcNow.Year);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Initialize(string username, List<int> warehouseIds, List<int> plWarehouseIds, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds is null || warehouseIds.Count<=0 || plWarehouseIds is null || plWarehouseIds.Count <= 0)
			{
				return -1;
			}
			plWarehouseIds = plWarehouseIds?.Distinct().ToList();
			warehouseIds = warehouseIds?.Distinct().ToList();
			var warehouseId = warehouseIds[0];
			string query = $@"
					DECLARE @LagerortIds TABLE (LagerortId INT);

					/* -- clear old data -- */
					DELETE FROM [Inventory].[InventoryStats] WHERE [WarehouseId] = @LagerId;

					/* -- Insert */
					INSERT INTO @LagerortIds VALUES ({string.Join("),(", plWarehouseIds)});

					/* -- Step 1: Filtered Fertigung rows used in multiple places */
					WITH OpenFertigung AS (
						SELECT ID, Lagerort_id, Artikel_Nr, Anzahl, FA_Gestartet
						FROM Fertigung
						WHERE Kennzeichen = 'offen'
						  AND Lagerort_id IN ({string.Join(",", warehouseIds)})
					),

					/* -- Step 2: Sum of Bedarf per Artikel_Nr */
					FertigungsBedarf AS (
						SELECT p.Artikel_Nr, SUM(ISNULL(p.Anzahl,0) * ISNULL(f.Anzahl,0)) AS Bedarf
						FROM Fertigung_Positionen p
						JOIN OpenFertigung f ON p.ID_Fertigung = f.ID AND ISNULL(f.FA_Gestartet, 0) = 1
						GROUP BY p.Artikel_Nr
					),

					/* -- Step 3: ROH not in BOM */
					ROH_NotInBOM AS (
						SELECT COUNT(DISTINCT a.Artikelnummer) AS ROHNotInBOM
						FROM Artikel a
						JOIN Lager l ON l.[Artikel-Nr] = a.[Artikel-Nr] AND l.Lagerort_id IN (SELECT LagerortId FROM @LagerortIds)
									AND (l.Bestand > 0 OR l.Gesamtbestand > 0)
						LEFT JOIN FertigungsBedarf f ON f.Artikel_Nr = a.[Artikel-Nr]
						WHERE a.Warengruppe = 'ROH' AND f.Artikel_Nr IS NULL
					),

					/* -- Step 4: ROH Surplus */
					ROH_Surplus AS (
						SELECT COUNT(DISTINCT a.Artikelnummer) AS ROHSurplus
						FROM Artikel a
						JOIN Lager l ON l.[Artikel-Nr] = a.[Artikel-Nr] AND l.Lagerort_id IN (SELECT LagerortId FROM @LagerortIds)
									AND l.Bestand > 0
						JOIN FertigungsBedarf f ON f.Artikel_Nr = a.[Artikel-Nr]
						WHERE a.Warengruppe = 'ROH' AND a.Warentyp=1 AND f.Bedarf < l.Bestand AND l.Bestand > 0
					)

					/* -- Save results */
					INSERT INTO [Inventory].[InventoryStats] ([StartTime],[StartUsername],[WarehouseId],[OpenFaCount],[StartedFaCount],[RohWihtoutNeedCount],[RohSurplusCount]) 
					OUTPUT INSERTED.[Id]
					SELECT 
						/* -- start date */
						GETDATE(), @startUsername, @LagerId,
						/* -- Open FA count */
						(SELECT COUNT(*) FROM OpenFertigung) AS TotalOpenFA,
						/* -- Started FA count */
						(SELECT COUNT(*) FROM OpenFertigung WHERE ISNULL(FA_Gestartet, 0) = 1) AS TotalStartedFA,
						/* -- ROH not in BOM */
						(SELECT ROHNotInBOM FROM ROH_NotInBOM) AS TotalROHNotInBOM,
						/* -- ROH Surplus */
						(SELECT ROHSurplus FROM ROH_Surplus) AS TotalROHSurplus;
					";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("LagerId", warehouseId);
				sqlCommand.Parameters.AddWithValue("startUsername", username);
				sqlCommand.CommandTimeout = 500;

				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int HQRejectInventoryRelease(int warehouseId, int examinerId, string examinerName, string notes, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Inventory].[InventoryStats] SET [HqLastRejectionId]=@examinerId,[HqLastRejectionName]=@examinerName, [HqLastRejectionTime]=GETDATE(), [HqLastRejectionNotes]=@notes WHERE [WarehouseId]=@warehouseId AND YEAR([StartTime])=@year";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);
				sqlCommand.Parameters.AddWithValue("examinerId", examinerId);
				sqlCommand.Parameters.AddWithValue("examinerName", examinerName?.SqlEscape());
				sqlCommand.Parameters.AddWithValue("year", DateTime.UtcNow.Year);
				sqlCommand.Parameters.AddWithValue("notes", notes);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int UpdateWarehouseNotesInventory(int warehouseId, string remarksHL, string remarksPL, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Inventory].[InventoryStats] SET [WarehouseNotesHL]=@remarksHL, [WarehouseNotesPL]=@remarksPL WHERE [WarehouseId]=@warehouseId AND YEAR([StartTime])=@year";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("warehouseId", warehouseId);
				sqlCommand.Parameters.AddWithValue("remarksHL", remarksHL?.SqlEscape());
				sqlCommand.Parameters.AddWithValue("remarksPL", remarksPL?.SqlEscape());
				sqlCommand.Parameters.AddWithValue("year", DateTime.UtcNow.Year);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion Custom Methods
	}
}
