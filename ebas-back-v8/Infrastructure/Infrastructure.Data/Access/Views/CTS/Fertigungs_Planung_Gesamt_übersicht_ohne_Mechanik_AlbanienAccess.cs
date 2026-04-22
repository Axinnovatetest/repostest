using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Views.CTS
{
	public class Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity Get(int fa_begonnen)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien] WHERE [fertigungsnummer]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", fa_begonnen);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity>();
		}
		private static List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien] WHERE [Fertigungsnummer] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity>();
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity> GetPlannungAL()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Fertigungsnummer, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Freigabestatus, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[PSZ Artikelnummer], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Kunde, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Halle, [Termin_Bestätigt1]-21 AS [Termin_Schneiderei], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Termin_Bestätigt1, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Quantity, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[Order Time], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].FA_begonnen, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[Gewerk 1], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[Gewerk 2], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[Gewerk 3]
                                    FROM [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien]
                                    GROUP BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Fertigungsnummer, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Freigabestatus, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[PSZ Artikelnummer], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Kunde, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Halle, [Termin_Bestätigt1]-21, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Termin_Bestätigt1, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Quantity, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[Order Time], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].FA_begonnen, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[Gewerk 1], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[Gewerk 2], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].[Gewerk 3]
                                    HAVING ((([Fertigungs_Planung Gesamt übersicht ohne Mechanik Albanien].Termin_Bestätigt1)<=getdate()+14))
                                    ORDER BY [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienEntity>();
			}
		}

		#endregion
	}
}
