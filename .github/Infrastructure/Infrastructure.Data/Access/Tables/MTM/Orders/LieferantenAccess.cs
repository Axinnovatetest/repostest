using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class LieferantenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lieferanten] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lieferanten]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Lieferanten] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Lieferanten] ([Belegkreis],[Bestellbestätigung anmahnen],[Bestellimit],[Branche],[EG - Identifikationsnummer],[Eilzuschlag],[Frachtfreigrenze],[gesperrt für weitere Bestellungen],[Grund für Sperre],[IMDS Firmen-ID],[Karenztage],[Konditionszuordnungs-Nr],[Kreditoren-Nr],[Kundennummer (Lieferanten)],[Kundennummer PSZ_AL (Lieferanten)],[Kundennummer PSZ_CZ (Lieferanten)],[Kundennummer PSZ_TN (Lieferanten)],[Kundennummer SC (Lieferanten)],[Kundennummer SC_CZ (Lieferanten)],[LH],[LH_Datum],[Lieferantengruppe],[Mahnsperre],[Mahnsperre (Lieferant)],[Mindestbestellwert],[nummer],[Rabattgruppe],[Sprache],[Umsatzsteuer berechnen],[Versandart],[Versandkosten],[Währung],[Wochentag_Anlieferung],[Zahlungsweise],[Zielaufschlag],[Zuschlag Mindestbestellwert]) OUTPUT INSERTED.[Nr] VALUES (@Belegkreis,@Bestellbestatigung_anmahnen,@Bestellimit,@Branche,@EG___Identifikationsnummer,@Eilzuschlag,@Frachtfreigrenze,@gesperrt_fur_weitere_Bestellungen,@Grund_fur_Sperre,@IMDS_Firmen_ID,@Karenztage,@Konditionszuordnungs_Nr,@Kreditoren_Nr,@Kundennummer__Lieferanten_,@Kundennummer_PSZ_AL__Lieferanten_,@Kundennummer_PSZ_CZ__Lieferanten_,@Kundennummer_PSZ_TN__Lieferanten_,@Kundennummer_SC__Lieferanten_,@Kundennummer_SC_CZ__Lieferanten_,@LH,@LH_Datum,@Lieferantengruppe,@Mahnsperre,@Mahnsperre__Lieferant_,@Mindestbestellwert,@nummer,@Rabattgruppe,@Sprache,@Umsatzsteuer_berechnen,@Versandart,@Versandkosten,@Wahrung,@Wochentag_Anlieferung,@Zahlungsweise,@Zielaufschlag,@Zuschlag_Mindestbestellwert); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_anmahnen", item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
					sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
					sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
					sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer", item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
					sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("gesperrt_fur_weitere_Bestellungen", item.gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.gesperrt_fur_weitere_Bestellungen);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
					sqlCommand.Parameters.AddWithValue("IMDS_Firmen_ID", item.IMDS_Firmen_ID == null ? (object)DBNull.Value : item.IMDS_Firmen_ID);
					sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
					sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
					sqlCommand.Parameters.AddWithValue("Kreditoren_Nr", item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
					sqlCommand.Parameters.AddWithValue("Kundennummer__Lieferanten_", item.Kundennummer__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_AL__Lieferanten_", item.Kundennummer_PSZ_AL__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZ__Lieferanten_", item.Kundennummer_PSZ_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TN__Lieferanten_", item.Kundennummer_PSZ_TN__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_SC__Lieferanten_", item.Kundennummer_SC__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZ__Lieferanten_", item.Kundennummer_SC_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("LH", item.LH == null ? (object)DBNull.Value : item.LH);
					sqlCommand.Parameters.AddWithValue("LH_Datum", item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
					sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
					sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
					sqlCommand.Parameters.AddWithValue("Mahnsperre__Lieferant_", item.Mahnsperre__Lieferant_ == null ? (object)DBNull.Value : item.Mahnsperre__Lieferant_);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("nummer", item.nummer == null ? (object)DBNull.Value : item.nummer);
					sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
					sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
					sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
					sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung", item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
					sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert", item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> items)
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
						query += " INSERT INTO [Lieferanten] ([Belegkreis],[Bestellbestätigung anmahnen],[Bestellimit],[Branche],[EG - Identifikationsnummer],[Eilzuschlag],[Frachtfreigrenze],[gesperrt für weitere Bestellungen],[Grund für Sperre],[IMDS Firmen-ID],[Karenztage],[Konditionszuordnungs-Nr],[Kreditoren-Nr],[Kundennummer (Lieferanten)],[Kundennummer PSZ_AL (Lieferanten)],[Kundennummer PSZ_CZ (Lieferanten)],[Kundennummer PSZ_TN (Lieferanten)],[Kundennummer SC (Lieferanten)],[Kundennummer SC_CZ (Lieferanten)],[LH],[LH_Datum],[Lieferantengruppe],[Mahnsperre],[Mahnsperre (Lieferant)],[Mindestbestellwert],[nummer],[Rabattgruppe],[Sprache],[Umsatzsteuer berechnen],[Versandart],[Versandkosten],[Währung],[Wochentag_Anlieferung],[Zahlungsweise],[Zielaufschlag],[Zuschlag Mindestbestellwert]) VALUES ( "

							+ "@Belegkreis" + i + ","
							+ "@Bestellbestatigung_anmahnen" + i + ","
							+ "@Bestellimit" + i + ","
							+ "@Branche" + i + ","
							+ "@EG___Identifikationsnummer" + i + ","
							+ "@Eilzuschlag" + i + ","
							+ "@Frachtfreigrenze" + i + ","
							+ "@gesperrt_fur_weitere_Bestellungen" + i + ","
							+ "@Grund_fur_Sperre" + i + ","
							+ "@IMDS_Firmen_ID" + i + ","
							+ "@Karenztage" + i + ","
							+ "@Konditionszuordnungs_Nr" + i + ","
							+ "@Kreditoren_Nr" + i + ","
							+ "@Kundennummer__Lieferanten_" + i + ","
							+ "@Kundennummer_PSZ_AL__Lieferanten_" + i + ","
							+ "@Kundennummer_PSZ_CZ__Lieferanten_" + i + ","
							+ "@Kundennummer_PSZ_TN__Lieferanten_" + i + ","
							+ "@Kundennummer_SC__Lieferanten_" + i + ","
							+ "@Kundennummer_SC_CZ__Lieferanten_" + i + ","
							+ "@LH" + i + ","
							+ "@LH_Datum" + i + ","
							+ "@Lieferantengruppe" + i + ","
							+ "@Mahnsperre" + i + ","
							+ "@Mahnsperre__Lieferant_" + i + ","
							+ "@Mindestbestellwert" + i + ","
							+ "@nummer" + i + ","
							+ "@Rabattgruppe" + i + ","
							+ "@Sprache" + i + ","
							+ "@Umsatzsteuer_berechnen" + i + ","
							+ "@Versandart" + i + ","
							+ "@Versandkosten" + i + ","
							+ "@Wahrung" + i + ","
							+ "@Wochentag_Anlieferung" + i + ","
							+ "@Zahlungsweise" + i + ","
							+ "@Zielaufschlag" + i + ","
							+ "@Zuschlag_Mindestbestellwert" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
						sqlCommand.Parameters.AddWithValue("Bestellbestatigung_anmahnen" + i, item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
						sqlCommand.Parameters.AddWithValue("Bestellimit" + i, item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
						sqlCommand.Parameters.AddWithValue("Branche" + i, item.Branche == null ? (object)DBNull.Value : item.Branche);
						sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer" + i, item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
						sqlCommand.Parameters.AddWithValue("Eilzuschlag" + i, item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
						sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
						sqlCommand.Parameters.AddWithValue("gesperrt_fur_weitere_Bestellungen" + i, item.gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.gesperrt_fur_weitere_Bestellungen);
						sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
						sqlCommand.Parameters.AddWithValue("IMDS_Firmen_ID" + i, item.IMDS_Firmen_ID == null ? (object)DBNull.Value : item.IMDS_Firmen_ID);
						sqlCommand.Parameters.AddWithValue("Karenztage" + i, item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
						sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr" + i, item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
						sqlCommand.Parameters.AddWithValue("Kreditoren_Nr" + i, item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
						sqlCommand.Parameters.AddWithValue("Kundennummer__Lieferanten_" + i, item.Kundennummer__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_AL__Lieferanten_" + i, item.Kundennummer_PSZ_AL__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZ__Lieferanten_" + i, item.Kundennummer_PSZ_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TN__Lieferanten_" + i, item.Kundennummer_PSZ_TN__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_SC__Lieferanten_" + i, item.Kundennummer_SC__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZ__Lieferanten_" + i, item.Kundennummer_SC_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("LH" + i, item.LH == null ? (object)DBNull.Value : item.LH);
						sqlCommand.Parameters.AddWithValue("LH_Datum" + i, item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
						sqlCommand.Parameters.AddWithValue("Lieferantengruppe" + i, item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
						sqlCommand.Parameters.AddWithValue("Mahnsperre" + i, item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
						sqlCommand.Parameters.AddWithValue("Mahnsperre__Lieferant_" + i, item.Mahnsperre__Lieferant_ == null ? (object)DBNull.Value : item.Mahnsperre__Lieferant_);
						sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
						sqlCommand.Parameters.AddWithValue("nummer" + i, item.nummer == null ? (object)DBNull.Value : item.nummer);
						sqlCommand.Parameters.AddWithValue("Rabattgruppe" + i, item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
						sqlCommand.Parameters.AddWithValue("Sprache" + i, item.Sprache == null ? (object)DBNull.Value : item.Sprache);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen" + i, item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Versandkosten" + i, item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
						sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
						sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung" + i, item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zielaufschlag" + i, item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
						sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert" + i, item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lieferanten] SET [Belegkreis]=@Belegkreis, [Bestellbestätigung anmahnen]=@Bestellbestatigung_anmahnen, [Bestellimit]=@Bestellimit, [Branche]=@Branche, [EG - Identifikationsnummer]=@EG___Identifikationsnummer, [Eilzuschlag]=@Eilzuschlag, [Frachtfreigrenze]=@Frachtfreigrenze, [gesperrt für weitere Bestellungen]=@gesperrt_fur_weitere_Bestellungen, [Grund für Sperre]=@Grund_fur_Sperre, [IMDS Firmen-ID]=@IMDS_Firmen_ID, [Karenztage]=@Karenztage, [Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr, [Kreditoren-Nr]=@Kreditoren_Nr, [Kundennummer (Lieferanten)]=@Kundennummer__Lieferanten_, [Kundennummer PSZ_AL (Lieferanten)]=@Kundennummer_PSZ_AL__Lieferanten_, [Kundennummer PSZ_CZ (Lieferanten)]=@Kundennummer_PSZ_CZ__Lieferanten_, [Kundennummer PSZ_TN (Lieferanten)]=@Kundennummer_PSZ_TN__Lieferanten_, [Kundennummer SC (Lieferanten)]=@Kundennummer_SC__Lieferanten_, [Kundennummer SC_CZ (Lieferanten)]=@Kundennummer_SC_CZ__Lieferanten_, [LH]=@LH, [LH_Datum]=@LH_Datum, [Lieferantengruppe]=@Lieferantengruppe, [Mahnsperre]=@Mahnsperre, [Mahnsperre (Lieferant)]=@Mahnsperre__Lieferant_, [Mindestbestellwert]=@Mindestbestellwert, [nummer]=@nummer, [Rabattgruppe]=@Rabattgruppe, [Sprache]=@Sprache, [Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen, [Versandart]=@Versandart, [Versandkosten]=@Versandkosten, [Währung]=@Wahrung, [Wochentag_Anlieferung]=@Wochentag_Anlieferung, [Zahlungsweise]=@Zahlungsweise, [Zielaufschlag]=@Zielaufschlag, [Zuschlag Mindestbestellwert]=@Zuschlag_Mindestbestellwert WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bestellbestatigung_anmahnen", item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
				sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
				sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
				sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer", item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
				sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
				sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
				sqlCommand.Parameters.AddWithValue("gesperrt_fur_weitere_Bestellungen", item.gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.gesperrt_fur_weitere_Bestellungen);
				sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
				sqlCommand.Parameters.AddWithValue("IMDS_Firmen_ID", item.IMDS_Firmen_ID == null ? (object)DBNull.Value : item.IMDS_Firmen_ID);
				sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
				sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
				sqlCommand.Parameters.AddWithValue("Kreditoren_Nr", item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
				sqlCommand.Parameters.AddWithValue("Kundennummer__Lieferanten_", item.Kundennummer__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer__Lieferanten_);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_AL__Lieferanten_", item.Kundennummer_PSZ_AL__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL__Lieferanten_);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZ__Lieferanten_", item.Kundennummer_PSZ_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ__Lieferanten_);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TN__Lieferanten_", item.Kundennummer_PSZ_TN__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN__Lieferanten_);
				sqlCommand.Parameters.AddWithValue("Kundennummer_SC__Lieferanten_", item.Kundennummer_SC__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC__Lieferanten_);
				sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZ__Lieferanten_", item.Kundennummer_SC_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ__Lieferanten_);
				sqlCommand.Parameters.AddWithValue("LH", item.LH == null ? (object)DBNull.Value : item.LH);
				sqlCommand.Parameters.AddWithValue("LH_Datum", item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
				sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
				sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
				sqlCommand.Parameters.AddWithValue("Mahnsperre__Lieferant_", item.Mahnsperre__Lieferant_ == null ? (object)DBNull.Value : item.Mahnsperre__Lieferant_);
				sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
				sqlCommand.Parameters.AddWithValue("nummer", item.nummer == null ? (object)DBNull.Value : item.nummer);
				sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
				sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
				sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
				sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung", item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
				sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert", item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> items)
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
						query += " UPDATE [Lieferanten] SET "

							+ "[Belegkreis]=@Belegkreis" + i + ","
							+ "[Bestellbestätigung anmahnen]=@Bestellbestatigung_anmahnen" + i + ","
							+ "[Bestellimit]=@Bestellimit" + i + ","
							+ "[Branche]=@Branche" + i + ","
							+ "[EG - Identifikationsnummer]=@EG___Identifikationsnummer" + i + ","
							+ "[Eilzuschlag]=@Eilzuschlag" + i + ","
							+ "[Frachtfreigrenze]=@Frachtfreigrenze" + i + ","
							+ "[gesperrt für weitere Bestellungen]=@gesperrt_fur_weitere_Bestellungen" + i + ","
							+ "[Grund für Sperre]=@Grund_fur_Sperre" + i + ","
							+ "[IMDS Firmen-ID]=@IMDS_Firmen_ID" + i + ","
							+ "[Karenztage]=@Karenztage" + i + ","
							+ "[Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr" + i + ","
							+ "[Kreditoren-Nr]=@Kreditoren_Nr" + i + ","
							+ "[Kundennummer (Lieferanten)]=@Kundennummer__Lieferanten_" + i + ","
							+ "[Kundennummer PSZ_AL (Lieferanten)]=@Kundennummer_PSZ_AL__Lieferanten_" + i + ","
							+ "[Kundennummer PSZ_CZ (Lieferanten)]=@Kundennummer_PSZ_CZ__Lieferanten_" + i + ","
							+ "[Kundennummer PSZ_TN (Lieferanten)]=@Kundennummer_PSZ_TN__Lieferanten_" + i + ","
							+ "[Kundennummer SC (Lieferanten)]=@Kundennummer_SC__Lieferanten_" + i + ","
							+ "[Kundennummer SC_CZ (Lieferanten)]=@Kundennummer_SC_CZ__Lieferanten_" + i + ","
							+ "[LH]=@LH" + i + ","
							+ "[LH_Datum]=@LH_Datum" + i + ","
							+ "[Lieferantengruppe]=@Lieferantengruppe" + i + ","
							+ "[Mahnsperre]=@Mahnsperre" + i + ","
							+ "[Mahnsperre (Lieferant)]=@Mahnsperre__Lieferant_" + i + ","
							+ "[Mindestbestellwert]=@Mindestbestellwert" + i + ","
							+ "[nummer]=@nummer" + i + ","
							+ "[Rabattgruppe]=@Rabattgruppe" + i + ","
							+ "[Sprache]=@Sprache" + i + ","
							+ "[Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen" + i + ","
							+ "[Versandart]=@Versandart" + i + ","
							+ "[Versandkosten]=@Versandkosten" + i + ","
							+ "[Währung]=@Wahrung" + i + ","
							+ "[Wochentag_Anlieferung]=@Wochentag_Anlieferung" + i + ","
							+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
							+ "[Zielaufschlag]=@Zielaufschlag" + i + ","
							+ "[Zuschlag Mindestbestellwert]=@Zuschlag_Mindestbestellwert" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
						sqlCommand.Parameters.AddWithValue("Bestellbestatigung_anmahnen" + i, item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
						sqlCommand.Parameters.AddWithValue("Bestellimit" + i, item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
						sqlCommand.Parameters.AddWithValue("Branche" + i, item.Branche == null ? (object)DBNull.Value : item.Branche);
						sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer" + i, item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
						sqlCommand.Parameters.AddWithValue("Eilzuschlag" + i, item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
						sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
						sqlCommand.Parameters.AddWithValue("gesperrt_fur_weitere_Bestellungen" + i, item.gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.gesperrt_fur_weitere_Bestellungen);
						sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
						sqlCommand.Parameters.AddWithValue("IMDS_Firmen_ID" + i, item.IMDS_Firmen_ID == null ? (object)DBNull.Value : item.IMDS_Firmen_ID);
						sqlCommand.Parameters.AddWithValue("Karenztage" + i, item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
						sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr" + i, item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
						sqlCommand.Parameters.AddWithValue("Kreditoren_Nr" + i, item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
						sqlCommand.Parameters.AddWithValue("Kundennummer__Lieferanten_" + i, item.Kundennummer__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_AL__Lieferanten_" + i, item.Kundennummer_PSZ_AL__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZ__Lieferanten_" + i, item.Kundennummer_PSZ_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TN__Lieferanten_" + i, item.Kundennummer_PSZ_TN__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_SC__Lieferanten_" + i, item.Kundennummer_SC__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZ__Lieferanten_" + i, item.Kundennummer_SC_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ__Lieferanten_);
						sqlCommand.Parameters.AddWithValue("LH" + i, item.LH == null ? (object)DBNull.Value : item.LH);
						sqlCommand.Parameters.AddWithValue("LH_Datum" + i, item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
						sqlCommand.Parameters.AddWithValue("Lieferantengruppe" + i, item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
						sqlCommand.Parameters.AddWithValue("Mahnsperre" + i, item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
						sqlCommand.Parameters.AddWithValue("Mahnsperre__Lieferant_" + i, item.Mahnsperre__Lieferant_ == null ? (object)DBNull.Value : item.Mahnsperre__Lieferant_);
						sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
						sqlCommand.Parameters.AddWithValue("nummer" + i, item.nummer == null ? (object)DBNull.Value : item.nummer);
						sqlCommand.Parameters.AddWithValue("Rabattgruppe" + i, item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
						sqlCommand.Parameters.AddWithValue("Sprache" + i, item.Sprache == null ? (object)DBNull.Value : item.Sprache);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen" + i, item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
						sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
						sqlCommand.Parameters.AddWithValue("Versandkosten" + i, item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
						sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
						sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung" + i, item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
						sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
						sqlCommand.Parameters.AddWithValue("Zielaufschlag" + i, item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
						sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert" + i, item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
				string query = "DELETE FROM [Lieferanten] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

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

					string query = "DELETE FROM [Lieferanten] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lieferanten] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lieferanten]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Lieferanten] WHERE [Nr] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Lieferanten] ([Belegkreis],[Bestellbestätigung anmahnen],[Bestellimit],[Branche],[EG - Identifikationsnummer],[Eilzuschlag],[Frachtfreigrenze],[gesperrt für weitere Bestellungen],[Grund für Sperre],[IMDS Firmen-ID],[Karenztage],[Konditionszuordnungs-Nr],[Kreditoren-Nr],[Kundennummer (Lieferanten)],[Kundennummer PSZ_AL (Lieferanten)],[Kundennummer PSZ_CZ (Lieferanten)],[Kundennummer PSZ_TN (Lieferanten)],[Kundennummer SC (Lieferanten)],[Kundennummer SC_CZ (Lieferanten)],[LH],[LH_Datum],[Lieferantengruppe],[Mahnsperre],[Mahnsperre (Lieferant)],[Mindestbestellwert],[nummer],[Rabattgruppe],[Sprache],[Umsatzsteuer berechnen],[Versandart],[Versandkosten],[Währung],[Wochentag_Anlieferung],[Zahlungsweise],[Zielaufschlag],[Zuschlag Mindestbestellwert]) OUTPUT INSERTED.[Nr] VALUES (@Belegkreis,@Bestellbestatigung_anmahnen,@Bestellimit,@Branche,@EG___Identifikationsnummer,@Eilzuschlag,@Frachtfreigrenze,@gesperrt_fur_weitere_Bestellungen,@Grund_fur_Sperre,@IMDS_Firmen_ID,@Karenztage,@Konditionszuordnungs_Nr,@Kreditoren_Nr,@Kundennummer__Lieferanten_,@Kundennummer_PSZ_AL__Lieferanten_,@Kundennummer_PSZ_CZ__Lieferanten_,@Kundennummer_PSZ_TN__Lieferanten_,@Kundennummer_SC__Lieferanten_,@Kundennummer_SC_CZ__Lieferanten_,@LH,@LH_Datum,@Lieferantengruppe,@Mahnsperre,@Mahnsperre__Lieferant_,@Mindestbestellwert,@nummer,@Rabattgruppe,@Sprache,@Umsatzsteuer_berechnen,@Versandart,@Versandkosten,@Wahrung,@Wochentag_Anlieferung,@Zahlungsweise,@Zielaufschlag,@Zuschlag_Mindestbestellwert); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bestellbestatigung_anmahnen", item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
			sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
			sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
			sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer", item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
			sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
			sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
			sqlCommand.Parameters.AddWithValue("gesperrt_fur_weitere_Bestellungen", item.gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.gesperrt_fur_weitere_Bestellungen);
			sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
			sqlCommand.Parameters.AddWithValue("IMDS_Firmen_ID", item.IMDS_Firmen_ID == null ? (object)DBNull.Value : item.IMDS_Firmen_ID);
			sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
			sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
			sqlCommand.Parameters.AddWithValue("Kreditoren_Nr", item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
			sqlCommand.Parameters.AddWithValue("Kundennummer__Lieferanten_", item.Kundennummer__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_AL__Lieferanten_", item.Kundennummer_PSZ_AL__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZ__Lieferanten_", item.Kundennummer_PSZ_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TN__Lieferanten_", item.Kundennummer_PSZ_TN__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_SC__Lieferanten_", item.Kundennummer_SC__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZ__Lieferanten_", item.Kundennummer_SC_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("LH", item.LH == null ? (object)DBNull.Value : item.LH);
			sqlCommand.Parameters.AddWithValue("LH_Datum", item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
			sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
			sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
			sqlCommand.Parameters.AddWithValue("Mahnsperre__Lieferant_", item.Mahnsperre__Lieferant_ == null ? (object)DBNull.Value : item.Mahnsperre__Lieferant_);
			sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
			sqlCommand.Parameters.AddWithValue("nummer", item.nummer == null ? (object)DBNull.Value : item.nummer);
			sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
			sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
			sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
			sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung", item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
			sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert", item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 37; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Lieferanten] ([Belegkreis],[Bestellbestätigung anmahnen],[Bestellimit],[Branche],[EG - Identifikationsnummer],[Eilzuschlag],[Frachtfreigrenze],[gesperrt für weitere Bestellungen],[Grund für Sperre],[IMDS Firmen-ID],[Karenztage],[Konditionszuordnungs-Nr],[Kreditoren-Nr],[Kundennummer (Lieferanten)],[Kundennummer PSZ_AL (Lieferanten)],[Kundennummer PSZ_CZ (Lieferanten)],[Kundennummer PSZ_TN (Lieferanten)],[Kundennummer SC (Lieferanten)],[Kundennummer SC_CZ (Lieferanten)],[LH],[LH_Datum],[Lieferantengruppe],[Mahnsperre],[Mahnsperre (Lieferant)],[Mindestbestellwert],[nummer],[Rabattgruppe],[Sprache],[Umsatzsteuer berechnen],[Versandart],[Versandkosten],[Währung],[Wochentag_Anlieferung],[Zahlungsweise],[Zielaufschlag],[Zuschlag Mindestbestellwert]) VALUES ( "

						+ "@Belegkreis" + i + ","
						+ "@Bestellbestatigung_anmahnen" + i + ","
						+ "@Bestellimit" + i + ","
						+ "@Branche" + i + ","
						+ "@EG___Identifikationsnummer" + i + ","
						+ "@Eilzuschlag" + i + ","
						+ "@Frachtfreigrenze" + i + ","
						+ "@gesperrt_fur_weitere_Bestellungen" + i + ","
						+ "@Grund_fur_Sperre" + i + ","
						+ "@IMDS_Firmen_ID" + i + ","
						+ "@Karenztage" + i + ","
						+ "@Konditionszuordnungs_Nr" + i + ","
						+ "@Kreditoren_Nr" + i + ","
						+ "@Kundennummer__Lieferanten_" + i + ","
						+ "@Kundennummer_PSZ_AL__Lieferanten_" + i + ","
						+ "@Kundennummer_PSZ_CZ__Lieferanten_" + i + ","
						+ "@Kundennummer_PSZ_TN__Lieferanten_" + i + ","
						+ "@Kundennummer_SC__Lieferanten_" + i + ","
						+ "@Kundennummer_SC_CZ__Lieferanten_" + i + ","
						+ "@LH" + i + ","
						+ "@LH_Datum" + i + ","
						+ "@Lieferantengruppe" + i + ","
						+ "@Mahnsperre" + i + ","
						+ "@Mahnsperre__Lieferant_" + i + ","
						+ "@Mindestbestellwert" + i + ","
						+ "@nummer" + i + ","
						+ "@Rabattgruppe" + i + ","
						+ "@Sprache" + i + ","
						+ "@Umsatzsteuer_berechnen" + i + ","
						+ "@Versandart" + i + ","
						+ "@Versandkosten" + i + ","
						+ "@Wahrung" + i + ","
						+ "@Wochentag_Anlieferung" + i + ","
						+ "@Zahlungsweise" + i + ","
						+ "@Zielaufschlag" + i + ","
						+ "@Zuschlag_Mindestbestellwert" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_anmahnen" + i, item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
					sqlCommand.Parameters.AddWithValue("Bestellimit" + i, item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
					sqlCommand.Parameters.AddWithValue("Branche" + i, item.Branche == null ? (object)DBNull.Value : item.Branche);
					sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer" + i, item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
					sqlCommand.Parameters.AddWithValue("Eilzuschlag" + i, item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("gesperrt_fur_weitere_Bestellungen" + i, item.gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.gesperrt_fur_weitere_Bestellungen);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
					sqlCommand.Parameters.AddWithValue("IMDS_Firmen_ID" + i, item.IMDS_Firmen_ID == null ? (object)DBNull.Value : item.IMDS_Firmen_ID);
					sqlCommand.Parameters.AddWithValue("Karenztage" + i, item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
					sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr" + i, item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
					sqlCommand.Parameters.AddWithValue("Kreditoren_Nr" + i, item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
					sqlCommand.Parameters.AddWithValue("Kundennummer__Lieferanten_" + i, item.Kundennummer__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_AL__Lieferanten_" + i, item.Kundennummer_PSZ_AL__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZ__Lieferanten_" + i, item.Kundennummer_PSZ_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TN__Lieferanten_" + i, item.Kundennummer_PSZ_TN__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_SC__Lieferanten_" + i, item.Kundennummer_SC__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZ__Lieferanten_" + i, item.Kundennummer_SC_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("LH" + i, item.LH == null ? (object)DBNull.Value : item.LH);
					sqlCommand.Parameters.AddWithValue("LH_Datum" + i, item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
					sqlCommand.Parameters.AddWithValue("Lieferantengruppe" + i, item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
					sqlCommand.Parameters.AddWithValue("Mahnsperre" + i, item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
					sqlCommand.Parameters.AddWithValue("Mahnsperre__Lieferant_" + i, item.Mahnsperre__Lieferant_ == null ? (object)DBNull.Value : item.Mahnsperre__Lieferant_);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("nummer" + i, item.nummer == null ? (object)DBNull.Value : item.nummer);
					sqlCommand.Parameters.AddWithValue("Rabattgruppe" + i, item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
					sqlCommand.Parameters.AddWithValue("Sprache" + i, item.Sprache == null ? (object)DBNull.Value : item.Sprache);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen" + i, item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Versandkosten" + i, item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
					sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung" + i, item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zielaufschlag" + i, item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
					sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert" + i, item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Lieferanten] SET [Belegkreis]=@Belegkreis, [Bestellbestätigung anmahnen]=@Bestellbestatigung_anmahnen, [Bestellimit]=@Bestellimit, [Branche]=@Branche, [EG - Identifikationsnummer]=@EG___Identifikationsnummer, [Eilzuschlag]=@Eilzuschlag, [Frachtfreigrenze]=@Frachtfreigrenze, [gesperrt für weitere Bestellungen]=@gesperrt_fur_weitere_Bestellungen, [Grund für Sperre]=@Grund_fur_Sperre, [IMDS Firmen-ID]=@IMDS_Firmen_ID, [Karenztage]=@Karenztage, [Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr, [Kreditoren-Nr]=@Kreditoren_Nr, [Kundennummer (Lieferanten)]=@Kundennummer__Lieferanten_, [Kundennummer PSZ_AL (Lieferanten)]=@Kundennummer_PSZ_AL__Lieferanten_, [Kundennummer PSZ_CZ (Lieferanten)]=@Kundennummer_PSZ_CZ__Lieferanten_, [Kundennummer PSZ_TN (Lieferanten)]=@Kundennummer_PSZ_TN__Lieferanten_, [Kundennummer SC (Lieferanten)]=@Kundennummer_SC__Lieferanten_, [Kundennummer SC_CZ (Lieferanten)]=@Kundennummer_SC_CZ__Lieferanten_, [LH]=@LH, [LH_Datum]=@LH_Datum, [Lieferantengruppe]=@Lieferantengruppe, [Mahnsperre]=@Mahnsperre, [Mahnsperre (Lieferant)]=@Mahnsperre__Lieferant_, [Mindestbestellwert]=@Mindestbestellwert, [nummer]=@nummer, [Rabattgruppe]=@Rabattgruppe, [Sprache]=@Sprache, [Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen, [Versandart]=@Versandart, [Versandkosten]=@Versandkosten, [Währung]=@Wahrung, [Wochentag_Anlieferung]=@Wochentag_Anlieferung, [Zahlungsweise]=@Zahlungsweise, [Zielaufschlag]=@Zielaufschlag, [Zuschlag Mindestbestellwert]=@Zuschlag_Mindestbestellwert WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
			sqlCommand.Parameters.AddWithValue("Bestellbestatigung_anmahnen", item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
			sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
			sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
			sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer", item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
			sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
			sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
			sqlCommand.Parameters.AddWithValue("gesperrt_fur_weitere_Bestellungen", item.gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.gesperrt_fur_weitere_Bestellungen);
			sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
			sqlCommand.Parameters.AddWithValue("IMDS_Firmen_ID", item.IMDS_Firmen_ID == null ? (object)DBNull.Value : item.IMDS_Firmen_ID);
			sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
			sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
			sqlCommand.Parameters.AddWithValue("Kreditoren_Nr", item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
			sqlCommand.Parameters.AddWithValue("Kundennummer__Lieferanten_", item.Kundennummer__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_AL__Lieferanten_", item.Kundennummer_PSZ_AL__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZ__Lieferanten_", item.Kundennummer_PSZ_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TN__Lieferanten_", item.Kundennummer_PSZ_TN__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_SC__Lieferanten_", item.Kundennummer_SC__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZ__Lieferanten_", item.Kundennummer_SC_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ__Lieferanten_);
			sqlCommand.Parameters.AddWithValue("LH", item.LH == null ? (object)DBNull.Value : item.LH);
			sqlCommand.Parameters.AddWithValue("LH_Datum", item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
			sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
			sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
			sqlCommand.Parameters.AddWithValue("Mahnsperre__Lieferant_", item.Mahnsperre__Lieferant_ == null ? (object)DBNull.Value : item.Mahnsperre__Lieferant_);
			sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
			sqlCommand.Parameters.AddWithValue("nummer", item.nummer == null ? (object)DBNull.Value : item.nummer);
			sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
			sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
			sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
			sqlCommand.Parameters.AddWithValue("Wahrung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
			sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung", item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
			sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
			sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
			sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert", item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 37; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Lieferanten] SET "

					+ "[Belegkreis]=@Belegkreis" + i + ","
					+ "[Bestellbestätigung anmahnen]=@Bestellbestatigung_anmahnen" + i + ","
					+ "[Bestellimit]=@Bestellimit" + i + ","
					+ "[Branche]=@Branche" + i + ","
					+ "[EG - Identifikationsnummer]=@EG___Identifikationsnummer" + i + ","
					+ "[Eilzuschlag]=@Eilzuschlag" + i + ","
					+ "[Frachtfreigrenze]=@Frachtfreigrenze" + i + ","
					+ "[gesperrt für weitere Bestellungen]=@gesperrt_fur_weitere_Bestellungen" + i + ","
					+ "[Grund für Sperre]=@Grund_fur_Sperre" + i + ","
					+ "[IMDS Firmen-ID]=@IMDS_Firmen_ID" + i + ","
					+ "[Karenztage]=@Karenztage" + i + ","
					+ "[Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr" + i + ","
					+ "[Kreditoren-Nr]=@Kreditoren_Nr" + i + ","
					+ "[Kundennummer (Lieferanten)]=@Kundennummer__Lieferanten_" + i + ","
					+ "[Kundennummer PSZ_AL (Lieferanten)]=@Kundennummer_PSZ_AL__Lieferanten_" + i + ","
					+ "[Kundennummer PSZ_CZ (Lieferanten)]=@Kundennummer_PSZ_CZ__Lieferanten_" + i + ","
					+ "[Kundennummer PSZ_TN (Lieferanten)]=@Kundennummer_PSZ_TN__Lieferanten_" + i + ","
					+ "[Kundennummer SC (Lieferanten)]=@Kundennummer_SC__Lieferanten_" + i + ","
					+ "[Kundennummer SC_CZ (Lieferanten)]=@Kundennummer_SC_CZ__Lieferanten_" + i + ","
					+ "[LH]=@LH" + i + ","
					+ "[LH_Datum]=@LH_Datum" + i + ","
					+ "[Lieferantengruppe]=@Lieferantengruppe" + i + ","
					+ "[Mahnsperre]=@Mahnsperre" + i + ","
					+ "[Mahnsperre (Lieferant)]=@Mahnsperre__Lieferant_" + i + ","
					+ "[Mindestbestellwert]=@Mindestbestellwert" + i + ","
					+ "[nummer]=@nummer" + i + ","
					+ "[Rabattgruppe]=@Rabattgruppe" + i + ","
					+ "[Sprache]=@Sprache" + i + ","
					+ "[Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen" + i + ","
					+ "[Versandart]=@Versandart" + i + ","
					+ "[Versandkosten]=@Versandkosten" + i + ","
					+ "[Währung]=@Wahrung" + i + ","
					+ "[Wochentag_Anlieferung]=@Wochentag_Anlieferung" + i + ","
					+ "[Zahlungsweise]=@Zahlungsweise" + i + ","
					+ "[Zielaufschlag]=@Zielaufschlag" + i + ","
					+ "[Zuschlag Mindestbestellwert]=@Zuschlag_Mindestbestellwert" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Belegkreis" + i, item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bestellbestatigung_anmahnen" + i, item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
					sqlCommand.Parameters.AddWithValue("Bestellimit" + i, item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
					sqlCommand.Parameters.AddWithValue("Branche" + i, item.Branche == null ? (object)DBNull.Value : item.Branche);
					sqlCommand.Parameters.AddWithValue("EG___Identifikationsnummer" + i, item.EG___Identifikationsnummer == null ? (object)DBNull.Value : item.EG___Identifikationsnummer);
					sqlCommand.Parameters.AddWithValue("Eilzuschlag" + i, item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze" + i, item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("gesperrt_fur_weitere_Bestellungen" + i, item.gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.gesperrt_fur_weitere_Bestellungen);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
					sqlCommand.Parameters.AddWithValue("IMDS_Firmen_ID" + i, item.IMDS_Firmen_ID == null ? (object)DBNull.Value : item.IMDS_Firmen_ID);
					sqlCommand.Parameters.AddWithValue("Karenztage" + i, item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
					sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr" + i, item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
					sqlCommand.Parameters.AddWithValue("Kreditoren_Nr" + i, item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
					sqlCommand.Parameters.AddWithValue("Kundennummer__Lieferanten_" + i, item.Kundennummer__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_AL__Lieferanten_" + i, item.Kundennummer_PSZ_AL__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZ__Lieferanten_" + i, item.Kundennummer_PSZ_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TN__Lieferanten_" + i, item.Kundennummer_PSZ_TN__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_SC__Lieferanten_" + i, item.Kundennummer_SC__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZ__Lieferanten_" + i, item.Kundennummer_SC_CZ__Lieferanten_ == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ__Lieferanten_);
					sqlCommand.Parameters.AddWithValue("LH" + i, item.LH == null ? (object)DBNull.Value : item.LH);
					sqlCommand.Parameters.AddWithValue("LH_Datum" + i, item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
					sqlCommand.Parameters.AddWithValue("Lieferantengruppe" + i, item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
					sqlCommand.Parameters.AddWithValue("Mahnsperre" + i, item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
					sqlCommand.Parameters.AddWithValue("Mahnsperre__Lieferant_" + i, item.Mahnsperre__Lieferant_ == null ? (object)DBNull.Value : item.Mahnsperre__Lieferant_);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert" + i, item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("nummer" + i, item.nummer == null ? (object)DBNull.Value : item.nummer);
					sqlCommand.Parameters.AddWithValue("Rabattgruppe" + i, item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
					sqlCommand.Parameters.AddWithValue("Sprache" + i, item.Sprache == null ? (object)DBNull.Value : item.Sprache);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen" + i, item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
					sqlCommand.Parameters.AddWithValue("Versandart" + i, item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Versandkosten" + i, item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
					sqlCommand.Parameters.AddWithValue("Wahrung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung" + i, item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
					sqlCommand.Parameters.AddWithValue("Zahlungsweise" + i, item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
					sqlCommand.Parameters.AddWithValue("Zielaufschlag" + i, item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
					sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert" + i, item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Lieferanten] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

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

				string query = "DELETE FROM [Lieferanten] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<int> GetForOrders(List<int> addressNrs)
		{
			if(addressNrs == null || addressNrs.Count <= 0)
				return new List<int>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT nummer FROM [Lieferanten] WHERE [nummer] IN ({string.Join(",", addressNrs)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.TryParse(x[0].ToString(), out var y) ? y : -1).ToList();
			}
			else
			{
				return new List<int>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity GetByAddressNr(int nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lieferanten] WHERE [nummer]=@nummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nummer", nummer);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.LieferantenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods

	}
}
