using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Configuration.Article
{
	public class GetModel
	{
		public List<MinimalModel> Articles { get; set; } = new List<MinimalModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
