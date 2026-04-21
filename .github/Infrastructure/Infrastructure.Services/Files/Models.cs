using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Files
{
	public class Models
	{
		public class FileContent
		{
			public byte[] FileBytes { get; set; }
			public string FileName { get; set; }
			public string FileExtension { get; set; }
		}
		public class FileAttachmentModel
		{
			public IFormFile AttachedFile { get; set; }
			public string AttachedFileExtension { get; set; }
		}
	}


}
