using System;

namespace Psz.Core.Apps.WorkPlan.Models.Country
{
	public class CountryModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Designation { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public Boolean IsArchived { get; set; }
	}
}

