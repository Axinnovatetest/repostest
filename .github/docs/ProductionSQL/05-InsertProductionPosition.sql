
DECLARE @nexProductionNumber int = 1710492;


INSERT INTO Fertigung_Positionen  ( ID_Fertigung_HL, ID_Fertigung, Artikel_Nr, Anzahl, Lagerort_ID, buchen, Vorgang_Nr, [ME gebucht] )
	SELECT 
		Fertigung.ID as ID_Fertigung_HL, 
		Fertigung.ID as ID_Fertigung, 
		Stücklisten.[Artikel-Nr des Bauteils] as Artikel_Nr, 
		Fertigung.Anzahl*Stücklisten.Anzahl as Anzahl, 
		Fertigung.Lagerort_id, 
		1 AS buchen, 
		Stücklisten.Vorgang_Nr, 
		0 AS [ME gebucht]
	FROM 
		Fertigung INNER JOIN Stücklisten ON Fertigung.Artikel_Nr = Stücklisten.[Artikel-Nr]
	WHERE Fertigung.Fertigungsnummer = @nexProductionNumber;