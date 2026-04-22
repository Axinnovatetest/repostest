using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class FATechnikPlanungEntity
	{
		public int? Lagerort_id { get; set; }
		public bool? Erstmuster { get; set; }
		public bool? Sonderfertigung { get; set; }
		public string Techniker { get; set; }
		public DateTime? AB_Termin { get; set; }
		public DateTime? Plan { get; set; }
		public string Termin_besprochen { get; set; }
		public string PSZ { get; set; }
		public Decimal? Menge { get; set; }
		public Decimal? Offen_Anzahl { get; set; }
		public int? FA { get; set; }
		public Decimal? Zeit_in_min_pro_Stück { get; set; }
		public string Status { get; set; }
		public string Prüfstatus_TN_Ware { get; set; }
		public string Status_intern { get; set; }
		public string Bemerkung_Technik { get; set; }
		public string Info_CS { get; set; }
		public bool? Quick_Area { get; set; }
		public bool? Kommisioniert_teilweise { get; set; }
		public bool? Kommisioniert_komplett { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public DateTime? Kabel_geschnitten_Datum { get; set; }
		public bool? FA_Gestartet { get; set; }
		public string Urs_Artikelnummer { get; set; }
		public int ArtikelNr { get; set; }

		public FATechnikPlanungEntity(DataRow dataRow)
		{
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			Sonderfertigung = (dataRow["Sonderfertigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Sonderfertigung"]);
			Techniker = (dataRow["Techniker"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Techniker"]);
			AB_Termin = (dataRow["AB_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AB_Termin"]);
			Plan = (dataRow["Plan"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Plan"]);
			Termin_besprochen = (dataRow["Termin besprochen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin besprochen"]);
			PSZ = (dataRow["PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			Offen_Anzahl = (dataRow["Offen_Anzahl"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Offen_Anzahl"]);
			FA = (dataRow["FA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA"]);
			Zeit_in_min_pro_Stück = (dataRow["Zeit in min pro Stück"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Zeit in min pro Stück"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			Prüfstatus_TN_Ware = (dataRow["Prüfstatus TN Ware"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfstatus TN Ware"]);
			Status_intern = (dataRow["Status intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status intern"]);
			Bemerkung_Technik = (dataRow["Bemerkung_Technik"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Technik"]);
			Info_CS = (dataRow["Info CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Info CS"]);
			Quick_Area = (dataRow["Quick_Area"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Quick_Area"]);
			Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_teilweise"]);
			Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_komplett"]);
			Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
			Kabel_geschnitten_Datum = (dataRow["Kabel_geschnitten_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Kabel_geschnitten_Datum"]);
			FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FA_Gestartet"]);
			Urs_Artikelnummer = (dataRow["Urs-Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Urs-Artikelnummer"]);
			ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ?0  : Convert.ToInt32(dataRow["ArtikelNr"]);
		}
	}
}
