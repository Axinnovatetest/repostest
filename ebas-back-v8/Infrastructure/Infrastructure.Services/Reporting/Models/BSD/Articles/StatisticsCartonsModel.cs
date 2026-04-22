using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.BSD.Articles
{
	public class StatisticsCartonsModel
	{
		public List<Header> Headers { get; set; }
		public List<Item> Items { get; set; }

		public class Header
		{
			public string From { get; set; }
			public string To { get; set; }
			public string Lagerort { get; set; }
		}
		public class Item
		{
			public string Anzahl { get; set; }
			public string Artikelnummer_Umlauf { get; set; }
			public string Bestand { get; set; }
			public string Bestatigter_Termin { get; set; }
			public string Bestellung_Nr { get; set; }
			public string Liefertermin { get; set; }
			public string SummevonBedarf { get; set; }
			public string Transfer_Bestand { get; set; }
		}
	}
}
