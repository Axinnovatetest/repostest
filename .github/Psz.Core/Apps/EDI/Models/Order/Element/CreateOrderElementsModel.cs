using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.Order.Element
{
	public class NotCalculatedOrderElementsModel
	{
		public int OrderId { get; set; }
		public List<NotCalculatedElementModel> Elements { get; set; } = new List<NotCalculatedElementModel>();
	}
}
