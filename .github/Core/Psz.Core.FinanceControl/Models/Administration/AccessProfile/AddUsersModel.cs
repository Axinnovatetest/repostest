using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Administration.AccessProfile
{
	public class AddUsersModel
	{
		public int ProfileId { get; set; }
		public List<int> UserIds { get; set; }
	}
}
