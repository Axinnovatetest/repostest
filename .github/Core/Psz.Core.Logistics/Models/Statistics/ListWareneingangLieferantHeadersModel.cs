using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class ListWareneingangLieferantHeadersModel
	{
		public string name1 { get; set; }
		public List<ListWareneingangDetailsByKundeUndDatumModel> details { get; set; }
	}
}
