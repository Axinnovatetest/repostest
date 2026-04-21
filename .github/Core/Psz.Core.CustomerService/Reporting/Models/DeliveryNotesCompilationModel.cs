using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class DeliveryNotesCompilationModel
	{
		public Psz.Core.CustomerService.Reporting.Models.DeliveryNoteModel Header { get; set; }
		public List<Psz.Core.CustomerService.Reporting.Models.DeliveryNoteModel.DeliveryNoteItemModel> Deliveries { get; set; }
		public string Logo { get; set; }
	}
}