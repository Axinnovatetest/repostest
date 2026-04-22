using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Statistics.Models
{
	public record ReasonChangeCommitteeRequestModel
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public string ArticleNumber { get; set; }
		public int LagerId { get; set; }
	}

	public record ReasonChangeCommitteeResponseModel
	{
		public string Typ { get; set; }
		public string Datum { get; set; }
		public decimal? Anzahl { get; set; }
		public int? LagerId { get; set; }
		public int Id { get; set; }
		public int ArtikelNr { get; set; }
		public string Grund { get; set; }
		public string ArticleNumber { get; set; }

	}
}
