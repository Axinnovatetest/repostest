using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Core.Common.Models
{
	public class ImportFileRequestModel
	{
		public int Nr { get; set; }
		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }
		public ImportFileRequestBusinessModel ToBusinessModel()
		{
			return new ImportFileRequestBusinessModel
			{
				FileExtension = AttachmentFileExtension,
				FileByteArray = getBytes(AttachmentFile),
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
