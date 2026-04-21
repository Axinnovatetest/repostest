using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.ArticleReference
{
	public class EditArtikelCustomerReferencesRequestModel
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

		[Required(ErrorMessage = " ID can not be empty")]
		public int Id { get; set; }
	}
}
