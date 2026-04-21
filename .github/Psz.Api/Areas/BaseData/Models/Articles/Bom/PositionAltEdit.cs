using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Api.Areas.BaseData.Models.Articles.Bom
{
	public class PositionAltEdit
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

		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }

		public Psz.Core.BaseData.Models.Article.BillOfMaterial.BomPositionAltEdit ToBusinessModel()
		{
			return new Psz.Core.BaseData.Models.Article.BillOfMaterial.BomPositionAltEdit
			{
				ParentArticleId = ParentArticleId,
				Id = Id,
				Position = Position,
				Quantity = Quantity,
				ArticleId = ArticleId,
				ArticleNumber = ArticleNumber,
				ArticleDesignation = ArticleDesignation,
				OriginalPositionId = OriginalPositionId,

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
