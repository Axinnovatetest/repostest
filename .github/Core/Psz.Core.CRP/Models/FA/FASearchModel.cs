namespace Psz.Core.CRP.Models.FA
{
	public class FASearchModel
	{
		public string Kunde { get; set; }
		public string? Artikelnummer { get; set; }
		public string? Fertigungsnummer { get; set; }
		public string? FA_Status { get; set; }
		public int? Lager { get; set; }
		public bool? Gestart { get; set; }
		public bool? Prio { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string? SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
}