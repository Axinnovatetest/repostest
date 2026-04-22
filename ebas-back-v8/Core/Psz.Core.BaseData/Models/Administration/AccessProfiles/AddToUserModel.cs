using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Administration.AccessProfiles
{
	public class AddToUserModel
	{
		public int UserId { get; set; }
		public List<KeyValuePair<int, string>> ProfileIds { get; set; }
	}
}
