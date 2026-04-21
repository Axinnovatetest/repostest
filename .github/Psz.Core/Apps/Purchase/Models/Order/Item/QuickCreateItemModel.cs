namespace Psz.Core.Apps.Purchase.Models.Order.Element
{
	public class QuickCreateItemModel
	{
		public int OrderId { get; set; }
		public string ItemNumber { get; set; }
		public int? ItemTypeId { get; set; }
		public decimal? Quantity { get; set; }

	}
}
