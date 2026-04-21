using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class FertigungEntity
	{
		public int? Angebot_Artikel_Nr { get; set; }
		public int? Angebot_nr { get; set; }
		public int? Anzahl { get; set; }
		public int? Anzahl_aktuell { get; set; }
		public int? Anzahl_erledigt { get; set; }
		public int AnzahlnachgedrucktPPS { get; set; }
		public int? Artikel_Nr { get; set; }
		public bool? Ausgangskontrolle { get; set; }
		public string Bemerkung { get; set; }
		public string Bemerkung_II_Planung { get; set; }
		public string Bemerkung_ohne_statte { get; set; }
		public string Bemerkung_Kommissionierung_AL { get; set; }
		public string Bemerkung_Planung { get; set; }
		public string Bemerkung_Technik { get; set; }
		public string Bemerkung_zu_Prio { get; set; }
		public int? BomVersion { get; set; }
		public bool? CAO { get; set; }
		public bool? Check_FAbegonnen { get; set; }
		public bool? Check_Gewerk1 { get; set; }
		public bool? Check_Gewerk1_Teilweise { get; set; }
		public bool? Check_Gewerk2 { get; set; }
		public bool? Check_Gewerk2_Teilweise { get; set; }
		public bool? Check_Gewerk3 { get; set; }
		public bool? Check_Gewerk3_Teilweise { get; set; }
		public bool? Check_Kabelgeschnitten { get; set; }
		public int? CPVersion { get; set; }
		public DateTime? Datum { get; set; }
		public bool? Endkontrolle { get; set; }
		public DateTime? Erledigte_FA_Datum { get; set; }
		public bool? Erstmuster { get; set; }
		public DateTime? FA_begonnen { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public bool? FA_Gestartet { get; set; }
		public bool? Fa_NachdruckPPS { get; set; }
		public int? Fertigungsnummer { get; set; }
		public bool? Gebucht { get; set; }
		public bool? Gedruckt { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public string Gewerk_Teilweise_Bemerkung { get; set; }
		public string GrundNachdruckPPS { get; set; }
		public int? HBGFAPositionId { get; set; }
		public int ID { get; set; }
		public int? ID_Hauptartikel { get; set; }
		public int? ID_Rahmenfertigung { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public DateTime? Kabel_geschnitten_Datum { get; set; }
		public bool? Kabel_Schneidebeginn { get; set; }
		public DateTime? Kabel_Schneidebeginn_Datum { get; set; }
		public string Kennzeichen { get; set; }
		public bool? Kommisioniert_komplett { get; set; }
		public bool? Kommisioniert_teilweise { get; set; }
		public DateTime? Kunden_Index_Datum { get; set; }
		public string KundenIndex { get; set; }
		public int? Lagerort_id { get; set; }
		public int? Lagerort_id_zubuchen { get; set; }
		public int? Letzte_Gebuchte_Menge { get; set; }
		public bool? Löschen { get; set; }
		public string Mandant { get; set; }
		public int? Menge1 { get; set; }
		public int? Menge2 { get; set; }
		public int? Originalanzahl { get; set; }
		public string Planungsstatus { get; set; }
		public decimal? Preis { get; set; }
		public bool? Prio { get; set; }
		public bool? Quick_Area { get; set; }
		public bool? ROH_umgebucht { get; set; }
		public bool? SpritzgieBerei_abgeschlossen { get; set; }
		public int? Tage_Abweichung { get; set; }
		public bool? Technik { get; set; }
		public string Techniker { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public DateTime? Termin_Bestatigt2 { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public DateTime? Termin_Material { get; set; }
		public DateTime? Termin_Ursprunglich { get; set; }
		public DateTime? Termin_voranderung { get; set; }
		public bool? UBG { get; set; }
		public bool? UBGTransfer { get; set; }
		public string Urs_Artikelnummer { get; set; }
		public string Urs_Fa { get; set; }
		public decimal? Zeit { get; set; }
		public bool? Termin_Bestatigt2_Updated { get; set; }
		public string FertigungType { get; set; }
		public bool? PlanningDateViolation { get; set; }

		public FertigungEntity() { }
		public FertigungEntity(DataRow dataRow)
		{
			Angebot_Artikel_Nr = (dataRow["Angebot_Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot_Artikel_Nr"]);
			Angebot_nr = (dataRow["Angebot_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot_nr"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Anzahl_aktuell = (dataRow["Anzahl_aktuell"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_aktuell"]);
			Anzahl_erledigt = (dataRow["Anzahl_erledigt"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_erledigt"]);
			AnzahlnachgedrucktPPS = Convert.ToInt32(dataRow["AnzahlnachgedrucktPPS"]);
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Ausgangskontrolle = (dataRow["Ausgangskontrolle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Ausgangskontrolle"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bemerkung_II_Planung = (dataRow["Bemerkung II Planung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung II Planung"]);
			Bemerkung_ohne_statte = (dataRow["Bemerkung ohne stätte"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung ohne stätte"]);
			Bemerkung_Kommissionierung_AL = (dataRow["Bemerkung_Kommissionierung_AL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Kommissionierung_AL"]);
			Bemerkung_Planung = (dataRow["Bemerkung_Planung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Planung"]);
			Bemerkung_Technik = (dataRow["Bemerkung_Technik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Technik"]);
			Bemerkung_zu_Prio = (dataRow["Bemerkung_zu_Prio"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_zu_Prio"]);
			BomVersion = (dataRow["BomVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BomVersion"]);
			CAO = (dataRow["CAO"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CAO"]);
			Check_FAbegonnen = (dataRow["Check_FAbegonnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Check_FAbegonnen"]);
			Check_Gewerk1 = (dataRow["Check_Gewerk1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Check_Gewerk1"]);
			Check_Gewerk1_Teilweise = (dataRow["Check_Gewerk1_Teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Check_Gewerk1_Teilweise"]);
			Check_Gewerk2 = (dataRow["Check_Gewerk2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Check_Gewerk2"]);
			Check_Gewerk2_Teilweise = (dataRow["Check_Gewerk2_Teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Check_Gewerk2_Teilweise"]);
			Check_Gewerk3 = (dataRow["Check_Gewerk3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Check_Gewerk3"]);
			Check_Gewerk3_Teilweise = (dataRow["Check_Gewerk3_Teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Check_Gewerk3_Teilweise"]);
			Check_Kabelgeschnitten = (dataRow["Check_Kabelgeschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Check_Kabelgeschnitten"]);
			CPVersion = (dataRow["CPVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CPVersion"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Endkontrolle = (dataRow["Endkontrolle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Endkontrolle"]);
			Erledigte_FA_Datum = (dataRow["Erledigte_FA_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Erledigte_FA_Datum"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			FA_begonnen = (dataRow["FA_begonnen"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_begonnen"]);
			FA_Druckdatum = (dataRow["FA_Druckdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Druckdatum"]);
			FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FA_Gestartet"]);
			Fa_NachdruckPPS = (dataRow["Fa-NachdruckPPS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Fa-NachdruckPPS"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			Gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gedruckt"]);
			Gewerk_1 = Convert.ToString(dataRow["Gewerk 1"]);
			Gewerk_2 = Convert.ToString(dataRow["Gewerk 2"]);
			Gewerk_3 = Convert.ToString(dataRow["Gewerk 3"]);
			Gewerk_Teilweise_Bemerkung = (dataRow["Gewerk_Teilweise_Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk_Teilweise_Bemerkung"]);
			GrundNachdruckPPS = (dataRow["GrundNachdruckPPS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["GrundNachdruckPPS"]);
			HBGFAPositionId = (dataRow["HBGFAPositionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HBGFAPositionId"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Hauptartikel = (dataRow["ID_Hauptartikel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Hauptartikel"]);
			ID_Rahmenfertigung = (dataRow["ID_Rahmenfertigung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Rahmenfertigung"]);
			Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
			Kabel_geschnitten_Datum = (dataRow["Kabel_geschnitten_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Kabel_geschnitten_Datum"]);
			Kabel_Schneidebeginn = (dataRow["Kabel_Schneidebeginn"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_Schneidebeginn"]);
			Kabel_Schneidebeginn_Datum = (dataRow["Kabel_Schneidebeginn_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Kabel_Schneidebeginn_Datum"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_komplett"]);
			Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_teilweise"]);
			Kunden_Index_Datum = (dataRow["Kunden_Index_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Kunden_Index_Datum"]);
			KundenIndex = (dataRow["KundenIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["KundenIndex"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Lagerort_id_zubuchen = (dataRow["Lagerort_id zubuchen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id zubuchen"]);
			Letzte_Gebuchte_Menge = (dataRow["Letzte_Gebuchte_Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Letzte_Gebuchte_Menge"]);
			Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
			Menge1 = (dataRow["Menge1"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge1"]);
			Menge2 = (dataRow["Menge2"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge2"]);
			Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			Planungsstatus = (dataRow["Planungsstatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Planungsstatus"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			Prio = (dataRow["Prio"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Prio"]);
			Quick_Area = (dataRow["Quick_Area"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Quick_Area"]);
			ROH_umgebucht = (dataRow["ROH_umgebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ROH_umgebucht"]);
			SpritzgieBerei_abgeschlossen = (dataRow["Spritzgießerei_abgeschlossen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Spritzgießerei_abgeschlossen"]);
			Tage_Abweichung = (dataRow["Tage Abweichung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Tage Abweichung"]);
			Technik = (dataRow["Technik"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Technik"]);
			Techniker = (dataRow["Techniker"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Techniker"]);
			Termin_Bestatigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Termin_Bestatigt2 = (dataRow["Termin_Bestätigt2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt2"]);
			Termin_Fertigstellung = (dataRow["Termin_Fertigstellung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Fertigstellung"]);
			Termin_Material = (dataRow["Termin_Material"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Material"]);
			Termin_Ursprunglich = (dataRow["Termin_Ursprünglich"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Ursprünglich"]);
			Termin_voranderung = (dataRow["Termin_voränderung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_voränderung"]);
			UBG = (dataRow["UBG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBG"]);
			UBGTransfer = (dataRow["UBGTransfer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBGTransfer"]);
			Urs_Artikelnummer = (dataRow["Urs-Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Urs-Artikelnummer"]);
			Urs_Fa = (dataRow["Urs-Fa"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Urs-Fa"]);
			FertigungType = (dataRow["FertigungType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FertigungType"]);
			Zeit = (dataRow["Zeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zeit"]);
			Termin_Bestatigt2_Updated = (dataRow["Termin_Bestatigt2_Updated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Termin_Bestatigt2_Updated"]);
			PlanningDateViolation = (dataRow["PlanningDateViolation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PlanningDateViolation"]);
		}
	}

	public class FertigungGewerkEntity
	{
		public string Gewerk { get; set; }
		public int? Fertigungsnummer { get; set; }
		public FertigungGewerkEntity(DataRow dataRow)
		{
			Gewerk = (dataRow["Angebot_Artikel_Nr"] == System.DBNull.Value) ? null : dataRow["Angebot_Artikel_Nr"].ToString();
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
		}
	}
}

