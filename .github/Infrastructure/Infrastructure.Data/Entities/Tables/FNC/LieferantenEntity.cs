using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class LieferantenEntity
	{
		public int? Belegkreis { get; set; }
		public bool? Bestellbestatigung_anmahnen { get; set; }
		public double? Bestellimit { get; set; }
		public string Branche { get; set; }
		public string EG_Identifikationsnummer { get; set; }
		public double? Eilzuschlag { get; set; }
		public double? Frachtfreigrenze { get; set; }
		public bool? Gesperrt_fur_weitere_Bestellungen { get; set; }
		public string Grund_fur_Sperre { get; set; }
		public string Karenztage { get; set; }
		public int? Konditionszuordnungs_Nr { get; set; }
		public string Kreditoren_Nr { get; set; }
		public string Kundennummer_Lieferanten { get; set; }
		public string Kundennummer_PSZ_AL_Lieferanten { get; set; }
		public string Kundennummer_PSZ_CZ_Lieferanten { get; set; }
		public string Kundennummer_PSZ_TN_Lieferanten { get; set; }
		public string Kundennummer_SC_Lieferanten { get; set; }
		public string Kundennummer_SC_CZ_Lieferanten { get; set; }
		public bool? LH { get; set; }
		public DateTime? LH_Datum { get; set; }
		public string Lieferantengruppe { get; set; }
		public bool? Mahnsperre { get; set; }
		public bool? Mahnsperre_Lieferant { get; set; }
		public double? Mindestbestellwert { get; set; }
		public int Nr { get; set; }
		public int? Nummer { get; set; }
		public int? Rabattgruppe { get; set; }
		public int? Sprache { get; set; }
		public bool? Umsatzsteuer_berechnen { get; set; }
		public string Versandart { get; set; }
		public double? Versandkosten { get; set; }
		public int? Wahrung { get; set; }
		public string Wochentag_Anlieferung { get; set; }
		public string Zahlungsweise { get; set; }
		public double? Zielaufschlag { get; set; }
		public double? Zuschlag_Mindestbestellwert { get; set; }

		public LieferantenEntity() { }
		public LieferantenEntity(DataRow dataRow)
		{
			Belegkreis = (dataRow["Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
			Bestellbestatigung_anmahnen = (dataRow["Bestellbestätigung anmahnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bestellbestätigung anmahnen"]);
			Bestellimit = (dataRow["Bestellimit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Bestellimit"]);
			Branche = (dataRow["Branche"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Branche"]);
			EG_Identifikationsnummer = (dataRow["EG - Identifikationsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EG - Identifikationsnummer"]);
			Eilzuschlag = (dataRow["Eilzuschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Eilzuschlag"]);
			Frachtfreigrenze = (dataRow["Frachtfreigrenze"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Frachtfreigrenze"]);
			Gesperrt_fur_weitere_Bestellungen = (dataRow["gesperrt für weitere Bestellungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gesperrt für weitere Bestellungen"]);
			Grund_fur_Sperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
			Karenztage = (dataRow["Karenztage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Karenztage"]);
			Konditionszuordnungs_Nr = (dataRow["Konditionszuordnungs-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Konditionszuordnungs-Nr"]);
			Kreditoren_Nr = (dataRow["Kreditoren-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kreditoren-Nr"]);
			Kundennummer_Lieferanten = (dataRow["Kundennummer (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer (Lieferanten)"]);
			Kundennummer_PSZ_AL_Lieferanten = (dataRow["Kundennummer PSZ_AL (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer PSZ_AL (Lieferanten)"]);
			Kundennummer_PSZ_CZ_Lieferanten = (dataRow["Kundennummer PSZ_CZ (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer PSZ_CZ (Lieferanten)"]);
			Kundennummer_PSZ_TN_Lieferanten = (dataRow["Kundennummer PSZ_TN (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer PSZ_TN (Lieferanten)"]);
			Kundennummer_SC_Lieferanten = (dataRow["Kundennummer SC (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer SC (Lieferanten)"]);
			Kundennummer_SC_CZ_Lieferanten = (dataRow["Kundennummer SC_CZ (Lieferanten)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundennummer SC_CZ (Lieferanten)"]);
			LH = (dataRow["LH"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LH"]);
			LH_Datum = (dataRow["LH_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LH_Datum"]);
			Lieferantengruppe = (dataRow["Lieferantengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantengruppe"]);
			Mahnsperre = (dataRow["Mahnsperre"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mahnsperre"]);
			Mahnsperre_Lieferant = (dataRow["Mahnsperre (Lieferant)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mahnsperre (Lieferant)"]);
			Mindestbestellwert = (dataRow["Mindestbestellwert"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mindestbestellwert"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Nummer = (dataRow["nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nummer"]);
			Rabattgruppe = (dataRow["Rabattgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Rabattgruppe"]);
			Sprache = (dataRow["Sprache"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Sprache"]);
			Umsatzsteuer_berechnen = (dataRow["Umsatzsteuer berechnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Umsatzsteuer berechnen"]);
			Versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			Versandkosten = (dataRow["Versandkosten"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Versandkosten"]);
			Wahrung = (dataRow["Währung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Währung"]);
			Wochentag_Anlieferung = (dataRow["Wochentag_Anlieferung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Wochentag_Anlieferung"]);
			Zahlungsweise = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
			Zielaufschlag = (dataRow["Zielaufschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Zielaufschlag"]);
			Zuschlag_Mindestbestellwert = (dataRow["Zuschlag Mindestbestellwert"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Zuschlag Mindestbestellwert"]);
		}
	}
}

