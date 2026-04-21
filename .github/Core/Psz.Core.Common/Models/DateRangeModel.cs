using System;

namespace Psz.Core.Common.Models
{
	public class IDateRangeModel
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}

	public class IDateRangeNullableModel
	{
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
	}
}
