namespace Psz.Core.CustomerService.Models.OrderProcessing
{
	public class CreateOrderItemModel
	{
		public int OrderId { get; set; }
		public string ItemNumber { get; set; }
		public int? ItemTypeId { get; set; }
		public decimal? Quantity { get; set; }
		public int? Position { get; set; }
		public string Headline { get; set; }
		public string CSInterneBemerkung { get; set; }
	}
}
