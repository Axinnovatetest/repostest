using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class ArtikelEntity
	{
		public string Abladestelle { get; set; }
		public bool? aktiv { get; set; }
		public DateTime? aktualisiert { get; set; }
		public decimal? Anfangsbestand { get; set; }
		public string ArticleNumber { get; set; }
		public bool? Artikel_aus_eigener_Produktion { get; set; }
		public bool? Artikel_fur_weitere_Bestellungen_sperren { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public string Artikelfamilie_Kunde_Detail1 { get; set; }
		public string Artikelfamilie_Kunde_Detail2 { get; set; }
		public string artikelklassifizierung { get; set; }
		public string Artikelkurztext { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public bool? Barverkauf { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Bezeichnung_3 { get; set; }
		public string BezeichnungAL { get; set; }
		public bool? Blokiert_Status { get; set; }
		public bool? COF_Pflichtig { get; set; }
		public bool? CP_required { get; set; }
		public string Crossreferenz { get; set; }
		public Single? Cu_Gewicht { get; set; }
		public string CustomerIndex { get; set; }
		public int? CustomerIndexSequence { get; set; }
		public string CustomerItemNumber { get; set; }
		public int? CustomerItemNumberSequence { get; set; }
		public int? CustomerNumber { get; set; }
		public string CustomerPrefix { get; set; }
		public DateTime? Datum_Anfangsbestand { get; set; }
		public int? DEL { get; set; }
		public bool? DEL_fixiert { get; set; }
		public int? Dienstelistung { get; set; }
		public string Dokumente { get; set; }
		public string EAN { get; set; }
		public string Einheit { get; set; }
		public bool? EMPB { get; set; }
		public bool? EMPB_Freigegeben { get; set; }
		public int? Ersatzartikel { get; set; }
		public bool? ESD_Schutz { get; set; }
		public string ESD_Schutz_Text { get; set; }
		public Single? Exportgewicht { get; set; }
		public bool? fakturieren_Stuckliste { get; set; }
		public string Farbe { get; set; }
		public int? fibu_rahmen { get; set; }
		public string Freigabestatus { get; set; }
		public string Freigabestatus_TN_intern { get; set; }
		public string Gebinde { get; set; }
		public decimal? Gewicht { get; set; }
		public decimal? Grosse { get; set; }
		public string Grund_fur_Sperre { get; set; }
		public DateTime? gultig_bis { get; set; }
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
		public Single? Materialkosten_Alt { get; set; }
		public bool? MHD { get; set; }
		public bool? Minerals_Confirmity { get; set; }
		public string Praeferenz_Aktuelles_jahr { get; set; }
		public string Praeferenz_Folgejahr { get; set; }
		public decimal? Preiseinheit { get; set; }
		public string pro_Zeiteinheit { get; set; }
		public string ProductionCountryName { get; set; }
		public int? ProductionCountrySequence { get; set; }
		public string ProductionSiteName { get; set; }
		public int? ProductionSiteSequence { get; set; }
		public Single? Produktionszeit { get; set; }
		public bool? Provisionsartikel { get; set; }
		public string Prufstatus_TN_Ware { get; set; }
		public bool? Rabattierfahig { get; set; }
		public bool? Rahmen { get; set; }
		public bool? Rahmen2 { get; set; }
		public DateTime? Rahmenauslauf { get; set; }
		public DateTime? Rahmenauslauf2 { get; set; }
		public Single? Rahmenmenge { get; set; }
		public Single? Rahmenmenge2 { get; set; }
		public string Rahmen_Nr { get; set; }
		public string Rahmen_Nr2 { get; set; }
		public bool? REACH_SVHC_Confirmity { get; set; }
		public bool? ROHS_EEE_Confirmity { get; set; }
		public string Seriennummer { get; set; }
		public bool? Seriennummernverwaltung { get; set; }
		public Single? Sonderrabatt { get; set; }
		public int? Standard_Lagerort_id { get; set; }
		public bool? Stuckliste { get; set; }
		public decimal? Stundensatz { get; set; }
		public string Sysmonummer { get; set; }
		public bool? UL_Etikett { get; set; }
		public bool? UL_zertifiziert { get; set; }
		public decimal? Umsatzsteuer { get; set; }
		public string Ursprungsland { get; set; }
		public bool? VDA_1 { get; set; }
		public bool? VDA_2 { get; set; }
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
		public decimal? Zuschlag_VK { get; set; }

		public ArtikelEntity() { }

		public ArtikelEntity(DataRow dataRow)
		{
			Abladestelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
			aktiv = (dataRow["aktiv"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["aktiv"]);
			aktualisiert = (dataRow["aktualisiert"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["aktualisiert"]);
			Anfangsbestand = (dataRow["Anfangsbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anfangsbestand"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			Artikel_aus_eigener_Produktion = (dataRow["Artikel aus eigener Produktion"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Artikel aus eigener Produktion"]);
			Artikel_fur_weitere_Bestellungen_sperren = (dataRow["Artikel für weitere Bestellungen sperren"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Artikel für weitere Bestellungen sperren"]);
			Artikelfamilie_Kunde = (dataRow["Artikelfamilie_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde"]);
			Artikelfamilie_Kunde_Detail1 = (dataRow["Artikelfamilie_Kunde_Detail1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail1"]);
			Artikelfamilie_Kunde_Detail2 = (dataRow["Artikelfamilie_Kunde_Detail2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail2"]);
			artikelklassifizierung = (dataRow["artikelklassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelklassifizierung"]);
			Artikelkurztext = (dataRow["Artikelkurztext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelkurztext"]);
			Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Barverkauf = (dataRow["Barverkauf"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Barverkauf"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			Bezeichnung_3 = (dataRow["Bezeichnung 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 3"]);
			BezeichnungAL = (dataRow["BezeichnungAL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BezeichnungAL"]);
			Blokiert_Status = (dataRow["Blokiert_Status"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blokiert_Status"]);
			COF_Pflichtig = (dataRow["COF_Pflichtig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COF_Pflichtig"]);
			CP_required = (dataRow["CP_required"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CP_required"]);
			Crossreferenz = (dataRow["Crossreferenz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Crossreferenz"]);
			Cu_Gewicht = (dataRow["Cu-Gewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Cu-Gewicht"]);
			CustomerIndex = (dataRow["CustomerIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerIndex"]);
			CustomerIndexSequence = (dataRow["CustomerIndexSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerIndexSequence"]);
			CustomerItemNumber = (dataRow["CustomerItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerItemNumber"]);
			CustomerItemNumberSequence = (dataRow["CustomerItemNumberSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerItemNumberSequence"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			CustomerPrefix = (dataRow["CustomerPrefix"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerPrefix"]);
			Datum_Anfangsbestand = (dataRow["Datum Anfangsbestand"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum Anfangsbestand"]);
			DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
			DEL_fixiert = (dataRow["DEL fixiert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DEL fixiert"]);
			Dienstelistung = (dataRow["Dienstelistung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Dienstelistung"]);
			Dokumente = (dataRow["Dokumente"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dokumente"]);
			EAN = (dataRow["EAN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EAN"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB"]);
			EMPB_Freigegeben = (dataRow["EMPB_Freigegeben"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Freigegeben"]);
			Ersatzartikel = (dataRow["Ersatzartikel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Ersatzartikel"]);
			ESD_Schutz = (dataRow["ESD_Schutz"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ESD_Schutz"]);
			ESD_Schutz_Text = (dataRow["ESD_Schutz_Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ESD_Schutz_Text"]);
			Exportgewicht = (dataRow["Exportgewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Exportgewicht"]);
			fakturieren_Stuckliste = (dataRow["fakturieren Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["fakturieren Stückliste"]);
			Farbe = (dataRow["Farbe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Farbe"]);
			fibu_rahmen = (dataRow["fibu_rahmen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["fibu_rahmen"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Freigabestatus_TN_intern = (dataRow["Freigabestatus TN intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus TN intern"]);
			Gebinde = (dataRow["Gebinde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gebinde"]);
			Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gewicht"]);
			Grosse = (dataRow["Größe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Größe"]);
			Grund_fur_Sperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
			gultig_bis = (dataRow["gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["gültig bis"]);
			Halle = (dataRow["Halle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Halle"]);
			Hubmastleitungen = (dataRow["Hubmastleitungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Hubmastleitungen"]);
			ID_Klassifizierung = (dataRow["ID_Klassifizierung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Klassifizierung"]);
			Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
			Index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
			Info_WE = (dataRow["Info_WE"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Info_WE"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			Kategorie = (dataRow["Kategorie"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kategorie"]);
			Klassifizierung = (dataRow["Klassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Klassifizierung"]);
			Kriterium1 = (dataRow["Kriterium1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium1"]);
			Kriterium2 = (dataRow["Kriterium2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium2"]);
			Kriterium3 = (dataRow["Kriterium3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium3"]);
			Kriterium4 = (dataRow["Kriterium4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kriterium4"]);
			Kupferbasis = (dataRow["Kupferbasis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kupferbasis"]);
			Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kupferzahl"]);
			Lagerartikel = (dataRow["Lagerartikel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Lagerartikel"]);
			Lagerhaltungskosten = (dataRow["Lagerhaltungskosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Lagerhaltungskosten"]);
			Langtext = (dataRow["Langtext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Langtext"]);
			Langtext_drucken_AB = (dataRow["Langtext_drucken_AB"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Langtext_drucken_AB"]);
			Langtext_drucken_BW = (dataRow["Langtext_drucken_BW"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Langtext_drucken_BW"]);
			Lieferzeit = (dataRow["Lieferzeit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferzeit"]);
			Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Losgroesse"]);
			Materialkosten_Alt = (dataRow["Materialkosten_Alt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Materialkosten_Alt"]);
			MHD = (dataRow["MHD"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MHD"]);
			Minerals_Confirmity = (dataRow["Minerals Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Minerals Confirmity"]);
			Praeferenz_Aktuelles_jahr = (dataRow["Praeferenz_Aktuelles_jahr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Praeferenz_Aktuelles_jahr"]);
			Praeferenz_Folgejahr = (dataRow["Praeferenz_Folgejahr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Praeferenz_Folgejahr"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			pro_Zeiteinheit = (dataRow["pro Zeiteinheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["pro Zeiteinheit"]);
			ProductionCountryName = (dataRow["ProductionCountryName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionCountryName"]);
			ProductionCountrySequence = (dataRow["ProductionCountrySequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionCountrySequence"]);
			ProductionSiteName = (dataRow["ProductionSiteName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionSiteName"]);
			ProductionSiteSequence = (dataRow["ProductionSiteSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionSiteSequence"]);
			Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Produktionszeit"]);
			Provisionsartikel = (dataRow["Provisionsartikel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Provisionsartikel"]);
			Prufstatus_TN_Ware = (dataRow["Prüfstatus TN Ware"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfstatus TN Ware"]);
			Rabattierfahig = (dataRow["Rabattierfähig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rabattierfähig"]);
			Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen"]);
			Rahmen2 = (dataRow["Rahmen2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen2"]);
			Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf"]);
			Rahmenauslauf2 = (dataRow["Rahmenauslauf2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf2"]);
			Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rahmenmenge"]);
			Rahmenmenge2 = (dataRow["Rahmenmenge2"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rahmenmenge2"]);
			Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
			Rahmen_Nr2 = (dataRow["Rahmen-Nr2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr2"]);
			REACH_SVHC_Confirmity = (dataRow["REACH SVHC Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["REACH SVHC Confirmity"]);
			ROHS_EEE_Confirmity = (dataRow["ROHS EEE Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ROHS EEE Confirmity"]);
			Seriennummer = (dataRow["Seriennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Seriennummer"]);
			Seriennummernverwaltung = (dataRow["Seriennummernverwaltung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Seriennummernverwaltung"]);
			Sonderrabatt = (dataRow["Sonderrabatt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Sonderrabatt"]);
			Standard_Lagerort_id = (dataRow["Standard_Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Standard_Lagerort_id"]);
			Stuckliste = (dataRow["Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Stückliste"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
			Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
			UL_Etikett = (dataRow["UL Etikett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UL Etikett"]);
			UL_zertifiziert = (dataRow["UL zertifiziert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UL zertifiziert"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Umsatzsteuer"]);
			Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
			VDA_1 = (dataRow["VDA_1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VDA_1"]);
			VDA_2 = (dataRow["VDA_2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VDA_2"]);
			Verpackung = (dataRow["Verpackung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackung"]);
			Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
			Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsmenge"]);
			VK_Festpreis = (dataRow["VK-Festpreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VK-Festpreis"]);
			Volumen = (dataRow["Volumen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Volumen"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			Warentyp = (dataRow["Warentyp"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Warentyp"]);
			Webshop = (dataRow["Webshop"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Webshop"]);
			Werkzeug = (dataRow["Werkzeug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Werkzeug"]);
			Wert_Anfangsbestand = (dataRow["Wert_Anfangsbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Wert_Anfangsbestand"]);
			Zeichnungsnummer = (dataRow["Zeichnungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeichnungsnummer"]);
			Zeitraum_MHD = (dataRow["Zeitraum_MHD"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeitraum_MHD"]);
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			Zuschlag_VK = (dataRow["Zuschlag_VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zuschlag_VK"]);
		}

		public ArtikelEntity ShallowClone()
		{
			return new ArtikelEntity
			{
				Abladestelle = Abladestelle,
				aktiv = aktiv,
				aktualisiert = aktualisiert,
				Anfangsbestand = Anfangsbestand,
				ArticleNumber = ArticleNumber,
				Artikel_aus_eigener_Produktion = Artikel_aus_eigener_Produktion,
				Artikel_fur_weitere_Bestellungen_sperren = Artikel_fur_weitere_Bestellungen_sperren,
				Artikelfamilie_Kunde = Artikelfamilie_Kunde,
				Artikelfamilie_Kunde_Detail1 = Artikelfamilie_Kunde_Detail1,
				Artikelfamilie_Kunde_Detail2 = Artikelfamilie_Kunde_Detail2,
				artikelklassifizierung = artikelklassifizierung,
				Artikelkurztext = Artikelkurztext,
				Artikel_Nr = Artikel_Nr,
				Artikelnummer = Artikelnummer,
				Barverkauf = Barverkauf,
				Bezeichnung_1 = Bezeichnung_1,
				Bezeichnung_2 = Bezeichnung_2,
				Bezeichnung_3 = Bezeichnung_3,
				BezeichnungAL = BezeichnungAL,
				Blokiert_Status = Blokiert_Status,
				COF_Pflichtig = COF_Pflichtig,
				CP_required = CP_required,
				Crossreferenz = Crossreferenz,
				Cu_Gewicht = Cu_Gewicht,
				CustomerIndex = CustomerIndex,
				CustomerIndexSequence = CustomerIndexSequence,
				CustomerItemNumber = CustomerItemNumber,
				CustomerItemNumberSequence = CustomerItemNumberSequence,
				CustomerNumber = CustomerNumber,
				CustomerPrefix = CustomerPrefix,
				Datum_Anfangsbestand = Datum_Anfangsbestand,
				DEL = DEL,
				DEL_fixiert = DEL_fixiert,
				Dienstelistung = Dienstelistung,
				Dokumente = Dokumente,
				EAN = EAN,
				Einheit = Einheit,
				EMPB = EMPB,
				EMPB_Freigegeben = EMPB_Freigegeben,
				Ersatzartikel = Ersatzartikel,
				ESD_Schutz = ESD_Schutz,
				ESD_Schutz_Text = ESD_Schutz_Text,
				Exportgewicht = Exportgewicht,
				fakturieren_Stuckliste = fakturieren_Stuckliste,
				Farbe = Farbe,
				fibu_rahmen = fibu_rahmen,
				Freigabestatus = Freigabestatus,
				Freigabestatus_TN_intern = Freigabestatus_TN_intern,
				Gebinde = Gebinde,
				Gewicht = Gewicht,
				Grosse = Grosse,
				Grund_fur_Sperre = Grund_fur_Sperre,
				gultig_bis = gultig_bis,
				Halle = Halle,
				Hubmastleitungen = Hubmastleitungen,
				ID_Klassifizierung = ID_Klassifizierung,
				Index_Kunde = Index_Kunde,
				Index_Kunde_Datum = Index_Kunde_Datum,
				Info_WE = Info_WE,
				Kanban = Kanban,
				Kategorie = Kategorie,
				Klassifizierung = Klassifizierung,
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
				Lieferzeit = Lieferzeit,
				Losgroesse = Losgroesse,
				Materialkosten_Alt = Materialkosten_Alt,
				MHD = MHD,
				Minerals_Confirmity = Minerals_Confirmity,
				Praeferenz_Aktuelles_jahr = Praeferenz_Aktuelles_jahr,
				Praeferenz_Folgejahr = Praeferenz_Folgejahr,
				Preiseinheit = Preiseinheit,
				pro_Zeiteinheit = pro_Zeiteinheit,
				ProductionCountryName = ProductionCountryName,
				ProductionCountrySequence = ProductionCountrySequence,
				ProductionSiteName = ProductionSiteName,
				ProductionSiteSequence = ProductionSiteSequence,
				Produktionszeit = Produktionszeit,
				Provisionsartikel = Provisionsartikel,
				Prufstatus_TN_Ware = Prufstatus_TN_Ware,
				Rabattierfahig = Rabattierfahig,
				Rahmen = Rahmen,
				Rahmen2 = Rahmen2,
				Rahmenauslauf = Rahmenauslauf,
				Rahmenauslauf2 = Rahmenauslauf2,
				Rahmenmenge = Rahmenmenge,
				Rahmenmenge2 = Rahmenmenge2,
				Rahmen_Nr = Rahmen_Nr,
				Rahmen_Nr2 = Rahmen_Nr2,
				REACH_SVHC_Confirmity = REACH_SVHC_Confirmity,
				ROHS_EEE_Confirmity = ROHS_EEE_Confirmity,
				Seriennummer = Seriennummer,
				Seriennummernverwaltung = Seriennummernverwaltung,
				Sonderrabatt = Sonderrabatt,
				Standard_Lagerort_id = Standard_Lagerort_id,
				Stuckliste = Stuckliste,
				Stundensatz = Stundensatz,
				Sysmonummer = Sysmonummer,
				UL_Etikett = UL_Etikett,
				UL_zertifiziert = UL_zertifiziert,
				Umsatzsteuer = Umsatzsteuer,
				Ursprungsland = Ursprungsland,
				VDA_1 = VDA_1,
				VDA_2 = VDA_2,
				Verpackung = Verpackung,
				Verpackungsart = Verpackungsart,
				Verpackungsmenge = Verpackungsmenge,
				VK_Festpreis = VK_Festpreis,
				Volumen = Volumen,
				Warengruppe = Warengruppe,
				Warentyp = Warentyp,
				Webshop = Webshop,
				Werkzeug = Werkzeug,
				Wert_Anfangsbestand = Wert_Anfangsbestand,
				Zeichnungsnummer = Zeichnungsnummer,
				Zeitraum_MHD = Zeitraum_MHD,
				Zolltarif_nr = Zolltarif_nr,
				Zuschlag_VK = Zuschlag_VK
			};
		}
	}
}

