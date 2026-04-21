using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.ROH.OfferRequests
{
	public  class AddAttachementToEmailRequestModel
	{
		public IFormFile file { get; set; }
		public string Ids { get; set; }
	}
	public class RemoveAttachementFromEmailRequestModel
	{
		public IFormFile file { get; set; }
		public string Ids { get; set; }
	}
	public class RemoveSingleAttachementFromEmailRequestModel
	{
		public List<int> Ids { get; set; }
		public int FiledId { get; set; }
	}
}


