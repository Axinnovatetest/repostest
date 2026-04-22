using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderListResponseModel: IPaginatedResponseModel<Models.Budget.Order.OrderModel>
	{
	}
	public class OrderListRequestModel: IPaginatedRequestModel
	{
		public string Searchtext { get; set; }
		public bool? ShowOnlyInProgress { get; set; }
		public bool? ShowCompletelyBooked { get; set; }
		public int? Year { get; set; }
		public int? Month { get; set; }
		public string Type { get; set; }
		public int? Filter { get; set; }
	}
}