using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class BestellnummernEntity
	{
		public string Angebot { get; set; }
		public DateTime? Angebot_Datum { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		public int? Artikel_Nr { get; set; }
		public decimal? Basispreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public DateTime? Einkaufspreis_gultig_bis { get; set; }
		public decimal? Einkaufspreis1 { get; set; }
		public DateTime? Einkaufspreis1_gultig_bis { get; set; }
		public decimal? Einkaufspreis2 { get; set; }
		public DateTime? Einkaufspreis2_gultig_bis { get; set; }
		public decimal? EK_EUR { get; set; }
		public decimal? EK_total { get; set; }
		public decimal? Fracht { get; set; }
		public DateTime? letzte_Aktualisierung { get; set; }
		public int? Lieferanten_Nr { get; set; }
		public decimal? Logistik { get; set; }
		public Single? Mindestbestellmenge { get; set; }
		public int Nr { get; set; }
		public decimal? Preiseinheit { get; set; }
		public int? Pruftiefe_WE { get; set; }
		public Single? Rabatt { get; set; }
		public bool? Standardlieferant { get; set; }
		public decimal? Umsatzsteuer { get; set; }
		public Single? Verpackungseinheit { get; set; }
		public string Warengruppe { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }
		public decimal? Zoll { get; set; }
		public decimal? Zusatz { get; set; }

		public BestellnummernEntity() { }

		public BestellnummernEntity(DataRow dataRow)
		{
			Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
			Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebot_Datum"]);
			Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
			Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Basispreis = (dataRow["Basispreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Basispreis"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Einkaufspreis_gultig_bis = (dataRow["Einkaufspreis gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis gültig bis"]);
			Einkaufspreis1 = (dataRow["Einkaufspreis1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis1"]);
			Einkaufspreis1_gultig_bis = (dataRow["Einkaufspreis1 gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis1 gültig bis"]);
			Einkaufspreis2 = (dataRow["Einkaufspreis2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis2"]);
			Einkaufspreis2_gultig_bis = (dataRow["Einkaufspreis2 gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis2 gültig bis"]);
			EK_EUR = (dataRow["EK_EUR"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_EUR"]);
			EK_total = (dataRow["EK_total"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_total"]);
			Fracht = (dataRow["Fracht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Fracht"]);
			letzte_Aktualisierung = (dataRow["letzte_Aktualisierung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["letzte_Aktualisierung"]);
			Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferanten-Nr"]);
			Logistik = (dataRow["Logistik"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Logistik"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Mindestbestellmenge"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Pruftiefe_WE = (dataRow["Prüftiefe_WE"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Prüftiefe_WE"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rabatt"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Umsatzsteuer"]);
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Verpackungseinheit"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
			Zoll = (dataRow["Zoll"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zoll"]);
			Zusatz = (dataRow["Zusatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zusatz"]);
		}

		public BestellnummernEntity ShallowClone()
		{
			return new BestellnummernEntity
			{
				Angebot = Angebot,
				Angebot_Datum = Angebot_Datum,
				Artikelbezeichnung = Artikelbezeichnung,
				Artikelbezeichnung2 = Artikelbezeichnung2,
				Artikel_Nr = Artikel_Nr,
				Basispreis = Basispreis,
				Bemerkungen = Bemerkungen,
				Bestell_Nr = Bestell_Nr,
				Einkaufspreis = Einkaufspreis,
				Einkaufspreis_gultig_bis = Einkaufspreis_gultig_bis,
				Einkaufspreis1 = Einkaufspreis1,
				Einkaufspreis1_gultig_bis = Einkaufspreis1_gultig_bis,
				Einkaufspreis2 = Einkaufspreis2,
				Einkaufspreis2_gultig_bis = Einkaufspreis2_gultig_bis,
				EK_EUR = EK_EUR,
				EK_total = EK_total,
				Fracht = Fracht,
				letzte_Aktualisierung = letzte_Aktualisierung,
				Lieferanten_Nr = Lieferanten_Nr,
				Logistik = Logistik,
				Mindestbestellmenge = Mindestbestellmenge,
				Nr = Nr,
				Preiseinheit = Preiseinheit,
				Pruftiefe_WE = Pruftiefe_WE,
				Rabatt = Rabatt,
				Standardlieferant = Standardlieferant,
				Umsatzsteuer = Umsatzsteuer,
				Verpackungseinheit = Verpackungseinheit,
				Warengruppe = Warengruppe,
				Wiederbeschaffungszeitraum = Wiederbeschaffungszeitraum,
				Zoll = Zoll,
				Zusatz = Zusatz
			};
		}
	}
}

