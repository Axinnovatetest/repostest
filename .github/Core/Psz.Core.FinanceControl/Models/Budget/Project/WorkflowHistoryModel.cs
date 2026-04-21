using System;

namespace Psz.Core.FinanceControl.Models.Budget.Project
{
	public class WorkflowHistoryModel
	{
		public int Id { get; set; }
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
		public string ProjectOwnerUserEmail { get; set; }
		public int? ProjectOwnerUserId { get; set; }
		public string ProjectOwnerUserName { get; set; }
		public string WorkflowActionComments { get; set; }
		public long? WorkflowActionId { get; set; }
		public string WorkflowActionName { get; set; }
		public DateTime? WorkflowActionTime { get; set; }
		public string WorkflowActionUserEmail { get; set; }
		public int? WorkflowActionUserId { get; set; }
		public string WorkflowActionUserName { get; set; }

		public WorkflowHistoryModel(Infrastructure.Data.Entities.Tables.FNC.ProjectWorkflowHistoryEntity projectWorkflow)
		{
			if(projectWorkflow == null)
				return;

			Id = projectWorkflow.Id;
			ProjectId = projectWorkflow.ProjectId;
			ProjectName = projectWorkflow.ProjectName;
			ProjectOwnerUserEmail = projectWorkflow.ProjectOwnerUserEmail;
			ProjectOwnerUserId = projectWorkflow.ProjectOwnerUserId;
			ProjectOwnerUserName = projectWorkflow.ProjectOwnerUserName;
			WorkflowActionComments = projectWorkflow.WorkflowActionComments;
			WorkflowActionId = projectWorkflow.WorkflowActionId;
			WorkflowActionName = projectWorkflow.WorkflowActionName;
			WorkflowActionTime = projectWorkflow.WorkflowActionTime;
			WorkflowActionUserEmail = projectWorkflow.WorkflowActionUserEmail;
			WorkflowActionUserId = projectWorkflow.WorkflowActionUserId;
			WorkflowActionUserName = projectWorkflow.WorkflowActionUserName;
		}
	}
}
