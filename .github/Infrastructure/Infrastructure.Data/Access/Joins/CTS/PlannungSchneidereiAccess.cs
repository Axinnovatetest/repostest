using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class PlannungSchneidereiAccess
	{

		public static List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity> GetPlannungCZ()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT 
                                    [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Fertigungsnummer,
                                    [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Freigabestatus, 
                                    [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[PSZ Artikelnummer], 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Kunde, 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Halle, 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Termin_Bestätigt1]-21 AS[Termin Schneiderei], 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Termin_Bestätigt1, 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Quantity, 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Order Time], 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].FA_begonnen, 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Gewerk 1], 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Gewerk 2], 
	                                [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Gewerk 3], 
	                                Fertigung.FA_Gestartet
                                FROM
                                    Fertigung INNER JOIN[Fertigungs_Planung Gesamt übersicht ohne Mechanik]
                                    ON Fertigung.Fertigungsnummer = [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Fertigungsnummer
                                GROUP BY[Fertigungs_Planung Gesamt übersicht ohne Mechanik].Fertigungsnummer, [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Freigabestatus, [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[PSZ Artikelnummer], [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Kunde, [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Halle, [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Termin_Bestätigt1]-21, [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Termin_Bestätigt1, [Fertigungs_Planung Gesamt übersicht ohne Mechanik].Quantity, [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Order Time], [Fertigungs_Planung Gesamt übersicht ohne Mechanik].FA_begonnen, [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Gewerk 1], [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Gewerk 2], [Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Gewerk 3], Fertigung.FA_Gestartet
                                HAVING((([Fertigungs_Planung Gesamt übersicht ohne Mechanik].Termin_Bestätigt1)<=getDate()+14) AND((Fertigung.FA_Gestartet)= 1 Or (Fertigung.FA_Gestartet)= -1))
                                ORDER BY[Fertigungs_Planung Gesamt übersicht ohne Mechanik].[Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity> GetPlannungTN()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Fertigungsnummer,
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Freigabestatus,
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[PSZ Artikelnummer],
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Kunde,
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Halle, 
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Termin_Bestätigt1-21 AS [Termin Schneiderei],
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Termin_Bestätigt1 ,
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Quantity,
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[Order Time], 
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].FA_begonnen, 
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[Gewerk 1],
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[Gewerk 2],
                              [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[Gewerk 3],
                              Fertigung.FA_Gestartet
                              FROM [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien] INNER JOIN Fertigung ON [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Fertigungsnummer = Fertigung.Fertigungsnummer
                             GROUP BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Fertigungsnummer, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Freigabestatus, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[PSZ Artikelnummer], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Kunde, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Halle, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Termin_Bestätigt1-21, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Termin_Bestätigt1, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Quantity, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[Order Time], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].FA_begonnen, [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[Gewerk 1], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[Gewerk 2], [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].[Gewerk 3], Fertigung.FA_Gestartet
                             HAVING ((([Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Termin_Bestätigt1)<=getDate()+14) AND ((Fertigung.FA_Gestartet)=-1 Or (Fertigung.FA_Gestartet)=1))
                             ORDER BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik Tunesien].Termin_Bestätigt1-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity> GetPlannungWS()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Fertigungsnummer,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Freigabestatus,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[PSZ Artikelnummer], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Kunde,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Halle,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Termin_Bestätigt1-21 AS [Termin Schneiderei],
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Termin_Bestätigt1 ,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Quantity,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[Order Time],
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].FA_begonnen, 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[Gewerk 1], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[Gewerk 2],
[Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[Gewerk 3], 
Fertigung.FA_Gestartet
FROM Fertigung INNER JOIN [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN] ON Fertigung.Fertigungsnummer = [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Fertigungsnummer
GROUP BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Fertigungsnummer, [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Freigabestatus, [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[PSZ Artikelnummer], [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Kunde, [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Halle, [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Termin_Bestätigt1, [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Quantity, [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[Order Time], [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].FA_begonnen, [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[Gewerk 1], [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[Gewerk 2], [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].[Gewerk 3], Fertigung.FA_Gestartet
HAVING ((([Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Termin_Bestätigt1)<=getdate()+14) AND ((Fertigung.FA_Gestartet)=1 Or (Fertigung.FA_Gestartet)=-1))
ORDER BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik KHTN].Termin_Bestätigt1-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity> GetPlannungBETN()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Fertigungsnummer,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Freigabestatus, 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[PSZ Artikelnummer], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Kunde, 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Halle,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Termin_Bestätigt1-21 AS [Termin Schneiderei],
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Termin_Bestätigt1 , 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Quantity,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[Order Time], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].FA_begonnen, 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[Gewerk 1], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[Gewerk 2], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[Gewerk 3], 
Fertigung.FA_Gestartet
FROM Fertigung INNER JOIN [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN] ON Fertigung.Fertigungsnummer = [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Fertigungsnummer
GROUP BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Fertigungsnummer, [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Freigabestatus, [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[PSZ Artikelnummer], [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Kunde, [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Halle, [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Termin_Bestätigt1, [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Quantity, [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[Order Time], [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].FA_begonnen, [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[Gewerk 1], [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[Gewerk 2], [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].[Gewerk 3], Fertigung.FA_Gestartet
HAVING ((([Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Termin_Bestätigt1)<=getdate()+14) AND ((Fertigung.FA_Gestartet)=1 Or (Fertigung.FA_Gestartet)=-1))
ORDER BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik BETN].Termin_Bestätigt1-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity> GetPlannungGZTN()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Fertigungsnummer,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Freigabestatus, 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[PSZ Artikelnummer], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Kunde, 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Halle,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Termin_Bestätigt1-21 AS [Termin Schneiderei],
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Termin_Bestätigt1 , 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Quantity,
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[Order Time], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].FA_begonnen, 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[Gewerk 1], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[Gewerk 2], 
[Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[Gewerk 3], 
Fertigung.FA_Gestartet
FROM Fertigung INNER JOIN [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN] ON Fertigung.Fertigungsnummer = [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Fertigungsnummer
GROUP BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Fertigungsnummer, [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Freigabestatus, [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[PSZ Artikelnummer], [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Kunde, [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Halle, [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Termin_Bestätigt1, [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Quantity, [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[Order Time], [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].FA_begonnen, [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[Gewerk 1], [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[Gewerk 2], [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].[Gewerk 3], Fertigung.FA_Gestartet
HAVING ((([Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Termin_Bestätigt1)<=getdate()+14) AND ((Fertigung.FA_Gestartet)=1 Or (Fertigung.FA_Gestartet)=-1))
ORDER BY [Fertigungs_Planung Gesamt übersicht ohne Mechanik GZTN].Termin_Bestätigt1-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.PlannungSchneidereiEntity>();
			}
		}
	}
}
