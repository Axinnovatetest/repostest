using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class NotNeededOrdersAccess
	{
		public static List<Entities.Joins.MTM.Order.NotNeededOrdersEntity> GetNotNeededOrders(List<int> fertigungLager, int hauftLager, string artikelnummer, bool? projectOrders, bool? OnlyUnconfirmed, DateTime? DateConfirmationBefore, int RequestedPage, int PageSize)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
				@$"	
				select count(*) over () as TotlaCount,
					[Artikel-Nr] ArtikelNr,
					Artikelnummer,ProjectPurchase
				FROM (
					SELECT	
							Bestellungen.[Vorname/NameFirma] AS Lieferant,
							Bestellungen.[Bestellung-Nr], 
							[bestellte Artikel].Anzahl, 
							Artikel.Artikelnummer, 
							Artikel.[Artikel-Nr], 
							[bestellte Artikel].Liefertermin AS Wünschtermin,
							[bestellte Artikel].Lagerort_id,
							[bestellte Artikel].Bestätigter_Termin,
							[bestellte Artikel].Nr,
                            Bestellungen.ProjectPurchase
							FROM [bestellte Artikel]
								INNER JOIN Bestellungen ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
								INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
							INNER JOIN Lieferanten ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer
							WHERE 
								[bestellte Artikel].Lagerort_id ={hauftLager}   
								And Bestellungen.Typ = 'Bestellung' 
								And Bestellungen.erledigt = 0 
								And [bestellte Artikel].erledigt_pos = 0 
								And Bestellungen.Rahmenbestellung = 0
								And Left([Artikelnummer], 3) <> '227' 
								And Left([Artikelnummer], 3) <> '226'
								And Bestellungen.gebucht = 1
                                {(artikelnummer is not null ? $"AND Artikel.Artikelnummer LIKE '{artikelnummer}%'" : "")}
                                {(projectOrders.HasValue && projectOrders.Value ? "" : "AND (Bestellungen.ProjectPurchase is null or Bestellungen.ProjectPurchase=0)")}
                                {(OnlyUnconfirmed.HasValue && OnlyUnconfirmed.Value ? "AND [bestellte Artikel].Bestätigter_Termin='31-12-2999'" : "")}
                                {(DateConfirmationBefore.HasValue ? $"AND [bestellte Artikel].Bestätigter_Termin<='{DateConfirmationBefore}'" : "")}
						) AS T
				GROUP BY 
					T.Artikelnummer,
					T.[Artikel-Nr],ProjectPurchase
				HAVING 
				T.Artikelnummer Not In 
				(
					select distinct A.Artikelnummer from Fertigung_Positionen FP inner join Artikel A
					on A.[Artikel-Nr]=FP.Artikel_Nr
					inner join Fertigung F on FP.ID_Fertigung=F.ID
					WHERE F.Lagerort_id in ({String.Join(", ", fertigungLager)}) AND F.Kennzeichen=N'offen' 
				)
				Order BY Artikelnummer

				OFFSET {RequestedPage * PageSize} ROWS FETCH NEXT {PageSize}  ROWS ONLY";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.NotNeededOrdersEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.NotNeededOrdersEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.NotNeededOrdersArticleEntity> GetNotNeededOrdersArtikel(List<int> fertigungLager, int hauftLager, int ArtikelNr)
		{


			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
				@$"	
				select [Week],SUM(Anzahl) Anzahl,ArtikelNr,Artikelnummer
				FROM (
					SELECT	
							Bestellungen.[Vorname/NameFirma] AS Lieferant,
							Bestellungen.[Bestellung-Nr], 
							[bestellte Artikel].Anzahl, 
							Artikel.Artikelnummer, 
							Artikel.[Artikel-Nr] ArtikelNr, 
							[bestellte Artikel].Liefertermin AS Wünschtermin,
							[bestellte Artikel].Lagerort_id,
							[bestellte Artikel].Bestätigter_Termin,
							[bestellte Artikel].Nr,
							case WHEN DATEPART(MONTH, [bestellte Artikel].Bestätigter_Termin) = 1 AND DATEPART(DAY, [bestellte Artikel].Bestätigter_Termin) < 8 AND DATEPART(iso_week , [bestellte Artikel].Bestätigter_Termin) <> 1
							THEN CONCAT(DATEPART(iso_week, [bestellte Artikel].Bestätigter_Termin) , '/' , DATEPART(YEAR , [bestellte Artikel].Bestätigter_Termin) - 1) 
							WHEN  (DATEPART(MONTH,[bestellte Artikel].Bestätigter_Termin) = 12 AND DATEPART(DAY, [bestellte Artikel].Bestätigter_Termin) > 24 AND DATEPART(iso_week ,[bestellte Artikel].Bestätigter_Termin) = 1)
							THEN CONCAT(DATEPART(iso_week,[bestellte Artikel].Bestätigter_Termin) , '/' , DATEPART(YEAR , [bestellte Artikel].Bestätigter_Termin) + 1) 
							ELSE CONCAT(DATEPART(iso_week, [bestellte Artikel].Bestätigter_Termin) , '/' , DATEPART(YEAR , [bestellte Artikel].Bestätigter_Termin)) 
							END AS [Week]
							FROM [bestellte Artikel]
								INNER JOIN Bestellungen ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
								INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
								INNER JOIN Lieferanten ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer
							WHERE 
								[bestellte Artikel].Lagerort_id ={hauftLager}
								And Bestellungen.Typ = 'Bestellung' 
								And Bestellungen.erledigt = 0 
								And [bestellte Artikel].erledigt_pos = 0 
								And Bestellungen.Rahmenbestellung = 0
								And Left([Artikelnummer], 3) <> '227' 
								And Left([Artikelnummer], 3) <> '226'
								And Bestellungen.gebucht = 1
								AND [bestellte Artikel].[Artikel-Nr]={ArtikelNr}
						) AS T
					GROUP BY 
						T.Artikelnummer,
						T.ArtikelNr,
						T.[Week]
					HAVING 
					T.Artikelnummer Not In 
					(
						select distinct A.Artikelnummer from Fertigung_Positionen FP inner join Artikel A
						on A.[Artikel-Nr]=FP.Artikel_Nr
						inner join Fertigung F on FP.ID_Fertigung=F.ID
						WHERE F.Lagerort_id in ({String.Join(',', fertigungLager)}) AND F.Kennzeichen=N'offen' 
					)
					Order BY Artikelnummer
		";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.NotNeededOrdersArticleEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.NotNeededOrdersArticleEntity>();
			}
		}

		public static List<Entities.Joins.MTM.Order.NotNeededOrdersAllEntity> GetNotNeededOrdersAll(List<int> fertigungLager, int hauftLager)
		{


			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
				@$"	
				Select 
				[Week],Sum(Total) Total,Sum(anzahl) anzahl
				FROM (
				SELECT  
					   [Bestellung-Nr],
					   Lieferant,
					   [Week],
					  Bestätigter_Termin,
					    SUM(Total) Total,
						SUM(Anzahl) Anzahl
					   FROM (
					SELECT 
						Bestellungen.[Vorname/NameFirma] AS Lieferant,
						Bestellungen.[Bestellung-Nr], 
						[bestellte Artikel].Anzahl, 
						[bestellte Artikel].[Artikel-Nr], 
						Artikel.Artikelnummer, 
						[bestellte Artikel].Liefertermin AS Wünschtermin, 
						[bestellte Artikel].Bestätigter_Termin,
						 ([bestellte Artikel].Einzelpreis * Anzahl) Total,
						
						CONCAT(DATEPART(iso_week, [bestellte Artikel].Bestätigter_Termin),'/' ,YEAR(DATEADD(day, 26 - DATEPART(isoww, [bestellte Artikel].Bestätigter_Termin), [bestellte Artikel].Bestätigter_Termin))) as [Week]

						FROM 
							Bestellungen
							INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
							INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
		
						WHERE 
							[bestellte Artikel].Lagerort_id = {hauftLager}
							And Bestellungen.Typ = 'Bestellung' 
							And Bestellungen.erledigt = 0 
							And [bestellte Artikel].erledigt_pos = 0 
							And Bestellungen.Rahmenbestellung = 0 
							And Left(Artikel.[Artikelnummer], 3) <> '227' 
							And Left(Artikel.[Artikelnummer], 3) <> '226'
							And Bestellungen.gebucht = 1
							AND Artikelnummer Not In (
									select distinct A.Artikelnummer from Fertigung_Positionen FP inner join Artikel A
									on A.[Artikel-Nr]=FP.Artikel_Nr
									inner join Fertigung F on FP.ID_Fertigung=F.ID
									WHERE F.Lagerort_id in ({String.Join(',', fertigungLager)}) AND F.Kennzeichen=N'offen' 
								)
						) as O
					GROUP BY 
						[Bestellung-Nr],
					   Lieferant,
					   [Week]
					   ,Bestätigter_Termin
					
					) s
					Group by [week]
					ORDER BY 
					CAST(SUBSTRING([week],CHARINDEX('/',[week])+1,LEN([week])) AS int),
					CAST(SUBSTRING([week],0,CHARINDEX('/',[week])) AS int)
					";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.NotNeededOrdersAllEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.NotNeededOrdersAllEntity>();
			}
		}

		public static List<Entities.Joins.MTM.Order.NotNeededOrderArticleDetailsEntity> GetNotNeededOrdersArticleDetails(List<int> fertigungLager, int hauftLager, int artikelNr, DateTime startDate, DateTime endDate, int PageSize, int RequestedPage)
		{


			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
				@$"
						SELECT	
				count(*) over () as TotlaCount,
                        Bestellungen.Nr,
						Bestellungen.[Vorname/NameFirma] AS Lieferant,
						[bestellte Artikel].Anzahl, 
						[bestellte Artikel].Anzahl * [bestellte Artikel].Einzelpreis Total,
						[bestellte Artikel].[Artikel-Nr] ArtikelNr, 
						Artikel.Artikelnummer,
						Bestellungen.[Bestellung-Nr] BestellungNr,
						[bestellte Artikel].Bestätigter_Termin,
						[bestellte Artikel].Liefertermin
				FROM [bestellte Artikel]
					INNER JOIN Bestellungen ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
					INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
				INNER JOIN Lieferanten ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer
				WHERE 
					[bestellte Artikel].Lagerort_id ={hauftLager}  
					And Bestellungen.Typ = 'Bestellung' 
					And Bestellungen.erledigt = 0 
					And [bestellte Artikel].erledigt_pos = 0 
					And Bestellungen.Rahmenbestellung = 0
					And Left([Artikelnummer], 3) <> '227' 
					And Left([Artikelnummer], 3) <> '226'
					And Bestellungen.gebucht = 1
					and [bestellte Artikel].Bestätigter_Termin between '{startDate}' AND '{endDate}'
					AND [bestellte Artikel].[Artikel-Nr]={artikelNr}
				Order BY Artikelnummer

				OFFSET {RequestedPage * PageSize} ROWS FETCH NEXT {PageSize}  ROWS ONLY
					";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.NotNeededOrderArticleDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.NotNeededOrderArticleDetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.NotNeededOrderDetailsAllEntity> GetNotNeededOrdersDetailsAll(List<int> fertigungLager, int hauftLager, DateTime startDate, DateTime endDate, int PageSize, int RequestedPage)
		{


			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
				@$"	
				select count(*) over () as TotlaCount,
					   [Bestellung-Nr] BestellungNr,
					   Lieferant,
					  Wünschtermin,
					  Bestätigter_Termin,
					    SUM(Total) Total,
						SUM(Anzahl) Anzahl
				FROM (
					SELECT	
							Bestellungen.[Vorname/NameFirma] AS Lieferant,
									Bestellungen.[Bestellung-Nr], 
									[bestellte Artikel].Anzahl, 
									[bestellte Artikel].[Artikel-Nr], 
									Artikel.Artikelnummer, 
									[bestellte Artikel].Liefertermin AS Wünschtermin, 
									[bestellte Artikel].Bestätigter_Termin,
									([bestellte Artikel].Einzelpreis * Anzahl) Total
							FROM [bestellte Artikel]
								INNER JOIN Bestellungen ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
								INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
							WHERE 
								[bestellte Artikel].Lagerort_id ={hauftLager}
								And Bestellungen.Typ = 'Bestellung' 
								And Bestellungen.erledigt = 0 
								And [bestellte Artikel].erledigt_pos = 0 
								And Bestellungen.Rahmenbestellung = 0
								And Left([Artikelnummer], 3) <> '227' 
								And Left([Artikelnummer], 3) <> '226'
								And Bestellungen.gebucht = 1
								AND Bestätigter_Termin BETWEEN '{startDate}' AND '{endDate}'
						) AS T
				GROUP BY 
					[Bestellung-Nr],
					Lieferant, Wünschtermin,
					Bestätigter_Termin,
					Artikelnummer
		
				HAVING 
				T.Artikelnummer Not In 
				(
					select distinct A.Artikelnummer from Fertigung_Positionen FP inner join Artikel A
					on A.[Artikel-Nr]=FP.Artikel_Nr
					inner join Fertigung F on FP.ID_Fertigung=F.ID
					WHERE F.Lagerort_id in ({String.Join(',', fertigungLager)}) AND F.Kennzeichen=N'offen' 
				)
				Order BY Bestätigter_Termin
				OFFSET {RequestedPage * PageSize} ROWS FETCH NEXT {PageSize}  ROWS ONLY
					";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.NotNeededOrderDetailsAllEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.NotNeededOrderDetailsAllEntity>();
			}
		}

	}
}
