using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class DeliveryNotesCompilationEntity
	{
		public int? Angebote_Angebot_Nr { get; set; }
		public string Angebote_Bezug { get; set; }
		public double? ANgeboteArtikel_Anzahl { get; set; }
		public string ANgeboteArtikel_Bezeichnung1 { get; set; }
		public string ANgeboteArtikel_Bezeichnung2 { get; set; }
		public string ANgeboteArtikel_Bezeichnung2_Kunde { get; set; }
		public string ANgeboteArtikel_Bezeichnung3 { get; set; }
		public string ANgeboteArtikel_Einheit { get; set; }
		public Single? ANgeboteArtikel_EinzelCu_Gewicht { get; set; }
		public Single? ANgeboteArtikel_GesamtCu_Gewicht { get; set; }
		public DateTime? ANgeboteArtikel_Liefertermin { get; set; }
		public string Artikelnummer { get; set; }
		public string Ursprungsland { get; set; }
		public string Zolltarif_nr { get; set; }


		public string Angebote_Anrede { get; set; }
		public string Angebote_Freitext { get; set; }
		public string Angebote_Ihr_Zeichen { get; set; }
		public string Angebote_Land_PLZ_Ort { get; set; }
		public string Angebote_Name2 { get; set; }
		public string Angebote_Name3 { get; set; }
		public string Angebote_Strasse_Postfach { get; set; }
		public string Angebote_Unser_Zeichen { get; set; }
		public string Angebote_Versandart { get; set; }
		public string Angebote_Vorname_NameFirma { get; set; }
		public string Angebote_LAnrede { get; set; }
		public int? Angebote_Lieferadresse { get; set; }
		public string Angebote_LLand_PLZ_Ort { get; set; }
		public string Angebote_LName2 { get; set; }
		public string Angebote_LName3 { get; set; }
		public string Angebote_LStrasse_Postfach { get; set; }
		public string Angebote_LVorname_NameFirma { get; set; }
		public int Angebote_Nr { get; set; }
		public string Lieferschein { get; set; }


		//public int? Angebote_ab_id { get; set; }
		//public string Angebote_ABSENDER { get; set; }
		//public string Angebote_Abteilung { get; set; }
		//public int? Angebote_Angebot_Nr { get; set; }
		//public string Angebote_Ansprechpartner { get; set; }
		//public bool? Angebote_Auswahl { get; set; }
		//public int? Angebote_Belegkreis { get; set; }
		//public string Angebote_Bemerkung { get; set; }
		//public string Angebote_Benutzer { get; set; }
		//public string Angebote_Bereich { get; set; }
		//public string Angebote_Bezug { get; set; }
		//public string Angebote_Briefanrede { get; set; }
		//public bool? Angebote_datueber { get; set; }
		//public DateTime? Angebote_Datum { get; set; }
		//public string Angebote_Debitorennummer { get; set; }
		//public string Angebote_Dplatz_Sirona { get; set; }
		//public string Angebote_EDI_Dateiname_CSV { get; set; }
		//public string Angebote_EDI_Kundenbestellnummer { get; set; }
		//public bool? Angebote_EDI_Order_Change { get; set; }
		//public bool? Angebote_EDI_Order_Change_Updated { get; set; }
		//public bool? Angebote_EDI_Order_Neu { get; set; }
		//public bool? Angebote_erledigt { get; set; }
		//public DateTime? Angebote_Falligkeit { get; set; }
		//public string Angebote_Freie_Text { get; set; }
		//public bool? Angebote_gebucht { get; set; }
		//public bool? Angebote_gedruckt { get; set; }
		//public bool? Angebote_In_Bearbeitung { get; set; }
		//public bool? Angebote_Interessent { get; set; }
		//public string Angebote_Konditionen { get; set; }
		//public int? Angebote_Kunden_Nr { get; set; }
		//public string Angebote_LAbteilung { get; set; }
		//public string Angebote_LAnsprechpartner { get; set; }
		//public string Angebote_LBriefanrede { get; set; }
		//public DateTime? Angebote_Liefertermin { get; set; }
		//public bool? Angebote_Loschen { get; set; }
		//public bool? Angebote_Mahnung { get; set; }
		//public string Angebote_Mandant { get; set; }
		//public bool? Angebote_Neu { get; set; }
		//public bool? Angebote_Neu_Order { get; set; }
		//public int? Angebote_nr_ang { get; set; }
		//public int? Angebote_nr_auf { get; set; }
		//public int? Angebote_nr_BV { get; set; }
		//public int? Angebote_nr_dlf { get; set; }
		//public int? Angebote_nr_gut { get; set; }
		//public int? Angebote_nr_Kanban { get; set; }
		//public int? Angebote_nr_lie { get; set; }
		//public int? Angebote_nr_pro { get; set; }
		//public int? Angebote_nr_RA { get; set; }
		//public int? Angebote_nr_rec { get; set; }
		//public int? Angebote_nr_sto { get; set; }
		//public bool? Angebote_Offnen { get; set; }
		//public int? Angebote_Personal_Nr { get; set; }
		//public string Angebote_Projekt_Nr { get; set; }
		//public bool? Angebote_rec_sent { get; set; }
		//public int? Angebote_reparatur_nr { get; set; }
		//public string Angebote_Status { get; set; }
		//public bool? Angebote_termin_eingehalten { get; set; }
		//public string Angebote_Typ { get; set; }
		//public bool? Angebote_USt_Berechnen { get; set; }
		//public string Angebote_Versandarten_Auswahl { get; set; }
		//public DateTime? Angebote_Wunschtermin { get; set; }
		//public string Angebote_Zahlungsweise { get; set; }
		//public string Angebote_Zahlungsziel { get; set; }
		//public int? ANgeboteArtikel_AB_Pos_zu_BV_Pos { get; set; }
		//public int? ANgeboteArtikel_AB_Pos_zu_RA_Pos { get; set; }
		//public string ANgeboteArtikel_Abladestelle { get; set; }
		//public double? ANgeboteArtikel_Aktuelle_Anzahl { get; set; }
		//public double? ANgeboteArtikel_AnfangLagerBestand { get; set; }
		//public int? ANgeboteArtikel_Angebot_Nr { get; set; }
		//public double? ANgeboteArtikel_Anzahl { get; set; }
		//public int? ANgeboteArtikel_Artikel_Nr { get; set; }
		//public bool? ANgeboteArtikel_Auswahl { get; set; }
		//public string ANgeboteArtikel_Bemerkungsfeld1 { get; set; }
		//public string ANgeboteArtikel_Bemerkungsfeld2 { get; set; }
		//public string ANgeboteArtikel_Bestellnummer { get; set; }
		//public string ANgeboteArtikel_Bezeichnung1 { get; set; }
		//public string ANgeboteArtikel_Bezeichnung2 { get; set; }
		//public string ANgeboteArtikel_Bezeichnung2_Kunde { get; set; }
		//public string ANgeboteArtikel_Bezeichnung3 { get; set; }
		//public string ANgeboteArtikel_CSInterneBemerkung { get; set; }
		//public int? ANgeboteArtikel_DEL { get; set; }
		//public bool? ANgeboteArtikel_DEL_fixiert { get; set; }
		//public int? ANgeboteArtikel_EDI_Historie_Nr { get; set; }
		//public double? ANgeboteArtikel_EDI_PREIS_KUNDE { get; set; }
		//public double? ANgeboteArtikel_EDI_PREISEINHEIT { get; set; }
		//public double? ANgeboteArtikel_EDI_Quantity_Ordered { get; set; }
		//public string ANgeboteArtikel_Einheit { get; set; }
		//public Single? ANgeboteArtikel_EinzelCu_Gewicht { get; set; }
		//public double? ANgeboteArtikel_Einzelkupferzuschlag { get; set; }
		//public double? ANgeboteArtikel_Einzelpreis { get; set; }
		//public bool? ANgeboteArtikel_EKPreise_Fix { get; set; }
		//public double? ANgeboteArtikel_EndeLagerBestand { get; set; }
		//public bool? ANgeboteArtikel_erledigt_pos { get; set; }
		//public int? ANgeboteArtikel_Fertigungsnummer { get; set; }
		//public string ANgeboteArtikel_FM { get; set; }
		//public double? ANgeboteArtikel_FM_Einzelpreis { get; set; }
		//public double? ANgeboteArtikel_FM_Gesamtpreis { get; set; }
		//public string ANgeboteArtikel_Freies_Format_EDI { get; set; }
		//public double? ANgeboteArtikel_Geliefert { get; set; }
		//public string ANgeboteArtikel_Gepackt_von { get; set; }
		//public DateTime? ANgeboteArtikel_Gepackt_Zeitpunkt { get; set; }
		//public Single? ANgeboteArtikel_GesamtCu_Gewicht { get; set; }
		//public double? ANgeboteArtikel_Gesamtkupferzuschlag { get; set; }
		//public double? ANgeboteArtikel_Gesamtpreis { get; set; }
		//public string ANgeboteArtikel_GSExternComment { get; set; }
		//public string ANgeboteArtikel_GSInternComment { get; set; }
		//public bool? ANgeboteArtikel_GSWithoutCopper { get; set; }
		//public bool? ANgeboteArtikel_In_Bearbeitung { get; set; }
		//public string ANgeboteArtikel_Index_Kunde { get; set; }
		//public DateTime? ANgeboteArtikel_Index_Kunde_datum { get; set; }
		//public int? ANgeboteArtikel_KB_Pos_zu_BV_Pos { get; set; }
		//public int? ANgeboteArtikel_KB_Pos_zu_RA_Pos { get; set; }
		//public int? ANgeboteArtikel_Kupferbasis { get; set; }
		//public bool? ANgeboteArtikel_Lagerbewegung { get; set; }
		//public bool? ANgeboteArtikel_Lagerbewegung_ruckgangig { get; set; }
		//public int? ANgeboteArtikel_Lagerort_id { get; set; }
		//public string ANgeboteArtikel_Langtext { get; set; }
		//public bool? ANgeboteArtikel_Langtext_drucken { get; set; }
		//public string ANgeboteArtikel_Lieferanweisung__P_FTXDIN_TEXT_ { get; set; }
		//public DateTime? ANgeboteArtikel_Liefertermin { get; set; }
		//public bool? ANgeboteArtikel_Loschen { get; set; }
		//public int? ANgeboteArtikel_LS_Pos_zu_AB_Pos { get; set; }
		//public int? ANgeboteArtikel_LS_Pos_zu_KB_Pos { get; set; }
		//public bool? ANgeboteArtikel_LS_von_Versand_gedruckt { get; set; }
		//public int ANgeboteArtikel_Nr { get; set; }
		//public double? ANgeboteArtikel_OriginalAnzahl { get; set; }
		//public string ANgeboteArtikel_Packinfo_von_Lager { get; set; }
		//public bool? ANgeboteArtikel_Packstatus { get; set; }
		//public int? ANgeboteArtikel_Position { get; set; }
		//public int? ANgeboteArtikel_PositionZUEDI { get; set; }
		//public string ANgeboteArtikel_POSTEXT { get; set; }
		//public bool? ANgeboteArtikel_Preis_ausweisen { get; set; }
		//public double? ANgeboteArtikel_Preiseinheit { get; set; }
		//public int? ANgeboteArtikel_Preisgruppe { get; set; }
		//public int? ANgeboteArtikel_RA_Pos_zu_BV_Pos { get; set; }
		//public double? ANgeboteArtikel_RA_Abgerufen { get; set; }
		//public double? ANgeboteArtikel_RA_Offen { get; set; }
		//public double? ANgeboteArtikel_RA_OriginalAnzahl { get; set; }
		//public Single? ANgeboteArtikel_Rabatt { get; set; }
		//public int? ANgeboteArtikel_RE_Pos_zu_GS_Pos { get; set; }
		//public bool? ANgeboteArtikel_RP { get; set; }
		//public string ANgeboteArtikel_schriftart { get; set; }
		//public bool? ANgeboteArtikel_Seriennummern_drucken { get; set; }
		//public string ANgeboteArtikel_sortierung { get; set; }
		//public bool? ANgeboteArtikel_Stuckliste { get; set; }
		//public bool? ANgeboteArtikel_Stuckliste_drucken { get; set; }
		//public bool? ANgeboteArtikel_Summenberechnung { get; set; }
		//public bool? ANgeboteArtikel_termin_eingehalten { get; set; }
		//public int? ANgeboteArtikel_Typ { get; set; }
		//public double? ANgeboteArtikel_USt { get; set; }
		//public bool? ANgeboteArtikel_VDA_gedruckt { get; set; }
		//public bool? ANgeboteArtikel_Versand_gedruckt { get; set; }
		//public string ANgeboteArtikel_Versandarten_Auswahl { get; set; }
		//public DateTime? ANgeboteArtikel_Versanddatum_Auswahl { get; set; }
		//public string ANgeboteArtikel_Versanddienstleister { get; set; }
		//public string ANgeboteArtikel_Versandinfo_von_CS { get; set; }
		//public string ANgeboteArtikel_Versandinfo_von_Lager { get; set; }
		//public string ANgeboteArtikel_Versandnummer { get; set; }
		//public bool? ANgeboteArtikel_Versandstatus { get; set; }
		//public double? ANgeboteArtikel_VKEinzelpreis { get; set; }
		//public bool? ANgeboteArtikel_VK_Festpreis { get; set; }
		//public double? ANgeboteArtikel_VKGesamtpreis { get; set; }
		//public DateTime? ANgeboteArtikel_Wunschtermin { get; set; }
		//public string ANgeboteArtikel_Zeichnungsnummer { get; set; }
		//public double? ANgeboteArtikel_Zuschlag_VK { get; set; }
		//public string ANgeboteArtikel_zwischensumme { get; set; }
		//public string Artikelnummer { get; set; }
		//public string Auftragsbestatigung { get; set; }
		//public string Fax { get; set; }
		//public double? Gesamtgewicht { get; set; }
		//public Single? Grosse { get; set; }
		//public string Gutschrift { get; set; }
		//public int? LS { get; set; }
		//public int? Nettotage { get; set; }
		//public int Nr { get; set; }
		//public string Rechnung { get; set; }
		//public Single? Skonto { get; set; }
		//public int? Skontotage { get; set; }
		//public string Ursprungsland { get; set; }
		//public string Zolltarif_nr { get; set; }

		public DeliveryNotesCompilationEntity() { }

		public DeliveryNotesCompilationEntity(DataRow dataRow)
		{
			Angebote_Angebot_Nr = (dataRow["Angebote_Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_Angebot-Nr"]);
			Angebote_Anrede = (dataRow["Angebote_Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Anrede"]);
			Angebote_Bezug = (dataRow["Angebote_Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Bezug"]);
			Angebote_Freitext = (dataRow["Angebote_Freitext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Freitext"]);
			Angebote_Land_PLZ_Ort = (dataRow["Angebote_Land/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Land/PLZ/Ort"]);
			Angebote_LAnrede = (dataRow["Angebote_LAnrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LAnrede"]);
			Angebote_Lieferadresse = (dataRow["Angebote_Lieferadresse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_Lieferadresse"]);
			Angebote_LLand_PLZ_Ort = (dataRow["Angebote_LLand/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LLand/PLZ/Ort"]);
			Angebote_LName2 = (dataRow["Angebote_LName2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LName2"]);
			Angebote_LName3 = (dataRow["Angebote_LName3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LName3"]);
			Angebote_LStrasse_Postfach = (dataRow["Angebote_LStraße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LStraße/Postfach"]);
			Angebote_LVorname_NameFirma = (dataRow["Angebote_LVorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LVorname/NameFirma"]);
			Angebote_Name2 = (dataRow["Angebote_Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Name2"]);
			Angebote_Name3 = (dataRow["Angebote_Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Name3"]);
			Angebote_Nr = Convert.ToInt32(dataRow["Angebote_Nr"]);
			Angebote_Strasse_Postfach = (dataRow["Angebote_Straße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Straße/Postfach"]);
			Angebote_Unser_Zeichen = (dataRow["Angebote_Unser Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Unser Zeichen"]);
			Angebote_Versandart = (dataRow["Angebote_Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Versandart"]);
			Angebote_Vorname_NameFirma = (dataRow["Angebote_Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Vorname/NameFirma"]);
			ANgeboteArtikel_Bezeichnung1 = (dataRow["ANgeboteArtikel_Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Bezeichnung1"]);
			ANgeboteArtikel_Bezeichnung2 = (dataRow["ANgeboteArtikel_Bezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Bezeichnung2"]);
			ANgeboteArtikel_Bezeichnung2_Kunde = (dataRow["ANgeboteArtikel_Bezeichnung2_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Bezeichnung2_Kunde"]);
			ANgeboteArtikel_Bezeichnung3 = (dataRow["ANgeboteArtikel_Bezeichnung3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Bezeichnung3"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			ANgeboteArtikel_Einheit = (dataRow["ANgeboteArtikel_Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Einheit"]);
			ANgeboteArtikel_EinzelCu_Gewicht = (dataRow["ANgeboteArtikel_EinzelCu-Gewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["ANgeboteArtikel_EinzelCu-Gewicht"]);
			Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			Angebote_Ihr_Zeichen = (dataRow["Angebote_Ihr Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Ihr Zeichen"]);
			ANgeboteArtikel_Liefertermin = (dataRow["ANgeboteArtikel_Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ANgeboteArtikel_Liefertermin"]);

			//Angebote_ab_id = (dataRow["Angebote_ab_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_ab_id"]);
			//Angebote_ABSENDER = (dataRow["Angebote_ABSENDER"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_ABSENDER"]);
			//Angebote_Abteilung = (dataRow["Angebote_Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Abteilung"]);
			//Angebote_Ansprechpartner = (dataRow["Angebote_Ansprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Ansprechpartner"]);
			//Angebote_Auswahl = (dataRow["Angebote_Auswahl"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_Auswahl"]);
			//Angebote_Belegkreis = (dataRow["Angebote_Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_Belegkreis"]);
			//Angebote_Bemerkung = (dataRow["Angebote_Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Bemerkung"]);
			//Angebote_Benutzer = (dataRow["Angebote_Benutzer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Benutzer"]);
			//Angebote_Bereich = (dataRow["Angebote_Bereich"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Bereich"]);
			//Angebote_Briefanrede = (dataRow["Angebote_Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Briefanrede"]);
			//Angebote_datueber = (dataRow["Angebote_datueber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_datueber"]);
			//Angebote_Datum = (dataRow["Angebote_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebote_Datum"]);
			//Angebote_Debitorennummer = (dataRow["Angebote_Debitorennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Debitorennummer"]);
			//Angebote_Dplatz_Sirona = (dataRow["Angebote_Dplatz_Sirona"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Dplatz_Sirona"]);
			//Angebote_EDI_Dateiname_CSV = (dataRow["Angebote_EDI_Dateiname_CSV"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_EDI_Dateiname_CSV"]);
			//Angebote_EDI_Kundenbestellnummer = (dataRow["Angebote_EDI_Kundenbestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_EDI_Kundenbestellnummer"]);
			//Angebote_EDI_Order_Change = (dataRow["Angebote_EDI_Order_Change"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_EDI_Order_Change"]);
			//Angebote_EDI_Order_Change_Updated = (dataRow["Angebote_EDI_Order_Change_Updated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_EDI_Order_Change_Updated"]);
			//Angebote_EDI_Order_Neu = (dataRow["Angebote_EDI_Order_Neu"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_EDI_Order_Neu"]);
			//Angebote_erledigt = (dataRow["Angebote_erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_erledigt"]);
			//Angebote_Falligkeit = (dataRow["Angebote_Fälligkeit"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebote_Fälligkeit"]);
			//Angebote_Freie_Text = (dataRow["Angebote_Freie_Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Freie_Text"]);
			////Angebote_gebucht = (dataRow["Angebote_gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_gebucht"]);
			////Angebote_gedruckt = (dataRow["Angebote_gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_gedruckt"]);
			//Angebote_In_Bearbeitung = (dataRow["Angebote_In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_In Bearbeitung"]);
			//Angebote_Interessent = (dataRow["Angebote_Interessent"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_Interessent"]);
			//Angebote_Konditionen = (dataRow["Angebote_Konditionen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Konditionen"]);
			//Angebote_Kunden_Nr = (dataRow["Angebote_Kunden-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_Kunden-Nr"]);
			//Angebote_LAbteilung = (dataRow["Angebote_LAbteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LAbteilung"]);
			//Angebote_LAnsprechpartner = (dataRow["Angebote_LAnsprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LAnsprechpartner"]);
			//Angebote_LBriefanrede = (dataRow["Angebote_LBriefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_LBriefanrede"]);
			//Angebote_Liefertermin = (dataRow["Angebote_Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebote_Liefertermin"]);
			//Angebote_Loschen = (dataRow["Angebote_Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_Löschen"]);
			//Angebote_Mahnung = (dataRow["Angebote_Mahnung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_Mahnung"]);
			//Angebote_Mandant = (dataRow["Angebote_Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Mandant"]);
			//Angebote_Neu = (dataRow["Angebote_Neu"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_Neu"]);
			//Angebote_Neu_Order = (dataRow["Angebote_Neu_Order"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_Neu_Order"]);
			//Angebote_nr_ang = (dataRow["Angebote_nr_ang"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_ang"]);
			//Angebote_nr_auf = (dataRow["Angebote_nr_auf"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_auf"]);
			//Angebote_nr_BV = (dataRow["Angebote_nr_BV"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_BV"]);
			//Angebote_nr_dlf = (dataRow["Angebote_nr_dlf"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_dlf"]);
			//Angebote_nr_gut = (dataRow["Angebote_nr_gut"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_gut"]);
			//Angebote_nr_Kanban = (dataRow["Angebote_nr_Kanban"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_Kanban"]);
			//Angebote_nr_lie = (dataRow["Angebote_nr_lie"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_lie"]);
			//Angebote_nr_pro = (dataRow["Angebote_nr_pro"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_pro"]);
			//Angebote_nr_RA = (dataRow["Angebote_nr_RA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_RA"]);
			//Angebote_nr_rec = (dataRow["Angebote_nr_rec"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_rec"]);
			//Angebote_nr_sto = (dataRow["Angebote_nr_sto"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_nr_sto"]);
			//Angebote_Offnen = (dataRow["Angebote_Öffnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_Öffnen"]);
			//Angebote_Personal_Nr = (dataRow["Angebote_Personal-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_Personal-Nr"]);
			//Angebote_Projekt_Nr = (dataRow["Angebote_Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Projekt-Nr"]);
			//Angebote_rec_sent = (dataRow["Angebote_rec_sent"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_rec_sent"]);
			//Angebote_reparatur_nr = (dataRow["Angebote_reparatur_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebote_reparatur_nr"]);
			//Angebote_Status = (dataRow["Angebote_Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Status"]);
			//Angebote_termin_eingehalten = (dataRow["Angebote_termin_eingehalten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_termin_eingehalten"]);
			//Angebote_Typ = (dataRow["Angebote_Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Typ"]);
			//Angebote_USt_Berechnen = (dataRow["Angebote_USt_Berechnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Angebote_USt_Berechnen"]);
			//Angebote_Versandarten_Auswahl = (dataRow["Angebote_Versandarten_Auswahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Versandarten_Auswahl"]);
			//Angebote_Wunschtermin = (dataRow["Angebote_Wunschtermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebote_Wunschtermin"]);
			//Angebote_Zahlungsweise = (dataRow["Angebote_Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Zahlungsweise"]);
			//Angebote_Zahlungsziel = (dataRow["Angebote_Zahlungsziel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebote_Zahlungsziel"]);
			//ANgeboteArtikel_AB_Pos_zu_BV_Pos = (dataRow["ANgeboteArtikel_AB Pos zu BV Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_AB Pos zu BV Pos"]);
			//ANgeboteArtikel_AB_Pos_zu_RA_Pos = (dataRow["ANgeboteArtikel_AB Pos zu RA Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_AB Pos zu RA Pos"]);
			//ANgeboteArtikel_Abladestelle = (dataRow["ANgeboteArtikel_Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Abladestelle"]);
			//ANgeboteArtikel_Aktuelle_Anzahl = (dataRow["ANgeboteArtikel_Aktuelle Anzahl"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Aktuelle Anzahl"]);
			//ANgeboteArtikel_AnfangLagerBestand = (dataRow["ANgeboteArtikel_AnfangLagerBestand"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_AnfangLagerBestand"]);
			//ANgeboteArtikel_Angebot_Nr = (dataRow["ANgeboteArtikel_Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_Angebot-Nr"]);
			ANgeboteArtikel_Anzahl = (dataRow["ANgeboteArtikel_Anzahl"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Anzahl"]);
			//ANgeboteArtikel_Artikel_Nr = (dataRow["ANgeboteArtikel_Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_Artikel-Nr"]);
			//ANgeboteArtikel_Auswahl = (dataRow["ANgeboteArtikel_Auswahl"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Auswahl"]);
			//ANgeboteArtikel_Bemerkungsfeld1 = (dataRow["ANgeboteArtikel_Bemerkungsfeld1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Bemerkungsfeld1"]);
			//ANgeboteArtikel_Bemerkungsfeld2 = (dataRow["ANgeboteArtikel_Bemerkungsfeld2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Bemerkungsfeld2"]);
			//ANgeboteArtikel_Bestellnummer = (dataRow["ANgeboteArtikel_Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Bestellnummer"]);
			//ANgeboteArtikel_CSInterneBemerkung = (dataRow["ANgeboteArtikel_CSInterneBemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_CSInterneBemerkung"]);
			//ANgeboteArtikel_DEL = (dataRow["ANgeboteArtikel_DEL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_DEL"]);
			//ANgeboteArtikel_DEL_fixiert = (dataRow["ANgeboteArtikel_DEL fixiert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_DEL fixiert"]);
			//ANgeboteArtikel_EDI_Historie_Nr = (dataRow["ANgeboteArtikel_EDI_Historie_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_EDI_Historie_Nr"]);
			//ANgeboteArtikel_EDI_PREIS_KUNDE = (dataRow["ANgeboteArtikel_EDI_PREIS_KUNDE"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_EDI_PREIS_KUNDE"]);
			//ANgeboteArtikel_EDI_PREISEINHEIT = (dataRow["ANgeboteArtikel_EDI_PREISEINHEIT"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_EDI_PREISEINHEIT"]);
			//ANgeboteArtikel_EDI_Quantity_Ordered = (dataRow["ANgeboteArtikel_EDI_Quantity_Ordered"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_EDI_Quantity_Ordered"]);
			//ANgeboteArtikel_Einzelkupferzuschlag = (dataRow["ANgeboteArtikel_Einzelkupferzuschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Einzelkupferzuschlag"]);
			//ANgeboteArtikel_Einzelpreis = (dataRow["ANgeboteArtikel_Einzelpreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Einzelpreis"]);
			//ANgeboteArtikel_EKPreise_Fix = (dataRow["ANgeboteArtikel_EKPreise_Fix"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_EKPreise_Fix"]);
			//ANgeboteArtikel_EndeLagerBestand = (dataRow["ANgeboteArtikel_EndeLagerBestand"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_EndeLagerBestand"]);
			//ANgeboteArtikel_erledigt_pos = (dataRow["ANgeboteArtikel_erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_erledigt_pos"]);
			//ANgeboteArtikel_Fertigungsnummer = (dataRow["ANgeboteArtikel_Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_Fertigungsnummer"]);
			//ANgeboteArtikel_FM = (dataRow["ANgeboteArtikel_FM"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_FM"]);
			//ANgeboteArtikel_FM_Einzelpreis = (dataRow["ANgeboteArtikel_FM_Einzelpreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_FM_Einzelpreis"]);
			//ANgeboteArtikel_FM_Gesamtpreis = (dataRow["ANgeboteArtikel_FM_Gesamtpreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_FM_Gesamtpreis"]);
			//ANgeboteArtikel_Freies_Format_EDI = (dataRow["ANgeboteArtikel_Freies_Format_EDI"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Freies_Format_EDI"]);
			//ANgeboteArtikel_Geliefert = (dataRow["ANgeboteArtikel_Geliefert"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Geliefert"]);
			//ANgeboteArtikel_Gepackt_von = (dataRow["ANgeboteArtikel_Gepackt_von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Gepackt_von"]);
			//ANgeboteArtikel_Gepackt_Zeitpunkt = (dataRow["ANgeboteArtikel_Gepackt_Zeitpunkt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ANgeboteArtikel_Gepackt_Zeitpunkt"]);
			ANgeboteArtikel_GesamtCu_Gewicht = (dataRow["ANgeboteArtikel_GesamtCu-Gewicht"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["ANgeboteArtikel_GesamtCu-Gewicht"]);
			//ANgeboteArtikel_Gesamtkupferzuschlag = (dataRow["ANgeboteArtikel_Gesamtkupferzuschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Gesamtkupferzuschlag"]);
			//ANgeboteArtikel_Gesamtpreis = (dataRow["ANgeboteArtikel_Gesamtpreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Gesamtpreis"]);
			//ANgeboteArtikel_GSExternComment = (dataRow["ANgeboteArtikel_GSExternComment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_GSExternComment"]);
			//ANgeboteArtikel_GSInternComment = (dataRow["ANgeboteArtikel_GSInternComment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_GSInternComment"]);
			//ANgeboteArtikel_GSWithoutCopper = (dataRow["ANgeboteArtikel_GSWithoutCopper"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_GSWithoutCopper"]);
			//ANgeboteArtikel_In_Bearbeitung = (dataRow["ANgeboteArtikel_In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_In Bearbeitung"]);
			//ANgeboteArtikel_Index_Kunde = (dataRow["ANgeboteArtikel_Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Index_Kunde"]);
			//ANgeboteArtikel_Index_Kunde_datum = (dataRow["ANgeboteArtikel_Index_Kunde_datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ANgeboteArtikel_Index_Kunde_datum"]);
			//ANgeboteArtikel_KB_Pos_zu_BV_Pos = (dataRow["ANgeboteArtikel_KB Pos zu BV Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_KB Pos zu BV Pos"]);
			//ANgeboteArtikel_KB_Pos_zu_RA_Pos = (dataRow["ANgeboteArtikel_KB Pos zu RA Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_KB Pos zu RA Pos"]);
			//ANgeboteArtikel_Kupferbasis = (dataRow["ANgeboteArtikel_Kupferbasis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_Kupferbasis"]);
			//ANgeboteArtikel_Lagerbewegung = (dataRow["ANgeboteArtikel_Lagerbewegung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Lagerbewegung"]);
			//ANgeboteArtikel_Lagerbewegung_ruckgangig = (dataRow["ANgeboteArtikel_Lagerbewegung_rückgängig"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Lagerbewegung_rückgängig"]);
			//ANgeboteArtikel_Lagerort_id = (dataRow["ANgeboteArtikel_Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_Lagerort_id"]);
			//ANgeboteArtikel_Langtext = (dataRow["ANgeboteArtikel_Langtext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Langtext"]);
			//ANgeboteArtikel_Langtext_drucken = (dataRow["ANgeboteArtikel_Langtext_drucken"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Langtext_drucken"]);
			//ANgeboteArtikel_Lieferanweisung__P_FTXDIN_TEXT_ = (dataRow["ANgeboteArtikel_Lieferanweisung (P_FTXDIN_TEXT)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Lieferanweisung (P_FTXDIN_TEXT)"]);
			//ANgeboteArtikel_Loschen = (dataRow["ANgeboteArtikel_Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Löschen"]);
			//ANgeboteArtikel_LS_Pos_zu_AB_Pos = (dataRow["ANgeboteArtikel_LS Pos zu AB Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_LS Pos zu AB Pos"]);
			//ANgeboteArtikel_LS_Pos_zu_KB_Pos = (dataRow["ANgeboteArtikel_LS Pos zu KB Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_LS Pos zu KB Pos"]);
			//ANgeboteArtikel_LS_von_Versand_gedruckt = (dataRow["ANgeboteArtikel_LS_von_Versand_gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_LS_von_Versand_gedruckt"]);
			//ANgeboteArtikel_Nr = Convert.ToInt32(dataRow["ANgeboteArtikel_Nr"]);
			//ANgeboteArtikel_OriginalAnzahl = (dataRow["ANgeboteArtikel_OriginalAnzahl"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_OriginalAnzahl"]);
			//ANgeboteArtikel_Packinfo_von_Lager = (dataRow["ANgeboteArtikel_Packinfo_von_Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Packinfo_von_Lager"]);
			//ANgeboteArtikel_Packstatus = (dataRow["ANgeboteArtikel_Packstatus"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Packstatus"]);
			//ANgeboteArtikel_Position = (dataRow["ANgeboteArtikel_Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_Position"]);
			//ANgeboteArtikel_PositionZUEDI = (dataRow["ANgeboteArtikel_PositionZUEDI"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_PositionZUEDI"]);
			//ANgeboteArtikel_POSTEXT = (dataRow["ANgeboteArtikel_POSTEXT"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_POSTEXT"]);
			//ANgeboteArtikel_Preis_ausweisen = (dataRow["ANgeboteArtikel_Preis_ausweisen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Preis_ausweisen"]);
			//ANgeboteArtikel_Preiseinheit = (dataRow["ANgeboteArtikel_Preiseinheit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Preiseinheit"]);
			//ANgeboteArtikel_Preisgruppe = (dataRow["ANgeboteArtikel_Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_Preisgruppe"]);
			//ANgeboteArtikel_RA_Pos_zu_BV_Pos = (dataRow["ANgeboteArtikel_RA Pos zu BV Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_RA Pos zu BV Pos"]);
			//ANgeboteArtikel_RA_Abgerufen = (dataRow["ANgeboteArtikel_RA_Abgerufen"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_RA_Abgerufen"]);
			//ANgeboteArtikel_RA_Offen = (dataRow["ANgeboteArtikel_RA_Offen"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_RA_Offen"]);
			//ANgeboteArtikel_RA_OriginalAnzahl = (dataRow["ANgeboteArtikel_RA_OriginalAnzahl"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_RA_OriginalAnzahl"]);
			//ANgeboteArtikel_Rabatt = (dataRow["ANgeboteArtikel_Rabatt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["ANgeboteArtikel_Rabatt"]);
			//ANgeboteArtikel_RE_Pos_zu_GS_Pos = (dataRow["ANgeboteArtikel_RE Pos zu GS Pos"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_RE Pos zu GS Pos"]);
			//ANgeboteArtikel_RP = (dataRow["ANgeboteArtikel_RP"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_RP"]);
			//ANgeboteArtikel_schriftart = (dataRow["ANgeboteArtikel_schriftart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_schriftart"]);
			//ANgeboteArtikel_Seriennummern_drucken = (dataRow["ANgeboteArtikel_Seriennummern_drucken"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Seriennummern_drucken"]);
			//ANgeboteArtikel_sortierung = (dataRow["ANgeboteArtikel_sortierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_sortierung"]);
			//ANgeboteArtikel_Stuckliste = (dataRow["ANgeboteArtikel_Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Stückliste"]);
			//ANgeboteArtikel_Stuckliste_drucken = (dataRow["ANgeboteArtikel_Stückliste_drucken"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Stückliste_drucken"]);
			//ANgeboteArtikel_Summenberechnung = (dataRow["ANgeboteArtikel_Summenberechnung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Summenberechnung"]);
			//ANgeboteArtikel_termin_eingehalten = (dataRow["ANgeboteArtikel_termin_eingehalten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_termin_eingehalten"]);
			//ANgeboteArtikel_Typ = (dataRow["ANgeboteArtikel_Typ"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ANgeboteArtikel_Typ"]);
			//ANgeboteArtikel_USt = (dataRow["ANgeboteArtikel_USt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_USt"]);
			//ANgeboteArtikel_VDA_gedruckt = (dataRow["ANgeboteArtikel_VDA_gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_VDA_gedruckt"]);
			//ANgeboteArtikel_Versand_gedruckt = (dataRow["ANgeboteArtikel_Versand_gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Versand_gedruckt"]);
			//ANgeboteArtikel_Versandarten_Auswahl = (dataRow["ANgeboteArtikel_Versandarten_Auswahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Versandarten_Auswahl"]);
			//ANgeboteArtikel_Versanddatum_Auswahl = (dataRow["ANgeboteArtikel_Versanddatum_Auswahl"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ANgeboteArtikel_Versanddatum_Auswahl"]);
			//ANgeboteArtikel_Versanddienstleister = (dataRow["ANgeboteArtikel_Versanddienstleister"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Versanddienstleister"]);
			//ANgeboteArtikel_Versandinfo_von_CS = (dataRow["ANgeboteArtikel_Versandinfo_von_CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Versandinfo_von_CS"]);
			//ANgeboteArtikel_Versandinfo_von_Lager = (dataRow["ANgeboteArtikel_Versandinfo_von_Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Versandinfo_von_Lager"]);
			//ANgeboteArtikel_Versandnummer = (dataRow["ANgeboteArtikel_Versandnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Versandnummer"]);
			//ANgeboteArtikel_Versandstatus = (dataRow["ANgeboteArtikel_Versandstatus"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_Versandstatus"]);
			//ANgeboteArtikel_VKEinzelpreis = (dataRow["ANgeboteArtikel_VKEinzelpreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_VKEinzelpreis"]);
			//ANgeboteArtikel_VK_Festpreis = (dataRow["ANgeboteArtikel_VK-Festpreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ANgeboteArtikel_VK-Festpreis"]);
			//ANgeboteArtikel_VKGesamtpreis = (dataRow["ANgeboteArtikel_VKGesamtpreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_VKGesamtpreis"]);
			//ANgeboteArtikel_Wunschtermin = (dataRow["ANgeboteArtikel_Wunschtermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ANgeboteArtikel_Wunschtermin"]);
			//ANgeboteArtikel_Zeichnungsnummer = (dataRow["ANgeboteArtikel_Zeichnungsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_Zeichnungsnummer"]);
			//ANgeboteArtikel_Zuschlag_VK = (dataRow["ANgeboteArtikel_Zuschlag_VK"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ANgeboteArtikel_Zuschlag_VK"]);
			//ANgeboteArtikel_zwischensumme = (dataRow["ANgeboteArtikel_zwischensumme"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ANgeboteArtikel_zwischensumme"]);
			//Auftragsbestatigung = (dataRow["Auftragsbestätigung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Auftragsbestätigung"]);
			//Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			//Gesamtgewicht = (dataRow["Gesamtgewicht"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Gesamtgewicht"]);
			//Grosse = (dataRow["Größe"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Größe"]);
			//Gutschrift = (dataRow["Gutschrift"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gutschrift"]);
			Lieferschein = (dataRow["Lieferschein"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferschein"]);
			//LS = (dataRow["LS"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LS"]);
			//Nettotage = (dataRow["Nettotage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nettotage"]);
			//Nr = Convert.ToInt32(dataRow["Nr"]);
			//Rechnung = (dataRow["Rechnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rechnung"]);
			//Skonto = (dataRow["Skonto"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Skonto"]);
			//Skontotage = (dataRow["Skontotage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Skontotage"]);
		}

	}
}
