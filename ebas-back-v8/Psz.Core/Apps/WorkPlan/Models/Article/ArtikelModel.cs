using System;

namespace Psz.Core.Apps.WorkPlan.Models.Article
{
	public class ArtikelModel
	{
		public string Abladestelle { get; set; }
		public bool? aktiv { get; set; }
		public DateTime? aktualisiert { get; set; }
		public decimal? Anfangsbestand { get; set; }
		public bool? Artikel_aus_eigener_Produktion { get; set; }
		public bool? Artikel_für_weitere_Bestellungen_sperren { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public string Artikelfamilie_Kunde_Detail1 { get; set; }
		public string Artikelfamilie_Kunde_Detail2 { get; set; }
		public string Artikelkurztext { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public bool? Barverkauf { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Bezeichnung_3 { get; set; }
		public string BezeichnungAL { get; set; }
		public bool? COF_Pflichtig { get; set; }
		public string Crossreferenz { get; set; }
		public decimal? Cu_Gewicht { get; set; }
		public DateTime? Datum_Anfangsbestand { get; set; }
		public int? DEL { get; set; }
		public bool? DEL_fixiert { get; set; }
		public string Dokumente { get; set; }
		public string EAN { get; set; }
		public string Einheit { get; set; }
		public bool? EMPB { get; set; }
		public bool? EMPB_Freigegeben { get; set; }
		public int? Ersatzartikel { get; set; }
		public bool? ESD_Schutz { get; set; }
		public decimal? Exportgewicht { get; set; }
		public bool? fakturieren_Stückliste { get; set; }
		public string Farbe { get; set; }
		public int? fibu_rahmen { get; set; }
		public string Freigabestatus { get; set; }
		public string Freigabestatus_TN_intern { get; set; }
		public string Gebinde { get; set; }
		public decimal? Gewicht { get; set; }
		public decimal? Größe { get; set; }
		public string Grund_für_Sperre { get; set; }
		public DateTime? gültig_bis { get; set; }
		public string Halle { get; set; }
		public bool? Hubmastleitungen { get; set; }
		public int? ID_Klassifizierung { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }
		public string Info_WE { get; set; }
		public bool? Kanban { get; set; }
		public string Kategorie { get; set; }
		public string Klassifizierung { get; set; }
		public string Kriterium1 { get; set; }
		public string Kriterium2 { get; set; }
		public string Kriterium3 { get; set; }
		public string Kriterium4 { get; set; }
		public int? Kupferbasis { get; set; }
		public decimal? Kupferzahl { get; set; }
		public bool? Lagerartikel { get; set; }
		public decimal? Lagerhaltungskosten { get; set; }
		public string Langtext { get; set; }
		public bool? Langtext_drucken_AB { get; set; }
		public bool? Langtext_drucken_BW { get; set; }
		public string Lieferzeit { get; set; }
		public int? Losgroesse { get; set; }
		public decimal? Materialkosten_Alt { get; set; }
		public bool? MHD { get; set; }
		public bool? Minerals_Confirmity { get; set; }
		public string Praeferenz_Aktuelles_jahr { get; set; }
		public string Praeferenz_Folgejahr { get; set; }
		public decimal? Preiseinheit { get; set; }
		public string pro_Zeiteinheit { get; set; }
		public decimal? Produktionszeit { get; set; }
		public bool? Provisionsartikel { get; set; }
		public string Prüfstatus_TN_Ware { get; set; }
		public bool? Rabattierfähig { get; set; }
		public bool? Rahmen { get; set; }
		public bool? Rahmen2 { get; set; }
		public DateTime? Rahmenauslauf { get; set; }
		public DateTime? Rahmenauslauf2 { get; set; }
		public decimal? Rahmenmenge { get; set; }
		public decimal? Rahmenmenge2 { get; set; }
		public string Rahmen_Nr { get; set; }
		public string Rahmen_Nr2 { get; set; }
		public bool? REACH_SVHC_Confirmity { get; set; }
		public bool? ROHS_EEE_Confirmity { get; set; }
		public string Seriennummer { get; set; }
		public bool? Seriennummernverwaltung { get; set; }
		public decimal? Sonderrabatt { get; set; }
		public int? Standard_Lagerort_id { get; set; }
		public bool? Stückliste { get; set; }
		public decimal? Stundensatz { get; set; }
		public string Sysmonummer { get; set; }
		public bool? UL_Etikett { get; set; }
		public bool? UL_zertifiziert { get; set; }
		public decimal? Umsatzsteuer { get; set; }
		public string Ursprungsland { get; set; }
		public string Verpackung { get; set; }
		public string Verpackungsart { get; set; }
		public int? Verpackungsmenge { get; set; }
		public bool? VK_Festpreis { get; set; }
		public string Volumen { get; set; }
		public string Warengruppe { get; set; }
		public int? Warentyp { get; set; }
		public bool? Webshop { get; set; }
		public string Werkzeug { get; set; }
		public decimal? Wert_Anfangsbestand { get; set; }
		public string Zeichnungsnummer { get; set; }
		public string Zeitraum_MHD { get; set; }
		public string Zolltarif_nr { get; set; }
		public ArtikelModel()
		{

		}
		public ArtikelModel(Infrastructure.Data.Entities.Tables.ArtikelEntity artikelEntity)
		{
			Abladestelle = artikelEntity?.Abladestelle;
			aktiv = artikelEntity?.aktiv;
			aktualisiert = artikelEntity?.aktualisiert;
			Anfangsbestand = artikelEntity?.Anfangsbestand;
			Artikel_aus_eigener_Produktion = artikelEntity?.Artikel_aus_eigener_Produktion;
			Artikel_für_weitere_Bestellungen_sperren = artikelEntity?.Artikel_für_weitere_Bestellungen_sperren;
			Artikelfamilie_Kunde = artikelEntity?.Artikelfamilie_Kunde;
			Artikelfamilie_Kunde_Detail1 = artikelEntity?.Artikelfamilie_Kunde_Detail1;
			Artikelfamilie_Kunde_Detail2 = artikelEntity?.Artikelfamilie_Kunde_Detail2;
			Artikelkurztext = artikelEntity?.Artikelkurztext;
			Artikel_Nr = (int)artikelEntity?.Artikel_Nr;
			Artikelnummer = artikelEntity?.Artikelnummer;
			Barverkauf = artikelEntity?.Barverkauf;
			Bezeichnung_1 = artikelEntity?.Bezeichnung_1;
			Bezeichnung_2 = artikelEntity?.Bezeichnung_2;
			Bezeichnung_3 = artikelEntity?.Bezeichnung_3;
			BezeichnungAL = artikelEntity?.BezeichnungAL;
			COF_Pflichtig = artikelEntity?.COF_Pflichtig;
			Crossreferenz = artikelEntity?.Crossreferenz;
			Cu_Gewicht = artikelEntity?.Cu_Gewicht;
			Datum_Anfangsbestand = artikelEntity?.Datum_Anfangsbestand;
			DEL = artikelEntity?.DEL;
			DEL_fixiert = artikelEntity?.DEL_fixiert;
			Dokumente = artikelEntity?.Dokumente;
			EAN = artikelEntity?.EAN;
			Einheit = artikelEntity?.Einheit;
			EMPB = artikelEntity?.EMPB;
			EMPB_Freigegeben = artikelEntity?.EMPB_Freigegeben;
			Ersatzartikel = artikelEntity?.Ersatzartikel;
			ESD_Schutz = artikelEntity?.ESD_Schutz;
			Exportgewicht = artikelEntity?.Exportgewicht;
			fakturieren_Stückliste = artikelEntity?.fakturieren_Stückliste;
			Farbe = artikelEntity?.Farbe;
			fibu_rahmen = artikelEntity?.fibu_rahmen;
			Freigabestatus = artikelEntity?.Freigabestatus;
			Freigabestatus_TN_intern = artikelEntity?.Freigabestatus_TN_intern;
			Gebinde = artikelEntity?.Gebinde;
			Gewicht = artikelEntity?.Gewicht;
			Größe = artikelEntity?.Größe;
			Grund_für_Sperre = artikelEntity?.Grund_für_Sperre;
			gültig_bis = artikelEntity?.gültig_bis;
			Halle = artikelEntity?.Halle;
			Hubmastleitungen = artikelEntity?.Hubmastleitungen;
			ID_Klassifizierung = artikelEntity?.ID_Klassifizierung;
			Index_Kunde = artikelEntity?.Index_Kunde;
			Index_Kunde_Datum = artikelEntity?.Index_Kunde_Datum;
			Info_WE = artikelEntity?.Info_WE;
			Kanban = artikelEntity?.Kanban;
			Kategorie = artikelEntity?.Kategorie;
			Klassifizierung = artikelEntity?.Klassifizierung;
			Kriterium1 = artikelEntity?.Kriterium1;
			Kriterium2 = artikelEntity?.Kriterium2;
			Kriterium3 = artikelEntity?.Kriterium3;
			Kriterium4 = artikelEntity?.Kriterium4;
			Kupferbasis = artikelEntity?.Kupferbasis;
			Kupferzahl = artikelEntity?.Kupferzahl;
			Lagerartikel = artikelEntity?.Lagerartikel;
			Lagerhaltungskosten = artikelEntity?.Lagerhaltungskosten;
			Langtext = artikelEntity?.Langtext;
			Langtext_drucken_AB = artikelEntity?.Langtext_drucken_AB;
			Langtext_drucken_BW = artikelEntity?.Langtext_drucken_BW;
			Lieferzeit = artikelEntity?.Lieferzeit;
			Losgroesse = artikelEntity?.Losgroesse;
			Materialkosten_Alt = artikelEntity?.Materialkosten_Alt;
			MHD = artikelEntity?.MHD;
			Minerals_Confirmity = artikelEntity?.Minerals_Confirmity;
			Praeferenz_Aktuelles_jahr = artikelEntity?.Praeferenz_Aktuelles_jahr;
			Praeferenz_Folgejahr = artikelEntity?.Praeferenz_Folgejahr;
			Preiseinheit = artikelEntity?.Preiseinheit;
			pro_Zeiteinheit = artikelEntity?.pro_Zeiteinheit;
			Produktionszeit = artikelEntity?.Produktionszeit;
			Provisionsartikel = artikelEntity?.Provisionsartikel;
			Prüfstatus_TN_Ware = artikelEntity?.Prüfstatus_TN_Ware;
			Rabattierfähig = artikelEntity?.Rabattierfähig;
			Rahmen = artikelEntity?.Rahmen;
			Rahmen2 = artikelEntity?.Rahmen2;
			Rahmenauslauf = artikelEntity?.Rahmenauslauf;
			Rahmenauslauf2 = artikelEntity?.Rahmenauslauf2;
			Rahmenmenge = artikelEntity?.Rahmenmenge;
			Rahmenmenge2 = artikelEntity?.Rahmenmenge2;
			Rahmen_Nr = artikelEntity?.Rahmen_Nr;
			Rahmen_Nr2 = artikelEntity?.Rahmen_Nr2;
			REACH_SVHC_Confirmity = artikelEntity?.REACH_SVHC_Confirmity;
			ROHS_EEE_Confirmity = artikelEntity?.ROHS_EEE_Confirmity;
			Seriennummer = artikelEntity?.Seriennummer;
			Seriennummernverwaltung = artikelEntity?.Seriennummernverwaltung;
			Sonderrabatt = artikelEntity?.Sonderrabatt;
			Standard_Lagerort_id = artikelEntity?.Standard_Lagerort_id;
			Stückliste = artikelEntity?.Stückliste;
			Stundensatz = artikelEntity?.Stundensatz;
			Sysmonummer = artikelEntity?.Sysmonummer;
			UL_Etikett = artikelEntity?.UL_Etikett;
			UL_zertifiziert = artikelEntity?.UL_zertifiziert;
			Umsatzsteuer = artikelEntity?.Umsatzsteuer;
			Ursprungsland = artikelEntity?.Ursprungsland;
			Verpackung = artikelEntity?.Verpackung;
			Verpackungsart = artikelEntity?.Verpackungsart;
			Verpackungsmenge = artikelEntity?.Verpackungsmenge;
			VK_Festpreis = artikelEntity?.VK_Festpreis;
			Volumen = artikelEntity?.Volumen;
			Warengruppe = artikelEntity?.Warengruppe;
			Warentyp = artikelEntity?.Warentyp;
			Webshop = artikelEntity?.Webshop;
			Werkzeug = artikelEntity?.Werkzeug;
			Wert_Anfangsbestand = artikelEntity?.Wert_Anfangsbestand;
			Zeichnungsnummer = artikelEntity?.Zeichnungsnummer;
			Zeitraum_MHD = artikelEntity?.Zeitraum_MHD;
			Zolltarif_nr = artikelEntity?.Zolltarif_nr;
		}
	}
}
