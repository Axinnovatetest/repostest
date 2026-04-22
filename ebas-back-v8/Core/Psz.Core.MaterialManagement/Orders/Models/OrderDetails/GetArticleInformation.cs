using Infrastructure.Data.Entities.Joins.MTM.Order;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class GetArticleInformationRequestModel
	{
		public int SupplierId { get; set; }
		public int ArticleId { get; set; }
		public int OrderId { get; set; }
		public bool IsEdit { get; set; }
		public int Nr { get; set; }
	}
	public class GetArticleInformationResponseModel
	{
		public string ArticleDiscription1 { get; set; }  // Description 1     Bestellnummern.Artikelbezeichnung, 
		public string ArticleDiscription2 { get; set; } // Description 2    Bestellnummern.Artikelbezeichnung2,
		public string Unit { get; set; }                   // Unit   Artikel.Einheit,        
		public decimal VAT { get; set; }          //VAT  Bestellnummern.Umsatzsteuer,     
		public decimal Price { get; set; }          //purchasing price   Bestellnummern.Einkaufspreis,    
		public string Bestell_Nr { get; set; }              ///            Bestellnummern.[Bestell-Nr],
		public decimal Discount { get; set; }          //Discount   Bestellnummern.Rabatt,   
		public decimal UnitPrice { get; set; }   //Unit price   Bestellnummern.Preiseinheit,  
		public DateTime ReplinshementDate { get; set; }  ///replenishment period + CURRENT DATE   GETDATE()+Bestellnummern.Wiederbeschaffungszeitraum, 
		public decimal MinimumOrderQuantity { get; set; } // Minimum Order Quantity   Bestellnummern.Mindestbestellmenge,
		public decimal TotalPrice { get; set; }//  Mindestbestellmenge*Einkaufspreis/Preiseinheit*(1-Rabatt) Calculated totalPrice
		public int? RA_Pos_zu_Bestellposition { get; set; }//  From Angebote 
		public int? RANummer { get; set; }
		public List<InfoRahmennummerEntity> InforRammen { get; set; }//  From Angebote 
		public List<DropDownMenu> LagerorteList { get; set; }//  From Angebote 
		public int LagerorteId { get; set; }//  From Angebote 
		public Boolean Kanban { get; set; }//  From Artikel
		public bool IsEdit { get; set; }
		public decimal Quantity { get; set; }
		public DateTime Bestatigter_Termin { get; set; }
		public string Bemerkung { get; set; }
		public string ABNr { get; set; }
		public decimal OriginalPrice { get; set; }
		public List<QuantitySupplierPrice> QuantitySupplierPrices { get; set; }   // On Quantity change the unit price may change too.
		public string CurrentRahmen { get; set; }
		// - 2023-08-25 - CoC
		public string CocVersion { get; set; }


		public GetArticleInformationResponseModel() { }
		public GetArticleInformationResponseModel(Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity bestellte_ArtikelEntity
											, Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity articleEntity
											, Infrastructure.Data.Entities.Tables.MTM.BestellnummernEntity bestellnummernEntity
											, List<Infrastructure.Data.Entities.Tables.MTM.Bestellnummern_StaffelpreiseEntity> bestellnummern_StaffelpreiseEntities
											, List<Infrastructure.Data.Entities.Joins.MTM.Order.InfoRahmennummerEntity> infoRahmennummerEntities
											, List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> lagerorteEntities)
		{
			decimal Mindestbestellmenge = 0;
			var linkedRapos = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition ?? -1)
				: null;
			ArticleDiscription1 = bestellnummernEntity?.Artikelbezeichnung;
			ArticleDiscription2 = bestellnummernEntity?.Artikelbezeichnung2;
			Bestell_Nr = bestellnummernEntity?.Bestell_Nr;
			Unit = articleEntity.Einheit;
			Price = bestellnummernEntity is not null && bestellnummernEntity.Einkaufspreis.HasValue ? bestellnummernEntity.Einkaufspreis.Value : 0;
			OriginalPrice = bestellnummernEntity is not null && bestellnummernEntity.Einkaufspreis.HasValue ? bestellnummernEntity.Einkaufspreis.Value : 0;
			MinimumOrderQuantity = bestellnummernEntity is not null && bestellnummernEntity.Mindestbestellmenge.HasValue ? (decimal.TryParse(bestellnummernEntity.Mindestbestellmenge.Value.ToString(), out Mindestbestellmenge) ? Mindestbestellmenge : 0) : 0;
			UnitPrice = bestellnummernEntity is not null && bestellnummernEntity.Preiseinheit.HasValue ? bestellnummernEntity.Preiseinheit.Value : 0;
			Discount = bestellnummernEntity is not null && bestellnummernEntity.Rabatt.HasValue ? (decimal.TryParse(bestellnummernEntity.Rabatt.Value.ToString(), out decimal Rabatt) ? Rabatt : 0) : 0;
			TotalPrice = 0;
			ReplinshementDate = bestellte_ArtikelEntity?.Liefertermin ?? (DateTime.Now.AddDays(bestellnummernEntity is not null && bestellnummernEntity.Wiederbeschaffungszeitraum.HasValue ? bestellnummernEntity.Wiederbeschaffungszeitraum.Value : 0));
			VAT = bestellnummernEntity is not null && bestellnummernEntity.Umsatzsteuer.HasValue ? bestellnummernEntity.Umsatzsteuer.Value : 0;
			QuantitySupplierPrices = bestellnummern_StaffelpreiseEntities?.Select(x => new QuantitySupplierPrice { PurchasePrice = x.Einkaufspreis.HasValue ? x.Einkaufspreis.Value : 0, Quantity = x.ab_Anzahl.HasValue ? x.ab_Anzahl.Value : 0 }).ToList();
			Kanban = articleEntity.Kanban ?? false;
			RA_Pos_zu_Bestellposition = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition;
			RANummer = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(linkedRapos.AngebotNr ?? -1).Nr
				: null;
			InforRammen = infoRahmennummerEntities;
			LagerorteList = DropDownMenu.GetDropDownMenu(lagerorteEntities, "Lagerort_id", "Lagerort");
			LagerorteId = -1;
			Quantity = bestellte_ArtikelEntity?.Anzahl ?? (Mindestbestellmenge != 0 ? Mindestbestellmenge : 0);
			IsEdit = false;
			Bestatigter_Termin = bestellte_ArtikelEntity?.Bestatigter_Termin ?? new DateTime(2999, 12, 31);
			Bemerkung = bestellte_ArtikelEntity?.Bemerkung_Pos ?? "";
			ABNr = "";
			CocVersion = bestellte_ArtikelEntity?.CocVersion;
		}
		public GetArticleInformationResponseModel(Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity bestellte_ArtikelEntity
			, List<Infrastructure.Data.Entities.Tables.MTM.Bestellnummern_StaffelpreiseEntity> bestellnummern_StaffelpreiseEntities
			, List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> lagerorteEntities
			, List<Infrastructure.Data.Entities.Joins.MTM.Order.InfoRahmennummerEntity> infoRahmennummerEntities
			, Infrastructure.Data.Entities.Tables.MTM.BestellnummernEntity bestellnummernEntity, string currentRahmen)
		{
			var linkedRapos = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition ?? -1)
				: null;
			ArticleDiscription1 = bestellte_ArtikelEntity.Bezeichnung_1;
			ArticleDiscription2 = bestellte_ArtikelEntity.Bezeichnung_2;
			Bestell_Nr = bestellte_ArtikelEntity.Bestellnummer;
			Unit = bestellte_ArtikelEntity.Einheit;
			Price = bestellte_ArtikelEntity.Einzelpreis.HasValue ? bestellte_ArtikelEntity.Einzelpreis.Value : 0;
			OriginalPrice = bestellnummernEntity is not null && bestellnummernEntity.Einkaufspreis.HasValue ? bestellnummernEntity.Einkaufspreis.Value : 0;
			MinimumOrderQuantity = bestellnummernEntity is not null && bestellnummernEntity.Mindestbestellmenge.HasValue ? (decimal.TryParse(bestellnummernEntity.Mindestbestellmenge.Value.ToString(), out decimal Mindestbestellmenge) ? Mindestbestellmenge : 0) : 0;  // bestellte_ArtikelEntity.Anzahl.HasValue ? (decimal.TryParse(bestellte_ArtikelEntity.Anzahl.Value.ToString() , out decimal Mindestbestellmenge) ? Mindestbestellmenge : 0) : 0;
			UnitPrice = bestellte_ArtikelEntity.Preiseinheit.HasValue ? bestellte_ArtikelEntity.Preiseinheit.Value : 0;
			Discount = bestellte_ArtikelEntity.Rabatt.HasValue ? (decimal.TryParse(bestellte_ArtikelEntity.Rabatt.Value.ToString(), out decimal Rabatt) ? Rabatt : 0) : 0;
			TotalPrice = bestellte_ArtikelEntity.Gesamtpreis.HasValue ? bestellte_ArtikelEntity.Gesamtpreis.Value : 0;
			ReplinshementDate = bestellte_ArtikelEntity.Liefertermin.HasValue ? bestellte_ArtikelEntity.Liefertermin.Value : DateTime.Today;
			VAT = bestellte_ArtikelEntity.Umsatzsteuer.HasValue ? Convert.ToDecimal(bestellte_ArtikelEntity.Umsatzsteuer.Value.ToString()) : 0;
			QuantitySupplierPrices = bestellnummern_StaffelpreiseEntities?.Select(x => new QuantitySupplierPrice { PurchasePrice = x.Einkaufspreis.HasValue ? x.Einkaufspreis.Value : 0, Quantity = x.ab_Anzahl.HasValue ? x.ab_Anzahl.Value : 0 }).ToList();
			Kanban = bestellte_ArtikelEntity.Kanban ?? false;
			RA_Pos_zu_Bestellposition = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition;
			RANummer = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(linkedRapos.AngebotNr ?? -1).Nr
				: null;
			InforRammen = infoRahmennummerEntities;
			LagerorteList = DropDownMenu.GetDropDownMenu(lagerorteEntities, "Lagerort_id", "Lagerort");
			LagerorteId = bestellte_ArtikelEntity.Lagerort_id ?? -1;
			Quantity = bestellte_ArtikelEntity.Anzahl.HasValue ? (decimal.TryParse(bestellte_ArtikelEntity.Anzahl.Value.ToString(), out decimal quantity) ? quantity : 0) : 0;
			IsEdit = true;
			Bestatigter_Termin = bestellte_ArtikelEntity?.Bestatigter_Termin ?? new DateTime(2999, 12, 31);
			Bemerkung = bestellte_ArtikelEntity.Bemerkung_Pos;
			ABNr = bestellte_ArtikelEntity.AB_Nr_Lieferant;
			CurrentRahmen = currentRahmen;
			CocVersion = bestellte_ArtikelEntity?.CocVersion;
		}
		public GetArticleInformationResponseModel(Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity bestellte_ArtikelEntity, string currentRahmen)
		{
			var linkedRapos = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition ?? -1)
				: null;
			ArticleDiscription1 = bestellte_ArtikelEntity.Bezeichnung_1;
			ArticleDiscription2 = bestellte_ArtikelEntity.Bezeichnung_2;
			Bestell_Nr = bestellte_ArtikelEntity.Bestellnummer;
			Unit = bestellte_ArtikelEntity.Einheit;
			Price = bestellte_ArtikelEntity.Einzelpreis.HasValue ? bestellte_ArtikelEntity.Einzelpreis.Value : 0;
			//OriginalPrice = bestellnummernEntity is not null && bestellnummernEntity.Einkaufspreis.HasValue ? bestellnummernEntity.Einkaufspreis.Value : 0;
			//MinimumOrderQuantity = bestellnummernEntity is not null && bestellnummernEntity.Mindestbestellmenge.HasValue ? (decimal.TryParse(bestellnummernEntity.Mindestbestellmenge.Value.ToString(), out decimal Mindestbestellmenge) ? Mindestbestellmenge : 0) : 0;  // bestellte_ArtikelEntity.Anzahl.HasValue ? (decimal.TryParse(bestellte_ArtikelEntity.Anzahl.Value.ToString() , out decimal Mindestbestellmenge) ? Mindestbestellmenge : 0) : 0;
			UnitPrice = bestellte_ArtikelEntity.Preiseinheit.HasValue ? bestellte_ArtikelEntity.Preiseinheit.Value : 0;
			Discount = bestellte_ArtikelEntity.Rabatt.HasValue ? (decimal.TryParse(bestellte_ArtikelEntity.Rabatt.Value.ToString(), out decimal Rabatt) ? Rabatt : 0) : 0;
			TotalPrice = bestellte_ArtikelEntity.Gesamtpreis.HasValue ? bestellte_ArtikelEntity.Gesamtpreis.Value : 0;
			ReplinshementDate = bestellte_ArtikelEntity.Liefertermin.HasValue ? bestellte_ArtikelEntity.Liefertermin.Value : DateTime.Today;
			VAT = bestellte_ArtikelEntity.Umsatzsteuer.HasValue ? Convert.ToDecimal(bestellte_ArtikelEntity.Umsatzsteuer.Value.ToString()) : 0;
			//QuantitySupplierPrices = bestellnummern_StaffelpreiseEntities?.Select(x => new QuantitySupplierPrice { PurchasePrice = x.Einkaufspreis.HasValue ? x.Einkaufspreis.Value : 0, Quantity = x.ab_Anzahl.HasValue ? x.ab_Anzahl.Value : 0 }).ToList();
			Kanban = bestellte_ArtikelEntity.Kanban ?? false;
			RA_Pos_zu_Bestellposition = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition;
			RANummer = bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(linkedRapos.AngebotNr ?? -1).Nr
				: null;
			//InforRammen = infoRahmennummerEntities;
			//LagerorteList = DropDownMenu.GetDropDownMenu(lagerorteEntities, "Lagerort_id", "Lagerort");
			LagerorteId = bestellte_ArtikelEntity.Lagerort_id ?? -1;
			Quantity = bestellte_ArtikelEntity.Anzahl.HasValue ? (decimal.TryParse(bestellte_ArtikelEntity.Anzahl.Value.ToString(), out decimal quantity) ? quantity : 0) : 0;
			IsEdit = true;
			Bestatigter_Termin = bestellte_ArtikelEntity?.Bestatigter_Termin ?? new DateTime(2999, 12, 31);
			Bemerkung = bestellte_ArtikelEntity.Bemerkung_Pos;
			ABNr = bestellte_ArtikelEntity.AB_Nr_Lieferant;
			CurrentRahmen = currentRahmen;
			CocVersion = bestellte_ArtikelEntity?.CocVersion;
		}
		public class QuantitySupplierPrice
		{
			public decimal PurchasePrice { get; set; }
			public decimal Quantity { get; set; }
		}
	}
}