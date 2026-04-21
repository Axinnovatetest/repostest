using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System;


namespace Infrastructure.Data.Access.Joins.CRP
{
	public class FAPlannungAccess
	{
		public static List<Entities.Joins.CRP.FaultyNeedsEntity> GetFaultyNeeds(string searchTerms, bool ubg, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			searchTerms = searchTerms ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH CTE AS (
    SELECT 
	    Id,
        PSZCustomernumber,
		DocumentNumber,
		ManualCreation,
        ReferenceVersionNumber,
        ROW_NUMBER() OVER (PARTITION BY PSZCustomernumber, DocumentNumber, ManualCreation ORDER BY ReferenceVersionNumber DESC) AS rn
    FROM 
        __EDI_DLF_Header WHERE ISNULL(Done,0)=0
)
SELECT * FROM (
SELECT DISTINCT A.Nr as Id,A.[Angebot-Nr] AS VorfallNr,'AB' AS [Type], A.[Bezug] AS DocumentNumber, AB.[Liefertermin] as DeliveryDate, AB.[Anzahl] AS Quantity, [Kunden-Nr] AS CustomerNumber, NULL as IsManual
FROM [angebotene Artikel] AB INNER JOIN [Angebote] A
ON AB.[Angebot-Nr]=A.[Nr]
WHERE A.[Typ]='Auftragsbestätigung'
AND (A.erledigt=0 OR A.erledigt is null)
and A.gebucht=1{(ubg == true ? " AND AB.[Artikel-Nr] IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)" : " AND AB.[Artikel-Nr] NOT IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)")}
AND AB.[Liefertermin]<GETDATE() AND (A.[Bezug] LIKE '{searchTerms.SqlEscape()}%' OR CAST(A.[Angebot-Nr] as NVARCHAR(100)) LIKE '{searchTerms.SqlEscape()}%')

UNION ALL

SELECT 
distinct
  LI.Id,
  '' as vorfallNr,
  'DELFOR' AS [Type], CTE.DocumentNumber,
  LIP.PlanningQuantityRequestedShipmentDate AS DeliveryDate,
  LIP.PlanningQuantityQuantity AS Qunatity,
PSZCustomernumber AS CustomerNumber, ISNULL(ManualCreation,0) as IsManual
FROM 
    CTE
	inner join __EDI_DLF_LineItem LI
	on CTE.Id=LI.HeaderId
	inner join __EDI_DLF_LineItemPlan LIP 
on LIP.LineItemId=LI.Id
WHERE 
    rn = 1{(ubg == true ? " AND LI.[ArticleId] IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)" : " AND LI.[ArticleId] NOT IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)")}
	and LIP.PlanningQuantityRequestedShipmentDate<GETDATE()
	AND (CTE.DocumentNumber LIKE '{searchTerms.SqlEscape()}%')
	) as TMP";
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [DeliveryDate] DESC ";
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.FaultyNeedsEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int GetFaultyNeedsCount(string searchTerms, bool ubg)
		{
			searchTerms = searchTerms ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH CTE AS (
    SELECT 
	    Id,
        PSZCustomernumber,
		DocumentNumber,
		ManualCreation,
        ReferenceVersionNumber,
        ROW_NUMBER() OVER (PARTITION BY PSZCustomernumber, DocumentNumber,ManualCreation ORDER BY ReferenceVersionNumber DESC) AS rn
    FROM 
        __EDI_DLF_Header WHERE ISNULL(Done,0)=0
)
SELECT COUNT(*) FROM (
SELECT DISTINCT A.Nr as Id,A.[Angebot-Nr] AS VorfallNr,'AB' AS [Type], A.[Bezug] AS DocumentNumber, AB.[Liefertermin] as DeliveryDate, AB.[Anzahl] AS Quantity, [Kunden-Nr] AS CustomerNumber, NULL as IsManual
FROM [angebotene Artikel] AB INNER JOIN [Angebote] A
ON AB.[Angebot-Nr]=A.[Nr]
WHERE A.[Typ]='Auftragsbestätigung'
AND (A.erledigt=0 OR A.erledigt is null)
AND A.gebucht=1{(ubg == true ? " AND AB.[Artikel-Nr] IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)" : " AND AB.[Artikel-Nr] NOT IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)")}
AND AB.[Liefertermin]<GETDATE()
AND (A.[Bezug] LIKE '{searchTerms.SqlEscape()}%' OR CAST(A.[Angebot-Nr] as NVARCHAR(100)) LIKE '{searchTerms.SqlEscape()}%')

UNION ALL

SELECT 
DISTINCT
  LI.Id,
  '' as vorfallNr,
  'DELFOR' AS [Type], CTE.DocumentNumber,
  LIP.PlanningQuantityRequestedShipmentDate AS DeliveryDate,
  LIP.PlanningQuantityQuantity AS Qunatity,
PSZCustomernumber AS CustomerNumber, ManualCreation as IsManual
FROM 
    CTE
	INNER JOIN __EDI_DLF_LineItem LI
	on CTE.Id=LI.HeaderId
	INNER JOIN __EDI_DLF_LineItemPlan LIP 
ON LIP.LineItemId=LI.Id
WHERE 
    rn = 1{(ubg == true ? " AND LI.[ArticleId] IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)" : " AND LI.[ArticleId] NOT IN (SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1)")}
	AND LIP.PlanningQuantityRequestedShipmentDate<GETDATE()
	AND (CTE.DocumentNumber LIKE '{searchTerms.SqlEscape()}%')
	) AS TMP";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}
		public static int GetArticleMinimumStock(int articleNr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT SUM([Mindestbestand]) FROM [Lager] WHERE [Artikel-Nr]=@articleNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetFAQuantitiesByArticle(int articleNr, DateTime? start, DateTime end, bool? internSiteProd = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) AS Week,ISNULL(SUM(ISNULL([Anzahl],0)),0) AS Quantity
                               FROM [Fertigung]{(!internSiteProd.HasValue ? "" : $" f JOIN __BSD_ArtikelProductionExtension e on e.ArticleId=f.Artikel_Nr")}
                               WHERE Kennzeichen='offen'{(start.HasValue == false ? "" : " AND @start<=COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)")} 
							   AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)<=@end
							   AND Artikel_Nr=@articleNr{(!internSiteProd.HasValue ? "" : $" AND {(internSiteProd == true ? $"Lagerort_id=e.ProductionPlace1_Id" : "Lagerort_id<>e.ProductionPlace1_Id")}")}
                               GROUP BY DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetABQuantitiesByArticle(int articleNr, DateTime? start, DateTime end)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, AR.Liefertermin) AS Week,
                               SUM(AR.Anzahl) AS Quantity
                               FROM Angebote A INNER JOIN [angebotene Artikel] AR
                               ON A.Nr=AR.[Angebot-Nr]
                               WHERE A.Typ='Auftragsbestätigung' AND A.gebucht=1 AND ISNULL(A.erledigt,0)=0 AND ISNULL(AR.erledigt_pos,0)=0
                               AND AR.[Artikel-Nr]=@articleNr {(start.HasValue == false ? "" : " AND @start<=AR.Liefertermin")}
                               AND AR.Liefertermin <= @end
                               GROUP BY DATEPART(iso_week, AR.Liefertermin)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetLPQuantitiesByArticle(int articleNr, DateTime? start, DateTime end)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH CTE AS (
    SELECT 
	    Id,
        PSZCustomernumber,
		DocumentNumber,
        ReferenceVersionNumber,
        ROW_NUMBER() OVER (PARTITION BY PSZCustomernumber, DocumentNumber ORDER BY ReferenceVersionNumber DESC) AS rn
    FROM 
        __EDI_DLF_Header  WHERE ISNULL(Done,0)=0
		)
SELECT 
  DATEPART(iso_week, PlanningQuantityRequestedShipmentDate) as Week,
  SUM(PlanningQuantityQuantity)-ISNULL(SUM(ISNULL(AbQuantity,0)),0) as Quantity
FROM 
    CTE
	INNER JOIN __EDI_DLF_LineItem LI on CTE.Id=LI.HeaderId
	INNER JOIN __EDI_DLF_LineItemPlan LIP ON LIP.LineItemId=LI.Id
LEFT JOIN (SELECT a.nr_dlf, ISNULL(SUM(ISNULL(b.[OriginalAnzahl],0)),0) AbQuantity FROM Angebote a JOIN [angebotene Artikel] b on b.[Angebot-Nr]=a.Nr WHERE a.Typ='Auftragsbestätigung' AND ISNULL(a.gebucht,0)=1 AND ISNULL(a.erledigt,0)=0 AND ISNULL(b.erledigt_pos,0)=0 GROUP BY nr_dlf) AS o on o.nr_dlf=LIP.Id
WHERE 
    rn = 1
	AND LI.ArticleId=@articleNr
	{(start.HasValue == false ? "" : " AND @start <= PlanningQuantityRequestedShipmentDate")}
	AND PlanningQuantityRequestedShipmentDate <= @end 
	group by 
	DATEPART(iso_week, PlanningQuantityRequestedShipmentDate)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetFCQuantitiesByArticle(int articleNr, DateTime? start, DateTime end)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH CTE AS (
    SELECT 
	    Id,
        kundennummer,
		TypeId,
        [Datum],
        ROW_NUMBER() OVER (PARTITION BY kundennummer, TypeId ORDER BY [Datum] DESC) AS rn
    FROM 
        Forecasts
		)
SELECT 
  DATEPART(iso_week, p.Datum) as Week,
  SUM(Menge) as Quantity
FROM 
    CTE
	INNER JOIN ForecastsPosition P
	on CTE.Id=P.IdForcast
WHERE 
    rn = 1
	AND P.ArtikelNr=@articleNr
	{(start.HasValue == false ? "" : " AND @start <= p.Datum")}
	and p.Datum <= @end 
	and TypeId=0
    and (IsOrdered IS NULL OR IsOrdered=0)
	group by 
	DATEPART(iso_week, p.Datum)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetBedarfByArticle(int articleNr, DateTime? start, DateTime end, int? lager, bool intern = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) AS Week, ISNULL(SUM(ISNULL([Anzahl],0)),0) AS Quantity
FROM [Fertigung]
WHERE Kennzeichen='offen' AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2) BETWEEN @start and @end
AND Artikel_Nr=@articleNr
{(intern ? $"{(lager.HasValue ? $" AND [Lagerort_id]={lager.Value}" : "")}" : $"{(lager.HasValue ? $" AND [Lagerort_id]<>{lager.Value}" : "")}")}
GROUP BY DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static decimal GetFAQuantitiesByArticle_Residue(int articleNr, bool? internSiteProd = null)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT ISNULL(SUM(ISNULL([Anzahl],0)),0)
                               FROM [Fertigung]{(!internSiteProd.HasValue ? "" : $" f JOIN __BSD_ArtikelProductionExtension e on e.ArticleId=f.Artikel_Nr")}
                               where Kennzeichen='offen' AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE) 
                               AND Artikel_Nr=@articleNr{(!internSiteProd.HasValue ? "" : $" AND {(internSiteProd == true ? $"Lagerort_id=e.ProductionPlace1_Id" : "Lagerort_id<>e.ProductionPlace1_Id")}")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static decimal GetFAQuantitiesByArticle_ResidueUBG(int articleNr, bool internSiteProd = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT ISNULL(SUM(ISNULL(fp.[Anzahl],0)),0)
                               FROM [Fertigung] f 
							   INNER JOIN Fertigung_Positionen fp ON fp.ID_Fertigung=F.ID
                               WHERE fp.Artikel_Nr=@articleNr 
							   AND Kennzeichen=N'offen' 							   
							   AND fp.Lagerort_ID {(internSiteProd ? "=" : "<>")}(select ProductionPlace1_Id from __BSD_ArtikelProductionExtension where ArticleId=@articleNr)     
							   AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out decimal d) ? d : 0;
			}
		}
		public static decimal GetABQuantitiesByArticle_Residue(int articleNr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT ISNULL(SUM(ISNULL(AR.[Anzahl],0)),0)
                               FROM Angebote A INNER JOIN [angebotene Artikel] AR
                               ON A.Nr=AR.[Angebot-Nr]
                               wHERE A.Typ='Auftragsbestätigung' AND A.gebucht=1 AND ISNULL(A.erledigt,0)=0 AND ISNULL(AR.erledigt_pos,0)=0
                               AND AR.[Artikel-Nr]=@articleNr
                               AND AR.Liefertermin<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static decimal GetLPQuantitiesByArticle_Residue(int articleNr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"WITH CTE AS (
									SELECT 
										Id,
										PSZCustomernumber,
										DocumentNumber,
										ReferenceVersionNumber,
										ROW_NUMBER() OVER (PARTITION BY PSZCustomernumber, DocumentNumber ORDER BY ReferenceVersionNumber DESC) AS rn
									FROM 
										__EDI_DLF_Header  WHERE ISNULL(Done,0)=0
										)
								SELECT ISNULL(SUM(ISNULL(PlanningQuantityQuantity,0)),0)-ISNULL(SUM(ISNULL(AbQuantity,0)),0) 
								FROM 
									CTE
									INNER JOIN __EDI_DLF_LineItem LI on CTE.Id=LI.HeaderId
									INNER JOIN __EDI_DLF_LineItemPlan LIP ON LIP.LineItemId=LI.Id
									LEFT JOIN (SELECT a.nr_dlf, ISNULL(SUM(ISNULL(b.[OriginalAnzahl],0)),0) AbQuantity FROM Angebote a JOIN [angebotene Artikel] b on b.[Angebot-Nr]=a.Nr WHERE a.Typ='Auftragsbestätigung' AND ISNULL(a.gebucht,0)=1 AND ISNULL(a.erledigt,0)=0 AND ISNULL(b.erledigt_pos,0)=0 GROUP BY nr_dlf) AS o on o.nr_dlf=LIP.Id
								WHERE 
									rn = 1
									AND LI.ArticleId=@articleNr
									and PlanningQuantityRequestedShipmentDate<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static decimal GetFCQuantitiesByArticle_Residue(int articleNr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"WITH CTE AS (
									SELECT 
										Id,
										kundennummer,
										TypeId,
										[Datum],
										ROW_NUMBER() OVER (PARTITION BY kundennummer, TypeId ORDER BY [Datum] DESC) AS rn
									FROM 
										Forecasts
										)
								SELECT ISNULL(SUM(ISNULL(Menge,0)),0) as Quantity
								FROM 
									CTE
									INNER JOIN ForecastsPosition P
									on CTE.Id=P.IdForcast
								WHERE 
									rn = 1
									AND P.ArtikelNr=@articleNr
                                    and TypeId=0
                                    and (IsOrdered IS NULL OR IsOrdered=0)
									and p.Datum<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static decimal GetBedarfByArticle_Residue(int articleNr, int? lager, bool intern = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT ISNULL(SUM(ISNULL([Anzahl],0)),0) AS Quantity
									FROM [Fertigung]
									WHERE Kennzeichen='offen' AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)
									AND Artikel_Nr=@articleNr
									{(intern ? $"{(lager.HasValue ? $" AND [Lagerort_id]={lager.Value}" : "")}" : $"{(lager.HasValue ? $" AND [Lagerort_id]<>{lager.Value}" : "")}")}
									GROUP BY DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static List<KeyValuePair<int, string>> GetArticlesUBGStucklist(int articleNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select DISTINCT S.[Artikelnummer], S.[Artikel-Nr des Bauteils] AS [Artikel-Nr] FROM [Stücklisten] S INNER JOIN [Artikel] A
                               ON S.[Artikel-Nr des Bauteils]=A.[Artikel-Nr]
                               WHERE S.[Artikel-Nr]=@articleNr
                               AND A.[UBG]=1";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x["Artikel-Nr"].ToString(), out var i) ? i : 0, Convert.ToString(x["Artikelnummer"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Joins.CRP.FAPlannungArticlesEntity_3> GetArticlesByNummerkreis(string kreisClause, string artikelnummer, int? deficit, DateTime? start, DateTime end, int? unit, bool ubg = false, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			kreisClause = kreisClause ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"IF OBJECT_ID('tempdb..#filterArts') IS NOT NULL DROP TABLE #filterArts;
				CREATE TABLE #filterArts ([Artikel-Nr] int);
				IF OBJECT_ID('tempdb..#Prod') IS NOT NULL DROP TABLE #Prod;
				CREATE TABLE #Prod ([Artikel-Nr] int, Qty decimal(20,7), [Year] INT, [Kw] INT);
				IF OBJECT_ID('tempdb..#Needs') IS NOT NULL DROP TABLE #Needs;
				CREATE TABLE #Needs ([Artikel-Nr] int, Qty decimal(20,7), [Year] INT, [Kw] INT);
				IF OBJECT_ID('tempdb..#usedArticles') IS NOT NULL DROP TABLE #usedArticles;
				CREATE TABLE #usedArticles ([Artikel-Nr] INT,[Year] INT, [Kw] INT);
				IF OBJECT_ID('tempdb..#resultsArticles') IS NOT NULL DROP TABLE #resultsArticles;
				CREATE TABLE #resultsArticles ([Artikel-Nr] INT,[Artikelnummer] NVARCHAR(100), [UBG] BIT, [Year] INT, [Kw] INT, SumProd decimal(20,7), SumNeeds decimal(20,7));
				IF OBJECT_ID('tempdb..#resultsCumuls') IS NOT NULL DROP TABLE #resultsCumuls;
				CREATE TABLE #resultsCumuls ([Artikel-Nr] INT,[Artikelnummer] NVARCHAR(100), [UBG] BIT, [Year] INT, [Kw] INT, SumProd decimal(20,7), SumNeeds decimal(20,7), SumProdCumul decimal(20,7), SumNeedsCumul decimal(20,7), [Prio] INT, [PosOrder] INT, [LeadOrder] INT);

				-- 
				INSERT INTO #filterArts ([Artikel-Nr]) 
				SELECT a.[Artikel-Nr] FROM (SELECT [Artikel-Nr], [Artikelnummer], [Warengruppe], [Freigabestatus] FROM [Artikel]) a
				{(ubg == true ? $" JOIN ({GetUBGQuerySupplement(start, end, kreisClause)}) s ON s.[Artikel-Nr]=a.[Artikel-Nr]" : "")}
				{(unit is null ? "" : $" JOIN __BSD_ArtikelProductionExtension e on e.ArticleId=a.[Artikel-Nr] ")}
				WHERE [Warengruppe]='EF' AND [Freigabestatus]<>'o' /* 2024-05-13 - Schremmer */ 
				{(string.IsNullOrWhiteSpace(kreisClause) ? "" : $" AND {kreisClause}")}
				{(unit is null ? "" : $" AND ProductionPlace1_Id={unit}")};

				INSERT INTO #Prod ([Artikel-Nr], Qty, [Year], [Kw])
				SELECT [Artikel-Nr], SUM(Qty) Qty, [Year], [Kw] FROM (
				SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0))-SUM(ISNULL(Mindestbestand,0)) as Qty, Year(GETDATE()) AS [Year], DATEPART(ISO_WEEK, GETDATE()) AS [Kw] 
				FROM Lager WHERE [Artikel-Nr] IN (SELECT [Artikel-Nr] FROM #filterArts) GROUP BY [Artikel-Nr] HAVING SUM(ISNULL(Bestand,0))-SUM(ISNULL(Mindestbestand,0))<>0
				UNION ALL
				SELECT Artikel_nr , ISNULL(SUM(ISNULL([Anzahl],0)),0) AS Qty, 
					YEAR(COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) [Year], 
					DATEPART(ISO_WEEK, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) [Kw]
					FROM [Fertigung]
					WHERE Kennzeichen='offen' AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2) <= @end
					AND [Artikel_Nr] IN (SELECT [Artikel-Nr] FROM #filterArts)
					GROUP BY Artikel_nr, 
					YEAR(COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)), 
					DATEPART(ISO_WEEK, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2))
					) X GROUP BY [Artikel-Nr], [Year], [Kw];

				INSERT INTO #Needs ([Artikel-Nr], Qty, [Year], [Kw])
				SELECT [Artikel-Nr], SUM(Qty) Qty, [Year], Kw FROM (
				SELECT AR.[Artikel-Nr],
					ISNULL(SUM(ISNULL(AR.[Anzahl],0)),0) AS Qty, 
					YEAR(AR.Liefertermin) [Year], 
					DATEPART(ISO_WEEK, AR.Liefertermin) KW
					FROM Angebote A 
					INNER JOIN [angebotene Artikel] AR ON A.Nr=AR.[Angebot-Nr]
					WHERE A.Typ= 'Auftragsbestätigung' AND
					ISNULL(A.gebucht,0)=1 AND ISNULL(A.erledigt,0)=0 AND ISNULL(AR.erledigt_pos,0)=0 
					AND AR.Liefertermin <= @end
					AND [Artikel-Nr] IN (SELECT [Artikel-Nr] FROM #filterArts)
					GROUP BY AR.[Artikel-Nr], 
					YEAR(AR.Liefertermin), 
					DATEPART(ISO_WEEK, AR.Liefertermin)
					UNION ALL
					/* Delfor */
					SELECT li.ArticleId [Artikel-Nr],
					ISNULL(SUM(ISNULL(lp.PlanningQuantityQuantity,0)),0)-ISNULL(SUM(ISNULL(AbQuantity,0)),0) AS Qty,
					YEAR(lp.PlanningQuantityRequestedShipmentDate) [Year], 
					DATEPART(ISO_WEEK, lp.PlanningQuantityRequestedShipmentDate) KW
					FROM __EDI_DLF_Header h 
					INNER JOIN __EDI_DLF_LineItem li on li.HeaderId=h.Id
					INNER JOIN __EDI_DLF_LineItemPlan lp on lp.LineItemId=li.Id
					LEFT JOIN (SELECT a.nr_dlf, ISNULL(SUM(ISNULL(b.[OriginalAnzahl],0)),0) AbQuantity FROM Angebote a JOIN [angebotene Artikel] b on b.[Angebot-Nr]=a.Nr WHERE a.Typ='Auftragsbestätigung' AND ISNULL(a.gebucht,0)=1 AND ISNULL(a.erledigt,0)=0 AND ISNULL(b.erledigt_pos,0)=0 GROUP BY nr_dlf) AS o on o.nr_dlf=lp.Id
					INNER JOIN (SELECT DISTINCT MAX(ReferenceVersionNumber) AS ReferenceVersionNumber, DocumentNumber, PSZCustomernumber 
					FROM __EDI_DLF_Header WHERE ISNULL(Done,0)=0 
					GROUP BY PSZCustomernumber, DocumentNumber) u on u.DocumentNumber=h.DocumentNumber AND u.ReferenceVersionNumber=h.ReferenceVersionNumber AND u.PSZCustomernumber=h.PSZCustomernumber
					WHERE lp.PlanningQuantityRequestedShipmentDate <= @end 
					AND [ArticleId] IN (SELECT [Artikel-Nr] FROM #filterArts)
					GROUP BY li.ArticleId,
					YEAR(lp.PlanningQuantityRequestedShipmentDate), 
					DATEPART(ISO_WEEK, lp.PlanningQuantityRequestedShipmentDate)
					UNION ALL
					/* Forecasts */
					SELECT p.ArtikelNr [Artikel-Nr],
					ISNULL(SUM(ISNULL(p.[Menge],0)),0) AS Qty,
					YEAR(p.Datum) [Year], 
					DATEPART(ISO_WEEK, p.Datum) KW
					FROM Forecasts f
					INNER JOIN ForecastsPosition p on p.IdForcast=f.Id
					INNER JOIN (SELECT MAX(Datum) AS Datum, kundennummer FROM Forecasts WHERE Type='forecast'
					GROUP BY kundennummer) as u on u.kundennummer=f.kundennummer AND u.Datum=f.Datum
					WHERE f.Type='forecast'
					AND (IsOrdered IS NULL OR IsOrdered=0)
					AND p.Datum <= @end
					AND [ArtikelNr] IN (SELECT [Artikel-Nr] FROM #filterArts)
					GROUP BY p.ArtikelNr,
					YEAR(p.Datum), 
					DATEPART(ISO_WEEK, p.Datum)
					{(ubg == true ? $@"/* HBG FAs - 2024-08-14 - Schremmer - */
					UNION ALL
					SELECT fp.Artikel_Nr, fp.Anzahl, YEAR(COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)), DATEPART(ISO_WEEK, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) FROM Fertigung F inner join Fertigung_Positionen fp
					ON fp.ID_Fertigung=F.ID
					WHERE Kennzeichen='offen' 			    
					AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2) <= @end
					AND fp.Artikel_Nr IN (SELECT [Artikel-Nr] FROM #filterArts)" : "")}
					) AS T
					GROUP BY [Artikel-Nr], [Year], KW;

				INSERT INTO #usedArticles ([Artikel-Nr], [Year], [Kw])
				SELECT DISTINCT [Artikel-Nr], [Year], Kw FROM (
				SELECT [Artikel-Nr], [Year], Kw FROM #Prod
				UNION ALL
				SELECT [Artikel-Nr], [Year], Kw FROM #Needs
				) AS T;

				/*- 2024-06-17 Heidenreich: ignore UGB wo needs -*/
				--{(ubg == true ? "DELETE FROM #usedArticles WHERE [Artikel-Nr] IN (SELECT p.[Artikel-Nr] FROM #Prod p LEFT JOIN #Needs n on n.[Artikel-Nr]=p.[Artikel-Nr] JOIN (SELECT [Artikel-Nr], UBG FROM Artikel WHERE UBG=1) a on a.[Artikel-Nr]=p.[Artikel-Nr] WHERE n.[Artikel-Nr] IS NULL);" : "")}

				INSERT INTO #resultsArticles ([Artikel-Nr], [Artikelnummer],[UBG], SumProd, SumNeeds, [Year], [Kw])
				SELECT A.[Artikel-Nr], A.Artikelnummer, A.UBG, SUM(ISNULL(x.prodQty,0)) AS SumProd,SUM(ISNULL(x.needsQty,0)) AS SumNeeds, ISNULL(X.[Year],0) AS [Year], ISNULL(X.Kw,0) Kw
				FROM [Artikel] A 
				LEFT JOIN (
				SELECT k.[Artikel-Nr], k.[Year], k.Kw, SUM(ISNULL(p.Qty,0)) prodQty, SUM(ISNULL(n.Qty,0)) needsQty 
				FROM #usedArticles k 
				left join #Prod p on p.[Artikel-Nr]=k.[Artikel-Nr] AND p.Year=k.Year AND p.Kw=k.Kw
				left join #Needs n on n.[Artikel-Nr]=k.[Artikel-Nr] AND n.Year=k.Year AND n.Kw=k.Kw 
				GROUP BY k.[Artikel-Nr], k.[Year], k.Kw
				) x on x.[Artikel-Nr]=A.[Artikel-Nr]
				WHERE Year>0 AND Kw>0";

				if(!string.IsNullOrEmpty(artikelnummer) && !string.IsNullOrWhiteSpace(artikelnummer))
				{
					query += $" AND [Artikelnummer] LIKE '%{artikelnummer}%'";
				}
				query += $@" GROUP BY A.[Artikel-Nr],A.Artikelnummer,A.UBG,X.[Year], X.KW 

INSERT INTO #resultsCumuls ([Artikel-Nr], [Artikelnummer], [UBG], [Year], [Kw], SumProd, SumNeeds, SumProdCumul, SumNeedsCumul,[Prio],[PosOrder],[LeadOrder])
SELECT t.*,
       sum(SumProd) over (partition by [Artikelnummer] order by [Year], [Kw]) as SumProd_cumul,
	   sum(SumNeeds) over (partition by [Artikelnummer] order by [Year], [Kw]) as SumNeeds_cumul, -1, 0, 98
FROM #resultsArticles t;

With cte As
(SELECT [Artikel-Nr], [Artikelnummer], [Year], [Kw], [PosOrder],
ROW_NUMBER() OVER (ORDER BY [Artikel-Nr],[Year], [Kw] ASC) AS RN
FROM #resultsCumuls)
UPDATE cte SET [PosOrder]=RN;

UPDATE curr SET
	[LeadOrder]=ISNULL((select top 1 (nx.[Year]-curr.[Year])*52 + nx.[kw]-curr.[Kw] from #resultsCumuls nx where nx.[Artikel-Nr]=curr.[Artikel-Nr] and nx.SumNeedsCumul-curr.SumProdCumul >= 0 and nx.PosOrder >= curr.PosOrder order by nx.PosOrder asc),99)
from #resultsCumuls curr WHERE SumProdCumul-SumNeedsCumul>=0;

UPDATE #resultsCumuls SET [Prio]=CASE WHEN [LeadOrder]>4 THEN 2
	WHEN [LeadOrder]=3 OR [LeadOrder]=4 THEN 3
	WHEN [LeadOrder]=2 THEN 4
	/* WHEN [LeadOrder]=1 OR [LeadOrder]=0 THEN 1 Heidenreich 2024-05-22 or first Need on same or next KW*/
END

/*- Prio 1: Neg. Bestand  -*/
UPDATE #resultsCumuls SET [Prio]=1 WHERE SumProdCumul<SumNeedsCumul;

/*- Prio 0: Neg. Bestand for Curr. & KW + 1 & FA in Backlog -*/
UPDATE #resultsCumuls SET [Prio]=0 WHERE SumProdCumul<SumNeedsCumul AND ([Kw]=DATEPART(ISO_WEEK,GETDATE()) OR [Kw]=DATEPART(ISO_WEEK,GETDATE())+1) AND [Artikel-Nr] IN (SELECT [Artikel-Nr] FROM #resultsCumuls WHERE SumProd>0 AND SumProdCumul<SumNeedsCumul AND ([Year]<YEAR(GETDATE()) OR ([Year]=YEAR(GETDATE()) AND [Kw]<DATEPART(ISO_WEEK,GETDATE()))));

/*- Prio 2: Pos. Bestand & first Need after more than 4 KW -*/
/* UPDATE #resultsCumuls SET [Prio]=2 WHERE SumProdCumul>SumNeedsCumul; */

/*- Prio 3: Pos. Bestand & first Need after 3 or 4  KW -*/
/* UPDATE #resultsCumuls SET [Prio]=3 WHERE SumProdCumul>SumNeedsCumul; */

/*- Prio 4: Pos. Bestand & first Need after 2 KW -*/
/* UPDATE #resultsCumuls SET [Prio]=4 WHERE SumProdCumul>SumNeedsCumul; */

/*- Prio 5: Pos. No Bestand & No Need -*/
UPDATE #resultsCumuls SET [Prio]=5 WHERE [Artikel-Nr] IN
(SELECT [Artikel-Nr] FROM #resultsCumuls GROUP BY [Artikel-Nr]
HAVING SUM(ISNULL(SumProdCumul,0))=0 AND SUM(ISNULL(SumNeedsCumul,0))=0);

/*- SET Prio as Min Prio per Article -*/
UPDATE t SET [Prio]=[mPrio] 
FROM #resultsCumuls t
JOIN (SELECT [Artikel-Nr], MIN([Prio]) [mPrio] FROM #resultsCumuls GROUP BY [Artikel-Nr]) g 
on g.[Artikel-Nr]=t.[Artikel-Nr];


SELECT DISTINCT [Artikel-Nr],[Artikelnummer], [UBG], [Prio] FROM #resultsCumuls
{(!deficit.HasValue ? "" : $" WHERE [Prio]={deficit}")} ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName) ? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}" : "[Artikelnummer]")}{(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}

IF OBJECT_ID('tempdb..#filterArts') IS NOT NULL DROP TABLE #filterArts;
IF OBJECT_ID('tempdb..#Prod') IS NOT NULL DROP TABLE #Prod;
IF OBJECT_ID('tempdb..#Needs') IS NOT NULL DROP TABLE #Needs;
IF OBJECT_ID('tempdb..#usedArticles') IS NOT NULL DROP TABLE #usedArticles;
IF OBJECT_ID('tempdb..#resultsCumuls') IS NOT NULL DROP TABLE #resultsCumuls;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.CommandTimeout = 90; // sec
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.FAPlannungArticlesEntity_3(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int GetArticlesByNummerkreisCount(string kreisClause, string artikelnummer, int? deficit, DateTime? start, DateTime end, int? unit, bool ubg = false)
		{
			kreisClause = kreisClause ?? "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"IF OBJECT_ID('tempdb..#filterArts') IS NOT NULL DROP TABLE #filterArts;
				CREATE TABLE #filterArts ([Artikel-Nr] int);
				IF OBJECT_ID('tempdb..#Prod') IS NOT NULL DROP TABLE #Prod;
				CREATE TABLE #Prod ([Artikel-Nr] int, Qty decimal(20,7), [Year] INT, [Kw] INT);
				IF OBJECT_ID('tempdb..#Needs') IS NOT NULL DROP TABLE #Needs;
				CREATE TABLE #Needs ([Artikel-Nr] int, Qty decimal(20,7), [Year] INT, [Kw] INT);
				IF OBJECT_ID('tempdb..#usedArticles') IS NOT NULL DROP TABLE #usedArticles;
				CREATE TABLE #usedArticles ([Artikel-Nr] INT,[Year] INT, [Kw] INT);
				IF OBJECT_ID('tempdb..#resultsArticles') IS NOT NULL DROP TABLE #resultsArticles;
				CREATE TABLE #resultsArticles ([Artikel-Nr] INT,[Artikelnummer] NVARCHAR(100), [UBG] BIT, [Year] INT, [Kw] INT, SumProd decimal(20,7), SumNeeds decimal(20,7));
				IF OBJECT_ID('tempdb..#resultsCumuls') IS NOT NULL DROP TABLE #resultsCumuls;
				CREATE TABLE #resultsCumuls ([Artikel-Nr] INT,[Artikelnummer] NVARCHAR(100), [UBG] BIT, [Year] INT, [Kw] INT, SumProd decimal(20,7), SumNeeds decimal(20,7), SumProdCumul decimal(20,7), SumNeedsCumul decimal(20,7), [Prio] INT, [PosOrder] INT, [LeadOrder] INT);

				-- 
				INSERT INTO #filterArts ([Artikel-Nr]) 
				SELECT a.[Artikel-Nr] FROM (SELECT [Artikel-Nr], [Artikelnummer], [Warengruppe], [Freigabestatus] FROM [Artikel]) a
				{(ubg == true ? $" JOIN ({GetUBGQuerySupplement(start, end, kreisClause)}) s ON s.[Artikel-Nr]=a.[Artikel-Nr]" : "")}
				{(unit is null ? "" : $" JOIN __BSD_ArtikelProductionExtension e on e.ArticleId=a.[Artikel-Nr] ")}
				WHERE [Warengruppe]='EF' AND [Freigabestatus]<>'o' /* 2024-05-13 - Schremmer */ 
				{(string.IsNullOrWhiteSpace(kreisClause) ? "" : $" AND {kreisClause}")}
				{(unit is null ? "" : $" AND ProductionPlace1_Id={unit}")};

				INSERT INTO #Prod ([Artikel-Nr], Qty, [Year], [Kw])
				SELECT [Artikel-Nr], SUM(Qty) Qty, [Year], [Kw] FROM (
				SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0))-SUM(ISNULL(Mindestbestand,0)) as Qty, Year(GETDATE()) AS [Year], DATEPART(ISO_WEEK, GETDATE()) AS [Kw] FROM Lager WHERE [Artikel-Nr] IN (SELECT [Artikel-Nr] FROM #filterArts) GROUP BY [Artikel-Nr] HAVING SUM(ISNULL(Bestand,0))-SUM(ISNULL(Mindestbestand,0))<>0
				UNION ALL
				SELECT Artikel_nr , ISNULL(SUM(ISNULL([Anzahl],0)),0) AS Qty, 
					YEAR(COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) [Year], 
					DATEPART(ISO_WEEK, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) [Kw]
					FROM [Fertigung]
					WHERE Kennzeichen='offen' AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2) <= @end
					AND [Artikel_Nr] IN (SELECT [Artikel-Nr] FROM #filterArts)
					GROUP BY Artikel_nr, 
					YEAR(COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)), 
					DATEPART(ISO_WEEK, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2))
					) X GROUP BY [Artikel-Nr], [Year], [Kw];

				INSERT INTO #Needs ([Artikel-Nr], Qty, [Year], [Kw])
				SELECT [Artikel-Nr], SUM(Qty) Qty, [Year], Kw FROM (
				SELECT AR.[Artikel-Nr],
					ISNULL(SUM(ISNULL(AR.[Anzahl],0)),0) AS Qty, 
					YEAR(AR.Liefertermin) [Year], 
					DATEPART(ISO_WEEK, AR.Liefertermin) KW
					FROM Angebote A 
					INNER JOIN [angebotene Artikel] AR ON A.Nr=AR.[Angebot-Nr]
					WHERE A.Typ= 'Auftragsbestätigung' AND
					ISNULL(A.gebucht,0)=1 AND ISNULL(A.erledigt,0)=0 AND ISNULL(AR.erledigt_pos,0)=0 
					AND AR.Liefertermin <= @end
					AND [Artikel-Nr] IN (SELECT [Artikel-Nr] FROM #filterArts)
					GROUP BY AR.[Artikel-Nr], 
					YEAR(AR.Liefertermin), 
					DATEPART(ISO_WEEK, AR.Liefertermin)
					UNION ALL
					/* Delfor */
					SELECT li.ArticleId [Artikel-Nr],
					ISNULL(SUM(ISNULL(lp.PlanningQuantityQuantity,0)),0)-ISNULL(SUM(ISNULL(AbQuantity,0)),0) AS Qty,
					YEAR(lp.PlanningQuantityRequestedShipmentDate) [Year], 
					DATEPART(ISO_WEEK, lp.PlanningQuantityRequestedShipmentDate) KW
					FROM __EDI_DLF_Header h 
					INNER JOIN __EDI_DLF_LineItem li on li.HeaderId=h.Id
					INNER JOIN __EDI_DLF_LineItemPlan lp on lp.LineItemId=li.Id
					LEFT JOIN (SELECT a.nr_dlf, ISNULL(SUM(ISNULL(b.[OriginalAnzahl],0)),0) AbQuantity FROM Angebote a JOIN [angebotene Artikel] b on b.[Angebot-Nr]=a.Nr WHERE a.Typ='Auftragsbestätigung' AND ISNULL(a.gebucht,0)=1 AND ISNULL(a.erledigt,0)=0 AND ISNULL(b.erledigt_pos,0)=0 GROUP BY nr_dlf) AS o on o.nr_dlf=lp.Id
					INNER JOIN (SELECT DISTINCT MAX(ReferenceVersionNumber) AS ReferenceVersionNumber, DocumentNumber,PSZCustomernumber  
					FROM __EDI_DLF_Header WHERE ISNULL(Done,0)=0 
					GROUP BY PSZCustomernumber, DocumentNumber) u on u.DocumentNumber=h.DocumentNumber AND u.ReferenceVersionNumber=h.ReferenceVersionNumber AND u.PSZCustomernumber=h.PSZCustomernumber
					WHERE lp.PlanningQuantityRequestedShipmentDate <= @end 
					AND [ArticleId] IN (SELECT [Artikel-Nr] FROM #filterArts)
					GROUP BY li.ArticleId,
					YEAR(lp.PlanningQuantityRequestedShipmentDate), 
					DATEPART(ISO_WEEK, lp.PlanningQuantityRequestedShipmentDate)
					UNION ALL
					/* Forecasts */
					SELECT p.ArtikelNr [Artikel-Nr],
					ISNULL(SUM(ISNULL(p.[Menge],0)),0) AS Qty,
					YEAR(p.Datum) [Year], 
					DATEPART(ISO_WEEK, p.Datum) KW
					FROM Forecasts f
					INNER JOIN ForecastsPosition p on p.IdForcast=f.Id
					INNER JOIN (SELECT MAX(Datum) AS Datum, kundennummer FROM Forecasts WHERE Type='forecast'
					GROUP BY kundennummer) as u on u.kundennummer=f.kundennummer AND u.Datum=f.Datum
					WHERE f.Type='forecast'
					AND (IsOrdered IS NULL OR IsOrdered=0)
					AND p.Datum <= @end
					AND [ArtikelNr] IN (SELECT [Artikel-Nr] FROM #filterArts)
					GROUP BY p.ArtikelNr,
					YEAR(p.Datum), 
					DATEPART(ISO_WEEK, p.Datum)
					) AS T
					GROUP BY [Artikel-Nr], [Year], KW;

				INSERT INTO #usedArticles ([Artikel-Nr], [Year], [Kw])
				SELECT DISTINCT [Artikel-Nr], [Year], Kw FROM (
				SELECT [Artikel-Nr], [Year], Kw FROM #Prod
				UNION ALL
				SELECT [Artikel-Nr], [Year], Kw FROM #Needs
				) AS T;

				/*- 2024-06-17 Heidenreich: ignore UGB wo needs -*/
				--{(ubg == true ? "DELETE FROM #usedArticles WHERE [Artikel-Nr] IN (SELECT p.[Artikel-Nr] FROM #Prod p LEFT JOIN #Needs n on n.[Artikel-Nr]=p.[Artikel-Nr] JOIN (SELECT [Artikel-Nr], UBG FROM Artikel WHERE UBG=1) a on a.[Artikel-Nr]=p.[Artikel-Nr] WHERE n.[Artikel-Nr] IS NULL);" : "")}

				INSERT INTO #resultsArticles ([Artikel-Nr], [Artikelnummer], [UBG], SumProd, SumNeeds, [Year], [Kw])
				SELECT A.[Artikel-Nr], A.Artikelnummer,A.UBG, SUM(ISNULL(x.prodQty,0)) AS SumProd,SUM(ISNULL(x.needsQty,0)) AS SumNeeds, ISNULL(X.[Year],0) AS [Year], ISNULL(X.Kw,0) Kw
				FROM [Artikel] A 
				LEFT JOIN (
				SELECT k.[Artikel-Nr], k.[Year], k.Kw, SUM(ISNULL(p.Qty,0)) prodQty, SUM(ISNULL(n.Qty,0)) needsQty 
				FROM #usedArticles k 
				left join #Prod p on p.[Artikel-Nr]=k.[Artikel-Nr] AND p.Year=k.Year AND p.Kw=k.Kw
				left join #Needs n on n.[Artikel-Nr]=k.[Artikel-Nr] AND n.Year=k.Year AND n.Kw=k.Kw 
				GROUP BY k.[Artikel-Nr], k.[Year], k.Kw
				) x on x.[Artikel-Nr]=A.[Artikel-Nr]
				WHERE Year>0 AND Kw>0";
				if(!string.IsNullOrEmpty(artikelnummer) && !string.IsNullOrWhiteSpace(artikelnummer))
				{
					query += $" AND [Artikelnummer] LIKE '%{artikelnummer}%'";
				}
				query += $@" GROUP BY A.[Artikel-Nr],A.Artikelnummer,A.UBG,X.[Year], X.KW 

				INSERT INTO #resultsCumuls ([Artikel-Nr], [Artikelnummer], [UBG], [Year], [Kw], SumProd, SumNeeds, SumProdCumul, SumNeedsCumul,[Prio],[PosOrder],[LeadOrder])
				SELECT t.*,
					   sum(SumProd) over (partition by [Artikelnummer] order by [Year], [Kw]) as SumProd_cumul,
					   sum(SumNeeds) over (partition by [Artikelnummer] order by [Year], [Kw]) as SumNeeds_cumul, -1, 0, 98
				FROM #resultsArticles t;

				With cte As
				(SELECT [Artikel-Nr], [Artikelnummer], [Year], [Kw], [PosOrder],
				ROW_NUMBER() OVER (ORDER BY [Artikel-Nr],[Year], [Kw] ASC) AS RN
				FROM #resultsCumuls)
				UPDATE cte SET [PosOrder]=RN;

				UPDATE curr SET
					[LeadOrder]=ISNULL((select top 1 (nx.[Year]-curr.[Year])*52 + nx.[kw]-curr.[Kw] from #resultsCumuls nx where nx.[Artikel-Nr]=curr.[Artikel-Nr] and nx.SumNeedsCumul-curr.SumProdCumul >= 0 and nx.PosOrder >= curr.PosOrder order by nx.PosOrder asc),99)
				from #resultsCumuls curr WHERE SumProdCumul-SumNeedsCumul>=0;

				UPDATE #resultsCumuls SET [Prio]=CASE WHEN [LeadOrder]>4 THEN 2
					WHEN [LeadOrder]=3 OR [LeadOrder]=4 THEN 3
					WHEN [LeadOrder]=2 THEN 4
					/* WHEN [LeadOrder]=1 OR [LeadOrder]=0 THEN 1 Heidenreich 2024-05-22 or first Need on same or next KW */
				END

				/*- Prio 1: Neg. Bestand -*/
				UPDATE #resultsCumuls SET [Prio]=1 WHERE SumProdCumul<SumNeedsCumul;

				/*- Prio 0: Neg. Bestand for Curr. & KW + 1 & FA in Backlog -*/
				UPDATE #resultsCumuls SET [Prio]=0 WHERE SumProdCumul<SumNeedsCumul AND ([Kw]=DATEPART(ISO_WEEK,GETDATE()) OR [Kw]=DATEPART(ISO_WEEK,GETDATE())+1) AND [Artikel-Nr] IN (SELECT [Artikel-Nr] FROM #resultsCumuls WHERE SumProd>0 AND SumProdCumul<SumNeedsCumul AND ([Year]<YEAR(GETDATE()) OR ([Year]=YEAR(GETDATE()) AND [Kw]<DATEPART(ISO_WEEK,GETDATE()))));

				/*- Prio 2: Pos. Bestand & first Need after more than 4 KW -*/
				/* UPDATE #resultsCumuls SET [Prio]=2 WHERE SumProdCumul>SumNeedsCumul; */

				/*- Prio 3: Pos. Bestand & first Need after 3 or 4  KW -*/
				/* UPDATE #resultsCumuls SET [Prio]=3 WHERE SumProdCumul>SumNeedsCumul; */

				/*- Prio 4: Pos. Bestand & first Need after 2 KW -*/
				/* UPDATE #resultsCumuls SET [Prio]=4 WHERE SumProdCumul>SumNeedsCumul; */

				/*- Prio 5: Pos. No Bestand & No Need -*/
				UPDATE #resultsCumuls SET [Prio]=5 WHERE [Artikel-Nr] IN
				(SELECT [Artikel-Nr] FROM #resultsCumuls GROUP BY [Artikel-Nr]
				HAVING SUM(ISNULL(SumProdCumul,0))=0 AND SUM(ISNULL(SumNeedsCumul,0))=0);

				/*- SET Prio as Min Prio per Article -*/
				UPDATE t SET [Prio]=[mPrio] 
				FROM #resultsCumuls t
				JOIN (SELECT [Artikel-Nr], MIN([Prio]) [mPrio] FROM #resultsCumuls GROUP BY [Artikel-Nr]) g 
				on g.[Artikel-Nr]=t.[Artikel-Nr];


				SELECT DISTINCT COUNT(DISTINCT [Artikel-Nr]) FROM #resultsCumuls{(!deficit.HasValue ? "" : $" WHERE [Prio]={deficit}")}

				IF OBJECT_ID('tempdb..#filterArts') IS NOT NULL DROP TABLE #filterArts;
				IF OBJECT_ID('tempdb..#Prod') IS NOT NULL DROP TABLE #Prod;
				IF OBJECT_ID('tempdb..#Needs') IS NOT NULL DROP TABLE #Needs;
				IF OBJECT_ID('tempdb..#usedArticles') IS NOT NULL DROP TABLE #usedArticles;
				IF OBJECT_ID('tempdb..#resultsCumuls') IS NOT NULL DROP TABLE #resultsCumuls;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}
		public static List<Entities.Joins.CRP.FAPlannungArticlesKwEntity> GetArticlesKwDetails(List<int> articleIds, DateTime? start, DateTime end, int startKw, int endKw, int? unit)
		{
			if(articleIds == null || articleIds.Count == 0)
			{ return null; }
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var kws = System.Linq.Enumerable.Range(startKw, endKw).Select(x => $"({x})");
				string query = $@"IF OBJECT_ID('tempdb..##KW_Data') IS NOT NULL DROP TABLE ##KW_Data;
				CREATE TABLE ##KW_Data (KW int);
				INSERT INTO ##KW_Data (KW) VALUES {string.Join(",", kws)};

				SELECT A.[Artikel-Nr], a.Artikelnummer, a.KW, SUM(ISNULL(X.SumFA,0)) SumFA, SUM(ISNULL(Y.SumAB,0)) SumAB, SUM(ISNULL(Z.SumLP,0)) SumLP, SUM(ISNULL(Q.SumFC,0)) SumFC 
				FROM 
				(SELECT [Artikel-Nr], Artikelnummer, KW 
				FROM [Artikel], ##KW_Data
				WHERE [Artikel-Nr] IN ({string.Join(",", articleIds)})
				) AS A 
				LEFT JOIN
				(SELECT Artikel_nr,SUM(ISNULL([Anzahl],0)) AS SumFA, DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) AS KW
				FROM [Fertigung]
				WHERE Kennzeichen='offen' 
                AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2) BETWEEN @start and @end
				GROUP BY Artikel_nr, DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2))
				) As X on X.KW=A.KW AND x.Artikel_Nr=A.[Artikel-Nr]
				LEFT JOIN
				(SELECT AR.[Artikel-Nr],
				SUM(ISNULL(AR.Anzahl,0)) AS SumAB, DATEPART(iso_week,AR.Liefertermin) AS KW
				FROM Angebote A 
				INNER JOIN [angebotene Artikel] AR ON A.Nr=AR.[Angebot-Nr]
				WHERE A.Typ='Auftragsbestätigung' AND
				A.gebucht=1 AND ISNULL(A.erledigt,0)=0 AND ISNULL(AR.erledigt_pos,0)=0 AND 
				AR.Liefertermin BETWEEN @start and @end
				GROUP BY AR.[Artikel-Nr], DATEPART(iso_week,AR.Liefertermin)
				) AS Y on Y.[Artikel-Nr]=A.[Artikel-Nr] AND Y.KW=A.KW
				/* Delfor */
				LEFT JOIN
				(SELECT li.ArticleId,
				SUM(ISNULL(lp.PlanningQuantityQuantity,0)) AS SumLP, DATEPART(iso_week,lp.PlanningQuantityRequestedShipmentDate) AS KW
				FROM __EDI_DLF_Header h 
				INNER JOIN __EDI_DLF_LineItem li on li.HeaderId=h.Id
				INNER JOIN __EDI_DLF_LineItemPlan lp on lp.LineItemId=li.Id
				INNER JOIN (SELECT DISTINCT MAX(ReferenceVersionNumber) AS ReferenceVersionNumber, DocumentNumber FROM __EDI_DLF_Header WHERE ISNULL(Done,0)=0 GROUP BY PSZCustomernumber, DocumentNumber) u on u.DocumentNumber=h.DocumentNumber AND u.ReferenceVersionNumber=h.ReferenceVersionNumber
				WHERE lp.PlanningQuantityRequestedShipmentDate BETWEEN @start and @end
				GROUP BY li.ArticleId, DATEPART(iso_week,lp.PlanningQuantityRequestedShipmentDate)
				) AS Z on Z.ArticleId=A.[Artikel-Nr] AND Z.KW=A.KW
				/* Forecasts */
				LEFT JOIN
				(SELECT p.ArtikelNr,
				SUM(ISNULL(p.Menge,0)) AS SumFC, DATEPART(iso_week,p.Datum) AS KW
				FROM Forecasts f
				INNER JOIN ForecastsPosition p on p.IdForcast=f.Id
				INNER JOIN (SELECT MAX(Datum) AS Datum, kundennummer FROM Forecasts WHERE Type='forecast' GROUP BY kundennummer) as u on u.kundennummer=f.kundennummer AND u.Datum=f.Datum
				WHERE f.Type='forecast'
                AND (IsOrdered IS NULL OR IsOrdered=0)
				AND p.Datum BETWEEN @start and @end
				GROUP BY p.ArtikelNr, DATEPART(iso_week,p.Datum)
				) AS Q on Q.ArtikelNr=A.[Artikel-Nr] AND Q.KW=A.KW
				GROUP BY A.[Artikel-Nr], a.Artikelnummer, a.KW
				ORDER BY A.[Artikel-Nr], a.KW";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.FAPlannungArticlesKwEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static KeyValuePair<int, decimal> GetResidueFaByArticle(int articleNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT COUNT(DISTINCT Fertigungsnummer) AS count,  ISNULL(SUM(ISNULL([Anzahl],0)),0) AS Quantity
								FROM [Fertigung]
								WHERE Kennzeichen='offen' AND Artikel_Nr=@articleNr
								AND (COALESCE(Termin_Bestätigt1, Termin_Bestätigt2))<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new KeyValuePair<int, decimal>(
					Convert.ToInt32(dataTable.Rows[0]["Count"]),
					Convert.ToDecimal(dataTable.Rows[0]["Quantity"]));
			}
			else
			{
				return new KeyValuePair<int, decimal>(0, 0);
			}
		}
		public static List<Entities.Joins.CRP.FAPlannungArticlesKwDataEntity> GetArticleKwData(int articleId, DateTime? kwDate, string type)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";
				switch(type.ToLower())
				{
					case "ab":
						query = $@"SELECT AR.[Artikel-Nr] ArticleId, A.Nr Id, A.[Angebot-Nr] Number, AR.Anzahl Quantity, 0 AS CustomerNumber, 0 AS Manual
									FROM Angebote A 
									INNER JOIN [angebotene Artikel] AR ON A.Nr=AR.[Angebot-Nr]
									WHERE AR.[Artikel-Nr]=@articleId AND A.Typ='Auftragsbestätigung' AND
									A.gebucht=1 AND ISNULL(A.erledigt,0)=0 
									AND ISNULL(AR.erledigt_pos,0)=0 {(kwDate.HasValue == false
									? $" AND AR.Liefertermin < cast(DATEADD(day, -1*(DATEPART(WEEKDAY, getdate())-1), getdate()) as DATE) "
									: $@" AND YEAR(AR.Liefertermin)=YEAR(@kw) 
									AND DATEPART(iso_week,AR.Liefertermin)=DATEPART(iso_week,@kw);")}";
						break;
					case "fa":
						query = $@"SELECT Artikel_nr ArticleId, Id, Fertigungsnummer Number, Anzahl Quantity, 0 AS CustomerNumber, 0 AS Manual
									FROM [Fertigung]
									WHERE Artikel_nr=@articleId AND Kennzeichen='offen'{(kwDate.HasValue == false
									? $" AND COALESCE(Termin_Bestätigt1,Termin_Bestätigt2) < cast(DATEADD(day, -1*(DATEPART(WEEKDAY, getdate())-1), getdate()) as DATE)"
									: $@" AND YEAR(COALESCE(Termin_Bestätigt1,Termin_Bestätigt2))=YEAR(@kw) 
									AND DATEPART(iso_week, COALESCE(Termin_Bestätigt1,Termin_Bestätigt2))=DATEPART(iso_week,@kw);")}";
						break;
					case "lp":
						query = $@"SELECT li.ArticleId, h.Id, h.DocumentNumber Number, lp.PlanningQuantityQuantity Quantity, h.PSZCustomerNumber CustomerNumber, h.ManualCreation Manual
									FROM __EDI_DLF_Header h 
									INNER JOIN __EDI_DLF_LineItem li on li.HeaderId=h.Id
									INNER JOIN __EDI_DLF_LineItemPlan lp on lp.LineItemId=li.Id
									INNER JOIN (SELECT DISTINCT MAX(ReferenceVersionNumber) AS ReferenceVersionNumber, DocumentNumber, PSZCustomernumber 
									FROM __EDI_DLF_Header WHERE ISNULL(Done,0)=0 GROUP BY PSZCustomernumber, DocumentNumber) u 
										on u.DocumentNumber=h.DocumentNumber AND u.ReferenceVersionNumber=h.ReferenceVersionNumber AND u.PSZCustomernumber=h.PSZCustomernumber
									WHERE li.ArticleId=@articleId {(kwDate.HasValue == false
									? $" AND lp.PlanningQuantityRequestedShipmentDate < cast(DATEADD(day, -1*(DATEPART(WEEKDAY, getdate())-1), getdate()) as DATE)"
									: $@" AND YEAR(lp.PlanningQuantityRequestedShipmentDate)=YEAR(GETDATE()) 
									AND DATEPART(iso_week,lp.PlanningQuantityRequestedShipmentDate)=DATEPART(iso_week,@kw);")}";
						break;
					case "fc":
						query = $@"SELECT p.ArtikelNr ArticleId, f.Id, f.Type Number, p.Menge Quantity, 0 AS CustomerNumber, 0 AS Manual
									FROM Forecasts f
									INNER JOIN ForecastsPosition p on p.IdForcast=f.Id
									INNER JOIN (SELECT MAX(Datum) AS Datum, kundennummer FROM Forecasts WHERE Type='forecast' GROUP BY kundennummer) as u on u.kundennummer=f.kundennummer AND u.Datum=f.Datum
									WHERE p.ArtikelNr=@articleId AND f.Type='forecast' 
                                    AND (IsOrdered IS NULL OR IsOrdered=0){(kwDate.HasValue == false
									? $" AND p.Datum < cast(DATEADD(day, -1*(DATEPART(WEEKDAY, getdate())-1), getdate()) as DATE)"
									: $@" AND YEAR(p.Datum)=YEAR(@kw) AND DATEPART(iso_week,p.Datum)=DATEPART(iso_week,@kw);")}";
						break;
					default:
						break;
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("kw", kwDate ?? (object)DBNull.Value);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.FAPlannungArticlesKwDataEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Joins.CRP.FAPlannungArticlesKwDataEntity> GetArticleKwData_delayed(int articleId, DateTime? kwDate, string type)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";
				switch(type.ToLower())
				{
					case "ab":
						query = $@"SELECT [Artikel-Nr] ArticleId, AbNr Id, [ABNumber] Number, AbQuantity Quantity, 0 AS CustomerNumber, 0 AS Manual
									FROM __CRP_FaPlanningAbStatus
									WHERE [Artikel-Nr]=@articleId {(kwDate.HasValue == false
									? $" AND ABDate < cast(DATEADD(day, -1*(DATEPART(WEEKDAY, getdate())-1), getdate()) as DATE) "
									: $@" AND YEAR(ABDate)=YEAR(@kw) 
									AND DATEPART(iso_week,ABDate)=DATEPART(iso_week,@kw);")}";
						break;
					case "fa":
						query = $@"SELECT Artikel_nr ArticleId, FAId Id, Fertigungsnummer Number,  FaQuantity Quantity, 0 AS CustomerNumber, 0 AS Manual
									FROM [__CRP_FaPlanningFaStatus]
									WHERE Artikel_nr=@articleId {(kwDate.HasValue == false
									? $" AND FaDate < cast(DATEADD(day, -1*(DATEPART(WEEKDAY, getdate())-1), getdate()) as DATE)"
									: $@" AND YEAR(FaDate)=YEAR(@kw) 
									AND DATEPART(iso_week, FaDate)=DATEPART(iso_week,@kw);")}";
						break;
					case "lp":
						query = $@"SELECT ArticleId, LpId Id, LpDocument Number, LpQuantity Quantity, LpCustomer CustomerNumber, ManualCreation Manual
									FROM __CRP_FaPlanningLpStatus
									WHERE ArticleId=@articleId {(kwDate.HasValue == false
									? $" AND LpDate < cast(DATEADD(day, -1*(DATEPART(WEEKDAY, getdate())-1), getdate()) as DATE)"
									: $@" AND YEAR(LpDate)=YEAR(GETDATE()) 
									AND DATEPART(iso_week,LpDate)=DATEPART(iso_week,@kw);")}";
						break;
					case "fc":
						query = $@"SELECT ArtikelNr ArticleId, Id, Type Number, FcQuantity Quantity, 0 AS CustomerNumber, 0 AS Manual
									FROM __CRP_FaPlanningFcStatus
									WHERE ArtikelNr=@articleId {(kwDate.HasValue == false
									? $" AND FcDate < cast(DATEADD(day, -1*(DATEPART(WEEKDAY, getdate())-1), getdate()) as DATE)"
									: $@" AND YEAR(FcDate)=YEAR(@kw) AND DATEPART(iso_week,FcDate)=DATEPART(iso_week,@kw);")}";
						break;
					default:
						break;
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("kw", kwDate ?? (object)DBNull.Value);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.FAPlannungArticlesKwDataEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		//
		public static List<KeyValuePair<int, decimal>> GetInternalOrExternalNeeds(int articleNr, DateTime? start, DateTime end, bool _internal = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)) AS Week,ISNULL(SUM(ISNULL(fp.[Anzahl],0)),0) AS Quantity
                               FROM [Fertigung] f 
							   INNER JOIN Fertigung_Positionen fp ON fp.ID_Fertigung=F.ID
                               WHERE fp.Artikel_Nr=@articleNr 
							   AND Kennzeichen=N'offen' 							   
							   AND fp.Lagerort_ID {(_internal ? "=" : "<>")} (select ProductionPlace1_Id from __BSD_ArtikelProductionExtension where ArticleId=@articleNr)
							   {(start.HasValue ? $"AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)>= @start" : "")}                       
							   AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2)<= @end
                               GROUP BY DATEPART(iso_week, COALESCE(Termin_Bestätigt1, Termin_Bestätigt2))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}

		#region Delayed version
		public static List<KeyValuePair<int, decimal>> GetFAQuantitiesByArticle_delayed(int articleNr, DateTime? start, DateTime end, bool? internSiteProd = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, FaDate) AS Week, ISNULL(SUM(ISNULL([FaQuantity],0)),0) AS Quantity
                               FROM [__CRP_FaPlanningFaStatus]
                               WHERE Artikel_Nr=@articleNr{(!internSiteProd.HasValue ? "" : $" AND {(internSiteProd == true ? $"Lagerort_id=ProductionPlace1_Id" : "Lagerort_id<>ProductionPlace1_Id")}")}
								{(start.HasValue == false ? "" : " AND @start<=FaDate")} 
								AND FaDate<=@end
                               GROUP BY FaDate";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetABQuantitiesByArticle_delayed(int articleNr, DateTime? start, DateTime end)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, AbDate) AS Week,
                               SUM(AbQuantity) AS Quantity
                               FROM __CRP_FaPlanningAbStatus
                               WHERE [Artikel-Nr]=@articleNr {(start.HasValue == false ? "" : " AND @start<=AbDate")}
                               AND AbDate <= @end
                               GROUP BY DATEPART(iso_week, AbDate)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetLPQuantitiesByArticle_delayed(int articleNr, DateTime? start, DateTime end)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, LpDate) as Week, ISNULL(SUM(ISNULL(LpQuantity,0)),0) as Quantity
								FROM __CRP_FaPlanningLpStatus
								WHERE ArticleId=@articleNr
									{(start.HasValue == false ? "" : " AND @start <= LpDate")}
									AND LpDate <= @end 
									GROUP BY DATEPART(iso_week, LpDate)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetFCQuantitiesByArticle_delayed(int articleNr, DateTime? start, DateTime end)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, FcDate) as Week, SUM(FcQuantity) as Quantity
									FROM __CRP_FaPlanningFcStatus
									WHERE ArtikelNr=@articleNr{(start.HasValue == false ? "" : " AND @start <= FcDate")} AND FcDate <= @end 
									GROUP BY DATEPART(iso_week, FcDate)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}

		public static decimal GetFAQuantitiesByArticle_Residue_delayed(int articleNr, bool? internSiteProd = null)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT ISNULL(SUM(ISNULL([FaQuantity],0)),0)
                               FROM __CRP_FaPlanningFaStatus
                               WHERE FaDate<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE) 
                               AND Artikel_Nr=@articleNr{(!internSiteProd.HasValue ? "" : $" AND {(internSiteProd == true ? $"Lagerort_id=ProductionPlace1_Id" : "Lagerort_id<>ProductionPlace1_Id")}")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static decimal GetFAQuantitiesByArticle_ResidueUBG_delayed(int articleNr, bool internSiteProd = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT ISNULL(SUM(ISNULL(FaQuantity,0)),0)
                               FROM __CRP_FaPlanningUbgFaStatus
                               WHERE Artikel_Nr=@articleNr							   
							   AND Lagerort_ID {(internSiteProd ? "=" : "<>")}ProductionPlace1_Id     
							   AND FaDate<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out decimal d) ? d : 0;
			}
		}
		public static decimal GetABQuantitiesByArticle_Residue_delayed(int articleNr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT ISNULL(SUM(ISNULL(AbQuantity,0)),0)
                               FROM __CRP_FaPlanningAbStatus
                               WHERE [Artikel-Nr]=@articleNr
                               AND AbDate<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static decimal GetLPQuantitiesByArticle_Residue_delayed(int articleNr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT ISNULL(SUM(ISNULL(LpQuantity,0)),0) 
								FROM __CRP_FaPlanningLpStatus
								WHERE ArticleId=@articleNr AND LpDate<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static decimal GetFCQuantitiesByArticle_Residue_delayed(int articleNr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT ISNULL(SUM(ISNULL(FcQuantity,0)),0) as Quantity
								FROM __CRP_FaPlanningFcStatus
								WHERE ArtikelNr=@articleNr AND FcDate<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
		public static decimal GetBedarfByArticle_Residue_delayed(int articleNr, int? lager, bool intern = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT ISNULL(SUM(ISNULL([FaQuantity],0)),0) AS Quantity
									FROM [__CRP_FaPlanningFaStatus]
									WHERE FaDate<=CAST(DATEADD(day,-1-(DATEPART(weekday,GETDATE())+@@DATEFIRST-2)%7,GETDATE()) AS DATE)
									AND Artikel_Nr=@articleNr
									{(intern ? $"{(lager.HasValue ? $" AND [Lagerort_id]={lager.Value}" : "")}" : $"{(lager.HasValue ? $" AND [Lagerort_id]<>{lager.Value}" : "")}")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}

		public static List<KeyValuePair<int, decimal>> GetInternalOrExternalNeeds_delayed(int articleNr, DateTime? start, DateTime end, bool _internal = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DATEPART(iso_week, FaDate) AS Week, ISNULL(SUM(ISNULL(FaQuantity,0)),0) AS Quantity
                               FROM __CRP_FaPlanningUbgFaStatus
                               WHERE Artikel_Nr=@articleNr 
							   AND Lagerort_ID {(_internal ? "=" : "<>")} ProductionPlace1_Id
							   {(start.HasValue ? $"AND FaDate>=@start" : "")}                       
							   AND FaDate<=@end
                               GROUP BY DATEPART(iso_week, FaDate)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("start", start ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("end", end);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					Convert.ToInt32(x["Week"]),
					Convert.ToDecimal(x["Quantity"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int FaPlanningRefreshData(DateTime dateEnd, int userId, SqlConnection connection, SqlTransaction transaction)
		{
			using(var sqlCommand = new SqlCommand("usp_crp_compute_fa_planning", connection, transaction))
			{
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 400;
				sqlCommand.Parameters.AddWithValue("end", dateEnd.ToString("yyyyMMdd"));
				sqlCommand.Parameters.AddWithValue("userId", userId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}

		public static List<Entities.Joins.CRP.FAPlannungArticlesEntity_3> GetArticlesByNummerkreis_delayed(string kreisClause, string artikelnummer, int? deficit, int? unit, bool ubg = false, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			kreisClause = kreisClause ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT DISTINCT [Artikel-Nr],[Artikelnummer], [UBG], [Prio] FROM {(ubg ? "__CRP_FaPlanningUbgArticles" : "__CRP_FaPlanningArticles")} WHERE {kreisClause}";

				if(!string.IsNullOrEmpty(artikelnummer) && !string.IsNullOrWhiteSpace(artikelnummer))
				{
					query += $"AND Artikelnummer LIKE'%{artikelnummer}%'";
				}
				if(deficit.HasValue)
				{
					query += $"AND Prio={deficit}";
				}
				if(unit.HasValue)
				{
					query += $"AND ProductionPlace={unit}";
				}
				query += $"ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName) ? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}" : "[Artikelnummer]")} {(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.FAPlannungArticlesEntity_3(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int GetArticlesByNummerkreisCount_delayed(string kreisClause, string artikelnummer, int? deficit, int? unit, bool ubg = false)
		{
			kreisClause = kreisClause ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM(
                               SELECT DISTINCT [Artikel-Nr],[Artikelnummer], [UBG], [Prio] FROM {(ubg ? "__CRP_FaPlanningUbgArticles" : "__CRP_FaPlanningArticles")} WHERE {kreisClause}";

				if(!string.IsNullOrEmpty(artikelnummer) && !string.IsNullOrWhiteSpace(value: artikelnummer))
				{
					query += $"AND Artikelnummer LIKE '%{artikelnummer}%'";
				}
				if(deficit.HasValue)
				{
					query += $"AND Prio={deficit}";
				}
				if(unit.HasValue)
				{
					query += $"AND ProductionPlace={unit}";
				}
				query += " ) A";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}
		public static Tuple<int, decimal, decimal> GetTotalAndSecurityStockByArticleNrs(int articleNr, List<int> lagerortIds)
		{
			if(articleNr <= 0)
				return new Tuple<int, decimal, decimal>(articleNr, 0, 0);

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Artikel-Nr], SUM(Bestand) Bestand, SUM(Mindestbestand) Mindestbestand FROM [__CRP_FaPlanningLagerStatus] WHERE [Artikel-Nr]={articleNr}{(lagerortIds?.Count > 0 ? $" AND [Lagerort_id] IN ({string.Join(",", lagerortIds)})" : "")} GROUP BY [Artikel-Nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return new Tuple<int, decimal, decimal>(int.TryParse(
					 dataTable.Rows[0][0].ToString(), out var _id) ? _id : 0,
					 decimal.TryParse(dataTable.Rows[0][1].ToString(), out var _bestand) ? _bestand : 0,
					 decimal.TryParse(dataTable.Rows[0][2].ToString(), out var _mbestand) ? _mbestand : 0);
			}
			else
			{
				return new Tuple<int, decimal, decimal>(articleNr, 0, 0);
			}
		}
		public static List<Entities.Joins.CRP.FaPlanningComputeLogsEntity> GetLogs()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CRP_FaPlanningComputeLogs]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.CRP.FaPlanningComputeLogsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.CRP.FaPlanningComputeLogsEntity>();
			}
		}
		public static Entities.Joins.CRP.FaPlanningComputeLogsEntity GetLastLog()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 1 * FROM [__CRP_FaPlanningComputeLogs] ORDER BY Id DESC";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Joins.CRP.FaPlanningComputeLogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Async version
		#region FA Plannung Historie
		public static int FaPlannungHistorieRefreshData(int userId, string username, int typeImportId, string typeImportName)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("usp_crp_historie_fa_plannung", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 400;
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("username", username);
				sqlCommand.Parameters.AddWithValue("TypeImportId", typeImportId);
				sqlCommand.Parameters.AddWithValue("TypeImportName", typeImportName);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion

		#region Helpers
		public static string GetUBGQuerySupplement(DateTime? start, DateTime end, string kreis)
		{
			return $@"SELECT DISTINCT [Artikel-Nr des Bauteils] AS [Artikel-Nr] FROM [Stücklisten] 
                    WHERE [Artikel-Nr des Bauteils] IS NOT NULL
                    {(string.IsNullOrWhiteSpace(kreis) ? "" : $" AND {kreis}")}

                    UNION ALL 

                    SELECT DISTINCT [Artikel-Nr des Bauteils] AS [Artikel-Nr] 
                    FROM __BSD_Stucklisten_Snapshot WHERE [Artikel-Nr des Bauteils] IS NOT NULL
                    {(string.IsNullOrWhiteSpace(kreis) ? "" : $" AND {kreis}")}

                    UNION ALL 

                    SELECT [Artikel-Nr] FROM Artikel WHERE UBG=1
                    {(string.IsNullOrWhiteSpace(kreis) ? "" : $" AND {kreis}")}

                    UNION ALL

                    SELECT DISTINCT A.[Artikel-Nr] FROM Fertigung F inner join Fertigung_Positionen fp
                    ON fp.ID_Fertigung=F.ID
                    INNER JOIN Artikel A ON A.[Artikel-Nr]=fp.Artikel_Nr
                    WHERE Kennzeichen='offen' 							   
                    {(start.HasValue ? $"AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2) >= '{start.Value.ToString("yyyyMMdd")}'" : "")}                       
                    AND COALESCE(Termin_Bestätigt1, Termin_Bestätigt2) <= '{end.ToString("yyyyMMdd")}'
                    AND A.Warengruppe='EF'
                    {(string.IsNullOrWhiteSpace(kreis) ? "" : $" AND {kreis}")}";
		}
		#endregion
		public static List<Infrastructure.Data.Entities.Joins.FAUpdate.FACRPUpdateEntity> GetOpenFAForCRPUpdate(string article, DateTime frozenZoneEnd)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT f.Fertigungsnummer, r.Artikelnummer,f.Kennzeichen,f.gedruckt,f.FA_Druckdatum,f.Anzahl,
								f.KundenIndex,f.Zeit,f.Datum,f.[Termin_Bestätigt1], a.[Angebot-Nr] OrderNumber, a.Typ OrderType, a.Nr OrderId, CASE WHEN f.FA_Gestartet=1 THEN 1 ELSE 0 END IsStarted,
								CASE WHEN f.Termin_Bestätigt1 <= @frozenZoneEnd THEN 1 ELSE 0 END InFrozenZone,
								CASE WHEN (a.Typ='Bedarfsvorschau' OR a.Nr IS NULL) AND f.Termin_Bestätigt1 > @frozenZoneEnd AND (f.FA_Gestartet=0 Or (f.FA_Gestartet) Is Null) THEN 1 ELSE 0 END CanUpdate
								FROM Fertigung f INNER JOIN Artikel r ON f.Artikel_Nr = r.[Artikel-Nr]
								LEFT JOIN [Angebotene Artikel] p on p.[Artikel-Nr]=r.[Artikel-Nr] AND p.[Fertigungsnummer]=f.Fertigungsnummer
								LEFT JOIN [Angebote] a ON a.Nr=p.[Angebot-Nr]
								WHERE f.Kennzeichen='Offen' AND TRIM(r.ArtikelNummer)=@article AND f.Anzahl_erledigt<=0 
								/* 2025-10-02 - Schremmer - show started AND (f.FA_Gestartet=0 Or (f.FA_Gestartet) Is Null) */
								ORDER BY f.gedruckt DESC , f.FA_Druckdatum DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("article", article?.Trim());
				sqlCommand.Parameters.AddWithValue("frozenZoneEnd", frozenZoneEnd);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAUpdate.FACRPUpdateEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static decimal GetROHUpcomingNeededQtyForCRPUpdate(int articleNr, int faId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select ISNULL(SUM(fp.Anzahl),0) as Anzahl
							   from Fertigung f inner join Fertigung_Positionen fp 
							   on f.ID=fp.ID_Fertigung
							   inner join Artikel a on f.artikel_nr=a.[Artikel-Nr]
							   inner join artikel aa on fp.Artikel_Nr=aa.[Artikel-Nr]
							   where f.Termin_Bestätigt1 between GETDATE() and DATEADD(DAY,120,GETDATE()) and f.Kennzeichen='Offen'
							   and f.Anzahl_erledigt<=0 and (f.FA_Gestartet=0 or f.FA_Gestartet is null)
							   and aa.[Artikel-Nr]=@articleNr and fp.ID_Fertigung<>@faId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				sqlCommand.Parameters.AddWithValue("faId", faId);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var d) ? d : 0;
			}
		}
	}
}