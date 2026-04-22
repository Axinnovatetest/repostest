using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class ExportEntity
	{
		public decimal? Preis { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public DateTime? Datum { get; set; }
		public string Artikelnummer { get; set; }
		public int? Fertigungsnummer { get; set; }
		public DateTime? Uhrzeit { get; set; }
		public string Bezeichnung { get; set; }
		public string Auftragsbemerkung { get; set; }
		public int? Anzahl_Auftrag { get; set; }
		public int? Anzahl_aktuelle_Lieferung { get; set; }
		public int? Anzahl_Kartons { get; set; }
		public string Mitarbeiter_Name { get; set; }
		public string Bemerkung { get; set; }
		public DateTime? Ausdr1 { get; set; }
		public DateTime? Ausdr2 { get; set; }
		public string Lagerort { get; set; }
		public string CS_Kontakt { get; set; }
		public string Kennzeichen { get; set; }
		public int? Liefermenge_QRCode { get; set; }

		public ExportEntity(DataRow dataRow)
		{
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Uhrzeit = (dataRow["Uhrzeit"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Uhrzeit"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			Auftragsbemerkung = (dataRow["Auftragsbemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Auftragsbemerkung"]);
			Anzahl_Auftrag = (dataRow["Anzahl_Auftrag"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_Auftrag"]);
			Anzahl_aktuelle_Lieferung = (dataRow["Anzahl_aktuelle Lieferung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_aktuelle Lieferung"]);
			Anzahl_Kartons = (dataRow["Anzahl_Kartons"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_Kartons"]);
			Mitarbeiter_Name = (dataRow["Mitarbeiter_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter_Name"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Ausdr1 = (dataRow["Ausdr1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ausdr1"]);
			Ausdr2 = (dataRow["Ausdr2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ausdr2"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Liefermenge_QRCode = (dataRow["Liefermenge_QRCode"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Liefermenge_QRCode"]);
		}
	}
}
