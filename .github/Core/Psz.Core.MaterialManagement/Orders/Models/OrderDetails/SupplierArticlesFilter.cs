namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class SupplierArticlesFilterResponseModel
	{
		public int Id { get; set; }
		public string ArticleNumber { get; set; }
		public string Description { get; set; }
	}
	public class SupplierArticlesFilterRequestModel
	{
		public int SupplierId { get; set; }
		public string ArticleNumber { get; set; }
	}
}
