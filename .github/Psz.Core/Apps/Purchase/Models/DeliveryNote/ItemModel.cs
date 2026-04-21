using System;

namespace Psz.Core.Apps.Purchase.Models.DeliveryNote
{
	public class ItemModel
	{
		public int Id { get; set; }
		public int? StorageLocationId { get; set; }
		public int? ItemId { get; set; }
		public string ItemNumber { get; set; }           //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Artikelnummer
		public string Designation1 { get; set; }            //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Bezeichnung1
		public string Designation2 { get; set; }
		public string Designation3 { get; set; }
		public Decimal? OriginalOrderQuantity { get; set; }         //[PSZ_Auftrag LS 01 angebotene Artikel filtern].OriginalAnzahl
		public DateTime? DesiredDate { get; set; }          //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Wunschtermin
		public Decimal? DeliveredQuantity { get; set; }              //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Geliefert
		public Decimal? OpenQuantity_Quantity { get; set; }                 //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Anzahl
		public DateTime? DeliveryDate { get; set; }          //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Liefertermin
		public int? ProductionNumber { get; set; }           //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Fertigungsnummer
		public Decimal? AktuelleLiefermenge { get; set; }    //[PSZ_Auftrag LS 01 angebotene Artikel filtern].[Aktuelle Liefermenge]
		public bool Done { get; set; }              //[PSZ_Auftrag LS 01 angebotene Artikel filtern].erledigt_pos
		public string Standardversand { get; set; }         //[PSZ_Auftrag LS 01 angebotene Artikel filtern].standardversand
		public DateTime? Versandatum { get; set; }           //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Versandatum
		public bool termin_eingehalten { get; set; }        //[PSZ_Auftrag LS 01 angebotene Artikel filtern].termin_eingehalten
															//*************new props (souilmi)
		public bool? DelFixed { get; set; }
		public bool? FixedTotalPrice { get; set; }
		public bool? FixedUnitPrice { get; set; }
		public string FreeText { get; set; }
		public string Note1 { get; set; }
		public string Note2 { get; set; }
		public int? ItemTypeId { get; set; }
		public bool? RP { get; set; }
		public int? Position { get; set; }
		//!CS Info
		public string Versandinfo_von_CS { get; set; }
		//!Packing
		public bool? Packstatus { get; set; }
		public string Gepackt_von { get; set; }
		public DateTime? Gepackt_Zeitpunkt { get; set; }
		public string Packinfo_von_Lager { get; set; }
		//!Shipping
		public bool? Versandstatus { get; set; }
		public string Versanddienstleister { get; set; }
		public string Versandnummer { get; set; }
		public string Versandinfo_von_Lager { get; set; }
		public string UnloadingPoint { get; set; } //Abladestelle
												   //!EDI
		public Decimal? EDI_PREIS_KUNDE { get; set; }
		public Decimal? EDI_PREISEINHEIT { get; set; }
		public Decimal? UnitPrice { get; set; }
		//new (souilmi)
		public int OrderId { get; set; }
		public int Version { get; set; }
		public string OrderNumber { get; set; }
		public DateTime? CreateDate { get; set; }
		public string StorageLocationName { get; set; }
		public Decimal? OpenQuantity_CopperSurcharge { get; set; }
		public Decimal? OpenQuantity_CopperWeight { get; set; }
		public Decimal? OpenQuantity_TotalPrice { get; set; }
		public Decimal? OpenQuantity_UnitPrice { get; set; }
		public Decimal? CopperWeight { get; set; }
		public Decimal? CopperSurcharge { get; set; }
		public string MeasureUnitQualifier { get; set; }
		public Decimal? OriginalOrderAmount { get; set; }
		public Decimal? CopperBase { get; set; }
		public Decimal? TotalPrice { get; set; }
		public Decimal? UnitPriceBasis { get; set; }
		public string DrawingIndex { get; set; }
		public Decimal? Discount { get; set; }
		public Decimal? VAT { get; set; }
		public Decimal? DelNote { get; set; }
		public string CustomerItemNumber { get; set; }
		public string ItemCustomerDescription { get; set; }
		public Decimal? CalculatedValue { get; set; }
		public int index { get; set; }
		public int? LS_ZU_AB { get; set; }
		public string Postext { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }
		public string CSInterneBemerkung { get; set; }

	}
}
