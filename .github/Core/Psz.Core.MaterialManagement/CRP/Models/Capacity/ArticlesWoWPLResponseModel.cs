namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class ArticlesWoWPLRequestModel
	{
		public bool? warengruppeEF { get; set; }
		public bool? wStuckliste { get; set; }
		public bool? wFa { get; set; }
		public bool? wOpenFa { get; set; }
		public int? lager { get; set; }
		public DateTime? faDateFrom { get; set; }
		public DateTime? faDateTill { get; set; }
	}

	public class ArticlesWoWPLResponseModel
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
		public ArticlesWoWPLResponseModel(Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleWpl artikelEntity)
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
