using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class WEnachDatumSearch
	{
		public DateTime DateBegin { get; set; }
		public DateTime DateEnd { get; set; }

		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
		public string SearchValue { get; set; }
	}
}
