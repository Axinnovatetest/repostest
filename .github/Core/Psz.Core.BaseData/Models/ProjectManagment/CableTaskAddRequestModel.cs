using System;

namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class CableTaskAddRequestModel
	{
		public int Id { get; set; }
		//public int CableId { get; set; }
		public string TaskName { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? Deadline { get; set; }
		public string Comment { get; set; }
		public int ProjectId { get; set; }
		public string Status { get; set; }
		public int? StatusId { get; set; }
		public CableTaskAddRequestModel()
		{

		}
		public Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CurrentTaskEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CurrentTaskEntity
			{
				//CableId = CableId,
				CurrentTaskName = TaskName,
				StartDate = StartDate,
				Deadline = Deadline,
				Status = Status,
				StatusId = StatusId,
				ProjectId = ProjectId,
				Comment = Comment,
				Id = Id,
			};
		}
	}
}
