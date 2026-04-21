using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class ExcessRohmaterialSearchListModel
	{
		//ExcessRohmaterialModel
		public List<ExcessRohmaterialModel> ExcessRohmaterialModel { get; set; } = new List<ExcessRohmaterialModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
