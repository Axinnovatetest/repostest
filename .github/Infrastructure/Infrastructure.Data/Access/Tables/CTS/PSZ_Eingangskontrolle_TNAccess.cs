using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class PSZ_Eingangskontrolle_TNAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity Get(int nummer_verpackung)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", nummer_verpackung);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_TN]", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung] IN ({string.Join(",", queryIds)})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();

		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Eingangskontrolle_TN] ([Aktiv],[Akzeptierte_Menge],[Anzahl_Verpackungen],[Artikelnummer],[Bestellung-Nr],[Clock Number],[Datum],[Eingangslieferscheinnr],[Geprüfte_Prüfmenge],[Gesamtmenge],[Inspektor],[Kunde],[LagerortID],[Laufende Nummer],[Menge],[MHDDatum],[Nach_Lager],[Prüfentscheid],[Prüfmenge],[Prüftiefe],[Recieve],[Reklamierte Menge],[Restmenge_Rolle_PPS],[Resultat],[Status_Rolle],[Verpackungsnr],[WE_Anzahl_VOH],[WE_Datum_VOH],[WE_LS_VOH]) OUTPUT INSERTED.[Nummer Verpackung] VALUES (@Aktiv,@Akzeptierte_Menge,@Anzahl_Verpackungen,@Artikelnummer,@Bestellung_Nr,@Clock_Number,@Datum,@Eingangslieferscheinnr,@Geprufte_Prufmenge,@Gesamtmenge,@Inspektor,@Kunde,@LagerortID,@Laufende_Nummer,@Menge,@MHDDatum,@Nach_Lager,@Prufentscheid,@Prufmenge,@Pruftiefe,@Recieve,@Reklamierte_Menge,@Restmenge_Rolle_PPS,@Resultat,@Status_Rolle,@Verpackungsnr,@WE_Anzahl_VOH,@WE_Datum_VOH,@WE_LS_VOH); ";

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
					sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
					sqlCommand.Parameters.AddWithValue("Gesamtmenge", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
					sqlCommand.Parameters.AddWithValue("Inspektor", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
					sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
					sqlCommand.Parameters.AddWithValue("LagerortID", item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
					sqlCommand.Parameters.AddWithValue("Laufende_Nummer", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
					sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("MHDDatum", item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
					sqlCommand.Parameters.AddWithValue("Nach_Lager", item.Nach_Lager == null ? (object)DBNull.Value : item.Nach_Lager);
					sqlCommand.Parameters.AddWithValue("Prufentscheid", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
					sqlCommand.Parameters.AddWithValue("Prufmenge", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
					sqlCommand.Parameters.AddWithValue("Pruftiefe", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
					sqlCommand.Parameters.AddWithValue("Recieve", item.Recieve == null ? (object)DBNull.Value : item.Recieve);
					sqlCommand.Parameters.AddWithValue("Reklamierte_Menge", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
					sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS", item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
					sqlCommand.Parameters.AddWithValue("Resultat", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
					sqlCommand.Parameters.AddWithValue("Status_Rolle", item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
					sqlCommand.Parameters.AddWithValue("Verpackungsnr", item.Verpackungsnr == null ? (object)DBNull.Value : item.Verpackungsnr);
					sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
					sqlCommand.Parameters.AddWithValue("WE_Datum_VOH", item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
					sqlCommand.Parameters.AddWithValue("WE_LS_VOH", item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 31; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> items)
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
						query += " INSERT INTO [PSZ_Eingangskontrolle_TN] ([Aktiv],[Akzeptierte_Menge],[Anzahl_Verpackungen],[Artikelnummer],[Bestellung-Nr],[Clock Number],[Datum],[Eingangslieferscheinnr],[Geprüfte_Prüfmenge],[Gesamtmenge],[Inspektor],[Kunde],[LagerortID],[Laufende Nummer],[Menge],[MHDDatum],[Nach_Lager],[Prüfentscheid],[Prüfmenge],[Prüftiefe],[Recieve],[Reklamierte Menge],[Restmenge_Rolle_PPS],[Resultat],[Status_Rolle],[Verpackungsnr],[WE_Anzahl_VOH],[WE_Datum_VOH],[WE_LS_VOH]) VALUES ( "

							+ "@Aktiv" + i + ","
							+ "@Akzeptierte_Menge" + i + ","
							+ "@Anzahl_Verpackungen" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bestellung_Nr" + i + ","
							+ "@Clock_Number" + i + ","
							+ "@Datum" + i + ","
							+ "@Eingangslieferscheinnr" + i + ","
							+ "@Geprufte_Prufmenge" + i + ","
							+ "@Gesamtmenge" + i + ","
							+ "@Inspektor" + i + ","
							+ "@Kunde" + i + ","
							+ "@LagerortID" + i + ","
							+ "@Laufende_Nummer" + i + ","
							+ "@Menge" + i + ","
							+ "@MHDDatum" + i + ","
							+ "@Nach_Lager" + i + ","
							+ "@Prufentscheid" + i + ","
							+ "@Prufmenge" + i + ","
							+ "@Pruftiefe" + i + ","
							+ "@Recieve" + i + ","
							+ "@Reklamierte_Menge" + i + ","
							+ "@Restmenge_Rolle_PPS" + i + ","
							+ "@Resultat" + i + ","
							+ "@Status_Rolle" + i + ","
							+ "@Verpackungsnr" + i + ","
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
						sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge" + i, item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
						sqlCommand.Parameters.AddWithValue("Gesamtmenge" + i, item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
						sqlCommand.Parameters.AddWithValue("Inspektor" + i, item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("LagerortID" + i, item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
						sqlCommand.Parameters.AddWithValue("Laufende_Nummer" + i, item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("MHDDatum" + i, item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
						sqlCommand.Parameters.AddWithValue("Nach_Lager" + i, item.Nach_Lager == null ? (object)DBNull.Value : item.Nach_Lager);
						sqlCommand.Parameters.AddWithValue("Prufentscheid" + i, item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
						sqlCommand.Parameters.AddWithValue("Prufmenge" + i, item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
						sqlCommand.Parameters.AddWithValue("Pruftiefe" + i, item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
						sqlCommand.Parameters.AddWithValue("Recieve" + i, item.Recieve == null ? (object)DBNull.Value : item.Recieve);
						sqlCommand.Parameters.AddWithValue("Reklamierte_Menge" + i, item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
						sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS" + i, item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
						sqlCommand.Parameters.AddWithValue("Resultat" + i, item.Resultat == null ? (object)DBNull.Value : item.Resultat);
						sqlCommand.Parameters.AddWithValue("Status_Rolle" + i, item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
						sqlCommand.Parameters.AddWithValue("Verpackungsnr" + i, item.Verpackungsnr == null ? (object)DBNull.Value : item.Verpackungsnr);
						sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH" + i, item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
						sqlCommand.Parameters.AddWithValue("WE_Datum_VOH" + i, item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
						sqlCommand.Parameters.AddWithValue("WE_LS_VOH" + i, item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Eingangskontrolle_TN] SET [Aktiv]=@Aktiv, [Akzeptierte_Menge]=@Akzeptierte_Menge, [Anzahl_Verpackungen]=@Anzahl_Verpackungen, [Artikelnummer]=@Artikelnummer, [Bestellung-Nr]=@Bestellung_Nr, [Clock Number]=@Clock_Number, [Datum]=@Datum, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Geprüfte_Prüfmenge]=@Geprufte_Prufmenge, [Gesamtmenge]=@Gesamtmenge, [Inspektor]=@Inspektor, [Kunde]=@Kunde, [LagerortID]=@LagerortID, [Laufende Nummer]=@Laufende_Nummer, [Menge]=@Menge, [MHDDatum]=@MHDDatum, [Nach_Lager]=@Nach_Lager, [Prüfentscheid]=@Prufentscheid, [Prüfmenge]=@Prufmenge, [Prüftiefe]=@Pruftiefe, [Recieve]=@Recieve, [Reklamierte Menge]=@Reklamierte_Menge, [Restmenge_Rolle_PPS]=@Restmenge_Rolle_PPS, [Resultat]=@Resultat, [Status_Rolle]=@Status_Rolle, [Verpackungsnr]=@Verpackungsnr, [WE_Anzahl_VOH]=@WE_Anzahl_VOH, [WE_Datum_VOH]=@WE_Datum_VOH, [WE_LS_VOH]=@WE_LS_VOH WHERE [Nummer Verpackung]=@Nummer_Verpackung";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nummer_Verpackung", item.Nummer_Verpackung);
				sqlCommand.Parameters.AddWithValue("Aktiv", item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
				sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge", item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
				sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen", item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Clock_Number", item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
				sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
				sqlCommand.Parameters.AddWithValue("Gesamtmenge", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
				sqlCommand.Parameters.AddWithValue("Inspektor", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
				sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("LagerortID", item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
				sqlCommand.Parameters.AddWithValue("Laufende_Nummer", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
				sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("MHDDatum", item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
				sqlCommand.Parameters.AddWithValue("Nach_Lager", item.Nach_Lager == null ? (object)DBNull.Value : item.Nach_Lager);
				sqlCommand.Parameters.AddWithValue("Prufentscheid", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
				sqlCommand.Parameters.AddWithValue("Prufmenge", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
				sqlCommand.Parameters.AddWithValue("Pruftiefe", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
				sqlCommand.Parameters.AddWithValue("Recieve", item.Recieve == null ? (object)DBNull.Value : item.Recieve);
				sqlCommand.Parameters.AddWithValue("Reklamierte_Menge", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
				sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS", item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
				sqlCommand.Parameters.AddWithValue("Resultat", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
				sqlCommand.Parameters.AddWithValue("Status_Rolle", item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
				sqlCommand.Parameters.AddWithValue("Verpackungsnr", item.Verpackungsnr == null ? (object)DBNull.Value : item.Verpackungsnr);
				sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
				sqlCommand.Parameters.AddWithValue("WE_Datum_VOH", item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
				sqlCommand.Parameters.AddWithValue("WE_LS_VOH", item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 31; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> items)
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
						query += " UPDATE [PSZ_Eingangskontrolle_TN] SET "

							+ "[Aktiv]=@Aktiv" + i + ","
							+ "[Akzeptierte_Menge]=@Akzeptierte_Menge" + i + ","
							+ "[Anzahl_Verpackungen]=@Anzahl_Verpackungen" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
							+ "[Clock Number]=@Clock_Number" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
							+ "[Geprüfte_Prüfmenge]=@Geprufte_Prufmenge" + i + ","
							+ "[Gesamtmenge]=@Gesamtmenge" + i + ","
							+ "[Inspektor]=@Inspektor" + i + ","
							+ "[Kunde]=@Kunde" + i + ","
							+ "[LagerortID]=@LagerortID" + i + ","
							+ "[Laufende Nummer]=@Laufende_Nummer" + i + ","
							+ "[Menge]=@Menge" + i + ","
							+ "[MHDDatum]=@MHDDatum" + i + ","
							+ "[Nach_Lager]=@Nach_Lager" + i + ","
							+ "[Prüfentscheid]=@Prufentscheid" + i + ","
							+ "[Prüfmenge]=@Prufmenge" + i + ","
							+ "[Prüftiefe]=@Pruftiefe" + i + ","
							+ "[Recieve]=@Recieve" + i + ","
							+ "[Reklamierte Menge]=@Reklamierte_Menge" + i + ","
							+ "[Restmenge_Rolle_PPS]=@Restmenge_Rolle_PPS" + i + ","
							+ "[Resultat]=@Resultat" + i + ","
							+ "[Status_Rolle]=@Status_Rolle" + i + ","
							+ "[Verpackungsnr]=@Verpackungsnr" + i + ","
							+ "[WE_Anzahl_VOH]=@WE_Anzahl_VOH" + i + ","
							+ "[WE_Datum_VOH]=@WE_Datum_VOH" + i + ","
							+ "[WE_LS_VOH]=@WE_LS_VOH" + i + " WHERE [Nummer Verpackung]=@Nummer_Verpackung" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nummer_Verpackung" + i, item.Nummer_Verpackung);
						sqlCommand.Parameters.AddWithValue("Aktiv" + i, item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
						sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge" + i, item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
						sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen" + i, item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Clock_Number" + i, item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge" + i, item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
						sqlCommand.Parameters.AddWithValue("Gesamtmenge" + i, item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
						sqlCommand.Parameters.AddWithValue("Inspektor" + i, item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("LagerortID" + i, item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
						sqlCommand.Parameters.AddWithValue("Laufende_Nummer" + i, item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("MHDDatum" + i, item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
						sqlCommand.Parameters.AddWithValue("Nach_Lager" + i, item.Nach_Lager == null ? (object)DBNull.Value : item.Nach_Lager);
						sqlCommand.Parameters.AddWithValue("Prufentscheid" + i, item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
						sqlCommand.Parameters.AddWithValue("Prufmenge" + i, item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
						sqlCommand.Parameters.AddWithValue("Pruftiefe" + i, item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
						sqlCommand.Parameters.AddWithValue("Recieve" + i, item.Recieve == null ? (object)DBNull.Value : item.Recieve);
						sqlCommand.Parameters.AddWithValue("Reklamierte_Menge" + i, item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
						sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS" + i, item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
						sqlCommand.Parameters.AddWithValue("Resultat" + i, item.Resultat == null ? (object)DBNull.Value : item.Resultat);
						sqlCommand.Parameters.AddWithValue("Status_Rolle" + i, item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
						sqlCommand.Parameters.AddWithValue("Verpackungsnr" + i, item.Verpackungsnr == null ? (object)DBNull.Value : item.Verpackungsnr);
						sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH" + i, item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
						sqlCommand.Parameters.AddWithValue("WE_Datum_VOH" + i, item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
						sqlCommand.Parameters.AddWithValue("WE_LS_VOH" + i, item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int nummer_verpackung)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("DELETE FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung]=@Nummer_Verpackung", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Nummer_Verpackung", nummer_verpackung);

				return sqlCommand.ExecuteNonQuery();
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

					string query = $"DELETE FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity GetWithTransaction(int nummer_verpackung, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nummer_verpackung);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Eingangskontrolle_TN]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [PSZ_Eingangskontrolle_TN] ([Aktiv],[Akzeptierte_Menge],[Anzahl_Verpackungen],[Artikelnummer],[Bestellung-Nr],[Clock Number],[Datum],[Eingangslieferscheinnr],[Geprüfte_Prüfmenge],[Gesamtmenge],[Inspektor],[Kunde],[LagerortID],[Laufende Nummer],[Menge],[MHDDatum],[Nach_Lager],[Prüfentscheid],[Prüfmenge],[Prüftiefe],[Recieve],[Reklamierte Menge],[Restmenge_Rolle_PPS],[Resultat],[Status_Rolle],[Verpackungsnr],[WE_Anzahl_VOH],[WE_Datum_VOH],[WE_LS_VOH]) OUTPUT INSERTED.[Nummer Verpackung] VALUES (@Aktiv,@Akzeptierte_Menge,@Anzahl_Verpackungen,@Artikelnummer,@Bestellung_Nr,@Clock_Number,@Datum,@Eingangslieferscheinnr,@Geprufte_Prufmenge,@Gesamtmenge,@Inspektor,@Kunde,@LagerortID,@Laufende_Nummer,@Menge,@MHDDatum,@Nach_Lager,@Prufentscheid,@Prufmenge,@Pruftiefe,@Recieve,@Reklamierte_Menge,@Restmenge_Rolle_PPS,@Resultat,@Status_Rolle,@Verpackungsnr,@WE_Anzahl_VOH,@WE_Datum_VOH,@WE_LS_VOH); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Aktiv", item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
			sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge", item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
			sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen", item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Clock_Number", item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
			sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
			sqlCommand.Parameters.AddWithValue("Gesamtmenge", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
			sqlCommand.Parameters.AddWithValue("Inspektor", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
			sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
			sqlCommand.Parameters.AddWithValue("LagerortID", item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
			sqlCommand.Parameters.AddWithValue("Laufende_Nummer", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
			sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
			sqlCommand.Parameters.AddWithValue("MHDDatum", item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
			sqlCommand.Parameters.AddWithValue("Nach_Lager", item.Nach_Lager == null ? (object)DBNull.Value : item.Nach_Lager);
			sqlCommand.Parameters.AddWithValue("Prufentscheid", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
			sqlCommand.Parameters.AddWithValue("Prufmenge", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
			sqlCommand.Parameters.AddWithValue("Pruftiefe", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
			sqlCommand.Parameters.AddWithValue("Recieve", item.Recieve == null ? (object)DBNull.Value : item.Recieve);
			sqlCommand.Parameters.AddWithValue("Reklamierte_Menge", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
			sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS", item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
			sqlCommand.Parameters.AddWithValue("Resultat", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
			sqlCommand.Parameters.AddWithValue("Status_Rolle", item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
			sqlCommand.Parameters.AddWithValue("Verpackungsnr", item.Verpackungsnr == null ? (object)DBNull.Value : item.Verpackungsnr);
			sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
			sqlCommand.Parameters.AddWithValue("WE_Datum_VOH", item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
			sqlCommand.Parameters.AddWithValue("WE_LS_VOH", item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 31; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [PSZ_Eingangskontrolle_TN] ([Aktiv],[Akzeptierte_Menge],[Anzahl_Verpackungen],[Artikelnummer],[Bestellung-Nr],[Clock Number],[Datum],[Eingangslieferscheinnr],[Geprüfte_Prüfmenge],[Gesamtmenge],[Inspektor],[Kunde],[LagerortID],[Laufende Nummer],[Menge],[MHDDatum],[Nach_Lager],[Prüfentscheid],[Prüfmenge],[Prüftiefe],[Recieve],[Reklamierte Menge],[Restmenge_Rolle_PPS],[Resultat],[Status_Rolle],[Verpackungsnr],[WE_Anzahl_VOH],[WE_Datum_VOH],[WE_LS_VOH]) VALUES ( "

						+ "@Aktiv" + i + ","
						+ "@Akzeptierte_Menge" + i + ","
						+ "@Anzahl_Verpackungen" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Bestellung_Nr" + i + ","
						+ "@Clock_Number" + i + ","
						+ "@Datum" + i + ","
						+ "@Eingangslieferscheinnr" + i + ","
						+ "@Geprufte_Prufmenge" + i + ","
						+ "@Gesamtmenge" + i + ","
						+ "@Inspektor" + i + ","
						+ "@Kunde" + i + ","
						+ "@LagerortID" + i + ","
						+ "@Laufende_Nummer" + i + ","
						+ "@Menge" + i + ","
						+ "@MHDDatum" + i + ","
						+ "@Nach_Lager" + i + ","
						+ "@Prufentscheid" + i + ","
						+ "@Prufmenge" + i + ","
						+ "@Pruftiefe" + i + ","
						+ "@Recieve" + i + ","
						+ "@Reklamierte_Menge" + i + ","
						+ "@Restmenge_Rolle_PPS" + i + ","
						+ "@Resultat" + i + ","
						+ "@Status_Rolle" + i + ","
						+ "@Verpackungsnr" + i + ","
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
					sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge" + i, item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
					sqlCommand.Parameters.AddWithValue("Gesamtmenge" + i, item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
					sqlCommand.Parameters.AddWithValue("Inspektor" + i, item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
					sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
					sqlCommand.Parameters.AddWithValue("LagerortID" + i, item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
					sqlCommand.Parameters.AddWithValue("Laufende_Nummer" + i, item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
					sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("MHDDatum" + i, item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
					sqlCommand.Parameters.AddWithValue("Nach_Lager" + i, item.Nach_Lager == null ? (object)DBNull.Value : item.Nach_Lager);
					sqlCommand.Parameters.AddWithValue("Prufentscheid" + i, item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
					sqlCommand.Parameters.AddWithValue("Prufmenge" + i, item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
					sqlCommand.Parameters.AddWithValue("Pruftiefe" + i, item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
					sqlCommand.Parameters.AddWithValue("Recieve" + i, item.Recieve == null ? (object)DBNull.Value : item.Recieve);
					sqlCommand.Parameters.AddWithValue("Reklamierte_Menge" + i, item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
					sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS" + i, item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
					sqlCommand.Parameters.AddWithValue("Resultat" + i, item.Resultat == null ? (object)DBNull.Value : item.Resultat);
					sqlCommand.Parameters.AddWithValue("Status_Rolle" + i, item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
					sqlCommand.Parameters.AddWithValue("Verpackungsnr" + i, item.Verpackungsnr == null ? (object)DBNull.Value : item.Verpackungsnr);
					sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH" + i, item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
					sqlCommand.Parameters.AddWithValue("WE_Datum_VOH" + i, item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
					sqlCommand.Parameters.AddWithValue("WE_LS_VOH" + i, item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [PSZ_Eingangskontrolle_TN] SET [Aktiv]=@Aktiv, [Akzeptierte_Menge]=@Akzeptierte_Menge, [Anzahl_Verpackungen]=@Anzahl_Verpackungen, [Artikelnummer]=@Artikelnummer, [Bestellung-Nr]=@Bestellung_Nr, [Clock Number]=@Clock_Number, [Datum]=@Datum, [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Geprüfte_Prüfmenge]=@Geprufte_Prufmenge, [Gesamtmenge]=@Gesamtmenge, [Inspektor]=@Inspektor, [Kunde]=@Kunde, [LagerortID]=@LagerortID, [Laufende Nummer]=@Laufende_Nummer, [Menge]=@Menge, [MHDDatum]=@MHDDatum, [Nach_Lager]=@Nach_Lager, [Prüfentscheid]=@Prufentscheid, [Prüfmenge]=@Prufmenge, [Prüftiefe]=@Pruftiefe, [Recieve]=@Recieve, [Reklamierte Menge]=@Reklamierte_Menge, [Restmenge_Rolle_PPS]=@Restmenge_Rolle_PPS, [Resultat]=@Resultat, [Status_Rolle]=@Status_Rolle, [Verpackungsnr]=@Verpackungsnr, [WE_Anzahl_VOH]=@WE_Anzahl_VOH, [WE_Datum_VOH]=@WE_Datum_VOH, [WE_LS_VOH]=@WE_LS_VOH WHERE [Nummer Verpackung]=@Nummer_Verpackung";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nummer_Verpackung", item.Nummer_Verpackung);
			sqlCommand.Parameters.AddWithValue("Aktiv", item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
			sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge", item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
			sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen", item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Clock_Number", item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
			sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
			sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
			sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge", item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
			sqlCommand.Parameters.AddWithValue("Gesamtmenge", item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
			sqlCommand.Parameters.AddWithValue("Inspektor", item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
			sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
			sqlCommand.Parameters.AddWithValue("LagerortID", item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
			sqlCommand.Parameters.AddWithValue("Laufende_Nummer", item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
			sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
			sqlCommand.Parameters.AddWithValue("MHDDatum", item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
			sqlCommand.Parameters.AddWithValue("Nach_Lager", item.Nach_Lager == null ? (object)DBNull.Value : item.Nach_Lager);
			sqlCommand.Parameters.AddWithValue("Prufentscheid", item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
			sqlCommand.Parameters.AddWithValue("Prufmenge", item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
			sqlCommand.Parameters.AddWithValue("Pruftiefe", item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
			sqlCommand.Parameters.AddWithValue("Recieve", item.Recieve == null ? (object)DBNull.Value : item.Recieve);
			sqlCommand.Parameters.AddWithValue("Reklamierte_Menge", item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
			sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS", item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
			sqlCommand.Parameters.AddWithValue("Resultat", item.Resultat == null ? (object)DBNull.Value : item.Resultat);
			sqlCommand.Parameters.AddWithValue("Status_Rolle", item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
			sqlCommand.Parameters.AddWithValue("Verpackungsnr", item.Verpackungsnr == null ? (object)DBNull.Value : item.Verpackungsnr);
			sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH", item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
			sqlCommand.Parameters.AddWithValue("WE_Datum_VOH", item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
			sqlCommand.Parameters.AddWithValue("WE_LS_VOH", item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 31; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PSZ_Eingangskontrolle_TN] SET "

					+ "[Aktiv]=@Aktiv" + i + ","
					+ "[Akzeptierte_Menge]=@Akzeptierte_Menge" + i + ","
					+ "[Anzahl_Verpackungen]=@Anzahl_Verpackungen" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
					+ "[Clock Number]=@Clock_Number" + i + ","
					+ "[Datum]=@Datum" + i + ","
					+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
					+ "[Geprüfte_Prüfmenge]=@Geprufte_Prufmenge" + i + ","
					+ "[Gesamtmenge]=@Gesamtmenge" + i + ","
					+ "[Inspektor]=@Inspektor" + i + ","
					+ "[Kunde]=@Kunde" + i + ","
					+ "[LagerortID]=@LagerortID" + i + ","
					+ "[Laufende Nummer]=@Laufende_Nummer" + i + ","
					+ "[Menge]=@Menge" + i + ","
					+ "[MHDDatum]=@MHDDatum" + i + ","
					+ "[Nach_Lager]=@Nach_Lager" + i + ","
					+ "[Prüfentscheid]=@Prufentscheid" + i + ","
					+ "[Prüfmenge]=@Prufmenge" + i + ","
					+ "[Prüftiefe]=@Pruftiefe" + i + ","
					+ "[Recieve]=@Recieve" + i + ","
					+ "[Reklamierte Menge]=@Reklamierte_Menge" + i + ","
					+ "[Restmenge_Rolle_PPS]=@Restmenge_Rolle_PPS" + i + ","
					+ "[Resultat]=@Resultat" + i + ","
					+ "[Status_Rolle]=@Status_Rolle" + i + ","
					+ "[Verpackungsnr]=@Verpackungsnr" + i + ","
					+ "[WE_Anzahl_VOH]=@WE_Anzahl_VOH" + i + ","
					+ "[WE_Datum_VOH]=@WE_Datum_VOH" + i + ","
					+ "[WE_LS_VOH]=@WE_LS_VOH" + i + " WHERE [Nummer Verpackung]=@Nummer_Verpackung" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nummer_Verpackung" + i, item.Nummer_Verpackung);
					sqlCommand.Parameters.AddWithValue("Aktiv" + i, item.Aktiv == null ? (object)DBNull.Value : item.Aktiv);
					sqlCommand.Parameters.AddWithValue("Akzeptierte_Menge" + i, item.Akzeptierte_Menge == null ? (object)DBNull.Value : item.Akzeptierte_Menge);
					sqlCommand.Parameters.AddWithValue("Anzahl_Verpackungen" + i, item.Anzahl_Verpackungen == null ? (object)DBNull.Value : item.Anzahl_Verpackungen);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Clock_Number" + i, item.Clock_Number == null ? (object)DBNull.Value : item.Clock_Number);
					sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Geprufte_Prufmenge" + i, item.Geprufte_Prufmenge == null ? (object)DBNull.Value : item.Geprufte_Prufmenge);
					sqlCommand.Parameters.AddWithValue("Gesamtmenge" + i, item.Gesamtmenge == null ? (object)DBNull.Value : item.Gesamtmenge);
					sqlCommand.Parameters.AddWithValue("Inspektor" + i, item.Inspektor == null ? (object)DBNull.Value : item.Inspektor);
					sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
					sqlCommand.Parameters.AddWithValue("LagerortID" + i, item.LagerortID == null ? (object)DBNull.Value : item.LagerortID);
					sqlCommand.Parameters.AddWithValue("Laufende_Nummer" + i, item.Laufende_Nummer == null ? (object)DBNull.Value : item.Laufende_Nummer);
					sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("MHDDatum" + i, item.MHDDatum == null ? (object)DBNull.Value : item.MHDDatum);
					sqlCommand.Parameters.AddWithValue("Nach_Lager" + i, item.Nach_Lager == null ? (object)DBNull.Value : item.Nach_Lager);
					sqlCommand.Parameters.AddWithValue("Prufentscheid" + i, item.Prufentscheid == null ? (object)DBNull.Value : item.Prufentscheid);
					sqlCommand.Parameters.AddWithValue("Prufmenge" + i, item.Prufmenge == null ? (object)DBNull.Value : item.Prufmenge);
					sqlCommand.Parameters.AddWithValue("Pruftiefe" + i, item.Pruftiefe == null ? (object)DBNull.Value : item.Pruftiefe);
					sqlCommand.Parameters.AddWithValue("Recieve" + i, item.Recieve == null ? (object)DBNull.Value : item.Recieve);
					sqlCommand.Parameters.AddWithValue("Reklamierte_Menge" + i, item.Reklamierte_Menge == null ? (object)DBNull.Value : item.Reklamierte_Menge);
					sqlCommand.Parameters.AddWithValue("Restmenge_Rolle_PPS" + i, item.Restmenge_Rolle_PPS == null ? (object)DBNull.Value : item.Restmenge_Rolle_PPS);
					sqlCommand.Parameters.AddWithValue("Resultat" + i, item.Resultat == null ? (object)DBNull.Value : item.Resultat);
					sqlCommand.Parameters.AddWithValue("Status_Rolle" + i, item.Status_Rolle == null ? (object)DBNull.Value : item.Status_Rolle);
					sqlCommand.Parameters.AddWithValue("Verpackungsnr" + i, item.Verpackungsnr == null ? (object)DBNull.Value : item.Verpackungsnr);
					sqlCommand.Parameters.AddWithValue("WE_Anzahl_VOH" + i, item.WE_Anzahl_VOH == null ? (object)DBNull.Value : item.WE_Anzahl_VOH);
					sqlCommand.Parameters.AddWithValue("WE_Datum_VOH" + i, item.WE_Datum_VOH == null ? (object)DBNull.Value : item.WE_Datum_VOH);
					sqlCommand.Parameters.AddWithValue("WE_LS_VOH" + i, item.WE_LS_VOH == null ? (object)DBNull.Value : item.WE_LS_VOH);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nummer_verpackung, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung]=@Nummer_Verpackung";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nummer_Verpackung", nummer_verpackung);

			results = sqlCommand.ExecuteNonQuery();


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

				string query = "DELETE FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity GetForInspection(int nummer_verpackung)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_TN] WHERE [LagerortID]=@Id " +
				"AND ISNULL([Menge],0) != 0 and ISNULL([Restmenge_Rolle_PPS],0) != 0" +
				"AND ISNULL([Status_Rolle],0)!= 0 and ISNULL([Gesamtmenge],0)!= 0 ", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", nummer_verpackung);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int CountPlantBookingsRowsByLagerId(int lagerId, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT Count(*) FROM [PSZ_Eingangskontrolle_TN] 
									WHERE [LagerortID]={lagerId} 
									and ISNULL([Menge],0) != 0 
									and ISNULL([Gesamtmenge],0)!= 0 ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}


		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_TNMinimalEntity> GetByLagerId(int lagerId,string filterSearch ,Settings.SortingModel dataSorting,Settings.PaginModel dataPaging)
		{

			string paging = "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"  SELECT Aktiv,
									[Nummer Verpackung] as Verpackungsnr ,
									Artikelnummer ,
									Restmenge_Rolle_PPS,
									Menge,
									Gesamtmenge,
									Status_Rolle,
                                    Inspektor,Resultat,datum
									FROM [PSZ_Eingangskontrolle_TN]
									WHERE [LagerortID]={lagerId} ";

				if(filterSearch != null)
				{
					query += $" AND ([Nummer Verpackung] LIKE '{filterSearch}%' OR  [Gesamtmenge] LIKE '{filterSearch}%'  OR [Artikelnummer] LIKE '{filterSearch}%')";
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

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
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

		public static int ResetMHD(List<int> mhdIds, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $"UPDATE [PSZ_Eingangskontrolle_TN] SET [Aktiv]=0 WHERE [Nummer Verpackung] IN ({string.Join(",", mhdIds)})";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				return sqlCommand.ExecuteNonQuery();
			}
		}

		public static Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity GetBynummer_verpackung(int nummer_verpackung)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [PSZ_Eingangskontrolle_TN] WHERE [Nummer Verpackung]=@Id ", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", nummer_verpackung);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity(dataTable.Rows[0],Entities.Joins.Logistics.LagerAccessEnum.TN);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods
	}
}
