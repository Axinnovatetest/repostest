using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class LagerorteAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity Get(int lagerort_id)
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
				return new Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> Get()
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> get(List<int> ids)
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
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Lagerorte] ([Lagerort_id],[Lagerort],[Simulieren],[Standard],[User_Simulieren]) OUTPUT INSERTED.[Lagerort_id] VALUES (@Lagerort_id,@Lagerort,@Simulieren,@Standard,@User_Simulieren); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
					sqlCommand.Parameters.AddWithValue("Simulieren", item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
					sqlCommand.Parameters.AddWithValue("Standard", item.Standard == null ? (object)DBNull.Value : item.Standard);
					sqlCommand.Parameters.AddWithValue("User_Simulieren", item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> items)
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
						query += " INSERT INTO [Lagerorte] ([Lagerort_id],[Lagerort],[Simulieren],[Standard],[User_Simulieren]) VALUES ( "

							+ "@Lagerort_id" + i + ","
							+ "@Lagerort" + i + ","
							+ "@Simulieren" + i + ","
							+ "@Standard" + i + ","
							+ "@User_Simulieren" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
						sqlCommand.Parameters.AddWithValue("Simulieren" + i, item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
						sqlCommand.Parameters.AddWithValue("Standard" + i, item.Standard == null ? (object)DBNull.Value : item.Standard);
						sqlCommand.Parameters.AddWithValue("User_Simulieren" + i, item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lagerorte] SET [Lagerort]=@Lagerort, [Simulieren]=@Simulieren, [Standard]=@Standard, [User_Simulieren]=@User_Simulieren WHERE [Lagerort_id]=@Lagerort_id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
				sqlCommand.Parameters.AddWithValue("Simulieren", item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
				sqlCommand.Parameters.AddWithValue("Standard", item.Standard == null ? (object)DBNull.Value : item.Standard);
				sqlCommand.Parameters.AddWithValue("User_Simulieren", item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> items)
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
							+ "[User_Simulieren]=@User_Simulieren" + i + " WHERE [Lagerort_id]=@Lagerort_id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
						sqlCommand.Parameters.AddWithValue("Simulieren" + i, item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
						sqlCommand.Parameters.AddWithValue("Standard" + i, item.Standard == null ? (object)DBNull.Value : item.Standard);
						sqlCommand.Parameters.AddWithValue("User_Simulieren" + i, item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
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
		public static Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity GetWithTransaction(int lagerort_id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lagerorte] WHERE [Lagerort_id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", lagerort_id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lagerorte]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Lagerorte] ([Lagerort_id],[Lagerort],[Simulieren],[Standard],[User_Simulieren]) OUTPUT INSERTED.[Lagerort_id] VALUES (@Lagerort_id,@Lagerort,@Simulieren,@Standard,@User_Simulieren); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
			sqlCommand.Parameters.AddWithValue("Simulieren", item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
			sqlCommand.Parameters.AddWithValue("Standard", item.Standard == null ? (object)DBNull.Value : item.Standard);
			sqlCommand.Parameters.AddWithValue("User_Simulieren", item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Lagerorte] ([Lagerort_id],[Lagerort],[Simulieren],[Standard],[User_Simulieren]) VALUES ( "

						+ "@Lagerort_id" + i + ","
						+ "@Lagerort" + i + ","
						+ "@Simulieren" + i + ","
						+ "@Standard" + i + ","
						+ "@User_Simulieren" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
					sqlCommand.Parameters.AddWithValue("Simulieren" + i, item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
					sqlCommand.Parameters.AddWithValue("Standard" + i, item.Standard == null ? (object)DBNull.Value : item.Standard);
					sqlCommand.Parameters.AddWithValue("User_Simulieren" + i, item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Lagerorte] SET [Lagerort]=@Lagerort, [Simulieren]=@Simulieren, [Standard]=@Standard, [User_Simulieren]=@User_Simulieren WHERE [Lagerort_id]=@Lagerort_id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
			sqlCommand.Parameters.AddWithValue("Simulieren", item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
			sqlCommand.Parameters.AddWithValue("Standard", item.Standard == null ? (object)DBNull.Value : item.Standard);
			sqlCommand.Parameters.AddWithValue("User_Simulieren", item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					+ "[User_Simulieren]=@User_Simulieren" + i + " WHERE [Lagerort_id]=@Lagerort_id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
					sqlCommand.Parameters.AddWithValue("Simulieren" + i, item.Simulieren == null ? (object)DBNull.Value : item.Simulieren);
					sqlCommand.Parameters.AddWithValue("Standard" + i, item.Standard == null ? (object)DBNull.Value : item.Standard);
					sqlCommand.Parameters.AddWithValue("User_Simulieren" + i, item.User_Simulieren == null ? (object)DBNull.Value : item.User_Simulieren);
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
		public static Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerEntity GetActualStockInLagerPerArticle(List<int?> LagersList, int ArtikelNr)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerEntity();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND Lagerort_id in({pattern})";
				}
				sqlConnection.Open();
				string query =
					"SELECT SUM(Bestand)  STOCK FROM Lager L " + " " +
					$" Where [Artikel-Nr] = {ArtikelNr}" + " " +
					Lagerort_sub_filter + " " +
					"GROUP BY L.[Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerEntity(x)).FirstOrDefault();
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerEntity();
			}
		}
		public static async Task<Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerEntity> GetActualStockInLagerPerArticleAsync(List<int?> LagersList, int ArtikelNr)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerEntity();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND Lagerort_id in({pattern})";
				}
				await sqlConnection.OpenAsync().ConfigureAwait(false);
				string query =
					"SELECT SUM(Bestand)  STOCK FROM Lager L " + " " +
					$" Where [Artikel-Nr] = {ArtikelNr}" + " " +
					Lagerort_sub_filter + " " +
					"GROUP BY L.[Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				await Task.Run(() => DbExecution.Fill(sqlCommand, dataTable)).ConfigureAwait(false);
				//DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerEntity(x)).FirstOrDefault();
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerEntity();
			}
		}
		public static async Task<Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeEntity> GetActualStockInLagerPerArticleByTypeAsync
			(List<int?> LagersList,
			int ArtikelNr,
			int MainLager,
			int ProductionLager)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeEntity();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"{pattern}";
				}
				await sqlConnection.OpenAsync().ConfigureAwait(false);
				string query =
				$@"select  ISNULL(res1.InitStock,0) - ISNULL(res2.reserved_quantity,0) totalInitStock from 
				(
				select a.[Artikel-Nr],SUM(l.Bestand) InitStock ,a.Warentyp from Lager l 
				JOIN Artikel A On  A.[Artikel-Nr] = l.[Artikel-Nr] 
				AND ISNULL(a.aktiv,0) = 1
				AND l.Lagerort_id IN ({Lagerort_sub_filter},(CASE WHEN a.Warentyp = 2  THEN  {ProductionLager} ELSE -1000 END )) 
				AND a.[Artikel-Nr] = {ArtikelNr}
				group by a.[Artikel-Nr] ,a.Warentyp 
				) res1
				left join 
				(
				select h.Artikel_Nr, h.Lagerort_ID , SUM(h.Menge_reserviert) reserved_quantity  from tbl_Planung_gestartet h
				WHERE 
				Lagerort_ID = {MainLager}
				AND Artikel_Nr = {ArtikelNr}
				group by h.Artikel_Nr, h.Lagerort_ID
				
				) res2 
				on  res1.[Artikel-Nr] = res2.Artikel_Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				await Task.Run(() => DbExecution.Fill(sqlCommand, dataTable)).ConfigureAwait(false);
				//DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeEntity(x)).FirstOrDefault();
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeEntity();
			}
		}
		//   stock with at once ....
		public static async Task<List<Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeWithArtikelNrEntity>> GetActualStockInLagerForArticlesByTypeAsync(List<int?> LagersList, List<int> ArticlesNr, int MainLager, int ProductionLager)
		{
			if(LagersList is null || LagersList.Count <= 0 || ArticlesNr is null || ArticlesNr.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeWithArtikelNrEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Lagerort_sub_filter = "";
				string ArticlesNr_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"{pattern}";
				}

				if(ArticlesNr is not null && (ArticlesNr.Count > 0))
				{
					string pattern = null;
					foreach(var item in ArticlesNr)
					{
						if(ArticlesNr.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(ArticlesNr.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					ArticlesNr_sub_filter = $"{pattern}";
				}
				await sqlConnection.OpenAsync().ConfigureAwait(false);
				string query =
				$@"select  ISNULL(res1.InitStock,0) - ISNULL(res2.reserved_quantity,0) totalInitStock,res1.[Artikel-Nr] ArtikelNr from 
				(
				select a.[Artikel-Nr],SUM(l.Bestand) InitStock ,a.Warentyp from Lager l 
				JOIN Artikel A On  A.[Artikel-Nr] = l.[Artikel-Nr] 
				AND ISNULL(a.aktiv,0) = 1
				AND l.Lagerort_id IN ({Lagerort_sub_filter},(CASE WHEN a.Warentyp = 2  THEN  {ProductionLager} ELSE -1000 END )) 
				AND a.[Artikel-Nr] IN ({ArticlesNr_sub_filter})
				group by a.[Artikel-Nr] ,a.Warentyp 
				) res1
				left join 
				(
				select h.Artikel_Nr, h.Lagerort_ID , SUM(h.Menge_reserviert) reserved_quantity  from tbl_Planung_gestartet h
				WHERE 
				Lagerort_ID = {MainLager}
				AND Artikel_Nr IN ({ArticlesNr_sub_filter})
				group by h.Artikel_Nr, h.Lagerort_ID
				
				) res2 
				on  res1.[Artikel-Nr] = res2.Artikel_Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				await Task.Run(() => DbExecution.Fill(sqlCommand, dataTable)).ConfigureAwait(false);
				//DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeWithArtikelNrEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeWithArtikelNrEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeEntity GetActualStockInLagerPerArticleByType(List<int?> LagersList, int ArtikelNr, int MainLager, int ProductionLager)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeEntity();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"{pattern}";
				}
				sqlConnection.Open();
				string query =
				$@"select  ISNULL(res1.InitStock,0) - ISNULL(res2.reserved_quantity,0) totalInitStock from 
				(
				select a.[Artikel-Nr],SUM(l.Bestand) InitStock ,a.Warentyp from Lager l 
				JOIN Artikel A On  A.[Artikel-Nr] = l.[Artikel-Nr] 
				AND ISNULL(a.aktiv,0) = 1
				AND l.Lagerort_id IN ({Lagerort_sub_filter},(CASE WHEN a.Warentyp = 2  THEN  {ProductionLager} ELSE -1000 END )) 
				AND a.[Artikel-Nr] = {ArtikelNr}
				group by a.[Artikel-Nr] ,a.Warentyp 
				) res1
				left join 
				(
				select h.Artikel_Nr, h.Lagerort_ID , SUM(h.Menge_reserviert) reserved_quantity  from tbl_Planung_gestartet h
				WHERE 
				Lagerort_ID = {MainLager}
				AND Artikel_Nr = {ArtikelNr}
				group by h.Artikel_Nr, h.Lagerort_ID
				
				) res2 
				on  res1.[Artikel-Nr] = res2.Artikel_Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeEntity(x)).FirstOrDefault();
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.StockInLagerTypeEntity();
			}
		}
		public static Infrastructure.Data.Entities.Tables.MTM.Orders.MinStockInLagerEntity GetMinimumStockInLagerPerArticle(List<int?> LagersList, int ArtikelNr)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.MinStockInLagerEntity();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND L.Lagerort_id IN({pattern})";
				}
				sqlConnection.Open();
				string query =
					"SELECT TOP(1)  Mindestbestand FROM Lager L " + " " +
					$"WHERE L.[Artikel-Nr] = {(ArtikelNr > 0 ? ArtikelNr : 0)} " + " " +
					Lagerort_sub_filter;

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Orders.MinStockInLagerEntity(x)).FirstOrDefault();
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.MTM.Orders.MinStockInLagerEntity();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> GetActive()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerorte] WHERE Lagerorte.Lagerort<>'-'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>();
			}
		}
		#endregion Custom Methods

	}
}
