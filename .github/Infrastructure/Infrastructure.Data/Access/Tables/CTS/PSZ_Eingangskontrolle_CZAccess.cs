using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class PSZ_Eingangskontrolle_CZAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity Get(int verpackungsnr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", verpackungsnr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_CZ]", sqlConnection))
			{
				sqlConnection.Open();
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr] IN ({string.Join(",", queryIds)})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();

		}

		/*public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> GetByPage(Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
        {  
            var dataTable = new DataTable();
            var query = "SELECT * FROM [PSZ_Eingangskontrolle_CZ]";
            if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
			{
				query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
			} else
			{
				query += " ORDER BY [Verpackungsnr] DESC ";
			}
			if(paging != null)
			{
				query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
			}     
            using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
            usning(var sqlCommand = new SqlCommand(query, sqlConnection))            
            {
                sqlConnection.Open();
                DbExecution.Fill(sqlCommand, dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
            }
        } */

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Eingangskontrolle_CZ] ([Aktiv],[Akzeptierte_Menge],[Anzahl_Verpackungen],[Artikelnummer],[Bestellung-Nr],[Clock Number],[Datum],[Eingangslieferscheinnr],[Gedruckt],[Geprüfte_Prüfmenge],[Gesamtmenge],[Inspektor],[Kunde],[LagerortID],[Laufende Nummer],[Menge],[MHDDatum],[Prüfentscheid],[Prüfmenge],[Prüftiefe],[Reklamierte Menge],[Restmenge_Rolle_PPS],[Resultat],[Status_Rolle],[WE_Anzahl_VOH],[WE_Datum_VOH],[WE_LS_VOH]) OUTPUT INSERTED.[Verpackungsnr] VALUES (@Aktiv,@Akzeptierte_Menge,@Anzahl_Verpackungen,@Artikelnummer,@Bestellung_Nr,@Clock_Number,@Datum,@Eingangslieferscheinnr,@Gedruckt,@Geprufte_Prufmenge,@Gesamtmenge,@Inspektor,@Kunde,@LagerortID,@Laufende_Nummer,@Menge,@MHDDatum,@Prufentscheid,@Prufmenge,@Pruftiefe,@Reklamierte_Menge,@Restmenge_Rolle_PPS,@Resultat,@Status_Rolle,@WE_Anzahl_VOH,@WE_Datum_VOH,@WE_LS_VOH); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Aktiv", item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
					sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge", item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
					sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen", item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Clock_Number", item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
					sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
					sqlCommand.Parameters.AddWithValue("Gesamtmenge", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
					sqlCommand.Parameters.AddWithValue("Inspektor", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
					sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
					sqlCommand.Parameters.AddWithValue("LagerortID", item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
					sqlCommand.Parameters.AddWithValue("Laufende_Nummer", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
					sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("MHDDatum", item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
					sqlCommand.Parameters.AddWithValue("Prufentscheid", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
					sqlCommand.Parameters.AddWithValue("Prufmenge", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
					sqlCommand.Parameters.AddWithValue("Pruftiefe", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
					sqlCommand.Parameters.AddWithValue("Reklamierte_Menge", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
					sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS", item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
					sqlCommand.Parameters.AddWithValue("Resultat", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
					sqlCommand.Parameters.AddWithValue("Status_Rolle", item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
					sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
					sqlCommand.Parameters.AddWithValue("WE_Datum_VOH", item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
					sqlCommand.Parameters.AddWithValue("WE_LS_VOH", item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; /* Nb params per query */
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> items)
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
						query += " INSERT INTO [PSZ_Eingangskontrolle_CZ] ([Aktiv],[Akzeptierte_Menge],[Anzahl_Verpackungen],[Artikelnummer],[Bestellung-Nr],[Clock Number],[Datum],[Eingangslieferscheinnr],[Gedruckt],[Geprüfte_Prüfmenge],[Gesamtmenge],[Inspektor],[Kunde],[LagerortID],[Laufende Nummer],[Menge],[MHDDatum],[Prüfentscheid],[Prüfmenge],[Prüftiefe],[Reklamierte Menge],[Restmenge_Rolle_PPS],[Resultat],[Status_Rolle],[WE_Anzahl_VOH],[WE_Datum_VOH],[WE_LS_VOH]) VALUES ("

							+ "@Aktiv" + i + ","
							+ "@Akzeptierte_Menge" + i + ","
							+ "@Anzahl_Verpackungen" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bestellung_Nr" + i + ","
							+ "@Clock_Number" + i + ","
							+ "@Datum" + i + ","
							+ "@Eingangslieferscheinnr" + i + ","
							+ "@Gedruckt" + i + ","
							+ "@Geprufte_Prufmenge" + i + ","
							+ "@Gesamtmenge" + i + ","
							+ "@Inspektor" + i + ","
							+ "@Kunde" + i + ","
							+ "@LagerortID" + i + ","
							+ "@Laufende_Nummer" + i + ","
							+ "@Menge" + i + ","
							+ "@MHDDatum" + i + ","
							+ "@Prufentscheid" + i + ","
							+ "@Prufmenge" + i + ","
							+ "@Pruftiefe" + i + ","
							+ "@Reklamierte_Menge" + i + ","
							+ "@Restmenge_Rolle_PPS" + i + ","
							+ "@Resultat" + i + ","
							+ "@Status_Rolle" + i + ","
							+ "@WE_Anzahl_VOH" + i + ","
							+ "@WE_Datum_VOH" + i + ","
							+ "@WE_LS_VOH" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Aktiv" + i, item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
						sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge" + i, item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
						sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen" + i, item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Clock_Number" + i, item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
						sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge" + i, item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
						sqlCommand.Parameters.AddWithValue("Gesamtmenge" + i, item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
						sqlCommand.Parameters.AddWithValue("Inspektor" + i, item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("LagerortID" + i, item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
						sqlCommand.Parameters.AddWithValue("Laufende_Nummer" + i, item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("MHDDatum" + i, item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
						sqlCommand.Parameters.AddWithValue("Prufentscheid" + i, item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
						sqlCommand.Parameters.AddWithValue("Prufmenge" + i, item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
						sqlCommand.Parameters.AddWithValue("Pruftiefe" + i, item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
						sqlCommand.Parameters.AddWithValue("Reklamierte_Menge" + i, item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
						sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS" + i, item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
						sqlCommand.Parameters.AddWithValue("Resultat" + i, item.Resultat == null ? (object)DBNull.Value : item.Resultat);
						sqlCommand.Parameters.AddWithValue("Status_Rolle" + i, item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
						sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH" + i, item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
						sqlCommand.Parameters.AddWithValue("WE_Datum_VOH" + i, item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
						sqlCommand.Parameters.AddWithValue("WE_LS_VOH" + i, item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);
					}

					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("", sqlConnection))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Eingangskontrolle_CZ] SET [Aktiv]=@Aktiv, [Akzeptierte_Menge]=@Akzeptierte_Menge, [Anzahl_Verpackungen]=@Anzahl_Verpackungen, [Artikelnummer]=@Artikelnummer, [Bestellung-Nr]=@Bestellung_Nr, [Clock Number]=@Clock_Number, [Datum]=@Datum, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Gedruckt]=@Gedruckt, [Geprüfte_Prüfmenge]=@Geprufte_Prufmenge, [Gesamtmenge]=@Gesamtmenge, [Inspektor]=@Inspektor, [Kunde]=@Kunde, [LagerortID]=@LagerortID, [Laufende Nummer]=@Laufende_Nummer, [Menge]=@Menge, [MHDDatum]=@MHDDatum, [Prüfentscheid]=@Prufentscheid, [Prüfmenge]=@Prufmenge, [Prüftiefe]=@Pruftiefe, [Reklamierte Menge]=@Reklamierte_Menge, [Restmenge_Rolle_PPS]=@Restmenge_Rolle_PPS, [Resultat]=@Resultat, [Status_Rolle]=@Status_Rolle, [WE_Anzahl_VOH]=@WE_Anzahl_VOH, [WE_Datum_VOH]=@WE_Datum_VOH, [WE_LS_VOH]=@WE_LS_VOH WHERE [Verpackungsnr]=@Verpackungsnr";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("Verpackungsnr", item.Verpackungsnr);
				sqlCommand.Parameters.AddWithValue("Aktiv", item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
				sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge", item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
				sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen", item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Clock_Number", item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
				sqlCommand.Parameters.AddWithValue("Gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
				sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
				sqlCommand.Parameters.AddWithValue("Gesamtmenge", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
				sqlCommand.Parameters.AddWithValue("Inspektor", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
				sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("LagerortID", item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
				sqlCommand.Parameters.AddWithValue("Laufende_Nummer", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
				sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("MHDDatum", item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
				sqlCommand.Parameters.AddWithValue("Prufentscheid", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
				sqlCommand.Parameters.AddWithValue("Prufmenge", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
				sqlCommand.Parameters.AddWithValue("Pruftiefe", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
				sqlCommand.Parameters.AddWithValue("Reklamierte_Menge", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
				sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS", item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
				sqlCommand.Parameters.AddWithValue("Resultat", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
				sqlCommand.Parameters.AddWithValue("Status_Rolle", item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
				sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
				sqlCommand.Parameters.AddWithValue("WE_Datum_VOH", item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
				sqlCommand.Parameters.AddWithValue("WE_LS_VOH", item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; /* Nb params per query */
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> items)
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
						query += "UPDATE [PSZ_Eingangskontrolle_CZ] SET "

							+ "[Aktiv]=@Aktiv" + i + ","
							+ "[Akzeptierte_Menge]=@Akzeptierte_Menge" + i + ","
							+ "[Anzahl_Verpackungen]=@Anzahl_Verpackungen" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
							+ "[Clock Number]=@Clock_Number" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
							+ "[Gedruckt]=@Gedruckt" + i + ","
							+ "[Geprüfte_Prüfmenge]=@Geprufte_Prufmenge" + i + ","
							+ "[Gesamtmenge]=@Gesamtmenge" + i + ","
							+ "[Inspektor]=@Inspektor" + i + ","
							+ "[Kunde]=@Kunde" + i + ","
							+ "[LagerortID]=@LagerortID" + i + ","
							+ "[Laufende Nummer]=@Laufende_Nummer" + i + ","
							+ "[Menge]=@Menge" + i + ","
							+ "[MHDDatum]=@MHDDatum" + i + ","
							+ "[Prüfentscheid]=@Prufentscheid" + i + ","
							+ "[Prüfmenge]=@Prufmenge" + i + ","
							+ "[Prüftiefe]=@Pruftiefe" + i + ","
							+ "[Reklamierte Menge]=@Reklamierte_Menge" + i + ","
							+ "[Restmenge_Rolle_PPS]=@Restmenge_Rolle_PPS" + i + ","
							+ "[Resultat]=@Resultat" + i + ","
							+ "[Status_Rolle]=@Status_Rolle" + i + ","
							+ "[WE_Anzahl_VOH]=@WE_Anzahl_VOH" + i + ","
							+ "[WE_Datum_VOH]=@WE_Datum_VOH" + i + ","
							+ "[WE_LS_VOH]=@WE_LS_VOH" + i + $" WHERE [Verpackungsnr]=@Verpackungsnr{i}"
							+ "; ";

						sqlCommand.Parameters.AddWithValue($"Verpackungsnr{i}", item.Verpackungsnr);

						sqlCommand.Parameters.AddWithValue("Aktiv" + i, item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
						sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge" + i, item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
						sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen" + i, item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Clock_Number" + i, item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
						sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge" + i, item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
						sqlCommand.Parameters.AddWithValue("Gesamtmenge" + i, item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
						sqlCommand.Parameters.AddWithValue("Inspektor" + i, item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("LagerortID" + i, item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
						sqlCommand.Parameters.AddWithValue("Laufende_Nummer" + i, item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("MHDDatum" + i, item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
						sqlCommand.Parameters.AddWithValue("Prufentscheid" + i, item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
						sqlCommand.Parameters.AddWithValue("Prufmenge" + i, item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
						sqlCommand.Parameters.AddWithValue("Pruftiefe" + i, item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
						sqlCommand.Parameters.AddWithValue("Reklamierte_Menge" + i, item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
						sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS" + i, item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
						sqlCommand.Parameters.AddWithValue("Resultat" + i, item.Resultat == null ? (object)DBNull.Value : item.Resultat);
						sqlCommand.Parameters.AddWithValue("Status_Rolle" + i, item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
						sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH" + i, item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
						sqlCommand.Parameters.AddWithValue("WE_Datum_VOH" + i, item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
						sqlCommand.Parameters.AddWithValue("WE_LS_VOH" + i, item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);
					}

					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int verpackungsnr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("DELETE FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr]=@Verpackungsnr", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Verpackungsnr", verpackungsnr);

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

					string query = $"DELETE FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity GetWithTransaction(int verpackungsnr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr] = @Id", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", verpackungsnr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_CZ]", connection, transaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr] IN ({string.Join(",", queryIds)})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO PSZ_Eingangskontrolle_CZ ([Aktiv],[Akzeptierte_Menge],[Anzahl_Verpackungen],[Artikelnummer],[Bestellung-Nr],[Clock Number],[Datum],[Eingangslieferscheinnr],[Gedruckt],[Geprüfte_Prüfmenge],[Gesamtmenge],[Inspektor],[Kunde],[LagerortID],[Laufende Nummer],[Menge],[MHDDatum],[Prüfentscheid],[Prüfmenge],[Prüftiefe],[Reklamierte Menge],[Restmenge_Rolle_PPS],[Resultat],[Status_Rolle],[WE_Anzahl_VOH],[WE_Datum_VOH],[WE_LS_VOH]) OUTPUT INSERTED.[Verpackungsnr] VALUES (@Aktiv,@Akzeptierte_Menge,@Anzahl_Verpackungen,@Artikelnummer,@Bestellung_Nr,@Clock_Number,@Datum,@Eingangslieferscheinnr,@Gedruckt,@Geprufte_Prufmenge,@Gesamtmenge,@Inspektor,@Kunde,@LagerortID,@Laufende_Nummer,@Menge,@MHDDatum,@Prufentscheid,@Prufmenge,@Pruftiefe,@Reklamierte_Menge,@Restmenge_Rolle_PPS,@Resultat,@Status_Rolle,@WE_Anzahl_VOH,@WE_Datum_VOH,@WE_LS_VOH); ";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Aktiv", item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
				sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge", item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
				sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen", item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Clock_Number", item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
				sqlCommand.Parameters.AddWithValue("Gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
				sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
				sqlCommand.Parameters.AddWithValue("Gesamtmenge", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
				sqlCommand.Parameters.AddWithValue("Inspektor", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
				sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("LagerortID", item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
				sqlCommand.Parameters.AddWithValue("Laufende_Nummer", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
				sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("MHDDatum", item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
				sqlCommand.Parameters.AddWithValue("Prufentscheid", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
				sqlCommand.Parameters.AddWithValue("Prufmenge", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
				sqlCommand.Parameters.AddWithValue("Pruftiefe", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
				sqlCommand.Parameters.AddWithValue("Reklamierte_Menge", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
				sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS", item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
				sqlCommand.Parameters.AddWithValue("Resultat", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
				sqlCommand.Parameters.AddWithValue("Status_Rolle", item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
				sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
				sqlCommand.Parameters.AddWithValue("WE_Datum_VOH", item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
				sqlCommand.Parameters.AddWithValue("WE_LS_VOH", item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; /* Nb params per query */
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> items, SqlConnection connection, SqlTransaction transaction)
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
						query += "INSERT INTO [PSZ_Eingangskontrolle_CZ] ([Aktiv],[Akzeptierte_Menge],[Anzahl_Verpackungen],[Artikelnummer],[Bestellung-Nr],[Clock Number],[Datum],[Eingangslieferscheinnr],[Gedruckt],[Geprüfte_Prüfmenge],[Gesamtmenge],[Inspektor],[Kunde],[LagerortID],[Laufende Nummer],[Menge],[MHDDatum],[Prüfentscheid],[Prüfmenge],[Prüftiefe],[Reklamierte Menge],[Restmenge_Rolle_PPS],[Resultat],[Status_Rolle],[WE_Anzahl_VOH],[WE_Datum_VOH],[WE_LS_VOH]) VALUES ( "

						+ "@Aktiv" + i + ","
						+ "@Akzeptierte_Menge" + i + ","
						+ "@Anzahl_Verpackungen" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Bestellung_Nr" + i + ","
						+ "@Clock_Number" + i + ","
						+ "@Datum" + i + ","
						+ "@Eingangslieferscheinnr" + i + ","
						+ "@Gedruckt" + i + ","
						+ "@Geprufte_Prufmenge" + i + ","
						+ "@Gesamtmenge" + i + ","
						+ "@Inspektor" + i + ","
						+ "@Kunde" + i + ","
						+ "@LagerortID" + i + ","
						+ "@Laufende_Nummer" + i + ","
						+ "@Menge" + i + ","
						+ "@MHDDatum" + i + ","
						+ "@Prufentscheid" + i + ","
						+ "@Prufmenge" + i + ","
						+ "@Pruftiefe" + i + ","
						+ "@Reklamierte_Menge" + i + ","
						+ "@Restmenge_Rolle_PPS" + i + ","
						+ "@Resultat" + i + ","
						+ "@Status_Rolle" + i + ","
						+ "@WE_Anzahl_VOH" + i + ","
						+ "@WE_Datum_VOH" + i + ","
						+ "@WE_LS_VOH" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Aktiv" + i, item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
						sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge" + i, item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
						sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen" + i, item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Clock_Number" + i, item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
						sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge" + i, item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
						sqlCommand.Parameters.AddWithValue("Gesamtmenge" + i, item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
						sqlCommand.Parameters.AddWithValue("Inspektor" + i, item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("LagerortID" + i, item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
						sqlCommand.Parameters.AddWithValue("Laufende_Nummer" + i, item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("MHDDatum" + i, item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
						sqlCommand.Parameters.AddWithValue("Prufentscheid" + i, item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
						sqlCommand.Parameters.AddWithValue("Prufmenge" + i, item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
						sqlCommand.Parameters.AddWithValue("Pruftiefe" + i, item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
						sqlCommand.Parameters.AddWithValue("Reklamierte_Menge" + i, item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
						sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS" + i, item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
						sqlCommand.Parameters.AddWithValue("Resultat" + i, item.Resultat == null ? (object)DBNull.Value : item.Resultat);
						sqlCommand.Parameters.AddWithValue("Status_Rolle" + i, item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
						sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH" + i, item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
						sqlCommand.Parameters.AddWithValue("WE_Datum_VOH" + i, item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
						sqlCommand.Parameters.AddWithValue("WE_LS_VOH" + i, item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);
					}

					sqlCommand.CommandText = query;

					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [PSZ_Eingangskontrolle_CZ] SET [Aktiv]=@Aktiv, [Akzeptierte_Menge]=@Akzeptierte_Menge, [Anzahl_Verpackungen]=@Anzahl_Verpackungen, [Artikelnummer]=@Artikelnummer, [Bestellung-Nr]=@Bestellung_Nr, [Clock Number]=@Clock_Number, [Datum]=@Datum, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Gedruckt]=@Gedruckt, [Geprüfte_Prüfmenge]=@Geprufte_Prufmenge, [Gesamtmenge]=@Gesamtmenge, [Inspektor]=@Inspektor, [Kunde]=@Kunde, [LagerortID]=@LagerortID, [Laufende Nummer]=@Laufende_Nummer, [Menge]=@Menge, [MHDDatum]=@MHDDatum, [Prüfentscheid]=@Prufentscheid, [Prüfmenge]=@Prufmenge, [Prüftiefe]=@Pruftiefe, [Reklamierte Menge]=@Reklamierte_Menge, [Restmenge_Rolle_PPS]=@Restmenge_Rolle_PPS, [Resultat]=@Resultat, [Status_Rolle]=@Status_Rolle, [WE_Anzahl_VOH]=@WE_Anzahl_VOH, [WE_Datum_VOH]=@WE_Datum_VOH, [WE_LS_VOH]=@WE_LS_VOH WHERE [Verpackungsnr]=@Verpackungsnr";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Verpackungsnr", item.Verpackungsnr);
				sqlCommand.Parameters.AddWithValue("Aktiv", item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
				sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge", item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
				sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen", item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Clock_Number", item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
				sqlCommand.Parameters.AddWithValue("Gedruckt", item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
				sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
				sqlCommand.Parameters.AddWithValue("Gesamtmenge", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
				sqlCommand.Parameters.AddWithValue("Inspektor", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
				sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("LagerortID", item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
				sqlCommand.Parameters.AddWithValue("Laufende_Nummer", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
				sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("MHDDatum", item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
				sqlCommand.Parameters.AddWithValue("Prufentscheid", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
				sqlCommand.Parameters.AddWithValue("Prufmenge", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
				sqlCommand.Parameters.AddWithValue("Pruftiefe", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
				sqlCommand.Parameters.AddWithValue("Reklamierte_Menge", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
				sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS", item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
				sqlCommand.Parameters.AddWithValue("Resultat", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
				sqlCommand.Parameters.AddWithValue("Status_Rolle", item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
				sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
				sqlCommand.Parameters.AddWithValue("WE_Datum_VOH", item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
				sqlCommand.Parameters.AddWithValue("WE_LS_VOH", item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}

		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 28; /* Nb params per query */
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> items, SqlConnection connection, SqlTransaction transaction)
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
						query += "UPDATE [PSZ_Eingangskontrolle_CZ] SET "

						+ "[Aktiv]=@Aktiv" + i + ","
						+ "[Akzeptierte_Menge]=@Akzeptierte_Menge" + i + ","
						+ "[Anzahl_Verpackungen]=@Anzahl_Verpackungen" + i + ","
						+ "[Artikelnummer]=@Artikelnummer" + i + ","
						+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
						+ "[Clock Number]=@Clock_Number" + i + ","
						+ "[Datum]=@Datum" + i + ","
						+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
						+ "[Gedruckt]=@Gedruckt" + i + ","
						+ "[Geprüfte_Prüfmenge]=@Geprufte_Prufmenge" + i + ","
						+ "[Gesamtmenge]=@Gesamtmenge" + i + ","
						+ "[Inspektor]=@Inspektor" + i + ","
						+ "[Kunde]=@Kunde" + i + ","
						+ "[LagerortID]=@LagerortID" + i + ","
						+ "[Laufende Nummer]=@Laufende_Nummer" + i + ","
						+ "[Menge]=@Menge" + i + ","
						+ "[MHDDatum]=@MHDDatum" + i + ","
						+ "[Prüfentscheid]=@Prufentscheid" + i + ","
						+ "[Prüfmenge]=@Prufmenge" + i + ","
						+ "[Prüftiefe]=@Pruftiefe" + i + ","
						+ "[Reklamierte Menge]=@Reklamierte_Menge" + i + ","
						+ "[Restmenge_Rolle_PPS]=@Restmenge_Rolle_PPS" + i + ","
						+ "[Resultat]=@Resultat" + i + ","
						+ "[Status_Rolle]=@Status_Rolle" + i + ","
						+ "[WE_Anzahl_VOH]=@WE_Anzahl_VOH" + i + ","
						+ "[WE_Datum_VOH]=@WE_Datum_VOH" + i + ","
						+ "[WE_LS_VOH]=@WE_LS_VOH" + i + " WHERE [Verpackungsnr]=@Verpackungsnr" + i
							+ ";";

						sqlCommand.Parameters.AddWithValue("Verpackungsnr" + i, item.Verpackungsnr);

						sqlCommand.Parameters.AddWithValue("Aktiv" + i, item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
						sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge" + i, item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
						sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen" + i, item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Clock_Number" + i, item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Gedruckt" + i, item.Gedruckt == null ? (object)DBNull.Value : item.Gedruckt);
						sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge" + i, item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
						sqlCommand.Parameters.AddWithValue("Gesamtmenge" + i, item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
						sqlCommand.Parameters.AddWithValue("Inspektor" + i, item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("LagerortID" + i, item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
						sqlCommand.Parameters.AddWithValue("Laufende_Nummer" + i, item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("MHDDatum" + i, item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
						sqlCommand.Parameters.AddWithValue("Prufentscheid" + i, item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
						sqlCommand.Parameters.AddWithValue("Prufmenge" + i, item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
						sqlCommand.Parameters.AddWithValue("Pruftiefe" + i, item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
						sqlCommand.Parameters.AddWithValue("Reklamierte_Menge" + i, item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
						sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS" + i, item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
						sqlCommand.Parameters.AddWithValue("Resultat" + i, item.Resultat == null ? (object)DBNull.Value : item.Resultat);
						sqlCommand.Parameters.AddWithValue("Status_Rolle" + i, item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
						sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH" + i, item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
						sqlCommand.Parameters.AddWithValue("WE_Datum_VOH" + i, item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
						sqlCommand.Parameters.AddWithValue("WE_LS_VOH" + i, item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);
					}

					sqlCommand.CommandText = query;
					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}

			return -1;
		}

		public static int DeleteWithTransaction(int verpackungsnr, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr]=@Verpackungsnr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Verpackungsnr", verpackungsnr);
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

					string query = $"DELETE FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int ResetMHD(List<int> mhdIds, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $"UPDATE [PSZ_Eingangskontrolle_CZ] SET [Aktiv]=0 WHERE [Verpackungsnr] IN ({string.Join(",", mhdIds)})";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}


		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity> GetByLagerId(int lagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_CZ] WHERE [LagerortID]=@LagerortID", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("LagerortID", lagerId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(x)).ToList();

			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNMinimalEntity> GetByLagerIdCZ(int lagerId, string filterSearch, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{

			string paging = "";
			string sorting = "";

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"SELECT Aktiv,Verpackungsnr as Verpackungsnr ,
								Artikelnummer ,
								Restmenge_Rolle_PPS,
								Menge,
								Gesamtmenge,
								Status_Rolle,
                                Inspektor,Resultat,datum
						        FROM [PSZ_Eingangskontrolle_CZ]
						        WHERE [LagerortID]={lagerId} ";

				if(filterSearch != null)
				{
					query += $" AND ([Verpackungsnr] LIKE '{filterSearch}%' OR  [Gesamtmenge] LIKE '{filterSearch}%'  OR [Artikelnummer] LIKE '{filterSearch}%')";
				}
				query += " AND ISNULL([Menge],0) != 0 " +
						 "and ISNULL([Gesamtmenge],0)!= 0 ";

				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += "ORDER BY [Artikelnummer]";
				}

				if(paging != null)
				{
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNMinimalEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNMinimalEntity>();
			}
		}

		public static int CountPlantBookingsLagerCZ(int lagerId, string searchValue, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT COUNT(*) FROM[PSZ_Eingangskontrolle_CZ] 
									WHERE [LagerortID]={lagerId} 
									and ISNULL([Menge],0) != 0 
									and ISNULL([Gesamtmenge],0)!= 0 ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static Infrastructure.Data.Entities.Tables.Logistics.PrintedDataPlantBookingEntity GetPrintDataCZ(int nummerVerpackung)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("select Top 10 Aktiv, [Verpackungsnr], Artikelnummer,LagerortID,Menge,Gesamtmenge,Inspektor,Datum,MHDDatum,Resultat, WE_LS_VOH, [Bestellung-Nr] as WE_VOH_Nr from [PSZ_Eingangskontrolle_CZ] where [Verpackungsnr]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", nummerVerpackung);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.PrintedDataPlantBookingEntity(dataTable.Rows[0], Infrastructure.Data.Entities.Tables.Logistics.LagerAccessEnum.Eigenfertigung);
			}
			else
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.PrintedDataPlantBookingEntity();
			}
		}

		public static Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity GetByVerpackungNrCz(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("select * from [PSZ_Eingangskontrolle_CZ]  where  [Verpackungsnr]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0], Infrastructure.Data.Entities.Joins.Logistics.LagerAccessEnum.Eigenfertigung);
			}
			else
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
			}
		}


		public static Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity GetPlantBookingsSuivantPrecedentCZ(int LagerId, int order, int PassedNmrVpckng)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				if(order == 1)//Siuvante
				{
					query = @"SELECT TOP 1
					*
					FROM [PSZ_Eingangskontrolle_CZ]
					WHERE LagerortID = @LagerId 
                    and [Verpackungsnr] > @PassedNmrVpckng order By [Verpackungsnr] asc ";
				}


				else if(order == 2)
				{
					query = @"SELECT TOP 1
					*
					FROM [PSZ_Eingangskontrolle_CZ]
					WHERE LagerortID = @LagerId 
                    and [Verpackungsnr] < @PassedNmrVpckng order By [Verpackungsnr] desc ";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId);
				sqlCommand.Parameters.AddWithValue("PassedNmrVpckng", PassedNmrVpckng);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{

				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0], Infrastructure.Data.Entities.Joins.Logistics.LagerAccessEnum.Eigenfertigung);
			}
			else
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
			}
		}


		public static Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity GetPalntBookingsMinMaxCZ(int LagerID, int order)
		{
			var dataTable = new DataTable();


			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				if(order == 1)//Max
				{
					query = @"SELECT TOP 1
							*
							FROM [PSZ_Eingangskontrolle_CZ]
							WHERE LagerortID = @LagerID
							ORDER BY [Verpackungsnr] DESC";
				}
				else if(order == 2)
				{
					query = @"SELECT TOP 1
							*
							FROM [PSZ_Eingangskontrolle_CZ]
							WHERE LagerortID = @LagerID
							ORDER BY [Verpackungsnr]";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("LagerID", LagerID);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{

				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0], Infrastructure.Data.Entities.Joins.Logistics.LagerAccessEnum.Eigenfertigung);
			}
			else
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
			}
		}
		public static int InsertWithArtikelNummerCz(int LagerId, string? ArtikelNummer, DateTime? MHDDatum, string inspector, string remarks, SqlConnection connection, SqlTransaction transaction)
		{
			string query = @"DECLARE @NewId TABLE([Verpackungsnr] INT);
								INSERT INTO[PSZ_Eingangskontrolle_CZ]
								(
								Aktiv,
								[Datum], 
								[LagerortID], 
								[Bestellung-Nr], 
								WE_Anzahl_VOH, 
								WE_Datum_VOH, 
								Eingangslieferscheinnr, 
								ArtikelNummer, 
								Menge, 
								Gesamtmenge, Anzahl_Verpackungen, 
								[Clock Number], 
								Inspektor, 
								Kunde, 
								Prüfentscheid, Resultat, 
								Prüfmenge, 
								Prüftiefe, 
								Geprüfte_Prüfmenge, 
								WE_LS_VOH, 
								MHDDatum
								)
								OUTPUT 
								INSERTED.[Verpackungsnr] INTO @NewId 
								SELECT 
								0,
								GETDATE(), 
								@LagerId,
								0,
								0,
								'',
								0,
								@ArtikelNummer,
								0,
								0,
								1,
								0,
								@inspector,
								'-',
								0,
								@remarks,
								0,
								1,
								0,
								null,
								@MHDDatum 
								SELECT[Verpackungsnr] AS NewAutoGeneratedWiw FROM @NewId";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ArtikelNummer", ArtikelNummer == null ? (object)DBNull.Value : ArtikelNummer);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId == 0 ? (object)DBNull.Value : LagerId);
				sqlCommand.Parameters.AddWithValue("MHDDatum", MHDDatum == null ? (object)DBNull.Value : MHDDatum);
				sqlCommand.Parameters.AddWithValue("inspector", inspector == null ? "-" : inspector);
				sqlCommand.Parameters.AddWithValue("remarks", remarks == null ? "-" : remarks);
				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransactionBestellteArtikelCz(int LagerId, int? nrBestellteArtikel, string inspector, string remarks, SqlConnection connection, SqlTransaction transaction)
		{
			string query = @"DECLARE @NewId TABLE ([Verpackungsnr] INT);
								INSERT INTO [PSZ_Eingangskontrolle_CZ]   
								(
								Aktiv,
                                [Datum],
								[LagerortID],
								[Bestellung-Nr],
								WE_Anzahl_VOH,
								WE_Datum_VOH,
								Eingangslieferscheinnr,
								Artikelnummer, 
								Menge,
								Gesamtmenge,
								Anzahl_Verpackungen,
								[Clock Number],
								Inspektor,   
								Kunde,   
								Prüfentscheid,
								Resultat,
								Prüfmenge,
								Prüftiefe,
								Geprüfte_Prüfmenge,
								WE_LS_VOH,
								MHDDatum 
								)      
								OUTPUT INSERTED.[Verpackungsnr] INTO @NewId 
                                SELECT 
								0,
								GETDATE(), 
								@LagerId ,
								B.[Bestellung-Nr],BA.Anzahl,B.Datum,@nrBestellteArtikel ,
								A.Artikelnummer,Anzahl / 1,Anzahl,1,0,@inspector,'-'   ,0,@remarks,   /*Prüfmenge*/  
								Anzahl / 1 * (        IIF(Prüftiefe_WE = 1, 1,
								IIF(Prüftiefe_WE = 2, 0.3,         
								IIF(Prüftiefe_WE = 3, 0.05, 0.01)            
								)        
								)    
								), 
								/*Prüftiefe*/ IIF(   Prüftiefe_WE = 1, 1,IIF(Prüftiefe_WE = 2, 0.3,IIF(Prüftiefe_WE = 3, 0.05, 0.01))), 
								0, 
								Eingangslieferscheinnr, 
								MhdDatumArtikel 
								FROM Bestellungen B INNER JOIN [bestellte Artikel] BA ON BA.[Bestellung-Nr] = B.Nr   
								INNER JOIN Artikel A ON A.[Artikel-Nr] = BA.[Artikel-Nr]  
								INNER JOIN Bestellnummern BE ON B.[Lieferanten-Nr] = BE.[Lieferanten-Nr] 
								AND A.[Artikel-Nr] = BE.[Artikel-Nr]   
								WHERE BA.nr = @nrBestellteArtikel
								SELECT [Verpackungsnr] AS NewAutoGeneratedWiw FROM @NewId ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("nrBestellteArtikel", nrBestellteArtikel == null ? (object)DBNull.Value : nrBestellteArtikel);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId == 0 ? (object)DBNull.Value : LagerId);
				sqlCommand.Parameters.AddWithValue("inspector", inspector is null ? "-": inspector);
				sqlCommand.Parameters.AddWithValue("remarks", remarks is null ? "-": remarks);
				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransactionTransferBestellteArtikelCZ(int? AnzahlNach, int LagerId, int? nrBestellteArtikel, int? LagerbewegungenId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = @"DECLARE @NewId TABLE ([Verpackungsnr] INT);
								INSERT INTO [PSZ_Eingangskontrolle_CZ]   
								(
								Aktiv,
			                    [Datum],
								[LagerortID],
								[Bestellung-Nr],
								WE_Anzahl_VOH,
								WE_Datum_VOH,
								Eingangslieferscheinnr,
								Artikelnummer, 
								Menge,
								Gesamtmenge,
								Anzahl_Verpackungen,
								[Clock Number],
								Inspektor,   
								Kunde,   
								Prüfentscheid,
								Resultat,
								Prüfmenge,
								Prüftiefe,
								Geprüfte_Prüfmenge,
								WE_LS_VOH,
								MHDDatum 
								)      
								OUTPUT INSERTED.[Verpackungsnr] INTO @NewId 
			                    SELECT 
								0,
								GETDATE(), 
								@LagerId ,
								 B.[Bestellung-Nr], l.Anzahl_nach ,B.Datum,@nrBestellteArtikel ,
								A.Artikelnummer,  ISNULL(@AnzahlNach,0) - isnull(l.receivedQuantity,0) / 1 ,@AnzahlNach,1,0,'-','-'   ,0,'-',   /*Prüfmenge*/  
								l.Anzahl / 1 * (        IIF(Prüftiefe_WE = 1, 1,
								IIF(Prüftiefe_WE = 2, 0.3,         
								IIF(Prüftiefe_WE = 3, 0.05, 0.01)                    
								)        
								)    
								), 
								/*Prüftiefe*/ IIF(   Prüftiefe_WE = 1, 1,IIF(Prüftiefe_WE = 2, 0.3,IIF(Prüftiefe_WE = 3, 0.05, 0.01))), 
								0, 
								Eingangslieferscheinnr, 
								MhdDatumArtikel 
								FROM Bestellungen B INNER JOIN [bestellte Artikel] BA ON BA.[Bestellung-Nr] = B.Nr   
								INNER JOIN Artikel A ON A.[Artikel-Nr] = BA.[Artikel-Nr]  
								INNER JOIN Bestellnummern BE ON B.[Lieferanten-Nr] = BE.[Lieferanten-Nr] 
								INNER JOIN  Lagerbewegungen_Artikel l on l.WereingangId=BA.Nr
								AND A.[Artikel-Nr] = BE.[Artikel-Nr]   
								WHERE L.id = @LagerbewegungenId
								SELECT [Verpackungsnr] AS NewAutoGeneratedWiw FROM @NewId ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("nrBestellteArtikel", nrBestellteArtikel == null ? (object)DBNull.Value : nrBestellteArtikel);
				sqlCommand.Parameters.AddWithValue("LagerbewegungenId", LagerbewegungenId == null ? (object)DBNull.Value : LagerbewegungenId);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId == 0 ? (object)DBNull.Value : LagerId);
				sqlCommand.Parameters.AddWithValue("AnzahlNach", AnzahlNach == 0 ? (object)DBNull.Value : AnzahlNach);
				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int DeleteWithArtikelCZ(int Id, int LagerId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = " delete from [PSZ_Eingangskontrolle_CZ] where [Verpackungsnr]=@Id AND  [LagerortID] = @LagerId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", Id);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity GetPlantBookingsPrecedentByLagerIDCZ(int LagerId, int PassedNmrVpckng)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				query = @"SELECT TOP 1
					*
					FROM [PSZ_Eingangskontrolle_CZ]
					WHERE LagerortID = @LagerId 
                    and [Verpackungsnr] < @PassedNmrVpckng 
					order By [Verpackungsnr] desc ";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId);
				sqlCommand.Parameters.AddWithValue("PassedNmrVpckng", PassedNmrVpckng);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0], Infrastructure.Data.Entities.Joins.Logistics.LagerAccessEnum.Eigenfertigung);
			}
			else
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
			}
		}


		public static Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity GetPlantBookingsSuivantByLagerIDCZ(int LagerId, int PassedNmrVpckng)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";

				query = @"SELECT TOP 1
					*
					FROM [PSZ_Eingangskontrolle_CZ]
					WHERE LagerortID = @LagerId 
                    and [Verpackungsnr] > @PassedNmrVpckng 
					order By [Verpackungsnr] asc ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("LagerId", LagerId);
				sqlCommand.Parameters.AddWithValue("PassedNmrVpckng", PassedNmrVpckng);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0], Infrastructure.Data.Entities.Joins.Logistics.LagerAccessEnum.Eigenfertigung);
			}
			else
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
			}
		}
		public static Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity GetBynummer_verpackungCz(int nummer_verpackung)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_CZ] WHERE [Verpackungsnr]=@Id ", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", nummer_verpackung);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0], Entities.Joins.Logistics.LagerAccessEnum.Eigenfertigung);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateData(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = @"UPDATE [PSZ_Eingangskontrolle_CZ]  SET  Aktiv = 1, Akzeptierte_Menge = @Akzeptierte_Menge, Artikelnummer = @Artikelnummer,[Clock Number] = @Clock_Number, Geprüfte_Prüfmenge = @Geprufte_Prufmenge,      Gesamtmenge = @Gesamtmenge, Inspektor = @Inspektor, Kunde = @Kunde,   Menge = @Menge,    [Laufende Nummer] = @Laufende_Nummer, Prüfentscheid = @Prufentscheid, Prüftiefe = @Pruftiefe, [Reklamierte Menge] = @Reklamierte_Menge, Resultat = @Resultat, Prüfmenge = @Prufmenge,WE_Anzahl_VOH =@WE_Anzahl_VOH WHERE [Verpackungsnr] = @Verpackungsnr; ";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Verpackungsnr", item.Verpackungsnr);
				sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge ", item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
				sqlCommand.Parameters.AddWithValue("Artikelnummer ", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Clock_Number ", item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
				sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge ", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
				sqlCommand.Parameters.AddWithValue("Gesamtmenge ", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
				sqlCommand.Parameters.AddWithValue("Inspektor ", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
				sqlCommand.Parameters.AddWithValue("Kunde ", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("Menge ", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("Laufende_Nummer ", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
				sqlCommand.Parameters.AddWithValue("Prufentscheid ", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
				sqlCommand.Parameters.AddWithValue("Reklamierte_Menge ", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
				sqlCommand.Parameters.AddWithValue("Resultat ", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
				sqlCommand.Parameters.AddWithValue("Prufmenge ", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
				sqlCommand.Parameters.AddWithValue("Pruftiefe ", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
				sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH ", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion Custom Methods
	}
}

