using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class BestellnummernEntity
	{
		public string Angebot { get; set; }
		public string Angebot_Datum { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		public decimal? Artikel_Nr { get; set; }
		public decimal? Basispreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public string Einkaufspreis_gültig_bis { get; set; }
		public decimal? EK_EUR { get; set; }
		public decimal? EK_total { get; set; }
		public decimal? Fracht { get; set; }
		public DateTime? letzte_Aktualisierung { get; set; }
		public decimal? Lieferanten_Nr { get; set; }
		public string LiefrantanName { get; set; }
		public decimal? Logistik { get; set; }
		public decimal? Mindestbestellmenge { get; set; }
		public int Nr { get; set; }
		public decimal? Preiseinheit { get; set; }
		public decimal? Prüftiefe_WE { get; set; }
		public decimal? Rabatt { get; set; }
		public bool Standardlieferant { get; set; }
		public decimal? Umsatzsteuer { get; set; }
		public decimal? Verpackungseinheit { get; set; }
		public string Warengruppe { get; set; }
		public decimal? Wiederbeschaffungszeitraum { get; set; }
		public decimal? Zoll { get; set; }
		public decimal? Zusatz { get; set; }

		public BestellnummernEntity() { }

		public BestellnummernEntity(DataRow dataRow)
		{
			Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
			Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot_Datum"]);
			Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
			Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Artikel-Nr"]);
			Basispreis = (dataRow["Basispreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Basispreis"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Einkaufspreis_gültig_bis = (dataRow["Einkaufspreis gültig bis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einkaufspreis gültig bis"]);
			EK_EUR = (dataRow["EK_EUR"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_EUR"]);
			EK_total = (dataRow["EK_total"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_total"]);
			Fracht = (dataRow["Fracht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Fracht"]);
			letzte_Aktualisierung = (dataRow["letzte_Aktualisierung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["letzte_Aktualisierung"]);
			Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Lieferanten-Nr"]);
			LiefrantanName = (dataRow["LiefrantanName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LiefrantanName"]);
			Logistik = (dataRow["Logistik"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Logistik"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestellmenge"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Prüftiefe_WE = (dataRow["Prüftiefe_WE"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Prüftiefe_WE"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Rabatt"]);
			Standardlieferant = Convert.ToBoolean(dataRow["Standardlieferant"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Umsatzsteuer"]);
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verpackungseinheit"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Wiederbeschaffungszeitraum"]);
			Zoll = (dataRow["Zoll"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zoll"]);
			Zusatz = (dataRow["Zusatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zusatz"]);
		}
	}
}

