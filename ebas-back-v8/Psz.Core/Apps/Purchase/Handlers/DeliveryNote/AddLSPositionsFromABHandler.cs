using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class AddLSPositionsFromABHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.DeliveryNote.LSPositionsFromABModel _data { get; set; }
		public AddLSPositionsFromABHandler(Identity.Models.UserModel user, Models.DeliveryNote.LSPositionsFromABModel model)
		{
			_user = user;
			_data = model;
		}

		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				lock(Locks.DeliveryNotesLock)
				{
					//opening sql transaction
					botransaction.beginTransaction();

					var errors = new List<KeyValuePair<string, string>>();
					List<int> InsertedIds = new List<int>();

					var ABEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.NrAB);
					var deliveryNoteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.NrLS);
					var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(Convert.ToInt32(ABEntity.Kunden_Nr ?? 0));

					// 0.1 - 
					var angeboteTermin = this._data.Items.All(x => x.termin_eingehalten == true); // >>>> ?Ridha
					ABEntity.Termin_eingehalten = angeboteTermin; // >>>> § 0.1 -
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(ABEntity, botransaction.connection, botransaction.transaction);


					if(this._data.Items != null && this._data.Items.Count > 0)
					{
						var itemsIds = this._data.Items.Where(x => x.Id != -1).Select(y => y.Id).ToList();
						var angeboteArtikelEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(itemsIds)?
							.Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false).ToList();

						// -
						if(angeboteArtikelEntities != null && angeboteArtikelEntities.Count > 0)
						{
							var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(
									this._data.Items?
										.Select(x => x.ItemNumber)?
										.ToList(), null);

							var articleNrs = artikelEntities?.Select(x => x.ArtikelNr)?.ToList();

							var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);

							var itemsDone = this._data.Items.FindAll(x => x.AktuelleLiefermenge > 0);
							foreach(var item in itemsDone)
							{
								var artikel = artikelEntities.FirstOrDefault(x => x.ArtikelNummer == item.ItemNumber);
								if(item.Id != -1)
								{
									var angeboteneArtikelEntity = angeboteArtikelEntities?.FirstOrDefault(x => x.ArtikelNr == (artikel?.ArtikelNr ?? -1));

									if(angeboteneArtikelEntity != null)
									{
										angeboteneArtikelEntity.termin_eingehalten = item.termin_eingehalten;
										Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(angeboteneArtikelEntity, botransaction.connection, botransaction.transaction);
									}

									if(artikel != null)
									{
										Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditWithTransaction(artikel, botransaction.connection, botransaction.transaction);
									}
								}

								var itemArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(item.ItemNumber);
								var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemArtikelEntity?.ArtikelNr ?? -1);
								if(item.AktuelleLiefermenge > 0)
								{
									var Bez1 = (string.IsNullOrEmpty(item.Designation1) || string.IsNullOrWhiteSpace(item.Designation1)) ? null :
										(item.Designation1.Length >= 200) ? artikel.Bezeichnung1.Substring(0, 200) : item.Designation1;
									var Bez3 = (string.IsNullOrEmpty(artikel.Bezeichnung3) || string.IsNullOrWhiteSpace(artikel.Bezeichnung3)) ? null :
										(artikel.Bezeichnung3.Length >= 200) ? artikel.Bezeichnung3.Substring(0, 200) : artikel.Bezeichnung3;
									InsertedIds.Add(Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
									{
										AngebotNr = this._data.NrLS,
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
										Preiseinheit = item.UnitPriceBasis ?? 1, // - 2022-05-30 - init as 1 to respect DB Constraint
										Zeichnungsnummer = item.DrawingIndex,
										Liefertermin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
										//DateTime.Now,
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
										Versandarten_Auswahl = deliveryNoteEntity.Versandarten_Auswahl,// this._data.Standardversand,
										Versanddatum_Auswahl = deliveryNoteEntity.Versanddatum_Auswahl,//this._data.Versandatum ?? null,
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
							var LSItemsAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(InsertedIds, botransaction.connection, botransaction.transaction)
								?.Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false)
								?.ToList();

							UpdateDeliveryNote(errors, deliveryNoteEntity, ABEntity, LSItemsAfterInsert, artikelEntities, botransaction);
						}
					}

					if(botransaction.commit())
					{
						var LSItemsAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(InsertedIds);
						var AllLSItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.NrLS);
						var LSItemsArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(AllLSItems.Select(l => (int)l.ArtikelNr).ToList() ?? new List<int> { -1 });
						generateDATFile(deliveryNoteEntity, AllLSItems, LSItemsArticles);
						//logging
						List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
						var _log = new LogHelper(deliveryNoteEntity.Nr, (int)deliveryNoteEntity.Angebot_Nr, int.TryParse(deliveryNoteEntity.Projekt_Nr, out var v) ? v : 0, deliveryNoteEntity.Typ, LogHelper.LogType.CREATIONPOS, "CTS", _user);
						foreach(var item in LSItemsAfterInsert)
						{
							_logs.Add(_log.LogCTS(null, null, null, (int)item.Position, item.Nr));
						}

						if(_logs != null && _logs.Count > 0)
							Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logs);
						return new ResponseModel<int>
						{
							Success = true,
							Body = 1,
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
						return ResponseModel<int>.FailureResponse("Transaction error");
					}
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var LSEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.NrLS);
			var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(LSEntity.Kunden_Nr.Value);
			if(!addressenEntity.Kundennummer.HasValue)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "Delivery Note does not have a customer number"}
					}
				};
			}
			if(LSEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "Delivery Note not found"}
					}
				};
			}

			var errors = new List<string> { };
			if(LSEntity.Erledigt == true)
			{
				errors.Add($"Delivery Note: Erledigt is true");
			}

			//--
			var angeboteArtikelEntities = this._data.Items.Where(x => !x.Done).ToList();
			var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(
					this._data.Items?
						.Select(x => x.ItemNumber)?
						.ToList(), null);

			var articleNrs = artikelEntities?.Select(x => x.ArtikelNr)?.ToList();
			var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);

			//-
			var NotNullItems = this._data.Items.Where(a => a.OpenQuantity_Quantity.HasValue && a.OpenQuantity_Quantity.Value > 0);
			foreach(var item in NotNullItems)
			{
				var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(item.ItemNumber);
				var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(artikelEntity.ArtikelNr);
				if(itemPricingGroupDb == null)
				{
					errors.Add($"Article {item.ItemNumber} has no verkaufspreis");
				}
				//if (string.IsNullOrEmpty(item.Index_Kunde) || string.IsNullOrWhiteSpace(item.Index_Kunde))
				//{
				//    errors.Add($"Index Kunde must have a value");
				//}
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
							errors.Add($"Article {item.ItemNumber}: quantity greater than Order");
						}
						else
						{
							var artikelItem = artikelEntities?.Find(x => x.ArtikelNummer == item.ItemNumber);
							if(artikelItem != null)
							{
								var lagerItem = lagerEntities?.Find(x => x.Artikel_Nr == artikelItem.ArtikelNr && x.Lagerort_id == item.StorageLocationId);
								var lagerExtensionItem = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(artikelItem.ArtikelNr, item.Index_Kunde, item.StorageLocationId ?? -1);
								if(lagerItem != null)
								{
									errors.AddRange(validateArticle(item, artikelItem, lagerItem, lagerExtensionItem)?.Select(x => x.Value)?.ToList() ?? new List<string>());
								}
							}
						}
						if(!item.StorageLocationId.HasValue || (item.StorageLocationId.HasValue && item.StorageLocationId.Value == -1))
						{
							errors.Add($"position with Article {item.ItemNumber}: has no storage location");
						}
					}
				}
				//var technicArtices = Program.BSD.TechnicArticleIds;
				var artikelItem_ = artikelEntities?.Find(x => x.ArtikelNummer == item.ItemNumber);
				if(!Core.CustomerService.Helpers.HorizonsHelper.ArticleIsTechnic(artikelItem_.ArtikelNr))
				{
					DateTime _newDate, _oldDate;
					_newDate = _oldDate = item.DeliveryDate ?? new DateTime(1900, 1, 1);
					var horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasLSPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
					if(!horizonCheck)
						errors.AddRange(messages);
				}
			}

			if(errors.Count > 0)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = errors.Select(x => new ResponseModel<int>.ResponseError { Key = "", Value = x }).Distinct().ToList()
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
		internal List<KeyValuePair<string, string>> validateArticle(
		   Models.DeliveryNote.ItemModel item,
		   Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
		   Infrastructure.Data.Entities.Tables.PRS.LagerEntity lagerEntity,
		   Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity lagerExtensionEntity)
		{
			var errors = new List<KeyValuePair<string, string>>();

			// 1-, 2-, 3-
			if(/*artikelEntity.FreigabestatusTNIntern.ToLower() == "r" ||*/ artikelEntity.FreigabestatusTNIntern.ToLower() == "b")
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

			//if ((lagerExtensionEntity?.Bestand ?? 0) < item.AktuelleLiefermenge)
			//{
			//    var lagerName = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(lagerExtensionEntity?.Lagerort_id ?? -1);
			//    errors.Add(new KeyValuePair<string, string>("Invalid quantity", $"Article {artikelEntity.ArtikelNummer}: the quantity of the warehouse [{lagerName?.Lagerort}]  [{lagerExtensionEntity?.Bestand}] < [{item.AktuelleLiefermenge}]"));
			//}

			return errors;
		}

		internal void UpdateDeliveryNote(
			List<KeyValuePair<string, string>> errors,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity deliveryNoteEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity ABEntity,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> LSItemsAfterInsert,
			List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities,
			Infrastructure.Services.Utils.TransactionsManager transactionManager)
		{
			lock(Locks.DeliveryNotesLock)
			{
				var articleNrs = artikelEntities?.Select(x => x.ArtikelNr).ToList();
				var lagerEntites = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);
				var lagerVorgExtenionEntities = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndex(LSItemsAfterInsert.Select(x => new KeyValuePair<int, string>(x.ArtikelNr ?? -1, x.Index_Kunde))?.ToList());
				// -
				var toUpdateLagers = new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
				var toUpdateLagersExt = new List<Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtensionModel>();

				foreach(var item in LSItemsAfterInsert)
				{
					var lagerItem = lagerEntites.Find(x => x.Artikel_Nr == item.ArtikelNr && x.Lagerort_id == item.Lagerort_id);
					var v = lagerVorgExtenionEntities.Find(x => x.ArtikelNr == item.ArtikelNr && x.Lagerort_id == item.Lagerort_id && x.Index_Kunde == item.Index_Kunde);
					if(lagerItem != null)
					{
						lagerItem.Bestand = lagerItem.Bestand - item.Anzahl;
						lagerItem.letzte_Bewegung = DateTime.Now;
						//Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateWithTransaction(lagerItem, connection, transaction);
						toUpdateLagers.Add(lagerItem);

						// -
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
				}

				// - refactor
				if(toUpdateLagers.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateWithTransaction(toUpdateLagers, transactionManager.connection, transactionManager.transaction);
				}

				// - 2022-03-11 track KundenIndex for Lager Bestand
				if(toUpdateLagersExt.Count > 0)
				{
					Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtension(this._user, toUpdateLagersExt, transactionManager);
				}

				//FIXME: CONFIRM !!! >>>>>
				//var LSItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(deliveryNoteEntity.Nr); // - angeboteneArtikelEntities.Where(x => x.AngebotNr == angeboteEntity.Nr).ToList();
				var ABItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(ABEntity.Nr);
				foreach(var ABitem in ABItems)
				{
					var LSitem = LSItemsAfterInsert.Where(x => x.LSPoszuABPos == ABitem.Nr)?.ToList().FirstOrDefault();
					if(!ABitem.Preiseinheit.HasValue || ABitem.Preiseinheit.Value == 0)
					{
						errors.Add(new KeyValuePair<string, string>("", $"{ABitem.Position}. Preiseinheit: invalid value {ABitem.Preiseinheit.Value}"));
						continue;
					}
					if(LSitem != null)
					{
						// 1.4
						if(ABitem.erledigt_pos.HasValue && ABitem.erledigt_pos.Value)
							ABitem.erledigt_pos = false;
						ABitem.Anzahl = ABitem.Anzahl - LSitem.Anzahl;
						ABitem.Geliefert = ABitem.Geliefert + LSitem.Anzahl;
						ABitem.Gesamtpreis = (ABitem.Anzahl - LSitem.Anzahl) / ABitem.Preiseinheit * ABitem.Einzelpreis * (1 - ABitem.Rabatt);
						ABitem.erledigt_pos = ABitem.Anzahl - LSitem.Anzahl > 0 ? false : true;

						// 1.5
						ABitem.Einzelkupferzuschlag = Math.Round((decimal)(((ABitem.DEL * 1.01m) - ABitem.Kupferbasis)
																				  / 100
																				  * (decimal?)ABitem.EinzelCuGewicht), 2);

						// 1.6 
						ABitem.GesamtCuGewicht = ABitem.Anzahl * ABitem.EinzelCuGewicht;
						ABitem.Einzelpreis = ABitem.VKFestpreis.HasValue && ABitem.VKFestpreis.Value == true
							? ABitem.VKEinzelpreis
							: ABitem.Einzelkupferzuschlag * ABitem.Preiseinheit + ABitem.VKEinzelpreis;

						// 1.7
						ABitem.Gesamtpreis = ABitem.Einzelpreis / ABitem.Preiseinheit * ABitem.Anzahl * (1 - ABitem.Rabatt);
						ABitem.Gesamtkupferzuschlag = ABitem.VKFestpreis.HasValue && ABitem.VKFestpreis.Value == true
							? 0
							: ABitem.Anzahl * ABitem.Einzelkupferzuschlag;
						ABitem.VKGesamtpreis = ABitem.Anzahl * ABitem.VKEinzelpreis / ABitem.Preiseinheit;

						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(ABitem, transactionManager.connection, transactionManager.transaction);
					}
				}
				deliveryNoteEntity.Gebucht = true;
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(deliveryNoteEntity, transactionManager.connection, transactionManager.transaction);
			}
		}

		internal void generateDATFile(
Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
		{
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
