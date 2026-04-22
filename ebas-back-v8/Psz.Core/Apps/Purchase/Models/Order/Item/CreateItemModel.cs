using System;

namespace Psz.Core.Apps.Purchase.Models.Order.Element
{
	public class CreateItemModel
	{
		public int? ItemType { get; set; }
		public int Id { get; set; }
		public int PositionNumber { get; set; }
		public string CustomerItemNumber { get; set; }
		public string ItemDescription { get; set; }
		public decimal OrderedQuantity { get; set; }
		public decimal CurrentItemPriceCalculationNet { get; set; }
		public decimal UnitPriceBasis { get; set; }
		public decimal LineItemAmount { get; set; }
		public string FreeText { get; set; }
		public string ItemNumber { get; set; }
		public DateTime DesiredDate { get; set; }
		public string UnloadingPoint { get; set; }

		// - 2022-03-15 - track KundenIndex for Lager.Bestand
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }

		// > Consignee
		public ConsigneeModel Consignee { get; set; } = new ConsigneeModel();
	}
}
