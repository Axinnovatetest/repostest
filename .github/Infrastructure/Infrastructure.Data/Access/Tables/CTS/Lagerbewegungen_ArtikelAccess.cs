using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Lagerbewegungen_ArtikelAccess
	{


		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerbewegungen_Artikel] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerbewegungen_Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Lagerbewegungen_Artikel] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Lagerbewegungen_Artikel] ([AnfangLagerBestand],[Anzahl],[Anzahl_nach],[Artikel-nr],[Artikel-nr_nach],[Bemerkung],[Bezeichnung 1],[Bezeichnung 1_nach],[Einheit],[EndeLagerBestand],[Fertigungsnummer],[Gebucht von],[Grund],[ID_Schneiderei],[Karton_ID],[Lager_nach],[Lager_von],[Lagerbewegungen_id],[Löschen],[Preiseinheit],[receivedQuantity],[Rollennummer],[STRG_SCAN],[Umlaufartikel],[WereingangId]) OUTPUT INSERTED.[ID] VALUES (@AnfangLagerBestand,@Anzahl,@Anzahl_nach,@Artikel_nr,@Artikel_nr_nach,@Bemerkung,@Bezeichnung_1,@Bezeichnung_1_nach,@Einheit,@EndeLagerBestand,@Fertigungsnummer,@Gebucht_von,@Grund,@ID_Schneiderei,@Karton_ID,@Lager_nach,@Lager_von,@Lagerbewegungen_id,@Loschen,@Preiseinheit,@receivedQuantity,@Rollennummer,@STRG_SCAN,@Umlaufartikel,@WereingangId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Anzahl_nach", item.Anzahl_nach == null ? (object)DBNull.Value : item.Anzahl_nach);
					sqlCommand.Parameters.AddWithValue("Artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
					sqlCommand.Parameters.AddWithValue("Artikel_nr_nach", item.Artikel_nr_nach == null ? (object)DBNull.Value : item.Artikel_nr_nach);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1_nach", item.Bezeichnung_1_nach == null ? (object)DBNull.Value : item.Bezeichnung_1_nach);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Gebucht_von", item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
					sqlCommand.Parameters.AddWithValue("Grund", item.Grund == null ? (object)DBNull.Value : item.Grund);
					sqlCommand.Parameters.AddWithValue("ID_Schneiderei", item.ID_Schneiderei == null ? (object)DBNull.Value : item.ID_Schneiderei);
					sqlCommand.Parameters.AddWithValue("Karton_ID", item.Karton_ID == null ? (object)DBNull.Value : item.Karton_ID);
					sqlCommand.Parameters.AddWithValue("Lager_nach", item.Lager_nach == null ? (object)DBNull.Value : item.Lager_nach);
					sqlCommand.Parameters.AddWithValue("Lager_von", item.Lager_von == null ? (object)DBNull.Value : item.Lager_von);
					sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", item.Lagerbewegungen_id == null ? (object)DBNull.Value : item.Lagerbewegungen_id);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("receivedQuantity", item.receivedQuantity == null ? (object)DBNull.Value : item.receivedQuantity);
					sqlCommand.Parameters.AddWithValue("Rollennummer", item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
					sqlCommand.Parameters.AddWithValue("STRG_SCAN", item.STRG_SCAN == null ? (object)DBNull.Value : item.STRG_SCAN);
					sqlCommand.Parameters.AddWithValue("Umlaufartikel", item.Umlaufartikel == null ? (object)DBNull.Value : item.Umlaufartikel);
					sqlCommand.Parameters.AddWithValue("WereingangId", item.WereingangId == null ? (object)DBNull.Value : item.WereingangId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber =  Settings.MAX_BATCH_SIZE / 27; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> items)
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
						query += " INSERT INTO [Lagerbewegungen_Artikel] ([AnfangLagerBestand],[Anzahl],[Anzahl_nach],[Artikel-nr],[Artikel-nr_nach],[Bemerkung],[Bezeichnung 1],[Bezeichnung 1_nach],[Einheit],[EndeLagerBestand],[Fertigungsnummer],[Gebucht von],[Grund],[ID_Schneiderei],[Karton_ID],[Lager_nach],[Lager_von],[Lagerbewegungen_id],[Löschen],[Preiseinheit],[receivedQuantity],[Rollennummer],[STRG_SCAN],[Umlaufartikel],[WereingangId]) VALUES ( "

							+ "@AnfangLagerBestand" + i + ","
							+ "@Anzahl" + i + ","
							+ "@Anzahl_nach" + i + ","
							+ "@Artikel_nr" + i + ","
							+ "@Artikel_nr_nach" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Bezeichnung_1_nach" + i + ","
							+ "@Einheit" + i + ","
							+ "@EndeLagerBestand" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@Gebucht_von" + i + ","
							+ "@Grund" + i + ","
							+ "@ID_Schneiderei" + i + ","
							+ "@Karton_ID" + i + ","
							+ "@Lager_nach" + i + ","
							+ "@Lager_von" + i + ","
							+ "@Lagerbewegungen_id" + i + ","
							+ "@Loschen" + i + ","
							+ "@Preiseinheit" + i + ","
							+ "@receivedQuantity" + i + ","
							+ "@Rollennummer" + i + ","
							+ "@STRG_SCAN" + i + ","
							+ "@Umlaufartikel" + i + ","
							+ "@WereingangId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Anzahl_nach" + i, item.Anzahl_nach == null ? (object)DBNull.Value : item.Anzahl_nach);
						sqlCommand.Parameters.AddWithValue("Artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
						sqlCommand.Parameters.AddWithValue("Artikel_nr_nach" + i, item.Artikel_nr_nach == null ? (object)DBNull.Value : item.Artikel_nr_nach);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1_nach" + i, item.Bezeichnung_1_nach == null ? (object)DBNull.Value : item.Bezeichnung_1_nach);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Gebucht_von" + i, item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
						sqlCommand.Parameters.AddWithValue("Grund" + i, item.Grund == null ? (object)DBNull.Value : item.Grund);
						sqlCommand.Parameters.AddWithValue("ID_Schneiderei" + i, item.ID_Schneiderei == null ? (object)DBNull.Value : item.ID_Schneiderei);
						sqlCommand.Parameters.AddWithValue("Karton_ID" + i, item.Karton_ID == null ? (object)DBNull.Value : item.Karton_ID);
						sqlCommand.Parameters.AddWithValue("Lager_nach" + i, item.Lager_nach == null ? (object)DBNull.Value : item.Lager_nach);
						sqlCommand.Parameters.AddWithValue("Lager_von" + i, item.Lager_von == null ? (object)DBNull.Value : item.Lager_von);
						sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id" + i, item.Lagerbewegungen_id == null ? (object)DBNull.Value : item.Lagerbewegungen_id);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("receivedQuantity" + i, item.receivedQuantity == null ? (object)DBNull.Value : item.receivedQuantity);
						sqlCommand.Parameters.AddWithValue("Rollennummer" + i, item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
						sqlCommand.Parameters.AddWithValue("STRG_SCAN" + i, item.STRG_SCAN == null ? (object)DBNull.Value : item.STRG_SCAN);
						sqlCommand.Parameters.AddWithValue("Umlaufartikel" + i, item.Umlaufartikel == null ? (object)DBNull.Value : item.Umlaufartikel);
						sqlCommand.Parameters.AddWithValue("WereingangId" + i, item.WereingangId == null ? (object)DBNull.Value : item.WereingangId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lagerbewegungen_Artikel] SET [AnfangLagerBestand]=@AnfangLagerBestand, [Anzahl]=@Anzahl, [Anzahl_nach]=@Anzahl_nach, [Artikel-nr]=@Artikel_nr, [Artikel-nr_nach]=@Artikel_nr_nach, [Bemerkung]=@Bemerkung, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 1_nach]=@Bezeichnung_1_nach, [Einheit]=@Einheit, [EndeLagerBestand]=@EndeLagerBestand, [Fertigungsnummer]=@Fertigungsnummer, [Gebucht von]=@Gebucht_von, [Grund]=@Grund, [ID_Schneiderei]=@ID_Schneiderei, [Karton_ID]=@Karton_ID, [Lager_nach]=@Lager_nach, [Lager_von]=@Lager_von, [Lagerbewegungen_id]=@Lagerbewegungen_id, [Löschen]=@Loschen, [Preiseinheit]=@Preiseinheit, [receivedQuantity]=@receivedQuantity, [Rollennummer]=@Rollennummer, [STRG_SCAN]=@STRG_SCAN, [Umlaufartikel]=@Umlaufartikel, [WereingangId]=@WereingangId WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Anzahl_nach", item.Anzahl_nach == null ? (object)DBNull.Value : item.Anzahl_nach);
				sqlCommand.Parameters.AddWithValue("Artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
				sqlCommand.Parameters.AddWithValue("Artikel_nr_nach", item.Artikel_nr_nach == null ? (object)DBNull.Value : item.Artikel_nr_nach);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1_nach", item.Bezeichnung_1_nach == null ? (object)DBNull.Value : item.Bezeichnung_1_nach);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
				sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Gebucht_von", item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
				sqlCommand.Parameters.AddWithValue("Grund", item.Grund == null ? (object)DBNull.Value : item.Grund);
				sqlCommand.Parameters.AddWithValue("ID_Schneiderei", item.ID_Schneiderei == null ? (object)DBNull.Value : item.ID_Schneiderei);
				sqlCommand.Parameters.AddWithValue("Karton_ID", item.Karton_ID == null ? (object)DBNull.Value : item.Karton_ID);
				sqlCommand.Parameters.AddWithValue("Lager_nach", item.Lager_nach == null ? (object)DBNull.Value : item.Lager_nach);
				sqlCommand.Parameters.AddWithValue("Lager_von", item.Lager_von == null ? (object)DBNull.Value : item.Lager_von);
				sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", item.Lagerbewegungen_id == null ? (object)DBNull.Value : item.Lagerbewegungen_id);
				sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("receivedQuantity", item.receivedQuantity == null ? (object)DBNull.Value : item.receivedQuantity);
				sqlCommand.Parameters.AddWithValue("Rollennummer", item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
				sqlCommand.Parameters.AddWithValue("STRG_SCAN", item.STRG_SCAN == null ? (object)DBNull.Value : item.STRG_SCAN);
				sqlCommand.Parameters.AddWithValue("Umlaufartikel", item.Umlaufartikel == null ? (object)DBNull.Value : item.Umlaufartikel);
				sqlCommand.Parameters.AddWithValue("WereingangId", item.WereingangId == null ? (object)DBNull.Value : item.WereingangId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber =  Settings.MAX_BATCH_SIZE / 27; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> items)
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
						query += " UPDATE [Lagerbewegungen_Artikel] SET "

							+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Anzahl_nach]=@Anzahl_nach" + i + ","
							+ "[Artikel-nr]=@Artikel_nr" + i + ","
							+ "[Artikel-nr_nach]=@Artikel_nr_nach" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
							+ "[Bezeichnung 1_nach]=@Bezeichnung_1_nach" + i + ","
							+ "[Einheit]=@Einheit" + i + ","
							+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[Gebucht von]=@Gebucht_von" + i + ","
							+ "[Grund]=@Grund" + i + ","
							+ "[ID_Schneiderei]=@ID_Schneiderei" + i + ","
							+ "[Karton_ID]=@Karton_ID" + i + ","
							+ "[Lager_nach]=@Lager_nach" + i + ","
							+ "[Lager_von]=@Lager_von" + i + ","
							+ "[Lagerbewegungen_id]=@Lagerbewegungen_id" + i + ","
							+ "[Löschen]=@Loschen" + i + ","
							+ "[Preiseinheit]=@Preiseinheit" + i + ","
							+ "[receivedQuantity]=@receivedQuantity" + i + ","
							+ "[Rollennummer]=@Rollennummer" + i + ","
							+ "[STRG_SCAN]=@STRG_SCAN" + i + ","
							+ "[Umlaufartikel]=@Umlaufartikel" + i + ","
							+ "[WereingangId]=@WereingangId" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Anzahl_nach" + i, item.Anzahl_nach == null ? (object)DBNull.Value : item.Anzahl_nach);
						sqlCommand.Parameters.AddWithValue("Artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
						sqlCommand.Parameters.AddWithValue("Artikel_nr_nach" + i, item.Artikel_nr_nach == null ? (object)DBNull.Value : item.Artikel_nr_nach);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1_nach" + i, item.Bezeichnung_1_nach == null ? (object)DBNull.Value : item.Bezeichnung_1_nach);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Gebucht_von" + i, item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
						sqlCommand.Parameters.AddWithValue("Grund" + i, item.Grund == null ? (object)DBNull.Value : item.Grund);
						sqlCommand.Parameters.AddWithValue("ID_Schneiderei" + i, item.ID_Schneiderei == null ? (object)DBNull.Value : item.ID_Schneiderei);
						sqlCommand.Parameters.AddWithValue("Karton_ID" + i, item.Karton_ID == null ? (object)DBNull.Value : item.Karton_ID);
						sqlCommand.Parameters.AddWithValue("Lager_nach" + i, item.Lager_nach == null ? (object)DBNull.Value : item.Lager_nach);
						sqlCommand.Parameters.AddWithValue("Lager_von" + i, item.Lager_von == null ? (object)DBNull.Value : item.Lager_von);
						sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id" + i, item.Lagerbewegungen_id == null ? (object)DBNull.Value : item.Lagerbewegungen_id);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("receivedQuantity" + i, item.receivedQuantity == null ? (object)DBNull.Value : item.receivedQuantity);
						sqlCommand.Parameters.AddWithValue("Rollennummer" + i, item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
						sqlCommand.Parameters.AddWithValue("STRG_SCAN" + i, item.STRG_SCAN == null ? (object)DBNull.Value : item.STRG_SCAN);
						sqlCommand.Parameters.AddWithValue("Umlaufartikel" + i, item.Umlaufartikel == null ? (object)DBNull.Value : item.Umlaufartikel);
						sqlCommand.Parameters.AddWithValue("WereingangId" + i, item.WereingangId == null ? (object)DBNull.Value : item.WereingangId);
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
				string query = "DELETE FROM [Lagerbewegungen_Artikel] WHERE [ID]=@ID";
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
				int maxParamsNumber =  Settings.MAX_BATCH_SIZE;
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

					string query = "DELETE FROM [Lagerbewegungen_Artikel] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lagerbewegungen_Artikel] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lagerbewegungen_Artikel]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber =  Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Lagerbewegungen_Artikel] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Lagerbewegungen_Artikel] ([AnfangLagerBestand],[Anzahl],[Anzahl_nach],[Artikel-nr],[Artikel-nr_nach],[Bemerkung],[Bezeichnung 1],[Bezeichnung 1_nach],[Einheit],[EndeLagerBestand],[Fertigungsnummer],[Gebucht von],[Grund],[ID_Schneiderei],[Karton_ID],[Lager_nach],[Lager_von],[Lagerbewegungen_id],[Löschen],[Preiseinheit],[receivedQuantity],[Rollennummer],[STRG_SCAN],[Umlaufartikel],[WereingangId]) OUTPUT INSERTED.[ID] VALUES (@AnfangLagerBestand,@Anzahl,@Anzahl_nach,@Artikel_nr,@Artikel_nr_nach,@Bemerkung,@Bezeichnung_1,@Bezeichnung_1_nach,@Einheit,@EndeLagerBestand,@Fertigungsnummer,@Gebucht_von,@Grund,@ID_Schneiderei,@Karton_ID,@Lager_nach,@Lager_von,@Lagerbewegungen_id,@Loschen,@Preiseinheit,@receivedQuantity,@Rollennummer,@STRG_SCAN,@Umlaufartikel,@WereingangId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Anzahl_nach", item.Anzahl_nach == null ? (object)DBNull.Value : item.Anzahl_nach);
			sqlCommand.Parameters.AddWithValue("Artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
			sqlCommand.Parameters.AddWithValue("Artikel_nr_nach", item.Artikel_nr_nach == null ? (object)DBNull.Value : item.Artikel_nr_nach);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1_nach", item.Bezeichnung_1_nach == null ? (object)DBNull.Value : item.Bezeichnung_1_nach);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Gebucht_von", item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
			sqlCommand.Parameters.AddWithValue("Grund", item.Grund == null ? (object)DBNull.Value : item.Grund);
			sqlCommand.Parameters.AddWithValue("ID_Schneiderei", item.ID_Schneiderei == null ? (object)DBNull.Value : item.ID_Schneiderei);
			sqlCommand.Parameters.AddWithValue("Karton_ID", item.Karton_ID == null ? (object)DBNull.Value : item.Karton_ID);
			sqlCommand.Parameters.AddWithValue("Lager_nach", item.Lager_nach == null ? (object)DBNull.Value : item.Lager_nach);
			sqlCommand.Parameters.AddWithValue("Lager_von", item.Lager_von == null ? (object)DBNull.Value : item.Lager_von);
			sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", item.Lagerbewegungen_id == null ? (object)DBNull.Value : item.Lagerbewegungen_id);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("receivedQuantity", item.receivedQuantity == null ? (object)DBNull.Value : item.receivedQuantity);
			sqlCommand.Parameters.AddWithValue("Rollennummer", item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
			sqlCommand.Parameters.AddWithValue("STRG_SCAN", item.STRG_SCAN == null ? (object)DBNull.Value : item.STRG_SCAN);
			sqlCommand.Parameters.AddWithValue("Umlaufartikel", item.Umlaufartikel == null ? (object)DBNull.Value : item.Umlaufartikel);
			sqlCommand.Parameters.AddWithValue("WereingangId", item.WereingangId == null ? (object)DBNull.Value : item.WereingangId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber =  Settings.MAX_BATCH_SIZE / 27; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Lagerbewegungen_Artikel] ([AnfangLagerBestand],[Anzahl],[Anzahl_nach],[Artikel-nr],[Artikel-nr_nach],[Bemerkung],[Bezeichnung 1],[Bezeichnung 1_nach],[Einheit],[EndeLagerBestand],[Fertigungsnummer],[Gebucht von],[Grund],[ID_Schneiderei],[Karton_ID],[Lager_nach],[Lager_von],[Lagerbewegungen_id],[Löschen],[Preiseinheit],[receivedQuantity],[Rollennummer],[STRG_SCAN],[Umlaufartikel],[WereingangId]) VALUES ( "

						+ "@AnfangLagerBestand" + i + ","
						+ "@Anzahl" + i + ","
						+ "@Anzahl_nach" + i + ","
						+ "@Artikel_nr" + i + ","
						+ "@Artikel_nr_nach" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@Bezeichnung_1" + i + ","
						+ "@Bezeichnung_1_nach" + i + ","
						+ "@Einheit" + i + ","
						+ "@EndeLagerBestand" + i + ","
						+ "@Fertigungsnummer" + i + ","
						+ "@Gebucht_von" + i + ","
						+ "@Grund" + i + ","
						+ "@ID_Schneiderei" + i + ","
						+ "@Karton_ID" + i + ","
						+ "@Lager_nach" + i + ","
						+ "@Lager_von" + i + ","
						+ "@Lagerbewegungen_id" + i + ","
						+ "@Loschen" + i + ","
						+ "@Preiseinheit" + i + ","
						+ "@receivedQuantity" + i + ","
						+ "@Rollennummer" + i + ","
						+ "@STRG_SCAN" + i + ","
						+ "@Umlaufartikel" + i + ","
						+ "@WereingangId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Anzahl_nach" + i, item.Anzahl_nach == null ? (object)DBNull.Value : item.Anzahl_nach);
					sqlCommand.Parameters.AddWithValue("Artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
					sqlCommand.Parameters.AddWithValue("Artikel_nr_nach" + i, item.Artikel_nr_nach == null ? (object)DBNull.Value : item.Artikel_nr_nach);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1_nach" + i, item.Bezeichnung_1_nach == null ? (object)DBNull.Value : item.Bezeichnung_1_nach);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Gebucht_von" + i, item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
					sqlCommand.Parameters.AddWithValue("Grund" + i, item.Grund == null ? (object)DBNull.Value : item.Grund);
					sqlCommand.Parameters.AddWithValue("ID_Schneiderei" + i, item.ID_Schneiderei == null ? (object)DBNull.Value : item.ID_Schneiderei);
					sqlCommand.Parameters.AddWithValue("Karton_ID" + i, item.Karton_ID == null ? (object)DBNull.Value : item.Karton_ID);
					sqlCommand.Parameters.AddWithValue("Lager_nach" + i, item.Lager_nach == null ? (object)DBNull.Value : item.Lager_nach);
					sqlCommand.Parameters.AddWithValue("Lager_von" + i, item.Lager_von == null ? (object)DBNull.Value : item.Lager_von);
					sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id" + i, item.Lagerbewegungen_id == null ? (object)DBNull.Value : item.Lagerbewegungen_id);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("receivedQuantity" + i, item.receivedQuantity == null ? (object)DBNull.Value : item.receivedQuantity);
					sqlCommand.Parameters.AddWithValue("Rollennummer" + i, item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
					sqlCommand.Parameters.AddWithValue("STRG_SCAN" + i, item.STRG_SCAN == null ? (object)DBNull.Value : item.STRG_SCAN);
					sqlCommand.Parameters.AddWithValue("Umlaufartikel" + i, item.Umlaufartikel == null ? (object)DBNull.Value : item.Umlaufartikel);
					sqlCommand.Parameters.AddWithValue("WereingangId" + i, item.WereingangId == null ? (object)DBNull.Value : item.WereingangId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Lagerbewegungen_Artikel] SET [AnfangLagerBestand]=@AnfangLagerBestand, [Anzahl]=@Anzahl, [Anzahl_nach]=@Anzahl_nach, [Artikel-nr]=@Artikel_nr, [Artikel-nr_nach]=@Artikel_nr_nach, [Bemerkung]=@Bemerkung, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 1_nach]=@Bezeichnung_1_nach, [Einheit]=@Einheit, [EndeLagerBestand]=@EndeLagerBestand, [Fertigungsnummer]=@Fertigungsnummer, [Gebucht von]=@Gebucht_von, [Grund]=@Grund, [ID_Schneiderei]=@ID_Schneiderei, [Karton_ID]=@Karton_ID, [Lager_nach]=@Lager_nach, [Lager_von]=@Lager_von, [Lagerbewegungen_id]=@Lagerbewegungen_id, [Löschen]=@Loschen, [Preiseinheit]=@Preiseinheit, [receivedQuantity]=@receivedQuantity, [Rollennummer]=@Rollennummer, [STRG_SCAN]=@STRG_SCAN, [Umlaufartikel]=@Umlaufartikel, [WereingangId]=@WereingangId WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Anzahl_nach", item.Anzahl_nach == null ? (object)DBNull.Value : item.Anzahl_nach);
			sqlCommand.Parameters.AddWithValue("Artikel_nr", item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
			sqlCommand.Parameters.AddWithValue("Artikel_nr_nach", item.Artikel_nr_nach == null ? (object)DBNull.Value : item.Artikel_nr_nach);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1_nach", item.Bezeichnung_1_nach == null ? (object)DBNull.Value : item.Bezeichnung_1_nach);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Gebucht_von", item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
			sqlCommand.Parameters.AddWithValue("Grund", item.Grund == null ? (object)DBNull.Value : item.Grund);
			sqlCommand.Parameters.AddWithValue("ID_Schneiderei", item.ID_Schneiderei == null ? (object)DBNull.Value : item.ID_Schneiderei);
			sqlCommand.Parameters.AddWithValue("Karton_ID", item.Karton_ID == null ? (object)DBNull.Value : item.Karton_ID);
			sqlCommand.Parameters.AddWithValue("Lager_nach", item.Lager_nach == null ? (object)DBNull.Value : item.Lager_nach);
			sqlCommand.Parameters.AddWithValue("Lager_von", item.Lager_von == null ? (object)DBNull.Value : item.Lager_von);
			sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id", item.Lagerbewegungen_id == null ? (object)DBNull.Value : item.Lagerbewegungen_id);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("receivedQuantity", item.receivedQuantity == null ? (object)DBNull.Value : item.receivedQuantity);
			sqlCommand.Parameters.AddWithValue("Rollennummer", item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
			sqlCommand.Parameters.AddWithValue("STRG_SCAN", item.STRG_SCAN == null ? (object)DBNull.Value : item.STRG_SCAN);
			sqlCommand.Parameters.AddWithValue("Umlaufartikel", item.Umlaufartikel == null ? (object)DBNull.Value : item.Umlaufartikel);
			sqlCommand.Parameters.AddWithValue("WereingangId", item.WereingangId == null ? (object)DBNull.Value : item.WereingangId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber =  Settings.MAX_BATCH_SIZE / 27; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Lagerbewegungen_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Lagerbewegungen_Artikel] SET "

					+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[Anzahl_nach]=@Anzahl_nach" + i + ","
					+ "[Artikel-nr]=@Artikel_nr" + i + ","
					+ "[Artikel-nr_nach]=@Artikel_nr_nach" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
					+ "[Bezeichnung 1_nach]=@Bezeichnung_1_nach" + i + ","
					+ "[Einheit]=@Einheit" + i + ","
					+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
					+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
					+ "[Gebucht von]=@Gebucht_von" + i + ","
					+ "[Grund]=@Grund" + i + ","
					+ "[ID_Schneiderei]=@ID_Schneiderei" + i + ","
					+ "[Karton_ID]=@Karton_ID" + i + ","
					+ "[Lager_nach]=@Lager_nach" + i + ","
					+ "[Lager_von]=@Lager_von" + i + ","
					+ "[Lagerbewegungen_id]=@Lagerbewegungen_id" + i + ","
					+ "[Löschen]=@Loschen" + i + ","
					+ "[Preiseinheit]=@Preiseinheit" + i + ","
					+ "[receivedQuantity]=@receivedQuantity" + i + ","
					+ "[Rollennummer]=@Rollennummer" + i + ","
					+ "[STRG_SCAN]=@STRG_SCAN" + i + ","
					+ "[Umlaufartikel]=@Umlaufartikel" + i + ","
					+ "[WereingangId]=@WereingangId" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Anzahl_nach" + i, item.Anzahl_nach == null ? (object)DBNull.Value : item.Anzahl_nach);
					sqlCommand.Parameters.AddWithValue("Artikel_nr" + i, item.Artikel_nr == null ? (object)DBNull.Value : item.Artikel_nr);
					sqlCommand.Parameters.AddWithValue("Artikel_nr_nach" + i, item.Artikel_nr_nach == null ? (object)DBNull.Value : item.Artikel_nr_nach);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1_nach" + i, item.Bezeichnung_1_nach == null ? (object)DBNull.Value : item.Bezeichnung_1_nach);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Gebucht_von" + i, item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
					sqlCommand.Parameters.AddWithValue("Grund" + i, item.Grund == null ? (object)DBNull.Value : item.Grund);
					sqlCommand.Parameters.AddWithValue("ID_Schneiderei" + i, item.ID_Schneiderei == null ? (object)DBNull.Value : item.ID_Schneiderei);
					sqlCommand.Parameters.AddWithValue("Karton_ID" + i, item.Karton_ID == null ? (object)DBNull.Value : item.Karton_ID);
					sqlCommand.Parameters.AddWithValue("Lager_nach" + i, item.Lager_nach == null ? (object)DBNull.Value : item.Lager_nach);
					sqlCommand.Parameters.AddWithValue("Lager_von" + i, item.Lager_von == null ? (object)DBNull.Value : item.Lager_von);
					sqlCommand.Parameters.AddWithValue("Lagerbewegungen_id" + i, item.Lagerbewegungen_id == null ? (object)DBNull.Value : item.Lagerbewegungen_id);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("receivedQuantity" + i, item.receivedQuantity == null ? (object)DBNull.Value : item.receivedQuantity);
					sqlCommand.Parameters.AddWithValue("Rollennummer" + i, item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
					sqlCommand.Parameters.AddWithValue("STRG_SCAN" + i, item.STRG_SCAN == null ? (object)DBNull.Value : item.STRG_SCAN);
					sqlCommand.Parameters.AddWithValue("Umlaufartikel" + i, item.Umlaufartikel == null ? (object)DBNull.Value : item.Umlaufartikel);
					sqlCommand.Parameters.AddWithValue("WereingangId" + i, item.WereingangId == null ? (object)DBNull.Value : item.WereingangId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Lagerbewegungen_Artikel] WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ID", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber =  Settings.MAX_BATCH_SIZE;
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

				string query = "DELETE FROM [Lagerbewegungen_Artikel] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static int GetTransferdQuantity(int WereingangId,int Lager_nach)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select sum(Anzahl) as TransferedQuantity from Lagerbewegungen_Artikel where WereingangId=@WereingangId and Lager_nach=@Lager_nach group by WereingangId , [Artikel-nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("WereingangId", WereingangId);
				sqlCommand.Parameters.AddWithValue("Lager_nach", Lager_nach);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0]["TransferedQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(dataTable.Rows[0]["TransferedQuantity"]);
			}
			else
			{
				return 0;
			}
		}
		public static int GeReceivedQuantity(int WereingangId, int ID)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select Sum(receivedQuantity) as receivedQuantity from Lagerbewegungen_Artikel where WereingangId=@WereingangId and ID=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("WereingangId", WereingangId);
				sqlCommand.Parameters.AddWithValue("ID", ID);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0]["receivedQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(dataTable.Rows[0]["receivedQuantity"]);
			}
			else
			{
				return 0;
			}
		}

		public static int UpdateReceivedQuantityWithTransaction(int ID, decimal? UbertrageneMenge, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = @"
			DECLARE 	@Quantity Float = 0;
			SET @Quantity = ( SELECT  ISNULL(receivedQuantity,0) FROM  [Lagerbewegungen_Artikel] WHERE ID= @ID  ) +  @UbertrageneMenge ;
			UPDATE [Lagerbewegungen_Artikel] SET   [receivedQuantity]=@Quantity  WHERE ID=@ID 
			";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", ID);
			sqlCommand.Parameters.AddWithValue("UbertrageneMenge", UbertrageneMenge == null ? (object)DBNull.Value : UbertrageneMenge);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		#endregion
	}
}
