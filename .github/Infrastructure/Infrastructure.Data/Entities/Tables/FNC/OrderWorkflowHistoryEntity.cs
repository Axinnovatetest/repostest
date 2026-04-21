using System;
using System.ComponentModel;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class OrderWorkflowHistoryEnum
	{
		public enum WorkflowActions: int
		{
			[Description("Creation")]
			Creation = 0,
			[Description("Last Validation Request")]
			LastValidationRequest = 1,
			[Description("Last Validation")]
			LastValidation = 2,
			[Description("Placement")]
			Placement = 3,
			[Description("Start Booking")]
			StartBooking = 4,
			[Description("End Booking")]
			EndBooking = 5,
			[Description("Closing")]
			Closing = 6
		}
	}
	public class OrderWorkflowHistoryEntity
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public string OrderIssuerUserEmail { get; set; }
		public int? OrderIssuerUserId { get; set; }
		public string OrderIssuerUserName { get; set; }
		public string OrderNumber { get; set; }
		public string WorkflowActionComments { get; set; }
		public long? WorkflowActionId { get; set; }
		public string WorkflowActionName { get; set; }
		public DateTime? WorkflowActionTime { get; set; }
		public string WorkflowActionUserEmail { get; set; }
		public int? WorkflowActionUserId { get; set; }
		public string WorkflowActionUserName { get; set; }

		public OrderWorkflowHistoryEntity() { }

		public OrderWorkflowHistoryEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderIssuerUserEmail = (dataRow["OrderIssuerUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderIssuerUserEmail"]);
			OrderIssuerUserId = (dataRow["OrderIssuerUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderIssuerUserId"]);
			OrderIssuerUserName = (dataRow["OrderIssuerUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderIssuerUserName"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
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

