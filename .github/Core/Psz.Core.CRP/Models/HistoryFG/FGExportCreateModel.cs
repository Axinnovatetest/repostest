using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.HistoryFG
{
	public class FGExportCreateModel
	{
		public string? ArticleNummer { get; set; }
		public string? Kunde { get; set; }

		public string? ArticleDesignation1 { get; set; }
		public string? ArticleDesignation2 { get; set; }
		public string? Freigabestatus { get; set; }
		public string? CsContact { get; set; }
		public string? Lagerort { get; set; }

		public string? Bestand { get; set; }
		public string? VKGEsamt { get; set; }
		public string? TotalCostsWithCu { get; set; }
		public string? TotalCostsWithoutCu { get; set; }
		public string? VKE { get; set; }
		public string? UBG { get; set; }
		public string? StdEdi { get; set; }
		public string? UnitSalesPrice { get; set; }
		public DateTime? Datum { get; set; }


	}
}
