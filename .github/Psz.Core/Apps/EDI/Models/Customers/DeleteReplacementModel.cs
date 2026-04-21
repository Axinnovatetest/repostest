using System;

namespace Psz.Core.Apps.EDI.Models.Customers
{
	public class DeleteReplacementModel
	{
		public int CustomerId { get; set; }
		public int ReplacementId { get; set; }
		public DateTime ValidFromTime { get; set; }
		public DateTime ValidIntoTime { get; set; }
	}
}
