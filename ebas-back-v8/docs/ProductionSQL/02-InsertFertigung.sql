

DECLARE @nextProductionNumber int = 954786;
DECLARE @manufacturingFacilityId int = 10;
DECLARE @positionDeliveryDate date = getdate();
DECLARE @productionDate date = getdate();
DECLARE @productionPrice int  = 0;

DECLARE @productionFirstSample bit = 1;
DECLARE @productionType nvarchar(100) = '------';
DECLARE @productionContact nvarchar(100) = '****';
DECLARE @productionUser nvarchar(100) = ' U, ///';
DECLARE @productionTechnical bit = 0;
DECLARE @productionQuick_Area nvarchar(100) ='RR';
DECLARE @productionOriginalArtikelnummer int = 1254;
DECLARE @productionUBG bit = 1;
DECLARE @productionUBGTransfer bit = 0;

DECLARE @prositionId int = 1450235;
DECLARE @positionItemNumber int = 3604;


INSERT INTO Fertigung 
	(Angebot_nr, Angebot_Artikel_Nr, Artikel_Nr, Anzahl, Lagerort_id, Fertigungsnummer, Datum, Termin_Fertigstellung, 
	Termin_Bestätigt1, Kennzeichen, Preis, gebucht, Bemerkung, Originalanzahl, Bemerkung_Planung, Mandant, 
	[Lagerort_id zubuchen], Techniker, Erstmuster, Technik, [Bemerkung ohne stätte], Termin_Ursprünglich, Quick_Area, 
	KundenIndex, [Urs-Artikelnummer], UBG, UBGTransfer )

SELECT Angebote.Nr as Angebot_nr, [angebotene Artikel].Nr as Angebot_Artikel_Nr, [angebotene Artikel].[Artikel-Nr] as Artikel_Nr, [angebotene Artikel].Anzahl, 
	@manufacturingFacilityId AS Lagerort_id, 
	@nextProductionNumber AS Fertigungsnummer, 
	getdate() AS Datum, 
	@positionDeliveryDate AS Termin_Fertigstellung, 
	@productionDate AS Termin_Bestätigt1,  
	'gesperrt' AS Kennzeichen, 
	CASE WHEN @productionFirstSample = 1 THEN artikel_kalkulatorische_kosten.Betrag +  @productionPrice
    ELSE artikel_kalkulatorische_kosten.Betrag END AS Preis, 
	0 AS gebucht, 
	(Angebote.[Vorname/NameFirma] +': '+ Angebote.Bezug +',  '+ @productionType +',  '+ @productionContact) AS Bemerkung, 
	[angebotene Artikel].Anzahl as OriginalAnzahl, 
	'Erstellt: ' + @productionUser AS Bemerkung_Planung,
	Angebote.Mandant, 
	[angebotene Artikel].Lagerort_id as [Lagerort_id zubuchen], 
	@productionTechnical AS Techniker, 
	@productionFirstSample AS Erstmuster, 
	@productionTechnical AS Technik, 
	('Eigenfertigung ,  '+ Angebote.[Vorname/NameFirma] +': '+ Angebote.Bezug +', '+ '.  ') AS [Bemerkung ohne stätte], 
	@productionDate AS Termin_Ursprünglich, 
	@productionQuick_Area AS Quick_Area, 
	Artikel.Index_Kunde, 
	@productionOriginalArtikelnummer AS [Urs-Artikelnummer], 
	@productionUBG AS UBG, 
	@productionUBGTransfer AS UBGTransfer

FROM Lagerorte, 
	(Angebote INNER JOIN ([angebotene Artikel] INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
		ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
	INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
WHERE Angebote.Nr=@prositionId AND [angebotene Artikel].Nr=@positionItemNumber AND [Lagerorte].[Standard] =1 AND [artikel_kalkulatorische_kosten].[Kostenart]='Arbeitskosten';



SELECT top 10
	Angebot_nr, Angebot_Artikel_Nr, Artikel_Nr, Anzahl, Lagerort_id, Fertigungsnummer, Datum, Termin_Fertigstellung, 
	Termin_Bestätigt1, Kennzeichen, Preis, gebucht, Bemerkung, Originalanzahl, Bemerkung_Planung, Mandant, 
	[Lagerort_id zubuchen], Techniker, Erstmuster, Technik, [Bemerkung ohne stätte], Termin_Ursprünglich, Quick_Area, 
	KundenIndex, [Urs-Artikelnummer], UBG, UBGTransfer
FROM Fertigung

