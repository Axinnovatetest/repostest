namespace Psz.Core.Logistics.Models.Verpackung
{
	public class HistoriePackingSearchModel
	{

		public string SearchTerms { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }

	}
}
