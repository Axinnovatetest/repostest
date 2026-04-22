using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Models
{
	public class RequestStatsModel
	{
		public List<StatsModel> StatsByStatus { get; set; }
		public List<StatsModel> StatsByPlant { get; set; }
		public List<StatsModel> StatsByCategory { get; set; }
	}
	public class StatsModel
	{
		public string Label { get; set; }
		public int Value { get; set; }
		public StatsModel(KeyValuePair<string, int> entity)
		{
			Label = entity.Key;
			Value = entity.Value;
		}
	}
}