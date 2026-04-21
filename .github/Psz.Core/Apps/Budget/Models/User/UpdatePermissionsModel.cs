using System.Collections.Generic;

namespace Psz.Core.Apps.Budget.Models.User
{
	public class UpdatePermissionsModel
	{
		public int Id { get; set; }
		public int AccessProfileId { get; set; }
		public string Email { get; set; }
		public List<int> Depts { get; set; } = new List<int>();
		public List<int> Lands { get; set; } = new List<int>();
		public List<int> UsersAssign { get; set; } = new List<int>();
	}
}
