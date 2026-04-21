using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Administration.AccessProfiles
{
	public class AddUsersModel
	{
		public int ProfileId { get; set; }
		public List<int> UserIds { get; set; }
	}
}
