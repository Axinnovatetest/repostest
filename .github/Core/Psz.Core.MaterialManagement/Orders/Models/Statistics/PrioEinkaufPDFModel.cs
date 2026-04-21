namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class PrioEinkaufPDFRequestModel
	{
		public int LagerId { get; set; }
		public int Type { get; set; }
		public string OrderId { get; set; }
		public string ArticleNummer { get; set; }
		public string SupplierName { get; set; }
	}
}
