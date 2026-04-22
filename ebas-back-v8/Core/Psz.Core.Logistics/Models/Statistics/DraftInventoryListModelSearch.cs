using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class DraftInventoryListModelSearch
	{
		public List<DraftInventoryListModel> DraftInventoryListModel { get; set; } = new List<DraftInventoryListModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
