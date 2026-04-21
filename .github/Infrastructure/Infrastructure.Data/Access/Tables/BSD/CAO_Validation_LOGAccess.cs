using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class CAO_Validation_LOGAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CAO_Validation_LOG] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CAO_Validation_LOG]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [CAO_Validation_LOG] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [CAO_Validation_LOG] ([artikel_nr],[artikelnummer],[date_time],[kunden_index],[username],[val_status]) OUTPUT INSERTED.[ID] VALUES (@artikel_nr,@artikelnummer,@date_time,@kunden_index,@username,@val_status); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("artikel_nr", item.artikel_nr == null ? (object)DBNull.Value : item.artikel_nr);
					sqlCommand.Parameters.AddWithValue("artikelnummer", item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
					sqlCommand.Parameters.AddWithValue("date_time", item.date_time == null ? (object)DBNull.Value : item.date_time);
					sqlCommand.Parameters.AddWithValue("kunden_index", item.kunden_index == null ? (object)DBNull.Value : item.kunden_index);
					sqlCommand.Parameters.AddWithValue("username", item.username == null ? (object)DBNull.Value : item.username);
					sqlCommand.Parameters.AddWithValue("val_status", item.val_status == null ? (object)DBNull.Value : item.val_status);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> items)
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
						query += " INSERT INTO [CAO_Validation_LOG] ([artikel_nr],[artikelnummer],[date_time],[kunden_index],[username],[val_status]) VALUES ( "

							+ "@artikel_nr" + i + ","
							+ "@artikelnummer" + i + ","
							+ "@date_time" + i + ","
							+ "@kunden_index" + i + ","
							+ "@username" + i + ","
							+ "@val_status" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("artikel_nr" + i, item.artikel_nr == null ? (object)DBNull.Value : item.artikel_nr);
						sqlCommand.Parameters.AddWithValue("artikelnummer" + i, item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
						sqlCommand.Parameters.AddWithValue("date_time" + i, item.date_time == null ? (object)DBNull.Value : item.date_time);
						sqlCommand.Parameters.AddWithValue("kunden_index" + i, item.kunden_index == null ? (object)DBNull.Value : item.kunden_index);
						sqlCommand.Parameters.AddWithValue("username" + i, item.username == null ? (object)DBNull.Value : item.username);
						sqlCommand.Parameters.AddWithValue("val_status" + i, item.val_status == null ? (object)DBNull.Value : item.val_status);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [CAO_Validation_LOG] SET [artikel_nr]=@artikel_nr, [artikelnummer]=@artikelnummer, [date_time]=@date_time, [kunden_index]=@kunden_index, [username]=@username, [val_status]=@val_status WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("artikel_nr", item.artikel_nr == null ? (object)DBNull.Value : item.artikel_nr);
				sqlCommand.Parameters.AddWithValue("artikelnummer", item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
				sqlCommand.Parameters.AddWithValue("date_time", item.date_time == null ? (object)DBNull.Value : item.date_time);
				sqlCommand.Parameters.AddWithValue("kunden_index", item.kunden_index == null ? (object)DBNull.Value : item.kunden_index);
				sqlCommand.Parameters.AddWithValue("username", item.username == null ? (object)DBNull.Value : item.username);
				sqlCommand.Parameters.AddWithValue("val_status", item.val_status == null ? (object)DBNull.Value : item.val_status);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> items)
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
						query += " UPDATE [CAO_Validation_LOG] SET "

							+ "[artikel_nr]=@artikel_nr" + i + ","
							+ "[artikelnummer]=@artikelnummer" + i + ","
							+ "[date_time]=@date_time" + i + ","
							+ "[kunden_index]=@kunden_index" + i + ","
							+ "[username]=@username" + i + ","
							+ "[val_status]=@val_status" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("artikel_nr" + i, item.artikel_nr == null ? (object)DBNull.Value : item.artikel_nr);
						sqlCommand.Parameters.AddWithValue("artikelnummer" + i, item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
						sqlCommand.Parameters.AddWithValue("date_time" + i, item.date_time == null ? (object)DBNull.Value : item.date_time);
						sqlCommand.Parameters.AddWithValue("kunden_index" + i, item.kunden_index == null ? (object)DBNull.Value : item.kunden_index);
						sqlCommand.Parameters.AddWithValue("username" + i, item.username == null ? (object)DBNull.Value : item.username);
						sqlCommand.Parameters.AddWithValue("val_status" + i, item.val_status == null ? (object)DBNull.Value : item.val_status);
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
				string query = "DELETE FROM [CAO_Validation_LOG] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [CAO_Validation_LOG] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [CAO_Validation_LOG] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [CAO_Validation_LOG]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [CAO_Validation_LOG] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [CAO_Validation_LOG] ([artikel_nr],[artikelnummer],[date_time],[kunden_index],[username],[val_status]) OUTPUT INSERTED.[ID] VALUES (@artikel_nr,@artikelnummer,@date_time,@kunden_index,@username,@val_status); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("artikel_nr", item.artikel_nr == null ? (object)DBNull.Value : item.artikel_nr);
			sqlCommand.Parameters.AddWithValue("artikelnummer", item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
			sqlCommand.Parameters.AddWithValue("date_time", item.date_time == null ? (object)DBNull.Value : item.date_time);
			sqlCommand.Parameters.AddWithValue("kunden_index", item.kunden_index == null ? (object)DBNull.Value : item.kunden_index);
			sqlCommand.Parameters.AddWithValue("username", item.username == null ? (object)DBNull.Value : item.username);
			sqlCommand.Parameters.AddWithValue("val_status", item.val_status == null ? (object)DBNull.Value : item.val_status);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [CAO_Validation_LOG] ([artikel_nr],[artikelnummer],[date_time],[kunden_index],[username],[val_status]) VALUES ( "

						+ "@artikel_nr" + i + ","
						+ "@artikelnummer" + i + ","
						+ "@date_time" + i + ","
						+ "@kunden_index" + i + ","
						+ "@username" + i + ","
						+ "@val_status" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("artikel_nr" + i, item.artikel_nr == null ? (object)DBNull.Value : item.artikel_nr);
					sqlCommand.Parameters.AddWithValue("artikelnummer" + i, item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
					sqlCommand.Parameters.AddWithValue("date_time" + i, item.date_time == null ? (object)DBNull.Value : item.date_time);
					sqlCommand.Parameters.AddWithValue("kunden_index" + i, item.kunden_index == null ? (object)DBNull.Value : item.kunden_index);
					sqlCommand.Parameters.AddWithValue("username" + i, item.username == null ? (object)DBNull.Value : item.username);
					sqlCommand.Parameters.AddWithValue("val_status" + i, item.val_status == null ? (object)DBNull.Value : item.val_status);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [CAO_Validation_LOG] SET [artikel_nr]=@artikel_nr, [artikelnummer]=@artikelnummer, [date_time]=@date_time, [kunden_index]=@kunden_index, [username]=@username, [val_status]=@val_status WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("artikel_nr", item.artikel_nr == null ? (object)DBNull.Value : item.artikel_nr);
			sqlCommand.Parameters.AddWithValue("artikelnummer", item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
			sqlCommand.Parameters.AddWithValue("date_time", item.date_time == null ? (object)DBNull.Value : item.date_time);
			sqlCommand.Parameters.AddWithValue("kunden_index", item.kunden_index == null ? (object)DBNull.Value : item.kunden_index);
			sqlCommand.Parameters.AddWithValue("username", item.username == null ? (object)DBNull.Value : item.username);
			sqlCommand.Parameters.AddWithValue("val_status", item.val_status == null ? (object)DBNull.Value : item.val_status);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.CAO_Validation_LOGEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [CAO_Validation_LOG] SET "

					+ "[artikel_nr]=@artikel_nr" + i + ","
					+ "[artikelnummer]=@artikelnummer" + i + ","
					+ "[date_time]=@date_time" + i + ","
					+ "[kunden_index]=@kunden_index" + i + ","
					+ "[username]=@username" + i + ","
					+ "[val_status]=@val_status" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("artikel_nr" + i, item.artikel_nr == null ? (object)DBNull.Value : item.artikel_nr);
					sqlCommand.Parameters.AddWithValue("artikelnummer" + i, item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
					sqlCommand.Parameters.AddWithValue("date_time" + i, item.date_time == null ? (object)DBNull.Value : item.date_time);
					sqlCommand.Parameters.AddWithValue("kunden_index" + i, item.kunden_index == null ? (object)DBNull.Value : item.kunden_index);
					sqlCommand.Parameters.AddWithValue("username" + i, item.username == null ? (object)DBNull.Value : item.username);
					sqlCommand.Parameters.AddWithValue("val_status" + i, item.val_status == null ? (object)DBNull.Value : item.val_status);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [CAO_Validation_LOG] WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ID", id);

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

				string query = "DELETE FROM [CAO_Validation_LOG] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion

		#region Custom Methods



		#endregion
	}
}
