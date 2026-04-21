


UPDATE Fertigung SET Fertigung.[Gewerk 2] = 'False'
	FROM [Gewerke zu FA ermitteln02] INNER JOIN Fertigung ON [Gewerke zu FA ermitteln02].Fertigungsnummer = Fertigung.Fertigungsnummer
WHERE [Gewerke zu FA ermitteln02].Gewerk = 'Gewerk 2';