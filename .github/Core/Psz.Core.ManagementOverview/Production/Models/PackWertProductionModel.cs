using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Models
{
	public class PackWertProductionModel
	{
		public decimal wertROH { get; set; }
		public decimal wertFG { get; set; }
		public decimal wertOrder { get; set; }
		public decimal wertScrap { get; set; }
		public decimal prozentsatzScrap { get; set; }
	}
}
