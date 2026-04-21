namespace Psz.Core.ManagementOverview.Statistics.Models
{
	public record GetSalesRequestModel
	{
		public int? Year { get; set; }
	}

	public record GetSalesResponseModel
	{
		public int Year { get; set; }
		public int KW { get; set; }
		public decimal? InvoiceAmount { get; set; }
		public decimal? StockFGWoUBGAmount { get; set; }
		public decimal? StockFGUBGAmount { get; set; }
		public decimal? StockROHAmount { get; set; }
		public decimal? OrderAmount { get; set; }
		public decimal? ProductionOrderFinishedAmount { get; set; }
		public decimal? ProductionOrderFinishedHours { get; set; }
		public decimal? ProductionOrderPlannedAmount { get; set; }
		public decimal? ProductionOrderPlannedHours { get; set; }
	}
}
