using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class StatsModel
	{
		public List<CountsModel> ProjectsByStatus { get; set; }
		public List<CountsModel> ProjectsByTasks { get; set; }
		public List<CountsModel> ProjectsByTime { get; set; }
	}
	public class CountsModel
	{
		public string Status { get; set; }
		public int Count { get; set; }
	}

	public class ProjectsOverviewModel
	{
		public string ProjectName { get; set; }
		public string Customer { get; set; }
		public string Status { get; set; }
		public string Manager { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public ProjectsOverviewModel(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity entity)
		{
			ProjectName = entity.ProjectName;
			Customer = entity.CustomerName;
			Status = entity.Status;
			Manager = entity.PMManagerUsername;
			DeliveryDate = entity.DeliveryDate;
		}
	}
	public class TasksOverviewModel
	{
		public string TaskName { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? DeadLine { get; set; }
		public string ProjectName { get; set; }
		public string Cable { get; set; }
		public TasksOverviewModel()
		{

		}
		public TasksOverviewModel(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CurrentTaskEntity entity)
		{
			TaskName = entity.CurrentTaskName;
			StartDate = entity.StartDate;
			DeadLine = entity.Deadline;
			ProjectName = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(entity.ProjectId ?? -1)?.ProjectName;
			//Cable = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.Get(entity.CableId ?? -1).ArticleNumber;
		}
	}
}