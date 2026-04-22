using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class AddLSValidatdatedPositionHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.DeliveryNote.ItemModel _data { get; set; }
		public AddLSValidatdatedPositionHandler(Identity.Models.UserModel user, Models.DeliveryNote.ItemModel model)
		{
			_user = user;
			_data = model;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				lock(Locks.DeliveryNotesLock)
				{
					var errors = new List<KeyValuePair<string, string>>();
					int InsertId = 0;
					//opening sql transaction
					var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
					botransaction.beginTransaction();
					var deliveryNoteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(this._data.OrderId, botransaction.connection, botransaction.transaction);

					if(this._data != null)
					{
						var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ItemNumber ?? null);
						var artikelNr = artikelEntity?.ArtikelNr ?? -1;

						var lagerEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { artikelNr });
						var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(artikelEntity?.ArtikelNr ?? -1);

						if(this._data.AktuelleLiefermenge > 0)
						{
							if(artikelNr == 223)
							{
								InsertId = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
								{
									AngebotNr = this._data.OrderId,
									ArtikelNr = 223,
									Bezeichnung1 = "Fracht+Verpackung",
									Anzahl = 1,
									OriginalAnzahl = 0,
									///// >>> get [PSZ_Auftrag LS 051 Filter für Versandkosten] QUERY
									Einzelpreis = this._data.UnitPrice.HasValue ? this._data.UnitPrice.Value : 0, //>>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0) 
									Gesamtpreis = this._data.UnitPrice.HasValue ? this._data.UnitPrice.Value : 0, //>>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0)
									Rabatt = 0,
									USt = this._data.VAT ?? 0.19m,
									Lagerbewegung = false,
									Lagerbewegung_rückgängig = false,
									Auswahl = false,
									FM_Einzelpreis = 0,
									FM_Gesamtpreis = 0,
									Summenberechnung = false,
									Preiseinheit = 1,
									Liefertermin = this._data.DeliveryDate ?? Convert.ToDateTime(DateTime.Now.ToShortDateString()),
									erledigt_pos = false,
									Stückliste = false,
									Stückliste_drucken = false,
									Langtext = "No",//to check with Ridha
									Langtext_drucken = false,
									Lagerort_id = 3,
									Seriennummern_drucken = false,
									Wunschtermin = new DateTime(2999, 12, 31),
									Fertigungsnummer = 0,
									Geliefert = 0,
									Position = 999,
									VKEinzelpreis = this._data.UnitPrice.HasValue ? this._data.UnitPrice.Value : 0, // >>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0) 
									VKGesamtpreis = this._data.UnitPrice.HasValue ? this._data.UnitPrice.Value : 0, // >>>>> IIf([Versandkosten in EUR]<>"",[Versandkosten in EUR],0)
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
									POSTEXT = this._data.Postext,
									Index_Kunde = "", // - Empty b/c Index is required
									Index_Kunde_Datum = null,
									Versandarten_Auswahl = deliveryNoteEntity.Versandarten_Auswahl,
									Versanddatum_Auswahl = deliveryNoteEntity.Versanddatum_Auswahl,
									CSInterneBemerkung = this._data.CSInterneBemerkung,
								}, botransaction.connection, botransaction.transaction);
							}
							else
							{
								var Bez1 = (string.IsNullOrEmpty(this._data.Designation1) || string.IsNullOrWhiteSpace(this._data.Designation1)) ? null :
								(this._data.Designation1.Length >= 200) ? this._data.Designation1.Substring(0, 200) : this._data.Designation1;
								var Bez3 = (string.IsNullOrEmpty(artikelEntity.Bezeichnung3) || string.IsNullOrWhiteSpace(artikelEntity.Bezeichnung3)) ? null :
									(artikelEntity.Bezeichnung3.Length >= 200) ? artikelEntity.Bezeichnung3.Substring(0, 200) : artikelEntity.Bezeichnung3;

								// R3
								InsertId = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
								{
									AngebotNr = this._data.OrderId,
									ArtikelNr = artikelEntity.ArtikelNr,
									Bezeichnung1 = Bez1,//column type in DB is nvarchar(200),
									Bezeichnung2 = this._data.Designation2,
									Bezeichnung3 = Bez3, //column type in DB is nvarchar(200),
									Einheit = artikelEntity.Einheit,
									Anzahl = this._data.AktuelleLiefermenge,
									OriginalAnzahl = this._data.OriginalOrderQuantity,
									Preisgruppe = itemPricingGroupDb?.Preisgruppe,
									//Bestellnummer = angeboteneArtikelEntity.Bestellnummer,
									Rabatt = this._data.Discount,
									USt = this._data.VAT.HasValue ? this._data.VAT.Value / 100 : 0m,
									//POSTEXT = angeboteneArtikelEntity.POSTEXT,
									Preiseinheit = this._data.UnitPriceBasis,
									Zeichnungsnummer = this._data.DrawingIndex,
									Liefertermin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
									//DateTime.Now,
									//this._data.DeliveryDate ?? null,
									erledigt_pos = false,
									Lagerort_id = this._data.StorageLocationId,
									Wunschtermin = this._data.DesiredDate,
									Fertigungsnummer = this._data.ProductionNumber,
									Geliefert = 0m,
									LSPoszuABPos = this._data.Id,
									Position = this._data.Position,
									VKFestpreis = this._data.FixedTotalPrice,
									EKPreise_Fix = this._data.FixedUnitPrice,//to check with Sani
																			 //
									Einzelpreis = this._data.OpenQuantity_UnitPrice,
									Gesamtpreis = this._data.OpenQuantity_TotalPrice,
									//
									DELFixiert = this._data.DelFixed,
									Abladestelle = this._data.UnloadingPoint,
									termin_eingehalten = this._data.termin_eingehalten,
									RP = this._data.RP,
									// R4
									Kupferbasis = int.TryParse(this._data.CopperBase.ToString(), out var val) ? val : 0,
									DEL = int.TryParse(this._data.DelNote.ToString(), out var val2) ? val2 : 0,
									EinzelCuGewicht = this._data.CopperWeight,
									GesamtCuGewicht = this._data.OpenQuantity_CopperWeight,
									Einzelkupferzuschlag = this._data.CopperSurcharge,
									VKGesamtpreis = this._data.TotalPrice,
									Versandarten_Auswahl = deliveryNoteEntity.Versandarten_Auswahl,
									Versanddatum_Auswahl = deliveryNoteEntity.Versanddatum_Auswahl,
									VKEinzelpreis = this._data.UnitPrice,
									//
									Versandinfo_von_CS = this._data.Versandinfo_von_CS,
									Packstatus = this._data.Packstatus,
									Gepackt_von = this._data.Gepackt_von,
									Gepackt_Zeitpunkt = this._data.Gepackt_Zeitpunkt,
									Packinfo_von_Lager = this._data.Packinfo_von_Lager,
									//!Shipping
									Versandstatus = this._data.Versandstatus,
									Versanddienstleister = this._data.Versanddienstleister,
									Versandnummer = this._data.Versandnummer,
									Versandinfo_von_Lager = this._data.Versandinfo_von_Lager,
									EDI_PREIS_KUNDE = this._data.EDI_PREIS_KUNDE,
									EDI_PREISEINHEIT = this._data.EDI_PREISEINHEIT,
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
									Typ = this._data.ItemTypeId,
									Bemerkungsfeld1 = this._data.Note1,
									Bemerkungsfeld2 = this._data.Note2,
									Freies_Format_EDI = this._data.FreeText,
									POSTEXT = this._data.Postext,
									Index_Kunde = this._data.Index_Kunde,
									Index_Kunde_Datum = this._data.Index_Kunde_Datum,
									CSInterneBemerkung = this._data.CSInterneBemerkung,
								}, botransaction.connection, botransaction.transaction);

							}

							// --------------- 2nd validate
							var angeboteArtikelEntityAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(InsertId, botransaction.connection, botransaction.transaction);
							//.Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false);
							if(angeboteArtikelEntityAfterInsert != null /*&& angeboteArtikelEntityAfterInsert?.erledigt_pos.Value == false*/)
							{ UpdateDeliveryNote(errors, deliveryNoteEntity, angeboteArtikelEntityAfterInsert, artikelEntity, botransaction.connection, botransaction.transaction); }

						}
					}
					if(botransaction.commit())
					{
						var _deliveryNoteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.OrderId);
						var angeboteArtikelEntityAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(InsertId);
						var LSAllItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.OrderId);
						var LSItemsArticlesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(LSAllItems.Select(l => (int)l.ArtikelNr).ToList() ?? new List<int> { -1 });
						generateDATFile(_deliveryNoteEntity, LSAllItems, LSItemsArticlesEntities);
						//logging
						var _log = new LogHelper(_deliveryNoteEntity.Nr, (int)_deliveryNoteEntity.Angebot_Nr, int.TryParse(_deliveryNoteEntity.Projekt_Nr, out var v) ? v : 0, _deliveryNoteEntity.Typ, LogHelper.LogType.CREATIONPOS, "CTS", _user)
							.LogCTS(null, null, null, (int)angeboteArtikelEntityAfterInsert.Position, angeboteArtikelEntityAfterInsert.Nr);
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

						return new ResponseModel<int>
						{
							Success = true,
							Body = this._data.OrderId,
							Errors = errors.Select(x =>
								new ResponseModel<int>.ResponseError
								{
									Key = x.Key,
									Value = x.Value
								}).ToList()
						};
					}
					else
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

			if(this._data.OrderId <= 0)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Project Number [{this._data.OrderId}] invalid"}
					}
				};
			}

			var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.OrderId);
			var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderEntity.Kunden_Nr.Value);
			if(!addressenEntity.Kundennummer.HasValue)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "Order Confirmation does not have a customer number"}
					}
				};
			}
			if(orderEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "Order Confirmation not found"}
					}
				};
			}

			var errors = new List<string> { };
			//if (orderEntity.Typ.ToLower() != "auftragsbestätigung")
			//{
			//    errors.Add($"Order: Type is not Auftragsbestätigung");
			//}
			if(orderEntity.Typ.ToLower() != "lieferschein")
			{
				errors.Add($"Order: Type is not Lieferschein");
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

			var angeboteArtikelEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.OrderId)?
							.Where(x => !x.erledigt_pos.HasValue || x.erledigt_pos.HasValue && x.erledigt_pos.Value == false)?.ToList()
							?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ItemNumber);
			var artikelNr = artikelEntity?.ArtikelNr ?? -1;

			var lagerEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { artikelNr });
			var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(artikelEntity.ArtikelNr);



			//-
			if(itemPricingGroupDb == null)
			{
				errors.Add($"Article {this._data.ItemNumber} has no verkaufspreis");
			}
			if(string.IsNullOrEmpty(this._data.ItemNumber) || string.IsNullOrWhiteSpace(this._data.ItemNumber))
			{
				errors.Add($"Article must not be empty");
			}
			if(!this._data.StorageLocationId.HasValue)
			{
				errors.Add($"Storage must not be empty");
			}
			if(!this._data.OpenQuantity_Quantity.HasValue)
			{
				errors.Add($"Order quantity must not be empty");
			}
			if(!this._data.UnitPrice.HasValue || (this._data.UnitPrice.HasValue && this._data.UnitPrice.Value < 0))
			{
				errors.Add($"Article {this._data.ItemNumber}: unit price not valid");
			}
			if(!this._data.AktuelleLiefermenge.HasValue || (this._data.AktuelleLiefermenge.HasValue && this._data.AktuelleLiefermenge.Value < 0))
			{
				errors.Add($"Article {this._data.ItemNumber}: invalid quantity");
			}
			else
			{
				if(this._data.AktuelleLiefermenge.HasValue && this._data.AktuelleLiefermenge.Value > 0)
				{
					if(this._data.AktuelleLiefermenge > this._data.OpenQuantity_Quantity)
					{
						errors.Add($"Article {this._data.ItemNumber}: quantity greater than Order");
					}
					else
					{

						if(artikelEntity != null)
						{
							var lagerItem = lagerEntity?.Find(x => x.Artikel_Nr == artikelEntity.ArtikelNr && x.Lagerort_id == this._data.StorageLocationId);
							var lagerExtensionItem = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(artikelEntity.ArtikelNr, _data.Index_Kunde, (int)_data.StorageLocationId);
							if(lagerItem != null)
							{
								errors.AddRange(validateArticle(this._data, artikelEntity, lagerItem, lagerExtensionItem)?.Select(x => x.Value)?.ToList() ?? new List<string>());
							}
						}
					}
					if(!this._data.StorageLocationId.HasValue || (this._data.StorageLocationId.HasValue && this._data.StorageLocationId.Value == -1))
					{
						errors.Add($"position with Article {this._data.ItemNumber}: has no storage location");
					}
				}
			}
			//var technicArticles = Program.BSD.TechnicArticleIds;
			if(!Core.CustomerService.Helpers.HorizonsHelper.ArticleIsTechnic(artikelEntity.ArtikelNr))
			{
				DateTime _newDate, _oldDate;
				_newDate = _oldDate = _data.DeliveryDate ?? new DateTime(1900, 1, 1);
				var horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasLSPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
				if(!horizonCheck)
					errors.AddRange(messages);
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
			if(artikelEntity.FreigabestatusTNIntern.ToLower() == "r" || artikelEntity.FreigabestatusTNIntern.ToLower() == "b")
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

			if(Convert.ToDecimal(lagerEntity.Bestand ?? 0) < item.AktuelleLiefermenge && artikelEntity.ArtikelNr != 223)
			{
				var lagerName = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(lagerEntity.Lagerort_id ?? -1);
				errors.Add(new KeyValuePair<string, string>("Invalid quantity", $"Article {artikelEntity.ArtikelNummer}: the quantity of the warehouse [{lagerName?.Lagerort}]  [{lagerEntity.Bestand}] < [{item.AktuelleLiefermenge}]"));
			}

			//if ((lagerExtensionEntity?.Bestand ?? 0) < item.AktuelleLiefermenge && artikelEntity.ArtikelNr != 223)
			//{
			//    var lagerName = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(lagerExtensionEntity?.Lagerort_id??-1);
			//    errors.Add(new KeyValuePair<string, string>("Invalid quantity", $"Article {artikelEntity.ArtikelNummer}: the quantity of the warehouse [{lagerName?.Lagerort}]  [{lagerExtensionEntity?.Bestand}] < [{item.AktuelleLiefermenge}]"));
			//}

			return errors;
		}
		internal void UpdateDeliveryNote(
			List<KeyValuePair<string, string>> errors,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity deliveryNoteEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angeboteneArtikelEntities,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntities,
			SqlConnection connection, SqlTransaction transaction)
		{
			lock(Locks.DeliveryNotesLock)
			{

				deliveryNoteEntity.Benutzer = $"Gebucht, {this._user.Username}, {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}";

				//
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(deliveryNoteEntity, connection, transaction);

				// 1.2 -
				var artikelNr = artikelEntities?.ArtikelNr ?? -1;
				var lagerEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { artikelNr });


				if(angeboteneArtikelEntities != null)
				{
					var lagerItem = lagerEntity.Find(x => x.Artikel_Nr == artikelNr && x.Lagerort_id == this._data.StorageLocationId);

					if(lagerItem != null && lagerItem.Bestand > 0)
					{

						lagerItem.Bestand = lagerItem.Bestand - this._data.OpenQuantity_Quantity;
						lagerItem.letzte_Bewegung = DateTime.Now;
						Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateWithTransaction(lagerItem, connection, transaction);

						Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtension(this._user,
					 new Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtensionModel
					 {
						 ArticleId = angeboteneArtikelEntities.ArtikelNr ?? -1,
						 OldKundenIndex = angeboteneArtikelEntities.Index_Kunde,
						 NewKundenIndex = angeboteneArtikelEntities.Index_Kunde,
						 OldLagerorId = angeboteneArtikelEntities.Lagerort_id ?? -1,
						 NewLagerorId = angeboteneArtikelEntities.Lagerort_id ?? -1,
						 OldBestand = 0,
						 NewBestand = angeboteneArtikelEntities.Anzahl ?? 0,
					 }, new Infrastructure.Services.Utils.TransactionsManager { connection = connection, transaction = transaction });
					}
				}
				var angeboteneArtikelEntities_not_223 = (angeboteneArtikelEntities?.ArtikelNr != 223) ? angeboteneArtikelEntities : null;
				if(angeboteneArtikelEntities_not_223 != null)
				{
					if(!angeboteneArtikelEntities_not_223.Preiseinheit.HasValue || angeboteneArtikelEntities_not_223.Preiseinheit.Value == 0)
					{
						errors.Add(new KeyValuePair<string, string>("", $"{angeboteneArtikelEntities_not_223.Position}. Preiseinheit: invalid value {angeboteneArtikelEntities_not_223.Preiseinheit.Value}"));
					}
					angeboteneArtikelEntities_not_223.VKGesamtpreis = angeboteneArtikelEntities_not_223.VKFestpreis.HasValue && angeboteneArtikelEntities_not_223.VKFestpreis.Value
						? angeboteneArtikelEntities_not_223.Anzahl * angeboteneArtikelEntities_not_223.Einzelpreis / angeboteneArtikelEntities_not_223.Preiseinheit
						: ((angeboteneArtikelEntities_not_223.Einzelpreis / angeboteneArtikelEntities_not_223.Preiseinheit) - angeboteneArtikelEntities_not_223.Einzelkupferzuschlag) * angeboteneArtikelEntities_not_223.Anzahl;

					angeboteneArtikelEntities_not_223.Gesamtkupferzuschlag = angeboteneArtikelEntities_not_223.Anzahl * angeboteneArtikelEntities_not_223.Einzelkupferzuschlag;

					angeboteneArtikelEntities_not_223.Gesamtpreis = angeboteneArtikelEntities_not_223.Anzahl * angeboteneArtikelEntities_not_223.Einzelpreis / angeboteneArtikelEntities_not_223.Preiseinheit * (1 - angeboteneArtikelEntities_not_223.Rabatt);

					angeboteneArtikelEntities_not_223.GesamtCuGewicht = angeboteneArtikelEntities_not_223.Anzahl * angeboteneArtikelEntities_not_223.EinzelCuGewicht;
					angeboteneArtikelEntities_not_223.VKEinzelpreis = angeboteneArtikelEntities_not_223.VKFestpreis.HasValue && angeboteneArtikelEntities_not_223.VKFestpreis.Value
						? angeboteneArtikelEntities_not_223.Einzelpreis
						: ((angeboteneArtikelEntities_not_223.Einzelpreis / angeboteneArtikelEntities_not_223.Preiseinheit) - angeboteneArtikelEntities_not_223.Einzelkupferzuschlag) * angeboteneArtikelEntities_not_223.Preiseinheit;

					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(angeboteneArtikelEntities_not_223, connection, transaction);
				}


				deliveryNoteEntity.Gebucht = true;
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(deliveryNoteEntity, connection, transaction);
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
