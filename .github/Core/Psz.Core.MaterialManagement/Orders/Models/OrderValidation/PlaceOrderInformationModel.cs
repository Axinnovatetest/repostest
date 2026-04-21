namespace Psz.Core.MaterialManagement.Orders.Models.OrderValidation
{
	public class PlaceOrderInformationRequestModel
	{
		public int OrderNumber { get; set; }
	}

	public class PlaceOrderInformationResponseModel
	{
		public string SupplierEmail { get; set; }
	}
}
