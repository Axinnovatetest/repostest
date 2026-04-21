using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Customer
{
	public class SearchCustomerResponseModel
	{
		public List<CustomerModel> Customers { get; set; } = new List<CustomerModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

	}
}
