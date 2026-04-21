using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class PrioEinkaufRequestModel
	{
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SortFieldKey { get; set; }
		public bool SortDesc { get; set; }
		public string SearchTerms { get; set; }

	}
	public class PrioEinkaufResponseModel
	{
		public string Title { get; set; }
		public string ReportDate { get; set; } = DateTime.Now.ToString("D", new System.Globalization.CultureInfo("de-DE"));
		public List<PrioEinkaufResponseData.Supplier> Suppliers { get; set; }
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

	}

	public class PrioEinkaufResponseData
	{
		public string Title { get; set; }
		public string ReportDate { get; set; } = DateTime.Now.ToString("D", new System.Globalization.CultureInfo("de-DE"));
		public List<Supplier> Suppliers { get; set; }

		// -
		public class Supplier
		{
			public string Name1 { get; set; }
			public string Telefon { get; set; }
			public string Fax { get; set; }
			public List<Item> Items { get; set; }
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
			public string Bestatigter_Termin { get; set; }
			public string Name1 { get; set; }
		}
	}
}
