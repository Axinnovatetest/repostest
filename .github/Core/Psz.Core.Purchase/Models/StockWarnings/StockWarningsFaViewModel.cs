
namespace Psz.Core.Purchase.Models.StockWarnings
{
	public class StockWarningsFaViewModel
	{
		public int? IdFertigung { get; set; }
		public int? Fertigungsnummer { get; set; }
		public DateTime? Termin { get; set; }
		public decimal? Quantity { get; set; }
		public StockWarningsFaViewModel()
		{

		}
		public StockWarningsFaViewModel(Infrastructure.Data.Entities.Joins.PRS.StockWarningsFaViewEntity entity)
		{
			IdFertigung = entity.IdFertigung;
			Fertigungsnummer = entity.Fertigungsnummer;
			Termin = entity.Termin;
			Quantity = entity.Quantity;
		}
	}
}