namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class BomPositionAlt
	{
		public int ParentArticleId { get; set; }
		public int Id { get; set; }
		public string Position { get; set; }
		public decimal? Quantity { get; set; }
		public int? ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleDesignation { get; set; }
		public int OriginalPositionId { get; set; }
		public int? DocumentId { get; set; }
		public int? CRUDState { get; set; }// add,update or delete
		public BomPositionAlt()
		{

		}
		public BomPositionAlt(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity stucklistenPositionAltEntity)
		{
			if(stucklistenPositionAltEntity == null)
				return;

			Id = stucklistenPositionAltEntity.Nr;
			Position = stucklistenPositionAltEntity.Position;
			ParentArticleId = stucklistenPositionAltEntity.ParentArtikelNr;
			Quantity = stucklistenPositionAltEntity.Anzahl;
			ArticleId = stucklistenPositionAltEntity.ArtikelNr;
			ArticleNumber = stucklistenPositionAltEntity.ArtikelNummer;
			ArticleDesignation = stucklistenPositionAltEntity.ArtikelBezeichnung;
			DocumentId = stucklistenPositionAltEntity.DocumentId;
			OriginalPositionId = stucklistenPositionAltEntity.OriginalStucklistenNr;
		}
		public Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity ToEntity(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity stucklistenPositionAltEntity, bool? persistAttachments = true)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity
			{
				Nr = Id,
				Position = Position,
				ParentArtikelNr = ParentArticleId,
				Anzahl = Quantity,
				ArtikelNr = ArticleId,
				ArtikelNummer = ArticleNumber,
				ArtikelBezeichnung = ArticleDesignation,
				DocumentId = stucklistenPositionAltEntity == null || (persistAttachments == true && DocumentId.HasValue && stucklistenPositionAltEntity.DocumentId != DocumentId) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFile(DocumentId.Value, (int)Enums.ArticleEnums.ArticleFileType.BOMAltPosition) : DocumentId,
				OriginalStucklistenNr = OriginalPositionId
			};
		}
	}
	public class BomPositionAltEdit
	{
		public int ParentArticleId { get; set; }
		public int Id { get; set; }
		public string Position { get; set; }
		public decimal? Quantity { get; set; }
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleDesignation { get; set; }
		public int OriginalPositionId { get; set; }
		public int? DocumentId { get; set; }

		public byte[] DocumentData { get; set; }
		public string DocumentExtension { get; set; }
		public Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity
			{
				Nr = Id,
				Position = Position,
				ParentArtikelNr = ParentArticleId,
				Anzahl = Quantity,
				ArtikelNr = ArticleId,
				ArtikelNummer = ArticleNumber,
				ArtikelBezeichnung = ArticleDesignation,
				DocumentId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(DocumentId, DocumentData, DocumentExtension, null),
				OriginalStucklistenNr = OriginalPositionId
			};
		}
	}
}
