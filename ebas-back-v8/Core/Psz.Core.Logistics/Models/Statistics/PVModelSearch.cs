namespace Psz.Core.Logistics.Models.Statistics
{
	public class PVModelSearch
	{
		public int PVSendungsnummer { get; set; }
		public int Lagernummer { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
		public string SearchValue { get; set; }
	}
}
