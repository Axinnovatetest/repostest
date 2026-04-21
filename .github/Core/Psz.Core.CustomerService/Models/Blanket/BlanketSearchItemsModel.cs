namespace Psz.Core.CustomerService.Models.Blanket
{
	public class BlanketSearchItemsModel
	{
		public string Bezeichnung1 { get; set; }

		public string Bezeichnung2 { get; set; }
		public string Einheit { get; set; }
		public decimal StandardPrice { get; set; }
		public BlanketSearchItemsModel()
		{

		}
	}
	public class BlanketSearchItemRequestModel
	{
		public int ArticleId { get; set; }
		public int SupplierAddressId { get; set; }
		public string ArticleNumber { get; set; }
	}
}
