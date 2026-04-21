using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Models.OrderProcessing
{
	public class DeliveryNotesCompilationRequestModel
	{
		public int CustomerNumber { get; set; }
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
	}
	public class DeliveryNotesCompilationResponseModel
	{
		public string Anrede { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Street { get; set; }
		public string Country { get; set; }
		public string LAnrede { get; set; }
		public string LName1 { get; set; }
		public string LName2 { get; set; }
		public string LStreet { get; set; }
		public string LCountry { get; set; }
		public string MessageHeader { get; set; }
		public string DocumentTitle { get; set; }
		public string DocumentHeaderText { get; set; }

		// -
		public string CustomerNumber { get; set; }
		public string ShippingMethod { get; set; }
		public string VAT_ID { get; set; }
		public string PosText { get; set; }
		public int Angebote_Unser_Zeichen { get; set; }

		// 
		public List<DeliveryNotesCompilationResponseItemModel> Items { get; set; }
		public DeliveryNotesCompilationResponseModel(List<Infrastructure.Data.Entities.Joins.CTS.DeliveryNotesCompilationEntity> compilationEntities)
		{
			if(compilationEntities?.Count <= 0)
				return;
			// -
			Anrede = compilationEntities[0].Angebote_Anrede;
			Name1 = compilationEntities[0].Angebote_Vorname_NameFirma;
			Name2 = compilationEntities[0].Angebote_Name2;
			Street = compilationEntities[0].Angebote_Strasse_Postfach;
			Country = compilationEntities[0].Angebote_Land_PLZ_Ort;

			LAnrede = compilationEntities[0].Angebote_LAnrede;
			LName1 = compilationEntities[0].Angebote_LVorname_NameFirma;
			LName2 = compilationEntities[0].Angebote_LName2;
			LStreet = compilationEntities[0].Angebote_LStrasse_Postfach;
			LCountry = compilationEntities[0].Angebote_LLand_PLZ_Ort;


			CustomerNumber = compilationEntities[0].Angebote_Ihr_Zeichen;
			ShippingMethod = compilationEntities[0].Angebote_Versandart;
			VAT_ID = compilationEntities[0].Angebote_Freitext;

			DocumentTitle = "Lieferscheine - Zusammenstellung";
			MessageHeader = compilationEntities[0].Lieferschein;
			//-
			PosText = "Wir liefern Ihnen folgende Posten:";

			Angebote_Unser_Zeichen = int.TryParse(compilationEntities[0].Angebote_Unser_Zeichen, out var v) ? v : 0;

			// -
			Items = compilationEntities.Select(x => new DeliveryNotesCompilationResponseItemModel(x)).ToList();
		}
	}
	public class DeliveryNotesCompilationResponseItemModel
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


		//public int? Angebote_ab_id { get; set; }
		//public string Angebote_ABSENDER { get; set; }
		//public string Angebote_Abteilung { get; set; }
		//public string Angebote_Anrede { get; set; }
		//public string Angebote_Ansprechpartner { get; set; }
		//public bool? Angebote_Auswahl { get; set; }
		//public int? Angebote_Belegkreis { get; set; }
		//public string Angebote_Bemerkung { get; set; }
		//public string Angebote_Benutzer { get; set; }
		//public string Angebote_Bereich { get; set; }
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
		//public string Angebote_Freitext { get; set; }
		//public bool? Angebote_gebucht { get; set; }
		//public bool? Angebote_gedruckt { get; set; }
		//public string Angebote_Ihr_Zeichen { get; set; }
		//public bool? Angebote_In_Bearbeitung { get; set; }
		//public bool? Angebote_Interessent { get; set; }
		//public string Angebote_Konditionen { get; set; }
		//public int? Angebote_Kunden_Nr { get; set; }
		//public string Angebote_LAbteilung { get; set; }
		//public string Angebote_Land_PLZ_Ort { get; set; }
		//public string Angebote_LAnrede { get; set; }
		//public string Angebote_LAnsprechpartner { get; set; }
		//public string Angebote_LBriefanrede { get; set; }
		//public int? Angebote_Lieferadresse { get; set; }
		//public DateTime? Angebote_Liefertermin { get; set; }
		//public string Angebote_LLand_PLZ_Ort { get; set; }
		//public string Angebote_LName2 { get; set; }
		//public string Angebote_LName3 { get; set; }
		//public bool? Angebote_Loschen { get; set; }
		//public string Angebote_LStrasse_Postfach { get; set; }
		//public string Angebote_LVorname_NameFirma { get; set; }
		//public bool? Angebote_Mahnung { get; set; }
		//public string Angebote_Mandant { get; set; }
		//public string Angebote_Name2 { get; set; }
		//public string Angebote_Name3 { get; set; }
		//public bool? Angebote_Neu { get; set; }
		//public bool? Angebote_Neu_Order { get; set; }
		//public int Angebote_Nr { get; set; }
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
		//public string Angebote_Strasse_Postfach { get; set; }
		//public bool? Angebote_termin_eingehalten { get; set; }
		//public string Angebote_Typ { get; set; }
		//public string Angebote_Unser_Zeichen { get; set; }
		//public bool? Angebote_USt_Berechnen { get; set; }
		//public string Angebote_Versandart { get; set; }
		//public string Angebote_Versandarten_Auswahl { get; set; }
		//public string Angebote_Vorname_NameFirma { get; set; }
		//public DateTime? Angebote_Wunschtermin { get; set; }
		//public string Angebote_Zahlungsweise { get; set; }
		//public string Angebote_Zahlungsziel { get; set; }
		//public int? ANgeboteArtikel_AB_Pos_zu_BV_Pos { get; set; }
		//public int? ANgeboteArtikel_AB_Pos_zu_RA_Pos { get; set; }
		//public string ANgeboteArtikel_Abladestelle { get; set; }
		//public double? ANgeboteArtikel_Aktuelle_Anzahl { get; set; }
		//public double? ANgeboteArtikel_AnfangLagerBestand { get; set; }
		//public int? ANgeboteArtikel_Angebot_Nr { get; set; }
		//public int? ANgeboteArtikel_Artikel_Nr { get; set; }
		//public bool? ANgeboteArtikel_Auswahl { get; set; }
		//public string ANgeboteArtikel_Bemerkungsfeld1 { get; set; }
		//public string ANgeboteArtikel_Bemerkungsfeld2 { get; set; }
		//public string ANgeboteArtikel_Bestellnummer { get; set; }
		//public string ANgeboteArtikel_CSInterneBemerkung { get; set; }
		//public int? ANgeboteArtikel_DEL { get; set; }
		//public bool? ANgeboteArtikel_DEL_fixiert { get; set; }
		//public int? ANgeboteArtikel_EDI_Historie_Nr { get; set; }
		//public double? ANgeboteArtikel_EDI_PREIS_KUNDE { get; set; }
		//public double? ANgeboteArtikel_EDI_PREISEINHEIT { get; set; }
		//public double? ANgeboteArtikel_EDI_Quantity_Ordered { get; set; }
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
		//public string Auftragsbestatigung { get; set; }
		//public string Fax { get; set; }
		//public double? Gesamtgewicht { get; set; }
		//public Single? Grosse { get; set; }
		//public string Gutschrift { get; set; }
		//public string Lieferschein { get; set; }
		//public int? LS { get; set; }
		//public int? Nettotage { get; set; }
		//public int Nr { get; set; }
		//public string Rechnung { get; set; }
		//public Single? Skonto { get; set; }
		//public int? Skontotage { get; set; }
		public DeliveryNotesCompilationResponseItemModel(Infrastructure.Data.Entities.Joins.CTS.DeliveryNotesCompilationEntity compilationEntity)
		{
			if(compilationEntity == null)
				return;

			// -
			//Angebote_ab_id = compilationEntity.Angebote_ab_id;
			//Angebote_ABSENDER = compilationEntity.Angebote_ABSENDER;
			//Angebote_Abteilung = compilationEntity.Angebote_Abteilung;
			Angebote_Angebot_Nr = compilationEntity.Angebote_Angebot_Nr;
			//Angebote_Anrede = compilationEntity.Angebote_Anrede;
			//Angebote_Ansprechpartner = compilationEntity.Angebote_Ansprechpartner;
			//Angebote_Auswahl = compilationEntity.Angebote_Auswahl;
			//Angebote_Belegkreis = compilationEntity.Angebote_Belegkreis;
			//Angebote_Bemerkung = compilationEntity.Angebote_Bemerkung;
			//Angebote_Benutzer = compilationEntity.Angebote_Benutzer;
			//Angebote_Bereich = compilationEntity.Angebote_Bereich;
			Angebote_Bezug = compilationEntity.Angebote_Bezug;
			//Angebote_Briefanrede = compilationEntity.Angebote_Briefanrede;
			//Angebote_datueber = compilationEntity.Angebote_datueber;
			//Angebote_Datum = compilationEntity.Angebote_Datum;
			//Angebote_Debitorennummer = compilationEntity.Angebote_Debitorennummer;
			//Angebote_Dplatz_Sirona = compilationEntity.Angebote_Dplatz_Sirona;
			//Angebote_EDI_Dateiname_CSV = compilationEntity.Angebote_EDI_Dateiname_CSV;
			//Angebote_EDI_Kundenbestellnummer = compilationEntity.Angebote_EDI_Kundenbestellnummer;
			//Angebote_EDI_Order_Change = compilationEntity.Angebote_EDI_Order_Change;
			//Angebote_EDI_Order_Change_Updated = compilationEntity.Angebote_EDI_Order_Change_Updated;
			//Angebote_EDI_Order_Neu = compilationEntity.Angebote_EDI_Order_Neu;
			//Angebote_erledigt = compilationEntity.Angebote_erledigt;
			//Angebote_Falligkeit = compilationEntity.Angebote_Falligkeit;
			//Angebote_Freie_Text = compilationEntity.Angebote_Freie_Text;
			//Angebote_Freitext = compilationEntity.Angebote_Freitext;
			//Angebote_gebucht = compilationEntity.Angebote_gebucht;
			//Angebote_gedruckt = compilationEntity.Angebote_gedruckt;
			//Angebote_Ihr_Zeichen = compilationEntity.Angebote_Ihr_Zeichen;
			//Angebote_In_Bearbeitung = compilationEntity.Angebote_In_Bearbeitung;
			//Angebote_Interessent = compilationEntity.Angebote_Interessent;
			//Angebote_Konditionen = compilationEntity.Angebote_Konditionen;
			//Angebote_Kunden_Nr = compilationEntity.Angebote_Kunden_Nr;
			//Angebote_LAbteilung = compilationEntity.Angebote_LAbteilung;
			//Angebote_Land_PLZ_Ort = compilationEntity.Angebote_Land_PLZ_Ort;
			//Angebote_LAnrede = compilationEntity.Angebote_LAnrede;
			//Angebote_LAnsprechpartner = compilationEntity.Angebote_LAnsprechpartner;
			//Angebote_LBriefanrede = compilationEntity.Angebote_LBriefanrede;
			//Angebote_Lieferadresse = compilationEntity.Angebote_Lieferadresse;
			//Angebote_Liefertermin = compilationEntity.Angebote_Liefertermin;
			//Angebote_LLand_PLZ_Ort = compilationEntity.Angebote_LLand_PLZ_Ort;
			//Angebote_LName2 = compilationEntity.Angebote_LName2;
			//Angebote_LName3 = compilationEntity.Angebote_LName3;
			//Angebote_Loschen = compilationEntity.Angebote_Loschen;
			//Angebote_LStrasse_Postfach = compilationEntity.Angebote_LStrasse_Postfach;
			//Angebote_LVorname_NameFirma = compilationEntity.Angebote_LVorname_NameFirma;
			//Angebote_Mahnung = compilationEntity.Angebote_Mahnung;
			//Angebote_Mandant = compilationEntity.Angebote_Mandant;
			//Angebote_Name2 = compilationEntity.Angebote_Name2;
			//Angebote_Name3 = compilationEntity.Angebote_Name3;
			//Angebote_Neu = compilationEntity.Angebote_Neu;
			//Angebote_Neu_Order = compilationEntity.Angebote_Neu_Order;
			//Angebote_Nr = compilationEntity.Angebote_Nr;
			//Angebote_nr_ang = compilationEntity.Angebote_nr_ang;
			//Angebote_nr_auf = compilationEntity.Angebote_nr_auf;
			//Angebote_nr_BV = compilationEntity.Angebote_nr_BV;
			//Angebote_nr_dlf = compilationEntity.Angebote_nr_dlf;
			//Angebote_nr_gut = compilationEntity.Angebote_nr_gut;
			//Angebote_nr_Kanban = compilationEntity.Angebote_nr_Kanban;
			//Angebote_nr_lie = compilationEntity.Angebote_nr_lie;
			//Angebote_nr_pro = compilationEntity.Angebote_nr_pro;
			//Angebote_nr_RA = compilationEntity.Angebote_nr_RA;
			//Angebote_nr_rec = compilationEntity.Angebote_nr_rec;
			//Angebote_nr_sto = compilationEntity.Angebote_nr_sto;
			//Angebote_Offnen = compilationEntity.Angebote_Offnen;
			//Angebote_Personal_Nr = compilationEntity.Angebote_Personal_Nr;
			//Angebote_Projekt_Nr = compilationEntity.Angebote_Projekt_Nr;
			//Angebote_rec_sent = compilationEntity.Angebote_rec_sent;
			//Angebote_reparatur_nr = compilationEntity.Angebote_reparatur_nr;
			//Angebote_Status = compilationEntity.Angebote_Status;
			//Angebote_Strasse_Postfach = compilationEntity.Angebote_Strasse_Postfach;
			//Angebote_termin_eingehalten = compilationEntity.Angebote_termin_eingehalten;
			//Angebote_Typ = compilationEntity.Angebote_Typ;
			//Angebote_Unser_Zeichen = compilationEntity.Angebote_Unser_Zeichen;
			//Angebote_USt_Berechnen = compilationEntity.Angebote_USt_Berechnen;
			//Angebote_Versandart = compilationEntity.Angebote_Versandart;
			//Angebote_Versandarten_Auswahl = compilationEntity.Angebote_Versandarten_Auswahl;
			//Angebote_Vorname_NameFirma = compilationEntity.Angebote_Vorname_NameFirma;
			//Angebote_Wunschtermin = compilationEntity.Angebote_Wunschtermin;
			//Angebote_Zahlungsweise = compilationEntity.Angebote_Zahlungsweise;
			//Angebote_Zahlungsziel = compilationEntity.Angebote_Zahlungsziel;
			//ANgeboteArtikel_AB_Pos_zu_BV_Pos = compilationEntity.ANgeboteArtikel_AB_Pos_zu_BV_Pos;
			//ANgeboteArtikel_AB_Pos_zu_RA_Pos = compilationEntity.ANgeboteArtikel_AB_Pos_zu_RA_Pos;
			//ANgeboteArtikel_Abladestelle = compilationEntity.ANgeboteArtikel_Abladestelle;
			//ANgeboteArtikel_Aktuelle_Anzahl = compilationEntity.ANgeboteArtikel_Aktuelle_Anzahl;
			//ANgeboteArtikel_AnfangLagerBestand = compilationEntity.ANgeboteArtikel_AnfangLagerBestand;
			//ANgeboteArtikel_Angebot_Nr = compilationEntity.ANgeboteArtikel_Angebot_Nr;
			ANgeboteArtikel_Anzahl = compilationEntity.ANgeboteArtikel_Anzahl;
			//ANgeboteArtikel_Artikel_Nr = compilationEntity.ANgeboteArtikel_Artikel_Nr;
			//ANgeboteArtikel_Auswahl = compilationEntity.ANgeboteArtikel_Auswahl;
			//ANgeboteArtikel_Bemerkungsfeld1 = compilationEntity.ANgeboteArtikel_Bemerkungsfeld1;
			//ANgeboteArtikel_Bemerkungsfeld2 = compilationEntity.ANgeboteArtikel_Bemerkungsfeld2;
			//ANgeboteArtikel_Bestellnummer = compilationEntity.ANgeboteArtikel_Bestellnummer;
			ANgeboteArtikel_Bezeichnung1 = compilationEntity.ANgeboteArtikel_Bezeichnung1;
			ANgeboteArtikel_Bezeichnung2 = compilationEntity.ANgeboteArtikel_Bezeichnung2;
			ANgeboteArtikel_Bezeichnung2_Kunde = compilationEntity.ANgeboteArtikel_Bezeichnung2_Kunde;
			ANgeboteArtikel_Bezeichnung3 = compilationEntity.ANgeboteArtikel_Bezeichnung3;
			//ANgeboteArtikel_CSInterneBemerkung = compilationEntity.ANgeboteArtikel_CSInterneBemerkung;
			//ANgeboteArtikel_DEL = compilationEntity.ANgeboteArtikel_DEL;
			//ANgeboteArtikel_DEL_fixiert = compilationEntity.ANgeboteArtikel_DEL_fixiert;
			//ANgeboteArtikel_EDI_Historie_Nr = compilationEntity.ANgeboteArtikel_EDI_Historie_Nr;
			//ANgeboteArtikel_EDI_PREIS_KUNDE = compilationEntity.ANgeboteArtikel_EDI_PREIS_KUNDE;
			//ANgeboteArtikel_EDI_PREISEINHEIT = compilationEntity.ANgeboteArtikel_EDI_PREISEINHEIT;
			//ANgeboteArtikel_EDI_Quantity_Ordered = compilationEntity.ANgeboteArtikel_EDI_Quantity_Ordered;
			ANgeboteArtikel_Einheit = compilationEntity.ANgeboteArtikel_Einheit;
			ANgeboteArtikel_EinzelCu_Gewicht = compilationEntity.ANgeboteArtikel_EinzelCu_Gewicht;
			//ANgeboteArtikel_Einzelkupferzuschlag = compilationEntity.ANgeboteArtikel_Einzelkupferzuschlag;
			//ANgeboteArtikel_Einzelpreis = compilationEntity.ANgeboteArtikel_Einzelpreis;
			//ANgeboteArtikel_EKPreise_Fix = compilationEntity.ANgeboteArtikel_EKPreise_Fix;
			//ANgeboteArtikel_EndeLagerBestand = compilationEntity.ANgeboteArtikel_EndeLagerBestand;
			//ANgeboteArtikel_erledigt_pos = compilationEntity.ANgeboteArtikel_erledigt_pos;
			//ANgeboteArtikel_Fertigungsnummer = compilationEntity.ANgeboteArtikel_Fertigungsnummer;
			//ANgeboteArtikel_FM = compilationEntity.ANgeboteArtikel_FM;
			//ANgeboteArtikel_FM_Einzelpreis = compilationEntity.ANgeboteArtikel_FM_Einzelpreis;
			//ANgeboteArtikel_FM_Gesamtpreis = compilationEntity.ANgeboteArtikel_FM_Gesamtpreis;
			//ANgeboteArtikel_Freies_Format_EDI = compilationEntity.ANgeboteArtikel_Freies_Format_EDI;
			//ANgeboteArtikel_Geliefert = compilationEntity.ANgeboteArtikel_Geliefert;
			//ANgeboteArtikel_Gepackt_von = compilationEntity.ANgeboteArtikel_Gepackt_von;
			//ANgeboteArtikel_Gepackt_Zeitpunkt = compilationEntity.ANgeboteArtikel_Gepackt_Zeitpunkt;
			ANgeboteArtikel_GesamtCu_Gewicht = compilationEntity.ANgeboteArtikel_GesamtCu_Gewicht;
			//ANgeboteArtikel_Gesamtkupferzuschlag = compilationEntity.ANgeboteArtikel_Gesamtkupferzuschlag;
			//ANgeboteArtikel_Gesamtpreis = compilationEntity.ANgeboteArtikel_Gesamtpreis;
			//ANgeboteArtikel_GSExternComment = compilationEntity.ANgeboteArtikel_GSExternComment;
			//ANgeboteArtikel_GSInternComment = compilationEntity.ANgeboteArtikel_GSInternComment;
			//ANgeboteArtikel_GSWithoutCopper = compilationEntity.ANgeboteArtikel_GSWithoutCopper;
			//ANgeboteArtikel_In_Bearbeitung = compilationEntity.ANgeboteArtikel_In_Bearbeitung;
			//ANgeboteArtikel_Index_Kunde = compilationEntity.ANgeboteArtikel_Index_Kunde;
			//ANgeboteArtikel_Index_Kunde_datum = compilationEntity.ANgeboteArtikel_Index_Kunde_datum;
			//ANgeboteArtikel_KB_Pos_zu_BV_Pos = compilationEntity.ANgeboteArtikel_KB_Pos_zu_BV_Pos;
			//ANgeboteArtikel_KB_Pos_zu_RA_Pos = compilationEntity.ANgeboteArtikel_KB_Pos_zu_RA_Pos;
			//ANgeboteArtikel_Kupferbasis = compilationEntity.ANgeboteArtikel_Kupferbasis;
			//ANgeboteArtikel_Lagerbewegung = compilationEntity.ANgeboteArtikel_Lagerbewegung;
			//ANgeboteArtikel_Lagerbewegung_ruckgangig = compilationEntity.ANgeboteArtikel_Lagerbewegung_ruckgangig;
			//ANgeboteArtikel_Lagerort_id = compilationEntity.ANgeboteArtikel_Lagerort_id;
			//ANgeboteArtikel_Langtext = compilationEntity.ANgeboteArtikel_Langtext;
			//ANgeboteArtikel_Langtext_drucken = compilationEntity.ANgeboteArtikel_Langtext_drucken;
			//ANgeboteArtikel_Lieferanweisung__P_FTXDIN_TEXT_ = compilationEntity.ANgeboteArtikel_Lieferanweisung__P_FTXDIN_TEXT_;
			ANgeboteArtikel_Liefertermin = compilationEntity.ANgeboteArtikel_Liefertermin;
			//ANgeboteArtikel_Loschen = compilationEntity.ANgeboteArtikel_Loschen;
			//ANgeboteArtikel_LS_Pos_zu_AB_Pos = compilationEntity.ANgeboteArtikel_LS_Pos_zu_AB_Pos;
			//ANgeboteArtikel_LS_Pos_zu_KB_Pos = compilationEntity.ANgeboteArtikel_LS_Pos_zu_KB_Pos;
			//ANgeboteArtikel_LS_von_Versand_gedruckt = compilationEntity.ANgeboteArtikel_LS_von_Versand_gedruckt;
			//ANgeboteArtikel_Nr = compilationEntity.ANgeboteArtikel_Nr;
			//ANgeboteArtikel_OriginalAnzahl = compilationEntity.ANgeboteArtikel_OriginalAnzahl;
			//ANgeboteArtikel_Packinfo_von_Lager = compilationEntity.ANgeboteArtikel_Packinfo_von_Lager;
			//ANgeboteArtikel_Packstatus = compilationEntity.ANgeboteArtikel_Packstatus;
			//ANgeboteArtikel_Position = compilationEntity.ANgeboteArtikel_Position;
			//ANgeboteArtikel_PositionZUEDI = compilationEntity.ANgeboteArtikel_PositionZUEDI;
			//ANgeboteArtikel_POSTEXT = compilationEntity.ANgeboteArtikel_POSTEXT;
			//ANgeboteArtikel_Preis_ausweisen = compilationEntity.ANgeboteArtikel_Preis_ausweisen;
			//ANgeboteArtikel_Preiseinheit = compilationEntity.ANgeboteArtikel_Preiseinheit;
			//ANgeboteArtikel_Preisgruppe = compilationEntity.ANgeboteArtikel_Preisgruppe;
			//ANgeboteArtikel_RA_Pos_zu_BV_Pos = compilationEntity.ANgeboteArtikel_RA_Pos_zu_BV_Pos;
			//ANgeboteArtikel_RA_Abgerufen = compilationEntity.ANgeboteArtikel_RA_Abgerufen;
			//ANgeboteArtikel_RA_Offen = compilationEntity.ANgeboteArtikel_RA_Offen;
			//ANgeboteArtikel_RA_OriginalAnzahl = compilationEntity.ANgeboteArtikel_RA_OriginalAnzahl;
			//ANgeboteArtikel_Rabatt = compilationEntity.ANgeboteArtikel_Rabatt;
			//ANgeboteArtikel_RE_Pos_zu_GS_Pos = compilationEntity.ANgeboteArtikel_RE_Pos_zu_GS_Pos;
			//ANgeboteArtikel_RP = compilationEntity.ANgeboteArtikel_RP;
			//ANgeboteArtikel_schriftart = compilationEntity.ANgeboteArtikel_schriftart;
			//ANgeboteArtikel_Seriennummern_drucken = compilationEntity.ANgeboteArtikel_Seriennummern_drucken;
			//ANgeboteArtikel_sortierung = compilationEntity.ANgeboteArtikel_sortierung;
			//ANgeboteArtikel_Stuckliste = compilationEntity.ANgeboteArtikel_Stuckliste;
			//ANgeboteArtikel_Stuckliste_drucken = compilationEntity.ANgeboteArtikel_Stuckliste_drucken;
			//ANgeboteArtikel_Summenberechnung = compilationEntity.ANgeboteArtikel_Summenberechnung;
			//ANgeboteArtikel_termin_eingehalten = compilationEntity.ANgeboteArtikel_termin_eingehalten;
			//ANgeboteArtikel_Typ = compilationEntity.ANgeboteArtikel_Typ;
			//ANgeboteArtikel_USt = compilationEntity.ANgeboteArtikel_USt;
			//ANgeboteArtikel_VDA_gedruckt = compilationEntity.ANgeboteArtikel_VDA_gedruckt;
			//ANgeboteArtikel_Versand_gedruckt = compilationEntity.ANgeboteArtikel_Versand_gedruckt;
			//ANgeboteArtikel_Versandarten_Auswahl = compilationEntity.ANgeboteArtikel_Versandarten_Auswahl;
			//ANgeboteArtikel_Versanddatum_Auswahl = compilationEntity.ANgeboteArtikel_Versanddatum_Auswahl;
			//ANgeboteArtikel_Versanddienstleister = compilationEntity.ANgeboteArtikel_Versanddienstleister;
			//ANgeboteArtikel_Versandinfo_von_CS = compilationEntity.ANgeboteArtikel_Versandinfo_von_CS;
			//ANgeboteArtikel_Versandinfo_von_Lager = compilationEntity.ANgeboteArtikel_Versandinfo_von_Lager;
			//ANgeboteArtikel_Versandnummer = compilationEntity.ANgeboteArtikel_Versandnummer;
			//ANgeboteArtikel_Versandstatus = compilationEntity.ANgeboteArtikel_Versandstatus;
			//ANgeboteArtikel_VKEinzelpreis = compilationEntity.ANgeboteArtikel_VKEinzelpreis;
			//ANgeboteArtikel_VK_Festpreis = compilationEntity.ANgeboteArtikel_VK_Festpreis;
			//ANgeboteArtikel_VKGesamtpreis = compilationEntity.ANgeboteArtikel_VKGesamtpreis;
			//ANgeboteArtikel_Wunschtermin = compilationEntity.ANgeboteArtikel_Wunschtermin;
			//ANgeboteArtikel_Zeichnungsnummer = compilationEntity.ANgeboteArtikel_Zeichnungsnummer;
			//ANgeboteArtikel_Zuschlag_VK = compilationEntity.ANgeboteArtikel_Zuschlag_VK;
			//ANgeboteArtikel_zwischensumme = compilationEntity.ANgeboteArtikel_zwischensumme;
			Artikelnummer = compilationEntity.Artikelnummer;
			//Auftragsbestatigung = compilationEntity.Auftragsbestatigung;
			//Fax = compilationEntity.Fax;
			//Gesamtgewicht = compilationEntity.Gesamtgewicht;
			//Grosse = compilationEntity.Grosse;
			//Gutschrift = compilationEntity.Gutschrift;
			//Lieferschein = compilationEntity.Lieferschein;
			//LS = compilationEntity.LS;
			//Nettotage = compilationEntity.Nettotage;
			//Nr = compilationEntity.Nr;
			//Rechnung = compilationEntity.Rechnung;
			//Skonto = compilationEntity.Skonto;
			//Skontotage = compilationEntity.Skontotage;
			Ursprungsland = compilationEntity.Ursprungsland;
			Zolltarif_nr = compilationEntity.Zolltarif_nr;
		}
	}
}
