using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class EmailNotificationModel
	{
		public int RechnungId { get; set; }
		public bool? CCSender { get; set; }
		public string Title { get; set; }
		public string FromEmail { get; set; }
		public string ToEmail { get; set; }
		public string CCEmail { get; set; }
		public string Message { get; set; }
		public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }
	}
}
