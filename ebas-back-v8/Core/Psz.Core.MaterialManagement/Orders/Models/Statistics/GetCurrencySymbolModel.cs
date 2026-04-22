namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class GetCurrencySymbolModel
	{

		public string Symbol { get; set; } //
		public int Nr { get; set; } //

		public GetCurrencySymbolModel(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.DispowsDetailsCurrencyEntity data)
		{
			Symbol = data.Symbol ?? string.Empty;
			Nr = data.Nr;
		}
	}
}
