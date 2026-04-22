using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Tbl_Planung_gestartet_HauptAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_Planung_gestartet_Haupt]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Tbl_Planung_gestartet_Haupt] ([Artikel_Nr],[Artikelnummer],[Benutzer_Komm],[Bezeichnung],[Datum_Bereit_G2],[Datum_Gestart_CAO],[Datum_Planung],[FA_Gestart_CAO],[Fertigungsnummer],[Gedruckt_Bereit_G2],[ID_Fertigung],[Id_User],[Lagerort_ID],[Menge],[N_Position_Ajouter],[Offen_Komm],[Status],[Termin_Bestatigt1],[Vergleich_Stuckliste],[Vergleich_Stuckliste_Automatique]) OUTPUT INSERTED.[ID] VALUES (@Artikel_Nr,@Artikelnummer,@Benutzer_Komm,@Bezeichnung,@Datum_Bereit_G2,@Datum_Gestart_CAO,@Datum_Planung,@FA_Gestart_CAO,@Fertigungsnummer,@Gedruckt_Bereit_G2,@ID_Fertigung,@Id_User,@Lagerort_ID,@Menge,@N_Position_Ajouter,@Offen_Komm,@Status,@Termin_Bestatigt1,@Vergleich_Stuckliste,@Vergleich_Stuckliste_Automatique); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Benutzer_Komm", item.Benutzer_Komm == null ? (object)DBNull.Value : item.Benutzer_Komm);
					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("Datum_Bereit_G2", item.Datum_Bereit_G2 == null ? (object)DBNull.Value : item.Datum_Bereit_G2);
					sqlCommand.Parameters.AddWithValue("Datum_Gestart_CAO", item.Datum_Gestart_CAO == null ? (object)DBNull.Value : item.Datum_Gestart_CAO);
					sqlCommand.Parameters.AddWithValue("Datum_Planung", item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
					sqlCommand.Parameters.AddWithValue("FA_Gestart_CAO", item.FA_Gestart_CAO == null ? (object)DBNull.Value : item.FA_Gestart_CAO);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Gedruckt_Bereit_G2", item.Gedruckt_Bereit_G2 == null ? (object)DBNull.Value : item.Gedruckt_Bereit_G2);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
					sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User == null ? (object)DBNull.Value : item.Id_User);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("N_Position_Ajouter", item.N_Position_Ajouter == null ? (object)DBNull.Value : item.N_Position_Ajouter);
					sqlCommand.Parameters.AddWithValue("Offen_Komm", item.Offen_Komm == null ? (object)DBNull.Value : item.Offen_Komm);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
					sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste", item.Vergleich_Stuckliste);
					sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste_Automatique", item.Vergleich_Stuckliste_Automatique);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> items)
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
						query += " INSERT INTO [Tbl_Planung_gestartet_Haupt] ([Artikel_Nr],[Artikelnummer],[Benutzer_Komm],[Bezeichnung],[Datum_Bereit_G2],[Datum_Gestart_CAO],[Datum_Planung],[FA_Gestart_CAO],[Fertigungsnummer],[Gedruckt_Bereit_G2],[ID_Fertigung],[Id_User],[Lagerort_ID],[Menge],[N_Position_Ajouter],[Offen_Komm],[Status],[Termin_Bestatigt1],[Vergleich_Stuckliste],[Vergleich_Stuckliste_Automatique]) VALUES ( "

							+ "@Artikel_Nr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Benutzer_Komm" + i + ","
							+ "@Bezeichnung" + i + ","
							+ "@Datum_Bereit_G2" + i + ","
							+ "@Datum_Gestart_CAO" + i + ","
							+ "@Datum_Planung" + i + ","
							+ "@FA_Gestart_CAO" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@Gedruckt_Bereit_G2" + i + ","
							+ "@ID_Fertigung" + i + ","
							+ "@Id_User" + i + ","
							+ "@Lagerort_ID" + i + ","
							+ "@Menge" + i + ","
							+ "@N_Position_Ajouter" + i + ","
							+ "@Offen_Komm" + i + ","
							+ "@Status" + i + ","
							+ "@Termin_Bestatigt1" + i + ","
							+ "@Vergleich_Stuckliste" + i + ","
							+ "@Vergleich_Stuckliste_Automatique" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Benutzer_Komm" + i, item.Benutzer_Komm == null ? (object)DBNull.Value : item.Benutzer_Komm);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("Datum_Bereit_G2" + i, item.Datum_Bereit_G2 == null ? (object)DBNull.Value : item.Datum_Bereit_G2);
						sqlCommand.Parameters.AddWithValue("Datum_Gestart_CAO" + i, item.Datum_Gestart_CAO == null ? (object)DBNull.Value : item.Datum_Gestart_CAO);
						sqlCommand.Parameters.AddWithValue("Datum_Planung" + i, item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
						sqlCommand.Parameters.AddWithValue("FA_Gestart_CAO" + i, item.FA_Gestart_CAO == null ? (object)DBNull.Value : item.FA_Gestart_CAO);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Gedruckt_Bereit_G2" + i, item.Gedruckt_Bereit_G2 == null ? (object)DBNull.Value : item.Gedruckt_Bereit_G2);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User == null ? (object)DBNull.Value : item.Id_User);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("N_Position_Ajouter" + i, item.N_Position_Ajouter == null ? (object)DBNull.Value : item.N_Position_Ajouter);
						sqlCommand.Parameters.AddWithValue("Offen_Komm" + i, item.Offen_Komm == null ? (object)DBNull.Value : item.Offen_Komm);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
						sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste" + i, item.Vergleich_Stuckliste);
						sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste_Automatique" + i, item.Vergleich_Stuckliste_Automatique);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Tbl_Planung_gestartet_Haupt] SET [Artikel_Nr]=@Artikel_Nr, [Artikelnummer]=@Artikelnummer, [Benutzer_Komm]=@Benutzer_Komm, [Bezeichnung]=@Bezeichnung, [Datum_Bereit_G2]=@Datum_Bereit_G2, [Datum_Gestart_CAO]=@Datum_Gestart_CAO, [Datum_Planung]=@Datum_Planung, [FA_Gestart_CAO]=@FA_Gestart_CAO, [Fertigungsnummer]=@Fertigungsnummer, [Gedruckt_Bereit_G2]=@Gedruckt_Bereit_G2, [ID_Fertigung]=@ID_Fertigung, [Id_User]=@Id_User, [Lagerort_ID]=@Lagerort_ID, [Menge]=@Menge, [N_Position_Ajouter]=@N_Position_Ajouter, [Offen_Komm]=@Offen_Komm, [Status]=@Status, [Termin_Bestatigt1]=@Termin_Bestatigt1, [Vergleich_Stuckliste]=@Vergleich_Stuckliste, [Vergleich_Stuckliste_Automatique]=@Vergleich_Stuckliste_Automatique WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Benutzer_Komm", item.Benutzer_Komm == null ? (object)DBNull.Value : item.Benutzer_Komm);
				sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
				sqlCommand.Parameters.AddWithValue("Datum_Bereit_G2", item.Datum_Bereit_G2 == null ? (object)DBNull.Value : item.Datum_Bereit_G2);
				sqlCommand.Parameters.AddWithValue("Datum_Gestart_CAO", item.Datum_Gestart_CAO == null ? (object)DBNull.Value : item.Datum_Gestart_CAO);
				sqlCommand.Parameters.AddWithValue("Datum_Planung", item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
				sqlCommand.Parameters.AddWithValue("FA_Gestart_CAO", item.FA_Gestart_CAO == null ? (object)DBNull.Value : item.FA_Gestart_CAO);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Gedruckt_Bereit_G2", item.Gedruckt_Bereit_G2 == null ? (object)DBNull.Value : item.Gedruckt_Bereit_G2);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
				sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User == null ? (object)DBNull.Value : item.Id_User);
				sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
				sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("N_Position_Ajouter", item.N_Position_Ajouter == null ? (object)DBNull.Value : item.N_Position_Ajouter);
				sqlCommand.Parameters.AddWithValue("Offen_Komm", item.Offen_Komm == null ? (object)DBNull.Value : item.Offen_Komm);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
				sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste", item.Vergleich_Stuckliste);
				sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste_Automatique", item.Vergleich_Stuckliste_Automatique);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> items)
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
						query += " UPDATE [Tbl_Planung_gestartet_Haupt] SET "

							+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Benutzer_Komm]=@Benutzer_Komm" + i + ","
							+ "[Bezeichnung]=@Bezeichnung" + i + ","
							+ "[Datum_Bereit_G2]=@Datum_Bereit_G2" + i + ","
							+ "[Datum_Gestart_CAO]=@Datum_Gestart_CAO" + i + ","
							+ "[Datum_Planung]=@Datum_Planung" + i + ","
							+ "[FA_Gestart_CAO]=@FA_Gestart_CAO" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[Gedruckt_Bereit_G2]=@Gedruckt_Bereit_G2" + i + ","
							+ "[ID_Fertigung]=@ID_Fertigung" + i + ","
							+ "[Id_User]=@Id_User" + i + ","
							+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
							+ "[Menge]=@Menge" + i + ","
							+ "[N_Position_Ajouter]=@N_Position_Ajouter" + i + ","
							+ "[Offen_Komm]=@Offen_Komm" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[Termin_Bestatigt1]=@Termin_Bestatigt1" + i + ","
							+ "[Vergleich_Stuckliste]=@Vergleich_Stuckliste" + i + ","
							+ "[Vergleich_Stuckliste_Automatique]=@Vergleich_Stuckliste_Automatique" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Benutzer_Komm" + i, item.Benutzer_Komm == null ? (object)DBNull.Value : item.Benutzer_Komm);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("Datum_Bereit_G2" + i, item.Datum_Bereit_G2 == null ? (object)DBNull.Value : item.Datum_Bereit_G2);
						sqlCommand.Parameters.AddWithValue("Datum_Gestart_CAO" + i, item.Datum_Gestart_CAO == null ? (object)DBNull.Value : item.Datum_Gestart_CAO);
						sqlCommand.Parameters.AddWithValue("Datum_Planung" + i, item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
						sqlCommand.Parameters.AddWithValue("FA_Gestart_CAO" + i, item.FA_Gestart_CAO == null ? (object)DBNull.Value : item.FA_Gestart_CAO);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Gedruckt_Bereit_G2" + i, item.Gedruckt_Bereit_G2 == null ? (object)DBNull.Value : item.Gedruckt_Bereit_G2);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User == null ? (object)DBNull.Value : item.Id_User);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("N_Position_Ajouter" + i, item.N_Position_Ajouter == null ? (object)DBNull.Value : item.N_Position_Ajouter);
						sqlCommand.Parameters.AddWithValue("Offen_Komm" + i, item.Offen_Komm == null ? (object)DBNull.Value : item.Offen_Komm);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
						sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste" + i, item.Vergleich_Stuckliste);
						sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste_Automatique" + i, item.Vergleich_Stuckliste_Automatique);
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
				string query = "DELETE FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Tbl_Planung_gestartet_Haupt]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Tbl_Planung_gestartet_Haupt] ([Artikel_Nr],[Artikelnummer],[Benutzer_Komm],[Bezeichnung],[Datum_Bereit_G2],[Datum_Gestart_CAO],[Datum_Planung],[FA_Gestart_CAO],[Fertigungsnummer],[Gedruckt_Bereit_G2],[ID_Fertigung],[Id_User],[Lagerort_ID],[Menge],[N_Position_Ajouter],[Offen_Komm],[Status],[Termin_Bestatigt1],[Vergleich_Stuckliste],[Vergleich_Stuckliste_Automatique]) OUTPUT INSERTED.[ID] VALUES (@Artikel_Nr,@Artikelnummer,@Benutzer_Komm,@Bezeichnung,@Datum_Bereit_G2,@Datum_Gestart_CAO,@Datum_Planung,@FA_Gestart_CAO,@Fertigungsnummer,@Gedruckt_Bereit_G2,@ID_Fertigung,@Id_User,@Lagerort_ID,@Menge,@N_Position_Ajouter,@Offen_Komm,@Status,@Termin_Bestatigt1,@Vergleich_Stuckliste,@Vergleich_Stuckliste_Automatique); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Benutzer_Komm", item.Benutzer_Komm == null ? (object)DBNull.Value : item.Benutzer_Komm);
			sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
			sqlCommand.Parameters.AddWithValue("Datum_Bereit_G2", item.Datum_Bereit_G2 == null ? (object)DBNull.Value : item.Datum_Bereit_G2);
			sqlCommand.Parameters.AddWithValue("Datum_Gestart_CAO", item.Datum_Gestart_CAO == null ? (object)DBNull.Value : item.Datum_Gestart_CAO);
			sqlCommand.Parameters.AddWithValue("Datum_Planung", item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
			sqlCommand.Parameters.AddWithValue("FA_Gestart_CAO", item.FA_Gestart_CAO == null ? (object)DBNull.Value : item.FA_Gestart_CAO);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Gedruckt_Bereit_G2", item.Gedruckt_Bereit_G2 == null ? (object)DBNull.Value : item.Gedruckt_Bereit_G2);
			sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
			sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User == null ? (object)DBNull.Value : item.Id_User);
			sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
			sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
			sqlCommand.Parameters.AddWithValue("N_Position_Ajouter", item.N_Position_Ajouter == null ? (object)DBNull.Value : item.N_Position_Ajouter);
			sqlCommand.Parameters.AddWithValue("Offen_Komm", item.Offen_Komm == null ? (object)DBNull.Value : item.Offen_Komm);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
			sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste", item.Vergleich_Stuckliste);
			sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste_Automatique", item.Vergleich_Stuckliste_Automatique);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Tbl_Planung_gestartet_Haupt] ([Artikel_Nr],[Artikelnummer],[Benutzer_Komm],[Bezeichnung],[Datum_Bereit_G2],[Datum_Gestart_CAO],[Datum_Planung],[FA_Gestart_CAO],[Fertigungsnummer],[Gedruckt_Bereit_G2],[ID_Fertigung],[Id_User],[Lagerort_ID],[Menge],[N_Position_Ajouter],[Offen_Komm],[Status],[Termin_Bestatigt1],[Vergleich_Stuckliste],[Vergleich_Stuckliste_Automatique]) VALUES ( "

						+ "@Artikel_Nr" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Benutzer_Komm" + i + ","
						+ "@Bezeichnung" + i + ","
						+ "@Datum_Bereit_G2" + i + ","
						+ "@Datum_Gestart_CAO" + i + ","
						+ "@Datum_Planung" + i + ","
						+ "@FA_Gestart_CAO" + i + ","
						+ "@Fertigungsnummer" + i + ","
						+ "@Gedruckt_Bereit_G2" + i + ","
						+ "@ID_Fertigung" + i + ","
						+ "@Id_User" + i + ","
						+ "@Lagerort_ID" + i + ","
						+ "@Menge" + i + ","
						+ "@N_Position_Ajouter" + i + ","
						+ "@Offen_Komm" + i + ","
						+ "@Status" + i + ","
						+ "@Termin_Bestatigt1" + i + ","
						+ "@Vergleich_Stuckliste" + i + ","
						+ "@Vergleich_Stuckliste_Automatique" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Benutzer_Komm" + i, item.Benutzer_Komm == null ? (object)DBNull.Value : item.Benutzer_Komm);
					sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("Datum_Bereit_G2" + i, item.Datum_Bereit_G2 == null ? (object)DBNull.Value : item.Datum_Bereit_G2);
					sqlCommand.Parameters.AddWithValue("Datum_Gestart_CAO" + i, item.Datum_Gestart_CAO == null ? (object)DBNull.Value : item.Datum_Gestart_CAO);
					sqlCommand.Parameters.AddWithValue("Datum_Planung" + i, item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
					sqlCommand.Parameters.AddWithValue("FA_Gestart_CAO" + i, item.FA_Gestart_CAO == null ? (object)DBNull.Value : item.FA_Gestart_CAO);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Gedruckt_Bereit_G2" + i, item.Gedruckt_Bereit_G2 == null ? (object)DBNull.Value : item.Gedruckt_Bereit_G2);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
					sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User == null ? (object)DBNull.Value : item.Id_User);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("N_Position_Ajouter" + i, item.N_Position_Ajouter == null ? (object)DBNull.Value : item.N_Position_Ajouter);
					sqlCommand.Parameters.AddWithValue("Offen_Komm" + i, item.Offen_Komm == null ? (object)DBNull.Value : item.Offen_Komm);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
					sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste" + i, item.Vergleich_Stuckliste);
					sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste_Automatique" + i, item.Vergleich_Stuckliste_Automatique);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Tbl_Planung_gestartet_Haupt] SET [Artikel_Nr]=@Artikel_Nr, [Artikelnummer]=@Artikelnummer, [Benutzer_Komm]=@Benutzer_Komm, [Bezeichnung]=@Bezeichnung, [Datum_Bereit_G2]=@Datum_Bereit_G2, [Datum_Gestart_CAO]=@Datum_Gestart_CAO, [Datum_Planung]=@Datum_Planung, [FA_Gestart_CAO]=@FA_Gestart_CAO, [Fertigungsnummer]=@Fertigungsnummer, [Gedruckt_Bereit_G2]=@Gedruckt_Bereit_G2, [ID_Fertigung]=@ID_Fertigung, [Id_User]=@Id_User, [Lagerort_ID]=@Lagerort_ID, [Menge]=@Menge, [N_Position_Ajouter]=@N_Position_Ajouter, [Offen_Komm]=@Offen_Komm, [Status]=@Status, [Termin_Bestatigt1]=@Termin_Bestatigt1, [Vergleich_Stuckliste]=@Vergleich_Stuckliste, [Vergleich_Stuckliste_Automatique]=@Vergleich_Stuckliste_Automatique WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Benutzer_Komm", item.Benutzer_Komm == null ? (object)DBNull.Value : item.Benutzer_Komm);
			sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
			sqlCommand.Parameters.AddWithValue("Datum_Bereit_G2", item.Datum_Bereit_G2 == null ? (object)DBNull.Value : item.Datum_Bereit_G2);
			sqlCommand.Parameters.AddWithValue("Datum_Gestart_CAO", item.Datum_Gestart_CAO == null ? (object)DBNull.Value : item.Datum_Gestart_CAO);
			sqlCommand.Parameters.AddWithValue("Datum_Planung", item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
			sqlCommand.Parameters.AddWithValue("FA_Gestart_CAO", item.FA_Gestart_CAO == null ? (object)DBNull.Value : item.FA_Gestart_CAO);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Gedruckt_Bereit_G2", item.Gedruckt_Bereit_G2 == null ? (object)DBNull.Value : item.Gedruckt_Bereit_G2);
			sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
			sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User == null ? (object)DBNull.Value : item.Id_User);
			sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
			sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
			sqlCommand.Parameters.AddWithValue("N_Position_Ajouter", item.N_Position_Ajouter == null ? (object)DBNull.Value : item.N_Position_Ajouter);
			sqlCommand.Parameters.AddWithValue("Offen_Komm", item.Offen_Komm == null ? (object)DBNull.Value : item.Offen_Komm);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1", item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
			sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste", item.Vergleich_Stuckliste);
			sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste_Automatique", item.Vergleich_Stuckliste_Automatique);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Tbl_Planung_gestartet_Haupt] SET "

					+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Benutzer_Komm]=@Benutzer_Komm" + i + ","
					+ "[Bezeichnung]=@Bezeichnung" + i + ","
					+ "[Datum_Bereit_G2]=@Datum_Bereit_G2" + i + ","
					+ "[Datum_Gestart_CAO]=@Datum_Gestart_CAO" + i + ","
					+ "[Datum_Planung]=@Datum_Planung" + i + ","
					+ "[FA_Gestart_CAO]=@FA_Gestart_CAO" + i + ","
					+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
					+ "[Gedruckt_Bereit_G2]=@Gedruckt_Bereit_G2" + i + ","
					+ "[ID_Fertigung]=@ID_Fertigung" + i + ","
					+ "[Id_User]=@Id_User" + i + ","
					+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
					+ "[Menge]=@Menge" + i + ","
					+ "[N_Position_Ajouter]=@N_Position_Ajouter" + i + ","
					+ "[Offen_Komm]=@Offen_Komm" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[Termin_Bestatigt1]=@Termin_Bestatigt1" + i + ","
					+ "[Vergleich_Stuckliste]=@Vergleich_Stuckliste" + i + ","
					+ "[Vergleich_Stuckliste_Automatique]=@Vergleich_Stuckliste_Automatique" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Benutzer_Komm" + i, item.Benutzer_Komm == null ? (object)DBNull.Value : item.Benutzer_Komm);
					sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("Datum_Bereit_G2" + i, item.Datum_Bereit_G2 == null ? (object)DBNull.Value : item.Datum_Bereit_G2);
					sqlCommand.Parameters.AddWithValue("Datum_Gestart_CAO" + i, item.Datum_Gestart_CAO == null ? (object)DBNull.Value : item.Datum_Gestart_CAO);
					sqlCommand.Parameters.AddWithValue("Datum_Planung" + i, item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
					sqlCommand.Parameters.AddWithValue("FA_Gestart_CAO" + i, item.FA_Gestart_CAO == null ? (object)DBNull.Value : item.FA_Gestart_CAO);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Gedruckt_Bereit_G2" + i, item.Gedruckt_Bereit_G2 == null ? (object)DBNull.Value : item.Gedruckt_Bereit_G2);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
					sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User == null ? (object)DBNull.Value : item.Id_User);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("N_Position_Ajouter" + i, item.N_Position_Ajouter == null ? (object)DBNull.Value : item.N_Position_Ajouter);
					sqlCommand.Parameters.AddWithValue("Offen_Komm" + i, item.Offen_Komm == null ? (object)DBNull.Value : item.Offen_Komm);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Termin_Bestatigt1" + i, item.Termin_Bestatigt1 == null ? (object)DBNull.Value : item.Termin_Bestatigt1);
					sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste" + i, item.Vergleich_Stuckliste);
					sqlCommand.Parameters.AddWithValue("Vergleich_Stuckliste_Automatique" + i, item.Vergleich_Stuckliste_Automatique);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity GetByFertigungId(int faId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_Planung_gestartet_Haupt] WHERE [ID_Fertigung]=@faId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("faId", faId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods

	}
}
