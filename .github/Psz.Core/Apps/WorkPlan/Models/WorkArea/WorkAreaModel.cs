using System;

namespace Psz.Core.Apps.WorkPlan.Models.WorkArea
{
	public class WorkAreaModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int HallId { get; set; }
		public int CountryId { get; set; }
		public Boolean IsArchived { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
	}
}
