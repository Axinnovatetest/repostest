using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Analyse
{
	public class FilterProcessorResponseModel
	{
		public List<int> Quartals { get; set; }
		public List<KeyValuePair<string, int>> Months { get; set; }

		public List<int> Weeks { get; set; }

		public FilterProcessorResponseModel()
		{
		}

		public FilterProcessorResponseModel(List<int> quartals, List<KeyValuePair<string, int>> months, List<int> weeks)
		{
			Quartals = quartals;
			Months = months;
			Weeks = weeks;
		}
	}
}
