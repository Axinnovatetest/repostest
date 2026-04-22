using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class ImportFromXlsModel
	{
		public IFormFile AttachmentFile { get; set; }

		public DateTime Date { get; set; }
		public string AttachmentFileExtension { get; set; }
		public int? LagerId { get; set; }
	}

}
