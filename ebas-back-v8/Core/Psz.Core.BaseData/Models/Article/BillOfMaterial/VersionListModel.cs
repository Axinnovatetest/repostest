using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class VersionListModel
	{
		public int BomVersion { get; set; }
		public DateTime ValidationDate { get; set; }
		public int ValidationUserId { get; set; }
		public string ValidationUserName { get; set; }
		public string CustomerIndex { get; set; }
		public DateTime? CustomerIndexDate { get; set; }
		public VersionListModel(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity snapshotEntity)
		{
			if(snapshotEntity == null)
				return;

			// -
			BomVersion = snapshotEntity.BomVersion ?? 0;
			ValidationDate = snapshotEntity.SnapshotTime;
			ValidationUserId = snapshotEntity.SnapshotUserId;
			ValidationUserName = "";
			CustomerIndex = snapshotEntity.KundenIndex;
			CustomerIndexDate = snapshotEntity.KundenIndexDate;
		}
	}
}
