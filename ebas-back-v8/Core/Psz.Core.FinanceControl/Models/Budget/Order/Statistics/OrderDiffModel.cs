using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Budget.Order.Statistics
{
	// 08-02-2024 Personalize

	public class OrderDiffModel
	{
		public int OrderId { get; set; }

		public string OrderNum { get; set; }

		public TimeSpan Difference { get; set; }

		public DateTime ValidationRequestTime { get; set; }

		public DateTime ValidationTime { get; set; }

		public string Date_Time { get; set; }

		public OrderDiffModel()
		{

		}

		public string ConvertToYearsMonthsDays(TimeSpan timeSpan)
		{
			int days = (int)timeSpan.TotalDays;

			int years = days / 365;
			int months = (days % 365) / 30; // Approximation, 30 days per month
			int remainingDays = (days % 365) % 30;

			return $"{years} années {months} mois {remainingDays} jours";
		}
	}

	// added  12-02-2024

	// model that contains Sub-models.
	public class FastestAndCheapestModel
	{

		public List<OrderDiffModel> TopFiveCheapest { get; set; }
		public List<OrderDiffModel> TopFiveFastest { get; set; }

	}

	public class FastestResponseModel
	{
		public List<OrderDiffModel> TopFiveFastest { get; set; }
	}

	public class CheapestResponseModel
	{

		public List<OrderDiffModel> TopFiveCheapest { get; set; }


	}
}
