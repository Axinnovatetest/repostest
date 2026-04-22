using Microsoft.AspNetCore.Http;

namespace Psz.Api.Areas.BaseData.Models.Articles.Bom
{
	public class PositionImportXLSModel
	{

		public int ArticleId { get; set; }
		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }
		public bool? Overwrite { get; set; } = true;

		// - 2022-07-13
		public bool UpgradeBomVersion { get; set; } = false;
	}
}
