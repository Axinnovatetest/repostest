using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class GetAuschusskosten_Technik_InfoSearchModel
	{
		public List<GetAuschusskosten_Technik_InfoModel> Auschusskosten { get; set; }
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
