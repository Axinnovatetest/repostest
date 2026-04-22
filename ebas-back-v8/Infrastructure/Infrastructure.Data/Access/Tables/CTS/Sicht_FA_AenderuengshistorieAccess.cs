using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Sicht_FA_AenderuengshistorieAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity Get(DateTime änderungsdatum)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Sicht_FA_Aenderuengshistorie] WHERE [Änderungsdatum]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", änderungsdatum);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Sicht_FA_Aenderuengshistorie]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> Get(List<DateTime> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> get(List<DateTime> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Sicht_FA_Aenderuengshistorie] WHERE [Änderungsdatum] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity>();
		}

		public static DateTime Insert(Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity item)
		{
			DateTime response = DateTime.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Sicht_FA_Aenderuengshistorie] ([Änderungsdatum],[Angebot-Nr],[Artikelnummer],[Bemerkung],[Bezeichnung],[CS_Mitarbeiter],[Erstmuster],[FA_Menge],[Fertigungsnummer],[Grund_CS],[ID],[Kapazitätsproblem],[Kapazitätsproblematik],[Lagerort_id],[Materialproblem],[Materialproblematik],[Mitarbeiter],[Sonstige_Problematik],[Sonstiges],[Termin_Angebot],[Termin_Bestätigt1],[Termin_voränderung],[Termin_Wunsch],[Ursprünglicher_termin],[Werkzeugproblem],[Werkzeugproblematik],[Wunsch_CS])  VALUES (@Änderungsdatum,@Angebot_Nr,@Artikelnummer,@Bemerkung,@Bezeichnung,@CS_Mitarbeiter,@Erstmuster,@FA_Menge,@Fertigungsnummer,@Grund_CS,@ID,@Kapazitätsproblem,@Kapazitätsproblematik,@Lagerort_id,@Materialproblem,@Materialproblematik,@Mitarbeiter,@Sonstige_Problematik,@Sonstiges,@Termin_Angebot,@Termin_Bestätigt1,@Termin_voränderung,@Termin_Wunsch,@Ursprünglicher_termin,@Werkzeugproblem,@Werkzeugproblematik,@Wunsch_CS); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Änderungsdatum", item.Änderungsdatum);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter", item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
					sqlCommand.Parameters.AddWithValue("FA_Menge", item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Grund_CS", item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
					sqlCommand.Parameters.AddWithValue("ID", item.ID);
					sqlCommand.Parameters.AddWithValue("Kapazitätsproblem", item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
					sqlCommand.Parameters.AddWithValue("Kapazitätsproblematik", item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Materialproblem", item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
					sqlCommand.Parameters.AddWithValue("Materialproblematik", item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Sonstige_Problematik", item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
					sqlCommand.Parameters.AddWithValue("Sonstiges", item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
					sqlCommand.Parameters.AddWithValue("Termin_Angebot", item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
					sqlCommand.Parameters.AddWithValue("Termin_Bestätigt1", item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
					sqlCommand.Parameters.AddWithValue("Termin_voränderung", item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
					sqlCommand.Parameters.AddWithValue("Termin_Wunsch", item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
					sqlCommand.Parameters.AddWithValue("Ursprünglicher_termin", item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
					sqlCommand.Parameters.AddWithValue("Werkzeugproblem", item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
					sqlCommand.Parameters.AddWithValue("Werkzeugproblematik", item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
					sqlCommand.Parameters.AddWithValue("Wunsch_CS", item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? DateTime.MinValue : DateTime.TryParse(result.ToString(), out var insertedId) ? insertedId : DateTime.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> items)
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
						query += " INSERT INTO [Sicht_FA_Aenderuengshistorie] ([Änderungsdatum],[Angebot-Nr],[Artikelnummer],[Bemerkung],[Bezeichnung],[CS_Mitarbeiter],[Erstmuster],[FA_Menge],[Fertigungsnummer],[Grund_CS],[ID],[Kapazitätsproblem],[Kapazitätsproblematik],[Lagerort_id],[Materialproblem],[Materialproblematik],[Mitarbeiter],[Sonstige_Problematik],[Sonstiges],[Termin_Angebot],[Termin_Bestätigt1],[Termin_voränderung],[Termin_Wunsch],[Ursprünglicher_termin],[Werkzeugproblem],[Werkzeugproblematik],[Wunsch_CS]) VALUES ( "

							+ "@Änderungsdatum" + i + ","
							+ "@Angebot_Nr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@Bezeichnung" + i + ","
							+ "@CS_Mitarbeiter" + i + ","
							+ "@Erstmuster" + i + ","
							+ "@FA_Menge" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@Grund_CS" + i + ","
							+ "@ID" + i + ","
							+ "@Kapazitätsproblem" + i + ","
							+ "@Kapazitätsproblematik" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Materialproblem" + i + ","
							+ "@Materialproblematik" + i + ","
							+ "@Mitarbeiter" + i + ","
							+ "@Sonstige_Problematik" + i + ","
							+ "@Sonstiges" + i + ","
							+ "@Termin_Angebot" + i + ","
							+ "@Termin_Bestätigt1" + i + ","
							+ "@Termin_voränderung" + i + ","
							+ "@Termin_Wunsch" + i + ","
							+ "@Ursprünglicher_termin" + i + ","
							+ "@Werkzeugproblem" + i + ","
							+ "@Werkzeugproblematik" + i + ","
							+ "@Wunsch_CS" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Änderungsdatum" + i, item.Änderungsdatum);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter" + i, item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
						sqlCommand.Parameters.AddWithValue("FA_Menge" + i, item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Grund_CS" + i, item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Kapazitätsproblem" + i, item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
						sqlCommand.Parameters.AddWithValue("Kapazitätsproblematik" + i, item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Materialproblem" + i, item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
						sqlCommand.Parameters.AddWithValue("Materialproblematik" + i, item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Sonstige_Problematik" + i, item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
						sqlCommand.Parameters.AddWithValue("Sonstiges" + i, item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
						sqlCommand.Parameters.AddWithValue("Termin_Angebot" + i, item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
						sqlCommand.Parameters.AddWithValue("Termin_Bestätigt1" + i, item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
						sqlCommand.Parameters.AddWithValue("Termin_voränderung" + i, item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
						sqlCommand.Parameters.AddWithValue("Termin_Wunsch" + i, item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
						sqlCommand.Parameters.AddWithValue("Ursprünglicher_termin" + i, item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
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

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Sicht_FA_Aenderuengshistorie] SET [Angebot-Nr]=@Angebot_Nr, [Artikelnummer]=@Artikelnummer, [Bemerkung]=@Bemerkung, [Bezeichnung]=@Bezeichnung, [CS_Mitarbeiter]=@CS_Mitarbeiter, [Erstmuster]=@Erstmuster, [FA_Menge]=@FA_Menge, [Fertigungsnummer]=@Fertigungsnummer, [Grund_CS]=@Grund_CS, [ID]=@ID, [Kapazitätsproblem]=@Kapazitätsproblem, [Kapazitätsproblematik]=@Kapazitätsproblematik, [Lagerort_id]=@Lagerort_id, [Materialproblem]=@Materialproblem, [Materialproblematik]=@Materialproblematik, [Mitarbeiter]=@Mitarbeiter, [Sonstige_Problematik]=@Sonstige_Problematik, [Sonstiges]=@Sonstiges, [Termin_Angebot]=@Termin_Angebot, [Termin_Bestätigt1]=@Termin_Bestätigt1, [Termin_voränderung]=@Termin_voränderung, [Termin_Wunsch]=@Termin_Wunsch, [Ursprünglicher_termin]=@Ursprünglicher_termin, [Werkzeugproblem]=@Werkzeugproblem, [Werkzeugproblematik]=@Werkzeugproblematik, [Wunsch_CS]=@Wunsch_CS WHERE [Änderungsdatum]=@Änderungsdatum";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Änderungsdatum", item.Änderungsdatum);
				sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
				sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter", item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
				sqlCommand.Parameters.AddWithValue("FA_Menge", item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Grund_CS", item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Kapazitätsproblem", item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
				sqlCommand.Parameters.AddWithValue("Kapazitätsproblematik", item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Materialproblem", item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
				sqlCommand.Parameters.AddWithValue("Materialproblematik", item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
				sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("Sonstige_Problematik", item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
				sqlCommand.Parameters.AddWithValue("Sonstiges", item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
				sqlCommand.Parameters.AddWithValue("Termin_Angebot", item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
				sqlCommand.Parameters.AddWithValue("Termin_Bestätigt1", item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
				sqlCommand.Parameters.AddWithValue("Termin_voränderung", item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
				sqlCommand.Parameters.AddWithValue("Termin_Wunsch", item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
				sqlCommand.Parameters.AddWithValue("Ursprünglicher_termin", item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
				sqlCommand.Parameters.AddWithValue("Werkzeugproblem", item.Werkzeugproblem == null ? (object)DBNull.Value : item.Werkzeugproblem);
				sqlCommand.Parameters.AddWithValue("Werkzeugproblematik", item.Werkzeugproblematik == null ? (object)DBNull.Value : item.Werkzeugproblematik);
				sqlCommand.Parameters.AddWithValue("Wunsch_CS", item.Wunsch_CS == null ? (object)DBNull.Value : item.Wunsch_CS);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> items)
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
						query += " UPDATE [Sicht_FA_Aenderuengshistorie] SET "

							+ "[Angebot-Nr]=@Angebot_Nr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Bezeichnung]=@Bezeichnung" + i + ","
							+ "[CS_Mitarbeiter]=@CS_Mitarbeiter" + i + ","
							+ "[Erstmuster]=@Erstmuster" + i + ","
							+ "[FA_Menge]=@FA_Menge" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[Grund_CS]=@Grund_CS" + i + ","
							+ "[ID]=@ID" + i + ","
							+ "[Kapazitätsproblem]=@Kapazitätsproblem" + i + ","
							+ "[Kapazitätsproblematik]=@Kapazitätsproblematik" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Materialproblem]=@Materialproblem" + i + ","
							+ "[Materialproblematik]=@Materialproblematik" + i + ","
							+ "[Mitarbeiter]=@Mitarbeiter" + i + ","
							+ "[Sonstige_Problematik]=@Sonstige_Problematik" + i + ","
							+ "[Sonstiges]=@Sonstiges" + i + ","
							+ "[Termin_Angebot]=@Termin_Angebot" + i + ","
							+ "[Termin_Bestätigt1]=@Termin_Bestätigt1" + i + ","
							+ "[Termin_voränderung]=@Termin_voränderung" + i + ","
							+ "[Termin_Wunsch]=@Termin_Wunsch" + i + ","
							+ "[Ursprünglicher_termin]=@Ursprünglicher_termin" + i + ","
							+ "[Werkzeugproblem]=@Werkzeugproblem" + i + ","
							+ "[Werkzeugproblematik]=@Werkzeugproblematik" + i + ","
							+ "[Wunsch_CS]=@Wunsch_CS" + i + " WHERE [Änderungsdatum]=@Änderungsdatum" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Änderungsdatum" + i, item.Änderungsdatum);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("CS_Mitarbeiter" + i, item.CS_Mitarbeiter == null ? (object)DBNull.Value : item.CS_Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Erstmuster" + i, item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
						sqlCommand.Parameters.AddWithValue("FA_Menge" + i, item.FA_Menge == null ? (object)DBNull.Value : item.FA_Menge);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Grund_CS" + i, item.Grund_CS == null ? (object)DBNull.Value : item.Grund_CS);
						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Kapazitätsproblem" + i, item.Kapazitätsproblem == null ? (object)DBNull.Value : item.Kapazitätsproblem);
						sqlCommand.Parameters.AddWithValue("Kapazitätsproblematik" + i, item.Kapazitätsproblematik == null ? (object)DBNull.Value : item.Kapazitätsproblematik);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Materialproblem" + i, item.Materialproblem == null ? (object)DBNull.Value : item.Materialproblem);
						sqlCommand.Parameters.AddWithValue("Materialproblematik" + i, item.Materialproblematik == null ? (object)DBNull.Value : item.Materialproblematik);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Sonstige_Problematik" + i, item.Sonstige_Problematik == null ? (object)DBNull.Value : item.Sonstige_Problematik);
						sqlCommand.Parameters.AddWithValue("Sonstiges" + i, item.Sonstiges == null ? (object)DBNull.Value : item.Sonstiges);
						sqlCommand.Parameters.AddWithValue("Termin_Angebot" + i, item.Termin_Angebot == null ? (object)DBNull.Value : item.Termin_Angebot);
						sqlCommand.Parameters.AddWithValue("Termin_Bestätigt1" + i, item.Termin_Bestätigt1 == null ? (object)DBNull.Value : item.Termin_Bestätigt1);
						sqlCommand.Parameters.AddWithValue("Termin_voränderung" + i, item.Termin_voränderung == null ? (object)DBNull.Value : item.Termin_voränderung);
						sqlCommand.Parameters.AddWithValue("Termin_Wunsch" + i, item.Termin_Wunsch == null ? (object)DBNull.Value : item.Termin_Wunsch);
						sqlCommand.Parameters.AddWithValue("Ursprünglicher_termin" + i, item.Ursprünglicher_termin == null ? (object)DBNull.Value : item.Ursprünglicher_termin);
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

		public static int Delete(DateTime änderungsdatum)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Sicht_FA_Aenderuengshistorie] WHERE [Änderungsdatum]=@Änderungsdatum";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Änderungsdatum", änderungsdatum);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<DateTime> ids)
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
		private static int delete(List<DateTime> ids)
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

					string query = "DELETE FROM [Sicht_FA_Aenderuengshistorie] WHERE [Änderungsdatum] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> GetByLagerAndDate(int? lager, DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Sicht_FA_Aenderuengshistorie] WHERE [Änderungsdatum]>=@From AND [Änderungsdatum]<=@To";
				if(lager.HasValue)
					query += $" AND Lagerort_id LIKE '%{lager}%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity>();
			}
		}
		#endregion
	}
}
