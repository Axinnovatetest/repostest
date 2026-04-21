using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class SearchSupplierResponseModel
	{
		public List<MinimalSupplierModel> Suppliers { get; set; } = new List<MinimalSupplierModel>();
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

	}
}
