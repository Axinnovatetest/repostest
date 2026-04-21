using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Models.Users
{
	public class CreateBudgetModel
	{
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int AccessProfileId { get; set; }
		public List<int> Depts { get; set; } = new List<int>();
		public List<int> Lands { get; set; } = new List<int>();
		public List<int> UsersAssign { get; set; } = new List<int>();
	}
}
