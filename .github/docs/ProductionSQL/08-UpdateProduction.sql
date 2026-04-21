

DECLARE @nexProductionNumber int = 1710492;

UPDATE Fertigung 
	SET Fertigung.Bemerkung = Fertigung.Bemerkung, Fertigung.[Bemerkung ohne stätte] = Fertigung.[Bemerkung ohne stätte]
FROM Lagerorte INNER JOIN Fertigung ON Lagerorte.Lagerort_id = Fertigung.Lagerort_id
WHERE Fertigung.Fertigungsnummer = @nexProductionNumber;