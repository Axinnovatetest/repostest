using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Production.Reporting
{
	public class TechnicalReportModel
	{
		public List<ProductionModel> Productions { get; set; } = new List<ProductionModel>();
		public List<ItemModel> Items { get; set; } = new List<ItemModel>();

		public class ProductionModel
		{
			public int Id { get; set; }

			public string L01 { get; set; } // -> ERSTMUSTERAUFTRAG
			public string L02 { get; set; } // -> D-2002247
			public string L03 { get; set; } // PB -> 11

			public string L04 { get; set; } // Status extern -> F
			public string L05 { get; set; } // FA vom -> 13.02.2020
			public string L06 { get; set; } // Psz # -> Technik
			public string L07 { get; set; } // Urs.Artikel -> 
			public string L26 { get; set; } // Artikelfamilie
			public string L08 { get; set; } // -> Artikel.[Bezeichnung 1]
			public string L09 { get; set; } // Fertigung.Anzahl
			public string L10 { get; set; } // Fertigung.bemerkung
			public string L11 { get; set; } // Leader -> Josef Hauser

			public string L19 { get; set; } // Klient -> Fertigung.Mandant
			public string L18 { get; set; } // Detail1 -> Artikel.Artikelfamilie_Kunde_Detail1
			public string L20 { get; set; } // FA -> Fertigung.Fertigungsnummer

			public string L12 { get; set; }
			public string L13 { get; set; } // Ins lager -> Fertigung.Lagerort_id zubuchen 
			public string L14 { get; set; } // Termin -> Fertigung.Termin_Bestätigt1 
			public string L15 { get; set; } // Detail 2 -> Artikel_1.Artikelfamilie_Kunde_Detail2 
			public string L16 { get; set; } // min/Stk -> Fertigung.Zeit
			public string L17 { get; set; } // h/FA -> (Fertigung.Anzahl)*(Fertigung.Zeit)/60

			public string User { get; set; }
		}
		public class ItemModel
		{
			public int ProductionId { get; set; }

			public string ItemNumber { get; set; } // Artikel1.Artikelnummer
			public string RequiredQuantity { get; set; } // Anzahl -> Fertigung_Positionen.Anzahl
			public string PreparedQuantity { get; set; } // Bereit 
			public string Name1 { get; set; } // Artikel1.[Bezeichnung 1]
			public string Name2 { get; set; } // Artikel1.[Bezeichnung 2]
			public string StorageLocation { get; set; } // Lagerort -> Lagerorte_1.Lagerort
			public string Scrap { get; set; }
		}
	}
}
