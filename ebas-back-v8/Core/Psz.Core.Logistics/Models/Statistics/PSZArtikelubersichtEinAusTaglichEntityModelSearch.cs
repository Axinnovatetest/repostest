using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class PSZArtikelubersichtEinAusTaglichEntityModelSearch
	{
		public List<PSZArtikelubersichtEinAusTaglichEntityModel> PSZArtikelubersichtEinAusTaglichEntityModel { get; set; } = new List<PSZArtikelubersichtEinAusTaglichEntityModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
