using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Budget.Order.Statistics
{
	public class OrdersArchivedByUserRequestModel
	{
		public OrdersArchivedByUserRequestModel()
		{
			year = DateTime.Now.Year;
		}
		public int? year { get; set; }
	}
}
