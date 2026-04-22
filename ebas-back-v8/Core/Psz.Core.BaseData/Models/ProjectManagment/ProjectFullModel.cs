using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class ProjectFullModel
	{
		public ProjectHeader Header { get; set; }
		public List<ProjectCableModel> Cables { get; set; }
	}

	public class ProjectGANTTModel
	{
		public ProjectHeader Header { get; set; }
		public int MileStoneId { get; set; }
		public string MileStoneName { get; set; }
		public DateTime? MileStoneStartDate { get; set; }
		public DateTime? MileStoneEndDate { get; set; }
		public List<ProjectCableModel> Cables { get; set; }
		public List<ProjectCableTasksMinimalModel> Tasks { get; set; }
	}
}
