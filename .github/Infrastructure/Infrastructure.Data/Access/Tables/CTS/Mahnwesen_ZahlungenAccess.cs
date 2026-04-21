using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Mahnwesen_ZahlungenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Mahnwesen_Zahlungen] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Mahnwesen_Zahlungen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Mahnwesen_Zahlungen] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Mahnwesen_Zahlungen] ([Datum],[gebucht],[Haben_DM],[Haben_FW],[Mahn_ID],[Soll_DM],[Soll_FW],[Text]) OUTPUT INSERTED.[ID] VALUES (@Datum,@gebucht,@Haben_DM,@Haben_FW,@Mahn_ID,@Soll_DM,@Soll_FW,@Text); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("Haben_DM", item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
					sqlCommand.Parameters.AddWithValue("Haben_FW", item.Haben_FW == null ? (object)DBNull.Value : item.Haben_FW);
					sqlCommand.Parameters.AddWithValue("Mahn_ID", item.Mahn_ID == null ? (object)DBNull.Value : item.Mahn_ID);
					sqlCommand.Parameters.AddWithValue("Soll_DM", item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
					sqlCommand.Parameters.AddWithValue("Soll_FW", item.Soll_FW == null ? (object)DBNull.Value : item.Soll_FW);
					sqlCommand.Parameters.AddWithValue("Text", item.Text == null ? (object)DBNull.Value : item.Text);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> items)
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
						query += " INSERT INTO [Mahnwesen_Zahlungen] ([Datum],[gebucht],[Haben_DM],[Haben_FW],[Mahn_ID],[Soll_DM],[Soll_FW],[Text]) VALUES ( "

							+ "@Datum" + i + ","
							+ "@gebucht" + i + ","
							+ "@Haben_DM" + i + ","
							+ "@Haben_FW" + i + ","
							+ "@Mahn_ID" + i + ","
							+ "@Soll_DM" + i + ","
							+ "@Soll_FW" + i + ","
							+ "@Text" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("Haben_DM" + i, item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
						sqlCommand.Parameters.AddWithValue("Haben_FW" + i, item.Haben_FW == null ? (object)DBNull.Value : item.Haben_FW);
						sqlCommand.Parameters.AddWithValue("Mahn_ID" + i, item.Mahn_ID == null ? (object)DBNull.Value : item.Mahn_ID);
						sqlCommand.Parameters.AddWithValue("Soll_DM" + i, item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
						sqlCommand.Parameters.AddWithValue("Soll_FW" + i, item.Soll_FW == null ? (object)DBNull.Value : item.Soll_FW);
						sqlCommand.Parameters.AddWithValue("Text" + i, item.Text == null ? (object)DBNull.Value : item.Text);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Mahnwesen_Zahlungen] SET [Datum]=@Datum, [gebucht]=@gebucht, [Haben_DM]=@Haben_DM, [Haben_FW]=@Haben_FW, [Mahn_ID]=@Mahn_ID, [Soll_DM]=@Soll_DM, [Soll_FW]=@Soll_FW, [Text]=@Text WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
				sqlCommand.Parameters.AddWithValue("Haben_DM", item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
				sqlCommand.Parameters.AddWithValue("Haben_FW", item.Haben_FW == null ? (object)DBNull.Value : item.Haben_FW);
				sqlCommand.Parameters.AddWithValue("Mahn_ID", item.Mahn_ID == null ? (object)DBNull.Value : item.Mahn_ID);
				sqlCommand.Parameters.AddWithValue("Soll_DM", item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
				sqlCommand.Parameters.AddWithValue("Soll_FW", item.Soll_FW == null ? (object)DBNull.Value : item.Soll_FW);
				sqlCommand.Parameters.AddWithValue("Text", item.Text == null ? (object)DBNull.Value : item.Text);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> items)
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
						query += " UPDATE [Mahnwesen_Zahlungen] SET "

							+ "[Datum]=@Datum" + i + ","
							+ "[gebucht]=@gebucht" + i + ","
							+ "[Haben_DM]=@Haben_DM" + i + ","
							+ "[Haben_FW]=@Haben_FW" + i + ","
							+ "[Mahn_ID]=@Mahn_ID" + i + ","
							+ "[Soll_DM]=@Soll_DM" + i + ","
							+ "[Soll_FW]=@Soll_FW" + i + ","
							+ "[Text]=@Text" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("Haben_DM" + i, item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
						sqlCommand.Parameters.AddWithValue("Haben_FW" + i, item.Haben_FW == null ? (object)DBNull.Value : item.Haben_FW);
						sqlCommand.Parameters.AddWithValue("Mahn_ID" + i, item.Mahn_ID == null ? (object)DBNull.Value : item.Mahn_ID);
						sqlCommand.Parameters.AddWithValue("Soll_DM" + i, item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
						sqlCommand.Parameters.AddWithValue("Soll_FW" + i, item.Soll_FW == null ? (object)DBNull.Value : item.Soll_FW);
						sqlCommand.Parameters.AddWithValue("Text" + i, item.Text == null ? (object)DBNull.Value : item.Text);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Mahnwesen_Zahlungen] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [Mahnwesen_Zahlungen] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Mahnwesen_Zahlungen] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Mahnwesen_Zahlungen]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Mahnwesen_Zahlungen] WHERE [ID] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Mahnwesen_Zahlungen] ([Datum],[gebucht],[Haben_DM],[Haben_FW],[Mahn_ID],[Soll_DM],[Soll_FW],[Text]) OUTPUT INSERTED.[ID] VALUES (@Datum,@gebucht,@Haben_DM,@Haben_FW,@Mahn_ID,@Soll_DM,@Soll_FW,@Text); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("Haben_DM", item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
			sqlCommand.Parameters.AddWithValue("Haben_FW", item.Haben_FW == null ? (object)DBNull.Value : item.Haben_FW);
			sqlCommand.Parameters.AddWithValue("Mahn_ID", item.Mahn_ID == null ? (object)DBNull.Value : item.Mahn_ID);
			sqlCommand.Parameters.AddWithValue("Soll_DM", item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
			sqlCommand.Parameters.AddWithValue("Soll_FW", item.Soll_FW == null ? (object)DBNull.Value : item.Soll_FW);
			sqlCommand.Parameters.AddWithValue("Text", item.Text == null ? (object)DBNull.Value : item.Text);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Mahnwesen_Zahlungen] ([Datum],[gebucht],[Haben_DM],[Haben_FW],[Mahn_ID],[Soll_DM],[Soll_FW],[Text]) VALUES ( "

						+ "@Datum" + i + ","
						+ "@gebucht" + i + ","
						+ "@Haben_DM" + i + ","
						+ "@Haben_FW" + i + ","
						+ "@Mahn_ID" + i + ","
						+ "@Soll_DM" + i + ","
						+ "@Soll_FW" + i + ","
						+ "@Text" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("Haben_DM" + i, item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
					sqlCommand.Parameters.AddWithValue("Haben_FW" + i, item.Haben_FW == null ? (object)DBNull.Value : item.Haben_FW);
					sqlCommand.Parameters.AddWithValue("Mahn_ID" + i, item.Mahn_ID == null ? (object)DBNull.Value : item.Mahn_ID);
					sqlCommand.Parameters.AddWithValue("Soll_DM" + i, item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
					sqlCommand.Parameters.AddWithValue("Soll_FW" + i, item.Soll_FW == null ? (object)DBNull.Value : item.Soll_FW);
					sqlCommand.Parameters.AddWithValue("Text" + i, item.Text == null ? (object)DBNull.Value : item.Text);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Mahnwesen_Zahlungen] SET [Datum]=@Datum, [gebucht]=@gebucht, [Haben_DM]=@Haben_DM, [Haben_FW]=@Haben_FW, [Mahn_ID]=@Mahn_ID, [Soll_DM]=@Soll_DM, [Soll_FW]=@Soll_FW, [Text]=@Text WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
			sqlCommand.Parameters.AddWithValue("Haben_DM", item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
			sqlCommand.Parameters.AddWithValue("Haben_FW", item.Haben_FW == null ? (object)DBNull.Value : item.Haben_FW);
			sqlCommand.Parameters.AddWithValue("Mahn_ID", item.Mahn_ID == null ? (object)DBNull.Value : item.Mahn_ID);
			sqlCommand.Parameters.AddWithValue("Soll_DM", item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
			sqlCommand.Parameters.AddWithValue("Soll_FW", item.Soll_FW == null ? (object)DBNull.Value : item.Soll_FW);
			sqlCommand.Parameters.AddWithValue("Text", item.Text == null ? (object)DBNull.Value : item.Text);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Mahnwesen_Zahlungen] SET "

					+ "[Datum]=@Datum" + i + ","
					+ "[gebucht]=@gebucht" + i + ","
					+ "[Haben_DM]=@Haben_DM" + i + ","
					+ "[Haben_FW]=@Haben_FW" + i + ","
					+ "[Mahn_ID]=@Mahn_ID" + i + ","
					+ "[Soll_DM]=@Soll_DM" + i + ","
					+ "[Soll_FW]=@Soll_FW" + i + ","
					+ "[Text]=@Text" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("Haben_DM" + i, item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
					sqlCommand.Parameters.AddWithValue("Haben_FW" + i, item.Haben_FW == null ? (object)DBNull.Value : item.Haben_FW);
					sqlCommand.Parameters.AddWithValue("Mahn_ID" + i, item.Mahn_ID == null ? (object)DBNull.Value : item.Mahn_ID);
					sqlCommand.Parameters.AddWithValue("Soll_DM" + i, item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
					sqlCommand.Parameters.AddWithValue("Soll_FW" + i, item.Soll_FW == null ? (object)DBNull.Value : item.Soll_FW);
					sqlCommand.Parameters.AddWithValue("Text" + i, item.Text == null ? (object)DBNull.Value : item.Text);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Mahnwesen_Zahlungen] WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ID", id);

			results = sqlCommand.ExecuteNonQuery();


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

				string query = "DELETE FROM [Mahnwesen_Zahlungen] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		#endregion Custom Methods

	}
}
