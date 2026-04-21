using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.ROH.OfferRequests
{
	public  class RemoveOfferRequestEmailAttachmentModel
	{
		public int FiledId { get; set; }
		public List<int> Ids { get; set; }
	}
}
