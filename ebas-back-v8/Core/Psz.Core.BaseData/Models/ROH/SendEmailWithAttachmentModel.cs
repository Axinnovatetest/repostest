using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.ROH
{
	public class SendEmailWithAttachmentModel
	{
		IFormFile OneEmailData { get; set; } 
		public List<int> Ids { get; set; }
		public int Index { get; set; }
	}
}
