using System;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class FilesModel
	{
		public string actionFile { get; set; }
		public DateTime? fileDate { get; set; }
		public byte[] DocumentData { get; set; }
		public string DocumentExtension { get; set; }
	}
}
