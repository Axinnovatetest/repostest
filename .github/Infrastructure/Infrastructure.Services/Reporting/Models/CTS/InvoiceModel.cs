using System;

namespace Infrastructure.Services.Reporting.Models
{
	public class InvoiceModel
	{
		public int Id { get; set; }
		public int LanguageId { get; set; }
		public int OrderTypeId { get; set; }
		public int CompanyLogoImageId { get; set; }
		public int ImportLogoImageId { get; set; }
		public string Header { get; set; }
		public string ItemsHeader { get; set; }
		public string ItemsFooter1 { get; set; }
		public string ItemsFooter2 { get; set; }

		public string Footer11 { get; set; }
		public string Footer12 { get; set; }
		public string Footer13 { get; set; }
		public string Footer14 { get; set; }
		public string Footer15 { get; set; }
		public string Footer16 { get; set; }
		public string Footer17 { get; set; }
		public string Footer21 { get; set; }
		public string Footer22 { get; set; }
		public string Footer23 { get; set; }
		public string Footer24 { get; set; }
		public string Footer25 { get; set; }
		public string Footer26 { get; set; }
		public string Footer27 { get; set; }
		public string Footer31 { get; set; }
		public string Footer32 { get; set; }
		public string Footer33 { get; set; }
		public string Footer34 { get; set; }
		public string Footer35 { get; set; }
		public string Footer36 { get; set; }
		public string Footer37 { get; set; }
		public string Footer41 { get; set; }
		public string Footer42 { get; set; }
		public string Footer43 { get; set; }
		public string Footer44 { get; set; }
		public string Footer45 { get; set; }
		public string Footer46 { get; set; }
		public string Footer47 { get; set; }
		public string Footer51 { get; set; }
		public string Footer52 { get; set; }
		public string Footer53 { get; set; }
		public string Footer54 { get; set; }
		public string Footer55 { get; set; }
		public string Footer56 { get; set; }
		public string Footer57 { get; set; }
		public string Footer61 { get; set; }
		public string Footer62 { get; set; }
		public string Footer63 { get; set; }
		public string Footer64 { get; set; }
		public string Footer65 { get; set; }
		public string Footer66 { get; set; }
		public string Footer67 { get; set; }
		public string Footer71 { get; set; }
		public string Footer72 { get; set; }
		public string Footer73 { get; set; }
		public string Footer74 { get; set; }
		public string Footer75 { get; set; }
		public string Footer76 { get; set; }
		public string Footer77 { get; set; }
		//
		public DateTime LastUpdateTime { get; set; }
		public int LastUpdateUserId { get; set; }
		// 
		public byte[] CompanyLogoImage { get; set; }
		public byte[] ImportLogoImage { get; set; }

		//
		public string AddressName { get; set; }
		public string AddressAddress { get; set; }
		public string AddressPhone { get; set; }
		public string AddressFax { get; set; }


		// PSZ Address
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string Address4 { get; set; }
		public string Address5 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Ansprechpartner { get; set; }
		public string Abteilung { get; set; }
		public string Briefanrede { get; set; }

		// Document
		public string OrderNumberPO { get; set; }
		public string DocumentType { get; set; }
		public string OrderNumber { get; set; }
		public string OrderDate { get; set; }

		// Client
		public string ClientNumber { get; set; }
		public string InternalNumber { get; set; }// Unser Zeichen
		public string ShippingMethod { get; set; }// Versandart
		public string PaymentMethod { get; set; }// Zahlungsweise
		public string PaymentTarget { get; set; }// Zahlungsart

		public string UST_ID { get; set; }

		// Items
		public string Position { get; set; }
		public string Article { get; set; }
		public string Description { get; set; }
		public string Amount { get; set; }
		public string PE { get; set; }
		public string BasisPrice150 { get; set; }
		public string Cu_G { get; set; }
		public string Cu_Surcharge { get; set; }
		public string UnitPrice { get; set; }
		public string Designation { get; set; }
		public string Unit { get; set; }
		public string TotalPrice150 { get; set; }
		public string DEL { get; set; }
		public string Cu_Total { get; set; }
		public string UnitTotal { get; set; }

		public string Bestellt { get; set; }
		public string Geliefert { get; set; }
		public string Liefertermin { get; set; }
		public string Offen { get; set; }

		public string Abladestelle { get; set; }

		// Summary
		public string SummarySum { get; set; }
		public string SummaryUST { get; set; }
		public string SummaryTotal { get; set; }

		// Delivery Address
		public string LAnrede { get; set; }
		public string LVorname { get; set; }
		public string LName2 { get; set; }
		public string LName3 { get; set; }
		public string Labteilung { get; set; }
		public string Lansprechpartner { get; set; }
		public string LStrabe { get; set; }
		public string LLandPLZOrt { get; set; }

		//
		public string LastPageText1 { get; set; }
		public string LastPageText2 { get; set; }
		public string LastPageText3 { get; set; }
		public string LastPageText4 { get; set; }
		public string LastPageText5 { get; set; }
		public string LastPageText6 { get; set; }
		public string LastPageText7 { get; set; }
		public string LastPageText8 { get; set; }
		public string LastPageText9 { get; set; }

		public string Lieferadresse { get; set; }
		public string Index_Kunde { get; set; }
	}

	public class InvoiceItemModel
	{
		public int InvoiceId { get; set; }
		public int Id { get; set; }

		public string PositionNumber { get; set; } // Pos
		public string ItemNumber { get; set; } // Artikel
		public string Description { get; set; } // Beschreibung
		public string Designation { get; set; } // Bezeichnung
		public string Amount { get; set; }  // Menge
		public string PE { get; set; } // PE
		public string Unit { get; set; } // Einheit
		public string BasePrice { get; set; } // basispreis
		public string TotalPrice { get; set; } // Gesamppreis
		public string TotalCopper { get; set; } // Cu-G
		public string DEL { get; set; } // DEL
		public string SurchargeCopper { get; set; } // Cu-Zuschlag
		public string TotalSurchargeCopper { get; set; } // Cu-Zuschlag Gesamt
		public string UnitPrice { get; set; } // Einzelpreis
		public string TotalUnitPrice { get; set; } // Einzelpreis Gesamt

		public string AB_Pos_zu_RA_Pos { get; set; }
		public string Liefertermin { get; set; }
		public string Geliefert { get; set; }
		public string Anzahl { get; set; }
		public string Bestellt { get; set; }
		public string Offen { get; set; }
		public string Abladestelle { get; set; }
		public string Postext { get; set; }
		public string DELFixiert { get; set; }
		public string Index_Kunde { get; set; }
	}

}
