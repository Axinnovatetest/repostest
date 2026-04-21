USE [ERP_WEB]
GO

/****** Object:  StoredProcedure [dbo].[usp_prs_compute_fa]    Script Date: 21.01.2025 10:52:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[usp_prs_compute_fa]
as
IF OBJECT_ID('dbo.__PRS_StockWarnings_FaStatus', 'U') IS NOT NULL DROP TABLE dbo.__PRS_StockWarnings_FaStatus;
SELECT [Week],[Year],SUM(Qty) Qty,ArtikelNr,Unit
INTO __PRS_StockWarnings_FaStatus
FROM (
SELECT DATEPART(ISO_WEEK, Termin_Bestätigt1) as [Week],DATEPART(year,Termin_Bestätigt1) AS [Year], 
SUM(FPAnzahl / Originalanzahl * FAnzahl) Qty, [Artikel_Nr] ArtikelNr,Lagerort_Id,
CASE
WHEN Lagerort_Id IN (42,7) THEN  'WS-TN'
WHEN Lagerort_ID IN (102) THEN  'GZ'
WHEN Lagerort_ID IN (6) THEN  'CZ'
WHEN Lagerort_ID IN (26) THEN  'AL'
WHEN Lagerort_ID IN (15) THEN  'DE'
ELSE ''
END AS Unit
FROM
(
SELECT  Termin_Bestätigt1, FP.[Artikel_Nr], FP.Anzahl FPAnzahl, F.Originalanzahl, F.Anzahl FAnzahl,F.Lagerort_ID
FROM Fertigung F INNER JOIN Fertigung_Positionen FP ON F.ID = FP.ID_Fertigung 
AND F.Termin_Bestätigt1 <= DATEADD(MONTH, 6, GETDATE()) 
AND F.Kennzeichen = 'offen' AND IsNULL(F.FA_Gestartet, 0) = 0 AND F.Originalanzahl <> 0 
AND F.Originalanzahl IS NOT NULL AND F.Anzahl IS NOT NULL
AND F.Lagerort_ID in (7,42,102,26,6,15)
inner join Artikel A on FP.Artikel_Nr=A.[Artikel-Nr]
) X Group by [Artikel_Nr],Lagerort_ID,Termin_Bestätigt1 
) W GROUP BY [Week],[Year],ArtikelNr,Unit
GO

