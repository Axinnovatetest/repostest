using Microsoft.AspNetCore.Http;

namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class UploadReport4WipModel
	{
		public string? AttachmentFilePath { get; set; }
		public DateTime Date { get; set; }
	}
}
