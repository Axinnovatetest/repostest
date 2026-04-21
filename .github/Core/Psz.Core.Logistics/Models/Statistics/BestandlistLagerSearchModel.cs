namespace Psz.Core.Logistics.Models.Statistics
{
	public class BestandlistLagerSearchModel
	{
		public int LagerID { get; set; }

		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
		public string SearchValue { get; set; }
	}
}
