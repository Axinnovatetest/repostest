using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models
{
	public class GetCustomerNamesForArticleReferenceModel
	{
		public string CustomerName { get; set; }
		public int? CustomerNumber { get; set; }
		public int? CustomerId { get; set; }
	}
	public class GetCustomerNamesForArticleReferenceRequestModel
	{
		public string searchtext { get; set; } = "";
		public int ArtikelId { get; set; }
	}
}
