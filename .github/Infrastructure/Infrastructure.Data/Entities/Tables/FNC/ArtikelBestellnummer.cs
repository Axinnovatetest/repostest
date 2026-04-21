using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class ArtikelBestellnummer
	{
		public bool aktiv { get; set; }
		public DateTime? aktualisiert { get; set; }
		public double? Anfangsbestand { get; set; }
		public bool Artikel_aus_eigener_Produktion { get; set; }
		public bool Artikel_für_weitere_Bestellungen_sperren { get; set; }
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
		public bool fakturieren_Stückliste { get; set; }
		public string Farbe { get; set; }
		public int? fibu_rahmen { get; set; }
		public string Freigabestatus { get; set; }
		public string Freigabestatus_TN_intern { get; set; }
		public string Gebinde { get; set; }
		public double? Gewicht { get; set; }
		public double? Größe { get; set; }
		public string Grund_für_Sperre { get; set; }
		public DateTime? gültig_bis { get; set; }
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
		public string Prüfstatus_TN_Ware { get; set; }
		public bool Rabattierfähig { get; set; }
		public bool Rahmen { get; set; }
		public DateTime? Rahmenauslauf { get; set; }
		public double? Rahmenmenge { get; set; }
		public string Rahmen_Nr { get; set; }
		public string Seriennummer { get; set; }
		public bool Seriennummernverwaltung { get; set; }
		public double? Sonderrabatt { get; set; }
		public int? Standard_Lagerort_id { get; set; }
		public bool Stückliste { get; set; }
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
		public string Einkaufspreis_gültig_bis { get; set; }
		public double? EK_EUR { get; set; }
		public double? EK_total { get; set; }
		public double? Fracht { get; set; }
		public DateTime? letzte_Aktualisierung { get; set; }
		public double? Lieferanten_Nr { get; set; }
		public double? Logistik { get; set; }
		public double? Mindestbestellmenge { get; set; }
		public int Nr { get; set; }
		//public double? Preiseinheit { get; set; }
		public double? Prüftiefe_WE { get; set; }
		public double? Rabatt { get; set; }
		public bool Standardlieferant { get; set; }
		//public double? Umsatzsteuer { get; set; }
		public double? Verpackungseinheit { get; set; }
		//public string Warengruppe { get; set; }
		public double? Wiederbeschaffungszeitraum { get; set; }
		public double? Zoll { get; set; }
		public double? Zusatz { get; set; }

		public ArtikelBestellnummer() { }

		public ArtikelBestellnummer(DataRow dataRow)
		{

			aktiv = Convert.ToBoolean(dataRow["aktiv"]);
			aktualisiert = (dataRow["aktualisiert"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["aktualisiert"]);
			Anfangsbestand = (dataRow["Anfangsbestand"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Anfangsbestand"]);
			Artikel_aus_eigener_Produktion = Convert.ToBoolean(dataRow["Artikel aus eigener Produktion"]);
			Artikel_für_weitere_Bestellungen_sperren = Convert.ToBoolean(dataRow["Artikel für weitere Bestellungen sperren"]);
			Artikelfamilie_Kunde = (dataRow["Artikelfamilie_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde"]);
			Artikelfamilie_Kunde_Detail1 = (dataRow["Artikelfamilie_Kunde_Detail1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail1"]);
			Artikelfamilie_Kunde_Detail2 = (dataRow["Artikelfamilie_Kunde_Detail2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail2"]);
			Artikelkurztext = (dataRow["Artikelkurztext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelkurztext"]);
			Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Barverkauf = Convert.ToBoolean(dataRow["Barverkauf"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			Bezeichnung_3 = (dataRow["Bezeichnung 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 3"]);
			Crossreferenz = (dataRow["Crossreferenz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Crossreferenz"]);
			Cu_Gewicht = (dataRow["Cu-Gewicht"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Cu-Gewicht"]);
			Datum_Anfangsbestand = (dataRow["Datum Anfangsbestand"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum Anfangsbestand"]);
			DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
			DEL_fixiert = Convert.ToBoolean(dataRow["DEL fixiert"]);
			Dokumente = (dataRow["Dokumente"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dokumente"]);
			EAN = (dataRow["EAN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EAN"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			Ersatzartikel = (dataRow["Ersatzartikel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Ersatzartikel"]);
			ESD_Schutz = Convert.ToBoolean(dataRow["ESD_Schutz"]);
			fakturieren_Stückliste = Convert.ToBoolean(dataRow["fakturieren Stückliste"]);
			Farbe = (dataRow["Farbe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Farbe"]);
			fibu_rahmen = (dataRow["fibu_rahmen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["fibu_rahmen"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Freigabestatus_TN_intern = (dataRow["Freigabestatus TN intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus TN intern"]);
			Gebinde = (dataRow["Gebinde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gebinde"]);
			Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Gewicht"]);
			Größe = (dataRow["Größe"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Größe"]);
			Grund_für_Sperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
			gültig_bis = (dataRow["gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["gültig bis"]);
			Halle = (dataRow["Halle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Halle"]);
			Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
			Index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde_Datum"]);
			Kategorie = (dataRow["Kategorie"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kategorie"]);
			Kriterium1 = (dataRow["Kriterium1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium1"]);
			Kriterium2 = (dataRow["Kriterium2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium2"]);
			Kriterium3 = (dataRow["Kriterium3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium3"]);
			Kriterium4 = (dataRow["Kriterium4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium4"]);
			Kupferbasis = (dataRow["Kupferbasis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kupferbasis"]);
			Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Kupferzahl"]);
			Lagerartikel = Convert.ToBoolean(dataRow["Lagerartikel"]);
			Lagerhaltungskosten = (dataRow["Lagerhaltungskosten"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Lagerhaltungskosten"]);
			Langtext = (dataRow["Langtext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Langtext"]);
			Langtext_drucken_AB = Convert.ToBoolean(dataRow["Langtext_drucken_AB"]);
			Langtext_drucken_BW = Convert.ToBoolean(dataRow["Langtext_drucken_BW"]);
			Materialkosten_Alt = (dataRow["Materialkosten_Alt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Materialkosten_Alt"]);
			Preiseinheit = Convert.ToDouble(dataRow["Preiseinheit"]);
			pro_Zeiteinheit = (dataRow["pro Zeiteinheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["pro Zeiteinheit"]);
			Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Produktionszeit"]);
			Provisionsartikel = Convert.ToBoolean(dataRow["Provisionsartikel"]);
			Prüfstatus_TN_Ware = (dataRow["Prüfstatus TN Ware"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfstatus TN Ware"]);
			Rabattierfähig = Convert.ToBoolean(dataRow["Rabattierfähig"]);
			Rahmen = Convert.ToBoolean(dataRow["Rahmen"]);
			Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf"]);
			Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rahmenmenge"]);
			Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
			Seriennummer = (dataRow["Seriennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Seriennummer"]);
			Seriennummernverwaltung = Convert.ToBoolean(dataRow["Seriennummernverwaltung"]);
			Sonderrabatt = (dataRow["Sonderrabatt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Sonderrabatt"]);
			Standard_Lagerort_id = (dataRow["Standard_Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Standard_Lagerort_id"]);
			Stückliste = Convert.ToBoolean(dataRow["Stückliste"]);
			Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
			UL_Etikett = Convert.ToBoolean(dataRow["UL Etikett"]);
			UL_zertifiziert = Convert.ToBoolean(dataRow["UL zertifiziert"]);
			Umsatzsteuer = Convert.ToDouble(dataRow["Umsatzsteuer"]);
			Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
			Verpackung = (dataRow["Verpackung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackung"]);
			VK_Festpreis = Convert.ToBoolean(dataRow["VK-Festpreis"]);
			Volumen = (dataRow["Volumen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Volumen"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			Webshop = Convert.ToBoolean(dataRow["Webshop"]);
			Werkzeug = (dataRow["Werkzeug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Werkzeug"]);
			Wert_Anfangsbestand = (dataRow["Wert_Anfangsbestand"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Wert_Anfangsbestand"]);
			Zeichnungsnummer = (dataRow["Zeichnungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeichnungsnummer"]);
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);


			// - Bestellnumern
			Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
			Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot_Datum"]);
			Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
			Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
			//Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Artikel-Nr"]);
			Basispreis = (dataRow["Basispreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Basispreis"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Einkaufspreis"]);
			Einkaufspreis_gültig_bis = (dataRow["Einkaufspreis gültig bis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einkaufspreis gültig bis"]);
			EK_EUR = (dataRow["EK_EUR"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["EK_EUR"]);
			EK_total = (dataRow["EK_total"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["EK_total"]);
			Fracht = (dataRow["Fracht"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Fracht"]);
			letzte_Aktualisierung = (dataRow["letzte_Aktualisierung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["letzte_Aktualisierung"]);
			Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Lieferanten-Nr"]);
			Logistik = (dataRow["Logistik"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Logistik"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mindestbestellmenge"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Preiseinheit = Convert.ToDouble(dataRow["Preiseinheit"]);
			Prüftiefe_WE = (dataRow["Prüftiefe_WE"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Prüftiefe_WE"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt"]);
			Standardlieferant = Convert.ToBoolean(dataRow["Standardlieferant"]);
			Umsatzsteuer = Convert.ToDouble(dataRow["Umsatzsteuer"]);
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Verpackungseinheit"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Wiederbeschaffungszeitraum"]);
			Zoll = (dataRow["Zoll"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Zoll"]);
			Zusatz = (dataRow["Zusatz"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Zusatz"]);

		}
	}
}

