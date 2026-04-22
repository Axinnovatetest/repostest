using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Api.Areas.BaseData.Models.Articles.Bom
{
	public class PositionEdit
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

		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }

		public Psz.Core.BaseData.Models.Article.BillOfMaterial.BomPositionEdit ToBusinessModel()
		{
			return new Psz.Core.BaseData.Models.Article.BillOfMaterial.BomPositionEdit
			{
				Id = Id,
				Quantity = Quantity, // Anzahl
				ArticleParentId = ArticleParentId, // Artikel_Nr
				ArticleId = ArticleId,  // Artikel_Nr_des_Bauteils
				ArticleNumber = ArticleNumber, // Artikelnummer
				ArticleDesignation = ArticleDesignation, // Bezeichnung_des_Bauteils
				Position = Position,
				Variante = Variante,
				ProcessId = ProcessId,

				DocumentId = DocumentId,
				DocumentExtension = AttachmentFileExtension,
				DocumentData = getBytes(AttachmentFile),
			};
		}
		internal static byte[] getBytes(IFormFile file)
		{
			if(file == null)
				return null;

			if(file.Length <= 0)
				return null;

			using(var ms = new MemoryStream())
			{
				file.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
