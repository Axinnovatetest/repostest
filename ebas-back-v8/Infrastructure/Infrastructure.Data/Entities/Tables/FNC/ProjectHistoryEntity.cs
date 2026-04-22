using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class ProjectHistoryEntity
	{
		public DateTime? ApprovalTime { get; set; }
		public string ApprovalUserEmail { get; set; }
		public int? ApprovalUserId { get; set; }
		public string ApprovalUserName { get; set; }
		public bool? Archived { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public int BudgetYear { get; set; }
		public bool? Closed { get; set; }
		public DateTime? ClosedTime { get; set; }
		public string ClosedUserEmail { get; set; }
		public int? ClosedUserId { get; set; }
		public string ClosedUserName { get; set; }
		public int? CompanyId { get; set; }
		public string CompanyName { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public int? CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerNr { get; set; }
		public bool? Deleted { get; set; }
		public DateTime? DeleteTime { get; set; }
		public int? DeleteUserId { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public string Description { get; set; }
		public DateTime HistoryTime { get; set; }
		public string HistoryUserEmail { get; set; }
		public int HistoryUserId { get; set; }
		public string HistoryUserName { get; set; }
		public int Id { get; set; }
		public int Id_State { get; set; }
		public int Id_Type { get; set; }
		public int? OrderCount { get; set; }
		public decimal ProjectBudget { get; set; }
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
		public int? ProjectStatus { get; set; }
		public DateTime? ProjectStatusChangeTime { get; set; }
		public int? ProjectStatusChangeUserId { get; set; }
		public string ProjectStatusChangeUserName { get; set; }
		public string ProjectStatusName { get; set; }
		public decimal? PSZOffer { get; set; }
		public string ResponsableEmail { get; set; }
		public int ResponsableId { get; set; }
		public string ResponsableName { get; set; }
		public decimal? TotalSpent { get; set; }
		public string Type { get; set; }

		public ProjectHistoryEntity() { }

		public ProjectHistoryEntity(DataRow dataRow)
		{
			ApprovalTime = (dataRow["ApprovalTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ApprovalTime"]);
			ApprovalUserEmail = (dataRow["ApprovalUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ApprovalUserEmail"]);
			ApprovalUserId = (dataRow["ApprovalUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ApprovalUserId"]);
			ApprovalUserName = (dataRow["ApprovalUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ApprovalUserName"]);
			Archived = (dataRow["Archived"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Archived"]);
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			BudgetYear = Convert.ToInt32(dataRow["BudgetYear"]);
			Closed = (dataRow["Closed"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Closed"]);
			ClosedTime = (dataRow["ClosedTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ClosedTime"]);
			ClosedUserEmail = (dataRow["ClosedUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ClosedUserEmail"]);
			ClosedUserId = (dataRow["ClosedUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ClosedUserId"]);
			ClosedUserName = (dataRow["ClosedUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ClosedUserName"]);
			CompanyId = (dataRow["CompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CompanyId"]);
			CompanyName = (dataRow["CompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CompanyName"]);
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			CurrencyId = (dataRow["CurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CurrencyId"]);
			CurrencyName = (dataRow["CurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CurrencyName"]);
			CustomerId = (dataRow["CustomerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerId"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			CustomerNr = (dataRow["CustomerNr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerNr"]);
			Deleted = (dataRow["Deleted"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Deleted"]);
			DeleteTime = (dataRow["DeleteTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeleteTime"]);
			DeleteUserId = (dataRow["DeleteUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DeleteUserId"]);
			DepartmentId = (dataRow["DepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DepartmentId"]);
			DepartmentName = (dataRow["DepartmentName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DepartmentName"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			HistoryTime = Convert.ToDateTime(dataRow["HistoryTime"]);
			HistoryUserEmail = (dataRow["HistoryUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HistoryUserEmail"]);
			HistoryUserId = Convert.ToInt32(dataRow["HistoryUserId"]);
			HistoryUserName = (dataRow["HistoryUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HistoryUserName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Id_State = Convert.ToInt32(dataRow["Id_State"]);
			Id_Type = Convert.ToInt32(dataRow["Id_Type"]);
			OrderCount = (dataRow["OrderCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderCount"]);
			ProjectBudget = Convert.ToDecimal(dataRow["ProjectBudget"]);
			ProjectId = Convert.ToInt32(dataRow["ProjectId"]);
			ProjectName = Convert.ToString(dataRow["ProjectName"]);
			ProjectStatus = (dataRow["ProjectStatus"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectStatus"]);
			ProjectStatusChangeTime = (dataRow["ProjectStatusChangeTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ProjectStatusChangeTime"]);
			ProjectStatusChangeUserId = (dataRow["ProjectStatusChangeUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectStatusChangeUserId"]);
			ProjectStatusChangeUserName = (dataRow["ProjectStatusChangeUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectStatusChangeUserName"]);
			ProjectStatusName = (dataRow["ProjectStatusName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectStatusName"]);
			PSZOffer = (dataRow["PSZOffer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PSZOffer"]);
			ResponsableEmail = (dataRow["ResponsableEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ResponsableEmail"]);
			ResponsableId = Convert.ToInt32(dataRow["ResponsableId"]);
			ResponsableName = (dataRow["ResponsableName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ResponsableName"]);
			TotalSpent = (dataRow["TotalSpent"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalSpent"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
		}
	}
}

