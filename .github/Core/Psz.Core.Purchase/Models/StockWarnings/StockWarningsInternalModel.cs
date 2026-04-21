using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class StockWarningsInternalModel
	{
		public List<WeekValueModel> Corrections { get; set; }
		public List<WeekValueModel> SuggestedOrders { get; set; }
	}
}
