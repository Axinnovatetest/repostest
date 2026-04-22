namespace Psz.Core.Logistics.Models.Lagebewegung.Fertigung
{
	public class FALagerSearchModel
	{


		public string Fertigungsnummer { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
}
