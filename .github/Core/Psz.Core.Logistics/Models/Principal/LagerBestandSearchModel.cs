namespace Psz.Core.Logistics.Models.Principal
{
	public class LagerBestandSearchModel
	{
		public string SearchTerms { get; set; }
		public int ArtikelNr { get; set; }
		public string Lagerort { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
}
