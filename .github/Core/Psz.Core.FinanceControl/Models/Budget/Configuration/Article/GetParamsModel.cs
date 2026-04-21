namespace Psz.Core.FinanceControl.Models.Budget.Configuration.Article
{
	public class GetParamsModel
	{
		public string SearchTerm { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public bool? SearchSupplierRef { get; set; }
	}
}
