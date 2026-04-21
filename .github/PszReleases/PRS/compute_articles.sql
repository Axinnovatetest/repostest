USE [ERP_WEB]
GO

/****** Object:  StoredProcedure [dbo].[usp_prs_compute_articles]    Script Date: 21.01.2025 10:52:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_prs_compute_articles]
@UserId INT
AS

--IF OBJECT_ID('dbo.__PRS_StockWarnings_Articles', 'U') IS NOT NULL DROP TABLE dbo.__PRS_StockWarnings_Articles;
--SELECT DISTINCT ArtikelNr,Prio
--INTO __PRS_StockWarnings_Articles
--from (
--SELECT DISTINCT ArtikelNr,3 Prio from __PRS_StockWarnings_FaStatus 
--UNION ALL 
--SELECT DISTINCT ArtikelNr,3 Prio from __PRS_StockWarnings_PoStatus
--) X


IF OBJECT_ID('tempdb..#Temp01') IS NOT NULL DROP TABLE #Temp01;
select ArtikelNr,[Year],[Week],SUM([Qty]) Qty,Unit into #Temp01 from (
select ArtikelNr ,[Year],[Week],-SUM([Qty]) Qty ,Unit  from __PRS_StockWarnings_FaStatus
GROUP BY ArtikelNr ,[Year],[Week],Unit
union all 
select ArtikelNr,[Year],[Week],SUM([Qty]) Qty,Unit  from __PRS_StockWarnings_PoStatus
GROUP BY ArtikelNr ,[Year],[Week],Unit
union all
SELECT [Artikel-Nr],[Year],[Week],SUM([Bestand]) [Bestand],Unit FROM (
select Artikel.[Artikel-Nr],YEAR(GETDATE()) [Year],DATEPART(ISO_WEEK,GETDATE()) [Week],[Bestand],
CASE
WHEN Lagerort_ID IN (42, 41, 40, 57, 49, 46, 47) THEN  'WS-TN'
WHEN Lagerort_ID IN (101, 102, 109, 105, 104, 107, 108) THEN  'GZ'
WHEN Lagerort_ID IN (6, 3, 52, 9, 53, 17) THEN  'CZ'
WHEN Lagerort_ID IN (26, 24, 25, 50, 34, 35) THEN  'AL'
WHEN Lagerort_ID IN (15, 8) THEN  'DE'
ELSE ''
END AS Unit
from [Lager] inner join Artikel on Lager.[Artikel-Nr]=Artikel.[Artikel-Nr]
where Artikel.Warengruppe<>'EF' and Bestand<>0 
and Lagerort_ID in (42, 41, 40, 57, 49, 46, 47,101, 102, 109, 105, 104, 107, 108,6, 3, 52, 9, 53, 17,26, 24, 25, 50, 34, 35,15, 8)
) L GROUP BY [Artikel-Nr],[Year],[Week],Unit
) X GROUP BY ArtikelNr ,[Year],[Week],Unit



IF OBJECT_ID('dbo.__PRS_StockWarnings_Cumuls', 'U') IS NOT NULL DROP TABLE dbo.__PRS_StockWarnings_Cumuls;
SELECT t.[ArtikelNr], t.[Year], t.[Week],t.[Qty],t.[Unit],
sum(Qty) over (partition by ArtikelNr,[Unit] order by [Year], [Week],[Unit]) as SumProd_cumul
into __PRS_StockWarnings_Cumuls
FROM #Temp01 t
order by [ArtikelNr],[Unit],[Year],[Week]

IF OBJECT_ID('dbo.__PRS_StockWarnings_Articles', 'U') IS NOT NULL DROP TABLE dbo.__PRS_StockWarnings_Articles;
SELECT DISTINCT ArtikelNr,3 AS Prio
INTO __PRS_StockWarnings_Articles
from __PRS_StockWarnings_Cumuls WHERE SumProd_cumul<0

declare @prio1 datetime=DATEADD(day,42,GETDATE())
declare @prio2 datetime=DATEADD(day,90,GETDATE())

update __PRS_StockWarnings_Articles set Prio=1 where ArtikelNr IN(
select distinct ArtikelNr from __PRS_StockWarnings_Cumuls
where [Year]<Year(@prio1) or ([Year]=Year(@prio1) AND [Week]<=DATEPART(ISO_WEEK,@prio1))
and SumProd_cumul<0
)

update __PRS_StockWarnings_Articles set Prio=2 where ArtikelNr IN(
select distinct ArtikelNr from __PRS_StockWarnings_Cumuls
where [Year]<Year(@prio2) or ([Year]=Year(@prio2) AND [Week]<=DATEPART(ISO_WEEK,@prio2))
and SumProd_cumul<0
)

INSERT INTO __PRS_StockWarnings_ComputeLogs ([Date],[UserId]) VALUES (GETDATE(),@UserId)

IF OBJECT_ID('tempdb..#Temp01') IS NOT NULL DROP TABLE #Temp01;
GO

