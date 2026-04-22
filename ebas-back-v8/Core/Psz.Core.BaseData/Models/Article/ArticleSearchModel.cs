using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleSearchModel
	{
		public string CustomerNr { get; set; }
		public string ArticleReference { get; set; }
		public string CustomerPrefix { get; set; }
		public string ArticleNummer { get; set; }
		public string ArticleDesignation { get; set; }
		public string GoodsGroup { get; set; }
		public string ArticleFamily { get; set; }
		public bool? active { get; set; }
		public string CustomerItemNumber { get; set; }
		public string Details { get; set; }
		public bool? EdiDefault { get; set; }
		public bool? EDrawing { get; set; }


		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
	public class CustomerSearchResponseModel
	{
		public string Key { get; set; }
		public string Value { get; set; }
		public string CustomerPrefix { get; set; }
	}
	public class ArticleAdvancedSearchModel
	{
		//public string CustomerNr { get; set; }
		//public string CustomerPrefix { get; set; }
		//public string ArticleNummer { get; set; }
		//public string ArticleDesignation { get; set; }
		//public string GoodsGroup { get; set; }
		//public string ArticleFamily { get; set; }
		//public bool? active { get; set; }
		//public string CustomerItemNumber { get; set; }
		//public string Details { get; set; }
		public List<SearchAdvanced> SearchAdvanced { get; set; }

		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
	public class SearchAdvanced
	{
		public int SearchColumn { get; set; }
		public string SearchValue { get; set; }
		public int SearchType { get; set; }
	}
}
