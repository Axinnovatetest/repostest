using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.CRP
{
	public class ArticleEntity
	{
		public string Artikelnummer { get; set; }
		public string Country { get; set; }
		public decimal? Differenz_Time_pro_Losgrosse_in_FA_vs_Prod { get; set; }
		public decimal? Differenz_Time_pro_Losgrosse_P3000_vs_Prod { get; set; }
		public string Hall { get; set; }
		public int? P3000_losgrosse { get; set; }
		public decimal? P3000_Vorgabezeit_min { get; set; }
		public int? Real_Losgrosse_der_letzten_5_FA { get; set; }
		public string Status_Extern { get; set; }
		public string Status_Intern { get; set; }
		public decimal? Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min { get; set; }
		public decimal? Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min { get; set; }
		public decimal? Total_Operation_Time_laut_AP_pro_Stuck_in_min { get; set; }

		public ArticleEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Country = (dataRow["Country"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Country"]);
			Differenz_Time_pro_Losgrosse_in_FA_vs_Prod = (dataRow["Differenz_Time_pro_Losgrosse_in_FA_vs_Prod"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Differenz_Time_pro_Losgrosse_in_FA_vs_Prod"]);
			Differenz_Time_pro_Losgrosse_P3000_vs_Prod = (dataRow["Differenz_Time_pro_Losgrosse_P3000_vs_Prod"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Differenz_Time_pro_Losgrosse_P3000_vs_Prod"]);
			Hall = (dataRow["Hall"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Hall"]);
			P3000_losgrosse = (dataRow["P3000 losgrosse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["P3000 losgrosse"]);
			P3000_Vorgabezeit_min = (dataRow["P3000_Vorgabezeit_min"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["P3000_Vorgabezeit_min"]);
			Real_Losgrosse_der_letzten_5_FA = (dataRow["Real_Losgrosse_der_letzten_5_FA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Real_Losgrosse_der_letzten_5_FA"]);
			Status_Extern = (dataRow["Status_Extern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status_Extern"]);
			Status_Intern = (dataRow["Status_Intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status_Intern"]);
			Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min = (dataRow["Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min"]);
			Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min = (dataRow["Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min"]);
			Total_Operation_Time_laut_AP_pro_Stuck_in_min = (dataRow["Total_Operation_Time_laut_AP_pro_Stuck_in_min"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Total_Operation_Time_laut_AP_pro_Stuck_in_min"]);
		}
	}
	public class ArticleWpl
	{
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Warengruppe { get; set; }
		public Single? Produktionszeit { get; set; }
		public string Freigabestatus { get; set; }
		public string Freigabestatus_TN_intern { get; set; }
		public string Prufstatus_TN_Ware { get; set; }
		public decimal? Stundensatz { get; set; }
		public int? Losgroesse { get; set; }
		public ArticleWpl(DataRow dataRow)
		{
			Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
			Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Produktionszeit"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Freigabestatus_TN_intern = (dataRow["Freigabestatus TN intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus TN intern"]);
			Prufstatus_TN_Ware = (dataRow["Prüfstatus TN Ware"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Prüfstatus TN Ware"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
			Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Losgroesse"]);
		}
	}
}
