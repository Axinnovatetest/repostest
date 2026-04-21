using Microsoft.AspNetCore.Http;
using System.IO;

namespace Psz.Api.Areas.WorkPlan.Models.WorkSchedule
{
	public class ImportExcelModel
	{
		public int Id { get; set; }
		// 
		public IFormFile WPLFile { get; set; }
		public string WPLFileExtension { get; set; }

		public Psz.Core.Apps.WorkPlan.Models.WorkSchedule.ImportModel ToBusinessModel()
		{
			return new Psz.Core.Apps.WorkPlan.Models.WorkSchedule.ImportModel
			{
				Id = Id,
				WPLFile = getBytes(WPLFile),
				WPLFileExtension = WPLFileExtension,

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
