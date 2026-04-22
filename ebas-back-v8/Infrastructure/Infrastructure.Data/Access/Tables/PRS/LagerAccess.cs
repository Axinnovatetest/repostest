using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class LagerAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.LagerEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lager] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lager]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Lager] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.LagerEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Lager] ([AB],[Artikel-Nr],[Bestand],[Bestand_reserviert],[Bestellvorschläge],[BW],[CCID],[CCID_Datum],[Dispoformel],[Durchschnittspreis],[GesamtBestand],[Höchstbestand],[Lagerort_id],[letzte Bewegung],[Meldebestand],[Mindestbestand],[Sollbestand])  VALUES (@AB,@Artikel_Nr,@Bestand,@Bestand_reserviert,@Bestellvorschläge,@BW,@CCID,@CCID_Datum,@Dispoformel,@Durchschnittspreis,@GesamtBestand,@Höchstbestand,@Lagerort_id,@letzte_Bewegung,@Meldebestand,@Mindestbestand,@Sollbestand); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB", item.AB == null ? (object)DBNull.Value : item.AB);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
					sqlCommand.Parameters.AddWithValue("Bestand_reserviert", item.Bestand_reserviert == null ? (object)DBNull.Value : item.Bestand_reserviert);
					sqlCommand.Parameters.AddWithValue("Bestellvorschläge", item.Bestellvorschläge == null ? (object)DBNull.Value : item.Bestellvorschläge);
					sqlCommand.Parameters.AddWithValue("BW", item.BW == null ? (object)DBNull.Value : item.BW);
					sqlCommand.Parameters.AddWithValue("CCID", item.CCID == null ? (object)DBNull.Value : item.CCID);
					sqlCommand.Parameters.AddWithValue("CCID_Datum", item.CCID_Datum == null ? (object)DBNull.Value : item.CCID_Datum);
					sqlCommand.Parameters.AddWithValue("Dispoformel", item.Dispoformel == null ? (object)DBNull.Value : item.Dispoformel);
					sqlCommand.Parameters.AddWithValue("Durchschnittspreis", item.Durchschnittspreis == null ? (object)DBNull.Value : item.Durchschnittspreis);
					sqlCommand.Parameters.AddWithValue("GesamtBestand", item.GesamtBestand == null ? (object)DBNull.Value : item.GesamtBestand);
					sqlCommand.Parameters.AddWithValue("Höchstbestand", item.Höchstbestand == null ? (object)DBNull.Value : item.Höchstbestand);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("letzte_Bewegung", item.letzte_Bewegung == null ? (object)DBNull.Value : item.letzte_Bewegung);
					sqlCommand.Parameters.AddWithValue("Meldebestand", item.Meldebestand == null ? (object)DBNull.Value : item.Meldebestand);
					sqlCommand.Parameters.AddWithValue("Mindestbestand", item.Mindestbestand == null ? (object)DBNull.Value : item.Mindestbestand);
					sqlCommand.Parameters.AddWithValue("Sollbestand", item.Sollbestand == null ? (object)DBNull.Value : item.Sollbestand);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> items)
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
						query += " INSERT INTO [Lager] ([AB],[Artikel-Nr],[Bestand],[Bestand_reserviert],[Bestellvorschläge],[BW],[CCID],[CCID_Datum],[Dispoformel],[Durchschnittspreis],[GesamtBestand],[Höchstbestand],[Lagerort_id],[letzte Bewegung],[Meldebestand],[Mindestbestand],[Sollbestand]) VALUES ( "

							+ "@AB" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Bestand" + i + ","
							+ "@Bestand_reserviert" + i + ","
							+ "@Bestellvorschläge" + i + ","
							+ "@BW" + i + ","
							+ "@CCID" + i + ","
							+ "@CCID_Datum" + i + ","
							+ "@Dispoformel" + i + ","
							+ "@Durchschnittspreis" + i + ","
							+ "@GesamtBestand" + i + ","
							+ "@Höchstbestand" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@letzte_Bewegung" + i + ","
							+ "@Meldebestand" + i + ","
							+ "@Mindestbestand" + i + ","
							+ "@Sollbestand" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB" + i, item.AB == null ? (object)DBNull.Value : item.AB);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bestand" + i, item.Bestand == null ? (object)DBNull.Value : item.Bestand);
						sqlCommand.Parameters.AddWithValue("Bestand_reserviert" + i, item.Bestand_reserviert == null ? (object)DBNull.Value : item.Bestand_reserviert);
						sqlCommand.Parameters.AddWithValue("Bestellvorschläge" + i, item.Bestellvorschläge == null ? (object)DBNull.Value : item.Bestellvorschläge);
						sqlCommand.Parameters.AddWithValue("BW" + i, item.BW == null ? (object)DBNull.Value : item.BW);
						sqlCommand.Parameters.AddWithValue("CCID" + i, item.CCID == null ? (object)DBNull.Value : item.CCID);
						sqlCommand.Parameters.AddWithValue("CCID_Datum" + i, item.CCID_Datum == null ? (object)DBNull.Value : item.CCID_Datum);
						sqlCommand.Parameters.AddWithValue("Dispoformel" + i, item.Dispoformel == null ? (object)DBNull.Value : item.Dispoformel);
						sqlCommand.Parameters.AddWithValue("Durchschnittspreis" + i, item.Durchschnittspreis == null ? (object)DBNull.Value : item.Durchschnittspreis);
						sqlCommand.Parameters.AddWithValue("GesamtBestand" + i, item.GesamtBestand == null ? (object)DBNull.Value : item.GesamtBestand);
						sqlCommand.Parameters.AddWithValue("Höchstbestand" + i, item.Höchstbestand == null ? (object)DBNull.Value : item.Höchstbestand);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("letzte_Bewegung" + i, item.letzte_Bewegung == null ? (object)DBNull.Value : item.letzte_Bewegung);
						sqlCommand.Parameters.AddWithValue("Meldebestand" + i, item.Meldebestand == null ? (object)DBNull.Value : item.Meldebestand);
						sqlCommand.Parameters.AddWithValue("Mindestbestand" + i, item.Mindestbestand == null ? (object)DBNull.Value : item.Mindestbestand);
						sqlCommand.Parameters.AddWithValue("Sollbestand" + i, item.Sollbestand == null ? (object)DBNull.Value : item.Sollbestand);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.LagerEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lager] SET [AB]=@AB, [Artikel-Nr]=@Artikel_Nr, [Bestand]=@Bestand, [Bestand_reserviert]=@Bestand_reserviert, [Bestellvorschläge]=@Bestellvorschläge, [BW]=@BW, [CCID]=@CCID, [CCID_Datum]=@CCID_Datum, [Dispoformel]=@Dispoformel, [Durchschnittspreis]=@Durchschnittspreis, [GesamtBestand]=@GesamtBestand, [Höchstbestand]=@Höchstbestand, [Lagerort_id]=@Lagerort_id, [letzte Bewegung]=@letzte_Bewegung, [Meldebestand]=@Meldebestand, [Mindestbestand]=@Mindestbestand, [Sollbestand]=@Sollbestand WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("AB", item.AB == null ? (object)DBNull.Value : item.AB);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
				sqlCommand.Parameters.AddWithValue("Bestand_reserviert", item.Bestand_reserviert == null ? (object)DBNull.Value : item.Bestand_reserviert);
				sqlCommand.Parameters.AddWithValue("Bestellvorschläge", item.Bestellvorschläge == null ? (object)DBNull.Value : item.Bestellvorschläge);
				sqlCommand.Parameters.AddWithValue("BW", item.BW == null ? (object)DBNull.Value : item.BW);
				sqlCommand.Parameters.AddWithValue("CCID", item.CCID == null ? (object)DBNull.Value : item.CCID);
				sqlCommand.Parameters.AddWithValue("CCID_Datum", item.CCID_Datum == null ? (object)DBNull.Value : item.CCID_Datum);
				sqlCommand.Parameters.AddWithValue("Dispoformel", item.Dispoformel == null ? (object)DBNull.Value : item.Dispoformel);
				sqlCommand.Parameters.AddWithValue("Durchschnittspreis", item.Durchschnittspreis == null ? (object)DBNull.Value : item.Durchschnittspreis);
				sqlCommand.Parameters.AddWithValue("GesamtBestand", item.GesamtBestand == null ? (object)DBNull.Value : item.GesamtBestand);
				sqlCommand.Parameters.AddWithValue("Höchstbestand", item.Höchstbestand == null ? (object)DBNull.Value : item.Höchstbestand);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("letzte_Bewegung", item.letzte_Bewegung == null ? (object)DBNull.Value : item.letzte_Bewegung);
				sqlCommand.Parameters.AddWithValue("Meldebestand", item.Meldebestand == null ? (object)DBNull.Value : item.Meldebestand);
				sqlCommand.Parameters.AddWithValue("Mindestbestand", item.Mindestbestand == null ? (object)DBNull.Value : item.Mindestbestand);
				sqlCommand.Parameters.AddWithValue("Sollbestand", item.Sollbestand == null ? (object)DBNull.Value : item.Sollbestand);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> items)
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
						query += " UPDATE [Lager] SET "

							+ "[AB]=@AB" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Bestand]=@Bestand" + i + ","
							+ "[Bestand_reserviert]=@Bestand_reserviert" + i + ","
							+ "[Bestellvorschläge]=@Bestellvorschläge" + i + ","
							+ "[BW]=@BW" + i + ","
							+ "[CCID]=@CCID" + i + ","
							+ "[CCID_Datum]=@CCID_Datum" + i + ","
							+ "[Dispoformel]=@Dispoformel" + i + ","
							+ "[Durchschnittspreis]=@Durchschnittspreis" + i + ","
							+ "[GesamtBestand]=@GesamtBestand" + i + ","
							+ "[Höchstbestand]=@Höchstbestand" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[letzte Bewegung]=@letzte_Bewegung" + i + ","
							+ "[Meldebestand]=@Meldebestand" + i + ","
							+ "[Mindestbestand]=@Mindestbestand" + i + ","
							+ "[Sollbestand]=@Sollbestand" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("AB" + i, item.AB == null ? (object)DBNull.Value : item.AB);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bestand" + i, item.Bestand == null ? (object)DBNull.Value : item.Bestand);
						sqlCommand.Parameters.AddWithValue("Bestand_reserviert" + i, item.Bestand_reserviert == null ? (object)DBNull.Value : item.Bestand_reserviert);
						sqlCommand.Parameters.AddWithValue("Bestellvorschläge" + i, item.Bestellvorschläge == null ? (object)DBNull.Value : item.Bestellvorschläge);
						sqlCommand.Parameters.AddWithValue("BW" + i, item.BW == null ? (object)DBNull.Value : item.BW);
						sqlCommand.Parameters.AddWithValue("CCID" + i, item.CCID == null ? (object)DBNull.Value : item.CCID);
						sqlCommand.Parameters.AddWithValue("CCID_Datum" + i, item.CCID_Datum == null ? (object)DBNull.Value : item.CCID_Datum);
						sqlCommand.Parameters.AddWithValue("Dispoformel" + i, item.Dispoformel == null ? (object)DBNull.Value : item.Dispoformel);
						sqlCommand.Parameters.AddWithValue("Durchschnittspreis" + i, item.Durchschnittspreis == null ? (object)DBNull.Value : item.Durchschnittspreis);
						sqlCommand.Parameters.AddWithValue("GesamtBestand" + i, item.GesamtBestand == null ? (object)DBNull.Value : item.GesamtBestand);
						sqlCommand.Parameters.AddWithValue("Höchstbestand" + i, item.Höchstbestand == null ? (object)DBNull.Value : item.Höchstbestand);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("letzte_Bewegung" + i, item.letzte_Bewegung == null ? (object)DBNull.Value : item.letzte_Bewegung);
						sqlCommand.Parameters.AddWithValue("Meldebestand" + i, item.Meldebestand == null ? (object)DBNull.Value : item.Meldebestand);
						sqlCommand.Parameters.AddWithValue("Mindestbestand" + i, item.Mindestbestand == null ? (object)DBNull.Value : item.Mindestbestand);
						sqlCommand.Parameters.AddWithValue("Sollbestand" + i, item.Sollbestand == null ? (object)DBNull.Value : item.Sollbestand);
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
				string query = "DELETE FROM [Lager] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Lager] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Transactions

		public static Infrastructure.Data.Entities.Tables.PRS.LagerEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lager] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lager]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;
				sqlCommand.Transaction = transaction;

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [Lager] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.LagerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;

			string query = "INSERT INTO [Lager] ([AB],[Artikel-Nr],[Bestand],[Bestand_reserviert],[Bestellvorschläge],[BW],[CCID],[CCID_Datum],[Dispoformel],[Durchschnittspreis],[GesamtBestand],[Höchstbestand],[Lagerort_id],[letzte Bewegung],[Meldebestand],[Mindestbestand],[Sollbestand])  VALUES (@AB,@Artikel_Nr,@Bestand,@Bestand_reserviert,@Bestellvorschläge,@BW,@CCID,@CCID_Datum,@Dispoformel,@Durchschnittspreis,@GesamtBestand,@Höchstbestand,@Lagerort_id,@letzte_Bewegung,@Meldebestand,@Mindestbestand,@Sollbestand); ";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{

				sqlCommand.Parameters.AddWithValue("AB", item.AB == null ? (object)DBNull.Value : item.AB);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
				sqlCommand.Parameters.AddWithValue("Bestand_reserviert", item.Bestand_reserviert == null ? (object)DBNull.Value : item.Bestand_reserviert);
				sqlCommand.Parameters.AddWithValue("Bestellvorschläge", item.Bestellvorschläge == null ? (object)DBNull.Value : item.Bestellvorschläge);
				sqlCommand.Parameters.AddWithValue("BW", item.BW == null ? (object)DBNull.Value : item.BW);
				sqlCommand.Parameters.AddWithValue("CCID", item.CCID == null ? (object)DBNull.Value : item.CCID);
				sqlCommand.Parameters.AddWithValue("CCID_Datum", item.CCID_Datum == null ? (object)DBNull.Value : item.CCID_Datum);
				sqlCommand.Parameters.AddWithValue("Dispoformel", item.Dispoformel == null ? (object)DBNull.Value : item.Dispoformel);
				sqlCommand.Parameters.AddWithValue("Durchschnittspreis", item.Durchschnittspreis == null ? (object)DBNull.Value : item.Durchschnittspreis);
				sqlCommand.Parameters.AddWithValue("GesamtBestand", item.GesamtBestand == null ? (object)DBNull.Value : item.GesamtBestand);
				sqlCommand.Parameters.AddWithValue("Höchstbestand", item.Höchstbestand == null ? (object)DBNull.Value : item.Höchstbestand);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("letzte_Bewegung", item.letzte_Bewegung == null ? (object)DBNull.Value : item.letzte_Bewegung);
				sqlCommand.Parameters.AddWithValue("Meldebestand", item.Meldebestand == null ? (object)DBNull.Value : item.Meldebestand);
				sqlCommand.Parameters.AddWithValue("Mindestbestand", item.Mindestbestand == null ? (object)DBNull.Value : item.Mindestbestand);
				sqlCommand.Parameters.AddWithValue("Sollbestand", item.Sollbestand == null ? (object)DBNull.Value : item.Sollbestand);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Lager] ([AB],[Artikel-Nr],[Bestand],[Bestand_reserviert],[Bestellvorschläge],[BW],[CCID],[CCID_Datum],[Dispoformel],[Durchschnittspreis],[GesamtBestand],[Höchstbestand],[Lagerort_id],[letzte Bewegung],[Meldebestand],[Mindestbestand],[Sollbestand]) VALUES ( "

						+ "@AB" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Bestand" + i + ","
						+ "@Bestand_reserviert" + i + ","
						+ "@Bestellvorschläge" + i + ","
						+ "@BW" + i + ","
						+ "@CCID" + i + ","
						+ "@CCID_Datum" + i + ","
						+ "@Dispoformel" + i + ","
						+ "@Durchschnittspreis" + i + ","
						+ "@GesamtBestand" + i + ","
						+ "@Höchstbestand" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@letzte_Bewegung" + i + ","
						+ "@Meldebestand" + i + ","
						+ "@Mindestbestand" + i + ","
						+ "@Sollbestand" + i
						+ "); ";


					sqlCommand.Parameters.AddWithValue("AB" + i, item.AB == null ? (object)DBNull.Value : item.AB);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bestand" + i, item.Bestand == null ? (object)DBNull.Value : item.Bestand);
					sqlCommand.Parameters.AddWithValue("Bestand_reserviert" + i, item.Bestand_reserviert == null ? (object)DBNull.Value : item.Bestand_reserviert);
					sqlCommand.Parameters.AddWithValue("Bestellvorschläge" + i, item.Bestellvorschläge == null ? (object)DBNull.Value : item.Bestellvorschläge);
					sqlCommand.Parameters.AddWithValue("BW" + i, item.BW == null ? (object)DBNull.Value : item.BW);
					sqlCommand.Parameters.AddWithValue("CCID" + i, item.CCID == null ? (object)DBNull.Value : item.CCID);
					sqlCommand.Parameters.AddWithValue("CCID_Datum" + i, item.CCID_Datum == null ? (object)DBNull.Value : item.CCID_Datum);
					sqlCommand.Parameters.AddWithValue("Dispoformel" + i, item.Dispoformel == null ? (object)DBNull.Value : item.Dispoformel);
					sqlCommand.Parameters.AddWithValue("Durchschnittspreis" + i, item.Durchschnittspreis == null ? (object)DBNull.Value : item.Durchschnittspreis);
					sqlCommand.Parameters.AddWithValue("GesamtBestand" + i, item.GesamtBestand == null ? (object)DBNull.Value : item.GesamtBestand);
					sqlCommand.Parameters.AddWithValue("Höchstbestand" + i, item.Höchstbestand == null ? (object)DBNull.Value : item.Höchstbestand);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("letzte_Bewegung" + i, item.letzte_Bewegung == null ? (object)DBNull.Value : item.letzte_Bewegung);
					sqlCommand.Parameters.AddWithValue("Meldebestand" + i, item.Meldebestand == null ? (object)DBNull.Value : item.Meldebestand);
					sqlCommand.Parameters.AddWithValue("Mindestbestand" + i, item.Mindestbestand == null ? (object)DBNull.Value : item.Mindestbestand);
					sqlCommand.Parameters.AddWithValue("Sollbestand" + i, item.Sollbestand == null ? (object)DBNull.Value : item.Sollbestand);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.LagerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Lager] SET [AB]=@AB, [Artikel-Nr]=@Artikel_Nr, [Bestand]=@Bestand, [Bestand_reserviert]=@Bestand_reserviert, [Bestellvorschläge]=@Bestellvorschläge, [BW]=@BW, [CCID]=@CCID, [CCID_Datum]=@CCID_Datum, [Dispoformel]=@Dispoformel, [Durchschnittspreis]=@Durchschnittspreis, [GesamtBestand]=@GesamtBestand, [Höchstbestand]=@Höchstbestand, [Lagerort_id]=@Lagerort_id, [letzte Bewegung]=@letzte_Bewegung, [Meldebestand]=@Meldebestand, [Mindestbestand]=@Mindestbestand, [Sollbestand]=@Sollbestand WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("AB", item.AB == null ? (object)DBNull.Value : item.AB);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
			sqlCommand.Parameters.AddWithValue("Bestand_reserviert", item.Bestand_reserviert == null ? (object)DBNull.Value : item.Bestand_reserviert);
			sqlCommand.Parameters.AddWithValue("Bestellvorschläge", item.Bestellvorschläge == null ? (object)DBNull.Value : item.Bestellvorschläge);
			sqlCommand.Parameters.AddWithValue("BW", item.BW == null ? (object)DBNull.Value : item.BW);
			sqlCommand.Parameters.AddWithValue("CCID", item.CCID == null ? (object)DBNull.Value : item.CCID);
			sqlCommand.Parameters.AddWithValue("CCID_Datum", item.CCID_Datum == null ? (object)DBNull.Value : item.CCID_Datum);
			sqlCommand.Parameters.AddWithValue("Dispoformel", item.Dispoformel == null ? (object)DBNull.Value : item.Dispoformel);
			sqlCommand.Parameters.AddWithValue("Durchschnittspreis", item.Durchschnittspreis == null ? (object)DBNull.Value : item.Durchschnittspreis);
			sqlCommand.Parameters.AddWithValue("GesamtBestand", item.GesamtBestand == null ? (object)DBNull.Value : item.GesamtBestand);
			sqlCommand.Parameters.AddWithValue("Höchstbestand", item.Höchstbestand == null ? (object)DBNull.Value : item.Höchstbestand);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("letzte_Bewegung", item.letzte_Bewegung == null ? (object)DBNull.Value : item.letzte_Bewegung);
			sqlCommand.Parameters.AddWithValue("Meldebestand", item.Meldebestand == null ? (object)DBNull.Value : item.Meldebestand);
			sqlCommand.Parameters.AddWithValue("Mindestbestand", item.Mindestbestand == null ? (object)DBNull.Value : item.Mindestbestand);
			sqlCommand.Parameters.AddWithValue("Sollbestand", item.Sollbestand == null ? (object)DBNull.Value : item.Sollbestand);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Lager] SET "

						+ "[AB]=@AB" + i + ","
						+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
						+ "[Bestand]=@Bestand" + i + ","
						+ "[Bestand_reserviert]=@Bestand_reserviert" + i + ","
						+ "[Bestellvorschläge]=@Bestellvorschläge" + i + ","
						+ "[BW]=@BW" + i + ","
						+ "[CCID]=@CCID" + i + ","
						+ "[CCID_Datum]=@CCID_Datum" + i + ","
						+ "[Dispoformel]=@Dispoformel" + i + ","
						+ "[Durchschnittspreis]=@Durchschnittspreis" + i + ","
						+ "[GesamtBestand]=@GesamtBestand" + i + ","
						+ "[Höchstbestand]=@Höchstbestand" + i + ","
						+ "[Lagerort_id]=@Lagerort_id" + i + ","
						+ "[letzte Bewegung]=@letzte_Bewegung" + i + ","
						+ "[Meldebestand]=@Meldebestand" + i + ","
						+ "[Mindestbestand]=@Mindestbestand" + i + ","
						+ "[Sollbestand]=@Sollbestand" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("AB" + i, item.AB == null ? (object)DBNull.Value : item.AB);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bestand" + i, item.Bestand == null ? (object)DBNull.Value : item.Bestand);
					sqlCommand.Parameters.AddWithValue("Bestand_reserviert" + i, item.Bestand_reserviert == null ? (object)DBNull.Value : item.Bestand_reserviert);
					sqlCommand.Parameters.AddWithValue("Bestellvorschläge" + i, item.Bestellvorschläge == null ? (object)DBNull.Value : item.Bestellvorschläge);
					sqlCommand.Parameters.AddWithValue("BW" + i, item.BW == null ? (object)DBNull.Value : item.BW);
					sqlCommand.Parameters.AddWithValue("CCID" + i, item.CCID == null ? (object)DBNull.Value : item.CCID);
					sqlCommand.Parameters.AddWithValue("CCID_Datum" + i, item.CCID_Datum == null ? (object)DBNull.Value : item.CCID_Datum);
					sqlCommand.Parameters.AddWithValue("Dispoformel" + i, item.Dispoformel == null ? (object)DBNull.Value : item.Dispoformel);
					sqlCommand.Parameters.AddWithValue("Durchschnittspreis" + i, item.Durchschnittspreis == null ? (object)DBNull.Value : item.Durchschnittspreis);
					sqlCommand.Parameters.AddWithValue("GesamtBestand" + i, item.GesamtBestand == null ? (object)DBNull.Value : item.GesamtBestand);
					sqlCommand.Parameters.AddWithValue("Höchstbestand" + i, item.Höchstbestand == null ? (object)DBNull.Value : item.Höchstbestand);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("letzte_Bewegung" + i, item.letzte_Bewegung == null ? (object)DBNull.Value : item.letzte_Bewegung);
					sqlCommand.Parameters.AddWithValue("Meldebestand" + i, item.Meldebestand == null ? (object)DBNull.Value : item.Meldebestand);
					sqlCommand.Parameters.AddWithValue("Mindestbestand" + i, item.Mindestbestand == null ? (object)DBNull.Value : item.Mindestbestand);
					sqlCommand.Parameters.AddWithValue("Sollbestand" + i, item.Sollbestand == null ? (object)DBNull.Value : item.Sollbestand);
				}

				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);
				return results;
			}

			return -1;
		}
		#endregion Transactions
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.PRS.LagerEntity GetByIdAndArticleNr(int id, int articleNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lager] WHERE [ID]=@Id AND [Artikel_Nr]=@artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("artikel_Nr", articleNr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> GetByArticleNr(int articleId, bool? notEmpty = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lager] WHERE [Artikel-Nr]=@articleId {(notEmpty.HasValue && notEmpty.Value ? " AND Bestand > 0" : "")} ORDER BY Bestand DESC";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> GetByArticleNrs(List<int> articleNrs, bool? notEmpty = null)
		{
			if(articleNrs == null || articleNrs.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lager] WHERE [Artikel-Nr] IN ({string.Join(",", articleNrs)}) {(notEmpty.HasValue && notEmpty.Value ? " AND Bestand > 0" : "")} ORDER BY Bestand DESC";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> GetByArticles(List<int> lagerIds, List<int> articleNrs, bool? notEmpty = null)
		{
			if(articleNrs == null || articleNrs.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lager] WHERE [Artikel-Nr] IN ({string.Join(",", articleNrs)}) {(notEmpty.HasValue && notEmpty.Value ? " AND Bestand > 0" : "")} AND Lagerort_id IN ({string.Join(",", lagerIds)}) ORDER BY Bestand DESC";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
			}
		}
		//
		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> GetByArticleNrAndBestand(int articleNr, bool includeMinStock)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lager] WHERE [Artikel-Nr]= @articleNr AND (Bestand <> 0 {(includeMinStock ? "OR  (Bestand=0 AND Mindestbestand>0)" : "")})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> GetByArticleNrAndBestand(int articleNr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			{
				string query = $"SELECT * FROM [Lager] WHERE [Artikel-Nr]= @articleNr AND Bestand <> 0";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity> GetStandardByArticleAndId(int articleNr, List<int> lagerortIds)
		{
			if(lagerortIds == null || lagerortIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lager] WHERE [Artikel-Nr]=@articleNr AND Lagerort_id IN ({string.Join(",", lagerortIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.LagerEntity GetStandardByArticleAndId(int articleNr, int? lagerortId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Lager] WHERE [Artikel-Nr]=@articleNr {(lagerortId.HasValue ? $"AND Lagerort_id={lagerortId.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.LagerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateMinStock(int Id, decimal? newMinStock)
		{
			int results = -1;
			if(!newMinStock.HasValue)
				return results;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lager] SET [Mindestbestand]=@Mindestbestand WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", Id);
				sqlCommand.Parameters.AddWithValue("Mindestbestand", newMinStock);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateStock(int Id, decimal newStock)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lager] SET [bestand]=@bestand WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", Id);
				sqlCommand.Parameters.AddWithValue("bestand", newStock);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int UpdateCCID(int Id, bool newCCID, DateTime? newCCIDDate)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lager] SET [CCID]=@ccid,[CCID_Datum]=@ccidDate WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", Id);
				sqlCommand.Parameters.AddWithValue("ccid", newCCID);
				sqlCommand.Parameters.AddWithValue("ccidDate", newCCIDDate);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateProposal(int Id, bool newProposal)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lager] SET [Bestellvorschläge]=@newValue WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", Id);
				sqlCommand.Parameters.AddWithValue("newValue", newProposal);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<KeyValuePair<int, decimal>> GetTotalByArticleNrs(List<int> articleNrs)
		{
			if(articleNrs == null || articleNrs.Count <= 0)
				return new List<KeyValuePair<int, decimal>>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Artikel-Nr], SUM(Bestand) Bestand  FROM [Lager] WHERE [Artikel-Nr] IN ({string.Join(",", articleNrs)}) GROUP BY [Artikel-Nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(int.TryParse(x[0].ToString(), out var _id) ? _id : 0, decimal.TryParse(x[1].ToString(), out var _bestand) ? _bestand : 0)).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, decimal>>();
			}
		}
		public static List<Tuple<int, decimal, decimal>> GetTotalAndSecurityStockByArticleNrs(List<int> articleNrs)
		{
			if(articleNrs == null || articleNrs.Count <= 0)
				return new List<Tuple<int, decimal, decimal>>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Artikel-Nr], SUM(Bestand) Bestand, SUM(Mindestbestand) Mindestbestand  FROM [Lager] WHERE [Artikel-Nr] IN ({string.Join(",", articleNrs)}) GROUP BY [Artikel-Nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Tuple<int, decimal, decimal>(int.TryParse(
					 x[0].ToString(), out var _id) ? _id : 0,
					 decimal.TryParse(x[1].ToString(), out var _bestand) ? _bestand : 0,
					 decimal.TryParse(x[2].ToString(), out var _mbestand) ? _mbestand : 0)).ToList();
			}
			else
			{
				return new List<Tuple<int, decimal, decimal>>();
			}
		}
		public static Tuple<int, decimal, decimal> GetTotalAndSecurityStockByArticleNrs(int articleNr, List<int> lagerortIds)
		{
			if(articleNr <= 0)
				return new Tuple<int, decimal, decimal>(articleNr, 0, 0);

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Artikel-Nr], SUM(Bestand) Bestand, SUM(Mindestbestand) Mindestbestand  FROM [Lager] WHERE [Artikel-Nr]={articleNr}{(lagerortIds?.Count > 0 ? $" AND [Lagerort_id] IN ({string.Join(",", lagerortIds)})" : "")} GROUP BY [Artikel-Nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return new Tuple<int, decimal, decimal>(int.TryParse(
					 dataTable.Rows[0][0].ToString(), out var _id) ? _id : 0,
					 decimal.TryParse(dataTable.Rows[0][1].ToString(), out var _bestand) ? _bestand : 0,
					 decimal.TryParse(dataTable.Rows[0][2].ToString(), out var _mbestand) ? _mbestand : 0);
			}
			else
			{
				return new Tuple<int, decimal, decimal>(articleNr, 0, 0);
			}
		}
		public static List<Tuple<int, decimal, decimal>> GetTotalAndSecurityStockByArticleNrs(List<int> articleNrs, List<int> lagerortIds)
		{
			if(articleNrs == null || articleNrs.Count <= 0)
				return new List<Tuple<int, decimal, decimal>>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Artikel-Nr], SUM(Bestand) Bestand, SUM(Mindestbestand) Mindestbestand  FROM [Lager] WHERE [Artikel-Nr] IN ({string.Join(",", articleNrs)}){(lagerortIds?.Count > 0 ? $" AND [Lagerort_id] IN ({string.Join(",", lagerortIds)})" : "")} GROUP BY [Artikel-Nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Tuple<int, decimal, decimal>(int.TryParse(
					 x[0].ToString(), out var _id) ? _id : 0,
					 decimal.TryParse(x[1].ToString(), out var _bestand) ? _bestand : 0,
					 decimal.TryParse(x[2].ToString(), out var _mbestand) ? _mbestand : 0)).ToList();
			}
			else
			{
				return new List<Tuple<int, decimal, decimal>>();
			}
		}
		public static int updateLagerMindestbestandByLagerId(int lagerId, int neuMinbestand, int ArticleNr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lager] SET  [Mindestbestand]=@Mindestbestand WHERE [Lagerort_id]=@ID AND [Artikel-Nr]=@articleNR";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", lagerId);
				sqlCommand.Parameters.AddWithValue("Mindestbestand", neuMinbestand);
				sqlCommand.Parameters.AddWithValue("articleNR", ArticleNr);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static Entities.Tables.PRS.LagerEntity GetByLagerortIdAndArtikelNR(int lagerortId, string artikelNummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lager] WHERE [Lagerort_id]=@Id  AND [Artikel-Nr]=(select [Artikel-Nr] from  [Artikel] where [Artikelnummer]=@artikelNummer)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", lagerortId);
				sqlCommand.Parameters.AddWithValue("artikelNummer", artikelNummer);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.LagerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static IEnumerable<Tuple<int, int, decimal>> UpdateLagerMindestbestand(IEnumerable<Tuple<int, int, decimal>> data, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(data == null || !data.Any())
				return Enumerable.Empty<Tuple<int, int, decimal>>();

			var results = new List<Tuple<int, int, decimal>>();


			// Prepare temp table
			using(var sqlCommand = new SqlCommand(@"
					CREATE TABLE #Updates (
						Lagerort_id INT,
						ArtikelNr INT,
						NewMindestbestand DECIMAL(18,2)
					)", sqlConnection, sqlTransaction))
			{
				DbExecution.ExecuteNonQuery(sqlCommand);
			}

			// Bulk insert into temp table
			using(var bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction))
			{
				bulkCopy.DestinationTableName = "#Updates";

				var table = new DataTable();
				table.Columns.Add("Lagerort_id", typeof(int));
				table.Columns.Add("ArtikelNr", typeof(int));
				table.Columns.Add("NewMindestbestand", typeof(decimal));

				foreach(var item in data)
					table.Rows.Add(item.Item1, item.Item2, item.Item3);

				bulkCopy.WriteToServer(table);
			}

			// Run update and capture old values
			using(var updateCmd = new SqlCommand(@"
							UPDATE L
							SET [Mindestbestand] = U.NewMindestbestand
							OUTPUT
								DELETED.Lagerort_id,
								DELETED.[Artikel-Nr],
								CAST(DELETED.Mindestbestand AS DECIMAL(18,2))
							FROM Lager L
							INNER JOIN #Updates U 
								ON L.Lagerort_id = U.Lagerort_id 
								AND L.[Artikel-Nr] = U.ArtikelNr
						", sqlConnection, sqlTransaction))
			{
				using(var reader = DbExecution.ExecuteReader(updateCmd))
				{
					while(reader.Read())
					{
						results.Add(new Tuple<int, int, decimal>
						(
							reader.GetInt32(0),
							 reader.GetInt32(1),
							reader.GetDecimal(2)
						));
					}
				}
			}

			return results;
		}
		#endregion

	}
}
