using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.CustomerService.Models
{
	public class BlanketFileModel
	{
		public int BlanketId { get; set; }
		public List<int> FileIds { get; set; }

		public List<Microsoft.AspNetCore.Http.IFormFile> AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }


		public Psz.Core.CustomerService.Models.Blanket.BlanketFilesModel ToBusinessModel()
		{
			return new Psz.Core.CustomerService.Models.Blanket.BlanketFilesModel
			{
				AngeboteNr = BlanketId,
				BlanketsFileIds = FileIds,
				Files = AttachmentFile == null || AttachmentFile.Count <= 0
				? null
				: AttachmentFile.Select(x => new Psz.Core.CustomerService.Models.Blanket.FilesModel
				{
					FileName = x.FileName, // 
					Id = -1,
					AngeboteNr = BlanketId,
					FileData = getBytes(x),
					FileExtension = System.IO.Path.GetExtension(x.FileName) //AttachmentFileExtension,
				})?.ToList()
			};
		}

		internal static byte[] getBytes(Microsoft.AspNetCore.Http.IFormFile file)
		{
			if(file == null)
				return null;

			if(file.Length <= 0)
				return null;

			using(var ms = new System.IO.MemoryStream())
			{
				file.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
