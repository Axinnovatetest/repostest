using System;
using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.BSD.Articles
{
	public class StatisticsPszPrio1DataModel
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
			public string Liefertermin { get; set; }
			public string Name1 { get; set; }
		}
	}
}
