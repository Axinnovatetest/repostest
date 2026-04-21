using System;

namespace Psz.Core.BaseData.Models.Article
{
	public class UpdateCustomerIndexRequestModel
	{
		public int OriginalArticleId { get; set; }
		public string NewCustomerIndex { get; set; }
		public DateTime? NewCustomerIndexDate { get; set; }
		public bool IsNewProduction { get; set; } = false;
		public bool WithBOM { get; set; } = true;
		public int ProductionCountryId { get; set; }
		public string ProductionCountryName { get; set; }
		public int ProductionSiteId { get; set; }
		public string ProductionSiteName { get; set; }
		public string ProductionSiteCode { get; set; }
		// -
		public string Reason { get; set; }
		// - 2023-01-22
		public bool IsEdiDefault { get; set; } = false;
	}
}
