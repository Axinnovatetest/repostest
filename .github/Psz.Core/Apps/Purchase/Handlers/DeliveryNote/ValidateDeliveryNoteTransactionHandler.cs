using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class ValidateDeliveryNoteTransactionHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.DeliveryNote.ValidateDeliveryNoteModel _data { get; set; }
		public ValidateDeliveryNoteTransactionHandler(Identity.Models.UserModel user, Models.DeliveryNote.ValidateDeliveryNoteModel model)
		{
			_user = user;
			_data = model;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			lock(Locks.DeliveryNotesLock)
			{
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				var errors = new List<KeyValuePair<string, string>>();
				var block = Psz.Core.Common.Helpers.blockHelper.GetBlockState();
				if(block.LS)
					return ResponseModel<int>.FailureResponse("Another Delivery note is in creation, please try again in a moment .");
				if(block.RG)
					return ResponseModel<int>.FailureResponse("Another Delivery note, please try again in a moment .");

				//var MaxCurrentValue = Program.CTS.lsMaxCurrentValue;
				//var MinNewValue = Program.CTS.lsMinNewValue;

				var maxAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndPrefix(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY, (int)Core.Common.Enums.INSEnums.INSOrderTypesAngebotNrPrefix.LS);
				//Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndSettingsValues(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY, MaxCurrentValue, MinNewValue);
				var checkAngebotNrExist = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetByAngebotNr(maxAngebotNr);
				if(checkAngebotNrExist != null && checkAngebotNrExist.Count > 0)
				{
					return ResponseModel<int>.FailureResponse("Another Delivery note is in creation, please try again in a moment .");
				}
				Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.Insert(new Infrastructure.Data.Entities.Tables.CRP.__crp_FertigungsnummerEntity
				{
					angebotNr = maxAngebotNr,
					User = $"{_user.Username}-{_user.Id}",
				});
				try
				{
					var angeboteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.Nr);
					var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(Convert.ToInt32(angeboteEntity.Kunden_Nr ?? 0));

					//opening sql transaction
					botransaction.beginTransaction();
					Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.LS, true);
					// 0.1 - 
					var angeboteTermin = this._data.Items.All(x => x.termin_eingehalten == true); // >>>> ?Ridha
					angeboteEntity.Termin_eingehalten = angeboteTermin; // >>>> § 0.1 -
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(angeboteEntity, botransaction.connection, botransaction.transaction);

					// >>>>>> Logging
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Debug, $" OrderImport[DeliveryNote Validate] >>>>>> insert orderDb ");


					var insertedNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity
					{
						Projekt_Nr = angeboteEntity.Projekt_Nr,
						Angebot_Nr = maxAngebotNr,
						Typ = "Lieferschein",
						Datum = DateTime.Now,
						Liefertermin = angeboteEntity.Liefertermin,
						Kunden_Nr = angeboteEntity.Kunden_Nr,
						Debitorennummer = angeboteEntity.Debitorennummer,
						Falligkeit = DateTime.Now.AddDays(30),
						Anrede = angeboteEntity.Anrede,
						Vorname_NameFirma = angeboteEntity.Vorname_NameFirma,
						Name2 = angeboteEntity.Name2,
						Name3 = angeboteEntity.Name3,
						Ansprechpartner = angeboteEntity.Ansprechpartner,
						Abteilung = angeboteEntity.Abteilung,
						Straße_Postfach = angeboteEntity.Straße_Postfach,
						Land_PLZ_Ort = angeboteEntity.Land_PLZ_Ort,
						Briefanrede = angeboteEntity.Briefanrede,
						LAnrede = angeboteEntity.LAnrede,
						LVorname_NameFirma = angeboteEntity.LVorname_NameFirma,
						LName2 = angeboteEntity.LName2,
						LName3 = angeboteEntity.LName3,
						LAnsprechpartner = angeboteEntity.LAnsprechpartner,
						LAbteilung = angeboteEntity.LAbteilung,
						LStraße_Postfach = angeboteEntity.LStraße_Postfach,
						LLand_PLZ_Ort = angeboteEntity.LLand_PLZ_Ort,
						LBriefanrede = angeboteEntity.LBriefanrede,
						Personal_Nr = angeboteEntity.Personal_Nr,
						Versandart = angeboteEntity.Versandart,
						Zahlungsweise = angeboteEntity.Zahlungsweise,
						Konditionen = angeboteEntity.Konditionen,
						Zahlungsziel = angeboteEntity.Zahlungsziel,
						USt_Berechnen = angeboteEntity.USt_Berechnen,
						Bezug = angeboteEntity.Bezug,
						Ihr_Zeichen = angeboteEntity.Ihr_Zeichen,
						Unser_Zeichen = angeboteEntity.Unser_Zeichen,
						Freitext = angeboteEntity.Freitext,
						Gebucht = false,
						Gedruckt = false,
						Erledigt = false,
						Auswahl = angeboteEntity.Auswahl,
						Mahnung = angeboteEntity.Mahnung,
						Lieferadresse = angeboteEntity.Lieferadresse,
						Reparatur_nr = angeboteEntity.Reparatur_nr,
						Interessent = angeboteEntity.Interessent,
						Nr_auf = angeboteEntity.Nr,
						Ab_id = angeboteEntity.Nr,
						Status = angeboteEntity.Status,
						Bemerkung = angeboteEntity.Bemerkung,
						Bereich = angeboteEntity.Bereich,
						Belegkreis = angeboteEntity.Belegkreis,
						Wunschtermin = angeboteEntity.Wunschtermin,
						Datueber = angeboteEntity.Datueber,
						Mandant = angeboteEntity.Mandant,
						Termin_eingehalten = angeboteTermin,
						Nr_BV = 0,
						Nr_RA = 0,
						Nr_Kanban = 0,
						Nr_ang = 0,
						Nr_lie = 0,
						Nr_rec = 0,
						Nr_pro = 0,
						Nr_gut = 0,
						Nr_sto = 0,
						Neu = 1,
						Loschen = false,
						In_Bearbeitung = false,
						Offnen = false,
						Versanddatum_Auswahl = this._data.Versandatum,
						Versandarten_Auswahl = this._data.Standardversand,
						LsDeliveryDate = this._data.DeliveryDate,
						LsAddressNr = angeboteEntity.LsAddressNr,
						StorageLocation = angeboteEntity.StorageLocation,
						UnloadingPoint = angeboteEntity.UnloadingPoint

					}, /*MaxCurrentValue, MinNewValue, "Lieferschein",*/ botransaction.connection, botransaction.transaction);
					if(this._data.VersandBerechnen.HasValue && this._data.VersandBerechnen.Value)
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
						{
							AngebotNr = insertedNr,
							ArtikelNr = 223,
							Bezeichnung1 = "Fracht+Verpackung",
							Anzahl = 1,
							OriginalAnzahl = 0,
							///// >>> get [PSZ_Auftrag LS 051 Filter für Versandkosten] QUERY
							Einzelpreis = this._data.VersandKosten.HasValue ? this._data.VersandKosten.Value : 0, //>>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0) 
							Gesamtpreis = this._data.VersandKosten.HasValue ? this._data.VersandKosten.Value : 0, //>>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0)
							Rabatt = 0,
							USt = (kundenEntity.Umsatzsteuer_berechnen.HasValue && kundenEntity.Umsatzsteuer_berechnen.Value) ? 0.19m : 0,
							Lagerbewegung = false,
							Lagerbewegung_rückgängig = false,
							Auswahl = false,
							FM_Einzelpreis = 0,
							FM_Gesamtpreis = 0,
							Summenberechnung = false,
							Preiseinheit = 1,
							Liefertermin = DateTime.Now,
							erledigt_pos = false,
							Stückliste = false,
							Stückliste_drucken = false,
							Langtext = "No",//to check with Ridha
							Langtext_drucken = false,
							Lagerort_id = 3,
							Seriennummern_drucken = false,
							Wunschtermin = null,
							Fertigungsnummer = 0,
							Geliefert = 0,
							Position = 999,
							VKEinzelpreis = this._data.VersandKosten.HasValue ? this._data.VersandKosten.Value : 0, // >>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0) 
							VKGesamtpreis = this._data.VersandKosten.HasValue ? this._data.VersandKosten.Value : 0, // >>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0)
							Einzelkupferzuschlag = 0,
							Gesamtkupferzuschlag = 0,
							termin_eingehalten = false,
							//cheched
							AnfangLagerBestand = 0m,
							AktuelleAnzahl = 0m,
							EndeLagerBestand = 0m,
							Preis_ausweisen = true,
							LSPoszuKBPos = 0,
							LSPoszuABPos = 0,
							RAPoszuBVPos = 0,
							KBPoszuBVPos = 0,
							ABPoszuBVPos = 0,
							KBPoszuRAPos = 0,
							ABPoszuRAPos = 0,
							VKFestpreis = true,
							Kupferbasis = 0,
							DEL = 0,
							EinzelCuGewicht = 0m,
							GesamtCuGewicht = 0m,
							DELFixiert = false,
							Loschen = false,
							InBearbeitung = false,
							RA_OriginalAnzahl = 1m,
							RA_Abgerufen = 1m,
							RA_Offen = 1m,
							Packstatus = false,
							Versandstatus = false,
							Versand_gedruckt = false,
							LS_von_Versand_gedruckt = false,
							VDA_gedruckt = false,
							Typ = -1,
							Index_Kunde = "", // - Empty b/c Index is required
							Index_Kunde_Datum = null,
							Versandarten_Auswahl = this._data.Standardversand,
							Versanddatum_Auswahl = this._data.Versandatum,
						}, botransaction.connection, botransaction.transaction);
					}
					if(this._data.Items != null && this._data.Items.Count > 0)
					{
						var itemsIds = this._data.Items.Where(x => x.Id != -1).Select(y => y.Id).ToList();
						var angeboteArtikelEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(itemsIds)?
							.Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false).ToList();


						var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(
								this._data.Items?
									.Select(x => x.ItemNumber)?
									.ToList(), null);

						var articleNrs = artikelEntities?.Select(x => x.ArtikelNr)?.ToList();

						var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);

						var itemsDone = this._data.Items.FindAll(x => x.AktuelleLiefermenge > 0);
						List<int> InsertedIds = new List<int>();
						foreach(var item in itemsDone)
						{
							var artikel = artikelEntities.FirstOrDefault(x => x.ArtikelNummer == item.ItemNumber);
							if(item.Id != -1)
							{
								var angeboteneArtikelEntity = angeboteArtikelEntities?.FirstOrDefault(x => x.ArtikelNr == artikel.ArtikelNr && x.Position == item.Position);

								angeboteneArtikelEntity.termin_eingehalten = item.termin_eingehalten;
								Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(angeboteneArtikelEntity, botransaction.connection, botransaction.transaction);
								Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditWithTransaction(artikel, botransaction.connection, botransaction.transaction);
								/////////*********************
							}
							var itemArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(item.ItemNumber);
							var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemArtikelEntity?.ArtikelNr ?? -1);
							if(item.AktuelleLiefermenge > 0)
							{
								var Bez1 = (string.IsNullOrEmpty(item.Designation1) || string.IsNullOrWhiteSpace(item.Designation1)) ? null :
									(item.Designation1.Length >= 200) ? item.Designation1.Substring(0, 200) : item.Designation1;
								var Bez3 = (string.IsNullOrEmpty(artikel.Bezeichnung3) || string.IsNullOrWhiteSpace(artikel.Bezeichnung3)) ? null :
									(artikel.Bezeichnung3.Length >= 200) ? artikel.Bezeichnung3.Substring(0, 200) : artikel.Bezeichnung3;
								InsertedIds.Add(Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
								{
									AngebotNr = insertedNr,
									ArtikelNr = artikel.ArtikelNr,
									Bezeichnung1 = Bez1,//column type in DB is nvarchar(200),
									Bezeichnung2 = item.Designation2,
									Bezeichnung3 = Bez3, //column type in DB is nvarchar(200),
									Einheit = artikel.Einheit,
									Anzahl = item.AktuelleLiefermenge,
									OriginalAnzahl = item.OriginalOrderQuantity,
									Preisgruppe = itemPricingGroupDb?.Preisgruppe,
									//Bestellnummer = angeboteneArtikelEntity.Bestellnummer,
									Rabatt = item.Discount,
									USt = item.VAT.HasValue ? item.VAT.Value / 100 : 0m,
									POSTEXT = item.Postext,
									Preiseinheit = item.UnitPriceBasis == 0 ? 1 : item.UnitPriceBasis, // - 2022-05-30 - init to 1 to respect DB Constraint
									Zeichnungsnummer = item.DrawingIndex,
									Liefertermin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
									//DateTime.Now,
									//item.DeliveryDate ?? DateTime.Now,
									erledigt_pos = false,//item.Done,
									Lagerort_id = item.StorageLocationId,
									Wunschtermin = item.DesiredDate,
									Fertigungsnummer = item.ProductionNumber,
									Geliefert = 0m,
									LSPoszuABPos = item.Id,
									Position = item.Position,
									VKFestpreis = item.FixedTotalPrice,
									EKPreise_Fix = item.FixedUnitPrice,//to check with Sani
																	   //
									Einzelpreis = item.OpenQuantity_UnitPrice,
									Gesamtpreis = item.OpenQuantity_TotalPrice,
									//
									DELFixiert = item.DelFixed,
									Abladestelle = item.UnloadingPoint,
									termin_eingehalten = item.termin_eingehalten,
									RP = item.RP,
									// R4
									Kupferbasis = int.TryParse(item.CopperBase.ToString(), out var val) ? val : 0,
									DEL = int.TryParse(item.DelNote.ToString(), out var val2) ? val2 : 0,
									EinzelCuGewicht = item.CopperWeight,
									GesamtCuGewicht = item.OpenQuantity_CopperWeight,
									Einzelkupferzuschlag = item.CopperSurcharge,
									VKGesamtpreis = item.TotalPrice,
									Versandarten_Auswahl = this._data.Standardversand,
									Versanddatum_Auswahl = this._data.Versandatum ?? null,
									VKEinzelpreis = item.UnitPrice,
									//
									Versandinfo_von_CS = item.Versandinfo_von_CS,
									Packstatus = item.Packstatus,
									Gepackt_von = item.Gepackt_von,
									Gepackt_Zeitpunkt = item.Gepackt_Zeitpunkt,
									Packinfo_von_Lager = item.Packinfo_von_Lager,
									//!Shipping
									Versandstatus = item.Versandstatus,
									Versanddienstleister = item.Versanddienstleister,
									Versandnummer = item.Versandnummer,
									Versandinfo_von_Lager = item.Versandinfo_von_Lager,
									EDI_PREIS_KUNDE = item.EDI_PREIS_KUNDE,
									EDI_PREISEINHEIT = item.EDI_PREISEINHEIT,
									AnfangLagerBestand = 0,
									AktuelleAnzahl = 0,
									Lagerbewegung = false,
									Lagerbewegung_rückgängig = false,
									Auswahl = false,
									FM_Einzelpreis = 0,
									FM_Gesamtpreis = 0,
									Summenberechnung = false,
									EndeLagerBestand = 0m,
									Preis_ausweisen = true,
									Stückliste = false,
									Stückliste_drucken = false,
									Langtext_drucken = false,
									Seriennummern_drucken = false,
									LSPoszuKBPos = 0,
									RAPoszuBVPos = 0,
									KBPoszuBVPos = 0,
									ABPoszuBVPos = 0,
									KBPoszuRAPos = 0,
									ABPoszuRAPos = 0,
									Loschen = false,
									InBearbeitung = false,
									RA_OriginalAnzahl = 1m,
									RA_Abgerufen = 1m,
									RA_Offen = 1m,
									Versand_gedruckt = false,
									LS_von_Versand_gedruckt = false,
									VDA_gedruckt = false,
									Typ = item.ItemTypeId,
									Bemerkungsfeld1 = item.Note1,
									Bemerkungsfeld2 = item.Note2,
									Freies_Format_EDI = item.FreeText,
									Index_Kunde = item.Index_Kunde,
									Index_Kunde_Datum = item.Index_Kunde_Datum,
									CSInterneBemerkung = item.CSInterneBemerkung,
								}, botransaction.connection, botransaction.transaction));

							}

						}

						// --------------- 2nd validate
						var deliveryNoteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(insertedNr, botransaction.connection, botransaction.transaction);
						var angeboteArtikelEntitiesAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(InsertedIds, botransaction.connection, botransaction.transaction)?
							.Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false).ToList();

						//
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(deliveryNoteEntity, botransaction.connection, botransaction.transaction);

						// 1.2 -
						var lagerEntites = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);
						var lagerVorgExtenionEntities = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndex(angeboteArtikelEntitiesAfterInsert?.Select(x => new KeyValuePair<int, string>(x.ArtikelNr ?? -1, x.Index_Kunde))?.ToList());
						var toUpdateLagers = new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
						var toUpdateLagersExt = new List<Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtensionModel>();

						foreach(var item in angeboteArtikelEntitiesAfterInsert)
						{
							var lagerItem = lagerEntites.Find(x => x.Artikel_Nr == item.ArtikelNr && x.Lagerort_id == item.Lagerort_id);
							if(lagerItem != null)
							{
								lagerItem.Bestand -= item.Anzahl;
								lagerItem.letzte_Bewegung = DateTime.Now;
								toUpdateLagers.Add(lagerItem);
							}                            // -

							toUpdateLagersExt.Add(new Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtensionModel
							{
								ArticleId = item.ArtikelNr ?? -1,
								OldBestand = 0m,
								NewBestand = item.Anzahl ?? 0,
								OldKundenIndex = item.Index_Kunde,
								NewKundenIndex = item.Index_Kunde,
								OldLagerorId = item.Lagerort_id ?? -1,
								NewLagerorId = item.Lagerort_id ?? -1,
							});
						}

						// - refactor
						if(toUpdateLagers.Count > 0)
						{
							Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateWithTransaction(toUpdateLagers, botransaction.connection, botransaction.transaction);
						}

						// - 2022-03-11 track KundenIndex for Lager Bestand
						if(toUpdateLagersExt.Count > 0)
						{
							Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtension(this._user, toUpdateLagersExt, botransaction);
						}

						var LSItems_not_223 = angeboteArtikelEntitiesAfterInsert.Where(x => x.ArtikelNr.HasValue && x.ArtikelNr != 223).ToList();
						foreach(var lsitem in LSItems_not_223)
						{
							if(!lsitem.Preiseinheit.HasValue || lsitem.Preiseinheit.Value == 0)
							{
								errors.Add(new KeyValuePair<string, string>("", $"{lsitem.Position}. Preiseinheit: invalid value {lsitem.Preiseinheit.Value}"));
								continue;
							}
							lsitem.VKGesamtpreis = lsitem.VKFestpreis.HasValue && lsitem.VKFestpreis.Value
								? lsitem.Anzahl * lsitem.Einzelpreis / lsitem.Preiseinheit
								: ((lsitem.Einzelpreis / lsitem.Preiseinheit) - lsitem.Einzelkupferzuschlag) * lsitem.Anzahl;

							lsitem.Gesamtkupferzuschlag = lsitem.Anzahl * lsitem.Einzelkupferzuschlag;

							lsitem.Gesamtpreis = lsitem.Anzahl * lsitem.Einzelpreis / lsitem.Preiseinheit * (1 - lsitem.Rabatt);

							lsitem.GesamtCuGewicht = lsitem.Anzahl * lsitem.EinzelCuGewicht;
							lsitem.VKEinzelpreis = lsitem.VKFestpreis.HasValue && lsitem.VKFestpreis.Value
								? lsitem.Einzelpreis
								: ((lsitem.Einzelpreis / lsitem.Preiseinheit) - lsitem.Einzelkupferzuschlag) * lsitem.Preiseinheit;

							Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(lsitem, botransaction.connection, botransaction.transaction);
						}
						//FIXME: CONFIRM !!! >>>>>
						var LSItems_zu_ABPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNrWithTransaction(deliveryNoteEntity.Nr, botransaction.connection, botransaction.transaction); // - angeboteneArtikelEntities.Where(x => x.AngebotNr == angeboteEntity.Nr).ToList();
						var ABPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNrWithTransaction(angeboteEntity.Nr, botransaction.connection, botransaction.transaction);
						foreach(var abpos in ABPositions)
						{
							var _lsItem = LSItems_zu_ABPos.Where(x => x.LSPoszuABPos == abpos.Nr)?.ToList().FirstOrDefault();
							if(!abpos.Preiseinheit.HasValue || abpos.Preiseinheit.Value == 0)
							{
								errors.Add(new KeyValuePair<string, string>("", $"{abpos.Position}. Preiseinheit: invalid value {abpos.Preiseinheit.Value}"));
								continue;
							}
							if(_lsItem != null)
							{
								// 1.4
								if(abpos.erledigt_pos.HasValue && abpos.erledigt_pos.Value)
									abpos.erledigt_pos = false;
								abpos.Anzahl = abpos.Anzahl - _lsItem.Anzahl;
								abpos.Geliefert = abpos.Geliefert + _lsItem.Anzahl;
								abpos.Gesamtpreis = (abpos.Anzahl - _lsItem.Anzahl) / abpos.Preiseinheit * abpos.Einzelpreis * (1 - abpos.Rabatt);
								abpos.erledigt_pos = abpos.Anzahl > 0 ? false : true;

								// 1.5
								abpos.Einzelkupferzuschlag = Math.Round((decimal)(((abpos.DEL * 1.01m) - (abpos.Kupferbasis ?? 0))
																						  / 100
																						  * (decimal?)(abpos.EinzelCuGewicht ?? 0)), 2);

								// 1.6 
								abpos.GesamtCuGewicht = abpos.Anzahl * abpos.EinzelCuGewicht;
								abpos.Einzelpreis = abpos.VKFestpreis.HasValue && abpos.VKFestpreis.Value == true
									? abpos.VKEinzelpreis
									: abpos.Einzelkupferzuschlag * abpos.Preiseinheit + abpos.VKEinzelpreis;

								// 1.7
								abpos.Gesamtpreis = abpos.Einzelpreis / abpos.Preiseinheit * abpos.Anzahl * (1 - abpos.Rabatt);
								abpos.Gesamtkupferzuschlag = abpos.VKFestpreis.HasValue && abpos.VKFestpreis.Value == true
									? 0
									: abpos.Anzahl * abpos.Einzelkupferzuschlag;
								abpos.VKGesamtpreis = abpos.Anzahl * abpos.VKEinzelpreis / abpos.Preiseinheit;

								Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(abpos, botransaction.connection, botransaction.transaction);
							}
						}
						var ABItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNrWithTransaction(angeboteEntity.Nr, botransaction.connection, botransaction.transaction);
						var GlobalErldigtAB = ABItems.All(x => x.erledigt_pos.HasValue && x.erledigt_pos.Value);
						//
						var LSItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNrWithTransaction(deliveryNoteEntity.Nr, botransaction.connection, botransaction.transaction);
						var GlobalErldigtLS = LSItems.All(x => x.erledigt_pos.HasValue && x.erledigt_pos.Value);

						//FIXME: Remove Erledigt for LS, as it will be set from P3000 on Rechnungsdruck - Schremmer / 2022-03-16
						// - 2025-09-03 - set AB to done, if all pos are delivered - but only for AB not from EDI
						angeboteEntity.Erledigt = GlobalErldigtAB && (angeboteEntity.EDI_Dateiname_CSV ?? "").Length == 0;
						//deliveryNoteEntity.Erledigt = GlobalErldigtLS;
						deliveryNoteEntity.Gebucht = true;
						//
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(angeboteEntity, botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(deliveryNoteEntity, botransaction.connection, botransaction.transaction);
					}
					// -- 2024-09-09 generate DESADV
					var lsEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(insertedNr, botransaction.connection, botransaction.transaction);
					Apps.EDI.Handlers.Order.GenerateDesadvResponse(lsEntity, botransaction);

					//commiting
					if(botransaction.commit())
					{

						var deliveryNoteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(insertedNr);
						deliveryNoteEntity.Benutzer = $"Gebucht, {this._user.Username}, {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}";
						deliveryNoteEntity.Projekt_Nr = angeboteEntity.Projekt_Nr;
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(deliveryNoteEntity);

						var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(
								this._data.Items?
									.Select(x => x.ItemNumber)?
									.ToList(), null);
						// - get LS Pos
						var pos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(deliveryNoteEntity.Nr);//
						generateDATFile(errors, deliveryNoteEntity, pos, artikelEntities);
						//logging
						var _log = new LogHelper(deliveryNoteEntity.Nr, (int)deliveryNoteEntity.Angebot_Nr, int.TryParse(deliveryNoteEntity.Projekt_Nr, out var v) ? v : 0, deliveryNoteEntity.Typ, LogHelper.LogType.CREATIONOBJECT, "CTS", _user)
							.LogCTS(null, null, null, 0);
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
						Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.LS, false);
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
						return new ResponseModel<int>
						{
							Success = true,
							Body = insertedNr,
							Errors = errors.Select(x =>
								new ResponseModel<int>.ResponseError
								{
									Key = x.Key,
									Value = x.Value
								}).ToList()
						};
					}
					else
					{
						Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.LS, false);
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
						return new ResponseModel<int>()
						{
							Success = false,
							Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Transaction error"}
					}
						};
					}
				} catch(Exception e)
				{
					botransaction.rollback();
					Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.LS, false);
					Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
					Infrastructure.Services.Logging.Logger.Log(e);
					Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
					throw;
				}
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(this._data.AngebotNr <= 0)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Project Number [{this._data.AngebotNr}] invalid"}
					}
				};
			}

			var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.Nr);
			if(orderEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "Order confirmation Note not found"}
					}
				};
			}
			if(!_data.DeliveryDate.HasValue)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Estimated Arrival date is required"}
					}
				};
			}
			if(_data.DeliveryDate < DateTime.Today)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Estimated Arrival date [{_data.DeliveryDate.Value.ToString("dd.MM.yyyy")}] cannot be in the past"}
					}
				};
			}
			if(orderEntity.Erledigt == true)
			{
				return ResponseModel<int>.FailureResponse($"Order Confirmation is done, cannot create Delivery Note");
			}
			var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderEntity.Kunden_Nr.Value);
			if(!addressenEntity.Kundennummer.HasValue)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "Order confirmation does not have a customer number"}
					}
				};
			}

			var errors = new List<string> { };
			if(orderEntity.Typ.ToLower() != "auftragsbestätigung")
			{
				errors.Add($"Order: Type is not Auftragsbestätigung");
			}
			//if (orderEntity.Gebucht == false)
			//{
			//    errors.Add($"Order: Gebucht is false");
			//}
			if(orderEntity.Erledigt == true)
			{
				errors.Add($"Order: Erledigt is true");
			}

			//--
			this._data.Items = this._data.Items?.Where(x => !x.Done && x.AktuelleLiefermenge > 0).ToList();

			var angeboteArtikelEntities = this._data.Items.Where(x => !x.Done && x.AktuelleLiefermenge > 0).ToList();
			var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(
					this._data.Items?
						.Select(x => x.ItemNumber)?
						.ToList());

			var articleNrs = artikelEntities?.Select(x => x.ArtikelNr)?.ToList();
			var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);
			var articleNrKundeIndex = new List<KeyValuePair<int, string>>();
			foreach(var item in _data.Items)
			{
				var articleItem = artikelEntities?.FirstOrDefault(x => x.ArtikelNummer == item.ItemNumber);
				if(!articleNrKundeIndex.Exists(x => x.Key == (articleItem?.ArtikelNr ?? -1) && x.Value?.Trim() == item.Index_Kunde?.Trim()))
				{
					articleNrKundeIndex.Add(new KeyValuePair<int, string>(articleItem.ArtikelNr, item?.Index_Kunde));
				}
			}
			var lagerExtensionEntity = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndex(articleNrKundeIndex);

			var itemPricingGroupDbs = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(articleNrs);
			//-

			foreach(var item in this._data.Items)
			{
				var artikelEntity = artikelEntities.FirstOrDefault(x => x.ArtikelNummer == item.ItemNumber);
				// - 2025-05-10-09 
				if(item.ItemTypeId == (int)Core.CustomerService.Enums.OrderEnums.ItemType.Serie)
				{
					var itemPricingGroupDb = itemPricingGroupDbs.FirstOrDefault(x => x.Artikel_Nr == artikelEntity?.ArtikelNr);
					if(itemPricingGroupDb == null)
					{
						errors.Add($"Article {item.ItemNumber} has no verkaufspreis");
					}
				}
				if(string.IsNullOrEmpty(item.ItemNumber) || string.IsNullOrWhiteSpace(item.ItemNumber))
				{
					errors.Add($"Article must not be empty");
				}
				if(!item.StorageLocationId.HasValue)
				{
					errors.Add($"Storage must not be empty");
				}
				if(!item.OpenQuantity_Quantity.HasValue)
				{
					errors.Add($"Order quantity must not be empty");
				}
				if(!item.UnitPrice.HasValue || (item.UnitPrice.HasValue && item.UnitPrice.Value < 0))
				{
					errors.Add($"Article {item.ItemNumber}: unit price not valid");
				}
				if(!item.AktuelleLiefermenge.HasValue || (item.AktuelleLiefermenge.HasValue && item.AktuelleLiefermenge.Value < 0))
				{
					errors.Add($"Article {item.ItemNumber}: invalid quantity");
				}
				else
				{
					if(item.AktuelleLiefermenge.HasValue && item.AktuelleLiefermenge.Value > 0)
					{
						var angeboteArtikelItem = (item.Id == -1) ? angeboteArtikelEntities.Find(x => x.index == item.index)
							: angeboteArtikelEntities.Find(x => x.Id == item.Id);
						if(angeboteArtikelItem != null && item.AktuelleLiefermenge > angeboteArtikelItem.OpenQuantity_Quantity)
						{
							errors.Add($"Pos [{item.Position}]- Article {item.ItemNumber}: quantity greater than Order");
						}
						else
						{
							var artikelItem = artikelEntities?.Find(x => x.ArtikelNummer == item.ItemNumber);
							if(artikelItem != null)
							{
								var lagerItem = lagerEntities?.Find(x => x.Artikel_Nr == artikelItem.ArtikelNr && x.Lagerort_id == item.StorageLocationId);
								var lagerExtItem = lagerExtensionEntity.Find(x => x.ArtikelNr == artikelItem.ArtikelNr && x.Lagerort_id == item.StorageLocationId && x.Index_Kunde == item.Index_Kunde);
								if(lagerItem != null)
								{
									errors.AddRange(validateArticle(item, artikelItem, lagerItem, lagerExtItem)?.Select(x => x.Value)?.ToList() ?? new List<string>());
								}
							}
						}
						if(!item.StorageLocationId.HasValue || (item.StorageLocationId.HasValue && item.StorageLocationId.Value == -1))
						{
							errors.Add($"Position [{item.Position}] has no storage location");
						}
					}
				}
			}

			// - 2022-06-03 - KI - Index Pos <> Index Article AND Index Pos not in BOM Index
			var bomSnapshotIndexes = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetKundenIndexByArticle(articleNrs)
				?? new List<KeyValuePair<int, string>>();
			//var technicArticles = Program.BSD.TechnicArticleIds;
			foreach(var item in this._data.Items)
			{
				var itemEntity = artikelEntities?.FirstOrDefault(x => x.ArtikelNummer?.Trim() == item.ItemNumber?.Trim());
				if(itemEntity?.Index_Kunde?.Trim() != item.Index_Kunde?.Trim() && !bomSnapshotIndexes.Exists(x => x.Value?.Trim() == item.Index_Kunde?.Trim() && x.Key == itemEntity?.ArtikelNr))
				{
					errors.Add($"Position [{item.Position}]: Index Kunde invalid");
				}
				if(!HorizonsHelper.ArticleIsTechnic(itemEntity.ArtikelNr))
				{
					DateTime _newDate, _oldDate;
					_newDate = _oldDate = item.DeliveryDate ?? new DateTime(1900, 1, 1);
					// - 2023-11-03 - Reil - accept position in the past
					if(_newDate < DateTime.Today)
					{
						_newDate = DateTime.Today;
					}
					var horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasLSPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
					if(!horizonCheck)
						errors.AddRange(messages);
				}
			}

			if(errors.Count > 0)
			{
				return new ResponseModel<int>
				{
					Success = false,
					Errors = errors.Select(x => new ResponseModel<int>.ResponseError { Key = "", Value = x }).Distinct().ToList()
				};
			}

			// Validate StorageLocation and UnloadingPoint BEFORE updating the entity

			var kundeEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByAddressNr(addressenEntity.Nr);
			// AB
			if((string.IsNullOrEmpty(orderEntity.StorageLocation) || string.IsNullOrEmpty(orderEntity.UnloadingPoint)) &&
				(kundeEntity.Edi_Aktiv_Desadv == true))
			{
				// Delfor
				var lineitemplan = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(orderEntity?.nr_dlf ?? 0);
				var lineitem = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(lineitemplan?.LineItemId ?? 0);
				var delforheader = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(lineitem?.HeaderId ?? 0);
				var delforAddress = new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity();
				if(delforheader == null)
				{
					var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(addressenEntity.Nr);
					delforAddress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderEntity.LsAddressNr is not null ? orderEntity.LsAddressNr.Value : (customerEntity.LSADR ?? 0));
				}
				if(string.IsNullOrEmpty(delforheader is not null ? delforheader.ConsigneeUnloadingPoint : delforAddress?.UnloadingPoint) ||
						string.IsNullOrEmpty(delforheader is not null ? delforheader.ConsigneeStorageLocation : delforAddress?.StorageLocation))
				{
					return new ResponseModel<int>()
					{
						Success = false,
						Errors = new List<ResponseModel<int>.ResponseError>() {
							new ResponseModel<int>.ResponseError {
								Key = "1",
								Value = "To proceed, please enter the unloading point and the local storage location in the 'Delivery Address' section of the order confirmation."
							}
						}
					};
				}
			}
			//


			return ResponseModel<int>.SuccessResponse();
		}
		internal List<KeyValuePair<string, string>> validateArticle(
			Models.DeliveryNote.ItemModel item,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.LagerEntity lagerEntity,
			Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity lagerExtEntity
			)
		{
			var errors = new List<KeyValuePair<string, string>>();

			// 1-, 2-, 3-
			if(artikelEntity?.FreigabestatusTNIntern.ToLower() == "b")
			{
				errors.Add(new KeyValuePair<string, string>(
					"Artikel gesperrt",
				   $"Lieferung ist derzeit nicht möglich! \n\r Artikel {artikelEntity.ArtikelNummer} ist gesperrt: Status-Intern auf {artikelEntity.FreigabestatusTNIntern} gesetzt!"
				   ));
			}

			//4-
			if(artikelEntity.ArtikelNummer.ToLower() != "reparatur" && artikelEntity.Freigabestatus.ToLower() == "n")
			{
				errors.Add(new KeyValuePair<string, string>(
					"Erstmuster gesperrt",
				   $"Lieferung ist derzeit nicht möglich!, Status-extern auf N gesetzt! \n\r Artikelnummer {artikelEntity.ArtikelNummer} ist gesperrt!"
				   ));
			}

			//5-
			if(artikelEntity.ArtikelNummer.ToLower() != "reparatur" && artikelEntity.FreigabestatusTNIntern.ToLower() == "n")
			{
				errors.Add(new KeyValuePair<string, string>(
					"Status-Intern",
				   $"Lieferung ist derzeit nicht möglich,\n\r Artikelnummer {artikelEntity.ArtikelNummer} ist gesperrt:Status-Inter auf N gesetzt!"
				   ));
			}

			if(Convert.ToDecimal(lagerEntity?.Bestand ?? 0) < item.AktuelleLiefermenge)
			{
				var lagerName = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(lagerEntity?.Lagerort_id ?? -1);
				errors.Add(new KeyValuePair<string, string>("Invalid quantity", $"Article {artikelEntity.ArtikelNummer}: the quantity of the warehouse [{lagerName?.Lagerort}]  [{lagerEntity?.Bestand}] < [{item.AktuelleLiefermenge}]"));
			}

			return errors;
		}
		internal void generateDATFile(
			List<KeyValuePair<string, string>> errors,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
			List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
		{
			if(!angeboteEntity.Kunden_Nr.HasValue)
			{
				errors.Add(new KeyValuePair<string, string>("", "Customer not found"));
				return;
			}

			var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(angeboteEntity.Kunden_Nr.Value);
			var WmsAngebotNr = angeboteEntity.Angebot_Nr;
			var path = $@"{Program.CTS.DeliveryNoteFilesPath}\WA{DateTime.Now.ToString("yyyyMMddhhmmss")}.dat";
			var content = $"AG;1;1;{WmsAngebotNr};{angeboteneArtikelEntities?.Count};50;{angeboteEntity.Datum.Value.ToString("yyyyMMdd")};{angeboteEntity.Versanddatum_Auswahl?.ToString("yyyyMMdd")};1;0;0;1;1;{addressenEntity.Kundennummer.Value};{angeboteEntity.Vorname_NameFirma.Substring(0, Math.Min(angeboteEntity.Vorname_NameFirma.Length, 37))}";
			foreach(var angeboteneArtikelEntity in angeboteneArtikelEntities)
			{
				var artikelEntity = artikelEntities.Where(x => x.ArtikelNr == angeboteneArtikelEntity.ArtikelNr)?.ToList().FirstOrDefault();

				if(artikelEntity != null && (artikelEntity.Warengruppe.ToUpper() == "EF" || artikelEntity.Warengruppe.ToUpper() == "ROH"))
				{
					content += $"\nAG;2;1;{WmsAngebotNr};{angeboteneArtikelEntity.Position};{artikelEntity.ArtikelNr};{angeboteneArtikelEntity.Anzahl}";
				}
			}

			File.WriteAllText(path, content);
		}
	}
}
