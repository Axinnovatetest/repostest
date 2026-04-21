using System;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class EntnahmeDetailsSearchModel
	{
		public string artikelnummer { get; set; }
		public int lager { get; set; }
		public DateTime? datum { get; set; }
		public int type { get; set; }
		public bool ek { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
}
