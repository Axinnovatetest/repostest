using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class ArticlesInOpenFaAccess
	{

		public static List<Entities.Joins.MTM.Order.ArticlesInFaFiltered> getArticlesFiltered(List<int?> LagersList, int magasinLager, int mainLager, Settings.PaginModel paging, int months, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string artikelNrfilter = artikelNr != 0 ? $"AND FP.[Artikel_Nr]={artikelNr}" : "";
				sqlConnection.Open();
				string query =
					$@"
						DROP TABLE IF EXISTS #fullData;
						CREATE TABLE #fullData(
							Id int PRIMARY KEY IDENTITY(1,1),
							Termin_Bestätigt1 datetime,
							NeedQuantity float,
							Artikelnummer varchar(50),
							Artikel_Nr int,
							totalInitStock float,
							[Artikel-Nr] int,
							oldQuantity float,
							artikelOldFa int,
							Bestätigter_Termin datetime,
							artikelNr int,
							Anzahl float,
							[Bestellung-Nr] int,
							ProcessedFA int,
							ProcessedOr int
						);

						DROP TABLE IF EXISTS #subsetData;
						CREATE TABLE #subsetData(
							Id int PRIMARY KEY IDENTITY(1,1),
							Termin_Bestätigt1 datetime,
							NeedQuantity float,
							Artikelnummer varchar(50),
							Artikel_Nr int,
							totalInitStock float,
							[Artikel-Nr] int,
							oldQuantity float,
							artikelOldFa int,
							Bestätigter_Termin datetime,
							artikelNr int,
							Anzahl float,
							[Bestellung-Nr] int,
							ProcessedFA int,
							ProcessedOr int
						);
						DROP TABLE IF EXISTS #result3;

					

						DROP TABLE IF EXISTS #result;
						CREATE TABLE #result(
							ArtikelNr int,
							ArtikelNummer varchar(50),
							CumulativeStock float,
							DateIssue datetime
						);

						WITH ArticlesFA AS (
							SELECT
								Termin_Bestätigt1,
								SUM(FPAnzahl / Originalanzahl * FAnzahl) NeedQuantity,
								Artikelnummer,
								[Artikel_Nr]
							FROM (
									SELECT
										Termin_Bestätigt1,
										Artikelnummer,
										FP.[Artikel_Nr],
										FP.Anzahl FPAnzahl,
										F.Originalanzahl,
										F.Anzahl FAnzahl
									FROM
										Fertigung F
										INNER JOIN Fertigung_Positionen FP ON F.ID = FP.ID_Fertigung
										AND F.Termin_Bestätigt1 Between GETDATE()
										AND DATEADD(MONTH, {months}, GETDATE())
										AND F.Kennzeichen = 'offen'
										AND IsNULL(F.FA_Gestartet, 0) = 0
										AND F.Originalanzahl <> 0
										AND F.Originalanzahl IS NOT NULL
										AND F.Anzahl IS NOT NULL
										AND FP.Lagerort_ID IN({string.Join(',', LagersList)})
										inner JOIN Artikel A ON FP.[Artikel_Nr] = A.[Artikel-Nr]
										{artikelNrfilter}
								) s
							Group by
								[Artikel_Nr],
								Artikelnummer,
								Termin_Bestätigt1
    
						),
						OldFA AS (
							select
								SUM(FP.Anzahl / Originalanzahl * f.Anzahl) oldQuantity,
								FP.Artikel_Nr artikelOldFA
							from
								Fertigung F
								join Fertigung_Positionen FP ON FP.ID_Fertigung = f.ID
								AND F.Termin_Bestätigt1 <GETDATE()
								AND F.Kennzeichen = 'offen'
								AND IsNULL(F.FA_Gestartet, 0) = 0
								AND F.Originalanzahl <> 0
								AND F.Originalanzahl IS NOT NULL
								AND F.Anzahl IS NOT NULL
								AND FP.Lagerort_ID IN({string.Join(',', LagersList)})
								--AND FP.Artikel_Nr in (select Artikel_Nr from ArticlesFA)
							Group by
								FP.Artikel_Nr
						),
						ORDERArticles AS (SELECT
										Bestätigter_Termin,
										[Artikel-Nr] artikelNr,
										SUM(Anzahl) Anzahl,
										[Bestellung-Nr]
									FROM
										(
											select
												Bestätigter_Termin,
												[Artikel-Nr],
												Anzahl,
												ba.[Bestellung-Nr]
											from
												  [bestellte Artikel] ba
												INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
												AND b.Typ = 'Bestellung'
											WHERE
												Anzahl > 0
												AND Bestätigter_Termin BETWEEN GETDATE()
												AND DATEADD(MONTH, {months}, GETDATE())
												AND [Position erledigt] <> 1
												AND Anzahl > 0
												AND Lagerort_id IN ({string.Join(',', LagersList)})
												--AND [Artikel-Nr] in (select Artikel_Nr from ArticlesFA)
										) s2
									Group by
										Bestätigter_Termin,
										[Artikel-Nr],
										[Bestellung-Nr]
			
									--HAVING [Artikel-Nr] in (select Artikel_Nr from ArticlesFA)
									)
						Insert into
							#fullData
						Select
  
							*,
							0,
							0
						from
							ArticlesFA afa
							left JOIN (
								select
									ISNULL(res1.InitStock, 0) - ISNULL(res2.reserved_quantity, 0) totalInitStock,
									[Artikel-Nr]
								from
									(
										select
											a.[Artikel-Nr],
											SUM(l.Bestand) InitStock,
											a.Warentyp
										from
											Lager l
											JOIN Artikel A On A.[Artikel-Nr] = l.[Artikel-Nr]
											AND ISNULL(a.aktiv, 0) = 1
											AND l.Lagerort_id IN (
											   {string.Join(',', LagersList)},
												(
													CASE
														WHEN a.Warentyp = 2 THEN {magasinLager}
														ELSE -1000
													END
												)
											)

										group by
											a.[Artikel-Nr],
											a.Warentyp
									) res1
									left join (
										select
											h.Artikel_Nr,
											h.Lagerort_ID,
											SUM(h.Menge_reserviert) reserved_quantity
										from
											tbl_Planung_gestartet h
										WHERE
											Lagerort_ID = {mainLager}
										group by
											h.Artikel_Nr,
											h.Lagerort_ID
									) res2 on res1.[Artikel-Nr] = res2.Artikel_Nr
							) d ON d.[Artikel-Nr] = afa.Artikel_Nr
							left join OldFA ofa ON ofa.artikelOldFA = afa.Artikel_Nr
							left join ORDERArticles on afa.Artikel_Nr = ORDERArticles.artikelNr
	

						

			DROP TABLE IF EXISTS #articleList;

				CREATE TABLE #articleList(
										Artikel_Nr int,
										calculated int,
									); 

			insert into #articleList
			select distinct [Artikel_Nr],0 from #fullData


			Declare @Id int;
						DECLARE @NeedQuantity float;
						Declare @Artikelnummer varchar(50);
						Declare @Artikel_Nr int;
						Declare @Anzahl float;
						Declare @totalInitStock float;
						Declare @oldQuantity float;
						Declare @artikelOldFa int;
						DECLARE @FaDate datetime;
						DECLARE @OrderDate datetime;

						DECLARE @artikelNr int;
						DECLARE @RES3Id int;
						DECLARE @RES3Anzahl float;

						--DECLARE @result3 TABLE (
						--		Id int,
						--	Termin_Bestätigt1 datetime,
						--	NeedQuantity float,
						--	Artikelnummer varchar(50),
						--	Artikel_Nr int,
						--	totalInitStock float,
						--	[Artikel-Nr] int,
						--	oldQuantity float,
						--	artikelOldFa int,
						--	Bestätigter_Termin datetime,
						--	artikelNr int,
						--	Anzahl float,
						--	[Bestellung-Nr] int,
						--	ProcessedFA int,
						--	ProcessedOr int
						--);
							CREATE TABLE #result3(
							Id int PRIMARY KEY IDENTITY(1,1),
							Bestätigter_Termin datetime,
							artikelNr int,
							Anzahl float,
							[Bestellung-Nr] int,
							ProcessedOr int
						);

					
						
						DECLARE @BestellungNr int;

					while((Select Count(*) From #articleList Where calculated = 0)>0)
					begin
						SET IDENTITY_INSERT #subsetData ON;
						Select top 1 @Artikel_Nr = [Artikel_Nr] from #articleList where calculated = 0
						insert into #subsetData( Id,
							Termin_Bestätigt1 ,
							NeedQuantity ,
							Artikelnummer ,
							Artikel_Nr ,
							totalInitStock ,
							[Artikel-Nr] ,
							oldQuantity ,
							artikelOldFa ,
							Bestätigter_Termin ,
							artikelNr ,
							Anzahl ,
							[Bestellung-Nr] ,
							ProcessedFA ,
							ProcessedOr ) select Id,
							Termin_Bestätigt1 ,
							NeedQuantity ,
							Artikelnummer ,
							Artikel_Nr ,
							totalInitStock ,
							[Artikel-Nr] ,
							oldQuantity ,
							artikelOldFa ,
							Bestätigter_Termin ,
							artikelNr ,
							Anzahl ,
							[Bestellung-Nr] ,
							ProcessedFA ,
							ProcessedOr 
							from #fullData where Artikel_Nr = @Artikel_Nr OR artikelNr = @Artikel_Nr;
							SET IDENTITY_INSERT #subsetData OFF;

						While (
						 (Select Count(*) From #subsetData Where ProcessedFA = 0) > 0
						)
						Begin 
							
							Select Top 1 @Id = Id,@Artikel_Nr = [Artikel_Nr],@NeedQuantity = NeedQuantity,@Artikelnummer = Artikelnummer,
							@Anzahl = Anzahl,@totalInitStock = totalInitStock,@oldQuantity = oldQuantity,
							@artikelNr = artikelNr,@FaDate = Termin_Bestätigt1,@OrderDate = Bestätigter_Termin
							From #subsetData Where Id = (select Min(Id ) from #subsetData WHERE ProcessedFA = 0)
							if(NOT EXISTS(select * from #result WHERE ArtikelNr = @Artikel_Nr))
							BEGIN 
								set @totalInitStock = ISNULL(@totalInitStock, 0)  - ISNULL(@oldQuantity, 0);
								insert into #result values(@Artikel_Nr,@ArtikelNummer,@totalInitStock,@FaDate );
							END
							
							if(EXISTS(select CumulativeStock from #result WHERE ArtikelNr = @Artikel_Nr AND CumulativeStock < 0)) 
							BEGIN
								Update #subsetData Set ProcessedFA = 1,ProcessedOr = 1 Where [Artikel_Nr] = @Artikel_Nr
								Continue;
							END
							SET IDENTITY_INSERT #result3 ON
							INSERT into #result3  (Id,Bestätigter_Termin,artikelNr,Anzahl,[Bestellung-Nr],ProcessedOr)
							select Id,Bestätigter_Termin,artikelNr,Anzahl,[bestellung-Nr],ProcessedOr from #subsetData -- WHERE Artikel_Nr = @Artikel_Nr
							WHERE ((
							DATEPART(iso_week, @FaDate) >= DATEPART(iso_week, Bestätigter_Termin)
							AND   year(dateadd(wk, datediff(d, 0, @FaDate) / 7, 3)) = year(dateadd(wk, datediff(d, 0, Bestätigter_Termin) / 7, 3))
							)
							OR   year(dateadd(wk, datediff(d, 0, @FaDate) / 7, 3)) > year(dateadd(wk, datediff(d, 0, Bestätigter_Termin) / 7, 3)))
							ORDER BY Id;
							SET IDENTITY_INSERT #result3 OFF

							While ((Select Count(*) From #result3 Where ProcessedOr = 0 ) > 0)
							Begin
								Select Top 1 @RES3Id = Id,@BestellungNr=[bestellung-Nr], @OrderDate = Bestätigter_Termin,@RES3Anzahl = Anzahl From #result3 Where ProcessedOr = 0 
								Update #result SET CumulativeStock = CumulativeStock + ISNULL(@RES3Anzahl, 0)  WHERE ArtikelNr = @Artikel_Nr
								Update #subsetData Set ProcessedOr = 1 Where Bestätigter_Termin = @OrderDate AND [bestellung-Nr] = @BestellungNr AND artikelNr = @Artikel_Nr
								Update #result3 Set ProcessedOr = 1 Where   Bestätigter_Termin = @OrderDate AND [bestellung-Nr] = @BestellungNr AND artikelNr = @Artikel_Nr
							END
	
					
							Update #result SET CumulativeStock = CumulativeStock - ISNULL(@NeedQuantity, 0),dateissue=@FaDate  Where [ArtikelNr] = @Artikel_Nr
							Update #subsetData Set ProcessedFA = 1 Where  Termin_Bestätigt1 = @FaDate  AND Artikel_Nr = @Artikel_Nr
							TRUNCATE TABLE #result3
						End
						TRUNCATE TABLE #subsetData
						update #articleList set calculated = 1 where Artikel_Nr = @Artikel_Nr;

					END
						


						SELECT TotalCount = COUNT(*) OVER(),
							*
						from
							#result
						WHERE CumulativeStock < 0
						ORDER BY ArtikelNummer
						OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY 
				";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 120;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.ArticlesInFaFiltered(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.ArticlesInFaFiltered>();
			}
		}
		public static Entities.Joins.MTM.Order.FaultyFertigungCountEntity GetFaultyFertigungsCount(List<int?> LagersList, int ArtikelNr)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new Entities.Joins.MTM.Order.FaultyFertigungCountEntity();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Lagerort_sub_filter = "";
				string Artikel_Sub_Filter = "";

				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND F.Lagerort_ID IN({pattern})";
				}
				if(ArtikelNr > 0)
				{
					Artikel_Sub_Filter = $"AND FP.Artikel_Nr = {ArtikelNr}";
				}
				sqlConnection.Open();
				string query =
					$@"SELECT COUNT(DISTINCT F.Fertigungsnummer) faulty_Fa FROM
					Fertigung F  INNER JOIN   Fertigung_Positionen FP ON
					F.ID = FP.ID_Fertigung
					AND F.Termin_Bestätigt1 <= GETDATE()
					AND F.Kennzeichen = 'offen'
					AND IsNULL(F.FA_Gestartet,0)= 0
					AND ISNULL(F.Originalanzahl,0 ) > 0
					AND ISNULL(F.Originalanzahl,0 ) > ISNULL(F.Anzahl_erledigt , 0)
					AND ISNULL(F.Anzahl,0) > 0
					{Lagerort_sub_filter} 
					{Artikel_Sub_Filter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.FaultyFertigungCountEntity(x)).FirstOrDefault();
			}
			else
			{
				return new Entities.Joins.MTM.Order.FaultyFertigungCountEntity();
			}
		}
		public static List<Entities.Joins.MTM.Order.GetFaultyFasEntity> GetFaultyFertigungs(List<int?> LagersList, int ArtikelNr, int RequestedPage, int PageSize)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new List<Entities.Joins.MTM.Order.GetFaultyFasEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Lagerort_sub_filter = "";
				string Artikel_Sub_Filter = "";

				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND F.Lagerort_ID IN({pattern})";
				}
				if(ArtikelNr > 0)
				{
					Artikel_Sub_Filter = $"AND FP.Artikel_Nr = {ArtikelNr}";
				}
				sqlConnection.Open();
				string query =
					$@"SELECT COUNT(*) OVER() TotalCount
					,F.Fertigungsnummer Fertigungsnummer,Termin_Bestätigt1,F.ID
					FROM Fertigung F 
					JOIN Fertigung_Positionen fp 
					ON F.ID = fp.ID_Fertigung 
					AND  F.Termin_Bestätigt1 <= GETDATE() 
					AND F.Kennzeichen = 'offen' 
					AND IsNULL(F.FA_Gestartet,0)= 0 
					AND ISNULL (F.Originalanzahl,0 ) > 0  
					AND ISNULL (F.Originalanzahl,0 ) > ISNULL (F.Anzahl_erledigt,0 )  
					AND ISNULL(F.Anzahl,0) > 0  
					{Lagerort_sub_filter}
					{Artikel_Sub_Filter}
					GROUP BY F.Fertigungsnummer ,Termin_Bestätigt1,F.ID
					ORDER BY Termin_Bestätigt1 ASC ";

				if(RequestedPage >= 0 && PageSize > 0)
				{
					query = query + $" OFFSET {RequestedPage * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.GetFaultyFasEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.GetFaultyFasEntity>();
			}
		}
		// table 
		public static async Task<List<Entities.Joins.MTM.Order.NeededQuantityByArticleAndWeekEntity>> GetListOfArticlesInOpenFasByWeekAsync(List<int?> LagersList, int? Klassifizierung = 0)
		{
			if(LagersList is null || LagersList.Count() <= 0)
			{
				return new List<Entities.Joins.MTM.Order.NeededQuantityByArticleAndWeekEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				string Klassifizierung_sub_filter = "";
				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND FP.Lagerort_ID IN({pattern})";

				}
				if(Klassifizierung is not null && Klassifizierung > 0)
				{
					Klassifizierung_sub_filter = $"AND A.ID_Klassifizierung={Klassifizierung}";
				}
				else if(Klassifizierung is not null && Klassifizierung == -1)
				{
					Klassifizierung_sub_filter = $"AND A.ID_Klassifizierung IS NULL";
				}

				await sqlConnection.OpenAsync().ConfigureAwait(false);

				string query =
						$@"SELECT 
							 [Week]
							, SUM(FPAnzahl / Originalanzahl * FAnzahl) NeedQuantity
							,Artikelnummer
							,[Artikel_Nr]
										FROM
									(SELECT
									 case WHEN DATEPART(MONTH , Termin_Bestätigt1) = 1 AND DATEPART(DAY , Termin_Bestätigt1) > 8 AND DATEPART(iso_week , Termin_Bestätigt1) <> 1
									 THEN  CONCAT(DATEPART(iso_week , Termin_Bestätigt1) , '/' , DATEPART(YEAR , Termin_Bestätigt1) - 1)
									 ELSE CONCAT(DATEPART(iso_week , Termin_Bestätigt1) , '/' , DATEPART(YEAR , Termin_Bestätigt1))
									 END AS [Week]
									, Artikelnummer
									, FP.[Artikel_Nr]
									, FP.Anzahl FPAnzahl
									, F.Originalanzahl
									, F.Anzahl FAnzahl
									, F.Termin_Bestätigt1
									FROM Fertigung F
									INNER JOIN Fertigung_Positionen FP  ON F.ID = FP.ID_Fertigung
									AND F.Termin_Bestätigt1 Between GETDATE() AND DATEADD(MONTH , 3 , GETDATE())
									AND F.Kennzeichen = 'offen'
									AND IsNULL(F.FA_Gestartet , 0) = 0
									AND F.Originalanzahl <> 0
									AND F.Originalanzahl IS NOT NULL
									AND F.Anzahl IS NOT NULL
									{Lagerort_sub_filter}
									INNER JOIN Artikel A ON FP.[Artikel_Nr] = A.[Artikel-Nr]
									{Klassifizierung_sub_filter}) s
							Group by
							[Artikel_Nr],Artikelnummer,[Week]
										ORDER BY
							Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				await Task.Run(() => new SqlDataAdapter(sqlCommand).Fill(dataTable)).ConfigureAwait(false);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.NeededQuantityByArticleAndWeekEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.NeededQuantityByArticleAndWeekEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.ArticlesInFaFiltered> getArticles(List<int?> LagersList, int? magasinLager, int? mainLager, string artikelNummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query =
					$@"
					
						DROP TABLE IF EXISTS #fullData;
						CREATE TABLE #fullData(
							Id int PRIMARY KEY IDENTITY(1,1),
							Termin_Bestätigt1 datetime,
							NeedQuantity float,
							Artikelnummer varchar(50),
							Artikel_Nr int,
							totalInitStock float,
							[Artikel-Nr] int,
							oldQuantity float,
							artikelOldFa int,
							Bestätigter_Termin datetime,
							artikelNr int,
							Anzahl float,
							[Bestellung-Nr] int,
							ProcessedFA int,
							ProcessedOr int
						);


						DROP TABLE IF EXISTS #result;
						CREATE TABLE #result(
							ArtikelNr int,
							ArtikelNummer varchar(50),
							CumulativeStock float,
							DateIssue datetime
						);

						WITH ArticlesFA AS (
							SELECT
								Termin_Bestätigt1,
								SUM(FPAnzahl / Originalanzahl * FAnzahl) NeedQuantity,
								Artikelnummer,
								[Artikel_Nr]
							FROM (
									SELECT
										Termin_Bestätigt1,
										Artikelnummer,
										FP.[Artikel_Nr],
										FP.Anzahl FPAnzahl,
										F.Originalanzahl,
										F.Anzahl FAnzahl
									FROM
										Fertigung F
										INNER JOIN Fertigung_Positionen FP ON F.ID = FP.ID_Fertigung
										AND F.Termin_Bestätigt1 Between GETDATE()
										AND DATEADD(MONTH, 3, GETDATE())
										AND F.Kennzeichen = 'offen'
										AND IsNULL(F.FA_Gestartet, 0) = 0
										AND F.Originalanzahl <> 0
										AND F.Originalanzahl IS NOT NULL
										AND F.Anzahl IS NOT NULL
										{(LagersList?.Count > 0 ? $"AND FP.Lagerort_ID IN ({string.Join(',', LagersList)})" : "")}
										inner JOIN Artikel A ON FP.[Artikel_Nr] = A.[Artikel-Nr] 
									WHERE A.ArtikelNummer like ('{artikelNummer.SqlEscape()}%')
								) s
							Group by
								[Artikel_Nr],
								Artikelnummer,
								Termin_Bestätigt1
    
						),
						OldFA AS (
							select
								SUM(FP.Anzahl) oldQuantity,
								FP.Artikel_Nr artikelOldFA
							from
								Fertigung F
								join Fertigung_Positionen FP ON FP.ID_Fertigung = f.ID
								AND F.Termin_Bestätigt1 <GETDATE()
								AND F.Kennzeichen = 'offen'
								AND IsNULL(F.FA_Gestartet, 0) = 0
								AND F.Originalanzahl <> 0
								AND F.Originalanzahl IS NOT NULL
								AND F.Anzahl IS NOT NULL
								{(LagersList?.Count > 0 ? $"AND FP.Lagerort_ID IN({string.Join(',', LagersList)})" : "")}
								AND FP.Artikel_Nr in (select Artikel_Nr from ArticlesFA)
							Group by
								FP.Artikel_Nr
						),
						ORDERArticles AS (SELECT
										Bestätigter_Termin,
										[Artikel-Nr] artikelNr,
										SUM(Anzahl) Anzahl,
										[Bestellung-Nr]
									FROM
										(
											select
												Bestätigter_Termin,
												[Artikel-Nr],
												Anzahl,
												ba.[Bestellung-Nr]
											from
												[bestellte Artikel] ba
												INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
												AND b.Typ = 'Bestellung'
											WHERE
												Anzahl > 0
												AND Bestätigter_Termin BETWEEN GETDATE()
												AND DATEADD(MONTH, 3, GETDATE())
												AND [Position erledigt] <> 1
												AND Anzahl > 0
												{(LagersList?.Count > 0 ? $"AND Lagerort_id IN ({string.Join(',', LagersList)})" : "")}
												AND [Artikel-Nr] in (select Artikel_Nr from ArticlesFA)
										) s2
									Group by
										Bestätigter_Termin,
										[Artikel-Nr],
										[Bestellung-Nr]
			
									HAVING [Artikel-Nr] in (select Artikel_Nr from ArticlesFA)
									)
						Insert into
							#fullData
						Select
  
							*,
							0,
							0
						from
							ArticlesFA afa
							left JOIN (
								select
									ISNULL(res1.InitStock, 0) - ISNULL(res2.reserved_quantity, 0) totalInitStock,
									[Artikel-Nr]
								from
									(
										select
											a.[Artikel-Nr],
											SUM(l.Bestand) InitStock,
											a.Warentyp
										from
											Lager l
											JOIN Artikel A On A.[Artikel-Nr] = l.[Artikel-Nr]
											AND ISNULL(a.aktiv, 0) = 1
											{(magasinLager.HasValue && LagersList?.Count > 0 ? $@" AND l.Lagerort_id IN (
											   {string.Join(',', LagersList)},
												(
													CASE
														WHEN a.Warentyp = 2 THEN {magasinLager.Value}
														ELSE -1000
													END
												)
											)" : "")}
										group by
											a.[Artikel-Nr],
											a.Warentyp
									) res1
									left join (
										select
											h.Artikel_Nr,
											h.Lagerort_ID,
											SUM(h.Menge_reserviert) reserved_quantity
										from
											tbl_Planung_gestartet h
										{(mainLager.HasValue ? $"WHERE Lagerort_ID = {mainLager.Value} " : "")}
										group by
											h.Artikel_Nr,
											h.Lagerort_ID
									) res2 on res1.[Artikel-Nr] = res2.Artikel_Nr
							) d ON d.[Artikel-Nr] = afa.Artikel_Nr
							left join OldFA ofa ON ofa.artikelOldFA = afa.Artikel_Nr
							left join ORDERArticles on afa.Artikel_Nr = ORDERArticles.artikelNr
	

						Declare @Id int;
						DECLARE @NeedQuantity float;
						Declare @Artikelnummer varchar(50);
						Declare @Artikel_Nr int;
						Declare @Anzahl float;
						Declare @totalInitStock float;
						Declare @oldQuantity float;
						Declare @artikelOldFa int;
						DECLARE @FaDate datetime;
						DECLARE @OrderDate datetime;

						DECLARE @artikelNr int;
						DECLARE @RES3Id int;
						DECLARE @RES3Anzahl float;

						DECLARE @result3 TABLE (
								Id int,
							Termin_Bestätigt1 datetime,
							NeedQuantity float,
							Artikelnummer varchar(50),
							Artikel_Nr int,
							totalInitStock float,
							[Artikel-Nr] int,
							oldQuantity float,
							artikelOldFa int,
							Bestätigter_Termin datetime,
							artikelNr int,
							Anzahl float,
							[Bestellung-Nr] int,
							ProcessedFA int,
							ProcessedOr int
						);

						DECLARE @BestellungNr int;
						While (
						 (Select Count(*) From #fullData Where ProcessedFA = 0) > 0
						)
						Begin 
							Select Top 1 @Id = Id,@Artikel_Nr = [Artikel_Nr],@NeedQuantity = NeedQuantity,@Artikelnummer = Artikelnummer,
							@Anzahl = Anzahl,@totalInitStock = totalInitStock,@oldQuantity = oldQuantity,
							@artikelNr = artikelNr,@FaDate = Termin_Bestätigt1,@OrderDate = Bestätigter_Termin
							From #fullData Where Id = (select Min(Id ) from #fullData WHERE ProcessedFA = 0)
							if(NOT EXISTS(select * from #result WHERE ArtikelNr = @Artikel_Nr))
							BEGIN 
								set @totalInitStock = ISNULL(@totalInitStock, 0)  - ISNULL(@oldQuantity, 0);
								insert into #result values(@Artikel_Nr,@ArtikelNummer,@totalInitStock,@FaDate );
							END
							if(EXISTS(select CumulativeStock from #result WHERE ArtikelNr = @Artikel_Nr AND CumulativeStock < 0)) 
							BEGIN
								Update #fullData Set ProcessedFA = 1,ProcessedOr = 1 Where [Artikel_Nr] = @Artikel_Nr
								Continue;
							END
							INSERT into @result3 
							select * from #fullData WHERE Artikel_Nr = @Artikel_Nr
							AND ((
							DATEPART(iso_week, @FaDate) >= DATEPART(iso_week, Bestätigter_Termin)
							AND   year(dateadd(wk, datediff(d, 0, @FaDate) / 7, 3)) = year(dateadd(wk, datediff(d, 0, Bestätigter_Termin) / 7, 3))
							)
							OR   year(dateadd(wk, datediff(d, 0, @FaDate) / 7, 3)) > year(dateadd(wk, datediff(d, 0, Bestätigter_Termin) / 7, 3)))
							ORDER BY Id;

							While ((Select Count(*) From @result3 Where ProcessedOr = 0 ) > 0)
							Begin
								Select Top 1 @RES3Id = Id,@BestellungNr=[bestellung-Nr], @OrderDate = Bestätigter_Termin,@RES3Anzahl = Anzahl From @result3 Where ProcessedOr = 0 
								Update #result SET CumulativeStock = CumulativeStock + ISNULL(@RES3Anzahl, 0)  WHERE ArtikelNr = @Artikel_Nr
								Update #fullData Set ProcessedOr = 1 Where Bestätigter_Termin = @OrderDate AND [bestellung-Nr] = @BestellungNr AND artikelNr = @Artikel_Nr
								Update @result3 Set ProcessedOr = 1 Where   Bestätigter_Termin = @OrderDate AND [bestellung-Nr] = @BestellungNr AND artikelNr = @Artikel_Nr
							END
	
	
							Update #result SET CumulativeStock = CumulativeStock - ISNULL(@NeedQuantity, 0),dateissue=@FaDate  Where [ArtikelNr] = @Artikel_Nr
							Update #fullData Set ProcessedFA = 1 Where Id = @Id
							DELETE FROM @result3
						End



						SELECT TotalCount = COUNT(*) OVER(),
							*
						from
							#result
						WHERE CumulativeStock < 0
						ORDER BY ArtikelNummer
				";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.ArticlesInFaFiltered(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.ArticlesInFaFiltered>();
			}
		}
	}
}
