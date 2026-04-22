using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class GetDeliveryNoteSingleItemHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.DeliveryNote.ItemModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetDeliveryNoteSingleItemHandler(Identity.Models.UserModel user, int data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<Models.DeliveryNote.ItemModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var angebotItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data);
				var angebotEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(angebotItemEntity.AngebotNr ?? -1);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)angebotItemEntity.ArtikelNr);
				var orderItemExtensionEntities = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderId(angebotItemEntity.AngebotNr ?? -1);
				var orderItemsExtensionentity = orderItemExtensionEntities.Find(e => e.OrderItemId == angebotItemEntity.Nr);
				var lagerEntities = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get();
				var lagerEntity = lagerEntities?.Find(x => x.LagerortId == angebotItemEntity.Lagerort_id);
				var response = new Models.DeliveryNote.ItemModel
				{
					Id = angebotItemEntity.Nr,
					StorageLocationId = angebotItemEntity.Lagerort_id,
					ItemId = angebotItemEntity.ArtikelNr,
					ItemNumber = articleEntity?.ArtikelNummer,
					Designation1 = angebotItemEntity.Bezeichnung1,
					Designation2 = angebotItemEntity.Bezeichnung2,
					Designation3 = angebotItemEntity.Bezeichnung3,
					OriginalOrderQuantity = angebotItemEntity.OriginalAnzahl,
					DesiredDate = angebotItemEntity.Wunschtermin,
					DeliveredQuantity = angebotItemEntity.Geliefert,
					OpenQuantity_Quantity = angebotItemEntity.Anzahl,
					DeliveryDate = angebotItemEntity.Liefertermin,
					ProductionNumber = angebotItemEntity.Fertigungsnummer,
					AktuelleLiefermenge = angebotItemEntity.Anzahl,
					Done = angebotItemEntity.erledigt_pos ?? false,
					Standardversand = angebotItemEntity.Versandarten_Auswahl,
					Versandatum = angebotItemEntity.Versanddatum_Auswahl,
					termin_eingehalten = angebotItemEntity.termin_eingehalten ?? false,

					DelFixed = angebotItemEntity.DELFixiert,
					FixedTotalPrice = angebotItemEntity.VKFestpreis,
					FixedUnitPrice = angebotItemEntity.EKPreise_Fix,
					//FreeText = angebotItemEntity.Freies_Format_EDI,
					Note1 = angebotItemEntity.Bemerkungsfeld1,
					Note2 = angebotItemEntity.Bemerkungsfeld2,
					FreeText = angebotItemEntity.Freies_Format_EDI,
					ItemTypeId = angebotItemEntity.Typ,
					RP = angebotItemEntity.RP,
					Position = angebotItemEntity.Position,
					//!CS Info
					Versandinfo_von_CS = angebotItemEntity.Versandinfo_von_CS,
					//!Packing
					Packstatus = angebotItemEntity.Packstatus,
					Gepackt_von = angebotItemEntity.Gepackt_von,
					Gepackt_Zeitpunkt = angebotItemEntity.Gepackt_Zeitpunkt,
					Packinfo_von_Lager = angebotItemEntity.Packinfo_von_Lager,
					//!Shipping
					Versandstatus = angebotItemEntity.Versandstatus,
					Versanddienstleister = angebotItemEntity.Versanddienstleister,
					Versandnummer = angebotItemEntity.Versandnummer,
					Versandinfo_von_Lager = angebotItemEntity.Versandinfo_von_Lager,
					UnloadingPoint = angebotItemEntity.Abladestelle, //Abladestelle
																	 //!EDI
					EDI_PREIS_KUNDE = angebotItemEntity.EDI_PREIS_KUNDE,
					EDI_PREISEINHEIT = angebotItemEntity.EDI_PREISEINHEIT,
					UnitPrice = angebotItemEntity.VKEinzelpreis,
					//new (souilmi)
					OrderId = angebotItemEntity.AngebotNr ?? -1,
					Version = orderItemsExtensionentity?.Version ?? -1,
					OrderNumber = angebotItemEntity.AngebotNr.HasValue ? angebotItemEntity.AngebotNr.Value.ToString() : "",
					CreateDate = angebotEntity?.Datum,
					StorageLocationName = lagerEntity?.Lagerort,
					OpenQuantity_CopperSurcharge = angebotItemEntity.Gesamtkupferzuschlag,
					OpenQuantity_CopperWeight = angebotItemEntity.GesamtCuGewicht,
					OpenQuantity_TotalPrice = angebotItemEntity.Gesamtpreis,
					OpenQuantity_UnitPrice = angebotItemEntity.Einzelpreis,
					CopperWeight = angebotItemEntity.EinzelCuGewicht,
					CopperSurcharge = angebotItemEntity.Einzelkupferzuschlag,
					MeasureUnitQualifier = angebotItemEntity.Einheit,
					OriginalOrderAmount = angebotItemEntity.Gesamtpreis,
					CopperBase = angebotItemEntity.Kupferbasis,
					TotalPrice = angebotItemEntity.VKGesamtpreis,
					UnitPriceBasis = angebotItemEntity.Preiseinheit,
					DrawingIndex = articleEntity?.Index_Kunde,
					Discount = angebotItemEntity.Rabatt,
					VAT = angebotItemEntity.USt,
					DelNote = angebotItemEntity.DEL,
					CustomerItemNumber = (angebotEntity != null && angebotEntity.Kunden_Nr.HasValue) ? angebotEntity?.Kunden_Nr.Value.ToString() : "",
					ItemCustomerDescription = angebotEntity?.Vorname_NameFirma,
					CalculatedValue = (angebotItemEntity.Preiseinheit.HasValue && angebotItemEntity.Preiseinheit.Value > 0) ?
								(angebotItemEntity.OriginalAnzahl ?? 0 / angebotItemEntity.Preiseinheit.Value) * angebotItemEntity.Einzelpreis ?? 0 * (1 - angebotItemEntity.Rabatt ?? 0)
								: 0,
					index = -1,
					LS_ZU_AB = angebotItemEntity.LSPoszuABPos,
					Postext = angebotItemEntity.POSTEXT,
					Index_Kunde = angebotItemEntity.Index_Kunde,
					Index_Kunde_Datum = angebotItemEntity.Index_Kunde_Datum,
					CSInterneBemerkung = angebotItemEntity.CSInterneBemerkung,
				};
				return ResponseModel<Models.DeliveryNote.ItemModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.DeliveryNote.ItemModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<Models.DeliveryNote.ItemModel>.AccessDeniedResponse();
			}
			var angebotItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data);
			if(angebotItemEntity == null)
			{
				return new ResponseModel<Models.DeliveryNote.ItemModel>()
				{
					Success = false,
					Errors = new List<ResponseModel<Models.DeliveryNote.ItemModel>.ResponseError>
					{
					   new ResponseModel<Models.DeliveryNote.ItemModel>.ResponseError{Key ="", Value = $"Position not found"}
					}
				};
			}
			return ResponseModel<Models.DeliveryNote.ItemModel>.SuccessResponse();
		}
	}
}
