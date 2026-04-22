using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class LagerorteAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity Get(int lagerort_id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] WHERE [Lagerort_id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", lagerort_id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Lagerorte] WHERE [Lagerort_id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Lagerorte] ([Lagerort_id],[Lagerort],[Simulieren],[Standard],[StandardLagerortId],[User_Simulieren],[WerkNachId],[WerkVonId]) OUTPUT INSERTED.[Lagerort_id] VALUES (@Lagerort_id,@Lagerort,@Simulieren,@Standard,@StandardLagerortId,@User_Simulieren,@WerkNachId,@WerkVonId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
					sqlCommand.Parameters.AddWithValue("Simulieren", item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
					sqlCommand.Parameters.AddWithValue("Standard", item.Standard == null ? (object)DBNull.Value : item.Standard);
					sqlCommand.Parameters.AddWithValue("StandardLagerortId", item.StandardLagerortId == null ? (object)DBNull.Value : item.StandardLagerortId);
					sqlCommand.Parameters.AddWithValue("User_Simulieren", item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
					sqlCommand.Parameters.AddWithValue("WerkNachId", item.WerkNachId == null ? (object)DBNull.Value : item.WerkNachId);
					sqlCommand.Parameters.AddWithValue("WerkVonId", item.WerkVonId == null ? (object)DBNull.Value : item.WerkVonId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> items)
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
						query += " INSERT INTO [Lagerorte] ([Lagerort_id],[Lagerort],[Simulieren],[Standard],[StandardLagerortId],[User_Simulieren],[WerkNachId],[WerkVonId]) VALUES ( "

							+ "@Lagerort_id" + i + ","
							+ "@Lagerort" + i + ","
							+ "@Simulieren" + i + ","
							+ "@Standard" + i + ","
							+ "@StandardLagerortId" + i + ","
							+ "@User_Simulieren" + i + ","
							+ "@WerkNachId" + i + ","
							+ "@WerkVonId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
						sqlCommand.Parameters.AddWithValue("Simulieren" + i, item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
						sqlCommand.Parameters.AddWithValue("Standard" + i, item.Standard == null ? (object)DBNull.Value : item.Standard);
						sqlCommand.Parameters.AddWithValue("StandardLagerortId" + i, item.StandardLagerortId == null ? (object)DBNull.Value : item.StandardLagerortId);
						sqlCommand.Parameters.AddWithValue("User_Simulieren" + i, item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
						sqlCommand.Parameters.AddWithValue("WerkNachId" + i, item.WerkNachId == null ? (object)DBNull.Value : item.WerkNachId);
						sqlCommand.Parameters.AddWithValue("WerkVonId" + i, item.WerkVonId == null ? (object)DBNull.Value : item.WerkVonId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lagerorte] SET [Lagerort]=@Lagerort, [Simulieren]=@Simulieren, [Standard]=@Standard, [StandardLagerortId]=@StandardLagerortId, [User_Simulieren]=@User_Simulieren, [WerkNachId]=@WerkNachId, [WerkVonId]=@WerkVonId WHERE [Lagerort_id]=@Lagerort_id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
				sqlCommand.Parameters.AddWithValue("Simulieren", item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
				sqlCommand.Parameters.AddWithValue("Standard", item.Standard == null ? (object)DBNull.Value : item.Standard);
				sqlCommand.Parameters.AddWithValue("StandardLagerortId", item.StandardLagerortId == null ? (object)DBNull.Value : item.StandardLagerortId);
				sqlCommand.Parameters.AddWithValue("User_Simulieren", item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
				sqlCommand.Parameters.AddWithValue("WerkNachId", item.WerkNachId == null ? (object)DBNull.Value : item.WerkNachId);
				sqlCommand.Parameters.AddWithValue("WerkVonId", item.WerkVonId == null ? (object)DBNull.Value : item.WerkVonId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> items)
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
						query += " UPDATE [Lagerorte] SET "

							+ "[Lagerort]=@Lagerort" + i + ","
							+ "[Simulieren]=@Simulieren" + i + ","
							+ "[Standard]=@Standard" + i + ","
							+ "[StandardLagerortId]=@StandardLagerortId" + i + ","
							+ "[User_Simulieren]=@User_Simulieren" + i + ","
							+ "[WerkNachId]=@WerkNachId" + i + ","
							+ "[WerkVonId]=@WerkVonId" + i + " WHERE [Lagerort_id]=@Lagerort_id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
						sqlCommand.Parameters.AddWithValue("Simulieren" + i, item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
						sqlCommand.Parameters.AddWithValue("Standard" + i, item.Standard == null ? (object)DBNull.Value : item.Standard);
						sqlCommand.Parameters.AddWithValue("StandardLagerortId" + i, item.StandardLagerortId == null ? (object)DBNull.Value : item.StandardLagerortId);
						sqlCommand.Parameters.AddWithValue("User_Simulieren" + i, item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
						sqlCommand.Parameters.AddWithValue("WerkNachId" + i, item.WerkNachId == null ? (object)DBNull.Value : item.WerkNachId);
						sqlCommand.Parameters.AddWithValue("WerkVonId" + i, item.WerkVonId == null ? (object)DBNull.Value : item.WerkVonId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int lagerort_id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Lagerorte] WHERE [Lagerort_id]=@Lagerort_id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", lagerort_id);

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

					string query = "DELETE FROM [Lagerorte] WHERE [Lagerort_id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity GetWithTransaction(int lagerort_id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lagerorte] WHERE [Lagerort_id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", lagerort_id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lagerorte]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Lagerorte] WHERE [Lagerort_id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Lagerorte] ([Lagerort_id],[Lagerort],[Simulieren],[Standard],[StandardLagerortId],[User_Simulieren],[WerkNachId],[WerkVonId]) OUTPUT INSERTED.[Lagerort_id] VALUES (@Lagerort_id,@Lagerort,@Simulieren,@Standard,@StandardLagerortId,@User_Simulieren,@WerkNachId,@WerkVonId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
			sqlCommand.Parameters.AddWithValue("Simulieren", item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
			sqlCommand.Parameters.AddWithValue("Standard", item.Standard == null ? (object)DBNull.Value : item.Standard);
			sqlCommand.Parameters.AddWithValue("StandardLagerortId", item.StandardLagerortId == null ? (object)DBNull.Value : item.StandardLagerortId);
			sqlCommand.Parameters.AddWithValue("User_Simulieren", item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
			sqlCommand.Parameters.AddWithValue("WerkNachId", item.WerkNachId == null ? (object)DBNull.Value : item.WerkNachId);
			sqlCommand.Parameters.AddWithValue("WerkVonId", item.WerkVonId == null ? (object)DBNull.Value : item.WerkVonId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Lagerorte] ([Lagerort_id],[Lagerort],[Simulieren],[Standard],[StandardLagerortId],[User_Simulieren],[WerkNachId],[WerkVonId]) VALUES ( "

						+ "@Lagerort_id" + i + ","
						+ "@Lagerort" + i + ","
						+ "@Simulieren" + i + ","
						+ "@Standard" + i + ","
						+ "@StandardLagerortId" + i + ","
						+ "@User_Simulieren" + i + ","
						+ "@WerkNachId" + i + ","
						+ "@WerkVonId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
					sqlCommand.Parameters.AddWithValue("Simulieren" + i, item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
					sqlCommand.Parameters.AddWithValue("Standard" + i, item.Standard == null ? (object)DBNull.Value : item.Standard);
					sqlCommand.Parameters.AddWithValue("StandardLagerortId" + i, item.StandardLagerortId == null ? (object)DBNull.Value : item.StandardLagerortId);
					sqlCommand.Parameters.AddWithValue("User_Simulieren" + i, item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
					sqlCommand.Parameters.AddWithValue("WerkNachId" + i, item.WerkNachId == null ? (object)DBNull.Value : item.WerkNachId);
					sqlCommand.Parameters.AddWithValue("WerkVonId" + i, item.WerkVonId == null ? (object)DBNull.Value : item.WerkVonId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Lagerorte] SET [Lagerort]=@Lagerort, [Simulieren]=@Simulieren, [Standard]=@Standard, [StandardLagerortId]=@StandardLagerortId, [User_Simulieren]=@User_Simulieren, [WerkNachId]=@WerkNachId, [WerkVonId]=@WerkVonId WHERE [Lagerort_id]=@Lagerort_id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
			sqlCommand.Parameters.AddWithValue("Simulieren", item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
			sqlCommand.Parameters.AddWithValue("Standard", item.Standard == null ? (object)DBNull.Value : item.Standard);
			sqlCommand.Parameters.AddWithValue("StandardLagerortId", item.StandardLagerortId == null ? (object)DBNull.Value : item.StandardLagerortId);
			sqlCommand.Parameters.AddWithValue("User_Simulieren", item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
			sqlCommand.Parameters.AddWithValue("WerkNachId", item.WerkNachId == null ? (object)DBNull.Value : item.WerkNachId);
			sqlCommand.Parameters.AddWithValue("WerkVonId", item.WerkVonId == null ? (object)DBNull.Value : item.WerkVonId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Lagerorte] SET "

					+ "[Lagerort]=@Lagerort" + i + ","
					+ "[Simulieren]=@Simulieren" + i + ","
					+ "[Standard]=@Standard" + i + ","
					+ "[StandardLagerortId]=@StandardLagerortId" + i + ","
					+ "[User_Simulieren]=@User_Simulieren" + i + ","
					+ "[WerkNachId]=@WerkNachId" + i + ","
					+ "[WerkVonId]=@WerkVonId" + i + " WHERE [Lagerort_id]=@Lagerort_id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
					sqlCommand.Parameters.AddWithValue("Simulieren" + i, item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
					sqlCommand.Parameters.AddWithValue("Standard" + i, item.Standard == null ? (object)DBNull.Value : item.Standard);
					sqlCommand.Parameters.AddWithValue("StandardLagerortId" + i, item.StandardLagerortId == null ? (object)DBNull.Value : item.StandardLagerortId);
					sqlCommand.Parameters.AddWithValue("User_Simulieren" + i, item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
					sqlCommand.Parameters.AddWithValue("WerkNachId" + i, item.WerkNachId == null ? (object)DBNull.Value : item.WerkNachId);
					sqlCommand.Parameters.AddWithValue("WerkVonId" + i, item.WerkVonId == null ? (object)DBNull.Value : item.WerkVonId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int lagerort_id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Lagerorte] WHERE [Lagerort_id]=@Lagerort_id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", lagerort_id);

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

				string query = "DELETE FROM [Lagerorte] WHERE [Lagerort_id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetCreationLagers()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lagerorte] WHERE Lagerort_id<>14 And Lagerort_id<>16 And Lagerort_id<>17";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetProductionLagers(List<int> productionLagerIds)
		{

			if(productionLagerIds == null || productionLagerIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lagerorte] WHERE [Lagerort_id] IN ({string.Join(",", productionLagerIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity GetByStandard(int standardLagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] WHERE [StandardLagerortId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", standardLagerId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetTransfertLagers()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lagerorte] WHERE Lagerort LIKE 'transfer%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetHauptLagers()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] Where Lagerort LIKE 'Haupt%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}
		public static List<int> GetHauptLagerIds()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT Lagerort_id FROM [Lagerorte] WHERE [Lagerort] LIKE 'Haupt%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x[0])).ToList();
			}
			else
			{
				return new List<int>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetPLLagers()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] Where Lagerort LIKE 'PL%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetByWerke(int werkeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] Where WerkVonId=@werkeId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("werkeId", werkeId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity GetBySingleWerke(int werkeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] Where WerkVonId=@werkeId and [Lagerort_id] in (7,102,42,6,15,26)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("werkeId", werkeId);
				sqlCommand.CommandTimeout = 300;
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity GetById(int lagerortId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] Where [Lagerort_id] = @lagerortId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerortId", lagerortId);
				sqlCommand.CommandTimeout = 300;
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity GetHauptByStandard(int standardLagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] WHERE [StandardLagerortId]=@Id and Lagerort LIKE 'Haupt%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", standardLagerId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int SetInventoryStatus(List<int> warehouseIds, bool status, SqlConnection connection, SqlTransaction transaction)
		{
			if(warehouseIds?.Count <= 0)
			{
				return 0;
			}
			string query = string.Join(" ", warehouseIds.Select(x => $"UPDATE [Lagerorte] SET [Inventory]=@status WHERE [Lagerort_id]={x};"));
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("status", status);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity> GetInInventory(IEnumerable<int> ids)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lagerorte] WHERE Lagerort_id IN ({string.Join(",", ids)}) AND [Inventory]=1";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
			}
		}
		public static IEnumerable<int> GetInInventory()
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT Lagerort_id FROM [Lagerorte] WHERE ISNULL(Inventory,0)=0";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.TryParse(x[0].ToString(), out var r)?r:0);
			}
			else
			{
				return null;
			}
		}
		public static bool IsInInventory(int warehouseId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT ISNULL(Inventory,0) Inventory FROM [Lagerorte] Where Lagerort_id=@lagerId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerId", warehouseId);
				sqlCommand.CommandTimeout = 300;
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return (int.TryParse(dataTable.Rows[0][0].ToString(), out var x) ? x : 0) == 1;
			}
			else
			{
				return false;
			}
		}
		#endregion
	}
}
