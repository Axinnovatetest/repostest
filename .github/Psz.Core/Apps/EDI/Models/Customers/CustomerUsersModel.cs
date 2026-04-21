using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.Customers
{
	public class CustomerUsersModel
	{
		public PrimaryUserModel PrimaryUser { get; set; }
		public List<ReplacementModel> Replacements { get; set; } = new List<ReplacementModel>();
	}
}
