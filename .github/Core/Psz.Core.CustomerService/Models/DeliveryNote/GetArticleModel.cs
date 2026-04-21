namespace Psz.Core.CustomerService.Models.DeliveryNote
{
	public class GetArticleModel
	{
		public string ArticleNumber { get; set; }
		public decimal Quantity { get; set; }
		public bool? VK_fixed { get; set; }
		public decimal? VK_price { get; set; }

		public GetArticleModel()
		{

		}
	}
}
