namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class InterTransferSearchModel
	{
		public int type { get; set; }
		public string artikelnummer { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
}
