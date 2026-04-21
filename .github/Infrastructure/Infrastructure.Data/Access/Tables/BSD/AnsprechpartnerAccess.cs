using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class AnsprechpartnerAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Ansprechpartner] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Ansprechpartner]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Ansprechpartner] WHERE [Nr] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Ansprechpartner] "
					+ " ([Abteilung],[Anrede],[Ansprechpartner],[auswahl_AB_BW],[Bemerkung],[Briefanrede], "
					+ " [eMail],[FAX],[Geburtstag],[Land],[Mobil],[Nummer],[Ort],[PLZ],[Position],[Serienbrief], "
					+ " [Sprache],[Straße],[Telefon],[Titel],[zu_Händen]) "
					+ " VALUES "
					+ " (@Abteilung,@Anrede,@Ansprechpartner,@auswahl_AB_BW,@Bemerkung,@Briefanrede, "
					+ " @eMail,@FAX,@Geburtstag,@Land,@Mobil,@Nummer,@Ort,@PLZ,@Position,@Serienbrief, "
					+ " @Sprache,@Straße,@Telefon,@Titel,@zu_Händen);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("auswahl_AB_BW", item.Auswahl_AB_BW == null ? (object)DBNull.Value : item.Auswahl_AB_BW);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("eMail", item.EMail == null ? (object)DBNull.Value : item.EMail);
					sqlCommand.Parameters.AddWithValue("FAX", item.FAX == null ? (object)DBNull.Value : item.FAX);
					sqlCommand.Parameters.AddWithValue("Geburtstag", item.Geburtstag == null ? (object)DBNull.Value : item.Geburtstag);
					sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Mobil", item.Mobil == null ? (object)DBNull.Value : item.Mobil);
					sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
					sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("PLZ", item.PLZ == null ? (object)DBNull.Value : item.PLZ);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Serienbrief", item.Serienbrief == null ? (object)DBNull.Value : item.Serienbrief);
					sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
					sqlCommand.Parameters.AddWithValue("Straße", item.StraBe == null ? (object)DBNull.Value : item.StraBe);
					sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
					sqlCommand.Parameters.AddWithValue("zu_Händen", item.Zu_Handen == null ? (object)DBNull.Value : item.Zu_Handen);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity item)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Ansprechpartner] SET "
					+ " [Abteilung]=@Abteilung, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [auswahl_AB_BW]=@auswahl_AB_BW, "
					+ " [Bemerkung]=@Bemerkung, [Briefanrede]=@Briefanrede, [eMail]=@eMail, [FAX]=@FAX, [Geburtstag]=@Geburtstag, "
					+ " [Land]=@Land, [Mobil]=@Mobil, [Nummer]=@Nummer, [Ort]=@Ort, [PLZ]=@PLZ, [Position]=@Position, [Serienbrief]=@Serienbrief, "
					+ " [Sprache]=@Sprache, [Straße]=@Straße, [Telefon]=@Telefon, [Titel]=@Titel, [zu_Händen]=@zu_Händen "
					+ " WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);

				sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
				sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
				sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
				sqlCommand.Parameters.AddWithValue("auswahl_AB_BW", item.Auswahl_AB_BW == null ? (object)DBNull.Value : item.Auswahl_AB_BW);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
				sqlCommand.Parameters.AddWithValue("eMail", item.EMail == null ? (object)DBNull.Value : item.EMail);
				sqlCommand.Parameters.AddWithValue("FAX", item.FAX == null ? (object)DBNull.Value : item.FAX);
				sqlCommand.Parameters.AddWithValue("Geburtstag", item.Geburtstag == null ? (object)DBNull.Value : item.Geburtstag);
				sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
				sqlCommand.Parameters.AddWithValue("Mobil", item.Mobil == null ? (object)DBNull.Value : item.Mobil);
				sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
				sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
				sqlCommand.Parameters.AddWithValue("PLZ", item.PLZ == null ? (object)DBNull.Value : item.PLZ);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("Serienbrief", item.Serienbrief == null ? (object)DBNull.Value : item.Serienbrief);
				sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
				sqlCommand.Parameters.AddWithValue("Straße", item.StraBe == null ? (object)DBNull.Value : item.StraBe);
				sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
				sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
				sqlCommand.Parameters.AddWithValue("zu_Händen", item.Zu_Handen == null ? (object)DBNull.Value : item.Zu_Handen);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Ansprechpartner] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

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

					string query = "DELETE FROM [Ansprechpartner] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Ansprechpartner] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Ansprechpartner]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Ansprechpartner] WHERE [Nr] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Ansprechpartner] ([Abteilung],[Anrede],[Ansprechpartner],[auswahl_AB_BW],[Bemerkung],[Briefanrede],[eMail],[FAX],[Geburtstag],[Land],[Mobil],[Nummer],[Ort],[PLZ],[Position],[Serienbrief],[Sprache],[Straße],[Telefon],[Titel],[zu_Händen]) OUTPUT INSERTED.[Nr] VALUES (@Abteilung,@Anrede,@Ansprechpartner,@auswahl_AB_BW,@Bemerkung,@Briefanrede,@eMail,@FAX,@Geburtstag,@Land,@Mobil,@Nummer,@Ort,@PLZ,@Position,@Serienbrief,@Sprache,@Strasse,@Telefon,@Titel,@zu_Handen); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
			sqlCommand.Parameters.AddWithValue("auswahl_AB_BW", item.Auswahl_AB_BW == null ? (object)DBNull.Value : item.Auswahl_AB_BW);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("eMail", item.EMail == null ? (object)DBNull.Value : item.EMail);
			sqlCommand.Parameters.AddWithValue("FAX", item.FAX == null ? (object)DBNull.Value : item.FAX);
			sqlCommand.Parameters.AddWithValue("Geburtstag", item.Geburtstag == null ? (object)DBNull.Value : item.Geburtstag);
			sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
			sqlCommand.Parameters.AddWithValue("Mobil", item.Mobil == null ? (object)DBNull.Value : item.Mobil);
			sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
			sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
			sqlCommand.Parameters.AddWithValue("PLZ", item.PLZ == null ? (object)DBNull.Value : item.PLZ);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("Serienbrief", item.Serienbrief == null ? (object)DBNull.Value : item.Serienbrief);
			sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
			sqlCommand.Parameters.AddWithValue("Strasse", item.StraBe == null ? (object)DBNull.Value : item.StraBe);
			sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
			sqlCommand.Parameters.AddWithValue("zu_Handen", item.Zu_Handen == null ? (object)DBNull.Value : item.Zu_Handen);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Ansprechpartner] ([Abteilung],[Anrede],[Ansprechpartner],[auswahl_AB_BW],[Bemerkung],[Briefanrede],[eMail],[FAX],[Geburtstag],[Land],[Mobil],[Nummer],[Ort],[PLZ],[Position],[Serienbrief],[Sprache],[Straße],[Telefon],[Titel],[zu_Händen]) VALUES ( "

						+ "@Abteilung" + i + ","
						+ "@Anrede" + i + ","
						+ "@Ansprechpartner" + i + ","
						+ "@auswahl_AB_BW" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@Briefanrede" + i + ","
						+ "@eMail" + i + ","
						+ "@FAX" + i + ","
						+ "@Geburtstag" + i + ","
						+ "@Land" + i + ","
						+ "@Mobil" + i + ","
						+ "@Nummer" + i + ","
						+ "@Ort" + i + ","
						+ "@PLZ" + i + ","
						+ "@Position" + i + ","
						+ "@Serienbrief" + i + ","
						+ "@Sprache" + i + ","
						+ "@Strasse" + i + ","
						+ "@Telefon" + i + ","
						+ "@Titel" + i + ","
						+ "@zu_Handen" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("auswahl_AB_BW" + i, item.Auswahl_AB_BW == null ? (object)DBNull.Value : item.Auswahl_AB_BW);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("eMail" + i, item.EMail == null ? (object)DBNull.Value : item.EMail);
					sqlCommand.Parameters.AddWithValue("FAX" + i, item.FAX == null ? (object)DBNull.Value : item.FAX);
					sqlCommand.Parameters.AddWithValue("Geburtstag" + i, item.Geburtstag == null ? (object)DBNull.Value : item.Geburtstag);
					sqlCommand.Parameters.AddWithValue("Land" + i, item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Mobil" + i, item.Mobil == null ? (object)DBNull.Value : item.Mobil);
					sqlCommand.Parameters.AddWithValue("Nummer" + i, item.Nummer == null ? (object)DBNull.Value : item.Nummer);
					sqlCommand.Parameters.AddWithValue("Ort" + i, item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("PLZ" + i, item.PLZ == null ? (object)DBNull.Value : item.PLZ);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Serienbrief" + i, item.Serienbrief == null ? (object)DBNull.Value : item.Serienbrief);
					sqlCommand.Parameters.AddWithValue("Sprache" + i, item.Sprache == null ? (object)DBNull.Value : item.Sprache);
					sqlCommand.Parameters.AddWithValue("Strasse" + i, item.StraBe == null ? (object)DBNull.Value : item.StraBe);
					sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Titel" + i, item.Titel == null ? (object)DBNull.Value : item.Titel);
					sqlCommand.Parameters.AddWithValue("zu_Handen" + i, item.Zu_Handen == null ? (object)DBNull.Value : item.Zu_Handen);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Ansprechpartner] SET [Abteilung]=@Abteilung, [Anrede]=@Anrede, [Ansprechpartner]=@Ansprechpartner, [auswahl_AB_BW]=@auswahl_AB_BW, [Bemerkung]=@Bemerkung, [Briefanrede]=@Briefanrede, [eMail]=@eMail, [FAX]=@FAX, [Geburtstag]=@Geburtstag, [Land]=@Land, [Mobil]=@Mobil, [Nummer]=@Nummer, [Ort]=@Ort, [PLZ]=@PLZ, [Position]=@Position, [Serienbrief]=@Serienbrief, [Sprache]=@Sprache, [Straße]=@Strasse, [Telefon]=@Telefon, [Titel]=@Titel, [zu_Händen]=@zu_Handen WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Ansprechpartner", item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
			sqlCommand.Parameters.AddWithValue("auswahl_AB_BW", item.Auswahl_AB_BW == null ? (object)DBNull.Value : item.Auswahl_AB_BW);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("eMail", item.EMail == null ? (object)DBNull.Value : item.EMail);
			sqlCommand.Parameters.AddWithValue("FAX", item.FAX == null ? (object)DBNull.Value : item.FAX);
			sqlCommand.Parameters.AddWithValue("Geburtstag", item.Geburtstag == null ? (object)DBNull.Value : item.Geburtstag);
			sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
			sqlCommand.Parameters.AddWithValue("Mobil", item.Mobil == null ? (object)DBNull.Value : item.Mobil);
			sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
			sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
			sqlCommand.Parameters.AddWithValue("PLZ", item.PLZ == null ? (object)DBNull.Value : item.PLZ);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("Serienbrief", item.Serienbrief == null ? (object)DBNull.Value : item.Serienbrief);
			sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
			sqlCommand.Parameters.AddWithValue("Strasse", item.StraBe == null ? (object)DBNull.Value : item.StraBe);
			sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
			sqlCommand.Parameters.AddWithValue("zu_Handen", item.Zu_Handen == null ? (object)DBNull.Value : item.Zu_Handen);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Ansprechpartner] SET "

					+ "[Abteilung]=@Abteilung" + i + ","
					+ "[Anrede]=@Anrede" + i + ","
					+ "[Ansprechpartner]=@Ansprechpartner" + i + ","
					+ "[auswahl_AB_BW]=@auswahl_AB_BW" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Briefanrede]=@Briefanrede" + i + ","
					+ "[eMail]=@eMail" + i + ","
					+ "[FAX]=@FAX" + i + ","
					+ "[Geburtstag]=@Geburtstag" + i + ","
					+ "[Land]=@Land" + i + ","
					+ "[Mobil]=@Mobil" + i + ","
					+ "[Nummer]=@Nummer" + i + ","
					+ "[Ort]=@Ort" + i + ","
					+ "[PLZ]=@PLZ" + i + ","
					+ "[Position]=@Position" + i + ","
					+ "[Serienbrief]=@Serienbrief" + i + ","
					+ "[Sprache]=@Sprache" + i + ","
					+ "[Straße]=@Strasse" + i + ","
					+ "[Telefon]=@Telefon" + i + ","
					+ "[Titel]=@Titel" + i + ","
					+ "[zu_Händen]=@zu_Handen" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Ansprechpartner" + i, item.Ansprechpartner == null ? (object)DBNull.Value : item.Ansprechpartner);
					sqlCommand.Parameters.AddWithValue("auswahl_AB_BW" + i, item.Auswahl_AB_BW == null ? (object)DBNull.Value : item.Auswahl_AB_BW);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("eMail" + i, item.EMail == null ? (object)DBNull.Value : item.EMail);
					sqlCommand.Parameters.AddWithValue("FAX" + i, item.FAX == null ? (object)DBNull.Value : item.FAX);
					sqlCommand.Parameters.AddWithValue("Geburtstag" + i, item.Geburtstag == null ? (object)DBNull.Value : item.Geburtstag);
					sqlCommand.Parameters.AddWithValue("Land" + i, item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Mobil" + i, item.Mobil == null ? (object)DBNull.Value : item.Mobil);
					sqlCommand.Parameters.AddWithValue("Nummer" + i, item.Nummer == null ? (object)DBNull.Value : item.Nummer);
					sqlCommand.Parameters.AddWithValue("Ort" + i, item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("PLZ" + i, item.PLZ == null ? (object)DBNull.Value : item.PLZ);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Serienbrief" + i, item.Serienbrief == null ? (object)DBNull.Value : item.Serienbrief);
					sqlCommand.Parameters.AddWithValue("Sprache" + i, item.Sprache == null ? (object)DBNull.Value : item.Sprache);
					sqlCommand.Parameters.AddWithValue("Strasse" + i, item.StraBe == null ? (object)DBNull.Value : item.StraBe);
					sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Titel" + i, item.Titel == null ? (object)DBNull.Value : item.Titel);
					sqlCommand.Parameters.AddWithValue("zu_Handen" + i, item.Zu_Handen == null ? (object)DBNull.Value : item.Zu_Handen);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Ansprechpartner] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

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

				string query = "DELETE FROM [Ansprechpartner] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> GetByNummer(int nummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Ansprechpartner] WHERE [Nummer]=@nummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nummer", nummer);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> GetByNummers(List<int> nummers)
		{
			if(nummers != null && nummers.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> results = null;
				if(nummers.Count <= maxQueryNumber)
				{
					results = getByNummers(nummers);
				}
				else
				{
					int batchNumber = nummers.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByNummers(nummers.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByNummers(nummers.GetRange(batchNumber * maxQueryNumber, nummers.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> getByNummers(List<int> nummers)
		{
			if(nummers != null && nummers.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < nummers.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, nummers[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Ansprechpartner] WHERE [Nummer] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
		}

		public static int DeleteBySupplierAddress(int idSupplierAddress)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Ansprechpartner] WHERE [Nummer]=@idSupplierAddress";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("idSupplierAddress", idSupplierAddress);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int DeleteBySupplierAddress(int idSupplierAddress, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			using(var sqlCommand = new SqlCommand("DELETE FROM [Ansprechpartner] WHERE [Nummer]=@idSupplierAddress", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("idSupplierAddress", idSupplierAddress);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static bool GetContactSalutation(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Ansprechpartner] where TRIM(ISNULL([Briefanrede],''))=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method ?? "");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> GetBySalutation(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Ansprechpartner] where TRIM(ISNULL([Briefanrede],''))=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method ?? "");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
			}
		}

		public static bool GetContactAddress(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Ansprechpartner] where TRIM(ISNULL([Anrede],'')=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method ?? "");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return false;
			}

			var maxNummer = (Convert.ToInt32(dataTable.Rows[0]["Nbr"]) == 0)
				? false
				: true;

			return maxNummer;
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> GetByAddress(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Ansprechpartner] where TRIM(ISNULL([Anrede],''))=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method ?? "");
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();
			}
		}
		public static List<string> GetAnreden()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT TRIM(ISNULL(Anrede,'')) AS Anrede FROM [Ansprechpartner]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["Anrede"].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static List<string> GetBriefanreden()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT TRIM(ISNULL(Briefanrede,'')) AS Briefanrede FROM [Ansprechpartner]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["Briefanrede"].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
