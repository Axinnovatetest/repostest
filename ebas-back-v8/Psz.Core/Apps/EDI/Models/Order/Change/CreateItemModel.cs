using System;

namespace Psz.Core.Apps.EDI.Models.Order.Change
{
	public class CreateItemModel
	{
		public int Type { get; set; }
		public string ItemNumber { get; set; }
		public string CustomerItemNumber { get; set; }
		public decimal OrderedQuantity { get; set; }
		public decimal CurrentItemPriceCalculationNet { get; set; }
		public int PositionNumber { get; set; }
		public DateTime DesiredDate { get; set; }
		public string ItemDescription { get; set; }
		public string MeasureUnitQualifier { get; set; }
		public decimal UnitPriceBasis { get; set; }
		public string FreeText { get; set; }
		public decimal LineItemAmount { get; set; }

		// > Consignee
		public ConsigneeModel Consignee { get; set; } = new ConsigneeModel();
	}
}
