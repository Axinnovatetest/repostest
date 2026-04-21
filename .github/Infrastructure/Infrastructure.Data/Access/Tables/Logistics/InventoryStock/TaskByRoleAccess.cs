using System.Text;

namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
	public class TaskByRoleAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[TasksByRole] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[TasksByRole]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Inventory].[TasksByRole] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Inventory].[TasksByRole] ([Id],[lagerId],[phase],[role],[status],[title]) OUTPUT INSERTED.[Id] VALUES (@Id,@lagerId,@phase,@role,@status,@title); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Id", item.Id);
					sqlCommand.Parameters.AddWithValue("lagerId", item.lagerId == null ? (object)DBNull.Value : item.lagerId);
					sqlCommand.Parameters.AddWithValue("phase", item.phase);
					sqlCommand.Parameters.AddWithValue("role", item.role);
					sqlCommand.Parameters.AddWithValue("status", item.status);
					sqlCommand.Parameters.AddWithValue("title", item.title);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items)
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
						query += " INSERT INTO [Inventory].[TasksByRole] ([Id],[lagerId],[phase],[role],[status],[title]) VALUES ( "

							+ "@Id" + i + ","
							+ "@lagerId" + i + ","
							+ "@phase" + i + ","
							+ "@role" + i + ","
							+ "@status" + i + ","
							+ "@title" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("lagerId" + i, item.lagerId == null ? (object)DBNull.Value : item.lagerId);
						sqlCommand.Parameters.AddWithValue("phase" + i, item.phase);
						sqlCommand.Parameters.AddWithValue("role" + i, item.role);
						sqlCommand.Parameters.AddWithValue("status" + i, item.status);
						sqlCommand.Parameters.AddWithValue("title" + i, item.title);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Inventory].[TasksByRole] SET [lagerId]=@lagerId, [phase]=@phase, [role]=@role, [status]=@status, [title]=@title WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("lagerId", item.lagerId == null ? (object)DBNull.Value : item.lagerId);
				sqlCommand.Parameters.AddWithValue("phase", item.phase);
				sqlCommand.Parameters.AddWithValue("role", item.role);
				sqlCommand.Parameters.AddWithValue("status", item.status);
				sqlCommand.Parameters.AddWithValue("title", item.title);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items)
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
						query += " UPDATE [Inventory].[TasksByRole] SET "

							+ "[lagerId]=@lagerId" + i + ","
							+ "[phase]=@phase" + i + ","
							+ "[role]=@role" + i + ","
							+ "[status]=@status" + i + ","
							+ "[title]=@title" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("lagerId" + i, item.lagerId == null ? (object)DBNull.Value : item.lagerId);
						sqlCommand.Parameters.AddWithValue("phase" + i, item.phase);
						sqlCommand.Parameters.AddWithValue("role" + i, item.role);
						sqlCommand.Parameters.AddWithValue("status" + i, item.status);
						sqlCommand.Parameters.AddWithValue("title" + i, item.title);
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
				string query = "DELETE FROM [Inventory].[TasksByRole] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Inventory].[TasksByRole] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[TasksByRole] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[TasksByRole]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Inventory].[TasksByRole] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Inventory].[TasksByRole] ([Id],[lagerId],[phase],[role],[status],[title]) OUTPUT INSERTED.[Id] VALUES (@Id,@lagerId,@phase,@role,@status,@title); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("lagerId", item.lagerId == null ? (object)DBNull.Value : item.lagerId);
			sqlCommand.Parameters.AddWithValue("phase", item.phase);
			sqlCommand.Parameters.AddWithValue("role", item.role);
			sqlCommand.Parameters.AddWithValue("status", item.status);
			sqlCommand.Parameters.AddWithValue("title", item.title);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Inventory].[TasksByRole] ([Id],[lagerId],[phase],[role],[status],[title]) VALUES ( "

						+ "@Id" + i + ","
						+ "@lagerId" + i + ","
						+ "@phase" + i + ","
						+ "@role" + i + ","
						+ "@status" + i + ","
						+ "@title" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("lagerId" + i, item.lagerId == null ? (object)DBNull.Value : item.lagerId);
					sqlCommand.Parameters.AddWithValue("phase" + i, item.phase);
					sqlCommand.Parameters.AddWithValue("role" + i, item.role);
					sqlCommand.Parameters.AddWithValue("status" + i, item.status);
					sqlCommand.Parameters.AddWithValue("title" + i, item.title);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Inventory].[TasksByRole] SET [lagerId]=@lagerId, [phase]=@phase, [role]=@role, [status]=@status, [title]=@title WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("lagerId", item.lagerId == null ? (object)DBNull.Value : item.lagerId);
			sqlCommand.Parameters.AddWithValue("phase", item.phase);
			sqlCommand.Parameters.AddWithValue("role", item.role);
			sqlCommand.Parameters.AddWithValue("status", item.status);
			sqlCommand.Parameters.AddWithValue("title", item.title);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Inventory].[TasksByRole] SET "

					+ "[lagerId]=@lagerId" + i + ","
					+ "[phase]=@phase" + i + ","
					+ "[role]=@role" + i + ","
					+ "[status]=@status" + i + ","
					+ "[title]=@title" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("lagerId" + i, item.lagerId == null ? (object)DBNull.Value : item.lagerId);
					sqlCommand.Parameters.AddWithValue("phase" + i, item.phase);
					sqlCommand.Parameters.AddWithValue("role" + i, item.role);
					sqlCommand.Parameters.AddWithValue("status" + i, item.status);
					sqlCommand.Parameters.AddWithValue("title" + i, item.title);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Inventory].[TasksByRole] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [Inventory].[TasksByRole] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods
		#region Custom Methods
		public static int UpdateTaskStatus(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
				string query = "UPDATE [Inventory].[TasksByRole] SET [status]=@status WHERE [Id]=@Id and [lagerId]=@lagerId";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("status", item.status);
				sqlCommand.Parameters.AddWithValue("lagerId", item.lagerId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateTaskStatus(IEnumerable<string> items, int lagerId, int status, SqlConnection connection, SqlTransaction transaction)
		{
			if(items == null || items.Count()==0)
			{
				return 0;
			}
			string query = $"UPDATE [Inventory].[TasksByRole] SET [status]=@status WHERE [lagerId]=@lagerId AND LEFT([title], CHARINDEX('.', [title]) - 1) IN ('{string.Join("','", items)}')";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("status", status);
				sqlCommand.Parameters.AddWithValue("lagerId", lagerId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> GetWithLager(int lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[TasksByRole] WHERE [lagerId]=@lagerId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerId", lager);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity Get(int id, int lager, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Inventory].[TasksByRole] WHERE [Id]=@Id and [lagerId]=@lagerId";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("lagerId", lager);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int InsertBulk(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity> items)
		{
			if(items == null || items.Count == 0)
				return -1;

			int insertedCount = 0;

			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				using(var sqlTransaction = sqlConnection.BeginTransaction())
				{
					try
					{
						var query = new StringBuilder();
						query.Append("INSERT INTO [Inventory].[TasksByRole] ([Id],[lagerId],[phase],[role],[status],[title]) VALUES ");

						var parameters = new List<SqlParameter>();

						for(int i = 0; i < items.Count; i++)
						{
							var item = items[i];
							if(i > 0)
								query.Append(",");

							query.Append($"(@Id{i}, @lagerId{i}, @phase{i}, @role{i}, @status{i}, @title{i})");

							parameters.Add(new SqlParameter($"@Id{i}", item.Id));
							parameters.Add(new SqlParameter($"@lagerId{i}", (object?)item.lagerId ?? DBNull.Value));
							parameters.Add(new SqlParameter($"@phase{i}", item.phase ?? (object)DBNull.Value));
							parameters.Add(new SqlParameter($"@role{i}", item.role ?? (object)DBNull.Value));
							parameters.Add(new SqlParameter($"@status{i}", item.status));
							parameters.Add(new SqlParameter($"@title{i}", item.title ?? (object)DBNull.Value));
						}

						using(var sqlCommand = new SqlCommand(query.ToString(), sqlConnection, sqlTransaction))
						{
							sqlCommand.Parameters.AddRange(parameters.ToArray());
							insertedCount = DbExecution.ExecuteNonQuery(sqlCommand);
						}

						sqlTransaction.Commit();
					} catch
					{
						sqlTransaction.Rollback();
						throw;
					}
				}
			}

			return insertedCount;
		}
		public static int Count()
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [Inventory].[TasksByRole]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return (int)DbExecution.ExecuteScalar(sqlCommand);
			}
		}
		public static int InitITTask(int lagerId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Inventory].[TasksByRole] SET [status]=2 WHERE LEFT([Title],2)='1.' and [lagerId]=@lagerId";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("lagerId", lagerId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int CountLogisticsTasksByStatus(IEnumerable<char> items, int lagerId, int status, SqlConnection connection, SqlTransaction transaction)
		{
			if(items == null || items.Count() == 0)
			{
				return 0;
			}
			string query = $"SELECT COUNT(*) FROM [Inventory].[TasksByRole] WHERE ISNULL([status],0)=@status AND [lagerId]=@lagerId AND LEFT([Title],1) IN ('{string.Join("','", items)}')";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("lagerId", lagerId);
				sqlCommand.Parameters.AddWithValue("status", status);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var x) ? x : 0;
			}
		}
		public static int CountLogisticsTasksByStatus(IEnumerable<char> items, int lagerId, int status)
		{
			if(items == null || items.Count() == 0)
			{
				return 0;
			}
			string query = $"SELECT COUNT(*) FROM [Inventory].[TasksByRole] WHERE ISNULL([status],0)=@status AND [lagerId]=@lagerId AND LEFT([Title],1) IN ('{string.Join("','", items)}')";
			using(var connection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand(query, connection))
			{
				connection.Open();
				sqlCommand.Parameters.AddWithValue("lagerId", lagerId);
				sqlCommand.Parameters.AddWithValue("status", status);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var x) ? x : 0;
			}
		}
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity GetByNumber(string taskNumber, int lagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[TasksByRole] WHERE TRIM(LEFT([Title], CHARINDEX('.', [Title]) - 1))=@taskNumber AND LagerId=@lagerId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("taskNumber", taskNumber.SqlEscape());
				sqlCommand.Parameters.AddWithValue("lagerId", lagerId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int RevertInventoryRelease(int warehouseId, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [Inventory].[TasksByRole] SET [status]=0 WHERE TRIM(LEFT([Title], CHARINDEX('.', [Title]) - 1))='h' AND [lagerId]=@lagerId";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("lagerId", warehouseId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion Custom Methods
	}
}
