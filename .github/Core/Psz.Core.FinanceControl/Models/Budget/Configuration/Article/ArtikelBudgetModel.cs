using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class ArtikelBudgetModel
	{
		public List<Supplier> suppliers { get; set; }
		public bool aktiv { get; set; }
		public DateTime? aktualisiert { get; set; }
		public double? Anfangsbestand { get; set; }
		public bool Artikel_aus_eigener_Produktion { get; set; }
		public bool Artikel_fur_weitere_Bestellungen_sperren { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public string Artikelfamilie_Kunde_Detail1 { get; set; }
		public string Artikelfamilie_Kunde_Detail2 { get; set; }
		public string Artikelkurztext { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public bool Barverkauf { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Bezeichnung_3 { get; set; }
		public string Crossreferenz { get; set; }
		public double? Cu_Gewicht { get; set; }
		public DateTime? Datum_Anfangsbestand { get; set; }
		public int? DEL { get; set; }
		public bool DEL_fixiert { get; set; }
		public string Dokumente { get; set; }
		public string EAN { get; set; }
		public string Einheit { get; set; }
		public int? Ersatzartikel { get; set; }
		public bool ESD_Schutz { get; set; }
		public bool fakturieren_Stuckliste { get; set; }
		public string Farbe { get; set; }
		public int? fibu_rahmen { get; set; }
		public string Freigabestatus { get; set; }
		public string Freigabestatus_TN_intern { get; set; }
		public string Gebinde { get; set; }
		public double? Gewicht { get; set; }
		public double? Grosse { get; set; }
		public string Grund_fur_Sperre { get; set; }
		public DateTime? gultig_bis { get; set; }
		public string Halle { get; set; }
		public string Index_Kunde { get; set; }
		public string Index_Kunde_Datum { get; set; }
		public string Kategorie { get; set; }
		public string Kriterium1 { get; set; }
		public string Kriterium2 { get; set; }
		public string Kriterium3 { get; set; }
		public string Kriterium4 { get; set; }
		public int? Kupferbasis { get; set; }
		public double? Kupferzahl { get; set; }
		public bool Lagerartikel { get; set; }
		public double? Lagerhaltungskosten { get; set; }
		public string Langtext { get; set; }
		public bool Langtext_drucken_AB { get; set; }
		public bool Langtext_drucken_BW { get; set; }
		public double? Materialkosten_Alt { get; set; }
		public double Preiseinheit { get; set; }
		public string pro_Zeiteinheit { get; set; }
		public double? Produktionszeit { get; set; }
		public bool Provisionsartikel { get; set; }
		public string Prufstatus_TN_Ware { get; set; }
		public bool Rabattierfahig { get; set; }
		public bool Rahmen { get; set; }
		public DateTime? Rahmenauslauf { get; set; }
		public double? Rahmenmenge { get; set; }
		public string Rahmen_Nr { get; set; }
		public string Seriennummer { get; set; }
		public bool Seriennummernverwaltung { get; set; }
		public double? Sonderrabatt { get; set; }
		public int? Standard_Lagerort_id { get; set; }
		public bool Stuckliste { get; set; }
		//public money? Stundensatz { get; set; }
		public double? Stundensatz { get; set; }
		public string Sysmonummer { get; set; }
		public bool UL_Etikett { get; set; }
		public bool UL_zertifiziert { get; set; }
		public double Umsatzsteuer { get; set; }
		public string Ursprungsland { get; set; }
		public string Verpackung { get; set; }
		public bool VK_Festpreis { get; set; }
		public string Volumen { get; set; }
		public string Warengruppe { get; set; }
		public bool Webshop { get; set; }
		public string Werkzeug { get; set; }
		public double? Wert_Anfangsbestand { get; set; }
		public string Zeichnungsnummer { get; set; }
		public string Zolltarif_nr { get; set; }

		// - Bestellnumern
		public string Angebot { get; set; }
		public string Angebot_Datum { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		//public double? Artikel_Nr { get; set; }
		public double? Basispreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Bestell_Nr { get; set; }
		public double? Einkaufspreis { get; set; }
		public string Einkaufspreis_gultig_bis { get; set; }
		public double? EK_EUR { get; set; }
		public double? EK_total { get; set; }
		public double? Fracht { get; set; }
		public DateTime? letzte_Aktualisierung { get; set; }
		public double? Lieferanten_Nr { get; set; }
		public double? Logistik { get; set; }
		public double? Mindestbestellmenge { get; set; }
		public int Nr { get; set; }
		//public double? Preiseinheit { get; set; }
		public double? Pruftiefe_WE { get; set; }
		public double? Rabatt { get; set; }
		public bool Standardlieferant { get; set; }
		//public double? Umsatzsteuer { get; set; }
		public double? Verpackungseinheit { get; set; }
		//public string Warengruppe { get; set; }
		public double? Wiederbeschaffungszeitraum { get; set; }
		public double? Zoll { get; set; }
		public double? Zusatz { get; set; }
		public ArtikelBudgetModel() { }

		public ArtikelBudgetModel(Infrastructure.Data.Entities.Tables.FNC.ArtikelBestellnummer ArtikelEntity)
		{
			aktiv = ArtikelEntity.aktiv;
			aktualisiert = ArtikelEntity.aktualisiert;
			Anfangsbestand = ArtikelEntity.Anfangsbestand;
			Artikel_aus_eigener_Produktion = ArtikelEntity.Artikel_aus_eigener_Produktion;
			Artikel_fur_weitere_Bestellungen_sperren = ArtikelEntity.Artikel_für_weitere_Bestellungen_sperren;
			Artikelfamilie_Kunde = ArtikelEntity.Artikelfamilie_Kunde;
			Artikelfamilie_Kunde_Detail1 = ArtikelEntity.Artikelfamilie_Kunde_Detail1;
			Artikelfamilie_Kunde_Detail2 = ArtikelEntity.Artikelfamilie_Kunde_Detail2;
			Artikelkurztext = ArtikelEntity.Artikelkurztext;
			Artikel_Nr = ArtikelEntity.Artikel_Nr;
			Artikelnummer = ArtikelEntity.Artikelnummer;
			Barverkauf = ArtikelEntity.Barverkauf;
			Bezeichnung_1 = ArtikelEntity.Bezeichnung_1;
			Bezeichnung_2 = ArtikelEntity.Bezeichnung_2;
			Bezeichnung_3 = ArtikelEntity.Bezeichnung_3;
			Crossreferenz = ArtikelEntity.Crossreferenz;
			Cu_Gewicht = ArtikelEntity.Cu_Gewicht;
			Datum_Anfangsbestand = ArtikelEntity.Datum_Anfangsbestand;
			DEL = ArtikelEntity.DEL;
			DEL_fixiert = ArtikelEntity.DEL_fixiert;
			Dokumente = ArtikelEntity.Dokumente;
			EAN = ArtikelEntity.EAN;
			Einheit = ArtikelEntity.Einheit;
			Ersatzartikel = ArtikelEntity.Ersatzartikel;
			ESD_Schutz = ArtikelEntity.ESD_Schutz;
			fakturieren_Stuckliste = ArtikelEntity.fakturieren_Stückliste;
			Farbe = ArtikelEntity.Farbe;
			fibu_rahmen = ArtikelEntity.fibu_rahmen;
			Freigabestatus = ArtikelEntity.Freigabestatus;
			Freigabestatus_TN_intern = ArtikelEntity.Freigabestatus_TN_intern;
			Gebinde = ArtikelEntity.Gebinde;
			Gewicht = ArtikelEntity.Gewicht;
			Grosse = ArtikelEntity.Größe;
			Grund_fur_Sperre = ArtikelEntity.Grund_für_Sperre;
			gultig_bis = ArtikelEntity.gültig_bis;
			Halle = ArtikelEntity.Halle;
			Index_Kunde = ArtikelEntity.Index_Kunde;
			Index_Kunde_Datum = ArtikelEntity.Index_Kunde_Datum;
			Kategorie = ArtikelEntity.Kategorie;
			Kriterium1 = ArtikelEntity.Kriterium1;
			Kriterium2 = ArtikelEntity.Kriterium2;
			Kriterium3 = ArtikelEntity.Kriterium3;
			Kriterium4 = ArtikelEntity.Kriterium4;
			Kupferbasis = ArtikelEntity.Kupferbasis;
			Kupferzahl = ArtikelEntity.Kupferzahl;
			Lagerartikel = ArtikelEntity.Lagerartikel;
			Lagerhaltungskosten = ArtikelEntity.Lagerhaltungskosten;
			Langtext = ArtikelEntity.Langtext;
			Langtext_drucken_AB = ArtikelEntity.Langtext_drucken_AB;
			Langtext_drucken_BW = ArtikelEntity.Langtext_drucken_BW;
			Materialkosten_Alt = ArtikelEntity.Materialkosten_Alt;
			Preiseinheit = ArtikelEntity.Preiseinheit;
			pro_Zeiteinheit = ArtikelEntity.pro_Zeiteinheit;
			Produktionszeit = ArtikelEntity.Produktionszeit;
			Provisionsartikel = ArtikelEntity.Provisionsartikel;
			Prufstatus_TN_Ware = ArtikelEntity.Prüfstatus_TN_Ware;
			Rabattierfahig = ArtikelEntity.Rabattierfähig;
			Rahmen = ArtikelEntity.Rahmen;
			Rahmenauslauf = ArtikelEntity.Rahmenauslauf;
			Rahmenmenge = ArtikelEntity.Rahmenmenge;
			Rahmen_Nr = ArtikelEntity.Rahmen_Nr;
			Seriennummer = ArtikelEntity.Seriennummer;
			Seriennummernverwaltung = ArtikelEntity.Seriennummernverwaltung;
			Sonderrabatt = ArtikelEntity.Sonderrabatt;
			Standard_Lagerort_id = ArtikelEntity.Standard_Lagerort_id;
			Stuckliste = ArtikelEntity.Stückliste;
			//public money? Stundensatz   = ArtikelEntity. ;
			Stundensatz = ArtikelEntity.Stundensatz;
			Sysmonummer = ArtikelEntity.Sysmonummer;
			UL_Etikett = ArtikelEntity.UL_Etikett;
			UL_zertifiziert = ArtikelEntity.UL_zertifiziert;
			Umsatzsteuer = ArtikelEntity.Umsatzsteuer;
			Ursprungsland = ArtikelEntity.Ursprungsland;
			Verpackung = ArtikelEntity.Verpackung;
			VK_Festpreis = ArtikelEntity.VK_Festpreis;
			Volumen = ArtikelEntity.Volumen;
			Warengruppe = ArtikelEntity.Warengruppe;
			Webshop = ArtikelEntity.Webshop;
			Werkzeug = ArtikelEntity.Werkzeug;
			Wert_Anfangsbestand = ArtikelEntity.Wert_Anfangsbestand;
			Zeichnungsnummer = ArtikelEntity.Zeichnungsnummer;
			Zolltarif_nr = ArtikelEntity.Zolltarif_nr;


			// - Bestellnumern
			Angebot = ArtikelEntity.Angebot;
			Angebot_Datum = ArtikelEntity.Angebot_Datum;
			Artikelbezeichnung = ArtikelEntity.Artikelbezeichnung;
			Artikelbezeichnung2 = ArtikelEntity.Artikelbezeichnung2;
			Basispreis = ArtikelEntity.Basispreis;
			Bemerkungen = ArtikelEntity.Bemerkungen;
			Bestell_Nr = ArtikelEntity.Bestell_Nr;
			Einkaufspreis = ArtikelEntity.Einkaufspreis;
			Einkaufspreis_gultig_bis = ArtikelEntity.Einkaufspreis_gültig_bis;
			EK_EUR = ArtikelEntity.EK_EUR;
			EK_total = ArtikelEntity.EK_total;
			Fracht = ArtikelEntity.Fracht;
			letzte_Aktualisierung = ArtikelEntity.letzte_Aktualisierung;
			Lieferanten_Nr = ArtikelEntity.Lieferanten_Nr;
			Logistik = ArtikelEntity.Logistik;
			Mindestbestellmenge = ArtikelEntity.Mindestbestellmenge;
			Nr = ArtikelEntity.Nr;
			Pruftiefe_WE = ArtikelEntity.Prüftiefe_WE;
			Rabatt = ArtikelEntity.Rabatt;
			Standardlieferant = ArtikelEntity.Standardlieferant;
			Verpackungseinheit = ArtikelEntity.Verpackungseinheit;
			Wiederbeschaffungszeitraum = ArtikelEntity.Wiederbeschaffungszeitraum;
			Zoll = ArtikelEntity.Zoll;
			Zusatz = ArtikelEntity.Zusatz;
		}
		public ArtikelBudgetModel(Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity ArtikelEntity)
		{
			aktiv = ArtikelEntity.aktiv;
			aktualisiert = ArtikelEntity.aktualisiert;
			Anfangsbestand = ArtikelEntity.Anfangsbestand;
			Artikel_aus_eigener_Produktion = ArtikelEntity.Artikel_aus_eigener_Produktion;
			Artikel_fur_weitere_Bestellungen_sperren = ArtikelEntity.Artikel_für_weitere_Bestellungen_sperren;
			Artikelfamilie_Kunde = ArtikelEntity.Artikelfamilie_Kunde;
			Artikelfamilie_Kunde_Detail1 = ArtikelEntity.Artikelfamilie_Kunde_Detail1;
			Artikelfamilie_Kunde_Detail2 = ArtikelEntity.Artikelfamilie_Kunde_Detail2;
			Artikelkurztext = ArtikelEntity.Artikelkurztext;
			Artikel_Nr = ArtikelEntity.Artikel_Nr;
			Artikelnummer = ArtikelEntity.Artikelnummer;
			Barverkauf = ArtikelEntity.Barverkauf;
			Bezeichnung_1 = ArtikelEntity.Bezeichnung_1;
			Bezeichnung_2 = ArtikelEntity.Bezeichnung_2;
			Bezeichnung_3 = ArtikelEntity.Bezeichnung_3;
			Crossreferenz = ArtikelEntity.Crossreferenz;
			Cu_Gewicht = ArtikelEntity.Cu_Gewicht;
			Datum_Anfangsbestand = ArtikelEntity.Datum_Anfangsbestand;
			DEL = ArtikelEntity.DEL;
			DEL_fixiert = ArtikelEntity.DEL_fixiert;
			Dokumente = ArtikelEntity.Dokumente;
			EAN = ArtikelEntity.EAN;
			Einheit = ArtikelEntity.Einheit;
			Ersatzartikel = ArtikelEntity.Ersatzartikel;
			ESD_Schutz = ArtikelEntity.ESD_Schutz;
			fakturieren_Stuckliste = ArtikelEntity.fakturieren_Stückliste;
			Farbe = ArtikelEntity.Farbe;
			fibu_rahmen = ArtikelEntity.fibu_rahmen;
			Freigabestatus = ArtikelEntity.Freigabestatus;
			Freigabestatus_TN_intern = ArtikelEntity.Freigabestatus_TN_intern;
			Gebinde = ArtikelEntity.Gebinde;
			Gewicht = ArtikelEntity.Gewicht;
			Grosse = ArtikelEntity.Größe;
			Grund_fur_Sperre = ArtikelEntity.Grund_für_Sperre;
			gultig_bis = ArtikelEntity.gültig_bis;
			Halle = ArtikelEntity.Halle;
			Index_Kunde = ArtikelEntity.Index_Kunde;
			Index_Kunde_Datum = ArtikelEntity.Index_Kunde_Datum;
			Kategorie = ArtikelEntity.Kategorie;
			Kriterium1 = ArtikelEntity.Kriterium1;
			Kriterium2 = ArtikelEntity.Kriterium2;
			Kriterium3 = ArtikelEntity.Kriterium3;
			Kriterium4 = ArtikelEntity.Kriterium4;
			Kupferbasis = ArtikelEntity.Kupferbasis;
			Kupferzahl = ArtikelEntity.Kupferzahl;
			Lagerartikel = ArtikelEntity.Lagerartikel;
			Lagerhaltungskosten = ArtikelEntity.Lagerhaltungskosten;
			Langtext = ArtikelEntity.Langtext;
			Langtext_drucken_AB = ArtikelEntity.Langtext_drucken_AB;
			Langtext_drucken_BW = ArtikelEntity.Langtext_drucken_BW;
			Materialkosten_Alt = ArtikelEntity.Materialkosten_Alt;
			Preiseinheit = ArtikelEntity.Preiseinheit;
			pro_Zeiteinheit = ArtikelEntity.pro_Zeiteinheit;
			Produktionszeit = ArtikelEntity.Produktionszeit;
			Provisionsartikel = ArtikelEntity.Provisionsartikel;
			Prufstatus_TN_Ware = ArtikelEntity.Prüfstatus_TN_Ware;
			Rabattierfahig = ArtikelEntity.Rabattierfähig;
			Rahmen = ArtikelEntity.Rahmen;
			Rahmenauslauf = ArtikelEntity.Rahmenauslauf;
			Rahmenmenge = ArtikelEntity.Rahmenmenge;
			Rahmen_Nr = ArtikelEntity.Rahmen_Nr;
			Seriennummer = ArtikelEntity.Seriennummer;
			Seriennummernverwaltung = ArtikelEntity.Seriennummernverwaltung;
			Sonderrabatt = ArtikelEntity.Sonderrabatt;
			Standard_Lagerort_id = ArtikelEntity.Standard_Lagerort_id;
			Stuckliste = ArtikelEntity.Stückliste;
			//public money? Stundensatz   = ArtikelEntity. ;
			Stundensatz = ArtikelEntity.Stundensatz;
			Sysmonummer = ArtikelEntity.Sysmonummer;
			UL_Etikett = ArtikelEntity.UL_Etikett;
			UL_zertifiziert = ArtikelEntity.UL_zertifiziert;
			Umsatzsteuer = ArtikelEntity.Umsatzsteuer;
			Ursprungsland = ArtikelEntity.Ursprungsland;
			Verpackung = ArtikelEntity.Verpackung;
			VK_Festpreis = ArtikelEntity.VK_Festpreis;
			Volumen = ArtikelEntity.Volumen;
			Warengruppe = ArtikelEntity.Warengruppe;
			Webshop = ArtikelEntity.Webshop;
			Werkzeug = ArtikelEntity.Werkzeug;
			Wert_Anfangsbestand = ArtikelEntity.Wert_Anfangsbestand;
			Zeichnungsnummer = ArtikelEntity.Zeichnungsnummer;
			Zolltarif_nr = ArtikelEntity.Zolltarif_nr;
		}
		public ArtikelBudgetModel(Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity ArtikelEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellnummernEntity> bestellnummernEntities)
			: this(ArtikelEntity)
		{
			suppliers = bestellnummernEntities?.Select(x => new Supplier
			{
				Name1 = "",
				Name2 = "",
				Article_supplier_name = x.Bestell_Nr,
				Nummer = (int?)x.Lieferanten_Nr ?? -1,
				Standard = x.Standardlieferant
			})?.ToList();
		}
		public Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity ToBudgetartikel()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity
			{
				aktiv = aktiv,
				aktualisiert = aktualisiert,
				Anfangsbestand = Anfangsbestand,
				Artikel_aus_eigener_Produktion = Artikel_aus_eigener_Produktion,
				Artikel_für_weitere_Bestellungen_sperren = Artikel_fur_weitere_Bestellungen_sperren,
				Artikelfamilie_Kunde = Artikelfamilie_Kunde,
				Artikelfamilie_Kunde_Detail1 = Artikelfamilie_Kunde_Detail1,
				Artikelfamilie_Kunde_Detail2 = Artikelfamilie_Kunde_Detail2,
				Artikelkurztext = Artikelkurztext,
				Artikel_Nr = Artikel_Nr,
				Artikelnummer = Artikelnummer,
				Barverkauf = Barverkauf,
				Bezeichnung_1 = Bezeichnung_1,
				Bezeichnung_2 = Bezeichnung_2,
				Bezeichnung_3 = Bezeichnung_3,
				Crossreferenz = Crossreferenz,
				Cu_Gewicht = Cu_Gewicht,
				Datum_Anfangsbestand = Datum_Anfangsbestand,
				DEL = DEL,
				DEL_fixiert = DEL_fixiert,
				Dokumente = Dokumente,
				EAN = EAN,
				Einheit = Einheit,
				Ersatzartikel = Ersatzartikel,
				ESD_Schutz = ESD_Schutz,
				fakturieren_Stückliste = fakturieren_Stuckliste,
				Farbe = Farbe,
				fibu_rahmen = fibu_rahmen,
				Freigabestatus = Freigabestatus,
				Freigabestatus_TN_intern = Freigabestatus_TN_intern,
				Gebinde = Gebinde,
				Gewicht = Gewicht,
				Größe = Grosse,
				Grund_für_Sperre = Grund_fur_Sperre,
				gültig_bis = gultig_bis,
				Halle = Halle,
				Index_Kunde = Index_Kunde,
				Index_Kunde_Datum = Index_Kunde_Datum,
				Kategorie = Kategorie,
				Kriterium1 = Kriterium1,
				Kriterium2 = Kriterium2,
				Kriterium3 = Kriterium3,
				Kriterium4 = Kriterium4,
				Kupferbasis = Kupferbasis,
				Kupferzahl = Kupferzahl,
				Lagerartikel = Lagerartikel,
				Lagerhaltungskosten = Lagerhaltungskosten,
				Langtext = Langtext,
				Langtext_drucken_AB = Langtext_drucken_AB,
				Langtext_drucken_BW = Langtext_drucken_BW,
				Materialkosten_Alt = Materialkosten_Alt,
				Preiseinheit = Preiseinheit,
				pro_Zeiteinheit = pro_Zeiteinheit,
				Produktionszeit = Produktionszeit,
				Provisionsartikel = Provisionsartikel,
				Prüfstatus_TN_Ware = Prufstatus_TN_Ware,
				Rabattierfähig = Rabattierfahig,
				Rahmen = Rahmen,
				Rahmenauslauf = Rahmenauslauf,
				Rahmenmenge = Rahmenmenge,
				Rahmen_Nr = Rahmen_Nr,
				Seriennummer = Seriennummer,
				Seriennummernverwaltung = Seriennummernverwaltung,
				Sonderrabatt = Sonderrabatt,
				Standard_Lagerort_id = Standard_Lagerort_id,
				Stückliste = Stuckliste,
				//public money? Stundensatz   =  ,
				Stundensatz = Stundensatz,
				Sysmonummer = Sysmonummer,
				UL_Etikett = UL_Etikett,
				UL_zertifiziert = UL_zertifiziert,
				Umsatzsteuer = Umsatzsteuer,
				Ursprungsland = Ursprungsland,
				Verpackung = Verpackung,
				VK_Festpreis = VK_Festpreis,
				Volumen = Volumen,
				Warengruppe = Warengruppe,
				Webshop = Webshop,
				Werkzeug = Werkzeug,
				Wert_Anfangsbestand = Wert_Anfangsbestand,
				Zeichnungsnummer = Zeichnungsnummer,
				Zolltarif_nr = Zolltarif_nr,
			};
		}


		public class Supplier
		{
			public int Nummer { get; set; }
			public int LieferantenNummer { get; set; }
			public string Name1 { get; set; }
			public string Name2 { get; set; }
			public string Article_supplier_name { get; set; }
			public bool? Standard { get; set; }
			public decimal Price { get; set; }
			public string Symbol { get; set; }
			public decimal VAT { get; set; }
			public decimal Discount { get; set; }
			public decimal UnitBasis { get; set; }
			public decimal WiederB { get; set; }
			public decimal Proof { get; set; }
			public string ArticleRef { get; set; }
			public Supplier()
			{

			}
			public Supplier(Infrastructure.Data.Entities.Tables.FNC.BestellnummernEntity bestellnummernEntity)
			{
				if(bestellnummernEntity == null)
					return;

				Nummer = (int)(bestellnummernEntity.Lieferanten_Nr ?? -1);
				//LieferantenNummer = bestellnummernEntity.num;
				Name1 = bestellnummernEntity.Artikelbezeichnung;
				Name2 = bestellnummernEntity.Artikelbezeichnung2;
				Article_supplier_name = bestellnummernEntity.LiefrantanName;
				Standard = bestellnummernEntity.Standardlieferant;
				Price = bestellnummernEntity.Einkaufspreis ?? 0;
				//Symbol = bestellnummernEntity.Lieferanten_Nr;
				VAT = bestellnummernEntity.Umsatzsteuer ?? 0;
				Discount = bestellnummernEntity.Rabatt ?? 0;
				UnitBasis = bestellnummernEntity.Preiseinheit ?? 0;
				WiederB = bestellnummernEntity.Wiederbeschaffungszeitraum ?? 0;
				Proof = bestellnummernEntity.Prüftiefe_WE ?? 0;
				ArticleRef = bestellnummernEntity.Bestell_Nr;
			}
		}

		public List<Infrastructure.Data.Entities.Tables.FNC.BestellnummernEntity> ToBestellnummernEntities()
		{
			return suppliers?.Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellnummernEntity
			{
				Artikel_Nr = this.Artikel_Nr,
				Lieferanten_Nr = x.Nummer,
				Artikelbezeichnung = Bezeichnung_1,
				Artikelbezeichnung2 = Bezeichnung_2,
				Standardlieferant = x.Standard ?? false,
				Bestell_Nr = x.ArticleRef,
				Einkaufspreis = x.Price,
				//Symbol = x.Symbol,
				Umsatzsteuer = x.VAT > 1 ? x.VAT / 100 : x.VAT,
				Rabatt = x.Discount,
				Preiseinheit = x.UnitBasis,
				Wiederbeschaffungszeitraum = x.WiederB,
				Prüftiefe_WE = x.Proof,
				LiefrantanName = x.Article_supplier_name
			})?.ToList() ?? null;
		}
	}
}
