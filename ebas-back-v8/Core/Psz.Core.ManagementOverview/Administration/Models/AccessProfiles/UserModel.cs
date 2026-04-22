using System.Collections.Generic;

namespace Psz.Core.ManagementOverview.Administration.Models.AccessProfiles
{
	public class AddUsersRequestModel
	{
		public int ProfileId { get; set; }
		public List<int> UserIds { get; set; }
	}
	public class AddToUserRequestModel
	{
		public int UserId { get; set; }
		public List<KeyValuePair<int, string>> ProfileIds { get; set; }
	}
}
