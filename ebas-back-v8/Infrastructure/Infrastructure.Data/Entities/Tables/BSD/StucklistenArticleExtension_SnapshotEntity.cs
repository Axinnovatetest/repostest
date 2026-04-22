using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class StucklistenArticleExtension_SnapshotEntity
	{
		public string ArticleDesignation { get; set; }
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public DateTime? BomValidFrom { get; set; }
		public int? BomVersion { get; set; }
		public int? BrcId { get; set; }
		public int Id { get; set; }
		public string KundenIndex { get; set; }
		public DateTime? KundenIndexDate { get; set; }
		public bool? Overwritten { get; set; }
		public DateTime? OverwrittenTime { get; set; }
		public int? OverwrittenUserId { get; set; }
		public DateTime SnapshotTime { get; set; }
		public int SnapshotUserId { get; set; }

		public StucklistenArticleExtension_SnapshotEntity() { }

		public StucklistenArticleExtension_SnapshotEntity(DataRow dataRow)
		{
			ArticleDesignation = (dataRow["ArticleDesignation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleDesignation"]);
			ArticleId = Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			BomValidFrom = (dataRow["BomValidFrom"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["BomValidFrom"]);
			BomVersion = (dataRow["BomVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BomVersion"]);
			BrcId = (dataRow["BrcId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BrcId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			KundenIndex = (dataRow["KundenIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["KundenIndex"]);
			KundenIndexDate = (dataRow["KundenIndexDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["KundenIndexDate"]);
			Overwritten = (dataRow["Overwritten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Overwritten"]);
			OverwrittenTime = (dataRow["OverwrittenTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OverwrittenTime"]);
			OverwrittenUserId = (dataRow["OverwrittenUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OverwrittenUserId"]);
			SnapshotTime = Convert.ToDateTime(dataRow["SnapshotTime"]);
			SnapshotUserId = Convert.ToInt32(dataRow["SnapshotUserId"]);
		}

		public StucklistenArticleExtension_SnapshotEntity ShallowClone()
		{
			return new StucklistenArticleExtension_SnapshotEntity
			{
				ArticleDesignation = ArticleDesignation,
				ArticleId = ArticleId,
				ArticleNumber = ArticleNumber,
				BomValidFrom = BomValidFrom,
				BomVersion = BomVersion,
				BrcId = BrcId,
				Id = Id,
				KundenIndex = KundenIndex,
				KundenIndexDate = KundenIndexDate,
				Overwritten = Overwritten,
				OverwrittenTime = OverwrittenTime,
				OverwrittenUserId = OverwrittenUserId,
				SnapshotTime = SnapshotTime,
				SnapshotUserId = SnapshotUserId
			};
		}
	}
}

