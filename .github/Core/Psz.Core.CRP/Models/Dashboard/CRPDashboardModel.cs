using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.Dashboard
{
	public class CRPDashboardStatisticsModel
	{
		public int CreatedFas { get; set; }
		public int CancelledFas { get; set; }
	}
	public class CRPDashboardKenzahllenModel
	{
		public int OpenFasByYear { get; set; }
		public int ActiveArticlesByYear { get; set; }
		public decimal OpenFasHours { get; set; }
	}
	public class CRPDashboardRequestModel
	{
		public int Month { get; set; }
		public int Year { get; set; }
	}
}