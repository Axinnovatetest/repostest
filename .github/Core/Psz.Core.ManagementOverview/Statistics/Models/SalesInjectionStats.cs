using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Statistics.Models
{
	public record SalesInjectionStatsRequestModel
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}

	public record SalesInjectionStatsResponseModel
	{
		public string Datum { get; set; }
		public string Produktionsbereich { get; set; }

		public decimal? VK { get; set; }
		public decimal? Preis { get; set; }
		public decimal? Produktionskosten { get; set; }
		public string FertigungsNummer { get; set; }
		public int? Menge { get; set; }
		public string Bezeichnung { get; set; }
		public string Ausdr { get; set; }

		public int? Originalanzahl { get; set; }
		public int? AnzahlErledigt { get; set; }
		public string ArticleNummer { get; set; }
	}
}
