using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Models
{
	public class AddUsersModel
	{
		public int ProfileId { get; set; }
		public List<int> UserIds { get; set; }
	}
}
