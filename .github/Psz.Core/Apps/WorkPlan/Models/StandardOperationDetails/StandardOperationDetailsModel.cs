using System;

namespace Psz.Core.Apps.WorkPlan.Models.StandardOperationDescription
{
	public class StandardOperationDescriptionModel
	{
		public int Id { get; set; }
		public int StandardOperationId { get; set; }
		public string Description { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public Boolean IsArchived { get; set; }

		public string NameDE { get; set; }
		public string NameTN { get; set; }
		public string NameCZ { get; set; }
		public string NameAL { get; set; }
	}
}
