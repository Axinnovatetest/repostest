using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.BSD
{
	public class ProjectTasksEntity
	{
		public int ProjectId { get; set; }
		public int CableId { get; set; }
		public int? ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleCustomerNumber { get; set; }
		public string CurrentTaskName { get; set; }
		public int? CurrentTaskId { get; set; }
		public string ResponsibleUsername { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? Deadline { get; set; }
		public string PMManagerUsername { get; set; }
		public DateTime? CreationDate { get; set; }
		public string Status { get; set; }
		public ProjectTasksEntity(DataRow dataRow)
		{
			ProjectId = Convert.ToInt32(dataRow["ProjectId"]);
			CableId = Convert.ToInt32(dataRow["CableId"]);
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			ArticleCustomerNumber = (dataRow["ArticleCustomerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleCustomerNumber"]);
			//CurrentTaskName = (dataRow["CurrentTaskName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CurrentTaskName"]);
			//CurrentTaskId = (dataRow["TaskId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TaskId"]);
			ResponsibleUsername = (dataRow["ResponsibleUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ResponsibleUsername"]);
			//StartDate = (dataRow["StartDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]);
			//Deadline = (dataRow["Deadline"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Deadline"]);
			PMManagerUsername = (dataRow["PMManagerUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PMManagerUsername"]);
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
		}
	}
}