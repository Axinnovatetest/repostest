using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Analyse
{
	public class CalculateTurnoverRequestModel: Core.Common.Models.AnalyseTimePointModel
	{
		public bool Estimated { get; set; }
		public List<int> CustomersNumbers { get; set; } = new List<int>();
	}
}
