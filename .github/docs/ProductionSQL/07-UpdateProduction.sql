

DECLARE @nexProductionNumber int = 1710492;

UPDATE Fertigung 
	SET Fertigung.Bemerkung = Lagerorte.Lagerort +',  '+ Fertigung.Bemerkung
FROM Lagerorte INNER JOIN Fertigung ON Lagerorte.Lagerort_id = Fertigung.Lagerort_id
WHERE Fertigung.Fertigungsnummer = @nexProductionNumber;