using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class FA_NPEX_ResponseModel
	{
		public string Lager { get; set; }
		public string Kunde { get; set; }
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public List<FA_NPEX_ArticlesModel> Articles { get; set; }
	}
	public class FA_NPEX_EntryModel
	{
		public int Lager { get; set; }
		public string Kunde { get; set; }
		public int? Order { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }
		public string SearchTerms { get; set; }
	}

	public class FA_NPEX_ArticlesModel
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Freigabestatus { get; set; }
		public List<FA_NPEX_OrderModel> Orders { get; set; }
	}
	public class FA_NPEX_OrderModel
	{
		public int Fertigungsnummer { get; set; }
		public int? Anzahl { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public string Bemerkung { get; set; }
		public bool Erstmuster { get; set; }
		public Decimal? Preis { get; set; }
	}

	public class FA_NPEX_DetailsModel
	{
		public string Artikelnummer_ROH { get; set; }
		public string ROH_Description { get; set; }
		public Decimal? Qty { get; set; }
		public Decimal? Nedded_qty { get; set; }
		public Decimal? Exsisting_qty { get; set; }
	}

}
