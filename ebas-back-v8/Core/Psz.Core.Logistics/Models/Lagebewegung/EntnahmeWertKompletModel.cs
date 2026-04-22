using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class EntnahmeWertKompletModel
	{
		public List<EntnahmeWertTreeModel> entnahmeWetWithEK { get; set; }
		public decimal gesamtEntnahmeWetWithEK { get; set; }
		public List<EntnahmeWertTreeModel> entnahmeWetWithoutEK { get; set; }
		public decimal gesamtEntnahmeWetWithoutEK { get; set; }
	}
}
