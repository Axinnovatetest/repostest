using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class __bsd_pm_CablesEntity
	{
		public string ArticleCustomerNumber { get; set; }
		public int? ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUsername { get; set; }
		public int Id { get; set; }
		public int? ProjectId { get; set; }
		public int? ResponsibleUserId { get; set; }
		public string ResponsibleUsername { get; set; }
		public string Status { get; set; }
		public int? StatusId { get; set; }

		public __bsd_pm_CablesEntity() { }

		public __bsd_pm_CablesEntity(DataRow dataRow)
		{
			ArticleCustomerNumber = (dataRow["ArticleCustomerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleCustomerNumber"]);
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreationUsername = (dataRow["CreationUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreationUsername"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProjectId = (dataRow["ProjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectId"]);
			ResponsibleUserId = (dataRow["ResponsibleUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ResponsibleUserId"]);
			ResponsibleUsername = (dataRow["ResponsibleUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ResponsibleUsername"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			StatusId = (dataRow["StatusId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StatusId"]);
		}

		public __bsd_pm_CablesEntity ShallowClone()
		{
			return new __bsd_pm_CablesEntity
			{
				ArticleCustomerNumber = ArticleCustomerNumber,
				ArticleId = ArticleId,
				ArticleNumber = ArticleNumber,
				CreationUserId = CreationUserId,
				CreationUsername = CreationUsername,
				Id = Id,
				ProjectId = ProjectId,
				ResponsibleUserId = ResponsibleUserId,
				ResponsibleUsername = ResponsibleUsername,
				Status = Status,
				StatusId = StatusId
			};
		}
	}
}

