using System.Threading.Tasks;
using Infrastructure.Data.Entities.Tables.BSD;
using Infrastructure.Data.Entities.Tables.PRS;

namespace Infrastructure.Data.Access.Joins
{
	public class ArticleStatisticsAccess
	{
		public class Logistics
		{
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsIn> GetIn(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM (SELECT " +
						"Artikel.[Artikel-Nr]," +
						"Bestellungen.[Projekt-Nr], " +
						"Bestellungen.Typ, " +
						"Artikel.Artikelnummer, " +
						"[bestellte Artikel].Anzahl, " +
						"[bestellte Artikel].Einheit, " +
						"Bestellungen.Datum, " +
						"adressen.Name1, " +
						"[bestellte Artikel].Liefertermin, " +
						"[bestellte Artikel].Bestätigter_Termin, " +
						"[bestellte Artikel].Bemerkung_Pos, " +
						"CAST(Bestellungen.[Bestellung-Nr] AS NVARCHAR(50)) AS [Bestellung-Nr], " +
						"[bestellte Artikel].erledigt_pos, " +
						"Bestellungen.gebucht, " +
						"[bestellte Artikel].[Start Anzahl], " +
						"[bestellte Artikel].Erhalten, " +
						"[PSZ_Lagerorte Zuordnung Fertigung].Fertigung, " +
						"CAST(CASE WHEN ISNULL(Bestellungen.Rahmenbestellung,0)>0 OR ISNULL([bestellte Artikel].[RA Pos zu Bestellposition],0)>0 THEN 1 ELSE 0 END AS BIT) Rahmenbestellung, " +
						"[bestellte Artikel].[AB-Nr_Lieferant] " +
					"FROM((Bestellungen INNER JOIN " +
						"([bestellte Artikel] " +
						"INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) " +
						"ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) " +
						"INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr) " +
						"INNER JOIN[PSZ_Lagerorte Zuordnung Fertigung] ON[bestellte Artikel].Lagerort_id = [PSZ_Lagerorte Zuordnung Fertigung].Lagerort_id " +
					"WHERE(((Artikel.Artikelnummer)= @artikelnummer)) /*AND ISNULL(Bestellungen.Rahmenbestellung,0)=0*/" +
					$@"
					UNION ALL
					SELECT a.[Artikel-Nr],[Projekt-Nr],'Rahmen' AS Typ, a.Artikelnummer, pr.Anzahl,pr.Einheit,r.Datum,r.[Vorname/NameFirma] as Name1, pr.Liefertermin, NULL as Bestätigter_Termin, NULL as Bemerkung_Pos, r.Bezug as [Bestellung-Nr], 
					pr.erledigt_pos, r.gebucht, pr.OriginalAnzahl as [Start Anzahl], pr.OriginalAnzahl-pr.Anzahl  as Erhalten, NULL as Fertigung, CAST(1 as BIT) AS Rahmenbestellung, NULL AS [AB-Nr_Lieferant]
					FROM (SELECT * FROM Artikel WHERE Artikelnummer = @artikelnummer) a
					JOIN [angebotene Artikel] pr on pr.[Artikel-Nr]=a.[Artikel-Nr]
					JOIN (SELECT * FROM Angebote a join __CTS_AngeboteBlanketExtension e on e.AngeboteNr=a.Nr WHERE Typ='Rahmenauftrag' and e.BlanketTypeName='purchase') r on r.nr=pr.[Angebot-Nr]
					) AS TMP
					ORDER BY [Projekt-Nr] DESC, Datum DESC
					";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsIn(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsOut> GetOut(int articleNr, string searchTerm, bool? booked, bool? completed, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize <= 0)
					pageSize = 1;
				if(string.IsNullOrWhiteSpace(sortColumn))
					sortColumn = "termin";

				searchTerm = searchTerm ?? "";
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT " +
						"Artikel.[Artikel-Nr]," +
						"Artikel.Artikelnummer AS PSZ_Nummer, " +
						"Artikel.[Bezeichnung 1] AS Kundennummer, " +
						"Angebote.Typ, Angebote.[Angebot-Nr] AS Nummer, " +
						"[angebotene Artikel].OriginalAnzahl AS Bestellt, " +
						"[angebotene Artikel].Anzahl AS [OffenAktuell], " +
						"[angebotene Artikel].Liefertermin AS Termin, " +
						"Angebote.Bezug, " +
						"Angebote.gebucht, " +
						"[angebotene Artikel].Lagerort_id AS Auslieferlager, " +
						"[angebotene Artikel].erledigt_pos AS Erledigt, Angebote.Nr OrderId " +
					"FROM Angebote " +
						"INNER JOIN " +
						"(Artikel INNER JOIN [angebotene Artikel] ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr] " +
					$"WHERE Artikel.[Artikel-Nr]=@artikelnr AND ([Bezeichnung 1] LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR Angebote.Typ LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR Angebote.[Angebot-Nr] LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [angebotene Artikel].OriginalAnzahl LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [angebotene Artikel].Anzahl LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [angebotene Artikel].Liefertermin LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [Angebote].Bezug LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [angebotene Artikel].Lagerort_id LIKE '{searchTerm.SqlEscape()}%' ) ";
					if(booked.HasValue)
					{
						query += $" AND [Angebote].gebucht={(booked.Value ? "1" : "0")}";
					}
					if(completed.HasValue)
					{
						query += $" AND [angebotene Artikel].erledigt_pos={(completed.Value ? "1" : "0")}";
					}
					query += $@" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnr", articleNr);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsOut(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetOut_Count(int articleNr, string searchTerm, bool? booked, bool? completed)
			{
				searchTerm = searchTerm ?? "";
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT COUNT(*) FROM Angebote 
						INNER JOIN
						(Artikel INNER JOIN [angebotene Artikel] ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) 
							ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
						WHERE Artikel.[Artikel-Nr]=@artikelnr " +
						$"AND ([Artikel].[Bezeichnung 1] LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [Angebote].Typ LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [Angebote].[Angebot-Nr] LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [angebotene Artikel].OriginalAnzahl LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [angebotene Artikel].Anzahl LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [angebotene Artikel].Liefertermin LIKE '{searchTerm.SqlEscape()}%' " +
						$"OR [Angebote].Bezug LIKE '{searchTerm.SqlEscape()}%'  " +
						$"OR [angebotene Artikel].Lagerort_id LIKE '{searchTerm.SqlEscape()}%' ) ";
					if(booked.HasValue)
					{
						query += $" AND [Angebote].gebucht={(booked.Value ? "1" : "0")}";
					}
					if(completed.HasValue)
					{
						query += $" AND [angebotene Artikel].erledigt_pos={(completed.Value ? "1" : "0")}";
					}
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnr", articleNr);
					sqlCommand.CommandTimeout = 300;

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsInOutDetails> GetInOutDetails(string articleNumber, string date, string name, string orderNumber, string type, string lagerFrom, string lagerTo, string rollerNumber, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize <= 0)
					pageSize = 1;
				if(string.IsNullOrWhiteSpace(sortColumn))
					sortColumn = "Artikelnummer";

				date = date ?? "";
				name = name ?? "";
				orderNumber = orderNumber ?? "";
				lagerFrom = lagerFrom ?? "";
				lagerTo = lagerTo ?? "";
				rollerNumber = rollerNumber ?? "";
				if(string.IsNullOrWhiteSpace(type))
					type = "";


				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT [Artikel-Nr], Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_von, Lagerplatz_nach,Rollennummer,[Gebucht von], OrderId,CAST(Bemerkung AS NVARCHAR(MAX)) Bemerkung FROM (
                                    /* Q1 */
                                    SELECT 
										Artikel.[Artikel-Nr],
	                                    Artikel.Artikelnummer, 
	                                    Bestellungen.Typ, 
	                                    Bestellungen.[Bestellung-Nr], 
	                                    [bestellte Artikel].Anzahl, 
	                                    Bestellungen.Datum, 
	                                    adressen.Name1, 
	                                    0 AS Lagerplatz_von, 
	                                    [bestellte Artikel].Lagerort_id AS Lagerplatz_nach,
	                                    '' AS Rollennummer,
	                                    '' AS [Gebucht von], Bestellungen.Nr AS OrderId,
										'' AS Bemerkung 
	                                    /* INTO [PSZ_Artikelübersicht Ein_Aus] */
                                    FROM 
	                                    ((Artikel INNER JOIN [bestellte Artikel] ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]) 
	                                    INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr) INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer) AND ((Bestellungen.Typ)='wareneingang'))

                                    UNION ALL
                                    /* Q2 */
                                    /* INSERT INTO [PSZ_Artikelübersicht Ein_Aus] ( Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_von, Lagerplatz_nach ) */
                                    SELECT 
										Artikel.[Artikel-Nr],
	                                    Artikel.Artikelnummer, 
	                                    'Fertigungsauftrag' AS Typ, 
	                                    Fertigung.Fertigungsnummer AS [Bestellung-Nr], 
	                                    Fertigung_Fertigungsvorgang.Anzahl, 
	                                    Fertigung_Fertigungsvorgang.Datum, 
	                                    'PSZ' AS Abbucher, 
	                                    Fertigung_Fertigungsvorgang.Lagerort_id AS Lagerplatz_von, 
	                                    0 AS Lagerplatz_nach,
	                                    '' AS Rollennummer,
	                                    '' AS [Gebucht von], Fertigung.ID AS OrderId,
										'' AS Bemerkung 
                                    FROM 
	                                    Artikel INNER JOIN (Fertigung_Fertigungsvorgang INNER JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) ON Artikel.[Artikel-Nr] = Fertigung_Fertigungsvorgang.Artikel_nr
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer))

                                    UNION ALL
                                    /* Q3 */
                                    /* INSERT INTO [PSZ_Artikelübersicht Ein_Aus] ( Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_von, Lagerplatz_nach ) */
                                    SELECT 
										Artikel.[Artikel-Nr],
	                                    Artikel.Artikelnummer, 
	                                    Lagerbewegungen.Typ, 
	                                    0 AS Ausdr1, 
	                                    IIf([Typ]='Entnahme',[Anzahl]*(-1),[Anzahl]) AS Menge, 
	                                    Lagerbewegungen.Datum, 
	                                    'PSZ' AS Abbucher, 
	                                    Lagerbewegungen_Artikel.Lager_von, 
	                                    0 AS Ausdr2,
	                                    Lagerbewegungen_Artikel.Rollennummer,
	                                    Lagerbewegungen_Artikel.[Gebucht von], 0 AS OrderId,
										Lagerbewegungen_Artikel.Bemerkung
                                    FROM 
	                                    (Artikel INNER JOIN Lagerbewegungen_Artikel ON Artikel.[Artikel-Nr] = Lagerbewegungen_Artikel.[Artikel-nr]) 
	                                    INNER JOIN Lagerbewegungen ON Lagerbewegungen_Artikel.Lagerbewegungen_id = Lagerbewegungen.ID
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer) AND ((Lagerbewegungen.Typ)<>'Umbuchung' And (Lagerbewegungen.Typ)<>'Zugang direkt'))

                                    UNION ALL
                                    /* Q4 */
                                    /* INSERT INTO [PSZ_Artikelübersicht Ein_Aus] ( Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_nach, Lagerplatz_von ) */
                                    SELECT 
										Artikel.[Artikel-Nr],
	                                    Artikel.Artikelnummer, 
	                                    Lagerbewegungen.Typ, 
	                                    0 AS Ausdr1, 
	                                    IIf([Typ]='Entnahme',[Anzahl]*(-1),[Anzahl]) AS Menge, 
	                                    Lagerbewegungen.Datum, 
	                                    'PSZ' AS Abbucher, 
	                                    0 AS Ausdr2,
	                                    Lagerbewegungen_Artikel.Lager_von, 
	                                    Lagerbewegungen_Artikel.Rollennummer,
	                                    Lagerbewegungen_Artikel.[Gebucht von], 0 AS OrderId,
										Lagerbewegungen_Artikel.Bemerkung
                                    FROM 
	                                    (Artikel INNER JOIN Lagerbewegungen_Artikel ON Artikel.[Artikel-Nr] = Lagerbewegungen_Artikel.[Artikel-nr]) 
	                                    INNER JOIN Lagerbewegungen ON Lagerbewegungen_Artikel.Lagerbewegungen_id = Lagerbewegungen.ID
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer) AND ((Lagerbewegungen.Typ)<>'Umbuchung' And (Lagerbewegungen.Typ)<>'Entnahme'))

                                    UNION ALL
                                    /* Q5 */
                                    /* INSERT INTO [PSZ_Artikelübersicht Ein_Aus] ( Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_von, Lagerplatz_nach ) */
                                    SELECT 
										Artikel.[Artikel-Nr],
	                                    Artikel.Artikelnummer, 
	                                    Lagerbewegungen.Typ,   
	                                    0 AS Ausdr1, 
	                                    IIf([Typ]='Entnahme',[Anzahl]*(-1),[Anzahl]) AS Menge, 
	                                    Lagerbewegungen.Datum,
	                                    'PSZ' AS Abbucher,
	                                    Lagerbewegungen_Artikel.Lager_von, 
	                                    Lagerbewegungen_Artikel.Lager_nach,
	                                    Lagerbewegungen_Artikel.Rollennummer,
	                                    Lagerbewegungen_Artikel.[Gebucht von], 0 AS OrderId,
										Lagerbewegungen_Artikel.Bemerkung
                                    FROM 
	                                    (Artikel INNER JOIN Lagerbewegungen_Artikel ON Artikel.[Artikel-Nr] = Lagerbewegungen_Artikel.[Artikel-nr]) 
	                                    INNER JOIN Lagerbewegungen ON Lagerbewegungen_Artikel.Lagerbewegungen_id = Lagerbewegungen.ID
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer) AND ((Lagerbewegungen.Typ)<>'Zugang direkt' And (Lagerbewegungen.Typ)<>'Entnahme'))";
					query += $") AS TMP WHERE COALESCE(CONVERT(NCHAR(8),[Datum],112), '') LIKE '{date}%' AND COALESCE([Bestellung-Nr], '') LIKE '{orderNumber.SqlEscape()}%' AND COALESCE([Typ], '') LIKE '{type.SqlEscape()}%' AND COALESCE([Name1], '') LIKE '{name.SqlEscape()}%' AND COALESCE([Lagerplatz_von], '') LIKE '{lagerFrom.SqlEscape()}%' AND COALESCE([Lagerplatz_nach], '') LIKE '{lagerTo.SqlEscape()}%' AND COALESCE([Rollennummer], '') LIKE '{rollerNumber.SqlEscape()}%'" +
						$" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", articleNumber);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsInOutDetails(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetInOutDetails_Count(string articleNumber, string date, string name, string orderNumber, string type, string lagerFrom, string lagerTo, string rollerNumber)
			{
				date = date ?? "";
				name = name ?? "";
				orderNumber = orderNumber ?? "";
				lagerFrom = lagerFrom ?? "";
				lagerTo = lagerTo ?? "";
				rollerNumber = rollerNumber ?? "";

				if(string.IsNullOrWhiteSpace(type))
					type = "";

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
                            SELECT COUNT(*) as R
                            FROM 
                                (
                                    /* Q1 */
                                    SELECT 
	                                    Artikel.Artikelnummer, 
	                                    Bestellungen.Typ, 
	                                    Bestellungen.[Bestellung-Nr], 
	                                    [bestellte Artikel].Anzahl, 
	                                    Bestellungen.Datum, 
	                                    adressen.Name1, 
	                                    0 AS Lagerplatz_von, 
	                                    [bestellte Artikel].Lagerort_id AS Lagerplatz_nach,
	                                    '' AS Rollennummer,
	                                    '' AS [Gebucht von] 
	                                    /* INTO [PSZ_Artikelübersicht Ein_Aus] */
                                    FROM 
	                                    ((Artikel INNER JOIN [bestellte Artikel] ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]) 
	                                    INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr) INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer) AND ((Bestellungen.Typ)='wareneingang'))

                                    UNION ALL
                                    /* Q2 */
                                    /* INSERT INTO [PSZ_Artikelübersicht Ein_Aus] ( Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_von, Lagerplatz_nach ) */
                                    SELECT 
	                                    Artikel.Artikelnummer, 
	                                    'Fertigungsauftrag' AS Typ, 
	                                    Fertigung.Fertigungsnummer AS [Bestellung-Nr], 
	                                    Fertigung_Fertigungsvorgang.Anzahl, 
	                                    Fertigung_Fertigungsvorgang.Datum, 
	                                    'PSZ' AS Abbucher, 
	                                    Fertigung_Fertigungsvorgang.Lagerort_id AS Lagerplatz_von, 
	                                    0 AS Lagerplatz_nach,
	                                    '' AS Rollennummer,
	                                    '' AS [Gebucht von] 
                                    FROM 
	                                    Artikel INNER JOIN (Fertigung_Fertigungsvorgang INNER JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) ON Artikel.[Artikel-Nr] = Fertigung_Fertigungsvorgang.Artikel_nr
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer))

                                    UNION ALL
                                    /* Q3 */
                                    /* INSERT INTO [PSZ_Artikelübersicht Ein_Aus] ( Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_von, Lagerplatz_nach ) */
                                    SELECT 
	                                    Artikel.Artikelnummer, 
	                                    Lagerbewegungen.Typ, 
	                                    0 AS Ausdr1, 
	                                    IIf([Typ]='Entnahme',[Anzahl]*(-1),[Anzahl]) AS Menge, 
	                                    Lagerbewegungen.Datum, 
	                                    'PSZ' AS Abbucher, 
	                                    Lagerbewegungen_Artikel.Lager_von, 
	                                    0 AS Ausdr2,
	                                    Lagerbewegungen_Artikel.Rollennummer,
	                                    Lagerbewegungen_Artikel.[Gebucht von]
                                    FROM 
	                                    (Artikel INNER JOIN Lagerbewegungen_Artikel ON Artikel.[Artikel-Nr] = Lagerbewegungen_Artikel.[Artikel-nr]) 
	                                    INNER JOIN Lagerbewegungen ON Lagerbewegungen_Artikel.Lagerbewegungen_id = Lagerbewegungen.ID
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer) AND ((Lagerbewegungen.Typ)<>'Umbuchung' And (Lagerbewegungen.Typ)<>'Zugang direkt'))

                                    UNION ALL
                                    /* Q4 */
                                    /* INSERT INTO [PSZ_Artikelübersicht Ein_Aus] ( Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_nach, Lagerplatz_von ) */
                                    SELECT 
	                                    Artikel.Artikelnummer, 
	                                    Lagerbewegungen.Typ, 
	                                    0 AS Ausdr1, 
	                                    IIf([Typ]='Entnahme',[Anzahl]*(-1),[Anzahl]) AS Menge, 
	                                    Lagerbewegungen.Datum, 
	                                    'PSZ' AS Abbucher, 
	                                    Lagerbewegungen_Artikel.Lager_von, 
	                                    0 AS Ausdr2,
	                                    Lagerbewegungen_Artikel.Rollennummer,
	                                    Lagerbewegungen_Artikel.[Gebucht von]
                                    FROM 
	                                    (Artikel INNER JOIN Lagerbewegungen_Artikel ON Artikel.[Artikel-Nr] = Lagerbewegungen_Artikel.[Artikel-nr]) 
	                                    INNER JOIN Lagerbewegungen ON Lagerbewegungen_Artikel.Lagerbewegungen_id = Lagerbewegungen.ID
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer) AND ((Lagerbewegungen.Typ)<>'Umbuchung' And (Lagerbewegungen.Typ)<>'Entnahme'))

                                    UNION ALL
                                    /* Q5 */
                                    /* INSERT INTO [PSZ_Artikelübersicht Ein_Aus] ( Artikelnummer, Typ, [Bestellung-Nr], Anzahl, Datum, Name1, Lagerplatz_von, Lagerplatz_nach ) */
                                    SELECT 
	                                    Artikel.Artikelnummer, 
	                                    Lagerbewegungen.Typ,   
	                                    0 AS Ausdr1, 
	                                    IIf([Typ]='Entnahme',[Anzahl]*(-1),[Anzahl]) AS Menge, 
	                                    Lagerbewegungen.Datum,
	                                    'PSZ' AS Abbucher,
	                                    Lagerbewegungen_Artikel.Lager_von, 
	                                    Lagerbewegungen_Artikel.Lager_nach,
	                                    Lagerbewegungen_Artikel.Rollennummer,
	                                    Lagerbewegungen_Artikel.[Gebucht von]
                                    FROM 
	                                    (Artikel INNER JOIN Lagerbewegungen_Artikel ON Artikel.[Artikel-Nr] = Lagerbewegungen_Artikel.[Artikel-nr]) 
	                                    INNER JOIN Lagerbewegungen ON Lagerbewegungen_Artikel.Lagerbewegungen_id = Lagerbewegungen.ID
                                    WHERE (((Artikel.Artikelnummer)=@Artikelnummer) AND ((Lagerbewegungen.Typ)<>'Zugang direkt' And (Lagerbewegungen.Typ)<>'Entnahme'))
                        ) AS TMP WHERE COALESCE(CONVERT(NCHAR(8),[Datum],112), '') LIKE '{date}%' AND COALESCE([Bestellung-Nr], '') LIKE '{orderNumber.SqlEscape()}%' AND COALESCE([Typ], '') LIKE '{type.SqlEscape()}%' AND COALESCE([Name1], '') LIKE '{name.SqlEscape()}%' AND COALESCE([Lagerplatz_von], '') LIKE '{lagerFrom.SqlEscape()}%' AND COALESCE([Lagerplatz_nach], '') LIKE '{lagerTo.SqlEscape()}%' AND COALESCE([Rollennummer], '') LIKE '{rollerNumber.SqlEscape()}%'";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", articleNumber);
					sqlCommand.CommandTimeout = 300;

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsFaStatus> GetFaStatus(int articleNr, string faStatus, string filter)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var clauses = new List<string>();
					string query = "SELECT "
						+ " a.[Artikel-Nr] AS [ArtikelNr],"
						+ " a.Artikelnummer As [Artikelnummer], "
						+ " a.[Bezeichnung 1] as [Bezeichnung], "
						+ " f.ID FaID, "
						+ " f.Fertigungsnummer as [Fertigungsnummer], "
						+ " f.Originalanzahl AS [FA Menge],"
						+ " f.Anzahl_erledigt AS Erledigt, "
						+ " f.Anzahl AS Offen, "
						+ " f.Termin_Fertigstellung AS Wunschtermin, "
						+ " f.Termin_Bestätigt1 AS	 Termin_Planung, "
						+ " f.Kennzeichen as [Kennzeichen], "
						+ " f.Bemerkung as [Bemerkung], "
						+ " f.CpVersion, "
						+ " f.BomVersion, "
						+ " f.KundenIndex, "
						+ " f.Kunden_Index_Datum KundenIndexDatum, "
						+ " f.Termin_Bestätigt2 AS Termin_werk,"
						+ " f.Kommisioniert_komplett,"
						+ " f.FA_Gestartet,"
						+ " f.Kommisioniert_teilweise"
					+ " FROM Fertigung f INNER JOIN Artikel a ON f.Artikel_Nr = a.[Artikel-Nr]";

					clauses.Add("((f.Termin_Bestätigt1) > getDate() - 365)");
					clauses.Add("((f.gebucht) = 1)");
					if(!string.IsNullOrWhiteSpace(filter))
					{
						clauses.Add($" ( f.[Fertigungsnummer] like '{filter}%' OR a.[Artikel-Nr] like '%{filter}%' OR  a.[Bezeichnung 1] like '%{filter}%' OR  f.[Bemerkung] like '%{filter}%' )");
					}
					if(!string.IsNullOrWhiteSpace(faStatus))
					{
						clauses.Add($" f.Kennzeichen='{faStatus}' ");
					}
					if(articleNr > 0)
					{
						clauses.Add($" a.[Artikel-Nr] = '{articleNr}' ");
					}

					if(clauses.Count > 0)
					{
						query += $"WHERE {string.Join(" AND ", clauses)}";
					}

					query += " ORDER BY f.Termin_Bestätigt1 asc; ";

					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsFaStatus(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsDeliveryList> GetDeliveryList(string searchTerm = "", int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT "
										+ "Artikel.[Artikel-Nr],"
										+ " Artikel.EAN, "
										+ " Artikel.Artikelnummer, "
										+ " Artikel.Ursprungsland, "
										+ " Artikel.Zolltarif_nr, "
										+ " Artikel.[Bezeichnung 1], "
										+ " Artikel.[Bezeichnung 2], "
										+ " adressen.Lieferantennummer, "
										+ " adressen.Name1, "
										+ " Artikel.[UL zertifiziert], "
										+ " Bestellnummern.[Bestell-Nr], "
										+ " Bestellnummern.Wiederbeschaffungszeitraum, "
										+ " Bestellnummern.Standardlieferant, "
										+ " Bestellnummern.Einkaufspreis, "
										+ " Bestellnummern.Verpackungseinheit, "
										+ " Bestellnummern.Mindestbestellmenge, "
										+ " Artikel.Zeichnungsnummer "
									+ " FROM"
										+ "(Artikel LEFT JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN(Lieferanten LEFT JOIN adressen ON Lieferanten.Nr = adressen.Nr) ON Bestellnummern.[Lieferanten-Nr] = Lieferanten.Nr";
					if(!string.IsNullOrWhiteSpace(searchTerm))
					{
						searchTerm = searchTerm.SqlEscape();
						query += ($" WHERE Artikel.EAN LIKE '%{searchTerm}%' OR Artikel.Artikelnummer LIKE '%{searchTerm}%' OR Artikel.Ursprungsland LIKE '%{searchTerm}%' OR Artikel.Zolltarif_nr LIKE '%{searchTerm}%'"
										+ $" OR Artikel.[Bezeichnung 1] LIKE '%{searchTerm}%' OR Artikel.[Bezeichnung 2] LIKE '%{searchTerm}%' OR adressen.Lieferantennummer LIKE '%{searchTerm}%' OR adressen.Name1 LIKE '%{searchTerm}%'"
										+ $" OR Artikel.[UL zertifiziert] LIKE '%{searchTerm}%' OR Bestellnummern.[Bestell-Nr] LIKE '%{searchTerm}%' OR Bestellnummern.Wiederbeschaffungszeitraum LIKE '%{searchTerm}%' OR Bestellnummern.Standardlieferant LIKE '%{searchTerm}%'"
										+ $" OR Bestellnummern.Einkaufspreis LIKE '%{searchTerm}%' OR Bestellnummern.Verpackungseinheit LIKE '%{searchTerm}%' OR Bestellnummern.Mindestbestellmenge LIKE '%{searchTerm}%' OR Artikel.Zeichnungsnummer LIKE '%{searchTerm}%'");
					}

					query += $" ORDER BY Artikelnummer OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsDeliveryList(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetDeliveryList_count(string searchTerm = "")
			{
				searchTerm = searchTerm.SqlEscape();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT COUNT(*) AS nb "
									+ " FROM (Artikel LEFT JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN(Lieferanten LEFT JOIN adressen ON Lieferanten.Nr = adressen.Nr) ON Bestellnummern.[Lieferanten-Nr] = Lieferanten.Nr";
					if(!string.IsNullOrWhiteSpace(searchTerm))
						query += ($" WHERE Artikel.EAN LIKE '%{searchTerm}%' OR Artikel.Artikelnummer LIKE '%{searchTerm}%' OR Artikel.Ursprungsland LIKE '%{searchTerm}%' OR Artikel.Zolltarif_nr LIKE '%{searchTerm}%'"
										+ $" OR Artikel.[Bezeichnung 1] LIKE '%{searchTerm}%' OR Artikel.[Bezeichnung 2] LIKE '%{searchTerm}%' OR adressen.Lieferantennummer LIKE '%{searchTerm}%' OR adressen.Name1 LIKE '%{searchTerm}%'"
										+ $" OR Artikel.[UL zertifiziert] LIKE '%{searchTerm}%' OR Bestellnummern.[Bestell-Nr] LIKE '%{searchTerm}%' OR Bestellnummern.Wiederbeschaffungszeitraum LIKE '%{searchTerm}%' OR Bestellnummern.Standardlieferant LIKE '%{searchTerm}%'"
										+ $" OR Bestellnummern.Einkaufspreis LIKE '%{searchTerm}%' OR Bestellnummern.Verpackungseinheit LIKE '%{searchTerm}%' OR Bestellnummern.Mindestbestellmenge LIKE '%{searchTerm}%' OR Artikel.Zeichnungsnummer LIKE '%{searchTerm}%'");
					var sqlCommand = new SqlCommand(query, sqlConnection);

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var val) ? val : 0;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Logistics_TransPot> GetTransPot(DateTime? fertigungDatum = null, string articleNumber = "Reparatur", string fertigungKennzeichen = "erledigt")
			{
				if(fertigungDatum == null)
					fertigungDatum = new DateTime(2010, 1, 1);

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
                                    IF OBJECT_ID('tempdb..#PSZ_TransferTunesien2') IS NOT NULL DROP TABLE #PSZ_TransferTunesien2;
                                    WITH [PSZ_Transfer Tunesien 1] AS (
                                    SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Originalanzahl AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, Sum(Stunden.Minuten) AS SummevonMinuten, Artikel.[Bezeichnung 1], Fertigung.Termin_Fertigstellung
                                    FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) INNER JOIN StundenCZ AS Stunden ON Fertigung.Fertigungsnummer = Stunden.Auftragsnummer
                                    GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Originalanzahl, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1], Fertigung.Termin_Fertigstellung, Fertigung.Kennzeichen
                                    HAVING (((Artikel.Artikelnummer)<>@articleNumber) AND ((Fertigung.Termin_Fertigstellung)>@fertigungDatum) AND ((Fertigung.Kennzeichen)=@fertigungKennzeichen))
                                    )
                                    SELECT [PSZ_Transfer Tunesien 1].Artikelnummer, [PSZ_Transfer Tunesien 1].Stundensatz, [PSZ_Transfer Tunesien 1].Produktionszeit, 
                                    Avg(ISNULL([Anzahl]*[Produktionszeit]/NULLIF([SummevonMinuten],0),0)) AS Produktivität, 
                                    Count([PSZ_Transfer Tunesien 1].Fertigungsnummer) AS AnzahlvonFertigungsnummer, [PSZ_Transfer Tunesien 1].[Bezeichnung 1]
                                    INTO #PSZ_TransferTunesien2
                                    FROM [PSZ_Transfer Tunesien 1]
                                    WHERE ISNULL([Anzahl]*[Produktionszeit]/NULLIF([SummevonMinuten], 0), 0)<2
                                    GROUP BY [PSZ_Transfer Tunesien 1].Artikelnummer, [PSZ_Transfer Tunesien 1].Stundensatz, [PSZ_Transfer Tunesien 1].Produktionszeit, [PSZ_Transfer Tunesien 1].[Bezeichnung 1];

                                    /* Q3 */
                                    SELECT [PSZ_Transfer Tunesien 2].Artikelnummer, [PSZ_Transfer Tunesien 2].[Bezeichnung 1], [PSZ_Transfer Tunesien 2].Stundensatz, [PSZ_Transfer Tunesien 2].Produktionszeit, [PSZ_Transfer Tunesien 2].Produktivität, [PSZ_Transfer Tunesien 2].AnzahlvonFertigungsnummer, [Stundensatz]*(3*[Produktivität]) AS Kennzahl
                                    FROM #PSZ_TransferTunesien2 AS [PSZ_Transfer Tunesien 2]
                                    WHERE ((([PSZ_Transfer Tunesien 2].Produktivität)<2) AND (([PSZ_Transfer Tunesien 2].AnzahlvonFertigungsnummer)>1))
                                    ORDER BY [Stundensatz]*(3*[Produktivität]);
                                    IF OBJECT_ID('tempdb..#PSZ_TransferTunesien2') IS NOT NULL DROP TABLE #PSZ_TransferTunesien2;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("articleNumber", articleNumber == null ? (object)DBNull.Value : articleNumber);
					sqlCommand.Parameters.AddWithValue("fertigungDatum", fertigungDatum == null ? (object)DBNull.Value : fertigungDatum);
					sqlCommand.Parameters.AddWithValue("fertigungKennzeichen", fertigungKennzeichen == null ? (object)DBNull.Value : fertigungKennzeichen);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Logistics_TransPot(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Logistics_TN_AL_Logistics> Get_TN_AL_Logistics(string site)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"IF OBJECT_ID('tempdb..#PSZTN_Logistiktabelle_01') IS NOT NULL DROP TABLE #PSZTN_Logistiktabelle_01;
                                        IF OBJECT_ID('tempdb..#PSZTN_Logistiktabelle_02') IS NOT NULL DROP TABLE #PSZTN_Logistiktabelle_02;
                                        /* Q1 */
                                        SELECT 
	                                        Artikel.Artikelnummer, 
	                                        Artikel.Exportgewicht AS [Exportgewicht Fertigprodukt], 
	                                        artikel_kalkulatorische_kosten.Betrag AS Arbeitskosten
	                                        INTO #PSZTN_Logistiktabelle_01
                                        FROM 
	                                        Artikel INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
                                        WHERE (((Artikel.Artikelnummer) Like ('%'+ @TN_oder_AL)) AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten') AND ((Artikel.Stückliste)=1));


                                        /* Q2 */
                                        SELECT 
	                                        #PSZTN_Logistiktabelle_01.Artikelnummer, 
	                                        #PSZTN_Logistiktabelle_01.[Exportgewicht Fertigprodukt] AS [Exportgewicht in gr], 
	                                        #PSZTN_Logistiktabelle_01.Arbeitskosten, 
	                                        --Stücklisten.Artikelnummer, 
	                                        Stücklisten.Anzahl, 
	                                        Bestellnummern.Einkaufspreis, 
	                                        Artikel.Zolltarif_nr, 
	                                        Artikel.[Bezeichnung 1], 
	                                        Artikel.[Bezeichnung 2], 
	                                        Artikel.Verpackungsart, 
	                                        Artikel.Verpackungsmenge
	                                        INTO #PSZTN_Logistiktabelle_02
                                        FROM 
	                                        ((#PSZTN_Logistiktabelle_01 INNER JOIN Artikel ON #PSZTN_Logistiktabelle_01.Artikelnummer = Artikel.Artikelnummer) INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) INNER JOIN Bestellnummern ON Stücklisten.[Artikel-Nr des Bauteils] = Bestellnummern.[Artikel-Nr]
                                        WHERE (((Bestellnummern.Standardlieferant)=1))
                                        ORDER BY #PSZTN_Logistiktabelle_01.Artikelnummer;

                                        /* Q3 */

                                        SELECT 
	                                        #PSZTN_Logistiktabelle_02.Artikelnummer AS Fertigprodukt, 
	                                        #PSZTN_Logistiktabelle_02.[Bezeichnung 1] AS Designation1, 
	                                        /* First(#PSZTN_Logistiktabelle_02.[Bezeichnung 2]) AS Designation2,  */
	                                        MAX(#PSZTN_Logistiktabelle_02.[Bezeichnung 2]) AS Designation2, 
	                                        #PSZTN_Logistiktabelle_02.Zolltarif_nr, 
	                                        #PSZTN_Logistiktabelle_02.[Exportgewicht in gr] AS [Exportgewicht Fertigprodukt in gr], 
	                                        Sum(COALESCE([Anzahl],0)*COALESCE([Einkaufspreis],0)) AS Materialkosten, 
	                                        #PSZTN_Logistiktabelle_02.Arbeitskosten, 
	                                        #PSZTN_Logistiktabelle_02.Verpackungsart, 
	                                        #PSZTN_Logistiktabelle_02.Verpackungsmenge
                                        FROM #PSZTN_Logistiktabelle_02
                                        GROUP BY #PSZTN_Logistiktabelle_02.Artikelnummer, #PSZTN_Logistiktabelle_02.[Bezeichnung 1], #PSZTN_Logistiktabelle_02.Zolltarif_nr, #PSZTN_Logistiktabelle_02.[Exportgewicht in gr], #PSZTN_Logistiktabelle_02.Arbeitskosten, #PSZTN_Logistiktabelle_02.Verpackungsart, #PSZTN_Logistiktabelle_02.Verpackungsmenge;
                                        
                                        IF OBJECT_ID('tempdb..#PSZTN_Logistiktabelle_01') IS NOT NULL DROP TABLE #PSZTN_Logistiktabelle_01;
                                        IF OBJECT_ID('tempdb..#PSZTN_Logistiktabelle_02') IS NOT NULL DROP TABLE #PSZTN_Logistiktabelle_02;
";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;
					sqlCommand.Parameters.AddWithValue("TN_oder_AL", site);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Logistics_TN_AL_Logistics(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsDeliveryOverview> GetDeliveryOverview()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT "
										+ " adressen.Name1, "
										+ " adressen.Lieferantennummer, "
										+ " Bestellnummern.Standardlieferant,"
										+ " Artikel.Artikelnummer, "
										+ " Artikel.[Bezeichnung 1], "
										+ " Artikel.[Bezeichnung 2], "
										+ " Bestellnummern.[Bestell-Nr], "
										+ " Bestellnummern.Einkaufspreis, "
										+ " Artikel.Zolltarif_nr, "
										+ " Artikel.Ursprungsland, "
										+ " CAST(Artikel.Größe AS DECIMAL(38,15)) AS [Nettogewicht in gr] "
									+ " FROM (adressen INNER JOIN Bestellnummern ON adressen.Nr = Bestellnummern.[Lieferanten-Nr]) INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr] "
									+ " WHERE (((adressen.Name1) <> 'PHD Elektrotechnik GmbH')) "
									+ " ORDER BY adressen.Name1, Artikel.Artikelnummer; ";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsDeliveryOverview(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsPreferences> GetPreferences(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT "
										+ " Zollabteilung_Preaferenzkalkulation.Artikelnummer, "
										+ " Zollabteilung_Preaferenzkalkulation.[Bezeichnung 1], "
										+ " Zollabteilung_Preaferenzkalkulation.Verkaufspreis_Preisgruppe_1, "
										+ " Zollabteilung_Preaferenzkalkulation.Position, "
										+ " Zollabteilung_Preaferenzkalkulation.Artikelnummer_stückliste, "
										+ " Zollabteilung_Preaferenzkalkulation.[Bezeichnung des Bauteils], "
										+ " Zollabteilung_Preaferenzkalkulation.Name1, "
										+ " Zollabteilung_Preaferenzkalkulation.Anzahl, "
										+ " Zollabteilung_Preaferenzkalkulation.Verkaufspreis, "
										+ " Zollabteilung_Preaferenzkalkulation.Standardlieferant, "
										+ " IIf([Praeferenz_Aktuelles_jahr] Is Null,'NEIN',[Praeferenz_Aktuelles_jahr]) AS Praeferenz,"
										+ " Zollabteilung_Preaferenzkalkulation.Ursprungsland, "
										+ " Round([SummeEK],2) AS SummeEK1,"
										+ " IIf([Praeferenz_Aktuelles_jahr]='JA', [SummeEK],0) AS WennK,"
										+ " Zollabteilung_Preaferenzkalkulation.SummevonBetrag"
									+ " FROM Zollabteilung_Preaferenzkalkulation"
									+ " WHERE (((Zollabteilung_Preaferenzkalkulation.Artikelnummer)= @ArtikelNummer));";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("ArtikelNummer", articleNumber);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsPreferences(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> GetArticleLogistics(List<string> articleNumberEndings, string articleNumber, bool? hasProductionPlace, bool activeOnly, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100)
			{
				articleNumber = articleNumber ?? "";
				bool isFirstClause = true;
				if(pageSize <= 0)
					pageSize = 1;
				if(string.IsNullOrWhiteSpace(sortColumn))
					sortColumn = "Artikelnummer";

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"SELECT * FROM Artikel";
					if(activeOnly)
					{
						query += $" WHERE [aktiv] = 1";
						isFirstClause = false;
					}
					if(!string.IsNullOrWhiteSpace(articleNumber))
					{
						if(isFirstClause)
						{
							query += $" WHERE [artikelnummer] LIKE '{articleNumber.SqlEscape()}%'";
						}
						else
						{
							query += $" AND [artikelnummer] LIKE '{articleNumber.SqlEscape()}%'";
						}
					}
					if(hasProductionPlace.HasValue)
					{
						if(isFirstClause)
						{
							query += $" WHERE [Artikel-nr] IN (Select ArticleId From [__BSD_ArtikelProductionExtension] WHERE ProductionPlace1_Id {(hasProductionPlace.Value ? "IS NOT NULL" : "IS NULL")})";
							isFirstClause = false;
						}
						else
						{
							query += $" AND [Artikel-nr] IN (Select ArticleId From [__BSD_ArtikelProductionExtension] WHERE ProductionPlace1_Id {(hasProductionPlace.Value ? "IS NOT NULL" : "IS NULL")})";
						}
					}
					if(articleNumberEndings != null && articleNumberEndings.Count > 0)
					{
						if(isFirstClause)
						{
							query += $" WHERE ({(string.Join(" OR ", articleNumberEndings.Select(x => $"[artikelnummer] LIKE '%{(x ?? "").Trim().SqlEscape()}'")))})";
						}
						else
						{
							query += $" AND ({(string.Join(" OR ", articleNumberEndings.Select(x => $"[artikelnummer] LIKE '%{(x ?? "").Trim().SqlEscape()}'")))})";
						}
					}

					query += $" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 660;
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
				}
			}
			public async static Task<int> GetArticleLogistics_Count(List<string> articleNumberEndings, string articleNumber, bool? hasProductionPlace, bool activeOnly)
			{
				bool isFirstClause = true;
				var dataTable = new DataTable();
				return await Task<int>.Factory.StartNew(() =>
				{
					using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
					{
						sqlConnection.Open();
						string query = $"SELECT COUNT(*) As Nb FROM Artikel";
						if(activeOnly)
						{
							query += $" WHERE [aktiv] = 1";
							isFirstClause = false;
						}
						if(!string.IsNullOrWhiteSpace(articleNumber))
						{
							if(isFirstClause)
							{
								query += $" WHERE [artikelnummer] LIKE '{articleNumber.SqlEscape()}%'";
							}
							else
							{
								query += $" AND [artikelnummer] LIKE '{articleNumber.SqlEscape()}%'";
							}
						}
						if(hasProductionPlace.HasValue)
						{
							if(isFirstClause)
							{
								query += $" WHERE [Artikel-nr] IN (Select ArticleId From [__BSD_ArtikelProductionExtension] WHERE ProductionPlace1_Id {(hasProductionPlace.Value ? "IS NOT NULL" : "IS NULL")})";
								isFirstClause = false;
							}
							else
							{
								query += $" AND [Artikel-nr] IN (Select ArticleId From [__BSD_ArtikelProductionExtension] WHERE ProductionPlace1_Id {(hasProductionPlace.Value ? "IS NOT NULL" : "IS NULL")})";
							}
						}
						if(articleNumberEndings != null && articleNumberEndings.Count > 0)
						{
							if(isFirstClause)
							{
								query += $" WHERE  ({(string.Join(" OR ", articleNumberEndings.Select(x => $"[artikelnummer] LIKE '%{(x ?? "").Trim().SqlEscape()}'")))})";
							}
							else
							{
								query += $" AND ({(string.Join(" OR ", articleNumberEndings.Select(x => $"[artikelnummer] LIKE '%{(x ?? "").Trim().SqlEscape()}'")))})";
							}
						}

						var sqlCommand = new SqlCommand(query, sqlConnection);

						return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
					}
				});
			}
		}
		public class Controlling
		{
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingDB> GetDB(int articleNr)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT 
	                                    Artikel.Artikelnummer ArtikelnummerOriginal, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2], 
	                                    Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], 
	                                    Stücklisten.Anzahl, Preisgruppen.Verkaufspreis, Stücklisten.Anzahl*Preisgruppen.Verkaufspreis AS Summe, 
	                                    Sum(artikel_kalkulatorische_kosten.Betrag) AS SummevonBetrag, Preisgruppen.[Artikel-Nr], Preisgruppen_1.Verkaufspreis Verkaufspreis_1, Preisgruppen_1.Verkaufspreis + ROUND(IIF(Artikel.[VK-Festpreis] = 0, IIF(Artikel.[DEL] = 0, 0, (((Artikel.[DEL] * 1.01)) - 150) / 100) * Artikel.[Cu-Gewicht], 0), 2) AS VK_PSZ_ink_Kupfer,
	                                    Preisgruppen_1.Preisgruppe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, 
	                                    IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0) AS Kupferzuschlag, 
	                                    Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)) AS SummeEK, 
	                                    Artikel_1.DEL, Artikel_1.Kupferbasis, Artikel_1.Kupferzahl, adressen.Name1, Artikel_1.Gewicht, 
	                                    Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1) AS SummeEKohneCU
                                    FROM ((((((Stücklisten RIGHT JOIN Artikel ON Stücklisten.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) LEFT JOIN Preisgruppen ON Stücklisten.[Artikel-Nr des Bauteils] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Preisgruppen AS Preisgruppen_1 ON Artikel.[Artikel-Nr] = Preisgruppen_1.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Preisgruppen.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
                                    GROUP BY Artikel.DEL,Artikel.[VK-Festpreis],Artikel.[Cu-Gewicht],
									Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2], Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl, Preisgruppen.Verkaufspreis, Stücklisten.Anzahl*Preisgruppen.Verkaufspreis, Preisgruppen.[Artikel-Nr], Preisgruppen_1.Verkaufspreis, Preisgruppen_1.Preisgruppe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0), Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1)+(IIf(Artikel_1.Kupferzahl<>0,(Artikel_1.Kupferzahl*((Artikel_1.DEL*1.01-Artikel_1.Kupferbasis)/100))/1000*[Anzahl],0)), Artikel_1.DEL, Artikel_1.Kupferbasis, Artikel_1.Kupferzahl, adressen.Name1, Artikel_1.Gewicht, Stücklisten.Anzahl*Bestellnummern.Einkaufspreis*(Artikel_1.Gewicht/100+1), Stücklisten.[Artikel-Nr], Stücklisten.Variante
                                    HAVING (((Preisgruppen_1.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)=1) AND ((Stücklisten.[Artikel-Nr])=@articleNr) AND ((Stücklisten.Variante)='0'));";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("articleNr", articleNr);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingDB(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingHistory> GetHistory(int articleId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT TOP 1000 * from PSZ_Artikelhistorie where[Artikel-Nr]=@artikelnr";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnr", articleId);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingHistory(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingArtikelEntity GetArticle(int articleId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT TOP 1 * FROM Artikel where [Artikel-Nr]=@artikelnr";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnr", articleId);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingArtikelEntity(dataTable.Rows[0]);
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingBomReport> GetBomReport(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"SELECT "
									+ " Stücklisten.Position AS Pos, "
									+ " Stücklisten.Artikelnummer, "
									+ "Stücklisten.[Artikel-Nr],"
									+ " Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], "
									+ " adressen.Name1 AS Lieferant, Bestellnummern.[Bestell-Nr]"
								+ " FROM (((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr])"
									+ " LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) "
									+ " LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) "
									+ " LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr"
								+ " WHERE (((Artikel.Artikelnummer) = @artikelnummer) AND ((Bestellnummern.Standardlieferant) = 1) AND ((Stücklisten.Variante) = '0'))"
								+ " ORDER BY Stücklisten.Position;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingBomReport(x)).ToList();
				}
				else
				{
					return null;
				}
			}
		}
		public class ControllingAnalysis
		{
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKIncludingCopper_OLD> GetVKIncludingCopper_OLD(string articleNumber, DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - Q1
					string query_1 = $@"DROP TABLE IF EXISTS [PSZ_AV VK Analyse];
                                        SELECT
                                            Artikel.EdiDefault,
                                            Artikel.UBG,
                                            Artikel.Artikelnummer,
                                            Artikel.[Bezeichnung 1],
                                            Artikel.[Bezeichnung 2],
                                            Artikel.[Bezeichnung 3],
                                            Artikel.Index_Kunde,
                                            Artikel.Index_Kunde_Datum,
                                            CAST(999.999 AS float) AS Materialkosten,
                                            CAST(Artikel.Produktionszeit AS float) AS Produktionszeit,
                                            CAST(Artikel.Stundensatz AS float) AS Stundensatz,
                                            Preisgruppen.Verkaufspreis,
                                            Artikel.DEL,
                                            Artikel.[DEL fixiert],
                                            CAST(Artikel.[Cu-Gewicht] AS float) AS [Cu-Gewicht],
                                            Artikel.[VK-Festpreis],
                                            (Round(IIf(Artikel.[VK-Festpreis]=0,((COALESCE(Artikel.DEL,0)*1.01) - COALESCE(Artikel.Kupferbasis,0))/100* COALESCE(Artikel.[Cu-Gewicht],0),0),2))*COALESCE([Preiseinheit],0) AS Kupferzuschlag,
                                            (Round(IIf(Artikel.[VK-Festpreis] = 0, ((COALESCE(Artikel.DEL, 0) * 1.01) - COALESCE(Artikel.Kupferbasis, 0)) / 100 * COALESCE(Artikel.[Cu-Gewicht], 0), 0), 2)) * COALESCE([Preiseinheit], 0) + COALESCE(Preisgruppen.[Verkaufspreis], 0) AS[VK inkl Kupfer],
                                            Artikel.Preiseinheit,
                                            CAST(Artikel.Größe AS float) AS  [Gewicht in gr],
                                            Artikel.Ursprungsland,
                                            Artikel.Zolltarif_nr,
                                            Artikel.Freigabestatus,
                                            0 AS Jahresmenge,
                                            0 AS Jahresumsatz,
                                            Preisgruppen.Staffelpreis1,
                                            Preisgruppen.ME1,
                                            Preisgruppen.Staffelpreis2,
                                            Preisgruppen.ME2,
                                            Preisgruppen.Staffelpreis3,
                                            Preisgruppen.ME3,
                                            Preisgruppen.Staffelpreis4,
                                            Preisgruppen.ME4,
											ISNULL(e.Verkaufspreis,0) AS ErstmusterVKpreis,
											(Round(IIf(Artikel.[VK-Festpreis] = 0, ((COALESCE(Artikel.DEL, 0) * 1.01) - COALESCE(Artikel.Kupferbasis, 0)) / 100 * COALESCE(Artikel.[Cu-Gewicht], 0), 0), 2)) * COALESCE([Preiseinheit], 0) + COALESCE(e.[Verkaufspreis], 0) AS [ErstmusterVKpreis Inkl Kupfer],
                                            Artikel.Hubmastleitungen,
                                            Artikel.Sysmonummer,
	                                        Artikel.Losgroesse,
	                                        Artikel.Verpackungsmenge,
	                                        Artikel.Warengruppe,
	                                        V.[Kalkulatorische kosten],
	                                        V.[DB I mit],
	                                        V.[Marge mit CU],
											Artikel.ProductionLotSize,
											Artikel.Artikelfamilie_Kunde_Detail1, Artikel.Artikelfamilie_Kunde_Detail2, Artikel.Artikelfamilie_Kunde
                                        INTO [PSZ_AV VK Analyse]
                                        FROM Artikel INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
										Left join (Select distinct ArticleNr, Verkaufspreis from __BSD_ArtikelSalesExtension where ArticleSalesTypeId=0) e on e.ArticleNr=Artikel.[Artikel-Nr]
                                        Left Join [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel] V ON V.Artikelnummer=Artikel.Artikelnummer
                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber.SqlEscape() ?? ""}%')) AND ((Artikel.aktiv) = 1) AND ((Artikel.Stückliste) = 1))
                                        ORDER BY Artikel.Artikelnummer;";

					// - Q2
					string query_2 = $@"DROP TABLE IF EXISTS [PSZ_AV VK Analyse Tabelle mit Jahresmengen];
                                        SELECT 
                                            [PSZ_AV VK Analyse].Artikelnummer, 
                                            Angebote.Typ, 
                                            Sum([angebotene Artikel].Anzahl) AS SummevonAnzahl, 
                                            Sum([Anzahl]*[Einzelpreis]) AS Umsatz, 
                                            Artikel.Sysmonummer 
                                        INTO [PSZ_AV VK Analyse Tabelle mit Jahresmengen]
                                        FROM ([PSZ_AV VK Analyse] INNER JOIN (Artikel INNER JOIN [angebotene Artikel] ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) ON [PSZ_AV VK Analyse].Artikelnummer = Artikel.Artikelnummer) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
                                        WHERE (((Angebote.Datum)>='{dateFrom.ToString("yyyyMMdd")}' And (Angebote.Datum)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY [PSZ_AV VK Analyse].Artikelnummer, Angebote.Typ, Artikel.Sysmonummer
                                        HAVING (((Angebote.Typ)='Rechnung'));";

					// - Q3
					string query_3 = $@"UPDATE [PSZ_AV VK Analyse]
                                        SET 
                                            [PSZ_AV VK Analyse].Jahresmenge = ISNULL([PSZ_AV VK Analyse Tabelle mit Jahresmengen].SummevonAnzahl, 0), 
                                            [PSZ_AV VK Analyse].Jahresumsatz = ISNULL([PSZ_AV VK Analyse Tabelle mit Jahresmengen].Umsatz, 0)
                                        FROM [PSZ_AV VK Analyse] INNER JOIN [PSZ_AV VK Analyse Tabelle mit Jahresmengen] ON [PSZ_AV VK Analyse].Artikelnummer = [PSZ_AV VK Analyse Tabelle mit Jahresmengen].Artikelnummer ";

					// - Q4  // What's the point?
					string query_4 = $@"SELECT [PSZ_AV VK Analyse].* FROM [PSZ_AV VK Analyse];";

					// - Q5
					string query_5 = $@"DROP TABLE IF EXISTS [PSZ_AV VK Analyse MK Hilfstabelle];
                                        SELECT 
                                            [PSZ_AV VK Analyse].Artikelnummer, 
                                            Sum(COALESCE(Stücklisten.Anzahl,0)*COALESCE(Bestellnummern.Einkaufspreis,0)*((COALESCE(Artikel_1.Gewicht,0)/100)+1)) AS Materialpreis 
                                        INTO [PSZ_AV VK Analyse MK Hilfstabelle]
                                        FROM ((([PSZ_AV VK Analyse] INNER JOIN Artikel ON [PSZ_AV VK Analyse].Artikelnummer = Artikel.Artikelnummer) INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) INNER JOIN Bestellnummern ON Stücklisten.[Artikel-Nr des Bauteils] = Bestellnummern.[Artikel-Nr]) INNER JOIN Artikel AS Artikel_1 ON Bestellnummern.[Artikel-Nr] = Artikel_1.[Artikel-Nr]
                                        WHERE (((Bestellnummern.Standardlieferant)=1))
                                        GROUP BY [PSZ_AV VK Analyse].Artikelnummer;";

					// - Q6
					string query_6 = $@"UPDATE [PSZ_AV VK Analyse]
                                        SET [PSZ_AV VK Analyse].Materialkosten = [PSZ_AV VK Analyse MK Hilfstabelle].Materialpreis
                                        FROM [PSZ_AV VK Analyse] INNER JOIN [PSZ_AV VK Analyse MK Hilfstabelle] ON [PSZ_AV VK Analyse].Artikelnummer = [PSZ_AV VK Analyse MK Hilfstabelle].Artikelnummer;";

					// - Q7
					string query_7 = $@"SELECT [PSZ_AV VK Analyse].* FROM [PSZ_AV VK Analyse];";

					// -
					string query = $"{query_1}{Environment.NewLine}{Environment.NewLine}" +
										$"{query_2}{Environment.NewLine}{Environment.NewLine}" +
										$"{query_3}{Environment.NewLine}{Environment.NewLine}" +
										//$"{query_4}{Environment.NewLine}{Environment.NewLine}" +
										$"{query_5}{Environment.NewLine}{Environment.NewLine}" +
										$"{query_6}{Environment.NewLine}{Environment.NewLine}" +
										$"{query_7}{Environment.NewLine}";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKIncludingCopper_OLD(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKIncludingCopper> GetVKIncludingCopper(string articleNumber, DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - 
					string query = $@" /* -- Main analysis CTE with calculated VK inkl. Kupfer and others */
											WITH VK_Analyse_Base AS (
												SELECT
													a.[Artikel-Nr],
													a.EdiDefault,
													a.UBG,
													a.Artikelnummer,
													a.[Bezeichnung 1],
													a.[Bezeichnung 2],
													a.[Bezeichnung 3],
													a.Index_Kunde,
													a.Index_Kunde_Datum,
													CAST(a.Produktionszeit AS MONEY) AS Produktionszeit,
													CAST(a.Stundensatz AS MONEY) AS Stundensatz,
													a.DEL,
													a.[DEL fixiert],
													CAST(a.[Cu-Gewicht] AS MONEY) AS [Cu-Gewicht],
													a.[VK-Festpreis],
													COALESCE(a.Preiseinheit, 0) AS Preiseinheit,
													CAST(a.Größe AS MONEY) AS [Gewicht in gr],
													a.Ursprungsland,
													a.Zolltarif_nr,
													a.Freigabestatus,
													a.Hubmastleitungen,
													a.Sysmonummer,
													a.Verpackungsmenge,
													a.Warengruppe,
													a.Artikelfamilie_Kunde_Detail1,
													a.Artikelfamilie_Kunde_Detail2,
													a.Artikelfamilie_Kunde,
													p.Verkaufspreis,
													a.ProductionLotSize,
													v.[Kalkulatorische kosten],
													v.[DB I mit],
													v.[Marge mit CU],

													-- Calculated fields
													ROUND(IIF(a.[VK-Festpreis] = 0, ((COALESCE(a.DEL, 0) * 1.01 - COALESCE(a.Kupferbasis, 0)) / 100) * COALESCE(a.[Cu-Gewicht], 0), 0), 2) * COALESCE(a.Preiseinheit, 0) AS Kupferzuschlag,
													ROUND(IIF(a.[VK-Festpreis] = 0, ((COALESCE(a.DEL, 0) * 1.01 - COALESCE(a.Kupferbasis, 0)) / 100) * COALESCE(a.[Cu-Gewicht], 0), 0), 2) * COALESCE(a.Preiseinheit, 0) + COALESCE(p.Verkaufspreis, 0) AS [VK inkl Kupfer]

												FROM Artikel a
												INNER JOIN Preisgruppen p ON a.[Artikel-Nr] = p.[Artikel-Nr]
												LEFT JOIN [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel] v ON v.Artikelnummer = a.Artikelnummer
												WHERE a.Artikelnummer LIKE @article AND a.aktiv = 1 AND a.Stückliste = 1
											),

											vk_preise AS (
												SELECT [Artikel-Nr], PriceTypeId, PriceType, Price, BisMenge, LotSize, 
												ProductionTime, HourlyRate, CAST(ProductionCosts AS MONEY) AS ProductionCosts FROM (
												SELECT b.[Artikel-Nr], e.ArticleSalesType AS PriceTypeId, e.ArticleSalesType AS PriceType, e.Verkaufspreis AS Price, NULL AS BisMenge, e.Losgroesse LotSize, 
												e.Profuktionszeit ProductionTime, e.Stundensatz AS HourlyRate, e.Produktionskosten ProductionCosts
												FROM VK_Analyse_Base b
												JOIN __BSD_ArtikelSalesExtension e ON e.ArticleNr = b.[Artikel-Nr]

												UNION ALL
												SELECT p.[Artikel-Nr], 'Serie1', 'S1', Staffelpreis1, ME1, k.LotSize, k.ProduKtionzeit, k.Stundensatz, k.Betrag
												FROM VK_Analyse_Base b  
												JOIN Preisgruppen p on b.[Artikel-Nr]=p.[Artikel-Nr]
												JOIN [Staffelpreis_Konditionzuordnung] k ON k.Artikel_Nr=p.[Artikel-Nr]
												WHERE  Staffelpreis1 IS NOT NULL AND k.TypeId=1 /*staffel 1*/

												UNION ALL
												SELECT p.[Artikel-Nr], 'Serie2', 'S2', Staffelpreis2, ME2, k.LotSize, k.ProduKtionzeit, k.Stundensatz, k.Betrag
												FROM VK_Analyse_Base b  
												JOIN Preisgruppen p on b.[Artikel-Nr]=p.[Artikel-Nr]
												JOIN [Staffelpreis_Konditionzuordnung] k ON k.Artikel_Nr=p.[Artikel-Nr]
												WHERE  Staffelpreis2 IS NOT NULL AND k.TypeId=2 /*staffel 2*/

												UNION ALL
												SELECT p.[Artikel-Nr], 'Serie3','S3', Staffelpreis3, ME3, k.LotSize, k.ProduKtionzeit, k.Stundensatz, k.Betrag
												FROM VK_Analyse_Base b  
												JOIN Preisgruppen p on b.[Artikel-Nr]=p.[Artikel-Nr]
												JOIN [Staffelpreis_Konditionzuordnung] k ON k.Artikel_Nr=p.[Artikel-Nr]
												WHERE  Staffelpreis3 IS NOT NULL AND k.TypeId=3 /*staffel 3*/

												UNION ALL
												SELECT p.[Artikel-Nr], 'Serie4', 'S4', Staffelpreis4, ME4, k.LotSize, k.ProduKtionzeit, k.Stundensatz, k.Betrag
												FROM VK_Analyse_Base b  
												JOIN Preisgruppen p on b.[Artikel-Nr]=p.[Artikel-Nr]
												JOIN [Staffelpreis_Konditionzuordnung] k ON k.Artikel_Nr=p.[Artikel-Nr]
												WHERE  Staffelpreis4 IS NOT NULL AND k.TypeId=4 /*staffel 4*/) TMP
											),


											/* -- Jahresmengen (Rechnungen) for selected Zeitraum */
											Jahresmengen AS (
												SELECT 
													a.Artikelnummer,
													SUM(aa.Anzahl) AS Jahresmenge,
													SUM(aa.Anzahl * aa.Einzelpreis) AS Jahresumsatz
												FROM [angebotene Artikel] aa
												INNER JOIN Angebote ag ON ag.Nr = aa.[Angebot-Nr]
												INNER JOIN Artikel a ON a.[Artikel-Nr] = aa.[Artikel-Nr]
												INNER JOIN VK_Analyse_Base b ON b.[Artikel-Nr]=aa.[Artikel-Nr]
												WHERE ag.Typ = 'Rechnung' AND ag.Datum BETWEEN @dateMin AND @dateMax
												GROUP BY a.Artikelnummer
											),

											
										/* -- Materialkosten berechnung*/
										Materialkosten AS (
											SELECT 
												a.Artikelnummer,
												SUM(ISNULL(s.Anzahl, 0) * ISNULL(b.Einkaufspreis, 0) * ((ISNULL(a1.Gewicht, 0) / 100) + 1)) AS Materialpreis
											FROM Artikel a
											INNER JOIN Stücklisten s ON a.[Artikel-Nr] = s.[Artikel-Nr]
											INNER JOIN Bestellnummern b ON s.[Artikel-Nr des Bauteils] = b.[Artikel-Nr]
											INNER JOIN Artikel a1 ON b.[Artikel-Nr] = a1.[Artikel-Nr]
											WHERE b.Standardlieferant = 1
											GROUP BY a.Artikelnummer
										)

										/* -- Final SELECT */
										SELECT
											v.*,
											v.Kupferzuschlag + ISNULL(vk.Price,0) AS PriceInclCu, 
											vk.PriceType, vk.Price, vk.BisMenge, vk.LotSize, 
											vk.ProductionTime, vk.HourlyRate, vk.ProductionCosts,
											ISNULL(j.Jahresmenge, 0) AS Jahresmenge,
											ISNULL(j.Jahresumsatz, 0) AS Jahresumsatz,
											ISNULL(m.Materialpreis, 999.999) AS Materialkosten,
											v.Kupferzuschlag + ISNULL(m.Materialpreis, 999.999) AS MaterialkostenMitCu,
											ISNULL(vk.Price,0) - (ISNULL(vk.ProductionCosts,0) + ISNULL(m.Materialpreis, 999.999)) DB,
											v.Kupferzuschlag + ISNULL(vk.Price,0) - (ISNULL(vk.ProductionCosts,0) + ISNULL(m.Materialpreis, 999.999) + v.Kupferzuschlag) DBMitCu,
											100.0 *
											(
												ISNULL(vk.Price,0)
												- (ISNULL(vk.ProductionCosts,0) + ISNULL(m.Materialpreis, 999.999))
											)
											/ NULLIF(
												ISNULL(vk.ProductionCosts,0) + ISNULL(m.Materialpreis, 999.999),
												0
											) AS Marge,

											100.0 *
											(
												v.Kupferzuschlag + ISNULL(vk.Price,0)
												- (ISNULL(vk.ProductionCosts,0) + ISNULL(m.Materialpreis, 999.999) + v.Kupferzuschlag)
											)
											/ NULLIF(
												ISNULL(vk.ProductionCosts,0) + ISNULL(m.Materialpreis, 999.999) + v.Kupferzuschlag,
												0
											) AS MargeMitCu
										FROM VK_Analyse_Base v
										LEFT JOIN vk_preise vk ON vk.[Artikel-Nr]=v.[Artikel-Nr]
										LEFT JOIN Jahresmengen j ON j.Artikelnummer = v.Artikelnummer
										LEFT JOIN Materialkosten m ON m.Artikelnummer = v.Artikelnummer
										ORDER BY v.Artikelnummer, vk.PriceTypeId;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("article", $"{(articleNumber ?? "").SqlEscape().Trim()}%");
					sqlCommand.Parameters.AddWithValue("dateMin", dateFrom);
					sqlCommand.Parameters.AddWithValue("dateMax", dateTill);
					sqlCommand.CommandTimeout = 300;
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKIncludingCopper(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static IEnumerable<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ArticleMargeEntity> GetArticleMarge(int articleNr)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"SELECT * FROM ufn_article_marge(@articleNr)";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ArticleMargeEntity(x));
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_AverageMaterialContent> GetAverageMaterialContent(string customerNr, decimal surcharge, DateTime dateFrom, DateTime dateTill)
			{
				if(string.IsNullOrWhiteSpace(customerNr))
					return null;

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					#region >>> Queries chunks <<<
					// - Q0
					string query_0 = $@"
                                        declare @MittelwertvonGesamtnettoumsatz decimal(20,4);
                                        declare @MittelwertvonMaterialanteil_ungewichtet decimal(20,4);
                                        /* DROP temp tables */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Lieferübersicht') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Lieferübersicht; /* Q1 */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Rohmaterial') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Rohmaterial; /* Q2 */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Rohmaterialpreise') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Rohmaterialpreise; /* Q5 */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Rohmaterialpreise_Mittelwerte') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Rohmaterialpreise_Mittelwerte; /* Q8 */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_UBG2_Kosten') IS NOT NULL DROP TABLE #PSZ_Materialanteil_UBG2_Kosten; /* Q11 */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_UBG1_Kosten') IS NOT NULL DROP TABLE #PSZ_Materialanteil_UBG1_Kosten; /* Q14 */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_HBG_Kosten') IS NOT NULL DROP TABLE #PSZ_Materialanteil_HBG_Kosten; /* Q17 */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Auswertung') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Auswertung; /* Q20 */
                                        IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Mittelwerte_Auswertung') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Mittelwerte_Auswertung; /* Q21 */";

					// - Q1 
					string query_1 = $@"SELECT 
                                            [Statistiken Angebote].[Adreß-Nr], 
                                            [Statistiken Angebote].[Artikel-Nr], 
                                            Sum(IIf([Typ]='Rechnung',[Anzahl],-1*[Anzahl])) AS Liefermenge, 
                                            Sum(IIf([Statistiken Angebote].Typ='Rechnung',
                                            [Statistiken Angebote].Gesamtpreis,-1*[Statistiken Angebote].Gesamtpreis)) AS Netto_gesamt, 
                                            Artikel.Artikelnummer --INTO [PSZ_Materialanteil Lieferübersicht]
                                            INTO #PSZ_Materialanteil_Lieferübersicht
                                        FROM 
                                            [Statistiken Angebote] INNER JOIN Artikel ON [Statistiken Angebote].[Artikel-Nr] = Artikel.[Artikel-Nr]
                                        WHERE 
                                            ((([Statistiken Angebote].Typ)='Rechnung' Or ([Statistiken Angebote].Typ)='gutschrift') AND (([Statistiken Angebote].Datum)>=@dateFrom 
                                            And ([Statistiken Angebote].Datum)<=@dateTill))
                                        GROUP BY [Statistiken Angebote].[Adreß-Nr], [Statistiken Angebote].[Artikel-Nr], Artikel.Artikelnummer
                                        HAVING ((([Statistiken Angebote].[Adreß-Nr])=@customerNumber ) 
                                            AND (([Statistiken Angebote].[Artikel-Nr])<>223 And ([Statistiken Angebote].[Artikel-Nr])<>232 
                                            And ([Statistiken Angebote].[Artikel-Nr])<>101 And ([Statistiken Angebote].[Artikel-Nr])<>222));";

					// - Q2 
					string query_2 = $@"SELECT 
                                            [PSZ_Materialanteil Lieferübersicht].[Artikel-Nr], 
                                            CAST('HBG' AS NVARCHAR(50)) AS Stufe, 
                                            Artikel.Artikelnummer, 
                                            Artikel.[Bezeichnung 1], 
                                            IIf(Artikel.Stückliste=1,Stücklisten.Anzahl,1) AS Anzahl, 
                                            IIf(Artikel.Stückliste=1,Artikel_1.Artikelnummer,Artikel.Artikelnummer) AS Artikel_1_Artikelnummer, 
                                            Artikel_1.Stückliste, 
                                            IIf(Artikel_1.Stückliste=1,'Ja','Nein') AS Löschen 
                                            INTO #PSZ_Materialanteil_Rohmaterial
                                        FROM (
                                            (#PSZ_Materialanteil_Lieferübersicht AS [PSZ_Materialanteil Lieferübersicht] 
                                                LEFT JOIN Artikel ON [PSZ_Materialanteil Lieferübersicht].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
                                                LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
                                                LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]
                                        WHERE 
                                            ((([PSZ_Materialanteil Lieferübersicht].[Artikel-Nr])<>223 
                                                And ([PSZ_Materialanteil Lieferübersicht].[Artikel-Nr])<>232 
                                                And ([PSZ_Materialanteil Lieferübersicht].[Artikel-Nr])<>101 
                                                And ([PSZ_Materialanteil Lieferübersicht].[Artikel-Nr])<>222));";

					// - Q3 
					string query_3 = $@"INSERT INTO #PSZ_Materialanteil_Rohmaterial ([Artikel-Nr], Stufe, Artikelnummer, [Bezeichnung 1], Anzahl, Artikel_1_Artikelnummer, Stückliste, Löschen )
                                        SELECT 
                                            Artikel.[Artikel-Nr], 
                                            'UBG1' AS Stufe, 
                                            Artikel.Artikelnummer, 
                                            Artikel.[Bezeichnung 1], 
                                            Stücklisten.Anzahl, 
                                            Artikel_1.Artikelnummer AS Artikel_1_Artikelnummer, 
                                            Artikel_1.Stückliste, 
                                            'Nein' AS Löschen
                                        FROM 
                                            #PSZ_Materialanteil_Rohmaterial AS [PSZ_Materialanteil Rohmaterial] 
                                                INNER JOIN ((Artikel INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
                                                INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) ON [PSZ_Materialanteil Rohmaterial].Artikel_1_Artikelnummer = Artikel.Artikelnummer
                                        WHERE ((([PSZ_Materialanteil Rohmaterial].Stückliste)=1));";

					// - Q4 
					string query_4 = $@"INSERT INTO #PSZ_Materialanteil_Rohmaterial ( [Artikel-Nr], Stufe, Artikelnummer, [Bezeichnung 1], Anzahl, Artikel_1_Artikelnummer, Stückliste, Löschen )
                                        SELECT 
                                            Artikel.[Artikel-Nr], 
                                            'UBG2' AS Ausdr1, 
                                            Artikel.Artikelnummer, 
                                            Artikel.[Bezeichnung 1], 
                                            Stücklisten.Anzahl, 
                                            Artikel_1.Artikelnummer, 
                                            Artikel_1.Stückliste, 
                                            'Nein' AS Ausdr2
                                        FROM 
                                            #PSZ_Materialanteil_Rohmaterial AS [PSZ_Materialanteil Rohmaterial] 
                                                INNER JOIN ((Artikel INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
                                                INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) ON [PSZ_Materialanteil Rohmaterial].Artikel_1_Artikelnummer = Artikel.Artikelnummer
                                        WHERE ((([PSZ_Materialanteil Rohmaterial].Stückliste)=1) AND (([PSZ_Materialanteil Rohmaterial].Löschen)='Nein'));";

					// - Q5 
					string query_5 = $@"IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Rohmaterialpreise') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Rohmaterialpreise;";

					// - Q6 
					string query_6 = $@"SELECT 
                                            [PSZ_Materialanteil Rohmaterial].Artikel_1_Artikelnummer, Artikel.[Bezeichnung 1], CAST(0 AS decimal(20, 6)) AS Preis
	                                        INTO #PSZ_Materialanteil_Rohmaterialpreise
                                        FROM 
                                            #PSZ_Materialanteil_Rohmaterial AS [PSZ_Materialanteil Rohmaterial] INNER JOIN Artikel ON [PSZ_Materialanteil Rohmaterial].Artikel_1_Artikelnummer = Artikel.Artikelnummer
                                        GROUP BY [PSZ_Materialanteil Rohmaterial].Artikel_1_Artikelnummer, Artikel.[Bezeichnung 1];";
					// - Q7 
					string query_7 = $@"UPDATE 
                                            #PSZ_Materialanteil_Rohmaterialpreise  
                                            SET #PSZ_Materialanteil_Rohmaterialpreise.Preis = Bestellnummern.Einkaufspreis/Bestellnummern.Preiseinheit
                                            FROM
                                            #PSZ_Materialanteil_Rohmaterialpreise AS [PSZ_Materialanteil Rohmaterialpreise] 
                                                INNER JOIN (Artikel INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
                                                ON [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer = Artikel.Artikelnummer 
                                            WHERE (((Bestellnummern.Standardlieferant)=1));";
					// - Q8 
					string query_8 = $@"SELECT 
                                            [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer, 
                                            [PSZ_Materialanteil Rohmaterialpreise].[Bezeichnung 1], 
                                            [PSZ_Materialanteil Rohmaterialpreise].Preis, 
                                            Avg([bestellte Artikel].Einzelpreis/[bestellte Artikel].Preiseinheit) AS Einzelpreis 
                                            INTO #PSZ_Materialanteil_Rohmaterialpreise_Mittelwerte
                                        FROM 
                                            #PSZ_Materialanteil_Rohmaterialpreise AS [PSZ_Materialanteil Rohmaterialpreise]  
                                                INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
                                                ON [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer = Artikel.Artikelnummer
                                        WHERE 
                                            ((([bestellte Artikel].Liefertermin)>=@dateFrom And ([bestellte Artikel].Liefertermin)<=@dateTill))
                                        GROUP BY [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer, [PSZ_Materialanteil Rohmaterialpreise].[Bezeichnung 1], [PSZ_Materialanteil Rohmaterialpreise].Preis;";
					// - Q9 
					string query_9 = $@"UPDATE #PSZ_Materialanteil_Rohmaterialpreise
                                        SET #PSZ_Materialanteil_Rohmaterialpreise.Preis = [PSZ_Materialanteil Rohmaterialpreise Mittelwerte].Einzelpreis
                                        FROM #PSZ_Materialanteil_Rohmaterialpreise AS [PSZ_Materialanteil Rohmaterialpreise] 
                                            INNER JOIN #PSZ_Materialanteil_Rohmaterialpreise_Mittelwerte AS [PSZ_Materialanteil Rohmaterialpreise Mittelwerte] 
                                            ON [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer = [PSZ_Materialanteil Rohmaterialpreise Mittelwerte].Artikel_1_Artikelnummer;";
					// - Q10 
					string query_10 = $@"WITH PSZ_Materialanteil_UBG2_Kosten_cte (Stufe, Artikelnummer, Anzahl, Artikel_1_Artikelnummer, Preis, Gesamtpreis) AS 
                                            (
	                                            SELECT 
		                                            [PSZ_Materialanteil Rohmaterial].Stufe, 
		                                            [PSZ_Materialanteil Rohmaterial].Artikelnummer, 
		                                            [PSZ_Materialanteil Rohmaterial].Anzahl, 
		                                            Artikel.Artikelnummer, 
		                                            [PSZ_Materialanteil Rohmaterialpreise].Preis, 
		                                            [PSZ_Materialanteil Rohmaterial].Anzahl*[PSZ_Materialanteil Rohmaterialpreise].Preis AS Gesamtpreis
	                                            FROM 
		                                            (#PSZ_Materialanteil_Rohmaterial AS [PSZ_Materialanteil Rohmaterial] 
			                                            INNER JOIN Artikel ON [PSZ_Materialanteil Rohmaterial].Artikel_1_Artikelnummer = Artikel.Artikelnummer) 
			                                            INNER JOIN #PSZ_Materialanteil_Rohmaterialpreise AS [PSZ_Materialanteil Rohmaterialpreise] ON Artikel.Artikelnummer = [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer
	                                            GROUP BY 
		                                            [PSZ_Materialanteil Rohmaterial].Stufe, 
		                                            [PSZ_Materialanteil Rohmaterial].Artikelnummer, 
		                                            [PSZ_Materialanteil Rohmaterial].Anzahl, Artikel.Artikelnummer, 
		                                            [PSZ_Materialanteil Rohmaterialpreise].Preis, 
		                                            [PSZ_Materialanteil Rohmaterial].Anzahl*[PSZ_Materialanteil Rohmaterialpreise].Preis
	                                            HAVING [PSZ_Materialanteil Rohmaterial].Stufe='UBG2'
                                            )";
					// - Q11 
					string query_11 = $@"SELECT 
                                              [PSZ_Materialanteil 13 UBG2 auswählen].Artikelnummer, 
                                              Sum([PSZ_Materialanteil 13 UBG2 auswählen].Gesamtpreis) AS SummevonGesamtpreis, 
                                              artikel_kalkulatorische_kosten.Betrag 
                                              INTO #PSZ_Materialanteil_UBG2_Kosten
                                            FROM 
                                              PSZ_Materialanteil_UBG2_Kosten_cte AS [PSZ_Materialanteil 13 UBG2 auswählen] 
                                                  INNER JOIN (Artikel INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                                                  ON [PSZ_Materialanteil 13 UBG2 auswählen].Artikel_1_Artikelnummer = Artikel.Artikelnummer
                                            WHERE (((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                                            GROUP BY [PSZ_Materialanteil 13 UBG2 auswählen].Artikelnummer, artikel_kalkulatorische_kosten.Betrag;";

					// - Q12 
					string query_12 = $@"UPDATE #PSZ_Materialanteil_Rohmaterialpreise
                                          SET #PSZ_Materialanteil_Rohmaterialpreise.Preis = 
                                              ([PSZ_Materialanteil UBG2 Kosten].SummevonGesamtpreis+[PSZ_Materialanteil UBG2 Kosten].Betrag)*(1+(@surcharge/100))
                                          FROM #PSZ_Materialanteil_Rohmaterialpreise AS [PSZ_Materialanteil Rohmaterialpreise] 
                                              INNER JOIN #PSZ_Materialanteil_UBG2_Kosten AS [PSZ_Materialanteil UBG2 Kosten] ON [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer = [PSZ_Materialanteil UBG2 Kosten].Artikelnummer ;";
					// - Q13 
					string query_13 = $@"WITH PSZ_Materialanteil_16_UBG1_auswahlen_cte (Stufe, Artikelnummer, Anzahl, Artikel_1_Artikelnummer, Preis, Gesamtpreis) AS
                                            (
	                                            SELECT
	                                              [PSZ_Materialanteil Rohmaterial].Stufe, 
	                                              [PSZ_Materialanteil Rohmaterial].Artikelnummer, 
	                                              [PSZ_Materialanteil Rohmaterial].Anzahl, 
	                                              Artikel.Artikelnummer, 
	                                              [PSZ_Materialanteil Rohmaterialpreise].Preis, 
	                                              [PSZ_Materialanteil Rohmaterial].Anzahl*[PSZ_Materialanteil Rohmaterialpreise].Preis AS Gesamtpreis
	                                            FROM 
	                                              (#PSZ_Materialanteil_Rohmaterial AS [PSZ_Materialanteil Rohmaterial] 
	                                              INNER JOIN Artikel ON [PSZ_Materialanteil Rohmaterial].Artikel_1_Artikelnummer = Artikel.Artikelnummer) 
	                                              INNER JOIN #PSZ_Materialanteil_Rohmaterialpreise AS [PSZ_Materialanteil Rohmaterialpreise] ON Artikel.Artikelnummer = [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer
	                                            GROUP BY 
	                                              [PSZ_Materialanteil Rohmaterial].Stufe, [PSZ_Materialanteil Rohmaterial].Artikelnummer, 
	                                              [PSZ_Materialanteil Rohmaterial].Anzahl, Artikel.Artikelnummer, 
	                                              [PSZ_Materialanteil Rohmaterialpreise].Preis, [PSZ_Materialanteil Rohmaterial].Anzahl*[PSZ_Materialanteil Rohmaterialpreise].Preis
	                                            HAVING ((([PSZ_Materialanteil Rohmaterial].Stufe)='UBG1'))
                                            )";
					// - Q14 
					string query_14 = $@"SELECT 
                                              [PSZ_Materialanteil 16 UBG1 auswählen].Artikelnummer, 
                                              Sum([PSZ_Materialanteil 16 UBG1 auswählen].Gesamtpreis) AS SummevonGesamtpreis, 
                                              artikel_kalkulatorische_kosten.Betrag 
                                              INTO #PSZ_Materialanteil_UBG1_Kosten
                                            FROM PSZ_Materialanteil_16_UBG1_auswahlen_cte AS [PSZ_Materialanteil 16 UBG1 auswählen] INNER JOIN (Artikel INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) ON [PSZ_Materialanteil 16 UBG1 auswählen].Artikelnummer = Artikel.Artikelnummer
                                            WHERE (((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                                            GROUP BY [PSZ_Materialanteil 16 UBG1 auswählen].Artikelnummer, artikel_kalkulatorische_kosten.Betrag;";
					// - Q15 
					string query_15 = $@"UPDATE #PSZ_Materialanteil_Rohmaterialpreise
                                          SET #PSZ_Materialanteil_Rohmaterialpreise.Preis = 
                                              ([PSZ_Materialanteil UBG1 Kosten].SummevonGesamtpreis+[PSZ_Materialanteil UBG1 Kosten].Betrag)*(1+(@surcharge/100))
                                          FROM
                                              #PSZ_Materialanteil_Rohmaterialpreise AS [PSZ_Materialanteil Rohmaterialpreise] 
                                              INNER JOIN #PSZ_Materialanteil_UBG1_Kosten AS [PSZ_Materialanteil UBG1 Kosten] 
                                              ON [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer = [PSZ_Materialanteil UBG1 Kosten].Artikelnummer;";
					// - Q16 
					string query_16 = $@"IF OBJECT_ID('tempdb..#PSZ_Materialanteil_HBG_Kosten') IS NOT NULL DROP TABLE #PSZ_Materialanteil_HBG_Kosten;
                                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_19_HBG_auswahlen_cte') IS NOT NULL DROP TABLE #PSZ_Materialanteil_19_HBG_auswahlen_cte;
                                            SELECT 
                                                [PSZ_Materialanteil Rohmaterial].Stufe, 
                                                [PSZ_Materialanteil Rohmaterial].Artikelnummer, 
                                                [PSZ_Materialanteil Rohmaterial].Anzahl, 
                                                Artikel.Artikelnummer AS Artikel_1_Artikelnummer, 
                                                [PSZ_Materialanteil Rohmaterialpreise].Preis, 
                                                [PSZ_Materialanteil Rohmaterial].Anzahl*[PSZ_Materialanteil Rohmaterialpreise].Preis AS Gesamtpreis
	                                            INTO #PSZ_Materialanteil_19_HBG_auswahlen_cte
                                            FROM 
                                                (#PSZ_Materialanteil_Rohmaterial AS [PSZ_Materialanteil Rohmaterial] 
                                                LEFT JOIN Artikel ON [PSZ_Materialanteil Rohmaterial].Artikel_1_Artikelnummer = Artikel.Artikelnummer) 
                                                LEFT JOIN #PSZ_Materialanteil_Rohmaterialpreise AS [PSZ_Materialanteil Rohmaterialpreise] ON Artikel.Artikelnummer = [PSZ_Materialanteil Rohmaterialpreise].Artikel_1_Artikelnummer
                                            GROUP BY 
                                                [PSZ_Materialanteil Rohmaterial].Stufe, [PSZ_Materialanteil Rohmaterial].Artikelnummer, 
                                                [PSZ_Materialanteil Rohmaterial].Anzahl, Artikel.Artikelnummer, [PSZ_Materialanteil Rohmaterialpreise].Preis, 
                                                [PSZ_Materialanteil Rohmaterial].Anzahl*[PSZ_Materialanteil Rohmaterialpreise].Preis
                                            HAVING ((([PSZ_Materialanteil Rohmaterial].Stufe)='HBG'));";
					// - Q17 
					string query_17 = $@"SELECT 
                                          [PSZ_Materialanteil 19 HBG auswählen].Artikelnummer, 
                                          Sum([PSZ_Materialanteil 19 HBG auswählen].Gesamtpreis) AS SummevonGesamtpreis, 
                                          artikel_kalkulatorische_kosten.Betrag 
                                          INTO #PSZ_Materialanteil_HBG_Kosten
                                        FROM 
                                          #PSZ_Materialanteil_19_HBG_auswahlen_cte AS [PSZ_Materialanteil 19 HBG auswählen] 
                                              INNER JOIN (Artikel INNER JOIN artikel_kalkulatorische_kosten 
                                              ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                                              ON [PSZ_Materialanteil 19 HBG auswählen].Artikelnummer = Artikel.Artikelnummer
                                        WHERE (((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                                        GROUP BY [PSZ_Materialanteil 19 HBG auswählen].Artikelnummer, artikel_kalkulatorische_kosten.Betrag;";
					// - Q18 
					string query_18 = $@"INSERT INTO #PSZ_Materialanteil_HBG_Kosten ( Artikelnummer, SummevonGesamtpreis, Betrag )
                                        SELECT 
                                          [PSZ_Materialanteil 19 HBG auswählen].Artikelnummer, 
                                          [PSZ_Materialanteil 19 HBG auswählen].Gesamtpreis, 
                                          0 AS Ausdr1
                                        FROM 
                                          #PSZ_Materialanteil_19_HBG_auswahlen_cte AS [PSZ_Materialanteil 19 HBG auswählen] 
                                              INNER JOIN Artikel ON [PSZ_Materialanteil 19 HBG auswählen].Artikelnummer = Artikel.Artikelnummer
                                        WHERE (((Artikel.Stückliste)=0));";
					// - Q19 
					string query_19 = $@"IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Auswertung') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Auswertung;";
					// - Q20 
					string query_20 = $@"SELECT 
                                            [PSZ_Materialanteil Lieferübersicht].Artikelnummer, 
                                            [PSZ_Materialanteil Lieferübersicht].Liefermenge, 
                                            [PSZ_Materialanteil Lieferübersicht].Netto_gesamt AS Gesamtnettoumsatz, 
                                            [PSZ_Materialanteil HBG Kosten].SummevonGesamtpreis AS Einzelmaterialkosten, 
                                            [PSZ_Materialanteil HBG Kosten].Betrag AS Einzellohnkosten, 
                                            [PSZ_Materialanteil Lieferübersicht].Netto_gesamt/[PSZ_Materialanteil Lieferübersicht].Liefermenge AS Einzelnettoumsatz, 
                                            [PSZ_Materialanteil HBG Kosten].SummevonGesamtpreis*[PSZ_Materialanteil Lieferübersicht].Liefermenge/[PSZ_Materialanteil Lieferübersicht].Netto_gesamt AS Materialanteil_ungewichtet, 
                                            [PSZ_Materialanteil HBG Kosten].Betrag*[PSZ_Materialanteil Lieferübersicht].Liefermenge/[PSZ_Materialanteil Lieferübersicht].Netto_gesamt AS Lohnanteil, 
                                            1-(([PSZ_Materialanteil HBG Kosten].SummevonGesamtpreis+[PSZ_Materialanteil HBG Kosten].Betrag)*[PSZ_Materialanteil Lieferübersicht].Liefermenge/[PSZ_Materialanteil Lieferübersicht].Netto_gesamt) AS DB1,
	                                        /* will need columns later */
	                                        CAST(0 AS decimal(20, 6)) Materialanteil_gewichtet,
	                                        CAST(0 AS decimal(20, 6)) Lohnanteil_gewichtet,
	                                        CAST(0 AS decimal(20, 6)) DB1_gewichtet,
	                                        CAST(1 AS decimal(20, 6)) Mittelwerte_Gesamtnettoumsatz,
	                                        CAST(0 AS decimal(20, 6)) Mittelwerte_Materialanteil
	                                        INTO #PSZ_Materialanteil_Auswertung
                                        FROM 
                                            #PSZ_Materialanteil_Lieferübersicht AS [PSZ_Materialanteil Lieferübersicht] 
                                                INNER JOIN #PSZ_Materialanteil_HBG_Kosten AS [PSZ_Materialanteil HBG Kosten] ON [PSZ_Materialanteil Lieferübersicht].Artikelnummer = [PSZ_Materialanteil HBG Kosten].Artikelnummer
                                        WHERE ((([PSZ_Materialanteil Lieferübersicht].Netto_gesamt)>0));";
					// - Q21 
					string query_21 = $@"SELECT 
                                            @MittelwertvonGesamtnettoumsatz = Avg([PSZ_Materialanteil Auswertung].Gesamtnettoumsatz), 
                                            @MittelwertvonMaterialanteil_ungewichtet = Avg([PSZ_Materialanteil Auswertung].Materialanteil_ungewichtet) 
                                            /* INTO #PSZ_Materialanteil_Mittelwerte_Auswertung -- put in vars instead of table */
                                        FROM #PSZ_Materialanteil_Auswertung AS [PSZ_Materialanteil Auswertung];";
					// - Q22 
					string query_22 = $@"UPDATE #PSZ_Materialanteil_Auswertung 
                                            SET Mittelwerte_Gesamtnettoumsatz = @MittelwertvonGesamtnettoumsatz, Mittelwerte_Materialanteil = @MittelwertvonMaterialanteil_ungewichtet;";
					// - Q23 
					string query_23 = $@"UPDATE #PSZ_Materialanteil_Auswertung 
                                        SET 
                                            Materialanteil_gewichtet = (Gesamtnettoumsatz/Mittelwerte_Gesamtnettoumsatz)*Materialanteil_ungewichtet, 
                                            Lohnanteil_gewichtet = (Gesamtnettoumsatz/Mittelwerte_Gesamtnettoumsatz)*Lohnanteil, 
                                            DB1_gewichtet = (Gesamtnettoumsatz/Mittelwerte_Gesamtnettoumsatz)*DB1;";
					// - Q24 
					string query_24 = $@"SELECT 
                                            [PSZ_Materialanteil Auswertung].Artikelnummer, 
                                            [PSZ_Materialanteil Auswertung].Liefermenge, 
                                            [PSZ_Materialanteil Auswertung].Gesamtnettoumsatz, 
                                            [PSZ_Materialanteil Auswertung].Einzelmaterialkosten, 
                                            [PSZ_Materialanteil Auswertung].Einzellohnkosten, 
                                            [PSZ_Materialanteil Auswertung].Einzelnettoumsatz, 
                                            [PSZ_Materialanteil Auswertung].Materialanteil_ungewichtet * 100 AS Materialanteil_ungewichtet, 
                                            [PSZ_Materialanteil Auswertung].Lohnanteil * 100 AS Lohnanteil, 
                                            [PSZ_Materialanteil Auswertung].DB1 * 100 AS DB1, 
                                            [PSZ_Materialanteil Auswertung].Materialanteil_gewichtet * 100 AS Materialanteil_gewichtet, 
                                            [PSZ_Materialanteil Auswertung].Lohnanteil_gewichtet * 100 AS Lohnanteil_gewichtet, 
                                            [PSZ_Materialanteil Auswertung].DB1_gewichtet * 100 AS DB1_gewichtet
                                        FROM #PSZ_Materialanteil_Auswertung AS [PSZ_Materialanteil Auswertung];";
					#endregion

					// -
					string query = $"{query_0}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_1}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_2}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_3}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_4}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_5}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_6}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_7}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_8}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_9}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_10}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_11}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_12}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_13}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_14}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_15}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_16}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_17}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_18}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_19}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_20}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_21}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_22}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_23}{Environment.NewLine}{Environment.NewLine}" +
						$"{query_24}{Environment.NewLine}{Environment.NewLine}" +
						$@"/* DROP temp tables */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Lieferübersicht') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Lieferübersicht; /* Q1 */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Rohmaterial') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Rohmaterial; /* Q2 */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Rohmaterialpreise') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Rohmaterialpreise; /* Q5 */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Rohmaterialpreise_Mittelwerte') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Rohmaterialpreise_Mittelwerte; /* Q8 */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_UBG2_Kosten') IS NOT NULL DROP TABLE #PSZ_Materialanteil_UBG2_Kosten; /* Q11 */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_UBG1_Kosten') IS NOT NULL DROP TABLE #PSZ_Materialanteil_UBG1_Kosten; /* Q14 */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_HBG_Kosten') IS NOT NULL DROP TABLE #PSZ_Materialanteil_HBG_Kosten; /* Q17 */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Auswertung') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Auswertung; /* Q20 */
                            IF OBJECT_ID('tempdb..#PSZ_Materialanteil_Mittelwerte_Auswertung') IS NOT NULL DROP TABLE #PSZ_Materialanteil_Mittelwerte_Auswertung; /* Q21 */";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
					sqlCommand.Parameters.AddWithValue("dateTill", dateTill);
					sqlCommand.Parameters.AddWithValue("customerNumber", customerNr);
					sqlCommand.Parameters.AddWithValue("surcharge", surcharge);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_AverageMaterialContent(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessage> GetProjectMessage(string articleNumber, string projectNumber, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"SELECT * FROM PSZ_Projektdaten_Details WHERE [Projekt-Nr] Like '{projectNumber.SqlEscape()}%' AND  Artikelnummer Like '{articleNumber.SqlEscape()}%' ";

					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						query += " ORDER BY [Artikelnummer] DESC, [Projekt-Nr] ASC ";
					}

					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessage(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessagePDF> GetProjectMessage_PDFData(List<int> ids, int? pmeldung = 1)
			{
				if(ids == null || ids.Count <= 0)
					return null;

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - Q1
					string query_1 = $@"UPDATE PSZ_Projektdaten_Details 
	                                        SET [Summe Arbeitszeit] = CAST(Round(CAST([Arbeitszeit Serien Pro Kabesatz] AS DECIMAL(20,6))*[EAU]/60,4) AS DECIMAL(20,4))
                                        WHERE {(pmeldung.HasValue ? $"Projektmeldung = {pmeldung.Value} AND " : "")} ID IN ({string.Join(",", ids)});";
					// - Q2
					string query_2 = $@"IF OBJECT_ID('tempdb..#data_view') IS NOT NULL DROP TABLE #data_view;
                                        SELECT PSZ_Projektdaten_Projektmeldung_Filter.*, [PSZ_Nummerschlüssel Kunde].Kunde INTO #data_view
                                        FROM (SELECT PSZ_Projektdaten_Details.*, (LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) AS Kundenschlüssel FROM PSZ_Projektdaten_Details) AS PSZ_Projektdaten_Projektmeldung_Filter 
	                                        INNER JOIN [PSZ_Nummerschlüssel Kunde] 
	                                        ON PSZ_Projektdaten_Projektmeldung_Filter.Kundenschlüssel = [PSZ_Nummerschlüssel Kunde].Nummerschlüssel;";
					// - Q3
					string query_3 = $@"UPDATE PSZ_Projektdaten_Details SET PSZ_Projektdaten_Details.Projektmeldung = 0;";

					// - Q4
					string query_4 = $@"SELECT * FROM #data_view WHERE ID IN ({string.Join(",", ids)});";


					// -
					string query = $"{query_1}{System.Environment.NewLine}" +
									$"{query_2}{System.Environment.NewLine}" +
									$"{query_3}{System.Environment.NewLine}" +
									$"{query_4}{System.Environment.NewLine}" +
									$"IF OBJECT_ID('tempdb..#data_view') IS NOT NULL DROP TABLE #data_view;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessagePDF(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessage> UpdateProjectMessage(List<int> ids)
			{
				if(ids == null || ids.Count <= 0)
					return null;

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - Q1
					string query_1 = $@"UPDATE PSZ_Projektdaten_Details 
	                                        SET [Summe Arbeitszeit] = CAST(Round(CAST([Arbeitszeit Serien Pro Kabesatz] AS DECIMAL(20,6))*[EAU]/60,4) AS DECIMAL(20,4))
                                        WHERE ID IN ({string.Join(",", ids)});";
					// - Q2
					string query_4 = $@"SELECT * FROM PSZ_Projektdaten_Details WHERE ID IN ({string.Join(",", ids)});";


					// -
					string query = $"{query_1}{System.Environment.NewLine}" +
									$"{query_4}{System.Environment.NewLine}";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjectMessage(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetProjectMessage_Count(string articleNumber, string projectNumber)
			{
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT COUNT(*) FROM PSZ_Projektdaten_Details WHERE [Projekt-Nr] Like '{projectNumber.SqlEscape()}%' AND  Artikelnummer Like '{articleNumber.SqlEscape()}%' ";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var x) ? x : 0;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_MaterialbestandSpezifischLautNummernkreis> GetMaterialbestandSpezifischLautNummernkreisEngineering(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"
									WITH PSZ_AV_Material_Kundenspezifisch_1 AS (
										SELECT 
											Stücklisten.[Artikel-Nr des Bauteils], 
											Stücklisten.Artikelnummer
										FROM Artikel
										INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
										WHERE Artikel.Artikelnummer LIKE (@articleNumber + '%') AND Artikel.Freigabestatus<>'o' AND ISNULL(Artikel.aktiv,0)<>0
										GROUP BY Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer
    
									),
									PSZ_AV_Material_Kundenspezifisch_2 AS (
										SELECT 
											Stücklisten.[Artikel-Nr des Bauteils], 
											Stücklisten.Artikelnummer
										FROM Artikel
										INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
										WHERE Artikel.Artikelnummer NOT LIKE (@articleNumber + '%') AND Artikel.Freigabestatus<>'o' AND ISNULL(Artikel.aktiv,0)<>0
										GROUP BY Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer
									),
									PSZ_AV_cte AS (
										SELECT 
											k1.[Artikel-Nr des Bauteils], 
											k1.Artikelnummer
										FROM PSZ_AV_Material_Kundenspezifisch_1 k1
										LEFT JOIN PSZ_AV_Material_Kundenspezifisch_2 k2 
											ON k1.[Artikel-Nr des Bauteils] = k2.[Artikel-Nr des Bauteils]
										WHERE k2.[Artikel-Nr des Bauteils] IS NULL
									)
									SELECT Artikel.aktiv,Artikel.Freigabestatus,
										@articleNumber AS Nummernkreis,
										Artikel.Artikelnummer,
										Artikel.[Bezeichnung 1],
										Lager.Lagerort_id,
										Lagerorte.Lagerort,
										Lager.Bestand,
										Lager.Mindestbestand,
										Bestellnummern.[Bestell-Nr],
										adressen.Name1,
										Bestellnummern.Einkaufspreis,
										Artikel.Kupferzahl,
										Lager.Bestand * Bestellnummern.Einkaufspreis AS Bestandskosten
									FROM PSZ_AV_cte cte
									INNER JOIN Lager ON cte.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]
									INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
									INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
									INNER JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
									INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
									WHERE Lager.Bestand > 0 AND Bestellnummern.Standardlieferant = 1
									AND Artikel.Freigabestatus<>'o' AND ISNULL(Artikel.aktiv,0)<>0
									ORDER BY Artikel.Artikelnummer;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("articleNumber", articleNumber.SqlEscape());
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_MaterialbestandSpezifischLautNummernkreis(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_MaterialbestandSpezifischLautNummernkreisPurchase> GetMaterialbestandSpezifischLautNummernkreisPurchase(string orExtension, string andExtension)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"
									WITH PSZ_AV_Material_Kundenspezifisch_1 AS (
									SELECT
									Stücklisten.[Artikel-Nr des Bauteils], 
									Stücklisten.Artikelnummer
									FROM Artikel
									INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
									WHERE 
									({orExtension}) 
									AND Artikel.Freigabestatus<>'o' AND ISNULL(Artikel.aktiv,0)<>0
									GROUP BY Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer
    
									),
									PSZ_AV_Material_Kundenspezifisch_2 AS (
									SELECT 
									Stücklisten.[Artikel-Nr des Bauteils], 
									Stücklisten.Artikelnummer
									FROM Artikel
									INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
									WHERE 
									{andExtension}
									AND Artikel.Freigabestatus<>'o' AND ISNULL(Artikel.aktiv,0)<>0
									GROUP BY Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer
									),
									PSZ_AV_cte AS (
									SELECT 
									k1.[Artikel-Nr des Bauteils], 
									k1.Artikelnummer
									FROM PSZ_AV_Material_Kundenspezifisch_1 k1
									LEFT JOIN PSZ_AV_Material_Kundenspezifisch_2 k2 
									ON k1.[Artikel-Nr des Bauteils] = k2.[Artikel-Nr des Bauteils]
									WHERE k2.[Artikel-Nr des Bauteils] IS NULL
									)
									SELECT 
									Artikel.Artikelnummer,
									Artikel.aktiv,Artikel.Freigabestatus,
									Artikel.[Bezeichnung 1],
									Bestellnummern.[Bestell-Nr],
									adressen.Name1,
									Bestellnummern.Einkaufspreis,
									Artikel.Kupferzahl,
									Bestellnummern.Wiederbeschaffungszeitraum Lieferzeit,
									SUM(ISNULL(Lager.Bestand,0)) Bestand,
									SUM(ISNULL(Lager.Mindestbestand,0))Mindestbestand,
									SUM(ISNULL(Lager.Bestand,0)) * Bestellnummern.Einkaufspreis AS Bestandskosten
									FROM PSZ_AV_cte cte
									INNER JOIN Lager ON cte.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]
									INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
									INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
									INNER JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
									INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
									WHERE Bestellnummern.Standardlieferant = 1
									AND Artikel.Freigabestatus<>'o' AND ISNULL(Artikel.aktiv,0)<>0
									GROUP BY Artikel.aktiv, 
									Artikel.Freigabestatus,
									Artikel.Artikelnummer,
									Artikel.[Bezeichnung 1],
									Bestellnummern.[Bestell-Nr],
									adressen.Name1,
									Bestellnummern.Einkaufspreis,
									Artikel.Kupferzahl,
									Bestellnummern.Wiederbeschaffungszeitraum
									ORDER BY Artikel.Artikelnummer";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_MaterialbestandSpezifischLautNummernkreisPurchase(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_PszPrioeinkauf_report1> GetPszPrioeinkauf_report1(string searchTerms = "", int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				searchTerms = searchTerms?.Trim()?.ToLower().SqlEscape();
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"SELECT 
	                                    PSZ_PrioeinkaufohneFilter.[Bestellung-Nr], 
	                                    PSZ_PrioeinkaufohneFilter.Datum, 
	                                    PSZ_PrioeinkaufohneFilter.Anzahl, 
	                                    PSZ_PrioeinkaufohneFilter.Artikelnummer, 
	                                    PSZ_PrioeinkaufohneFilter.[Bezeichnung 1], 
	                                    PSZ_PrioeinkaufohneFilter.Liefertermin, 
	                                    PSZ_PrioeinkaufohneFilter.Bestätigter_Termin, 
	                                    TRIM(PSZ_PrioeinkaufohneFilter.Name1) AS Name1, 
                                        TRIM(PSZ_PrioeinkaufohneFilter.Telefon) AS Telefon, 
	                                    TRIM(PSZ_PrioeinkaufohneFilter.Fax) AS Fax, PSZ_PrioeinkaufohneFilter.Lagerort_id, 
	                                    PSZ_PrioeinkaufohneFilter.erledigt_pos, PSZ_PrioeinkaufohneFilter.Typ, 
	                                    PSZ_PrioeinkaufohneFilter.gebucht, PSZ_PrioeinkaufohneFilter.erledigt, 
	                                    PSZ_PrioeinkaufohneFilter.[Position erledigt], 
	                                    GETDATE()-[Datum] AS Differenz
                                    FROM View_Prioeinkauf AS PSZ_PrioeinkaufohneFilter
                                    WHERE (((GETDATE()-[Datum])>5)) ";
					if(!string.IsNullOrWhiteSpace(searchTerms))
					{
						query += $" AND (Name1 LIKE '{searchTerms}%' OR Telefon LIKE '{searchTerms}%' OR Fax LIKE '{searchTerms}%' OR Artikelnummer LIKE '{searchTerms}%' OR [Bezeichnung 1] LIKE '{searchTerms}%')";
					}
					query += $" ORDER BY Name1, Telefon, Fax, Datum OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_PszPrioeinkauf_report1(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_PszPrioeinkauf_report2> GetPszPrioeinkauf_report2(string searchTerms = "", int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				searchTerms = searchTerms?.Trim()?.ToLower().SqlEscape();
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"SELECT 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].[Bestellung-Nr], 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].Datum, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].Anzahl, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].Artikelnummer, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].[Bezeichnung 1], 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].Liefertermin, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].Bestätigter_Termin, 
	                                    TRIM([PSZ_Disposition Ab Termin zu Spät sql].Name1) Name1, 
	                                    TRIM([PSZ_Disposition Ab Termin zu Spät sql].Telefon) Telefon, 
	                                    TRIM([PSZ_Disposition Ab Termin zu Spät sql].Fax) Fax, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].Lagerort_id, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].erledigt_pos, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].Typ, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].gebucht, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].erledigt, 
	                                    [PSZ_Disposition Ab Termin zu Spät sql].[Position erledigt]
                                    FROM [View_PSZ_Disposition Ab Termin zu Spät sql] AS [PSZ_Disposition Ab Termin zu Spät sql]";
					if(!string.IsNullOrWhiteSpace(searchTerms))
					{
						query += $" WHERE (Name1 LIKE '{searchTerms}%' OR Telefon LIKE '{searchTerms}%' OR Fax LIKE '{searchTerms}%' OR Artikelnummer LIKE '{searchTerms}%' OR [Bezeichnung 1] LIKE '{searchTerms}%')";
					}
					query += $" ORDER BY Name1, Telefon, Fax, Datum OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_PszPrioeinkauf_report2(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetPszPrioeinkauf_report1_count(string searchTerms = "")
			{
				searchTerms = searchTerms?.Trim()?.ToLower().SqlEscape();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"SELECT COUNT(*) FROM View_Prioeinkauf AS PSZ_PrioeinkaufohneFilter WHERE (((GETDATE()-[Datum])>5))";
					if(!string.IsNullOrWhiteSpace(searchTerms))
					{
						query += $" AND (Name1 LIKE '{searchTerms}%' OR Telefon LIKE '{searchTerms}%' OR Fax LIKE '{searchTerms}%' OR Artikelnummer LIKE '{searchTerms}%' OR [Bezeichnung 1] LIKE '{searchTerms}%')";
					}

					var sqlCommand = new SqlCommand(query, sqlConnection);
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var val) ? val : 0;
				}
			}
			public static int GetPszPrioeinkauf_report2_count(string searchTerms = "")
			{
				searchTerms = searchTerms?.Trim()?.ToLower().SqlEscape();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"SELECT COUNT(*) FROM [View_PSZ_Disposition Ab Termin zu Spät sql] AS [PSZ_Disposition Ab Termin zu Spät sql]";
					if(!string.IsNullOrWhiteSpace(searchTerms))
					{
						query += $" WHERE (Name1 LIKE '{searchTerms}%' OR Telefon LIKE '{searchTerms}%' OR Fax LIKE '{searchTerms}%' OR Artikelnummer LIKE '{searchTerms}%' OR [Bezeichnung 1] LIKE '{searchTerms}%')";
					}

					var sqlCommand = new SqlCommand(query, sqlConnection);
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var val) ? val : 0;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_HighRunner> GetHighRunners(string customerNumber, DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - Q1
					string query_1 = $@"IF OBJECT_ID('tempdb..#PSZ_Bestellte_Artikel_Highrunnerliste') IS NOT NULL DROP TABLE #PSZ_Bestellte_Artikel_Highrunnerliste;
                                        IF OBJECT_ID('tempdb..#PSZ_bestellte_Artikel_Zusammenfassung') IS NOT NULL DROP TABLE #PSZ_bestellte_Artikel_Zusammenfassung;
                                        IF OBJECT_ID('tempdb..#Gebrauchte_Roh_Highrunner') IS NOT NULL DROP TABLE #Gebrauchte_Roh_Highrunner;
                                        IF OBJECT_ID('tempdb..#PSZ_bestellte_Artikel_Zusammenfassung_2') IS NOT NULL DROP TABLE #PSZ_bestellte_Artikel_Zusammenfassung_2;

                                        SELECT 
	                                        Bestellungen.Typ, Bestellungen.[Bestellung-Nr], 
	                                        Artikel.Artikelnummer, Artikel.[Bezeichnung 1], 
	                                        [bestellte Artikel].Anzahl, Bestellnummern.[Bestell-Nr], 
	                                        Bestellungen.[Lieferanten-Nr], adressen.Name1, 
	                                        Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, 
	                                        CAST(Artikel.Größe AS DECIMAL(38,15)) AS Gewichte 
	                                        INTO #PSZ_Bestellte_Artikel_Highrunnerliste
                                        FROM ((((Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
	                                        INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr) INNER JOIN Stücklisten 
	                                        ON [bestellte Artikel].[Artikel-Nr] = Stücklisten.[Artikel-Nr des Bauteils]) 
	                                        INNER JOIN (Artikel INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
	                                        ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]) 
	                                        INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr] = Artikel_1.[Artikel-Nr]
                                        WHERE (((Bestellnummern.Standardlieferant)=1) AND ((Bestellungen.Datum)>=@dateFrom 
	                                        And (Bestellungen.Datum)<=@dateTill) 
	                                        AND ((Artikel_1.Artikelnummer) Like (@customerNumber + '%')))
                                        GROUP BY Bestellungen.Typ, Bestellungen.[Bestellung-Nr], Artikel.Artikelnummer, 
	                                        Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, Bestellnummern.[Bestell-Nr], 
	                                        Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, Artikel.Größe
                                        HAVING (((Bestellungen.Typ)='Wareneingang'));";
					// - Q2
					string query_2 = $@"SELECT 
	                                        [PSZ_Bestellte Artikel Highrunnerliste].Artikelnummer, [PSZ_Bestellte Artikel Highrunnerliste].[Bezeichnung 1], 
	                                        Sum([PSZ_Bestellte Artikel Highrunnerliste].Anzahl) AS SummevonAnzahl, 
	                                        [PSZ_Bestellte Artikel Highrunnerliste].[Bestell-Nr], [PSZ_Bestellte Artikel Highrunnerliste].Name1, 
	                                        [PSZ_Bestellte Artikel Highrunnerliste].Einkaufspreis, [PSZ_Bestellte Artikel Highrunnerliste].Zolltarif_nr, 
	                                        [PSZ_Bestellte Artikel Highrunnerliste].Gewichte
	                                        INTO #PSZ_bestellte_Artikel_Zusammenfassung
                                        FROM #PSZ_Bestellte_Artikel_Highrunnerliste AS [PSZ_Bestellte Artikel Highrunnerliste]
                                        GROUP BY [PSZ_Bestellte Artikel Highrunnerliste].Artikelnummer, [PSZ_Bestellte Artikel Highrunnerliste].[Bezeichnung 1], 
	                                        [PSZ_Bestellte Artikel Highrunnerliste].[Bestell-Nr], [PSZ_Bestellte Artikel Highrunnerliste].Name1, 
	                                        [PSZ_Bestellte Artikel Highrunnerliste].Einkaufspreis, [PSZ_Bestellte Artikel Highrunnerliste].Zolltarif_nr, 
	                                        [PSZ_Bestellte Artikel Highrunnerliste].Gewichte;";
					// - Q3
					string query_3 = $@"SELECT 
	                                        [PSZ_bestellte Artikel Zusammenfassung].Artikelnummer, 
	                                        [PSZ_bestellte Artikel Zusammenfassung].[Bezeichnung 1], 
	                                        Artikel.[Bezeichnung 2], 
	                                        [PSZ_bestellte Artikel Zusammenfassung].[Bestell-Nr], 
	                                        [PSZ_bestellte Artikel Zusammenfassung].Name1, 
	                                        [PSZ_bestellte Artikel Zusammenfassung].SummevonAnzahl AS Einkaufsmenge, 
	                                        [PSZ_bestellte Artikel Zusammenfassung].Einkaufspreis, 
	                                        [PSZ_bestellte Artikel Zusammenfassung].SummevonAnzahl*[PSZ_bestellte Artikel Zusammenfassung].Einkaufspreis AS Einkaufsvolumen, 
	                                        [PSZ_bestellte Artikel Zusammenfassung].Zolltarif_nr, [PSZ_bestellte Artikel Zusammenfassung].Gewichte
	                                        INTO #PSZ_bestellte_Artikel_Zusammenfassung_2
                                        FROM #PSZ_bestellte_Artikel_Zusammenfassung AS [PSZ_bestellte Artikel Zusammenfassung] 
	                                        INNER JOIN Artikel ON [PSZ_bestellte Artikel Zusammenfassung].Artikelnummer = Artikel.Artikelnummer;";
					// - Q4
					string query_4 = $@"SELECT 
	                                        Sum(-1*[Fertigung_Fertigungsvorgang].[Anzahl]) AS MENGE, 
	                                        Artikel.Artikelnummer AS ROH, Fertigung.Artikel_Nr, 
	                                        Artikel_1.Artikelnummer AS [FG Artikelnummer] 
	                                        INTO #Gebrauchte_Roh_Highrunner
                                        FROM 
	                                        ((Fertigung_Fertigungsvorgang 
	                                        INNER JOIN Artikel ON Fertigung_Fertigungsvorgang.Artikel_nr = Artikel.[Artikel-Nr]) 
	                                        INNER JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) 
	                                        INNER JOIN Artikel AS Artikel_1 ON Fertigung.Artikel_Nr = Artikel_1.[Artikel-Nr]
                                        GROUP BY Artikel.Artikelnummer, Fertigung.Artikel_Nr, Artikel_1.Artikelnummer, Fertigung_Fertigungsvorgang.Datum
                                        HAVING (((Artikel_1.Artikelnummer) Like (@customerNumber + '%')) 
	                                        AND ((Fertigung_Fertigungsvorgang.Datum)>=@dateFrom 
	                                        And (Fertigung_Fertigungsvorgang.Datum)<=@dateTill))
                                        ORDER BY Artikel.Artikelnummer;";
					// - Q5
					string query_5 = $@"WITH Gebrauchte_ROH_Summe (ROH, SummevonMENGE) AS
                                        (
                                            SELECT 
	                                            Gebrauchte_Roh_Highrunner.ROH, 
	                                            Sum(Gebrauchte_Roh_Highrunner.MENGE) AS SummevonMENGE
                                            FROM #Gebrauchte_Roh_Highrunner AS Gebrauchte_Roh_Highrunner
                                            GROUP BY Gebrauchte_Roh_Highrunner.ROH
                                        )
                                        -- Q8
                                        SELECT 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].Artikelnummer, 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].[Bezeichnung 1], 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].[Bezeichnung 2], 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].[Bestell-Nr], 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].Name1, 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].Einkaufsmenge AS [Gebuchter Wareneingang], 
	                                        IIf([SummevonMENGE] Is Null, 0,[SummevonMENGE]) AS [Menge Gebucht FA], 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].Einkaufsvolumen, 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].Einkaufspreis, 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].Zolltarif_nr, 
	                                        [PSZ_bestellte Artikel Zusammenfassung 2].Gewichte
                                        FROM #PSZ_bestellte_Artikel_Zusammenfassung_2 AS [PSZ_bestellte Artikel Zusammenfassung 2] 
	                                        LEFT JOIN Gebrauchte_ROH_Summe 
	                                        ON [PSZ_bestellte Artikel Zusammenfassung 2].Artikelnummer = Gebrauchte_ROH_Summe.ROH;";
					// -
					string query = $"{query_1}{System.Environment.NewLine}" +
									$"{query_2}{System.Environment.NewLine}" +
									$"{query_3}{System.Environment.NewLine}" +
									$"{query_4}{System.Environment.NewLine}" +
									$"{query_5}{System.Environment.NewLine}" +
									$@"IF OBJECT_ID('tempdb..#PSZ_Bestellte_Artikel_Highrunnerliste') IS NOT NULL DROP TABLE #PSZ_Bestellte_Artikel_Highrunnerliste;
                                        IF OBJECT_ID('tempdb..#PSZ_bestellte_Artikel_Zusammenfassung') IS NOT NULL DROP TABLE #PSZ_bestellte_Artikel_Zusammenfassung;
                                        IF OBJECT_ID('tempdb..#Gebrauchte_Roh_Highrunner') IS NOT NULL DROP TABLE #Gebrauchte_Roh_Highrunner;
                                        IF OBJECT_ID('tempdb..#PSZ_bestellte_Artikel_Zusammenfassung_2') IS NOT NULL DROP TABLE #PSZ_bestellte_Artikel_Zusammenfassung_2;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
					sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
					sqlCommand.Parameters.AddWithValue("dateTill", dateTill);
					sqlCommand.CommandTimeout = 1000;
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_HighRunner(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int UpdateFixDelNote(string articleNumber, string HM, int newDelNote)
			{
				if(string.IsNullOrWhiteSpace(articleNumber) || string.IsNullOrWhiteSpace(HM))
					return -1;

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"UPDATE Artikel SET DEL=@newDelNote WHERE Artikelnummer Like '{articleNumber.SqlEscape()}%' AND [DEL fixiert]=1 AND IIf([Hubmastleitungen]=-1,'JA',IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}';";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("newDelNote", newDelNote);
					return int.TryParse(DbExecution.ExecuteNonQuery(sqlCommand).ToString(), out var x) ? x : 0;
				}
			}
			public static int UpdateSalesPriceWCopperOrders(string articleNumber, string HM)
			{
				if(string.IsNullOrWhiteSpace(articleNumber) || string.IsNullOrWhiteSpace(HM))
					return -1;

				articleNumber = articleNumber.SqlEscape();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - Q0 updating first sample and erstmuster Amor--Souilmi 17/12/2025
					string query_0 = $@"update [angebotene Artikel] SET [angebotene Artikel].VKEinzelpreis=[__BSD_ArtikelSalesExtension].Verkaufspreis,
										[angebotene Artikel].Einzelpreis = [__BSD_ArtikelSalesExtension].Verkaufspreis
										from [__BSD_ArtikelSalesExtension]
										INNER JOIN ((([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr) 
										INNER JOIN 
										(
										SELECT [angebotene Artikel].Nr,Artikel.Artikelnummer,Artikel.[Artikel-Nr], Artikel.DEL, Angebote.Typ,
										[angebotene Artikel].VKEinzelpreis,[angebotene Artikel].Einzelpreis,
										case 
										when [angebotene Artikel].Typ=1 then 'Prototype'
										when [angebotene Artikel].Typ=2 then 'First sample'
										when [angebotene Artikel].Typ=4 then 'Serie'
										else 'Null serie'
										END AS [Type],
										Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
										FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
										WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') 
										AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) 
										AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) 
										AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
										) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
										ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr) on [__BSD_ArtikelSalesExtension].ArticleNr=[angebotene Artikel].[Artikel-Nr]
										and [__BSD_ArtikelSalesExtension].ArticleSalesType=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Type]
										WHERE (((Angebote.Typ)='Auftragsbestätigung') AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										AND (IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) AND ((Artikel.[DEL fixiert])=1))
										and [__BSD_ArtikelSalesExtension].ArticleSalesTypeId in (0,1,2);";


					// - Q1
					string query_1 = $@"Update [angebotene Artikel] 
                                            SET [angebotene Artikel].VKEinzelpreis = [Preisgruppen].[Verkaufspreis], [angebotene Artikel].Einzelpreis = [Preisgruppen].[Verkaufspreis]
	                                            FROM Preisgruppen 
		                                            INNER JOIN ((([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr) 
		                                            INNER JOIN (
			                                            SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
			                                            FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
			                                            WHERE Artikel.Artikelnummer Like '{articleNumber}%' AND Angebote.Typ='Auftragsbestätigung' AND Artikel.[DEL fixiert]=1 AND IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0 /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND [angebotene Artikel].erledigt_pos=0 AND Angebote.gebucht=1 AND Angebote.erledigt=0 AND IIf([Hubmastleitungen]=1,'JA','NEIN')='{HM}'
		                                            ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr) ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]
	                                            WHERE (((Angebote.Typ)='Auftragsbestätigung') AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) AND (IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ((Artikel.[DEL fixiert])=1) AND [angebotene Artikel].Typ not in (1,2,3));";

					// - Q2
					string query_2 = $@"UPDATE [angebotene Artikel] 
	                                    SET [angebotene Artikel].Bezeichnung1 = Artikel.[Bezeichnung 1], 
	                                    [angebotene Artikel].Bezeichnung2 = Artikel.[Bezeichnung 2], 
	                                    [angebotene Artikel].Bezeichnung3 = Artikel.[Bezeichnung 3], 
	                                    [angebotene Artikel].Einheit = Artikel.Einheit, 
	                                    [angebotene Artikel].Preisgruppe = 1, 
	                                    [angebotene Artikel].Preiseinheit = Artikel.Preiseinheit, 
	                                    [angebotene Artikel].Zeichnungsnummer = Artikel.Index_Kunde, 
	                                    [angebotene Artikel].[VK-Festpreis] = Artikel.[VK-Festpreis], 
	                                    [angebotene Artikel].Kupferbasis = Artikel.Kupferbasis,
	                                    [angebotene Artikel].DEL = Artikel.DEL, 
	                                    [angebotene Artikel].[EinzelCu-Gewicht] = Artikel.[Cu-Gewicht], 
	                                    [angebotene Artikel].VKEinzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].VKEinzelpreis))))), 
	                                    [angebotene Artikel].Einzelkupferzuschlag = Round(IIf(Artikel.[VK-Festpreis]=0,((Artikel.DEL*1.01)-Artikel.Kupferbasis)/100*Artikel.[Cu-Gewicht],0),2), 
	                                    /*[angebotene Artikel].Fertigungsnummer = 0, */
	                                    [angebotene Artikel].[DEL fixiert] = Artikel.[DEL fixiert], 
	                                    [angebotene Artikel].Abladestelle = Artikel.Abladestelle
	                                    FROM (([angebotene Artikel] 
	                                    INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                    INNER JOIN (
	                                    SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
		                                    FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
			                                    INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                    WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
	                                    ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr
                                    WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]) AND ((Preisgruppen.Preisgruppe)=1) AND [angebotene Artikel].Typ not in (1,2,3));";

					// - Q3
					string query_3 = $@"UPDATE [Preisgruppen] 
	                                        SET Preisgruppen.PM2 = (1-[Staffelpreis2]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM3 = (1-[Staffelpreis3]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM4 = (1-[Staffelpreis4]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM1 = 0, 
	                                        Preisgruppen.Staffelpreis1 = [Verkaufspreis]
	                                        FROM (([angebotene Artikel] 
		                                        INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
		                                        INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
		                                        INNER JOIN (
			                                    SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                    FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
			                                    WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                    ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr
                                    WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]) AND ((Preisgruppen.Preisgruppe)=1));";

					// - Q4
					string query_4 = $@"Update [angebotene Artikel]
	                                    SET [angebotene Artikel].USt = IIf(Kunden.[Umsatzsteuer berechnen]=1,0.19,0), 
		                                    [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),
		                                    IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+[angebotene Artikel].VKEinzelpreis))))), 
		                                    [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
		                                    [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Einzelkupferzuschlag)
		                                    FROM (Angebote INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
			                                    INNER JOIN ((
				                                    SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
					                                    FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
						                                    INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
				                                    WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
			                                    ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
		                                    INNER JOIN ([angebotene Artikel] INNER JOIN Preisgruppen ON [angebotene Artikel].[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) ON [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr = [angebotene Artikel].Nr) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
	                                    WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]) AND [angebotene Artikel].Typ not in (1,2,3));";

					//  - Q4.2 /updating Einzelpreis of erstmuster and nullSerie
					string query_4_1 = $@"Update [angebotene Artikel]
										SET [angebotene Artikel].USt = IIf(Kunden.[Umsatzsteuer berechnen]=1,0.19,0),  
										[angebotene Artikel].Einzelpreis=(Einzelkupferzuschlag * Preiseinheit+ VKEinzelpreis),
										[angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
										[angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Einzelkupferzuschlag)
										FROM (Angebote INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
										INNER JOIN ((
										SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
										FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
										WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) 
										AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) 
										AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
										) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
										INNER JOIN ([angebotene Artikel] INNER JOIN Preisgruppen ON [angebotene Artikel].[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
										ON [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr = [angebotene Artikel].Nr) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
										WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]) AND [angebotene Artikel].Typ in (1,2,3));";
					// - Q5
					string query_5 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
		                                        FROM Preisgruppen 
			                                        INNER JOIN ((Kunden INNER JOIN Angebote ON Kunden.nummer = Angebote.[Kunden-Nr]) INNER JOIN ((
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
			                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] INNER JOIN [angebotene Artikel] ON [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr = [angebotene Artikel].Nr) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));";

					// - Q6
					string query_6 = $@"Update [angebotene Artikel]
                                            SET [angebotene Artikel].Einzelkupferzuschlag = Round(IIf([angebotene Artikel].[VK-Festpreis] = 0, (([angebotene Artikel].DEL * 1.01) - [angebotene Artikel].Kupferbasis) / 100 * [angebotene Artikel].[EinzelCu-Gewicht], 0), 2)
	                                            FROM [angebotene Artikel] INNER JOIN (
		                                            SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
			                                            FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                            WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
	                                            ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung]
	                                            ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr
                                            WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));";

					// -  Q7
					string query_7 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
	                                        [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].[VKEinzelpreis],IIf([Anzahl]<=[ME1],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),
	                                        IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+[angebotene Artikel].[VKEinzelpreis])))))
		                                        FROM Preisgruppen INNER JOIN ([angebotene Artikel] INNER JOIN (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr) ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]) AND [angebotene Artikel].Typ not in (1,2,3));";

					// - Q8
					string query_8 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*[angebotene Artikel].Anzahl*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].OriginalAnzahl = [angebotene Artikel].Anzahl, 
	                                        [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Anzahl*[angebotene Artikel].Einzelkupferzuschlag), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
		                                        FROM [angebotene Artikel] INNER JOIN (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
	                                        ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));";

					// - 
					string query = //$"{query_0}{System.Environment.NewLine}" +
										$"{query_1}{System.Environment.NewLine}" +
										$"{query_2}{System.Environment.NewLine}" +
										$"{query_3}{System.Environment.NewLine}" +
										$"{query_4}{System.Environment.NewLine}" +
										//$"{query_4_1}{System.Environment.NewLine}" +
										$"{query_5}{System.Environment.NewLine}" +
										$"{query_6}{System.Environment.NewLine}" +
										$"{query_7}{System.Environment.NewLine}" +
										$"{query_8}{System.Environment.NewLine}";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300; // 5 min
					return int.TryParse(DbExecution.ExecuteNonQuery(sqlCommand).ToString(), out var x) ? x : 0;
				}
			}
			public static int UpdateSalesPriceWoCopperOrders(string articleNumber, string HM)
			{
				if(string.IsNullOrWhiteSpace(articleNumber) || string.IsNullOrWhiteSpace(HM))
					return -1;

				articleNumber = articleNumber.SqlEscape();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - Q1
					string query_1 = $@"UPDATE [angebotene Artikel] 
	                                        SET [angebotene Artikel].Einzelpreis = [Verkaufspreis], 
	                                        [angebotene Artikel].VKEinzelpreis = [Verkaufspreis]
		                                        FROM (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung] INNER JOIN (([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) ON [Offene Ab ermitteln Für VK aktualisierung].Nr = [angebotene Artikel].Nr;";

					// - Q2
					string query_2 = $@"UPDATE [angebotene Artikel] 
                                            SET [angebotene Artikel].Bezeichnung1 = Artikel.[Bezeichnung 1], 
                                            [angebotene Artikel].Bezeichnung2 = Artikel.[Bezeichnung 2], 
                                            [angebotene Artikel].Bezeichnung3 = Artikel.[Bezeichnung 3], 
                                            [angebotene Artikel].Einheit = Artikel.Einheit,
                                            [angebotene Artikel].Preisgruppe = 1, 
                                            [angebotene Artikel].Preiseinheit = Artikel.Preiseinheit, 
                                            [angebotene Artikel].Zeichnungsnummer = Artikel.Index_Kunde, 
                                            [angebotene Artikel].[VK-Festpreis] = Artikel.[VK-Festpreis], 
                                            [angebotene Artikel].Kupferbasis = Artikel.Kupferbasis, 
                                            [angebotene Artikel].[EinzelCu-Gewicht] = Artikel.[Cu-Gewicht], 
                                            [angebotene Artikel].VKEinzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].VKEinzelpreis))))),
                                            [angebotene Artikel].[DEL fixiert] = Artikel.[DEL fixiert], 
                                            [angebotene Artikel].Abladestelle = Artikel.Abladestelle
	                                            FROM (
		                                            SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
			                                            FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                            WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
	                                            ) AS [Offene Ab ermitteln Für VK aktualisierung], ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                            WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]) AND ((Preisgruppen.Preisgruppe)=1));";

					// - Q3
					string query_3 = $@"UPDATE Preisgruppen 
	                                        SET Preisgruppen.PM2 = (1-[Staffelpreis2]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM3 = (1-[Staffelpreis3]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM4 = (1-[Staffelpreis4]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM1 = 0, 
	                                        Preisgruppen.Staffelpreis1 = [Verkaufspreis]
		                                        FROM (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
			                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung], ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]) AND ((Preisgruppen.Preisgruppe)=1));";

					// - Q4
					string query_4 = $@"UPDATE [angebotene Artikel] 
	                                        SET [angebotene Artikel].USt = IIf(Kunden.[Umsatzsteuer berechnen]=1,0.19,0), 
	                                        [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+[angebotene Artikel].VKEinzelpreis)))))
		                                        FROM (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
			                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung], ((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) INNER JOIN Preisgruppen ON [angebotene Artikel].[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]));";

					// - Q5
					string query_5 = $@"UPDATE [angebotene Artikel] 
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
		                                        FROM ((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
			                                        INNER JOIN (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
			                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung] ON [angebotene Artikel].Nr = [Offene Ab ermitteln Für VK aktualisierung].Nr
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]));";

					// - Q6
					string query_6 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
	                                        [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+[angebotene Artikel].VKEinzelpreis)))))
		                                        FROM (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung], [angebotene Artikel] INNER JOIN Preisgruppen ON [angebotene Artikel].[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]));";

					// - Q7
					string query_7 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*[angebotene Artikel].Anzahl*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].OriginalAnzahl = [angebotene Artikel].Anzahl, 
	                                        [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Anzahl*[angebotene Artikel].Einzelkupferzuschlag), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
		                                        FROM [angebotene Artikel] INNER JOIN (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) /*ignore fixedPrice pos* / AND IsNULL([angebotene Artikel].[EKPreise_Fix],0)=0 / * close */ AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung] ON [angebotene Artikel].Nr = [Offene Ab ermitteln Für VK aktualisierung].Nr
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]));";

					// -
					string query = $"{query_1}{System.Environment.NewLine}" +
										$"{query_2}{System.Environment.NewLine}" +
										$"{query_3}{System.Environment.NewLine}" +
										$"{query_4}{System.Environment.NewLine}" +
										$"{query_5}{System.Environment.NewLine}" +
										$"{query_6}{System.Environment.NewLine}" +
										$"{query_7}{System.Environment.NewLine}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300; // 5 min
					return int.TryParse(DbExecution.ExecuteNonQuery(sqlCommand).ToString(), out var x) ? x : 0;
				}
			}
			public static List<AngeboteneArtikelEntity> GetOpenRAPositions_wCopper(string articleNumber, string HM)
			{
				if(string.IsNullOrWhiteSpace(articleNumber) || string.IsNullOrWhiteSpace(HM))
					return null;

				articleNumber = articleNumber.SqlEscape();

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var query = $@"SELECT * FROM [angebotene Artikel] WHERE Nr IN (
                                        SELECT RaPos.Nr
                                        FROM ([angebotene Artikel] RaPos
	                                        INNER JOIN __CTS_AngeboteBlanketExtension raExt on raExt.AngeboteNr=RaPos.[Angebot-Nr]
	                                        INNER JOIN __CTS_AngeboteArticleBlanketExtension raPosExt ON raPosExt.AngeboteArtikelNr=RaPos.Nr
			                                        INNER JOIN Artikel ON RaPos.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
			                                        INNER JOIN __CTS_RahmenPriceHistory RaPrice on RaPrice.PositionNr=RaPos.Nr AND (RaPrice.ValidFrom<=Getdate())
                                        WHERE Artikel.Artikelnummer Like '{articleNumber}%' 
	                                        AND Artikel.[DEL fixiert]=1 AND RaPos.erledigt_pos=0 
	                                        AND raExt.StatusId = 2 
	                                        AND raPosExt.GultigAb <= GETDATE() AND raPosExt.GultigBis>=GETDATE()
	                                        AND IIf([Hubmastleitungen]=1,'JA','NEIN')='{HM}'
                                        )";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300; // 5 min
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<AngeboteneArtikelEntity> GetOpenRAPositions_woCopper(string articleNumber, string HM)
			{
				if(string.IsNullOrWhiteSpace(articleNumber) || string.IsNullOrWhiteSpace(HM))
					return null;

				articleNumber = articleNumber.SqlEscape();

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var query = $@"SELECT * FROM [angebotene Artikel] WHERE Nr IN (
                                        SELECT RaPos.Nr
                                        FROM ([angebotene Artikel] RaPos
	                                        INNER JOIN __CTS_AngeboteBlanketExtension raExt on raExt.AngeboteNr=RaPos.[Angebot-Nr]
	                                        INNER JOIN __CTS_AngeboteArticleBlanketExtension raPosExt ON raPosExt.AngeboteArtikelNr=RaPos.Nr
			                                        INNER JOIN Artikel ON RaPos.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
			                                        INNER JOIN __CTS_RahmenPriceHistory RaPrice on RaPrice.PositionNr=RaPos.Nr AND (RaPrice.ValidFrom<=Getdate())
                                        WHERE Artikel.Artikelnummer Like '{articleNumber}%' 
	                                        AND Artikel.[DEL fixiert]=0 AND RaPos.erledigt_pos=0 
	                                        AND raExt.StatusId = 2 
	                                        AND raPosExt.GultigAb <= GETDATE() AND raPosExt.GultigBis>=GETDATE()
	                                        AND IIf([Hubmastleitungen]=1,'JA','NEIN')='{HM}'
                                        )";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300; // 5 min
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int UpdateSalesPriceWCopperOrders_wRa(string articleNumber, string HM)
			{
				if(string.IsNullOrWhiteSpace(articleNumber) || string.IsNullOrWhiteSpace(HM))
					return -1;

				articleNumber = articleNumber.SqlEscape();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - Q1 - updating Price from Artikel
					string query_1 = $@"";

					// - Q2
					string query_2 = $@"UPDATE [angebotene Artikel] 
	                                    SET [angebotene Artikel].Bezeichnung1 = Artikel.[Bezeichnung 1], 
	                                    [angebotene Artikel].Bezeichnung2 = Artikel.[Bezeichnung 2], 
	                                    [angebotene Artikel].Bezeichnung3 = Artikel.[Bezeichnung 3], 
	                                    [angebotene Artikel].Einheit = Artikel.Einheit, 
	                                    [angebotene Artikel].Preisgruppe = 1, 
	                                    [angebotene Artikel].Preiseinheit = Artikel.Preiseinheit, 
	                                    [angebotene Artikel].Zeichnungsnummer = Artikel.Index_Kunde, 
	                                    /*[angebotene Artikel].[VK-Festpreis] = Artikel.[VK-Festpreis],*/
	                                    [angebotene Artikel].Kupferbasis = Artikel.Kupferbasis,
	                                    [angebotene Artikel].DEL = Artikel.DEL, 
	                                    [angebotene Artikel].[EinzelCu-Gewicht] = Artikel.[Cu-Gewicht], 
	                                    [angebotene Artikel].VKEinzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].VKEinzelpreis))))), 
	                                    [angebotene Artikel].Einzelkupferzuschlag = Round(IIf(Artikel.[VK-Festpreis]=0,((Artikel.DEL*1.01)-Artikel.Kupferbasis)/100*Artikel.[Cu-Gewicht],0),2), 
	                                    /*[angebotene Artikel].Fertigungsnummer = 0, */
	                                    [angebotene Artikel].[DEL fixiert] = Artikel.[DEL fixiert], 
	                                    [angebotene Artikel].Abladestelle = Artikel.Abladestelle
	                                    FROM (([angebotene Artikel] 
	                                    INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                    INNER JOIN (
	                                    SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
		                                    FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
			                                    INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                    WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
	                                    ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr
                                    WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]) AND ((Preisgruppen.Preisgruppe)=1));";

					// - Q3 - update Preisgruppen
					string query_3 = $@"";

					// - Q4
					string query_4 = $@"Update [angebotene Artikel]
	                                    SET [angebotene Artikel].USt = IIf(Kunden.[Umsatzsteuer berechnen]=1,0.19,0), 
		                                    [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),
		                                    IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+[angebotene Artikel].VKEinzelpreis))))), 
		                                    [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
		                                    [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Einzelkupferzuschlag)
		                                    FROM (Angebote INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
			                                    INNER JOIN ((
				                                    SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
					                                    FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
						                                    INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
				                                    WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
			                                    ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
		                                    INNER JOIN ([angebotene Artikel] INNER JOIN Preisgruppen ON [angebotene Artikel].[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) ON [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr = [angebotene Artikel].Nr) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
	                                    WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));";

					// - Q5
					string query_5 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
		                                        FROM Preisgruppen 
			                                        INNER JOIN ((Kunden INNER JOIN Angebote ON Kunden.nummer = Angebote.[Kunden-Nr]) INNER JOIN ((
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
			                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] INNER JOIN [angebotene Artikel] ON [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr = [angebotene Artikel].Nr) ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));";

					// - Q6
					string query_6 = $@"Update [angebotene Artikel]
                                            SET [angebotene Artikel].Einzelkupferzuschlag = Round(IIf([angebotene Artikel].[VK-Festpreis] = 0, (([angebotene Artikel].DEL * 1.01) - [angebotene Artikel].Kupferbasis) / 100 * [angebotene Artikel].[EinzelCu-Gewicht], 0), 2)
	                                            FROM [angebotene Artikel] INNER JOIN (
		                                            SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
			                                            FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                            WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
	                                            ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung]
	                                            ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr
                                            WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));";

					// -  Q7
					string query_7 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
	                                        [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].[VKEinzelpreis],IIf([Anzahl]<=[ME1],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),
	                                        IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+[angebotene Artikel].[VKEinzelpreis])))))
		                                        FROM Preisgruppen INNER JOIN ([angebotene Artikel] INNER JOIN (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr) ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));";

					// - Q8
					string query_8 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*[angebotene Artikel].Anzahl*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].OriginalAnzahl = [angebotene Artikel].Anzahl, 
	                                        [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Anzahl*[angebotene Artikel].Einzelkupferzuschlag), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
		                                        FROM [angebotene Artikel] INNER JOIN (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like '{articleNumber}%') AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
	                                        ) AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));";

					// - 
					string query = $"{query_1}{System.Environment.NewLine}" +
										$"{query_2}{System.Environment.NewLine}" +
										$"{query_3}{System.Environment.NewLine}" +
										$"{query_4}{System.Environment.NewLine}" +
										$"{query_5}{System.Environment.NewLine}" +
										$"{query_6}{System.Environment.NewLine}" +
										$"{query_7}{System.Environment.NewLine}" +
										$"{query_8}{System.Environment.NewLine}";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300; // 5 min
					return int.TryParse(DbExecution.ExecuteNonQuery(sqlCommand).ToString(), out var x) ? x : 0;
				}
			}
			public static int UpdateSalesPriceWoCopperOrders_wRa(string articleNumber, string HM)
			{
				if(string.IsNullOrWhiteSpace(articleNumber) || string.IsNullOrWhiteSpace(HM))
					return -1;

				articleNumber = articleNumber.SqlEscape();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// - Q1
					string query_1 = $@"";

					// - Q2
					string query_2 = $@"UPDATE [angebotene Artikel] 
                                            SET [angebotene Artikel].Bezeichnung1 = Artikel.[Bezeichnung 1], 
                                            [angebotene Artikel].Bezeichnung2 = Artikel.[Bezeichnung 2], 
                                            [angebotene Artikel].Bezeichnung3 = Artikel.[Bezeichnung 3], 
                                            [angebotene Artikel].Einheit = Artikel.Einheit,
                                            [angebotene Artikel].Preisgruppe = 1, 
                                            [angebotene Artikel].Preiseinheit = Artikel.Preiseinheit, 
                                            [angebotene Artikel].Zeichnungsnummer = Artikel.Index_Kunde, 
                                            /*[angebotene Artikel].[VK-Festpreis] = Artikel.[VK-Festpreis],*/ 
                                            [angebotene Artikel].Kupferbasis = Artikel.Kupferbasis, 
                                            [angebotene Artikel].[EinzelCu-Gewicht] = Artikel.[Cu-Gewicht], 
                                            [angebotene Artikel].VKEinzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].VKEinzelpreis))))),
                                            [angebotene Artikel].[DEL fixiert] = Artikel.[DEL fixiert], 
                                            [angebotene Artikel].Abladestelle = Artikel.Abladestelle
	                                            FROM (
		                                            SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
			                                            FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                            WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
	                                            ) AS [Offene Ab ermitteln Für VK aktualisierung], ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                            WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]) AND ((Preisgruppen.Preisgruppe)=1));";

					// - Q3
					string query_3 = $@"";

					// - Q4
					string query_4 = $@"UPDATE [angebotene Artikel] 
	                                        SET [angebotene Artikel].USt = IIf(Kunden.[Umsatzsteuer berechnen]=1,0.19,0), 
	                                        [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+[angebotene Artikel].VKEinzelpreis)))))
		                                        FROM (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
			                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung], ((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) INNER JOIN Preisgruppen ON [angebotene Artikel].[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]));";

					// - Q5
					string query_5 = $@"UPDATE [angebotene Artikel] 
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
		                                        FROM ((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
			                                        INNER JOIN (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
			                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung] ON [angebotene Artikel].Nr = [Offene Ab ermitteln Für VK aktualisierung].Nr
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]));";

					// - Q6
					string query_6 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
	                                        [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+[angebotene Artikel].VKEinzelpreis)))))
		                                        FROM (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung], [angebotene Artikel] INNER JOIN Preisgruppen ON [angebotene Artikel].[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]));";

					// - Q7
					string query_7 = $@"Update [angebotene Artikel]
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*[angebotene Artikel].Anzahl*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].OriginalAnzahl = [angebotene Artikel].Anzahl, 
	                                        [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Anzahl*[angebotene Artikel].Einzelkupferzuschlag), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
		                                        FROM [angebotene Artikel] INNER JOIN (
			                                        SELECT [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr]
				                                        FROM ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr
		                                        WHERE (((Artikel.Artikelnummer) Like ('{articleNumber}%')) AND ((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=0) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)>0) AND ([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND ((IIf([Hubmastleitungen]=1,'JA','NEIN'))='{HM}'))
		                                        ) AS [Offene Ab ermitteln Für VK aktualisierung] ON [angebotene Artikel].Nr = [Offene Ab ermitteln Für VK aktualisierung].Nr
                                        WHERE ((([angebotene Artikel].Nr)=[Offene Ab ermitteln Für VK aktualisierung].[Nr]));";

					// -
					string query = $"{query_1}{System.Environment.NewLine}" +
										$"{query_2}{System.Environment.NewLine}" +
										$"{query_3}{System.Environment.NewLine}" +
										$"{query_4}{System.Environment.NewLine}" +
										$"{query_5}{System.Environment.NewLine}" +
										$"{query_6}{System.Environment.NewLine}" +
										$"{query_7}{System.Environment.NewLine}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300; // 5 min
					return int.TryParse(DbExecution.ExecuteNonQuery(sqlCommand).ToString(), out var x) ? x : 0;
				}
			}
			public static Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH GetSuperbillROH(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHInput> data, SqlConnection connection, SqlTransaction transaction, bool isCreate = true)
			{
				if(data == null || data.Count <= 0)
					return null;

				var result = new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH();


				// - 
				//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					//sqlConnection.Open();
					var connectionId = connection.ClientConnectionId.ToString().Replace('-', '_');

					string swapQuery = "";

					if(isCreate)
					{
						string insertValues = "";
						for(int i = 0; i < data.Count; i++)
						{
							insertValues += $"(@Artikelnummer_{i}, @Menge_{i}),";
						}
						insertValues = insertValues.TrimEnd(',');
						// -
						swapQuery = $"INSERT INTO #PSZ_Superbill_FG(Artikelnummer, Menge) VALUES {insertValues};";
					}
					else
					{
						swapQuery = $@"IF OBJECT_ID('tempdb..#Superbill_Auto') IS NOT NULL DROP TABLE #Superbill_Auto;
                                        SELECT Fertigung.FA_Gestartet, Artikel.Artikelnummer, Sum(Fertigung.Anzahl) AS SummevonAnzahl
                                        INTO #Superbill_Auto
                                        FROM Artikel 
	                                        INNER JOIN (SELECT * FROM Fertigung WHERE Fertigungsnummer IN ({string.Join(",", data.Select(x => x.Fertigungsnummer))})) AS Fertigung
	                                        ON Artikel.[Artikel-Nr] = Fertigung.Artikel_Nr
                                        GROUP BY Fertigung.FA_Gestartet, Artikel.Artikelnummer
                                        HAVING (((Fertigung.FA_Gestartet) Is Null Or (Fertigung.FA_Gestartet)=0));

                                        INSERT INTO #PSZ_Superbill_FG (Artikelnummer, Menge)
                                        SELECT  Superbill_Auto.Artikelnummer, Superbill_Auto.SummevonAnzahl AS Menge FROM #Superbill_Auto AS Superbill_Auto;";
					}

					// - 15 | 19
					string query = $@"/* BUTTON 15 */
                                    /* Q1 */
                                    IF OBJECT_ID('tempdb..#PSZ_Superbill_FG') IS NOT NULL DROP TABLE #PSZ_Superbill_FG;
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Detail_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Detail_{connectionId}];
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Summe_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Summe_{connectionId}];


                                    CREATE TABLE #PSZ_Superbill_FG(Artikelnummer nvarchar(20), Menge decimal(28,6));
                                    {swapQuery}

                                    /* Q2 */
                                    WITH [PSZ_Superbill 00 Angebot_Datum] ([Artikel-Nr], Standardlieferant, Angebot_Datum) AS (
	                                    SELECT Bestellnummern.[Artikel-Nr], Bestellnummern.Standardlieferant, Bestellnummern.Angebot_Datum
	                                    FROM Bestellnummern
	                                    WHERE (((Bestellnummern.Standardlieferant)<>0))
                                    )
                                    SELECT 
	                                    Artikel.Artikelnummer AS Artikelnummer_FG, 
	                                    Artikel.[Artikel-Nr] AS [Artikel-Nr_FG], 
	                                    [PSZ_Superbill FG].Menge AS Menge_FG, 
	                                    Artikel.[Bezeichnung 1] AS Bez1_FG, 
	                                    Artikel.[Bezeichnung 2] AS Bez2_FG, 
	                                    Stücklisten.[Artikel-Nr des Bauteils] AS [Artikel-Nr_ROH], 
	                                    Stücklisten.Anzahl*[PSZ_Superbill FG].Menge AS Menge_ROH, 
	                                    Artikel_1.Artikelnummer AS Artikelnummer_ROH, 
	                                    Artikel_1.[Bezeichnung 1] AS Bez1_ROH, 
	                                    Artikel_1.[Bezeichnung 2] AS Bez2_ROH, 
	                                    Artikel_1.Stückliste AS Stückliste_ROH, 
	                                    [PSZ_Superbill 00 Angebot_Datum].Angebot_Datum AS ROH_Angebotsdatum,
	                                    /* completing columns */
	                                    CAST(NULL AS nvarchar(250)) AS Standardlieferant,
	                                    CAST(NULL AS nvarchar(250)) AS  [Bestell-Nr_ROH], 
	                                    CAST(NULL AS float) AS Einkaufspreis_ROH, 
	                                    CAST(NULL AS float) AS Kupferzahl_ROH, 
	                                    CAST(NULL AS real) AS Mindestbestellmenge_ROH, 
	                                    CAST(NULL AS int) AS Wiederbeschaffungszeitraum_ROH, 
	                                    CAST(NULL AS bit) AS [UL zertifiziert_ROH], 
	                                    CAST(NULL AS bit) AS Rahmen, 
	                                    CAST(NULL AS nvarchar(50)) AS [Rahmen-Nr], 
	                                    CAST(NULL AS real) AS Rahmenmenge, 
	                                    CAST(NULL AS datetime) AS Rahmenauslauf,
	                                    CAST(NULL AS float) AS Bestand_ROH_CZ,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_CZ,
	                                    CAST(NULL AS float) AS Bestand_ROH_AL,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_AL,
	                                    CAST(NULL AS float) AS Bestand_ROH_TN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_TN,
	                                    CAST(NULL AS float) AS Bestand_ROH_KHTN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_KHTN,
	                                    CAST(NULL AS float) AS [Bestand_ROH_SC CZ],
	                                    CAST(NULL AS float) AS [Mindesbestand_ROH_SC CZ],
	                                    CAST(NULL AS float) AS Bestand_ROH_BETN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_BETN,
	                                    CAST(NULL AS float) AS Bestand_ROH_GZTN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_GZN,
	                                    CAST(NULL AS float) AS Bestand_ROH_Obsolete,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_Obsolete

	                                    INTO [##PSZ_Superbill_ROH_Detail_{connectionId}]
                                    FROM (((#PSZ_Superbill_FG AS [PSZ_Superbill FG] INNER JOIN Artikel ON [PSZ_Superbill FG].Artikelnummer = Artikel.Artikelnummer) 
	                                    INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
	                                    INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
	                                    INNER JOIN [PSZ_Superbill 00 Angebot_Datum] ON Artikel_1.[Artikel-Nr] = [PSZ_Superbill 00 Angebot_Datum].[Artikel-Nr]
                                    ORDER BY Artikel_1.Artikelnummer;

                                    -- Q3
                                    INSERT INTO [##PSZ_Superbill_ROH_Detail_{connectionId}] ([Artikel-Nr_FG], Menge_FG, Artikelnummer_FG, Bez1_FG, Bez2_FG, [Artikel-Nr_ROH], Menge_ROH, Artikelnummer_ROH, Bez1_ROH, Bez2_ROH)
                                    SELECT 
	                                    [PSZ_Superbill ROH Detail].[Artikel-Nr_FG], 
	                                    [PSZ_Superbill ROH Detail].Menge_FG, 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez1_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez2_FG, 
	                                    Artikel.[Artikel-Nr], 
	                                    [PSZ_Superbill ROH Detail].Menge_ROH*Stücklisten.Anzahl AS Bedarf, 
	                                    Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2]
                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Stücklisten ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Stücklisten.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]
                                    ORDER BY Artikel.Artikelnummer;

                                    -- Q4
                                    DELETE FROM [##PSZ_Superbill_ROH_Detail_{connectionId}]  WHERE (((Stückliste_ROH)=1));

                                    -- Q5
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Standardlieferant = adressen.Name1, 
		                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH] = Bestellnummern.[Bestell-Nr], 
		                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH = Bestellnummern.Einkaufspreis, 
		                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH = Artikel.Kupferzahl, 
		                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH = Bestellnummern.Mindestbestellmenge, 
		                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH = Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH] = Artikel.[UL zertifiziert], 
		                                    [PSZ_Superbill ROH Detail].Rahmen = Artikel.Rahmen, 
		                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr] = Artikel.[Rahmen-Nr], 
		                                    [PSZ_Superbill ROH Detail].Rahmenmenge = Artikel.Rahmenmenge, 
		                                    [PSZ_Superbill ROH Detail].Rahmenauslauf = Artikel.Rahmenauslauf, 
		                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum = Bestellnummern.Angebot_Datum
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
		                                    INNER JOIN ((Bestellnummern 
		                                    INNER JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
		                                    ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Artikel.[Artikel-Nr]
                                    WHERE (((Bestellnummern.Standardlieferant)=1));

                                    -- Q6
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_CZ = Lager.Bestand,
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=6));

                                    -- Q7
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_CZ = [PSZ_Superbill ROH Detail].Bestand_ROH_CZ+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=66) AND ((Artikel.Warentyp)=2));


                                    -- Q8
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_AL = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=26));

                                    -- Q9
                                    UPDATE  [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_TN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=7));

                                    -- Q10
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_TN = [PSZ_Superbill ROH Detail].bestand_ROH_TN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=77) AND ((Artikel.Warentyp)=2));

                                    -- Q11
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=42));

                                    -- Q12
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN = [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=420) AND ((Artikel.Warentyp)=2));

                                    -- Q13
                                    UPDATE [PSZ_Superbill ROH Detail] 
	                                    SET [PSZ_Superbill ROH Detail].[Bestand_ROH_SC CZ] = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].[Mindesbestand_ROH_SC CZ] = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=21));

                                    -- Q14
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_BETN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=60));

                                    -- Q15
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_BETN = [PSZ_Superbill ROH Detail].Bestand_ROH_BETN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=580) AND ((Artikel.Warentyp)=2));

                                    -- Q14GZ
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=102));

                                    -- Q15GZ
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN = [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=103) AND ((Artikel.Warentyp)=2));

                                    -- Q16
                                    UPDATE [PSZ_Superbill ROH Detail] 
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=22));

                                    -- Q17
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Summe_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Summe_{connectionId}];

                                    -- Q18
                                    SELECT 
	                                    [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH, 
	                                    Sum([PSZ_Superbill ROH Detail].Menge_ROH) AS Menge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez1_ROH, [PSZ_Superbill ROH Detail].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ,
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, 
	                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, 
	                                    [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum
	                                    INTO [##PSZ_Superbill_ROH_Summe_{connectionId}]
                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
                                    GROUP BY [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, 
	                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, 
	                                    [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum;";
					// - 16
					var query_16 = $@"SELECT 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_FG AS [FG], 
	                                    [PSZ_Superbill ROH Detail].Menge_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez1_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez2_FG, 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH AS [Rohmaterial], 
	                                    [PSZ_Superbill ROH Detail].Menge_ROH, [PSZ_Superbill ROH Detail].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez2_ROH, [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ, [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN AS Bestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum
                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
                                    ORDER BY [PSZ_Superbill ROH Detail].Artikelnummer_ROH;";
					// - 17
					var query_17 = $@"SELECT 
	                                    [PSZ_Superbill ROH Summe].Artikelnummer_ROH AS [Rohmaterial#], 
	                                    [PSZ_Superbill ROH Summe].Menge_ROH, 
	                                    [PSZ_Superbill ROH Summe].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Summe].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Summe].Standardlieferant, 
	                                    [PSZ_Superbill ROH Summe].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Summe].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Summe].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Summe].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Summe].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Summe].Rahmen, 
	                                    [PSZ_Superbill ROH Summe].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Summe].Rahmenmenge,
	                                    [PSZ_Superbill ROH Summe].Rahmenauslauf,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_AL,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_KHTN AS Bestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_KHTN AS Mindestbestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_Obsolete,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Summe].ROH_Angebotsdatum
                                    FROM [##PSZ_Superbill_ROH_Summe_{connectionId}] AS [PSZ_Superbill ROH Summe]
                                    ORDER BY [PSZ_Superbill ROH Summe].Artikelnummer_ROH;";


					// - START
					//SqlTransaction transaction = sqlConnection.BeginTransaction();
					using(SqlCommand _sqlCommand = new SqlCommand(query, connection, transaction))
					{
						if(isCreate)
						{
							for(int i = 0; i < data.Count; i++)
							{
								_sqlCommand.Parameters.AddWithValue($"Artikelnummer_{i}", data[i].Artikelnummer);
								_sqlCommand.Parameters.AddWithValue($"Menge_{i}", data[i].Menge);
							}
						}
						DbExecution.ExecuteNonQuery(_sqlCommand);
					}

					// - 16
					using(SqlCommand _sqlCommand_16 = new SqlCommand(query_16, connection, transaction))
					{
						var dataTable = new DataTable();
						new SqlDataAdapter(_sqlCommand_16).Fill(dataTable);
						if(dataTable.Rows.Count > 0)
						{
							result.SuperbillROHDetails = dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHDetail(x)).ToList();
						}
					}

					// - 17
					using(SqlCommand _sqlCommand_17 = new SqlCommand(query_17, connection, transaction))
					{
						var dataTable = new DataTable();
						new SqlDataAdapter(_sqlCommand_17).Fill(dataTable);
						if(dataTable.Rows.Count > 0)
						{
							result.SuperbillROHSums = dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHSum(x)).ToList();
						}
					}

					// - END
					//transaction.Commit();
				}

				// - 
				return result;
			}
			public static Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH GetSuperbillROH_MultiQuery(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHInput> data, SqlConnection connection, SqlTransaction transaction, bool isCreate = true)
			{
				return GetSuperbillROH_MonoQuery(data, connection, transaction, isCreate);

				//var result = new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH
				//{
				//	SuperbillROHDetails = new List<Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHDetail>(),
				//	SuperbillROHSums = new List<Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHSum>()
				//};

				//if(data != null && data.Count > 0)
				//{
				//	if(data.Count <= 1050)
				//	{
				//		result = GetSuperbillROH_MonoQuery(data, connection, transaction, isCreate);
				//	}
				//	else
				//	{
				//		data = data.OrderBy(x => x.Artikelnummer).ToList();
				//		var nbBacth = data.Count / Settings.MAX_BATCH_SIZE;
				//		var nbRest = data.Count % Settings.MAX_BATCH_SIZE;
				//		var dupps = new List<string>();
				//		var duppsSum = new List<string>();
				//		for(int i = 0; i < nbBacth; i++)
				//		{
				//			var idx__ = i * Settings.MAX_BATCH_SIZE;


				//			dupps = new List<string>();
				//			duppsSum = new List<string>();
				//			var r = GetSuperbillROH_MonoQuery(data.GetRange(i * Settings.MAX_BATCH_SIZE, Settings.MAX_BATCH_SIZE), connection, transaction, isCreate);

				//			// - 2023-01-26 - update dupps
				//			foreach(var item in r.SuperbillROHDetails)
				//			{
				//				var idx = result.SuperbillROHDetails.FindIndex(x => x.Artikelnummer_ROH == item.Artikelnummer_ROH);
				//				if(idx >= 0)
				//				{
				//					dupps.Add(result.SuperbillROHDetails[idx].Artikelnummer_ROH);
				//					result.SuperbillROHDetails[idx].Menge_ROH = $"{(int.TryParse(result.SuperbillROHDetails[idx].Menge_ROH, out var _m) ? _m : 0) + (int.TryParse(item.Menge_ROH, out var _n) ? _n : 0)}";
				//				}
				//			}
				//			// - remove dupps and append the rest
				//			r.SuperbillROHDetails.RemoveAll(t => dupps.Contains(t.Artikelnummer_ROH));
				//			result.SuperbillROHDetails.AddRange(r.SuperbillROHDetails);

				//			// - 2023-01-26 - update duppsSum
				//			foreach(var item in r.SuperbillROHSums)
				//			{
				//				var idx = result.SuperbillROHSums.FindIndex(x => x.Artikelnummer_ROH == item.Artikelnummer_ROH);
				//				if(idx >= 0)
				//				{
				//					duppsSum.Add(result.SuperbillROHSums[idx].Artikelnummer_ROH);
				//					result.SuperbillROHSums[idx].Menge_ROH = $"{(int.TryParse(result.SuperbillROHSums[idx].Menge_ROH, out var _m) ? _m : 0) + (int.TryParse(item.Menge_ROH, out var _n) ? _n : 0)}";
				//				}
				//			}
				//			// - remove duppsSum and append the rest
				//			r.SuperbillROHSums.RemoveAll(t => duppsSum.Contains(t.Artikelnummer_ROH));
				//			result.SuperbillROHSums.AddRange(r.SuperbillROHSums);
				//		}

				//		// -
				//		if(nbRest > 0)
				//		{
				//			var idx__ = data.Count - nbRest;
				//			dupps = new List<string>();
				//			duppsSum = new List<string>();
				//			var r = GetSuperbillROH_MonoQuery(data.GetRange(data.Count - nbRest, nbRest), connection, transaction, isCreate);

				//			// - 2023-01-26 - update dupps
				//			foreach(var item in r.SuperbillROHDetails)
				//			{
				//				var idx = result.SuperbillROHDetails.FindIndex(x => x.Artikelnummer_ROH == item.Artikelnummer_ROH);
				//				if(idx >= 0)
				//				{
				//					dupps.Add(result.SuperbillROHDetails[idx].Artikelnummer_ROH);
				//					result.SuperbillROHDetails[idx].Menge_ROH = $"{(int.TryParse(result.SuperbillROHDetails[idx].Menge_ROH, out var _m) ? _m : 0) + (int.TryParse(item.Menge_ROH, out var _n) ? _n : 0)}";
				//				}
				//			}
				//			// - remove dupps and append the rest
				//			r.SuperbillROHDetails.RemoveAll(t => dupps.Contains(t.Artikelnummer_ROH));
				//			result.SuperbillROHDetails.AddRange(r.SuperbillROHDetails);

				//			// - 2023-01-26 - update duppsSum
				//			foreach(var item in r.SuperbillROHSums)
				//			{
				//				var idx = result.SuperbillROHSums.FindIndex(x => x.Artikelnummer_ROH == item.Artikelnummer_ROH);
				//				if(idx >= 0)
				//				{
				//					duppsSum.Add(result.SuperbillROHSums[idx].Artikelnummer_ROH);
				//					result.SuperbillROHSums[idx].Menge_ROH = $"{(int.TryParse(result.SuperbillROHSums[idx].Menge_ROH, out var _m) ? _m : 0) + (int.TryParse(item.Menge_ROH, out var _n) ? _n : 0)}";
				//				}
				//			}
				//			// - remove dupps and append the rest
				//			r.SuperbillROHSums.RemoveAll(t => duppsSum.Contains(t.Artikelnummer_ROH));
				//			result.SuperbillROHSums.AddRange(r.SuperbillROHSums);
				//		}
				//	}
				//}

				//// - 
				//return result;
			}
			public static Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH GetSuperbillROH_MonoQuery_OLD(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHInput> data, SqlConnection connection, SqlTransaction transaction, bool isCreate = true)
			{
				if(data == null || data.Count <= 0)
					return null;

				var result = new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH();


				// - 
				//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					//sqlConnection.Open();
					var connectionId = connection.ClientConnectionId.ToString().Replace('-', '_');

					string swapQuery = "";

					if(isCreate)
					{
						string insertValues = "";
						for(int i = 0; i < data.Count; i++)
						{
							insertValues += $"(@Artikelnummer_{i}, @Menge_{i}),";
						}
						insertValues = insertValues.TrimEnd(',');
						// -
						swapQuery = $"INSERT INTO #PSZ_Superbill_FG(Artikelnummer, Menge) VALUES {insertValues};";
					}
					else
					{
						swapQuery = $@"IF OBJECT_ID('tempdb..#Superbill_Auto') IS NOT NULL DROP TABLE #Superbill_Auto;
                                        SELECT Fertigung.FA_Gestartet, Artikel.Artikelnummer, Sum(Fertigung.Anzahl) AS SummevonAnzahl
                                        INTO #Superbill_Auto
                                        FROM Artikel 
	                                        INNER JOIN (SELECT * FROM Fertigung WHERE Fertigungsnummer IN ({string.Join(",", data.Select(x => x.Fertigungsnummer))})) AS Fertigung
	                                        ON Artikel.[Artikel-Nr] = Fertigung.Artikel_Nr
                                        GROUP BY Fertigung.FA_Gestartet, Artikel.Artikelnummer
                                        HAVING (((Fertigung.FA_Gestartet) Is Null Or (Fertigung.FA_Gestartet)=0));

                                        INSERT INTO #PSZ_Superbill_FG (Artikelnummer, Menge)
                                        SELECT  Superbill_Auto.Artikelnummer, Superbill_Auto.SummevonAnzahl AS Menge FROM #Superbill_Auto AS Superbill_Auto;";
					}

					// - 15 | 19
					string query = $@"/* BUTTON 15 */
                                    /* Q1 */
                                    IF OBJECT_ID('tempdb..#PSZ_Superbill_FG') IS NOT NULL DROP TABLE #PSZ_Superbill_FG;
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Detail_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Detail_{connectionId}];
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Summe_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Summe_{connectionId}];


                                    CREATE TABLE #PSZ_Superbill_FG(Artikelnummer nvarchar(20), Menge decimal(28,6));
                                    {swapQuery}

                                    /* Q2 */
                                    WITH [PSZ_Superbill 00 Angebot_Datum] ([Artikel-Nr], Standardlieferant, Angebot_Datum) AS (
	                                    SELECT Bestellnummern.[Artikel-Nr], Bestellnummern.Standardlieferant, Bestellnummern.Angebot_Datum
	                                    FROM Bestellnummern
	                                    WHERE (((Bestellnummern.Standardlieferant)<>0))
                                    )
                                    SELECT 
	                                    Artikel.Artikelnummer AS Artikelnummer_FG, 
	                                    Artikel.[Artikel-Nr] AS [Artikel-Nr_FG], 
	                                    [PSZ_Superbill FG].Menge AS Menge_FG, 
	                                    Artikel.[Bezeichnung 1] AS Bez1_FG, 
	                                    Artikel.[Bezeichnung 2] AS Bez2_FG, 
	                                    Stücklisten.[Artikel-Nr des Bauteils] AS [Artikel-Nr_ROH], 
	                                    Stücklisten.Anzahl*[PSZ_Superbill FG].Menge AS Menge_ROH, 
	                                    Artikel_1.Artikelnummer AS Artikelnummer_ROH, 
	                                    Artikel_1.[Bezeichnung 1] AS Bez1_ROH, 
	                                    Artikel_1.[Bezeichnung 2] AS Bez2_ROH, 
	                                    Artikel_1.Stückliste AS Stückliste_ROH, 
	                                    [PSZ_Superbill 00 Angebot_Datum].Angebot_Datum AS ROH_Angebotsdatum,
	                                    /* completing columns */
	                                    CAST(NULL AS nvarchar(250)) AS Standardlieferant,
	                                    CAST(NULL AS nvarchar(250)) AS  [Bestell-Nr_ROH], 
	                                    CAST(NULL AS float) AS Einkaufspreis_ROH, 
	                                    CAST(NULL AS float) AS Kupferzahl_ROH, 
	                                    CAST(NULL AS real) AS Mindestbestellmenge_ROH, 
	                                    CAST(NULL AS int) AS Wiederbeschaffungszeitraum_ROH, 
	                                    CAST(NULL AS bit) AS [UL zertifiziert_ROH], 
	                                    CAST(NULL AS bit) AS Rahmen, 
	                                    CAST(NULL AS nvarchar(50)) AS [Rahmen-Nr], 
	                                    CAST(NULL AS real) AS Rahmenmenge, 
	                                    CAST(NULL AS datetime) AS Rahmenauslauf,
	                                    CAST(NULL AS float) AS Bestand_ROH_CZ,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_CZ,
	                                    CAST(NULL AS float) AS Bestand_ROH_AL,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_AL,
	                                    CAST(NULL AS float) AS Bestand_ROH_TN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_TN,
	                                    CAST(NULL AS float) AS Bestand_ROH_KHTN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_KHTN,
	                                    CAST(NULL AS float) AS [Bestand_ROH_SC CZ],
	                                    CAST(NULL AS float) AS [Mindesbestand_ROH_SC CZ],
	                                    CAST(NULL AS float) AS Bestand_ROH_BETN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_BETN,
	                                    CAST(NULL AS float) AS Bestand_ROH_GZTN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_GZTN,
	                                    CAST(NULL AS float) AS Bestand_ROH_Obsolete,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_Obsolete

	                                    INTO [##PSZ_Superbill_ROH_Detail_{connectionId}]
                                    FROM (((#PSZ_Superbill_FG AS [PSZ_Superbill FG] INNER JOIN Artikel ON [PSZ_Superbill FG].Artikelnummer = Artikel.Artikelnummer) 
	                                    INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
	                                    INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
	                                    INNER JOIN [PSZ_Superbill 00 Angebot_Datum] ON Artikel_1.[Artikel-Nr] = [PSZ_Superbill 00 Angebot_Datum].[Artikel-Nr]
                                    ORDER BY Artikel_1.Artikelnummer;

                                    /* Q3 */
                                    INSERT INTO [##PSZ_Superbill_ROH_Detail_{connectionId}] ([Artikel-Nr_FG], Menge_FG, Artikelnummer_FG, Bez1_FG, Bez2_FG, [Artikel-Nr_ROH], Menge_ROH, Artikelnummer_ROH, Bez1_ROH, Bez2_ROH)
                                    SELECT 
	                                    [PSZ_Superbill ROH Detail].[Artikel-Nr_FG], 
	                                    [PSZ_Superbill ROH Detail].Menge_FG, 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez1_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez2_FG, 
	                                    Artikel.[Artikel-Nr], 
	                                    [PSZ_Superbill ROH Detail].Menge_ROH*Stücklisten.Anzahl AS Bedarf, 
	                                    Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2]
                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Stücklisten ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Stücklisten.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]
                                    ORDER BY Artikel.Artikelnummer;

                                    /* Q4 */
                                    DELETE FROM [##PSZ_Superbill_ROH_Detail_{connectionId}]  WHERE (((Stückliste_ROH)=1));

                                    /* Q5 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Standardlieferant = adressen.Name1, 
		                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH] = Bestellnummern.[Bestell-Nr], 
		                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH = Bestellnummern.Einkaufspreis, 
		                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH = Artikel.Kupferzahl, 
		                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH = Bestellnummern.Mindestbestellmenge, 
		                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH = Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH] = Artikel.[UL zertifiziert], 
		                                    [PSZ_Superbill ROH Detail].Rahmen = Artikel.Rahmen, 
		                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr] = Artikel.[Rahmen-Nr], 
		                                    [PSZ_Superbill ROH Detail].Rahmenmenge = Artikel.Rahmenmenge, 
		                                    [PSZ_Superbill ROH Detail].Rahmenauslauf = Artikel.Rahmenauslauf, 
		                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum = Bestellnummern.Angebot_Datum
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
		                                    INNER JOIN ((Bestellnummern 
		                                    INNER JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
		                                    ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Artikel.[Artikel-Nr]
                                    WHERE (((Bestellnummern.Standardlieferant)=1));

                                    /* Q6 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_CZ = Lager.Bestand,
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=6));

                                    /* Q7 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_CZ = [PSZ_Superbill ROH Detail].Bestand_ROH_CZ+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=66) AND ((Artikel.Warentyp)=2));


                                    /* Q8 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_AL = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=26));

                                    /* Q9 */
                                    UPDATE  [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_TN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=7));

                                    /* Q10 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_TN = [PSZ_Superbill ROH Detail].bestand_ROH_TN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=77) AND ((Artikel.Warentyp)=2));

                                    /* Q11 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=42));

                                    /* Q12 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN = [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=420) AND ((Artikel.Warentyp)=2));

                                    /* Q13 */
                                    UPDATE [PSZ_Superbill ROH Detail] 
	                                    SET [PSZ_Superbill ROH Detail].[Bestand_ROH_SC CZ] = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].[Mindesbestand_ROH_SC CZ] = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=21));

                                    /* Q14 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_BETN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=60));

                                    /* Q15 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_BETN = [PSZ_Superbill ROH Detail].Bestand_ROH_BETN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=580) AND ((Artikel.Warentyp)=2));

                                    /* Q14GZ */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=102));

                                    /* Q15GZ */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN = [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=103) AND ((Artikel.Warentyp)=2));

                                    /* Q16 */
                                    UPDATE [PSZ_Superbill ROH Detail] 
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=22));

                                    /* Q17 */
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Summe_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Summe_{connectionId}];

                                    /* Q18 */
                                    SELECT 
	                                    [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH, 
	                                    Sum([PSZ_Superbill ROH Detail].Menge_ROH) AS Menge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez1_ROH, [PSZ_Superbill ROH Detail].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ,
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, 
	                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, 
	                                    [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum
	                                    INTO [##PSZ_Superbill_ROH_Summe_{connectionId}]
                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
                                    GROUP BY [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, 
	                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, 
	                                    [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum;";
					// - 16
					var query_16 = $@"SELECT 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_FG AS [FG], 
	                                    [PSZ_Superbill ROH Detail].Menge_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez1_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez2_FG, 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH AS [Rohmaterial], 
	                                    [PSZ_Superbill ROH Detail].Menge_ROH, [PSZ_Superbill ROH Detail].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez2_ROH, [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ, [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN AS Bestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum
                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
                                    ORDER BY [PSZ_Superbill ROH Detail].Artikelnummer_ROH;";
					// - 17
					var query_17 = $@"SELECT 
	                                    [PSZ_Superbill ROH Summe].Artikelnummer_ROH AS [Rohmaterial#], 
	                                    [PSZ_Superbill ROH Summe].Menge_ROH, 
	                                    [PSZ_Superbill ROH Summe].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Summe].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Summe].Standardlieferant, 
	                                    [PSZ_Superbill ROH Summe].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Summe].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Summe].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Summe].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Summe].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Summe].Rahmen, 
	                                    [PSZ_Superbill ROH Summe].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Summe].Rahmenmenge,
	                                    [PSZ_Superbill ROH Summe].Rahmenauslauf,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_AL,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_KHTN AS Bestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_KHTN AS Mindestbestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_Obsolete,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Summe].ROH_Angebotsdatum
                                    FROM [##PSZ_Superbill_ROH_Summe_{connectionId}] AS [PSZ_Superbill ROH Summe]
                                    ORDER BY [PSZ_Superbill ROH Summe].Artikelnummer_ROH;";


					// - START
					//SqlTransaction transaction = sqlConnection.BeginTransaction();
					using(SqlCommand _sqlCommand = new SqlCommand(query, connection, transaction))
					{
						if(isCreate)
						{
							for(int i = 0; i < data.Count; i++)
							{
								_sqlCommand.Parameters.AddWithValue($"Artikelnummer_{i}", data[i].Artikelnummer);
								_sqlCommand.Parameters.AddWithValue($"Menge_{i}", data[i].Menge);
							}
						}
						DbExecution.ExecuteNonQuery(_sqlCommand);
					}

					// - 16
					using(SqlCommand _sqlCommand_16 = new SqlCommand(query_16, connection, transaction))
					{
						var dataTable = new DataTable();
						new SqlDataAdapter(_sqlCommand_16).Fill(dataTable);
						if(dataTable.Rows.Count > 0)
						{
							result.SuperbillROHDetails = dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHDetail(x)).ToList();
						}
					}

					// - 17
					using(SqlCommand _sqlCommand_17 = new SqlCommand(query_17, connection, transaction))
					{
						var dataTable = new DataTable();
						new SqlDataAdapter(_sqlCommand_17).Fill(dataTable);
						if(dataTable.Rows.Count > 0)
						{
							result.SuperbillROHSums = dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHSum(x)).ToList();
						}
					}

					// - END
					//transaction.Commit();
				}

				// - 
				return result;
			}
			public static Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH GetSuperbillROH_MonoQuery(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHInput> data, SqlConnection connection, SqlTransaction transaction, bool isCreate = true)
			{
				if(data == null || data.Count <= 0)
					return null;

				var result = new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH();

				// - 
				var connectionId = connection.ClientConnectionId.ToString().Replace('-', '_');

				string swapQuery = "";

				if(isCreate)
				{
					var LIMIT = 1000;
					if(data?.Count > LIMIT)
					{
						var nbBacth = data.Count / LIMIT;
						var nbRest = data.Count % LIMIT;
						for(int i = 0; i < nbBacth; i++)
						{
							swapQuery += $"INSERT INTO #PSZ_Superbill_FG(Artikelnummer, Menge) VALUES {string.Join(",", data.GetRange(i * LIMIT, LIMIT).Select(x => $"('{x.Artikelnummer}', {x.Menge})"))};\n";
						}
						swapQuery += $"INSERT INTO #PSZ_Superbill_FG(Artikelnummer, Menge) VALUES {string.Join(",", data.GetRange(data.Count - nbRest, nbRest).Select(x => $"('{x.Artikelnummer}', {x.Menge})"))};";
					}
					else
					{
						swapQuery = $"INSERT INTO #PSZ_Superbill_FG(Artikelnummer, Menge) VALUES {string.Join(",", data.Select(x => $"('{x.Artikelnummer}', {x.Menge})"))};";
					}
				}
				else
				{
					swapQuery = $@"IF OBJECT_ID('tempdb..#Superbill_Auto') IS NOT NULL DROP TABLE #Superbill_Auto;
                                        SELECT Fertigung.FA_Gestartet, Artikel.Artikelnummer, Sum(Fertigung.Anzahl) AS SummevonAnzahl
                                        INTO #Superbill_Auto
                                        FROM Artikel 
	                                        INNER JOIN (SELECT * FROM Fertigung WHERE Fertigungsnummer IN ({string.Join(",", data.Select(x => x.Fertigungsnummer))})) AS Fertigung
	                                        ON Artikel.[Artikel-Nr] = Fertigung.Artikel_Nr
                                        GROUP BY Fertigung.FA_Gestartet, Artikel.Artikelnummer
                                        HAVING (((Fertigung.FA_Gestartet) Is Null Or (Fertigung.FA_Gestartet)=0));

                                        INSERT INTO #PSZ_Superbill_FG (Artikelnummer, Menge)
                                        SELECT  Superbill_Auto.Artikelnummer, Superbill_Auto.SummevonAnzahl AS Menge FROM #Superbill_Auto AS Superbill_Auto;";
				}

				// - 15 | 19
				string query = $@"/* BUTTON 15 */
                                    /* Q1 */
                                    IF OBJECT_ID('tempdb..#PSZ_Superbill_FG') IS NOT NULL DROP TABLE #PSZ_Superbill_FG;
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Detail_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Detail_{connectionId}];
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Summe_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Summe_{connectionId}];


                                    CREATE TABLE #PSZ_Superbill_FG(Artikelnummer nvarchar(20), Menge decimal(28,6));
                                    {swapQuery}

                                    /* Q2 */
                                    WITH [PSZ_Superbill 00 Angebot_Datum] ([Artikel-Nr], Standardlieferant, Angebot_Datum) AS (
	                                    SELECT Bestellnummern.[Artikel-Nr], Bestellnummern.Standardlieferant, Bestellnummern.Angebot_Datum
	                                    FROM Bestellnummern
	                                    WHERE (((Bestellnummern.Standardlieferant)<>0))
                                    )
                                    SELECT 
	                                    Artikel.Artikelnummer AS Artikelnummer_FG, 
	                                    Artikel.[Artikel-Nr] AS [Artikel-Nr_FG], 
	                                    [PSZ_Superbill FG].Menge AS Menge_FG, 
	                                    Artikel.[Bezeichnung 1] AS Bez1_FG, 
	                                    Artikel.[Bezeichnung 2] AS Bez2_FG, 
	                                    Stücklisten.[Artikel-Nr des Bauteils] AS [Artikel-Nr_ROH], 
	                                    Stücklisten.Anzahl*[PSZ_Superbill FG].Menge AS Menge_ROH, 
	                                    Artikel_1.Artikelnummer AS Artikelnummer_ROH, 
	                                    Artikel_1.[Bezeichnung 1] AS Bez1_ROH, 
	                                    Artikel_1.[Bezeichnung 2] AS Bez2_ROH, 
	                                    Artikel_1.Stückliste AS Stückliste_ROH, 
	                                    [PSZ_Superbill 00 Angebot_Datum].Angebot_Datum AS ROH_Angebotsdatum,
	                                    /* completing columns */
	                                    CAST(NULL AS nvarchar(250)) AS Standardlieferant,
	                                    CAST(NULL AS nvarchar(250)) AS  [Bestell-Nr_ROH], 
	                                    CAST(NULL AS float) AS Einkaufspreis_ROH, 
	                                    CAST(NULL AS float) AS Kupferzahl_ROH, 
	                                    CAST(NULL AS real) AS Mindestbestellmenge_ROH, 
	                                    CAST(NULL AS int) AS Wiederbeschaffungszeitraum_ROH, 
	                                    CAST(NULL AS bit) AS [UL zertifiziert_ROH], 
	                                    CAST(NULL AS bit) AS Rahmen, 
	                                    CAST(NULL AS nvarchar(50)) AS [Rahmen-Nr], 
	                                    CAST(NULL AS real) AS Rahmenmenge, 
	                                    CAST(NULL AS datetime) AS Rahmenauslauf,
	                                    CAST(NULL AS float) AS Bestand_ROH_CZ,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_CZ,
	                                    CAST(NULL AS float) AS Bestand_ROH_AL,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_AL,
	                                    CAST(NULL AS float) AS Bestand_ROH_TN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_TN,
	                                    CAST(NULL AS float) AS Bestand_ROH_KHTN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_KHTN,
	                                    CAST(NULL AS float) AS [Bestand_ROH_SC CZ],
	                                    CAST(NULL AS float) AS [Mindesbestand_ROH_SC CZ],
	                                    CAST(NULL AS float) AS Bestand_ROH_BETN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_BETN,
	                                    CAST(NULL AS float) AS Bestand_ROH_GZTN,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_GZTN,
	                                    CAST(NULL AS float) AS Bestand_ROH_Obsolete,
	                                    CAST(NULL AS float) AS Mindestbestand_ROH_Obsolete

	                                    INTO [##PSZ_Superbill_ROH_Detail_{connectionId}]
                                    FROM (((#PSZ_Superbill_FG AS [PSZ_Superbill FG] INNER JOIN Artikel ON [PSZ_Superbill FG].Artikelnummer = Artikel.Artikelnummer) 
	                                    INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
	                                    INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
	                                    INNER JOIN [PSZ_Superbill 00 Angebot_Datum] ON Artikel_1.[Artikel-Nr] = [PSZ_Superbill 00 Angebot_Datum].[Artikel-Nr]
                                    ORDER BY Artikel_1.Artikelnummer;

                                    /* Q3 */
                                    INSERT INTO [##PSZ_Superbill_ROH_Detail_{connectionId}] ([Artikel-Nr_FG], Menge_FG, Artikelnummer_FG, Bez1_FG, Bez2_FG, [Artikel-Nr_ROH], Menge_ROH, Artikelnummer_ROH, Bez1_ROH, Bez2_ROH)
                                    SELECT 
	                                    [PSZ_Superbill ROH Detail].[Artikel-Nr_FG], 
	                                    [PSZ_Superbill ROH Detail].Menge_FG, 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez1_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez2_FG, 
	                                    Artikel.[Artikel-Nr], 
	                                    [PSZ_Superbill ROH Detail].Menge_ROH*Stücklisten.Anzahl AS Bedarf, 
	                                    Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2]
                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Stücklisten ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Stücklisten.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]
                                    ORDER BY Artikel.Artikelnummer;

                                    /* Q4 */
                                    DELETE FROM [##PSZ_Superbill_ROH_Detail_{connectionId}]  WHERE (((Stückliste_ROH)=1));

                                    /* Q5 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Standardlieferant = adressen.Name1, 
		                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH] = Bestellnummern.[Bestell-Nr], 
		                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH = Bestellnummern.Einkaufspreis, 
		                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH = Artikel.Kupferzahl, 
		                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH = Bestellnummern.Mindestbestellmenge, 
		                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH = Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH] = Artikel.[UL zertifiziert], 
		                                    [PSZ_Superbill ROH Detail].Rahmen = Artikel.Rahmen, 
		                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr] = Artikel.[Rahmen-Nr], 
		                                    [PSZ_Superbill ROH Detail].Rahmenmenge = Artikel.Rahmenmenge, 
		                                    [PSZ_Superbill ROH Detail].Rahmenauslauf = Artikel.Rahmenauslauf, 
		                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum = Bestellnummern.Angebot_Datum
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
		                                    INNER JOIN ((Bestellnummern 
		                                    INNER JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
		                                    ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Artikel.[Artikel-Nr]
                                    WHERE (((Bestellnummern.Standardlieferant)=1));

                                    /* Q6 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_CZ = Lager.Bestand,
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=6));

                                    /* Q7 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_CZ = [PSZ_Superbill ROH Detail].Bestand_ROH_CZ+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=66) AND ((Artikel.Warentyp)=2));


                                    /* Q8 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_AL = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=26));

                                    /* Q9 */
                                    UPDATE  [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_TN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=7));

                                    /* Q10 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_TN = [PSZ_Superbill ROH Detail].bestand_ROH_TN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=77) AND ((Artikel.Warentyp)=2));

                                    /* Q11 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=42));

                                    /* Q12 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN = [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=420) AND ((Artikel.Warentyp)=2));

                                    /* Q13 */
                                    UPDATE [PSZ_Superbill ROH Detail] 
	                                    SET [PSZ_Superbill ROH Detail].[Bestand_ROH_SC CZ] = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].[Mindesbestand_ROH_SC CZ] = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=21));

                                    /* Q14 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_BETN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=60));

                                    /* Q15 */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_BETN = [PSZ_Superbill ROH Detail].Bestand_ROH_BETN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=580) AND ((Artikel.Warentyp)=2));

                                    /* Q14GZ */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=102));

                                    /* Q15GZ */
                                    UPDATE [PSZ_Superbill ROH Detail]
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN = [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN+Lager.Bestand
	                                    FROM ([##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr] 
                                    WHERE (((Lager.Lagerort_id)=103) AND ((Artikel.Warentyp)=2));

                                    /* Q16 */
                                    UPDATE [PSZ_Superbill ROH Detail] 
	                                    SET [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete = Lager.Bestand, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete = Lager.Mindestbestand
	                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail] 
	                                    INNER JOIN Lager ON [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH] = Lager.[Artikel-Nr]
                                    WHERE (((Lager.Lagerort_id)=22));

                                    /* Q17 */
                                    IF OBJECT_ID('tempdb..[##PSZ_Superbill_ROH_Summe_{connectionId}]') IS NOT NULL DROP TABLE [##PSZ_Superbill_ROH_Summe_{connectionId}];

                                    /* Q18 */
                                    SELECT 
	                                    [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH, 
	                                    Sum([PSZ_Superbill ROH Detail].Menge_ROH) AS Menge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez1_ROH, [PSZ_Superbill ROH Detail].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ,
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, 
	                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, 
	                                    [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum
	                                    INTO [##PSZ_Superbill_ROH_Summe_{connectionId}]
                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
                                    GROUP BY [PSZ_Superbill ROH Detail].[Artikel-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, 
	                                    [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, 
	                                    [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum;";
				// - 16
				var query_16 = $@"SELECT 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_FG AS [FG], 
	                                    [PSZ_Superbill ROH Detail].Menge_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez1_FG, 
	                                    [PSZ_Superbill ROH Detail].Bez2_FG, 
	                                    [PSZ_Superbill ROH Detail].Artikelnummer_ROH AS [Rohmaterial], 
	                                    [PSZ_Superbill ROH Detail].Menge_ROH, [PSZ_Superbill ROH Detail].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Detail].Bez2_ROH, [PSZ_Superbill ROH Detail].Standardlieferant, 
	                                    [PSZ_Superbill ROH Detail].[Bestell-Nr_ROH], [PSZ_Superbill ROH Detail].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Detail].Kupferzahl_ROH, [PSZ_Superbill ROH Detail].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Detail].Wiederbeschaffungszeitraum_ROH, [PSZ_Superbill ROH Detail].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_CZ, [PSZ_Superbill ROH Detail].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_AL, [PSZ_Superbill ROH Detail].Bestand_ROH_KHTN AS Bestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Detail].Bestand_ROH_BETN, [PSZ_Superbill ROH Detail].Bestand_ROH_GZTN, [PSZ_Superbill ROH Detail].Bestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].Rahmen, [PSZ_Superbill ROH Detail].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Detail].Rahmenmenge, [PSZ_Superbill ROH Detail].Rahmenauslauf, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_CZ, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_AL, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_KHTN, 
	                                    [PSZ_Superbill ROH Detail].Mindestbestand_ROH_BETN, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_GZTN, [PSZ_Superbill ROH Detail].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Detail].ROH_Angebotsdatum
                                    FROM [##PSZ_Superbill_ROH_Detail_{connectionId}] AS [PSZ_Superbill ROH Detail]
                                    ORDER BY [PSZ_Superbill ROH Detail].Artikelnummer_ROH;";
				// - 17
				var query_17 = $@"SELECT 
	                                    [PSZ_Superbill ROH Summe].Artikelnummer_ROH AS [Rohmaterial#], 
	                                    [PSZ_Superbill ROH Summe].Menge_ROH, 
	                                    [PSZ_Superbill ROH Summe].Bez1_ROH, 
	                                    [PSZ_Superbill ROH Summe].Bez2_ROH, 
	                                    [PSZ_Superbill ROH Summe].Standardlieferant, 
	                                    [PSZ_Superbill ROH Summe].[Bestell-Nr_ROH], 
	                                    [PSZ_Superbill ROH Summe].Einkaufspreis_ROH, 
	                                    [PSZ_Superbill ROH Summe].Kupferzahl_ROH, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestellmenge_ROH, 
	                                    [PSZ_Superbill ROH Summe].Wiederbeschaffungszeitraum_ROH, 
	                                    [PSZ_Superbill ROH Summe].[UL zertifiziert_ROH], 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_AL, 
	                                    [PSZ_Superbill ROH Summe].Rahmen, 
	                                    [PSZ_Superbill ROH Summe].[Rahmen-Nr], 
	                                    [PSZ_Superbill ROH Summe].Rahmenmenge,
	                                    [PSZ_Superbill ROH Summe].Rahmenauslauf,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_TN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_AL,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_CZ, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_KHTN AS Bestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_KHTN AS Mindestbestand_ROH_WS, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_BETN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_GZTN, 
	                                    [PSZ_Superbill ROH Summe].Bestand_ROH_Obsolete,
	                                    [PSZ_Superbill ROH Summe].Mindestbestand_ROH_Obsolete, 
	                                    [PSZ_Superbill ROH Summe].ROH_Angebotsdatum
                                    FROM [##PSZ_Superbill_ROH_Summe_{connectionId}] AS [PSZ_Superbill ROH Summe]
                                    ORDER BY [PSZ_Superbill ROH Summe].Artikelnummer_ROH;";


				// - START
				using(SqlCommand _sqlCommand = new SqlCommand(query, connection, transaction))
				{
					DbExecution.ExecuteNonQuery(_sqlCommand);
				}

				// - 16
				using(SqlCommand _sqlCommand_16 = new SqlCommand(query_16, connection, transaction))
				{
					var dataTable = new DataTable();
					new SqlDataAdapter(_sqlCommand_16).Fill(dataTable);
					if(dataTable.Rows.Count > 0)
					{
						result.SuperbillROHDetails = dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHDetail(x)).ToList();
					}
				}

				// - 17
				using(SqlCommand _sqlCommand_17 = new SqlCommand(query_17, connection, transaction))
				{
					var dataTable = new DataTable();
					new SqlDataAdapter(_sqlCommand_17).Fill(dataTable);
					if(dataTable.Rows.Count > 0)
					{
						result.SuperbillROHSums = dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHSum(x)).ToList();
					}
				}

				// - END

				// - 
				return result;
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_DBzuArtikel> GetDBzuArtikel(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"WITH cte AS (
                                        SELECT 
	                                        Artikel.Artikelnummer, 
	                                        [angebotene Artikel].OriginalAnzahl AS Verkauft, 
	                                        Angebote.Datum
                                        FROM (Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
                                        WHERE (((Artikel.Artikelnummer)<>'Reparatur' And (Artikel.Artikelnummer)<>'Technik' And (Artikel.Artikelnummer)<>'UMBAU' 
	                                        And (Artikel.Artikelnummer)<>'VP-Universalverpackung') 
	                                        AND ((Angebote.Datum)>=@dateFrom 
	                                        And (Angebote.Datum)<=@dateTill) 
	                                        AND ((Angebote.Typ)='Lieferschein') AND ((Artikel.Warengruppe)='EF') AND ((Artikel.Kupferbasis)=150))
                                        /* ORDER BY Artikel.Artikelnummer */
                                        )
                                        SELECT 
	                                        [Deckungsbeitrag verkaufte Artikel].Artikelnummer, 
	                                        Sum([Deckungsbeitrag verkaufte Artikel].Verkauft) AS [verkaufte Artikel], 
	                                        Round([DB I mit],6) AS [DB I]
                                        FROM cte as [Deckungsbeitrag verkaufte Artikel] 
	                                        INNER JOIN [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel] 
	                                        ON [Deckungsbeitrag verkaufte Artikel].Artikelnummer = [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel].Artikelnummer
                                        GROUP BY [Deckungsbeitrag verkaufte Artikel].Artikelnummer, Round([DB I mit], 6)
                                        ORDER BY [Deckungsbeitrag verkaufte Artikel].Artikelnummer;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
					sqlCommand.Parameters.AddWithValue("dateTill", dateTill);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_DBzuArtikel(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_BestellungProDisponent> GetBestellungProDsiponent(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"WITH [PSZ_Disposition Bestellungen pro Disponent Vorberechnung] (Typ, Nummer, Disponent, Lieferant, AnzahlVonPos, [Bestellung-Nr]) AS
                                        (
                                        SELECT 
	                                        Bestellungen.Typ, Bestellungen.Bearbeiter AS Nummer, 
	                                        u.Name AS Disponent, Bestellungen.[Vorname/NameFirma] AS Lieferant, 
	                                        Count(Bestellungen.[Vorname/NameFirma]) AS AnzahlVonPos, 
	                                        Bestellungen.[Bestellung-Nr]
                                        FROM ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        INNER JOIN (Bestellungen INNER JOIN [user] U ON Bestellungen.Bearbeiter = u.Nummer) 
	                                        ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr
                                        WHERE (((Bestellungen.Datum)>=@dateFrom And (Bestellungen.Datum)<=@dateTill))
                                        GROUP BY Bestellungen.Typ, Bestellungen.Bearbeiter,u.Name, Bestellungen.[Vorname/NameFirma], 
	                                        Bestellungen.[Bestellung-Nr]
                                        HAVING (((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf'))
                                        --ORDER BY Bestellungen.Bearbeiter, Bestellungen.[Vorname/NameFirma]
                                        )
                                        SELECT 
	                                        [PSZ_Disposition Bestellungen pro Disponent Vorberechnung].Nummer, 
	                                        [PSZ_Disposition Bestellungen pro Disponent Vorberechnung].Disponent, 
	                                        [PSZ_Disposition Bestellungen pro Disponent Vorberechnung].Lieferant, 
	                                        Count([PSZ_Disposition Bestellungen pro Disponent Vorberechnung].[Bestellung-Nr]) AS AnzahlBestellungen, 
	                                        Sum([PSZ_Disposition Bestellungen pro Disponent Vorberechnung].AnzahlVonPos) AS SummePositionen
                                        FROM [PSZ_Disposition Bestellungen pro Disponent Vorberechnung]
                                        GROUP BY [PSZ_Disposition Bestellungen pro Disponent Vorberechnung].Nummer, 
	                                        [PSZ_Disposition Bestellungen pro Disponent Vorberechnung].Disponent, 
	                                        [PSZ_Disposition Bestellungen pro Disponent Vorberechnung].Lieferant;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
					sqlCommand.Parameters.AddWithValue("dateTill", dateTill);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_BestellungProDisponent(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_Rahmen> GetRahmenlist()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"WITH List_Bestellung_Rahmnummet_Total (InfoRahmennummer, Bestellt) AS
                                        (
	                                        SELECT 
		                                        [bestellte Artikel].InfoRahmennummer, Sum([bestellte Artikel].[Start Anzahl]) AS Bestellt
	                                        FROM Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
	                                        GROUP BY [bestellte Artikel].InfoRahmennummer, Bestellungen.Typ, Bestellungen.Rahmenbestellung
	                                        HAVING ((([bestellte Artikel].InfoRahmennummer) Is Not Null) AND InfoRahmennummer<>'' AND ((Bestellungen.Typ)='Bestellung') 
		                                        AND ((Bestellungen.Rahmenbestellung)=0))
                                        )
                                        SELECT DISTINCT [Artikel-Nr],Artikelnummer, [Bezeichnung 1], [Bezeichnung 2], 
	                                        Case 
	                                            When ([Rahmen-Nr] = InfoRahmennummer) then Rahmen
	                                            When ([Rahmen-Nr2] = InfoRahmennummer) then Rahmen2 
                                            end As Rahmen, 
                                            Case
	                                            When ([Rahmen-Nr] = InfoRahmennummer) then [Rahmen-Nr] 
	                                            When ([Rahmen-Nr2] = InfoRahmennummer) then [Rahmen-Nr2]
                                            end As [Rahmen-Nr],
                                            IsNULL(Case
	                                            When ([Rahmen-Nr] = InfoRahmennummer) then Rahmenmenge
	                                            When ([Rahmen-Nr2] = InfoRahmennummer) then Rahmenmenge2
                                            end,0) As Rahmenmenge, 
                                            IsNULL(List_Bestellung_Rahmnummet_Total.Bestellt, 0) Bestellt,
                                            Case
	                                            When ([Rahmen-Nr] = InfoRahmennummer) then Rahmenauslauf 
	                                            When ([Rahmen-Nr2] = InfoRahmennummer) then Rahmenauslauf2
                                            End as Rahmenauslauf, 
                                            IsNULL(Case
	                                            When ([Rahmen-Nr] = InfoRahmennummer) then IsNULL(Rahmenmenge,0) - IsNULL(Bestellt, 0)
	                                            When ([Rahmen-Nr2] = InfoRahmennummer) then IsNULL(Rahmenmenge2,0) - IsNULL(Bestellt, 0)
                                            End, 0) as Rahmenrest
                                            FROM Artikel LEFT JOIN List_Bestellung_Rahmnummet_Total ON ([Rahmen-Nr] = InfoRahmennummer OR [Rahmen-Nr2] = InfoRahmennummer)
                                            WHERE Warengruppe='ROH' AND (IsNULL([Rahmen-Nr], '')<>'' OR IsNULL([Rahmen-Nr2], '')<>'')
                                        ORDER BY Rahmenauslauf DESC;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_Rahmen(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjektdatenDetails> GetProjectMeldungAnalyse()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"SELECT PSZ_Projektdaten_Details.Erstanlage, PSZ_Projektdaten_Details.[Projekt-Nr], PSZ_Projektdaten_Details.Artikelnummer, PSZ_Projektdaten_Details.Kontakt_AV_PSZ
                                        FROM PSZ_Projektdaten_Details 
                                        GROUP BY PSZ_Projektdaten_Details.Erstanlage, PSZ_Projektdaten_Details.[Projekt-Nr], PSZ_Projektdaten_Details.Artikelnummer, PSZ_Projektdaten_Details.Kontakt_AV_PSZ;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_ProjektdatenDetails(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_InventurROH> GetInventurROH(int minStock)
			{
				if(minStock < 0)
					minStock = 0;

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"SELECT 
	                                        Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Lager.Bestand,
	                                        IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0) AS EK, Lager.Bestand*IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0) AS EK_Summe, 
	                                        CAST(Artikel.Größe AS DECIMAL(38,15)) AS Gewicht, [Größe]*[Bestand]/1000 AS Gesamtgewicht, Artikel.Zolltarif_nr, Artikel.Ursprungsland, 
	                                        Bestellnummern.[Lieferanten-Nr], adressen.Name1, Bestellnummern.[Bestell-Nr], Lagerorte.Lagerort_id, Lagerorte.Lagerort
                                        FROM ((((Lager LEFT JOIN Preisgruppen ON Lager.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Lagerorte 
	                                        ON Lager.Lagerort_id = Lagerorte.Lagerort_id) INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        LEFT JOIN Bestellnummern ON Lager.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN adressen 
	                                        ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
                                        WHERE (((Lager.Bestand)>=@minStock) AND ((Preisgruppen.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)=1))
                                        ORDER BY Artikel.Artikelnummer;";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("minStock", minStock);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_InventurROH(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationInData> GetVKSimulationData(string articleNumber, decimal anteil)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					var tablename = $"[#PSZ_AV_Auswahl_Simulation_VK_Update_{articleNumber.Trim()}]";
					sqlConnection.Open();

					// -
					string query = $@"
                                        IF OBJECT_ID('tempdb..#PSZ_AV_VK_Analyse') IS NOT NULL DROP TABLE #PSZ_AV_VK_Analyse;
                                        IF OBJECT_ID('tempdb..#NEUE_ListeVKMitProzenanteil') IS NOT NULL DROP TABLE #NEUE_ListeVKMitProzenanteil;
                                        IF OBJECT_ID('tempdb..#PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung') IS NOT NULL DROP TABLE #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung;

                                        /* Q1 */
                                        SELECT 
	                                        Artikel.Artikelnummer, Artikel.[Bezeichnung 1], 
	                                        Artikel.[Bezeichnung 2], Artikel.[Bezeichnung 3], 
	                                        Artikel.Index_Kunde, Artikel.Index_Kunde_Datum, 
	                                        999.999 AS Materialkosten, Artikel.Produktionszeit, 
	                                        Artikel.Stundensatz, Preisgruppen.Verkaufspreis, Artikel.DEL, 
	                                        Artikel.[DEL fixiert], Artikel.[Cu-Gewicht], Artikel.[VK-Festpreis], 
	                                        (Round(IIf(Artikel.[VK-Festpreis]=0,((Artikel.DEL*1.01)-Artikel.Kupferbasis)/100*Artikel.[Cu-Gewicht],0),2))*[Preiseinheit] AS Kupferzuschlag, 
	                                        (Round(IIf(Artikel.[VK-Festpreis]=0,((Artikel.DEL*1.01)-Artikel.Kupferbasis)/100*Artikel.[Cu-Gewicht],0),2))*[Preiseinheit]+[Verkaufspreis] AS [VK inkl Kupfer], 
	                                        Artikel.Preiseinheit, CAST(Artikel.Größe AS DECIMAL(38,15)) AS [Gewicht in gr], 
	                                        Artikel.Ursprungsland, Artikel.Zolltarif_nr, Artikel.Freigabestatus, 
	                                        0 AS Jahresmenge, 0 AS Jahresumsatz, Artikel.[Artikel-Nr], Artikel.Kupferbasis 
	                                        INTO #PSZ_AV_VK_Analyse --[PSZ_AV_VK_Analyse]
                                        FROM Artikel INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                        WHERE (((Artikel.Artikelnummer) Like (@articleNumber +'%')) AND ((Artikel.aktiv)=1) AND ((Artikel.Stückliste)=1))
                                        ORDER BY Artikel.Artikelnummer;

                                        /* Q2 */
                                        SELECT 
	                                        [PSZ_AV VK Analyse].Artikelnummer, [PSZ_AV VK Analyse].[Bezeichnung 1], 
	                                        [PSZ_AV VK Analyse].[Bezeichnung 2], [PSZ_AV VK Analyse].Verkaufspreis AS AlteVK, 
	                                        [PSZ_AV VK Analyse].DEL, Round([Verkaufspreis]+[Verkaufspreis]*@anteil/100,2) AS AktuelleVK, 
	                                        [Verkaufspreis]*@anteil/100 AS [Anteilberechen], [PSZ_AV VK Analyse].[DEL fixiert], 
	                                        CAST(@anteil AS nvarchar(20)) + '%' AS Prozent, [PSZ_AV VK Analyse].[Artikel-Nr], 
	                                        [PSZ_AV VK Analyse].[VK-Festpreis], [PSZ_AV VK Analyse].[Cu-Gewicht], 
	                                        [PSZ_AV VK Analyse].Kupferbasis 
	                                        INTO #NEUE_ListeVKMitProzenanteil -- INTO NEUE_ListeVKMitProzenanteil
                                        FROM #PSZ_AV_VK_Analyse AS [PSZ_AV VK Analyse];


                                        /* Q3 */
                                        SELECT 
	                                        [angebotene Artikel].Nr, Artikel.Artikelnummer, Artikel.DEL, 
	                                        Angebote.Typ, Angebote.[Projekt-Nr], Angebote.[Angebot-Nr] 
	                                        INTO #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung -- [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung]
                                        FROM (([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr) 
	                                        INNER JOIN #NEUE_ListeVKMitProzenanteil AS NEUE_ListeVKMitProzenanteil ON Artikel.[Artikel-Nr] = NEUE_ListeVKMitProzenanteil.[Artikel-Nr]
                                        WHERE (((Angebote.Typ)='Auftragsbestätigung') AND ((Artikel.[DEL fixiert])=1) AND ((IsNULL([angebotene Artikel].[AB Pos zu RA Pos],0)<=0) AND ([angebotene Artikel].erledigt_pos)=0 ) 
	                                        AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0));
	
                                        /* Q4 */
                                        SELECT 
	                                        Artikel.[Artikel-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], 
	                                        Artikel.DEL, Artikel.[Bezeichnung 2], Preisgruppen.Verkaufspreis AS VK, 
	                                        NEUE_ListeVKMitProzenanteil.AktuelleVK AS VK_Simulation, 
	                                        [Preisgruppen].[Verkaufspreis]+Round(IIf([Artikel].[VK-Festpreis]=0,(([Artikel].[DEL]*1.01)-[Artikel].[Kupferbasis])/100*[Artikel].[Cu-Gewicht],0),2) AS VKCU, 
	                                        Round([NEUE_ListeVKMitProzenanteil].[AktuelleVK]+Round(IIf([NEUE_ListeVKMitProzenanteil].[VK-Festpreis]=0,(([Artikel].[DEL]*1.01)-[NEUE_ListeVKMitProzenanteil].[Kupferbasis])/100*[NEUE_ListeVKMitProzenanteil].[Cu-Gewicht],0),2),2) AS VKCU_Simulation, 
	                                        NEUE_ListeVKMitProzenanteil.[Anteilberechen], NEUE_ListeVKMitProzenanteil.Prozent, 0 AS [Update] 
	                                        /*INTO {tablename}*/
                                        FROM #NEUE_ListeVKMitProzenanteil AS NEUE_ListeVKMitProzenanteil 
	                                        INNER JOIN (Artikel INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                        ON NEUE_ListeVKMitProzenanteil.[Artikel-Nr] = Preisgruppen.[Artikel-Nr];


                                        IF OBJECT_ID('tempdb..#PSZ_AV_VK_Analyse') IS NOT NULL DROP TABLE #PSZ_AV_VK_Analyse;
                                        IF OBJECT_ID('tempdb..#NEUE_ListeVKMitProzenanteil') IS NOT NULL DROP TABLE #NEUE_ListeVKMitProzenanteil;
                                        IF OBJECT_ID('tempdb..#PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung') IS NOT NULL DROP TABLE #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung;
                                        ";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("articleNumber", articleNumber);
					sqlCommand.Parameters.AddWithValue("anteil", anteil);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationInData(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationStffelPreis> UpdateVKOnly(string username, List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationInData> data)
			{
				if(data == null || data.Count < 0)
					return null;

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					var tablename = $"[#PSZ_AV_Auswahl_Simulation_VK_Update_{username.Trim()}]";
					var sqlCommand = new SqlCommand("", sqlConnection);
					string insertData = $"INSERT INTO {tablename} ([Artikel-Nr],[Anteilberechen],[Artikelnummer],[Bezeichnung 1],[Bezeichnung 2],[DEL],[Prozent],[Update],[VK],[VK_Simulation],[VKCU],[VKCU_Simulation]) VALUES ";
					for(int i = 0; i < data.Count; i++)
					{
						insertData += ("("
							+ "@Artikel_Nr" + i + ","
							+ "@Anteilberechen" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Bezeichnung_2" + i + ","
							+ "@DEL" + i + ","
							+ "@Prozent" + i + ","
							+ "@Update" + i + ","
							+ "@VK" + i + ","
							+ "@VK_Simulation" + i + ","
							+ "@VKCU" + i + ","
							+ "@VKCU_Simulation" + i
							+ "),");

						sqlCommand.Parameters.AddWithValue("Anteilberechen" + i, data[i].Anteilberechen);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, data[i].Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, data[i].Artikelnummer == null ? (object)DBNull.Value : data[i].Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, data[i].Bezeichnung_1 == null ? (object)DBNull.Value : data[i].Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, data[i].Bezeichnung_2 == null ? (object)DBNull.Value : data[i].Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("DEL" + i, data[i].DEL == null ? (object)DBNull.Value : data[i].DEL);
						sqlCommand.Parameters.AddWithValue("Prozent" + i, data[i].Prozent == null ? (object)DBNull.Value : decimal.TryParse(data[i].Prozent.TrimEnd('%'), out var v) ? v : 0);
						sqlCommand.Parameters.AddWithValue("Update" + i, 1); // -
						sqlCommand.Parameters.AddWithValue("VK" + i, data[i].VK == null ? (object)DBNull.Value : data[i].VK);
						sqlCommand.Parameters.AddWithValue("VK_Simulation" + i, data[i].VK_Simulation == null ? (object)DBNull.Value : data[i].VK_Simulation);
						sqlCommand.Parameters.AddWithValue("VKCU" + i, data[i].VKCU == null ? (object)DBNull.Value : data[i].VKCU);
						sqlCommand.Parameters.AddWithValue("VKCU_Simulation" + i, data[i].VKCU_Simulation == null ? (object)DBNull.Value : data[i].VKCU_Simulation);

					}
					insertData = insertData.TrimEnd(',');

					sqlConnection.Open();

					// -
					string query = $@"
                                        IF OBJECT_ID('tempdb..{tablename}') IS NOT NULL DROP TABLE {tablename};

                                        /* Q1 */
                                        CREATE TABLE {tablename} (
                                            [Artikel-Nr] INT,
                                            Artikelnummer nvarchar(20),
                                            [Bezeichnung 1] nvarchar(1000),
                                            DEL int,
                                            [Bezeichnung 2]nvarchar(1000),
                                            VK decimal(20, 7),
                                            VK_Simulation decimal(20,7),
                                            VKCU decimal(20, 7),
                                            VKCU_Simulation decimal(20,7),
                                            [Anteilberechen]  decimal(20,7),
                                            Prozent decimal(10, 4),
                                            [Update] int
                                            );
                                            {insertData}

                                        /* Q2 */
                                        SELECT 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.Artikelnummer, 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.VK, 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation, 
	                                        Preisgruppen.Staffelpreis1, 
	                                        VK_Simulation*(1-Preisgruppen.PM2/100) AS Staffelpreis2, 
	                                        VK_Simulation*(1-Preisgruppen.PM3/100) AS Staffelpreis3, 
	                                        VK_Simulation*(1-Preisgruppen.PM4/100) AS Staffelpreis4
	                                        INTO #VK_Simulation_Staffelpreis_VK_großer_Staffelpreis
                                        FROM {tablename} PSZ_AV_Auswahl_Simulation_VK_Update INNER JOIN Preisgruppen 
	                                        ON PSZ_AV_Auswahl_Simulation_VK_Update.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                        WHERE (((PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation)<>[Staffelpreis2]) 
	                                        AND ((Preisgruppen.Staffelpreis2)<>0) AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1)) 
	                                        OR (((PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation)<[Staffelpreis3]) AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1) 
	                                        AND ((Preisgruppen.Staffelpreis3)<>0)) OR (((PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation)<[Staffelpreis4]) 
	                                        AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1) AND ((Preisgruppen.Staffelpreis4)<>0)) 
	                                        OR (((PSZ_AV_Auswahl_Simulation_VK_Update.VK)<>[PSZ_AV_Auswahl_Simulation_VK_Update].[VK_Simulation]) 
	                                        AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1));



                                        /* Q3 */
                                        UPDATE Preisgruppen
	                                        SET Verkaufspreis = AV.VK_Simulation, 
		                                        PM1 = 0, 
		                                        Staffelpreis1 = IIf(P.ME1>0,AV.VK_Simulation, NULL), 
		                                        Staffelpreis2 = IIf(P.PM2>0,AV.VK_Simulation*(1-P.PM2/100), NULL), 
		                                        Staffelpreis3 = IIf(P.PM3>0,AV.VK_Simulation*(1-P.PM3/100), NULL), 
		                                        Staffelpreis4 = IIf(P.PM4>0,AV.VK_Simulation*(1-P.PM4/100), NULL)
	                                        FROM {tablename} AS AV INNER JOIN Preisgruppen AS P ON AV.[Artikel-Nr] = P.[Artikel-Nr] 
                                        WHERE AV.[Update]=1;


                                        /* Q4 */
                                        INSERT INTO Preisgruppen_Historie ( Artikelnummer, [Artikel-Nr], Aenderungsdatum, [Verkaupspreis-Aktuell], [Verkaufspreis-Voränderung], Bearbeiter )
                                        SELECT PSZ_AV_Auswahl_Simulation_VK_Update.Artikelnummer, PSZ_AV_Auswahl_Simulation_VK_Update.[Artikel-Nr], GETDATE(), 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation, PSZ_AV_Auswahl_Simulation_VK_Update.VK, '{username + " Bemerkung:VK aktualisiert durch Simulation!"}' 
                                        FROM {tablename} AS PSZ_AV_Auswahl_Simulation_VK_Update
                                        WHERE (((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1));


                                        IF OBJECT_ID('tempdb..{tablename}') IS NOT NULL DROP TABLE {tablename};

                                        SELECT * FROM #VK_Simulation_Staffelpreis_VK_großer_Staffelpreis;
                                        ";

					sqlCommand.CommandText = query;
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationStffelPreis(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationStffelPreis> UpdateVKandAB(string username, List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationInData> data)
			{
				if(data == null || data.Count < 0)
					return null;

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					var tablename = $"[#PSZ_AV_Auswahl_Simulation_VK_Update_{username.Trim()}]";
					var sqlCommand = new SqlCommand("", sqlConnection);
					string insertData = $"INSERT INTO {tablename} ([Artikel-Nr],[Anteilberechen],[Artikelnummer],[Bezeichnung 1],[Bezeichnung 2],[DEL],[Prozent],[Update],[VK],[VK_Simulation],[VKCU],[VKCU_Simulation]) VALUES ";
					for(int i = 0; i < data.Count; i++)
					{
						insertData += ("("
							+ "@Artikel_Nr" + i + ","
							+ "@Anteilberechen" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Bezeichnung_2" + i + ","
							+ "@DEL" + i + ","
							+ "@Prozent" + i + ","
							+ "@Update" + i + ","
							+ "@VK" + i + ","
							+ "@VK_Simulation" + i + ","
							+ "@VKCU" + i + ","
							+ "@VKCU_Simulation" + i
							+ "),");

						sqlCommand.Parameters.AddWithValue("Anteilberechen" + i, data[i].Anteilberechen);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, data[i].Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, data[i].Artikelnummer == null ? (object)DBNull.Value : data[i].Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, data[i].Bezeichnung_1 == null ? (object)DBNull.Value : data[i].Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, data[i].Bezeichnung_2 == null ? (object)DBNull.Value : data[i].Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("DEL" + i, data[i].DEL == null ? (object)DBNull.Value : data[i].DEL);
						sqlCommand.Parameters.AddWithValue("Prozent" + i, data[i].Prozent == null ? (object)DBNull.Value : decimal.TryParse(data[i].Prozent.TrimEnd('%'), out var v) ? v : 0);
						sqlCommand.Parameters.AddWithValue("Update" + i, 1); // -
						sqlCommand.Parameters.AddWithValue("VK" + i, data[i].VK == null ? (object)DBNull.Value : data[i].VK);
						sqlCommand.Parameters.AddWithValue("VK_Simulation" + i, data[i].VK_Simulation == null ? (object)DBNull.Value : data[i].VK_Simulation);
						sqlCommand.Parameters.AddWithValue("VKCU" + i, data[i].VKCU == null ? (object)DBNull.Value : data[i].VKCU);
						sqlCommand.Parameters.AddWithValue("VKCU_Simulation" + i, data[i].VKCU_Simulation == null ? (object)DBNull.Value : data[i].VKCU_Simulation);

					}
					insertData = insertData.TrimEnd(',');

					sqlConnection.Open();

					// -
					string query = $@"
                                        IF OBJECT_ID('tempdb..{tablename}') IS NOT NULL DROP TABLE {tablename};
                                        IF OBJECT_ID('tempdb..#VK_Simulation_Staffelpreis_VK_großer_Staffelpreis') IS NOT NULL DROP TABLE #VK_Simulation_Staffelpreis_VK_großer_Staffelpreis;
                                        IF OBJECT_ID('tempdb..#PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung') IS NOT NULL DROP TABLE #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung;

                                        /* Q1 */
                                        CREATE TABLE {tablename} (
                                            [Artikel-Nr] INT,
                                            Artikelnummer nvarchar(20),
                                            [Bezeichnung 1] nvarchar(1000),
                                            DEL int,
                                            [Bezeichnung 2]nvarchar(1000),
                                            VK decimal(20, 7),
                                            VK_Simulation decimal(20,7),
                                            VKCU decimal(20, 7),
                                            VKCU_Simulation decimal(20,7),
                                            [Anteilberechen]  decimal(20,7),
                                            Prozent decimal(10, 4),
                                            [Update] int
                                            );
                                            {insertData}

                                        /* Q2 */
                                        SELECT 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.Artikelnummer, PSZ_AV_Auswahl_Simulation_VK_Update.VK, 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation, 
	                                        Preisgruppen.Staffelpreis1, 
	                                        VK_Simulation*(1-Preisgruppen.PM2/100) AS Staffelpreis2, 
	                                        VK_Simulation*(1-Preisgruppen.PM3/100) AS Staffelpreis3, 
	                                        VK_Simulation*(1-Preisgruppen.PM4/100) AS Staffelpreis4
	                                        INTO #VK_Simulation_Staffelpreis_VK_großer_Staffelpreis
                                        FROM {tablename} AS PSZ_AV_Auswahl_Simulation_VK_Update 
	                                        INNER JOIN Preisgruppen ON PSZ_AV_Auswahl_Simulation_VK_Update.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
                                        WHERE (((PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation)<>[Staffelpreis2]) AND ((Preisgruppen.Staffelpreis2)<>0) 
	                                        AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1)) OR (((PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation)<[Staffelpreis3]) 
	                                        AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1) AND ((Preisgruppen.Staffelpreis3)<>0)) 
	                                        OR (((PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation)<[Staffelpreis4]) AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1) 
	                                        AND ((Preisgruppen.Staffelpreis4)<>0)) OR (((PSZ_AV_Auswahl_Simulation_VK_Update.VK)<>[PSZ_AV_Auswahl_Simulation_VK_Update].[VK_Simulation]) 
	                                        AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1));



                                        /* Q3 */
                                        UPDATE Preisgruppen
	                                        SET Preisgruppen.Verkaufspreis = PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation, Preisgruppen.PM1 = 0, 
	                                        Preisgruppen.Staffelpreis1 = IIf(Preisgruppen.ME1>0,PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation, NULL), 
	                                        Preisgruppen.Staffelpreis2 = IIf(Preisgruppen.PM2>0,PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation*(1-Preisgruppen.PM2/100), NULL), 
	                                        Preisgruppen.Staffelpreis3 = IIf(Preisgruppen.PM3>0,PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation*(1-Preisgruppen.PM3/100), NULL), 
	                                        Preisgruppen.Staffelpreis4 = IIf(Preisgruppen.PM4>0,PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation*(1-Preisgruppen.PM4/100), NULL)
	                                        FROM {tablename} AS PSZ_AV_Auswahl_Simulation_VK_Update INNER JOIN Preisgruppen 
		                                        ON PSZ_AV_Auswahl_Simulation_VK_Update.[Artikel-Nr] = Preisgruppen.[Artikel-Nr] 
                                        WHERE (((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1));


                                        /* Q4 */
                                        UPDATE [angebotene Artikel]
	                                        SET [angebotene Artikel].VKEinzelpreis = [PSZ_AV_Auswahl_Simulation_VK_Update].[VK_Simulation], 
	                                        [angebotene Artikel].Einzelpreis = [PSZ_AV_Auswahl_Simulation_VK_Update].[VK_Simulation]
	                                        FROM (([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr) 
	                                        INNER JOIN {tablename} AS PSZ_AV_Auswahl_Simulation_VK_Update ON Artikel.[Artikel-Nr] = PSZ_AV_Auswahl_Simulation_VK_Update.[Artikel-Nr] 
                                        WHERE (((Angebote.Typ)='Auftragsbestätigung') AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1) 
	                                        AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
	                                        AND ((Artikel.[DEL fixiert])=1));

                                        /* Q5 */
                                        INSERT INTO Preisgruppen_Historie(Artikelnummer, [Artikel-Nr], Aenderungsdatum, [Verkaupspreis-Aktuell], [Verkaufspreis-Voränderung], Bearbeiter)
                                        SELECT 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.Artikelnummer, 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.[Artikel-Nr], 
	                                        GETDATE(), PSZ_AV_Auswahl_Simulation_VK_Update.VK_Simulation, 
	                                        PSZ_AV_Auswahl_Simulation_VK_Update.VK, 
	                                        '{username}  Bemerkung:VK aktualisiert durch Simulation.'
                                        FROM {tablename} AS PSZ_AV_Auswahl_Simulation_VK_Update
                                        WHERE (((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1));

                                        /* Q6 */
                                        SELECT 
	                                        [angebotene Artikel].Nr, Artikel.Artikelnummer, 
	                                        Artikel.DEL, Angebote.Typ, Angebote.[Projekt-Nr], 
	                                        Angebote.[Angebot-Nr], PSZ_AV_Auswahl_Simulation_VK_Update.[Update] 
	                                        INTO #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung /*[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung]*/
                                        FROM (([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        INNER JOIN Angebote ON [angebotene Artikel].[Angebot-Nr] = Angebote.Nr) 
	                                        INNER JOIN {tablename} AS PSZ_AV_Auswahl_Simulation_VK_Update ON Artikel.[Artikel-Nr] = PSZ_AV_Auswahl_Simulation_VK_Update.[Artikel-Nr]
                                        WHERE (((Angebote.Typ)='Auftragsbestätigung') AND ((PSZ_AV_Auswahl_Simulation_VK_Update.[Update])=1) 
	                                        AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
	                                        AND ((Artikel.[DEL fixiert])=1));


                                        /***** MACRO ****/
                                        /* Q1 -- Possible error on UPDATE Multiple tables at once -- */
                                        /* Q1 is splitted into Q1.1 & Q1.2 in order to update the 2 tables from original query */
                                        /* Q1.1 */
                                        UPDATE [angebotene Artikel]
	                                        SET [angebotene Artikel].Bezeichnung1 = Artikel.[Bezeichnung 1], 
	                                        [angebotene Artikel].Bezeichnung2 = Artikel.[Bezeichnung 2], 
	                                        [angebotene Artikel].Bezeichnung3 = Artikel.[Bezeichnung 3], 
	                                        [angebotene Artikel].Einheit = Artikel.Einheit, 
	                                        [angebotene Artikel].Preisgruppe = 1, [angebotene Artikel].Preiseinheit = Artikel.Preiseinheit, 
	                                        [angebotene Artikel].Zeichnungsnummer = Artikel.Index_Kunde, 
	                                        [angebotene Artikel].[VK-Festpreis] = Artikel.[VK-Festpreis], 
	                                        [angebotene Artikel].Kupferbasis = Artikel.Kupferbasis, [angebotene Artikel].DEL = Artikel.DEL, 
	                                        [angebotene Artikel].[EinzelCu-Gewicht] = Artikel.[Cu-Gewicht], 
	                                        [angebotene Artikel].VKEinzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].VKEinzelpreis))))), 
	                                        [angebotene Artikel].Einzelkupferzuschlag = Round(IIf(Artikel.[VK-Festpreis]=0,((Artikel.DEL*1.01)-Artikel.Kupferbasis)/100*Artikel.[Cu-Gewicht],0),2), 
	                                        [angebotene Artikel].Fertigungsnummer = 0, [angebotene Artikel].[DEL fixiert] = Artikel.[DEL fixiert], 
	                                        [angebotene Artikel].Abladestelle = Artikel.Abladestelle/*, 
                                            Preisgruppen.PM2 = (1-[Staffelpreis2]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM3 = (1-[Staffelpreis3]/[Verkaufspreis])*100, Preisgruppen.PM4 = (1-[Staffelpreis4]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM1 = 0, Preisgruppen.Staffelpreis1 = [Verkaufspreis]*/
	                                        FROM (([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                        INNER JOIN #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
	                                        ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr 
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]) AND ((Preisgruppen.Preisgruppe)=1));

                                        /* Q1.2 */
                                        UPDATE Preisgruppen
	                                        SET /*[angebotene Artikel].Bezeichnung1 = Artikel.[Bezeichnung 1], 
	                                        [angebotene Artikel].Bezeichnung2 = Artikel.[Bezeichnung 2], 
	                                        [angebotene Artikel].Bezeichnung3 = Artikel.[Bezeichnung 3], 
	                                        [angebotene Artikel].Einheit = Artikel.Einheit, 
	                                        [angebotene Artikel].Preisgruppe = 1, [angebotene Artikel].Preiseinheit = Artikel.Preiseinheit, 
	                                        [angebotene Artikel].Zeichnungsnummer = Artikel.Index_Kunde, 
	                                        [angebotene Artikel].[VK-Festpreis] = Artikel.[VK-Festpreis], 
	                                        [angebotene Artikel].Kupferbasis = Artikel.Kupferbasis, [angebotene Artikel].DEL = Artikel.DEL, 
	                                        [angebotene Artikel].[EinzelCu-Gewicht] = Artikel.[Cu-Gewicht], 
	                                        [angebotene Artikel].VKEinzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].VKEinzelpreis))))), 
	                                        [angebotene Artikel].Einzelkupferzuschlag = Round(IIf(Artikel.[VK-Festpreis]=0,((Artikel.DEL*1.01)-Artikel.Kupferbasis)/100*Artikel.[Cu-Gewicht],0),2), 
	                                        [angebotene Artikel].Fertigungsnummer = 0, [angebotene Artikel].[DEL fixiert] = Artikel.[DEL fixiert], 
	                                        [angebotene Artikel].Abladestelle = Artikel.Abladestelle,*/ 
                                            Preisgruppen.PM2 = (1-[Staffelpreis2]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM3 = (1-[Staffelpreis3]/[Verkaufspreis])*100, Preisgruppen.PM4 = (1-[Staffelpreis4]/[Verkaufspreis])*100, 
	                                        Preisgruppen.PM1 = 0, Preisgruppen.Staffelpreis1 = [Verkaufspreis]
	                                        FROM (([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                        INNER JOIN #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
	                                        ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr 
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]) AND ((Preisgruppen.Preisgruppe)=1));


                                        /* Q2 */
                                        UPDATE [angebotene Artikel]
	                                        SET [angebotene Artikel].USt = IIf(Kunden.[Umsatzsteuer berechnen]=1,0.19,0), 
	                                        [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].VKEinzelpreis,IIf([Anzahl]<=[ME1],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].Einzelkupferzuschlag*[angebotene Artikel].Preiseinheit+[angebotene Artikel].VKEinzelpreis))))), 
	                                        [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
	                                        [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Einzelkupferzuschlag)
	                                        FROM (Angebote INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
	                                        INNER JOIN (#PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
	                                        INNER JOIN ([angebotene Artikel] INNER JOIN Preisgruppen ON [angebotene Artikel].[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	                                        ON [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr = [angebotene Artikel].Nr) 
	                                        ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr] 
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));

                                        /* Q3 */
                                        UPDATE [angebotene Artikel]
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
	                                        FROM Preisgruppen INNER JOIN ((Kunden INNER JOIN Angebote ON Kunden.nummer = Angebote.[Kunden-Nr]) 
	                                        INNER JOIN (#PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
	                                        INNER JOIN [angebotene Artikel] ON [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr = [angebotene Artikel].Nr) 
	                                        ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr] 
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));

                                        /* Q4 */
                                        UPDATE [angebotene Artikel]
	                                        SET [angebotene Artikel].Einzelkupferzuschlag = Round(IIf([angebotene Artikel].[VK-Festpreis]=0,(([angebotene Artikel].DEL*1.01)-[angebotene Artikel].Kupferbasis)/100*[angebotene Artikel].[EinzelCu-Gewicht],0),2)
	                                        FROM [angebotene Artikel] INNER JOIN #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
	                                        ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr 
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));

                                        /* Q5 */
                                        UPDATE [angebotene Artikel]
	                                        SET [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel].Anzahl*[angebotene Artikel].[EinzelCu-Gewicht], 
	                                        [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel].[VK-Festpreis]=1,[angebotene Artikel].[VKEinzelpreis],IIf([Anzahl]<=[ME1],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM1]/100),IIf([Anzahl]>[ME1] And [Anzahl]<=[ME2],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM2]/100),IIf([Anzahl]>[ME2] And [Anzahl]<=[ME3],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM3]/100),IIf([Anzahl]>[ME3] And [Anzahl]<=[ME4],[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+([Verkaufspreis]-[Verkaufspreis]*[PM4]/100),[angebotene Artikel].[Einzelkupferzuschlag]*[angebotene Artikel].[Preiseinheit]+[angebotene Artikel].[VKEinzelpreis])))))
	                                        FROM Preisgruppen INNER JOIN ([angebotene Artikel] 
	                                        INNER JOIN #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
	                                        ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr) 
	                                        ON Preisgruppen.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr] 
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));

                                        /* Q6 */
                                        UPDATE [angebotene Artikel]
	                                        SET [angebotene Artikel].Gesamtpreis = [angebotene Artikel].Einzelpreis/[angebotene Artikel].Preiseinheit*[angebotene Artikel].Anzahl*(1-[angebotene Artikel].Rabatt), 
	                                        [angebotene Artikel].OriginalAnzahl = [angebotene Artikel].Anzahl, 
	                                        [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel].[VK-Festpreis]=1,0,[angebotene Artikel].Anzahl*[angebotene Artikel].Einzelkupferzuschlag), 
	                                        [angebotene Artikel].VKGesamtpreis = [angebotene Artikel].Anzahl*[angebotene Artikel].VKEinzelpreis/[angebotene Artikel].Preiseinheit
	                                        FROM [angebotene Artikel] INNER JOIN #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung AS [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung] 
	                                        ON [angebotene Artikel].Nr = [PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].Nr 
                                        WHERE ((([angebotene Artikel].Nr)=[PSZ_Offene Ab ermittelnb nach Nummernkreis für CU Umstellung].[Nr]));


                                        IF OBJECT_ID('tempdb..{tablename}') IS NOT NULL DROP TABLE {tablename};
                                        IF OBJECT_ID('tempdb..#PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung') IS NOT NULL DROP TABLE #PSZ_Offene_Ab_ermittelnb_nach_Nummernkreis_fur_CU_Umstellung;

                                        /* Q-end */
                                        SELECT * FROM #VK_Simulation_Staffelpreis_VK_großer_Staffelpreis;
                                        ";

					sqlCommand.CommandText = query;
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationStffelPreis(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public async static Task<List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_HighRunner>> GetHighRunner(string articleNumber, DateTime dateFrom, DateTime dateTo, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				var dataTable = new DataTable();
				return await Task<List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_HighRunner>>.Factory.StartNew(() =>
				{
					using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
					{
						sqlConnection.Open();
						string query = $@"
                            SELECT [PSZ_bestellte Artikel Zusammenfassung1].[Artikel-Nr],[PSZ_bestellte Artikel Zusammenfassung1].Artikelnummer, [PSZ_bestellte Artikel Zusammenfassung1].Bezeichnung1, Artikel.[Bezeichnung 2], [PSZ_bestellte Artikel Zusammenfassung1].[Bestell-Nr], [PSZ_bestellte Artikel Zusammenfassung1].Name1, [PSZ_bestellte Artikel Zusammenfassung1].SummevonAnzahl AS Einkaufsmenge, [PSZ_bestellte Artikel Zusammenfassung1].Einkaufspreis, [PSZ_bestellte Artikel Zusammenfassung1].SummevonAnzahl*[PSZ_bestellte Artikel Zusammenfassung1].Einkaufspreis AS Einkaufsvolumen, [PSZ_bestellte Artikel Zusammenfassung1].Zolltarif_nr, [PSZ_bestellte Artikel Zusammenfassung1].Gewichte
                            FROM 
                                (
                                SELECT [PSZ_Bestellte Artikel Highrunnerliste1].[Artikel-Nr] ,[PSZ_Bestellte Artikel Highrunnerliste1].Artikelnummer , [PSZ_Bestellte Artikel Highrunnerliste1].[Bezeichnung 1] as Bezeichnung1, Sum([PSZ_Bestellte Artikel Highrunnerliste1].Anzahl) AS SummevonAnzahl, [PSZ_Bestellte Artikel Highrunnerliste1].[Bestell-Nr] , [PSZ_Bestellte Artikel Highrunnerliste1].Name1, [PSZ_Bestellte Artikel Highrunnerliste1].Einkaufspreis, [PSZ_Bestellte Artikel Highrunnerliste1].Zolltarif_nr, [PSZ_Bestellte Artikel Highrunnerliste1].Gewichte
                                FROM 
                                (
                                    SELECT Bestellungen.Typ,Artikel.[Artikel-Nr], Bestellungen.[Bestellung-Nr], Artikel.Artikelnummer , Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, Bestellnummern.[Bestell-Nr], Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, CAST(Artikel.Größe AS DECIMAL(38,15)) AS Gewichte/* INTO [PSZ_Bestellte Artikel Highrunnerliste]*/
                                    FROM ((((Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr) INNER JOIN Stücklisten ON [bestellte Artikel].[Artikel-Nr] = Stücklisten.[Artikel-Nr des Bauteils]) INNER JOIN (Artikel INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]) INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr] = Artikel_1.[Artikel-Nr]
                                    WHERE (((Bestellnummern.Standardlieferant)=1) AND ((Bestellungen.Datum)>='{dateFrom.ToString("yyyyMMdd")}' And (Bestellungen.Datum)<='{dateTo.ToString("yyyyMMdd")}') AND ((Artikel_1.Artikelnummer) Like ('{articleNumber.SqlEscape() ?? ""}%')))
                                    GROUP BY Bestellungen.Typ, Bestellungen.[Bestellung-Nr],Artikel.[Artikel-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, Bestellnummern.[Bestell-Nr], Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, Artikel.Größe
                                    HAVING (((Bestellungen.Typ)='Wareneingang'))
                                )[PSZ_Bestellte Artikel Highrunnerliste1]
                                GROUP BY [PSZ_Bestellte Artikel Highrunnerliste1].[Artikel-Nr],[PSZ_Bestellte Artikel Highrunnerliste1].Artikelnummer, [PSZ_Bestellte Artikel Highrunnerliste1].[Bezeichnung 1], [PSZ_Bestellte Artikel Highrunnerliste1].[Bestell-Nr], [PSZ_Bestellte Artikel Highrunnerliste1].Name1, [PSZ_Bestellte Artikel Highrunnerliste1].Einkaufspreis, [PSZ_Bestellte Artikel Highrunnerliste1].Zolltarif_nr, [PSZ_Bestellte Artikel Highrunnerliste1].Gewichte

                            )[PSZ_bestellte Artikel Zusammenfassung1] INNER JOIN Artikel ON [PSZ_bestellte Artikel Zusammenfassung1].Artikelnummer = Artikel.Artikelnummer";
						query += $" ORDER BY Artikelnummer OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
						var sqlCommand = new SqlCommand(query, sqlConnection);
						sqlCommand.CommandTimeout = 1000;

						DbExecution.Fill(sqlCommand, dataTable);
					}

					if(dataTable.Rows.Count > 0)
					{
						return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_HighRunner(x)).ToList();
					}
					else
					{
						return null;
					}
				});
			}
			public async static Task<int> GetHighRunner_Count(string articleNumber, DateTime dateFrom, DateTime dateTo)
			{
				var dataTable = new DataTable();

				return await Task<int>.Factory.StartNew(() =>
				{
					using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
					{
						sqlConnection.Open();
						string query = $@"
                            SELECT COUNT(*) as R
                            FROM 
                                (
                                SELECT [PSZ_Bestellte Artikel Highrunnerliste1].Artikelnummer , [PSZ_Bestellte Artikel Highrunnerliste1].[Bezeichnung 1] as Bezeichnung1, Sum([PSZ_Bestellte Artikel Highrunnerliste1].Anzahl) AS SummevonAnzahl, [PSZ_Bestellte Artikel Highrunnerliste1].[Bestell-Nr] , [PSZ_Bestellte Artikel Highrunnerliste1].Name1, [PSZ_Bestellte Artikel Highrunnerliste1].Einkaufspreis, [PSZ_Bestellte Artikel Highrunnerliste1].Zolltarif_nr, [PSZ_Bestellte Artikel Highrunnerliste1].Gewichte
                                FROM 
                                (
                                    SELECT Bestellungen.Typ, Bestellungen.[Bestellung-Nr], Artikel.Artikelnummer , Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, Bestellnummern.[Bestell-Nr], Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, CAST(Artikel.Größe AS DECIMAL(38,15)) AS Gewichte/* INTO [PSZ_Bestellte Artikel Highrunnerliste]*/
                                    FROM ((((Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr) INNER JOIN Stücklisten ON [bestellte Artikel].[Artikel-Nr] = Stücklisten.[Artikel-Nr des Bauteils]) INNER JOIN (Artikel INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]) INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr] = Artikel_1.[Artikel-Nr]
                                    WHERE (((Bestellnummern.Standardlieferant)=1) AND ((Bestellungen.Datum)>='{dateFrom.ToString("yyyyMMdd")}' And (Bestellungen.Datum)<='{dateTo.ToString("yyyyMMdd")}') AND ((Artikel_1.Artikelnummer) Like ('{articleNumber.SqlEscape() ?? ""}%')))
                                    GROUP BY Bestellungen.Typ, Bestellungen.[Bestellung-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, Bestellnummern.[Bestell-Nr], Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, Artikel.Größe
                                    HAVING (((Bestellungen.Typ)='Wareneingang'))
                                )[PSZ_Bestellte Artikel Highrunnerliste1]
                                GROUP BY [PSZ_Bestellte Artikel Highrunnerliste1].Artikelnummer, [PSZ_Bestellte Artikel Highrunnerliste1].[Bezeichnung 1], [PSZ_Bestellte Artikel Highrunnerliste1].[Bestell-Nr], [PSZ_Bestellte Artikel Highrunnerliste1].Name1, [PSZ_Bestellte Artikel Highrunnerliste1].Einkaufspreis, [PSZ_Bestellte Artikel Highrunnerliste1].Zolltarif_nr, [PSZ_Bestellte Artikel Highrunnerliste1].Gewichte

                            )[PSZ_bestellte Artikel Zusammenfassung1] INNER JOIN Artikel ON [PSZ_bestellte Artikel Zusammenfassung1].Artikelnummer = Artikel.Artikelnummer;";
						var sqlCommand = new SqlCommand(query, sqlConnection);
						sqlCommand.CommandTimeout = 1000;

						return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
					}
				});
			}
			public async static Task<List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_SupplierHitCount>> GetSupplierHitCounts(string articleNumber, DateTime dateFrom, DateTime dateTo, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				var dataTable = new DataTable();
				return await Task<List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_SupplierHitCount>>.Factory.StartNew(() =>
				{
					using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
					{
						sqlConnection.Open();
						string query = $@"
                            SELECT [PSZ_bestellte Artikel Zusammenfassung1].Name1, 
	                            Sum([PSZ_bestellte Artikel Zusammenfassung1].SummevonAnzahl*[PSZ_bestellte Artikel Zusammenfassung1].Einkaufspreis) AS Einkaufsvolumen
                            FROM
                            (
                            SELECT [PSZ_Bestellte Artikel Highrunnerliste].Artikelnummer, [PSZ_Bestellte Artikel Highrunnerliste].[Bezeichnung 1], 
	                            Sum([PSZ_Bestellte Artikel Highrunnerliste].Anzahl) AS SummevonAnzahl, [PSZ_Bestellte Artikel Highrunnerliste].[Bestell-Nr], 
	                            [PSZ_Bestellte Artikel Highrunnerliste].Name1, [PSZ_Bestellte Artikel Highrunnerliste].Einkaufspreis, 
	                            [PSZ_Bestellte Artikel Highrunnerliste].Zolltarif_nr, [PSZ_Bestellte Artikel Highrunnerliste].Gewichte
                            FROM
                            (
                            SELECT Bestellungen.Typ, Bestellungen.[Bestellung-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, 
	                            Bestellnummern.[Bestell-Nr], Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, 
	                            CAST(Artikel.Größe AS DECIMAL(38,15)) AS Gewichte 
                            FROM ((((Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
	                            INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr) 
	                            INNER JOIN Stücklisten ON [bestellte Artikel].[Artikel-Nr] = Stücklisten.[Artikel-Nr des Bauteils]) 
	                            INNER JOIN (Artikel 
	                            INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]) 
	                            INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr] = Artikel_1.[Artikel-Nr]
                            WHERE (((Bestellnummern.Standardlieferant)=1) AND ((Bestellungen.Datum)>='{dateFrom.ToString("yyyyMMdd")}' And (Bestellungen.Datum)<='{dateTo.ToString("yyyyMMdd")}') 
	                            AND ((Artikel_1.Artikelnummer) Like ('{articleNumber.SqlEscape(true) ?? ""}%')))
                            GROUP BY Bestellungen.Typ, Bestellungen.[Bestellung-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, 
	                            Bestellnummern.[Bestell-Nr], Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, Artikel.Größe
                            HAVING (((Bestellungen.Typ)='Wareneingang'))
                            )[PSZ_Bestellte Artikel Highrunnerliste]
                            GROUP BY [PSZ_Bestellte Artikel Highrunnerliste].Artikelnummer, [PSZ_Bestellte Artikel Highrunnerliste].[Bezeichnung 1], 
	                            [PSZ_Bestellte Artikel Highrunnerliste].[Bestell-Nr], [PSZ_Bestellte Artikel Highrunnerliste].Name1, 
	                            [PSZ_Bestellte Artikel Highrunnerliste].Einkaufspreis, [PSZ_Bestellte Artikel Highrunnerliste].Zolltarif_nr, 
	                            [PSZ_Bestellte Artikel Highrunnerliste].Gewichte
                            )[PSZ_bestellte Artikel Zusammenfassung1]
                            GROUP BY [PSZ_bestellte Artikel Zusammenfassung1].Name1
                            ORDER BY [PSZ_bestellte Artikel Zusammenfassung1].Name1 ";
						query += $" OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
						var sqlCommand = new SqlCommand(query, sqlConnection);
						sqlCommand.CommandTimeout = 1000;

						DbExecution.Fill(sqlCommand, dataTable);
					}

					if(dataTable.Rows.Count > 0)
					{
						return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_SupplierHitCount(x)).ToList();
					}
					else
					{
						return null;
					}
				});
			}
			public async static Task<int> GetSupplierHitCounts_Count(string articleNumber, DateTime dateFrom, DateTime dateTo)
			{
				return await Task<int>.Factory.StartNew(() =>
				{
					using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
					{
						sqlConnection.Open();
						string query = $@"SELECT COUNT(*) FROM (
                            SELECT [PSZ_bestellte Artikel Zusammenfassung1].Name1
                            FROM
                            (
                            SELECT [PSZ_Bestellte Artikel Highrunnerliste].Artikelnummer, [PSZ_Bestellte Artikel Highrunnerliste].[Bezeichnung 1], 
	                            Sum([PSZ_Bestellte Artikel Highrunnerliste].Anzahl) AS SummevonAnzahl, [PSZ_Bestellte Artikel Highrunnerliste].[Bestell-Nr], 
	                            [PSZ_Bestellte Artikel Highrunnerliste].Name1, [PSZ_Bestellte Artikel Highrunnerliste].Einkaufspreis, 
	                            [PSZ_Bestellte Artikel Highrunnerliste].Zolltarif_nr, [PSZ_Bestellte Artikel Highrunnerliste].Gewichte
                            FROM
                            (
                            SELECT Bestellungen.Typ, Bestellungen.[Bestellung-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, 
	                            Bestellnummern.[Bestell-Nr], Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, 
	                            CAST(Artikel.Größe AS DECIMAL(38,15)) AS Gewichte 
                            FROM ((((Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
	                            INNER JOIN adressen ON Bestellungen.[Lieferanten-Nr] = adressen.Nr) 
	                            INNER JOIN Stücklisten ON [bestellte Artikel].[Artikel-Nr] = Stücklisten.[Artikel-Nr des Bauteils]) 
	                            INNER JOIN (Artikel 
	                            INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]) 
	                            INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr] = Artikel_1.[Artikel-Nr]
                            WHERE (((Bestellnummern.Standardlieferant)=1) AND ((Bestellungen.Datum)>='{dateFrom.ToString("yyyyMMdd")}' And (Bestellungen.Datum)<='{dateTo.ToString("yyyyMMdd")}') 
	                            AND ((Artikel_1.Artikelnummer) Like ('{articleNumber.SqlEscape(true) ?? ""}%')))
                            GROUP BY Bestellungen.Typ, Bestellungen.[Bestellung-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], [bestellte Artikel].Anzahl, 
	                            Bestellnummern.[Bestell-Nr], Bestellungen.[Lieferanten-Nr], adressen.Name1, Bestellnummern.Einkaufspreis, Artikel.Zolltarif_nr, Artikel.Größe
                            HAVING (((Bestellungen.Typ)='Wareneingang'))
                            )[PSZ_Bestellte Artikel Highrunnerliste]
                            GROUP BY [PSZ_Bestellte Artikel Highrunnerliste].Artikelnummer, [PSZ_Bestellte Artikel Highrunnerliste].[Bezeichnung 1], 
	                            [PSZ_Bestellte Artikel Highrunnerliste].[Bestell-Nr], [PSZ_Bestellte Artikel Highrunnerliste].Name1, 
	                            [PSZ_Bestellte Artikel Highrunnerliste].Einkaufspreis, [PSZ_Bestellte Artikel Highrunnerliste].Zolltarif_nr, 
	                            [PSZ_Bestellte Artikel Highrunnerliste].Gewichte
                            )[PSZ_bestellte Artikel Zusammenfassung1]
                            GROUP BY [PSZ_bestellte Artikel Zusammenfassung1].Name1) AS TMP;";
						var sqlCommand = new SqlCommand(query, sqlConnection);
						sqlCommand.CommandTimeout = 1000;

						return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var r) ? r : 0;
					}
				});
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibu> GetAnalyseFibu(bool? isInvoice, DateTime dateFrom, DateTime dateTo, string customerNumber = null)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
                            SELECT Angebote.Typ, Angebote.Datum AS Rechnungsdatum, Angebote.[Angebot-Nr] AS Rechnungsnummer, Angebote.[Unser Zeichen] AS Debitorennummer, 
	                            [angebotene Artikel].Gesamtkupferzuschlag, {(isInvoice.HasValue == false ? "Angebote.Typ"
								: (isInvoice.Value == true ? "'Rechnung'" : "'Gutschrift'"))} AS Ausdruck, [Vorname/NameFirma] AS Debitorenname
                            FROM (((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
	                            INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
	                            INNER JOIN Konditionszuordnungstabelle ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) 
	                            INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
                            WHERE {(string.IsNullOrWhiteSpace(customerNumber) == true ? "" : $"Angebote.[Unser Zeichen]='{customerNumber}' AND ")}
                            {(isInvoice.HasValue == false
								? $" (Angebote.Typ='Rechnung' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}' AND [angebotene Artikel].Gesamtkupferzuschlag>0.000001)" +
								$" OR (Angebote.Typ='Gutschrift' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}')" :
								(isInvoice.Value == true
									? $" Angebote.Typ='Rechnung' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}' AND [angebotene Artikel].Gesamtkupferzuschlag>0.000001"
									: $" Angebote.Typ='Gutschrift' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}'"))} ";
					query += $" ORDER BY Angebote.Datum";
					//query += $" OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibu(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibuHeader> GetAnalyseFibuHeader(bool? isInvoice, DateTime dateFrom, DateTime dateTo)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
                        SELECT Debitorennummer, Debitorenname,
                            COUNT(Rechnungsnummer) AS RechnungsCount, Ausdruck,
	                        SUM(Gesamtkupferzuschlag) AS Gesamtkupferzuschlag
                        FROM (
                        SELECT Angebote.Typ, Angebote.Datum AS Rechnungsdatum, Angebote.[Angebot-Nr] AS Rechnungsnummer, Angebote.[Unser Zeichen] AS Debitorennummer, 
	                            [angebotene Artikel].Gesamtkupferzuschlag, {(isInvoice.HasValue == false ? "Angebote.Typ"
								: (isInvoice.Value == true ? "'Rechnung'" : "'Gutschrift'"))} AS Ausdruck, [Vorname/NameFirma] AS Debitorenname
                            FROM (((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
	                            INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
	                            INNER JOIN Konditionszuordnungstabelle ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) 
	                            INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
                            WHERE
                            {(isInvoice.HasValue == false
								? $" (Angebote.Typ='Rechnung' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}' AND [angebotene Artikel].Gesamtkupferzuschlag>0.000001)" +
								$" OR (Angebote.Typ='Gutschrift' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}')" :
								(isInvoice.Value == true
									? $" Angebote.Typ='Rechnung' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}' AND [angebotene Artikel].Gesamtkupferzuschlag>0.000001"
									: $" Angebote.Typ='Gutschrift' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}'"))} 
                        ) AS TMP
	                        GROUP BY Debitorennummer, Debitorenname, Ausdruck
                            ";
					//query += $" OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibuHeader(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetAnalyseFibu_Count(bool? isInvoice, DateTime dateFrom, DateTime dateTo, string customerNumber = null)
			{
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
                            SELECT Count(*) as count
                            FROM
                            (
                                SELECT Angebote.Typ, Angebote.Datum AS Rechnungsdatum, Angebote.[Angebot-Nr] AS Rechnungsnummer, Angebote.[Unser Zeichen] AS Debitorennummer, 
	                                [angebotene Artikel].Gesamtkupferzuschlag, {(isInvoice.HasValue == false ? "Angebote.Typ"
									: (isInvoice.Value == true ? "'Rechnung'" : "'Gutschrift'"))} AS Ausdruck, [Vorname/NameFirma] AS Debitorenname
                                FROM (((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
	                                INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer) 
	                                INNER JOIN Konditionszuordnungstabelle ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) 
	                                INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
                                {(isInvoice.HasValue == false
									? $"WHERE (Angebote.Typ='Rechnung' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}' AND [angebotene Artikel].Gesamtkupferzuschlag>0.000001)" +
									$" OR (Angebote.Typ='Gutschrift' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}')" :
									(isInvoice.Value == true
										? $"WHERE Angebote.Typ='Rechnung' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}' AND [angebotene Artikel].Gesamtkupferzuschlag>0.000001"
										: $"WHERE Angebote.Typ='Gutschrift' AND Angebote.Datum>='{dateFrom.ToString("yyyyMMdd")}' And Angebote.Datum<='{dateTo.ToString("yyyyMMdd")}'"))} 
                            );";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var r) ? r : 0;
				}
			}
			public static IEnumerable<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_RohArticlesSuppliers> GetRohArticlesSuppliers()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					// -
					string query = $@"WITH s AS (
												SELECT Artikelnr, SUM(FaQuantity) BedarfPO FROM [stats].[MaterialRequirementsFa] 
												WHERE SyncId=(SELECT MAX(SyncId) FROM [stats].[MaterialRequirementsHeader]) GROUP BY Artikelnr
											),
											b AS (
												SELECT [Lieferanten-Nr], p.[Artikel-Nr], SUM(ISNULL(p.[Start Anzahl],0)) Last2YearsOrderQuantity 
												FROM [bestellte Artikel] p
												LEFT JOIN Bestellungen b ON b.Nr=p.[Bestellung-Nr]
												WHERE b.Typ='Bestellung' AND b.Datum>= DATEADD(YEAR, -2, GETDATE()) 
												GROUP BY [Lieferanten-Nr], p.[Artikel-Nr]
											),
											w AS (
												SELECT [Lieferanten-Nr], p.[Artikel-Nr], SUM(ISNULL(p.[Start Anzahl],0)) LastYearsBookingQuantity 
												FROM [bestellte Artikel] p
												LEFT JOIN Bestellungen b ON b.Nr=p.[Bestellung-Nr]
												WHERE b.Typ='Wareneingang' AND b.Datum>= DATEADD(YEAR, -1, GETDATE()) 
												GROUP BY [Lieferanten-Nr], p.[Artikel-Nr]
											)  
											SELECT d.lieferantennummer, d.Name1 Lieferant, d.Stufe,
												CASE WHEN n.Standardlieferant=1 THEN 'ja' ELSE 'nein' END ist_Priolieferant, 
												a.Artikelnummer, a.artikelklassifizierung, CASE WHEN a.aktiv=1 THEN 'ja' ELSE 'nein' END ist_Systemaktiv, a.Freigabestatus [Status],
												n.Artikelbezeichnung, CAST(n.Artikelbezeichnung2 AS nvarchar) Artikelbezeichnung2, 
												n.[Bestell-Nr], n.Einkaufspreis, n.Angebot, n.Angebot_Datum,
												n.Mindestbestellmenge, n.Wiederbeschaffungszeitraum,
												n.Verpackungseinheit, ISNULL(b.Last2YearsOrderQuantity,0) Last2YearsOrderQuantity, 
												ISNULL(w.LastYearsBookingQuantity,0) LastYearsBookingQuantity, ISNULL(s.BedarfPO,0) BedarfPO, a.Manufacturer,  a.ManufacturerNumber
												FROM adressen d
												INNER JOIN Bestellnummern n ON d.Nr = n.[Lieferanten-Nr] 
												INNER JOIN Artikel a ON n.[Artikel-Nr] = a.[Artikel-Nr]
												LEFT JOIN s ON s.Artikelnr=a.[Artikel-Nr]
												LEFT JOIN b ON b.[Artikel-Nr]=a.[Artikel-Nr] AND b.[Lieferanten-Nr]=n.[Lieferanten-Nr]
												LEFT JOIN w ON w.[Artikel-Nr]=a.[Artikel-Nr] AND w.[Lieferanten-Nr]=n.[Lieferanten-Nr]
												WHERE a.Warengruppe='roh'
												ORDER BY d.Name1, a.Artikelnummer ";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_RohArticlesSuppliers(x));
				}
				else
				{
					return null;
				}
			}
		}
		public class Technic
		{
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_PlanningOrder> GetPlanningOrder(int? lager, string employeeName)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT
	                                    Fertigung.Lagerort_id, Fertigung.Erstmuster, Fertigung.Quick_Area AS Sonderfertigung, 
	                                    Fertigung.Techniker, [angebotene Artikel].Liefertermin AS AB_Termin, 
	                                    Fertigung.Termin_Bestätigt1 AS [Plan], '' AS [Termin besprochen], Artikel.Artikelnummer AS [PSZ#],
	                                    Fertigung.Originalanzahl AS Menge, Fertigung.Anzahl AS Offen_Anzahl, Fertigung.Fertigungsnummer AS FA, 
	                                    [Zeit]/1 AS [Zeit in min pro Stück], Artikel.Freigabestatus AS Status, Artikel.[Prüfstatus TN Ware], 
	                                    Artikel.[Freigabestatus TN intern] AS [Status intern], Fertigung.Bemerkung_Technik, Fertigung.Bemerkung AS [Info CS], 
	                                    Fertigung.Quick_Area, Fertigung.Kommisioniert_teilweise, Fertigung.Kommisioniert_komplett, Fertigung.Kabel_geschnitten, 
	                                    Fertigung.Kabel_geschnitten_Datum, Fertigung.FA_Gestartet, Fertigung.[Urs-Artikelnummer]
                                    FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
	                                    LEFT JOIN [angebotene Artikel] ON Fertigung.Angebot_Artikel_Nr = [angebotene Artikel].Nr
                                    WHERE (((Fertigung.Lagerort_id) Like ('{(lager.HasValue ? lager.Value.ToString() : "%")}')) AND ((Fertigung.Quick_Area) Not Like -1)
	                                    AND ((Fertigung.Techniker) Like ('%{employeeName.SqlEscape() ?? ""}%')) AND ((Fertigung.Technik)=1) AND ((Fertigung.Kennzeichen)='offen'))
                                    ORDER BY Fertigung.Techniker, Fertigung.Termin_Bestätigt1, Artikel.Artikelnummer;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_PlanningOrder(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.TechnicTechnikerEntity> GetTechnicians()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM PSZ_Techniker;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.TechnicTechnikerEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int InsertTechnician(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.TechnicTechnikerEntity item)
			{

				int response = int.MinValue;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlTransaction = sqlConnection.BeginTransaction();

					string query = "INSERT INTO [PSZ_Techniker] ([Name])  VALUES (@Name); ";
					query += "SELECT SCOPE_IDENTITY();";

					using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
					{

						sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);

						var result = DbExecution.ExecuteScalar(sqlCommand);
						response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
					}
					sqlTransaction.Commit();

					return response;
				}
			}
			public static int UpdateTechnician(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.TechnicTechnikerEntity item)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "UPDATE [PSZ_Techniker] SET [Name]=@Name WHERE [ID]=@Id";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					sqlCommand.Parameters.AddWithValue("Id", item.ID);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			public static int DeleteTechnician(int id)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "DELETE FROM [PSZ_Techniker] WHERE [ID]=@Id";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("Id", id);

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_PSZ_Projektdaten_DetailsEntity> GetProjectData(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM PSZ_Projektdaten_Details WHERE PSZ_Projektdaten_Details.Artikelnummer=@articleNumber;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("articleNumber", articleNumber);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_PSZ_Projektdaten_DetailsEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_QuickAreaBestand> GetQuickAreaBestand_CZ()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM PSZ_QuickArea_Bestand WHERE SummevonBestand=0 ORDER BY Verfügbar;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_QuickAreaBestand(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_QuickAreaBestand> GetQuickAreaBestand_TN()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM  PSZ_QuickArea_Bestand_TN WHERE SummevonBestand=0 ORDER BY Verfügbar;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_QuickAreaBestand(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_ProdTN> GetProdTN(DateTime? fertigungDatum = null, string articleNumber = "Reparatur", string fertigungKennzeichen = "erledigt")
			{
				if(fertigungDatum == null)
					fertigungDatum = new DateTime(2010, 1, 1);

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"IF OBJECT_ID('tempdb..#PSZ_ProdTunesien2') IS NOT NULL DROP TABLE #PSZ_ProdTunesien2;

                                WITH [PSZ_Prod Tunesien 1] AS (
                                SELECT Artikel.[Artikel-Nr],Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Originalanzahl AS Anzahl, Artikel.Stundensatz, 
	                                Artikel.Produktionszeit, Sum(StundenTN.Minuten) AS SummevonMinuten, Artikel.[Bezeichnung 1], Fertigung.Termin_Fertigstellung, 
	                                Artikel.[Bezeichnung 3]
                                FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) INNER JOIN StundenTN ON Fertigung.Fertigungsnummer = StundenTN.Auftragsnummer
                                GROUP BY Fertigung.Fertigungsnummer,Artikel.[Artikel-Nr], Artikel.Artikelnummer, Fertigung.Originalanzahl, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1], Fertigung.Termin_Fertigstellung, Artikel.[Bezeichnung 3], Fertigung.Kennzeichen
                                HAVING (((Artikel.Artikelnummer)<>@articleNumber) AND ((Fertigung.Termin_Fertigstellung)>@fertigungDatum) AND ((Fertigung.Kennzeichen)=@fertigungKennzeichen))
                                )
                                SELECT [PSZ_Prod Tunesien 1].[Artikel-Nr],[PSZ_Prod Tunesien 1].Artikelnummer, [PSZ_Prod Tunesien 1].Stundensatz, [PSZ_Prod Tunesien 1].Produktionszeit, 
	                                Avg(ISNULL([Anzahl]*[Produktionszeit]/NULLIF([SummevonMinuten], 0), 0)) AS Produktivität, 
	                                Count([PSZ_Prod Tunesien 1].Fertigungsnummer) AS AnzahlvonFertigungsnummer, [PSZ_Prod Tunesien 1].[Bezeichnung 1], 
	                                [PSZ_Prod Tunesien 1].[Bezeichnung 3]
                                INTO #PSZ_ProdTunesien2
                                FROM [PSZ_Prod Tunesien 1]
                                WHERE ISNULL([Anzahl]*[Produktionszeit]/NULLIF([SummevonMinuten], 0), 0)<2
                                GROUP BY [PSZ_Prod Tunesien 1].[Artikel-Nr],[PSZ_Prod Tunesien 1].Artikelnummer, [PSZ_Prod Tunesien 1].Stundensatz, [PSZ_Prod Tunesien 1].Produktionszeit, [PSZ_Prod Tunesien 1].[Bezeichnung 1], [PSZ_Prod Tunesien 1].[Bezeichnung 3];

                                /* Q3 */
                                SELECT [PSZ_Prod Tunesien 2].[Artikel-Nr],[PSZ_Prod Tunesien 2].Artikelnummer, [PSZ_Prod Tunesien 2].[Bezeichnung 1], [PSZ_Prod Tunesien 2].Stundensatz, [PSZ_Prod Tunesien 2].Produktionszeit, [PSZ_Prod Tunesien 2].Produktivität, [PSZ_Prod Tunesien 2].AnzahlvonFertigungsnummer, [Stundensatz]*(3*[Produktivität]) AS Kennzahl, [PSZ_Prod Tunesien 2].[Bezeichnung 3]
                                FROM #PSZ_ProdTunesien2 AS [PSZ_Prod Tunesien 2]
                                WHERE ((([PSZ_Prod Tunesien 2].Produktivität)<2) AND (([PSZ_Prod Tunesien 2].AnzahlvonFertigungsnummer)>1))
                                ORDER BY [Stundensatz]*(3*[Produktivität]);
                            IF OBJECT_ID('tempdb..#PSZ_TransferTunesien2') IS NOT NULL DROP TABLE #PSZ_ProdTunesien2;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("fertigungDatum", fertigungDatum == null ? (object)DBNull.Value : fertigungDatum);
					sqlCommand.Parameters.AddWithValue("articleNumber", articleNumber == null ? (object)DBNull.Value : articleNumber);
					sqlCommand.Parameters.AddWithValue("fertigungKennzeichen", fertigungKennzeichen == null ? (object)DBNull.Value : fertigungKennzeichen);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_ProdTN(x)).ToList();
				}
				else
				{
					return null;
				}
			}
		}
		public class CustomerService
		{
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_PlanningTechnicOrder> GetPlanningTechnicOrder(string technicianName, string employeeName)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH PSZ_PlanungstabelleTechnikCS AS (
                                        SELECT 
	                                        Fertigung.Techniker, [angebotene Artikel].Liefertermin AS AB_Termin, Fertigung.Termin_Bestätigt1 AS [Plan], 
	                                        Artikel.Artikelnummer AS [PSZ#], Fertigung.Originalanzahl AS Menge, Fertigung.Fertigungsnummer AS FA, 
	                                        Fertigung.Planungsstatus, Fertigung.Bemerkung_Technik, Fertigung.Bemerkung AS [Info CS], 
	                                        [Zeit]/1 AS [Zeit in min pro Stück], Artikel.Freigabestatus AS Status, Fertigung.Erstmuster AS EM, 
	                                        Fertigung.Lagerort_id AS Fertigung, (LEFT(Artikel.[Artikelnummer], (CASE WHEN CHARINDEX('-',Artikel.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',Artikel.[Artikelnummer],0)-1 END))) AS Kundencode
                                        FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
	                                        LEFT JOIN [angebotene Artikel] ON Fertigung.Angebot_Artikel_Nr = [angebotene Artikel].Nr
                                        WHERE (((Fertigung.Techniker) Like '%{technicianName.SqlEscape()}%') AND ((Fertigung.Technik)=1) AND ((Fertigung.Kennzeichen)='offen'))
                                        )
                                        SELECT 
	                                        [PSZ_Nummerschlüssel Kunde].[CS Kontakt], [PSZ_Planungstabelle Technik CS].*
                                        FROM PSZ_PlanungstabelleTechnikCS AS [PSZ_Planungstabelle Technik CS] 
	                                        INNER JOIN [PSZ_Nummerschlüssel Kunde] ON [PSZ_Planungstabelle Technik CS].Kundencode = [PSZ_Nummerschlüssel Kunde].Nummerschlüssel
                                        WHERE ((([PSZ_Nummerschlüssel Kunde].[CS Kontakt]) Like '%{employeeName.SqlEscape()}%'));";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_PlanningTechnicOrder(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StatusPArticle> GetStatusPArticle(string employeeName, string status)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT
	                                        PSZ_P_Statusabfrage_CS_II_01.[CS Kontakt], PSZ_P_Statusabfrage_CS_II_01.Kunde, 
	                                        PSZ_P_Statusabfrage_CS_II_01.[PSZ#], PSZ_P_Statusabfrage_CS_II_01.[Kunde#], 
	                                        PSZ_P_Statusabfrage_CS_II_01.[Index], PSZ_P_Statusabfrage_CS_II_01.Index_Datum, 
	                                        PSZ_P_Statusabfrage_CS_II_01.Freigabestatus
                                        FROM View_PSZ_P_Statusabfrage_CS_II AS PSZ_P_Statusabfrage_CS_II_01
                                        WHERE (((PSZ_P_Statusabfrage_CS_II_01.[CS Kontakt]) Like '%{employeeName.SqlEscape()}%') AND ((PSZ_P_Statusabfrage_CS_II_01.Freigabestatus) Like '%{status.SqlEscape()}%'))
                                        ORDER BY PSZ_P_Statusabfrage_CS_II_01.Kunde, PSZ_P_Statusabfrage_CS_II_01.[PSZ#];";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StatusPArticle(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_RepairEvaluation> GetRepairEvaluation(string customerNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Auswertung_OffeneFAReparatur_01 AS (
                                        SELECT 
	                                        Artikel.Artikelnummer, Fertigung.Anzahl, Fertigung.Originalanzahl, Fertigung.Fertigungsnummer, 
	                                        Fertigung.Lagerort_id, Fertigung.Termin_Bestätigt1, Fertigung.[Urs-Artikelnummer], 
	                                        Fertigung.Kennzeichen, (LEFT([Urs-Artikelnummer], (CASE WHEN CHARINDEX('-',[Urs-Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Urs-Artikelnummer],0)-1 END))) AS NKreis, Artikel_1.[Bezeichnung 1], Artikel_1.[Bezeichnung 2]
                                        FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) INNER JOIN Artikel AS Artikel_1 ON Fertigung.[Urs-Artikelnummer] = Artikel_1.Artikelnummer
                                        WHERE (((Artikel.Artikelnummer) Like 'Repar%') AND ((Fertigung.Kennzeichen)='Offen'))
                                        )
                                        SELECT 
	                                        [PSZ_Nummerschlüssel Kunde].Nummerschlüssel, [PSZ_Nummerschlüssel Kunde].Kunde, 
	                                        [Auswertung_Offene FA Reparatur_01].[Urs-Artikelnummer], [Auswertung_Offene FA Reparatur_01].[Bezeichnung 1], 
	                                        [Auswertung_Offene FA Reparatur_01].[Bezeichnung 2], [Auswertung_Offene FA Reparatur_01].Fertigungsnummer, 
	                                        [Auswertung_Offene FA Reparatur_01].Lagerort_id AS [Produktionsort ID], [Auswertung_Offene FA Reparatur_01].Originalanzahl, 
	                                        [Auswertung_Offene FA Reparatur_01].Anzahl AS [Menge Offen]
                                        FROM Auswertung_OffeneFAReparatur_01 AS [Auswertung_Offene FA Reparatur_01] INNER JOIN [PSZ_Nummerschlüssel Kunde] 
	                                        ON [Auswertung_Offene FA Reparatur_01].NKreis = [PSZ_Nummerschlüssel Kunde].Nummerschlüssel
                                        WHERE ((([PSZ_Nummerschlüssel Kunde].Nummerschlüssel) Like ('%{customerNumber.SqlEscape()}%')))
                                        ORDER BY [PSZ_Nummerschlüssel Kunde].Kunde;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_RepairEvaluation(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_AVEvaluation> GetAVEvaluation(DateTime dateFrom, DateTime dateTill)
			{
				if(dateFrom > dateTill)
					return null;

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT
	                                    Artikel.[Artikel-Nr],PSZ_Artikelhistorie.[Datum Änderung], Artikel.Artikelnummer, PSZ_Artikelhistorie.Änderungsbeschreibung, 
	                                    Artikel.[Bezeichnung 1], PSZ_Artikelhistorie.[Änderung von], Artikel.Umsatzsteuer, Artikel.Preiseinheit, 
	                                    Artikel.Einheit, CAST(Artikel.Größe AS DECIMAL(38,15)) AS Größe, Artikel.Exportgewicht, Artikel.Zolltarif_nr, Artikel.Gewicht, 
	                                    Artikel.Warengruppe, Artikel.Warentyp, Artikel.Produktionszeit, Artikel.Stundensatz, Artikel.Verpackungsart, 
	                                    Artikel.Verpackungsmenge, Artikel.Losgroesse, [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel].[DB I ohne], 
	                                    Bestellnummern.Einkaufspreis, Preisgruppen.Einkaufspreis AS Preisgruppen_Einkaufspreis, Preisgruppen.Verkaufspreis, 
	                                    Bestellnummern.Standardlieferant, adressen.Name1
                                    FROM (((([PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel] 
	                                    RIGHT JOIN (Artikel INNER JOIN PSZ_Artikelhistorie ON Artikel.[Artikel-Nr] = PSZ_Artikelhistorie.[Artikel-Nr]) 
	                                    ON [PSZ_Steinbacher Marge in Prozent ermitteln Alle Artikel].Artikelnummer = Artikel.Artikelnummer) 
	                                    LEFT JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN Preisgruppen 
	                                    ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) LEFT JOIN Lieferanten ON Bestellnummern.[Lieferanten-Nr] = Lieferanten.nummer) 
	                                    LEFT JOIN adressen ON Lieferanten.nummer = adressen.Nr
                                    WHERE (((PSZ_Artikelhistorie.[Datum Änderung])>='{dateFrom.ToString("yyyyMMdd")}' 
	                                    And (PSZ_Artikelhistorie.[Datum Änderung])<='{dateTill.ToString("yyyyMMdd")}') 
	                                    AND ((PSZ_Artikelhistorie.Änderungsbeschreibung) Like 'NEW-NEU%'));";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_AVEvaluation(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus> GetStockStatus(int? lagerortId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT a.[Artikel-Nr],a.Artikelnummer,a.[Bezeichnung 1], a.[Aktiv], a.[Index_Kunde], a.[Index_Kunde_Datum],a.[CustomerNumber],a.[CustomerItemNumber],a.[ProductionCountryName]
                                        ,a.[ProductionSiteName],a.[ProductionCountryCode],a.[ProductionSiteCode],a.[EdiDefault],a.[UBG], l.Bestand, l.Bestand_reserviert, 
	                                    l.ID, l.Lagerort_id, l.[letzte Bewegung], l.Mindestbestand, tmp.nbAb, tmp.nbFa, tmp.nbFAPositive
	                                FROM Artikel a Join Lager l on l.[Artikel-Nr]=a.[Artikel-Nr]
	                                Join (
	                                SELECT [Artikel-Nr], SUM(nbFAPositive) nbFAPositive, SUM(nbAB) nbAB, SUM(nbFA) nbFA, Lagerort_id FROM (
			                            /* AB */
			                            SELECT [Artikel-Nr], 0 nbFAPositive, sum(ISNULL(p.Anzahl,0)) nbAB, 0 nbFa, l.StandardLagerortId Lagerort_id FROM [angebotene Artikel] p Join [Angebote] a ON a.Nr=p.[Angebot-Nr] Left Join Lagerorte l ON l.Lagerort_id=p.Lagerort_id
				                            WHERE a.Typ='Auftragsbestätigung' AND ISNULL(a.gebucht,0)=1 AND ISNULL(p.erledigt_pos,0)=0 AND ISNULL(a.erledigt,0)=0 AND p.Anzahl>0 /*AND ISNULL(p.Fertigungsnummer,0)=0 no need or remove FA linked to AB in Positive*/ {(lagerortId.HasValue == true ? ($"AND l.StandardLagerortId={lagerortId.Value}") : "")} GROUP BY [Artikel-Nr], StandardLagerortId
			                            UNION ALL
			                            /* Positive FA */
			                            SELECT [Artikel_Nr] ArticleId, SUM(ISNULL(f.Anzahl,0)) nbFAPositive, 0 nbAB, 0 nbFa, f.Lagerort_id FROM Fertigung f 
				                            WHERE f.Kennzeichen='offen' /*AND ISNULL(f.FA_Gestartet,0)=0-- 202212-20 - Schremmer */ AND f.Anzahl>0 {(lagerortId.HasValue == true ? ($"AND f.Lagerort_id={lagerortId.Value}") : "")} GROUP BY [Artikel_Nr], Lagerort_id
			                            UNION ALL
			                            /* Negative FA */
			                            SELECT p.[Artikel_Nr] ArticleId, 0 nbFAPositive, 0 nbAB, SUM(ISNULL(p.Anzahl,0)*ISNULL(f.Anzahl,0)/(ISNULL(f.Originalanzahl,1))) nbFa, f.Lagerort_id FROM Fertigung f Join Fertigung_Positionen p ON p.ID_Fertigung=f.ID
				                            WHERE f.Kennzeichen='offen' /*AND ISNULL(f.FA_Gestartet,0)=0 -- 202212-20 - Schremmer */ AND f.Anzahl>0 {(lagerortId.HasValue == true ? ($"AND f.Lagerort_id={lagerortId.Value}") : "")} GROUP BY p.[Artikel_Nr], f.Lagerort_id
			                            ) t GROUP BY [Artikel-Nr], Lagerort_id
	                                ) tmp on tmp.[Artikel-Nr]=a.[Artikel-Nr] AND tmp.Lagerort_id=l.Lagerort_id
	                                WHERE Warengruppe='ef' AND UBG=1 AND Aktiv=1 ORDER BY Artikelnummer;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus> GetStockStatusLagers(List<int> lagerortIds)
			{
				if(lagerortIds == null || lagerortIds.Count <= 0)
				{
					return null;
				}

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT a.[Artikel-Nr],a.Artikelnummer,a.[Bezeichnung 1], a.[Aktiv], a.[Index_Kunde], a.[Index_Kunde_Datum],a.[CustomerNumber],a.[CustomerItemNumber],a.[ProductionCountryName]
                                        ,a.[ProductionSiteName],a.[ProductionCountryCode],a.[ProductionSiteCode],a.[EdiDefault],a.[UBG], l.Bestand, l.Bestand_reserviert, 
	                                    l.ID, l.Lagerort_id, l.[letzte Bewegung], l.Mindestbestand, 0 nbAb,  0 nbFa, 0 nbFAPositive
	                                FROM Artikel a Join Lager l on l.[Artikel-Nr]=a.[Artikel-Nr]
	                                WHERE Warengruppe='ef' AND UBG=1 AND Aktiv=1 AND l.Bestand<>0 AND l.Lagerort_id IN ({string.Join(",", lagerortIds)}) ORDER BY Artikelnummer;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus> GetStockStatus(List<int> articleIds)
			{
				if(articleIds == null || articleIds.Count <= 0)
				{
					return null;
				}
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT a.[Artikel-Nr],a.Artikelnummer,a.[Bezeichnung 1], a.[Aktiv], a.[Index_Kunde], a.[Index_Kunde_Datum],a.[CustomerNumber],a.[CustomerItemNumber],a.[ProductionCountryName]
                                            ,a.[ProductionSiteName],a.[ProductionCountryCode],a.[ProductionSiteCode],a.[EdiDefault],a.[UBG], 0 Bestand, 0 Bestand_reserviert, 
	                                    0 ID, 0 Lagerort_id,  getdate() [letzte Bewegung], 0 Mindestbestand, tmp.nbAb, tmp.nbFa, tmp.nbFAPositive
	                                    FROM Artikel a
	                                    Join (
	                                    SELECT [Artikel-Nr], SUM(nbFAPositive) nbFAPositive, SUM(nbAB) nbAB, SUM(nbFA) nbFA FROM (
		                                    /* AB */
		                                    SELECT [Artikel-Nr], 0 nbFAPositive, sum(ISNULL(p.Anzahl,0)) nbAB, 0 nbFa FROM [angebotene Artikel] p Join [Angebote] a ON a.Nr=p.[Angebot-Nr] 
			                                    WHERE [Artikel-Nr] IN ({string.Join(",", articleIds)}) AND a.Typ='Auftragsbestätigung' AND ISNULL(a.gebucht,0)=1 AND ISNULL(p.erledigt_pos,0)=0 AND ISNULL(a.erledigt,0)=0 AND p.Anzahl>0  GROUP BY [Artikel-Nr]
		                                    UNION ALL
		                                    /* Positive FA */
		                                    SELECT [Artikel_Nr] ArticleId, SUM(ISNULL(f.Anzahl,0)) nbFAPositive, 0 nbAB, 0 nbFa FROM Fertigung f 
			                                    WHERE [Artikel_Nr] IN ({string.Join(",", articleIds)}) AND f.Kennzeichen='offen' AND ISNULL(f.FA_Gestartet,0)=0 AND f.Anzahl>0 GROUP BY [Artikel_Nr]
		                                    UNION ALL
		                                    /* Negative FA */
		                                    SELECT p.[Artikel_Nr] ArticleId, 0 nbFAPositive, 0 nbAB, SUM(ISNULL(p.Anzahl,0)*ISNULL(f.Anzahl,0)/(ISNULL(f.Originalanzahl,1))) nbFa FROM Fertigung f Join Fertigung_Positionen p ON p.ID_Fertigung=f.ID
			                                    WHERE p.[Artikel_Nr] IN ({string.Join(",", articleIds)}) AND f.Kennzeichen='offen' AND ISNULL(f.FA_Gestartet,0)=0 AND f.Anzahl>0 GROUP BY p.[Artikel_Nr]
		                                    ) t GROUP BY [Artikel-Nr]
	                                    ) tmp on tmp.[Artikel-Nr]=a.[Artikel-Nr]
	                                    WHERE a.[Artikel-Nr] IN ({string.Join(",", articleIds)}) ORDER BY Artikelnummer;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus(x)).ToList();
				}
				else
				{
					return null;
				}
			}
		}
		public class Basics
		{
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Mindesbestand> GetMinimumStock()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT 
	                                    Artikel.[Artikel-Nr],Artikel.Artikelnummer AS [PSZ Nummer], Artikel.[Bezeichnung 1] AS Bezeichnung, 
	                                    Bestellnummern.Einkaufspreis AS EK, Lagerorte.Lagerort, Lager.Bestand AS [Aktueller Bestand], 
	                                    Lager.Bestand*Bestellnummern.Einkaufspreis AS Bestandskosten, Lager.Mindestbestand, 
	                                    Lager.Mindestbestand*Bestellnummern.Einkaufspreis AS Mindestbestandskosten
                                    FROM ((Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
	                                    INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) 
	                                    INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
                                    WHERE (((Artikel.Artikelnummer)<>'000-000-00') AND ((Lager.Mindestbestand)<>0) AND ((Bestellnummern.Standardlieferant)=1))
                                    ORDER BY Artikel.Artikelnummer;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Mindesbestand(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCirculations_TN(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, 
	                                        [bestellte Artikel].Anzahl, [bestellte Artikel].Liefertermin, 
	                                        Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] 
	                                        INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=4 Or ([bestellte Artikel].Lagerort_id)=7) 
	                                        AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) 
	                                        AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' 
	                                        Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT
	                                        Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_Tunesien.Bedarf) AS SummevonBedarf,
	                                        Kartonagen_Bedarf_Tunesien.Bestand, Kartonagen_Bedarf_Tunesien.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton.Bestätigter_Termin, 
	                                        Liste_Bestellung_Umlauf_Karton.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton.Anzahl, Liste_Bestellung_Umlauf_Karton.Liefertermin
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand 003] as Kartonagen_Bedarf_Tunesien 
	                                        LEFT JOIN Liste_Bestellung_Umlauf_Karton ON Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_Tunesien.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                        And (Kartonagen_Bedarf_Tunesien.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf, Kartonagen_Bedarf_Tunesien.Bestand, Kartonagen_Bedarf_Tunesien.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton.Anzahl, 
	                                        Liste_Bestellung_Umlauf_Karton.Liefertermin
                                        HAVING (((Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf) Like 'Um-%' Or (Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf) Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCirculations_WS(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_KH AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel 
	                                        ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=41 Or ([bestellte Artikel].Lagerort_id)=42) AND 
	                                        (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND 
	                                        ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_KHTN.Bedarf) AS SummevonBedarf, 
	                                        Kartonagen_Bedarf_KHTN.Bestand, Kartonagen_Bedarf_KHTN.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_KH.[Bestellung-Nr], 
	                                        Liste_Bestellung_Umlauf_Karton_KH.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_KH.Liefertermin, 
	                                        Liste_Bestellung_Umlauf_Karton_KH.Anzahl
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_KHTN 003] AS Kartonagen_Bedarf_KHTN LEFT JOIN Liste_Bestellung_Umlauf_Karton_KH 
	                                        ON Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_KH.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_KHTN.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                        And (Kartonagen_Bedarf_KHTN.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf, Kartonagen_Bedarf_KHTN.Bestand, Kartonagen_Bedarf_KHTN.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_KH.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_KH.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_KH.Liefertermin, Liste_Bestellung_Umlauf_Karton_KH.Anzahl
                                        HAVING (((Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf) Like 'Um-%' Or 
	                                        (Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf) Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCirculations_BETN(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_BETN AS (
                                    SELECT 
	                                    [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                    [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                    FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                    ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                    WHERE ((([bestellte Artikel].Lagerort_id)=58 Or ([bestellte Artikel].Lagerort_id)=60) AND 
	                                    (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) 
	                                    AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                    SELECT 
	                                    Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_BETN.Bedarf) AS SummevonBedarf, 
	                                    Kartonagen_Bedarf_BETN.Bestand, Kartonagen_Bedarf_BETN.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_BETN.[Bestellung-Nr], 
	                                    Liste_Bestellung_Umlauf_Karton_BETN.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_BETN.Liefertermin, 
	                                    Liste_Bestellung_Umlauf_Karton_BETN.Anzahl
                                    FROM [Kartonage_bedarf_Berechnung_TrasferBestand_BETN 003] AS Kartonagen_Bedarf_BETN LEFT JOIN Liste_Bestellung_Umlauf_Karton_BETN 
	                                    ON Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_BETN.Artikelnummer
                                    WHERE (((Kartonagen_Bedarf_BETN.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                    And (Kartonagen_Bedarf_BETN.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                    GROUP BY Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf, Kartonagen_Bedarf_BETN.Bestand, Kartonagen_Bedarf_BETN.[Transfer Bestand], 
	                                    Liste_Bestellung_Umlauf_Karton_BETN.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_BETN.Bestätigter_Termin, 
	                                    Liste_Bestellung_Umlauf_Karton_BETN.Liefertermin, Liste_Bestellung_Umlauf_Karton_BETN.Anzahl
                                    HAVING (((Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf) Like 'Um-%' 
	                                    Or (Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf) Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCirculations_GZTN(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton AS (
										SELECT 
											[bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
											[bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
										FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
											ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
										WHERE ((([bestellte Artikel].Lagerort_id)=101 Or ([bestellte Artikel].Lagerort_id)=102) AND 
											(([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) 
											AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

										SELECT 
											Kartonagen_Bedarf.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf.Bedarf) AS SummevonBedarf, 
											Kartonagen_Bedarf.Bestand, Kartonagen_Bedarf.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton.[Bestellung-Nr], 
											Liste_Bestellung_Umlauf_Karton.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton.Liefertermin, 
											Liste_Bestellung_Umlauf_Karton.Anzahl
										FROM [Kartonage_bedarf_Berechnung_TrasferBestand_GZTN 003] AS Kartonagen_Bedarf LEFT JOIN Liste_Bestellung_Umlauf_Karton 
											ON Kartonagen_Bedarf.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton.Artikelnummer
										WHERE (((Kartonagen_Bedarf.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
											And (Kartonagen_Bedarf.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
										GROUP BY Kartonagen_Bedarf.Artikelnummer_Umlauf, Kartonagen_Bedarf.Bestand, Kartonagen_Bedarf.[Transfer Bestand], 
											Liste_Bestellung_Umlauf_Karton.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton.Bestätigter_Termin, 
											Liste_Bestellung_Umlauf_Karton.Liefertermin, Liste_Bestellung_Umlauf_Karton.Anzahl
										HAVING (((Kartonagen_Bedarf.Artikelnummer_Umlauf) Like 'Um-%' 
											Or (Kartonagen_Bedarf.Artikelnummer_Umlauf) Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCirculations_AL(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_AL AS(
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=24 Or ([bestellte Artikel].Lagerort_id)=26) AND (([bestellte Artikel].erledigt_pos)=0) 
	                                        AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' 
	                                        Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_Albanien.Bedarf) AS SummevonBedarf, 
	                                        Kartonagen_Bedarf_Albanien.Bestand, Kartonagen_Bedarf_Albanien.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_AL.Bestätigter_Termin, 
	                                        Liste_Bestellung_Umlauf_Karton_AL.Anzahl, Liste_Bestellung_Umlauf_Karton_AL.Liefertermin, 
	                                        Liste_Bestellung_Umlauf_Karton_AL.[Bestellung-Nr]
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_Albanien 003] AS Kartonagen_Bedarf_Albanien LEFT JOIN Liste_Bestellung_Umlauf_Karton_AL 
	                                        ON Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_AL.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_Albanien.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' And 
	                                        (Kartonagen_Bedarf_Albanien.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf, Kartonagen_Bedarf_Albanien.Bestand, Kartonagen_Bedarf_Albanien.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton_AL.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_AL.Anzahl, Liste_Bestellung_Umlauf_Karton_AL.Liefertermin, Liste_Bestellung_Umlauf_Karton_AL.[Bestellung-Nr]
                                        HAVING (((Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf) Like 'Um-%' Or (Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf) Like 'VP%'));
                    ";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCirculations_CZ(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_CZ AS (
                                        SELECT [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=3 Or ([bestellte Artikel].Lagerort_id)=6) AND (([bestellte Artikel].erledigt_pos)=0) 
	                                        AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' 
	                                        Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_CZ.Bedarf) AS SummevonBedarf, 
	                                        Kartonagen_Bedarf_CZ.Bestand, Kartonagen_Bedarf_CZ.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_CZ.Bestätigter_Termin, 
	                                        Liste_Bestellung_Umlauf_Karton_CZ.Anzahl, Liste_Bestellung_Umlauf_Karton_CZ.Liefertermin, Liste_Bestellung_Umlauf_Karton_CZ.[Bestellung-Nr]
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_CZ 003] AS Kartonagen_Bedarf_CZ LEFT JOIN Liste_Bestellung_Umlauf_Karton_CZ 
	                                        ON Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_CZ.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_CZ.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                        And (Kartonagen_Bedarf_CZ.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf, Kartonagen_Bedarf_CZ.Bestand, Kartonagen_Bedarf_CZ.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton_CZ.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_CZ.Anzahl, Liste_Bestellung_Umlauf_Karton_CZ.Liefertermin, Liste_Bestellung_Umlauf_Karton_CZ.[Bestellung-Nr]
                                        HAVING (((Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf) Like 'Um-%' Or (Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf) Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCirculations_DE(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_D AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=8) AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) 
	                                        AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') 
	                                        AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonagen_Bedarf_DE.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_DE.Bedarf) AS SummevonBedarf, Kartonagen_Bedarf_DE.Bestand, 
	                                        Kartonagen_Bedarf_DE.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_D.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_D.Anzahl, 
	                                        Liste_Bestellung_Umlauf_Karton_D.Liefertermin, Liste_Bestellung_Umlauf_Karton_D.[Bestellung-Nr]
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_DE 003] AS Kartonagen_Bedarf_DE LEFT JOIN Liste_Bestellung_Umlauf_Karton_D 
	                                        ON Kartonagen_Bedarf_DE.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_D.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_DE.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                        And (Kartonagen_Bedarf_DE.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_DE.Artikelnummer_Umlauf, Kartonagen_Bedarf_DE.Bestand, Kartonagen_Bedarf_DE.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton_D.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_D.Anzahl, Liste_Bestellung_Umlauf_Karton_D.Liefertermin, Liste_Bestellung_Umlauf_Karton_D.[Bestellung-Nr]
                                        HAVING (((Kartonagen_Bedarf_DE.Artikelnummer_Umlauf) Like 'Um-%' Or (Kartonagen_Bedarf_DE.Artikelnummer_Umlauf) Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCirculations_Gesamt(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_T') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_T;
                                        IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_2_T') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_2_T;
                                        IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_Final') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_Final;
                                        SELECT 
	                                        Kartonagen_Bedarf_Gesamt.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_Gesamt.Bedarf) AS SummevonBedarf, 
	                                        Kartonagen_Bedarf_Gesamt.Bestand, Kartonagen_Bedarf_Gesamt.[Transfer Bestand] 
	                                        INTO #Kartonage_Bedarf_Gesamt_INT_T
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_Gesamt_003] AS Kartonagen_Bedarf_Gesamt
                                        GROUP BY Kartonagen_Bedarf_Gesamt.Artikelnummer_Umlauf, Kartonagen_Bedarf_Gesamt.Bestand, Kartonagen_Bedarf_Gesamt.[Transfer Bestand], 
	                                        Kartonagen_Bedarf_Gesamt.Termin_Bestätigt1
                                        HAVING (((Kartonagen_Bedarf_Gesamt.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                        And (Kartonagen_Bedarf_Gesamt.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'));


                                        SELECT 
	                                        Kartonage_Bedarf_Gesamt_INT_T.Artikelnummer_Umlauf, Kartonage_Bedarf_Gesamt_INT_T.SummevonBedarf AS Bedarf, 
	                                        Kartonage_Bedarf_Gesamt_INT_T.Bestand, Kartonage_Bedarf_Gesamt_INT_T.[Transfer Bestand] 
	                                        INTO #Kartonage_Bedarf_Gesamt_INT_2_T
                                        FROM #Kartonage_Bedarf_Gesamt_INT_T AS Kartonage_Bedarf_Gesamt_INT_T;


                                        SELECT 
	                                        Kartonage_Bedarf_Gesamt_INT_2_T.Artikelnummer_Umlauf, Sum(Kartonage_Bedarf_Gesamt_INT_2_T.Bedarf) AS SummevonBedarf, 
	                                        Kartonage_Bedarf_Gesamt_INT_2_T.Bestand, Kartonage_Bedarf_Gesamt_INT_2_T.[Transfer Bestand]
	                                        INTO #Kartonage_Bedarf_Gesamt_INT_Final
                                        FROM #Kartonage_Bedarf_Gesamt_INT_2_T AS Kartonage_Bedarf_Gesamt_INT_2_T
                                        GROUP BY Kartonage_Bedarf_Gesamt_INT_2_T.Artikelnummer_Umlauf, Kartonage_Bedarf_Gesamt_INT_2_T.Bestand, 
	                                        Kartonage_Bedarf_Gesamt_INT_2_T.[Transfer Bestand];
	
                                        IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_T') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_T;
                                        IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_2_T') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_2_T;

                                        WITH  Liste_Bestellung_Umlauf_Karton_Gesammt AS (
                                        SELECT
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=4 Or ([bestellte Artikel].Lagerort_id)=7 Or ([bestellte Artikel].Lagerort_id)=3 
	                                        Or ([bestellte Artikel].Lagerort_id)=6 Or ([bestellte Artikel].Lagerort_id)=24 Or ([bestellte Artikel].Lagerort_id)=26 
	                                        Or ([bestellte Artikel].Lagerort_id)=41 Or ([bestellte Artikel].Lagerort_id)=42 Or ([bestellte Artikel].Lagerort_id)=8
											Or ([bestellte Artikel].Lagerort_id)=60 Or ([bestellte Artikel].Lagerort_id)=58
											Or ([bestellte Artikel].Lagerort_id)=102 Or ([bestellte Artikel].Lagerort_id)=101) 
	                                        AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) 
	                                        AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf, Kartonage_Bedarf_Gesamt_INT_Final.SummevonBedarf, 
	                                        Kartonage_Bedarf_Gesamt_INT_Final.Bestand, Kartonage_Bedarf_Gesamt_INT_Final.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton_Gesammt.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_Gesammt.Anzahl, 
	                                        Liste_Bestellung_Umlauf_Karton_Gesammt.Liefertermin, NULL AS [Bestätigter_Termin]
                                        FROM #Kartonage_Bedarf_Gesamt_INT_Final AS Kartonage_Bedarf_Gesamt_INT_Final LEFT JOIN Liste_Bestellung_Umlauf_Karton_Gesammt 
	                                        ON Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_Gesammt.Artikelnummer
                                        GROUP BY Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf, Kartonage_Bedarf_Gesamt_INT_Final.SummevonBedarf, 
	                                        Kartonage_Bedarf_Gesamt_INT_Final.Bestand, Kartonage_Bedarf_Gesamt_INT_Final.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton_Gesammt.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_Gesammt.Anzahl, 
	                                        Liste_Bestellung_Umlauf_Karton_Gesammt.Liefertermin
                                        HAVING (((Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf) Like 'Um-%' Or 
	                                        (Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf) Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}

			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCartons_TN(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=4 Or ([bestellte Artikel].Lagerort_id)=7) AND (([bestellte Artikel].erledigt_pos)=0) 
	                                        AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' 
	                                        Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT
	                                        Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_Tunesien.Bedarf) AS SummevonBedarf, 
	                                        Kartonagen_Bedarf_Tunesien.Bestand, Kartonagen_Bedarf_Tunesien.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton.Anzahl, 
	                                        Liste_Bestellung_Umlauf_Karton.Liefertermin, Liste_Bestellung_Umlauf_Karton.[Bestellung-Nr]
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand 003] AS Kartonagen_Bedarf_Tunesien LEFT JOIN Liste_Bestellung_Umlauf_Karton 
	                                        ON Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_Tunesien.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                        And (Kartonagen_Bedarf_Tunesien.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf, Kartonagen_Bedarf_Tunesien.Bestand, 
	                                        Kartonagen_Bedarf_Tunesien.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton.Bestätigter_Termin, 
	                                        Liste_Bestellung_Umlauf_Karton.Anzahl, Liste_Bestellung_Umlauf_Karton.Liefertermin, Liste_Bestellung_Umlauf_Karton.[Bestellung-Nr]
                                        HAVING (((Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf) Not Like 'Um-%' 
	                                        And (Kartonagen_Bedarf_Tunesien.Artikelnummer_Umlauf) Not Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCartons_WS(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_KH AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, 
	                                        [bestellte Artikel].Anzahl, [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel
	                                        ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=41 Or ([bestellte Artikel].Lagerort_id)=42) 
	                                        AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) 
	                                        AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') 
	                                        AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_KHTN.Bedarf) AS SummevonBedarf, 
	                                        Kartonagen_Bedarf_KHTN.Bestand, Kartonagen_Bedarf_KHTN.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton_KH.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_KH.Bestätigter_Termin, 
	                                        Liste_Bestellung_Umlauf_Karton_KH.Liefertermin, Liste_Bestellung_Umlauf_Karton_KH.Anzahl
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_KHTN 003] AS Kartonagen_Bedarf_KHTN LEFT JOIN Liste_Bestellung_Umlauf_Karton_KH 
	                                        ON Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_KH.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_KHTN.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}'
	                                        And (Kartonagen_Bedarf_KHTN.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf, Kartonagen_Bedarf_KHTN.Bestand, Kartonagen_Bedarf_KHTN.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_KH.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_KH.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_KH.Liefertermin, Liste_Bestellung_Umlauf_Karton_KH.Anzahl
                                        HAVING (((Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf) Not Like 'Um-%' 
	                                        And (Kartonagen_Bedarf_KHTN.Artikelnummer_Umlauf) Not Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCartons_BETN(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_BETN AS (
                                    SELECT 
	                                    [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                    [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                    FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                    ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                    WHERE ((([bestellte Artikel].Lagerort_id)=58 Or ([bestellte Artikel].Lagerort_id)=60) AND (([bestellte Artikel].erledigt_pos)=0) 
	                                    AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' 
	                                    Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                    SELECT 
	                                    Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_BETN.Bedarf) AS SummevonBedarf,
	                                    Kartonagen_Bedarf_BETN.Bestand, Kartonagen_Bedarf_BETN.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_BETN.[Bestellung-Nr], 
	                                    Liste_Bestellung_Umlauf_Karton_BETN.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_BETN.Liefertermin, 
	                                    Liste_Bestellung_Umlauf_Karton_BETN.Anzahl
                                    FROM [Kartonage_bedarf_Berechnung_TrasferBestand_BETN 003] AS Kartonagen_Bedarf_BETN LEFT JOIN Liste_Bestellung_Umlauf_Karton_BETN 
	                                    ON Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_BETN.Artikelnummer
                                    WHERE (((Kartonagen_Bedarf_BETN.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' And 
	                                    (Kartonagen_Bedarf_BETN.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                    GROUP BY Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf, Kartonagen_Bedarf_BETN.Bestand, Kartonagen_Bedarf_BETN.[Transfer Bestand], 
	                                    Liste_Bestellung_Umlauf_Karton_BETN.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_BETN.Bestätigter_Termin, 
	                                    Liste_Bestellung_Umlauf_Karton_BETN.Liefertermin, Liste_Bestellung_Umlauf_Karton_BETN.Anzahl
                                    HAVING (((Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf) Not Like 'Um-%' And (Kartonagen_Bedarf_BETN.Artikelnummer_Umlauf) Not Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCartons_GZTN(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_GZTN AS (
                                    SELECT 
										[bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
										[bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
									FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
									WHERE ((([bestellte Artikel].Lagerort_id)=101 Or ([bestellte Artikel].Lagerort_id)=102) AND (([bestellte Artikel].erledigt_pos)=0) 
										AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' 
										Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

									SELECT 
										Kartonagen_Bedarf.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf.Bedarf) AS SummevonBedarf,
										Kartonagen_Bedarf.Bestand, Kartonagen_Bedarf.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_GZTN.[Bestellung-Nr], 
										Liste_Bestellung_Umlauf_Karton_GZTN.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_GZTN.Liefertermin, 
										Liste_Bestellung_Umlauf_Karton_GZTN.Anzahl
									FROM [Kartonage_bedarf_Berechnung_TrasferBestand_GZTN 003] AS Kartonagen_Bedarf LEFT JOIN Liste_Bestellung_Umlauf_Karton_GZTN 
										ON Kartonagen_Bedarf.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_GZTN.Artikelnummer
									WHERE (((Kartonagen_Bedarf.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' And 
										(Kartonagen_Bedarf.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
									GROUP BY Kartonagen_Bedarf.Artikelnummer_Umlauf, Kartonagen_Bedarf.Bestand, Kartonagen_Bedarf.[Transfer Bestand], 
										Liste_Bestellung_Umlauf_Karton_GZTN.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_GZTN.Bestätigter_Termin, 
										Liste_Bestellung_Umlauf_Karton_GZTN.Liefertermin, Liste_Bestellung_Umlauf_Karton_GZTN.Anzahl
									HAVING (((Kartonagen_Bedarf.Artikelnummer_Umlauf) Not Like 'Um-%' And (Kartonagen_Bedarf.Artikelnummer_Umlauf) Not Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCartons_AL(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_AL AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=24 Or ([bestellte Artikel].Lagerort_id)=26) AND (([bestellte Artikel].erledigt_pos)=0) 
	                                        AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' 
	                                        Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_Albanien.Bedarf) AS SummevonBedarf, Kartonagen_Bedarf_Albanien.Bestand, Kartonagen_Bedarf_Albanien.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_AL.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_AL.Liefertermin, Liste_Bestellung_Umlauf_Karton_AL.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_AL.Anzahl
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_Albanien 003] AS Kartonagen_Bedarf_Albanien LEFT JOIN Liste_Bestellung_Umlauf_Karton_AL ON Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_AL.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_Albanien.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' And 
	                                        (Kartonagen_Bedarf_Albanien.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf, Kartonagen_Bedarf_Albanien.Bestand, Kartonagen_Bedarf_Albanien.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_AL.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_AL.Liefertermin, Liste_Bestellung_Umlauf_Karton_AL.[Bestellung-Nr], Liste_Bestellung_Umlauf_Karton_AL.Anzahl
                                        HAVING (((Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf) Not Like 'Um-%' 
	                                        And (Kartonagen_Bedarf_Albanien.Artikelnummer_Umlauf) Not Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCartons_CZ(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_CZ AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=3 Or ([bestellte Artikel].Lagerort_id)=6) AND (([bestellte Artikel].erledigt_pos)=0) 
	                                        AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' 
	                                        Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_CZ.Bedarf) AS SummevonBedarf, Kartonagen_Bedarf_CZ.Bestand, 
	                                        Kartonagen_Bedarf_CZ.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_CZ.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_CZ.Anzahl, 
	                                        Liste_Bestellung_Umlauf_Karton_CZ.Liefertermin, Liste_Bestellung_Umlauf_Karton_CZ.[Bestellung-Nr]
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_CZ 003] AS Kartonagen_Bedarf_CZ LEFT JOIN Liste_Bestellung_Umlauf_Karton_CZ ON Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_CZ.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_CZ.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}'
	                                        And (Kartonagen_Bedarf_CZ.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf, Kartonagen_Bedarf_CZ.Bestand, Kartonagen_Bedarf_CZ.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_CZ.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_CZ.Anzahl, Liste_Bestellung_Umlauf_Karton_CZ.Liefertermin, Liste_Bestellung_Umlauf_Karton_CZ.[Bestellung-Nr]
                                        HAVING (((Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf) Not Like 'Um-%' And (Kartonagen_Bedarf_CZ.Artikelnummer_Umlauf) Not Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCartons_DE(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"WITH Liste_Bestellung_Umlauf_Karton_D AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=8) AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) 
	                                        AND ((Bestellungen.gebucht)=1) AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') 
	                                        AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonagen_Bedarf_DE.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_DE.Bedarf) AS SummevonBedarf, 
	                                        Kartonagen_Bedarf_DE.Bestand, Kartonagen_Bedarf_DE.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_D.Bestätigter_Termin, 
	                                        Liste_Bestellung_Umlauf_Karton_D.Anzahl, Liste_Bestellung_Umlauf_Karton_D.Liefertermin, Liste_Bestellung_Umlauf_Karton_D.[Bestellung-Nr]
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_DE 003] AS Kartonagen_Bedarf_DE LEFT JOIN Liste_Bestellung_Umlauf_Karton_D 
	                                        ON Kartonagen_Bedarf_DE.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_D.Artikelnummer
                                        WHERE (((Kartonagen_Bedarf_DE.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                        And (Kartonagen_Bedarf_DE.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'))
                                        GROUP BY Kartonagen_Bedarf_DE.Artikelnummer_Umlauf, Kartonagen_Bedarf_DE.Bestand, Kartonagen_Bedarf_DE.[Transfer Bestand], Liste_Bestellung_Umlauf_Karton_D.Bestätigter_Termin, Liste_Bestellung_Umlauf_Karton_D.Anzahl, Liste_Bestellung_Umlauf_Karton_D.Liefertermin, Liste_Bestellung_Umlauf_Karton_D.[Bestellung-Nr]
                                        HAVING (((Kartonagen_Bedarf_DE.Artikelnummer_Umlauf) Not Like 'Um-%' And (Kartonagen_Bedarf_DE.Artikelnummer_Umlauf) Not Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf> GetCartons_Gesamt(DateTime dateFrom, DateTime dateTill)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_T') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_T;
                                        IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_2_T') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_2_T;
                                        IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_Final') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_Final;
                                        SELECT 
	                                        Kartonagen_Bedarf_Gesamt.Artikelnummer_Umlauf, Sum(Kartonagen_Bedarf_Gesamt.Bedarf) AS SummevonBedarf, 
	                                        Kartonagen_Bedarf_Gesamt.Bestand, Kartonagen_Bedarf_Gesamt.[Transfer Bestand] 
	                                        INTO #Kartonage_Bedarf_Gesamt_INT_T
                                        FROM [Kartonage_bedarf_Berechnung_TrasferBestand_Gesamt_003] AS Kartonagen_Bedarf_Gesamt
                                        GROUP BY Kartonagen_Bedarf_Gesamt.Artikelnummer_Umlauf, Kartonagen_Bedarf_Gesamt.Bestand, Kartonagen_Bedarf_Gesamt.[Transfer Bestand], 
	                                        Kartonagen_Bedarf_Gesamt.Termin_Bestätigt1
                                        HAVING (((Kartonagen_Bedarf_Gesamt.Termin_Bestätigt1)>='{dateFrom.ToString("yyyyMMdd")}' 
	                                        And (Kartonagen_Bedarf_Gesamt.Termin_Bestätigt1)<='{dateTill.ToString("yyyyMMdd")}'));

                                        SELECT 
	                                        Kartonage_Bedarf_Gesamt_INT_T.Artikelnummer_Umlauf, Kartonage_Bedarf_Gesamt_INT_T.SummevonBedarf AS Bedarf, 
	                                        Kartonage_Bedarf_Gesamt_INT_T.Bestand, Kartonage_Bedarf_Gesamt_INT_T.[Transfer Bestand] 
	                                        INTO #Kartonage_Bedarf_Gesamt_INT_2_T
                                        FROM #Kartonage_Bedarf_Gesamt_INT_T AS Kartonage_Bedarf_Gesamt_INT_T;

                                        SELECT 
	                                        Kartonage_Bedarf_Gesamt_INT_2_T.Artikelnummer_Umlauf, Sum(Kartonage_Bedarf_Gesamt_INT_2_T.Bedarf) AS SummevonBedarf, 
	                                        Kartonage_Bedarf_Gesamt_INT_2_T.Bestand, Kartonage_Bedarf_Gesamt_INT_2_T.[Transfer Bestand]
	                                        INTO #Kartonage_Bedarf_Gesamt_INT_Final
                                        FROM #Kartonage_Bedarf_Gesamt_INT_2_T AS Kartonage_Bedarf_Gesamt_INT_2_T
                                        GROUP BY Kartonage_Bedarf_Gesamt_INT_2_T.Artikelnummer_Umlauf, Kartonage_Bedarf_Gesamt_INT_2_T.Bestand, 
	                                        Kartonage_Bedarf_Gesamt_INT_2_T.[Transfer Bestand];
	
                                        IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_T') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_T;
                                        IF OBJECT_ID('tempdb..#Kartonage_Bedarf_Gesamt_INT_2_T') IS NOT NULL DROP TABLE #Kartonage_Bedarf_Gesamt_INT_2_T;
                                        WITH Liste_Bestellung_Umlauf_Karton_Gesammt AS (
                                        SELECT 
	                                        [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, [bestellte Artikel].Anzahl, 
	                                        [bestellte Artikel].Liefertermin, Artikel.Artikelnummer, Bestellungen.[Bestellung-Nr]
                                        FROM Bestellungen INNER JOIN ([bestellte Artikel] INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
	                                        ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
                                        WHERE ((([bestellte Artikel].Lagerort_id)=4 Or ([bestellte Artikel].Lagerort_id)=7 Or ([bestellte Artikel].Lagerort_id)=3	
	                                        Or ([bestellte Artikel].Lagerort_id)=6 Or ([bestellte Artikel].Lagerort_id)=24 Or ([bestellte Artikel].Lagerort_id)=26 
	                                        Or ([bestellte Artikel].Lagerort_id)=41 Or ([bestellte Artikel].Lagerort_id)=42 Or ([bestellte Artikel].Lagerort_id)=8
											Or ([bestellte Artikel].Lagerort_id)=60 Or ([bestellte Artikel].Lagerort_id)=58
											Or ([bestellte Artikel].Lagerort_id)=102 Or ([bestellte Artikel].Lagerort_id)=101) 
	                                        AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) 
	                                        AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>getDate()-400)))

                                        SELECT 
	                                        Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf, Kartonage_Bedarf_Gesamt_INT_Final.SummevonBedarf, 
	                                        Kartonage_Bedarf_Gesamt_INT_Final.Bestand, Kartonage_Bedarf_Gesamt_INT_Final.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton_Gesammt.Anzahl, Liste_Bestellung_Umlauf_Karton_Gesammt.Liefertermin, 
	                                        Liste_Bestellung_Umlauf_Karton_Gesammt.[Bestellung-Nr], NULL AS [Bestätigter_Termin]
                                        FROM #Kartonage_Bedarf_Gesamt_INT_Final AS Kartonage_Bedarf_Gesamt_INT_Final LEFT JOIN Liste_Bestellung_Umlauf_Karton_Gesammt 
	                                        ON Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf = Liste_Bestellung_Umlauf_Karton_Gesammt.Artikelnummer
                                        GROUP BY Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf, Kartonage_Bedarf_Gesamt_INT_Final.SummevonBedarf, 
	                                        Kartonage_Bedarf_Gesamt_INT_Final.Bestand, Kartonage_Bedarf_Gesamt_INT_Final.[Transfer Bestand], 
	                                        Liste_Bestellung_Umlauf_Karton_Gesammt.Anzahl, Liste_Bestellung_Umlauf_Karton_Gesammt.Liefertermin, 
	                                        Liste_Bestellung_Umlauf_Karton_Gesammt.[Bestellung-Nr]
                                        HAVING (((Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf) Not Like 'Um-%' 
	                                        And (Kartonage_Bedarf_Gesamt_INT_Final.Artikelnummer_Umlauf) Not Like 'VP%'));
";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Umlauf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_MaterialBestandProd> GetMaterialBestandProd(string materialnummer, int? lagerId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"IF OBJECT_ID('tempdb..#PSZ_Materialanalyse_in_Produktion_02') IS NOT NULL DROP TABLE #PSZ_Materialanalyse_in_Produktion_02;
                                        WITH PSZ_Materialanalyse_in_Produktion_01 AS (
                                        SELECT Fertigung.Fertigungsnummer, Fertigung.Kennzeichen, Fertigung.Anzahl FA_Anzahl, Stücklisten.Anzahl, 
	                                        Artikel.[Artikel-Nr] as ArtikleNr,Artikel.Artikelnummer, Fertigung.Kabel_geschnitten, Fertigung.Lagerort_id FA_Lagerort_id, Lager.Lagerort_id, 
	                                        Lager.Bestand, [Fertigung].[Anzahl]*[Stücklisten].[Anzahl] AS In_Produktion
                                        FROM ((Fertigung INNER JOIN Stücklisten ON Fertigung.Artikel_Nr = Stücklisten.[Artikel-Nr]) INNER JOIN Artikel 
	                                        ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]) INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]
                                        WHERE (((Fertigung.Kennzeichen)='offen') AND ((Artikel.Artikelnummer)=@materialnummer) 
	                                        AND ((Fertigung.Kabel_geschnitten)=1) {(lagerId.HasValue ? " AND ((Fertigung.Lagerort_id)=@lagerId) AND ((Lager.Lagerort_id)=@lagerId)" : "")}))

                                        SELECT 
											PSZ_Materialanalyse_in_Produktion_01.ArtikleNr,
	                                        PSZ_Materialanalyse_in_Produktion_01.Artikelnummer, PSZ_Materialanalyse_in_Produktion_01.Bestand, 
	                                        Sum(PSZ_Materialanalyse_in_Produktion_01.In_Produktion) AS SummevonIn_Produktion
	                                        INTO #PSZ_Materialanalyse_in_Produktion_02
                                        FROM PSZ_Materialanalyse_in_Produktion_01
                                        GROUP BY PSZ_Materialanalyse_in_Produktion_01.ArtikleNr,  PSZ_Materialanalyse_in_Produktion_01.Artikelnummer, PSZ_Materialanalyse_in_Produktion_01.Bestand;


                                        SELECT 
											PSZ_Materialanalyse_in_Produktion_02.ArtikleNr,
	                                        PSZ_Materialanalyse_in_Produktion_02.Artikelnummer, PSZ_Materialanalyse_in_Produktion_02.Bestand, 
	                                        PSZ_Materialanalyse_in_Produktion_02.SummevonIn_Produktion AS In_Produktion, 
	                                        [PSZ_Materialanalyse_in_Produktion_02].[Bestand]-[PSZ_Materialanalyse_in_Produktion_02].[SummevonIn_Produktion] AS Im_Lager
                                        FROM #PSZ_Materialanalyse_in_Produktion_02 AS PSZ_Materialanalyse_in_Produktion_02;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("materialnummer", materialnummer);
					if(lagerId.HasValue)
						sqlCommand.Parameters.AddWithValue("lagerId", lagerId);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_MaterialBestandProd(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_MaterialBestandAnalyse> GetMinimumStockAnalyse(int lager)
			{
				try
				{
					var dataTable = new DataTable();
					using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
					{
						sqlConnection.Open();
						string query = $@"IF OBJECT_ID('tempdb..#PSZ_Auswertung_Mindestbestaende_TN_02') IS NOT NULL DROP TABLE #PSZ_Auswertung_Mindestbestaende_TN_02;

                                    WITH PSZ_Auswertung_Mindestbestaende_TN_01 AS (
                                    SELECT Fertigung.Termin_Bestätigt1, Fertigung.Anzahl FA_Anzahl, Stücklisten.Anzahl, [Fertigung].[Anzahl]*[Stücklisten].[Anzahl] AS Bedarf, 
                                    Artikel.[Artikel-Nr],Artikel.Artikelnummer, Fertigung.Lagerort_id
                                    FROM (Fertigung INNER JOIN Stücklisten ON Fertigung.Artikel_Nr = Stücklisten.[Artikel-Nr]) INNER JOIN Artikel ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]
                                    WHERE (((Fertigung.Termin_Bestätigt1)<=getDate()+60) AND ((Fertigung.Lagerort_id)=@lager) AND ((Fertigung.Kennzeichen)='offen')))

                                    SELECT PSZ_Auswertung_Mindestbestaende_TN_01.[Artikel-Nr],PSZ_Auswertung_Mindestbestaende_TN_01.Artikelnummer, 
	                                    Sum(PSZ_Auswertung_Mindestbestaende_TN_01.Bedarf) AS SummevonBedarf, Lager.Bestand, Lager.Mindestbestand, 
	                                    Bestellnummern.Wiederbeschaffungszeitraum, Lager.Lagerort_id
	                                    INTO #PSZ_Auswertung_Mindestbestaende_TN_02
                                    FROM (PSZ_Auswertung_Mindestbestaende_TN_01 INNER JOIN (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
	                                    ON PSZ_Auswertung_Mindestbestaende_TN_01.Artikelnummer = Artikel.Artikelnummer) INNER JOIN Bestellnummern 
	                                    ON Lager.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
                                    GROUP BY PSZ_Auswertung_Mindestbestaende_TN_01.[Artikel-Nr],PSZ_Auswertung_Mindestbestaende_TN_01.Artikelnummer, Lager.Bestand, Lager.Mindestbestand, Bestellnummern.Wiederbeschaffungszeitraum, Lager.Lagerort_id, Bestellnummern.Standardlieferant
                                    HAVING (((Lager.Lagerort_id)=@lager) AND ((Bestellnummern.Standardlieferant)=1))
                                    ORDER BY PSZ_Auswertung_Mindestbestaende_TN_01.Artikelnummer;

                                    SELECT PSZ_Auswertung_Mindestbestaende_TN_02.[Artikel-Nr],PSZ_Auswertung_Mindestbestaende_TN_02.Artikelnummer, PSZ_Auswertung_Mindestbestaende_TN_02.SummevonBedarf AS Bedarf_nächste_8_Wochen, 
	                                    [summevonBedarf]*(([Wiederbeschaffungszeitraum]+0.1)/60)*0.25 AS Empfohlener_Mindestbestand, 
	                                    PSZ_Auswertung_Mindestbestaende_TN_02.Mindestbestand, [Mindestbestand]-([summevonBedarf]*(([Wiederbeschaffungszeitraum]+0.1)/60)*0.25) AS Abweichung, 
	                                    PSZ_Auswertung_Mindestbestaende_TN_02.Wiederbeschaffungszeitraum, PSZ_Auswertung_Mindestbestaende_TN_02.Bestand
                                    FROM #PSZ_Auswertung_Mindestbestaende_TN_02 AS PSZ_Auswertung_Mindestbestaende_TN_02
                                    ORDER BY [summevonBedarf]*(([Wiederbeschaffungszeitraum]+0.1)/60)*0.25 DESC;";
						var sqlCommand = new SqlCommand(query, sqlConnection);
						sqlCommand.Parameters.AddWithValue("lager", lager);

						DbExecution.Fill(sqlCommand, dataTable);
					}

					if(dataTable.Rows.Count > 0)
					{
						return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_MaterialBestandAnalyse(x)).ToList();
					}
					else
					{
						return null;
					}
				} catch(Exception e)
				{
					throw;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_OFFeneFaEsd> GetOpenFaEsd(int lagerId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT Artikel.[Artikel-Nr],Artikel.Artikelnummer, Artikel_1.Artikelnummer AS ESD_ARTIKEL, Fertigung.Fertigungsnummer, Fertigung.Lagerort_id, 
	                                    Fertigung.Kennzeichen, Fertigung.Termin_Bestätigt1
                                    FROM ((Artikel INNER JOIN Fertigung ON Artikel.[Artikel-Nr] = Fertigung.Artikel_Nr) INNER JOIN Fertigung_Positionen 
	                                    ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung) INNER JOIN Artikel AS Artikel_1 
	                                    ON Fertigung_Positionen.Artikel_Nr = Artikel_1.[Artikel-Nr]
                                    WHERE (((Fertigung.Lagerort_id)=@lagerId) AND ((Fertigung.Kennzeichen)='Offen') AND ((Artikel_1.ESD_Schutz)=1));";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("lagerId", lagerId);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_OFFeneFaEsd(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProjektartArtikel> GetArticleProjectType(string searchTerms, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				searchTerms = searchTerms?.Trim()?.ToLower().SqlEscape();
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT [Artikel-Nr],Artikelnummer, Warengruppe, Zeichnungsnummer, artikelklassifizierung, Freigabestatus
                                        FROM Artikel WHERE (((Artikel.Warengruppe)='EF'))";
					if(!string.IsNullOrWhiteSpace(searchTerms))
					{
						query += $" AND (Artikelnummer LIKE '{searchTerms}%' OR Warengruppe LIKE '{searchTerms}%' OR Zeichnungsnummer LIKE '{searchTerms}%' OR artikelklassifizierung LIKE '{searchTerms}%' OR Freigabestatus LIKE '{searchTerms}%')";
					}
					query += $" ORDER BY Artikelnummer, artikelklassifizierung OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProjektartArtikel(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetArticleProjectType_count(string searchTerms)
			{
				searchTerms = searchTerms?.Trim()?.ToLower().SqlEscape();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					string query = $@"SELECT COUNT(*) FROM Artikel WHERE (((Artikel.Warengruppe)='EF'))";
					if(!string.IsNullOrWhiteSpace(searchTerms))
					{
						query += $" AND (Artikelnummer LIKE '{searchTerms}%' OR Warengruppe LIKE '{searchTerms}%' OR Zeichnungsnummer LIKE '{searchTerms}%' OR artikelklassifizierung LIKE '{searchTerms}%' OR Freigabestatus LIKE '{searchTerms}%')";
					}

					var sqlCommand = new SqlCommand(query, sqlConnection);
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var val) ? val : 0;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProofOfUsage> GetProofOfUsage(int articleId, string customerNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT 
	                                    Artikel.[Artikel-Nr],Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Stücklisten.Anzahl, Stücklisten.Variante, Stücklisten.Position, Artikel.[Bezeichnung 2], 
	                                    Artikel.Freigabestatus, Artikel.Sysmonummer/*, Artikel_1.Sysmonummer*/, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge, Artikel.Rahmenauslauf
                                    FROM (Artikel INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
	                                    INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]
                                    WHERE (((Artikel.Artikelnummer) Like (@customerNumber + '%')) AND ((Artikel.Freigabestatus)='F' Or (Artikel.Freigabestatus)='N' 
	                                    Or (Artikel.Freigabestatus)='P' Or (Artikel.Freigabestatus)='X' Or (Artikel.Freigabestatus)='E' Or (Artikel.Freigabestatus)='A' 
	                                    Or (Artikel.Freigabestatus)='T' Or (Artikel.Freigabestatus)='O' Or (Artikel.Freigabestatus)='RP' Or (Artikel.Freigabestatus)='FT' 
	                                    Or (Artikel.Freigabestatus)='ES' Or (Artikel.Freigabestatus)='FR') AND ((Stücklisten.[Artikel-Nr des Bauteils])=@articleId));";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("articleId", articleId);
					sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProofOfUsage(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf> GetBedarf(string queryParamed)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("customerNumber", queryParamed);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public class BruttoBedarf
			{
				public static Tuple<
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Liferant>,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bestellung>,
			string>
			getQueryParamed(int userId, long Artikel_Nr, string Artikelnummer, int Land, string fertigungNumber, string fertigungLager, List<int> aLVirtualBestandArticleIds)
				{
					var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

					string userID = (userId * userId).ToString();
					string dispoBedarfTableName = $"[___PSZTN_Disposition_BedarfTN_{userID}_________M_{Timestamp}]";
					string dispoLieferTableName = $"[___PSZTN_Disposition_Liferant_{userID}_________M_{Timestamp}]";
					string dispoBestellTableName = $"[___PSZTN_Disposition_Bestellung_{userID}_________M_{Timestamp}]";
					string PLANNED_ENTRY = "eingeplante zugang";

					try
					{

						var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)Artikel_Nr);
						//string Artikelnummer = articleNumber;
						//long Artikel_Nr = articleId;
						//int Land = this._data.Land;
						//string fertigungNumber, fertigungLager;
						//switch(Land)
						//{
						//	case 1:
						//		fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.TN}";
						//		fertigungLager = $"Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.TN.GetDescription()}";
						//		break;
						//	case 2:
						//		fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.WS}";
						//		fertigungLager = $"{Common.Enums.ArticleEnums.ArticleProductionPlace.WS.GetDescription()}";
						//		break;
						//	case 3:
						//		fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.AL}";
						//		fertigungLager = $"Albanien - {Common.Enums.ArticleEnums.ArticleProductionPlace.AL.GetDescription()}";
						//		break;
						//	case 4:
						//		fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.CZ}";
						//		fertigungLager = $"Eigenfertigung - {Common.Enums.ArticleEnums.ArticleProductionPlace.CZ.GetDescription()}";
						//		break;
						//	case 5:
						//		fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.DE}";
						//		fertigungLager = $"Fertigung - {Common.Enums.ArticleEnums.ArticleProductionPlace.DE.GetDescription()}";
						//		break;
						//	case 6:
						//		fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.BETN}";
						//		fertigungLager = $"Benane Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.BETN.GetDescription()}";
						//		break;
						//	case 7:
						//		fertigungNumber = $"{(int)Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN}";
						//		fertigungLager = $"Ghezala Tunesien - {Common.Enums.ArticleEnums.ArticleProductionPlace.GZTN.GetDescription()}";
						//		break;
						//	default:
						//		fertigungNumber = "";
						//		fertigungLager = "";
						//		break;
						//}

						// - 
						//List<RST> rst = new List<RST>();
						//List<RST1> rst1 = new List<RST1>();
						//List<RST2> rst2 = new List<RST2>();
						List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST3> rst3 = new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST3>();

						#region >>>>>>> Bloc 1
						string Ch;
						string Ch1 = "";
						string Ch3 = "";
						string Lager = "";
						string Lager_Bestand = "";
						string Lager_Best = "";
						string Lager_Bestand1 = "";
						string Lager_Bestand2 = "";
						if(Land == 1)
						{
							Lager = "(Fertigung.Lagerort_id)=4 Or (Fertigung.Lagerort_id)=7 Or (Fertigung.Lagerort_id)=30 Or (Fertigung.Lagerort_id)=29 Or (Fertigung.Lagerort_id)=10 Or (Fertigung.Lagerort_id)=23 Or (Fertigung.Lagerort_id)=56";
							Lager_Bestand = "((Lager.Lagerort_id) = 4 Or (Lager.Lagerort_id) = 7 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 29 Or (Lager.Lagerort_id) = 30 Or (Lager.Lagerort_id) = 10 Or (Lager.Lagerort_id) = 23 Or (Lager.Lagerort_id) = 56 Or (Lager.Lagerort_id) = 200 Or (Lager.Lagerort_id) = 205)";
							Lager_Best = "([bestellte Artikel].Lagerort_id) = 4 Or ([bestellte Artikel].Lagerort_id) = 7)";
							Lager_Bestand1 = "((Lager.Lagerort_id) = 4 Or (Lager.Lagerort_id) = 7 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 29 Or (Lager.Lagerort_id) = 30  Or (Lager.Lagerort_id)=10 Or (Lager.Lagerort_id) = 23  Or (Lager.Lagerort_id) = 56 )";
							Lager_Bestand2 = "((Lager.Lagerort_id) = 4 Or (Lager.Lagerort_id) = 7 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 29 Or (Lager.Lagerort_id) = 30  Or (Lager.Lagerort_id)=10 Or (Lager.Lagerort_id) = 23  Or (Lager.Lagerort_id) = 56 Or (Lager.Lagerort_id) = 77)";
						}
						else if(Land == 2)
						{
							Lager = "(Fertigung.Lagerort_id)=41 Or (Fertigung.Lagerort_id)=42 Or (Fertigung.Lagerort_id)=47 Or (Fertigung.Lagerort_id)=46 Or (Fertigung.Lagerort_id)=40 Or (Fertigung.Lagerort_id)=57";
							Lager_Bestand = "((Lager.Lagerort_id)=41 Or (Lager.Lagerort_id)=42 Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=46 Or (Lager.Lagerort_id)=47 Or (Lager.Lagerort_id)=49 Or (Lager.Lagerort_id)=40 Or (Lager.Lagerort_id)=57 Or (Lager.Lagerort_id) = 201 Or (Lager.Lagerort_id) = 203)";
							Lager_Best = "([bestellte Artikel].Lagerort_id)=41 Or ([bestellte Artikel].Lagerort_id)=42)";
							Lager_Bestand1 = "((Lager.Lagerort_id) = 41 Or (Lager.Lagerort_id) = 42 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 46 Or (Lager.Lagerort_id) = 47 Or (Lager.Lagerort_id) = 49 Or (Lager.Lagerort_id) = 40 Or (Lager.Lagerort_id) = 57)";
							Lager_Bestand2 = "((Lager.Lagerort_id) = 41 Or (Lager.Lagerort_id) = 42 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 46 Or (Lager.Lagerort_id) = 47 Or (Lager.Lagerort_id) = 49 Or (Lager.Lagerort_id) = 40 Or (Lager.Lagerort_id) = 57 Or (Lager.Lagerort_id) = 420)";

							/* - 2024-12-02 - Khelil merge Lager TN into WS */
							Lager = @"((Fertigung.Lagerort_id)=41 Or (Fertigung.Lagerort_id)=42 Or (Fertigung.Lagerort_id)=47 Or (Fertigung.Lagerort_id)=46 Or (Fertigung.Lagerort_id)=40 Or (Fertigung.Lagerort_id)=57 
											OR /*** TN ***/ (Fertigung.Lagerort_id)=4 Or (Fertigung.Lagerort_id)=7 Or (Fertigung.Lagerort_id)=30 Or (Fertigung.Lagerort_id)=29 Or (Fertigung.Lagerort_id)=10 Or (Fertigung.Lagerort_id)=23 Or (Fertigung.Lagerort_id)=56)";
							Lager_Bestand = @"(((Lager.Lagerort_id)=41 Or (Lager.Lagerort_id)=42 Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=46 Or (Lager.Lagerort_id)=47 Or (Lager.Lagerort_id)=49 Or (Lager.Lagerort_id)=40 Or (Lager.Lagerort_id)=57 Or (Lager.Lagerort_id) = 201 Or (Lager.Lagerort_id) = 203) 
											/*** TN *** / OR ((Lager.Lagerort_id) = 4 Or (Lager.Lagerort_id) = 7 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 29 Or (Lager.Lagerort_id) = 30 Or (Lager.Lagerort_id) = 10 Or (Lager.Lagerort_id) = 23 Or (Lager.Lagerort_id) = 56 Or (Lager.Lagerort_id) = 200 Or (Lager.Lagerort_id) = 205)*/)";
							Lager_Best = @"(([bestellte Artikel].Lagerort_id)=41 Or ([bestellte Artikel].Lagerort_id)=42) 
											/*** TN *** / OR ([bestellte Artikel].Lagerort_id) = 4 Or ([bestellte Artikel].Lagerort_id) = 7*/)";
							Lager_Bestand1 = @"(((Lager.Lagerort_id) = 41 Or (Lager.Lagerort_id) = 42 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 46 Or (Lager.Lagerort_id) = 47 Or (Lager.Lagerort_id) = 49 Or (Lager.Lagerort_id) = 40 Or (Lager.Lagerort_id) = 57)
											/*** TN ***/ OR Lager.Lagerort_id = 29 Or Lager.Lagerort_id = 30 Or Lager.Lagerort_id = 201)";
							Lager_Bestand2 = @"(((Lager.Lagerort_id) = 41 Or (Lager.Lagerort_id) = 42 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 46 Or (Lager.Lagerort_id) = 47 Or (Lager.Lagerort_id) = 49 Or (Lager.Lagerort_id) = 40 Or (Lager.Lagerort_id) = 57 Or (Lager.Lagerort_id) = 420)
											/*** TN ***/ OR Lager.Lagerort_id = 29 Or Lager.Lagerort_id = 30  Or Lager.Lagerort_id = 201)";
						}
						else if(Land == 3)
						{
							Lager = " (Fertigung.Lagerort_id)=24 Or (Fertigung.Lagerort_id)=25 Or (Fertigung.Lagerort_id)=26 Or (Fertigung.Lagerort_id)=34 Or (Fertigung.Lagerort_id)=35 Or (Fertigung.Lagerort_id)=50";
							Lager_Bestand = " ((Lager.Lagerort_id)=24 Or (Lager.Lagerort_id)=25 Or (Lager.Lagerort_id)=26 Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=34 Or (Lager.Lagerort_id)=35 Or (Lager.Lagerort_id)=50)";
							Lager_Best = " ([bestellte Artikel].Lagerort_id)=24 Or ([bestellte Artikel].Lagerort_id)=26 Or ([bestellte Artikel].Lagerort_id)=25 Or ([bestellte Artikel].Lagerort_id)=34 Or ([bestellte Artikel].Lagerort_id)=35 Or ([bestellte Artikel].Lagerort_id)=50)";
							Lager_Bestand1 = "((Lager.Lagerort_id) = 24 Or (Lager.Lagerort_id) = 26 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 34 Or (Lager.Lagerort_id) = 35 Or (Lager.Lagerort_id) = 25 Or (Lager.Lagerort_id) = 50)";
							Lager_Bestand2 = "((Lager.Lagerort_id) = 24 Or (Lager.Lagerort_id) = 26 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 34 Or (Lager.Lagerort_id) = 35 Or (Lager.Lagerort_id) = 25  Or (Lager.Lagerort_id) = 50 Or (Lager.Lagerort_id) = 260)";
						}
						else if(Land == 4)
						{
							Lager = " (Fertigung.Lagerort_id)=3 Or (Fertigung.Lagerort_id)=6 Or (Fertigung.Lagerort_id)=20 Or (Fertigung.Lagerort_id)=21";
							Lager_Bestand = " ((Lager.Lagerort_id)=3 Or (Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=9   Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=52 Or (Lager.Lagerort_id)=53 Or (Lager.Lagerort_id)=54 Or (Lager.Lagerort_id)=55)";
							Lager_Best = " ([bestellte Artikel].Lagerort_id)=3 Or ([bestellte Artikel].Lagerort_id)=6 Or ([bestellte Artikel].Lagerort_id)=20 Or ([bestellte Artikel].Lagerort_id)=21)";
							Lager_Bestand1 = "((Lager.Lagerort_id) = 3 Or (Lager.Lagerort_id) = 9  Or (Lager.Lagerort_id) = 6 Or (Lager.Lagerort_id) = 22   Or (Lager.Lagerort_id) = 52  Or (Lager.Lagerort_id) = 53 Or (Lager.Lagerort_id) = 17)";
							Lager_Bestand2 = "((Lager.Lagerort_id) = 3 Or (Lager.Lagerort_id) = 9 Or (Lager.Lagerort_id) = 6 Or (Lager.Lagerort_id) = 22   Or (Lager.Lagerort_id) = 52  Or (Lager.Lagerort_id) = 53 Or (Lager.Lagerort_id) = 66 Or (Lager.Lagerort_id) = 17)";
						}
						else if(Land == 5)
						{
							Lager = " (Fertigung.Lagerort_id)=14 Or (Fertigung.Lagerort_id)=15";
							Lager_Bestand = " ((Lager.Lagerort_id)=14 Or (Lager.Lagerort_id)=15  Or (Lager.Lagerort_id)=22)";
							Lager_Best = " ([bestellte Artikel].Lagerort_id)=14 Or ([bestellte Artikel].Lagerort_id)=15 Or ([bestellte Artikel].Lagerort_id)=8)";
						}
						else if(Land == 6)
						{
							Lager = "(Fertigung.Lagerort_id)=58 Or (Fertigung.Lagerort_id)=60 Or (Fertigung.Lagerort_id)=64 Or (Fertigung.Lagerort_id)=63 Or (Fertigung.Lagerort_id)=65 Or (Fertigung.Lagerort_id)=61";
							Lager_Bestand = "((Lager.Lagerort_id)=58 Or (Lager.Lagerort_id)=60 Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=63 Or (Lager.Lagerort_id)=64 Or (Lager.Lagerort_id)=61 Or (Lager.Lagerort_id)=-1)";
							Lager_Best = "([bestellte Artikel].Lagerort_id)=58 Or ([bestellte Artikel].Lagerort_id)=60 Or ([bestellte Artikel].Lagerort_id)=60)";
							Lager_Bestand1 = "((Lager.Lagerort_id) = 58 Or (Lager.Lagerort_id) = 60 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 63 Or (Lager.Lagerort_id) = 64 Or (Lager.Lagerort_id) = 59 Or (Lager.Lagerort_id) = 61 Or (Lager.Lagerort_id) = 65)";
							Lager_Bestand2 = "((Lager.Lagerort_id) = 58 Or (Lager.Lagerort_id) = 60 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 63 Or (Lager.Lagerort_id) = 64 Or (Lager.Lagerort_id) = 59 Or (Lager.Lagerort_id) = 61 Or (Lager.Lagerort_id) = 65 Or (Lager.Lagerort_id) = 580)";
						}
						else if(Land == 7)
						{
							Lager = "(Fertigung.Lagerort_id)=101 Or (Fertigung.Lagerort_id)=102 Or (Fertigung.Lagerort_id)=108 Or (Fertigung.Lagerort_id)=107 Or (Fertigung.Lagerort_id)=109 Or (Fertigung.Lagerort_id)=105";
							Lager_Bestand = "((Lager.Lagerort_id)=101 Or (Lager.Lagerort_id)=102 Or (Lager.Lagerort_id)=22 Or (Lager.Lagerort_id)=107 Or (Lager.Lagerort_id)=108 Or (Lager.Lagerort_id)=105 Or (Lager.Lagerort_id)=-1 Or (Lager.Lagerort_id) = 202 Or (Lager.Lagerort_id) = 204)";
							Lager_Best = "([bestellte Artikel].Lagerort_id)=101 Or ([bestellte Artikel].Lagerort_id)=102 Or ([bestellte Artikel].Lagerort_id)=102)";
							Lager_Bestand1 = "((Lager.Lagerort_id) = 101 Or (Lager.Lagerort_id) = 102 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 107 Or (Lager.Lagerort_id) = 108 Or (Lager.Lagerort_id) = 104 Or (Lager.Lagerort_id) = 105 Or (Lager.Lagerort_id) = 109 Or (Lager.Lagerort_id) = 204)";
							Lager_Bestand2 = "((Lager.Lagerort_id) = 101 Or (Lager.Lagerort_id) = 102 Or (Lager.Lagerort_id) = 22 Or (Lager.Lagerort_id) = 107 Or (Lager.Lagerort_id) = 108 Or (Lager.Lagerort_id) = 104 Or (Lager.Lagerort_id) = 105 Or (Lager.Lagerort_id) = 109 Or (Lager.Lagerort_id) = 103 Or (Lager.Lagerort_id) = 204)";
						}


						if(Land == 2)
						{
							/* - 2024-12-02 - Khelil merge Lager TN into WS */
							Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "42", "420", Lager_Best);
							Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						}
						else if(Land == 1)
						{
							Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "7", "77", Lager_Best);
							Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						}
						else if(Land == 6)
						{
							Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "60", "580", Lager_Best);
							Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						}
						else if(Land == 7)
						{
							Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "102", "103", Lager_Best);
							Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						}
						else if(Land == 4)
						{
							Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "6", "66", Lager_Best);
							Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						}
						else if(Land == 3)
						{
							Ch = Requete03_PPS_P(Artikelnummer, Lager, Lager_Bestand1, Lager_Bestand2, "26", "260", Lager_Best);
							Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand2 + " and A.Artikelnummer='" + Artikelnummer + "'";
						}
						else
						{
							Ch = Requete03_CZ(Artikelnummer, Lager, Lager_Bestand, Lager_Best);
							Ch3 = "select sum(Lager.Bestand) as Bestand from Lager  inner join Artikel A on A.[Artikel-Nr]=Lager.[Artikel-Nr] where " + Lager_Bestand + " and A.Artikelnummer='" + Artikelnummer + "'";
						}


						if(Land == 1 | Land == 2 | Land == 6 | Land == 7 /*| Land == 3*/)
							Ch1 = Requete04(Artikel_Nr, Lager_Best);
						else if(Land == 3 |/*R*/ Land == 4 | Land == 5)
							Ch1 = Requete05(Artikel_Nr, Lager_Best);
						#endregion <<<<<< Bloc 1

						//Infrastructure.Services.Logging.Logger.LogTrace("12>>" + Ch);
						var rst = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST(Ch);

						#region Bloc 2
						string rst1_query = "select isnull(Bestellnummern.[Lieferanten-Nr],0) as Lief_Nr,Bestellnummern.[Artikel-Nr],isnull(adressen.Name1,'') as N1,Bestellnummern.Standardlieferant as St1,isnull(Bestellnummern.Wiederbeschaffungszeitraum,0) as BW,isnull(Bestellnummern.Mindestbestellmenge,0) as M1, isnull(Bestellnummern.[Einkaufspreis],0) as P1,isnull(Bestellnummern.[Bestell-Nr],'') as B1,isnull(adressen.Telefon,'') as T1 ,isnull(adressen.Fax,'') as F1 from Bestellnummern LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr where [Artikel-Nr]=" + Artikel_Nr + " order by [Name1] ";
						//Infrastructure.Services.Logging.Logger.LogTrace("13>>" + rst1_query);
						var rst1 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST1(rst1_query);

						double SummBedarf;
						double VerfugbarIni;
						double Verfugbar;
						SummBedarf = 0;

						// - Empty temp table
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery
							($"IF OBJECT_ID('{dispoBedarfTableName}', N'U') IS NULL BEGIN "
							+ $@" CREATE TABLE {dispoBedarfTableName} ([Fertigung] nvarchar(50),[ArtikelNummer] nvarchar(50),[Termin_Bestatigen] datetime,[Bezeichnung] nvarchar(250),[FA_Offen]  decimal(20, 8) NOT NULL DEFAULT 0,[Anzahl] decimal(20, 8) NOT NULL DEFAULT 0
                    ,[Bedarf_FA] decimal(20, 8) NOT NULL DEFAULT 0,[Termin_MA] datetime,[Verfügbar] decimal(20, 8) NOT NULL DEFAULT 0,[Bedarf_Summiert] decimal(20, 8) NOT NULL DEFAULT 0,[S-Extetrn] nvarchar(50),
                    [S_Intern] nvarchar(50),[Kommisioniert_komplett] bit,[Kommisioniert_teilweise] bit, [Kabel_geschnitten] bit,[Verfug_Ini]  decimal(20, 8) NOT NULL DEFAULT 0,
                    [Stücklisten_Artikelnummer] nvarchar(50),[Bezeichnung_des_Bauteils] nvarchar(250),[Gestart] bit,[Reserviert_Menge] decimal(20, 8) NOT NULL DEFAULT 0) END"
							+ $" TRUNCATE TABLE {dispoBedarfTableName};");


						//  - 2022-06-08 - virtual negative bestand for AL & art.
						if(Land == 3 && aLVirtualBestandArticleIds?.Exists(x => x == articleEntity.ArtikelNr) == true)
						{
							var bestandEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetStandardByArticleAndId(articleEntity.ArtikelNr, 26);
							if(bestandEntity != null && bestandEntity.Bestand < 0)
							{
								if(rst != null && rst.Count > 0)
								{
									var p = double.TryParse(bestandEntity.Bestand?.ToString(), out var b) ? b : 0d;
									rst.Insert(0, new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST
									{
										Anzahl_F = p,
										Artikel_Bau = "Virtual",
										Artikel_H = "Virtual",
										Artikel_Nr = -1,
										Artikel_Nr_H = -1,
										Bezeichnung_1 = "Virtual",
										Bezeichnung_D = "Virtual",
										Bezeichnung_H = "Virtual",
										Bruttobedarf = -1d * p,
										Fertigungsnummer = -1,
										Freigabestatus = "Virtual",
										FreigabestatusTN_Int = "Virtual",
										Gestart = false,
										Kabel_geschnitten = false,
										Kommisioniert_komplett = false,
										Kommisioniert_teilweise = false,
										Lagerort_id = bestandEntity.Lagerort_id,
										Reserviert_Menge = 0,
										St_Anzahl = 0,
										Termin_Bestätigt1 = new DateTime(2000, 1, 1),
										Termin_Fertigstellung = new DateTime(2000, 1, 1),
										Termin_Materialbedarf = new DateTime(2000, 1, 1),
										Verfug_Ini = 0
									});
								}
							}
						}

						if(rst != null && rst.Count > 0)
						{
							VerfugbarIni = rst[0].Verfug_Ini ?? 0;

							if((rst[0].Bezeichnung_D?.Trim()?.ToLower() == "zugang" || rst[0].Bezeichnung_D?.Trim()?.ToLower() == PLANNED_ENTRY)
								&& !rst.Exists(x => x.Bezeichnung_D?.Trim()?.ToLower() != "zugang") && !rst.Exists(x => x.Bezeichnung_D?.Trim()?.ToLower() != PLANNED_ENTRY))
							{
								//Infrastructure.Services.Logging.Logger.LogTrace("15>>" + Ch3);
								rst3 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST3(Ch3);
								VerfugbarIni = (rst3[0].Bestand ?? 0);
							}
						}
						else
						{
							//Infrastructure.Services.Logging.Logger.LogTrace("16>>" + Ch3);
							rst3 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST3(Ch3);
							VerfugbarIni = (rst3[0].Bestand ?? 0);
							Ch = $"insert into {dispoBedarfTableName} ([Stücklisten_Artikelnummer],[Verfug_Ini]) values('" + escapeSpecialCars(Artikelnummer) + "'," + VerfugbarIni.ToString().Replace(",", ".") + ")";
							//Infrastructure.Services.Logging.Logger.LogTrace("13>>" + Ch);
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery(Ch);
						}

						var BestellungAnzahl = 0d;
						for(int i = 0; i < rst.Count; i++)
						{
							var rstItem = rst[i];
							if((VerfugbarIni < rstItem.Verfug_Ini))
							{
								VerfugbarIni = rstItem.Verfug_Ini ?? 0;
								Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"UPDATE {dispoBedarfTableName} SET Verfug_Ini =" + magicReplace(VerfugbarIni, ",", "."));
								Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"UPDATE {dispoBedarfTableName} SET Verfügbar =Verfügbar+" + magicReplace(VerfugbarIni, ",", "."));
							}
							int K_Komplett;
							if(rstItem.Kommisioniert_komplett == true)
								K_Komplett = -1;
							else
								K_Komplett = 0;

							int K_teilweisen;
							if(rstItem.Kommisioniert_teilweise == true)
								K_teilweisen = -1;
							else
								K_teilweisen = 0;

							int Gechnitt;
							if(rstItem.Kabel_geschnitten == true)
								Gechnitt = -1;
							else
								Gechnitt = 0;

							// Declarer Gestart ou non																			
							int Gestart;
							if(Land == 2 | Land == 6 | Land == 7 | Land == 4 | Land == 1 | Land == 3)
							{
								if(rstItem.Gestart == true)
									Gestart = -1;
								else
									Gestart = 0;
							}
							else
							{
								Gestart = 0;
							}

							var valeur = rstItem.Bruttobedarf * 1;
							if(rstItem.Bezeichnung_D?.Trim()?.ToLower() != "zugang" && rstItem.Bezeichnung_D?.Trim()?.ToLower() != PLANNED_ENTRY)
							{
								if(Land != 2 & Land != 6 & Land != 7 & Land != 4 & Land != 1 & Land != 3)
									SummBedarf = SummBedarf + (valeur ?? 0);
								else if(Gestart == 0)
									SummBedarf = SummBedarf + (valeur ?? 0);
								else
									SummBedarf = 0;
							}
							//var BestellungAnzahl = 0d; // - 2022-03-09 // wrong Verfugar after Zugang!!! move init f´before for-loop
							if(rstItem.Bezeichnung_D?.Trim()?.ToLower() == "zugang" || rstItem.Bezeichnung_D?.Trim()?.ToLower() == PLANNED_ENTRY)
							{
								BestellungAnzahl = BestellungAnzahl + (rstItem.Anzahl_F ?? 0);
								Verfugbar = VerfugbarIni - SummBedarf + BestellungAnzahl;
							}
							else
								Verfugbar = VerfugbarIni - SummBedarf + BestellungAnzahl;


							if(Land == 2 | Land == 3 | Land == 6 | Land == 7 | Land == 1 | Land == 4)
							{
								if(rstItem.Bezeichnung_D?.Trim()?.ToLower() == "zugang" || rstItem.Bezeichnung_D?.Trim()?.ToLower() == PLANNED_ENTRY)
								{
									Ch = $"insert into {dispoBedarfTableName} ([Fertigung],[ArtikelNummer],[Termin_Bestatigen],[Bezeichnung],[FA_Offen],[Anzahl],[Bedarf_FA],[Termin_MA],[Verfügbar],[Bedarf_Summiert],[S-Extetrn],[S_Intern],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kabel_geschnitten],[Verfug_Ini],[Stücklisten_Artikelnummer],[Bezeichnung_des_Bauteils],[Gestart],[Reserviert_Menge])";
									Ch += " values(NULL,'PO:" + escapeSpecialCars(rstItem.Artikel_H) + "',NULL,'" + escapeSpecialCars(rstItem.Bezeichnung_D) + "'," + (rstItem.Anzahl_F.HasValue ? rstItem.Anzahl_F.ToString().Replace(",", ".") : "0") + ",0,0,";
									Ch += "" + (!rstItem.Termin_Materialbedarf.HasValue ? "NULL" : $"'{rstItem.Termin_Materialbedarf?.ToString("yyyyMMdd")}'") + "," + magicReplace(Math.Round(Verfugbar, 1), ",", ".") + ",0,'" + escapeSpecialCars(rstItem.Freigabestatus) + "','" + escapeSpecialCars(rstItem.FreigabestatusTN_Int) + "'," + K_Komplett + "," + K_teilweisen + "," + Gechnitt + "," + magicReplace(VerfugbarIni, ",", ".");
									Ch += ",'" + escapeSpecialCars(Artikelnummer) + "','" + escapeSpecialCars(rstItem.Bezeichnung_H) + "'," + Gestart + "," + magicReplace(rstItem.Reserviert_Menge, ",", ".") + ")";
								}
								else
								{
									Ch = $"insert into {dispoBedarfTableName} ([Fertigung],[ArtikelNummer],[Termin_Bestatigen],[Bezeichnung],[FA_Offen],[Anzahl],[Bedarf_FA]"
										+ " ,[Termin_MA],[Verfügbar],[Bedarf_Summiert],[S-Extetrn],[S_Intern],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kabel_geschnitten],[Verfug_Ini],[Stücklisten_Artikelnummer],[Bezeichnung_des_Bauteils],[Gestart],[Reserviert_Menge])";
									Ch += " values('" + rstItem.Fertigungsnummer + "','" + escapeSpecialCars(rstItem.Artikel_H) + "'," + (!rstItem.Termin_Bestätigt1.HasValue ? "NULL" : $"'{rstItem.Termin_Bestätigt1?.ToString("yyyyMMdd")}'") + ",'" + escapeSpecialCars(rstItem.Bezeichnung_D) + "'," + magicReplace(rstItem.Anzahl_F, ",", ".") + "," + magicReplace(rstItem.St_Anzahl, ",", ".") + "," + magicReplace(rstItem.Bruttobedarf, ",", ".") + ",";
									Ch += "" + (!rstItem.Termin_Materialbedarf.HasValue ? "NULL" : $"'{rstItem.Termin_Materialbedarf?.ToString("yyyyMMdd")}'") + "," + magicReplace(Math.Round(Verfugbar, 1), ",", ".") + "," + magicReplace(Math.Round(SummBedarf, 1), ",", ".") + ",'" + escapeSpecialCars(rstItem.Freigabestatus) + "','" + escapeSpecialCars(rstItem.FreigabestatusTN_Int) + "'," + K_Komplett + "," + K_teilweisen + "," + Gechnitt + "," + magicReplace(VerfugbarIni, ",", ".");
									Ch += ",'" + escapeSpecialCars(rstItem.Artikel_Bau) + "','" + escapeSpecialCars(rstItem.Bezeichnung_H) + "'," + Gestart + "," + magicReplace(rstItem.Reserviert_Menge, ",", ".") + ")";
								}
							}
							else if(rstItem.Bezeichnung_D?.Trim()?.ToLower() == "zugang" || rstItem.Bezeichnung_D?.Trim()?.ToLower() == PLANNED_ENTRY)
							{
								Ch = $"insert into {dispoBedarfTableName} ([Fertigung],[ArtikelNummer],[Termin_Bestatigen],[Bezeichnung],[FA_Offen],[Anzahl],[Bedarf_FA]"
									+ " ,[Termin_MA],[Verfügbar],[Bedarf_Summiert],[S-Extetrn],[S_Intern],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kabel_geschnitten],[Verfug_Ini],[Stücklisten_Artikelnummer],[Bezeichnung_des_Bauteils])";
								Ch += " values(NULL,'PO:" + escapeSpecialCars(rstItem.Artikel_H) + "'," + (!rstItem.Termin_Bestätigt1.HasValue ? "NULL" : $"'{rstItem.Termin_Bestätigt1?.ToString("yyyyMMdd")}'") + ",'" + escapeSpecialCars(rstItem.Bezeichnung_D) + "'," + magicReplace(rstItem.Anzahl_F, ",", ".") + ",0,0,";
								Ch += "" + (!rstItem.Termin_Materialbedarf.HasValue ? "NULL" : $"'{rstItem.Termin_Materialbedarf?.ToString("yyyyMMdd")}'") + "," + magicReplace(Math.Round(Verfugbar, 1), ",", ".") + ",0,'" + escapeSpecialCars(rstItem.Freigabestatus) + "',''," + K_Komplett + "," + K_teilweisen + "," + Gechnitt + "," + magicReplace(VerfugbarIni, ",", ".");
								Ch += ",'" + escapeSpecialCars(Artikelnummer) + "','" + escapeSpecialCars(rstItem.Bezeichnung_H) + "')";
							}
							else
							{
								Ch = $"insert into {dispoBedarfTableName} ([Fertigung],[ArtikelNummer],[Termin_Bestatigen],[Bezeichnung],[FA_Offen],[Anzahl],[Bedarf_FA]"
									+ " ,[Termin_MA],[Verfügbar],[Bedarf_Summiert],[S-Extetrn],[S_Intern],[Kommisioniert_komplett],[Kommisioniert_teilweise],[Kabel_geschnitten],[Verfug_Ini],[Stücklisten_Artikelnummer],[Bezeichnung_des_Bauteils])";
								Ch += " values(" + rstItem.Fertigungsnummer + ",'" + escapeSpecialCars(rstItem.Artikel_H) + "'," + (!rstItem.Termin_Bestätigt1.HasValue ? "NULL" : $"'{rstItem.Termin_Bestätigt1?.ToString("yyyyMMdd")}'") + ",'" + escapeSpecialCars(rstItem.Bezeichnung_D) + "'," + magicReplace(rstItem.Anzahl_F, ",", ".") + "," + magicReplace(rstItem.St_Anzahl, ",", ".") + "," + magicReplace(rstItem.Bruttobedarf, ",", ".") + ",";
								Ch += "" + (!rstItem.Termin_Materialbedarf.HasValue ? "NULL" : $"'{rstItem.Termin_Materialbedarf?.ToString("yyyyMMdd")}'") + "," + magicReplace(Math.Round(Verfugbar, 1), ",", ".") + "," + magicReplace(Math.Round(SummBedarf, 1), ",", ".") + ",'" + escapeSpecialCars(rstItem.Freigabestatus) + "','" + escapeSpecialCars(rstItem.FreigabestatusTN_Int) + "'," + K_Komplett + "," + K_teilweisen + "," + Gechnitt + "," + magicReplace(VerfugbarIni, ",", ".");
								Ch += ",'" + escapeSpecialCars(rstItem.Artikel_Bau) + "','" + escapeSpecialCars(rstItem.Bezeichnung_H) + "')";
							}
							// - 
							try
							{
								Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery(Ch);
							} catch(Exception e)
							{
								//Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Debug, Ch);
								//Infrastructure.Services.Logging.Logger.Log(e);
								throw;
							}
						}

						//FIXME:
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery
							($"IF OBJECT_ID('{dispoLieferTableName}', N'U') IS NULL BEGIN "
							+ $" CREATE TABLE {dispoLieferTableName}([Lieferant] nvarchar(250),[Standar_Liferent] bit,[Bestell-Nr] nvarchar(250),[Peis] decimal(20, 8) NOT NULL DEFAULT 0,[LT] int,[MQO] decimal(20,8) NOT NULL DEFAULT 0,[Telefon] nvarchar(100),[Fax]nvarchar(200),[Artikel-Nr] int,[Lief_Nr] int) END"
							+ $" TRUNCATE TABLE {dispoLieferTableName};");

						for(int i = 0; i < rst1.Count; i++)
						{
							var rstItem = rst1[i];
							var st = "";
							if(rstItem.St1 == true)
								st = "1";
							else
								st = "0";
							Ch = $"insert into {dispoLieferTableName} ([Lieferant],[Standar_Liferent],[Bestell-Nr],[Peis],[LT],[MQO],[Telefon],[Fax],[Artikel-Nr],[Lief_Nr])"
								+ " values('" + escapeSpecialCars(rstItem.N1) + "','" + st + "','" + escapeSpecialCars(rstItem.B1) + "'," + magicReplace(rstItem.P1, ",", ".") + "," + rstItem.BW + ","
								+ magicReplace(rstItem.M1, ",", ".") + ",'" + escapeSpecialCars(rstItem.T1) + "','" + escapeSpecialCars(rstItem.F1) + "'," + rstItem.Artikel_Nr + "," + rstItem.Lief_Nr + ")";
							//Infrastructure.Services.Logging.Logger.LogTrace("2>>" + Ch);
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery(Ch);
						}


						// FIXME:
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery
							($"IF OBJECT_ID('{dispoBestellTableName}', N'U') IS NULL BEGIN "
							+ $" CREATE TABLE {dispoBestellTableName} ([PO] int,[Anzhal] decimal(20,8) NOT NULL DEFAULT 0,[Liefertermin] datetime,[ABtermin] datetime,[VornameFirma] nvarchar(250),[Lief_Nr] int,[Bemerkung] nvarchar(2000),[AB] nvarchar(250)) END"
							+ $" TRUNCATE TABLE {dispoBestellTableName};");

						// ---------------- LOOP
						//Infrastructure.Services.Logging.Logger.LogTrace("3>>" + Ch1);
						var rst2 = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_RST2(Ch1);
						for(int i = 0; i < rst2.Count; i++)
						{
							var rst2Item = rst2[i];

							string DLiefer;
							if(rst2Item.Liefertermin.HasValue)
								DLiefer = $"'{rst2Item.Liefertermin.Value.ToString("yyyyMMdd")}'";
							else
								DLiefer = "NULL";

							string D_AB;
							if(rst2Item.Bestätigter_Termin.HasValue)
								D_AB = $"'{rst2Item.Bestätigter_Termin.Value.ToString("yyyyMMdd")}'";
							else
								D_AB = "NULL";
							double Anz;
							if(rst2Item.Anzahl1.HasValue)
								Anz = rst2Item.Anzahl1.Value;
							else
								Anz = 0;

							var Proj = rst2Item.Projekt_Nr;
							Ch = $"insert into {dispoBestellTableName} ([PO],[Anzhal],[Liefertermin],[ABtermin],[VornameFirma],[Lief_Nr],[Bemerkung],[AB])";
							Ch += " values(" + rst2Item.Projekt_Nr + "," + magicReplace(Anz, ",", ".") + "," + DLiefer + "," + D_AB + ",'" + escapeSpecialCars(rst2Item.VornameFirma) + "'," + rst2Item.Lief_Nr + ",'" + escapeSpecialCars(rst2Item.Bemerk) + "','" + escapeSpecialCars(rst2Item.AB_L) + "')";

							System.Console.WriteLine("4>>" + Ch); //Infrastructure.Services.Logging.Logger.LogTrace("4>>" + Ch);
																  //FIXME:
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery(Ch);
						}

						#endregion Bloc 2

						// - get report data
						var results = new Tuple<
							List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>,
							List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>,
							List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Liferant>,
							List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bestellung>,
							string>(
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_DispoBedarf($"SELECT * FROM {dispoBedarfTableName} WHERE (Gestart=1 Or Gestart=-1) AND Anzahl >= 0 ORDER BY [Termin_MA], [Termin_Bestatigen], [Bedarf_Summiert] ASC"),
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_DispoBedarf($"SELECT  * FROM {dispoBedarfTableName} WHERE (Gestart=0 Or Gestart IS NULL) AND Anzahl >= 0 ORDER BY [Termin_MA], [Termin_Bestatigen], [Bedarf_Summiert] ASC"),
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_DispoLiferant($"SELECT * FROM {dispoLieferTableName} ORDER BY [Standar_Liferent] DESC, [Lieferant] ASC"),
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_DispoBestellung($"SELECT *, 0 AS ProjectPurchase FROM {dispoBestellTableName} ORDER BY [Liefertermin], [ABtermin], [Lief_Nr] ASC"),
							$"BRUTTO-Bedarfsliste für Fertigung: {fertigungNumber} = {fertigungLager}"
							);

						// - Drop user [temp] tables
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoBedarfTableName}");
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoLieferTableName}");
						Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoBestellTableName}");

						// -
						return results;
					} catch(Exception)
					{
						try
						{
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoBedarfTableName}");
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoLieferTableName}");
							Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetBedarf_ExecuteNonQuery($"DROP TABLE {dispoBestellTableName}");
						} catch { }
						throw;
					}
				}
				internal static string Requete03_PPS_P(string ArtikelNr, string Lager, string Lager_Bestand1, string Lager_Bestand2, string Lager_Haupt, string Lager_Pr, string LagerBestellung)
				{
					string query = "";
					string PLANNED_ENTRY = "Eingeplante Zugang";
					// -
					#region >>>>>> Content
					query = $@"select Fertigung.Fertigungsnummer, Fertigung.Artikel_Nr, Artikel.Artikelnummer as Artikel_H, Artikel.[Bezeichnung 1] as Bezeichnung_D, Fertigung.Anzahl as Anzahl_F, Fertigung.Termin_Fertigstellung, Fertigung_Positionen.[Artikel_Nr], Artikel_1.Artikelnummer as Artikel_Bau, Artikel_1.[Bezeichnung 1], Fertigung_Positionen.Anzahl/fertigung.Originalanzahl as St_Anzahl, Fertigung_Positionen.Anzahl/fertigung.Originalanzahl*fertigung.Anzahl AS Bruttobedarf, IIf([Artikel_1].[ID_Klassifizierung]>10 
					 Or [Artikel_1].[ID_Klassifizierung] Is Null and Fertigung.erstmuster=0  ,[Fertigung].[Termin_Bestätigt1]-28,[Fertigung].[Termin_Bestätigt1]-28) AS Termin_Materialbedarf, Fertigung.Lagerort_id, Fertigung.Termin_Bestätigt1, Artikel_1.[Bezeichnung 1]as Bezeichnung_H, Fertigung.Kommisioniert_teilweise, Fertigung.Kommisioniert_komplett, Fertigung.Kabel_geschnitten, Artikel.Freigabestatus as Freigabestatus , Artikel.[Freigabestatus TN intern]as FreigabestatusTN_Int, CAST(isNull(T1.SummevonBestand,0) as decimal(20,8))  as Verfug_Ini, CAST(isnull(T1.Bestand_reserviert_F,0) as decimal(20,8)) as Reserviert_Menge,isnull(Fertigung.FA_Gestartet,0) as Gestart
					 FROM Fertigung
					 inner join Artikel on artikel.[artikel-nr]=fertigung.[Artikel_Nr]
					 inner join (Fertigung_Positionen inner join (Artikel AS Artikel_1 LEFT JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung)  on artikel_1.[artikel-nr]=Fertigung_Positionen.[Artikel_Nr]) on Fertigung_Positionen.ID_Fertigung=fertigung.ID
					 left join(
					 SELECT Lager.[Artikel-Nr] as Artikel_Nr , Sum(Lager.Bestand) AS SummevonBestand,coalesce(L1.Bestand_reserviert,0) as Bestand_reserviert_F
					 FROM Lager inner join Artikel on Artikel.[Artikel-Nr]=lager.[Artikel-Nr]
					 left join Lager L1 on L1.Lagerort_id={Lager_Pr} and L1.[Artikel-Nr]=Lager.[Artikel-Nr]
					 where ({Lager_Bestand2})
					 and artikel.Warentyp=2
					 GROUP BY Lager.[Artikel-Nr],L1.Bestand_reserviert
					 Union all
					 SELECT Lager.[Artikel-Nr] as Artikel_Nr , Sum(Lager.Bestand)-isnull(T_Reserviert.Menge_Reserviert,0) AS SummevonBestand,isnull(T_Reserviert.Menge_Reserviert,0) as Menge_Reserviert_F
					 FROM Lager inner join Artikel on Artikel.[Artikel-Nr]=lager.[Artikel-Nr]
					 Left Join
					 (SELECT tbl_Planung_gestartet.Artikel_Nr AS Artikel_Nr, SUM(Menge_Reserviert) AS Menge_Reserviert,Artikel.Warentyp AS type ,Lagerort_ID
					 FROM tbl_Planung_gestartet
					 INNER JOIN Artikel ON Artikel.[Artikel-Nr]=tbl_Planung_gestartet.Artikel_Nr
					 where tbl_Planung_gestartet.Lagerort_id IN ({Lager_Haupt})
					 AND (Artikel.Warentyp<>2 OR Artikel.Warentyp IS NULL)
					 GROUP BY Artikel_Nr, Artikel.Artikelnummer,Artikel.Warentyp,Lagerort_ID)
					 as T_Reserviert on T_Reserviert.Artikel_Nr=Lager.[Artikel-Nr]
					 where ({Lager_Bestand1})
					 and (artikel.Warentyp<>2 or artikel.Warentyp is null)
					GROUP BY Lager.[Artikel-Nr],T_Reserviert.Menge_Reserviert,isnull(T_Reserviert.Menge_Reserviert,0))
					 as T1 on  Fertigung_Positionen.[Artikel_Nr]=T1.Artikel_Nr
					 where
					(((Artikel_1.Artikelnummer)='{ArtikelNr}') AND ({Lager}) AND
					 ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') )
					 Union all
					 SELECT  '','',[Bestellungen].[Projekt-Nr],IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,'Zugang','{PLANNED_ENTRY}'),sum([bestellte Artikel].Anzahl) as Anzahl1 ,IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin),'','','','','',IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin),'',null,'','','','',IIF([bestellte Artikel].[Bestätigter_Termin] is not null,'','L T'),'',0,0,0
					 FROM [bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr
					 inner join Artikel on Artikel.[Artikel-Nr]=[bestellte Artikel].[Artikel-Nr]
					 WHERE (((Artikelnummer)='{ArtikelNr}') AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1)
					 AND ( {LagerBestellung}  AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum>GETDATE()-730)))
					 group by IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,'Zugang','{PLANNED_ENTRY}'), IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin),[Bestellungen].[Projekt-Nr],IIF([bestellte Artikel].[Bestätigter_Termin] is not null,'','L T')
					 order by Gestart desc,Termin_Materialbedarf,Fertigung.Fertigungsnummer,Artikel_H";
					#endregion <<<<< Content XXXX
					// - 
					return query;
				}
				internal static string Requete03_CZ(string ArtikelNr, string Lager, string Lager_Bestand, string LagerBestellung)
				{
					string query = "";
					string PLANNED_ENTRY = "Eingeplante Zugang";
					// -
					#region >>>>>> Content
					query += "SELECT Fertigung.Fertigungsnummer, Fertigung.Artikel_Nr, Artikel.Artikelnummer as Artikel_H, Artikel.[Bezeichnung 1] as Bezeichnung_D, Fertigung.Anzahl as Anzahl_F, Fertigung.Termin_Fertigstellung, Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer as Artikel_Bau, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl as St_Anzahl, Fertigung.Anzahl*Stücklisten.Anzahl AS Bruttobedarf, IIf([Artikel_1].[ID_Klassifizierung]>10 Or [Artikel_1].[ID_Klassifizierung] Is Null and Fertigung.erstmuster=0  ,[Fertigung].[Termin_Bestätigt1]-28,[Fertigung].[Termin_Bestätigt1]-28) AS Termin_Materialbedarf, Fertigung.Lagerort_id, Fertigung.Termin_Bestätigt1, Artikel_1.[Bezeichnung 1]as Bezeichnung_H, Fertigung.Kommisioniert_teilweise, Fertigung.Kommisioniert_komplett, Fertigung.Kabel_geschnitten, Artikel.Freigabestatus as Freigabestatus , Artikel.[Freigabestatus TN intern]as FreigabestatusTN_Int,CAST(T1.SummevonBestand as decimal(20,8)) as Verfug_Ini, CAST(0 as decimal(20,8)) as Reserviert_Menge, isnull(Fertigung.FA_Gestartet,0) as Gestart";
					query += " FROM (((Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) LEFT JOIN Stücklisten ON Fertigung.Artikel_Nr = Stücklisten.[Artikel-Nr]) LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.Artikelnummer = Artikel_1.Artikelnummer) LEFT JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung";
					query += " left join( ";
					query += " SELECT Lager.[Artikel-Nr] as Artikel_Nr , Sum(Lager.Bestand) AS SummevonBestand ";
					query += " FROM Lager ";
					query += " WHERE (" + Lager_Bestand + ")";
					query += " GROUP BY Lager.[Artikel-Nr])as T1 on  Stücklisten.[Artikel-Nr des Bauteils]=T1.Artikel_Nr";
					query += " WHERE (((Stücklisten.Artikelnummer)='" + ArtikelNr + "') AND (" + Lager + ") AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') AND ((Stücklisten.Variante)='0'))";
					query += " Union all";
					query += $" SELECT  '','',[Bestellungen].[Projekt-Nr],IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,'Zugang','{PLANNED_ENTRY}'),sum([bestellte Artikel].Anzahl) as Anzahl1 ,IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin) as [Bestätigter_Termin],'','','','','',IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin) as[Bestätigter_Termin],'',null,'','','','',IIF([bestellte Artikel].[Bestätigter_Termin] is not null,'','L T'),'',0,0,0";
					query += " FROM [bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr";
					query += " inner join Artikel on Artikel.[Artikel-Nr]=[bestellte Artikel].[Artikel-Nr]";
					query += " WHERE (((Artikelnummer)='" + ArtikelNr + "') AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1)";
					query += " AND ( " + LagerBestellung + "  AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum>GETDATE()-730)))";
					query += $" group by IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,'Zugang','{PLANNED_ENTRY}'), IIF([bestellte Artikel].[Bestätigter_Termin] is not null AND YEAR([bestellte Artikel].[Bestätigter_Termin])<>2999,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].Liefertermin),[Bestellungen].[Projekt-Nr],IIF([bestellte Artikel].[Bestätigter_Termin] is not null,'','L T')";
					query += " order by Termin_Materialbedarf,Fertigung.Fertigungsnummer,Artikel_H ";

					#endregion <<<<< Content XXXX
					// - 
					return query;
				}
				internal static string Requete04(long Artikel_Nr, string Lager_Best)
				{
					string query = "";
					// -
					#region >>>>>> Content
					query += "SELECT  Bestellungen.[Lieferanten-Nr] as Lief_Nr,Bestellungen.[Vorname/NameFirma] as VornameFirma,[bestellte Artikel].Anzahl as Anzahl1 , Bestellungen.[Projekt-Nr],[bestellte Artikel].Liefertermin, [bestellte Artikel].Bestätigter_Termin,isnull([bestellte Artikel].[Bemerkung_Pos],'') as Bemerk,isnull([bestellte Artikel].[AB-Nr_Lieferant],'') as AB_L";
					query += " FROM [bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr";
					query += " WHERE ((([bestellte Artikel].[Artikel-Nr]) = " + Artikel_Nr + ") And (([bestellte Artikel].erledigt_pos) = 0) And ((Bestellungen.erledigt) = 0) And ((Bestellungen.gebucht) = 1) And (" + Lager_Best + ")";
					query += " ORDER BY [bestellte Artikel].[Artikel-Nr],[bestellte Artikel].Liefertermin;";
					#endregion <<<<< Content XXXX
					// - 
					return query;
				}
				internal static string Requete05(long Artikel_Nr, string Lager_Best)
				{
					string query = "";
					// -
					#region >>>>>> Content
					query += "SELECT  Bestellungen.[Lieferanten-Nr] as Lief_Nr,Bestellungen.[Vorname/NameFirma] as VornameFirma,[bestellte Artikel].Anzahl as Anzahl1 , Bestellungen.[Projekt-Nr],[bestellte Artikel].Liefertermin, [bestellte Artikel].Bestätigter_Termin,isnull([bestellte Artikel].[Bemerkung_Pos],'') as Bemerk,isnull([bestellte Artikel].[AB-Nr_Lieferant],'') as AB_L";
					query += " FROM [bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr";
					query += " WHERE ((([bestellte Artikel].[Artikel-Nr])=" + Artikel_Nr + ") AND (([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) ";
					query += " AND ( " + Lager_Best + "  AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum>GETDATE()-730)))";
					query += " ORDER BY [bestellte Artikel].[Artikel-Nr]";
					#endregion <<<<< Content XXXX
					// - 
					return query;
				}
				public static string magicReplace(double? val, string c, string r)
				{
					return val.HasValue
							? val.ToString().Replace(c?.Trim(), r?.Trim())
							: "0";
				}
				static string escapeSpecialCars(string value)
				{
					value = value ?? "";
					return value.Replace("'", "''");
				}
			}
			public static int GetBedarf_ExecuteNonQuery(string queryParamed)
			{
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand(queryParamed, sqlConnection);
					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST> GetBedarf_RST(string queryParamed)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand(queryParamed, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST1> GetBedarf_RST1(string queryParamed)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand(queryParamed, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST1(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST1>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST2> GetBedarf_RST2(string queryParamed)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand(queryParamed, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST2(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST2>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST3> GetBedarf_RST3(string queryParamed)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand(queryParamed, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST3(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.RST3>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bestellung> GetBedarf_DispoBestellung(string query)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bestellung(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bestellung>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf> GetBedarf_DispoBedarf(string query)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Liferant> GetBedarf_DispoLiferant(string query)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Liferant(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Liferant>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz> GetBomTz_TN(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT 
	                                    Q1.Artikelnummer, Q1.Anzahl, Q1.[Bezeichnung des Bauteils], Q1.Name1, Q1.[Bestell-Nr], Q1.Einkaufspreis, Q1.Kupferzahl, Q1.Mindestbestellmenge, 
	                                    Q1.Wiederbeschaffungszeitraum, Sum(Q1.Bestand) AS [Bestand], Q3.Gesamtbestand
                                    FROM (
	                                    /*Q1*/
	                                    SELECT 
		                                    Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                    Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    Artikel_1.[UL zertifiziert], Lager.Bestand, Artikel_1.Zolltarif_nr, CAST(Artikel_1.Größe AS DECIMAL(38,15)) AS [Gewicht in gr], Artikel_1.Ursprungsland, Lager.Lagerort_id
	                                    FROM ((((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
		                                    LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
		                                    LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    LEFT JOIN Lager ON Artikel_1.[Artikel-Nr] = Lager.[Artikel-Nr]
	                                    GROUP BY Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, 
		                                    Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, 
		                                    Bestellnummern.Wiederbeschaffungszeitraum, Artikel_1.[UL zertifiziert], Lager.Bestand, Artikel_1.Zolltarif_nr, Artikel_1.Größe, 
		                                    Artikel_1.Ursprungsland, Lager.Lagerort_id, Artikel.Artikelnummer, Bestellnummern.Standardlieferant, Stücklisten.Variante
	                                    HAVING (((Lager.Lagerort_id)=7 Or (Lager.Lagerort_id)=4 Or (Lager.Lagerort_id)=29 Or (Lager.Lagerort_id)=30 Or (Lager.Lagerort_id)=41 
		                                    Or (Lager.Lagerort_id)=42 Or (Lager.Lagerort_id)=46 Or (Lager.Lagerort_id)=47
											Or (Lager.Lagerort_id)=60 Or (Lager.Lagerort_id)=58
											Or (Lager.Lagerort_id)=102 Or (Lager.Lagerort_id)=101) AND ((Artikel.Artikelnummer)=@artikelnummer) 
		                                    AND ((Bestellnummern.Standardlieferant)=1) AND ((Stücklisten.Variante)='0'))
	                                    /*ORDER BY Stücklisten.Artikelnummer*/
                                    ) AS Q1
                                    INNER JOIN (
	                                    /*Q3*/
	                                    SELECT 
		                                    Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                    Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    Sum(Lager.Bestand) AS Gesamtbestand
	                                    FROM ((((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
		                                    LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
		                                    LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
		                                    LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    LEFT JOIN Lager ON Artikel_1.[Artikel-Nr] = Lager.[Artikel-Nr]
	                                    GROUP BY Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                    Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    Stücklisten.Position, Artikel_1.[UL zertifiziert], Artikel_1.Zolltarif_nr, Artikel_1.Größe, Artikel_1.Ursprungsland, Artikel.Artikelnummer, 
		                                    Bestellnummern.Standardlieferant, Stücklisten.Variante
	                                    HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Bestellnummern.Standardlieferant)=1) AND ((Stücklisten.Variante)='0'))
	                                    /*ORDER BY Stücklisten.Artikelnummer*/
                                    ) AS Q3 ON Q1.Artikelnummer = Q3.Artikelnummer

                                    GROUP BY Q1.Artikelnummer, Q1.Anzahl, Q1.[Bezeichnung des Bauteils], Q1.Name1, Q1.[Bestell-Nr], Q1.Einkaufspreis, Q1.Kupferzahl, 
	                                    Q1.Mindestbestellmenge, Q1.Wiederbeschaffungszeitraum, Q3.Gesamtbestand, Q1.Position, Q1.[UL zertifiziert], Q1.Zolltarif_nr, 
	                                    Q1.[Gewicht in gr], Q1.Ursprungsland;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz> GetBomTz_AL(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT 
	                                    Q1.ArtikelNrFG, Q1.Artikelnummer, Q1.Anzahl, Q1.[Bezeichnung des Bauteils], Q1.Name1, Q1.[Bestell-Nr], Q1.Einkaufspreis, Q1.Kupferzahl, 
	                                    Q1.Mindestbestellmenge, Q1.Wiederbeschaffungszeitraum, Sum(Q1.Bestand) AS [Bestand], Q3.Gesamtbestand
                                    FROM (
	                                    /*Q1*/
	                                    SELECT 
		                                    Stücklisten.Position, Artikel.Artikelnummer AS ArtikelNrFG, Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], 
		                                    adressen.Name1, Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, 
		                                    Bestellnummern.Wiederbeschaffungszeitraum, Artikel_1.[UL zertifiziert], Lager.Bestand, Artikel_1.Zolltarif_nr, CAST(Artikel_1.Größe AS DECIMAL(38,15)) AS [Gewicht in gr], 
		                                    Artikel_1.Ursprungsland, Lager.Lagerort_id
	                                    FROM ((((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
		                                    LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
		                                    LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
		                                    LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    LEFT JOIN Lager ON Artikel_1.[Artikel-Nr] = Lager.[Artikel-Nr]
	                                    GROUP BY Stücklisten.Position, Artikel.Artikelnummer, Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], 
		                                    adressen.Name1, Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, 
		                                    Bestellnummern.Wiederbeschaffungszeitraum, Artikel_1.[UL zertifiziert], Lager.Bestand, Artikel_1.Zolltarif_nr, Artikel_1.Größe, 
		                                    Artikel_1.Ursprungsland, Lager.Lagerort_id, Bestellnummern.Standardlieferant, Stücklisten.Variante
	                                    HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Lager.Lagerort_id)=24 Or (Lager.Lagerort_id)=26 Or (Lager.Lagerort_id)=34 
		                                    Or (Lager.Lagerort_id)=35) AND ((Bestellnummern.Standardlieferant)=1) AND ((Stücklisten.Variante)='0'))
	                                    /*ORDER BY Stücklisten.Artikelnummer*/
                                    ) AS Q1
                                    INNER JOIN (
	                                    /*Q3*/
	                                    SELECT 
		                                    Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                    Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    Sum(Lager.Bestand) AS Gesamtbestand
	                                    FROM ((((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
		                                    LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
		                                    LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
		                                    LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    LEFT JOIN Lager ON Artikel_1.[Artikel-Nr] = Lager.[Artikel-Nr]
	                                    GROUP BY Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                    Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    Stücklisten.Position, Artikel_1.[UL zertifiziert], Artikel_1.Zolltarif_nr, Artikel_1.Größe, Artikel_1.Ursprungsland, Artikel.Artikelnummer, 
		                                    Bestellnummern.Standardlieferant, Stücklisten.Variante
	                                    HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Bestellnummern.Standardlieferant)=1) AND ((Stücklisten.Variante)='0'))
	                                    /*ORDER BY Stücklisten.Artikelnummer*/
                                    ) AS Q3 ON Q1.Artikelnummer = Q3.Artikelnummer
                                    GROUP BY Q1.ArtikelNrFG, Q1.Artikelnummer, Q1.Anzahl, Q1.[Bezeichnung des Bauteils], Q1.Name1, Q1.[Bestell-Nr], Q1.Einkaufspreis, Q1.Kupferzahl, 
	                                    Q1.Mindestbestellmenge, Q1.Wiederbeschaffungszeitraum, Q3.Gesamtbestand, Q1.Position, Q1.[UL zertifiziert], Q1.Zolltarif_nr, Q1.[Gewicht in gr], 
	                                    Q1.Ursprungsland;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz> GetBomTz_DE(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"/*Q2*/
                                        SELECT 
	                                        Q1.Artikelnummer, Q1.Anzahl, Q1.[Bezeichnung des Bauteils], Q1.Name1, Q1.[Bestell-Nr], Q1.Einkaufspreis, Q1.Kupferzahl, 
	                                        Q1.Mindestbestellmenge, Q1.Wiederbeschaffungszeitraum, Sum(Q1.Bestand) AS [Bestand], Q3.Gesamtbestand
                                        FROM (
	                                        /*Q1*/
	                                        SELECT 
		                                        Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, 
		                                        Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, 
		                                        Bestellnummern.Wiederbeschaffungszeitraum, Artikel_1.[UL zertifiziert], Lager.Bestand, Artikel_1.Zolltarif_nr, 
		                                        CAST(Artikel_1.Größe AS DECIMAL(38,15)) AS [Gewicht in gr], Artikel_1.Ursprungsland, Lager.Lagerort_id
	                                        FROM ((((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
		                                        LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
		                                        LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
		                                        LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                        LEFT JOIN Lager ON Artikel_1.[Artikel-Nr] = Lager.[Artikel-Nr]
	                                        GROUP BY Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, 
		                                        Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, 
		                                        Bestellnummern.Wiederbeschaffungszeitraum, Artikel_1.[UL zertifiziert], Lager.Bestand, Artikel_1.Zolltarif_nr, Artikel_1.Größe, 
		                                        Artikel_1.Ursprungsland, Lager.Lagerort_id, Artikel.Artikelnummer, Bestellnummern.Standardlieferant, Stücklisten.Variante
	                                        HAVING (((Lager.Lagerort_id)=8 Or (Lager.Lagerort_id)=15) AND ((Artikel.Artikelnummer)=@artikelnummer) AND ((Bestellnummern.Standardlieferant)=1) 
		                                        AND ((Stücklisten.Variante)='0'))
	                                        /*ORDER BY Stücklisten.Artikelnummer*/
                                        ) AS Q1 INNER JOIN (
	                                        /*Q3*/
	                                        SELECT 
		                                        Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                        Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                        Sum(Lager.Bestand) AS Gesamtbestand
	                                        FROM ((((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
		                                        LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
		                                        LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
		                                        LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                        LEFT JOIN Lager ON Artikel_1.[Artikel-Nr] = Lager.[Artikel-Nr]
	                                        GROUP BY Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                        Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                        Stücklisten.Position, Artikel_1.[UL zertifiziert], Artikel_1.Zolltarif_nr, Artikel_1.Größe, Artikel_1.Ursprungsland, Artikel.Artikelnummer, 
		                                        Bestellnummern.Standardlieferant, Stücklisten.Variante
	                                        HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Bestellnummern.Standardlieferant)=1) AND ((Stücklisten.Variante)='0'))
                                        ) AS Q3
                                        ON Q1.Artikelnummer = Q3.Artikelnummer
                                        GROUP BY Q1.Artikelnummer, Q1.Anzahl, Q1.[Bezeichnung des Bauteils], Q1.Name1, Q1.[Bestell-Nr], Q1.Einkaufspreis, Q1.Kupferzahl, Q1.Mindestbestellmenge, 
	                                        Q1.Wiederbeschaffungszeitraum, Q3.Gesamtbestand, Q1.Position, Q1.[UL zertifiziert], Q1.Zolltarif_nr, Q1.[Gewicht in gr], Q1.Ursprungsland;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz> GetBomTz_CZ(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT 
	                                    Q1.Artikelnummer, Q1.Anzahl, Q1.[Bezeichnung des Bauteils], Q1.Name1, Q1.[Bestell-Nr], Q1.Einkaufspreis, Q1.Kupferzahl, Q1.Mindestbestellmenge, 
	                                    Q1.Wiederbeschaffungszeitraum, Sum(Q1.Bestand) AS [Bestand], Q3.Gesamtbestand
                                    FROM (
	                                    /*Q1*/
	                                    SELECT 
		                                    Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, 
		                                    Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, 
		                                    Bestellnummern.Wiederbeschaffungszeitraum, Artikel_1.[UL zertifiziert], Lager.Bestand, Artikel_1.Zolltarif_nr, CAST(Artikel_1.Größe AS DECIMAL(38,15)) AS [Gewicht in gr], 
		                                    Artikel_1.Ursprungsland, Lager.Lagerort_id
	                                    FROM ((((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
		                                    LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
		                                    LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
		                                    LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    LEFT JOIN Lager ON Artikel_1.[Artikel-Nr] = Lager.[Artikel-Nr]
	                                    GROUP BY Stücklisten.Position, Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1,	
		                                    Bestellnummern.[Bestell-Nr], Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, 
		                                    Bestellnummern.Wiederbeschaffungszeitraum, Artikel_1.[UL zertifiziert], Lager.Bestand, Artikel_1.Zolltarif_nr, Artikel_1.Größe, 
		                                    Artikel_1.Ursprungsland, Lager.Lagerort_id, Artikel.Artikelnummer, Bestellnummern.Standardlieferant, Stücklisten.Variante
	                                    HAVING (((Lager.Lagerort_id)=3 Or (Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=9 Or (Lager.Lagerort_id)=20 Or (Lager.Lagerort_id)=21 
		                                    Or (Lager.Lagerort_id)=23) AND ((Artikel.Artikelnummer)=@artikelnummer) AND ((Bestellnummern.Standardlieferant)=1) AND ((Stücklisten.Variante)='0'))
	                                    /*ORDER BY Stücklisten.Artikelnummer*/
                                    ) AS Q1 INNER JOIN 
                                    (
                                    /*Q3*/
	                                    SELECT 
		                                    Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                    Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    Sum(Lager.Bestand) AS Gesamtbestand
	                                    FROM ((((Artikel LEFT JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
		                                    LEFT JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
		                                    LEFT JOIN Bestellnummern ON Artikel_1.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
		                                    LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr) 
		                                    LEFT JOIN Lager ON Artikel_1.[Artikel-Nr] = Lager.[Artikel-Nr]
	                                    GROUP BY Stücklisten.Artikelnummer, Stücklisten.Anzahl, Stücklisten.[Bezeichnung des Bauteils], adressen.Name1, Bestellnummern.[Bestell-Nr], 
		                                    Bestellnummern.Einkaufspreis, Artikel_1.Kupferzahl, Bestellnummern.Mindestbestellmenge, Bestellnummern.Wiederbeschaffungszeitraum, 
		                                    Stücklisten.Position, Artikel_1.[UL zertifiziert], Artikel_1.Zolltarif_nr, Artikel_1.Größe, Artikel_1.Ursprungsland, Artikel.Artikelnummer, 
		                                    Bestellnummern.Standardlieferant, Stücklisten.Variante
	                                    HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Bestellnummern.Standardlieferant)=1) AND ((Stücklisten.Variante)='0'))
	                                    ) AS Q3
                                    ON Q1.Artikelnummer = Q3.Artikelnummer
                                    GROUP BY Q1.Artikelnummer, Q1.Anzahl, Q1.[Bezeichnung des Bauteils], Q1.Name1, Q1.[Bestell-Nr], Q1.Einkaufspreis, Q1.Kupferzahl, 
	                                    Q1.Mindestbestellmenge, Q1.Wiederbeschaffungszeitraum, Q3.Gesamtbestand, Q1.Position, Q1.[UL zertifiziert], Q1.Zolltarif_nr, 
	                                    Q1.[Gewicht in gr], Q1.Ursprungsland;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> GetProductivity_CZ(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"/*Q3*/
                                        SELECT Artikelnummer, [Bezeichnung 1] AS [Artikel Kunde], Stundensatz AS [Std Satz aktuell], Produktionszeit AS Artikelzeit, Produktivität AS [Prod Artikelzeit], 
	                                        AnzahlvonFertigungsnummer AS Fertigungen, [Produktivität FA] AS [Prod FA Zeit]
                                        FROM (
		                                        /*Q2*/
		                                        SELECT Artikelnummer, Stundensatz, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS Produktivität, Count(Fertigungsnummer) AS AnzahlvonFertigungsnummer, 
			                                        [Bezeichnung 1], Avg(([Anzahl]*[MittelwertvonZeit]/[SummevonMinuten])) AS [Produktivität FA], Produktionszeit, Avg(MittelwertvonZeit) AS Zeit
		                                        FROM (
			                                        /*Q1*/
			                                        SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
				                                        Sum(IIf([Minuten]=0,0.00000001,[Minuten])) AS SummevonMinuten, Artikel.[Bezeichnung 1], Avg(Fertigung.Zeit) AS MittelwertvonZeit
			                                        FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) INNER JOIN StundenCZ ON Fertigung.Fertigungsnummer = StundenCZ.Auftragsnummer
			                                        GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1], Fertigung.Kennzeichen
			                                        HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Fertigung.Kennzeichen)='erledigt'))
			                                        ) AS Q1
		                                        WHERE [Anzahl]*[Produktionszeit]/[SummevonMinuten]<2
		                                        GROUP BY Artikelnummer, Stundensatz, [Bezeichnung 1], Produktionszeit
                                        ) AS Q2;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails> GetProductivityDetails_CZ(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT Fertigungsnummer AS FA, Termin_Bestätigt1 AS Termin, Anzahl, Zeit AS [FA Zeit], Avg(([Anzahl]*[Zeit]/[SummevonMinuten])) AS [Prod FA Zeit], Produktionszeit AS Artikelzeit, 
	                                    Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS [Prod Artikelzeit]
                                    FROM 
                                    (
	                                    /*Q1*/
	                                    SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
		                                    Sum(StundenCZ.Minuten) AS SummevonMinuten, Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit
	                                    FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
		                                    INNER JOIN StundenCZ ON Fertigung.Fertigungsnummer = StundenCZ.Auftragsnummer
	                                    GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, 
		                                    Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit, Fertigung.Kennzeichen
	                                    HAVING (((Fertigung.Kennzeichen)='erledigt'))
                                    ) AS Q1
                                    GROUP BY Fertigungsnummer, Termin_Bestätigt1, Anzahl, Zeit, Produktionszeit, Artikelnummer, Stundensatz
                                    HAVING (((Artikelnummer)=@artikelnummer))
                                    ORDER BY Termin_Bestätigt1;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> GetProductivity_TN(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"/*Q3*/
                                        SELECT Artikelnummer, [Bezeichnung 1] AS [Artikel Kunde], Stundensatz AS [Std Satz aktuell], Produktionszeit AS Artikelzeit, Produktivität AS [Prod Artikelzeit], AnzahlvonFertigungsnummer AS Fertigungen, [Produktivität FA] AS [Prod FA Zeit]
                                        FROM (
	                                        /*Q2*/
	                                        SELECT Artikelnummer, Stundensatz, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS Produktivität, Count(Fertigungsnummer) AS AnzahlvonFertigungsnummer, 
		                                        [Bezeichnung 1], Avg(([Anzahl]*[MittelwertvonZeit]/[SummevonMinuten])) AS [Produktivität FA], Produktionszeit, Avg(MittelwertvonZeit) AS Zeit
	                                        FROM (
		                                        /*Q1*/
		                                        SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
			                                        Sum(IIf([Minuten]=0,0.00000001,[Minuten])) AS SummevonMinuten, Artikel.[Bezeichnung 1], Avg(Fertigung.Zeit) AS MittelwertvonZeit
		                                        FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) INNER JOIN StundenTN 
			                                        ON Fertigung.Fertigungsnummer = StundenTN.Auftragsnummer
		                                        GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, 
			                                        Artikel.[Bezeichnung 1], Fertigung.Kennzeichen
		                                        HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Fertigung.Kennzeichen)='erledigt'))
	                                        ) AS Q1
	                                        WHERE (((([Anzahl]*[Produktionszeit]/[SummevonMinuten]))<2))
	                                        GROUP BY Artikelnummer, Stundensatz, [Bezeichnung 1], Produktionszeit
                                        ) AS Q2";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails> GetProductivityDetails_TN(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT Fertigungsnummer AS FA, Termin_Bestätigt1 AS Termin, Anzahl, Zeit AS [FA Zeit], Avg(([Anzahl]*[Zeit]/[SummevonMinuten])) AS [Prod FA Zeit], Produktionszeit AS Artikelzeit, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS [Prod Artikelzeit]
                                    FROM 
                                    (
	                                    /*Q1*/
	                                    SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
		                                    Sum(StundenTN.Minuten) AS SummevonMinuten, Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit
	                                    FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) INNER JOIN StundenTN 
		                                    ON Fertigung.Fertigungsnummer = StundenTN.Auftragsnummer
	                                    GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, 
		                                    Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit, Fertigung.Kennzeichen
	                                    HAVING (((Fertigung.Kennzeichen)='erledigt'))
                                    ) AS Q1
                                    GROUP BY Fertigungsnummer, Termin_Bestätigt1, Anzahl, Zeit, Produktionszeit, Artikelnummer, Stundensatz
                                    HAVING (((Artikelnummer)=@artikelnummer))
                                    ORDER BY Termin_Bestätigt1;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> GetProductivity_AL(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"/*Q3*/
                                        SELECT Artikelnummer, [Bezeichnung 1] AS [Artikel Kunde], Stundensatz AS [Std Satz aktuell], Produktionszeit AS Artikelzeit, Produktivität AS [Prod Artikelzeit], AnzahlvonFertigungsnummer AS Fertigungen, [Produktivität FA] AS [Prod FA Zeit]
                                        FROM (
	                                        /*Q2*/
	                                        SELECT Artikelnummer, Stundensatz, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS Produktivität, Count(Fertigungsnummer) AS AnzahlvonFertigungsnummer, [Bezeichnung 1], Avg(([Anzahl]*[MittelwertvonZeit]/[SummevonMinuten])) AS [Produktivität FA], Produktionszeit, Avg(MittelwertvonZeit) AS Zeit
	                                        FROM 
	                                        ( 
		                                        /*Q1*/
		                                        SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit,		
			                                        Artikel.[Bezeichnung 1], Avg(Fertigung.Zeit) AS MittelwertvonZeit, Sum(IIf([Minuten]=0,0.00000001,[Minuten])) AS SummevonMinuten
		                                        FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
			                                        INNER JOIN StundenAL s ON Fertigung.Fertigungsnummer = s.Auftragsnummer
		                                        GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1],
			                                        Fertigung.Kennzeichen
		                                        HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Fertigung.Kennzeichen)='erledigt'))
	                                        ) AS Q1
	                                        WHERE (((([Anzahl]*[Produktionszeit]/[SummevonMinuten]))<2))
	                                        GROUP BY Artikelnummer, Stundensatz, [Bezeichnung 1], Produktionszeit
                                        ) AS Q2";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails> GetProductivityDetails_AL(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT Fertigungsnummer AS FA, Termin_Bestätigt1 AS Termin, Anzahl, Zeit AS [FA Zeit], Avg(([Anzahl]*[Zeit]/[SummevonMinuten])) AS [Prod FA Zeit], Produktionszeit AS Artikelzeit, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS [Prod Artikelzeit]
                                    FROM (
	                                    /*Q1*/
	                                    SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
		                                    Sum(s.Minuten) AS SummevonMinuten, Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit
	                                    FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
		                                    INNER JOIN StundenAL s ON Fertigung.Fertigungsnummer = s.Auftragsnummer
	                                    GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1], 
	                                    Fertigung.Termin_Bestätigt1, Fertigung.Zeit, Fertigung.Kennzeichen
	                                    HAVING (((Fertigung.Kennzeichen)='erledigt'))
                                    ) AS Q1
                                    GROUP BY Fertigungsnummer, Termin_Bestätigt1, Anzahl, Zeit, Produktionszeit, Artikelnummer, Stundensatz
                                    HAVING (((Artikelnummer)=@artikelnummer))
                                    ORDER BY Termin_Bestätigt1;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> GetProductivity_WS(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"/*Q3*/
                                        SELECT Artikelnummer, [Bezeichnung 1] AS [Artikel Kunde], Stundensatz AS [Std Satz aktuell], Produktionszeit AS Artikelzeit, Produktivität AS [Prod Artikelzeit], AnzahlvonFertigungsnummer AS Fertigungen, [Produktivität FA] AS [Prod FA Zeit]
                                        FROM (
	                                        /*Q2*/
	                                        SELECT Artikelnummer, Stundensatz, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS Produktivität, Count(Fertigungsnummer) AS AnzahlvonFertigungsnummer, [Bezeichnung 1], Avg(([Anzahl]*[MittelwertvonZeit]/[SummevonMinuten])) AS [Produktivität FA], Produktionszeit, Avg(MittelwertvonZeit) AS Zeit
	                                        FROM (
		                                        /*Q1*/
		                                        SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
			                                        Sum(IIf([Minuten]=0,0.00000001,[Minuten])) AS SummevonMinuten, Artikel.[Bezeichnung 1], Avg(Fertigung.Zeit) AS MittelwertvonZeit
		                                        FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
			                                        INNER JOIN StundenWS ON Fertigung.Fertigungsnummer = StundenWS.Auftragsnummer
		                                        GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1], Fertigung.Kennzeichen
		                                        HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Fertigung.Kennzeichen)='erledigt'))
	                                        ) AS Q1
	                                        WHERE (((([Anzahl]*[Produktionszeit]/[SummevonMinuten]))<2))
	                                        GROUP BY Artikelnummer, Stundensatz, [Bezeichnung 1], Produktionszeit
                                        ) AS Q2";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails> GetProductivityDetails_WS(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT Fertigungsnummer AS FA, Termin_Bestätigt1 AS Termin, Anzahl, Zeit AS [FA Zeit], Avg(([Anzahl]*[Zeit]/[SummevonMinuten])) AS [Prod FA Zeit], Produktionszeit AS Artikelzeit, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS [Prod Artikelzeit]
                                    FROM (
	                                    /*Q1*/
	                                    SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
		                                    Sum(StundenWS.Minuten) AS SummevonMinuten, Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit
	                                    FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
		                                    INNER JOIN StundenWS ON Fertigung.Fertigungsnummer = StundenWS.Auftragsnummer
	                                    GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit, Fertigung.Kennzeichen
	                                    HAVING (((Fertigung.Kennzeichen)='erledigt'))
                                    ) AS Q1
                                    GROUP BY Fertigungsnummer, Termin_Bestätigt1, Anzahl, Zeit, Produktionszeit, Artikelnummer, Stundensatz
                                    HAVING (((Artikelnummer)=@artikelnummer))
                                    ORDER BY Termin_Bestätigt1;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> GetProductivity_GZ(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"/*Q3*/
                                        SELECT Artikelnummer, [Bezeichnung 1] AS [Artikel Kunde], Stundensatz AS [Std Satz aktuell], Produktionszeit AS Artikelzeit, Produktivität AS [Prod Artikelzeit], AnzahlvonFertigungsnummer AS Fertigungen, [Produktivität FA] AS [Prod FA Zeit]
                                        FROM (
	                                        /*Q2*/
	                                        SELECT Artikelnummer, Stundensatz, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS Produktivität, Count(Fertigungsnummer) AS AnzahlvonFertigungsnummer, [Bezeichnung 1], Avg(([Anzahl]*[MittelwertvonZeit]/[SummevonMinuten])) AS [Produktivität FA], Produktionszeit, Avg(MittelwertvonZeit) AS Zeit
	                                        FROM (
		                                        /*Q1*/
		                                        SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
			                                        Sum(IIf([Minuten]=0,0.00000001,[Minuten])) AS SummevonMinuten, Artikel.[Bezeichnung 1], Avg(Fertigung.Zeit) AS MittelwertvonZeit
		                                        FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
			                                        INNER JOIN StundenGZTN ON Fertigung.Fertigungsnummer = StundenGZTN.Auftragsnummer
		                                        GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1], Fertigung.Kennzeichen
		                                        HAVING (((Artikel.Artikelnummer)=@artikelnummer) AND ((Fertigung.Kennzeichen)='erledigt'))
	                                        ) AS Q1
	                                        WHERE (((([Anzahl]*[Produktionszeit]/[SummevonMinuten]))<2))
	                                        GROUP BY Artikelnummer, Stundensatz, [Bezeichnung 1], Produktionszeit
                                        ) AS Q2";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails> GetProductivityDetails_GZ(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT Fertigungsnummer AS FA, Termin_Bestätigt1 AS Termin, Anzahl, Zeit AS [FA Zeit], Avg(([Anzahl]*[Zeit]/[SummevonMinuten])) AS [Prod FA Zeit], Produktionszeit AS Artikelzeit, Avg(([Anzahl]*[Produktionszeit]/[SummevonMinuten])) AS [Prod Artikelzeit]
                                    FROM (
	                                    /*Q1*/
	                                    SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt AS Anzahl, Artikel.Stundensatz, Artikel.Produktionszeit, 
		                                    Sum(StundenGZTN.Minuten) AS SummevonMinuten, Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit
	                                    FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
		                                    INNER JOIN StundenGZTN ON Fertigung.Fertigungsnummer = StundenGZTN.Auftragsnummer
	                                    GROUP BY Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Anzahl_erledigt, Artikel.Stundensatz, Artikel.Produktionszeit, Artikel.[Bezeichnung 1], Fertigung.Termin_Bestätigt1, Fertigung.Zeit, Fertigung.Kennzeichen
	                                    HAVING (((Fertigung.Kennzeichen)='erledigt'))
                                    ) AS Q1
                                    GROUP BY Fertigungsnummer, Termin_Bestätigt1, Anzahl, Zeit, Produktionszeit, Artikelnummer, Stundensatz
                                    HAVING (((Artikelnummer)=@artikelnummer))
                                    ORDER BY Termin_Bestätigt1;";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails>();
				}
			}
			public static List<KeyValuePair<string, string>> GetTools(string articleNumber)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT 'tn' AS Warehouse, [Inventarnummer JPM] AS Tool FROM [Kontakte] WHERE [Artikelnummer JPM]=@artikelnummer";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("artikelnummer", articleNumber ?? "");
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<string, string>(x[0].ToString(), x[1].ToString())).ToList();
				}
				else
				{
					return new List<KeyValuePair<string, string>>();
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Binsh> GetBinsh(string articleNumber, string designation, string confirmationStatus, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize <= 0)
					pageSize = 1;
				if(string.IsNullOrWhiteSpace(sortColumn))
					sortColumn = "Artikelnummer";

				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT 
	                                    [Artikel-Nr],Artikelnummer, [Bezeichnung 1], IIf([Prüfstatus TN Ware]='N','100%',IIf([Prüfstatus TN Ware]='UL','100%',
	                                    IIf([Prüfstatus TN Ware]='P','100%',IIf([Prüfstatus TN Ware]='E','10%',IIf([Prüfstatus TN Ware]='F','0%',
	                                    IIf([Prüfstatus TN Ware]='T','keine Prüfung in CZ', '')))))) AS Prüftiefe, [Prüfstatus TN Ware] AS [Prüfstatus TN_AL]
                                    FROM Artikel
                                    WHERE ((Artikelnummer Like '%TN' AND [Prüfstatus TN Ware] Is Not Null AND Freigabestatus<>'O') 
	                                    OR (Artikelnummer Like '%AL' AND [Prüfstatus TN Ware] Is Not Null AND Freigabestatus<>'O'))";

					if(!string.IsNullOrWhiteSpace(articleNumber))
					{ query += $" AND [Artikelnummer] LIKE '{articleNumber.SqlEscape()}%'"; }
					if(!string.IsNullOrWhiteSpace(designation))
					{ query += $" AND [Bezeichnung 1] LIKE '{designation.SqlEscape()}%'"; }
					if(!string.IsNullOrWhiteSpace(confirmationStatus))
					{ query += $" AND [Prüfstatus TN Ware] LIKE '{confirmationStatus.SqlEscape()}%'"; }

					query += $" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Binsh(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Binsh>();
				}
			}
			public static int GetBinsh_Count(string articleNumber, string designation, string confirmationStatus)
			{
				articleNumber = (articleNumber ?? "").Trim();
				designation = (designation ?? "").Trim();
				confirmationStatus = (confirmationStatus ?? "").Trim();
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT 
	                                    Count(*) as c
                                    FROM Artikel
                                    WHERE ((Artikelnummer Like '%TN' AND [Prüfstatus TN Ware] Is Not Null AND Freigabestatus<>'O') 
	                                    OR (Artikelnummer Like '%AL' AND [Prüfstatus TN Ware] Is Not Null AND Freigabestatus<>'O'))";

					if(!string.IsNullOrWhiteSpace(articleNumber))
					{ query += $" AND [Artikelnummer] LIKE '{articleNumber.SqlEscape()}%'"; }
					if(!string.IsNullOrWhiteSpace(designation))
					{ query += $" AND [Bezeichnung 1] LIKE '{designation.SqlEscape()}%'"; }
					if(!string.IsNullOrWhiteSpace(confirmationStatus))
					{ query += $" AND [Prüfstatus TN Ware] LIKE '{confirmationStatus.SqlEscape()}%'"; }

					var sqlCommand = new SqlCommand(query, sqlConnection);

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_SiteArticle> GetSiteArticles(string siteSuffix, int siteIndex)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT Artikelnummer ArticleNumber, a.Index_Kunde KundenIndex, [Bezeichnung 1] Designation, Warengruppe [Type], b.[Projekt-Nr] ABProjectNr
										FROM Artikel a 
										LEFT JOIN [angebotene Artikel] p on p.[Artikel-Nr] = a.[Artikel-Nr]
										LEFT JOIN Angebote b on b.Nr=p.[Angebot-Nr]
										LEFT JOIN [__BSD_ArtikelProductionExtension] e on e.ArticleId=a.[Artikel-Nr]
										WHERE (Artikelnummer LIKE '%{siteSuffix.SqlEscape()}' OR e.ProductionPlace1_Id IS NULL OR e.ProductionPlace1_Id={siteIndex}) 
										AND isnull(b.erledigt,0)=0 and isnull(p.erledigt_pos,0)=0
										ORDER BY artikelnummer;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_SiteArticle(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_SiteBom> GetSiteBoms(string siteSuffix, int siteIndex)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT DISTINCT a.Artikelnummer, s.Artikelnummer Material, ss.Anzahl Quantity, ss.[Bezeichnung des Bauteils] Bezeichnung
										FROM Artikel a Join Stücklisten ss on ss.[Artikel-Nr]=a.[Artikel-Nr]
										Join Artikel s on s.[Artikel-Nr]=ss.[Artikel-Nr des Bauteils]
										Left Join __BSD_ArtikelProductionExtension e on e.ArticleId=a.[Artikel-Nr]
										WHERE (a.Artikelnummer LIKE '%{siteSuffix.SqlEscape()}' OR e.ProductionPlace1_Id={siteIndex}) 
										ORDER BY artikelnummer;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_SiteBom(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_HbgUbg> GetHbgUbg()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT Artikel.[Artikel-Nr],Artikel.Artikelnummer AS [HBG_FG], Artikel.Losgroesse AS [Losgroesse_HBG], Artikel.Freigabestatus AS [HBG Freigabestatus], 
										Artikel_1.Artikelnummer AS [UBG Artikelnummer], Stücklisten.Anzahl AS [Menge_Stückliste], 
										Artikel_1.Warengruppe AS [UBG Warengruppe], Artikel_1.Losgroesse AS [Losgroesse UBG], Artikel_1.UBG
									FROM Artikel INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
									INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]
									WHERE Artikel.Freigabestatus<>'O' AND Artikel_1.Warengruppe='EF' ORDER BY Artikel.Artikelnummer ASC;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_HbgUbg(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_SalesRa> GetOpenSalesRa()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"Select Artikelnummer, Rahmen, [Rahmen-Nr], Rahmenmenge, Rahmenauslauf, Rahmen2, [Rahmen-Nr2], Rahmenmenge2, Rahmenauslauf2, b.Einkaufspreis, r.Name1
										From Artikel a
											join Bestellnummern b on b.[Artikel-Nr] = a.[Artikel-Nr]
											join adressen r on r.Nr = b.[Lieferanten-Nr]
										Where a.aktiv = 1
											  and b.Standardlieferant = 1
											  AND (
													  IsNULL(Rahmen, 0) <> 0
													  or IsNULL([Rahmen-Nr], '') <> ''
													  or IsNULL([Rahmenmenge], 0) <> 0
													  or IsNULL(Rahmen2, 0) <> 0
													  or IsNULL([Rahmen-Nr2], '') <> ''
													  or IsNULL([Rahmenmenge2], 0) <> 0
												  )
										Order by Artikelnummer desc";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_SalesRa(x)).ToList();
				}
				else
				{
					return null;
				}
			}
		}
		public class Sales
		{
			public class PrsSupplierOrderHistoryEntity
			{
				public decimal? Anzahl { get; set; }
				public int? Bestellung_Nr { get; set; }
				public DateTime? Datum { get; set; }
				public decimal? Einzelpreis { get; set; }
				public decimal? Gesamtpreis { get; set; }
				public int? LagerId { get; set; }
				public string Lieferant { get; set; }
				public string Artikelnummer { get; set; }
				public string Position { get; set; }
				public int? OrderId { get; set; }
				public int? ArticleId { get; set; }
				public int? SupplierId { get; set; }
				public string Lager { get; set; }
				public PrsSupplierOrderHistoryEntity() { }
				public PrsSupplierOrderHistoryEntity(DataRow dataRow)
				{
					Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
					Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
					Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
					Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelpreis"]);
					Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
					LagerId = (dataRow["LagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerId"]);
					Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferant"]);
					Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
					Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
					// -
					OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderId"]);
					ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
					SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
					Lager = (dataRow["Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lager"]);
				}
				public PrsSupplierOrderHistoryEntity ShallowClone()
				{
					return new PrsSupplierOrderHistoryEntity
					{
						Anzahl = Anzahl,
						Bestellung_Nr = Bestellung_Nr,
						Datum = Datum,
						Einzelpreis = Einzelpreis,
						Gesamtpreis = Gesamtpreis,
						LagerId = LagerId,
						Lieferant = Lieferant
					};
				}
			}
			public static List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Sl_SupplierClass> GetSupplierClass()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT Nr, Name1, stufe FROM adressen a JOIN Lieferanten l on l.nummer=a.Nr;";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Sl_SupplierClass(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<Infrastructure.Data.Entities.Functions.ArticleROHNeedStockEntity> GetArticleROHNeedStock(List<string> adressenNrs, string articleNumber)
			{
				articleNumber = articleNumber ?? "";
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string whereClause = "";
					if(adressenNrs?.Count > 0)
					{
						whereClause = $"WHERE [PRIO1_Lieferant] IN ('{(string.Join("','", adressenNrs))}')";
					}
					if(!string.IsNullOrWhiteSpace(articleNumber))
					{
						if(string.IsNullOrWhiteSpace(whereClause))
						{
							whereClause = $"WHERE Artikelnummer LIKE '{articleNumber}'%";
						}
						else
						{
							whereClause += $" AND Artikelnummer LIKE '{articleNumber}'%";
						}
					}
					string query = $@"SELECT * FROM [stats].[MaterialRequirements] {whereClause}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Functions.ArticleROHNeedStockEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static KeyValuePair<int, DateTime> GetArticleROHNeedStock_Sync()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
								INSERT INTO [stats].[MaterialRequirementsHeader] (
								[Artikelnr],[Artikelnummer]
								,[Lager]
								,[Bestellung]
								,[PRIO1_Lieferant]
								,[LieferantArtikelnummer]
								,[EK]
								,[Lieferzeit]
								,[VPE_Losgroesse]
								,[GesamtbedarfOffeneFA360]
								,[Verfugbarbestand]
								,[Min_Lagerbestand]
								,[BedarfPO]
								,[SummePO]
								,SyncId
								,Mindestbestellmenge
								,SyncDate
								)

								SELECT a.[Artikel-Nr], a.Artikelnummer, l.LaBestand AS Lager, ISNULL(b.Anzahl,0) AS Bestellung, s.Name1 AS PRIO1_Lieferant, n.[Bestell-Nr] AS LieferantArtikelnummer, n.Einkaufspreis AS EK,
								n.Wiederbeschaffungszeitraum, n.Verpackungseinheit AS VPE_Losgroesse, ISNULL(f.FaAnzahl,0) AS GesamtbedarfOffeneFA360, l.LaBestand_reserviert AS Verfugbarbestand /**/, ISNULL(l.LaMindestbestand,0)/*+ISNULL(f.FaAnzahl,0)*/ AS Min_Lagerbestand
								,0 [BedarfPO], 0 [SummePO], (SELECT ISNULL(MAX(ISNULL(SyncId,0)),0)+1 as sc FROM [stats].[MaterialRequirementsHeader]) SyncId,n.Mindestbestellmenge, GETDATE() as SyncDate
								FROM Artikel a
								Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) AS LaBestand, SUM(ISNULL(Bestand_reserviert,0)) AS LaBestand_reserviert, MAX(ISNULL(Mindestbestand,0)) AS LaMindestbestand FROM Lager WHERE Lagerort_id NOT IN (SELECT Lagerort_id FROM Lagerorte WHERE Lagerort like 'repl%') GROUP BY [Artikel-Nr]) AS l on l.[Artikel-Nr]=a.[Artikel-Nr]
								Left Join (SELECT ba.[Artikel-Nr], SUM(ISNULL(ba.Anzahl,0)) AS Anzahl FROM Bestellungen bs Join [bestellte Artikel] ba on ba.[Bestellung-Nr]=bs.Nr WHERE bs.Typ='Bestellung' AND ISNULL(bs.erledigt,0)<>0 AND ISNULL(ba.erledigt_pos,0)<>0 GROUP BY [Artikel-Nr]) AS b on b.[Artikel-Nr]=a.[Artikel-Nr]
								Left Join (SELECT [Lieferanten-Nr],[Artikel-Nr], Einkaufspreis, [Bestell-Nr],Verpackungseinheit,Mindestbestellmenge,Wiederbeschaffungszeitraum FROM Bestellnummern WHERE ISNULL(Standardlieferant,0)=1) AS n on n.[Artikel-Nr]=a.[Artikel-Nr]
								Left Join adressen s on s.Nr=n.[Lieferanten-Nr]
								Left Join (SELECT fp.Artikel_Nr, SUM(ISNULL(fp.Anzahl,0)) AS FaAnzahl FROM Fertigung ff Join Fertigung_Positionen fp on fp.ID_Fertigung=ff.ID Where ff.Kennzeichen='offen' AND Datum>=GETDATE()-360 GROUP BY fp.Artikel_Nr) AS f on f.Artikel_Nr=a.[Artikel-Nr]
								WHERE a.[Artikel-Nr] IS NOT NULL AND a.Warengruppe <> 'EF' and ISNULL(a.aktiv,0)=1 and ISNULL(a.UBG,0)=0;

								SELECT TOP 1 SyncId, SyncDate FROM [stats].[MaterialRequirementsHeader] WHERE SyncId=(SELECT MAX(SyncId) FROM [stats].[MaterialRequirementsHeader]);";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return new KeyValuePair<int, DateTime>(int.TryParse(dataTable.Rows[0][0].ToString(), out var i) ? i : 0, DateTime.TryParse(dataTable.Rows[0][1].ToString(), out var j) ? j : DateTime.MinValue);
				}
				else
				{
					return new KeyValuePair<int, DateTime>(0, DateTime.MinValue);
				}
			}
			public static KeyValuePair<int, DateTime> GetArticleROHNeedStock_Sync_PO()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
									DECLARE @maxSyncId int;
									SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[MaterialRequirementsHeader];
									DELETE FROM [stats].[MaterialRequirementsPo] WHERE [SyncId]=@maxSyncId;
									INSERT INTO [stats].[MaterialRequirementsPo]([Artikelnr],[PoQuantity],[Year],[CW],[SyncId])

									SELECT [Artikel-Nr] as [Artikelnr], SUM(Anzahl) [PoQuantity], DATEPART(YEAR, Bestätigter_Termin) AS [Year], DATEPART(iso_week, Bestätigter_Termin) AS [CW], @maxSyncId AS [SyncId]
									FROM (SELECT [Artikel-Nr],Bestätigter_Termin,Anzahl
									FROM [bestellte Artikel]  ba
									INNER JOIN Bestellungen b ON ba.[Bestellung-Nr] = b.Nr
									WHERE Bestätigter_Termin<DATEADD(YEAR, 1, CAST(GETDATE() AS DATE)) /* 1 year from today */ 
										AND [erledigt_pos] <> 1 AND Anzahl <> 0 
										AND [Artikel-Nr] IN (SELECT DISTINCT ArtikelNr FROM [stats].[MaterialRequirementsHeader] WHERE SyncId=@maxSyncId)
										AND b.Typ = 'Bestellung' AND ISNULL(b.Rahmenbestellung,0)<>1) as s 
									GROUP BY DATEPART(YEAR, Bestätigter_Termin), DATEPART(iso_week, Bestätigter_Termin), [Artikel-Nr]";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return new KeyValuePair<int, DateTime>(int.TryParse(dataTable.Rows[0][0].ToString(), out var i) ? i : 0, DateTime.TryParse(dataTable.Rows[0][1].ToString(), out var j) ? j : DateTime.MinValue);
				}
				else
				{
					return new KeyValuePair<int, DateTime>(0, DateTime.MinValue);
				}
			}
			public static KeyValuePair<int, DateTime> GetArticleROHNeedStock_Sync_FA()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"DECLARE @maxSyncId int;
									SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[MaterialRequirementsHeader];
									DELETE FROM [stats].[MaterialRequirementsFa] WHERE [SyncId]=@maxSyncId;
									INSERT INTO [stats].[MaterialRequirementsFa]([Artikelnr], [FaQuantity], [Year], [CW], [SyncId])	
									SELECT FP.[Artikel_Nr], SUM(FP.Anzahl/F.Originalanzahl*F.Anzahl) AS [FaQuantity],
									DATEPART(YEAR, DATEADD(DAY,-28,F.Termin_Bestätigt1)) AS [Year], DATEPART(iso_week, DATEADD(DAY,-28,F.Termin_Bestätigt1)) AS [CW], @maxSyncId AS [SyncId]
									FROM Fertigung F INNER JOIN Fertigung_Positionen FP  ON F.ID = FP.ID_Fertigung
									INNER JOIN Artikel A ON FP.[Artikel_Nr] = A.[Artikel-Nr]
									WHERE A.[Artikel-Nr] IN (SELECT DISTINCT ArtikelNr FROM [stats].[MaterialRequirementsHeader] WHERE SyncId=(@maxSyncId))
									AND DATEADD(DAY,-28,F.Termin_Bestätigt1)<DATEADD(YEAR, 1, CAST(GETDATE() AS DATE)) /* 1 year from today */ 
									AND F.Kennzeichen = 'offen' AND IsNULL(F.FA_Gestartet,0)=0 
									AND F.Originalanzahl <> 0 AND F.Originalanzahl IS NOT NULL
									AND F.Anzahl IS NOT NULL AND F.Anzahl <> 0
									GROUP BY FP.[Artikel_Nr],DATEPART(YEAR, DATEADD(DAY,-28,F.Termin_Bestätigt1)), DATEPART(iso_week, DATEADD(DAY,-28,F.Termin_Bestätigt1))";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return new KeyValuePair<int, DateTime>(int.TryParse(dataTable.Rows[0][0].ToString(), out var i) ? i : 0, DateTime.TryParse(dataTable.Rows[0][1].ToString(), out var j) ? j : DateTime.MinValue);
				}
				else
				{
					return new KeyValuePair<int, DateTime>(0, DateTime.MinValue);
				}
			}
			public static int GetArticleROHNeedStock_Sync_Details()
			{
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"DECLARE @maxSyncId int;
										SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[MaterialRequirementsHeader];
										TRUNCATE TABLE [stats].[MaterialRequirementsFaNeedsSnapshot];
										INSERT INTO [stats].[MaterialRequirementsFaNeedsSnapshot]([FaId], [FaMaterialArticleId], [FaMaterialOpenQuantity])	
										SELECT F.ID, FP.[Artikel_Nr], SUM(FP.Anzahl/F.Originalanzahl*F.Anzahl)
										FROM Fertigung F INNER JOIN Fertigung_Positionen FP  ON F.ID = FP.ID_Fertigung
										INNER JOIN Artikel A ON FP.[Artikel_Nr] = A.[Artikel-Nr]
										WHERE A.[Artikel-Nr] IN (SELECT DISTINCT ArtikelNr FROM [stats].[MaterialRequirementsHeader] WHERE SyncId=(@maxSyncId))
										AND F.Termin_Bestätigt1<DATEADD(YEAR, 1, CAST(GETDATE() AS DATE)) /* 1 year from today */ 
										AND F.Kennzeichen = 'offen' AND IsNULL(F.FA_Gestartet,0)=0 
										AND F.Originalanzahl <> 0 AND F.Originalanzahl IS NOT NULL
										AND F.Anzahl IS NOT NULL AND F.Anzahl <> 0
										GROUP BY F.ID, FP.[Artikel_Nr];

										/* Fa data */
										UPDATE [stats].[MaterialRequirementsFaNeedsSnapshot] SET
										[FaNumber]=F.Fertigungsnummer, [FaOpenQuantity]=F.Anzahl, [FaArticleId]=F.Artikel_Nr, [FaDate]=F.Termin_Bestätigt1, [FaMaterialDate]=F.Termin_Bestätigt1-28, [FaProductionSite]=F.Lagerort_id  
										FROM [stats].[MaterialRequirementsFaNeedsSnapshot] s JOIN Fertigung F on F.ID=s.FaId;
										/* Fa article */
										UPDATE [stats].[MaterialRequirementsFaNeedsSnapshot] SET
										[FaArticleNumber]=CAST(A.Artikelnummer AS NVARCHAR(25))
										FROM [stats].[MaterialRequirementsFaNeedsSnapshot] s JOIN Artikel A on A.[Artikel-Nr]=s.[FaArticleId];
										/* Fa stl article */
										UPDATE [stats].[MaterialRequirementsFaNeedsSnapshot] SET
										[FaMaterialArticleNumber]=CAST(A.Artikelnummer AS NVARCHAR(25))
										FROM [stats].[MaterialRequirementsFaNeedsSnapshot] s JOIN Artikel A on A.[Artikel-Nr]=s.[FaMaterialArticleId];";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}
			public static List<MaterialRequirementsFaNeedsSnapshotEntity> GetArticleROHNeedStock_Details()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[MaterialRequirementsFaNeedsSnapshot]";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new MaterialRequirementsFaNeedsSnapshotEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<MaterialRequirementsHeaderEntity> GetArticleROHNeedStock_Header(List<string> adressenNrs, string articleNumber, bool fa30Postive, bool onlyPrioSupplier, string classification, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
			{
				articleNumber = articleNumber ?? "";
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string whereClause = "";
					if(adressenNrs?.Count > 0)
					{
						whereClause += $" AND m.[PRIO1_Lieferant] IN ('{(string.Join("','", adressenNrs))}')";
					}
					if(!string.IsNullOrWhiteSpace(articleNumber))
					{
						whereClause += $" AND m.Artikelnummer LIKE '{articleNumber}%'";
					}
					if(!string.IsNullOrWhiteSpace(classification))
					{
						whereClause += $" AND a.artikelklassifizierung LIKE '{classification}%'";
					}
					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						whereClause += $" ORDER BY {(sorting.SortFieldName == "m.[ArtikelNummer]" ? "CAST(Replace(LEFT(SUBSTRING(m.[ArtikelNummer], PATINDEX('%[0-9.-]%', m.[ArtikelNummer]), 8000), PATINDEX('%[^0-9.-]%', SUBSTRING(m.[ArtikelNummer], PATINDEX('%[0-9.-]%', m.[ArtikelNummer]), 8000) + 'X') -1), '-', '') AS BIGINT)" : sorting.SortFieldName)} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					else
					{
						whereClause += " ORDER BY m.[PRIO1_Lieferant], m.[Artikelnummer]";
					}

					if(paging != null)
					{
						whereClause += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					string query = $@"SELECT m.*,a.artikelklassifizierung,a.[Bezeichnung 2] FROM [stats].[MaterialRequirementsHeader] m inner join Artikel a on m.Artikelnr=a.[Artikel-Nr]
                                    WHERE SyncId=(SELECT MAX(SyncId) FROM [stats].[MaterialRequirementsHeader]){(fa30Postive ? " AND [GesamtbedarfOffeneFA360]>0" : "")}{whereClause}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new MaterialRequirementsHeaderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetArticleROHNeedStock_Header_count(List<string> adressenNrs, string articleNumber, bool fa30Postive, bool onlyPrioSupplier, string classification)
			{
				articleNumber = articleNumber ?? "";
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string whereClause = "";
					if(adressenNrs?.Count > 0)
					{
						whereClause += $" AND m.[PRIO1_Lieferant] IN ('{(string.Join("','", adressenNrs))}')";
					}
					if(!string.IsNullOrWhiteSpace(articleNumber))
					{
						whereClause += $" AND m.Artikelnummer LIKE '{articleNumber}%'";
					}
					if(!string.IsNullOrWhiteSpace(classification))
					{
						whereClause += $" AND a.artikelklassifizierung LIKE '{classification}%'";
					}
					string query = $@"SELECT COUNT(*) FROM [stats].[MaterialRequirementsHeader] m inner join Artikel a on m.Artikelnr=a.[Artikel-Nr] 
                                      WHERE SyncId=(SELECT MAX(SyncId) FROM [stats].[MaterialRequirementsHeader]){(fa30Postive ? " AND [GesamtbedarfOffeneFA360]>0" : "")}{whereClause}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return int.TryParse(dataTable.Rows[0][0].ToString(), out var _x) ? _x : 0;
				}
				else
				{
					return 0;
				}
			}
			public static List<MaterialRequirementsQuantityEntity> GetArticleROHNeedStock_Po(int maxSyncId, List<int> articleNrs)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT ArtikelNr, CW, Year, PoQuantity AS Quantity FROM [stats].[MaterialRequirementsPo] WHERE SyncId={maxSyncId} {(articleNrs?.Count > 0 ? $" AND Artikelnr IN ({string.Join(",", articleNrs)})" : "")}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new MaterialRequirementsQuantityEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<MaterialRequirementsQuantityEntity> GetArticleROHNeedStock_Fa(int maxSyncId, List<int> articleNrs)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT ArtikelNr, CW, Year, FaQuantity AS Quantity FROM [stats].[MaterialRequirementsFa] WHERE SyncId={maxSyncId} {(articleNrs?.Count > 0 ? $" AND Artikelnr IN ({string.Join(",", articleNrs)})" : "")}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new MaterialRequirementsQuantityEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static MaterialRequirementsParamsEntity GetArticleROHNeedStock_SyncParams()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT TOP 1 SyncDate, SyncId FROM [stats].[MaterialRequirementsHeader] WHERE SyncId=(SELECT MAX(SyncId) FROM [stats].[MaterialRequirementsHeader])";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return new MaterialRequirementsParamsEntity(dataTable.Rows[0]);
				}
				else
				{
					return null;
				}
			}
			public static int GetSupplierArticles_Count(bool? isStandard, bool? isActive)
			{
				List<string> whereClause = null;
				if(isStandard.HasValue || isActive.HasValue)
				{
					whereClause = new List<string>();
					if(isStandard.HasValue)
					{
						whereClause.Add($"b.[Standardlieferant]={(isStandard.Value ? 1 : 0)}");
					}
					if(isActive.HasValue)
					{
						whereClause.Add($"a.[Aktiv]={(isActive.Value ? 1 : 0)}");
					}
				}
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
							SELECT COUNT(*)
							FROM Artikel a JOIN Bestellnummern b on b.[Artikel-Nr]=a.[Artikel-Nr] 
							LEFT JOIN adressen d on d.Nr=b.[Lieferanten-Nr] 
							{(whereClause?.Count > 0 ? $"WHERE {string.Join(" AND ", whereClause)}" : "")}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
				}
			}
			public static List<KeyValuePair<int, string>> GetSupplierLagers()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
									SELECT [LagerId], l.Lagerort AS [LagerName] FROM (
									SELECT [LagerId] FROM [stats].[PrsSupplierTotalOrders]
									UNION SELECT [LagerId] FROM [stats].[PrsSupplierClosedOrders]
									UNION SELECT [LagerId] FROM [stats].[PrsSupplierDelayedDelivery]
									UNION SELECT [LagerId] FROM [stats].[PrsSupplierOpenOrders]
									UNION SELECT [LagerId] FROM [stats].[PrsSupplierUnplacedOrders]
									UNION SELECT [LagerId] FROM [stats].[PrsSupplierUnconfirmedDelivery]
									UNION SELECT [LagerId] FROM [stats].[PrsSupplierDeliveryOverdue]
									UNION SELECT [LagerId] FROM [stats].[PrsSupplierNext4KwDelivery]) AS tmp 
									JOIN dbo.[Lagerorte] l on l.Lagerort_id=tmp.[LagerId]
									ORDER BY [LagerId];";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x["LagerId"].ToString(), out var _x) ? _x : 0, x["LagerName"].ToString())).ToList();
				}
				else
				{
					return null;
				}
			}
			public enum SupplierOrderStatType
			{
				Total = 1,
				Closed = 2,
				Delayed = 3,
				Open = 4,
				Unplaced = 5,
				Unconfirmed = 6,
				OverDue = 7,
				Next4Kw = 8
			}
			static Dictionary<SupplierOrderStatType, string> SupplierOrderStatTypeDic = new Dictionary<SupplierOrderStatType, string>(){
					{SupplierOrderStatType.Total, $@"WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND b.[Projekt-Nr] IN
												(SELECT [Projekt-Nr] FROM (
												/* created BE */
												SELECT DISTINCT b.[Projekt-Nr]
												FROM Bestellungen b JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr 
												WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND YEAR(b.Datum)=@year AND ISNULL(p.anzahl,0)>0 /* exclude fully booked BE (w/ WE) for currYear */
												UNION ALL
												/* closed BE */
												SELECT DISTINCT b.[Projekt-Nr]
												FROM Bestellungen b JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr
												JOIN (
													SELECT y.[WE Pos zu Bestellposition], Year(x.Datum) [WeYear]
													FROM Bestellungen x JOIN [bestellte Artikel] y on y.[Bestellung-Nr]=x.Nr 
													WHERE Typ='wareneingang' AND YEAR(x.Datum)=@year
												) w on w.[WE Pos zu Bestellposition]=p.Nr
												WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND [WeYear]=@year
												) as tmp)"},
					{SupplierOrderStatType.Closed, $@"WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 
														AND p.Nr IN (SELECT y.[WE Pos zu Bestellposition] 
															FROM Bestellungen x JOIN [bestellte Artikel] y on y.[Bestellung-Nr]=x.Nr 
															WHERE Typ='wareneingang' AND YEAR(x.Datum)=@year)"},
					{SupplierOrderStatType.Delayed, @"LEFT JOIN (SELECT [WE Pos zu Bestellposition], MAX(p.Liefertermin) AS Liefertermin 
										FROM Bestellungen w join [bestellte Artikel] p on p.[Bestellung-Nr]=w.Nr 
										WHERE w.Typ='Wareneingang' AND YEAR(w.Datum)>=@year AND ISNULL([WE Pos zu Bestellposition],0)>0 
										GROUP BY [WE Pos zu Bestellposition]) wp on wp.[WE Pos zu Bestellposition]=p.Nr
										WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND ISNULL(erledigt,0)=0 AND ISNULL(p.erledigt_pos,0)=0 AND p.Bestätigter_Termin<ISNULL(wp.Liefertermin,GETDATE()) AND YEAR(b.Datum)=@year"},
					{SupplierOrderStatType.Open, @"WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND YEAR(b.Datum)=@year AND ISNULL(p.anzahl,0)>0 /* exclude fully booked BE (w/ WE) for currYear */"},
					{SupplierOrderStatType.Unplaced, @"WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND YEAR(b.Datum)=@year AND b.Nr NOT IN (SELECT DISTINCT OrderId FROM [__PRS_OrderPlacementHistory])"},
					{SupplierOrderStatType.Unconfirmed, @"WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND ISNULL(p.Bestätigter_Termin,CAST(CONCAT(YEAR(GETDATE())+100,'01','01') AS DATETIME)) > CAST(CONCAT(YEAR(GETDATE())+5,'01','01') AS DATETIME)AND YEAR(b.Datum)=@year"},
					{SupplierOrderStatType.OverDue, @"WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND ISNULL(erledigt,0)=0 AND (ISNULL(p.anzahl,0)<=0 OR ISNULL(p.erledigt_pos,0)=0) 
										AND COALESCE(p.Bestätigter_Termin,p.Liefertermin,GETDATE())<GETDATE() AND p.Anzahl>0 AND YEAR(b.Datum)=@year"},
					{SupplierOrderStatType.Next4Kw, @"WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND ISNULL(erledigt,0)=0 AND (ISNULL(p.anzahl,0)<=0 OR ISNULL(p.erledigt_pos,0)=0) 
										AND GETDATE()<=COALESCE(p.Bestätigter_Termin,p.Liefertermin,GETDATE()+365) AND COALESCE(p.Bestätigter_Termin,p.Liefertermin,GETDATE()+365)<=GETDATE()+28 AND p.Anzahl>0 AND YEAR(b.Datum)=@year"},
			};
			public static KeyValuePair<int, DateTime> SupplierOverview_Sync(bool wTruncate = true)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"{(wTruncate ? $@"
								/* DELETE FROM [stats].[PrsSuppliers] WHERE [BeYear]=@year; */
								DELETE FROM [stats].[PrsSupplierTotalOrders] WHERE [BeYear]=@year;
								DELETE FROM [stats].[PrsSupplierClosedOrders] WHERE [BeYear]=@year;
								DELETE FROM [stats].[PrsSupplierDelayedDelivery] WHERE [BeYear]=@year;
								DELETE FROM [stats].[PrsSupplierOpenOrders] WHERE [BeYear]=@year;
								DELETE FROM [stats].[PrsSupplierUnplacedOrders] WHERE [BeYear]=@year;
								DELETE FROM [stats].[PrsSupplierUnconfirmedDelivery] WHERE [BeYear]=@year;
								DELETE FROM [stats].[PrsSupplierDeliveryOverdue] WHERE [BeYear]=@year;
								DELETE FROM [stats].[PrsSupplierNext4KwDelivery] WHERE [BeYear]=@year;
								" : "")}

								/** -- Supplier Basics -- */
								DROP TABLE IF EXISTS ##ActiveArticles;
								SELECT [Artikel-Nr] INTO ##ActiveArticles FROM Artikel WHERE ISNULL(aktiv,0)=1;
								INSERT INTO [stats].[PrsSuppliers](
								SupplierId,
								SupplierAddressNr,
								SupplierName,
								SupplierBlockedForFurtherBe, 
								SupplierAddressBlocked,
								Stufe,
								StandardActiveArticlesCount,
								AllActiveArticlesCount,
								StandardArticlesCount,
								AllArticlesCount, 
								BeYear, 
								BeKw,
								SyncId, 
								SyncDate
								)
								SELECT l.Nr AS SupplierId, a.Nr as SupplierAddressNr, a.Name1 as SupplierName, l.[gesperrt für weitere Bestellungen] as SupplierBlockedForFurtherBe
								, a.sperren as SupplierAddressBlocked, ISNULL(a.stufe,'') as Stufe, stda.CountArticle as StandardActiveArticlesCount, arta.CountArticle as AllActiveArticlesCount
								, std.CountArticle as StandardArticlesCount, art.CountArticle as AllArticlesCount, YEAR(GETDATE()) BeYear, DATEPART(ISO_WEEK, GETDATE()) as BeKw
								,(SELECT ISNULL(MAX(ISNULL(SyncId,0)),0)+1 as sc FROM [stats].[PrsSuppliers]) as SyncId, GETDATE() as SyncDate
								FROM Lieferanten l JOIN adressen a on a.Nr=l.nummer
								JOIN (SELECT [Lieferanten-Nr] SupplierAddressNr, COUNT(DISTINCT [Artikel-Nr]) as CountArticle FROM Bestellnummern WHERE ISNULL(Standardlieferant,0)=1 GROUP BY [Lieferanten-Nr]) std on std.SupplierAddressNr=l.nummer
								JOIN (SELECT [Lieferanten-Nr] SupplierAddressNr, COUNT(DISTINCT b.[Artikel-Nr]) as CountArticle FROM Bestellnummern b JOIN ##ActiveArticles a on a.[Artikel-Nr]=b.[Artikel-Nr] WHERE ISNULL(Standardlieferant,0)=1 GROUP BY [Lieferanten-Nr]) stda on stda.SupplierAddressNr=l.nummer
								JOIN (SELECT [Lieferanten-Nr] SupplierAddressNr, COUNT(DISTINCT [Artikel-Nr]) as CountArticle FROM Bestellnummern GROUP BY [Lieferanten-Nr]) art on art.SupplierAddressNr=l.nummer
								JOIN (SELECT [Lieferanten-Nr] SupplierAddressNr, COUNT(DISTINCT b.[Artikel-Nr]) as CountArticle FROM Bestellnummern b JOIN ##ActiveArticles a on a.[Artikel-Nr]=b.[Artikel-Nr] GROUP BY [Lieferanten-Nr]) arta on arta.SupplierAddressNr=l.nummer
								/* --GO */

								/* -- TOTAL ORDERS -- */
								DECLARE @maxSyncId int;
								SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[PrsSuppliers];
								INSERT INTO [stats].[PrsSupplierTotalOrders](
								SupplierAddressNr, 
								[LagerId],
								[BeYear],
								BeKw,
								BeCount, 
								BeArticleCount,
								BeArticleAmount,
								SyncId
								)
								SELECT SupplierAddressNr, Lagerort_id, [BeYear], [BeKw], SUM(ISNULL([BeCount],0)) [BeCount], 
								SUM(ISNULL([BeArticleCount],0)) [BeArticleCount], SUM(ISNULL([BeArticleAmount],0)) [BeArticleAmount], @maxSyncId
								FROM (
								/* created BE */
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, Year(b.Datum) [BeYear], DATEPART(ISO_WEEK, b.Datum) BeKw, 
								COUNT(DISTINCT b.Nr) BeCount, SUM(ISNULL(p.[Anzahl],0)) BeArticleCount, 
								SUM(ISNULL(p.[Anzahl],0)*ISNULL(p.Einzelpreis,0)) BeArticleAmount
								FROM Bestellungen b JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr 
								WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND YEAR(b.Datum)=@year AND ISNULL(p.anzahl,0)>0 /* exclude fully booked BE (w/ WE) for currYear */
								GROUP BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								UNION 
								/* closed BE */
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, w.[WeYear] [BeYear], w.[WeKw] BeKw, 
								COUNT(DISTINCT b.Nr) [BeCount], SUM(ISNULL(w.[WeArticleCount],0)) BeArticleCount, 
								SUM(ISNULL(w.[WeArticleAmount],0)) BeArticleAmount
								FROM Bestellungen b JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr
								JOIN (
									SELECT y.[WE Pos zu Bestellposition], Year(x.Datum) [WeYear], DATEPART(ISO_WEEK, x.Datum) [WeKw], 
									COUNT(DISTINCT x.Nr) [WeCount], SUM(ISNULL(y.[Anzahl],0)) [WeArticleCount], 
									SUM(ISNULL(y.[Anzahl],0)*ISNULL(y.Einzelpreis,0)) [WeArticleAmount]
									FROM Bestellungen x JOIN [bestellte Artikel] y on y.[Bestellung-Nr]=x.Nr 
									WHERE Typ='wareneingang' AND YEAR(x.Datum)=@year
									GROUP BY [WE Pos zu Bestellposition], Year(x.Datum), DATEPART(ISO_WEEK, x.Datum)
								) w on w.[WE Pos zu Bestellposition]=p.Nr
								WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND [WeYear]=@year
								GROUP BY [Lieferanten-Nr], w.[WeYear], w.[WeKw], p.Lagerort_id
								) as tmp
								GROUP BY SupplierAddressNr, [BeYear], [BeKw], Lagerort_id
								ORDER BY SupplierAddressNr, [BeYear], [BeKw], Lagerort_id
								/* --GO */

								/* -- CLOSED ORDERS -- */
								SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[PrsSuppliers];
								INSERT INTO [stats].[PrsSupplierClosedOrders](
								SupplierAddressNr, 
								[LagerId],
								[BeYear],
								BeKw,
								BeCount, 
								BeArticleCount,
								BeArticleAmount,
								SyncId
								)
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, w.[WeYear] [BeYear], w.[WeKw] BeKw, 
								COUNT(DISTINCT b.Nr) [BeCount], SUM(ISNULL(w.[WeArticleCount],0)) BeArticleCount, 
								SUM(ISNULL(w.[WeArticleAmount],0)) BeArticleAmount, @maxSyncId
								FROM Bestellungen b JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr
								JOIN (
									SELECT y.[WE Pos zu Bestellposition], Year(x.Datum) [WeYear], DATEPART(ISO_WEEK, x.Datum) [WeKw], 
									COUNT(DISTINCT x.Nr) [WeCount], SUM(ISNULL(y.[Anzahl],0)) [WeArticleCount], 
									SUM(ISNULL(y.[Anzahl],0)*ISNULL(y.Einzelpreis,0)) [WeArticleAmount]
									FROM Bestellungen x JOIN [bestellte Artikel] y on y.[Bestellung-Nr]=x.Nr 
									WHERE Typ='wareneingang' AND YEAR(x.Datum)=@year
									GROUP BY [WE Pos zu Bestellposition], Year(x.Datum), DATEPART(ISO_WEEK, x.Datum)
								) w on w.[WE Pos zu Bestellposition]=p.Nr
								WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 
								AND [WeYear]=@year
								GROUP BY [Lieferanten-Nr], w.[WeYear], w.[WeKw], p.Lagerort_id
								ORDER BY [Lieferanten-Nr], w.[WeYear], w.[WeKw], p.Lagerort_id
								/* --GO */

								/* -- Delayed ORDERS -- */ /* confirmed orders and not received on a lter date OR not received at all and confirmed-date is before today */
								SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[PrsSuppliers];
								INSERT INTO [stats].[PrsSupplierDelayedDelivery](
								SupplierAddressNr, 
								[LagerId],
								[BeYear],
								BeKw,
								BeCount, 
								BeArticleCount,
								BeArticleAmount,
								SyncId
								)
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, Year(b.Datum) [BeYear], DATEPART(ISO_WEEK, b.Datum) BeKw,COUNT(DISTINCT b.Nr) BeCount, 
								SUM(ISNULL(p.[Anzahl],0)) BeArticleCount, SUM(ISNULL(p.[Anzahl],0)*ISNULL(p.Einzelpreis,0)) BeArticleAmount, @maxSyncId
								FROM Bestellungen b 
									JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr 
									LEFT JOIN (SELECT [WE Pos zu Bestellposition], MAX(p.Liefertermin) AS Liefertermin 
										FROM Bestellungen w join [bestellte Artikel] p on p.[Bestellung-Nr]=w.Nr 
										WHERE w.Typ='Wareneingang' AND YEAR(w.Datum)>=@year AND ISNULL([WE Pos zu Bestellposition],0)>0 
										GROUP BY [WE Pos zu Bestellposition]) wp on wp.[WE Pos zu Bestellposition]=p.Nr
									WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND p.Bestätigter_Termin IS NOT NULL AND p.Bestätigter_Termin>GETDATE() AND p.Bestätigter_Termin<ISNULL(wp.Liefertermin,GETDATE()) AND YEAR(b.Datum)=@year
								GROUP BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								ORDER BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								/* --GO */

								/* -- OPEN ORDERS -- */
								SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[PrsSuppliers];
								INSERT INTO [stats].[PrsSupplierOpenOrders](
								SupplierAddressNr, 
								[LagerId],
								[BeYear],
								BeKw,
								BeCount, 
								BeArticleCount,
								BeArticleAmount,
								SyncId
								)
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, Year(b.Datum) [BeYear], DATEPART(ISO_WEEK, b.Datum) BeKw, 
								COUNT(DISTINCT b.Nr) BeCount, SUM(ISNULL(p.[Anzahl],0)) BeArticleCount, 
								SUM(ISNULL(p.[Anzahl],0)*ISNULL(p.Einzelpreis,0)) BeArticleAmount, @maxSyncId
								FROM Bestellungen b 
								JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr 
								WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND YEAR(b.Datum)=@year AND ISNULL(p.anzahl,0)>0 /* exclude fully booked BE (w/ WE) for currYear */
								GROUP BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								ORDER BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								/* --GO */

								/* -- UNPLACED ORDERS -- */
								SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[PrsSuppliers];
								INSERT INTO [stats].[PrsSupplierUnplacedOrders](
								SupplierAddressNr, 
								[LagerId],
								[BeYear],
								BeKw,
								BeCount, 
								BeArticleCount,
								BeArticleAmount,
								SyncId
								)
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, Year(b.Datum) [BeYear], DATEPART(ISO_WEEK, b.Datum) BeKw, 
								COUNT(DISTINCT b.Nr) BeCount, SUM(ISNULL(p.[Anzahl],0)) BeArticleCount, 
								SUM(ISNULL(p.[Anzahl],0)*ISNULL(p.Einzelpreis,0)) BeArticleAmount, @maxSyncId
								FROM Bestellungen b 
								JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr 
								WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND YEAR(b.Datum)=@year AND b.Nr NOT IN (SELECT DISTINCT OrderId FROM [__PRS_OrderPlacementHistory])
								GROUP BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								ORDER BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								/* --GO */

								/* -- UNCONFIRMED ORDERS -- */
								SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[PrsSuppliers];
								INSERT INTO [stats].[PrsSupplierUnconfirmedDelivery](
								SupplierAddressNr, 
								[LagerId],
								[BeYear],
								BeKw,
								BeCount, 
								BeArticleCount,
								BeArticleAmount,
								SyncId
								)
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, Year(b.Datum) [BeYear], DATEPART(ISO_WEEK, b.Datum) BeKw, 
								COUNT(DISTINCT b.Nr) BeCount, SUM(ISNULL(p.[Anzahl],0)) BeArticleCount, 
								SUM(ISNULL(p.[Anzahl],0)*ISNULL(p.Einzelpreis,0)) BeArticleAmount, @maxSyncId
								FROM Bestellungen b 
								JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr 
								WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND ISNULL(p.Bestätigter_Termin,CAST(CONCAT(YEAR(GETDATE())+100,'01','01') AS DATETIME)) > CAST(CONCAT(YEAR(GETDATE())+5,'01','01') AS DATETIME)AND YEAR(b.Datum)=@year
								GROUP BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								ORDER BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								/* --GO */

								/* -- DELIVERY OVERDUE ORDERS -- */ /* orders with open qty and delivery date in the past */
								SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[PrsSuppliers];
								INSERT INTO [stats].[PrsSupplierDeliveryOverdue](
								SupplierAddressNr, 
								[LagerId],
								[BeYear],
								BeKw,
								BeCount, 
								BeArticleCount,
								BeArticleAmount,
								SyncId
								)
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, Year(b.Datum) [BeYear], DATEPART(ISO_WEEK, b.Datum) BeKw,COUNT(DISTINCT b.Nr) BeCount, 
								SUM(ISNULL(p.[Anzahl],0)) BeArticleCount, SUM(ISNULL(p.[Anzahl],0)*ISNULL(p.Einzelpreis,0)) BeArticleAmount,@maxSyncId
								FROM Bestellungen b 
									JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr 
									WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND ISNULL(erledigt,0)=0 AND ISNULL(p.erledigt_pos,0)=0 
									AND COALESCE(p.Bestätigter_Termin,p.Liefertermin,GETDATE())<GETDATE() AND p.Anzahl>0 AND YEAR(b.Datum)=@year
								GROUP BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								ORDER BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								/* --GO */

								/* -- NEXT 4Kw DELIVERY ORDERS -- */
								SELECT @maxSyncId=ISNULL(MAX(ISNULL(SyncId,0)),0) FROM [stats].[PrsSuppliers];
								INSERT INTO [stats].[PrsSupplierNext4KwDelivery](
								SupplierAddressNr, 
								[LagerId],
								[BeYear],
								BeKw,
								BeCount, 
								BeArticleCount,
								BeArticleAmount,
								SyncId
								)
								SELECT [Lieferanten-Nr] AS SupplierAddressNr, p.Lagerort_id, Year(b.Datum) [BeYear], DATEPART(ISO_WEEK, b.Datum) BeKw,COUNT(DISTINCT b.Nr) BeCount, 
								SUM(ISNULL(p.[Anzahl],0)) BeArticleCount, SUM(ISNULL(p.[Anzahl],0)*ISNULL(p.Einzelpreis,0)) BeArticleAmount, @maxSyncId
								FROM Bestellungen b 
									JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr 
									WHERE b.Typ='Bestellung' AND ISNULL(b.gebucht,0)=1 AND ISNULL(erledigt,0)=0 AND ISNULL(p.erledigt_pos,0)=0 
									AND GETDATE()<=COALESCE(p.Bestätigter_Termin,p.Liefertermin,GETDATE()+365) AND COALESCE(p.Bestätigter_Termin,p.Liefertermin,GETDATE()+365)<=GETDATE()+28 AND p.Anzahl>0 AND YEAR(b.Datum)=@year
								GROUP BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id
								ORDER BY [Lieferanten-Nr], Year(b.Datum), DATEPART(ISO_WEEK, b.Datum), p.Lagerort_id

								SELECT TOP 1 SyncId, SyncDate FROM [stats].[PrsSuppliers] WHERE SyncId=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers]);";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;
					sqlCommand.Parameters.AddWithValue("year", DateTime.Today.Year);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return new KeyValuePair<int, DateTime>(int.TryParse(dataTable.Rows[0][0].ToString(), out var i) ? i : 0, DateTime.TryParse(dataTable.Rows[0][1].ToString(), out var j) ? j : DateTime.MinValue);
				}
				else
				{
					return new KeyValuePair<int, DateTime>(0, DateTime.MinValue);
				}
			}
			public static List<PrsSuppliersEntity> GetSupplierOverview()
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSuppliers] WHERE SyncId=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers])";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSuppliersEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static PrsSuppliersEntity GetSupplierOverview(int? supplierAddressNr, int? year = null)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT TOP 1 * FROM [stats].[PrsSuppliers] {(supplierAddressNr.HasValue ? $"WHERE [SupplierAddressNr]={supplierAddressNr}" : "")} ORDER BY Id DESC";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return new PrsSuppliersEntity(dataTable.Rows[0]);
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierTotalOrders(int? addressNr, int maxSyncId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierTotalOrders] WHERE [BeYear]=YEAR(GETDATE()) AND {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}SyncId={maxSyncId}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierClosedOrders(int? addressNr, int maxSyncId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierClosedOrders] WHERE [BeYear]=YEAR(GETDATE()) AND {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}SyncId={maxSyncId}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierDelayedDelivery(int? addressNr, int maxSyncId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierDelayedDelivery] WHERE [BeYear]=YEAR(GETDATE()) AND {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}SyncId={maxSyncId}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierOpenOrders(int? addressNr, int maxSyncId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierOpenOrders] WHERE [BeYear]=YEAR(GETDATE()) AND {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}SyncId={maxSyncId}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierUnplacedOrders(int? addressNr, int maxSyncId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierUnplacedOrders] WHERE [BeYear]=YEAR(GETDATE()) AND {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}SyncId={maxSyncId}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierUnconfirmedDelivery(int? addressNr, int maxSyncId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierUnconfirmedDelivery] WHERE [BeYear]=YEAR(GETDATE()) AND {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}SyncId={maxSyncId}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierDeliveryOverdue(int? addressNr, int maxSyncId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierDeliveryOverdue] WHERE [BeYear]=YEAR(GETDATE()) AND {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}SyncId={maxSyncId}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierNext4KwDelivery(int? addressNr, int maxSyncId)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierNext4KwDelivery] WHERE [BeYear]=YEAR(GETDATE()) AND {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}SyncId={maxSyncId}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierTotalOrdersByYear(int? addressNr, int year)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierTotalOrders] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year} AND [SyncId]=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers] WHERE [BeYear]={year})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierClosedOrdersByYear(int? addressNr, int year)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierClosedOrders] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year} AND [SyncId]=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers] WHERE [BeYear]={year})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierDelayedDeliveryByYear(int? addressNr, int year)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierDelayedDelivery] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year} AND [SyncId]=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierOpenOrdersByYear(int? addressNr, int year)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierOpenOrders] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year} AND [SyncId]=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierUnplacedOrdersByYear(int? addressNr, int year)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierUnplacedOrders] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year} AND [SyncId]=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierUnconfirmedDeliveryByYear(int? addressNr, int year)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierUnconfirmedDelivery] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year} AND [SyncId]=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierDeliveryOverdueByYear(int? addressNr, int year)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierDeliveryOverdue] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year} AND [SyncId]=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierOrderEntity> GetSupplierNext4KwDeliveryByYear(int? addressNr, int year)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT * FROM [stats].[PrsSupplierNext4KwDelivery] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year} AND [SyncId]=(SELECT MAX(SyncId) FROM [stats].[PrsSuppliers] WHERE {(addressNr.HasValue ? $"[SupplierAddressNr]={addressNr} AND " : "")}[BeYear]={year})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static List<PrsSupplierArticleEntity> GetSupplierActiveArticles(int? addressNr, bool? isStandard, bool? isActive, string queryTerm, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				sortColumn = string.IsNullOrWhiteSpace(sortColumn) ? "Artikelnummer" : sortColumn;
				List<string> whereClause = null;
				if(addressNr.HasValue || isStandard.HasValue || isActive.HasValue || !string.IsNullOrWhiteSpace(queryTerm))
				{
					whereClause = new List<string>();
					if(addressNr.HasValue)
					{
						whereClause.Add($"b.[Lieferanten-Nr]={addressNr}");
					}
					if(isStandard.HasValue)
					{
						whereClause.Add($"b.[Standardlieferant]={(isStandard.Value ? 1 : 0)}");
					}
					if(isActive.HasValue)
					{
						whereClause.Add($"a.[Aktiv]={(isActive.Value ? 1 : 0)}");
					}
					if(!string.IsNullOrWhiteSpace(queryTerm))
					{
						whereClause.Add($"(a.Artikelnummer LIKE '{queryTerm}%' OR a.[Bezeichnung 1] LIKE '{queryTerm}%' OR a.[Bezeichnung 2] LIKE '{queryTerm}%' OR b.[Bestell-Nr] LIKE '{queryTerm}%' OR d.Name1 LIKE '{queryTerm}%' OR d.Name2 LIKE '{queryTerm}%' OR d.Lieferantennummer LIKE '{queryTerm}%')");
					}
				}
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
							SELECT l.Nr AS SupplierId, a.[Artikel-Nr], a.Artikelnummer, a.[Bezeichnung 1], a.[Bezeichnung 2], a.aktiv, b.Standardlieferant, b.[Bestell-Nr], b.Einkaufspreis, b.Wiederbeschaffungszeitraum, b.Mindestbestellmenge, d.Nr AS Address_Nr, d.Name1, d.Name2, d.Lieferantennummer 
							FROM Artikel a join Bestellnummern b on b.[Artikel-Nr]=a.[Artikel-Nr] 
							LEFT JOIN adressen d on d.Nr=b.[Lieferanten-Nr] LEFT JOIN Lieferanten l on l.Nummer=d.Nr 
							{(whereClause?.Count > 0 ? $"WHERE {string.Join(" AND ", whereClause)}" : "")}";
					query += $@" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierArticleEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetSupplierActiveArticles_Count(int? addressNr, bool? isStandard, bool? isActive, string queryTerm)
			{
				List<string> whereClause = null;
				if(addressNr.HasValue || isStandard.HasValue || isActive.HasValue || !string.IsNullOrWhiteSpace(queryTerm))
				{
					whereClause = new List<string>();
					if(addressNr.HasValue)
					{
						whereClause.Add($"b.[Lieferanten-Nr]={addressNr}");
					}
					if(isStandard.HasValue)
					{
						whereClause.Add($"b.[Standardlieferant]={(isStandard.Value ? 1 : 0)}");
					}
					if(isActive.HasValue)
					{
						whereClause.Add($"a.[Aktiv]={(isActive.Value ? 1 : 0)}");
					}
					if(!string.IsNullOrWhiteSpace(queryTerm))
					{
						whereClause.Add($"(a.Artikelnummer LIKE '{queryTerm}%' OR a.[Bezeichnung 1] LIKE '{queryTerm}%' OR a.[Bezeichnung 2] LIKE '{queryTerm}%' OR b.[Bestell-Nr] LIKE '{queryTerm}%' OR d.Name1 LIKE '{queryTerm}%' OR d.Name2 LIKE '{queryTerm}%' OR d.Lieferantennummer LIKE '{queryTerm}%' )");
					}
				}
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"
							SELECT COUNT(*)
							FROM Artikel a JOIN Bestellnummern b on b.[Artikel-Nr]=a.[Artikel-Nr] 
							LEFT JOIN adressen d on d.Nr=b.[Lieferanten-Nr] LEFT JOIN Lieferanten l on l.Nummer=d.Nr 
							{(whereClause?.Count > 0 ? $"WHERE {string.Join(" AND ", whereClause)}" : "")}";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.CommandTimeout = 300;

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
				}
			}
			public static List<PrsSupplierOrderHistoryEntity> GetSupplierHistoryByYear(int? addressNr, int year, int? lagerId, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				sortColumn = string.IsNullOrWhiteSpace(sortColumn) ? "b.Datum" : sortColumn;
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT p.Position,b.[Bestellung-Nr], b.Datum, b.[Vorname/NameFirma] AS Lieferant, p.[Start Anzahl] Anzahl, p.Einzelpreis, p.Einzelpreis*p.[Start Anzahl] AS Gesamtpreis,p.Lagerort_id AS LagerId, a.Artikelnummer
						,l.Lagerort AS Lager, b.Nr AS OrderId, p.[Artikel-Nr] AS ArticleId, s.[Nr] AS SupplierId
						FROM (SELECT Nr,[Bestellung-Nr],Datum, [Vorname/NameFirma],[Lieferanten-Nr] FROM [dbo].[Bestellungen] WHERE Typ='bestellung' AND {(addressNr.HasValue ? $"[Lieferanten-Nr]={addressNr} AND " : "")}YEAR([Datum])={year}) AS b 
						JOIN [bestellte Artikel] AS p on p.[Bestellung-Nr]=b.Nr LEFT Join Artikel a on a.[Artikel-Nr]=p.[Artikel-Nr] LEFT JOIN Lagerorte l ON l.Lagerort_id=p.Lagerort_id LEFT JOIN Lieferanten s on s.nummer=b.[Lieferanten-Nr]{(lagerId.HasValue && lagerId > 0 ? $" WHERE p.Lagerort_id={lagerId}" : "")}";
					query += $@" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderHistoryEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetSupplierHistoryByYear_Count(int? addressNr, int year, int? lagerId)
			{
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT COUNT(*) FROM (SELECT Nr,[Bestellung-Nr],Datum, [Vorname/NameFirma],[Lieferanten-Nr] FROM [dbo].[Bestellungen] WHERE Typ='bestellung' AND {(addressNr.HasValue ? $"[Lieferanten-Nr]={addressNr} AND " : "")}YEAR([Datum])={year}) AS b 
						JOIN [bestellte Artikel] AS p on p.[Bestellung-Nr]=b.Nr LEFT Join Artikel a on a.[Artikel-Nr]=p.[Artikel-Nr] LEFT JOIN Lagerorte l ON l.Lagerort_id=p.Lagerort_id LEFT JOIN Lieferanten s on s.nummer=b.[Lieferanten-Nr]{(lagerId.HasValue && lagerId > 0 ? $" WHERE p.Lagerort_id={lagerId}" : "")}";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
				}
			}
			public static List<PrsSupplierOrderHistoryEntity> GetSupplierSummaryByYear(int? addressNr, int year, int? lagerId, string queryTerm, SupplierOrderStatType orderType, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100)
			{
				if(pageSize == 0)
				{
					pageSize = 1;
				}
				sortColumn = string.IsNullOrWhiteSpace(sortColumn) ? "b.Datum" : sortColumn;
				var dataTable = new DataTable();
				List<string> whereClause = null;
				if(addressNr.HasValue || lagerId.HasValue || !string.IsNullOrWhiteSpace(queryTerm))
				{
					whereClause = new List<string>();
					if(lagerId.HasValue && lagerId > 0)
					{
						whereClause.Add($"p.Lagerort_id={lagerId}");
					}
					if(addressNr.HasValue)
					{
						whereClause.Add($"b.[Lieferanten-Nr]={addressNr}");
					}
					if(!string.IsNullOrWhiteSpace(queryTerm))
					{
						whereClause.Add($"(a.Artikelnummer LIKE '{queryTerm}%' OR a.[Bezeichnung 1] LIKE '{queryTerm}%' OR a.[Bezeichnung 2] LIKE '{queryTerm}%' OR  b.[Vorname/NameFirma] LIKE '{queryTerm}%' OR l.Lagerort like '%{queryTerm}%' OR b.[Bestellung-Nr]LIKE '{queryTerm}%')");
					}
				}
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"DECLARE @year INT = {year}; 
						SELECT p.Position,b.[Bestellung-Nr], b.Datum, b.[Vorname/NameFirma] AS Lieferant, p.[Start Anzahl] Anzahl, p.Einzelpreis, p.Einzelpreis*p.[Start Anzahl] AS Gesamtpreis,p.Lagerort_id AS LagerId, a.Artikelnummer
						,l.Lagerort AS Lager, b.Nr AS OrderId, p.[Artikel-Nr] AS ArticleId, s.[Nr] AS SupplierId
						FROM (SELECT Nr,[Bestellung-Nr],Datum, [Vorname/NameFirma],[Lieferanten-Nr],gebucht,erledigt,Typ,[Projekt-Nr] FROM [dbo].[Bestellungen] WHERE Typ='bestellung'{(addressNr.HasValue ? $" AND [Lieferanten-Nr]={addressNr}" : "")}{((orderType == SupplierOrderStatType.Total || orderType == SupplierOrderStatType.Closed) ? "" : " AND YEAR([Datum])=@year")}) AS b 
						JOIN [bestellte Artikel] AS p on p.[Bestellung-Nr]=b.Nr
						LEFT Join Artikel a on a.[Artikel-Nr]=p.[Artikel-Nr] LEFT JOIN Lagerorte l ON l.Lagerort_id=p.Lagerort_id LEFT JOIN Lieferanten s on s.nummer=b.[Lieferanten-Nr]
						{SupplierOrderStatTypeDic.GetValueOrDefault(orderType)} 
						{(whereClause?.Count > 0 ? $"AND {string.Join(" AND ", whereClause)}" : "")}";
					query += $@" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PrsSupplierOrderHistoryEntity(x)).ToList();
				}
				else
				{
					return null;
				}
			}
			public static int GetSupplierSummaryByYear_Count(int? addressNr, int year, int? lagerId, string queryTerm, SupplierOrderStatType orderType)
			{
				List<string> whereClause = null;
				if(addressNr.HasValue || lagerId.HasValue || !string.IsNullOrWhiteSpace(queryTerm))
				{
					whereClause = new List<string>();
					if(lagerId.HasValue && lagerId > 0)
					{
						whereClause.Add($"p.Lagerort_id={lagerId}");
					}
					if(addressNr.HasValue)
					{
						whereClause.Add($"b.[Lieferanten-Nr]={addressNr}");
					}
					if(!string.IsNullOrWhiteSpace(queryTerm))
					{
						whereClause.Add($"(a.Artikelnummer LIKE '{queryTerm}%' OR a.[Bezeichnung 1] LIKE '{queryTerm}%' OR a.[Bezeichnung 2] LIKE '{queryTerm}%' OR  b.[Vorname/NameFirma] LIKE '{queryTerm}%' OR l.Lagerort like '%{queryTerm}%' OR b.[Bestellung-Nr]LIKE '{queryTerm}%')");
					}
				}
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"DECLARE @year INT = {year}; 
						SELECT COUNT(*) FROM 
						(SELECT Nr,[Bestellung-Nr],Datum, [Vorname/NameFirma],[Lieferanten-Nr],gebucht,erledigt,Typ,[Projekt-Nr] FROM [dbo].[Bestellungen] WHERE Typ='bestellung'{(addressNr.HasValue ? $" AND[Lieferanten-Nr]={addressNr}" : "")}{((orderType == SupplierOrderStatType.Total || orderType == SupplierOrderStatType.Closed) ? "" : " AND YEAR([Datum])=@year")}) AS b 
						JOIN [bestellte Artikel] AS p on p.[Bestellung-Nr]=b.Nr
						LEFT Join Artikel a on a.[Artikel-Nr]=p.[Artikel-Nr] LEFT JOIN Lagerorte l ON l.Lagerort_id=p.Lagerort_id LEFT JOIN Lieferanten s on s.nummer=b.[Lieferanten-Nr]
						{SupplierOrderStatTypeDic.GetValueOrDefault(orderType)} 
						{(whereClause?.Count > 0 ? $"AND {string.Join(" AND ", whereClause)}" : "")}";

					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.CommandTimeout = 300;

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
				}
			}
		}
	}
}