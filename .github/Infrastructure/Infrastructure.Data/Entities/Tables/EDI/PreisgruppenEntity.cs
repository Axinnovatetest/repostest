using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class PreisgruppenEntity
	{
		public int? Artikel_Nr { get; set; }
		public decimal? Aufschlag { get; set; }
		public decimal? Aufschlagsatz { get; set; }
		public string Bemerkung { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_1 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_10 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_2 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_3 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_4 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_5 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_6 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_7 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_8 { get; set; }
		public decimal? bis_Anzahl_Mengeneinheiten_9 { get; set; }
		public bool? brutto { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? kalk_kosten { get; set; }
		public DateTime? letzte_Aktualisierung { get; set; }
		public decimal? ME1 { get; set; }
		public decimal? ME2 { get; set; }
		public decimal? ME3 { get; set; }
		public decimal? ME4 { get; set; }
		public int Nr { get; set; }
		public decimal? PM1 { get; set; }
		public decimal? PM2 { get; set; }
		public decimal? PM3 { get; set; }
		public decimal? PM4 { get; set; }
		public int? Preisgruppe { get; set; }
		public decimal? Preisminderung_1____ { get; set; }
		public decimal? Preisminderung_10____ { get; set; }
		public decimal? Preisminderung_2____ { get; set; }
		public decimal? Preisminderung_3____ { get; set; }
		public decimal? Preisminderung_4____ { get; set; }
		public decimal? Preisminderung_5____ { get; set; }
		public decimal? Preisminderung_6____ { get; set; }
		public decimal? Preisminderung_7____ { get; set; }
		public decimal? Preisminderung_8____ { get; set; }
		public decimal? Preisminderung_9____ { get; set; }
		public decimal? Staffelpreis1 { get; set; }
		public decimal? Staffelpreis2 { get; set; }
		public decimal? Staffelpreis3 { get; set; }
		public decimal? Staffelpreis4 { get; set; }
		public decimal? Verkaufspreis { get; set; }

		public PreisgruppenEntity() { }

		public PreisgruppenEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Aufschlag = (dataRow["Aufschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aufschlag"]);
			Aufschlagsatz = (dataRow["Aufschlagsatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aufschlagsatz"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			bis_Anzahl_Mengeneinheiten_1 = (dataRow["bis Anzahl Mengeneinheiten 1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 1"]);
			bis_Anzahl_Mengeneinheiten_10 = (dataRow["bis Anzahl Mengeneinheiten 10"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 10"]);
			bis_Anzahl_Mengeneinheiten_2 = (dataRow["bis Anzahl Mengeneinheiten 2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 2"]);
			bis_Anzahl_Mengeneinheiten_3 = (dataRow["bis Anzahl Mengeneinheiten 3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 3"]);
			bis_Anzahl_Mengeneinheiten_4 = (dataRow["bis Anzahl Mengeneinheiten 4"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 4"]);
			bis_Anzahl_Mengeneinheiten_5 = (dataRow["bis Anzahl Mengeneinheiten 5"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 5"]);
			bis_Anzahl_Mengeneinheiten_6 = (dataRow["bis Anzahl Mengeneinheiten 6"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 6"]);
			bis_Anzahl_Mengeneinheiten_7 = (dataRow["bis Anzahl Mengeneinheiten 7"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 7"]);
			bis_Anzahl_Mengeneinheiten_8 = (dataRow["bis Anzahl Mengeneinheiten 8"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 8"]);
			bis_Anzahl_Mengeneinheiten_9 = (dataRow["bis Anzahl Mengeneinheiten 9"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["bis Anzahl Mengeneinheiten 9"]);
			brutto = (dataRow["brutto"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["brutto"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			kalk_kosten = (dataRow["kalk_kosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["kalk_kosten"]);
			letzte_Aktualisierung = (dataRow["letzte_Aktualisierung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["letzte_Aktualisierung"]);
			ME1 = (dataRow["ME1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ME1"]);
			ME2 = (dataRow["ME2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ME2"]);
			ME3 = (dataRow["ME3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ME3"]);
			ME4 = (dataRow["ME4"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ME4"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			PM1 = (dataRow["PM1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PM1"]);
			PM2 = (dataRow["PM2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PM2"]);
			PM3 = (dataRow["PM3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PM3"]);
			PM4 = (dataRow["PM4"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PM4"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			Preisminderung_1____ = (dataRow["Preisminderung 1 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 1 (%)"]);
			Preisminderung_10____ = (dataRow["Preisminderung 10 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 10 (%)"]);
			Preisminderung_2____ = (dataRow["Preisminderung 2 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 2 (%)"]);
			Preisminderung_3____ = (dataRow["Preisminderung 3 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 3 (%)"]);
			Preisminderung_4____ = (dataRow["Preisminderung 4 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 4 (%)"]);
			Preisminderung_5____ = (dataRow["Preisminderung 5 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 5 (%)"]);
			Preisminderung_6____ = (dataRow["Preisminderung 6 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 6 (%)"]);
			Preisminderung_7____ = (dataRow["Preisminderung 7 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 7 (%)"]);
			Preisminderung_8____ = (dataRow["Preisminderung 8 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 8 (%)"]);
			Preisminderung_9____ = (dataRow["Preisminderung 9 (%)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preisminderung 9 (%)"]);
			Staffelpreis1 = (dataRow["Staffelpreis1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis1"]);
			Staffelpreis2 = (dataRow["Staffelpreis2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis2"]);
			Staffelpreis3 = (dataRow["Staffelpreis3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis3"]);
			Staffelpreis4 = (dataRow["Staffelpreis4"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Staffelpreis4"]);
			Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
		}
	}

	public class PreisgruppenStatisticsEntity
	{
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Bis1 { get; set; }
		public decimal? Bis2 { get; set; }
		public decimal? Bis3 { get; set; }
		public decimal? Bis4 { get; set; }
		public int Nr { get; set; }
		public decimal? Preis1 { get; set; }
		public decimal? Preis2 { get; set; }
		public decimal? Preis3 { get; set; }
		public decimal? Preis4 { get; set; }
	}
}

