using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class ReportLogoImageModel
	{
		public int LagerId { get; set; }
		public IFormFile LogoImage { get; set; }
		public string LogoImageExtension { get; set; }
		public string Typ { get; set; }

		public Psz.Core.CustomerService.Reporting.Models.ReportLogoImportedImage ToBusinessModel()
		{
			return new Psz.Core.CustomerService.Reporting.Models.ReportLogoImportedImage
			{
				Typ = Typ,
				LagerId = LagerId,
				LogoImage = getBytes(LogoImage),
				LogoImageExtension = LogoImageExtension,
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

	public class ReportLogoImportedImage
	{
		public int LagerId { get; set; }
		public string Typ { get; set; }
		public byte[] LogoImage { get; set; }
		public string LogoImageExtension { get; set; }
	}
}
