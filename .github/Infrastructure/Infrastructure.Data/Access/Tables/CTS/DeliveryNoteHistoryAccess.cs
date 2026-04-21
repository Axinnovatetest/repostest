using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class DeliveryNoteHistoryAccess
	{
		public static List<Infrastructure.Data.Entities.Tables.CTS.DeliveryNoteHistoryEntity> GetDeliveryNoteHistory(string project_nr, string type)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT
                              [angebotene Artikel].[Nr],
                              [Angebote].[Projekt-Nr],
                              [Angebote].[Vorname/NameFirma],
                              [Angebote].[Typ],
                              [Angebote].[erledigt],
                              [Angebote].[Angebot-Nr] AS [Vorfall-Nr],
                              [angebotene Artikel].[Position],
                              [angebotene Artikel].[Liefertermin],
                              [angebotene Artikel].[Bezeichnung1],
                              [angebotene Artikel].[Anzahl],
                              [angebotene Artikel].[OriginalAnzahl],
                              [angebotene Artikel].[Geliefert] AS Geliefert_Abgerufen,
                              [angebotene Artikel].[erledigt_pos],
                              [angebotene Artikel].[Fertigungsnummer],
                              [angebotene Artikel].[Artikel-Nr],
                              [Angebote].Benutzer
                              FROM [Angebote] INNER JOIN [angebotene Artikel] ON Angebote.Nr=[angebotene Artikel].[Angebot-Nr]
                              WHERE [Angebote].[Projekt-Nr]=@project_nr And [Angebote].[gebucht]=1 AND [Angebote].[Typ]=@type
                              ORDER BY [angebotene Artikel].Liefertermin DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("project_nr", project_nr);
				sqlCommand.Parameters.AddWithValue("type", type);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.DeliveryNoteHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.DeliveryNoteHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.DeliveryNoteHistoryEntity> GetABHistory(string project_nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT
                              [angebotene Artikel].[Nr],
                              [Angebote].[Projekt-Nr],
                              [Angebote].[Vorname/NameFirma],
                              [Angebote].[Typ],
                              [Angebote].[erledigt],
                              [Angebote].[Angebot-Nr] AS [Vorfall-Nr],
                              [angebotene Artikel].[Position],
                              [angebotene Artikel].[Liefertermin],
                              [angebotene Artikel].[Bezeichnung1],
                              [angebotene Artikel].[Anzahl],
                              [angebotene Artikel].[OriginalAnzahl],
                              [angebotene Artikel].[Geliefert] AS Geliefert_Abgerufen,
                              [angebotene Artikel].[erledigt_pos],
                              [angebotene Artikel].[Fertigungsnummer],
                              [angebotene Artikel].[Artikel-Nr],
                              [Angebote].Benutzer
                              FROM [Angebote] INNER JOIN [angebotene Artikel] ON Angebote.Nr=[angebotene Artikel].[Angebot-Nr]
                              WHERE [Angebote].[Projekt-Nr]=@project_nr And [Angebote].[gebucht]=1
                              ORDER BY [angebotene Artikel].Liefertermin DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("project_nr", project_nr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.DeliveryNoteHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.DeliveryNoteHistoryEntity>();
			}
		}
	}
}
