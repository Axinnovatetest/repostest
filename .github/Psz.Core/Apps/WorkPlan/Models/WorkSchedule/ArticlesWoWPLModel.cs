namespace Psz.Core.Apps.WorkPlan.Models.WorkSchedule
{
	public class ArticlesWoWPLModel
	{
		public string ArticleNumber { get; set; }
		public string Designation1 { get; set; }
		public string StatusRelease { get; set; }
		public string StatusInternalTN { get; set; }
		public string StatusCheckTN { get; set; }
		public string ProductGroup { get; set; }
		public string ProductionTime { get; set; }
		public string HourlyRate { get; set; }
		public string LotSize { get; set; }
		public ArticlesWoWPLModel(Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleWpl artikelEntity)
		{
			if(artikelEntity == null)
				return;

			ArticleNumber = artikelEntity.Artikelnummer;
			Designation1 = artikelEntity.Bezeichnung_1;
			ProductGroup = artikelEntity.Warengruppe;
			ProductionTime = artikelEntity.Produktionszeit?.ToString();
			StatusRelease = artikelEntity.Freigabestatus;
			StatusInternalTN = artikelEntity.Freigabestatus_TN_intern;
			StatusCheckTN = artikelEntity.Prufstatus_TN_Ware;
			HourlyRate = artikelEntity.Stundensatz?.ToString();
			LotSize = artikelEntity.Losgroesse?.ToString();
		}
	}
}
