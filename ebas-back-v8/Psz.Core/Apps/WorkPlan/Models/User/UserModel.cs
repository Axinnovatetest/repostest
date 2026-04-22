using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.User
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public int AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }
		public List<KeyValuePair<int, string>> Halls { get; set; } = new List<KeyValuePair<int, string>>();
		public List<KeyValuePair<int, string>> Countries { get; set; } = new List<KeyValuePair<int, string>>();
	}
}
