using Psz.Core.Common.Models;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class KapzitatkurzEntryModel: IDateRangeNullableModel
	{
		public int? ProdTage { get; set; }
		public int Lager { get; set; }
	}
}
