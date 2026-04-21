using Microsoft.AspNetCore.Http;

namespace Psz.Api.Areas.CRP.Models
{
	public class FaPlannungHistorieImportXLSModel
	{
		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }
	}
}
