namespace Psz.Core.MaterialManagement.Orders.Models.OrderValidation
{
	public class PlaceOrderResponseModel
	{

	}
	public class placeOrderRequestModel
	{
		public string OrderId { get; set; }
		public string SupplierEmail { get; set; }
		public string EmailTitle { get; set; }
		public string EmailBody { get; set; }
		public string OrderPlacementCCEmail { get; set; }
		public string IssuerEmail { get; set; }
		public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }


	}
}
