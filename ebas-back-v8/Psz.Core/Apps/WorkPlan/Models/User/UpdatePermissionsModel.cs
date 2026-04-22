using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.User
{
	public class UpdatePermissionsModel
	{
		public int Id { get; set; }
		public int AccessProfileId { get; set; }
		public List<int> Halls { get; set; } = new List<int>();
		public List<int> Countries { get; set; } = new List<int>();
	}
}
