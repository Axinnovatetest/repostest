using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.DeliveryNote
{
	public class LSPositionsFromABModel
	{
		public int NrLS { get; set; }
		public int NrAB { get; set; }
		public List<ItemModel> Items { get; set; }
	}
}
