
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
										   --AND (
										   --Artikel.Artikelnummer Like '825-035-01AL'
										   --)
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
												--DATET is null or 
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

										  -- SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) Bestand FROM Lager where [Artikel-Nr] Like '985-003-01' group by [Artikel-Nr],Bestand



