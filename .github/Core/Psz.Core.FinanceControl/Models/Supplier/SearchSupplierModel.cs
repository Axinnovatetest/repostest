namespace Psz.Core.FinanceControl.Models.Supplier
{
	public class SearchSupplierModel
	{
		public string SupplierName { get; set; }
		public string SupplierNumber { get; set; }


		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
	}
}
