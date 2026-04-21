using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.WorkSchedule
{
	public class CountryModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Hall> Halls { get; set; } = new List<Hall>();

		public class Hall
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}
