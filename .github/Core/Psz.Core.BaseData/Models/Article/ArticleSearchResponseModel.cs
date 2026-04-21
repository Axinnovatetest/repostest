using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleSearchResponseModel
	{
		public List<ArticleMinimalModel> Articles { get; set; } = new List<ArticleMinimalModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
