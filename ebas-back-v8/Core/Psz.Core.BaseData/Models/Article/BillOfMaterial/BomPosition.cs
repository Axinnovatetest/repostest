using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class BomPosition
	{
		public int Id { get; set; } // Nr
		public decimal? Quantity { get; set; } // Anzahl
		public int ArticleParentId { get; set; } // Artikel_Nr
		public int ArticleId { get; set; } // Artikel_Nr_des_Bauteils
		public string ArticleNumber { get; set; } // Artikelnummer
		public string ArticleDesignation { get; set; } // Bezeichnung_des_Bauteils
		public string Position { get; set; }
		public string Variante { get; set; }
		public int? ProcessId { get; set; } // Vorgang_Nr
		public int? DocumentId { get; set; }
		public int? CRUDState { get; set; }// add,update or delete
		public bool IsUBG { get; set; } // 2022-09-21


		public List<BomPositionAlt> BomPositionsAlt { get; set; }


		public BomPosition() { }

		public BomPosition(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity stucklistenPositionEntity,
			List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAltEntity> stucklistenPositionAltEntities,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			if(stucklistenPositionEntity == null)
				return;

			Id = stucklistenPositionEntity.Nr;
			Quantity = Convert.ToDecimal(stucklistenPositionEntity.Anzahl ?? 0);
			ArticleParentId = stucklistenPositionEntity.Artikel_Nr.HasValue ? stucklistenPositionEntity.Artikel_Nr.Value : -1;
			ArticleId = stucklistenPositionEntity.Artikel_Nr_des_Bauteils.HasValue ? stucklistenPositionEntity.Artikel_Nr_des_Bauteils.Value : -1;
			ArticleNumber = stucklistenPositionEntity.Artikelnummer;
			ArticleDesignation = stucklistenPositionEntity.Bezeichnung_des_Bauteils;
			Position = stucklistenPositionEntity.Position;
			Variante = stucklistenPositionEntity.Variante;
			ProcessId = stucklistenPositionEntity.Vorgang_Nr;
			DocumentId = stucklistenPositionEntity.DocumentId;

			BomPositionsAlt = stucklistenPositionAltEntities?
					.Select(x => new BomPositionAlt(x))?
					.ToList();

			// - 2022-09-21
			IsUBG = artikelEntity?.UBG ?? false;

		}
		public Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity ToEntity(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity stucklistenPositionEntity, bool? persistAttachments = true)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity
			{
				Nr = Id,
				Anzahl = Convert.ToSingle(Quantity ?? 0),
				Artikel_Nr = ArticleParentId,
				Artikel_Nr_des_Bauteils = ArticleId,
				Artikelnummer = ArticleNumber,
				Bezeichnung_des_Bauteils = ArticleDesignation,
				Position = Position,
				Variante = int.TryParse(Variante, out var v) ? $"{v}" : "0",
				Vorgang_Nr = ProcessId,
				DocumentId = (stucklistenPositionEntity == null || (persistAttachments == true && DocumentId.HasValue && stucklistenPositionEntity.DocumentId != DocumentId)) ? Psz.Core.Common.Helpers.ImageFileHelper.PersistTempFile(DocumentId.HasValue ? DocumentId.Value : -1, (int)Enums.ArticleEnums.ArticleFileType.BOMPosition) : DocumentId,
			};
		}
	}

	public class BomPositionEdit
	{
		public int Id { get; set; } // Nr
		public decimal? Quantity { get; set; } // Anzahl
		public int ArticleParentId { get; set; } // Artikel_Nr
		public int ArticleId { get; set; } // Artikel_Nr_des_Bauteils
		public string ArticleNumber { get; set; } // Artikelnummer
		public string ArticleDesignation { get; set; } // Bezeichnung_des_Bauteils
		public string Position { get; set; }
		public string Variante { get; set; }
		public int? ProcessId { get; set; } // Vorgang_Nr
		public int? DocumentId { get; set; }

		public byte[] DocumentData { get; set; }
		public string DocumentExtension { get; set; }

		public Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity
			{
				Nr = Id,
				Anzahl = Convert.ToSingle(Quantity ?? 0),
				Artikel_Nr = ArticleParentId,
				Artikel_Nr_des_Bauteils = ArticleId,
				Artikelnummer = ArticleNumber,
				Bezeichnung_des_Bauteils = ArticleDesignation,
				Position = Position,
				Variante = int.TryParse(Variante, out var v) ? $"{v}" : "0",
				Vorgang_Nr = ProcessId,
				DocumentId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(DocumentId, DocumentData, DocumentExtension, null)
			};
		}
	}
	public class ExportAsXLSModel
	{
		public int ArticleId { get; set; }
		public int? BomVerion { get; set; } = null;
		public bool PositionsOnly { get; set; } = true;
	}
}
