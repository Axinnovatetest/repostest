namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class DeleteArticleRequestModel
	{
		public int OrderItemId { get; set; }
	}
	public class DeleteArticleResponseModel
	{
		public bool Success { get; set; }

		public DeleteArticleResponseModel() { }

		public DeleteArticleResponseModel(bool success)
		{
			Success = success;
		}


	}
}
