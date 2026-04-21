using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class PreisgruppenAccess
	{
		public static int PRICE_INFINITY { get; } = 999999999;
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Preisgruppen] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Preisgruppen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Preisgruppen] WHERE [Nr] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Preisgruppen] ([Artikel-Nr],[Aufschlag],[Aufschlagsatz],[Bemerkung],[bis Anzahl Mengeneinheiten 1],[bis Anzahl Mengeneinheiten 10],[bis Anzahl Mengeneinheiten 2],[bis Anzahl Mengeneinheiten 3],[bis Anzahl Mengeneinheiten 4],[bis Anzahl Mengeneinheiten 5],[bis Anzahl Mengeneinheiten 6],[bis Anzahl Mengeneinheiten 7],[bis Anzahl Mengeneinheiten 8],[bis Anzahl Mengeneinheiten 9],[brutto],[Einkaufspreis],[kalk_kosten],[letzte_Aktualisierung],[ME1],[ME2],[ME3],[ME4],[PM1],[PM2],[PM3],[PM4],[Preisgruppe],[Preisminderung 1 (%)],[Preisminderung 10 (%)],[Preisminderung 2 (%)],[Preisminderung 3 (%)],[Preisminderung 4 (%)],[Preisminderung 5 (%)],[Preisminderung 6 (%)],[Preisminderung 7 (%)],[Preisminderung 8 (%)],[Preisminderung 9 (%)],[Staffelpreis1],[Staffelpreis2],[Staffelpreis3],[Staffelpreis4],[Verkaufspreis]) OUTPUT INSERTED.[Nr] VALUES (@Artikel_Nr,@Aufschlag,@Aufschlagsatz,@Bemerkung,@bis_Anzahl_Mengeneinheiten_1,@bis_Anzahl_Mengeneinheiten_10,@bis_Anzahl_Mengeneinheiten_2,@bis_Anzahl_Mengeneinheiten_3,@bis_Anzahl_Mengeneinheiten_4,@bis_Anzahl_Mengeneinheiten_5,@bis_Anzahl_Mengeneinheiten_6,@bis_Anzahl_Mengeneinheiten_7,@bis_Anzahl_Mengeneinheiten_8,@bis_Anzahl_Mengeneinheiten_9,@brutto,@Einkaufspreis,@kalk_kosten,@letzte_Aktualisierung,@ME1,@ME2,@ME3,@ME4,@PM1,@PM2,@PM3,@PM4,@Preisgruppe,@Preisminderung_1____,@Preisminderung_10____,@Preisminderung_2____,@Preisminderung_3____,@Preisminderung_4____,@Preisminderung_5____,@Preisminderung_6____,@Preisminderung_7____,@Preisminderung_8____,@Preisminderung_9____,@Staffelpreis1,@Staffelpreis2,@Staffelpreis3,@Staffelpreis4,@Verkaufspreis); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
					sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_1", item.bis_Anzahl_Mengeneinheiten_1 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_1);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_10", item.bis_Anzahl_Mengeneinheiten_10 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_10);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_2", item.bis_Anzahl_Mengeneinheiten_2 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_2);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_3", item.bis_Anzahl_Mengeneinheiten_3 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_3);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_4", item.bis_Anzahl_Mengeneinheiten_4 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_4);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_5", item.bis_Anzahl_Mengeneinheiten_5 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_5);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_6", item.bis_Anzahl_Mengeneinheiten_6 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_6);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_7", item.bis_Anzahl_Mengeneinheiten_7 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_7);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_8", item.bis_Anzahl_Mengeneinheiten_8 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_8);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_9", item.bis_Anzahl_Mengeneinheiten_9 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_9);
					sqlCommand.Parameters.AddWithValue("brutto", item.brutto == null ? (object)DBNull.Value : item.brutto);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
					sqlCommand.Parameters.AddWithValue("kalk_kosten", item.kalk_kosten == null ? (object)DBNull.Value : item.kalk_kosten);
					sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
					sqlCommand.Parameters.AddWithValue("ME1", item.ME1 == null ? (object)DBNull.Value : item.ME1);
					sqlCommand.Parameters.AddWithValue("ME2", item.ME2 == null ? (object)DBNull.Value : item.ME2);
					sqlCommand.Parameters.AddWithValue("ME3", item.ME3 == null ? (object)DBNull.Value : item.ME3);
					sqlCommand.Parameters.AddWithValue("ME4", item.ME4 == null ? (object)DBNull.Value : item.ME4);
					sqlCommand.Parameters.AddWithValue("PM1", item.PM1 == null ? (object)DBNull.Value : item.PM1);
					sqlCommand.Parameters.AddWithValue("PM2", item.PM2 == null ? (object)DBNull.Value : item.PM2);
					sqlCommand.Parameters.AddWithValue("PM3", item.PM3 == null ? (object)DBNull.Value : item.PM3);
					sqlCommand.Parameters.AddWithValue("PM4", item.PM4 == null ? (object)DBNull.Value : item.PM4);
					sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Preisminderung_1____", item.Preisminderung_1____ == null ? (object)DBNull.Value : item.Preisminderung_1____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_10____", item.Preisminderung_10____ == null ? (object)DBNull.Value : item.Preisminderung_10____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_2____", item.Preisminderung_2____ == null ? (object)DBNull.Value : item.Preisminderung_2____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_3____", item.Preisminderung_3____ == null ? (object)DBNull.Value : item.Preisminderung_3____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_4____", item.Preisminderung_4____ == null ? (object)DBNull.Value : item.Preisminderung_4____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_5____", item.Preisminderung_5____ == null ? (object)DBNull.Value : item.Preisminderung_5____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_6____", item.Preisminderung_6____ == null ? (object)DBNull.Value : item.Preisminderung_6____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_7____", item.Preisminderung_7____ == null ? (object)DBNull.Value : item.Preisminderung_7____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_8____", item.Preisminderung_8____ == null ? (object)DBNull.Value : item.Preisminderung_8____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_9____", item.Preisminderung_9____ == null ? (object)DBNull.Value : item.Preisminderung_9____);
					sqlCommand.Parameters.AddWithValue("Staffelpreis1", item.Staffelpreis1 == null ? (object)DBNull.Value : item.Staffelpreis1);
					sqlCommand.Parameters.AddWithValue("Staffelpreis2", item.Staffelpreis2 == null ? (object)DBNull.Value : item.Staffelpreis2);
					sqlCommand.Parameters.AddWithValue("Staffelpreis3", item.Staffelpreis3 == null ? (object)DBNull.Value : item.Staffelpreis3);
					sqlCommand.Parameters.AddWithValue("Staffelpreis4", item.Staffelpreis4 == null ? (object)DBNull.Value : item.Staffelpreis4);
					sqlCommand.Parameters.AddWithValue("Verkaufspreis", item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 44; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> items)
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
						query += " INSERT INTO [Preisgruppen] ([Artikel-Nr],[Aufschlag],[Aufschlagsatz],[Bemerkung],[bis Anzahl Mengeneinheiten 1],[bis Anzahl Mengeneinheiten 10],[bis Anzahl Mengeneinheiten 2],[bis Anzahl Mengeneinheiten 3],[bis Anzahl Mengeneinheiten 4],[bis Anzahl Mengeneinheiten 5],[bis Anzahl Mengeneinheiten 6],[bis Anzahl Mengeneinheiten 7],[bis Anzahl Mengeneinheiten 8],[bis Anzahl Mengeneinheiten 9],[brutto],[Einkaufspreis],[kalk_kosten],[letzte_Aktualisierung],[ME1],[ME2],[ME3],[ME4],[PM1],[PM2],[PM3],[PM4],[Preisgruppe],[Preisminderung 1 (%)],[Preisminderung 10 (%)],[Preisminderung 2 (%)],[Preisminderung 3 (%)],[Preisminderung 4 (%)],[Preisminderung 5 (%)],[Preisminderung 6 (%)],[Preisminderung 7 (%)],[Preisminderung 8 (%)],[Preisminderung 9 (%)],[Staffelpreis1],[Staffelpreis2],[Staffelpreis3],[Staffelpreis4],[Verkaufspreis]) VALUES ( "

							+ "@Artikel_Nr" + i + ","
							+ "@Aufschlag" + i + ","
							+ "@Aufschlagsatz" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_1" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_10" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_2" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_3" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_4" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_5" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_6" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_7" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_8" + i + ","
							+ "@bis_Anzahl_Mengeneinheiten_9" + i + ","
							+ "@brutto" + i + ","
							+ "@Einkaufspreis" + i + ","
							+ "@kalk_kosten" + i + ","
							+ "@letzte_Aktualisierung" + i + ","
							+ "@ME1" + i + ","
							+ "@ME2" + i + ","
							+ "@ME3" + i + ","
							+ "@ME4" + i + ","
							+ "@PM1" + i + ","
							+ "@PM2" + i + ","
							+ "@PM3" + i + ","
							+ "@PM4" + i + ","
							+ "@Preisgruppe" + i + ","
							+ "@Preisminderung_1____" + i + ","
							+ "@Preisminderung_10____" + i + ","
							+ "@Preisminderung_2____" + i + ","
							+ "@Preisminderung_3____" + i + ","
							+ "@Preisminderung_4____" + i + ","
							+ "@Preisminderung_5____" + i + ","
							+ "@Preisminderung_6____" + i + ","
							+ "@Preisminderung_7____" + i + ","
							+ "@Preisminderung_8____" + i + ","
							+ "@Preisminderung_9____" + i + ","
							+ "@Staffelpreis1" + i + ","
							+ "@Staffelpreis2" + i + ","
							+ "@Staffelpreis3" + i + ","
							+ "@Staffelpreis4" + i + ","
							+ "@Verkaufspreis" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
						sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_1" + i, item.bis_Anzahl_Mengeneinheiten_1 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_1);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_10" + i, item.bis_Anzahl_Mengeneinheiten_10 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_10);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_2" + i, item.bis_Anzahl_Mengeneinheiten_2 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_2);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_3" + i, item.bis_Anzahl_Mengeneinheiten_3 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_3);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_4" + i, item.bis_Anzahl_Mengeneinheiten_4 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_4);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_5" + i, item.bis_Anzahl_Mengeneinheiten_5 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_5);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_6" + i, item.bis_Anzahl_Mengeneinheiten_6 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_6);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_7" + i, item.bis_Anzahl_Mengeneinheiten_7 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_7);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_8" + i, item.bis_Anzahl_Mengeneinheiten_8 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_8);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_9" + i, item.bis_Anzahl_Mengeneinheiten_9 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_9);
						sqlCommand.Parameters.AddWithValue("brutto" + i, item.brutto == null ? (object)DBNull.Value : item.brutto);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
						sqlCommand.Parameters.AddWithValue("kalk_kosten" + i, item.kalk_kosten == null ? (object)DBNull.Value : item.kalk_kosten);
						sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung" + i, item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
						sqlCommand.Parameters.AddWithValue("ME1" + i, item.ME1 == null ? (object)DBNull.Value : item.ME1);
						sqlCommand.Parameters.AddWithValue("ME2" + i, item.ME2 == null ? (object)DBNull.Value : item.ME2);
						sqlCommand.Parameters.AddWithValue("ME3" + i, item.ME3 == null ? (object)DBNull.Value : item.ME3);
						sqlCommand.Parameters.AddWithValue("ME4" + i, item.ME4 == null ? (object)DBNull.Value : item.ME4);
						sqlCommand.Parameters.AddWithValue("PM1" + i, item.PM1 == null ? (object)DBNull.Value : item.PM1);
						sqlCommand.Parameters.AddWithValue("PM2" + i, item.PM2 == null ? (object)DBNull.Value : item.PM2);
						sqlCommand.Parameters.AddWithValue("PM3" + i, item.PM3 == null ? (object)DBNull.Value : item.PM3);
						sqlCommand.Parameters.AddWithValue("PM4" + i, item.PM4 == null ? (object)DBNull.Value : item.PM4);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Preisminderung_1____" + i, item.Preisminderung_1____ == null ? (object)DBNull.Value : item.Preisminderung_1____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_10____" + i, item.Preisminderung_10____ == null ? (object)DBNull.Value : item.Preisminderung_10____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_2____" + i, item.Preisminderung_2____ == null ? (object)DBNull.Value : item.Preisminderung_2____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_3____" + i, item.Preisminderung_3____ == null ? (object)DBNull.Value : item.Preisminderung_3____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_4____" + i, item.Preisminderung_4____ == null ? (object)DBNull.Value : item.Preisminderung_4____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_5____" + i, item.Preisminderung_5____ == null ? (object)DBNull.Value : item.Preisminderung_5____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_6____" + i, item.Preisminderung_6____ == null ? (object)DBNull.Value : item.Preisminderung_6____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_7____" + i, item.Preisminderung_7____ == null ? (object)DBNull.Value : item.Preisminderung_7____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_8____" + i, item.Preisminderung_8____ == null ? (object)DBNull.Value : item.Preisminderung_8____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_9____" + i, item.Preisminderung_9____ == null ? (object)DBNull.Value : item.Preisminderung_9____);
						sqlCommand.Parameters.AddWithValue("Staffelpreis1" + i, item.Staffelpreis1 == null ? (object)DBNull.Value : item.Staffelpreis1);
						sqlCommand.Parameters.AddWithValue("Staffelpreis2" + i, item.Staffelpreis2 == null ? (object)DBNull.Value : item.Staffelpreis2);
						sqlCommand.Parameters.AddWithValue("Staffelpreis3" + i, item.Staffelpreis3 == null ? (object)DBNull.Value : item.Staffelpreis3);
						sqlCommand.Parameters.AddWithValue("Staffelpreis4" + i, item.Staffelpreis4 == null ? (object)DBNull.Value : item.Staffelpreis4);
						sqlCommand.Parameters.AddWithValue("Verkaufspreis" + i, item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Preisgruppen] SET [Artikel-Nr]=@Artikel_Nr, [Aufschlag]=@Aufschlag, [Aufschlagsatz]=@Aufschlagsatz, [Bemerkung]=@Bemerkung, [bis Anzahl Mengeneinheiten 1]=@bis_Anzahl_Mengeneinheiten_1, [bis Anzahl Mengeneinheiten 10]=@bis_Anzahl_Mengeneinheiten_10, [bis Anzahl Mengeneinheiten 2]=@bis_Anzahl_Mengeneinheiten_2, [bis Anzahl Mengeneinheiten 3]=@bis_Anzahl_Mengeneinheiten_3, [bis Anzahl Mengeneinheiten 4]=@bis_Anzahl_Mengeneinheiten_4, [bis Anzahl Mengeneinheiten 5]=@bis_Anzahl_Mengeneinheiten_5, [bis Anzahl Mengeneinheiten 6]=@bis_Anzahl_Mengeneinheiten_6, [bis Anzahl Mengeneinheiten 7]=@bis_Anzahl_Mengeneinheiten_7, [bis Anzahl Mengeneinheiten 8]=@bis_Anzahl_Mengeneinheiten_8, [bis Anzahl Mengeneinheiten 9]=@bis_Anzahl_Mengeneinheiten_9, [brutto]=@brutto, [Einkaufspreis]=@Einkaufspreis, [kalk_kosten]=@kalk_kosten, [letzte_Aktualisierung]=@letzte_Aktualisierung, [ME1]=@ME1, [ME2]=@ME2, [ME3]=@ME3, [ME4]=@ME4, [PM1]=@PM1, [PM2]=@PM2, [PM3]=@PM3, [PM4]=@PM4, [Preisgruppe]=@Preisgruppe, [Preisminderung 1 (%)]=@Preisminderung_1____, [Preisminderung 10 (%)]=@Preisminderung_10____, [Preisminderung 2 (%)]=@Preisminderung_2____, [Preisminderung 3 (%)]=@Preisminderung_3____, [Preisminderung 4 (%)]=@Preisminderung_4____, [Preisminderung 5 (%)]=@Preisminderung_5____, [Preisminderung 6 (%)]=@Preisminderung_6____, [Preisminderung 7 (%)]=@Preisminderung_7____, [Preisminderung 8 (%)]=@Preisminderung_8____, [Preisminderung 9 (%)]=@Preisminderung_9____, [Staffelpreis1]=@Staffelpreis1, [Staffelpreis2]=@Staffelpreis2, [Staffelpreis3]=@Staffelpreis3, [Staffelpreis4]=@Staffelpreis4, [Verkaufspreis]=@Verkaufspreis WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
				sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_1", item.bis_Anzahl_Mengeneinheiten_1 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_1);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_10", item.bis_Anzahl_Mengeneinheiten_10 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_10);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_2", item.bis_Anzahl_Mengeneinheiten_2 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_2);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_3", item.bis_Anzahl_Mengeneinheiten_3 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_3);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_4", item.bis_Anzahl_Mengeneinheiten_4 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_4);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_5", item.bis_Anzahl_Mengeneinheiten_5 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_5);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_6", item.bis_Anzahl_Mengeneinheiten_6 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_6);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_7", item.bis_Anzahl_Mengeneinheiten_7 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_7);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_8", item.bis_Anzahl_Mengeneinheiten_8 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_8);
				sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_9", item.bis_Anzahl_Mengeneinheiten_9 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_9);
				sqlCommand.Parameters.AddWithValue("brutto", item.brutto == null ? (object)DBNull.Value : item.brutto);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
				sqlCommand.Parameters.AddWithValue("kalk_kosten", item.kalk_kosten == null ? (object)DBNull.Value : item.kalk_kosten);
				sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
				sqlCommand.Parameters.AddWithValue("ME1", item.ME1 == null ? (object)DBNull.Value : item.ME1);
				sqlCommand.Parameters.AddWithValue("ME2", item.ME2 == null ? (object)DBNull.Value : item.ME2);
				sqlCommand.Parameters.AddWithValue("ME3", item.ME3 == null ? (object)DBNull.Value : item.ME3);
				sqlCommand.Parameters.AddWithValue("ME4", item.ME4 == null ? (object)DBNull.Value : item.ME4);
				sqlCommand.Parameters.AddWithValue("PM1", item.PM1 == null ? (object)DBNull.Value : item.PM1);
				sqlCommand.Parameters.AddWithValue("PM2", item.PM2 == null ? (object)DBNull.Value : item.PM2);
				sqlCommand.Parameters.AddWithValue("PM3", item.PM3 == null ? (object)DBNull.Value : item.PM3);
				sqlCommand.Parameters.AddWithValue("PM4", item.PM4 == null ? (object)DBNull.Value : item.PM4);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				sqlCommand.Parameters.AddWithValue("Preisminderung_1____", item.Preisminderung_1____ == null ? (object)DBNull.Value : item.Preisminderung_1____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_10____", item.Preisminderung_10____ == null ? (object)DBNull.Value : item.Preisminderung_10____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_2____", item.Preisminderung_2____ == null ? (object)DBNull.Value : item.Preisminderung_2____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_3____", item.Preisminderung_3____ == null ? (object)DBNull.Value : item.Preisminderung_3____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_4____", item.Preisminderung_4____ == null ? (object)DBNull.Value : item.Preisminderung_4____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_5____", item.Preisminderung_5____ == null ? (object)DBNull.Value : item.Preisminderung_5____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_6____", item.Preisminderung_6____ == null ? (object)DBNull.Value : item.Preisminderung_6____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_7____", item.Preisminderung_7____ == null ? (object)DBNull.Value : item.Preisminderung_7____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_8____", item.Preisminderung_8____ == null ? (object)DBNull.Value : item.Preisminderung_8____);
				sqlCommand.Parameters.AddWithValue("Preisminderung_9____", item.Preisminderung_9____ == null ? (object)DBNull.Value : item.Preisminderung_9____);
				sqlCommand.Parameters.AddWithValue("Staffelpreis1", item.Staffelpreis1 == null ? (object)DBNull.Value : item.Staffelpreis1);
				sqlCommand.Parameters.AddWithValue("Staffelpreis2", item.Staffelpreis2 == null ? (object)DBNull.Value : item.Staffelpreis2);
				sqlCommand.Parameters.AddWithValue("Staffelpreis3", item.Staffelpreis3 == null ? (object)DBNull.Value : item.Staffelpreis3);
				sqlCommand.Parameters.AddWithValue("Staffelpreis4", item.Staffelpreis4 == null ? (object)DBNull.Value : item.Staffelpreis4);
				sqlCommand.Parameters.AddWithValue("Verkaufspreis", item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 44; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> items)
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
						query += " UPDATE [Preisgruppen] SET "

							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Aufschlag]=@Aufschlag" + i + ","
							+ "[Aufschlagsatz]=@Aufschlagsatz" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[bis Anzahl Mengeneinheiten 1]=@bis_Anzahl_Mengeneinheiten_1" + i + ","
							+ "[bis Anzahl Mengeneinheiten 10]=@bis_Anzahl_Mengeneinheiten_10" + i + ","
							+ "[bis Anzahl Mengeneinheiten 2]=@bis_Anzahl_Mengeneinheiten_2" + i + ","
							+ "[bis Anzahl Mengeneinheiten 3]=@bis_Anzahl_Mengeneinheiten_3" + i + ","
							+ "[bis Anzahl Mengeneinheiten 4]=@bis_Anzahl_Mengeneinheiten_4" + i + ","
							+ "[bis Anzahl Mengeneinheiten 5]=@bis_Anzahl_Mengeneinheiten_5" + i + ","
							+ "[bis Anzahl Mengeneinheiten 6]=@bis_Anzahl_Mengeneinheiten_6" + i + ","
							+ "[bis Anzahl Mengeneinheiten 7]=@bis_Anzahl_Mengeneinheiten_7" + i + ","
							+ "[bis Anzahl Mengeneinheiten 8]=@bis_Anzahl_Mengeneinheiten_8" + i + ","
							+ "[bis Anzahl Mengeneinheiten 9]=@bis_Anzahl_Mengeneinheiten_9" + i + ","
							+ "[brutto]=@brutto" + i + ","
							+ "[Einkaufspreis]=@Einkaufspreis" + i + ","
							+ "[kalk_kosten]=@kalk_kosten" + i + ","
							+ "[letzte_Aktualisierung]=@letzte_Aktualisierung" + i + ","
							+ "[ME1]=@ME1" + i + ","
							+ "[ME2]=@ME2" + i + ","
							+ "[ME3]=@ME3" + i + ","
							+ "[ME4]=@ME4" + i + ","
							+ "[PM1]=@PM1" + i + ","
							+ "[PM2]=@PM2" + i + ","
							+ "[PM3]=@PM3" + i + ","
							+ "[PM4]=@PM4" + i + ","
							+ "[Preisgruppe]=@Preisgruppe" + i + ","
							+ "[Preisminderung 1 (%)]=@Preisminderung_1____" + i + ","
							+ "[Preisminderung 10 (%)]=@Preisminderung_10____" + i + ","
							+ "[Preisminderung 2 (%)]=@Preisminderung_2____" + i + ","
							+ "[Preisminderung 3 (%)]=@Preisminderung_3____" + i + ","
							+ "[Preisminderung 4 (%)]=@Preisminderung_4____" + i + ","
							+ "[Preisminderung 5 (%)]=@Preisminderung_5____" + i + ","
							+ "[Preisminderung 6 (%)]=@Preisminderung_6____" + i + ","
							+ "[Preisminderung 7 (%)]=@Preisminderung_7____" + i + ","
							+ "[Preisminderung 8 (%)]=@Preisminderung_8____" + i + ","
							+ "[Preisminderung 9 (%)]=@Preisminderung_9____" + i + ","
							+ "[Staffelpreis1]=@Staffelpreis1" + i + ","
							+ "[Staffelpreis2]=@Staffelpreis2" + i + ","
							+ "[Staffelpreis3]=@Staffelpreis3" + i + ","
							+ "[Staffelpreis4]=@Staffelpreis4" + i + ","
							+ "[Verkaufspreis]=@Verkaufspreis" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
						sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_1" + i, item.bis_Anzahl_Mengeneinheiten_1 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_1);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_10" + i, item.bis_Anzahl_Mengeneinheiten_10 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_10);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_2" + i, item.bis_Anzahl_Mengeneinheiten_2 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_2);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_3" + i, item.bis_Anzahl_Mengeneinheiten_3 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_3);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_4" + i, item.bis_Anzahl_Mengeneinheiten_4 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_4);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_5" + i, item.bis_Anzahl_Mengeneinheiten_5 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_5);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_6" + i, item.bis_Anzahl_Mengeneinheiten_6 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_6);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_7" + i, item.bis_Anzahl_Mengeneinheiten_7 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_7);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_8" + i, item.bis_Anzahl_Mengeneinheiten_8 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_8);
						sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_9" + i, item.bis_Anzahl_Mengeneinheiten_9 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_9);
						sqlCommand.Parameters.AddWithValue("brutto" + i, item.brutto == null ? (object)DBNull.Value : item.brutto);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
						sqlCommand.Parameters.AddWithValue("kalk_kosten" + i, item.kalk_kosten == null ? (object)DBNull.Value : item.kalk_kosten);
						sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung" + i, item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
						sqlCommand.Parameters.AddWithValue("ME1" + i, item.ME1 == null ? (object)DBNull.Value : item.ME1);
						sqlCommand.Parameters.AddWithValue("ME2" + i, item.ME2 == null ? (object)DBNull.Value : item.ME2);
						sqlCommand.Parameters.AddWithValue("ME3" + i, item.ME3 == null ? (object)DBNull.Value : item.ME3);
						sqlCommand.Parameters.AddWithValue("ME4" + i, item.ME4 == null ? (object)DBNull.Value : item.ME4);
						sqlCommand.Parameters.AddWithValue("PM1" + i, item.PM1 == null ? (object)DBNull.Value : item.PM1);
						sqlCommand.Parameters.AddWithValue("PM2" + i, item.PM2 == null ? (object)DBNull.Value : item.PM2);
						sqlCommand.Parameters.AddWithValue("PM3" + i, item.PM3 == null ? (object)DBNull.Value : item.PM3);
						sqlCommand.Parameters.AddWithValue("PM4" + i, item.PM4 == null ? (object)DBNull.Value : item.PM4);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Preisminderung_1____" + i, item.Preisminderung_1____ == null ? (object)DBNull.Value : item.Preisminderung_1____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_10____" + i, item.Preisminderung_10____ == null ? (object)DBNull.Value : item.Preisminderung_10____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_2____" + i, item.Preisminderung_2____ == null ? (object)DBNull.Value : item.Preisminderung_2____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_3____" + i, item.Preisminderung_3____ == null ? (object)DBNull.Value : item.Preisminderung_3____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_4____" + i, item.Preisminderung_4____ == null ? (object)DBNull.Value : item.Preisminderung_4____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_5____" + i, item.Preisminderung_5____ == null ? (object)DBNull.Value : item.Preisminderung_5____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_6____" + i, item.Preisminderung_6____ == null ? (object)DBNull.Value : item.Preisminderung_6____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_7____" + i, item.Preisminderung_7____ == null ? (object)DBNull.Value : item.Preisminderung_7____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_8____" + i, item.Preisminderung_8____ == null ? (object)DBNull.Value : item.Preisminderung_8____);
						sqlCommand.Parameters.AddWithValue("Preisminderung_9____" + i, item.Preisminderung_9____ == null ? (object)DBNull.Value : item.Preisminderung_9____);
						sqlCommand.Parameters.AddWithValue("Staffelpreis1" + i, item.Staffelpreis1 == null ? (object)DBNull.Value : item.Staffelpreis1);
						sqlCommand.Parameters.AddWithValue("Staffelpreis2" + i, item.Staffelpreis2 == null ? (object)DBNull.Value : item.Staffelpreis2);
						sqlCommand.Parameters.AddWithValue("Staffelpreis3" + i, item.Staffelpreis3 == null ? (object)DBNull.Value : item.Staffelpreis3);
						sqlCommand.Parameters.AddWithValue("Staffelpreis4" + i, item.Staffelpreis4 == null ? (object)DBNull.Value : item.Staffelpreis4);
						sqlCommand.Parameters.AddWithValue("Verkaufspreis" + i, item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Preisgruppen] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int DeleteByArticleNr(int articleNr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Preisgruppen] WHERE [Artikel-Nr]=@ArtikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", articleNr);

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

					string query = "DELETE FROM [Preisgruppen] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Preisgruppen] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Preisgruppen]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Preisgruppen] WHERE [Nr] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Preisgruppen] ([Artikel-Nr],[Aufschlag],[Aufschlagsatz],[Bemerkung],[bis Anzahl Mengeneinheiten 1],[bis Anzahl Mengeneinheiten 10],[bis Anzahl Mengeneinheiten 2],[bis Anzahl Mengeneinheiten 3],[bis Anzahl Mengeneinheiten 4],[bis Anzahl Mengeneinheiten 5],[bis Anzahl Mengeneinheiten 6],[bis Anzahl Mengeneinheiten 7],[bis Anzahl Mengeneinheiten 8],[bis Anzahl Mengeneinheiten 9],[brutto],[Einkaufspreis],[kalk_kosten],[letzte_Aktualisierung],[ME1],[ME2],[ME3],[ME4],[PM1],[PM2],[PM3],[PM4],[Preisgruppe],[Preisminderung 1 (%)],[Preisminderung 10 (%)],[Preisminderung 2 (%)],[Preisminderung 3 (%)],[Preisminderung 4 (%)],[Preisminderung 5 (%)],[Preisminderung 6 (%)],[Preisminderung 7 (%)],[Preisminderung 8 (%)],[Preisminderung 9 (%)],[Staffelpreis1],[Staffelpreis2],[Staffelpreis3],[Staffelpreis4],[Verkaufspreis]) OUTPUT INSERTED.[Nr] VALUES (@Artikel_Nr,@Aufschlag,@Aufschlagsatz,@Bemerkung,@bis_Anzahl_Mengeneinheiten_1,@bis_Anzahl_Mengeneinheiten_10,@bis_Anzahl_Mengeneinheiten_2,@bis_Anzahl_Mengeneinheiten_3,@bis_Anzahl_Mengeneinheiten_4,@bis_Anzahl_Mengeneinheiten_5,@bis_Anzahl_Mengeneinheiten_6,@bis_Anzahl_Mengeneinheiten_7,@bis_Anzahl_Mengeneinheiten_8,@bis_Anzahl_Mengeneinheiten_9,@brutto,@Einkaufspreis,@kalk_kosten,@letzte_Aktualisierung,@ME1,@ME2,@ME3,@ME4,@PM1,@PM2,@PM3,@PM4,@Preisgruppe,@Preisminderung_1____,@Preisminderung_10____,@Preisminderung_2____,@Preisminderung_3____,@Preisminderung_4____,@Preisminderung_5____,@Preisminderung_6____,@Preisminderung_7____,@Preisminderung_8____,@Preisminderung_9____,@Staffelpreis1,@Staffelpreis2,@Staffelpreis3,@Staffelpreis4,@Verkaufspreis); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
			sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_1", item.bis_Anzahl_Mengeneinheiten_1 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_1);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_10", item.bis_Anzahl_Mengeneinheiten_10 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_10);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_2", item.bis_Anzahl_Mengeneinheiten_2 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_2);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_3", item.bis_Anzahl_Mengeneinheiten_3 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_3);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_4", item.bis_Anzahl_Mengeneinheiten_4 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_4);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_5", item.bis_Anzahl_Mengeneinheiten_5 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_5);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_6", item.bis_Anzahl_Mengeneinheiten_6 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_6);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_7", item.bis_Anzahl_Mengeneinheiten_7 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_7);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_8", item.bis_Anzahl_Mengeneinheiten_8 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_8);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_9", item.bis_Anzahl_Mengeneinheiten_9 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_9);
			sqlCommand.Parameters.AddWithValue("brutto", item.brutto == null ? (object)DBNull.Value : item.brutto);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
			sqlCommand.Parameters.AddWithValue("kalk_kosten", item.kalk_kosten == null ? (object)DBNull.Value : item.kalk_kosten);
			sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
			sqlCommand.Parameters.AddWithValue("ME1", item.ME1 == null ? (object)DBNull.Value : item.ME1);
			sqlCommand.Parameters.AddWithValue("ME2", item.ME2 == null ? (object)DBNull.Value : item.ME2);
			sqlCommand.Parameters.AddWithValue("ME3", item.ME3 == null ? (object)DBNull.Value : item.ME3);
			sqlCommand.Parameters.AddWithValue("ME4", item.ME4 == null ? (object)DBNull.Value : item.ME4);
			sqlCommand.Parameters.AddWithValue("PM1", item.PM1 == null ? (object)DBNull.Value : item.PM1);
			sqlCommand.Parameters.AddWithValue("PM2", item.PM2 == null ? (object)DBNull.Value : item.PM2);
			sqlCommand.Parameters.AddWithValue("PM3", item.PM3 == null ? (object)DBNull.Value : item.PM3);
			sqlCommand.Parameters.AddWithValue("PM4", item.PM4 == null ? (object)DBNull.Value : item.PM4);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Preisminderung_1____", item.Preisminderung_1____ == null ? (object)DBNull.Value : item.Preisminderung_1____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_10____", item.Preisminderung_10____ == null ? (object)DBNull.Value : item.Preisminderung_10____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_2____", item.Preisminderung_2____ == null ? (object)DBNull.Value : item.Preisminderung_2____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_3____", item.Preisminderung_3____ == null ? (object)DBNull.Value : item.Preisminderung_3____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_4____", item.Preisminderung_4____ == null ? (object)DBNull.Value : item.Preisminderung_4____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_5____", item.Preisminderung_5____ == null ? (object)DBNull.Value : item.Preisminderung_5____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_6____", item.Preisminderung_6____ == null ? (object)DBNull.Value : item.Preisminderung_6____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_7____", item.Preisminderung_7____ == null ? (object)DBNull.Value : item.Preisminderung_7____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_8____", item.Preisminderung_8____ == null ? (object)DBNull.Value : item.Preisminderung_8____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_9____", item.Preisminderung_9____ == null ? (object)DBNull.Value : item.Preisminderung_9____);
			sqlCommand.Parameters.AddWithValue("Staffelpreis1", item.Staffelpreis1 == null ? (object)DBNull.Value : item.Staffelpreis1);
			sqlCommand.Parameters.AddWithValue("Staffelpreis2", item.Staffelpreis2 == null ? (object)DBNull.Value : item.Staffelpreis2);
			sqlCommand.Parameters.AddWithValue("Staffelpreis3", item.Staffelpreis3 == null ? (object)DBNull.Value : item.Staffelpreis3);
			sqlCommand.Parameters.AddWithValue("Staffelpreis4", item.Staffelpreis4 == null ? (object)DBNull.Value : item.Staffelpreis4);
			sqlCommand.Parameters.AddWithValue("Verkaufspreis", item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 44; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Preisgruppen] ([Artikel-Nr],[Aufschlag],[Aufschlagsatz],[Bemerkung],[bis Anzahl Mengeneinheiten 1],[bis Anzahl Mengeneinheiten 10],[bis Anzahl Mengeneinheiten 2],[bis Anzahl Mengeneinheiten 3],[bis Anzahl Mengeneinheiten 4],[bis Anzahl Mengeneinheiten 5],[bis Anzahl Mengeneinheiten 6],[bis Anzahl Mengeneinheiten 7],[bis Anzahl Mengeneinheiten 8],[bis Anzahl Mengeneinheiten 9],[brutto],[Einkaufspreis],[kalk_kosten],[letzte_Aktualisierung],[ME1],[ME2],[ME3],[ME4],[PM1],[PM2],[PM3],[PM4],[Preisgruppe],[Preisminderung 1 (%)],[Preisminderung 10 (%)],[Preisminderung 2 (%)],[Preisminderung 3 (%)],[Preisminderung 4 (%)],[Preisminderung 5 (%)],[Preisminderung 6 (%)],[Preisminderung 7 (%)],[Preisminderung 8 (%)],[Preisminderung 9 (%)],[Staffelpreis1],[Staffelpreis2],[Staffelpreis3],[Staffelpreis4],[Verkaufspreis]) VALUES ( "

						+ "@Artikel_Nr" + i + ","
						+ "@Aufschlag" + i + ","
						+ "@Aufschlagsatz" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_1" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_10" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_2" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_3" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_4" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_5" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_6" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_7" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_8" + i + ","
						+ "@bis_Anzahl_Mengeneinheiten_9" + i + ","
						+ "@brutto" + i + ","
						+ "@Einkaufspreis" + i + ","
						+ "@kalk_kosten" + i + ","
						+ "@letzte_Aktualisierung" + i + ","
						+ "@ME1" + i + ","
						+ "@ME2" + i + ","
						+ "@ME3" + i + ","
						+ "@ME4" + i + ","
						+ "@PM1" + i + ","
						+ "@PM2" + i + ","
						+ "@PM3" + i + ","
						+ "@PM4" + i + ","
						+ "@Preisgruppe" + i + ","
						+ "@Preisminderung_1____" + i + ","
						+ "@Preisminderung_10____" + i + ","
						+ "@Preisminderung_2____" + i + ","
						+ "@Preisminderung_3____" + i + ","
						+ "@Preisminderung_4____" + i + ","
						+ "@Preisminderung_5____" + i + ","
						+ "@Preisminderung_6____" + i + ","
						+ "@Preisminderung_7____" + i + ","
						+ "@Preisminderung_8____" + i + ","
						+ "@Preisminderung_9____" + i + ","
						+ "@Staffelpreis1" + i + ","
						+ "@Staffelpreis2" + i + ","
						+ "@Staffelpreis3" + i + ","
						+ "@Staffelpreis4" + i + ","
						+ "@Verkaufspreis" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
					sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_1" + i, item.bis_Anzahl_Mengeneinheiten_1 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_1);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_10" + i, item.bis_Anzahl_Mengeneinheiten_10 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_10);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_2" + i, item.bis_Anzahl_Mengeneinheiten_2 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_2);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_3" + i, item.bis_Anzahl_Mengeneinheiten_3 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_3);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_4" + i, item.bis_Anzahl_Mengeneinheiten_4 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_4);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_5" + i, item.bis_Anzahl_Mengeneinheiten_5 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_5);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_6" + i, item.bis_Anzahl_Mengeneinheiten_6 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_6);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_7" + i, item.bis_Anzahl_Mengeneinheiten_7 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_7);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_8" + i, item.bis_Anzahl_Mengeneinheiten_8 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_8);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_9" + i, item.bis_Anzahl_Mengeneinheiten_9 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_9);
					sqlCommand.Parameters.AddWithValue("brutto" + i, item.brutto == null ? (object)DBNull.Value : item.brutto);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
					sqlCommand.Parameters.AddWithValue("kalk_kosten" + i, item.kalk_kosten == null ? (object)DBNull.Value : item.kalk_kosten);
					sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung" + i, item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
					sqlCommand.Parameters.AddWithValue("ME1" + i, item.ME1 == null ? (object)DBNull.Value : item.ME1);
					sqlCommand.Parameters.AddWithValue("ME2" + i, item.ME2 == null ? (object)DBNull.Value : item.ME2);
					sqlCommand.Parameters.AddWithValue("ME3" + i, item.ME3 == null ? (object)DBNull.Value : item.ME3);
					sqlCommand.Parameters.AddWithValue("ME4" + i, item.ME4 == null ? (object)DBNull.Value : item.ME4);
					sqlCommand.Parameters.AddWithValue("PM1" + i, item.PM1 == null ? (object)DBNull.Value : item.PM1);
					sqlCommand.Parameters.AddWithValue("PM2" + i, item.PM2 == null ? (object)DBNull.Value : item.PM2);
					sqlCommand.Parameters.AddWithValue("PM3" + i, item.PM3 == null ? (object)DBNull.Value : item.PM3);
					sqlCommand.Parameters.AddWithValue("PM4" + i, item.PM4 == null ? (object)DBNull.Value : item.PM4);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Preisminderung_1____" + i, item.Preisminderung_1____ == null ? (object)DBNull.Value : item.Preisminderung_1____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_10____" + i, item.Preisminderung_10____ == null ? (object)DBNull.Value : item.Preisminderung_10____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_2____" + i, item.Preisminderung_2____ == null ? (object)DBNull.Value : item.Preisminderung_2____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_3____" + i, item.Preisminderung_3____ == null ? (object)DBNull.Value : item.Preisminderung_3____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_4____" + i, item.Preisminderung_4____ == null ? (object)DBNull.Value : item.Preisminderung_4____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_5____" + i, item.Preisminderung_5____ == null ? (object)DBNull.Value : item.Preisminderung_5____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_6____" + i, item.Preisminderung_6____ == null ? (object)DBNull.Value : item.Preisminderung_6____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_7____" + i, item.Preisminderung_7____ == null ? (object)DBNull.Value : item.Preisminderung_7____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_8____" + i, item.Preisminderung_8____ == null ? (object)DBNull.Value : item.Preisminderung_8____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_9____" + i, item.Preisminderung_9____ == null ? (object)DBNull.Value : item.Preisminderung_9____);
					sqlCommand.Parameters.AddWithValue("Staffelpreis1" + i, item.Staffelpreis1 == null ? (object)DBNull.Value : item.Staffelpreis1);
					sqlCommand.Parameters.AddWithValue("Staffelpreis2" + i, item.Staffelpreis2 == null ? (object)DBNull.Value : item.Staffelpreis2);
					sqlCommand.Parameters.AddWithValue("Staffelpreis3" + i, item.Staffelpreis3 == null ? (object)DBNull.Value : item.Staffelpreis3);
					sqlCommand.Parameters.AddWithValue("Staffelpreis4" + i, item.Staffelpreis4 == null ? (object)DBNull.Value : item.Staffelpreis4);
					sqlCommand.Parameters.AddWithValue("Verkaufspreis" + i, item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Preisgruppen] SET [Artikel-Nr]=@Artikel_Nr, [Aufschlag]=@Aufschlag, [Aufschlagsatz]=@Aufschlagsatz, [Bemerkung]=@Bemerkung, [bis Anzahl Mengeneinheiten 1]=@bis_Anzahl_Mengeneinheiten_1, [bis Anzahl Mengeneinheiten 10]=@bis_Anzahl_Mengeneinheiten_10, [bis Anzahl Mengeneinheiten 2]=@bis_Anzahl_Mengeneinheiten_2, [bis Anzahl Mengeneinheiten 3]=@bis_Anzahl_Mengeneinheiten_3, [bis Anzahl Mengeneinheiten 4]=@bis_Anzahl_Mengeneinheiten_4, [bis Anzahl Mengeneinheiten 5]=@bis_Anzahl_Mengeneinheiten_5, [bis Anzahl Mengeneinheiten 6]=@bis_Anzahl_Mengeneinheiten_6, [bis Anzahl Mengeneinheiten 7]=@bis_Anzahl_Mengeneinheiten_7, [bis Anzahl Mengeneinheiten 8]=@bis_Anzahl_Mengeneinheiten_8, [bis Anzahl Mengeneinheiten 9]=@bis_Anzahl_Mengeneinheiten_9, [brutto]=@brutto, [Einkaufspreis]=@Einkaufspreis, [kalk_kosten]=@kalk_kosten, [letzte_Aktualisierung]=@letzte_Aktualisierung, [ME1]=@ME1, [ME2]=@ME2, [ME3]=@ME3, [ME4]=@ME4, [PM1]=@PM1, [PM2]=@PM2, [PM3]=@PM3, [PM4]=@PM4, [Preisgruppe]=@Preisgruppe, [Preisminderung 1 (%)]=@Preisminderung_1____, [Preisminderung 10 (%)]=@Preisminderung_10____, [Preisminderung 2 (%)]=@Preisminderung_2____, [Preisminderung 3 (%)]=@Preisminderung_3____, [Preisminderung 4 (%)]=@Preisminderung_4____, [Preisminderung 5 (%)]=@Preisminderung_5____, [Preisminderung 6 (%)]=@Preisminderung_6____, [Preisminderung 7 (%)]=@Preisminderung_7____, [Preisminderung 8 (%)]=@Preisminderung_8____, [Preisminderung 9 (%)]=@Preisminderung_9____, [Staffelpreis1]=@Staffelpreis1, [Staffelpreis2]=@Staffelpreis2, [Staffelpreis3]=@Staffelpreis3, [Staffelpreis4]=@Staffelpreis4, [Verkaufspreis]=@Verkaufspreis WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Aufschlag", item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
			sqlCommand.Parameters.AddWithValue("Aufschlagsatz", item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_1", item.bis_Anzahl_Mengeneinheiten_1 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_1);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_10", item.bis_Anzahl_Mengeneinheiten_10 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_10);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_2", item.bis_Anzahl_Mengeneinheiten_2 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_2);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_3", item.bis_Anzahl_Mengeneinheiten_3 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_3);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_4", item.bis_Anzahl_Mengeneinheiten_4 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_4);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_5", item.bis_Anzahl_Mengeneinheiten_5 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_5);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_6", item.bis_Anzahl_Mengeneinheiten_6 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_6);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_7", item.bis_Anzahl_Mengeneinheiten_7 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_7);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_8", item.bis_Anzahl_Mengeneinheiten_8 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_8);
			sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_9", item.bis_Anzahl_Mengeneinheiten_9 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_9);
			sqlCommand.Parameters.AddWithValue("brutto", item.brutto == null ? (object)DBNull.Value : item.brutto);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
			sqlCommand.Parameters.AddWithValue("kalk_kosten", item.kalk_kosten == null ? (object)DBNull.Value : item.kalk_kosten);
			sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
			sqlCommand.Parameters.AddWithValue("ME1", item.ME1 == null ? (object)DBNull.Value : item.ME1);
			sqlCommand.Parameters.AddWithValue("ME2", item.ME2 == null ? (object)DBNull.Value : item.ME2);
			sqlCommand.Parameters.AddWithValue("ME3", item.ME3 == null ? (object)DBNull.Value : item.ME3);
			sqlCommand.Parameters.AddWithValue("ME4", item.ME4 == null ? (object)DBNull.Value : item.ME4);
			sqlCommand.Parameters.AddWithValue("PM1", item.PM1 == null ? (object)DBNull.Value : item.PM1);
			sqlCommand.Parameters.AddWithValue("PM2", item.PM2 == null ? (object)DBNull.Value : item.PM2);
			sqlCommand.Parameters.AddWithValue("PM3", item.PM3 == null ? (object)DBNull.Value : item.PM3);
			sqlCommand.Parameters.AddWithValue("PM4", item.PM4 == null ? (object)DBNull.Value : item.PM4);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Preisminderung_1____", item.Preisminderung_1____ == null ? (object)DBNull.Value : item.Preisminderung_1____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_10____", item.Preisminderung_10____ == null ? (object)DBNull.Value : item.Preisminderung_10____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_2____", item.Preisminderung_2____ == null ? (object)DBNull.Value : item.Preisminderung_2____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_3____", item.Preisminderung_3____ == null ? (object)DBNull.Value : item.Preisminderung_3____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_4____", item.Preisminderung_4____ == null ? (object)DBNull.Value : item.Preisminderung_4____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_5____", item.Preisminderung_5____ == null ? (object)DBNull.Value : item.Preisminderung_5____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_6____", item.Preisminderung_6____ == null ? (object)DBNull.Value : item.Preisminderung_6____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_7____", item.Preisminderung_7____ == null ? (object)DBNull.Value : item.Preisminderung_7____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_8____", item.Preisminderung_8____ == null ? (object)DBNull.Value : item.Preisminderung_8____);
			sqlCommand.Parameters.AddWithValue("Preisminderung_9____", item.Preisminderung_9____ == null ? (object)DBNull.Value : item.Preisminderung_9____);
			sqlCommand.Parameters.AddWithValue("Staffelpreis1", item.Staffelpreis1 == null ? (object)DBNull.Value : item.Staffelpreis1);
			sqlCommand.Parameters.AddWithValue("Staffelpreis2", item.Staffelpreis2 == null ? (object)DBNull.Value : item.Staffelpreis2);
			sqlCommand.Parameters.AddWithValue("Staffelpreis3", item.Staffelpreis3 == null ? (object)DBNull.Value : item.Staffelpreis3);
			sqlCommand.Parameters.AddWithValue("Staffelpreis4", item.Staffelpreis4 == null ? (object)DBNull.Value : item.Staffelpreis4);
			sqlCommand.Parameters.AddWithValue("Verkaufspreis", item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 44; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Preisgruppen] SET "

					+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
					+ "[Aufschlag]=@Aufschlag" + i + ","
					+ "[Aufschlagsatz]=@Aufschlagsatz" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[bis Anzahl Mengeneinheiten 1]=@bis_Anzahl_Mengeneinheiten_1" + i + ","
					+ "[bis Anzahl Mengeneinheiten 10]=@bis_Anzahl_Mengeneinheiten_10" + i + ","
					+ "[bis Anzahl Mengeneinheiten 2]=@bis_Anzahl_Mengeneinheiten_2" + i + ","
					+ "[bis Anzahl Mengeneinheiten 3]=@bis_Anzahl_Mengeneinheiten_3" + i + ","
					+ "[bis Anzahl Mengeneinheiten 4]=@bis_Anzahl_Mengeneinheiten_4" + i + ","
					+ "[bis Anzahl Mengeneinheiten 5]=@bis_Anzahl_Mengeneinheiten_5" + i + ","
					+ "[bis Anzahl Mengeneinheiten 6]=@bis_Anzahl_Mengeneinheiten_6" + i + ","
					+ "[bis Anzahl Mengeneinheiten 7]=@bis_Anzahl_Mengeneinheiten_7" + i + ","
					+ "[bis Anzahl Mengeneinheiten 8]=@bis_Anzahl_Mengeneinheiten_8" + i + ","
					+ "[bis Anzahl Mengeneinheiten 9]=@bis_Anzahl_Mengeneinheiten_9" + i + ","
					+ "[brutto]=@brutto" + i + ","
					+ "[Einkaufspreis]=@Einkaufspreis" + i + ","
					+ "[kalk_kosten]=@kalk_kosten" + i + ","
					+ "[letzte_Aktualisierung]=@letzte_Aktualisierung" + i + ","
					+ "[ME1]=@ME1" + i + ","
					+ "[ME2]=@ME2" + i + ","
					+ "[ME3]=@ME3" + i + ","
					+ "[ME4]=@ME4" + i + ","
					+ "[PM1]=@PM1" + i + ","
					+ "[PM2]=@PM2" + i + ","
					+ "[PM3]=@PM3" + i + ","
					+ "[PM4]=@PM4" + i + ","
					+ "[Preisgruppe]=@Preisgruppe" + i + ","
					+ "[Preisminderung 1 (%)]=@Preisminderung_1____" + i + ","
					+ "[Preisminderung 10 (%)]=@Preisminderung_10____" + i + ","
					+ "[Preisminderung 2 (%)]=@Preisminderung_2____" + i + ","
					+ "[Preisminderung 3 (%)]=@Preisminderung_3____" + i + ","
					+ "[Preisminderung 4 (%)]=@Preisminderung_4____" + i + ","
					+ "[Preisminderung 5 (%)]=@Preisminderung_5____" + i + ","
					+ "[Preisminderung 6 (%)]=@Preisminderung_6____" + i + ","
					+ "[Preisminderung 7 (%)]=@Preisminderung_7____" + i + ","
					+ "[Preisminderung 8 (%)]=@Preisminderung_8____" + i + ","
					+ "[Preisminderung 9 (%)]=@Preisminderung_9____" + i + ","
					+ "[Staffelpreis1]=@Staffelpreis1" + i + ","
					+ "[Staffelpreis2]=@Staffelpreis2" + i + ","
					+ "[Staffelpreis3]=@Staffelpreis3" + i + ","
					+ "[Staffelpreis4]=@Staffelpreis4" + i + ","
					+ "[Verkaufspreis]=@Verkaufspreis" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Aufschlag" + i, item.Aufschlag == null ? (object)DBNull.Value : item.Aufschlag);
					sqlCommand.Parameters.AddWithValue("Aufschlagsatz" + i, item.Aufschlagsatz == null ? (object)DBNull.Value : item.Aufschlagsatz);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_1" + i, item.bis_Anzahl_Mengeneinheiten_1 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_1);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_10" + i, item.bis_Anzahl_Mengeneinheiten_10 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_10);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_2" + i, item.bis_Anzahl_Mengeneinheiten_2 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_2);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_3" + i, item.bis_Anzahl_Mengeneinheiten_3 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_3);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_4" + i, item.bis_Anzahl_Mengeneinheiten_4 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_4);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_5" + i, item.bis_Anzahl_Mengeneinheiten_5 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_5);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_6" + i, item.bis_Anzahl_Mengeneinheiten_6 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_6);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_7" + i, item.bis_Anzahl_Mengeneinheiten_7 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_7);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_8" + i, item.bis_Anzahl_Mengeneinheiten_8 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_8);
					sqlCommand.Parameters.AddWithValue("bis_Anzahl_Mengeneinheiten_9" + i, item.bis_Anzahl_Mengeneinheiten_9 == null ? (object)DBNull.Value : item.bis_Anzahl_Mengeneinheiten_9);
					sqlCommand.Parameters.AddWithValue("brutto" + i, item.brutto == null ? (object)DBNull.Value : item.brutto);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
					sqlCommand.Parameters.AddWithValue("kalk_kosten" + i, item.kalk_kosten == null ? (object)DBNull.Value : item.kalk_kosten);
					sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung" + i, item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
					sqlCommand.Parameters.AddWithValue("ME1" + i, item.ME1 == null ? (object)DBNull.Value : item.ME1);
					sqlCommand.Parameters.AddWithValue("ME2" + i, item.ME2 == null ? (object)DBNull.Value : item.ME2);
					sqlCommand.Parameters.AddWithValue("ME3" + i, item.ME3 == null ? (object)DBNull.Value : item.ME3);
					sqlCommand.Parameters.AddWithValue("ME4" + i, item.ME4 == null ? (object)DBNull.Value : item.ME4);
					sqlCommand.Parameters.AddWithValue("PM1" + i, item.PM1 == null ? (object)DBNull.Value : item.PM1);
					sqlCommand.Parameters.AddWithValue("PM2" + i, item.PM2 == null ? (object)DBNull.Value : item.PM2);
					sqlCommand.Parameters.AddWithValue("PM3" + i, item.PM3 == null ? (object)DBNull.Value : item.PM3);
					sqlCommand.Parameters.AddWithValue("PM4" + i, item.PM4 == null ? (object)DBNull.Value : item.PM4);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Preisminderung_1____" + i, item.Preisminderung_1____ == null ? (object)DBNull.Value : item.Preisminderung_1____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_10____" + i, item.Preisminderung_10____ == null ? (object)DBNull.Value : item.Preisminderung_10____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_2____" + i, item.Preisminderung_2____ == null ? (object)DBNull.Value : item.Preisminderung_2____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_3____" + i, item.Preisminderung_3____ == null ? (object)DBNull.Value : item.Preisminderung_3____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_4____" + i, item.Preisminderung_4____ == null ? (object)DBNull.Value : item.Preisminderung_4____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_5____" + i, item.Preisminderung_5____ == null ? (object)DBNull.Value : item.Preisminderung_5____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_6____" + i, item.Preisminderung_6____ == null ? (object)DBNull.Value : item.Preisminderung_6____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_7____" + i, item.Preisminderung_7____ == null ? (object)DBNull.Value : item.Preisminderung_7____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_8____" + i, item.Preisminderung_8____ == null ? (object)DBNull.Value : item.Preisminderung_8____);
					sqlCommand.Parameters.AddWithValue("Preisminderung_9____" + i, item.Preisminderung_9____ == null ? (object)DBNull.Value : item.Preisminderung_9____);
					sqlCommand.Parameters.AddWithValue("Staffelpreis1" + i, item.Staffelpreis1 == null ? (object)DBNull.Value : item.Staffelpreis1);
					sqlCommand.Parameters.AddWithValue("Staffelpreis2" + i, item.Staffelpreis2 == null ? (object)DBNull.Value : item.Staffelpreis2);
					sqlCommand.Parameters.AddWithValue("Staffelpreis3" + i, item.Staffelpreis3 == null ? (object)DBNull.Value : item.Staffelpreis3);
					sqlCommand.Parameters.AddWithValue("Staffelpreis4" + i, item.Staffelpreis4 == null ? (object)DBNull.Value : item.Staffelpreis4);
					sqlCommand.Parameters.AddWithValue("Verkaufspreis" + i, item.Verkaufspreis == null ? (object)DBNull.Value : item.Verkaufspreis);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Preisgruppen] WHERE [Nr]=@Nr";
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

				string query = "DELETE FROM [Preisgruppen] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction

		#endregion

		#region Custom Methods


		public static Entities.Tables.PRS.PreisgruppenEntity GetByArtikelNr(int artikelNr)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Preisgruppen] WHERE [Artikel-Nr]=@artikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return null;
			}

			return new Entities.Tables.PRS.PreisgruppenEntity(dataTable.Rows[0]);
		}
		public static List<Entities.Tables.PRS.PreisgruppenEntity> GetByArtikelNr(List<int> artikelNrs, SqlConnection connection, SqlTransaction transaction)
		{
			if(artikelNrs?.Count<=0)
			{
				return null;
			}
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [Preisgruppen] WHERE [Artikel-Nr] IN ({string.Join(",", artikelNrs)})";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.PRS.PreisgruppenEntity>();
			}
		}
		public static Entities.Tables.PRS.PreisgruppenEntity GetByArtikelNr(int artikelNr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Preisgruppen] WHERE [Artikel-Nr]=@artikelNr";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count == 0)
			{
				return null;
			}

			return new Entities.Tables.PRS.PreisgruppenEntity(dataTable.Rows[0]);
		}
		public static List<Entities.Tables.PRS.PreisgruppenEntity> GetByArtikelNrs(List<int> artikelNrs)
		{
			if(artikelNrs != null && artikelNrs.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.PreisgruppenEntity>();
				if(artikelNrs.Count <= maxQueryNumber)
				{
					result = getByArtikelNrs(artikelNrs);
				}
				else
				{
					int batchNumber = artikelNrs.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.PreisgruppenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByArtikelNrs(artikelNrs.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getByArtikelNrs(artikelNrs.GetRange(batchNumber * maxQueryNumber, artikelNrs.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.PreisgruppenEntity>();
		}
		private static List<Entities.Tables.PRS.PreisgruppenEntity> getByArtikelNrs(List<int> artikelNrs)
		{
			if(artikelNrs != null && artikelNrs.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < artikelNrs.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, artikelNrs[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Preisgruppen] WHERE [Artikel-Nr] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.PRS.PreisgruppenEntity>();
				}
			}
			return new List<Entities.Tables.PRS.PreisgruppenEntity>();
		}

		public static List<Entities.Tables.PRS.PreisgruppenEntity> GetByArtikelNrs(List<int> artikelNrs, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(artikelNrs != null && artikelNrs.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.PreisgruppenEntity>();
				if(artikelNrs.Count <= maxQueryNumber)
				{
					result = getByArtikelNrs(artikelNrs, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = artikelNrs.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.PreisgruppenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByArtikelNrs(artikelNrs.GetRange(i * maxQueryNumber, maxQueryNumber), sqlConnection, sqlTransaction));
					}
					result.AddRange(getByArtikelNrs(artikelNrs.GetRange(batchNumber * maxQueryNumber, artikelNrs.Count - batchNumber * maxQueryNumber), sqlConnection, sqlTransaction));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.PreisgruppenEntity>();
		}
		private static List<Entities.Tables.PRS.PreisgruppenEntity> getByArtikelNrs(List<int> artikelNrs, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(artikelNrs != null && artikelNrs.Count > 0)
			{
				var dataTable = new DataTable();
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.Transaction = sqlTransaction;

				string queryIds = string.Empty;
				for(int i = 0; i < artikelNrs.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, artikelNrs[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = "SELECT * FROM [Preisgruppen] WHERE [Artikel-Nr] IN (" + queryIds + ")";
				DbExecution.Fill(sqlCommand, dataTable);


				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.PRS.PreisgruppenEntity>();
				}
			}
			return new List<Entities.Tables.PRS.PreisgruppenEntity>();
		}

		public static Entities.Tables.PRS.PreisgruppenEntity GetByArtikelNrAndType(int artikelNr, int type)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Preisgruppen] WHERE [Artikel-Nr]=@artikelNr AND [Preisgruppe]=@type";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("type", type);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return null;
			}

			return new Entities.Tables.PRS.PreisgruppenEntity(dataTable.Rows[0]);
		}
		public static Entities.Tables.PRS.PreisgruppenEntity GetByArtikelNrAndType(int artikelNr, int type, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Preisgruppen] WHERE [Artikel-Nr]=@artikelNr AND [Preisgruppe]=@type";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("type", type);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return null;
			}

			return new Entities.Tables.PRS.PreisgruppenEntity(dataTable.Rows[0]);
		}
		public static int UpdatePurchasePrice(decimal newPurchasePrice, int articleId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Preisgruppen] SET [Einkaufspreis]=@Einkaufspreis, [letzte_Aktualisierung]=@letzte_Aktualisierung WHERE [Artikel-Nr]=@Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleId);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis", newPurchasePrice);
				sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", DateTime.Now);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdatePurchasePrice(decimal newPurchasePrice, int articleId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE [Preisgruppen] SET [Einkaufspreis]=@Einkaufspreis, [letzte_Aktualisierung]=@letzte_Aktualisierung WHERE [Artikel-Nr]=@Artikel_Nr";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Artikel_Nr", articleId);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis", newPurchasePrice);
			sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", DateTime.Now);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int UpdateStatisticsSalesPrice(List<Entities.Tables.PRS.PreisgruppenStatisticsEntity> data, string userName)
		{
			if(data == null || data.Count <= 0)
				return 0;

			// - 
			#region comments
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');

				string swapQuery = "";

				string insertValues = "";
				for(int i = 0; i < data.Count; i++)
				{
					insertValues += $"(@Artikelnummer_{i}, @prix1_{i}, @bis1_{i}, @prix2_{i}, @bis2_{i}, @prix3_{i}, @bis3_{i}, @prix4_{i}, @bis4_{i}),";
				}
				insertValues = insertValues.TrimEnd(',');
				// -
				swapQuery = $"INSERT INTO #PrixModifier_{connectionId}(artikelnummer,prix1,bis1,prix2,bis2,prix3,bis3,prix4,bis4) VALUES {insertValues};";

				// - 15 | 19
				string query = $@"// * BUTTON 15 * //
			// * Q1 * //
			IF OBJECT_ID('tempdb..#PrixModifier_{connectionId}') IS NOT NULL DROP TABLE #PrixModifier_{connectionId};

                                    CREATE TABLE #PrixModifier_{connectionId} (artikelnummer varchar(50),prix1 decimal(20,7),bis1 decimal(20,7),prix2 decimal(20,7),bis2 decimal(20,7),prix3 decimal(20,7),bis3 decimal(20,7),prix4 decimal(20,7),bis4 decimal(20,7));
                                    {swapQuery}

			// * Q2 -- Weird [Artikel Trouver Update VK T] NOT EXISTS * //
			// * SELECT Pr.artikelnummer, B.Verkaufspreis AS [Alte Preis], Pr.prix1 AS [Neue Preis] INTO [Artikel Trouver Update VK T]
				FROM #PrixModifier_{connectionId} AS Pr INNER JOIN (Artikel AS A INNER JOIN Preisgruppen AS B ON A.[Artikel-Nr] = B.[Artikel-Nr]) 
					ON Pr.artikelnummer = A.Artikelnummer;
				* //

			// * Q3 * //
			INSERT INTO tbl_Historie_VK_Update(Datum, Artikelnummer, [Alte Preis], [Neue Preis], [User])

										SELECT GetDate() Datum, Pr.artikelnummer, Preisgruppen.Verkaufspreis[Alte Preis], prix1[Neue Preis], '{userName}'[User]

										FROM #PrixModifier_{connectionId} Pr INNER JOIN (Artikel INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                        ON Pr.artikelnummer = Artikel.Artikelnummer;

			// * Q4 * //
			UPDATE Preisgruppen

										SET Verkaufspreis = [prix1],
										Staffelpreis1 = IIf(Pr.prix2 Is Not Null, Pr.prix1, Null),
										PM1 = 100 - ([prix1] /[prix1] * 100),
										ME1 = Pr.bis1,
										Staffelpreis2 = Pr.prix2,
										PM2 = 100 - ([prix2] /[prix1] * 100),
										ME2 = IIf(Pr.prix2 Is Not Null And[bis2] Is Null, 999999999, Pr.bis2),
										Staffelpreis3 = Pr.prix3,
										PM3 = 100 - ([prix3] /[prix1] * 100),
										ME3 = IIf(Pr.prix3 Is Not Null And[bis3] Is Null, 999999999, Pr.bis3),
										Staffelpreis4 = Pr.prix4,
										PM4 = 100 - ([prix4] /[prix1] * 100),
										ME4 = IIf(Pr.prix4 Is Not Null And([bis4] Is Null Or[bis4] = 0),999999999,Pr.bis4)
	                                    FROM #PrixModifier_{connectionId} AS Pr 
	                                    INNER JOIN(Artikel AS A INNER JOIN Preisgruppen AS B ON A.[Artikel - Nr] = B.[Artikel - Nr])

										ON Pr.artikelnummer = A.Artikelnummer
										;
			";

				// - START
				SqlTransaction transaction = sqlConnection.BeginTransaction();
				using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, transaction))
				{
					for(int i = 0; i < data.Count; i++)
					{
						sqlCommand.Parameters.AddWithValue($"Artikelnummer_{i}", data[i].Artikelnummer == null ? (object)DBNull.Value : data[i].Artikelnummer.Trim());
						sqlCommand.Parameters.AddWithValue($"prix1_{i}", data[i].Preis1 == null ? (object)DBNull.Value : data[i].Preis1);
						sqlCommand.Parameters.AddWithValue($"bis1_{i}", data[i].Bis1 == null ? (object)DBNull.Value : data[i].Bis1);
						sqlCommand.Parameters.AddWithValue($"prix2_{i}", data[i].Preis2 == null ? (object)DBNull.Value : data[i].Preis2);
						sqlCommand.Parameters.AddWithValue($"bis2_{i}", data[i].Bis2 == null ? (object)DBNull.Value : data[i].Bis2);
						sqlCommand.Parameters.AddWithValue($"prix3_{i}", data[i].Preis3 == null ? (object)DBNull.Value : data[i].Preis3);
						sqlCommand.Parameters.AddWithValue($"bis3_{i}", data[i].Bis3 == null ? (object)DBNull.Value : data[i].Bis3);
						sqlCommand.Parameters.AddWithValue($"prix4_{i}", data[i].Preis4 == null ? (object)DBNull.Value : data[i].Preis4);
						sqlCommand.Parameters.AddWithValue($"bis4_{i}", data[i].Bis4 == null ? (object)DBNull.Value : data[i].Bis4);
					}
					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				// - END
				transaction.Commit();
			}
			#endregion

			// - 
			return 0;
		}
		public static int UpdateStatisticsSalesPriceTrans(List<Entities.Tables.PRS.PreisgruppenStatisticsEntity> data, string userName, SqlConnection connection, SqlTransaction transaction)
		{
			if(data == null || data.Count <= 0)
				return 0;

			// - 
			#region comments
			/*
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');

				string swapQuery = "";

				string insertValues = "";
				for(int i = 0; i < data.Count; i++)
				{
					insertValues += $"(@Artikelnummer_{i}, @prix1_{i}, @bis1_{i}, @prix2_{i}, @bis2_{i}, @prix3_{i}, @bis3_{i}, @prix4_{i}, @bis4_{i}),";
				}
				insertValues = insertValues.TrimEnd(',');
				// -
				swapQuery = $"INSERT INTO #PrixModifier_{connectionId}(artikelnummer,prix1,bis1,prix2,bis2,prix3,bis3,prix4,bis4) VALUES {insertValues};";

				// - 15 | 19
				string query = $@"// * BUTTON 15 * //
			// * Q1 * //
			IF OBJECT_ID('tempdb..#PrixModifier_{connectionId}') IS NOT NULL DROP TABLE #PrixModifier_{connectionId};

                                    CREATE TABLE #PrixModifier_{connectionId} (artikelnummer varchar(50),prix1 decimal(20,7),bis1 decimal(20,7),prix2 decimal(20,7),bis2 decimal(20,7),prix3 decimal(20,7),bis3 decimal(20,7),prix4 decimal(20,7),bis4 decimal(20,7));
                                    { swapQuery}

			// * Q2 -- Weird [Artikel Trouver Update VK T] NOT EXISTS * //
			// * SELECT Pr.artikelnummer, B.Verkaufspreis AS [Alte Preis], Pr.prix1 AS [Neue Preis] INTO [Artikel Trouver Update VK T]
				FROM #PrixModifier_{connectionId} AS Pr INNER JOIN (Artikel AS A INNER JOIN Preisgruppen AS B ON A.[Artikel-Nr] = B.[Artikel-Nr]) 
					ON Pr.artikelnummer = A.Artikelnummer;
				* //

			// * Q3 * //
			INSERT INTO tbl_Historie_VK_Update(Datum, Artikelnummer, [Alte Preis], [Neue Preis], [User])

										SELECT GetDate() Datum, Pr.artikelnummer, Preisgruppen.Verkaufspreis[Alte Preis], prix1[Neue Preis], '{userName}'[User]

										FROM #PrixModifier_{connectionId} Pr INNER JOIN (Artikel INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                        ON Pr.artikelnummer = Artikel.Artikelnummer;

			// * Q4 * //
			UPDATE Preisgruppen

										SET Verkaufspreis = [prix1],
										Staffelpreis1 = IIf(Pr.prix2 Is Not Null, Pr.prix1, Null),
										PM1 = 100 - ([prix1] /[prix1] * 100),
										ME1 = Pr.bis1,
										Staffelpreis2 = Pr.prix2,
										PM2 = 100 - ([prix2] /[prix1] * 100),
										ME2 = IIf(Pr.prix2 Is Not Null And[bis2] Is Null, 999999999, Pr.bis2),
										Staffelpreis3 = Pr.prix3,
										PM3 = 100 - ([prix3] /[prix1] * 100),
										ME3 = IIf(Pr.prix3 Is Not Null And[bis3] Is Null, 999999999, Pr.bis3),
										Staffelpreis4 = Pr.prix4,
										PM4 = 100 - ([prix4] /[prix1] * 100),
										ME4 = IIf(Pr.prix4 Is Not Null And([bis4] Is Null Or[bis4] = 0),999999999,Pr.bis4)
	                                    FROM #PrixModifier_{connectionId} AS Pr 
	                                    INNER JOIN(Artikel AS A INNER JOIN Preisgruppen AS B ON A.[Artikel - Nr] = B.[Artikel - Nr])

										ON Pr.artikelnummer = A.Artikelnummer
										;
			";

				// - START
				SqlTransaction transaction = sqlConnection.BeginTransaction();
			using(SqlCommand _sqlCommand = new SqlCommand(query, sqlConnection, transaction))
			{
				for(int i = 0; i < data.Count; i++)
				{
					_sqlCommand.Parameters.AddWithValue($"Artikelnummer_{i}", data[i].Artikelnummer == null ? (object)DBNull.Value : data[i].Artikelnummer.Trim());
					_sqlCommand.Parameters.AddWithValue($"prix1_{i}", data[i].Preis1 == null ? (object)DBNull.Value : data[i].Preis1);
					_sqlCommand.Parameters.AddWithValue($"bis1_{i}", data[i].Bis1 == null ? (object)DBNull.Value : data[i].Bis1);
					_sqlCommand.Parameters.AddWithValue($"prix2_{i}", data[i].Preis2 == null ? (object)DBNull.Value : data[i].Preis2);
					_sqlCommand.Parameters.AddWithValue($"bis2_{i}", data[i].Bis2 == null ? (object)DBNull.Value : data[i].Bis2);
					_sqlCommand.Parameters.AddWithValue($"prix3_{i}", data[i].Preis3 == null ? (object)DBNull.Value : data[i].Preis3);
					_sqlCommand.Parameters.AddWithValue($"bis3_{i}", data[i].Bis3 == null ? (object)DBNull.Value : data[i].Bis3);
					_sqlCommand.Parameters.AddWithValue($"prix4_{i}", data[i].Preis4 == null ? (object)DBNull.Value : data[i].Preis4);
					_sqlCommand.Parameters.AddWithValue($"bis4_{i}", data[i].Bis4 == null ? (object)DBNull.Value : data[i].Bis4);
				}
				DbExecution.ExecuteNonQuery(_sqlCommand);
			}

			// - END
			transaction.Commit();
		}
			*/
			#endregion

			int maxCmdParams = 200;
			if(data.Count > maxCmdParams)
			{
				for(int i = 0; i < Math.Ceiling((decimal)data.Count / maxCmdParams); i++)
				{
					updateStatisticsSalesPrice(i, data.Take(new Range(i * maxCmdParams, (i + 1) * maxCmdParams)).ToList(), userName, connection, transaction);
				}
			}
			else
			{
				updateStatisticsSalesPrice(0, data, userName, connection, transaction);
			}

			// - 
			return 0;
		}
		internal static int updateStatisticsSalesPrice(int idx, List<Entities.Tables.PRS.PreisgruppenStatisticsEntity> data, string userName, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(data == null || data.Count <= 0)
				return 0;

			// - 
			//using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				// sqlConnection.Open();
				var connectionId = $"{sqlConnection.ClientConnectionId.ToString().Replace('-', '_')}_{idx}";

				string insertValues = "";
				for(int i = 0; i < data.Count; i++)
				{
					insertValues += $"(@Artikelnummer_{i}, @prix1_{i}, @bis1_{i}, @prix2_{i}, @bis2_{i}, @prix3_{i}, @bis3_{i}, @prix4_{i}, @bis4_{i}),";
				}
				insertValues = insertValues.TrimEnd(',');
				// -
				var swapQuery = $"INSERT INTO #PrixModifier_{connectionId}(artikelnummer,prix1,bis1,prix2,bis2,prix3,bis3,prix4,bis4) VALUES {insertValues};";

				// - 15 | 19
				string query = $@"/* BUTTON 15 */
                                    /* Q1 */
                                    IF OBJECT_ID('tempdb..#PrixModifier_{connectionId}') IS NOT NULL DROP TABLE #PrixModifier_{connectionId};

                                    CREATE TABLE #PrixModifier_{connectionId} (artikelnummer varchar(50),prix1 decimal(20,7),bis1 decimal(20,7),prix2 decimal(20,7),bis2 decimal(20,7),prix3 decimal(20,7),bis3 decimal(20,7),prix4 decimal(20,7),bis4 decimal(20,7));
                                    {swapQuery}

                                    /* Q2 -- Weird [Artikel Trouver Update VK T] NOT EXISTS */
                                    /* SELECT Pr.artikelnummer, B.Verkaufspreis AS [Alte Preis], Pr.prix1 AS [Neue Preis] INTO [Artikel Trouver Update VK T]
                                        FROM #PrixModifier_{connectionId} AS Pr INNER JOIN (Artikel AS A INNER JOIN Preisgruppen AS B ON A.[Artikel-Nr] = B.[Artikel-Nr]) 
	                                        ON Pr.artikelnummer = A.Artikelnummer;
                                        */

                                    /* Q3 */
                                    INSERT INTO tbl_Historie_VK_Update ( Datum, Artikelnummer, [Alte Preis], [Neue Preis], [User] )
                                        SELECT GetDate() Datum, Pr.artikelnummer, Preisgruppen.Verkaufspreis [Alte Preis], prix1 [Neue Preis], '{userName}' [User]
                                        FROM #PrixModifier_{connectionId} Pr INNER JOIN (Artikel INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                        ON Pr.artikelnummer = Artikel.Artikelnummer;

                                    /* Q4 */
                                    UPDATE Preisgruppen 
	                                    SET Verkaufspreis = IIF(Pr.[prix1] IS NOT NULL, Pr.[prix1], Verkaufspreis), 

										Staffelpreis1 = IIf(Pr.[prix1] IS NOT NULL AND (Pr.[bis1] IS NOT NULL OR ME1 IS NOT NULL), Pr.[prix1], Staffelpreis1), 
										Staffelpreis2 = IIF(Pr.[prix2] IS NOT NULL AND (Pr.[bis1] IS NOT NULL OR ME1 IS NOT NULL), Pr.[prix2], Staffelpreis2), 
										Staffelpreis3 = IIF(Pr.[prix3] IS NOT NULL AND (Pr.[bis2] IS NOT NULL OR ME2 IS NOT NULL), Pr.[prix3], Staffelpreis3),
										Staffelpreis4 = IIF(Pr.[prix4] IS NOT NULL AND (Pr.[bis3] IS NOT NULL OR ME3 IS NOT NULL), Pr.[prix4], Staffelpreis4),

										PM1 = IIF(Pr.[prix1] IS NOT NULL AND (Pr.[bis1] IS NOT NULL OR ME1 IS NOT NULL), 100-(Pr.[prix1]/Pr.[prix1]*100), PM1), 
										PM2 = IIF(Pr.[prix2] IS NOT NULL AND (Pr.[bis1] IS NOT NULL OR ME1 IS NOT NULL) AND Pr.[prix1] IS NOT NULL, 100-(Pr.[prix2]/Pr.[prix1]*100), PM2), 
										PM3 = IIF(Pr.[prix3] IS NOT NULL AND (Pr.[bis2] IS NOT NULL OR ME2 IS NOT NULL) AND Pr.[prix1] IS NOT NULL, 100-(Pr.[prix3]/Pr.[prix1]*100), PM3), 
										PM4 = IIF(Pr.[prix4] IS NOT NULL AND (Pr.[bis3] IS NOT NULL OR ME3 IS NOT NULL) AND Pr.[prix1] IS NOT NULL, 100-(Pr.[prix4]/Pr.[prix1]*100), PM4), 

										ME1 = IIF(Pr.[bis1] IS NOT NULL AND (Pr.[prix1] IS NOT NULL OR Staffelpreis1 IS NOT NULL), Pr.[bis1], ME1), 
										ME2 = IIF(Pr.[prix2] IS NOT NULL AND (Staffelpreis2 IS NOT NULL), IIf(Pr.[bis2] Is Null,999999999,Pr.[bis2]), ME2), 
										ME3 = IIF(Pr.[prix3] IS NOT NULL AND (Staffelpreis3 IS NOT NULL), IIf([bis3] Is Null,999999999,Pr.[bis3]), ME3), 
										ME4 = IIF(Pr.[prix4] IS NOT NULL AND (Staffelpreis4 IS NOT NULL), IIf(([bis4] Is Null Or [bis4]=0),999999999,Pr.[bis4]), ME4)

	                                    FROM #PrixModifier_{connectionId} AS Pr 
	                                    INNER JOIN (Artikel AS A INNER JOIN Preisgruppen AS B ON A.[Artikel-Nr] = B.[Artikel-Nr]) 
	                                    ON Pr.artikelnummer = A.Artikelnummer;

								/* 2023-01-18 - Update Extension */
								UPDATE E SET E.Verkaufspreis = B.Verkaufspreis 
										FROM #PrixModifier_{connectionId} AS Pr 
										INNER JOIN (Artikel AS A INNER JOIN Preisgruppen AS B ON A.[Artikel-Nr] = B.[Artikel-Nr]) ON Pr.artikelnummer = A.Artikelnummer
	                                    INNER JOIN [__BSD_ArtikelSalesExtension] AS E ON E.[ArticleNr] = A.[Artikel-Nr] WHERE E.ArticleSalesType='Serie';
	                                    ";

				// - START
				//SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
				using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					for(int i = 0; i < data.Count; i++)
					{
						sqlCommand.Parameters.AddWithValue($"Artikelnummer_{i}", data[i].Artikelnummer == null ? (object)DBNull.Value : data[i].Artikelnummer.Trim());
						sqlCommand.Parameters.AddWithValue($"prix1_{i}", data[i].Preis1 == null ? (object)DBNull.Value : data[i].Preis1);
						sqlCommand.Parameters.AddWithValue($"bis1_{i}", data[i].Bis1 == null ? (object)DBNull.Value : data[i].Bis1);
						sqlCommand.Parameters.AddWithValue($"prix2_{i}", data[i].Preis2 == null ? (object)DBNull.Value : data[i].Preis2);
						sqlCommand.Parameters.AddWithValue($"bis2_{i}", data[i].Bis2 == null ? (object)DBNull.Value : data[i].Bis2);
						sqlCommand.Parameters.AddWithValue($"prix3_{i}", data[i].Preis3 == null ? (object)DBNull.Value : data[i].Preis3);
						sqlCommand.Parameters.AddWithValue($"bis3_{i}", data[i].Bis3 == null ? (object)DBNull.Value : data[i].Bis3);
						sqlCommand.Parameters.AddWithValue($"prix4_{i}", data[i].Preis4 == null ? (object)DBNull.Value : data[i].Preis4);
						sqlCommand.Parameters.AddWithValue($"bis4_{i}", data[i].Bis4 == null ? (object)DBNull.Value : data[i].Bis4);
					}
					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				// - END
				//sqlTransaction.Commit();
			}

			// - 
			return 0;
		}
		public static int UpdateME2(IEnumerable<KeyValuePair<int, decimal>> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count() > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += $"UPDATE [Preisgruppen] SET [ME2]=@ME{i} WHERE [Nr]=@Nr{i}; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Key);
					sqlCommand.Parameters.AddWithValue("ME" + i, item.Value == null ? (object)DBNull.Value : item.Value);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return -1;
		}
		public static int UpdateME3(IEnumerable<KeyValuePair<int, decimal>> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count() > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += $"UPDATE [Preisgruppen] SET [ME3]=@ME{i} WHERE [Nr]=@Nr{i}; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Key);
					sqlCommand.Parameters.AddWithValue("ME" + i, item.Value == null ? (object)DBNull.Value : item.Value);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return -1;
		}
		public static int UpdateME4(IEnumerable<KeyValuePair<int, decimal>> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count() > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += $"UPDATE [Preisgruppen] SET [ME4]=@ME{i} WHERE [Nr]=@Nr{i}; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Key);
					sqlCommand.Parameters.AddWithValue("ME" + i, item.Value == null ? (object)DBNull.Value : item.Value);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return -1;
		}
		#endregion
	}
}
