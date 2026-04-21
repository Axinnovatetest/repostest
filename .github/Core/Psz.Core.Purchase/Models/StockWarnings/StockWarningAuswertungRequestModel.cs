using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class StockWarningAuswertungRequestModel
	{
		public int? Prio { get; set; }
		public int? UnitId { get; set; }
	}
}