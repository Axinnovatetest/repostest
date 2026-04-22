using Infrastructure.Data.Entities.Joins.MGO;
using Infrastructure.Data.Entities.Tables.MGO;
using Infrastructure.Data.Entities.Tables.Statistics;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using static Infrastructure.Data.Entities.Joins.MGO.ProductionOrderChangeHistoryEntity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data.Access.Joins.MGO
{
	public class MainViewsAccess
	{
		#region Needs vs Prod Amounts
		public static List<KeyValuePair<int, string>> GetCTSDashboardCustomersAccess()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT a.Kundennummer, a.Name1 FROM Kunden k Join adressen a ON a.Nr=k.nummer WHERE a.Kundennummer is not null AND a.Adresstyp=1;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x[0].ToString(), out var _x) ? _x : 0, x[1].ToString())).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, string>>();
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardEntity> GetCTSDashboardAccess(DateTime dateTill, string customerName, int maxPrice = 1000)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH TBL_0002 AS (
									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], 
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										   AND ((Angebote.[Vorname/NameFirma]) Like '{customerName}%'))
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)
									SELECT f.Artikel_Nr, a.Artikelnummer, t.sBedarf AS ABBedarf, t.sGesamt ABGesamt, l.Bestand, ISNULL(F.Anzahl,0) OpenFa, 
										l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ImmediatAmount, 
										ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ProductionAmount,
									CASE WHEN t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END) > {maxPrice} THEN 1 ELSE 0 END AS SuspiciousPrice
										FROM 
										   (SELECT [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt from TBL_0002 Where DATET is null or 
										   DATET <='{dateTill.ToString("yyyMMdd")}' GROUP BY [Artikel-Nr]) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager
											where  Lager.[Lagerort_id] not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )


GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <='{dateTill.ToString("yyyMMdd")}' 
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSDashboardEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardEntity>();
			}

		}

		public static string GetCTSDashboardSecAccess(int syncEntity, DateTime dateTill)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
						Declare @dateLastRefresh datetime
						Declare @SyncId int
                        select @SyncId=Id,@dateLastRefresh=SyncDatum
						from stats.Sync where id =(select max(Id) from stats.Sync where syncEntity=@syncEntity)

						select Id,[CustomerGroupClass]
						  ,[CustomerGroupOrder]
						  ,[ImmediatAmount]
						  ,[Results]
						  ,[TotalAmount]
						  ,[ProductionAmount]
							,@dateTill as DateTill
							,@dateLastRefresh as DateLastRefresh

						   ,
						   ( 
						     SELECT Id,Needs as ABBedarf,Total as ABGesamt,ArticleNumber as Artikelnummer,
							ArticleNr as Artikel_Nr,ImmediateAmount as ImmediatAmount,
							ProductionAmount as ProductionAmount,SuspiciousPrice as SuspiciousPrice,Bestand as Bestand,
							OpenFa as OpenFa
							
							 FROM [stats].DashboardArticle
							 WHERE DashboardArticle.DashboardId = [stats].[Dashboard].Id
							 FOR JSON AUTO
							) As SuspiciousArticles
							,
							( 
						     SELECT 
							Name as [CustomerName],
							Name as Name
							 FROM [stats].DashboardCustomer
							 WHERE DashboardCustomer.DashboardId = [stats].[Dashboard].Id
							 FOR JSON AUTO
							) As CustomersData

				  from [stats].[Dashboard] 

				   --right OUTER join [stats].[DashboardCustomer] CustomersData on CustomersData.DashboardId = [Dashboard].Id

				   --right OUTER join [stats].[DashboardArticle] SuspiciousArticles on SuspiciousArticles.DashboardId = [Dashboard].Id

				  where SyncId = @SyncId
				  order by [CustomerGroupClass] asc
				  for json path ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("syncEntity", syncEntity);
				sqlCommand.Parameters.AddWithValue("dateTill", dateTill);

				var jsonResult = new System.Text.StringBuilder();
				var reader = DbExecution.ExecuteReader(sqlCommand);

				while(reader.Read())
				{
					jsonResult.Append(reader.GetValue(0).ToString());
				}
				if(jsonResult is not null)
					return jsonResult.ToString();
				return string.Empty;
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardKwEntity> GetCTSDashboardKwAccess(DateTime dateTill, string customerName, int? articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH TBL_0002 AS (
									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], 
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET,
											YEAR(IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
														[Angebotene Artikel].[Wunschtermin],
														[Angebotene Artikel].[Liefertermin])) AS dYear,
											DATEPART(iso_week, IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
														[Angebotene Artikel].[Wunschtermin],
														[Angebotene Artikel].[Liefertermin])) AS dKW
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) {(!articleId.HasValue ? "" : $"AND [angebotene Artikel].[Artikel-Nr]={articleId.Value}")} 
										   AND ((Angebote.[Vorname/NameFirma]) Like '{customerName}%'))
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)
									SELECT a.Artikelnummer, t.sBedarf AS ABBedarf, t.sGesamt ABGesamt, l.Bestand, ISNULL(F.Anzahl,0) OpenFa, 
										l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ImmediatAmount, 
										ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ProductionAmount, dYear, dKW
										FROM 
										   (SELECT [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt, dYear, dKW from TBL_0002 Where DATET is null or 
										   DATET <='{dateTill.ToString("yyyMMdd")}' GROUP BY [Artikel-Nr], dYear, dKW) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager
											where  Lager.[Lagerort_id] not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )

GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <='{dateTill.ToString("yyyMMdd")}' 
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr] ORDER BY dYear, dKW";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSDashboardKwEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardKwEntity>();
			}

		}



		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardKwEntity> GetCTSDashboardKwSecAccess(DateTime dateTill, string customerName, int? articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"Declare @SyncIdToUse as int
								Declare @SyncDate as date
								SELECT @SyncIdToUse = max(id), @SyncDate = max(SyncDatum) 
						from[stats].[Sync]  where SyncEntity = @SyncEntity
								select ArticleNumber as [Artikelnummer],ArticleNr ,Needs as ABBedarf,Total as ABGesamt, Bestand ,OpenFa,ImmediateAmount as ImmediatAmount,ProductionAmount,[DYear] as DYear,[DKW] as DKw,ArtikelDate
										  from 
										  [stats].DashboardArticle where custumer Like '{customerName}%' {(!articleId.HasValue ? "" : $"AND ArticleNr={articleId.Value}")}  AND SyncId = @SyncIdToUse AND  (ArtikelDate is null or ArtikelDate <='{dateTill.ToString("yyyMMdd")}' )";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("SyncEntity", 6);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSDashboardKwEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardKwEntity>();
			}

		}
		public static List<KeyValuePair<int, string>> GetCTSDashboardArticlesByCustomerAccess(DateTime dateTill, string customerName)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH TBL_0002 AS (
									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], 
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										   AND ((Angebote.[Vorname/NameFirma]) Like '{customerName}%'))
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)
									SELECT DISTINCT a.[Artikel-Nr], a.Artikelnummer
										FROM 
										   (SELECT [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt from TBL_0002 Where DATET is null or 
										   DATET <='{dateTill.ToString("yyyMMdd")}' GROUP BY [Artikel-Nr]) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager
											where [Lager].Lagerort_id not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )
GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <='{dateTill.ToString("yyyMMdd")}' 
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x[0].ToString(), out var _x) ? _x : 0, x[1].ToString())).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, string>>();
			}

		}
		public static CTSDashboardSummaryEntity GetCTSDashboardSummaryByCustomerAccess(DateTime dateTill, string customerName)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH TBL_0002 AS (
									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], 
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										   AND ((Angebote.[Vorname/NameFirma]) Like '{customerName}%'))
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)
									SELECT SUM(t.sGesamt) TotalAmount,
											SUM(l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)))  AS ImmediatAmount,
											SUM(ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END))) AS ProductionAmount
										FROM 
										   (SELECT [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt from TBL_0002 Where DATET is null or 
										   DATET <='{dateTill.ToString("yyyMMdd")}' GROUP BY [Artikel-Nr]) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager
											where [Lager].Lagerort_id not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )

GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <='{dateTill.ToString("yyyMMdd")}' 
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new CTSDashboardSummaryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}

		}
		public static List<KeyValuePair<int, decimal>> GetCTSDashboardTopCustomersAccess(int topNumber, DateTime dateTill)
		{
			var dataTable = new DataTable();
			if(topNumber <= 0 || topNumber > 20)
			{
				topNumber = 10;
			}
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
					SELECT TOP {topNumber} [Kunden-Nr], SUM(p.VKGesamtpreis) TotalAmount from Angebote r join [angebotene Artikel] p on p.[Angebot-Nr]=r.Nr
					WHERE r.Typ LIKE 'Auftrags%' AND r.Datum>'{dateTill.AddYears(-2).ToString("yyyyMMdd")}' AND r.Datum<='{dateTill.ToString("yyyyMMdd")}' AND ISNULL(erledigt,0)=0 AND ISNULL(erledigt_pos,0)=0
					GROUP BY r.[Kunden-Nr] ORDER BY SUM(p.VKGesamtpreis) DESC;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(int.TryParse(x[0].ToString(), out var _x) ? _x : 0,
					decimal.TryParse(x[1].ToString(), out var _v) ? _v : 0)).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, decimal>>();
			}

		}
		public static CTSDashboardSummaryEntity GetCTSDashboardSummaryByCustomerAccess(DateTime dateTill, int customerNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH TBL_0002 AS (
									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], 
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										   AND ((Angebote.[Kunden-Nr]) = '{customerNumber}'))
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)
									SELECT SUM(t.sGesamt) TotalAmount,
											SUM(l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)))  AS ImmediatAmount,
											SUM(ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END))) AS ProductionAmount
										FROM 
										   (SELECT [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt from TBL_0002 Where DATET is null or 
										   DATET <='{dateTill.ToString("yyyMMdd")}' GROUP BY [Artikel-Nr]) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager
											where [Lager].Lagerort_id not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )



GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <='{dateTill.ToString("yyyMMdd")}' 
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new CTSDashboardSummaryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSCustomerGroupEntity> GetCTSCustomersGroupesAccess()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT distinct a.Nr, a.Name1 AS [Name], a.Kundennummer, ISNULL(n.Kunde,'') Kunde, ISNULL(n.Stufe,'') Stufe 
									FROM  Kunden k Join adressen a on k.nummer=a.Nr Left Join [PSZ_Nummerschlüssel Kunde] n ON a.Name1=n.Kunde
									Where a.Kundennummer is not null";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSCustomerGroupEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSCustomerGroupEntity>();
			}

		}

		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardFullEntity> GetCTSCustomersGroupesSecAccess(DateTime syncDatum)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select [KundenKlasse]
								  ,[ArtikelGesamtzahl]
								  ,[KundenGesamtzahl]
								  ,[UsamtzTotal]
								  ,[SofortUmsatz]
								  ,[FAUmsatz]
								  ,[Ergebnis]
								  [SyncDatum]
								  from [stats].[Dashboard]
								  join [stats].[Sync] on [stats].[Dashboard].[SyncId]=[stats].[Sync].[id] where [SyncDatum]>=@SyncDatum";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("SyncDatum", syncDatum);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSDashboardFullEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardFullEntity>();
			}

		}
		public static CTSDashboardSummaryEntity GetCTSDashboardSummaryByCustomerGroupAccess(DateTime dateTill, List<int> customerNumbers)
		{
			if(customerNumbers == null || customerNumbers.Count <= 0)
			{
				return null;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH TBL_0002 AS (
									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], 
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										   AND ((Angebote.[Kunden-Nr]) IN ({string.Join(",", customerNumbers)})))
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)
									SELECT SUM(t.sGesamt) TotalAmount,
											SUM(l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)))  AS ImmediatAmount,
											SUM(ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END))) AS ProductionAmount
										FROM 
										   (SELECT [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt from TBL_0002 Where DATET is null or 
										   DATET <='{dateTill.ToString("yyyMMdd")}' GROUP BY [Artikel-Nr]) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager
											where [Lager].Lagerort_id not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )


GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <='{dateTill.ToString("yyyMMdd")}' 
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new CTSDashboardSummaryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}

		}
		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardEntity> GetCTSDashboardAccess(DateTime dateTill, List<int> customerNumbers, bool onlySuspicious = false, int maxPrice = 1000)
		{
			if(customerNumbers == null || customerNumbers.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH TBL_0002 AS (
									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], 
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										   AND ((Angebote.[Kunden-Nr]) IN ({string.Join(",", customerNumbers)})))
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)
									SELECT t.[Artikel-Nr] AS Artikel_Nr, a.Artikelnummer, t.sBedarf AS ABBedarf, t.sGesamt ABGesamt, l.Bestand, ISNULL(F.Anzahl,0) OpenFa, 
										l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ImmediatAmount, 
										ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ProductionAmount,
									CASE WHEN t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END) > {maxPrice} THEN 1 ELSE 0 END AS SuspiciousPrice
										FROM 
										   (SELECT [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt from TBL_0002 Where DATET is null or 
										   DATET <='{dateTill.ToString("yyyMMdd")}' GROUP BY [Artikel-Nr]) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager
											where [Lager].Lagerort_id not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )
 

											GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <='{dateTill.ToString("yyyMMdd")}' 
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]{(onlySuspicious ? $" WHERE t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END) > {maxPrice}" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSDashboardEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSDashboardEntity>();
			}

		}
		#endregion Needs vs Prod Amount

		#region VK Marge
		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSArticleVKMargeEntity> GetCTSArticleVKMarginAccess(int margin)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"

								Declare @SyncIdToUse as int 
								Declare @SyncDate as date 
								SELECT @SyncIdToUse= max(id),@SyncDate=max(SyncDatum) from [stats].[Sync]  where SyncEntity=@SyncEntity

								SELECT Artikelnummer, SUM_Material_Mit_CU as SUM_Material_Mit_CU, 
									SUM_Material_ohne_CU as SUM_Material_ohne_CU,
									Kalkulatorische_kosten as [Kalkulatorische kosten], 
									EK_Mit_CU as EK_Mit_CU, EK_ohne_CU as EK_ohne_CU, 
									VK_PSZ as [VK PSZ], DB_I_Mit_CU as DB_I_Mit_CU, 
									DB_I_Ohne_CU as DB_I_Ohne_CU, Prozent_Mit_CU as Prozent_Mit_CU, 
									Prozent_Ohne_CU as Prozent_Ohne_CU, Freigabestatus as Freigabestatus,
									@SyncDate as LastSyncDate
								FROM [stats].ArticleVK where SyncId=@SyncIdToUse";

				//string query = $@"SELECT v.Artikelnummer, v.[Summe Material ohne] as SUM_Material_Mit_CU, 
				//					v.[Summe Material mit] as SUM_Material_ohne_CU, v.[Kalkulatorische kosten], 
				//					v.[EK PSZ ohne] as EK_Mit_CU, v.[EK PSZ mit] as EK_ohne_CU, 
				//					v.[VK PSZ], v.[DB I ohne] as DB_I_Mit_CU, 
				//					v.[DB I mit] as DB_I_Ohne_CU, v.[Marge ohne CU] as Prozent_Mit_CU, 
				//					v.[Marge mit CU] as Prozent_Ohne_CU, Artikel.Freigabestatus
				//				FROM [View_PSZ_steinbacher Marge berechnung alle Artikel ergebniss_01] v INNER JOIN
				//					Artikel ON v.Artikelnummer = Artikel.Artikelnummer
				//				WHERE (v.[VK PSZ] <= v.[EK PSZ ohne]) AND
				//				Freigabestatus <>'O' and (UBG =0  or UBG is NULL)  and [Marge ohne CU]<{margin}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("SyncEntity", 3);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSArticleVKMargeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSArticleVKMargeEntity>();
			}

		}
		#endregion VK Marge

		#region VK Marge
		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSProductionFrozenZoneEntity> GetCTSProductionFrozenZoneAccess(int frozenZoneKwCount, bool broughtIntoFZ,
			DateTime? from,
			DateTime? to,
			Settings.PaginModel paging
			)
		{
			if(frozenZoneKwCount <= 0)
			{
				frozenZoneKwCount = 5;
			}
			// - convert to days
			frozenZoneKwCount = frozenZoneKwCount * 7;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				//string query = $@"SELECT F.*, t.LastUpdateDate, DATEPART(ISO_WEEK, t.LastUpdateDate) as lastUpdateKW FROM Fertigung F Join (
				//					SELECT fertigungsnummer, max(Änderungsdatum) LastUpdateDate FROM PSZ_Historique_Import_Excel_FA
				//					WHERE fertigungsnummer in (SELECT fertigungsnummer FROM fertigung WHERE Kennzeichen='offen')
				//					GROUP BY fertigungsnummer
				//					) as t on t.fertigungsnummer=f.fertigungsnummer
				//					";

				//if(broughtIntoFZ)
				//{
				//	// - /* -- Bring FAs INTO FrozenZone -- */ //
				//	query += $@"WHERE F.Termin_Ursprünglich>DATEADD(DAY, {frozenZoneKwCount}, LastUpdateDate) /* - OUT of frozenZone - */
				//					AND F.Termin_Bestätigt1<=DATEADD(DAY, {frozenZoneKwCount}, LastUpdateDate); /* - IN frozenZone - */";
				//} else
				//{
				//	// - /* -- Reschedule FAs OUT OF FrozenZone -- */ //
				//	query += $@"WHERE  F.Termin_Ursprünglich<=DATEADD(DAY, {frozenZoneKwCount}, LastUpdateDate) /* - IN frozenZone - */
				//					AND F.Termin_Bestätigt1>DATEADD(DAY, {frozenZoneKwCount}, LastUpdateDate); /* - OUT of frozenZone - */";
				//}



				string query = $@"Declare @SyncIdToUse as int 
									Declare @SyncDate as date 
									SELECT @SyncIdToUse= max(id),@SyncDate=max(SyncDatum) from [stats].[Sync]  where SyncEntity=@SyncEntity 
									SELECT *,@SyncDate as SyncDate,Count(*) over() as TotalCount from [stats].ProductionFrozen ";

				if(broughtIntoFZ)
				{
					// - /* -- Bring FAs INTO FrozenZone -- */ //
					query += $@"WHERE Termin_Ursprunglich>DATEADD(DAY, {frozenZoneKwCount}, LastUpdateDate) AND  Termin_Bestatigt1<=DATEADD(DAY, {frozenZoneKwCount}, LastUpdateDate)";
				}
				else
				{
					// - /* -- Reschedule FAs OUT OF FrozenZone -- */ //
					query += $@"WHERE Termin_Ursprunglich<=DATEADD(DAY, {frozenZoneKwCount}, LastUpdateDate) AND Termin_Bestatigt1>DATEADD(DAY, {frozenZoneKwCount}, LastUpdateDate)";
				}
				if(from.HasValue)
				{
					query += $@" AND LastUpdateDate>=@dateFrom";
				}
				if(to.HasValue)
				{
					query += $@" AND LastUpdateDate<=@dateTo";
				}
				query += $@" and SyncId=@SyncIdToUse";

				if(paging != null)
				{
					if(0 >= paging.RequestRows || paging.RequestRows > 100)
						paging.RequestRows = 100;
					query += $" order by id asc OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				if(from.HasValue)
				{
					sqlCommand.Parameters.AddWithValue("dateFrom", from.Value.Date);
				}
				if(to.HasValue)
				{
					sqlCommand.Parameters.AddWithValue("dateTo", to.Value.Date);
				}
				sqlCommand.Parameters.AddWithValue("SyncEntity", 2);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSProductionFrozenZoneEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSProductionFrozenZoneEntity>();
			}

		}
		#endregion VK Marge

		#region Beadrf Analyse
		public static List<Infrastructure.Data.Entities.Joins.MGO.CTSBedarfEntity> GetCTSBedarfAnalyseAccess(DateTime dateTill, bool isExtra)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";

				if(isExtra)
				{
					// - /* --  Analyse Bestand - Bedarf ( zu Viel bestellt ) -- */ //
					query = $@"SELECT top 500 t2.*, t1.ROH_Quantity, ISNULL(ROH_Quantity,0) * Einkaufspreis  as Wert_LagerBestandBedarf,
								(ROH_Bestand - ISNULL(ROH_Quantity,0)) DiffQuantity, (ROH_Bestand - ISNULL(ROH_Quantity,0)) * Einkaufspreis as DiffPrice 
								FROM (SELECT Artikelnummer, SUM(Bestand) as ROH_Bestand, (Einkaufspreis), SUM(Bestand*(Einkaufspreis)) as Gesamtpreis,Name1,[Bestell-Nr] FROM (
								SELECT a.Name1,Bestellnummern.[Bestell-Nr], Artikel.Artikelnummer, Bestellnummern.Standardlieferant, Lager.Bestand+Lager.Bestand_reserviert as Bestand ,
								Lager.Lagerort_id, Artikel.Warengruppe, Bestellnummern.Einkaufspreis
								FROM Artikel INNER JOIN Lager
								ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr] 
								AND [Lager].Lagerort_id not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )

								INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr] Join adressen a on a.Nr=Bestellnummern.[Lieferanten-Nr]
								WHERE (Artikel.Warengruppe <> N'EF') aND Bestellnummern.Standardlieferant=1
								) AS Tmp 
								GROUP BY Artikelnummer,Einkaufspreis,Name1,[Bestell-Nr]
								) t2 
								Left Join (SELECT Artikelnummer, SUM(ROH_quantity) as ROH_Quantity FROM
								(SELECT Artikel.Artikelnummer, Fertigung_Positionen.Anzahl PositionAnzahl, Fertigung.Kennzeichen, Fertigung.FA_Gestartet, Fertigung.Fertigungsnummer, 
								Fertigung.Originalanzahl, Fertigung.Anzahl, Fertigung_Positionen.Anzahl / Fertigung.Originalanzahl * Fertigung.Anzahl AS ROH_Quantity, Fertigung.Lagerort_id
								FROM Fertigung 
								INNER JOIN Fertigung_Positionen ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung_HL 
								INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
								WHERE (Fertigung.Kennzeichen = N'Offen') and (Fertigung.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}')
								) AS Tmp GROUP BY Artikelnummer
								) t1 on t1.Artikelnummer=t2.Artikelnummer";
				}
				else
				{
					// - /* -- Analyse Bedarf - Bestand (Missing ROH) -- */ //
					query += $@"SELECT top 500 t1.*, t2.ROH_Quantity, ISNULL(ROH_Quantity,0) * Einkaufspreis  as Wert_LagerBestandBedarf,
								(ROH_Bestand - ISNULL(ROH_Quantity,0)) DiffQuantity, (ROH_Bestand - ISNULL(ROH_Quantity,0)) * Einkaufspreis as DiffPrice 
								FROM (SELECT Artikelnummer, SUM(ROH_quantity) as ROH_Quantity FROM
								(
								SELECT Artikel.Artikelnummer, Fertigung_Positionen.Anzahl PositionAnzahl, Fertigung.Kennzeichen, Fertigung.FA_Gestartet, Fertigung.Fertigungsnummer, 
								Fertigung.Originalanzahl, Fertigung.Anzahl, Fertigung_Positionen.Anzahl / Fertigung.Originalanzahl * Fertigung.Anzahl AS ROH_Quantity, Fertigung.Lagerort_id
								FROM Fertigung 
									INNER JOIN Fertigung_Positionen ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung_HL 
									INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
								WHERE (Fertigung.Kennzeichen = N'Offen') and (Fertigung.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}')
								) AS Tmp GROUP BY Artikelnummer
								) t2 
								Left Join (SELECT Artikelnummer, SUM(Bestand) as ROH_Bestand, (Einkaufspreis), SUM(Bestand*(Einkaufspreis)) as Gesamtpreis,Name1,[Bestell-Nr] FROM (
								SELECT a.Name1,Bestellnummern.[Bestell-Nr], Artikel.Artikelnummer, Bestellnummern.Standardlieferant, Lager.Bestand+Lager.Bestand_reserviert as Bestand, 
								Lager.Lagerort_id, Artikel.Warengruppe, Bestellnummern.Einkaufspreis
								FROM Artikel 
									INNER JOIN Lager




								ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr] 
								AND [Lager].Lagerort_id not in 
											  (
												SELECT  [Lagerort_id]
												FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
											  )

									INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
									Join adressen a on a.Nr=Bestellnummern.[Lieferanten-Nr]
								WHERE (Artikel.Warengruppe <> N'EF') aND Bestellnummern.Standardlieferant=1
								) AS Tmp GROUP BY Artikelnummer,Einkaufspreis,Name1,[Bestell-Nr]
								) t1 on t1.Artikelnummer=t2.Artikelnummer Where ISNULL((ROH_Bestand - ISNULL(ROH_Quantity,0)),-1)<0";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSBedarfEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MGO.CTSBedarfEntity>();
			}

		}
		static string getCronQuery(int cronId)
		{
			string query = "";
			//
			switch(cronId)
			{
				// - 1-Dashboard
				case 1:
					query = $@"
insert into 
  [stats].[Sync]
  values (getdate(),1)

DROP TABLE IF EXISTS #ClientTable;

CREATE TABLE #ClientTable
(
    /* columns returned by the stat function that will match the PeriodicSales */
	[ClientNumber] int,
	[Stufe] nvarchar(10),
	[Name]  nvarchar(250)
)

INSERT INTO #ClientTable

SELECT 
/*distinct a.Nr, a.Name1 AS [Name], a.Kundennummer, ISNULL(n.Kunde,'') Kunde, ISNULL(n.Stufe,'')*/
 distinct a.Nr ClientNumber, ISNULL(n.Stufe,'') Stufe ,a.Name1 AS [Name]
									FROM  Kunden k Join adressen a on k.nummer=a.Nr Left Join [PSZ_Nummerschlüssel Kunde] n ON a.Name1=n.Kunde
									/* --Where a.Kundennummer is not null and n.stufe is not null and n.Stufe <> '' --*/


;
/* --- Insert into dashboard table -- */
WITH TBL_0002 AS (
				SELECT ClientTable.Stufe, [Artikel-Nr], Artikelnummer, Sum(Anzahl) AS [Bedarf AB], 
						Sum(Gesamtpreis) AS SummevonGesamtpreis, 
						IIf((PP.[Liefertermin]>=2200 OR PP.[Liefertermin] IS NULL),
								PP.[Wunschtermin],
								PP.[Liefertermin]) AS DATET
				FROM #ClientTable ClientTable  
					left join (SELECT * FROM Angebote WHERE Typ Like 'Auftrags%' AND erledigt=0) as A  on ClientTable.[clientNumber]=A.[Kunden-Nr]
					left JOIN (SELECT P.*, Artikel.Artikelnummer FROM (SELECT * FROM [angebotene Artikel] WHERE erledigt_pos=0) AS P INNER JOIN Artikel ON P.[Artikel-Nr] = Artikel.[Artikel-Nr]) AS PP  ON A.Nr = PP.[Angebot-Nr]
						
				GROUP BY PP.[Artikel-Nr], PP.Artikelnummer, ClientTable.[clientNumber], ClientTable.[Stufe],
						IIf((PP.[Liefertermin]>=2200 OR PP.[Liefertermin] IS NULL),
								PP.[Wunschtermin],
								PP.[Liefertermin])
				/* ---HAVING ClientTable.Stufe='KKM' ---AND Sum(PP.Gesamtpreis)<>0 -- */
				)		
				

					/* -- insert into dashboard table -- */
				insert into [stats].[Dashboard]([CustomerGroupClass]
				,[TotalAmount]
				,[ImmediatAmount]
				,[ProductionAmount]
				/*,[CustomerGroupOrder] */
				,[Results]
				,[SyncId])

				SELECT t.Stufe,
						SUM(t.sGesamt) TotalAmount,
						SUM(l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)))  AS ImmediatAmount,
						SUM(ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END))) AS ProductionAmount,
						/* --- the result will be the sum of ImmediatAmount and productionAmount */
						SUM(l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)))
						+
						SUM(ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END))) as Results,
						
						/* 0 */
						(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')
						FROM 
						(
								SELECT DATET,[Stufe],[Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt 
								from TBL_0002 
								Where DATET is null or  DATET <=getdate()
								GROUP BY DATET,[Stufe],[Artikel-Nr]
						)
						as t
						left Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]

						Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager 
						where Lagerort_id not in 
						  (
							SELECT  [Lagerort_id]
							FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
						  )
						GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
						Left Join 
						(
							SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null
							or Termin_Bestätigt1 <=getdate()
							GROUP BY  [Artikel_Nr]
						) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]

						Group by  t.Stufe;
						
						



/* Insert into dashboard article table */
WITH TBL_0002 AS (

									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], 
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET,
												 Angebote.[Kunden-Nr] as Kunden_Nr
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
										   inner join #ClientTable ClientTable on ClientTable.[clientNumber]=Angebote.[Kunden-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) )
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer,Angebote.[Kunden-Nr],
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)

									insert into [stats].DashboardArticle(
									ArticleNr,ArticleNumber,Needs,Total,Bestand,OpenFa,ImmediateAmount,ProductionAmount,SuspiciousPrice,DashboardId
									)

									SELECT 
									t.[Artikel-Nr] AS ArticleNr, 
									a.Artikelnummer as ArticleNumber, t.sBedarf as Needs, t.sGesamt as Total, l.Bestand, ISNULL(F.Anzahl,0) OpenFa, 
										l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ImmediateAmount, 
										ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ProductionAmount,
									CASE WHEN t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END) > 1000 THEN 1 ELSE 0 END AS SuspiciousPrice,
								/*	1 */
										(
											Select id 
											from [stats].[Dashboard] 
											where syncId= (Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT') 
											and Kunden_Nr in 
											(
												select ClientNumber
												from  #ClientTable
												where Stufe=[stats].[Dashboard].CustomerGroupClass
											)
										)

										FROM 
										   (SELECT [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt ,
										   Kunden_Nr
										   from TBL_0002 Where DATET is null or 
										   DATET <=
										   GETDATE() 
										   GROUP BY [Artikel-Nr],Kunden_Nr) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <=
										   
										   GETDATE()
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]  WHERE t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END) > 1000
										   
										   /* Insert into Dashboard customer table */
insert into [stats].DashboardCustomer
([Name],DashboardId)
select [Name],(select Id from [stats].Dashboard where CustomerGroupClass=#ClientTable.Stufe and SyncId=(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')) DashboardIdSec
from #ClientTable

/* Delete extra lines */
delete from [stats].DashboardCustomer  where dashboardId is null

";
					break;
				// - 2-CronFrozenProducts
				case 2:
					query = $@"
/* --delete from  stats.ProductionFrozen */
insert into 
  [stats].[Sync]
  values (getdate(),2)
					
Declare @SyncIdUsed as int =(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')

							insert into stats.ProductionFrozen
							(Artikel_Nr,Anzahl,Originalanzahl,Anzahl_erledigt,Anzahl_aktuell,Angebot_nr,Angebot_Artikel_Nr,AnzahlnachgedrucktPPS,Ausgangskontrolle,
							Bemerkung,Bemerkung_II_Planung,
							Bemerkung_ohne_statte,
							Bemerkung_Kommissionierung_AL,
							Bemerkung_Planung,
							Bemerkung_Technik,
							Bemerkung_zu_Prio,
							BomVersion,
							CAO,
							Check_FAbegonnen,
							Check_Gewerk1,
							Check_Gewerk1_Teilweise,
							Check_Gewerk2,
							Check_Gewerk2_Teilweise,
							Check_Gewerk3,
							Check_Gewerk3_Teilweise,
							Check_Kabelgeschnitten,
							CPVersion,
							Datum,
							Endkontrolle,
							Erledigte_FA_Datum,
							Erstmuster,
							FA_begonnen,
							FA_Druckdatum,
							FA_Gestartet,
							Fa_NachdruckPPS,
							Fertigungsnummer,
							gebucht,
							gedruckt,
							Gewerk_1,
							Gewerk_2,
							Gewerk_3,
							Gewerk_Teilweise_Bemerkung,
							GrundNachdruckPPS,
							HBGFAPositionId,
							ID_Hauptartikel,
							ID_Rahmenfertigung,
							Kabel_geschnitten,
							Kabel_geschnitten_Datum,
							Kennzeichen,
							Kommisioniert_komplett,
							Kommisioniert_teilweise,
							Kunden_Index_Datum,
							KundenIndex,
							Lagerort_id,
							Lagerort_id_zubuchen,
							LastUpdateDate,
							Mandant,
							Letzte_Gebuchte_Menge,
							Menge1,
							Menge2,
							Preis,Planungsstatus,Prio,Quick_Area,ROH_umgebucht,Spritzgiesserei_abgeschlossen,
							Tage_Abweichung,Technik,


							Techniker,Termin_Bestatigt1,Termin_Bestatigt2,Termin_Fertigstellung,
							Termin_Material,Termin_Ursprunglich,Termin_voranderung,
							UBG,
							UBGTransfer ,
							Urs_Artikelnummer ,
							Urs_Fa ,
							Zeit ,
							lastUpdateKW ,
							TotalCount,
							SyncId
							)
SELECT
F.Artikel_Nr,F.Anzahl,F.Originalanzahl,F.Anzahl_erledigt,F.Anzahl_aktuell,F.Angebot_nr,F.Angebot_Artikel_Nr,F.AnzahlnachgedrucktPPS,F.Ausgangskontrolle
,F.Bemerkung,F.[Bemerkung II Planung],F.[Bemerkung ohne stätte],F.Bemerkung_Kommissionierung_AL,F.Bemerkung_Planung,F.Bemerkung_Technik,F.Bemerkung_zu_Prio,
F.BomVersion,F.CAO,F.Check_FAbegonnen,F.Check_Gewerk1,F.Check_Gewerk1_Teilweise,Check_Gewerk2,F.Check_Gewerk2_Teilweise,F.Check_Gewerk3,
F.Check_Gewerk3_Teilweise,F.Check_Kabelgeschnitten,F.CPVersion,F.Datum,F.Endkontrolle,F.Erledigte_FA_Datum,F.Erstmuster,F.FA_begonnen,
F.FA_Druckdatum,F.FA_Gestartet,F.[Fa-NachdruckPPS],F.Fertigungsnummer,F.gebucht,F.gedruckt,F.[Gewerk 1],F.[Gewerk 2],F.[Gewerk 3],
F.Gewerk_Teilweise_Bemerkung,F.GrundNachdruckPPS,F.HBGFAPositionId,F.ID_Hauptartikel,F.ID_Rahmenfertigung,
F.Kabel_geschnitten,F.Kabel_geschnitten_Datum,F.Kennzeichen,F.Kommisioniert_komplett,F.Kommisioniert_teilweise,F.Kunden_Index_Datum,
F.KundenIndex,F.Lagerort_id,F.[Lagerort_id zubuchen],t.LastUpdateDate,F.Mandant,F.Letzte_Gebuchte_Menge,
F.Menge1,F.Menge2,F.Preis,F.Planungsstatus,F.Prio,F.Quick_Area,F.ROH_umgebucht,F.[Spritzgießerei_abgeschlossen],F.[Tage Abweichung],F.Technik,

F.Techniker,F.[Termin_Bestätigt1],F.[Termin_Bestätigt2],F.[Termin_Fertigstellung],
F.Termin_Material,F.[Termin_Ursprünglich],F.[Termin_voränderung],
UBG,
UBGTransfer ,
F.[Urs-Artikelnummer] ,
F.[Urs-Fa] ,
Zeit ,
DATEPART(ISO_WEEK, t.LastUpdateDate) as lastUpdateKW  ,
Count(*) over() as TotalCount,
@SyncIdUsed
/* --F.* */
/* --, DATEPART(ISO_WEEK, t.LastUpdateDate) as lastUpdateKW */
FROM Fertigung F Join (
			SELECT fertigungsnummer, max(Änderungsdatum) LastUpdateDate FROM PSZ_Historique_Import_Excel_FA
					WHERE fertigungsnummer in (SELECT fertigungsnummer FROM fertigung WHERE Kennzeichen='offen')
							GROUP BY fertigungsnummer
							) as t on t.fertigungsnummer=f.fertigungsnummer
							


";
					break;
				// - 3-ArticleFK
				case 3:
					query = $@"
insert into 
  [stats].[Sync]
  values (getdate(),3)
					
Declare @SyncIdUsed as int =(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')

INSERT INTO [stats].ArticleVK
	(
		Artikelnummer,
		SUM_Material_Mit_CU,
		SUM_Material_ohne_CU,
		Kalkulatorische_kosten,
		EK_Mit_CU,
		EK_ohne_CU,
		VK_PSZ,
		DB_I_Mit_CU,
		DB_I_Ohne_CU,
		Prozent_Mit_CU,
		Prozent_Ohne_CU,
		Freigabestatus,
		SyncId
	)


SELECT v.Artikelnummer, v.[Summe Material ohne] as SUM_Material_Mit_CU, 
									v.[Summe Material mit] as SUM_Material_ohne_CU, 
									v.[Kalkulatorische kosten] as Kalkulatorische_kosten, 
									v.[EK PSZ ohne] as EK_Mit_CU,
									v.[EK PSZ mit] as EK_ohne_CU, 
									v.[VK PSZ] as VK_PSZ,
									v.[DB I ohne] as DB_I_Mit_CU, 
									v.[DB I mit] as DB_I_Ohne_CU,
									v.[Marge ohne CU] as Prozent_Mit_CU, 
									v.[Marge mit CU] as Prozent_Ohne_CU, 
									Artikel.Freigabestatus,
									@SyncIdUsed
								FROM [View_PSZ_steinbacher Marge berechnung alle Artikel ergebniss_01] v INNER JOIN
									Artikel ON v.Artikelnummer = Artikel.Artikelnummer
								WHERE (v.[VK PSZ] <= v.[EK PSZ ohne]) AND
								Freigabestatus <>'O' and (UBG =0  or UBG is NULL) 
";
					break;
				// - 4-BedrafExtra
				case 4:
					query = $@"
insert into 
  [stats].[Sync]
  values (getdate(),4)
					
Declare @SyncIdUsed as int =(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')


Insert into [stats].BedarfAnalyse
(
	Artikelnummer,ROH_Bestand,Einkaufspreis,Gesamtpreis,Name1,Bestell_Nr,ROH_Quantity,Wert_LagerBestandBedarf,
		DiffQuantity,
		DiffPrice,
		TerminBestatigt,
		SyncId
)

SELECT t2.*, t1.ROH_Quantity, ISNULL(ROH_Quantity,0) * Einkaufspreis  as Wert_LagerBestandBedarf,
								(ROH_Bestand - ISNULL(ROH_Quantity,0)) DiffQuantity, (ROH_Bestand - ISNULL(ROH_Quantity,0)) * Einkaufspreis as DiffPrice 
								,t1.TerminBestatigt
								,@SyncIdUsed
								FROM (SELECT Artikelnummer, SUM(Bestand) as ROH_Bestand, (Einkaufspreis), SUM(Bestand*(Einkaufspreis)) as Gesamtpreis,Name1,[Bestell-Nr] 
								FROM (
								SELECT a.Name1,Bestellnummern.[Bestell-Nr], Artikel.Artikelnummer, Bestellnummern.Standardlieferant, Lager.Bestand+Lager.Bestand_reserviert 
								as Bestand ,
								Lager.Lagerort_id, Artikel.Warengruppe, Bestellnummern.Einkaufspreis
								FROM Artikel INNER JOIN  Lager 
								

								ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr] 
								and Lager.[Lagerort_id] not in 
								  (
									SELECT  [Lagerort_id]
									FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
								  )
								
								INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr] Join adressen a on a.Nr=Bestellnummern.[Lieferanten-Nr]
								WHERE (Artikel.Warengruppe <> N'EF') aND Bestellnummern.Standardlieferant=1
								) AS Tmp 
								GROUP BY Artikelnummer,Einkaufspreis,Name1,[Bestell-Nr]
								) t2 
								Left Join (SELECT Artikelnummer, SUM(ROH_quantity) as ROH_Quantity,TerminBestatigt FROM
								(SELECT Artikel.Artikelnummer, Fertigung_Positionen.Anzahl PositionAnzahl, Fertigung.Kennzeichen,
								Fertigung.FA_Gestartet, Fertigung.Fertigungsnummer, 
								Fertigung.Originalanzahl, Fertigung.Anzahl, Fertigung_Positionen.Anzahl / Fertigung.Originalanzahl * Fertigung.Anzahl 
								AS ROH_Quantity, Fertigung.Lagerort_id
								,Fertigung.Termin_Bestätigt1 as TerminBestatigt
								FROM Fertigung 
								INNER JOIN Fertigung_Positionen ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung_HL 
								INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
								WHERE (Fertigung.Kennzeichen = N'Offen') and (Fertigung.Termin_Bestätigt1<=getdate())
								) AS Tmp GROUP BY Artikelnummer,TerminBestatigt
								) t1 on t1.Artikelnummer=t2.Artikelnummer

";
					break;
				// - 5-BedrafNonExtra
				case 5:
					query = $@"
insert into 
  [stats].[Sync]
  values (getdate(),5)
					
Declare @SyncIdUsed as int =(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')


Insert into [stats].BedarfAnalyse
(
	Artikelnummer,ROH_Bestand,Einkaufspreis,Gesamtpreis,Name1,Bestell_Nr,ROH_Quantity,Wert_LagerBestandBedarf,
		DiffQuantity,
		DiffPrice,
		TerminBestatigt,

		SyncId
)



SELECT   t1.*, t2.ROH_Quantity, ISNULL(ROH_Quantity,0) * Einkaufspreis  as Wert_LagerBestandBedarf,
								(ROH_Bestand - ISNULL(ROH_Quantity,0)) DiffQuantity, (ROH_Bestand - ISNULL(ROH_Quantity,0)) * Einkaufspreis as DiffPrice 
								,t2.TerminBestatigt
								,@SyncIdUsed 
								FROM (SELECT Artikelnummer, SUM(ROH_quantity) as ROH_Quantity,TerminBestatigt FROM
								(
								SELECT Artikel.Artikelnummer, Fertigung_Positionen.Anzahl PositionAnzahl, Fertigung.Kennzeichen, Fertigung.FA_Gestartet, Fertigung.Fertigungsnummer, 
								Fertigung.Originalanzahl, Fertigung.Anzahl, Fertigung_Positionen.Anzahl / Fertigung.Originalanzahl * Fertigung.Anzahl AS ROH_Quantity, Fertigung.Lagerort_id
								,Fertigung.Termin_Bestätigt1 as TerminBestatigt

								FROM Fertigung 
									INNER JOIN Fertigung_Positionen ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung_HL 
									INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
								WHERE (Fertigung.Kennzeichen = N'Offen') and (Fertigung.Termin_Bestätigt1<=getdate())
								) AS Tmp GROUP BY Artikelnummer,TerminBestatigt
								) t2 
								Left Join (SELECT Artikelnummer, SUM(Bestand) as ROH_Bestand, (Einkaufspreis), SUM(Bestand*(Einkaufspreis)) as Gesamtpreis,Name1,[Bestell-Nr] FROM (
								SELECT a.Name1,Bestellnummern.[Bestell-Nr], Artikel.Artikelnummer, Bestellnummern.Standardlieferant, Lager.Bestand+Lager.Bestand_reserviert as Bestand, 
								Lager.Lagerort_id, Artikel.Warengruppe, Bestellnummern.Einkaufspreis
								FROM Artikel 
									INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr] 
									and Lagerort_id not in 
									  (
										SELECT  [Lagerort_id]
										FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
									  )
									INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
									Join adressen a on a.Nr=Bestellnummern.[Lieferanten-Nr]
								WHERE (Artikel.Warengruppe <> N'EF') aND Bestellnummern.Standardlieferant=1
								) AS Tmp GROUP BY Artikelnummer,Einkaufspreis,Name1,[Bestell-Nr]
								) t1 on t1.Artikelnummer=t2.Artikelnummer Where ISNULL((ROH_Bestand - ISNULL(ROH_Quantity,0)),-1)<0


";
					break;
				// - 6-CTSDashboard
				case 6:
					query = $@"

insert into 
  [stats].[Sync]
  values (getdate(),6);

WITH TBL_0002 AS (
									SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Sum([angebotene Artikel].Anzahl) AS [Bedarf AB], [angebotene Artikel].Nr as AngeboteneArtikelNr,
										   Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis, 
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin]) AS DATET,
												 

												 			YEAR(IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
														[Angebotene Artikel].[Wunschtermin],
														[Angebotene Artikel].[Liefertermin])) AS dYear,
											DATEPART(iso_week, IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
														[Angebotene Artikel].[Wunschtermin],
														[Angebotene Artikel].[Liefertermin])) AS DKw
												 
												 ,Angebote.[Vorname/NameFirma]  as custumer
									FROM Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
										   ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										   /* --AND ( */
										   /* --Artikel.Artikelnummer Like '825-035-01AL' */
										   /* --) */
										   )
									GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, Angebote.[Vorname/NameFirma],
									[angebotene Artikel].Nr,
										   IIf(([Angebotene Artikel].[Liefertermin]>=2200 OR [Angebotene Artikel].[Liefertermin] IS NULL),
												 [Angebotene Artikel].[Wunschtermin],
												 [Angebotene Artikel].[Liefertermin])
									HAVING (((Sum([angebotene Artikel].Gesamtpreis))<>0))
									)




									insert into [stats].DashboardArticle(
									ArticleNr,ArticleNumber,Needs,Total,Bestand,OpenFa,ImmediateAmount,ProductionAmount,SuspiciousPrice,Custumer,
									DYear,DKw,
									ArtikelDate,
									DashboardId,SyncId
									)

									SELECT distinct f.Artikel_Nr as ArticleNr,
									a.Artikelnummer  ArticleNumber,
									t.sBedarf  Needs,
									t.sGesamt Total, 
									l.Bestand, 
									ISNULL(F.Anzahl,0) OpenFa, 
										l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ImmediateAmount, 
										ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)) AS ProductionAmount,
									CASE WHEN t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END) > 1000 THEN 1 ELSE 0 END AS SuspiciousPrice,
									t.custumer,
									t.dYear,
									t.DKw,
									F.Termin_Bestätigt1 as ArtikelDate,
									null,
									(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')
									FROM 
										(
											SELECT distinct [Artikel-Nr], sum([Bedarf AB]) sBedarf, cast(sum(SummevonGesamtpreis) as decimal(38,6)) sGesamt,custumer,
												dYear,DKw,DATET,AngeboteneArtikelNr
											from TBL_0002 
											Where 
												/* --DATET is null or */
												DATET <=GETDATE()
											GROUP BY [Artikel-Nr],custumer,dYear,DKw,DATET,AngeboteneArtikelNr
										) as t
										Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
									   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM  Lager 
										where Lagerort_id not in 
										  (
											SELECT  [Lagerort_id]
											FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
										  )

									   GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
									   Left Join 
											(
												SELECT [Artikel_Nr],Termin_Bestätigt1, SUM(ISNULL(Anzahl,0)) Anzahl 
												FROM Fertigung 
												Where 
													Termin_Bestätigt1 is null or 
													(Termin_Bestätigt1 >=dateadd(year, -1, getdate()) and Termin_Bestätigt1 <=dateadd(month, 3, getdate()) )
												GROUP BY [Artikel_Nr],Termin_Bestätigt1
											) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr] --and F.Termin_Bestätigt1=t.DATET
											order by ArtikelDate

										 /* -- SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager where [Artikel-Nr] Like '985-003-01' group by [Artikel-Nr],Bestand */
";
					break;
				default:
					break;
			}

			// 
			return query;
		}
		public static void RefreshData(int cronSql)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand(getCronQuery(cronSql), sqlConnection))
			{
				sqlConnection.Open();
				DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}

		public static CTSRefreshEntity CheckDataRefreshDate(int cronSql)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT max([SyncDatum]) as RefreshDate
					          FROM [stats].[Sync] where SyncEntity=@SyncEntity";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("SyncEntity", cronSql);
				DbExecution.ExecuteNonQuery(sqlCommand);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MGO.CTSRefreshEntity(x)).FirstOrDefault();
			}
			else
			{
				return new CTSRefreshEntity();
			}
		}
		public static List<BedarfAnalyseEntity> GetCTSBedarfAnalyseSecAccess(DateTime dateTill, int statType)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"Declare @SyncIdToUse as int 
									Declare @SyncDate as date 
									SELECT @SyncIdToUse= max(id),@SyncDate=max(SyncDatum) from [stats].[Sync]  where SyncEntity=@SyncEntity
	
									SELECT top(1000)
									Artikelnummer,
									ROH_Bestand,
									Einkaufspreis,
									Gesamtpreis,
									Name1,
									Bestell_Nr,
									ROH_Quantity,
									Wert_LagerBestandBedarf,
									DiffQuantity,
									DiffPrice,
									TerminBestatigt,
									SyncId,
									@SyncDate as LastSyncDate
									from [stats].BedarfAnalyse
									where 

SyncId=@SyncIdToUse and [Artikelnummer] is not null";
				//TerminBestatigt<=@dateTill and 
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateTill", dateTill);
				sqlCommand.Parameters.AddWithValue("SyncEntity", statType);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new BedarfAnalyseEntity(x)).ToList();
			}
			else
			{
				return new List<BedarfAnalyseEntity>();
			}
		}
		public static List<BedarfAnalyseEntity> GetCTSBedarfAnalyseSecAccess(DateTime dateTill, int statType, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"Declare @SyncIdToUse as int 
									Declare @SyncDate as date 
									SELECT @SyncIdToUse= max(id),@SyncDate=max(SyncDatum) from [stats].[Sync]  where SyncEntity=@SyncEntity
	
									SELECT count(*) over () as TotalCount,
									Artikelnummer,
									ROH_Bestand,
									Einkaufspreis,
									Gesamtpreis,
									Name1,
									Bestell_Nr,
									ROH_Quantity,
									Wert_LagerBestandBedarf,
									DiffQuantity,
									DiffPrice,
									TerminBestatigt,
									SyncId,
									@SyncDate as LastSyncDate
									from [stats].BedarfAnalyse
									where 
									SyncId=@SyncIdToUse and [Artikelnummer] is not null";

				if(paging != null)
				{
					query += $" ORDER BY Artikelnummer OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				//TerminBestatigt<=@dateTill and 
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateTill", dateTill);
				sqlCommand.Parameters.AddWithValue("SyncEntity", statType);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new BedarfAnalyseEntity(x)).ToList();
			}
			else
			{
				return new List<BedarfAnalyseEntity>();
			}
		}

		public static List<ProductionOrderChangeHistoryEntity> GetProductionOrderHistory(DateTime changeDateFrom, DateTime changeDateTo, int warehouseId, bool? inFrozen,bool? outFR, string status = "")
		{
			var dataTable = new DataTable();
			var tableName = "dbo.[uvw_ProductionOrderChangeHistory]";
			if(inFrozen.HasValue || outFR.HasValue)
			{
				if(inFrozen.Value)
				{
					tableName = "dbo.[uvw_ProductionOrderChangeHistory_BroughtInFZ]";
				}
				if(outFR.Value )
				{
					tableName = "dbo.[uvw_ProductionOrderChangeHistory_SentOutFZ]";
				}
			}
			var query = $@"SELECT  faInFz.ProductionOrderNumber,
   f.ID,
  faInFZ.ChangeDate,
  faInFZ.ConfirmedDeadline,
  faInFZ.PreviousDeadline,
  faInFZ.ProductionOrderStatus,
  faInFZ.ProductionOrderWarehouseId,
  faInFZ.ProductionOrderTime FROM {tableName} 
faInFZ
  inner join [Fertigung] f on faInFZ.ProductionOrderNumber = f.Fertigungsnummer
WHERE @ChangeDateFrom<=[ChangeDate] AND [ChangeDate] <=@ChangeDateTo AND [ProductionOrderWarehouseId]=@WarehouseId{(string.IsNullOrWhiteSpace(status) ? "" : $" and [ProductionOrderStatus]='{status}'")}";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand(query, sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("ChangeDateFrom", changeDateFrom);
				sqlCommand.Parameters.AddWithValue("ChangeDateTo", changeDateTo);
				sqlCommand.Parameters.AddWithValue("WarehouseId", warehouseId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ProductionOrderChangeHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<ProductionOrderChangeHistoryEntity>();
			}
		}
		public static List<ProductionOrderChangeHistoryWarehouseEntity> GetProductionOrderHistoryPerWarehouse(DateTime changeDateFrom, DateTime changeDateTo, int? type, string status = "")
		{
			var dataTable = new DataTable();
			var tableName = "dbo.[uvw_ProductionOrderChangeHistory]";
			if(type.HasValue)
			{
				if(type == 0)
				{
					tableName = "dbo.[uvw_ProductionOrderChangeHistory_BroughtInFZ]";
				}
				else
				{
					tableName = "dbo.[uvw_ProductionOrderChangeHistory_SentOutFZ]";
				}
			}
			var query = $@"SELECT COUNT(DISTINCT ProductionOrderNumber) ProductionOrderCount, ProductionOrderWarehouseId, ProductionOrderStatus, SUM(ProductionOrderTime) AS ProductionOrderTime FROM {tableName} 
										WHERE @ChangeDateFrom<=[ChangeDate] AND [ChangeDate] <=@ChangeDateTo{(string.IsNullOrWhiteSpace(status) ? "" : $" and [ProductionOrderStatus]='{status}'")}
										GROUP BY ProductionOrderWarehouseId, ProductionOrderStatus";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand(query, sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("ChangeDateFrom", changeDateFrom);
				sqlCommand.Parameters.AddWithValue("ChangeDateTo", changeDateTo);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ProductionOrderChangeHistoryWarehouseEntity(x)).ToList();
			}
			else
			{
				return new List<ProductionOrderChangeHistoryWarehouseEntity>();
			}
		}
		public static List<ProductionOrderChangeHistoryWarehouseYearWeekEntity> GetProductionOrderHistoryPerWarehouseYearWeek(DateTime changeDateFrom, DateTime changeDateTo, int warehouseId, int? type, string status = "")
		{
			var dataTable = new DataTable();
			var tableName = "dbo.[uvw_ProductionOrderChangeHistory]";
			if(type.HasValue)
			{
				if(type == 0)
				{
					tableName = "dbo.[uvw_ProductionOrderChangeHistory_BroughtInFZ]";
				}
				else
				{
					tableName = "dbo.[uvw_ProductionOrderChangeHistory_SentOutFZ]";
				}
			}
			var query = $@"SELECT COUNT(DISTINCT ProductionOrderNumber) ProductionOrderCount, 
									ProductionOrderWarehouseId, ProductionOrderChangeId,
									SUM(ProductionOrderTime) AS ProductionOrderTime, 
									YEAR(ChangeDate) ChangeDateYear, 
									DATEPART(ISO_WEEK, ChangeDate) ChangeDateWeek FROM {tableName} 
										WHERE @ChangeDateFrom<=[ChangeDate] AND [ChangeDate] <=@ChangeDateTo AND [ProductionOrderWarehouseId]=@WarehouseId{(string.IsNullOrWhiteSpace(status) ? "" : $" and [ProductionOrderStatus]='{status}'")}
										GROUP BY ProductionOrderWarehouseId, YEAR(ChangeDate), DATEPART(ISO_WEEK, ChangeDate)
										ORDER BY ProductionOrderWarehouseId, YEAR(ChangeDate), DATEPART(ISO_WEEK, ChangeDate)";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand(query, sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("ChangeDateFrom", changeDateFrom);
				sqlCommand.Parameters.AddWithValue("ChangeDateTo", changeDateTo);
				sqlCommand.Parameters.AddWithValue("WarehouseId", warehouseId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ProductionOrderChangeHistoryWarehouseYearWeekEntity(x)).ToList();
			}
			else
			{
				return new List<ProductionOrderChangeHistoryWarehouseYearWeekEntity>();
			}
		}
		public static List<FaChangesWeekYearHoursLeftEntity> GetProductionOrderHistoryPerWarehouseYearWeekFull(DateTime from , DateTime to,int warehouseId, int horizon, bool inFrozenZone, bool outOfFrozenZone, string status = "")
		{


			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"DECLARE @H1LengthInDays INT = {horizon};
					SELECT 
					  DATEPART(ISO_WEEK, fa.Termin_Bestätigt1) AS AffectedWeek, 
					  DATEPART(YEAR, fa.Termin_Bestätigt1) AS AffectedYear, 
					  SUM (
						(fa.FA_Menge * f.Zeit) / 60
					  ) as [HoursLeft] ,
					fa.Lagerort_id as  [Lager]
					FROM 
					  [PSZ_Fertigungsauftrag Änderungshistorie] as fa 
					  inner join [Fertigung] f on fa.Fertigungsnummer = f.Fertigungsnummer 
					  inner join [Lagerorte] l on fa.Lagerort_id = l.Lagerort_id ";

				var clauses = new List<string>();

				clauses.Add(" year(fa.Termin_Bestätigt1) = year(getDate())");

				if(from != null && to != null )
				{					
					clauses.Add($"fa.Termin_Bestätigt1 BETWEEN CONVERT(date, '{from:yyyy-MM-dd}') AND CONVERT(date, '{to:yyyy-MM-dd}')");
				}

				if(warehouseId != 0)
				{
					clauses.Add($"fa.Lagerort_id = {warehouseId}");
				}

				if(inFrozenZone)
				{
					clauses.Add(" fa.Termin_Bestätigt1 <= DATEADD(DD, @H1LengthInDays, fa.Änderungsdatum )");
					clauses.Add(" fa.Termin_voränderung > dateadd(day, @H1LengthInDays, fa.Änderungsdatum  ) ");

				}
				if(outOfFrozenZone)
				{
					clauses.Add($"fa.Termin_Bestätigt1 > DATEADD(DD, @H1LengthInDays, fa.Änderungsdatum) ");
					clauses.Add($" fa.Termin_voränderung <= dateadd(day, @H1LengthInDays, fa.Änderungsdatum )");

				}

				if(clauses.Count > 0)
				{
					query += " WHERE " + string.Join(" AND ", clauses);
				}


				query += "group by  DATEPART(ISO_WEEK, fa.Termin_Bestätigt1),  DATEPART(YEAR, fa.Termin_Bestätigt1),  fa.Lagerort_id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandText = query;
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandTimeout = 240; //in seconds
				DbExecution.Fill(sqlCommand, dataTable);

			}


			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new FaChangesWeekYearHoursLeftEntity(x)).ToList();
			}
			else
			{
				return new List<FaChangesWeekYearHoursLeftEntity>();
			}
		}
		#endregion Beadrf Analyse


		#region Days off
		public static List<DayOff> GetDaysOff(string stateCode)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT *,DATEPART(ISO_WEEK, [Date]) as KW  FROM [stats].[DaysOff]";
				if(!string.IsNullOrEmpty(stateCode))
				{
					query += " WHERE [" + stateCode.ToUpper() + "] = 1";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new DayOff(x)).ToList();
			}
			else
			{
				return new List<DayOff>();
			}
		}

		public static int SyncDaysOff(List<DayOff> items)
		{
			if(items != null && items.Count > 0)
			{
				initDaysOffTable();
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 41;
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = syncDaysOff(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += syncDaysOff(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += syncDaysOff(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}

			return -1;
		}

		private static int syncDaysOff(List<DayOff> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = string.Empty;
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [stats].[DaysOff] ([Date],[Name],[AllStates],[BW],[BY],[BE],[BB],[HB],[HH],[HE]" +
							",[MV],[NI],[NW],[RP],[SL],[SN],[ST],[SH],[TH]) VALUES ( "

							+ "@Date" + i + ","
							+ "@Name" + i + ","
							+ "@AllStates" + i + ","
							+ "@BW" + i + ","
							+ "@BY" + i + ","
							+ "@BE" + i + ","
							+ "@BB" + i + ","
							+ "@HB" + i + ","
							+ "@HH" + i + ","
							+ "@HE" + i + ","
							+ "@MV" + i + ","
							+ "@NI" + i + ","
							+ "@NW" + i + ","
							+ "@RP" + i + ","
							+ "@SL" + i + ","
							+ "@SN" + i + ","
							+ "@ST" + i + ","
							+ "@SH" + i + ","
							+ "@TH" + i + "); ";


						sqlCommand.Parameters.AddWithValue("Date" + i, item.Date == null ? (object)DBNull.Value : item.Date);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("AllStates" + i, item.AllStates == null ? (object)DBNull.Value : item.AllStates);
						sqlCommand.Parameters.AddWithValue("BW" + i, item.BW == null ? (object)DBNull.Value : item.BW);
						sqlCommand.Parameters.AddWithValue("BY" + i, item.BY == null ? (object)DBNull.Value : item.BY);
						sqlCommand.Parameters.AddWithValue("BE" + i, item.BE == null ? (object)DBNull.Value : item.BE);
						sqlCommand.Parameters.AddWithValue("BB" + i, item.BB == null ? (object)DBNull.Value : item.BB);
						sqlCommand.Parameters.AddWithValue("HB" + i, item.HB == null ? (object)DBNull.Value : item.HB);
						sqlCommand.Parameters.AddWithValue("HH" + i, item.HH == null ? (object)DBNull.Value : item.HH);
						sqlCommand.Parameters.AddWithValue("HE" + i, item.HE == null ? (object)DBNull.Value : item.HE);
						sqlCommand.Parameters.AddWithValue("MV" + i, item.MV == null ? (object)DBNull.Value : item.MV);
						sqlCommand.Parameters.AddWithValue("NI" + i, item.NI == null ? (object)DBNull.Value : item.NI);
						sqlCommand.Parameters.AddWithValue("NW" + i, item.NW == null ? (object)DBNull.Value : item.NW);
						sqlCommand.Parameters.AddWithValue("RP" + i, item.RP == null ? (object)DBNull.Value : item.RP);
						sqlCommand.Parameters.AddWithValue("SL" + i, item.SL == null ? (object)DBNull.Value : item.SL);
						sqlCommand.Parameters.AddWithValue("SN" + i, item.SN == null ? (object)DBNull.Value : item.SN);
						sqlCommand.Parameters.AddWithValue("ST" + i, item.ST == null ? (object)DBNull.Value : item.ST);
						sqlCommand.Parameters.AddWithValue("SH" + i, item.SH == null ? (object)DBNull.Value : item.SH);
						sqlCommand.Parameters.AddWithValue("TH" + i, item.TH == null ? (object)DBNull.Value : item.TH);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		private static int initDaysOffTable()
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var query = @$"
						IF  NOT EXISTS (SELECT * FROM sys.objects  WHERE object_id = OBJECT_ID(N'[stats].[DaysOff]') AND type in (N'U'))
						BEGIN
							CREATE TABLE [stats].[DaysOff](
								[Id] [int] IDENTITY(1,1) NOT NULL,
								[Date] [date] NULL,
								[Name] [nvarchar](50) NULL,
								[AllStates] [bit] NULL,
								[BW] [bit] NULL,
								[BY] [bit] NULL,
								[BE] [bit] NULL,
								[BB] [bit] NULL,
								[HB] [bit] NULL,
								[HH] [bit] NULL,
								[HE] [bit] NULL,
								[MV] [bit] NULL,
								[NI] [bit] NULL,
								[NW] [bit] NULL,
								[RP] [bit] NULL,
								[SL] [bit] NULL,
								[SN] [bit] NULL,
								[ST] [bit] NULL,
								[SH] [bit] NULL,
								[TH] [bit] NULL,
							 CONSTRAINT [PK_stats.DaysOff] PRIMARY KEY CLUSTERED 
							(
								[id] ASC
							)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
							) ON [PRIMARY]
						END
						ELSE
						BEGIN 
							DELETE FROM [stats].[DaysOff]
						END;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandText = query;
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return results;
		}
		#endregion
	}
}
