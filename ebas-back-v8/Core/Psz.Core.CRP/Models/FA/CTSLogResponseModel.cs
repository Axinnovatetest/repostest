using System.Collections.Generic;

namespace Psz.Core.CRP.Models.FA
{
	public class CTSLogResponseModel
	{
		public List<CTSLogModel> CTSLog { get; set; }
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
