using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class BestellnummernEntity
	{
		public string Angebot { get; set; }
		public DateTime? Angebot_Datum { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		public int? ArtikelNr { get; set; }
		public double? Basispreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Bestell_Nr { get; set; }
		public double? Einkaufspreis { get; set; }
		public DateTime? Einkaufspreis_gultig_bis { get; set; }
		public double? Einkaufspreis1 { get; set; }
		public DateTime? Einkaufspreis1_gultig_bis { get; set; }
		public double? Einkaufspreis2 { get; set; }
		public DateTime? Einkaufspreis2_gultig_bis { get; set; }
		public double? EK_EUR { get; set; }
		public double? EK_total { get; set; }
		public double? Fracht { get; set; }
		public DateTime? letzte_Aktualisierung { get; set; }
		public int? Lieferanten_Nr { get; set; }
		public double? Logistik { get; set; }
		public double? Mindestbestellmenge { get; set; }
		public int Nr { get; set; }
		public double? Preiseinheit { get; set; }
		public int? Pruftiefe_WE { get; set; }
		public decimal? Rabatt { get; set; }
		public bool? Standardlieferant { get; set; }
		public double? Umsatzsteuer { get; set; }
		public double? Verpackungseinheit { get; set; }
		public string Warengruppe { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }
		public double? Zoll { get; set; }
		public double? Zusatz { get; set; }

		public BestellnummernEntity() { }

		public BestellnummernEntity(DataRow dataRow)
		{
			Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
			Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebot_Datum"]);
			Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
			Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Basispreis = (dataRow["Basispreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Basispreis"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Einkaufspreis"]);
			Einkaufspreis_gultig_bis = (dataRow["Einkaufspreis gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis gültig bis"]);
			Einkaufspreis1 = (dataRow["Einkaufspreis1"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Einkaufspreis1"]);
			Einkaufspreis1_gultig_bis = (dataRow["Einkaufspreis1 gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis1 gültig bis"]);
			Einkaufspreis2 = (dataRow["Einkaufspreis2"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Einkaufspreis2"]);
			Einkaufspreis2_gultig_bis = (dataRow["Einkaufspreis2 gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis2 gültig bis"]);
			EK_EUR = (dataRow["EK_EUR"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["EK_EUR"]);
			EK_total = (dataRow["EK_total"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["EK_total"]);
			Fracht = (dataRow["Fracht"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Fracht"]);
			letzte_Aktualisierung = (dataRow["letzte_Aktualisierung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["letzte_Aktualisierung"]);
			Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferanten-Nr"]);
			Logistik = (dataRow["Logistik"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Logistik"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mindestbestellmenge"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Preiseinheit"]);
			Pruftiefe_WE = (dataRow["Prüftiefe_WE"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Prüftiefe_WE"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Rabatt"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Umsatzsteuer"]);
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Verpackungseinheit"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
			Zoll = (dataRow["Zoll"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Zoll"]);
			Zusatz = (dataRow["Zusatz"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Zusatz"]);
		}
	}
}

