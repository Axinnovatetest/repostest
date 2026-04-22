namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class ArticleSearchModel
	{

		public string artikelnummer { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
	public class ArticleSearchMhdModel
	{
		public string artikelnummer { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}

}
