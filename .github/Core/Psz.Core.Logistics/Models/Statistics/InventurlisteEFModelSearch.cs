using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class InventurlisteEFModelSearch
	{
		public List<InventurlisteEFModel> InventurlisteEFModel { get; set; } = new List<InventurlisteEFModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
