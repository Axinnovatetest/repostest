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