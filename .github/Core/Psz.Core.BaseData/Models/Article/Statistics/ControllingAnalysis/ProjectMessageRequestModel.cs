namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class ProjectMessageRequestModel
	{
		public string ArticleNumber { get; set; }
		public string ProjectNumber { get; set; }

		// - pagination data
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }

	}
}
