using System;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class WorkflowFullModel
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public string OrderIssuerUserEmail { get; set; }
		public int? OrderIssuerUserId { get; set; }
		public string OrderIssuerUserName { get; set; }
		public string OrderNumber { get; set; }
		public long? WorkflowActionId { get; set; }
		public string WorkflowActionName { get; set; }
		public DateTime? WorkflowActionTime { get; set; }
		public string WorkflowActionUserEmail { get; set; }
		public int? WorkflowActionUserId { get; set; }
		public string WorkflowActionUserName { get; set; }
		public string WorkflowActionComments { get; set; }

		public WorkflowFullModel()
		{

		}
		public WorkflowFullModel(Infrastructure.Data.Entities.Tables.FNC.OrderWorkflowHistoryEntity orderWorkflowHistory)
		{
			if(orderWorkflowHistory == null)
				return;

			Id = orderWorkflowHistory.Id;
			OrderId = orderWorkflowHistory.OrderId;
			OrderIssuerUserEmail = orderWorkflowHistory.OrderIssuerUserEmail;
			OrderIssuerUserId = orderWorkflowHistory.OrderIssuerUserId;
			OrderIssuerUserName = orderWorkflowHistory.OrderIssuerUserName;
			OrderNumber = orderWorkflowHistory.OrderNumber;
			WorkflowActionId = orderWorkflowHistory.WorkflowActionId;
			WorkflowActionName = orderWorkflowHistory.WorkflowActionName;
			WorkflowActionTime = orderWorkflowHistory.WorkflowActionTime;
			WorkflowActionUserEmail = orderWorkflowHistory.WorkflowActionUserEmail;
			WorkflowActionUserId = orderWorkflowHistory.WorkflowActionUserId;
			WorkflowActionUserName = orderWorkflowHistory.WorkflowActionUserName;
			WorkflowActionComments = orderWorkflowHistory.WorkflowActionComments;
		}
	}
}
