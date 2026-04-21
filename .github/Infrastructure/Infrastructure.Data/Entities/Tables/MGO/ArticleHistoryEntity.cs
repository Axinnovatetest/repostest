using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Statistics.MGO
{
	public class ArticleHistoryEntity
	{
		public int Id { get; set; }
		public string LastUpdateUserFullName { get; set; }
		public string LogDescription { get; set; }
		public DateTime? LastUpdateTime { get; set; }

		public ArticleHistoryEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Id"]);
			LastUpdateUserFullName = (dataRow["LastUpdateUserFullName"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["LastUpdateUserFullName"]);
			LogDescription = (dataRow["LogDescription"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["LogDescription"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
		}
	}
}

