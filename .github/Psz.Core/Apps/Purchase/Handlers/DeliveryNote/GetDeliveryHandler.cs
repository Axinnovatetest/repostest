using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetDeliveryHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.DeliveryNote.CreateResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetDeliveryHandler(Identity.Models.UserModel user, int id)
		{
			_user = user;
			_data = id;
		}
		public ResponseModel<Models.DeliveryNote.CreateResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var angeboteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
				var angeboteArtikelEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data)?
												.FindAll(x => !x.erledigt_pos.HasValue || (x.erledigt_pos.HasValue && x.erledigt_pos == false))?
												.ToList();

				var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(
						angeboteArtikelEntities?
							.Select(x => Convert.ToInt32(x.ArtikelNr ?? 0))?
							.ToList());
				var articleNrs = artikelEntities?.Select(x => x.ArtikelNr)?.ToList();

				var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(articleNrs);

				var errors = new List<KeyValuePair<string, string>>();
				var items = new List<Models.DeliveryNote.ItemModel>();
				//(souilmi)
				var orderItemsIds = angeboteArtikelEntities.Select(e => e.Nr).ToList();
				var orderItemsExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderItemsIds(orderItemsIds);
				var itemsIds = angeboteArtikelEntities.Where(e => e.ArtikelNr.HasValue).Select(e => e.ArtikelNr.Value).ToList();
				var itemsDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(itemsIds);
				if(angeboteArtikelEntities != null && angeboteArtikelEntities.Count > 0)
				{
					foreach(var angeboteneArtikelEntity in angeboteArtikelEntities)
					{
						var artikelEntity = artikelEntities.FirstOrDefault(x => x.ArtikelNr == angeboteneArtikelEntity.ArtikelNr);
						var lagerEntity = lagerEntities.FirstOrDefault(x => x.Artikel_Nr == artikelEntity.ArtikelNr && x.ID == angeboteneArtikelEntity?.Lagerort_id);
						var orderItemsExtensionDb = orderItemsExtensionsDb.Find(e => e.OrderItemId == angeboteneArtikelEntity.Nr);
						var storageLocationDb = angeboteneArtikelEntity.Lagerort_id.HasValue ? Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get((int)angeboteneArtikelEntity.Lagerort_id) : null;
						var check = validateArticle(artikelEntity, angeboteneArtikelEntity, lagerEntity);
						errors.AddRange(validateArticle(artikelEntity, angeboteneArtikelEntity, lagerEntity));
						var itemDb = angeboteneArtikelEntity.ArtikelNr.HasValue
						? itemsDb.Find(e => e.ArtikelNr == angeboteneArtikelEntity.ArtikelNr.Value)
						: null;
						if(check == null || check.Count == 0)
							items.Add(new Models.DeliveryNote.ItemModel
							{
								Id = angeboteneArtikelEntity?.Nr ?? -1,
								StorageLocationId = angeboteneArtikelEntity?.Lagerort_id ?? -1,
								ItemId = artikelEntity?.ArtikelNr ?? -1,
								ItemNumber = artikelEntity?.ArtikelNummer,
								Designation1 = angeboteneArtikelEntity.Bezeichnung1,
								Designation2 = angeboteneArtikelEntity.Bezeichnung2,
								Designation3 = artikelEntity.Bezeichnung3,
								OriginalOrderQuantity = Convert.ToDecimal(angeboteneArtikelEntity.OriginalAnzahl ?? 0),
								DesiredDate = Convert.ToDateTime(angeboteneArtikelEntity.Wunschtermin ?? DateTime.MaxValue),
								DeliveredQuantity = Convert.ToDecimal(angeboteneArtikelEntity.Geliefert ?? 0),
								OpenQuantity_Quantity = Convert.ToDecimal(angeboteneArtikelEntity.Anzahl ?? 0),
								DeliveryDate = angeboteneArtikelEntity.Liefertermin ?? null,
								ProductionNumber = Convert.ToInt32(angeboteneArtikelEntity.Fertigungsnummer ?? 0),
								AktuelleLiefermenge = 0, //>>>>> will be updated from front
								Done = angeboteneArtikelEntity.erledigt_pos ?? false, //>>>>> will be updated from front
								Standardversand = "", //>>>>> will be updated from front
								Versandatum = DateTime.MaxValue, //>>>>> will be updated from front
								termin_eingehalten = angeboteneArtikelEntity.termin_eingehalten ?? false, //>>>>> will be updated from front
																										  //new props (souilmi)
								DelFixed = angeboteneArtikelEntity.DELFixiert ?? false,
								FixedTotalPrice = angeboteneArtikelEntity.VKFestpreis ?? false,
								FixedUnitPrice = angeboteneArtikelEntity.EKPreise_Fix ?? false,
								FreeText = angeboteneArtikelEntity.Freies_Format_EDI,
								Postext = angeboteneArtikelEntity.POSTEXT,
								Note1 = angeboteneArtikelEntity.Bemerkungsfeld1,
								Note2 = angeboteneArtikelEntity.Bemerkungsfeld2,
								ItemTypeId = angeboteneArtikelEntity.Typ,
								RP = angeboteneArtikelEntity.RP ?? false,
								Position = angeboteneArtikelEntity.Position ?? 0,
								//!CS Info
								Versandinfo_von_CS = angeboteneArtikelEntity.Versandinfo_von_CS,
								//!Packing
								Packstatus = angeboteneArtikelEntity.Packstatus ?? false,
								Gepackt_von = angeboteneArtikelEntity.Gepackt_von,
								Gepackt_Zeitpunkt = angeboteneArtikelEntity.Gepackt_Zeitpunkt ?? null,
								Packinfo_von_Lager = angeboteneArtikelEntity.Packinfo_von_Lager,
								//!Shipping
								Versandstatus = angeboteneArtikelEntity.Versandstatus ?? false,
								Versanddienstleister = angeboteneArtikelEntity.Versanddienstleister,
								Versandnummer = angeboteneArtikelEntity.Versandnummer,
								Versandinfo_von_Lager = angeboteneArtikelEntity.Versandinfo_von_Lager,
								UnloadingPoint = angeboteneArtikelEntity.Abladestelle, //Abladestelle
																					   //!EDI                                                      
								EDI_PREIS_KUNDE = angeboteneArtikelEntity.EDI_PREIS_KUNDE,
								EDI_PREISEINHEIT = angeboteneArtikelEntity.EDI_PREISEINHEIT,
								//new (souilmi)
								OrderId = angeboteneArtikelEntity.AngebotNr ?? -1,
								Version = (orderItemsExtensionDb?.Version ?? 0),
								OrderNumber = angeboteneArtikelEntity.AngebotNr.ToString(),
								//CreateDate=
								StorageLocationName = storageLocationDb?.Lagerort,
								OpenQuantity_CopperSurcharge = Convert.ToDecimal(angeboteneArtikelEntity.Gesamtkupferzuschlag ?? 0),
								OpenQuantity_CopperWeight = Convert.ToDecimal(angeboteneArtikelEntity.GesamtCuGewicht ?? 0),
								OpenQuantity_TotalPrice = Convert.ToDecimal(angeboteneArtikelEntity.Gesamtpreis ?? 0),
								OpenQuantity_UnitPrice = Convert.ToDecimal(angeboteneArtikelEntity.Einzelpreis ?? 0),
								CopperWeight = Convert.ToDecimal(angeboteneArtikelEntity.EinzelCuGewicht ?? 0),
								CopperSurcharge = Convert.ToDecimal(angeboteneArtikelEntity.Einzelkupferzuschlag ?? 0),
								MeasureUnitQualifier = itemDb.Einheit,
								OriginalOrderAmount = Convert.ToDecimal(angeboteneArtikelEntity.Gesamtpreis ?? 0),
								CopperBase = angeboteneArtikelEntity.Kupferbasis ?? 0,
								UnitPrice = Convert.ToDecimal(angeboteneArtikelEntity.VKEinzelpreis ?? 0),
								TotalPrice = Convert.ToDecimal(angeboteneArtikelEntity.VKGesamtpreis ?? 0),
								UnitPriceBasis = Convert.ToDecimal(angeboteneArtikelEntity.Preiseinheit ?? 0),
								DrawingIndex = artikelEntity.Index_Kunde,
								Discount = Math.Round(Convert.ToDecimal(angeboteneArtikelEntity.Rabatt ?? 0) * 100, 2),
								VAT = Math.Round(Convert.ToDecimal(angeboteneArtikelEntity.USt ?? 0) * 100, 2),
								DelNote = angeboteneArtikelEntity.DEL ?? 0,
								/*to verify*/
								CustomerItemNumber = angeboteEntity.Kunden_Nr.HasValue ? angeboteEntity.Kunden_Nr.Value.ToString() : null,
								ItemCustomerDescription = angeboteEntity.Vorname_NameFirma,
								CalculatedValue = (angeboteneArtikelEntity.Preiseinheit.HasValue && angeboteneArtikelEntity.Preiseinheit.Value > 0) ?
								(angeboteneArtikelEntity.OriginalAnzahl ?? 0 / angeboteneArtikelEntity.Preiseinheit.Value) * angeboteneArtikelEntity.Einzelpreis ?? 0 * (1 - angeboteneArtikelEntity.Rabatt ?? 0)
								: 0,
								Index_Kunde = angeboteneArtikelEntity.Index_Kunde,
								Index_Kunde_Datum = angeboteneArtikelEntity.Index_Kunde_Datum,
								CSInterneBemerkung = angeboteneArtikelEntity.CSInterneBemerkung,
							});
					}
				}

				//if (errors.Count > 0)
				//    return new ResponseModel<Models.DeliveryNote.CreateResponseModel>
				//    {
				//        Success = false,
				//        Errors = errors.Select(x => new ResponseModel<Models.DeliveryNote.CreateResponseModel>.ResponseError(x)).ToList()
				//    };
				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(angeboteEntity.Kunden_Nr ?? -1);
				var adressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(angeboteEntity.Kunden_Nr ?? 0);
				return ResponseModel<Models.DeliveryNote.CreateResponseModel>.SuccessResponse(
					new Models.DeliveryNote.CreateResponseModel
					{
						Nr = angeboteEntity.Nr,
						AngebotNr = angeboteEntity.Angebot_Nr ?? 0,
						Bezug = angeboteEntity.Bezug,
						KundenNr = adressEntity.Kundennummer ?? 0,
						//Convert.ToInt32(angeboteEntity.Kunden_Nr ?? 0),
						VornameFirma = angeboteEntity.Vorname_NameFirma,
						Standardversand = kundenEntity?.Standardversand,
						//Versandatum = DateTime.Now.AddDays(1),
						Items = items.OrderBy(p => p.Position).ToList(),
						Infos = errors
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.DeliveryNote.CreateResponseModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<Models.DeliveryNote.CreateResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.DeliveryNote.CreateResponseModel>()
				{
					Success = false,
					Errors = new List<ResponseModel<Models.DeliveryNote.CreateResponseModel>.ResponseError>
					{
					   new ResponseModel<Models.DeliveryNote.CreateResponseModel>.ResponseError{Key ="", Value = "Order not found"}
					}
				};
			}

			return ResponseModel<Models.DeliveryNote.CreateResponseModel>.SuccessResponse();
		}

		internal List<KeyValuePair<string, string>> validateArticle(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angeboteneArtikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.LagerEntity lagerEntity)
		{
			var errors = new List<KeyValuePair<string, string>>();

			// 1-, 2-, 3-
			if(/*artikelEntity.FreigabestatusTNIntern.ToLower() == "r" || */artikelEntity.FreigabestatusTNIntern.ToLower() == "b")
			{
				errors.Add(new KeyValuePair<string, string>(
					"Article blocked",
				   $"Delivery is currently not possible! \n\r Artikel {artikelEntity.ArtikelNummer} is locked: Status internal on {artikelEntity.FreigabestatusTNIntern} set!"
				   ));
			}

			//4-
			if(artikelEntity.ArtikelNummer.ToLower() != "reparatur" && artikelEntity.Freigabestatus.ToLower() == "n")
			{
				errors.Add(new KeyValuePair<string, string>(
					"Initial sample blocked",
				   $"Delivery is currently not possible! \n\rExternal status set to N! \n\r Artikelnummer {artikelEntity.ArtikelNummer} is locked!"
				   ));
			}

			//5-
			if(artikelEntity.ArtikelNummer.ToLower() != "reparatur" && artikelEntity.FreigabestatusTNIntern.ToLower() == "n")
			{
				errors.Add(new KeyValuePair<string, string>(
					"Internal Status",
				   $"Delivery is currently not possible,\n\r Artikelnummer {artikelEntity.ArtikelNummer} is blocked: Status Inter set to N!"
				   ));
			}

			return errors;
		}
	}
}
