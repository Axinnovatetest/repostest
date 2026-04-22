using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class ArtikelEntity
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
		public bool? IsArticleNumberSpecial { get; set; }
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
		public bool? EdiDefault { get; set; }

		public int ArticleImageId { get; set; }
		// - 2023-08-23 CoC
		public string CocVersion { get; set; }
		// - 202024-02-28 Capital // E-Drawing
		public bool? IsEDrawing { get; set; }
		public string BemerkungCRP { get; set; }
		// - 2024-03-08
		public decimal? ProductionLotSize { get; set; }
		// - 2024-03-06 Task :00024 PM - FG1(back)
		public string Artikelbezeichnung { get; set; }
		public string OrderNumber { get; set; }
		public string Consumption12Months { get; set; }
		public int CountRows { get; set; }


		public string Projektname { get; set; }
		public decimal? Produktionlosgrosse { get; set; }
		public int? ManufacturerPreviousArticleId { get; set; }
		public string ManufacturerPreviousArticle { get; set; }
		public int? ManufacturerNextArticleId { get; set; }
		public string ManufacturerNextArticle { get; set; }
		public int? CustomerTechnicId { get; set; }
		public string CustomerTechnic { get; set; }
		public string CustomerEnd { get; set; }
		// - 2024-08-06 Ticket #38753
		public string DeliveryNoteCustomerComments { get; set; }
		// - 21/05/2024 for roh artikelnummer
		public string Manufacturer { get; set; }
		public string ManufacturerNumber { get; set; }
		public string BemerkungCRPPlanung { get; set; }

		public ArtikelEntity() { }
		public ArtikelEntity(SqlDataReader dataRow)
		{
			try
			{
				try
				{
					// - FIXME: Column Größe in DB is of type real, which can hold more than a decimal in C#. Articles 897-086-00 & 897-202-00 has value around 10E+37 which cannot fit in decimal variable!
					Größe = (dataRow["Größe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Größe"]);
				} catch(Exception e)
				{
					Größe = 0;
				}

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
				//Größe = (dataRow["Größe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Größe"]);
				GrundFürSperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
				gültigBis = (dataRow["gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["gültig bis"]);
				Halle = (dataRow["Halle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Halle"]);
				Hubmastleitungen = (dataRow["Hubmastleitungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Hubmastleitungen"]);
				ID_Klassifizierung = (dataRow["ID_Klassifizierung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Klassifizierung"]);
				Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
				Index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
				Info_WE = (dataRow["Info_WE"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Info_WE"]);
				IsArticleNumberSpecial = (dataRow["IsArticleNumberSpecial"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsArticleNumberSpecial"]);
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
				//new column
				ESD_Schutz_Text = (dataRow["ESD_Schutz_Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ESD_Schutz_Text"]);
				Dienstelistung = (dataRow["Dienstelistung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Dienstelistung"]);
				//
				// REM: Quick fix - 01/05/2021 - reverse VAT to initial value // Ridha
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
				//2021-10-22
				Blokiert_Status = (dataRow["Blokiert_Status"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blokiert_Status"]);

				// 2022-07-06
				ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
				CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
				CustomerIndex = (dataRow["CustomerIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerIndex"]);
				CustomerIndexSequence = (dataRow["CustomerIndexSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerIndexSequence"]);
				CustomerItemNumber = (dataRow["CustomerItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerItemNumber"]);
				CustomerItemNumberSequence = (dataRow["CustomerItemNumberSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerItemNumberSequence"]);
				CustomerPrefix = (dataRow["CustomerPrefix"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerPrefix"]);
				ProductionCountryCode = (dataRow["ProductionCountryCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionCountryCode"]);
				ProductionCountryName = (dataRow["ProductionCountryName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionCountryName"]);
				ProductionCountrySequence = (dataRow["ProductionCountrySequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionCountrySequence"]);
				ProductionSiteCode = (dataRow["ProductionSiteCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionSiteCode"]);
				ProductionSiteName = (dataRow["ProductionSiteName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionSiteName"]);
				ProductionSiteSequence = (dataRow["ProductionSiteSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionSiteSequence"]);
				// - 2022-09-05
				UBG = Convert.ToBoolean(dataRow["UBG"]);
				// - 2023-01-20
				EdiDefault = (dataRow["EdiDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EdiDefault"]);
				// - 2023-08-23 - CoC
				CocVersion = (dataRow["CocVersion"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CocVersion"]);
				IsEDrawing = (dataRow["IsEDrawing"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsEDrawing"]);
				ProductionLotSize = (dataRow["ProductionLotSize"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionLotSize"]);

				Projektname = (dataRow["Projektname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projektname"]);
				Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
				ManufacturerPreviousArticleId = (dataRow["ManufacturerPreviousArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ManufacturerPreviousArticleId"]);
				ManufacturerPreviousArticle = (dataRow["ManufacturerPreviousArticle"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["ManufacturerPreviousArticle"]);
				ManufacturerNextArticleId = (dataRow["ManufacturerNextArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ManufacturerNextArticleId"]);
				ManufacturerNextArticle = (dataRow["ManufacturerNextArticle"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["ManufacturerNextArticle"]);
				CustomerTechnicId = (dataRow["CustomerTechnicId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerTechnicId"]);
				CustomerTechnic = (dataRow["CustomerTechnic"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["CustomerTechnic"]);
				CustomerEnd = (dataRow["CustomerEnd"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["CustomerEnd"]);
				DeliveryNoteCustomerComments = (dataRow["DeliveryNoteCustomerComments"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["DeliveryNoteCustomerComments"]);

				// - 21/05/2024 for roh artikelnummer
				Manufacturer = (dataRow["Manufacturer"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["Manufacturer"]);
				ManufacturerNumber = (dataRow["ManufacturerNumber"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["ManufacturerNumber"]);
			} catch(Exception e)
			{
				throw;
			}
		}
		public ArtikelEntity(DataRow dataRow)
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
				IsArticleNumberSpecial = (dataRow["IsArticleNumberSpecial"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsArticleNumberSpecial"]);
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
				// - 2023-01-20
				EdiDefault = (dataRow["EdiDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EdiDefault"]);
				// - 2032-08-23 - CoC
				CocVersion = (dataRow["CocVersion"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CocVersion"]);
				IsEDrawing = (dataRow["IsEDrawing"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsEDrawing"]);
				ProductionLotSize = (dataRow["ProductionLotSize"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionLotSize"]);
				BemerkungCRP = (dataRow["BemerkungCRP"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BemerkungCRP"]);
				BemerkungCRPPlanung = (dataRow["BemerkungCRPPlanung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BemerkungCRPPlanung"]);

				Produktionlosgrosse = (dataRow["ProductionLotSize"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionLotSize"]);
				Projektname = (dataRow["Projektname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projektname"]);
				Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
				ManufacturerPreviousArticleId = (dataRow["ManufacturerPreviousArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ManufacturerPreviousArticleId"]);
				ManufacturerPreviousArticle = (dataRow["ManufacturerPreviousArticle"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["ManufacturerPreviousArticle"]);
				ManufacturerNextArticleId = (dataRow["ManufacturerNextArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ManufacturerNextArticleId"]);
				ManufacturerNextArticle = (dataRow["ManufacturerNextArticle"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["ManufacturerNextArticle"]);
				CustomerTechnicId = (dataRow["CustomerTechnicId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerTechnicId"]);
				CustomerTechnic = (dataRow["CustomerTechnic"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["CustomerTechnic"]);
				CustomerEnd = (dataRow["CustomerEnd"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["CustomerEnd"]);
				DeliveryNoteCustomerComments = (dataRow["DeliveryNoteCustomerComments"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["DeliveryNoteCustomerComments"]);
				// - 21/05/2024 for roh artikelnummer
				Manufacturer = (dataRow["Manufacturer"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["Manufacturer"]);
				ManufacturerNumber = (dataRow["ManufacturerNumber"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["ManufacturerNumber"]);
				Blokiert_Status = (dataRow["Blokiert_Status"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Blokiert_Status"]);
				try
				{
					CountRows = (dataRow["CountRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["CountRows"]);
				} catch(Exception ex)
				{
					CountRows = 0;
				}
			} catch(Exception sss)
			{
				// - SELECT * FROM Artikel ORDER BY Artikelnummer ASC OFFSET 0 ROWS FETCH NEXT 58457 ROWS ONLY
				throw;
			}

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
				ArtikelAusEigenerProduktion = ArtikelAusEigenerProduktion,
				ArtikelFürWeitereBestellungenSperren = ArtikelFürWeitereBestellungenSperren,
				Artikelfamilie_Kunde = Artikelfamilie_Kunde,
				Artikelfamilie_Kunde_Detail1 = Artikelfamilie_Kunde_Detail1,
				Artikelfamilie_Kunde_Detail2 = Artikelfamilie_Kunde_Detail2,
				artikelklassifizierung = artikelklassifizierung,
				Artikelkurztext = Artikelkurztext,
				ArtikelNr = ArtikelNr,
				ArtikelNummer = ArtikelNummer,
				Barverkauf = Barverkauf,
				Bezeichnung1 = Bezeichnung1,
				Bezeichnung2 = Bezeichnung2,
				Bezeichnung3 = Bezeichnung3,
				BezeichnungAL = BezeichnungAL,
				Blokiert_Status = Blokiert_Status,
				COF_Pflichtig = COF_Pflichtig,
				CP_required = CP_required,
				Crossreferenz = Crossreferenz,
				CuGewicht = CuGewicht,
				CustomerNumber = CustomerNumber,
				CustomerIndex = CustomerIndex,
				CustomerIndexSequence = CustomerIndexSequence,
				CustomerItemNumber = CustomerItemNumber,
				CustomerItemNumberSequence = CustomerItemNumberSequence,
				CustomerPrefix = CustomerPrefix,
				DatumAnfangsbestand = DatumAnfangsbestand,
				DEL = DEL,
				DELFixiert = DELFixiert,
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
				fakturierenStückliste = fakturierenStückliste,
				Farbe = Farbe,
				fibu_rahmen = fibu_rahmen,
				Freigabestatus = Freigabestatus,
				FreigabestatusTNIntern = FreigabestatusTNIntern,
				Gebinde = Gebinde,
				Gewicht = Gewicht,
				Größe = Größe,
				GrundFürSperre = GrundFürSperre,
				gültigBis = gültigBis,
				Halle = Halle,
				Hubmastleitungen = Hubmastleitungen,
				ID_Klassifizierung = ID_Klassifizierung,
				Index_Kunde = Index_Kunde,
				Index_Kunde_Datum = Index_Kunde_Datum,
				Info_WE = Info_WE,
				IsArticleNumberSpecial = IsArticleNumberSpecial,
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
				MineralsConfirmity = MineralsConfirmity,
				Praeferenz_Aktuelles_jahr = Praeferenz_Aktuelles_jahr,
				Praeferenz_Folgejahr = Praeferenz_Folgejahr,
				Preiseinheit = Preiseinheit,
				proZeiteinheit = proZeiteinheit,
				ProductionCountryCode = ProductionCountryCode,
				ProductionCountryName = ProductionCountryName,
				ProductionCountrySequence = ProductionCountrySequence,
				ProductionSiteCode = ProductionSiteCode,
				ProductionSiteName = ProductionSiteName,
				ProductionSiteSequence = ProductionSiteSequence,
				Produktionszeit = Produktionszeit,
				Provisionsartikel = Provisionsartikel,
				PrufstatusTNWare = PrufstatusTNWare,
				Rabattierfähig = Rabattierfähig,
				Rahmen = Rahmen,
				Rahmen2 = Rahmen2,
				Rahmenauslauf = Rahmenauslauf,
				Rahmenauslauf2 = Rahmenauslauf2,
				Rahmenmenge = Rahmenmenge,
				Rahmenmenge2 = Rahmenmenge2,
				RahmenNr = RahmenNr,
				RahmenNr2 = RahmenNr2,
				REACHSVHCConfirmity = REACHSVHCConfirmity,
				ROHSEEEConfirmity = ROHSEEEConfirmity,
				Seriennummer = Seriennummer,
				Seriennummernverwaltung = Seriennummernverwaltung,
				Sonderrabatt = Sonderrabatt,
				Standard_Lagerort_id = Standard_Lagerort_id,
				Stuckliste = Stuckliste,
				Stundensatz = Stundensatz,
				Sysmonummer = Sysmonummer,
				UBG = UBG,
				ULEtikett = ULEtikett,
				ULzertifiziert = ULzertifiziert,
				Umsatzsteuer = Umsatzsteuer,
				Ursprungsland = Ursprungsland,
				VDA_1 = VDA_1,
				VDA_2 = VDA_2,
				Verpackung = Verpackung,
				Verpackungsart = Verpackungsart,
				Verpackungsmenge = Verpackungsmenge,
				VKFestpreis = VKFestpreis,
				Volumen = Volumen,
				Warengruppe = Warengruppe,
				Warentyp = Warentyp,
				Webshop = Webshop,
				Werkzeug = Werkzeug,
				Wert_Anfangsbestand = Wert_Anfangsbestand,
				Zeichnungsnummer = Zeichnungsnummer,
				Zeitraum_MHD = Zeitraum_MHD,
				Zolltarif_nr = Zolltarif_nr,
				Zuschlag_VK = Zuschlag_VK,
				EdiDefault = EdiDefault,
				CocVersion = CocVersion,
				IsEDrawing = IsEDrawing,
				Manufacturer = Manufacturer,
				ManufacturerNumber = ManufacturerNumber,
				BemerkungCRPPlanung = BemerkungCRPPlanung,
			};
		}

	}
	public class PreviousAndNextArtikelEntity
	{
		public string ArtikelNummer { get; set; }
		public int ArtikelNr { get; set; }
		public PreviousAndNextArtikelEntity() { }
		public PreviousAndNextArtikelEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			ArtikelNummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
		}
	}
	public class ROHArticlesUnitsEntity
	{
		public string Unit { get; set; }
		public ROHArticlesUnitsEntity() { }
		public ROHArticlesUnitsEntity(DataRow dataRow)
		{
			Unit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
		}
	}

	public class MinimalArtikelEntity
	{
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int? CustomerNumber { get; set; }
		public string CustomerItemNumber { get; set; }
		public MinimalArtikelEntity()
		{

		}
		public MinimalArtikelEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			CustomerItemNumber = (dataRow["CustomerItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerItemNumber"]);
		}
	}
	public class MinimalSalesArtikelEntity
	{
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Stundensatz { get; set; }
		public MinimalSalesArtikelEntity()
		{

		}
		public MinimalSalesArtikelEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
		}
	}
	public class MinimalArtikelDataEntity
	{
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public int? Warentyp { get; set; }
		public string Warengruppe { get; set; }
		public MinimalArtikelDataEntity()
		{

		}
		public MinimalArtikelDataEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNummer"]);
			Warentyp = (dataRow["Warentyp"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Warentyp"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
		}
	}
}
