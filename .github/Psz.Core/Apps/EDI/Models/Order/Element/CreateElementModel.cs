using System;

namespace Psz.Core.Apps.EDI.Models.Order.Element
{
	public class NotCalculatedElementModel
	{
		public int Id { get; set; }
		public int PositionNumber { get; set; }
		public string CustomerItemNumber { get; set; }
		public string SuppliersItemMaterialNumber { get; set; }
		public string ItemDescription { get; set; }
		public decimal OrderedQuantity { get; set; }
		public string MeasureUnitQualifier { get; set; }
		public decimal CurrentItemPriceCalculationNet { get; set; }
		public decimal UnitPriceBasis { get; set; }
		public decimal LineItemAmount { get; set; }
		public int LineItemActionRequest { get; set; }
		public string FreeText { get; set; }
		public int ChangeType { get; set; }
		public string ItemNumber { get; set; }
		public DateTime DesiredDate { get; set; }
		public string UnloadingPoint { get; set; }
		public int? ItemTypeId { get; set; }


		public string ItemCategory { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }

		// > Consignee
		public ConsigneeModel Consignee { get; set; } = new ConsigneeModel();
	}
}
