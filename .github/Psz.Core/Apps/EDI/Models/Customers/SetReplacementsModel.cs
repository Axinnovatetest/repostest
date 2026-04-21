using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.Customers
{
	public class SetReplacementsModel
	{
		public int CustomerId { get; set; }
		public List<Replacement> Replacements { get; set; } = new List<Replacement>();

		public class Replacement
		{
			public int Id { get; set; }
			public DateTime ValidFromTime { get; set; }
			public DateTime ValidIntoTime { get; set; }
		}
	}
}
