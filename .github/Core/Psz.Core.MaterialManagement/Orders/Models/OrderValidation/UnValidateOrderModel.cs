namespace Psz.Core.MaterialManagement.Orders.Models.OrderValidation
{

	public class UnValidateResponseModel
	{
		public bool Success { get; set; }
	}
	public class UnValidateRequestModel
	{
		public int OrderNumber { get; set; }
	}
}
