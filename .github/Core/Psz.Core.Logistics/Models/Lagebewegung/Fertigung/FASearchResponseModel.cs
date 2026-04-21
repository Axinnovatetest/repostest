using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Lagebewegung.Fertigung
{
	public class FASearchResponseModel
	{
		public List<FALagerModel> Orders { get; set; } = new List<FALagerModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
