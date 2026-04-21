using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class MainRequestModelSearch
	{
		public List<MainRequestModel> mainRequestModel { get; set; } = new List<MainRequestModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
