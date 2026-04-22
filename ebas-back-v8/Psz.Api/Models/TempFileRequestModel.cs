using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Api.Models
{
	public class TempFileRequestModel
	{
		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }

		public byte[] GetBytes()
		{
			if(AttachmentFile == null)
				return null;

			if(AttachmentFile.Length <= 0)
				return null;

			using(var ms = new MemoryStream())
			{
				AttachmentFile.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
