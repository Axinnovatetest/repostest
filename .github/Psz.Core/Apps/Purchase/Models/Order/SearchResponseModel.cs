using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Order
{
	public class SearchResponseModel
	{
		public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
