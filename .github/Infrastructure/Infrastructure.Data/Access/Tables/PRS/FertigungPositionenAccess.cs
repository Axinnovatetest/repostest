using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class FertigungPositionenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Fertigung_Positionen] WHERE [ID]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Fertigung_Positionen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> results = new List<Entities.Tables.PRS.FertigungPositionenEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [Fertigung_Positionen] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[IsUBG],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[UBGFertigungsId],[UBGFertigungsnummer],[Vorgang_Nr]) OUTPUT INSERTED.[ID] VALUES (@Anzahl,@Arbeitsanweisung,@Artikel_Nr,@Bemerkungen,@buchen,@Fertiger,@Fertigstellung_Ist,@ID_Fertigung,@ID_Fertigung_HL,@IsUBG,@Lagerort_ID,@Loschen,@ME_gebucht,@Termin_Soll,@UBGFertigungsId,@UBGFertigungsnummer,@Vorgang_Nr); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Arbeitsanweisung", item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("buchen", item.Buchen == null ? (object)DBNull.Value : item.Buchen);
					sqlCommand.Parameters.AddWithValue("Fertiger", item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
					sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist", item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
					sqlCommand.Parameters.AddWithValue("IsUBG", item.IsUBG == null ? (object)DBNull.Value : item.IsUBG);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("ME_gebucht", item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
					sqlCommand.Parameters.AddWithValue("Termin_Soll", item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
					sqlCommand.Parameters.AddWithValue("UBGFertigungsId", item.UBGFertigungsId == null ? (object)DBNull.Value : item.UBGFertigungsId);
					sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer", item.UBGFertigungsnummer == null ? (object)DBNull.Value : item.UBGFertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items)
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
						query += " INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[IsUBG],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[UBGFertigungsId],[UBGFertigungsnummer],[Vorgang_Nr]) VALUES ( "

							+ "@Anzahl" + i + ","
							+ "@Arbeitsanweisung" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@buchen" + i + ","
							+ "@Fertiger" + i + ","
							+ "@Fertigstellung_Ist" + i + ","
							+ "@ID_Fertigung" + i + ","
							+ "@ID_Fertigung_HL" + i + ","
							+ "@IsUBG" + i + ","
							+ "@Lagerort_ID" + i + ","
							+ "@Loschen" + i + ","
							+ "@ME_gebucht" + i + ","
							+ "@Termin_Soll" + i + ","
							+ "@UBGFertigungsId" + i + ","
							+ "@UBGFertigungsnummer" + i + ","
							+ "@Vorgang_Nr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Arbeitsanweisung" + i, item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("buchen" + i, item.Buchen == null ? (object)DBNull.Value : item.Buchen);
						sqlCommand.Parameters.AddWithValue("Fertiger" + i, item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
						sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist" + i, item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
						sqlCommand.Parameters.AddWithValue("IsUBG" + i, item.IsUBG == null ? (object)DBNull.Value : item.IsUBG);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("ME_gebucht" + i, item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
						sqlCommand.Parameters.AddWithValue("Termin_Soll" + i, item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
						sqlCommand.Parameters.AddWithValue("UBGFertigungsId" + i, item.UBGFertigungsId == null ? (object)DBNull.Value : item.UBGFertigungsId);
						sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer" + i, item.UBGFertigungsnummer == null ? (object)DBNull.Value : item.UBGFertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Fertigung_Positionen] SET [Anzahl]=@Anzahl, [Arbeitsanweisung]=@Arbeitsanweisung, [Artikel_Nr]=@Artikel_Nr, [Bemerkungen]=@Bemerkungen, [buchen]=@buchen, [Fertiger]=@Fertiger, [Fertigstellung_Ist]=@Fertigstellung_Ist, [ID_Fertigung]=@ID_Fertigung, [ID_Fertigung_HL]=@ID_Fertigung_HL, [IsUBG]=@IsUBG, [Lagerort_ID]=@Lagerort_ID, [Löschen]=@Loschen, [ME gebucht]=@ME_gebucht, [Termin_Soll]=@Termin_Soll, [UBGFertigungsId]=@UBGFertigungsId, [UBGFertigungsnummer]=@UBGFertigungsnummer, [Vorgang_Nr]=@Vorgang_Nr WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Arbeitsanweisung", item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("buchen", item.Buchen == null ? (object)DBNull.Value : item.Buchen);
				sqlCommand.Parameters.AddWithValue("Fertiger", item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
				sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist", item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
				sqlCommand.Parameters.AddWithValue("IsUBG", item.IsUBG == null ? (object)DBNull.Value : item.IsUBG);
				sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
				sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
				sqlCommand.Parameters.AddWithValue("ME_gebucht", item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
				sqlCommand.Parameters.AddWithValue("Termin_Soll", item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
				sqlCommand.Parameters.AddWithValue("UBGFertigungsId", item.UBGFertigungsId == null ? (object)DBNull.Value : item.UBGFertigungsId);
				sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer", item.UBGFertigungsnummer == null ? (object)DBNull.Value : item.UBGFertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items)
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
						query += " UPDATE [Fertigung_Positionen] SET "

							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Arbeitsanweisung]=@Arbeitsanweisung" + i + ","
							+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
							+ "[Bemerkungen]=@Bemerkungen" + i + ","
							+ "[buchen]=@buchen" + i + ","
							+ "[Fertiger]=@Fertiger" + i + ","
							+ "[Fertigstellung_Ist]=@Fertigstellung_Ist" + i + ","
							+ "[ID_Fertigung]=@ID_Fertigung" + i + ","
							+ "[ID_Fertigung_HL]=@ID_Fertigung_HL" + i + ","
							+ "[IsUBG]=@IsUBG" + i + ","
							+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
							+ "[Löschen]=@Loschen" + i + ","
							+ "[ME gebucht]=@ME_gebucht" + i + ","
							+ "[Termin_Soll]=@Termin_Soll" + i + ","
							+ "[UBGFertigungsId]=@UBGFertigungsId" + i + ","
							+ "[UBGFertigungsnummer]=@UBGFertigungsnummer" + i + ","
							+ "[Vorgang_Nr]=@Vorgang_Nr" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Arbeitsanweisung" + i, item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("buchen" + i, item.Buchen == null ? (object)DBNull.Value : item.Buchen);
						sqlCommand.Parameters.AddWithValue("Fertiger" + i, item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
						sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist" + i, item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
						sqlCommand.Parameters.AddWithValue("IsUBG" + i, item.IsUBG == null ? (object)DBNull.Value : item.IsUBG);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("ME_gebucht" + i, item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
						sqlCommand.Parameters.AddWithValue("Termin_Soll" + i, item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
						sqlCommand.Parameters.AddWithValue("UBGFertigungsId" + i, item.UBGFertigungsId == null ? (object)DBNull.Value : item.UBGFertigungsId);
						sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer" + i, item.UBGFertigungsnummer == null ? (object)DBNull.Value : item.UBGFertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
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
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Fertigung_Positionen] WHERE [ID]=@ID";
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
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [Fertigung_Positionen] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Fertigung_Positionen] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Fertigung_Positionen]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Fertigung_Positionen] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[IsUBG],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[UBGFertigungsId],[UBGFertigungsnummer],[Vorgang_Nr]) OUTPUT INSERTED.[ID] VALUES (@Anzahl,@Arbeitsanweisung,@Artikel_Nr,@Bemerkungen,@buchen,@Fertiger,@Fertigstellung_Ist,@ID_Fertigung,@ID_Fertigung_HL,@IsUBG,@Lagerort_ID,@Loschen,@ME_gebucht,@Termin_Soll,@UBGFertigungsId,@UBGFertigungsnummer,@Vorgang_Nr); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Arbeitsanweisung", item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("buchen", item.Buchen == null ? (object)DBNull.Value : item.Buchen);
			sqlCommand.Parameters.AddWithValue("Fertiger", item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
			sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist", item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
			sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
			sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
			sqlCommand.Parameters.AddWithValue("IsUBG", item.IsUBG == null ? (object)DBNull.Value : item.IsUBG);
			sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
			sqlCommand.Parameters.AddWithValue("ME_gebucht", item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
			sqlCommand.Parameters.AddWithValue("Termin_Soll", item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
			sqlCommand.Parameters.AddWithValue("UBGFertigungsId", item.UBGFertigungsId == null ? (object)DBNull.Value : item.UBGFertigungsId);
			sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer", item.UBGFertigungsnummer == null ? (object)DBNull.Value : item.UBGFertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[IsUBG],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[UBGFertigungsId],[UBGFertigungsnummer],[Vorgang_Nr]) VALUES ( "

						+ "@Anzahl" + i + ","
						+ "@Arbeitsanweisung" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Bemerkungen" + i + ","
						+ "@buchen" + i + ","
						+ "@Fertiger" + i + ","
						+ "@Fertigstellung_Ist" + i + ","
						+ "@ID_Fertigung" + i + ","
						+ "@ID_Fertigung_HL" + i + ","
						+ "@IsUBG" + i + ","
						+ "@Lagerort_ID" + i + ","
						+ "@Loschen" + i + ","
						+ "@ME_gebucht" + i + ","
						+ "@Termin_Soll" + i + ","
						+ "@UBGFertigungsId" + i + ","
						+ "@UBGFertigungsnummer" + i + ","
						+ "@Vorgang_Nr" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Arbeitsanweisung" + i, item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("buchen" + i, item.Buchen == null ? (object)DBNull.Value : item.Buchen);
					sqlCommand.Parameters.AddWithValue("Fertiger" + i, item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
					sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist" + i, item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
					sqlCommand.Parameters.AddWithValue("IsUBG" + i, item.IsUBG == null ? (object)DBNull.Value : item.IsUBG);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("ME_gebucht" + i, item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
					sqlCommand.Parameters.AddWithValue("Termin_Soll" + i, item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
					sqlCommand.Parameters.AddWithValue("UBGFertigungsId" + i, item.UBGFertigungsId == null ? (object)DBNull.Value : item.UBGFertigungsId);
					sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer" + i, item.UBGFertigungsnummer == null ? (object)DBNull.Value : item.UBGFertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Fertigung_Positionen] SET [Anzahl]=@Anzahl, [Arbeitsanweisung]=@Arbeitsanweisung, [Artikel_Nr]=@Artikel_Nr, [Bemerkungen]=@Bemerkungen, [buchen]=@buchen, [Fertiger]=@Fertiger, [Fertigstellung_Ist]=@Fertigstellung_Ist, [ID_Fertigung]=@ID_Fertigung, [ID_Fertigung_HL]=@ID_Fertigung_HL, [IsUBG]=@IsUBG, [Lagerort_ID]=@Lagerort_ID, [Löschen]=@Loschen, [ME gebucht]=@ME_gebucht, [Termin_Soll]=@Termin_Soll, [UBGFertigungsId]=@UBGFertigungsId, [UBGFertigungsnummer]=@UBGFertigungsnummer, [Vorgang_Nr]=@Vorgang_Nr WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Arbeitsanweisung", item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("buchen", item.Buchen == null ? (object)DBNull.Value : item.Buchen);
			sqlCommand.Parameters.AddWithValue("Fertiger", item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
			sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist", item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
			sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
			sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
			sqlCommand.Parameters.AddWithValue("IsUBG", item.IsUBG == null ? (object)DBNull.Value : item.IsUBG);
			sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
			sqlCommand.Parameters.AddWithValue("ME_gebucht", item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
			sqlCommand.Parameters.AddWithValue("Termin_Soll", item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
			sqlCommand.Parameters.AddWithValue("UBGFertigungsId", item.UBGFertigungsId == null ? (object)DBNull.Value : item.UBGFertigungsId);
			sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer", item.UBGFertigungsnummer == null ? (object)DBNull.Value : item.UBGFertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Fertigung_Positionen] SET "

					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[Arbeitsanweisung]=@Arbeitsanweisung" + i + ","
					+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
					+ "[Bemerkungen]=@Bemerkungen" + i + ","
					+ "[buchen]=@buchen" + i + ","
					+ "[Fertiger]=@Fertiger" + i + ","
					+ "[Fertigstellung_Ist]=@Fertigstellung_Ist" + i + ","
					+ "[ID_Fertigung]=@ID_Fertigung" + i + ","
					+ "[ID_Fertigung_HL]=@ID_Fertigung_HL" + i + ","
					+ "[IsUBG]=@IsUBG" + i + ","
					+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
					+ "[Löschen]=@Loschen" + i + ","
					+ "[ME gebucht]=@ME_gebucht" + i + ","
					+ "[Termin_Soll]=@Termin_Soll" + i + ","
					+ "[UBGFertigungsId]=@UBGFertigungsId" + i + ","
					+ "[UBGFertigungsnummer]=@UBGFertigungsnummer" + i + ","
					+ "[Vorgang_Nr]=@Vorgang_Nr" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Arbeitsanweisung" + i, item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("buchen" + i, item.Buchen == null ? (object)DBNull.Value : item.Buchen);
					sqlCommand.Parameters.AddWithValue("Fertiger" + i, item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
					sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist" + i, item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
					sqlCommand.Parameters.AddWithValue("IsUBG" + i, item.IsUBG == null ? (object)DBNull.Value : item.IsUBG);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("ME_gebucht" + i, item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
					sqlCommand.Parameters.AddWithValue("Termin_Soll" + i, item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
					sqlCommand.Parameters.AddWithValue("UBGFertigungsId" + i, item.UBGFertigungsId == null ? (object)DBNull.Value : item.UBGFertigungsId);
					sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer" + i, item.UBGFertigungsnummer == null ? (object)DBNull.Value : item.UBGFertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Fertigung_Positionen] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [Fertigung_Positionen] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> GetByIdFertigung(int fertigungId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Fertigung_Positionen] WHERE [ID_Fertigung]=@fertigungId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fertigungId", fertigungId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> GetByIdFertigung(List<int> fertigungIds)
		{
			if(fertigungIds == null || fertigungIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Fertigung_Positionen] WHERE [ID_Fertigung] IN ({string.Join(",", fertigungIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> GetByIdFertigung(int fertigungId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Fertigung_Positionen] WHERE [ID_Fertigung]=@fertigungId";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("fertigungId", fertigungId);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> GetByIdFertigung(List<int> fertigungIds, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(fertigungIds == null || fertigungIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [Fertigung_Positionen] WHERE [ID_Fertigung] IN ({string.Join(",", fertigungIds)})";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
			}
		}
		public static int DeleteByIdFertigung(int fertigungId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Fertigung_Positionen] WHERE [ID_Fertigung_HL]=@fertigungId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fertigungId", fertigungId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int DeleteByIdFertigung(int fertigungId, SqlConnection sqlConnection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [Fertigung_Positionen] WHERE [ID_Fertigung_HL]=@fertigungId";
			var sqlCommand = new SqlCommand(query, sqlConnection, transaction);
			sqlCommand.Parameters.AddWithValue("fertigungId", fertigungId);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int DeleteByIdFertigung(List<int> fertigungIds, SqlConnection sqlConnection, SqlTransaction transaction)
		{
			if(fertigungIds == null || fertigungIds.Count <= 0)
				return 0;

			string query = $"DELETE FROM [Fertigung_Positionen] WHERE [ID_Fertigung] IN ({string.Join(",", fertigungIds)})";
			var sqlCommand = new SqlCommand(query, sqlConnection, transaction);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> GetAvailableHBGByArticleId(int articleId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Fertigung_Positionen] WHERE [Artikel_Nr]=@articleId AND " +
					"ISNULL([Löschen],0)=0 AND IsNULL(UBGFertigungsId,0)=0 AND ISNULL([UBGFertigungsnummer],0)=0 AND [Anzahl]>0";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> GetByArticles(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> results = new List<Entities.Tables.PRS.FertigungPositionenEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					results = getByArticles(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByArticles(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByArticles(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> getByArticles(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [Fertigung_Positionen] WHERE [Artikel_Nr] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>();
		}
		#endregion
		#region Querys with transaction
		//public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity item, SqlConnection conncection, SqlTransaction transaction)
		//{
		//    Debug.WriteLine($"transaction FAPositions insert {transaction.ToString()}");
		//    int response = int.MinValue;
		//    //using (conncection)
		//    //{
		//    //conncection.Open();

		//    //var sqlTransaction = sqlConnection.BeginTransaction();

		//    string query = "INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[Vorgang_Nr])  VALUES (@Anzahl,@Arbeitsanweisung,@Artikel_Nr,@Bemerkungen,@buchen,@Fertiger,@Fertigstellung_Ist,@ID_Fertigung,@ID_Fertigung_HL,@Lagerort_ID,@Löschen,@ME_gebucht,@Termin_Soll,@Vorgang_Nr);";
		//    query += "SELECT SCOPE_IDENTITY();";

		//    //using (var sqlCommand = new SqlCommand(query, conncection, transaction))
		//    //{
		//    var sqlCommand = new SqlCommand(query, conncection, transaction);
		//    sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
		//    sqlCommand.Parameters.AddWithValue("Arbeitsanweisung", item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
		//    sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
		//    sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
		//    sqlCommand.Parameters.AddWithValue("buchen", item.Buchen == null ? (object)DBNull.Value : item.Buchen);
		//    sqlCommand.Parameters.AddWithValue("Fertiger", item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
		//    sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist", item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
		//    sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
		//    sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
		//    sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
		//    sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
		//    sqlCommand.Parameters.AddWithValue("ME_gebucht", item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
		//    sqlCommand.Parameters.AddWithValue("Termin_Soll", item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
		//    sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

		//    var result = DbExecution.ExecuteScalar(sqlCommand);
		//    response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		//    //}

		//    //sqlTransaction.Commit();

		//    return response;
		//    //}
		//}
		//private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items, SqlConnection conncection, SqlTransaction transaction)
		//{
		//    if (items != null && items.Count > 0)
		//    {
		//        int results = -1;
		//        //using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
		//        //{
		//        // sqlConnection.Open();
		//        string query = "";
		//        var sqlCommand = new SqlCommand(query, conncection, transaction);

		//        int i = 0;
		//        foreach (var item in items)
		//        {
		//            i++;
		//            query += " INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[Vorgang_Nr]) VALUES ( "

		//                + "@Anzahl" + i + ","
		//                + "@Arbeitsanweisung" + i + ","
		//                + "@Artikel_Nr" + i + ","
		//                + "@Bemerkungen" + i + ","
		//                + "@buchen" + i + ","
		//                + "@Fertiger" + i + ","
		//                + "@Fertigstellung_Ist" + i + ","
		//                + "@ID_Fertigung" + i + ","
		//                + "@ID_Fertigung_HL" + i + ","
		//                + "@Lagerort_ID" + i + ","
		//                + "@Löschen" + i + ","
		//                + "@ME_gebucht" + i + ","
		//                + "@Termin_Soll" + i + ","
		//                + "@Vorgang_Nr" + i
		//                + "); ";


		//            sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
		//            sqlCommand.Parameters.AddWithValue("Arbeitsanweisung" + i, item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
		//            sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
		//            sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
		//            sqlCommand.Parameters.AddWithValue("buchen" + i, item.Buchen == null ? (object)DBNull.Value : item.Buchen);
		//            sqlCommand.Parameters.AddWithValue("Fertiger" + i, item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
		//            sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist" + i, item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
		//            sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
		//            sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
		//            sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
		//            sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
		//            sqlCommand.Parameters.AddWithValue("ME_gebucht" + i, item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
		//            sqlCommand.Parameters.AddWithValue("Termin_Soll" + i, item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
		//            sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
		//        }

		//        sqlCommand.CommandText = query;

		//        results = DbExecution.ExecuteNonQuery(sqlCommand);
		//        //}

		//        return results;
		//    }

		//    return -1;
		//}
		//public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items, SqlConnection conncection, SqlTransaction transaction)
		//{
		//    if (items != null && items.Count > 0)
		//    {
		//        int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 15; // Nb params per query
		//        int results = 0;
		//        if (items.Count <= maxParamsNumber)
		//        {
		//            results = insertWithTransaction(items, conncection, transaction);
		//        }
		//        else
		//        {
		//            int batchNumber = items.Count / maxParamsNumber;
		//            for (int i = 0; i < batchNumber; i++)
		//            {
		//                results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), conncection, transaction);
		//            }
		//            results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), conncection, transaction);
		//        }
		//        return results;
		//    }

		//    return -1;
		//}
		public static int DeleteByIdFertigungWithTransaction(int fertigungId, SqlConnection conncection, SqlTransaction transaction)
		{
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			string query = "DELETE FROM [Fertigung_Positionen] WHERE [ID_Fertigung_HL]=@fertigungId";
			var sqlCommand = new SqlCommand(query, conncection, transaction);
			sqlCommand.Parameters.AddWithValue("fertigungId", fertigungId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			//}

			return results;
		}
		public static int UpdateUBGIdWithTransaction(int id, int? ubgFaId, int? ubgFaNummer, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Fertigung_Positionen] SET [UBGFertigungsId]=@UBGFertigungsId, [UBGFertigungsnummer]=@UBGFertigungsnummer, [IsUBG]=1 WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", id);
			sqlCommand.Parameters.AddWithValue("UBGFertigungsId", ubgFaId == null ? (object)DBNull.Value : ubgFaId);
			sqlCommand.Parameters.AddWithValue("UBGFertigungsnummer", ubgFaNummer == null ? (object)DBNull.Value : ubgFaNummer);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}

		#endregion
		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
