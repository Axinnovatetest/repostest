using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class EmailNotificationUsersEntity
	{
		public bool? ArticleBomCpControl_Engineering { get; set; }
		public bool? ArticleBomCpControl_Quality { get; set; }
		public bool? ArticlePurchase { get; set; }
		public bool? ArticleSales { get; set; }
		public long Id { get; set; }
		public string UserEmail { get; set; }
		public long UserId { get; set; }
		public string UserName { get; set; }

		public EmailNotificationUsersEntity() { }

		public EmailNotificationUsersEntity(DataRow dataRow)
		{
			ArticleBomCpControl_Engineering = (dataRow["ArticleBomCpControl_Engineering"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticleBomCpControl_Engineering"]);
			ArticleBomCpControl_Quality = (dataRow["ArticleBomCpControl_Quality"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticleBomCpControl_Quality"]);
			ArticlePurchase = (dataRow["ArticlePurchase"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticlePurchase"]);
			ArticleSales = (dataRow["ArticleSales"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticleSales"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			UserEmail = (dataRow["UserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserEmail"]);
			UserId = Convert.ToInt64(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
		}
	}
}

