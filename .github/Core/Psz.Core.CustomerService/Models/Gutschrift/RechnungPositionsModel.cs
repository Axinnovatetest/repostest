using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Gutshrift
{
	public class RechnungPositionsModel
	{
		public int Nr { get; set; }
		public int AngebotNr { get; set; }
		public int Customernumber { get; set; }
		public string CustomerName { get; set; }
		public string DocumentNr { get; set; }
		public List<GutschriftItemModel> Items { get; set; }
	}
}
