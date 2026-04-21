using System;

namespace Psz.Core.Apps.EDI.Models.Order
{
	public class UpdateElementModel
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public string ItemNumber { get; set; }
		public decimal OrderedQuantity { get; set; }
		public int PositionNumber { get; set; }
		public DateTime? DesiredDate { get; set; }
		public string MeasureUnitQualifier { get; set; }
		public decimal UnitPriceBasis { get; set; }
		public decimal Discount { get; set; }
		public bool FixedPrice { get; set; }
		public string UnloadingPoint { get; set; }
		public string FreeText { get; set; }
		public string Note1 { get; set; }
		public string Note2 { get; set; }
		public int StorageLocationId { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public bool RP { get; set; }
		public int? ItemTypeId { get; set; }
		public decimal VAT { get; set; }
		public decimal UnitPrice { get; set; }
		public bool FixedTotalPrice { get; set; }
		public int? DelNote { get; set; }
		public bool? DelFixed { get; set; }
		public string Postext { get; set; }
		public string Designation1 { get; set; }
		public string Designation2 { get; set; }
		public int CopperBase { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }
		public string CSInterneBemerkung { get; set; }

		public int? RahmenPosId { get; set; }
		public int? RahmenId { get; set; }
	}
}
