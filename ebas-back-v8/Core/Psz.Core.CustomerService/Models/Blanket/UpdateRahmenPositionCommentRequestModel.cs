using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class UpdateRahmenPositionCommentRequestModel
	{
		public int PositionExtensionId { get; set; }
		public string Comment { get; set; }
		public string ABNummer { get; set; }
	}
}
