using Microsoft.AspNetCore.Http;
using System.IO;


namespace Psz.Api.Areas.Settings.Models.Company
{
	public class EditLogoModel
	{
		public int CompanyId { get; set; }
		public IFormFile ImageFile { get; set; }
		public string ImageFileExtension { get; set; }

		public Psz.Core.Apps.Settings.Models.Company.EditLogoModel ToBusinessModel()
		{
			return new Psz.Core.Apps.Settings.Models.Company.EditLogoModel
			{
				CompanyId = CompanyId,
				FileExtension = ImageFileExtension,
				FileData = getBytes(ImageFile)
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
