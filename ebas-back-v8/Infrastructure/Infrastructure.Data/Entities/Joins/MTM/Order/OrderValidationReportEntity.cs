using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class OrderValidationReportEntity
	{
		public string Lieferanten_Nr { get; set; }
		public string Bestellung_Nr { get; set; }
		public string Typ { get; set; }
		public string Datum { get; set; }
		public string Anrede { get; set; }
		public string Vorname_NameFirma { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Strasse_Postfach { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public string Personal_Nr { get; set; }
		public string Versandart { get; set; }
		public string Zahlungsweise { get; set; }
		public string Konditionen { get; set; }
		public string Zahlungsziel { get; set; }
		public string Bezug { get; set; }
		public string Ihr_Zeichen { get; set; }
		public string Unser_Zeichen { get; set; }
		public string Bestellbestatigung_erbeten_bis { get; set; }
		public string Freitext { get; set; }
		public string Wahrung { get; set; }
		public string Ansprechpartner { get; set; }
		public string Abteilung { get; set; }
		public string Nr { get; set; }
		public string Rahmenbestellung { get; set; }
		public string Mandant { get; set; }
		public string Bestellnummer { get; set; }
		public string Rabatt1 { get; set; }
		public string Rabatt2 { get; set; }
		public string Menge { get; set; }
		public string Einzelpreis { get; set; }
		public decimal Gesamtpreis { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Einheit { get; set; }
		public string Umsatzsteuer { get; set; }
		public string sortierung { get; set; }
		public string best_art_nr { get; set; }
		public string schrift { get; set; }
		public string Preiseinheit { get; set; }
		public string LT { get; set; }
		public string Lagerort_id { get; set; }
		public string InfoRahmennummer { get; set; }
		public string CUPreis { get; set; }
		public string Langtexte { get; set; }
		public byte[] Logo { get; set; }
		public string Text_kopf { get; set; }
		public string Text_fuss { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public string Name { get; set; }
		public string Telefonnummer { get; set; }
		public string Faxnummer { get; set; }
		public string Email { get; set; }
		public string Bestellung { get; set; }
		public string Artikelnummer { get; set; }
		public bool MHD { get; set; }
		public bool COF_Pflichtig { get; set; }
		public string Zeitraum_MHD { get; set; }
		public bool EMPB { get; set; }
		public bool ESD_Schutz { get; set; }
		public string Symbol { get; set; }
		public string Bestellungen_unten { get; set; }
		public string LagerortLabel { get; set; }
		public string Position { get; set; }
		public string RahmenBezug { get; set; }
		public decimal StartAnzahl { get; set; }
		public decimal Anzahl { get; set; }
		public string CocVersion { get; set; }
		public OrderValidationReportEntity() { }
		public OrderValidationReportEntity(DataRow dataRow)
		{
			Lieferanten_Nr = dataRow["Lieferanten-Nr"].ToString();
			Bestellung_Nr = dataRow["Bestellung-Nr"].ToString();
			Typ = dataRow["Typ"].ToString();
			Datum = DateTime.TryParse(dataRow["Datum"].ToString(), out var d) ? d.ToShortDateString() : null;
			Anrede = dataRow["Anrede"].ToString();
			Vorname_NameFirma = dataRow["Vorname/NameFirma"].ToString();
			Name2 = dataRow["Name2"].ToString();
			Name3 = dataRow["Name3"].ToString();
			Strasse_Postfach = dataRow["Straße/Postfach"].ToString();
			Land_PLZ_Ort = dataRow["Land/PLZ/Ort"].ToString();
			Personal_Nr = dataRow["Personal-Nr"].ToString();
			Versandart = dataRow["Versandart"].ToString();
			Zahlungsweise = dataRow["Zahlungsweise"].ToString();
			Konditionen = dataRow["Konditionen"].ToString();
			Zahlungsziel = dataRow["Zahlungsziel"].ToString();
			Bezug = dataRow["Bezug"].ToString();
			Ihr_Zeichen = dataRow["Ihr Zeichen"].ToString();
			Unser_Zeichen = dataRow["Unser Zeichen"].ToString();
			Bestellbestatigung_erbeten_bis = DateTime.TryParse(dataRow["Bestellbestätigung erbeten bis"].ToString(), out var e) ? e.ToShortDateString() : null;
			Freitext = dataRow["Freitext"].ToString();
			Wahrung = dataRow["Währung"].ToString();
			Ansprechpartner = dataRow["Ansprechpartner"].ToString();
			Abteilung = dataRow["Abteilung"].ToString();
			Nr = dataRow["Nr"].ToString();
			Rahmenbestellung = dataRow["Rahmenbestellung"].ToString();
			Mandant = dataRow["Mandant"].ToString();
			Bestellnummer = dataRow["Bestellnummer"].ToString();
			Rabatt1 = dataRow["Rabatt1"].ToString();
			Rabatt2 = dataRow["Rabatt2"].ToString();
			Menge = dataRow["Menge"].ToString();
			Einzelpreis = dataRow["Einzelpreis"].ToString();
			Gesamtpreis = Decimal.TryParse(dataRow["Gesamtpreis"].ToString(), out var x) ? x : 0;
			Bezeichnung_1 = dataRow["Bezeichnung 1"].ToString();
			Bezeichnung_2 = dataRow["Bezeichnung 2"].ToString();
			Einheit = dataRow["Einheit"].ToString();
			Umsatzsteuer = dataRow["Umsatzsteuer"].ToString();
			sortierung = dataRow["sortierung"].ToString();
			best_art_nr = dataRow["best_art_nr"].ToString();
			schrift = dataRow["schrift"].ToString();
			Preiseinheit = dataRow["Preiseinheit"].ToString();
			LT = DateTime.TryParse(dataRow["LT"].ToString(), out var f) ? f.ToString("dd/MM/yyyy") : null;
			Lagerort_id = dataRow["Lagerort_id"].ToString();
			InfoRahmennummer = dataRow["InfoRahmennummer"].ToString();
			CUPreis = dataRow["CUPreis"].ToString();
			Langtexte = dataRow["Langtexte"].ToString();
			Logo = (byte[])dataRow["Logo"];
			Text_kopf = dataRow["Text_kopf"].ToString();
			Text_fuss = dataRow["Text_fuß"].ToString();
			Telefon = dataRow["Telefon"].ToString();
			Fax = dataRow["Fax"].ToString();
			Name = dataRow["Name"].ToString();
			Telefonnummer = dataRow["Telefonnummer"].ToString();
			Faxnummer = dataRow["Faxnummer"].ToString();
			Email = dataRow["Email"].ToString();
			Bestellung = dataRow["Bestellung"].ToString();
			Artikelnummer = dataRow["Artikelnummer"].ToString();
			MHD = (dataRow["MHD"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["MHD"]);
			COF_Pflichtig = (dataRow["COF_Pflichtig"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["COF_Pflichtig"]);
			Zeitraum_MHD = dataRow["Zeitraum_MHD"].ToString();
			EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["EMPB"]);
			ESD_Schutz = (dataRow["ESD_Schutz"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["ESD_Schutz"]);
			Symbol = dataRow["Symbol"].ToString();
			Bestellungen_unten = dataRow["Bestellungen unten"].ToString();
			Position = dataRow["Position"].ToString();
			RahmenBezug = dataRow["RahmenBezug"].ToString();

			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Anzahl"]);
			StartAnzahl = (dataRow["StartAnzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["StartAnzahl"]);
			CocVersion = (dataRow["CocVersion"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["CocVersion"]);


			Rabatt1 = string.IsNullOrEmpty(Rabatt1) ? "0" : Rabatt1;
			Rabatt2 = string.IsNullOrEmpty(Rabatt2) ? "0" : Rabatt2;
			Menge = string.IsNullOrEmpty(Menge) ? "0" : Menge;
			Einzelpreis = string.IsNullOrEmpty(Einzelpreis) ? "0" : Einzelpreis;
			//Gesamtpreis = string.IsNullOrEmpty(Gesamtpreis) ? "0" : Gesamtpreis;
			Umsatzsteuer = string.IsNullOrEmpty(Umsatzsteuer) ? "0" : Umsatzsteuer;
			Preiseinheit = string.IsNullOrEmpty(Preiseinheit) ? "0" : Preiseinheit;

			LagerortLabel = getLagerLabel(Lagerort_id);
		}
		private string getLagerLabel(string Lagerortid)
		{
			switch(Lagerortid)
			{
				case "4":
					return "TN-";
				case "41":
					return "WS-";
				case "15":
					return "FD-";
				case "20":
					return "SC-";
				case "8":
					return "HD-";
				case "24":
					return "AL-";
				case "58":
					return "BE-";
				case "101":
					return "GZ-";
				default:
					return "CZ-";
			}
		}
	}
}
