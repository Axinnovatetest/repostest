namespace Psz.Core.Apps.WorkPlan.Models.WorkSchedule
{
	public class ArticleWPLTimeDiffModel
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
		public ArticleWPLTimeDiffModel(Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleEntity artikelEntity)
		{
			if(artikelEntity == null)
				return;

			Artikelnummer = artikelEntity.Artikelnummer;
			Country = artikelEntity.Country;
			Differenz_Time_pro_Losgrosse_in_FA_vs_Prod = artikelEntity.Differenz_Time_pro_Losgrosse_in_FA_vs_Prod;
			Differenz_Time_pro_Losgrosse_P3000_vs_Prod = artikelEntity.Differenz_Time_pro_Losgrosse_P3000_vs_Prod;
			Hall = artikelEntity.Hall;
			P3000_losgrosse = artikelEntity.P3000_losgrosse;
			P3000_Vorgabezeit_min = artikelEntity.P3000_Vorgabezeit_min;
			Real_Losgrosse_der_letzten_5_FA = artikelEntity.Real_Losgrosse_der_letzten_5_FA;
			Status_Extern = artikelEntity.Status_Extern;
			Status_Intern = artikelEntity.Status_Intern;
			Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min = artikelEntity.Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min;
			Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min = artikelEntity.Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min;
			Total_Operation_Time_laut_AP_pro_Stuck_in_min = artikelEntity.Total_Operation_Time_laut_AP_pro_Stuck_in_min;
		}
	}
}
