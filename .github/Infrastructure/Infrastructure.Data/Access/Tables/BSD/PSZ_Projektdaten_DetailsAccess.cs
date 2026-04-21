using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class PSZ_Projektdaten_DetailsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Projektdaten_Details] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Projektdaten_Details]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Projektdaten_Details] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Projektdaten_Details] ([AB_Datum],[Arbeitszeit Serien Pro Kabesatz],[Artikelnummer],[Bemerkungen],[EAU],[EMPB],[Erstanlage],[FA_Datum],[Kontakt_AV_PSZ],[Kontakt_CS_PSZ],[Kontakt_Technik_Kunde],[Kontakt_Technik_PSZ],[Kosten],[Krimp_WKZ],[Material_Eskalation_AV],[Material_Eskalation_Termin],[Material_Komplett],[Menge],[MOQ],[Projekt betreung],[Projekt_Start],[Projektmeldung],[Projekt-Nr],[Rapid Prototyp],[Serie_PSZ],[SG_WKZ],[Standort_Muster],[Standort_Serie],[Summe Arbeitszeit],[Termin mit Technik abgesprochen],[TSP Kunden],[Typ],[UL_Verpackung],[Wunschtermin_Kunde],[Zuschlag])  VALUES (@AB_Datum,@Arbeitszeit_Serien_Pro_Kabesatz,@Artikelnummer,@Bemerkungen,@EAU,@EMPB,@Erstanlage,@FA_Datum,@Kontakt_AV_PSZ,@Kontakt_CS_PSZ,@Kontakt_Technik_Kunde,@Kontakt_Technik_PSZ,@Kosten,@Krimp_WKZ,@Material_Eskalation_AV,@Material_Eskalation_Termin,@Material_Komplett,@Menge,@MOQ,@Projekt_betreung,@Projekt_Start,@Projektmeldung,@Projekt_Nr,@Rapid_Prototyp,@Serie_PSZ,@SG_WKZ,@Standort_Muster,@Standort_Serie,@Summe_Arbeitszeit,@Termin_mit_Technik_abgesprochen,@TSP_Kunden,@Typ,@UL_Verpackung,@Wunschtermin_Kunde,@Zuschlag); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB_Datum", item.AB_Datum == null ? (object)DBNull.Value : item.AB_Datum);
					sqlCommand.Parameters.AddWithValue("Arbeitszeit_Serien_Pro_Kabesatz", item.Arbeitszeit_Serien_Pro_Kabesatz == null ? (object)DBNull.Value : item.Arbeitszeit_Serien_Pro_Kabesatz);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("EAU", item.EAU == null ? (object)DBNull.Value : item.EAU);
					sqlCommand.Parameters.AddWithValue("EMPB", item.EMPB == null ? (object)DBNull.Value : item.EMPB);
					sqlCommand.Parameters.AddWithValue("Erstanlage", item.Erstanlage == null ? (object)DBNull.Value : item.Erstanlage);
					sqlCommand.Parameters.AddWithValue("FA_Datum", item.FA_Datum == null ? (object)DBNull.Value : item.FA_Datum);
					sqlCommand.Parameters.AddWithValue("Kontakt_AV_PSZ", item.Kontakt_AV_PSZ == null ? (object)DBNull.Value : item.Kontakt_AV_PSZ);
					sqlCommand.Parameters.AddWithValue("Kontakt_CS_PSZ", item.Kontakt_CS_PSZ == null ? (object)DBNull.Value : item.Kontakt_CS_PSZ);
					sqlCommand.Parameters.AddWithValue("Kontakt_Technik_Kunde", item.Kontakt_Technik_Kunde == null ? (object)DBNull.Value : item.Kontakt_Technik_Kunde);
					sqlCommand.Parameters.AddWithValue("Kontakt_Technik_PSZ", item.Kontakt_Technik_PSZ == null ? (object)DBNull.Value : item.Kontakt_Technik_PSZ);
					sqlCommand.Parameters.AddWithValue("Kosten", item.Kosten == null ? (object)DBNull.Value : item.Kosten);
					sqlCommand.Parameters.AddWithValue("Krimp_WKZ", item.Krimp_WKZ == null ? (object)DBNull.Value : item.Krimp_WKZ);
					sqlCommand.Parameters.AddWithValue("Material_Eskalation_AV", item.Material_Eskalation_AV == null ? (object)DBNull.Value : item.Material_Eskalation_AV);
					sqlCommand.Parameters.AddWithValue("Material_Eskalation_Termin", item.Material_Eskalation_Termin == null ? (object)DBNull.Value : item.Material_Eskalation_Termin);
					sqlCommand.Parameters.AddWithValue("Material_Komplett", item.Material_Komplett == null ? (object)DBNull.Value : item.Material_Komplett);
					sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
					sqlCommand.Parameters.AddWithValue("MOQ", item.MOQ == null ? (object)DBNull.Value : item.MOQ);
					sqlCommand.Parameters.AddWithValue("Projekt_betreung", item.Projekt_betreung == null ? (object)DBNull.Value : item.Projekt_betreung);
					sqlCommand.Parameters.AddWithValue("Projekt_Start", item.Projekt_Start == null ? (object)DBNull.Value : item.Projekt_Start);
					sqlCommand.Parameters.AddWithValue("Projektmeldung", item.Projektmeldung == null ? (object)DBNull.Value : item.Projektmeldung);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					sqlCommand.Parameters.AddWithValue("Rapid_Prototyp", item.Rapid_Prototyp == null ? (object)DBNull.Value : item.Rapid_Prototyp);
					sqlCommand.Parameters.AddWithValue("Serie_PSZ", item.Serie_PSZ == null ? (object)DBNull.Value : item.Serie_PSZ);
					sqlCommand.Parameters.AddWithValue("SG_WKZ", item.SG_WKZ == null ? (object)DBNull.Value : item.SG_WKZ);
					sqlCommand.Parameters.AddWithValue("Standort_Muster", item.Standort_Muster == null ? (object)DBNull.Value : item.Standort_Muster);
					sqlCommand.Parameters.AddWithValue("Standort_Serie", item.Standort_Serie == null ? (object)DBNull.Value : item.Standort_Serie);
					sqlCommand.Parameters.AddWithValue("Summe_Arbeitszeit", item.Summe_Arbeitszeit == null ? (object)DBNull.Value : item.Summe_Arbeitszeit);
					sqlCommand.Parameters.AddWithValue("Termin_mit_Technik_abgesprochen", item.Termin_mit_Technik_abgesprochen == null ? (object)DBNull.Value : item.Termin_mit_Technik_abgesprochen);
					sqlCommand.Parameters.AddWithValue("TSP_Kunden", item.TSP_Kunden == null ? (object)DBNull.Value : item.TSP_Kunden);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("UL_Verpackung", item.UL_Verpackung == null ? (object)DBNull.Value : item.UL_Verpackung);
					sqlCommand.Parameters.AddWithValue("Wunschtermin_Kunde", item.Wunschtermin_Kunde == null ? (object)DBNull.Value : item.Wunschtermin_Kunde);
					sqlCommand.Parameters.AddWithValue("Zuschlag", item.Zuschlag == null ? (object)DBNull.Value : item.Zuschlag);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 37; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity> items)
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
						query += " INSERT INTO [PSZ_Projektdaten_Details] ([AB_Datum],[Arbeitszeit Serien Pro Kabesatz],[Artikelnummer],[Bemerkungen],[EAU],[EMPB],[Erstanlage],[FA_Datum],[Kontakt_AV_PSZ],[Kontakt_CS_PSZ],[Kontakt_Technik_Kunde],[Kontakt_Technik_PSZ],[Kosten],[Krimp_WKZ],[Material_Eskalation_AV],[Material_Eskalation_Termin],[Material_Komplett],[Menge],[MOQ],[Projekt betreung],[Projekt_Start],[Projektmeldung],[Projekt-Nr],[Rapid Prototyp],[Serie_PSZ],[SG_WKZ],[Standort_Muster],[Standort_Serie],[Summe Arbeitszeit],[Termin mit Technik abgesprochen],[TSP Kunden],[Typ],[UL_Verpackung],[Wunschtermin_Kunde],[Zuschlag]) VALUES ( "

							+ "@AB_Datum" + i + ","
							+ "@Arbeitszeit_Serien_Pro_Kabesatz" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@EAU" + i + ","
							+ "@EMPB" + i + ","
							+ "@Erstanlage" + i + ","
							+ "@FA_Datum" + i + ","
							+ "@Kontakt_AV_PSZ" + i + ","
							+ "@Kontakt_CS_PSZ" + i + ","
							+ "@Kontakt_Technik_Kunde" + i + ","
							+ "@Kontakt_Technik_PSZ" + i + ","
							+ "@Kosten" + i + ","
							+ "@Krimp_WKZ" + i + ","
							+ "@Material_Eskalation_AV" + i + ","
							+ "@Material_Eskalation_Termin" + i + ","
							+ "@Material_Komplett" + i + ","
							+ "@Menge" + i + ","
							+ "@MOQ" + i + ","
							+ "@Projekt_betreung" + i + ","
							+ "@Projekt_Start" + i + ","
							+ "@Projektmeldung" + i + ","
							+ "@Projekt_Nr" + i + ","
							+ "@Rapid_Prototyp" + i + ","
							+ "@Serie_PSZ" + i + ","
							+ "@SG_WKZ" + i + ","
							+ "@Standort_Muster" + i + ","
							+ "@Standort_Serie" + i + ","
							+ "@Summe_Arbeitszeit" + i + ","
							+ "@Termin_mit_Technik_abgesprochen" + i + ","
							+ "@TSP_Kunden" + i + ","
							+ "@Typ" + i + ","
							+ "@UL_Verpackung" + i + ","
							+ "@Wunschtermin_Kunde" + i + ","
							+ "@Zuschlag" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB_Datum" + i, item.AB_Datum == null ? (object)DBNull.Value : item.AB_Datum);
						sqlCommand.Parameters.AddWithValue("Arbeitszeit_Serien_Pro_Kabesatz" + i, item.Arbeitszeit_Serien_Pro_Kabesatz == null ? (object)DBNull.Value : item.Arbeitszeit_Serien_Pro_Kabesatz);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("EAU" + i, item.EAU == null ? (object)DBNull.Value : item.EAU);
						sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
						sqlCommand.Parameters.AddWithValue("Erstanlage" + i, item.Erstanlage == null ? (object)DBNull.Value : item.Erstanlage);
						sqlCommand.Parameters.AddWithValue("FA_Datum" + i, item.FA_Datum == null ? (object)DBNull.Value : item.FA_Datum);
						sqlCommand.Parameters.AddWithValue("Kontakt_AV_PSZ" + i, item.Kontakt_AV_PSZ == null ? (object)DBNull.Value : item.Kontakt_AV_PSZ);
						sqlCommand.Parameters.AddWithValue("Kontakt_CS_PSZ" + i, item.Kontakt_CS_PSZ == null ? (object)DBNull.Value : item.Kontakt_CS_PSZ);
						sqlCommand.Parameters.AddWithValue("Kontakt_Technik_Kunde" + i, item.Kontakt_Technik_Kunde == null ? (object)DBNull.Value : item.Kontakt_Technik_Kunde);
						sqlCommand.Parameters.AddWithValue("Kontakt_Technik_PSZ" + i, item.Kontakt_Technik_PSZ == null ? (object)DBNull.Value : item.Kontakt_Technik_PSZ);
						sqlCommand.Parameters.AddWithValue("Kosten" + i, item.Kosten == null ? (object)DBNull.Value : item.Kosten);
						sqlCommand.Parameters.AddWithValue("Krimp_WKZ" + i, item.Krimp_WKZ == null ? (object)DBNull.Value : item.Krimp_WKZ);
						sqlCommand.Parameters.AddWithValue("Material_Eskalation_AV" + i, item.Material_Eskalation_AV == null ? (object)DBNull.Value : item.Material_Eskalation_AV);
						sqlCommand.Parameters.AddWithValue("Material_Eskalation_Termin" + i, item.Material_Eskalation_Termin == null ? (object)DBNull.Value : item.Material_Eskalation_Termin);
						sqlCommand.Parameters.AddWithValue("Material_Komplett" + i, item.Material_Komplett == null ? (object)DBNull.Value : item.Material_Komplett);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("MOQ" + i, item.MOQ == null ? (object)DBNull.Value : item.MOQ);
						sqlCommand.Parameters.AddWithValue("Projekt_betreung" + i, item.Projekt_betreung == null ? (object)DBNull.Value : item.Projekt_betreung);
						sqlCommand.Parameters.AddWithValue("Projekt_Start" + i, item.Projekt_Start == null ? (object)DBNull.Value : item.Projekt_Start);
						sqlCommand.Parameters.AddWithValue("Projektmeldung" + i, item.Projektmeldung == null ? (object)DBNull.Value : item.Projektmeldung);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rapid_Prototyp" + i, item.Rapid_Prototyp == null ? (object)DBNull.Value : item.Rapid_Prototyp);
						sqlCommand.Parameters.AddWithValue("Serie_PSZ" + i, item.Serie_PSZ == null ? (object)DBNull.Value : item.Serie_PSZ);
						sqlCommand.Parameters.AddWithValue("SG_WKZ" + i, item.SG_WKZ == null ? (object)DBNull.Value : item.SG_WKZ);
						sqlCommand.Parameters.AddWithValue("Standort_Muster" + i, item.Standort_Muster == null ? (object)DBNull.Value : item.Standort_Muster);
						sqlCommand.Parameters.AddWithValue("Standort_Serie" + i, item.Standort_Serie == null ? (object)DBNull.Value : item.Standort_Serie);
						sqlCommand.Parameters.AddWithValue("Summe_Arbeitszeit" + i, item.Summe_Arbeitszeit == null ? (object)DBNull.Value : item.Summe_Arbeitszeit);
						sqlCommand.Parameters.AddWithValue("Termin_mit_Technik_abgesprochen" + i, item.Termin_mit_Technik_abgesprochen == null ? (object)DBNull.Value : item.Termin_mit_Technik_abgesprochen);
						sqlCommand.Parameters.AddWithValue("TSP_Kunden" + i, item.TSP_Kunden == null ? (object)DBNull.Value : item.TSP_Kunden);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("UL_Verpackung" + i, item.UL_Verpackung == null ? (object)DBNull.Value : item.UL_Verpackung);
						sqlCommand.Parameters.AddWithValue("Wunschtermin_Kunde" + i, item.Wunschtermin_Kunde == null ? (object)DBNull.Value : item.Wunschtermin_Kunde);
						sqlCommand.Parameters.AddWithValue("Zuschlag" + i, item.Zuschlag == null ? (object)DBNull.Value : item.Zuschlag);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Projektdaten_Details] SET [AB_Datum]=@AB_Datum, [Arbeitszeit Serien Pro Kabesatz]=@Arbeitszeit_Serien_Pro_Kabesatz, [Artikelnummer]=@Artikelnummer, [Bemerkungen]=@Bemerkungen, [EAU]=@EAU, [EMPB]=@EMPB, [Erstanlage]=@Erstanlage, [FA_Datum]=@FA_Datum, [Kontakt_AV_PSZ]=@Kontakt_AV_PSZ, [Kontakt_CS_PSZ]=@Kontakt_CS_PSZ, [Kontakt_Technik_Kunde]=@Kontakt_Technik_Kunde, [Kontakt_Technik_PSZ]=@Kontakt_Technik_PSZ, [Kosten]=@Kosten, [Krimp_WKZ]=@Krimp_WKZ, [Material_Eskalation_AV]=@Material_Eskalation_AV, [Material_Eskalation_Termin]=@Material_Eskalation_Termin, [Material_Komplett]=@Material_Komplett, [Menge]=@Menge, [MOQ]=@MOQ, [Projekt betreung]=@Projekt_betreung, [Projekt_Start]=@Projekt_Start, [Projektmeldung]=@Projektmeldung, [Projekt-Nr]=@Projekt_Nr, [Rapid Prototyp]=@Rapid_Prototyp, [Serie_PSZ]=@Serie_PSZ, [SG_WKZ]=@SG_WKZ, [Standort_Muster]=@Standort_Muster, [Standort_Serie]=@Standort_Serie, [Summe Arbeitszeit]=@Summe_Arbeitszeit, [Termin mit Technik abgesprochen]=@Termin_mit_Technik_abgesprochen, [TSP Kunden]=@TSP_Kunden, [Typ]=@Typ, [UL_Verpackung]=@UL_Verpackung, [Wunschtermin_Kunde]=@Wunschtermin_Kunde, [Zuschlag]=@Zuschlag WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("AB_Datum", item.AB_Datum == null ? (object)DBNull.Value : item.AB_Datum);
				sqlCommand.Parameters.AddWithValue("Arbeitszeit_Serien_Pro_Kabesatz", item.Arbeitszeit_Serien_Pro_Kabesatz == null ? (object)DBNull.Value : item.Arbeitszeit_Serien_Pro_Kabesatz);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("EAU", item.EAU == null ? (object)DBNull.Value : item.EAU);
				sqlCommand.Parameters.AddWithValue("EMPB", item.EMPB == null ? (object)DBNull.Value : item.EMPB);
				sqlCommand.Parameters.AddWithValue("Erstanlage", item.Erstanlage == null ? (object)DBNull.Value : item.Erstanlage);
				sqlCommand.Parameters.AddWithValue("FA_Datum", item.FA_Datum == null ? (object)DBNull.Value : item.FA_Datum);
				sqlCommand.Parameters.AddWithValue("Kontakt_AV_PSZ", item.Kontakt_AV_PSZ == null ? (object)DBNull.Value : item.Kontakt_AV_PSZ);
				sqlCommand.Parameters.AddWithValue("Kontakt_CS_PSZ", item.Kontakt_CS_PSZ == null ? (object)DBNull.Value : item.Kontakt_CS_PSZ);
				sqlCommand.Parameters.AddWithValue("Kontakt_Technik_Kunde", item.Kontakt_Technik_Kunde == null ? (object)DBNull.Value : item.Kontakt_Technik_Kunde);
				sqlCommand.Parameters.AddWithValue("Kontakt_Technik_PSZ", item.Kontakt_Technik_PSZ == null ? (object)DBNull.Value : item.Kontakt_Technik_PSZ);
				sqlCommand.Parameters.AddWithValue("Kosten", item.Kosten == null ? (object)DBNull.Value : item.Kosten);
				sqlCommand.Parameters.AddWithValue("Krimp_WKZ", item.Krimp_WKZ == null ? (object)DBNull.Value : item.Krimp_WKZ);
				sqlCommand.Parameters.AddWithValue("Material_Eskalation_AV", item.Material_Eskalation_AV == null ? (object)DBNull.Value : item.Material_Eskalation_AV);
				sqlCommand.Parameters.AddWithValue("Material_Eskalation_Termin", item.Material_Eskalation_Termin == null ? (object)DBNull.Value : item.Material_Eskalation_Termin);
				sqlCommand.Parameters.AddWithValue("Material_Komplett", item.Material_Komplett == null ? (object)DBNull.Value : item.Material_Komplett);
				sqlCommand.Parameters.AddWithValue("Menge", item.Menge == null ? (object)DBNull.Value : item.Menge);
				sqlCommand.Parameters.AddWithValue("MOQ", item.MOQ == null ? (object)DBNull.Value : item.MOQ);
				sqlCommand.Parameters.AddWithValue("Projekt_betreung", item.Projekt_betreung == null ? (object)DBNull.Value : item.Projekt_betreung);
				sqlCommand.Parameters.AddWithValue("Projekt_Start", item.Projekt_Start == null ? (object)DBNull.Value : item.Projekt_Start);
				sqlCommand.Parameters.AddWithValue("Projektmeldung", item.Projektmeldung == null ? (object)DBNull.Value : item.Projektmeldung);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
				sqlCommand.Parameters.AddWithValue("Rapid_Prototyp", item.Rapid_Prototyp == null ? (object)DBNull.Value : item.Rapid_Prototyp);
				sqlCommand.Parameters.AddWithValue("Serie_PSZ", item.Serie_PSZ == null ? (object)DBNull.Value : item.Serie_PSZ);
				sqlCommand.Parameters.AddWithValue("SG_WKZ", item.SG_WKZ == null ? (object)DBNull.Value : item.SG_WKZ);
				sqlCommand.Parameters.AddWithValue("Standort_Muster", item.Standort_Muster == null ? (object)DBNull.Value : item.Standort_Muster);
				sqlCommand.Parameters.AddWithValue("Standort_Serie", item.Standort_Serie == null ? (object)DBNull.Value : item.Standort_Serie);
				sqlCommand.Parameters.AddWithValue("Summe_Arbeitszeit", item.Summe_Arbeitszeit == null ? (object)DBNull.Value : item.Summe_Arbeitszeit);
				sqlCommand.Parameters.AddWithValue("Termin_mit_Technik_abgesprochen", item.Termin_mit_Technik_abgesprochen == null ? (object)DBNull.Value : item.Termin_mit_Technik_abgesprochen);
				sqlCommand.Parameters.AddWithValue("TSP_Kunden", item.TSP_Kunden == null ? (object)DBNull.Value : item.TSP_Kunden);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("UL_Verpackung", item.UL_Verpackung == null ? (object)DBNull.Value : item.UL_Verpackung);
				sqlCommand.Parameters.AddWithValue("Wunschtermin_Kunde", item.Wunschtermin_Kunde == null ? (object)DBNull.Value : item.Wunschtermin_Kunde);
				sqlCommand.Parameters.AddWithValue("Zuschlag", item.Zuschlag == null ? (object)DBNull.Value : item.Zuschlag);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 37; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.PSZ_Projektdaten_DetailsEntity> items)
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
						query += " UPDATE [PSZ_Projektdaten_Details] SET "

							+ "[AB_Datum]=@AB_Datum" + i + ","
							+ "[Arbeitszeit Serien Pro Kabesatz]=@Arbeitszeit_Serien_Pro_Kabesatz" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bemerkungen]=@Bemerkungen" + i + ","
							+ "[EAU]=@EAU" + i + ","
							+ "[EMPB]=@EMPB" + i + ","
							+ "[Erstanlage]=@Erstanlage" + i + ","
							+ "[FA_Datum]=@FA_Datum" + i + ","
							+ "[Kontakt_AV_PSZ]=@Kontakt_AV_PSZ" + i + ","
							+ "[Kontakt_CS_PSZ]=@Kontakt_CS_PSZ" + i + ","
							+ "[Kontakt_Technik_Kunde]=@Kontakt_Technik_Kunde" + i + ","
							+ "[Kontakt_Technik_PSZ]=@Kontakt_Technik_PSZ" + i + ","
							+ "[Kosten]=@Kosten" + i + ","
							+ "[Krimp_WKZ]=@Krimp_WKZ" + i + ","
							+ "[Material_Eskalation_AV]=@Material_Eskalation_AV" + i + ","
							+ "[Material_Eskalation_Termin]=@Material_Eskalation_Termin" + i + ","
							+ "[Material_Komplett]=@Material_Komplett" + i + ","
							+ "[Menge]=@Menge" + i + ","
							+ "[MOQ]=@MOQ" + i + ","
							+ "[Projekt betreung]=@Projekt_betreung" + i + ","
							+ "[Projekt_Start]=@Projekt_Start" + i + ","
							+ "[Projektmeldung]=@Projektmeldung" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + ","
							+ "[Rapid Prototyp]=@Rapid_Prototyp" + i + ","
							+ "[Serie_PSZ]=@Serie_PSZ" + i + ","
							+ "[SG_WKZ]=@SG_WKZ" + i + ","
							+ "[Standort_Muster]=@Standort_Muster" + i + ","
							+ "[Standort_Serie]=@Standort_Serie" + i + ","
							+ "[Summe Arbeitszeit]=@Summe_Arbeitszeit" + i + ","
							+ "[Termin mit Technik abgesprochen]=@Termin_mit_Technik_abgesprochen" + i + ","
							+ "[TSP Kunden]=@TSP_Kunden" + i + ","
							+ "[Typ]=@Typ" + i + ","
							+ "[UL_Verpackung]=@UL_Verpackung" + i + ","
							+ "[Wunschtermin_Kunde]=@Wunschtermin_Kunde" + i + ","
							+ "[Zuschlag]=@Zuschlag" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("AB_Datum" + i, item.AB_Datum == null ? (object)DBNull.Value : item.AB_Datum);
						sqlCommand.Parameters.AddWithValue("Arbeitszeit_Serien_Pro_Kabesatz" + i, item.Arbeitszeit_Serien_Pro_Kabesatz == null ? (object)DBNull.Value : item.Arbeitszeit_Serien_Pro_Kabesatz);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("EAU" + i, item.EAU == null ? (object)DBNull.Value : item.EAU);
						sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
						sqlCommand.Parameters.AddWithValue("Erstanlage" + i, item.Erstanlage == null ? (object)DBNull.Value : item.Erstanlage);
						sqlCommand.Parameters.AddWithValue("FA_Datum" + i, item.FA_Datum == null ? (object)DBNull.Value : item.FA_Datum);
						sqlCommand.Parameters.AddWithValue("Kontakt_AV_PSZ" + i, item.Kontakt_AV_PSZ == null ? (object)DBNull.Value : item.Kontakt_AV_PSZ);
						sqlCommand.Parameters.AddWithValue("Kontakt_CS_PSZ" + i, item.Kontakt_CS_PSZ == null ? (object)DBNull.Value : item.Kontakt_CS_PSZ);
						sqlCommand.Parameters.AddWithValue("Kontakt_Technik_Kunde" + i, item.Kontakt_Technik_Kunde == null ? (object)DBNull.Value : item.Kontakt_Technik_Kunde);
						sqlCommand.Parameters.AddWithValue("Kontakt_Technik_PSZ" + i, item.Kontakt_Technik_PSZ == null ? (object)DBNull.Value : item.Kontakt_Technik_PSZ);
						sqlCommand.Parameters.AddWithValue("Kosten" + i, item.Kosten == null ? (object)DBNull.Value : item.Kosten);
						sqlCommand.Parameters.AddWithValue("Krimp_WKZ" + i, item.Krimp_WKZ == null ? (object)DBNull.Value : item.Krimp_WKZ);
						sqlCommand.Parameters.AddWithValue("Material_Eskalation_AV" + i, item.Material_Eskalation_AV == null ? (object)DBNull.Value : item.Material_Eskalation_AV);
						sqlCommand.Parameters.AddWithValue("Material_Eskalation_Termin" + i, item.Material_Eskalation_Termin == null ? (object)DBNull.Value : item.Material_Eskalation_Termin);
						sqlCommand.Parameters.AddWithValue("Material_Komplett" + i, item.Material_Komplett == null ? (object)DBNull.Value : item.Material_Komplett);
						sqlCommand.Parameters.AddWithValue("Menge" + i, item.Menge == null ? (object)DBNull.Value : item.Menge);
						sqlCommand.Parameters.AddWithValue("MOQ" + i, item.MOQ == null ? (object)DBNull.Value : item.MOQ);
						sqlCommand.Parameters.AddWithValue("Projekt_betreung" + i, item.Projekt_betreung == null ? (object)DBNull.Value : item.Projekt_betreung);
						sqlCommand.Parameters.AddWithValue("Projekt_Start" + i, item.Projekt_Start == null ? (object)DBNull.Value : item.Projekt_Start);
						sqlCommand.Parameters.AddWithValue("Projektmeldung" + i, item.Projektmeldung == null ? (object)DBNull.Value : item.Projektmeldung);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
						sqlCommand.Parameters.AddWithValue("Rapid_Prototyp" + i, item.Rapid_Prototyp == null ? (object)DBNull.Value : item.Rapid_Prototyp);
						sqlCommand.Parameters.AddWithValue("Serie_PSZ" + i, item.Serie_PSZ == null ? (object)DBNull.Value : item.Serie_PSZ);
						sqlCommand.Parameters.AddWithValue("SG_WKZ" + i, item.SG_WKZ == null ? (object)DBNull.Value : item.SG_WKZ);
						sqlCommand.Parameters.AddWithValue("Standort_Muster" + i, item.Standort_Muster == null ? (object)DBNull.Value : item.Standort_Muster);
						sqlCommand.Parameters.AddWithValue("Standort_Serie" + i, item.Standort_Serie == null ? (object)DBNull.Value : item.Standort_Serie);
						sqlCommand.Parameters.AddWithValue("Summe_Arbeitszeit" + i, item.Summe_Arbeitszeit == null ? (object)DBNull.Value : item.Summe_Arbeitszeit);
						sqlCommand.Parameters.AddWithValue("Termin_mit_Technik_abgesprochen" + i, item.Termin_mit_Technik_abgesprochen == null ? (object)DBNull.Value : item.Termin_mit_Technik_abgesprochen);
						sqlCommand.Parameters.AddWithValue("TSP_Kunden" + i, item.TSP_Kunden == null ? (object)DBNull.Value : item.TSP_Kunden);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("UL_Verpackung" + i, item.UL_Verpackung == null ? (object)DBNull.Value : item.UL_Verpackung);
						sqlCommand.Parameters.AddWithValue("Wunschtermin_Kunde" + i, item.Wunschtermin_Kunde == null ? (object)DBNull.Value : item.Wunschtermin_Kunde);
						sqlCommand.Parameters.AddWithValue("Zuschlag" + i, item.Zuschlag == null ? (object)DBNull.Value : item.Zuschlag);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
				string query = "DELETE FROM [PSZ_Projektdaten_Details] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [PSZ_Projektdaten_Details] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods



		#endregion
	}
}
