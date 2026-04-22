

SELECT T.Gewerk,  T.Fertigungsnummer FROM  
	(SELECT Artikel_1.Artikelnummer AS Artikelnummer1, Artikel_1.[Bezeichnung 1] AS B1, 
			Artikel_1.[Bezeichnung 2] AS B2, 
			Fertigung.Anzahl AS Anzahl1, 
			Lagerorte.Lagerort AS L1, 
			Fertigung.Fertigungsnummer, 
			Fertigung.Datum, 
			Fertigung.Termin_Fertigstellung, 
			Fertigung.Kennzeichen, 
			Fertigung.Bemerkung, 
			Artikel.Artikelnummer, 
			Artikel.[Bezeichnung 1], 
			Artikel.[Bezeichnung 2], 
			Fertigung_Positionen.Anzahl, 
			Fertigung_Positionen.Arbeitsanweisung, 
			Fertigung_Positionen.Fertiger, 
			Fertigung_Positionen.Termin_Soll, 
			Fertigung_Positionen.Bemerkungen, 
			Lagerorte_1.Lagerort, 
			Artikel_1.EAN, 
			artikel_kalkulatorische_kosten.Betrag, 
			Artikel_1.Freigabestatus, 
			Fertigung.Zeit AS Produktionszeit, 
			Fertigung.Termin_Bestätigt1, 
			Fertigung.Erstmuster, 
			Artikel_1.[Freigabestatus TN intern] , 
			Artikel_1.Index_Kunde, 
			Fertigung.[Lagerort_ID zubuchen], 
			Fertigung.Mandant, Artikel_1.Sysmonummer AS S1, 
			Artikel.Sysmonummer, 
			Artikel_1.[UL Etikett], 
			Fertigung.Technik, 
			Fertigung.Techniker,
			Artikel_1.Kanban, 
			Artikel_1.Verpackungsart, 
			Artikel_1.Verpackungsmenge, 
			Artikel_1.Losgroesse, 
			Fertigung.Quick_Area, 
			Artikel_1.Artikelfamilie_Kunde, 
			Artikel_1.Artikelfamilie_Kunde_Detail1, 
			Artikel_1.Artikelfamilie_Kunde_Detail2, 
			Artikel.Klassifizierung, 
			Artikelstamm_Klassifizierung.Bezeichnung, 
			Artikelstamm_Klassifizierung.Nummernkreis, 
			Artikelstamm_Klassifizierung.Kupferzahl, 
			Artikelstamm_Klassifizierung.ID, 
			Artikelstamm_Klassifizierung.Gewerk 
			FROM 
			(( (Lagerorte AS Lagerorte_1 INNER JOIN 
				((Artikel AS Artikel_1 INNER JOIN 
				(Artikel INNER JOIN 
				(Fertigung INNER JOIN Fertigung_Positionen 
					ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung) 
					ON Artikel.[Artikel-Nr] = Fertigung_Positionen.Artikel_Nr) 
					ON Artikel_1.[Artikel-Nr] = Fertigung.Artikel_Nr) INNER JOIN Lagerorte 
					ON Fertigung.Lagerort_ID = Lagerorte.Lagerort_ID) 
					ON Lagerorte_1.Lagerort_ID = Fertigung_Positionen.Lagerort_ID) 
				LEFT JOIN artikel_kalkulatorische_kosten 
					ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[Artikel-Nr]) 
				INNER JOIN Artikelstamm_Klassifizierung 
					ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID)  
			WHERE Artikelstamm_Klassifizierung.Gewerk IS NOT NULL AND Fertigung.Fertigungsnummer=1996380 )
T GROUP BY T.Klassifizierung, T.Gewerk, T.Artikelnummer1, T.Anzahl1, T.Termin_Bestätigt1, T.Fertigungsnummer, T.Bezeichnung, T.B1, T.Artikelfamilie_Kunde 
ORDER BY T.Gewerk;