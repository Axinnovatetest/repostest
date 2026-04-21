using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class UmbuchungslisteSearch
	{
		public string Lager { get; set; }
		public string Lieferant { get; set; }
		public DateTime bis { get; set; }
		public string withFG { get; set; }
		public string withoutFG { get; set; }
		public string Stucklisten_Artikelnummer { get; set; }



	}
}
