insert into 
  [stats].[Sync]
  values (getdate(),5)
					
Declare @SyncIdUsed as int =(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')


Insert into [stats].BedarfAnalyse
(
	Artikelnummer,ROH_Bestand,Einkaufspreis,Gesamtpreis,Name1,Bestell_Nr,ROH_Quantity,Wert_LagerBestandBedarf,
		DiffQuantity,
		DiffPrice,
		TerminBestatigt,

		SyncId
)



SELECT   t1.*, t2.ROH_Quantity, ISNULL(ROH_Quantity,0) * Einkaufspreis  as Wert_LagerBestandBedarf,
								(ROH_Bestand - ISNULL(ROH_Quantity,0)) DiffQuantity, (ROH_Bestand - ISNULL(ROH_Quantity,0)) * Einkaufspreis as DiffPrice 
								,t2.TerminBestatigt
								,@SyncIdUsed 
								FROM (SELECT Artikelnummer, SUM(ROH_quantity) as ROH_Quantity,TerminBestatigt FROM
								(
								SELECT Artikel.Artikelnummer, Fertigung_Positionen.Anzahl PositionAnzahl, Fertigung.Kennzeichen, Fertigung.FA_Gestartet, Fertigung.Fertigungsnummer, 
								Fertigung.Originalanzahl, Fertigung.Anzahl, Fertigung_Positionen.Anzahl / Fertigung.Originalanzahl * Fertigung.Anzahl AS ROH_Quantity, Fertigung.Lagerort_id
								,Fertigung.Termin_Bestätigt1 as TerminBestatigt

								FROM Fertigung 
									INNER JOIN Fertigung_Positionen ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung_HL 
									INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
								WHERE (Fertigung.Kennzeichen = N'Offen') and (Fertigung.Termin_Bestätigt1<=getdate())
								) AS Tmp GROUP BY Artikelnummer,TerminBestatigt
								) t2 
								Left Join (SELECT Artikelnummer, SUM(Bestand) as ROH_Bestand, (Einkaufspreis), SUM(Bestand*(Einkaufspreis)) as Gesamtpreis,Name1,[Bestell-Nr] FROM (
								SELECT a.Name1,Bestellnummern.[Bestell-Nr], Artikel.Artikelnummer, Bestellnummern.Standardlieferant, Lager.Bestand+Lager.Bestand_reserviert as Bestand, 
								Lager.Lagerort_id, Artikel.Warengruppe, Bestellnummern.Einkaufspreis
								FROM Artikel 
									INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr] 
									and Lagerort_id not in 
									  (
										SELECT  [Lagerort_id]
										FROM [dbo].[Lagerorte] where [Lagerort] like 'AUss%'
									  )
									INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
									Join adressen a on a.Nr=Bestellnummern.[Lieferanten-Nr]
								WHERE (Artikel.Warengruppe <> N'EF') aND Bestellnummern.Standardlieferant=1
								) AS Tmp GROUP BY Artikelnummer,Einkaufspreis,Name1,[Bestell-Nr]
								) t1 on t1.Artikelnummer=t2.Artikelnummer Where ISNULL((ROH_Bestand - ISNULL(ROH_Quantity,0)),-1)<0

