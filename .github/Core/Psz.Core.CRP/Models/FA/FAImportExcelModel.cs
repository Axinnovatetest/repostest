using Microsoft.AspNetCore.Http;

namespace Psz.Core.CRP.Models.FA
{
	public class FAImportExcelModel
	{
		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }
	}
}
