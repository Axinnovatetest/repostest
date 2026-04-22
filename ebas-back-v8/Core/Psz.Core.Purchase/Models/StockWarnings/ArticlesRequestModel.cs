using Psz.Core.Common.Models;

namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class ArticlesRequestModel: IPaginatedRequestModel
	{
		public int? countryId { get; set; }
		public int? UnitId { get; set; }
		public int? Prio { get; set; }
		public string Artikelnummer { get; set; }
	}
	public class ArticlesResponseModel: IPaginatedResponseModel<KeyValuePair<int, string>> { }
}