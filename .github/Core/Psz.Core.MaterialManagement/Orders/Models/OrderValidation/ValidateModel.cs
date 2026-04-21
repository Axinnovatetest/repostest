namespace Psz.Core.MaterialManagement.Orders.Models.OrderValidation
{
	public class ValidateResponseModel
	{

	}
	public class ValidateRequestModel
	{
		public int OrderNumber { get; set; }
		public string Client { get; set; }
		public bool SendEmail { get; set; }
	}
}
