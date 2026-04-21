using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class UBGParentModel
	{
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string Designation1 { get; set; }
		public string Designation2 { get; set; }
		public string CustomerIndex { get; set; }
		public DateTime? CustomerIndexDate { get; set; }
		public int? BomVersion { get; set; }
		public int? BomStatusId { get; set; }
		public string BomStatus { get; set; }
		public DateTime? BomValidationDate { get; set; }
		public UBGParentModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity articleExtensionEntity)
		{
			if(artikelEntity == null)
			{
				return;
			}

			// -
			ArticleId = artikelEntity.ArtikelNr;
			ArticleNumber = artikelEntity.ArtikelNummer;
			Designation1 = artikelEntity.Bezeichnung1;
			Designation2 = artikelEntity.Bezeichnung2;
			CustomerIndex = artikelEntity.Index_Kunde;
			CustomerIndexDate = artikelEntity.Index_Kunde_Datum;
			BomVersion = articleExtensionEntity?.BomVersion;
			BomStatus = articleExtensionEntity?.BomStatus;
			BomStatusId = articleExtensionEntity?.BomStatusId;
		}
	}
}
