using System;

namespace Psz.Core.Apps.Purchase.Models.Order
{
	public class UpdateItemModel
	{
		public int? ItemTypeId { get; set; }
		public int Id { get; set; }
		public int OrderId { get; set; }
		public string ItemNumber { get; set; }
		public decimal OrderedQuantity { get; set; }
		public int PositionNumber { get; set; }
		public DateTime? DesiredDate { get; set; }
		public string MeasureUnitQualifier { get; set; }
		public decimal UnitPriceBasis { get; set; }
		public decimal Discount { get; set; }
		public bool IsFixedPrice { get; set; }
		public string ConsigneeUnloadingPoint { get; set; }
		public string FreeText { get; set; }
		public string Note1 { get; set; }
		public string Note2 { get; set; }
		public int StorageLocationId { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public bool RP { get; set; }
		public int Version { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }
		public string Postext { get; set; }
		public string Designation1 { get; set; }
		public string Designation2 { get; set; }
		public string CSInterneBemerkung { get; set; }
	}
}
