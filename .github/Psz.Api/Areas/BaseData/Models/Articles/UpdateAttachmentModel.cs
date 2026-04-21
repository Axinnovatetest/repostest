using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Api.Areas.BaseData.Models.Articles
{
	public class UpdateAttachmentModel
	{
		public int Id { get; set; }
		public int AttachmentType { get; set; }
		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }

		public Psz.Core.BaseData.Models.UpdateAttachmentModel ToBusinessModel()
		{
			return new Core.BaseData.Models.UpdateAttachmentModel
			{
				Id = Id,
				AttachmentFileExtension = AttachmentFileExtension,
				AttachmentFile = getBytes(AttachmentFile),
				AttachmentType = AttachmentType
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
