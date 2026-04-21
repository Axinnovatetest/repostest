using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class LieferantenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity Get(int nr)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__FNC_Lieferanten] WHERE [Nr]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__FNC_Lieferanten]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = "SELECT * FROM [__FNC_Lieferanten] WHERE [Nr] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_Lieferanten] ([Belegkreis],[Bestellbestätigung anmahnen],[Bestellimit],[Branche],[EG - Identifikationsnummer],[Eilzuschlag],[Frachtfreigrenze],[gesperrt für weitere Bestellungen],[Grund für Sperre],[Karenztage],[Konditionszuordnungs-Nr],[Kreditoren-Nr],[Kundennummer (Lieferanten)],[Kundennummer PSZ_AL (Lieferanten)],[Kundennummer PSZ_CZ (Lieferanten)],[Kundennummer PSZ_TN (Lieferanten)],[Kundennummer SC (Lieferanten)],[Kundennummer SC_CZ (Lieferanten)],[LH],[LH_Datum],[Lieferantengruppe],[Mahnsperre],[Mahnsperre (Lieferant)],[Mindestbestellwert],[nummer],[Rabattgruppe],[Sprache],[Umsatzsteuer berechnen],[Versandart],[Versandkosten],[Währung],[Wochentag_Anlieferung],[Zahlungsweise],[Zielaufschlag],[Zuschlag Mindestbestellwert]) VALUES (@Belegkreis,@Bestellbestätigung_anmahnen,@Bestellimit,@Branche,@EG__Identifikationsnummer,@Eilzuschlag,@Frachtfreigrenze,@gesperrt_für_weitere_Bestellungen,@Grund_für_Sperre,@Karenztage,@Konditionszuordnungs_Nr,@Kreditoren_Nr,@Kundennummer_Lieferanten,@Kundennummer_PSZ_ALLieferanten,@Kundennummer_PSZ_CZLieferanten,@Kundennummer_PSZ_TNLieferanten,@Kundennummer_SCLieferanten,@Kundennummer_SC_CZLieferanten,@LH,@LH_Datum,@Lieferantengruppe,@Mahnsperre,@Mahnsperre_Lieferant,@Mindestbestellwert,@nummer,@Rabattgruppe,@Sprache,@Umsatzsteuer_berechnen,@Versandart,@Versandkosten,@Währung,@Wochentag_Anlieferung,@Zahlungsweise,@Zielaufschlag,@Zuschlag_Mindestbestellwert)";

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

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Nr] FROM [__FNC_Lieferanten] WHERE [Nr] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_Lieferanten] SET [Belegkreis]=@Belegkreis, [Bestellbestätigung anmahnen]=@Bestellbestätigung_anmahnen, [Bestellimit]=@Bestellimit, [Branche]=@Branche, [EG - Identifikationsnummer]=@EG__Identifikationsnummer, [Eilzuschlag]=@Eilzuschlag, [Frachtfreigrenze]=@Frachtfreigrenze, [gesperrt für weitere Bestellungen]=@gesperrt_für_weitere_Bestellungen, [Grund für Sperre]=@Grund_für_Sperre, [Karenztage]=@Karenztage, [Konditionszuordnungs-Nr]=@Konditionszuordnungs_Nr, [Kreditoren-Nr]=@Kreditoren_Nr, [Kundennummer (Lieferanten)]=@Kundennummer_Lieferanten, [Kundennummer PSZ_AL (Lieferanten)]=@Kundennummer_PSZ_ALLieferanten, [Kundennummer PSZ_CZ (Lieferanten)]=@Kundennummer_PSZ_CZLieferanten, [Kundennummer PSZ_TN (Lieferanten)]=@Kundennummer_PSZ_TNLieferanten, [Kundennummer SC (Lieferanten)]=@Kundennummer_SCLieferanten, [Kundennummer SC_CZ (Lieferanten)]=@Kundennummer_SC_CZLieferanten, [LH]=@LH, [LH_Datum]=@LH_Datum, [Lieferantengruppe]=@Lieferantengruppe, [Mahnsperre]=@Mahnsperre, [Mahnsperre (Lieferant)]=@Mahnsperre_Lieferant, [Mindestbestellwert]=@Mindestbestellwert, [nummer]=@nummer, [Rabattgruppe]=@Rabattgruppe, [Sprache]=@Sprache, [Umsatzsteuer berechnen]=@Umsatzsteuer_berechnen, [Versandart]=@Versandart, [Versandkosten]=@Versandkosten, [Währung]=@Währung, [Wochentag_Anlieferung]=@Wochentag_Anlieferung, [Zahlungsweise]=@Zahlungsweise, [Zielaufschlag]=@Zielaufschlag, [Zuschlag Mindestbestellwert]=@Zuschlag_Mindestbestellwert WHERE [Nr]=@Nr";
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
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_Lieferanten] WHERE [Nr]=@Nr";
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [__FNC_Lieferanten] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int GetMaxNummber()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "SELECT MAX([nummer]) AS MaxNummer FROM [__FNC_Lieferanten]";

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

		public static List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity> SearchByNumberName(
			int? nummer,
			string name,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = " SELECT lf.* FROM [__FNC_Adressen] AS ad LEFT JOIN [__FNC_Lieferanten] AS lf ON ad.Nr = lf.nummer ";


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
				return new List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity>();
			}

			return toList(dataTable);
		}
		public static int SearchByNumberName_CountAll(int? nummer, string name)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*) FROM [__FNC_Adressen] AS ad LEFT JOIN [__FNC_Lieferanten] AS lf ON ad.Nr = lf.nummer ";

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
		public static List<Entities.Tables.FNC.LieferantenEntity> GetLikeNumber(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * FROM [__FNC_Lieferanten] WHERE Nr IS NOT NULL AND [nummer] LIKE '{nummer}%' ORDER by [nummer] ASC";
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

		public static List<Infrastructure.Data.Entities.Tables.FNC.Supplier_Article_BudgetEntity> GetSupplierArticle(string search = "")
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 100 Adres.Name1 as Article_supplier_name, Adres.Lieferantennummer, Adres.Ort, Lief.Nr, Anrede, Name1, Name2, Name3, Vorname, Fax, email, [Abteilung]," +
					" [Straße]+ ' '+ [Postfach] as StrassePostfach, [Straße] Strasse, [Land], [Land]+' '+[PLZ_Straße] as LandPLZ, [PLZ_Straße] PLZ, [Briefanrede], [Lieferantennummer], [Telefon], [Fax], [Versandart], Lief.[nummer], " +
					" [Zahlungsweise], Konds.[Text] AS Konditionszuordnungs, [Kundennummer (Lieferanten)] as IhrZeichen  " +
					"FROM __FNC_Lieferanten Lief " +
					"INNER JOIN __FNC_Adressen Adres ON Lief.nummer = Adres.Nr " +
					"JOIN [__FNC_Konditionszuordnungstabelle] Konds ON Konds.[Nr]=Lief.[Konditionszuordnungs-Nr] " +
					$"WHERE Adres.Lieferantennummer IS Not Null AND Adres.Adresstyp = 1 AND Name1 LIKE '%{search}%' OR Name2 LIKE '%{search}%' OR Adres.Lieferantennummer LIKE '%{search}%'";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toListSupplierArticle(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Supplier_Article_BudgetEntity>();
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.Supplier_Article_BudgetEntity> toListSupplierArticle(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.Supplier_Article_BudgetEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.FNC.Supplier_Article_BudgetEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
