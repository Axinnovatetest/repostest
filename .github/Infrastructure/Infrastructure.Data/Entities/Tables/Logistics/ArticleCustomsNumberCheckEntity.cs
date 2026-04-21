using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.LGT
{
	public class ArticleCustomsNumberCheckEntity
	{
		public int? ArticlesTotalCount { get; set; }
		public int? ArticlesWithoutNumberCount { get; set; }
		public int? ArticlesWithWrongNumberCount { get; set; }
		public DateTime? CheckDate { get; set; }
		public int? CheckUser { get; set; }
		public string CheckUserName { get; set; }
		public int Id { get; set; }

		public ArticleCustomsNumberCheckEntity() { }

		public ArticleCustomsNumberCheckEntity(DataRow dataRow)
		{
			ArticlesTotalCount = (dataRow["ArticlesTotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticlesTotalCount"]);
			ArticlesWithoutNumberCount = (dataRow["ArticlesWithoutNumberCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticlesWithoutNumberCount"]);
			ArticlesWithWrongNumberCount = (dataRow["ArticlesWithWrongNumberCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticlesWithWrongNumberCount"]);
			CheckDate = (dataRow["CheckDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckDate"]);
			CheckUser = (dataRow["CheckUser"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CheckUser"]);
			CheckUserName = (dataRow["CheckUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CheckUserName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
		}

		public ArticleCustomsNumberCheckEntity ShallowClone()
		{
			return new ArticleCustomsNumberCheckEntity
			{
				ArticlesTotalCount = ArticlesTotalCount,
				ArticlesWithoutNumberCount = ArticlesWithoutNumberCount,
				ArticlesWithWrongNumberCount = ArticlesWithWrongNumberCount,
				CheckDate = CheckDate,
				CheckUser = CheckUser,
				CheckUserName = CheckUserName,
				Id = Id
			};
		}
	}
}

