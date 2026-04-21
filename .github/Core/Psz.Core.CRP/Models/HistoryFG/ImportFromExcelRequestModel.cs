using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.HistoryFG
{
	public class ImportFromExcelRequestModel
	{
		public string? AttachmentFilePath { get; set; }
		public DateTime Date { get; set; }
	}
}
