using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class AngeboteEntity
	{
		public int? ab_id { get; set; }
		public string ABSENDER { get; set; }
		public string Abteilung { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Anrede { get; set; }
		public string Ansprechpartner { get; set; }
		public bool? Auswahl { get; set; }
		public int? Belegkreis { get; set; }
		public string Bemerkung { get; set; }
		public string Benutzer { get; set; }
		public string Bereich { get; set; }
		public string Bezug { get; set; }
		public string Briefanrede { get; set; }
		public bool? datueber { get; set; }
		public DateTime? Datum { get; set; }
		public string Debitorennummer { get; set; }
		public string Dplatz_Sirona { get; set; }
		public string EDI_Dateiname_CSV { get; set; }
		public string EDI_Kundenbestellnummer { get; set; }
		public bool? EDI_Order_Change { get; set; }
		public bool? EDI_Order_Change_Updated { get; set; }
		public bool? EDI_Order_Neu { get; set; }
		public bool? erledigt { get; set; }
		public DateTime? Falligkeit { get; set; }
		public string Freie_Text { get; set; }
		public string Freitext { get; set; }
		public bool? gebucht { get; set; }
		public bool? gedruckt { get; set; }
		public string Ihr_Zeichen { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public bool? Interessent { get; set; }
		public string Konditionen { get; set; }
		public int? Kunden_Nr { get; set; }
		public string LAbteilung { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public string LAnrede { get; set; }
		public string LAnsprechpartner { get; set; }
		public string LBriefanrede { get; set; }
		public int? Lieferadresse { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string LLand_PLZ_Ort { get; set; }
		public string LName2 { get; set; }
		public string LName3 { get; set; }
		public bool? Loschen { get; set; }
		public string LStrasse_Postfach { get; set; }
		public string LVorname_NameFirma { get; set; }
		public bool? Mahnung { get; set; }
		public string Mandant { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public bool? Neu { get; set; }
		public bool? Neu_Order { get; set; }
		public int Nr { get; set; }
		public int? nr_ang { get; set; }
		public int? nr_auf { get; set; }
		public int? nr_BV { get; set; }
		public int? nr_gut { get; set; }
		public int? nr_Kanban { get; set; }
		public int? nr_lie { get; set; }
		public int? nr_pro { get; set; }
		public int? nr_RA { get; set; }
		public int? nr_rec { get; set; }
		public int? nr_sto { get; set; }
		public bool? Offnen { get; set; }
		public int? Personal_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public int? reparatur_nr { get; set; }
		public string Status { get; set; }
		public string Strasse_Postfach { get; set; }
		public bool? termin_eingehalten { get; set; }
		public string Typ { get; set; }
		public string Unser_Zeichen { get; set; }
		public bool? USt_Berechnen { get; set; }
		public string Versandart { get; set; }
		public string Versandarten_Auswahl { get; set; }
		public DateTime? Versanddatum_Auswahl { get; set; }
		public string Vorname_NameFirma { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public string Zahlungsweise { get; set; }
		public string Zahlungsziel { get; set; }

		public AngeboteEntity() { }

		public AngeboteEntity(DataRow dataRow)
		{
			ab_id = (dataRow["ab_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ab_id"]);
			ABSENDER = (dataRow["ABSENDER"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ABSENDER"]);
			Abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			Angebot_Nr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			Ansprechpartner = (dataRow["Ansprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ansprechpartner"]);
			Auswahl = (dataRow["Auswahl"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Auswahl"]);
			Belegkreis = (dataRow["Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Benutzer"]);
			Bereich = (dataRow["Bereich"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bereich"]);
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			Briefanrede = (dataRow["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Briefanrede"]);
			datueber = (dataRow["datueber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["datueber"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Debitorennummer = (dataRow["Debitorennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Debitorennummer"]);
			Dplatz_Sirona = (dataRow["Dplatz_Sirona"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dplatz_Sirona"]);
			EDI_Dateiname_CSV = (dataRow["EDI_Dateiname_CSV"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EDI_Dateiname_CSV"]);
			EDI_Kundenbestellnummer = (dataRow["EDI_Kundenbestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EDI_Kundenbestellnummer"]);
			EDI_Order_Change = (dataRow["EDI_Order_Change"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI_Order_Change"]);
			EDI_Order_Change_Updated = (dataRow["EDI_Order_Change_Updated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI_Order_Change_Updated"]);
			EDI_Order_Neu = (dataRow["EDI_Order_Neu"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI_Order_Neu"]);
			erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt"]);
			Falligkeit = (dataRow["Fälligkeit"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Fälligkeit"]);
			Freie_Text = (dataRow["Freie_Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freie_Text"]);
			Freitext = (dataRow["Freitext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freitext"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gedruckt"]);
			Ihr_Zeichen = (dataRow["Ihr Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ihr Zeichen"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			Interessent = (dataRow["Interessent"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Interessent"]);
			Konditionen = (dataRow["Konditionen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Konditionen"]);
			Kunden_Nr = (dataRow["Kunden-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kunden-Nr"]);
			LAbteilung = (dataRow["LAbteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAbteilung"]);
			Land_PLZ_Ort = (dataRow["Land/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land/PLZ/Ort"]);
			LAnrede = (dataRow["LAnrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAnrede"]);
			LAnsprechpartner = (dataRow["LAnsprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAnsprechpartner"]);
			LBriefanrede = (dataRow["LBriefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LBriefanrede"]);
			Lieferadresse = (dataRow["Lieferadresse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferadresse"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			LLand_PLZ_Ort = (dataRow["LLand/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LLand/PLZ/Ort"]);
			LName2 = (dataRow["LName2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName2"]);
			LName3 = (dataRow["LName3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName3"]);
			Loschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			LStrasse_Postfach = (dataRow["LStraße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LStraße/Postfach"]);
			LVorname_NameFirma = (dataRow["LVorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LVorname/NameFirma"]);
			Mahnung = (dataRow["Mahnung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mahnung"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			Neu = (dataRow["Neu"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Neu"]);
			Neu_Order = (dataRow["Neu_Order"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Neu_Order"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			nr_ang = (dataRow["nr_ang"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_ang"]);
			nr_auf = (dataRow["nr_auf"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_auf"]);
			nr_BV = (dataRow["nr_BV"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_BV"]);
			nr_gut = (dataRow["nr_gut"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_gut"]);
			nr_Kanban = (dataRow["nr_Kanban"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_Kanban"]);
			nr_lie = (dataRow["nr_lie"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_lie"]);
			nr_pro = (dataRow["nr_pro"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_pro"]);
			nr_RA = (dataRow["nr_RA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_RA"]);
			nr_rec = (dataRow["nr_rec"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_rec"]);
			nr_sto = (dataRow["nr_sto"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_sto"]);
			Offnen = (dataRow["Öffnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Öffnen"]);
			Personal_Nr = (dataRow["Personal-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personal-Nr"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			reparatur_nr = (dataRow["reparatur_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["reparatur_nr"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			Strasse_Postfach = (dataRow["Straße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße/Postfach"]);
			termin_eingehalten = (dataRow["termin_eingehalten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["termin_eingehalten"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Unser_Zeichen = (dataRow["Unser Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Unser Zeichen"]);
			USt_Berechnen = (dataRow["USt_Berechnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["USt_Berechnen"]);
			Versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			Versandarten_Auswahl = (dataRow["Versandarten_Auswahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandarten_Auswahl"]);
			Versanddatum_Auswahl = (dataRow["Versanddatum_Auswahl"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Versanddatum_Auswahl"]);
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			Wunschtermin = (dataRow["Wunschtermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wunschtermin"]);
			Zahlungsweise = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
			Zahlungsziel = (dataRow["Zahlungsziel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsziel"]);
		}

		public AngeboteEntity ShallowClone()
		{
			return new AngeboteEntity
			{
				ab_id = ab_id,
				ABSENDER = ABSENDER,
				Abteilung = Abteilung,
				Angebot_Nr = Angebot_Nr,
				Anrede = Anrede,
				Ansprechpartner = Ansprechpartner,
				Auswahl = Auswahl,
				Belegkreis = Belegkreis,
				Bemerkung = Bemerkung,
				Benutzer = Benutzer,
				Bereich = Bereich,
				Bezug = Bezug,
				Briefanrede = Briefanrede,
				datueber = datueber,
				Datum = Datum,
				Debitorennummer = Debitorennummer,
				Dplatz_Sirona = Dplatz_Sirona,
				EDI_Dateiname_CSV = EDI_Dateiname_CSV,
				EDI_Kundenbestellnummer = EDI_Kundenbestellnummer,
				EDI_Order_Change = EDI_Order_Change,
				EDI_Order_Change_Updated = EDI_Order_Change_Updated,
				EDI_Order_Neu = EDI_Order_Neu,
				erledigt = erledigt,
				Falligkeit = Falligkeit,
				Freie_Text = Freie_Text,
				Freitext = Freitext,
				gebucht = gebucht,
				gedruckt = gedruckt,
				Ihr_Zeichen = Ihr_Zeichen,
				In_Bearbeitung = In_Bearbeitung,
				Interessent = Interessent,
				Konditionen = Konditionen,
				Kunden_Nr = Kunden_Nr,
				LAbteilung = LAbteilung,
				Land_PLZ_Ort = Land_PLZ_Ort,
				LAnrede = LAnrede,
				LAnsprechpartner = LAnsprechpartner,
				LBriefanrede = LBriefanrede,
				Lieferadresse = Lieferadresse,
				Liefertermin = Liefertermin,
				LLand_PLZ_Ort = LLand_PLZ_Ort,
				LName2 = LName2,
				LName3 = LName3,
				Loschen = Loschen,
				LStrasse_Postfach = LStrasse_Postfach,
				LVorname_NameFirma = LVorname_NameFirma,
				Mahnung = Mahnung,
				Mandant = Mandant,
				Name2 = Name2,
				Name3 = Name3,
				Neu = Neu,
				Neu_Order = Neu_Order,
				Nr = Nr,
				nr_ang = nr_ang,
				nr_auf = nr_auf,
				nr_BV = nr_BV,
				nr_gut = nr_gut,
				nr_Kanban = nr_Kanban,
				nr_lie = nr_lie,
				nr_pro = nr_pro,
				nr_RA = nr_RA,
				nr_rec = nr_rec,
				nr_sto = nr_sto,
				Offnen = Offnen,
				Personal_Nr = Personal_Nr,
				Projekt_Nr = Projekt_Nr,
				reparatur_nr = reparatur_nr,
				Status = Status,
				Strasse_Postfach = Strasse_Postfach,
				termin_eingehalten = termin_eingehalten,
				Typ = Typ,
				Unser_Zeichen = Unser_Zeichen,
				USt_Berechnen = USt_Berechnen,
				Versandart = Versandart,
				Versandarten_Auswahl = Versandarten_Auswahl,
				Versanddatum_Auswahl = Versanddatum_Auswahl,
				Vorname_NameFirma = Vorname_NameFirma,
				Wunschtermin = Wunschtermin,
				Zahlungsweise = Zahlungsweise,
				Zahlungsziel = Zahlungsziel
			};
		}
	}
}

