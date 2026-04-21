using Microsoft.AspNetCore.Http;

namespace Psz.Core.Common.Models
{
	public class IAttachmentRequestModel
	{
		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }
		public string CommaSeperator { get; set; }
		public string CheckFrequency { get; set; }
		public int ForcastDraftTypeId { get; set; }

	}
}
