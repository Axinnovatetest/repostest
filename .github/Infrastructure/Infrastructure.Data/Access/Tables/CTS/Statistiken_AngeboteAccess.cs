using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Statistiken_AngeboteAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Statistiken Angebote] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Statistiken Angebote]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Statistiken Angebote] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Statistiken Angebote] ([Adreß-Nr],[Angebot_Nr],[Anzahl],[Artikel-Nr],[Artikel-Nr_nach],[Bestellung_Nr],[Datum],[fibu_export],[Gesamtpreis],[Lagerort_ID],[Lagerort_ID_nach],[Liefertermin],[Mandant],[NR-Angebotene_Artikel],[Personal-Nr],[Projekt-Nr],[Typ],[USt]) OUTPUT INSERTED.[ID] VALUES (@Adress_Nr,@Angebot_Nr,@Anzahl,@Artikel_Nr,@Artikel_Nr_nach,@Bestellung_Nr,@Datum,@fibu_export,@Gesamtpreis,@Lagerort_ID,@Lagerort_ID_nach,@Liefertermin,@Mandant,@NR_Angebotene_Artikel,@Personal_Nr,@Projekt_Nr,@Typ,@USt); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Adreß_Nr", item.Adress_Nr == null ? (object)DBNull.Value : item.Adress_Nr);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_nach", item.Artikel_Nr_nach == null ? (object)DBNull.Value : item.Artikel_Nr_nach);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("fibu_export", item.fibu_export == null ? (object)DBNull.Value : item.fibu_export);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID_nach", item.Lagerort_ID_nach == null ? (object)DBNull.Value : item.Lagerort_ID_nach);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("NR_Angebotene_Artikel", item.NR_Angebotene_Artikel == null ? (object)DBNull.Value : item.NR_Angebotene_Artikel);
					sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 20; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> items)
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
						query += " INSERT INTO [Statistiken Angebote] ([Adreß-Nr],[Angebot_Nr],[Anzahl],[Artikel-Nr],[Artikel-Nr_nach],[Bestellung_Nr],[Datum],[fibu_export],[Gesamtpreis],[Lagerort_ID],[Lagerort_ID_nach],[Liefertermin],[Mandant],[NR-Angebotene_Artikel],[Personal-Nr],[Projekt-Nr],[Typ],[USt]) VALUES ( "

							+ "@Adress_Nr" + i + ","
							+ "@Angebot_Nr" + i + ","
							+ "@Anzahl" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Artikel_Nr_nach" + i + ","
							+ "@Bestellung_Nr" + i + ","
							+ "@Datum" + i + ","
							+ "@fibu_export" + i + ","
							+ "@Gesamtpreis" + i + ","
							+ "@Lagerort_ID" + i + ","
							+ "@Lagerort_ID_nach" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@Mandant" + i + ","
							+ "@NR_Angebotene_Artikel" + i + ","
							+ "@Personal_Nr" + i + ","
							+ "@Projekt_Nr" + i + ","
							+ "@Typ" + i + ","
							+ "@USt" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Adreß_Nr" + i, item.Adress_Nr == null ? (object)DBNull.Value : item.Adress_Nr);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_nach" + i, item.Artikel_Nr_nach == null ? (object)DBNull.Value : item.Artikel_Nr_nach);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("fibu_export" + i, item.fibu_export == null ? (object)DBNull.Value : item.fibu_export);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID_nach" + i, item.Lagerort_ID_nach == null ? (object)DBNull.Value : item.Lagerort_ID_nach);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("NR_Angebotene_Artikel" + i, item.NR_Angebotene_Artikel == null ? (object)DBNull.Value : item.NR_Angebotene_Artikel);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Statistiken Angebote] SET [Adreß-Nr]=@Adress_Nr, [Angebot_Nr]=@Angebot_Nr, [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Artikel-Nr_nach]=@Artikel_Nr_nach, [Bestellung_Nr]=@Bestellung_Nr, [Datum]=@Datum, [fibu_export]=@fibu_export, [Gesamtpreis]=@Gesamtpreis, [Lagerort_ID]=@Lagerort_ID, [Lagerort_ID_nach]=@Lagerort_ID_nach, [Liefertermin]=@Liefertermin, [Mandant]=@Mandant, [NR-Angebotene_Artikel]=@NR_Angebotene_Artikel, [Personal-Nr]=@Personal_Nr, [Projekt-Nr]=@Projekt_Nr, [Typ]=@Typ, [USt]=@USt WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Adreß_Nr", item.Adress_Nr == null ? (object)DBNull.Value : item.Adress_Nr);
				sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr_nach", item.Artikel_Nr_nach == null ? (object)DBNull.Value : item.Artikel_Nr_nach);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("fibu_export", item.fibu_export == null ? (object)DBNull.Value : item.fibu_export);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
				sqlCommand.Parameters.AddWithValue("Lagerort_ID_nach", item.Lagerort_ID_nach == null ? (object)DBNull.Value : item.Lagerort_ID_nach);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
				sqlCommand.Parameters.AddWithValue("NR_Angebotene_Artikel", item.NR_Angebotene_Artikel == null ? (object)DBNull.Value : item.NR_Angebotene_Artikel);
				sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 20; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> items)
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
						query += " UPDATE [Statistiken Angebote] SET "

							+ "[Adreß-Nr]=@Adress_Nr" + i + ","
							+ "[Angebot_Nr]=@Angebot_Nr" + i + ","
							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Artikel-Nr_nach]=@Artikel_Nr_nach" + i + ","
							+ "[Bestellung_Nr]=@Bestellung_Nr" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[fibu_export]=@fibu_export" + i + ","
							+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
							+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
							+ "[Lagerort_ID_nach]=@Lagerort_ID_nach" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[Mandant]=@Mandant" + i + ","
							+ "[NR-Angebotene_Artikel]=@NR_Angebotene_Artikel" + i + ","
							+ "[Personal-Nr]=@Personal_Nr" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[Typ]=@Typ" + i + ","
							+ "[USt]=@USt" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Adreß_Nr" + i, item.Adress_Nr == null ? (object)DBNull.Value : item.Adress_Nr);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_nach" + i, item.Artikel_Nr_nach == null ? (object)DBNull.Value : item.Artikel_Nr_nach);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("fibu_export" + i, item.fibu_export == null ? (object)DBNull.Value : item.fibu_export);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID_nach" + i, item.Lagerort_ID_nach == null ? (object)DBNull.Value : item.Lagerort_ID_nach);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("NR_Angebotene_Artikel" + i, item.NR_Angebotene_Artikel == null ? (object)DBNull.Value : item.NR_Angebotene_Artikel);
						sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
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
				string query = "DELETE FROM [Statistiken Angebote] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Statistiken Angebote] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Statistiken Angebote] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Statistiken Angebote]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Statistiken Angebote] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Statistiken Angebote] ([Adreß-Nr],[Angebot_Nr],[Anzahl],[Artikel-Nr],[Artikel-Nr_nach],[Bestellung_Nr],[Datum],[fibu_export],[Gesamtpreis],[Lagerort_ID],[Lagerort_ID_nach],[Liefertermin],[Mandant],[NR-Angebotene_Artikel],[Personal-Nr],[Projekt-Nr],[Typ],[USt]) OUTPUT INSERTED.[ID] VALUES (@Adress_Nr,@Angebot_Nr,@Anzahl,@Artikel_Nr,@Artikel_Nr_nach,@Bestellung_Nr,@Datum,@fibu_export,@Gesamtpreis,@Lagerort_ID,@Lagerort_ID_nach,@Liefertermin,@Mandant,@NR_Angebotene_Artikel,@Personal_Nr,@Projekt_Nr,@Typ,@USt); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Adreß_Nr", item.Adress_Nr == null ? (object)DBNull.Value : item.Adress_Nr);
			sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr_nach", item.Artikel_Nr_nach == null ? (object)DBNull.Value : item.Artikel_Nr_nach);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("fibu_export", item.fibu_export == null ? (object)DBNull.Value : item.fibu_export);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
			sqlCommand.Parameters.AddWithValue("Lagerort_ID_nach", item.Lagerort_ID_nach == null ? (object)DBNull.Value : item.Lagerort_ID_nach);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("NR_Angebotene_Artikel", item.NR_Angebotene_Artikel == null ? (object)DBNull.Value : item.NR_Angebotene_Artikel);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 20; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Statistiken Angebote] ([Adreß-Nr],[Angebot_Nr],[Anzahl],[Artikel-Nr],[Artikel-Nr_nach],[Bestellung_Nr],[Datum],[fibu_export],[Gesamtpreis],[Lagerort_ID],[Lagerort_ID_nach],[Liefertermin],[Mandant],[NR-Angebotene_Artikel],[Personal-Nr],[Projekt-Nr],[Typ],[USt]) VALUES ( "

						+ "@Adress_Nr" + i + ","
						+ "@Angebot_Nr" + i + ","
						+ "@Anzahl" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Artikel_Nr_nach" + i + ","
						+ "@Bestellung_Nr" + i + ","
						+ "@Datum" + i + ","
						+ "@fibu_export" + i + ","
						+ "@Gesamtpreis" + i + ","
						+ "@Lagerort_ID" + i + ","
						+ "@Lagerort_ID_nach" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@Mandant" + i + ","
						+ "@NR_Angebotene_Artikel" + i + ","
						+ "@Personal_Nr" + i + ","
						+ "@Projekt_Nr" + i + ","
						+ "@Typ" + i + ","
						+ "@USt" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Adress_Nr" + i, item.Adress_Nr == null ? (object)DBNull.Value : item.Adress_Nr);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_nach" + i, item.Artikel_Nr_nach == null ? (object)DBNull.Value : item.Artikel_Nr_nach);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("fibu_export" + i, item.fibu_export == null ? (object)DBNull.Value : item.fibu_export);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID_nach" + i, item.Lagerort_ID_nach == null ? (object)DBNull.Value : item.Lagerort_ID_nach);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("NR_Angebotene_Artikel" + i, item.NR_Angebotene_Artikel == null ? (object)DBNull.Value : item.NR_Angebotene_Artikel);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Statistiken Angebote] SET [Adreß-Nr]=@Adress_Nr, [Angebot_Nr]=@Angebot_Nr, [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Artikel-Nr_nach]=@Artikel_Nr_nach, [Bestellung_Nr]=@Bestellung_Nr, [Datum]=@Datum, [fibu_export]=@fibu_export, [Gesamtpreis]=@Gesamtpreis, [Lagerort_ID]=@Lagerort_ID, [Lagerort_ID_nach]=@Lagerort_ID_nach, [Liefertermin]=@Liefertermin, [Mandant]=@Mandant, [NR-Angebotene_Artikel]=@NR_Angebotene_Artikel, [Personal-Nr]=@Personal_Nr, [Projekt-Nr]=@Projekt_Nr, [Typ]=@Typ, [USt]=@USt WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Adress_Nr", item.Adress_Nr == null ? (object)DBNull.Value : item.Adress_Nr);
			sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr_nach", item.Artikel_Nr_nach == null ? (object)DBNull.Value : item.Artikel_Nr_nach);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("fibu_export", item.fibu_export == null ? (object)DBNull.Value : item.fibu_export);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
			sqlCommand.Parameters.AddWithValue("Lagerort_ID_nach", item.Lagerort_ID_nach == null ? (object)DBNull.Value : item.Lagerort_ID_nach);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("NR_Angebotene_Artikel", item.NR_Angebotene_Artikel == null ? (object)DBNull.Value : item.NR_Angebotene_Artikel);
			sqlCommand.Parameters.AddWithValue("Personal_Nr", item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 20; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Statistiken Angebote] SET "

					+ "[Adreß-Nr]=@Adress_Nr" + i + ","
					+ "[Angebot_Nr]=@Angebot_Nr" + i + ","
					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
					+ "[Artikel-Nr_nach]=@Artikel_Nr_nach" + i + ","
					+ "[Bestellung_Nr]=@Bestellung_Nr" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[fibu_export]=@fibu_export" + i + ","
					+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
					+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
					+ "[Lagerort_ID_nach]=@Lagerort_ID_nach" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[Mandant]=@Mandant" + i + ","
					+ "[NR-Angebotene_Artikel]=@NR_Angebotene_Artikel" + i + ","
					+ "[Personal-Nr]=@Personal_Nr" + i + ","
					+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
					+ "[Typ]=@Typ" + i + ","
					+ "[USt]=@USt" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Adress_Nr" + i, item.Adress_Nr == null ? (object)DBNull.Value : item.Adress_Nr);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_nach" + i, item.Artikel_Nr_nach == null ? (object)DBNull.Value : item.Artikel_Nr_nach);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("fibu_export" + i, item.fibu_export == null ? (object)DBNull.Value : item.fibu_export);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID_nach" + i, item.Lagerort_ID_nach == null ? (object)DBNull.Value : item.Lagerort_ID_nach);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("NR_Angebotene_Artikel" + i, item.NR_Angebotene_Artikel == null ? (object)DBNull.Value : item.NR_Angebotene_Artikel);
					sqlCommand.Parameters.AddWithValue("Personal_Nr" + i, item.Personal_Nr == null ? (object)DBNull.Value : item.Personal_Nr);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Statistiken Angebote] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [Statistiken Angebote] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


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
