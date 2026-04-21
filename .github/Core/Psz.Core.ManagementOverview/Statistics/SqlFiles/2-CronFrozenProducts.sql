--delete from  stats.ProductionFrozen
insert into 
  [stats].[Sync]
  values (getdate(),2)
					
Declare @SyncIdUsed as int =(Select IDENT_CURRENT('[stats].[Sync]') 'IDENT_CURRENT')

							insert into stats.ProductionFrozen
							(Artikel_Nr,Anzahl,Originalanzahl,Anzahl_erledigt,Anzahl_aktuell,Angebot_nr,Angebot_Artikel_Nr,AnzahlnachgedrucktPPS,Ausgangskontrolle,
							Bemerkung,Bemerkung_II_Planung,
							Bemerkung_ohne_statte,
							Bemerkung_Kommissionierung_AL,
							Bemerkung_Planung,
							Bemerkung_Technik,
							Bemerkung_zu_Prio,
							BomVersion,
							CAO,
							Check_FAbegonnen,
							Check_Gewerk1,
							Check_Gewerk1_Teilweise,
							Check_Gewerk2,
							Check_Gewerk2_Teilweise,
							Check_Gewerk3,
							Check_Gewerk3_Teilweise,
							Check_Kabelgeschnitten,
							CPVersion,
							Datum,
							Endkontrolle,
							Erledigte_FA_Datum,
							Erstmuster,
							FA_begonnen,
							FA_Druckdatum,
							FA_Gestartet,
							Fa_NachdruckPPS,
							Fertigungsnummer,
							gebucht,
							gedruckt,
							Gewerk_1,
							Gewerk_2,
							Gewerk_3,
							Gewerk_Teilweise_Bemerkung,
							GrundNachdruckPPS,
							HBGFAPositionId,
							ID_Hauptartikel,
							ID_Rahmenfertigung,
							Kabel_geschnitten,
							Kabel_geschnitten_Datum,
							Kennzeichen,
							Kommisioniert_komplett,
							Kommisioniert_teilweise,
							Kunden_Index_Datum,
							KundenIndex,
							Lagerort_id,
							Lagerort_id_zubuchen,
							LastUpdateDate,
							Mandant,
							Letzte_Gebuchte_Menge,
							Menge1,
							Menge2,
							Preis,Planungsstatus,Prio,Quick_Area,ROH_umgebucht,Spritzgiesserei_abgeschlossen,
							Tage_Abweichung,Technik,


							Techniker,Termin_Bestatigt1,Termin_Bestatigt2,Termin_Fertigstellung,
							Termin_Material,Termin_Ursprunglich,Termin_voranderung,
							UBG,
							UBGTransfer ,
							Urs_Artikelnummer ,
							Urs_Fa ,
							Zeit ,
							lastUpdateKW ,
							TotalCount,
							SyncId
							)
SELECT
F.Artikel_Nr,F.Anzahl,F.Originalanzahl,F.Anzahl_erledigt,F.Anzahl_aktuell,F.Angebot_nr,F.Angebot_Artikel_Nr,F.AnzahlnachgedrucktPPS,F.Ausgangskontrolle
,F.Bemerkung,F.[Bemerkung II Planung],F.[Bemerkung ohne stätte],F.Bemerkung_Kommissionierung_AL,F.Bemerkung_Planung,F.Bemerkung_Technik,F.Bemerkung_zu_Prio,
F.BomVersion,F.CAO,F.Check_FAbegonnen,F.Check_Gewerk1,F.Check_Gewerk1_Teilweise,Check_Gewerk2,F.Check_Gewerk2_Teilweise,F.Check_Gewerk3,
F.Check_Gewerk3_Teilweise,F.Check_Kabelgeschnitten,F.CPVersion,F.Datum,F.Endkontrolle,F.Erledigte_FA_Datum,F.Erstmuster,F.FA_begonnen,
F.FA_Druckdatum,F.FA_Gestartet,F.[Fa-NachdruckPPS],F.Fertigungsnummer,F.gebucht,F.gedruckt,F.[Gewerk 1],F.[Gewerk 2],F.[Gewerk 3],
F.Gewerk_Teilweise_Bemerkung,F.GrundNachdruckPPS,F.HBGFAPositionId,F.ID_Hauptartikel,F.ID_Rahmenfertigung,
F.Kabel_geschnitten,F.Kabel_geschnitten_Datum,F.Kennzeichen,F.Kommisioniert_komplett,F.Kommisioniert_teilweise,F.Kunden_Index_Datum,
F.KundenIndex,F.Lagerort_id,F.[Lagerort_id zubuchen],t.LastUpdateDate,F.Mandant,F.Letzte_Gebuchte_Menge,
F.Menge1,F.Menge2,F.Preis,F.Planungsstatus,F.Prio,F.Quick_Area,F.ROH_umgebucht,F.[Spritzgießerei_abgeschlossen],F.[Tage Abweichung],F.Technik,

F.Techniker,F.[Termin_Bestätigt1],F.[Termin_Bestätigt2],F.[Termin_Fertigstellung],
F.Termin_Material,F.[Termin_Ursprünglich],F.[Termin_voränderung],
UBG,
UBGTransfer ,
F.[Urs-Artikelnummer] ,
F.[Urs-Fa] ,
Zeit ,
DATEPART(ISO_WEEK, t.LastUpdateDate) as lastUpdateKW  ,
Count(*) over() as TotalCount,
@SyncIdUsed
--F.*
--, DATEPART(ISO_WEEK, t.LastUpdateDate) as lastUpdateKW 
FROM Fertigung F Join (
			SELECT fertigungsnummer, max(Änderungsdatum) LastUpdateDate FROM PSZ_Historique_Import_Excel_FA
					WHERE fertigungsnummer in (SELECT fertigungsnummer FROM fertigung WHERE Kennzeichen='offen')
							GROUP BY fertigungsnummer
							) as t on t.fertigungsnummer=f.fertigungsnummer
							

