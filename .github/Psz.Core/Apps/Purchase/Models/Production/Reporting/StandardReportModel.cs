using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Production.Reporting
{
	public class StandardReportModel
	{
		public List<ProductionModel> Productions { get; set; } = new List<ProductionModel>();
		public List<ItemModel> Items { get; set; } = new List<ItemModel>();


		public class ProductionModel
		{
			public int Id { get; set; }

			public string L01 { get; set; } // -> ERSTMUSTERAUFTRAG
			public string L02 { get; set; } // -> D-2002247
			public string L03 { get; set; } // -> 0
			public string L04 { get; set; } // FA vom -> 13.02.2020
			public string L05 { get; set; } // klient -> PSZ electronic
			public string L06 { get; set; }

			public string L07 { get; set; } // Psz #:
			public string L08 { get; set; } // Bez. 1 -> 47-15-000-tbd - 1731708 - Klappe Zusatzplatine Regelung CGB2 75/100
			public string L09 { get; set; } // Artikelfamilie
			public string L10 { get; set; } // -> 01
			public string L11 { get; set; } // Menge -> 5
			public string L12 { get; set; } // FA-Bem: fertigung D, Wolf GmbH: 411 / 47371616, Erstmuster inkl. EMPB, --
			public string L13 { get; set; } // Techniker -> Josef Hauser
			public string L14 { get; set; } // Details 1 -> 

			public string L15 { get; set; } // Termin -> 14.04.2020
			public string L16 { get; set; } // Details 2 -> 
			public string L17 { get; set; } // min / Stk -> 240
			public string L18 { get; set; } // h / FA -> 20.0
			public string L19 { get; set; } // Stav extern -> N
			public string L20 { get; set; } // Stav intern -> N

			public string L21 { get; set; } // -> Fertigung F-2002247
			public string L22 { get; set; } // Verp. ->
			public string L23 { get; set; } // Verp.-Menge -> 1
			public string L24 { get; set; } // LG -> 1

			public string Date { get; set; }
			public string User { get; set; }

			public string R01 { get; set; }
			public string R02 { get; set; }
			public string R03 { get; set; }
			public string R04 { get; set; }
			public string R05 { get; set; }
			public string R06 { get; set; }

			public string S01 { get; set; }
			public string S02 { get; set; }
			public string S03 { get; set; }
			public string S04 { get; set; }
			public string S05 { get; set; }
		}
		public class ItemModel
		{
			public int ProductionId { get; set; }

			public string ItemNumber { get; set; } // Artikelnummer -> Artikel1.Artikelnummer
			public string Requirement { get; set; } // bedarf -> 
			public string Prepared { get; set; } // Bereit ->  Fertigung_Positionen.Anzahl
			public string Name { get; set; } // Bezeichnung -> Artikel1.[Bezeichnung 1]
			public string StorageLocation { get; set; } // Lagerort -> Lagerorte_1.Lagerort
			public string Odpad { get; set; } // Odpad/..B ->
		}

		public class Gewerk
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
		public class GewerkItem
		{
			public int GewerkId { get; set; }
			public string Name { get; set; }
		}
	}
}
