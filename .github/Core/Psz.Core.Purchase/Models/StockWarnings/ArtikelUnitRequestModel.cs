

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class ArtikelUnitRequestModel
	{
		public int ArtikelNr { get; set; }
		public int Unit { get; set; }
		public int? Year { get; set; }
		public int? Week { get; set; }
		public bool Backlog { get; set; }
	}
}
