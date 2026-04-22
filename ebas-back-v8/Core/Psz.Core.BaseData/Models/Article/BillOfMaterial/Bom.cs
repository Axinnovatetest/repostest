using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class Bom
	{
		public int Id { get; set; }
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleDesignation { get; set; }
		public int? BomVersion { get; set; }
		public int? BomStatusId { get; set; }
		public string BomStatus { get; set; }
		public DateTime? BomValidFrom { get; set; }
		public List<BomPosition> BomPositions { get; set; }

		public Bom() { }

		public Bom(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity stucklistenBomEntity,
			List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> stucklistenEntities,
			List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity> stucklistenErsatzEntities,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			if(stucklistenBomEntity == null && stucklistenEntities == null)
				return;

			if(stucklistenBomEntity != null)
			{
				Id = stucklistenBomEntity.Id;
				ArticleId = stucklistenBomEntity.ArticleId;
				ArticleNumber = stucklistenBomEntity.ArticleNumber;
				ArticleDesignation = stucklistenBomEntity.ArticleDesignation;
				BomVersion = stucklistenBomEntity.BomVersion;
				BomStatus = stucklistenBomEntity.BomStatus;
				BomStatusId = stucklistenBomEntity.BomStatusId;
				BomValidFrom = stucklistenBomEntity.BomValidFrom;
			}

			BomPositions = stucklistenEntities?.
					Select(x => new BomPosition(x, stucklistenErsatzEntities?.
						FindAll(y => y.OriginalStucklistenNr == x.Nr)?.ToList(), artikelEntity))?.ToList();
		}

		public Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
			{
				Id = Id,
				ArticleId = ArticleId,
				ArticleNumber = ArticleNumber,
				ArticleDesignation = ArticleDesignation,
				BomVersion = BomVersion,
				BomStatus = BomStatus,
				BomStatusId = BomStatusId,
				BomValidFrom = BomValidFrom
			};
		}

	}
}
