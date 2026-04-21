using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Reception
{
	public class SearchResponseModel<T>
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public List<T> Results { get; set; }
		public int AllCount { get; set; }
	}
}
