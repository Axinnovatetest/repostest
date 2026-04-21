using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class BruttoBedarfListEntity
	{
		public bool? FA_Gestartet { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Artikel_H { get; set; }
		public string Bezeichnung_D { get; set; }
		public decimal? Anzahl_F { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public int? Artikel_Nr_des_Bauteils { get; set; }
		public string Artikel_Bau { get; set; }
		public string Bezeichnung_des_Bauteils { get; set; }
		public decimal? St_Anzahl { get; set; }
		public decimal? Bruttobedarf { get; set; }
		public DateTime? Termin_Materialbedarf { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public string Bezeichnung_H { get; set; }
		public bool? Kommisioniert_teilweise { get; set; }
		public bool? Kommisioniert_komplett { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public string Freigabestatus { get; set; }
		public string FreigabestatusTN_Int { get; set; }
		public decimal? Verfug_Ini { get; set; }
		public BruttoBedarfListEntity()
		{

		}
		public BruttoBedarfListEntity(DataRow dataRow)
		{
			FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FA_Gestartet"].ToString());
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"].ToString());
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"].ToString());
			Artikel_H = (dataRow["Artikel_H"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel_H"].ToString());
			Bezeichnung_D = (dataRow["Bezeichnung_D"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung_D"].ToString());
			Anzahl_F = (dataRow["Anzahl_F"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl_F"].ToString());
			Termin_Fertigstellung = (dataRow["Termin_Fertigstellung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Fertigstellung"].ToString());
			Artikel_Nr_des_Bauteils = (dataRow["Artikel-Nr des Bauteils"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr des Bauteils"].ToString());
			Artikel_Bau = (dataRow["Artikel_Bau"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel_Bau"].ToString());
			Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"].ToString());
			St_Anzahl = (dataRow["St_Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["St_Anzahl"].ToString());
			Bruttobedarf = (dataRow["Bruttobedarf"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bruttobedarf"].ToString());
			Termin_Materialbedarf = (dataRow["Termin_Materialbedarf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Materialbedarf"].ToString());
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"].ToString());
			Termin_Bestatigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"].ToString());
			Bezeichnung_H = (dataRow["Bezeichnung_H"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung_H"].ToString());
			Kommisioniert_teilweise = (dataRow["Kommisioniert_teilweise"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_teilweise"].ToString());
			Kommisioniert_komplett = (dataRow["Kommisioniert_komplett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kommisioniert_komplett"].ToString());
			Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"].ToString());
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"].ToString());
			FreigabestatusTN_Int = (dataRow["FreigabestatusTN_Int"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FreigabestatusTN_Int"].ToString());
			Verfug_Ini = (dataRow["Verfug_Ini"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verfug_Ini"].ToString());
		}
	}
}
