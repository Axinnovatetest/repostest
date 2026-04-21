
DECLARE @nexProductionNumber int = 1710492;

UPDATE Fertigung SET Fertigung.gebucht = 1, Fertigung.Kennzeichen = 'Offen'
WHERE Fertigung.Fertigungsnummer = @nexProductionNumber;

