using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class ProformaModelSearch
	{
		public List<ProformaModel> ProformaModel { get; set; }
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
