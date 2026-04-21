using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class InventurlistePetraModelSearch
	{
		public List<InventurlistePetraModel> InventurlistePetraModel { get; set; } = new List<InventurlistePetraModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
