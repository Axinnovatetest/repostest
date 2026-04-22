using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class BOMStatusDetailsModel
	{
		public int Id { get; set; }
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleDesignation { get; set; }
		public int? BomVersion { get; set; }
		public string BomStatus { get; set; }
		public DateTime? BomValidFrom { get; set; }
		public bool? CP_required { get; set; }
		public bool aktiv { get; set; }
		// - 2022-0713 - Partial Validation // - 2022-07-25 deprecated
		public bool UpgradeBomVersion { get; set; } = false;
		// - 2022-09-21 -
		public bool IsUBG { get; set; }
		public bool IsEDrawing { get; set; }
		//---Update 22-04-2024 Ridha Ben Abdelaali
		public bool? KontakteStatus { get; set; }

		//-- Add Freigabestatus 2024-09-11
		public string? Freigabestatus { get; set; }

		public BOMStatusDetailsModel()
		{

		}
		public BOMStatusDetailsModel(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity stucklistenBomEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			if(stucklistenBomEntity == null)
				return;
			Id = stucklistenBomEntity.Id;
			ArticleId = stucklistenBomEntity.ArticleId;
			ArticleNumber = stucklistenBomEntity.ArticleNumber;
			ArticleDesignation = stucklistenBomEntity.ArticleDesignation;
			BomVersion = stucklistenBomEntity.BomVersion;
			BomStatus = stucklistenBomEntity.BomStatus;
			BomValidFrom = stucklistenBomEntity.BomValidFrom;
			CP_required = artikelEntity?.CP_required ?? true;
			aktiv = artikelEntity?.aktiv ?? false;
			IsUBG = artikelEntity?.UBG ?? false;
			IsEDrawing = artikelEntity?.IsEDrawing ?? false;
			KontakteStatus = stucklistenBomEntity.KontakteStatus;
			Freigabestatus = artikelEntity.Freigabestatus;

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
				BomValidFrom = BomValidFrom
			};
		}
	}
}
