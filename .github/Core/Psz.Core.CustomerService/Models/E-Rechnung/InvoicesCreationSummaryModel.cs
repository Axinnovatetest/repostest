using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class InvoicesCreationSummaryModel
	{
		public int LseinzelTotalCount { get; set; }
		public int CreatedeinzelInvoicesCount { get; set; }

		public int LssammelTotalCount { get; set; }
		public int CreatedsammelInvoicesCount { get; set; }

		public int LsSonstigesTotalCount { get; set; }
		public int CreatedSonstigesInvoicesCount { get; set; }
		public int Creationresult { get; set; }
	}
	public class InvoicesCreationModel
	{
		public int LsTotalCount { get; set; }
		public int CreatedInvoicesCount { get; set; }
	}
}
