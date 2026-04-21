using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class LiquiditatsplanungOffeneMaterialbestellungenEntity
	{
		public int TotalCount { get; set; }
		public string Benutzer { get; set; }
		public DateTime? Bestätigter_Termin { get; set; }
		public int Lieferantennr { get; set; }
		public string Lieferant { get; set; }
		public int Bestellung_Nr { get; set; }
		public double Anzahl { get; set; }
		public double Mindestbestellmenge { get; set; }
		public double Verpackungseinheit { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Artikelnummer { get; set; }
		public string Bestellnummer { get; set; }
		public decimal Einzelpreis { get; set; }
		public double Gesamtpreis { get; set; }
		public DateTime? Anlieferung { get; set; }
		public int Zahlungsziel_Netto { get; set; }
		public DateTime? Falligkeit { get; set; }
		public string Produktionsstatte { get; set; }
		public string Mandant { get; set; }
		public int Bearbeiter { get; set; }
		public DateTime? Belegdatum { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public string Bemerkung_Pos { get; set; }
		public bool Standardlieferant { get; set; }
		public LiquiditatsplanungOffeneMaterialbestellungenEntity(DataRow dataRow)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Benutzer = (dataRow["Benutzer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Benutzer"].ToString());
			//Bestätigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"].ToString());
			Lieferantennr = ((string.IsNullOrEmpty(dataRow["Lieferantennr"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Lieferantennr"].ToString())) || dataRow["Lieferantennr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lieferantennr"].ToString());
			Lieferant = (dataRow["Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferant"].ToString());
			Bestellung_Nr = ((string.IsNullOrEmpty(dataRow["Bestellung-Nr"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Bestellung-Nr"].ToString())) || dataRow["Bestellung-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Bestellung-Nr"].ToString());
			Anzahl = ((string.IsNullOrEmpty(dataRow["Anzahl"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Anzahl"].ToString())) || dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Anzahl"].ToString());
			Mindestbestellmenge = ((string.IsNullOrEmpty(dataRow["Mindestbestellmenge"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Mindestbestellmenge"].ToString())) || dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Mindestbestellmenge"].ToString());
			Verpackungseinheit = ((string.IsNullOrEmpty(dataRow["Verpackungseinheit"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Verpackungseinheit"].ToString())) || dataRow["Verpackungseinheit"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Verpackungseinheit"].ToString());
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"].ToString());
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"].ToString());
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellnummer"].ToString());
			Einzelpreis = ((string.IsNullOrEmpty(dataRow["Einzelpreis"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Einzelpreis"].ToString())) || dataRow["Einzelpreis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Einzelpreis"].ToString());
			Gesamtpreis = ((string.IsNullOrEmpty(dataRow["Gesamtpreis"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Gesamtpreis"].ToString())) || dataRow["Gesamtpreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gesamtpreis"].ToString());
			Anlieferung = (dataRow["Anlieferung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Anlieferung"].ToString());
			Zahlungsziel_Netto = ((string.IsNullOrEmpty(dataRow["Zahlungsziel Netto"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Zahlungsziel Netto"].ToString())) || dataRow["Zahlungsziel Netto"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Zahlungsziel Netto"].ToString());
			Falligkeit = (dataRow["Fälligkeit"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Fälligkeit"].ToString());
			Produktionsstatte = (dataRow["Produktionsstätte"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Produktionsstätte"].ToString());
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"].ToString());
			Bearbeiter = ((string.IsNullOrEmpty(dataRow["Bearbeiter"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Bearbeiter"].ToString())) || dataRow["Bearbeiter"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Bearbeiter"].ToString());
			Belegdatum = (dataRow["Belegdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Belegdatum"].ToString());
			Wunschtermin = (dataRow["Wünschtermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wünschtermin"].ToString());
			Bemerkung_Pos = (dataRow["Bemerkung_Pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Pos"].ToString());
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"].ToString());
		}
	}
}
