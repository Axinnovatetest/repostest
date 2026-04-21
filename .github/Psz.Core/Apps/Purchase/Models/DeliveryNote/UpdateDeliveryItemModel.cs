using System;

namespace Psz.Core.Apps.Purchase.Models.DeliveryNote
{
	public class UpdateDeliveryItemModel
	{
		public string ItemNumber { get; set; }
		public Decimal? CopperSurcharge { get; set; }
		public Decimal? OpenQuantity_CopperSurcharge { get; set; }
		public Decimal UnitPrice { get; set; }
		public Decimal? TotalPrice { get; set; }
		public Decimal? OpenQuantity_UnitPrice { get; set; }
		public Decimal? OpenQuantity_TotalPrice { get; set; }
		public int Id { get; set; }
		public int? ItemId { get; set; }
		public int OrderId { get; set; }
		public string OrderNumber { get; set; }
		public string CreateDate { get; set; }
		//TODO:--- Param Fields
		public int? ItemTypeId { get; set; }
		public Decimal? OpenQuantity_Quantity { get; set; }
		//TODO:--- Get fields
		public string Designation1 { get; set; }
		public string Designation2 { get; set; }
		public string Designation3 { get; set; }
		public Decimal? DelNote { get; set; }
		public string CustomerItemNumber { get; set; }
		public Decimal? Discount { get; set; }
		public Decimal? VAT { get; set; }
		public string ItemCustomerDescription { get; set; }
		//TODO:--- Calculated fields
		public Decimal? OriginalOrderQuantity { get; set; }
		public Decimal? OriginalOrderAmount { get; set; }
		public Decimal? CopperBase { get; set; }
		public Decimal? CopperWeight { get; set; }
		public Decimal? CurrentItemPriceCalculationNet { get; set; }
		public Decimal? OpenQuantity_CopperWeight { get; set; }
		public Decimal? UnitPriceBasis { get; set; }
		//TODO:--- Changed fields
		public int DelieveryId { get; set; }
		public int Version { get; set; }
		public int Position { get; set; }
		public int? StorageLocationId { get; set; }
		public string StorageLocationName { get; set; }
		public bool? DelFixed { get; set; }
		public int? ChangeType { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public DateTime? DesiredDate { get; set; }
		public bool? Done { get; set; }
		public bool? FixedUnitPrice { get; set; }
		public bool? FixedTotalPrice { get; set; }
		public bool? RP { get; set; }
		public string FreeText { get; set; }
		public string Note1 { get; set; }
		public string Note2 { get; set; }
		public int ProductionNumber { get; set; }
		//TODO:--- New fields
		public int positionViewMode { get; set; }
		public bool? DelNew { get; set; }
		public bool? DelUpdate { get; set; }
		public int index { get; set; }
		//!CS Info
		public string Versandinfo_von_CS { get; set; }
		//!Packing
		public bool? Packstatus { get; set; }
		public string Gepackt_von { get; set; }
		public string Gepackt_Zeitpunkt { get; set; }
		public string Packinfo_von_Lager { get; set; }
		//!Shipping
		public bool? Versandstatus { get; set; }
		public string Versanddienstleister { get; set; }
		public int? Versandnummer { get; set; }
		public string Versandinfo_von_Lager { get; set; }
		public string UnloadingPoint { get; set; } //Abladestelle
												   //!EDI
		public Decimal? EDI_PREIS_KUNDE { get; set; }
		public Decimal? EDI_PREISEINHEIT { get; set; }
		//TODO:--- Position Table fields
		public int originalPosition { get; set; }
		public Decimal? ArticleQuantity { get; set; }
		public Decimal OrderedQuantity { get; set; }
		public Decimal? DeliveredQuantity { get; set; }
		public Decimal? DesiredQuantity { get; set; }
		public Decimal? DesiredUnitPrice { get; set; }
		public Decimal? ApprovedQuantity { get; set; }
		public Decimal? ApprovedUnitPrice { get; set; }
		// ????---- fields
		public string MeasureUnitQualifier { get; set; }
		public string DrawingIndex { get; set; }
		public bool? HasPendingChange { get; set; }
		public bool? HasPendingCancel { get; set; }
		public bool? termin_eingehalten { get; set; }
		public Decimal? CalculatedValue { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }
		public string Postext { get; set; }
		public string CSInterneBemerkung { get; set; }
	}
}
