using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Models
{
	public class AddToUserModel
	{
		public int UserId { get; set; }
		public List<KeyValuePair<int, string>> ProfileIds { get; set; }
	}
}
