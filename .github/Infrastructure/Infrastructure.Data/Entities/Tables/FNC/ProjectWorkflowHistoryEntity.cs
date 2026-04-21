using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class ProjectWorkflowHistoryEntity
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

		public ProjectWorkflowHistoryEntity() { }

		public ProjectWorkflowHistoryEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			ProjectId = Convert.ToInt32(dataRow["ProjectId"]);
			ProjectName = (dataRow["ProjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectName"]);
			ProjectOwnerUserEmail = (dataRow["ProjectOwnerUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectOwnerUserEmail"]);
			ProjectOwnerUserId = (dataRow["ProjectOwnerUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectOwnerUserId"]);
			ProjectOwnerUserName = (dataRow["ProjectOwnerUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectOwnerUserName"]);
			WorkflowActionComments = (dataRow["WorkflowActionComments"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WorkflowActionComments"]);
			WorkflowActionId = (dataRow["WorkflowActionId"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["WorkflowActionId"]);
			WorkflowActionName = (dataRow["WorkflowActionName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WorkflowActionName"]);
			WorkflowActionTime = (dataRow["WorkflowActionTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["WorkflowActionTime"]);
			WorkflowActionUserEmail = (dataRow["WorkflowActionUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WorkflowActionUserEmail"]);
			WorkflowActionUserId = (dataRow["WorkflowActionUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WorkflowActionUserId"]);
			WorkflowActionUserName = (dataRow["WorkflowActionUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WorkflowActionUserName"]);
		}
	}
}

