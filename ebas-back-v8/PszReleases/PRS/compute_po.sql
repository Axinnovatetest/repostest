USE [ERP_WEB]
GO

/****** Object:  StoredProcedure [dbo].[usp_prs_compute_po]    Script Date: 21.01.2025 10:53:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[usp_prs_compute_po]
as
IF OBJECT_ID('dbo.__PRS_StockWarnings_PoStatus', 'U') IS NOT NULL DROP TABLE dbo.__PRS_StockWarnings_PoStatus;
SELECT [Week],[Year],SUM(Qty) Qty,ArtikelNr,Unit
INTO __PRS_StockWarnings_PoStatus
FROM(
SELECT DATEPART(ISO_WEEK, Liefertermin) as [Week],DATEPART(Year,Liefertermin) AS [Year],
SUM(Anzahl) Qty,[Artikel-Nr] ArtikelNr,Lagerort_Id,
CASE
WHEN Lagerort_Id IN (42, 41, 40, 57, 49, 46, 47) THEN  'WS-TN'
WHEN Lagerort_ID IN (101, 102, 109, 105, 104, 107, 108) THEN  'GZ'
WHEN Lagerort_ID IN (6, 3, 52, 9, 53, 17) THEN  'CZ'
WHEN Lagerort_ID IN (26, 24, 25, 50, 34, 35) THEN  'AL'
WHEN Lagerort_ID IN (15, 8) THEN  'DE'
ELSE ''
END AS Unit
FROM
(
SELECT ba.[Artikel-Nr], Anzahl,Lagerort_id,ba.Liefertermin
FROM [bestellte Artikel] ba INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
AND b.Typ = 'Bestellung' 
inner join Artikel A on A.[Artikel-Nr]=ba.[Artikel-Nr]
WHERE Anzahl > 0 AND ba.Liefertermin <= DATEADD(MONTH, 6, GETDATE()) 
AND ISNULL([Position erledigt],0) <> 1 AND Anzahl > 0 and ISNULL(b.erledigt,0)<>1 and ISNULL(erledigt_pos,0)<>1 and b.gebucht=1
and Lagerort_ID in (42, 41, 40, 57, 49, 46, 47,101, 102, 109, 105, 104, 107, 108,6, 3, 52, 9, 53, 17,26, 24, 25, 50, 34, 35,15, 8)
) X GROUP BY [Artikel-Nr],Lagerort_id,DATEPART(ISO_WEEK, Liefertermin),DATEPART(Year,Liefertermin) 
) Z GROUP BY [Week],[Year],ArtikelNr,Unit

GO

