using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class PackingEntity
	{
		public DateTime? liefertermin { get; set; }
		public long nrAngebote { get; set; } //champ nr en table angebote
		public long nrAngeboteArtikel { get; set; }
		public long angeboteNr { get; set; }
		public string vornameNameFirma { get; set; }
		public string benutzer { get; set; }
		public int anzahl { get; set; }
		public string artikelnummer { get; set; }
		public int artikelNr { get; set; }
		public string bezeichnung1 { get; set; }
		public int lagerort_id { get; set; }
		public string versandinfo_von_CS { get; set; }
		public bool packstatus { get; set; }
		public string gepackt_von { get; set; }
		public DateTime? gepackt_Zeitpunkt { get; set; }
		public bool versandstatus { get; set; }
		public string versanddienstleister { get; set; }
		public string versandnummer { get; set; }
		public string postext { get; set; }
		public string packinfo_von_Lager { get; set; }
		public string versandinfo_von_Lager { get; set; }
		public string lagerort { get; set; }
		public bool gebucht { get; set; }
		public bool versand_gedruckt { get; set; }
		public string Abladestelle { get; set; }
		public bool ls_von_Versand_gedruckt { get; set; }
		public string versandarten_Auswahl { get; set; }
		public DateTime? versanddatum_Auswahl { get; set; }
		public decimal Groesse { get; set; }

		public PackingEntity(DataRow dataRow)
		{
			liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			nrAngebote = (dataRow["nrAngebote"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["nrAngebote"]);
			nrAngeboteArtikel = (dataRow["nrAngeboteArtikel"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["nrAngeboteArtikel"]);
			angeboteNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Angebot-Nr"]);
			vornameNameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Benutzer"]);
			anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Anzahl"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			artikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["ArtikelNr"]);
			bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lagerort_id"]);
			versandinfo_von_CS = (dataRow["Versandinfo_von_CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandinfo_von_CS"]);
			packstatus = (dataRow["Packstatus"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Packstatus"]);
			gepackt_von = (dataRow["Gepackt_von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gepackt_von"]);
			gepackt_Zeitpunkt = (dataRow["Gepackt_Zeitpunkt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Gepackt_Zeitpunkt"]);
			versandstatus = (dataRow["Versandstatus"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Versandstatus"]);
			versanddienstleister = (dataRow["Versanddienstleister"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versanddienstleister"]);
			versandnummer = (dataRow["Versandnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandnummer"]);
			postext = (dataRow["POSTEXT"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["POSTEXT"]);
			packinfo_von_Lager = (dataRow["Packinfo_von_Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Packinfo_von_Lager"]);
			versandinfo_von_Lager = (dataRow["Versandinfo_von_Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandinfo_von_Lager"]);
			lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["gebucht"]);
			versand_gedruckt = (dataRow["Versand_gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Versand_gedruckt"]);
			Abladestelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
			ls_von_Versand_gedruckt = (dataRow["LS_von_Versand_gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["LS_von_Versand_gedruckt"]);
			versandarten_Auswahl = (dataRow["Versandarten_Auswahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandarten_Auswahl"]);
			versanddatum_Auswahl = (dataRow["Versanddatum_Auswahl"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Versanddatum_Auswahl"]);
			Groesse = (dataRow["Größe"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Größe"]);
		}
	}
	public class PackingChooseEntity
	{
		public DateTime? liefertermin { get; set; }
		public long nrAngeboteArtikel { get; set; }
		public string artikelnummer { get; set; }
		public long angeboteNr { get; set; }
		public string versandinfo_von_CS { get; set; }
		public string versandAart { get; set; }//
		public string bestellnummer { get; set; }//
		public int anzahl { get; set; }
		public string bezeichnung1 { get; set; }
		public int lagerort_id { get; set; }
		public bool packstatus { get; set; }
		public decimal exportGewicht { get; set; }
		public bool versandstatus { get; set; }
		public string versanddienstleister { get; set; }
		public string versandnummer { get; set; }
		public string postext { get; set; }
		public string lagerort { get; set; }
		public bool gebucht { get; set; }
		public bool versand_gedruckt { get; set; }
		public string Abladestelle { get; set; }
		public string? verpackungsart { get; set; }
		public int? verpackungsmenge { get; set; }
		public decimal gewichtArtikel { get; set; }
		public string versandarten_Auswahl { get; set; }
		public DateTime? versanddatum_Auswahl { get; set; }
		public decimal verkaufpreis { get; set; }
		public decimal? verpackungGewicht { get; set; }
		public string lStrassePostfach { get; set; }
		public string lLandPLZORT { get; set; }
		public string lAnrede { get; set; }
		public string lName2 { get; set; }
		public string lName3 { get; set; }
		public string lAnsprechpartner { get; set; }
		public string lAbteilung { get; set; }
		public string lVornameNameFirma { get; set; }
		public decimal preis { get; set; }
		public string bezug { get; set; }
		public bool vDAGedruckt { get; set; }



		public PackingChooseEntity(DataRow dataRow)
		{
			liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			nrAngeboteArtikel = (dataRow["Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Nr"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			angeboteNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Angebot-Nr"]);
			versandinfo_von_CS = (dataRow["Versandinfo_von_CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandinfo_von_CS"]);
			versandAart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellnummer"]);
			anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Anzahl"]);
			bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lagerort_id"]);
			packstatus = (dataRow["Packstatus"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Packstatus"]);
			exportGewicht = (dataRow["Exportgewicht"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Exportgewicht"]);
			versandstatus = (dataRow["Versandstatus"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Versandstatus"]);
			versanddienstleister = (dataRow["Versanddienstleister"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versanddienstleister"]);
			versandnummer = (dataRow["Versandnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandnummer"]);
			postext = (dataRow["POSTEXT"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["POSTEXT"]);
			lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["gebucht"]);
			versand_gedruckt = (dataRow["Versand_gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Versand_gedruckt"]);
			Abladestelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
			verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? (string?)null : Convert.ToString(dataRow["Verpackungsart"]);
			verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsmenge"]);
			gewichtArtikel = (dataRow["Gewicht_Artikel"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gewicht_Artikel"]);
			versandarten_Auswahl = (dataRow["Versandarten_Auswahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandarten_Auswahl"]);
			versanddatum_Auswahl = (dataRow["Versanddatum_Auswahl"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Versanddatum_Auswahl"]);
			verkaufpreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Verkaufspreis"]);
			verpackungGewicht = (dataRow["VerpackungGewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VerpackungGewicht"]);
			lStrassePostfach = (dataRow["LStraße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LStraße/Postfach"]);
			lLandPLZORT = (dataRow["LLand/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LLand/PLZ/Ort"]);
			lAnrede = (dataRow["LAnrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAnrede"]);
			lName2 = (dataRow["LName2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName2"]);
			lName3 = (dataRow["LName3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName3"]);
			lAnsprechpartner = (dataRow["LAnsprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAnsprechpartner"]);
			lAbteilung = (dataRow["LAbteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAbteilung"]);
			lVornameNameFirma = (dataRow["LVorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LVorname/NameFirma"]);
			preis = (dataRow["Preis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Preis"]);
			bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			vDAGedruckt = (dataRow["VDA_gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["VDA_gedruckt"]);

		}
	}
}
