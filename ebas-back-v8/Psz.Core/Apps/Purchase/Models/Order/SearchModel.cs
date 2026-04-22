using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Order
{
	public class SearchModel
	{
		public string ProjectNumber { get; set; }
		public string DocumentNumber { get; set; }
		public string PrefailNumber { get; set; }
		public bool OnlyInProgress { get; set; }
		public DateTime? GultigAb { get; set; }
		public DateTime? GultigBis { get; set; }
		public DateTime? CreatedFrom { get; set; }
		public DateTime? CreatedTo { get; set; }
		public int? BlanketTypeId { get; set; }
		public List<int> Types { get; set; } = new List<int>();
		public List<int> CustomersIds { get; set; } = new List<int>();
		//souilmi 23/08/2022
		public int? RechnungCustomerId { get; set; }
		public int? RechnungType { get; set; }
		// - 2023-01-18 
		public bool Unbooked { get; set; } = false;
		//

		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
}
