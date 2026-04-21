using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Tbl_umbuchung_historienAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Tbl_umbuchung_historien] WHERE [ID]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Tbl_umbuchung_historien]", sqlConnection))
			{
				sqlConnection.Open();
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();

					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					sqlCommand.CommandText = $"SELECT * FROM [Tbl_umbuchung_historien] WHERE [ID] IN ({string.Join(",", queryIds)})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();

		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Tbl_umbuchung_historien] ([Artikel_Nr],[Bearbeiter],[Datum_Planung],[Datum_umbuchung],[Fertigungsnummer],[ID_Fertigung],[ID_Fertigung_HL],[IDMHD1],[IDMHD2],[IDMHD3],[IDMHD4],[IDMHD5],[IDMHD6],[InternStatus],[LagerID_Ziel],[Lagerort_ID],[Matrikelnummer],[Menge_gebucht],[reserve_zurueck],[Rollennummer],[Sens],[Status]) OUTPUT INSERTED.[ID] VALUES (@Artikel_Nr,@Bearbeiter,@Datum_Planung,@Datum_umbuchung,@Fertigungsnummer,@ID_Fertigung,@ID_Fertigung_HL,@IDMHD1,@IDMHD2,@IDMHD3,@IDMHD4,@IDMHD5,@IDMHD6,@InternStatus,@LagerID_Ziel,@Lagerort_ID,@Matrikelnummer,@Menge_gebucht,@reserve_zurueck,@Rollennummer,@Sens,@Status); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("Datum_Planung", item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
					sqlCommand.Parameters.AddWithValue("Datum_umbuchung", item.Datum_umbuchung);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
					sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
					sqlCommand.Parameters.AddWithValue("IDMHD1", item.IDMHD1 == null ? (object)DBNull.Value : item.IDMHD1);
					sqlCommand.Parameters.AddWithValue("IDMHD2", item.IDMHD2 == null ? (object)DBNull.Value : item.IDMHD2);
					sqlCommand.Parameters.AddWithValue("IDMHD3", item.IDMHD3 == null ? (object)DBNull.Value : item.IDMHD3);
					sqlCommand.Parameters.AddWithValue("IDMHD4", item.IDMHD4 == null ? (object)DBNull.Value : item.IDMHD4);
					sqlCommand.Parameters.AddWithValue("IDMHD5", item.IDMHD5 == null ? (object)DBNull.Value : item.IDMHD5);
					sqlCommand.Parameters.AddWithValue("IDMHD6", item.IDMHD6 == null ? (object)DBNull.Value : item.IDMHD6);
					sqlCommand.Parameters.AddWithValue("InternStatus", item.InternStatus == null ? (object)DBNull.Value : item.InternStatus);
					sqlCommand.Parameters.AddWithValue("LagerID_Ziel", item.LagerID_Ziel == null ? (object)DBNull.Value : item.LagerID_Ziel);
					sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
					sqlCommand.Parameters.AddWithValue("Matrikelnummer", item.Matrikelnummer == null ? (object)DBNull.Value : item.Matrikelnummer);
					sqlCommand.Parameters.AddWithValue("Menge_gebucht", item.Menge_gebucht == null ? (object)DBNull.Value : item.Menge_gebucht);
					sqlCommand.Parameters.AddWithValue("reserve_zurueck", item.reserve_zurueck == null ? (object)DBNull.Value : item.reserve_zurueck);
					sqlCommand.Parameters.AddWithValue("Rollennummer", item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
					sqlCommand.Parameters.AddWithValue("Sens", item.Sens == null ? (object)DBNull.Value : item.Sens);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; /* Nb params per query */
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Tbl_umbuchung_historien] ([Artikel_Nr],[Bearbeiter],[Datum_Planung],[Datum_umbuchung],[Fertigungsnummer],[ID_Fertigung],[ID_Fertigung_HL],[IDMHD1],[IDMHD2],[IDMHD3],[IDMHD4],[IDMHD5],[IDMHD6],[InternStatus],[LagerID_Ziel],[Lagerort_ID],[Matrikelnummer],[Menge_gebucht],[reserve_zurueck],[Rollennummer],[Sens],[Status]) VALUES ("

							+ "@Artikel_Nr" + i + ","
							+ "@Bearbeiter" + i + ","
							+ "@Datum_Planung" + i + ","
							+ "@Datum_umbuchung" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@ID_Fertigung" + i + ","
							+ "@ID_Fertigung_HL" + i + ","
							+ "@IDMHD1" + i + ","
							+ "@IDMHD2" + i + ","
							+ "@IDMHD3" + i + ","
							+ "@IDMHD4" + i + ","
							+ "@IDMHD5" + i + ","
							+ "@IDMHD6" + i + ","
							+ "@InternStatus" + i + ","
							+ "@LagerID_Ziel" + i + ","
							+ "@Lagerort_ID" + i + ","
							+ "@Matrikelnummer" + i + ","
							+ "@Menge_gebucht" + i + ","
							+ "@reserve_zurueck" + i + ","
							+ "@Rollennummer" + i + ","
							+ "@Sens" + i + ","
							+ "@Status" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("Datum_Planung" + i, item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
						sqlCommand.Parameters.AddWithValue("Datum_umbuchung" + i, item.Datum_umbuchung);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
						sqlCommand.Parameters.AddWithValue("IDMHD1" + i, item.IDMHD1 == null ? (object)DBNull.Value : item.IDMHD1);
						sqlCommand.Parameters.AddWithValue("IDMHD2" + i, item.IDMHD2 == null ? (object)DBNull.Value : item.IDMHD2);
						sqlCommand.Parameters.AddWithValue("IDMHD3" + i, item.IDMHD3 == null ? (object)DBNull.Value : item.IDMHD3);
						sqlCommand.Parameters.AddWithValue("IDMHD4" + i, item.IDMHD4 == null ? (object)DBNull.Value : item.IDMHD4);
						sqlCommand.Parameters.AddWithValue("IDMHD5" + i, item.IDMHD5 == null ? (object)DBNull.Value : item.IDMHD5);
						sqlCommand.Parameters.AddWithValue("IDMHD6" + i, item.IDMHD6 == null ? (object)DBNull.Value : item.IDMHD6);
						sqlCommand.Parameters.AddWithValue("InternStatus" + i, item.InternStatus == null ? (object)DBNull.Value : item.InternStatus);
						sqlCommand.Parameters.AddWithValue("LagerID_Ziel" + i, item.LagerID_Ziel == null ? (object)DBNull.Value : item.LagerID_Ziel);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Matrikelnummer" + i, item.Matrikelnummer == null ? (object)DBNull.Value : item.Matrikelnummer);
						sqlCommand.Parameters.AddWithValue("Menge_gebucht" + i, item.Menge_gebucht == null ? (object)DBNull.Value : item.Menge_gebucht);
						sqlCommand.Parameters.AddWithValue("reserve_zurueck" + i, item.reserve_zurueck == null ? (object)DBNull.Value : item.reserve_zurueck);
						sqlCommand.Parameters.AddWithValue("Rollennummer" + i, item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
						sqlCommand.Parameters.AddWithValue("Sens" + i, item.Sens == null ? (object)DBNull.Value : item.Sens);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					}

					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("", sqlConnection))
			{
				sqlConnection.Open();
				string query = "UPDATE [Tbl_umbuchung_historien] SET [Artikel_Nr]=@Artikel_Nr, [Bearbeiter]=@Bearbeiter, [Datum_Planung]=@Datum_Planung, [Datum_umbuchung]=@Datum_umbuchung, [Fertigungsnummer]=@Fertigungsnummer, [ID_Fertigung]=@ID_Fertigung, [ID_Fertigung_HL]=@ID_Fertigung_HL, [IDMHD1]=@IDMHD1, [IDMHD2]=@IDMHD2, [IDMHD3]=@IDMHD3, [IDMHD4]=@IDMHD4, [IDMHD5]=@IDMHD5, [IDMHD6]=@IDMHD6, [InternStatus]=@InternStatus, [LagerID_Ziel]=@LagerID_Ziel, [Lagerort_ID]=@Lagerort_ID, [Matrikelnummer]=@Matrikelnummer, [Menge_gebucht]=@Menge_gebucht, [reserve_zurueck]=@reserve_zurueck, [Rollennummer]=@Rollennummer, [Sens]=@Sens, [Status]=@Status WHERE [ID]=@ID";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
				sqlCommand.Parameters.AddWithValue("Datum_Planung", item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
				sqlCommand.Parameters.AddWithValue("Datum_umbuchung", item.Datum_umbuchung);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
				sqlCommand.Parameters.AddWithValue("IDMHD1", item.IDMHD1 == null ? (object)DBNull.Value : item.IDMHD1);
				sqlCommand.Parameters.AddWithValue("IDMHD2", item.IDMHD2 == null ? (object)DBNull.Value : item.IDMHD2);
				sqlCommand.Parameters.AddWithValue("IDMHD3", item.IDMHD3 == null ? (object)DBNull.Value : item.IDMHD3);
				sqlCommand.Parameters.AddWithValue("IDMHD4", item.IDMHD4 == null ? (object)DBNull.Value : item.IDMHD4);
				sqlCommand.Parameters.AddWithValue("IDMHD5", item.IDMHD5 == null ? (object)DBNull.Value : item.IDMHD5);
				sqlCommand.Parameters.AddWithValue("IDMHD6", item.IDMHD6 == null ? (object)DBNull.Value : item.IDMHD6);
				sqlCommand.Parameters.AddWithValue("InternStatus", item.InternStatus == null ? (object)DBNull.Value : item.InternStatus);
				sqlCommand.Parameters.AddWithValue("LagerID_Ziel", item.LagerID_Ziel == null ? (object)DBNull.Value : item.LagerID_Ziel);
				sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
				sqlCommand.Parameters.AddWithValue("Matrikelnummer", item.Matrikelnummer == null ? (object)DBNull.Value : item.Matrikelnummer);
				sqlCommand.Parameters.AddWithValue("Menge_gebucht", item.Menge_gebucht == null ? (object)DBNull.Value : item.Menge_gebucht);
				sqlCommand.Parameters.AddWithValue("reserve_zurueck", item.reserve_zurueck == null ? (object)DBNull.Value : item.reserve_zurueck);
				sqlCommand.Parameters.AddWithValue("Rollennummer", item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
				sqlCommand.Parameters.AddWithValue("Sens", item.Sens == null ? (object)DBNull.Value : item.Sens);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; /* Nb params per query */
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [Tbl_umbuchung_historien] SET "

							+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
							+ "[Bearbeiter]=@Bearbeiter" + i + ","
							+ "[Datum_Planung]=@Datum_Planung" + i + ","
							+ "[Datum_umbuchung]=@Datum_umbuchung" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[ID_Fertigung]=@ID_Fertigung" + i + ","
							+ "[ID_Fertigung_HL]=@ID_Fertigung_HL" + i + ","
							+ "[IDMHD1]=@IDMHD1" + i + ","
							+ "[IDMHD2]=@IDMHD2" + i + ","
							+ "[IDMHD3]=@IDMHD3" + i + ","
							+ "[IDMHD4]=@IDMHD4" + i + ","
							+ "[IDMHD5]=@IDMHD5" + i + ","
							+ "[IDMHD6]=@IDMHD6" + i + ","
							+ "[InternStatus]=@InternStatus" + i + ","
							+ "[LagerID_Ziel]=@LagerID_Ziel" + i + ","
							+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
							+ "[Matrikelnummer]=@Matrikelnummer" + i + ","
							+ "[Menge_gebucht]=@Menge_gebucht" + i + ","
							+ "[reserve_zurueck]=@reserve_zurueck" + i + ","
							+ "[Rollennummer]=@Rollennummer" + i + ","
							+ "[Sens]=@Sens" + i + ","
							+ "[Status]=@Status" + i + $" WHERE [ID]=@ID{i}"
							+ "; ";

						sqlCommand.Parameters.AddWithValue($"ID{i}", item.ID);

						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("Datum_Planung" + i, item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
						sqlCommand.Parameters.AddWithValue("Datum_umbuchung" + i, item.Datum_umbuchung);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
						sqlCommand.Parameters.AddWithValue("IDMHD1" + i, item.IDMHD1 == null ? (object)DBNull.Value : item.IDMHD1);
						sqlCommand.Parameters.AddWithValue("IDMHD2" + i, item.IDMHD2 == null ? (object)DBNull.Value : item.IDMHD2);
						sqlCommand.Parameters.AddWithValue("IDMHD3" + i, item.IDMHD3 == null ? (object)DBNull.Value : item.IDMHD3);
						sqlCommand.Parameters.AddWithValue("IDMHD4" + i, item.IDMHD4 == null ? (object)DBNull.Value : item.IDMHD4);
						sqlCommand.Parameters.AddWithValue("IDMHD5" + i, item.IDMHD5 == null ? (object)DBNull.Value : item.IDMHD5);
						sqlCommand.Parameters.AddWithValue("IDMHD6" + i, item.IDMHD6 == null ? (object)DBNull.Value : item.IDMHD6);
						sqlCommand.Parameters.AddWithValue("InternStatus" + i, item.InternStatus == null ? (object)DBNull.Value : item.InternStatus);
						sqlCommand.Parameters.AddWithValue("LagerID_Ziel" + i, item.LagerID_Ziel == null ? (object)DBNull.Value : item.LagerID_Ziel);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Matrikelnummer" + i, item.Matrikelnummer == null ? (object)DBNull.Value : item.Matrikelnummer);
						sqlCommand.Parameters.AddWithValue("Menge_gebucht" + i, item.Menge_gebucht == null ? (object)DBNull.Value : item.Menge_gebucht);
						sqlCommand.Parameters.AddWithValue("reserve_zurueck" + i, item.reserve_zurueck == null ? (object)DBNull.Value : item.reserve_zurueck);
						sqlCommand.Parameters.AddWithValue("Rollennummer" + i, item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
						sqlCommand.Parameters.AddWithValue("Sens" + i, item.Sens == null ? (object)DBNull.Value : item.Sens);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("DELETE FROM [Tbl_umbuchung_historien] WHERE [ID]=@ID", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("ID", id);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
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
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					string query = $"DELETE FROM [Tbl_umbuchung_historien] WHERE [ID] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Tbl_umbuchung_historien] WHERE [ID] = @Id", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Tbl_umbuchung_historien]", connection, transaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlCommand = new SqlCommand("", connection, transaction))
				{
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					sqlCommand.CommandText = $"SELECT * FROM [Tbl_umbuchung_historien] WHERE [ID] IN ({string.Join(",", queryIds)})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO Tbl_umbuchung_historien ([Artikel_Nr],[Bearbeiter],[Datum_Planung],[Datum_umbuchung],[Fertigungsnummer],[ID_Fertigung],[ID_Fertigung_HL],[IDMHD1],[IDMHD2],[IDMHD3],[IDMHD4],[IDMHD5],[IDMHD6],[InternStatus],[LagerID_Ziel],[Lagerort_ID],[Matrikelnummer],[Menge_gebucht],[reserve_zurueck],[Rollennummer],[Sens],[Status]) OUTPUT INSERTED.[ID] VALUES (@Artikel_Nr,@Bearbeiter,@Datum_Planung,@Datum_umbuchung,@Fertigungsnummer,@ID_Fertigung,@ID_Fertigung_HL,@IDMHD1,@IDMHD2,@IDMHD3,@IDMHD4,@IDMHD5,@IDMHD6,@InternStatus,@LagerID_Ziel,@Lagerort_ID,@Matrikelnummer,@Menge_gebucht,@reserve_zurueck,@Rollennummer,@Sens,@Status); ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
				sqlCommand.Parameters.AddWithValue("Datum_Planung", item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
				sqlCommand.Parameters.AddWithValue("Datum_umbuchung", item.Datum_umbuchung);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
				sqlCommand.Parameters.AddWithValue("IDMHD1", item.IDMHD1 == null ? (object)DBNull.Value : item.IDMHD1);
				sqlCommand.Parameters.AddWithValue("IDMHD2", item.IDMHD2 == null ? (object)DBNull.Value : item.IDMHD2);
				sqlCommand.Parameters.AddWithValue("IDMHD3", item.IDMHD3 == null ? (object)DBNull.Value : item.IDMHD3);
				sqlCommand.Parameters.AddWithValue("IDMHD4", item.IDMHD4 == null ? (object)DBNull.Value : item.IDMHD4);
				sqlCommand.Parameters.AddWithValue("IDMHD5", item.IDMHD5 == null ? (object)DBNull.Value : item.IDMHD5);
				sqlCommand.Parameters.AddWithValue("IDMHD6", item.IDMHD6 == null ? (object)DBNull.Value : item.IDMHD6);
				sqlCommand.Parameters.AddWithValue("InternStatus", item.InternStatus == null ? (object)DBNull.Value : item.InternStatus);
				sqlCommand.Parameters.AddWithValue("LagerID_Ziel", item.LagerID_Ziel == null ? (object)DBNull.Value : item.LagerID_Ziel);
				sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
				sqlCommand.Parameters.AddWithValue("Matrikelnummer", item.Matrikelnummer == null ? (object)DBNull.Value : item.Matrikelnummer);
				sqlCommand.Parameters.AddWithValue("Menge_gebucht", item.Menge_gebucht == null ? (object)DBNull.Value : item.Menge_gebucht);
				sqlCommand.Parameters.AddWithValue("reserve_zurueck", item.reserve_zurueck == null ? (object)DBNull.Value : item.reserve_zurueck);
				sqlCommand.Parameters.AddWithValue("Rollennummer", item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
				sqlCommand.Parameters.AddWithValue("Sens", item.Sens == null ? (object)DBNull.Value : item.Sens);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; /* Nb params per query */
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "INSERT INTO [Tbl_umbuchung_historien] ([Artikel_Nr],[Bearbeiter],[Datum_Planung],[Datum_umbuchung],[Fertigungsnummer],[ID_Fertigung],[ID_Fertigung_HL],[IDMHD1],[IDMHD2],[IDMHD3],[IDMHD4],[IDMHD5],[IDMHD6],[InternStatus],[LagerID_Ziel],[Lagerort_ID],[Matrikelnummer],[Menge_gebucht],[reserve_zurueck],[Rollennummer],[Sens],[Status]) VALUES ( "

						+ "@Artikel_Nr" + i + ","
						+ "@Bearbeiter" + i + ","
						+ "@Datum_Planung" + i + ","
						+ "@Datum_umbuchung" + i + ","
						+ "@Fertigungsnummer" + i + ","
						+ "@ID_Fertigung" + i + ","
						+ "@ID_Fertigung_HL" + i + ","
						+ "@IDMHD1" + i + ","
						+ "@IDMHD2" + i + ","
						+ "@IDMHD3" + i + ","
						+ "@IDMHD4" + i + ","
						+ "@IDMHD5" + i + ","
						+ "@IDMHD6" + i + ","
						+ "@InternStatus" + i + ","
						+ "@LagerID_Ziel" + i + ","
						+ "@Lagerort_ID" + i + ","
						+ "@Matrikelnummer" + i + ","
						+ "@Menge_gebucht" + i + ","
						+ "@reserve_zurueck" + i + ","
						+ "@Rollennummer" + i + ","
						+ "@Sens" + i + ","
						+ "@Status" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("Datum_Planung" + i, item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
						sqlCommand.Parameters.AddWithValue("Datum_umbuchung" + i, item.Datum_umbuchung);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
						sqlCommand.Parameters.AddWithValue("IDMHD1" + i, item.IDMHD1 == null ? (object)DBNull.Value : item.IDMHD1);
						sqlCommand.Parameters.AddWithValue("IDMHD2" + i, item.IDMHD2 == null ? (object)DBNull.Value : item.IDMHD2);
						sqlCommand.Parameters.AddWithValue("IDMHD3" + i, item.IDMHD3 == null ? (object)DBNull.Value : item.IDMHD3);
						sqlCommand.Parameters.AddWithValue("IDMHD4" + i, item.IDMHD4 == null ? (object)DBNull.Value : item.IDMHD4);
						sqlCommand.Parameters.AddWithValue("IDMHD5" + i, item.IDMHD5 == null ? (object)DBNull.Value : item.IDMHD5);
						sqlCommand.Parameters.AddWithValue("IDMHD6" + i, item.IDMHD6 == null ? (object)DBNull.Value : item.IDMHD6);
						sqlCommand.Parameters.AddWithValue("InternStatus" + i, item.InternStatus == null ? (object)DBNull.Value : item.InternStatus);
						sqlCommand.Parameters.AddWithValue("LagerID_Ziel" + i, item.LagerID_Ziel == null ? (object)DBNull.Value : item.LagerID_Ziel);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Matrikelnummer" + i, item.Matrikelnummer == null ? (object)DBNull.Value : item.Matrikelnummer);
						sqlCommand.Parameters.AddWithValue("Menge_gebucht" + i, item.Menge_gebucht == null ? (object)DBNull.Value : item.Menge_gebucht);
						sqlCommand.Parameters.AddWithValue("reserve_zurueck" + i, item.reserve_zurueck == null ? (object)DBNull.Value : item.reserve_zurueck);
						sqlCommand.Parameters.AddWithValue("Rollennummer" + i, item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
						sqlCommand.Parameters.AddWithValue("Sens" + i, item.Sens == null ? (object)DBNull.Value : item.Sens);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					}

					sqlCommand.CommandText = query;

					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [Tbl_umbuchung_historien] SET [Artikel_Nr]=@Artikel_Nr, [Bearbeiter]=@Bearbeiter, [Datum_Planung]=@Datum_Planung, [Datum_umbuchung]=@Datum_umbuchung, [Fertigungsnummer]=@Fertigungsnummer, [ID_Fertigung]=@ID_Fertigung, [ID_Fertigung_HL]=@ID_Fertigung_HL, [IDMHD1]=@IDMHD1, [IDMHD2]=@IDMHD2, [IDMHD3]=@IDMHD3, [IDMHD4]=@IDMHD4, [IDMHD5]=@IDMHD5, [IDMHD6]=@IDMHD6, [InternStatus]=@InternStatus, [LagerID_Ziel]=@LagerID_Ziel, [Lagerort_ID]=@Lagerort_ID, [Matrikelnummer]=@Matrikelnummer, [Menge_gebucht]=@Menge_gebucht, [reserve_zurueck]=@reserve_zurueck, [Rollennummer]=@Rollennummer, [Sens]=@Sens, [Status]=@Status WHERE [ID]=@ID";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
				sqlCommand.Parameters.AddWithValue("Datum_Planung", item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
				sqlCommand.Parameters.AddWithValue("Datum_umbuchung", item.Datum_umbuchung);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung", item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
				sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL", item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
				sqlCommand.Parameters.AddWithValue("IDMHD1", item.IDMHD1 == null ? (object)DBNull.Value : item.IDMHD1);
				sqlCommand.Parameters.AddWithValue("IDMHD2", item.IDMHD2 == null ? (object)DBNull.Value : item.IDMHD2);
				sqlCommand.Parameters.AddWithValue("IDMHD3", item.IDMHD3 == null ? (object)DBNull.Value : item.IDMHD3);
				sqlCommand.Parameters.AddWithValue("IDMHD4", item.IDMHD4 == null ? (object)DBNull.Value : item.IDMHD4);
				sqlCommand.Parameters.AddWithValue("IDMHD5", item.IDMHD5 == null ? (object)DBNull.Value : item.IDMHD5);
				sqlCommand.Parameters.AddWithValue("IDMHD6", item.IDMHD6 == null ? (object)DBNull.Value : item.IDMHD6);
				sqlCommand.Parameters.AddWithValue("InternStatus", item.InternStatus == null ? (object)DBNull.Value : item.InternStatus);
				sqlCommand.Parameters.AddWithValue("LagerID_Ziel", item.LagerID_Ziel == null ? (object)DBNull.Value : item.LagerID_Ziel);
				sqlCommand.Parameters.AddWithValue("Lagerort_ID", item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
				sqlCommand.Parameters.AddWithValue("Matrikelnummer", item.Matrikelnummer == null ? (object)DBNull.Value : item.Matrikelnummer);
				sqlCommand.Parameters.AddWithValue("Menge_gebucht", item.Menge_gebucht == null ? (object)DBNull.Value : item.Menge_gebucht);
				sqlCommand.Parameters.AddWithValue("reserve_zurueck", item.reserve_zurueck == null ? (object)DBNull.Value : item.reserve_zurueck);
				sqlCommand.Parameters.AddWithValue("Rollennummer", item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
				sqlCommand.Parameters.AddWithValue("Sens", item.Sens == null ? (object)DBNull.Value : item.Sens);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 23; /* Nb params per query */
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [Tbl_umbuchung_historien] SET "

						+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
						+ "[Bearbeiter]=@Bearbeiter" + i + ","
						+ "[Datum_Planung]=@Datum_Planung" + i + ","
						+ "[Datum_umbuchung]=@Datum_umbuchung" + i + ","
						+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
						+ "[ID_Fertigung]=@ID_Fertigung" + i + ","
						+ "[ID_Fertigung_HL]=@ID_Fertigung_HL" + i + ","
						+ "[IDMHD1]=@IDMHD1" + i + ","
						+ "[IDMHD2]=@IDMHD2" + i + ","
						+ "[IDMHD3]=@IDMHD3" + i + ","
						+ "[IDMHD4]=@IDMHD4" + i + ","
						+ "[IDMHD5]=@IDMHD5" + i + ","
						+ "[IDMHD6]=@IDMHD6" + i + ","
						+ "[InternStatus]=@InternStatus" + i + ","
						+ "[LagerID_Ziel]=@LagerID_Ziel" + i + ","
						+ "[Lagerort_ID]=@Lagerort_ID" + i + ","
						+ "[Matrikelnummer]=@Matrikelnummer" + i + ","
						+ "[Menge_gebucht]=@Menge_gebucht" + i + ","
						+ "[reserve_zurueck]=@reserve_zurueck" + i + ","
						+ "[Rollennummer]=@Rollennummer" + i + ","
						+ "[Sens]=@Sens" + i + ","
						+ "[Status]=@Status" + i + " WHERE [ID]=@ID" + i
							+ ";";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);

						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("Datum_Planung" + i, item.Datum_Planung == null ? (object)DBNull.Value : item.Datum_Planung);
						sqlCommand.Parameters.AddWithValue("Datum_umbuchung" + i, item.Datum_umbuchung);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, item.ID_Fertigung == null ? (object)DBNull.Value : item.ID_Fertigung);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, item.ID_Fertigung_HL == null ? (object)DBNull.Value : item.ID_Fertigung_HL);
						sqlCommand.Parameters.AddWithValue("IDMHD1" + i, item.IDMHD1 == null ? (object)DBNull.Value : item.IDMHD1);
						sqlCommand.Parameters.AddWithValue("IDMHD2" + i, item.IDMHD2 == null ? (object)DBNull.Value : item.IDMHD2);
						sqlCommand.Parameters.AddWithValue("IDMHD3" + i, item.IDMHD3 == null ? (object)DBNull.Value : item.IDMHD3);
						sqlCommand.Parameters.AddWithValue("IDMHD4" + i, item.IDMHD4 == null ? (object)DBNull.Value : item.IDMHD4);
						sqlCommand.Parameters.AddWithValue("IDMHD5" + i, item.IDMHD5 == null ? (object)DBNull.Value : item.IDMHD5);
						sqlCommand.Parameters.AddWithValue("IDMHD6" + i, item.IDMHD6 == null ? (object)DBNull.Value : item.IDMHD6);
						sqlCommand.Parameters.AddWithValue("InternStatus" + i, item.InternStatus == null ? (object)DBNull.Value : item.InternStatus);
						sqlCommand.Parameters.AddWithValue("LagerID_Ziel" + i, item.LagerID_Ziel == null ? (object)DBNull.Value : item.LagerID_Ziel);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Matrikelnummer" + i, item.Matrikelnummer == null ? (object)DBNull.Value : item.Matrikelnummer);
						sqlCommand.Parameters.AddWithValue("Menge_gebucht" + i, item.Menge_gebucht == null ? (object)DBNull.Value : item.Menge_gebucht);
						sqlCommand.Parameters.AddWithValue("reserve_zurueck" + i, item.reserve_zurueck == null ? (object)DBNull.Value : item.reserve_zurueck);
						sqlCommand.Parameters.AddWithValue("Rollennummer" + i, item.Rollennummer == null ? (object)DBNull.Value : item.Rollennummer);
						sqlCommand.Parameters.AddWithValue("Sens" + i, item.Sens == null ? (object)DBNull.Value : item.Sens);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					}

					sqlCommand.CommandText = query;
					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [Tbl_umbuchung_historien] WHERE [ID]=@ID";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ID", id);
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
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
				using(var sqlCommand = new SqlCommand("", connection, transaction))
				{
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					string query = $"DELETE FROM Tbl_umbuchung_historien] WHERE [ID] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity> GetByFertigung(int faId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Tbl_umbuchung_historien] WHERE [ID_Fertigung]=@faId", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("faId", faId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_umbuchung_historienEntity>();
			}
		}
		#endregion Custom Methods

	}
}
