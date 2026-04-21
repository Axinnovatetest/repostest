
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.BaseData.Models.Article.ArticleReference
{
	public class AddArtikelCustomerReferencesRequestModel
	{
		[Required(ErrorMessage = "Artikel ID can not be empty")]
		public int? ArticleId { get; set; }
		[Required(ErrorMessage = "Customer ID can not be empty")]
		public int? CustomerId { get; set; }
		[Required(ErrorMessage = "Artikel ID can not be empty")]
		public string CustomerName { get; set; }
		[Required(ErrorMessage = "Customer Name can not be empty")]
		public int? CustomerNumber { get; set; }
		public string CustomerReference { get; set; }
	}

}
