using System;

namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class MileStoneModel
	{
		public int Id { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Comment { get; set; }
		public MileStoneModel()
		{

		}
		public MileStoneModel(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_MileStonesEntity entity)
		{
			Id = entity.Id;
			StartDate = entity.StartDate;
			EndDate = entity.EndDate;
			Comment = entity.Comment;
		}

		public Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_MileStonesEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_MileStonesEntity
			{
				Id = Id,
				StartDate = StartDate,
				EndDate = EndDate,
				Comment = Comment
			};
		}
	}
	public class MileStoneTasks
	{
		public int Id { get; set; }
		public int TaskId { get; set; }
		public string TaskName { get; set; }
	}
}