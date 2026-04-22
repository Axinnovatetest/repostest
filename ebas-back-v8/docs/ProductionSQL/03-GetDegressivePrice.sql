
DECLARE @positionArtikelnummer int  = 15;
DECLARE @productionAmount real = 90;


SELECT 
	Artikel.[Artikel-Nr], 
	CASE WHEN Preisgruppen.ME4>0 And @productionAmount <= Preisgruppen.ME4 And @productionAmount > Preisgruppen.ME3 And Not Preisgruppen.Staffelpreis4 IS NULL THEN 'S4' ELSE 
		CASE WHEN Preisgruppen.ME3>0 And @productionAmount <= Preisgruppen.ME3 And @productionAmount > Preisgruppen.ME2 And Not Preisgruppen.Staffelpreis3 IS NULL THEN 'S3' ELSE 
			CASE WHEN Preisgruppen.ME2>0 And @productionAmount <= Preisgruppen.ME2 And @productionAmount > Preisgruppen.ME1 And Not Preisgruppen.Staffelpreis2 IS NULL THEN 'S2' ELSE 
				CASE WHEN Preisgruppen.ME1>0 And @productionAmount <= Preisgruppen.ME1 And Not Preisgruppen.Staffelpreis1  IS NULL THEN 'S1' ELSE 'S0' END
			END 
		END
	END AS TypeStaff, 
	Artikel.Artikelnummer INTO ##T_Staffe_vorhandeln
FROM 
	(Artikel INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
	INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
WHERE Artikel.[Artikel-Nr] = @positionArtikelnummer;