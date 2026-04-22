using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateArtikelHandler
	{
		private Models.Budget.ArtikelBudgetModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateArtikelHandler(Models.Budget.ArtikelBudgetModel data, Identity.Models.UserModel user)
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


				var ArtikelEntity = new Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity()
				{
					aktiv = _data.aktiv,
					aktualisiert = _data.aktualisiert,
					Anfangsbestand = _data.Anfangsbestand,
					Artikel_aus_eigener_Produktion = _data.Artikel_aus_eigener_Produktion,
					Artikel_für_weitere_Bestellungen_sperren = _data.Artikel_fur_weitere_Bestellungen_sperren,
					Artikelfamilie_Kunde = _data.Artikelfamilie_Kunde,
					Artikelfamilie_Kunde_Detail1 = _data.Artikelfamilie_Kunde_Detail1,
					Artikelfamilie_Kunde_Detail2 = _data.Artikelfamilie_Kunde_Detail2,
					Artikelkurztext = _data.Artikelkurztext,
					Artikel_Nr = _data.Artikel_Nr,
					Artikelnummer = _data.Artikelnummer,
					Barverkauf = _data.Barverkauf,
					Bezeichnung_1 = _data.Bezeichnung_1,
					Bezeichnung_2 = _data.Bezeichnung_2,
					Bezeichnung_3 = _data.Bezeichnung_3,
					Crossreferenz = _data.Crossreferenz,
					Cu_Gewicht = _data.Cu_Gewicht,
					Datum_Anfangsbestand = _data.Datum_Anfangsbestand,
					DEL = _data.DEL,
					DEL_fixiert = _data.DEL_fixiert,
					Dokumente = _data.Dokumente,
					EAN = _data.EAN,
					Einheit = _data.Einheit,
					Ersatzartikel = _data.Ersatzartikel,
					ESD_Schutz = _data.ESD_Schutz,
					fakturieren_Stückliste = _data.fakturieren_Stuckliste,
					Farbe = _data.Farbe,
					fibu_rahmen = _data.fibu_rahmen,
					Freigabestatus = _data.Freigabestatus,
					Freigabestatus_TN_intern = _data.Freigabestatus_TN_intern,
					Gebinde = _data.Gebinde,
					Gewicht = _data.Gewicht,
					Größe = _data.Grosse,
					Grund_für_Sperre = _data.Grund_fur_Sperre,
					gültig_bis = _data.gultig_bis,
					Halle = _data.Halle,
					Index_Kunde = _data.Index_Kunde,
					Index_Kunde_Datum = _data.Index_Kunde_Datum,
					Kategorie = _data.Kategorie,
					Kriterium1 = _data.Kriterium1,
					Kriterium2 = _data.Kriterium2,
					Kriterium3 = _data.Kriterium3,
					Kriterium4 = _data.Kriterium4,
					Kupferbasis = _data.Kupferbasis,
					Kupferzahl = _data.Kupferzahl,
					Lagerartikel = _data.Lagerartikel,
					Lagerhaltungskosten = _data.Lagerhaltungskosten,
					Langtext = _data.Langtext,
					Langtext_drucken_AB = _data.Langtext_drucken_AB,
					Langtext_drucken_BW = _data.Langtext_drucken_BW,
					Materialkosten_Alt = _data.Materialkosten_Alt,
					Preiseinheit = _data.Preiseinheit,
					pro_Zeiteinheit = _data.pro_Zeiteinheit,
					Produktionszeit = _data.Produktionszeit,
					Provisionsartikel = _data.Provisionsartikel,
					Prüfstatus_TN_Ware = _data.Prufstatus_TN_Ware,
					Rabattierfähig = _data.Rabattierfahig,
					Rahmen = _data.Rahmen,
					Rahmenauslauf = _data.Rahmenauslauf,
					Rahmenmenge = _data.Rahmenmenge,
					Rahmen_Nr = _data.Rahmen_Nr,
					Seriennummer = _data.Seriennummer,
					Seriennummernverwaltung = _data.Seriennummernverwaltung,
					Sonderrabatt = _data.Sonderrabatt,
					Standard_Lagerort_id = _data.Standard_Lagerort_id,
					Stückliste = _data.Stuckliste,
					//public money? Stundensatz   = _data.  ,
					Stundensatz = _data.Stundensatz,
					Sysmonummer = _data.Sysmonummer,
					UL_Etikett = _data.UL_Etikett,
					UL_zertifiziert = _data.UL_zertifiziert,
					Umsatzsteuer = _data.Umsatzsteuer > 1 ? _data.Umsatzsteuer / 100 : _data.Umsatzsteuer,
					Ursprungsland = _data.Ursprungsland,
					Verpackung = _data.Verpackung,
					VK_Festpreis = _data.VK_Festpreis,
					Volumen = _data.Volumen,
					Warengruppe = _data.Warengruppe,
					Webshop = _data.Webshop,
					Werkzeug = _data.Werkzeug,
					Wert_Anfangsbestand = _data.Wert_Anfangsbestand,
					Zeichnungsnummer = _data.Zeichnungsnummer,
					Zolltarif_nr = _data.Zolltarif_nr,
				};
				var InsertedArtikel = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.InsertArtikel(ArtikelEntity);

				var bestellnummerEntities = this._data.ToBestellnummernEntities();
				if(bestellnummerEntities?.Count > 0)
				{
					for(int i = 0; i < bestellnummerEntities.Count; i++)
					{
						bestellnummerEntities[i].Artikel_Nr = InsertedArtikel;
					}
					Infrastructure.Data.Access.Tables.FNC.BestellnummernAccess.Insert(bestellnummerEntities);
				}
				return ResponseModel<int>.SuccessResponse(InsertedArtikel);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.ConfigCreateArtikel)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
