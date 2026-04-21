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
									--Where a.Kundennummer is not null and n.stufe is not null and n.Stufe <> ''


;
--- Insert into dashboard table
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
				---HAVING ClientTable.Stufe='KKM' ---AND Sum(PP.Gesamtpreis)<>0
				)		
				

					-- insert into dashboard table
				insert into [stats].[Dashboard]([CustomerGroupClass]
				,[TotalAmount]
				,[ImmediatAmount]
				,[ProductionAmount]
				--,[CustomerGroupOrder]
				,[Results]
				
				,[SyncId])

				SELECT t.Stufe,
						SUM(t.sGesamt) TotalAmount,
						SUM(l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)))  AS ImmediatAmount,
						SUM(ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END))) AS ProductionAmount,
						--- the result will be the sum of ImmediatAmount and productionAmount
						SUM(l.Bestand * (t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END)))
						+
						SUM(ISNULL(F.Anzahl,0)*(t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END))) as Results,
						
						--0,
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
						
						



--- Insert into dashboard article table
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

									WHERE (((Angebote.Typ) Like 'Auftrags%') AND ((Angebote.erledigt)=0) AND (([angebotene Artikel].erledigt_pos)=0) 
										   --AND ((Angebote.[Kunden-Nr]) IN ({string.Join(",", customerNumbers)}))
										   )
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
								--	1
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
										   --'{dateTill.ToString("yyyMMdd")}' 
										   GROUP BY [Artikel-Nr],Kunden_Nr) as t
										   Inner Join Artikel a on a.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager GROUP BY [Artikel-Nr]) AS L ON L.[Artikel-Nr]=t.[Artikel-Nr]
										   Left Join (SELECT [Artikel_Nr], SUM(ISNULL(Anzahl,0)) Anzahl FROM Fertigung Where Termin_Bestätigt1 is null or Termin_Bestätigt1 <=
										   --'{dateTill.ToString("yyyMMdd")}' 
										   GETDATE()
										   GROUP BY [Artikel_Nr]) AS F ON F.[Artikel_Nr]=t.[Artikel-Nr]  WHERE t.sGesamt/(CASE WHEN t.sBedarf=0 THEN 1 ELSE t.sBedarf END) > 1000
										   
										   --- Insert into Dashboard customer table
insert into [stats].DashboardCustomer
([Name],DashboardId)
select [Name],(select Id from [stats].Dashboard where CustomerGroupClass=#ClientTable.Stufe and SyncId=(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')) DashboardIdSec

from #ClientTable

--- Delete extra lines 
delete from [stats].DashboardCustomer  where dashboardId is null
