using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.CRP
{
	public class ArticleAccess
	{
		#region MTM - CRP
		public static List<Entities.Joins.MTM.CRP.ArticleWpl> GetArticlesWoWPL(bool? warengruppeEF = null, bool? wStuckliste = null, bool? wFa = null, bool? wOpenFa = null, int? lager = null, DateTime? faDateFrom = null, DateTime? faDateTill = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//string query = "SELECT [Artikel-Nr], Artikelnummer,[Bezeichnung 1],Warengruppe,Produktionszeit,Freigabestatus,[Freigabestatus TN intern],[Prüfstatus TN Ware],Stundensatz,Losgroesse FROM Artikel WHERE Freigabestatus <> 'o' AND Artikelnummer IN (SELECT [Name] FROM Article WHERE Id NOT IN (SELECT Article_Id FROM WorkSchedule WHERE Is_Active=1))";
				//Rami 03/09/2025
				string query = "SELECT [Artikel-Nr], Artikelnummer,[Bezeichnung 1],Warengruppe,Produktionszeit,Freigabestatus,[Freigabestatus TN intern],[Prüfstatus TN Ware],Stundensatz,Losgroesse FROM Artikel  a WHERE a.Freigabestatus <> 'o'  AND a.[Artikel-Nr] NOT IN (SELECT ws.Article_Id FROM WorkSchedule ws  WHERE ws.Is_Active = 1)";

				// - w/ warengruppe EF
				if(warengruppeEF.HasValue)
					query += $" AND Warengruppe = 'EF' ";

				// - w/ Stuckliste
				if(wStuckliste.HasValue)
					query += $" AND Stückliste = 1 ";

				// - w/ at least FA
				if(wFa.HasValue)
					query += $" AND [Artikel-Nr] NOT IN ( /* Articles w/o FA */ SELECT DISTINCT A.[Artikel-Nr] FROM dbo.Artikel A FULL OUTER JOIN Fertigung F ON F.Artikel_Nr=A.[Artikel-Nr] WHERE F.ID IS NULL And A.Freigabestatus<> 'O') ";

				// - w/ FA lager
				if(wFa.HasValue)
				{
					if(lager.HasValue || wOpenFa.HasValue)
					{
						if(lager.HasValue && wOpenFa.HasValue)
						{
							query += $" AND [Artikel-Nr] IN (SELECT [Artikel_Nr] FROM Fertigung WHERE Kennzeichen='offen' AND Lagerort_id={lager.Value} ) ";
						}
						else if(lager.HasValue)
						{
							query += $" AND [Artikel-Nr] IN (SELECT [Artikel_Nr] FROM Fertigung WHERE Lagerort_id={lager.Value} ) ";
						}
						else
						{
							query += $" AND [Artikel-Nr] IN (SELECT [Artikel_Nr] FROM Fertigung WHERE Kennzeichen='offen' ) ";
						}
					}
				}

				// - w/o FaDate
				string query_cte = $@"WITH query_CTE AS ({query}) SELECT * FROM query_CTE";

				// - w/ faDate
				if(wFa.HasValue)
				{
					if(faDateFrom.HasValue || faDateTill.HasValue)
					{
						if(faDateFrom.HasValue && faDateTill.HasValue)
						{
							query_cte += $@" WHERE [Artikel-Nr] IN (SELECT DISTINCT Artikel_Nr FROM Fertigung WHERE '{faDateFrom.Value.ToString("yyyyMMdd")}'<=[Termin_Bestätigt1] AND [Termin_Bestätigt1]<='{faDateTill.Value.ToString("yyyyMMdd")}')";
						}
						else if(faDateFrom.HasValue)
						{
							query_cte += $@" WHERE [Artikel-Nr] IN (SELECT DISTINCT Artikel_Nr FROM Fertigung WHERE '{faDateFrom.Value.ToString("yyyyMMdd")}'<=[Termin_Bestätigt1])";
						}
						else
						{
							query_cte += $@" WHERE [Artikel-Nr] IN (SELECT DISTINCT Artikel_Nr FROM Fertigung WHERE [Termin_Bestätigt1]<='{faDateTill.Value.ToString("yyyyMMdd")}')";
						}
					}
				}

				// - 
				var sqlCommand = new SqlCommand(query_cte, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.CRP.ArticleWpl(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.CRP.ArticleWpl>();
			}
		}
		public static List<Entities.Joins.MTM.CRP.ArticleEntity> GetArticleWPLTimeDiff(int? countryId = null, int? hallId = null, decimal? minDiff = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string filterQuery = "";
				if(countryId.HasValue)
					filterQuery += $"C.id = {countryId.Value} ";

				if(hallId.HasValue)
					if(filterQuery.Length > 0)
						filterQuery += $"AND H.id = {hallId.Value} ";
					else
						filterQuery += $"H.id = {hallId.Value} ";

				if(minDiff.HasValue)
					if(filterQuery.Length > 0)
						filterQuery += $"AND {Math.Abs(minDiff.Value)} <= ABS(AV.ArticleOperationTime_losgrosse - A.Produktionszeit)";
					else
						filterQuery += $"{Math.Abs(minDiff.Value)} <= ABS(AV.ArticleOperationTime_losgrosse - A.Produktionszeit)";

				sqlConnection.Open();


				//				string query = $@"IF OBJECT_ID('tempdb..#FA_AVG_Time') IS NOT NULL DROP TABLE #FA_AVG_Time;
				//                                    IF OBJECT_ID('tempdb..#AP_ValueAddingTime') IS NOT NULL DROP TABLE #AP_ValueAddingTime;

				//                                    /* Q1 - Average 5 FA Time */
				//                                    SELECT Artikelnummer,A.Losgroesse, y.* 
				//                                    INTO #FA_AVG_Time
				//                                    FROM Artikel A
				//                                    JOIN (
				//                                    SELECT Artikel_Nr, AVG(Originalanzahl) total
				//                                    FROM (
				//                                        SELECT Artikel_Nr, Originalanzahl, ROW_NUMBER() OVER(PARTITION BY Artikel_Nr ORDER BY Termin_Bestätigt1 DESC) rn
				//                                        FROM dbo.Fertigung) x 
				//                                    WHERE rn <= 5
				//                                    GROUP BY Artikel_Nr
				//                                    ) y
				//                                    ON A.[Artikel-Nr]=y.Artikel_Nr
				//                                    ORDER BY Artikelnummer


				//                                    /* Q2 - AP OperationTime Total */
				//                                    SELECT A.[Artikel-Nr],
				//                                    SUM(
				//                                          CASE WHEN RelationOperationTime=1 
				//                                            THEN  Amount * OperationTimeSeconds / 60  + SetupTimeMinutes/COALESCE(Wd.LotSizeSTD,1) 
				//                                            ELSE  Amount * OperationTimeSeconds / 60 /COALESCE(Wd.LotSizeSTD,1)  + SetupTimeMinutes/COALESCE(Wd.LotSizeSTD,1) END)
				//                                           ArticleOperationTime,
				//                                    SUM(
				//                                          CASE WHEN RelationOperationTime=1 THEN 
				//                                            Amount * OperationTimeSeconds / 60  + SetupTimeMinutes/COALESCE(A.Losgroesse,1)
				//                                        ELSE
				//                                            Amount * OperationTimeSeconds / 60 /COALESCE(A.Losgroesse,1)  + SetupTimeMinutes/COALESCE(A.Losgroesse,1) END
				//                                           ) AS
				//                                           ArticleOperationTime_losgrosse,
				//                                    SUM(
				//                                           CASE 
				//                                        WHEN RelationOperationTime=1 THEN 
				//                                            Amount * OperationTimeSeconds / 60  + SetupTimeMinutes/COALESCE(FA_AVG.total,1)
				//                                        ELSE
				//                                            Amount * OperationTimeSeconds / 60 /COALESCE(FA_AVG.total,1)  + SetupTimeMinutes/COALESCE(FA_AVG.total,1)
				//                                    END
				//                                           )
				//                                           ArticleOperationTime_RealLosgrosse
				//                                           INTO #AP_ValueAddingTime
				//                                           FROM Artikel A
				//                                           JOIN #FA_AVG_Time FA_AVG ON FA_AVG.Artikel_Nr=A.[Artikel-Nr]
				//                                           JOIN dbo.Article AA ON AA.[Name]=A.Artikelnummer
				//                                           JOIN dbo.WorkSchedule Ws ON Ws.Article_Id=AA.Id
				//                                           JOIN dbo.WorkScheduleDetails Wd ON Wd.WorkScheduleId=Ws.Id
				//                                    WHERE Ws.Is_Active=1
				//                                    GROUP BY [Artikel-Nr];



				//                                    SELECT A.Artikelnummer, A.Freigabestatus [Status_Extern], A.[Freigabestatus TN intern] [Status_Intern], 
				//                                                C.Name Country, H.Name Hall
				//                                                ,A.Produktionszeit [P3000_Vorgabezeit_min]
				//                                                ,A.Losgroesse [P3000 losgrosse]
				//                                                , COALESCE(FT.total, 0) [Real_Losgrosse_der_letzten_5_FA]
				//                                                --,replace(AV.ArticleOperationTime, ',', '.') [Wertschöpfende Zeit laut AP in min]
				//                                                , T.ArticleOperationTime  [Total_Operation_Time_laut_AP_pro_Stuck_in_min]
				//                                                , AV.ArticleOperationTime_losgrosse  [Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min]
				//                                                , AV.ArticleOperationTime_RealLosgrosse  [Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min]
				//                                                -- replace(A.Produktionszeit - T.ArticleOperationTime, ',', '.') Diff_A_WPL   
				//                                                ,AV.ArticleOperationTime_losgrosse - A.Produktionszeit [Differenz_Time_pro_Losgrosse_P3000_vs_Prod]   
				//                                                ,AV.ArticleOperationTime_RealLosgrosse - A.Produktionszeit [Differenz_Time_pro_Losgrosse_in_FA_vs_Prod]   
				//                                        FROM Artikel A 
				//                                        LEFT JOIN #FA_AVG_Time FT ON FT.Artikel_Nr=A.[Artikel-Nr]
				//                                        LEFT JOIN #AP_ValueAddingTime AV ON AV.[Artikel-Nr]=A.[Artikel-Nr]
				//                                        JOIN Article AA ON AA.[name]=A.Artikelnummer
				//                                        JOIN (SELECT A.[Artikel-Nr], MAX(Ws.Id) WId,
				//                                            SUM(
				//                                                   CASE 
				//                                                   WHEN Wd.RelationOperationTime=1 THEN 
				//                                                         Wd.Amount * Wd.OperationTimeSeconds / 60  + Wd.SetupTimeMinutes/1
				//                                                   ELSE
				//                                                         Wd.Amount * Wd.OperationTimeSeconds / 60 /1  + Wd.SetupTimeMinutes/1
				//                                                   END
				//                                                   ) 
				//                                                   ArticleOperationTime /* in Minutes */
				//                                                   FROM Artikel A
				//                                                   JOIN dbo.Article AA ON AA.[Name]=A.Artikelnummer
				//                                                   LEFT JOIN dbo.WorkSchedule Ws ON Ws.Article_Id=AA.Id
				//                                                   LEFT JOIN dbo.WorkScheduleDetails Wd ON Wd.WorkScheduleId=Ws.Id
				//                                            WHERE Ws.Is_Active=1 
				//                                            GROUP BY [Artikel-Nr]) T ON T.[Artikel-Nr]=A.[Artikel-Nr]
				//                                         JOIN WorkSchedule W ON W.Id=T.WId
				//                                         LEFT JOIN Hall H ON H.Id=W.Hall_Id
				//                                         LEFT JOIN Countries C ON C.Id=H.Country_Id
				//"


				//rami 03/09 -instead of Article joinning, direct join of WorkSchedule w Artikel
				string query = $@"IF OBJECT_ID('tempdb..#FA_AVG_Time') IS NOT NULL DROP TABLE #FA_AVG_Time;
IF OBJECT_ID('tempdb..#AP_ValueAddingTime') IS NOT NULL DROP TABLE #AP_ValueAddingTime;

/* Q1 - Average 5 FA Time */
SELECT 
    Artikelnummer,
    A.Losgroesse, 
    y.* 
INTO #FA_AVG_Time
FROM Artikel A
JOIN (
    SELECT Artikel_Nr, AVG(Originalanzahl) total
    FROM (
        SELECT Artikel_Nr, Originalanzahl, 
               ROW_NUMBER() OVER(PARTITION BY Artikel_Nr ORDER BY Termin_Bestätigt1 DESC) rn
        FROM dbo.Fertigung
    ) x 
    WHERE rn <= 5
    GROUP BY Artikel_Nr
) y
ON A.[Artikel-Nr]=y.Artikel_Nr
ORDER BY Artikelnummer;

/* Q2 - AP OperationTime Total */
SELECT 
    A.[Artikel-Nr],
    SUM(
        CASE WHEN RelationOperationTime=1 
             THEN Amount * OperationTimeSeconds / 60 + SetupTimeMinutes/COALESCE(Wd.LotSizeSTD,1) 
             ELSE Amount * OperationTimeSeconds / 60 / COALESCE(Wd.LotSizeSTD,1) + SetupTimeMinutes/COALESCE(Wd.LotSizeSTD,1) 
        END
    ) AS ArticleOperationTime,
    SUM(
        CASE WHEN RelationOperationTime=1 
             THEN Amount * OperationTimeSeconds / 60 + SetupTimeMinutes/COALESCE(A.Losgroesse,1)
             ELSE Amount * OperationTimeSeconds / 60 / COALESCE(A.Losgroesse,1) + SetupTimeMinutes/COALESCE(A.Losgroesse,1) 
        END
    ) AS ArticleOperationTime_losgrosse,
    SUM(
        CASE WHEN RelationOperationTime=1 
             THEN Amount * OperationTimeSeconds / 60 + SetupTimeMinutes/COALESCE(FA_AVG.total,1)
             ELSE Amount * OperationTimeSeconds / 60 / COALESCE(FA_AVG.total,1) + SetupTimeMinutes/COALESCE(FA_AVG.total,1) 
        END
    ) AS ArticleOperationTime_RealLosgrosse
INTO #AP_ValueAddingTime
FROM Artikel A
JOIN #FA_AVG_Time FA_AVG ON FA_AVG.Artikel_Nr = A.[Artikel-Nr]
JOIN dbo.WorkSchedule Ws ON Ws.Article_Id = A.[Artikel-Nr]
JOIN dbo.WorkScheduleDetails Wd ON Wd.WorkScheduleId = Ws.Id
WHERE Ws.Is_Active = 1
GROUP BY A.[Artikel-Nr];

/* Final Output */
SELECT 
    A.Artikelnummer, 
    A.Freigabestatus AS [Status_Extern], 
    A.[Freigabestatus TN intern] AS [Status_Intern], 
    C.Name AS Country, 
    H.Name AS Hall,
    A.Produktionszeit AS [P3000_Vorgabezeit_min],
    A.Losgroesse AS [P3000 losgrosse],
    COALESCE(FT.total, 0) AS [Real_Losgrosse_der_letzten_5_FA],
    T.ArticleOperationTime  AS [Total_Operation_Time_laut_AP_pro_Stuck_in_min],
    AV.ArticleOperationTime_losgrosse  AS [Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min],
    AV.ArticleOperationTime_RealLosgrosse  AS [Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min],
    AV.ArticleOperationTime_losgrosse - A.Produktionszeit AS [Differenz_Time_pro_Losgrosse_P3000_vs_Prod],   
    AV.ArticleOperationTime_RealLosgrosse - A.Produktionszeit AS [Differenz_Time_pro_Losgrosse_in_FA_vs_Prod]
FROM Artikel A 
LEFT JOIN #FA_AVG_Time FT ON FT.Artikel_Nr = A.[Artikel-Nr]
LEFT JOIN #AP_ValueAddingTime AV ON AV.[Artikel-Nr] = A.[Artikel-Nr]
JOIN (
    SELECT 
        A.[Artikel-Nr], 
        MAX(Ws.Id) AS WId,
        SUM(
            CASE WHEN Wd.RelationOperationTime=1 
                 THEN Wd.Amount * Wd.OperationTimeSeconds / 60 + Wd.SetupTimeMinutes/1
                 ELSE Wd.Amount * Wd.OperationTimeSeconds / 60 / 1 + Wd.SetupTimeMinutes/1
            END
        ) AS ArticleOperationTime
    FROM Artikel A
    LEFT JOIN dbo.WorkSchedule Ws ON Ws.Article_Id = A.[Artikel-Nr]
    LEFT JOIN dbo.WorkScheduleDetails Wd ON Wd.WorkScheduleId = Ws.Id
    WHERE Ws.Is_Active = 1 
    GROUP BY A.[Artikel-Nr]
) T ON T.[Artikel-Nr] = A.[Artikel-Nr]
JOIN WorkSchedule W ON W.Id = T.WId
LEFT JOIN Hall H ON H.Id = W.Hall_Id
LEFT JOIN Countries C ON C.Id = H.Country_Id
"
							+ $" {(filterQuery.Length > 0 ? $" WHERE {filterQuery}" : "")} "
							+ " ORDER BY C.Id, H.Id, A.Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.CRP.ArticleEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.CRP.ArticleEntity>();
			}
		}
		#endregion MTM - CRP
	}
}
