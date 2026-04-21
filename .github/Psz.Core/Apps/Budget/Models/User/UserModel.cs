using System.Collections.Generic;

namespace Psz.Core.Apps.Budget.Models.User
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }
		public List<KeyValuePair<int, string>> Depts { get; set; } = new List<KeyValuePair<int, string>>();
		public List<KeyValuePair<int, string>> Lands { get; set; } = new List<KeyValuePair<int, string>>();
		public List<KeyValuePair<int, string>> UsersAssign { get; set; } = new List<KeyValuePair<int, string>>();

	}
}
