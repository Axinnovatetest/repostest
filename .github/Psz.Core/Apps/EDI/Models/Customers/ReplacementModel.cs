using System;

namespace Psz.Core.Apps.EDI.Models.Customers
{
	public class ReplacementModel: PrimaryUserModel
	{
		public DateTime? ValidFromTime { get; set; }
		public DateTime? ValidIntoTime { get; set; }
	}
}
