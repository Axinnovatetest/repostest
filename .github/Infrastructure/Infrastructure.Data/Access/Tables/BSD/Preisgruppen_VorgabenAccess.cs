using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class Preisgruppen_VorgabenAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Preisgruppen_Vorgaben] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Preisgruppen_Vorgaben]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Preisgruppen_Vorgaben] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Preisgruppen_Vorgaben] ([Aufschlag],[Aufschlagsatz],[Bemerkung],[Preisgruppe]) OUTPUT INSERTED.[ID] VALUES (@Aufschlag,@Aufschlagsatz,@Bemerkung,@Preisgruppe); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
					sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> items)
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
						query += " INSERT INTO [Preisgruppen_Vorgaben] ([Aufschlag],[Aufschlagsatz],[Bemerkung],[Preisgruppe]) VALUES ( "

							+ "@Aufschlag" + i + ","
							+ "@Aufschlagsatz" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@Preisgruppe" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
						sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Preisgruppen_Vorgaben] SET [Aufschlag]=@Aufschlag, [Aufschlagsatz]=@Aufschlagsatz, [Bemerkung]=@Bemerkung, [Preisgruppe]=@Preisgruppe WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
				sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> items)
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
						query += " UPDATE [Preisgruppen_Vorgaben] SET "

							+ "[Aufschlag]=@Aufschlag" + i + ","
							+ "[Aufschlagsatz]=@Aufschlagsatz" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Preisgruppe]=@Preisgruppe" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
						sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
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
				string query = "DELETE FROM [Preisgruppen_Vorgaben] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Preisgruppen_Vorgaben] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Preisgruppen_Vorgaben] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Preisgruppen_Vorgaben]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Preisgruppen_Vorgaben] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Preisgruppen_Vorgaben] ([Aufschlag],[Aufschlagsatz],[Bemerkung],[Preisgruppe]) OUTPUT INSERTED.[ID] VALUES (@Aufschlag,@Aufschlagsatz,@Bemerkung,@Preisgruppe); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
			sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Preisgruppen_Vorgaben] ([Aufschlag],[Aufschlagsatz],[Bemerkung],[Preisgruppe]) VALUES ( "

						+ "@Aufschlag" + i + ","
						+ "@Aufschlagsatz" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@Preisgruppe" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
					sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Preisgruppen_Vorgaben] SET [Aufschlag]=@Aufschlag, [Aufschlagsatz]=@Aufschlagsatz, [Bemerkung]=@Bemerkung, [Preisgruppe]=@Preisgruppe WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
			sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Preisgruppen_Vorgaben] SET "

					+ "[Aufschlag]=@Aufschlag" + i + ","
					+ "[Aufschlagsatz]=@Aufschlagsatz" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Preisgruppe]=@Preisgruppe" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
					sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Preisgruppen_Vorgaben] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [Preisgruppen_Vorgaben] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction

		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> GetPricingGroups()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Preisgruppen_Vorgaben] ORDER BY Preisgruppe";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
			}
		}

		public static int GetMaxGroup()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT MAX([Preisgruppe]) AS MaxGroup FROM [Preisgruppen_Vorgaben]";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return 0;
			}

			var maxNummer = (dataTable.Rows[0]["MaxGroup"] == System.DBNull.Value)
				? (int?)null
				: Convert.ToInt32(dataTable.Rows[0]["MaxGroup"]);

			return maxNummer ?? 0;
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
