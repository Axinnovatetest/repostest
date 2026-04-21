namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class BomCandidateSearchRequestModel
	{
		public int ArticleId { get; set; }
		public string SearchNummer { get; set; }
		public int? MaxItemsNumber { get; set; }
	}

	public class BomCandidateSearchResponseModel
	{
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleDesignation { get; set; }
	}
}
