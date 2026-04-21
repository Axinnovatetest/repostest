using System.Collections.Generic;

namespace Psz.Core.Common.Models
{
	public class IPaginatedRequestModel
	{
		// - 
		public int RequestedPage { get; set; } = 0;
		public int PageSize { get; set; } = 10;
		public bool SortDesc { get; set; } = true;
		public string SortField { get; set; }
		// -
		public bool FullData { get; set; } = false;
	}

	public class IPaginatedResponseModel<T>
	{
		public int TotalCount { get; set; }
		public int TotalPageCount { get; set; }
		public int PageRequested { get; set; }
		public int PageSize { get; set; }
		public List<T> Items { get; set; }
	}

}
