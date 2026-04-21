using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
	public class LogsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[Logs] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[Logs]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Inventory].[Logs] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Inventory].[Logs] ([LagerId],[LogDescription],[LogsType],[LogTime],[LogUserId],[LogUserName],[ObjectId],[ObjectName]) OUTPUT INSERTED.[Id] VALUES (@LagerId,@LogDescription,@LogsType,@LogTime,@LogUserId,@LogUserName,@ObjectId,@ObjectName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
					sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogsType", item.LogsType == null ? (object)DBNull.Value : item.LogsType);
					sqlCommand.Parameters.AddWithValue("LogTime", item.LogTime == null ? (object)DBNull.Value : item.LogTime);
					sqlCommand.Parameters.AddWithValue("LogUserId", item.LogUserId == null ? (object)DBNull.Value : item.LogUserId);
					sqlCommand.Parameters.AddWithValue("LogUserName", item.LogUserName == null ? (object)DBNull.Value : item.LogUserName);
					sqlCommand.Parameters.AddWithValue("ObjectId", item.ObjectId == null ? (object)DBNull.Value : item.ObjectId);
					sqlCommand.Parameters.AddWithValue("ObjectName", item.ObjectName == null ? (object)DBNull.Value : item.ObjectName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> items)
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
						query += " INSERT INTO [Inventory].[Logs] ([LagerId],[LogDescription],[LogsType],[LogTime],[LogUserId],[LogUserName],[ObjectId],[ObjectName]) VALUES ( "

							+ "@LagerId" + i + ","
							+ "@LogDescription" + i + ","
							+ "@LogsType" + i + ","
							+ "@LogTime" + i + ","
							+ "@LogUserId" + i + ","
							+ "@LogUserName" + i + ","
							+ "@ObjectId" + i + ","
							+ "@ObjectName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
						sqlCommand.Parameters.AddWithValue("LogsType" + i, item.LogsType == null ? (object)DBNull.Value : item.LogsType);
						sqlCommand.Parameters.AddWithValue("LogTime" + i, item.LogTime == null ? (object)DBNull.Value : item.LogTime);
						sqlCommand.Parameters.AddWithValue("LogUserId" + i, item.LogUserId == null ? (object)DBNull.Value : item.LogUserId);
						sqlCommand.Parameters.AddWithValue("LogUserName" + i, item.LogUserName == null ? (object)DBNull.Value : item.LogUserName);
						sqlCommand.Parameters.AddWithValue("ObjectId" + i, item.ObjectId == null ? (object)DBNull.Value : item.ObjectId);
						sqlCommand.Parameters.AddWithValue("ObjectName" + i, item.ObjectName == null ? (object)DBNull.Value : item.ObjectName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Inventory].[Logs] SET [LagerId]=@LagerId, [LogDescription]=@LogDescription, [LogsType]=@LogsType, [LogTime]=@LogTime, [LogUserId]=@LogUserId, [LogUserName]=@LogUserName, [ObjectId]=@ObjectId, [ObjectName]=@ObjectName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
				sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
				sqlCommand.Parameters.AddWithValue("LogsType", item.LogsType == null ? (object)DBNull.Value : item.LogsType);
				sqlCommand.Parameters.AddWithValue("LogTime", item.LogTime == null ? (object)DBNull.Value : item.LogTime);
				sqlCommand.Parameters.AddWithValue("LogUserId", item.LogUserId == null ? (object)DBNull.Value : item.LogUserId);
				sqlCommand.Parameters.AddWithValue("LogUserName", item.LogUserName == null ? (object)DBNull.Value : item.LogUserName);
				sqlCommand.Parameters.AddWithValue("ObjectId", item.ObjectId == null ? (object)DBNull.Value : item.ObjectId);
				sqlCommand.Parameters.AddWithValue("ObjectName", item.ObjectName == null ? (object)DBNull.Value : item.ObjectName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> items)
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
						query += " UPDATE [Inventory].[Logs] SET "

							+ "[LagerId]=@LagerId" + i + ","
							+ "[LogDescription]=@LogDescription" + i + ","
							+ "[LogsType]=@LogsType" + i + ","
							+ "[LogTime]=@LogTime" + i + ","
							+ "[LogUserId]=@LogUserId" + i + ","
							+ "[LogUserName]=@LogUserName" + i + ","
							+ "[ObjectId]=@ObjectId" + i + ","
							+ "[ObjectName]=@ObjectName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
						sqlCommand.Parameters.AddWithValue("LogsType" + i, item.LogsType == null ? (object)DBNull.Value : item.LogsType);
						sqlCommand.Parameters.AddWithValue("LogTime" + i, item.LogTime == null ? (object)DBNull.Value : item.LogTime);
						sqlCommand.Parameters.AddWithValue("LogUserId" + i, item.LogUserId == null ? (object)DBNull.Value : item.LogUserId);
						sqlCommand.Parameters.AddWithValue("LogUserName" + i, item.LogUserName == null ? (object)DBNull.Value : item.LogUserName);
						sqlCommand.Parameters.AddWithValue("ObjectId" + i, item.ObjectId == null ? (object)DBNull.Value : item.ObjectId);
						sqlCommand.Parameters.AddWithValue("ObjectName" + i, item.ObjectName == null ? (object)DBNull.Value : item.ObjectName);
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
				string query = "DELETE FROM [Inventory].[Logs] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Inventory].[Logs] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[Logs] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[Logs]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Inventory].[Logs] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO [Inventory].[Logs] ([LagerId],[LogDescription],[LogsType],[LogTime],[LogUserId],[LogUserName],[ObjectId],[ObjectName]) OUTPUT INSERTED.[Id] VALUES (@LagerId,@LogDescription,@LogsType,@LogTime,@LogUserId,@LogUserName,@ObjectId,@ObjectName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
			sqlCommand.Parameters.AddWithValue("LogsType", item.LogsType == null ? (object)DBNull.Value : item.LogsType);
			sqlCommand.Parameters.AddWithValue("LogTime", item.LogTime == null ? (object)DBNull.Value : item.LogTime);
			sqlCommand.Parameters.AddWithValue("LogUserId", item.LogUserId == null ? (object)DBNull.Value : item.LogUserId);
			sqlCommand.Parameters.AddWithValue("LogUserName", item.LogUserName == null ? (object)DBNull.Value : item.LogUserName);
			sqlCommand.Parameters.AddWithValue("ObjectId", item.ObjectId == null ? (object)DBNull.Value : item.ObjectId);
			sqlCommand.Parameters.AddWithValue("ObjectName", item.ObjectName == null ? (object)DBNull.Value : item.ObjectName);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Inventory].[Logs] ([LagerId],[LogDescription],[LogsType],[LogTime],[LogUserId],[LogUserName],[ObjectId],[ObjectName]) VALUES ( "

						+ "@LagerId" + i + ","
						+ "@LogDescription" + i + ","
						+ "@LogsType" + i + ","
						+ "@LogTime" + i + ","
						+ "@LogUserId" + i + ","
						+ "@LogUserName" + i + ","
						+ "@ObjectId" + i + ","
						+ "@ObjectName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogsType" + i, item.LogsType == null ? (object)DBNull.Value : item.LogsType);
					sqlCommand.Parameters.AddWithValue("LogTime" + i, item.LogTime == null ? (object)DBNull.Value : item.LogTime);
					sqlCommand.Parameters.AddWithValue("LogUserId" + i, item.LogUserId == null ? (object)DBNull.Value : item.LogUserId);
					sqlCommand.Parameters.AddWithValue("LogUserName" + i, item.LogUserName == null ? (object)DBNull.Value : item.LogUserName);
					sqlCommand.Parameters.AddWithValue("ObjectId" + i, item.ObjectId == null ? (object)DBNull.Value : item.ObjectId);
					sqlCommand.Parameters.AddWithValue("ObjectName" + i, item.ObjectName == null ? (object)DBNull.Value : item.ObjectName);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Inventory].[Logs] SET [LagerId]=@LagerId, [LogDescription]=@LogDescription, [LogsType]=@LogsType, [LogTime]=@LogTime, [LogUserId]=@LogUserId, [LogUserName]=@LogUserName, [ObjectId]=@ObjectId, [ObjectName]=@ObjectName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
			sqlCommand.Parameters.AddWithValue("LogsType", item.LogsType == null ? (object)DBNull.Value : item.LogsType);
			sqlCommand.Parameters.AddWithValue("LogTime", item.LogTime == null ? (object)DBNull.Value : item.LogTime);
			sqlCommand.Parameters.AddWithValue("LogUserId", item.LogUserId == null ? (object)DBNull.Value : item.LogUserId);
			sqlCommand.Parameters.AddWithValue("LogUserName", item.LogUserName == null ? (object)DBNull.Value : item.LogUserName);
			sqlCommand.Parameters.AddWithValue("ObjectId", item.ObjectId == null ? (object)DBNull.Value : item.ObjectId);
			sqlCommand.Parameters.AddWithValue("ObjectName", item.ObjectName == null ? (object)DBNull.Value : item.ObjectName);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Inventory].[Logs] SET "

					+ "[LagerId]=@LagerId" + i + ","
					+ "[LogDescription]=@LogDescription" + i + ","
					+ "[LogsType]=@LogsType" + i + ","
					+ "[LogTime]=@LogTime" + i + ","
					+ "[LogUserId]=@LogUserId" + i + ","
					+ "[LogUserName]=@LogUserName" + i + ","
					+ "[ObjectId]=@ObjectId" + i + ","
					+ "[ObjectName]=@ObjectName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogsType" + i, item.LogsType == null ? (object)DBNull.Value : item.LogsType);
					sqlCommand.Parameters.AddWithValue("LogTime" + i, item.LogTime == null ? (object)DBNull.Value : item.LogTime);
					sqlCommand.Parameters.AddWithValue("LogUserId" + i, item.LogUserId == null ? (object)DBNull.Value : item.LogUserId);
					sqlCommand.Parameters.AddWithValue("LogUserName" + i, item.LogUserName == null ? (object)DBNull.Value : item.LogUserName);
					sqlCommand.Parameters.AddWithValue("ObjectId" + i, item.ObjectId == null ? (object)DBNull.Value : item.ObjectId);
					sqlCommand.Parameters.AddWithValue("ObjectName" + i, item.ObjectName == null ? (object)DBNull.Value : item.ObjectName);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Inventory].[Logs] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [Inventory].[Logs] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods


		#region Custom Methods

		public static int CountLogsData(string filterSearch, int LagerId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = @$"SELECT COUNT(*) FROM [Inventory].[Logs]";
				var isFirstClaus = true;
				if(LagerId > 0)
				{
					query += $"{(isFirstClaus ? " WHERE" : " AND")} [LagerId]=@LagerId";
					isFirstClaus = false;
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId);
				sqlCommand.CommandTimeout = 300;

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity> GetLogsData(string filterSearch, int LagerId, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT * FROM [Inventory].[Logs]";
				var isFirstClaus = true;
				if(LagerId > 0)
				{
					query += $"{(isFirstClaus ? " WHERE" : " AND")} LagerId=@LagerId";
					isFirstClaus = false;
				}
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [LogTime] DESC";
				}
				if(dataPaging is not null)
				{
					query += $"{(dataPaging is null ? "" : $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ")}";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();

			}
			#endregion Custom Methods
		}
	}
}