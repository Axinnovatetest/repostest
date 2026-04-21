using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.DeliveryNote
{
	public class ValidateModel
	{
		public int Nr { get; set; }
		public int AngebotNr { get; set; }          // Angebote.[Angebot - Nr]
		public int KundenNr { get; set; }           //Angebote.[Kunden-Nr]
		public string VornameFirma { get; set; }    //Angebote.[Vorname/NameFirma]
		public string Bezug { get; set; }           //Angebote.[Bezug]
		public bool VersandBerechnen { get; set; }
		public decimal? Versandkosten { get; set; }

		// Items fields that should be same for all items
		public string Standardversand { get; set; }         //[PSZ_Auftrag LS 01 angebotene Artikel filtern].standardversand // Shipping Method
		public DateTime Versandatum { get; set; }           //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Versandatum >>>>> Shipping Date
		public List<Item> Items { get; set; } = new List<Item>();

		public class Item
		{
			public int Nr { get; set; }
			public int Lagerort_id { get; set; }
			public int ArtikelNr { get; set; }
			public string ArtikelNummer { get; set; }           //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Artikelnummer
			public string Bezeichnung1 { get; set; }            //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Bezeichnung1
			public decimal OriginalAnzahl { get; set; }         //[PSZ_Auftrag LS 01 angebotene Artikel filtern].OriginalAnzahl
			public DateTime Wunschtermin { get; set; }          //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Wunschtermin
			public decimal Geliefert { get; set; }              //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Geliefert
			public decimal Anzahl { get; set; }                 //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Anzahl
			public DateTime? Liefertermin { get; set; }          //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Liefertermin
			public int Fertigungsnummer { get; set; }           //[PSZ_Auftrag LS 01 angebotene Artikel filtern].Fertigungsnummer
			public decimal AktuelleLiefermenge { get; set; }    //[PSZ_Auftrag LS 01 angebotene Artikel filtern].[Aktuelle Liefermenge]
			public bool erledigt_pos { get; set; }              //[PSZ_Auftrag LS 01 angebotene Artikel filtern].erledigt_pos
			public bool termin_eingehalten { get; set; }        //[PSZ_Auftrag LS 01 angebotene Artikel filtern].termin_eingehalten >>>> Shipping Date Compliance

			//new props (souilmi)
			public bool? DelFixed { get; set; }
			public bool? FixedTotalPrice { get; set; }
			public bool? FixedUnitPrice { get; set; }
			public string FreeText { get; set; }
			public string Note1 { get; set; }
			public string Note2 { get; set; }
			public int? ItemTypeId { get; set; }
			public bool? RP { get; set; }
			public int Position { get; set; }
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
			public int? Versandnummer { get; set; }
			public string Versandinfo_von_Lager { get; set; }
			public string UnloadingPoint { get; set; } //Abladestelle
													   //!EDI
			public Decimal? EDI_PREIS_KUNDE { get; set; }
			public Decimal? EDI_PREISEINHEIT { get; set; }
			//
			public decimal UnitPrice { get; set; }
		}
	}
}
