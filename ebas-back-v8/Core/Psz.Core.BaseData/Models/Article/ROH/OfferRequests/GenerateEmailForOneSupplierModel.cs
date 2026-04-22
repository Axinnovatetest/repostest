using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.ROH.OfferRequests
{
	public  class GenerateEmailForOneSupplierModel
	{
		public int Id { get; set; }
		public List<int> OfferIds { get; set; }
	}
}
