using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Mahnwesen_ArchivAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Mahnwesen_Archiv] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Mahnwesen_Archiv]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Mahnwesen_Archiv] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Mahnwesen_Archiv] ([Adress_id],[Belegdatum],[Belegnummer],[Datum],[Haben_DM],[Projekt-Nr],[Soll_DM],[Zahlungsfrist]) OUTPUT INSERTED.[ID] VALUES (@Adress_id,@Belegdatum,@Belegnummer,@Datum,@Haben_DM,@Projekt_Nr,@Soll_DM,@Zahlungsfrist); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Adress_id", item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
					sqlCommand.Parameters.AddWithValue("Belegdatum", item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
					sqlCommand.Parameters.AddWithValue("Belegnummer", item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Haben_DM", item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Soll_DM", item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
					sqlCommand.Parameters.AddWithValue("Zahlungsfrist", item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> items)
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
						query += " INSERT INTO [Mahnwesen_Archiv] ([Adress_id],[Belegdatum],[Belegnummer],[Datum],[Haben_DM],[Projekt-Nr],[Soll_DM],[Zahlungsfrist]) VALUES ( "

							+ "@Adress_id" + i + ","
							+ "@Belegdatum" + i + ","
							+ "@Belegnummer" + i + ","
							+ "@Datum" + i + ","
							+ "@Haben_DM" + i + ","
							+ "@Projekt_Nr" + i + ","
							+ "@Soll_DM" + i + ","
							+ "@Zahlungsfrist" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Adress_id" + i, item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
						sqlCommand.Parameters.AddWithValue("Belegdatum" + i, item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
						sqlCommand.Parameters.AddWithValue("Belegnummer" + i, item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Haben_DM" + i, item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Soll_DM" + i, item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
						sqlCommand.Parameters.AddWithValue("Zahlungsfrist" + i, item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Mahnwesen_Archiv] SET [Adress_id]=@Adress_id, [Belegdatum]=@Belegdatum, [Belegnummer]=@Belegnummer, [Datum]=@Datum, [Haben_DM]=@Haben_DM, [Projekt-Nr]=@Projekt_Nr, [Soll_DM]=@Soll_DM, [Zahlungsfrist]=@Zahlungsfrist WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Adress_id", item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
				sqlCommand.Parameters.AddWithValue("Belegdatum", item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
				sqlCommand.Parameters.AddWithValue("Belegnummer", item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Haben_DM", item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("Soll_DM", item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
				sqlCommand.Parameters.AddWithValue("Zahlungsfrist", item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> items)
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
						query += " UPDATE [Mahnwesen_Archiv] SET "

							+ "[Adress_id]=@Adress_id" + i + ","
							+ "[Belegdatum]=@Belegdatum" + i + ","
							+ "[Belegnummer]=@Belegnummer" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Haben_DM]=@Haben_DM" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[Soll_DM]=@Soll_DM" + i + ","
							+ "[Zahlungsfrist]=@Zahlungsfrist" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Adress_id" + i, item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
						sqlCommand.Parameters.AddWithValue("Belegdatum" + i, item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
						sqlCommand.Parameters.AddWithValue("Belegnummer" + i, item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Haben_DM" + i, item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Soll_DM" + i, item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
						sqlCommand.Parameters.AddWithValue("Zahlungsfrist" + i, item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);
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
				string query = "DELETE FROM [Mahnwesen_Archiv] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Mahnwesen_Archiv] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Mahnwesen_Archiv] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Mahnwesen_Archiv]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Mahnwesen_Archiv] WHERE [ID] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Mahnwesen_Archiv] ([Adress_id],[Belegdatum],[Belegnummer],[Datum],[Haben_DM],[Projekt-Nr],[Soll_DM],[Zahlungsfrist]) OUTPUT INSERTED.[ID] VALUES (@Adress_id,@Belegdatum,@Belegnummer,@Datum,@Haben_DM,@Projekt_Nr,@Soll_DM,@Zahlungsfrist); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Adress_id", item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
			sqlCommand.Parameters.AddWithValue("Belegdatum", item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
			sqlCommand.Parameters.AddWithValue("Belegnummer", item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Haben_DM", item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Soll_DM", item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
			sqlCommand.Parameters.AddWithValue("Zahlungsfrist", item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Mahnwesen_Archiv] ([Adress_id],[Belegdatum],[Belegnummer],[Datum],[Haben_DM],[Projekt-Nr],[Soll_DM],[Zahlungsfrist]) VALUES ( "

						+ "@Adress_id" + i + ","
						+ "@Belegdatum" + i + ","
						+ "@Belegnummer" + i + ","
						+ "@Datum" + i + ","
						+ "@Haben_DM" + i + ","
						+ "@Projekt_Nr" + i + ","
						+ "@Soll_DM" + i + ","
						+ "@Zahlungsfrist" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Adress_id" + i, item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
					sqlCommand.Parameters.AddWithValue("Belegdatum" + i, item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
					sqlCommand.Parameters.AddWithValue("Belegnummer" + i, item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Haben_DM" + i, item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Soll_DM" + i, item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
					sqlCommand.Parameters.AddWithValue("Zahlungsfrist" + i, item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Mahnwesen_Archiv] SET [Adress_id]=@Adress_id, [Belegdatum]=@Belegdatum, [Belegnummer]=@Belegnummer, [Datum]=@Datum, [Haben_DM]=@Haben_DM, [Projekt-Nr]=@Projekt_Nr, [Soll_DM]=@Soll_DM, [Zahlungsfrist]=@Zahlungsfrist WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Adress_id", item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
			sqlCommand.Parameters.AddWithValue("Belegdatum", item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
			sqlCommand.Parameters.AddWithValue("Belegnummer", item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Haben_DM", item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Soll_DM", item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
			sqlCommand.Parameters.AddWithValue("Zahlungsfrist", item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Mahnwesen_Archiv] SET "

					+ "[Adress_id]=@Adress_id" + i + ","
					+ "[Belegdatum]=@Belegdatum" + i + ","
					+ "[Belegnummer]=@Belegnummer" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[Haben_DM]=@Haben_DM" + i + ","
					+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
					+ "[Soll_DM]=@Soll_DM" + i + ","
					+ "[Zahlungsfrist]=@Zahlungsfrist" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Adress_id" + i, item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
					sqlCommand.Parameters.AddWithValue("Belegdatum" + i, item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
					sqlCommand.Parameters.AddWithValue("Belegnummer" + i, item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Haben_DM" + i, item.Haben_DM == null ? (object)DBNull.Value : item.Haben_DM);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Soll_DM" + i, item.Soll_DM == null ? (object)DBNull.Value : item.Soll_DM);
					sqlCommand.Parameters.AddWithValue("Zahlungsfrist" + i, item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Mahnwesen_Archiv] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [Mahnwesen_Archiv] WHERE [ID] IN (" + queryIds + ")";
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
