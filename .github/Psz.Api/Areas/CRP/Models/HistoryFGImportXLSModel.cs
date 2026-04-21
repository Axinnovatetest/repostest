using System;
using Microsoft.AspNetCore.Http;

namespace Psz.Api.Areas.CRP.Models
{
	public class HistoryFGImportXLSModel
	{
		public IFormFile AttachmentFile { get; set; }
		public DateTime Date { get; set; }
	}
}
