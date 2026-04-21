using Infrastructure.Data.Entities.Joins.CTS;
using Infrastructure.Data.Entities.Joins.Logistics;
using System.Buffers;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class PSZ_Fertigungsauftrag_ÄnderungshistorieAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Fertigungsauftrag Änderungshistorie]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Fertigungsauftrag Änderungshistorie] ([Änderungsdatum],[Angebot-Nr],[Artikelnummer],[Bemerkung],[Bezeichnung],[CS_Mitarbeiter],[Erstmuster],[FA_Menge],[Fertigungsnummer],[Grund_CS],[Kapazitätsproblem],[Kapazitätsproblematik],[Lagerort_id],[Materialproblem],[Materialproblematik],[Mitarbeiter],[Sonstige_Problematik],[Sonstiges],[Termin_Angebot],[Termin_Bestätigt1],[Termin_voränderung],[Termin_Wunsch],[Ursprünglicher_termin],[Werkzeugproblem],[Werkzeugproblematik],[Wunsch_CS]) OUTPUT INSERTED.[ID] VALUES (@Anderungsdatum,@Angebot_Nr,@Artikelnummer,@Bemerkung,@Bezeichnung,@CS_Mitarbeiter,@Erstmuster,@FA_Menge,@Fertigungsnummer,@Grund_CS,@Kapazitatsproblem,@Kapazitatsproblematik,@Lagerort_id,@Materialproblem,@Materialproblematik,@Mitarbeiter,@Sonstige_Problematik,@Sonstiges,@Termin_Angebot,@Termin_Bestatigt1,@Termin_voranderung,@Termin_Wunsch,@Ursprunglicher_termin,@Werkzeugproblem,@Werkzeugproblematik,@Wunsch_CS); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anderungsdatum", item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter", item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
					sqlCommand.Parameters.AddWithValue("FA_Menge", item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Grund_CS", item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
					sqlCommand.Parameters.AddWithValue("Kapazitatsproblem", item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
					sqlCommand.Parameters.AddWithValue("Kapazitatsproblematik", item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Materialproblem", item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
					sqlCommand.Parameters.AddWithValue("Materialproblematik", item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Sonstige_Problematik", item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
					sqlCommand.Parameters.AddWithValue("Sonstiges", item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
					sqlCommand.Parameters.AddWithValue("Termin_Angebot", item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
					sqlCommand.Parameters.AddWithValue("Termin_voranderung", item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
					sqlCommand.Parameters.AddWithValue("Termin_Wunsch", item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
					sqlCommand.Parameters.AddWithValue("Ursprunglicher_termin", item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
					sqlCommand.Parameters.AddWithValue("Werkzeugproblem", item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
					sqlCommand.Parameters.AddWithValue("Werkzeugproblematik", item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
					sqlCommand.Parameters.AddWithValue("Wunsch_CS", item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> items)
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
						query += " INSERT INTO [PSZ_Fertigungsauftrag Änderungshistorie] ([Änderungsdatum],[Angebot-Nr],[Artikelnummer],[Bemerkung],[Bezeichnung],[CS_Mitarbeiter],[Erstmuster],[FA_Menge],[Fertigungsnummer],[Grund_CS],[Kapazitätsproblem],[Kapazitätsproblematik],[Lagerort_id],[Materialproblem],[Materialproblematik],[Mitarbeiter],[Sonstige_Problematik],[Sonstiges],[Termin_Angebot],[Termin_Bestätigt1],[Termin_voränderung],[Termin_Wunsch],[Ursprünglicher_termin],[Werkzeugproblem],[Werkzeugproblematik],[Wunsch_CS]) VALUES ( "

							+ "@Anderungsdatum" + i + ","
							+ "@Angebot_Nr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@Bezeichnung" + i + ","
							+ "@CS_Mitarbeiter" + i + ","
							+ "@Erstmuster" + i + ","
							+ "@FA_Menge" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@Grund_CS" + i + ","
							+ "@Kapazitatsproblem" + i + ","
							+ "@Kapazitatsproblematik" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Materialproblem" + i + ","
							+ "@Materialproblematik" + i + ","
							+ "@Mitarbeiter" + i + ","
							+ "@Sonstige_Problematik" + i + ","
							+ "@Sonstiges" + i + ","
							+ "@Termin_Angebot" + i + ","
							+ "@Termin_Bestatigt1" + i + ","
							+ "@Termin_voranderung" + i + ","
							+ "@Termin_Wunsch" + i + ","
							+ "@Ursprunglicher_termin" + i + ","
							+ "@Werkzeugproblem" + i + ","
							+ "@Werkzeugproblematik" + i + ","
							+ "@Wunsch_CS" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anderungsdatum" + i, item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter" + i, item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
						sqlCommand.Parameters.AddWithValue("FA_Menge" + i, item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Grund_CS" + i, item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
						sqlCommand.Parameters.AddWithValue("Kapazitatsproblem" + i, item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
						sqlCommand.Parameters.AddWithValue("Kapazitatsproblematik" + i, item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Materialproblem" + i, item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
						sqlCommand.Parameters.AddWithValue("Materialproblematik" + i, item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Sonstige_Problematik" + i, item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
						sqlCommand.Parameters.AddWithValue("Sonstiges" + i, item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
						sqlCommand.Parameters.AddWithValue("Termin_Angebot" + i, item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
						sqlCommand.Parameters.AddWithValue("Termin_voranderung" + i, item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
						sqlCommand.Parameters.AddWithValue("Termin_Wunsch" + i, item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
						sqlCommand.Parameters.AddWithValue("Ursprunglicher_termin" + i, item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
						sqlCommand.Parameters.AddWithValue("Werkzeugproblem" + i, item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
						sqlCommand.Parameters.AddWithValue("Werkzeugproblematik" + i, item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
						sqlCommand.Parameters.AddWithValue("Wunsch_CS" + i, item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Fertigungsauftrag Änderungshistorie] SET [Änderungsdatum]=@Anderungsdatum, [Angebot-Nr]=@Angebot_Nr, [Artikelnummer]=@Artikelnummer, [Bemerkung]=@Bemerkung, [Bezeichnung]=@Bezeichnung, [CS_Mitarbeiter]=@CS_Mitarbeiter, [Erstmuster]=@Erstmuster, [FA_Menge]=@FA_Menge, [Fertigungsnummer]=@Fertigungsnummer, [Grund_CS]=@Grund_CS, [Kapazitätsproblem]=@Kapazitatsproblem, [Kapazitätsproblematik]=@Kapazitatsproblematik, [Lagerort_id]=@Lagerort_id, [Materialproblem]=@Materialproblem, [Materialproblematik]=@Materialproblematik, [Mitarbeiter]=@Mitarbeiter, [Sonstige_Problematik]=@Sonstige_Problematik, [Sonstiges]=@Sonstiges, [Termin_Angebot]=@Termin_Angebot, [Termin_Bestätigt1]=@Termin_Bestatigt1, [Termin_voränderung]=@Termin_voranderung, [Termin_Wunsch]=@Termin_Wunsch, [Ursprünglicher_termin]=@Ursprunglicher_termin, [Werkzeugproblem]=@Werkzeugproblem, [Werkzeugproblematik]=@Werkzeugproblematik, [Wunsch_CS]=@Wunsch_CS WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Anderungsdatum", item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
				sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
				sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter", item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
				sqlCommand.Parameters.AddWithValue("FA_Menge", item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Grund_CS", item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
				sqlCommand.Parameters.AddWithValue("Kapazitatsproblem", item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
				sqlCommand.Parameters.AddWithValue("Kapazitatsproblematik", item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Materialproblem", item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
				sqlCommand.Parameters.AddWithValue("Materialproblematik", item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
				sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("Sonstige_Problematik", item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
				sqlCommand.Parameters.AddWithValue("Sonstiges", item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
				sqlCommand.Parameters.AddWithValue("Termin_Angebot", item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
				sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
				sqlCommand.Parameters.AddWithValue("Termin_voranderung", item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
				sqlCommand.Parameters.AddWithValue("Termin_Wunsch", item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
				sqlCommand.Parameters.AddWithValue("Ursprunglicher_termin", item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
				sqlCommand.Parameters.AddWithValue("Werkzeugproblem", item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
				sqlCommand.Parameters.AddWithValue("Werkzeugproblematik", item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
				sqlCommand.Parameters.AddWithValue("Wunsch_CS", item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> items)
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
						query += " UPDATE [PSZ_Fertigungsauftrag Änderungshistorie] SET "

							+ "[Änderungsdatum]=@Anderungsdatum" + i + ","
							+ "[Angebot-Nr]=@Angebot_Nr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Bezeichnung]=@Bezeichnung" + i + ","
							+ "[CS_Mitarbeiter]=@CS_Mitarbeiter" + i + ","
							+ "[Erstmuster]=@Erstmuster" + i + ","
							+ "[FA_Menge]=@FA_Menge" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[Grund_CS]=@Grund_CS" + i + ","
							+ "[Kapazitätsproblem]=@Kapazitatsproblem" + i + ","
							+ "[Kapazitätsproblematik]=@Kapazitatsproblematik" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Materialproblem]=@Materialproblem" + i + ","
							+ "[Materialproblematik]=@Materialproblematik" + i + ","
							+ "[Mitarbeiter]=@Mitarbeiter" + i + ","
							+ "[Sonstige_Problematik]=@Sonstige_Problematik" + i + ","
							+ "[Sonstiges]=@Sonstiges" + i + ","
							+ "[Termin_Angebot]=@Termin_Angebot" + i + ","
							+ "[Termin_Bestätigt1]=@Termin_Bestatigt1" + i + ","
							+ "[Termin_voränderung]=@Termin_voranderung" + i + ","
							+ "[Termin_Wunsch]=@Termin_Wunsch" + i + ","
							+ "[Ursprünglicher_termin]=@Ursprunglicher_termin" + i + ","
							+ "[Werkzeugproblem]=@Werkzeugproblem" + i + ","
							+ "[Werkzeugproblematik]=@Werkzeugproblematik" + i + ","
							+ "[Wunsch_CS]=@Wunsch_CS" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Anderungsdatum" + i, item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter" + i, item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
						sqlCommand.Parameters.AddWithValue("FA_Menge" + i, item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Grund_CS" + i, item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
						sqlCommand.Parameters.AddWithValue("Kapazitatsproblem" + i, item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
						sqlCommand.Parameters.AddWithValue("Kapazitatsproblematik" + i, item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Materialproblem" + i, item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
						sqlCommand.Parameters.AddWithValue("Materialproblematik" + i, item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Sonstige_Problematik" + i, item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
						sqlCommand.Parameters.AddWithValue("Sonstiges" + i, item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
						sqlCommand.Parameters.AddWithValue("Termin_Angebot" + i, item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
						sqlCommand.Parameters.AddWithValue("Termin_voranderung" + i, item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
						sqlCommand.Parameters.AddWithValue("Termin_Wunsch" + i, item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
						sqlCommand.Parameters.AddWithValue("Ursprunglicher_termin" + i, item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
						sqlCommand.Parameters.AddWithValue("Werkzeugproblem" + i, item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
						sqlCommand.Parameters.AddWithValue("Werkzeugproblematik" + i, item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
						sqlCommand.Parameters.AddWithValue("Wunsch_CS" + i, item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);
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
				string query = "DELETE FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Fertigungsauftrag Änderungshistorie]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [PSZ_Fertigungsauftrag Änderungshistorie] ([Änderungsdatum],[Angebot-Nr],[Artikelnummer],[Bemerkung],[Bezeichnung],[CS_Mitarbeiter],[Erstmuster],[FA_Menge],[Fertigungsnummer],[Grund_CS],[Kapazitätsproblem],[Kapazitätsproblematik],[Lagerort_id],[Materialproblem],[Materialproblematik],[Mitarbeiter],[Sonstige_Problematik],[Sonstiges],[Termin_Angebot],[Termin_Bestätigt1],[Termin_voränderung],[Termin_Wunsch],[Ursprünglicher_termin],[Werkzeugproblem],[Werkzeugproblematik],[Wunsch_CS]) OUTPUT INSERTED.[ID] VALUES (@Anderungsdatum,@Angebot_Nr,@Artikelnummer,@Bemerkung,@Bezeichnung,@CS_Mitarbeiter,@Erstmuster,@FA_Menge,@Fertigungsnummer,@Grund_CS,@Kapazitatsproblem,@Kapazitatsproblematik,@Lagerort_id,@Materialproblem,@Materialproblematik,@Mitarbeiter,@Sonstige_Problematik,@Sonstiges,@Termin_Angebot,@Termin_Bestatigt1,@Termin_voranderung,@Termin_Wunsch,@Ursprunglicher_termin,@Werkzeugproblem,@Werkzeugproblematik,@Wunsch_CS); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Anderungsdatum", item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
			sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
			sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter", item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
			sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
			sqlCommand.Parameters.AddWithValue("FA_Menge", item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Grund_CS", item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
			sqlCommand.Parameters.AddWithValue("Kapazitatsproblem", item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
			sqlCommand.Parameters.AddWithValue("Kapazitatsproblematik", item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Materialproblem", item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
			sqlCommand.Parameters.AddWithValue("Materialproblematik", item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
			sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
			sqlCommand.Parameters.AddWithValue("Sonstige_Problematik", item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
			sqlCommand.Parameters.AddWithValue("Sonstiges", item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
			sqlCommand.Parameters.AddWithValue("Termin_Angebot", item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
			sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
			sqlCommand.Parameters.AddWithValue("Termin_voranderung", item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
			sqlCommand.Parameters.AddWithValue("Termin_Wunsch", item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
			sqlCommand.Parameters.AddWithValue("Ursprunglicher_termin", item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
			sqlCommand.Parameters.AddWithValue("Werkzeugproblem", item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
			sqlCommand.Parameters.AddWithValue("Werkzeugproblematik", item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
			sqlCommand.Parameters.AddWithValue("Wunsch_CS", item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [PSZ_Fertigungsauftrag Änderungshistorie] ([Änderungsdatum],[Angebot-Nr],[Artikelnummer],[Bemerkung],[Bezeichnung],[CS_Mitarbeiter],[Erstmuster],[FA_Menge],[Fertigungsnummer],[Grund_CS],[Kapazitätsproblem],[Kapazitätsproblematik],[Lagerort_id],[Materialproblem],[Materialproblematik],[Mitarbeiter],[Sonstige_Problematik],[Sonstiges],[Termin_Angebot],[Termin_Bestätigt1],[Termin_voränderung],[Termin_Wunsch],[Ursprünglicher_termin],[Werkzeugproblem],[Werkzeugproblematik],[Wunsch_CS]) VALUES ( "

						+ "@Anderungsdatum" + i + ","
						+ "@Angebot_Nr" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@Bezeichnung" + i + ","
						+ "@CS_Mitarbeiter" + i + ","
						+ "@Erstmuster" + i + ","
						+ "@FA_Menge" + i + ","
						+ "@Fertigungsnummer" + i + ","
						+ "@Grund_CS" + i + ","
						+ "@Kapazitatsproblem" + i + ","
						+ "@Kapazitatsproblematik" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@Materialproblem" + i + ","
						+ "@Materialproblematik" + i + ","
						+ "@Mitarbeiter" + i + ","
						+ "@Sonstige_Problematik" + i + ","
						+ "@Sonstiges" + i + ","
						+ "@Termin_Angebot" + i + ","
						+ "@Termin_Bestatigt1" + i + ","
						+ "@Termin_voranderung" + i + ","
						+ "@Termin_Wunsch" + i + ","
						+ "@Ursprunglicher_termin" + i + ","
						+ "@Werkzeugproblem" + i + ","
						+ "@Werkzeugproblematik" + i + ","
						+ "@Wunsch_CS" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Anderungsdatum" + i, item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter" + i, item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
					sqlCommand.Parameters.AddWithValue("FA_Menge" + i, item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Grund_CS" + i, item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
					sqlCommand.Parameters.AddWithValue("Kapazitatsproblem" + i, item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
					sqlCommand.Parameters.AddWithValue("Kapazitatsproblematik" + i, item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Materialproblem" + i, item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
					sqlCommand.Parameters.AddWithValue("Materialproblematik" + i, item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Sonstige_Problematik" + i, item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
					sqlCommand.Parameters.AddWithValue("Sonstiges" + i, item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
					sqlCommand.Parameters.AddWithValue("Termin_Angebot" + i, item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
					sqlCommand.Parameters.AddWithValue("Termin_voranderung" + i, item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
					sqlCommand.Parameters.AddWithValue("Termin_Wunsch" + i, item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
					sqlCommand.Parameters.AddWithValue("Ursprunglicher_termin" + i, item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
					sqlCommand.Parameters.AddWithValue("Werkzeugproblem" + i, item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
					sqlCommand.Parameters.AddWithValue("Werkzeugproblematik" + i, item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
					sqlCommand.Parameters.AddWithValue("Wunsch_CS" + i, item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [PSZ_Fertigungsauftrag Änderungshistorie] SET [Änderungsdatum]=@Anderungsdatum, [Angebot-Nr]=@Angebot_Nr, [Artikelnummer]=@Artikelnummer, [Bemerkung]=@Bemerkung, [Bezeichnung]=@Bezeichnung, [CS_Mitarbeiter]=@CS_Mitarbeiter, [Erstmuster]=@Erstmuster, [FA_Menge]=@FA_Menge, [Fertigungsnummer]=@Fertigungsnummer, [Grund_CS]=@Grund_CS, [Kapazitätsproblem]=@Kapazitatsproblem, [Kapazitätsproblematik]=@Kapazitatsproblematik, [Lagerort_id]=@Lagerort_id, [Materialproblem]=@Materialproblem, [Materialproblematik]=@Materialproblematik, [Mitarbeiter]=@Mitarbeiter, [Sonstige_Problematik]=@Sonstige_Problematik, [Sonstiges]=@Sonstiges, [Termin_Angebot]=@Termin_Angebot, [Termin_Bestätigt1]=@Termin_Bestatigt1, [Termin_voränderung]=@Termin_voranderung, [Termin_Wunsch]=@Termin_Wunsch, [Ursprünglicher_termin]=@Ursprunglicher_termin, [Werkzeugproblem]=@Werkzeugproblem, [Werkzeugproblematik]=@Werkzeugproblematik, [Wunsch_CS]=@Wunsch_CS WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Anderungsdatum", item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
			sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
			sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter", item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
			sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
			sqlCommand.Parameters.AddWithValue("FA_Menge", item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Grund_CS", item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
			sqlCommand.Parameters.AddWithValue("Kapazitatsproblem", item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
			sqlCommand.Parameters.AddWithValue("Kapazitatsproblematik", item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Materialproblem", item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
			sqlCommand.Parameters.AddWithValue("Materialproblematik", item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
			sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
			sqlCommand.Parameters.AddWithValue("Sonstige_Problematik", item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
			sqlCommand.Parameters.AddWithValue("Sonstiges", item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
			sqlCommand.Parameters.AddWithValue("Termin_Angebot", item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
			sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
			sqlCommand.Parameters.AddWithValue("Termin_voranderung", item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
			sqlCommand.Parameters.AddWithValue("Termin_Wunsch", item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
			sqlCommand.Parameters.AddWithValue("Ursprunglicher_termin", item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
			sqlCommand.Parameters.AddWithValue("Werkzeugproblem", item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
			sqlCommand.Parameters.AddWithValue("Werkzeugproblematik", item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
			sqlCommand.Parameters.AddWithValue("Wunsch_CS", item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PSZ_Fertigungsauftrag Änderungshistorie] SET "

					+ "[Änderungsdatum]=@Anderungsdatum" + i + ","
					+ "[Angebot-Nr]=@Angebot_Nr" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Bezeichnung]=@Bezeichnung" + i + ","
					+ "[CS_Mitarbeiter]=@CS_Mitarbeiter" + i + ","
					+ "[Erstmuster]=@Erstmuster" + i + ","
					+ "[FA_Menge]=@FA_Menge" + i + ","
					+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
					+ "[Grund_CS]=@Grund_CS" + i + ","
					+ "[Kapazitätsproblem]=@Kapazitatsproblem" + i + ","
					+ "[Kapazitätsproblematik]=@Kapazitatsproblematik" + i + ","
					+ "[Lagerort_id]=@Lagerort_id" + i + ","
					+ "[Materialproblem]=@Materialproblem" + i + ","
					+ "[Materialproblematik]=@Materialproblematik" + i + ","
					+ "[Mitarbeiter]=@Mitarbeiter" + i + ","
					+ "[Sonstige_Problematik]=@Sonstige_Problematik" + i + ","
					+ "[Sonstiges]=@Sonstiges" + i + ","
					+ "[Termin_Angebot]=@Termin_Angebot" + i + ","
					+ "[Termin_Bestätigt1]=@Termin_Bestatigt1" + i + ","
					+ "[Termin_voränderung]=@Termin_voranderung" + i + ","
					+ "[Termin_Wunsch]=@Termin_Wunsch" + i + ","
					+ "[Ursprünglicher_termin]=@Ursprunglicher_termin" + i + ","
					+ "[Werkzeugproblem]=@Werkzeugproblem" + i + ","
					+ "[Werkzeugproblematik]=@Werkzeugproblematik" + i + ","
					+ "[Wunsch_CS]=@Wunsch_CS" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Anderungsdatum" + i, item.Änderungsdatum == null ? (object)DBNull.Value : item.Änderungsdatum);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter" + i, item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
					sqlCommand.Parameters.AddWithValue("FA_Menge" + i, item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Grund_CS" + i, item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
					sqlCommand.Parameters.AddWithValue("Kapazitatsproblem" + i, item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
					sqlCommand.Parameters.AddWithValue("Kapazitatsproblematik" + i, item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Materialproblem" + i, item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
					sqlCommand.Parameters.AddWithValue("Materialproblematik" + i, item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Sonstige_Problematik" + i, item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
					sqlCommand.Parameters.AddWithValue("Sonstiges" + i, item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
					sqlCommand.Parameters.AddWithValue("Termin_Angebot" + i, item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
					sqlCommand.Parameters.AddWithValue("Termin_voranderung" + i, item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
					sqlCommand.Parameters.AddWithValue("Termin_Wunsch" + i, item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
					sqlCommand.Parameters.AddWithValue("Ursprunglicher_termin" + i, item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
					sqlCommand.Parameters.AddWithValue("Werkzeugproblem" + i, item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
					sqlCommand.Parameters.AddWithValue("Werkzeugproblematik" + i, item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
					sqlCommand.Parameters.AddWithValue("Wunsch_CS" + i, item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction

		#endregion

		#region Custom Methods

		public static List<int> GetFAsOnly()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT [Fertigungsnummer] FROM [PSZ_Fertigungsauftrag Änderungshistorie]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x["Fertigungsnummer"])).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> GetByFA(int fa)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Fertigungsauftrag Änderungshistorie] WHERE [Fertigungsnummer]=@fa";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fa", fa);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
			}
		}

		public static List<FertigungAuftragChangeEntity> GetByHorizon(
			string SearchValue,
			bool broughtIntoH1,
			bool sendOutOfH1,
			bool includeDelayBacklog,
			int lengthInDays,
			DateTime? from,
			DateTime? to,
			string faStatus,
			bool FullData,
			int? lagerortId = null,
			Settings.PaginModel paging = null,
			Settings.SortingModel sorting = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"DECLARE @H1LengthInDays INT = {lengthInDays};" +
					"SELECT fa.ID as [Id], Abs(datediff(DAY,fa.Termin_voränderung,fa.Termin_Bestätigt1)) as [DiffInDays] ,"
								+ "fa.Änderungsdatum as [Änderungsdatum],"
								+ "(fa.FA_Menge * f.Zeit) / 60 as [HoursLeft],"
								+ "fa.Fertigungsnummer as [Fertigungsnummer],"
								+ "fa.Artikelnummer as [Artikelnummer],"
								+ "f.Artikel_Nr as [ArticleNr],"
								+ "CAST(fa.Bemerkung AS NVARCHAR(MAX))  as [Bemerkung],"
								+ "fa.Bezeichnung as [Bezeichnung],"
								+ "fa.CS_Mitarbeiter as [CS_Mitarbeiter],"
								+ "fa.Erstmuster as [Erstmuster],"
								+ "fa.FA_Menge as [FA_Menge],"
								+ "fa.Grund_CS as [Grund_CS],"
								+ "fa.Mitarbeiter as [Mitarbeiter],"
								+ "fa.Termin_Bestätigt1 as [Termin_Bestätigt1],"
								+ "fa.Termin_voränderung as [Termin_voränderung],"
								+ "fa.Termin_Wunsch as [Termin_Wunsch],"
								+ "fa.Wunsch_CS as [Wunsch_CS],"
								+ "l.Lagerort as [Lager],"
								+ "f.Kennzeichen as [FaStatus],"
							+ " CASE WHEN abs(datediff(day,fa.Änderungsdatum,fa.Termin_voränderung)) > @H1LengthInDays and abs(datediff(DAY,fa.Termin_Bestätigt1,fa.Änderungsdatum)) <= @H1LengthInDays THEN 1"
	+ " WHEN abs(datediff(day,fa.Änderungsdatum,fa.Termin_voränderung)) <= @H1LengthInDays and abs(datediff(DAY,fa.Termin_Bestätigt1,fa.Änderungsdatum)) > @H1LengthInDays THEN 2"
	+ " ELSE 0 END AS [FaPositionZone]"
					+ "FROM [PSZ_Fertigungsauftrag Änderungshistorie] as fa   inner join [Fertigung] f on fa.Fertigungsnummer = f.Fertigungsnummer"
					+ " inner join [Lagerorte] l on  fa.Lagerort_id = l.Lagerort_id";

				var clauses = new List<string>();
				bool otherFiltersAdded = false;

				if(broughtIntoH1)
				{
					clauses.Add("abs(datediff(day,fa.Änderungsdatum,fa.Termin_voränderung)) > @H1LengthInDays and abs(datediff(DAY,fa.Termin_Bestätigt1,fa.Änderungsdatum)) <= @H1LengthInDays");
					otherFiltersAdded = true;
				}

				if(sendOutOfH1)
				{
					clauses.Add("abs(datediff(day,fa.Änderungsdatum,fa.Termin_voränderung)) <= @H1LengthInDays and abs(datediff(DAY,fa.Termin_Bestätigt1,fa.Änderungsdatum)) > @H1LengthInDays");
					otherFiltersAdded = true;
				}

				if(includeDelayBacklog)
				{
					clauses.Add("fa.Termin_voränderung > fa.Änderungsdatum");
					otherFiltersAdded = true;
				}

				if(!string.IsNullOrEmpty(SearchValue))
				{
					clauses.Add($"fa.Fertigungsnummer LIKE '{SearchValue}%'");
					otherFiltersAdded = true;
				}

				if(lagerortId is not null)
				{
					clauses.Add($"fa.Lagerort_id = {lagerortId}");
					otherFiltersAdded = true;
				}

				if(!string.IsNullOrWhiteSpace(faStatus))
				{
					clauses.Add($"LOWER(f.Kennzeichen) = '{faStatus.ToLower()}'");
					otherFiltersAdded = true;
				}

				if(from is not null)
				{
					clauses.Add($"CONVERT(date, fa.Änderungsdatum) >= CONVERT(date, '{from:yyyy-MM-dd}')");
					otherFiltersAdded = true;
				}

				if(to is not null)
				{
					clauses.Add($"CONVERT(date, fa.Änderungsdatum) <= CONVERT(date, '{to:yyyy-MM-dd}')");
					otherFiltersAdded = true;
				}

				if(FullData && !otherFiltersAdded)
				{
					string today = DateTime.Today.ToString("yyyy-MM-dd");
					string past360Days = DateTime.Today.AddDays(-360).ToString("yyyy-MM-dd");
					clauses.Add($"CONVERT(date, fa.Änderungsdatum) BETWEEN CONVERT(date, '{past360Days}') AND CONVERT(date, '{today}')");
				}
				if(clauses.Count > 0)
				{
					query += " WHERE " + string.Join(" AND ", clauses);
				}

				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY fa.Änderungsdatum DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				#endregion pagination sorting


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandText = query;
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandTimeout = 240; //in seconds
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new FertigungAuftragChangeEntity(x)).ToList();
			}
			else
			{
				return new List<FertigungAuftragChangeEntity>();
			}
		}
		public static int GetByHorizon_Count(string SearchValue,
			bool broughtIntoH1,
			bool sendOutOfH1,
			bool includeDelayBacklog,
			int lengthInDays,
			DateTime? from,
			DateTime? to,
			string faStatus,
			bool FullData,
			int? lagerortId = null)
		{

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"DECLARE @H1LengthInDays INT = {lengthInDays};" +
							"SELECT COUNT(*) FROM [PSZ_Fertigungsauftrag Änderungshistorie] as fa inner join [Fertigung] f on fa.Fertigungsnummer = f.Fertigungsnummer"
									+ " inner join [Lagerorte] l on  fa.Lagerort_id = l.Lagerort_id";

				var clauses = new List<string>();
				bool otherFiltersAdded = false;

				if(broughtIntoH1)
				{
					clauses.Add("abs(datediff(day,fa.Änderungsdatum,fa.Termin_voränderung)) > @H1LengthInDays and abs(datediff(DAY,fa.Termin_Bestätigt1,fa.Änderungsdatum)) <= @H1LengthInDays");
					otherFiltersAdded = true;
				}

				if(sendOutOfH1)
				{
					clauses.Add("abs(datediff(day,fa.Änderungsdatum,fa.Termin_voränderung)) <= @H1LengthInDays and abs(datediff(DAY,fa.Termin_Bestätigt1,fa.Änderungsdatum)) > @H1LengthInDays");
					otherFiltersAdded = true;
				}

				if(includeDelayBacklog)
				{
					clauses.Add("fa.Termin_voränderung > fa.Änderungsdatum");
					otherFiltersAdded = true;
				}

				if(!string.IsNullOrEmpty(SearchValue))
				{
					clauses.Add($"fa.Fertigungsnummer LIKE '{SearchValue}%'");
					otherFiltersAdded = true;
				}

				if(lagerortId is not null)
				{
					clauses.Add($"fa.Lagerort_id = {lagerortId}");
					otherFiltersAdded = true;
				}

				if(!string.IsNullOrWhiteSpace(faStatus))
				{
					clauses.Add($"LOWER(f.Kennzeichen) = '{faStatus.ToLower()}'");
					otherFiltersAdded = true;
				}

				if(from is not null)
				{
					clauses.Add($"CONVERT(date, fa.Änderungsdatum) >= CONVERT(date, '{from:yyyy-MM-dd}')");
					otherFiltersAdded = true;
				}

				if(to is not null)
				{
					clauses.Add($"CONVERT(date, fa.Änderungsdatum) <= CONVERT(date, '{to:yyyy-MM-dd}')");
					otherFiltersAdded = true;
				}

				if(FullData && !otherFiltersAdded)
				{
					string today = DateTime.Today.ToString("yyyy-MM-dd");
					string past360Days = DateTime.Today.AddDays(-360).ToString("yyyy-MM-dd");
					clauses.Add($"CONVERT(date, fa.Änderungsdatum) BETWEEN CONVERT(date, '{past360Days}') AND CONVERT(date, '{today}')");
				}
				if(clauses.Count > 0)
				{
					query += " WHERE " + string.Join(" AND ", clauses);
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static List<FAHoursChangesEntity> GetFaHoursByWeekAndLager(List<int> kws,
			int? lagerort, int? year, int lengthInDays, int? faPositionZone = null, Settings.PaginModel paging = null,
			Settings.SortingModel sorting = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var clauses = new List<string>();
				string query = $"DECLARE @H1LengthInDays INT = {lengthInDays}; WITH PreparedData AS( SELECT (h.FA_Menge * f.Zeit) / 60 as [HoursLeft],l.Lagerort AS Lager,"
							+ " DATEPART(ISO_WEEK, h.Änderungsdatum) AS KW,DATEPART(YEAR, h.Änderungsdatum) AS KW_YEAR,h.Lagerort_id as LagerId,"
							+ " CASE WHEN abs(datediff(day,h.Änderungsdatum,h.Termin_voränderung)) > @H1LengthInDays and abs(datediff(DAY,h.Termin_Bestätigt1,h.Änderungsdatum)) <= @H1LengthInDays THEN 1"
	+ " WHEN abs(datediff(day,h.Änderungsdatum,h.Termin_voränderung)) <= @H1LengthInDays and abs(datediff(DAY,h.Termin_Bestätigt1,h.Änderungsdatum)) > @H1LengthInDays THEN 2"
	+ " ELSE 0 END AS [FaPositionZone]"
  +" FROM [PSZ_Fertigungsauftrag Änderungshistorie] h INNER JOIN[Fertigung] f ON h.Fertigungsnummer = f.Fertigungsnummer INNER JOIN[Lagerorte] l ON l.Lagerort_id = h.Lagerort_id) "
+ " SELECT [HoursLeft],Lager,KW,KW_YEAR,FaPositionZone,COUNT(*) OVER() AS TotalCount,LagerId FROM PreparedData";
	
				#region where columns

				if(lagerort.Value != 0)
				{
					clauses.Add($"LagerId={lagerort.Value}");
				}

				if(kws.Count > 0)
				{
					clauses.Add($"KW IN ({string.Join(',', kws)})");

				}
				if(year.HasValue)

				{
					clauses.Add($"KW_YEAR = {year}");

				}
		
				if(faPositionZone is not null)
				{
					clauses.Add($" FaPositionZone = {faPositionZone}");
				}

				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}

				#endregion


				query += " GROUP BY Lager,KW,KW_YEAR,FaPositionZone,LagerId,HoursLeft";


				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY KW_YEAR DESC,KW DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				else
				{
					query += $" OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY ";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				#endregion pagination sorting
					sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new FAHoursChangesEntity(x)).ToList();
			}
			else
			{
				return new List<FAHoursChangesEntity>();
			}
		}
		
		public static List<FaChartDataEntity> GetFaChartData(List<int> LagerIds,int lengthH1,
			DateTime? affectedWeek
			)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var clauses = new List<string>();
				string affectedWeekDateSql = affectedWeek is not null
				? $"CONVERT(date, '{affectedWeek:yyyy-MM-dd}')"
				: "GETDATE()";

				string changedBaseDateSql = affectedWeek is not null
					? $"DATEADD(DAY, -{lengthH1}, CONVERT(date, '{affectedWeek:yyyy-MM-dd}'))"
					: $"DATEADD(DAY, -{lengthH1}, GETDATE())";

				string query = @$"DECLARE @H1LengthInDays INT = {lengthH1};
				SELECT 
					DATEPART(ISO_WEEK, fa.Änderungsdatum) AS ChangeWeek,
					DATEPART(YEAR, fa.Änderungsdatum) AS ChangedYear,
					DATEPART(ISO_WEEK, fa.Termin_Bestätigt1) AS AffectedWeek,
					DATEPART(YEAR, fa.Termin_Bestätigt1) AS AffectedYear,
					SUM((fa.FA_Menge * f.Zeit) / 60) AS [HoursLeft]
				FROM [PSZ_Fertigungsauftrag Änderungshistorie] AS fa
				INNER JOIN [Fertigung] f ON fa.Fertigungsnummer = f.Fertigungsnummer
				INNER JOIN [Lagerorte] l ON fa.Lagerort_id = l.Lagerort_id
				WHERE 
					fa.Änderungsdatum >= {changedBaseDateSql}
					AND fa.Änderungsdatum < DATEADD(WEEK, 10, {changedBaseDateSql})
					AND fa.Termin_Bestätigt1 BETWEEN DATEADD(WEEK, -3, {affectedWeekDateSql}) AND DATEADD(WEEK, 7, {affectedWeekDateSql})
					AND fa.Termin_Bestätigt1 <= DATEADD(DAY, @H1LengthInDays, fa.Änderungsdatum)
					AND fa.Termin_voränderung > DATEADD(DAY, @H1LengthInDays, fa.Änderungsdatum)";
				if(LagerIds.Count > 0)
				{
					query += $" and fa.Lagerort_id  in ({string.Join(',', LagerIds)})";
				}
				

				query += $@" group by DATEPART(ISO_WEEK, fa.Änderungsdatum), DATEPART(YEAR, fa.Änderungsdatum), DATEPART(ISO_WEEK, fa.Termin_Bestätigt1), DATEPART(YEAR, fa.Termin_Bestätigt1)";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new FaChartDataEntity(x)).ToList();
			}
			else
			{
				return new List<FaChartDataEntity>();
			}
		}
		#endregion
	}
}
