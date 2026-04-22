
DECLARE @nexProductionNumber int = 1710492;

UPDATE [angebotene Artikel] SET [angebotene Artikel].Fertigungsnummer = Fertigung.Fertigungsnummer
	FROM Fertigung INNER JOIN [angebotene Artikel] ON Fertigung.Angebot_Artikel_Nr = [angebotene Artikel].Nr 
WHERE Fertigung.Fertigungsnummer = @nexProductionNumber;