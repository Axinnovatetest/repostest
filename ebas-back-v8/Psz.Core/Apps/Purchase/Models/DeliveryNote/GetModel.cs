using System;

namespace Psz.Core.Apps.Purchase.Models.DeliveryNote
{
	public class GetModel
	{
		public int AngebotNr { get; set; }
		public int KundenNr { get; set; }
		public string VornameFirma { get; set; }
		public string Bezug { get; set; }
		public DateTime? Datum { get; set; }
		public DateTime? Falligkeit { get; set; }
	}
}
