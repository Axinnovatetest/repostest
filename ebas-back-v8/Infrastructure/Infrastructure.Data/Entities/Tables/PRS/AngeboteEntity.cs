using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class AngeboteEntity
	{
		public int? Ab_id { get; set; }
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
		public bool? Datueber { get; set; }
		public DateTime? Datum { get; set; }
		public string Debitorennummer { get; set; }
		public string Dplatz_Sirona { get; set; }
		public string EDI_Dateiname_CSV { get; set; }
		public string EDI_Kundenbestellnummer { get; set; }
		public bool? EDI_Order_Change { get; set; }
		public bool? EDI_Order_Change_Updated { get; set; }
		public bool? EDI_Order_Neu { get; set; }
		public bool? Erledigt { get; set; }
		public DateTime? Falligkeit { get; set; }
		public string Freie_Text { get; set; }
		public string Freitext { get; set; }
		public bool? Gebucht { get; set; }
		public bool? Gedruckt { get; set; }
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
		public string Lieferadresse { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string LLand_PLZ_Ort { get; set; }
		public string LName2 { get; set; }
		public string LName3 { get; set; }
		public bool? Loschen { get; set; }
		public string LStraße_Postfach { get; set; }
		public string LVorname_NameFirma { get; set; }
		public bool? Mahnung { get; set; }
		public string Mandant { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public int? Neu { get; set; }
		public bool? Neu_Order { get; set; }
		public int Nr { get; set; }
		public int? Nr_ang { get; set; }
		public int? Nr_auf { get; set; }
		public int? Nr_BV { get; set; }
		public int? nr_dlf { get; set; }
		public int? Nr_gut { get; set; }
		public int? Nr_Kanban { get; set; }
		public int? Nr_lie { get; set; }
		public int? Nr_pro { get; set; }
		public int? Nr_RA { get; set; }
		public int? Nr_rec { get; set; }
		public int? Nr_sto { get; set; }
		public bool? Offnen { get; set; }
		public int? Personal_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public int? Reparatur_nr { get; set; }
		public string Status { get; set; }
		public string Straße_Postfach { get; set; }
		public bool? Termin_eingehalten { get; set; }
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
		public decimal? Betrag_MWSt { get; set; }
		public bool? rec_sent { get; set; }
		public DateTime? LsDeliveryDate { get; set; }
		public int? LsAddressNr { get; set; }
		public string UnloadingPoint { get; set; }
		public string StorageLocation { get; set; }

		public AngeboteEntity() { }

		public AngeboteEntity(DataRow dataRow, bool rechnung = false)
		{
			Ab_id = (dataRow["ab_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ab_id"]);
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
			Datueber = (dataRow["datueber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["datueber"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Debitorennummer = (dataRow["Debitorennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Debitorennummer"]);
			Dplatz_Sirona = (dataRow["Dplatz_Sirona"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dplatz_Sirona"]);
			EDI_Dateiname_CSV = (dataRow["EDI_Dateiname_CSV"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EDI_Dateiname_CSV"]);
			EDI_Kundenbestellnummer = (dataRow["EDI_Kundenbestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EDI_Kundenbestellnummer"]);
			EDI_Order_Change = (dataRow["EDI_Order_Change"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI_Order_Change"]);
			EDI_Order_Change_Updated = (dataRow["EDI_Order_Change_Updated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI_Order_Change_Updated"]);
			EDI_Order_Neu = (dataRow["EDI_Order_Neu"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI_Order_Neu"]);
			Erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt"]);
			Falligkeit = (dataRow["Fälligkeit"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Fälligkeit"]);
			Freie_Text = (dataRow["Freie_Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freie_Text"]);
			Freitext = (dataRow["Freitext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freitext"]);
			Gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			Gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gedruckt"]);
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
			Lieferadresse = (dataRow["Lieferadresse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferadresse"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			LLand_PLZ_Ort = (dataRow["LLand/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LLand/PLZ/Ort"]);
			LName2 = (dataRow["LName2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName2"]);
			LName3 = (dataRow["LName3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName3"]);
			Loschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			LStraße_Postfach = (dataRow["LStraße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LStraße/Postfach"]);
			LVorname_NameFirma = (dataRow["LVorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LVorname/NameFirma"]);
			Mahnung = (dataRow["Mahnung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mahnung"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			Neu = (dataRow["Neu"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Neu"]);
			Neu_Order = (dataRow["Neu_Order"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Neu_Order"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Nr_ang = (dataRow["nr_ang"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_ang"]);
			Nr_auf = (dataRow["nr_auf"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_auf"]);
			Nr_BV = (dataRow["nr_BV"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_BV"]);
			nr_dlf = (dataRow["nr_dlf"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_dlf"]);
			Nr_gut = (dataRow["nr_gut"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_gut"]);
			Nr_Kanban = (dataRow["nr_Kanban"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_Kanban"]);
			Nr_lie = (dataRow["nr_lie"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_lie"]);
			Nr_pro = (dataRow["nr_pro"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_pro"]);
			Nr_RA = (dataRow["nr_RA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_RA"]);
			Nr_rec = (dataRow["nr_rec"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_rec"]);
			Nr_sto = (dataRow["nr_sto"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_sto"]);
			Offnen = (dataRow["Öffnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Öffnen"]);
			Personal_Nr = (dataRow["Personal-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personal-Nr"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			Reparatur_nr = (dataRow["reparatur_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["reparatur_nr"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			Straße_Postfach = (dataRow["Straße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße/Postfach"]);
			Termin_eingehalten = (dataRow["termin_eingehalten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["termin_eingehalten"]);
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
			rec_sent = (dataRow["rec_sent"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["rec_sent"]);
			LsDeliveryDate = (dataRow["LsDeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LsDeliveryDate"]);
			StorageLocation = (dataRow["StorageLocation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StorageLocation"]);
			UnloadingPoint = (dataRow["UnloadingPoint"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UnloadingPoint"]);
			LsAddressNr = (dataRow["LsAddressNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LsAddressNr"]);
			if(rechnung)
				Betrag_MWSt = (dataRow["Betrag_MWSt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag_MWSt"]);
		}

		public AngeboteEntity ShallowClone()
		{
			return new AngeboteEntity
			{
				Ab_id = Ab_id,
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
				Datueber = Datueber,
				Datum = Datum,
				Debitorennummer = Debitorennummer,
				Dplatz_Sirona = Dplatz_Sirona,
				EDI_Dateiname_CSV = EDI_Dateiname_CSV,
				EDI_Kundenbestellnummer = EDI_Kundenbestellnummer,
				EDI_Order_Change = EDI_Order_Change,
				EDI_Order_Change_Updated = EDI_Order_Change_Updated,
				EDI_Order_Neu = EDI_Order_Neu,
				Erledigt = Erledigt,
				Falligkeit = Falligkeit,
				Freie_Text = Freie_Text,
				Freitext = Freitext,
				Gebucht = Gebucht,
				Gedruckt = Gedruckt,
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
				LStraße_Postfach = LStraße_Postfach,
				LVorname_NameFirma = LVorname_NameFirma,
				Mahnung = Mahnung,
				Mandant = Mandant,
				Name2 = Name2,
				Name3 = Name3,
				Neu = Neu,
				Neu_Order = Neu_Order,
				Nr = Nr,
				Nr_ang = Nr_ang,
				Nr_auf = Nr_auf,
				Nr_BV = Nr_BV,
				nr_dlf = nr_dlf,
				Nr_gut = Nr_gut,
				Nr_Kanban = Nr_Kanban,
				Nr_lie = Nr_lie,
				Nr_pro = Nr_pro,
				Nr_RA = Nr_RA,
				Nr_rec = Nr_rec,
				Nr_sto = Nr_sto,
				Offnen = Offnen,
				Personal_Nr = Personal_Nr,
				Projekt_Nr = Projekt_Nr,
				Reparatur_nr = Reparatur_nr,
				Status = Status,
				Straße_Postfach = Straße_Postfach,
				Termin_eingehalten = Termin_eingehalten,
				Typ = Typ,
				Unser_Zeichen = Unser_Zeichen,
				USt_Berechnen = USt_Berechnen,
				Versandart = Versandart,
				Versandarten_Auswahl = Versandarten_Auswahl,
				Versanddatum_Auswahl = Versanddatum_Auswahl,
				Vorname_NameFirma = Vorname_NameFirma,
				Wunschtermin = Wunschtermin,
				Zahlungsweise = Zahlungsweise,
				Zahlungsziel = Zahlungsziel,
				rec_sent = rec_sent,
				LsDeliveryDate = LsDeliveryDate,
				LsAddressNr = LsAddressNr,
				UnloadingPoint = UnloadingPoint,
				StorageLocation = StorageLocation,
			};
		}
	}
}

