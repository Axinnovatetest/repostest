using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatedInvoiceModels
{
	public class CreatedInvoiceModel
	{
		public int TotalCount { get; set; }
		public int LsTotalCount { get; set; }
		public string ForfallNr { get; set; }
		public string Customer { get; set; }
		public bool Sent { get; set; }

		//-
		public int RgNr { get; set; }
		public int RgNumber { get; set; }
		public decimal Amount { get; set; } = 0;
		public int CustomerNumber { get; set; }
		public int LsNumber { get; set; }
		public string TypeInvoice { get; set; }
		public List<SammelItem> SammelItems { get; set; }
		public class SammelItem
		{
			public int RechnungProjectNr { get; set; }
			public int LSForfallNr { get; set; }
			public int LSNr { get; set; }
		}
	}
}
