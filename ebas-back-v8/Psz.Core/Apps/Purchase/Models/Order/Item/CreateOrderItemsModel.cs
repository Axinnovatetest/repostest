using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Order.Element
{
	public class NotCalculatedOrderElementsModel
	{
		public int OrderId { get; set; }
		public List<CreateItemModel> Items { get; set; } = new List<CreateItemModel>();
	}
}
