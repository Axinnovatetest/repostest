using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Fertigung_PositionenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung_Positionen] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung_Positionen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Fertigung_Positionen] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[Vorgang_Nr])  VALUES (@Anzahl,@Arbeitsanweisung,@Artikel_Nr,@Bemerkungen,@buchen,@Fertiger,@Fertigstellung_Ist,@ID_Fertigung,@ID_Fertigung_HL,@Lagerort_ID,@Löschen,@ME_gebucht,@Termin_Soll,@Vorgang_Nr); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Arbeitsanweisung", item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("buchen", item.buchen == null ? (object)DBNull.Value : item.buchen);
					sqlCommand.Parameters.AddWithValue("Fertiger", item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
					sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist", item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("ME_gebucht", item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
					sqlCommand.Parameters.AddWithValue("Termin_Soll", item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> items)
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
						query += " INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[Vorgang_Nr]) VALUES ( "

							+ "@Anzahl" + i + ","
							+ "@Arbeitsanweisung" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@buchen" + i + ","
							+ "@Fertiger" + i + ","
							+ "@Fertigstellung_Ist" + i + ","
							+ "@ID_Fertigung" + i + ","
							+ "@ID_Fertigung_HL" + i + ","
							+ "@Lagerort_ID" + i + ","
							+ "@Löschen" + i + ","
							+ "@ME_gebucht" + i + ","
							+ "@Termin_Soll" + i + ","
							+ "@Vorgang_Nr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Arbeitsanweisung" + i, item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("buchen" + i, item.buchen == null ? (object)DBNull.Value : item.buchen);
						sqlCommand.Parameters.AddWithValue("Fertiger" + i, item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
						sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist" + i, item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("ME_gebucht" + i, item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
						sqlCommand.Parameters.AddWithValue("Termin_Soll" + i, item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
						sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Fertigung_Positionen] SET [Anzahl]=@Anzahl, [Arbeitsanweisung]=@Arbeitsanweisung, [Artikel_Nr]=@Artikel_Nr, [Bemerkungen]=@Bemerkungen, [buchen]=@buchen, [Fertiger]=@Fertiger, [Fertigstellung_Ist]=@Fertigstellung_Ist, [ID_Fertigung]=@ID_Fertigung, [ID_Fertigung_HL]=@ID_Fertigung_HL, [Lagerort_ID]=@Lagerort_ID, [Löschen]=@Löschen, [ME gebucht]=@ME_gebucht, [Termin_Soll]=@Termin_Soll, [Vorgang_Nr]=@Vorgang_Nr WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Arbeitsanweisung", item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("buchen", item.buchen == null ? (object)DBNull.Value : item.buchen);
				sqlCommand.Parameters.AddWithValue("Fertiger", item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
				sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist", item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
				sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
				sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
				sqlCommand.Parameters.AddWithValue("ME_gebucht", item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
				sqlCommand.Parameters.AddWithValue("Termin_Soll", item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
				sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> items)
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
							+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
							+ "[Löschen]=@Löschen" + i + ","
							+ "[ME gebucht]=@ME_gebucht" + i + ","
							+ "[Termin_Soll]=@Termin_Soll" + i + ","
							+ "[Vorgang_Nr]=@Vorgang_Nr" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Arbeitsanweisung" + i, item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("buchen" + i, item.buchen == null ? (object)DBNull.Value : item.buchen);
						sqlCommand.Parameters.AddWithValue("Fertiger" + i, item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
						sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist" + i, item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("ME_gebucht" + i, item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
						sqlCommand.Parameters.AddWithValue("Termin_Soll" + i, item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					string query = "DELETE FROM [Fertigung_Positionen] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity> GetByFertigung(int FAId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigung_Positionen] WHERE [ID_Fertigung]=@FAId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("FAId", FAId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PositionenEntity>();
			}
		}

		#endregion
	}
}
