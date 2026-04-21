namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class SupplierArticleResponseModel
	{
		public int Id { get; set; }
		public string ArticleNumber { get; set; }
		public string Description { get; set; }
	}
	public class SupplierArticleRequestModel
	{
		public int SuppplierId { get; set; }
	}
}
