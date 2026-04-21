using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class Angebotene_ArtikelEntity
	{
		public int? AB_Pos_zu_BV_Pos { get; set; }
		public int? AB_Pos_zu_RA_Pos { get; set; }
		public string Abladestelle { get; set; }
		public decimal? Aktuelle_Anzahl { get; set; }
		public decimal? AnfangLagerBestand { get; set; }
		public int? Angebot_Nr { get; set; }
		public decimal? Anzahl { get; set; }
		public int? Artikel_Nr { get; set; }
		public bool? Auswahl { get; set; }
		public string Bemerkungsfeld1 { get; set; }
		public string Bemerkungsfeld2 { get; set; }
		public string Bestellnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Bezeichnung2 { get; set; }
		public string Bezeichnung2_Kunde { get; set; }
		public string Bezeichnung3 { get; set; }
		public string CSInterneBemerkung { get; set; }
		public int? DEL { get; set; }
		public bool? DEL_fixiert { get; set; }
		public int? EDI_Historie_Nr { get; set; }
		public decimal? EDI_PREIS_KUNDE { get; set; }
		public decimal? EDI_PREISEINHEIT { get; set; }
		public decimal? EDI_Quantity_Ordered { get; set; }
		public string Einheit { get; set; }
		public Single? EinzelCu_Gewicht { get; set; }
		public decimal? Einzelkupferzuschlag { get; set; }
		public decimal? Einzelpreis { get; set; }
		public bool? EKPreise_Fix { get; set; }
		public decimal? EndeLagerBestand { get; set; }
		public bool? erledigt_pos { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string FM { get; set; }
		public decimal? FM_Einzelpreis { get; set; }
		public decimal? FM_Gesamtpreis { get; set; }
		public string Freies_Format_EDI { get; set; }
		public decimal? Geliefert { get; set; }
		public string Gepackt_von { get; set; }
		public DateTime? Gepackt_Zeitpunkt { get; set; }
		public Single? GesamtCu_Gewicht { get; set; }
		public decimal? Gesamtkupferzuschlag { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public string GSExternComment { get; set; }
		public string GSInternComment { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_datum { get; set; }
		public int? KB_Pos_zu_BV_Pos { get; set; }
		public int? KB_Pos_zu_RA_Pos { get; set; }
		public int? Kupferbasis { get; set; }
		public bool? Lagerbewegung { get; set; }
		public bool? Lagerbewegung_ruckgangig { get; set; }
		public int? Lagerort_id { get; set; }
		public string Langtext { get; set; }
		public bool? Langtext_drucken { get; set; }
		public string Lieferanweisung__P_FTXDIN_TEXT_ { get; set; }
		public DateTime? Liefertermin { get; set; }
		public bool? Loschen { get; set; }
		public int? LS_Pos_zu_AB_Pos { get; set; }
		public int? LS_Pos_zu_KB_Pos { get; set; }
		public bool? LS_von_Versand_gedruckt { get; set; }
		public int Nr { get; set; }
		public decimal? OriginalAnzahl { get; set; }
		public string Packinfo_von_Lager { get; set; }
		public bool? Packstatus { get; set; }
		public int? Position { get; set; }
		public int? PositionZUEDI { get; set; }
		public string POSTEXT { get; set; }
		public bool? Preis_ausweisen { get; set; }
		public decimal? Preiseinheit { get; set; }
		public int? Preisgruppe { get; set; }
		public int? RA_Pos_zu_BV_Pos { get; set; }
		public decimal? RA_Abgerufen { get; set; }
		public decimal? RA_Offen { get; set; }
		public decimal? RA_OriginalAnzahl { get; set; }
		public Single? Rabatt { get; set; }
		public int? RE_Pos_zu_GS_Pos { get; set; }
		public bool? RP { get; set; }
		public string schriftart { get; set; }
		public bool? Seriennummern_drucken { get; set; }
		public string sortierung { get; set; }
		public bool? Stuckliste { get; set; }
		public bool? Stuckliste_drucken { get; set; }
		public bool? Summenberechnung { get; set; }
		public bool? termin_eingehalten { get; set; }
		public int? Typ { get; set; }
		public decimal? USt { get; set; }
		public bool? VDA_gedruckt { get; set; }
		public bool? Versand_gedruckt { get; set; }
		public string Versandarten_Auswahl { get; set; }
		public DateTime? Versanddatum_Auswahl { get; set; }
		public string Versanddienstleister { get; set; }
		public string Versandinfo_von_CS { get; set; }
		public string Versandinfo_von_Lager { get; set; }
		public string Versandnummer { get; set; }
		public bool? Versandstatus { get; set; }
		public decimal? VKEinzelpreis { get; set; }
		public bool? VK_Festpreis { get; set; }
		public decimal? VKGesamtpreis { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public string Zeichnungsnummer { get; set; }
		public decimal? Zuschlag_VK { get; set; }
		public string zwischensumme { get; set; }

		public Angebotene_ArtikelEntity() { }

		public Angebotene_ArtikelEntity(DataRow dataRow)
		{
			AB_Pos_zu_BV_Pos = (dataRow["AB Pos zu BV Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AB Pos zu BV Pos"]);
			AB_Pos_zu_RA_Pos = (dataRow["AB Pos zu RA Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AB Pos zu RA Pos"]);
			Abladestelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
			Aktuelle_Anzahl = (dataRow["Aktuelle Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aktuelle Anzahl"]);
			AnfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
			Angebot_Nr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Auswahl = (dataRow["Auswahl"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Auswahl"]);
			Bemerkungsfeld1 = (dataRow["Bemerkungsfeld1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungsfeld1"]);
			Bemerkungsfeld2 = (dataRow["Bemerkungsfeld2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungsfeld2"]);
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			Bezeichnung2 = (dataRow["Bezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung2"]);
			Bezeichnung2_Kunde = (dataRow["Bezeichnung2_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung2_Kunde"]);
			Bezeichnung3 = (dataRow["Bezeichnung3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung3"]);
			CSInterneBemerkung = (dataRow["CSInterneBemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CSInterneBemerkung"]);
			DEL = (dataRow["DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DEL"]);
			DEL_fixiert = (dataRow["DEL fixiert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DEL fixiert"]);
			EDI_Historie_Nr = (dataRow["EDI_Historie_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EDI_Historie_Nr"]);
			EDI_PREIS_KUNDE = (dataRow["EDI_PREIS_KUNDE"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EDI_PREIS_KUNDE"]);
			EDI_PREISEINHEIT = (dataRow["EDI_PREISEINHEIT"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EDI_PREISEINHEIT"]);
			EDI_Quantity_Ordered = (dataRow["EDI_Quantity_Ordered"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EDI_Quantity_Ordered"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			EinzelCu_Gewicht = (dataRow["EinzelCu-Gewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["EinzelCu-Gewicht"]);
			Einzelkupferzuschlag = (dataRow["Einzelkupferzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelkupferzuschlag"]);
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelpreis"]);
			EKPreise_Fix = (dataRow["EKPreise_Fix"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EKPreise_Fix"]);
			EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			FM = (dataRow["FM"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FM"]);
			FM_Einzelpreis = (dataRow["FM_Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FM_Einzelpreis"]);
			FM_Gesamtpreis = (dataRow["FM_Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FM_Gesamtpreis"]);
			Freies_Format_EDI = (dataRow["Freies_Format_EDI"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freies_Format_EDI"]);
			Geliefert = (dataRow["Geliefert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Geliefert"]);
			Gepackt_von = (dataRow["Gepackt_von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gepackt_von"]);
			Gepackt_Zeitpunkt = (dataRow["Gepackt_Zeitpunkt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Gepackt_Zeitpunkt"]);
			GesamtCu_Gewicht = (dataRow["GesamtCu-Gewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["GesamtCu-Gewicht"]);
			Gesamtkupferzuschlag = (dataRow["Gesamtkupferzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtkupferzuschlag"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			GSExternComment = (dataRow["GSExternComment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["GSExternComment"]);
			GSInternComment = (dataRow["GSInternComment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["GSInternComment"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
			Index_Kunde_datum = (dataRow["Index_Kunde_datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_datum"]);
			KB_Pos_zu_BV_Pos = (dataRow["KB Pos zu BV Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KB Pos zu BV Pos"]);
			KB_Pos_zu_RA_Pos = (dataRow["KB Pos zu RA Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KB Pos zu RA Pos"]);
			Kupferbasis = (dataRow["Kupferbasis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kupferbasis"]);
			Lagerbewegung = (dataRow["Lagerbewegung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Lagerbewegung"]);
			Lagerbewegung_ruckgangig = (dataRow["Lagerbewegung_rückgängig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Lagerbewegung_rückgängig"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Langtext = (dataRow["Langtext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Langtext"]);
			Langtext_drucken = (dataRow["Langtext_drucken"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Langtext_drucken"]);
			Lieferanweisung__P_FTXDIN_TEXT_ = (dataRow["Lieferanweisung (P_FTXDIN_TEXT)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferanweisung (P_FTXDIN_TEXT)"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Loschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			LS_Pos_zu_AB_Pos = (dataRow["LS Pos zu AB Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LS Pos zu AB Pos"]);
			LS_Pos_zu_KB_Pos = (dataRow["LS Pos zu KB Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LS Pos zu KB Pos"]);
			LS_von_Versand_gedruckt = (dataRow["LS_von_Versand_gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LS_von_Versand_gedruckt"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			OriginalAnzahl = (dataRow["OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OriginalAnzahl"]);
			Packinfo_von_Lager = (dataRow["Packinfo_von_Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Packinfo_von_Lager"]);
			Packstatus = (dataRow["Packstatus"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Packstatus"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			PositionZUEDI = (dataRow["PositionZUEDI"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PositionZUEDI"]);
			POSTEXT = (dataRow["POSTEXT"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["POSTEXT"]);
			Preis_ausweisen = (dataRow["Preis_ausweisen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Preis_ausweisen"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			RA_Pos_zu_BV_Pos = (dataRow["RA Pos zu BV Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RA Pos zu BV Pos"]);
			RA_Abgerufen = (dataRow["RA_Abgerufen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RA_Abgerufen"]);
			RA_Offen = (dataRow["RA_Offen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RA_Offen"]);
			RA_OriginalAnzahl = (dataRow["RA_OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RA_OriginalAnzahl"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rabatt"]);
			RE_Pos_zu_GS_Pos = (dataRow["RE Pos zu GS Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RE Pos zu GS Pos"]);
			RP = (dataRow["RP"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RP"]);
			schriftart = (dataRow["schriftart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["schriftart"]);
			Seriennummern_drucken = (dataRow["Seriennummern_drucken"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Seriennummern_drucken"]);
			sortierung = (dataRow["sortierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["sortierung"]);
			Stuckliste = (dataRow["Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Stückliste"]);
			Stuckliste_drucken = (dataRow["Stückliste_drucken"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Stückliste_drucken"]);
			Summenberechnung = (dataRow["Summenberechnung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Summenberechnung"]);
			termin_eingehalten = (dataRow["termin_eingehalten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["termin_eingehalten"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Typ"]);
			USt = (dataRow["USt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["USt"]);
			VDA_gedruckt = (dataRow["VDA_gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VDA_gedruckt"]);
			Versand_gedruckt = (dataRow["Versand_gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Versand_gedruckt"]);
			Versandarten_Auswahl = (dataRow["Versandarten_Auswahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandarten_Auswahl"]);
			Versanddatum_Auswahl = (dataRow["Versanddatum_Auswahl"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Versanddatum_Auswahl"]);
			Versanddienstleister = (dataRow["Versanddienstleister"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versanddienstleister"]);
			Versandinfo_von_CS = (dataRow["Versandinfo_von_CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandinfo_von_CS"]);
			Versandinfo_von_Lager = (dataRow["Versandinfo_von_Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandinfo_von_Lager"]);
			Versandnummer = (dataRow["Versandnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandnummer"]);
			Versandstatus = (dataRow["Versandstatus"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Versandstatus"]);
			VKEinzelpreis = (dataRow["VKEinzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VKEinzelpreis"]);
			VK_Festpreis = (dataRow["VK-Festpreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VK-Festpreis"]);
			VKGesamtpreis = (dataRow["VKGesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VKGesamtpreis"]);
			Wunschtermin = (dataRow["Wunschtermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wunschtermin"]);
			Zeichnungsnummer = (dataRow["Zeichnungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zeichnungsnummer"]);
			Zuschlag_VK = (dataRow["Zuschlag_VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zuschlag_VK"]);
			zwischensumme = (dataRow["zwischensumme"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["zwischensumme"]);
		}

		public Angebotene_ArtikelEntity ShallowClone()
		{
			return new Angebotene_ArtikelEntity
			{
				AB_Pos_zu_BV_Pos = AB_Pos_zu_BV_Pos,
				AB_Pos_zu_RA_Pos = AB_Pos_zu_RA_Pos,
				Abladestelle = Abladestelle,
				Aktuelle_Anzahl = Aktuelle_Anzahl,
				AnfangLagerBestand = AnfangLagerBestand,
				Angebot_Nr = Angebot_Nr,
				Anzahl = Anzahl,
				Artikel_Nr = Artikel_Nr,
				Auswahl = Auswahl,
				Bemerkungsfeld1 = Bemerkungsfeld1,
				Bemerkungsfeld2 = Bemerkungsfeld2,
				Bestellnummer = Bestellnummer,
				Bezeichnung1 = Bezeichnung1,
				Bezeichnung2 = Bezeichnung2,
				Bezeichnung2_Kunde = Bezeichnung2_Kunde,
				Bezeichnung3 = Bezeichnung3,
				CSInterneBemerkung = CSInterneBemerkung,
				DEL = DEL,
				DEL_fixiert = DEL_fixiert,
				EDI_Historie_Nr = EDI_Historie_Nr,
				EDI_PREIS_KUNDE = EDI_PREIS_KUNDE,
				EDI_PREISEINHEIT = EDI_PREISEINHEIT,
				EDI_Quantity_Ordered = EDI_Quantity_Ordered,
				Einheit = Einheit,
				EinzelCu_Gewicht = EinzelCu_Gewicht,
				Einzelkupferzuschlag = Einzelkupferzuschlag,
				Einzelpreis = Einzelpreis,
				EKPreise_Fix = EKPreise_Fix,
				EndeLagerBestand = EndeLagerBestand,
				erledigt_pos = erledigt_pos,
				Fertigungsnummer = Fertigungsnummer,
				FM = FM,
				FM_Einzelpreis = FM_Einzelpreis,
				FM_Gesamtpreis = FM_Gesamtpreis,
				Freies_Format_EDI = Freies_Format_EDI,
				Geliefert = Geliefert,
				Gepackt_von = Gepackt_von,
				Gepackt_Zeitpunkt = Gepackt_Zeitpunkt,
				GesamtCu_Gewicht = GesamtCu_Gewicht,
				Gesamtkupferzuschlag = Gesamtkupferzuschlag,
				Gesamtpreis = Gesamtpreis,
				GSExternComment = GSExternComment,
				GSInternComment = GSInternComment,
				In_Bearbeitung = In_Bearbeitung,
				Index_Kunde = Index_Kunde,
				Index_Kunde_datum = Index_Kunde_datum,
				KB_Pos_zu_BV_Pos = KB_Pos_zu_BV_Pos,
				KB_Pos_zu_RA_Pos = KB_Pos_zu_RA_Pos,
				Kupferbasis = Kupferbasis,
				Lagerbewegung = Lagerbewegung,
				Lagerbewegung_ruckgangig = Lagerbewegung_ruckgangig,
				Lagerort_id = Lagerort_id,
				Langtext = Langtext,
				Langtext_drucken = Langtext_drucken,
				Lieferanweisung__P_FTXDIN_TEXT_ = Lieferanweisung__P_FTXDIN_TEXT_,
				Liefertermin = Liefertermin,
				Loschen = Loschen,
				LS_Pos_zu_AB_Pos = LS_Pos_zu_AB_Pos,
				LS_Pos_zu_KB_Pos = LS_Pos_zu_KB_Pos,
				LS_von_Versand_gedruckt = LS_von_Versand_gedruckt,
				Nr = Nr,
				OriginalAnzahl = OriginalAnzahl,
				Packinfo_von_Lager = Packinfo_von_Lager,
				Packstatus = Packstatus,
				Position = Position,
				PositionZUEDI = PositionZUEDI,
				POSTEXT = POSTEXT,
				Preis_ausweisen = Preis_ausweisen,
				Preiseinheit = Preiseinheit,
				Preisgruppe = Preisgruppe,
				RA_Pos_zu_BV_Pos = RA_Pos_zu_BV_Pos,
				RA_Abgerufen = RA_Abgerufen,
				RA_Offen = RA_Offen,
				RA_OriginalAnzahl = RA_OriginalAnzahl,
				Rabatt = Rabatt,
				RE_Pos_zu_GS_Pos = RE_Pos_zu_GS_Pos,
				RP = RP,
				schriftart = schriftart,
				Seriennummern_drucken = Seriennummern_drucken,
				sortierung = sortierung,
				Stuckliste = Stuckliste,
				Stuckliste_drucken = Stuckliste_drucken,
				Summenberechnung = Summenberechnung,
				termin_eingehalten = termin_eingehalten,
				Typ = Typ,
				USt = USt,
				VDA_gedruckt = VDA_gedruckt,
				Versand_gedruckt = Versand_gedruckt,
				Versandarten_Auswahl = Versandarten_Auswahl,
				Versanddatum_Auswahl = Versanddatum_Auswahl,
				Versanddienstleister = Versanddienstleister,
				Versandinfo_von_CS = Versandinfo_von_CS,
				Versandinfo_von_Lager = Versandinfo_von_Lager,
				Versandnummer = Versandnummer,
				Versandstatus = Versandstatus,
				VKEinzelpreis = VKEinzelpreis,
				VK_Festpreis = VK_Festpreis,
				VKGesamtpreis = VKGesamtpreis,
				Wunschtermin = Wunschtermin,
				Zeichnungsnummer = Zeichnungsnummer,
				Zuschlag_VK = Zuschlag_VK,
				zwischensumme = zwischensumme
			};
		}
	}
}

