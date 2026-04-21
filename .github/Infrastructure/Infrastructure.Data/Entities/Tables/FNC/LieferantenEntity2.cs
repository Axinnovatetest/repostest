using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{


	public class LieferantenEntity2
	{
		public int? Belegkreis { get; set; }
		public bool? Bestellbestatigung_anmahnen { get; set; }
		public decimal? Bestellimit { get; set; }
		public string Branche { get; set; }
		public string EG___Identifikationsnummer { get; set; }
		public decimal? Eilzuschlag { get; set; }
		public decimal? Frachtfreigrenze { get; set; }
		public bool? gesperrt_fur_weitere_Bestellungen { get; set; }
		public string Grund_fur_Sperre { get; set; }
		public int? IMDS_Firmen_ID { get; set; }
		public string Karenztage { get; set; }
		public int? Konditionszuordnungs_Nr { get; set; }
		public string Kreditoren_Nr { get; set; }
		public string Kundennummer__Lieferanten_ { get; set; }
		public string Kundennummer_PSZ_AL__Lieferanten_ { get; set; }
		public string Kundennummer_PSZ_CZ__Lieferanten_ { get; set; }
		public string Kundennummer_PSZ_TN__Lieferanten_ { get; set; }
		public string Kundennummer_SC__Lieferanten_ { get; set; }
		public string Kundennummer_SC_CZ__Lieferanten_ { get; set; }
		public bool? LH { get; set; }
		public DateTime? LH_Datum { get; set; }
		public string Lieferantengruppe { get; set; }
		public bool? Mahnsperre { get; set; }
		public bool? Mahnsperre__Lieferant_ { get; set; }
		public decimal? Mindestbestellwert { get; set; }
		public int Nr { get; set; }
		public int? nummer { get; set; }
		public int? Rabattgruppe { get; set; }
		public int? Sprache { get; set; }
		public bool? Umsatzsteuer_berechnen { get; set; }
		public string Versandart { get; set; }
		public decimal? Versandkosten { get; set; }
		public int? Wahrung { get; set; }
		public string Wochentag_Anlieferung { get; set; }
		public string Zahlungsweise { get; set; }
		public Single? Zielaufschlag { get; set; }
		public decimal? Zuschlag_Mindestbestellwert { get; set; }

		public LieferantenEntity2() { }

		public LieferantenEntity2(DataRow dataRow)
		{
			Belegkreis = (dataRow["Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
			Bestellbestatigung_anmahnen = (dataRow["Bestellbestätigung anmahnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bestellbestätigung anmahnen"]);
			Bestellimit = (dataRow["Bestellimit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestellimit"]);
			Branche = (dataRow["Branche"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Branche"]);
			EG___Identifikationsnummer = (dataRow["EG - Identifikationsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EG - Identifikationsnummer"]);
			Eilzuschlag = (dataRow["Eilzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Eilzuschlag"]);
			Frachtfreigrenze = (dataRow["Frachtfreigrenze"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Frachtfreigrenze"]);
			gesperrt_fur_weitere_Bestellungen = (dataRow["gesperrt für weitere Bestellungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gesperrt für weitere Bestellungen"]);
			Grund_fur_Sperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
			IMDS_Firmen_ID = (dataRow["IMDS Firmen-ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IMDS Firmen-ID"]);
			Karenztage = (dataRow["Karenztage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Karenztage"]);
			Konditionszuordnungs_Nr = (dataRow["Konditionszuordnungs-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Konditionszuordnungs-Nr"]);
			Kreditoren_Nr = (dataRow["Kreditoren-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kreditoren-Nr"]);
			Kundennummer__Lieferanten_ = (dataRow["Kundennummer (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer (Lieferanten)"]);
			Kundennummer_PSZ_AL__Lieferanten_ = (dataRow["Kundennummer PSZ_AL (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer PSZ_AL (Lieferanten)"]);
			Kundennummer_PSZ_CZ__Lieferanten_ = (dataRow["Kundennummer PSZ_CZ (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer PSZ_CZ (Lieferanten)"]);
			Kundennummer_PSZ_TN__Lieferanten_ = (dataRow["Kundennummer PSZ_TN (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer PSZ_TN (Lieferanten)"]);
			Kundennummer_SC__Lieferanten_ = (dataRow["Kundennummer SC (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer SC (Lieferanten)"]);
			Kundennummer_SC_CZ__Lieferanten_ = (dataRow["Kundennummer SC_CZ (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer SC_CZ (Lieferanten)"]);
			LH = (dataRow["LH"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LH"]);
			LH_Datum = (dataRow["LH_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LH_Datum"]);
			Lieferantengruppe = (dataRow["Lieferantengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantengruppe"]);
			Mahnsperre = (dataRow["Mahnsperre"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mahnsperre"]);
			Mahnsperre__Lieferant_ = (dataRow["Mahnsperre (Lieferant)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mahnsperre (Lieferant)"]);
			Mindestbestellwert = (dataRow["Mindestbestellwert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestellwert"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			nummer = (dataRow["nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nummer"]);
			Rabattgruppe = (dataRow["Rabattgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Rabattgruppe"]);
			Sprache = (dataRow["Sprache"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Sprache"]);
			Umsatzsteuer_berechnen = (dataRow["Umsatzsteuer berechnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Umsatzsteuer berechnen"]);
			Versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			Versandkosten = (dataRow["Versandkosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Versandkosten"]);
			Wahrung = (dataRow["Währung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Währung"]);
			Wochentag_Anlieferung = (dataRow["Wochentag_Anlieferung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Wochentag_Anlieferung"]);
			Zahlungsweise = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
			Zielaufschlag = (dataRow["Zielaufschlag"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Zielaufschlag"]);
			Zuschlag_Mindestbestellwert = (dataRow["Zuschlag Mindestbestellwert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zuschlag Mindestbestellwert"]);
		}

		public LieferantenEntity2 ShallowClone()
		{
			return new LieferantenEntity2
			{
				Belegkreis = Belegkreis,
				Bestellbestatigung_anmahnen = Bestellbestatigung_anmahnen,
				Bestellimit = Bestellimit,
				Branche = Branche,
				EG___Identifikationsnummer = EG___Identifikationsnummer,
				Eilzuschlag = Eilzuschlag,
				Frachtfreigrenze = Frachtfreigrenze,
				gesperrt_fur_weitere_Bestellungen = gesperrt_fur_weitere_Bestellungen,
				Grund_fur_Sperre = Grund_fur_Sperre,
				IMDS_Firmen_ID = IMDS_Firmen_ID,
				Karenztage = Karenztage,
				Konditionszuordnungs_Nr = Konditionszuordnungs_Nr,
				Kreditoren_Nr = Kreditoren_Nr,
				Kundennummer__Lieferanten_ = Kundennummer__Lieferanten_,
				Kundennummer_PSZ_AL__Lieferanten_ = Kundennummer_PSZ_AL__Lieferanten_,
				Kundennummer_PSZ_CZ__Lieferanten_ = Kundennummer_PSZ_CZ__Lieferanten_,
				Kundennummer_PSZ_TN__Lieferanten_ = Kundennummer_PSZ_TN__Lieferanten_,
				Kundennummer_SC__Lieferanten_ = Kundennummer_SC__Lieferanten_,
				Kundennummer_SC_CZ__Lieferanten_ = Kundennummer_SC_CZ__Lieferanten_,
				LH = LH,
				LH_Datum = LH_Datum,
				Lieferantengruppe = Lieferantengruppe,
				Mahnsperre = Mahnsperre,
				Mahnsperre__Lieferant_ = Mahnsperre__Lieferant_,
				Mindestbestellwert = Mindestbestellwert,
				Nr = Nr,
				nummer = nummer,
				Rabattgruppe = Rabattgruppe,
				Sprache = Sprache,
				Umsatzsteuer_berechnen = Umsatzsteuer_berechnen,
				Versandart = Versandart,
				Versandkosten = Versandkosten,
				Wahrung = Wahrung,
				Wochentag_Anlieferung = Wochentag_Anlieferung,
				Zahlungsweise = Zahlungsweise,
				Zielaufschlag = Zielaufschlag,
				Zuschlag_Mindestbestellwert = Zuschlag_Mindestbestellwert
			};
		}
	}



}
