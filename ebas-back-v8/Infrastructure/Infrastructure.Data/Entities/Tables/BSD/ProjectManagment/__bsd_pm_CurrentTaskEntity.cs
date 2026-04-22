using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class __bsd_pm_CurrentTaskEntity
	{
		public string Comment { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUsername { get; set; }
		public string CurrentTaskName { get; set; }
		public DateTime? Deadline { get; set; }
		public int Id { get; set; }
		public int? ProjectId { get; set; }
		public DateTime? StartDate { get; set; }
		public string Status { get; set; }
		public int? StatusId { get; set; }

		public __bsd_pm_CurrentTaskEntity() { }

		public __bsd_pm_CurrentTaskEntity(DataRow dataRow)
		{
			Comment = (dataRow["Comment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment"]);
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreationUsername = (dataRow["CreationUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreationUsername"]);
			CurrentTaskName = (dataRow["CurrentTaskName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CurrentTaskName"]);
			Deadline = (dataRow["Deadline"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Deadline"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProjectId = (dataRow["ProjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectId"]);
			StartDate = (dataRow["StartDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			StatusId = (dataRow["StatusId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StatusId"]);
		}

		public __bsd_pm_CurrentTaskEntity ShallowClone()
		{
			return new __bsd_pm_CurrentTaskEntity
			{
				Comment = Comment,
				CreationDate = CreationDate,
				CreationUserId = CreationUserId,
				CreationUsername = CreationUsername,
				CurrentTaskName = CurrentTaskName,
				Deadline = Deadline,
				Id = Id,
				ProjectId = ProjectId,
				StartDate = StartDate,
				Status = Status,
				StatusId = StatusId
			};
		}
	}
}

