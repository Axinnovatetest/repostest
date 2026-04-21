using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class BestAndWorstCustomersStatsModel
	{
		public IEnumerable<BestProfitModel> BestProfits { get; set; }
		public IEnumerable<BestProfitModel> WorstProfits { get; set; }
		public IEnumerable<BestPszOffermodel> BestPszOffer { get; set; }
		public IEnumerable<BestPszOffermodel> WorstPszOffer { get; set; }
	}
	public class BestProfitModel
	{
		public string CustomerName { get; set; }
		public decimal Profit { get; set; }
		public BestProfitModel(KeyValuePair<string, decimal> entity)
		{
			CustomerName = entity.Key;
			Profit = entity.Value;
		}
	}
	public class BestPszOffermodel
	{
		public string CustomerName { get; set; }
		public decimal PSZOffer { get; set; }
		public BestPszOffermodel(KeyValuePair<string, decimal> entity)
		{
			CustomerName = entity.Key;
			PSZOffer = entity.Value;
		}
	}
}