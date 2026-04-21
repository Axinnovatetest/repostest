using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class InventurListRohmaterialSearch
	{
		public List<Rohmaterial> Rohmaterial { get; set; } = new List<Rohmaterial>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
