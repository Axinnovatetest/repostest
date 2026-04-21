namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class ArticleProjectTypeRequestModel
	{
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
		public string SearchTerms { get; set; }
	}
}
