using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class ProjectCableTasksModel
	{
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
		public int CableId { get; set; }
		public int? ArticleId { get; set; }
		public string ArtikelNumber { get; set; }
		public string ArticleCustomerNumber { get; set; }
		public string CurrentTaskName { get; set; }
		public int? CurrentTaskId { get; set; }
		public string Responsible { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? Deadline { get; set; }
		public string Manager { get; set; }
		public bool OnTime { get; set; }
		public int Delay { get; set; }
		public DateTime? CreationDate { get; set; }
		public string Status { get; set; }
		public int ProductionStatus { get; set; }
		public ProjectCableTasksModel()
		{

		}
		public ProjectCableTasksModel(Infrastructure.Data.Entities.Joins.BSD.ProjectTasksEntity entity, int prodStatus)
		{
			var project = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(entity.ProjectId);
			ProjectId = entity.ProjectId;
			ProjectName = project?.ProjectName;
			CableId = entity.CableId;
			ArticleId = entity.ArticleId;
			ArtikelNumber = entity.ArticleNumber;
			ArticleCustomerNumber = entity.ArticleCustomerNumber;
			CurrentTaskName = null;
			CurrentTaskId = null;
			Responsible = entity.ResponsibleUsername;
			StartDate = null;
			Deadline = null;
			Manager = entity.PMManagerUsername;
			CreationDate = entity.CreationDate;
			Status = entity.Status;
			ProductionStatus = prodStatus;
		}
	}

	public class ProjectCableModel
	{
		public int Id { get; set; }
		public int? ArticleId { get; set; }
		public string Artikelnummer { get; set; }
		public string CustomerItemNumber { get; set; }
		public int ResponsibleId { get; set; }
		public string ResponsibleUsername { get; set; }
		public string Status { get; set; }
		public int ProductionStatus { get; set; }
		public List<ProjectCableTasksMinimalModel> Tasks { get; set; }
		public ProjectCableModel()
		{

		}
		public ProjectCableModel(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity entity, bool getTasks = false)
		{
			Id = entity.Id;
			ArticleId = entity.ArticleId;
			Artikelnummer = entity.ArticleNumber;
			CustomerItemNumber = entity.ArticleCustomerNumber;
			ResponsibleId = entity.ResponsibleUserId ?? -1;
			ResponsibleUsername = entity.ResponsibleUsername;
			Status = entity.Status;
			if(getTasks)
			{
				var tasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.GetByCable(entity.Id);
				Tasks = tasks?.Select(x => new ProjectCableTasksMinimalModel(x)).ToList();
			}
		}
		public ProjectCableModel(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity entity, int prodStatus, bool getTasks = false)
		{
			Id = entity.Id;
			ArticleId = entity.ArticleId;
			Artikelnummer = entity.ArticleNumber;
			CustomerItemNumber = entity.ArticleCustomerNumber;
			ResponsibleId = entity.ResponsibleUserId ?? -1;
			ResponsibleUsername = entity.ResponsibleUsername;
			Status = entity.Status;
			ProductionStatus = prodStatus;
			if(getTasks)
			{
				var tasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.GetByCable(entity.Id);
				Tasks = tasks?.Select(x => new ProjectCableTasksMinimalModel(x)).ToList();
			}
		}
	}

	public class ProjectCableTasksMinimalModel
	{
		public int Id { get; set; }
		public string TaskName { get; set; }
		public int? CableId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? Deadline { get; set; }
		public string Comment { get; set; }
		public int? ProjectId { get; set; }
		public string Status { get; set; }
		public int? StatusId { get; set; }
		public ProjectCableTasksMinimalModel()
		{

		}
		public ProjectCableTasksMinimalModel(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CurrentTaskEntity entity)
		{
			Id = entity.Id;
			TaskName = entity.CurrentTaskName;
			//CableId = entity.CableId;
			StartDate = entity.StartDate;
			Deadline = entity.Deadline;
			Comment = entity.Comment;
			ProjectId = entity.ProjectId;
			Status = entity.Status;
			StatusId = entity.StatusId;
		}
	}
}
