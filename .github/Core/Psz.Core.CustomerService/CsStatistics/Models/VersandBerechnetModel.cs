using System;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class VersandBerechnetResponseModel
	{
		public string Typ { get; set; }
		public string Artikelnummer { get; set; }
		public Decimal? SummevonGesamtpreis { get; set; }
	}
}
