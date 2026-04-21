
DECLARE @nexProductionNumber int = 1710492;

UPDATE Fertigung SET Fertigung.Preis = artikel_kalkulatorische_kosten.Betrag, Fertigung.Zeit = Artikel.Produktionszeit
	FROM (Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
WHERE Fertigung.Fertigungsnummer = @nexProductionNumber AND artikel_kalkulatorische_kosten.Kostenart = 'Arbeitskosten';