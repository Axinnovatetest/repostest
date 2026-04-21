namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetCurrencyModel
	{
		public string Currency { get; set; }
		public int IdC { get; set; }
		public string Symol { get; set; }

		public GetCurrencyModel() { }

		public GetCurrencyModel(Infrastructure.Data.Entities.Tables.STG.WahrungenEntity budget_CyrrencysEntity)
		{
			IdC = budget_CyrrencysEntity.Nr;
			Currency = budget_CyrrencysEntity.Wahrung;
			Symol = budget_CyrrencysEntity.Symbol;

		}
	}
}

