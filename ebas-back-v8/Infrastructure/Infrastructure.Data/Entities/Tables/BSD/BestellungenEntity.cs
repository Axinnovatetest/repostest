using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class BestellungenEntity
	{
		public string AB_Nr_Lieferant { get; set; }
		public string Abteilung { get; set; }
		public DateTime? Anfrage_Lieferfrist { get; set; }
		public string Anrede { get; set; }
		public string Ansprechpartner { get; set; }
		public int? Bearbeiter { get; set; }
		public int? Belegkreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Benutzer { get; set; }
		public int? best_id { get; set; }
		public DateTime? Bestellbestätigung_erbeten_bis { get; set; }
		public int? Bestellung_Nr { get; set; }
		public string Bezug { get; set; }
		public string Briefanrede { get; set; }
		public bool? datueber { get; set; }
		public DateTime? Datum { get; set; }
		public string Eingangslieferscheinnr { get; set; }
		public string Eingangsrechnungsnr { get; set; }
		public bool? erledigt { get; set; }
		public decimal? Frachtfreigrenze { get; set; }
		public string Freitext { get; set; }
		public bool? gebucht { get; set; }
		public bool? gedruckt { get; set; }
		public string Ihr_Zeichen { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public bool? Kanban { get; set; }
		public string Konditionen { get; set; }
		public string Kreditorennummer { get; set; }
		public int? Kundenbestellung { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public int? Lieferanten_Nr { get; set; }
		public DateTime? Liefertermin { get; set; }
		public bool? Löschen { get; set; }
		public DateTime? Mahnung { get; set; }
		public string Mandant { get; set; }
		public decimal? Mindestbestellwert { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public bool? Neu { get; set; }
		public int Nr { get; set; }
		public int? nr_anf { get; set; }
		public int? nr_bes { get; set; }
		public int? nr_gut { get; set; }
		public int? nr_RB { get; set; }
		public int? nr_sto { get; set; }
		public int? nr_war { get; set; }
		public bool? Öffnen { get; set; }
		public int? Personal_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public double? Rabatt { get; set; }
		public bool? Rahmenbestellung { get; set; }
		public string Straße_Postfach { get; set; }
		public string Typ { get; set; }
		public string Unser_Zeichen { get; set; }
		public decimal? USt { get; set; }
		public string Versandart { get; set; }
		public string Vorname_NameFirma { get; set; }
		public int? Währung { get; set; }
		public string Zahlungsweise { get; set; }
		public string Zahlungsziel { get; set; }
		// - 2024-04-24
		public bool? ProjectPurchase { get; set; }

		public BestellungenEntity() { }

		public BestellungenEntity(DataRow dataRow)
		{
			AB_Nr_Lieferant = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
			Abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			Anfrage_Lieferfrist = (dataRow["Anfrage_Lieferfrist"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Anfrage_Lieferfrist"]);
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			Ansprechpartner = (dataRow["Ansprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ansprechpartner"]);
			Bearbeiter = (dataRow["Bearbeiter"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bearbeiter"]);
			Belegkreis = (dataRow["Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Benutzer"]);
			best_id = (dataRow["best_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["best_id"]);
			Bestellbestätigung_erbeten_bis = (dataRow["Bestellbestätigung erbeten bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestellbestätigung erbeten bis"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			Briefanrede = (dataRow["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Briefanrede"]);
			datueber = (dataRow["datueber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["datueber"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Eingangslieferscheinnr = (dataRow["Eingangslieferscheinnr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Eingangslieferscheinnr"]);
			Eingangsrechnungsnr = (dataRow["Eingangsrechnungsnr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Eingangsrechnungsnr"]);
			erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt"]);
			Frachtfreigrenze = (dataRow["Frachtfreigrenze"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Frachtfreigrenze"]);
			Freitext = (dataRow["Freitext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freitext"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gedruckt"]);
			Ihr_Zeichen = (dataRow["Ihr Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ihr Zeichen"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			Konditionen = (dataRow["Konditionen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Konditionen"]);
			Kreditorennummer = (dataRow["Kreditorennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kreditorennummer"]);
			Kundenbestellung = (dataRow["Kundenbestellung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundenbestellung"]);
			Land_PLZ_Ort = (dataRow["Land/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land/PLZ/Ort"]);
			Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferanten-Nr"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			Mahnung = (dataRow["Mahnung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Mahnung"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
			Mindestbestellwert = (dataRow["Mindestbestellwert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestellwert"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			Neu = (dataRow["Neu"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Neu"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			nr_anf = (dataRow["nr_anf"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_anf"]);
			nr_bes = (dataRow["nr_bes"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_bes"]);
			nr_gut = (dataRow["nr_gut"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_gut"]);
			nr_RB = (dataRow["nr_RB"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_RB"]);
			nr_sto = (dataRow["nr_sto"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_sto"]);
			nr_war = (dataRow["nr_war"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_war"]);
			Öffnen = (dataRow["Öffnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Öffnen"]);
			Personal_Nr = (dataRow["Personal-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personal-Nr"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt"]);
			Rahmenbestellung = (dataRow["Rahmenbestellung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmenbestellung"]);
			Straße_Postfach = (dataRow["Straße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße/Postfach"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Unser_Zeichen = (dataRow["Unser Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Unser Zeichen"]);
			USt = (dataRow["USt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["USt"]);
			Versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			Währung = (dataRow["Währung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Währung"]);
			Zahlungsweise = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
			Zahlungsziel = (dataRow["Zahlungsziel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsziel"]);
			ProjectPurchase = (dataRow["ProjectPurchase"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectPurchase"]);
		}
	}
}

