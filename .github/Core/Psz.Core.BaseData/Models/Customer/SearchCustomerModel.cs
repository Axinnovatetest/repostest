namespace Psz.Core.BaseData.Models.Customer
{
	public class SearchCustomerModel
	{
		public string CustomerName { get; set; }
		public string CustomerNumber { get; set; }


		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }

		public bool ArchivedOnly { get; set; }
	}
}
