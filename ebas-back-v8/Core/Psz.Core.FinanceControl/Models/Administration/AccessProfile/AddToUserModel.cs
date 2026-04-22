using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Administration.AccessProfile
{
	public class AddToUserModel
	{
		public int UserId { get; set; }
		public List<KeyValuePair<int, string>> ProfileIds { get; set; }
	}
}
