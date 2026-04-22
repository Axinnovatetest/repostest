using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Administration.AccessProfiles
{
	public class AddLagerModel
	{
		public int ProfileId { get; set; }
		public List<int> LagerIds { get; set; }
	}
}
