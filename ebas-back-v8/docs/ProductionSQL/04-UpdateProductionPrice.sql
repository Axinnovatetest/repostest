
DECLARE @productionFirstSample bit = 1;
DECLARE @productionPrice real = 120;
DECLARE @T_Max_Id_FertgungID int = 90872;

--UPDATE 
--	Fertigung
--	SET Fertigung.Preis = 
--		CASE WHEN  @productionFirstSample = 1 THEN [artikel_kalkulatorische_kosten].[Betrag] + @productionPrice ELSE
--			CASE WHEN [T_Staffe_vorhandeln].[TypeStaff]='S1' 
--				Or [T_Staffe_vorhandeln].[TypeStaff]='S2' 
--				Or [T_Staffe_vorhandeln].[TypeStaff]='S3' 
--				Or [T_Staffe_vorhandeln].[TypeStaff]='S4' THEN [Staffelpreis_Konditionzuordnung].[Betrag] ELSE [artikel_kalkulatorische_kosten].[Betrag]
--			END
--		END, 
--		Fertigung.Zeit = 
--		CASE WHEN [T_Staffe_vorhandeln].[TypeStaff]='S1' 
--				Or [T_Staffe_vorhandeln].[TypeStaff]='S2' 
--				Or [T_Staffe_vorhandeln].[TypeStaff]='S3' 
--				Or [T_Staffe_vorhandeln].[TypeStaff]='S4' THEN [Staffelpreis_Konditionzuordnung].[ProduKtionzeit] ELSE [Artikel].[Produktionszeit] END
--	FROM
--		(((Artikel INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
--		INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
--		INNER JOIN ##T_Staffe_vorhandeln AS T_Staffe_vorhandeln ON Artikel.[Artikel-Nr] = T_Staffe_vorhandeln.[Artikel-Nr]) 
--		LEFT JOIN Staffelpreis_Konditionzuordnung 
--			ON (T_Staffe_vorhandeln.TypeStaff = Staffelpreis_Konditionzuordnung.Staffelpreis_Typ) AND (T_Staffe_vorhandeln.[Artikel-Nr] = Staffelpreis_Konditionzuordnung.Artikel_Nr), 
--WHERE Fertigung.ID = @T_Max_Id_FertgungID 


SELECT 
		CASE WHEN  @productionFirstSample = 1 THEN [artikel_kalkulatorische_kosten].[Betrag] + @productionPrice ELSE
			CASE WHEN [T_Staffe_vorhandeln].[TypeStaff]='S1' 
				Or [T_Staffe_vorhandeln].[TypeStaff]='S2' 
				Or [T_Staffe_vorhandeln].[TypeStaff]='S3' 
				Or [T_Staffe_vorhandeln].[TypeStaff]='S4' THEN [Staffelpreis_Konditionzuordnung].[Betrag] ELSE [artikel_kalkulatorische_kosten].[Betrag]
			END
		END as Preis, 
		
		CASE WHEN [T_Staffe_vorhandeln].[TypeStaff]='S1' 
				Or [T_Staffe_vorhandeln].[TypeStaff]='S2' 
				Or [T_Staffe_vorhandeln].[TypeStaff]='S3' 
				Or [T_Staffe_vorhandeln].[TypeStaff]='S4' THEN [Staffelpreis_Konditionzuordnung].[ProduKtionzeit] ELSE [Artikel].[Produktionszeit] END as Zeit
	FROM
		(((Artikel INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
		INNER JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
		INNER JOIN ##T_Staffe_vorhandeln AS T_Staffe_vorhandeln ON Artikel.[Artikel-Nr] = T_Staffe_vorhandeln.[Artikel-Nr]) 
		LEFT JOIN Staffelpreis_Konditionzuordnung 
			ON (T_Staffe_vorhandeln.TypeStaff = Staffelpreis_Konditionzuordnung.Staffelpreis_Typ) AND (T_Staffe_vorhandeln.[Artikel-Nr] = Staffelpreis_Konditionzuordnung.Artikel_Nr),
			Fertigung
WHERE Fertigung.ID = @T_Max_Id_FertgungID 

