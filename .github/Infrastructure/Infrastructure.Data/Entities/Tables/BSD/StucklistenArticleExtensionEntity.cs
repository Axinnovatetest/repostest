using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class StucklistenArticleExtensionEntity
	{
		public string ArticleDesignation { get; set; }
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string BomStatus { get; set; }
		public int? BomStatusId { get; set; }
		public DateTime? BomValidFrom { get; set; }
		public int? BomVersion { get; set; }
		public int Id { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		//---Update 22-04-2024 Ridha Ben Abdelaali
		public bool? KontakteStatus { get; set; }
		public StucklistenArticleExtensionEntity() { }

		public StucklistenArticleExtensionEntity(DataRow dataRow)
		{
			ArticleDesignation = (dataRow["ArticleDesignation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleDesignation"]);
			ArticleId = Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			BomStatus = (dataRow["BomStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BomStatus"]);
			BomStatusId = (dataRow["BomStatusId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BomStatusId"]);
			BomValidFrom = (dataRow["BomValidFrom"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["BomValidFrom"]);
			BomVersion = (dataRow["BomVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BomVersion"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
		}
	}
}

