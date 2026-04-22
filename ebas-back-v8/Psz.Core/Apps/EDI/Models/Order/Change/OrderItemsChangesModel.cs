using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.Order.Change
{
	public class OrderItemsChangesModel
	{
		public List<Models.Order.Element.OrderElementModel> NewElements { get; set; } = new List<Element.OrderElementModel>();
		public List<Models.Order.Element.OrderElementModel> CanceledElements { get; set; } = new List<Element.OrderElementModel>();
		public List<ChangedElementModel> ChangedElements { get; set; } = new List<ChangedElementModel>();

		public class ChangedElementModel
		{
			public Models.Order.Element.OrderElementModel OriginalItem { get; set; }
			public Models.Order.Element.OrderElementModel ChangedItem { get; set; }

			public List<ChangeModel> Changes { get; set; }

			public class ChangeModel
			{
				public string Key { get; set; }
				public object Value { get; set; }
			}
		}
	}
}
