using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class MahnwesenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Mahnwesen] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Mahnwesen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Mahnwesen] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Mahnwesen] ([Adress_id],[Anrede],[Belegdatum],[Belegnummer],[Belegtyp],[Bemerkungen],[Betrag],[Betrag_FW],[Datum_letzte_Zahlung],[Land/PLZ/Ort],[Mahnstufe],[Mandant],[Markierung],[Name2],[Name3],[Projekt-Nr],[Straße/Postfach],[Vorname/NameFirma],[Zahlungsfrist]) OUTPUT INSERTED.[ID] VALUES (@Adress_id,@Anrede,@Belegdatum,@Belegnummer,@Belegtyp,@Bemerkungen,@Betrag,@Betrag_FW,@Datum_letzte_Zahlung,@Land_PLZ_Ort,@Mahnstufe,@Mandant,@Markierung,@Name2,@Name3,@Projekt_Nr,@Strasse_Postfach,@Vorname_NameFirma,@Zahlungsfrist); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Adress_id", item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
					sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Belegdatum", item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
					sqlCommand.Parameters.AddWithValue("Belegnummer", item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
					sqlCommand.Parameters.AddWithValue("Belegtyp", item.Belegtyp == null ? (object)DBNull.Value : item.Belegtyp);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
					sqlCommand.Parameters.AddWithValue("Betrag_FW", item.Betrag_FW == null ? (object)DBNull.Value : item.Betrag_FW);
					sqlCommand.Parameters.AddWithValue("Datum_letzte_Zahlung", item.Datum_letzte_Zahlung == null ? (object)DBNull.Value : item.Datum_letzte_Zahlung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("Mahnstufe", item.Mahnstufe == null ? (object)DBNull.Value : item.Mahnstufe);
					sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Markierung", item.Markierung == null ? (object)DBNull.Value : item.Markierung);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Straße_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Zahlungsfrist", item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> items)
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
						query += " INSERT INTO [Mahnwesen] ([Adress_id],[Anrede],[Belegdatum],[Belegnummer],[Belegtyp],[Bemerkungen],[Betrag],[Betrag_FW],[Datum_letzte_Zahlung],[Land/PLZ/Ort],[Mahnstufe],[Mandant],[Markierung],[Name2],[Name3],[Projekt-Nr],[Straße/Postfach],[Vorname/NameFirma],[Zahlungsfrist]) VALUES ( "

							+ "@Adress_id" + i + ","
							+ "@Anrede" + i + ","
							+ "@Belegdatum" + i + ","
							+ "@Belegnummer" + i + ","
							+ "@Belegtyp" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@Betrag" + i + ","
							+ "@Betrag_FW" + i + ","
							+ "@Datum_letzte_Zahlung" + i + ","
							+ "@Land_PLZ_Ort" + i + ","
							+ "@Mahnstufe" + i + ","
							+ "@Mandant" + i + ","
							+ "@Markierung" + i + ","
							+ "@Name2" + i + ","
							+ "@Name3" + i + ","
							+ "@Projekt_Nr" + i + ","
							+ "@Strasse_Postfach" + i + ","
							+ "@Vorname_NameFirma" + i + ","
							+ "@Zahlungsfrist" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Adress_id" + i, item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Belegdatum" + i, item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
						sqlCommand.Parameters.AddWithValue("Belegnummer" + i, item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
						sqlCommand.Parameters.AddWithValue("Belegtyp" + i, item.Belegtyp == null ? (object)DBNull.Value : item.Belegtyp);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
						sqlCommand.Parameters.AddWithValue("Betrag_FW" + i, item.Betrag_FW == null ? (object)DBNull.Value : item.Betrag_FW);
						sqlCommand.Parameters.AddWithValue("Datum_letzte_Zahlung" + i, item.Datum_letzte_Zahlung == null ? (object)DBNull.Value : item.Datum_letzte_Zahlung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("Mahnstufe" + i, item.Mahnstufe == null ? (object)DBNull.Value : item.Mahnstufe);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Markierung" + i, item.Markierung == null ? (object)DBNull.Value : item.Markierung);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Straße_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Zahlungsfrist" + i, item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Mahnwesen] SET [Adress_id]=@Adress_id, [Anrede]=@Anrede, [Belegdatum]=@Belegdatum, [Belegnummer]=@Belegnummer, [Belegtyp]=@Belegtyp, [Bemerkungen]=@Bemerkungen, [Betrag]=@Betrag, [Betrag_FW]=@Betrag_FW, [Datum_letzte_Zahlung]=@Datum_letzte_Zahlung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [Mahnstufe]=@Mahnstufe, [Mandant]=@Mandant, [Markierung]=@Markierung, [Name2]=@Name2, [Name3]=@Name3, [Projekt-Nr]=@Projekt_Nr, [Straße/Postfach]=@Strasse_Postfach, [Vorname/NameFirma]=@Vorname_NameFirma, [Zahlungsfrist]=@Zahlungsfrist WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Adress_id", item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
				sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
				sqlCommand.Parameters.AddWithValue("Belegdatum", item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
				sqlCommand.Parameters.AddWithValue("Belegnummer", item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
				sqlCommand.Parameters.AddWithValue("Belegtyp", item.Belegtyp == null ? (object)DBNull.Value : item.Belegtyp);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
				sqlCommand.Parameters.AddWithValue("Betrag_FW", item.Betrag_FW == null ? (object)DBNull.Value : item.Betrag_FW);
				sqlCommand.Parameters.AddWithValue("Datum_letzte_Zahlung", item.Datum_letzte_Zahlung == null ? (object)DBNull.Value : item.Datum_letzte_Zahlung);
				sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
				sqlCommand.Parameters.AddWithValue("Mahnstufe", item.Mahnstufe == null ? (object)DBNull.Value : item.Mahnstufe);
				sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
				sqlCommand.Parameters.AddWithValue("Markierung", item.Markierung == null ? (object)DBNull.Value : item.Markierung);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("Straße_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
				sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
				sqlCommand.Parameters.AddWithValue("Zahlungsfrist", item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> items)
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
						query += " UPDATE [Mahnwesen] SET "

							+ "[Adress_id]=@Adress_id" + i + ","
							+ "[Anrede]=@Anrede" + i + ","
							+ "[Belegdatum]=@Belegdatum" + i + ","
							+ "[Belegnummer]=@Belegnummer" + i + ","
							+ "[Belegtyp]=@Belegtyp" + i + ","
							+ "[Bemerkungen]=@Bemerkungen" + i + ","
							+ "[Betrag]=@Betrag" + i + ","
							+ "[Betrag_FW]=@Betrag_FW" + i + ","
							+ "[Datum_letzte_Zahlung]=@Datum_letzte_Zahlung" + i + ","
							+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
							+ "[Mahnstufe]=@Mahnstufe" + i + ","
							+ "[Mandant]=@Mandant" + i + ","
							+ "[Markierung]=@Markierung" + i + ","
							+ "[Name2]=@Name2" + i + ","
							+ "[Name3]=@Name3" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[Straße/Postfach]=@Strasse_Postfach" + i + ","
							+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
							+ "[Zahlungsfrist]=@Zahlungsfrist" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Adress_id" + i, item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Belegdatum" + i, item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
						sqlCommand.Parameters.AddWithValue("Belegnummer" + i, item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
						sqlCommand.Parameters.AddWithValue("Belegtyp" + i, item.Belegtyp == null ? (object)DBNull.Value : item.Belegtyp);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
						sqlCommand.Parameters.AddWithValue("Betrag_FW" + i, item.Betrag_FW == null ? (object)DBNull.Value : item.Betrag_FW);
						sqlCommand.Parameters.AddWithValue("Datum_letzte_Zahlung" + i, item.Datum_letzte_Zahlung == null ? (object)DBNull.Value : item.Datum_letzte_Zahlung);
						sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
						sqlCommand.Parameters.AddWithValue("Mahnstufe" + i, item.Mahnstufe == null ? (object)DBNull.Value : item.Mahnstufe);
						sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
						sqlCommand.Parameters.AddWithValue("Markierung" + i, item.Markierung == null ? (object)DBNull.Value : item.Markierung);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Straße_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
						sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
						sqlCommand.Parameters.AddWithValue("Zahlungsfrist" + i, item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);
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
				string query = "DELETE FROM [Mahnwesen] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Mahnwesen] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Mahnwesen] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Mahnwesen]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Mahnwesen] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Mahnwesen] ([Adress_id],[Anrede],[Belegdatum],[Belegnummer],[Belegtyp],[Bemerkungen],[Betrag],[Betrag_FW],[Datum_letzte_Zahlung],[Land/PLZ/Ort],[Mahnstufe],[Mandant],[Markierung],[Name2],[Name3],[Projekt-Nr],[Straße/Postfach],[Vorname/NameFirma],[Zahlungsfrist]) OUTPUT INSERTED.[ID] VALUES (@Adress_id,@Anrede,@Belegdatum,@Belegnummer,@Belegtyp,@Bemerkungen,@Betrag,@Betrag_FW,@Datum_letzte_Zahlung,@Land_PLZ_Ort,@Mahnstufe,@Mandant,@Markierung,@Name2,@Name3,@Projekt_Nr,@Strasse_Postfach,@Vorname_NameFirma,@Zahlungsfrist); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Adress_id", item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Belegdatum", item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
			sqlCommand.Parameters.AddWithValue("Belegnummer", item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
			sqlCommand.Parameters.AddWithValue("Belegtyp", item.Belegtyp == null ? (object)DBNull.Value : item.Belegtyp);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
			sqlCommand.Parameters.AddWithValue("Betrag_FW", item.Betrag_FW == null ? (object)DBNull.Value : item.Betrag_FW);
			sqlCommand.Parameters.AddWithValue("Datum_letzte_Zahlung", item.Datum_letzte_Zahlung == null ? (object)DBNull.Value : item.Datum_letzte_Zahlung);
			sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("Mahnstufe", item.Mahnstufe == null ? (object)DBNull.Value : item.Mahnstufe);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Markierung", item.Markierung == null ? (object)DBNull.Value : item.Markierung);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Straße_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Zahlungsfrist", item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Mahnwesen] ([Adress_id],[Anrede],[Belegdatum],[Belegnummer],[Belegtyp],[Bemerkungen],[Betrag],[Betrag_FW],[Datum_letzte_Zahlung],[Land/PLZ/Ort],[Mahnstufe],[Mandant],[Markierung],[Name2],[Name3],[Projekt-Nr],[Straße/Postfach],[Vorname/NameFirma],[Zahlungsfrist]) VALUES ( "

						+ "@Adress_id" + i + ","
						+ "@Anrede" + i + ","
						+ "@Belegdatum" + i + ","
						+ "@Belegnummer" + i + ","
						+ "@Belegtyp" + i + ","
						+ "@Bemerkungen" + i + ","
						+ "@Betrag" + i + ","
						+ "@Betrag_FW" + i + ","
						+ "@Datum_letzte_Zahlung" + i + ","
						+ "@Land_PLZ_Ort" + i + ","
						+ "@Mahnstufe" + i + ","
						+ "@Mandant" + i + ","
						+ "@Markierung" + i + ","
						+ "@Name2" + i + ","
						+ "@Name3" + i + ","
						+ "@Projekt_Nr" + i + ","
						+ "@Strasse_Postfach" + i + ","
						+ "@Vorname_NameFirma" + i + ","
						+ "@Zahlungsfrist" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Adress_id" + i, item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Belegdatum" + i, item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
					sqlCommand.Parameters.AddWithValue("Belegnummer" + i, item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
					sqlCommand.Parameters.AddWithValue("Belegtyp" + i, item.Belegtyp == null ? (object)DBNull.Value : item.Belegtyp);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
					sqlCommand.Parameters.AddWithValue("Betrag_FW" + i, item.Betrag_FW == null ? (object)DBNull.Value : item.Betrag_FW);
					sqlCommand.Parameters.AddWithValue("Datum_letzte_Zahlung" + i, item.Datum_letzte_Zahlung == null ? (object)DBNull.Value : item.Datum_letzte_Zahlung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("Mahnstufe" + i, item.Mahnstufe == null ? (object)DBNull.Value : item.Mahnstufe);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Markierung" + i, item.Markierung == null ? (object)DBNull.Value : item.Markierung);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Zahlungsfrist" + i, item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Mahnwesen] SET [Adress_id]=@Adress_id, [Anrede]=@Anrede, [Belegdatum]=@Belegdatum, [Belegnummer]=@Belegnummer, [Belegtyp]=@Belegtyp, [Bemerkungen]=@Bemerkungen, [Betrag]=@Betrag, [Betrag_FW]=@Betrag_FW, [Datum_letzte_Zahlung]=@Datum_letzte_Zahlung, [Land/PLZ/Ort]=@Land_PLZ_Ort, [Mahnstufe]=@Mahnstufe, [Mandant]=@Mandant, [Markierung]=@Markierung, [Name2]=@Name2, [Name3]=@Name3, [Projekt-Nr]=@Projekt_Nr, [Straße/Postfach]=@Strasse_Postfach, [Vorname/NameFirma]=@Vorname_NameFirma, [Zahlungsfrist]=@Zahlungsfrist WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Adress_id", item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Belegdatum", item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
			sqlCommand.Parameters.AddWithValue("Belegnummer", item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
			sqlCommand.Parameters.AddWithValue("Belegtyp", item.Belegtyp == null ? (object)DBNull.Value : item.Belegtyp);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Betrag", item.Betrag == null ? (object)DBNull.Value : item.Betrag);
			sqlCommand.Parameters.AddWithValue("Betrag_FW", item.Betrag_FW == null ? (object)DBNull.Value : item.Betrag_FW);
			sqlCommand.Parameters.AddWithValue("Datum_letzte_Zahlung", item.Datum_letzte_Zahlung == null ? (object)DBNull.Value : item.Datum_letzte_Zahlung);
			sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort", item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
			sqlCommand.Parameters.AddWithValue("Mahnstufe", item.Mahnstufe == null ? (object)DBNull.Value : item.Mahnstufe);
			sqlCommand.Parameters.AddWithValue("Mandant", item.Mandant == null ? (object)DBNull.Value : item.Mandant);
			sqlCommand.Parameters.AddWithValue("Markierung", item.Markierung == null ? (object)DBNull.Value : item.Markierung);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
			sqlCommand.Parameters.AddWithValue("Strasse_Postfach", item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
			sqlCommand.Parameters.AddWithValue("Vorname_NameFirma", item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
			sqlCommand.Parameters.AddWithValue("Zahlungsfrist", item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Mahnwesen] SET "

					+ "[Adress_id]=@Adress_id" + i + ","
					+ "[Anrede]=@Anrede" + i + ","
					+ "[Belegdatum]=@Belegdatum" + i + ","
					+ "[Belegnummer]=@Belegnummer" + i + ","
					+ "[Belegtyp]=@Belegtyp" + i + ","
					+ "[Bemerkungen]=@Bemerkungen" + i + ","
					+ "[Betrag]=@Betrag" + i + ","
					+ "[Betrag_FW]=@Betrag_FW" + i + ","
					+ "[Datum_letzte_Zahlung]=@Datum_letzte_Zahlung" + i + ","
					+ "[Land/PLZ/Ort]=@Land_PLZ_Ort" + i + ","
					+ "[Mahnstufe]=@Mahnstufe" + i + ","
					+ "[Mandant]=@Mandant" + i + ","
					+ "[Markierung]=@Markierung" + i + ","
					+ "[Name2]=@Name2" + i + ","
					+ "[Name3]=@Name3" + i + ","
					+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
					+ "[Straße/Postfach]=@Strasse_Postfach" + i + ","
					+ "[Vorname/NameFirma]=@Vorname_NameFirma" + i + ","
					+ "[Zahlungsfrist]=@Zahlungsfrist" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Adress_id" + i, item.Adress_id == null ? (object)DBNull.Value : item.Adress_id);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Belegdatum" + i, item.Belegdatum == null ? (object)DBNull.Value : item.Belegdatum);
					sqlCommand.Parameters.AddWithValue("Belegnummer" + i, item.Belegnummer == null ? (object)DBNull.Value : item.Belegnummer);
					sqlCommand.Parameters.AddWithValue("Belegtyp" + i, item.Belegtyp == null ? (object)DBNull.Value : item.Belegtyp);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Betrag" + i, item.Betrag == null ? (object)DBNull.Value : item.Betrag);
					sqlCommand.Parameters.AddWithValue("Betrag_FW" + i, item.Betrag_FW == null ? (object)DBNull.Value : item.Betrag_FW);
					sqlCommand.Parameters.AddWithValue("Datum_letzte_Zahlung" + i, item.Datum_letzte_Zahlung == null ? (object)DBNull.Value : item.Datum_letzte_Zahlung);
					sqlCommand.Parameters.AddWithValue("Land_PLZ_Ort" + i, item.Land_PLZ_Ort == null ? (object)DBNull.Value : item.Land_PLZ_Ort);
					sqlCommand.Parameters.AddWithValue("Mahnstufe" + i, item.Mahnstufe == null ? (object)DBNull.Value : item.Mahnstufe);
					sqlCommand.Parameters.AddWithValue("Mandant" + i, item.Mandant == null ? (object)DBNull.Value : item.Mandant);
					sqlCommand.Parameters.AddWithValue("Markierung" + i, item.Markierung == null ? (object)DBNull.Value : item.Markierung);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Strasse_Postfach" + i, item.Strasse_Postfach == null ? (object)DBNull.Value : item.Strasse_Postfach);
					sqlCommand.Parameters.AddWithValue("Vorname_NameFirma" + i, item.Vorname_NameFirma == null ? (object)DBNull.Value : item.Vorname_NameFirma);
					sqlCommand.Parameters.AddWithValue("Zahlungsfrist" + i, item.Zahlungsfrist == null ? (object)DBNull.Value : item.Zahlungsfrist);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Mahnwesen] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [Mahnwesen] WHERE [ID] IN (" + queryIds + ")";
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
