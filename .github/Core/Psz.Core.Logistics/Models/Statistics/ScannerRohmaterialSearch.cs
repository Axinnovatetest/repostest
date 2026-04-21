using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class ScannerRohmaterialSearch
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public string SearchValue { get; set; }
		public int? SearchLager { get; set; }
	}
}
