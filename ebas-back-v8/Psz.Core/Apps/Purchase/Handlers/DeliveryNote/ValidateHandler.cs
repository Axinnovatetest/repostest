namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class ValideHandler //: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		//private Identity.Models.UserModel _user { get; set; }
		//private Models.DeliveryNote.ValidateModel _data { get; set; }
		//public ValideHandler(Identity.Models.UserModel user, Models.DeliveryNote.ValidateModel model)
		//{
		//    _user = user;
		//    _data = model;
		//}
		//public ResponseModel<int> Handle()
		//{
		//    try
		//    {
		//        var validationResponse = this.Validate();
		//        if (!validationResponse.Success)
		//        {
		//            return validationResponse;
		//        }

		//        lock (Locks.DeliveryNotesLock)
		//        {
		//            var errors = new List<KeyValuePair<string, string>>();

		//            var angeboteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.Nr);
		//            var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(Convert.ToInt32(angeboteEntity.Kunden_Nr ?? 0));

		//            // 0.1 - 
		//            var angeboteTermin = this._data.Items.All(x => x.termin_eingehalten == true); // >>>> ?Ridha
		//            angeboteEntity.Termin_eingehalten = angeboteTermin; // >>>> § 0.1 -
		//            Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(angeboteEntity);

		//            // >>>>>> Logging
		//            Infrastructure.Services.Logging.Logger.Log(new Exception($" OrderImport[DeliveryNote Validate] >>>>>> insert orderDb "));

		//            var insertedNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity
		//            {
		//                Projekt_Nr = angeboteEntity.Projekt_Nr,
		//                Angebot_Nr = 0,
		//                Typ = "Lieferschein",
		//                Datum = DateTime.Now,
		//                Liefertermin = angeboteEntity.Liefertermin,
		//                Kunden_Nr = angeboteEntity.Kunden_Nr,
		//                Debitorennummer = angeboteEntity.Debitorennummer,
		//                Falligkeit = DateTime.Now.AddDays(30),
		//                Anrede = angeboteEntity.Anrede,
		//                Vorname_NameFirma = angeboteEntity.Vorname_NameFirma,
		//                Name2 = angeboteEntity.Name2,
		//                Name3 = angeboteEntity.Name3,
		//                Ansprechpartner = angeboteEntity.Ansprechpartner,
		//                Abteilung = angeboteEntity.Abteilung,
		//                Straße_Postfach = angeboteEntity.Straße_Postfach,
		//                Land_PLZ_Ort = angeboteEntity.Land_PLZ_Ort,
		//                Briefanrede = angeboteEntity.Briefanrede,
		//                LAnrede = angeboteEntity.LAnrede,
		//                LVorname_NameFirma = angeboteEntity.LVorname_NameFirma,
		//                LName2 = angeboteEntity.LName2,
		//                LName3 = angeboteEntity.LName3,
		//                LAnsprechpartner = angeboteEntity.LAnsprechpartner,
		//                LAbteilung = angeboteEntity.LAbteilung,
		//                LStraße_Postfach = angeboteEntity.LStraße_Postfach,
		//                LLand_PLZ_Ort = angeboteEntity.LLand_PLZ_Ort,
		//                LBriefanrede = angeboteEntity.LBriefanrede,
		//                Personal_Nr = angeboteEntity.Personal_Nr,
		//                Versandart = angeboteEntity.Versandart,
		//                Zahlungsweise = angeboteEntity.Zahlungsweise,
		//                Konditionen = angeboteEntity.Konditionen,
		//                Zahlungsziel = angeboteEntity.Zahlungsziel,
		//                USt_Berechnen = angeboteEntity.USt_Berechnen,
		//                Bezug = angeboteEntity.Bezug,
		//                Ihr_Zeichen = angeboteEntity.Ihr_Zeichen,
		//                Unser_Zeichen = angeboteEntity.Unser_Zeichen,
		//                Freitext = angeboteEntity.Freie_Text,
		//                Gebucht = false,
		//                Gedruckt = false,
		//                Erledigt = false,
		//                Auswahl = angeboteEntity.Auswahl,
		//                Mahnung = angeboteEntity.Mahnung,
		//                Lieferadresse = angeboteEntity.Lieferadresse,
		//                Reparatur_nr = angeboteEntity.Reparatur_nr,
		//                Interessent = angeboteEntity.Interessent,
		//                Nr_auf = angeboteEntity.Nr,
		//                Ab_id = angeboteEntity.Nr,
		//                Status = angeboteEntity.Status,
		//                Bemerkung = angeboteEntity.Bemerkung,
		//                Bereich = angeboteEntity.Bereich,
		//                Belegkreis = angeboteEntity.Belegkreis,
		//                Wunschtermin = angeboteEntity.Wunschtermin,
		//                Datueber = true,
		//                Mandant = angeboteEntity.Mandant,
		//                Termin_eingehalten = angeboteTermin,

		//            });
		//            if (this._data.VersandBerechnen)
		//            {
		//                Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
		//                {
		//                    AngebotNr = insertedNr,
		//                    ArtikelNr = 223,
		//                    Bezeichnung1 = "Fracht+Verpackung",
		//                    Anzahl = 1,
		//                    OriginalAnzahl = 0,
		//                    ///// >>> get [PSZ_Auftrag LS 051 Filter für Versandkosten] QUERY
		//                    Einzelpreis = this._data.Versandkosten.HasValue ? this._data.Versandkosten.Value : 0, //>>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0) 
		//                    Gesamtpreis = this._data.Versandkosten.HasValue ? this._data.Versandkosten.Value : 0, //>>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0)
		//                    Rabatt = 0,
		//                    USt = (kundenEntity.Umsatzsteuer_berechnen.HasValue && kundenEntity.Umsatzsteuer_berechnen.Value) ? 0.19m : 0,
		//                    Lagerbewegung = false,
		//                    Lagerbewegung_rückgängig = false,
		//                    Auswahl = false,
		//                    FM_Einzelpreis = 0,
		//                    FM_Gesamtpreis = 0,
		//                    Summenberechnung = false,
		//                    Preiseinheit = 1,
		//                    Preis_ausweisen = true,
		//                    Liefertermin = DateTime.Now,
		//                    erledigt_pos = false,
		//                    Stückliste = false,
		//                    Stückliste_drucken = false,
		//                    Langtext = "No",
		//                    Langtext_drucken = false,
		//                    Lagerort_id = 3,
		//                    Seriennummern_drucken = false,
		//                    Wunschtermin = new DateTime(2999, 12, 31),
		//                    Fertigungsnummer = 0,
		//                    Geliefert = 0,
		//                    Position = 999,
		//                    VKEinzelpreis = this._data.Versandkosten.HasValue ? this._data.Versandkosten.Value : 0, // >>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0) 
		//                    VKGesamtpreis = this._data.Versandkosten.HasValue ? this._data.Versandkosten.Value : 0, // >>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0)
		//                    Einzelkupferzuschlag = 0,
		//                    Gesamtkupferzuschlag = 0,
		//                    termin_eingehalten = false,
		//                });
		//            }
		//            if (this._data.Items != null && this._data.Items.Count > 0)
		//            {
		//                var angeboteArtikelEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.Nr)?
		//                    .Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false).ToList();

		//                List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> NewInsertedPositions = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
		//                foreach (var p in this._data.Items)
		//                {
		//                    if (p.Nr == -1)
		//                    {
		//                        var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(p.ArtikelNummer);
		//                        var itemsPricingGroup = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(new List<int> { artikelEntity.ArtikelNr }).FirstOrDefault();
		//                        var GET = new GetArtikelHandler(this._user, artikelEntity.ArtikelNummer, p.Anzahl).Handle().Body;
		//                        NewInsertedPositions.Add(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
		//                        {
		//                            AngebotNr = -1,
		//                            ArtikelNr = artikelEntity.ArtikelNr,
		//                            Bezeichnung1 = artikelEntity.Bezeichnung1,
		//                            Bezeichnung2 = artikelEntity.Bezeichnung2,
		//                            Bezeichnung3 = artikelEntity.Bezeichnung3,
		//                            Einheit = artikelEntity.Einheit,
		//                            Anzahl = p.AktuelleLiefermenge,
		//                            OriginalAnzahl = p.OriginalAnzahl,
		//                            Preisgruppe = itemsPricingGroup.Preisgruppe,
		//                            //Bestellnummer = angeboteneArtikelEntity.Bestellnummer,
		//                            Rabatt = 0m,
		//                            USt = Convert.ToDecimal(artikelEntity?.Umsatzsteuer ?? 0),
		//                            //POSTEXT = angeboteneArtikelEntity.POSTEXT,
		//                            Preiseinheit = GET.UnitPriceBasis,
		//                            Zeichnungsnummer = GET.DrawingIndex,
		//                            Liefertermin = DateTime.Now,
		//                            erledigt_pos = p.erledigt_pos,
		//                            Lagerort_id = p.Lagerort_id,
		//                            Wunschtermin = p.Wunschtermin,
		//                            Fertigungsnummer = p.Fertigungsnummer,
		//                            Geliefert = p.Geliefert,
		//                            LSPoszuABPos = p.Nr,
		//                            Position = p.Position,
		//                            VKFestpreis = p.FixedTotalPrice,
		//                            EKPreise_Fix = p.FixedUnitPrice,
		//                            Einzelpreis = GET.OpenQuantity_UnitPrice,
		//                            DELFixiert = p.DelFixed,
		//                            Abladestelle = p.UnloadingPoint,
		//                            termin_eingehalten = p.termin_eingehalten,
		//                            RP = p.RP,
		//                            // R4
		//                            Gesamtpreis = p.AktuelleLiefermenge / GET.UnitPriceBasis * GET.OpenQuantity_UnitPrice * (1 - 0m),
		//                            Kupferbasis = GET.CopperBase,
		//                            DEL = GET.DelNote,
		//                            EinzelCuGewicht = GET.CopperWeight,
		//                            GesamtCuGewicht = p.AktuelleLiefermenge * GET.OpenQuantity_CopperWeight,
		//                            Einzelkupferzuschlag = GET.CopperSurcharge,
		//                            VKGesamtpreis = p.AktuelleLiefermenge * GET.UnitPrice / GET.UnitPriceBasis,
		//                            Versandarten_Auswahl = this._data.Standardversand,
		//                            Versanddatum_Auswahl = this._data.Versandatum,
		//                            VKEinzelpreis = p.UnitPrice,
		//                            //
		//                            Versandinfo_von_CS = p.Versandinfo_von_CS,
		//                            Packstatus = p.Packstatus,
		//                            Gepackt_von = p.Gepackt_von,
		//                            Gepackt_Zeitpunkt = p.Gepackt_Zeitpunkt,
		//                            Packinfo_von_Lager = p.Packinfo_von_Lager,
		//                            //!Shipping
		//                            Versandstatus = p.Versandstatus,
		//                            Versanddienstleister = p.Versanddienstleister,
		//                            Versandnummer = p.Versandnummer.ToString(),
		//                            Versandinfo_von_Lager = p.Versandinfo_von_Lager,
		//                            EDI_PREIS_KUNDE = p.EDI_PREIS_KUNDE,
		//                            EDI_PREISEINHEIT = p.EDI_PREISEINHEIT,
		//                        });
		//                    }
		//                }
		//                if (NewInsertedPositions != null && NewInsertedPositions.Count > 0)
		//                {
		//                    angeboteArtikelEntities.AddRange(NewInsertedPositions);
		//                }

		//                var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(
		//                        angeboteArtikelEntities?
		//                            .Select(x => Convert.ToInt32(x.ArtikelNr ?? 0))?
		//                            .ToList());

		//                var articleNrs = artikelEntities?.Select(x => x.ArtikelNr)?.ToList();

		//                var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);

		//                var itemsDone = this._data.Items.FindAll(x => x.AktuelleLiefermenge > 0);
		//                List<int> InsertedIds = new List<int>();
		//                foreach (var item in itemsDone)
		//                {
		//                    var artikel = artikelEntities.FirstOrDefault(x => x.ArtikelNummer == item.ArtikelNummer);
		//                    var angeboteneArtikelEntity = angeboteArtikelEntities.FirstOrDefault(x => x.ArtikelNr == artikel.ArtikelNr);

		//                    angeboteneArtikelEntity.termin_eingehalten = item.termin_eingehalten;
		//                    Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(angeboteneArtikelEntity);

		//                    Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Edit(artikel);

		//                    //if (!this._data.VersandBerechnen)
		//                    //{
		//                    if (item.AktuelleLiefermenge > 0)
		//                    {
		//                        // R3
		//                        InsertedIds.Add(Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
		//                        {
		//                            AngebotNr = insertedNr,
		//                            ArtikelNr = artikel.ArtikelNr,
		//                            Bezeichnung1 = artikel.Bezeichnung1,
		//                            Bezeichnung2 = artikel.Bezeichnung2,
		//                            Bezeichnung3 = artikel.Bezeichnung3,
		//                            Einheit = artikel.Einheit,
		//                            Anzahl = item.AktuelleLiefermenge,
		//                            OriginalAnzahl = item.OriginalAnzahl,
		//                            Preisgruppe = angeboteneArtikelEntity.Preisgruppe,
		//                            Bestellnummer = angeboteneArtikelEntity.Bestellnummer,
		//                            Rabatt = angeboteneArtikelEntity.Rabatt,
		//                            USt = angeboteneArtikelEntity.USt,
		//                            POSTEXT = angeboteneArtikelEntity.POSTEXT,
		//                            Preiseinheit = angeboteneArtikelEntity.Preiseinheit,
		//                            Zeichnungsnummer = angeboteneArtikelEntity.Zeichnungsnummer,
		//                            Liefertermin = item.Liefertermin ?? DateTime.Now,
		//                            erledigt_pos = item.erledigt_pos,
		//                            Lagerort_id = item.Lagerort_id,
		//                            Wunschtermin = item.Wunschtermin,
		//                            Fertigungsnummer = angeboteneArtikelEntity.Fertigungsnummer,
		//                            Geliefert = angeboteneArtikelEntity.Geliefert,
		//                            LSPoszuABPos = angeboteneArtikelEntity.Nr,
		//                            Position = item.Position,
		//                            VKFestpreis = item.FixedTotalPrice,
		//                            EKPreise_Fix = item.FixedUnitPrice,
		//                            Einzelpreis = angeboteneArtikelEntity.Einzelpreis,
		//                            DELFixiert = item.DelFixed,
		//                            Abladestelle = item.UnloadingPoint,
		//                            termin_eingehalten = item.termin_eingehalten,
		//                            RP = item.RP,

		//                            // R4
		//                            Gesamtpreis = item.AktuelleLiefermenge / angeboteneArtikelEntity.Preiseinheit * angeboteneArtikelEntity.Einzelpreis * (1 - angeboteneArtikelEntity.Rabatt),
		//                            Kupferbasis = angeboteneArtikelEntity.Kupferbasis,
		//                            DEL = angeboteneArtikelEntity.DEL,
		//                            EinzelCuGewicht = angeboteneArtikelEntity.EinzelCuGewicht,
		//                            GesamtCuGewicht = item.AktuelleLiefermenge * angeboteneArtikelEntity.EinzelCuGewicht,
		//                            Einzelkupferzuschlag = angeboteneArtikelEntity.Einzelkupferzuschlag,
		//                            VKGesamtpreis = item.AktuelleLiefermenge * angeboteneArtikelEntity.VKEinzelpreis / angeboteneArtikelEntity.Preiseinheit,
		//                            Versandarten_Auswahl = this._data.Standardversand,
		//                            Versanddatum_Auswahl = this._data.Versandatum,
		//                            VKEinzelpreis = item.UnitPrice,
		//                            //
		//                            Versandinfo_von_CS = item.Versandinfo_von_CS,
		//                            Packstatus = item.Packstatus,
		//                            Gepackt_von = item.Gepackt_von,
		//                            Gepackt_Zeitpunkt = item.Gepackt_Zeitpunkt,
		//                            Packinfo_von_Lager = item.Packinfo_von_Lager,
		//                            //!Shipping
		//                            Versandstatus = item.Versandstatus,
		//                            Versanddienstleister = item.Versanddienstleister,
		//                            Versandnummer = item.Versandnummer.ToString(),
		//                            Versandinfo_von_Lager = item.Versandinfo_von_Lager,
		//                            EDI_PREIS_KUNDE = item.EDI_PREIS_KUNDE,
		//                            EDI_PREISEINHEIT = item.EDI_PREISEINHEIT,
		//                        }));
		//                    }
		//                    //}
		//                    //else
		//                    //{
		//                    // R5
		//                    //Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
		//                    //{
		//                    //    AngebotNr = insertedNr,
		//                    //    ArtikelNr = 223,
		//                    //    Bezeichnung1 = "Fracht+Verpackung",
		//                    //    Anzahl = 1,
		//                    //    OriginalAnzahl = 0,
		//                    //    ///// >>> get [PSZ_Auftrag LS 051 Filter für Versandkosten] QUERY
		//                    //    Einzelpreis = this._data.Versandkosten.HasValue? this._data.Versandkosten.Value:0 , //>>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0) 
		//                    //    Gesamtpreis = this._data.Versandkosten.HasValue ? this._data.Versandkosten.Value : 0, //>>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0)
		//                    //    Rabatt = 0,
		//                    //    USt = (kundenEntity.Umsatzsteuer_berechnen.HasValue && kundenEntity.Umsatzsteuer_berechnen.Value) ? 0.19m : 0,
		//                    //    Lagerbewegung = false,
		//                    //    Lagerbewegung_rückgängig = false,
		//                    //    Auswahl = false,
		//                    //    FM_Einzelpreis = 0,
		//                    //    FM_Gesamtpreis = 0,
		//                    //    Summenberechnung = false,
		//                    //    Preiseinheit = 1,
		//                    //    Preis_ausweisen = true,
		//                    //    Liefertermin = DateTime.Now,
		//                    //    erledigt_pos = false,
		//                    //    Stückliste = false,
		//                    //    Stückliste_drucken = false,
		//                    //    Langtext = "No",
		//                    //    Langtext_drucken = false,
		//                    //    Lagerort_id = 3,
		//                    //    Seriennummern_drucken = false,
		//                    //    Wunschtermin = new DateTime(2999, 12, 31),
		//                    //    Fertigungsnummer = 0,
		//                    //    Geliefert = 0,
		//                    //    Position = 999,
		//                    //    VKEinzelpreis = this._data.Versandkosten.HasValue ? this._data.Versandkosten.Value : 0, // >>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0) 
		//                    //    VKGesamtpreis = this._data.Versandkosten.HasValue ? this._data.Versandkosten.Value : 0, // >>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0)
		//                    //    Einzelkupferzuschlag = 0,
		//                    //    Gesamtkupferzuschlag = 0,

		//                    //    termin_eingehalten = item.termin_eingehalten,
		//                    //});
		//                    //}
		//                }

		//                // --------------- 2nd validate
		//                var deliveryNoteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(insertedNr);
		//                var angeboteArtikelEntitiesAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.Nr)?
		//                    .Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false).ToList();
		//                UpdateDeliveryNote(errors, deliveryNoteEntity, angeboteEntity, angeboteArtikelEntitiesAfterInsert, artikelEntities);

		//                //
		//                generateDATFile(errors, deliveryNoteEntity, angeboteArtikelEntities, artikelEntities);
		//            }

		//            return new ResponseModel<int>
		//            {
		//                Success = true,
		//                Body = insertedNr,
		//                Errors = errors.Select(x =>
		//                    new ResponseModel<int>.ResponseError
		//                    {
		//                        Key = x.Key,
		//                        Value = x.Value
		//                    }).ToList()
		//            };
		//        }
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//public ResponseModel<int> Validate()
		//{
		//    if (this._user == null)
		//    {
		//        return ResponseModel<int>.AccessDeniedResponse();
		//    }

		//    if (this._data.AngebotNr <= 0)
		//    {
		//        return new ResponseModel<int>()
		//        {
		//            Success = false,
		//            Errors = new List<ResponseModel<int>.ResponseError>
		//            {
		//               new ResponseModel<int>.ResponseError{Key ="", Value = $"Project Number [{this._data.AngebotNr}] invalid"}
		//            }
		//        };
		//    }

		//    var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.Nr);
		//    if (orderEntity == null)
		//    {
		//        return new ResponseModel<int>()
		//        {
		//            Success = false,
		//            Errors = new List<ResponseModel<int>.ResponseError>
		//            {
		//               new ResponseModel<int>.ResponseError{Key ="", Value = "Order Confirmation not found"}
		//            }
		//        };
		//    }

		//    var errors = new List<string> { };
		//    if (orderEntity.Typ.ToLower() != "auftragsbestätigung")
		//    {
		//        errors.Add($"Order: Type is not Auftragsbestätigung");
		//    }
		//    if (orderEntity.Gebucht == false)
		//    {
		//        errors.Add($"Order: Gebucht is false");
		//    }
		//    if (orderEntity.Erledigt == true)
		//    {
		//        errors.Add($"Order: Erledigt is true");
		//    }

		//    //--
		//    var angeboteArtikelEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.Nr)?
		//                    .Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false)?.ToList()
		//                    ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
		//    var XX = this._data.Items?
		//                .Select(x => x.ArtikelNummer)?
		//                .ToList();
		//    var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(
		//            this._data.Items?
		//                .Select(x => x.ArtikelNummer)?
		//                .ToList(), null);

		//    var articleNrs = artikelEntities?.Select(x => x.ArtikelNr)?.ToList();
		//    var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);

		//    //-

		//    foreach (var item in this._data.Items)
		//    {
		//        if (item.AktuelleLiefermenge < 0)
		//        {
		//            errors.Add($"Article {item.ArtikelNummer}: invalid quantity");
		//        }
		//        else
		//        {
		//            if (item.AktuelleLiefermenge > 0)
		//            {
		//                var angeboteArtikelItem = angeboteArtikelEntities.Find(x => x.Nr == item.Nr);
		//                if (angeboteArtikelItem != null && item.AktuelleLiefermenge > (decimal)(angeboteArtikelItem.Anzahl ?? 0))
		//                {
		//                    errors.Add($"Article {item.ArtikelNummer}: quantity greater than Order");
		//                }
		//                else
		//                {
		//                    var artikelItem = artikelEntities?.Find(x => x.ArtikelNummer == item.ArtikelNummer);
		//                    if (artikelItem != null)
		//                    {
		//                        var lagerItem = lagerEntities?.Find(x => x.Artikel_Nr == item.ArtikelNr && x.Lagerort_id == item.Lagerort_id);
		//                        if (lagerItem != null)
		//                        {
		//                            errors.AddRange(validateArticle(item, artikelItem, lagerItem)?.Select(x => x.Value)?.ToList() ?? new List<string>());
		//                        }
		//                    }
		//                }
		//            }
		//        }
		//    }


		//    if (errors.Count > 0)
		//    {
		//        return new ResponseModel<int>()
		//        {
		//            Success = false,
		//            Errors = errors.Select(x => new ResponseModel<int>.ResponseError { Key = "", Value = x }).Distinct().ToList()
		//        };
		//    }

		//    return ResponseModel<int>.SuccessResponse();
		//}

		//internal List<KeyValuePair<string, string>> validateArticle(
		//    Models.DeliveryNote.ValidateModel.Item item,
		//    Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
		//    Infrastructure.Data.Entities.Tables.PRS.LagerEntity lagerEntity)
		//{
		//    var errors = new List<KeyValuePair<string, string>>();

		//    // 1-, 2-, 3-
		//    if (artikelEntity.FreigabestatusTNIntern.ToLower() == "r" || artikelEntity.FreigabestatusTNIntern.ToLower() == "b")
		//    {
		//        errors.Add(new KeyValuePair<string, string>(
		//            "Artikel gesperrt",
		//           $"Lieferung ist derzeit nicht möglich! \n\r Artikel {artikelEntity.ArtikelNummer} ist gesperrt: Status-Intern auf {artikelEntity.FreigabestatusTNIntern} gesetzt!"
		//           ));
		//    }

		//    //4-
		//    if (artikelEntity.ArtikelNummer.ToLower() != "reparatur" && artikelEntity.Freigabestatus.ToLower() == "n")
		//    {
		//        errors.Add(new KeyValuePair<string, string>(
		//            "Erstmuster gesperrt",
		//           $"Lieferung ist derzeit nicht möglich!, Status-extern auf N gesetzt! \n\r Artikelnummer {artikelEntity.ArtikelNummer} ist gesperrt!"
		//           ));
		//    }

		//    //5-
		//    if (artikelEntity.ArtikelNummer.ToLower() != "reparatur" && artikelEntity.FreigabestatusTNIntern.ToLower() == "n")
		//    {
		//        errors.Add(new KeyValuePair<string, string>(
		//            "Status-Intern",
		//           $"Lieferung ist derzeit nicht möglich,\n\r Artikelnummer {artikelEntity.ArtikelNummer} ist gesperrt:Status-Inter auf N gesetzt!"
		//           ));
		//    }

		//    if (Convert.ToDecimal(lagerEntity.Bestand ?? 0) <= item.AktuelleLiefermenge)
		//    {
		//        errors.Add(new KeyValuePair<string, string>("Invalid quantity", $"Article {artikelEntity.ArtikelNummer}: lager[{lagerEntity.Lagerort_id}] quantity {lagerEntity.Bestand} < {item.AktuelleLiefermenge}"));
		//    }

		//    return errors;
		//}
		//internal void UpdateDeliveryNote(
		//    List<KeyValuePair<string, string>> errors,
		//    Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity deliveryNoteEntity,
		//    Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
		//    List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
		//    List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
		//{
		//    lock (Locks.DeliveryNotesLock)
		//    {
		//        // Update corrensponding order
		//        // 1.1-
		//        var maxNr = Convert.ToInt32(Handlers.Order.getNextAngebotNr(Enums.OrderEnums.Types.Delivery)); // Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxNr();
		//        deliveryNoteEntity.Angebot_Nr = maxNr + 1;

		//        //deliveryNoteEntity.Projekt_Nr = !string.IsNullOrEmpty(angeboteEntity.Projekt_Nr) && !string.IsNullOrWhiteSpace(angeboteEntity.Projekt_Nr)
		//        //    ? angeboteEntity.Projekt_Nr
		//        //    : maxNr + "1";

		//        deliveryNoteEntity.Benutzer = $"Gebucht, {this._user.Username}, {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}";

		//        //
		//        Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(deliveryNoteEntity);

		//        // 1.2 -
		//        var articleNrs = artikelEntities?.Select(x => x.ArtikelNr).ToList();
		//        var lagerEntites = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);
		//        foreach (var item in this._data.Items)
		//        {
		//            var angeboteArtikelItem = angeboteneArtikelEntities.Find(x => x.Nr == item.Nr);
		//            if (angeboteArtikelItem != null)
		//            {
		//                var lagerItem = lagerEntites.Find(x => x.Artikel_Nr == item.ArtikelNr && x.Lagerort_id == item.Lagerort_id);
		//                if (lagerItem != null)
		//                {
		//                    lagerItem.Bestand = lagerItem.Bestand - item.AktuelleLiefermenge;
		//                    lagerItem.letzte_Bewegung = DateTime.Now;
		//                    Infrastructure.Data.Access.Tables.PRS.LagerAccess.Update(lagerItem);
		//                }
		//            }
		//        }

		//        /*
		//         * // 1.3 set current DN positions
		//            UPDATE Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr] SET 
		//                [angebotene Artikel].VKGesamtpreis = IIf([angebotene Artikel]![VK-Festpreis]=Yes,[angebotene Artikel]!Anzahl*[angebotene Artikel]!Einzelpreis/[angebotene Artikel]!Preiseinheit,(([angebotene Artikel]!Einzelpreis/[angebotene Artikel]!Preiseinheit)-[angebotene Artikel]!Einzelkupferzuschlag)*[angebotene Artikel]!Anzahl), 
		//                [angebotene Artikel].Gesamtkupferzuschlag = [angebotene Artikel]!Anzahl*[angebotene Artikel]!Einzelkupferzuschlag, 
		//                [angebotene Artikel].Gesamtpreis = [angebotene Artikel]!Anzahl*[angebotene Artikel]!Einzelpreis/[angebotene Artikel]!Preiseinheit*(1-[angebotene Artikel]!Rabatt), 
		//                [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel]!Anzahl*[angebotene Artikel]![EinzelCu-Gewicht], 
		//                [angebotene Artikel].VKEinzelpreis = IIf([angebotene Artikel]![VK-Festpreis]=Yes,[angebotene Artikel]!Einzelpreis,(([angebotene Artikel]!Einzelpreis/[angebotene Artikel]!Preiseinheit)-[angebotene Artikel]!Einzelkupferzuschlag)*[angebotene Artikel]!Preiseinheit)
		//            WHERE (((Angebote.Nr)=[Formulare]![Angebote]![Nr]) AND (([angebotene Artikel].erledigt_pos)=No) AND (([angebotene Artikel].[Artikel-Nr])<>223));

		//         * */

		//        var angeboteneArtikelEntities_not_223 = angeboteneArtikelEntities.Where(x => x.ArtikelNr.HasValue && x.ArtikelNr != 223).ToList();
		//        foreach (var angeboteneArtikelEntity in angeboteneArtikelEntities_not_223)
		//        {
		//            if (!angeboteneArtikelEntity.Preiseinheit.HasValue || angeboteneArtikelEntity.Preiseinheit.Value == 0)
		//            {
		//                errors.Add(new KeyValuePair<string, string>("", $"{angeboteneArtikelEntity.Position}. Preiseinheit: invalid value {angeboteneArtikelEntity.Preiseinheit.Value}"));
		//                continue;
		//            }
		//            angeboteneArtikelEntity.VKGesamtpreis = angeboteneArtikelEntity.VKFestpreis.HasValue && angeboteneArtikelEntity.VKFestpreis.Value
		//                ? angeboteneArtikelEntity.Anzahl * angeboteneArtikelEntity.Einzelpreis / angeboteneArtikelEntity.Preiseinheit
		//                : ((angeboteneArtikelEntity.Einzelpreis / angeboteneArtikelEntity.Preiseinheit) - angeboteneArtikelEntity.Einzelkupferzuschlag) * angeboteneArtikelEntity.Anzahl;

		//            angeboteneArtikelEntity.Gesamtkupferzuschlag = angeboteneArtikelEntity.Anzahl * angeboteneArtikelEntity.Einzelkupferzuschlag;

		//            angeboteneArtikelEntity.Gesamtpreis = angeboteneArtikelEntity.Anzahl * angeboteneArtikelEntity.Einzelpreis / angeboteneArtikelEntity.Preiseinheit * (1 - angeboteneArtikelEntity.Rabatt);

		//            angeboteneArtikelEntity.GesamtCuGewicht = angeboteneArtikelEntity.Anzahl * angeboteneArtikelEntity.EinzelCuGewicht;
		//            angeboteneArtikelEntity.VKEinzelpreis = angeboteneArtikelEntity.VKFestpreis.HasValue && angeboteneArtikelEntity.VKFestpreis.Value
		//                ? angeboteneArtikelEntity.Einzelpreis
		//                : ((angeboteneArtikelEntity.Einzelpreis / angeboteneArtikelEntity.Preiseinheit) - angeboteneArtikelEntity.Einzelkupferzuschlag) * angeboteneArtikelEntity.Preiseinheit;

		//            Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(angeboteneArtikelEntity);
		//        }

		//        /*
		//        //
		//        SELECT 
		//         [angebotene Artikel].[LS Pos zu AB Pos], 
		//         [angebotene Artikel].[Angebot-Nr], 
		//         [angebotene Artikel].[Artikel-Nr], 
		//         [angebotene Artikel].Bezeichnung1, 
		//         [angebotene Artikel].Anzahl

		//        INTO [PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]
		//        FROM [angebotene Artikel]
		//        WHERE ((([angebotene Artikel].[LS Pos zu AB Pos])<>0) AND (([angebotene Artikel].[Angebot-Nr])=[Formulare]![Angebote]![Nr]));


		//        // 1.4 set current DN positions
		//        UPDATE [angebotene Artikel], [PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen] SET 
		//         [angebotene Artikel].Anzahl = [angebotene Artikel]!Anzahl-[PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]!Anzahl, 
		//         [angebotene Artikel].Geliefert = [angebotene Artikel]!Geliefert+[PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]!Anzahl, 
		//         [angebotene Artikel].Gesamtpreis = ([angebotene Artikel]!Anzahl-[PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]!Anzahl)/[angebotene Artikel]!Preiseinheit*[angebotene Artikel]!Einzelpreis*(1-[angebotene Artikel]!Rabatt), 
		//         [angebotene Artikel].erledigt_pos = IIf(([angebotene Artikel]!Anzahl-[PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]!Anzahl)>0,No,Yes)
		//        WHERE ((([angebotene Artikel].Nr)=[PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]![LS Pos zu AB Pos]));


		//        // 1.5 
		//        UPDATE [angebotene Artikel], [PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen] SET 
		//         [angebotene Artikel].Einzelkupferzuschlag = Round((([angebotene Artikel]!DEL*1.01)-[angebotene Artikel]!Kupferbasis)/100*[angebotene Artikel]![EinzelCu-Gewicht],2)
		//        WHERE ((([angebotene Artikel].Nr)=[PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]![LS Pos zu AB Pos]));


		//        // 1.6 -
		//        UPDATE [angebotene Artikel], [PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen] SET 
		//         [angebotene Artikel].[GesamtCu-Gewicht] = [angebotene Artikel]!Anzahl*[angebotene Artikel]![EinzelCu-Gewicht], 
		//         [angebotene Artikel].Einzelpreis = IIf([angebotene Artikel]![VK-Festpreis]=Yes,[angebotene Artikel]!VKEinzelpreis,[angebotene Artikel]!Einzelkupferzuschlag*[angebotene Artikel]!Preiseinheit+[angebotene Artikel]!VKEinzelpreis)
		//        WHERE ((([angebotene Artikel].Nr)=[PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]![LS Pos zu AB Pos]));

		//        // 1.7- 
		//        UPDATE [angebotene Artikel], [PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen] SET 
		//         [angebotene Artikel].Gesamtpreis = [angebotene Artikel]!Einzelpreis/[angebotene Artikel]!Preiseinheit*[angebotene Artikel]!Anzahl*(1-[angebotene Artikel]!Rabatt), 
		//         [angebotene Artikel].Gesamtkupferzuschlag = IIf([angebotene Artikel]![VK-Festpreis]=Yes,0,[angebotene Artikel]!Anzahl*[angebotene Artikel]!Einzelkupferzuschlag), 
		//         [angebotene Artikel].VKGesamtpreis = [angebotene Artikel]!Anzahl*[angebotene Artikel]!VKEinzelpreis/[angebotene Artikel]!Preiseinheit
		//        WHERE ((([angebotene Artikel].Nr)=[PSZ_Auftrag Druck 026 LS Hilfstabelle erstellen]![LS Pos zu AB Pos]));

		//         */


		//        //FIXME: CONFIRM !!! >>>>>
		//        var angeboteneArtikelEntities_zu_ABPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(deliveryNoteEntity.Nr); // - angeboteneArtikelEntities.Where(x => x.AngebotNr == angeboteEntity.Nr).ToList();
		//        var angeboteArtikelEntities_AB = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(angeboteEntity.Nr);
		//        foreach (var angeboteneArtikelEntity in angeboteArtikelEntities_AB)
		//        {
		//            var _angeboteneArtikelEntity = angeboteneArtikelEntities_zu_ABPos.Where(x => x.LSPoszuABPos == angeboteneArtikelEntity.Nr)?.ToList().FirstOrDefault();
		//            if (!angeboteneArtikelEntity.Preiseinheit.HasValue || angeboteneArtikelEntity.Preiseinheit.Value == 0)
		//            {
		//                errors.Add(new KeyValuePair<string, string>("", $"{angeboteneArtikelEntity.Position}. Preiseinheit: invalid value {angeboteneArtikelEntity.Preiseinheit.Value}"));
		//                continue;
		//            }
		//            if (_angeboteneArtikelEntity != null)
		//            {
		//                // 1.4
		//                angeboteneArtikelEntity.Anzahl = angeboteneArtikelEntity.Anzahl - _angeboteneArtikelEntity.Anzahl;
		//                angeboteneArtikelEntity.Geliefert = angeboteneArtikelEntity.Geliefert + _angeboteneArtikelEntity.Anzahl;
		//                angeboteneArtikelEntity.Gesamtpreis = (angeboteneArtikelEntity.Anzahl - _angeboteneArtikelEntity.Anzahl) / angeboteneArtikelEntity.Preiseinheit * angeboteneArtikelEntity.Einzelpreis * (1 - angeboteneArtikelEntity.Rabatt);
		//                angeboteneArtikelEntity.erledigt_pos = angeboteneArtikelEntity.Anzahl - _angeboteneArtikelEntity.Anzahl > 0 ? false : true;

		//                // 1.5
		//                angeboteneArtikelEntity.Einzelkupferzuschlag = Math.Round((decimal)(((angeboteneArtikelEntity.DEL * 1.01m) - angeboteneArtikelEntity.Kupferbasis)
		//                                                                          / 100
		//                                                                          * (decimal?)angeboteneArtikelEntity.EinzelCuGewicht), 2);

		//                // 1.6 
		//                angeboteneArtikelEntity.GesamtCuGewicht = angeboteneArtikelEntity.Anzahl * angeboteneArtikelEntity.EinzelCuGewicht;
		//                angeboteneArtikelEntity.Einzelpreis = angeboteneArtikelEntity.VKFestpreis.HasValue && angeboteneArtikelEntity.VKFestpreis.Value == true
		//                    ? angeboteneArtikelEntity.VKEinzelpreis
		//                    : angeboteneArtikelEntity.Einzelkupferzuschlag * angeboteneArtikelEntity.Preiseinheit + angeboteneArtikelEntity.VKEinzelpreis;

		//                // 1.7
		//                angeboteneArtikelEntity.Gesamtpreis = angeboteneArtikelEntity.Einzelpreis / angeboteneArtikelEntity.Preiseinheit * angeboteneArtikelEntity.Anzahl * (1 - angeboteneArtikelEntity.Rabatt);
		//                angeboteneArtikelEntity.Gesamtkupferzuschlag = angeboteneArtikelEntity.VKFestpreis.HasValue && angeboteneArtikelEntity.VKFestpreis.Value == true
		//                    ? 0
		//                    : angeboteneArtikelEntity.Anzahl * angeboteneArtikelEntity.Einzelkupferzuschlag;
		//                angeboteneArtikelEntity.VKGesamtpreis = angeboteneArtikelEntity.Anzahl * angeboteneArtikelEntity.VKEinzelpreis / angeboteneArtikelEntity.Preiseinheit;

		//                Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(angeboteneArtikelEntity);
		//            }
		//        }

		//        /*
		//        // 1.8 - 
		//        UPDATE Angebote SET 
		//         Angebote.gebucht = Yes
		//        WHERE (((Angebote.Nr)=[Formulare]![Angebote]![Nr]));
		//        */
		//        deliveryNoteEntity.Gebucht = true;
		//        Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(deliveryNoteEntity);
		//    }
		//}

		//internal void generateDATFile(
		//    List<KeyValuePair<string, string>> errors,
		//    Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
		//    List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
		//    List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
		//{
		//    if (!angeboteEntity.Kunden_Nr.HasValue)
		//    {
		//        errors.Add(new KeyValuePair<string, string>("", "Customer not found"));
		//        return;
		//    }

		//    var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(angeboteEntity.Kunden_Nr.Value);
		//    var WmsAngebotNr = angeboteEntity.Angebot_Nr;
		//    var path = $@"{Infrastructure.Services.Files.DN.DeliveryNoteFilesPath}\WA{DateTime.Now.ToString("yyyyMMddhhmmss")}.dat";
		//    var content = $"AG;1;1;{WmsAngebotNr };{angeboteneArtikelEntities?.Count};50;{angeboteEntity.Datum.Value.ToString("yyyyMMdd")};{angeboteEntity.Versanddatum_Auswahl?.ToString("yyyyMMdd")};1;0;0;1;1;{addressenEntity.Kundennummer.Value};{angeboteEntity.Vorname_NameFirma.Substring(0, Math.Min(angeboteEntity.Vorname_NameFirma.Length, 37))}";
		//    foreach (var angeboteneArtikelEntity in angeboteneArtikelEntities)
		//    {
		//        var artikelEntity = artikelEntities.Where(x => x.ArtikelNr == angeboteneArtikelEntity.ArtikelNr)?.ToList().FirstOrDefault();

		//        if (artikelEntity != null && (artikelEntity.Warengruppe.ToUpper() == "EF" || artikelEntity.Warengruppe.ToUpper() == "ROH"))
		//        {
		//            content += $"\nAG;2;1;{WmsAngebotNr};{ angeboteneArtikelEntity.Position};{artikelEntity.ArtikelNummer};{angeboteneArtikelEntity.Anzahl}";
		//        }
		//    }

		//    File.WriteAllText(path, content);
		//}
	}
}
