using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime;

namespace Infrastructure.Data.Access.Joins.BSD
{
	public class Migration
	{
		#region Entities
		public class ArtikelExtendedEntity
		{
			public string Abladestelle { get; set; }
			public bool? aktiv { get; set; }
			public DateTime? aktualisiert { get; set; }
			public decimal? Anfangsbestand { get; set; }
			public string ArticleNumber { get; set; }
			public bool? ArtikelAusEigenerProduktion { get; set; }
			public bool? ArtikelFürWeitereBestellungenSperren { get; set; }
			public string Artikelfamilie_Kunde { get; set; }
			public string Artikelfamilie_Kunde_Detail1 { get; set; }
			public string Artikelfamilie_Kunde_Detail2 { get; set; }
			public string artikelklassifizierung { get; set; }
			public string Artikelkurztext { get; set; }
			public int ArtikelNr { get; set; }
			public string ArtikelNummer { get; set; }
			public bool? Barverkauf { get; set; }
			public string Bezeichnung1 { get; set; }
			public string Bezeichnung2 { get; set; }
			public string Bezeichnung3 { get; set; }
			public string BezeichnungAL { get; set; }
			public bool? Blokiert_Status { get; set; }
			public bool? COF_Pflichtig { get; set; }
			public bool? CP_required { get; set; }
			public string Crossreferenz { get; set; }
			public decimal? CuGewicht { get; set; }
			public int? CustomerNumber { get; set; }
			public string CustomerIndex { get; set; }
			public int? CustomerIndexSequence { get; set; }
			public string CustomerItemNumber { get; set; }
			public int? CustomerItemNumberSequence { get; set; }
			public string CustomerPrefix { get; set; }
			public DateTime? DatumAnfangsbestand { get; set; }
			public int? DEL { get; set; }
			public bool? DELFixiert { get; set; }
			public int? Dienstelistung { get; set; }
			public string Dokumente { get; set; }
			public string EAN { get; set; }
			public string Einheit { get; set; }
			public bool? EMPB { get; set; }
			public bool? EMPB_Freigegeben { get; set; }
			public int? Ersatzartikel { get; set; }
			public bool? ESD_Schutz { get; set; }
			public string ESD_Schutz_Text { get; set; }
			public decimal? Exportgewicht { get; set; }
			public bool? fakturierenStückliste { get; set; }
			public string Farbe { get; set; }
			public int? fibu_rahmen { get; set; }
			public string Freigabestatus { get; set; }
			public string FreigabestatusTNIntern { get; set; }
			public string Gebinde { get; set; }
			public decimal? Gewicht { get; set; }
			public decimal? Größe { get; set; }
			public string GrundFürSperre { get; set; }
			public DateTime? gültigBis { get; set; }
			public string Halle { get; set; }
			public bool? Hubmastleitungen { get; set; }
			public int? ID_Klassifizierung { get; set; }
			public string Index_Kunde { get; set; }
			public DateTime? Index_Kunde_Datum { get; set; }
			public string Info_WE { get; set; }
			public bool? EdiDefault { get; set; }
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
			public bool? MineralsConfirmity { get; set; }
			public string Praeferenz_Aktuelles_jahr { get; set; }
			public string Praeferenz_Folgejahr { get; set; }
			public decimal? Preiseinheit { get; set; }
			public string proZeiteinheit { get; set; }
			public string ProductionCountryCode { get; set; }
			public string ProductionCountryName { get; set; }
			public int? ProductionCountrySequence { get; set; }
			public string ProductionSiteCode { get; set; }
			public string ProductionSiteName { get; set; }
			public int? ProductionSiteSequence { get; set; }
			public decimal? Produktionszeit { get; set; }
			public bool? Provisionsartikel { get; set; }
			public string PrufstatusTNWare { get; set; }
			public bool? Rabattierfähig { get; set; }
			public bool? Rahmen { get; set; }
			public bool? Rahmen2 { get; set; }
			public DateTime? Rahmenauslauf { get; set; }
			public DateTime? Rahmenauslauf2 { get; set; }
			public decimal? Rahmenmenge { get; set; }
			public decimal? Rahmenmenge2 { get; set; }
			public string RahmenNr { get; set; }
			public string RahmenNr2 { get; set; }
			public bool? REACHSVHCConfirmity { get; set; }
			public bool? ROHSEEEConfirmity { get; set; }
			public string Seriennummer { get; set; }
			public bool? Seriennummernverwaltung { get; set; }
			public decimal? Sonderrabatt { get; set; }
			public int? Standard_Lagerort_id { get; set; }
			public bool? Stuckliste { get; set; }
			public decimal? Stundensatz { get; set; }
			public string Sysmonummer { get; set; }
			public bool? ULEtikett { get; set; }
			public bool? ULzertifiziert { get; set; }
			public decimal? Umsatzsteuer { get; set; }
			public string Ursprungsland { get; set; }
			public bool? VDA_1 { get; set; }
			public bool? VDA_2 { get; set; }
			public string Verpackung { get; set; }
			public string Verpackungsart { get; set; }
			public int? Verpackungsmenge { get; set; }
			public bool? VKFestpreis { get; set; }
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
			// - 2022-09-05
			public bool UBG { get; set; }
			public string NummerKreise { get; set; }

			public int ArticleImageId { get; set; }


			public ArtikelExtendedEntity() { }
			public ArtikelExtendedEntity(DataRow dataRow)
			{
				try
				{
					// - FIXME: Column Größe in DB is of type real, which can hold more than a decimal in C#. Articles 897-086-00 & 897-202-00 has value around 10E+37 which cannot fit in decimal variable!
					Größe = (dataRow["Größe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Größe"]);
				} catch(Exception e)
				{
					Größe = 0;
				}

				try
				{

					Abladestelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
					aktiv = (dataRow["aktiv"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["aktiv"]);
					aktualisiert = (dataRow["aktualisiert"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["aktualisiert"]);
					Anfangsbestand = (dataRow["Anfangsbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anfangsbestand"]);
					Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
					ArtikelAusEigenerProduktion = (dataRow["Artikel aus eigener Produktion"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Artikel aus eigener Produktion"]);
					ArtikelFürWeitereBestellungenSperren = (dataRow["Artikel für weitere Bestellungen sperren"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Artikel für weitere Bestellungen sperren"]);
					Artikelfamilie_Kunde = (dataRow["Artikelfamilie_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde"]);
					Artikelfamilie_Kunde_Detail1 = (dataRow["Artikelfamilie_Kunde_Detail1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail1"]);
					Artikelfamilie_Kunde_Detail2 = (dataRow["Artikelfamilie_Kunde_Detail2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail2"]);
					Artikelkurztext = (dataRow["Artikelkurztext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelkurztext"]);
					ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
					ArtikelNummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
					Barverkauf = (dataRow["Barverkauf"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Barverkauf"]);
					Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
					Bezeichnung2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
					Bezeichnung3 = (dataRow["Bezeichnung 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 3"]);
					BezeichnungAL = (dataRow["BezeichnungAL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BezeichnungAL"]);
					COF_Pflichtig = (dataRow["COF_Pflichtig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COF_Pflichtig"]);
					Crossreferenz = (dataRow["Crossreferenz"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Crossreferenz"]);
					CuGewicht = (dataRow["Cu-Gewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Cu-Gewicht"]);
					DatumAnfangsbestand = (dataRow["Datum Anfangsbestand"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum Anfangsbestand"]);
					DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
					DELFixiert = (dataRow["DEL fixiert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DEL fixiert"]);
					Dokumente = (dataRow["Dokumente"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dokumente"]);
					EAN = (dataRow["EAN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EAN"]);
					Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
					EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB"]);
					EMPB_Freigegeben = (dataRow["EMPB_Freigegeben"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Freigegeben"]);
					Ersatzartikel = (dataRow["Ersatzartikel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Ersatzartikel"]);
					ESD_Schutz = (dataRow["ESD_Schutz"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ESD_Schutz"]);
					Exportgewicht = (dataRow["Exportgewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Exportgewicht"]);
					fakturierenStückliste = (dataRow["fakturieren Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["fakturieren Stückliste"]);
					Farbe = (dataRow["Farbe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Farbe"]);
					fibu_rahmen = (dataRow["fibu_rahmen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["fibu_rahmen"]);
					Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
					FreigabestatusTNIntern = (dataRow["Freigabestatus TN intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus TN intern"]);
					Gebinde = (dataRow["Gebinde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gebinde"]);
					Gewicht = (dataRow["Gewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gewicht"]);
					GrundFürSperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
					gültigBis = (dataRow["gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["gültig bis"]);
					Halle = (dataRow["Halle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Halle"]);
					Hubmastleitungen = (dataRow["Hubmastleitungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Hubmastleitungen"]);
					ID_Klassifizierung = (dataRow["ID_Klassifizierung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Klassifizierung"]);
					Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
					Index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
					Info_WE = (dataRow["Info_WE"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Info_WE"]);
					EdiDefault = (dataRow["EdiDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EdiDefault"]);
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
					Materialkosten_Alt = (dataRow["Materialkosten_Alt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Materialkosten_Alt"]);
					MHD = (dataRow["MHD"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MHD"]);
					MineralsConfirmity = (dataRow["Minerals Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Minerals Confirmity"]);
					Praeferenz_Aktuelles_jahr = (dataRow["Praeferenz_Aktuelles_jahr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Praeferenz_Aktuelles_jahr"]);
					Praeferenz_Folgejahr = (dataRow["Praeferenz_Folgejahr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Praeferenz_Folgejahr"]);
					Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
					proZeiteinheit = (dataRow["pro Zeiteinheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["pro Zeiteinheit"]);
					Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Produktionszeit"]);
					Provisionsartikel = (dataRow["Provisionsartikel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Provisionsartikel"]);
					PrufstatusTNWare = (dataRow["Prüfstatus TN Ware"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfstatus TN Ware"]);
					Rabattierfähig = (dataRow["Rabattierfähig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rabattierfähig"]);
					Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen"]);
					Rahmen2 = (dataRow["Rahmen2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen2"]);
					Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf"]);
					Rahmenauslauf2 = (dataRow["Rahmenauslauf2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf2"]);
					Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Rahmenmenge"]);
					Rahmenmenge2 = (dataRow["Rahmenmenge2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Rahmenmenge2"]);
					RahmenNr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
					RahmenNr2 = (dataRow["Rahmen-Nr2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr2"]);
					REACHSVHCConfirmity = (dataRow["REACH SVHC Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["REACH SVHC Confirmity"]);
					ROHSEEEConfirmity = (dataRow["ROHS EEE Confirmity"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ROHS EEE Confirmity"]);
					Seriennummer = (dataRow["Seriennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Seriennummer"]);
					Seriennummernverwaltung = (dataRow["Seriennummernverwaltung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Seriennummernverwaltung"]);
					Sonderrabatt = (dataRow["Sonderrabatt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Sonderrabatt"]);
					Standard_Lagerort_id = (dataRow["Standard_Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Standard_Lagerort_id"]);
					Stuckliste = (dataRow["Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Stückliste"]);
					Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
					ULEtikett = (dataRow["UL Etikett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UL Etikett"]);
					ULzertifiziert = (dataRow["UL zertifiziert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UL zertifiziert"]);
					Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Umsatzsteuer"]);
					Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
					Verpackung = (dataRow["Verpackung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackung"]);
					Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
					Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsmenge"]);
					VKFestpreis = (dataRow["VK-Festpreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VK-Festpreis"]);
					Volumen = (dataRow["Volumen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Volumen"]);
					Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
					Warentyp = (dataRow["Warentyp"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Warentyp"]);
					Webshop = (dataRow["Webshop"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Webshop"]);
					Werkzeug = (dataRow["Werkzeug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Werkzeug"]);
					Wert_Anfangsbestand = (dataRow["Wert_Anfangsbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Wert_Anfangsbestand"]);
					Zeichnungsnummer = (dataRow["Zeichnungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeichnungsnummer"]);
					Zeitraum_MHD = (dataRow["Zeitraum_MHD"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeitraum_MHD"]);
					Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);

					// REM: Quick fix -- Remove from Ridha
					//if (Umsatzsteuer != 0.16)
					//{
					//    Umsatzsteuer = (decimal)0.16;
					//}
					CP_required = (dataRow["CP_required"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CP_required"]);
					//
					VDA_1 = (dataRow["VDA_1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VDA_1"]);
					VDA_2 = (dataRow["VDA_2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VDA_2"]);
					Zuschlag_VK = (dataRow["Zuschlag_VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zuschlag_VK"]);
					artikelklassifizierung = (dataRow["artikelklassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelklassifizierung"]);

					// 2022-07-06
					ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
					CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
					CustomerIndex = (dataRow["CustomerIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerIndex"]);
					CustomerIndexSequence = (dataRow["CustomerIndexSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerIndexSequence"]);
					CustomerItemNumber = (dataRow["CustomerItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerItemNumber"]);
					CustomerItemNumberSequence = (dataRow["CustomerItemNumberSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerItemNumberSequence"]);
					ProductionCountryCode = (dataRow["ProductionCountryCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionCountryCode"]);
					CustomerPrefix = (dataRow["CustomerPrefix"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerPrefix"]);
					ProductionCountryName = (dataRow["ProductionCountryName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionCountryName"]);
					ProductionCountrySequence = (dataRow["ProductionCountrySequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionCountrySequence"]);
					ProductionSiteCode = (dataRow["ProductionSiteCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionSiteCode"]);
					ProductionSiteName = (dataRow["ProductionSiteName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionSiteName"]);
					ProductionSiteSequence = (dataRow["ProductionSiteSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionSiteSequence"]);
					// - 2022-09-05
					UBG = Convert.ToBoolean(dataRow["UBG"]);
					// -
					NummerKreise = (dataRow["NummerKreise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["NummerKreise"]);
				} catch(Exception)
				{
					// - SELECT * FROM Artikel ORDER BY Artikelnummer ASC OFFSET 0 ROWS FETCH NEXT 58457 ROWS ONLY
					throw;
				}

			}
		}
		public class ArticleBomEntity
		{
			public int ArticleNr { get; set; }
			public string ArticleNumber { get; set; }
			public int BomArticleNr { get; set; }
			public string BomArticleNumber { get; set; }
			public decimal BomQuantity { get; set; }
			public string BomPosition { get; set; }
			public ArticleBomEntity() { }
			public ArticleBomEntity(DataRow dataRow)
			{
				ArticleNr = (dataRow["ArticleNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["ArticleNr"]);
				ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
				ArticleNr = (dataRow["BomArticleNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["BomArticleNr"]);
				ArticleNumber = (dataRow["BomArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BomArticleNumber"]);
				BomPosition = (dataRow["BomPosition"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BomPosition"]);
				BomQuantity = (dataRow["BomQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["BomQuantity"]);
			}
		}
		public class FertigungWOrder
		{
			public int FertigungId { get; set; }
			public int Fertigungsnummer { get; set; }
			public int OrderId { get; set; }
			public int OrderPositionId { get; set; }
			public int OrderNumber { get; set; }
			public FertigungWOrder()
			{

			}
			public FertigungWOrder(DataRow dataRow)
			{
				FertigungId = (dataRow["FertigungId"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["FertigungId"]);
				Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Fertigungsnummer"]);
				OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["OrderId"]);
				OrderPositionId = (dataRow["OrderPositionId"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["OrderPositionId"]);
				OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["OrderNumber"]);
			}
		}
		#endregion region Entities

		#region stats
		public static List<ArtikelExtendedEntity> GetEFForCustomerArticleNumber()
		{
			var dt = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"
                                select  T1.NummerKreise,A1.*  from Artikel A1 inner join
                                (
                                select distinct SUBSTRING(A.Artikelnummer, 1, T.debut) as NummerKreise from Artikel A
                                inner join
                                (
                                SELECT CHARINDEX('-', artikelnummer,(CHARINDEX('-', artikelnummer) )+1) as debut, artikelnummer from Artikel
                                where Warengruppe='EF' and artikelnummer not like'%rep%' and artikelnummer not like'umb%' and artikelnummer not like'tech%' and artikelnummer not like 'PFEIFFER%'
                                and artikelnummer not like 'end%' and artikelnummer not like 'Inv%' and artikelnummer not like 'test%' and artikelnummer not like 'ana%' and artikelnummer  like '%-%'
                                and artikelnummer not like 'B%' and artikelnummer not like 'selt%'
                                and CHARINDEX('-', artikelnummer,(CHARINDEX('-', artikelnummer) )+1) >0
                                ) T on T.Artikelnummer=A.Artikelnummer
                                inner join Fertigung F on F.Artikel_Nr=A.[Artikel-Nr] and F.Kennzeichen='Offen'
                                )T1 on T1.NummerKreise = left(A1.artikelnummer,LEN(T1.NummerKreise))
                                left join
                                (
                                select count(*) as nombre,F.Artikel_Nr as Artikel_Nr from fertigung f where F.Kennzeichen='offen' group by F.Artikel_Nr
                                )T2 on T2.Artikel_Nr=A1.[Artikel-Nr]
                                left join
                                (
                                select [Artikel-Nr] as Artikel_Nr,sum(isnull(bestand,0)) as bestand from Lager where bestand<>0 group by [Artikel-Nr]
                                )L on L.Artikel_Nr=A1.[Artikel-Nr]
                                inner join
                                (
                                select  T0.NummerKreise,count(T0.Artikelnummer) as Nombre from
                                (
                                select distinct SUBSTRING(A.Artikelnummer, 1, T.debut) as NummerKreise, A2.Artikelnummer from Artikel A
                                inner join
                                (
                                SELECT CHARINDEX('-', artikelnummer,(CHARINDEX('-', artikelnummer) )+1) as debut, artikelnummer from Artikel
                                where Warengruppe='EF' and artikelnummer not like'%rep%' and artikelnummer not like'umb%' and artikelnummer not like'tech%' and artikelnummer not like 'PFEIFFER%'
                                and artikelnummer not like 'end%' and artikelnummer not like 'Inv%' and artikelnummer not like 'test%' and artikelnummer not like 'ana%' and artikelnummer  like '%-%'
                                and artikelnummer not like 'B%' and artikelnummer not like 'selt%'
                                and CHARINDEX('-', artikelnummer,(CHARINDEX('-', artikelnummer) )+1) >0
                                ) T on T.Artikelnummer=A.Artikelnummer
                                inner join Fertigung F on F.Artikel_Nr=A.[Artikel-Nr] and F.Kennzeichen='Offen'
                                inner join Artikel A2 on SUBSTRING(A.Artikelnummer, 1, T.debut) = left(A2.artikelnummer,len(SUBSTRING(A.Artikelnummer, 1, T.debut)))
                                ) T0
                                group by T0.NummerKreise
                                )T7 on T7.NummerKreise=T1.NummerKreise
                                where  T7.Nombre>1
                                order by T1.NummerKreise";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 600;
				new SqlDataAdapter(sqlCommand).Fill(dt);

				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new ArtikelExtendedEntity(x)).ToList();
				}
				else
				{
					return new List<ArtikelExtendedEntity>();
				}
			}
		}
		public static List<ArtikelExtendedEntity> GetEFForCustomerArticleNumber_ohneFA()
		{
			var dt = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"
                                select T6.NummerKreise,A.* from
                                (
                                select T5.NummerKreise as NummerKreise ,count(T5.Artikelnummer) as [nombre Artikel] from
                                (
                                select T4.NummerKreise,AA.Artikelnummer from Artikel AA inner join
                                (
                                select T3.NummerKreise, sum( isnull(T3.[Nombre FA],0)) as sommeFA from
                                (
                                select  T1.NummerKreise,A1.Artikelnummer,T2.nombre as [Nombre FA],A1.Index_Kunde,A1.Freigabestatus from Artikel A1 inner join
                                (
                                select distinct SUBSTRING(A.Artikelnummer, 1, T.debut) as NummerKreise from Artikel A
                                inner join
                                (
                                SELECT CHARINDEX('-', artikelnummer,(CHARINDEX('-', artikelnummer) )+1) as debut, artikelnummer from Artikel
                                where Warengruppe='EF' and artikelnummer not like'%rep%' and artikelnummer not like'umb%' and artikelnummer not like'tech%' and artikelnummer not like 'PFEIFFER%'
                                and artikelnummer not like 'end%' and artikelnummer not like 'Inv%' and artikelnummer not like 'test%' and artikelnummer not like 'ana%' and artikelnummer  like '%-%'
                                and artikelnummer not like 'B%' and artikelnummer not like 'selt%'
                                and CHARINDEX('-', artikelnummer,(CHARINDEX('-', artikelnummer) )+1) >0
                                ) T on T.Artikelnummer=A.Artikelnummer
                                left join Fertigung F on F.Artikel_Nr=A.[Artikel-Nr] and F.Kennzeichen='Offen'
                                where fertigungsnummer is null
                                --order by NummerKreise
                                )T1 on T1.NummerKreise = left(A1.artikelnummer,LEN(T1.NummerKreise))
                                left join
                                (
                                select count(*) as nombre,F.Artikel_Nr as Artikel_Nr from fertigung f where F.Kennzeichen='offen' group by F.Artikel_Nr
                                )T2 on T2.Artikel_Nr=A1.[Artikel-Nr]
                                --order by T1.NummerKreise
                                )T3
                                group by T3.NummerKreise
                                having  sum( isnull(T3.[Nombre FA],0))=0
                                )T4 on T4.NummerKreise=left(AA.artikelnummer,LEN(T4.NummerKreise))
                                --order by T4.NummerKreise
                                )T5
                                group by T5.NummerKreise
                                )T6 inner join Artikel A on T6.NummerKreise=left(A.artikelnummer,LEN(T6.NummerKreise))
                                left join
                                (
                                select [Artikel-Nr] as Artikel_Nr,sum(isnull(bestand,0)) as bestand from Lager where bestand<>0 group by [Artikel-Nr]
                                )L on L.Artikel_Nr=A.[Artikel-Nr]
                                where T6.[nombre Artikel]>1
                                order by T6.NummerKreise
                                 ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 600;
				new SqlDataAdapter(sqlCommand).Fill(dt);

				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new ArtikelExtendedEntity(x)).ToList();
				}
				else
				{
					return new List<ArtikelExtendedEntity>();
				}
			}
		}
		public static int EditCustomerItem(List<ArtikelExtendedEntity> articles)
		{
			if(articles == null || articles.Count <= 0)
			{
				return 0;
			}
			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "";
				SqlCommand cmd = new SqlCommand(query, cnn);
				// -
				foreach(var item in articles)
				{
					query += $"UPDATE [Artikel] SET [CustomerItemNumber]=${item.CustomerItemNumber},[CustomerItemNumberSequence]={item.CustomerItemNumberSequence}  WHERE [Artikel-Nr]=${item.ArtikelNr};";
				}

				// -
				cmd.CommandText = query;
				return cmd.ExecuteNonQuery();
			}
		}

		//  -
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetOpenFaByLager(int lagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM Fertigung f Where f.Kennzeichen='offen' AND ISNULL(f.FA_Gestartet,0)=0 AND f.Lagerort_id={lagerId}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.FertigungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetLastBoms(List<int> articleIds)
		{
			if(articleIds == null || articleIds.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@" Select s1.* From __BSD_Stucklisten_Snapshot s1 
		                        Join (Select Max(BOMVersion) BOMVersion, [Artikel-Nr] From __BSD_Stucklisten_Snapshot 
			                        WHERE [Artikel-Nr] IN ({(string.Join(",", articleIds))}) Group By [Artikel-Nr]) s2 on s2.BOMVersion=s1.BomVersion AND s2.[Artikel-Nr]=s1.[Artikel-Nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		public static int replacePositions(int faId, List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $" DELETE FROM [Fertigung_Positionen] WHERE [ID_Fertigung_HL]={faId};";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += $"" +
							$"INSERT INTO [Fertigung_Positionen] ([Anzahl],[Arbeitsanweisung],[Artikel_Nr],[Bemerkungen],[buchen],[Fertiger],[Fertigstellung_Ist],[ID_Fertigung],[ID_Fertigung_HL],[Lagerort_ID],[Löschen],[ME gebucht],[Termin_Soll],[Vorgang_Nr]) VALUES ( "

							+ "@Anzahl" + i + ","
							+ "@Arbeitsanweisung" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@buchen" + i + ","
							+ "@Fertiger" + i + ","
							+ "@Fertigstellung_Ist" + i + ","
							+ "@ID_Fertigung" + i + ","
							+ "@ID_Fertigung_HL" + i + ","
							+ "@Lagerort_ID" + i + ","
							+ "@Löschen" + i + ","
							+ "@ME_gebucht" + i + ","
							+ "@Termin_Soll" + i + ","
							+ "@Vorgang_Nr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Arbeitsanweisung" + i, item.Arbeitsanweisung == null ? (object)DBNull.Value : item.Arbeitsanweisung);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("buchen" + i, item.Buchen == null ? (object)DBNull.Value : item.Buchen);
						sqlCommand.Parameters.AddWithValue("Fertiger" + i, item.Fertiger == null ? (object)DBNull.Value : item.Fertiger);
						sqlCommand.Parameters.AddWithValue("Fertigstellung_Ist" + i, item.Fertigstellung_Ist == null ? (object)DBNull.Value : item.Fertigstellung_Ist);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung" + i, faId);
						sqlCommand.Parameters.AddWithValue("ID_Fertigung_HL" + i, faId);
						sqlCommand.Parameters.AddWithValue("Lagerort_ID" + i, item.Lagerort_ID == null ? (object)DBNull.Value : item.Lagerort_ID);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("ME_gebucht" + i, item.ME_gebucht == null ? (object)DBNull.Value : item.ME_gebucht);
						sqlCommand.Parameters.AddWithValue("Termin_Soll" + i, item.Termin_Soll == null ? (object)DBNull.Value : item.Termin_Soll);
						sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}
		public static List<ArticleBomEntity> GetBomsWUbg(IEnumerable<int> articleIds)
		{
			if(articleIds == null || articleIds.Count() <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
							SELECT b.[Artikel-Nr] AS ArticleNr, b.ArticleNumber AS ArticleNumber, a.[Artikel-Nr] AS BomArticleNr, a.Artikelnummer AS BomArticleNumber, CAST(s.Anzahl AS DECIMAL(38,3)) AS BomQuantity, s.Position AS BomPosition FROM [Stücklisten] s 
							JOIN Artikel b on b.[Artikel-Nr]=s.[Artikel-Nr]
							JOIN artikel a on a.[Artikel-Nr]=s.[Artikel-Nr des Bauteils]
							WHERE a.UBG=1 AND s.[Artikel-Nr] IN ({(string.Join(",", articleIds))});";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ArticleBomEntity(x)).ToList();
			}
			else
			{
				return new List<ArticleBomEntity>();
			}
		}
		public static int UpdateFaStlWithTransaction(IEnumerable<int> faIds, int newlager, SqlConnection connection, SqlTransaction transaction)
		{
			if(faIds == null || faIds.Count() <= 0)
			{
				return -1;
			}
			string query = $"DELETE [Fertigung_Positionen] WHERE [ID_Fertigung] IN ({string.Join(",", faIds)});\n";
			foreach(var item in faIds)
			{
				query += $"INSERT INTO [Fertigung_Positionen] (ID_Fertigung, ID_Fertigung_HL, [Artikel_Nr], [Anzahl], [Lagerort_ID], [buchen], [Vorgang_Nr], [ME gebucht], [Löschen]) " +
					$"SELECT f.ID AS ID_Fertigung, f.ID AS ID_Fertigung_HL, s.[Artikel-Nr des Bauteils] AS [Artikel_Nr], CAST(f.Originalanzahl AS DECIMAL(38,3))*CAST(s.Anzahl AS DECIMAL(38,3)) AS Anzahl,{newlager} AS [Lagerort_ID], 1 AS [buchen], [Vorgang_Nr], 0 AS [ME gebucht], 0 AS [Löschen] FROM [Stücklisten] s JOIN Fertigung f on f.Artikel_Nr=s.[Artikel-Nr] WHERE f.ID={item};\n";
			}

			var sqlCommand = new SqlCommand(query, connection, transaction);

			return sqlCommand.ExecuteNonQuery();
		}
		public static List<FertigungWOrder> GetFaWOrders(IEnumerable<int> faNummers)
		{
			if(faNummers == null || faNummers.Count() <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
							SELECT 0 AS FertigungId, p.Fertigungsnummer, a.Nr AS OrderId, p.Nr AS OrderPositionId, a.[Angebot-Nr] AS OrderNumber FROM [Angebotene Artikel] p 
							JOIN Angebote a on a.Nr=p.[Angebot-Nr]
							WHERE Fertigungsnummer>0 AND [Fertigungsnummer] IN ({(string.Join(",", faNummers))});";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new FertigungWOrder(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		#endregion stats
	}
}
