using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Fertigungs_Planung_Sicht_Gewerk_2_KHTNAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity Get(DateTime ack_date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigungs Planung Sicht Gewerk 2 KHTN] WHERE [Ack Date]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", ack_date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigungs Planung Sicht Gewerk 2 KHTN]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity> Get(List<DateTime> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity> get(List<DateTime> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Fertigungs Planung Sicht Gewerk 2 KHTN] WHERE [Ack Date] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity>();
		}

		public static DateTime Insert(Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity item)
		{
			DateTime response = DateTime.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Fertigungs Planung Sicht Gewerk 2 KHTN] ([Ack Date],[Anzahl],[Artikelnummer],[FA_begonnen],[Fertigungsnummer],[Freigabestatus],[Gewerk 1],[Gewerk 2],[Gewerk 3],[Halle],[ID],[Kabel_geschnitten],[Kennzeichen],[Klassifizierung],[KW Aktuel],[KW_Produktion],[Lagerort_id],[Order Time],[PSZ Artikelnummer])  VALUES (@Ack_Date,@Anzahl,@Artikelnummer,@FA_begonnen,@Fertigungsnummer,@Freigabestatus,@Gewerk_1,@Gewerk_2,@Gewerk_3,@Halle,@ID,@Kabel_geschnitten,@Kennzeichen,@Klassifizierung,@KW_Aktuel,@KW_Produktion,@Lagerort_id,@Order_Time,@PSZ_Artikelnummer); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Ack_Date", item.Ack_Date);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("FA_begonnen", item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Gewerk_1", item.Gewerk_1);
					sqlCommand.Parameters.AddWithValue("Gewerk_2", item.Gewerk_2);
					sqlCommand.Parameters.AddWithValue("Gewerk_3", item.Gewerk_3);
					sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
					sqlCommand.Parameters.AddWithValue("ID", item.ID);
					sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
					sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
					sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("KW_Aktuel", item.KW_Aktuel == null ? (object)DBNull.Value : item.KW_Aktuel);
					sqlCommand.Parameters.AddWithValue("KW_Produktion", item.KW_Produktion == null ? (object)DBNull.Value : item.KW_Produktion);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Order_Time", item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
					sqlCommand.Parameters.AddWithValue("PSZ_Artikelnummer", item.PSZ_Artikelnummer == null ? (object)DBNull.Value : item.PSZ_Artikelnummer);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? DateTime.MinValue : DateTime.TryParse(result.ToString(), out var insertedId) ? insertedId : DateTime.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity> items)
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
						query += " INSERT INTO [Fertigungs Planung Sicht Gewerk 2 KHTN] ([Ack Date],[Anzahl],[Artikelnummer],[FA_begonnen],[Fertigungsnummer],[Freigabestatus],[Gewerk 1],[Gewerk 2],[Gewerk 3],[Halle],[ID],[Kabel_geschnitten],[Kennzeichen],[Klassifizierung],[KW Aktuel],[KW_Produktion],[Lagerort_id],[Order Time],[PSZ Artikelnummer]) VALUES ( "

							+ "@Ack Date" + i + ","
							+ "@Anzahl" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@FA_begonnen" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@Freigabestatus" + i + ","
							+ "@Gewerk_1" + i + ","
							+ "@Gewerk_2" + i + ","
							+ "@Gewerk_3" + i + ","
							+ "@Halle" + i + ","
							+ "@ID" + i + ","
							+ "@Kabel_geschnitten" + i + ","
							+ "@Kennzeichen" + i + ","
							+ "@Klassifizierung" + i + ","
							+ "@KW_Aktuel" + i + ","
							+ "@KW_Produktion" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Order_Time" + i + ","
							+ "@PSZ_Artikelnummer" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Ack_Date" + i, item.Ack_Date);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("FA_begonnen" + i, item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Gewerk_1" + i, item.Gewerk_1);
						sqlCommand.Parameters.AddWithValue("Gewerk_2" + i, item.Gewerk_2);
						sqlCommand.Parameters.AddWithValue("Gewerk_3" + i, item.Gewerk_3);
						sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
						sqlCommand.Parameters.AddWithValue("Kennzeichen" + i, item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
						sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
						sqlCommand.Parameters.AddWithValue("KW_Aktuel" + i, item.KW_Aktuel == null ? (object)DBNull.Value : item.KW_Aktuel);
						sqlCommand.Parameters.AddWithValue("KW_Produktion" + i, item.KW_Produktion == null ? (object)DBNull.Value : item.KW_Produktion);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Order_Time" + i, item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
						sqlCommand.Parameters.AddWithValue("PSZ_Artikelnummer" + i, item.PSZ_Artikelnummer == null ? (object)DBNull.Value : item.PSZ_Artikelnummer);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Fertigungs Planung Sicht Gewerk 2 KHTN] SET [Anzahl]=@Anzahl, [Artikelnummer]=@Artikelnummer, [FA_begonnen]=@FA_begonnen, [Fertigungsnummer]=@Fertigungsnummer, [Freigabestatus]=@Freigabestatus, [Gewerk 1]=@Gewerk_1, [Gewerk 2]=@Gewerk_2, [Gewerk 3]=@Gewerk_3, [Halle]=@Halle, [ID]=@ID, [Kabel_geschnitten]=@Kabel_geschnitten, [Kennzeichen]=@Kennzeichen, [Klassifizierung]=@Klassifizierung, [KW Aktuel]=@KW_Aktuel, [KW_Produktion]=@KW_Produktion, [Lagerort_id]=@Lagerort_id, [Order Time]=@Order_Time, [PSZ Artikelnummer]=@PSZ_Artikelnummer WHERE [Ack Date]=@Ack_Date";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Ack_Date", item.Ack_Date);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("FA_begonnen", item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
				sqlCommand.Parameters.AddWithValue("Gewerk_1", item.Gewerk_1);
				sqlCommand.Parameters.AddWithValue("Gewerk_2", item.Gewerk_2);
				sqlCommand.Parameters.AddWithValue("Gewerk_3", item.Gewerk_3);
				sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Kabel_geschnitten", item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
				sqlCommand.Parameters.AddWithValue("Kennzeichen", item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
				sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
				sqlCommand.Parameters.AddWithValue("KW_Aktuel", item.KW_Aktuel == null ? (object)DBNull.Value : item.KW_Aktuel);
				sqlCommand.Parameters.AddWithValue("KW_Produktion", item.KW_Produktion == null ? (object)DBNull.Value : item.KW_Produktion);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Order_Time", item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
				sqlCommand.Parameters.AddWithValue("PSZ_Artikelnummer", item.PSZ_Artikelnummer == null ? (object)DBNull.Value : item.PSZ_Artikelnummer);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNEntity> items)
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
						query += " UPDATE [Fertigungs Planung Sicht Gewerk 2 KHTN] SET "

							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[FA_begonnen]=@FA_begonnen" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[Freigabestatus]=@Freigabestatus" + i + ","
							+ "[Gewerk 1]=@Gewerk_1" + i + ","
							+ "[Gewerk 2]=@Gewerk_2" + i + ","
							+ "[Gewerk 3]=@Gewerk_3" + i + ","
							+ "[Halle]=@Halle" + i + ","
							+ "[ID]=@ID" + i + ","
							+ "[Kabel_geschnitten]=@Kabel_geschnitten" + i + ","
							+ "[Kennzeichen]=@Kennzeichen" + i + ","
							+ "[Klassifizierung]=@Klassifizierung" + i + ","
							+ "[KW Aktuel]=@KW_Aktuel" + i + ","
							+ "[KW_Produktion]=@KW_Produktion" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Order Time]=@Order_Time" + i + ","
							+ "[PSZ Artikelnummer]=@PSZ_Artikelnummer" + i + " WHERE [Ack Date]=@Ack_Date" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Ack_Date" + i, item.Ack_Date);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("FA_begonnen" + i, item.FA_begonnen == null ? (object)DBNull.Value : item.FA_begonnen);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Gewerk_1" + i, item.Gewerk_1);
						sqlCommand.Parameters.AddWithValue("Gewerk_2" + i, item.Gewerk_2);
						sqlCommand.Parameters.AddWithValue("Gewerk_3" + i, item.Gewerk_3);
						sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Kabel_geschnitten" + i, item.Kabel_geschnitten == null ? (object)DBNull.Value : item.Kabel_geschnitten);
						sqlCommand.Parameters.AddWithValue("Kennzeichen" + i, item.Kennzeichen == null ? (object)DBNull.Value : item.Kennzeichen);
						sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
						sqlCommand.Parameters.AddWithValue("KW_Aktuel" + i, item.KW_Aktuel == null ? (object)DBNull.Value : item.KW_Aktuel);
						sqlCommand.Parameters.AddWithValue("KW_Produktion" + i, item.KW_Produktion == null ? (object)DBNull.Value : item.KW_Produktion);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Order_Time" + i, item.Order_Time == null ? (object)DBNull.Value : item.Order_Time);
						sqlCommand.Parameters.AddWithValue("PSZ_Artikelnummer" + i, item.PSZ_Artikelnummer == null ? (object)DBNull.Value : item.PSZ_Artikelnummer);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(DateTime ack_date)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Fertigungs Planung Sicht Gewerk 2 KHTN] WHERE [Ack Date]=@Ack_Date";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Ack_Date", ack_date);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [Fertigungs Planung Sicht Gewerk 2 KHTN] WHERE [Ack Date] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_TNEntity> Analysegewerk2KHTN(string ack_date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigungs Planung Sicht Gewerk 2 KHTN] WHERE [Ack Date]<=@ack_date";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ack_date", ack_date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_TNEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_TNEntity>();
			}
		}

		#endregion
	}
}
