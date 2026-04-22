using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Models.HistoryFG
{
	public class FgHistoryBestandXlsStatisticsModel
	{
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
		public int? AdressCustomerNumber { get; set; }
	}
}
