using Psz.Core.Common.Models;
using System;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class RechnungEntryModel: IDateRangeModel
	{
		public int Lager { get; set; }
		public DateTime? RechnungsDatum { get; set; }
	}
}
