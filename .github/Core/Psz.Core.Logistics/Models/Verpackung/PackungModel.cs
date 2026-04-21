using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Verpackung
{
	public class PackungModel
	{
		public List<PackingModel> listeVerpackung { get; set; }
		public List<KeyValuePair<string, string>> listeMitarbeiter { get; set; }
	}
}
