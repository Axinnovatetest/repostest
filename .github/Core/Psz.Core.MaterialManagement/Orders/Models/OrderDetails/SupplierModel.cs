namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class SupplierResponseModel
	{
		public int Id { get; set; }
		public string SupplierName { get; set; }
		public bool IsDefault { get; set; }
		public decimal MinimumOrderQuantity { get; set; }
		public decimal PurchasePrice { get; set; }
		public decimal ShippingDays { get; set; }
	}
	public class SupplierRequestModel
	{
		public int ArticleId { get; set; }
	}
}
