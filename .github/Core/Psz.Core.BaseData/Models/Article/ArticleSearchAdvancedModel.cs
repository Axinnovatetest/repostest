using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleSearchAdvancedModel
	{
		public List<ISearchArticlePosition> ListeSearchArticleAdvanced { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}

	public class ISearchArticlePosition
	{
		public int typeColum { get; set; }
		public string typeColumString { get; set; }
		public string inputColum { get; set; }
	}
}
