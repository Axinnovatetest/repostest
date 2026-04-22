using System;

namespace Psz.Core.Apps.WorkPlan.Models.WorkPlan
{
	public class WorkPlanModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int ArticleId { get; set; }
		public int HallId { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public WorkPlanModel() { }
	}


}
