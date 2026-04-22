namespace Psz.Core.FinanceControl.Models.Currency
{
	public class CurrencyModel
	{
		public bool? EU { get; set; }
		public string Country { get; set; } // Land
		public int Id { get; set; } // Nr
		public string Symbol { get; set; }
		public string Name { get; set; } // Wahrung

		public CurrencyModel()
		{

		}
		public CurrencyModel(Infrastructure.Data.Entities.Tables.STG.WahrungenEntity wahrungenEntity)
		{
			Id = wahrungenEntity.Nr;
			EU = wahrungenEntity.EU;
			Country = wahrungenEntity.Land;
			Symbol = wahrungenEntity.Symbol;
			Name = wahrungenEntity.Wahrung;
		}
	}
}
