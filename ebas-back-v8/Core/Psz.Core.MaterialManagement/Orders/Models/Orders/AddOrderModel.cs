namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class AddOrderRequestModel
	{
		public int SupplierId { get; set; }
		public int OrderType { get; set; }
	}
	public class AddOrderResponseModel
	{
		public int OrderId;
		public AddOrderResponseModel() { }
		public AddOrderResponseModel(int orderId)
		{
			OrderId = orderId;
		}
	}
}
