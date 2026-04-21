using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Supplier
{
	public class CreateHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		private Models.Supplier.UpdateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public CreateHandler(Models.Supplier.UpdateModel data,
			Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
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

				var nextSupplierNummer = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.GetMaxNummber() + 1;

				var addressEntity = new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity()
				{
					Nr = -1,
					Lieferantennummer = nextSupplierNummer,
					Adresstyp = (int)Infrastructure.Data.Access.Tables.FNC.AdressenAccess.AddressType.Supplier, // -_data.AddressType,

					Anrede = _data.AdressPreName,
					Auswahl = _data.AdressSelection,
					Bemerkungen = _data.AdressNotes,
					Briefanrede = _data.AdressSalutation,
					eMail = _data.AdressEmailAdress,
					erfasst = _data.AdressRecordTime,
					Fax = _data.AdressFaxNumber,
					Funktion = _data.AdressFunction,
					Land = _data.AdressCountry,
					Name1 = _data.AdressName1,
					Name2 = _data.AdressName2,
					Name3 = _data.AdressName3,
					Ort = _data.AdressCity,
					PLZ_Postfach = _data.AdressMailboxZipCode,
					PLZ_StraBe = _data.AdressStreetZipCode,
					Postfach = _data.AdressMailbox,
					Postfach_bevorzugt = _data.AdressMailboxIsPreferred,
					Sortierbegriff = _data.AdressSortTerm,
					StraBe = _data.AdressStreet,
					stufe = _data.AdressLevel,
					Telefon = _data.AdressPhoneNumber,
					Titel = _data.AdressTitle,
					von = _data.AdressFrom,
					Vorname = _data.AdressFirstName,
					WWW = _data.AdressWebsite,

					sperren = false,
					Abteilung = string.Empty,
					Kundennummer = null,
					Personalnummer = null,
				};
				var insertedAddressId = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Insert(addressEntity);

				// var konditionsId = Infrastructure.Data.Access.Tables.FNC.KonditionsZuordnungstabelleEntity.Insert(_data.ToKonditionsEntity());

				var supplierEntity = new Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity()
				{
					Nr = -1,
					Nummer = insertedAddressId,

					Belegkreis = _data.SlipCircleId,
					Bestellbestatigung_anmahnen = _data.RemindOrderConfirmation,
					Bestellimit = _data.OrderLimit.HasValue ? (double)_data.OrderLimit.Value : (double?)null,
					Branche = _data.Industry,
					EG_Identifikationsnummer = _data.EgIdentificationNumber,
					Eilzuschlag = _data.ShippingExpressSurcharge.HasValue ? (double)_data.ShippingExpressSurcharge.Value : (double?)null,
					Frachtfreigrenze = _data.ShippingFreightAllowance.HasValue ? (double)_data.ShippingFreightAllowance.Value : (double?)null,
					Gesperrt_fur_weitere_Bestellungen = _data.BlockedForFurtherOrders,
					Grund_fur_Sperre = _data.ReasonForBlocking,
					Karenztage = _data.WaitingPeriod,
					Konditionszuordnungs_Nr = _data.ConditionAssignmentNumber ?? 1,
					Kreditoren_Nr = string.Empty,
					Kundennummer_Lieferanten = string.Empty,
					Kundennummer_PSZ_AL_Lieferanten = string.Empty,
					Kundennummer_PSZ_CZ_Lieferanten = string.Empty,
					Kundennummer_PSZ_TN_Lieferanten = string.Empty,
					Kundennummer_SC_CZ_Lieferanten = string.Empty,
					Kundennummer_SC_Lieferanten = string.Empty,
					LH = false,
					LH_Datum = null,
					Lieferantengruppe = _data.SuppliersGroup,
					Mahnsperre = _data.Dunning,
					Mahnsperre_Lieferant = _data.DunningBlockSupplier,
					Mindestbestellwert = _data.MinimumValue.HasValue ? (double)_data.MinimumValue.Value : (double?)null,
					Rabattgruppe = _data.DiscountGroupId,
					Sprache = _data.LanguageId,
					Umsatzsteuer_berechnen = _data.CalculateSalesTax,
					Versandart = _data.ShippingMethod,
					Versandkosten = _data.ShippingCosts.HasValue ? (double)_data.ShippingCosts.Value : (double?)null,
					Wahrung = _data.CurrencyId,
					Wochentag_Anlieferung = _data.ShippingWeekDay,
					Zahlungsweise = _data.PaymentMethod,
					Zielaufschlag = _data.TargetImpact.HasValue ? (double)_data.TargetImpact.Value : (double?)null,
					Zuschlag_Mindestbestellwert = _data.SurchargeMinimumOrderValue.HasValue ? (double)_data.SurchargeMinimumOrderValue.Value : (double?)null,
				};
				var insertedSupplierId = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Insert(supplierEntity);

				// - 
				addDiverseArticle(insertedAddressId);

				if(_data.ContactPersons != null && _data.ContactPersons.Count > 0)
				{
					foreach(var contactPerson in _data.ContactPersons)
					{
						Infrastructure.Data.Access.Tables.FNC.AnsprechpartnerAccess.Insert(contactPerson.ToDataEntity(insertedAddressId));
					}
				}

				return ResponseModel<int>.SuccessResponse(insertedSupplierId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var errors = new List<ResponseModel<int>.ResponseError>();

			if(string.IsNullOrEmpty(_data.AdressName1))
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Address Name 1 is empty" });
			}
			else
			{
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(_data.AdressName1.Trim(), _data.AddressType);
				if(addressEntity != null)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Address Name 1 with the same Address Type is already used" });
				}
			}

			if(_data.AddressEDIActive.HasValue && Convert.ToBoolean(_data.AddressEDIActive))
			{
				if(!_data.AdressDUNS.HasValue || !int.TryParse(_data.AdressDUNS.ToString(), out int duns))
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Invalid value for Address DUNS" });
				}
			}

			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}

			return ResponseModel<int>.SuccessResponse();
		}

		public static void addDiverseArticle(int supplierId)
		{

			var firstDiverse = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.GetFirstDiverse();
			if(firstDiverse == null || firstDiverse.Artikel_Nr < 0)
			{
				var ArtikelEntity = new Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity()
				{
					aktiv = true,
					aktualisiert = DateTime.Now,
					Anfangsbestand = null,
					Artikel_aus_eigener_Produktion = false,
					Artikel_für_weitere_Bestellungen_sperren = false,
					Artikelfamilie_Kunde = null,
					Artikelfamilie_Kunde_Detail1 = null,
					Artikelfamilie_Kunde_Detail2 = null,
					Artikelkurztext = null,
					Artikel_Nr = -1,
					Artikelnummer = "Diverse",
					Barverkauf = false,
					Bezeichnung_1 = "Diverse",
					Bezeichnung_2 = "Diverse",
					Bezeichnung_3 = null,
					Crossreferenz = null,
					Cu_Gewicht = null,
					Datum_Anfangsbestand = null,
					DEL = null,
					DEL_fixiert = false,
					Dokumente = null,
					EAN = null,
					Einheit = "St",
					Ersatzartikel = null,
					ESD_Schutz = false,
					fakturieren_Stückliste = false,
					Farbe = null,
					fibu_rahmen = null,
					Freigabestatus = null,
					Freigabestatus_TN_intern = null,
					Gebinde = null,
					Gewicht = null,
					Größe = null,
					Grund_für_Sperre = null,
					gültig_bis = null,
					Halle = null,
					Index_Kunde = null,
					Index_Kunde_Datum = null,
					Kategorie = null,
					Kriterium1 = null,
					Kriterium2 = null,
					Kriterium3 = null,
					Kriterium4 = null,
					Kupferbasis = null,
					Kupferzahl = null,
					Lagerartikel = false,
					Lagerhaltungskosten = null,
					Langtext = null,
					Langtext_drucken_AB = false,
					Langtext_drucken_BW = false,
					Materialkosten_Alt = null,
					Preiseinheit = 1,
					pro_Zeiteinheit = null,
					Produktionszeit = null,
					Provisionsartikel = false,
					Prüfstatus_TN_Ware = null,
					Rabattierfähig = false,
					Rahmen = false,
					Rahmenauslauf = null,
					Rahmenmenge = null,
					Rahmen_Nr = null,
					Seriennummer = null,
					Seriennummernverwaltung = false,
					Sonderrabatt = null,
					Standard_Lagerort_id = null,
					Stückliste = false,
					//public money? Stundensatz   = _data.  ,
					Stundensatz = null,
					Sysmonummer = null,
					UL_Etikett = false,
					UL_zertifiziert = false,
					Umsatzsteuer = 0.19f,
					Ursprungsland = null,
					Verpackung = null,
					VK_Festpreis = false,
					Volumen = null,
					Warengruppe = null,
					Webshop = false,
					Werkzeug = null,
					Wert_Anfangsbestand = null,
					Zeichnungsnummer = null,
					Zolltarif_nr = null,
				};
				var InsertedArtikel = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.InsertArtikel(ArtikelEntity);
				firstDiverse = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(InsertedArtikel);
			}

			var bestellnummerEntities = new Infrastructure.Data.Entities.Tables.FNC.BestellnummernEntity
			{
				Artikel_Nr = firstDiverse?.Artikel_Nr ?? -1,
				Lieferanten_Nr = supplierId,
				Artikelbezeichnung = "Diverse",
				Artikelbezeichnung2 = "Diverse",
				Standardlieferant = false,
				Bestell_Nr = "Diverse",
				Einkaufspreis = 1,
				//Symbol = x.Symbol,
				Umsatzsteuer = 0.19m,
				Rabatt = 0,
				Preiseinheit = 1,
				Wiederbeschaffungszeitraum = 0,
				Prüftiefe_WE = 0
			};
			Infrastructure.Data.Access.Tables.FNC.BestellnummernAccess.Insert(bestellnummerEntities);
		}
	}
}
