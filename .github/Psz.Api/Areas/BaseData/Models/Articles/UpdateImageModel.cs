using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Api.Areas.BaseData.Models.Articles
{
	public class UpdateImageModel
	{
		public int Nr { get; set; }
		public IFormFile ImageFile { get; set; }
		public string ImageFileExtension { get; set; }

		public Psz.Core.BaseData.Models.UpdateImageModel ToBusinessModel()
		{
			return new Core.BaseData.Models.UpdateImageModel
			{
				Nr = Nr,
				ImageFileExtension = ImageFileExtension,
				ImageFile = getBytes(ImageFile)
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
