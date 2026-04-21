using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class LieferantenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity Get(int nr)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Lieferanten] WHERE [Nr]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [Lieferanten]", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Lieferanten] WHERE [Nr] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Lieferanten] ([Belegkreis],[Bestellbestätigung anmahnen],[Bestellimit],[Branche],[EG - Identifikationsnummer],[Eilzuschlag],[Frachtfreigrenze],[gesperrt für weitere Bestellungen],[Grund für Sperre],[Karenztage],[Konditionszuordnungs-Nr],[Kreditoren-Nr],[Kundennummer (Lieferanten)],[Kundennummer PSZ_AL (Lieferanten)],[Kundennummer PSZ_CZ (Lieferanten)],[Kundennummer PSZ_TN (Lieferanten)],[Kundennummer SC (Lieferanten)],[Kundennummer SC_CZ (Lieferanten)],[LH],[LH_Datum],[Lieferantengruppe],[Mahnsperre],[Mahnsperre (Lieferant)],[Mindestbestellwert],[nummer],[Rabattgruppe],[Sprache],[Umsatzsteuer berechnen],[Versandart],[Versandkosten],[Währung],[Wochentag_Anlieferung],[Zahlungsweise],[Zielaufschlag],[Zuschlag Mindestbestellwert]) VALUES (@Belegkreis,@Bestellbestätigung_anmahnen,@Bestellimit,@Branche,@EG__Identifikationsnummer,@Eilzuschlag,@Frachtfreigrenze,@gesperrt_für_weitere_Bestellungen,@Grund_für_Sperre,@Karenztage,@Konditionszuordnungs_Nr,@Kreditoren_Nr,@Kundennummer_Lieferanten,@Kundennummer_PSZ_ALLieferanten,@Kundennummer_PSZ_CZLieferanten,@Kundennummer_PSZ_TNLieferanten,@Kundennummer_SCLieferanten,@Kundennummer_SC_CZLieferanten,@LH,@LH_Datum,@Lieferantengruppe,@Mahnsperre,@Mahnsperre_Lieferant,@Mindestbestellwert,@nummer,@Rabattgruppe,@Sprache,@Umsatzsteuer_berechnen,@Versandart,@Versandkosten,@Währung,@Wochentag_Anlieferung,@Zahlungsweise,@Zielaufschlag,@Zuschlag_Mindestbestellwert);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
					sqlCommand.Parameters.AddWithValue("Bestellbestätigung_anmahnen", item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
					sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
					sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
					sqlCommand.Parameters.AddWithValue("EG__Identifikationsnummer", item.EG_Identifikationsnummer == null ? (object)DBNull.Value : item.EG_Identifikationsnummer);
					sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
					sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
					sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Bestellungen", item.Gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.Gesperrt_fur_weitere_Bestellungen);
					sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
					sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
					sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
					sqlCommand.Parameters.AddWithValue("Kreditoren_Nr", item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
					sqlCommand.Parameters.AddWithValue("Kundennummer_Lieferanten", item.Kundennummer_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_Lieferanten);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_ALLieferanten", item.Kundennummer_PSZ_AL_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL_Lieferanten);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZLieferanten", item.Kundennummer_PSZ_CZ_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ_Lieferanten);
					sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TNLieferanten", item.Kundennummer_PSZ_TN_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN_Lieferanten);
					sqlCommand.Parameters.AddWithValue("Kundennummer_SCLieferanten", item.Kundennummer_SC_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_SC_Lieferanten);
					sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZLieferanten", item.Kundennummer_SC_CZ_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ_Lieferanten);
					sqlCommand.Parameters.AddWithValue("LH", item.LH == null ? (object)DBNull.Value : item.LH);
					sqlCommand.Parameters.AddWithValue("LH_Datum", item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
					sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
					sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
					sqlCommand.Parameters.AddWithValue("Mahnsperre_Lieferant", item.Mahnsperre_Lieferant == null ? (object)DBNull.Value : item.Mahnsperre_Lieferant);
					sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
					sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
					sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
					sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
					sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
					sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
					sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
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

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lieferanten] SET [Belegkreis]=@Belegkreis, [Bestellbestätigung anmahnen]=@Bestellbestätigung_anmahnen, [Bestellimit]=@Bestellimit, [Branche]=@Branche, [EG - Identifikationsnummer]=@EG__Identifikationsnummer, [Eilzuschlag]=@Eilzuschlag, [Frachtfreigrenze]=@Frachtfreigrenze, [gesperrt für weitere Bestellungen]=@gesperrt_für_weitere_Bestellungen, [Grund für Sperre]=@Grund_für_Sperre, [Karenztage]=@Karenztage, [Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr, [Kreditoren-Nr]=@Kreditoren_Nr, [Kundennummer (Lieferanten)]=@Kundennummer_Lieferanten, [Kundennummer PSZ_AL (Lieferanten)]=@Kundennummer_PSZ_ALLieferanten, [Kundennummer PSZ_CZ (Lieferanten)]=@Kundennummer_PSZ_CZLieferanten, [Kundennummer PSZ_TN (Lieferanten)]=@Kundennummer_PSZ_TNLieferanten, [Kundennummer SC (Lieferanten)]=@Kundennummer_SCLieferanten, [Kundennummer SC_CZ (Lieferanten)]=@Kundennummer_SC_CZLieferanten, [LH]=@LH, [LH_Datum]=@LH_Datum, [Lieferantengruppe]=@Lieferantengruppe, [Mahnsperre]=@Mahnsperre, [Mahnsperre (Lieferant)]=@Mahnsperre_Lieferant, [Mindestbestellwert]=@Mindestbestellwert, [nummer]=@nummer, [Rabattgruppe]=@Rabattgruppe, [Sprache]=@Sprache, [Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen, [Versandart]=@Versandart, [Versandkosten]=@Versandkosten, [Währung]=@Währung, [Wochentag_Anlieferung]=@Wochentag_Anlieferung, [Zahlungsweise]=@Zahlungsweise, [Zielaufschlag]=@Zielaufschlag, [Zuschlag Mindestbestellwert]=@Zuschlag_Mindestbestellwert WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);

				sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bestellbestätigung_anmahnen", item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
				sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
				sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
				sqlCommand.Parameters.AddWithValue("EG__Identifikationsnummer", item.EG_Identifikationsnummer == null ? (object)DBNull.Value : item.EG_Identifikationsnummer);
				sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
				sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
				sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Bestellungen", item.Gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.Gesperrt_fur_weitere_Bestellungen);
				sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
				sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
				sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
				sqlCommand.Parameters.AddWithValue("Kreditoren_Nr", item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
				sqlCommand.Parameters.AddWithValue("Kundennummer_Lieferanten", item.Kundennummer_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_ALLieferanten", item.Kundennummer_PSZ_AL_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZLieferanten", item.Kundennummer_PSZ_CZ_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TNLieferanten", item.Kundennummer_PSZ_TN_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_SCLieferanten", item.Kundennummer_SC_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_SC_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZLieferanten", item.Kundennummer_SC_CZ_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ_Lieferanten);
				sqlCommand.Parameters.AddWithValue("LH", item.LH == null ? (object)DBNull.Value : item.LH);
				sqlCommand.Parameters.AddWithValue("LH_Datum", item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
				sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
				sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
				sqlCommand.Parameters.AddWithValue("Mahnsperre_Lieferant", item.Mahnsperre_Lieferant == null ? (object)DBNull.Value : item.Mahnsperre_Lieferant);
				sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
				sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
				sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
				sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
				sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
				sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung", item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
				sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert", item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [Lieferanten] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int UpdateLanguage(int Nr, int languageId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Lieferanten] SET [Sprache]=@Sprache WHERE [Nr]=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Nr", Nr);
			sqlCommand.Parameters.AddWithValue("Sprache", languageId);
			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int UpdateIndustryCascade(string oldBranche, string newBranche)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE [Lieferanten] SET [Branche]=@newBranche WHERE [Branche]=@oldBranche";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("oldBranche", oldBranche);
			sqlCommand.Parameters.AddWithValue("newBranche", newBranche);
			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int UpdateData(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"UPDATE [Lieferanten] SET [Belegkreis]=@Belegkreis,
				[Bestellimit]=@Bestellimit,
				[Branche]=@Branche,
				[gesperrt für weitere Bestellungen]=@gesperrt_für_weitere_Bestellungen,
				[Grund für Sperre]=@Grund_für_Sperre,
				[Karenztage]=@Karenztage,
				[Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr,
				[Lieferantengruppe]=@Lieferantengruppe,
				[Mahnsperre]=@Mahnsperre,
				[Mindestbestellwert]=@Mindestbestellwert,
				[nummer]=@nummer,
				[Rabattgruppe]=@Rabattgruppe,
				[Währung]=@Währung,
				[Zahlungsweise]=@Zahlungsweise,
				[Zielaufschlag]=@Zielaufschlag,
				[Zuschlag Mindestbestellwert]=@Zuschlag_Mindestbestellwert,
				[Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen,
				[EG - Identifikationsnummer]=@EG_Identifikationsnummer,
                [Kundennummer (Lieferanten)]=@Kundennummer_Lieferanten 
				WHERE [Nr]=@Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
				sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
				sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Bestellungen", item.Gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.Gesperrt_fur_weitere_Bestellungen);
				sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
				sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
				sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
				sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
				sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
				sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
				sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
				sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
				sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
				sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert", item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
				sqlCommand.Parameters.AddWithValue("EG_Identifikationsnummer", item.EG_Identifikationsnummer == null ? (object)DBNull.Value : item.EG_Identifikationsnummer);
				sqlCommand.Parameters.AddWithValue("Kundennummer_Lieferanten", item.Kundennummer_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_Lieferanten);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int GetMaxNummber()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT MAX([nummer]) AS MaxNummer FROM [Lieferanten]";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return 0;
			}

			var maxNummer = (dataTable.Rows[0]["MaxNummer"] == System.DBNull.Value)
				? (int?)null
				: Convert.ToInt32(dataTable.Rows[0]["MaxNummer"]);

			return maxNummer ?? 0;
		}
		public static int UpdateShipping(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity item)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = @"UPDATE [Lieferanten] SET [Wochentag_Anlieferung]=@Wochentag_Anlieferung,[Versandkosten]=@Versandkosten,
                            [Frachtfreigrenze]=@Frachtfreigrenze,[Eilzuschlag]=@Eilzuschlag,[Versandart]=@Versandart
                           WHERE [Nr]=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung", item.Wochentag_Anlieferung);
			sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten);
			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			//
			sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze);
			sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag);
			sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> SearchByNumberName(
			int? nummer,
			string name,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT lf.* FROM [adressen] AS ad LEFT JOIN [Lieferanten] AS lf ON ad.Nr = lf.nummer ";


				using(var sqlCommand = new SqlCommand())
				{
					if(nummer.HasValue)
					{
						query += " WHERE [Lieferantennummer]=@nummer  AND lf.nr is not null ";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Lieferantennummer is not null AND lf.nr is not null ";
					}

					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY Lieferantennummer DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
			}

			return toList(dataTable);
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> SearchByNumberNameV2(
	int? nummer,
	string name,
	Data.Access.Settings.SortingModel sorting,
	Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT lf.* FROM [adressen] AS ad LEFT JOIN [Lieferanten] AS lf ON ad.Nr = lf.nummer ";


				using(var sqlCommand = new SqlCommand())
				{
					if(nummer.HasValue)
					{
						//query += " WHERE [Lieferantennummer]=@nummer  AND lf.nr is not null ";
						query += " WHERE ad.Nr=@nummer AND lf.nr is not null";

						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						//query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Lieferantennummer is not null AND lf.nr is not null ";
						query += $" WHERE ad.Nr = '{name}' AND ad.Lieferantennummer is not null AND lf.nr is not null";

					}

					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY Lieferantennummer DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
			}

			return toList(dataTable);
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> SearchArchived(
			int? nummer,
			string name,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT lf.* FROM [adressen] AS ad LEFT JOIN [Lieferanten] AS lf ON ad.Nr = lf.nummer
                                inner join __BSD_LieferantenExtension X on lf.Nr=X.Nr
                                where ad.Adresstyp=1 and X.IsArchived=1";
				using(var sqlCommand = new SqlCommand())
				{
					if(nummer.HasValue)
					{
						query += " AND [Lieferantennummer]=@nummer  AND lf.nr is not null ";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						query += $" AND (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Lieferantennummer is not null AND lf.nr is not null ";
					}
					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY Lieferantennummer DESC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
			}

			return toList(dataTable);
		}
		public static int SearchByNumberName_CountAll(int? nummer, string name)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*) FROM [adressen] AS ad LEFT JOIN [Lieferanten] AS lf ON ad.Nr = lf.nummer ";

				using(var sqlCommand = new SqlCommand())
				{

					if(nummer.HasValue)
					{
						query += " WHERE [Lieferantennummer]=@nummer AND lf.nr is not null ";
						sqlCommand.Parameters.AddWithValue("nummer", nummer.HasValue ? nummer : -1);
					}
					else
					{
						query += $" WHERE (ad.Name1 Like '%{name}%' OR  ad.Name2 Like '%{name}%' OR  ad.Name3 Like '%{name}%') AND ad.Lieferantennummer is not null AND lf.nr is not null ";
					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}
		public static List<Entities.Tables.BSD.LieferantenEntity> GetLikeNumber(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * FROM [Lieferanten] WHERE Nr IS NOT NULL AND [nummer] LIKE '{nummer}%' ORDER by [nummer] ASC";
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity GetByNummer(int nummer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lieferanten] WHERE [Nummer]=@nummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nummer", nummer);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> GetByNummers(List<int> nummers)
		{
			if(nummers == null || nummers.Count <= 0)
				return null;

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Lieferanten] WHERE [Nummer] IN ({string.Join(",", nummers)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static bool GetSupplierWithDiscoutGroup(int discountGroupId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Lieferanten] where [Rabattgruppe]=@discountGroupId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("discountGroupId", discountGroupId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
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
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> GetByDiscoutGroup(int discountGroupId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lieferanten] where [Rabattgruppe]=@discountGroupId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("discountGroupId", discountGroupId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static bool GetSupplierWithConditionAssignement(int conditionAssignement)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Lieferanten] where [Konditionszuordnungs-Nr]=@conditionAssignement";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("conditionAssignement", conditionAssignement);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
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
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> GetByConditionAssignement(int conditionAssignement)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lieferanten] where [Konditionszuordnungs-Nr]=@conditionAssignement";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("conditionAssignement", conditionAssignement);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static bool GetSupplierWithIndustry(string industry)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Lieferanten] where Branche=@industry";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("industry", industry);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
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
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> GetByIndustry(string industry)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lieferanten] where Branche=@industry";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("industry", industry);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static bool GetSupplierWithSupplierGroup(string group)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Lieferanten] where Lieferantengruppe=@group";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("group", group);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
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
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> GetBySupplierGroup(string group)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lieferanten] where Lieferantengruppe=@group";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("group", group);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static bool GetSupplierWithCurrency(int currency)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Lieferanten] where Währung=@currency";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("currency", currency);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
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
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> GetByCurrency(int currency)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lieferanten] where Währung=@currency";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("currency", currency);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static bool GetSupplierrWithSlipCircle(int circle)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Lieferanten] where Belegkreis=@circle";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("circle", circle);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
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
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> GetBySlipCircle(int circle)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lieferanten] where Belegkreis=@circle";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("circle", circle);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static bool GetSupplierWithShippingMethod(string method)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) AS Nbr FROM [Lieferanten] where Versandart=@method";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("method", method);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
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

		public static List<string> GetBranches()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT Branche FROM [Lieferanten]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["Branche"].ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = -1;
			string query = "INSERT INTO [Lieferanten] ([Belegkreis],[Bestellbestätigung anmahnen],[Bestellimit],[Branche],[EG - Identifikationsnummer],[Eilzuschlag],[Frachtfreigrenze],[gesperrt für weitere Bestellungen],[Grund für Sperre],[Karenztage],[Konditionszuordnungs-Nr],[Kreditoren-Nr],[Kundennummer (Lieferanten)],[Kundennummer PSZ_AL (Lieferanten)],[Kundennummer PSZ_CZ (Lieferanten)],[Kundennummer PSZ_TN (Lieferanten)],[Kundennummer SC (Lieferanten)],[Kundennummer SC_CZ (Lieferanten)],[LH],[LH_Datum],[Lieferantengruppe],[Mahnsperre],[Mahnsperre (Lieferant)],[Mindestbestellwert],[nummer],[Rabattgruppe],[Sprache],[Umsatzsteuer berechnen],[Versandart],[Versandkosten],[Währung],[Wochentag_Anlieferung],[Zahlungsweise],[Zielaufschlag],[Zuschlag Mindestbestellwert]) VALUES (@Belegkreis,@Bestellbestätigung_anmahnen,@Bestellimit,@Branche,@EG__Identifikationsnummer,@Eilzuschlag,@Frachtfreigrenze,@gesperrt_für_weitere_Bestellungen,@Grund_für_Sperre,@Karenztage,@Konditionszuordnungs_Nr,@Kreditoren_Nr,@Kundennummer_Lieferanten,@Kundennummer_PSZ_ALLieferanten,@Kundennummer_PSZ_CZLieferanten,@Kundennummer_PSZ_TNLieferanten,@Kundennummer_SCLieferanten,@Kundennummer_SC_CZLieferanten,@LH,@LH_Datum,@Lieferantengruppe,@Mahnsperre,@Mahnsperre_Lieferant,@Mindestbestellwert,@nummer,@Rabattgruppe,@Sprache,@Umsatzsteuer_berechnen,@Versandart,@Versandkosten,@Währung,@Wochentag_Anlieferung,@Zahlungsweise,@Zielaufschlag,@Zuschlag_Mindestbestellwert);";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bestellbestätigung_anmahnen", item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
				sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
				sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
				sqlCommand.Parameters.AddWithValue("EG__Identifikationsnummer", item.EG_Identifikationsnummer == null ? (object)DBNull.Value : item.EG_Identifikationsnummer);
				sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
				sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
				sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Bestellungen", item.Gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.Gesperrt_fur_weitere_Bestellungen);
				sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
				sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
				sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
				sqlCommand.Parameters.AddWithValue("Kreditoren_Nr", item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
				sqlCommand.Parameters.AddWithValue("Kundennummer_Lieferanten", item.Kundennummer_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_ALLieferanten", item.Kundennummer_PSZ_AL_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZLieferanten", item.Kundennummer_PSZ_CZ_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TNLieferanten", item.Kundennummer_PSZ_TN_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_SCLieferanten", item.Kundennummer_SC_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_SC_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZLieferanten", item.Kundennummer_SC_CZ_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ_Lieferanten);
				sqlCommand.Parameters.AddWithValue("LH", item.LH == null ? (object)DBNull.Value : item.LH);
				sqlCommand.Parameters.AddWithValue("LH_Datum", item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
				sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
				sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
				sqlCommand.Parameters.AddWithValue("Mahnsperre_Lieferant", item.Mahnsperre_Lieferant == null ? (object)DBNull.Value : item.Mahnsperre_Lieferant);
				sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
				sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
				sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
				sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
				sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
				sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung", item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
				sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert", item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity GetByNummer(int nummer, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Lieferanten] WHERE [Nummer]=@nummer";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("nummer", nummer);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity Get(int nr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			using(var sqlCommand = new SqlCommand("SELECT * FROM [Lieferanten] WHERE [Nr]=@Id", sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> GetSuppliersExcept(List<int> Suppliersids)
		{
			var dataTable = new DataTable();
			string intListString = string.Join(",", Suppliersids);

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($"SELECT * FROM [Lieferanten] Where Nr Not IN ({intListString})", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
			}
		}
		public static int Update(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int results = -1;
			string query = "UPDATE [Lieferanten] SET [Belegkreis]=@Belegkreis, [Bestellbestätigung anmahnen]=@Bestellbestätigung_anmahnen, [Bestellimit]=@Bestellimit, [Branche]=@Branche, [EG - Identifikationsnummer]=@EG__Identifikationsnummer, [Eilzuschlag]=@Eilzuschlag, [Frachtfreigrenze]=@Frachtfreigrenze, [gesperrt für weitere Bestellungen]=@gesperrt_für_weitere_Bestellungen, [Grund für Sperre]=@Grund_für_Sperre, [Karenztage]=@Karenztage, [Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr, [Kreditoren-Nr]=@Kreditoren_Nr, [Kundennummer (Lieferanten)]=@Kundennummer_Lieferanten, [Kundennummer PSZ_AL (Lieferanten)]=@Kundennummer_PSZ_ALLieferanten, [Kundennummer PSZ_CZ (Lieferanten)]=@Kundennummer_PSZ_CZLieferanten, [Kundennummer PSZ_TN (Lieferanten)]=@Kundennummer_PSZ_TNLieferanten, [Kundennummer SC (Lieferanten)]=@Kundennummer_SCLieferanten, [Kundennummer SC_CZ (Lieferanten)]=@Kundennummer_SC_CZLieferanten, [LH]=@LH, [LH_Datum]=@LH_Datum, [Lieferantengruppe]=@Lieferantengruppe, [Mahnsperre]=@Mahnsperre, [Mahnsperre (Lieferant)]=@Mahnsperre_Lieferant, [Mindestbestellwert]=@Mindestbestellwert, [nummer]=@nummer, [Rabattgruppe]=@Rabattgruppe, [Sprache]=@Sprache, [Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen, [Versandart]=@Versandart, [Versandkosten]=@Versandkosten, [Währung]=@Währung, [Wochentag_Anlieferung]=@Wochentag_Anlieferung, [Zahlungsweise]=@Zahlungsweise, [Zielaufschlag]=@Zielaufschlag, [Zuschlag Mindestbestellwert]=@Zuschlag_Mindestbestellwert WHERE [Nr]=@Nr";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);

				sqlCommand.Parameters.AddWithValue("Belegkreis", item.Belegkreis == null ? (object)DBNull.Value : item.Belegkreis);
				sqlCommand.Parameters.AddWithValue("Bestellbestätigung_anmahnen", item.Bestellbestatigung_anmahnen == null ? (object)DBNull.Value : item.Bestellbestatigung_anmahnen);
				sqlCommand.Parameters.AddWithValue("Bestellimit", item.Bestellimit == null ? (object)DBNull.Value : item.Bestellimit);
				sqlCommand.Parameters.AddWithValue("Branche", item.Branche == null ? (object)DBNull.Value : item.Branche);
				sqlCommand.Parameters.AddWithValue("EG__Identifikationsnummer", item.EG_Identifikationsnummer == null ? (object)DBNull.Value : item.EG_Identifikationsnummer);
				sqlCommand.Parameters.AddWithValue("Eilzuschlag", item.Eilzuschlag == null ? (object)DBNull.Value : item.Eilzuschlag);
				sqlCommand.Parameters.AddWithValue("Frachtfreigrenze", item.Frachtfreigrenze == null ? (object)DBNull.Value : item.Frachtfreigrenze);
				sqlCommand.Parameters.AddWithValue("gesperrt_für_weitere_Bestellungen", item.Gesperrt_fur_weitere_Bestellungen == null ? (object)DBNull.Value : item.Gesperrt_fur_weitere_Bestellungen);
				sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
				sqlCommand.Parameters.AddWithValue("Karenztage", item.Karenztage == null ? (object)DBNull.Value : item.Karenztage);
				sqlCommand.Parameters.AddWithValue("Konditionszuordnungs_Nr", item.Konditionszuordnungs_Nr == null ? (object)DBNull.Value : item.Konditionszuordnungs_Nr);
				sqlCommand.Parameters.AddWithValue("Kreditoren_Nr", item.Kreditoren_Nr == null ? (object)DBNull.Value : item.Kreditoren_Nr);
				sqlCommand.Parameters.AddWithValue("Kundennummer_Lieferanten", item.Kundennummer_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_ALLieferanten", item.Kundennummer_PSZ_AL_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_AL_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_CZLieferanten", item.Kundennummer_PSZ_CZ_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_CZ_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_PSZ_TNLieferanten", item.Kundennummer_PSZ_TN_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_PSZ_TN_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_SCLieferanten", item.Kundennummer_SC_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_SC_Lieferanten);
				sqlCommand.Parameters.AddWithValue("Kundennummer_SC_CZLieferanten", item.Kundennummer_SC_CZ_Lieferanten == null ? (object)DBNull.Value : item.Kundennummer_SC_CZ_Lieferanten);
				sqlCommand.Parameters.AddWithValue("LH", item.LH == null ? (object)DBNull.Value : item.LH);
				sqlCommand.Parameters.AddWithValue("LH_Datum", item.LH_Datum == null ? (object)DBNull.Value : item.LH_Datum);
				sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);
				sqlCommand.Parameters.AddWithValue("Mahnsperre", item.Mahnsperre == null ? (object)DBNull.Value : item.Mahnsperre);
				sqlCommand.Parameters.AddWithValue("Mahnsperre_Lieferant", item.Mahnsperre_Lieferant == null ? (object)DBNull.Value : item.Mahnsperre_Lieferant);
				sqlCommand.Parameters.AddWithValue("Mindestbestellwert", item.Mindestbestellwert == null ? (object)DBNull.Value : item.Mindestbestellwert);
				sqlCommand.Parameters.AddWithValue("nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
				sqlCommand.Parameters.AddWithValue("Rabattgruppe", item.Rabattgruppe == null ? (object)DBNull.Value : item.Rabattgruppe);
				sqlCommand.Parameters.AddWithValue("Sprache", item.Sprache == null ? (object)DBNull.Value : item.Sprache);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer_berechnen", item.Umsatzsteuer_berechnen == null ? (object)DBNull.Value : item.Umsatzsteuer_berechnen);
				sqlCommand.Parameters.AddWithValue("Versandart", item.Versandart == null ? (object)DBNull.Value : item.Versandart);
				sqlCommand.Parameters.AddWithValue("Versandkosten", item.Versandkosten == null ? (object)DBNull.Value : item.Versandkosten);
				sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
				sqlCommand.Parameters.AddWithValue("Wochentag_Anlieferung", item.Wochentag_Anlieferung == null ? (object)DBNull.Value : item.Wochentag_Anlieferung);
				sqlCommand.Parameters.AddWithValue("Zahlungsweise", item.Zahlungsweise == null ? (object)DBNull.Value : item.Zahlungsweise);
				sqlCommand.Parameters.AddWithValue("Zielaufschlag", item.Zielaufschlag == null ? (object)DBNull.Value : item.Zielaufschlag);
				sqlCommand.Parameters.AddWithValue("Zuschlag_Mindestbestellwert", item.Zuschlag_Mindestbestellwert == null ? (object)DBNull.Value : item.Zuschlag_Mindestbestellwert);

				results = sqlCommand.ExecuteNonQuery();
				return results;
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
