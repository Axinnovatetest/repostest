using Psz.Core.Apps.Purchase.Models.DeliveryNote;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class UpdateLSSingleItemHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.DeliveryNote.ItemModel _data { get; set; }
		public UpdateLSSingleItemHandler(Identity.Models.UserModel user, Models.DeliveryNote.ItemModel model)
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

				//lock (Locks.DeliveryNotesLock)
				//{
				var LSEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.OrderId);
				var oldLSItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data.Id);
				//updating original Order position
				var LSItemOrderdQuantiy = oldLSItem.Anzahl;
				if(oldLSItem.LSPoszuABPos.HasValue && oldLSItem.LSPoszuABPos.Value != 0 && oldLSItem.LSPoszuABPos.Value != -1)
				{
					var orderItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(oldLSItem.LSPoszuABPos ?? -1);
					if(orderItem == null)
						return new ResponseModel<int>()
						{
							Success = false,
							Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "original AB position not found."}
					}
						};
					var ActaulOrderRest = orderItem.Anzahl + oldLSItem.Anzahl;

					if(this._data.OpenQuantity_Quantity > ActaulOrderRest)
					{
						return new ResponseModel<int>()
						{
							Success = false,
							Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = "requested quantity is bigger then Order rest quantity"}
					}
						};
					}
					var newOrderRestQuantity = ActaulOrderRest - this._data.OpenQuantity_Quantity;
					var newOrderSentQuantity = this._data.OpenQuantity_Quantity;

					orderItem.Anzahl = newOrderRestQuantity;
					orderItem.Geliefert = newOrderSentQuantity;

					var OrderItemToUpdate = (orderItem?.ArtikelNr != 223) ? orderItem : null;
					if(OrderItemToUpdate != null)
					{
						if(!OrderItemToUpdate.Preiseinheit.HasValue || OrderItemToUpdate.Preiseinheit.Value == 0)
						{
							return new ResponseModel<int>()
							{
								Success = false,
								Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"{OrderItemToUpdate.Position}. Preiseinheit: invalid value {OrderItemToUpdate.Preiseinheit.Value}"}
					}
							};
						}
						if(OrderItemToUpdate.erledigt_pos.HasValue && OrderItemToUpdate.erledigt_pos.Value)
							OrderItemToUpdate.erledigt_pos = false;
						OrderItemToUpdate.Gesamtpreis = (OrderItemToUpdate.Anzahl) / OrderItemToUpdate.Preiseinheit * OrderItemToUpdate.Einzelpreis * (1 - OrderItemToUpdate.Rabatt);
						OrderItemToUpdate.erledigt_pos = OrderItemToUpdate.Anzahl > 0 ? false : true;

						// 1.5
						OrderItemToUpdate.Einzelkupferzuschlag = Math.Round((decimal)(((OrderItemToUpdate.DEL * 1.01m) - OrderItemToUpdate.Kupferbasis)
																				  / 100
																				  * (decimal?)OrderItemToUpdate.EinzelCuGewicht), 2);

						// 1.6 
						OrderItemToUpdate.GesamtCuGewicht = OrderItemToUpdate.Anzahl * OrderItemToUpdate.EinzelCuGewicht;
						OrderItemToUpdate.Einzelpreis = OrderItemToUpdate.VKFestpreis.HasValue && OrderItemToUpdate.VKFestpreis.Value == true
							? OrderItemToUpdate.VKEinzelpreis
							: OrderItemToUpdate.Einzelkupferzuschlag * OrderItemToUpdate.Preiseinheit + OrderItemToUpdate.VKEinzelpreis;

						// 1.7
						OrderItemToUpdate.Gesamtpreis = OrderItemToUpdate.Einzelpreis / OrderItemToUpdate.Preiseinheit * OrderItemToUpdate.Anzahl * (1 - OrderItemToUpdate.Rabatt);
						OrderItemToUpdate.Gesamtkupferzuschlag = OrderItemToUpdate.VKFestpreis.HasValue && OrderItemToUpdate.VKFestpreis.Value == true
							? 0
							: OrderItemToUpdate.Anzahl * OrderItemToUpdate.Einzelkupferzuschlag;
						OrderItemToUpdate.VKGesamtpreis = OrderItemToUpdate.Anzahl * OrderItemToUpdate.VKEinzelpreis / OrderItemToUpdate.Preiseinheit;

						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(OrderItemToUpdate);
					}
				}
				//updating delivery position
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ItemNumber);
				var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(articleEntity?.ArtikelNr ?? -1);

				//redo calculation
				var LSItemTocalculate = new UpdateDeliveryItemModel
				{
					ItemNumber = this._data.ItemNumber,
					CopperSurcharge = this._data.CopperSurcharge,
					OpenQuantity_CopperSurcharge = 0m,
					UnitPrice = this._data.UnitPrice ?? 0m,
					TotalPrice = 0m,
					OpenQuantity_UnitPrice = 0m,
					OpenQuantity_TotalPrice = 0m,
					Id = this._data.Id,
					ItemId = this._data.ItemId,
					OrderId = this._data.OrderId,
					OrderNumber = this._data.OrderNumber,
					CreateDate = this._data.CreateDate.ToString(),
					//TODO:--- Param Fields
					ItemTypeId = this._data.ItemTypeId,
					OpenQuantity_Quantity = this._data.OpenQuantity_Quantity,
					//TODO:--- Get fields
					Designation1 = this._data.Designation1,
					Designation2 = this._data.Designation2,
					Designation3 = this._data.Designation3,
					DelNote = this._data.DelNote,
					CustomerItemNumber = this._data.CustomerItemNumber,
					Discount = this._data.Discount,
					VAT = this._data.VAT,
					ItemCustomerDescription = this._data.ItemCustomerDescription,
					//TODO:--- Calculated fields
					OriginalOrderQuantity = this._data.OriginalOrderQuantity,
					OriginalOrderAmount = _data.OriginalOrderAmount,
					CopperBase = _data.CopperBase,
					CopperWeight = _data.CopperWeight,
					CurrentItemPriceCalculationNet = 0m,
					OpenQuantity_CopperWeight = 0m,
					UnitPriceBasis = _data.UnitPriceBasis,
					//TODO:--- Changed fields
					DelieveryId = _data.OrderId,
					Version = _data.Version,
					Position = _data.Position ?? 0,
					StorageLocationId = _data.StorageLocationId,
					StorageLocationName = _data.StorageLocationName,
					DelFixed = _data.DelFixed,
					ChangeType = _data.ItemTypeId,
					DeliveryDate = _data.DeliveryDate,
					DesiredDate = _data.DesiredDate,
					Done = _data.Done,
					FixedUnitPrice = _data.FixedUnitPrice,
					FixedTotalPrice = _data.FixedTotalPrice,
					RP = _data.RP,
					FreeText = _data.FreeText,
					Note1 = _data.Note1,
					Note2 = _data.Note2,
					ProductionNumber = _data.ProductionNumber ?? 0,
					//TODO:--- New fields
					positionViewMode = 1,
					DelNew = false,
					DelUpdate = false,
					index = _data.index,
					//!CS Info
					Versandinfo_von_CS = _data.Versandinfo_von_CS,
					//!Packing
					Packstatus = _data.Packstatus,
					Gepackt_von = _data.Gepackt_von,
					Gepackt_Zeitpunkt = _data.Gepackt_Zeitpunkt.ToString(),
					Packinfo_von_Lager = _data.Packinfo_von_Lager,
					//!Shipping
					Versandstatus = _data.Versandstatus,
					Versanddienstleister = _data.Versanddienstleister,
					Versandnummer = int.TryParse(_data.Versandnummer, out var val) ? val : 0,
					Versandinfo_von_Lager = _data.Versandinfo_von_Lager,
					UnloadingPoint = _data.UnloadingPoint, //Abladestelle
														   //!EDI
					EDI_PREIS_KUNDE = _data.EDI_PREIS_KUNDE,
					EDI_PREISEINHEIT = _data.EDI_PREISEINHEIT,
					//TODO:--- Position Table fields
					originalPosition = _data.Position ?? 0,
					ArticleQuantity = _data.OpenQuantity_Quantity,
					OrderedQuantity = _data.OpenQuantity_Quantity ?? 0,
					DeliveredQuantity = _data.DeliveredQuantity,
					DesiredQuantity = _data.OpenQuantity_Quantity ?? 0,
					DesiredUnitPrice = _data.UnitPrice,
					ApprovedQuantity = _data.OpenQuantity_Quantity ?? 0,
					ApprovedUnitPrice = _data.UnitPrice,
					// ????---- fields
					MeasureUnitQualifier = _data.MeasureUnitQualifier,
					DrawingIndex = _data.DrawingIndex,
					HasPendingChange = true,
					HasPendingCancel = false,
					termin_eingehalten = _data.termin_eingehalten,
					CalculatedValue = 0m,
					Index_Kunde = _data.Index_Kunde,
					Index_Kunde_Datum = _data.Index_Kunde_Datum,

				};
				var LSItemCalculated = Psz.Core.Apps.Purchase.Handlers.DeliveryNote.UpdateDeliveryItemTemporaryHandler.UpdateDeliveryItem(LSItemTocalculate, _user).Body;
				//
				var LSItemToUpdate = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
				{
					Nr = oldLSItem.Nr,
					AngebotNr = this._data.OrderId,
					ArtikelNr = articleEntity.ArtikelNr,
					Bezeichnung1 = this._data.Designation1,//column type in DB is nvarchar(200),
					Bezeichnung2 = this._data.Designation2,
					Bezeichnung3 = this._data.Designation3, //column type in DB is nvarchar(200),
					Einheit = articleEntity.Einheit,
					Anzahl = this._data.OpenQuantity_Quantity,
					OriginalAnzahl = this._data.OriginalOrderQuantity,
					Preisgruppe = itemPricingGroupDb?.Preisgruppe,
					//Bestellnummer = angeboteneArtikelEntity.Bestellnummer,
					Rabatt = this._data.Discount,
					USt = this._data.VAT.HasValue ? this._data.VAT.Value : 0m,
					POSTEXT = this._data.Postext,
					Preiseinheit = LSItemCalculated.UnitPriceBasis ?? 1, // - 2022-05-30 - init to 1 to respect DB Constraint
					Zeichnungsnummer = this._data.DrawingIndex,
					Liefertermin = this._data.DeliveryDate ?? null,
					erledigt_pos = this._data.Done,
					Lagerort_id = this._data.StorageLocationId,
					Wunschtermin = this._data.DesiredDate,
					Fertigungsnummer = this._data.ProductionNumber,
					Geliefert = LSItemCalculated.DeliveredQuantity,
					LSPoszuABPos = oldLSItem.LSPoszuABPos,
					Position = this._data.Position,
					VKFestpreis = LSItemCalculated.FixedTotalPrice,
					EKPreise_Fix = LSItemCalculated.FixedUnitPrice,//to check with Sani
																   //
					Einzelpreis = LSItemCalculated.OpenQuantity_UnitPrice,
					Gesamtpreis = LSItemCalculated.OpenQuantity_TotalPrice,
					//
					DELFixiert = this._data.DelFixed,
					Abladestelle = this._data.UnloadingPoint,
					termin_eingehalten = this._data.termin_eingehalten,
					RP = this._data.RP,
					// R4
					Kupferbasis = int.TryParse(LSItemCalculated.CopperBase.ToString(), out var v) ? v : 0,//int.TryParse(this._data.CopperBase.ToString(), out var val11) ? val11 : 0,
					DEL = int.TryParse(this._data.DelNote.ToString(), out var val22) ? val22 : 0,
					EinzelCuGewicht = LSItemCalculated.CopperWeight,
					GesamtCuGewicht = LSItemCalculated.OpenQuantity_CopperWeight,
					Einzelkupferzuschlag = LSItemCalculated.CopperSurcharge,
					VKGesamtpreis = LSItemCalculated.TotalPrice, //oldLSItem.OriginalAnzahl * oldLSItem.VKEinzelpreis / oldLSItem.Preiseinheit,
					Versandarten_Auswahl = oldLSItem.Versandarten_Auswahl,//this._data.Standardversand,
					Versanddatum_Auswahl = oldLSItem.Versanddatum_Auswahl,//this._data.Versandatum ?? null,
					VKEinzelpreis = LSItemCalculated.UnitPrice,
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
					AnfangLagerBestand = oldLSItem.AnfangLagerBestand,
					AktuelleAnzahl = oldLSItem.AktuelleAnzahl,
					Lagerbewegung = oldLSItem.Lagerbewegung,
					Lagerbewegung_rückgängig = oldLSItem.Lagerbewegung_rückgängig,
					Auswahl = oldLSItem.Auswahl,
					FM_Einzelpreis = oldLSItem.FM_Einzelpreis,
					FM_Gesamtpreis = oldLSItem.FM_Gesamtpreis,
					Summenberechnung = oldLSItem.Summenberechnung,
					EndeLagerBestand = oldLSItem.EndeLagerBestand,
					Preis_ausweisen = oldLSItem.Preis_ausweisen,
					Stückliste = oldLSItem.Stückliste,
					Stückliste_drucken = oldLSItem.Stückliste_drucken,
					Langtext_drucken = oldLSItem.Langtext_drucken,
					Seriennummern_drucken = oldLSItem.Seriennummern_drucken,
					LSPoszuKBPos = oldLSItem.LSPoszuKBPos,
					RAPoszuBVPos = oldLSItem.RAPoszuBVPos,
					KBPoszuBVPos = oldLSItem.KBPoszuBVPos,
					ABPoszuBVPos = oldLSItem.ABPoszuBVPos,
					KBPoszuRAPos = oldLSItem.KBPoszuRAPos,
					ABPoszuRAPos = oldLSItem.ABPoszuRAPos,
					Loschen = oldLSItem.Loschen,
					InBearbeitung = oldLSItem.InBearbeitung,
					RA_OriginalAnzahl = oldLSItem.RA_OriginalAnzahl,
					RA_Abgerufen = oldLSItem.RA_Abgerufen,
					RA_Offen = oldLSItem.RA_Offen,
					Versand_gedruckt = oldLSItem.Versand_gedruckt,
					LS_von_Versand_gedruckt = oldLSItem.LS_von_Versand_gedruckt,
					VDA_gedruckt = oldLSItem.VDA_gedruckt,
					Bemerkungsfeld1 = this._data.Note1,
					Bemerkungsfeld2 = this._data.Note2,
					Freies_Format_EDI = this._data.FreeText,
					Index_Kunde = this._data.Index_Kunde,
					Index_Kunde_Datum = this._data.Index_Kunde_Datum,
					Gesamtkupferzuschlag = LSItemTocalculate.OpenQuantity_CopperSurcharge,
					CSInterneBemerkung = this._data.CSInterneBemerkung,
					Typ = this._data.ItemTypeId,
				};
				var _oldLSItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.Id);
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(LSItemToUpdate);

				//updating lager bestand
				var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { articleEntity.ArtikelNr });
				var lagerEntity = lagerEntities?.Find(x => x.Lagerort_id == this._data.StorageLocationId);
				var oldLagerEntity = lagerEntities?.Find(x => x.Lagerort_id == oldLSItem.Lagerort_id);
				if(oldLSItem.Lagerort_id != this._data.StorageLocationId)
				{
					var newLagerEntity = lagerEntities?.Find(x => x.Lagerort_id == this._data.StorageLocationId);
					oldLagerEntity.Bestand += this._data.OpenQuantity_Quantity;
					Infrastructure.Data.Access.Tables.PRS.LagerAccess.Update(oldLagerEntity);
					newLagerEntity.Bestand -= this._data.OpenQuantity_Quantity;
					Infrastructure.Data.Access.Tables.PRS.LagerAccess.Update(newLagerEntity);
				}
				//

				if(this._data.OpenQuantity_Quantity != oldLSItem.Anzahl)
				{
					lagerEntity.Bestand = (lagerEntity.Bestand + oldLSItem.Anzahl) - this._data.OpenQuantity_Quantity;
					Infrastructure.Data.Access.Tables.PRS.LagerAccess.Update(lagerEntity);
				}
				// - 2022-03-11 track KundenIndex for Lager Bestand
				Core.CustomerService.Helpers.ItemElementHelper.UpdateLagerExtension(this._user,
					new ItemElementHelper.UpdateLagerExtensionModel
					{
						ArticleId = oldLSItem?.ArtikelNr ?? -1,
						OldKundenIndex = oldLSItem?.Index_Kunde,
						NewKundenIndex = this._data.Index_Kunde,
						OldLagerorId = oldLagerEntity.Lagerort_id ?? -1,
						NewLagerorId = this._data.StorageLocationId ?? -1,
						OldBestand = oldLSItem?.Anzahl ?? 0,
						NewBestand = this._data.OpenQuantity_Quantity ?? 0
					});


				var LSItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(LSEntity.Nr);
				var LSItemsArtilesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(LSItemsEntities?.Select(l => l.ArtikelNr ?? -1).ToList() ?? new List<int> { -1 });
				generateDATFile(LSEntity, LSItemsEntities, LSItemsArtilesEntities);
				//}
				//logging
				var _logs = GetLogs(_oldLSItem, LSItemToUpdate);
				if(_logs != null && _logs.Count > 0)
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logs);

				return ResponseModel<int>.SuccessResponse(1);
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
							var lagerExtItem = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndexLager(artikelEntity.ArtikelNr, _data.Index_Kunde, _data.StorageLocationId ?? -1);
							if(lagerItem != null)
							{
								errors.AddRange(validateArticle(this._data, artikelEntity, lagerItem, lagerExtItem)?.Select(x => x.Value)?.ToList() ?? new List<string>());
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
			var orderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.Id);
			if(!Core.CustomerService.Helpers.HorizonsHelper.ArticleIsTechnic(orderItemEntity.ArtikelNr ?? -1))
			{
				var _newDate = _data.DeliveryDate ?? new DateTime(1900, 1, 1);
				var _oldDate = orderItemEntity.Liefertermin ?? new DateTime(1900, 1, 1);
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

			if(Convert.ToDecimal(lagerEntity.Bestand ?? 0) < item.AktuelleLiefermenge)
			{
				var lagerName = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(lagerEntity.Lagerort_id ?? -1);
				errors.Add(new KeyValuePair<string, string>("Invalid quantity", $"Article {artikelEntity.ArtikelNummer}: the quantity of the warehouse [{lagerName?.Lagerort}]  [{lagerEntity.Bestand}] < [{item.AktuelleLiefermenge}]"));
			}

			//if ((lagerExtensionEntity?.Bestand ?? 0) < item.AktuelleLiefermenge)
			//{
			//    var lagerName = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(lagerExtensionEntity?.Lagerort_id??-1);
			//    errors.Add(new KeyValuePair<string, string>("Invalid quantity", $"Article {artikelEntity.ArtikelNummer}:Index {item.Index_Kunde} the quantity of the warehouse [{lagerName?.Lagerort}]  [{lagerExtensionEntity?.Bestand}] < [{item.AktuelleLiefermenge}]"));
			//}

			return errors;
		}
		public List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity _old,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity _new)
		{
			var _object = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_old.AngebotNr ?? -1);
			var _newArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_new.ArtikelNr ?? -1);
			var _oldArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_old.ArtikelNr ?? -1);
			var _oldStorageLocation = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(_old.Lagerort_id ?? -1);
			var _newStorageLocation = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(_new.Lagerort_id ?? -1);
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var _toLog = new LogHelper(_object.Nr, _object.Angebot_Nr ?? -1, int.TryParse(_object.Projekt_Nr, out var val) ? val : 0, _object.Typ, LogHelper.LogType.MODIFICATIONPOS, "CTS", _user);
			if(_old.Kupferbasis != _new.Kupferbasis)
			{
				_logs.Add(_toLog.LogCTS("Kupferbasis", _old.Kupferbasis.ToString(), _new.Kupferbasis.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.DEL != _new.DEL)
			{
				_logs.Add(_toLog.LogCTS("DEL", _old.DEL.ToString(), _new.DEL.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Liefertermin != _new.Liefertermin)
			{
				_logs.Add(_toLog.LogCTS("Liefertermin", _old.Liefertermin.ToString(), _new.Liefertermin.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Bezeichnung1 != _new.Bezeichnung1)
			{
				_logs.Add(_toLog.LogCTS("Bezeichnung1", _old.Bezeichnung1.ToString(), _new.Bezeichnung1.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Bezeichnung2 != _new.Bezeichnung2)
			{
				_logs.Add(_toLog.LogCTS("Bezeichnung2", _old.Bezeichnung2.ToString(), _new.Bezeichnung2.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Wunschtermin != _new.Wunschtermin)
			{
				_logs.Add(_toLog.LogCTS("Wunschtermin", _old.Wunschtermin.ToString(), _new.Wunschtermin.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.EKPreise_Fix.HasValue && _old.EKPreise_Fix != _new.EKPreise_Fix)
			{
				_logs.Add(_toLog.LogCTS("EKPreise_Fix", _old.EKPreise_Fix.ToString(), _new.EKPreise_Fix.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.VKFestpreis != _new.VKFestpreis)
			{
				_logs.Add(_toLog.LogCTS("VKFestpreis", _old.VKFestpreis.ToString(), _new.VKFestpreis.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Freies_Format_EDI != _new.Freies_Format_EDI)
			{
				_logs.Add(_toLog.LogCTS("Freies_Format_EDI", _old.Freies_Format_EDI.ToString(), _new.Freies_Format_EDI.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.ArtikelNr != _newArticle.ArtikelNr)
			{
				_logs.Add(_toLog.LogCTS("Artikel", _oldArticle.ArtikelNummer, _newArticle.ArtikelNummer.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Bemerkungsfeld1 != _new.Bemerkungsfeld1)
			{
				_logs.Add(_toLog.LogCTS("Bemerkungsfeld1", _old.Bemerkungsfeld1.ToString(), _new.Bemerkungsfeld1.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Bemerkungsfeld2 != _new.Bemerkungsfeld2)
			{
				_logs.Add(_toLog.LogCTS("Bemerkungsfeld2", _old.Bemerkungsfeld2.ToString(), _new.Bemerkungsfeld2.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Position != _new.Position)
			{
				_logs.Add(_toLog.LogCTS("Position", _old.Position.ToString(), _new.Position.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Lagerort_id != _new.Lagerort_id)
			{
				_logs.Add(_toLog.LogCTS("Lagerort", _oldStorageLocation?.Lagerort, _newStorageLocation?.Lagerort, _old.Position ?? -1, _old.Nr));
			}
			if(_old.VKEinzelpreis != _new.VKEinzelpreis)
			{
				_logs.Add(_toLog.LogCTS("VKEinzelpreis", _old.VKEinzelpreis.ToString(), _new.VKEinzelpreis.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Preiseinheit != _new.Preiseinheit)
			{
				_logs.Add(_toLog.LogCTS("Preiseinheit", _old.Preiseinheit.ToString(), _new.Preiseinheit.ToString(), _old.Position ?? -1, _old.Nr));
			}
			if(_old.Abladestelle != _new.Abladestelle)
			{
				_logs.Add(_toLog.LogCTS("Abladestelle", _old.Abladestelle, _new.Abladestelle, _old.Position ?? -1, _old.Nr));
			}
			return _logs;
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
