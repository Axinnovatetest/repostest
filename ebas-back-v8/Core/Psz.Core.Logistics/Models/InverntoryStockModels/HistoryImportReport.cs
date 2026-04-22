using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class HistoryImportReport
	{
		public IFormFile AttachmentFile { get; set; }
		public DateTime Date { get; set; }
	}
}
