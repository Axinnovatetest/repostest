using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.EDI.FileTrackingInEntity
{
	public class FileHelperExtractDocumentsEntity
	{

		public string? DocumentNumber { get; set; }
		public string? FilePath { get; set; }
		public string? MessageType { get; set; }
		public string? CustomerNumber { get; set; }
		public string? RecipientId { get; set; }
		public string? CustomerName { get; set; }
	}


}
