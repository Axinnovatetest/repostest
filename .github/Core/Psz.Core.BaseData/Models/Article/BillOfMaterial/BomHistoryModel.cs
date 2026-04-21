using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class BomHistoryResponseModel
	{
		public int ArticleId { get; set; }
		public int BomVersion { get; set; }
		public string IndexCustomer { get; set; }
		public DateTime? IndexCustomerDate { get; set; }
		public DateTime? ValidFrom { get; set; }
		public int PositionsCount { get; set; }
		public DateTime? ApprovalTime { get; set; }
		public int ApprovalUserId { get; set; }
		public string ApprovalUserName { get; set; }
		public int? BcrId { get; set; }
		public BomHistoryResponseModel(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtension_SnapshotEntity headerEntity,
			Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity positionEntity,
			Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity, int positionsCount)
		{
			ArticleId = (headerEntity?.ArticleId ?? positionEntity?.Artikel_Nr) ?? -1;
			BomVersion = (headerEntity?.BomVersion ?? positionEntity?.BomVersion) ?? -1;
			IndexCustomer = headerEntity?.KundenIndex ?? positionEntity?.KundenIndex;
			IndexCustomerDate = headerEntity?.KundenIndexDate ?? positionEntity?.KundenIndexDate;
			ValidFrom = headerEntity?.BomValidFrom;
			PositionsCount = positionsCount;
			ApprovalTime = headerEntity?.SnapshotTime ?? positionEntity?.SnapshotTime;
			ApprovalUserId = (headerEntity?.SnapshotUserId ?? positionEntity?.SnapshotUserId) ?? -1;
			ApprovalUserName = userEntity?.Name;
			BcrId = headerEntity?.BrcId;
		}
	}
}
