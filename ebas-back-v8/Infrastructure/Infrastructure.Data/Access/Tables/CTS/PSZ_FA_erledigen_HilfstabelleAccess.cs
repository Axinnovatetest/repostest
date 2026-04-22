using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class PSZ_FA_erledigen_HilfstabelleAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_FA erledigen Hilfstabelle] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_FA erledigen Hilfstabelle]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_FA erledigen Hilfstabelle] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_FA erledigen Hilfstabelle] ([Anzahl],[Anzahl_aktuell],[Anzahl_erledigt],[Artikelnummer],[Bezeichnung 1],[Faktor Material],[Fertigungsnummer],[Kennzeichen],[Lagerort],[Lagerort Entnahme],[Lagerort_id],[Lagerort_id Entnahme],[Mitarbeiter],[Originalanzahl],[Termin_Fertigstellung],[zeit])  VALUES (@Anzahl,@Anzahl_aktuell,@Anzahl_erledigt,@Artikelnummer,@Bezeichnung_1,@Faktor_Material,@Fertigungsnummer,@Kennzeichen,@Lagerort,@Lagerort_Entnahme,@Lagerort_id,@Lagerort_id_Entnahme,@Mitarbeiter,@Originalanzahl,@Termin_Fertigstellung,@zeit); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
					sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Faktor_Material", item.Faktor_Material == null ? (object)DBNull.Value : item.Faktor_Material);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
					sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
					sqlCommand.Parameters.AddWithValue("Lagerort_Entnahme", item.Lagerort_Entnahme == null ? (object)DBNull.Value : item.Lagerort_Entnahme);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Lagerort_id_Entnahme", item.Lagerort_id_Entnahme == null ? (object)DBNull.Value : item.Lagerort_id_Entnahme);
					sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
					sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
					sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
					sqlCommand.Parameters.AddWithValue("zeit", item.zeit == null ? (object)DBNull.Value : item.zeit);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity> items)
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
						query += " INSERT INTO [PSZ_FA erledigen Hilfstabelle] ([Anzahl],[Anzahl_aktuell],[Anzahl_erledigt],[Artikelnummer],[Bezeichnung 1],[Faktor Material],[Fertigungsnummer],[Kennzeichen],[Lagerort],[Lagerort Entnahme],[Lagerort_id],[Lagerort_id Entnahme],[Mitarbeiter],[Originalanzahl],[Termin_Fertigstellung],[zeit]) VALUES ( "

							+ "@Anzahl" + i + ","
							+ "@Anzahl_aktuell" + i + ","
							+ "@Anzahl_erledigt" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Faktor_Material" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@Kennzeichen" + i + ","
							+ "@Lagerort" + i + ","
							+ "@Lagerort_Entnahme" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Lagerort_id_Entnahme" + i + ","
							+ "@Mitarbeiter" + i + ","
							+ "@Originalanzahl" + i + ","
							+ "@Termin_Fertigstellung" + i + ","
							+ "@zeit" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Anzahl_aktuell" + i, item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
						sqlCommand.Parameters.AddWithValue("Anzahl_erledigt" + i, item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Faktor_Material" + i, item.Faktor_Material == null ? (object)DBNull.Value : item.Faktor_Material);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Kennzeichen" + i, item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
						sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
						sqlCommand.Parameters.AddWithValue("Lagerort_Entnahme" + i, item.Lagerort_Entnahme == null ? (object)DBNull.Value : item.Lagerort_Entnahme);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Lagerort_id_Entnahme" + i, item.Lagerort_id_Entnahme == null ? (object)DBNull.Value : item.Lagerort_id_Entnahme);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Originalanzahl" + i, item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
						sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung" + i, item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
						sqlCommand.Parameters.AddWithValue("zeit" + i, item.zeit == null ? (object)DBNull.Value : item.zeit);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_FA erledigen Hilfstabelle] SET [Anzahl]=@Anzahl, [Anzahl_aktuell]=@Anzahl_aktuell, [Anzahl_erledigt]=@Anzahl_erledigt, [Artikelnummer]=@Artikelnummer, [Bezeichnung 1]=@Bezeichnung_1, [Faktor Material]=@Faktor_Material, [Fertigungsnummer]=@Fertigungsnummer, [Kennzeichen]=@Kennzeichen, [Lagerort]=@Lagerort, [Lagerort Entnahme]=@Lagerort_Entnahme, [Lagerort_id]=@Lagerort_id, [Lagerort_id Entnahme]=@Lagerort_id_Entnahme, [Mitarbeiter]=@Mitarbeiter, [Originalanzahl]=@Originalanzahl, [Termin_Fertigstellung]=@Termin_Fertigstellung, [zeit]=@zeit WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
				sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Faktor_Material", item.Faktor_Material == null ? (object)DBNull.Value : item.Faktor_Material);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
				sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
				sqlCommand.Parameters.AddWithValue("Lagerort_Entnahme", item.Lagerort_Entnahme == null ? (object)DBNull.Value : item.Lagerort_Entnahme);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Lagerort_id_Entnahme", item.Lagerort_id_Entnahme == null ? (object)DBNull.Value : item.Lagerort_id_Entnahme);
				sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
				sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
				sqlCommand.Parameters.AddWithValue("zeit", item.zeit == null ? (object)DBNull.Value : item.zeit);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity> items)
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
						query += " UPDATE [PSZ_FA erledigen Hilfstabelle] SET "

							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Anzahl_aktuell]=@Anzahl_aktuell" + i + ","
							+ "[Anzahl_erledigt]=@Anzahl_erledigt" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
							+ "[Faktor Material]=@Faktor_Material" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[Kennzeichen]=@Kennzeichen" + i + ","
							+ "[Lagerort]=@Lagerort" + i + ","
							+ "[Lagerort Entnahme]=@Lagerort_Entnahme" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Lagerort_id Entnahme]=@Lagerort_id_Entnahme" + i + ","
							+ "[Mitarbeiter]=@Mitarbeiter" + i + ","
							+ "[Originalanzahl]=@Originalanzahl" + i + ","
							+ "[Termin_Fertigstellung]=@Termin_Fertigstellung" + i + ","
							+ "[zeit]=@zeit" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Anzahl_aktuell" + i, item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
						sqlCommand.Parameters.AddWithValue("Anzahl_erledigt" + i, item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Faktor_Material" + i, item.Faktor_Material == null ? (object)DBNull.Value : item.Faktor_Material);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Kennzeichen" + i, item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
						sqlCommand.Parameters.AddWithValue("Lagerort" + i, item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
						sqlCommand.Parameters.AddWithValue("Lagerort_Entnahme" + i, item.Lagerort_Entnahme == null ? (object)DBNull.Value : item.Lagerort_Entnahme);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Lagerort_id_Entnahme" + i, item.Lagerort_id_Entnahme == null ? (object)DBNull.Value : item.Lagerort_id_Entnahme);
						sqlCommand.Parameters.AddWithValue("Mitarbeiter" + i, item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
						sqlCommand.Parameters.AddWithValue("Originalanzahl" + i, item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
						sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung" + i, item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
						sqlCommand.Parameters.AddWithValue("zeit" + i, item.zeit == null ? (object)DBNull.Value : item.zeit);
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
				string query = "DELETE FROM [PSZ_FA erledigen Hilfstabelle] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [PSZ_FA erledigen Hilfstabelle] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods



		#endregion
		#region Querys with transaction
		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_FA_erledigen_HilfstabelleEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			//var sqlTransaction = sqlConnection.BeginTransaction();

			string query = "INSERT INTO [PSZ_FA erledigen Hilfstabelle] ([Anzahl],[Anzahl_aktuell],[Anzahl_erledigt],[Artikelnummer],[Bezeichnung 1],[Faktor Material],[Fertigungsnummer],[Kennzeichen],[Lagerort],[Lagerort Entnahme],[Lagerort_id],[Lagerort_id Entnahme],[Mitarbeiter],[Originalanzahl],[Termin_Fertigstellung],[zeit])  VALUES (@Anzahl,@Anzahl_aktuell,@Anzahl_erledigt,@Artikelnummer,@Bezeichnung_1,@Faktor_Material,@Fertigungsnummer,@Kennzeichen,@Lagerort,@Lagerort_Entnahme,@Lagerort_id,@Lagerort_id_Entnahme,@Mitarbeiter,@Originalanzahl,@Termin_Fertigstellung,@zeit); ";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{

				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Anzahl_aktuell", item.Anzahl_aktuell == null ? (object)DBNull.Value : item.Anzahl_aktuell);
				sqlCommand.Parameters.AddWithValue("Anzahl_erledigt", item.Anzahl_erledigt == null ? (object)DBNull.Value : item.Anzahl_erledigt);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Faktor_Material", item.Faktor_Material == null ? (object)DBNull.Value : item.Faktor_Material);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
				sqlCommand.Parameters.AddWithValue("Lagerort", item.Lagerort == null ? (object)DBNull.Value : item.Lagerort);
				sqlCommand.Parameters.AddWithValue("Lagerort_Entnahme", item.Lagerort_Entnahme == null ? (object)DBNull.Value : item.Lagerort_Entnahme);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Lagerort_id_Entnahme", item.Lagerort_id_Entnahme == null ? (object)DBNull.Value : item.Lagerort_id_Entnahme);
				sqlCommand.Parameters.AddWithValue("Mitarbeiter", item.Mitarbeiter == null ? (object)DBNull.Value : item.Mitarbeiter);
				sqlCommand.Parameters.AddWithValue("Originalanzahl", item.Originalanzahl == null ? (object)DBNull.Value : item.Originalanzahl);
				sqlCommand.Parameters.AddWithValue("Termin_Fertigstellung", item.Termin_Fertigstellung == null ? (object)DBNull.Value : item.Termin_Fertigstellung);
				sqlCommand.Parameters.AddWithValue("zeit", item.zeit == null ? (object)DBNull.Value : item.zeit);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
			//sqlTransaction.Commit();

			return response;
			//}
		}
		#endregion
	}
}
