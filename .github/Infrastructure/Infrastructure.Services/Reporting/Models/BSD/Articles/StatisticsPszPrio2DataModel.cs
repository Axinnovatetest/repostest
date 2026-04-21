using System;
using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.BSD.Articles
{
	public class StatisticsPszPrio2DataModel
	{
		public List<Header> ReportData { get; set; }
		public List<Supplier> Suppliers { get; set; }
		public List<Item> Items { get; set; }

		// -
		public class Header
		{
			public string Title { get; set; }
			public string ReportDate { get; set; } = DateTime.Now.ToString("D", new System.Globalization.CultureInfo("de-DE"));
		}
		public class Supplier
		{
			public string Name1 { get; set; }
			public string Telefon { get; set; }
			public string Fax { get; set; }
		}
		public class Item
		{
			public string Datum { get; set; }
			public string Bestellung_Nr { get; set; }
			public string Lagerort_id { get; set; }
			public string Anzahl { get; set; }
			public string Artikelnummer { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bestatigter_Termin { get; set; }
			public string Liefertermin { get; set; }
			public string Name1 { get; set; }
		}
	}

	public class StatisticsProjectMessage
	{
		public List<Header> ReportData { get; set; }
		public List<Item> Items { get; set; }

		// -
		public class Header
		{
			public string Title { get; set; }
			public string ReportDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
		}
		public class Item
		{
			public string AB_Datum { get; set; }
			public int Arbeitszeit_Serien_Pro_Kabesatz { get; set; }
			public string Artikelnummer { get; set; }
			public string Bemerkungen { get; set; }
			public int EAU { get; set; }
			public string EMPB { get; set; }
			public DateTime Erstanlage { get; set; }
			public string FA_Datum { get; set; }
			public int ID { get; set; }
			public string Kontakt_AV_PSZ { get; set; }
			public string Kontakt_CS_PSZ { get; set; }
			public string Kontakt_Technik_Kunde { get; set; }
			public string Kontakt_Technik_PSZ { get; set; }
			public int Kosten { get; set; }
			public string Krimp_WKZ { get; set; }
			public string Material_Eskalation_AV { get; set; }
			public string Material_Eskalation_Termin { get; set; }
			public string Material_Komplett { get; set; }
			public int Menge { get; set; }
			public int MOQ { get; set; }
			public string Projekt_betreung { get; set; }
			public string Projekt_Start { get; set; }
			public bool Projektmeldung { get; set; }
			public string Projekt_Nr { get; set; }
			public string Rapid_Prototyp { get; set; }
			public string Serie_PSZ { get; set; }
			public string SG_WKZ { get; set; }
			public string Standort_Muster { get; set; }
			public string Standort_Serie { get; set; }
			public string Summe_Arbeitszeit { get; set; }
			public string Termin_mit_Technik_abgesprochen { get; set; }
			public string TSP_Kunden { get; set; }
			public string Typ { get; set; }
			public string UL_Verpackung { get; set; }
			public string Wunschtermin_Kunde { get; set; }
			public string Zuschlag { get; set; }

			// 
			public string Kunde { get; set; }
			public string Kundenschlussel { get; set; }
		}
	}
}
